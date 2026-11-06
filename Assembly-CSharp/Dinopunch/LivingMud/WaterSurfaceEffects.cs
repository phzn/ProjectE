using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200076A RID: 1898
	[RequireComponent(typeof(Collider))]
	public class WaterSurfaceEffects : MonoBehaviour
	{
		// Token: 0x0600311B RID: 12571 RVA: 0x0011432C File Offset: 0x0011252C
		private void Start()
		{
			this.myCollider = base.GetComponent<Collider>();
			if (this.EffectColor.a < 0.1f)
			{
				Debug.LogWarning(string.Format("The Water Surface effect color alpha is {0}, is this what you want?", this.EffectColor.a));
			}
			this.SplashSurfaceParticleSystem.main.startColor = this.EffectColor;
			this.WaterSplashParticleSystem.main.startColor = this.EffectColor;
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x001143B4 File Offset: 0x001125B4
		private void OnTriggerEnter(Collider other)
		{
			float y = base.transform.position.y;
			Vector3 vector = new Vector3(other.transform.position.x, y, other.transform.position.z);
			this.SplashSurfaceParticleSystem.transform.position = vector;
			this.WaterSplashParticleSystem.transform.position = vector;
			this.SplashSurfaceParticleSystem.Play();
			this.WaterSplashParticleSystem.Play();
			this.SplashSurfaceParticleSystem.Emit(5);
			this.WaterSplashParticleSystem.Emit(10);
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x0011444C File Offset: 0x0011264C
		private void OnTriggerStay(Collider other)
		{
			this.timeSinceEffect += Time.fixedDeltaTime;
			if (this.timeSinceEffect < this.secondsBetweenEffect)
			{
				return;
			}
			this.timeSinceEffect = 0f;
			float y = base.transform.position.y;
			Vector3 vector = new Vector3(other.transform.position.x, y, other.transform.position.z);
			this.SplashSurfaceParticleSystem.transform.position = vector;
			this.SplashSurfaceParticleSystem.Emit(1);
		}

		// Token: 0x04003183 RID: 12675
		[SerializeField]
		private ParticleSystem SplashSurfaceParticleSystem;

		// Token: 0x04003184 RID: 12676
		[SerializeField]
		private ParticleSystem WaterSplashParticleSystem;

		// Token: 0x04003185 RID: 12677
		[SerializeField]
		private Color EffectColor;

		// Token: 0x04003186 RID: 12678
		[SerializeField]
		private float secondsBetweenEffect = 0.1f;

		// Token: 0x04003187 RID: 12679
		private float timeSinceEffect;

		// Token: 0x04003188 RID: 12680
		private Collider myCollider;
	}
}
