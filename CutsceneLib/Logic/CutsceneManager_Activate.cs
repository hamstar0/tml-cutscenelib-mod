using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using CutsceneLib.Net;
using CutsceneLib.Definitions;


namespace CutsceneLib.Logic {
	public partial class CutsceneManager : ILoadable {
		public bool CanBeginCutscene( bool isAutoplay, CutsceneID cutsceneId, Player playsFor, out string result ) {
			return this.CanBeginCutscene( isAutoplay, cutsceneId, playsFor, out Cutscene _, out result );
		}

		////

		private bool CanBeginCutscene(
					bool isAutoplay,
					CutsceneID cutsceneId,
					Player playsFor,
					out Cutscene cutscene,
					out string result ) {
			if( this.GetCurrentCutscene_Player( playsFor ) != null ) {
				cutscene = null;
				result = "Player "+playsFor.name+" ("+playsFor.whoAmI+") is already playing "+cutsceneId;
				return false;
			}

			cutscene = cutsceneId.Create( playsFor );
			
			if( isAutoplay && !cutscene.CanAutoplay ) {
				result = "Cutscene " + cutsceneId + " does not support autoplay.";
				return false;
			}
			if( !cutscene.CanReplayPerWorld && this.HasCutscenePlayed_World( cutsceneId ) ) {
				cutscene = null;
				result = "World has already played " + playsFor.name + " (" + playsFor.whoAmI + ")'s cutscene " + cutsceneId;
				return false;
			}
			if( !cutscene.CanReplayPerPlayer && this.HasCutscenePlayed_Player( cutsceneId, playsFor ) ) {
				cutscene = null;
				result = "Player has already played " + playsFor.name + " (" + playsFor.whoAmI + ")'s cutscene " + cutsceneId;
				return false;
			}

			return cutscene.CanBegin( out result );
		}


		////////////////

		public bool TryBeginCutscene(
					bool isAutoplay,
					CutsceneID cutsceneId,
					Player playsFor,
					bool sync,
					out string result ) {
			if( !this.CanBeginCutscene(isAutoplay, cutsceneId, playsFor, out Cutscene cutscene, out result) ) {
				result = "Cannot play cutscene "+cutsceneId+": "+result;
				return false;
			}

			return this.TryBeginCutscene( cutscene, cutscene.FirstSceneId, sync, out result );
		}
		
		/*public bool TryBeginCutscene(
					CutsceneID cutsceneId,
					SceneID sceneId,
					Player playsFor,
					bool sync,
					out string result ) {
			if( !this.CanBeginCutscene(cutsceneId, playsFor, out Cutscene cutscene, out result) ) {
				result = "Cannot play cutscene " + cutsceneId + ": " + result;
				return false;
			}
			return this.TryBeginCutscene( cutscene, sceneId, sync, out result );
		}*/

		////

		private bool TryBeginCutscene(
					Cutscene cutscene,
					SceneID sceneId,
					bool sync, 
					out string result ) {
			int playsForWhom = cutscene.PlaysForWhom;
			Player playsFor = Main.player[playsForWhom];

			if( this.GetCurrentCutscene_Player(playsFor) != null ) {
				result = playsFor.name+" ("+playsForWhom+") already playing cutscene "+cutscene.UniqueId;
				return false;
			}
			
			cutscene.BeginCutscene_Internal( sceneId );

			this._CutscenePerPlayer[ playsForWhom ] = cutscene;

			var myplayer = playsFor.GetModPlayer<CutsceneLibPlayer>();
			myplayer.TriggeredCutsceneIDs_Player.Add( cutscene.UniqueId );

			var myworld = ModContent.GetInstance<CutsceneLibWorld>();
			myworld.TriggeredCutsceneIDs_World.Add( cutscene.UniqueId );

			if( sync ) {
				if( Main.netMode == NetmodeID.Server ) {
					CutsceneNetStart.SendToClients( cutscene: cutscene, ignoreWho: -1 );
				} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
					CutsceneNetStart.Broadcast( cutscene: cutscene );
				}
			}

			result = "Success.";
			return true;
		}

		////

		public void TryBeginCutsceneFromNetwork(
					CutsceneID cutsceneId,
					SceneID sceneId,
					Player playsFor,
					CutsceneNetStart data,
					Action<string> onSuccess,
					Action<string> onFail ) {
			if( !this.CanBeginCutscene(false, cutsceneId, playsFor, out Cutscene cutscene, out string result) ) {
				onFail( "Cannot play cutscene " + cutsceneId + ": " + result );
				return;
			}

			if( this.CutsceneInWaitingPerClient.ContainsKey(playsFor.whoAmI) ) {
				cutscene = this.CutsceneInWaitingPerClient[ playsFor.whoAmI ];
			}
			this.CutsceneInWaitingPerClient[ playsFor.whoAmI ] = cutscene;

			//

			void onMySuccess( string myResult ) {
				this._CutscenePerPlayer[ playsFor.whoAmI ] = cutscene;
				this.CutsceneInWaitingPerClient.Remove( playsFor.whoAmI );	// just in case

				var myplayer = playsFor.GetModPlayer<CutsceneLibPlayer>();
				myplayer.TriggeredCutsceneIDs_Player.Add( cutsceneId );

				var myworld = ModContent.GetInstance<CutsceneLibWorld>();
				myworld.TriggeredCutsceneIDs_World.Add( cutsceneId );

				onSuccess( myResult );
			}

			void onMyFail( string myResult ) {
				this.EndCutscene( cutscene, playsFor.whoAmI, false );

				onFail( myResult );
			}

			//

			cutscene.BeginCutsceneFromNetwork_Internal( sceneId, data, onMySuccess, onMyFail );
		}


		////////////////

		public bool SetCutsceneScene( CutsceneID cutsceneId, Player playsFor, SceneID sceneId, bool sync ) {
			Cutscene cutscene = this._CutscenePerPlayer.GetOrDefault( playsFor.whoAmI );
			if( cutscene == null ) {
				return false;
			}
			if( cutscene.UniqueId != cutsceneId ) {
				return false;
			}

			cutscene.SetCurrentScene( sceneId, sync );
			return true;
		}


		////////////////

		public bool EndCutscene( CutsceneID cutsceneId, int playsForWhom, bool sync ) {
			Cutscene cutscene = this._CutscenePerPlayer.GetOrDefault( playsForWhom );
			if( cutscene == null ) {
				return false;
			}
			if( cutscene.UniqueId != cutsceneId ) {
				return false;
			}

			return this.EndCutscene( cutscene, playsForWhom, sync );
		}

		private bool EndCutscene( Cutscene cutscene, int playsForWhom, bool sync ) {
			cutscene.EndCutscene_Internal();

			this._CutscenePerPlayer.Remove( playsForWhom );
			this.CutsceneInWaitingPerClient.Remove( playsForWhom );

			if( sync ) {
				if( Main.netMode == NetmodeID.Server ) {
					CutsceneNetEnd.SendToClients( cutscene: cutscene, ignoreWho: -1 );
				} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
					CutsceneNetEnd.Broadcast( cutscene: cutscene );
				}
			}
			return true;
		}
	}
}
