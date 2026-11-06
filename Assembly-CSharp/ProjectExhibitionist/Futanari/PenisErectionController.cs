using System;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using MagicaCloth2;
using UnityEngine;

namespace ProjectExhibitionist.Futanari
{
	// Token: 0x020009E2 RID: 2530
	[DisallowMultipleComponent]
	public class PenisErectionController : MonoBehaviour
	{
		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06004792 RID: 18322 RVA: 0x001B2923 File Offset: 0x001B0B23
		public bool IsErectionRequested
		{
			get
			{
				return this._coverPoseOverride != null || this._rapidEmoteErectionRequested || this._clothingFlashErectionRequested || this._npcErectionRequested || this.HasExternalErectionRequest;
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06004793 RID: 18323 RVA: 0x001B2950 File Offset: 0x001B0B50
		public bool HasExternalErectionRequest
		{
			get
			{
				if (this._externalErectionOwners.Count == 0)
				{
					return false;
				}
				this._externalErectionOwners.RemoveWhere((Object owner) => owner == null);
				return this._externalErectionOwners.Count > 0;
			}
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x001B29A5 File Offset: 0x001B0BA5
		public bool IsExternalErectionRequestedBy(Object owner)
		{
			return owner != null && owner != null && this._externalErectionOwners.Contains(owner);
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06004795 RID: 18325 RVA: 0x001B29C1 File Offset: 0x001B0BC1
		public bool IsNpcErectionRequested
		{
			get
			{
				return this._npcErectionRequested;
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06004796 RID: 18326 RVA: 0x001B29C9 File Offset: 0x001B0BC9
		public bool IsClothingFlashErectionRequested
		{
			get
			{
				return this._clothingFlashErectionRequested;
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06004797 RID: 18327 RVA: 0x001B29D1 File Offset: 0x001B0BD1
		public bool IsNpcReleasePending
		{
			get
			{
				return this._npcReleaseDeadline >= 0f;
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06004798 RID: 18328 RVA: 0x001B29E3 File Offset: 0x001B0BE3
		public float ErectionProgress
		{
			get
			{
				return this._erectionProgress;
			}
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x001B29EC File Offset: 0x001B0BEC
		private void Start()
		{
			if (this.coverPose == null)
			{
				this.coverPose = CoverPoseController.Instance;
			}
			if (this.targetCloths == null || this.targetCloths.Count == 0)
			{
				this.AutoFindClothsFromHierarchy();
			}
			this.ApplyFlaccid();
			this.HookCoverPose();
			this.EvaluateCurrentPose();
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x001B2A3F File Offset: 0x001B0C3F
		private void OnEnable()
		{
			if (this.coverPose == null)
			{
				this.coverPose = CoverPoseController.Instance;
			}
			this.HookCoverPose();
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x001B2A60 File Offset: 0x001B0C60
		private void OnDestroy()
		{
			this.UnhookCoverPose();
			this.ForceRevertToFlaccid();
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x001B2A60 File Offset: 0x001B0C60
		private void OnDisable()
		{
			this.UnhookCoverPose();
			this.ForceRevertToFlaccid();
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x001B2A70 File Offset: 0x001B0C70
		private void Update()
		{
			this.EvaluateCurrentPose();
			this.EvaluateNpcAttention();
			if (!this._isErecting && !this._isDetumescing)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			if (this._isErecting && this._activeOverride != null)
			{
				float num = deltaTime / Mathf.Max(0.01f, this._activeOverride.ErectionDuration);
				this._erectionProgress = Mathf.Min(1f, this._erectionProgress + num);
				this.ApplyProgress(this._activeOverride, this._erectionProgress);
				if (this._erectionProgress >= 1f)
				{
					this._isErecting = false;
					return;
				}
			}
			else if (this._isDetumescing && this._detumescingFromOverride != null)
			{
				float num2 = deltaTime / Mathf.Max(0.01f, this._detumescingFromOverride.DetumescenceDuration);
				this._erectionProgress = Mathf.Max(0f, this._erectionProgress - num2);
				this.ApplyProgress(this._detumescingFromOverride, this._erectionProgress);
				if (this._erectionProgress <= 0f)
				{
					this._isDetumescing = false;
					this._detumescingFromOverride = null;
					this.ApplyFlaccid();
				}
			}
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x001B2B7A File Offset: 0x001B0D7A
		private void HookCoverPose()
		{
			if (this.coverPose == null || this._coverPoseHooked)
			{
				return;
			}
			this.coverPose.OnEmotionPoseSelectionChanged += this.HandlePoseChanged;
			this._coverPoseHooked = true;
		}

		// Token: 0x0600479F RID: 18335 RVA: 0x001B2BB1 File Offset: 0x001B0DB1
		private void UnhookCoverPose()
		{
			if (!this._coverPoseHooked)
			{
				return;
			}
			if (this.coverPose != null)
			{
				this.coverPose.OnEmotionPoseSelectionChanged -= this.HandlePoseChanged;
			}
			this._coverPoseHooked = false;
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x001B2BE8 File Offset: 0x001B0DE8
		private void HandlePoseChanged()
		{
			this.EvaluateCurrentPose();
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x001B2BF0 File Offset: 0x001B0DF0
		private void EvaluateCurrentPose()
		{
			bool flag = this.coverPose != null && this.coverPose.IsCovering;
			string text = (flag ? (this.coverPose.GetCurrentEmotionPoseLabel() ?? string.Empty) : string.Empty);
			string text2 = (flag ? text : "<not-covering>");
			if (text2 == this._lastEvaluatedLabel)
			{
				return;
			}
			this._lastEvaluatedLabel = text2;
			this._coverPoseOverride = (flag ? this.FindOverride(text) : null);
			this.RefreshRequestedErectionState();
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x001B2C74 File Offset: 0x001B0E74
		public void SetRapidEmoteErectionRequested(bool requested)
		{
			if (this._rapidEmoteErectionRequested == requested)
			{
				return;
			}
			this._rapidEmoteErectionRequested = requested;
			this.RefreshRequestedErectionState();
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x001B2C8D File Offset: 0x001B0E8D
		public void SetClothingFlashErectionRequested(bool requested)
		{
			if (this._clothingFlashErectionRequested == requested)
			{
				return;
			}
			this._clothingFlashErectionRequested = requested;
			this.RefreshRequestedErectionState();
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x001B2CA6 File Offset: 0x001B0EA6
		public void SetExternalErectionRequested(Object owner, bool requested)
		{
			if (owner == null)
			{
				return;
			}
			if ((requested && owner != null) ? this._externalErectionOwners.Add(owner) : this._externalErectionOwners.Remove(owner))
			{
				this.RefreshRequestedErectionState();
			}
		}

		// Token: 0x060047A5 RID: 18341 RVA: 0x001B2CDA File Offset: 0x001B0EDA
		public static bool IsVisualAttentionAboveThreshold(bool visualActive, float attentionLevel, float threshold)
		{
			return visualActive && attentionLevel > threshold;
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x001B2CE5 File Offset: 0x001B0EE5
		public static bool ShouldHoldNpcErection(bool wasLatched, bool isInCooldown, bool visualActive, float attentionLevel, float threshold)
		{
			return !isInCooldown && visualActive && (wasLatched || attentionLevel > threshold);
		}

		// Token: 0x060047A7 RID: 18343 RVA: 0x001B2CFC File Offset: 0x001B0EFC
		private void RefreshRequestedErectionState()
		{
			PenisErectionController.PoseOverride poseOverride = this._coverPoseOverride;
			if (poseOverride == null && (this._rapidEmoteErectionRequested || this._clothingFlashErectionRequested || this._npcErectionRequested || this.HasExternalErectionRequest))
			{
				poseOverride = this.FindAutomaticOverride();
			}
			if (poseOverride == null)
			{
				this.StartDetumescence();
				return;
			}
			if (this._activeOverride == poseOverride && !this._isDetumescing)
			{
				if (this._erectionProgress < 1f)
				{
					this._isErecting = true;
				}
				return;
			}
			this.StartErection(poseOverride);
		}

		// Token: 0x060047A8 RID: 18344 RVA: 0x001B2D74 File Offset: 0x001B0F74
		private PenisErectionController.PoseOverride FindAutomaticOverride()
		{
			PenisErectionController.PoseOverride poseOverride = this.FindOverride(this.automaticErectionPoseLabel);
			if (poseOverride != null)
			{
				return poseOverride;
			}
			if (this.overrides == null)
			{
				return null;
			}
			for (int i = 0; i < this.overrides.Count; i++)
			{
				if (this.overrides[i] != null)
				{
					return this.overrides[i];
				}
			}
			return null;
		}

		// Token: 0x060047A9 RID: 18345 RVA: 0x001B2DD0 File Offset: 0x001B0FD0
		private PenisErectionController.PoseOverride FindOverride(string label)
		{
			if (string.IsNullOrEmpty(label) || this.overrides == null)
			{
				return null;
			}
			for (int i = 0; i < this.overrides.Count; i++)
			{
				PenisErectionController.PoseOverride poseOverride = this.overrides[i];
				if (poseOverride != null && string.Equals(poseOverride.PoseLabel, label, StringComparison.Ordinal))
				{
					return poseOverride;
				}
			}
			return null;
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x001B2E28 File Offset: 0x001B1028
		private void EvaluateNpcAttention()
		{
			SensoryManager instance = SensoryManager.Instance;
			if ((instance != null && instance.IsPaused) || NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				if (this._npcReleaseDeadline >= 0f)
				{
					this._npcReleaseDeadline += Time.deltaTime;
				}
				return;
			}
			if (instance != null && !instance.IsRunModeDetectionEnabled)
			{
				this.UpdateNpcErectionRequest(false);
				return;
			}
			this._registeredNpcScratch.Clear();
			bool flag = false;
			IReadOnlyList<NPCSenses> readOnlyList = ((instance != null) ? instance.GetRegisteredNPCs() : null);
			if (readOnlyList != null)
			{
				for (int i = 0; i < readOnlyList.Count; i++)
				{
					NPCSenses npcsenses = readOnlyList[i];
					if (!(npcsenses == null) && npcsenses.isActiveAndEnabled)
					{
						this._registeredNpcScratch.Add(npcsenses);
						bool flag2 = this._qualifyingNpcObservers.Contains(npcsenses);
						bool flag3;
						float num;
						this.GetNpcVisualAttentionSample(npcsenses, out flag3, out num);
						if (PenisErectionController.ShouldHoldNpcErection(flag2, npcsenses.IsInCooldown, flag3, num, this.npcAttentionThreshold))
						{
							this._qualifyingNpcObservers.Add(npcsenses);
							flag = true;
						}
					}
				}
			}
			this.RemoveUnregisteredNpcState();
			this.UpdateNpcErectionRequest(flag);
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x001B2F44 File Offset: 0x001B1144
		private void GetNpcVisualAttentionSample(NPCSenses npc, out bool visualActive, out float attentionLevel)
		{
			AttentionUIController componentInChildren;
			if (!this._attentionUiByNpc.TryGetValue(npc, out componentInChildren) || componentInChildren == null)
			{
				componentInChildren = npc.GetComponentInChildren<AttentionUIController>(true);
				if (componentInChildren != null)
				{
					this._attentionUiByNpc[npc] = componentInChildren;
				}
			}
			if (componentInChildren != null && componentInChildren.UISource != null)
			{
				IDetectionUISource uisource = componentInChildren.UISource;
				Object @object = uisource as Object;
				if (!(uisource is Object) || @object != null)
				{
					visualActive = uisource.VisualActive;
					attentionLevel = uisource.DetectionLevel;
					return;
				}
			}
			visualActive = npc.HasLineOfSight;
			attentionLevel = npc.CurrentAttention;
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x001B2FDC File Offset: 0x001B11DC
		private void RemoveUnregisteredNpcState()
		{
			this._npcRemovalScratch.Clear();
			foreach (NPCSenses npcsenses in this._qualifyingNpcObservers)
			{
				if (npcsenses == null || !this._registeredNpcScratch.Contains(npcsenses))
				{
					this._npcRemovalScratch.Add(npcsenses);
				}
			}
			for (int i = 0; i < this._npcRemovalScratch.Count; i++)
			{
				this._qualifyingNpcObservers.Remove(this._npcRemovalScratch[i]);
			}
			this._npcRemovalScratch.Clear();
			foreach (KeyValuePair<NPCSenses, AttentionUIController> keyValuePair in this._attentionUiByNpc)
			{
				if (keyValuePair.Key == null || !this._registeredNpcScratch.Contains(keyValuePair.Key))
				{
					this._npcRemovalScratch.Add(keyValuePair.Key);
				}
			}
			for (int j = 0; j < this._npcRemovalScratch.Count; j++)
			{
				NPCSenses npcsenses2 = this._npcRemovalScratch[j];
				if (npcsenses2 != null)
				{
					this._attentionUiByNpc.Remove(npcsenses2);
				}
			}
			this._npcRemovalScratch.Clear();
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x001B3148 File Offset: 0x001B1348
		private void UpdateNpcErectionRequest(bool hasQualifyingObserver)
		{
			if (hasQualifyingObserver)
			{
				this._npcReleaseDeadline = -1f;
				if (!this._npcErectionRequested)
				{
					this._npcErectionRequested = true;
					this.RefreshRequestedErectionState();
					if (this.debugLog)
					{
						Debug.Log("[PenisErection] NPC attention > threshold: erection requested.");
					}
				}
				return;
			}
			if (!this._npcErectionRequested)
			{
				this._npcReleaseDeadline = -1f;
				this._qualifyingNpcObservers.Clear();
				return;
			}
			if (this._npcReleaseDeadline < 0f)
			{
				float num = Mathf.Max(0f, Mathf.Min(this.npcReleaseDelayRange.x, this.npcReleaseDelayRange.y));
				float num2 = Mathf.Max(num, Mathf.Max(this.npcReleaseDelayRange.x, this.npcReleaseDelayRange.y));
				float num3 = (Mathf.Approximately(num, num2) ? num : Random.Range(num, num2));
				this._npcReleaseDeadline = Time.time + num3;
				if (this.debugLog)
				{
					Debug.Log(string.Format("[PenisErection] Last NPC lost sight; release in {0:F1}s.", num3));
				}
				return;
			}
			if (Time.time < this._npcReleaseDeadline)
			{
				return;
			}
			this._npcReleaseDeadline = -1f;
			this._npcErectionRequested = false;
			this._qualifyingNpcObservers.Clear();
			this.RefreshRequestedErectionState();
			if (this.debugLog)
			{
				Debug.Log("[PenisErection] NPC hold expired; automatic request released.");
			}
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x001B3284 File Offset: 0x001B1484
		private void StartErection(PenisErectionController.PoseOverride o)
		{
			this._activeOverride = o;
			this._isErecting = true;
			this._isDetumescing = false;
			this._detumescingFromOverride = null;
			if (o.InitialImpulseVelocity > 0f)
			{
				Vector3 vector = ((o.GravityDirection.sqrMagnitude > 1E-06f) ? (-o.GravityDirection.normalized) : Vector3.up);
				for (int i = 0; i < this.targetCloths.Count; i++)
				{
					MagicaCloth magicaCloth = this.targetCloths[i];
					if (magicaCloth != null)
					{
						magicaCloth.AddForce(vector, o.InitialImpulseVelocity, ClothForceMode.VelocityAdd);
					}
				}
			}
			if (this.debugLog)
			{
				Debug.Log(string.Format("[PenisErection] Erection starting on pose '{0}' (duration={1}s).", o.PoseLabel, o.ErectionDuration));
			}
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x001B3348 File Offset: 0x001B1548
		private void StartDetumescence()
		{
			if (this._activeOverride == null && this._erectionProgress <= 0f)
			{
				this._isErecting = false;
				this._isDetumescing = false;
				return;
			}
			this._detumescingFromOverride = this._activeOverride ?? this._detumescingFromOverride;
			this._activeOverride = null;
			this._isErecting = false;
			this._isDetumescing = true;
			if (this.debugLog)
			{
				Debug.Log(string.Format("[PenisErection] Detumescing (from progress={0:F2}).", this._erectionProgress));
			}
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x001B33C8 File Offset: 0x001B15C8
		private void ApplyProgress(PenisErectionController.PoseOverride o, float linearProgress)
		{
			float num = ((o.ErectionCurve != null) ? o.ErectionCurve.Evaluate(linearProgress) : linearProgress);
			Vector3 vector = ((this.flaccid.GravityDirection.sqrMagnitude > 1E-06f) ? this.flaccid.GravityDirection : Vector3.down);
			Vector3 vector2 = ((o.GravityDirection.sqrMagnitude > 1E-06f) ? o.GravityDirection : Vector3.down);
			Vector3 vector3 = Vector3.Slerp(vector, vector2, num);
			float num2 = Mathf.Lerp(this.flaccid.GravityStrength, o.GravityStrength, num);
			float num3 = Mathf.Lerp(this.flaccid.LimitAngle, o.LimitAngle, num);
			float num4 = Mathf.Lerp(this.flaccid.RestorationStiffness, o.RestorationStiffness, num);
			float num5 = Mathf.Lerp(this.flaccid.Damping, o.Damping, num);
			for (int i = 0; i < this.targetCloths.Count; i++)
			{
				MagicaCloth magicaCloth = this.targetCloths[i];
				if (!(magicaCloth == null))
				{
					ClothSerializeData serializeData = magicaCloth.SerializeData;
					serializeData.gravityDirection = vector3;
					serializeData.gravity = num2;
					serializeData.angleLimitConstraint.useAngleLimit = true;
					serializeData.angleLimitConstraint.limitAngle.SetValue(num3, 0f, 1f, true);
					serializeData.angleRestorationConstraint.useAngleRestoration = true;
					if (serializeData.angleRestorationConstraint.stiffness != null)
					{
						serializeData.angleRestorationConstraint.stiffness.value = num4;
					}
					if (serializeData.damping != null)
					{
						serializeData.damping.value = num5;
					}
					magicaCloth.SetParameterChange();
				}
			}
		}

		// Token: 0x060047B1 RID: 18353 RVA: 0x001B3574 File Offset: 0x001B1774
		private void ApplyFlaccid()
		{
			Vector3 vector = ((this.flaccid.GravityDirection.sqrMagnitude > 1E-06f) ? this.flaccid.GravityDirection : Vector3.down);
			for (int i = 0; i < this.targetCloths.Count; i++)
			{
				MagicaCloth magicaCloth = this.targetCloths[i];
				if (!(magicaCloth == null))
				{
					ClothSerializeData serializeData = magicaCloth.SerializeData;
					serializeData.gravityDirection = vector;
					serializeData.gravity = this.flaccid.GravityStrength;
					serializeData.angleLimitConstraint.useAngleLimit = true;
					serializeData.angleLimitConstraint.limitAngle.SetValue(this.flaccid.LimitAngle, 0f, 1f, true);
					serializeData.angleRestorationConstraint.useAngleRestoration = true;
					if (serializeData.angleRestorationConstraint.stiffness != null)
					{
						serializeData.angleRestorationConstraint.stiffness.value = this.flaccid.RestorationStiffness;
					}
					if (serializeData.damping != null)
					{
						serializeData.damping.value = this.flaccid.Damping;
					}
					magicaCloth.SetParameterChange();
				}
			}
			this._flaccidApplied = true;
			if (this.debugLog)
			{
				Debug.Log("[PenisErection] Flaccid state applied.");
			}
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x001B36A8 File Offset: 0x001B18A8
		private void ForceRevertToFlaccid()
		{
			this._isErecting = false;
			this._isDetumescing = false;
			this._activeOverride = null;
			this._detumescingFromOverride = null;
			this._coverPoseOverride = null;
			this._rapidEmoteErectionRequested = false;
			this._clothingFlashErectionRequested = false;
			this._npcErectionRequested = false;
			this._npcReleaseDeadline = -1f;
			this._externalErectionOwners.Clear();
			this._lastEvaluatedLabel = null;
			this._qualifyingNpcObservers.Clear();
			this._registeredNpcScratch.Clear();
			this._npcRemovalScratch.Clear();
			this._attentionUiByNpc.Clear();
			this._erectionProgress = 0f;
			if (this._flaccidApplied)
			{
				this.ApplyFlaccid();
			}
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x001B3750 File Offset: 0x001B1950
		private void AutoFindClothsFromHierarchy()
		{
			MagicaCloth[] componentsInChildren = base.GetComponentsInChildren<MagicaCloth>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && !this.targetCloths.Contains(componentsInChildren[i]))
				{
					this.targetCloths.Add(componentsInChildren[i]);
				}
			}
			if (this.debugLog)
			{
				Debug.Log(string.Format("[PenisErection] Auto-resolved {0} MagicaCloth(s) from hierarchy.", this.targetCloths.Count));
			}
		}

		// Token: 0x04004741 RID: 18241
		[Header("References")]
		[Tooltip("CoverPoseController da osservare. Se null viene cercato in scena allo Start.")]
		[SerializeField]
		private CoverPoseController coverPose;

		// Token: 0x04004742 RID: 18242
		[Tooltip("MagicaCloth da pilotare. Tipicamente Sao_Physics (pisello). Se vuoto, viene popolato automaticamente dai cloths del FutanariModeController sullo stesso GameObject.")]
		[SerializeField]
		private List<MagicaCloth> targetCloths = new List<MagicaCloth>();

		// Token: 0x04004743 RID: 18243
		[Header("Flaccid State (default a inizio game e a fine detumescenza)")]
		[SerializeField]
		private PenisErectionController.FlaccidState flaccid = new PenisErectionController.FlaccidState();

		// Token: 0x04004744 RID: 18244
		[Header("Pose Overrides")]
		[Tooltip("Per ogni pose-label specifica, definisce gli override Magica da applicare. Tipicamente una sola entry: 'Stimulation'.")]
		[SerializeField]
		private List<PenisErectionController.PoseOverride> overrides = new List<PenisErectionController.PoseOverride>
		{
			new PenisErectionController.PoseOverride()
		};

		// Token: 0x04004745 RID: 18245
		[Header("Automatic Erection")]
		[Tooltip("La barra deve superare questa soglia mentre l'NPC vede Ciel per richiedere l'erezione.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float npcAttentionThreshold = 0.5f;

		// Token: 0x04004746 RID: 18246
		[Tooltip("Attesa casuale dopo che l'ultimo NPC qualificato smette di vedere Ciel. Terminata l'attesa parte la normale detumescenza graduale.")]
		[SerializeField]
		private Vector2 npcReleaseDelayRange = new Vector2(15f, 20f);

		// Token: 0x04004747 RID: 18247
		[Tooltip("PoseOverride riutilizzato dalle richieste NPC, dai flash e dagli emote rapidi Q/Z.")]
		[SerializeField]
		private string automaticErectionPoseLabel = "Stimulation";

		// Token: 0x04004748 RID: 18248
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04004749 RID: 18249
		private PenisErectionController.PoseOverride _activeOverride;

		// Token: 0x0400474A RID: 18250
		private PenisErectionController.PoseOverride _detumescingFromOverride;

		// Token: 0x0400474B RID: 18251
		private string _lastEvaluatedLabel = string.Empty;

		// Token: 0x0400474C RID: 18252
		private float _erectionProgress;

		// Token: 0x0400474D RID: 18253
		private bool _isErecting;

		// Token: 0x0400474E RID: 18254
		private bool _isDetumescing;

		// Token: 0x0400474F RID: 18255
		private bool _flaccidApplied;

		// Token: 0x04004750 RID: 18256
		private bool _coverPoseHooked;

		// Token: 0x04004751 RID: 18257
		private PenisErectionController.PoseOverride _coverPoseOverride;

		// Token: 0x04004752 RID: 18258
		private bool _rapidEmoteErectionRequested;

		// Token: 0x04004753 RID: 18259
		private bool _clothingFlashErectionRequested;

		// Token: 0x04004754 RID: 18260
		private bool _npcErectionRequested;

		// Token: 0x04004755 RID: 18261
		private float _npcReleaseDeadline = -1f;

		// Token: 0x04004756 RID: 18262
		private readonly HashSet<Object> _externalErectionOwners = new HashSet<Object>();

		// Token: 0x04004757 RID: 18263
		private readonly HashSet<NPCSenses> _qualifyingNpcObservers = new HashSet<NPCSenses>();

		// Token: 0x04004758 RID: 18264
		private readonly HashSet<NPCSenses> _registeredNpcScratch = new HashSet<NPCSenses>();

		// Token: 0x04004759 RID: 18265
		private readonly List<NPCSenses> _npcRemovalScratch = new List<NPCSenses>();

		// Token: 0x0400475A RID: 18266
		private readonly Dictionary<NPCSenses, AttentionUIController> _attentionUiByNpc = new Dictionary<NPCSenses, AttentionUIController>();

		// Token: 0x020009E3 RID: 2531
		[Serializable]
		public class FlaccidState
		{
			// Token: 0x0400475B RID: 18267
			[Tooltip("Direzione di gravita' applicata a stato moscio. (0,-1,0) = verso il basso. Tipicamente uguale al campo GravityDirection del target erect (cambia solo lo strength).")]
			public Vector3 GravityDirection = new Vector3(0f, -1f, 0f);

			// Token: 0x0400475C RID: 18268
			[Tooltip("Intensita' della gravita' a stato moscio (0..10). Valore alto = pisello che pende deciso.")]
			[Range(0f, 10f)]
			public float GravityStrength = 10f;

			// Token: 0x0400475D RID: 18269
			[Tooltip("Angolo del cono di movimento laterale a stato moscio. Valori medi (30-60°) = ampia oscillazione.")]
			[Range(0f, 180f)]
			public float LimitAngle = 36f;

			// Token: 0x0400475E RID: 18270
			[Tooltip("Stiffness dell'Angle Restoration a stato moscio (0..1). Bassa = il cloth tira debolmente verso la posa di riposo, oscilla libero.")]
			[Range(0f, 1f)]
			public float RestorationStiffness = 0.2f;

			// Token: 0x0400475F RID: 18271
			[Tooltip("Damping (resistenza dell'aria) a stato moscio. Alto = movimento smorzato/pesante.")]
			[Range(0f, 1f)]
			public float Damping = 0.5f;
		}

		// Token: 0x020009E4 RID: 2532
		[Serializable]
		public class PoseOverride
		{
			// Token: 0x04004760 RID: 18272
			[Tooltip("Label esatta (case-sensitive) della cover-pose che attiva l'erezione. Match con CoverPoseController.GetCurrentEmotionPoseLabel(). Esempio: 'Stimulation'.")]
			public string PoseLabel = "Stimulation";

			// Token: 0x04004761 RID: 18273
			[Header("Gravity Target (a fine erezione)")]
			[Tooltip("Direzione di gravita' applicata a erezione=100%. Tipicamente uguale al flaccido ma con strength piu' basso, cosi' il pisello 'galleggia' verso l'alto rispetto alla forza di gravita' percepita.")]
			public Vector3 GravityDirection = new Vector3(0f, -1f, 0f);

			// Token: 0x04004762 RID: 18274
			[Tooltip("Intensita' della gravita' a erezione=100% (0..10). Valore basso = il cloth resiste alla gravita' e il pisello sta su.")]
			[Range(0f, 10f)]
			public float GravityStrength = 2f;

			// Token: 0x04004763 RID: 18275
			[Header("Angle Limit (cono di movimento laterale)")]
			[Tooltip("Angolo del cono di movimento a erezione=100% (eretto). Valori bassi (0-15°) = movimento laterale quasi nullo, pisello rigido lungo asse.")]
			[Range(0f, 180f)]
			public float LimitAngle;

			// Token: 0x04004764 RID: 18276
			[Tooltip("Stiffness dell'Angle Restoration a erezione=100% (0..1). Alta = il cloth tira deciso verso la posa di riposo, pisello rigido lungo asse.")]
			[Range(0f, 1f)]
			public float RestorationStiffness = 1f;

			// Token: 0x04004765 RID: 18277
			[Header("Damping")]
			[Tooltip("Damping (resistenza dell'aria) a erezione=100%. Basso = movimento piu' libero/molle.")]
			[Range(0f, 1f)]
			public float Damping = 0.2f;

			// Token: 0x04004766 RID: 18278
			[Header("Timing")]
			[Tooltip("Durata in secondi della rampa 0→1 (da flaccido a completamente eretto).")]
			[Min(0.01f)]
			public float ErectionDuration = 3f;

			// Token: 0x04004767 RID: 18279
			[Tooltip("Durata in secondi della rampa 1→0 (detumescenza, ritorno al flaccido).")]
			[Min(0.01f)]
			public float DetumescenceDuration = 4f;

			// Token: 0x04004768 RID: 18280
			[Tooltip("Curva di easing applicata al progress della rampa (X=tempo normalizzato, Y=erezione 0..1).")]
			public AnimationCurve ErectionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

			// Token: 0x04004769 RID: 18281
			[Header("Initial Impulse")]
			[Tooltip("Se > 0, applica un impulso istantaneo all'entrata della pose, per far 'scattare' rapidamente la simulazione invece di aspettare l'integrazione graduale.")]
			[Min(0f)]
			public float InitialImpulseVelocity = 2f;
		}
	}
}
