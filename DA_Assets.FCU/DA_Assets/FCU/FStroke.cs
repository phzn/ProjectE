using System;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public struct FStroke
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00003992 File Offset: 0x00001B92
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x0000399A File Offset: 0x00001B9A
		public StrokeAlign Align
		{
			get
			{
				return this.align;
			}
			set
			{
				this.align = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000039A3 File Offset: 0x00001BA3
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000039AB File Offset: 0x00001BAB
		public float Weight
		{
			get
			{
				return this.weight;
			}
			set
			{
				this.weight = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000039B4 File Offset: 0x00001BB4
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000039BC File Offset: 0x00001BBC
		public bool HasSolid
		{
			get
			{
				return this.hasSolid;
			}
			set
			{
				this.hasSolid = value;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000039C5 File Offset: 0x00001BC5
		// (set) Token: 0x060000FF RID: 255 RVA: 0x000039CD File Offset: 0x00001BCD
		public bool HasGradient
		{
			get
			{
				return this.hasGradient;
			}
			set
			{
				this.hasGradient = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000039D6 File Offset: 0x00001BD6
		// (set) Token: 0x06000101 RID: 257 RVA: 0x000039DE File Offset: 0x00001BDE
		public Paint SolidPaint
		{
			get
			{
				return this.solidPaint;
			}
			set
			{
				this.solidPaint = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000039E7 File Offset: 0x00001BE7
		// (set) Token: 0x06000103 RID: 259 RVA: 0x000039EF File Offset: 0x00001BEF
		public Paint GradientPaint
		{
			get
			{
				return this.gradientPaint;
			}
			set
			{
				this.gradientPaint = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000039F8 File Offset: 0x00001BF8
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00003A00 File Offset: 0x00001C00
		public Color SingleColor
		{
			get
			{
				return this.singleColor;
			}
			set
			{
				this.singleColor = value;
			}
		}

		// Token: 0x04000326 RID: 806
		[SerializeField]
		private StrokeAlign align;

		// Token: 0x04000327 RID: 807
		[SerializeField]
		private float weight;

		// Token: 0x04000328 RID: 808
		[SerializeField]
		private bool hasSolid;

		// Token: 0x04000329 RID: 809
		[SerializeField]
		private bool hasGradient;

		// Token: 0x0400032A RID: 810
		[SerializeField]
		private Paint solidPaint;

		// Token: 0x0400032B RID: 811
		[SerializeField]
		private Paint gradientPaint;

		// Token: 0x0400032C RID: 812
		[SerializeField]
		private Color singleColor;
	}
}
