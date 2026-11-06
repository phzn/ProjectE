using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000855 RID: 2133
	public class SVImageControl : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerClickHandler
	{
		// Token: 0x06003881 RID: 14465 RVA: 0x00149E74 File Offset: 0x00148074
		private void Awake()
		{
			this.SVImage = base.GetComponent<RawImage>();
			this.rect = base.GetComponent<RectTransform>();
			this.pickerTransform = this.PickerImage.GetComponent<RectTransform>();
			if (this.colorPicker == null)
			{
				this.colorPicker = base.GetComponentInParent<ColorPickerControlExtended>(true);
				if (this.colorPicker == null)
				{
					this.colorPicker = Object.FindFirstObjectByType<ColorPickerControlExtended>();
				}
				if (this.colorPicker == null)
				{
					Debug.LogError("SVImageControl: ColorPickerControlExtended non trovato!");
				}
			}
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x00149EF8 File Offset: 0x001480F8
		private void UpdateColor(PointerEventData eventData)
		{
			if (this.rect == null || this.pickerTransform == null || this.colorPicker == null)
			{
				return;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rect, eventData.position, eventData.pressEventCamera, out vector);
			Rect rect = this.rect.rect;
			float num = Mathf.Clamp01((vector.x - rect.xMin) / rect.width);
			float num2 = Mathf.Clamp01((vector.y - rect.yMin) / rect.height);
			this.pickerTransform.localPosition = new Vector3(vector.x, vector.y, this.pickerTransform.localPosition.z);
			this.pickerTransform.localPosition = new Vector3(Mathf.Clamp(this.pickerTransform.localPosition.x, rect.xMin, rect.xMax), Mathf.Clamp(this.pickerTransform.localPosition.y, rect.yMin, rect.yMax), this.pickerTransform.localPosition.z);
			if (this.PickerImage != null)
			{
				this.PickerImage.color = Color.white;
			}
			this.colorPicker.SetSV(num, num2);
			if (this.colorPreviewImage != null && this.colorPicker != null)
			{
				this.colorPreviewImage.color = this.colorPicker.OutputColor;
			}
			Debug.Log(string.Format("SV aggiornati: S={0}, V={1}", num, num2));
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x0014A098 File Offset: 0x00148298
		public void OnDrag(PointerEventData eventData)
		{
			this.UpdateColor(eventData);
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x0014A098 File Offset: 0x00148298
		public void OnPointerClick(PointerEventData eventData)
		{
			this.UpdateColor(eventData);
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x0014A0A4 File Offset: 0x001482A4
		public void UpdatePickerPosition(float saturation, float value)
		{
			if (this.rect == null || this.pickerTransform == null)
			{
				return;
			}
			Rect rect = this.rect.rect;
			float num = Mathf.Lerp(rect.xMin, rect.xMax, saturation);
			float num2 = Mathf.Lerp(rect.yMin, rect.yMax, value);
			this.pickerTransform.localPosition = new Vector3(num, num2, this.pickerTransform.localPosition.z);
			if (this.PickerImage != null)
			{
				this.PickerImage.color = Color.white;
			}
			if (this.colorPreviewImage != null && this.colorPicker != null)
			{
				this.colorPreviewImage.color = this.colorPicker.OutputColor;
			}
		}

		// Token: 0x0400396B RID: 14699
		[SerializeField]
		private Image PickerImage;

		// Token: 0x0400396C RID: 14700
		[Tooltip("Riferimento al ColorPickerControlExtended. Se non specificato, verrà cercato automaticamente.")]
		[SerializeField]
		private ColorPickerControlExtended colorPicker;

		// Token: 0x0400396D RID: 14701
		[Tooltip("Riferimento all'immagine interna che mostra l'anteprima del colore selezionato")]
		[SerializeField]
		private Image colorPreviewImage;

		// Token: 0x0400396E RID: 14702
		private RawImage SVImage;

		// Token: 0x0400396F RID: 14703
		private RectTransform rect;

		// Token: 0x04003970 RID: 14704
		private RectTransform pickerTransform;
	}
}
