using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x02000253 RID: 595
	public struct QueryEnumerable<T1> : IEnumerable<T1>, IEnumerable
	{
		// Token: 0x06000F51 RID: 3921 RVA: 0x000453EC File Offset: 0x000435EC
		public QueryEnumerable<T1> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1>() where TComponent1 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00045400 File Offset: 0x00043600
		public QueryEnumerable<T1> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00045414 File Offset: 0x00043614
		public QueryEnumerable<T1> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent3>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent where TComponent3 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x00045428 File Offset: 0x00043628
		public QueryEnumerable<T1> WithAbsent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0004543C File Offset: 0x0004363C
		public QueryEnumerable<T1> WithAbsent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00045450 File Offset: 0x00043650
		public QueryEnumerable<T1> WithAbsent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00045464 File Offset: 0x00043664
		public QueryEnumerable<T1> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00045478 File Offset: 0x00043678
		public QueryEnumerable<T1> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0004548C File Offset: 0x0004368C
		public QueryEnumerable<T1> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x000454A0 File Offset: 0x000436A0
		public QueryEnumerable<T1> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x000454B4 File Offset: 0x000436B4
		public QueryEnumerable<T1> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x000454C8 File Offset: 0x000436C8
		public QueryEnumerable<T1> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x000454DC File Offset: 0x000436DC
		public QueryEnumerable<T1> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x000454F0 File Offset: 0x000436F0
		public QueryEnumerable<T1> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00045504 File Offset: 0x00043704
		public QueryEnumerable<T1> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00045518 File Offset: 0x00043718
		public QueryEnumerable<T1> WithPresent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0004552C File Offset: 0x0004372C
		public QueryEnumerable<T1> WithPresent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00045540 File Offset: 0x00043740
		public QueryEnumerable<T1> WithPresent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00045554 File Offset: 0x00043754
		public QueryEnumerable<T1> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x00045568 File Offset: 0x00043768
		public QueryEnumerable<T1> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0004557C File Offset: 0x0004377C
		public QueryEnumerable<T1> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00045590 File Offset: 0x00043790
		public QueryEnumerable<T1> WithSharedComponentFilterManaged<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x000455A4 File Offset: 0x000437A4
		public QueryEnumerable<T1> WithSharedComponentFilterManaged<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x000455B8 File Offset: 0x000437B8
		public QueryEnumerable<T1> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x000455CC File Offset: 0x000437CC
		public QueryEnumerable<T1> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ValueType, ISharedComponentData where TSharedComponent2 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x000455E0 File Offset: 0x000437E0
		public QueryEnumerableWithEntity<T1> WithEntityAccess()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<T1> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x000326DA File Offset: 0x000308DA
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}
	}
}
