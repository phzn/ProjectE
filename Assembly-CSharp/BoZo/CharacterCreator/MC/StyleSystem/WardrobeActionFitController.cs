using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Features.Detection;
using Features.PlayerEmotes;
using Invector.vCharacterController.vActions;
using MagicaCloth2;
using UnityEngine;
using UnityEngine.Events;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007FF RID: 2047
	[DisallowMultipleComponent]
	public sealed class WardrobeActionFitController : MonoBehaviour
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x0600365E RID: 13918 RVA: 0x00138E53 File Offset: 0x00137053
		public bool IsActionFitActive
		{
			get
			{
				return this._activeContexts > WardrobeActionFitController.ActionContext.None;
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x0600365F RID: 13919 RVA: 0x00138E5E File Offset: 0x0013705E
		public string ActiveContextSummary
		{
			get
			{
				return this._activeContexts.ToString();
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06003660 RID: 13920 RVA: 0x00138E71 File Offset: 0x00137071
		public int ActiveShrinkShapeCount
		{
			get
			{
				return this._actionShapes.Count;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06003661 RID: 13921 RVA: 0x00138E7E File Offset: 0x0013707E
		public int TrackedOutfitClothCount
		{
			get
			{
				return this._outfitClothBaseWeights.Count;
			}
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x00138E8C File Offset: 0x0013708C
		public string BuildDiagnostics()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("active={0}; contexts={1}; ", this.IsActionFitActive, this.ActiveContextSummary) + string.Format("shapes={0}; outfitCloths={1}; ", this.ActiveShrinkShapeCount, this.TrackedOutfitClothCount) + string.Format("deepPelvisFactor={0:0.000}; ", this.ResolveDeepPelvisFactor()) + string.Format("deepPelvisExtra={0:0.00}", this._wardrobeDeepPelvisExtra));
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"owner=",
				base.name,
				"; styleService=",
				(this.styleService != null) ? this.styleService.name : "<null>",
				"; ",
				string.Format("definitions={0}; ", (this.styleService != null) ? this.styleService.PartDefinitions.Count : 0),
				string.Format("signature={0}", this._wardrobeSignature)
			}));
			if (this.styleService != null)
			{
				foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in this.styleService.PartDefinitions)
				{
					if (!(mcstylePartDefinitionSO == null))
					{
						GameObject spawnedRoot = this.styleService.GetSpawnedRoot(mcstylePartDefinitionSO.partId);
						MCStylePresetSO activePreset = this.styleService.GetActivePreset(mcstylePartDefinitionSO.partId);
						if (!(spawnedRoot == null) || !(activePreset == null))
						{
							StringBuilder stringBuilder2 = stringBuilder;
							string[] array = new string[12];
							array[0] = "slot ";
							array[1] = mcstylePartDefinitionSO.partId;
							array[2] = ": preset=";
							array[3] = ((activePreset != null) ? activePreset.name : "<null>");
							array[4] = "; root=";
							array[5] = ((spawnedRoot != null) ? spawnedRoot.name : "<null>");
							array[6] = "; ";
							array[7] = string.Format("active={0}; visibleRenderer={1}; ", spawnedRoot != null && spawnedRoot.activeInHierarchy, WardrobeActionFitController.HasVisibleRenderer(spawnedRoot));
							int num = 8;
							string text = "static={0}; ";
							int? num2;
							if (activePreset == null)
							{
								num2 = null;
							}
							else
							{
								List<MCStylePresetSO.ClothingShrinkEntry> clothingShrinkBlendShapes = activePreset.clothingShrinkBlendShapes;
								num2 = ((clothingShrinkBlendShapes != null) ? new int?(clothingShrinkBlendShapes.Count) : null);
							}
							int? num3 = num2;
							array[num] = string.Format(text, num3.GetValueOrDefault());
							int num4 = 9;
							string text2 = "pose={0}; ";
							int? num5;
							if (activePreset == null)
							{
								num5 = null;
							}
							else
							{
								List<MCStylePresetSO.ActionShrinkEntry> actionShrinkBlendShapes = activePreset.actionShrinkBlendShapes;
								num5 = ((actionShrinkBlendShapes != null) ? new int?(actionShrinkBlendShapes.Count) : null);
							}
							num3 = num5;
							array[num4] = string.Format(text2, num3.GetValueOrDefault());
							array[10] = string.Format("genitalCover={0}; ", activePreset != null && activePreset.resetCustomGenitalBlendShapesWhenWorn);
							array[11] = string.Format("deepPelvis={0:0.00}", (activePreset != null) ? activePreset.deepPelvisShrinkAdditional : 0f);
							stringBuilder2.AppendLine(string.Concat(array));
						}
					}
				}
			}
			if (this.bodyPhysics != null)
			{
				stringBuilder.AppendLine(string.Format("bodyPhysics breasts={0:0.00}; ", this.bodyPhysics.CurrentBreastWeight) + string.Format("butt={0:0.00}", this.bodyPhysics.CurrentButtWeight));
			}
			if (this.blendShapeManager != null)
			{
				foreach (string text3 in this._actionShapes)
				{
					stringBuilder.AppendLine(string.Format("{0}={1:0.00}", text3, this.blendShapeManager.GetBlendShapeValue(text3)));
				}
			}
			foreach (KeyValuePair<MagicaCloth, float> keyValuePair in this._outfitClothBaseWeights)
			{
				if (keyValuePair.Key != null)
				{
					stringBuilder.AppendLine(string.Format("cloth {0}: current={1:0.00}; ", keyValuePair.Key.name, keyValuePair.Key.SerializeData.blendWeight) + string.Format("base={0:0.00}", keyValuePair.Value));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x00139350 File Offset: 0x00137550
		private void Awake()
		{
			this.ResolveDependencies();
			this.ResolvePoseBones();
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x0013935E File Offset: 0x0013755E
		private void OnEnable()
		{
			this.ResolveDependencies();
			this.Subscribe();
			this.SynchronizeContexts();
			this.RecomputeAndTransition(true);
			base.StartCoroutine(this.CaptureReferencePoseAfterAnimator());
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x00139388 File Offset: 0x00137588
		private void Update()
		{
			this.SynchronizeContexts();
			if (this.IsActionFitActive && Time.unscaledTime >= this._nextWardrobePollTime)
			{
				this._nextWardrobePollTime = Time.unscaledTime + 0.2f;
				if (this.ComputeVisibleWardrobeSignature() != this._wardrobeSignature)
				{
					this.RecomputeAndTransition(false);
				}
			}
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x001393D6 File Offset: 0x001375D6
		private void LateUpdate()
		{
			if (!this._referencePoseReady && !this.IsActionFitActive)
			{
				this.CaptureReferencePoseNow();
			}
			if (this.IsActionFitActive && this._fitTransition == null && this._referencePoseReady)
			{
				this.TrackPoseDrivenShrink(Time.unscaledDeltaTime);
			}
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x00139411 File Offset: 0x00137611
		private void OnDisable()
		{
			this.Unsubscribe();
			this.StopOwnedCoroutines();
			this.RestoreImmediately(true);
			this._activeContexts = WardrobeActionFitController.ActionContext.None;
			this._debugForcedContexts = WardrobeActionFitController.ActionContext.None;
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x00139434 File Offset: 0x00137634
		private void OnDestroy()
		{
			if (this.blendShapeManager != null)
			{
				this.blendShapeManager.RemoveAllOverridesFromSource("WardrobeActionFit");
			}
			if (this.bodyPhysics != null)
			{
				this.bodyPhysics.ClearExternalPhysicsCap("WardrobeActionFit");
			}
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x00139474 File Offset: 0x00137674
		private void ResolveDependencies()
		{
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
			if (this.bodyPhysics == null)
			{
				this.bodyPhysics = base.GetComponent<WardrobeBodyPhysicsController>() ?? base.GetComponentInChildren<WardrobeBodyPhysicsController>(true);
			}
			if (this.blendShapeManager == null)
			{
				this.blendShapeManager = BlendShapeManager.ResolveForRig(this);
			}
			if (this.coverPoseController == null)
			{
				this.coverPoseController = base.GetComponent<CoverPoseController>() ?? base.GetComponentInChildren<CoverPoseController>(true);
			}
			if (this.emoteController == null)
			{
				this.emoteController = base.GetComponent<GameplayEmoteController>() ?? base.GetComponentInChildren<GameplayEmoteController>(true);
			}
			if (this.actionAnimator == null)
			{
				this.actionAnimator = base.GetComponent<PlayerActionAnimator>() ?? base.GetComponentInChildren<PlayerActionAnimator>(true);
			}
			if (this._animator == null)
			{
				this._animator = base.GetComponent<Animator>() ?? base.GetComponentInChildren<Animator>(true);
			}
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x00139570 File Offset: 0x00137770
		private void ResolvePoseBones()
		{
			if (this._animator == null)
			{
				return;
			}
			HumanBodyBones[] array = new HumanBodyBones[]
			{
				HumanBodyBones.Spine,
				HumanBodyBones.Chest,
				HumanBodyBones.UpperChest,
				HumanBodyBones.LeftUpperArm,
				HumanBodyBones.RightUpperArm,
				HumanBodyBones.LeftLowerArm,
				HumanBodyBones.RightLowerArm,
				HumanBodyBones.LeftUpperLeg,
				HumanBodyBones.RightUpperLeg,
				HumanBodyBones.LeftLowerLeg,
				HumanBodyBones.RightLowerLeg,
				HumanBodyBones.LeftFoot,
				HumanBodyBones.RightFoot
			};
			this._poseBones.Clear();
			foreach (HumanBodyBones humanBodyBones in array)
			{
				Transform boneTransform = this._animator.GetBoneTransform(humanBodyBones);
				if (boneTransform != null)
				{
					this._poseBones[humanBodyBones] = boneTransform;
				}
			}
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x001395DF File Offset: 0x001377DF
		private IEnumerator CaptureReferencePoseAfterAnimator()
		{
			yield return null;
			if (this._activeContexts != WardrobeActionFitController.ActionContext.None)
			{
				yield break;
			}
			this._referenceBoneRotations.Clear();
			foreach (KeyValuePair<HumanBodyBones, Transform> keyValuePair in this._poseBones)
			{
				if (keyValuePair.Value != null)
				{
					this._referenceBoneRotations[keyValuePair.Key] = keyValuePair.Value.localRotation;
				}
			}
			this._referencePoseReady = this._referenceBoneRotations.Count > 0;
			yield break;
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x001395F0 File Offset: 0x001377F0
		private void CaptureReferencePoseNow()
		{
			this._referenceBoneRotations.Clear();
			foreach (KeyValuePair<HumanBodyBones, Transform> keyValuePair in this._poseBones)
			{
				if (keyValuePair.Value != null)
				{
					this._referenceBoneRotations[keyValuePair.Key] = keyValuePair.Value.localRotation;
				}
			}
			this._referencePoseReady = this._referenceBoneRotations.Count > 0;
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x00139688 File Offset: 0x00137888
		private void Subscribe()
		{
			if (this.coverPoseController != null)
			{
				this.coverPoseController.OnCoverStarted -= this.HandleCoverStarted;
				this.coverPoseController.OnCoverEnded -= this.HandleCoverEnded;
				this.coverPoseController.OnCoverStarted += this.HandleCoverStarted;
				this.coverPoseController.OnCoverEnded += this.HandleCoverEnded;
			}
			if (this.emoteController != null)
			{
				this.emoteController.OnEmoteStarted -= this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded -= this.HandleEmoteEnded;
				this.emoteController.OnEmoteStarted += this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded += this.HandleEmoteEnded;
			}
			if (this.actionAnimator != null && this.actionAnimator.GenericAction != null)
			{
				this.actionAnimator.GenericAction.OnStartAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandleGenericActionStarted));
				this.actionAnimator.GenericAction.OnEndAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandleGenericActionEnded));
				this.actionAnimator.GenericAction.OnStartAction.AddListener(new UnityAction<vTriggerGenericAction>(this.HandleGenericActionStarted));
				this.actionAnimator.GenericAction.OnEndAction.AddListener(new UnityAction<vTriggerGenericAction>(this.HandleGenericActionEnded));
			}
			if (this.styleService != null)
			{
				this.styleService.OnActiveStateChanged -= this.HandleWardrobeChanged;
				this.styleService.OnSlotVisibilityChanged -= this.HandleWardrobeChanged;
				this.styleService.OnPhysicalSwapCompleted -= this.HandlePhysicalChanged;
				this.styleService.OnPhysicalRemoved -= this.HandlePhysicalChanged;
				this.styleService.OnActiveStateChanged += this.HandleWardrobeChanged;
				this.styleService.OnSlotVisibilityChanged += this.HandleWardrobeChanged;
				this.styleService.OnPhysicalSwapCompleted += this.HandlePhysicalChanged;
				this.styleService.OnPhysicalRemoved += this.HandlePhysicalChanged;
			}
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x001398E0 File Offset: 0x00137AE0
		private void Unsubscribe()
		{
			if (this.coverPoseController != null)
			{
				this.coverPoseController.OnCoverStarted -= this.HandleCoverStarted;
				this.coverPoseController.OnCoverEnded -= this.HandleCoverEnded;
			}
			if (this.emoteController != null)
			{
				this.emoteController.OnEmoteStarted -= this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			if (this.actionAnimator != null && this.actionAnimator.GenericAction != null)
			{
				this.actionAnimator.GenericAction.OnStartAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandleGenericActionStarted));
				this.actionAnimator.GenericAction.OnEndAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandleGenericActionEnded));
			}
			if (this.styleService != null)
			{
				this.styleService.OnActiveStateChanged -= this.HandleWardrobeChanged;
				this.styleService.OnSlotVisibilityChanged -= this.HandleWardrobeChanged;
				this.styleService.OnPhysicalSwapCompleted -= this.HandlePhysicalChanged;
				this.styleService.OnPhysicalRemoved -= this.HandlePhysicalChanged;
			}
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x00139A32 File Offset: 0x00137C32
		private void HandleCoverStarted()
		{
			this.SetContext(WardrobeActionFitController.ActionContext.Cover, true);
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x00139A3C File Offset: 0x00137C3C
		private void HandleCoverEnded()
		{
			this.SetContext(WardrobeActionFitController.ActionContext.Cover, false);
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x00139A46 File Offset: 0x00137C46
		private void HandleEmoteStarted(EmoteDefinitionSO _)
		{
			this.SetContext(WardrobeActionFitController.ActionContext.Emote, true);
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x00139A50 File Offset: 0x00137C50
		private void HandleEmoteEnded(EmoteDefinitionSO _)
		{
			this.SetContext(WardrobeActionFitController.ActionContext.Emote, false);
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x00139A5A File Offset: 0x00137C5A
		private void HandleGenericActionStarted(vTriggerGenericAction _)
		{
			this._genericActionStartedFrame = Time.frameCount;
			this.SetContext(WardrobeActionFitController.ActionContext.GenericAction, true);
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x00139A6F File Offset: 0x00137C6F
		private void HandleGenericActionEnded(vTriggerGenericAction _)
		{
			this.SetContext(WardrobeActionFitController.ActionContext.GenericAction, false);
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x00139A79 File Offset: 0x00137C79
		private void HandleWardrobeChanged()
		{
			if (this.IsActionFitActive)
			{
				this.RecomputeAndTransition(false);
			}
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x00139A79 File Offset: 0x00137C79
		private void HandlePhysicalChanged(MCStylePartDefinitionSO _, GameObject __)
		{
			if (this.IsActionFitActive)
			{
				this.RecomputeAndTransition(false);
			}
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x00139A8C File Offset: 0x00137C8C
		private void SynchronizeContexts()
		{
			if ((this._debugForcedContexts & WardrobeActionFitController.ActionContext.Cover) == WardrobeActionFitController.ActionContext.None)
			{
				this.SetContext(WardrobeActionFitController.ActionContext.Cover, this.coverPoseController != null && this.coverPoseController.IsCovering);
			}
			if ((this._debugForcedContexts & WardrobeActionFitController.ActionContext.Emote) == WardrobeActionFitController.ActionContext.None)
			{
				this.SetContext(WardrobeActionFitController.ActionContext.Emote, this.emoteController != null && this.emoteController.IsPlaying);
			}
			bool flag = this.actionAnimator != null && this.actionAnimator.GenericAction != null && this.actionAnimator.GenericAction.doingAction;
			if ((this._debugForcedContexts & WardrobeActionFitController.ActionContext.GenericAction) == WardrobeActionFitController.ActionContext.None && (flag || Time.frameCount > this._genericActionStartedFrame))
			{
				this.SetContext(WardrobeActionFitController.ActionContext.GenericAction, flag);
			}
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x00139B4C File Offset: 0x00137D4C
		private void SetContext(WardrobeActionFitController.ActionContext context, bool active)
		{
			WardrobeActionFitController.ActionContext activeContexts = this._activeContexts;
			if (active)
			{
				this._activeContexts |= context;
			}
			else
			{
				this._activeContexts &= ~context;
			}
			if (activeContexts == this._activeContexts)
			{
				return;
			}
			if (this._resetRoutine != null)
			{
				base.StopCoroutine(this._resetRoutine);
				this._resetRoutine = null;
			}
			this.RecomputeAndTransition(false);
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x00139BAC File Offset: 0x00137DAC
		private void RecomputeAndTransition(bool immediate)
		{
			this.CollectVisibleWardrobe();
			WardrobeActionFitController.ActionFitProfile actionFitProfile = this.BuildEffectiveProfile();
			bool flag = actionFitProfile != null;
			bool flag2 = this._wardrobeConstrainsBreasts;
			bool flag3 = this._wardrobeConstrainsButt;
			foreach (KeyValuePair<string, float> keyValuePair in this._wardrobeShrink)
			{
				string key = keyValuePair.Key;
				flag2 |= key.IndexOf("Boob", StringComparison.OrdinalIgnoreCase) >= 0 || key.IndexOf("Chest", StringComparison.OrdinalIgnoreCase) >= 0;
				flag3 |= key.IndexOf("Hip", StringComparison.OrdinalIgnoreCase) >= 0 || key.IndexOf("Leg", StringComparison.OrdinalIgnoreCase) >= 0 || key.IndexOf("PANTS", StringComparison.OrdinalIgnoreCase) >= 0;
			}
			if (flag)
			{
				float num = (flag2 ? actionFitProfile.breastPhysicsCap : 1f);
				float num2 = (flag3 ? actionFitProfile.buttPhysicsCap : 1f);
				WardrobeBodyPhysicsController wardrobeBodyPhysicsController = this.bodyPhysics;
				if (wardrobeBodyPhysicsController != null)
				{
					wardrobeBodyPhysicsController.SetExternalPhysicsCap("WardrobeActionFit", num, num2);
				}
			}
			else
			{
				WardrobeBodyPhysicsController wardrobeBodyPhysicsController2 = this.bodyPhysics;
				if (wardrobeBodyPhysicsController2 != null)
				{
					wardrobeBodyPhysicsController2.ClearExternalPhysicsCap("WardrobeActionFit");
				}
			}
			if (this._fitTransition != null)
			{
				base.StopCoroutine(this._fitTransition);
			}
			this._fitTransition = null;
			float num3 = (immediate ? 0f : (flag ? this.enterDuration : this.exitDuration));
			IEnumerator enumerator2 = this.TransitionFit(actionFitProfile, num3);
			if (num3 <= 0f)
			{
				while (enumerator2.MoveNext())
				{
				}
				return;
			}
			if (base.isActiveAndEnabled)
			{
				this._fitTransition = base.StartCoroutine(enumerator2);
			}
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x00139D60 File Offset: 0x00137F60
		private WardrobeActionFitController.ActionFitProfile BuildEffectiveProfile()
		{
			if (this._activeContexts == WardrobeActionFitController.ActionContext.None)
			{
				return null;
			}
			WardrobeActionFitController.ActionFitProfile actionFitProfile = new WardrobeActionFitController.ActionFitProfile
			{
				poseResponseScale = 0f,
				breastPhysicsCap = 1f,
				buttPhysicsCap = 1f,
				outfitClothWeightMultiplier = 1f
			};
			if ((this._activeContexts & WardrobeActionFitController.ActionContext.Cover) != WardrobeActionFitController.ActionContext.None)
			{
				WardrobeActionFitController.Merge(actionFitProfile, this.coverProfile);
			}
			if ((this._activeContexts & WardrobeActionFitController.ActionContext.Emote) != WardrobeActionFitController.ActionContext.None)
			{
				WardrobeActionFitController.Merge(actionFitProfile, this.emoteProfile);
			}
			if ((this._activeContexts & WardrobeActionFitController.ActionContext.GenericAction) != WardrobeActionFitController.ActionContext.None)
			{
				WardrobeActionFitController.Merge(actionFitProfile, this.genericActionProfile);
			}
			return actionFitProfile;
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x00139DEC File Offset: 0x00137FEC
		private static void Merge(WardrobeActionFitController.ActionFitProfile target, WardrobeActionFitController.ActionFitProfile source)
		{
			if (source == null)
			{
				return;
			}
			target.poseResponseScale = Mathf.Max(target.poseResponseScale, source.poseResponseScale);
			target.breastPhysicsCap = Mathf.Min(target.breastPhysicsCap, source.breastPhysicsCap);
			target.buttPhysicsCap = Mathf.Min(target.buttPhysicsCap, source.buttPhysicsCap);
			target.outfitClothWeightMultiplier = Mathf.Min(target.outfitClothWeightMultiplier, source.outfitClothWeightMultiplier);
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x00139E5C File Offset: 0x0013805C
		private void CollectVisibleWardrobe()
		{
			this._wardrobeShrink.Clear();
			this._wardrobeActionExtra.Clear();
			this._wardrobeDeepPelvisExtra = 0f;
			this._wardrobeConstrainsBreasts = false;
			this._wardrobeConstrainsButt = false;
			HashSet<MagicaCloth> hashSet = new HashSet<MagicaCloth>();
			if (this.styleService == null)
			{
				return;
			}
			IReadOnlyList<MCStylePartDefinitionSO> partDefinitions = this.styleService.PartDefinitions;
			for (int i = 0; i < partDefinitions.Count; i++)
			{
				MCStylePartDefinitionSO mcstylePartDefinitionSO = partDefinitions[i];
				if (!(mcstylePartDefinitionSO == null) && !string.IsNullOrWhiteSpace(mcstylePartDefinitionSO.partId))
				{
					GameObject spawnedRoot = this.styleService.GetSpawnedRoot(mcstylePartDefinitionSO.partId);
					if (WardrobeActionFitController.HasVisibleRenderer(spawnedRoot))
					{
						MCStylePresetSO activePreset = this.styleService.GetActivePreset(mcstylePartDefinitionSO.partId);
						if (activePreset != null && activePreset.overrideBodyPhysics)
						{
							this._wardrobeConstrainsBreasts |= activePreset.breastPhysicsWeight < 0.999f;
							this._wardrobeConstrainsButt |= activePreset.buttPhysicsWeight < 0.999f;
						}
						if (activePreset != null && activePreset.clothingShrinkBlendShapes != null)
						{
							for (int j = 0; j < activePreset.clothingShrinkBlendShapes.Count; j++)
							{
								MCStylePresetSO.ClothingShrinkEntry clothingShrinkEntry = activePreset.clothingShrinkBlendShapes[j];
								float num;
								if (clothingShrinkEntry != null && !string.IsNullOrWhiteSpace(clothingShrinkEntry.blendShapeName) && (!this._wardrobeShrink.TryGetValue(clothingShrinkEntry.blendShapeName, out num) || clothingShrinkEntry.value > num))
								{
									this._wardrobeShrink[clothingShrinkEntry.blendShapeName] = clothingShrinkEntry.value;
								}
							}
						}
						if (activePreset != null && activePreset.actionShrinkBlendShapes != null)
						{
							for (int k = 0; k < activePreset.actionShrinkBlendShapes.Count; k++)
							{
								MCStylePresetSO.ActionShrinkEntry actionShrinkEntry = activePreset.actionShrinkBlendShapes[k];
								if (actionShrinkEntry != null && !string.IsNullOrWhiteSpace(actionShrinkEntry.blendShapeName) && actionShrinkEntry.maximumAdditionalValue > 0f)
								{
									float num2;
									if (!this._wardrobeActionExtra.TryGetValue(actionShrinkEntry.blendShapeName, out num2) || actionShrinkEntry.maximumAdditionalValue > num2)
									{
										this._wardrobeActionExtra[actionShrinkEntry.blendShapeName] = actionShrinkEntry.maximumAdditionalValue;
									}
									if (!this._wardrobeShrink.ContainsKey(actionShrinkEntry.blendShapeName))
									{
										this._wardrobeShrink[actionShrinkEntry.blendShapeName] = 0f;
									}
								}
							}
						}
						if (activePreset != null && activePreset.deepPelvisShrinkAdditional > 0f)
						{
							this._wardrobeDeepPelvisExtra = Mathf.Max(this._wardrobeDeepPelvisExtra, activePreset.deepPelvisShrinkAdditional);
							if (!this._wardrobeShrink.ContainsKey("Shrink_Hip"))
							{
								this._wardrobeShrink["Shrink_Hip"] = 0f;
							}
						}
						foreach (MagicaCloth magicaCloth in spawnedRoot.GetComponentsInChildren<MagicaCloth>(true))
						{
							if (!(magicaCloth == null) && !WardrobeActionFitController.IsNonGarmentCloth(magicaCloth))
							{
								hashSet.Add(magicaCloth);
								if (!this._outfitClothBaseWeights.ContainsKey(magicaCloth))
								{
									this._outfitClothBaseWeights.Add(magicaCloth, magicaCloth.SerializeData.blendWeight);
								}
							}
						}
					}
				}
			}
			List<MagicaCloth> list = new List<MagicaCloth>();
			foreach (MagicaCloth magicaCloth2 in this._outfitClothBaseWeights.Keys)
			{
				if (magicaCloth2 == null || !hashSet.Contains(magicaCloth2))
				{
					list.Add(magicaCloth2);
				}
			}
			for (int m = 0; m < list.Count; m++)
			{
				MagicaCloth magicaCloth3 = list[m];
				if (magicaCloth3 != null)
				{
					magicaCloth3.SerializeData.blendWeight = this._outfitClothBaseWeights[magicaCloth3];
					magicaCloth3.SetParameterChange();
				}
				this._outfitClothBaseWeights.Remove(magicaCloth3);
			}
			this._wardrobeSignature = this.ComputeVisibleWardrobeSignature();
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x0013A260 File Offset: 0x00138460
		private int ComputeVisibleWardrobeSignature()
		{
			if (this.styleService == null)
			{
				return 0;
			}
			int num = 17;
			IReadOnlyList<MCStylePartDefinitionSO> partDefinitions = this.styleService.PartDefinitions;
			for (int i = 0; i < partDefinitions.Count; i++)
			{
				MCStylePartDefinitionSO mcstylePartDefinitionSO = partDefinitions[i];
				if (!(mcstylePartDefinitionSO == null) && !string.IsNullOrWhiteSpace(mcstylePartDefinitionSO.partId))
				{
					GameObject spawnedRoot = this.styleService.GetSpawnedRoot(mcstylePartDefinitionSO.partId);
					if (WardrobeActionFitController.HasVisibleRenderer(spawnedRoot))
					{
						MCStylePresetSO activePreset = this.styleService.GetActivePreset(mcstylePartDefinitionSO.partId);
						num = num * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(mcstylePartDefinitionSO.partId);
						num = num * 31 + ((activePreset != null) ? activePreset.GetInstanceID() : 0);
						num = num * 31 + ((spawnedRoot != null) ? spawnedRoot.GetInstanceID() : 0);
					}
				}
			}
			return num;
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x0013A340 File Offset: 0x00138540
		private static bool IsNonGarmentCloth(MagicaCloth cloth)
		{
			string text = ((cloth != null) ? cloth.gameObject.name : string.Empty);
			return WardrobeActionFitController.IsHairCloth(cloth) || text.Equals("BoobL", StringComparison.OrdinalIgnoreCase) || text.Equals("BoobR", StringComparison.OrdinalIgnoreCase) || text.Equals("ButtL", StringComparison.OrdinalIgnoreCase) || text.Equals("ButtR", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x0013A3A9 File Offset: 0x001385A9
		private static bool IsHairCloth(MagicaCloth cloth)
		{
			return cloth != null && cloth.gameObject.name.IndexOf("Hair", StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x0013A3D4 File Offset: 0x001385D4
		private static bool HasVisibleRenderer(GameObject root)
		{
			if (root == null || !root.activeInHierarchy)
			{
				return false;
			}
			foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>(true))
			{
				if (renderer != null && renderer.enabled && renderer.gameObject.activeInHierarchy)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x0013A42F File Offset: 0x0013862F
		private IEnumerator TransitionFit(WardrobeActionFitController.ActionFitProfile profile, float duration)
		{
			bool active = profile != null;
			Dictionary<string, float> shapeStarts = new Dictionary<string, float>(StringComparer.Ordinal);
			Dictionary<string, float> shapeTargets = new Dictionary<string, float>(StringComparer.Ordinal);
			foreach (string text in this._actionShapes)
			{
				shapeStarts[text] = ((this.blendShapeManager != null) ? this.blendShapeManager.GetBlendShapeValue(text) : 0f);
				float num;
				shapeTargets[text] = (this._wardrobeShrink.TryGetValue(text, out num) ? num : 0f);
			}
			if (active)
			{
				foreach (KeyValuePair<string, float> keyValuePair in this._wardrobeShrink)
				{
					shapeStarts[keyValuePair.Key] = ((this.blendShapeManager != null) ? this.blendShapeManager.GetBlendShapeValue(keyValuePair.Key) : keyValuePair.Value);
					shapeTargets[keyValuePair.Key] = this.ComputePoseTarget(keyValuePair.Key, keyValuePair.Value, profile);
					this._actionShapes.Add(keyValuePair.Key);
				}
			}
			Dictionary<MagicaCloth, float> clothStarts = new Dictionary<MagicaCloth, float>();
			foreach (KeyValuePair<MagicaCloth, float> keyValuePair2 in this._outfitClothBaseWeights)
			{
				if (keyValuePair2.Key != null)
				{
					clothStarts[keyValuePair2.Key] = keyValuePair2.Key.SerializeData.blendWeight;
				}
			}
			float elapsed = 0f;
			for (;;)
			{
				elapsed += Time.unscaledDeltaTime;
				float num2 = ((duration <= 0f) ? 1f : Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration)));
				this.ApplyTransitionFrame(shapeStarts, shapeTargets, clothStarts, profile, num2);
				if (num2 >= 1f)
				{
					break;
				}
				yield return null;
			}
			List<string> list = new List<string>();
			foreach (string text2 in this._actionShapes)
			{
				if (!active || !this._wardrobeShrink.ContainsKey(text2))
				{
					if (this.blendShapeManager != null)
					{
						this.blendShapeManager.RemoveOverride(text2, "WardrobeActionFit", false, false, 0f);
					}
					list.Add(text2);
				}
			}
			foreach (string text3 in list)
			{
				this._actionShapes.Remove(text3);
			}
			if (!active)
			{
				if (this.blendShapeManager != null)
				{
					this.blendShapeManager.RemoveAllOverridesFromSource("WardrobeActionFit");
				}
				this._actionShapes.Clear();
				this._resetRoutine = base.StartCoroutine(this.ResetClothAfterExit());
			}
			this._fitTransition = null;
			yield break;
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x0013A44C File Offset: 0x0013864C
		private void TrackPoseDrivenShrink(float deltaTime)
		{
			if (this.blendShapeManager == null)
			{
				return;
			}
			WardrobeActionFitController.ActionFitProfile actionFitProfile = this.BuildEffectiveProfile();
			if (actionFitProfile == null)
			{
				return;
			}
			float num = 1f - Mathf.Exp(-this.poseTrackingSpeed * Mathf.Max(0f, deltaTime));
			foreach (KeyValuePair<string, float> keyValuePair in this._wardrobeShrink)
			{
				float blendShapeValue = this.blendShapeManager.GetBlendShapeValue(keyValuePair.Key);
				float num2 = this.ComputePoseTarget(keyValuePair.Key, keyValuePair.Value, actionFitProfile);
				this.blendShapeManager.SetOverride(keyValuePair.Key, Mathf.Lerp(blendShapeValue, num2, num), "WardrobeActionFit", BlendShapePriority.Pose, false);
				this._actionShapes.Add(keyValuePair.Key);
			}
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x0013A534 File Offset: 0x00138734
		private float ComputePoseTarget(string shapeName, float baseline, WardrobeActionFitController.ActionFitProfile profile)
		{
			if (profile == null)
			{
				return baseline;
			}
			float num = baseline;
			float num2;
			if (this._wardrobeActionExtra.TryGetValue(shapeName, out num2))
			{
				num += num2 * this.ResolvePoseFactor(shapeName) * profile.poseResponseScale;
			}
			if (string.Equals(shapeName, "Shrink_Hip", StringComparison.OrdinalIgnoreCase) && this._wardrobeDeepPelvisExtra > 0f)
			{
				num += this._wardrobeDeepPelvisExtra * this.ResolveDeepPelvisFactor() * profile.poseResponseScale;
			}
			return Mathf.Clamp(num, baseline, 100f);
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x0013A5AC File Offset: 0x001387AC
		private float ResolveDeepPelvisFactor()
		{
			float num = this.BoneFactor(HumanBodyBones.LeftUpperLeg, 32f, 105f);
			float num2 = this.BoneFactor(HumanBodyBones.RightUpperLeg, 32f, 105f);
			return Mathf.Clamp01(Mathf.Min(num, num2) * 0.85f + Mathf.Max(num, num2) * 0.15f);
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x0013A600 File Offset: 0x00138800
		private float ResolvePoseFactor(string shapeName)
		{
			string text = shapeName ?? string.Empty;
			bool flag = text.EndsWith("_L", StringComparison.OrdinalIgnoreCase) || text.IndexOf("Left", StringComparison.OrdinalIgnoreCase) >= 0;
			bool flag2 = text.EndsWith("_R", StringComparison.OrdinalIgnoreCase) || text.IndexOf("Right", StringComparison.OrdinalIgnoreCase) >= 0;
			float num = this.MaxBoneFactor(new HumanBodyBones[]
			{
				HumanBodyBones.Spine,
				HumanBodyBones.Chest,
				HumanBodyBones.UpperChest
			});
			float num2 = this.BoneFactor(HumanBodyBones.LeftUpperArm);
			float num3 = this.BoneFactor(HumanBodyBones.RightUpperArm);
			float num4 = this.BoneFactor(HumanBodyBones.LeftLowerArm);
			float num5 = this.BoneFactor(HumanBodyBones.RightLowerArm);
			float num6 = this.BoneFactor(HumanBodyBones.LeftUpperLeg);
			float num7 = this.BoneFactor(HumanBodyBones.RightUpperLeg);
			float num8 = this.BoneFactor(HumanBodyBones.LeftLowerLeg);
			float num9 = this.BoneFactor(HumanBodyBones.RightLowerLeg);
			float num10 = this.BoneFactor(HumanBodyBones.LeftFoot);
			float num11 = this.BoneFactor(HumanBodyBones.RightFoot);
			if (text.IndexOf("Boob", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Chest", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Spine", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return Mathf.Max(new float[] { num, num2, num3 });
			}
			if (text.IndexOf("UpperArm", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				if (flag)
				{
					return num2;
				}
				if (!flag2)
				{
					return Mathf.Max(num2, num3);
				}
				return num3;
			}
			else if (text.IndexOf("Elbow", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("LowerArm", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				if (flag)
				{
					return num4;
				}
				if (!flag2)
				{
					return Mathf.Max(num4, num5);
				}
				return num5;
			}
			else
			{
				if (text.IndexOf("Hip", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("PANTS", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return Mathf.Max(new float[] { num, num6, num7 });
				}
				if (text.IndexOf("UpperLeg", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					if (flag)
					{
						return num6;
					}
					if (!flag2)
					{
						return Mathf.Max(num6, num7);
					}
					return num7;
				}
				else if (text.IndexOf("Knee", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					if (flag)
					{
						return Mathf.Max(num6 * 0.35f, num8);
					}
					if (!flag2)
					{
						return Mathf.Max(num8, num9);
					}
					return Mathf.Max(num7 * 0.35f, num9);
				}
				else if (text.IndexOf("LowerLeg", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					if (flag)
					{
						return num8;
					}
					if (!flag2)
					{
						return Mathf.Max(num8, num9);
					}
					return num9;
				}
				else
				{
					if (text.IndexOf("Toe", StringComparison.OrdinalIgnoreCase) < 0 && text.IndexOf("Foot", StringComparison.OrdinalIgnoreCase) < 0)
					{
						return Mathf.Max(new float[] { num, num2, num3, num6, num7, num8, num9 });
					}
					if (flag)
					{
						return num10;
					}
					if (!flag2)
					{
						return Mathf.Max(num10, num11);
					}
					return num11;
				}
			}
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x0013A898 File Offset: 0x00138A98
		private float MaxBoneFactor(params HumanBodyBones[] bones)
		{
			float num = 0f;
			for (int i = 0; i < bones.Length; i++)
			{
				num = Mathf.Max(num, this.BoneFactor(bones[i]));
			}
			return num;
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x0013A8CA File Offset: 0x00138ACA
		private float BoneFactor(HumanBodyBones bone)
		{
			return this.BoneFactor(bone, this.responseStartAngle, this.fullResponseAngle);
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x0013A8E0 File Offset: 0x00138AE0
		private float BoneFactor(HumanBodyBones bone, float startAngle, float endAngle)
		{
			Transform transform;
			Quaternion quaternion;
			if (!this._poseBones.TryGetValue(bone, out transform) || transform == null || !this._referenceBoneRotations.TryGetValue(bone, out quaternion))
			{
				return 0f;
			}
			float num = Quaternion.Angle(quaternion, transform.localRotation);
			float num2 = Mathf.Max(1f, endAngle - startAngle);
			return Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((num - startAngle) / num2));
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x0013A950 File Offset: 0x00138B50
		private void ApplyTransitionFrame(Dictionary<string, float> shapeStarts, Dictionary<string, float> shapeTargets, Dictionary<MagicaCloth, float> clothStarts, WardrobeActionFitController.ActionFitProfile profile, float t)
		{
			if (this.blendShapeManager != null)
			{
				foreach (KeyValuePair<string, float> keyValuePair in shapeTargets)
				{
					float num2;
					float num = (shapeStarts.TryGetValue(keyValuePair.Key, out num2) ? num2 : keyValuePair.Value);
					this.blendShapeManager.SetOverride(keyValuePair.Key, Mathf.Lerp(num, keyValuePair.Value, t), "WardrobeActionFit", BlendShapePriority.Pose, false);
				}
			}
			float num3 = ((profile != null) ? profile.outfitClothWeightMultiplier : 1f);
			foreach (KeyValuePair<MagicaCloth, float> keyValuePair2 in this._outfitClothBaseWeights)
			{
				MagicaCloth key = keyValuePair2.Key;
				if (!(key == null))
				{
					float num5;
					float num4 = (clothStarts.TryGetValue(key, out num5) ? num5 : key.SerializeData.blendWeight);
					key.SerializeData.blendWeight = Mathf.Lerp(num4, keyValuePair2.Value * num3, t);
					key.SetParameterChange();
				}
			}
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x0013AA94 File Offset: 0x00138C94
		private IEnumerator ResetClothAfterExit()
		{
			if (this.resetDelayAfterExit > 0f)
			{
				yield return new WaitForSecondsRealtime(this.resetDelayAfterExit);
			}
			this.ResetAllRelevantCloth();
			this._resetRoutine = null;
			yield break;
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x0013AAA4 File Offset: 0x00138CA4
		private void ResetAllRelevantCloth()
		{
			foreach (MagicaCloth magicaCloth in base.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth != null && !WardrobeActionFitController.IsHairCloth(magicaCloth) && magicaCloth.isActiveAndEnabled)
				{
					magicaCloth.ResetCloth(false);
				}
			}
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x0013AAEB File Offset: 0x00138CEB
		private void StopOwnedCoroutines()
		{
			if (this._fitTransition != null)
			{
				base.StopCoroutine(this._fitTransition);
			}
			if (this._resetRoutine != null)
			{
				base.StopCoroutine(this._resetRoutine);
			}
			this._fitTransition = null;
			this._resetRoutine = null;
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x0013AB24 File Offset: 0x00138D24
		private void RestoreImmediately(bool resetCloth)
		{
			if (this.blendShapeManager != null)
			{
				this.blendShapeManager.RemoveAllOverridesFromSource("WardrobeActionFit");
			}
			this._actionShapes.Clear();
			WardrobeBodyPhysicsController wardrobeBodyPhysicsController = this.bodyPhysics;
			if (wardrobeBodyPhysicsController != null)
			{
				wardrobeBodyPhysicsController.ClearExternalPhysicsCap("WardrobeActionFit");
			}
			foreach (KeyValuePair<MagicaCloth, float> keyValuePair in this._outfitClothBaseWeights)
			{
				if (!(keyValuePair.Key == null))
				{
					keyValuePair.Key.SerializeData.blendWeight = keyValuePair.Value;
					keyValuePair.Key.SetParameterChange();
				}
			}
			if (resetCloth)
			{
				this.ResetAllRelevantCloth();
			}
		}

		// Token: 0x040036A4 RID: 13988
		private const string BlendShapeSource = "WardrobeActionFit";

		// Token: 0x040036A5 RID: 13989
		private const string BodyPhysicsSource = "WardrobeActionFit";

		// Token: 0x040036A6 RID: 13990
		[Header("References (auto-resolved when empty)")]
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x040036A7 RID: 13991
		[SerializeField]
		private WardrobeBodyPhysicsController bodyPhysics;

		// Token: 0x040036A8 RID: 13992
		[SerializeField]
		private BlendShapeManager blendShapeManager;

		// Token: 0x040036A9 RID: 13993
		[SerializeField]
		private CoverPoseController coverPoseController;

		// Token: 0x040036AA RID: 13994
		[SerializeField]
		private GameplayEmoteController emoteController;

		// Token: 0x040036AB RID: 13995
		[SerializeField]
		private PlayerActionAnimator actionAnimator;

		// Token: 0x040036AC RID: 13996
		[Header("Action profiles")]
		[SerializeField]
		private WardrobeActionFitController.ActionFitProfile coverProfile = new WardrobeActionFitController.ActionFitProfile
		{
			poseResponseScale = 0.85f,
			breastPhysicsCap = 0.72f,
			buttPhysicsCap = 0.76f,
			outfitClothWeightMultiplier = 0.72f
		};

		// Token: 0x040036AD RID: 13997
		[SerializeField]
		private WardrobeActionFitController.ActionFitProfile emoteProfile = new WardrobeActionFitController.ActionFitProfile
		{
			poseResponseScale = 1f,
			breastPhysicsCap = 0.65f,
			buttPhysicsCap = 0.65f,
			outfitClothWeightMultiplier = 0.58f
		};

		// Token: 0x040036AE RID: 13998
		[SerializeField]
		private WardrobeActionFitController.ActionFitProfile genericActionProfile = new WardrobeActionFitController.ActionFitProfile
		{
			poseResponseScale = 1f,
			breastPhysicsCap = 0.7f,
			buttPhysicsCap = 0.7f,
			outfitClothWeightMultiplier = 0.65f
		};

		// Token: 0x040036AF RID: 13999
		[Header("Transitions")]
		[SerializeField]
		[Min(0f)]
		private float enterDuration = 0.12f;

		// Token: 0x040036B0 RID: 14000
		[SerializeField]
		[Min(0f)]
		private float exitDuration = 0.32f;

		// Token: 0x040036B1 RID: 14001
		[SerializeField]
		[Min(0f)]
		private float resetDelayAfterExit = 0.08f;

		// Token: 0x040036B2 RID: 14002
		[Header("Pose response")]
		[SerializeField]
		[Range(0f, 45f)]
		private float responseStartAngle = 8f;

		// Token: 0x040036B3 RID: 14003
		[SerializeField]
		[Range(30f, 160f)]
		private float fullResponseAngle = 85f;

		// Token: 0x040036B4 RID: 14004
		[SerializeField]
		[Min(0.1f)]
		private float poseTrackingSpeed = 12f;

		// Token: 0x040036B5 RID: 14005
		private readonly Dictionary<string, float> _wardrobeShrink = new Dictionary<string, float>(StringComparer.Ordinal);

		// Token: 0x040036B6 RID: 14006
		private readonly Dictionary<string, float> _wardrobeActionExtra = new Dictionary<string, float>(StringComparer.Ordinal);

		// Token: 0x040036B7 RID: 14007
		private float _wardrobeDeepPelvisExtra;

		// Token: 0x040036B8 RID: 14008
		private readonly HashSet<string> _actionShapes = new HashSet<string>(StringComparer.Ordinal);

		// Token: 0x040036B9 RID: 14009
		private readonly Dictionary<MagicaCloth, float> _outfitClothBaseWeights = new Dictionary<MagicaCloth, float>();

		// Token: 0x040036BA RID: 14010
		private WardrobeActionFitController.ActionContext _activeContexts;

		// Token: 0x040036BB RID: 14011
		private WardrobeActionFitController.ActionContext _debugForcedContexts;

		// Token: 0x040036BC RID: 14012
		private bool _wardrobeConstrainsBreasts;

		// Token: 0x040036BD RID: 14013
		private bool _wardrobeConstrainsButt;

		// Token: 0x040036BE RID: 14014
		private Coroutine _fitTransition;

		// Token: 0x040036BF RID: 14015
		private Coroutine _resetRoutine;

		// Token: 0x040036C0 RID: 14016
		private int _genericActionStartedFrame = -1;

		// Token: 0x040036C1 RID: 14017
		private readonly Dictionary<HumanBodyBones, Transform> _poseBones = new Dictionary<HumanBodyBones, Transform>();

		// Token: 0x040036C2 RID: 14018
		private readonly Dictionary<HumanBodyBones, Quaternion> _referenceBoneRotations = new Dictionary<HumanBodyBones, Quaternion>();

		// Token: 0x040036C3 RID: 14019
		private Animator _animator;

		// Token: 0x040036C4 RID: 14020
		private bool _referencePoseReady;

		// Token: 0x040036C5 RID: 14021
		private int _wardrobeSignature;

		// Token: 0x040036C6 RID: 14022
		private float _nextWardrobePollTime;

		// Token: 0x02000800 RID: 2048
		[Flags]
		private enum ActionContext
		{
			// Token: 0x040036C8 RID: 14024
			None = 0,
			// Token: 0x040036C9 RID: 14025
			Cover = 1,
			// Token: 0x040036CA RID: 14026
			Emote = 2,
			// Token: 0x040036CB RID: 14027
			GenericAction = 4
		}

		// Token: 0x02000801 RID: 2049
		[Serializable]
		private sealed class ActionFitProfile
		{
			// Token: 0x040036CC RID: 14028
			[Min(0f)]
			public float poseResponseScale = 1f;

			// Token: 0x040036CD RID: 14029
			[Range(0f, 1f)]
			public float breastPhysicsCap = 0.72f;

			// Token: 0x040036CE RID: 14030
			[Range(0f, 1f)]
			public float buttPhysicsCap = 0.72f;

			// Token: 0x040036CF RID: 14031
			[Range(0f, 1f)]
			public float outfitClothWeightMultiplier = 0.68f;
		}
	}
}
