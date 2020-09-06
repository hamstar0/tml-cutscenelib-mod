using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Logic;
using CutsceneLib.Definitions;


namespace CutsceneLib {
	public partial class CutsceneLibMod : Mod {
		public override void PostUpdateEverything() {
			if( !Main.gameMenu || Main.netMode == NetmodeID.Server ) {
				CutsceneManager.Instance.Update_Internal();
			}
		}


		////////////////

		public override void PostUpdateInput() {
			if( Main.gameMenu ) { return; }
			if( CutsceneLibConfig.Instance.DebugModeFreeMove ) { return; }

			Cutscene nowCutscene = CutsceneManager.Instance?.GetCurrentCutscene_Player( Main.LocalPlayer );
			if( nowCutscene?.IsSiezingControls_Internal() != true ) {
				return;
			}

			IDictionary<string, bool> keys = PlayerInput.Triggers.Current.KeyStatus;

			foreach( string key in keys.Keys.ToArray() ) {
				bool on = keys[key];

				nowCutscene.SiezeControl_Internal( key, ref on );
				keys[key] = on;
			}
		}
	}
}