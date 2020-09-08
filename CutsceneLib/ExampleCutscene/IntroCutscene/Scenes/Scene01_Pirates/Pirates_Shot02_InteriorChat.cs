using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates {
	partial class Intro01_PiratesScene
				: Scene<IntroCutscene, IntroMovieSet, IntroCutsceneStartProtocol, IntroCutsceneUpdateProtocol> {
		private int CannonHitLoopTimer = 90;

		private DialogueChoices Dialogue;
		private string ChoiceMade = null;



		////////////////

		private void BeginShot02_InteriorChat( IntroCutscene cutscene ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			//

			var plrPos = new Vector2(
				( this.Set.InteriorTileLeft + 46 ) * 16,
				( this.Set.InteriorTileTop + 37 ) * 16
			);

			Player plr = Main.player[ cutscene.PlaysForWhom ];
			plr.position = plrPos;

			//

			this.IsSiezingControls = true;
			this.IsCutscenePlayerImmune = false;

			//

			this.Dialogue = new DialogueChoices(
				dialogue: "We're under attack! Do we stay and fight?",
				portrait: ModContent.GetTexture( "CutsceneLib/ExampleCutscene/IntroCutscene/guide_head" ),
				height: 224,
				choices: new List<string> { "Bring it!", "Must escape!" },
				( choice ) => {
					this.ChoiceMade = choice;
					this.Dialogue.HideDialogue();
				}
			);
		}


		////////////////

		private void GetCam02_InteriorChat( IList<CameraMover> cams, Action onCamStop ) {
			Vector2 interiorShipView = this.Set.InteriorShipView;
			int next = cams.Count;

			CameraMover copyCam = null;
			var cam = new CameraMover(
				name: "CutsceneLib_Intro_Pirates_" + cams.Count,
				moveXFrom: (int)interiorShipView.X,
				moveYFrom: (int)interiorShipView.Y,
				moveXTo: (int)interiorShipView.X,
				moveYTo: (int)interiorShipView.Y,
				toDuration: 0,
				lingerDuration: 60 * 30,
				froDuration: 0,
				isSmoothed: false,
				onTraversed: () => {
					if( this.ChoiceMade == null ) {
						copyCam.Seek( 1 );
					} else if( copyCam.TicksElapsed < (59 * 30) ) {
						copyCam.Seek( (60 * 30) - 2 );
					}
				},
				onStop: () => {
					onCamStop?.Invoke();
					CameraMover.Current = cams[next + 1];
				}
			);
			copyCam = cam;

			cams.Add( cam );
		}


		////////////////

		private void Update02_InteriorChat( IntroCutscene cutscene ) {
			if( this.CannonHitLoopTimer-- <= 0 ) {
				this.CannonHitLoopTimer = Main.rand.Next( 60, 60 * 3 );
				this.ApplyInteriorCannonHit();
			}
		}

		private void ApplyInteriorCannonHit() {
			Main.PlaySound( SoundID.Item62 );

			var sparkOrigin = new Vector2(
				( this.Set.InteriorTileLeft + 40 ) * 16,
				( this.Set.InteriorTileTop + 30 ) * 16
			);
			sparkOrigin.X += Main.rand.Next( 0, 34 * 16 );
			sparkOrigin.Y += Main.rand.Next( 0, 8 * 16 );

			int sparks = Main.rand.Next( 2, 12 );
			for( int i = 0; i < sparks; i++ ) {
				int idx = Dust.NewDust(
					Position: sparkOrigin,
					Width: 8,
					Height: 8,
					Type: 158,  //204, 87, 64, 269
					SpeedX: 0f,
					SpeedY: 0f,
					Alpha: 0,
					newColor: new Color( 255, 255, 255 ),
					Scale: 1f
				);
				Main.dust[idx].noLight = true;
			}

			CameraShaker.Current = new CameraShaker(
				name: "CutsceneLib_Intro_Pirates_Shake_2",
				peakMagnitude: 3f,
				toDuration: 0,
				lingerDuration: 0,
				froDuration: 60,
				isSmoothed: false,
				onStop: () => { }
			);

			/*int x = (int)(sparkOrigin.X / 16f);
			int y = (int)(sparkOrigin.Y / 16f);
			void blackout( bool on ) {
				for( int i=x; i<(x+34); i++ ) {
					for( int j=y; j<(y+8); j++ ) {
						if( Main.tile[x, y].wall <= 1 ) {
							Main.tile[x, y].wall = (ushort)( on ? 1 : 0 );
						}
					}
				}
			}

			//

			blackout( true );
			Timers.SetTimer( 6, false, () => {
				blackout( false );
				return false;
			} );
			Timers.SetTimer( 12, false, () => {
				blackout( true );
				return false;
			} );
			Timers.SetTimer( 18, false, () => {
				blackout( false );
				return false;
			} );*/
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
