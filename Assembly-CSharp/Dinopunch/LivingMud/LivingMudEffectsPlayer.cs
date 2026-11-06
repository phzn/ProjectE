using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000753 RID: 1875
	public abstract class LivingMudEffectsPlayer : MonoBehaviour
	{
		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06003085 RID: 12421
		public abstract MudType RequiredThickness { get; }

		// Token: 0x06003086 RID: 12422
		public abstract void StartEffects(DynamicMudSurface surface, MudType thickness, bool overrideThickness = false);

		// Token: 0x06003087 RID: 12423
		public abstract void StopEffects();
	}
}
