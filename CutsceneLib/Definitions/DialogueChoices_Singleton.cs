using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.UI.FreeHUD;


namespace CutsceneLib.Definitions {
	public partial class DialogueChoices : ILoadable {
		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() {
			if( Main.netMode != NetmodeID.Server ) {
				this.DialogueElem = new UIThemedPanel( UITheme.Vanilla, false );
				this.DialogueElem.Width.Set( 560f, 0f );
				this.DialogueElem.Height.Set( 160f, 0f );
				this.DialogueElem.Left.Set( -280f, 0.5f );
				this.DialogueElem.Top.Set( 96f, 0f );
				
				this.PortraitElem = new UIImage( ModContent.GetTexture("Terraria/MapDeath") );
				this.PortraitElem.Width.Set( 128f, 0f );
				this.PortraitElem.Height.Set( 128f, 0f );
				this.PortraitElem.Left.Set( 12f, 0f );
				this.PortraitElem.Top.Set( 12f, 0f );
				this.DialogueElem.AppendThemed( this.PortraitElem );

				this.TextElem = new UIThemedText( UITheme.Vanilla, false, "" );
				this.TextElem.Width.Set( -128f, 1f );
				this.TextElem.Left.Set( 12f+128f, 0f );
				this.TextElem.Top.Set( 12f, 0f );
				this.DialogueElem.AppendThemed( this.TextElem );

				this.DialogueElem.Hide();

				FreeHUD.AddElement( "CutsceneDialogue", this.DialogueElem );
			}
		}

		void ILoadable.OnModsUnload() { }
	}
}
