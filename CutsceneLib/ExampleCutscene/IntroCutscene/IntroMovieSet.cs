using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.TileStructure;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.World;
using CutsceneLib.Definitions;
using CutsceneLib.ExampleCutscene.IntroCutscene.Net;


namespace CutsceneLib.ExampleCutscene.IntroCutscene {
	partial class IntroMovieSet : MovieSet {
		public static bool GetSceneCoordinates(
					int tileWidth,
					out int leftTile,
					out int topTile,
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

			for( leftTile = topTile = 41; canContinueScan(leftTile, topTile); topTile++ ) { }

			Tile tile = Main.tile[ leftTile, topTile ];
			int oceanTop = topTile;
			topTile -= 18;

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

		public int ExteriorTileLeft;
		public int ExteriorTileTop;
		public int InteriorTileLeft;
		public int InteriorTileTop;
		public int ExteriorDeckWidth;
		public int ExteriorDeckX;
		public int ExteriorDeckY;

		public IList<int> ExteriorCrewNPCs = new List<int>();
		public int ExteriorCrewCaptainNPC;

		public Vector2 ExteriorShipView;
		public Vector2 InteriorShipView;



		////////////////

		private IntroMovieSet( IntroCutsceneNetData data ) {
			this.ExteriorShipView = data.ExteriorShipView;
			this.InteriorShipView = data.InteriorShipView;

			this.ExteriorDeckX = data.ExteriorDeckX;
			this.ExteriorDeckY = data.ExteriorDeckY;
			this.ExteriorDeckWidth = data.ExteriorDeckWidth;
			this.ExteriorCrewNPCs = data.ExteriorCrewNPCs.ToList();
			this.ExteriorCrewCaptainNPC = data.ExteriorCrewNPCs.Last();

			Main.dungeonX = data.DungeonX;
			Main.dungeonY = data.DungeonY;
		}

		private IntroMovieSet( int extTileLeft, int extTileTop, int intTileLeft, int intTileTop ) {
			this.ExteriorTileLeft = extTileLeft;
			this.ExteriorTileTop = extTileTop;
			this.InteriorTileLeft = intTileLeft;
			this.InteriorTileTop = intTileTop;

			this.ExteriorShipView = new Vector2( extTileLeft * 16, extTileTop * 16 );
			this.ExteriorShipView.X += 48f * 16f;
			this.ExteriorShipView.Y += 8f * 16f;

			this.InteriorShipView = new Vector2( intTileLeft * 16, intTileTop * 16 );
			this.InteriorShipView.X += 64f * 16f;
			this.InteriorShipView.Y += 36f * 16f;
		}

		private IntroMovieSet(
					TileStructure shipExterior,
					TileStructure shipInterior,
					int extTileLeft,
					int extTileTop,
					int intTileLeft,
					int intTileTop,
					bool isFlipped
				) : this( extTileLeft, extTileTop, intTileLeft, intTileTop ) {
			shipExterior.PaintToWorld(
				leftTileX: extTileLeft,
				topTileY: extTileTop,
				paintAir: false,
				respectLiquids: true,
				flipHorizontally: isFlipped,
				flipVertically: false
			);
			shipInterior.PaintToWorld(
				leftTileX: intTileLeft,
				topTileY: intTileTop,
				paintAir: false,
				respectLiquids: true,
				flipHorizontally: isFlipped,
				flipVertically: false
			);

			//

			var nonDeckPattern = new TilePattern( new TilePatternBuilder {
				IsNotAnyOfType = new HashSet<int> { TileID.Platforms }
			} );
			
			this.ExteriorDeckWidth = TileFinderHelpers.GetFloorWidth(
				nonFloorPattern: nonDeckPattern,
				tileX: extTileLeft + (shipExterior.Bounds.Width / 2),
				tileY: extTileTop,
				maxFallRange: 50,
				floorX: out this.ExteriorDeckX,
				floorY: out this.ExteriorDeckY
			);

			this.SpawnActors();
		}
	}
}
