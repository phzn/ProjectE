using System;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class vParticleEmissionByVelocity : MonoBehaviour
{
	// Token: 0x060009C7 RID: 2503 RVA: 0x00044F58 File Offset: 0x00043158
	private void Start()
	{
		this.emmision = this.particle.emission;
		this.rate = this.emmision.rateOverTime.constant;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00044F90 File Offset: 0x00043190
	private void FixedUpdate()
	{
		Vector3 velocity = this.rigidbodyReference.velocity;
		velocity.y = 0f;
		float magnitude = velocity.magnitude;
		float num;
		if (this.normalizeVelocity)
		{
			num = velocity.normalized.magnitude * Mathf.Clamp(magnitude, 0f, 1f);
		}
		else
		{
			num = magnitude;
		}
		if (this.inverse)
		{
			this.emmision.rateOverTime = this.rate - this.rate * num;
			return;
		}
		this.emmision.rateOverTime = this.rate * num;
	}

	// Token: 0x0400091B RID: 2331
	public Rigidbody rigidbodyReference;

	// Token: 0x0400091C RID: 2332
	public ParticleSystem particle;

	// Token: 0x0400091D RID: 2333
	private ParticleSystem.EmissionModule emmision;

	// Token: 0x0400091E RID: 2334
	public bool normalizeVelocity = true;

	// Token: 0x0400091F RID: 2335
	public bool inverse;

	// Token: 0x04000920 RID: 2336
	private float rate;
}
