using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Parallel;
using Unity;

namespace System.Linq
{
	/// <summary>Represents a parallel sequence.</summary>
	/// <typeparam name="TSource">The type of element in the source sequence.</typeparam>
	// Token: 0x0200007F RID: 127
	public class ParallelQuery<TSource> : ParallelQuery, IEnumerable<TSource>, IEnumerable
	{
		// Token: 0x060002A1 RID: 673 RVA: 0x000080F2 File Offset: 0x000062F2
		internal ParallelQuery(QuerySettings settings)
			: base(settings)
		{
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000080FB File Offset: 0x000062FB
		internal sealed override ParallelQuery<TCastTo> Cast<TCastTo>()
		{
			return this.Select<TSource, TCastTo>((TSource elem) => (TCastTo)((object)elem));
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00008124 File Offset: 0x00006324
		internal sealed override ParallelQuery<TCastTo> OfType<TCastTo>()
		{
			return from elem in this
				where elem is TCastTo
				select (TCastTo)((object)elem);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000817A File Offset: 0x0000637A
		internal override IEnumerator GetEnumeratorUntyped()
		{
			return ((IEnumerable<TSource>)this).GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through the sequence.</summary>
		/// <returns>An enumerator that iterates through the sequence.</returns>
		// Token: 0x060002A5 RID: 677 RVA: 0x000080E3 File Offset: 0x000062E3
		public virtual IEnumerator<TSource> GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000235B File Offset: 0x0000055B
		internal ParallelQuery()
		{
			global::Unity.ThrowStub.ThrowNotSupportedException();
		}
	}
}
