using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Feel
{
	// Token: 0x02000385 RID: 901
	[AddComponentMenu("")]
	public class FeelSpringsDemoSlider : MonoBehaviour
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x00068C88 File Offset: 0x00066E88
		public float value
		{
			get
			{
				return this.TargetSlider.value;
			}
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x00068C98 File Offset: 0x00066E98
		public void UpdateText()
		{
			this.ValueText.text = this.TargetSlider.value.ToString("F2");
		}

		// Token: 0x04001CB9 RID: 7353
		[Header("Bindings")]
		public Slider TargetSlider;

		// Token: 0x04001CBA RID: 7354
		public TMP_Text ValueText;
	}
}
