using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x02000003 RID: 3
	[StaticAccessor("SpriteUtilityBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Modules/SpriteMask/Public/ScriptBindings/SpriteMask.bindings.h")]
	public static class SpriteMaskUtility
	{
		// Token: 0x06000014 RID: 20 RVA: 0x0000206F File Offset: 0x0000026F
		public static bool HasSpriteMaskInLayerRange(SortingLayerRange range)
		{
			return SpriteMaskUtility.HasSpriteMaskInLayerRange_Injected(ref range);
		}

		// Token: 0x06000015 RID: 21
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool HasSpriteMaskInLayerRange_Injected(ref SortingLayerRange range);
	}
}
