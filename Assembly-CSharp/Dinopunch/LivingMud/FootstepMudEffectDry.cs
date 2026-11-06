using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000737 RID: 1847
	public class FootstepMudEffectDry : LivingMudEffectsPlayer, IFootstepMudEffect
	{
		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06002FBA RID: 12218 RVA: 0x0010E785 File Offset: 0x0010C985
		public override MudType RequiredThickness
		{
			get
			{
				return this.targetThickness;
			}
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x0010E78D File Offset: 0x0010C98D
		private void Awake()
		{
			FootstepMudEffectDry.StopAndClearParticles(this.dirtSprayParticles);
			FootstepMudEffectDry.StopAndClearParticles(this.stoneSprayParticles);
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x0010E7A5 File Offset: 0x0010C9A5
		private void OnDisable()
		{
			this.StopEffectsScaled();
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x0010E7AD File Offset: 0x0010C9AD
		public override void StartEffects(DynamicMudSurface surface, MudType thickness, bool overrideThickness = false)
		{
			this.StartEffectsScaled(surface, thickness, 1f);
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x0010E7BC File Offset: 0x0010C9BC
		public void StartEffectsScaled(DynamicMudSurface surface, MudType thickness, float intensity)
		{
			if ((this.RequiredThickness & thickness) == (MudType)0)
			{
				return;
			}
			this.effectsActive = true;
			if (this.dirtSprayParticles != null)
			{
				int num = Mathf.Max(1, Mathf.RoundToInt((float)this.dirtParticleCount * intensity));
				this.dirtSprayParticles.Play();
				this.dirtSprayParticles.Emit(num);
			}
			if (this.stoneSprayParticles != null)
			{
				int num2 = Mathf.Max(1, Mathf.RoundToInt((float)this.stoneParticleCount * intensity));
				this.stoneSprayParticles.Play();
				this.stoneSprayParticles.Emit(num2);
			}
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x0010E7A5 File Offset: 0x0010C9A5
		public override void StopEffects()
		{
			this.StopEffectsScaled();
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x0010E84F File Offset: 0x0010CA4F
		public void StopEffectsScaled()
		{
			this.effectsActive = false;
			FootstepMudEffectDry.StopAndClearParticles(this.dirtSprayParticles);
			FootstepMudEffectDry.StopAndClearParticles(this.stoneSprayParticles);
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x0010E86E File Offset: 0x0010CA6E
		private static void StopAndClearParticles(ParticleSystem ps)
		{
			if (ps == null)
			{
				return;
			}
			ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			ps.Clear(true);
		}

		// Token: 0x04003046 RID: 12358
		[SerializeField]
		private MudType targetThickness = MudType.Dry;

		// Token: 0x04003047 RID: 12359
		[Header("Particle Effects")]
		[Tooltip("Small dirt/dust puff particles on footstep")]
		[SerializeField]
		private ParticleSystem dirtSprayParticles;

		// Token: 0x04003048 RID: 12360
		[Tooltip("Optional small stones scatter on footstep")]
		[SerializeField]
		private ParticleSystem stoneSprayParticles;

		// Token: 0x04003049 RID: 12361
		[Header("Tuning")]
		[Tooltip("Number of dirt particles per footstep")]
		[SerializeField]
		private int dirtParticleCount = 5;

		// Token: 0x0400304A RID: 12362
		[Tooltip("Number of stone scatter particles per footstep")]
		[SerializeField]
		private int stoneParticleCount = 3;

		// Token: 0x0400304B RID: 12363
		private bool effectsActive;
	}
}
