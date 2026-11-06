using System;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000016 RID: 22
	[RequireComponent(typeof(BoxCollider))]
	public class PedestrianZone : MonoBehaviour
	{
		// Token: 0x060000CA RID: 202 RVA: 0x000076A9 File Offset: 0x000058A9
		private void Awake()
		{
			base.GetComponent<BoxCollider>().isTrigger = true;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000076B8 File Offset: 0x000058B8
		private void OnTriggerEnter(Collider other)
		{
			VehicleAI component = other.GetComponent<VehicleAI>();
			if (component != null)
			{
				this._vehicles.Add(component);
				component.SetSpeedZoneLimit(this.crawlSpeed);
				if (this._pedestrians.Count > 0)
				{
					component.SetBlocked(true);
				}
				return;
			}
			if (this.IsPedestrianTag(other.gameObject.tag))
			{
				this._pedestrians.Add(other);
				foreach (VehicleAI vehicleAI in this._vehicles)
				{
					if (vehicleAI != null)
					{
						vehicleAI.SetBlocked(true);
					}
				}
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00007774 File Offset: 0x00005974
		private void OnTriggerExit(Collider other)
		{
			VehicleAI component = other.GetComponent<VehicleAI>();
			if (component != null)
			{
				this._vehicles.Remove(component);
				component.ClearSpeedZoneLimit();
				component.SetBlocked(false);
				return;
			}
			if (this.IsPedestrianTag(other.gameObject.tag))
			{
				this._pedestrians.Remove(other);
				if (this._pedestrians.Count == 0)
				{
					foreach (VehicleAI vehicleAI in this._vehicles)
					{
						if (vehicleAI != null)
						{
							vehicleAI.SetBlocked(false);
						}
					}
				}
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00007828 File Offset: 0x00005A28
		private bool IsPedestrianTag(string tag)
		{
			using (List<string>.Enumerator enumerator = this.pedestrianTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == tag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040000CD RID: 205
		[Tooltip("Speed applied to vehicles inside the zone when no pedestrian is present (m/s).")]
		[Range(0.5f, 10f)]
		public float crawlSpeed = 3f;

		// Token: 0x040000CE RID: 206
		[Tooltip("Tags treated as pedestrians in this zone (e.g. Pedestrian, Player, NPC).")]
		public List<string> pedestrianTags = new List<string> { "Pedestrian" };

		// Token: 0x040000CF RID: 207
		private readonly HashSet<VehicleAI> _vehicles = new HashSet<VehicleAI>();

		// Token: 0x040000D0 RID: 208
		private readonly HashSet<Collider> _pedestrians = new HashSet<Collider>();
	}
}
