using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Logic;
using CutsceneLib.Net;


namespace CutsceneLib.Definitions {
	public abstract partial class Cutscene {
		public bool CanAdvanceCurrentScene() {
			if( Main.netMode == NetmodeID.Server ) {
				return this.CurrentScene.PrimaryViewerDefersToHostForSync;
			} else {
				if( this.PlaysForWhom == Main.myPlayer ) {
					return !this.CurrentScene.PrimaryViewerDefersToHostForSync;
				}
				return false;
			}
		}


		////////////////

		internal void SetCurrentScene( SceneID uid, bool sync ) {
			this.CurrentScene.EndScene_Internal( this );
			this.CurrentScene = this.CreateScene( uid );
			this.CurrentScene.BeginScene_Internal( this );

			if( sync ) {
				if( Main.netMode == NetmodeID.Server ) {
					CutsceneNetStart.SendToClients( cutscene: this, ignoreWho: -1 );
				} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
					CutsceneNetStart.Broadcast( cutscene: this );
				}
			}
		}

		////

		public void AdvanceScene( bool sync ) {
			SceneID nextUid = this.CurrentScene.GetNextSceneId();

			if( nextUid != null ) {
				this.SetCurrentScene( nextUid, sync );
			} else {
				CutsceneManager.Instance.EndCutscene( this.UniqueId, this.PlaysForWhom, sync );
			}
		}
	}
}
