using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200016C RID: 364
public class RadialMenuPrefabDisplay : MonoBehaviour
{
	// Token: 0x06000BAB RID: 2987 RVA: 0x00050C18 File Offset: 0x0004EE18
	private void Start()
	{
		if (this.allPrefabs.Count == 0)
		{
			Debug.LogError("No radial menu prefabs are registered in the All Prefabs list.");
			return;
		}
		for (int i = 0; i < this.allPrefabs.Count; i++)
		{
			this.allRadialMenus.Add(Object.Instantiate<GameObject>(this.allPrefabs[i], base.transform).GetComponent<UltimateRadialMenu>());
			if (i > 0)
			{
				this.allRadialMenus[this.allRadialMenus.Count - 1].Disable();
			}
		}
		this.RegisterDummyRadialMenuButtons(this.allRadialMenus[0]);
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x00050CB0 File Offset: 0x0004EEB0
	public void NextRadialMenu()
	{
		this.allRadialMenus[this.currentRadialMenuIndex].Disable();
		int num = this.currentRadialMenuIndex + 1;
		if (num >= this.allRadialMenus.Count)
		{
			num = 0;
		}
		this.RegisterDummyRadialMenuButtons(this.allRadialMenus[num]);
		this.currentRadialMenuIndex = num;
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00050D08 File Offset: 0x0004EF08
	public void PreviousRadialMenu()
	{
		this.allRadialMenus[this.currentRadialMenuIndex].Disable();
		int num = this.currentRadialMenuIndex - 1;
		if (num < 0)
		{
			num = this.allRadialMenus.Count - 1;
		}
		this.RegisterDummyRadialMenuButtons(this.allRadialMenus[num]);
		this.currentRadialMenuIndex = num;
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x00004F31 File Offset: 0x00003131
	public void SelectPrefabInProject()
	{
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x00050D5F File Offset: 0x0004EF5F
	public void IncreaseRadialButtonCount()
	{
		this.radialMenuButtons++;
		this.RegisterDummyRadialMenuButtons(this.allRadialMenus[this.currentRadialMenuIndex]);
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x00050D86 File Offset: 0x0004EF86
	public void DecreaseRadialButtonCount()
	{
		if (this.radialMenuButtons <= 2)
		{
			return;
		}
		this.radialMenuButtons--;
		this.RegisterDummyRadialMenuButtons(this.allRadialMenus[this.currentRadialMenuIndex]);
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x00050DB8 File Offset: 0x0004EFB8
	private void RegisterDummyRadialMenuButtons(UltimateRadialMenu radialMenu)
	{
		radialMenu.ClearMenu(0);
		for (int i = 0; i < this.radialMenuButtons; i++)
		{
			radialMenu.RegisterButton(new Action(this.DummyVoid), new UltimateRadialButtonInfo
			{
				icon = this.radialMenuButtonInfo.icon
			}, -1);
		}
		radialMenu.Enable();
		this.prefabNameText.text = "Prefab name: " + this.allRadialMenus[this.currentRadialMenuIndex].name.Split('(', StringSplitOptions.None)[0];
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x00004F31 File Offset: 0x00003131
	private void DummyVoid()
	{
	}

	// Token: 0x04000B76 RID: 2934
	[Header("Common Settings")]
	public Text prefabNameText;

	// Token: 0x04000B77 RID: 2935
	public int radialMenuButtons = 4;

	// Token: 0x04000B78 RID: 2936
	public UltimateRadialButtonInfo radialMenuButtonInfo;

	// Token: 0x04000B79 RID: 2937
	[Header("Prefabs")]
	public List<GameObject> allPrefabs = new List<GameObject>();

	// Token: 0x04000B7A RID: 2938
	private List<UltimateRadialMenu> allRadialMenus = new List<UltimateRadialMenu>();

	// Token: 0x04000B7B RID: 2939
	private int currentRadialMenuIndex;
}
