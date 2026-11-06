using System;
using Features.BodyPaintStudio;
using Features.RunModes;
using ProjectExhibitionist;
using UnityEngine;
using UnityEngine.AzureSky;

namespace Features.BodyPaintRain
{
	// Token: 0x02001525 RID: 5413
	[DisallowMultipleComponent]
	public sealed class BodyPaintRainWashController : MonoBehaviour
	{
		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x0600A768 RID: 42856 RVA: 0x0045FED8 File Offset: 0x0045E0D8
		public float WashProgress
		{
			get
			{
				return Mathf.Max(this._washProgress, (this.bodyPaintCanvasService != null) ? this.bodyPaintCanvasService.RainFadeAmount : 0f);
			}
		}

		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x0600A769 RID: 42857 RVA: 0x0045FF05 File Offset: 0x0045E105
		public bool IsRaining
		{
			get
			{
				return (this._rainOverrideEnabled ? this._rainOverrideValue : this._isRaining) && !RainShelterRuntime.IsShelteredAt(base.transform.position);
			}
		}

		// Token: 0x170019CA RID: 6602
		// (get) Token: 0x0600A76A RID: 42858 RVA: 0x0045FF34 File Offset: 0x0045E134
		public bool IsTrackingPaint
		{
			get
			{
				return this._trackingPaint || (this.bodyPaintCanvasService != null && this.bodyPaintCanvasService.HasAnyPaint && !this.bodyPaintCanvasService.IsApplyingPaint);
			}
		}

		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x0600A76B RID: 42859 RVA: 0x0045FF6B File Offset: 0x0045E16B
		public bool HasRainOverride
		{
			get
			{
				return this._rainOverrideEnabled;
			}
		}

		// Token: 0x170019CC RID: 6604
		// (get) Token: 0x0600A76C RID: 42860 RVA: 0x0045FF73 File Offset: 0x0045E173
		public bool OwnsRainSweatDecal
		{
			get
			{
				return this._ownsRainSweatDecal;
			}
		}

		// Token: 0x170019CD RID: 6605
		// (get) Token: 0x0600A76D RID: 42861 RVA: 0x0045FF7B File Offset: 0x0045E17B
		public float RainSweatOpacity
		{
			get
			{
				return this._rainSweatOpacity;
			}
		}

		// Token: 0x0600A76E RID: 42862 RVA: 0x0045FF83 File Offset: 0x0045E183
		private void Awake()
		{
			this._wetOwner = string.Format("BodyPaintRainWashController:{0}", base.GetInstanceID());
			this.ResolvePaintTargets(true, 0f);
			this._weatherPollRemaining = 0f;
		}

		// Token: 0x0600A76F RID: 42863 RVA: 0x0045FFB7 File Offset: 0x0045E1B7
		private void OnEnable()
		{
			this._referenceRetryRemaining = 0f;
			this._weatherPollRemaining = 0f;
		}

		// Token: 0x0600A770 RID: 42864 RVA: 0x0045FFCF File Offset: 0x0045E1CF
		private void Update()
		{
			this.AdvanceInternal(Time.deltaTime, Time.unscaledDeltaTime, false);
		}

		// Token: 0x0600A771 RID: 42865 RVA: 0x0045FFE2 File Offset: 0x0045E1E2
		public void SetRainOverride(bool isRaining)
		{
			this._rainOverrideEnabled = true;
			this._rainOverrideValue = isRaining;
			this.SetCachedRainState(isRaining, "override");
		}

		// Token: 0x0600A772 RID: 42866 RVA: 0x0045FFFE File Offset: 0x0045E1FE
		public void ClearRainOverride()
		{
			this._rainOverrideEnabled = false;
			this._weatherPollRemaining = 0f;
		}

		// Token: 0x0600A773 RID: 42867 RVA: 0x00460012 File Offset: 0x0045E212
		public void AdvanceForTests(float deltaTime)
		{
			this.AdvanceInternal(deltaTime, deltaTime, true);
		}

