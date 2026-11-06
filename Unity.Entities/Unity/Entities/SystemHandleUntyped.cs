using System;

namespace Unity.Entities
{
	// Token: 0x02000358 RID: 856
	[Obsolete("(UnityUpgradable) -> SystemHandle", true)]
	public struct SystemHandleUntyped : IEquatable<SystemHandleUntyped>, IComparable<SystemHandleUntyped>
	{
		// Token: 0x060016DE RID: 5854 RVA: 0x00056C4D File Offset: 0x00054E4D
		internal SystemHandleUntyped(Entity systemEntity, ushort handle, ushort version, uint worldSeqNo)
		{
			this.m_Entity = systemEntity;
			this.m_Handle = handle;
			this.m_Version = version;
			this.m_WorldSeqNo = worldSeqNo;
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		public int CompareTo(SystemHandleUntyped other)
		{
			return 0;
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		public override bool Equals(object obj)
		{
			return false;
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		public bool Equals(SystemHandleUntyped other)
		{
			return false;
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		public static bool operator ==(SystemHandleUntyped lhs, SystemHandleUntyped rhs)
		{
			return false;
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		public static bool operator !=(SystemHandleUntyped lhs, SystemHandleUntyped rhs)
		{
			return false;
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x00002287 File Offset: 0x00000487
		public void Update(WorldUnmanaged world)
		{
		}

		// Token: 0x040008A6 RID: 2214
		internal Entity m_Entity;

		// Token: 0x040008A7 RID: 2215
		internal ushort m_Handle;

		// Token: 0x040008A8 RID: 2216
		internal ushort m_Version;

		// Token: 0x040008A9 RID: 2217
		internal uint m_WorldSeqNo;
	}
}
