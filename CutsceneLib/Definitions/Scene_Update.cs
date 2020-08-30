using System;
using Terraria;
using CutsceneLib.Net;


namespace CutsceneLib.Definitions {
	public abstract partial class Scene<T, U, V> : SceneBase
				where T : Cutscene
				where U : MovieSet
				where V : AMLCutsceneNetStart {
		/// <summary></summary>
		/// <returns>`true` signifies scene has ended.</returns>
		internal sealed override bool UpdateScene_Internal( Cutscene parent ) {
			if( base.UpdateScene_Internal( parent ) ) {
				return true;
			}
			return this.Update( (T)parent );
		}


		////

		/// <summary></summary>
		/// <returns>`true` signifies scene has ended.</returns>
		protected abstract bool Update( T parent );
	}
}
