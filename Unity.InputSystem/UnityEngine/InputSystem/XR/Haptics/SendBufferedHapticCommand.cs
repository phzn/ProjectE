using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.XR.Haptics
{
	// Token: 0x02000079 RID: 121
	[StructLayout(LayoutKind.Explicit, Size = 1040)]
	public struct SendBufferedHapticCommand : IInputDeviceCommandInfo
	{
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x00038C6D File Offset: 0x00036E6D
		private static FourCC Type
		{
			get
			{
				return new FourCC('X', 'H', 'U', '0');
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x00038C7C File Offset: 0x00036E7C
		public FourCC typeStatic
		{
			get
			{
				return SendBufferedHapticCommand.Type;
			}
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00038C84 File Offset: 0x00036E84
		public unsafe static SendBufferedHapticCommand Create(byte[] rumbleBuffer)
		{
			if (rumbleBuffer == null)
			{
				throw new ArgumentNullException("rumbleBuffer");
			}
			int num = Mathf.Min(1024, rumbleBuffer.Length);
			SendBufferedHapticCommand sendBufferedHapticCommand = new SendBufferedHapticCommand
			{
				baseCommand = new InputDeviceCommand(SendBufferedHapticCommand.Type, 1040),
				bufferSize = num
			};
			SendBufferedHapticCommand* ptr = &sendBufferedHapticCommand;
			fixed (byte[] array = rumbleBuffer)
			{
				byte* ptr2;
				if (rumbleBuffer == null || array.Length == 0)
				{
					ptr2 = null;
				}
				else
				{
					ptr2 = &array[0];
				}
				for (int i = 0; i < num; i++)
				{
					*((ref ptr->buffer.FixedElementField) + i) = ptr2[i];
				}
			}
			return sendBufferedHapticCommand;
		}

		// Token: 0x040003A4 RID: 932
		private const int kMaxHapticBufferSize = 1024;

		// Token: 0x040003A5 RID: 933
		private const int kSize = 1040;

		// Token: 0x040003A6 RID: 934
		[FieldOffset(0)]
		private InputDeviceCommand baseCommand;

		// Token: 0x040003A7 RID: 935
		[FieldOffset(8)]
		private int channel;

		// Token: 0x040003A8 RID: 936
		[FieldOffset(12)]
		private int bufferSize;

		// Token: 0x040003A9 RID: 937
		[FixedBuffer(typeof(byte), 1024)]
		[FieldOffset(16)]
		private SendBufferedHapticCommand.<buffer>e__FixedBuffer buffer;

		// Token: 0x020001C9 RID: 457
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 1024)]
		public struct <buffer>e__FixedBuffer
		{
			// Token: 0x04000969 RID: 2409
			public byte FixedElementField;
		}
	}
}
