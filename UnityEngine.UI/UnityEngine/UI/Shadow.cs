using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace UnityEngine.UI
{
	// Token: 0x02000045 RID: 69
	[AddComponentMenu("UI/Effects/Shadow", 80)]
	public class Shadow : BaseMeshEffect
	{
		// Token: 0x060004B8 RID: 1208 RVA: 0x00016C4C File Offset: 0x00014E4C
		protected Shadow()
		{
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00016C9A File Offset: 0x00014E9A
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x00016CA2 File Offset: 0x00014EA2
		public Color effectColor
		{
			get
			{
				return this.m_EffectColor;
			}
			set
			{
				this.m_EffectColor = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00016CC4 File Offset: 0x00014EC4
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x00016CCC File Offset: 0x00014ECC
		public Vector2 effectDistance
		{
			get
			{
				return this.m_EffectDistance;
			}
			set
			{
				if (value.x > 600f)
				{
					value.x = 600f;
				}
				if (value.x < -600f)
				{
					value.x = -600f;
				}
				if (value.y > 600f)
				{
					value.y = 600f;
				}
				if (value.y < -600f)
				{
					value.y = -600f;
				}
				if (this.m_EffectDistance == value)
				{
					return;
				}
				this.m_EffectDistance = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00016D6C File Offset: 0x00014F6C
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x00016D74 File Offset: 0x00014F74
		public bool useGraphicAlpha
		{
			get
			{
				return this.m_UseGraphicAlpha;
			}
			set
			{
				this.m_UseGraphicAlpha = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00016D98 File Offset: 0x00014F98
		protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
		{
			int num = verts.Count + end - start;
			if (verts.Capacity < num)
			{
				verts.Capacity = num;
			}
			for (int i = start; i < end; i++)
			{
				UIVertex uivertex = verts[i];
				verts.Add(uivertex);
				Vector3 position = uivertex.position;
				position.x += x;
				position.y += y;
				uivertex.position = position;
				Color32 color2 = color;
				if (this.m_UseGraphicAlpha)
				{
					color2.a = color2.a * verts[i].color.a / byte.MaxValue;
				}
				uivertex.color = color2;
				verts[i] = uivertex;
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00016E4C File Offset: 0x0001504C
		protected void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
		{
			this.ApplyShadowZeroAlloc(verts, color, start, end, x, y);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00016E60 File Offset: 0x00015060
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = CollectionPool<List<UIVertex>, UIVertex>.Get();
			vh.GetUIVertexStream(list);
			this.ApplyShadow(list, this.effectColor, 0, list.Count, this.effectDistance.x, this.effectDistance.y);
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
			CollectionPool<List<UIVertex>, UIVertex>.Release(list);
		}

		// Token: 0x04000191 RID: 401
		[SerializeField]
		private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x04000192 RID: 402
		[SerializeField]
		private Vector2 m_EffectDistance = new Vector2(1f, -1f);

		// Token: 0x04000193 RID: 403
		[SerializeField]
		private bool m_UseGraphicAlpha = true;

		// Token: 0x04000194 RID: 404
		private const float kMaxEffectDistance = 600f;
	}
}
