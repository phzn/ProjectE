using System;
using RayznGames.VehicleSystem;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x020006FC RID: 1788
	[RequireComponent(typeof(BicycleVehicle))]
	public class BicycleSpeedProvider : MonoBehaviour, IVehicleSpeedProvider
	{
		// Token: 0x06002D6E RID: 11630 RVA: 0x000FCD04 File Offset: 0x000FAF04
		private void Awake()
		{
			this.bicycle = base.GetComponent<BicycleVehicle>();
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000FCD12 File Offset: 0x000FAF12
		public float GetCurrentSpeed()
		{
			if (!(this.bicycle != null))
			{
				return 0f;
			}
			return this.bicycle.currentSpeed;
		}

		// Token: 0x04002D39 RID: 11577
		private BicycleVehicle bicycle;
	}
}
