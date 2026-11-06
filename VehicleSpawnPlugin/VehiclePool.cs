using System;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000005 RID: 5
	public class VehiclePool
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00004BAB File Offset: 0x00002DAB
		public int ActiveCount
		{
			get
			{
				return this._active.Count;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00004BB8 File Offset: 0x00002DB8
		public Transform Root
		{
			get
			{
				return this._poolRoot;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004BC0 File Offset: 0x00002DC0
		public VehiclePool(Transform poolRoot)
		{
			this._poolRoot = poolRoot;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004BE8 File Offset: 0x00002DE8
		public void Initialize(SpawnProfile profile)
		{
			if (profile.vehicleTypes == null)
			{
				return;
			}
			foreach (VehicleDefinition vehicleDefinition in profile.vehicleTypes)
			{
				if (!(vehicleDefinition == null) && !(vehicleDefinition.prefab == null) && !this._available.ContainsKey(vehicleDefinition))
				{
					Stack<GameObject> stack = new Stack<GameObject>(vehicleDefinition.poolSize);
					for (int i = 0; i < vehicleDefinition.poolSize; i++)
					{
						GameObject gameObject = Object.Instantiate<GameObject>(vehicleDefinition.prefab, this._poolRoot);
						gameObject.name = string.Format("{0}_pooled_{1}", vehicleDefinition.vehicleId, i);
						gameObject.SetActive(false);
						stack.Push(gameObject);
					}
					this._available[vehicleDefinition] = stack;
				}
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004CD8 File Offset: 0x00002ED8
		public GameObject Spawn(VehicleDefinition def, Vector3 position, Quaternion rotation)
		{
			Stack<GameObject> stack;
			if (!this._available.TryGetValue(def, out stack) || stack.Count == 0)
			{
				return null;
			}
			GameObject gameObject = stack.Pop();
			gameObject.transform.SetPositionAndRotation(position, rotation);
			gameObject.SetActive(true);
			this._active[gameObject] = def;
			return gameObject;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004D28 File Offset: 0x00002F28
		public void Despawn(GameObject go)
		{
			VehicleDefinition vehicleDefinition;
			if (!this._active.TryGetValue(go, out vehicleDefinition))
			{
				return;
			}
			if (VspDiag.Carry)
			{
				VehicleAI component = go.GetComponent<VehicleAI>();
				bool flag = component != null && component.HasPassenger;
				bool flag2 = false;
				for (int i = 0; i < go.transform.childCount; i++)
				{
					if (go.transform.GetChild(i).CompareTag("Player"))
					{
						flag2 = true;
						break;
					}
				}
				if (flag || flag2)
				{
					Debug.LogError(string.Format("[CarryDiag] VehiclePool.Despawn SetActive(false) on '{0}' with HasPassenger={1} playerStillParented={2} frame={3}", new object[]
					{
						go.name,
						flag,
						flag2,
						Time.frameCount
					}), go);
				}
				else
				{
					Debug.Log(string.Format("[CarryDiag] VehiclePool.Despawn '{0}' → inactive, frame={1}", go.name, Time.frameCount));
				}
			}
			go.SetActive(false);
			go.transform.SetParent(this._poolRoot);
			this._active.Remove(go);
			this._available[vehicleDefinition].Push(go);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004E40 File Offset: 0x00003040
		public void DespawnAll()
		{
			foreach (GameObject gameObject in new List<GameObject>(this._active.Keys))
			{
				this.Despawn(gameObject);
			}
		}

		// Token: 0x0400005F RID: 95
		private readonly Transform _poolRoot;

		// Token: 0x04000060 RID: 96
		private readonly Dictionary<VehicleDefinition, Stack<GameObject>> _available = new Dictionary<VehicleDefinition, Stack<GameObject>>();

		// Token: 0x04000061 RID: 97
		private readonly Dictionary<GameObject, VehicleDefinition> _active = new Dictionary<GameObject, VehicleDefinition>();
	}
}
