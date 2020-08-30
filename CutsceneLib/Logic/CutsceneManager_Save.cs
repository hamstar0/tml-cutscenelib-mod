using System;
using System.IO;
using Terraria;
using Terraria.ModLoader.IO;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;


namespace CutsceneLib.Logic {
	public partial class CutsceneManager : ILoadable {
		internal void Load_World( CutsceneLibWorld myworld, TagCompound tag ) {
			this.ResetCutscenes();

			myworld.TriggeredCutsceneIDs_World.Clear();

			if( !tag.ContainsKey( "TriggeredCutscenesCount" ) ) {
				return;
			}
			int count = tag.GetInt( "TriggeredCutscenesCount" );

			for( int i = 0; i < count; i++ ) {
				string modName = tag.GetString( "TriggeredCutsceneMod_"+i );
				string className = tag.GetString( "TriggeredCutsceneName_"+i );
				var uid = new CutsceneID( modName, className );

				myworld.TriggeredCutsceneIDs_World.Add( uid );
			}
		}

		internal void Save_World( CutsceneLibWorld myworld, TagCompound tag ) {
			int count = myworld.TriggeredCutsceneIDs_World.Count;
			tag["TriggeredCutscenesCount"] = count;

			int i = 0;
			foreach( CutsceneID uid in myworld.TriggeredCutsceneIDs_World ) {
				tag["TriggeredCutsceneMod_" + i] = uid.ModName;
				tag["TriggeredCutsceneName_" + i] = uid.FullClassName;
				i++;
			}
		}

		////

		internal void NetSend_World( CutsceneLibWorld myworld, BinaryWriter writer ) {
			writer.Write( (int)myworld.TriggeredCutsceneIDs_World.Count );

			foreach( CutsceneID uid in myworld.TriggeredCutsceneIDs_World ) {
				writer.Write( uid.ModName );
				writer.Write( uid.FullClassName );
			}
		}

		internal void NetReceive_World( CutsceneLibWorld myworld, BinaryReader reader ) {
			myworld.TriggeredCutsceneIDs_World.Clear();

			int count = reader.ReadInt32();

			for( int i=0; i<count; i++ ) {
				string modName = reader.ReadString();
				string className = reader.ReadString();
				var uid = new CutsceneID( modName, className );

				myworld.TriggeredCutsceneIDs_World.Add( uid );
			}
		}


		////////////////

		internal void Load_Player( CutsceneLibPlayer myplayer, TagCompound tag ) {
			myplayer.TriggeredCutsceneIDs_Player.Clear();

			if( !tag.ContainsKey("TriggeredCutscenesCount") ) {
				return;
			}
			int count = tag.GetInt( "TriggeredCutscenesCount" );

			for( int i=0; i<count; i++ ) {
				string modName = tag.GetString( "TriggeredCutsceneMod_" + i );
				string className = tag.GetString( "TriggeredCutsceneName_" + i );
				var uid = new CutsceneID( modName, className );

				myplayer.TriggeredCutsceneIDs_Player.Add( uid );
			}
		}

		internal void Save_Player( CutsceneLibPlayer myplayer, TagCompound tag ) {
			int count = myplayer.TriggeredCutsceneIDs_Player.Count;
			tag["TriggeredCutscenesCount"] = count;

			int i = 0;
			foreach( CutsceneID uid in myplayer.TriggeredCutsceneIDs_Player ) {
				tag["TriggeredCutsceneMod_" + i] = uid.ModName;
				tag["TriggeredCutsceneName_" + i] = uid.FullClassName;
				i++;
			}
		}
	}
}
