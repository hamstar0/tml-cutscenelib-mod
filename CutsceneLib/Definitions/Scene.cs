using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using CutsceneLib.Net;
using HamstarHelpers.Helpers.DotNET.Reflection;


namespace CutsceneLib.Definitions {
	public abstract partial class Scene<T, U, V> : SceneBase
				where T : Cutscene
				where U : MovieSet
				where V : CutsceneNetStart {
		protected U Set;



		////////////////

		protected Scene( bool defersToHostForSync, U set ) : base( defersToHostForSync ) {
			this.Set = set;
		}


		////////////////

		internal sealed override CutsceneNetStart CreatePacketPayload_Internal( Cutscene cutscene ) {
			return this.CreatePacketPayload( (T)cutscene );
		}

		////

		protected V CreatePacketPayload( T cutscene ) {
			return Activator.CreateInstance(
				type: typeof( V ),
				bindingAttr: ReflectionHelpers.MostAccess,
				binder: null,
				args: new object[] { cutscene, this.Set },
				culture: null
			) as V;
		}


		////////////////

		internal sealed override void BeginScene_Internal( Cutscene parent ) {
			base.BeginScene_Internal( parent );
			this.OnBegin( (T)parent );
		}

		////

		protected virtual void OnBegin( T parent ) { }


		////////////////

		internal sealed override void EndScene_Internal( Cutscene parent ) {
			base.EndScene_Internal( parent );
			this.OnEnd( (T)parent );
		}

		////

		protected virtual void OnEnd( T parent ) { }
	}
}
