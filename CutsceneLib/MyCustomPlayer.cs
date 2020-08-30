using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.PlayerData;
using CutsceneLib.Logic;


namespace CutsceneLib {
	class CutsceneLibCustomPlayer : CustomPlayerData {
		protected override void OnEnter( object data ) {
			base.OnEnter( data );

			this.Player.GetModPlayer<CutsceneLibPlayer>().IsPlayerCutsceneLibCompat = true;
			LogHelpers.Log( "Player "+this.Player.name+" prepped for Cutscene Lib." );
		}

		protected override object OnExit() {
			CutsceneManager.Instance.ResetCutscenes();
			return base.OnExit();
		}
	}
}