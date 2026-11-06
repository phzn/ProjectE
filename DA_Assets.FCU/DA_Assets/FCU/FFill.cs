using System;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000033 RID: 51
	[Serializable]
	public struct FFill
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000EE RID: 238 RVA: 0x0000393D File Offset: 0x00001B3D
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00003945 File Offset: 0x00001B45
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

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x0000394E File Offset: 0x00001B4E
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00003956 File Offset: 0x00001B56
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000395F File Offset: 0x00001B5F
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00003967 File Offset: 0x00001B67
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

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00003970 File Offset: 0x00001B70
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00003978 File Offset: 0x00001B78
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

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00003981 File Offset: 0x00001B81
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00003989 File Offset: 0x00001B89
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

		// Token: 0x04000321 RID: 801
		[SerializeField]
		private bool hasSolid;

		// Token: 0x04000322 RID: 802
		[SerializeField]
		private bool hasGradient;

		// Token: 0x04000323 RID: 803
		[SerializeField]
		private Paint gradientPaint;

		// Token: 0x04000324 RID: 804
		[SerializeField]
		private Paint solidPaint;

		// Token: 0x04000325 RID: 805
		[SerializeField]
		private Color singleColor;
	}
}
