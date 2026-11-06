using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x0200002A RID: 42
	[RequiredByNativeCode]
	[NativeHeader("Modules/XR/Subsystems/Meshing/XRMeshBindings.h")]
	public struct MeshGenerationResult : IEquatable<MeshGenerationResult>
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00004807 File Offset: 0x00002A07
		public readonly MeshId MeshId { get; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000480F File Offset: 0x00002A0F
		public readonly Mesh Mesh { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00004817 File Offset: 0x00002A17
		public readonly MeshCollider MeshCollider { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000481F File Offset: 0x00002A1F
		public readonly MeshGenerationStatus Status { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00004827 File Offset: 0x00002A27
		public readonly MeshVertexAttributes Attributes { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0000482F File Offset: 0x00002A2F
		public readonly ulong Timestamp { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00004837 File Offset: 0x00002A37
		public readonly Vector3 Position { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000483F File Offset: 0x00002A3F
		public readonly Quaternion Rotation { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00004847 File Offset: 0x00002A47
		public readonly Vector3 Scale { get; }

		// Token: 0x0600014E RID: 334 RVA: 0x00004850 File Offset: 0x00002A50
		public override bool Equals(object obj)
		{
			bool flag = !(obj is MeshGenerationResult);
			return !flag && this.Equals((MeshGenerationResult)obj);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00004884 File Offset: 0x00002A84
		public bool Equals(MeshGenerationResult other)
		{
			return this.MeshId.Equals(other.MeshId) && this.Mesh.Equals(other.Mesh) && this.MeshCollider.Equals(other.MeshCollider) && this.Status == other.Status && this.Attributes == other.Attributes && this.Position.Equals(other.Position) && this.Rotation.Equals(other.Rotation) && this.Scale.Equals(other.Scale);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000493C File Offset: 0x00002B3C
		public static bool operator ==(MeshGenerationResult lhs, MeshGenerationResult rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00004958 File Offset: 0x00002B58
		public static bool operator !=(MeshGenerationResult lhs, MeshGenerationResult rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00004978 File Offset: 0x00002B78
		public override int GetHashCode()
		{
			return HashCodeHelper.Combine(this.MeshId.GetHashCode(), this.Mesh.GetHashCode(), this.MeshCollider.GetHashCode(), ((int)this.Status).GetHashCode(), ((int)this.Attributes).GetHashCode(), this.Position.GetHashCode(), this.Rotation.GetHashCode(), this.Scale.GetHashCode());
		}
	}
}
