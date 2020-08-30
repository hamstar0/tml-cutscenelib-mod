using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using CutsceneLib.Net;


namespace CutsceneLib.Definitions {
	public abstract partial class SceneBase {
		public abstract SceneID UniqueId { get; }


		////

		public bool DefersToHostForSync { get; }


		////////////////

		protected CutsceneDialogue Dialogue = null;



		////////////////
		
		protected SceneBase( bool defersToHostForSync ) {
			if( !this.ValidateSceneType(this.GetType()) ) {
				throw new ModHelpersException( "Invalid Scene type "+this.GetType().Name );
			}

			this.DefersToHostForSync = defersToHostForSync;
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
		internal virtual bool UpdateScene_Internal( Cutscene parent ) {
			return false;
		}


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

		public virtual void DrawInterface() { }
	}
}
