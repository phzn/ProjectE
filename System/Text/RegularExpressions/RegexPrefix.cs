using System;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200020A RID: 522
	internal readonly struct RegexPrefix
	{
		// Token: 0x06000F06 RID: 3846 RVA: 0x00043334 File Offset: 0x00041534
		internal RegexPrefix(string prefix, bool ci)
		{
			this.Prefix = prefix;
			this.CaseInsensitive = ci;
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x00043344 File Offset: 0x00041544
		internal bool CaseInsensitive { get; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x0004334C File Offset: 0x0004154C
		internal static RegexPrefix Empty { get; } = new RegexPrefix(string.Empty, false);

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x00043353 File Offset: 0x00041553
		internal string Prefix { get; }
	}
}
