using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x02000027 RID: 39
	[NativeHeader("Modules/XR/Subsystems/Meshing/XRMeshBindings.h")]
	[UsedByNativeCode]
	public struct MeshId : IEquatable<MeshId>
	{
		// Token: 0x06000136 RID: 310 RVA: 0x00004624 File Offset: 0x00002824
		public override string ToString()
		{
			return string.Format("{0}-{1}", this.m_SubId1.ToString("X16"), this.m_SubId2.ToString("X16"));
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00004660 File Offset: 0x00002860
		public override int GetHashCode()
		{
			return this.m_SubId1.GetHashCode() ^ this.m_SubId2.GetHashCode();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000468C File Offset: 0x0000288C
		public override bool Equals(object obj)
		{
			return obj is MeshId && this.Equals((MeshId)obj);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000046B8 File Offset: 0x000028B8
		public bool Equals(MeshId other)
		{
			return this.m_SubId1 == other.m_SubId1 && this.m_SubId2 == other.m_SubId2;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000046EC File Offset: 0x000028EC
		public static bool operator ==(MeshId id1, MeshId id2)
		{
			return id1.m_SubId1 == id2.m_SubId1 && id1.m_SubId2 == id2.m_SubId2;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004720 File Offset: 0x00002920
		public static bool operator !=(MeshId id1, MeshId id2)
		{
			return id1.m_SubId1 != id2.m_SubId1 || id1.m_SubId2 != id2.m_SubId2;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00004754 File Offset: 0x00002954
		public static MeshId InvalidId
		{
			get
			{
				return MeshId.s_InvalidId;
			}
		}

		// Token: 0x040000F1 RID: 241
		private static MeshId s_InvalidId = default(MeshId);

		// Token: 0x040000F2 RID: 242
		private ulong m_SubId1;

		// Token: 0x040000F3 RID: 243
		private ulong m_SubId2;
	}
}
