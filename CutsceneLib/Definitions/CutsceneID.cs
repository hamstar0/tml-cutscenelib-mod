using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.Errors;


namespace CutsceneLib.Definitions {
	public class CutsceneID {
		public string ModName { get; }
		public string FullClassName { get; }



		////////////////

		public CutsceneID( Mod mod, Cutscene instance ) : this( mod, instance.GetType() ) { }
		
		public CutsceneID( Mod mod, Type cutsceneType ) : this( mod.Name, cutsceneType.FullName ) {
			for( Type baseType=cutsceneType.BaseType; baseType!=typeof(Cutscene); baseType = baseType.BaseType ) {
				if( baseType == typeof(object) ) {
					throw new ModHelpersException( cutsceneType.Name + " is not a `Cutscene`." );
				}
			}
		}

		internal CutsceneID( string modName, string fullClassName ) {
			this.ModName = modName;
			this.FullClassName = fullClassName;
		}

		////

		public override int GetHashCode() {
			return this.ModName.GetHashCode() ^ this.FullClassName.GetHashCode();
		}

		public override bool Equals( object obj ) {
			var comp = obj as CutsceneID;
			if( comp == null ) { return false; }

			return comp.ModName == this.ModName && comp.FullClassName == this.FullClassName;
		}

		////

		public override string ToString() {
			return this.ModName+":"+this.FullClassName.Split('.').Last();
		}


		////////////////

		internal Cutscene Create( Player playsFor, params object[] args ) {
			var newArgs = new object[args.Length + 1];
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
			return objHand.Unwrap() as Cutscene;
		}
	}
}
