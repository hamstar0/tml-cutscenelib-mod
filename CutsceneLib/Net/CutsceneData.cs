using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;
using CutsceneLib.Definitions;


namespace CutsceneLib.Net {
	[Serializable]
	public abstract class CutsceneSceneDataProtocol : NetIOBroadcastPayload {
		public int PlaysForWho = -1;
		public string CutsceneModName = null;
		public string CutsceneClassFullName = null;
		public string SceneModName = null;
		public string SceneClassFullName = null;



		////////////////

		protected CutsceneSceneDataProtocol() { }

		protected CutsceneSceneDataProtocol( Cutscene cutscene, MovieSet set ) {
			this.PlaysForWho = cutscene.PlaysForWhom;
			this.CutsceneModName = cutscene.UniqueId.ModName;
			this.CutsceneClassFullName = cutscene.UniqueId.FullClassName;
			this.SceneModName = cutscene.CurrentScene.UniqueId.ModName;
			this.SceneClassFullName = cutscene.CurrentScene.UniqueId.FullClassName;
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

		protected abstract void Receive();
	}
}
