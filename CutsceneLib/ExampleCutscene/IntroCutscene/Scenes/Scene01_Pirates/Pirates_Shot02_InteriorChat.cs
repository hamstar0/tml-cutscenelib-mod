using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates {
	partial class Intro01_PiratesScene : Scene<IntroCutscene, IntroMovieSet, IntroCutsceneNetData> {
		private void BeginShot02_InteriorChat() {

		}


		////////////////

		private void GetCam02_InteriorChat( IList<CameraMover> cams, Action onCamStop ) {
			Vector2 interiorShipView = this.Set.InteriorShipView;
			int next = cams.Count;

			var cam = new CameraMover(
				name: "CutsceneLib_Intro_Pirates_" + cams.Count,
				moveXFrom: (int)interiorShipView.X,
				moveYFrom: (int)interiorShipView.Y,
				moveXTo: (int)interiorShipView.X,
				moveYTo: (int)interiorShipView.Y,
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
	}
}
