using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.NPCs;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates {
	partial class Intro01_PiratesScene
				: Scene<IntroCutscene, IntroMovieSet, IntroCutsceneStartProtocol, IntroCutsceneUpdateProtocol> {
		private void PrepareMovieSet() {
			this.Set.ShipPropNPC = this.PreparePirateShip();
			this.Set.InteriorCrewNPC = this.PrepareInteriorCrew();
		}


		////

		private int PreparePirateShip() {
			Main.instance.LoadNPC( NPCID.PirateShip );

			int x = (this.Set.ExteriorTileLeft + this.Set.ExteriorDeckWidth + 48) * 16;
			int y = (int)this.Set.ExteriorShipView.Y + (15 * 16);
			y -= Main.npcTexture[NPCID.PirateShip].Height / 2;

			int shipNpcWho = NPC.NewNPC( x, y, ModContent.NPCType<PropNPC>() );
			NPC ship = Main.npc[shipNpcWho];
			ship.direction = -1;

			var myship = ship.modNPC as PropNPC;
			myship.SetTexture( "Terraria/NPC_" + NPCID.PirateShip );

			if( Main.netMode != NetmodeID.SinglePlayer ) {
				NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, shipNpcWho );
			}

			return shipNpcWho;
		}


		private int PrepareInteriorCrew() {
			int x = (this.Set.InteriorTileLeft + 54) * 16;
			int y = (this.Set.InteriorTileTop + 40) * 16;

			int npcWho = NPC.NewNPC( x, y, NPCID.Guide );
			Main.npc[npcWho].friendly = true;
			Main.npc[npcWho].direction = -1;
			Main.npc[npcWho].color = new Color(
				Main.rand.Next( 192, 255 ),
				Main.rand.Next( 192, 255 ),
				Main.rand.Next( 192, 255 )
			);

			if( Main.netMode != NetmodeID.SinglePlayer ) {
				NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, npcWho );
			}

			return npcWho;
		}
	}
}
