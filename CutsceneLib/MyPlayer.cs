using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Net;
using CutsceneLib.Definitions;
using CutsceneLib.Logic;


namespace CutsceneLib {
	public partial class CutsceneLibPlayer : ModPlayer {
		internal ISet<CutsceneID> TriggeredCutsceneIDs_Player = new HashSet<CutsceneID>();


		////////////////

		public bool IsPlayerCutsceneLibCompat { get; internal set; } = false;

		////

		public override bool CloneNewInstances => false;



		////////////////

		public override void clientClone( ModPlayer clientClone ) {
			var myclone = clientClone as CutsceneLibPlayer;
			myclone.TriggeredCutsceneIDs_Player = new HashSet<CutsceneID>( this.TriggeredCutsceneIDs_Player );
			myclone.IsPlayerCutsceneLibCompat = this.IsPlayerCutsceneLibCompat;
		}


		////////////////

		public override void Load( TagCompound tag ) {
			if( tag.ContainsKey( "IsPlayerCutsceneLibCompat" ) ) {
				this.IsPlayerCutsceneLibCompat = true;
				CutsceneManager.Instance?.Load_Player( this, tag );
			}
		}

		public override TagCompound Save() {
			var tag = new TagCompound {
				{ "IsPlayerCutsceneLibCompat", this.IsPlayerCutsceneLibCompat },
			};
			CutsceneManager.Instance.Save_Player( this, tag );
			return tag;
		}


		////////////////
		
		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			if( Main.netMode == NetmodeID.Server ) {
				PlayerDataNetData.SendToClients( this, toWho, fromWho );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				PlayerDataNetData.SendToServer( this );
			}
		}

		public override void SendClientChanges( ModPlayer clientPlayer ) {
			var myclone = clientPlayer as CutsceneLibPlayer;
			bool isDesynced = myclone.IsPlayerCutsceneLibCompat != this.IsPlayerCutsceneLibCompat
				|| !this.TriggeredCutsceneIDs_Player.SetEquals( myclone.TriggeredCutsceneIDs_Player );

			if( isDesynced ) {
				if( Main.netMode == NetmodeID.Server ) {
					PlayerDataNetData.SendToClients( this, -1, this.player.whoAmI );
				} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
					PlayerDataNetData.SendToServer( this );	// server only?
				}
			}
		}

		/////

		internal void SyncFromNet( PlayerDataNetData payload ) {
			this.IsPlayerCutsceneLibCompat = payload.IsPlayerCutsceneLibCompat;
			this.TriggeredCutsceneIDs_Player = new HashSet<CutsceneID>();

			int len = payload.ActivatedCutsceneModNames_Player.Length;
			for( int i=0; i<len; i++ ) {
				string modName = payload.ActivatedCutsceneModNames_Player[i];
				string className = payload.ActivatedCutsceneNames_Player[i];

				this.TriggeredCutsceneIDs_Player.Add( new CutsceneID(modName, className) );
			}
		}


		////////////////

		public override void PreUpdate() {
			CutsceneManager.Instance.Update_Player_Internal( this );
		}
	}
}