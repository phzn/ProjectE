using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.XR.Haptics
{
	// Token: 0x02000076 RID: 118
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct GetCurrentHapticStateCommand : IInputDeviceCommandInfo
	{
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00038B40 File Offset: 0x00036D40
		private static FourCC Type
		{
			get
			{
				return new FourCC('X', 'H', 'S', '0');
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x00038B4F File Offset: 0x00036D4F
		public FourCC typeStatic
		{
			get
			{
				return GetCurrentHapticStateCommand.Type;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x00038B56 File Offset: 0x00036D56
		public HapticState currentState
		{
			get
			{
				return new HapticState(this.samplesQueued, this.samplesAvailable);
			}
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00038B6C File Offset: 0x00036D6C
		public static GetCurrentHapticStateCommand Create()
		{
			return new GetCurrentHapticStateCommand
			{
				baseCommand = new InputDeviceCommand(GetCurrentHapticStateCommand.Type, 16)
			};
		}

		// Token: 0x04000392 RID: 914
		private const int kSize = 16;

		// Token: 0x04000393 RID: 915
		[FieldOffset(0)]
		private InputDeviceCommand baseCommand;

		// Token: 0x04000394 RID: 916
		[FieldOffset(8)]
		public uint samplesQueued;

		// Token: 0x04000395 RID: 917
		[FieldOffset(12)]
		public uint samplesAvailable;
	}
}
