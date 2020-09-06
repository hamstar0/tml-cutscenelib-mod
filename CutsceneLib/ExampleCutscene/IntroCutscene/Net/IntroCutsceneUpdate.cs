using System;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene.Net {
	[Serializable]
	class IntroCutsceneUpdateProtocol : CutsceneUpdateProtocol {
		public int InteriorCrewNPC;
		public int ShipPropNPC;



		////////////////

		private IntroCutsceneUpdateProtocol() : base() { }
		
		public IntroCutsceneUpdateProtocol( IntroCutscene cutscene, IntroMovieSet set ) : base( cutscene, set ) {
			this.InteriorCrewNPC = set.InteriorCrewNPC;
			this.ShipPropNPC = set.ShipPropNPC;
		}
	}
}
