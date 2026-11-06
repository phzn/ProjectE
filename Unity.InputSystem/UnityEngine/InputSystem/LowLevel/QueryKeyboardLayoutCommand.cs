using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000BE RID: 190
	[StructLayout(LayoutKind.Explicit, Size = 264)]
	public struct QueryKeyboardLayoutCommand : IInputDeviceCommandInfo
	{
		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x00044325 File Offset: 0x00042525
		public static FourCC Type
		{
			get
			{
				return new FourCC('K', 'B', 'L', 'T');
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00044334 File Offset: 0x00042534
		public unsafe string ReadLayoutName()
		{
			fixed (QueryKeyboardLayoutCommand* ptr = &this)
			{
				return StringHelpers.ReadStringFromBuffer(new IntPtr((void*)(&ptr->nameBuffer.FixedElementField)), 256);
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00044360 File Offset: 0x00042560
		public unsafe void WriteLayoutName(string name)
		{
			fixed (QueryKeyboardLayoutCommand* ptr = &this)
			{
				QueryKeyboardLayoutCommand* ptr2 = ptr;
				StringHelpers.WriteStringToBuffer(name, new IntPtr((void*)(&ptr2->nameBuffer.FixedElementField)), 256);
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x00044392 File Offset: 0x00042592
		public FourCC typeStatic
		{
			get
			{
				return QueryKeyboardLayoutCommand.Type;
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0004439C File Offset: 0x0004259C
		public static QueryKeyboardLayoutCommand Create()
		{
			return new QueryKeyboardLayoutCommand
			{
				baseCommand = new InputDeviceCommand(QueryKeyboardLayoutCommand.Type, 264)
			};
		}

		// Token: 0x0400050E RID: 1294
		internal const int kMaxNameLength = 256;

		// Token: 0x0400050F RID: 1295
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x04000510 RID: 1296
		[FixedBuffer(typeof(byte), 256)]
		[FieldOffset(8)]
		public QueryKeyboardLayoutCommand.<nameBuffer>e__FixedBuffer nameBuffer;

		// Token: 0x02000208 RID: 520
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 256)]
		public struct <nameBuffer>e__FixedBuffer
		{
			// Token: 0x04000B82 RID: 2946
			public byte FixedElementField;
		}
	}
}
