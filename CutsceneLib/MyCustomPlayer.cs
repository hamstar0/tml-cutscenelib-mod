using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.PlayerData;
using CutsceneLib.Logic;


namespace CutsceneLib {
	class CutsceneLibCustomPlayer : CustomPlayerData {
		protected override object OnExit() {
			CutsceneManager.Instance.ResetCutscenes();
			return base.OnExit();
		}
	}
}