using System;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class ResetPosition : MonoBehaviour
{
	// Token: 0x06000BBC RID: 3004 RVA: 0x000510B3 File Offset: 0x0004F2B3
	private void Start()
	{
		this.startPosition = base.transform.position;
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x000510C6 File Offset: 0x0004F2C6
	private void Update()
	{
		if (Vector3.Distance(this.startPosition, base.transform.position) >= this.distanceToReset)
		{
			base.transform.position = this.startPosition;
		}
	}

	// Token: 0x04000B85 RID: 2949
	public float distanceToReset = 5f;

	// Token: 0x04000B86 RID: 2950
	private Vector3 startPosition;
}
