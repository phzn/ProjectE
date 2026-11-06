using System;
using System.Collections;
using System.Collections.Generic;
using Features.CensorBars;
using Features.CielOS;
using Features.Inventory.Wearable;
using Features.Localization;
using Features.Rank;
using Features.RapidActions;
using Features.RunModes;
using Invector.vCharacterController;
using Invector.vCover;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Headphones;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Inventory.UI
{
	// Token: 0x02001156 RID: 4438
	[DefaultExecutionOrder(-450)]
	public class InventoryDialogController : MonoBehaviour
	{
		// Token: 0x1700155F RID: 5471
		// (get) Token: 0x060086F9 RID: 34553 RVA: 0x0037307B File Offset: 0x0037127B
		// (set) Token: 0x060086FA RID: 34554 RVA: 0x00373082 File Offset: 0x00371282
		public static InventoryDialogController Instance { get; private set; }

		// Token: 0x17001560 RID: 5472
		// (get) Token: 0x060086FB RID: 34555 RVA: 0x0037308A File Offset: 0x0037128A
		// (set) Token: 0x060086FC RID: 34556 RVA: 0x00373092 File Offset: 0x00371292
		public bool IsOpen { get; private set; }

		// Token: 0x060086FD RID: 34557 RVA: 0x0037309B File Offset: 0x0037129B
		private void Awake()
		{
			if (InventoryDialogController.Instance == null)
			{
				InventoryDialogController.Instance = this;
				if (this.canvasGroup == null)
				{
					this.canvasGroup = base.GetComponent<CanvasGroup>();
				}
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x060086FE RID: 34558 RVA: 0x003730D8 File Offset: 0x003712D8
		private void Start()
		{
			this.ResolveRefs();
			this.CacheSlidePositions();
			this.InitSlots();
			this.RefreshAll();
			this.ApplyOpenState(false, true);
		}

		// Token: 0x060086FF RID: 34559 RVA: 0x003730FC File Offset: 0x003712FC
		private void ResolveRefs()
		{
			if (this.inventory == null || (PlayerInventoryManager.Instance != null && this.inventory != PlayerInventoryManager.Instance))
			{
				this.inventory = PlayerInventoryManager.Instance;
			}
			if (this.inventory != null)
			{
				this.wearController = this.inventory.GetComponent<WearableUseController>() ?? this.inventory.GetComponentInChildren<WearableUseController>(true);
			}
			if (this.dropController == null)
			{
				this.dropController = InventoryDropController.Instance ?? Object.FindObjectOfType<InventoryDropController>(true);
			}
			if (this.rootCanvas == null)
			{
				this.rootCanvas = base.GetComponentInParent<Canvas>();
			}
		}

		// Token: 0x06008700 RID: 34560 RVA: 0x003731AD File Offset: 0x003713AD
		private void OnEnable()
		{
			this.BindInventory();
		}

		// Token: 0x06008701 RID: 34561 RVA: 0x003731B5 File Offset: 0x003713B5
		private void OnDisable()
		{
			this.UnbindInventory();
			this.CancelDragState();
			this.CloseInternal(true, false);
		}

		// Token: 0x06008702 RID: 34562 RVA: 0x003731CB File Offset: 0x003713CB
		private void OnDestroy()
		{
			this.CloseInternal(true, false);
			if (InventoryDialogController.Instance == this)
			{
				InventoryDialogController.Instance = null;
			}
		}

		// Token: 0x06008703 RID: 34563 RVA: 0x003731E8 File Offset: 0x003713E8
		private void BindInventory()
		{
			if (this._bound)
			{
				return;
			}
			if (this.inventory == null)
			{
				this.inventory = PlayerInventoryManager.Instance;
			}
			if (this.inventory == null)
			{
				return;
			}
			this.inventory.OnItemAdded += this.HandleItemChanged;
			this.inventory.OnItemVisualChanged += this.HandleItemChanged;
			this.inventory.OnItemRemoved += this.HandleItemRemoved;
			this.inventory.OnItemStackChanged += this.HandleItemStackChanged;
			this.inventory.OnClothAdded += this.HandleClothChanged;
			this.inventory.OnClothRemoved += this.HandleClothRemoved;
			this.inventory.OnClothDurabilityChanged += this.HandleClothDurability;
			this.inventory.OnDurabilityChanged += this.HandleItemDurability;
			this._bound = true;
			this.RefreshAll();
		}

		// Token: 0x06008704 RID: 34564 RVA: 0x003732EC File Offset: 0x003714EC
		private void UnbindInventory()
		{
			this._bound = false;
			if (this.inventory == null)
			{
				return;
			}
			this.inventory.OnItemAdded -= this.HandleItemChanged;
			this.inventory.OnItemVisualChanged -= this.HandleItemChanged;
			this.inventory.OnItemRemoved -= this.HandleItemRemoved;
			this.inventory.OnItemStackChanged -= this.HandleItemStackChanged;
			this.inventory.OnClothAdded -= this.HandleClothChanged;
			this.inventory.OnClothRemoved -= this.HandleClothRemoved;
			this.inventory.OnClothDurabilityChanged -= this.HandleClothDurability;
			this.inventory.OnDurabilityChanged -= this.HandleItemDurability;
		}

		// Token: 0x06008705 RID: 34565 RVA: 0x003733C8 File Offset: 0x003715C8
		private void Update()
		{
			this.EnsureInventoryBinding();
			if (GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return;
			}
			if (this.toggleInput.GetButtonDown() && (!this.IsOpen || !ControllerInput.IsActive))
			{
				if (!this.IsOpen && InteractionCandidateRegistry.IsInventoryInputReserved(this.toggleInput))
				{
					return;
				}
				if (this.IsOpen)
				{
					this.Close();
				}
				else if (!this.TryOpen())
				{
					BlockedActionFeedback.Show("Stop what you're doing first");
				}
				if (ControllerInput.IsActive)
				{
					ControllerInput.ConsumeUIInput();
				}
				return;
			}
			else
			{
				if (this.IsOpen && this.closeOnEscape && EscapeInputCoordinator.TryConsume(this))
				{
					if (this._controllerMoveSource != null)
					{
						this.CancelControllerMove();
					}
					else
					{
						this.Close();
					}
					if (ControllerInput.IsActive)
					{
						ControllerInput.ConsumeUIInput();
					}
					return;
				}
				if (this.IsOpen)
				{
					this.HandleControllerInput();
				}
				return;
			}
		}

		// Token: 0x06008706 RID: 34566 RVA: 0x00373494 File Offset: 0x00371694
		private void HandleControllerInput()
		{
			bool isActive = ControllerInput.IsActive;
			this.UpdateControllerHint(isActive);
			if (!isActive || GameplayInputGate.BlocksGameplayInput || GraphicsSettingsRuntimeUI.AnyOpen || ControllerInput.UiInputConsumedThisFrame)
			{
				return;
			}
			if (this._dragSlot != null)
			{
				this.CancelDragState();
			}
			this.EnsureControllerSelection();
			Vector2 navigation = ControllerInput.Navigation;
			Vector2Int vector2Int = ((navigation.sqrMagnitude < 0.25f) ? Vector2Int.zero : ((Mathf.Abs(navigation.x) > Mathf.Abs(navigation.y)) ? new Vector2Int((navigation.x > 0f) ? 1 : (-1), 0) : new Vector2Int(0, (navigation.y > 0f) ? 1 : (-1))));
			if (vector2Int == Vector2Int.zero)
			{
				this._controllerDirection = vector2Int;
			}
			else if (vector2Int != this._controllerDirection || Time.unscaledTime >= this._nextControllerMove)
			{
				bool flag = vector2Int != this._controllerDirection;
				this._controllerDirection = vector2Int;
				this._nextControllerMove = Time.unscaledTime + (flag ? 0.32f : 0.14f);
				this.MoveControllerSelection(vector2Int);
				ControllerInput.ConsumeUIInput();
			}
			if (this._headphonesSelected)
			{
				this.HandleHeadphonesInput();
				return;
			}
			if (this._detailsScroll != null && Mathf.Abs(ControllerInput.RadialDirection.y) > 0.2f)
			{
				this._detailsScroll.verticalNormalizedPosition = Mathf.Clamp01(this._detailsScroll.verticalNormalizedPosition + ControllerInput.RadialDirection.y * Time.unscaledDeltaTime);
			}
			if (ControllerInput.SubmitDown)
			{
				ControllerInput.ConsumeUIInput();
				if (!(this._controllerMoveSource != null))
				{
					this.UseSelectedSlot();
					return;
				}
				if (this._selected != null && this._selected.Kind == this._controllerMoveSource.Kind)
				{
					if (this._selected != this._controllerMoveSource)
					{
						this.SwapSlots(this._controllerMoveSource, this._selected);
					}
					this.CancelControllerMove();
					return;
				}
			}
			else
			{
				if (ControllerInput.DropDown)
				{
					ControllerInput.ConsumeUIInput();
					this.CancelControllerMove();
					this.DropSelectedSlot();
					return;
				}
				if (ControllerInput.GetButtonDown(ControllerButton.West))
				{
					ControllerInput.ConsumeUIInput();
					if (this._controllerMoveSource != null)
					{
						this.CancelControllerMove();
						return;
					}
					if (this._selected != null && this._selected.HasItem)
					{
						this._controllerMoveSource = this._selected;
						this._controllerMoveSource.SetDropHighlight(true);
						return;
					}
				}
				else if (this._controllerMoveSource == null && ControllerInput.GetButtonDown(ControllerButton.LeftShoulder))
				{
					ControllerInput.ConsumeUIInput();
					this.FocusSelectedSlot();
				}
			}
		}

		// Token: 0x06008707 RID: 34567 RVA: 0x0037371C File Offset: 0x0037191C
		private void EnsureControllerSelection()
		{
			if (this._headphonesSelected && this._headphoneSlot != null)
			{
				return;
			}
			if (this._selected != null && this._selected.gameObject.activeInHierarchy)
			{
				return;
			}
			foreach (InventoryPanelSlotUI inventoryPanelSlotUI in this.itemSlots)
			{
				if (inventoryPanelSlotUI != null && inventoryPanelSlotUI.HasItem)
				{
					this.OnSlotClicked(inventoryPanelSlotUI);
					return;
				}
			}
			foreach (InventoryPanelSlotUI inventoryPanelSlotUI2 in this.clothSlots)
			{
				if (inventoryPanelSlotUI2 != null && inventoryPanelSlotUI2.HasItem)
				{
					this.OnSlotClicked(inventoryPanelSlotUI2);
					return;
				}
			}
			foreach (InventoryPanelSlotUI inventoryPanelSlotUI3 in this.itemSlots)
			{
				if (inventoryPanelSlotUI3 != null)
				{
					this.OnSlotClicked(inventoryPanelSlotUI3);
					return;
				}
			}
			foreach (InventoryPanelSlotUI inventoryPanelSlotUI4 in this.clothSlots)
			{
				if (inventoryPanelSlotUI4 != null)
				{
					this.OnSlotClicked(inventoryPanelSlotUI4);
					break;
				}
			}
		}

		// Token: 0x06008708 RID: 34568 RVA: 0x003738B4 File Offset: 0x00371AB4
		private void MoveControllerSelection(Vector2Int direction)
		{
			if (this.MoveHeadphonesSelection(direction))
			{
				return;
			}
			if (this._selected == null)
			{
				return;
			}
			List<InventoryPanelSlotUI> list = ((this._selected.Kind == InventoryPanelSlotUI.Collection.Item) ? this.itemSlots : this.clothSlots);
			int num = list.IndexOf(this._selected);
			if (direction.y != 0)
			{
				if (this._controllerMoveSource != null)
				{
					return;
				}
				list = ((direction.y > 0) ? this.itemSlots : this.clothSlots);
				num = Mathf.Clamp(num, 0, list.Count - 1);
			}
			else if (list.Count > 0)
			{
				num = (num + direction.x + list.Count) % list.Count;
			}
			if (num >= 0 && num < list.Count && list[num] != null)
			{
				this.OnSlotClicked(list[num]);
			}
		}

		// Token: 0x06008709 RID: 34569 RVA: 0x00373990 File Offset: 0x00371B90
		private void CancelControllerMove()
		{
			if (this._controllerMoveSource != null)
			{
				this._controllerMoveSource.SetDropHighlight(false);
			}
			this._controllerMoveSource = null;
		}

		// Token: 0x0600870A RID: 34570 RVA: 0x003739B4 File Offset: 0x00371BB4
		public void UseSelectedSlot()
		{
			if (!this.IsOpen || this._selected == null || !this._selected.HasItem || this.inventory == null)
			{
				return;
			}
			this.ResolveRefs();
			InventoryItemSO item = this._selected.Item;
			int slotIndex = this._selected.SlotIndex;
			bool flag = this._selected.Kind == InventoryPanelSlotUI.Collection.Clothing;
			this.Close();
			WearableItemSO wearableItemSO = item as WearableItemSO;
			if (wearableItemSO != null && this.wearController != null)
			{
				this.wearController.ToggleFromInventory(wearableItemSO, slotIndex, flag);
				return;
			}
			if (flag)
			{
				this.inventory.SelectCloth(slotIndex);
				return;
			}
			if (this.inventory.ActiveSlotIndex != slotIndex || this.inventory.ActiveClothIndex >= 0)
			{
				this.inventory.SelectSlot(slotIndex);
			}
		}

		// Token: 0x0600870B RID: 34571 RVA: 0x00373A84 File Offset: 0x00371C84
		public void FocusSelectedSlot()
		{
			if (!this.IsOpen || this._selected == null || !this._selected.HasItem || this.inventory == null)
			{
				return;
			}
			int slotIndex = this._selected.SlotIndex;
			bool flag = this._selected.Kind == InventoryPanelSlotUI.Collection.Clothing;
			this.Close();
			if (flag)
			{
				if (this.inventory.ActiveClothIndex != slotIndex)
				{
					this.inventory.SelectCloth(slotIndex);
					return;
				}
			}
			else if (this.inventory.ActiveSlotIndex != slotIndex || this.inventory.ActiveClothIndex >= 0)
			{
				this.inventory.SelectSlot(slotIndex);
			}
		}

		// Token: 0x0600870C RID: 34572 RVA: 0x00373B28 File Offset: 0x00371D28
		public void DropSelectedSlot()
		{
			if (!this.IsOpen || this._selected == null || !this._selected.HasItem || this.dropController == null)
			{
				return;
			}
			if (!this.dropController.CanDropFromInventory)
			{
				if (Application.isPlaying)
				{
					ToastService.Show("Leave wall cover before dropping items.", "Inventory", null, -1f, "inventory_feedback");
				}
				return;
			}
			WearableItemSO wearableItemSO = this._selected.Item as WearableItemSO;
			if (wearableItemSO != null && !InventoryDialogController.CanRemoveWearable(wearableItemSO))
			{
				BlockedActionFeedback.Show(null);
				return;
			}
			if (this._selected.Kind == InventoryPanelSlotUI.Collection.Item)
			{
				this.dropController.DropItemSlot(this._selected.SlotIndex);
			}
			else
			{
				this.dropController.DropClothSlot(this._selected.SlotIndex);
			}
			this.RefreshAll();
			this.PlaySfx(this.dropSFX);
		}

		// Token: 0x0600870D RID: 34573 RVA: 0x00373C08 File Offset: 0x00371E08
		private static bool CanRemoveWearable(WearableItemSO item)
		{
			RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
			return rankRuntimeController == null || RankProgressionService.IsAdminRankActive || rankRuntimeController.CanUnwearItem(item);
		}

		// Token: 0x0600870E RID: 34574 RVA: 0x00373C34 File Offset: 0x00371E34
		private void UpdateControllerHint(bool visible)
		{
			if (this._pointerCloseHint == null && this.panel != null)
			{
				Transform transform = this.panel.Find("Content/Actions/Hint");
				if (transform != null)
				{
					this._pointerCloseHint = transform.gameObject;
				}
			}
			if (visible && this._pointerCloseHint != null)
			{
				if (!this._pointerCloseHintCaptured)
				{
					this._pointerCloseHintWasActive = this._pointerCloseHint.activeSelf;
					this._pointerCloseHintCaptured = true;
				}
				this._pointerCloseHint.SetActive(false);
			}
			else if (this._pointerCloseHintCaptured)
			{
				if (this._pointerCloseHint != null)
				{
					this._pointerCloseHint.SetActive(this._pointerCloseHintWasActive);
				}
				this._pointerCloseHintCaptured = false;
			}
			if (visible && this._controllerHint == null && this.panel != null)
			{
				GameObject gameObject = new GameObject("ControllerHints", new Type[]
				{
					typeof(RectTransform),
					typeof(TextMeshProUGUI)
				});
				gameObject.layer = this.panel.gameObject.layer;
				gameObject.transform.SetParent(this.panel, false);
				this._controllerHint = gameObject.GetComponent<TextMeshProUGUI>();
				RectTransform rectTransform = this._controllerHint.rectTransform;
				rectTransform.anchorMin = new Vector2(0f, 0f);
				rectTransform.anchorMax = new Vector2(1f, 0f);
				rectTransform.pivot = new Vector2(0.5f, 0f);
				rectTransform.offsetMin = new Vector2(20f, 14f);
				rectTransform.offsetMax = new Vector2(-20f, 78f);
				this._controllerHint.fontSize = 16f;
				this._controllerHint.enableAutoSizing = true;
				this._controllerHint.fontSizeMin = 10f;
				this._controllerHint.fontSizeMax = 16f;
				this._controllerHint.alignment = TextAlignmentOptions.Center;
				this._controllerHint.raycastTarget = false;
				this._controllerHintBinding = LocalizedGameText.BindComputed(this._controllerHint, new Func<string>(this.ControllerHintPresentation));
				this._controllerHintBinding.FitTranslationToRectangle(false);
			}
			if (this._controllerHint == null)
			{
				return;
			}
			this._controllerHint.gameObject.SetActive(visible);
			int num = (this._headphonesSelected ? 1 : 0) | ((this._controllerMoveSource != null) ? 2 : 0);
			if (visible && (num != this._controllerHintState || this._controllerHintGlyphVersion != ControllerGlyphs.Version))
			{
				this._controllerHintState = num;
				this._controllerHintGlyphVersion = ControllerGlyphs.Version;
				this._controllerHintBinding.Refresh();
			}
		}

		// Token: 0x0600870F RID: 34575 RVA: 0x00373EE4 File Offset: 0x003720E4
		private string ControllerHintPresentation()
		{
			if (this._headphonesSelected)
			{
				return string.Concat(new string[]
				{
					InventoryDialogController.ControllerHint("A", "Play / Pause"),
					"   ",
					InventoryDialogController.ControllerHint("LB / RB", "Previous / Next"),
					"\n",
					InventoryDialogController.ControllerHint("RS", "Volume"),
					"   ",
					InventoryDialogController.ControllerHint("Y", "Drop"),
					"   ",
					InventoryDialogController.ControllerHint("B", "Back")
				});
			}
			return string.Concat(new string[]
			{
				InventoryDialogController.ControllerHint("D-Pad / LS", "Select slot"),
				"   ",
				InventoryDialogController.ControllerHint("A", (this._controllerMoveSource != null) ? "Move" : "Equip / Use"),
				"   ",
				(this._controllerMoveSource != null) ? InventoryDialogController.ControllerHint("B", "Cancel") : string.Concat(new string[]
				{
					InventoryDialogController.ControllerHint("LB", "Select and show actions"),
					"\n",
					InventoryDialogController.ControllerHint("Y", "Drop"),
					"   ",
					InventoryDialogController.ControllerHint("X", "Move"),
					"   ",
					InventoryDialogController.ControllerHint("B", "Back"),
					"   ",
					InventoryDialogController.ControllerHint("RS", "Scroll")
				})
			});
		}

		// Token: 0x06008710 RID: 34576 RVA: 0x00374082 File Offset: 0x00372282
		private static string ControllerHint(string keys, string action)
		{
			return ControllerGlyphs.Format(keys) + ": " + GameLocalization.GetSource("inventory_ui", action);
		}

		// Token: 0x06008711 RID: 34577 RVA: 0x003740A0 File Offset: 0x003722A0
		private void EnsureInventoryBinding()
		{
			if (this._bound && (this.inventory == null || (PlayerInventoryManager.Instance != null && this.inventory != PlayerInventoryManager.Instance)))
			{
				this.UnbindInventory();
			}
			if (!this._bound)
			{
				this.ResolveRefs();
				this.BindInventory();
			}
		}

		// Token: 0x06008712 RID: 34578 RVA: 0x003740FC File Offset: 0x003722FC
		public void Toggle()
		{
			if (this.IsOpen)
			{
				this.Close();
				return;
			}
			this.Open();
		}

		// Token: 0x06008713 RID: 34579 RVA: 0x00374113 File Offset: 0x00372313
		public void Open()
		{
			this.TryOpen();
		}

		// Token: 0x06008714 RID: 34580 RVA: 0x0037411C File Offset: 0x0037231C
		private bool TryOpen()
		{
			if (this.IsOpen)
			{
				return true;
			}
			if (RadialControllerNavigation.AnyWheelOpen)
			{
				return false;
			}
			if (CielOSController.AnyOpen || !this.TryAcquireDialogContext())
			{
				return false;
			}
			if (!base.isActiveAndEnabled || this._dialogLease == null || !this._dialogLease.IsActive)
			{
				this.ReleaseDialogContext();
				return false;
			}
			bool flag;
			try
			{
				this.IsOpen = true;
				this._headphoneSlot = base.GetComponentInChildren<HeadphonesInventorySlotUI>(true);
				this._controllerDirection = Vector2Int.zero;
				this._nextControllerMove = 0f;
				this.RefreshAll();
				this.SetGameplayLocked(true);
				this.ApplyOpenState(true, false);
				this.UpdateControllerHint(ControllerInput.IsActive);
				this.PlaySfx(this.openSFX);
				base.StartCoroutine(this.ForceTextRebuildNextFrame());
				flag = true;
			}
			catch
			{
				this.CloseInternal(true, false);
				throw;
			}
			return flag;
		}

		// Token: 0x06008715 RID: 34581 RVA: 0x003741F8 File Offset: 0x003723F8
		private IEnumerator ForceTextRebuildNextFrame()
		{
			yield return null;
			TextMeshProUGUI[] componentsInChildren = base.GetComponentsInChildren<TextMeshProUGUI>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null)
				{
					componentsInChildren[i].ForceMeshUpdate(true, true);
				}
			}
			yield break;
		}

		// Token: 0x06008716 RID: 34582 RVA: 0x00374207 File Offset: 0x00372407
		public void Close()
		{
			this.CloseInternal(false, true);
		}

		// Token: 0x06008717 RID: 34583 RVA: 0x00374211 File Offset: 0x00372411
		public void InterruptForEncounter()
		{
			this.CloseInternal(true, false);
		}

		// Token: 0x06008718 RID: 34584 RVA: 0x0037421C File Offset: 0x0037241C
		private void CloseInternal(bool instant, bool playSfx)
		{
			this.UpdateControllerHint(false);
			bool isOpen = this.IsOpen;
			if (!isOpen && !this._gameplayStateCaptured && this._dialogLease == null)
			{
				return;
			}
			this.IsOpen = false;
			this.CancelControllerMove();
			this._controllerDirection = Vector2Int.zero;
			this.CancelDragState();
			this.SetGameplayLocked(false);
			this.ApplyOpenState(false, instant);
			this.ReleaseDialogContext();
			if (isOpen && playSfx)
			{
				this.PlaySfx(this.closeSFX);
			}
		}

		// Token: 0x06008719 RID: 34585 RVA: 0x00374290 File Offset: 0x00372490
		private void PlaySfx(AudioClip clip)
		{
			if (clip == null || this.sfxVolume <= 0f)
			{
				return;
			}
			if (this.uiAudioSource != null)
			{
				this.uiAudioSource.PlayOneShot(clip, this.sfxVolume);
				return;
			}
			AudioSource.PlayClipAtPoint(clip, (Camera.main != null) ? Camera.main.transform.position : Vector3.zero, this.sfxVolume);
		}

		// Token: 0x0600871A RID: 34586 RVA: 0x00374304 File Offset: 0x00372504
		private void ApplySortingPriority(bool open)
		{
			if (this._sortCanvas == null)
			{
				this._sortCanvas = base.GetComponent<Canvas>();
				if (this._sortCanvas == null)
				{
					this._sortCanvas = base.gameObject.AddComponent<Canvas>();
				}
				GameplayHudVisibility.RegisterCanvas(this._sortCanvas);
			}
			if (this._sortRaycaster == null)
			{
				this._sortRaycaster = base.GetComponent<GraphicRaycaster>();
				if (this._sortRaycaster == null)
				{
					this._sortRaycaster = base.gameObject.AddComponent<GraphicRaycaster>();
				}
			}
			this._sortCanvas.overrideSorting = open;
			if (open)
			{
				this._sortCanvas.sortingOrder = this.openSortingOrder;
			}
			this._sortRaycaster.enabled = open;
		}

		// Token: 0x0600871B RID: 34587 RVA: 0x003743BC File Offset: 0x003725BC
		private void ApplyOpenState(bool open, bool instant)
		{
			if (this.canvasGroup != null)
			{
				this.canvasGroup.interactable = open;
				this.canvasGroup.blocksRaycasts = open;
			}
			this.ApplySortingPriority(open);
			if (this.panel == null)
			{
				if (this.canvasGroup != null)
				{
					this.canvasGroup.alpha = (open ? 1f : 0f);
				}
				return;
			}
			if (this._slideCo != null)
			{
				base.StopCoroutine(this._slideCo);
			}
			if (instant || this.slideDuration <= 0f)
			{
				this.panel.anchoredPosition = (open ? this._shownPos : this._hiddenPos);
				if (this.canvasGroup != null)
				{
					this.canvasGroup.alpha = (open ? 1f : 0f);
					return;
				}
			}
			else
			{
				this._slideCo = base.StartCoroutine(this.SlideRoutine(open));
			}
		}

		// Token: 0x0600871C RID: 34588 RVA: 0x003744A9 File Offset: 0x003726A9
		private IEnumerator SlideRoutine(bool open)
		{
			Vector2 from = this.panel.anchoredPosition;
			Vector2 to = (open ? this._shownPos : this._hiddenPos);
			float fromAlpha = ((this.canvasGroup != null) ? this.canvasGroup.alpha : 1f);
			float toAlpha = (open ? 1f : 0f);
			if (open && this.canvasGroup != null)
			{
				this.canvasGroup.alpha = Mathf.Max(fromAlpha, 0.01f);
			}
			float t = 0f;
			float dur = Mathf.Max(0.01f, this.slideDuration);
			while (t < dur)
			{
				t += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / dur));
				this.panel.anchoredPosition = Vector2.LerpUnclamped(from, to, num);
				if (this.canvasGroup != null)
				{
					this.canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, num);
				}
				yield return null;
			}
			this.panel.anchoredPosition = to;
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = toAlpha;
			}
			this._slideCo = null;
			yield break;
		}

		// Token: 0x0600871D RID: 34589 RVA: 0x003744C0 File Offset: 0x003726C0
		private void CacheSlidePositions()
		{
			if (this.panel == null)
			{
				return;
			}
			this._shownPos = this.panel.anchoredPosition;
			float num = ((this.panel.rect.width > 1f) ? this.panel.rect.width : 600f);
			this._hiddenPos = this._shownPos + new Vector2(num + this.offscreenPadding, 0f);
		}

		// Token: 0x0600871E RID: 34590 RVA: 0x00374548 File Offset: 0x00372748
		private bool TryAcquireDialogContext()
		{
			if (this._dialogLease != null)
			{
				if (this._dialogLease.IsActive)
				{
					return true;
				}
				this._dialogLease = null;
			}
			PlayerContextResource playerContextResource = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI;
			vBasicCoverController activeController = WallCoverActivityLock.ActiveController;
			if (activeController != null)
			{
				if (!activeController.CanOpenModalInventory(playerContextResource))
				{
					return false;
				}
				activeController.PrepareForModalUI();
				playerContextResource &= ~(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl);
			}
			return PlayerActivityGate.TryAcquireUtilityModal("InventoryDialog#" + base.GetInstanceID().ToString(), this, PlayerActivity.Inventory, playerContextResource, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.StanceSwitch | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.ArmsPose, out this._dialogLease, null);
		}

		// Token: 0x0600871F RID: 34591 RVA: 0x003745CB File Offset: 0x003727CB
		private void ReleaseDialogContext()
		{
			if (this._dialogLease == null)
			{
				return;
			}
			this._dialogLease.Dispose();
			this._dialogLease = null;
		}

		// Token: 0x06008720 RID: 34592 RVA: 0x003745E8 File Offset: 0x003727E8
		private void SetGameplayLocked(bool locked)
		{
			if (locked && !this.lockPlayerInput)
			{
				return;
			}
			if (this._playerInput == null)
			{
				this._playerInput = ((this.player != null) ? this.player.GetComponentInChildren<vThirdPersonInput>(true) : Object.FindObjectOfType<vThirdPersonInput>());
			}
			if (locked)
			{
				if (this._gameplayStateCaptured)
				{
					return;
				}
				if (this._playerInput != null)
				{
					this._prevLockBasic = this._playerInput.lockInput;
					this._prevLockCamera = this._playerInput.lockCameraInput;
					this._playerInput.SetLockAllInput(true);
					this._playerInput.SetLockCameraInput(true);
					if (this._playerInput.cc != null)
					{
						this._playerInput.cc.input = Vector2.zero;
						this._playerInput.cc.isSprinting = false;
					}
				}
				this._prevForceCameraLock = vThirdPersonInput.ForceCameraLock;
				vThirdPersonInput.ForceCameraLock = true;
				this._prevCursorLock = Cursor.lockState;
				this._prevCursorVisible = Cursor.visible;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				this._gameplayStateCaptured = true;
				return;
			}
			else
			{
				if (!this._gameplayStateCaptured)
				{
					return;
				}
				if (this._playerInput != null)
				{
					this._playerInput.SetLockAllInput(this._prevLockBasic);
					this._playerInput.SetLockCameraInput(this._prevLockCamera);
				}
				vThirdPersonInput.ForceCameraLock = this._prevForceCameraLock;
				Cursor.lockState = this._prevCursorLock;
				Cursor.visible = this._prevCursorVisible;
				this._gameplayStateCaptured = false;
				return;
			}
		}

		// Token: 0x06008721 RID: 34593 RVA: 0x00374768 File Offset: 0x00372968
		private void CancelDragState()
		{
			InventoryPanelSlotUI dragSlot = this._dragSlot;
			if (dragSlot != null)
			{
				dragSlot.SetDragSource(false);
			}
			if (this._ghost != null)
			{
				this._ghost.gameObject.SetActive(false);
				if (Application.isPlaying)
				{
					Object.Destroy(this._ghost.gameObject);
				}
				else
				{
					Object.DestroyImmediate(this._ghost.gameObject);
				}
			}
			this._ghost = null;
			this._ghostImage = null;
			this._ghostParent = null;
			InventoryPanelSlotUI hoverSlot = this._hoverSlot;
			if (hoverSlot != null)
			{
				hoverSlot.SetDropHighlight(false);
			}
			this._hoverSlot = null;
			this._dragSlot = null;
			this._dragPointerId = -1;
		}

		// Token: 0x06008722 RID: 34594 RVA: 0x0037480C File Offset: 0x00372A0C
		private void InitSlots()
		{
			for (int i = 0; i < this.itemSlots.Count; i++)
			{
				if (this.itemSlots[i] != null)
				{
					this.itemSlots[i].Init(this, InventoryPanelSlotUI.Collection.Item, i);
				}
			}
			for (int j = 0; j < this.clothSlots.Count; j++)
			{
				if (this.clothSlots[j] != null)
				{
					this.clothSlots[j].Init(this, InventoryPanelSlotUI.Collection.Clothing, j);
				}
			}
			this.ShowDetails(null, null);
		}

		// Token: 0x06008723 RID: 34595 RVA: 0x003748A0 File Offset: 0x00372AA0
		private void RefreshAll()
		{
			if (this.inventory == null)
			{
				return;
			}
			for (int i = 0; i < this.itemSlots.Count; i++)
			{
				InventoryPanelSlotUI inventoryPanelSlotUI = this.itemSlots[i];
				if (!(inventoryPanelSlotUI == null))
				{
					InventoryItemSO itemAt = this.inventory.GetItemAt(i);
					inventoryPanelSlotUI.SetItem(itemAt, this.inventory.GetItemDurability01(i));
				}
			}
			for (int j = 0; j < this.clothSlots.Count; j++)
			{
				InventoryPanelSlotUI inventoryPanelSlotUI2 = this.clothSlots[j];
				if (!(inventoryPanelSlotUI2 == null))
				{
					InventoryItemSO clothAt = this.inventory.GetClothAt(j);
					inventoryPanelSlotUI2.SetItem(clothAt, this.ClothDurability01(j));
				}
			}
			if (this._selected != null)
			{
				this.ShowDetails(this._selected.Item, this._selected);
			}
		}

		// Token: 0x06008724 RID: 34596 RVA: 0x00374979 File Offset: 0x00372B79
		private float ClothDurability01(int index)
		{
			if (this.inventory.GetClothAt(index) == null)
			{
				return 1f;
			}
			return Mathf.Clamp01(this.inventory.GetClothDurability(index) / this.inventory.GetClothDurabilityBase(index));
		}

		// Token: 0x06008725 RID: 34597 RVA: 0x003749B4 File Offset: 0x00372BB4
		public void OnSlotClicked(InventoryPanelSlotUI slot)
		{
			if (slot == null)
			{
				return;
			}
			if (this._selected != null)
			{
				this._selected.SetSelected(false);
			}
			this.ClearHeadphonesSelection();
			this._selected = slot;
			slot.SetSelected(true);
			this.PlaySfx(this.selectSFX);
			this.ShowDetails(slot.Item, slot);
			if (this._detailsScroll != null)
			{
				this._detailsScroll.verticalNormalizedPosition = 1f;
			}
		}

		// Token: 0x06008726 RID: 34598 RVA: 0x00374A30 File Offset: 0x00372C30
		private void ShowDetails(InventoryItemSO item, InventoryPanelSlotUI slot = null)
		{
			if (this._detailsScroll == null)
			{
				this._detailsScroll = InventoryDetailsView.Create(this.titleText, this.descriptionText);
			}
			if (this.titleText != null)
			{
				LocalizedGameText.BindSource(this.titleText, "items", (item != null) ? item.itemName : string.Empty, null);
			}
			if (this.descriptionText == null)
			{
				return;
			}
			ItemRarity itemRarity = ((item != null) ? item.rarity : ItemRarity.Common);
			int num = -1;
			if (slot != null && this.inventory != null)
			{
				bool flag = slot.Kind == InventoryPanelSlotUI.Collection.Clothing;
				itemRarity = (flag ? this.inventory.GetClothRarityAt(slot.SlotIndex) : this.inventory.GetItemRarityAt(slot.SlotIndex));
				num = (flag ? this.inventory.GetClothVariantAt(slot.SlotIndex) : this.inventory.GetItemVariantAt(slot.SlotIndex));
			}
			if (item != null)
			{
				InventoryItemSO capturedItem = item;
				ItemRarity capturedRarity = itemRarity;
				int capturedVariant = num;
				bool capturedLongWayHome = RunModeService.IsLongWayHome;
				LocalizedGameText.BindComputed(this.descriptionText, () => InventoryItemDetails.Build(capturedItem, capturedRarity, capturedVariant, capturedLongWayHome));
			}
			else
			{
				LocalizedGameText.BindSource(this.descriptionText, "inventory_ui", "<color=#D9CADF>Select an item to see its details.</color>", null);
			}
			if (this._detailsScroll != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.descriptionText.rectTransform);
				this._detailsScroll.StopMovement();
			}
		}

		// Token: 0x06008727 RID: 34599 RVA: 0x00374BB4 File Offset: 0x00372DB4
		public void BeginSlotDrag(InventoryPanelSlotUI slot, PointerEventData eventData)
		{
			if (slot == null || !slot.HasItem || eventData == null || eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (this._dragSlot != null)
			{
				return;
			}
			RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
			if (rankRuntimeController != null && !RankProgressionService.IsAdminRankActive)
			{
				WearableItemSO wearableItemSO = slot.Item as WearableItemSO;
				if (wearableItemSO != null && !rankRuntimeController.CanUnwearItem(wearableItemSO))
				{
					BlockedActionFeedback.Show(null);
					return;
				}
			}
			if (this.rootCanvas == null)
			{
				this.rootCanvas = base.GetComponentInParent<Canvas>();
			}
			if (this.rootCanvas == null)
			{
				return;
			}
			this.CancelControllerMove();
			this._dragSlot = slot;
			this._dragPointerId = eventData.pointerId;
			slot.SetDragSource(true);
			this._ghostParent = (RectTransform)base.transform;
			GameObject gameObject = new GameObject("DragGhost", new Type[] { typeof(RectTransform) });
			this._ghost = (RectTransform)gameObject.transform;
			this._ghost.SetParent(this._ghostParent, false);
			Shadow shadow = gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color(0f, 0f, 0f, 0.6f);
			shadow.effectDistance = new Vector2(3f, -3f);
			this._ghostImage = gameObject.AddComponent<Image>();
			this._ghostImage.raycastTarget = false;
			this._ghostImage.sprite = ((slot.Item != null) ? slot.Item.icon : null);
			StreamerCensorImage.BindItem(this._ghostImage, slot.Item);
			this._ghostImage.enabled = this._ghostImage.sprite != null;
			this._ghostImage.preserveAspect = true;
			this._ghostImage.color = Color.white;
			this._ghost.sizeDelta = new Vector2(96f, 96f);
			this._ghost.SetAsLastSibling();
			this.MoveGhost(eventData);
		}

		// Token: 0x06008728 RID: 34600 RVA: 0x00374DB0 File Offset: 0x00372FB0
		public void UpdateSlotDrag(PointerEventData eventData)
		{
			if (!this.OwnsSlotDrag(eventData))
			{
				return;
			}
			if (this._ghost != null)
			{
				this.MoveGhost(eventData);
			}
			InventoryPanelSlotUI inventoryPanelSlotUI = this.ResolveSwapTarget(this._dragSlot, eventData);
			if (inventoryPanelSlotUI != this._hoverSlot)
			{
				InventoryPanelSlotUI hoverSlot = this._hoverSlot;
				if (hoverSlot != null)
				{
					hoverSlot.SetDropHighlight(false);
				}
				this._hoverSlot = inventoryPanelSlotUI;
				InventoryPanelSlotUI hoverSlot2 = this._hoverSlot;
				if (hoverSlot2 == null)
				{
					return;
				}
				hoverSlot2.SetDropHighlight(true);
			}
		}

		// Token: 0x06008729 RID: 34601 RVA: 0x00374E24 File Offset: 0x00373024
		public void EndSlotDrag(InventoryPanelSlotUI slot, PointerEventData eventData)
		{
			if (slot != this._dragSlot || !this.OwnsSlotDrag(eventData))
			{
				return;
			}
			this.CancelDragState();
			InventoryPanelSlotUI inventoryPanelSlotUI = this.ResolveSwapTarget(slot, eventData);
			if (inventoryPanelSlotUI != null)
			{
				this.SwapSlots(slot, inventoryPanelSlotUI);
				return;
			}
			if ((this.panel == null || !RectTransformUtility.RectangleContainsScreenPoint(this.panel, eventData.position, eventData.pressEventCamera)) && this.dropController != null)
			{
				if (!this.dropController.CanDropFromInventory)
				{
					if (Application.isPlaying)
					{
						ToastService.Show("Leave wall cover before dropping items.", "Inventory", null, -1f, "inventory_feedback");
					}
					return;
				}
				if (slot.Kind == InventoryPanelSlotUI.Collection.Item)
				{
					this.dropController.DropItemSlotTowards(slot.SlotIndex, eventData.position);
				}
				else
				{
					this.dropController.DropClothSlotTowards(slot.SlotIndex, eventData.position);
				}
				this.PlaySfx(this.dropSFX);
				if (this._selected == slot)
				{
					this._selected = null;
					this.ShowDetails(null, null);
				}
			}
		}

		// Token: 0x0600872A RID: 34602 RVA: 0x00374F3B File Offset: 0x0037313B
		private bool OwnsSlotDrag(PointerEventData eventData)
		{
			return this._dragSlot != null && eventData != null && eventData.button == PointerEventData.InputButton.Left && eventData.pointerId == this._dragPointerId;
		}

		// Token: 0x0600872B RID: 34603 RVA: 0x00374F68 File Offset: 0x00373168
		private InventoryPanelSlotUI ResolveSwapTarget(InventoryPanelSlotUI source, PointerEventData eventData)
		{
			if (source == null || EventSystem.current == null)
			{
				return null;
			}
			this._raycastHits.Clear();
			EventSystem.current.RaycastAll(eventData, this._raycastHits);
			for (int i = 0; i < this._raycastHits.Count; i++)
			{
				GameObject gameObject = this._raycastHits[i].gameObject;
				if (!(gameObject == null))
				{
					InventoryPanelSlotUI componentInParent = gameObject.GetComponentInParent<InventoryPanelSlotUI>();
					if (!(componentInParent == null) && !(componentInParent == source) && componentInParent.Kind == source.Kind)
					{
						return componentInParent;
					}
				}
			}
			return null;
		}

		// Token: 0x0600872C RID: 34604 RVA: 0x00375008 File Offset: 0x00373208
		private void SwapSlots(InventoryPanelSlotUI a, InventoryPanelSlotUI b)
		{
			if (this.inventory == null || a == null || b == null)
			{
				return;
			}
			int slotIndex = a.SlotIndex;
			int slotIndex2 = b.SlotIndex;
			if (a.Kind == InventoryPanelSlotUI.Collection.Item)
			{
				this.inventory.MoveItem(slotIndex, slotIndex2);
			}
			else
			{
				this.inventory.MoveCloth(slotIndex, slotIndex2);
			}
			this.PlaySfx(this.swapSFX);
			if (this._selected == a)
			{
				a.SetSelected(false);
				this._selected = b;
				b.SetSelected(true);
				this.ShowDetails(b.Item, b);
			}
		}

		// Token: 0x0600872D RID: 34605 RVA: 0x003750A4 File Offset: 0x003732A4
		private void MoveGhost(PointerEventData eventData)
		{
			if (this._ghost == null || this._ghostParent == null)
			{
				return;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this._ghostParent, eventData.position, eventData.pressEventCamera, out vector);
			this._ghost.anchoredPosition = vector;
		}

		// Token: 0x0600872E RID: 34606 RVA: 0x003750F4 File Offset: 0x003732F4
		private void HandleItemChanged(int index, InventoryItemSO item)
		{
			if (index >= 0 && index < this.itemSlots.Count && this.itemSlots[index] != null)
			{
				this.itemSlots[index].SetItem(item, this.inventory.GetItemDurability01(index));
				if (this._selected == this.itemSlots[index])
				{
					this.ShowDetails(item, this._selected);
				}
			}
		}

		// Token: 0x0600872F RID: 34607 RVA: 0x0037516C File Offset: 0x0037336C
		private void HandleItemDurability(int index, float normalized01)
		{
			if (index >= 0 && index < this.itemSlots.Count && this.itemSlots[index] != null && this.itemSlots[index].HasItem)
			{
				this.itemSlots[index].SetItem(this.itemSlots[index].Item, normalized01);
			}
		}

		// Token: 0x06008730 RID: 34608 RVA: 0x003751D8 File Offset: 0x003733D8
		private void HandleItemStackChanged(int index, int previous, int current)
		{
			if (index >= 0 && index < this.itemSlots.Count && this.itemSlots[index] != null)
			{
				this.itemSlots[index].SetStackCount(current);
			}
			if (this._selected != null && this._selected.Kind == InventoryPanelSlotUI.Collection.Item && this._selected.SlotIndex == index)
			{
				this.ShowDetails(this._selected.Item, this._selected);
			}
		}

		// Token: 0x06008731 RID: 34609 RVA: 0x00375260 File Offset: 0x00373460
		private void HandleItemRemoved(int index)
		{
			if (index >= 0 && index < this.itemSlots.Count && this.itemSlots[index] != null)
			{
				this.itemSlots[index].SetItem(null, 1f);
				if (this._selected == this.itemSlots[index])
				{
					this.ShowDetails(null, null);
				}
			}
		}

		// Token: 0x06008732 RID: 34610 RVA: 0x003752CC File Offset: 0x003734CC
		private void HandleClothChanged(int index, InventoryItemSO cloth)
		{
			if (index >= 0 && index < this.clothSlots.Count && this.clothSlots[index] != null)
			{
				this.clothSlots[index].SetItem(cloth, this.ClothDurability01(index));
				if (this._selected == this.clothSlots[index])
				{
					this.ShowDetails(cloth, this._selected);
				}
			}
		}

		// Token: 0x06008733 RID: 34611 RVA: 0x00375340 File Offset: 0x00373540
		private void HandleClothRemoved(int index)
		{
			if (index >= 0 && index < this.clothSlots.Count && this.clothSlots[index] != null)
			{
				this.clothSlots[index].SetItem(null, 1f);
				if (this._selected == this.clothSlots[index])
				{
					this.ShowDetails(null, null);
				}
			}
		}

		// Token: 0x06008734 RID: 34612 RVA: 0x003753AC File Offset: 0x003735AC
		private void HandleClothDurability(int index, float normalized01)
		{
			if (index >= 0 && index < this.clothSlots.Count && this.clothSlots[index] != null && this.clothSlots[index].HasItem)
			{
				this.clothSlots[index].SetItem(this.clothSlots[index].Item, normalized01);
			}
		}

		// Token: 0x06008735 RID: 34613 RVA: 0x00375418 File Offset: 0x00373618
		public void SelectHeadphones(HeadphonesInventorySlotUI slot)
		{
			if (!this.IsOpen || slot == null || this._controllerMoveSource != null)
			{
				return;
			}
			InventoryPanelSlotUI selected = this._selected;
			if (selected != null)
			{
				selected.SetSelected(false);
			}
			this._selected = null;
			this._headphoneSlot = slot;
			this._headphonesSelected = true;
			slot.SetControllerSelected(true);
			LocalizedGameText.BindSource(this.titleText, "inventory_ui", "Headphones", null);
			LocalizedGameText.BindSource(this.descriptionText, "inventory_ui", "Select the headphones to control your music.", null);
			this.UpdateControllerHint(ControllerInput.IsActive);
			this.PlaySfx(this.selectSFX);
		}

		// Token: 0x06008736 RID: 34614 RVA: 0x003754B8 File Offset: 0x003736B8
		private void ClearHeadphonesSelection()
		{
			this._headphonesSelected = false;
			HeadphonesInventorySlotUI headphoneSlot = this._headphoneSlot;
			if (headphoneSlot == null)
			{
				return;
			}
			headphoneSlot.SetControllerSelected(false);
		}

		// Token: 0x06008737 RID: 34615 RVA: 0x003754D4 File Offset: 0x003736D4
		private bool MoveHeadphonesSelection(Vector2Int direction)
		{
			if (this._headphoneSlot == null || this._controllerMoveSource != null)
			{
				return false;
			}
			if (this._headphonesSelected)
			{
				List<InventoryPanelSlotUI> list = ((direction.y < 0) ? this.clothSlots : this.itemSlots);
				if (direction.y > 0 || list.Count == 0)
				{
					return true;
				}
				int num = ((direction.x > 0) ? 0 : (list.Count - 1));
				if (list[num] != null)
				{
					this.OnSlotClicked(list[num]);
				}
				return true;
			}
			else
			{
				if (this._selected == null || this._selected.Kind != InventoryPanelSlotUI.Collection.Item || direction.y != 0)
				{
					return false;
				}
				int num2 = this.itemSlots.IndexOf(this._selected);
				if ((direction.x > 0 && num2 == this.itemSlots.Count - 1) || (direction.x < 0 && num2 == 0))
				{
					this.SelectHeadphones(this._headphoneSlot);
					return true;
				}
				return false;
			}
		}

		// Token: 0x06008738 RID: 34616 RVA: 0x003755D8 File Offset: 0x003737D8
		private void HandleHeadphonesInput()
		{
			if (ControllerInput.UiInputConsumedThisFrame || !HeadphonesEquipment.IsWorn)
			{
				return;
			}
			if (ControllerInput.DropDown)
			{
				ControllerInput.ConsumeUIInput();
				HeadphonesEquipment headphonesEquipment = HeadphonesEquipment.Current;
				if (headphonesEquipment != null)
				{
					headphonesEquipment.TryDropTowardsScreenPoint(new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f));
				}
				HeadphonesInventorySlotUI headphoneSlot = this._headphoneSlot;
				if (headphoneSlot == null)
				{
					return;
				}
				headphoneSlot.RefreshState();
				return;
			}
			else
			{
				CielOSAudioService instance = CielOSAudioService.Instance;
				if (instance == null)
				{
					return;
				}
				if (ControllerInput.SubmitDown)
				{
					instance.TogglePlayPause();
				}
				else if (ControllerInput.GetButtonDown(ControllerButton.LeftShoulder))
				{
					instance.SkipPrevious();
				}
				else if (ControllerInput.GetButtonDown(ControllerButton.RightShoulder))
				{
					instance.Next();
				}
				else
				{
					if (Mathf.Abs(ControllerInput.RadialDirection.y) <= 0.2f)
					{
						return;
					}
					instance.Volume += ControllerInput.RadialDirection.y * Time.unscaledDeltaTime * 0.4f;
				}
				ControllerInput.ConsumeUIInput();
				return;
			}
		}

		// Token: 0x040089AE RID: 35246
		[Header("Input")]
		[SerializeField]
		private GenericInput toggleInput = new GenericInput("I", "", "");

		// Token: 0x040089AF RID: 35247
		[Tooltip("Also close on Escape.")]
		[SerializeField]
		private bool closeOnEscape = true;

		// Token: 0x040089B0 RID: 35248
		[Header("References")]
		[SerializeField]
		private PlayerInventoryManager inventory;

		// Token: 0x040089B1 RID: 35249
		[SerializeField]
		private WearableUseController wearController;

		// Token: 0x040089B2 RID: 35250
		[SerializeField]
		private GameObject player;

		// Token: 0x040089B3 RID: 35251
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x040089B4 RID: 35252
		[Tooltip("The sliding panel RectTransform (the purple panel). Slides between off-screen-right and on-screen.")]
		[SerializeField]
		private RectTransform panel;

		// Token: 0x040089B5 RID: 35253
		[Header("Slots (authored in prefab)")]
		[SerializeField]
		private List<InventoryPanelSlotUI> itemSlots = new List<InventoryPanelSlotUI>();

		// Token: 0x040089B6 RID: 35254
		[SerializeField]
		private List<InventoryPanelSlotUI> clothSlots = new List<InventoryPanelSlotUI>();

		// Token: 0x040089B7 RID: 35255
		[Header("Details")]
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x040089B8 RID: 35256
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x040089B9 RID: 35257
		[Header("Animation")]
		[SerializeField]
		private float slideDuration = 0.28f;

		// Token: 0x040089BA RID: 35258
		[Tooltip("How far off-screen (to the right) the panel sits when closed, in its own width units.")]
		[SerializeField]
		private float offscreenPadding = 40f;

		// Token: 0x040089BB RID: 35259
		[Header("Audio (clips optional - assign in editor)")]
		[SerializeField]
		private AudioSource uiAudioSource;

		// Token: 0x040089BC RID: 35260
		[Tooltip("Master volume applied to every inventory UI sound (open/close/select/drop/swap).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float sfxVolume = 1f;

		// Token: 0x040089BD RID: 35261
		[SerializeField]
		private AudioClip openSFX;

		// Token: 0x040089BE RID: 35262
		[SerializeField]
		private AudioClip closeSFX;

		// Token: 0x040089BF RID: 35263
		[SerializeField]
		private AudioClip selectSFX;

		// Token: 0x040089C0 RID: 35264
		[SerializeField]
		private AudioClip dropSFX;

		// Token: 0x040089C1 RID: 35265
		[Tooltip("Played when two slots are swapped via drag-reorder.")]
		[SerializeField]
		private AudioClip swapSFX;

		// Token: 0x040089C2 RID: 35266
		[Header("Behavior")]
		[SerializeField]
		private bool lockPlayerInput = true;

		// Token: 0x040089C3 RID: 35267
		[Header("Drag to drop")]
		[SerializeField]
		private InventoryDropController dropController;

		// Token: 0x040089C4 RID: 35268
		[Tooltip("Canvas the ghost icon is parented under while dragging. Defaults to this dialog's root canvas.")]
		[SerializeField]
		private Canvas rootCanvas;

		// Token: 0x040089C6 RID: 35270
		private InventoryPanelSlotUI _dragSlot;

		// Token: 0x040089C7 RID: 35271
		private int _dragPointerId = -1;

		// Token: 0x040089C8 RID: 35272
		private InventoryPanelSlotUI _hoverSlot;

		// Token: 0x040089C9 RID: 35273
		private RectTransform _ghost;

		// Token: 0x040089CA RID: 35274
		private RectTransform _ghostParent;

		// Token: 0x040089CB RID: 35275
		private Image _ghostImage;

		// Token: 0x040089CC RID: 35276
		private readonly List<RaycastResult> _raycastHits = new List<RaycastResult>();

		// Token: 0x040089CD RID: 35277
		private InventoryPanelSlotUI _selected;

		// Token: 0x040089CE RID: 35278
		private Coroutine _slideCo;

		// Token: 0x040089CF RID: 35279
		private vThirdPersonInput _playerInput;

		// Token: 0x040089D0 RID: 35280
		private ScrollRect _detailsScroll;

		// Token: 0x040089D1 RID: 35281
		private InventoryPanelSlotUI _controllerMoveSource;

		// Token: 0x040089D2 RID: 35282
		private Vector2Int _controllerDirection;

		// Token: 0x040089D3 RID: 35283
		private float _nextControllerMove;

		// Token: 0x040089D4 RID: 35284
		private TextMeshProUGUI _controllerHint;

		// Token: 0x040089D5 RID: 35285
		private LocalizedGameText _controllerHintBinding;

		// Token: 0x040089D6 RID: 35286
		private int _controllerHintState = -1;

		// Token: 0x040089D7 RID: 35287
		private int _controllerHintGlyphVersion = -1;

		// Token: 0x040089D8 RID: 35288
		private GameObject _pointerCloseHint;

		// Token: 0x040089D9 RID: 35289
		private bool _pointerCloseHintCaptured;

		// Token: 0x040089DA RID: 35290
		private bool _pointerCloseHintWasActive;

		// Token: 0x040089DB RID: 35291
		private bool _prevLockBasic;

		// Token: 0x040089DC RID: 35292
		private bool _prevLockCamera;

		// Token: 0x040089DD RID: 35293
		private bool _prevForceCameraLock;

		// Token: 0x040089DE RID: 35294
		private CursorLockMode _prevCursorLock;

		// Token: 0x040089DF RID: 35295
		private bool _prevCursorVisible;

		// Token: 0x040089E0 RID: 35296
		private bool _gameplayStateCaptured;

		// Token: 0x040089E1 RID: 35297
		private Vector2 _shownPos;

		// Token: 0x040089E2 RID: 35298
		private Vector2 _hiddenPos;

		// Token: 0x040089E3 RID: 35299
		private bool _bound;

		// Token: 0x040089E4 RID: 35300
		[Header("Sorting")]
		[Tooltip("Canvas sortingOrder used while the panel is open, to draw above HUD overlays. The HUD's highest nested Canvas is ~200, so keep this comfortably higher.")]
		[SerializeField]
		private int openSortingOrder = 500;

		// Token: 0x040089E5 RID: 35301
		private Canvas _sortCanvas;

		// Token: 0x040089E6 RID: 35302
		private GraphicRaycaster _sortRaycaster;

		// Token: 0x040089E7 RID: 35303
		private const string GateReason = "InventoryDialog";

		// Token: 0x040089E8 RID: 35304
		private const PlayerActivity GateMask = PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.StanceSwitch | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.ArmsPose;

		// Token: 0x040089E9 RID: 35305
		private const PlayerContextResource DialogResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI;

		// Token: 0x040089EA RID: 35306
		private PlayerActivityLease _dialogLease;

		// Token: 0x040089EB RID: 35307
		private HeadphonesInventorySlotUI _headphoneSlot;

		// Token: 0x040089EC RID: 35308
		private bool _headphonesSelected;
	}
}
