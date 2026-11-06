using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x02000254 RID: 596
	public readonly struct QueryEnumerableWithEntity<T1> : IEnumerable<ValueTuple<T1, Entity>>, IEnumerable
	{
		// Token: 0x06000F6D RID: 3949 RVA: 0x000455F2 File Offset: 0x000437F2
		public QueryEnumerableWithEntity(T1 item1, Entity entity)
		{
			this.Item1 = item1;
			this.Item2 = entity;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00045602 File Offset: 0x00043802
		public void Deconstruct(out T1 item1, out Entity entity)
		{
			item1 = this.Item1;
			entity = this.Item2;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0004561C File Offset: 0x0004381C
		public QueryEnumerableWithEntity<T1> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00045630 File Offset: 0x00043830
		public QueryEnumerableWithEntity<T1> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00045644 File Offset: 0x00043844
		public QueryEnumerableWithEntity<T1> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00045658 File Offset: 0x00043858
		public QueryEnumerableWithEntity<T1> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0004566C File Offset: 0x0004386C
		public QueryEnumerableWithEntity<T1> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00045680 File Offset: 0x00043880
		public QueryEnumerableWithEntity<T1> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00045694 File Offset: 0x00043894
		public QueryEnumerableWithEntity<T1> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x000456A8 File Offset: 0x000438A8
		public QueryEnumerableWithEntity<T1> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x000456BC File Offset: 0x000438BC
		public QueryEnumerableWithEntity<T1> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x000456D0 File Offset: 0x000438D0
		public QueryEnumerableWithEntity<T1> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x000456E4 File Offset: 0x000438E4
		public QueryEnumerableWithEntity<T1> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x000456F8 File Offset: 0x000438F8
		public QueryEnumerableWithEntity<T1> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0004570C File Offset: 0x0004390C
		public QueryEnumerableWithEntity<T1> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00045720 File Offset: 0x00043920
		public QueryEnumerableWithEntity<T1> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, Entity>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00045732 File Offset: 0x00043932
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000625 RID: 1573
		public readonly T1 Item1;

		// Token: 0x04000626 RID: 1574
		public readonly Entity Item2;
	}
}
