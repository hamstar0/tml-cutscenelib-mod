using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;
using CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene00_Setting;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates {
	partial class Intro01_PiratesScene
				: Scene<IntroCutscene, IntroMovieSet, IntroCutsceneStartProtocol, IntroCutsceneUpdateProtocol> {
		public static Intro01_PiratesScene Create( IntroCutscene cutscene ) {
			var currScene = cutscene.CurrentScene as Intro00_SettingScene;
			return new Intro01_PiratesScene( currScene.Set );
		}



		////////////////

		public Intro01_PiratesScene( IntroMovieSet set )  : base( false, true, true, set ) { }


		////////////////

		public override SceneID GetNextSceneId() {
			return null;
		}


		////////////////
		protected override int? UpdateMusicID() {
			return MusicID.PirateInvasion;
		}


		////////////////

		public override bool AllowNPC( IntroCutscene parent, NPC npc ) {
			if( npc.friendly ) {
				return true;
			}
			if( npc.whoAmI == this.Set.ShipPropNPC ) {
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

			this.PrepareMovieSet();

			this.BeginShot00_ExteriorAttack( parent );

			this.GetCam00_ExteriorAttack( cams, this.BeginShot01_PiratesArrive );
			this.GetCam01_PiratesArrive( cams, () => this.BeginShot02_InteriorChat(parent) );
			this.GetCam02_InteriorChat( cams, this.BeginShot03_TBC );
			this.GetCam03_TBC( cams, null );

			CameraMover.Current = cams[0];
		}


		////////////////

		protected override bool Update( IntroCutscene parent ) {
			var cam = CameraMover.Current;

			if( Main.netMode != NetmodeID.Server ) {
				if( cam == null || !cam.Name.StartsWith( "CutsceneLib_Intro_Pirates_" ) || !cam.IsAnimating() ) {
					return true;
				}
			}

			switch( cam.Name ) {
			case "CutsceneLib_Intro_Pirates_0":
				break;
			case "CutsceneLib_Intro_Pirates_1":
				this.Update01_PiratesArrive( parent );
				break;
			case "CutsceneLib_Intro_Pirates_2":
				this.Update02_InteriorChat( parent );
				break;
			case "CutsceneLib_Intro_Pirates_3":
				break;
			}

			return false;
		}

		////

		protected override bool UpdateNPC( NPC npc ) {
			var cam = CameraMover.Current;

			switch( cam.Name ) {
			case "CutsceneLib_Intro_Pirates_0":
				return this.UpdateNPC00_ExteriorAttack( npc );
			case "CutsceneLib_Intro_Pirates_1":
				return this.UpdateNPC01_PiratesArrive( npc );
			case "CutsceneLib_Intro_Pirates_2":
				return this.UpdateNPC02_InteriorChat( npc );
			case "CutsceneLib_Intro_Pirates_3":
				break;
			}
			return true;
		}


		////////////////

		protected override void UpdateNPCFrame( NPC npc, int frameHeight ) {
			switch( CameraMover.Current.Name ) {
			case "CutsceneLib_Intro_Pirates_0":
				this.UpdateNPCFrame00_ExteriorAttack( npc, frameHeight );
				break;
			case "CutsceneLib_Intro_Pirates_1":
				this.UpdateNPCFrame01_PiratesArrive( npc, frameHeight );
				break;
			case "CutsceneLib_Intro_Pirates_2":
				break;
			case "CutsceneLib_Intro_Pirates_3":
				break;
			}
		}


		////////////////

		protected override void DrawInterface() {
			switch( CameraMover.Current.Name ) {
			case "CutsceneLib_Intro_Pirates_0":
				break;
			case "CutsceneLib_Intro_Pirates_1":
				//this.DrawInterface01_PiratesArrive();
				break;
			case "CutsceneLib_Intro_Pirates_2":
				break;
			case "CutsceneLib_Intro_Pirates_3":
				this.DrawInterface03_TBC();
				break;
			}
		}
	}
}
