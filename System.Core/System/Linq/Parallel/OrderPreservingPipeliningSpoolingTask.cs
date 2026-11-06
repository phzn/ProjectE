using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
	// Token: 0x020001D4 RID: 468
	internal class OrderPreservingPipeliningSpoolingTask<TOutput, TKey> : SpoolingTaskBase
	{
		// Token: 0x06000BBA RID: 3002 RVA: 0x00029428 File Offset: 0x00027628
		internal OrderPreservingPipeliningSpoolingTask(QueryOperatorEnumerator<TOutput, TKey> partition, QueryTaskGroupState taskGroupState, bool[] consumerWaiting, bool[] producerWaiting, bool[] producerDone, int partitionIndex, Queue<Pair<TKey, TOutput>>[] buffers, object bufferLock, bool autoBuffered)
			: base(partitionIndex, taskGroupState)
		{
			this._partition = partition;
			this._taskGroupState = taskGroupState;
			this._producerDone = producerDone;
			this._consumerWaiting = consumerWaiting;
			this._producerWaiting = producerWaiting;
			this._partitionIndex = partitionIndex;
			this._buffers = buffers;
			this._bufferLock = bufferLock;
			this._autoBuffered = autoBuffered;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00029484 File Offset: 0x00027684
		protected override void SpoolingWork()
		{
			TOutput toutput = default(TOutput);
			TKey tkey = default(TKey);
			int num = (this._autoBuffered ? 16 : 1);
			Pair<TKey, TOutput>[] array = new Pair<TKey, TOutput>[num];
			QueryOperatorEnumerator<TOutput, TKey> partition = this._partition;
			CancellationToken mergedCancellationToken = this._taskGroupState.CancellationState.MergedCancellationToken;
			int num2;
			do
			{
				num2 = 0;
				while (num2 < num && partition.MoveNext(ref toutput, ref tkey))
				{
					array[num2] = new Pair<TKey, TOutput>(tkey, toutput);
					num2++;
				}
				if (num2 == 0)
				{
					break;
				}
				object bufferLock = this._bufferLock;
				lock (bufferLock)
				{
					if (mergedCancellationToken.IsCancellationRequested)
					{
						break;
					}
					for (int i = 0; i < num2; i++)
					{
						this._buffers[this._partitionIndex].Enqueue(array[i]);
					}
					if (this._consumerWaiting[this._partitionIndex])
					{
						Monitor.Pulse(this._bufferLock);
						this._consumerWaiting[this._partitionIndex] = false;
					}
					if (this._buffers[this._partitionIndex].Count >= 8192)
					{
						this._producerWaiting[this._partitionIndex] = true;
						Monitor.Wait(this._bufferLock);
					}
				}
			}
			while (num2 == num);
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x000295D4 File Offset: 0x000277D4
		public static void Spool(QueryTaskGroupState groupState, PartitionedStream<TOutput, TKey> partitions, bool[] consumerWaiting, bool[] producerWaiting, bool[] producerDone, Queue<Pair<TKey, TOutput>>[] buffers, object[] bufferLocks, TaskScheduler taskScheduler, bool autoBuffered)
		{
			int degreeOfParallelism = partitions.PartitionCount;
			for (int i = 0; i < degreeOfParallelism; i++)
			{
				buffers[i] = new Queue<Pair<TKey, TOutput>>(128);
				bufferLocks[i] = new object();
			}
			Task task = new Task(delegate
			{
				for (int j = 0; j < degreeOfParallelism; j++)
				{
					new OrderPreservingPipeliningSpoolingTask<TOutput, TKey>(partitions[j], groupState, consumerWaiting, producerWaiting, producerDone, j, buffers, bufferLocks[j], autoBuffered).RunAsynchronously(taskScheduler);
				}
			});
			groupState.QueryBegin(task);
			task.Start(taskScheduler);
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00029698 File Offset: 0x00027898
		protected override void SpoolingFinally()
		{
			object bufferLock = this._bufferLock;
			lock (bufferLock)
			{
				this._producerDone[this._partitionIndex] = true;
				if (this._consumerWaiting[this._partitionIndex])
				{
					Monitor.Pulse(this._bufferLock);
					this._consumerWaiting[this._partitionIndex] = false;
				}
			}
			base.SpoolingFinally();
			this._partition.Dispose();
		}

		// Token: 0x0400083C RID: 2108
		private readonly QueryTaskGroupState _taskGroupState;

		// Token: 0x0400083D RID: 2109
		private readonly QueryOperatorEnumerator<TOutput, TKey> _partition;

		// Token: 0x0400083E RID: 2110
		private readonly bool[] _consumerWaiting;

		// Token: 0x0400083F RID: 2111
		private readonly bool[] _producerWaiting;

		// Token: 0x04000840 RID: 2112
		private readonly bool[] _producerDone;

		// Token: 0x04000841 RID: 2113
		private readonly int _partitionIndex;

		// Token: 0x04000842 RID: 2114
		private readonly Queue<Pair<TKey, TOutput>>[] _buffers;

		// Token: 0x04000843 RID: 2115
		private readonly object _bufferLock;

		// Token: 0x04000844 RID: 2116
		private readonly bool _autoBuffered;

		// Token: 0x04000845 RID: 2117
		private const int PRODUCER_BUFFER_AUTO_SIZE = 16;
	}
}
