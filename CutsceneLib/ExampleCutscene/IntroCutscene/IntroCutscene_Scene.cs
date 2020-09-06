using System;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using CutsceneLib.Definitions;
using CutsceneLib.Net;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;
using CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene00_Setting;
using CutsceneLib.ExampleCutscene.IntroCutscene.Scenes.Scene01_Pirates;


namespace CutsceneLib.ExampleCutscene.IntroCutscene {
	partial class IntroCutscene : Cutscene {
		protected override SceneBase CreateIntroScene() {
			var set = IntroMovieSet.Create( ref this._ShipExterior, ref this._ShipInterior, out _, out string __ );
			if( set == null ) {
				return null;
			}

			return new Intro00_SettingScene( set );
		}

		protected override SceneBase CreateIntroSceneFromNetwork( CutsceneStartProtocol netData ) {
			var set = IntroMovieSet.Create(
				ref this._ShipExterior,
				ref this._ShipInterior,
				out Rectangle chunkRange,
				out string result
			);
			if( set != null ) {
				return new Intro00_SettingScene( set );
			}

			if( result != "Found null tile." ) {    //result == "Success."
				set = IntroMovieSet.Create( (IntroCutsceneStartProtocol)netData );
				return new Intro00_SettingScene( set );
			}

			// Request and await tile chunks from server
			if( this._MovieSetChunksRequestRetryTimer-- <= 0 ) {
				this._MovieSetChunksRequestRetryTimer = 60 * 2; // Retry every 2s until timeout

				LogHelpers.Log( "Requesting chunks from range " + chunkRange );
				TileWorldHelpers.RequestChunksFromServer( chunkRange );
			}

			return null;
		}


		////////////////

		protected override SceneBase CreateNextScene( SceneID sceneId ) {
			var currScene = this.CurrentScene as Intro00_SettingScene;
			IntroMovieSet set = currScene.Set;

			if( sceneId.Equals(this.FirstSceneId) ) {
				return new Intro00_SettingScene( set );
			} else if( sceneId.Equals( this.SecondSceneId ) ) {
				return Intro01_PiratesScene.Create( this );
			}

			return null;
		}

		protected override SceneBase CreateNextSceneFromNetwork( SceneID sceneId, CutsceneUpdateProtocol netData ) {
			IntroMovieSet set = this.UpdateMovieSetFromNetwork( netData );

			if( sceneId.Equals(this.FirstSceneId) ) {
				return new Intro00_SettingScene( set );
			} else if( sceneId.Equals( this.SecondSceneId ) ) {
				return Intro01_PiratesScene.Create( this );
			}

			return null;
		}


		////////////////

		private IntroMovieSet UpdateMovieSetFromNetwork( CutsceneUpdateProtocol netData ) {
			var currScene = this.CurrentScene as Intro00_SettingScene;
			var myNetData = netData as IntroCutsceneUpdateProtocol;

			currScene.Set.InteriorCrewNPC = myNetData?.InteriorCrewNPC ?? -1;
			currScene.Set.ShipPropNPC = myNetData?.ShipPropNPC ?? -1;

			return currScene.Set;
		}
	}
}
