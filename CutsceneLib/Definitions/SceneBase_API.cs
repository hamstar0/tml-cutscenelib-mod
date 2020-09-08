using System;
using Terraria;
using HamstarHelpers.Classes.Errors;


namespace CutsceneLib.Definitions {
	public abstract partial class SceneBase {
		public abstract SceneID GetNextSceneId();


		////////////////

		internal bool UpdateNPC_Internal( NPC npc ) {
			return this.UpdateNPC( npc );
		}

		////

		protected virtual bool UpdateNPC( NPC npc ) {
			return true;
		}


		////////////////

		internal void UpdateNPCFrame_Internal( NPC npc, int frameHeight ) {
			this.UpdateNPCFrame( npc, frameHeight );
		}

		////

		protected virtual void UpdateNPCFrame( NPC npc, int frameHeight ) { }


		////////////////

		internal void DrawInterface_Internal() {
			this.DrawInterface();
		}

		////

		protected virtual void DrawInterface() { }
	}
}
