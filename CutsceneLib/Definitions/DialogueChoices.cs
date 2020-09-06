using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.UI.FreeHUD;


namespace CutsceneLib.Definitions {
	public class DialogueChoices : ILoadable {
		protected UIThemedTextPanel DialogueElem;


		////////////////

		private DialogueChoices CurrentChoice;

		private Func<bool> OnChoice;


		////////////////

		public string ChoiceText { get; protected set; }

		public string Dialogue { get; protected set; }

		public IList<DialogueChoices> Choices { get; protected set; }



		////////////////

		private DialogueChoices() { }

		public DialogueChoices( string dialogue ) : this( dialogue, new List<DialogueChoices>(), null ) { }

		public DialogueChoices( string dialogue, IList<DialogueChoices> choices, Func<bool> onChoice ) {
			var singleton = ModContent.GetInstance<DialogueChoices>();
			this.DialogueElem = singleton.DialogueElem;

			this.CurrentChoice = this;
			this.Dialogue = dialogue;
			this.Choices = choices;
			this.OnChoice = onChoice;
		}


		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() {
			if( Main.netMode != NetmodeID.Server ) {
				this.DialogueElem = new UIThemedTextPanel( UITheme.Vanilla, false, "" );
				this.DialogueElem.Hide();

				FreeHUD.AddElement( "CutsceneDialogue", this.DialogueElem );
			}
		}

		void ILoadable.OnModsUnload() { }


		////////////////
		
		public void ShowDialogue() {
			var panel = FreeHUD.GetElement( "CutsceneDialogue" ) as UIThemedText;
			panel.SetText( this.Dialogue );
			panel?.Show();
		}

		public void HideDialogue() {
			var panel = FreeHUD.GetElement( "CutsceneDialogue" ) as UIThemedText;
			panel.SetText( "" );
			panel?.Hide();
		}


		////////////////

		public bool MakeChoice( int choice ) {
			this.CurrentChoice = this.Choices[choice];
			return this.CurrentChoice.OnChoice?.Invoke() ?? true;
		}
	}
}
