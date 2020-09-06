using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene00_Setting {
	partial class Intro00_SettingScene
				: Scene<IntroCutscene, IntroMovieSet, IntroCutsceneStartProtocol, IntroCutsceneUpdateProtocol> {
		private void BeginShot00_Title( IntroCutscene cutscene ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			int x = this.Set.ExteriorDeckX;
			int y = this.Set.ExteriorDeckY - 2;

			for( int i = 0; i < 4; i++ ) {
				x += this.Set.ExteriorDeckWidth / 5;
				int npcWho = NPC.NewNPC( x * 16, y * 16, NPCID.Guide );

				Main.npc[npcWho].friendly = true;
				Main.npc[npcWho].color = new Color(
					Main.rand.Next( 192, 255 ),
					Main.rand.Next( 192, 255 ),
					Main.rand.Next( 192, 255 )
				);

				this.Set.ExteriorCrewNPCs.Add( npcWho );
				if( i >= 3 ) {
					this.Set.ExteriorCrewCaptainNPC = npcWho;
				}

				if( Main.netMode != NetmodeID.SinglePlayer ) {
					NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, npcWho );
				}
			}
		}


		////////////////

		private void GetCam00_Title( IList<CameraMover> cams, Action onCamStop ) {
			int next = cams.Count;
			int worldX = (Main.maxTilesX - 40) - (Main.screenWidth / 16);
			worldX *= 16;
			int worldY = 40 * 16;

			var cam = new CameraMover(
				name: "CutsceneLib_Intro_Setting_" + cams.Count,
				moveXFrom: worldX,
				moveYFrom: worldY,
				moveXTo: worldX,
				moveYTo: worldY,
				toDuration: 0,
				lingerDuration: 10,//60 * 5,
				froDuration: 0,
				onStop: () => {
					onCamStop?.Invoke();
					CameraMover.Current = cams[next + 1];
				}
			);

			cams.Add( cam );
		}


		////////////////
		
		private void DrawInterface00_Title() {
			string titleText = "Test Title";
			Vector2 titleDim = Main.fontMouseText.MeasureString( titleText );
			var pos = new Vector2( (Main.screenWidth / 2) - titleDim.X, (Main.screenHeight / 2) - titleDim.Y );

			Utils.DrawBorderStringFourWay(
				sb: Main.spriteBatch,
				font: Main.fontMouseText,
				text: titleText,
				x: pos.X,
				y: pos.Y,
				textColor: Color.White,
				borderColor: Color.Black,
				origin: default(Vector2),
				scale: 2f
			);
		}
	}
}
