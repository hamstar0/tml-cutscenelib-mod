using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network.NetIO;
using CutsceneLib.Logic;
using CutsceneLib.Definitions;


namespace CutsceneLib.Net {
	[Serializable]
	public abstract class CutsceneUpdateProtocol : CutsceneSceneDataProtocol {
		public static void Broadcast( Cutscene cutscene ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}

			CutsceneStartProtocol protocol = cutscene.CreatePacketPayload();

			NetIO.Broadcast( protocol );
		}

		public static void SendToClients( Cutscene cutscene, int ignoreWho ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}

			CutsceneStartProtocol protocol = cutscene.CreatePacketPayload();

			NetIO.SendToClients(
				data: protocol,
				ignoreWho: ignoreWho
			);
		}



		////////////////

		protected CutsceneUpdateProtocol() : base() { }

		protected CutsceneUpdateProtocol( Cutscene cutscene, MovieSet set ) : base( cutscene, set ) { }


		////////////////

		protected sealed override void Receive() {
			var mngr = CutsceneManager.Instance;
			var cutsceneId = new CutsceneID( this.CutsceneModName, this.CutsceneClassFullName );

			Player playsFor = Main.player[ this.PlaysForWho ];
			if( playsFor?.active != true ) {
				LogHelpers.Warn( "Missing player #"+this.PlaysForWho );
				return;
			}

			var cutscene = mngr.GetCurrentCutscene_Player( playsFor );
			if( cutscene == null ) {
				LogHelpers.Warn( "No cutscene "+cutsceneId+" playing "+playsFor.name+" ("+this.PlaysForWho+")" );
				return;
			}

			SceneID sceneId = new SceneID( this.SceneModName, this.SceneClassFullName );
			mngr.SetCutsceneSceneFromNetwork( cutsceneId, playsFor, sceneId, this, false );
		}
	}
}
