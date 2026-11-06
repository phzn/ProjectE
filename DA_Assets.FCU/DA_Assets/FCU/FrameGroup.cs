using System;
using System.Collections.Generic;
using DA_Assets.FCU.Model;

namespace DA_Assets.FCU
{
	// Token: 0x0200003B RID: 59
	public struct FrameGroup
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00003C4F File Offset: 0x00001E4F
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00003C57 File Offset: 0x00001E57
		public FObject RootFrame { readonly get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00003C60 File Offset: 0x00001E60
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00003C68 File Offset: 0x00001E68
		public List<FObject> Childs { readonly get; set; }
	}
}
