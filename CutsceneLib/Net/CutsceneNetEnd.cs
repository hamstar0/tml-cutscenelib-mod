using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;
using CutsceneLib.Logic;
using CutsceneLib.Definitions;


namespace CutsceneLib.Net {
	[Serializable]
	public class CutsceneNetEnd : NetIOBroadcastPayload {
		public static void Broadcast( Cutscene cutscene ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException("Not client");
			}

			var protocol = new CutsceneNetEnd( cutscene );

			NetIO.Broadcast( protocol );
		}

		public static void SendToClients( Cutscene cutscene, int ignoreWho ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}

			var protocol = new CutsceneNetEnd( cutscene );

			NetIO.SendToClients(
				data: protocol,
				ignoreWho: ignoreWho
			);
		}



		////////////////

		public int PlaysForWho = -1;
		public string CutsceneModName = null;
		public string CutsceneClassFullName = null;



		////////////////

		protected CutsceneNetEnd() { }

		protected CutsceneNetEnd( Cutscene cutscene ) {
			this.PlaysForWho = cutscene.PlaysForWhom;
			this.CutsceneModName = cutscene.UniqueId.ModName;
			this.CutsceneClassFullName = cutscene.UniqueId.FullClassName;
		}


		////////////////

		public override bool ReceiveOnServerBeforeRebroadcast( int fromWho ) {
			this.Receive();
			return true;
		}

		public override void ReceiveBroadcastOnClient() {
			this.Receive();
		}

		////

		private void Receive() {
			var mngr = CutsceneManager.Instance;
			var cutsceneId = new CutsceneID( this.CutsceneModName, this.CutsceneClassFullName );

			if( mngr.EndCutscene(cutsceneId, this.PlaysForWho, false) ) {
				LogHelpers.Warn( "Cannot end cutscene "+cutsceneId+"." );
			}
		}
	}
}
