using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Transforms
{
	// Token: 0x0200000E RID: 14
	[Serializable]
	public struct LocalToWorld : IComponentData, IQueryTypeParameter
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002D87 File Offset: 0x00000F87
		public float3 Right
		{
			get
			{
				return new float3(this.Value.c0.x, this.Value.c0.y, this.Value.c0.z);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002DBE File Offset: 0x00000FBE
		public float3 Up
		{
			get
			{
				return new float3(this.Value.c1.x, this.Value.c1.y, this.Value.c1.z);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002DF5 File Offset: 0x00000FF5
		public float3 Forward
		{
			get
			{
				return new float3(this.Value.c2.x, this.Value.c2.y, this.Value.c2.z);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002E2C File Offset: 0x0000102C
		public float3 Position
		{
			get
			{
				return new float3(this.Value.c3.x, this.Value.c3.y, this.Value.c3.z);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002E63 File Offset: 0x00001063
		public quaternion Rotation
		{
			get
			{
				return new quaternion(math.orthonormalize(new float3x3(this.Value)));
			}
		}

		// Token: 0x0400002D RID: 45
		public float4x4 Value;
	}
}
