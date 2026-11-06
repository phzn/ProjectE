using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x0200025D RID: 605
	public struct QueryEnumerable<T1, T2, T3, T4, T5, T6> : IEnumerable<ValueTuple<T1, T2, T3, T4, T5, T6>>, IEnumerable
	{
		// Token: 0x06001037 RID: 4151 RVA: 0x00046558 File Offset: 0x00044758
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1>() where TComponent1 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0004656C File Offset: 0x0004476C
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00046580 File Offset: 0x00044780
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent3>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent where TComponent3 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x00046594 File Offset: 0x00044794
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAbsent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x000465A8 File Offset: 0x000447A8
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAbsent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x000465BC File Offset: 0x000447BC
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAbsent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x000465D0 File Offset: 0x000447D0
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x000465E4 File Offset: 0x000447E4
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x000465F8 File Offset: 0x000447F8
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0004660C File Offset: 0x0004480C
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00046620 File Offset: 0x00044820
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00046634 File Offset: 0x00044834
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00046648 File Offset: 0x00044848
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0004665C File Offset: 0x0004485C
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00046670 File Offset: 0x00044870
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x00046684 File Offset: 0x00044884
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithPresent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00046698 File Offset: 0x00044898
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithPresent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x000466AC File Offset: 0x000448AC
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithPresent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x000466C0 File Offset: 0x000448C0
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x000466D4 File Offset: 0x000448D4
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x000466E8 File Offset: 0x000448E8
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x000466FC File Offset: 0x000448FC
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithSharedComponentFilterManaged<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00046710 File Offset: 0x00044910
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithSharedComponentFilterManaged<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00046724 File Offset: 0x00044924
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00046738 File Offset: 0x00044938
		public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ValueType, ISharedComponentData where TSharedComponent2 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0004674C File Offset: 0x0004494C
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithEntityAccess()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2, T3, T4, T5, T6>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x000326DA File Offset: 0x000308DA
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}
	}
}
