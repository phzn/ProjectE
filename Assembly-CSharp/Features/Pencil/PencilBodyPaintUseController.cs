using System;
using System.Collections;
using Features.Achievements;
using Features.BodyPaintStudio;
using Features.Inventory;
using Features.Localization;
using Features.RunModes;
using Features.UI.TimedEffects;
using Features.WardrobeFeedback;
using Invector.vCharacterController;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.UI;
using UnityEngine;

namespace Features.Pencil
{
	// Token: 0x02000E86 RID: 3718
	[RequireComponent(typeof(PencilBodyPaintPickerUI))]
	public sealed class PencilBodyPaintUseController : MonoBehaviour
	{
		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x06006F8D RID: 28557 RVA: 0x002D0D57 File Offset: 0x002CEF57
		// (set) Token: 0x06006F8E RID: 28558 RVA: 0x002D0D5F File Offset: 0x002CEF5F
		public bool LastApplyFeedbackPlayed { get; private set; }

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x06006F8F RID: 28559 RVA: 0x002D0D68 File Offset: 0x002CEF68
		// (set) Token: 0x06006F90 RID: 28560 RVA: 0x002D0D70 File Offset: 0x002CEF70
		public bool LastApplyAdmireStarted { get; private set; }

		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x06006F91 RID: 28561 RVA: 0x002D0D79 File Offset: 0x002CEF79
		// (set) Token: 0x06006F92 RID: 28562 RVA: 0x002D0D81 File Offset: 0x002CEF81
		public bool LastItemConsumed { get; private set; }

		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x06006F93 RID: 28563 RVA: 0x002D0D8A File Offset: 0x002CEF8A
		public SkinDecal ActiveTimedDecal
		{
			get
			{
				return this._timedDecal;
			}
		}

		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06006F94 RID: 28564 RVA: 0x002D0D92 File Offset: 0x002CEF92
		// (set) Token: 0x06006F95 RID: 28565 RVA: 0x002D0D9A File Offset: 0x002CEF9A
		public float ActiveTimeRemaining { get; private set; }

		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06006F96 RID: 28566 RVA: 0x002D0DA3 File Offset: 0x002CEFA3
		// (set) Token: 0x06006F97 RID: 28567 RVA: 0x002D0DAB File Offset: 0x002CEFAB
		public float CustomPaintTimeRemaining { get; private set; }

		// Token: 0x06006F98 RID: 28568 RVA: 0x002D0DB4 File Offset: 0x002CEFB4
		private void Awake()
		{
			if (this.picker == null)
			{
				this.picker = base.GetComponent<PencilBodyPaintPickerUI>();
			}
			this.picker.ApplyRequested += this.ApplySelection;
			this.picker.CancelRequested += this.ClosePicker;
		}

		// Token: 0x06006F99 RID: 28569 RVA: 0x002D0E09 File Offset: 0x002CF009
		private void Start()
		{
			this.ResolveReferences();
		}

		// Token: 0x06006F9A RID: 28570 RVA: 0x002D0E14 File Offset: 0x002CF014
		private void Update()
		{
			this.ResolveReferences();
			if (this._customPenSuspendedForStudio && (this.customBodyPaintMode == null || !this.customBodyPaintMode.IsActive))
			{
				this.RestoreCustomPenAfterStudio();
			}
			this.AdvanceCustomPaintLifetime(Time.deltaTime);
			if (this.picker != null && this.picker.IsOpen)
			{
				if (!this.IsPencilActive() || EscapeInputCoordinator.TryConsume(this))
				{
					this.ClosePicker();
				}
				return;
			}
			if (this.IsPencilActive())
			{
				if (this.useInput.GetButtonDown() && this.CanOpenPicker())
				{
					this.OpenPicker();
				}
				return;
			}
			if (this.IsCustomBodyPaintPenActive() && this.useInput.GetButtonDown())
			{
				this.TryOpenCustomBodyPaintStudio();
			}
		}

