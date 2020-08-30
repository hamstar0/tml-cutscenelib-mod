using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace CutsceneLib {
	public partial class CutsceneLibMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-cutscenelib-mod";


		////////////////

		public static CutsceneLibMod Instance { get; private set; }



		////////////////

		public CutsceneLibMod() {
			CutsceneLibMod.Instance = this;
		}

		public override void Unload() {
			CutsceneLibMod.Instance = null;
		}
	}
}