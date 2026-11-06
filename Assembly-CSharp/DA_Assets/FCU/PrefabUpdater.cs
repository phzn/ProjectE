using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002D9 RID: 729
	[Serializable]
	public class PrefabUpdater : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600144E RID: 5198 RVA: 0x000835F8 File Offset: 0x000817F8
		public void UpdatePrefabs()
		{
			CancellationTokenSource cancellationTokenSource = this.monoBeh.CancellationTokenController.CreateNew(TokenType.Import);
			this.UpdatePrefabsAsync(cancellationTokenSource);
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x00083620 File Offset: 0x00081820
		private async Task UpdatePrefabsAsync(CancellationTokenSource cts)
		{
			await Task.Yield();
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0008365B File Offset: 0x0008185B
		private bool IsNeedUpdatePrefab(GameObject obj, out GameObject prefabAsset)
		{
			prefabAsset = null;
			return false;
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0002AE8F File Offset: 0x0002908F
		public string GetPrefabPath(GameObject prefab)
		{
			return null;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x00083664 File Offset: 0x00081864
		private void AddHashCodesRecursively(GameObject obj, List<int> hashCodes)
		{
			foreach (object obj2 in obj.transform)
			{
				Transform transform = (Transform)obj2;
				hashCodes.Add(transform.GetHashCode());
				this.AddHashCodesRecursively(transform.gameObject, hashCodes);
			}
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x000836D0 File Offset: 0x000818D0
		private bool CompareLists(List<int> a, List<int> b)
		{
			if (a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
