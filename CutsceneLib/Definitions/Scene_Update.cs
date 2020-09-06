using System;
using Terraria;
using CutsceneLib.Net;


namespace CutsceneLib.Definitions {
	public abstract partial class Scene<T, U, V, W> : SceneBase
				where T : Cutscene
				where U : MovieSet
				where V : CutsceneStartProtocol
				where W : CutsceneUpdateProtocol {
		/// <summary></summary>
		/// <returns>`true` signifies scene has ended.</returns>
		internal sealed override bool UpdateScene_Internal( Cutscene parent ) {
			return this.Update( (T)parent );
		}


		////

		/// <summary></summary>
		/// <returns>`true` signifies scene has ended.</returns>
		protected abstract bool Update( T parent );
	}
}
