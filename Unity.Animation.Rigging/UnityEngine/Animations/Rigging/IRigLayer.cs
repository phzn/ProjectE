using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000059 RID: 89
	public interface IRigLayer
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000239 RID: 569
		Rig rig { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600023A RID: 570
		IRigConstraint[] constraints { get; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600023B RID: 571
		IAnimationJob[] jobs { get; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600023C RID: 572
		bool active { get; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600023D RID: 573
		string name { get; }

		// Token: 0x0600023E RID: 574
		bool Initialize(Animator animator);

		// Token: 0x0600023F RID: 575
		void Update();

		// Token: 0x06000240 RID: 576
		void Reset();

		// Token: 0x06000241 RID: 577
		bool IsValid();
	}
}