		// Token: 0x06006F9B RID: 28571 RVA: 0x002D0ED0 File Offset: 0x002CF0D0
		private void LateUpdate()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			InteractionConfigSO interactionConfigSO = null;
			if (this.IsPencilActive() && (this.picker == null || !this.picker.IsOpen) && this.catalog != null)
			{
				interactionConfigSO = this.catalog.heldHudConfig;
			}
			else if (this.IsCustomBodyPaintPenActive() && (this.customBodyPaintMode == null || !this.customBodyPaintMode.IsActive))
			{
				interactionConfigSO = this.customBodyPaintHeldHudConfig;
			}
			if (interactionConfigSO == this._shownHud)
			{
				if (interactionConfigSO != null && instance.CurrentConfig == null)
				{
					instance.Show(interactionConfigSO);
				}
				return;
			}
			if (interactionConfigSO == null)
			{
				if (this._shownHud != null && instance.IsShowing)
				{
					instance.Hide();
				}
			}
			else
			{
				instance.Show(interactionConfigSO);
			}
			this._shownHud = interactionConfigSO;
		}

		// Token: 0x06006F9C RID: 28572 RVA: 0x002D0FBC File Offset: 0x002CF1BC
		public void ResetForHomeReturn()
		{
			this.ClosePicker();
			this.HideOwnedHud();
			this.StopCustomPaintTimer();
			if (this._durationRoutine != null)
			{
				base.StopCoroutine(this._durationRoutine);
			}
			this._durationRoutine = null;
			this._timedDecal = null;
			this.ActiveTimeRemaining = 0f;
			TimedEffectHUDService instance = TimedEffectHUDService.Instance;
			if (instance == null)
			{
				return;
			}
			instance.UnregisterEffect("PencilBodyPaint");
		}

		// Token: 0x06006F9D RID: 28573 RVA: 0x002D101C File Offset: 0x002CF21C
		private void OnDisable()
		{
			this.ClosePicker();
			this.HideOwnedHud();
		}

		// Token: 0x06006F9E RID: 28574 RVA: 0x002D102C File Offset: 0x002CF22C
		private void OnDestroy()
		{
			this.ObserveSkinDecalController(null);
			this.ObserveCustomPaintCanvas(null);
			this.StopCustomPaintTimer();
			if (this.picker != null)
			{
				this.picker.ApplyRequested -= this.ApplySelection;
				this.picker.CancelRequested -= this.ClosePicker;
			}
			this.ClosePicker();
			if (TimedEffectHUDService.Instance != null)
			{
				TimedEffectHUDService.Instance.UnregisterEffect("PencilBodyPaint");
			}
		}

		// Token: 0x06006F9F RID: 28575 RVA: 0x002D10AC File Offset: 0x002CF2AC
		private bool IsPencilActive()
		{
			return !(this.catalog == null) && !(this.catalog.pencilItem == null) && !(this.inventory == null) && this.inventory.GetActiveItem() == this.catalog.pencilItem;
		}

		// Token: 0x06006FA0 RID: 28576 RVA: 0x002D1105 File Offset: 0x002CF305
		private bool IsCustomBodyPaintPenActive()
		{
			return !(this.customBodyPaintPenItem == null) && !(this.inventory == null) && this.inventory.GetActiveItem() == this.customBodyPaintPenItem;
		}

