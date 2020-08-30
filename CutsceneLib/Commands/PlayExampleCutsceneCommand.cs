using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Logic;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene;


namespace CutsceneLib.Commands {
	/// @private
	public class PlayExampleCutsceneCommand : ModCommand {
		/// @private
		public override CommandType Type {
			get {
				if( Main.netMode == NetmodeID.SinglePlayer && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console | CommandType.World;
			}
		}
		/// @private
		public override string Command => "cl-example";
		/// @private
		public override string Usage => "/" + this.Command;
		/// @private
		public override string Description => "Plays the example cutscene for all players. Can only be played once per world, per character.";



		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				LogHelpers.Warn( "Not supposed to run on client." );
				return;
			}

			var cutMngr = CutsceneManager.Instance;
			var introId = new CutsceneID( CutsceneLibMod.Instance, typeof(IntroCutscene) );

			if( cutMngr.CanBeginCutscene(introId, caller.Player, out string result) ) {
				if( cutMngr.TryBeginCutscene(introId, caller.Player, true, out result) ) {
					caller.Reply( result, Color.Lime );
				} else {
					caller.Reply( result, Color.Yellow );
				}
			} else {
				caller.Reply( result, Color.Red );
			}
		}
	}
}
