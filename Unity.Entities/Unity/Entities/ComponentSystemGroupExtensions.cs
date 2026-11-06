using System;

namespace Unity.Entities
{
	// Token: 0x0200003B RID: 59
	[Obsolete("This class will soon be empty and will be removed. (RemovedAfter Entities 1.0)")]
	public static class ComponentSystemGroupExtensions
	{
		// Token: 0x060001CB RID: 459 RVA: 0x000091A2 File Offset: 0x000073A2
		[Obsolete("RemoveUnmanagedSystemFromUpdateList has been deprecated. Please use RemoveSystemFromUpdateList. (RemovedAfter Entities 1.0)")]
		public static void RemoveUnmanagedSystemFromUpdateList(this ComponentSystemGroup self, SystemHandle sysHandle)
		{
			self.RemoveSystemFromUpdateList(sysHandle);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000091AB File Offset: 0x000073AB
		[Obsolete("AddUnmanagedSystemToUpdateList has been deprecated. Please use AddSystemToUpdateList. (RemovedAfter Entities 1.0)")]
		public static void AddUnmanagedSystemToUpdateList(this ComponentSystemGroup self, SystemHandle sysHandle)
		{
			self.AddSystemToUpdateList(sysHandle);
		}
	}
}
