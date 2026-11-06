using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000766 RID: 1894
	public class TireSpinOutDry : LivingMudEffectsPlayer, ISpinOut
	{
		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x00113FE9 File Offset: 0x001121E9
		public override MudType RequiredThickness { get; } = MudType.Dry;

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060030FE RID: 12542 RVA: 0x00113FF1 File Offset: 0x001121F1
		// (set) Token: 0x060030FF RID: 12543 RVA: 0x00113FF9 File Offset: 0x001121F9
		public bool spinningOut { get; private set; }

		// Token: 0x06003100 RID: 12544 RVA: 0x00114002 File Offset: 0x00112202
		public override void StartEffects(DynamicMudSurface surface, MudType thickness, bool overrideThickness = false)
		{
			if ((thickness == this.RequiredThickness || overrideThickness) && !this.spinningOut)
			{
				this.SpinOutStart();
			}
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x0011401F File Offset: 0x0011221F
		public override void StopEffects()
		{
			this.SpinOutStop();
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x00114028 File Offset: 0x00112228
		private void SpinOutStart()
		{
			this.spinningOut = true;
			if (this.DirtSprayParticles)
			{
				this.DirtSprayParticles.Play();
				this.DirtSprayParticles.Emit(20);
			}
			if (this.StoneSprayParticles)
			{
				this.StoneSprayParticles.Play();
				this.StoneSprayParticles.Emit(50);
			}
		}

		// Token: 0x06003103 RID: 12547 RVA: 0x00114086 File Offset: 0x00112286
		private void SpinOutStop()
		{
			this.spinningOut = false;
			if (this.DirtSprayParticles)
			{
				this.DirtSprayParticles.Stop();
			}
			if (this.StoneSprayParticles)
			{
				this.StoneSprayParticles.Stop();
			}
		}

		// Token: 0x04003174 RID: 12660
		public ParticleSystem DirtSprayParticles;

		// Token: 0x04003175 RID: 12661
		public ParticleSystem StoneSprayParticles;
	}
}
