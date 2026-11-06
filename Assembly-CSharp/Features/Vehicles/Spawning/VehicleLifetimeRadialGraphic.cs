using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Vehicles.Spawning
{
	// Token: 0x02000B88 RID: 2952
	[RequireComponent(typeof(CanvasRenderer))]
	public sealed class VehicleLifetimeRadialGraphic : MaskableGraphic
	{
		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06005447 RID: 21575 RVA: 0x0020B3A4 File Offset: 0x002095A4
		public float FillAmount
		{
			get
			{
				return this.fillAmount;
			}
		}

		// Token: 0x06005448 RID: 21576 RVA: 0x0020B3AC File Offset: 0x002095AC
		public void SetFill(float normalized)
		{
			float num = Mathf.Clamp01(normalized);
			if (Mathf.Approximately(this.fillAmount, num))
			{
				return;
			}
			this.fillAmount = num;
			this.SetVerticesDirty();
		}

		// Token: 0x06005449 RID: 21577 RVA: 0x0020B3DC File Offset: 0x002095DC
		public void SetInnerRadius(float normalized)
		{
			this.innerRadius = Mathf.Clamp(normalized, 0f, 0.98f);
			this.SetVerticesDirty();
		}

		// Token: 0x0600544A RID: 21578 RVA: 0x0020B3FC File Offset: 0x002095FC
		protected override void OnPopulateMesh(VertexHelper vertexHelper)
		{
			vertexHelper.Clear();
			if (this.fillAmount <= 0f)
			{
				return;
			}
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			float num = Mathf.Max(0f, Mathf.Min(pixelAdjustedRect.width, pixelAdjustedRect.height) * 0.5f);
			float num2 = num * this.innerRadius;
			if (num <= 0f)
			{
				return;
			}
			int num3 = Mathf.Max(1, Mathf.CeilToInt((float)this.segmentCount * this.fillAmount));
			float num4 = 6.2831855f * this.fillAmount;
			Vector2 center = pixelAdjustedRect.center;
			for (int i = 0; i < num3; i++)
			{
				float num5 = (float)i / (float)num3;
				float num6 = (float)(i + 1) / (float)num3;
				float num7 = 1.5707964f - num4 * num5;
				float num8 = 1.5707964f - num4 * num6;
				Vector2 vector = new Vector2(Mathf.Cos(num7), Mathf.Sin(num7));
				Vector2 vector2 = new Vector2(Mathf.Cos(num8), Mathf.Sin(num8));
				int currentVertCount = vertexHelper.currentVertCount;
				vertexHelper.AddVert(this.CreateVertex(center + vector * num2));
				vertexHelper.AddVert(this.CreateVertex(center + vector * num));
				vertexHelper.AddVert(this.CreateVertex(center + vector2 * num));
				vertexHelper.AddVert(this.CreateVertex(center + vector2 * num2));
				vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
				vertexHelper.AddTriangle(currentVertCount, currentVertCount + 2, currentVertCount + 3);
			}
		}

		// Token: 0x0600544B RID: 21579 RVA: 0x0020B58C File Offset: 0x0020978C
		private UIVertex CreateVertex(Vector2 position)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = this.color;
			simpleVert.position = position;
			return simpleVert;
		}

		// Token: 0x0400549C RID: 21660
		[SerializeField]
		[Range(0f, 1f)]
		private float fillAmount = 1f;

		// Token: 0x0400549D RID: 21661
		[SerializeField]
		[Range(0f, 0.98f)]
		private float innerRadius = 0.78f;

		// Token: 0x0400549E RID: 21662
		[SerializeField]
		[Range(12f, 128f)]
		private int segmentCount = 72;
	}
}