		// Token: 0x0600A774 RID: 42868 RVA: 0x00460020 File Offset: 0x0045E220
		private void AdvanceInternal(float deltaTime, float visualDeltaTime, bool forceReferenceResolve)
		{
			float num = BodyPaintRainWashController.SanitizeDeltaTime(deltaTime);
			float num2 = BodyPaintRainWashController.SanitizeDeltaTime(visualDeltaTime);
			this.ResolvePaintTargets(forceReferenceResolve, num);
			bool flag = this.EvaluateRainState(num) && !RainShelterRuntime.IsShelteredAt(base.transform.position);
			this.SetRainWet(flag);
			this.UpdateRainSweat(flag, num2);
			flag &= RunModeService.BodyPaintRainWashEnabled;
			bool flag2 = this.skinDecalController != null && this.skinDecalController.HasRainWashableDecals();
			this.AdvanceCustomPaintWash(flag, num);
			if (!flag2)
			{
				if (this._trackingPaint)
				{
					this.ResetWashSession(true);
				}
				return;
			}
			if (!this._trackingPaint)
			{
				this._trackingPaint = true;
				this._washProgress = 0f;
			}
			if (flag && num > 0f)
			{
				this._washProgress = ((this.washDurationSeconds <= 0f) ? 1f : Mathf.Clamp01(this._washProgress + num / this.washDurationSeconds));
			}
			this.skinDecalController.SetRainFade(this._washProgress);
			if (this._washProgress >= 1f)
			{
				this.CompleteWash();
			}
		}

		// Token: 0x0600A775 RID: 42869 RVA: 0x00460128 File Offset: 0x0045E328
		private void AdvanceCustomPaintWash(bool raining, float step)
		{
			if (this.bodyPaintCanvasService == null || !this.bodyPaintCanvasService.HasAnyPaint || this.bodyPaintCanvasService.IsApplyingPaint || !raining || step <= 0f)
			{
				return;
			}
			float num = ((this.washDurationSeconds <= 0f) ? 1f : Mathf.Clamp01(this.bodyPaintCanvasService.RainFadeAmount + step / this.washDurationSeconds));
			this.bodyPaintCanvasService.SetRainFade(num);
			if (num >= 1f)
			{
				this.bodyPaintCanvasService.ClearAllPaint();
			}
		}

		// Token: 0x0600A776 RID: 42870 RVA: 0x004601B8 File Offset: 0x0045E3B8
		private bool EvaluateRainState(float deltaTime)
		{
			if (this._rainOverrideEnabled)
			{
				this.SetCachedRainState(this._rainOverrideValue, "override");
				return this._rainOverrideValue;
			}
			this._weatherPollRemaining -= deltaTime;
			if (this._weatherPollRemaining > 0f)
			{
				return this._isRaining;
			}
			this._weatherPollRemaining = Mathf.Max(0.05f, this.weatherPollInterval);
			if (this.azureSystem == null)
			{
				this.azureSystem = Object.FindObjectOfType<AzureCoreSystem>();
			}
			AzureWeatherSystem azureWeatherSystem = ((this.azureSystem != null) ? this.azureSystem.weatherSystem : null);
			AzureWeatherPreset azureWeatherPreset = null;
			if (azureWeatherSystem != null)
			{
				azureWeatherPreset = ((azureWeatherSystem.isWeatherChanging && azureWeatherSystem.targetWeatherPreset != null) ? azureWeatherSystem.targetWeatherPreset : azureWeatherSystem.currentWeatherPreset);
			}
			bool flag = azureWeatherPreset != null && this.IsRainyPreset(azureWeatherPreset.name);
			this.SetCachedRainState(flag, (azureWeatherPreset != null) ? azureWeatherPreset.name : "none");
			return flag;
		}

