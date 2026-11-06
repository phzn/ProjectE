using System;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000005 RID: 5
	[Serializable]
	public struct BezierKnot : ISerializationCallbackReceiver, IEquatable<BezierKnot>
	{
		// Token: 0x06000018 RID: 24 RVA: 0x0000247D File Offset: 0x0000067D
		public BezierKnot(float3 position)
		{
			this = new BezierKnot(position, 0f, 0f, quaternion.identity);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000249F File Offset: 0x0000069F
		public BezierKnot(float3 position, float3 tangentIn, float3 tangentOut)
		{
			this = new BezierKnot(position, tangentIn, tangentOut, quaternion.identity);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024AF File Offset: 0x000006AF
		public BezierKnot(float3 position, float3 tangentIn, float3 tangentOut, quaternion rotation)
		{
			this.Position = position;
			this.TangentIn = tangentIn;
			this.TangentOut = tangentOut;
			this.Rotation = rotation;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000024D0 File Offset: 0x000006D0
		public BezierKnot Transform(float4x4 matrix)
		{
			quaternion quaternion = math.mul(new quaternion(matrix), this.Rotation);
			quaternion quaternion2 = math.inverse(quaternion);
			return new BezierKnot(math.transform(matrix, this.Position), math.rotate(quaternion2, math.rotate(matrix, math.rotate(this.Rotation, this.TangentIn))), math.rotate(quaternion2, math.rotate(matrix, math.rotate(this.Rotation, this.TangentOut))), quaternion);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002542 File Offset: 0x00000742
		public static BezierKnot operator +(BezierKnot knot, float3 rhs)
		{
			return new BezierKnot(knot.Position + rhs, knot.TangentIn, knot.TangentOut, knot.Rotation);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002567 File Offset: 0x00000767
		public static BezierKnot operator -(BezierKnot knot, float3 rhs)
		{
			return new BezierKnot(knot.Position - rhs, knot.TangentIn, knot.TangentOut, knot.Rotation);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000258C File Offset: 0x0000078C
		internal BezierKnot BakeTangentDirectionToRotation(bool mirrored, BezierTangent main = BezierTangent.Out)
		{
			if (mirrored)
			{
				float num = math.length((main == BezierTangent.In) ? this.TangentIn : this.TangentOut);
				return new BezierKnot(this.Position, new float3(0f, 0f, -num), new float3(0f, 0f, num), SplineUtility.GetKnotRotation(math.mul(this.Rotation, (main == BezierTangent.In) ? (-this.TangentIn) : this.TangentOut), math.mul(this.Rotation, math.up())));
			}
			return new BezierKnot(this.Position, new float3(0f, 0f, -math.length(this.TangentIn)), new float3(0f, 0f, math.length(this.TangentOut)), this.Rotation = SplineUtility.GetKnotRotation(math.mul(this.Rotation, (main == BezierTangent.In) ? (-this.TangentIn) : this.TangentOut), math.mul(this.Rotation, math.up())));
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002695 File Offset: 0x00000895
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002697 File Offset: 0x00000897
		public void OnAfterDeserialize()
		{
			if (math.lengthsq(this.Rotation) == 0f)
			{
				this.Rotation = quaternion.identity;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000026B8 File Offset: 0x000008B8
		public override string ToString()
		{
			return string.Format("{{{0}, {1}, {2}, {3}}}", new object[] { this.Position, this.TangentIn, this.TangentOut, this.Rotation });
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002710 File Offset: 0x00000910
		public bool Equals(BezierKnot other)
		{
			return this.Position.Equals(other.Position) && this.TangentIn.Equals(other.TangentIn) && this.TangentOut.Equals(other.TangentOut) && this.Rotation.Equals(other.Rotation);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000276C File Offset: 0x0000096C
		public override bool Equals(object obj)
		{
			if (obj is BezierKnot)
			{
				BezierKnot bezierKnot = (BezierKnot)obj;
				return this.Equals(bezierKnot);
			}
			return false;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002791 File Offset: 0x00000991
		public override int GetHashCode()
		{
			return HashCode.Combine<float3, float3, float3, quaternion>(this.Position, this.TangentIn, this.TangentOut, this.Rotation);
		}

		// Token: 0x04000005 RID: 5
		public float3 Position;

		// Token: 0x04000006 RID: 6
		public float3 TangentIn;

		// Token: 0x04000007 RID: 7
		public float3 TangentOut;

		// Token: 0x04000008 RID: 8
		public quaternion Rotation;
	}
}
