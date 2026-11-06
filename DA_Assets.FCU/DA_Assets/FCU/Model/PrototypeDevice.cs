using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200006E RID: 110
	public struct PrototypeDevice
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000325 RID: 805 RVA: 0x00007924 File Offset: 0x00005B24
		// (set) Token: 0x06000326 RID: 806 RVA: 0x0000792C File Offset: 0x00005B2C
		[JsonProperty("type")]
		public PrototypeDeviceType Type { readonly get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00007935 File Offset: 0x00005B35
		// (set) Token: 0x06000328 RID: 808 RVA: 0x0000793D File Offset: 0x00005B3D
		[JsonProperty("rotation")]
		public string Rotation { readonly get; set; }
	}
}
