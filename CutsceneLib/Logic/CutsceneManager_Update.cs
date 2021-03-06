﻿using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Capture;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;


namespace CutsceneLib.Logic {
	public partial class CutsceneManager : ILoadable {
		internal void Update_Internal() {
			// Cleanup unclaimed cutscenes
			foreach( int plrWho in this._CutscenePerPlayer.Keys.ToArray() ) {
				if( Main.netMode != NetmodeID.Server && CutsceneLibConfig.Instance.DebugModeInfo ) {
					Cutscene cutscene = this._CutscenePerPlayer[plrWho];
					DebugHelpers.Print(
						"cutscene_"+plrWho,
						cutscene.UniqueId.ToString()+", "+cutscene?.CurrentScene.UniqueId.ToString()
					);
				}

				if( Main.player[plrWho]?.active != true ) {
					this.EndCutscene( this._CutscenePerPlayer[plrWho].UniqueId, plrWho, false );
				}
			}

			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				Cutscene cutscene = this.GetCurrentCutscene_Player( Main.LocalPlayer );
				cutscene?.UpdateCutscene_Internal();
			} else {
				this.UpdateActivations_Host_Internal();

				foreach( Cutscene cutscene in this._CutscenePerPlayer.Values.ToArray() ) {
					cutscene.UpdateCutscene_Internal();
				}
			}
		}

		private void UpdateActivations_Host_Internal() {
			int playerCount = Main.player.Length;
			
			foreach( CutsceneID cutsceneId in this.CutsceneIDs ) {
				for( int i=0; i<playerCount; i++ ) {
					Player plr = Main.player[i];
					if( plr?.active != true ) {
						continue;
					}

					if( !this.TryBeginCutscene(true, cutsceneId, plr, true, out string result) ) {
						if( CutsceneLibConfig.Instance.DebugModeInfo ) {
							LogHelpers.LogOnce( "Tried to begin cutscene: " + result );
						}
					}

					if( Main.netMode == NetmodeID.SinglePlayer ) {
						break;
					}
				}
			}
		}


		////////////////
		
		internal void Update_Player_Internal( CutsceneLibPlayer myplayer ) {
			Cutscene cutscene = this.GetCurrentCutscene_Player( myplayer.player );
			if( cutscene == null ) {
				return;
			}

			if( cutscene.IsCutscenePlayerImmune_Internal() ) {
				myplayer.player.immune = true;
				myplayer.player.immuneTime = 2;
			}

			if( !CutsceneLibConfig.Instance.DebugModeFreeMove ) {
				if( Main.netMode != NetmodeID.Server ) {
					if( myplayer.player.whoAmI == Main.myPlayer ) {
						//Main.mapFullscreen = false;
						//Main.mapEnabled = false;
						Main.mapStyle = 0;
						CaptureManager.Instance.Active = false;
					}
				}
			}
		}


		////////////////

		internal bool Update_NPC_Internal( NPC npc ) {
			IEnumerable<Cutscene> cutscenes = this.GetActiveCutscenes_World();
			bool allowAI = true;

			foreach( Cutscene cutscene in cutscenes ) {
				if( !cutscene.UpdateNPC_Internal(npc) ) {
					allowAI = false;
				}
			}

			return allowAI;
		}

		internal void Update_NPCFrame_Internal( NPC npc, int frameHeight ) {
			IEnumerable<Cutscene> cutscenes = this.GetActiveCutscenes_World();
			foreach( Cutscene cutscene in cutscenes ) {
				cutscene.Update_NPCFrame_Internal( npc, frameHeight );
			}
		}
	}
}
