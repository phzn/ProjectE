using System;
using System.Collections.Generic;

namespace System.Collections.Concurrent
{
	/// <summary>Represents a particular manner of splitting an orderable data source into multiple partitions.</summary>
	/// <typeparam name="TSource">Type of the elements in the collection.</typeparam>
	// Token: 0x02000A63 RID: 2659
	public abstract class OrderablePartitioner<TSource> : Partitioner<TSource>
	{
		/// <summary>Called from constructors in derived classes to initialize the <see cref="T:System.Collections.Concurrent.OrderablePartitioner`1" /> class with the specified constraints on the index keys.</summary>
		/// <param name="keysOrderedInEachPartition">Indicates whether the elements in each partition are yielded in the order of increasing keys.</param>
		/// <param name="keysOrderedAcrossPartitions">Indicates whether elements in an earlier partition always come before elements in a later partition. If true, each element in partition 0 has a smaller order key than any element in partition 1, each element in partition 1 has a smaller order key than any element in partition 2, and so on.</param>
		/// <param name="keysNormalized">Indicates whether keys are normalized. If true, all order keys are distinct integers in the range [0 .. numberOfElements-1]. If false, order keys must still be distinct, but only their relative order is considered, not their absolute values.</param>
		// Token: 0x06005F7B RID: 24443 RVA: 0x00140FCC File Offset: 0x0013F1CC
		protected OrderablePartitioner(bool keysOrderedInEachPartition, bool keysOrderedAcrossPartitions, bool keysNormalized)
		{
			this.KeysOrderedInEachPartition = keysOrderedInEachPartition;
			this.KeysOrderedAcrossPartitions = keysOrderedAcrossPartitions;
			this.KeysNormalized = keysNormalized;
		}

		/// <summary>Partitions the underlying collection into the specified number of orderable partitions.</summary>
		/// <param name="partitionCount">The number of partitions to create.</param>
		/// <returns>A list containing <paramref name="partitionCount" /> enumerators.</returns>
		// Token: 0x06005F7C RID: 24444
		public abstract IList<IEnumerator<KeyValuePair<long, TSource>>> GetOrderablePartitions(int partitionCount);

		/// <summary>Creates an object that can partition the underlying collection into a variable number of partitions.</summary>
		/// <returns>An object that can create partitions over the underlying data source.</returns>
		/// <exception cref="T:System.NotSupportedException">Dynamic partitioning is not supported by this partitioner.</exception>
		// Token: 0x06005F7D RID: 24445 RVA: 0x00140FE9 File Offset: 0x0013F1E9
		public virtual IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions()
		{
			throw new NotSupportedException("Dynamic partitions are not supported by this partitioner.");
		}

		/// <summary>Gets whether elements in each partition are yielded in the order of increasing keys.</summary>
		/// <returns>
		///   <see langword="true" /> if the elements in each partition are yielded in the order of increasing keys; otherwise, <see langword="false" />.</returns>
		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x06005F7E RID: 24446 RVA: 0x00140FF5 File Offset: 0x0013F1F5
		// (set) Token: 0x06005F7F RID: 24447 RVA: 0x00140FFD File Offset: 0x0013F1FD
		public bool KeysOrderedInEachPartition { get; private set; }

		/// <summary>Gets whether elements in an earlier partition always come before elements in a later partition.</summary>
		/// <returns>
		///   <see langword="true" /> if the elements in an earlier partition always come before elements in a later partition; otherwise, <see langword="false" />.</returns>
		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x06005F80 RID: 24448 RVA: 0x00141006 File Offset: 0x0013F206
		// (set) Token: 0x06005F81 RID: 24449 RVA: 0x0014100E File Offset: 0x0013F20E
		public bool KeysOrderedAcrossPartitions { get; private set; }

		/// <summary>Gets whether order keys are normalized.</summary>
		/// <returns>
		///   <see langword="true" /> if the keys are normalized; otherwise, <see langword="false" />.</returns>
		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x06005F82 RID: 24450 RVA: 0x00141017 File Offset: 0x0013F217
		// (set) Token: 0x06005F83 RID: 24451 RVA: 0x0014101F File Offset: 0x0013F21F
		public bool KeysNormalized { get; private set; }

