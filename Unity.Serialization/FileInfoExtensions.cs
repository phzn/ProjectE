using System;
using System.IO;

namespace Unity.Serialization
{
	// Token: 0x02000018 RID: 24
	internal static class FileInfoExtensions
	{
		// Token: 0x06000039 RID: 57 RVA: 0x000028A8 File Offset: 0x00000AA8
		internal static string GetRelativePath(this FileInfo fileInfo)
		{
			string text = new DirectoryInfo(".").FullName.ToForwardSlash();
			string text2 = fileInfo.FullName.ToForwardSlash();
			if (!text2.StartsWith(text))
			{
				return text2;
			}
			return text2.Substring(text.Length).TrimStart('/');
		}
	}
}
