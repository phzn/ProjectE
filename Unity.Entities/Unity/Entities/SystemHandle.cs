using System;
using System.Diagnostics;

namespace Unity.Entities
{
	// Token: 0x02000359 RID: 857
	[DebuggerTypeProxy(typeof(SystemDebugView))]
	public struct SystemHandle : IEquatable<SystemHandle>, IComparable<SystemHandle>
	{
		// Token: 0x060016E6 RID: 5862 RVA: 0x00056C6C File Offset: 0x00054E6C
		private ulong ToUlong()
		{
			return ((ulong)this.m_WorldSeqNo << 32) | ((ulong)this.m_Handle << 16) | (ulong)this.m_Version;
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00056C8B File Offset: 0x00054E8B
		internal SystemHandle(Entity systemEntity, ushort handle, ushort version, uint worldSeqNo)
		{
			this.m_Entity = systemEntity;
			this.m_Handle = handle;
			this.m_Version = version;
			this.m_WorldSeqNo = worldSeqNo;
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00056CAC File Offset: 0x00054EAC
		public int CompareTo(SystemHandle other)
		{
			ulong num = this.ToUlong();
			ulong num2 = other.ToUlong();
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00056CD8 File Offset: 0x00054ED8
		public override bool Equals(object obj)
		{
			if (obj is SystemHandle)
			{
				SystemHandle systemHandle = (SystemHandle)obj;
				return this.Equals(systemHandle);
			}
			return false;
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x00056CFD File Offset: 0x00054EFD
		public bool Equals(SystemHandle other)
		{
			return this.ToUlong() == other.ToUlong();
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00056D0E File Offset: 0x00054F0E
		public override int GetHashCode()
		{
			return ((-116238775 * -1521134295 + this.m_Handle.GetHashCode()) * -1521134295 + this.m_Version.GetHashCode()) * -1521134295 + this.m_WorldSeqNo.GetHashCode();
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x00056D4B File Offset: 0x00054F4B
		public static bool operator ==(SystemHandle lhs, SystemHandle rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x00056D55 File Offset: 0x00054F55
		public static bool operator !=(SystemHandle lhs, SystemHandle rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060016EE RID: 5870 RVA: 0x00056D64 File Offset: 0x00054F64
		public static SystemHandle Null
		{
			get
			{
				return default(SystemHandle);
			}
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00056D7A File Offset: 0x00054F7A
		public void Update(WorldUnmanaged world)
		{
			world.GetImpl().UpdateSystem(this);
		}

		// Token: 0x040008AA RID: 2218
		internal Entity m_Entity;

		// Token: 0x040008AB RID: 2219
		internal ushort m_Handle;

		// Token: 0x040008AC RID: 2220
		internal ushort m_Version;

		// Token: 0x040008AD RID: 2221
		internal uint m_WorldSeqNo;
	}
}
