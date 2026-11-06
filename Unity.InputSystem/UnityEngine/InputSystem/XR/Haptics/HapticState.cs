using System;

namespace UnityEngine.InputSystem.XR.Haptics
{
	// Token: 0x02000075 RID: 117
	public struct HapticState
	{
		// Token: 0x06000A68 RID: 2664 RVA: 0x00038B0E File Offset: 0x00036D0E
		public HapticState(uint samplesQueued, uint samplesAvailable)
		{
			this.samplesQueued = samplesQueued;
			this.samplesAvailable = samplesAvailable;
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x00038B1E File Offset: 0x00036D1E
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x00038B26 File Offset: 0x00036D26
		public uint samplesQueued { readonly get; private set; }

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x00038B2F File Offset: 0x00036D2F
		// (set) Token: 0x06000A6C RID: 2668 RVA: 0x00038B37 File Offset: 0x00036D37
		public uint samplesAvailable { readonly get; private set; }
	}
}
