using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000207 RID: 519
	[RequireComponent(typeof(Animator))]
	public class HorizontalSelector : MonoBehaviour
	{
		// Token: 0x06000EA9 RID: 3753 RVA: 0x00062F0C File Offset: 0x0006110C
		private void Awake()
		{
			if (this.selectorAnimator == null)
			{
				this.selectorAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (this.label == null || this.labelHelper == null)
			{
				Debug.LogError("<b>[Horizontal Selector]</b> Cannot initalize the object due to missing resources.", this);
				return;
			}
			this.SetupSelector();
			this.UpdateContentLayout();
			if (this.invokeAtStart)
			{
				this.items[this.index].onItemSelect.Invoke();
				this.onValueChanged.Invoke(this.index);
			}
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00062FA0 File Offset: 0x000611A0
		private void OnEnable()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine("DisableAnimator");
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00062FBC File Offset: 0x000611BC
		public void SetupSelector()
		{
			if (this.items.Count == 0)
			{
				return;
			}
			if (this.saveSelected)
			{
				if (PlayerPrefs.HasKey("HorizontalSelector_" + this.saveKey))
				{
					this.defaultIndex = PlayerPrefs.GetInt("HorizontalSelector_" + this.saveKey);
				}
				else
				{
					PlayerPrefs.SetInt("HorizontalSelector_" + this.saveKey, this.defaultIndex);
				}
			}
			this.label.text = this.items[this.defaultIndex].itemTitle;
			this.labelHelper.text = this.label.text;
			HorizontalSelector.ItemTextChangedEvent itemTextChangedEvent = this.onItemTextChanged;
			if (itemTextChangedEvent != null)
			{
				itemTextChangedEvent.Invoke(this.label);
			}
			if (this.labelIcon != null && this.enableIcon)
			{
				this.labelIcon.sprite = this.items[this.defaultIndex].itemIcon;
				this.labelIconHelper.sprite = this.labelIcon.sprite;
			}
			else if (!this.enableIcon)
			{
				if (this.labelIcon != null)
				{
					this.labelIcon.gameObject.SetActive(false);
				}
				if (this.labelIconHelper != null)
				{
					this.labelIconHelper.gameObject.SetActive(false);
				}
			}
			this.index = this.defaultIndex;
			if (this.enableIndicators)
			{
				this.UpdateIndicators();
				return;
			}
			if (this.indicatorParent != null)
			{
				Object.Destroy(this.indicatorParent.gameObject);
			}
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0006314C File Offset: 0x0006134C
		public void PreviousItem()
		{
			if (this.items.Count == 0)
			{
				return;
			}
			base.StopCoroutine("DisableAnimator");
			this.selectorAnimator.enabled = true;
			if (!this.loopSelection)
			{
				if (this.index != 0)
				{
					this.labelHelper.text = this.label.text;
					if (this.labelIcon != null && this.enableIcon)
					{
						this.labelIconHelper.sprite = this.labelIcon.sprite;
					}
					if (this.index == 0)
					{
						this.index = this.items.Count - 1;
					}
					else
					{
						this.index--;
					}
					this.label.text = this.items[this.index].itemTitle;
					HorizontalSelector.ItemTextChangedEvent itemTextChangedEvent = this.onItemTextChanged;
					if (itemTextChangedEvent != null)
					{
						itemTextChangedEvent.Invoke(this.label);
					}
					if (this.labelIcon != null && this.enableIcon)
					{
						this.labelIcon.sprite = this.items[this.index].itemIcon;
					}
					this.items[this.index].onItemSelect.Invoke();
					this.onValueChanged.Invoke(this.index);
					this.selectorAnimator.Play(null);
					this.selectorAnimator.StopPlayback();
					if (this.invertAnimation)
					{
						this.selectorAnimator.Play("Forward");
					}
					else
					{
						this.selectorAnimator.Play("Previous");
					}
				}
			}
			else
			{
				this.labelHelper.text = this.label.text;
				if (this.labelIcon != null && this.enableIcon)
				{
					this.labelIconHelper.sprite = this.labelIcon.sprite;
				}
				if (this.index == 0)
				{
					this.index = this.items.Count - 1;
				}
				else
				{
					this.index--;
				}
				this.label.text = this.items[this.index].itemTitle;
				HorizontalSelector.ItemTextChangedEvent itemTextChangedEvent2 = this.onItemTextChanged;
				if (itemTextChangedEvent2 != null)
				{
					itemTextChangedEvent2.Invoke(this.label);
				}
				if (this.labelIcon != null && this.enableIcon)
				{
					this.labelIcon.sprite = this.items[this.index].itemIcon;
				}
				this.items[this.index].onItemSelect.Invoke();
				this.onValueChanged.Invoke(this.index);
				this.selectorAnimator.Play(null);
				this.selectorAnimator.StopPlayback();
				if (this.invertAnimation)
				{
					this.selectorAnimator.Play("Forward");
				}
				else
				{
					this.selectorAnimator.Play("Previous");
				}
			}
			if (this.saveSelected)
			{
				PlayerPrefs.SetInt("HorizontalSelector_" + this.saveKey, this.index);
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine("DisableAnimator");
			}
			if (this.enableIndicators)
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					GameObject gameObject = this.indicatorParent.GetChild(i).gameObject;
					Transform transform = gameObject.transform.Find("On");
					Transform transform2 = gameObject.transform.Find("Off");
					if (i == this.index)
					{
						transform.gameObject.SetActive(true);
						transform2.gameObject.SetActive(false);
					}
					else
					{
						transform.gameObject.SetActive(false);
						transform2.gameObject.SetActive(true);
					}
				}
			}
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x000634F8 File Offset: 0x000616F8
		public void NextItem()
		{
			if (this.items.Count == 0)
			{
				return;
			}
			base.StopCoroutine("DisableAnimator");
			this.selectorAnimator.enabled = true;
			if (!this.loopSelection)
			{
				if (this.index != this.items.Count - 1)
				{
					this.labelHelper.text = this.label.text;
					if (this.labelIcon != null && this.enableIcon)
					{
						this.labelIconHelper.sprite = this.labelIcon.sprite;
					}
					if (this.index + 1 >= this.items.Count)
					{
						this.index = 0;
					}
					else
					{
						this.index++;
					}
					this.label.text = this.items[this.index].itemTitle;
					HorizontalSelector.ItemTextChangedEvent itemTextChangedEvent = this.onItemTextChanged;
					if (itemTextChangedEvent != null)
					{
						itemTextChangedEvent.Invoke(this.label);
					}
					if (this.labelIcon != null && this.enableIcon)
					{
						this.labelIcon.sprite = this.items[this.index].itemIcon;
					}
					this.items[this.index].onItemSelect.Invoke();
					this.onValueChanged.Invoke(this.index);
					this.selectorAnimator.Play(null);
					this.selectorAnimator.StopPlayback();
					if (this.invertAnimation)
					{
						this.selectorAnimator.Play("Previous");
					}
					else
					{
						this.selectorAnimator.Play("Forward");
					}
				}
			}
			else
			{
				this.labelHelper.text = this.label.text;
				if (this.labelIcon != null && this.enableIcon)
				{
					this.labelIconHelper.sprite = this.labelIcon.sprite;
				}
				if (this.index + 1 >= this.items.Count)
				{
					this.index = 0;
				}
				else
				{
					this.index++;
				}
				this.label.text = this.items[this.index].itemTitle;
				HorizontalSelector.ItemTextChangedEvent itemTextChangedEvent2 = this.onItemTextChanged;
				if (itemTextChangedEvent2 != null)
				{
					itemTextChangedEvent2.Invoke(this.label);
				}
				if (this.labelIcon != null && this.enableIcon)
				{
					this.labelIcon.sprite = this.items[this.index].itemIcon;
				}
				this.items[this.index].onItemSelect.Invoke();
				this.onValueChanged.Invoke(this.index);
				this.selectorAnimator.Play(null);
				this.selectorAnimator.StopPlayback();
				if (this.invertAnimation)
				{
					this.selectorAnimator.Play("Previous");
				}
				else
				{
					this.selectorAnimator.Play("Forward");
				}
			}
			if (this.saveSelected)
			{
				PlayerPrefs.SetInt("HorizontalSelector_" + this.saveKey, this.index);
			}
			if (this.enableIndicators)
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					GameObject gameObject = this.indicatorParent.GetChild(i).gameObject;
					Transform transform = gameObject.transform.Find("On");
					Transform transform2 = gameObject.transform.Find("Off");
					if (i == this.index)
					{
						transform.gameObject.SetActive(true);
						transform2.gameObject.SetActive(false);
					}
					else
					{
						transform.gameObject.SetActive(false);
						transform2.gameObject.SetActive(true);
					}
				}
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine("DisableAnimator");
			}
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x000638B3 File Offset: 0x00061AB3
		public void PreviousClick()
		{
			this.PreviousItem();
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x000638BB File Offset: 0x00061ABB
		public void ForwardClick()
		{
			this.NextItem();
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x000638C4 File Offset: 0x00061AC4
		public void CreateNewItem(string title)
		{
			HorizontalSelector.Item item = new HorizontalSelector.Item();
			this.newItemTitle = title;
			item.itemTitle = this.newItemTitle;
			this.items.Add(item);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x000638F8 File Offset: 0x00061AF8
		public void CreateNewItem(string title, Sprite icon)
		{
			HorizontalSelector.Item item = new HorizontalSelector.Item();
			this.newItemTitle = title;
			item.itemTitle = this.newItemTitle;
			item.itemIcon = icon;
			this.items.Add(item);
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00063934 File Offset: 0x00061B34
		public void RemoveItem(string itemTitle)
		{
			HorizontalSelector.Item item = this.items.Find((HorizontalSelector.Item x) => x.itemTitle == itemTitle);
			this.items.Remove(item);
			this.SetupSelector();
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0006397C File Offset: 0x00061B7C
		public void UpdateUI()
		{
			this.selectorAnimator.enabled = true;
			this.label.text = this.items[this.index].itemTitle;
			HorizontalSelector.ItemTextChangedEvent itemTextChangedEvent = this.onItemTextChanged;
			if (itemTextChangedEvent != null)
			{
				itemTextChangedEvent.Invoke(this.label);
			}
			if (this.labelIcon != null && this.enableIcon)
			{
				this.labelIcon.sprite = this.items[this.index].itemIcon;
			}
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine("DisableAnimator");
			}
			this.UpdateContentLayout();
			this.UpdateIndicators();
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00063A2C File Offset: 0x00061C2C
		public void UpdateIndicators()
		{
			if (!this.enableIndicators)
			{
				return;
			}
			foreach (object obj in this.indicatorParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.indicatorObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.indicatorParent, false);
				gameObject.name = this.items[i].itemTitle;
				Transform transform = gameObject.transform.Find("On");
				Transform transform2 = gameObject.transform.Find("Off");
				if (i == this.index)
				{
					transform.gameObject.SetActive(true);
					transform2.gameObject.SetActive(false);
				}
				else
				{
					transform.gameObject.SetActive(false);
					transform2.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00063B58 File Offset: 0x00061D58
		public void UpdateContentLayout()
		{
			if (this.contentLayout != null)
			{
				this.contentLayout.spacing = (float)this.contentSpacing;
			}
			if (this.contentLayoutHelper != null)
			{
				this.contentLayoutHelper.spacing = (float)this.contentSpacing;
			}
			if (this.labelIcon != null)
			{
				this.labelIcon.transform.localScale = new Vector3(this.iconScale, this.iconScale, this.iconScale);
				this.labelIconHelper.transform.localScale = new Vector3(this.iconScale, this.iconScale, this.iconScale);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.label.transform.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.label.transform.parent.GetComponent<RectTransform>());
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00063C30 File Offset: 0x00061E30
		private IEnumerator DisableAnimator()
		{
			yield return new WaitForSecondsRealtime(0.5f);
			this.selectorAnimator.enabled = false;
			yield break;
		}

		// Token: 0x04000F54 RID: 3924
		public TextMeshProUGUI label;

		// Token: 0x04000F55 RID: 3925
		public TextMeshProUGUI labelHelper;

		// Token: 0x04000F56 RID: 3926
		public Image labelIcon;

		// Token: 0x04000F57 RID: 3927
		public Image labelIconHelper;

		// Token: 0x04000F58 RID: 3928
		public Transform indicatorParent;

		// Token: 0x04000F59 RID: 3929
		public GameObject indicatorObject;

		// Token: 0x04000F5A RID: 3930
		public Animator selectorAnimator;

		// Token: 0x04000F5B RID: 3931
		public HorizontalLayoutGroup contentLayout;

		// Token: 0x04000F5C RID: 3932
		public HorizontalLayoutGroup contentLayoutHelper;

		// Token: 0x04000F5D RID: 3933
		private string newItemTitle;

		// Token: 0x04000F5E RID: 3934
		public bool enableIcon = true;

		// Token: 0x04000F5F RID: 3935
		public bool saveSelected;

		// Token: 0x04000F60 RID: 3936
		public string saveKey = "My Selector";

		// Token: 0x04000F61 RID: 3937
		public bool enableIndicators = true;

		// Token: 0x04000F62 RID: 3938
		public bool invokeAtStart;

		// Token: 0x04000F63 RID: 3939
		public bool invertAnimation;

		// Token: 0x04000F64 RID: 3940
		public bool loopSelection;

		// Token: 0x04000F65 RID: 3941
		[Range(0.25f, 2.5f)]
		public float iconScale = 1f;

		// Token: 0x04000F66 RID: 3942
		[Range(1f, 50f)]
		public int contentSpacing = 15;

		// Token: 0x04000F67 RID: 3943
		public int defaultIndex;

		// Token: 0x04000F68 RID: 3944
		[HideInInspector]
		public int index;

		// Token: 0x04000F69 RID: 3945
		public List<HorizontalSelector.Item> items = new List<HorizontalSelector.Item>();

		// Token: 0x04000F6A RID: 3946
		public HorizontalSelector.SelectorEvent onValueChanged;

		// Token: 0x04000F6B RID: 3947
		public HorizontalSelector.ItemTextChangedEvent onItemTextChanged;

		// Token: 0x02000208 RID: 520
		[Serializable]
		public class SelectorEvent : UnityEvent<int>
		{
		}

		// Token: 0x02000209 RID: 521
		[Serializable]
		public class ItemTextChangedEvent : UnityEvent<TMP_Text>
		{
		}

		// Token: 0x0200020A RID: 522
		[Serializable]
		public class Item
		{
			// Token: 0x04000F6C RID: 3948
			public string itemTitle = "Item Title";

			// Token: 0x04000F6D RID: 3949
			public Sprite itemIcon;

			// Token: 0x04000F6E RID: 3950
			public UnityEvent onItemSelect = new UnityEvent();
		}
	}
}
