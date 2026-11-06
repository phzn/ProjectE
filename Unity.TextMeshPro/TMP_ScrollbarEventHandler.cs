using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro
{
	// Token: 0x0200004E RID: 78
	public class TMP_ScrollbarEventHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler
	{
		// Token: 0x0600035F RID: 863 RVA: 0x00024B16 File Offset: 0x00022D16
		public void OnPointerClick(PointerEventData eventData)
		{
			Debug.Log("Scrollbar click...");
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00024B22 File Offset: 0x00022D22
		public void OnSelect(BaseEventData eventData)
		{
			Debug.Log("Scrollbar selected");
			this.isSelected = true;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00024B35 File Offset: 0x00022D35
		public void OnDeselect(BaseEventData eventData)
		{
			Debug.Log("Scrollbar De-Selected");
			this.isSelected = false;
		}

		// Token: 0x0400032E RID: 814
		public bool isSelected;
	}
}
