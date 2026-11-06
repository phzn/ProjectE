using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000037 RID: 55
	public interface ICullingTarget
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000157 RID: 343
		GameObject GameObject { get; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000158 RID: 344
		Bounds Bounds { get; }

		// Token: 0x06000159 RID: 345
		void MakeVisible();

		// Token: 0x0600015A RID: 346
		void MakeInvisible();
	}
}
