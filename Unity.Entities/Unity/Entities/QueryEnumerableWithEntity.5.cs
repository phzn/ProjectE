using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x0200025C RID: 604
	public readonly struct QueryEnumerableWithEntity<T1, T2, T3, T4, T5> : IEnumerable<ValueTuple<T1, T2, T3, T4, T5, Entity>>, IEnumerable
	{
		// Token: 0x06001025 RID: 4133 RVA: 0x000463AE File Offset: 0x000445AE
		public QueryEnumerableWithEntity(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, Entity entity)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = entity;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x000463E0 File Offset: 0x000445E0
		public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out Entity entity)
		{
			item1 = this.Item1;
			item2 = this.Item2;
			item3 = this.Item3;
			item4 = this.Item4;
			item5 = this.Item5;
			entity = this.Item6;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00046438 File Offset: 0x00044638
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0004644C File Offset: 0x0004464C
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00046460 File Offset: 0x00044660
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00046474 File Offset: 0x00044674
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00046488 File Offset: 0x00044688
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0004649C File Offset: 0x0004469C
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x000464B0 File Offset: 0x000446B0
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x000464C4 File Offset: 0x000446C4
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x000464D8 File Offset: 0x000446D8
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x000464EC File Offset: 0x000446EC
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x00046500 File Offset: 0x00044700
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00046514 File Offset: 0x00044714
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x00046528 File Offset: 0x00044728
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0004653C File Offset: 0x0004473C
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2, T3, T4, T5, Entity>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0004654E File Offset: 0x0004474E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000633 RID: 1587
		public readonly T1 Item1;

		// Token: 0x04000634 RID: 1588
		public readonly T2 Item2;

		// Token: 0x04000635 RID: 1589
		public readonly T3 Item3;

		// Token: 0x04000636 RID: 1590
		public readonly T4 Item4;

		// Token: 0x04000637 RID: 1591
		public readonly T5 Item5;

		// Token: 0x04000638 RID: 1592
		public readonly Entity Item6;
	}
}