		// Token: 0x06006FA1 RID: 28577 RVA: 0x002D113C File Offset: 0x002CF33C
		public bool TryOpenCustomBodyPaintStudio()
		{
			this.ResolveReferences();
			if (!this.IsCustomBodyPaintPenActive())
			{
				return false;
			}
			if (this.customBodyPaintMode == null)
			{
				ToastService.Show("Custom body paint is not available.", "Body Paint", null, -1f, "pencil_vegetables");
				Debug.LogWarning("[Pencil] BodyPaintModeController not found on the player.", this);
				return false;
			}
			this.HideOwnedHud();
			if (!this.customBodyPaintMode.TryEnterMode())
			{
				return false;
			}
			this.inventory.SuspendActiveItemImmediate();
			this._customPenSuspendedForStudio = true;
			bool flag;
			using (WardrobeFeedback.SuppressInventoryRemovalFeedback())
			{
				flag = this.TryCompletePenUse();
			}
			if (!flag)
			{
				this._customPenSuspendedForStudio = false;
				this.customBodyPaintMode.ExitMode();
				this.inventory.ResumeActiveItem();
				Debug.LogWarning("[Pencil] Custom body-paint studio closed because the legendary pen could not be consumed.", this);
				return false;
			}
			if (this.debugLogs)
			{
				Debug.Log("[Pencil] Custom body-paint studio opened; item consumed=" + this.LastItemConsumed.ToString(), this);
			}
			return true;
		}

		// Token: 0x06006FA2 RID: 28578 RVA: 0x002D1234 File Offset: 0x002CF434
		private void RestoreCustomPenAfterStudio()
		{
			this._customPenSuspendedForStudio = false;
			AchievementTrackingService.RecordCustomPaint(this.LastItemConsumed, this._customPaintCanvas != null && this._customPaintCanvas.HasAnyPaint);
			this.StartCustomPaintTimer();
			if (this.inventory != null)
			{
				this.inventory.ResumeActiveItem();
			}
		}

		// Token: 0x06006FA3 RID: 28579 RVA: 0x002D1290 File Offset: 0x002CF490
		private void StartCustomPaintTimer()
		{
			if (this._customPaintCanvas == null || !this._customPaintCanvas.HasAnyPaint)
			{
				return;
			}
			float num = RunModeService.ResolveBodyPaintDuration(Mathf.Max(1f, (this.catalog != null) ? this.catalog.totalDurationSeconds : 180f) * 2f);
			this._customPaintFadeDuration = Mathf.Clamp((this.catalog != null) ? this.catalog.fadeDurationSeconds : 20f, 0f, num);
			this.CustomPaintTimeRemaining = num;
			this._customPaintCanvas.SetLifetimeFade(0f);
			TimedEffectHUDService timedEffectHUDService = PencilBodyPaintUseController.ResolveTimedEffectHud();
			if (timedEffectHUDService != null)
			{
				timedEffectHUDService.RegisterEffect(new TimedEffectData
				{
					EffectId = "CustomBodyPaint",
					DisplayName = "Custom Body Paint",
					Icon = ((this.customBodyPaintPenItem != null) ? this.customBodyPaintPenItem.icon : null),
					TotalDuration = num,
					ExternalTimeControl = true,
					AccentColor = ((this.catalog != null) ? this.catalog.timerAccentColor : new Color(1f, 0.55f, 0.78f))
				});
			}
		}

		// Token: 0x06006FA4 RID: 28580 RVA: 0x002D13D0 File Offset: 0x002CF5D0
		private void AdvanceCustomPaintLifetime(float deltaTime)
		{
			if (this.CustomPaintTimeRemaining <= 0f)
			{
				return;
			}
			if (this._customPaintCanvas == null || !this._customPaintCanvas.HasAnyPaint)
			{
				this.StopCustomPaintTimer();
				return;
			}
			bool flag = GameplayDurationPause.IsPaused || (this.customBodyPaintMode != null && this.customBodyPaintMode.IsActive);
			if (!flag && !float.IsPositiveInfinity(this.CustomPaintTimeRemaining))
			{
				this.CustomPaintTimeRemaining = Mathf.Max(0f, this.CustomPaintTimeRemaining - Mathf.Max(0f, deltaTime));
			}
			TimedEffectHUDService timedEffectHUDService = PencilBodyPaintUseController.ResolveTimedEffectHud();
			if (timedEffectHUDService != null)
			{
				timedEffectHUDService.SetRemainingTime("CustomBodyPaint", this.CustomPaintTimeRemaining);
				timedEffectHUDService.SetPaused("CustomBodyPaint", flag);
			}
			float num = ((this._customPaintFadeDuration > 0f) ? (1f - Mathf.Clamp01(this.CustomPaintTimeRemaining / this._customPaintFadeDuration)) : ((this.CustomPaintTimeRemaining <= 0f) ? 1f : 0f));
			this._customPaintCanvas.SetLifetimeFade(num);
			if (this.CustomPaintTimeRemaining <= 0f)
			{
				this._customPaintCanvas.ClearAllPaint();
				this.StopCustomPaintTimer();
			}
		}

