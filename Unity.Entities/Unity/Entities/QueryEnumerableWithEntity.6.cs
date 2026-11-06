using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x0200025E RID: 606
	public readonly struct QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> : IEnumerable<ValueTuple<T1, T2, T3, T4, T5, T6, Entity>>, IEnumerable
	{
		// Token: 0x06001053 RID: 4179 RVA: 0x0004675E File Offset: 0x0004495E
		public QueryEnumerableWithEntity(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, Entity entity)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
			this.Item7 = entity;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00046798 File Offset: 0x00044998
		public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out Entity entity)
		{
			item1 = this.Item1;
			item2 = this.Item2;
			item3 = this.Item3;
			item4 = this.Item4;
			item5 = this.Item5;
			item6 = this.Item6;
			entity = this.Item7;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00046800 File Offset: 0x00044A00
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00046814 File Offset: 0x00044A14
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00046828 File Offset: 0x00044A28
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0004683C File Offset: 0x00044A3C
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00046850 File Offset: 0x00044A50
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00046864 File Offset: 0x00044A64
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00046878 File Offset: 0x00044A78
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0004688C File Offset: 0x00044A8C
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x000468A0 File Offset: 0x00044AA0
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x000468B4 File Offset: 0x00044AB4
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x000468C8 File Offset: 0x00044AC8
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x000468DC File Offset: 0x00044ADC
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x000468F0 File Offset: 0x00044AF0
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00046904 File Offset: 0x00044B04
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2, T3, T4, T5, T6, Entity>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x00046916 File Offset: 0x00044B16
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000639 RID: 1593
		public readonly T1 Item1;

		// Token: 0x0400063A RID: 1594
		public readonly T2 Item2;

		// Token: 0x0400063B RID: 1595
		public readonly T3 Item3;

		// Token: 0x0400063C RID: 1596
		public readonly T4 Item4;

		// Token: 0x0400063D RID: 1597
		public readonly T5 Item5;

		// Token: 0x0400063E RID: 1598
		public readonly T6 Item6;

		// Token: 0x0400063F RID: 1599
		public readonly Entity Item7;
	}
}
