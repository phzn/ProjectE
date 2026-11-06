using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BF RID: 191
	public class JsonCloneSettings
	{
		// Token: 0x06000A9C RID: 2716 RVA: 0x00029F6D File Offset: 0x0002816D
		public JsonCloneSettings()
		{
			this.CopyAnnotations = true;
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x00029F7C File Offset: 0x0002817C
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x00029F84 File Offset: 0x00028184
		public bool CopyAnnotations { get; set; }

		// Token: 0x04000383 RID: 899
		[Nullable(1)]
		internal static readonly JsonCloneSettings SkipCopyAnnotations = new JsonCloneSettings
		{
			CopyAnnotations = false
		};
	}
}
