using System;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
	// Token: 0x020001DD RID: 477
	internal static class SpoolingTask
	{
		// Token: 0x06000BDA RID: 3034 RVA: 0x00029BB8 File Offset: 0x00027DB8
		internal static void SpoolStopAndGo<TInputOutput, TIgnoreKey>(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TIgnoreKey> partitions, SynchronousChannel<TInputOutput>[] channels, TaskScheduler taskScheduler)
		{
			Task task = new Task(delegate
			{
				int num = partitions.PartitionCount - 1;
				for (int i = 0; i < num; i++)
				{
					new StopAndGoSpoolingTask<TInputOutput, TIgnoreKey>(i, groupState, partitions[i], channels[i]).RunAsynchronously(taskScheduler);
				}
				new StopAndGoSpoolingTask<TInputOutput, TIgnoreKey>(num, groupState, partitions[num], channels[num]).RunSynchronously(taskScheduler);
			});
			groupState.QueryBegin(task);
			task.RunSynchronously(taskScheduler);
			groupState.QueryEnd(false);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00029C20 File Offset: 0x00027E20
		internal static void SpoolPipeline<TInputOutput, TIgnoreKey>(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TIgnoreKey> partitions, AsynchronousChannel<TInputOutput>[] channels, TaskScheduler taskScheduler)
		{
			Task task = new Task(delegate
			{
				for (int i = 0; i < partitions.PartitionCount; i++)
				{
					new PipelineSpoolingTask<TInputOutput, TIgnoreKey>(i, groupState, partitions[i], channels[i]).RunAsynchronously(taskScheduler);
				}
			});
			groupState.QueryBegin(task);
			task.Start(taskScheduler);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00029C7C File Offset: 0x00027E7C
		internal static void SpoolForAll<TInputOutput, TIgnoreKey>(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TIgnoreKey> partitions, TaskScheduler taskScheduler)
		{
			Task task = new Task(delegate
			{
				int num = partitions.PartitionCount - 1;
				for (int i = 0; i < num; i++)
				{
					new ForAllSpoolingTask<TInputOutput, TIgnoreKey>(i, groupState, partitions[i]).RunAsynchronously(taskScheduler);
				}
				new ForAllSpoolingTask<TInputOutput, TIgnoreKey>(num, groupState, partitions[num]).RunSynchronously(taskScheduler);
			});
			groupState.QueryBegin(task);
			task.RunSynchronously(taskScheduler);
			groupState.QueryEnd(false);
		}
	}
}
