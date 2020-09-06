using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;
using HamstarHelpers.Services.Timers;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates {
	partial class Intro01_PiratesScene
				: Scene<IntroCutscene, IntroMovieSet, IntroCutsceneStartProtocol, IntroCutsceneUpdateProtocol> {
		private void BeginShot00_ExteriorAttack( IntroCutscene parent ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			//

			void launchCannon( bool rand ) {
				var position = new Vector2(
					(this.Set.ExteriorDeckX + this.Set.ExteriorDeckWidth + 56) * 16,
					(this.Set.ExteriorDeckY + 8) * 16
				);
				var velocity = new Vector2(
					-30f,
					rand ? -3f + (-4f * Main.rand.NextFloat()) : -5f
				);

				int projWho = Projectile.NewProjectile(
					position: position,
					velocity: velocity,
					Type: ProjectileID.CannonballHostile,
					Damage: rand ? 100 : 500,
					KnockBack: 4f,
					Owner: parent.PlaysForWhom
				);

				Projectile proj = Main.projectile[projWho];
				proj.hostile = true;
				proj.friendly = false;

				if( Main.netMode != NetmodeID.SinglePlayer ) {
					NetMessage.SendData( MessageID.SyncProjectile, -1, -1, null, projWho );
				}
			}

			//

			Timers.SetTimer( 60, false, () => {
				launchCannon( true );
				return false;
			} );
			Timers.SetTimer( (int)(60f * 1.5f), false, () => {
				launchCannon( true );
				return false;
			} );
			Timers.SetTimer( 60 * 3, false, () => {
				launchCannon( true );
				return false;
			} );
			Timers.SetTimer( 60 * 4, false, () => {
				launchCannon( true );
				return false;
			} );
			Timers.SetTimer( (int)(60f * 4.5f), false, () => {
				launchCannon( false );
				return false;
			} );
		}


		////////////////

		private void GetCam00_ExteriorAttack(
					IList<CameraMover> cams,
					Action onCamStop ) {
			Vector2 exteriorShipView = this.Set.ExteriorShipView;
			int next = cams.Count;

			var cam = new CameraMover(
				name: "CutsceneLib_Intro_Pirates_" + cams.Count,
				moveXFrom: (int)exteriorShipView.X,
				moveYFrom: (int)exteriorShipView.Y,
				moveXTo: (int)exteriorShipView.X,
				moveYTo: (int)exteriorShipView.Y,
				toDuration: 0,
				lingerDuration: 60 * 10,
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
