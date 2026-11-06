using System;
using System.IO;

namespace Unity.Serialization
{
	// Token: 0x02000017 RID: 23
	internal static class DirectoryInfoExtensions
	{
		// Token: 0x06000038 RID: 56 RVA: 0x0000285C File Offset: 0x00000A5C
		internal static string GetRelativePath(this DirectoryInfo directoryInfo)
		{
			string text = new DirectoryInfo(".").FullName.ToForwardSlash();
			string text2 = directoryInfo.FullName.ToForwardSlash();
			if (!text2.StartsWith(text))
			{
				return text2;
			}
			return text2.Substring(text.Length).TrimStart('/');
		}
	}
}
