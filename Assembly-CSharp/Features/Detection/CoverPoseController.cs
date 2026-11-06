using System;
using System.Collections.Generic;
using System.Text;
using Bozo.AnimeCharacters;
using Features.CielVoice;
using Features.Inventory;
using Features.Localization;
using Features.Rank;
using Features.TimeFreeze;
using Invector.vCharacterController;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Futanari;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

namespace Features.Detection
{
	// Token: 0x02001261 RID: 4705
	public class CoverPoseController : MonoBehaviour
	{
		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x060091E9 RID: 37353 RVA: 0x003BCB00 File Offset: 0x003BAD00
		// (set) Token: 0x060091EA RID: 37354 RVA: 0x003BCB07 File Offset: 0x003BAD07
		public static CoverPoseController Instance { get; private set; }

		// Token: 0x140000FE RID: 254
		// (add) Token: 0x060091EB RID: 37355 RVA: 0x003BCB10 File Offset: 0x003BAD10
		// (remove) Token: 0x060091EC RID: 37356 RVA: 0x003BCB48 File Offset: 0x003BAD48
		public event Action OnCoverStarted;

		// Token: 0x140000FF RID: 255
		// (add) Token: 0x060091ED RID: 37357 RVA: 0x003BCB80 File Offset: 0x003BAD80
		// (remove) Token: 0x060091EE RID: 37358 RVA: 0x003BCBB8 File Offset: 0x003BADB8
		public event Action OnCoverEnded;

		// Token: 0x060091EF RID: 37359 RVA: 0x003BCBF0 File Offset: 0x003BADF0
		private void Awake()
		{
			if (CoverPoseController.Instance == null)
			{
				CoverPoseController.Instance = this;
			}
			else
			{
				Object.Destroy(this);
			}
			if (this.futanariMode == null)
			{
				this.futanariMode = base.GetComponentInChildren<FutanariModeController>(true);
			}
			if (this.futanariMode != null)
			{
				this.futanariMode.OnFutanariModeChanged.AddListener(new UnityAction<bool>(this.OnFutanariModeChangedHandler));
			}
		}

		// Token: 0x060091F0 RID: 37360 RVA: 0x003BCC5D File Offset: 0x003BAE5D
		private void OnEnable()
		{
			PlayerActivityGate.Changed -= this.HandleActivityGateChanged;
			PlayerActivityGate.Changed += this.HandleActivityGateChanged;
		}

		// Token: 0x060091F1 RID: 37361 RVA: 0x003BCC81 File Offset: 0x003BAE81
		private void OnDisable()
		{
			PlayerActivityGate.Changed -= this.HandleActivityGateChanged;
			this.CancelPoseImmediate();
		}

		// Token: 0x060091F2 RID: 37362 RVA: 0x003BCC9C File Offset: 0x003BAE9C
		public void CancelPoseImmediate()
		{
			this.DeactivateCoverPose();
			CoverPoseController.ClearBlendShapeOverrides(this.EmbarrassedBlendShapesPerPose);
			CoverPoseController.ClearBlendShapeOverrides(this.ConfidentBlendShapesPerPose);
			if (this.PlayerAnimator != null && this.PlayerAnimator.runtimeAnimatorController != null && this.PlayerAnimator.isInitialized)
			{
				this.ForceLayerWeight(0f);
			}
			this.RestoreHeldItemCoverDeformations();
			this._activeBlendShapes = null;
			this._isCovering = false;
			this._wasCoveringLastFrame = false;
			CoverPoseController.ResetIKWeights(this.fullCoverEmbarrassedIK);
			CoverPoseController.ResetIKWeights(this.fullCoverExhibitIK);
			if (this.rigController != null)
			{
				this.rigController.DeactivateCoverRig();
			}
			this.ReleaseAnimatorUpdateMode();
			this._coverActivatedRig = false;
			this._coverTypeBlend = 0f;
			this._ikTargetWeight = 0f;
			this._endedPoseDeformationCleanupPending = false;
		}

		// Token: 0x060091F3 RID: 37363 RVA: 0x003BCD74 File Offset: 0x003BAF74
		private static void ResetIKWeights(List<CoverIKOverride> entries)
		{
			if (entries == null)
			{
				return;
			}
			foreach (CoverIKOverride coverIKOverride in entries)
			{
				if (coverIKOverride != null)
				{
					coverIKOverride.currentWeight = 0f;
				}
			}
		}

		// Token: 0x060091F4 RID: 37364 RVA: 0x003BCDD0 File Offset: 0x003BAFD0
		private static void ClearBlendShapeOverrides(List<CoverPoseBlendShapeSet> sets)
		{
			if (sets == null)
			{
				return;
			}
			foreach (CoverPoseBlendShapeSet coverPoseBlendShapeSet in sets)
			{
				if (((coverPoseBlendShapeSet != null) ? coverPoseBlendShapeSet.BlendShapes : null) != null)
				{
					foreach (CoverBlendShapeEntry coverBlendShapeEntry in coverPoseBlendShapeSet.BlendShapes)
					{
						if (coverBlendShapeEntry != null)
						{
							coverBlendShapeEntry.RemoveOverride();
							coverBlendShapeEntry.CurrentValue = 0f;
						}
					}
				}
			}
		}

		// Token: 0x060091F5 RID: 37365 RVA: 0x003BCE78 File Offset: 0x003BB078
		private void HandleActivityGateChanged(PlayerActivity blockedActivities)
		{
			if ((this._isCovering || this._endedPoseDeformationCleanupPending || this._heldItemDeformationCleanupPending) && (blockedActivities & (PlayerActivity.Cover | PlayerActivity.ArmsPose)) != PlayerActivity.None)
			{
				if (this.DebugMode)
				{
					Debug.Log("[CoverPoseController] Active cover stopped by PlayerActivityGate.");
				}
				this.CancelPoseImmediate();
			}
		}

		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x060091F6 RID: 37366 RVA: 0x003BCEB3 File Offset: 0x003BB0B3
		public bool IsFutanariLocked
		{
			get
			{
				return this.futanariMode != null && this.futanariMode.FutanariMode;
			}
		}

		// Token: 0x060091F7 RID: 37367 RVA: 0x003BCED0 File Offset: 0x003BB0D0
		private void OnFutanariModeChangedHandler(bool isFutanariOn)
		{
			if (isFutanariOn && this._currentCoverType != CoverType.BoobsCover)
			{
				if (this.DebugMode)
				{
					Debug.Log("[CoverPoseController] Futanari ON: forzato a BoobsCover.");
				}
				this._currentCoverType = CoverType.BoobsCover;
				if (this._isCovering)
				{
					this.ApplyCoverType();
				}
			}
		}

		// Token: 0x060091F8 RID: 37368 RVA: 0x003BCF04 File Offset: 0x003BB104
		private void Start()
		{
			this.InitializeAnimator();
			this.CacheAnimatorParameters();
			this.InitializeClothingIntegration();
			this.CacheBlendShapeIndices();
			this.InitializeIKBones();
			this._coverTypeTarget = ((this._currentCoverType == CoverType.FullCover) ? 1f : 0f);
			this._coverTypeBlend = this._coverTypeTarget;
			this._ikTargetWeight = this._coverTypeBlend;
			if (this.PlayerAnimator != null && this._hasCoverTypeParam)
			{
				this.PlayerAnimator.SetFloat(this.CoverTypeParamName, this._coverTypeBlend);
			}
		}

