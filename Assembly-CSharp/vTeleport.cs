using System;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class vTeleport : MonoBehaviour
{
	// Token: 0x060009F0 RID: 2544 RVA: 0x000456D4 File Offset: 0x000438D4
	public void Teleport(Collider collider)
	{
		Transform transform = (this.includeRoot ? collider.transform.root : collider.transform);
		Vector3 vector = base.transform.InverseTransformPoint(transform.position);
		Vector3 vector2 = base.transform.InverseTransformDirection(transform.forward);
		vector.Set(0f, vector.y, 0f);
		transform.position = this.targetPoint.TransformPoint(vector);
		switch (this.rotationType)
		{
		case vTeleport.RotationType.None:
			break;
		case vTeleport.RotationType.TargetForward:
			transform.rotation = this.targetPoint.rotation;
			break;
		case vTeleport.RotationType.RelativeForward:
			transform.forward = this.targetPoint.TransformDirection(vector2);
			return;
		default:
			return;
		}
	}

	// Token: 0x0400092F RID: 2351
	public Transform targetPoint;

	// Token: 0x04000930 RID: 2352
	public bool includeRoot;

	// Token: 0x04000931 RID: 2353
	public vTeleport.RotationType rotationType;

	// Token: 0x02000123 RID: 291
	public enum RotationType
	{
		// Token: 0x04000933 RID: 2355
		None,
		// Token: 0x04000934 RID: 2356
		TargetForward,
		// Token: 0x04000935 RID: 2357
		RelativeForward
	}
}
