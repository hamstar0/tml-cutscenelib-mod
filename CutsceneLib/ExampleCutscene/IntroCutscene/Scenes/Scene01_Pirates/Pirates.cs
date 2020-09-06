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

		public override SceneID UniqueId { get; } = new SceneID(
			mod: CutsceneLibMod.Instance,
			sceneType: typeof(Intro01_PiratesScene)
		);

		////

		protected DialogueChoices Dialogue = new DialogueChoices(
			dialogue: "",
			choices: new List<DialogueChoices>(),
			onChoice: () => {
			}
		);



		////////////////

		public Intro01_PiratesScene( IntroMovieSet set )  : base( false, true, true, set ) { }


		////////////////

		public override SceneID GetNextSceneId() {
			return null;
		}


		////////////////

		protected override void OnBegin( IntroCutscene parent ) {
			var cams = new List<CameraMover>();

			this.BeginShot00_ExteriorAttack( parent );

			this.GetCam00_ExteriorAttack( cams, this.BeginShot01_PiratesArrive );
			this.GetCam01_PiratesArrive( cams, () => this.BeginShot02_InteriorChat(parent) );
			this.GetCam02_InteriorChat( cams, null );

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
				break;
			case "CutsceneLib_Intro_Pirates_2":
				this.UpdateNPC02_InteriorChat( parent );
				break;
			}

			return false;
		}


		////////////////

		public override void DrawInterface() {
			switch( CameraMover.Current.Name ) {
			case "CutsceneLib_Intro_Pirates_0":
				break;
			case "CutsceneLib_Intro_Pirates_1":
				this.DrawInterface01_PiratesArrive();
				break;
			case "CutsceneLib_Intro_Pirates_2":
				break;
			}
		}
	}
}
