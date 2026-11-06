using System;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000017 RID: 23
	[RequireComponent(typeof(BoxCollider))]
	public class SpeedZone : MonoBehaviour
	{
		// Token: 0x060000CF RID: 207 RVA: 0x000078C3 File Offset: 0x00005AC3
		private void Awake()
		{
			base.GetComponent<BoxCollider>().isTrigger = true;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000078D4 File Offset: 0x00005AD4
		private void OnTriggerEnter(Collider other)
		{
			VehicleAI component = other.GetComponent<VehicleAI>();
			if (component != null)
			{
				component.SetSpeedZoneLimit(this.speedLimit);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00007900 File Offset: 0x00005B00
		private void OnTriggerExit(Collider other)
		{
			VehicleAI component = other.GetComponent<VehicleAI>();
			if (component != null)
			{
				component.ClearSpeedZoneLimit();
			}
		}

		// Token: 0x040000D1 RID: 209
		[Tooltip("Speed limit applied inside this zone (m/s). Overrides both cruise speed and track limit if lower.")]
		[Range(0.5f, 60f)]
		public float speedLimit = 5f;
	}
}
