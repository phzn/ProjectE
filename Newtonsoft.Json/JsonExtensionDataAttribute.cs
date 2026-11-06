using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000020 RID: 32
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class JsonExtensionDataAttribute : Attribute
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00002FEC File Offset: 0x000011EC
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00002FF4 File Offset: 0x000011F4
		public bool WriteData { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00002FFD File Offset: 0x000011FD
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00003005 File Offset: 0x00001205
		public bool ReadData { get; set; }

		// Token: 0x0600009D RID: 157 RVA: 0x0000300E File Offset: 0x0000120E
		public JsonExtensionDataAttribute()
		{
			this.WriteData = true;
			this.ReadData = true;
		}
	}
}
