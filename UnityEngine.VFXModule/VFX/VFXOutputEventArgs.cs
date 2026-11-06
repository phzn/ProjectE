using System;

namespace UnityEngine.VFX
{
	// Token: 0x0200001C RID: 28
	public struct VFXOutputEventArgs
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00002A8B File Offset: 0x00000C8B
		public readonly int nameId { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00002A93 File Offset: 0x00000C93
		public readonly VFXEventAttribute eventAttribute { get; }

		// Token: 0x060000BA RID: 186 RVA: 0x00002A9B File Offset: 0x00000C9B
		public VFXOutputEventArgs(int nameId, VFXEventAttribute eventAttribute)
		{
			this.nameId = nameId;
			this.eventAttribute = eventAttribute;
		}
	}
}
