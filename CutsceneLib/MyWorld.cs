using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.Logic;


namespace CutsceneLib {
	partial class CutsceneLibWorld : ModWorld {
		internal ISet<CutsceneID> TriggeredCutsceneIDs_World { get; } = new HashSet<CutsceneID>();



		////////////////

		public override void Initialize() {
			this.TriggeredCutsceneIDs_World.Clear();
		}

		////////////////

		public override void Load( TagCompound tag ) {
			CutsceneManager.Instance.Load_World( this, tag );
		}

		public override TagCompound Save() {
			var tag = new TagCompound { };
			CutsceneManager.Instance.Save_World( this, tag );
			return tag;
		}

		////

		public override void NetSend( BinaryWriter writer ) {
			try {
				CutsceneManager.Instance.NetSend_World( this, writer );
			} catch { }
		}

		public override void NetReceive( BinaryReader reader ) {
			try {
				CutsceneManager.Instance.NetReceive_World( this, reader );
			} catch { }
		}
	}
}