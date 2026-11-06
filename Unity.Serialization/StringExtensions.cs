using System;

namespace Unity.Serialization
{
	// Token: 0x02000019 RID: 25
	internal static class StringExtensions
	{
		// Token: 0x0600003A RID: 58 RVA: 0x000028F4 File Offset: 0x00000AF4
		internal static string ToForwardSlash(this string value)
		{
			return value.Replace('\\', '/');
		}
	}
}
