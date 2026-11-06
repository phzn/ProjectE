using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000201 RID: 513
	public class DropdownMultiSelect : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerClickHandler
	{
		// Token: 0x06000E92 RID: 3730 RVA: 0x0006243C File Offset: 0x0006063C
		private void OnEnable()
		{
			if (!this.isInitialized)
			{
				this.Initialize();
			}
			this.listCG.alpha = 0f;
			this.listCG.interactable = false;
			this.listCG.blocksRaycasts = false;
			this.listRect.sizeDelta = new Vector2(this.listRect.sizeDelta.x, this.closeOn);
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x000624A8 File Offset: 0x000606A8
		private void Initialize()
		{
			if (this.listCG == null)
			{
				this.listCG = base.gameObject.GetComponentInChildren<CanvasGroup>();
			}
			if (this.listRect == null)
			{
				this.listRect = this.listCG.GetComponent<RectTransform>();
			}
			if (this.initAtStart)
			{
				this.SetupDropdown();
			}
			if (this.animationType == DropdownMultiSelect.AnimationType.Modular && this.dropdownAnimator != null)
			{
				Object.Destroy(this.dropdownAnimator);
			}
			if (this.enableTrigger && this.triggerObject != null)
			{
				this.triggerEvent = this.triggerObject.AddComponent<EventTrigger>();
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerClick;
				entry.callback.AddListener(delegate(BaseEventData eventData)
				{
					this.Animate();
				});
				this.triggerEvent.GetComponent<EventTrigger>().triggers.Add(entry);
			}
			if (this.setHighPriority)
			{
				if (this.contentCG == null)
				{
					this.contentCG = base.transform.Find("Content/Item List").GetComponent<CanvasGroup>();
				}
				this.contentCG.alpha = 1f;
				Canvas canvas = this.contentCG.gameObject.AddComponent<Canvas>();
				canvas.overrideSorting = true;
				canvas.sortingOrder = 30000;
				this.contentCG.gameObject.AddComponent<GraphicRaycaster>();
			}
			this.currentListParent = base.transform.parent;
			this.closeOn = base.gameObject.GetComponent<RectTransform>().sizeDelta.y;
			this.isInitialized = true;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00062629 File Offset: 0x00060829
		private void Update()
		{
			if (!this.isInTransition)
			{
				return;
			}
			this.ProcessModularAnimation();
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0006263C File Offset: 0x0006083C
		private void ProcessModularAnimation()
		{
			if (this.isOn)
			{
				this.listCG.alpha += Time.unscaledDeltaTime * this.transitionSmoothness;
				this.listRect.sizeDelta = Vector2.Lerp(this.listRect.sizeDelta, new Vector2(this.listRect.sizeDelta.x, this.panelSize), Time.unscaledDeltaTime * this.sizeSmoothness);
				if (this.listRect.sizeDelta.y >= this.panelSize - 0.1f && this.listCG.alpha >= 1f)
				{
					this.isInTransition = false;
					return;
				}
			}
			else
			{
				this.listCG.alpha -= Time.unscaledDeltaTime * this.transitionSmoothness;
				this.listRect.sizeDelta = Vector2.Lerp(this.listRect.sizeDelta, new Vector2(this.listRect.sizeDelta.x, this.closeOn), Time.unscaledDeltaTime * this.sizeSmoothness);
				if (this.listRect.sizeDelta.y <= this.closeOn + 0.1f && this.listCG.alpha <= 0f)
				{
					this.isInTransition = false;
				}
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0006278C File Offset: 0x0006098C
		public void SetupDropdown()
		{
			if (this.dropdownAnimator == null)
			{
				this.dropdownAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (!this.enableScrollbar && this.scrollbar != null)
			{
				Object.Destroy(this.scrollbar);
			}
			if (this.itemList == null)
			{
				this.itemList = this.itemParent.GetComponent<VerticalLayoutGroup>();
			}
			this.UpdateItemLayout();
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.itemParent, false);
				this.setItemText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
				this.textHelper = this.items[i].itemName;
				this.setItemText.text = this.textHelper;
				this.items[i].itemIndex = i;
				DropdownMultiSelect.Item mainItem = this.items[i];
				Toggle component = gameObject.GetComponent<Toggle>();
				component.onValueChanged.AddListener(delegate
				{
					this.UpdateToggleData(mainItem.itemIndex);
				});
				component.onValueChanged.AddListener(new UnityAction<bool>(this.UpdateToggle));
				component.onValueChanged.AddListener(new UnityAction<bool>(this.items[i].onValueChanged.Invoke));
				if (this.items[i].isOn)
				{
					component.isOn = true;
				}
				else
				{
					component.isOn = false;
				}
				if (this.invokeAtStart)
				{
					if (this.items[i].isOn)
					{
						this.items[i].onValueChanged.Invoke(true);
					}
					else
					{
						this.items[i].onValueChanged.Invoke(false);
					}
				}
			}
			this.currentListParent = base.transform.parent;
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x000629EC File Offset: 0x00060BEC
		private void UpdateToggle(bool value)
		{
			if (value)
			{
				this.currentToggle.isOn = true;
				this.items[this.currentIndex].isOn = true;
				return;
			}
			this.currentToggle.isOn = false;
			this.items[this.currentIndex].isOn = false;
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00062A43 File Offset: 0x00060C43
		private void UpdateToggleData(int itemIndex)
		{
			this.currentIndex = itemIndex;
			this.currentToggle = this.itemParent.GetChild(this.currentIndex).GetComponent<Toggle>();
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00062A68 File Offset: 0x00060C68
		public void Animate()
		{
			if (!this.isOn && this.animationType == DropdownMultiSelect.AnimationType.Modular)
			{
				this.isOn = true;
				this.isInTransition = true;
				base.enabled = true;
				this.listCG.blocksRaycasts = true;
				this.listCG.interactable = true;
				if (this.isListItem)
				{
					this.siblingIndex = base.transform.GetSiblingIndex();
					base.gameObject.transform.SetParent(this.listParent, true);
				}
			}
			else if (this.isOn && this.animationType == DropdownMultiSelect.AnimationType.Modular)
			{
				this.isOn = false;
				this.isInTransition = true;
				base.enabled = true;
				this.listCG.blocksRaycasts = false;
				this.listCG.interactable = false;
				if (this.isListItem)
				{
					base.gameObject.transform.SetParent(this.currentListParent, true);
					base.gameObject.transform.SetSiblingIndex(this.siblingIndex);
				}
			}
			else if (!this.isOn && this.animationType == DropdownMultiSelect.AnimationType.Stylish)
			{
				this.dropdownAnimator.Play("Stylish In");
				this.isOn = true;
				if (this.isListItem)
				{
					this.siblingIndex = base.transform.GetSiblingIndex();
					base.gameObject.transform.SetParent(this.listParent, true);
				}
			}
			else if (this.isOn && this.animationType == DropdownMultiSelect.AnimationType.Stylish)
			{
				this.dropdownAnimator.Play("Stylish Out");
				this.isOn = false;
				if (this.isListItem)
				{
					base.gameObject.transform.SetParent(this.currentListParent, true);
					base.gameObject.transform.SetSiblingIndex(this.siblingIndex);
				}
			}
			if (this.enableTrigger && !this.isOn)
			{
				this.triggerObject.SetActive(false);
			}
			else if (this.enableTrigger && this.isOn)
			{
				this.triggerObject.SetActive(true);
			}
			if (this.enableTrigger && this.outOnPointerExit)
			{
				this.triggerObject.SetActive(false);
			}
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00062C74 File Offset: 0x00060E74
		public void CreateNewItem(string title, bool value, bool notify)
		{
			DropdownMultiSelect.Item item = new DropdownMultiSelect.Item();
			item.itemName = title;
			item.isOn = value;
			this.items.Add(item);
			if (notify)
			{
				this.SetupDropdown();
			}
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00062CAC File Offset: 0x00060EAC
		public void CreateNewItem(string title, bool value)
		{
			DropdownMultiSelect.Item item = new DropdownMultiSelect.Item();
			item.itemName = title;
			item.isOn = value;
			this.items.Add(item);
			this.SetupDropdown();
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00062CE0 File Offset: 0x00060EE0
		public void CreateNewItem(string title)
		{
			DropdownMultiSelect.Item item = new DropdownMultiSelect.Item();
			item.itemName = title;
			this.items.Add(item);
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00062D08 File Offset: 0x00060F08
		public void RemoveItem(string itemTitle)
		{
			DropdownMultiSelect.Item item = this.items.Find((DropdownMultiSelect.Item x) => x.itemName == itemTitle);
			this.items.Remove(item);
			this.SetupDropdown();
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00062D50 File Offset: 0x00060F50
		public void UpdateItemLayout()
		{
			if (this.itemList != null)
			{
				this.itemList.spacing = (float)this.itemSpacing;
				this.itemList.padding.top = this.itemPaddingTop;
				this.itemList.padding.bottom = this.itemPaddingBottom;
				this.itemList.padding.left = this.itemPaddingLeft;
				this.itemList.padding.right = this.itemPaddingRight;
			}
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00062DD5 File Offset: 0x00060FD5
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.isInteractable)
			{
				return;
			}
			this.Animate();
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00062DE6 File Offset: 0x00060FE6
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.outOnPointerExit && this.isOn)
			{
				this.Animate();
				this.isOn = false;
				if (this.isListItem)
				{
					base.gameObject.transform.SetParent(this.currentListParent, true);
				}
			}
		}

		// Token: 0x04000F22 RID: 3874
		public GameObject triggerObject;

		// Token: 0x04000F23 RID: 3875
		public Transform itemParent;

		// Token: 0x04000F24 RID: 3876
		public GameObject itemObject;

		// Token: 0x04000F25 RID: 3877
		public GameObject scrollbar;

		// Token: 0x04000F26 RID: 3878
		private VerticalLayoutGroup itemList;

		// Token: 0x04000F27 RID: 3879
		private Transform currentListParent;

		// Token: 0x04000F28 RID: 3880
		public Transform listParent;

		// Token: 0x04000F29 RID: 3881
		private Animator dropdownAnimator;

		// Token: 0x04000F2A RID: 3882
		public TextMeshProUGUI setItemText;

		// Token: 0x04000F2B RID: 3883
		public CanvasGroup contentCG;

		// Token: 0x04000F2C RID: 3884
		public bool isInteractable = true;

		// Token: 0x04000F2D RID: 3885
		public bool initAtStart = true;

		// Token: 0x04000F2E RID: 3886
		public bool enableIcon = true;

		// Token: 0x04000F2F RID: 3887
		public bool enableTrigger = true;

		// Token: 0x04000F30 RID: 3888
		public bool enableScrollbar = true;

		// Token: 0x04000F31 RID: 3889
		public bool setHighPriority = true;

		// Token: 0x04000F32 RID: 3890
		public bool outOnPointerExit;

		// Token: 0x04000F33 RID: 3891
		public bool isListItem;

		// Token: 0x04000F34 RID: 3892
		public bool invokeAtStart;

		// Token: 0x04000F35 RID: 3893
		[Range(1f, 50f)]
		public int itemPaddingTop = 8;

		// Token: 0x04000F36 RID: 3894
		[Range(1f, 50f)]
		public int itemPaddingBottom = 8;

		// Token: 0x04000F37 RID: 3895
		[Range(1f, 50f)]
		public int itemPaddingLeft = 8;

		// Token: 0x04000F38 RID: 3896
		[Range(1f, 50f)]
		public int itemPaddingRight = 25;

		// Token: 0x04000F39 RID: 3897
		[Range(1f, 50f)]
		public int itemSpacing = 8;

		// Token: 0x04000F3A RID: 3898
		public DropdownMultiSelect.AnimationType animationType;

		// Token: 0x04000F3B RID: 3899
		[Range(1f, 25f)]
		public float transitionSmoothness = 10f;

		// Token: 0x04000F3C RID: 3900
		[Range(1f, 25f)]
		public float sizeSmoothness = 15f;

		// Token: 0x04000F3D RID: 3901
		public float panelSize = 200f;

		// Token: 0x04000F3E RID: 3902
		public RectTransform listRect;

		// Token: 0x04000F3F RID: 3903
		public CanvasGroup listCG;

		// Token: 0x04000F40 RID: 3904
		private bool isInTransition;

		// Token: 0x04000F41 RID: 3905
		private float closeOn;

		// Token: 0x04000F42 RID: 3906
		[SerializeField]
		public List<DropdownMultiSelect.Item> items = new List<DropdownMultiSelect.Item>();

		// Token: 0x04000F43 RID: 3907
		private bool isInitialized;

		// Token: 0x04000F44 RID: 3908
		private int currentIndex;

		// Token: 0x04000F45 RID: 3909
		private Toggle currentToggle;

		// Token: 0x04000F46 RID: 3910
		private string textHelper;

		// Token: 0x04000F47 RID: 3911
		private bool isOn;

		// Token: 0x04000F48 RID: 3912
		public int siblingIndex;

		// Token: 0x04000F49 RID: 3913
		private EventTrigger triggerEvent;

		// Token: 0x02000202 RID: 514
		[Serializable]
		public class ToggleEvent : UnityEvent<bool>
		{
		}

		// Token: 0x02000203 RID: 515
		public enum AnimationType
		{
			// Token: 0x04000F4B RID: 3915
			Modular,
			// Token: 0x04000F4C RID: 3916
			Stylish
		}

		// Token: 0x02000204 RID: 516
		[Serializable]
		public class Item
		{
			// Token: 0x04000F4D RID: 3917
			public string itemName = "Dropdown Item";

			// Token: 0x04000F4E RID: 3918
			public bool isOn;

			// Token: 0x04000F4F RID: 3919
			[HideInInspector]
			public int itemIndex;

			// Token: 0x04000F50 RID: 3920
			[SerializeField]
			public DropdownMultiSelect.ToggleEvent onValueChanged = new DropdownMultiSelect.ToggleEvent();
		}
	}
}
