using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;


namespace CutsceneLib.Definitions {
	public abstract partial class Cutscene {
		internal void UpdateCutscene_Internal() {
			// If the current scene has ended, continue to next scene
			if( this.CurrentScene?.UpdateScene_Internal(this) ?? false ) {
				if( this.CanAdvanceCurrentScene() ) {
					this.AdvanceScene( true );
				}
			}
		}


		////

		internal bool UpdateNPC_Internal( NPC npc ) {
			return this.CurrentScene?.UpdateNPC_Internal( npc ) ?? true;
		}

		internal void Update_NPCFrame_Internal( NPC npc, int frameHeight ) {
			this.CurrentScene?.UpdateNPCFrame_Internal( npc, frameHeight );
		}
	}
}
