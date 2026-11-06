using System;
using UnityEngine;

namespace SplineMesh
{
	// Token: 0x02000181 RID: 385
	public struct CurveSample
	{
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x00053630 File Offset: 0x00051830
		public Quaternion Rotation
		{
			get
			{
				if (this.rotation == Quaternion.identity)
				{
					Vector3 vector = Vector3.Cross(this.tangent, Vector3.Cross(Quaternion.AngleAxis(this.roll, Vector3.forward) * this.up, this.tangent).normalized);
					this.rotation = Quaternion.LookRotation(this.tangent, vector);
				}
				return this.rotation;
			}
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x000536A4 File Offset: 0x000518A4
		public CurveSample(Vector3 location, Vector3 tangent, Vector3 up, Vector2 scale, float roll, float distanceInCurve, float timeInCurve, CubicBezierCurve curve)
		{
			this.location = location;
			this.tangent = tangent;
			this.up = up;
			this.roll = roll;
			this.scale = scale;
			this.distanceInCurve = distanceInCurve;
			this.timeInCurve = timeInCurve;
			this.curve = curve;
			this.rotation = Quaternion.identity;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x000536FC File Offset: 0x000518FC
		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			CurveSample curveSample = (CurveSample)obj;
			return this.location == curveSample.location && this.tangent == curveSample.tangent && this.up == curveSample.up && this.scale == curveSample.scale && this.roll == curveSample.roll && this.distanceInCurve == curveSample.distanceInCurve && this.timeInCurve == curveSample.timeInCurve;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000537AA File Offset: 0x000519AA
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x000537BC File Offset: 0x000519BC
		public static bool operator ==(CurveSample cs1, CurveSample cs2)
		{
			return cs1.Equals(cs2);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x000537D1 File Offset: 0x000519D1
		public static bool operator !=(CurveSample cs1, CurveSample cs2)
		{
			return !cs1.Equals(cs2);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000537EC File Offset: 0x000519EC
		public static CurveSample Lerp(CurveSample a, CurveSample b, float t)
		{
			return new CurveSample(Vector3.Lerp(a.location, b.location, t), Vector3.Lerp(a.tangent, b.tangent, t).normalized, Vector3.Lerp(a.up, b.up, t), Vector2.Lerp(a.scale, b.scale, t), Mathf.Lerp(a.roll, b.roll, t), Mathf.Lerp(a.distanceInCurve, b.distanceInCurve, t), Mathf.Lerp(a.timeInCurve, b.timeInCurve, t), a.curve);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0005388C File Offset: 0x00051A8C
		public MeshVertex GetBent(MeshVertex vert)
		{
			MeshVertex meshVertex = new MeshVertex(vert.position, vert.normal, vert.uv);
			meshVertex.position = Vector3.Scale(meshVertex.position, new Vector3(0f, this.scale.y, this.scale.x));
			meshVertex.position = Quaternion.AngleAxis(this.roll, Vector3.right) * meshVertex.position;
			meshVertex.normal = Quaternion.AngleAxis(this.roll, Vector3.right) * meshVertex.normal;
			meshVertex.position.x = 0f;
			Quaternion quaternion = this.Rotation * Quaternion.Euler(0f, -90f, 0f);
			meshVertex.position = quaternion * meshVertex.position + this.location;
			meshVertex.normal = quaternion * meshVertex.normal;
			return meshVertex;
		}

		// Token: 0x04000C06 RID: 3078
		public readonly Vector3 location;

		// Token: 0x04000C07 RID: 3079
		public readonly Vector3 tangent;

		// Token: 0x04000C08 RID: 3080
		public readonly Vector3 up;

		// Token: 0x04000C09 RID: 3081
		public readonly Vector2 scale;

		// Token: 0x04000C0A RID: 3082
		public readonly float roll;

		// Token: 0x04000C0B RID: 3083
		public readonly float distanceInCurve;

		// Token: 0x04000C0C RID: 3084
		public readonly float timeInCurve;

		// Token: 0x04000C0D RID: 3085
		public readonly CubicBezierCurve curve;

		// Token: 0x04000C0E RID: 3086
		private Quaternion rotation;
	}
}
