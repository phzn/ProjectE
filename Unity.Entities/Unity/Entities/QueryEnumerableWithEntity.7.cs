using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x02000260 RID: 608
	public readonly struct QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> : IEnumerable<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<Entity>>>, IEnumerable
	{
		// Token: 0x06001081 RID: 4225 RVA: 0x00046B26 File Offset: 0x00044D26
		public QueryEnumerableWithEntity(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, Entity entity)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
			this.Item7 = item7;
			this.Item8 = entity;
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00046B68 File Offset: 0x00044D68
		public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out Entity entity)
		{
			item1 = this.Item1;
			item2 = this.Item2;
			item3 = this.Item3;
			item4 = this.Item4;
			item5 = this.Item5;
			item6 = this.Item6;
			item7 = this.Item7;
			entity = this.Item8;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00046BDC File Offset: 0x00044DDC
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00046BF0 File Offset: 0x00044DF0
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00046C04 File Offset: 0x00044E04
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00046C18 File Offset: 0x00044E18
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00046C2C File Offset: 0x00044E2C
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00046C40 File Offset: 0x00044E40
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00046C54 File Offset: 0x00044E54
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00046C68 File Offset: 0x00044E68
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00046C7C File Offset: 0x00044E7C
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00046C90 File Offset: 0x00044E90
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00046CA4 File Offset: 0x00044EA4
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00046CB8 File Offset: 0x00044EB8
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00046CCC File Offset: 0x00044ECC
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00046CE0 File Offset: 0x00044EE0
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2, T3, T4, T5, T6, T7, ValueTuple<Entity>>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00046CF2 File Offset: 0x00044EF2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000640 RID: 1600
		public readonly T1 Item1;

		// Token: 0x04000641 RID: 1601
		public readonly T2 Item2;

		// Token: 0x04000642 RID: 1602
		public readonly T3 Item3;

		// Token: 0x04000643 RID: 1603
		public readonly T4 Item4;

		// Token: 0x04000644 RID: 1604
		public readonly T5 Item5;

		// Token: 0x04000645 RID: 1605
		public readonly T6 Item6;

		// Token: 0x04000646 RID: 1606
		public readonly T7 Item7;

		// Token: 0x04000647 RID: 1607
		public readonly Entity Item8;
	}
}
