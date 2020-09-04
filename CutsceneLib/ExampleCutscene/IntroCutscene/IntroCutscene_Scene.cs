﻿using System;
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
		protected override SceneBase CreateScene( SceneID sceneId ) {
			if( sceneId.Equals(this.FirstSceneId) ) {
				return this.CreateIntroScene();
			} else if( sceneId.Equals(this.SecondSceneId) ) {
				return Intro01_PiratesScene.Create( this );
			}

			return null;
		}

		////

		protected override SceneBase CreateSceneFromNetwork( SceneID sceneId, CutsceneNetStart data ) {
			if( sceneId.Equals(this.FirstSceneId) ) {
				return this.CreateIntroSceneFromNetwork( data );
			} else if( sceneId.Equals( this.SecondSceneId ) ) {
				return Intro01_PiratesScene.Create( this );
			}

			return null;
		}


		////////////////

		private SceneBase CreateIntroScene() {
			var set = IntroMovieSet.Create( ref this._ShipExterior, ref this._ShipInterior, out _, out string __ );
			if( set != null ) {
				return new Intro00_SettingScene( set );
			}

			return null;
		}

		////

		private SceneBase CreateIntroSceneFromNetwork( CutsceneNetStart data ) {
			IntroMovieSet set = IntroMovieSet.Create(
				ref this._ShipExterior,
				ref this._ShipInterior,
				out Rectangle chunkRange,
				out string result
			);
			if( set != null ) {
				return new Intro00_SettingScene( set );
			}

			if( result != "Found null tile." ) {
				set = IntroMovieSet.Create( (IntroCutsceneNetData)data );
				return new Intro00_SettingScene( set );
			}

			// Request and await tile chunks from server
			if( this._MovieSetChunksRequestRetryTimer-- <= 0 ) {
				this._MovieSetChunksRequestRetryTimer = 60 * 2;	// Retry every 2s until timeout

				LogHelpers.Log( "Requesting chunks from range " + chunkRange );
				TileWorldHelpers.RequestChunksFromServer( chunkRange );
			}

			return null;
		}
	}
}
