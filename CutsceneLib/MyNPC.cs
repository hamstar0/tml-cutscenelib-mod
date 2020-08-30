using System;
using Terraria;
using Terraria.ModLoader;
using CutsceneLib.Logic;
using CutsceneLib.Definitions;


namespace CutsceneLib {
	class CutsceneLibNPC : GlobalNPC {
		/*public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			Cutscene nowCutscene = CutsceneManager.Instance?.GetCurrentPlayerCutscene( Main.LocalPlayer );
			if( nowCutscene == null ) {
				return;
			}

			pool.Clear();
		}

		public override void EditSpawnRate( Player player, ref int spawnRate, ref int maxSpawns ) {
			Cutscene nowCutscene = CutsceneManager.Instance?.GetCurrentPlayerCutscene( Main.LocalPlayer );
			if( nowCutscene == null ) {
				return;
			}

			maxSpawns = 0;
		}*/


		////

		public override bool PreAI( NPC npc ) {
			Cutscene nowCutscene = CutsceneManager.Instance?.GetCurrentCutscene_Player( Main.LocalPlayer );
			if( nowCutscene == null ) {
				return base.PreAI( npc );
			}
			
			if( !nowCutscene.AllowNPC(npc) ) {
				npc.active = false;
				npc.life = 0;
			}
			
			return base.PreAI( npc );
		}
	}
}
