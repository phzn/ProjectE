using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x0200002F RID: 47
	[UsedByNativeCode]
	[NativeHeader("Modules/XR/Subsystems/Meshing/XRMeshBindings.h")]
	public readonly struct MeshTransform : IEquatable<MeshTransform>
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00004B5E File Offset: 0x00002D5E
		public MeshId MeshId { get; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00004B66 File Offset: 0x00002D66
		public ulong Timestamp { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00004B6E File Offset: 0x00002D6E
		public Vector3 Position { get; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00004B76 File Offset: 0x00002D76
		public Quaternion Rotation { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00004B7E File Offset: 0x00002D7E
		public Vector3 Scale { get; }

		// Token: 0x06000163 RID: 355 RVA: 0x00004B86 File Offset: 0x00002D86
		public MeshTransform(in MeshId meshId, ulong timestamp, in Vector3 position, in Quaternion rotation, in Vector3 scale)
		{
			this.MeshId = meshId;
			this.Timestamp = timestamp;
			this.Position = position;
			this.Rotation = rotation;
			this.Scale = scale;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00004BC4 File Offset: 0x00002DC4
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is MeshTransform)
			{
				MeshTransform meshTransform = (MeshTransform)obj;
				flag = this.Equals(meshTransform);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00004BEC File Offset: 0x00002DEC
		public bool Equals(MeshTransform other)
		{
			return this.MeshId.Equals(other.MeshId) && this.Timestamp == other.Timestamp && this.Position.Equals(other.Position) && this.Rotation.Equals(other.Rotation) && this.Scale.Equals(other.Scale);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00004C65 File Offset: 0x00002E65
		public static bool operator ==(MeshTransform lhs, MeshTransform rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00004C6F File Offset: 0x00002E6F
		public static bool operator !=(MeshTransform lhs, MeshTransform rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00004C7C File Offset: 0x00002E7C
		public override int GetHashCode()
		{
			return HashCodeHelper.Combine(this.MeshId.GetHashCode(), this.Timestamp.GetHashCode(), this.Position.GetHashCode(), this.Rotation.GetHashCode(), this.Scale.GetHashCode());
		}
	}
}
