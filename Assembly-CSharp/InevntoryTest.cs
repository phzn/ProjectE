using System;
using Features.Inventory.Placement;
using ProjectExhibitionist.Core;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class InevntoryTest : MonoBehaviour
{
	// Token: 0x060000F9 RID: 249 RVA: 0x00004F31 File Offset: 0x00003131
	private void Start()
	{
	}

	// Token: 0x060000FA RID: 250 RVA: 0x0000870C File Offset: 0x0000690C
	private void Update()
	{
		if (GameplayInput.GetKeyDown(KeyCode.P))
		{
			Debug.Log("[InventoryTest] P premuto");
			base.GetComponent<DildoPlacementSystem>().EnterPlacementMode();
		}
	}
}
