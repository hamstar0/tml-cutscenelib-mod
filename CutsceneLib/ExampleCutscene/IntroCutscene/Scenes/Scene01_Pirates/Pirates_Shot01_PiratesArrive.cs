using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates {
	partial class Intro01_PiratesScene : Scene<IntroCutscene, IntroMovieSet, IntroCutsceneNetData> {
		private Vector2 PirateShipPos;



		////////////////

		private void BeginShot01_PiratesArrive() {
			this.PirateShipPos = new Vector2(
				(this.Set.ExteriorTileLeft + this.Set.ExteriorDeckWidth + 24) * 16,
				(int)this.Set.ExteriorShipView.Y + (12 * 16)
			);
		}


		////////////////

		private void GetCam01_PiratesArrive( IList<CameraMover> cams, Action onCamStop ) {
			int lowExteriorShipViewX = this.Set.ExteriorTileLeft * 16;
			lowExteriorShipViewX += (this.Set.ExteriorDeckWidth + 24) * 16;
			int lowExteriorShipViewY = (int)this.Set.ExteriorShipView.Y;
			lowExteriorShipViewY += 12 * 16;
			int next = cams.Count;

			var cam = new CameraMover(
				name: "CutsceneLib_Intro_Pirates_" + cams.Count,
				moveXFrom: lowExteriorShipViewX,
				moveYFrom: lowExteriorShipViewY,
				moveXTo: (int)this.Set.ExteriorShipView.X,
				moveYTo: lowExteriorShipViewY,
				toDuration: 60 * 15,
				lingerDuration: 0,
				froDuration: 0,
				onStop: () => {
					onCamStop?.Invoke();
					CameraMover.Current = cams[next + 1];
				}
			);

			cams.Add( cam );
		}


		////////////////

		private void DrawInterface01_PiratesArrive() {
			Main.instance.LoadNPC( NPCID.PirateShip );

			Main.spriteBatch.Draw(
				texture: Main.npcTexture[ NPCID.PirateShip ],
				position: this.PirateShipPos - Main.screenPosition,
				sourceRectangle: null,
				color: Color.White,
				rotation: 0,
				origin: default( Vector2 ),
				scale: 1f,
				effects: SpriteEffects.FlipHorizontally,
				layerDepth: 1f
			);

			this.PirateShipPos.X -= 1;
		}
	}
}
