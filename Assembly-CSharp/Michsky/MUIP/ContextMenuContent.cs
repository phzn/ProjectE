using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x020001D4 RID: 468
	[AddComponentMenu("Modern UI Pack/Context Menu/Context Menu Content")]
	public class ContextMenuContent : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06000DBC RID: 3516 RVA: 0x0005EA8C File Offset: 0x0005CC8C
		private void Awake()
		{
			if (this.contextManager == null)
			{
				try
				{
					this.contextManager = (ContextMenuManager)Object.FindObjectsOfType(typeof(ContextMenuManager))[0];
					this.itemParent = this.contextManager.transform.Find("Content/Item List").transform;
				}
				catch
				{
					Debug.LogError("<b>[Context Menu]</b> Context Manager is missing.", this);
					return;
				}
			}
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0005EB50 File Offset: 0x0005CD50
		public void ProcessContent()
		{
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			for (int i = 0; i < this.contexItems.Count; i++)
			{
				bool flag = false;
				if (this.contexItems[i].contextItemType == ContextMenuContent.ContextItemType.Button && this.contextManager.contextButton != null)
				{
					this.selectedItem = this.contextManager.contextButton;
				}
				else if (this.contexItems[i].contextItemType == ContextMenuContent.ContextItemType.Separator && this.contextManager.contextSeparator != null)
				{
					this.selectedItem = this.contextManager.contextSeparator;
				}
				else
				{
					Debug.LogError("<b>[Context Menu]</b> At least one of the item presets is missing. You can assign a new variable in Resources (Context Menu) tab. All default presets can be found in <b>Modern UI Pack > Prefabs > Context Menu</b> folder.", this);
					flag = true;
				}
				if (!flag)
				{
					if (this.contexItems[i].subMenuItems.Count == 0)
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this.selectedItem, new Vector3(0f, 0f, 0f), Quaternion.identity);
						gameObject.transform.SetParent(this.itemParent, false);
						if (this.contexItems[i].contextItemType == ContextMenuContent.ContextItemType.Button)
						{
							this.setItemText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
							this.textHelper = this.contexItems[i].itemText;
							this.setItemText.text = this.textHelper;
							Transform transform = gameObject.gameObject.transform.Find("Icon");
							this.setItemImage = transform.GetComponent<Image>();
							this.imageHelper = this.contexItems[i].itemIcon;
							this.setItemImage.sprite = this.imageHelper;
							if (this.imageHelper == null)
							{
								this.setItemImage.color = new Color(0f, 0f, 0f, 0f);
							}
							Button component = gameObject.GetComponent<Button>();
							component.onClick.AddListener(new UnityAction(this.contexItems[i].onClick.Invoke));
							component.onClick.AddListener(new UnityAction(this.contextManager.Close));
						}
					}
					else if (this.contextManager.contextSubMenu != null && this.contexItems[i].subMenuItems.Count != 0)
					{
						GameObject gameObject2 = Object.Instantiate<GameObject>(this.contextManager.contextSubMenu, new Vector3(0f, 0f, 0f), Quaternion.identity);
						gameObject2.transform.SetParent(this.itemParent, false);
						ContextMenuSubMenu component2 = gameObject2.GetComponent<ContextMenuSubMenu>();
						component2.cmManager = this.contextManager;
						component2.cmContent = this;
						component2.subMenuIndex = i;
						this.setItemText = gameObject2.GetComponentInChildren<TextMeshProUGUI>();
						this.textHelper = this.contexItems[i].itemText;
						this.setItemText.text = this.textHelper;
						Transform transform2 = gameObject2.gameObject.transform.Find("Icon");
						this.setItemImage = transform2.GetComponent<Image>();
						this.imageHelper = this.contexItems[i].itemIcon;
						this.setItemImage.sprite = this.imageHelper;
					}
					base.StopCoroutine("ExecuteAfterTime");
					base.StartCoroutine("ExecuteAfterTime", 0.01f);
				}
			}
			this.contextManager.SetContextMenuPosition();
			this.contextManager.Open();
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0005EF04 File Offset: 0x0005D104
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.contextManager.isOn)
			{
				this.contextManager.Close();
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Right && !this.contextManager.isOn)
			{
				this.ProcessContent();
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0005EF3B File Offset: 0x0005D13B
		private IEnumerator ExecuteAfterTime(float time)
		{
			yield return new WaitForSecondsRealtime(time);
			this.itemParent.gameObject.SetActive(false);
			this.itemParent.gameObject.SetActive(true);
			yield break;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0005EF51 File Offset: 0x0005D151
		public void OnMouseOver()
		{
			if (this.useIn3D && Input.GetMouseButtonDown(1))
			{
				this.ProcessContent();
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0005EF6C File Offset: 0x0005D16C
		public void AddNewItem()
		{
			ContextMenuContent.ContextItem contextItem = new ContextMenuContent.ContextItem();
			this.contexItems.Add(contextItem);
		}

		// Token: 0x04000E29 RID: 3625
		public ContextMenuManager contextManager;

		// Token: 0x04000E2A RID: 3626
		public Transform itemParent;

		// Token: 0x04000E2B RID: 3627
		public bool useIn3D;

		// Token: 0x04000E2C RID: 3628
		public List<ContextMenuContent.ContextItem> contexItems = new List<ContextMenuContent.ContextItem>();

		// Token: 0x04000E2D RID: 3629
		private GameObject selectedItem;

		// Token: 0x04000E2E RID: 3630
		private Image setItemImage;

		// Token: 0x04000E2F RID: 3631
		private TextMeshProUGUI setItemText;

		// Token: 0x04000E30 RID: 3632
		private Sprite imageHelper;

		// Token: 0x04000E31 RID: 3633
		private string textHelper;

		// Token: 0x020001D5 RID: 469
		[Serializable]
		public class ContextItem
		{
			// Token: 0x04000E32 RID: 3634
			[Header("Information")]
			[Space(-5f)]
			public string itemText = "Item Text";

			// Token: 0x04000E33 RID: 3635
			public Sprite itemIcon;

			// Token: 0x04000E34 RID: 3636
			public ContextMenuContent.ContextItemType contextItemType;

			// Token: 0x04000E35 RID: 3637
			[Header("Sub Menu")]
			public List<ContextMenuContent.SubMenuItem> subMenuItems = new List<ContextMenuContent.SubMenuItem>();

			// Token: 0x04000E36 RID: 3638
			[Header("Events")]
			public UnityEvent onClick;
		}

		// Token: 0x020001D6 RID: 470
		[Serializable]
		public class SubMenuItem
		{
			// Token: 0x04000E37 RID: 3639
			public string itemText = "Item Text";

			// Token: 0x04000E38 RID: 3640
			public Sprite itemIcon;

			// Token: 0x04000E39 RID: 3641
			public ContextMenuContent.ContextItemType contextItemType;

			// Token: 0x04000E3A RID: 3642
			public UnityEvent onClick;
		}

		// Token: 0x020001D7 RID: 471
		public enum ContextItemType
		{
			// Token: 0x04000E3C RID: 3644
			Button,
			// Token: 0x04000E3D RID: 3645
			Separator
		}
	}
}
