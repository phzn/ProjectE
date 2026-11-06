using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NPC.StateMachine
{
	// Token: 0x02000F08 RID: 3848
	[CreateAssetMenu(fileName = "New Alert Animation Profile", menuName = "NPC/Alert Animation Profile")]
	public class AlertAnimationProfileSO : ScriptableObject
	{
		// Token: 0x0600737D RID: 29565 RVA: 0x002F19FA File Offset: 0x002EFBFA
		public AlertAnimationEntry GetRandomStopAndStareAnimation()
		{
			return this.GetRandomEntry(this.stopAndStareAnimations);
		}

		// Token: 0x0600737E RID: 29566 RVA: 0x002F1A08 File Offset: 0x002EFC08
		public AlertAnimationEntry GetRandomSearchingAnimation()
		{
			return this.GetRandomEntry(this.searchingAnimations);
		}

		// Token: 0x0600737F RID: 29567 RVA: 0x002F1A18 File Offset: 0x002EFC18
		private AlertAnimationEntry GetRandomEntry(List<AlertAnimationEntry> entries)
		{
			if (entries == null || entries.Count == 0)
			{
				return null;
			}
			int num = 0;
			foreach (AlertAnimationEntry alertAnimationEntry in entries)
			{
				num += alertAnimationEntry.weight;
			}
			if (num <= 0)
			{
				return entries[0];
			}
			int num2 = Random.Range(0, num);
			int num3 = 0;
			foreach (AlertAnimationEntry alertAnimationEntry2 in entries)
			{
				num3 += alertAnimationEntry2.weight;
				if (num2 < num3)
				{
					return alertAnimationEntry2;
				}
			}
			return entries[0];
		}

		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06007380 RID: 29568 RVA: 0x002F1AE4 File Offset: 0x002EFCE4
		public int StopAndStareCount
		{
			get
			{
				List<AlertAnimationEntry> list = this.stopAndStareAnimations;
				if (list == null)
				{
					return 0;
				}
				return list.Count;
			}
		}

		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x06007381 RID: 29569 RVA: 0x002F1AF7 File Offset: 0x002EFCF7
		public int SearchingCount
		{
			get
			{
				List<AlertAnimationEntry> list = this.searchingAnimations;
				if (list == null)
				{
					return 0;
				}
				return list.Count;
			}
		}

		// Token: 0x040076C1 RID: 30401
		[Header("StopAndStare Animations")]
		[Tooltip("Available animations for StopAndStare state (surprised reactions)")]
		public List<AlertAnimationEntry> stopAndStareAnimations = new List<AlertAnimationEntry>();

		// Token: 0x040076C2 RID: 30402
		[Header("Searching Animations")]
		[Tooltip("Available animations for Searching state (chase/investigate)")]
		public List<AlertAnimationEntry> searchingAnimations = new List<AlertAnimationEntry>();
	}
}
