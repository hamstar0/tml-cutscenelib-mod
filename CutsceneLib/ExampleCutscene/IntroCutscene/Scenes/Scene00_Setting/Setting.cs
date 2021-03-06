﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;
using CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene00_Setting {
	partial class Intro00_SettingScene
				: Scene<IntroCutscene, IntroMovieSet, IntroCutsceneStartProtocol, IntroCutsceneUpdateProtocol> {
		public Intro00_SettingScene( IntroMovieSet set )  : base( false, true, true, set ) { }


		////////////////

		public override SceneID GetNextSceneId() {
			return new SceneID(
				mod: CutsceneLibMod.Instance,
				sceneType: typeof( Intro01_PiratesScene )
			);
		}


		////////////////

		public override bool AllowNPC( IntroCutscene parent, NPC npc ) {
			if( npc.friendly ) {
				return true;
			}
			if( parent._ShipInterior.Bounds.Intersects(npc.getRect()) ) {
				return false;
			}
			if( parent._ShipExterior.Bounds.Intersects(npc.getRect()) ) {
				return false;
			}
			return true;
		}


		////////////////

		protected override void OnBegin( IntroCutscene parent ) {
			var cams = new List<CameraMover>();

			this.BeginShot00_Title( parent );
			
			this.GetCam00_Title( cams, this.BeginShot01_ExteriorChat );
			this.GetCam01_ExteriorChat( cams, this.BeginShot02_Dungeon );
			this.GetCam02_Dungeon( cams, null );

			CameraMover.Current = cams[0];
		}


		////////////////

		protected override bool Update( IntroCutscene parent ) {
			if( Main.netMode != NetmodeID.Server ) {
				var cam = CameraMover.Current;
				if( cam == null || !cam.Name.StartsWith( "CutsceneLib_Intro_Setting_" ) || !cam.IsAnimating() ) {
					return true;
				}
			}

			return false;
		}

		////

		protected override bool UpdateNPC( NPC npc ) {
			var cam = CameraMover.Current;

			switch( cam.Name ) {
			case "CutsceneLib_Intro_Setting_0":
				break;
			case "CutsceneLib_Intro_Setting_1":
				return this.UpdateNPC01_ExteriorChat( npc );
			case "CutsceneLib_Intro_Setting_2":
				return this.UpdateNPC02_DungeonView( npc );
			}
			return true;
		}

		protected override void UpdateNPCFrame( NPC npc, int frameHeight ) {
			var cam = CameraMover.Current;

			switch( cam.Name ) {
			case "CutsceneLib_Intro_Setting_0":
				break;
			case "CutsceneLib_Intro_Setting_1":
				this.UpdateNPCFrame01_ExteriorChat( npc, frameHeight );
				break;
			case "CutsceneLib_Intro_Setting_2":
				break;
			}
		}


		////////////////

		protected override void DrawInterface() {
			switch( CameraMover.Current.Name ) {
			case "CutsceneLib_Intro_Setting_0":
				this.DrawInterface00_Title();
				break;
			case "CutsceneLib_Intro_Setting_1":
				this.DrawInterface01_ExteriorChat();
				break;
			case "CutsceneLib_Intro_Setting_2":
				break;
			}
		}
	}
}
