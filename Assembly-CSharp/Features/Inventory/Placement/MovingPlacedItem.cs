using System;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Inventory.Placement
{
	// Token: 0x02001142 RID: 4418
	[DisallowMultipleComponent]
	public sealed class MovingPlacedItem : MonoBehaviour
	{
		// Token: 0x06008620 RID: 34336 RVA: 0x0036BB10 File Offset: 0x00369D10
		public void Initialize(Transform support)
		{
			this._support = support;
			this._vehicle = ((support != null) ? support.GetComponentInParent<VehicleAI>() : null);
			this._spawnGeneration = ((this._vehicle != null) ? this._vehicle.SpawnGeneration : 0U);
			foreach (Rigidbody rigidbody in base.GetComponentsInChildren<Rigidbody>(true))
			{
				rigidbody.isKinematic = true;
				rigidbody.useGravity = false;
			}
		}

		// Token: 0x06008621 RID: 34337 RVA: 0x0036BB84 File Offset: 0x00369D84
		private void OnEnable()
		{
			if (Application.isPlaying && this._vehicle != null && this._vehicle.SpawnGeneration != this._spawnGeneration)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06008622 RID: 34338 RVA: 0x0036BBB9 File Offset: 0x00369DB9
		private void OnDisable()
		{
			if (Application.isPlaying && this._support != null && !this._support.gameObject.activeInHierarchy)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x040088C9 RID: 35017
		private Transform _support;

		// Token: 0x040088CA RID: 35018
		private VehicleAI _vehicle;

		// Token: 0x040088CB RID: 35019
		private uint _spawnGeneration;
	}
}
