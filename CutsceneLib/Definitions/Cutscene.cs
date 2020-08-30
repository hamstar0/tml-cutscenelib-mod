using System;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Timers;
using CutsceneLib.Net;


namespace CutsceneLib.Definitions {
	public abstract partial class Cutscene {
		public abstract CutsceneID UniqueId { get; }

		public abstract SceneID FirstSceneId { get; }

		public abstract bool Autoplay { get; }

		////

		public int PlaysForWhom { get; private set; } = -1;

		////

		public SceneBase CurrentScene { get; protected set; } = null;



		////////////////

		protected Cutscene( Player playsFor ) {
			this.PlaysForWhom = playsFor?.whoAmI ?? -1;
		}

		////

		protected abstract SceneBase CreateScene( SceneID sceneId );

		protected abstract SceneBase CreateSceneFromNetwork( SceneID sceneId, CutsceneNetStart data );


		////////////////

		internal CutsceneNetStart CreatePacketPayload() {
			return this.CurrentScene.CreatePacketPayload_Internal( this );
		}


		////////////////

		public abstract bool CanBegin( out string result );

		////////////////

		internal void BeginCutscene_Internal( SceneID sceneId ) {
			this.CurrentScene = this.CreateScene( sceneId );
			this.CurrentScene.BeginScene_Internal( this );
		}

		internal void BeginCutsceneFromNetwork_Internal(
					SceneID sceneId,
					CutsceneNetStart data,
					Action<string> onSuccess,
					Action<string> onFail ) {
			this.CurrentScene = this.CreateSceneFromNetwork( sceneId, data );

			if( this.CurrentScene != null ) {
				this.CurrentScene.BeginScene_Internal( this );
				onSuccess( "Success." );
				return;
			}

			int retries = 0;
			Timers.SetTimer( 2, true, () => {
				this.CurrentScene = this.CreateSceneFromNetwork( sceneId, data );

				if( this.CurrentScene == null ) {
					if( retries++ < 1000 ) {
						return true;
					} else {
						onFail( "Timed out." );
						return false;
					}
				}

				this.CurrentScene.BeginScene_Internal( this );

				onSuccess( "Success." );
				return false;
			} );
		}


		////////////////

		internal void EndCutscene_Internal() {
			this.OnEnd();
		}

		////

		protected virtual void OnEnd() { }
	}
}
