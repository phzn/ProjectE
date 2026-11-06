using System;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x02000012 RID: 18
	public class GPUITreeChopper : GPUIInputHandler
	{
		// Token: 0x060000DB RID: 219 RVA: 0x000067C8 File Offset: 0x000049C8
		private void OnEnable()
		{
			if (this.gpuiTerrain == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Terrain is not assigned!");
				return;
			}
			if (this.treeManager == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Tree Manager is not assigned!");
				return;
			}
			this._terrainData = this.gpuiTerrain.GetComponent<Terrain>().terrainData;
			this._terrainCollider = this.gpuiTerrain.GetComponent<TerrainCollider>();
			this._treeCache = this._terrainData.treeInstances;
			this._currentTreeInstances = this._treeCache;
			this._treePrototypes = this._terrainData.treePrototypes;
			this._chopperCollider = base.GetComponent<Collider>();
			this.OnTriggerEnter(this._terrainCollider);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006888 File Offset: 0x00004A88
		private void Update()
		{
			if (base.GetKeyDown(KeyCode.Alpha1))
			{
				this.ResetTrees();
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000689A File Offset: 0x00004A9A
		private void OnDisable()
		{
			if (this._terrainData != null)
			{
				this._terrainData.treeInstances = this._treeCache;
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000068BB File Offset: 0x00004ABB
		private void ResetTrees()
		{
			GPUITerrainAPI.SetTreeInstances(this.gpuiTerrain, this._treeCache, true);
			this._currentTreeInstances = this._treeCache;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000068DC File Offset: 0x00004ADC
		private void OnTriggerEnter(Collider other)
		{
			if (other == this._terrainCollider)
			{
				Vector3 position = this.gpuiTerrain.GetPosition();
				int num = this._currentTreeInstances.Length;
				Bounds bounds = this._chopperCollider.bounds;
				for (int i = 0; i < num; i++)
				{
					Vector3 vector = Vector3.Scale(this._currentTreeInstances[i].position, this._terrainData.size) + position;
					if (bounds.Contains(vector))
					{
						this._removalBounds.center = vector;
						this.RemoveTreeFromTerrain(i);
						num--;
					}
				}
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006970 File Offset: 0x00004B70
		private void RemoveTreeFromTerrain(int treeIndex)
		{
			Debug.Log(string.Concat(new string[]
			{
				GPUIConstants.LOG_PREFIX,
				"Removing tree at index: ",
				treeIndex.ToString(),
				", position: ",
				this._removalBounds.center.ToString()
			}));
			TreeInstance treeInstance = this._currentTreeInstances[treeIndex];
			this._currentTreeInstances = this._currentTreeInstances.RemoveAtAndReturn<TreeInstance>(treeIndex);
			GPUITerrainAPI.SetTreeInstances(this.gpuiTerrain, this._currentTreeInstances, true);
			this.GenerateCutTree(this._treePrototypes[treeInstance.prototypeIndex].prefab, this._removalBounds.center, Quaternion.Euler(0f, treeInstance.rotation * 57.29578f, 0f));
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006A3C File Offset: 0x00004C3C
		private void GenerateCutTree(GameObject treePrefab, Vector3 position, Quaternion rotation)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(treePrefab, position, rotation);
			gameObject.AddComponent<GPUIObjectDestroyer>().timeToDestroy = 2f;
			Collider collider;
			if (gameObject.TryGetComponent<Collider>(out collider))
			{
				Object.Destroy(collider);
			}
			gameObject.AddComponent<Rigidbody>().AddForce(base.transform.forward * 5f, ForceMode.Impulse);
		}

		// Token: 0x040000A7 RID: 167
		public GPUITerrain gpuiTerrain;

		// Token: 0x040000A8 RID: 168
		public GPUITreeManager treeManager;

		// Token: 0x040000A9 RID: 169
		private TerrainData _terrainData;

		// Token: 0x040000AA RID: 170
		private TerrainCollider _terrainCollider;

		// Token: 0x040000AB RID: 171
		private Collider _chopperCollider;

		// Token: 0x040000AC RID: 172
		private Bounds _removalBounds = new Bounds(Vector3.zero, Vector3.one * 0.1f);

		// Token: 0x040000AD RID: 173
		private TreeInstance[] _treeCache;

		// Token: 0x040000AE RID: 174
		private TreePrototype[] _treePrototypes;

		// Token: 0x040000AF RID: 175
		private TreeInstance[] _currentTreeInstances;
	}
}
