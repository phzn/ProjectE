using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000738 RID: 1848
	public class FootstepMudEffectSloppy : LivingMudEffectsPlayer, ISetColorMap, IFootstepMudEffect
	{
		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06002FC3 RID: 12227 RVA: 0x0010E8A6 File Offset: 0x0010CAA6
		public override MudType RequiredThickness
		{
			get
			{
				return this.targetThickness;
			}
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x0010E8AE File Offset: 0x0010CAAE
		private void Awake()
		{
			FootstepMudEffectSloppy.StopAndClearParticles(this.mudSprayParticles);
			if (this.directionalSplashVFX != null)
			{
				this.directionalSplashVFX.Stop();
			}
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x0010E8D4 File Offset: 0x0010CAD4
		private void OnDisable()
		{
			this.StopEffectsScaled();
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x0010E8DC File Offset: 0x0010CADC
		public override void StartEffects(DynamicMudSurface surface, MudType thickness, bool overrideThickness = false)
		{
			this.StartEffectsScaled(surface, thickness, 1f);
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x0010E8EC File Offset: 0x0010CAEC
		public void StartEffectsScaled(DynamicMudSurface surface, MudType thickness, float intensity)
		{
			if ((this.RequiredThickness & thickness) == (MudType)0)
			{
				return;
			}
			this.effectsActive = true;
			int num = Mathf.Max(1, Mathf.RoundToInt((float)this.sprayParticleCount * intensity));
			if (this.mudSprayParticles != null)
			{
				this.mudSprayParticles.Play();
				this.mudSprayParticles.Emit(num);
			}
			if (this.directionalSplashVFX != null)
			{
				this.directionalSplashVFX.SetFloat("Scale Modifier", this.splashScaleModifier * intensity);
				this.directionalSplashVFX.Play();
			}
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x0010E8D4 File Offset: 0x0010CAD4
		public override void StopEffects()
		{
			this.StopEffectsScaled();
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x0010E976 File Offset: 0x0010CB76
		public void StopEffectsScaled()
		{
			this.effectsActive = false;
			FootstepMudEffectSloppy.StopAndClearParticles(this.mudSprayParticles);
			if (this.directionalSplashVFX != null)
			{
				this.directionalSplashVFX.Stop();
			}
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x0010E86E File Offset: 0x0010CA6E
		private static void StopAndClearParticles(ParticleSystem ps)
		{
			if (ps == null)
			{
				return;
			}
			ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			ps.Clear(true);
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x0010E9A3 File Offset: 0x0010CBA3
		public void SetColorMap(Texture2D tex)
		{
			if (this.directionalSplashVFX != null && tex != null)
			{
				this.directionalSplashVFX.SetTexture("Color Map", tex);
			}
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x0010E9D0 File Offset: 0x0010CBD0
		public void SetColor(Color color)
		{
			if (this.directionalSplashVFX != null)
			{
				this.directionalSplashVFX.SetVector4("Tint", new Vector4(color.r, color.g, color.b, color.a));
			}
			if (this.mudSprayParticles != null)
			{
				this.mudSprayParticles.main.startColor = color;
			}
		}

		// Token: 0x0400304C RID: 12364
		[SerializeField]
		private MudType targetThickness = MudType.Sloppy;

		// Token: 0x0400304D RID: 12365
		[Header("Particle Effects")]
		[Tooltip("Small mud spray particles on footstep")]
		[SerializeField]
		private ParticleSystem mudSprayParticles;

		// Token: 0x0400304E RID: 12366
		[Tooltip("Optional small directional splash VFX on footstep")]
		[SerializeField]
		private VisualEffect directionalSplashVFX;

		// Token: 0x0400304F RID: 12367
		[Header("Tuning")]
		[Tooltip("Number of mud spray particles per footstep")]
		[SerializeField]
		private int sprayParticleCount = 8;

		// Token: 0x04003050 RID: 12368
		[Tooltip("Scale modifier for the directional splash (1.0 = tire size, use 0.2-0.4 for feet)")]
		[SerializeField]
		private float splashScaleModifier = 0.25f;

		// Token: 0x04003051 RID: 12369
		private bool effectsActive;
	}
}
