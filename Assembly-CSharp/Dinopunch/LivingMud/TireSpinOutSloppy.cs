using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000767 RID: 1895
	public class TireSpinOutSloppy : LivingMudEffectsPlayer, ISpinOut
	{
		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x001140CE File Offset: 0x001122CE
		public override MudType RequiredThickness { get; } = MudType.Sloppy;

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06003106 RID: 12550 RVA: 0x001140D6 File Offset: 0x001122D6
		// (set) Token: 0x06003107 RID: 12551 RVA: 0x001140DE File Offset: 0x001122DE
		public bool spinningOut { get; private set; }

		// Token: 0x06003108 RID: 12552 RVA: 0x001140E7 File Offset: 0x001122E7
		public override void StartEffects(DynamicMudSurface surface, MudType thickness, bool overrideThickness = false)
		{
			if ((thickness == this.RequiredThickness || overrideThickness) && !this.spinningOut)
			{
				base.StartCoroutine(this.SpinOutStart());
			}
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x0011410B File Offset: 0x0011230B
		public override void StopEffects()
		{
			this.SpinOutStop();
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x00114113 File Offset: 0x00112313
		private IEnumerator SpinOutStart()
		{
			this.spinningOut = true;
			float splashSize = 1f;
			if (this.MudSprayParticles)
			{
				this.MudSprayParticles.Play();
			}
			while (this.spinningOut)
			{
				this.SpinOutVFX.SetFloat("Scale Modifier", splashSize);
				splashSize *= 0.75f;
				if (this.SpinOutVFX)
				{
					this.SpinOutVFX.Play();
				}
				yield return new WaitForSeconds(0.25f);
			}
			this.SpinOutVFX.SetFloat("Scale Modifier", 1f);
			yield break;
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x00114122 File Offset: 0x00112322
		private void SpinOutStop()
		{
			this.spinningOut = false;
			if (this.MudSprayParticles)
			{
				this.MudSprayParticles.Stop();
			}
		}

		// Token: 0x04003178 RID: 12664
		public ParticleSystem MudSprayParticles;

		// Token: 0x04003179 RID: 12665
		public VisualEffect SpinOutVFX;
	}
}
