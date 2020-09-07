using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
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
		private UIThemedPanel DialogueElem = null;
		private UIImage PortraitElem = null;
		private UIThemedText TextElem = null;

		private IList<UITextPanelButton> ChoiceButtons = new List<UITextPanelButton>();



		////////////////

		private DialogueChoices() { }

		public DialogueChoices( string dialogue, Texture2D portrait, int height )
			: this( dialogue, portrait, height, new List<string>(), null ) { }

		public DialogueChoices(
					string dialogue,
					Texture2D portrait,
					int height,
					IList<string> choices,
					Action<string> onChoice ) {
			if( Main.netMode == NetmodeID.Server ) {
				return;
			}

			var singleton = ModContent.GetInstance<DialogueChoices>();
			this.DialogueElem = singleton.DialogueElem;
			this.PortraitElem = singleton.PortraitElem;
			this.TextElem = singleton.TextElem;

			this.TextElem.SetText( dialogue );
			this.PortraitElem.SetImage( portrait );
			this.DialogueElem.Height.Set( height, 0f );

			int i = 0;
			foreach( string choice in choices ) {
				string thisChoice = choice;
				var choiceElem = new UITextPanelButton( UITheme.Vanilla, thisChoice );
				choiceElem.Left.Set( i * 96f, 0f );
				choiceElem.Top.Set( -40f, 1f );
				choiceElem.Width.Set( 96f, 0f );
				choiceElem.OnClick += (_, __) => {
					onChoice( thisChoice );
				};
				this.DialogueElem.AppendThemed( choiceElem );

				this.ChoiceButtons.Add( choiceElem );
				i++;
			}

			this.DialogueElem.Recalculate();
			this.DialogueElem.Show();
			this.DialogueElem.Recalculate();
		}


		////////////////
		
		public void HideDialogue() {
			var panel = FreeHUD.GetElement( "CutsceneDialogue" ) as UIThemedPanel;
			panel?.Hide();

			this.TextElem?.SetText( "" );

			foreach( UITextPanelButton button in this.ChoiceButtons ) {
				this.DialogueElem.RemoveChild( button );
				button.Remove();
			}
			this.ChoiceButtons.Clear();
		}
	}
}
