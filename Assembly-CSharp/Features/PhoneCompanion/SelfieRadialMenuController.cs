using System;
using System.Collections.Generic;
using Features.RapidActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E70 RID: 3696
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-400)]
	public class SelfieRadialMenuController : MonoBehaviour
	{
		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x06006E58 RID: 28248 RVA: 0x002C652C File Offset: 0x002C472C
		public static bool AnyWheelOpen
		{
			get
			{
				return SelfieRadialMenuController._wheelOwner != null && SelfieRadialMenuController._wheelOwner.isActiveAndEnabled && SelfieRadialMenuController._wheelOwner.IsWheelOpen;
			}
		}

		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x06006E59 RID: 28249 RVA: 0x002C6553 File Offset: 0x002C4753
		private bool PoseSlotAvailable
		{
			get
			{
				return this._poseSlotEnabled && (this.picker == null || this.picker.BodyPosesAllowed);
			}
		}

		// Token: 0x06006E5A RID: 28250 RVA: 0x002C657A File Offset: 0x002C477A
		public void Initialize(SelfiePosePicker posePicker, Action<bool> onWheelVisibilityChanged = null, SmartphoneController owner = null)
		{
			this.picker = posePicker;
			this._owner = owner;
			this._onWheelVisibilityChanged = onWheelVisibilityChanged;
			this.RefreshBodyPoseAvailability();
		}

		// Token: 0x06006E5B RID: 28251 RVA: 0x002C6598 File Offset: 0x002C4798
		public void SetActive(bool active)
		{
			this._active = active;
			if (!active)
			{
				this.CloseWheel(false);
				this.ResetSlot(this.poseSlot);
				this.ResetSlot(this.faceSlot);
				this.poseSlot.currentMenuIndex = 0;
				this.faceSlot.currentMenuIndex = 0;
				this._poseSlotEnabled = true;
				this._persistentHudVisible = true;
				if (this.wheel != null && this.wheel.gameObject.activeSelf)
				{
					this.wheel.gameObject.SetActive(false);
				}
			}
			else
			{
				this.RefreshBodyPoseAvailability();
				this.PrepareWheel();
				this.RefreshSlotLabel(this.poseSlot);
				this.RefreshSlotLabel(this.faceSlot);
				SelfieRadialMenuController.SetHintActive(this._pagePrevHint, false);
				SelfieRadialMenuController.SetHintActive(this._pageNextHint, false);
			}
			this.ApplyPersistentHudVisibility();
		}

		// Token: 0x06006E5C RID: 28252 RVA: 0x002C666C File Offset: 0x002C486C
		public void SetPoseSlotEnabled(bool enabled)
		{
			this.RefreshBodyPoseAvailability();
			if (this._poseSlotEnabled == enabled)
			{
				return;
			}
			this._poseSlotEnabled = enabled;
			if (!enabled)
			{
				if (this._activeSlot == this.poseSlot)
				{
					this.CloseWheel(false);
				}
				else
				{
					this.ResetSlot(this.poseSlot);
				}
			}
			if (this.poseSlot.slotView != null)
			{
				this.poseSlot.slotView.gameObject.SetActive(this.PoseSlotAvailable && this._active && this._persistentHudVisible);
			}
			if (enabled)
			{
				this.RefreshSlotLabel(this.poseSlot);
			}
			this.ApplyPersistentHudVisibility();
		}

		// Token: 0x06006E5D RID: 28253 RVA: 0x002C670C File Offset: 0x002C490C
		public void SetPersistentHudVisible(bool visible)
		{
			if (this._persistentHudVisible == visible)
			{
				return;
			}
			this._persistentHudVisible = visible;
			this.ApplyPersistentHudVisibility();
		}

		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x06006E5E RID: 28254 RVA: 0x002C6725 File Offset: 0x002C4925
		public bool IsWheelOpen
		{
			get
			{
				return this._activeSlot != null && this._activeSlot.phase == SelfieRadialMenuController.Phase.WheelOpen;
			}
		}

		// Token: 0x06006E5F RID: 28255 RVA: 0x002C673F File Offset: 0x002C493F
		public void CancelTransientInput()
		{
			if (this._activeSlot == null)
			{
				return;
			}
			if (this._activeSlot.phase == SelfieRadialMenuController.Phase.WheelOpen)
			{
				this.CloseWheel(false);
				return;
			}
			this.ResetSlot(this._activeSlot);
		}

		// Token: 0x06006E60 RID: 28256 RVA: 0x002C676C File Offset: 0x002C496C
		private void ApplyPersistentHudVisibility()
		{
			if (this.poseSlot.slotView != null)
			{
				this.poseSlot.slotView.gameObject.SetActive(this._active && this.PoseSlotAvailable && this._persistentHudVisible);
			}
			if (this.faceSlot.slotView != null)
			{
				this.faceSlot.slotView.gameObject.SetActive(this._active && this._persistentHudVisible);
			}
		}

		// Token: 0x06006E61 RID: 28257 RVA: 0x002C67F4 File Offset: 0x002C49F4
		private void RefreshBodyPoseAvailability()
		{
			SelfiePosePicker selfiePosePicker = this.picker;
			if (selfiePosePicker != null)
			{
				selfiePosePicker.RefreshBodyPoseAvailability();
			}
			bool flag = this.picker == null || this.picker.BodyPosesAllowed;
			if (this._bodyPosesAllowed == flag)
			{
				return;
			}
			this._bodyPosesAllowed = flag;
			if (!flag)
			{
				if (this._activeSlot == this.poseSlot)
				{
					this.CloseWheel(false);
				}
				else
				{
					this.ResetSlot(this.poseSlot);
				}
				this.poseSlot.currentMenuIndex = 0;
			}
			this.RefreshSlotLabel(this.poseSlot);
			this.ApplyPersistentHudVisibility();
		}

		// Token: 0x06006E62 RID: 28258 RVA: 0x002C6884 File Offset: 0x002C4A84
		public void ResetPoseSelection()
		{
			this.poseSlot.currentMenuIndex = 0;
			if (this.picker != null)
			{
				this.picker.SelectPose(0);
			}
			this.RefreshSlotLabel(this.poseSlot);
		}

		// Token: 0x06006E63 RID: 28259 RVA: 0x002C68B8 File Offset: 0x002C4AB8
		private void Update()
		{
			if (!this._active)
			{
				return;
			}
			this.RefreshBodyPoseAvailability();
			if (GameplayInputGate.BlocksGameplayInput || GraphicsSettingsRuntimeUI.AnyOpen || Time.timeScale <= 0f)
			{
				this.CancelTransientInput();
				return;
			}
			if (!((this._owner != null) ? this._owner.SelfieActive : SmartphoneController.IsAnySelfieActive))
			{
				this.SetActive(false);
				return;
			}
			if (this._lastControllerActive != ControllerInput.IsActive || this._lastGlyphVersion != ControllerGlyphs.Version)
			{
				this._lastControllerActive = ControllerInput.IsActive;
				this._lastGlyphVersion = ControllerGlyphs.Version;
				this.RefreshSlotLabel(this.poseSlot);
				this.RefreshSlotLabel(this.faceSlot);
				this.RefreshPageIndicator();
			}
			if (this.PoseSlotAvailable)
			{
				this.TickSlot(this.poseSlot);
			}
			this.TickSlot(this.faceSlot);
		}

		// Token: 0x06006E64 RID: 28260 RVA: 0x002C698D File Offset: 0x002C4B8D
		private void OnDisable()
		{
			this.CancelTransientInput();
		}

		// Token: 0x06006E65 RID: 28261 RVA: 0x002C6998 File Offset: 0x002C4B98
		private void TickSlot(SelfieRadialMenuController.Slot slot)
		{
			if (slot.kind == SelfieRadialMenuController.SlotKind.Pose && !this.PoseSlotAvailable)
			{
				this.RefreshBodyPoseAvailability();
				if (this._activeSlot == slot)
				{
					this.CloseWheel(false);
					return;
				}
				this.ResetSlot(slot);
				return;
			}
			else
			{
				switch (slot.phase)
				{
				case SelfieRadialMenuController.Phase.Idle:
					this.TickIdle(slot);
					return;
				case SelfieRadialMenuController.Phase.Pending:
					this.TickPending(slot);
					return;
				case SelfieRadialMenuController.Phase.WheelOpen:
					this.TickWheelOpen(slot);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06006E66 RID: 28262 RVA: 0x002C6A05 File Offset: 0x002C4C05
		private void TickIdle(SelfieRadialMenuController.Slot slot)
		{
			if (this._activeSlot != null)
			{
				return;
			}
			if (!this.SlotKeyDown(slot))
			{
				return;
			}
			if (ControllerInput.IsActive)
			{
				ControllerInput.ConsumeUIInput();
			}
			slot.pressStart = Time.unscaledTime;
			slot.phase = SelfieRadialMenuController.Phase.Pending;
			this._activeSlot = slot;
		}

		// Token: 0x06006E67 RID: 28263 RVA: 0x002C6A3F File Offset: 0x002C4C3F
		private void TickPending(SelfieRadialMenuController.Slot slot)
		{
			if (!this.SlotKeyHeld(slot))
			{
				this.OpenWheel(slot);
				slot.clickToConfirm = slot.phase == SelfieRadialMenuController.Phase.WheelOpen;
				return;
			}
			if (Time.unscaledTime - slot.pressStart >= this.longPressThreshold)
			{
				this.OpenWheel(slot);
			}
		}

		// Token: 0x06006E68 RID: 28264 RVA: 0x002C6A7C File Offset: 0x002C4C7C
		private void TickWheelOpen(SelfieRadialMenuController.Slot slot)
		{
			RadialControllerNavigation.ShowControllerHint(this.wheel, this._paging ? "LB / RB" : null);
			if (this.wheel != null && this.wheel.IsEnabled && this.wheel.BaseTransform != null && !RadialControllerNavigation.Process(this.wheel, true))
			{
				RadialControllerNavigation.ProcessPointer(this.wheel, GameplayInput.mousePosition);
			}
			if (EscapeInputCoordinator.TryConsume(this) || GameplayInput.GetMouseButtonDown(1))
			{
				if (ControllerInput.IsActive)
				{
					ControllerInput.ConsumeUIInput();
				}
				this.CloseWheel(false);
				return;
			}
			if (this._paging)
			{
				if (Input.GetKeyDown(this.pagePrevKey) || ControllerInput.GetButtonDown(ControllerButton.LeftShoulder))
				{
					ControllerInput.ConsumeUIInput();
					this.ChangePage(-1, slot);
					return;
				}
				if (Input.GetKeyDown(this.pageNextKey) || ControllerInput.GetButtonDown(ControllerButton.RightShoulder))
				{
					ControllerInput.ConsumeUIInput();
					this.ChangePage(1, slot);
					return;
				}
			}
			if (ControllerInput.SubmitDown)
			{
				ControllerInput.ConsumeUIInput();
				this.CloseWheel(true);
				return;
			}
			if (slot.clickToConfirm)
			{
				if (GameplayInput.GetMouseButtonDown(0) || this.SlotKeyDown(slot) || GameplayInput.GetKeyDown(KeyCode.Return))
				{
					this.CloseWheel(true);
					return;
				}
			}
			else if (!this.SlotKeyHeld(slot))
			{
				this.CloseWheel(true);
			}
		}

		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x06006E69 RID: 28265 RVA: 0x002C6BB5 File Offset: 0x002C4DB5
		private bool UsesTripodBindings
		{
			get
			{
				if (!(this._owner != null))
				{
					return SmartphoneController.IsAnyTripodActive;
				}
				return this._owner.TripodActive;
			}
		}

		// Token: 0x06006E6A RID: 28266 RVA: 0x002C6BD6 File Offset: 0x002C4DD6
		private KeyCode SlotKey(SelfieRadialMenuController.Slot slot)
		{
			if (!this.UsesTripodBindings || slot.kind != SelfieRadialMenuController.SlotKind.Face)
			{
				return slot.hotkey;
			}
			return KeyCode.Alpha6;
		}

		// Token: 0x06006E6B RID: 28267 RVA: 0x002C6BF2 File Offset: 0x002C4DF2
		private ControllerButton SlotButton(SelfieRadialMenuController.Slot slot)
		{
			if (this.UsesTripodBindings && slot.kind == SelfieRadialMenuController.SlotKind.Face)
			{
				return ControllerButton.DpadDown;
			}
			if (slot.kind != SelfieRadialMenuController.SlotKind.Pose)
			{
				return ControllerButton.DpadRight;
			}
			return ControllerButton.DpadLeft;
		}

		// Token: 0x06006E6C RID: 28268 RVA: 0x002C6C15 File Offset: 0x002C4E15
		private bool SlotKeyDown(SelfieRadialMenuController.Slot slot)
		{
			return Input.GetKeyDown(this.SlotKey(slot)) || (!ControllerInput.GetButton(ControllerButton.LeftTrigger) && ControllerInput.GetButtonDown(this.SlotButton(slot)));
		}

		// Token: 0x06006E6D RID: 28269 RVA: 0x002C6C3D File Offset: 0x002C4E3D
		private bool SlotKeyHeld(SelfieRadialMenuController.Slot slot)
		{
			return Input.GetKey(this.SlotKey(slot)) || ControllerInput.GetButton(this.SlotButton(slot));
		}

		// Token: 0x06006E6E RID: 28270 RVA: 0x002C6C5C File Offset: 0x002C4E5C
		private void ChangePage(int dir, SelfieRadialMenuController.Slot slot)
		{
			if (this._pageCount <= 1)
			{
				return;
			}
			int num = this._currentPage + dir;
			if (this.pagesWrap)
			{
				num = (num % this._pageCount + this._pageCount) % this._pageCount;
			}
			else
			{
				num = Mathf.Clamp(num, 0, this._pageCount - 1);
			}
			if (num == this._currentPage)
			{
				return;
			}
			this._currentPage = num;
			this.PopulateWheel(slot.currentOptions, this._currentPage);
			this.RefreshPageIndicator();
		}

		// Token: 0x06006E6F RID: 28271 RVA: 0x002C6CD6 File Offset: 0x002C4ED6
		private void ResetSlot(SelfieRadialMenuController.Slot slot)
		{
			slot.phase = SelfieRadialMenuController.Phase.Idle;
			slot.clickToConfirm = false;
			slot.currentOptions = null;
			if (this._activeSlot == slot)
			{
				this._activeSlot = null;
			}
		}

		// Token: 0x06006E70 RID: 28272 RVA: 0x002C6D00 File Offset: 0x002C4F00
		private void OpenWheel(SelfieRadialMenuController.Slot slot)
		{
			this.RefreshBodyPoseAvailability();
			if (slot.kind == SelfieRadialMenuController.SlotKind.Pose && !this.PoseSlotAvailable)
			{
				this.ResetSlot(slot);
				return;
			}
			if (this.wheel == null || this.picker == null)
			{
				this.ResetSlot(slot);
				return;
			}
			this.PrepareWheel();
			List<string> list = this.BuildOptions(slot);
			if (list.Count == 0)
			{
				this.ResetSlot(slot);
				return;
			}
			slot.currentOptions = list;
			this._pageSize = this.wheel.UltimateRadialButtonList.Count;
			this._paging = this._pageSize > 0 && list.Count > this._pageSize;
			this._pageCount = (this._paging ? Mathf.CeilToInt((float)list.Count / (float)this._pageSize) : 1);
			this._currentPage = 0;
			this.PopulateWheel(list, this._currentPage);
			if (!this.wheel.gameObject.activeSelf)
			{
				this.wheel.gameObject.SetActive(true);
			}
			this.wheel.Enable();
			slot.phase = SelfieRadialMenuController.Phase.WheelOpen;
			SelfieRadialMenuController._wheelOwner = this;
			this.RefreshPageIndicator();
			Action<bool> onWheelVisibilityChanged = this._onWheelVisibilityChanged;
			if (onWheelVisibilityChanged == null)
			{
				return;
			}
			onWheelVisibilityChanged(true);
		}

		// Token: 0x06006E71 RID: 28273 RVA: 0x002C6E38 File Offset: 0x002C5038
		private void PrepareWheel()
		{
			if (this.wheel == null)
			{
				return;
			}
			RadialControllerNavigation.TakeManualOwnership(this.wheel);
			if (!this.wheel.gameObject.activeSelf)
			{
				this.wheel.gameObject.SetActive(true);
				this.wheel.DisableImmediate();
			}
		}

		// Token: 0x06006E72 RID: 28274 RVA: 0x002C6E90 File Offset: 0x002C5090
		private void RefreshPageIndicator()
		{
			bool flag = this.showPageKeyHints && this._paging && this._pageCount > 1;
			if (flag)
			{
				this.EnsurePageKeyHints();
			}
			SelfieRadialMenuController.SetHintActive(this._pagePrevHint, flag);
			SelfieRadialMenuController.SetHintActive(this._pageNextHint, flag);
		}

		// Token: 0x06006E73 RID: 28275 RVA: 0x002C6EDB File Offset: 0x002C50DB
		private static void SetHintActive(Text hint, bool active)
		{
			if (hint != null && hint.gameObject.activeSelf != active)
			{
				hint.gameObject.SetActive(active);
			}
		}

		// Token: 0x06006E74 RID: 28276 RVA: 0x002C6F00 File Offset: 0x002C5100
		private void EnsurePageKeyHints()
		{
			if (this.wheel == null)
			{
				return;
			}
			if (this._pagePrevHint == null)
			{
				this._pagePrevHint = this.BuildKeyHint("PagePrevHint", this.pagePrevKey, -1f);
			}
			if (this._pageNextHint == null)
			{
				this._pageNextHint = this.BuildKeyHint("PageNextHint", this.pageNextKey, 1f);
			}
			this.ApplyKeyHint(this._pagePrevHint, this.pagePrevKey, -1f);
			this.ApplyKeyHint(this._pageNextHint, this.pageNextKey, 1f);
		}

		// Token: 0x06006E75 RID: 28277 RVA: 0x002C6FA0 File Offset: 0x002C51A0
		private Text BuildKeyHint(string name, KeyCode key, float side)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Text)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(this.wheel.transform, false);
			float num = ((side < 0f) ? 0f : 1f);
			Vector2 vector = new Vector2(num, 0.5f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2((side < 0f) ? 1f : 0f, 0.5f);
			rectTransform.sizeDelta = new Vector2(64f, 64f);
			Text component = gameObject.GetComponent<Text>();
			component.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component.alignment = TextAnchor.MiddleCenter;
			component.fontStyle = FontStyle.Bold;
			component.raycastTarget = false;
			this.ApplyKeyHint(component, key, side);
			return component;
		}

		// Token: 0x06006E76 RID: 28278 RVA: 0x002C7090 File Offset: 0x002C5290
		private void ApplyKeyHint(Text txt, KeyCode key, float side)
		{
			if (txt == null)
			{
				return;
			}
			txt.text = (ControllerInput.IsActive ? ControllerGlyphs.Plain((side < 0f) ? "LB" : "RB", ControllerGlyphs.Family) : SelfieRadialMenuController.KeyCapLabel(key));
			txt.fontSize = this.pageKeyHintFontSize;
			txt.color = new Color(1f, 1f, 1f, this.pageKeyHintAlpha);
			((RectTransform)txt.transform).anchoredPosition = new Vector2(side * this.pageKeyHintOffset, 0f);
		}

		// Token: 0x06006E77 RID: 28279 RVA: 0x002C7128 File Offset: 0x002C5328
		private static string KeyCapLabel(KeyCode key)
		{
			string text = key.ToString();
			if (text.StartsWith("Alpha") && text.Length == 6)
			{
				return text.Substring(5);
			}
			if (text.StartsWith("Keypad") && text.Length == 7)
			{
				return text.Substring(6);
			}
			return text;
		}

		// Token: 0x06006E78 RID: 28280 RVA: 0x002C7180 File Offset: 0x002C5380
		private void CloseWheel(bool commit)
		{
			SelfieRadialMenuController.Slot activeSlot = this._activeSlot;
			bool flag = activeSlot != null && activeSlot.phase == SelfieRadialMenuController.Phase.WheelOpen;
			if (activeSlot != null && commit && activeSlot.currentOptions != null && this.wheel != null)
			{
				int currentButtonIndex = this.wheel.CurrentButtonIndex;
				if (currentButtonIndex >= 0)
				{
					int num = this._currentPage * this._pageSize + currentButtonIndex;
					if (num >= 0 && num < activeSlot.currentOptions.Count)
					{
						this.Commit(activeSlot, num);
					}
				}
			}
			if (this.wheel != null && this.wheel.IsEnabled)
			{
				this.wheel.DisableImmediate();
			}
			if (activeSlot != null)
			{
				this.ResetSlot(activeSlot);
			}
			if (SelfieRadialMenuController._wheelOwner == this)
			{
				SelfieRadialMenuController._wheelOwner = null;
			}
			this._paging = false;
			this._pageCount = 1;
			this._currentPage = 0;
			SelfieRadialMenuController.SetHintActive(this._pagePrevHint, false);
			SelfieRadialMenuController.SetHintActive(this._pageNextHint, false);
			if (flag)
			{
				Action<bool> onWheelVisibilityChanged = this._onWheelVisibilityChanged;
				if (onWheelVisibilityChanged == null)
				{
					return;
				}
				onWheelVisibilityChanged(false);
			}
		}

		// Token: 0x06006E79 RID: 28281 RVA: 0x002C7280 File Offset: 0x002C5480
		private void Commit(SelfieRadialMenuController.Slot slot, int menuIndex)
		{
			if (slot.kind == SelfieRadialMenuController.SlotKind.Pose && !this.PoseSlotAvailable)
			{
				return;
			}
			if (slot.optionMenuIndices == null || menuIndex < 0 || menuIndex >= slot.optionMenuIndices.Count)
			{
				return;
			}
			menuIndex = slot.optionMenuIndices[menuIndex];
			if (slot.kind == SelfieRadialMenuController.SlotKind.Pose && !this.picker.TrySelectPose(menuIndex))
			{
				return;
			}
			slot.currentMenuIndex = menuIndex;
			if (slot.kind == SelfieRadialMenuController.SlotKind.Face)
			{
				this.picker.SelectFace(menuIndex);
			}
			this.RefreshSlotLabel(slot);
		}

		// Token: 0x06006E7A RID: 28282 RVA: 0x002C7304 File Offset: 0x002C5504
		private void PopulateWheel(List<string> options, int page)
		{
			this.wheel.ClearButtonInformations();
			RadialMenuTextLocalization radialMenuTextLocalization = RadialMenuTextLocalization.Begin(this.wheel);
			int count = this.wheel.UltimateRadialButtonList.Count;
			if (count <= 0)
			{
				return;
			}
			int num = page * count;
			int num2 = Mathf.Min(count, options.Count - num);
			for (int i = 0; i < num2; i++)
			{
				int num3 = num + i;
				UltimateRadialButtonInfo ultimateRadialButtonInfo = new UltimateRadialButtonInfo
				{
					name = options[num3],
					icon = this.defaultSliceSprite,
					key = num3.ToString()
				};
				this.wheel.RegisterButton(new Action(SelfieRadialMenuController.NoOp), ultimateRadialButtonInfo, i);
				radialMenuTextLocalization.BindButton(i, "gameplay_prompts", options[num3], null);
			}
		}

		// Token: 0x06006E7B RID: 28283 RVA: 0x00004F31 File Offset: 0x00003131
		private static void NoOp()
		{
		}

		// Token: 0x06006E7C RID: 28284 RVA: 0x002C73C8 File Offset: 0x002C55C8
		private List<string> BuildOptions(SelfieRadialMenuController.Slot slot)
		{
			List<string> list = new List<string> { "Default" };
			slot.optionMenuIndices = new List<int> { 0 };
			SelfiePoseConfigSO selfiePoseConfigSO = ((this.picker != null) ? this.picker.Config : null);
			if (selfiePoseConfigSO == null)
			{
				return list;
			}
			if (slot.kind == SelfieRadialMenuController.SlotKind.Pose && selfiePoseConfigSO.poses != null)
			{
				using (List<int>.Enumerator enumerator = this.picker.GetAvailablePoseMenuIndices().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int num = enumerator.Current;
						if (num != 0)
						{
							list.Add(SelfiePoseConfigSO.PoseLabel(selfiePoseConfigSO.poses[num - 1]));
							slot.optionMenuIndices.Add(num);
						}
					}
					return list;
				}
			}
			if (slot.kind == SelfieRadialMenuController.SlotKind.Face && selfiePoseConfigSO.faces != null)
			{
				for (int i = 0; i < selfiePoseConfigSO.faces.Count; i++)
				{
					list.Add(SelfiePoseConfigSO.FaceLabel(selfiePoseConfigSO.faces[i]));
					slot.optionMenuIndices.Add(i + 1);
				}
			}
			return list;
		}

		// Token: 0x06006E7D RID: 28285 RVA: 0x002C74F0 File Offset: 0x002C56F0
		private void RefreshSlotLabel(SelfieRadialMenuController.Slot slot)
		{
			if (slot.slotView == null)
			{
				return;
			}
			string text = "Default";
			SelfiePoseConfigSO selfiePoseConfigSO = ((this.picker != null) ? this.picker.Config : null);
			int num = slot.currentMenuIndex - 1;
			if (slot.kind == SelfieRadialMenuController.SlotKind.Pose && num >= 0 && this.picker != null && this.picker.IsPoseAvailable(slot.currentMenuIndex))
			{
				text = SelfiePoseConfigSO.PoseLabel(selfiePoseConfigSO.poses[num]);
			}
			else if (slot.kind == SelfieRadialMenuController.SlotKind.Face && selfiePoseConfigSO != null && selfiePoseConfigSO.faces != null && num >= 0 && num < selfiePoseConfigSO.faces.Count)
			{
				text = SelfiePoseConfigSO.FaceLabel(selfiePoseConfigSO.faces[num]);
			}
			string text2 = ((!string.IsNullOrEmpty(slot.keyCapOverride)) ? slot.keyCapOverride : slot.hotkey.ToString());
			if (ControllerInput.IsActive)
			{
				text2 = ((slot.kind == SelfieRadialMenuController.SlotKind.Pose) ? "D-pad Left" : "D-pad Right");
			}
			if (this.UsesTripodBindings && slot.kind == SelfieRadialMenuController.SlotKind.Face)
			{
				text2 = (ControllerInput.IsActive ? "D-pad Down" : "6");
			}
			slot.slotView.Set(slot.categoryLabel, text, text2, null);
		}

		// Token: 0x04007106 RID: 28934
		[Header("Slots")]
		public SelfieRadialMenuController.Slot poseSlot = new SelfieRadialMenuController.Slot
		{
			kind = SelfieRadialMenuController.SlotKind.Pose,
			categoryLabel = "Pose:",
			keyCapOverride = "1",
			hotkey = KeyCode.Alpha1
		};

		// Token: 0x04007107 RID: 28935
		public SelfieRadialMenuController.Slot faceSlot = new SelfieRadialMenuController.Slot
		{
			kind = SelfieRadialMenuController.SlotKind.Face,
			categoryLabel = "Face:",
			keyCapOverride = "2",
			hotkey = KeyCode.Alpha2
		};

		// Token: 0x04007108 RID: 28936
		[Header("Wheel")]
		[Tooltip("UltimateRadialMenu condivisa per pose/facce (una sola: la apriamo con le opzioni dello slot attivo).")]
		public UltimateRadialMenu wheel;

		// Token: 0x04007109 RID: 28937
		[Tooltip("Hold duration before opening the wheel. A shorter tap opens it for click confirmation.")]
		public float longPressThreshold = 0.25f;

		// Token: 0x0400710A RID: 28938
		[Tooltip("Sprite di default per le fette della wheel (se un'opzione non ha icona).")]
		public Sprite defaultSliceSprite;

		// Token: 0x0400710B RID: 28939
		[Header("Paginazione (snella)")]
		[Tooltip("Tasto per la pagina PRECEDENTE mentre la wheel e' aperta (solo se le opzioni superano la capacita' della wheel). Default Z.")]
		public KeyCode pagePrevKey = KeyCode.Z;

		// Token: 0x0400710C RID: 28940
		[Tooltip("Tasto per la pagina SUCCESSIVA mentre la wheel e' aperta. Default X.")]
		public KeyCode pageNextKey = KeyCode.X;

		// Token: 0x0400710D RID: 28941
		[Tooltip("Le pagine ciclano (dall'ultima si torna alla prima e viceversa)? Se false, si fermano agli estremi.")]
		public bool pagesWrap = true;

		// Token: 0x0400710E RID: 28942
		[Tooltip("Mostra ai lati della wheel la LETTERA del tasto per cambiare pagina (Z a sinistra, X a destra), SOLO quando c'e' una pagina da cambiare (>1 pagina). Le label sono create da codice come figli della wheel: nessun setup in scena. Spegni se non le vuoi.")]
		public bool showPageKeyHints = true;

		// Token: 0x0400710F RID: 28943
		[Tooltip("Margine (px) di quanto le lettere Z/X stanno OLTRE il bordo della wheel (sono ancorate ai lati della ruota, non al centro). Aumenta per allontanarle di piu' dalla ruota.")]
		public float pageKeyHintOffset = 40f;

		// Token: 0x04007110 RID: 28944
		[Tooltip("Opacita' (0..1) delle lettere Z/X: bassa per un effetto 'lieve'.")]
		[Range(0f, 1f)]
		public float pageKeyHintAlpha = 0.5f;

		// Token: 0x04007111 RID: 28945
		[Tooltip("Dimensione font delle lettere Z/X.")]
		public int pageKeyHintFontSize = 34;

		// Token: 0x04007112 RID: 28946
		[Header("Refs")]
		[Tooltip("Il SelfiePosePicker che applica davvero pose/facce. Auto-risolto dal SmartphoneController.")]
		public SelfiePosePicker picker;

		// Token: 0x04007113 RID: 28947
		private const string NoneLabel = "Default";

		// Token: 0x04007114 RID: 28948
		private SelfieRadialMenuController.Slot _activeSlot;

		// Token: 0x04007115 RID: 28949
		private static SelfieRadialMenuController _wheelOwner;

		// Token: 0x04007116 RID: 28950
		private bool _lastControllerActive;

		// Token: 0x04007117 RID: 28951
		private int _lastGlyphVersion = -1;

		// Token: 0x04007118 RID: 28952
		private bool _active;

		// Token: 0x04007119 RID: 28953
		private SmartphoneController _owner;

		// Token: 0x0400711A RID: 28954
		private bool _poseSlotEnabled = true;

		// Token: 0x0400711B RID: 28955
		private bool _bodyPosesAllowed = true;

		// Token: 0x0400711C RID: 28956
		private bool _persistentHudVisible = true;

		// Token: 0x0400711D RID: 28957
		private bool _paging;

		// Token: 0x0400711E RID: 28958
		private int _pageSize;

		// Token: 0x0400711F RID: 28959
		private int _pageCount;

		// Token: 0x04007120 RID: 28960
		private int _currentPage;

		// Token: 0x04007121 RID: 28961
		private Text _pagePrevHint;

		// Token: 0x04007122 RID: 28962
		private Text _pageNextHint;

		// Token: 0x04007123 RID: 28963
		private Action<bool> _onWheelVisibilityChanged;

		// Token: 0x02000E71 RID: 3697
		public enum SlotKind
		{
			// Token: 0x04007125 RID: 28965
			Pose,
			// Token: 0x04007126 RID: 28966
			Face
		}

		// Token: 0x02000E72 RID: 3698
		public enum Phase
		{
			// Token: 0x04007128 RID: 28968
			Idle,
			// Token: 0x04007129 RID: 28969
			Pending,
			// Token: 0x0400712A RID: 28970
			WheelOpen
		}

		// Token: 0x02000E73 RID: 3699
		[Serializable]
		public class Slot
		{
			// Token: 0x0400712B RID: 28971
			[Tooltip("Pose o Face.")]
			public SelfieRadialMenuController.SlotKind kind;

			// Token: 0x0400712C RID: 28972
			[Tooltip("Tap: open and click to select. Hold: open and release to select. Default Alpha1 (Pose) / Alpha2 (Face). Si usa KeyCode diretto (non GenericInput) per non dipendere da assi nell'InputManager.")]
			public KeyCode hotkey = KeyCode.Alpha1;

			// Token: 0x0400712D RID: 28973
			[Tooltip("View dello slot (categoria + nome selezione + keycap). Riusa il prefab RapidActionSlotUI.")]
			public RapidActionSlotUI slotView;

			// Token: 0x0400712E RID: 28974
			[Tooltip("Prefisso categoria mostrato nello slot (es. 'Pose:' / 'Face:').")]
			public string categoryLabel = "Pose:";

			// Token: 0x0400712F RID: 28975
			[Tooltip("Testo keycap (es. '1', '2'). Se vuoto, deriva dal binding tastiera della hotkey.")]
			public string keyCapOverride = "";

			// Token: 0x04007130 RID: 28976
			[NonSerialized]
			public SelfieRadialMenuController.Phase phase;

			// Token: 0x04007131 RID: 28977
			[NonSerialized]
			public float pressStart;

			// Token: 0x04007132 RID: 28978
			[NonSerialized]
			public int currentMenuIndex;

			// Token: 0x04007133 RID: 28979
			[NonSerialized]
			public List<string> currentOptions;

			// Token: 0x04007134 RID: 28980
			[NonSerialized]
			public List<int> optionMenuIndices;

			// Token: 0x04007135 RID: 28981
			[NonSerialized]
			public bool clickToConfirm;
		}
	}
}
