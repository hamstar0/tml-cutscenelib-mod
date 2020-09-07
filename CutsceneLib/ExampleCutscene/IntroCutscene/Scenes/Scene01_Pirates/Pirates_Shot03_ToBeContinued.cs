using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates {
	partial class Intro01_PiratesScene
				: Scene<IntroCutscene, IntroMovieSet, IntroCutsceneStartProtocol, IntroCutsceneUpdateProtocol> {
		private void BeginShot03_TBC() {
		}


		////////////////

		private void GetCam03_TBC( IList<CameraMover> cams, Action onCamStop ) {
			int next = cams.Count;
			int worldX = ( Main.maxTilesX - 40 ) - ( Main.screenWidth / 16 );
			worldX *= 16;
			int worldY = 40 * 16;

			var cam = new CameraMover(
				name: "CutsceneLib_Intro_Pirates_" + cams.Count,
				moveXFrom: worldX,
				moveYFrom: worldY,
				moveXTo: worldX,
				moveYTo: worldY,
				toDuration: 0,
				lingerDuration: 60 * 5,
				froDuration: 0,
				onStop: () => {
					onCamStop?.Invoke();
					//CameraMover.Current = cams[next + 1];
				}
			);

			cams.Add( cam );
		}


		////////////////

		private void DrawInterface03_TBC() {
			string titleText = "To be continued in Adventure Mode!";
			Vector2 titleDim = Main.fontMouseText.MeasureString( titleText );
			var pos = new Vector2( ( Main.screenWidth / 2 ) - titleDim.X, ( Main.screenHeight / 2 ) - titleDim.Y );

			Utils.DrawBorderStringFourWay(
				sb: Main.spriteBatch,
				font: Main.fontMouseText,
				text: titleText,
				x: pos.X,
				y: pos.Y,
				textColor: Color.White,
				borderColor: Color.Black,
				origin: default( Vector2 ),
				scale: 2f
			);
		}
	}
}
