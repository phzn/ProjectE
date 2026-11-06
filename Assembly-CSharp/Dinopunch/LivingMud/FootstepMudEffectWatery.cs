using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000739 RID: 1849
	public class FootstepMudEffectWatery : LivingMudEffectsPlayer, IFootstepMudEffect
	{
		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06002FCE RID: 12238 RVA: 0x0010EA60 File Offset: 0x0010CC60
		public override MudType RequiredThickness
		{
			get
			{
				return this.targetThickness;
			}
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x0010EA68 File Offset: 0x0010CC68
		private void Awake()
		{
			FootstepMudEffectWatery.StopAndClearParticles(this.waterSprayParticles);
			FootstepMudEffectWatery.StopAndClearParticles(this.stoneSprayParticles);
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x0010EA80 File Offset: 0x0010CC80
		private void OnDisable()
		{
			this.StopEffectsScaled();
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x0010EA88 File Offset: 0x0010CC88
		public override void StartEffects(DynamicMudSurface surface, MudType thickness, bool overrideThickness = false)
		{
			this.StartEffectsScaled(surface, thickness, 1f);
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x0010EA98 File Offset: 0x0010CC98
		public void StartEffectsScaled(DynamicMudSurface surface, MudType thickness, float intensity)
		{
			if ((this.RequiredThickness & thickness) == (MudType)0)
			{
				return;
			}
			this.effectsActive = true;
			if (this.waterSprayParticles != null)
			{
				int num = Mathf.Max(1, Mathf.RoundToInt((float)this.waterParticleCount * intensity));
				this.waterSprayParticles.Emit(num);
				this.waterSprayParticles.Play();
			}
			if (this.stoneSprayParticles != null)
			{
				int num2 = Mathf.Max(1, Mathf.RoundToInt((float)this.stoneParticleCount * intensity));
				this.stoneSprayParticles.Emit(num2);
				this.stoneSprayParticles.Play();
			}
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x0010EA80 File Offset: 0x0010CC80
		public override void StopEffects()
		{
			this.StopEffectsScaled();
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x0010EB2B File Offset: 0x0010CD2B
		public void StopEffectsScaled()
		{
			this.effectsActive = false;
			FootstepMudEffectWatery.StopAndClearParticles(this.waterSprayParticles);
			FootstepMudEffectWatery.StopAndClearParticles(this.stoneSprayParticles);
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x0010E86E File Offset: 0x0010CA6E
		private static void StopAndClearParticles(ParticleSystem ps)
		{
			if (ps == null)
			{
				return;
			}
			ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			ps.Clear(true);
		}

		// Token: 0x04003052 RID: 12370
		[SerializeField]
		private MudType targetThickness = MudType.Watery;

		// Token: 0x04003053 RID: 12371
		[Header("Particle Effects")]
		[Tooltip("Small water spray particles on footstep")]
		[SerializeField]
		private ParticleSystem waterSprayParticles;

		// Token: 0x04003054 RID: 12372
		[Tooltip("Optional small stones scatter on footstep")]
		[SerializeField]
		private ParticleSystem stoneSprayParticles;

		// Token: 0x04003055 RID: 12373
		[Header("Tuning")]
		[Tooltip("Number of water spray particles per footstep")]
		[SerializeField]
		private int waterParticleCount = 3;

		// Token: 0x04003056 RID: 12374
		[Tooltip("Number of stone scatter particles per footstep")]
		[SerializeField]
		private int stoneParticleCount = 5;

		// Token: 0x04003057 RID: 12375
		private bool effectsActive;
	}
}
