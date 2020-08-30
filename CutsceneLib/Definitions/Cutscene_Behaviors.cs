using System;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Helpers.Debug;


namespace CutsceneLib.Definitions {
	public abstract partial class Cutscene {
		public abstract bool IsSiezingControls();
		
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
		
		public virtual bool AllowNPC( NPC npc ) {
			return npc.friendly;
		}


		////////////////

		internal void DrawInterface() {
			this.CurrentScene?.DrawInterface();
		}
	}
}
