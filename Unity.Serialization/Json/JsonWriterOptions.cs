using System;

namespace Unity.Serialization.Json
{
	// Token: 0x02000032 RID: 50
	public struct JsonWriterOptions
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x0000425B File Offset: 0x0000245B
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00004263 File Offset: 0x00002463
		public bool Minified { readonly get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x0000426C File Offset: 0x0000246C
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00004274 File Offset: 0x00002474
		public bool Simplified { readonly get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x0000427D File Offset: 0x0000247D
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00004291 File Offset: 0x00002491
		public int Indent
		{
			readonly get
			{
				if ((JsonWriterOptions.Overrides.Indent & this.m_Overrides) == (JsonWriterOptions.Overrides)0)
				{
					return 4;
				}
				return this.m_Indent;
			}
			set
			{
				this.m_Overrides |= JsonWriterOptions.Overrides.Indent;
				this.m_Indent = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000042A8 File Offset: 0x000024A8
		// (set) Token: 0x060000FC RID: 252 RVA: 0x000042BC File Offset: 0x000024BC
		public bool StringEscapeHandling
		{
			readonly get
			{
				return (JsonWriterOptions.Overrides.StringEscapeHandling & this.m_Overrides) == (JsonWriterOptions.Overrides)0 || this.m_StringEscapeHandling;
			}
			set
			{
				this.m_Overrides |= JsonWriterOptions.Overrides.StringEscapeHandling;
				this.m_StringEscapeHandling = value;
			}
		}

		// Token: 0x04000064 RID: 100
		private JsonWriterOptions.Overrides m_Overrides;

		// Token: 0x04000065 RID: 101
		private int m_Indent;

		// Token: 0x04000066 RID: 102
		private bool m_StringEscapeHandling;

		// Token: 0x02000033 RID: 51
		[Flags]
		private enum Overrides
		{
			// Token: 0x0400006A RID: 106
			None = 1,
			// Token: 0x0400006B RID: 107
			Indent = 2,
			// Token: 0x0400006C RID: 108
			StringEscapeHandling = 4
		}
	}
}
