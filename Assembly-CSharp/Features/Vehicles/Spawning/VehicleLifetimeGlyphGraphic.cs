using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Vehicles.Spawning
{
	// Token: 0x02000B86 RID: 2950
	[RequireComponent(typeof(CanvasRenderer))]
	public sealed class VehicleLifetimeGlyphGraphic : MaskableGraphic
	{
		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06005413 RID: 21523 RVA: 0x00209D7D File Offset: 0x00207F7D
		public VehicleLifetimeGlyph Glyph
		{
			get
			{
				return this.glyph;
			}
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x00209D85 File Offset: 0x00207F85
		public void SetGlyph(VehicleLifetimeGlyph value)
		{
			if (this.glyph == value)
			{
				return;
			}
			this.glyph = value;
			this.SetVerticesDirty();
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x00209DA0 File Offset: 0x00207FA0
		protected override void OnPopulateMesh(VertexHelper vertexHelper)
		{
			vertexHelper.Clear();
			switch (this.glyph)
			{
			case VehicleLifetimeGlyph.Bicycle:
				this.DrawBicycle(vertexHelper);
				return;
			case VehicleLifetimeGlyph.Scooter:
				this.DrawScooter(vertexHelper);
				return;
			case VehicleLifetimeGlyph.KickScooter:
				this.DrawKickScooter(vertexHelper);
				return;
			case VehicleLifetimeGlyph.Motorbike:
				this.DrawMotorbike(vertexHelper);
				return;
			case VehicleLifetimeGlyph.RollerSkates:
				this.DrawRollerSkates(vertexHelper);
				return;
			case VehicleLifetimeGlyph.Unicycle:
				this.DrawUnicycle(vertexHelper);
				return;
			case VehicleLifetimeGlyph.Skateboard:
				this.DrawSkateboard(vertexHelper);
				return;
			default:
				this.DrawGenericWheel(vertexHelper);
				return;
			}
		}

		// Token: 0x06005416 RID: 21526 RVA: 0x00209E20 File Offset: 0x00208020
		private void DrawBicycle(VertexHelper vh)
		{
			this.AddRing(vh, new Vector2(-27f, -14f), 14f, 4f, 20);
			this.AddRing(vh, new Vector2(27f, -14f), 14f, 4f, 20);
			this.AddLine(vh, new Vector2(-27f, -14f), new Vector2(-5f, -14f), 4f);
			this.AddLine(vh, new Vector2(-5f, -14f), new Vector2(-15f, 8f), 4f);
			this.AddLine(vh, new Vector2(-15f, 8f), new Vector2(8f, 8f), 4f);
			this.AddLine(vh, new Vector2(8f, 8f), new Vector2(-5f, -14f), 4f);
			this.AddLine(vh, new Vector2(8f, 8f), new Vector2(27f, -14f), 4f);
			this.AddLine(vh, new Vector2(8f, 8f), new Vector2(15f, 23f), 4f);
			this.AddLine(vh, new Vector2(10f, 23f), new Vector2(23f, 23f), 4f);
			this.AddLine(vh, new Vector2(-21f, 11f), new Vector2(-10f, 11f), 4f);
		}

		// Token: 0x06005417 RID: 21527 RVA: 0x00209FC4 File Offset: 0x002081C4
		private void DrawScooter(VertexHelper vh)
		{
			this.AddRing(vh, new Vector2(-25f, -15f), 12f, 4f, 20);
			this.AddRing(vh, new Vector2(25f, -15f), 12f, 4f, 20);
			this.AddLine(vh, new Vector2(-25f, -15f), new Vector2(-10f, 2f), 7f);
			this.AddLine(vh, new Vector2(-10f, 2f), new Vector2(15f, 0f), 12f);
			this.AddLine(vh, new Vector2(15f, 0f), new Vector2(25f, -15f), 6f);
			this.AddLine(vh, new Vector2(-12f, 9f), new Vector2(5f, 9f), 6f);
			this.AddLine(vh, new Vector2(13f, 2f), new Vector2(19f, 24f), 5f);
			this.AddLine(vh, new Vector2(14f, 24f), new Vector2(28f, 21f), 4f);
		}

		// Token: 0x06005418 RID: 21528 RVA: 0x0020A114 File Offset: 0x00208314
		private void DrawKickScooter(VertexHelper vh)
		{
			this.AddRing(vh, new Vector2(-28f, -18f), 8f, 3.5f, 16);
			this.AddRing(vh, new Vector2(28f, -18f), 8f, 3.5f, 16);
			this.AddLine(vh, new Vector2(-25f, -10f), new Vector2(22f, -10f), 6f);
			this.AddLine(vh, new Vector2(22f, -10f), new Vector2(17f, 24f), 5f);
			this.AddLine(vh, new Vector2(8f, 24f), new Vector2(26f, 24f), 5f);
		}

		// Token: 0x06005419 RID: 21529 RVA: 0x0020A1E4 File Offset: 0x002083E4
		private void DrawMotorbike(VertexHelper vh)
		{
			this.AddRing(vh, new Vector2(-27f, -15f), 13f, 4f, 20);
			this.AddRing(vh, new Vector2(27f, -15f), 13f, 4f, 20);
			this.AddLine(vh, new Vector2(-27f, -15f), new Vector2(-8f, 4f), 6f);
			this.AddLine(vh, new Vector2(-8f, 4f), new Vector2(13f, 2f), 14f);
			this.AddLine(vh, new Vector2(13f, 2f), new Vector2(27f, -15f), 6f);
			this.AddLine(vh, new Vector2(-14f, 14f), new Vector2(3f, 14f), 7f);
			this.AddLine(vh, new Vector2(12f, 6f), new Vector2(19f, 23f), 5f);
			this.AddLine(vh, new Vector2(14f, 23f), new Vector2(28f, 20f), 4f);
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x0020A334 File Offset: 0x00208534
		private void DrawRollerSkates(VertexHelper vh)
		{
			this.AddLine(vh, new Vector2(-27f, 20f), new Vector2(2f, 20f), 7f);
			this.AddLine(vh, new Vector2(2f, 20f), new Vector2(20f, 2f), 7f);
			this.AddLine(vh, new Vector2(20f, 2f), new Vector2(27f, -10f), 7f);
			this.AddLine(vh, new Vector2(27f, -10f), new Vector2(-29f, -10f), 7f);
			this.AddLine(vh, new Vector2(-29f, -10f), new Vector2(-27f, 20f), 7f);
			this.AddLine(vh, new Vector2(-27f, -18f), new Vector2(24f, -18f), 4f);
			this.AddDisc(vh, new Vector2(-20f, -25f), 6f, 12);
			this.AddDisc(vh, new Vector2(-4f, -25f), 6f, 12);
			this.AddDisc(vh, new Vector2(12f, -25f), 6f, 12);
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x0020A494 File Offset: 0x00208694
		private void DrawUnicycle(VertexHelper vh)
		{
			this.AddRing(vh, new Vector2(0f, -13f), 23f, 5f, 28);
			this.AddLine(vh, new Vector2(0f, -13f), new Vector2(0f, 24f), 5f);
			this.AddLine(vh, new Vector2(-13f, 24f), new Vector2(13f, 24f), 7f);
			this.AddLine(vh, new Vector2(-9f, -13f), new Vector2(9f, -13f), 4f);
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x0020A544 File Offset: 0x00208744
		private void DrawSkateboard(VertexHelper vh)
		{
			this.AddLine(vh, new Vector2(-34f, 4f), new Vector2(-27f, -4f), 5f);
			this.AddLine(vh, new Vector2(-27f, -4f), new Vector2(27f, -4f), 6f);
			this.AddLine(vh, new Vector2(27f, -4f), new Vector2(34f, 4f), 5f);
			this.AddDisc(vh, new Vector2(-20f, -14f), 6f, 12);
			this.AddDisc(vh, new Vector2(20f, -14f), 6f, 12);
			this.AddLine(vh, new Vector2(-24f, 10f), new Vector2(24f, 10f), 3f);
		}

		// Token: 0x0600541D RID: 21533 RVA: 0x0020A634 File Offset: 0x00208834
		private void DrawGenericWheel(VertexHelper vh)
		{
			this.AddRing(vh, Vector2.zero, 27f, 6f, 28);
			this.AddDisc(vh, Vector2.zero, 7f, 16);
			this.AddLine(vh, Vector2.zero, new Vector2(0f, 27f), 5f);
			this.AddLine(vh, Vector2.zero, new Vector2(-23f, -14f), 5f);
			this.AddLine(vh, Vector2.zero, new Vector2(23f, -14f), 5f);
		}

		// Token: 0x0600541E RID: 21534 RVA: 0x0020A6CC File Offset: 0x002088CC
		private void AddLine(VertexHelper vh, Vector2 from, Vector2 to, float width)
		{
			Vector2 vector = this.ToLocal(from);
			Vector2 vector2 = this.ToLocal(to);
			Vector2 vector3 = vector2 - vector;
			if (vector3.sqrMagnitude < 0.0001f)
			{
				return;
			}
			float num = width * this.ResolveScale();
			Vector2 vector4 = new Vector2(-vector3.y, vector3.x).normalized * (num * 0.5f);
			this.AddQuad(vh, vector - vector4, vector + vector4, vector2 + vector4, vector2 - vector4);
		}

		// Token: 0x0600541F RID: 21535 RVA: 0x0020A75C File Offset: 0x0020895C
		private void AddRing(VertexHelper vh, Vector2 center, float radius, float width, int segments)
		{
			float num = width * 0.5f;
			for (int i = 0; i < segments; i++)
			{
				float num2 = 6.2831855f * (float)i / (float)segments;
				float num3 = 6.2831855f * (float)(i + 1) / (float)segments;
				Vector2 vector = new Vector2(Mathf.Cos(num2), Mathf.Sin(num2));
				Vector2 vector2 = new Vector2(Mathf.Cos(num3), Mathf.Sin(num3));
				this.AddQuad(vh, this.ToLocal(center + vector * (radius - num)), this.ToLocal(center + vector * (radius + num)), this.ToLocal(center + vector2 * (radius + num)), this.ToLocal(center + vector2 * (radius - num)));
			}
		}

		// Token: 0x06005420 RID: 21536 RVA: 0x0020A828 File Offset: 0x00208A28
		private void AddDisc(VertexHelper vh, Vector2 center, float radius, int segments)
		{
			Vector2 vector = this.ToLocal(center);
			int currentVertCount = vh.currentVertCount;
			vh.AddVert(this.CreateVertex(vector));
			for (int i = 0; i <= segments; i++)
			{
				float num = 6.2831855f * (float)i / (float)segments;
				Vector2 vector2 = center + new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * radius;
				vh.AddVert(this.CreateVertex(this.ToLocal(vector2)));
				if (i > 0)
				{
					vh.AddTriangle(currentVertCount, currentVertCount + i, currentVertCount + i + 1);
				}
			}
		}

		// Token: 0x06005421 RID: 21537 RVA: 0x0020A8B4 File Offset: 0x00208AB4
		private void AddQuad(VertexHelper vh, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
		{
			vh.AddUIVertexQuad(new UIVertex[]
			{
				this.CreateVertex(a),
				this.CreateVertex(b),
				this.CreateVertex(c),
				this.CreateVertex(d)
			});
		}

		// Token: 0x06005422 RID: 21538 RVA: 0x0020A90C File Offset: 0x00208B0C
		private UIVertex CreateVertex(Vector2 position)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = this.color;
			simpleVert.position = position;
			return simpleVert;
		}

		// Token: 0x06005423 RID: 21539 RVA: 0x0020A940 File Offset: 0x00208B40
		private Vector2 ToLocal(Vector2 designPosition)
		{
			return base.GetPixelAdjustedRect().center + designPosition * this.ResolveScale();
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x0020A96C File Offset: 0x00208B6C
		private float ResolveScale()
		{
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			return Mathf.Min(pixelAdjustedRect.width, pixelAdjustedRect.height) / 100f;
		}

		// Token: 0x04005488 RID: 21640
		[SerializeField]
		private VehicleLifetimeGlyph glyph;
	}
}
