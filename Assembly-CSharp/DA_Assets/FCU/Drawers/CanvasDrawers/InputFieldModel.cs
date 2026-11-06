using System;
using UnityEngine;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003BB RID: 955
	internal struct InputFieldModel
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x0009CB5A File Offset: 0x0009AD5A
		// (set) Token: 0x0600188C RID: 6284 RVA: 0x0009CB62 File Offset: 0x0009AD62
		public GameObject TextArea { readonly get; set; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x0600188D RID: 6285 RVA: 0x0009CB6B File Offset: 0x0009AD6B
		// (set) Token: 0x0600188E RID: 6286 RVA: 0x0009CB73 File Offset: 0x0009AD73
		public GameObject Background { readonly get; set; }

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x0600188F RID: 6287 RVA: 0x0009CB7C File Offset: 0x0009AD7C
		// (set) Token: 0x06001890 RID: 6288 RVA: 0x0009CB84 File Offset: 0x0009AD84
		public GameObject TextComponent { readonly get; set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001891 RID: 6289 RVA: 0x0009CB8D File Offset: 0x0009AD8D
		// (set) Token: 0x06001892 RID: 6290 RVA: 0x0009CB95 File Offset: 0x0009AD95
		public GameObject Placeholder { readonly get; set; }
	}
}
