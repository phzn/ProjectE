using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000C2 RID: 194
	[StructLayout(LayoutKind.Explicit, Size = 520)]
	internal struct QueryUserIdCommand : IInputDeviceCommandInfo
	{
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x000445F5 File Offset: 0x000427F5
		public static FourCC Type
		{
			get
			{
				return new FourCC('U', 'S', 'E', 'R');
			}
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00044604 File Offset: 0x00042804
		public unsafe string ReadId()
		{
			fixed (QueryUserIdCommand* ptr = &this)
			{
				return StringHelpers.ReadStringFromBuffer(new IntPtr((void*)(&ptr->idBuffer.FixedElementField)), 256);
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x0004462F File Offset: 0x0004282F
		public FourCC typeStatic
		{
			get
			{
				return QueryUserIdCommand.Type;
			}
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00044638 File Offset: 0x00042838
		public static QueryUserIdCommand Create()
		{
			return new QueryUserIdCommand
			{
				baseCommand = new InputDeviceCommand(QueryUserIdCommand.Type, 520)
			};
		}

		// Token: 0x04000520 RID: 1312
		public const int kMaxIdLength = 256;

		// Token: 0x04000521 RID: 1313
		internal const int kSize = 520;

		// Token: 0x04000522 RID: 1314
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x04000523 RID: 1315
		[FixedBuffer(typeof(byte), 512)]
		[FieldOffset(8)]
		public QueryUserIdCommand.<idBuffer>e__FixedBuffer idBuffer;

		// Token: 0x0200020D RID: 525
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 512)]
		public struct <idBuffer>e__FixedBuffer
		{
			// Token: 0x04000B8B RID: 2955
			public byte FixedElementField;
		}
	}
}
