using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x020001D0 RID: 464
	public class PieChart : MaskableGraphic
	{
		// Token: 0x06000DA9 RID: 3497 RVA: 0x0005E215 File Offset: 0x0005C415
		protected override void Awake()
		{
			base.Awake();
			this.UpdateIndicators();
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0005E224 File Offset: 0x0005C424
		private void Update()
		{
			this.borderThickness = Mathf.Clamp(this.borderThickness, -75f, base.rectTransform.rect.width / 3.333f);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0005E264 File Offset: 0x0005C464
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (this.chartData.Count == 0)
			{
				return;
			}
			float num = -base.rectTransform.pivot.x * base.rectTransform.rect.width;
			float num2 = -base.rectTransform.pivot.x * base.rectTransform.rect.width + this.borderThickness;
			float num3 = -base.rectTransform.pivot.x * base.rectTransform.rect.width * 0.6f;
			float num4 = -base.rectTransform.pivot.x * base.rectTransform.rect.width * 0.6f + this.borderThickness;
			vh.Clear();
			Vector2 vector = Vector2.zero;
			Vector2 vector2 = Vector2.zero;
			Vector2 vector3 = new Vector2(0f, 0f);
			Vector2 vector4 = new Vector2(0f, 1f);
			Vector2 vector5 = new Vector2(1f, 1f);
			Vector2 vector6 = new Vector2(1f, 0f);
			float num5 = this.fillAmount;
			float num6 = 360f / (float)this.segments;
			int num7 = (int)((float)(this.segments + 1) * num5);
			int num8 = 0;
			float total = 0f;
			float num9 = this.chartData[0].value;
			this.chartData.ForEach(delegate(PieChart.PieChartDataNode s)
			{
				total += s.value;
			});
			Color32 color = this.chartData[0].color;
			for (int i = 0; i < num7; i++)
			{
				float num10 = 0.017453292f * ((float)i * num6);
				float num11 = Mathf.Cos(num10);
				float num12 = Mathf.Sin(num10);
				vector3 = new Vector2(0f, 1f);
				vector4 = new Vector2(1f, 1f);
				vector5 = new Vector2(1f, 0f);
				vector6 = new Vector2(0f, 0f);
				Vector2 vector7 = vector;
				Vector2 vector8 = new Vector2(num * num11, num * num12);
				Vector2 vector9 = new Vector2(num2 * num11, num2 * num12);
				Vector2 vector10 = vector2;
				if ((float)i > num9 / total * (float)this.segments && num8 < this.chartData.Count - 1)
				{
					num8++;
					num9 += this.chartData[num8].value;
					color = this.chartData[num8].color;
				}
				vh.AddUIVertexQuad(this.SetVbo(new Vector2[]
				{
					vector7,
					vector8,
					vector9 * num4 / num2,
					vector10 * num4 / num2
				}, new Vector2[] { vector3, vector4, vector5, vector6 }, color));
				if (this.enableBorderColor)
				{
					vh.AddUIVertexQuad(this.SetVbo(new Vector2[] { vector7, vector8, vector9, vector10 }, new Vector2[] { vector3, vector4, vector5, vector6 }, this.borderColor));
					vh.AddUIVertexQuad(this.SetVbo(new Vector2[]
					{
						vector7 * num3 / num,
						vector8 * num3 / num,
						vector9 * num4 / num2,
						vector10 * num4 / num2
					}, new Vector2[] { vector3, vector4, vector5, vector6 }, this.borderColor));
				}
				vector = vector8;
				vector2 = vector9;
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0005E690 File Offset: 0x0005C890
		public void SetData(List<PieChart.PieChartDataNode> data)
		{
			this.chartData = data;
			this.SetVerticesDirty();
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0005E6A0 File Offset: 0x0005C8A0
		protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs, Color32 color)
		{
			UIVertex[] array = new UIVertex[4];
			for (int i = 0; i < vertices.Length; i++)
			{
				UIVertex simpleVert = UIVertex.simpleVert;
				simpleVert.color = color;
				simpleVert.position = vertices[i];
				simpleVert.uv0 = uvs[i];
				array[i] = simpleVert;
			}
			return array;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0005E700 File Offset: 0x0005C900
		public void UpdateIndicators()
		{
			for (int i = 0; i < this.chartData.Count; i++)
			{
				if (this.chartData[i].indicatorImage != null)
				{
					this.chartData[i].indicatorImage.color = this.chartData[i].color;
				}
				if (this.chartData[i].indicatorText != null && this.addValueToIndicator)
				{
					this.chartData[i].indicatorText.text = this.chartData[i].name + this.valuePrefix + this.chartData[i].value.ToString() + this.valueSuffix;
				}
				else if (this.chartData[i].indicatorText != null && !this.addValueToIndicator)
				{
					this.chartData[i].indicatorText.text = this.chartData[i].name;
				}
			}
			if (this.indicatorParent != null)
			{
				base.StartCoroutine("UpdateIndicatorLayout");
			}
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0005E841 File Offset: 0x0005CA41
		public void ChangeValue(int itemIndex, float itemValue)
		{
			this.chartData[itemIndex].value = itemValue;
			base.enabled = false;
			base.enabled = true;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0005E864 File Offset: 0x0005CA64
		public void AddNewItem()
		{
			PieChart.PieChartDataNode pieChartDataNode = new PieChart.PieChartDataNode();
			if (this.indicatorParent.childCount != 0)
			{
				int num = this.indicatorParent.childCount - 1;
				GameObject gameObject = Object.Instantiate<GameObject>(this.indicatorParent.GetChild(num).gameObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.indicatorParent, false);
				gameObject.gameObject.name = "Item " + num.ToString() + " Indicator";
				pieChartDataNode.indicatorImage = gameObject.GetComponentInChildren<Image>();
				pieChartDataNode.indicatorText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
				pieChartDataNode.name = "Chart Item " + num.ToString();
			}
			this.chartData.Add(pieChartDataNode);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0005E933 File Offset: 0x0005CB33
		private IEnumerator UpdateIndicatorLayout()
		{
			yield return new WaitForSeconds(0.1f);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.indicatorParent.GetComponentInParent<RectTransform>());
			yield break;
		}

		// Token: 0x04000E16 RID: 3606
		[SerializeField]
		public List<PieChart.PieChartDataNode> chartData = new List<PieChart.PieChartDataNode>();

		// Token: 0x04000E17 RID: 3607
		[Range(-75f, 150f)]
		public float borderThickness = 5f;

		// Token: 0x04000E18 RID: 3608
		[SerializeField]
		private Color borderColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000E19 RID: 3609
		public Transform indicatorParent;

		// Token: 0x04000E1A RID: 3610
		public string valuePrefix = "(";

		// Token: 0x04000E1B RID: 3611
		public string valueSuffix = ")";

		// Token: 0x04000E1C RID: 3612
		public bool addValueToIndicator = true;

		// Token: 0x04000E1D RID: 3613
		public bool enableBorderColor;

		// Token: 0x04000E1E RID: 3614
		private float fillAmount = 1f;

		// Token: 0x04000E1F RID: 3615
		private int segments = 720;

		// Token: 0x020001D1 RID: 465
		[Serializable]
		public class PieChartDataNode
		{
			// Token: 0x04000E20 RID: 3616
			public string name = "Chart Item";

			// Token: 0x04000E21 RID: 3617
			public float value = 10f;

			// Token: 0x04000E22 RID: 3618
			public Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

			// Token: 0x04000E23 RID: 3619
			public Image indicatorImage;

			// Token: 0x04000E24 RID: 3620
			public TextMeshProUGUI indicatorText;
		}
	}
}
