using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities.Internal;

namespace Unity.Entities
{
	// Token: 0x0200025F RID: 607
	public struct QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> : IEnumerable<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>, IEnumerable
	{
		// Token: 0x06001065 RID: 4197 RVA: 0x00046920 File Offset: 0x00044B20
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1>() where TComponent1 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x00046934 File Offset: 0x00044B34
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00046948 File Offset: 0x00044B48
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithDisabled<[global::System.Runtime.CompilerServices.IsUnmanaged] TComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent2, [global::System.Runtime.CompilerServices.IsUnmanaged] TComponent3>() where TComponent1 : struct, ValueType, IEnableableComponent where TComponent2 : struct, ValueType, IEnableableComponent where TComponent3 : struct, ValueType, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0004695C File Offset: 0x00044B5C
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAbsent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x00046970 File Offset: 0x00044B70
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAbsent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00046984 File Offset: 0x00044B84
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAbsent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00046998 File Offset: 0x00044B98
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x000469AC File Offset: 0x00044BAC
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x000469C0 File Offset: 0x00044BC0
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x000469D4 File Offset: 0x00044BD4
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x000469E8 File Offset: 0x00044BE8
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x000469FC File Offset: 0x00044BFC
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00046A10 File Offset: 0x00044C10
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00046A24 File Offset: 0x00044C24
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00046A38 File Offset: 0x00044C38
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00046A4C File Offset: 0x00044C4C
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithPresent<TComponent1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00046A60 File Offset: 0x00044C60
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithPresent<TComponent1, TComponent2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00046A74 File Offset: 0x00044C74
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithPresent<TComponent1, TComponent2, TComponent3>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00046A88 File Offset: 0x00044C88
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithChangeFilter<TChangeFilter1>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x00046A9C File Offset: 0x00044C9C
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithChangeFilter<TChangeFilter1, TChangeFilter2>()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00046AB0 File Offset: 0x00044CB0
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithOptions(EntityQueryOptions options)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00046AC4 File Offset: 0x00044CC4
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithSharedComponentFilterManaged<TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00046AD8 File Offset: 0x00044CD8
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithSharedComponentFilterManaged<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ISharedComponentData where TSharedComponent2 : struct, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00046AEC File Offset: 0x00044CEC
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1>(TSharedComponent1 sharedComponent) where TSharedComponent1 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00046B00 File Offset: 0x00044D00
		public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2) where TSharedComponent1 : struct, ValueType, ISharedComponentData where TSharedComponent2 : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00046B14 File Offset: 0x00044D14
		public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithEntityAccess()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x000326DA File Offset: 0x000308DA
		public IEnumerator<ValueTuple<T1, T2, T3, T4, T5, T6, T7>> GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x000326DA File Offset: 0x000308DA
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}
	}
}
