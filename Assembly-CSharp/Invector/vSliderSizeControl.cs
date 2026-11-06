using System;
using UnityEngine;
using UnityEngine.UI;

namespace Invector
{
	// Token: 0x020004B6 RID: 1206
	public class vSliderSizeControl : MonoBehaviour
	{
		// Token: 0x06001D56 RID: 7510 RVA: 0x000B43C4 File Offset: 0x000B25C4
		private void OnDrawGizmosSelected()
		{
			this.UpdateScale();
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x000B43CC File Offset: 0x000B25CC
		private void Start()
		{
			base.Invoke("UpdateScale", 0.1f);
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x000B43E0 File Offset: 0x000B25E0
		public void UpdateScale()
		{
			if (this.rectTransform && this.slider && this.slider.maxValue != this.oldMaxValue)
			{
				Vector2 sizeDelta = this.rectTransform.sizeDelta;
				sizeDelta.x = this.slider.maxValue * this.multipScale;
				this.rectTransform.sizeDelta = sizeDelta;
				this.oldMaxValue = this.slider.maxValue;
			}
		}

		// Token: 0x04001E64 RID: 7780
		public Slider slider;

		// Token: 0x04001E65 RID: 7781
		public RectTransform rectTransform;

		// Token: 0x04001E66 RID: 7782
		public float multipScale = 0.1f;

		// Token: 0x04001E67 RID: 7783
		private float oldMaxValue;
	}
}
