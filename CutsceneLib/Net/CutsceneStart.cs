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
	public abstract class CutsceneStartProtocol : CutsceneSceneDataProtocol {
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

		protected CutsceneStartProtocol() : base() { }

		protected CutsceneStartProtocol( Cutscene cutscene, MovieSet set ) : base( cutscene, set ) {
/*LogHelpers.Log( "SEND "
	+"PlaysForWho:"+this.PlaysForWho
	+ ", CutsceneModName:" + this.CutsceneModName
	+ ", CutsceneClassFullName:" + this.CutsceneClassFullName
	+ ", SceneModName:" + this.SceneModName
	+ ", SceneClassFullName:" + this.SceneClassFullName );*/
		}


		////////////////

		protected sealed override void Receive() {
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
