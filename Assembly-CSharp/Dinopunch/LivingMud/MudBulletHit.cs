using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000755 RID: 1877
	public class MudBulletHit : MonoBehaviour
	{
		// Token: 0x06003090 RID: 12432 RVA: 0x001124F0 File Offset: 0x001106F0
		public void SpawnBulletHit(Vector3 pos, DynamicMudSurface surface)
		{
			base.transform.position = pos;
			this.BulletVFX.Play();
			this.BulletStamp.SetSurface(surface);
			this.BulletStamp.StampSurface();
			this.MudParticles.Emit(50);
			this.SplashParticles.Play();
			this.SplashParticles.Emit(1);
		}

		// Token: 0x04003105 RID: 12549
		public VisualEffect BulletVFX;

		// Token: 0x04003106 RID: 12550
		public MudStamp BulletStamp;

		// Token: 0x04003107 RID: 12551
		public ParticleSystem MudParticles;

		// Token: 0x04003108 RID: 12552
		public ParticleSystem SplashParticles;
	}
}
