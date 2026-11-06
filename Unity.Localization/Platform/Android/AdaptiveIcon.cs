using System;

namespace UnityEngine.Localization.Platform.Android
{
	// Token: 0x020000B7 RID: 183
	[Serializable]
	public class AdaptiveIcon
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x0001219D File Offset: 0x0001039D
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x000121A5 File Offset: 0x000103A5
		public LocalizedTexture Background
		{
			get
			{
				return this.m_Background;
			}
			set
			{
				this.m_Background = value;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x000121AE File Offset: 0x000103AE
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x000121B6 File Offset: 0x000103B6
		public LocalizedTexture Foreground
		{
			get
			{
				return this.m_Foreground;
			}
			set
			{
				this.m_Foreground = value;
			}
		}

		// Token: 0x0400017B RID: 379
		[SerializeField]
		private LocalizedTexture m_Background;

		// Token: 0x0400017C RID: 380
		[SerializeField]
		private LocalizedTexture m_Foreground;
	}
}
