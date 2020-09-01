using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Net {
	[Serializable]
	class IntroCutsceneNetData : CutsceneNetStart {
		public Vector2 ExteriorShipView;
		public Vector2 InteriorShipView;
		public int DungeonX;
		public int DungeonY;



		////////////////

		private IntroCutsceneNetData() : base() { }
		
		public IntroCutsceneNetData( IntroCutscene cutscene, IntroMovieSet set ) : base( cutscene ) {
			this.ExteriorShipView = set.ExteriorShipView;
			this.InteriorShipView = set.InteriorShipView;
			this.DungeonX = Main.dungeonX;
			this.DungeonY = Main.dungeonY;
		}
	}
}
