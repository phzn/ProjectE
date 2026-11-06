using System;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000268 RID: 616
	[Serializable]
	public class MMSpringDebug
	{
		// Token: 0x0600117F RID: 4479 RVA: 0x0004D1D4 File Offset: 0x0004B3D4
		public void Update(float value, float target)
		{
			this.CurrentValue = value;
			this.TargetValue = target;
		}

		// Token: 0x04001460 RID: 5216
		public float CurrentValue;

		// Token: 0x04001461 RID: 5217
		public float TargetValue;
	}
}
