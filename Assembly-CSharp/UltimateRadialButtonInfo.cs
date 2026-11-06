using System;
using UnityEngine;

// Token: 0x02000152 RID: 338
[Serializable]
public class UltimateRadialButtonInfo
{
	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06000A95 RID: 2709 RVA: 0x000480BD File Offset: 0x000462BD
	// (set) Token: 0x06000A96 RID: 2710 RVA: 0x000480C5 File Offset: 0x000462C5
	public bool Selected
	{
		get
		{
			return this.selected;
		}
		set
		{
			this.selected = value;
			if (this.RadialButtonError_NoWarning)
			{
				return;
			}
			if (this.selected)
			{
				this.radialButton.OnSelect();
				return;
			}
			this.radialButton.OnDeselect();
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06000A97 RID: 2711 RVA: 0x000480F6 File Offset: 0x000462F6
	public bool Registered
	{
		get
		{
			return this.radialButton != null && this.radialButton.radialMenu != null;
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06000A98 RID: 2712 RVA: 0x00048116 File Offset: 0x00046316
	public int GetButtonIndex
	{
		get
		{
			if (this.RadialButtonError)
			{
				return 0;
			}
			return this.radialButton.buttonIndex;
		}
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0004812D File Offset: 0x0004632D
	public void UpdateText(string newText)
	{
		if (this.RadialButtonError)
		{
			return;
		}
		this.radialButton.UpdateText(newText);
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x00048144 File Offset: 0x00046344
	public void UpdateIcon(Sprite newIcon)
	{
		this.icon = newIcon;
		if (this.RadialButtonError_NoWarning)
		{
			return;
		}
		this.radialButton.UpdateIcon(newIcon);
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x00048162 File Offset: 0x00046362
	public void UpdateName(string newName)
	{
		this.name = newName;
		if (this.RadialButtonError_NoWarning)
		{
			return;
		}
		this.radialButton.UpdateName(newName);
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00048180 File Offset: 0x00046380
	public void UpdateDescription(string newDescription)
	{
		this.description = newDescription;
		if (this.RadialButtonError_NoWarning)
		{
			return;
		}
		this.radialButton.UpdateDescription(newDescription);
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x000481A0 File Offset: 0x000463A0
	public void SelectButton(bool exclusive = false)
	{
		if (this.RadialButtonError)
		{
			return;
		}
		this.Selected = true;
		if (exclusive)
		{
			for (int i = 0; i < this.radialButton.radialMenu.UltimateRadialButtonList.Count; i++)
			{
				if (i != this.radialButton.buttonIndex && this.radialButton.radialMenu.UltimateRadialButtonList[i].Selected)
				{
					this.radialButton.radialMenu.UltimateRadialButtonList[i].OnDeselect();
				}
			}
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00048226 File Offset: 0x00046426
	public void DeselectButton()
	{
		if (this.RadialButtonError)
		{
			return;
		}
		this.Selected = false;
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x00048238 File Offset: 0x00046438
	public void ToggleSelect()
	{
		if (this.RadialButtonError)
		{
			return;
		}
		if (this.radialButton.radialMenu.selectButtonOnInteract || (this.radialButton.radialMenu.selectButtonOnInteract && this.radialButton.radialMenu.toggleSelection))
		{
			Debug.LogWarning(this.FormatDebug("You are trying to toggle selection of the radial buttons while the Ultimate Radial Menu has some Automatic Selection settings. This will counteract each other so that nothing will happen", "Please use only one method of selecting buttons", "Unknown (user script)"));
			return;
		}
		this.Selected = !this.Selected;
		if (!this.Selected && this.radialButton.radialMenu.IsEnabled && this.radialButton.IsInAngle(this.radialButton.radialMenu.GetCurrentInputAngle))
		{
			this.radialButton.OnEnter();
		}
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x000482F0 File Offset: 0x000464F0
	public void EnableButton()
	{
		if (this.RadialButtonError)
		{
			return;
		}
		this.radialButton.OnEnable();
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00048306 File Offset: 0x00046506
	public void DisableButton()
	{
		if (this.RadialButtonError)
		{
			return;
		}
		this.radialButton.OnDisable();
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0004831C File Offset: 0x0004651C
	public void RemoveFromMenu()
	{
		if (this.RadialButtonError)
		{
			return;
		}
		this.radialButton.radialMenu.RemoveButton(this.radialButton.buttonIndex);
		this.radialButton = null;
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00048349 File Offset: 0x00046549
	public void RemoveInfoFromButton()
	{
		if (this.RadialButtonError)
		{
			return;
		}
		this.radialButton.ClearButtonInformation();
		this.radialButton = null;
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00048366 File Offset: 0x00046566
	public void OnClearButtonInformation()
	{
		this.radialButton = null;
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x0004836F File Offset: 0x0004656F
	public void OnSelectedStateChanged(bool selected)
	{
		this.selected = selected;
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x00048378 File Offset: 0x00046578
	private bool RadialButtonError_NoWarning
	{
		get
		{
			return this.radialButton == null || this.radialButton.radialMenu == null;
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x00048398 File Offset: 0x00046598
	private bool RadialButtonError
	{
		get
		{
			if (this.radialButton == null || this.radialButton.radialMenu == null)
			{
				Debug.LogWarning(this.FormatDebug("No UltimateRadialButton has been assigned to this button info", "Please register this button info to the radial menu using the RegisterButton function", "Unknown (user script)"));
				return true;
			}
			return false;
		}
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x000483D2 File Offset: 0x000465D2
	private string FormatDebug(string error, string solution, string objectName)
	{
		return string.Concat(new string[] { "<b>Ultimate Radial Button Info</b>\n<color=red><b>×</b></color> <i><b>Error:</b></i> ", error, ".\n<color=green><b>√</b></color> <i><b>Solution:</b></i> ", solution, ".\n<color=cyan><b>∙</b></color> <i><b>Object:</b></i> ", objectName, "\n" });
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x0004840B File Offset: 0x0004660B
	[Obsolete("This bool is obsolete. Please use the Selected property instead.")]
	public bool IsSelected
	{
		get
		{
			return !this.RadialButtonError && this.radialButton.Selected;
		}
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x0004831C File Offset: 0x0004651C
	[Obsolete("Please use the RemoveFromMenu function.")]
	public void RemoveRadialButton()
	{
		if (this.RadialButtonError)
		{
			return;
		}
		this.radialButton.radialMenu.RemoveButton(this.radialButton.buttonIndex);
		this.radialButton = null;
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x00048349 File Offset: 0x00046549
	[Obsolete("Please use the RemoveFromMenu function.")]
	public void RemoveInfoFromRadialButton()
	{
		if (this.RadialButtonError)
		{
			return;
		}
		this.radialButton.ClearButtonInformation();
		this.radialButton = null;
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x000480F6 File Offset: 0x000462F6
	[Obsolete("Please use the Registered property instead.")]
	public bool ExistsOnRadialMenu()
	{
		return this.radialButton != null && this.radialButton.radialMenu != null;
	}

	// Token: 0x040009DD RID: 2525
	public UltimateRadialMenu.UltimateRadialButton radialButton;

	// Token: 0x040009DE RID: 2526
	private bool selected;

	// Token: 0x040009DF RID: 2527
	public string key;

	// Token: 0x040009E0 RID: 2528
	public int id;

	// Token: 0x040009E1 RID: 2529
	public string name;

	// Token: 0x040009E2 RID: 2530
	public string description;

	// Token: 0x040009E3 RID: 2531
	public Sprite icon;
}
