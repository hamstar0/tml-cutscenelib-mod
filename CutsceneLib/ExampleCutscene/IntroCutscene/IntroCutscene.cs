﻿using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.TileStructure;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Info;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Scenes;


namespace CutsceneLib.ExampleCutscene.IntroCutscene {
	partial class IntroCutscene : Cutscene {
		private TileStructure _ShipExterior;
		private TileStructure _ShipInterior;
		private int _MovieSetChunksRequestRetryTimer = 0;


		////////////////

		public override CutsceneID UniqueId { get; } = new CutsceneID(
			mod: CutsceneLibMod.Instance,
			cutsceneType: typeof(IntroCutscene)
		);

		public override SceneID FirstSceneId { get; } = new SceneID(
			mod: CutsceneLibMod.Instance,
			sceneType: typeof(IntroCutsceneScene_00)
		);

		////

		public override bool CanAutoplay => false;

		public override bool CanReplayPerWorld => true;

		public override bool CanReplayPerPlayer => false;


		////

		public override bool IsSiezingControls() => true;



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


		////////////////

		protected override void UpdateNPC( NPC npc ) {
			if( npc.type == NPCID.OldMan ) {
				npc.ai[0] = 0f;
				npc.ai[1] = 0f;
				npc.ai[2] = 0f;
				npc.ai[3] = 0f;
			}
		}
	}
}
