using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x020001E2 RID: 482
	public class ContextMenuSubMenu : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x06000DE6 RID: 3558 RVA: 0x0005FC17 File Offset: 0x0005DE17
		private void OnEnable()
		{
			if (this.itemParent == null)
			{
				Debug.Log("<b>[Context Menu]</b> Item Parent is missing.", this);
				return;
			}
			this.listParent = this.itemParent.parent.gameObject.GetComponent<RectTransform>();
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0005FC50 File Offset: 0x0005DE50
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.cmManager.subMenuBehaviour == ContextMenuManager.SubMenuBehaviour.Click)
			{
				if (this.subMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName("Menu In"))
				{
					this.subMenuAnimator.Play("Menu Out");
					if (this.trigger != null)
					{
						this.trigger.SetActive(false);
						return;
					}
				}
				else
				{
					this.subMenuAnimator.Play("Menu In");
					if (this.trigger != null)
					{
						this.trigger.SetActive(true);
					}
				}
			}
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0005FCDC File Offset: 0x0005DEDC
		public void OnPointerEnter(PointerEventData eventData)
		{
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			for (int i = 0; i < this.cmContent.contexItems[this.subMenuIndex].subMenuItems.Count; i++)
			{
				bool flag = false;
				if (this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].contextItemType == ContextMenuContent.ContextItemType.Button && this.cmManager.contextButton != null)
				{
					this.selectedItem = this.cmManager.contextButton;
				}
				else if (this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].contextItemType == ContextMenuContent.ContextItemType.Separator && this.cmManager.contextSeparator != null)
				{
					this.selectedItem = this.cmManager.contextSeparator;
				}
				else
				{
					Debug.LogError("<b>[Context Menu]</b> At least one of the item presets is missing. You can assign a new variable in Resources (Context Menu) tab. All default presets can be found in <b>Modern UI Pack > Prefabs > Context Menu</b> folder.", this);
					flag = true;
				}
				if (!flag)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.selectedItem, new Vector3(0f, 0f, 0f), Quaternion.identity);
					gameObject.transform.SetParent(this.itemParent, false);
					if (this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].contextItemType == ContextMenuContent.ContextItemType.Button)
					{
						this.setItemText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
						this.textHelper = this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].itemText;
						this.setItemText.text = this.textHelper;
						Transform transform = gameObject.gameObject.transform.Find("Icon");
						this.setItemImage = transform.GetComponent<Image>();
						this.imageHelper = this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].itemIcon;
						this.setItemImage.sprite = this.imageHelper;
						if (this.imageHelper == null)
						{
							this.setItemImage.color = new Color(0f, 0f, 0f, 0f);
						}
						Button component = gameObject.GetComponent<Button>();
						component.onClick.AddListener(new UnityAction(this.cmContent.contexItems[this.subMenuIndex].subMenuItems[i].onClick.Invoke));
						component.onClick.AddListener(new UnityAction(this.CloseOnClick));
						base.StartCoroutine(this.ExecuteAfterTime(0.01f));
					}
				}
			}
			if (this.cmManager.autoSubMenuPosition)
			{
				if (this.cmManager.horizontalBound == ContextMenuManager.CursorBoundHorizontal.Left)
				{
					this.listParent.pivot = new Vector2(0f, this.listParent.pivot.y);
				}
				else if (this.cmManager.horizontalBound == ContextMenuManager.CursorBoundHorizontal.Right)
				{
					this.listParent.pivot = new Vector2(1f, this.listParent.pivot.y);
				}
				if (this.cmManager.verticalBound == ContextMenuManager.CursorBoundVertical.Top)
				{
					this.listParent.pivot = new Vector2(this.listParent.pivot.x, 0f);
				}
				else if (this.cmManager.verticalBound == ContextMenuManager.CursorBoundVertical.Bottom)
				{
					this.listParent.pivot = new Vector2(this.listParent.pivot.x, 1f);
				}
			}
			if (this.cmManager.subMenuBehaviour == ContextMenuManager.SubMenuBehaviour.Hover)
			{
				this.subMenuAnimator.Play("Menu In");
			}
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00004F31 File Offset: 0x00003131
		public void OnPointerExit(PointerEventData eventData)
		{
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x000600C4 File Offset: 0x0005E2C4
		private IEnumerator ExecuteAfterTime(float time)
		{
			yield return new WaitForSecondsRealtime(time);
			this.itemParent.gameObject.SetActive(false);
			this.itemParent.gameObject.SetActive(true);
			base.StopCoroutine(this.ExecuteAfterTime(0.01f));
			base.StopCoroutine("ExecuteAfterTime");
			yield break;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x000600DA File Offset: 0x0005E2DA
		public void CloseOnClick()
		{
			this.cmManager.contextAnimator.Play("Menu Out");
			this.cmManager.isOn = false;
			this.trigger.SetActive(false);
		}

		// Token: 0x04000E7C RID: 3708
		public ContextMenuManager cmManager;

		// Token: 0x04000E7D RID: 3709
		public ContextMenuContent cmContent;

		// Token: 0x04000E7E RID: 3710
		public Animator subMenuAnimator;

		// Token: 0x04000E7F RID: 3711
		public Transform itemParent;

		// Token: 0x04000E80 RID: 3712
		public GameObject trigger;

		// Token: 0x04000E81 RID: 3713
		[HideInInspector]
		public int subMenuIndex;

		// Token: 0x04000E82 RID: 3714
		private GameObject selectedItem;

		// Token: 0x04000E83 RID: 3715
		private Image setItemImage;

		// Token: 0x04000E84 RID: 3716
		private TextMeshProUGUI setItemText;

		// Token: 0x04000E85 RID: 3717
		private Sprite imageHelper;

		// Token: 0x04000E86 RID: 3718
		private string textHelper;

		// Token: 0x04000E87 RID: 3719
		private RectTransform listParent;

		// Token: 0x04000E88 RID: 3720
		[HideInInspector]
		public bool enableFadeOut = true;
	}
}
