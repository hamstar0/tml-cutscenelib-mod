using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates {
	partial class Intro01_PiratesScene
				: Scene<IntroCutscene, IntroMovieSet, IntroCutsceneStartProtocol, IntroCutsceneUpdateProtocol> {
		private int CannonHitLoopTimer = 90;



		////////////////

		private void BeginShot02_InteriorChat( IntroCutscene cutscene ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			//

			var plrPos = new Vector2(
				( this.Set.InteriorTileLeft + 48 ) * 16,
				( this.Set.InteriorTileTop + 37 ) * 16
			);

			Player plr = Main.player[ cutscene.PlaysForWhom ];
			plr.position = plrPos;

			//

			this.IsSiezingControls = true;
			this.IsCutscenePlayerImmune = false;

			//

			int guideWho = Main.npc.FirstOrDefault( n => n.type == NPCID.Guide ).whoAmI;

			//this.Dialogue.ShowDialogue();
			// display dialogue from guide: "We're under attack!"
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
				lingerDuration: 60 * 30,
				froDuration: 0,
				onStop: () => {
					onCamStop?.Invoke();
					//CameraMover.Current = cams[next + 1];
				}
			);

			cams.Add( cam );
		}


		////////////////

		private void Update02_InteriorChat( IntroCutscene cutscene ) {
			if( this.CannonHitLoopTimer-- <= 0 ) {
				this.CannonHitLoopTimer = Main.rand.Next( 60, 60 * 7 );
				this.ApplyInteriorCannonHit();
			}
		}

		private void ApplyInteriorCannonHit() {
			Main.PlaySound( SoundID.Item62 );

			var sparkOrigin = new Vector2(
				( this.Set.InteriorTileLeft + 48 ) * 16,
				( this.Set.InteriorTileTop + 28 ) * 16
			);
			sparkOrigin.X += Main.rand.Next( 0, 32 * 16 );
			sparkOrigin.Y += Main.rand.Next( 0, 8 * 16 );

			int sparks = Main.rand.Next( 4, 12 );
			for( int i = 0; i < sparks; i++ ) {
				Dust.NewDust(
					Position: sparkOrigin,
					Width: 16,
					Height: 16,
					Type: 204,
					SpeedX: 0f,
					SpeedY: 0f,
					Alpha: 0,
					newColor: new Color( 255, 255, 255 ),
					Scale: 1f
				);
			}

			CameraShaker.Current = new CameraShaker(
				name: "CutsceneLib_Intro_Pirates_Shake_2",
				peakMagnitude: 3f,
				toDuration: 0,
				lingerDuration: 0,
				froDuration: 60,
				onStop: () => { }
			);
		}


		////////////////

		private bool UpdateNPC02_InteriorChat( NPC npc ) {
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
	}
}
