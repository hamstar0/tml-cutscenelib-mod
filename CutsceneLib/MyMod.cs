using System.Linq;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Logic;
using CutsceneLib.Definitions;


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


		////////////////

		public override void UpdateMusic( ref int music, ref MusicPriority priority ) {
			var mngr = CutsceneManager.Instance;
			if( mngr == null ) { return; }

			Cutscene cutscene = mngr.GetCurrentCutscene_Player( Main.LocalPlayer );
			if( cutscene == null ) { return; }

			int? newMusIdx = cutscene.CurrentScene?.UpdateMusic_Internal() ?? -1;
			if( !newMusIdx.HasValue ) { return; }

			Main.musicFade[Main.curMusic] = 0;

			if( newMusIdx.Value != -1 ) {
				Main.curMusic = newMusIdx.Value;
				Main.musicFade[Main.curMusic] = 1f;
				music = newMusIdx.Value;
			}
		}
	}
}