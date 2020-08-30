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
		public bool IsThisWorldCutsceneLibCompat { get; private set; } = false;

		////

		internal ISet<CutsceneID> TriggeredCutsceneIDs_World { get; } = new HashSet<CutsceneID>();



		////////////////

		public override void Initialize() {
			this.IsThisWorldCutsceneLibCompat = false;
			this.TriggeredCutsceneIDs_World.Clear();
		}


		////////////////

		public override void PostWorldGen() {
			LogHelpers.Log( "World "+Main.worldName+" prepped for Cutscene Lib." );
			this.IsThisWorldCutsceneLibCompat = true;
		}

		////////////////

		public override void Load( TagCompound tag ) {
			if( tag.ContainsKey( "IsThisWorldCutsceneLibCompat" ) ) {
				this.IsThisWorldCutsceneLibCompat = tag.GetBool( "IsThisWorldCutsceneLibCompat" );
				CutsceneManager.Instance.Load_World( this, tag );
			}
		}

		public override TagCompound Save() {
			var tag = new TagCompound {
				{ "IsThisWorldCutsceneLibCompat", this.IsThisWorldCutsceneLibCompat },
			};
			CutsceneManager.Instance.Save_World( this, tag );
			return tag;
		}

		////

		public override void NetSend( BinaryWriter writer ) {
			try {
				writer.Write( this.IsThisWorldCutsceneLibCompat );
				CutsceneManager.Instance.NetSend_World( this, writer );
			} catch { }
		}

		public override void NetReceive( BinaryReader reader ) {
			try {
				this.IsThisWorldCutsceneLibCompat = reader.ReadBoolean();
				CutsceneManager.Instance.NetReceive_World( this, reader );
			} catch { }
		}
	}
}