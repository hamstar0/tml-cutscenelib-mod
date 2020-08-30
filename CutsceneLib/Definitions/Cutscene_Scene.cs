using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Logic;
using CutsceneLib.Net;


namespace CutsceneLib.Definitions {
	public abstract partial class Cutscene {
		public bool CanAdvanceCurrentScene() {
			return ( this.CurrentScene.DefersToHostForSync && Main.netMode == NetmodeID.Server )
				|| ( !this.CurrentScene.DefersToHostForSync && Main.netMode != NetmodeID.Server );
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
				this.CurrentScene = this.CreateScene( nextUid );

				if( sync ) {
					if( Main.netMode == NetmodeID.MultiplayerClient ) {
						CutsceneNetStart.Broadcast( cutscene: this );
					} else if( Main.netMode == NetmodeID.Server ) {
						CutsceneNetStart.SendToClients( cutscene: this, -1 );
					}
				}
			} else {
				CutsceneManager.Instance.EndCutscene( this.UniqueId, this.PlaysForWhom, sync );
			}
		}
	}
}
