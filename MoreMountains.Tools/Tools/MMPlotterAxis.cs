using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000027 RID: 39
	public class MMPlotterAxis : MonoBehaviour
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x0000530C File Offset: 0x0000350C
		public virtual void SetLabel(string newLabel)
		{
			this.Label.text = newLabel;
		}

		// Token: 0x04000118 RID: 280
		public Text Label;

		// Token: 0x04000119 RID: 281
		public Text TimeLabel;

		// Token: 0x0400011A RID: 282
		public Transform PlotterCurvePoint;

		// Token: 0x0400011B RID: 283
		public Transform PositionPoint;

		// Token: 0x0400011C RID: 284
		public Transform PositionPointVertical;

		// Token: 0x0400011D RID: 285
		public Transform RotationPoint;

		// Token: 0x0400011E RID: 286
		public Transform ScalePoint;
	}
}
