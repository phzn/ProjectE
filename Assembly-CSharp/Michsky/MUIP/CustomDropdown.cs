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
	// Token: 0x020001F6 RID: 502
	public class CustomDropdown : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler, IPointerClickHandler, ISubmitHandler
	{
		// Token: 0x06000E67 RID: 3687 RVA: 0x00061310 File Offset: 0x0005F510
		private void CaptureListRect()
		{
			if (this.listRect == null || this._listRectSnapshotTaken)
			{
				return;
			}
			this._listAnchorMin = this.listRect.anchorMin;
			this._listAnchorMax = this.listRect.anchorMax;
			this._listPivot = this.listRect.pivot;
			this._listSizeDelta = this.listRect.sizeDelta;
			this._listAnchoredPos = this.listRect.anchoredPosition;
			this._listLocalScale = this.listRect.localScale;
			this._listRectSnapshotTaken = true;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x000613A4 File Offset: 0x0005F5A4
		private void RestoreListRect()
		{
			if (this.listRect == null || !this._listRectSnapshotTaken)
			{
				return;
			}
			this.listRect.anchorMin = this._listAnchorMin;
			this.listRect.anchorMax = this._listAnchorMax;
			this.listRect.pivot = this._listPivot;
			this.listRect.localScale = this._listLocalScale;
			this.listRect.sizeDelta = new Vector2(this._listSizeDelta.x, 0f);
			this.listRect.anchoredPosition = this._listAnchoredPos;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00061440 File Offset: 0x0005F640
		private void ReassertListAnchors()
		{
			if (this.listRect == null || !this._listRectSnapshotTaken)
			{
				return;
			}
			this.listRect.anchorMin = this._listAnchorMin;
			this.listRect.anchorMax = this._listAnchorMax;
			this.listRect.pivot = this._listPivot;
			this.listRect.localScale = this._listLocalScale;
			this.listRect.anchoredPosition = this._listAnchoredPos;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x000614BC File Offset: 0x0005F6BC
		private void OnEnable()
		{
			if (!this.isInitialized)
			{
				this.Initialize();
			}
			if (this.updateOnEnable && this.index < this.items.Count)
			{
				this.SetDropdownIndex(this.selectedItemIndex, false);
			}
			this.RestoreListRect();
			this.listCG.alpha = 0f;
			this.listCG.interactable = false;
			this.listCG.blocksRaycasts = false;
			this.listRect.sizeDelta = new Vector2(this.listRect.sizeDelta.x, 0f);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00061554 File Offset: 0x0005F754
		private void Initialize()
		{
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
			if (this.listCG == null)
			{
				this.listCG = base.gameObject.GetComponentInChildren<CanvasGroup>();
			}
			if (this.listRect == null)
			{
				this.listRect = this.listCG.GetComponent<RectTransform>();
			}
			this.CaptureListRect();
			if (this.setHighPriority)
			{
				if (this.contentCG == null)
				{
					this.contentCG = base.transform.Find("Content/Item List").GetComponent<CanvasGroup>();
				}
				this.contentCG.alpha = 1f;
				if (this.contentCG.gameObject.GetComponent<Canvas>() == null)
				{
					Canvas canvas = this.contentCG.gameObject.AddComponent<Canvas>();
					canvas.overrideSorting = true;
					canvas.sortingOrder = 30000;
				}
				if (this.contentCG.gameObject.GetComponent<GraphicRaycaster>() == null)
				{
					this.contentCG.gameObject.AddComponent<GraphicRaycaster>();
				}
			}
			this.dropdownAnimator = base.gameObject.GetComponent<Animator>();
			if (this.initAtStart && this.items.Count != 0)
			{
				this.SetupDropdown();
			}
			if (this.animationType == CustomDropdown.AnimationType.Modular && this.dropdownAnimator != null)
			{
				Object.Destroy(this.dropdownAnimator);
			}
			this.isInitialized = true;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00061700 File Offset: 0x0005F900
		public void SetupDropdown()
		{
			if (!this.enableScrollbar && this.scrollbar != null)
			{
				Object.Destroy(this.scrollbar);
			}
			if (this.itemList == null)
			{
				this.itemList = this.itemParent.GetComponent<VerticalLayoutGroup>();
			}
			this.UpdateItemLayout();
			this.index = 0;
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.itemParent, false);
				gameObject.name = this.items[i].itemName;
				this.setItemText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
				this.textHelper = this.items[i].itemName;
				this.setItemText.text = this.textHelper;
				CustomDropdown.ItemTextChangedEvent itemTextChangedEvent = this.onItemTextChanged;
				if (itemTextChangedEvent != null)
				{
					itemTextChangedEvent.Invoke(this.setItemText);
				}
				CustomDropdown.LocalizedItemTextChangedEvent localizedItemTextChangedEvent = this.onLocalizedItemTextChanged;
				if (localizedItemTextChangedEvent != null)
				{
					localizedItemTextChangedEvent.Invoke(this.setItemText, i);
				}
				Transform transform = gameObject.gameObject.transform.Find("Icon");
				this.setItemImage = transform.GetComponent<Image>();
				if (this.items[i].itemIcon == null)
				{
					this.setItemImage.gameObject.SetActive(false);
				}
				else
				{
					this.imageHelper = this.items[i].itemIcon;
					this.setItemImage.sprite = this.imageHelper;
				}
				this.items[i].itemIndex = i;
				CustomDropdown.Item mainItem = this.items[i];
				Button component = gameObject.GetComponent<Button>();
				component.onClick.AddListener(new UnityAction(this.Animate));
				component.onClick.AddListener(new UnityAction(this.items[i].OnItemSelection.Invoke));
				component.onClick.AddListener(delegate
				{
					this.SetDropdownIndex(this.index = mainItem.itemIndex);
					this.onValueChanged.Invoke(this.index = mainItem.itemIndex);
					if (this.saveSelected)
					{
						PlayerPrefs.SetInt("Dropdown_" + this.saveKey, mainItem.itemIndex);
					}
				});
			}
			if (this.selectedImage != null && !this.enableIcon)
			{
				this.selectedImage.gameObject.SetActive(false);
			}
			else if (this.selectedImage != null)
			{
				this.selectedImage.sprite = this.items[this.selectedItemIndex].itemIcon;
			}
			if (this.selectedText != null)
			{
				this.selectedText.text = this.items[this.selectedItemIndex].itemName;
				CustomDropdown.ItemTextChangedEvent itemTextChangedEvent2 = this.onItemTextChanged;
				if (itemTextChangedEvent2 != null)
				{
					itemTextChangedEvent2.Invoke(this.selectedText);
				}
				CustomDropdown.LocalizedItemTextChangedEvent localizedItemTextChangedEvent2 = this.onLocalizedItemTextChanged;
				if (localizedItemTextChangedEvent2 != null)
				{
					localizedItemTextChangedEvent2.Invoke(this.selectedText, this.selectedItemIndex);
				}
			}
			if (!this.saveSelected)
			{
				if (this.invokeAtStart)
				{
					this.items[this.selectedItemIndex].OnItemSelection.Invoke();
				}
				return;
			}
			if (this.invokeAtStart)
			{
				this.items[PlayerPrefs.GetInt("Dropdown_" + this.saveKey)].OnItemSelection.Invoke();
				return;
			}
			this.SetDropdownIndex(PlayerPrefs.GetInt("Dropdown_" + this.saveKey), false);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00061AB4 File Offset: 0x0005FCB4
		public void ChangeDropdownInfo(int itemIndex)
		{
			this.SetDropdownIndex(itemIndex);
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00061ABD File Offset: 0x0005FCBD
		public void SetDropdownIndex(int itemIndex)
		{
			this.SetDropdownIndex(itemIndex, true);
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00061AC8 File Offset: 0x0005FCC8
		public void SetDropdownIndex(int itemIndex, bool bypassSound = false)
		{
			if (this.selectedImage != null && this.enableIcon && this.items[itemIndex].itemIcon != null)
			{
				this.selectedImage.gameObject.SetActive(true);
				this.selectedImage.sprite = this.items[itemIndex].itemIcon;
			}
			else if (this.selectedImage != null && this.enableIcon && this.items[itemIndex].itemIcon == null)
			{
				this.selectedImage.gameObject.SetActive(false);
			}
			if (this.selectedText != null)
			{
				this.selectedText.text = this.items[itemIndex].itemName;
				CustomDropdown.ItemTextChangedEvent itemTextChangedEvent = this.onItemTextChanged;
				if (itemTextChangedEvent != null)
				{
					itemTextChangedEvent.Invoke(this.selectedText);
				}
				CustomDropdown.LocalizedItemTextChangedEvent localizedItemTextChangedEvent = this.onLocalizedItemTextChanged;
				if (localizedItemTextChangedEvent != null)
				{
					localizedItemTextChangedEvent.Invoke(this.selectedText, itemIndex);
				}
			}
			if (!bypassSound && this.enableDropdownSounds && this.useClickSound)
			{
				this.soundSource.PlayOneShot(this.clickSound);
			}
			this.selectedItemIndex = itemIndex;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00061BF8 File Offset: 0x0005FDF8
		public void Animate()
		{
			if (!this.isOn && this.animationType == CustomDropdown.AnimationType.Modular)
			{
				this.isOn = true;
				this.listCG.blocksRaycasts = true;
				this.listCG.interactable = true;
				this.listCG.gameObject.SetActive(true);
				this.RestoreListRect();
				base.StopCoroutine("StartMinimize");
				base.StopCoroutine("StartExpand");
				base.StartCoroutine("StartExpand");
			}
			else if (this.isOn && this.animationType == CustomDropdown.AnimationType.Modular)
			{
				this.isOn = false;
				this.listCG.blocksRaycasts = false;
				this.listCG.interactable = false;
				base.StopCoroutine("StartMinimize");
				base.StopCoroutine("StartExpand");
				base.StartCoroutine("StartMinimize");
			}
			else if (!this.isOn && this.animationType == CustomDropdown.AnimationType.Custom)
			{
				this.dropdownAnimator.Play("Stylish In");
				this.isOn = true;
			}
			else if (this.isOn && this.animationType == CustomDropdown.AnimationType.Custom)
			{
				this.dropdownAnimator.Play("Stylish Out");
				this.isOn = false;
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

		// Token: 0x06000E71 RID: 3697 RVA: 0x00061D6D File Offset: 0x0005FF6D
		public void Interactable(bool value)
		{
			this.isInteractable = value;
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00061D78 File Offset: 0x0005FF78
		public void CreateNewItem(string title, Sprite icon, bool notify = false)
		{
			CustomDropdown.Item item = new CustomDropdown.Item
			{
				itemName = title,
				itemIcon = icon
			};
			this.items.Add(item);
			if (this.selectedItemIndex > this.items.Count)
			{
				this.selectedItemIndex = 0;
			}
			if (notify)
			{
				this.SetupDropdown();
			}
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x00061DC8 File Offset: 0x0005FFC8
		public void CreateNewItem(string title, bool notify = false)
		{
			CustomDropdown.Item item = new CustomDropdown.Item
			{
				itemName = title
			};
			this.items.Add(item);
			if (this.selectedItemIndex > this.items.Count)
			{
				this.selectedItemIndex = 0;
			}
			if (notify)
			{
				this.SetupDropdown();
			}
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00061E14 File Offset: 0x00060014
		public void RemoveItem(string itemTitle, bool notify = false)
		{
			CustomDropdown.Item item = this.items.Find((CustomDropdown.Item x) => x.itemName == itemTitle);
			this.items.Remove(item);
			if (this.selectedItemIndex > this.items.Count)
			{
				this.selectedItemIndex = 0;
			}
			if (notify)
			{
				this.SetupDropdown();
			}
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00061E78 File Offset: 0x00060078
		public void UpdateItemLayout()
		{
			if (this.itemList == null)
			{
				return;
			}
			this.itemList.spacing = (float)this.itemSpacing;
			this.itemList.padding.top = this.itemPaddingTop;
			this.itemList.padding.bottom = this.itemPaddingBottom;
			this.itemList.padding.left = this.itemPaddingLeft;
			this.itemList.padding.right = this.itemPaddingRight;
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00061EFE File Offset: 0x000600FE
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.isInteractable)
			{
				return;
			}
			if (this.enableDropdownSounds && this.useClickSound)
			{
				this.soundSource.PlayOneShot(this.clickSound);
			}
			this.Animate();
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00061F30 File Offset: 0x00060130
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this.isInteractable)
			{
				return;
			}
			if (this.enableDropdownSounds && this.useHoverSound)
			{
				this.soundSource.PlayOneShot(this.hoverSound);
			}
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00061F5C File Offset: 0x0006015C
		public void OnPointerExit(PointerEventData eventData)
		{
			if (!this.isInteractable)
			{
				return;
			}
			if (this.outOnPointerExit && this.isOn)
			{
				this.Animate();
				this.isOn = false;
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00061EFE File Offset: 0x000600FE
		public void OnSubmit(BaseEventData eventData)
		{
			if (!this.isInteractable)
			{
				return;
			}
			if (this.enableDropdownSounds && this.useClickSound)
			{
				this.soundSource.PlayOneShot(this.clickSound);
			}
			this.Animate();
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00061F84 File Offset: 0x00060184
		private IEnumerator StartExpand()
		{
			float elapsedTime = 0f;
			this.RestoreListRect();
			Vector2 startPos = this.listRect.sizeDelta;
			Vector2 endPos = new Vector2(this.listRect.sizeDelta.x, this.panelSize);
			while (this.listRect.sizeDelta.y <= this.panelSize - 0.1f)
			{
				elapsedTime += Time.unscaledDeltaTime;
				this.listCG.alpha += Time.unscaledDeltaTime * (this.curveSpeed * 2f);
				this.listRect.sizeDelta = Vector2.Lerp(startPos, endPos, this.animationCurve.Evaluate(elapsedTime * this.curveSpeed));
				this.ReassertListAnchors();
				yield return null;
			}
			this.listCG.alpha = 1f;
			this.listRect.sizeDelta = endPos;
			this.ReassertListAnchors();
			yield break;
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00061F93 File Offset: 0x00060193
		private IEnumerator StartMinimize()
		{
			float elapsedTime = 0f;
			Vector2 startPos = this.listRect.sizeDelta;
			Vector2 endPos = new Vector2(this.listRect.sizeDelta.x, 0f);
			while (this.listRect.sizeDelta.y >= 0.1f)
			{
				elapsedTime += Time.unscaledDeltaTime;
				this.listCG.alpha -= Time.unscaledDeltaTime * (this.curveSpeed * 2f);
				this.listRect.sizeDelta = Vector2.Lerp(startPos, endPos, this.animationCurve.Evaluate(elapsedTime * this.curveSpeed));
				yield return null;
			}
			this.listCG.alpha = 0f;
			this.listRect.sizeDelta = endPos;
			this.listCG.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04000ECE RID: 3790
		public Animator dropdownAnimator;

		// Token: 0x04000ECF RID: 3791
		public GameObject triggerObject;

		// Token: 0x04000ED0 RID: 3792
		public TextMeshProUGUI selectedText;

		// Token: 0x04000ED1 RID: 3793
		public Image selectedImage;

		// Token: 0x04000ED2 RID: 3794
		public Transform itemParent;

		// Token: 0x04000ED3 RID: 3795
		public GameObject itemObject;

		// Token: 0x04000ED4 RID: 3796
		public GameObject scrollbar;

		// Token: 0x04000ED5 RID: 3797
		public VerticalLayoutGroup itemList;

		// Token: 0x04000ED6 RID: 3798
		public AudioSource soundSource;

		// Token: 0x04000ED7 RID: 3799
		public RectTransform listRect;

		// Token: 0x04000ED8 RID: 3800
		public CanvasGroup listCG;

		// Token: 0x04000ED9 RID: 3801
		public CanvasGroup contentCG;

		// Token: 0x04000EDA RID: 3802
		public bool isInteractable = true;

		// Token: 0x04000EDB RID: 3803
		public bool enableIcon = true;

		// Token: 0x04000EDC RID: 3804
		public bool enableTrigger = true;

		// Token: 0x04000EDD RID: 3805
		public bool enableScrollbar = true;

		// Token: 0x04000EDE RID: 3806
		public bool updateOnEnable = true;

		// Token: 0x04000EDF RID: 3807
		public bool outOnPointerExit;

		// Token: 0x04000EE0 RID: 3808
		public bool setHighPriority = true;

		// Token: 0x04000EE1 RID: 3809
		public bool invokeAtStart;

		// Token: 0x04000EE2 RID: 3810
		public bool initAtStart = true;

		// Token: 0x04000EE3 RID: 3811
		public bool enableDropdownSounds;

		// Token: 0x04000EE4 RID: 3812
		public bool useHoverSound = true;

		// Token: 0x04000EE5 RID: 3813
		public bool useClickSound = true;

		// Token: 0x04000EE6 RID: 3814
		[Range(1f, 50f)]
		public int itemPaddingTop = 8;

		// Token: 0x04000EE7 RID: 3815
		[Range(1f, 50f)]
		public int itemPaddingBottom = 8;

		// Token: 0x04000EE8 RID: 3816
		[Range(1f, 50f)]
		public int itemPaddingLeft = 8;

		// Token: 0x04000EE9 RID: 3817
		[Range(1f, 50f)]
		public int itemPaddingRight = 25;

		// Token: 0x04000EEA RID: 3818
		[Range(1f, 50f)]
		public int itemSpacing = 8;

		// Token: 0x04000EEB RID: 3819
		public int selectedItemIndex;

		// Token: 0x04000EEC RID: 3820
		public CustomDropdown.AnimationType animationType;

		// Token: 0x04000EED RID: 3821
		public CustomDropdown.PanelDirection panelDirection;

		// Token: 0x04000EEE RID: 3822
		[Range(25f, 1000f)]
		public float panelSize = 200f;

		// Token: 0x04000EEF RID: 3823
		[Range(0.5f, 10f)]
		public float curveSpeed = 3f;

		// Token: 0x04000EF0 RID: 3824
		public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000EF1 RID: 3825
		public bool saveSelected;

		// Token: 0x04000EF2 RID: 3826
		public string saveKey = "My Dropdown";

		// Token: 0x04000EF3 RID: 3827
		[SerializeField]
		public List<CustomDropdown.Item> items = new List<CustomDropdown.Item>();

		// Token: 0x04000EF4 RID: 3828
		public CustomDropdown.DropdownEvent onValueChanged = new CustomDropdown.DropdownEvent();

		// Token: 0x04000EF5 RID: 3829
		public CustomDropdown.ItemTextChangedEvent onItemTextChanged = new CustomDropdown.ItemTextChangedEvent();

		// Token: 0x04000EF6 RID: 3830
		[NonSerialized]
		public CustomDropdown.LocalizedItemTextChangedEvent onLocalizedItemTextChanged = new CustomDropdown.LocalizedItemTextChangedEvent();

		// Token: 0x04000EF7 RID: 3831
		public AudioClip hoverSound;

		// Token: 0x04000EF8 RID: 3832
		public AudioClip clickSound;

		// Token: 0x04000EF9 RID: 3833
		private bool isInitialized;

		// Token: 0x04000EFA RID: 3834
		[HideInInspector]
		public bool isOn;

		// Token: 0x04000EFB RID: 3835
		[HideInInspector]
		public int index;

		// Token: 0x04000EFC RID: 3836
		[HideInInspector]
		public int siblingIndex;

		// Token: 0x04000EFD RID: 3837
		[HideInInspector]
		public TextMeshProUGUI setItemText;

		// Token: 0x04000EFE RID: 3838
		[HideInInspector]
		public Image setItemImage;

		// Token: 0x04000EFF RID: 3839
		private EventTrigger triggerEvent;

		// Token: 0x04000F00 RID: 3840
		private Sprite imageHelper;

		// Token: 0x04000F01 RID: 3841
		private string textHelper;

		// Token: 0x04000F02 RID: 3842
		private bool _listRectSnapshotTaken;

		// Token: 0x04000F03 RID: 3843
		private Vector2 _listAnchorMin;

		// Token: 0x04000F04 RID: 3844
		private Vector2 _listAnchorMax;

		// Token: 0x04000F05 RID: 3845
		private Vector2 _listPivot;

		// Token: 0x04000F06 RID: 3846
		private Vector2 _listSizeDelta;

		// Token: 0x04000F07 RID: 3847
		private Vector2 _listAnchoredPos;

		// Token: 0x04000F08 RID: 3848
		private Vector3 _listLocalScale;

		// Token: 0x020001F7 RID: 503
		[Serializable]
		public class DropdownEvent : UnityEvent<int>
		{
		}

		// Token: 0x020001F8 RID: 504
		[Serializable]
		public class ItemTextChangedEvent : UnityEvent<TMP_Text>
		{
		}

		// Token: 0x020001F9 RID: 505
		[Serializable]
		public class LocalizedItemTextChangedEvent : UnityEvent<TMP_Text, int>
		{
		}

		// Token: 0x020001FA RID: 506
		public enum AnimationType
		{
			// Token: 0x04000F0A RID: 3850
			Modular,
			// Token: 0x04000F0B RID: 3851
			Custom
		}

		// Token: 0x020001FB RID: 507
		public enum PanelDirection
		{
			// Token: 0x04000F0D RID: 3853
			Bottom,
			// Token: 0x04000F0E RID: 3854
			Top
		}

		// Token: 0x020001FC RID: 508
		[Serializable]
		public class Item
		{
			// Token: 0x04000F0F RID: 3855
			public string itemName = "Dropdown Item";

			// Token: 0x04000F10 RID: 3856
			public Sprite itemIcon;

			// Token: 0x04000F11 RID: 3857
			[HideInInspector]
			public int itemIndex;

			// Token: 0x04000F12 RID: 3858
			public UnityEvent OnItemSelection = new UnityEvent();
		}
	}
}
