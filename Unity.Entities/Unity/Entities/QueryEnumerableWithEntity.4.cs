using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x0200025A RID: 602
	public readonly struct QueryEnumerableWithEntity<T1, T2, T3, T4> : IEnumerable<ValueTuple<T1, T2, T3, T4, Entity>>, IEnumerable
	{
		// Token: 0x06000FF7 RID: 4087 RVA: 0x0004601E File Offset: 0x0004421E
		public QueryEnumerableWithEntity(T1 item1, T2 item2, T3 item3, T4 item4, Entity entity)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = entity;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00046045 File Offset: 0x00044245
		public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out Entity entity)
		{
			item1 = this.Item1;
			item2 = this.Item2;
			item3 = this.Item3;
			item4 = this.Item4;
			entity = this.Item5;
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00046088 File Offset: 0x00044288
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0004609C File Offset: 0x0004429C
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x000460B0 File Offset: 0x000442B0
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x000460C4 File Offset: 0x000442C4
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x000460D8 File Offset: 0x000442D8
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x000460EC File Offset: 0x000442EC
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00046100 File Offset: 0x00044300
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00046114 File Offset: 0x00044314
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00046128 File Offset: 0x00044328
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0004613C File Offset: 0x0004433C
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00046150 File Offset: 0x00044350
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00046164 File Offset: 0x00044364
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00046178 File Offset: 0x00044378
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0004618C File Offset: 0x0004438C
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2, T3, T4, Entity>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0004619E File Offset: 0x0004439E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400062E RID: 1582
		public readonly T1 Item1;

		// Token: 0x0400062F RID: 1583
		public readonly T2 Item2;

		// Token: 0x04000630 RID: 1584
		public readonly T3 Item3;

		// Token: 0x04000631 RID: 1585
		public readonly T4 Item4;

		// Token: 0x04000632 RID: 1586
		public readonly Entity Item5;
	}
}
