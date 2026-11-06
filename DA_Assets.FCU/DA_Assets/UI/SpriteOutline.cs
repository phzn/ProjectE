using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.UI
{
	// Token: 0x0200000A RID: 10
	[RequireComponent(typeof(CanvasRenderer))]
	public class SpriteOutline : MaskableGraphic
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000024C1 File Offset: 0x000006C1
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000024C9 File Offset: 0x000006C9
		public float OutlineWidth
		{
			get
			{
				return this._outlineWidth;
			}
			set
			{
				this._outlineWidth = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000024D2 File Offset: 0x000006D2
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000024DA File Offset: 0x000006DA
		public float CornerRadius
		{
			get
			{
				return this._cornerRadius;
			}
			set
			{
				this._cornerRadius = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000024E3 File Offset: 0x000006E3
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000024EB File Offset: 0x000006EB
		public int CornerSegments
		{
			get
			{
				return this._cornerSegments;
			}
			set
			{
				this._cornerSegments = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000024F4 File Offset: 0x000006F4
		// (set) Token: 0x06000038 RID: 56 RVA: 0x000024FC File Offset: 0x000006FC
		public float MappingBias
		{
			get
			{
				return this._mappingBias;
			}
			set
			{
				this._mappingBias = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002505 File Offset: 0x00000705
		// (set) Token: 0x0600003A RID: 58 RVA: 0x0000250D File Offset: 0x0000070D
		public bool FillCenter
		{
			get
			{
				return this._fillCenter;
			}
			set
			{
				this._fillCenter = value;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002516 File Offset: 0x00000716
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			this.SetVerticesDirty();
			this.SetMaterialDirty();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000252C File Offset: 0x0000072C
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			Rect rect = base.rectTransform.rect;
			float num = Mathf.Min(Mathf.Min(rect.width, rect.height) / 2f, this._cornerRadius);
			base.rectTransform.GetLocalCorners(this._corners);
			this._corners[0] += new Vector3(num, num, 0f);
			this._corners[1] += new Vector3(num, -num, 0f);
			this._corners[2] += new Vector3(-num, -num, 0f);
			this._corners[3] += new Vector3(-num, num, 0f);
			float num2 = this._corners[1].y - this._corners[0].y;
			float num3 = this._corners[2].x - this._corners[1].x;
			float[] array = new float[] { num2, num3, num2, num3 };
			float num4 = 6.2831855f * Mathf.Lerp(num, num + this._outlineWidth, this._mappingBias);
			float num5 = num2 * 2f + num3 * 2f + num4;
			float num6 = num4 / 4f / (float)this._cornerSegments;
			UIVertex uivertex = new UIVertex
			{
				color = this.color
			};
			this._verts.Clear();
			float num7 = 0f;
			for (int i = 0; i < 4; i++)
			{
				Vector3 vector = this._corners[i];
				for (int j = 0; j < this._cornerSegments + 1; j++)
				{
					float num8 = (float)j / (float)this._cornerSegments * 3.1415927f / 2f + 1.5707964f - 3.1415927f * (float)i * 1.5f;
					Vector3 vector2 = new Vector3(Mathf.Cos(-num8), Mathf.Sin(-num8), 0f);
					uivertex.position = vector + vector2 * num;
					uivertex.uv0 = new Vector2(num7, 0f);
					this._verts.Add(uivertex);
					uivertex.position = vector + vector2 * (num + this._outlineWidth);
					uivertex.uv0 = new Vector2(num7, 1f);
					this._verts.Add(uivertex);
					if (this._fillCenter)
					{
						uivertex.position = rect.center;
						uivertex.uv0 = new Vector2(num7, 0f);
						this._verts.Add(uivertex);
					}
					if (j < this._cornerSegments)
					{
						num7 += num6 / num5;
					}
					else
					{
						num7 += array[i] / num5;
					}
				}
			}
			uivertex = this._verts[0];
			uivertex.uv0 = new Vector2(1f, 0f);
			this._verts.Add(uivertex);
			uivertex = this._verts[1];
			uivertex.uv0 = new Vector2(1f, 1f);
			this._verts.Add(uivertex);
			if (this._fillCenter)
			{
				uivertex = this._verts[2];
				uivertex.uv0 = new Vector2(1f, 1f);
				this._verts.Add(uivertex);
			}
			foreach (UIVertex uivertex2 in this._verts)
			{
				vh.AddVert(uivertex2);
			}
			if (this._fillCenter)
			{
				for (int k = 0; k < vh.currentVertCount - 3; k += 3)
				{
					vh.AddTriangle(k, k + 1, k + 4);
					vh.AddTriangle(k, k + 4, k + 3);
					vh.AddTriangle(k + 2, k, k + 3);
					vh.AddTriangle(k + 2, k + 3, k + 5);
				}
				return;
			}
			for (int l = 0; l < vh.currentVertCount - 2; l += 2)
			{
				vh.AddTriangle(l, l + 1, l + 3);
				vh.AddTriangle(l, l + 3, l + 2);
			}
		}

		// Token: 0x04000018 RID: 24
		[SerializeField]
		[Range(0f, 500f)]
		private float _outlineWidth = 100f;

		// Token: 0x04000019 RID: 25
		[SerializeField]
		[Range(0f, 500f)]
		private float _cornerRadius = 50f;

		// Token: 0x0400001A RID: 26
		[SerializeField]
		[Range(1f, 20f)]
		private int _cornerSegments = 10;

		// Token: 0x0400001B RID: 27
		[SerializeField]
		[Range(0f, 1f)]
		private float _mappingBias = 0.5f;

		// Token: 0x0400001C RID: 28
		[SerializeField]
		private bool _fillCenter;

		// Token: 0x0400001D RID: 29
		private Vector3[] _corners = new Vector3[4];

		// Token: 0x0400001E RID: 30
		private List<UIVertex> _verts = new List<UIVertex>();
	}
}
