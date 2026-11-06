using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000E7 RID: 231
	public struct InputEventBuffer : IEnumerable<InputEventPtr>, IEnumerable, IDisposable, ICloneable
	{
		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x000455D7 File Offset: 0x000437D7
		public int eventCount
		{
			get
			{
				return this.m_EventCount;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x000455DF File Offset: 0x000437DF
		public long sizeInBytes
		{
			get
			{
				return this.m_SizeInBytes;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x000455E7 File Offset: 0x000437E7
		public long capacityInBytes
		{
			get
			{
				if (!this.m_Buffer.IsCreated)
				{
					return 0L;
				}
				return (long)this.m_Buffer.Length;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00045605 File Offset: 0x00043805
		public NativeArray<byte> data
		{
			get
			{
				return this.m_Buffer;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x0004560D File Offset: 0x0004380D
		public unsafe InputEventPtr bufferPtr
		{
			get
			{
				return (InputEvent*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(this.m_Buffer);
			}
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00045620 File Offset: 0x00043820
		public unsafe InputEventBuffer(InputEvent* eventPtr, int eventCount, int sizeInBytes = -1, int capacityInBytes = -1)
		{
			this = default(InputEventBuffer);
			if (eventPtr == null && eventCount != 0)
			{
				throw new ArgumentException("eventPtr is NULL but eventCount is != 0", "eventCount");
			}
			if (capacityInBytes != 0 && capacityInBytes < sizeInBytes)
			{
				throw new ArgumentException(string.Format("capacity({0}) cannot be smaller than size({1})", capacityInBytes, sizeInBytes), "capacityInBytes");
			}
			if (eventPtr != null)
			{
				if (capacityInBytes < 0)
				{
					capacityInBytes = sizeInBytes;
				}
				this.m_Buffer = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>((void*)eventPtr, (capacityInBytes > 0) ? capacityInBytes : 0, Allocator.None);
				this.m_SizeInBytes = ((sizeInBytes >= 0) ? ((long)sizeInBytes) : (-1L));
				this.m_EventCount = eventCount;
				this.m_WeOwnTheBuffer = false;
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x000456BC File Offset: 0x000438BC
		public InputEventBuffer(NativeArray<byte> buffer, int eventCount, int sizeInBytes = -1, bool transferNativeArrayOwnership = false)
		{
			if (eventCount > 0 && !buffer.IsCreated)
			{
				throw new ArgumentException("buffer has no data but eventCount is > 0", "eventCount");
			}
			if (sizeInBytes > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("sizeInBytes");
			}
			this.m_Buffer = buffer;
			this.m_WeOwnTheBuffer = transferNativeArrayOwnership;
			this.m_SizeInBytes = (long)((sizeInBytes >= 0) ? sizeInBytes : buffer.Length);
			this.m_EventCount = eventCount;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x00045728 File Offset: 0x00043928
		public unsafe void AppendEvent(InputEvent* eventPtr, int capacityIncrementInBytes = 2048, Allocator allocator = Allocator.Persistent)
		{
			if (eventPtr == null)
			{
				throw new ArgumentNullException("eventPtr");
			}
			uint sizeInBytes = eventPtr->sizeInBytes;
			UnsafeUtility.MemCpy((void*)this.AllocateEvent((int)sizeInBytes, capacityIncrementInBytes, allocator), (void*)eventPtr, (long)((ulong)sizeInBytes));
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00045760 File Offset: 0x00043960
		public unsafe InputEvent* AllocateEvent(int sizeInBytes, int capacityIncrementInBytes = 2048, Allocator allocator = Allocator.Persistent)
		{
			if (sizeInBytes < 20)
			{
				throw new ArgumentException(string.Format("sizeInBytes must be >= sizeof(InputEvent) == {0} (was {1})", 20, sizeInBytes), "sizeInBytes");
			}
			int num = sizeInBytes.AlignToMultipleOf(4);
			long num2 = this.m_SizeInBytes + (long)num;
			if (this.capacityInBytes < num2)
			{
				long num3 = num2.AlignToMultipleOf((long)capacityIncrementInBytes);
				if (num3 > 2147483647L)
				{
					throw new NotImplementedException("NativeArray long support");
				}
				NativeArray<byte> nativeArray = new NativeArray<byte>((int)num3, allocator, NativeArrayOptions.ClearMemory);
				if (this.m_Buffer.IsCreated)
				{
					UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<byte>(), NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(this.m_Buffer), this.sizeInBytes);
					if (this.m_WeOwnTheBuffer)
					{
						this.m_Buffer.Dispose();
					}
				}
				this.m_Buffer = nativeArray;
				this.m_WeOwnTheBuffer = true;
			}
			InputEvent* ptr = (InputEvent*)((byte*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(this.m_Buffer) + this.m_SizeInBytes);
			ptr->sizeInBytes = (uint)sizeInBytes;
			this.m_SizeInBytes += (long)num;
			this.m_EventCount++;
			return ptr;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0004585C File Offset: 0x00043A5C
		public unsafe bool Contains(InputEvent* eventPtr)
		{
			if (eventPtr == null)
			{
				return false;
			}
			if (this.sizeInBytes == 0L)
			{
				return false;
			}
			void* unsafeBufferPointerWithoutChecks = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(this.data);
			return eventPtr >= (InputEvent*)unsafeBufferPointerWithoutChecks && (this.sizeInBytes == -1L || eventPtr < (InputEvent*)((byte*)unsafeBufferPointerWithoutChecks + this.sizeInBytes));
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x000458A5 File Offset: 0x00043AA5
		public void Reset()
		{
			this.m_EventCount = 0;
			if (this.m_SizeInBytes != -1L)
			{
				this.m_SizeInBytes = 0L;
			}
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x000458C0 File Offset: 0x00043AC0
		internal unsafe void AdvanceToNextEvent(ref InputEvent* currentReadPos, ref InputEvent* currentWritePos, ref int numEventsRetainedInBuffer, ref int numRemainingEvents, bool leaveEventInBuffer)
		{
			InputEvent* ptr = currentReadPos;
			if (numRemainingEvents > 1)
			{
				ptr = InputEvent.GetNextInMemory(currentReadPos);
			}
			if (leaveEventInBuffer)
			{
				uint sizeInBytes = currentReadPos.sizeInBytes;
				if (currentReadPos != currentWritePos)
				{
					UnsafeUtility.MemMove(currentWritePos, currentReadPos, (long)((ulong)sizeInBytes));
				}
				currentWritePos += (IntPtr)((UIntPtr)sizeInBytes.AlignToMultipleOf(4U));
				numEventsRetainedInBuffer++;
			}
			currentReadPos = ptr;
			numRemainingEvents--;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00045919 File Offset: 0x00043B19
		public IEnumerator<InputEventPtr> GetEnumerator()
		{
			return new InputEventBuffer.Enumerator(this);
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0004592B File Offset: 0x00043B2B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00045933 File Offset: 0x00043B33
		public void Dispose()
		{
			if (!this.m_WeOwnTheBuffer)
			{
				return;
			}
			this.m_Buffer.Dispose();
			this.m_WeOwnTheBuffer = false;
			this.m_SizeInBytes = 0L;
			this.m_EventCount = 0;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00045960 File Offset: 0x00043B60
		public InputEventBuffer Clone()
		{
			InputEventBuffer inputEventBuffer = default(InputEventBuffer);
			if (this.m_Buffer.IsCreated)
			{
				inputEventBuffer.m_Buffer = new NativeArray<byte>(this.m_Buffer.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				inputEventBuffer.m_Buffer.CopyFrom(this.m_Buffer);
				inputEventBuffer.m_WeOwnTheBuffer = true;
			}
			inputEventBuffer.m_SizeInBytes = this.m_SizeInBytes;
			inputEventBuffer.m_EventCount = this.m_EventCount;
			return inputEventBuffer;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x000459D0 File Offset: 0x00043BD0
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x040005B3 RID: 1459
		public const long BufferSizeUnknown = -1L;

		// Token: 0x040005B4 RID: 1460
		private NativeArray<byte> m_Buffer;

		// Token: 0x040005B5 RID: 1461
		private long m_SizeInBytes;

		// Token: 0x040005B6 RID: 1462
		private int m_EventCount;

		// Token: 0x040005B7 RID: 1463
		private bool m_WeOwnTheBuffer;

		// Token: 0x02000216 RID: 534
		private struct Enumerator : IEnumerator<InputEventPtr>, IEnumerator, IDisposable
		{
			// Token: 0x06001500 RID: 5376 RVA: 0x00062248 File Offset: 0x00060448
			public Enumerator(InputEventBuffer buffer)
			{
				this.m_Buffer = buffer.bufferPtr;
				this.m_EventCount = buffer.m_EventCount;
				this.m_CurrentEvent = null;
				this.m_CurrentIndex = 0;
			}

			// Token: 0x06001501 RID: 5377 RVA: 0x00062278 File Offset: 0x00060478
			public bool MoveNext()
			{
				if (this.m_CurrentIndex == this.m_EventCount)
				{
					return false;
				}
				if (this.m_CurrentEvent == null)
				{
					this.m_CurrentEvent = this.m_Buffer;
					return this.m_CurrentEvent != null;
				}
				this.m_CurrentIndex++;
				if (this.m_CurrentIndex == this.m_EventCount)
				{
					return false;
				}
				this.m_CurrentEvent = InputEvent.GetNextInMemory(this.m_CurrentEvent);
				return true;
			}

			// Token: 0x06001502 RID: 5378 RVA: 0x000622E9 File Offset: 0x000604E9
			public void Reset()
			{
				this.m_CurrentEvent = null;
				this.m_CurrentIndex = 0;
			}

			// Token: 0x06001503 RID: 5379 RVA: 0x000622FA File Offset: 0x000604FA
			public void Dispose()
			{
			}

			// Token: 0x170005A6 RID: 1446
			// (get) Token: 0x06001504 RID: 5380 RVA: 0x000622FC File Offset: 0x000604FC
			public InputEventPtr Current
			{
				get
				{
					return this.m_CurrentEvent;
				}
			}

			// Token: 0x170005A7 RID: 1447
			// (get) Token: 0x06001505 RID: 5381 RVA: 0x00062309 File Offset: 0x00060509
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04000B9B RID: 2971
			private unsafe readonly InputEvent* m_Buffer;

			// Token: 0x04000B9C RID: 2972
			private readonly int m_EventCount;

			// Token: 0x04000B9D RID: 2973
			private unsafe InputEvent* m_CurrentEvent;

			// Token: 0x04000B9E RID: 2974
			private int m_CurrentIndex;
		}
	}
}
