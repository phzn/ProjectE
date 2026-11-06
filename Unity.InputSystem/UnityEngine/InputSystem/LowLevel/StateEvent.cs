using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000EC RID: 236
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 25)]
	public struct StateEvent : IInputEventTypeInfo
	{
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000E2F RID: 3631 RVA: 0x00047162 File Offset: 0x00045362
		public uint stateSizeInBytes
		{
			get
			{
				return this.baseEvent.sizeInBytes - 24U;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x00047174 File Offset: 0x00045374
		public unsafe void* state
		{
			get
			{
				fixed (byte* ptr = &this.stateData.FixedElementField)
				{
					return (void*)ptr;
				}
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00047190 File Offset: 0x00045390
		public unsafe InputEventPtr ToEventPtr()
		{
			fixed (StateEvent* ptr = &this)
			{
				return new InputEventPtr((InputEvent*)ptr);
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x000471A6 File Offset: 0x000453A6
		public FourCC typeStatic
		{
			get
			{
				return 1398030676;
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x000471B4 File Offset: 0x000453B4
		public TState GetState<TState>() where TState : struct, IInputStateTypeInfo
		{
			TState tstate = default(TState);
			if (this.stateFormat != tstate.format)
			{
				throw new InvalidOperationException(string.Format("Expected state format '{0}' but got '{1}' instead", tstate.format, this.stateFormat));
			}
			UnsafeUtility.MemCpy(UnsafeUtility.AddressOf<TState>(ref tstate), this.state, Math.Min((long)((ulong)this.stateSizeInBytes), (long)UnsafeUtility.SizeOf<TState>()));
			return tstate;
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00047235 File Offset: 0x00045435
		public unsafe static TState GetState<TState>(InputEventPtr ptr) where TState : struct, IInputStateTypeInfo
		{
			return StateEvent.From(ptr)->GetState<TState>();
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00047242 File Offset: 0x00045442
		public static int GetEventSizeWithPayload<TState>() where TState : struct
		{
			return UnsafeUtility.SizeOf<TState>() + 20 + 4;
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00047250 File Offset: 0x00045450
		public unsafe static StateEvent* From(InputEventPtr ptr)
		{
			if (!ptr.valid)
			{
				throw new ArgumentNullException("ptr");
			}
			if (!ptr.IsA<StateEvent>())
			{
				throw new InvalidCastException(string.Format("Cannot cast event with type '{0}' into StateEvent", ptr.type));
			}
			return StateEvent.FromUnchecked(ptr);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0004729C File Offset: 0x0004549C
		internal unsafe static StateEvent* FromUnchecked(InputEventPtr ptr)
		{
			return (StateEvent*)ptr.data;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x000472A5 File Offset: 0x000454A5
		public static NativeArray<byte> From(InputDevice device, out InputEventPtr eventPtr, Allocator allocator = Allocator.Temp)
		{
			return StateEvent.From(device, out eventPtr, allocator, false);
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x000472B0 File Offset: 0x000454B0
		public static NativeArray<byte> FromDefaultStateFor(InputDevice device, out InputEventPtr eventPtr, Allocator allocator = Allocator.Temp)
		{
			return StateEvent.From(device, out eventPtr, allocator, true);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x000472BC File Offset: 0x000454BC
		private unsafe static NativeArray<byte> From(InputDevice device, out InputEventPtr eventPtr, Allocator allocator, bool useDefaultState)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			if (!device.added)
			{
				throw new ArgumentException(string.Format("Device '{0}' has not been added to system", device), "device");
			}
			FourCC format = device.m_StateBlock.format;
			uint alignedSizeInBytes = device.m_StateBlock.alignedSizeInBytes;
			uint byteOffset = device.m_StateBlock.byteOffset;
			byte* ptr = (byte*)((useDefaultState ? device.defaultStatePtr : device.currentStatePtr) + byteOffset);
			uint num = 24U + alignedSizeInBytes;
			NativeArray<byte> nativeArray = new NativeArray<byte>((int)num.AlignToMultipleOf(4U), allocator, NativeArrayOptions.ClearMemory);
			StateEvent* unsafePtr = (StateEvent*)nativeArray.GetUnsafePtr<byte>();
			unsafePtr->baseEvent = new InputEvent(1398030676, (int)num, device.deviceId, InputRuntime.s_Instance.currentTime);
			unsafePtr->stateFormat = format;
			UnsafeUtility.MemCpy(unsafePtr->state, (void*)ptr, (long)((ulong)alignedSizeInBytes));
			eventPtr = unsafePtr->ToEventPtr();
			return nativeArray;
		}

		// Token: 0x040005D8 RID: 1496
		public const int Type = 1398030676;

		// Token: 0x040005D9 RID: 1497
		internal const int kStateDataSizeToSubtract = 1;

		// Token: 0x040005DA RID: 1498
		[FieldOffset(0)]
		public InputEvent baseEvent;

		// Token: 0x040005DB RID: 1499
		[FieldOffset(20)]
		public FourCC stateFormat;

		// Token: 0x040005DC RID: 1500
		[FixedBuffer(typeof(byte), 1)]
		[FieldOffset(24)]
		internal StateEvent.<stateData>e__FixedBuffer stateData;

		// Token: 0x0200021D RID: 541
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct <stateData>e__FixedBuffer
		{
			// Token: 0x04000BBA RID: 3002
			public byte FixedElementField;
		}
	}
}
