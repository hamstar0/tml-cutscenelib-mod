using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using Terraria.ModLoader;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;


namespace CutsceneLib.Definitions {
	public class SceneID {
		public string ModName { get; }
		public string FullClassName { get; }



		////////////////

		public SceneID( Mod mod, SceneBase instance ) : this( mod, instance.GetType() ) { }

		public SceneID( Mod mod, Type sceneType ) : this( mod.Name, sceneType.FullName ) {
			for( Type baseType= sceneType.BaseType; baseType!=typeof(SceneBase); baseType = baseType.BaseType ) {
				if( baseType == typeof(object) ) {
					throw new ModHelpersException( sceneType.Name + " is not a `Scene`." );
				}
			}
		}

		internal SceneID( string modName, string fullClassName ) {
			this.ModName = modName;
			this.FullClassName = fullClassName;
		}

		////

		public override int GetHashCode() {
			return this.ModName.GetHashCode() ^ this.FullClassName.GetHashCode();
		}

		public override bool Equals( object obj ) {
			var comp = obj as SceneID;
			if( comp == null ) { return false; }

			return comp.ModName == this.ModName && comp.FullClassName == this.FullClassName;
		}

		////

		public override string ToString() {
			return this.ModName+":"+this.FullClassName.Split('.').Last();
		}


		////////////////

		internal SceneBase Create( Player playsFor, params object[] args ) {
			var newArgs = new object[ args.Length + 1 ];
			newArgs[0] = playsFor;
			args.CopyTo( newArgs, 1 );

			Mod mod = ModLoader.GetMod( this.ModName );

			ObjectHandle objHand = Activator.CreateInstance(
				assemblyName: mod.Code.GetName().Name,
				typeName: this.FullClassName,
				ignoreCase: false,
				bindingAttr: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				binder: null,
				args: newArgs,
				culture: null,
				activationAttributes: new object[] { }
			);
			return objHand.Unwrap() as SceneBase;
		}
	}
}
