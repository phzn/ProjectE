using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000769 RID: 1897
	public class TireSpinOutWatery : LivingMudEffectsPlayer, ISpinOut
	{
		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06003113 RID: 12563 RVA: 0x00114244 File Offset: 0x00112444
		public override MudType RequiredThickness { get; } = MudType.Watery;

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06003114 RID: 12564 RVA: 0x0011424C File Offset: 0x0011244C
		// (set) Token: 0x06003115 RID: 12565 RVA: 0x00114254 File Offset: 0x00112454
		public bool spinningOut { get; private set; }

		// Token: 0x06003116 RID: 12566 RVA: 0x0011425D File Offset: 0x0011245D
		public override void StartEffects(DynamicMudSurface surface, MudType thickness, bool overrideThickness = false)
		{
			if ((thickness == this.RequiredThickness || overrideThickness) && !this.spinningOut)
			{
				this.SpinOutStart();
			}
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x0011427A File Offset: 0x0011247A
		public override void StopEffects()
		{
			this.SpinOutStop();
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x00114284 File Offset: 0x00112484
		private void SpinOutStart()
		{
			this.spinningOut = true;
			if (this.WaterSprayParticles)
			{
				this.WaterSprayParticles.Emit(5);
				this.WaterSprayParticles.Play();
			}
			if (this.StoneSprayParticles)
			{
				this.StoneSprayParticles.Emit(50);
				this.StoneSprayParticles.Play();
			}
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x001142E1 File Offset: 0x001124E1
		private void SpinOutStop()
		{
			this.spinningOut = false;
			if (this.WaterSprayParticles)
			{
				this.WaterSprayParticles.Stop();
			}
			if (this.StoneSprayParticles)
			{
				this.StoneSprayParticles.Stop();
			}
		}

		// Token: 0x04003180 RID: 12672
		public ParticleSystem WaterSprayParticles;

		// Token: 0x04003181 RID: 12673
		public ParticleSystem StoneSprayParticles;
	}
}
