using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x02000139 RID: 313
	public class DebugUIHandlerValueTuple : DebugUIHandlerWidget
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x0002ADD2 File Offset: 0x00028FD2
		protected override void OnEnable()
		{
			this.m_Timer = 0f;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0002ADDF File Offset: 0x00028FDF
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			this.nameLabel.color = this.colorSelected;
			return true;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0002ADF3 File Offset: 0x00028FF3
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0002AE08 File Offset: 0x00029008
		internal override void SetWidget(DebugUI.Widget widget)
		{
			this.m_Widget = widget;
			this.m_Field = base.CastWidget<DebugUI.ValueTuple>();
			this.nameLabel.text = this.m_Field.displayName;
			int numElements = this.m_Field.numElements;
			this.valueElements = new Text[numElements];
			this.valueElements[0] = this.valueLabel;
			float num = 230f / (float)numElements;
			for (int i = 1; i < numElements; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.valueLabel.gameObject, base.transform);
				gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
				RectTransform rectTransform = gameObject.transform as RectTransform;
				RectTransform rectTransform2 = this.nameLabel.transform as RectTransform;
				Vector2 vector = new Vector2(0f, 1f);
				rectTransform.anchorMin = vector;
				rectTransform.anchorMax = vector;
				rectTransform.sizeDelta = new Vector2(100f, 26f);
				Vector3 vector2 = rectTransform2.anchoredPosition;
				vector2.x += (float)(i + 1) * num + 200f;
				rectTransform.anchoredPosition = vector2;
				rectTransform.pivot = new Vector2(0f, 1f);
				this.valueElements[i] = gameObject.GetComponent<Text>();
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002AF4C File Offset: 0x0002914C
		internal virtual void UpdateValueLabels()
		{
			for (int i = 0; i < this.m_Field.numElements; i++)
			{
				if (i < this.valueElements.Length && this.valueElements[i] != null)
				{
					object value = this.m_Field.values[i].GetValue();
					this.valueElements[i].text = this.m_Field.values[i].FormatString(value);
					if (value is float)
					{
						this.valueElements[i].color = (((float)value == 0f) ? DebugUIHandlerValueTuple.k_ZeroColor : this.colorDefault);
					}
				}
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002AFF4 File Offset: 0x000291F4
		private void Update()
		{
			if (this.m_Field != null && this.m_Timer >= this.m_Field.refreshRate)
			{
				this.UpdateValueLabels();
				this.m_Timer -= this.m_Field.refreshRate;
			}
			this.m_Timer += Time.deltaTime;
		}

		// Token: 0x0400056B RID: 1387
		public Text nameLabel;

		// Token: 0x0400056C RID: 1388
		public Text valueLabel;

		// Token: 0x0400056D RID: 1389
		protected internal DebugUI.ValueTuple m_Field;

		// Token: 0x0400056E RID: 1390
		protected internal Text[] valueElements;

		// Token: 0x0400056F RID: 1391
		private const float k_XOffset = 230f;

		// Token: 0x04000570 RID: 1392
		private float m_Timer;

		// Token: 0x04000571 RID: 1393
		private static readonly Color k_ZeroColor = Color.gray;
	}
}
