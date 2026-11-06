using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000035 RID: 53
	public class MMDebugMenu : MonoBehaviour
	{
		// Token: 0x0600011A RID: 282 RVA: 0x00005B3C File Offset: 0x00003D3C
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005B44 File Offset: 0x00003D44
		protected virtual void Initialization()
		{
			if (this.Data != null)
			{
				this.FillMenu(false);
			}
			this.CloseIcon.color = this.Data.TextColor;
			this._containerRect = this.MenuContainer.GetComponent<RectTransform>();
			this._initialContainerPosition = this._containerRect.localPosition;
			this.MenuBackground.color = this.Data.BackgroundColor;
			switch (this.Data.ToggleDirection)
			{
			case MMDebugMenu.ToggleDirections.TopToBottom:
				this._offPosition = this._initialContainerPosition + Vector3.up * this._containerRect.rect.height;
				break;
			case MMDebugMenu.ToggleDirections.LeftToRight:
				this._offPosition = this._initialContainerPosition + Vector3.left * this._containerRect.rect.width;
				break;
			case MMDebugMenu.ToggleDirections.RightToLeft:
				this._offPosition = this._initialContainerPosition + Vector3.right * this._containerRect.rect.width;
				break;
			case MMDebugMenu.ToggleDirections.BottomToTop:
				this._offPosition = this._initialContainerPosition + Vector3.down * this._containerRect.rect.height;
				break;
			}
			this._containerRect.localPosition = this._offPosition;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005CB0 File Offset: 0x00003EB0
		public virtual void FillMenu(bool triggerEvents = false)
		{
			int num = 0;
			if (this.MMLogo != null)
			{
				this.MMLogo.color = this.Data.TextColor;
			}
			foreach (object obj in this.Contents.transform)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			foreach (object obj2 in this.TabBar.transform)
			{
				Object.Destroy(((Transform)obj2).gameObject);
			}
			this.TabManager.Tabs.Clear();
			this.TabManager.TabsContents.Clear();
			foreach (MMDebugMenuTabData mmdebugMenuTabData in this.Data.Tabs)
			{
				if (mmdebugMenuTabData.Active)
				{
					MMDebugMenuTab mmdebugMenuTab = Object.Instantiate<MMDebugMenuTab>(this.Data.TabPrefab);
					mmdebugMenuTab.SelectedBackgroundColor = this.Data.TextColor;
					mmdebugMenuTab.SelectedTextColor = this.Data.BackgroundColor;
					mmdebugMenuTab.DeselectedBackgroundColor = this.Data.BackgroundColor;
					mmdebugMenuTab.DeselectedTextColor = this.Data.TextColor;
					mmdebugMenuTab.TabText.text = mmdebugMenuTabData.Name;
					mmdebugMenuTab.TabText.font = this.Data.RegularFont;
					mmdebugMenuTab.transform.SetParent(this.TabBar);
					mmdebugMenuTab.Index = num;
					mmdebugMenuTab.Manager = this.TabManager;
					this.TabManager.Tabs.Add(mmdebugMenuTab);
					MMDebugMenuTabContents mmdebugMenuTabContents = Object.Instantiate<MMDebugMenuTabContents>(this.Data.TabContentsPrefab);
					mmdebugMenuTabContents.transform.SetParent(this.TabContainer);
					RectTransform component = mmdebugMenuTabContents.GetComponent<RectTransform>();
					component.MMSetLeft(0f);
					component.MMSetRight(0f);
					component.MMSetTop(0f);
					component.MMSetBottom(0f);
					mmdebugMenuTabContents.Index = num;
					this.FillTab(mmdebugMenuTabContents, num, triggerEvents);
					if (num == this.Data.InitialActiveTabIndex)
					{
						mmdebugMenuTabContents.gameObject.SetActive(true);
						mmdebugMenuTab.Select();
					}
					else
					{
						mmdebugMenuTabContents.gameObject.SetActive(false);
						mmdebugMenuTab.Deselect();
					}
					this.TabManager.TabsContents.Add(mmdebugMenuTabContents);
					num++;
				}
			}
			if (this.Data.DisplayDebugTab)
			{
				MMDebugMenuTab mmdebugMenuTab2 = Object.Instantiate<MMDebugMenuTab>(this.Data.TabPrefab);
				mmdebugMenuTab2.SelectedBackgroundColor = this.Data.TextColor;
				mmdebugMenuTab2.SelectedTextColor = this.Data.BackgroundColor;
				mmdebugMenuTab2.DeselectedBackgroundColor = this.Data.BackgroundColor;
				mmdebugMenuTab2.DeselectedTextColor = this.Data.TextColor;
				mmdebugMenuTab2.TabText.text = this.Data.DebugTabName;
				mmdebugMenuTab2.TabText.font = this.Data.RegularFont;
				mmdebugMenuTab2.transform.SetParent(this.TabBar);
				mmdebugMenuTab2.Index = num;
				mmdebugMenuTab2.Manager = this.TabManager;
				this.TabManager.Tabs.Add(mmdebugMenuTab2);
				MMDebugMenuDebugTab mmdebugMenuDebugTab = Object.Instantiate<MMDebugMenuDebugTab>(this.Data.DebugTabPrefab);
				mmdebugMenuDebugTab.DebugText.color = this.Data.TextColor;
				mmdebugMenuDebugTab.DebugText.font = this.Data.RegularFont;
				mmdebugMenuDebugTab.transform.SetParent(this.TabContainer);
				mmdebugMenuDebugTab.CommandPrompt.textComponent.font = this.Data.RegularFont;
				mmdebugMenuDebugTab.CommandPrompt.textComponent.color = this.Data.TextColor;
				mmdebugMenuDebugTab.CommandPromptCharacter.font = this.Data.RegularFont;
				mmdebugMenuDebugTab.CommandPromptCharacter.color = this.Data.TextColor;
				MMDebugMenuTabContents component2 = mmdebugMenuDebugTab.GetComponent<MMDebugMenuTabContents>();
				component2.Index = num;
				this.TabManager.TabsContents.Add(component2);
				RectTransform component3 = component2.GetComponent<RectTransform>();
				component3.MMSetLeft(0f);
				component3.MMSetRight(0f);
				component3.MMSetTop(0f);
				component3.MMSetBottom(0f);
				if (num == this.Data.InitialActiveTabIndex)
				{
					mmdebugMenuDebugTab.gameObject.SetActive(true);
					this.TabManager.Tabs[num].Select();
				}
				else
				{
					mmdebugMenuDebugTab.gameObject.SetActive(false);
					this.TabManager.Tabs[num].Deselect();
				}
				num++;
			}
			int num2 = this.Data.MaxTabs - num;
			for (int i = 0; i < num2; i++)
			{
				Object.Instantiate<RectTransform>(this.Data.TabSpacerPrefab).transform.SetParent(this.TabBar);
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006208 File Offset: 0x00004408
		protected virtual void FillTab(MMDebugMenuTabContents tab, int index, bool triggerEvents = false)
		{
			Transform parent = tab.Parent;
			foreach (MMDebugMenuItem mmdebugMenuItem in this.Data.Tabs[index].MenuItems)
			{
				if (mmdebugMenuItem.Active)
				{
					switch (mmdebugMenuItem.Type)
					{
					case MMDebugMenuItem.MMDebugMenuItemTypes.Title:
					{
						MMDebugMenuItemTitle mmdebugMenuItemTitle = Object.Instantiate<MMDebugMenuItemTitle>(this.Data.TitlePrefab);
						mmdebugMenuItemTitle.name = "MMDebugMenuItemSlider_" + mmdebugMenuItem.Name;
						mmdebugMenuItemTitle.TitleText.text = mmdebugMenuItem.TitleText;
						mmdebugMenuItemTitle.TitleText.color = this.Data.TextColor;
						mmdebugMenuItemTitle.TitleText.font = this.Data.BoldFont;
						mmdebugMenuItemTitle.TitleLine.color = this.Data.AccentColor;
						mmdebugMenuItemTitle.transform.SetParent(parent);
						break;
					}
					case MMDebugMenuItem.MMDebugMenuItemTypes.Spacer:
					{
						GameObject gameObject = Object.Instantiate<GameObject>((mmdebugMenuItem.SpacerType == MMDebugMenuItem.MMDebugMenuItemSpacerTypes.Small) ? this.Data.SpacerSmallPrefab : this.Data.SpacerBigPrefab);
						gameObject.name = "MMDebugMenuItemSpacer_" + mmdebugMenuItem.Name;
						gameObject.transform.SetParent(parent);
						break;
					}
					case MMDebugMenuItem.MMDebugMenuItemTypes.Button:
					{
						MMDebugMenuItemButton mmdebugMenuItemButton = ((mmdebugMenuItem.ButtonType == MMDebugMenuItem.MMDebugMenuItemButtonTypes.Border) ? Object.Instantiate<MMDebugMenuItemButton>(this.Data.ButtonBorderPrefab) : Object.Instantiate<MMDebugMenuItemButton>(this.Data.ButtonPrefab));
						mmdebugMenuItemButton.name = "MMDebugMenuItemButton_" + mmdebugMenuItem.Name;
						mmdebugMenuItemButton.ButtonText.text = mmdebugMenuItem.ButtonText;
						mmdebugMenuItemButton.ButtonEventName = mmdebugMenuItem.ButtonEventName;
						if (mmdebugMenuItem.ButtonType == MMDebugMenuItem.MMDebugMenuItemButtonTypes.Border)
						{
							mmdebugMenuItemButton.ButtonText.color = this.Data.AccentColor;
							mmdebugMenuItemButton.ButtonBg.color = this.Data.TextColor;
						}
						else
						{
							mmdebugMenuItemButton.ButtonText.color = this.Data.BackgroundColor;
							mmdebugMenuItemButton.ButtonBg.color = this.Data.AccentColor;
						}
						mmdebugMenuItemButton.ButtonText.font = this.Data.RegularFont;
						mmdebugMenuItemButton.transform.SetParent(parent);
						break;
					}
					case MMDebugMenuItem.MMDebugMenuItemTypes.Checkbox:
					{
						MMDebugMenuItemCheckbox mmdebugMenuItemCheckbox = Object.Instantiate<MMDebugMenuItemCheckbox>(this.Data.CheckboxPrefab);
						mmdebugMenuItemCheckbox.name = "MMDebugMenuItemCheckbox_" + mmdebugMenuItem.Name;
						mmdebugMenuItemCheckbox.SwitchText.text = mmdebugMenuItem.CheckboxText;
						if (mmdebugMenuItem.CheckboxInitialState)
						{
							mmdebugMenuItemCheckbox.Switch.SetTrue();
						}
						else
						{
							mmdebugMenuItemCheckbox.Switch.SetFalse();
						}
						mmdebugMenuItemCheckbox.CheckboxEventName = mmdebugMenuItem.CheckboxEventName;
						mmdebugMenuItemCheckbox.transform.SetParent(parent);
						mmdebugMenuItemCheckbox.Switch.GetComponent<Image>().color = this.Data.AccentColor;
						mmdebugMenuItemCheckbox.SwitchText.color = this.Data.TextColor;
						mmdebugMenuItemCheckbox.SwitchText.font = this.Data.RegularFont;
						break;
					}
					case MMDebugMenuItem.MMDebugMenuItemTypes.Slider:
					{
						MMDebugMenuItemSlider mmdebugMenuItemSlider = Object.Instantiate<MMDebugMenuItemSlider>(this.Data.SliderPrefab);
						mmdebugMenuItemSlider.name = "MMDebugMenuItemSlider_" + mmdebugMenuItem.Name;
						mmdebugMenuItemSlider.Mode = mmdebugMenuItem.SliderMode;
						mmdebugMenuItemSlider.RemapZero = mmdebugMenuItem.SliderRemapZero;
						mmdebugMenuItemSlider.RemapOne = mmdebugMenuItem.SliderRemapOne;
						mmdebugMenuItemSlider.TargetSlider.value = MMMaths.Remap(mmdebugMenuItem.SliderInitialValue, mmdebugMenuItem.SliderRemapZero, mmdebugMenuItem.SliderRemapOne, 0f, 1f);
						mmdebugMenuItemSlider.transform.SetParent(parent);
						mmdebugMenuItemSlider.SliderText.text = mmdebugMenuItem.SliderText;
						mmdebugMenuItemSlider.SliderText.color = this.Data.TextColor;
						mmdebugMenuItemSlider.SliderText.font = this.Data.RegularFont;
						mmdebugMenuItemSlider.SliderValueText.text = ((mmdebugMenuItem.SliderMode == MMDebugMenuItemSlider.Modes.Int) ? mmdebugMenuItem.SliderInitialValue.ToString() : mmdebugMenuItem.SliderInitialValue.ToString("F3"));
						mmdebugMenuItemSlider.SliderValueText.color = this.Data.AccentColor;
						mmdebugMenuItemSlider.SliderValueText.font = this.Data.BoldFont;
						mmdebugMenuItemSlider.SliderKnob.color = this.Data.AccentColor;
						mmdebugMenuItemSlider.SliderLine.color = this.Data.TextColor;
						mmdebugMenuItemSlider.SliderEventName = mmdebugMenuItem.SliderEventName;
						break;
					}
					case MMDebugMenuItem.MMDebugMenuItemTypes.Text:
					{
						MMDebugMenuItemText mmdebugMenuItemText;
						switch (mmdebugMenuItem.TextType)
						{
						case MMDebugMenuItem.MMDebugMenuItemTextTypes.Tiny:
							mmdebugMenuItemText = this.Data.TextTinyPrefab;
							break;
						case MMDebugMenuItem.MMDebugMenuItemTextTypes.Small:
							mmdebugMenuItemText = this.Data.TextSmallPrefab;
							break;
						case MMDebugMenuItem.MMDebugMenuItemTextTypes.Long:
							mmdebugMenuItemText = this.Data.TextLongPrefab;
							break;
						default:
							mmdebugMenuItemText = this.Data.TextTinyPrefab;
							break;
						}
						MMDebugMenuItemText mmdebugMenuItemText2 = Object.Instantiate<MMDebugMenuItemText>(mmdebugMenuItemText);
						mmdebugMenuItemText2.name = "MMDebugMenuItemText_" + mmdebugMenuItem.Name;
						mmdebugMenuItemText2.ContentText.text = mmdebugMenuItem.TextContents;
						mmdebugMenuItemText2.ContentText.color = this.Data.TextColor;
						mmdebugMenuItemText2.ContentText.font = this.Data.RegularFont;
						mmdebugMenuItemText2.transform.SetParent(parent);
						break;
					}
					case MMDebugMenuItem.MMDebugMenuItemTypes.Value:
					{
						MMDebugMenuItemValue mmdebugMenuItemValue = Object.Instantiate<MMDebugMenuItemValue>(this.Data.ValuePrefab);
						mmdebugMenuItemValue.name = "MMDebugMenuItemValue_" + mmdebugMenuItem.Name;
						mmdebugMenuItemValue.LabelText.text = mmdebugMenuItem.ValueLabel;
						mmdebugMenuItemValue.LabelText.color = this.Data.TextColor;
						mmdebugMenuItemValue.LabelText.font = this.Data.RegularFont;
						mmdebugMenuItemValue.ValueText.text = mmdebugMenuItem.ValueInitialValue;
						mmdebugMenuItemValue.ValueText.color = this.Data.AccentColor;
						mmdebugMenuItemValue.ValueText.font = this.Data.BoldFont;
						mmdebugMenuItemValue.RadioReceiver.Channel = mmdebugMenuItem.ValueMMRadioReceiverChannel;
						mmdebugMenuItemValue.transform.SetParent(parent);
						break;
					}
					case MMDebugMenuItem.MMDebugMenuItemTypes.Choices:
					{
						MMDebugMenuItemChoices mmdebugMenuItemChoices;
						if (mmdebugMenuItem.ChoicesType == MMDebugMenuItem.MMDebugMenuItemChoicesTypes.TwoChoices)
						{
							mmdebugMenuItemChoices = this.Data.TwoChoicesPrefab;
						}
						else
						{
							mmdebugMenuItemChoices = this.Data.ThreeChoicesPrefab;
						}
						MMDebugMenuItemChoices mmdebugMenuItemChoices2 = Object.Instantiate<MMDebugMenuItemChoices>(mmdebugMenuItemChoices);
						mmdebugMenuItemChoices2.name = "MMDebugMenuItemChoices_" + mmdebugMenuItem.Name;
						mmdebugMenuItemChoices2.Choices[0].ButtonText.text = mmdebugMenuItem.ChoiceOneText;
						mmdebugMenuItemChoices2.Choices[1].ButtonText.text = mmdebugMenuItem.ChoiceTwoText;
						mmdebugMenuItemChoices2.Choices[0].ButtonEventName = mmdebugMenuItem.ChoiceOneEventName;
						mmdebugMenuItemChoices2.Choices[1].ButtonEventName = mmdebugMenuItem.ChoiceTwoEventName;
						if (mmdebugMenuItem.ChoicesType == MMDebugMenuItem.MMDebugMenuItemChoicesTypes.ThreeChoices)
						{
							mmdebugMenuItemChoices2.Choices[2].ButtonEventName = mmdebugMenuItem.ChoiceThreeEventName;
							mmdebugMenuItemChoices2.Choices[2].ButtonText.text = mmdebugMenuItem.ChoiceThreeText;
						}
						mmdebugMenuItemChoices2.OffColor = this.Data.BackgroundColor;
						mmdebugMenuItemChoices2.OnColor = this.Data.TextColor;
						mmdebugMenuItemChoices2.AccentColor = this.Data.AccentColor;
						foreach (MMDebugMenuChoiceEntry mmdebugMenuChoiceEntry in mmdebugMenuItemChoices2.Choices)
						{
							if (mmdebugMenuChoiceEntry != null)
							{
								mmdebugMenuChoiceEntry.ButtonText.font = this.Data.RegularFont;
							}
						}
						mmdebugMenuItemChoices2.Select(mmdebugMenuItem.SelectedChoice);
						if (triggerEvents)
						{
							mmdebugMenuItemChoices2.TriggerButtonEvent(mmdebugMenuItem.SelectedChoice);
						}
						mmdebugMenuItemChoices2.transform.SetParent(parent);
						break;
					}
					}
				}
			}
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.Data.SpacerBigPrefab);
			gameObject2.name = "MMDebugMenuItemSpacer_FinalSpacer";
			gameObject2.transform.SetParent(parent);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000069E8 File Offset: 0x00004BE8
		public virtual void OpenMenu()
		{
			UnityEvent onOpenEvent = this.OnOpenEvent;
			if (onOpenEvent != null)
			{
				onOpenEvent.Invoke();
			}
			base.StartCoroutine(this.ToggleCo(false));
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006A09 File Offset: 0x00004C09
		public virtual void CloseMenu()
		{
			base.StartCoroutine(this.ToggleCo(true));
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006A19 File Offset: 0x00004C19
		public virtual void ToggleMenu()
		{
			base.StartCoroutine(this.ToggleCo(this.Active));
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006A2E File Offset: 0x00004C2E
		protected virtual IEnumerator ToggleCo(bool active)
		{
			if (this._toggling)
			{
				yield break;
			}
			if (!active)
			{
				UnityEvent onOpenEvent = this.OnOpenEvent;
				if (onOpenEvent != null)
				{
					onOpenEvent.Invoke();
				}
				this._containerRect.gameObject.SetActive(true);
			}
			this._toggling = true;
			this.Active = active;
			this._newPosition = (active ? this._offPosition : this._initialContainerPosition);
			MMTween.MoveRectTransform(this, this._containerRect, this._containerRect.localPosition, this._newPosition, null, 0f, this.Data.ToggleDuration, this.Data.ToggleCurve, true);
			yield return MMCoroutine.WaitForUnscaled(this.Data.ToggleDuration);
			if (active)
			{
				UnityEvent onCloseEvent = this.OnCloseEvent;
				if (onCloseEvent != null)
				{
					onCloseEvent.Invoke();
				}
				this._containerRect.gameObject.SetActive(false);
			}
			this.Active = !active;
			this._toggling = false;
			yield break;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006A44 File Offset: 0x00004C44
		protected virtual void Update()
		{
			this.HandleInput();
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006A4C File Offset: 0x00004C4C
		protected virtual void HandleInput()
		{
			if (Input.GetKeyDown(this.Data.ToggleShortcut))
			{
				this.ToggleMenu();
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006A66 File Offset: 0x00004C66
		protected virtual void CaptureConsoleLog(string logString, string stackTrace, LogType type)
		{
			MMDebug.LogDebugToConsole(logString + " (" + type.ToString() + ")", "#00FFFF", 3, false);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006A92 File Offset: 0x00004C92
		protected virtual void OnEnable()
		{
			Application.logMessageReceived += this.CaptureConsoleLog;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006AA6 File Offset: 0x00004CA6
		protected virtual void OnDisable()
		{
			Application.logMessageReceived -= this.CaptureConsoleLog;
		}

		// Token: 0x04000141 RID: 321
		[Header("Data")]
		public MMDebugMenuData Data;

		// Token: 0x04000142 RID: 322
		[Header("Bindings")]
		public CanvasGroup MenuContainer;

		// Token: 0x04000143 RID: 323
		public RectTransform Contents;

		// Token: 0x04000144 RID: 324
		public Image MenuBackground;

		// Token: 0x04000145 RID: 325
		public Image CloseIcon;

		// Token: 0x04000146 RID: 326
		public RectTransform TabBar;

		// Token: 0x04000147 RID: 327
		public RectTransform TabContainer;

		// Token: 0x04000148 RID: 328
		public MMDebugMenuTabManager TabManager;

		// Token: 0x04000149 RID: 329
		public Image MMLogo;

		// Token: 0x0400014A RID: 330
		[Header("Events")]
		public UnityEvent OnOpenEvent;

		// Token: 0x0400014B RID: 331
		public UnityEvent OnCloseEvent;

		// Token: 0x0400014C RID: 332
		[Header("Test")]
		[MMReadOnly]
		public bool Active;

		// Token: 0x0400014D RID: 333
		[MMInspectorButton("ToggleMenu")]
		public bool ToggleButton;

		// Token: 0x0400014E RID: 334
		protected RectTransform _containerRect;

		// Token: 0x0400014F RID: 335
		protected Vector3 _initialContainerPosition;

		// Token: 0x04000150 RID: 336
		protected Vector3 _offPosition;

		// Token: 0x04000151 RID: 337
		protected Vector3 _newPosition;

		// Token: 0x04000152 RID: 338
		protected bool _toggling;

		// Token: 0x020003B0 RID: 944
		public enum ToggleDirections
		{
			// Token: 0x04001D7F RID: 7551
			TopToBottom,
			// Token: 0x04001D80 RID: 7552
			LeftToRight,
			// Token: 0x04001D81 RID: 7553
			RightToLeft,
			// Token: 0x04001D82 RID: 7554
			BottomToTop
		}
	}
}
