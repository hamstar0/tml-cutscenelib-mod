using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using HamstarHelpers.Classes.TileStructure;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.World;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene {
	class IntroMovieSet : MovieSet {
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
			intTop = Math.Max( intTop - 100, 40 );

			if( !isOcean ) {
				chunkRange = new Rectangle(
					intLeft,
					41,
					Math.Max( shipExterior.Bounds.Width, shipInterior.Bounds.Width ),
					shipExterior.Bounds.Height + shipInterior.Bounds.Height + 20
				);
				return null;
			}

			chunkRange = default( Rectangle );
			return new IntroMovieSet( shipExterior, shipInterior, extLeft, extTop, intLeft, intTop, isFlipped );
		}


		internal static IntroMovieSet Create( IntroCutsceneNetData data ) {
			return new IntroMovieSet( data );
		}


		////////////////

		public static bool GetSceneCoordinates(
					int width,
					out int left,
					out int top,
					out bool isFlipped,
					out string result ) {
			bool canContinueScan( int x, int y ) {
				Tile mytile = Main.tile[x, y];
				if( mytile == null ) {		// halt checking if non-existent
					return false;
				}
				if( mytile.liquid != 0 ) {	// halt on liquid; success!
					return false;
				}
				if( mytile.active() ) {		// halt on ("active") solid
					return false;
				}
				if( y >= WorldHelpers.SurfaceLayerBottomTileY ) {	// halt when out of range
					return false;
				}
				return true;
			}

			//

			isFlipped = false;
			/*//isFlipped = Main.spawnTileX > ( Main.maxTilesX / 2 );

			if( isFlipped ) {
				left = ( Main.maxTilesX - 41 ) - width;
				if( ( left % 2 ) == 0 ) {
					left++;
				}
			} else {
				left = 41;
			}*/

			for( left = top = 41; canContinueScan(left, top); top++ ) { }

			Tile tile = Main.tile[left, top];
			int oceanTop = top;
			top -= 18;

			bool isOcean = tile != null								// tile exists
				&& tile.liquid != 0									// tile has liquid
				&& !tile.active()									// tile is non-solid
				&& oceanTop < WorldHelpers.SurfaceLayerBottomTileY;	// tile is within range

			if( isOcean ) {
				result = "Success.";
			} else {
/*LogHelpers.LogOnce( "x:"+left+", y: "+oceanTop
	+", isn't null? " + (tile != null)
	+", has liquid? "+(tile?.liquid != 0)
	+", isn't active? "+(tile == null || !tile.active())
	+", above ground? "+(oceanTop < WorldHelpers.SurfaceLayerBottomTileY) );*/
				if( tile == null ) {
					result = "Found null tile.";
				} else if( tile.active() ) {
					result = "Found active tile.";
				} else if( oceanTop >= WorldHelpers.SurfaceLayerBottomTileY ) {
					result = "Too deep.";
				} else {
					result = "???";
				}
			}

			return isOcean;
		}



		////////////////

		public Vector2 ExteriorShipView;
		public Vector2 InteriorShipView;



		////////////////

		private IntroMovieSet( IntroCutsceneNetData data ) {
			this.ExteriorShipView = data.ExteriorShipView;
			this.InteriorShipView = data.InteriorShipView;
		}

		private IntroMovieSet(
					int extWid,
					int extLeft,
					int extTop,
					int intWid,
					int intLeft,
					int intTop ) {
			this.ExteriorShipView = new Vector2( extLeft * 16, extTop * 16 );
			this.ExteriorShipView.X += extWid * 8;    // (wid*16) / 2
			this.ExteriorShipView.Y += -8 * 16;

			this.InteriorShipView = new Vector2( intLeft * 16, intTop * 16 );
			this.InteriorShipView.X += intWid * 8;    // (wid*16) / 2
			this.InteriorShipView.Y += 32 * 16;
		}

		private IntroMovieSet(
					TileStructure shipExterior,
					TileStructure shipInterior,
					int extLeft,
					int extTop,
					int intLeft,
					int intTop,
					bool isFlipped ) : this(
						shipExterior.Bounds.Width,
						extLeft,
						extTop,
						shipInterior.Bounds.Width,
						intLeft,
						intTop ) {
			shipExterior.PaintToWorld(
				leftTileX: extLeft,
				topTileY: extTop,
				paintAir: false,
				respectLiquids: true,
				flipHorizontally: isFlipped,
				flipVertically: false );

			shipInterior.PaintToWorld(
				leftTileX: intLeft,
				topTileY: intTop,
				paintAir: false,
				respectLiquids: true,
				flipHorizontally: isFlipped,
				flipVertically: false );
		}
	}
}
