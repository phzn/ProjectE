using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000C0 RID: 192
	[StructLayout(LayoutKind.Explicit, Size = 1040)]
	public struct QueryPairedUserAccountCommand : IInputDeviceCommandInfo
	{
		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00044440 File Offset: 0x00042640
		public static FourCC Type
		{
			get
			{
				return new FourCC('P', 'A', 'C', 'C');
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x00044450 File Offset: 0x00042650
		// (set) Token: 0x06000D29 RID: 3369 RVA: 0x0004447C File Offset: 0x0004267C
		public unsafe string id
		{
			get
			{
				fixed (byte* ptr = &this.idBuffer.FixedElementField)
				{
					return StringHelpers.ReadStringFromBuffer(new IntPtr((void*)ptr), 256);
				}
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length > 256)
				{
					throw new ArgumentException(string.Format("ID '{0}' exceeds maximum supported length of {1} characters", value, 256), "value");
				}
				fixed (byte* ptr = &this.idBuffer.FixedElementField)
				{
					byte* ptr2 = ptr;
					StringHelpers.WriteStringToBuffer(value, new IntPtr((void*)ptr2), 256);
				}
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x000444E8 File Offset: 0x000426E8
		// (set) Token: 0x06000D2B RID: 3371 RVA: 0x00044514 File Offset: 0x00042714
		public unsafe string name
		{
			get
			{
				fixed (byte* ptr = &this.nameBuffer.FixedElementField)
				{
					return StringHelpers.ReadStringFromBuffer(new IntPtr((void*)ptr), 256);
				}
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length > 256)
				{
					throw new ArgumentException(string.Format("Name '{0}' exceeds maximum supported length of {1} characters", value, 256), "value");
				}
				fixed (byte* ptr = &this.nameBuffer.FixedElementField)
				{
					byte* ptr2 = ptr;
					StringHelpers.WriteStringToBuffer(value, new IntPtr((void*)ptr2), 256);
				}
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x00044580 File Offset: 0x00042780
		public FourCC typeStatic
		{
			get
			{
				return QueryPairedUserAccountCommand.Type;
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00044588 File Offset: 0x00042788
		public static QueryPairedUserAccountCommand Create()
		{
			return new QueryPairedUserAccountCommand
			{
				baseCommand = new InputDeviceCommand(QueryPairedUserAccountCommand.Type, 1040)
			};
		}

		// Token: 0x04000516 RID: 1302
		internal const int kMaxNameLength = 256;

		// Token: 0x04000517 RID: 1303
		internal const int kMaxIdLength = 256;

		// Token: 0x04000518 RID: 1304
		internal const int kSize = 1040;

		// Token: 0x04000519 RID: 1305
		[FieldOffset(0)]
		public InputDeviceCommand baseCommand;

		// Token: 0x0400051A RID: 1306
		[FieldOffset(8)]
		public ulong handle;

		// Token: 0x0400051B RID: 1307
		[FixedBuffer(typeof(byte), 512)]
		[FieldOffset(16)]
		internal QueryPairedUserAccountCommand.<nameBuffer>e__FixedBuffer nameBuffer;

		// Token: 0x0400051C RID: 1308
		[FixedBuffer(typeof(byte), 512)]
		[FieldOffset(528)]
		internal QueryPairedUserAccountCommand.<idBuffer>e__FixedBuffer idBuffer;

		// Token: 0x0200020A RID: 522
		[Flags]
		public enum Result : long
		{
			// Token: 0x04000B85 RID: 2949
			DevicePairedToUserAccount = 2L,
			// Token: 0x04000B86 RID: 2950
			UserAccountSelectionInProgress = 4L,
			// Token: 0x04000B87 RID: 2951
			UserAccountSelectionComplete = 8L,
			// Token: 0x04000B88 RID: 2952
			UserAccountSelectionCanceled = 16L
		}

		// Token: 0x0200020B RID: 523
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 512)]
		public struct <idBuffer>e__FixedBuffer
		{
			// Token: 0x04000B89 RID: 2953
			public byte FixedElementField;
		}

		// Token: 0x0200020C RID: 524
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 512)]
		public struct <nameBuffer>e__FixedBuffer
		{
			// Token: 0x04000B8A RID: 2954
			public byte FixedElementField;
		}
	}
}
