using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000039 RID: 57
	[Serializable]
	public struct FontMetadata
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00003BD8 File Offset: 0x00001DD8
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00003BE0 File Offset: 0x00001DE0
		public string Family
		{
			get
			{
				return this.family;
			}
			set
			{
				this.family = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00003BE9 File Offset: 0x00001DE9
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00003BF1 File Offset: 0x00001DF1
		public int Weight
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

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00003BFA File Offset: 0x00001DFA
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00003C02 File Offset: 0x00001E02
		public FontStyle FontStyle
		{
			get
			{
				return this.fontStyle;
			}
			set
			{
				this.fontStyle = value;
			}
		}

		// Token: 0x04000336 RID: 822
		[SerializeField]
		private string family;

		// Token: 0x04000337 RID: 823
		[SerializeField]
		private int weight;

		// Token: 0x04000338 RID: 824
		[SerializeField]
		private FontStyle fontStyle;
	}
}
