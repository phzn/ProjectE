using System;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Detection;
using Features.Inventory;
using Features.Inventory.Wearable;
using Invector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.WardrobeCombos
{
	// Token: 0x02000B62 RID: 2914
	public sealed class WardrobeComboRuntime : MonoBehaviour
	{
		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x060052AD RID: 21165 RVA: 0x00201406 File Offset: 0x001FF606
		// (set) Token: 0x060052AE RID: 21166 RVA: 0x0020140D File Offset: 0x001FF60D
		public static WardrobeComboRuntime Instance { get; private set; }

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x060052AF RID: 21167 RVA: 0x00201418 File Offset: 0x001FF618
		public static WardrobeCombo ActiveCombos
		{
			get
			{
				if (WardrobeComboRuntime.Instance == null)
				{
					return WardrobeCombo.None;
				}
				if (WardrobeComboRuntime.Instance._wear == null && Time.unscaledTime >= WardrobeComboRuntime.Instance._nextResolveTime)
				{
					WardrobeComboRuntime.Instance._nextResolveTime = Time.unscaledTime + 1f;
					WardrobeComboRuntime.Instance.ResolvePlayer();
				}
				WardrobeComboRuntime.Instance.RefreshCombos(true, false);
				return WardrobeComboRuntime.Instance._active;
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x060052B0 RID: 21168 RVA: 0x0020148C File Offset: 0x001FF68C
		public static bool HasAthletic
		{
			get
			{
				return (WardrobeComboRuntime.ActiveCombos & WardrobeCombo.Athletic) > WardrobeCombo.None;
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x060052B1 RID: 21169 RVA: 0x00201498 File Offset: 0x001FF698
		public static bool HasLucky
		{
			get
			{
				return (WardrobeComboRuntime.ActiveCombos & WardrobeCombo.Lucky) > WardrobeCombo.None;
			}
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x060052B2 RID: 21170 RVA: 0x002014A4 File Offset: 0x001FF6A4
		public static bool HasGhost
		{
			get
			{
				return (WardrobeComboRuntime.ActiveCombos & WardrobeCombo.Ghost) > WardrobeCombo.None;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x060052B3 RID: 21171 RVA: 0x002014B1 File Offset: 0x001FF6B1
		public static bool HasBlind
		{
			get
			{
				return (WardrobeComboRuntime.ActiveCombos & WardrobeCombo.Blind) > WardrobeCombo.None;
			}
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x002014BE File Offset: 0x001FF6BE
		public static bool ShouldIgnore(DetectionActorType actor)
		{
			return WardrobeComboRules.ShouldIgnore(WardrobeComboRuntime.ActiveCombos, actor);
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x002014CB File Offset: 0x001FF6CB
		public static bool ShouldDisableFinalDetection(DetectionActorType actor)
		{
			return WardrobeComboRules.ShouldDisableFinalDetection(WardrobeComboRuntime.ActiveCombos, actor, 0f);
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x002014E0 File Offset: 0x001FF6E0
		public static bool ShouldDisableFinalDetection(DetectionActorType actor, Vector3 observerPosition)
		{
			return WardrobeComboRules.ShouldDisableFinalDetection(WardrobeComboRuntime.ActiveCombos, actor, (WardrobeComboRuntime.Instance != null && WardrobeComboRuntime.Instance._wear != null) ? (observerPosition - WardrobeComboRuntime.Instance._wear.transform.position).sqrMagnitude : 0f);
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x00201540 File Offset: 0x001FF740
		public static bool TryGetBlindVisionCenter(out Vector3 center)
		{
			center = Vector3.zero;
			if (!WardrobeComboRuntime.HasBlind || WardrobeComboRuntime.Instance._wear == null)
			{
				return false;
			}
			center = WardrobeComboRuntime.Instance._wear.transform.position + Vector3.up;
			return true;
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x00201598 File Offset: 0x001FF798
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (WardrobeComboRuntime.Instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("WardrobeComboRuntime");
			Object.DontDestroyOnLoad(gameObject);
			WardrobeComboRuntime.Instance = gameObject.AddComponent<WardrobeComboRuntime>();
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x002015C4 File Offset: 0x001FF7C4
		private void Awake()
		{
			if (WardrobeComboRuntime.Instance != null && WardrobeComboRuntime.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			WardrobeComboRuntime.Instance = this;
			this._hud = base.GetComponent<WardrobeComboHUD>();
			if (this._hud == null)
			{
				this._hud = base.gameObject.AddComponent<WardrobeComboHUD>();
			}
			if (base.GetComponent<BlindfoldVision>() == null)
			{
				base.gameObject.AddComponent<BlindfoldVision>();
			}
			this._feedbackMutedUntil = Time.unscaledTime + 2f;
			SceneManager.sceneLoaded += this.HandleSceneLoaded;
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x00201664 File Offset: 0x001FF864
		private void OnDestroy()
		{
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			this.UnbindWearController();
			this.ReleaseImmortality();
			if (WardrobeComboRuntime.Instance == this)
			{
				WardrobeComboRuntime.Instance = null;
			}
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x00201696 File Offset: 0x001FF896
		private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this._feedbackMutedUntil = Time.unscaledTime + 2f;
			this._nextResolveTime = 0f;
			this._lastRefreshFrame = -1;
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x002016BB File Offset: 0x001FF8BB
		private void Update()
		{
			if (this._wear == null && Time.unscaledTime >= this._nextResolveTime)
			{
				this._nextResolveTime = Time.unscaledTime + 1f;
				this.ResolvePlayer();
			}
			this.RefreshCombos(true, false);
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x002016F8 File Offset: 0x001FF8F8
		private void RefreshCombos(bool showFeedback, bool force = false)
		{
			if (!force && this._lastRefreshFrame == Time.frameCount)
			{
				return;
			}
			this._lastRefreshFrame = Time.frameCount;
			this.BindStyleService();
			WardrobeCombo wardrobeCombo = this.Evaluate();
			if (wardrobeCombo != this._active)
			{
				WardrobeCombo wardrobeCombo2 = wardrobeCombo & ~this._active;
				this._active = wardrobeCombo;
				this.ApplyImmortality();
				WardrobeComboHUD hud = this._hud;
				if (hud != null)
				{
					hud.SetActiveCombos(this._active);
				}
				if (showFeedback && Time.unscaledTime >= this._feedbackMutedUntil)
				{
					this.ShowActivatedCombos(wardrobeCombo2);
					return;
				}
			}
			else if ((this._active & WardrobeCombo.Lucky) != WardrobeCombo.None)
			{
				this.ApplyImmortality();
			}
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x00201790 File Offset: 0x001FF990
		private void ResolvePlayer()
		{
			WearableUseController wearableUseController = Object.FindObjectOfType<WearableUseController>();
			if (wearableUseController == this._wear)
			{
				return;
			}
			this.UnbindWearController();
			this.ReleaseImmortality();
			this._wear = wearableUseController;
			this._health = ((this._wear != null) ? this._wear.GetComponentInParent<vHealthController>() : null);
			if (this._health == null && this._wear != null)
			{
				this._health = this._wear.GetComponentInChildren<vHealthController>(true);
			}
			this._active = WardrobeCombo.None;
			WardrobeComboHUD hud = this._hud;
			if (hud != null)
			{
				hud.SetActiveCombos(WardrobeCombo.None);
			}
			if (this._wear != null)
			{
				this._wear.OnItemWorn += this.HandleWardrobeChanged;
				this._wear.OnItemUnworn += this.HandleWardrobeChanged;
				this.RefreshCombos(false, true);
			}
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x00201874 File Offset: 0x001FFA74
		private void UnbindWearController()
		{
			if (this._style != null)
			{
				this._style.OnActiveStateChanged -= this.HandleStyleChanged;
			}
			this._style = null;
			if (this._wear == null)
			{
				return;
			}
			this._wear.OnItemWorn -= this.HandleWardrobeChanged;
			this._wear.OnItemUnworn -= this.HandleWardrobeChanged;
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x002018EA File Offset: 0x001FFAEA
		private void HandleWardrobeChanged(WearableItemSO item)
		{
			this.RefreshCombos(true, true);
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x002018F4 File Offset: 0x001FFAF4
		private void BindStyleService()
		{
			MCStyleCustomizationService mcstyleCustomizationService = ((this._wear != null) ? this._wear.StyleService : null);
			if (mcstyleCustomizationService == this._style)
			{
				return;
			}
			if (this._style != null)
			{
				this._style.OnActiveStateChanged -= this.HandleStyleChanged;
			}
			this._style = mcstyleCustomizationService;
			if (this._style != null)
			{
				this._style.OnActiveStateChanged += this.HandleStyleChanged;
			}
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x002018EA File Offset: 0x001FFAEA
		private void HandleStyleChanged()
		{
			this.RefreshCombos(true, true);
		}

		// Token: 0x060052C3 RID: 21187 RVA: 0x00201980 File Offset: 0x001FFB80
		private WardrobeCombo Evaluate()
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this._wear))
			{
				return WardrobeCombo.None;
			}
			if (this._wear == null)
			{
				return WardrobeCombo.None;
			}
			bool flag = this._wear.IsWorn("cloth_mask", true);
			bool flag2 = this.IsWearingSunglasses();
			bool flag3 = this._wear.IsWorn("cloth_gym_set_tanktop", true);
			bool flag4 = this._wear.IsWorn("cloth_gym_set_leggings", true);
			bool flag5 = this._wear.IsWorn("cloth_gym_set_full", true);
			bool flag6 = this._wear.IsWorn("cloth_rope", true);
			bool flag7 = this._wear.IsWorn("cloth_gagball", true);
			bool flag8 = this._wear.IsWorn("cloth_blindfold", true);
			bool flag9 = this.IsWearingLegendaryClassicPanties();
			bool flag10 = this._wear.IsWorn("cloth_ghost_main", true);
			bool flag11 = this._wear.IsWorn("headbag", true);
			bool flag12 = this._wear.IsWorn("cloth_panties_head_face", true);
			return WardrobeComboRules.Evaluate(flag, flag2, flag3, flag4, flag5, flag6, flag7, flag8, flag9, flag10, flag11, flag12);
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x00201A8C File Offset: 0x001FFC8C
		private bool IsWearingLegendaryClassicPanties()
		{
			WearableItemSO worn = this._wear.GetWorn("cloth_classic_panties", true);
			if (worn == null || this._wear.StyleService == null || !worn.UsesMCStyle)
			{
				return false;
			}
			int slotSurfaceIndex = this._wear.StyleService.GetSlotSurfaceIndex(worn.mcStylePartId);
			return WardrobeComboRules.IsLegendarySurface(worn, slotSurfaceIndex);
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x00201AF0 File Offset: 0x001FFCF0
		private bool IsWearingSunglasses()
		{
			WearableItemSO worn = this._wear.GetWorn("cloth_glasses", true);
			if (worn == null || this._wear.StyleService == null || !worn.UsesMCStyle)
			{
				return false;
			}
			int slotSurfaceIndex = this._wear.StyleService.GetSlotSurfaceIndex(worn.mcStylePartId);
			MCStylePresetSO activePreset = this._wear.StyleService.GetActivePreset(worn.mcStylePartId);
			MCSurfaceVariantSO mcsurfaceVariantSO;
			return activePreset != null && activePreset.TryGetSurfaceVariant(slotSurfaceIndex, out mcsurfaceVariantSO) && worn.ResolveDetectionCoverage(mcsurfaceVariantSO) >= 2.9999f;
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x00201B8C File Offset: 0x001FFD8C
		private void ApplyImmortality()
		{
			bool flag = (this._active & WardrobeCombo.Lucky) > WardrobeCombo.None;
			if (this._health == null)
			{
				return;
			}
			if (flag)
			{
				if (!this._ownedImmortality)
				{
					this._previousImmortality = this._health.isImmortal;
					this._ownedImmortality = true;
				}
				this._health.isImmortal = true;
				return;
			}
			this.ReleaseImmortality();
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x00201BEA File Offset: 0x001FFDEA
		private void ReleaseImmortality()
		{
			if (this._ownedImmortality && this._health != null)
			{
				this._health.isImmortal = this._previousImmortality;
			}
			this._ownedImmortality = false;
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x00201C1A File Offset: 0x001FFE1A
		private void ShowActivatedCombos(WardrobeCombo activated)
		{
			WardrobeComboHUD hud = this._hud;
			if (hud == null)
			{
				return;
			}
			hud.QueueReveal(activated);
		}

		// Token: 0x04005343 RID: 21315
		private WearableUseController _wear;

		// Token: 0x04005344 RID: 21316
		private vHealthController _health;

		// Token: 0x04005345 RID: 21317
		private WardrobeCombo _active;

		// Token: 0x04005346 RID: 21318
		private bool _ownedImmortality;

		// Token: 0x04005347 RID: 21319
		private bool _previousImmortality;

		// Token: 0x04005348 RID: 21320
		private float _nextResolveTime;

		// Token: 0x04005349 RID: 21321
		private float _feedbackMutedUntil;

		// Token: 0x0400534A RID: 21322
		private WardrobeComboHUD _hud;

		// Token: 0x0400534B RID: 21323
		private int _lastRefreshFrame = -1;

		// Token: 0x0400534C RID: 21324
		private MCStyleCustomizationService _style;
	}
}
