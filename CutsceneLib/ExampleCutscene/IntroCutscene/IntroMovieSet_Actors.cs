using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;


namespace CutsceneLib.ExampleCutscene.IntroCutscene {
	partial class IntroMovieSet : MovieSet {
		private void SpawnActors() {
			int x = this.ExteriorDeckX;
			int y = this.ExteriorDeckY - 2;

			for( int i=0; i<4; i++ ) {
				x += this.ExteriorDeckWidth / 5;
				int npcWho = NPC.NewNPC( x * 16, y * 16, NPCID.Guide );
				Main.npc[npcWho].color = new Color( Main.rand.Next(255), Main.rand.Next(255), Main.rand.Next(255) );

				this.ExteriorCrewNPCs.Add( npcWho );
				if( i >= 3 ) {
					this.ExteriorCrewCaptainNPC = npcWho;
				}

				if( Main.netMode != NetmodeID.SinglePlayer ) {
					NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, npcWho );
				}
			}
		}
	}
}
