using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Logic;
using CutsceneLib.Definitions;


namespace CutsceneLib {
	public partial class CutsceneLibMod : Mod {
		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			if( Main.gameMenu ) { return; }
			if( CutsceneLibConfig.Instance.DebugModeFreeMove ) { return; }

			Cutscene nowCutscene = CutsceneManager.Instance?.GetCurrentCutscene_Player( Main.LocalPlayer );
			if( nowCutscene == null ) {
				return;
			}

			foreach( GameInterfaceLayer layer in layers ) {
				if( layer.Name.Equals("Vanilla: Cursor") ) {
					continue;
				}
				if( layer.Name.Equals("Cutscene Lib: Titles") ) {
					continue;
				}
				if( layer.Name.Equals( "ModHelpers: HUD UI") ) {
					continue;
				}
				if( layer.Name.Equals( "ModHelpers: Mod Lock" ) ) {
					continue;
				}
				if( layer.Name.Equals( "ModHelpers: Debug Display" ) ) {
					continue;
				}
				if( nowCutscene.AllowInterfaceLayer(layer) ) {
					continue;
				}
				layer.Active = false;
			}

			this.AddTitleDisplayLayer( layers );
		}

		////

		private void AddTitleDisplayLayer( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			GameInterfaceDrawMethod ui = () => {
				Cutscene cutscene = CutsceneManager.Instance.GetCurrentCutscene_Player( Main.LocalPlayer );
				if( cutscene != null ) {
					cutscene.DrawInterface();
				}
				return true;
			};

			////

			var tradeLayer = new LegacyGameInterfaceLayer( "Cutscene Lib: Titles", ui, InterfaceScaleType.UI );
			layers.Insert( idx, tradeLayer );
		}
	}
}