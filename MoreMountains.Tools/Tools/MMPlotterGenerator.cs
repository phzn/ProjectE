using System;
using System.Reflection;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000028 RID: 40
	public class MMPlotterGenerator : MonoBehaviour
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00005322 File Offset: 0x00003522
		protected virtual void Awake()
		{
			Time.timeScale = 0f;
			this.GeneratePlotters();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005334 File Offset: 0x00003534
		protected virtual void GeneratePlotters()
		{
			base.transform.MMDestroyAllChildren();
			BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public;
			MethodInfo[] methods = typeof(MMTweenDefinitions).GetMethods(bindingFlags);
			int num = 0;
			int num2 = 0;
			float num3 = 0f;
			for (int i = 0; i < methods.Length; i++)
			{
				this._position.x = (float)num2 * this.Spacing.x;
				this._position.y = num3;
				MMPlotter mmplotter = Object.Instantiate<MMPlotter>(this.PlotterPrefab);
				mmplotter.transform.SetParent(base.transform);
				mmplotter.transform.localPosition = this._position;
				mmplotter.TweenMethodIndex = i;
				string text = mmplotter.TweenName(mmplotter.TweenMethodIndex);
				mmplotter.gameObject.name = text;
				Material material = this.LinearMaterial;
				if (text.Contains("Linear"))
				{
					material = this.LinearMaterial;
				}
				if (text.Contains("Quadratic"))
				{
					material = this.QuadraticMaterial;
				}
				if (text.Contains("Cubic"))
				{
					material = this.CubicMaterial;
				}
				if (text.Contains("Quartic"))
				{
					material = this.QuarticMaterial;
				}
				if (text.Contains("Quintic"))
				{
					material = this.QuinticMaterial;
				}
				if (text.Contains("Sinusoidal"))
				{
					material = this.SinusoidalMaterial;
				}
				if (text.Contains("Bounce"))
				{
					material = this.BounceMaterial;
				}
				if (text.Contains("Overhead"))
				{
					material = this.OverheadMaterial;
				}
				if (text.Contains("Exponential"))
				{
					material = this.ExponentialMaterial;
				}
				if (text.Contains("Elastic"))
				{
					material = this.ElasticMaterial;
				}
				if (text.Contains("Circular"))
				{
					material = this.CircularMaterial;
				}
				mmplotter.SetMaterial(material);
				mmplotter.GetMethodsList();
				mmplotter.DrawGraph();
				if (num2 >= this.RowLength - 1)
				{
					num2 = 0;
					num++;
					if (num % 2 == 0)
					{
						num3 += this.Spacing.y + this.VerticalOddSpacing;
					}
					else
					{
						num3 += this.Spacing.y;
					}
				}
				else
				{
					num2++;
				}
			}
		}

		// Token: 0x0400011F RID: 287
		public MMPlotter PlotterPrefab;

		// Token: 0x04000120 RID: 288
		public Vector2 Spacing;

		// Token: 0x04000121 RID: 289
		public float VerticalOddSpacing;

		// Token: 0x04000122 RID: 290
		public int RowLength;

		// Token: 0x04000123 RID: 291
		[Header("Materials")]
		public Material LinearMaterial;

		// Token: 0x04000124 RID: 292
		public Material QuadraticMaterial;

		// Token: 0x04000125 RID: 293
		public Material CubicMaterial;

		// Token: 0x04000126 RID: 294
		public Material QuarticMaterial;

		// Token: 0x04000127 RID: 295
		public Material QuinticMaterial;

		// Token: 0x04000128 RID: 296
		public Material SinusoidalMaterial;

		// Token: 0x04000129 RID: 297
		public Material BounceMaterial;

		// Token: 0x0400012A RID: 298
		public Material OverheadMaterial;

		// Token: 0x0400012B RID: 299
		public Material ExponentialMaterial;

		// Token: 0x0400012C RID: 300
		public Material ElasticMaterial;

		// Token: 0x0400012D RID: 301
		public Material CircularMaterial;

		// Token: 0x0400012E RID: 302
		protected Vector2 _position;

		// Token: 0x0400012F RID: 303
		[MMInspectorButton("GeneratePlotters")]
		public bool GeneratePlottersButton;
	}
}
