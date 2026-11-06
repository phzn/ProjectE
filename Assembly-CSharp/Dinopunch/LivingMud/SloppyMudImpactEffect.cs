using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200075F RID: 1887
	public class SloppyMudImpactEffect : LivingMudEffectsPlayer, ISetColorMap, IUseImpactData
	{
		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060030C1 RID: 12481 RVA: 0x001130DE File Offset: 0x001112DE
		public override MudType RequiredThickness
		{
			get
			{
				return this.targetThickness;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060030C2 RID: 12482 RVA: 0x001130E6 File Offset: 0x001112E6
		// (set) Token: 0x060030C3 RID: 12483 RVA: 0x001130EE File Offset: 0x001112EE
		public float mass { get; set; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060030C4 RID: 12484 RVA: 0x001130F7 File Offset: 0x001112F7
		// (set) Token: 0x060030C5 RID: 12485 RVA: 0x001130FF File Offset: 0x001112FF
		public float speed { get; set; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060030C6 RID: 12486 RVA: 0x00113108 File Offset: 0x00111308
		// (set) Token: 0x060030C7 RID: 12487 RVA: 0x00113110 File Offset: 0x00111310
		public Vector3 impactDirection { get; set; }

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060030C8 RID: 12488 RVA: 0x00113119 File Offset: 0x00111319
		// (set) Token: 0x060030C9 RID: 12489 RVA: 0x00113121 File Offset: 0x00111321
		public Vector3 impactPoint { get; set; }

		// Token: 0x060030CA RID: 12490 RVA: 0x0011312A File Offset: 0x0011132A
		public void SetColor(Color color)
		{
			Debug.Log("Set Color");
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x00113136 File Offset: 0x00111336
		public void SetColorMap(Texture2D tex)
		{
			Debug.Log("Set Color Map");
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x00113144 File Offset: 0x00111344
		public override void StartEffects(DynamicMudSurface surface, MudType thickness, bool overrideThickness = false)
		{
			if ((this.RequiredThickness & thickness) == (MudType)0 && !overrideThickness)
			{
				return;
			}
			if (this.mudChunksParticleEffect)
			{
				this.mudChunksParticleEffect.transform.rotation = Quaternion.identity;
				this.mudChunksParticleEffect.transform.position = this.impactPoint;
				this.mudChunksParticleEffect.Play();
				this.mudChunksParticleEffect.Emit(250);
			}
			if (this.MudBlobsSplatter)
			{
				this.MudBlobsSplatter.Splatter(64, this.impactPoint);
			}
			if (this.crownSplash)
			{
				this.crownSplash.gameObject.SetActive(true);
				this.crownSplash.gameObject.transform.rotation = Quaternion.identity;
				this.crownSplash.gameObject.transform.position = this.impactPoint;
				this.crownSplash.Play();
			}
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x00113230 File Offset: 0x00111430
		public override void StopEffects()
		{
			Debug.Log("Out of mud");
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x0011323C File Offset: 0x0011143C
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

		// Token: 0x060030CF RID: 12495 RVA: 0x001132A4 File Offset: 0x001114A4
		private bool StraightHit()
		{
			float num = Vector3.Dot(Vector3.down, this.impactDirection);
			Debug.Log(num);
			return Mathf.Abs(num) > 0.8f;
		}

		// Token: 0x04003133 RID: 12595
		[SerializeField]
		private MudType targetThickness = MudType.Sloppy;

		// Token: 0x04003134 RID: 12596
		[SerializeField]
		private VisualEffect crownSplash;

		// Token: 0x04003135 RID: 12597
		[SerializeField]
		private VisualEffect directionalSplash;

		// Token: 0x04003136 RID: 12598
		[SerializeField]
		private ParticleSystem mudChunksParticleEffect;

		// Token: 0x04003137 RID: 12599
		[SerializeField]
		private SplatterMudBlobs MudBlobsSplatter;
	}
}
