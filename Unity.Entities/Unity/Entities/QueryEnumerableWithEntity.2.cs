using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x02000256 RID: 598
	public readonly struct QueryEnumerableWithEntity<T1, T2> : IEnumerable<ValueTuple<T1, T2, Entity>>, IEnumerable
	{
		// Token: 0x06000F9B RID: 3995 RVA: 0x00045942 File Offset: 0x00043B42
		public QueryEnumerableWithEntity(T1 item1, T2 item2, Entity entity)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = entity;
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00045959 File Offset: 0x00043B59
		public void Deconstruct(out T1 item1, out T2 item2, out Entity entity)
		{
			item1 = this.Item1;
			item2 = this.Item2;
			entity = this.Item3;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00045980 File Offset: 0x00043B80
		public QueryEnumerableWithEntity<T1, T2> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00045994 File Offset: 0x00043B94
		public QueryEnumerableWithEntity<T1, T2> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x000459A8 File Offset: 0x00043BA8
		public QueryEnumerableWithEntity<T1, T2> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x000459BC File Offset: 0x00043BBC
		public QueryEnumerableWithEntity<T1, T2> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x000459D0 File Offset: 0x00043BD0
		public QueryEnumerableWithEntity<T1, T2> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x000459E4 File Offset: 0x00043BE4
		public QueryEnumerableWithEntity<T1, T2> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x000459F8 File Offset: 0x00043BF8
		public QueryEnumerableWithEntity<T1, T2> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00045A0C File Offset: 0x00043C0C
		public QueryEnumerableWithEntity<T1, T2> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00045A20 File Offset: 0x00043C20
		public QueryEnumerableWithEntity<T1, T2> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00045A34 File Offset: 0x00043C34
		public QueryEnumerableWithEntity<T1, T2> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00045A48 File Offset: 0x00043C48
		public QueryEnumerableWithEntity<T1, T2> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00045A5C File Offset: 0x00043C5C
		public QueryEnumerableWithEntity<T1, T2> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00045A70 File Offset: 0x00043C70
		public QueryEnumerableWithEntity<T1, T2> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00045A84 File Offset: 0x00043C84
		public QueryEnumerableWithEntity<T1, T2> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2, Entity>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00045A96 File Offset: 0x00043C96
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000627 RID: 1575
		public readonly T1 Item1;

		// Token: 0x04000628 RID: 1576
		public readonly T2 Item2;

		// Token: 0x04000629 RID: 1577
		public readonly Entity Item3;
	}
}
