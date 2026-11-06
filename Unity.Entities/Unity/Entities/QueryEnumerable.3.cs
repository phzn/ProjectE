using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x02000257 RID: 599
	public struct QueryEnumerable<T1, T2, T3> : IEnumerable<ValueTuple<T1, T2, T3>>, IEnumerable
	{
		// Token: 0x06000FAD RID: 4013 RVA: 0x00045AA0 File Offset: 0x00043CA0
		public QueryEnumerable<T1, T2, T3> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1>() where TComponent1 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x00045AB4 File Offset: 0x00043CB4
		public QueryEnumerable<T1, T2, T3> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00045AC8 File Offset: 0x00043CC8
		public QueryEnumerable<T1, T2, T3> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent3>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent where TComponent3 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00045ADC File Offset: 0x00043CDC
		public QueryEnumerable<T1, T2, T3> WithAbsent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00045AF0 File Offset: 0x00043CF0
		public QueryEnumerable<T1, T2, T3> WithAbsent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00045B04 File Offset: 0x00043D04
		public QueryEnumerable<T1, T2, T3> WithAbsent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00045B18 File Offset: 0x00043D18
		public QueryEnumerable<T1, T2, T3> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00045B2C File Offset: 0x00043D2C
		public QueryEnumerable<T1, T2, T3> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00045B40 File Offset: 0x00043D40
		public QueryEnumerable<T1, T2, T3> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00045B54 File Offset: 0x00043D54
		public QueryEnumerable<T1, T2, T3> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00045B68 File Offset: 0x00043D68
		public QueryEnumerable<T1, T2, T3> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00045B7C File Offset: 0x00043D7C
		public QueryEnumerable<T1, T2, T3> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00045B90 File Offset: 0x00043D90
		public QueryEnumerable<T1, T2, T3> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00045BA4 File Offset: 0x00043DA4
		public QueryEnumerable<T1, T2, T3> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00045BB8 File Offset: 0x00043DB8
		public QueryEnumerable<T1, T2, T3> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00045BCC File Offset: 0x00043DCC
		public QueryEnumerable<T1, T2, T3> WithPresent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00045BE0 File Offset: 0x00043DE0
		public QueryEnumerable<T1, T2, T3> WithPresent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00045BF4 File Offset: 0x00043DF4
		public QueryEnumerable<T1, T2, T3> WithPresent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00045C08 File Offset: 0x00043E08
		public QueryEnumerable<T1, T2, T3> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00045C1C File Offset: 0x00043E1C
		public QueryEnumerable<T1, T2, T3> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00045C30 File Offset: 0x00043E30
		public QueryEnumerable<T1, T2, T3> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00045C44 File Offset: 0x00043E44
		public QueryEnumerable<T1, T2, T3> WithSharedComponentFilterManaged<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00045C58 File Offset: 0x00043E58
		public QueryEnumerable<T1, T2, T3> WithSharedComponentFilterManaged<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00045C6C File Offset: 0x00043E6C
		public QueryEnumerable<T1, T2, T3> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00045C80 File Offset: 0x00043E80
		public QueryEnumerable<T1, T2, T3> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ValueType, ISharedComponentData where TSharedComponent2 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00045C94 File Offset: 0x00043E94
		public QueryEnumerableWithEntity<T1, T2, T3> WithEntityAccess()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2, T3>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x000326DA File Offset: 0x000308DA
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}
	}
}
