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

		private IntroMovieSet( int extLeft, int extTop, int intLeft, int intTop ) {
			this.ExteriorTileLeft = extLeft;
			this.ExteriorTileTop = extTop;
			this.InteriorTileLeft = intLeft;
			this.InteriorTileTop = intTop;

			this.ExteriorShipView = new Vector2( extLeft * 16, extTop * 16 );
			//this.ExteriorShipView.X += 28f * 16f;
			//this.ExteriorShipView.Y -= 12f * 16f;
			this.InteriorShipView = new Vector2( intLeft * 16, intTop * 16 );
			//this.InteriorShipView.X += 40f * 16f;
			this.InteriorShipView.X += 20f * 16f;
			this.InteriorShipView.Y += 24f * 16f;
		}

		private IntroMovieSet(
					TileStructure shipExterior,
					TileStructure shipInterior,
					int extLeft,
					int extTop,
					int intLeft,
					int intTop,
					bool isFlipped
				) : this( extLeft, extTop, intLeft, intTop ) {
			shipExterior.PaintToWorld(
				leftTileX: extLeft,
				topTileY: extTop,
				paintAir: false,
				respectLiquids: true,
				flipHorizontally: isFlipped,
				flipVertically: false
			);
			shipInterior.PaintToWorld(
				leftTileX: intLeft,
				topTileY: intTop,
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
				tileX: extLeft + (shipExterior.Bounds.Width / 2),
				tileY: extTop,
				maxFallRange: 50,
				floorX: out this.ExteriorDeckX,
				floorY: out this.ExteriorDeckY
			);

			this.SpawnActors();
		}
	}
}
