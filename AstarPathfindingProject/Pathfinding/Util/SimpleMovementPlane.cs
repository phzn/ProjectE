using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x0200029B RID: 667
	[GenerateTestsForBurstCompatibility]
	public readonly struct SimpleMovementPlane : IMovementPlane
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x000649D1 File Offset: 0x00062BD1
		public bool isXY
		{
			get
			{
				return this.plane == 1;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x000649DC File Offset: 0x00062BDC
		public bool isXZ
		{
			get
			{
				return this.plane == 2;
			}
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x000649E8 File Offset: 0x00062BE8
		public SimpleMovementPlane(Quaternion rotation)
		{
			this.rotation = rotation;
			this.inverseRotation = Quaternion.Inverse(rotation);
			if (rotation == SimpleMovementPlane.XYPlane.rotation)
			{
				this.plane = 1;
				return;
			}
			if (rotation == Quaternion.identity)
			{
				this.plane = 2;
				return;
			}
			this.plane = 0;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00064A40 File Offset: 0x00062C40
		public Vector2 ToPlane(Vector3 point)
		{
			if (this.isXY)
			{
				return new Vector2(point.x, point.y);
			}
			if (!this.isXZ)
			{
				point = this.inverseRotation * point;
			}
			return new Vector2(point.x, point.z);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00064A90 File Offset: 0x00062C90
		public float2 ToPlane(float3 point)
		{
			return (this.inverseRotation * point).xz;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00064ABB File Offset: 0x00062CBB
		public Vector2 ToPlane(Vector3 point, out float elevation)
		{
			if (!this.isXZ)
			{
				point = this.inverseRotation * point;
			}
			elevation = point.y;
			return new Vector2(point.x, point.z);
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00064AEC File Offset: 0x00062CEC
		public float2 ToPlane(float3 point, out float elevation)
		{
			point = math.mul(this.inverseRotation, point);
			elevation = point.y;
			return point.xz;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00064B10 File Offset: 0x00062D10
		public Vector3 ToWorld(Vector2 point, float elevation = 0f)
		{
			return this.rotation * new Vector3(point.x, elevation, point.y);
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x00064B2F File Offset: 0x00062D2F
		public float3 ToWorld(float2 point, float elevation = 0f)
		{
			return this.rotation * new Vector3(point.x, elevation, point.y);
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00064B53 File Offset: 0x00062D53
		public SimpleMovementPlane ToSimpleMovementPlane()
		{
			return this;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x00064B5B File Offset: 0x00062D5B
		public static bool operator ==(SimpleMovementPlane lhs, SimpleMovementPlane rhs)
		{
			return lhs.rotation == rhs.rotation;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00064B6E File Offset: 0x00062D6E
		public static bool operator !=(SimpleMovementPlane lhs, SimpleMovementPlane rhs)
		{
			return lhs.rotation != rhs.rotation;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00064B81 File Offset: 0x00062D81
		public override bool Equals(object other)
		{
			return other is SimpleMovementPlane && this.rotation == ((SimpleMovementPlane)other).rotation;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00064BA4 File Offset: 0x00062DA4
		public override int GetHashCode()
		{
			return this.rotation.GetHashCode();
		}

		// Token: 0x04000BF9 RID: 3065
		public readonly Quaternion rotation;

		// Token: 0x04000BFA RID: 3066
		public readonly Quaternion inverseRotation;

		// Token: 0x04000BFB RID: 3067
		private readonly byte plane;

		// Token: 0x04000BFC RID: 3068
		public static readonly SimpleMovementPlane XYPlane = new SimpleMovementPlane(Quaternion.Euler(-90f, 0f, 0f));

		// Token: 0x04000BFD RID: 3069
		public static readonly SimpleMovementPlane XZPlane = new SimpleMovementPlane(Quaternion.identity);
	}
}
