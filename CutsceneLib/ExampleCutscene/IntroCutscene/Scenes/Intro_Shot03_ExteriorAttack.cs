using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes {
	partial class IntroCutsceneScene_00 : Scene<IntroCutscene, IntroMovieSet, IntroCutsceneNetData> {
		private void BeginShot03_ExteriorAttack() {

		}


		////////////////

		private void GetCam03_ExteriorAttack(
					IList<CameraMover> cams,
					Action onCamStop,
					Vector2 exteriorShipView ) {
			int next = cams.Count;
			var cam = new CameraMover(
				name: "CutsceneLibIntro",
				moveXFrom: (int)exteriorShipView.X,
				moveYFrom: (int)exteriorShipView.Y,
				moveXTo: (int)exteriorShipView.X,
				moveYTo: (int)exteriorShipView.Y,
				toDuration: 0,
				lingerDuration: 60 * 5,
				froDuration: 0,
				onStop: () => {
					onCamStop?.Invoke();
					CameraMover.Current = cams[next + 1];
				}
			);

			cams.Add( cam );
		}
	}
}
