using System;
using Terraria;
using Terraria.ID;
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


		////////////////

		public override bool PreAI( NPC npc ) {
			var cutMngr = CutsceneManager.Instance;
			Cutscene nowCutscene = cutMngr?.GetCurrentCutscene_Player( Main.LocalPlayer );
			if( nowCutscene == null ) {
				return base.PreAI( npc );
			}
			
			if( !nowCutscene.AllowNPC(npc) ) {
				npc.active = false;
				npc.life = 0;
			}

			return cutMngr.Update_NPC_Internal( npc );
		}


		////////////////

		public override void FindFrame( NPC npc, int frameHeight ) {
			var cutMngr = CutsceneManager.Instance;
			Cutscene nowCutscene = cutMngr?.GetCurrentCutscene_Player( Main.LocalPlayer );
			if( nowCutscene == null ) {
				return;
			}

			cutMngr.Update_NPCFrame_Internal( npc, frameHeight );
		}
	}
}
