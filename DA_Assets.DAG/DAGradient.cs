using System;
using System.Collections.Generic;
using System.Linq;
using DA_Assets.DAI;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.DAG
{
	// Token: 0x02000003 RID: 3
	[AddComponentMenu("UI/Effects/D.A. Gradient (DAG)")]
	public class DAGradient : BaseMeshEffect
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020BE File Offset: 0x000002BE
		protected override void Awake()
		{
			base.Awake();
			this.rectTransform = base.GetComponent<RectTransform>();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020D2 File Offset: 0x000002D2
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020DA File Offset: 0x000002DA
		[SerializeProperty("blendMode")]
		public DAColorBlendMode BlendMode
		{
			get
			{
				return this.blendMode;
			}
			set
			{
				this.blendMode = value;
				Graphic graphic = base.graphic;
				if (graphic == null)
				{
					return;
				}
				graphic.SetVerticesDirty();
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020F3 File Offset: 0x000002F3
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000020FB File Offset: 0x000002FB
		[SerializeProperty("intensity")]
		public float Intensity
		{
			get
			{
				return this.intensity;
			}
			set
			{
				this.intensity = Mathf.Clamp01(value);
				Graphic graphic = base.graphic;
				if (graphic == null)
				{
					return;
				}
				graphic.SetVerticesDirty();
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002119 File Offset: 0x00000319
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002121 File Offset: 0x00000321
		[SerializeProperty("angle")]
		public float Angle
		{
			get
			{
				return this.angle;
			}
			set
			{
				this.angle = value.NormalizeAngle360();
				Graphic graphic = base.graphic;
				if (graphic == null)
				{
					return;
				}
				graphic.SetVerticesDirty();
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000213F File Offset: 0x0000033F
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002147 File Offset: 0x00000347
		[SerializeProperty("gradient")]
		public Gradient Gradient
		{
			get
			{
				return this.gradient;
			}
			set
			{
				this.gradient = value;
				Graphic graphic = base.graphic;
				if (graphic == null)
				{
					return;
				}
				graphic.SetVerticesDirty();
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002160 File Offset: 0x00000360
		public override void ModifyMesh(VertexHelper vertexHelper)
		{
			if (!base.enabled)
			{
				return;
			}
			if (vertexHelper == null)
			{
				return;
			}
			UIVertex uivertex = default(UIVertex);
			Vector2 size = this.rectTransform.rect.size;
			Vector2 center = this.rectTransform.rect.center;
			this.AdjustGradientPart(vertexHelper);
			for (int i = 0; i < vertexHelper.currentVertCount; i++)
			{
				vertexHelper.PopulateUIVertex(ref uivertex, i);
				float num = 0.017453292f * this.angle;
				float num2 = Mathf.Abs(Mathf.Sin(num)) + Mathf.Abs(Mathf.Cos(num));
				float num3 = (uivertex.position.x - center.x) / size.x;
				float num4 = (uivertex.position.y - center.y) / size.y;
				Vector2 vector = new Vector2(num3, num4).Rotate(this.angle) / num2;
				Vector2 vector2 = new Vector2(0.5f, 0.5f);
				Vector2 vector3 = vector + vector2;
				float y = vector3.y;
				if (!float.IsNaN(y) && !float.IsInfinity(y))
				{
					Color color = this.gradient.Evaluate(vector3.y);
					uivertex.color = DAColorBlender.Blend(uivertex.color, color, this.blendMode, this.intensity);
				}
				vertexHelper.SetUIVertex(uivertex, i);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022CC File Offset: 0x000004CC
		private void AdjustGradientPart(VertexHelper vertexHelper)
		{
			List<UIVertex> originalVertices = new List<UIVertex>();
			vertexHelper.GetUIVertexStream(originalVertices);
			vertexHelper.Clear();
			List<UIVertex> modifiedVertices = new List<UIVertex>();
			Vector2 vector = Vector2.up.Rotate(-this.angle);
			vector.Scale(Vector2.one / this.rectTransform.rect.size);
			Vector2 perpendicularDirection = vector.Rotate(90f);
			this.gradient.alphaKeys.Select<GradientAlphaKey, float>((GradientAlphaKey alphaKey) => alphaKey.time).Union<float>(this.gradient.colorKeys.Select<GradientColorKey, float>((GradientColorKey colorKey) => colorKey.time)).ToList<float>()
				.ForEach(delegate(float pos)
				{
					modifiedVertices.Clear();
					Vector2 p = this.InterpolateSlice(pos);
					Enumerable.Range(0, originalVertices.Count / 3).ToList<int>().ForEach(delegate(int i)
					{
						this.GetVertices(originalVertices, i * 3, modifiedVertices, perpendicularDirection, p);
					});
					originalVertices.Clear();
					originalVertices.AddRange(modifiedVertices);
				});
			vertexHelper.AddUIVertexTriangleStream(originalVertices);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023DC File Offset: 0x000005DC
		private Vector2 InterpolateSlice(float interpolationFactor)
		{
			Vector2 size = this.rectTransform.rect.size;
			Vector2 vector = Vector2.up.Rotate(-this.angle);
			vector.Scale(Vector2.one / size);
			Vector2 vector2 = ((this.angle % 180f < 90f) ? (Vector2.down + Vector2.left) : (Vector2.up + Vector2.left));
			Vector2 vector3 = ((this.angle % 180f < 90f) ? (Vector2.up + Vector2.right) : (Vector2.down + Vector2.right));
			Vector3 vector4 = Vector3.Project(size * vector2 * 0.5f, vector);
			Vector3 vector5 = Vector3.Project(size * vector3 * 0.5f, vector);
			return Vector2.Lerp((this.angle % 360f >= 180f) ? vector5 : vector4, (this.angle % 360f >= 180f) ? vector4 : vector5, interpolationFactor) + this.rectTransform.rect.center;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000252C File Offset: 0x0000072C
		private void GetVertices(List<UIVertex> triangleVertices, int startIndex, List<UIVertex> outputVertices, Vector2 direction, Vector2 referencePoint)
		{
			ValueTuple<UIVertex, UIVertex, UIVertex> valueTuple = new ValueTuple<UIVertex, UIVertex, UIVertex>(triangleVertices[startIndex], triangleVertices[startIndex + 1], triangleVertices[startIndex + 2]);
			Vector3 vector = new Vector3(valueTuple.Item1.position.CalculateIntersectionFactor(valueTuple.Item2.position, referencePoint, direction), valueTuple.Item2.position.CalculateIntersectionFactor(valueTuple.Item3.position, referencePoint, direction), valueTuple.Item3.position.CalculateIntersectionFactor(valueTuple.Item1.position, referencePoint, direction));
			if (vector.x.IsBetween())
			{
				UIVertex uivertex = valueTuple.Item1.Lerp(valueTuple.Item2, vector.x);
				if (vector.y.IsBetween())
				{
					UIVertex uivertex2 = valueTuple.Item2.Lerp(valueTuple.Item3, vector.y);
					outputVertices.AddRange(new UIVertex[] { valueTuple.Item1, uivertex, valueTuple.Item3, uivertex, uivertex2, valueTuple.Item3, uivertex, valueTuple.Item2, uivertex2 });
					return;
				}
				UIVertex uivertex3 = valueTuple.Item3.Lerp(valueTuple.Item1, vector.z);
				outputVertices.AddRange(new UIVertex[] { valueTuple.Item3, uivertex3, valueTuple.Item2, uivertex3, uivertex, valueTuple.Item2, uivertex3, valueTuple.Item1, uivertex });
				return;
			}
			else
			{
				if (vector.y.IsBetween())
				{
					UIVertex uivertex4 = valueTuple.Item2.Lerp(valueTuple.Item3, vector.y);
					UIVertex uivertex5 = valueTuple.Item3.Lerp(valueTuple.Item1, vector.z);
					outputVertices.AddRange(new UIVertex[] { valueTuple.Item2, uivertex4, valueTuple.Item1, uivertex4, uivertex5, valueTuple.Item1, uivertex4, valueTuple.Item3, uivertex5 });
					return;
				}
				outputVertices.AddRange(new UIVertex[] { valueTuple.Item1, valueTuple.Item2, valueTuple.Item3 });
				return;
			}
		}

		// Token: 0x04000001 RID: 1
		private RectTransform rectTransform;

		// Token: 0x04000002 RID: 2
		[SerializeField]
		private DAColorBlendMode blendMode;

		// Token: 0x04000003 RID: 3
		[SerializeField]
		[Range(0f, 1f)]
		private float intensity = 1f;

		// Token: 0x04000004 RID: 4
		[SerializeField]
		[Range(0f, 360f)]
		private float angle;

		// Token: 0x04000005 RID: 5
		[SerializeField]
		private Gradient gradient = new Gradient();
	}
}
