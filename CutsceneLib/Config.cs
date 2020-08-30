using System;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace CutsceneLib {
	class CutsceneLibConfig : ModConfig {
		public static CutsceneLibConfig Instance => ModContent.GetInstance<CutsceneLibConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;


		////////////////

		public bool DebugModeInfo { get; set; } = false;

		public bool DebugModeFreeMove { get; set; } = false;
	}
}
