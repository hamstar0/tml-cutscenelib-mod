using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using CutsceneLib.Net;


namespace CutsceneLib.Definitions {
	public abstract partial class SceneBase {
		public abstract SceneID UniqueId { get; }


		////

		public bool PrimaryViewerDefersToHostForSync { get; }


		////////////////

		protected CutsceneDialogue Dialogue = null;



		////////////////
		
		protected SceneBase( bool primaryViewerDefersToHostForSync ) {
			if( !this.ValidateSceneType(this.GetType()) ) {
				throw new ModHelpersException( "Invalid Scene type "+this.GetType().Name );
			}

			this.PrimaryViewerDefersToHostForSync = primaryViewerDefersToHostForSync;
		}

		////

		private bool ValidateSceneType( Type sceneType ) {
			Type parentType = sceneType.BaseType;
			if( parentType == null || parentType == typeof(object) ) {
				return false;
			}

			Type genSceneType = typeof( Scene<,,> );
			if( parentType.IsGenericType && parentType.GetGenericTypeDefinition() == genSceneType ) {
				return true;
			}

			return this.ValidateSceneType( parentType );
		}


		////////////////

		public abstract SceneID GetNextSceneId();


		////////////////

		internal abstract CutsceneNetStart CreatePacketPayload_Internal( Cutscene cutscene );


		////////////////

		/// <summary></summary>
		/// <returns>`true` signifies scene has ended.</returns>
		internal abstract bool UpdateScene_Internal( Cutscene parent );


		////////////////
		
		internal virtual void BeginScene_Internal( Cutscene parent ) {
			if( parent.PlaysForWhom == Main.myPlayer ) {
				this.Dialogue?.ShowDialogue();
			}
		}

		////////////////
		
		internal virtual void EndScene_Internal( Cutscene parent ) {
			if( parent.PlaysForWhom == Main.myPlayer ) {
				this.Dialogue?.HideDialogue();
			}
		}


		////////////////

		public virtual void UpdateNPC( NPC npc ) { }


		////////////////

		public virtual void DrawInterface() { }
	}
}
