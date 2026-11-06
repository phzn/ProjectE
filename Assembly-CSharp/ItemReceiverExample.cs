using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200016A RID: 362
public class ItemReceiverExample : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000BA3 RID: 2979 RVA: 0x000508ED File Offset: 0x0004EAED
	private void Start()
	{
		this.myTransform = base.GetComponent<RectTransform>();
		this.originalPosition = this.myTransform.localPosition;
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x0005090C File Offset: 0x0004EB0C
	public void OnDrag(PointerEventData eventData)
	{
		this.myTransform.position = eventData.position;
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x00050924 File Offset: 0x0004EB24
	public void OnPointerUp(PointerEventData eventData)
	{
		int currentButtonIndex = UltimateRadialMenu.ReturnComponent("ItemWheelExample").CurrentButtonIndex;
		if (currentButtonIndex >= 0 && !this.newRadialButtonInfo.Registered && !UltimateRadialMenu.ReturnComponent("ItemWheelExample").UltimateRadialButtonList[currentButtonIndex].Registered)
		{
			UltimateRadialMenu.RegisterButton("ItemWheelExample", new Action(this.UseItem), this.newRadialButtonInfo, currentButtonIndex);
		}
		this.itemCount++;
		if (this.newRadialButtonInfo.Registered)
		{
			this.newRadialButtonInfo.UpdateText(this.itemCount.ToString());
		}
		this.myTransform.localPosition = this.originalPosition;
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x000509D0 File Offset: 0x0004EBD0
	private void UseItem()
	{
		this.itemCount--;
		this.newRadialButtonInfo.UpdateText(this.itemCount.ToString());
		if (this.itemCount <= 0)
		{
			UltimateRadialMenu.UltimateRadialButton radialButton = this.newRadialButtonInfo.radialButton;
			this.newRadialButtonInfo.RemoveInfoFromButton();
			radialButton.UpdateText("Text");
			radialButton.UpdateIcon(this.placeholderIcon);
		}
	}

	// Token: 0x04000B6D RID: 2925
	private RectTransform myTransform;

	// Token: 0x04000B6E RID: 2926
	private Vector3 originalPosition;

	// Token: 0x04000B6F RID: 2927
	public UltimateRadialButtonInfo newRadialButtonInfo;

	// Token: 0x04000B70 RID: 2928
	private int itemCount;

	// Token: 0x04000B71 RID: 2929
	public Sprite placeholderIcon;
}
