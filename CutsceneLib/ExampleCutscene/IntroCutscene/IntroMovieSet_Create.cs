using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.TileStructure;
using HamstarHelpers.Helpers.Debug;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene {
	partial class IntroMovieSet : MovieSet {
		internal static IntroMovieSet Create(
					ref TileStructure shipExterior,
					ref TileStructure shipInterior,
					out Rectangle chunkRange,
					out string result ) {
			if( shipInterior == null ) {
				char d = Path.DirectorySeparatorChar;
				shipExterior = TileStructure.Load(
					mod: CutsceneLibMod.Instance,
					pathOfModFile: "ExampleCutscene" + d + "IntroCutscene" + d + "Ship Exterior.dat"
				);
				shipInterior = TileStructure.Load(
					mod: CutsceneLibMod.Instance,
					pathOfModFile: "ExampleCutscene" + d + "IntroCutscene" + d + "Ship Interior.dat"
				);
				//LogHelpers.Log( "interior: "+ shipInterior.Bounds.ToString()+" ("+shipInterior.TileCount+")"
				//	+", exterior: "+shipExterior.Bounds.ToString()+" ("+shipExterior.TileCount+")");
			}

			int extLeft, extTop;
			int intLeft, intTop;
			bool isFlipped;
			bool isOcean;

			isOcean = IntroMovieSet.GetSceneCoordinates( shipExterior.Bounds.Width, out extLeft, out extTop, out isFlipped, out result );
			isOcean = IntroMovieSet.GetSceneCoordinates( shipInterior.Bounds.Width, out intLeft, out intTop, out isFlipped, out result );

			if( !isOcean ) {
				chunkRange = new Rectangle(
					intLeft,
					41,
					Math.Max( shipExterior.Bounds.Width, shipInterior.Bounds.Width ),
					shipExterior.Bounds.Height + shipInterior.Bounds.Height + 20
				);
				return null;
			}

			//extLeft += shipExterior.Bounds.Width / 2;
			//intLeft += shipInterior.Bounds.Width / 2;
			//extTop -= 8;
			intTop = Math.Max( intTop - 160, 41 );

			chunkRange = default( Rectangle );
			return new IntroMovieSet( shipExterior, shipInterior, extLeft, extTop, intLeft, intTop, isFlipped );
		}


		internal static IntroMovieSet Create( IntroCutsceneNetData data ) {
			return new IntroMovieSet( data );
		}
	}
}
