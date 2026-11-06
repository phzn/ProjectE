using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004CE RID: 1230
	public interface vIEffect
	{
		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001DA9 RID: 7593
		string EffectName { get; }

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001DAA RID: 7594
		float EffectDuration { get; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001DAB RID: 7595
		Vector3 EffectPosition { get; }

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001DAC RID: 7596
		Transform Sender { get; }
	}
}
