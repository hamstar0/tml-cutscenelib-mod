using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes {
	partial class IntroCutsceneScene_00 : Scene<IntroCutscene, IntroMovieSet, IntroCutsceneNetData> {
		public override SceneID UniqueId { get; } = new SceneID(
			mod: CutsceneLibMod.Instance,
			sceneType: typeof(IntroCutsceneScene_00)
		);



		////////////////

		public IntroCutsceneScene_00( IntroMovieSet set )  : base( false, set ) { }


		////////////////

		public override SceneID GetNextSceneId() {
			return null;
		}


		////////////////

		protected override void OnBegin( IntroCutscene parent ) {
			var cams = new List<CameraMover>();
			Vector2 exteriorShipView = this.Set.ExteriorShipView;
			Vector2 interiorShipView = this.Set.InteriorShipView;

			bool isShipOnLeft = (int)exteriorShipView.X < ((16 * Main.maxTilesX) / 2);

			var dungeonView = new Vector2( Main.dungeonX * 16, Main.dungeonY * 16 );
			dungeonView.X += isShipOnLeft ? (-32 * 16) : (32 * 16);
			dungeonView.Y += -32 * 16;

			this.BeginShot00_Title( parent );
			
			this.GetCam00_Title( cams, this.BeginShot01_ExteriorChat );
			this.GetCam01_ExteriorChat( cams, null, exteriorShipView );
			this.GetCam02_Dungeon( cams, this.BeginShot03_ExteriorAttack, dungeonView );
			this.GetCam03_ExteriorAttack( cams, this.BeginShot04_InteriorChat, exteriorShipView );
			this.GetCam04_InteriorChat( cams, null, interiorShipView );

			CameraMover.Current = cams[0];
		}


		////////////////

		protected override bool Update( IntroCutscene parent ) {
			if( Main.netMode == NetmodeID.Server ) {
				return false;
			}

			var cam = CameraMover.Current;
			if( cam == null || !cam.Name.StartsWith("CutsceneLibIntro") || !cam.IsAnimating() ) {
				return true;
			}

			return false;
		}

		////

		public override void UpdateNPC( NPC npc ) {
			if( npc.type == NPCID.OldMan ) {
				npc.ai[0] = 0f;
				npc.ai[1] = 0f;
				npc.ai[2] = 0f;
				npc.ai[3] = 0f;
			}
		}


		////////////////

		public override void DrawInterface() {
			switch( CameraMover.Current.Name ) {
			case "CutsceneLibIntro_Title":
				this.DrawInterface00_Title();
				break;
			}
		}
	}
}
