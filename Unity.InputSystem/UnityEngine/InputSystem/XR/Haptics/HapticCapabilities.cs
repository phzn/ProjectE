using System;

namespace UnityEngine.InputSystem.XR.Haptics
{
	// Token: 0x02000077 RID: 119
	public struct HapticCapabilities
	{
		// Token: 0x06000A71 RID: 2673 RVA: 0x00038B95 File Offset: 0x00036D95
		public HapticCapabilities(uint numChannels, bool supportsImpulse, bool supportsBuffer, uint frequencyHz, uint maxBufferSize, uint optimalBufferSize)
		{
			this.numChannels = numChannels;
			this.supportsImpulse = supportsImpulse;
			this.supportsBuffer = supportsBuffer;
			this.frequencyHz = frequencyHz;
			this.maxBufferSize = maxBufferSize;
			this.optimalBufferSize = optimalBufferSize;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00038BC4 File Offset: 0x00036DC4
		public HapticCapabilities(uint numChannels, uint frequencyHz, uint maxBufferSize)
		{
			this = new HapticCapabilities(numChannels, false, false, frequencyHz, maxBufferSize, 0U);
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x00038BD2 File Offset: 0x00036DD2
		public readonly uint numChannels { get; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x00038BDA File Offset: 0x00036DDA
		public readonly bool supportsImpulse { get; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x00038BE2 File Offset: 0x00036DE2
		public readonly bool supportsBuffer { get; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x00038BEA File Offset: 0x00036DEA
		public readonly uint frequencyHz { get; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x00038BF2 File Offset: 0x00036DF2
		public readonly uint maxBufferSize { get; }

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x00038BFA File Offset: 0x00036DFA
		public readonly uint optimalBufferSize { get; }
	}
}
