using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000138 RID: 312
public class vScoreDataDisplay : MonoBehaviour
{
	// Token: 0x06000A37 RID: 2615 RVA: 0x00046678 File Offset: 0x00044878
	public void Show(int index, float? score, List<float> hits)
	{
		if (this.index)
		{
			this.index.text = index.ToString("00");
		}
		if (this.score)
		{
			this.score.text = ((score != null) ? score.Value.ToString("00") : "--");
		}
		if (hits != null)
		{
			for (int i = 0; i < this.hits.Length; i++)
			{
				Text text = this.hits[i];
				if (i >= hits.Count)
				{
					return;
				}
				text.text = hits[i].ToString("00");
			}
			return;
		}
		for (int j = 0; j < this.hits.Length; j++)
		{
			this.hits[j].text = "--";
		}
	}

	// Token: 0x0400096B RID: 2411
	public Text index;

	// Token: 0x0400096C RID: 2412
	public Text score;

	// Token: 0x0400096D RID: 2413
	public Text[] hits;
}
