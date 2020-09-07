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
			var pirateShipPos = new Vector2(
				( this.Set.ExteriorTileLeft + this.Set.ExteriorDeckWidth + 24 ) * 16,
				(int)this.Set.ExteriorShipView.Y + ( 8 * 16 )
			);

			Main.instance.LoadNPC( NPCID.PirateShip );

			int shipNpcWho = NPC.NewNPC(
				(int)pirateShipPos.X,
				(int)pirateShipPos.Y - ( Main.npcTexture[NPCID.PirateShip].Height / 2 ),
				ModContent.NPCType<PropNPC>()
			);

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
			var plrPos = new Vector2(
				( this.Set.InteriorTileLeft + 48 ) * 16,
				( this.Set.InteriorTileTop + 38 ) * 16
			);

			int x = (int)plrPos.X + (6 * 16);
			int y = (int)plrPos.Y;

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
