using System;
using System.Collections.Generic;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000BB1 RID: 2993
	[DisallowMultipleComponent]
	public class VehiclePoolWarmup : MonoBehaviour
	{
		// Token: 0x060055E0 RID: 21984 RVA: 0x00215245 File Offset: 0x00213445
		private void Awake()
		{
			if (this.spawner == null)
			{
				this.spawner = base.GetComponent<VehicleSpawner>();
			}
		}

		// Token: 0x060055E1 RID: 21985 RVA: 0x00215264 File Offset: 0x00213464
		private void Update()
		{
			if (this._done)
			{
				return;
			}
			if (this.spawner == null || this.spawner.Profile == null)
			{
				return;
			}
			if (ChunkManager.Instance == null || !ChunkManager.Instance.enabled)
			{
				return;
			}
			List<GameObject> list = VehiclePoolWarmup.CollectVehiclePrefabs(this.spawner.Profile);
			if (list.Count == 0)
			{
				this._done = true;
				return;
			}
			ChunkManager instance = ChunkManager.Instance;
			if (!instance.WarmRenderStarted)
			{
				bool flag = instance.RegisterWarmRenderPrefabs(list);
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[VSP-Warmup] Registrati {0} prefab veicolo al warm render mappa (ok={1}).", list.Count, flag));
				}
			}
			else
			{
				instance.StartCoroutine(instance.WarmRenderExtraPrefabsNow(list));
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[VSP-Warmup] Warm render mappa già passato → fallback one-shot per {0} prefab veicolo.", list.Count));
				}
			}
			this._done = true;
		}

		// Token: 0x060055E2 RID: 21986 RVA: 0x00215350 File Offset: 0x00213550
		private static List<GameObject> CollectVehiclePrefabs(SpawnProfile profile)
		{
			List<GameObject> list = new List<GameObject>();
			if (profile.vehicleTypes == null)
			{
				return list;
			}
			foreach (VehicleDefinition vehicleDefinition in profile.vehicleTypes)
			{
				if (!(vehicleDefinition == null) && !(vehicleDefinition.prefab == null) && !list.Contains(vehicleDefinition.prefab))
				{
					list.Add(vehicleDefinition.prefab);
				}
			}
			return list;
		}

		// Token: 0x04005641 RID: 22081
		[Tooltip("Spawner da cui leggere i prefab veicolo (via SpawnProfile). Se vuoto viene risolto sullo stesso GameObject in Awake.")]
		[SerializeField]
		private VehicleSpawner spawner;

		// Token: 0x04005642 RID: 22082
		[Tooltip("Log diagnostici (registrazione warm / fallback).")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005643 RID: 22083
		private bool _done;
	}
}
