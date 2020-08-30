using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;
using CutsceneLib.Logic;
using CutsceneLib.Definitions;


namespace CutsceneLib.Net {
	[Serializable]
	class PlayerDataNetData : NetIOBidirectionalPayload {
		public static void SendToServer( CutsceneLibPlayer plrData ) {
			var protocol = new PlayerDataNetData( plrData );

			NetIO.SendToServer( protocol );
		}

		public static void SendToClients( CutsceneLibPlayer plrData, int toWho, int ignoreWho ) {
			var protocol = new PlayerDataNetData( plrData );

			NetIO.SendToClients( protocol, toWho, ignoreWho );
		}



		////////////////

		public int FromWho;

		public string[] CurrentCutsceneModNames_World;
		public string[] CurrentCutsceneNames_World;
		public string[] ActivatedCutsceneModNames_World;
		public string[] ActivatedCutsceneNames_World;
		public string[] ActivatedCutsceneModNames_Player;
		public string[] ActivatedCutsceneNames_Player;



		////////////////

		private PlayerDataNetData() { }
		
		private PlayerDataNetData( CutsceneLibPlayer myplayer ) {
			var myworld = ModContent.GetInstance<CutsceneLibWorld>();
			var cutMngr = CutsceneManager.Instance;
			IEnumerable<Cutscene> activeCutscenes = cutMngr.GetActiveCutscenes_World();

			this.FromWho = myplayer.player.whoAmI;

			this.CurrentCutsceneModNames_World = new string[ activeCutscenes.Count() ];
			this.CurrentCutsceneNames_World = new string[ activeCutscenes.Count() ];
			this.ActivatedCutsceneModNames_World = new string[ myworld.TriggeredCutsceneIDs_World.Count() ];
			this.ActivatedCutsceneNames_World = new string[ myworld.TriggeredCutsceneIDs_World.Count() ];
			this.ActivatedCutsceneModNames_Player = new string[ myplayer.TriggeredCutsceneIDs_Player.Count() ];
			this.ActivatedCutsceneNames_Player = new string[ myplayer.TriggeredCutsceneIDs_Player.Count() ];

			int i = 0;
			foreach( Cutscene c in activeCutscenes ) {
				this.CurrentCutsceneModNames_World[i] = c.UniqueId.ModName;
				this.CurrentCutsceneNames_World[i] = c.UniqueId.FullClassName;
				i++;
			}

			i = 0;
			foreach( CutsceneID cid in myworld.TriggeredCutsceneIDs_World ) {
				this.ActivatedCutsceneModNames_World[i] = cid.ModName;
				this.ActivatedCutsceneNames_World[i] = cid.FullClassName;
				i++;
			}

			i = 0;
			foreach( CutsceneID cid in myplayer.TriggeredCutsceneIDs_Player ) {
				this.ActivatedCutsceneModNames_Player[i] = cid.ModName;
				this.ActivatedCutsceneNames_Player[i] = cid.FullClassName;
				i++;
			}
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			var myplr = Main.player[this.FromWho].GetModPlayer<CutsceneLibPlayer>();
			myplr.SyncFromNet( this );
		}

		public override void ReceiveOnClient() {
			var myplr = Main.player[this.FromWho].GetModPlayer<CutsceneLibPlayer>();
			myplr.SyncFromNet( this );
		}
	}
}
