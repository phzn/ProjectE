using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x0200000C RID: 12
	public class CameraZone : MonoBehaviour
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002D4D File Offset: 0x00000F4D
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002D54 File Offset: 0x00000F54
		public static List<CameraZone> Instances { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002D5C File Offset: 0x00000F5C
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002D64 File Offset: 0x00000F64
		public VisibilityTree VisibilityTree { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002D6D File Offset: 0x00000F6D
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00002D75 File Offset: 0x00000F75
		public int CellsCount { get; private set; }

		// Token: 0x0600003B RID: 59 RVA: 0x00002D7E File Offset: 0x00000F7E
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ReloadDomain()
		{
			if (CameraZone.Instances != null)
			{
				CameraZone.Instances.Clear();
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002D91 File Offset: 0x00000F91
		private void Awake()
		{
			if (CameraZone.Instances == null)
			{
				CameraZone.Instances = new List<CameraZone>();
			}
			CameraZone.Instances.Add(this);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002DAF File Offset: 0x00000FAF
		private void OnDestroy()
		{
			CameraZone.Instances.Remove(this);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002DC0 File Offset: 0x00000FC0
		public bool CreateVisibilityTree(float cellSize)
		{
			if (cellSize < 0.01f)
			{
				Debug.Log("Unable to create VisibilityTree with so small cell size : " + cellSize.ToString());
				return false;
			}
			if (this.VisibilityTree != null)
			{
				this.ClearVisibilityTree();
			}
			Vector3 position = base.transform.position;
			Vector3 lossyScale = base.transform.lossyScale;
			lossyScale.x = Mathf.Abs(lossyScale.x);
			lossyScale.y = Mathf.Abs(lossyScale.y);
			lossyScale.z = Mathf.Abs(lossyScale.z);
			int num = Mathf.CeilToInt(lossyScale.x / cellSize);
			int num2 = Mathf.CeilToInt(lossyScale.y / cellSize);
			int num3 = Mathf.CeilToInt(lossyScale.z / cellSize);
			if (num == 0 || num2 == 0 || num3 == 0)
			{
				Debug.Log("Unable to create VisibilityTree with side size equals zero");
				return false;
			}
			try
			{
				this.VisibilityTree = new VisibilityTree(cellSize);
				Vector3 vector = position - lossyScale / 2f;
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						for (int k = 0; k < num3; k++)
						{
							Vector3 vector2 = new Vector3
							{
								x = cellSize * (float)i,
								y = cellSize * (float)j,
								z = cellSize * (float)k
							};
							this.VisibilityTree.Add(vector + vector2);
						}
					}
				}
				this.CellsCount = num * num2 * num3;
			}
			catch (Exception ex)
			{
				Debug.Log("Unable to create VisibilityTree, reason : " + ex.Message + ex.StackTrace);
				return false;
			}
			return true;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002F68 File Offset: 0x00001168
		public void ClearVisibilityTree()
		{
			this.VisibilityTree = null;
			this.CellsCount = 0;
		}
	}
}
