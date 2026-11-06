using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x02000255 RID: 597
	public struct QueryEnumerable<T1, T2> : IEnumerable<ValueTuple<T1, T2>>, IEnumerable
	{
		// Token: 0x06000F7F RID: 3967 RVA: 0x0004573C File Offset: 0x0004393C
		public QueryEnumerable<T1, T2> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1>() where TComponent1 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00045750 File Offset: 0x00043950
		public QueryEnumerable<T1, T2> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00045764 File Offset: 0x00043964
		public QueryEnumerable<T1, T2> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent3>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent where TComponent3 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x00045778 File Offset: 0x00043978
		public QueryEnumerable<T1, T2> WithAbsent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0004578C File Offset: 0x0004398C
		public QueryEnumerable<T1, T2> WithAbsent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x000457A0 File Offset: 0x000439A0
		public QueryEnumerable<T1, T2> WithAbsent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x000457B4 File Offset: 0x000439B4
		public QueryEnumerable<T1, T2> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x000457C8 File Offset: 0x000439C8
		public QueryEnumerable<T1, T2> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x000457DC File Offset: 0x000439DC
		public QueryEnumerable<T1, T2> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x000457F0 File Offset: 0x000439F0
		public QueryEnumerable<T1, T2> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00045804 File Offset: 0x00043A04
		public QueryEnumerable<T1, T2> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x00045818 File Offset: 0x00043A18
		public QueryEnumerable<T1, T2> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0004582C File Offset: 0x00043A2C
		public QueryEnumerable<T1, T2> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x00045840 File Offset: 0x00043A40
		public QueryEnumerable<T1, T2> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00045854 File Offset: 0x00043A54
		public QueryEnumerable<T1, T2> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x00045868 File Offset: 0x00043A68
		public QueryEnumerable<T1, T2> WithPresent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0004587C File Offset: 0x00043A7C
		public QueryEnumerable<T1, T2> WithPresent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00045890 File Offset: 0x00043A90
		public QueryEnumerable<T1, T2> WithPresent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x000458A4 File Offset: 0x00043AA4
		public QueryEnumerable<T1, T2> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x000458B8 File Offset: 0x00043AB8
		public QueryEnumerable<T1, T2> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x000458CC File Offset: 0x00043ACC
		public QueryEnumerable<T1, T2> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x000458E0 File Offset: 0x00043AE0
		public QueryEnumerable<T1, T2> WithSharedComponentFilterManaged<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x000458F4 File Offset: 0x00043AF4
		public QueryEnumerable<T1, T2> WithSharedComponentFilterManaged<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00045908 File Offset: 0x00043B08
		public QueryEnumerable<T1, T2> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0004591C File Offset: 0x00043B1C
		public QueryEnumerable<T1, T2> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ValueType, ISharedComponentData where TSharedComponent2 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00045930 File Offset: 0x00043B30
		public QueryEnumerableWithEntity<T1, T2> WithEntityAccess()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x000326DA File Offset: 0x000308DA
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}
	}
}
