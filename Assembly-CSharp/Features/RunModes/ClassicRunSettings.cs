using System;

namespace Features.RunModes
{
	// Token: 0x02000D76 RID: 3446
	[Serializable]
	public sealed class ClassicRunSettings
	{
		// Token: 0x06006561 RID: 25953 RVA: 0x0028715C File Offset: 0x0028535C
		public ClassicRunSettings Clone()
		{
			return new ClassicRunSettings
			{
				startAtNight = this.startAtNight
			};
		}

		// Token: 0x040067EA RID: 26602
		public bool startAtNight;
	}
}
