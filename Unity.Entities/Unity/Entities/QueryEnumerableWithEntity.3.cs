using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x02000258 RID: 600
	public readonly struct QueryEnumerableWithEntity<T1, T2, T3> : IEnumerable<ValueTuple<T1, T2, T3, Entity>>, IEnumerable
	{
		// Token: 0x06000FC9 RID: 4041 RVA: 0x00045CA6 File Offset: 0x00043EA6
		public QueryEnumerableWithEntity(T1 item1, T2 item2, T3 item3, Entity entity)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = entity;
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00045CC5 File Offset: 0x00043EC5
		public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out Entity entity)
		{
			item1 = this.Item1;
			item2 = this.Item2;
			item3 = this.Item3;
			entity = this.Item4;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00045CF8 File Offset: 0x00043EF8
		public QueryEnumerableWithEntity<T1, T2, T3> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00045D0C File Offset: 0x00043F0C
		public QueryEnumerableWithEntity<T1, T2, T3> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00045D20 File Offset: 0x00043F20
		public QueryEnumerableWithEntity<T1, T2, T3> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x00045D34 File Offset: 0x00043F34
		public QueryEnumerableWithEntity<T1, T2, T3> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00045D48 File Offset: 0x00043F48
		public QueryEnumerableWithEntity<T1, T2, T3> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00045D5C File Offset: 0x00043F5C
		public QueryEnumerableWithEntity<T1, T2, T3> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00045D70 File Offset: 0x00043F70
		public QueryEnumerableWithEntity<T1, T2, T3> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00045D84 File Offset: 0x00043F84
		public QueryEnumerableWithEntity<T1, T2, T3> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00045D98 File Offset: 0x00043F98
		public QueryEnumerableWithEntity<T1, T2, T3> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00045DAC File Offset: 0x00043FAC
		public QueryEnumerableWithEntity<T1, T2, T3> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00045DC0 File Offset: 0x00043FC0
		public QueryEnumerableWithEntity<T1, T2, T3> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00045DD4 File Offset: 0x00043FD4
		public QueryEnumerableWithEntity<T1, T2, T3> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00045DE8 File Offset: 0x00043FE8
		public QueryEnumerableWithEntity<T1, T2, T3> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00045DFC File Offset: 0x00043FFC
		public QueryEnumerableWithEntity<T1, T2, T3> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2, T3, Entity>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x00045E0E File Offset: 0x0004400E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400062A RID: 1578
		public readonly T1 Item1;

		// Token: 0x0400062B RID: 1579
		public readonly T2 Item2;

		// Token: 0x0400062C RID: 1580
		public readonly T3 Item3;

		// Token: 0x0400062D RID: 1581
		public readonly Entity Item4;
	}
}
