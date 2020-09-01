using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;


namespace CutsceneLib.Definitions {
	public abstract partial class Cutscene {
		internal void UpdateCutscene_Internal() {
			// If the cutscene says so, continue to next scene
			if( this.Update() ) {
				if( this.CanAdvanceCurrentScene() ) {
					this.AdvanceScene( true );
				}
			}

			// If the current scene has ended, continue to next scene
			if( this.CurrentScene?.UpdateScene_Internal(this) ?? false ) {
				if( this.CanAdvanceCurrentScene() ) {
					this.AdvanceScene( true );
				}
			}
		}


		////

		/// <summary></summary>
		/// <returns>`true` signifies current scene has ended, and should advance.</returns>
		protected abstract bool Update();

		protected virtual void UpdateNPC( NPC npc ) { }
	}
}
