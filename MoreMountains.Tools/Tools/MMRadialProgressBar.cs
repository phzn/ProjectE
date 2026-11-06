using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000053 RID: 83
	[Obsolete("This component is obsolete, it's recommended to use MMProgressBar instead", true)]
	public class MMRadialProgressBar : MonoBehaviour
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x000092CE File Offset: 0x000074CE
		protected virtual void Awake()
		{
			this._radialImage = base.GetComponent<Image>();
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000092DC File Offset: 0x000074DC
		public virtual void UpdateBar(float currentValue, float minValue, float maxValue)
		{
			this._newPercent = MMMaths.Remap(currentValue, minValue, maxValue, this.StartValue, this.EndValue);
			if (this._radialImage == null)
			{
				return;
			}
			this._radialImage.fillAmount = this._newPercent;
			if (this._radialImage.fillAmount > 1f - this.Tolerance)
			{
				this._radialImage.fillAmount = 1f;
			}
			if (this._radialImage.fillAmount < this.Tolerance)
			{
				this._radialImage.fillAmount = 0f;
			}
		}

		// Token: 0x0400027F RID: 639
		public float StartValue = 1f;

		// Token: 0x04000280 RID: 640
		public float EndValue;

		// Token: 0x04000281 RID: 641
		public float Tolerance = 0.01f;

		// Token: 0x04000282 RID: 642
		public string PlayerID;

		// Token: 0x04000283 RID: 643
		protected Image _radialImage;

		// Token: 0x04000284 RID: 644
		protected float _newPercent;
	}
}
