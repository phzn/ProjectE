using System;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x020014A7 RID: 5287
	public class DialogueHoverTip : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x0600A2CA RID: 41674 RVA: 0x0043960C File Offset: 0x0043780C
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.tip == null)
			{
				return;
			}
			this.tip.SetActive(true);
			Canvas canvas = this.tip.GetComponent<Canvas>();
			if (canvas == null)
			{
				canvas = this.tip.AddComponent<Canvas>();
			}
			canvas.overrideSorting = true;
			canvas.sortingOrder = 500;
			GameplayHudVisibility.RegisterCanvas(canvas);
		}

		// Token: 0x0600A2CB RID: 41675 RVA: 0x0043966D File Offset: 0x0043786D
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.tip != null)
			{
				this.tip.SetActive(false);
			}
		}

		// Token: 0x0600A2CC RID: 41676 RVA: 0x0043966D File Offset: 0x0043786D
		private void OnDisable()
		{
			if (this.tip != null)
			{
				this.tip.SetActive(false);
			}
		}

		// Token: 0x0400A686 RID: 42630
		[Tooltip("The tip panel to show on hover (child of this element, ignoreLayout).")]
		[SerializeField]
		private GameObject tip;
	}
}
