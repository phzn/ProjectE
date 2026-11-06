using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200077C RID: 1916
	public class UnicyclePedalController : MonoBehaviour
	{
		// Token: 0x06003233 RID: 12851 RVA: 0x0011C9D2 File Offset: 0x0011ABD2
		private void Awake()
		{
			if (this.unicycle == null)
			{
				this.unicycle = base.GetComponent<UnicycleVehicle>();
			}
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x0011C9F0 File Offset: 0x0011ABF0
		private void Update()
		{
			if (this.pedalAnimator == null || this.unicycle == null)
			{
				return;
			}
			if (this.unicycle.isInControl && this.unicycle.verticalInput > 0.01f && this.unicycle.currentSpeed > this.minSpeedForPedaling && !this.unicycle.braking)
			{
				float num = this.unicycle.currentSpeed * this.pedalSpeedMultiplier;
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

		// Token: 0x040032F4 RID: 13044
		[Header("References")]
		[Tooltip("The Animator component controlling the pedals")]
		public Animator pedalAnimator;

		// Token: 0x040032F5 RID: 13045
		[Tooltip("Reference to the UnicycleVehicle")]
		public UnicycleVehicle unicycle;

		// Token: 0x040032F6 RID: 13046
		[Header("Settings")]
		[Tooltip("Minimum speed to start pedal animation")]
		public float minSpeedForPedaling = 0.5f;

		// Token: 0x040032F7 RID: 13047
		[Tooltip("Speed multiplier for pedal animation")]
		public float pedalSpeedMultiplier = 1f;
	}
}
