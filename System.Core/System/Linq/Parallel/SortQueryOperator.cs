using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
	// Token: 0x020001C3 RID: 451
	internal sealed class SortQueryOperator<TInputOutput, TSortKey> : UnaryQueryOperator<TInputOutput, TInputOutput>, IOrderedEnumerable<TInputOutput>, IEnumerable<TInputOutput>, IEnumerable
	{
		// Token: 0x06000B77 RID: 2935 RVA: 0x000282C4 File Offset: 0x000264C4
		internal SortQueryOperator(IEnumerable<TInputOutput> source, Func<TInputOutput, TSortKey> keySelector, IComparer<TSortKey> comparer, bool descending)
			: base(source, true)
		{
			this._keySelector = keySelector;
			if (comparer == null)
			{
				this._comparer = Util.GetDefaultComparer<TSortKey>();
			}
			else
			{
				this._comparer = comparer;
			}
			if (descending)
			{
				this._comparer = new ReverseComparer<TSortKey>(this._comparer);
			}
			base.SetOrdinalIndexState(OrdinalIndexState.Shuffled);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00028314 File Offset: 0x00026514
		IOrderedEnumerable<TInputOutput> IOrderedEnumerable<TInputOutput>.CreateOrderedEnumerable<TKey2>(Func<TInputOutput, TKey2> key2Selector, IComparer<TKey2> key2Comparer, bool descending)
		{
			key2Comparer = key2Comparer ?? Util.GetDefaultComparer<TKey2>();
			if (descending)
			{
				key2Comparer = new ReverseComparer<TKey2>(key2Comparer);
			}
			IComparer<Pair<TSortKey, TKey2>> comparer = new PairComparer<TSortKey, TKey2>(this._comparer, key2Comparer);
			Func<TInputOutput, Pair<TSortKey, TKey2>> func = (TInputOutput elem) => new Pair<TSortKey, TKey2>(this._keySelector(elem), key2Selector(elem));
			return new SortQueryOperator<TInputOutput, Pair<TSortKey, TKey2>>(base.Child, func, comparer, false);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00028372 File Offset: 0x00026572
		internal override QueryResults<TInputOutput> Open(QuerySettings settings, bool preferStriping)
		{
			return new SortQueryOperatorResults<TInputOutput, TSortKey>(base.Child.Open(settings, false), this, settings);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00028388 File Offset: 0x00026588
		internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInputOutput, TKey> inputStream, IPartitionedStreamRecipient<TInputOutput> recipient, bool preferStriping, QuerySettings settings)
		{
			PartitionedStream<TInputOutput, TSortKey> partitionedStream = new PartitionedStream<TInputOutput, TSortKey>(inputStream.PartitionCount, this._comparer, this.OrdinalIndexState);
			for (int i = 0; i < partitionedStream.PartitionCount; i++)
			{
				partitionedStream[i] = new SortQueryOperatorEnumerator<TInputOutput, TKey, TSortKey>(inputStream[i], this._keySelector);
			}
			recipient.Receive<TSortKey>(partitionedStream);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x000283DE File Offset: 0x000265DE
		internal override IEnumerable<TInputOutput> AsSequentialQuery(CancellationToken token)
		{
			return CancellableEnumerable.Wrap<TInputOutput>(base.Child.AsSequentialQuery(token), token).OrderBy<TInputOutput, TSortKey>(this._keySelector, this._comparer);
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x000023D1 File Offset: 0x000005D1
		internal override bool LimitsParallelism
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040007FB RID: 2043
		private readonly Func<TInputOutput, TSortKey> _keySelector;

		// Token: 0x040007FC RID: 2044
		private readonly IComparer<TSortKey> _comparer;
	}
}
