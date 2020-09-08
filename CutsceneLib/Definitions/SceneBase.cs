using System;
using Terraria;
using CutsceneLib.Net;
using HamstarHelpers.Classes.Errors;


namespace CutsceneLib.Definitions {
	public abstract partial class SceneBase {
		public SceneID UniqueId { get; }


		////

		public bool PrimaryViewerDefersToHostForSync { get; protected set; }

		public bool IsSiezingControls { get; protected set; }

		public bool IsCutscenePlayerImmune { get; protected set; }



		////////////////
		
		protected SceneBase(
					bool primaryViewerDefersToHostForSync,
					bool isSiezingControls,
					bool isCutscenePlayerImmune ) {
			Type mytype = this.GetType();
			if( !this.ValidateSceneType(mytype) ) {
				throw new ModHelpersException( "Invalid Scene type "+mytype.Name );
			}

			this.UniqueId = new SceneID(
				mytype.Assembly.GetName().Name,
				mytype.FullName
			);

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

		internal abstract bool AllowNPC_Internal( Cutscene parent, NPC npc );


		internal abstract CutsceneStartProtocol CreatePacketPayload_Internal( Cutscene cutscene );


		internal abstract void BeginScene_Internal( Cutscene parent );

		internal abstract void EndScene_Internal( Cutscene parent );

		/// <summary></summary>
		/// <returns>`true` signifies scene has ended.</returns>
		internal abstract bool UpdateScene_Internal( Cutscene parent );
	}
}
