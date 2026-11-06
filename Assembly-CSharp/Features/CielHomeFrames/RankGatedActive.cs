using System;
using Features.Rank;
using UnityEngine;

namespace Features.CielHomeFrames
{
	// Token: 0x020014B5 RID: 5301
	[DisallowMultipleComponent]
	public class RankGatedActive : MonoBehaviour
	{
		// Token: 0x0600A31B RID: 41755 RVA: 0x0043BF0C File Offset: 0x0043A10C
		private void OnEnable()
		{
			this.Apply();
			RankProgressionService.Changed += this.Apply;
		}

		// Token: 0x0600A31C RID: 41756 RVA: 0x0043BF25 File Offset: 0x0043A125
		private void OnDisable()
		{
			RankProgressionService.Changed -= this.Apply;
		}

		// Token: 0x0600A31D RID: 41757 RVA: 0x0043BF38 File Offset: 0x0043A138
		private void Apply()
		{
			bool flag = this.IsUnlocked();
			if (this.targets == null)
			{
				return;
			}
			foreach (GameObject gameObject in this.targets)
			{
				if (gameObject != null && gameObject.activeSelf != flag)
				{
					gameObject.SetActive(flag);
				}
			}
		}

		// Token: 0x0600A31E RID: 41758 RVA: 0x0043BF88 File Offset: 0x0043A188
		private bool IsUnlocked()
		{
			if (RankProgressionService.IsAdminRankActive)
			{
				return true;
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			if (config == null || config.ranksInOrder == null)
			{
				return true;
			}
			int num = -1;
			for (int i = 0; i < config.ranksInOrder.Count; i++)
			{
				if (config.ranksInOrder[i] != null && config.ranksInOrder[i].rankId == this.minRankId)
				{
					num = i;
					break;
				}
			}
			return num < 0 || RankProgressionService.RankIndex >= num;
		}

		// Token: 0x0400A6D9 RID: 42713
		[Tooltip("rankId (RankDefinitionSO) minimo perche' i target siano visibili, es. 'hornyslut_declared'.")]
		[SerializeField]
		private string minRankId = "hornyslut_declared";

		// Token: 0x0400A6DA RID: 42714
		[Tooltip("Oggetti mostrati dal rank minimo in poi, nascosti prima.")]
		[SerializeField]
		private GameObject[] targets;
	}
}
