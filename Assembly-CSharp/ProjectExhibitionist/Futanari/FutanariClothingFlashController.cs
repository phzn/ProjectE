using System;
using Features.CheerleaderWind;
using Features.Flash;
using Features.UniversityUniform;
using UnityEngine;

namespace ProjectExhibitionist.Futanari
{
	// Token: 0x020009DB RID: 2523
	[DefaultExecutionOrder(100)]
	[DisallowMultipleComponent]
	public sealed class FutanariClothingFlashController : MonoBehaviour
	{
		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x0600473C RID: 18236 RVA: 0x001AEE6A File Offset: 0x001AD06A
		public bool IsClothingExposureActive
		{
			get
			{
				return this.EvaluateExposure();
			}
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x001AEE72 File Offset: 0x001AD072
		private void Awake()
		{
			this.ResolveReferences();
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x001AEE7A File Offset: 0x001AD07A
		private void OnEnable()
		{
			this.ResolveReferences();
			this._hasAppliedState = false;
			this.RefreshExposure();
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x001AEE8F File Offset: 0x001AD08F
		private void Update()
		{
			if (this.HasMissingReference() && Time.unscaledTime >= this._nextResolveTime)
			{
				this._nextResolveTime = Time.unscaledTime + 1f;
				this.ResolveReferences();
			}
			this.RefreshExposure();
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x001AEEC3 File Offset: 0x001AD0C3
		private void OnDisable()
		{
			this.ApplyExposure(false, true);
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x001AEED0 File Offset: 0x001AD0D0
		private void ResolveReferences()
		{
			if (this.futanariMode == null)
			{
				this.futanariMode = base.GetComponent<FutanariModeController>();
			}
			if (this.erectionController == null)
			{
				this.erectionController = base.GetComponent<PenisErectionController>();
			}
			Transform root = base.transform.root;
			if (root == null)
			{
				return;
			}
			if (this.universitySkirtFlash == null || this.rainCoatFlash == null)
			{
				foreach (ArmsFlashController armsFlashController in root.GetComponentsInChildren<ArmsFlashController>(true))
				{
					if (!(armsFlashController == null) && armsFlashController.Config != null)
					{
						string flashLayerName = armsFlashController.Config.flashLayerName;
						if (this.universitySkirtFlash == null && flashLayerName == this.universitySkirtFlashId)
						{
							this.universitySkirtFlash = armsFlashController;
						}
						else if (this.rainCoatFlash == null && flashLayerName == this.rainCoatFlashId)
						{
							this.rainCoatFlash = armsFlashController;
						}
					}
				}
			}
			if (this.universitySkirtWind == null)
			{
				this.universitySkirtWind = root.GetComponentInChildren<UniversityUniformWindController>(true);
			}
			if (this.cheerleaderSkirtWind == null)
			{
				this.cheerleaderSkirtWind = root.GetComponentInChildren<CheerleaderWindController>(true);
			}
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x001AF000 File Offset: 0x001AD200
		private bool HasMissingReference()
		{
			return this.futanariMode == null || this.erectionController == null || this.universitySkirtFlash == null || this.rainCoatFlash == null || this.universitySkirtWind == null || this.cheerleaderSkirtWind == null;
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x001AF061 File Offset: 0x001AD261
		private bool EvaluateExposure()
		{
			return FutanariClothingFlashController.IsEngaged(this.universitySkirtFlash) || FutanariClothingFlashController.IsEngaged(this.rainCoatFlash) || FutanariClothingFlashController.IsWindActive(this.universitySkirtWind) || FutanariClothingFlashController.IsWindActive(this.cheerleaderSkirtWind);
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x001AF097 File Offset: 0x001AD297
		private static bool IsEngaged(ArmsFlashController flash)
		{
			return flash != null && flash.isActiveAndEnabled && flash.IsEngaged;
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x001AF0B2 File Offset: 0x001AD2B2
		private static bool IsWindActive(UniversityUniformWindController wind)
		{
			return wind != null && wind.isActiveAndEnabled && wind.IsWindActive;
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x001AF0CD File Offset: 0x001AD2CD
		private static bool IsWindActive(CheerleaderWindController wind)
		{
			return wind != null && wind.isActiveAndEnabled && wind.IsWindActive;
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x001AF0E8 File Offset: 0x001AD2E8
		private void RefreshExposure()
		{
			this.ApplyExposure(this.EvaluateExposure() && this.futanariMode != null && this.futanariMode.IsEffectivelyVisible, false);
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x001AF118 File Offset: 0x001AD318
		private void ApplyExposure(bool active, bool force)
		{
			if (!force && this._hasAppliedState && this._lastExposureState == active)
			{
				return;
			}
			this._hasAppliedState = true;
			this._lastExposureState = active;
			PenisErectionController penisErectionController = this.erectionController;
			if (penisErectionController != null)
			{
				penisErectionController.SetClothingFlashErectionRequested(active);
			}
			if (this.debugLog)
			{
				Debug.Log(string.Format("[FutanariClothingFlash] Exposure/erection = {0}.", active), this);
			}
		}

		// Token: 0x040046CE RID: 18126
		[Header("Futanari")]
		[SerializeField]
		private FutanariModeController futanariMode;

		// Token: 0x040046CF RID: 18127
		[SerializeField]
		private PenisErectionController erectionController;

		// Token: 0x040046D0 RID: 18128
		[Header("Flash")]
		[SerializeField]
		private ArmsFlashController universitySkirtFlash;

		// Token: 0x040046D1 RID: 18129
		[SerializeField]
		private ArmsFlashController rainCoatFlash;

		// Token: 0x040046D2 RID: 18130
		[SerializeField]
		private string universitySkirtFlashId = "UniversitySkirtFlash";

		// Token: 0x040046D3 RID: 18131
		[SerializeField]
		private string rainCoatFlashId = "RainCoatFlash";

		// Token: 0x040046D4 RID: 18132
		[Header("Skirt lift")]
		[Tooltip("Le azioni Skirt Breeze espongono fisicamente Ciel come il flash della gonna.")]
		[SerializeField]
		private UniversityUniformWindController universitySkirtWind;

		// Token: 0x040046D5 RID: 18133
		[SerializeField]
		private CheerleaderWindController cheerleaderSkirtWind;

		// Token: 0x040046D6 RID: 18134
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x040046D7 RID: 18135
		private bool _hasAppliedState;

		// Token: 0x040046D8 RID: 18136
		private bool _lastExposureState;

		// Token: 0x040046D9 RID: 18137
		private float _nextResolveTime;
	}
}
