using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000167 RID: 359
	[AddComponentMenu("")]
	public class MMDebugMenuTestClass : MonoBehaviour
	{
		// Token: 0x06000885 RID: 2181 RVA: 0x000287A3 File Offset: 0x000269A3
		private void Start()
		{
			this.multiplier = Random.Range(0f, 50000f);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x000287BC File Offset: 0x000269BC
		private void Update()
		{
			float num = (Mathf.Sin(Time.time) + 2f) * this.multiplier;
			MMDebug.DebugOnScreen(this.Label, num, 25);
		}

		// Token: 0x04000A31 RID: 2609
		public string Label;

		// Token: 0x04000A32 RID: 2610
		private float multiplier;
	}
}
