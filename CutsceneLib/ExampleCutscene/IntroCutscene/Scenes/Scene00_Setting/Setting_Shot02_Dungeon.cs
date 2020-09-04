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
	partial class Intro00_SettingScene : Scene<IntroCutscene, IntroMovieSet, IntroCutsceneNetData> {
		private void BeginShot02_Dungeon() { }


		////////////////

		private void GetCam02_Dungeon( IList<CameraMover> cams, Action onCamStop, bool isShipOnLeft ) {
			var dungeonView = new Vector2( Main.dungeonX * 16, Main.dungeonY * 16 );
			dungeonView.X += isShipOnLeft
				? 0
				: (32 * 16);
			//dungeonView.Y += -32 * 16;

			int next = cams.Count;
			
			var cam = new CameraMover(
				name: "CutsceneLib_Intro_Setting_" + cams.Count,
				moveXFrom: (int)dungeonView.X,
				moveYFrom: (int)dungeonView.Y,
				moveXTo: (int)dungeonView.X,
				moveYTo: (int)dungeonView.Y - ( 4 * 16 ),
				toDuration: 60 * 5,
				lingerDuration: 0,
				froDuration: 0,
				onStop: () => {
					onCamStop?.Invoke();
					//CameraMover.Current = cams[next + 1];
				}
			);

			cams.Add( cam );
		}


		////////////////

		private bool UpdateNPC02_DungeonView( NPC npc ) {
			if( npc.type == NPCID.OldMan ) {
				npc.ai[0] = 0f;
				npc.ai[1] = 0f;
				npc.ai[2] = 0f;
				npc.ai[3] = 0f;
				npc.velocity = new Vector2( 0.5f, 0f );
				return false;
			}
			return true;
		}
	}
}
