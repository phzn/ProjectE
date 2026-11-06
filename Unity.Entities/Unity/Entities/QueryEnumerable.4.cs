using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x02000259 RID: 601
	public struct QueryEnumerable<T1, T2, T3, T4> : IEnumerable<ValueTuple<T1, T2, T3, T4>>, IEnumerable
	{
		// Token: 0x06000FDB RID: 4059 RVA: 0x00045E18 File Offset: 0x00044018
		public QueryEnumerable<T1, T2, T3, T4> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1>() where TComponent1 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00045E2C File Offset: 0x0004402C
		public QueryEnumerable<T1, T2, T3, T4> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00045E40 File Offset: 0x00044040
		public QueryEnumerable<T1, T2, T3, T4> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent3>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent where TComponent3 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00045E54 File Offset: 0x00044054
		public QueryEnumerable<T1, T2, T3, T4> WithAbsent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00045E68 File Offset: 0x00044068
		public QueryEnumerable<T1, T2, T3, T4> WithAbsent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00045E7C File Offset: 0x0004407C
		public QueryEnumerable<T1, T2, T3, T4> WithAbsent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00045E90 File Offset: 0x00044090
		public QueryEnumerable<T1, T2, T3, T4> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00045EA4 File Offset: 0x000440A4
		public QueryEnumerable<T1, T2, T3, T4> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x00045EB8 File Offset: 0x000440B8
		public QueryEnumerable<T1, T2, T3, T4> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x00045ECC File Offset: 0x000440CC
		public QueryEnumerable<T1, T2, T3, T4> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x00045EE0 File Offset: 0x000440E0
		public QueryEnumerable<T1, T2, T3, T4> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x00045EF4 File Offset: 0x000440F4
		public QueryEnumerable<T1, T2, T3, T4> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00045F08 File Offset: 0x00044108
		public QueryEnumerable<T1, T2, T3, T4> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x00045F1C File Offset: 0x0004411C
		public QueryEnumerable<T1, T2, T3, T4> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00045F30 File Offset: 0x00044130
		public QueryEnumerable<T1, T2, T3, T4> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00045F44 File Offset: 0x00044144
		public QueryEnumerable<T1, T2, T3, T4> WithPresent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00045F58 File Offset: 0x00044158
		public QueryEnumerable<T1, T2, T3, T4> WithPresent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00045F6C File Offset: 0x0004416C
		public QueryEnumerable<T1, T2, T3, T4> WithPresent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00045F80 File Offset: 0x00044180
		public QueryEnumerable<T1, T2, T3, T4> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00045F94 File Offset: 0x00044194
		public QueryEnumerable<T1, T2, T3, T4> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00045FA8 File Offset: 0x000441A8
		public QueryEnumerable<T1, T2, T3, T4> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00045FBC File Offset: 0x000441BC
		public QueryEnumerable<T1, T2, T3, T4> WithSharedComponentFilterManaged<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00045FD0 File Offset: 0x000441D0
		public QueryEnumerable<T1, T2, T3, T4> WithSharedComponentFilterManaged<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00045FE4 File Offset: 0x000441E4
		public QueryEnumerable<T1, T2, T3, T4> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00045FF8 File Offset: 0x000441F8
		public QueryEnumerable<T1, T2, T3, T4> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ValueType, ISharedComponentData where TSharedComponent2 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0004600C File Offset: 0x0004420C
		public QueryEnumerableWithEntity<T1, T2, T3, T4> WithEntityAccess()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2, T3, T4>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x000326DA File Offset: 0x000308DA
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}
	}
}
