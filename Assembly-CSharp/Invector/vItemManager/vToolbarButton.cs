using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x020005D7 RID: 1495
	[RequireComponent(typeof(Image))]
	public class vToolbarButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060022CC RID: 8908 RVA: 0x000C7963 File Offset: 0x000C5B63
		public virtual void Reset()
		{
			this.image = base.GetComponent<Image>();
			if (!this.image)
			{
				this.image = base.gameObject.AddComponent<Image>();
			}
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000C798F File Offset: 0x000C5B8F
		protected virtual void OnDisable()
		{
			this.image.color = this.unSelectedColor;
			this.image.SetAllDirty();
			this.onDeselect.Invoke();
			this.isSelected = false;
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000C79C0 File Offset: 0x000C5BC0
		public virtual void OnSelectTool(vToolbarButton toolbarButton)
		{
			if (toolbarButton.Equals(this))
			{
				this.image.color = this.selectedColor;
				if (!this.isSelected)
				{
					this.isSelected = true;
					this.onSelect.Invoke();
				}
				this.image.SetAllDirty();
				return;
			}
			this.image.color = this.unSelectedColor;
			this.image.SetAllDirty();
			this.onDeselect.Invoke();
			this.isSelected = false;
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000C7A3B File Offset: 0x000C5C3B
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			this.onSelect.Invoke();
			this.isSelected = true;
		}

		// Token: 0x0400230E RID: 8974
		public GameObject targetWindow;

		// Token: 0x0400230F RID: 8975
		public Image image;

		// Token: 0x04002310 RID: 8976
		public Color selectedColor = Color.white;

		// Token: 0x04002311 RID: 8977
		public Color unSelectedColor = Color.grey;

		// Token: 0x04002312 RID: 8978
		public UnityEvent onSelect;

		// Token: 0x04002313 RID: 8979
		public UnityEvent onDeselect;

		// Token: 0x04002314 RID: 8980
		private bool isSelected;
	}
}
