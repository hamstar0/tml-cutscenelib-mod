using System;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Helpers.Debug;


namespace CutsceneLib.Definitions {
	public abstract partial class Cutscene {
		internal bool IsSiezingControls_Internal() {
			return this.CurrentScene?.IsSiezingControls ?? true;
		}

		internal bool IsCutscenePlayerImmune_Internal() {
			return this.CurrentScene?.IsCutscenePlayerImmune ?? true;
		}
		
		////

		internal void SiezeControl_Internal( string control, ref bool state ) {
			this.SiezeControl( control, ref state );
		}

		protected virtual void SiezeControl( string control, ref bool state ) {
			if( control == "Inventory" ) { return; }
			state = false;
		}
		
		////////////////

		public virtual bool AllowInterfaceLayer( GameInterfaceLayer layer ) {
			return false;
		}
		
		////////////////
		
		internal bool AllowNPC_Internal( NPC npc ) {
			return this.CurrentScene?.AllowNPC_Internal( this, npc ) ?? true;
		}


		////////////////

		internal void DrawInterface() {
			this.CurrentScene?.DrawInterface_Internal();
		}
	}
}
