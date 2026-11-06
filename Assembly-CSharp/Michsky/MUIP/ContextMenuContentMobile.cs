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
	// Token: 0x020001D9 RID: 473
	[AddComponentMenu("Modern UI Pack/Context Menu/Context Menu Content (Mobile)")]
	public class ContextMenuContentMobile : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x06000DCB RID: 3531 RVA: 0x0005F058 File Offset: 0x0005D258
		private void Start()
		{
			if (this.contextManager == null)
			{
				try
				{
					this.contextManager = GameObject.Find("Context Menu").GetComponent<ContextMenuManager>();
					this.itemParent = this.contextManager.transform.Find("Content/Item List").transform;
				}
				catch
				{
					Debug.Log("<b>[Context Menu]</b> Context Manager is missing.", this);
					return;
				}
			}
			this.contextAnimator = this.contextManager.contextAnimator;
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0005F124 File Offset: 0x0005D324
		private void Update()
		{
			if (this.timerEnabled)
			{
				this.timer += Time.deltaTime;
				if (this.timer >= this.holdToOpen)
				{
					this.CheckForTimer();
					this.timerEnabled = false;
					this.timer = 0f;
				}
			}
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0005F171 File Offset: 0x0005D371
		public void OnPointerDown(PointerEventData eventData)
		{
			this.timerEnabled = true;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0005F17A File Offset: 0x0005D37A
		public void OnPointerUp(PointerEventData eventData)
		{
			this.timerEnabled = false;
			this.timer = 0f;
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0005F190 File Offset: 0x0005D390
		public void CheckForTimer()
		{
			if (this.timer <= this.holdToOpen)
			{
				return;
			}
			if (this.contextManager.isOn)
			{
				this.contextAnimator.Play("Menu Out");
				this.contextManager.isOn = false;
				return;
			}
			if (!this.contextManager.isOn)
			{
				foreach (object obj in this.itemParent)
				{
					Object.Destroy(((Transform)obj).gameObject);
				}
				for (int i = 0; i < this.contexItems.Count; i++)
				{
					if (this.contexItems[i].contextItemType == ContextMenuContentMobile.ContextItemType.BUTTON)
					{
						this.selectedItem = this.contextManager.contextButton;
					}
					GameObject gameObject = Object.Instantiate<GameObject>(this.selectedItem, new Vector3(0f, 0f, 0f), Quaternion.identity);
					gameObject.transform.SetParent(this.itemParent, false);
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
					component.onClick.AddListener(new UnityAction(this.CloseOnClick));
					base.StartCoroutine(this.ExecuteAfterTime(0.01f));
				}
				this.contextManager.SetContextMenuPosition();
				this.contextAnimator.Play("Menu In");
				this.contextManager.isOn = true;
				this.contextManager.SetContextMenuPosition();
			}
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0005F3E4 File Offset: 0x0005D5E4
		private IEnumerator ExecuteAfterTime(float time)
		{
			yield return new WaitForSeconds(time);
			this.itemParent.gameObject.SetActive(false);
			this.itemParent.gameObject.SetActive(true);
			base.StopCoroutine(this.ExecuteAfterTime(0.01f));
			base.StopCoroutine("ExecuteAfterTime");
			yield break;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0005F3FA File Offset: 0x0005D5FA
		public void CloseOnClick()
		{
			this.contextAnimator.Play("Menu Out");
			this.contextManager.isOn = false;
		}

		// Token: 0x04000E42 RID: 3650
		[Header("Resources")]
		public ContextMenuManager contextManager;

		// Token: 0x04000E43 RID: 3651
		public Transform itemParent;

		// Token: 0x04000E44 RID: 3652
		[Header("Settings")]
		[Range(0.1f, 6f)]
		public float holdToOpen = 0.75f;

		// Token: 0x04000E45 RID: 3653
		[Header("Items")]
		public List<ContextMenuContentMobile.ContextItem> contexItems = new List<ContextMenuContentMobile.ContextItem>();

		// Token: 0x04000E46 RID: 3654
		private Animator contextAnimator;

		// Token: 0x04000E47 RID: 3655
		private GameObject selectedItem;

		// Token: 0x04000E48 RID: 3656
		private Image setItemImage;

		// Token: 0x04000E49 RID: 3657
		private TextMeshProUGUI setItemText;

		// Token: 0x04000E4A RID: 3658
		private Sprite imageHelper;

		// Token: 0x04000E4B RID: 3659
		private string textHelper;

		// Token: 0x04000E4C RID: 3660
		private float timer;

		// Token: 0x04000E4D RID: 3661
		private bool timerEnabled;

		// Token: 0x020001DA RID: 474
		[Serializable]
		public class ContextItem
		{
			// Token: 0x04000E4E RID: 3662
			public string itemText = "Item Text";

			// Token: 0x04000E4F RID: 3663
			public Sprite itemIcon;

			// Token: 0x04000E50 RID: 3664
			public ContextMenuContentMobile.ContextItemType contextItemType;

			// Token: 0x04000E51 RID: 3665
			public UnityEvent onClick;
		}

		// Token: 0x020001DB RID: 475
		public enum ContextItemType
		{
			// Token: 0x04000E53 RID: 3667
			BUTTON
		}
	}
}
