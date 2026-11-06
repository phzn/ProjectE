using System;
using UnityEngine;

namespace ProjectExhibitionist.CoverWall
{
	// Token: 0x020009F9 RID: 2553
	[DefaultExecutionOrder(1000)]
	public class CoverSlideSpeedLimiter : MonoBehaviour
	{
		// Token: 0x060048BE RID: 18622 RVA: 0x0007CCF6 File Offset: 0x0007AEF6
		private void OnEnable()
		{
			base.enabled = false;
		}
	}
}
