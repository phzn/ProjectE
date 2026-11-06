using System;
using System.Collections.Generic;

namespace Unity.Serialization.Json
{
	// Token: 0x02000030 RID: 48
	public struct JsonSerializationParameters
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00004139 File Offset: 0x00002339
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00004141 File Offset: 0x00002341
		public Type SerializedType { readonly get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000DB RID: 219 RVA: 0x0000414A File Offset: 0x0000234A
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00004152 File Offset: 0x00002352
		public bool DisableRootAdapters { readonly get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000DD RID: 221 RVA: 0x0000415B File Offset: 0x0000235B
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00004163 File Offset: 0x00002363
		public List<IJsonAdapter> UserDefinedAdapters { readonly get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000DF RID: 223 RVA: 0x0000416C File Offset: 0x0000236C
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00004174 File Offset: 0x00002374
		public List<IJsonMigration> UserDefinedMigrations { readonly get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000417D File Offset: 0x0000237D
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00004185 File Offset: 0x00002385
		public object UserData { readonly get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x0000418E File Offset: 0x0000238E
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00004196 File Offset: 0x00002396
		public int InitialCapacity { readonly get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000419F File Offset: 0x0000239F
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x000041A7 File Offset: 0x000023A7
		public bool RequiresThreadSafety { readonly get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000041B0 File Offset: 0x000023B0
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x000041B8 File Offset: 0x000023B8
		public bool DisableSerializedReferences { readonly get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000041C1 File Offset: 0x000023C1
		// (set) Token: 0x060000EA RID: 234 RVA: 0x000041C9 File Offset: 0x000023C9
		public bool Minified { readonly get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000041D2 File Offset: 0x000023D2
		// (set) Token: 0x060000EC RID: 236 RVA: 0x000041DA File Offset: 0x000023DA
		public bool Simplified { readonly get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000ED RID: 237 RVA: 0x000041E3 File Offset: 0x000023E3
		// (set) Token: 0x060000EE RID: 238 RVA: 0x000041EB File Offset: 0x000023EB
		public bool DisableValidation { readonly get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000EF RID: 239 RVA: 0x000041F4 File Offset: 0x000023F4
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00004208 File Offset: 0x00002408
		public int Indent
		{
			readonly get
			{
				if ((JsonSerializationParameters.Overrides.Indent & this.m_Overrides) == (JsonSerializationParameters.Overrides)0)
				{
					return 4;
				}
				return this.m_Indent;
			}
			set
			{
				this.m_Overrides |= JsonSerializationParameters.Overrides.Indent;
				this.m_Indent = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x0000421F File Offset: 0x0000241F
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00004233 File Offset: 0x00002433
		public bool StringEscapeHandling
		{
			readonly get
			{
				return (JsonSerializationParameters.Overrides.StringEscapeHandling & this.m_Overrides) == (JsonSerializationParameters.Overrides)0 || this.m_StringEscapeHandling;
			}
			set
			{
				this.m_Overrides |= JsonSerializationParameters.Overrides.StringEscapeHandling;
				this.m_StringEscapeHandling = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x0000424A File Offset: 0x0000244A
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00004252 File Offset: 0x00002452
		internal JsonSerializationState State { readonly get; set; }

		// Token: 0x04000051 RID: 81
		private JsonSerializationParameters.Overrides m_Overrides;

		// Token: 0x04000052 RID: 82
		private int m_Indent;

		// Token: 0x04000053 RID: 83
		private bool m_StringEscapeHandling;

		// Token: 0x02000031 RID: 49
		[Flags]
		private enum Overrides
		{
			// Token: 0x04000061 RID: 97
			None = 1,
			// Token: 0x04000062 RID: 98
			Indent = 2,
			// Token: 0x04000063 RID: 99
			StringEscapeHandling = 4
		}
	}
}
