using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	// Token: 0x02000134 RID: 308
	internal sealed class ZipQueryOperator<TLeftInput, TRightInput, TOutput> : QueryOperator<TOutput>
	{
		// Token: 0x06000947 RID: 2375 RVA: 0x0002126A File Offset: 0x0001F46A
		internal ZipQueryOperator(ParallelQuery<TLeftInput> leftChildSource, ParallelQuery<TRightInput> rightChildSource, Func<TLeftInput, TRightInput, TOutput> resultSelector)
			: this(QueryOperator<TLeftInput>.AsQueryOperator(leftChildSource), QueryOperator<TRightInput>.AsQueryOperator(rightChildSource), resultSelector)
		{
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00021280 File Offset: 0x0001F480
		private ZipQueryOperator(QueryOperator<TLeftInput> left, QueryOperator<TRightInput> right, Func<TLeftInput, TRightInput, TOutput> resultSelector)
			: base(left.SpecifiedQuerySettings.Merge(right.SpecifiedQuerySettings))
		{
			this._leftChild = left;
			this._rightChild = right;
			this._resultSelector = resultSelector;
			this._outputOrdered = this._leftChild.OutputOrdered || this._rightChild.OutputOrdered;
			OrdinalIndexState ordinalIndexState = this._leftChild.OrdinalIndexState;
			OrdinalIndexState ordinalIndexState2 = this._rightChild.OrdinalIndexState;
			this._prematureMergeLeft = ordinalIndexState > OrdinalIndexState.Indexable;
			this._prematureMergeRight = ordinalIndexState2 > OrdinalIndexState.Indexable;
			this._limitsParallelism = (this._prematureMergeLeft && ordinalIndexState != OrdinalIndexState.Shuffled) || (this._prematureMergeRight && ordinalIndexState2 != OrdinalIndexState.Shuffled);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00021330 File Offset: 0x0001F530
		internal override QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping)
		{
			QueryResults<TLeftInput> queryResults = this._leftChild.Open(settings, preferStriping);
			QueryResults<TRightInput> queryResults2 = this._rightChild.Open(settings, preferStriping);
			int value = settings.DegreeOfParallelism.Value;
			if (this._prematureMergeLeft)
			{
				PartitionedStreamMerger<TLeftInput> partitionedStreamMerger = new PartitionedStreamMerger<TLeftInput>(false, ParallelMergeOptions.FullyBuffered, settings.TaskScheduler, this._leftChild.OutputOrdered, settings.CancellationState, settings.QueryId);
				queryResults.GivePartitionedStream(partitionedStreamMerger);
				queryResults = new ListQueryResults<TLeftInput>(partitionedStreamMerger.MergeExecutor.GetResultsAsArray(), value, preferStriping);
			}
			if (this._prematureMergeRight)
			{
				PartitionedStreamMerger<TRightInput> partitionedStreamMerger2 = new PartitionedStreamMerger<TRightInput>(false, ParallelMergeOptions.FullyBuffered, settings.TaskScheduler, this._rightChild.OutputOrdered, settings.CancellationState, settings.QueryId);
				queryResults2.GivePartitionedStream(partitionedStreamMerger2);
				queryResults2 = new ListQueryResults<TRightInput>(partitionedStreamMerger2.MergeExecutor.GetResultsAsArray(), value, preferStriping);
			}
			return new ZipQueryOperator<TLeftInput, TRightInput, TOutput>.ZipQueryOperatorResults(queryResults, queryResults2, this._resultSelector, value, preferStriping);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00021412 File Offset: 0x0001F612
		internal override IEnumerable<TOutput> AsSequentialQuery(CancellationToken token)
		{
			using (IEnumerator<TLeftInput> leftEnumerator = this._leftChild.AsSequentialQuery(token).GetEnumerator())
			{
				using (IEnumerator<TRightInput> rightEnumerator = this._rightChild.AsSequentialQuery(token).GetEnumerator())
				{
					while (leftEnumerator.MoveNext() && rightEnumerator.MoveNext())
					{
						yield return this._resultSelector(leftEnumerator.Current, rightEnumerator.Current);
					}
				}
				IEnumerator<TRightInput> rightEnumerator = null;
			}
			IEnumerator<TLeftInput> leftEnumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x000023D1 File Offset: 0x000005D1
		internal override OrdinalIndexState OrdinalIndexState
		{
			get
			{
				return OrdinalIndexState.Indexable;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x00021429 File Offset: 0x0001F629
		internal override bool LimitsParallelism
		{
			get
			{
				return this._limitsParallelism;
			}
		}

		// Token: 0x040006CA RID: 1738
		private readonly Func<TLeftInput, TRightInput, TOutput> _resultSelector;

		// Token: 0x040006CB RID: 1739
		private readonly QueryOperator<TLeftInput> _leftChild;

		// Token: 0x040006CC RID: 1740
		private readonly QueryOperator<TRightInput> _rightChild;

		// Token: 0x040006CD RID: 1741
		private readonly bool _prematureMergeLeft;

		// Token: 0x040006CE RID: 1742
		private readonly bool _prematureMergeRight;

		// Token: 0x040006CF RID: 1743
		private readonly bool _limitsParallelism;

		// Token: 0x02000135 RID: 309
		internal class ZipQueryOperatorResults : QueryResults<TOutput>
		{
			// Token: 0x0600094D RID: 2381 RVA: 0x00021434 File Offset: 0x0001F634
			internal ZipQueryOperatorResults(QueryResults<TLeftInput> leftChildResults, QueryResults<TRightInput> rightChildResults, Func<TLeftInput, TRightInput, TOutput> resultSelector, int partitionCount, bool preferStriping)
			{
				this._leftChildResults = leftChildResults;
				this._rightChildResults = rightChildResults;
				this._resultSelector = resultSelector;
				this._partitionCount = partitionCount;
				this._preferStriping = preferStriping;
				this._count = Math.Min(this._leftChildResults.Count, this._rightChildResults.Count);
			}

			// Token: 0x1700010E RID: 270
			// (get) Token: 0x0600094E RID: 2382 RVA: 0x0002148D File Offset: 0x0001F68D
			internal override int ElementsCount
			{
				get
				{
					return this._count;
				}
			}

			// Token: 0x1700010F RID: 271
			// (get) Token: 0x0600094F RID: 2383 RVA: 0x00007E1D File Offset: 0x0000601D
			internal override bool IsIndexible
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06000950 RID: 2384 RVA: 0x00021495 File Offset: 0x0001F695
			internal override TOutput GetElement(int index)
			{
				return this._resultSelector(this._leftChildResults.GetElement(index), this._rightChildResults.GetElement(index));
			}

			// Token: 0x06000951 RID: 2385 RVA: 0x000214BC File Offset: 0x0001F6BC
			internal override void GivePartitionedStream(IPartitionedStreamRecipient<TOutput> recipient)
			{
				PartitionedStream<TOutput, int> partitionedStream = ExchangeUtilities.PartitionDataSource<TOutput>(this, this._partitionCount, this._preferStriping);
				recipient.Receive<int>(partitionedStream);
			}

			// Token: 0x040006D0 RID: 1744
			private readonly QueryResults<TLeftInput> _leftChildResults;

			// Token: 0x040006D1 RID: 1745
			private readonly QueryResults<TRightInput> _rightChildResults;

			// Token: 0x040006D2 RID: 1746
			private readonly Func<TLeftInput, TRightInput, TOutput> _resultSelector;

			// Token: 0x040006D3 RID: 1747
			private readonly int _count;

			// Token: 0x040006D4 RID: 1748
			private readonly int _partitionCount;

			// Token: 0x040006D5 RID: 1749
			private readonly bool _preferStriping;
		}
	}
}
