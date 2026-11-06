using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000897 RID: 2199
	public class HueSliderControl : MonoBehaviour
	{
		// Token: 0x06003B31 RID: 15153 RVA: 0x0015FBFC File Offset: 0x0015DDFC
		private void Awake()
		{
			if (this.colorPicker == null)
			{
				this.colorPicker = base.GetComponentInParent<ColorPickerControlExtended>(true);
				if (this.colorPicker == null)
				{
					this.colorPicker = Object.FindFirstObjectByType<ColorPickerControlExtended>();
				}
				if (this.colorPicker == null)
				{
					Debug.LogError("HueSliderControl: ColorPickerControlExtended non trovato!");
				}
			}
			if (this.hueSlider != null)
			{
				this.hueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnHueSliderValueChanged));
				return;
			}
			Debug.LogError("HueSliderControl: Slider non assegnato!");
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x0015FC8A File Offset: 0x0015DE8A
		private void OnHueSliderValueChanged(float value)
		{
			if (this.colorPicker != null)
			{
				this.colorPicker.SetHue(value);
				this.UpdateColorPreview();
			}
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x0015FCAC File Offset: 0x0015DEAC
		public void UpdateColorPreview()
		{
			if (this.handleImage != null)
			{
				this.handleImage.color = Color.white;
			}
			if (this.colorPreviewImage != null && this.colorPicker != null)
			{
				this.colorPreviewImage.color = Color.HSVToRGB(this.colorPicker.currentHue, 1f, 1f);
			}
		}

		// Token: 0x04003B5D RID: 15197
		[SerializeField]
		private Image handleImage;

		// Token: 0x04003B5E RID: 15198
		[Tooltip("Riferimento all'immagine interna che mostra l'anteprima del colore selezionato")]
		[SerializeField]
		private Image colorPreviewImage;

		// Token: 0x04003B5F RID: 15199
		[Tooltip("Riferimento al ColorPickerControlExtended. Se non specificato, verrà cercato automaticamente.")]
		[SerializeField]
		private ColorPickerControlExtended colorPicker;

		// Token: 0x04003B60 RID: 15200
		[SerializeField]
		private Slider hueSlider;
	}
}
