using System;

namespace UnityEngine.UI.CoroutineTween
{
	// Token: 0x02000047 RID: 71
	internal interface ITweenValue
	{
		// Token: 0x060004D9 RID: 1241
		void TweenValue(float floatPercentage);

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060004DA RID: 1242
		bool ignoreTimeScale { get; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060004DB RID: 1243
		float duration { get; }

		// Token: 0x060004DC RID: 1244
		bool ValidTarget();
	}
}
