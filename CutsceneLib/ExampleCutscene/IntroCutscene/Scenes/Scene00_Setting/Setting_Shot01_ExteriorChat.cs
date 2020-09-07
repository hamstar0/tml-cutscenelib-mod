using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.UI;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene00_Setting {
	partial class Intro00_SettingScene
				: Scene<IntroCutscene, IntroMovieSet, IntroCutsceneStartProtocol, IntroCutsceneUpdateProtocol> {
		private void BeginShot01_ExteriorChat() {

		}


		////////////////
		
		private void GetCam01_ExteriorChat(
					IList<CameraMover> cams,
					Action onCamStop ) {
			Vector2 exteriorShipView = this.Set.ExteriorShipView;
			int next = cams.Count;
			
			var cam = new CameraMover(
				name: "CutsceneLib_Intro_Setting_" + cams.Count,
				moveXFrom: (int)exteriorShipView.X,
				moveYFrom: (int)exteriorShipView.Y,
				moveXTo: (int)exteriorShipView.X,
				moveYTo: (int)exteriorShipView.Y - (12 * 16),
				toDuration: 60 * 3,
				lingerDuration: 60 * 3,
				froDuration: 0,
				onStop: () => {
					onCamStop?.Invoke();
					CameraMover.Current = cams[next + 1];
				}
			);

			cams.Add( cam );
		}


		////////////////
		
		private bool UpdateNPC01_ExteriorChat( NPC npc ) {
			if( npc.whoAmI == this.Set.ExteriorCrewCaptainNPC ) {
				npc.ai[0] = 0f;
				npc.ai[1] = 0f;
				npc.ai[2] = 0f;
				npc.ai[3] = 0f;
				npc.direction = 1;
				npc.velocity = default( Vector2 );
				return false;
			}
			return true;
		}


		////////////////

		private void UpdateNPCFrame01_ExteriorChat( NPC npc, int frameHeight ) {
			if( npc.whoAmI == this.Set.ExteriorCrewCaptainNPC ) {
				npc.frame.Y = frameHeight * 22;
			}
		}


		////////////////

		private void DrawInterface01_ExteriorChat() {
			NPC captain = Main.npc[ this.Set.ExteriorCrewCaptainNPC ];
			var binocPos = captain.Center + new Vector2( 6, -9 );
			binocPos = UIZoomHelpers.ConvertToScreenPosition( binocPos, null, null );

			Main.spriteBatch.Draw(
				texture: Main.itemTexture[ ItemID.Flare ],
				position: binocPos,
				sourceRectangle: null,
				color: Color.White,
				rotation: MathHelper.ToRadians(270),
				origin: default(Vector2),
				scale: 1f,
				effects: SpriteEffects.None,
				layerDepth: 1f
			);
		}
	}
}
