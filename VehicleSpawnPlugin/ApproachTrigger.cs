using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000012 RID: 18
	[RequireComponent(typeof(BoxCollider))]
	public class ApproachTrigger : MonoBehaviour
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00007240 File Offset: 0x00005440
		private void Awake()
		{
			base.GetComponent<BoxCollider>().isTrigger = true;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000724E File Offset: 0x0000544E
		private void Start()
		{
			if (this.intersection == null)
			{
				Debug.LogWarning("[VSP] ApproachTrigger '" + base.name + "' has no IntersectionZone assigned.");
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00007278 File Offset: 0x00005478
		public void OnPhaseGreen()
		{
			this._isGreen = true;
			foreach (VehicleAI vehicleAI in this._waiting)
			{
				if (vehicleAI != null)
				{
					vehicleAI.SetBlocked(false);
				}
			}
			this._waiting.Clear();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000072E8 File Offset: 0x000054E8
		public void OnPhaseYellow()
		{
			this._isGreen = false;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000072F4 File Offset: 0x000054F4
		private void OnTriggerEnter(Collider other)
		{
			VehicleAI component = other.GetComponent<VehicleAI>();
			if (component == null)
			{
				return;
			}
			this._inside.Add(component);
			if (this.intersection == null)
			{
				return;
			}
			this.intersection.Enqueue(this);
			if (!this.intersection.CanEnter(this) || !this._isGreen)
			{
				this._waiting.Add(component);
				component.SetBlocked(true);
				if (this.intersection.intersectionType == IntersectionType.StopSign || this.intersection.intersectionType == IntersectionType.Yield)
				{
					base.StartCoroutine(this.GrantWhenClearRetry(component));
					return;
				}
			}
			else
			{
				this.GrantAccess(component);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000739C File Offset: 0x0000559C
		private void OnTriggerExit(Collider other)
		{
			VehicleAI component = other.GetComponent<VehicleAI>();
			if (component == null)
			{
				return;
			}
			this._inside.Remove(component);
			this._waiting.Remove(component);
			component.SetBlocked(false);
			if (this.intersection != null)
			{
				this.intersection.Dequeue(this);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000073F5 File Offset: 0x000055F5
		private void GrantAccess(VehicleAI vehicle)
		{
			vehicle.SetBlocked(false);
			IntersectionZone intersectionZone = this.intersection;
			if (intersectionZone == null)
			{
				return;
			}
			intersectionZone.NotifyCrossing(this);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000740F File Offset: 0x0000560F
		private IEnumerator GrantWhenClearRetry(VehicleAI vehicle)
		{
			if (this.intersection.intersectionType == IntersectionType.StopSign)
			{
				yield return VehicleSimulationPause.WaitForSimulationSeconds(this.intersection.stopHoldSeconds);
			}
			while (this._waiting.Contains(vehicle))
			{
				if (vehicle == null)
				{
					yield break;
				}
				if (this.intersection.CanEnter(this) && this._isGreen)
				{
					this._waiting.Remove(vehicle);
					this.GrantAccess(vehicle);
					yield break;
				}
				yield return VehicleSimulationPause.WaitForSimulationSeconds(0.3f);
			}
			yield break;
		}

		// Token: 0x040000B7 RID: 183
		[Tooltip("The IntersectionZone that manages this approach.")]
		public IntersectionZone intersection;

		// Token: 0x040000B8 RID: 184
		[Tooltip("Crawl speed applied to vehicles waiting in this approach (m/s).")]
		[Range(0f, 3f)]
		public float approachCrawlSpeed;

		// Token: 0x040000B9 RID: 185
		private bool _isGreen = true;

		// Token: 0x040000BA RID: 186
		private readonly HashSet<VehicleAI> _inside = new HashSet<VehicleAI>();

		// Token: 0x040000BB RID: 187
		private readonly HashSet<VehicleAI> _waiting = new HashSet<VehicleAI>();
	}
}
