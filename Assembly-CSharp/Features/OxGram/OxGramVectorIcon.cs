using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.OxGram
{
	// Token: 0x02000EB8 RID: 3768
	[RequireComponent(typeof(CanvasRenderer))]
	public sealed class OxGramVectorIcon : MaskableGraphic
	{
		// Token: 0x060070F8 RID: 28920 RVA: 0x002DDD78 File Offset: 0x002DBF78
		protected override void OnPopulateMesh(VertexHelper mesh)
		{
			mesh.Clear();
			switch (this.symbol)
			{
			case OxGramVectorIcon.Symbol.Search:
				this.Circle(mesh, new Vector2(0.43f, 0.57f), 0.26f);
				this.Line(mesh, new Vector2(0.63f, 0.37f), new Vector2(0.85f, 0.15f));
				return;
			case OxGramVectorIcon.Symbol.Star:
			{
				Vector2[] array = new Vector2[10];
				for (int i = 0; i < array.Length; i++)
				{
					float num = (90f + (float)i * 36f) * 0.017453292f;
					float num2 = ((i % 2 == 0) ? 0.4f : 0.19f);
					array[i] = new Vector2(0.5f + Mathf.Cos(num) * num2, 0.5f + Mathf.Sin(num) * num2);
				}
				this.Path(mesh, array, true);
				return;
			}
			case OxGramVectorIcon.Symbol.Shield:
				this.Path(mesh, new Vector2[]
				{
					new Vector2(0.5f, 0.9f),
					new Vector2(0.82f, 0.77f),
					new Vector2(0.78f, 0.38f),
					new Vector2(0.66f, 0.21f),
					new Vector2(0.5f, 0.1f),
					new Vector2(0.34f, 0.21f),
					new Vector2(0.22f, 0.38f),
					new Vector2(0.18f, 0.77f)
				}, true);
				this.Path(mesh, new Vector2[]
				{
					new Vector2(0.34f, 0.51f),
					new Vector2(0.46f, 0.39f),
					new Vector2(0.68f, 0.63f)
				}, false);
				return;
			case OxGramVectorIcon.Symbol.Chevron:
				this.Path(mesh, new Vector2[]
				{
					new Vector2(0.36f, 0.78f),
					new Vector2(0.64f, 0.5f),
					new Vector2(0.36f, 0.22f)
				}, false);
				return;
			case OxGramVectorIcon.Symbol.Close:
				this.Line(mesh, new Vector2(0.27f, 0.27f), new Vector2(0.73f, 0.73f));
				this.Line(mesh, new Vector2(0.27f, 0.73f), new Vector2(0.73f, 0.27f));
				return;
			case OxGramVectorIcon.Symbol.Paperclip:
				this.ClipLine(mesh, new Vector2(0.75f, 0.4f), new Vector2(0.75f, 0.68f));
				this.ClipArc(mesh, new Vector2(0.5f, 0.68f), 0.25f, 0f, 180f);
				this.ClipLine(mesh, new Vector2(0.25f, 0.68f), new Vector2(0.25f, 0.26f));
				this.ClipArc(mesh, new Vector2(0.43f, 0.26f), 0.18f, 180f, 360f);
				this.ClipLine(mesh, new Vector2(0.61f, 0.26f), new Vector2(0.61f, 0.67f));
				this.ClipArc(mesh, new Vector2(0.5f, 0.67f), 0.11f, 0f, 180f);
				this.ClipLine(mesh, new Vector2(0.39f, 0.67f), new Vector2(0.39f, 0.28f));
				return;
			case OxGramVectorIcon.Symbol.Back:
				this.Path(mesh, new Vector2[]
				{
					new Vector2(0.66f, 0.82f),
					new Vector2(0.34f, 0.5f),
					new Vector2(0.66f, 0.18f)
				}, false);
				return;
			case OxGramVectorIcon.Symbol.Info:
				this.Circle(mesh, new Vector2(0.5f, 0.5f), 0.38f);
				this.Line(mesh, new Vector2(0.5f, 0.43f), new Vector2(0.5f, 0.26f));
				this.Line(mesh, new Vector2(0.5f, 0.65f), new Vector2(0.5f, 0.68f));
				return;
			case OxGramVectorIcon.Symbol.Check:
				this.Path(mesh, new Vector2[]
				{
					new Vector2(0.2f, 0.5f),
					new Vector2(0.42f, 0.28f),
					new Vector2(0.82f, 0.76f)
				}, false);
				return;
			case OxGramVectorIcon.Symbol.Edit:
				this.Path(mesh, new Vector2[]
				{
					new Vector2(0.2f, 0.2f),
					new Vector2(0.26f, 0.44f),
					new Vector2(0.69f, 0.87f),
					new Vector2(0.87f, 0.69f),
					new Vector2(0.44f, 0.26f)
				}, true);
				this.Line(mesh, new Vector2(0.6f, 0.78f), new Vector2(0.78f, 0.6f));
				return;
			}
			this.Path(mesh, new Vector2[]
			{
				new Vector2(0.2f, 0.8f),
				new Vector2(0.8f, 0.8f),
				new Vector2(0.8f, 0.3f),
				new Vector2(0.46f, 0.3f),
				new Vector2(0.24f, 0.13f),
				new Vector2(0.24f, 0.3f),
				new Vector2(0.2f, 0.3f)
			}, true);
			this.Line(mesh, new Vector2(0.35f, 0.61f), new Vector2(0.65f, 0.61f));
			this.Line(mesh, new Vector2(0.35f, 0.46f), new Vector2(0.54f, 0.46f));
		}

		// Token: 0x060070F9 RID: 28921 RVA: 0x002DE3C8 File Offset: 0x002DC5C8
		private static Vector2 TiltClip(Vector2 point)
		{
			point -= new Vector2(0.5f, 0.5f);
			return new Vector2(point.x * Mathf.Cos(-0.61086524f) - point.y * Mathf.Sin(-0.61086524f), point.x * Mathf.Sin(-0.61086524f) + point.y * Mathf.Cos(-0.61086524f)) + new Vector2(0.5f, 0.5f);
		}

		// Token: 0x060070FA RID: 28922 RVA: 0x002DE44B File Offset: 0x002DC64B
		private void ClipLine(VertexHelper mesh, Vector2 from, Vector2 to)
		{
			this.Line(mesh, OxGramVectorIcon.TiltClip(from), OxGramVectorIcon.TiltClip(to));
		}

		// Token: 0x060070FB RID: 28923 RVA: 0x002DE460 File Offset: 0x002DC660
		private void ClipArc(VertexHelper mesh, Vector2 center, float radius, float from, float to)
		{
			for (int i = 0; i < 18; i++)
			{
				float num = Mathf.Lerp(from, to, (float)i / 18f) * 0.017453292f;
				float num2 = Mathf.Lerp(from, to, ((float)i + 1f) / 18f) * 0.017453292f;
				this.ClipLine(mesh, center + new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * radius, center + new Vector2(Mathf.Cos(num2), Mathf.Sin(num2)) * radius);
			}
		}

		// Token: 0x060070FC RID: 28924 RVA: 0x002DE4F4 File Offset: 0x002DC6F4
		private void Circle(VertexHelper mesh, Vector2 center, float radius)
		{
			for (int i = 0; i < 28; i++)
			{
				float num = (float)i * 3.1415927f * 2f / 28f;
				float num2 = (float)(i + 1) * 3.1415927f * 2f / 28f;
				this.Line(mesh, center + new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * radius, center + new Vector2(Mathf.Cos(num2), Mathf.Sin(num2)) * radius);
			}
		}

		// Token: 0x060070FD RID: 28925 RVA: 0x002DE57C File Offset: 0x002DC77C
		private void Path(VertexHelper mesh, Vector2[] points, bool closed)
		{
			for (int i = 1; i < points.Length; i++)
			{
				this.Line(mesh, points[i - 1], points[i]);
			}
			if (closed)
			{
				this.Line(mesh, points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x060070FE RID: 28926 RVA: 0x002DE5CC File Offset: 0x002DC7CC
		private void Line(VertexHelper mesh, Vector2 from, Vector2 to)
		{
			Rect rect = base.rectTransform.rect;
			from = rect.min + Vector2.Scale(from, rect.size);
			to = rect.min + Vector2.Scale(to, rect.size);
			Vector2 vector = to - from;
			Vector2 vector2 = new Vector2(-vector.y, vector.x).normalized * Mathf.Min(rect.width, rect.height) * 0.0375f;
			int currentVertCount = mesh.currentVertCount;
			mesh.AddVert(from - vector2, this.color, Vector2.zero);
			mesh.AddVert(from + vector2, this.color, Vector2.zero);
			mesh.AddVert(to + vector2, this.color, Vector2.zero);
			mesh.AddVert(to - vector2, this.color, Vector2.zero);
			mesh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			mesh.AddTriangle(currentVertCount, currentVertCount + 2, currentVertCount + 3);
		}

		// Token: 0x040073B3 RID: 29619
		public OxGramVectorIcon.Symbol symbol;

		// Token: 0x02000EB9 RID: 3769
		public enum Symbol
		{
			// Token: 0x040073B5 RID: 29621
			Search,
			// Token: 0x040073B6 RID: 29622
			Star,
			// Token: 0x040073B7 RID: 29623
			Shield,
			// Token: 0x040073B8 RID: 29624
			Chevron,
			// Token: 0x040073B9 RID: 29625
			Chat,
			// Token: 0x040073BA RID: 29626
			Close,
			// Token: 0x040073BB RID: 29627
			Paperclip,
			// Token: 0x040073BC RID: 29628
			Back,
			// Token: 0x040073BD RID: 29629
			Info,
			// Token: 0x040073BE RID: 29630
			Check,
			// Token: 0x040073BF RID: 29631
			Edit
		}
	}
}
