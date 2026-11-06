using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class vCoverLinePoint : MonoBehaviour
{
	// Token: 0x06000946 RID: 2374 RVA: 0x00004F31 File Offset: 0x00003131
	private void Reset()
	{
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06000947 RID: 2375 RVA: 0x000437FE File Offset: 0x000419FE
	public float tangentsLenght
	{
		get
		{
			return this.tangentLeft.magnitude + this.tangentRight.magnitude;
		}
	}

	// Token: 0x0400087F RID: 2175
	public Vector3 tangentLeft = Vector3.one;

	// Token: 0x04000880 RID: 2176
	public Vector3 tangentRight = Vector3.one * -1f;

	// Token: 0x04000881 RID: 2177
	public vCoverLine.CurveType curveType = vCoverLine.CurveType.Align;

	// Token: 0x04000882 RID: 2178
	public vCoverLinePoint conectedLeft;

	// Token: 0x04000883 RID: 2179
	public vCoverLinePoint conectedRight;
}
