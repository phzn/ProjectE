using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
	// Token: 0x02000068 RID: 104
	internal class LocalAppContext
	{
		// Token: 0x060004B6 RID: 1206 RVA: 0x00010C97 File Offset: 0x0000EE97
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool GetCachedSwitchValue(string switchName, ref int switchValue)
		{
			return switchValue >= 0 && (switchValue > 0 || LocalAppContext.GetCachedSwitchValueInternal(switchName, ref switchValue));
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00010CB0 File Offset: 0x0000EEB0
		private static bool GetCachedSwitchValueInternal(string switchName, ref int switchValue)
		{
			bool flag;
			AppContext.TryGetSwitch(switchName, out flag);
			if (LocalAppContext.DisableCaching)
			{
				return flag;
			}
			switchValue = (flag ? 1 : (-1));
			return flag;
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00010CD9 File Offset: 0x0000EED9
		private static bool DisableCaching
		{
			get
			{
				return LazyInitializer.EnsureInitialized<bool>(ref LocalAppContext.s_disableCaching, ref LocalAppContext.s_isDisableCachingInitialized, ref LocalAppContext.s_syncObject, delegate
				{
					bool flag;
					AppContext.TryGetSwitch("TestSwitch.LocalAppContext.DisableCaching", out flag);
					return flag;
				});
			}
		}

		// Token: 0x04000616 RID: 1558
		private static bool s_isDisableCachingInitialized;

		// Token: 0x04000617 RID: 1559
		private static bool s_disableCaching;

		// Token: 0x04000618 RID: 1560
		private static object s_syncObject;
	}
}
