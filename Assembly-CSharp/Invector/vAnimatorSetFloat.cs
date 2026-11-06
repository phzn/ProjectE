using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000461 RID: 1121
	public class vAnimatorSetFloat : vAnimatorSetValue<float>
	{
		// Token: 0x06001BDD RID: 7133 RVA: 0x000AF2DC File Offset: 0x000AD4DC
		protected override float GetEnterValue()
		{
			float num;
			if (this.randomEnter)
			{
				num = Random.Range(base.GetEnterValue(), this.maxEnterValue);
				if (this.roundValue)
				{
					num = (float)Math.Round((double)num, this.roundDigits);
				}
			}
			else
			{
				num = base.GetEnterValue();
			}
			if (this.randomInvert && Random.Range(0, 100) > 50)
			{
				num *= -1f;
			}
			return num;
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x000AF348 File Offset: 0x000AD548
		protected override float GetExitValue()
		{
			float num;
			if (this.randomEnter)
			{
				num = Random.Range(base.GetExitValue(), this.maxEnterValue);
				if (this.roundValue)
				{
					num = (float)Math.Round((double)num, this.roundDigits);
				}
			}
			else
			{
				num = base.GetExitValue();
			}
			if (this.randomInvert && Random.Range(0, 100) > 50)
			{
				num *= -1f;
			}
			return num;
		}

		// Token: 0x04001D22 RID: 7458
		[vHelpBox("Random Value between Default Value and Max Value", vHelpBoxAttribute.MessageType.None)]
		public bool randomEnter;

		// Token: 0x04001D23 RID: 7459
		[vHideInInspector("randomEnter", false)]
		public float maxEnterValue;

		// Token: 0x04001D24 RID: 7460
		public bool randomExit;

		// Token: 0x04001D25 RID: 7461
		[vHideInInspector("randomExit", false)]
		public float maxExitValue;

		// Token: 0x04001D26 RID: 7462
		[vHelpBox("Use this in <b>Random mode</b> to generat a rounded value", vHelpBoxAttribute.MessageType.None)]
		public bool roundValue;

		// Token: 0x04001D27 RID: 7463
		[Tooltip("Digits after the comma")]
		[vHideInInspector("roundValue", false)]
		public int roundDigits = 1;

		// Token: 0x04001D28 RID: 7464
		[Tooltip("Invert number randomly")]
		public bool randomInvert;
	}
}
