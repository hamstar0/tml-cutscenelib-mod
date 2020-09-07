﻿using System;
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
		private void BeginShot01_PiratesArrive() {
		}


		////////////////

		private void GetCam01_PiratesArrive( IList<CameraMover> cams, Action onCamStop ) {
			int lowExteriorShipViewX = this.Set.ExteriorTileLeft * 16;
			lowExteriorShipViewX += (this.Set.ExteriorDeckWidth + 24) * 16;
			int lowExteriorShipViewY = (int)this.Set.ExteriorShipView.Y;
			lowExteriorShipViewY += 8 * 16;
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

		private bool UpdateNPC01_PiratesArrive( NPC npc ) {
			if( npc.whoAmI == this.Set.InteriorCrewNPC ) {
				npc.ai[0] = 0f;
				npc.ai[1] = 0f;
				npc.ai[2] = 0f;
				npc.ai[3] = 0f;
				npc.direction = -1;
				npc.velocity = default( Vector2 );
				return false;
			}
			return true;
		}


		////////////////

		private void UpdateNPCFrame01_PiratesArrive( NPC npc, int frameHeight ) {
			if( npc.whoAmI == this.Set.ShipPropNPC ) {
				npc.direction = -1;
				npc.velocity = new Vector2( -1.35f, 0f );
			}
		}


		////////////////

		/*private void DrawInterface01_PiratesArrive() {
			Main.instance.LoadNPC( NPCID.PirateShip );

			Vector2 pos = this.PirateShipPos - Main.screenPosition;
			pos.Y -= Main.npcTexture[ NPCID.PirateShip ].Height;
			pos.Y += 48;

			Main.spriteBatch.Draw(
				texture: Main.npcTexture[ NPCID.PirateShip ],
				position: pos,
				sourceRectangle: null,
				color: Lighting.GetColor( (int)(this.PirateShipPos.X/16), (int)(this.PirateShipPos.Y/16) ),
				rotation: 0,
				origin: default( Vector2 ),
				scale: 1f,
				effects: SpriteEffects.FlipHorizontally,
				layerDepth: 1f
			);

			this.PirateShipPos.X -= 1;
		}*/
	}
}