		/// <summary>Partitions the underlying collection into the given number of ordered partitions.</summary>
		/// <param name="partitionCount">The number of partitions to create.</param>
		/// <returns>A list containing <paramref name="partitionCount" /> enumerators.</returns>
		// Token: 0x06005F84 RID: 24452 RVA: 0x00141028 File Offset: 0x0013F228
		public override IList<IEnumerator<TSource>> GetPartitions(int partitionCount)
		{
			IList<IEnumerator<KeyValuePair<long, TSource>>> orderablePartitions = this.GetOrderablePartitions(partitionCount);
			if (orderablePartitions.Count != partitionCount)
			{
				throw new InvalidOperationException("GetPartitions returned an incorrect number of partitions.");
			}
			IEnumerator<TSource>[] array = new IEnumerator<TSource>[partitionCount];
			for (int i = 0; i < partitionCount; i++)
			{
				array[i] = new OrderablePartitioner<TSource>.EnumeratorDropIndices(orderablePartitions[i]);
			}
			return array;
		}

		/// <summary>Creates an object that can partition the underlying collection into a variable number of partitions.</summary>
		/// <returns>An object that can create partitions over the underlying data source.</returns>
		/// <exception cref="T:System.NotSupportedException">Dynamic partitioning is not supported by the base class. It must be implemented in derived classes.</exception>
		// Token: 0x06005F85 RID: 24453 RVA: 0x00141074 File Offset: 0x0013F274
		public override IEnumerable<TSource> GetDynamicPartitions()
		{
			return new OrderablePartitioner<TSource>.EnumerableDropIndices(this.GetOrderableDynamicPartitions());
		}

		// Token: 0x02000A64 RID: 2660
		private class EnumerableDropIndices : IEnumerable<TSource>, IEnumerable, IDisposable
		{
			// Token: 0x06005F86 RID: 24454 RVA: 0x00141081 File Offset: 0x0013F281
			public EnumerableDropIndices(IEnumerable<KeyValuePair<long, TSource>> source)
			{
				this._source = source;
			}

			// Token: 0x06005F87 RID: 24455 RVA: 0x00141090 File Offset: 0x0013F290
			public IEnumerator<TSource> GetEnumerator()
			{
				return new OrderablePartitioner<TSource>.EnumeratorDropIndices(this._source.GetEnumerator());
			}

			// Token: 0x06005F88 RID: 24456 RVA: 0x001410A2 File Offset: 0x0013F2A2
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06005F89 RID: 24457 RVA: 0x001410AC File Offset: 0x0013F2AC
			public void Dispose()
			{
				IDisposable disposable = this._source as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}

			// Token: 0x04003962 RID: 14690
			private readonly IEnumerable<KeyValuePair<long, TSource>> _source;
		}

		// Token: 0x02000A65 RID: 2661
		private class EnumeratorDropIndices : IEnumerator<TSource>, IDisposable, IEnumerator
		{
			// Token: 0x06005F8A RID: 24458 RVA: 0x001410CE File Offset: 0x0013F2CE
			public EnumeratorDropIndices(IEnumerator<KeyValuePair<long, TSource>> source)
			{
				this._source = source;
			}

			// Token: 0x06005F8B RID: 24459 RVA: 0x001410DD File Offset: 0x0013F2DD
			public bool MoveNext()
			{
				return this._source.MoveNext();
			}

			// Token: 0x170010C9 RID: 4297
			// (get) Token: 0x06005F8C RID: 24460 RVA: 0x001410EC File Offset: 0x0013F2EC
			public TSource Current
			{
				get
				{
					KeyValuePair<long, TSource> keyValuePair = this._source.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x170010CA RID: 4298
			// (get) Token: 0x06005F8D RID: 24461 RVA: 0x0014110C File Offset: 0x0013F30C
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06005F8E RID: 24462 RVA: 0x00141119 File Offset: 0x0013F319
			public void Dispose()
			{
				this._source.Dispose();
			}

			// Token: 0x06005F8F RID: 24463 RVA: 0x00141126 File Offset: 0x0013F326
			public void Reset()
			{
				this._source.Reset();
			}

			// Token: 0x04003963 RID: 14691
			private readonly IEnumerator<KeyValuePair<long, TSource>> _source;
		}
	}
}