		// Token: 0x06006FA5 RID: 28581 RVA: 0x002D14FD File Offset: 0x002CF6FD
		private void StopCustomPaintTimer()
		{
			this.CustomPaintTimeRemaining = 0f;
			if (TimedEffectHUDService.Instance != null)
			{
				TimedEffectHUDService.Instance.UnregisterEffect("CustomBodyPaint");
			}
		}

		// Token: 0x06006FA6 RID: 28582 RVA: 0x002D1526 File Offset: 0x002CF726
		private void HandleCustomPaintStateChanged(bool hasPaint)
		{
			if (!hasPaint)
			{
				this.StopCustomPaintTimer();
			}
		}

		// Token: 0x06006FA7 RID: 28583 RVA: 0x002D1534 File Offset: 0x002CF734
		private void ObserveCustomPaintCanvas(BodyPaintCanvasService canvas)
		{
			if (this._customPaintCanvas == canvas)
			{
				return;
			}
			if (this._customPaintCanvas != null)
			{
				this._customPaintCanvas.OnPaintStateChanged -= this.HandleCustomPaintStateChanged;
			}
			this._customPaintCanvas = canvas;
			if (this._customPaintCanvas != null)
			{
				this._customPaintCanvas.OnPaintStateChanged += this.HandleCustomPaintStateChanged;
			}
		}

