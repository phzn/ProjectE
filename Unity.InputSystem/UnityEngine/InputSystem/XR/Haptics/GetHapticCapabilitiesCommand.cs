using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.XR.Haptics
{
	// Token: 0x02000078 RID: 120
	[StructLayout(LayoutKind.Explicit, Size = 28)]
	public struct GetHapticCapabilitiesCommand : IInputDeviceCommandInfo
	{
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x00038C02 File Offset: 0x00036E02
		private static FourCC Type
		{
			get
			{
				return new FourCC('X', 'H', 'C', '0');
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x00038C11 File Offset: 0x00036E11
		public FourCC typeStatic
		{
			get
			{
				return GetHapticCapabilitiesCommand.Type;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x00038C18 File Offset: 0x00036E18
		public HapticCapabilities capabilities
		{
			get
			{
				return new HapticCapabilities(this.numChannels, this.supportsImpulse, this.supportsBuffer, this.frequencyHz, this.maxBufferSize, this.optimalBufferSize);
			}
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00038C44 File Offset: 0x00036E44
		public static GetHapticCapabilitiesCommand Create()
		{
			return new GetHapticCapabilitiesCommand
			{
				baseCommand = new InputDeviceCommand(GetHapticCapabilitiesCommand.Type, 28)
			};
		}

		// Token: 0x0400039C RID: 924
		private const int kSize = 28;

		// Token: 0x0400039D RID: 925
		[FieldOffset(0)]
		private InputDeviceCommand baseCommand;

		// Token: 0x0400039E RID: 926
		[FieldOffset(8)]
		public uint numChannels;

		// Token: 0x0400039F RID: 927
		[FieldOffset(12)]
		public bool supportsImpulse;

		// Token: 0x040003A0 RID: 928
		[FieldOffset(13)]
		public bool supportsBuffer;

		// Token: 0x040003A1 RID: 929
		[FieldOffset(16)]
		public uint frequencyHz;

		// Token: 0x040003A2 RID: 930
		[FieldOffset(20)]
		public uint maxBufferSize;

		// Token: 0x040003A3 RID: 931
		[FieldOffset(24)]
		public uint optimalBufferSize;
	}
}
