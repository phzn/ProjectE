using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.XR.Haptics
{
	// Token: 0x0200007A RID: 122
	[StructLayout(LayoutKind.Explicit, Size = 20)]
	public struct SendHapticImpulseCommand : IInputDeviceCommandInfo
	{
		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x00038D1F File Offset: 0x00036F1F
		private static FourCC Type
		{
			get
			{
				return new FourCC('X', 'H', 'I', '0');
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x00038D2E File Offset: 0x00036F2E
		public FourCC typeStatic
		{
			get
			{
				return SendHapticImpulseCommand.Type;
			}
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00038D38 File Offset: 0x00036F38
		public static SendHapticImpulseCommand Create(int motorChannel, float motorAmplitude, float motorDuration)
		{
			return new SendHapticImpulseCommand
			{
				baseCommand = new InputDeviceCommand(SendHapticImpulseCommand.Type, 20),
				channel = motorChannel,
				amplitude = motorAmplitude,
				duration = motorDuration
			};
		}

		// Token: 0x040003AA RID: 938
		private const int kSize = 20;

		// Token: 0x040003AB RID: 939
		[FieldOffset(0)]
		private InputDeviceCommand baseCommand;

		// Token: 0x040003AC RID: 940
		[FieldOffset(8)]
		private int channel;

		// Token: 0x040003AD RID: 941
		[FieldOffset(12)]
		private float amplitude;

		// Token: 0x040003AE RID: 942
		[FieldOffset(16)]
		private float duration;
	}
}
