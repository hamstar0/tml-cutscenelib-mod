using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Timers;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates {
	partial class Intro01_PiratesScene
				: Scene<IntroCutscene, IntroMovieSet, IntroCutsceneStartProtocol, IntroCutsceneUpdateProtocol> {
		private void BeginShot02_InteriorChat( IntroCutscene cutscene ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			var plrPos = new Vector2(
				( this.Set.InteriorTileLeft + 48 ) * 16,
				( this.Set.InteriorTileTop + 38 ) * 16
			);

			Player plr = Main.player[ cutscene.PlaysForWhom ];
			plr.position = plrPos;

			//

			int x = (int)plrPos.X + (56 * 16);
			int y = (int)plrPos.Y;

			int npcWho = NPC.NewNPC( x, y, NPCID.Guide );
			Main.npc[npcWho].friendly = true;
			Main.npc[npcWho].color = new Color(
				Main.rand.Next( 192, 255 ),
				Main.rand.Next( 192, 255 ),
				Main.rand.Next( 192, 255 )
			);

			this.Set.InteriorCrewNPC = npcWho;

			if( Main.netMode != NetmodeID.SinglePlayer ) {
				NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, npcWho );
			}

			//

			Timers.SetTimer( (int)(60f * 1.5f), false, () => {
				var cam = new CameraShaker(
					name: "CutsceneLib_Intro_Pirates_Shake_2",
					peakMagnitude: 1f,
					toDuration: 0,
					lingerDuration: 0,
					froDuration: 60,
					onStop: () => { }
				);
				return false;
			} );

			//

			this.IsSiezingControls = false;
			this.IsCutscenePlayerImmune = false;

			//

			int guideWho = Main.npc.FirstOrDefault( n => n.type == NPCID.Guide ).whoAmI;

			this.Dialogue.ShowDialogue();
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
				lingerDuration: 60 * 5,
				froDuration: 0,
				onStop: () => {
					onCamStop?.Invoke();
					//CameraMover.Current = cams[next + 1];
				}
			);

			cams.Add( cam );
		}


		////////////////

		private void UpdateNPC02_InteriorChat( IntroCutscene cutscene ) {

			// hold player + guides
		}
	}
}
