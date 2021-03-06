﻿using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.TileStructure;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Info;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene00_Setting;
using CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates;


namespace CutsceneLib.ExampleCutscene.IntroCutscene {
	partial class IntroCutscene : Cutscene {
		internal TileStructure _ShipExterior;
		internal TileStructure _ShipInterior;
		private int _MovieSetChunksRequestRetryTimer = 0;


		////////////////

		protected override SceneID FirstSceneId { get; } = new SceneID(
			mod: CutsceneLibMod.Instance,
			sceneType: typeof(Intro00_SettingScene)
		);

		public SceneID SecondSceneId { get; } = new SceneID(
			mod: CutsceneLibMod.Instance,
			sceneType: typeof(Intro01_PiratesScene)
		);

		////////////////

		public override bool CanAutoplay => false;

		public override bool CanReplayPerWorld => true;

		public override bool CanReplayPerPlayer => false;



		////////////////
		
		private IntroCutscene( Player playsFor ) : base( playsFor ) { }


		////////////////

		public override bool CanBegin( out string result ) {
			if( GameInfoHelpers.GetVanillaProgressList().Count > 0 ) {
				result = "World progress has occurred.";
				return false;
			}
			if( NPC.AnyNPCs(NPCID.Merchant) ) {
				result = "Merchants exist.";
				return false;
			}
			result = "Success.";
			return true;
		}
	}
}
