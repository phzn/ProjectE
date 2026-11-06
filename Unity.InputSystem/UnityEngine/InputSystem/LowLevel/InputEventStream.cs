using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000EA RID: 234
	internal struct InputEventStream
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x00045F05 File Offset: 0x00044105
		public bool isOpen
		{
			get
			{
				return this.m_IsOpen;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x00045F0D File Offset: 0x0004410D
		public int remainingEventCount
		{
			get
			{
				return this.m_RemainingNativeEventCount + this.m_RemainingAppendEventCount;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x00045F1C File Offset: 0x0004411C
		public int numEventsRetainedInBuffer
		{
			get
			{
				return this.m_NumEventsRetainedInBuffer;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000DFB RID: 3579 RVA: 0x00045F24 File Offset: 0x00044124
		public unsafe InputEvent* currentEventPtr
		{
			get
			{
				if (this.m_RemainingNativeEventCount > 0)
				{
					return this.m_CurrentNativeEventReadPtr;
				}
				if (this.m_RemainingAppendEventCount <= 0)
				{
					return null;
				}
				return this.m_CurrentAppendEventReadPtr;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x00045F48 File Offset: 0x00044148
		public unsafe uint numBytesRetainedInBuffer
		{
			get
			{
				return (uint)((long)((byte*)this.m_CurrentNativeEventWritePtr - (byte*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(this.m_NativeBuffer.data)));
			}
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00045F68 File Offset: 0x00044168
		public unsafe InputEventStream(ref InputEventBuffer eventBuffer, int maxAppendedEvents)
		{
			this.m_CurrentNativeEventWritePtr = (this.m_CurrentNativeEventReadPtr = (InputEvent*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(eventBuffer.data));
			this.m_NativeBuffer = eventBuffer;
			this.m_RemainingNativeEventCount = this.m_NativeBuffer.eventCount;
			this.m_NumEventsRetainedInBuffer = 0;
			this.m_CurrentAppendEventReadPtr = (this.m_CurrentAppendEventWritePtr = null);
			this.m_AppendBuffer = default(InputEventBuffer);
			this.m_RemainingAppendEventCount = 0;
			this.m_MaxAppendedEvents = maxAppendedEvents;
			this.m_IsOpen = true;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00045FE8 File Offset: 0x000441E8
		public unsafe void Close(ref InputEventBuffer eventBuffer)
		{
			if (this.m_NumEventsRetainedInBuffer > 0)
			{
				void* unsafeBufferPointerWithoutChecks = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(this.m_NativeBuffer.data);
				long num = (long)((byte*)this.m_CurrentNativeEventWritePtr - (byte*)unsafeBufferPointerWithoutChecks);
				this.m_NativeBuffer = new InputEventBuffer((InputEvent*)unsafeBufferPointerWithoutChecks, this.m_NumEventsRetainedInBuffer, (int)num, (int)this.m_NativeBuffer.capacityInBytes);
			}
			else
			{
				this.m_NativeBuffer.Reset();
			}
			if (this.m_AppendBuffer.data.IsCreated)
			{
				this.m_AppendBuffer.Dispose();
			}
			eventBuffer = this.m_NativeBuffer;
			this.m_IsOpen = false;
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0004607C File Offset: 0x0004427C
		public void CleanUpAfterException()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.m_NativeBuffer.Reset();
			if (this.m_AppendBuffer.data.IsCreated)
			{
				this.m_AppendBuffer.Dispose();
			}
			this.m_IsOpen = false;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x000460C4 File Offset: 0x000442C4
		public unsafe void Write(InputEvent* eventPtr)
		{
			if (this.m_AppendBuffer.eventCount >= this.m_MaxAppendedEvents)
			{
				Debug.LogError("Maximum number of queued events exceeded. Set the 'maxQueuedEventsPerUpdate' setting to a higher value if you need to queue more events than this. " + string.Format("Current limit is '{0}'.", this.m_MaxAppendedEvents));
				return;
			}
			bool isCreated = this.m_AppendBuffer.data.IsCreated;
			byte* data = (byte*)this.m_AppendBuffer.bufferPtr.data;
			this.m_AppendBuffer.AppendEvent(eventPtr, 2048, Allocator.Temp);
			if (!isCreated)
			{
				this.m_CurrentAppendEventWritePtr = (this.m_CurrentAppendEventReadPtr = (InputEvent*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(this.m_AppendBuffer.data));
			}
			else
			{
				byte* data2 = (byte*)this.m_AppendBuffer.bufferPtr.data;
				if (data != data2)
				{
					long num = (long)((byte*)this.m_CurrentAppendEventWritePtr - (byte*)data);
					long num2 = (long)((byte*)this.m_CurrentAppendEventReadPtr - (byte*)data);
					this.m_CurrentAppendEventWritePtr = (InputEvent*)(data2 + num);
					this.m_CurrentAppendEventReadPtr = (InputEvent*)(data2 + num2);
				}
			}
			this.m_RemainingAppendEventCount++;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x000461C0 File Offset: 0x000443C0
		public unsafe InputEvent* Advance(bool leaveEventInBuffer)
		{
			if (this.m_RemainingNativeEventCount > 0)
			{
				this.m_NativeBuffer.AdvanceToNextEvent(ref this.m_CurrentNativeEventReadPtr, ref this.m_CurrentNativeEventWritePtr, ref this.m_NumEventsRetainedInBuffer, ref this.m_RemainingNativeEventCount, leaveEventInBuffer);
			}
			else if (this.m_RemainingAppendEventCount > 0)
			{
				int num = 0;
				this.m_AppendBuffer.AdvanceToNextEvent(ref this.m_CurrentAppendEventReadPtr, ref this.m_CurrentAppendEventWritePtr, ref num, ref this.m_RemainingAppendEventCount, false);
			}
			return this.currentEventPtr;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00046230 File Offset: 0x00044430
		public unsafe InputEvent* Peek()
		{
			if (this.m_RemainingNativeEventCount > 1)
			{
				return InputEvent.GetNextInMemory(this.m_CurrentNativeEventReadPtr);
			}
			if (this.m_RemainingNativeEventCount == 1)
			{
				if (this.m_RemainingAppendEventCount <= 0)
				{
					return null;
				}
				return this.m_CurrentAppendEventReadPtr;
			}
			else
			{
				if (this.m_RemainingAppendEventCount > 1)
				{
					return InputEvent.GetNextInMemory(this.m_CurrentAppendEventReadPtr);
				}
				return null;
			}
		}

		// Token: 0x040005BA RID: 1466
		private InputEventBuffer m_NativeBuffer;

		// Token: 0x040005BB RID: 1467
		private unsafe InputEvent* m_CurrentNativeEventReadPtr;

		// Token: 0x040005BC RID: 1468
		private unsafe InputEvent* m_CurrentNativeEventWritePtr;

		// Token: 0x040005BD RID: 1469
		private int m_RemainingNativeEventCount;

		// Token: 0x040005BE RID: 1470
		private readonly int m_MaxAppendedEvents;

		// Token: 0x040005BF RID: 1471
		private InputEventBuffer m_AppendBuffer;

		// Token: 0x040005C0 RID: 1472
		private unsafe InputEvent* m_CurrentAppendEventReadPtr;

		// Token: 0x040005C1 RID: 1473
		private unsafe InputEvent* m_CurrentAppendEventWritePtr;

		// Token: 0x040005C2 RID: 1474
		private int m_RemainingAppendEventCount;

		// Token: 0x040005C3 RID: 1475
		private int m_NumEventsRetainedInBuffer;

		// Token: 0x040005C4 RID: 1476
		private bool m_IsOpen;
	}
}