		// Token: 0x0600A777 RID: 42871 RVA: 0x004602B4 File Offset: 0x0045E4B4
		private bool IsRainyPreset(string presetName)
		{
			if (string.IsNullOrEmpty(presetName) || this.rainyPresetTokens == null)
			{
				return false;
			}
			for (int i = 0; i < this.rainyPresetTokens.Length; i++)
			{
				string text = this.rainyPresetTokens[i];
				if (!string.IsNullOrEmpty(text) && presetName.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A778 RID: 42872 RVA: 0x00460305 File Offset: 0x0045E505
		private void SetCachedRainState(bool value, string source)
		{
			if (this._isRaining == value)
			{
				return;
			}
			this._isRaining = value;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[BodyPaintRainWashController] Rain={0} ({1}).", value, source), this);
			}
		}

		// Token: 0x0600A779 RID: 42873 RVA: 0x00460338 File Offset: 0x0045E538
		private void SetRainWet(bool raining)
		{
			if (!raining)
			{
				this.ReleaseRainWet();
				return;
			}
			if (this._wetHeld || this.wetController == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this._wetOwner))
			{
				this._wetOwner = string.Format("BodyPaintRainWashController:{0}", base.GetInstanceID());
			}
			this.wetController.AcquireWet(this._wetOwner);
			this._wetHeld = true;
		}

		// Token: 0x0600A77A RID: 42874 RVA: 0x004603A6 File Offset: 0x0045E5A6
		private void ReleaseRainWet()
		{
			if (!this._wetHeld)
			{
				return;
			}
			if (this.wetController != null)
			{
				this.wetController.ReleaseWet(this._wetOwner);
			}
			this._wetHeld = false;
		}

		// Token: 0x0600A77B RID: 42875 RVA: 0x004603D8 File Offset: 0x0045E5D8
		private void UpdateRainSweat(bool raining, float deltaTime)
		{
			if (this.skinDecalController == null || this.rainSweatDecal == null)
			{
				return;
			}
			if (raining)
			{
				if (!this.TryAcquireRainSweatDecal())
				{
					return;
				}
				float num = ((this.rainSweatFadeInDuration <= 0f) ? 1f : (deltaTime / this.rainSweatFadeInDuration));
				this.SetOwnedRainSweatOpacity(Mathf.MoveTowards(this._rainSweatOpacity, this.rainSweatPeakOpacity, num));
				return;
			}
			else
			{
				if (!this._ownsRainSweatDecal)
				{
					return;
				}
				float num2 = ((this.rainSweatFadeOutDuration <= 0f) ? 1f : (deltaTime / this.rainSweatFadeOutDuration));
				this.SetOwnedRainSweatOpacity(Mathf.MoveTowards(this._rainSweatOpacity, 0f, num2));
				if (Mathf.Approximately(this._rainSweatOpacity, 0f))
				{
					this.RestoreRainSweatState();
				}
				return;
			}
		}

		// Token: 0x0600A77C RID: 42876 RVA: 0x0046049C File Offset: 0x0045E69C
		private bool TryAcquireRainSweatDecal()
		{
			if (this._ownsRainSweatDecal)
			{
				if (!this.skinDecalController.HasDecal(this.rainSweatDecal))
				{
					this.skinDecalController.AddDecal(this.rainSweatDecal, null, new float?(this._rainSweatOpacity));
				}
				this.skinDecalController.SetDecalActive(this.rainSweatDecal, true);
				return true;
			}
			this._rainSweatWasPresent = this.skinDecalController.TryGetDecalState(this.rainSweatDecal, out this._rainSweatPreviousActive, out this._rainSweatPreviousOpacity, out this._rainSweatPreviousTint);
			if (this._rainSweatWasPresent && this._rainSweatPreviousActive)
			{
				this.ClearRainSweatSnapshot();
				return false;
			}
			if (this._rainSweatWasPresent)
			{
				this.skinDecalController.SetDecalOpacity(this.rainSweatDecal, 0f);
				this.skinDecalController.SetDecalActive(this.rainSweatDecal, true);
			}
			else
			{
				this.skinDecalController.AddDecal(this.rainSweatDecal, null, new float?(0f));
				this.skinDecalController.SetDecalActive(this.rainSweatDecal, true);
			}
			this._ownsRainSweatDecal = true;
			this._rainSweatOpacity = 0f;
			return true;
		}

		// Token: 0x0600A77D RID: 42877 RVA: 0x004605BC File Offset: 0x0045E7BC
		private void SetOwnedRainSweatOpacity(float opacity)
		{
			if (!this._ownsRainSweatDecal || this.skinDecalController == null || this.rainSweatDecal == null)
			{
				return;
			}
			this._rainSweatOpacity = Mathf.Clamp01(opacity);
			if (!this.skinDecalController.HasDecal(this.rainSweatDecal))
			{
				this.skinDecalController.AddDecal(this.rainSweatDecal, null, new float?(this._rainSweatOpacity));
			}
			this.skinDecalController.SetDecalActive(this.rainSweatDecal, true);
			this.skinDecalController.SetDecalOpacity(this.rainSweatDecal, this._rainSweatOpacity);
		}

		// Token: 0x0600A77E RID: 42878 RVA: 0x0046065C File Offset: 0x0045E85C
		private void RestoreRainSweatState()
		{
			if (this._ownsRainSweatDecal && this.skinDecalController != null && this.rainSweatDecal != null && this.skinDecalController.HasDecal(this.rainSweatDecal))
			{
				if (this._rainSweatWasPresent)
				{
					this.skinDecalController.SetDecalTint(this.rainSweatDecal, this._rainSweatPreviousTint);
					this.skinDecalController.SetDecalOpacity(this.rainSweatDecal, this._rainSweatPreviousOpacity);
					this.skinDecalController.SetDecalActive(this.rainSweatDecal, this._rainSweatPreviousActive);
				}
				else
				{
					this.skinDecalController.RemoveDecal(this.rainSweatDecal);
				}
			}
			this._ownsRainSweatDecal = false;
			this._rainSweatOpacity = 0f;
			this.ClearRainSweatSnapshot();
		}

		// Token: 0x0600A77F RID: 42879 RVA: 0x0046071E File Offset: 0x0045E91E
		private void ClearRainSweatSnapshot()
		{
			this._rainSweatWasPresent = false;
			this._rainSweatPreviousActive = false;
			this._rainSweatPreviousOpacity = 0f;
			this._rainSweatPreviousTint = Color.white;
		}

		// Token: 0x0600A780 RID: 42880 RVA: 0x00460744 File Offset: 0x0045E944
		private void CompleteWash()
		{
			if (this.debugLogs)
			{
				Debug.Log("[BodyPaintRainWashController] Rain wash completed; removing paint.", this);
			}
			if (this.skinDecalController != null)
			{
				this.skinDecalController.RemoveRainWashableDecals();
			}
			this.ResetWashSession(true);
		}

		// Token: 0x0600A781 RID: 42881 RVA: 0x0046077A File Offset: 0x0045E97A
		private void ResetWashSession(bool resetReceivers)
		{
			this._washProgress = 0f;
			this._trackingPaint = false;
			if (!resetReceivers)
			{
				return;
			}
			if (this.skinDecalController != null)
			{
				this.skinDecalController.SetRainFade(0f);
			}
		}

		// Token: 0x0600A782 RID: 42882 RVA: 0x004607B0 File Offset: 0x0045E9B0
		private void ResolvePaintTargets(bool force, float deltaTime)
		{
			if (this.skinDecalController != null && this.bodyPaintCanvasService != null && this.wetController != null)
			{
				return;
			}
			this._referenceRetryRemaining -= deltaTime;
			if (!force && this._referenceRetryRemaining > 0f)
			{
				return;
			}
			this._referenceRetryRemaining = Mathf.Max(0.05f, this.referenceRetryInterval);
			if (this.skinDecalController == null)
			{
				this.skinDecalController = base.GetComponentInParent<SkinDecalController>();
				if (this.skinDecalController == null)
				{
					this.skinDecalController = base.GetComponentInChildren<SkinDecalController>(true);
				}
				if (this.skinDecalController == null)
				{
					this.skinDecalController = Object.FindObjectOfType<SkinDecalController>();
				}
			}
			if (this.bodyPaintCanvasService == null)
			{
				this.bodyPaintCanvasService = base.GetComponentInParent<BodyPaintCanvasService>();
				if (this.bodyPaintCanvasService == null)
				{
					this.bodyPaintCanvasService = base.GetComponentInChildren<BodyPaintCanvasService>(true);
				}
				if (this.bodyPaintCanvasService == null && this.skinDecalController != null)
				{
					this.bodyPaintCanvasService = this.skinDecalController.GetComponentInChildren<BodyPaintCanvasService>(true);
					if (this.bodyPaintCanvasService == null)
					{
						this.bodyPaintCanvasService = this.skinDecalController.GetComponentInParent<BodyPaintCanvasService>();
					}
				}
				if (this.bodyPaintCanvasService == null)
				{
					this.bodyPaintCanvasService = Object.FindObjectOfType<BodyPaintCanvasService>();
				}
			}
			if (this.wetController == null)
			{
				this.wetController = base.GetComponentInParent<CielWetController>();
				if (this.wetController == null)
				{
					this.wetController = base.GetComponentInChildren<CielWetController>(true);
				}
				if (this.wetController == null && this.skinDecalController != null)
				{
					this.wetController = this.skinDecalController.GetComponentInChildren<CielWetController>(true);
					if (this.wetController == null)
					{
						this.wetController = this.skinDecalController.GetComponentInParent<CielWetController>();
					}
				}
				if (this.wetController == null)
				{
					this.wetController = Object.FindObjectOfType<CielWetController>();
				}
			}
		}

		// Token: 0x0600A783 RID: 42883 RVA: 0x004609A7 File Offset: 0x0045EBA7
		private static float SanitizeDeltaTime(float deltaTime)
		{
			if (float.IsNaN(deltaTime) || float.IsInfinity(deltaTime) || deltaTime <= 0f)
			{
				return 0f;
			}
			return deltaTime;
		}

		// Token: 0x0600A784 RID: 42884 RVA: 0x004609C8 File Offset: 0x0045EBC8
		private void OnValidate()
		{
			this.washDurationSeconds = Mathf.Max(0f, this.washDurationSeconds);
			this.weatherPollInterval = Mathf.Max(0.05f, this.weatherPollInterval);
			this.referenceRetryInterval = Mathf.Max(0.05f, this.referenceRetryInterval);
			this.rainSweatPeakOpacity = Mathf.Clamp01(this.rainSweatPeakOpacity);
			this.rainSweatFadeInDuration = Mathf.Max(0f, this.rainSweatFadeInDuration);
			this.rainSweatFadeOutDuration = Mathf.Max(0f, this.rainSweatFadeOutDuration);
		}

		// Token: 0x0600A785 RID: 42885 RVA: 0x00460A54 File Offset: 0x0045EC54
		public void ResetForHomeReturn()
		{
			this.ReleaseRainWet();
			this.RestoreRainSweatState();
			this.ResetWashSession(true);
			this._isRaining = false;
			this._rainOverrideEnabled = false;
			this._weatherPollRemaining = 0f;
		}

		// Token: 0x0600A786 RID: 42886 RVA: 0x00460A82 File Offset: 0x0045EC82
		private void OnDisable()
		{
			this.ReleaseRainWet();
			this.RestoreRainSweatState();
		}

		// Token: 0x0400AC25 RID: 44069
		[Header("References")]
		[Tooltip("Skin decal controller to wash. Resolved automatically when left empty.")]
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x0400AC26 RID: 44070
		[Tooltip("Custom body-paint canvas to wash. Resolved automatically when left empty.")]
		[SerializeField]
		private BodyPaintCanvasService bodyPaintCanvasService;

		// Token: 0x0400AC27 RID: 44071
		[Tooltip("Azure sky system. It is resolved lazily because the core scene can load additively.")]
		[SerializeField]
		private AzureCoreSystem azureSystem;

		// Token: 0x0400AC28 RID: 44072
		[Tooltip("Player wet-look controller. Resolved lazily when left empty.")]
		[SerializeField]
		private CielWetController wetController;

		// Token: 0x0400AC29 RID: 44073
		[Header("Rain Wet Decal")]
		[Tooltip("Temporary skin decal kept visible while it rains (Ciel uses Sweat Light).")]
		[SerializeField]
		private SkinDecal rainSweatDecal;

		// Token: 0x0400AC2A RID: 44074
		[Tooltip("Sweat decal opacity reached while rain continues.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float rainSweatPeakOpacity = 1f;

		// Token: 0x0400AC2B RID: 44075
		[Tooltip("Seconds used to fade the rain sweat decal in.")]
		[Min(0f)]
		[SerializeField]
		private float rainSweatFadeInDuration = 5f;

		// Token: 0x0400AC2C RID: 44076
		[Tooltip("Seconds used to fade the rain sweat decal out after rain stops.")]
		[Min(0f)]
		[SerializeField]
		private float rainSweatFadeOutDuration = 5f;

		// Token: 0x0400AC2D RID: 44077
		[Header("Rain Detection")]
		[Tooltip("A weather preset is rainy when its name contains one of these tokens (case-insensitive).")]
		[SerializeField]
		private string[] rainyPresetTokens = new string[] { "rain", "storm" };

		// Token: 0x0400AC2E RID: 44078
		[Tooltip("Seconds between Azure weather checks.")]
		[Min(0.05f)]
		[SerializeField]
		private float weatherPollInterval = 0.5f;

		// Token: 0x0400AC2F RID: 44079
		[Tooltip("Seconds between retries while a paint target is not available yet.")]
		[Min(0.05f)]
		[SerializeField]
		private float referenceRetryInterval = 1f;

		// Token: 0x0400AC30 RID: 44080
		[Header("Wash")]
		[Tooltip("Continuous seconds of rain required to remove the paint completely. Zero removes it on the first positive rain step.")]
		[Min(0f)]
		[SerializeField]
		private float washDurationSeconds = 20f;

		// Token: 0x0400AC31 RID: 44081
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400AC32 RID: 44082
		private float _washProgress;

		// Token: 0x0400AC33 RID: 44083
		private float _weatherPollRemaining;

		// Token: 0x0400AC34 RID: 44084
		private float _referenceRetryRemaining;

		// Token: 0x0400AC35 RID: 44085
		private bool _isRaining;

		// Token: 0x0400AC36 RID: 44086
		private bool _trackingPaint;

		// Token: 0x0400AC37 RID: 44087
		private bool _rainOverrideEnabled;

		// Token: 0x0400AC38 RID: 44088
		private bool _rainOverrideValue;

		// Token: 0x0400AC39 RID: 44089
		private bool _wetHeld;

		// Token: 0x0400AC3A RID: 44090
		private string _wetOwner;

		// Token: 0x0400AC3B RID: 44091
		private bool _ownsRainSweatDecal;

		// Token: 0x0400AC3C RID: 44092
		private bool _rainSweatWasPresent;

		// Token: 0x0400AC3D RID: 44093
		private bool _rainSweatPreviousActive;

		// Token: 0x0400AC3E RID: 44094
		private float _rainSweatPreviousOpacity;

		// Token: 0x0400AC3F RID: 44095
		private Color _rainSweatPreviousTint = Color.white;

		// Token: 0x0400AC40 RID: 44096
		private float _rainSweatOpacity;
	}
}
