using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000770 RID: 1904
	public class BikePedalController : MonoBehaviour
	{
		// Token: 0x06003198 RID: 12696 RVA: 0x0011733F File Offset: 0x0011553F
		private void Awake()
		{
			if (this.bicycle == null)
			{
				this.bicycle = base.GetComponent<BicycleVehicle>();
			}
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x0011735C File Offset: 0x0011555C
		private void Update()
		{
			if (this.pedalAnimator == null || this.bicycle == null)
			{
				return;
			}
			if (this.bicycle.isInControl && this.bicycle.verticalInput > 0.01f && this.bicycle.currentSpeed > this.minSpeedForPedaling && !this.bicycle.braking)
			{
				float num = this.bicycle.currentSpeed * this.pedalSpeedMultiplier;
				this.pedalAnimator.speed = Mathf.Clamp(num, 0.5f, 3f);
				if (!this.pedalAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pedals_Rotate"))
				{
					this.pedalAnimator.Play("Pedals_Rotate");
					return;
				}
			}
			else
			{
				this.pedalAnimator.speed = 0f;
			}
		}

		// Token: 0x04003217 RID: 12823
		[Header("References")]
		[Tooltip("The Animator component controlling the pedals")]
		public Animator pedalAnimator;

		// Token: 0x04003218 RID: 12824
		[Tooltip("Reference to the BicycleVehicle")]
		public BicycleVehicle bicycle;

		// Token: 0x04003219 RID: 12825
		[Header("Settings")]
		[Tooltip("Minimum speed to start pedal animation")]
		public float minSpeedForPedaling = 0.5f;

		// Token: 0x0400321A RID: 12826
		[Tooltip("Speed multiplier for pedal animation")]
		public float pedalSpeedMultiplier = 1f;
	}
}
