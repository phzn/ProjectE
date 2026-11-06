using System;

namespace UnityEngine.InputSystem.XR.Haptics
{
	// Token: 0x02000074 RID: 116
	public struct BufferedRumble
	{
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00038A87 File Offset: 0x00036C87
		// (set) Token: 0x06000A63 RID: 2659 RVA: 0x00038A8F File Offset: 0x00036C8F
		public HapticCapabilities capabilities { readonly get; private set; }

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x00038A98 File Offset: 0x00036C98
		// (set) Token: 0x06000A65 RID: 2661 RVA: 0x00038AA0 File Offset: 0x00036CA0
		private InputDevice device { readonly get; set; }

		// Token: 0x06000A66 RID: 2662 RVA: 0x00038AAC File Offset: 0x00036CAC
		public BufferedRumble(InputDevice device)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			this.device = device;
			GetHapticCapabilitiesCommand getHapticCapabilitiesCommand = GetHapticCapabilitiesCommand.Create();
			device.ExecuteCommand<GetHapticCapabilitiesCommand>(ref getHapticCapabilitiesCommand);
			this.capabilities = getHapticCapabilitiesCommand.capabilities;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00038AEC File Offset: 0x00036CEC
		public void EnqueueRumble(byte[] samples)
		{
			SendBufferedHapticCommand sendBufferedHapticCommand = SendBufferedHapticCommand.Create(samples);
			this.device.ExecuteCommand<SendBufferedHapticCommand>(ref sendBufferedHapticCommand);
		}
	}
}
