using System;

namespace UnityEngine.Localization
{
	// Token: 0x02000022 RID: 34
	internal static class InstanceIdHelper
	{
		// Token: 0x06000104 RID: 260 RVA: 0x0000480C File Offset: 0x00002A0C
		public static int GetInstanceId(Object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetInstanceID();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004820 File Offset: 0x00002A20
		public static string GetInstanceIdString(Object obj)
		{
			if (obj == null)
			{
				return "0";
			}
			return obj.GetInstanceID().ToString();
		}
	}
}
