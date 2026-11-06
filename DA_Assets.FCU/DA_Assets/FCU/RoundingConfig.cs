using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public class RoundingConfig
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002DC6 File Offset: 0x00000FC6
		public int IsCircleDigits
		{
			get
			{
				return this.isCircleDigits;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002DCE File Offset: 0x00000FCE
		public int GetBoundingSizeDigits
		{
			get
			{
				return this.getBoundingSizeDigits;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002DD6 File Offset: 0x00000FD6
		public int GetAngleFromMatrixDigits
		{
			get
			{
				return this.getAngleFromMatrixDigits;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002DDE File Offset: 0x00000FDE
		public int DiffCheckerSizeDigits
		{
			get
			{
				return this.diffCheckerSizeDigits;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00002DE6 File Offset: 0x00000FE6
		public int GetPaddingDigits
		{
			get
			{
				return this.getPaddingDigits;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002DEE File Offset: 0x00000FEE
		public int FontDigits
		{
			get
			{
				return this.fontDigits;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00002DF6 File Offset: 0x00000FF6
		public int GetMaxAllowedScaleDigits
		{
			get
			{
				return this.getMaxAllowedScaleDigits;
			}
		}

		// Token: 0x04000051 RID: 81
		[SerializeField]
		private int isCircleDigits;

		// Token: 0x04000052 RID: 82
		[SerializeField]
		private int getBoundingSizeDigits = 2;

		// Token: 0x04000053 RID: 83
		[SerializeField]
		private int getAngleFromMatrixDigits = 2;

		// Token: 0x04000054 RID: 84
		[SerializeField]
		private int diffCheckerSizeDigits = 2;

		// Token: 0x04000055 RID: 85
		[SerializeField]
		private int getPaddingDigits;

		// Token: 0x04000056 RID: 86
		[SerializeField]
		private int fontDigits = 2;

		// Token: 0x04000057 RID: 87
		[SerializeField]
		private int getMaxAllowedScaleDigits = 2;
	}
}
