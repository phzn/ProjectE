using System;
using System.Diagnostics.Tracing;

namespace System.Buffers
{
	// Token: 0x02000AD2 RID: 2770
	[EventSource(Guid = "0866B2B8-5CEF-5DB9-2612-0C0FFD814A44", Name = "System.Buffers.ArrayPoolEventSource")]
	internal sealed class ArrayPoolEventSource : EventSource
	{
		// Token: 0x060062D4 RID: 25300 RVA: 0x0014A59C File Offset: 0x0014879C
		private ArrayPoolEventSource()
			: base(new Guid(140948152, 23791, 23993, 38, 18, 12, 15, 253, 129, 74, 68), "System.Buffers.ArrayPoolEventSource")
		{
		}

		// Token: 0x060062D5 RID: 25301 RVA: 0x0014A5E0 File Offset: 0x001487E0
		[Event(1, Level = EventLevel.Verbose)]
		internal unsafe void BufferRented(int bufferId, int bufferSize, int poolId, int bucketId)
		{
			EventSource.EventData* ptr;
			checked
			{
				ptr = stackalloc EventSource.EventData[unchecked((UIntPtr)4) * (UIntPtr)sizeof(EventSource.EventData)];
				ptr->Size = 4;
			}
			ptr->DataPointer = (IntPtr)((void*)(&bufferId));
			ptr->Reserved = 0;
			ptr[1].Size = 4;
			ptr[1].DataPointer = (IntPtr)((void*)(&bufferSize));
			ptr[1].Reserved = 0;
			ptr[2].Size = 4;
			ptr[2].DataPointer = (IntPtr)((void*)(&poolId));
			ptr[2].Reserved = 0;
			ptr[3].Size = 4;
			ptr[3].DataPointer = (IntPtr)((void*)(&bucketId));
			ptr[3].Reserved = 0;
			base.WriteEventCore(1, 4, ptr);
		}

		// Token: 0x060062D6 RID: 25302 RVA: 0x0014A6C4 File Offset: 0x001488C4
		[Event(2, Level = EventLevel.Informational)]
		internal unsafe void BufferAllocated(int bufferId, int bufferSize, int poolId, int bucketId, ArrayPoolEventSource.BufferAllocatedReason reason)
		{
			EventSource.EventData* ptr;
			checked
			{
				ptr = stackalloc EventSource.EventData[unchecked((UIntPtr)5) * (UIntPtr)sizeof(EventSource.EventData)];
				ptr->Size = 4;
			}
			ptr->DataPointer = (IntPtr)((void*)(&bufferId));
			ptr->Reserved = 0;
			ptr[1].Size = 4;
			ptr[1].DataPointer = (IntPtr)((void*)(&bufferSize));
			ptr[1].Reserved = 0;
			ptr[2].Size = 4;
			ptr[2].DataPointer = (IntPtr)((void*)(&poolId));
			ptr[2].Reserved = 0;
			ptr[3].Size = 4;
			ptr[3].DataPointer = (IntPtr)((void*)(&bucketId));
			ptr[3].Reserved = 0;
			ptr[4].Size = 4;
			ptr[4].DataPointer = (IntPtr)((void*)(&reason));
			ptr[4].Reserved = 0;
			base.WriteEventCore(2, 5, ptr);
		}

		// Token: 0x060062D7 RID: 25303 RVA: 0x0014A7E1 File Offset: 0x001489E1
		[Event(3, Level = EventLevel.Verbose)]
		internal void BufferReturned(int bufferId, int bufferSize, int poolId)
		{
			base.WriteEvent(3, bufferId, bufferSize, poolId);
		}

		// Token: 0x060062D8 RID: 25304 RVA: 0x0014A7ED File Offset: 0x001489ED
		[Event(4, Level = EventLevel.Informational)]
		internal void BufferTrimmed(int bufferId, int bufferSize, int poolId)
		{
			base.WriteEvent(4, bufferId, bufferSize, poolId);
		}

		// Token: 0x060062D9 RID: 25305 RVA: 0x0014A7F9 File Offset: 0x001489F9
		[Event(5, Level = EventLevel.Informational)]
		internal void BufferTrimPoll(int milliseconds, int pressure)
		{
			base.WriteEvent(5, milliseconds, pressure);
		}

		// Token: 0x04003A38 RID: 14904
		internal static readonly ArrayPoolEventSource Log = new ArrayPoolEventSource();

		// Token: 0x02000AD3 RID: 2771
		internal enum BufferAllocatedReason
		{
			// Token: 0x04003A3A RID: 14906
			Pooled,
			// Token: 0x04003A3B RID: 14907
			OverMaximumSize,
			// Token: 0x04003A3C RID: 14908
			PoolExhausted
		}
	}
}
