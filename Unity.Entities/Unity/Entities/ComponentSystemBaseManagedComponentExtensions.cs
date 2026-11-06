using System;

namespace Unity.Entities
{
	// Token: 0x02000038 RID: 56
	public static class ComponentSystemBaseManagedComponentExtensions
	{
		// Token: 0x0600019E RID: 414 RVA: 0x00007FC8 File Offset: 0x000061C8
		[Obsolete("Use SystemAPI.ManagedAPI.GetSingleton instead (RemovedAfter Entities 1.0)")]
		public unsafe static T GetSingleton<T>(this ComponentSystemBase sys) where T : class, IComponentData, new()
		{
			ComponentType componentType = ComponentType.ReadOnly<T>();
			return sys.CheckedState()->GetSingletonEntityQueryInternal(componentType).GetSingleton<T>();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007FEC File Offset: 0x000061EC
		[Obsolete("Use SystemAPI.ManagedAPI.GetSingletonRW instead (RemovedAfter Entities 1.0)")]
		public unsafe static T GetSingletonRW<T>(this ComponentSystemBase sys) where T : class, IComponentData, new()
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			return sys.CheckedState()->GetSingletonEntityQueryInternal(componentType).GetSingletonRW<T>();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008010 File Offset: 0x00006210
		[Obsolete("Use SystemAPI.ManagedAPI.SetSingleton instead (RemovedAfter Entities 1.0)")]
		public unsafe static void SetSingleton<T>(this ComponentSystemBase sys, T value) where T : class, IComponentData, new()
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			sys.CheckedState()->GetSingletonEntityQueryInternal(componentType).SetSingleton(value);
		}
	}
}
