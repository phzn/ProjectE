using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000235 RID: 565
	public interface ITransitionEvent
	{
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x0600103D RID: 4157
		StylePropertyNameCollection stylePropertyNames { get; }

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x0600103E RID: 4158
		double elapsedTime { get; }
	}
}
