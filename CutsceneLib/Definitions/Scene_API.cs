using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.DotNET.Reflection;
using CutsceneLib.Net;


namespace CutsceneLib.Definitions {
	public abstract partial class Scene<T, U, V, W> : SceneBase
				where T : Cutscene
				where U : MovieSet
				where V : CutsceneStartProtocol
				where W : CutsceneUpdateProtocol {
		internal sealed override bool AllowNPC_Internal( Cutscene parent, NPC npc ) {
			return this.AllowNPC( (T)parent, npc );
		}

		////

		public abstract bool AllowNPC( T parent, NPC npc );


		////////////////

		internal sealed override void BeginScene_Internal( Cutscene parent ) {
			this.OnBegin( (T)parent );
		}

		////

		protected virtual void OnBegin( T parent ) { }


		////////////////

		internal sealed override void EndScene_Internal( Cutscene parent ) {
			this.OnEnd( (T)parent );
		}

		////

		protected virtual void OnEnd( T parent ) { }
	}
}
