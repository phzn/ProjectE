using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200004E RID: 78
	public class GPUISpaceshipController : GPUIInputHandler
	{
		// Token: 0x0600029A RID: 666 RVA: 0x0000EC2C File Offset: 0x0000CE2C
		private void Awake()
		{
			this.shipRigidbody = base.GetComponent<Rigidbody>();
			this.engineThrusterEmission = base.transform.GetChild(0).GetComponent<ParticleSystem>().emission;
			this.originalThrusterEmissionRate = this.engineThrusterEmission.rateOverTime.constant;
			this.engineGlowEmission = base.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>()
				.emission;
			this.originalGlowEmissionRate = this.engineGlowEmission.rateOverTime.constant;
			Transform transform = base.transform.Find("EngineGlowLight");
			if (transform)
			{
				this.engineGlowLight = transform.GetComponent<Light>();
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000ECDA File Offset: 0x0000CEDA
		private void FixedUpdate()
		{
			this.GetInputs();
			this.Move();
			this.AdjustThrusterEffects();
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000ECF0 File Offset: 0x0000CEF0
		private void GetInputs()
		{
			this.yawInput = base.GetAxis("Horizontal");
			this.thrustInput = base.GetAxis("Jump");
			this.pitchInput = base.GetAxis("Vertical");
			this.rollInput = (base.GetKey(KeyCode.Q) ? 1f : (base.GetKey(KeyCode.E) ? (-1f) : 0f));
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000ED60 File Offset: 0x0000CF60
		private void Move()
		{
			this.shipRigidbody.AddRelativeTorque(Vector3.up * this.yawInput * this.engineTorque * Time.deltaTime);
			this.shipRigidbody.AddRelativeTorque(Vector3.right * this.pitchInput * this.engineTorque * Time.deltaTime);
			this.shipRigidbody.AddRelativeTorque(Vector3.forward * this.rollInput * this.engineTorque * Time.deltaTime);
			this.shipRigidbody.AddRelativeForce(Vector3.forward * this.thrustInput * this.enginePower * Time.deltaTime);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000EE30 File Offset: 0x0000D030
		private void AdjustThrusterEffects()
		{
			this.engineThrusterEmission.rateOverTime = this.originalThrusterEmissionRate * this.thrustInput;
			this.engineGlowEmission.rateOverTime = Mathf.Lerp(0.5f * this.originalGlowEmissionRate, this.originalGlowEmissionRate, this.thrustInput);
			if (this.engineGlowLight)
			{
				this.engineGlowLight.intensity = Mathf.Clamp01(0.5f + this.thrustInput);
			}
		}

		// Token: 0x040001FF RID: 511
		public float engineTorque = 1500f;

		// Token: 0x04000200 RID: 512
		public float enginePower = 4500f;

		// Token: 0x04000201 RID: 513
		private Rigidbody shipRigidbody;

		// Token: 0x04000202 RID: 514
		private float rollInput;

		// Token: 0x04000203 RID: 515
		private float thrustInput;

		// Token: 0x04000204 RID: 516
		private float pitchInput;

		// Token: 0x04000205 RID: 517
		private float yawInput;

		// Token: 0x04000206 RID: 518
		private ParticleSystem.EmissionModule engineThrusterEmission;

		// Token: 0x04000207 RID: 519
		private ParticleSystem.EmissionModule engineGlowEmission;

		// Token: 0x04000208 RID: 520
		private Light engineGlowLight;

		// Token: 0x04000209 RID: 521
		private float originalThrusterEmissionRate;

		// Token: 0x0400020A RID: 522
		private float originalGlowEmissionRate;
	}
}
