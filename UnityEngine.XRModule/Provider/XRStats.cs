using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.XR.Provider
{
	// Token: 0x02000033 RID: 51
	public static class XRStats
	{
		// Token: 0x0600017F RID: 383 RVA: 0x00004E28 File Offset: 0x00003028
		public static bool TryGetStat(IntegratedSubsystem xrSubsystem, string tag, out float value)
		{
			return XRStats.TryGetStat_Internal(xrSubsystem.m_Ptr, tag, out value);
		}

		// Token: 0x06000180 RID: 384
		[NativeHeader("Modules/XR/Stats/XRStats.h")]
		[NativeConditional("ENABLE_XR")]
		[StaticAccessor("XRStats::Get()", StaticAccessorType.Dot)]
		[NativeMethod("TryGetStatByName_Internal")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TryGetStat_Internal(IntPtr ptr, string tag, out float value);
	}
}
