using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000080 RID: 128
	public class DocumentationInfo
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0001181B File Offset: 0x0000FA1B
		public static string version
		{
			get
			{
				return "13.1";
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00011822 File Offset: 0x0000FA22
		public static string GetPageLink(string packageName, string pageName)
		{
			return string.Format("https://docs.unity3d.com/Packages/{0}@{1}/manual/{2}.html{3}", new object[]
			{
				packageName,
				DocumentationInfo.version,
				pageName,
				""
			});
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001184C File Offset: 0x0000FA4C
		public static string GetPageLink(string packageName, string pageName, string pageHash)
		{
			return string.Format("https://docs.unity3d.com/Packages/{0}@{1}/manual/{2}.html{3}", new object[]
			{
				packageName,
				DocumentationInfo.version,
				pageName,
				pageHash
			});
		}

		// Token: 0x04000236 RID: 566
		private const string fallbackVersion = "13.1";

		// Token: 0x04000237 RID: 567
		private const string url = "https://docs.unity3d.com/Packages/{0}@{1}/manual/{2}.html{3}";
	}
}
