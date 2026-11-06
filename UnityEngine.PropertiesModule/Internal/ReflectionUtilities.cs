using System;
using System.Reflection;

namespace Unity.Properties.Internal
{
	// Token: 0x020000D1 RID: 209
	internal static class ReflectionUtilities
	{
		// Token: 0x06000424 RID: 1060 RVA: 0x0000D488 File Offset: 0x0000B688
		public static string SanitizeMemberName(MemberInfo info)
		{
			return info.Name.Replace(".", "_").Replace("<", "_").Replace(">", "_")
				.Replace("+", "_");
		}
	}
}
