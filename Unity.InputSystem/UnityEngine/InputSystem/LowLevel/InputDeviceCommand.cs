using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000BA RID: 186
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public struct InputDeviceCommand : IInputDeviceCommandInfo
	{
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x000441E4 File Offset: 0x000423E4
		public int payloadSizeInBytes
		{
			get
			{
				return this.sizeInBytes - 8;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x000441F0 File Offset: 0x000423F0
		public unsafe void* payloadPtr
		{
			get
			{
				fixed (InputDeviceCommand* ptr = &this)
				{
					void* ptr2 = (void*)ptr;
					return (void*)((byte*)ptr2 + 8);
				}
			}
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00044205 File Offset: 0x00042405
		public InputDeviceCommand(FourCC type, int sizeInBytes = 8)
		{
			this.type = type;
			this.sizeInBytes = sizeInBytes;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00044218 File Offset: 0x00042418
		public unsafe static NativeArray<byte> AllocateNative(FourCC type, int payloadSize)
		{
			int num = payloadSize + 8;
			NativeArray<byte> nativeArray = new NativeArray<byte>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			InputDeviceCommand* unsafePtr = (InputDeviceCommand*)nativeArray.GetUnsafePtr<byte>();
			unsafePtr->type = type;
			unsafePtr->sizeInBytes = num;
			return nativeArray;
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00044248 File Offset: 0x00042448
		public FourCC typeStatic
		{
			get
			{
				return default(FourCC);
			}
		}

		// Token: 0x040004FF RID: 1279
		internal const int kBaseCommandSize = 8;

		// Token: 0x04000500 RID: 1280
		public const int BaseCommandSize = 8;

		// Token: 0x04000501 RID: 1281
		public const long GenericFailure = -1L;

		// Token: 0x04000502 RID: 1282
		public const long GenericSuccess = 1L;

		// Token: 0x04000503 RID: 1283
		[FieldOffset(0)]
		public FourCC type;

		// Token: 0x04000504 RID: 1284
		[FieldOffset(4)]
		public int sizeInBytes;
	}
}
