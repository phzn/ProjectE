using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine.VFX
{
	// Token: 0x0200001B RID: 27
	[UsedByNativeCode]
	[NativeHeader("Modules/VFX/Public/VisualEffectAsset.h")]
	[NativeHeader("VFXScriptingClasses.h")]
	public class VisualEffectAsset : VisualEffectObject
	{
		// Token: 0x060000AD RID: 173
		[FreeFunction(Name = "VisualEffectAssetBindings::GetTextureDimension", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern TextureDimension GetTextureDimension(int nameID);

		// Token: 0x060000AE RID: 174
		[FreeFunction(Name = "VisualEffectAssetBindings::GetExposedProperties", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetExposedProperties([NotNull("ArgumentNullException")] List<VFXExposedProperty> exposedProperties);

		// Token: 0x060000AF RID: 175
		[FreeFunction(Name = "VisualEffectAssetBindings::GetEvents", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetEvents([NotNull("ArgumentNullException")] List<string> names);

		// Token: 0x060000B0 RID: 176
		[FreeFunction(Name = "VisualEffectAssetBindings::HasSystemFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasSystem(int nameID);

		// Token: 0x060000B1 RID: 177
		[FreeFunction(Name = "VisualEffectAssetBindings::GetSystemNamesFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void GetSystemNames([NotNull("ArgumentNullException")] List<string> names);

		// Token: 0x060000B2 RID: 178
		[FreeFunction(Name = "VisualEffectAssetBindings::GetParticleSystemNamesFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void GetParticleSystemNames([NotNull("ArgumentNullException")] List<string> names);

		// Token: 0x060000B3 RID: 179
		[FreeFunction(Name = "VisualEffectAssetBindings::GetOutputEventNamesFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void GetOutputEventNames([NotNull("ArgumentNullException")] List<string> names);

		// Token: 0x060000B4 RID: 180
		[FreeFunction(Name = "VisualEffectAssetBindings::GetSpawnSystemNamesFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void GetSpawnSystemNames([NotNull("ArgumentNullException")] List<string> names);

		// Token: 0x060000B5 RID: 181 RVA: 0x00002A44 File Offset: 0x00000C44
		public TextureDimension GetTextureDimension(string name)
		{
			return this.GetTextureDimension(Shader.PropertyToID(name));
		}

		// Token: 0x04000120 RID: 288
		public const string PlayEventName = "OnPlay";

		// Token: 0x04000121 RID: 289
		public const string StopEventName = "OnStop";

		// Token: 0x04000122 RID: 290
		public static readonly int PlayEventID = Shader.PropertyToID("OnPlay");

		// Token: 0x04000123 RID: 291
		public static readonly int StopEventID = Shader.PropertyToID("OnStop");
	}
}
