using System;
using Features.BodyPaintStudio;
using Features.ExposeLingerie;
using Features.Flash;
using Features.HideFlash;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.PoolDive;
using Features.Rank;
using Features.RunModes;
using Invector.vCharacterController;
using ProjectExhibitionist;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Detection
{
	// Token: 0x020012D3 RID: 4819
	public static class PlayerExposureState
	{
		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x06009554 RID: 38228 RVA: 0x003D4B03 File Offset: 0x003D2D03
		public static bool IsExposed
		{
			get
			{
				return PlayerExposureState.DetectionAttentionMultiplier > 0.0001f;
			}
		}

		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x06009555 RID: 38229 RVA: 0x003D4B11 File Offset: 0x003D2D11
		public static bool IsDecentlyCovered
		{
			get
			{
				PlayerExposureState.Resolve();
				return PlayerExposureState._clothing != null && PlayerExposureState._clothing.IsFullyClothed;
			}
		}

		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x06009556 RID: 38230 RVA: 0x003D4B34 File Offset: 0x003D2D34
		public static float DetectionAttentionMultiplier
		{
			get
			{
				PlayerExposureState.Resolve();
				if (PlayerExposureState._clothing == null)
				{
					return 1f;
				}
				float num = ((PlayerExposureState._skinDecals != null && PlayerExposureState._skinDecals.gameObject.activeInHierarchy) ? PlayerExposureState._skinDecals.DetectionCoverage : 0f);
				float num2 = ((PlayerExposureState._customPaint != null && PlayerExposureState._customPaint.isActiveAndEnabled) ? PlayerExposureState._customPaint.DetectionCoverage : 0f);
				num += num2;
				float num3 = ((PlayerExposureState._skinDecals != null && PlayerExposureState._skinDecals.gameObject.activeInHierarchy) ? PlayerExposureState._skinDecals.DetectionAttentionMultiplier : 1f);
				float detectionAttentionMultiplier = PlayerExposureState._clothing.DetectionAttentionMultiplier;
				float num4 = PlayerExposureState.ApplyHomeExposureAttentionFloor(detectionAttentionMultiplier, SceneManager.GetActiveScene().name == "CielHome", PlayerExposureState._clothing.IsTopPhysicallyCovered, PlayerExposureState._clothing.IsBottomPhysicallyCovered);
				bool flag = PlayerExposureState.IsExposureActionActive();
				if (flag)
				{
					num4 = Mathf.Max(1f, num4);
				}
				bool flag2 = RankDetectionPolicy.ProgressionTuningActive && RankDetectionPolicy.IsWearingStarterUnderwear(PlayerExposureState._clothing);
				num4 = RankDetectionPolicy.ApplyClothingAttentionFloor(num4, PlayerExposureState._clothing.IsFullyClothed, flag2, RankDetectionPolicy.ProgressionTuningActive);
				if (num == 0f)
				{
					return num4 * num3;
				}
				float num5 = ((num4 > detectionAttentionMultiplier) ? num4 : 0f);
				bool flag3 = flag || flag2 || (num2 > 0f && num4 > 0f);
				return PlayerExposureState.CalculatePaintedAttentionMultiplier(PlayerExposureState._clothing.RawDetectionCoverage, num, PlayerExposureState._clothing.IsTopPhysicallyCovered && !flag3, PlayerExposureState._clothing.IsBottomPhysicallyCovered && !flag3, num5) * num3;
			}
		}

		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x06009557 RID: 38231 RVA: 0x003D4CEC File Offset: 0x003D2EEC
		public static float GarmentAttentionGainMultiplier
		{
			get
			{
				if (!RankDetectionPolicy.ProgressionTuningActive)
				{
					return 1f;
				}
				PlayerExposureState.Resolve();
				RunModeKind currentMode = RunModeService.CurrentMode;
				RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
				return RankDetectionPolicy.GetAttentionGainMultiplier(currentMode, (currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive, RankDetectionPolicy.IsWearingStarterUnderwear(PlayerExposureState._clothing), RankDetectionPolicy.IsWearingTowel(PlayerExposureState._clothing));
			}
		}

		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x06009558 RID: 38232 RVA: 0x003D4D40 File Offset: 0x003D2F40
		public static bool IsDirectedFlashActive
		{
			get
			{
				PlayerExposureState.Resolve();
				if (FlashGateRegistry.IsFlashEngaged(PlayerExposureState._fullFlash) || FlashGateRegistry.IsFlashEngaged(PlayerExposureState._hideFlash))
				{
					return true;
				}
				if (PlayerExposureState._armsFlash != null)
				{
					for (int i = 0; i < PlayerExposureState._armsFlash.Length; i++)
					{
						if (PlayerExposureState.IsGarmentFlashActive(PlayerExposureState._armsFlash[i]))
						{
							return true;
						}
					}
				}
				return (PlayerExposureState._swimsuitFlash != null && PlayerExposureState._swimsuitFlash.isActiveAndEnabled && PlayerExposureState._swimsuitFlash.IsOpen) || (PlayerExposureState._lingerie != null && PlayerExposureState._lingerie.isActiveAndEnabled && PlayerExposureState._clothing != null && ((PlayerExposureState._lingerie.IsBraOpen && PlayerExposureState._clothing.IsWearingVisiblePreset("Outfit.Top", "expose_bra")) || (PlayerExposureState._lingerie.IsPantiesOpen && PlayerExposureState._clothing.IsWearingVisiblePreset("Outfit.Underwear", "expose_panties"))));
			}
		}

		// Token: 0x06009559 RID: 38233 RVA: 0x003D4E2A File Offset: 0x003D302A
		public static float CalculatePaintedAttentionMultiplier(float clothingCoverage, float paintCoverage, bool topPhysicallyCovered, bool bottomPhysicallyCovered, float minimumClothingAttentionMultiplier = 0f)
		{
			if (minimumClothingAttentionMultiplier > 0f)
			{
				clothingCoverage = Mathf.Min(clothingCoverage, (1f - minimumClothingAttentionMultiplier) * 10f);
			}
			return ClothingToggleController.DetectionCoverageToAttentionMultiplier(ClothingToggleController.ApplyPhysicalCoverageCap(clothingCoverage + paintCoverage, topPhysicallyCovered, bottomPhysicallyCovered));
		}

		// Token: 0x0600955A RID: 38234 RVA: 0x003D4E5A File Offset: 0x003D305A
		public static float ApplyHomeExposureAttentionFloor(float multiplier, bool isAtHome, bool topPhysicallyCovered, bool bottomPhysicallyCovered)
		{
			if (!isAtHome || (topPhysicallyCovered && bottomPhysicallyCovered))
			{
				return multiplier;
			}
			return Mathf.Max(1f, multiplier);
		}

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x0600955B RID: 38235 RVA: 0x003D4E74 File Offset: 0x003D3074
		public static bool IsFlashActive
		{
			get
			{
				PlayerExposureState.Resolve();
				if (PlayerExposureState._hideFlash != null && PlayerExposureState._hideFlash.IsExposureActive)
				{
					return true;
				}
				if (PlayerExposureState._fullFlash != null && PlayerExposureState._fullFlash.IsEngaged)
				{
					return true;
				}
				if (PlayerExposureState._armsFlash != null)
				{
					for (int i = 0; i < PlayerExposureState._armsFlash.Length; i++)
					{
						if (PlayerExposureState.IsGarmentFlashActive(PlayerExposureState._armsFlash[i]))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x0600955C RID: 38236 RVA: 0x003D4EE6 File Offset: 0x003D30E6
		public static bool IsBottomPhysicallyCovered
		{
			get
			{
				PlayerExposureState.Resolve();
				return PlayerExposureState._clothing != null && PlayerExposureState._clothing.IsBottomPhysicallyCoveredNow;
			}
		}

		// Token: 0x170017DA RID: 6106
		// (get) Token: 0x0600955D RID: 38237 RVA: 0x003D4F08 File Offset: 0x003D3108
		public static bool IsMouthPhysicallyBlocked
		{
			get
			{
				PlayerExposureState.Resolve();
				if (PlayerExposureState._clothing != null && PlayerExposureState._clothing.IsMouthPhysicallyBlocked)
				{
					return true;
				}
				PlayerInventoryManager instance = PlayerInventoryManager.Instance;
				if (instance == null)
				{
					return false;
				}
				vThirdPersonInput componentInParent = instance.GetComponentInParent<vThirdPersonInput>(true);
				WearableUseController componentInChildren = ((componentInParent != null) ? componentInParent.transform : instance.transform.root).GetComponentInChildren<WearableUseController>(true);
				return componentInChildren != null && componentInChildren.IsMouthPhysicallyBlocked;
			}
		}

		// Token: 0x0600955E RID: 38238 RVA: 0x003D4F84 File Offset: 0x003D3184
		private static void Resolve()
		{
			if (PlayerExposureState._clothing != null && Time.frameCount < PlayerExposureState._nextResolveFrame)
			{
				if (PlayerExposureState._skinDecalOwner != PlayerExposureState._clothing)
				{
					PlayerExposureState.ResolveSkinDecals();
				}
				return;
			}
			PlayerExposureState._nextResolveFrame = Time.frameCount + 60;
			if (PlayerExposureState._clothing == null)
			{
				PlayerExposureState._clothing = Object.FindObjectOfType<ClothingToggleController>();
			}
			PlayerExposureState.ResolveSkinDecals();
			if (PlayerExposureState._fullFlash == null)
			{
				PlayerExposureState._fullFlash = Object.FindObjectOfType<ClothFullFlashController>();
			}
			if (PlayerExposureState._lingerie == null)
			{
				PlayerExposureState._lingerie = Object.FindObjectOfType<ExposeLingerieController>();
			}
			if (PlayerExposureState._swimsuitFlash == null)
			{
				PlayerExposureState._swimsuitFlash = Object.FindObjectOfType<SwimsuitFlashController>();
			}
			if (PlayerExposureState._hideFlash == null)
			{
				PlayerExposureState._hideFlash = Object.FindObjectOfType<HideFlashController>();
			}
			if (PlayerExposureState._armsFlash == null || PlayerExposureState._armsFlash.Length == 0 || PlayerExposureState.AnyDead(PlayerExposureState._armsFlash))
			{
				PlayerExposureState._armsFlash = Object.FindObjectsOfType<ArmsFlashController>();
			}
		}

		// Token: 0x0600955F RID: 38239 RVA: 0x003D506C File Offset: 0x003D326C
		private static void ResolveSkinDecals()
		{
			if (PlayerExposureState._skinDecalOwner != PlayerExposureState._clothing)
			{
				PlayerExposureState._skinDecalOwner = PlayerExposureState._clothing;
				PlayerExposureState._skinDecals = null;
				PlayerExposureState._customPaint = null;
			}
			if (PlayerExposureState._skinDecals == null && PlayerExposureState._clothing != null)
			{
				PlayerExposureState._skinDecals = PlayerExposureState._clothing.GetComponentInChildren<SkinDecalController>();
			}
			if (PlayerExposureState._customPaint == null && PlayerExposureState._clothing != null)
			{
				PlayerExposureState._customPaint = PlayerExposureState._clothing.GetComponentInChildren<BodyPaintCanvasService>();
			}
		}

		// Token: 0x06009560 RID: 38240 RVA: 0x003D50F4 File Offset: 0x003D32F4
		private static bool AnyDead(ArmsFlashController[] arr)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				if (arr[i] == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009561 RID: 38241 RVA: 0x003D5120 File Offset: 0x003D3320
		private static bool IsExposureActionActive()
		{
			if (PlayerExposureState._hideFlash != null && PlayerExposureState._hideFlash.IsExposureActive)
			{
				return true;
			}
			if (PlayerExposureState._fullFlash != null && PlayerExposureState._fullFlash.IsEngaged)
			{
				return true;
			}
			if (PlayerExposureState._lingerie != null && PlayerExposureState._lingerie.AnyOpen)
			{
				return true;
			}
			if (PlayerExposureState._swimsuitFlash != null && PlayerExposureState._swimsuitFlash.IsOpen)
			{
				return true;
			}
			if (PlayerExposureState._armsFlash == null)
			{
				return false;
			}
			for (int i = 0; i < PlayerExposureState._armsFlash.Length; i++)
			{
				if (PlayerExposureState.IsGarmentFlashActive(PlayerExposureState._armsFlash[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009562 RID: 38242 RVA: 0x003D51C5 File Offset: 0x003D33C5
		private static bool IsGarmentFlashActive(ArmsFlashController flash)
		{
			return FlashGateRegistry.IsFlashEngaged(flash);
		}

		// Token: 0x04009A6D RID: 39533
		private const int ResolveThrottleFrames = 60;

		// Token: 0x04009A6E RID: 39534
		private static ClothingToggleController _clothing;

		// Token: 0x04009A6F RID: 39535
		private static ClothingToggleController _skinDecalOwner;

		// Token: 0x04009A70 RID: 39536
		private static SkinDecalController _skinDecals;

		// Token: 0x04009A71 RID: 39537
		private static BodyPaintCanvasService _customPaint;

		// Token: 0x04009A72 RID: 39538
		private static ClothFullFlashController _fullFlash;

		// Token: 0x04009A73 RID: 39539
		private static ArmsFlashController[] _armsFlash;

		// Token: 0x04009A74 RID: 39540
		private static ExposeLingerieController _lingerie;

		// Token: 0x04009A75 RID: 39541
		private static SwimsuitFlashController _swimsuitFlash;

		// Token: 0x04009A76 RID: 39542
		private static HideFlashController _hideFlash;

		// Token: 0x04009A77 RID: 39543
		private static int _nextResolveFrame;
	}
}
