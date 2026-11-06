using System;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200073C RID: 1852
	public interface IFootstepMudEffect
	{
		// Token: 0x06002FDB RID: 12251
		void StartEffectsScaled(DynamicMudSurface surface, MudType thickness, float intensity);

		// Token: 0x06002FDC RID: 12252
		void StopEffectsScaled();
	}
}
