using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000BF RID: 191
	[StructLayout(LayoutKind.Explicit, Size = 268)]
	public struct QueryKeyNameCommand : IInputDeviceCommandInfo
	{
		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x000443C8 File Offset: 0x000425C8
		public static FourCC Type
		{
			get
			{
				return new FourCC('K', 'Y', 'C', 'F');
			}
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x000443D8 File Offset: 0x000425D8
		public unsafe string ReadKeyName()
		{
			fixed (QueryKeyNameCommand* ptr = &this)
			{
				return StringHelpers.ReadStringFromBuffer(new IntPtr((void*)(&ptr->nameBuffer.FixedElementField)), 256);
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x00044403 File Offset: 0x00042603
		public FourCC typeStatic
		{
			get
			{
				return QueryKeyNameCommand.Type;
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0004440C File Offset: 0x0004260C
		public static QueryKeyNameCommand Create(Key key)
		{
			return new QueryKeyNameCommand
			{
				baseCommand = new InputDeviceCommand(QueryKeyNameCommand.Type, 268),
				scanOrKeyCode = (int)key
			};
		}

		// Token: 0x04000511 RID: 1297
		internal const int kMaxNameLength = 256;

		// Token: 0x04000512 RID: 1298
		internal const int kSize = 268;

		// Token: 0x04000513 RID: 1299
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x04000514 RID: 1300
		[FieldOffset(8)]
		public int scanOrKeyCode;

		// Token: 0x04000515 RID: 1301
		[FixedBuffer(typeof(byte), 256)]
		[FieldOffset(12)]
		public QueryKeyNameCommand.<nameBuffer>e__FixedBuffer nameBuffer;

		// Token: 0x02000209 RID: 521
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 256)]
		public struct <nameBuffer>e__FixedBuffer
		{
			// Token: 0x04000B83 RID: 2947
			public byte FixedElementField;
		}
	}
}
