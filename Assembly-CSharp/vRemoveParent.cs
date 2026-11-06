using System;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class vRemoveParent : MonoBehaviour
{
	// Token: 0x060009CF RID: 2511 RVA: 0x00045344 File Offset: 0x00043544
	private void Start()
	{
		if (this.removeOnStart)
		{
			this.RemoveParent();
		}
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00045354 File Offset: 0x00043554
	public void RemoveParentOfOtherTransform(Transform target)
	{
		target.SetParent(null);
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0004535D File Offset: 0x0004355D
	public void RemoveParent()
	{
		base.transform.SetParent(null);
	}

	// Token: 0x0400092B RID: 2347
	public bool removeOnStart = true;
}
