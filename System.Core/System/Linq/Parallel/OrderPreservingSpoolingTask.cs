using System;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
	// Token: 0x020001D6 RID: 470
	internal class OrderPreservingSpoolingTask<TInputOutput, TKey> : SpoolingTaskBase
	{
		// Token: 0x06000BC0 RID: 3008 RVA: 0x00029784 File Offset: 0x00027984
		private OrderPreservingSpoolingTask(int taskIndex, QueryTaskGroupState groupState, Shared<TInputOutput[]> results, SortHelper<TInputOutput> sortHelper)
			: base(taskIndex, groupState)
		{
			this._results = results;
			this._sortHelper = sortHelper;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x000297A0 File Offset: 0x000279A0
		internal static void Spool(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TKey> partitions, Shared<TInputOutput[]> results, TaskScheduler taskScheduler)
		{
			int maxToRunInParallel = partitions.PartitionCount - 1;
			SortHelper<TInputOutput, TKey>[] sortHelpers = SortHelper<TInputOutput, TKey>.GenerateSortHelpers(partitions, groupState);
			Task task = new Task(delegate
			{
				for (int j = 0; j < maxToRunInParallel; j++)
				{
					new OrderPreservingSpoolingTask<TInputOutput, TKey>(j, groupState, results, sortHelpers[j]).RunAsynchronously(taskScheduler);
				}
				new OrderPreservingSpoolingTask<TInputOutput, TKey>(maxToRunInParallel, groupState, results, sortHelpers[maxToRunInParallel]).RunSynchronously(taskScheduler);
			});
			groupState.QueryBegin(task);
			task.RunSynchronously(taskScheduler);
			for (int i = 0; i < sortHelpers.Length; i++)
			{
				sortHelpers[i].Dispose();
			}
			groupState.QueryEnd(false);
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00029840 File Offset: 0x00027A40
		protected override void SpoolingWork()
		{
			TInputOutput[] array = this._sortHelper.Sort();
			if (!this._groupState.CancellationState.MergedCancellationToken.IsCancellationRequested && this._taskIndex == 0)
			{
				this._results.Value = array;
			}
		}

		// Token: 0x04000850 RID: 2128
		private Shared<TInputOutput[]> _results;

		// Token: 0x04000851 RID: 2129
		private SortHelper<TInputOutput> _sortHelper;
	}
}
