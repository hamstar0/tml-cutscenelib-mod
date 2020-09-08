using System;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Timers;
using CutsceneLib.Net;


namespace CutsceneLib.Definitions {
	public abstract partial class Cutscene {
		public CutsceneID UniqueId { get; }

		protected abstract SceneID FirstSceneId { get; }

		////

		public abstract bool CanAutoplay { get; }

		public abstract bool CanReplayPerWorld { get; }

		public abstract bool CanReplayPerPlayer { get; }


		////

		public int PlaysForWhom { get; private set; } = -1;

		////

		public SceneBase CurrentScene { get; protected set; } = null;



		////////////////

		protected Cutscene( Player playsFor ) {
			Type mytype = this.GetType();

			this.UniqueId = new CutsceneID(
				mytype.Assembly.GetName().Name,
				mytype.FullName
			);

			this.PlaysForWhom = playsFor?.whoAmI ?? -1;
		}

		////

		protected abstract SceneBase CreateIntroScene();

		protected abstract SceneBase CreateNextScene( SceneID sceneId );

		protected abstract SceneBase CreateIntroSceneFromNetwork( CutsceneStartProtocol data );

		protected abstract SceneBase CreateNextSceneFromNetwork( SceneID sceneId, CutsceneUpdateProtocol data );


		////////////////

		internal CutsceneStartProtocol CreatePacketPayload() {
			return this.CurrentScene.CreatePacketPayload_Internal( this );
		}


		////////////////

		public abstract bool CanBegin( out string result );

		////////////////

		internal void BeginCutscene_Internal() {
			this.CurrentScene = this.CreateIntroScene();
			this.CurrentScene.BeginScene_Internal( this );
		}

		internal void BeginCutsceneFromNetwork_Internal(
					SceneID sceneId,
					CutsceneStartProtocol netData,
					Action<string> onSuccess,
					Action<string> onFail ) {
			this.CurrentScene = this.CreateIntroSceneFromNetwork( netData );

			if( this.CurrentScene != null ) {
				this.CurrentScene.BeginScene_Internal( this );
				onSuccess( "Success." );
				return;
			}

			int retries = 0;
			Timers.SetTimer( 2, true, () => {
				this.CurrentScene = this.CreateIntroSceneFromNetwork( netData );

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
