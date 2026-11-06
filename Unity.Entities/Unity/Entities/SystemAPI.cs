using System;
using System.Runtime.CompilerServices;
using Unity.Core;
using Unity.Entities.Internal;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x020002C8 RID: 712
	public static class SystemAPI
	{
		// Token: 0x060012A7 RID: 4775 RVA: 0x0004ABD4 File Offset: 0x00048DD4
		public static SystemAPIQueryBuilder QueryBuilder()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0004ABE8 File Offset: 0x00048DE8
		public static QueryEnumerable<T1> Query<T1>() where T1 : IQueryTypeParameter
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0004ABFC File Offset: 0x00048DFC
		public static QueryEnumerable<T1, T2> Query<T1, T2>() where T1 : IQueryTypeParameter where T2 : IQueryTypeParameter
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0004AC10 File Offset: 0x00048E10
		public static QueryEnumerable<T1, T2, T3> Query<T1, T2, T3>() where T1 : IQueryTypeParameter where T2 : IQueryTypeParameter where T3 : IQueryTypeParameter
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0004AC24 File Offset: 0x00048E24
		public static QueryEnumerable<T1, T2, T3, T4> Query<T1, T2, T3, T4>() where T1 : IQueryTypeParameter where T2 : IQueryTypeParameter where T3 : IQueryTypeParameter where T4 : IQueryTypeParameter
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0004AC38 File Offset: 0x00048E38
		public static QueryEnumerable<T1, T2, T3, T4, T5> Query<T1, T2, T3, T4, T5>() where T1 : IQueryTypeParameter where T2 : IQueryTypeParameter where T3 : IQueryTypeParameter where T4 : IQueryTypeParameter where T5 : IQueryTypeParameter
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0004AC4C File Offset: 0x00048E4C
		public static QueryEnumerable<T1, T2, T3, T4, T5, T6> Query<T1, T2, T3, T4, T5, T6>() where T1 : IQueryTypeParameter where T2 : IQueryTypeParameter where T3 : IQueryTypeParameter where T4 : IQueryTypeParameter where T5 : IQueryTypeParameter where T6 : IQueryTypeParameter
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0004AC60 File Offset: 0x00048E60
		public static QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> Query<T1, T2, T3, T4, T5, T6, T7>() where T1 : IQueryTypeParameter where T2 : IQueryTypeParameter where T3 : IQueryTypeParameter where T4 : IQueryTypeParameter where T5 : IQueryTypeParameter where T6 : IQueryTypeParameter where T7 : IQueryTypeParameter
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060012AF RID: 4783 RVA: 0x0004AC74 File Offset: 0x00048E74
		public static readonly ref TimeData Time
		{
			get
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0004AC88 File Offset: 0x00048E88
		public static ComponentLookup<T> GetComponentLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0004AC9C File Offset: 0x00048E9C
		public static T GetComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0004ACB0 File Offset: 0x00048EB0
		public static RefRO<T> GetComponentRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0004ACC4 File Offset: 0x00048EC4
		public static RefRW<T> GetComponentRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x000326DA File Offset: 0x000308DA
		public static void SetComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T component) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool HasComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool IsComponentEnabled<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x000326DA File Offset: 0x000308DA
		public static void SetComponentEnabled<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, bool value) where T : struct, ValueType, IComponentData, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0004ACD8 File Offset: 0x00048ED8
		public static T GetComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle systemHandle) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0004ACEC File Offset: 0x00048EEC
		public static RefRW<T> GetComponentRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle systemHandle) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x000326DA File Offset: 0x000308DA
		public static void SetComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle systemHandle, T component) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool HasComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle systemHandle) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool IsComponentEnabled<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle systemHandle) where T : struct, ValueType, IComponentData, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x000326DA File Offset: 0x000308DA
		public static void SetComponentEnabled<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle systemHandle, bool value) where T : struct, ValueType, IComponentData, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0004AD00 File Offset: 0x00048F00
		public static BufferLookup<T> GetBufferLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0004AD14 File Offset: 0x00048F14
		public static DynamicBuffer<T> GetBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IBufferElementData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool HasBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IBufferElementData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool IsBufferEnabled<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x000326DA File Offset: 0x000308DA
		public static void SetBufferEnabled<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, bool value) where T : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0004AD28 File Offset: 0x00048F28
		public static EntityStorageInfoLookup GetEntityStorageInfoLookup()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool Exists(Entity entity)
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0004AD3C File Offset: 0x00048F3C
		public static T GetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool TryGetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out T value) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0004AD50 File Offset: 0x00048F50
		public static RefRW<T> GetSingletonRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool TryGetSingletonRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out RefRW<T> value) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x000326DA File Offset: 0x000308DA
		public static void SetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T value) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0004AD64 File Offset: 0x00048F64
		public static Entity GetSingletonEntity<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool TryGetSingletonEntity<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out Entity value) where T : struct, ValueType
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0004AD78 File Offset: 0x00048F78
		public static DynamicBuffer<T> GetSingletonBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool TryGetSingletonBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out DynamicBuffer<T> value, bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x000326DA File Offset: 0x000308DA
		public static bool HasSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0004AD8C File Offset: 0x00048F8C
		public static T GetAspect<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IAspect
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0004ADA0 File Offset: 0x00048FA0
		public static EntityTypeHandle GetEntityTypeHandle()
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0004ADB4 File Offset: 0x00048FB4
		public static ComponentTypeHandle<T> GetComponentTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0004ADC8 File Offset: 0x00048FC8
		public static BufferTypeHandle<T> GetBufferTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0004ADDC File Offset: 0x00048FDC
		public static SharedComponentTypeHandle<T> GetSharedComponentTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, ISharedComponentData
		{
			throw InternalCompilerInterface.ThrowCodeGenException();
		}

		// Token: 0x020002C9 RID: 713
		public static class ManagedAPI
		{
			// Token: 0x060012D4 RID: 4820 RVA: 0x0004ADF0 File Offset: 0x00048FF0
			public static T GetComponent<T>(Entity entity) where T : class
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012D5 RID: 4821 RVA: 0x000326DA File Offset: 0x000308DA
			public static bool HasComponent<T>(Entity entity) where T : class
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012D6 RID: 4822 RVA: 0x000326DA File Offset: 0x000308DA
			public static bool IsComponentEnabled<T>(Entity entity) where T : class, IEnableableComponent
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012D7 RID: 4823 RVA: 0x000326DA File Offset: 0x000308DA
			public static void SetComponentEnabled<T>(Entity entity, bool value) where T : class, IEnableableComponent
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012D8 RID: 4824 RVA: 0x0004AE04 File Offset: 0x00049004
			public static T GetComponent<T>(SystemHandle systemHandle) where T : class
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012D9 RID: 4825 RVA: 0x000326DA File Offset: 0x000308DA
			public static bool HasComponent<T>(SystemHandle systemHandle) where T : class
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012DA RID: 4826 RVA: 0x000326DA File Offset: 0x000308DA
			public static bool IsComponentEnabled<T>(SystemHandle systemHandle) where T : class, IEnableableComponent
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012DB RID: 4827 RVA: 0x000326DA File Offset: 0x000308DA
			public static void SetComponentEnabled<T>(SystemHandle systemHandle, bool value) where T : class, IEnableableComponent
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012DC RID: 4828 RVA: 0x0004AE18 File Offset: 0x00049018
			public static T GetSingleton<T>() where T : class
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012DD RID: 4829 RVA: 0x000326DA File Offset: 0x000308DA
			public static bool TryGetSingleton<T>(out T value) where T : class
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012DE RID: 4830 RVA: 0x0004AE2C File Offset: 0x0004902C
			public static Entity GetSingletonEntity<T>() where T : class
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012DF RID: 4831 RVA: 0x000326DA File Offset: 0x000308DA
			public static bool TryGetSingletonEntity<T>(out Entity value) where T : class
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012E0 RID: 4832 RVA: 0x000326DA File Offset: 0x000308DA
			public static bool HasSingleton<T>() where T : class
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012E1 RID: 4833 RVA: 0x0004AE40 File Offset: 0x00049040
			public static ComponentTypeHandle<T> GetComponentTypeHandle<T>(bool isReadOnly = false) where T : class, IComponentData
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x060012E2 RID: 4834 RVA: 0x0004AE54 File Offset: 0x00049054
			public static SharedComponentTypeHandle<T> GetSharedComponentTypeHandle<T>() where T : struct, ISharedComponentData
			{
				throw InternalCompilerInterface.ThrowCodeGenException();
			}

			// Token: 0x020002CA RID: 714
			public struct UnityEngineComponent<T> : IQueryTypeParameter where T : Object
			{
				// Token: 0x17000127 RID: 295
				// (get) Token: 0x060012E3 RID: 4835 RVA: 0x0004AE66 File Offset: 0x00049066
				public readonly T Value { get; }

				// Token: 0x060012E4 RID: 4836 RVA: 0x0004AE6E File Offset: 0x0004906E
				public UnityEngineComponent(T value)
				{
					this.Value = value;
				}
			}
		}
	}
}
