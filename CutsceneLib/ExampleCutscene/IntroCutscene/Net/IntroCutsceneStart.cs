using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Net {
	[Serializable]
	class IntroCutsceneStartProtocol : CutsceneStartProtocol {
		public Vector2 ExteriorShipView;
		public Vector2 InteriorShipView;
		public int ExteriorDeckWidth;
		public int ExteriorDeckX;
		public int ExteriorDeckY;

		public int[] ExteriorCrewNPCs;

		public int DungeonX;
		public int DungeonY;



		////////////////

		private IntroCutsceneStartProtocol() : base() { }
		
		public IntroCutsceneStartProtocol( IntroCutscene cutscene, IntroMovieSet set ) : base( cutscene, set ) {
			this.ExteriorShipView = set.ExteriorShipView;
			this.InteriorShipView = set.InteriorShipView;
			this.ExteriorDeckWidth = set.ExteriorDeckWidth;
			this.ExteriorDeckX = set.ExteriorDeckX;
			this.ExteriorDeckY = set.ExteriorDeckY;

			this.ExteriorCrewNPCs = set.ExteriorCrewNPCs.ToArray();

			this.DungeonX = Main.dungeonX;
			this.DungeonY = Main.dungeonY;
		}
	}
}
