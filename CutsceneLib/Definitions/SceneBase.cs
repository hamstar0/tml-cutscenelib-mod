using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using CutsceneLib.Net;


namespace CutsceneLib.Definitions {
	public abstract partial class SceneBase {
		public abstract SceneID UniqueId { get; }


		////

		public bool PrimaryViewerDefersToHostForSync { get; protected set; }

		public bool IsSiezingControls { get; protected set; }

		public bool IsCutscenePlayerImmune { get; protected set; }



		////////////////
		
		protected SceneBase( bool primaryViewerDefersToHostForSync, bool isSiezingControls, bool isCutscenePlayerImmune ) {
			if( !this.ValidateSceneType(this.GetType()) ) {
				throw new ModHelpersException( "Invalid Scene type "+this.GetType().Name );
			}

			this.PrimaryViewerDefersToHostForSync = primaryViewerDefersToHostForSync;
			this.IsSiezingControls = isSiezingControls;
			this.IsCutscenePlayerImmune = isCutscenePlayerImmune;
		}

		////

		private bool ValidateSceneType( Type sceneType ) {
			Type parentType = sceneType.BaseType;
			if( parentType == null || parentType == typeof(object) ) {
				return false;
			}

			Type genSceneType = typeof( Scene<,,,> );
			if( parentType.IsGenericType && parentType.GetGenericTypeDefinition() == genSceneType ) {
				return true;
			}

			return this.ValidateSceneType( parentType );
		}


		////////////////

		public abstract SceneID GetNextSceneId();


		////////////////

		internal abstract CutsceneStartProtocol CreatePacketPayload_Internal( Cutscene cutscene );


		////////////////

		internal abstract void BeginScene_Internal( Cutscene parent );

		////////////////

		internal abstract void EndScene_Internal( Cutscene parent );


		////////////////

		/// <summary></summary>
		/// <returns>`true` signifies scene has ended.</returns>
		internal abstract bool UpdateScene_Internal( Cutscene parent );

		////

		public virtual bool UpdateNPC( NPC npc ) {
			return true;
		}

		public virtual void UpdateNPCFrame( NPC npc, int frameHeight ) { }


		////////////////

		public virtual void DrawInterface() { }
	}
}
