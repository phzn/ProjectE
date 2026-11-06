using System;
using DA_Assets.FCU.Model;

namespace DA_Assets.FCU
{
	// Token: 0x02000043 RID: 67
	public struct ScriptData
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00003E8B File Offset: 0x0000208B
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00003E93 File Offset: 0x00002093
		public FObject FObject { readonly get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00003E9C File Offset: 0x0000209C
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00003EA4 File Offset: 0x000020A4
		public Type ComponentType { readonly get; set; }

		// Token: 0x06000176 RID: 374 RVA: 0x00003EAD File Offset: 0x000020AD
		public ScriptData(FObject fobject, Type type)
		{
			this.FObject = fobject;
			this.ComponentType = type;
		}
	}
}
