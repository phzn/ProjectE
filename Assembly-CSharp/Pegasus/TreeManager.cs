using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001C1 RID: 449
	public class TreeManager
	{
		// Token: 0x06000D5B RID: 3419 RVA: 0x0005C230 File Offset: 0x0005A430
		public void LoadTreesFromTerrain()
		{
			this.m_terrainTrees = null;
			this.m_terrainTreeLocations = null;
			float num = float.NaN;
			float num2 = float.NaN;
			float num3 = float.NaN;
			float num4 = float.NaN;
			float num5 = float.NaN;
			Terrain terrain = null;
			foreach (Terrain terrain2 in Terrain.activeTerrains)
			{
				if (float.IsNaN(num))
				{
					terrain = terrain2;
					num = terrain2.transform.position.y;
					num2 = terrain2.transform.position.x;
					num4 = terrain2.transform.position.z;
					num3 = num2 + terrain2.terrainData.size.x;
					num5 = num4 + terrain2.terrainData.size.z;
				}
				else
				{
					if (terrain2.transform.position.x < num2)
					{
						num2 = terrain2.transform.position.x;
					}
					if (terrain2.transform.position.z < num4)
					{
						num4 = terrain2.transform.position.z;
					}
					if (terrain2.transform.position.x + terrain2.terrainData.size.x > num3)
					{
						num3 = terrain2.transform.position.x + terrain2.terrainData.size.x;
					}
					if (terrain2.transform.position.z + terrain2.terrainData.size.z > num5)
					{
						num5 = terrain2.transform.position.z + terrain2.terrainData.size.z;
					}
				}
			}
			if (terrain != null)
			{
				Rect rect = new Rect(num2, num4, num3 - num2, num5 - num4);
				this.m_terrainTreeLocations = new Quadtree<TreeManager.TreeStruct>(rect, 32);
				this.m_terrainTrees = new List<TreePrototype>(terrain.terrainData.treePrototypes);
				foreach (Terrain terrain3 in Terrain.activeTerrains)
				{
					float x = terrain3.transform.position.x;
					float y = terrain3.transform.position.y;
					float z = terrain3.transform.position.z;
					float x2 = terrain3.terrainData.size.x;
					float y2 = terrain3.terrainData.size.y;
					float z2 = terrain3.terrainData.size.z;
					TreeInstance[] treeInstances = terrain3.terrainData.treeInstances;
					for (int j = 0; j < treeInstances.Length; j++)
					{
						TreeInstance treeInstance = treeInstances[j];
						TreeManager.TreeStruct treeStruct = default(TreeManager.TreeStruct);
						treeStruct.position = new Vector3(x + treeInstance.position.x * x2, y + treeInstance.position.y * y2, z + treeInstance.position.z * z2);
						treeStruct.prototypeID = treeInstances[j].prototypeIndex;
						this.m_terrainTreeLocations.Insert(treeStruct.position.x, treeStruct.position.z, treeStruct);
					}
				}
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0005C56F File Offset: 0x0005A76F
		public void AddTree(TreeManager.TreeStruct tree)
		{
			if (this.m_terrainTreeLocations == null)
			{
				return;
			}
			this.m_terrainTreeLocations.Insert(tree.position.x, tree.position.z, tree);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0005C5A0 File Offset: 0x0005A7A0
		public int Count(Vector3 position, float range)
		{
			if (this.m_terrainTreeLocations == null)
			{
				return 0;
			}
			Rect rect = new Rect(position.x - range, position.z - range, range * 2f, range * 2f);
			return this.m_terrainTreeLocations.Find(rect).Count<TreeManager.TreeStruct>();
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0005C5ED File Offset: 0x0005A7ED
		public int Count()
		{
			if (this.m_terrainTreeLocations == null)
			{
				return 0;
			}
			return this.m_terrainTreeLocations.Count;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0005C604 File Offset: 0x0005A804
		public int GetTrees(Vector3 position, float range, ref List<TreeManager.TreeStruct> treeList)
		{
			treeList.Clear();
			if (this.m_terrainTreeLocations == null)
			{
				return 0;
			}
			Rect rect = new Rect(position.x - range, position.z - range, range * 2f, range * 2f);
			treeList.AddRange(this.m_terrainTreeLocations.Find(rect));
			return treeList.Count;
		}

		// Token: 0x04000DA6 RID: 3494
		private List<TreePrototype> m_terrainTrees = new List<TreePrototype>();

		// Token: 0x04000DA7 RID: 3495
		private Quadtree<TreeManager.TreeStruct> m_terrainTreeLocations = new Quadtree<TreeManager.TreeStruct>(new Rect(0f, 0f, 10f, 10f), 32);

		// Token: 0x020001C2 RID: 450
		public struct TreeStruct
		{
			// Token: 0x04000DA8 RID: 3496
			public Vector3 position;

			// Token: 0x04000DA9 RID: 3497
			public int prototypeID;
		}
	}
}
