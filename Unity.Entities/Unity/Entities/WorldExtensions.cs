using System;
using System.Runtime.CompilerServices;

namespace Unity.Entities
{
	// Token: 0x02000352 RID: 850
	public static class WorldExtensions
	{
		// Token: 0x060016D4 RID: 5844 RVA: 0x00056BB4 File Offset: 0x00054DB4
		public static SystemHandle CreateSystem<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this World self) where T : struct, ValueType, ISystem
		{
			return self.Unmanaged.CreateUnmanagedSystem<T>(self, true);
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00056BD4 File Offset: 0x00054DD4
		public static SystemHandle GetExistingSystem<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this World self) where T : struct, ValueType, ISystem
		{
			return self.Unmanaged.GetExistingUnmanagedSystem<T>();
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00056BF0 File Offset: 0x00054DF0
		public static SystemHandle GetOrCreateSystem<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this World self) where T : struct, ValueType, ISystem
		{
			return self.Unmanaged.GetOrCreateUnmanagedSystem<T>();
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00056C0B File Offset: 0x00054E0B
		[Obsolete("Use World.GetOrCreateSystem instead")]
		public static SystemHandle GetOrCreateSystem(World self, Type unmanagedType)
		{
			return self.GetOrCreateSystem(unmanagedType);
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x00056C14 File Offset: 0x00054E14
		[Obsolete("Use World.DestroySystem instead")]
		public static void DestroySystem(World self, SystemHandle sysHandle)
		{
			self.DestroySystem(sysHandle);
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00056C1D File Offset: 0x00054E1D
		[Obsolete("Use World.CreateSystem instead")]
		public static SystemHandle AddSystem<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this World self) where T : struct, ValueType, ISystem
		{
			return self.CreateSystem<T>();
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00056C25 File Offset: 0x00054E25
		[Obsolete("Use World.GetOrCreateSystem instead")]
		public static SystemHandle GetOrCreateUnmanagedSystem(this World self, Type unmanagedType)
		{
			return WorldExtensions.GetOrCreateSystem(self, unmanagedType);
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00056C2E File Offset: 0x00054E2E
		[Obsolete("Use World.DestroySystem instead")]
		public static void DestroyUnmanagedSystem(this World self, SystemHandle sysHandle)
		{
			WorldExtensions.DestroySystem(self, sysHandle);
		}
	}
}
