using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000850 RID: 2128
	public class BlendSlider : MonoBehaviour
	{
		// Token: 0x0600386B RID: 14443 RVA: 0x00149A24 File Offset: 0x00147C24
		public void Init(OutfitSystem system, string key)
		{
			this.slider = base.GetComponentInChildren<Slider>();
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.Apply));
			this.system = system;
			this.title.text = key;
			this.shape = key;
			float shapeValue = system.GetShapeValue(key);
			this.slider.value = shapeValue;
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x00149A88 File Offset: 0x00147C88
		private void OnEnable()
		{
			float shapeValue = this.system.GetShapeValue(this.shape);
			this.slider.value = shapeValue;
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x00004F31 File Offset: 0x00003131
		private void UpdateSlider()
		{
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x00149AB3 File Offset: 0x00147CB3
		public void Apply(float value)
		{
			this.system.SetShape(this.shape, value);
			this.sliderValue.text = string.Format("{0}%", this.slider.value);
		}

		// Token: 0x04003953 RID: 14675
		private Slider slider;

		// Token: 0x04003954 RID: 14676
		[SerializeField]
		private TMP_Text title;

		// Token: 0x04003955 RID: 14677
		[SerializeField]
		private TMP_Text sliderValue;

		// Token: 0x04003956 RID: 14678
		[SerializeField]
		private OutfitSystem system;

		// Token: 0x04003957 RID: 14679
		[SerializeField]
		private string shape;
	}
}
