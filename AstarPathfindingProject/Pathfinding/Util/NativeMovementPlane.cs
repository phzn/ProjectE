using System;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000299 RID: 665
	public readonly struct NativeMovementPlane
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600108F RID: 4239 RVA: 0x00064684 File Offset: 0x00062884
		public float3 up
		{
			get
			{
				return 2f * new float3(this.rotation.value.x * this.rotation.value.y - this.rotation.value.w * this.rotation.value.z, 0.5f - this.rotation.value.x * this.rotation.value.x - this.rotation.value.z * this.rotation.value.z, this.rotation.value.w * this.rotation.value.x + this.rotation.value.y * this.rotation.value.z);
			}
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0006476F File Offset: 0x0006296F
		public NativeMovementPlane(quaternion rotation)
		{
			this.rotation = math.normalizesafe(rotation);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0006477D File Offset: 0x0006297D
		public NativeMovementPlane(SimpleMovementPlane plane)
		{
			this = new NativeMovementPlane(plane.rotation);
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00064790 File Offset: 0x00062990
		public ToPlaneMatrix AsWorldToPlaneMatrix()
		{
			return new ToPlaneMatrix(this);
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0006479D File Offset: 0x0006299D
		public ToWorldMatrix AsPlaneToWorldMatrix()
		{
			return new ToWorldMatrix(this);
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x000647AC File Offset: 0x000629AC
		public NativeMovementPlane MatchUpDirection(float3 up)
		{
			float3 @float = math.normalizesafe(math.mul(this.rotation, new float3(0f, 0f, 1f)), default(float3));
			up = math.normalizesafe(up, default(float3));
			return new NativeMovementPlane(new quaternion(new float3x3(math.cross(up, @float), up, @float)));
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00064810 File Offset: 0x00062A10
		public float ProjectedLength(float3 v)
		{
			return math.length(this.ToPlane(v));
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00064820 File Offset: 0x00062A20
		public float2 ToPlane(float3 p)
		{
			return math.mul(math.conjugate(this.rotation), p).xz;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x00064846 File Offset: 0x00062A46
		public float2 ToPlane(float3 p, out float elevation)
		{
			p = math.mul(math.conjugate(this.rotation), p);
			elevation = p.y;
			return p.xz;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0006486A File Offset: 0x00062A6A
		public float3 ToWorld(float2 p, float elevation = 0f)
		{
			return math.mul(this.rotation, new float3(p.x, elevation, p.y));
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0006488C File Offset: 0x00062A8C
		public float ToPlane(quaternion rotation)
		{
			quaternion quaternion = math.mul(math.conjugate(this.rotation), rotation);
			if (quaternion.value.y < 0f)
			{
				quaternion.value = -quaternion.value;
			}
			return -VectorMath.QuaternionAngle(math.normalizesafe(new quaternion(0f, quaternion.value.y, 0f, quaternion.value.w)));
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x000648FF File Offset: 0x00062AFF
		public quaternion ToWorldRotation(float angle)
		{
			return math.mul(this.rotation, quaternion.RotateY(-angle));
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00064913 File Offset: 0x00062B13
		public quaternion ToWorldRotationDelta(float deltaAngle)
		{
			return quaternion.AxisAngle(this.ToWorld(float2.zero, 1f), -deltaAngle);
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0006492C File Offset: 0x00062B2C
		public Bounds ToWorld(Bounds bounds)
		{
			return this.AsPlaneToWorldMatrix().ToWorld(bounds);
		}

		// Token: 0x04000BF8 RID: 3064
		public readonly quaternion rotation;
	}
}
