using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200076B RID: 1899
	public class WateryMudImpactEffect : LivingMudEffectsPlayer, ISetColorMap, IUseImpactData
	{
		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x0600311F RID: 12575 RVA: 0x001144EE File Offset: 0x001126EE
		public override MudType RequiredThickness
		{
			get
			{
				return this.targetThickness;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06003120 RID: 12576 RVA: 0x001144F6 File Offset: 0x001126F6
		// (set) Token: 0x06003121 RID: 12577 RVA: 0x001144FE File Offset: 0x001126FE
		public float mass { get; set; }

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06003122 RID: 12578 RVA: 0x00114507 File Offset: 0x00112707
		// (set) Token: 0x06003123 RID: 12579 RVA: 0x0011450F File Offset: 0x0011270F
		public float speed { get; set; }

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06003124 RID: 12580 RVA: 0x00114518 File Offset: 0x00112718
		// (set) Token: 0x06003125 RID: 12581 RVA: 0x00114520 File Offset: 0x00112720
		public Vector3 impactDirection { get; set; }

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06003126 RID: 12582 RVA: 0x00114529 File Offset: 0x00112729
		// (set) Token: 0x06003127 RID: 12583 RVA: 0x00114531 File Offset: 0x00112731
		public Vector3 impactPoint { get; set; }

		// Token: 0x06003128 RID: 12584 RVA: 0x00004F31 File Offset: 0x00003131
		public void SetColor(Color color)
		{
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x00004F31 File Offset: 0x00003131
		public void SetColorMap(Texture2D tex)
		{
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x0011453C File Offset: 0x0011273C
		public override void StartEffects(DynamicMudSurface surface, MudType thickness, bool overrideThickness = false)
		{
			if ((this.RequiredThickness & thickness) == (MudType)0 && !overrideThickness)
			{
				return;
			}
			if (this.WaterySplashParticleEffect)
			{
				this.WaterySplashParticleEffect.transform.rotation = Quaternion.identity;
				this.WaterySplashParticleEffect.transform.position = this.impactPoint;
				this.WaterySplashParticleEffect.Play();
				this.WaterySplashParticleEffect.Emit(25);
			}
			if (this.StonesScatterParticleEffect)
			{
				this.StonesScatterParticleEffect.transform.rotation = Quaternion.identity;
				this.StonesScatterParticleEffect.transform.position = this.impactPoint;
				this.StonesScatterParticleEffect.Play();
				this.StonesScatterParticleEffect.Emit(50);
			}
			if (this.MudBlobsSplatter)
			{
				this.MudBlobsSplatter.Splatter(32, this.impactPoint);
			}
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x00004F31 File Offset: 0x00003131
		public override void StopEffects()
		{
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x00114618 File Offset: 0x00112818
		private void InitiateImpact(GameObject[] impactEffects)
		{
			foreach (GameObject gameObject in impactEffects)
			{
				gameObject.SetActive(true);
				gameObject.transform.rotation = Quaternion.identity;
				gameObject.transform.position = this.impactPoint;
				VisualEffect visualEffect;
				if (gameObject.TryGetComponent<VisualEffect>(out visualEffect))
				{
					visualEffect.Play();
				}
				ParticleSystem particleSystem;
				if (gameObject.TryGetComponent<ParticleSystem>(out particleSystem))
				{
					particleSystem.Play();
				}
			}
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x00114680 File Offset: 0x00112880
		private bool StraightHit()
		{
			float num = Vector3.Dot(Vector3.down, this.impactDirection);
			Debug.Log(num);
			return Mathf.Abs(num) > 0.8f;
		}

		// Token: 0x04003189 RID: 12681
		[SerializeField]
		private MudType targetThickness = MudType.Watery;

		// Token: 0x0400318A RID: 12682
		[SerializeField]
		private ParticleSystem WaterySplashParticleEffect;

		// Token: 0x0400318B RID: 12683
		[SerializeField]
		private ParticleSystem StonesScatterParticleEffect;

		// Token: 0x0400318C RID: 12684
		[SerializeField]
		private SplatterMudBlobs MudBlobsSplatter;
	}
}
