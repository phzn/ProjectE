using System;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000032 RID: 50
	[Serializable]
	public class FGraphic
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000DB RID: 219 RVA: 0x0000389C File Offset: 0x00001A9C
		// (set) Token: 0x060000DC RID: 220 RVA: 0x000038A4 File Offset: 0x00001AA4
		public bool HasFill
		{
			get
			{
				return this.hasFill;
			}
			set
			{
				this.hasFill = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000038AD File Offset: 0x00001AAD
		// (set) Token: 0x060000DE RID: 222 RVA: 0x000038B5 File Offset: 0x00001AB5
		public bool HasStroke
		{
			get
			{
				return this.hasStroke;
			}
			set
			{
				this.hasStroke = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000038BE File Offset: 0x00001ABE
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x000038C6 File Offset: 0x00001AC6
		public bool HasSingleColor
		{
			get
			{
				return this.hasSingleColor;
			}
			set
			{
				this.hasSingleColor = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000038CF File Offset: 0x00001ACF
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x000038D7 File Offset: 0x00001AD7
		public bool HasSingleGradient
		{
			get
			{
				return this.hasSingleGradient;
			}
			set
			{
				this.hasSingleGradient = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x000038E0 File Offset: 0x00001AE0
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x000038E8 File Offset: 0x00001AE8
		public FFill Fill
		{
			get
			{
				return this.fill;
			}
			set
			{
				this.fill = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000038F1 File Offset: 0x00001AF1
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x000038F9 File Offset: 0x00001AF9
		public FStroke Stroke
		{
			get
			{
				return this.stroke;
			}
			set
			{
				this.stroke = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00003902 File Offset: 0x00001B02
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x0000390A File Offset: 0x00001B0A
		public Color SpriteSingleColor
		{
			get
			{
				return this.spriteSingleColor;
			}
			set
			{
				this.spriteSingleColor = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00003913 File Offset: 0x00001B13
		// (set) Token: 0x060000EA RID: 234 RVA: 0x0000391B File Offset: 0x00001B1B
		public Paint SpriteSingleLinearGradient
		{
			get
			{
				return this.spriteSingleLinearGradient;
			}
			set
			{
				this.spriteSingleLinearGradient = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00003924 File Offset: 0x00001B24
		// (set) Token: 0x060000EC RID: 236 RVA: 0x0000392C File Offset: 0x00001B2C
		public bool FillAlpha1
		{
			get
			{
				return this.fillAlpha1;
			}
			set
			{
				this.fillAlpha1 = value;
			}
		}

		// Token: 0x04000318 RID: 792
		[SerializeField]
		private bool hasFill;

		// Token: 0x04000319 RID: 793
		[SerializeField]
		private bool hasStroke;

		// Token: 0x0400031A RID: 794
		[SerializeField]
		private bool hasSingleColor;

		// Token: 0x0400031B RID: 795
		[SerializeField]
		private bool hasSingleGradient;

		// Token: 0x0400031C RID: 796
		[SerializeField]
		private FFill fill;

		// Token: 0x0400031D RID: 797
		[SerializeField]
		private FStroke stroke;

		// Token: 0x0400031E RID: 798
		[SerializeField]
		private Color spriteSingleColor;

		// Token: 0x0400031F RID: 799
		[SerializeField]
		private Paint spriteSingleLinearGradient;

		// Token: 0x04000320 RID: 800
		[SerializeField]
		private bool fillAlpha1;
	}
}
