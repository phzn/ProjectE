using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200074A RID: 1866
	public class DryMudImpactEffect : LivingMudEffectsPlayer, ISetColorMap, IUseImpactData
	{
		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003056 RID: 12374 RVA: 0x00111D43 File Offset: 0x0010FF43
		public override MudType RequiredThickness
		{
			get
			{
				return this.targetThickness;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003057 RID: 12375 RVA: 0x00111D4B File Offset: 0x0010FF4B
		// (set) Token: 0x06003058 RID: 12376 RVA: 0x00111D53 File Offset: 0x0010FF53
		public float mass { get; set; }

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06003059 RID: 12377 RVA: 0x00111D5C File Offset: 0x0010FF5C
		// (set) Token: 0x0600305A RID: 12378 RVA: 0x00111D64 File Offset: 0x0010FF64
		public float speed { get; set; }

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x0600305B RID: 12379 RVA: 0x00111D6D File Offset: 0x0010FF6D
		// (set) Token: 0x0600305C RID: 12380 RVA: 0x00111D75 File Offset: 0x0010FF75
		public Vector3 impactDirection { get; set; }

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x0600305D RID: 12381 RVA: 0x00111D7E File Offset: 0x0010FF7E
		// (set) Token: 0x0600305E RID: 12382 RVA: 0x00111D86 File Offset: 0x0010FF86
		public Vector3 impactPoint { get; set; }

		// Token: 0x0600305F RID: 12383 RVA: 0x00004F31 File Offset: 0x00003131
		public void SetColor(Color color)
		{
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x00004F31 File Offset: 0x00003131
		public void SetColorMap(Texture2D tex)
		{
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x00111D90 File Offset: 0x0010FF90
		public override void StartEffects(DynamicMudSurface surface, MudType thickness, bool overrideThickness = false)
		{
			if ((this.RequiredThickness & thickness) == (MudType)0 && !overrideThickness)
			{
				return;
			}
			if (this.DirtSplashParticleEffect)
			{
				this.DirtSplashParticleEffect.transform.rotation = Quaternion.identity;
				this.DirtSplashParticleEffect.transform.position = this.impactPoint;
				this.DirtSplashParticleEffect.Play();
				this.DirtSplashParticleEffect.Emit(25);
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

		// Token: 0x06003062 RID: 12386 RVA: 0x00004F31 File Offset: 0x00003131
		public override void StopEffects()
		{
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x00111E6C File Offset: 0x0011006C
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

		// Token: 0x06003064 RID: 12388 RVA: 0x00111ED4 File Offset: 0x001100D4
		private bool StraightHit()
		{
			float num = Vector3.Dot(Vector3.down, this.impactDirection);
			Debug.Log(num);
			return Mathf.Abs(num) > 0.8f;
		}

		// Token: 0x040030EE RID: 12526
		[Tooltip("Mud Type that will trigger these effects")]
		[SerializeField]
		private MudType targetThickness = MudType.Dry;

		// Token: 0x040030EF RID: 12527
		[SerializeField]
		private ParticleSystem DirtSplashParticleEffect;

		// Token: 0x040030F0 RID: 12528
		[SerializeField]
		private ParticleSystem StonesScatterParticleEffect;

		// Token: 0x040030F1 RID: 12529
		[Tooltip("Reference to component that will spawn mud blobs on impact")]
		[SerializeField]
		private SplatterMudBlobs MudBlobsSplatter;
	}
}
