using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000DF RID: 223
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 29)]
	public struct DeltaStateEvent : IInputEventTypeInfo
	{
		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x00044EFD File Offset: 0x000430FD
		public uint deltaStateSizeInBytes
		{
			get
			{
				return this.baseEvent.sizeInBytes - 28U;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00044F10 File Offset: 0x00043110
		public unsafe void* deltaState
		{
			get
			{
				fixed (byte* ptr = &this.stateData.FixedElementField)
				{
					return (void*)ptr;
				}
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x00044F2B File Offset: 0x0004312B
		public FourCC typeStatic
		{
			get
			{
				return 1145852993;
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00044F38 File Offset: 0x00043138
		public unsafe InputEventPtr ToEventPtr()
		{
			fixed (DeltaStateEvent* ptr = &this)
			{
				return new InputEventPtr((InputEvent*)ptr);
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00044F50 File Offset: 0x00043150
		public unsafe static DeltaStateEvent* From(InputEventPtr ptr)
		{
			if (!ptr.valid)
			{
				throw new ArgumentNullException("ptr");
			}
			if (!ptr.IsA<DeltaStateEvent>())
			{
				throw new InvalidCastException(string.Format("Cannot cast event with type '{0}' into DeltaStateEvent", ptr.type));
			}
			return DeltaStateEvent.FromUnchecked(ptr);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00044F9C File Offset: 0x0004319C
		internal unsafe static DeltaStateEvent* FromUnchecked(InputEventPtr ptr)
		{
			return (DeltaStateEvent*)ptr.data;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00044FA8 File Offset: 0x000431A8
		public unsafe static NativeArray<byte> From(InputControl control, out InputEventPtr eventPtr, Allocator allocator = Allocator.Temp)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			InputDevice device = control.device;
			if (!device.added)
			{
				throw new ArgumentException(string.Format("Device for control '{0}' has not been added to system", control), "control");
			}
			ref InputStateBlock ptr = ref device.m_StateBlock;
			ref InputStateBlock ptr2 = ref control.m_StateBlock;
			FourCC format = ptr.format;
			uint num;
			if (ptr2.bitOffset != 0U)
			{
				num = (ptr2.bitOffset + ptr2.sizeInBits + 7U) / 8U;
			}
			else
			{
				num = ptr2.alignedSizeInBytes;
			}
			uint byteOffset = ptr2.byteOffset;
			byte* ptr3 = (byte*)control.currentStatePtr + byteOffset;
			uint num2 = 28U + num;
			NativeArray<byte> nativeArray = new NativeArray<byte>((int)num2.AlignToMultipleOf(4U), allocator, NativeArrayOptions.ClearMemory);
			DeltaStateEvent* unsafePtr = (DeltaStateEvent*)nativeArray.GetUnsafePtr<byte>();
			unsafePtr->baseEvent = new InputEvent(1145852993, (int)num2, device.deviceId, InputRuntime.s_Instance.currentTime);
			unsafePtr->stateFormat = format;
			unsafePtr->stateOffset = ptr2.byteOffset - ptr.byteOffset;
			UnsafeUtility.MemCpy(unsafePtr->deltaState, (void*)ptr3, (long)((ulong)num));
			eventPtr = unsafePtr->ToEventPtr();
			return nativeArray;
		}

		// Token: 0x0400059B RID: 1435
		public const int Type = 1145852993;

		// Token: 0x0400059C RID: 1436
		[FieldOffset(0)]
		public InputEvent baseEvent;

		// Token: 0x0400059D RID: 1437
		[FieldOffset(20)]
		public FourCC stateFormat;

		// Token: 0x0400059E RID: 1438
		[FieldOffset(24)]
		public uint stateOffset;

		// Token: 0x0400059F RID: 1439
		[FixedBuffer(typeof(byte), 1)]
		[FieldOffset(28)]
		internal DeltaStateEvent.<stateData>e__FixedBuffer stateData;

		// Token: 0x02000213 RID: 531
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct <stateData>e__FixedBuffer
		{
			// Token: 0x04000B96 RID: 2966
			public byte FixedElementField;
		}
	}
}
