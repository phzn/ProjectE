using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x0200025B RID: 603
	public struct QueryEnumerable<T1, T2, T3, T4, T5> : IEnumerable<ValueTuple<T1, T2, T3, T4, T5>>, IEnumerable
	{
		// Token: 0x06001009 RID: 4105 RVA: 0x000461A8 File Offset: 0x000443A8
		public QueryEnumerable<T1, T2, T3, T4, T5> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1>() where TComponent1 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x000461BC File Offset: 0x000443BC
		public QueryEnumerable<T1, T2, T3, T4, T5> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x000461D0 File Offset: 0x000443D0
		public QueryEnumerable<T1, T2, T3, T4, T5> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent3>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent where TComponent3 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x000461E4 File Offset: 0x000443E4
		public QueryEnumerable<T1, T2, T3, T4, T5> WithAbsent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x000461F8 File Offset: 0x000443F8
		public QueryEnumerable<T1, T2, T3, T4, T5> WithAbsent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0004620C File Offset: 0x0004440C
		public QueryEnumerable<T1, T2, T3, T4, T5> WithAbsent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00046220 File Offset: 0x00044420
		public QueryEnumerable<T1, T2, T3, T4, T5> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00046234 File Offset: 0x00044434
		public QueryEnumerable<T1, T2, T3, T4, T5> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x00046248 File Offset: 0x00044448
		public QueryEnumerable<T1, T2, T3, T4, T5> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0004625C File Offset: 0x0004445C
		public QueryEnumerable<T1, T2, T3, T4, T5> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00046270 File Offset: 0x00044470
		public QueryEnumerable<T1, T2, T3, T4, T5> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00046284 File Offset: 0x00044484
		public QueryEnumerable<T1, T2, T3, T4, T5> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00046298 File Offset: 0x00044498
		public QueryEnumerable<T1, T2, T3, T4, T5> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x000462AC File Offset: 0x000444AC
		public QueryEnumerable<T1, T2, T3, T4, T5> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x000462C0 File Offset: 0x000444C0
		public QueryEnumerable<T1, T2, T3, T4, T5> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x000462D4 File Offset: 0x000444D4
		public QueryEnumerable<T1, T2, T3, T4, T5> WithPresent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x000462E8 File Offset: 0x000444E8
		public QueryEnumerable<T1, T2, T3, T4, T5> WithPresent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x000462FC File Offset: 0x000444FC
		public QueryEnumerable<T1, T2, T3, T4, T5> WithPresent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00046310 File Offset: 0x00044510
		public QueryEnumerable<T1, T2, T3, T4, T5> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00046324 File Offset: 0x00044524
		public QueryEnumerable<T1, T2, T3, T4, T5> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00046338 File Offset: 0x00044538
		public QueryEnumerable<T1, T2, T3, T4, T5> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0004634C File Offset: 0x0004454C
		public QueryEnumerable<T1, T2, T3, T4, T5> WithSharedComponentFilterManaged<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00046360 File Offset: 0x00044560
		public QueryEnumerable<T1, T2, T3, T4, T5> WithSharedComponentFilterManaged<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00046374 File Offset: 0x00044574
		public QueryEnumerable<T1, T2, T3, T4, T5> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00046388 File Offset: 0x00044588
		public QueryEnumerable<T1, T2, T3, T4, T5> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ValueType, ISharedComponentData where TSharedComponent2 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0004639C File Offset: 0x0004459C
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithEntityAccess()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2, T3, T4, T5>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x000326DA File Offset: 0x000308DA
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}
	}
}
