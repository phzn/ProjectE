using System;
using Invector;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class vSnapToBody : MonoBehaviour
{
	// Token: 0x0600098A RID: 2442 RVA: 0x00044100 File Offset: 0x00042300
	private void Start()
	{
		this.bodySnap = base.transform.root.GetComponentInChildren<vBodySnappingControl>(true);
		if (this.boneName != "ManuallyAssign" && this.bodySnap != null && this.bodySnap.boneSnappingList != null)
		{
			this.boneToSnap = this.bodySnap.GetBone(this.boneName);
		}
		if (this.boneToSnap)
		{
			base.transform.parent = this.boneToSnap;
		}
	}

	// Token: 0x040008C8 RID: 2248
	public const string manuallyAssignBone = "ManuallyAssign";

	// Token: 0x040008C9 RID: 2249
	public vBodySnappingControl bodySnap;

	// Token: 0x040008CA RID: 2250
	public Transform boneToSnap;

	// Token: 0x040008CB RID: 2251
	public string boneName;
}
