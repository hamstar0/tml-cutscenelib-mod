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
	public abstract class CutsceneNetStart : NetIOBroadcastPayload {
		public static void Broadcast( Cutscene cutscene ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}

			CutsceneNetStart protocol = cutscene.CreatePacketPayload();

			NetIO.Broadcast( protocol );
		}

		public static void SendToClients( Cutscene cutscene, int ignoreWho ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}

			CutsceneNetStart protocol = cutscene.CreatePacketPayload();

			NetIO.SendToClients(
				data: protocol,
				ignoreWho: ignoreWho
			);
		}



		////////////////

		public int PlaysForWho = -1;
		public string CutsceneModName = null;
		public string CutsceneClassFullName = null;
		public string SceneModName = null;
		public string SceneClassFullName = null;



		////////////////

		protected CutsceneNetStart() { }

		protected CutsceneNetStart( Cutscene cutscene ) {
			this.PlaysForWho = cutscene.PlaysForWhom;
			this.CutsceneModName = cutscene.UniqueId.ModName;
			this.CutsceneClassFullName = cutscene.UniqueId.FullClassName;
			this.SceneModName = cutscene.CurrentScene.UniqueId.ModName;
			this.SceneClassFullName = cutscene.CurrentScene.UniqueId.FullClassName;
/*LogHelpers.Log( "SEND "
	+"PlaysForWho:"+this.PlaysForWho
	+ ", CutsceneModName:" + this.CutsceneModName
	+ ", CutsceneClassFullName:" + this.CutsceneClassFullName
	+ ", SceneModName:" + this.SceneModName
	+ ", SceneClassFullName:" + this.SceneClassFullName );*/
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
/*LogHelpers.Log( "RECEIVE "
	+"PlaysForWho:"+this.PlaysForWho
	+ ", CutsceneModName:" + this.CutsceneModName
	+ ", CutsceneClassFullName:" + this.CutsceneClassFullName
	+ ", SceneModName:" + this.SceneModName
	+ ", SceneClassFullName:" + this.SceneClassFullName );*/
			var mngr = CutsceneManager.Instance;
			var cutsceneId = new CutsceneID( this.CutsceneModName, this.CutsceneClassFullName );

			Player playsFor = Main.player[ this.PlaysForWho ];
			if( playsFor?.active != true ) {
				LogHelpers.Warn( "Missing player #"+this.PlaysForWho );
				return;
			}

			if( !mngr.CanBeginCutscene(false, cutsceneId, playsFor, out string result) ) {
				LogHelpers.Warn( "Cannot play cutscene " + cutsceneId + ": "+result );
				return;
			}

			SceneID sceneId = new SceneID( this.SceneModName, this.SceneClassFullName );
			if( !this.PreReceive(cutsceneId, sceneId) ) {
				return;
			}

			//

			void onSuccess( string myResult ) {
				LogHelpers.Log( "Beginning cutscene " + cutsceneId + " result for client: " + myResult );
			}
			void onFail( string myResult ) {
				LogHelpers.Warn( "Cannot play cutscene " + cutsceneId + ": " + myResult );
				//if( !mngr.SetCutsceneScene( cutsceneId, playsFor, sceneId, false ) ) { }
			}

			//

			mngr.TryBeginCutsceneFromNetwork( cutsceneId, sceneId, playsFor, this, onSuccess, onFail );

			this.PostReceive();
		}


		////

		protected virtual bool PreReceive( CutsceneID cutsceneId, SceneID sceneId ) {
			return true;
		}

		protected virtual void PostReceive() {
		}
	}
}
