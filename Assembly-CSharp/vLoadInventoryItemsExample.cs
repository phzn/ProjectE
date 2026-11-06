using System;
using System.Collections;
using Invector;
using Invector.vItemManager;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class vLoadInventoryItemsExample : MonoBehaviour
{
	// Token: 0x06000A00 RID: 2560 RVA: 0x00045B2B File Offset: 0x00043D2B
	private void Start()
	{
		this.gm = base.GetComponent<vGameController>();
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x00045B39 File Offset: 0x00043D39
	public void LoadItemsToInventory()
	{
		if (!this.gm)
		{
			return;
		}
		base.StartCoroutine(this.LoadItems());
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x00045B56 File Offset: 0x00043D56
	private IEnumerator LoadItems()
	{
		yield return new WaitForSeconds(0.1f);
		this.gm.currentPlayer.GetComponent<vItemManager>().LoadItemsExample();
		yield break;
	}

	// Token: 0x04000941 RID: 2369
	private vGameController gm;
}