		// Token: 0x06006FA8 RID: 28584 RVA: 0x002D15A4 File Offset: 0x002CF7A4
		private bool CanOpenPicker()
		{
			return !(this.catalog == null) && this.catalog.options != null && this.catalog.options.Count != 0 && PlayerActivityGate.Allows(PlayerActivity.Inventory) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem);
		}

		// Token: 0x06006FA9 RID: 28585 RVA: 0x002D15F8 File Offset: 0x002CF7F8
		public void OpenPicker()
		{
			this.ResolveReferences();
			if (this.picker == null || !this.IsPencilActive() || !this.CanOpenPicker())
			{
				return;
			}
			if (this.skinDecalController == null)
			{
				ToastService.Show("Body paint system is not available.", "Pencil", null, -1f, "pencil_vegetables");
				Debug.LogWarning("[Pencil] SkinDecalController not found on the player.", this);
				return;
			}
			SkinDecal skinDecal = null;
			for (int i = 0; i < this.catalog.options.Count; i++)
			{
				PencilBodyPaintCatalogSO.Option option = this.catalog.options[i];
				if (option != null && option.decal != null && this.skinDecalController.HasDecal(option.decal))
				{
					skinDecal = option.decal;
					break;
				}
			}
			if (!this.TryAcquirePickerContext())
			{
				return;
			}
			if (!base.isActiveAndEnabled || this._pickerLease == null || !this._pickerLease.IsActive)
			{
				this.ReleasePickerContext();
				return;
			}
			try
			{
				this.AcquireGameplayLock();
				this.HideOwnedHud();
				this.picker.Show(this.catalog, skinDecal);
				if (this.debugLogs)
				{
					Debug.Log("[Pencil] Body-paint picker opened.", this);
				}
			}
			catch
			{
				this.ClosePicker();
				throw;
			}
		}

		// Token: 0x06006FAA RID: 28586 RVA: 0x002D1734 File Offset: 0x002CF934
		public void ClosePicker()
		{
			if (this.picker != null)
			{
				this.picker.HideImmediate();
			}
			this.ReleaseGameplayLock();
			if (this.debugLogs)
			{
				Debug.Log("[Pencil] Body-paint picker closed.", this);
			}
		}

		// Token: 0x06006FAB RID: 28587 RVA: 0x002D1768 File Offset: 0x002CF968
		private void ApplySelection(SkinDecal selected)
		{
			this.TryApplyBodyPaint(selected);
		}

		// Token: 0x06006FAC RID: 28588 RVA: 0x002D1774 File Offset: 0x002CF974
		public bool TryApplyBodyPaint(SkinDecal selected)
		{
			this.ResolveReferences();
			if (selected == null || this.skinDecalController == null || this.catalog == null || this.catalog.options == null)
			{
				return false;
			}
			if (!this.IsPencilActive())
			{
				Debug.LogWarning("[Pencil] A body paint can only be applied while a Pencil unit is active.", this);
				return false;
			}
			PencilBodyPaintCatalogSO.Option option = null;
			for (int i = 0; i < this.catalog.options.Count; i++)
			{
				PencilBodyPaintCatalogSO.Option option2 = this.catalog.options[i];
				if (option2 != null && option2.decal == selected)
				{
					option = option2;
					break;
				}
			}
			if (option == null)
			{
				Debug.LogWarning("[Pencil] Refused a body paint that is not present in the Pencil catalog.", this);
				return false;
			}
			for (int j = 0; j < this.catalog.options.Count; j++)
			{
				PencilBodyPaintCatalogSO.Option option3 = this.catalog.options[j];
				if (option3 != null && !(option3.decal == null) && !(option3.decal == selected) && this.skinDecalController.HasDecal(option3.decal))
				{
					this.skinDecalController.RemoveDecal(option3.decal);
				}
			}
			this.skinDecalController.AddDecal(selected, null, null);
			bool flag = this.skinDecalController.HasDecal(selected);
			if (!flag)
			{
				return false;
			}
			this.StartTimedPaint(selected, option.DisplayName);
			this.LastApplyFeedbackPlayed = WardrobeFeedback.PlayCosmeticApply(new Color(1f, 0.55f, 0.78f, 1f));
			this.LastApplyAdmireStarted = WearAdmireAnimation.PlayCosmeticAdmire();
			string appliedName = option.DisplayName;
			float appliedDuration = this.ActiveTimeRemaining;
			ToastService.ShowComputed(() => GameLocalization.FormatSource("pencil_vegetables", float.IsPositiveInfinity(appliedDuration) ? "{0} applied without a time limit." : "{0} applied for {1}.", new object[]
			{
				GameLocalization.GetSource("timed_effects", appliedName),
				float.IsPositiveInfinity(appliedDuration) ? "" : string.Format("{0}:{1:D2}", Mathf.FloorToInt(appliedDuration / 60f), Mathf.CeilToInt(appliedDuration % 60f))
			}), "Body Paint", -1f, "pencil_vegetables", null);
			this.ClosePicker();
			bool flag2 = this.TryCompletePenUse();
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[Pencil] Applied body paint: ",
					selected.name,
					" | wear feedback=",
					this.LastApplyFeedbackPlayed.ToString(),
					" | admire animation=",
					this.LastApplyAdmireStarted.ToString(),
					" | item consumed=",
					this.LastItemConsumed.ToString()
				}), this);
			}
			return flag && flag2;
		}

		// Token: 0x06006FAD RID: 28589 RVA: 0x002D19E0 File Offset: 0x002CFBE0
		private bool TryCompletePenUse()
		{
			this.LastItemConsumed = false;
			InventoryItemSO inventoryItemSO = ((this.inventory != null) ? this.inventory.GetActiveSlotItem() : null);
			if (inventoryItemSO == null || (inventoryItemSO != this.customBodyPaintPenItem && (this.catalog == null || inventoryItemSO != this.catalog.pencilItem)))
			{
				return false;
			}
			if (RunModeService.UnlimitedBodyPaintPenUses)
			{
				return true;
			}
			this.LastItemConsumed = this.inventory.ConsumeActiveItem();
			return this.LastItemConsumed;
		}

		// Token: 0x06006FAE RID: 28590 RVA: 0x002D1A6C File Offset: 0x002CFC6C
		private void StartTimedPaint(SkinDecal decal, string displayName)
		{
			if (this._durationRoutine != null)
			{
				base.StopCoroutine(this._durationRoutine);
			}
			this._durationRoutine = null;
			this._timedDecal = decal;
			float num = RunModeService.ResolveBodyPaintDuration(Mathf.Max(1f, this.catalog.totalDurationSeconds));
			float num2 = Mathf.Clamp(this.catalog.fadeDurationSeconds, 0f, num);
			this.ActiveTimeRemaining = num;
			this.skinDecalController.SetDecalOpacity(decal, 1f);
			TimedEffectHUDService timedEffectHUDService = PencilBodyPaintUseController.ResolveTimedEffectHud();
			if (timedEffectHUDService != null)
			{
				timedEffectHUDService.RegisterEffect(new TimedEffectData
				{
					EffectId = "PencilBodyPaint",
					DisplayName = displayName,
					Icon = ((this.catalog.pencilItem != null) ? this.catalog.pencilItem.icon : null),
					TotalDuration = num,
					ExternalTimeControl = true,
					AccentColor = this.catalog.timerAccentColor
				});
			}
			if (!float.IsPositiveInfinity(num))
			{
				this._durationRoutine = base.StartCoroutine(this.TimedPaintRoutine(decal, num, num2));
			}
		}

		// Token: 0x06006FAF RID: 28591 RVA: 0x002D1B79 File Offset: 0x002CFD79
		private IEnumerator TimedPaintRoutine(SkinDecal decal, float total, float fadeDuration)
		{
			float elapsed = 0f;
			float holdDuration = Mathf.Max(0f, total - fadeDuration);
			while (elapsed < holdDuration)
			{
				elapsed += Time.deltaTime;
				this.ActiveTimeRemaining = Mathf.Max(0f, total - elapsed);
				this.SyncHudRemainingTime();
				yield return null;
			}
			float fadeElapsed = 0f;
			while (fadeElapsed < fadeDuration && this.skinDecalController != null && decal != null)
			{
				fadeElapsed += Time.deltaTime;
				elapsed = holdDuration + fadeElapsed;
				this.ActiveTimeRemaining = Mathf.Max(0f, total - elapsed);
				this.SyncHudRemainingTime();
				float num = ((fadeDuration > 0f) ? Mathf.Clamp01(fadeElapsed / fadeDuration) : 1f);
				this.skinDecalController.SetDecalOpacity(decal, 1f - num);
				yield return null;
			}
			if (this._timedDecal == decal)
			{
				this._timedDecal = null;
			}
			this.ActiveTimeRemaining = 0f;
			this.SyncHudRemainingTime();
			if (TimedEffectHUDService.Instance != null)
			{
				TimedEffectHUDService.Instance.UnregisterEffect("PencilBodyPaint");
			}
			this._durationRoutine = null;
			if (this.skinDecalController != null && decal != null)
			{
				this.skinDecalController.RemoveDecal(decal);
			}
			yield break;
		}

		// Token: 0x06006FB0 RID: 28592 RVA: 0x002D1BA0 File Offset: 0x002CFDA0
		private void HandleDecalsChanged()
		{
			if (this._timedDecal == null || this.skinDecalController == null || this.skinDecalController.HasDecal(this._timedDecal))
			{
				return;
			}
			Coroutine durationRoutine = this._durationRoutine;
			this._durationRoutine = null;
			this._timedDecal = null;
			this.ActiveTimeRemaining = 0f;
			if (durationRoutine != null)
			{
				base.StopCoroutine(durationRoutine);
			}
			if (TimedEffectHUDService.Instance != null)
			{
				TimedEffectHUDService.Instance.UnregisterEffect("PencilBodyPaint");
			}
		}

		// Token: 0x06006FB1 RID: 28593 RVA: 0x002D1C24 File Offset: 0x002CFE24
		private void SyncHudRemainingTime()
		{
			TimedEffectHUDService timedEffectHUDService = PencilBodyPaintUseController.ResolveTimedEffectHud();
			if (timedEffectHUDService != null)
			{
				timedEffectHUDService.SetRemainingTime("PencilBodyPaint", this.ActiveTimeRemaining);
			}
		}

		// Token: 0x06006FB2 RID: 28594 RVA: 0x002D1C54 File Offset: 0x002CFE54
		private static TimedEffectHUDService ResolveTimedEffectHud()
		{
			TimedEffectHUDService timedEffectHUDService = TimedEffectHUDService.Instance ?? Object.FindObjectOfType<TimedEffectHUDService>(true);
			if (timedEffectHUDService != null && !timedEffectHUDService.enabled)
			{
				timedEffectHUDService.enabled = true;
			}
			return timedEffectHUDService;
		}

		// Token: 0x06006FB3 RID: 28595 RVA: 0x002D1C8C File Offset: 0x002CFE8C
		private void ResolveReferences()
		{
			if (this.inventory == null)
			{
				this.inventory = PlayerInventoryManager.Instance ?? base.GetComponentInParent<PlayerInventoryManager>();
			}
			Transform transform = ((this.inventory != null) ? this.inventory.transform.root : base.transform.root);
			if (this.playerInput == null)
			{
				this.playerInput = transform.GetComponentInChildren<vThirdPersonInput>(true);
			}
			if (this.skinDecalController == null)
			{
				this.skinDecalController = transform.GetComponentInChildren<SkinDecalController>(true);
			}
			if (this.customBodyPaintMode == null)
			{
				this.customBodyPaintMode = transform.GetComponentInChildren<BodyPaintModeController>(true);
			}
			this.ObserveSkinDecalController(this.skinDecalController);
			if (this._customPaintCanvas == null)
			{
				this.ObserveCustomPaintCanvas(transform.GetComponentInChildren<BodyPaintCanvasService>(true));
			}
		}

		// Token: 0x06006FB4 RID: 28596 RVA: 0x002D1D60 File Offset: 0x002CFF60
		private void ObserveSkinDecalController(SkinDecalController controller)
		{
			if (this._observedSkinDecalController == controller)
			{
				return;
			}
			if (this._observedSkinDecalController != null)
			{
				this._observedSkinDecalController.OnDecalsChanged -= this.HandleDecalsChanged;
			}
			this._observedSkinDecalController = controller;
			if (this._observedSkinDecalController != null)
			{
				this._observedSkinDecalController.OnDecalsChanged += this.HandleDecalsChanged;
			}
		}

		// Token: 0x06006FB5 RID: 28597 RVA: 0x002D1DD0 File Offset: 0x002CFFD0
		private void AcquireGameplayLock()
		{
			if (this._locked)
			{
				return;
			}
			this._locked = true;
			if (this.playerInput != null)
			{
				this._previousInputLock = this.playerInput.lockInput;
				this._previousCameraLock = this.playerInput.lockCameraInput;
				this.playerInput.SetLockAllInput(true);
				this.playerInput.SetLockCameraInput(true);
				if (this.playerInput.cc != null)
				{
					this.playerInput.cc.input = Vector2.zero;
					this.playerInput.cc.isSprinting = false;
				}
			}
			this._previousCursorLock = Cursor.lockState;
			this._previousCursorVisible = Cursor.visible;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		// Token: 0x06006FB6 RID: 28598 RVA: 0x002D1E98 File Offset: 0x002D0098
		private void ReleaseGameplayLock()
		{
			if (!this._locked)
			{
				this.ReleasePickerContext();
				return;
			}
			this._locked = false;
			if (this.playerInput != null)
			{
				this.playerInput.SetLockAllInput(this._previousInputLock);
				this.playerInput.SetLockCameraInput(this._previousCameraLock);
			}
			Cursor.lockState = this._previousCursorLock;
			Cursor.visible = this._previousCursorVisible;
			this.ReleasePickerContext();
		}

		// Token: 0x06006FB7 RID: 28599 RVA: 0x002D1F08 File Offset: 0x002D0108
		private bool TryAcquirePickerContext()
		{
			if (this._pickerLease != null)
			{
				if (this._pickerLease.IsActive)
				{
					return true;
				}
				this._pickerLease = null;
			}
			return PlayerActivityGate.TryAcquire("PencilBodyPaintPicker#" + base.GetInstanceID().ToString(), this, PlayerActivity.Inventory, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._pickerLease, false, null);
		}

		// Token: 0x06006FB8 RID: 28600 RVA: 0x002D1F65 File Offset: 0x002D0165
		private void ReleasePickerContext()
		{
			if (this._pickerLease == null)
			{
				return;
			}
			this._pickerLease.Dispose();
			this._pickerLease = null;
		}

		// Token: 0x06006FB9 RID: 28601 RVA: 0x002D1F84 File Offset: 0x002D0184
		private void HideOwnedHud()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (this._shownHud != null && instance != null && instance.IsShowing)
			{
				instance.Hide();
			}
			this._shownHud = null;
		}

		// Token: 0x0400726F RID: 29295
		private const string GateReason = "PencilBodyPaintPicker";

		// Token: 0x04007270 RID: 29296
		private const string TimerEffectId = "PencilBodyPaint";

		// Token: 0x04007271 RID: 29297
		private const string CustomTimerEffectId = "CustomBodyPaint";

		// Token: 0x04007272 RID: 29298
		private const PlayerContextResource PickerResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem;

		// Token: 0x04007273 RID: 29299
		[Header("Feature Data")]
		[SerializeField]
		private PencilBodyPaintCatalogSO catalog;

		// Token: 0x04007274 RID: 29300
		[Header("Legendary Custom Body Paint Pen")]
		[SerializeField]
		private InventoryItemSO customBodyPaintPenItem;

		// Token: 0x04007275 RID: 29301
		[SerializeField]
		private InteractionConfigSO customBodyPaintHeldHudConfig;

		// Token: 0x04007276 RID: 29302
		[SerializeField]
		private BodyPaintModeController customBodyPaintMode;

		// Token: 0x04007277 RID: 29303
		[Header("Input")]
		[SerializeField]
		private GenericInput useInput = new GenericInput("F", "X", "X");

		// Token: 0x04007278 RID: 29304
		[Header("References (auto-resolved)")]
		[SerializeField]
		private PencilBodyPaintPickerUI picker;

		// Token: 0x04007279 RID: 29305
		[SerializeField]
		private PlayerInventoryManager inventory;

		// Token: 0x0400727A RID: 29306
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x0400727B RID: 29307
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x0400727C RID: 29308
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04007282 RID: 29314
		private InteractionConfigSO _shownHud;

		// Token: 0x04007283 RID: 29315
		private bool _locked;

		// Token: 0x04007284 RID: 29316
		private bool _previousInputLock;

		// Token: 0x04007285 RID: 29317
		private bool _previousCameraLock;

		// Token: 0x04007286 RID: 29318
		private CursorLockMode _previousCursorLock;

		// Token: 0x04007287 RID: 29319
		private bool _previousCursorVisible;

		// Token: 0x04007288 RID: 29320
		private Coroutine _durationRoutine;

		// Token: 0x04007289 RID: 29321
		private SkinDecal _timedDecal;

		// Token: 0x0400728A RID: 29322
		private SkinDecalController _observedSkinDecalController;

		// Token: 0x0400728B RID: 29323
		private bool _customPenSuspendedForStudio;

		// Token: 0x0400728C RID: 29324
		private PlayerActivityLease _pickerLease;

		// Token: 0x0400728D RID: 29325
		private BodyPaintCanvasService _customPaintCanvas;

		// Token: 0x0400728E RID: 29326
		private float _customPaintFadeDuration;
	}
}