		// Token: 0x060091F9 RID: 37369 RVA: 0x003BCF90 File Offset: 0x003BB190
		private void InitializeIKBones()
		{
			if (this.PlayerAnimator == null)
			{
				return;
			}
			this._hipsBone = this.PlayerAnimator.GetBoneTransform(HumanBodyBones.Hips);
			if (this.ikReferenceBone == null)
			{
				this.ikReferenceBone = this._hipsBone;
			}
			if (this.rigController == null)
			{
				this.rigController = base.GetComponent<CharacterRigController>();
				if (this.rigController == null)
				{
					this.rigController = base.GetComponentInChildren<CharacterRigController>();
				}
			}
			if (this.rigController != null)
			{
				this._rigInitialized = this.rigController.Initialize();
			}
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[CoverPoseController] IK initialized - Hips: {0}, Reference: {1}, RigController: {2}, RigInit: {3}", new object[]
				{
					(this._hipsBone != null) ? this._hipsBone.name : "NULL",
					(this.ikReferenceBone != null) ? this.ikReferenceBone.name : "NULL",
					(this.rigController != null) ? "FOUND" : "NULL",
					this._rigInitialized
				}));
			}
		}

		// Token: 0x060091FA RID: 37370 RVA: 0x003BD0BC File Offset: 0x003BB2BC
		private void CacheBlendShapeIndices()
		{
			this.CacheVehicleBreastCompression();
			this.CachePerPoseBlendShapes(this.EmbarrassedBlendShapesPerPose, "Embarrassed");
			this.CachePerPoseBlendShapes(this.ConfidentBlendShapesPerPose, "Confident");
			this.WarnIfPoseSetCountMismatch(this.EmbarrassedBlendShapesPerPose, this.EmbarrassedPoseTypeCount, "Embarrassed");
			this.WarnIfPoseSetCountMismatch(this.ConfidentBlendShapesPerPose, this.ConfidentPoseTypeCount, "Confident");
			this._blendShapesInitialized = true;
			if (this.DebugMode)
			{
				int num = CoverPoseController.CountTotalBlendShapeEntries(this.EmbarrassedBlendShapesPerPose);
				int num2 = CoverPoseController.CountTotalBlendShapeEntries(this.ConfidentBlendShapesPerPose);
				Debug.Log(string.Format("[CoverPoseController] Cached {0} Embarrassed entries across {1} pose-variants, {2} Confident entries across {3} pose-variants", new object[]
				{
					num,
					this.EmbarrassedBlendShapesPerPose.Count,
					num2,
					this.ConfidentBlendShapesPerPose.Count
				}));
			}
		}

		// Token: 0x060091FB RID: 37371 RVA: 0x003BD194 File Offset: 0x003BB394
		private void CachePerPoseBlendShapes(List<CoverPoseBlendShapeSet> sets, string emotionLabel)
		{
			if (sets == null)
			{
				return;
			}
			for (int i = 0; i < sets.Count; i++)
			{
				CoverPoseBlendShapeSet coverPoseBlendShapeSet = sets[i];
				if (coverPoseBlendShapeSet != null && coverPoseBlendShapeSet.BlendShapes != null)
				{
					foreach (CoverBlendShapeEntry coverBlendShapeEntry in coverPoseBlendShapeSet.BlendShapes)
					{
						coverBlendShapeEntry.CacheIndex();
						if (coverBlendShapeEntry.CachedIndex < 0 && this.DebugMode)
						{
							Debug.LogWarning(string.Format("[CoverPoseController] {0} pose {1} blend shape '{2}' not found", emotionLabel, i, coverBlendShapeEntry.BlendShapeName));
						}
					}
				}
			}
		}

		// Token: 0x060091FC RID: 37372 RVA: 0x003BD240 File Offset: 0x003BB440
		private static int CountTotalBlendShapeEntries(List<CoverPoseBlendShapeSet> sets)
		{
			int num = 0;
			if (sets == null)
			{
				return 0;
			}
			foreach (CoverPoseBlendShapeSet coverPoseBlendShapeSet in sets)
			{
				if (((coverPoseBlendShapeSet != null) ? coverPoseBlendShapeSet.BlendShapes : null) != null)
				{
					num += coverPoseBlendShapeSet.BlendShapes.Count;
				}
			}
			return num;
		}

		// Token: 0x060091FD RID: 37373 RVA: 0x003BD2AC File Offset: 0x003BB4AC
		private void WarnIfPoseSetCountMismatch(List<CoverPoseBlendShapeSet> sets, int expectedCount, string emotionLabel)
		{
			int num = ((sets != null) ? sets.Count : 0);
			if (num != expectedCount)
			{
				Debug.LogWarning(string.Format("[CoverPoseController] {0} blend shape set count ({1}) does not match {2}PoseTypeCount ({3}). Pose-variants without a matching set will have no blend shapes applied.", new object[] { emotionLabel, num, emotionLabel, expectedCount }));
			}
		}

		// Token: 0x060091FE RID: 37374 RVA: 0x003BD2FC File Offset: 0x003BB4FC
		private void CacheAnimatorParameters()
		{
			if (this.PlayerAnimator == null)
			{
				return;
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (AnimatorControllerParameter animatorControllerParameter in this.PlayerAnimator.parameters)
			{
				hashSet.Add(animatorControllerParameter.name);
				if (animatorControllerParameter.name == this.CoverPoseParamName)
				{
					this._hasCoverPoseParam = true;
				}
				if (animatorControllerParameter.name == this.IsCoveringParamName)
				{
					this._hasIsCoveringParam = true;
				}
				if (animatorControllerParameter.name == this.CoverTriggerName)
				{
					this._hasCoverTriggerParam = true;
				}
				if (animatorControllerParameter.name == this.CoverTypeParamName)
				{
					this._hasCoverTypeParam = true;
				}
				if (animatorControllerParameter.name == "BoobsType")
				{
					this._hasBoobsTypeParam = true;
				}
				if (animatorControllerParameter.name == this.CoverPoseTypeParamName)
				{
					this._hasCoverPoseTypeParam = true;
				}
				if (animatorControllerParameter.name == this.vehicleSpeedParamName)
				{
					this._hasVehicleSpeedParam = true;
				}
			}
			this._vehicleSpeedParamHash = Animator.StringToHash(this.vehicleSpeedParamName);
			int num = Mathf.Max(1, this.CoverPoseWeightParamCount);
			this._coverPoseWeightParamHashes = new int[num];
			this._hasCoverPoseWeightParam = new bool[num];
			this._coverPoseWeightCurrent = new float[num];
			int num2 = 0;
			for (int j = 0; j < num; j++)
			{
				string text = string.Format("{0}{1}", this.CoverPoseWeightParamPrefix, j);
				this._coverPoseWeightParamHashes[j] = Animator.StringToHash(text);
				bool flag = hashSet.Contains(text);
				this._hasCoverPoseWeightParam[j] = flag;
				if (flag)
				{
					num2++;
				}
				this._coverPoseWeightCurrent[j] = ((j == 0) ? 1f : 0f);
			}
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[CoverPoseController] Parameter check: CoverPose={0}, IsCovering={1}, CoverTrigger={2}, CoverType={3}, PerPoseWeights={4}/{5}", new object[] { this._hasCoverPoseParam, this._hasIsCoveringParam, this._hasCoverTriggerParam, this._hasCoverTypeParam, num2, num }));
			}
			if (!this._hasCoverPoseParam && !string.IsNullOrEmpty(this.CoverPoseParamName))
			{
				Debug.LogWarning("[CoverPoseController] Parameter '" + this.CoverPoseParamName + "' not found in Animator");
			}
			if (!this._hasIsCoveringParam && !string.IsNullOrEmpty(this.IsCoveringParamName))
			{
				Debug.LogWarning("[CoverPoseController] Parameter '" + this.IsCoveringParamName + "' not found in Animator");
			}
			if (!this._hasCoverTriggerParam && !string.IsNullOrEmpty(this.CoverTriggerName))
			{
				Debug.LogWarning("[CoverPoseController] Parameter '" + this.CoverTriggerName + "' not found in Animator");
			}
			if (!this._hasCoverTypeParam && !string.IsNullOrEmpty(this.CoverTypeParamName))
			{
				Debug.LogWarning("[CoverPoseController] Parameter '" + this.CoverTypeParamName + "' not found in Animator");
			}
			if (!this._hasCoverPoseTypeParam && !string.IsNullOrEmpty(this.CoverPoseTypeParamName) && num2 == 0)
			{
				Debug.LogWarning(string.Concat(new string[] { "[CoverPoseController] Neither '", this.CoverPoseTypeParamName, "' nor any '", this.CoverPoseWeightParamPrefix, "<i>' parameters found — pose-variant cycling will not animate." }));
			}
		}

		// Token: 0x060091FF RID: 37375 RVA: 0x003BD63C File Offset: 0x003BB83C
		private void InitializeClothingIntegration()
		{
			if (this.ClothingController == null)
			{
				this.ClothingController = Object.FindObjectOfType<ClothingToggleController>();
			}
			if (this.ClothingController != null)
			{
				this.ClothingController.OnClothingStateChanged.AddListener(new UnityAction<bool>(this.OnClothingStateChanged));
				this.ClothingController.OnGranularCoverageChanged += this.OnGranularCoverageChanged;
				if (this.DebugMode)
				{
					Debug.Log("[CoverPoseController] Subscribed to ClothingToggleController events");
				}
			}
		}

		// Token: 0x06009200 RID: 37376 RVA: 0x003BD6B5 File Offset: 0x003BB8B5
		private void OnClothingStateChanged(bool isFullyClothed)
		{
			this.ValidateActivePose();
		}

		// Token: 0x06009201 RID: 37377 RVA: 0x003BD6BD File Offset: 0x003BB8BD
		private void OnGranularCoverageChanged()
		{
			this.CacheVehicleBreastCompression();
			this.ValidateActivePose();
		}

		// Token: 0x06009202 RID: 37378 RVA: 0x003BD6CC File Offset: 0x003BB8CC
		private void ValidateActivePose()
		{
			if (!this._isCovering)
			{
				return;
			}
			bool flag = this.IsExhibitCoverPoseActive();
			int num = (flag ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex);
			if (this.IsPoseAvailable(flag, num))
			{
				if (this._currentCoverType == CoverType.FullCover && !this.CurrentPoseAllowsFullCover())
				{
					this.ApplyCoverType();
				}
				return;
			}
			int num2 = this.FirstAvailablePoseIndex(flag);
			if (num2 < 0)
			{
				if (this.DebugMode)
				{
					Debug.Log("[CoverPoseController] Nessuna cover pose disponibile col vestiario attuale — deattivo.");
				}
				this.DeactivateCoverPose();
				return;
			}
			if (flag)
			{
				this._confidentPoseTypeIndex = num2;
			}
			else
			{
				this._embarrassedPoseTypeIndex = num2;
			}
			this.ApplyCoverPoseTypeToAnimator();
			this.RefreshActiveBlendShapesForCurrentPose();
			this.RefreshMagicaClothForCurrentPose();
			this.ApplyCoverType();
		}

		// Token: 0x06009203 RID: 37379 RVA: 0x003BD770 File Offset: 0x003BB970
		private void Update()
		{
			this.RecalculateArmLayerTargets();
			this.BlendArmLayers();
			this.UpdateVehicleLimbConstraintSuppression();
			this.UpdateCoverTypeBlend();
			this.ApplyCoverPoseTypeToAnimator();
			this.UpdateVehicleMotionStateBlending();
			this.PollEmotionStateForCoverPoseUpdate();
			this.ValidateActivePose();
			this.HandleCoverTypeToggleInput();
			this.ApplyCoverIK();
			this.BroadcastEmotionPoseSelectionIfChanged();
		}

		// Token: 0x06009204 RID: 37380 RVA: 0x003BD7C0 File Offset: 0x003BB9C0
		private void PollEmotionStateForCoverPoseUpdate()
		{
			if (this.ForceEmotion != CoverEmotionOverride.None)
			{
				return;
			}
			if (!this._isCovering || PlayerEmotionController.Instance == null)
			{
				return;
			}
			bool isExhibitionDominant = PlayerEmotionController.Instance.IsExhibitionDominant;
			if (!this._hasTrackedDominantOnce)
			{
				this._lastSeenExhibitDominant = isExhibitionDominant;
				this._hasTrackedDominantOnce = true;
				return;
			}
			if (isExhibitionDominant != this._lastSeenExhibitDominant)
			{
				this._lastSeenExhibitDominant = isExhibitionDominant;
				this.UpdateCoverPoseForCurrentState();
			}
		}

		// Token: 0x06009205 RID: 37381 RVA: 0x003BD824 File Offset: 0x003BBA24
		[ContextMenu("Dump Cover IK Diagnostics")]
		public void DumpCoverIKDiagnostics()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("===== Cover IK Diagnostic Dump (RUNTIME) =====");
			stringBuilder.AppendLine(string.Format("Context        : PLAY MODE  |  IsCovering={0}  IsRidingVehicle={1}  AllowCover={2}", this._isCovering, this._isRidingVehicle, this._vehicleAllowsCover));
			stringBuilder.AppendLine(string.Format("MotionState    : current={0}  previous={1}  blendT={2:F3}", this._vehicleMotionStateCurrent, this._vehicleMotionStatePrevious, this._vehicleMotionStateBlend));
			stringBuilder.AppendLine(string.Format("Emotion        : exhibit={0}  coverType={1}  coverTypeBlend={2:F3}  ikTargetWeight={3:F3}", new object[]
			{
				this.IsExhibitCoverPoseActive(),
				this._currentCoverType,
				this._coverTypeBlend,
				this._ikTargetWeight
			}));
			if (base.transform != null)
			{
				stringBuilder.AppendLine(string.Format("Character      : name={0} worldPos={1:F4} worldRot={2:F2}", base.name, base.transform.position, base.transform.rotation.eulerAngles));
			}
			if (this.rigController != null)
			{
				Transform coverIKAnchor = this.rigController.CoverIKAnchor;
				if (coverIKAnchor != null)
				{
					stringBuilder.AppendLine(string.Format("CoverIKAnchor  : name={0} worldPos={1:F4} worldRot={2:F2}", coverIKAnchor.name, coverIKAnchor.position, coverIKAnchor.rotation.eulerAngles));
					stringBuilder.AppendLine(string.Format("                 parent={0} localPos={1:F4} localRot={2:F2}", (coverIKAnchor.parent != null) ? coverIKAnchor.parent.name : "null", coverIKAnchor.localPosition, coverIKAnchor.localEulerAngles));
					Transform parent = coverIKAnchor.parent;
					if (parent != null)
					{
						stringBuilder.AppendLine(string.Format("Hips Bone      : name={0} worldPos={1:F4} worldRot={2:F2}", parent.name, parent.position, parent.rotation.eulerAngles));
					}
				}
				Transform pelvisTarget = this.rigController.GetPelvisTarget();
				if (pelvisTarget != null)
				{
					stringBuilder.AppendLine(string.Format("Pelvis Target  : worldPos={0:F4} worldRot={1:F2}", pelvisTarget.position, pelvisTarget.rotation.eulerAngles));
				}
				CoverPoseController.DumpDiagTargetLine(stringBuilder, "Cover L Hand   ", this.rigController.GetCoverLimbTarget(LimbType.LeftHand));
				CoverPoseController.DumpDiagTargetLine(stringBuilder, "Cover R Hand   ", this.rigController.GetCoverLimbTarget(LimbType.RightHand));
				CoverPoseController.DumpDiagTargetLine(stringBuilder, "Cover L Hint   ", this.rigController.GetCoverLimbHint(LimbType.LeftHand));
				CoverPoseController.DumpDiagTargetLine(stringBuilder, "Cover R Hint   ", this.rigController.GetCoverLimbHint(LimbType.RightHand));
				VehiclePosePreset activeVehicleCoverPosePreset = this.GetActiveVehicleCoverPosePreset();
				if (activeVehicleCoverPosePreset != null && activeVehicleCoverPosePreset.coverPoseIK != null && activeVehicleCoverPosePreset.coverPoseIK.enabled)
				{
					bool flag = this.IsExhibitCoverPoseActive();
					VehicleCoverPoseStateOffsets vehicleCoverPoseStateOffsets;
					VehicleCoverPoseStateOffsets vehicleCoverPoseStateOffsets2;
					float num;
					this.GetBlendedPresetOffsets(activeVehicleCoverPosePreset, flag, out vehicleCoverPoseStateOffsets, out vehicleCoverPoseStateOffsets2, out num);
					string text = ((vehicleCoverPoseStateOffsets != null) ? vehicleCoverPoseStateOffsets.state.ToString() : "null");
					string text2 = ((vehicleCoverPoseStateOffsets2 != null) ? vehicleCoverPoseStateOffsets2.state.ToString() : "null");
					stringBuilder.AppendLine(string.Format("Preset Blend   : from={0}  to={1}  presetBlendT={2:F3}", text, text2, num));
					Vector3 vector;
					Vector3 vector2;
					Vector3 vector3;
					CoverPoseController.GetPresetLimbOffsets(vehicleCoverPoseStateOffsets, LimbType.RightHand, out vector, out vector2, out vector3);
					Vector3 vector4;
					Vector3 vector5;
					Vector3 vector6;
					CoverPoseController.GetPresetLimbOffsets(vehicleCoverPoseStateOffsets2, LimbType.RightHand, out vector4, out vector5, out vector6);
					Vector3 vector7 = Vector3.Lerp(vector, vector4, num);
					Vector3 eulerAngles = Quaternion.Lerp(Quaternion.Euler(vector2), Quaternion.Euler(vector5), num).eulerAngles;
					if (this.rigController.CoverIKAnchor != null)
					{
						Vector3 vector8 = this.rigController.CoverIKAnchor.TransformPoint(vector7);
						stringBuilder.AppendLine(string.Format("[Runtime R Hand] blendedLocalOffset={0:F4}  blendedRot={1:F2}", vector7, eulerAngles));
						stringBuilder.AppendLine(string.Format("                 reconstructedWorldPos={0:F4}", vector8));
						stringBuilder.AppendLine(string.Format("                 [from state '{0}'] localOffset={1:F4}  rot={2:F2}", text, vector, vector2));
						stringBuilder.AppendLine(string.Format("                 [to   state '{0}']   localOffset={1:F4}  rot={2:F2}", text2, vector4, vector5));
					}
					Vector3 vector9;
					Vector3 vector10;
					Vector3 vector11;
					CoverPoseController.GetPresetLimbOffsets(vehicleCoverPoseStateOffsets, LimbType.LeftHand, out vector9, out vector10, out vector11);
					Vector3 vector12;
					CoverPoseController.GetPresetLimbOffsets(vehicleCoverPoseStateOffsets2, LimbType.LeftHand, out vector12, out vector11, out vector10);
					Vector3 vector13 = Vector3.Lerp(vector9, vector12, num);
					if (this.rigController.CoverIKAnchor != null)
					{
						Transform coverIKAnchor2 = this.rigController.CoverIKAnchor;
						stringBuilder.AppendLine(string.Format("[Runtime L Hand] blendedLocalOffset={0:F4}  reconstructedWorldPos={1:F4}", vector13, coverIKAnchor2.TransformPoint(vector13)));
					}
				}
				else
				{
					stringBuilder.AppendLine("Preset Blend   : <no active vehicle cover preset>");
				}
			}
			if (this.PlayerAnimator != null)
			{
				stringBuilder.AppendLine(string.Format("Animator       : enabled={0} hasController={1} ", this.PlayerAnimator.enabled, this.PlayerAnimator.runtimeAnimatorController != null) + string.Format("speed={0:F2} updateMode={1}", this.PlayerAnimator.speed, this.PlayerAnimator.updateMode));
				if (this.PlayerAnimator.runtimeAnimatorController != null && this.PlayerAnimator.isInitialized)
				{
					AnimatorStateInfo currentAnimatorStateInfo = this.PlayerAnimator.GetCurrentAnimatorStateInfo(0);
					stringBuilder.AppendLine(string.Format("   layer0 stateHash={0} normalizedTime={1:F3}", currentAnimatorStateInfo.shortNameHash, currentAnimatorStateInfo.normalizedTime));
				}
			}
			if (this._vehiclePoseDriver != null)
			{
				stringBuilder.AppendLine(string.Format("VehiclePoseDriver: enabled={0} isActive={1} isBlending={2} current={3} target={4}", new object[]
				{
					this._vehiclePoseDriver.enabled,
					this._vehiclePoseDriver.IsActive,
					this._vehiclePoseDriver.IsBlending,
					this._vehiclePoseDriver.CurrentState,
					this._vehiclePoseDriver.TargetState
				}));
			}
			stringBuilder.AppendLine("==============================================");
			Debug.Log(stringBuilder.ToString());
		}

		// Token: 0x06009206 RID: 37382 RVA: 0x003BDE48 File Offset: 0x003BC048
		private static void DumpDiagTargetLine(StringBuilder sb, string label, Transform t)
		{
			if (t == null)
			{
				sb.AppendLine(label + ": <null>");
				return;
			}
			sb.AppendLine(string.Format("{0}: worldPos={1:F4} worldRot={2:F2}", label, t.position, t.rotation.eulerAngles));
		}

		// Token: 0x06009207 RID: 37383 RVA: 0x003BDEA4 File Offset: 0x003BC0A4
		private void UpdateCoverTypeBlend()
		{
			float num = (this._isCovering ? this._coverTypeTarget : 0f);
			this._coverTypeBlend = Mathf.MoveTowards(this._coverTypeBlend, num, this.coverTypeBlendSpeed * Time.deltaTime);
			this._ikTargetWeight = this._coverTypeBlend;
			if (this.PlayerAnimator != null && this._hasCoverTypeParam)
			{
				if (this.coverTypeParamDampTime > 0f)
				{
					this.PlayerAnimator.SetFloat(this.CoverTypeParamName, this._coverTypeBlend, this.coverTypeParamDampTime, Time.deltaTime);
					return;
				}
				this.PlayerAnimator.SetFloat(this.CoverTypeParamName, this._coverTypeBlend);
			}
		}

		// Token: 0x06009208 RID: 37384 RVA: 0x003BDF50 File Offset: 0x003BC150
		private bool ProcessIKOverrideList(List<CoverIKOverride> overrides, bool isActiveList, float dt)
		{
			if (overrides == null || overrides.Count == 0)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < overrides.Count; i++)
			{
				CoverIKOverride coverIKOverride = overrides[i];
				float num = (isActiveList ? this._ikTargetWeight : 0f);
				if (!this.IsIKLimbAllowedByVehicleMode(coverIKOverride.limb))
				{
					num = 0f;
				}
				float num2 = ((this._isRidingVehicle && this._vehicleAllowsCover) ? this.vehicleCoverIKBlendSpeed : this.ikBlendSpeed);
				coverIKOverride.currentWeight = Mathf.MoveTowards(coverIKOverride.currentWeight, num, num2 * dt);
				if (coverIKOverride.currentWeight > 0.001f)
				{
					flag = true;
					float currentWeight = coverIKOverride.currentWeight;
					Vector3 vector;
					Quaternion quaternion;
					Vector3 vector2;
					if (this.IsManualCoverIKTargetEditingActive())
					{
						this.SyncOverrideFromRigTargets(coverIKOverride);
						Transform coverLimbTarget = this.rigController.GetCoverLimbTarget(coverIKOverride.limb);
						Transform coverLimbHint = this.rigController.GetCoverLimbHint(coverIKOverride.limb);
						vector = ((coverLimbTarget != null) ? coverLimbTarget.position : Vector3.zero);
						quaternion = ((coverLimbTarget != null) ? coverLimbTarget.rotation : Quaternion.identity);
						vector2 = ((coverLimbHint != null) ? coverLimbHint.position : vector);
					}
					else
					{
						Transform transform = ((coverIKOverride.referenceMode == IKReferenceMode.CustomBone && coverIKOverride.customReferenceBone != null) ? coverIKOverride.customReferenceBone : this.ikReferenceBone);
						if (transform == null)
						{
							goto IL_023D;
						}
						Vector3 vector3 = coverIKOverride.localPosition;
						Vector3 vector4 = coverIKOverride.rotationOffset;
						Vector3 vector5 = coverIKOverride.hintLocalPosition;
						this.ApplyVehicleHandIKAdjustments(coverIKOverride.limb, ref transform, ref vector3, ref vector4, ref vector5);
						if (this._seatedHandTargets && coverIKOverride.limb == LimbType.RightHand)
						{
							SeatedCoverIKTarget seatedCoverIKTarget = ((overrides == this.fullCoverExhibitIK) ? this.seatedExhibitIK : this.seatedEmbarrassedIK);
							vector3 = seatedCoverIKTarget.localPosition;
							vector4 = seatedCoverIKTarget.rotationOffset;
							vector5 = seatedCoverIKTarget.hintLocalPosition;
						}
						vector = transform.TransformPoint(vector3);
						quaternion = transform.rotation * Quaternion.Euler(vector4);
						vector2 = transform.TransformPoint(vector5);
						if (this._seatedHandTargets && coverIKOverride.limb == LimbType.RightHand)
						{
							this.BlendSeatedProfileFlip(overrides, coverIKOverride, transform, ref vector, ref quaternion, ref vector2, ref currentWeight);
						}
					}
					this.rigController.SetCoverLimbPoseWeighted(coverIKOverride.limb, vector, quaternion, currentWeight, coverIKOverride.targetPositionWeight, coverIKOverride.targetRotationWeight, vector2, coverIKOverride.hintWeight);
				}
				else
				{
					coverIKOverride.currentWeight = 0f;
				}
				IL_023D:;
			}
			return flag;
		}

		// Token: 0x06009209 RID: 37385 RVA: 0x003BE1AC File Offset: 0x003BC3AC
		private void BlendSeatedProfileFlip(List<CoverIKOverride> overrides, CoverIKOverride ik, Transform refBone, ref Vector3 worldPos, ref Quaternion worldRot, ref Vector3 hintWorld, ref float weight)
		{
			bool flag = overrides == this.fullCoverExhibitIK;
			float num = 0f;
			foreach (CoverIKOverride coverIKOverride in (flag ? this.fullCoverEmbarrassedIK : this.fullCoverExhibitIK))
			{
				if (coverIKOverride != null && coverIKOverride.limb == ik.limb)
				{
					num = Mathf.Max(num, coverIKOverride.currentWeight);
				}
			}
			if (num <= 0f)
			{
				return;
			}
			SeatedCoverIKTarget seatedCoverIKTarget = (flag ? this.seatedEmbarrassedIK : this.seatedExhibitIK);
			float num2 = weight / (weight + num);
			weight = Mathf.Min(1f, weight + num);
			worldPos = Vector3.Lerp(refBone.TransformPoint(seatedCoverIKTarget.localPosition), worldPos, num2);
			worldRot = Quaternion.Slerp(refBone.rotation * Quaternion.Euler(seatedCoverIKTarget.rotationOffset), worldRot, num2);
			hintWorld = Vector3.Lerp(refBone.TransformPoint(seatedCoverIKTarget.hintLocalPosition), hintWorld, num2);
		}

		// Token: 0x0600920A RID: 37386 RVA: 0x003BE2D8 File Offset: 0x003BC4D8
		private void ApplyCoverIK()
		{
			if (this.rigController == null || !this._rigInitialized)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			this.ResetCoverIKLimbs(this.fullCoverEmbarrassedIK);
			this.ResetCoverIKLimbs(this.fullCoverExhibitIK);
			bool flag = this._activeIKOverrides == this.fullCoverEmbarrassedIK;
			bool flag2 = this._activeIKOverrides == this.fullCoverExhibitIK;
			bool flag3;
			bool flag4;
			if (flag)
			{
				flag3 = this.ProcessIKOverrideList(this.fullCoverExhibitIK, false, deltaTime);
				flag4 = this.ProcessIKOverrideList(this.fullCoverEmbarrassedIK, true, deltaTime);
			}
			else if (flag2)
			{
				flag4 = this.ProcessIKOverrideList(this.fullCoverEmbarrassedIK, false, deltaTime);
				flag3 = this.ProcessIKOverrideList(this.fullCoverExhibitIK, true, deltaTime);
			}
			else
			{
				flag4 = this.ProcessIKOverrideList(this.fullCoverEmbarrassedIK, false, deltaTime);
				flag3 = this.ProcessIKOverrideList(this.fullCoverExhibitIK, false, deltaTime);
			}
			bool flag5 = flag4 || flag3;
			if (flag5 && !this.rigController.IsCoverRigActive)
			{
				if (this._animatorUpdateModeLease == null)
				{
					this._animatorUpdateModeLease = AnimatorNormalUpdateModeLease.Acquire(this.PlayerAnimator);
				}
				this.rigController.ActivateCoverRig();
				this._coverActivatedRig = true;
			}
			else if (!flag5 && this._coverActivatedRig)
			{
				this.rigController.DeactivateCoverRig();
				this._coverActivatedRig = false;
				this.ReleaseAnimatorUpdateMode();
			}
			this.rigController.EnsureCoverRigWeight();
		}

		// Token: 0x0600920B RID: 37387 RVA: 0x003BE40C File Offset: 0x003BC60C
		private void ReleaseAnimatorUpdateMode()
		{
			AnimatorNormalUpdateModeLease animatorUpdateModeLease = this._animatorUpdateModeLease;
			if (animatorUpdateModeLease != null)
			{
				animatorUpdateModeLease.Dispose();
			}
			this._animatorUpdateModeLease = null;
		}

		// Token: 0x0600920C RID: 37388 RVA: 0x003BE428 File Offset: 0x003BC628
		private void ResetCoverIKLimbs(List<CoverIKOverride> overrides)
		{
			if (overrides == null)
			{
				return;
			}
			foreach (CoverIKOverride coverIKOverride in overrides)
			{
				if (coverIKOverride != null)
				{
					this.rigController.ZeroCoverLimb(coverIKOverride.limb);
				}
			}
		}

		// Token: 0x0600920D RID: 37389 RVA: 0x003BE488 File Offset: 0x003BC688
		private void HandleCoverTypeToggleInput()
		{
			if (!this._isCovering)
			{
				return;
			}
			if (PlayerActivityGate.IsBlocked(PlayerActivity.Cover | PlayerActivity.ArmsPose))
			{
				return;
			}
			if (this.respectVehicleCoverRestrictions && this._isRidingVehicle && !this._vehicleAllowsCover)
			{
				return;
			}
			if (this.coverTypeToggleInput.GetButtonDown())
			{
				this.ToggleCoverType();
			}
			if (!this.SuppressBuiltInPoseCycleInput && this.coverPoseTypeCycleInput.GetButtonDown())
			{
				this.CycleCoverPoseType();
			}
		}

		// Token: 0x0600920E RID: 37390 RVA: 0x003BE4F4 File Offset: 0x003BC6F4
		public void ToggleCoverType()
		{
			if (!this._isCovering)
			{
				return;
			}
			if (this.IsFutanariLocked)
			{
				if (this.DebugMode)
				{
					Debug.Log("[CoverPoseController] ToggleCoverType ignorato: Futanari Mode lockata su BoobsCover.");
				}
				if (this._currentCoverType != CoverType.BoobsCover)
				{
					this._currentCoverType = CoverType.BoobsCover;
					this.ApplyCoverType();
				}
				return;
			}
			if (this._currentCoverType == CoverType.BoobsCover && !this.CurrentPoseAllowsFullCover())
			{
				if (this.DebugMode)
				{
					Debug.Log("[CoverPoseController] ToggleCoverType ignorato: la posa attiva non permette FullCover.");
				}
				return;
			}
			this._currentCoverType = ((this._currentCoverType == CoverType.BoobsCover) ? CoverType.FullCover : CoverType.BoobsCover);
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[CoverPoseController] ToggleCoverType -> New Type: {0}", this._currentCoverType));
			}
			this.ApplyCoverType();
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[CoverPoseController] Cover type toggled to: {0}", this._currentCoverType));
			}
		}

		// Token: 0x0600920F RID: 37391 RVA: 0x003BE5BC File Offset: 0x003BC7BC
		public void SetCoverType(CoverType type)
		{
			if (this.IsFutanariLocked && type != CoverType.BoobsCover)
			{
				if (this.DebugMode)
				{
					Debug.Log(string.Format("[CoverPoseController] SetCoverType({0}) overridden a BoobsCover: Futanari Mode lockata.", type));
				}
				type = CoverType.BoobsCover;
			}
			if (type == CoverType.FullCover && !this.CurrentPoseAllowsFullCover())
			{
				if (this.DebugMode)
				{
					Debug.Log("[CoverPoseController] SetCoverType(FullCover) overridden a BoobsCover: la posa attiva non permette FullCover.");
				}
				type = CoverType.BoobsCover;
			}
			this._currentCoverType = type;
			if (this._isCovering)
			{
				this.ApplyCoverType();
			}
		}

		// Token: 0x06009210 RID: 37392 RVA: 0x003BE62C File Offset: 0x003BC82C
		public void CycleCoverPoseType()
		{
			if (!this._isCovering)
			{
				return;
			}
			bool flag = this.IsExhibitCoverPoseActive();
			int num = (flag ? this.ConfidentPoseTypeCount : this.EmbarrassedPoseTypeCount);
			if (num <= 1)
			{
				return;
			}
			int num2 = (flag ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex);
			int num3 = -1;
			for (int i = 1; i <= num; i++)
			{
				int num4 = (num2 + i) % num;
				if (this.IsPoseAvailable(flag, num4))
				{
					num3 = num4;
					break;
				}
			}
			if (num3 < 0 || num3 == num2)
			{
				return;
			}
			if (flag)
			{
				this._confidentPoseTypeIndex = num3;
			}
			else
			{
				this._embarrassedPoseTypeIndex = num3;
			}
			this.ApplyCoverPoseTypeToAnimator();
			this.RefreshActiveBlendShapesForCurrentPose();
			this.RefreshMagicaClothForCurrentPose();
			this.ApplyCoverType();
			if (this.DebugMode)
			{
				int num5 = (flag ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex);
				Debug.Log(string.Format("[CoverPoseController] CycleCoverPoseType -> {0} pose variant = {1}/{2}", flag ? "Confident" : "Embarrassed", num5, num - 1));
			}
		}

		// Token: 0x06009211 RID: 37393 RVA: 0x003BE718 File Offset: 0x003BC918
		private void ApplyCoverPoseTypeToAnimator()
		{
			if (this.PlayerAnimator == null)
			{
				return;
			}
			int num = (this.IsExhibitCoverPoseActive() ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex);
			if (this._coverPoseWeightParamHashes != null && this._coverPoseWeightParamHashes.Length != 0)
			{
				bool flag = false;
				for (int i = 0; i < this._coverPoseWeightParamHashes.Length; i++)
				{
					if (this._hasCoverPoseWeightParam[i])
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					float num2 = Mathf.Max(0.0001f, this.coverPoseTypeParamDampTime);
					float num3 = 1f - Mathf.Exp(-Time.deltaTime / num2);
					for (int j = 0; j < this._coverPoseWeightParamHashes.Length; j++)
					{
						if (this._hasCoverPoseWeightParam[j])
						{
							float num4 = ((j == num) ? 1f : 0f);
							this._coverPoseWeightCurrent[j] = Mathf.Lerp(this._coverPoseWeightCurrent[j], num4, num3);
							this.PlayerAnimator.SetFloat(this._coverPoseWeightParamHashes[j], this._coverPoseWeightCurrent[j]);
						}
					}
					return;
				}
			}
			if (!this._hasCoverPoseTypeParam)
			{
				return;
			}
			if (this.coverPoseTypeParamDampTime > 0f)
			{
				this.PlayerAnimator.SetFloat(this.CoverPoseTypeParamName, (float)num, this.coverPoseTypeParamDampTime, Time.deltaTime);
				return;
			}
			this.PlayerAnimator.SetFloat(this.CoverPoseTypeParamName, (float)num);
		}

		// Token: 0x06009212 RID: 37394 RVA: 0x003BE868 File Offset: 0x003BCA68
		private void RefreshActiveBlendShapesForCurrentPose()
		{
			bool flag = this.IsExhibitCoverPoseActive();
			int num = (flag ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex);
			List<CoverPoseBlendShapeSet> list = (flag ? this.ConfidentBlendShapesPerPose : this.EmbarrassedBlendShapesPerPose);
			if (list == null || num < 0 || num >= list.Count)
			{
				this._activeBlendShapes = null;
				return;
			}
			CoverPoseBlendShapeSet coverPoseBlendShapeSet = list[num];
			this._activeBlendShapes = ((coverPoseBlendShapeSet != null) ? coverPoseBlendShapeSet.BlendShapes : null);
		}

		// Token: 0x06009213 RID: 37395 RVA: 0x003BE8D0 File Offset: 0x003BCAD0
		private CoverArmsMode GetCurrentPoseArmsMode()
		{
			bool flag = this.IsExhibitCoverPoseActive();
			int num = (flag ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex);
			List<CoverPoseBlendShapeSet> list = (flag ? this.ConfidentBlendShapesPerPose : this.EmbarrassedBlendShapesPerPose);
			if (list == null || num < 0 || num >= list.Count)
			{
				return CoverArmsMode.BothArms;
			}
			CoverPoseBlendShapeSet coverPoseBlendShapeSet = list[num];
			if (coverPoseBlendShapeSet == null)
			{
				return CoverArmsMode.BothArms;
			}
			return coverPoseBlendShapeSet.ArmsMode;
		}

		// Token: 0x06009214 RID: 37396 RVA: 0x003BE92C File Offset: 0x003BCB2C
		private bool CurrentPoseAllowsFullCover()
		{
			bool flag = this.IsExhibitCoverPoseActive();
			return this.CanUseLowerBodyCover(flag, flag ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex);
		}

		// Token: 0x06009215 RID: 37397 RVA: 0x003BE958 File Offset: 0x003BCB58
		public bool CanUseLowerBodyCover(bool exhibit, int index)
		{
			if (this.ClothingController != null)
			{
				bool flag;
				bool flag2;
				this.ClothingController.GetCurrentPhysicalCoverage(out flag, out flag2);
				if (flag2)
				{
					return false;
				}
			}
			List<CoverPoseBlendShapeSet> list = (exhibit ? this.ConfidentBlendShapesPerPose : this.EmbarrassedBlendShapesPerPose);
			if (index < 0 || index >= this.GetEmotionPoseTypeCount(exhibit))
			{
				return false;
			}
			if (list == null || index >= list.Count)
			{
				return true;
			}
			CoverPoseBlendShapeSet coverPoseBlendShapeSet = list[index];
			return coverPoseBlendShapeSet == null || coverPoseBlendShapeSet.AllowFullCover;
		}

		// Token: 0x06009216 RID: 37398 RVA: 0x003BE9CC File Offset: 0x003BCBCC
		private bool CurrentPoseDisablesMagica()
		{
			bool flag = this.IsExhibitCoverPoseActive();
			int num = (flag ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex);
			List<CoverPoseBlendShapeSet> list = (flag ? this.ConfidentBlendShapesPerPose : this.EmbarrassedBlendShapesPerPose);
			if (list == null || num < 0 || num >= list.Count)
			{
				return true;
			}
			CoverPoseBlendShapeSet coverPoseBlendShapeSet = list[num];
			return coverPoseBlendShapeSet == null || coverPoseBlendShapeSet.DisableMagicaCloth;
		}

		// Token: 0x06009217 RID: 37399 RVA: 0x003BEA27 File Offset: 0x003BCC27
		private void RefreshMagicaClothForCurrentPose()
		{
			if (this.CurrentPoseDisablesMagica())
			{
				this.DisableMagicaClothObjects();
				return;
			}
			this.EnableMagicaClothObjects();
		}

		// Token: 0x06009218 RID: 37400 RVA: 0x003BEA40 File Offset: 0x003BCC40
		public void SetCoverEmotionVariant(bool exhibit)
		{
			if (!this.IsPoseAvailable(exhibit, this.GetEmotionPoseTypeIndex(exhibit)))
			{
				this.ValidateActivePose();
				return;
			}
			if (this.PlayerAnimator == null || !this._hasCoverPoseParam)
			{
				return;
			}
			int num = (exhibit ? this.ConfidentCoverPoseIndex : this.EmbarrassedCoverPoseIndex);
			this.PlayerAnimator.SetInteger(this.CoverPoseParamName, num);
			this.ApplyCoverPoseTypeToAnimator();
			this.RefreshActiveBlendShapesForCurrentPose();
			this.RefreshMagicaClothForCurrentPose();
			this.ApplyCoverType();
		}

		// Token: 0x06009219 RID: 37401 RVA: 0x003BEAB7 File Offset: 0x003BCCB7
		public void SetSeatedHandTargets(bool seated)
		{
			this._seatedHandTargets = seated;
		}

		// Token: 0x0600921A RID: 37402 RVA: 0x003BEAC0 File Offset: 0x003BCCC0
		public void SetVehicleMotionStateOverride(bool active, VehicleCoverMotionState state)
		{
			this._vehicleMotionStateOverrideActive = active;
			this._vehicleMotionStateOverride = state;
		}

		// Token: 0x0600921B RID: 37403 RVA: 0x003BEAD0 File Offset: 0x003BCCD0
		private void ApplyCoverType()
		{
			if (this._currentCoverType == CoverType.FullCover && !this.CurrentPoseAllowsFullCover())
			{
				if (this.DebugMode)
				{
					Debug.Log("[CoverPoseController] ApplyCoverType: posa attiva non permette FullCover, forzato BoobsCover.");
				}
				this._currentCoverType = CoverType.BoobsCover;
			}
			this._coverTypeTarget = ((this._currentCoverType == CoverType.FullCover) ? 1f : 0f);
			if (this._currentCoverType == CoverType.FullCover)
			{
				if ((this._hasCoverPoseParam ? this.PlayerAnimator.GetInteger(this.CoverPoseParamName) : 0) == this.ConfidentCoverPoseIndex)
				{
					this._activeIKOverrides = ((this.fullCoverExhibitIK.Count > 0) ? this.fullCoverExhibitIK : null);
				}
				else
				{
					this._activeIKOverrides = ((this.fullCoverEmbarrassedIK.Count > 0) ? this.fullCoverEmbarrassedIK : null);
				}
			}
			else
			{
				this._activeIKOverrides = null;
			}
			if (this.DebugMode)
			{
				string text = ((this._activeIKOverrides == this.fullCoverEmbarrassedIK) ? "FullCover-Embarrassed" : ((this._activeIKOverrides == this.fullCoverExhibitIK) ? "FullCover-Exhibit" : "None"));
				Debug.Log(string.Format("[CoverPoseController] Applied CoverType={0}, IK list={1}, CoverBlend target={2:F2}, current={3:F2}", new object[] { this._currentCoverType, text, this._coverTypeTarget, this._coverTypeBlend }));
			}
		}

		// Token: 0x0600921C RID: 37404 RVA: 0x003BEC0F File Offset: 0x003BCE0F
		private void LateUpdate()
		{
			this.UpdateBlendShapes(Time.deltaTime);
			this.ContinueEndedPoseDeformationCleanup();
			this.ContinueHeldItemCoverDeformationCleanup();
		}

		// Token: 0x0600921D RID: 37405 RVA: 0x003BEC28 File Offset: 0x003BCE28
		private void ContinueEndedPoseDeformationCleanup()
		{
			if (!this._endedPoseDeformationCleanupPending)
			{
				return;
			}
			if (this._isCovering)
			{
				this.DisposeCoverStreamRelease();
				this._endedPoseDeformationCleanupPending = false;
				return;
			}
			if (this.IsCoverLayerStillAnimating(this._armsLayerIndex, false) || this.IsCoverLayerStillAnimating(this._rightArmLayerIndex, false) || this.IsCoverLayerStillAnimating(this._leftArmLayerIndex, false))
			{
				return;
			}
			if (CoverPoseController.HasBlendShapeOverrides(this.EmbarrassedBlendShapesPerPose) || CoverPoseController.HasBlendShapeOverrides(this.ConfidentBlendShapesPerPose))
			{
				return;
			}
			this.ClearEndedShapeOverrides();
			this.RestoreCoverDeformations();
			this._endedPoseDeformationCleanupPending = !this.ResetFinishedCoverAnimationStream();
		}

		// Token: 0x0600921E RID: 37406 RVA: 0x003BECB8 File Offset: 0x003BCEB8
		private static bool HasBlendShapeOverrides(List<CoverPoseBlendShapeSet> sets)
		{
			if (sets == null)
			{
				return false;
			}
			foreach (CoverPoseBlendShapeSet coverPoseBlendShapeSet in sets)
			{
				if (((coverPoseBlendShapeSet != null) ? coverPoseBlendShapeSet.BlendShapes : null) != null)
				{
					foreach (CoverBlendShapeEntry coverBlendShapeEntry in coverPoseBlendShapeSet.BlendShapes)
					{
						if (coverBlendShapeEntry != null && coverBlendShapeEntry.HasOverride)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600921F RID: 37407 RVA: 0x003BED64 File Offset: 0x003BCF64
		public void ContinueHeldItemCoverDeformationCleanup()
		{
			if (!this._heldItemDeformationCleanupPending)
			{
				return;
			}
			if (this._isCovering)
			{
				this.DisposeCoverStreamRelease();
				this._heldItemDeformationCleanupPending = false;
				return;
			}
			this.RestoreCoverDeformations();
			this._heldItemDeformationCleanupPending = this.IsCoverLayerStillAnimating(this._armsLayerIndex, false) || this.IsCoverLayerStillAnimating(this._rightArmLayerIndex, false) || this.IsCoverLayerStillAnimating(this._leftArmLayerIndex, false);
			if (!this._heldItemDeformationCleanupPending)
			{
				this._heldItemDeformationCleanupPending = !this.ResetFinishedCoverAnimationStream();
			}
		}

		// Token: 0x06009220 RID: 37408 RVA: 0x003BEDE4 File Offset: 0x003BCFE4
		private bool ResetFinishedCoverAnimationStream()
		{
			if (this._coverStreamRelease != null)
			{
				if (!this._coverStreamRelease.Finished)
				{
					return false;
				}
				bool wasEvaluated = this._coverStreamRelease.WasEvaluated;
				this.DisposeCoverStreamRelease();
				if (wasEvaluated)
				{
					this._coverAnimationStreamNeedsReset = false;
					return true;
				}
			}
			if (!this._coverAnimationStreamNeedsReset)
			{
				return true;
			}
			if (!Application.isPlaying || this.PlayerAnimator == null)
			{
				this._coverAnimationStreamNeedsReset = false;
				return true;
			}
			RigBuilder component = this.PlayerAnimator.GetComponent<RigBuilder>();
			if (!(component != null) || !component.isActiveAndEnabled || !component.graph.IsValid())
			{
				this._coverAnimationStreamNeedsReset = false;
				return true;
			}
			if (!this.PlayerAnimator.isActiveAndEnabled)
			{
				return false;
			}
			if (!component.graph.IsPlaying())
			{
				return false;
			}
			this._coverStreamRelease = new CoverPoseAnimationStreamRelease(this.PlayerAnimator, component.graph, this._coverClipDeformations);
			return false;
		}

		// Token: 0x06009221 RID: 37409 RVA: 0x003BEEC1 File Offset: 0x003BD0C1
		private void DisposeCoverStreamRelease()
		{
			CoverPoseAnimationStreamRelease coverStreamRelease = this._coverStreamRelease;
			if (coverStreamRelease != null)
			{
				coverStreamRelease.Dispose();
			}
			this._coverStreamRelease = null;
		}

		// Token: 0x06009222 RID: 37410 RVA: 0x003BEEDC File Offset: 0x003BD0DC
		private void UpdateBlendShapes(float deltaTime)
		{
			if (!this._blendShapesInitialized)
			{
				return;
			}
			float num = ((this.PlayerAnimator != null && this._hasBoobsTypeParam) ? this.PlayerAnimator.GetFloat("BoobsType") : 3f);
			float num2 = Mathf.Max(this.BlendShapeBlendSpeed, this.LayerBlendSpeed) * deltaTime;
			this.BlendInactivePerPoseSetsToZero(this.EmbarrassedBlendShapesPerPose, num2);
			this.BlendInactivePerPoseSetsToZero(this.ConfidentBlendShapesPerPose, num2);
			if (this._activeBlendShapes != null && this._isCovering)
			{
				foreach (CoverBlendShapeEntry coverBlendShapeEntry in this._activeBlendShapes)
				{
					if (coverBlendShapeEntry.CachedIndex >= 0)
					{
						float num3 = ((coverBlendShapeEntry.BlendSpeedOverride > 0f) ? coverBlendShapeEntry.BlendSpeedOverride : this.BlendShapeBlendSpeed);
						coverBlendShapeEntry.Advance(coverBlendShapeEntry.ResolveTargetValue(num), num3 * deltaTime * 100f, coverBlendShapeEntry.RequiresChestContact ? this.GetVehicleBreastContactWeight() : 1f);
					}
				}
			}
			this.ApplyVehicleBreastCompression();
		}

		// Token: 0x06009223 RID: 37411 RVA: 0x003BEFFC File Offset: 0x003BD1FC
		private void CacheVehicleBreastCompression()
		{
			this._vehicleBreastCompression.Clear();
			this._coverClipDeformations.Clear();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer.sharedMesh == null))
				{
					BlendShapeSynchronizer component = skinnedMeshRenderer.GetComponent<BlendShapeSynchronizer>();
					if (!(component != null) || !component.Synchronizes(skinnedMeshRenderer))
					{
						foreach (string text in CoverPoseController.CoverClipDeformationShapes)
						{
							if (skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(text) >= 0)
							{
								CoverBlendShapeEntry coverBlendShapeEntry = new CoverBlendShapeEntry
								{
									TargetRenderer = skinnedMeshRenderer,
									BlendShapeName = text
								};
								coverBlendShapeEntry.CacheIndex();
								this._coverClipDeformations.Add(coverBlendShapeEntry);
								if (Array.IndexOf<string>(CoverPoseController.VehicleBreastCompressionShapes, text) >= 0)
								{
									this._vehicleBreastCompression.Add(coverBlendShapeEntry);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06009224 RID: 37412 RVA: 0x003BF0E0 File Offset: 0x003BD2E0
		private float GetVehicleBreastContactWeight()
		{
			if (!this._isCovering || !this._isRidingVehicle || !this._vehicleAllowsCover || this._vehicleCoverArmMode == VehicleCoverArmMode.BothArms)
			{
				return 1f;
			}
			LimbType limbType = ((this._vehicleCoverArmMode == VehicleCoverArmMode.LeftArmOnly) ? LimbType.LeftHand : LimbType.RightHand);
			return 1f - this.GetCoverHandIKWeight(limbType);
		}

		// Token: 0x06009225 RID: 37413 RVA: 0x003BF12E File Offset: 0x003BD32E
		public void RestoreHeldItemCoverDeformations()
		{
			this._heldItemDeformationCleanupPending = true;
			this.RestoreCoverDeformations();
		}

		// Token: 0x06009226 RID: 37414 RVA: 0x003BF140 File Offset: 0x003BD340
		private void RestoreCoverDeformations()
		{
			if (this._coverClipDeformations.Count == 0)
			{
				this.CacheVehicleBreastCompression();
			}
			foreach (CoverBlendShapeEntry coverBlendShapeEntry in this._coverClipDeformations)
			{
				coverBlendShapeEntry.RestoreUnanimatedValue();
			}
			CoverPoseController.RestoreUnanimatedValues(this.EmbarrassedBlendShapesPerPose);
			CoverPoseController.RestoreUnanimatedValues(this.ConfidentBlendShapesPerPose);
		}

		// Token: 0x06009227 RID: 37415 RVA: 0x003BF1BC File Offset: 0x003BD3BC
		private static void RestoreUnanimatedValues(List<CoverPoseBlendShapeSet> sets)
		{
			if (sets == null)
			{
				return;
			}
			foreach (CoverPoseBlendShapeSet coverPoseBlendShapeSet in sets)
			{
				if (((coverPoseBlendShapeSet != null) ? coverPoseBlendShapeSet.BlendShapes : null) != null)
				{
					foreach (CoverBlendShapeEntry coverBlendShapeEntry in coverPoseBlendShapeSet.BlendShapes)
					{
						if (coverBlendShapeEntry != null)
						{
							coverBlendShapeEntry.RestoreUnanimatedValue();
						}
					}
				}
			}
		}

		// Token: 0x06009228 RID: 37416 RVA: 0x003BF25C File Offset: 0x003BD45C
		private float GetCoverHandIKWeight(LimbType hand)
		{
			float num = 0f;
			CoverPoseController.AccumulateCoverHandIKWeight(this.fullCoverEmbarrassedIK, hand, ref num);
			CoverPoseController.AccumulateCoverHandIKWeight(this.fullCoverExhibitIK, hand, ref num);
			return Mathf.Clamp01(num);
		}

		// Token: 0x06009229 RID: 37417 RVA: 0x003BF294 File Offset: 0x003BD494
		private static void AccumulateCoverHandIKWeight(List<CoverIKOverride> overrides, LimbType hand, ref float weight)
		{
			if (overrides == null)
			{
				return;
			}
			foreach (CoverIKOverride coverIKOverride in overrides)
			{
				if (coverIKOverride != null && coverIKOverride.limb == hand)
				{
					weight = Mathf.Max(weight, coverIKOverride.currentWeight * coverIKOverride.targetPositionWeight);
				}
			}
		}

		// Token: 0x0600922A RID: 37418 RVA: 0x003BF304 File Offset: 0x003BD504
		private void ApplyVehicleBreastCompression()
		{
			float vehicleBreastContactWeight = this.GetVehicleBreastContactWeight();
			if (vehicleBreastContactWeight >= 1f)
			{
				return;
			}
			foreach (CoverBlendShapeEntry coverBlendShapeEntry in this._vehicleBreastCompression)
			{
				if (!(coverBlendShapeEntry.TargetRenderer == null) && !(coverBlendShapeEntry.TargetRenderer.sharedMesh == null) && coverBlendShapeEntry.CachedIndex < coverBlendShapeEntry.TargetRenderer.sharedMesh.blendShapeCount)
				{
					float blendShapeWeight = coverBlendShapeEntry.TargetRenderer.GetBlendShapeWeight(coverBlendShapeEntry.CachedIndex);
					coverBlendShapeEntry.TargetRenderer.SetBlendShapeWeight(coverBlendShapeEntry.CachedIndex, blendShapeWeight * vehicleBreastContactWeight);
				}
			}
		}

		// Token: 0x0600922B RID: 37419 RVA: 0x003BF3C0 File Offset: 0x003BD5C0
		private void BlendInactivePerPoseSetsToZero(List<CoverPoseBlendShapeSet> sets, float blendStep)
		{
			if (sets == null)
			{
				return;
			}
			foreach (CoverPoseBlendShapeSet coverPoseBlendShapeSet in sets)
			{
				if (coverPoseBlendShapeSet != null)
				{
					this.BlendInactiveToZero(coverPoseBlendShapeSet.BlendShapes, blendStep);
				}
			}
		}

		// Token: 0x0600922C RID: 37420 RVA: 0x003BF41C File Offset: 0x003BD61C
		private void BlendInactiveToZero(List<CoverBlendShapeEntry> entries, float blendStep)
		{
			if (entries == null || (this._isCovering && entries == this._activeBlendShapes))
			{
				return;
			}
			foreach (CoverBlendShapeEntry coverBlendShapeEntry in entries)
			{
				if (coverBlendShapeEntry != null)
				{
					bool flag = false;
					if (this._isCovering && this._activeBlendShapes != null)
					{
						foreach (CoverBlendShapeEntry coverBlendShapeEntry2 in this._activeBlendShapes)
						{
							if (coverBlendShapeEntry2 != null && coverBlendShapeEntry2.BlendShapeName == coverBlendShapeEntry.BlendShapeName && coverBlendShapeEntry2.TargetRenderer == coverBlendShapeEntry.TargetRenderer)
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						coverBlendShapeEntry.RelinquishSharedOverride();
					}
					else
					{
						coverBlendShapeEntry.Release(blendStep * 100f);
					}
				}
			}
		}

		// Token: 0x0600922D RID: 37421 RVA: 0x003BF51C File Offset: 0x003BD71C
		private void InitializeAnimator()
		{
			if (this.PlayerAnimator == null)
			{
				this.PlayerAnimator = base.GetComponentInChildren<Animator>();
			}
			if (this.PlayerAnimator != null)
			{
				if (!string.IsNullOrEmpty(this.ArmsLayerName))
				{
					this._armsLayerIndex = this.PlayerAnimator.GetLayerIndex(this.ArmsLayerName);
				}
				if (this._armsLayerIndex >= 0)
				{
					this._originalArmsWeight = this.PlayerAnimator.GetLayerWeight(this._armsLayerIndex);
					this._targetArmsWeight = this._originalArmsWeight;
				}
				else
				{
					Debug.LogWarning("[CoverPoseController] Arms layer '" + this.ArmsLayerName + "' not found in animator.");
				}
				if (!string.IsNullOrEmpty(this.RightArmLayerName))
				{
					this._rightArmLayerIndex = this.PlayerAnimator.GetLayerIndex(this.RightArmLayerName);
				}
				if (this._rightArmLayerIndex >= 0)
				{
					this._originalRightArmWeight = this.PlayerAnimator.GetLayerWeight(this._rightArmLayerIndex);
					this._targetRightArmWeight = this._originalRightArmWeight;
				}
				else if (this.DebugMode)
				{
					Debug.LogWarning("[CoverPoseController] RightArm layer '" + this.RightArmLayerName + "' not found in animator.");
				}
				if (!string.IsNullOrEmpty(this.LeftArmLayerName))
				{
					this._leftArmLayerIndex = this.PlayerAnimator.GetLayerIndex(this.LeftArmLayerName);
				}
				if (this._leftArmLayerIndex >= 0)
				{
					this._originalLeftArmWeight = this.PlayerAnimator.GetLayerWeight(this._leftArmLayerIndex);
					this._targetLeftArmWeight = this._originalLeftArmWeight;
					return;
				}
				if (this.DebugMode)
				{
					Debug.LogWarning("[CoverPoseController] LeftArm layer '" + this.LeftArmLayerName + "' not found in animator.");
				}
			}
		}

		// Token: 0x0600922E RID: 37422 RVA: 0x003BF6A8 File Offset: 0x003BD8A8
		private void RecalculateArmLayerTargets()
		{
			if (!this._isCovering)
			{
				this._targetArmsWeight = this._originalArmsWeight;
				this._targetRightArmWeight = this._originalRightArmWeight;
				this._targetLeftArmWeight = this._originalLeftArmWeight;
				this._wasCoveringLastFrame = false;
				return;
			}
			CoverArmsMode coverArmsMode = ((this._isRidingVehicle && this._vehicleAllowsCover && this._vehicleCoverArmMode > VehicleCoverArmMode.BothArms) ? CoverPoseController.VehicleArmModeToPoseArmsMode(this._vehicleCoverArmMode) : this.GetCurrentPoseArmsMode());
			if (!this._wasCoveringLastFrame)
			{
				this._previousEffectiveArmsMode = coverArmsMode;
				this._currentEffectiveArmsMode = coverArmsMode;
				this._armsModeBlend = 1f;
				this._wasCoveringLastFrame = true;
				return;
			}
			if (coverArmsMode != this._currentEffectiveArmsMode)
			{
				this._previousEffectiveArmsMode = this._currentEffectiveArmsMode;
				this._currentEffectiveArmsMode = coverArmsMode;
				this._armsModeBlend = 0f;
			}
		}

		// Token: 0x0600922F RID: 37423 RVA: 0x003BF76B File Offset: 0x003BD96B
		private static CoverArmsMode VehicleArmModeToPoseArmsMode(VehicleCoverArmMode mode)
		{
			if (mode == VehicleCoverArmMode.LeftArmOnly)
			{
				return CoverArmsMode.LeftArmOnly;
			}
			if (mode != VehicleCoverArmMode.RightArmOnly)
			{
				return CoverArmsMode.BothArms;
			}
			return CoverArmsMode.RightArmOnly;
		}

		// Token: 0x06009230 RID: 37424 RVA: 0x003BF77C File Offset: 0x003BD97C
		private void GetLayerWeightsForMode(CoverArmsMode mode, out float arms, out float left, out float right)
		{
			switch (mode)
			{
			case CoverArmsMode.LeftArmOnly:
				arms = 0f;
				left = this.CoverLayerWeight;
				right = 0f;
				return;
			case CoverArmsMode.RightArmOnly:
				arms = 0f;
				left = 0f;
				right = this.CoverLayerWeight;
				return;
			}
			arms = this.CoverLayerWeight;
			left = 0f;
			right = 0f;
		}

		// Token: 0x06009231 RID: 37425 RVA: 0x003BF7E4 File Offset: 0x003BD9E4
		private void BlendArmLayers()
		{
			if (this.PlayerAnimator == null)
			{
				return;
			}
			if (!this._isCovering)
			{
				this.BlendLayerWeight(this._armsLayerIndex, this._targetArmsWeight);
				this.BlendLayerWeight(this._rightArmLayerIndex, this._targetRightArmWeight);
				this.BlendLayerWeight(this._leftArmLayerIndex, this._targetLeftArmWeight);
				return;
			}
			if (this._armsModeBlend < 1f)
			{
				this._armsModeBlend = Mathf.MoveTowards(this._armsModeBlend, 1f, this.LayerBlendSpeed * Time.deltaTime);
			}
			float armsModeBlend = this._armsModeBlend;
			float num;
			float num2;
			float num3;
			this.GetLayerWeightsForMode(this._previousEffectiveArmsMode, out num, out num2, out num3);
			float num4;
			float num5;
			float num6;
			this.GetLayerWeightsForMode(this._currentEffectiveArmsMode, out num4, out num5, out num6);
			float num7 = Mathf.SmoothStep(0f, 1f, armsModeBlend);
			float num8 = Mathf.Lerp(num, num4, num7);
			float num9 = Mathf.Lerp(num2, num5, num7);
			float num10 = Mathf.Lerp(num3, num6, num7);
			this._targetArmsWeight = num8;
			this._targetLeftArmWeight = num9;
			this._targetRightArmWeight = num10;
			if (this._armsLayerIndex >= 0)
			{
				this.PlayerAnimator.SetLayerWeight(this._armsLayerIndex, num8);
			}
			if (this._leftArmLayerIndex >= 0)
			{
				this.PlayerAnimator.SetLayerWeight(this._leftArmLayerIndex, num9);
			}
			if (this._rightArmLayerIndex >= 0)
			{
				this.PlayerAnimator.SetLayerWeight(this._rightArmLayerIndex, num10);
			}
		}

		// Token: 0x06009232 RID: 37426 RVA: 0x003BF93C File Offset: 0x003BDB3C
		private void BlendLayerWeight(int layerIndex, float targetWeight)
		{
			if (layerIndex < 0)
			{
				return;
			}
			float layerWeight = this.PlayerAnimator.GetLayerWeight(layerIndex);
			if (Mathf.Approximately(layerWeight, targetWeight))
			{
				return;
			}
			float num = Mathf.MoveTowards(layerWeight, targetWeight, this.LayerBlendSpeed * Time.deltaTime);
			this.PlayerAnimator.SetLayerWeight(layerIndex, num);
		}

		// Token: 0x06009233 RID: 37427 RVA: 0x003BF988 File Offset: 0x003BDB88
		private bool IsIKLimbAllowedByVehicleMode(LimbType limb)
		{
			if (!this._isRidingVehicle || !this._vehicleAllowsCover)
			{
				return true;
			}
			VehicleCoverArmMode vehicleCoverArmMode = this._vehicleCoverArmMode;
			if (vehicleCoverArmMode != VehicleCoverArmMode.LeftArmOnly)
			{
				return vehicleCoverArmMode != VehicleCoverArmMode.RightArmOnly || limb == LimbType.RightHand;
			}
			return limb == LimbType.LeftHand;
		}

		// Token: 0x06009234 RID: 37428 RVA: 0x003BF9C4 File Offset: 0x003BDBC4
		private VehicleCoverMotionState GetVehicleMotionState()
		{
			if (!this._isRidingVehicle)
			{
				return VehicleCoverMotionState.Idle;
			}
			if (this._vehicleMotionStateOverrideActive)
			{
				return this._vehicleMotionStateOverride;
			}
			float num;
			if (this.TryGetVehicleNormalizedSpeed(out num))
			{
				return this.EvaluateMotionStateWithHysteresis(num);
			}
			if (this._vehiclePoseDriver != null)
			{
				VehiclePoseState vehiclePoseState = (this._vehiclePoseDriver.IsBlending ? this._vehiclePoseDriver.TargetState : this._vehiclePoseDriver.CurrentState);
				if (vehiclePoseState == VehiclePoseState.MovingLowSpeed)
				{
					return VehicleCoverMotionState.Moving;
				}
				if (vehiclePoseState == VehiclePoseState.MovingHighSpeed)
				{
					return VehicleCoverMotionState.Fast;
				}
			}
			if (this.PlayerAnimator != null && this._hasVehicleSpeedParam)
			{
				float @float = this.PlayerAnimator.GetFloat(this._vehicleSpeedParamHash);
				if (@float >= 0.7f)
				{
					return VehicleCoverMotionState.Fast;
				}
				if (@float >= 0.1f)
				{
					return VehicleCoverMotionState.Moving;
				}
			}
			return VehicleCoverMotionState.Idle;
		}

		// Token: 0x06009235 RID: 37429 RVA: 0x003BFA7C File Offset: 0x003BDC7C
		private bool TryGetVehicleNormalizedSpeed(out float normalizedSpeed)
		{
			normalizedSpeed = 0f;
			if (this.PlayerAnimator != null && this._hasVehicleSpeedParam)
			{
				normalizedSpeed = Mathf.Clamp01(this.PlayerAnimator.GetFloat(this._vehicleSpeedParamHash));
				return true;
			}
			if (this._vehiclePoseDriver != null)
			{
				float num = CoverPoseController.MotionStateToNormalizedSpeed(this._vehiclePoseDriver.CurrentState);
				float num2 = CoverPoseController.MotionStateToNormalizedSpeed(this._vehiclePoseDriver.TargetState);
				float num3 = (this._vehiclePoseDriver.IsBlending ? this._vehiclePoseDriver.BlendProgress : 1f);
				normalizedSpeed = Mathf.Lerp(num, num2, Mathf.Clamp01(num3));
				return true;
			}
			return false;
		}

		// Token: 0x06009236 RID: 37430 RVA: 0x003BFB22 File Offset: 0x003BDD22
		private bool IsManualCoverIKTargetEditingActive()
		{
			return (!this._isRidingVehicle || !this._vehicleAllowsCover || this._manualCoverIKAuthoringOverride) && this._manualCoverIKTargetEditing;
		}

		// Token: 0x06009237 RID: 37431 RVA: 0x003BFB44 File Offset: 0x003BDD44
		private void SyncOverrideFromRigTargets(CoverIKOverride ik)
		{
			if (ik == null || this.rigController == null)
			{
				return;
			}
			Transform transform = ((ik.referenceMode == IKReferenceMode.CustomBone && ik.customReferenceBone != null) ? ik.customReferenceBone : this.ikReferenceBone);
			if (transform == null)
			{
				return;
			}
			Transform coverLimbTarget = this.rigController.GetCoverLimbTarget(ik.limb);
			if (coverLimbTarget != null)
			{
				ik.localPosition = transform.InverseTransformPoint(coverLimbTarget.position);
				ik.rotationOffset = (Quaternion.Inverse(transform.rotation) * coverLimbTarget.rotation).eulerAngles;
			}
			Transform coverLimbHint = this.rigController.GetCoverLimbHint(ik.limb);
			if (coverLimbHint != null)
			{
				ik.hintLocalPosition = transform.InverseTransformPoint(coverLimbHint.position);
			}
		}

		// Token: 0x06009238 RID: 37432 RVA: 0x003BFC11 File Offset: 0x003BDE11
		private static float MotionStateToNormalizedSpeed(VehiclePoseState state)
		{
			if (state == VehiclePoseState.MovingLowSpeed)
			{
				return 0.5f;
			}
			if (state == VehiclePoseState.MovingHighSpeed)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06009239 RID: 37433 RVA: 0x003BFC2C File Offset: 0x003BDE2C
		private VehicleCoverMotionState EvaluateMotionStateWithHysteresis(float normalizedSpeed)
		{
			float num = Mathf.Clamp01(Mathf.Max(this.movingEnterSpeedNormalized, this.movingExitSpeedNormalized));
			float num2 = Mathf.Clamp01(Mathf.Min(this.movingEnterSpeedNormalized, this.movingExitSpeedNormalized));
			float num3 = Mathf.Clamp01(Mathf.Max(this.fastEnterSpeedNormalized, this.fastExitSpeedNormalized));
			float num4 = Mathf.Clamp01(Mathf.Min(this.fastEnterSpeedNormalized, this.fastExitSpeedNormalized));
			switch (this._vehicleMotionStateHysteresis)
			{
			case VehicleCoverMotionState.Idle:
				if (normalizedSpeed >= num)
				{
					this._vehicleMotionStateHysteresis = VehicleCoverMotionState.Moving;
				}
				break;
			case VehicleCoverMotionState.Moving:
				if (normalizedSpeed >= num3)
				{
					this._vehicleMotionStateHysteresis = VehicleCoverMotionState.Fast;
				}
				else if (normalizedSpeed <= num2)
				{
					this._vehicleMotionStateHysteresis = VehicleCoverMotionState.Idle;
				}
				break;
			case VehicleCoverMotionState.Fast:
				if (normalizedSpeed <= num4)
				{
					this._vehicleMotionStateHysteresis = VehicleCoverMotionState.Moving;
				}
				break;
			}
			return this._vehicleMotionStateHysteresis;
		}

		// Token: 0x0600923A RID: 37434 RVA: 0x003BFCEC File Offset: 0x003BDEEC
		private void UpdateVehicleMotionStateBlending()
		{
			if (!this._isRidingVehicle)
			{
				this._vehicleMotionStateInitialized = false;
				this._vehicleMotionStateCurrent = VehicleCoverMotionState.Idle;
				this._vehicleMotionStatePrevious = VehicleCoverMotionState.Idle;
				this._vehicleMotionStateHysteresis = VehicleCoverMotionState.Idle;
				this._vehicleMotionStateBlend = 1f;
				return;
			}
			VehicleCoverMotionState vehicleMotionState = this.GetVehicleMotionState();
			if (!this._vehicleMotionStateInitialized)
			{
				this.ResetVehicleMotionSmoothing(vehicleMotionState);
				return;
			}
			if (this._vehiclePoseDriver != null && this._vehiclePoseDriver.IsBlending)
			{
				this._vehicleMotionStateCurrent = CoverPoseController.FromVehiclePoseState(this._vehiclePoseDriver.TargetState);
				this._vehicleMotionStatePrevious = CoverPoseController.FromVehiclePoseState(this._vehiclePoseDriver.CurrentState);
				this._vehicleMotionStateBlend = 1f;
				this._vehicleMotionStateHysteresis = this._vehicleMotionStateCurrent;
				if (this.DebugMode && Time.frameCount % 30 == 0)
				{
					Debug.Log(string.Format("[CoverPoseController] Synced with VehiclePoseDriver blend: {0} -> {1}, progress={2:F2}", this._vehiclePoseDriver.CurrentState, this._vehiclePoseDriver.TargetState, this._vehiclePoseDriver.BlendProgress));
				}
				return;
			}
			if (vehicleMotionState != this._vehicleMotionStateCurrent)
			{
				this._vehicleMotionStatePrevious = this._vehicleMotionStateCurrent;
				this._vehicleMotionStateCurrent = vehicleMotionState;
				this._vehicleMotionStateBlend = 0f;
			}
			if (this._vehicleMotionStateBlend < 1f)
			{
				float num = Mathf.Max(0.02f, this.vehicleMotionStateBlendDuration);
				this._vehicleMotionStateBlend = Mathf.MoveTowards(this._vehicleMotionStateBlend, 1f, Time.deltaTime / num);
				if (this.DebugMode && Time.frameCount % 10 == 0)
				{
					Debug.Log(string.Format("[CoverPoseController] Motion State Blend: {0} -> {1}, blendT={2:F2}", this._vehicleMotionStatePrevious, this._vehicleMotionStateCurrent, this._vehicleMotionStateBlend));
				}
			}
		}

		// Token: 0x0600923B RID: 37435 RVA: 0x003BFE9A File Offset: 0x003BE09A
		private void ResetVehicleMotionSmoothing(VehicleCoverMotionState initialState)
		{
			this._vehicleMotionStateInitialized = true;
			this._vehicleMotionStateCurrent = initialState;
			this._vehicleMotionStatePrevious = initialState;
			this._vehicleMotionStateHysteresis = initialState;
			this._vehicleMotionStateBlend = 1f;
		}

		// Token: 0x0600923C RID: 37436 RVA: 0x003BFEC4 File Offset: 0x003BE0C4
		private bool IsExhibitCoverPoseActive()
		{
			if (this.ForceEmotion != CoverEmotionOverride.None)
			{
				return this.ForceEmotion == CoverEmotionOverride.Confident;
			}
			if (this._isCovering && this.PlayerAnimator != null && this._hasCoverPoseParam)
			{
				return this.PlayerAnimator.GetInteger(this.CoverPoseParamName) == this.ConfidentCoverPoseIndex;
			}
			return PlayerEmotionController.Instance != null && PlayerEmotionController.Instance.IsExhibitionDominant;
		}

		// Token: 0x0600923D RID: 37437 RVA: 0x003BFF34 File Offset: 0x003BE134
		private VehiclePosePreset GetActiveVehicleCoverPosePreset()
		{
			if (this._vehiclePosePreset != null)
			{
				return this._vehiclePosePreset;
			}
			if (this._vehiclePoseDriver != null)
			{
				return this._vehiclePoseDriver.PosePreset;
			}
			return null;
		}

		// Token: 0x0600923E RID: 37438 RVA: 0x003BF76B File Offset: 0x003BD96B
		private static VehiclePoseState ToVehiclePoseState(VehicleCoverMotionState state)
		{
			if (state == VehicleCoverMotionState.Moving)
			{
				return VehiclePoseState.MovingLowSpeed;
			}
			if (state != VehicleCoverMotionState.Fast)
			{
				return VehiclePoseState.Idle;
			}
			return VehiclePoseState.MovingHighSpeed;
		}

		// Token: 0x0600923F RID: 37439 RVA: 0x003BF76B File Offset: 0x003BD96B
		private static VehicleCoverMotionState FromVehiclePoseState(VehiclePoseState state)
		{
			if (state == VehiclePoseState.MovingLowSpeed)
			{
				return VehicleCoverMotionState.Moving;
			}
			if (state != VehiclePoseState.MovingHighSpeed)
			{
				return VehicleCoverMotionState.Idle;
			}
			return VehicleCoverMotionState.Fast;
		}

		// Token: 0x06009240 RID: 37440 RVA: 0x003BFF68 File Offset: 0x003BE168
		private void GetBlendedPresetOffsets(VehiclePosePreset preset, bool exhibit, out VehicleCoverPoseStateOffsets fromOffsets, out VehicleCoverPoseStateOffsets toOffsets, out float blendT)
		{
			VehiclePoseState vehiclePoseState = CoverPoseController.ToVehiclePoseState(this._vehicleMotionStatePrevious);
			VehiclePoseState vehiclePoseState2 = CoverPoseController.ToVehiclePoseState(this._vehicleMotionStateCurrent);
			blendT = 1f;
			if (this._vehiclePoseDriver != null && this._vehiclePoseDriver.IsBlending)
			{
				vehiclePoseState = this._vehiclePoseDriver.CurrentState;
				vehiclePoseState2 = this._vehiclePoseDriver.TargetState;
				blendT = this._vehiclePoseDriver.BlendProgress;
			}
			else if (!this._vehicleMotionStateInitialized)
			{
				vehiclePoseState = (vehiclePoseState2 = CoverPoseController.ToVehiclePoseState(this.GetVehicleMotionState()));
				blendT = 1f;
			}
			else if (this._vehicleMotionStateBlend < 1f)
			{
				blendT = Mathf.SmoothStep(0f, 1f, this._vehicleMotionStateBlend);
			}
			fromOffsets = preset.GetCoverOffsets(vehiclePoseState, exhibit);
			toOffsets = preset.GetCoverOffsets(vehiclePoseState2, exhibit);
			if (fromOffsets == null)
			{
				fromOffsets = toOffsets;
			}
			if (toOffsets == null)
			{
				toOffsets = fromOffsets;
			}
		}

		// Token: 0x06009241 RID: 37441 RVA: 0x003C0048 File Offset: 0x003BE248
		private static void GetPresetLimbOffsets(VehicleCoverPoseStateOffsets stateOffsets, LimbType limb, out Vector3 position, out Vector3 rotation, out Vector3 hint)
		{
			if (stateOffsets == null)
			{
				position = Vector3.zero;
				rotation = Vector3.zero;
				hint = Vector3.zero;
				return;
			}
			if (limb == LimbType.LeftHand)
			{
				position = stateOffsets.leftHandLocalPositionOffset;
				rotation = stateOffsets.leftHandRotationOffset;
				hint = stateOffsets.leftHandHintLocalOffset;
				return;
			}
			position = stateOffsets.rightHandLocalPositionOffset;
			rotation = stateOffsets.rightHandRotationOffset;
			hint = stateOffsets.rightHandHintLocalOffset;
		}

		// Token: 0x06009242 RID: 37442 RVA: 0x003C00CC File Offset: 0x003BE2CC
		private void ApplyVehicleHandIKAdjustments(LimbType limb, ref Transform referenceBone, ref Vector3 localPosition, ref Vector3 rotationOffset, ref Vector3 hintLocalPosition)
		{
			if (!this._isCovering || !this._isRidingVehicle || !this._vehicleAllowsCover)
			{
				return;
			}
			if (limb != LimbType.LeftHand && limb != LimbType.RightHand)
			{
				return;
			}
			VehiclePosePreset activeVehicleCoverPosePreset = this.GetActiveVehicleCoverPosePreset();
			if (activeVehicleCoverPosePreset == null || activeVehicleCoverPosePreset.coverPoseIK == null || !activeVehicleCoverPosePreset.coverPoseIK.enabled)
			{
				return;
			}
			Transform transform = ((this.rigController != null) ? this.rigController.CoverIKAnchor : null);
			if (transform != null)
			{
				referenceBone = transform;
			}
			bool flag = this.IsExhibitCoverPoseActive();
			VehicleCoverPoseStateOffsets vehicleCoverPoseStateOffsets;
			VehicleCoverPoseStateOffsets vehicleCoverPoseStateOffsets2;
			float num;
			this.GetBlendedPresetOffsets(activeVehicleCoverPosePreset, flag, out vehicleCoverPoseStateOffsets, out vehicleCoverPoseStateOffsets2, out num);
			if (vehicleCoverPoseStateOffsets == null && vehicleCoverPoseStateOffsets2 == null)
			{
				return;
			}
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			CoverPoseController.GetPresetLimbOffsets(vehicleCoverPoseStateOffsets, limb, out vector, out vector2, out vector3);
			Vector3 vector4;
			Vector3 vector5;
			Vector3 vector6;
			CoverPoseController.GetPresetLimbOffsets(vehicleCoverPoseStateOffsets2, limb, out vector4, out vector5, out vector6);
			localPosition = Vector3.Lerp(vector, vector4, num);
			Quaternion quaternion = Quaternion.Euler(vector2);
			Quaternion quaternion2 = Quaternion.Euler(vector5);
			rotationOffset = Quaternion.Lerp(quaternion, quaternion2, num).eulerAngles;
			hintLocalPosition = Vector3.Lerp(vector3, vector6, num);
			if (this.DebugMode && Time.frameCount - this._lastVehicleIKDebugFrame > 30)
			{
				this._lastVehicleIKDebugFrame = Time.frameCount;
				Debug.Log(string.Format("[CoverPoseController] VehicleCoverIK preset, state={0}, blend={1:F2}, emotion={2}, limb={3}, anchor={4}", new object[]
				{
					this._vehicleMotionStateCurrent,
					num,
					flag ? "Exhibit" : "Embarrassed",
					limb,
					((transform != null) ? transform.name : null) ?? "null"
				}));
			}
		}

		// Token: 0x06009243 RID: 37443 RVA: 0x003C0254 File Offset: 0x003BE454
		private bool HasAnyCoverIKWeight(float minWeight)
		{
			return CoverPoseController.HasAnyIKWeightInList(this.fullCoverEmbarrassedIK, minWeight) || CoverPoseController.HasAnyIKWeightInList(this.fullCoverExhibitIK, minWeight);
		}

		// Token: 0x06009244 RID: 37444 RVA: 0x003C0274 File Offset: 0x003BE474
		private static bool HasAnyIKWeightInList(List<CoverIKOverride> overrides, float minWeight)
		{
			if (overrides == null || overrides.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < overrides.Count; i++)
			{
				if (overrides[i] != null && overrides[i].currentWeight > minWeight)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009245 RID: 37445 RVA: 0x003C02BC File Offset: 0x003BE4BC
		private void UpdateVehicleLimbConstraintSuppression()
		{
			if (this.rigController == null || !this._rigInitialized)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (this._isRidingVehicle && this._vehicleAllowsCover)
			{
				if (this._isCovering)
				{
					flag = this._targetArmsWeight + this._targetLeftArmWeight > 0.01f;
					flag2 = this._targetArmsWeight + this._targetRightArmWeight > 0.01f;
					this._releasingLeftCoverArm = flag;
					this._releasingRightCoverArm = flag2;
				}
				else
				{
					bool flag3 = this.IsCoverLayerStillAnimating(this._armsLayerIndex, true);
					flag = this._releasingLeftCoverArm && (flag3 || this.IsCoverLayerStillAnimating(this._leftArmLayerIndex, true));
					flag2 = this._releasingRightCoverArm && (flag3 || this.IsCoverLayerStillAnimating(this._rightArmLayerIndex, true));
					this._releasingLeftCoverArm = flag;
					this._releasingRightCoverArm = flag2;
				}
				flag |= this.GetCoverHandIKWeight(LimbType.LeftHand) > 0.01f;
				flag2 |= this.GetCoverHandIKWeight(LimbType.RightHand) > 0.01f;
			}
			this.rigController.SetVehicleLimbSuppressed(LimbType.LeftHand, flag);
			this.rigController.SetVehicleLimbSuppressed(LimbType.RightHand, flag2);
		}

		// Token: 0x06009246 RID: 37446 RVA: 0x003C03D4 File Offset: 0x003BE5D4
		private bool IsCoverLayerStillAnimating(int layer, bool requireLayerWeight = true)
		{
			return !(this.PlayerAnimator == null) && !(this.PlayerAnimator.runtimeAnimatorController == null) && this.PlayerAnimator.isInitialized && layer >= 0 && layer < this.PlayerAnimator.layerCount && (!requireLayerWeight || this.PlayerAnimator.GetLayerWeight(layer) > 0.01f) && (this.PlayerAnimator.GetCurrentAnimatorClipInfoCount(layer) > 0 || (this.PlayerAnimator.IsInTransition(layer) && this.PlayerAnimator.GetNextAnimatorClipInfoCount(layer) > 0));
		}

		// Token: 0x06009247 RID: 37447 RVA: 0x003C046C File Offset: 0x003BE66C
		private bool TryResolveCoverActivation(out bool exhibit, out int resolvedVariant, out string denialReason)
		{
			string text;
			string text2;
			return this.TryResolveCoverActivation(out exhibit, out resolvedVariant, out denialReason, out text, out text2);
		}

		// Token: 0x06009248 RID: 37448 RVA: 0x003C0488 File Offset: 0x003BE688
		private bool TryResolveCoverActivation(out bool exhibit, out int resolvedVariant, out string denialReason, out string denialTemplate, out string denialLabel)
		{
			exhibit = this.IsExhibitCoverPoseActive();
			resolvedVariant = (exhibit ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex);
			denialReason = null;
			denialTemplate = null;
			denialLabel = null;
			if (PlayerActivityGate.IsBlocked(PlayerActivity.Cover | PlayerActivity.ArmsPose))
			{
				denialReason = "Finish what Ciel is doing before posing.";
				return false;
			}
			if (!TimeFreezeController.IsAnyActive && ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive)
			{
				denialReason = "Wait until the conversation is over before posing.";
				return false;
			}
			if (PlayerInventoryManager.Instance != null && PlayerInventoryManager.Instance.GetActiveItem() != null)
			{
				denialReason = "Put the item away to free Ciel's hands first.";
				return false;
			}
			if (this.respectVehicleCoverRestrictions && this._isRidingVehicle && !this._vehicleAllowsCover)
			{
				denialReason = "Get off the vehicle before using this pose.";
				return false;
			}
			if (this.IsPoseAvailable(exhibit, resolvedVariant))
			{
				return true;
			}
			resolvedVariant = this.FirstAvailablePoseIndex(exhibit);
			if (resolvedVariant >= 0)
			{
				return true;
			}
			denialTemplate = this.GetPoseUnavailableTemplate(exhibit, exhibit ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex, out denialLabel);
			denialReason = string.Format(denialTemplate, denialLabel);
			return false;
		}

		// Token: 0x06009249 RID: 37449 RVA: 0x003C0590 File Offset: 0x003BE790
		private string GetPoseUnavailableTemplate(bool exhibit, int index, out string label)
		{
			List<CoverPoseBlendShapeSet> list = (exhibit ? this.ConfidentBlendShapesPerPose : this.EmbarrassedBlendShapesPerPose);
			CoverPoseBlendShapeSet coverPoseBlendShapeSet = ((list != null && index >= 0 && index < list.Count) ? list[index] : null);
			label = ((coverPoseBlendShapeSet != null && !string.IsNullOrWhiteSpace(coverPoseBlendShapeSet.Label)) ? coverPoseBlendShapeSet.Label : "This pose");
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			if (coverPoseBlendShapeSet != null && currentRank != null && ((currentRank.rankId == "curious" && coverPoseBlendShapeSet.CuriousAccess == CuriousCoverPoseAccess.Locked) || (coverPoseBlendShapeSet.RankRequirement == CoverPoseRankRequirement.BraRemoval && !currentRank.allowBraRemoval && !currentRank.allowFullUndress) || (coverPoseBlendShapeSet.RankRequirement == CoverPoseRankRequirement.FullUndress && !currentRank.allowFullUndress)))
			{
				return "{0} isn't unlocked at Ciel's current rank yet.";
			}
			if (coverPoseBlendShapeSet != null && this.ClothingController != null)
			{
				if (coverPoseBlendShapeSet.RequiresTopRemoved && this.ClothingController.HasVisibleUpperGarment)
				{
					return "Take off Ciel's top before using {0}.";
				}
				if (currentRank != null && currentRank.rankId == "curious" && coverPoseBlendShapeSet.CuriousAccess == CuriousCoverPoseAccess.RequiresOpenBra && !this.ClothingController.IsWornBraOpen)
				{
					return "Open Ciel's bra before using {0}.";
				}
				bool flag;
				bool flag2;
				this.ClothingController.GetCurrentPhysicalCoverage(out flag, out flag2);
				if (coverPoseBlendShapeSet.Coverage != CoverPoseCoverage.CoversNothing && flag)
				{
					return "{0} is unavailable while Ciel's breasts are covered. Uncover them first.";
				}
				if (coverPoseBlendShapeSet.Coverage == CoverPoseCoverage.CoversTopAndBottom && flag2)
				{
					return "{0} is unavailable while Ciel's crotch is covered. Uncover it first.";
				}
			}
			return "{0} isn't available right now. Try another pose.";
		}

		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x0600924A RID: 37450 RVA: 0x003C06EC File Offset: 0x003BE8EC
		public bool CanActivateCoverPose
		{
			get
			{
				bool flag;
				int num;
				string text;
				return base.isActiveAndEnabled && (this._isCovering || (!(this.PlayerAnimator == null) && (this._armsLayerIndex >= 0 || this._rightArmLayerIndex >= 0 || this._leftArmLayerIndex >= 0) && this.TryResolveCoverActivation(out flag, out num, out text)));
			}
		}

		// Token: 0x0600924B RID: 37451 RVA: 0x003C0744 File Offset: 0x003BE944
		public void ActivateCoverPose()
		{
			this.ActivateCoverPoseInternal(false);
		}

		// Token: 0x0600924C RID: 37452 RVA: 0x003C074D File Offset: 0x003BE94D
		public void ActivateCoverPoseFromInput()
		{
			this.ActivateCoverPoseInternal(true);
		}

		// Token: 0x0600924D RID: 37453 RVA: 0x003C0758 File Offset: 0x003BE958
		private void ActivateCoverPoseInternal(bool showFeedback)
		{
			if (!base.isActiveAndEnabled || this.PlayerAnimator == null)
			{
				if (showFeedback)
				{
					BlockedActionFeedback.Show("Put away the held item before using a cover pose.");
				}
				return;
			}
			if (this._armsLayerIndex < 0 && this._rightArmLayerIndex < 0 && this._leftArmLayerIndex < 0)
			{
				return;
			}
			if (this._isCovering)
			{
				this.ValidateActivePose();
				if (!this._isCovering)
				{
					return;
				}
			}
			if (!this._isCovering)
			{
				bool flag;
				int num;
				string text;
				string denialTemplate;
				string denialLabel;
				if (!this.TryResolveCoverActivation(out flag, out num, out text, out denialTemplate, out denialLabel))
				{
					if (this.DebugMode)
					{
						Debug.Log("[CoverPoseController] Cover pose denied: " + text + ".");
					}
					if (showFeedback)
					{
						if (denialTemplate == null)
						{
							BlockedActionFeedback.Show(text);
							return;
						}
						BlockedActionFeedback.ShowComputed(() => GameLocalization.FormatSource("gameplay_prompts", denialTemplate, new object[] { GameLocalization.GetSource("gameplay_prompts", denialLabel) }));
					}
					return;
				}
				if (flag)
				{
					this._confidentPoseTypeIndex = num;
				}
				else
				{
					this._embarrassedPoseTypeIndex = num;
				}
			}
			int coverPoseForCurrentState = this.GetCoverPoseForCurrentState();
			if (this._isCovering)
			{
				if (this._hasCoverPoseParam && this.PlayerAnimator.GetInteger(this.CoverPoseParamName) != coverPoseForCurrentState)
				{
					this.PlayerAnimator.SetInteger(this.CoverPoseParamName, coverPoseForCurrentState);
					this.ApplyCoverPoseTypeToAnimator();
					this.RefreshActiveBlendShapesForCurrentPose();
					this.RefreshMagicaClothForCurrentPose();
					this.ApplyCoverType();
					if (this.DebugMode)
					{
						string text2 = ((coverPoseForCurrentState == this.EmbarrassedCoverPoseIndex) ? "Embarrassed" : "Confident");
						Debug.Log(string.Format("[CoverPoseController] Updated cover pose to {0} (index: {1}) while already covering", text2, coverPoseForCurrentState));
					}
				}
				return;
			}
			if (this._hasCoverPoseParam)
			{
				this.PlayerAnimator.SetInteger(this.CoverPoseParamName, coverPoseForCurrentState);
			}
			this.ApplyCoverPoseTypeToAnimator();
			if (this._hasIsCoveringParam)
			{
				this.PlayerAnimator.SetBool(this.IsCoveringParamName, true);
			}
			if (this._hasCoverTriggerParam)
			{
				this.PlayerAnimator.SetTrigger(this.CoverTriggerName);
			}
			this.DisposeCoverStreamRelease();
			this._isCovering = true;
			Action onCoverStarted = this.OnCoverStarted;
			if (onCoverStarted != null)
			{
				onCoverStarted();
			}
			this.UpdateVehicleLimbConstraintSuppression();
			if (this.stimulationVoice != null && CielVoiceController.Instance != null && !string.IsNullOrEmpty(this.stimulationPoseLabel) && string.Equals(this.GetCurrentEmotionPoseLabel(), this.stimulationPoseLabel, StringComparison.OrdinalIgnoreCase))
			{
				CielVoiceController.Instance.Play(this.stimulationVoice);
				this._stimulationVoicePlaying = true;
			}
			this.ApplyCoverType();
			this.RefreshActiveBlendShapesForCurrentPose();
			this.RefreshMagicaClothForCurrentPose();
			if (this.DebugMode)
			{
				string text3 = ((coverPoseForCurrentState == this.EmbarrassedCoverPoseIndex) ? "Embarrassed" : "Confident");
				int num2 = ((this._activeBlendShapes != null) ? this._activeBlendShapes.Count : 0);
				Debug.Log(string.Format("[CoverPoseController] Activated {0} cover pose (index: {1}, coverType: {2}) with {3} blend shapes", new object[] { text3, coverPoseForCurrentState, this._currentCoverType, num2 }));
			}
		}

		// Token: 0x0600924E RID: 37454 RVA: 0x003C0A18 File Offset: 0x003BEC18
		public void DeactivateCoverPose()
		{
			if (this._stimulationVoicePlaying && CielVoiceController.Instance != null && this.stimulationVoice != null && CielVoiceController.Instance.CurrentVoice == this.stimulationVoice)
			{
				CielVoiceController.Instance.Stop();
			}
			this._stimulationVoicePlaying = false;
			if (this.PlayerAnimator != null && this._hasIsCoveringParam)
			{
				this.PlayerAnimator.SetBool(this.IsCoveringParamName, false);
			}
			if (this.PlayerAnimator != null && this._hasCoverPoseParam)
			{
				this.PlayerAnimator.SetInteger(this.CoverPoseParamName, 0);
			}
			this._currentCoverType = CoverType.BoobsCover;
			this._coverTypeTarget = 0f;
			this._activeIKOverrides = null;
			bool isCovering = this._isCovering;
			this._coverAnimationStreamNeedsReset = this._coverAnimationStreamNeedsReset || isCovering;
			this._isCovering = false;
			this._endedPoseDeformationCleanupPending = this._endedPoseDeformationCleanupPending || isCovering;
			if (isCovering)
			{
				Action onCoverEnded = this.OnCoverEnded;
				if (onCoverEnded != null)
				{
					onCoverEnded();
				}
			}
			this.UpdateVehicleLimbConstraintSuppression();
			this._activeBlendShapes = null;
			if (!base.isActiveAndEnabled || this.PlayerAnimator == null || !this.PlayerAnimator.isActiveAndEnabled)
			{
				CoverPoseController.ClearBlendShapeOverrides(this.EmbarrassedBlendShapesPerPose);
				CoverPoseController.ClearBlendShapeOverrides(this.ConfidentBlendShapesPerPose);
			}
			this.EnableMagicaClothObjects();
			if (this.DebugMode)
			{
				Debug.Log("[CoverPoseController] Deactivated cover pose (Animator params first, IK blending out smoothly)");
			}
		}

		// Token: 0x0600924F RID: 37455 RVA: 0x003C0B78 File Offset: 0x003BED78
		public void UpdateCoverPoseForCurrentState()
		{
			if (!this._isCovering || this.PlayerAnimator == null)
			{
				return;
			}
			int coverPoseForCurrentState = this.GetCoverPoseForCurrentState();
			if (this._hasCoverPoseParam && this.PlayerAnimator.GetInteger(this.CoverPoseParamName) != coverPoseForCurrentState)
			{
				this.PlayerAnimator.SetInteger(this.CoverPoseParamName, coverPoseForCurrentState);
				this.ApplyCoverPoseTypeToAnimator();
				this.RefreshActiveBlendShapesForCurrentPose();
				this.RefreshMagicaClothForCurrentPose();
				this.ApplyCoverType();
				if (this.DebugMode)
				{
					string text = ((coverPoseForCurrentState == this.EmbarrassedCoverPoseIndex) ? "Embarrassed" : "Confident");
					Debug.Log(string.Format("[CoverPoseController] Updated to {0} cover pose (index: {1})", text, coverPoseForCurrentState));
				}
			}
		}

		// Token: 0x06009250 RID: 37456 RVA: 0x003C0C20 File Offset: 0x003BEE20
		private void DisableMagicaClothObjects()
		{
			if (this._magicaClothDisabled)
			{
				return;
			}
			if (this.magicaClothObjects == null || this.magicaClothObjects.Length == 0)
			{
				return;
			}
			foreach (GameObject gameObject in this.magicaClothObjects)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
					if (this.DebugMode)
					{
						Debug.Log("[CoverPoseController] Disabled MagicaCloth object: " + gameObject.name);
					}
				}
			}
			this._magicaClothDisabled = true;
		}

		// Token: 0x06009251 RID: 37457 RVA: 0x003C0C98 File Offset: 0x003BEE98
		private void EnableMagicaClothObjects()
		{
			if (!this._magicaClothDisabled)
			{
				return;
			}
			if (this.magicaClothObjects == null || this.magicaClothObjects.Length == 0)
			{
				return;
			}
			foreach (GameObject gameObject in this.magicaClothObjects)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
					if (this.DebugMode)
					{
						Debug.Log("[CoverPoseController] Re-enabled MagicaCloth object: " + gameObject.name);
					}
				}
			}
			this._magicaClothDisabled = false;
		}

		// Token: 0x06009252 RID: 37458 RVA: 0x003C0D0D File Offset: 0x003BEF0D
		public void SetMagicaClothObjects(GameObject[] objects)
		{
			this.magicaClothObjects = objects;
		}

		// Token: 0x06009253 RID: 37459 RVA: 0x003C0D18 File Offset: 0x003BEF18
		public void AddMagicaClothObject(GameObject obj)
		{
			if (obj == null)
			{
				return;
			}
			List<GameObject> list = new List<GameObject>(this.magicaClothObjects ?? new GameObject[0]);
			if (!list.Contains(obj))
			{
				list.Add(obj);
				this.magicaClothObjects = list.ToArray();
			}
		}

		// Token: 0x06009254 RID: 37460 RVA: 0x003C0D64 File Offset: 0x003BEF64
		private int GetCoverPoseForCurrentState()
		{
			if (this.ForceEmotion != CoverEmotionOverride.None)
			{
				if (this.ForceEmotion != CoverEmotionOverride.Confident)
				{
					return this.EmbarrassedCoverPoseIndex;
				}
				return this.ConfidentCoverPoseIndex;
			}
			else
			{
				if (PlayerEmotionController.Instance == null)
				{
					return this.EmbarrassedCoverPoseIndex;
				}
				if (PlayerEmotionController.Instance.IsExhibitionDominant)
				{
					return this.ConfidentCoverPoseIndex;
				}
				return this.EmbarrassedCoverPoseIndex;
			}
		}

		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x06009255 RID: 37461 RVA: 0x003C0DBD File Offset: 0x003BEFBD
		public bool IsCovering
		{
			get
			{
				return this._isCovering;
			}
		}

		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x06009256 RID: 37462 RVA: 0x003C0DC5 File Offset: 0x003BEFC5
		public int ArmsPoseTransitionKey
		{
			get
			{
				return (int)((((this._isCovering ? CoverType.FullCover : CoverType.BoobsCover) * (CoverType)397 + this.CurrentEmotionPoseTypeIndex) * (CoverType)397 + (int)this._currentCoverType) * (CoverType)397 + (this.IsExhibitDominant ? 1 : 0));
			}
		}

		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x06009257 RID: 37463 RVA: 0x003C0E00 File Offset: 0x003BF000
		public bool HasReadyArmsAnimation
		{
			get
			{
				if (!this._isCovering || this.PlayerAnimator == null || !this.PlayerAnimator.isInitialized)
				{
					return false;
				}
				bool flag = false;
				return this.IsArmDestinationReady(this._armsLayerIndex, ref flag) && this.IsArmDestinationReady(this._rightArmLayerIndex, ref flag) && this.IsArmDestinationReady(this._leftArmLayerIndex, ref flag) && flag;
			}
		}

		// Token: 0x06009258 RID: 37464 RVA: 0x003C0E68 File Offset: 0x003BF068
		private bool IsArmDestinationReady(int layer, ref bool any)
		{
			if (layer < 0 || layer >= this.PlayerAnimator.layerCount || this.PlayerAnimator.GetLayerWeight(layer) <= 0.01f)
			{
				return true;
			}
			any = true;
			if (!this.PlayerAnimator.IsInTransition(layer))
			{
				return this.PlayerAnimator.GetCurrentAnimatorClipInfoCount(layer) > 0;
			}
			return this.PlayerAnimator.GetNextAnimatorClipInfoCount(layer) > 0;
		}

		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x06009259 RID: 37465 RVA: 0x003C0ECC File Offset: 0x003BF0CC
		public bool HasActiveArmsAnimation
		{
			get
			{
				return this._isCovering || this.IsCoverLayerStillAnimating(this._armsLayerIndex, true) || this.IsCoverLayerStillAnimating(this._rightArmLayerIndex, true) || this.IsCoverLayerStillAnimating(this._leftArmLayerIndex, true) || this.HasAnyCoverIKWeight(0.01f);
			}
		}

		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x0600925A RID: 37466 RVA: 0x003C0F1B File Offset: 0x003BF11B
		public CoverType CurrentCoverType
		{
			get
			{
				return this._currentCoverType;
			}
		}

		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x0600925B RID: 37467 RVA: 0x003C0F23 File Offset: 0x003BF123
		public bool IsCoverAllowed
		{
			get
			{
				return !this.respectVehicleCoverRestrictions || !this._isRidingVehicle || this._vehicleAllowsCover;
			}
		}

		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x0600925C RID: 37468 RVA: 0x003C0F40 File Offset: 0x003BF140
		public bool HasAvailablePose
		{
			get
			{
				return this.FirstAvailablePoseIndex(false) >= 0 || this.FirstAvailablePoseIndex(true) >= 0;
			}
		}

		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x0600925D RID: 37469 RVA: 0x003C0F5B File Offset: 0x003BF15B
		public bool IsExhibitDominant
		{
			get
			{
				return this.IsExhibitCoverPoseActive();
			}
		}

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x0600925E RID: 37470 RVA: 0x003C0F63 File Offset: 0x003BF163
		public int CurrentEmotionPoseTypeIndex
		{
			get
			{
				if (!this.IsExhibitCoverPoseActive())
				{
					return this._embarrassedPoseTypeIndex;
				}
				return this._confidentPoseTypeIndex;
			}
		}

		// Token: 0x0600925F RID: 37471 RVA: 0x003C0F7C File Offset: 0x003BF17C
		public string GetCurrentEmotionPoseLabel()
		{
			bool flag = this.IsExhibitCoverPoseActive();
			int num = (flag ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex);
			List<CoverPoseBlendShapeSet> list = (flag ? this.ConfidentBlendShapesPerPose : this.EmbarrassedBlendShapesPerPose);
			if (list == null || num < 0 || num >= list.Count)
			{
				return num.ToString();
			}
			CoverPoseBlendShapeSet coverPoseBlendShapeSet = list[num];
			if (coverPoseBlendShapeSet == null || string.IsNullOrEmpty(coverPoseBlendShapeSet.Label))
			{
				return num.ToString();
			}
			return coverPoseBlendShapeSet.Label;
		}

		// Token: 0x14000100 RID: 256
		// (add) Token: 0x06009260 RID: 37472 RVA: 0x003C0FF0 File Offset: 0x003BF1F0
		// (remove) Token: 0x06009261 RID: 37473 RVA: 0x003C1028 File Offset: 0x003BF228
		public event Action OnEmotionPoseSelectionChanged;

		// Token: 0x06009262 RID: 37474 RVA: 0x003C105D File Offset: 0x003BF25D
		public int GetEmotionPoseTypeCount(bool exhibit)
		{
			if (!exhibit)
			{
				return Mathf.Max(1, this.EmbarrassedPoseTypeCount);
			}
			return Mathf.Max(1, this.ConfidentPoseTypeCount);
		}

		// Token: 0x06009263 RID: 37475 RVA: 0x003C107C File Offset: 0x003BF27C
		public bool IsPoseAvailable(bool exhibit, int index)
		{
			List<CoverPoseBlendShapeSet> list = (exhibit ? this.ConfidentBlendShapesPerPose : this.EmbarrassedBlendShapesPerPose);
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			if (currentRank != null && currentRank.rankId == "curious")
			{
				if (list == null || index < 0 || index >= list.Count || list[index] == null)
				{
					return false;
				}
				CuriousCoverPoseAccess curiousAccess = list[index].CuriousAccess;
				if (curiousAccess != CuriousCoverPoseAccess.Available)
				{
					if (curiousAccess != CuriousCoverPoseAccess.RequiresOpenBra)
					{
						return false;
					}
					if (this.ClothingController == null || !this.ClothingController.IsWornBraOpen)
					{
						return false;
					}
				}
			}
			if (list == null || index < 0 || index >= list.Count || list[index] == null)
			{
				return true;
			}
			CoverPoseBlendShapeSet coverPoseBlendShapeSet = list[index];
			if (currentRank != null)
			{
				if (coverPoseBlendShapeSet.RankRequirement == CoverPoseRankRequirement.BraRemoval && !currentRank.allowBraRemoval && !currentRank.allowFullUndress)
				{
					return false;
				}
				if (coverPoseBlendShapeSet.RankRequirement == CoverPoseRankRequirement.FullUndress && !currentRank.allowFullUndress)
				{
					return false;
				}
			}
			if (coverPoseBlendShapeSet.RequiresTopRemoved && (this.ClothingController == null || this.ClothingController.HasVisibleUpperGarment))
			{
				return false;
			}
			if (this.ClothingController == null)
			{
				return true;
			}
			bool flag;
			bool flag2;
			this.ClothingController.GetCurrentPhysicalCoverage(out flag, out flag2);
			switch (list[index].Coverage)
			{
			case CoverPoseCoverage.CoversTopAndBottom:
				return !flag && !flag2;
			case CoverPoseCoverage.CoversNothing:
				return true;
			}
			return !flag;
		}

		// Token: 0x06009264 RID: 37476 RVA: 0x003C11E0 File Offset: 0x003BF3E0
		private int FirstAvailablePoseIndex(bool exhibit)
		{
			int emotionPoseTypeCount = this.GetEmotionPoseTypeCount(exhibit);
			for (int i = 0; i < emotionPoseTypeCount; i++)
			{
				if (this.IsPoseAvailable(exhibit, i))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06009265 RID: 37477 RVA: 0x003C1210 File Offset: 0x003BF410
		public string GetEmotionPoseLabel(bool exhibit, int index)
		{
			List<CoverPoseBlendShapeSet> list = (exhibit ? this.ConfidentBlendShapesPerPose : this.EmbarrassedBlendShapesPerPose);
			if (list == null || index < 0 || index >= list.Count)
			{
				return index.ToString();
			}
			CoverPoseBlendShapeSet coverPoseBlendShapeSet = list[index];
			if (coverPoseBlendShapeSet == null || string.IsNullOrEmpty(coverPoseBlendShapeSet.Label))
			{
				return index.ToString();
			}
			return coverPoseBlendShapeSet.Label;
		}

		// Token: 0x06009266 RID: 37478 RVA: 0x003C1270 File Offset: 0x003BF470
		public void SetCurrentEmotionPoseTypeIndex(int index)
		{
			bool flag = this.IsExhibitCoverPoseActive();
			if (!this.IsPoseAvailable(flag, index))
			{
				return;
			}
			int num = (flag ? this.ConfidentPoseTypeCount : this.EmbarrassedPoseTypeCount);
			if (num <= 0)
			{
				return;
			}
			int num2 = Mathf.Clamp(index, 0, num - 1);
			if (flag)
			{
				if (this._confidentPoseTypeIndex == num2)
				{
					return;
				}
				this._confidentPoseTypeIndex = num2;
			}
			else
			{
				if (this._embarrassedPoseTypeIndex == num2)
				{
					return;
				}
				this._embarrassedPoseTypeIndex = num2;
			}
			this.ApplyCoverPoseTypeToAnimator();
			this.RefreshActiveBlendShapesForCurrentPose();
			this.ApplyCoverType();
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[CoverPoseController] SetCurrentEmotionPoseTypeIndex -> {0} = {1}/{2}", flag ? "Confident" : "Embarrassed", num2, num - 1));
			}
		}

		// Token: 0x06009267 RID: 37479 RVA: 0x003C1320 File Offset: 0x003BF520
		public void SetEmotionPoseTypeIndex(bool exhibit, int index)
		{
			if (!this.IsPoseAvailable(exhibit, index))
			{
				return;
			}
			int num = (exhibit ? this.ConfidentPoseTypeCount : this.EmbarrassedPoseTypeCount);
			if (num <= 0)
			{
				return;
			}
			int num2 = Mathf.Clamp(index, 0, num - 1);
			if (exhibit)
			{
				this._confidentPoseTypeIndex = num2;
			}
			else
			{
				this._embarrassedPoseTypeIndex = num2;
			}
			if (this.IsExhibitCoverPoseActive() == exhibit)
			{
				this.ApplyCoverPoseTypeToAnimator();
				this.RefreshActiveBlendShapesForCurrentPose();
				this.ApplyCoverType();
			}
		}

		// Token: 0x06009268 RID: 37480 RVA: 0x003C1388 File Offset: 0x003BF588
		public int GetEmotionPoseTypeIndex(bool exhibit)
		{
			if (!exhibit)
			{
				return this._embarrassedPoseTypeIndex;
			}
			return this._confidentPoseTypeIndex;
		}

		// Token: 0x06009269 RID: 37481 RVA: 0x003C139C File Offset: 0x003BF59C
		private void BroadcastEmotionPoseSelectionIfChanged()
		{
			bool flag = this.IsExhibitCoverPoseActive();
			int num = (flag ? this._confidentPoseTypeIndex : this._embarrassedPoseTypeIndex);
			if (this._lastBroadcastPoseIndex == num && this._lastBroadcastExhibit == flag)
			{
				return;
			}
			this._lastBroadcastExhibit = flag;
			this._lastBroadcastPoseIndex = num;
			Action onEmotionPoseSelectionChanged = this.OnEmotionPoseSelectionChanged;
			if (onEmotionPoseSelectionChanged == null)
			{
				return;
			}
			onEmotionPoseSelectionChanged();
		}

		// Token: 0x0600926A RID: 37482 RVA: 0x003C13F4 File Offset: 0x003BF5F4
		public void ForceLayerWeight(float weight)
		{
			if (this.PlayerAnimator == null)
			{
				return;
			}
			if (this._armsLayerIndex >= 0)
			{
				this.PlayerAnimator.SetLayerWeight(this._armsLayerIndex, weight);
			}
			if (this._rightArmLayerIndex >= 0)
			{
				this.PlayerAnimator.SetLayerWeight(this._rightArmLayerIndex, weight);
			}
			if (this._leftArmLayerIndex >= 0)
			{
				this.PlayerAnimator.SetLayerWeight(this._leftArmLayerIndex, weight);
			}
			this._targetArmsWeight = weight;
			this._targetRightArmWeight = weight;
			this._targetLeftArmWeight = weight;
		}

		// Token: 0x17001754 RID: 5972
		// (get) Token: 0x0600926B RID: 37483 RVA: 0x003C1476 File Offset: 0x003BF676
		public CharacterRigController RigController
		{
			get
			{
				return this.rigController;
			}
		}

		// Token: 0x0600926C RID: 37484 RVA: 0x003C1480 File Offset: 0x003BF680
		public void OnVehicleMounted(bool allowCover)
		{
			this.OnVehicleMounted(new VehicleCoverPoseConfig
			{
				allowCover = allowCover,
				armMode = VehicleCoverArmMode.BothArms
			});
		}

		// Token: 0x0600926D RID: 37485 RVA: 0x003C14AC File Offset: 0x003BF6AC
		public void OnVehicleMounted(VehicleCoverPoseConfig config)
		{
			this.ClearEndedShapeOverrides();
			this.CacheVehicleBreastCompression();
			this._isRidingVehicle = true;
			this._vehicleAllowsCover = config.allowCover;
			this._vehicleCoverArmMode = config.armMode;
			this._vehicleAnchorProvider = config.anchorProvider;
			this._vehiclePoseDriver = config.poseDriver;
			this._vehiclePosePreset = ((config.posePreset != null) ? config.posePreset : ((this._vehiclePoseDriver != null) ? this._vehiclePoseDriver.PosePreset : null));
			this.ResetVehicleMotionSmoothing(this.GetVehicleMotionState());
			this.RecalculateArmLayerTargets();
			this.UpdateVehicleLimbConstraintSuppression();
			if (this._isCovering && !config.allowCover)
			{
				this.DeactivateCoverPose();
				if (this.DebugMode)
				{
					Debug.Log("[CoverPoseController] Cover pose deactivated - vehicle does not allow cover");
				}
			}
			if (this.DebugMode)
			{
				Debug.Log(string.Format("[CoverPoseController] Vehicle mounted - allowCover={0}, armMode={1}", config.allowCover, config.armMode));
			}
		}

		// Token: 0x0600926E RID: 37486 RVA: 0x003C15A4 File Offset: 0x003BF7A4
		public void OnVehicleDismounted()
		{
			this.ClearEndedShapeOverrides();
			this._isRidingVehicle = false;
			this._vehicleAllowsCover = false;
			this._vehicleCoverArmMode = VehicleCoverArmMode.BothArms;
			this._vehicleAnchorProvider = null;
			this._vehiclePoseDriver = null;
			this._vehiclePosePreset = null;
			this._vehicleMotionStateOverrideActive = false;
			this._vehicleMotionStateInitialized = false;
			this._vehicleMotionStateCurrent = VehicleCoverMotionState.Idle;
			this._vehicleMotionStatePrevious = VehicleCoverMotionState.Idle;
			this._vehicleMotionStateHysteresis = VehicleCoverMotionState.Idle;
			this._vehicleMotionStateBlend = 1f;
			this.RecalculateArmLayerTargets();
			this.UpdateVehicleLimbConstraintSuppression();
			if (this.DebugMode)
			{
				Debug.Log("[CoverPoseController] Vehicle dismounted - cover restrictions cleared");
			}
		}

		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x0600926F RID: 37487 RVA: 0x003C162D File Offset: 0x003BF82D
		public bool IsRidingVehicle
		{
			get
			{
				return this._isRidingVehicle;
			}
		}

		// Token: 0x06009270 RID: 37488 RVA: 0x003C1635 File Offset: 0x003BF835
		private void ClearEndedShapeOverrides()
		{
			if (this._isCovering)
			{
				return;
			}
			CoverPoseController.ClearBlendShapeOverrides(this.EmbarrassedBlendShapesPerPose);
			CoverPoseController.ClearBlendShapeOverrides(this.ConfidentBlendShapesPerPose);
		}

		// Token: 0x06009271 RID: 37489 RVA: 0x003C1656 File Offset: 0x003BF856
		public void SetManualCoverIKTargetEditing(bool active)
		{
			this._manualCoverIKTargetEditing = active;
			if (this.DebugMode)
			{
				Debug.Log("[CoverPoseController] Manual Cover IK target editing " + (active ? "ENABLED" : "DISABLED"));
			}
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x06009272 RID: 37490 RVA: 0x003C1685 File Offset: 0x003BF885
		public bool IsManualCoverIKTargetEditing
		{
			get
			{
				return this._manualCoverIKTargetEditing;
			}
		}

		// Token: 0x06009273 RID: 37491 RVA: 0x003C168D File Offset: 0x003BF88D
		public void SetManualCoverIKAuthoringOverride(bool active)
		{
			this._manualCoverIKAuthoringOverride = active;
			if (this.DebugMode)
			{
				Debug.Log("[CoverPoseController] Manual Cover IK authoring override " + (active ? "ENABLED" : "DISABLED"));
			}
		}

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x06009274 RID: 37492 RVA: 0x003C16BC File Offset: 0x003BF8BC
		public bool IsManualCoverIKAuthoringOverride
		{
			get
			{
				return this._manualCoverIKAuthoringOverride;
			}
		}

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x06009275 RID: 37493 RVA: 0x003C16C4 File Offset: 0x003BF8C4
		public bool VehicleAllowsCover
		{
			get
			{
				return this._vehicleAllowsCover;
			}
		}

		// Token: 0x06009276 RID: 37494 RVA: 0x003C16CC File Offset: 0x003BF8CC
		private void OnDestroy()
		{
			this.ReleaseAnimatorUpdateMode();
			this.DisposeCoverStreamRelease();
			PlayerActivityGate.Changed -= this.HandleActivityGateChanged;
			if (this.ClothingController != null)
			{
				this.ClothingController.OnClothingStateChanged.RemoveListener(new UnityAction<bool>(this.OnClothingStateChanged));
				this.ClothingController.OnGranularCoverageChanged -= this.OnGranularCoverageChanged;
			}
			if (this.futanariMode != null)
			{
				this.futanariMode.OnFutanariModeChanged.RemoveListener(new UnityAction<bool>(this.OnFutanariModeChangedHandler));
			}
			if (this.rigController != null)
			{
				this.rigController.ClearVehicleLimbSuppressions();
			}
			if (CoverPoseController.Instance == this)
			{
				CoverPoseController.Instance = null;
			}
		}

		// Token: 0x0400967D RID: 38525
		private const PlayerActivity ArmsPoseBlockMask = PlayerActivity.Cover | PlayerActivity.ArmsPose;

		// Token: 0x04009681 RID: 38529
		[Header("References")]
		[Tooltip("Reference to the player's Animator")]
		public Animator PlayerAnimator;

		// Token: 0x04009682 RID: 38530
		[Tooltip("Reference to clothing controller - cover pose is disabled when clothed")]
		public ClothingToggleController ClothingController;

		// Token: 0x04009683 RID: 38531
		[Header("Voice")]
		[Tooltip("Battuta vocale di Ciel suonata quando si attiva la cover pose la cui LABEL corrisponde a 'stimulationPoseLabel' (es. 'Stimulation'). Passa per CielVoiceController (audio + sottotitolo). Lascia null per nessuna voce.")]
		[SerializeField]
		private CielVoiceSO stimulationVoice;

		// Token: 0x04009684 RID: 38532
		[Tooltip("Label ESATTA della pose-variant che fa scattare stimulationVoice (case-insensitive). E' la Label del BlendShapeSet della pose, la stessa che appare nello slot Arms della wheel.")]
		[SerializeField]
		private string stimulationPoseLabel = "Stimulation";

		// Token: 0x04009685 RID: 38533
		private bool _stimulationVoicePlaying;

		// Token: 0x04009686 RID: 38534
		[Tooltip("CharacterRigController reference (auto-found if null)")]
		[SerializeField]
		private CharacterRigController rigController;

		// Token: 0x04009687 RID: 38535
		[Header("Animator Parameters")]
		[Tooltip("Name of the Arms layer for cover poses")]
		public string ArmsLayerName = "Arms";

		// Token: 0x04009688 RID: 38536
		[Tooltip("Name of the synced layer used to control only the right arm")]
		public string RightArmLayerName = "RightArm";

		// Token: 0x04009689 RID: 38537
		[Tooltip("Name of the synced layer used to control only the left arm")]
		public string LeftArmLayerName = "LeftArm";

		// Token: 0x0400968A RID: 38538
		[Tooltip("Animator parameter for cover pose index (Int)")]
		public string CoverPoseParamName = "CoverPoseIndex";

		// Token: 0x0400968B RID: 38539
		[Tooltip("Animator trigger to activate cover pose")]
		public string CoverTriggerName = "CoverTrigger";

		// Token: 0x0400968C RID: 38540
		[Tooltip("Animator bool to indicate covering state")]
		public string IsCoveringParamName = "IsCovering";

		// Token: 0x0400968D RID: 38541
		[Tooltip("Animator parameter for cover type (Float): 0 = Boobs Cover, 1 = Full Cover")]
		public string CoverTypeParamName = "CoverType";

		// Token: 0x0400968E RID: 38542
		[Tooltip("Animator parameter for cover pose variant (Float). Picks between multiple pose variants inside the same emotion blend tree. Used by the legacy 1D blend tree path.")]
		public string CoverPoseTypeParamName = "CoverPoseType";

		// Token: 0x0400968F RID: 38543
		[Tooltip("Prefix for the per-pose weight parameters used by the Direct Blend Tree path. With prefix 'CoverPoseWeight_' and 8 max poses, the animator parameters are 'CoverPoseWeight_0' through 'CoverPoseWeight_7'. Each frame, the controller drives weight=1 for the active pose and weight=0 for the others (with damping), so the blend tree never crosses through unwanted intermediate poses.")]
		public string CoverPoseWeightParamPrefix = "CoverPoseWeight_";

		// Token: 0x04009690 RID: 38544
		[Tooltip("Maximum pose-variant count supported. Must match the count used by the Direct Blend Tree converter — i.e. how many CoverPoseWeight_<i> floats exist on the animator.")]
		[Min(1f)]
		public int CoverPoseWeightParamCount = 8;

		// Token: 0x04009691 RID: 38545
		[Header("Cover Pose Settings")]
		[Tooltip("Cover pose index for Embarrassed state (shy, covering up)")]
		public int EmbarrassedCoverPoseIndex = 1;

		// Token: 0x04009692 RID: 38546
		[Tooltip("Cover pose index for Confident/Exhibit state (casual, relaxed cover)")]
		public int ConfidentCoverPoseIndex = 2;

		// Token: 0x04009693 RID: 38547
		[Tooltip("Locks the cover pose to a fixed emotion, ignoring the live emotion dominance. None = follow the live emotion (default). Embarrassed/Confident = always use that pose, blend shapes and IK. Set to Embarrassed on the Prison scene's player to keep the cover permanently shy.")]
		public CoverEmotionOverride ForceEmotion;

		// Token: 0x04009694 RID: 38548
		[Tooltip("Number of pose variants available for the Embarrassed blend tree (matches CoverPoseType thresholds 0..N-1)")]
		[Min(1f)]
		public int EmbarrassedPoseTypeCount = 3;

		// Token: 0x04009695 RID: 38549
		[Tooltip("Number of pose variants available for the Confident blend tree (matches CoverPoseType thresholds 0..N-1)")]
		[Min(1f)]
		public int ConfidentPoseTypeCount = 2;

		// Token: 0x04009696 RID: 38550
		[Tooltip("Input to toggle between Boobs Cover and Full Cover while covering")]
		public GenericInput coverTypeToggleInput = new GenericInput("Mouse0", "Y", "Y");

		// Token: 0x04009697 RID: 38551
		[Tooltip("Input to cycle the cover pose variant (CoverPoseType) for the current emotion state. No-op if the current state has only one variant.")]
		public GenericInput coverPoseTypeCycleInput = new GenericInput("Q", "Q", "Q");

		// Token: 0x04009698 RID: 38552
		[NonSerialized]
		public bool SuppressBuiltInPoseCycleInput;

		// Token: 0x04009699 RID: 38553
		[Tooltip("Current cover type")]
		[SerializeField]
		private CoverType _currentCoverType;

		// Token: 0x0400969A RID: 38554
		[Header("Futanari Lock (Optional)")]
		[Tooltip("Quando la Futanari Mode e' attiva, il CoverType viene forzato a BoobsCover e il toggle FullCover viene ignorato. Se null, viene cercato in modo automatico nei figli del player.")]
		[SerializeField]
		private FutanariModeController futanariMode;

		// Token: 0x0400969B RID: 38555
		[Header("Full Cover IK — Embarrassed")]
		[Tooltip("IK overrides for FullCover + Embarrassed pose. Controls limb positions when the player is shy/covering up.")]
		[SerializeField]
		private List<CoverIKOverride> fullCoverEmbarrassedIK = new List<CoverIKOverride>
		{
			new CoverIKOverride
			{
				limb = LimbType.LeftHand,
				localPosition = new Vector3(0f, -0.1f, 0.15f),
				rotationOffset = Vector3.zero,
				targetPositionWeight = 1f
			}
		};

		// Token: 0x0400969C RID: 38556
		[Header("Full Cover IK — Exhibit")]
		[Tooltip("IK overrides for FullCover + Confident/Exhibit pose. Controls limb positions when the player is confident.")]
		[SerializeField]
		private List<CoverIKOverride> fullCoverExhibitIK = new List<CoverIKOverride>();

		// Token: 0x0400969D RID: 38557
		[Header("Full Cover IK — Seated")]
		[Tooltip("Right hand target replacing FullCover + Embarrassed while seated on a bench.")]
		[SerializeField]
		private SeatedCoverIKTarget seatedEmbarrassedIK = new SeatedCoverIKTarget
		{
			localPosition = new Vector3(0.03f, 0.14f, 0.07f),
			rotationOffset = new Vector3(302.6f, 0f, 155f),
			hintLocalPosition = new Vector3(2.1f, 0f, -0.6f)
		};

		// Token: 0x0400969E RID: 38558
		[Tooltip("Right hand target replacing FullCover + Confident/Exhibit while seated on a bench.")]
		[SerializeField]
		private SeatedCoverIKTarget seatedExhibitIK = new SeatedCoverIKTarget
		{
			localPosition = new Vector3(0f, 0.14f, 0.09f),
			rotationOffset = new Vector3(315.1f, 343.92f, 195.97f),
			hintLocalPosition = new Vector3(2.88f, 0f, 0f)
		};

		// Token: 0x0400969F RID: 38559
		[Header("IK Settings")]
		[Tooltip("Speed of IK weight blending (0-1 transition)")]
		[Range(1f, 20f)]
		[SerializeField]
		private float ikBlendSpeed = 8f;

		// Token: 0x040096A0 RID: 38560
		[Tooltip("Speed of BoobsCover (0) <-> FullCover (1) interpolation")]
		[Range(1f, 20f)]
		[SerializeField]
		private float coverTypeBlendSpeed = 10f;

		// Token: 0x040096A1 RID: 38561
		[Tooltip("Animator damping time for CoverType parameter")]
		[Range(0f, 0.5f)]
		[SerializeField]
		private float coverTypeParamDampTime = 0.12f;

		// Token: 0x040096A2 RID: 38562
		[Tooltip("Animator damping time for CoverPoseType parameter (smooths the cycle between pose variants on Q)")]
		[Range(0f, 0.5f)]
		[SerializeField]
		private float coverPoseTypeParamDampTime = 0.18f;

		// Token: 0x040096A3 RID: 38563
		[Tooltip("Reference bone for IK target positioning (auto-found to Hips if null)")]
		[SerializeField]
		private Transform ikReferenceBone;

		// Token: 0x040096A4 RID: 38564
		[Tooltip("Play Mode IK authoring for body-free setup. When enabled, CoverPose uses current rig target/hint transforms and updates FullCover IK offsets live. Automatically ignored while vehicle cover is active, unless an authoring override is set via SetManualCoverIKAuthoringOverride (used by VehiclePoseSetupWindow).")]
		[SerializeField]
		private bool _manualCoverIKTargetEditing;

		// Token: 0x040096A5 RID: 38565
		[NonSerialized]
		private bool _manualCoverIKAuthoringOverride;

		// Token: 0x040096A6 RID: 38566
		[Header("Blend Shape Overrides — Embarrassed (one entry per CoverPoseType variant)")]
		[Tooltip("Blend shape sets for the Embarrassed state. Index in this list = CoverPoseType variant (0, 1, 2, ...). The set count should match EmbarrassedPoseTypeCount.")]
		public List<CoverPoseBlendShapeSet> EmbarrassedBlendShapesPerPose = new List<CoverPoseBlendShapeSet>();

		// Token: 0x040096A7 RID: 38567
		[Header("Blend Shape Overrides — Confident (one entry per CoverPoseType variant)")]
		[Tooltip("Blend shape sets for the Confident/Exhibit state. Index in this list = CoverPoseType variant (0, 1, 2, ...). The set count should match ConfidentPoseTypeCount.")]
		public List<CoverPoseBlendShapeSet> ConfidentBlendShapesPerPose = new List<CoverPoseBlendShapeSet>();

		// Token: 0x040096A8 RID: 38568
		[Tooltip("Speed of blend shape interpolation")]
		[Range(0f, 20f)]
		public float BlendShapeBlendSpeed = 0.005f;

		// Token: 0x040096A9 RID: 38569
		[Header("Layer & Transitions")]
		[Tooltip("Target weight for the active cover arm layer(s) when covering")]
		[Range(0f, 1f)]
		public float CoverLayerWeight = 1f;

		// Token: 0x040096AA RID: 38570
		[Tooltip("Speed of arms-mode crossfade (1/value = duration in seconds). 4 -> ~0.25s, 6 -> ~0.17s. Lower = smoother, higher = snappier.")]
		[Range(1f, 20f)]
		public float LayerBlendSpeed = 5f;

		// Token: 0x040096AB RID: 38571
		[Header("MagicaCloth")]
		[Tooltip("GameObjects with MagicaCloth to disable while covering. Whether they are actually disabled is decided per-pose by each CoverPoseBlendShapeSet.DisableMagicaCloth flag, NOT by the emotion (Embarrassed/Confident).")]
		[SerializeField]
		private GameObject[] magicaClothObjects;

		// Token: 0x040096AC RID: 38572
		[Header("Vehicle Compatibility")]
		[Tooltip("If true, cover pose input is blocked while riding a vehicle that doesn't allow it")]
		[SerializeField]
		private bool respectVehicleCoverRestrictions = true;

		// Token: 0x040096AD RID: 38573
		[Header("Vehicle FullCover IK")]
		[Tooltip("Animator float parameter used as fallback to infer vehicle motion state when no VehiclePoseDriver is available")]
		[SerializeField]
		private string vehicleSpeedParamName = "VehicleSpeed";

		// Token: 0x040096AE RID: 38574
		[Tooltip("Normalized speed to enter Moving state (hysteresis enter threshold)")]
		[Range(0f, 1f)]
		[SerializeField]
		private float movingEnterSpeedNormalized = 0.12f;

		// Token: 0x040096AF RID: 38575
		[Tooltip("Normalized speed to return from Moving to Idle (hysteresis exit threshold)")]
		[Range(0f, 1f)]
		[SerializeField]
		private float movingExitSpeedNormalized = 0.08f;

		// Token: 0x040096B0 RID: 38576
		[Tooltip("Normalized speed to enter Fast state (hysteresis enter threshold)")]
		[Range(0f, 1f)]
		[SerializeField]
		private float fastEnterSpeedNormalized = 0.72f;

		// Token: 0x040096B1 RID: 38577
		[Tooltip("Normalized speed to return from Fast to Moving (hysteresis exit threshold)")]
		[Range(0f, 1f)]
		[SerializeField]
		private float fastExitSpeedNormalized = 0.64f;

		// Token: 0x040096B2 RID: 38578
		[Tooltip("Duration (seconds) to blend cover IK offsets when motion state changes (Idle/Low/Fast)")]
		[Range(0.02f, 2f)]
		[SerializeField]
		private float vehicleMotionStateBlendDuration = 0.18f;

		// Token: 0x040096B3 RID: 38579
		[Tooltip("Speed at which cover IK constraint weight blends in/out when entering/exiting vehicle cover mode")]
		[Range(0.5f, 30f)]
		[SerializeField]
		private float vehicleCoverIKBlendSpeed = 8f;

		// Token: 0x040096B4 RID: 38580
		[Header("Debug")]
		public bool DebugMode;

		// Token: 0x040096B5 RID: 38581
		private int _armsLayerIndex = -1;

		// Token: 0x040096B6 RID: 38582
		private int _rightArmLayerIndex = -1;

		// Token: 0x040096B7 RID: 38583
		private int _leftArmLayerIndex = -1;

		// Token: 0x040096B8 RID: 38584
		private bool _magicaClothDisabled;

		// Token: 0x040096B9 RID: 38585
		private float _targetArmsWeight;

		// Token: 0x040096BA RID: 38586
		private float _targetRightArmWeight;

		// Token: 0x040096BB RID: 38587
		private float _targetLeftArmWeight;

		// Token: 0x040096BC RID: 38588
		private bool _releasingLeftCoverArm;

		// Token: 0x040096BD RID: 38589
		private bool _releasingRightCoverArm;

		// Token: 0x040096BE RID: 38590
		private float _originalArmsWeight = 1f;

		// Token: 0x040096BF RID: 38591
		private float _originalRightArmWeight;

		// Token: 0x040096C0 RID: 38592
		private float _originalLeftArmWeight;

		// Token: 0x040096C1 RID: 38593
		private bool _isCovering;

		// Token: 0x040096C2 RID: 38594
		private CoverArmsMode _currentEffectiveArmsMode;

		// Token: 0x040096C3 RID: 38595
		private CoverArmsMode _previousEffectiveArmsMode;

		// Token: 0x040096C4 RID: 38596
		private bool _wasCoveringLastFrame;

		// Token: 0x040096C5 RID: 38597
		private float _armsModeBlend = 1f;

		// Token: 0x040096C6 RID: 38598
		private List<CoverBlendShapeEntry> _activeBlendShapes;

		// Token: 0x040096C7 RID: 38599
		private bool _blendShapesInitialized;

		// Token: 0x040096C8 RID: 38600
		private bool _heldItemDeformationCleanupPending;

		// Token: 0x040096C9 RID: 38601
		private bool _endedPoseDeformationCleanupPending;

		// Token: 0x040096CA RID: 38602
		private bool _coverAnimationStreamNeedsReset;

		// Token: 0x040096CB RID: 38603
		private CoverPoseAnimationStreamRelease _coverStreamRelease;

		// Token: 0x040096CC RID: 38604
		private static readonly string[] VehicleBreastCompressionShapes = new string[] { "Breast_Squish.L", "Breast_Squish.R", "Squish_Horizontal", "Squish_Horizontal_Small" };

		// Token: 0x040096CD RID: 38605
		private readonly List<CoverBlendShapeEntry> _vehicleBreastCompression = new List<CoverBlendShapeEntry>();

		// Token: 0x040096CE RID: 38606
		private static readonly string[] CoverClipDeformationShapes = new string[]
		{
			"Breast_Squish.L", "Breast_Squish.R", "Squish_Horizontal", "Squish_Horizontal_Small", "Breast Horizontal Position", "Breast Spacing OUT", "Breast Vertical Angle UP", "Breast Vertical Position", "correct_01.L", "correct_01.R",
			"Key 163", "Key 164", "Breast_Size_00_Flat", "Breast_Size_01_Small", "Breast_Size_02_Smaller", "Breast_Size_03_Natural", "Breast_Size_04_Balloon"
		};

		// Token: 0x040096CF RID: 38607
		private readonly List<CoverBlendShapeEntry> _coverClipDeformations = new List<CoverBlendShapeEntry>();

		// Token: 0x040096D0 RID: 38608
		private bool _hasCoverPoseParam;

		// Token: 0x040096D1 RID: 38609
		private bool _hasIsCoveringParam;

		// Token: 0x040096D2 RID: 38610
		private bool _hasCoverTriggerParam;

		// Token: 0x040096D3 RID: 38611
		private bool _hasCoverTypeParam;

		// Token: 0x040096D4 RID: 38612
		private bool _hasBoobsTypeParam;

		// Token: 0x040096D5 RID: 38613
		private bool _hasCoverPoseTypeParam;

		// Token: 0x040096D6 RID: 38614
		private int _embarrassedPoseTypeIndex;

		// Token: 0x040096D7 RID: 38615
		private int _confidentPoseTypeIndex;

		// Token: 0x040096D8 RID: 38616
		private int[] _coverPoseWeightParamHashes;

		// Token: 0x040096D9 RID: 38617
		private bool[] _hasCoverPoseWeightParam;

		// Token: 0x040096DA RID: 38618
		private float[] _coverPoseWeightCurrent;

		// Token: 0x040096DB RID: 38619
		private bool _lastSeenExhibitDominant;

		// Token: 0x040096DC RID: 38620
		private bool _hasTrackedDominantOnce;

		// Token: 0x040096DD RID: 38621
		private float _ikTargetWeight;

		// Token: 0x040096DE RID: 38622
		private float _coverTypeBlend;

		// Token: 0x040096DF RID: 38623
		private float _coverTypeTarget;

		// Token: 0x040096E0 RID: 38624
		private List<CoverIKOverride> _activeIKOverrides;

		// Token: 0x040096E1 RID: 38625
		private bool _seatedHandTargets;

		// Token: 0x040096E2 RID: 38626
		private Transform _hipsBone;

		// Token: 0x040096E3 RID: 38627
		private bool _rigInitialized;

		// Token: 0x040096E4 RID: 38628
		private bool _coverActivatedRig;

		// Token: 0x040096E5 RID: 38629
		private AnimatorNormalUpdateModeLease _animatorUpdateModeLease;

		// Token: 0x040096E6 RID: 38630
		private bool _isRidingVehicle;

		// Token: 0x040096E7 RID: 38631
		private bool _vehicleAllowsCover;

		// Token: 0x040096E8 RID: 38632
		private VehicleCoverArmMode _vehicleCoverArmMode;

		// Token: 0x040096E9 RID: 38633
		private IVehicleAnchorProvider _vehicleAnchorProvider;

		// Token: 0x040096EA RID: 38634
		private VehiclePoseDriver _vehiclePoseDriver;

		// Token: 0x040096EB RID: 38635
		private VehiclePosePreset _vehiclePosePreset;

		// Token: 0x040096EC RID: 38636
		private bool _hasVehicleSpeedParam;

		// Token: 0x040096ED RID: 38637
		private int _vehicleSpeedParamHash;

		// Token: 0x040096EE RID: 38638
		private bool _vehicleMotionStateOverrideActive;

		// Token: 0x040096EF RID: 38639
		private VehicleCoverMotionState _vehicleMotionStateOverride;

		// Token: 0x040096F0 RID: 38640
		private int _lastVehicleIKDebugFrame = -999;

		// Token: 0x040096F1 RID: 38641
		private VehicleCoverMotionState _vehicleMotionStateCurrent;

		// Token: 0x040096F2 RID: 38642
		private VehicleCoverMotionState _vehicleMotionStatePrevious;

		// Token: 0x040096F3 RID: 38643
		private VehicleCoverMotionState _vehicleMotionStateHysteresis;

		// Token: 0x040096F4 RID: 38644
		private float _vehicleMotionStateBlend = 1f;

		// Token: 0x040096F5 RID: 38645
		private bool _vehicleMotionStateInitialized;

		// Token: 0x040096F7 RID: 38647
		private bool _lastBroadcastExhibit;

		// Token: 0x040096F8 RID: 38648
		private int _lastBroadcastPoseIndex = -1;
	}
}
