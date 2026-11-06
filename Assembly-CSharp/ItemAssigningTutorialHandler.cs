using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000169 RID: 361
public class ItemAssigningTutorialHandler : MonoBehaviour
{
	// Token: 0x06000B9F RID: 2975 RVA: 0x00050891 File Offset: 0x0004EA91
	private void Start()
	{
		UltimateRadialMenu.ReturnComponent("ItemWheelExample").OnMenuEnabled += this.OnRadialMenuEnabled;
		UltimateRadialMenu.ReturnComponent("ItemWheelExample").OnMenuDisabled += this.OnRadialMenuDisabled;
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x000508C9 File Offset: 0x0004EAC9
	private void OnRadialMenuEnabled()
	{
		this.tutorialText.text = "Click and drag the items on the right to the radial menu to assign them to a button.\n\nClicking on the item will increase the count of that item.";
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x000508DB File Offset: 0x0004EADB
	private void OnRadialMenuDisabled()
	{
		this.tutorialText.text = "Press SPACE BAR to open your radial menu.";
	}

	// Token: 0x04000B6C RID: 2924
	public Text tutorialText;
}
