using System;
using UnityEngine;

// Token: 0x02000116 RID: 278
public struct vGroundPlaneCast
{
	// Token: 0x17000211 RID: 529
	// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0004495E File Offset: 0x00042B5E
	public readonly int planeAngle
	{
		get
		{
			return (int)Vector3.Angle(Vector3.up, this.planeNormal);
		}
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x00044974 File Offset: 0x00042B74
	public void CalculatePlane(Transform transform, Vector3 position, Vector3 down, float distance, LayerMask mask, float plane = 0.5f)
	{
		Vector3 position2 = transform.position;
		float num = plane * 0.5f;
		this.planeNormal = -down;
		this.p1 = position + transform.forward * num - transform.right * num;
		this.p2 = position + transform.forward * num + transform.right * num;
		this.p3 = position - transform.forward * num + transform.right * num;
		this.p4 = position - transform.forward * num - transform.right * num;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (Physics.Raycast(position, down, out this.hit, distance, mask))
		{
			this.centerNormal = this.hit.normal;
			this.planeCenter = this.hit.point;
		}
		else
		{
			this.planeCenter = transform.position + Vector3.down * distance;
		}
		if (Physics.Raycast(this.p1, down, out this.hit, distance, mask))
		{
			this.p1 = this.hit.point;
			flag = true;
		}
		else
		{
			this.p1 = position2 + Vector3.forward * num + Vector3.left * num;
		}
		if (Physics.Raycast(this.p2, down, out this.hit, distance, mask))
		{
			this.p2 = this.hit.point;
			flag2 = true;
		}
		else
		{
			this.p2 = position2 + Vector3.forward * num + Vector3.right * num;
		}
		if (Physics.Raycast(this.p3, down, out this.hit, distance, mask))
		{
			this.p3 = this.hit.point;
			flag3 = true;
		}
		else
		{
			this.p3 = position2 + Vector3.back * num + Vector3.right * num;
		}
		if (Physics.Raycast(this.p4, down, out this.hit, distance, mask))
		{
			this.p4 = this.hit.point;
			flag4 = true;
		}
		else
		{
			this.p4 = position2 + Vector3.back * num + Vector3.left * num;
		}
		Vector3 vector = (this.p1 + this.p2 + this.p3 + this.p4) / 4f;
		this.planeCenter = vector;
		this.planeNormal = -this.GetNormal(this.p1, this.p2, this.p3) + -this.GetNormal(this.p2, this.p1, this.p3) + -this.GetNormal(this.p3, this.p2, this.p4) + -this.GetNormal(this.p4, this.p3, this.p1);
		this.planeDistance = (position - this.planeCenter).magnitude;
		this.hasFloor = flag || flag2 || flag3 || flag4;
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x00044CF8 File Offset: 0x00042EF8
	private readonly Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
	{
		Vector3 vector = b - a;
		Vector3 vector2 = c - a;
		return Vector3.Cross(vector, vector2).normalized;
	}

	// Token: 0x04000903 RID: 2307
	public Vector3 p1;

	// Token: 0x04000904 RID: 2308
	public Vector3 p2;

	// Token: 0x04000905 RID: 2309
	public Vector3 p3;

	// Token: 0x04000906 RID: 2310
	public Vector3 p4;

	// Token: 0x04000907 RID: 2311
	public Vector3 planeNormal;

	// Token: 0x04000908 RID: 2312
	public Vector3 planeCenter;

	// Token: 0x04000909 RID: 2313
	public Vector3 centerNormal;

	// Token: 0x0400090A RID: 2314
	public float planeDistance;

	// Token: 0x0400090B RID: 2315
	public bool hasFloor;

	// Token: 0x0400090C RID: 2316
	private RaycastHit hit;
}
