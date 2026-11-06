using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x0200002E RID: 46
	[NativeHeader("Modules/XR/Subsystems/Meshing/XRMeshBindings.h")]
	[UsedByNativeCode]
	public struct MeshInfo : IEquatable<MeshInfo>
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00004A13 File Offset: 0x00002C13
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00004A1B File Offset: 0x00002C1B
		public MeshId MeshId { readonly get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00004A24 File Offset: 0x00002C24
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00004A2C File Offset: 0x00002C2C
		public MeshChangeState ChangeState { readonly get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00004A35 File Offset: 0x00002C35
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00004A3D File Offset: 0x00002C3D
		public int PriorityHint { readonly get; set; }

		// Token: 0x06000159 RID: 345 RVA: 0x00004A48 File Offset: 0x00002C48
		public override bool Equals(object obj)
		{
			bool flag = !(obj is MeshInfo);
			return !flag && this.Equals((MeshInfo)obj);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00004A7C File Offset: 0x00002C7C
		public bool Equals(MeshInfo other)
		{
			return this.MeshId.Equals(other.MeshId) && this.ChangeState.Equals(other.ChangeState) && this.PriorityHint.Equals(other.PriorityHint);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00004AE0 File Offset: 0x00002CE0
		public static bool operator ==(MeshInfo lhs, MeshInfo rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00004AFC File Offset: 0x00002CFC
		public static bool operator !=(MeshInfo lhs, MeshInfo rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00004B1C File Offset: 0x00002D1C
		public override int GetHashCode()
		{
			return HashCodeHelper.Combine(this.MeshId.GetHashCode(), ((int)this.ChangeState).GetHashCode(), this.PriorityHint.GetHashCode());
		}
	}
}
