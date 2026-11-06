using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.BodyPaintStudio.UI
{
	// Token: 0x02001519 RID: 5401
	public class BodyPaintColorWheel : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x14000120 RID: 288
		// (add) Token: 0x0600A6F5 RID: 42741 RVA: 0x0045CB28 File Offset: 0x0045AD28
		// (remove) Token: 0x0600A6F6 RID: 42742 RVA: 0x0045CB60 File Offset: 0x0045AD60
		public event Action<float, float> OnHueSatChanged;

		// Token: 0x0600A6F7 RID: 42743 RVA: 0x0045CB95 File Offset: 0x0045AD95
		private void Awake()
		{
			this._rect = ((this.wheelImage != null) ? this.wheelImage.rectTransform : base.GetComponent<RectTransform>());
			this.GenerateWheelTexture();
		}

		// Token: 0x0600A6F8 RID: 42744 RVA: 0x0045CBC4 File Offset: 0x0045ADC4
		private void OnDestroy()
		{
			if (this._wheelTexture != null)
			{
				Object.Destroy(this._wheelTexture);
			}
		}

		// Token: 0x0600A6F9 RID: 42745 RVA: 0x0045CBE0 File Offset: 0x0045ADE0
		public void SetValue(float v)
		{
			this._value = Mathf.Clamp01(v);
			if (this.wheelImage != null)
			{
				this.wheelImage.color = new Color(this._value, this._value, this._value, 1f);
			}
		}

		// Token: 0x0600A6FA RID: 42746 RVA: 0x0045CC30 File Offset: 0x0045AE30
		public void SetKnobFromHueSat(float hue, float sat)
		{
			if (this.knob == null || this._rect == null)
			{
				return;
			}
			float num = hue * 3.1415927f * 2f;
			float num2 = sat * this._rect.rect.width * 0.5f;
			this.knob.anchoredPosition = new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * num2;
		}

		// Token: 0x0600A6FB RID: 42747 RVA: 0x0045CCA8 File Offset: 0x0045AEA8
		private void GenerateWheelTexture()
		{
			if (this.wheelImage == null)
			{
				return;
			}
			this._wheelTexture = new Texture2D(this.textureSize, this.textureSize, TextureFormat.RGBA32, false)
			{
				name = "BodyPaintColorWheel",
				wrapMode = TextureWrapMode.Clamp
			};
			float num = (float)this.textureSize * 0.5f;
			Color32[] array = new Color32[this.textureSize * this.textureSize];
			for (int i = 0; i < this.textureSize; i++)
			{
				for (int j = 0; j < this.textureSize; j++)
				{
					float num2 = ((float)j - num) / num;
					float num3 = ((float)i - num) / num;
					float num4 = Mathf.Sqrt(num2 * num2 + num3 * num3);
					if (num4 > 1f)
					{
						array[i * this.textureSize + j] = new Color32(0, 0, 0, 0);
					}
					else
					{
						float num5 = Mathf.Atan2(num3, num2) / 6.2831855f;
						if (num5 < 0f)
						{
							num5 += 1f;
						}
						Color color = Color.HSVToRGB(num5, Mathf.Clamp01(num4), 1f);
						color.a = Mathf.Clamp01((1f - num4) * num);
						array[i * this.textureSize + j] = color;
					}
				}
			}
			this._wheelTexture.SetPixels32(array);
			this._wheelTexture.Apply(false);
			this.wheelImage.texture = this._wheelTexture;
		}

		// Token: 0x0600A6FC RID: 42748 RVA: 0x0045CE13 File Offset: 0x0045B013
		public void OnPointerDown(PointerEventData eventData)
		{
			this.HandlePointer(eventData);
		}

		// Token: 0x0600A6FD RID: 42749 RVA: 0x0045CE13 File Offset: 0x0045B013
		public void OnDrag(PointerEventData eventData)
		{
			this.HandlePointer(eventData);
		}

		// Token: 0x0600A6FE RID: 42750 RVA: 0x0045CE1C File Offset: 0x0045B01C
		private void HandlePointer(PointerEventData eventData)
		{
			if (this._rect == null)
			{
				return;
			}
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._rect, eventData.position, eventData.pressEventCamera, out vector))
			{
				return;
			}
			float num = this._rect.rect.width * 0.5f;
			Vector2 vector2 = vector / num;
			float num2 = Mathf.Clamp01(vector2.magnitude);
			float num3 = Mathf.Atan2(vector2.y, vector2.x) / 6.2831855f;
			if (num3 < 0f)
			{
				num3 += 1f;
			}
			if (this.knob != null)
			{
				this.knob.anchoredPosition = vector2.normalized * (num2 * num);
			}
			Action<float, float> onHueSatChanged = this.OnHueSatChanged;
			if (onHueSatChanged == null)
			{
				return;
			}
			onHueSatChanged(num3, num2);
		}

		// Token: 0x0400ABC3 RID: 43971
		[SerializeField]
		private RawImage wheelImage;

		// Token: 0x0400ABC4 RID: 43972
		[SerializeField]
		private RectTransform knob;

		// Token: 0x0400ABC5 RID: 43973
		[SerializeField]
		private int textureSize = 256;

		// Token: 0x0400ABC7 RID: 43975
		private Texture2D _wheelTexture;

		// Token: 0x0400ABC8 RID: 43976
		private RectTransform _rect;

		// Token: 0x0400ABC9 RID: 43977
		private float _value = 1f;
	}
}
