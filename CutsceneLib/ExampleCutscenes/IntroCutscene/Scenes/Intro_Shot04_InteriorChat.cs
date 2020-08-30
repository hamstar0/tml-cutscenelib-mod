using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.Cutscenes.IntroCutscene.Net;


namespace CutsceneLib.Cutscenes.IntroCutscene.Scenes {
	partial class IntroCutsceneScene_00 : Scene<IntroCutscene, IntroMovieSet, IntroCutsceneNetData> {
		private void BeginShot04_InteriorChat() {

		}


		////////////////

		private void GetCam04_InteriorChat( IList<CameraMover> cams, Action onCamStop, Vector2 interiorShipView ) {
			int next = cams.Count;
			var cam = new CameraMover(
				name: "CutsceneLibIntro",
				moveXFrom: (int)interiorShipView.X,
				moveYFrom: (int)interiorShipView.Y,
				moveXTo: (int)interiorShipView.X,
				moveYTo: (int)interiorShipView.Y,
				toDuration: 0,
				lingerDuration: 60 * 5,
				froDuration: 0
				/*onStop: () => {
					onCamStop?.Invoke();
					CameraMover.Current = cams[next + 1];
				}*/
			);

			cams.Add( cam );
		}
	}
}
