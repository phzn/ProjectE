using System;
using UnityEngine;

namespace SplineMesh
{
	// Token: 0x0200018C RID: 396
	[DisallowMultipleComponent]
	public class ExampleTentacle : MonoBehaviour
	{
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x000551B4 File Offset: 0x000533B4
		private Spline spline
		{
			get
			{
				return base.GetComponent<Spline>();
			}
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x000551BC File Offset: 0x000533BC
		private void OnValidate()
		{
			float num = 0f;
			foreach (CubicBezierCurve cubicBezierCurve in this.spline.GetCurves())
			{
				float num2 = num / this.spline.Length;
				num += cubicBezierCurve.Length;
				float num3 = num / this.spline.Length;
				cubicBezierCurve.n1.Scale = Vector2.one * (this.startScale + (this.endScale - this.startScale) * num2);
				cubicBezierCurve.n2.Scale = Vector2.one * (this.startScale + (this.endScale - this.startScale) * num3);
				cubicBezierCurve.n1.Roll = this.startRoll + (this.endRoll - this.startRoll) * num2;
				cubicBezierCurve.n2.Roll = this.startRoll + (this.endRoll - this.startRoll) * num3;
			}
		}

		// Token: 0x04000C4D RID: 3149
		public float startScale = 1f;

		// Token: 0x04000C4E RID: 3150
		public float endScale = 1f;

		// Token: 0x04000C4F RID: 3151
		public float startRoll;

		// Token: 0x04000C50 RID: 3152
		public float endRoll;
	}
}
