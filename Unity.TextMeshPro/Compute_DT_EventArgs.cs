using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000011 RID: 17
	public class Compute_DT_EventArgs
	{
		// Token: 0x060000F3 RID: 243 RVA: 0x00016AFD File Offset: 0x00014CFD
		public Compute_DT_EventArgs(Compute_DistanceTransform_EventTypes type, float progress)
		{
			this.EventType = type;
			this.ProgressPercentage = progress;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00016B13 File Offset: 0x00014D13
		public Compute_DT_EventArgs(Compute_DistanceTransform_EventTypes type, Color[] colors)
		{
			this.EventType = type;
			this.Colors = colors;
		}

		// Token: 0x0400008D RID: 141
		public Compute_DistanceTransform_EventTypes EventType;

		// Token: 0x0400008E RID: 142
		public float ProgressPercentage;

		// Token: 0x0400008F RID: 143
		public Color[] Colors;
	}
}
