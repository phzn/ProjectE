using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Features.Police;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001240 RID: 4672
	public class DetectionCameraController : MonoBehaviour
	{
		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x0600914E RID: 37198 RVA: 0x003B7942 File Offset: 0x003B5B42
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x0600914F RID: 37199 RVA: 0x003B794A File Offset: 0x003B5B4A
		public int CurrentPresetIndex
		{
			get
			{
				return this._currentPresetIndex;
			}
		}

		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x06009150 RID: 37200 RVA: 0x003B7952 File Offset: 0x003B5B52
		public int ActivePresetCount
		{
			get
			{
				if (!(this._activeCollection != null))
				{
					return 0;
				}
				return this._activeCollection.Count;
			}
		}

		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x06009151 RID: 37201 RVA: 0x003B7970 File Offset: 0x003B5B70
		public float ActivePresetBlendDuration
		{
			get
			{
				if (!this._isActive)
				{
					return 0f;
				}
				DetectionCameraPresetCollectionSO activeCollection = this._activeCollection;
				float? num;
				if (activeCollection == null)
				{
					num = null;
				}
				else
				{
					DetectionCameraPresetEntry preset = activeCollection.GetPreset(this._currentPresetIndex);
					num = ((preset != null) ? new float?(preset.BlendDuration) : null);
				}
				float? num2 = num;
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x06009152 RID: 37202 RVA: 0x003B79CC File Offset: 0x003B5BCC
		public void PrepareActiveCameraForReveal()
		{
			if (!this._isActive || this.ManagedVCam == null)
			{
				return;
			}
			this.ManagedVCam.PreviousStateIsValid = false;
			this.ManagedVCam.InternalUpdateCameraState((this._brain != null) ? this._brain.DefaultWorldUp : Vector3.up, -1f);
		}

		// Token: 0x06009153 RID: 37203 RVA: 0x003B7A2C File Offset: 0x003B5C2C
		public void ApplyPresetByIndex(int index)
		{
			if (!this._isActive || this._activeCollection == null)
			{
				return;
			}
			if (index < 0 || index >= this._activeCollection.Count)
			{
				return;
			}
			this.ApplyPreset(index);
		}

		// Token: 0x06009154 RID: 37204 RVA: 0x003B7A5F File Offset: 0x003B5C5F
		public void ActivateForNPCPhase(DetectionCameraPresetCollectionSO collection, Transform npcTransform, Transform playerTransform = null)
		{
			if (collection == null || collection.Count == 0)
			{
				if (this.DebugMode)
				{
					Debug.LogWarning("[DetCamCtrl] No NPC camera presets provided.");
				}
				return;
			}
			this.Activate(collection, playerTransform, npcTransform, DetectionCameraController.CameraPhase.NPC);
		}

		// Token: 0x06009155 RID: 37205 RVA: 0x003B7A8F File Offset: 0x003B5C8F
		public void ActivateForEmotePhase(DetectionCameraPresetCollectionSO collection, Transform playerTransform, Transform npcTransform = null)
		{
			if (collection == null || collection.Count == 0)
			{
				if (this.DebugMode)
				{
					Debug.LogWarning("[DetCamCtrl] No emote camera presets provided.");
				}
				return;
			}
			this.Activate(collection, playerTransform, npcTransform, DetectionCameraController.CameraPhase.Emote);
		}

		// Token: 0x06009156 RID: 37206 RVA: 0x003B7ABF File Offset: 0x003B5CBF
		public void ActivateForTVPhase(DetectionCameraPresetCollectionSO collection, Transform primaryTransform, Transform secondaryTransform = null)
		{
			if (collection == null || collection.Count == 0)
			{
				if (this.DebugMode)
				{
					Debug.LogWarning("[DetCamCtrl] No TV camera presets provided.");
				}
				return;
			}
			this.Activate(collection, primaryTransform, secondaryTransform, DetectionCameraController.CameraPhase.TV);
		}

		// Token: 0x06009157 RID: 37207 RVA: 0x003B7AF0 File Offset: 0x003B5CF0
		public void Deactivate()
		{
			this._isActive = false;
			this._isMobilePreset = false;
			this._activeCollection = null;
			this._currentPresetIndex = 0;
			if (this._dampingRestoreCoroutine != null)
			{
				base.StopCoroutine(this._dampingRestoreCoroutine);
				this._dampingRestoreCoroutine = null;
			}
			if (this.ManagedVCam != null)
			{
				this.ManagedVCam.Priority = 0;
				this.ManagedVCam.gameObject.SetActive(false);
			}
			this.HideAllHints();
			this._currentPhase = DetectionCameraController.CameraPhase.None;
			if (this.DebugMode)
			{
				Debug.Log("[DetCamCtrl] Deactivated");
			}
		}

		// Token: 0x06009158 RID: 37208 RVA: 0x003B7B7E File Offset: 0x003B5D7E
		public void OnFlyModeEnter()
		{
			this._isFlyModeActive = true;
			this.HideAllHints();
		}

		// Token: 0x06009159 RID: 37209 RVA: 0x003B7B8D File Offset: 0x003B5D8D
		public void OnFlyModeExit()
		{
			this._isFlyModeActive = false;
			if (this._isActive)
			{
				this.ShowPhaseHint();
				this.ApplyPreset(this._currentPresetIndex);
			}
		}

		// Token: 0x0600915A RID: 37210 RVA: 0x003B7BB0 File Offset: 0x003B5DB0
		private void Awake()
		{
			ControllerContextBindings.Set(this.CycleCameraInput, "RightStickClick");
			ControllerKeyHint.BindMatching(this.NpcCameraHintUI, this.CycleCameraInput);
			ControllerKeyHint.BindMatching(this.EmoteCameraHintUI, this.CycleCameraInput);
			ControllerKeyHint.BindMatching(this.TvCameraHintUI, this.CycleCameraInput);
			this.EnsureVCam();
			if (this.ManagedVCam != null)
			{
				this.ManagedVCam.gameObject.SetActive(false);
			}
			this.HideAllHints();
		}

		// Token: 0x0600915B RID: 37211 RVA: 0x003B7C2C File Offset: 0x003B5E2C
		private void Update()
		{
			if (!this._isActive || this._isFlyModeActive)
			{
				return;
			}
			if (PerpWalkController.Instance != null && PerpWalkController.Instance.ActiveCameraController == this)
			{
				return;
			}
			if (this._activeCollection != null && this._activeCollection.Count > 1 && this.CycleCameraInput.GetButtonDown())
			{
				this.CycleToNextPreset();
			}
		}

		// Token: 0x0600915C RID: 37212 RVA: 0x003B7C9C File Offset: 0x003B5E9C
		private void LateUpdate()
		{
			if (!this._isActive || this._isFlyModeActive || !this._isMobilePreset)
			{
				return;
			}
			if (this.ManagedVCam == null)
			{
				return;
			}
			DetectionCameraPresetCollectionSO activeCollection = this._activeCollection;
			DetectionCameraPresetEntry detectionCameraPresetEntry = ((activeCollection != null) ? activeCollection.GetPreset(this._currentPresetIndex) : null);
			if (detectionCameraPresetEntry == null)
			{
				return;
			}
			if (this.ResolveFollowTarget(detectionCameraPresetEntry) == null)
			{
				return;
			}
			this._mobileTimer += Time.deltaTime;
			Vector3 vector;
			switch (detectionCameraPresetEntry.TargetMode)
			{
			case CameraTargetMode.OrbitAround:
				vector = this.ComputeOrbitPosition(detectionCameraPresetEntry);
				break;
			case CameraTargetMode.Dolly:
				vector = this.ComputeDollyPosition(detectionCameraPresetEntry);
				break;
			case CameraTargetMode.PingPong:
				vector = this.ComputePingPongPosition(detectionCameraPresetEntry);
				break;
			case CameraTargetMode.Crane:
				vector = this.ComputeCranePosition(detectionCameraPresetEntry);
				break;
			default:
				return;
			}
			bool flag = this._shotProtectionExtension != null && this._shotProtectionExtension.Owner != null && this._shotProtectionExtension.Owner.IsActive && this._shotProtectionExtension.Owner.TryAlternateAngles;
			Vector3 vector2 = ((detectionCameraPresetEntry.EnableCollisionAvoidance && !flag) ? this.ApplyCollisionAvoidance(vector, detectionCameraPresetEntry) : vector);
			this.ManagedVCam.transform.position = vector2;
			Transform transform = this.ResolveLookAt(detectionCameraPresetEntry);
			if (transform != null)
			{
				this.ManagedVCam.transform.rotation = Quaternion.LookRotation(transform.position - vector2, Vector3.up);
			}
		}

		// Token: 0x0600915D RID: 37213 RVA: 0x003B7E04 File Offset: 0x003B6004
		private void Activate(DetectionCameraPresetCollectionSO collection, Transform playerTransform, Transform npcTransform, DetectionCameraController.CameraPhase phase)
		{
			this.EnsureVCam();
			this.EnsureBrain();
			this._activeCollection = collection;
			this._playerTransform = playerTransform;
			this._npcTransform = npcTransform;
			this._currentPresetIndex = 0;
			this._currentPhase = phase;
			this._isFlyModeActive = false;
			this._isActive = true;
			this.ApplyPreset(0);
			this.ManagedVCam.Priority = ((phase == DetectionCameraController.CameraPhase.TV) ? this.TVPhasePriority : this.BasePriority);
			this.ManagedVCam.gameObject.SetActive(true);
			if (this._brain != null)
			{
				this._brain.enabled = true;
			}
			this.ShowPhaseHint();
			if (this.DebugMode)
			{
				string text = ((playerTransform != null) ? playerTransform.name : "null");
				string text2 = ((npcTransform != null) ? npcTransform.name : "null");
				Debug.Log(string.Concat(new string[]
				{
					string.Format("[DetCamCtrl] Activated for {0} phase with {1} presets ", phase, collection.Count),
					"(player=",
					text,
					", npc=",
					text2,
					")"
				}));
			}
		}

		// Token: 0x0600915E RID: 37214 RVA: 0x003B7F2C File Offset: 0x003B612C
		public void CycleToNextPreset()
		{
			if (this._activeCollection == null || this._activeCollection.Count == 0)
			{
				return;
			}
			int num = (this._currentPresetIndex + 1) % this._activeCollection.Count;
			this.ApplyPreset(num);
			if (this.DebugMode)
			{
				string text = string.Format("[DetCamCtrl] Cycled to preset {0}: ", num);
				string text2 = "'";
				DetectionCameraPresetEntry preset = this._activeCollection.GetPreset(num);
				Debug.Log(text + text2 + ((preset != null) ? preset.Label : null) + "'");
			}
		}

		// Token: 0x0600915F RID: 37215 RVA: 0x003B7FB8 File Offset: 0x003B61B8
		private void ApplyPreset(int index)
		{
			if (this._activeCollection == null)
			{
				return;
			}
			DetectionCameraPresetEntry preset = this._activeCollection.GetPreset(index);
			if (preset == null)
			{
				return;
			}
			this._currentPresetIndex = index;
			this._mobileTimer = 0f;
			this._currentCollisionPullback = 0f;
			this._craneFinished = false;
			this._dollyCurrentWaypoint = 0;
			this._dollyDirection = 1;
			this._dollySegmentTimer = 0f;
			this._dollyHolding = false;
			if (this._brain != null)
			{
				this._brain.m_DefaultBlend.m_Time = preset.BlendDuration;
			}
			this._isMobilePreset = preset.TargetMode == CameraTargetMode.OrbitAround || preset.TargetMode == CameraTargetMode.Dolly || preset.TargetMode == CameraTargetMode.PingPong || preset.TargetMode == CameraTargetMode.Crane;
			if (this._isMobilePreset)
			{
				this.ManagedVCam.Follow = null;
				this.ManagedVCam.LookAt = null;
				this.DestroyBodyComponent();
				this.DestroyAimComponent();
				CinemachineCollider component = this.ManagedVCam.GetComponent<CinemachineCollider>();
				if (component != null)
				{
					component.enabled = false;
				}
				Vector3 vector = this.ComputeStartPosition(preset);
				this.ManagedVCam.transform.position = vector;
				Transform transform = this.ResolveLookAt(preset);
				if (transform != null)
				{
					this.ManagedVCam.transform.rotation = Quaternion.LookRotation(transform.position - vector, Vector3.up);
				}
			}
			else
			{
				switch (preset.TargetMode)
				{
				case CameraTargetMode.Orbital:
					this.ConfigureOrbital(preset);
					break;
				case CameraTargetMode.FollowOnly:
					this.ConfigureFollowOnly(preset);
					break;
				case CameraTargetMode.Static:
					this.ConfigureStatic(preset);
					break;
				}
			}
			if (this._dampingRestoreCoroutine != null)
			{
				base.StopCoroutine(this._dampingRestoreCoroutine);
				this._dampingRestoreCoroutine = null;
			}
			if (!this._isMobilePreset)
			{
				this.ZeroDampingOnVCam();
				this._dampingRestoreCoroutine = base.StartCoroutine(this.RestoreDampingCoroutine(preset, preset.BlendDuration));
			}
			this.ManagedVCam.m_Lens.FieldOfView = preset.FieldOfView;
			if (this._currentPhase != DetectionCameraController.CameraPhase.TV && this.OutputCameraOverride == null)
			{
				ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
				if (instance != null)
				{
					instance.ProtectDetectionCamera(this.ManagedVCam, this.ResolveFollowRoot(preset));
				}
			}
			this._shotProtectionExtension = this.ManagedVCam.GetComponent<DetectionShotCameraExtension>();
			this.PrepareActiveCameraForReveal();
		}

		// Token: 0x06009160 RID: 37216 RVA: 0x003B81F0 File Offset: 0x003B63F0
		private Vector3 ComputeStartPosition(DetectionCameraPresetEntry preset)
		{
			Transform transform = this.ResolveFollowTarget(preset);
			Vector3 vector = ((transform != null) ? transform.position : Vector3.zero);
			switch (preset.TargetMode)
			{
			case CameraTargetMode.OrbitAround:
				return this.ComputeOrbitPositionAtAngle(preset, preset.OrbitStartAngle);
			case CameraTargetMode.Dolly:
				if (preset.DollyWaypoints != null && preset.DollyWaypoints.Count > 0)
				{
					return vector + preset.DollyWaypoints[0].PositionOffset;
				}
				return vector + preset.PositionOffset;
			case CameraTargetMode.PingPong:
				return vector + preset.PositionOffset;
			case CameraTargetMode.Crane:
				return vector + preset.PositionOffset;
			default:
				return vector + preset.PositionOffset;
			}
		}

		// Token: 0x06009161 RID: 37217 RVA: 0x003B82AC File Offset: 0x003B64AC
		private Vector3 ComputeOrbitPosition(DetectionCameraPresetEntry preset)
		{
			float num = (preset.OrbitClockwise ? (-1f) : 1f);
			float num2 = preset.OrbitStartAngle + this._mobileTimer * preset.OrbitSpeed * num;
			return this.ComputeOrbitPositionAtAngle(preset, num2);
		}

		// Token: 0x06009162 RID: 37218 RVA: 0x003B82F0 File Offset: 0x003B64F0
		private Vector3 ComputeOrbitPositionAtAngle(DetectionCameraPresetEntry preset, float angleDeg)
		{
			float num = angleDeg * 0.017453292f;
			Transform transform = this.ResolveFollowTarget(preset);
			Vector3 vector = ((transform != null) ? transform.position : Vector3.zero);
			return new Vector3(vector.x + Mathf.Sin(num) * preset.OrbitRadius, vector.y + preset.OrbitHeight, vector.z + Mathf.Cos(num) * preset.OrbitRadius);
		}

		// Token: 0x06009163 RID: 37219 RVA: 0x003B8360 File Offset: 0x003B6560
		private Vector3 ComputeDollyPosition(DetectionCameraPresetEntry preset)
		{
			Transform transform = this.ResolveFollowTarget(preset);
			Vector3 vector = ((transform != null) ? transform.position : Vector3.zero);
			List<DollyWaypoint> dollyWaypoints = preset.DollyWaypoints;
			if (dollyWaypoints == null || dollyWaypoints.Count == 0)
			{
				return vector + preset.PositionOffset;
			}
			if (dollyWaypoints.Count == 1)
			{
				return vector + dollyWaypoints[0].PositionOffset;
			}
			this._dollySegmentTimer += Time.deltaTime;
			DollyWaypoint dollyWaypoint = dollyWaypoints[this._dollyCurrentWaypoint];
			if (this._dollyHolding)
			{
				if (this._dollySegmentTimer >= dollyWaypoint.HoldDuration)
				{
					this._dollyHolding = false;
					this._dollySegmentTimer = 0f;
					this.AdvanceDollyWaypoint(preset);
				}
				return vector + dollyWaypoint.PositionOffset;
			}
			int nextDollyWaypoint = this.GetNextDollyWaypoint(preset);
			DollyWaypoint dollyWaypoint2 = dollyWaypoints[nextDollyWaypoint];
			float num = Mathf.Clamp01(this._dollySegmentTimer / dollyWaypoint2.TransitDuration);
			num = num * num * (3f - 2f * num);
			Vector3 positionOffset = dollyWaypoint.PositionOffset;
			Vector3 positionOffset2 = dollyWaypoint2.PositionOffset;
			Vector3 vector2 = Vector3.Lerp(positionOffset, positionOffset2, num);
			if (this._dollySegmentTimer >= dollyWaypoint2.TransitDuration)
			{
				this._dollySegmentTimer = 0f;
				this._dollyCurrentWaypoint = nextDollyWaypoint;
				if (dollyWaypoints[this._dollyCurrentWaypoint].HoldDuration > 0f)
				{
					this._dollyHolding = true;
				}
				else
				{
					this.AdvanceDollyWaypoint(preset);
				}
			}
			return vector + vector2;
		}

		// Token: 0x06009164 RID: 37220 RVA: 0x00004F31 File Offset: 0x00003131
		private void AdvanceDollyWaypoint(DetectionCameraPresetEntry preset)
		{
		}

		// Token: 0x06009165 RID: 37221 RVA: 0x003B84CC File Offset: 0x003B66CC
		private int GetNextDollyWaypoint(DetectionCameraPresetEntry preset)
		{
			List<DollyWaypoint> dollyWaypoints = preset.DollyWaypoints;
			if (preset.DollyPingPong)
			{
				int num = this._dollyCurrentWaypoint + this._dollyDirection;
				if (num >= dollyWaypoints.Count || num < 0)
				{
					this._dollyDirection *= -1;
					num = this._dollyCurrentWaypoint + this._dollyDirection;
				}
				return Mathf.Clamp(num, 0, dollyWaypoints.Count - 1);
			}
			return (this._dollyCurrentWaypoint + 1) % dollyWaypoints.Count;
		}

		// Token: 0x06009166 RID: 37222 RVA: 0x003B8540 File Offset: 0x003B6740
		private Vector3 ComputePingPongPosition(DetectionCameraPresetEntry preset)
		{
			Transform transform = this.ResolveFollowTarget(preset);
			Vector3 vector = ((transform != null) ? transform.position : Vector3.zero);
			float num = this._mobileTimer % preset.PingPongCycleDuration / preset.PingPongCycleDuration;
			float num2 = (1f - Mathf.Cos(num * 3.1415927f * 2f)) * 0.5f;
			Vector3 vector2 = Vector3.Lerp(preset.PositionOffset, preset.PingPongEndOffset, num2);
			return vector + vector2;
		}

		// Token: 0x06009167 RID: 37223 RVA: 0x003B85B8 File Offset: 0x003B67B8
		private Vector3 ComputeCranePosition(DetectionCameraPresetEntry preset)
		{
			Transform transform = this.ResolveFollowTarget(preset);
			Vector3 vector = ((transform != null) ? transform.position : Vector3.zero);
			if (this._craneFinished)
			{
				return vector + preset.CraneEndOffset;
			}
			float num = Mathf.Clamp01(this._mobileTimer / preset.CraneDuration);
			float num2 = ((preset.CraneEasing != null) ? preset.CraneEasing.Evaluate(num) : num);
			if (num >= 1f)
			{
				this._craneFinished = true;
			}
			Vector3 vector2 = Vector3.Lerp(preset.PositionOffset, preset.CraneEndOffset, num2);
			return vector + vector2;
		}

		// Token: 0x06009168 RID: 37224 RVA: 0x003B8650 File Offset: 0x003B6850
		private Vector3 ApplyCollisionAvoidance(Vector3 idealPos, DetectionCameraPresetEntry preset)
		{
			Transform transform = this.ResolveFollowTarget(preset);
			if (transform == null)
			{
				return idealPos;
			}
			Transform transform2 = this.ResolveFollowRoot(preset) ?? transform;
			Transform transform3 = this.ResolveLookAt(preset);
			Vector3 vector = ((transform3 != null) ? transform3.position : transform.position);
			Vector3 vector2 = idealPos - vector;
			float magnitude = vector2.magnitude;
			if (magnitude < 0.01f)
			{
				return idealPos;
			}
			Vector3 vector3 = vector2 / magnitude;
			int num = Physics.SphereCastNonAlloc(vector, preset.CollisionRadius, vector3, this._sphereCastHits, magnitude, preset.CollisionLayers, QueryTriggerInteraction.Ignore);
			float num2 = magnitude;
			for (int i = 0; i < num; i++)
			{
				if (!this._sphereCastHits[i].transform.IsChildOf(transform2) && this._sphereCastHits[i].distance < num2)
				{
					num2 = this._sphereCastHits[i].distance;
				}
			}
			float num3 = Mathf.Max(num2 - preset.CollisionRadius, preset.MinDistanceFromTarget);
			if (num3 < magnitude)
			{
				this._currentCollisionPullback = Mathf.Lerp(this._currentCollisionPullback, 1f, Time.deltaTime / Mathf.Max(preset.CollisionDamping * 0.3f, 0.01f));
			}
			else
			{
				this._currentCollisionPullback = Mathf.Lerp(this._currentCollisionPullback, 0f, Time.deltaTime / Mathf.Max(preset.CollisionDamping, 0.01f));
			}
			float num4 = Mathf.Lerp(magnitude, num3, this._currentCollisionPullback);
			return vector + vector3 * num4;
		}

		// Token: 0x06009169 RID: 37225 RVA: 0x003B87E0 File Offset: 0x003B69E0
		private void ConfigureOrbital(DetectionCameraPresetEntry preset)
		{
			Transform transform = this.ResolveFollowTarget(preset);
			Transform transform2 = this.ResolveLookAt(preset);
			this.ManagedVCam.Follow = transform;
			this.ManagedVCam.LookAt = transform2;
			CinemachineTransposer cinemachineTransposer = this.ManagedVCam.GetCinemachineComponent<CinemachineTransposer>();
			if (cinemachineTransposer == null)
			{
				this.DestroyBodyComponent();
				cinemachineTransposer = this.ManagedVCam.AddCinemachineComponent<CinemachineTransposer>();
			}
			cinemachineTransposer.m_BindingMode = (CinemachineTransposer.BindingMode)preset.BodyBindingMode;
			cinemachineTransposer.m_FollowOffset = preset.BodyFollowOffset;
			cinemachineTransposer.m_XDamping = preset.BodyXDamping;
			cinemachineTransposer.m_YDamping = preset.BodyYDamping;
			cinemachineTransposer.m_ZDamping = preset.BodyZDamping;
			cinemachineTransposer.m_YawDamping = preset.BodyYawDamping;
			cinemachineTransposer.m_PitchDamping = preset.BodyPitchDamping;
			cinemachineTransposer.m_RollDamping = preset.BodyRollDamping;
			CinemachineComposer cinemachineComposer = this.ManagedVCam.GetCinemachineComponent<CinemachineComposer>();
			if (cinemachineComposer == null)
			{
				this.DestroyAimComponent();
				cinemachineComposer = this.ManagedVCam.AddCinemachineComponent<CinemachineComposer>();
			}
			cinemachineComposer.m_TrackedObjectOffset = preset.AimTrackedObjectOffset;
			cinemachineComposer.m_LookaheadTime = 0f;
			cinemachineComposer.m_LookaheadSmoothing = 0f;
			cinemachineComposer.m_LookaheadIgnoreY = false;
			cinemachineComposer.m_HorizontalDamping = preset.AimHorizontalDamping;
			cinemachineComposer.m_VerticalDamping = preset.AimVerticalDamping;
			cinemachineComposer.m_ScreenX = preset.AimScreenX;
			cinemachineComposer.m_ScreenY = preset.AimScreenY;
			cinemachineComposer.m_DeadZoneWidth = preset.AimDeadZoneWidth;
			cinemachineComposer.m_DeadZoneHeight = preset.AimDeadZoneHeight;
			cinemachineComposer.m_SoftZoneWidth = preset.AimSoftZoneWidth;
			cinemachineComposer.m_SoftZoneHeight = preset.AimSoftZoneHeight;
			cinemachineComposer.m_BiasX = preset.AimBiasX;
			cinemachineComposer.m_BiasY = preset.AimBiasY;
			this.ConfigureCollisionExtension(preset);
		}

		// Token: 0x0600916A RID: 37226 RVA: 0x003B896C File Offset: 0x003B6B6C
		private void ConfigureFollowOnly(DetectionCameraPresetEntry preset)
		{
			Transform transform = this.ResolveFollowTarget(preset);
			this.ManagedVCam.Follow = transform;
			this.ManagedVCam.LookAt = null;
			CinemachineTransposer cinemachineTransposer = this.ManagedVCam.GetCinemachineComponent<CinemachineTransposer>();
			if (cinemachineTransposer == null)
			{
				this.DestroyBodyComponent();
				cinemachineTransposer = this.ManagedVCam.AddCinemachineComponent<CinemachineTransposer>();
			}
			cinemachineTransposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
			cinemachineTransposer.m_FollowOffset = preset.PositionOffset;
			cinemachineTransposer.m_XDamping = preset.FollowDamping;
			cinemachineTransposer.m_YDamping = preset.FollowDamping;
			cinemachineTransposer.m_ZDamping = preset.FollowDamping;
			this.DestroyAimComponent();
			if (transform != null)
			{
				Quaternion quaternion = transform.rotation * Quaternion.Euler(preset.Rotation);
				this.ManagedVCam.transform.rotation = quaternion;
			}
			this.ConfigureCollisionExtension(preset);
		}

		// Token: 0x0600916B RID: 37227 RVA: 0x003B8A38 File Offset: 0x003B6C38
		private void ConfigureStatic(DetectionCameraPresetEntry preset)
		{
			this.ManagedVCam.Follow = null;
			this.ManagedVCam.LookAt = null;
			this.DestroyBodyComponent();
			this.DestroyAimComponent();
			Transform transform = this.ResolveFollowTarget(preset);
			if (transform != null)
			{
				this.ManagedVCam.transform.position = transform.position + preset.PositionOffset;
				this.ManagedVCam.transform.rotation = Quaternion.Euler(preset.Rotation);
			}
			this.ConfigureCollisionExtension(preset);
		}

		// Token: 0x0600916C RID: 37228 RVA: 0x003B8AC0 File Offset: 0x003B6CC0
		private void ConfigureCollisionExtension(DetectionCameraPresetEntry preset)
		{
			CinemachineCollider cinemachineCollider = this.ManagedVCam.GetComponent<CinemachineCollider>();
			if (preset.EnableCollisionAvoidance)
			{
				if (cinemachineCollider == null)
				{
					cinemachineCollider = this.ManagedVCam.gameObject.AddComponent<CinemachineCollider>();
				}
				cinemachineCollider.m_CollideAgainst = preset.CollisionLayers;
				cinemachineCollider.m_MinimumDistanceFromTarget = preset.MinDistanceFromTarget;
				cinemachineCollider.m_AvoidObstacles = true;
				cinemachineCollider.m_CameraRadius = preset.CollisionRadius;
				cinemachineCollider.m_Damping = preset.CollisionDamping;
				cinemachineCollider.m_DampingWhenOccluded = preset.CollisionDamping * 0.5f;
				cinemachineCollider.m_Strategy = CinemachineCollider.ResolutionStrategy.PullCameraForward;
				cinemachineCollider.enabled = true;
				return;
			}
			if (cinemachineCollider != null)
			{
				cinemachineCollider.enabled = false;
			}
		}

		// Token: 0x0600916D RID: 37229 RVA: 0x003B8B64 File Offset: 0x003B6D64
		private Transform ResolveFollowTarget(DetectionCameraPresetEntry preset)
		{
			Transform transform = this.ResolveFollowRoot(preset);
			if (transform != null && !string.IsNullOrEmpty(preset.FollowBoneName))
			{
				Transform transform2 = DetectionCameraController.FindBone(transform, preset.FollowBoneName);
				if (transform2 != null)
				{
					return transform2;
				}
			}
			return transform;
		}

		// Token: 0x0600916E RID: 37230 RVA: 0x003B8BA8 File Offset: 0x003B6DA8
		private Transform ResolveFollowRoot(DetectionCameraPresetEntry preset)
		{
			PresetReferenceTarget referenceTarget = preset.ReferenceTarget;
			if (referenceTarget == PresetReferenceTarget.Player)
			{
				return this._playerTransform;
			}
			if (referenceTarget == PresetReferenceTarget.NPC)
			{
				return this._npcTransform;
			}
			if (this._currentPhase != DetectionCameraController.CameraPhase.NPC)
			{
				return this._playerTransform;
			}
			return this._npcTransform;
		}

		// Token: 0x0600916F RID: 37231 RVA: 0x003B8BEC File Offset: 0x003B6DEC
		private Transform ResolveLookAt(DetectionCameraPresetEntry preset)
		{
			Transform transform = this.ResolveFollowTarget(preset);
			if (!string.IsNullOrEmpty(preset.LookAtBoneName) && transform != null)
			{
				Transform transform2 = DetectionCameraController.FindBone(transform, preset.LookAtBoneName);
				if (transform2 != null)
				{
					return transform2;
				}
			}
			return transform;
		}

		// Token: 0x06009170 RID: 37232 RVA: 0x003B8C30 File Offset: 0x003B6E30
		private void EnsureVCam()
		{
			if (this.ManagedVCam != null)
			{
				return;
			}
			this.ManagedVCam = base.GetComponentInChildren<CinemachineVirtualCamera>(true);
			if (this.ManagedVCam == null)
			{
				GameObject gameObject = new GameObject("CM_DetectionPreset");
				gameObject.transform.SetParent(base.transform);
				this.ManagedVCam = gameObject.AddComponent<CinemachineVirtualCamera>();
				this.ManagedVCam.Priority = 0;
				this.ManagedVCam.m_Lens.NearClipPlane = 0.01f;
				gameObject.SetActive(false);
				if (this.DebugMode)
				{
					Debug.Log("[DetCamCtrl] Created managed VCam with Near Clip Plane = 0.01");
				}
			}
		}

		// Token: 0x06009171 RID: 37233 RVA: 0x003B8CCC File Offset: 0x003B6ECC
		private void EnsureBrain()
		{
			if (this._brain != null)
			{
				return;
			}
			Camera camera = ((this.OutputCameraOverride != null) ? this.OutputCameraOverride : Camera.main);
			if (camera != null)
			{
				this._brain = camera.GetComponent<CinemachineBrain>();
			}
		}

		// Token: 0x06009172 RID: 37234 RVA: 0x003B8D1C File Offset: 0x003B6F1C
		private void DestroyBodyComponent()
		{
			CinemachineComponentBase cinemachineComponent = this.ManagedVCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
			if (cinemachineComponent != null)
			{
				cinemachineComponent.enabled = false;
				RuntimeUtility.DestroyObject(cinemachineComponent);
				this.ManagedVCam.InvalidateComponentPipeline();
			}
		}

		// Token: 0x06009173 RID: 37235 RVA: 0x003B8D58 File Offset: 0x003B6F58
		private void DestroyAimComponent()
		{
			CinemachineComponentBase cinemachineComponent = this.ManagedVCam.GetCinemachineComponent(CinemachineCore.Stage.Aim);
			if (cinemachineComponent != null)
			{
				cinemachineComponent.enabled = false;
				RuntimeUtility.DestroyObject(cinemachineComponent);
				this.ManagedVCam.InvalidateComponentPipeline();
			}
		}

		// Token: 0x06009174 RID: 37236 RVA: 0x003B8D94 File Offset: 0x003B6F94
		private void ShowPhaseHint()
		{
			this.HideAllHints();
			if (this._activeCollection == null || this._activeCollection.Count <= 1)
			{
				return;
			}
			switch (this._currentPhase)
			{
			case DetectionCameraController.CameraPhase.NPC:
				if (this.NpcCameraHintUI != null)
				{
					this.NpcCameraHintUI.SetActive(true);
					return;
				}
				break;
			case DetectionCameraController.CameraPhase.Emote:
				if (this.EmoteCameraHintUI != null)
				{
					this.EmoteCameraHintUI.SetActive(true);
					return;
				}
				break;
			case DetectionCameraController.CameraPhase.TV:
				if (this.TvCameraHintUI != null)
				{
					this.TvCameraHintUI.SetActive(true);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06009175 RID: 37237 RVA: 0x003B8E30 File Offset: 0x003B7030
		private void HideAllHints()
		{
			if (this.NpcCameraHintUI != null)
			{
				this.NpcCameraHintUI.SetActive(false);
			}
			if (this.EmoteCameraHintUI != null)
			{
				this.EmoteCameraHintUI.SetActive(false);
			}
			if (this.TvCameraHintUI != null)
			{
				this.TvCameraHintUI.SetActive(false);
			}
		}

		// Token: 0x06009176 RID: 37238 RVA: 0x003B8E8C File Offset: 0x003B708C
		private void ZeroDampingOnVCam()
		{
			CinemachineTransposer cinemachineComponent = this.ManagedVCam.GetCinemachineComponent<CinemachineTransposer>();
			if (cinemachineComponent != null)
			{
				cinemachineComponent.m_XDamping = 0f;
				cinemachineComponent.m_YDamping = 0f;
				cinemachineComponent.m_ZDamping = 0f;
				cinemachineComponent.m_YawDamping = 0f;
				cinemachineComponent.m_PitchDamping = 0f;
				cinemachineComponent.m_RollDamping = 0f;
			}
			CinemachineComposer cinemachineComponent2 = this.ManagedVCam.GetCinemachineComponent<CinemachineComposer>();
			if (cinemachineComponent2 != null)
			{
				cinemachineComponent2.m_HorizontalDamping = 0f;
				cinemachineComponent2.m_VerticalDamping = 0f;
			}
		}

		// Token: 0x06009177 RID: 37239 RVA: 0x003B8F1C File Offset: 0x003B711C
		private void RestoreDampingOnVCam(DetectionCameraPresetEntry preset)
		{
			CinemachineTransposer cinemachineComponent = this.ManagedVCam.GetCinemachineComponent<CinemachineTransposer>();
			if (cinemachineComponent != null)
			{
				if (preset.TargetMode == CameraTargetMode.Orbital)
				{
					cinemachineComponent.m_XDamping = preset.BodyXDamping;
					cinemachineComponent.m_YDamping = preset.BodyYDamping;
					cinemachineComponent.m_ZDamping = preset.BodyZDamping;
					cinemachineComponent.m_YawDamping = preset.BodyYawDamping;
					cinemachineComponent.m_PitchDamping = preset.BodyPitchDamping;
					cinemachineComponent.m_RollDamping = preset.BodyRollDamping;
				}
				else if (preset.TargetMode == CameraTargetMode.FollowOnly)
				{
					cinemachineComponent.m_XDamping = preset.FollowDamping;
					cinemachineComponent.m_YDamping = preset.FollowDamping;
					cinemachineComponent.m_ZDamping = preset.FollowDamping;
				}
			}
			CinemachineComposer cinemachineComponent2 = this.ManagedVCam.GetCinemachineComponent<CinemachineComposer>();
			if (cinemachineComponent2 != null && preset.TargetMode == CameraTargetMode.Orbital)
			{
				cinemachineComponent2.m_HorizontalDamping = preset.AimHorizontalDamping;
				cinemachineComponent2.m_VerticalDamping = preset.AimVerticalDamping;
			}
		}

		// Token: 0x06009178 RID: 37240 RVA: 0x003B8FF2 File Offset: 0x003B71F2
		private IEnumerator RestoreDampingCoroutine(DetectionCameraPresetEntry preset, float delay)
		{
			if (delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}
			else
			{
				yield return null;
			}
			this._dampingRestoreCoroutine = null;
			if (this._isActive && this.ManagedVCam != null)
			{
				this.RestoreDampingOnVCam(preset);
			}
			yield break;
		}

		// Token: 0x06009179 RID: 37241 RVA: 0x003B9010 File Offset: 0x003B7210
		private static Transform FindBone(Transform root, string boneName)
		{
			if (root == null || string.IsNullOrEmpty(boneName))
			{
				return null;
			}
			Animator componentInChildren = root.GetComponentInChildren<Animator>();
			if (componentInChildren != null && componentInChildren.isHuman)
			{
				if (boneName == "Head")
				{
					return componentInChildren.GetBoneTransform(HumanBodyBones.Head);
				}
				if (boneName == "Spine1" || boneName == "Chest")
				{
					return componentInChildren.GetBoneTransform(HumanBodyBones.Chest);
				}
				if (boneName == "Spine")
				{
					return componentInChildren.GetBoneTransform(HumanBodyBones.Spine);
				}
				if (boneName == "Hips")
				{
					return componentInChildren.GetBoneTransform(HumanBodyBones.Hips);
				}
				if (boneName == "UpperChest")
				{
					return componentInChildren.GetBoneTransform(HumanBodyBones.UpperChest);
				}
				if (boneName == "Neck")
				{
					return componentInChildren.GetBoneTransform(HumanBodyBones.Neck);
				}
			}
			return DetectionCameraController.FindDeepChild(root, boneName);
		}

		// Token: 0x0600917A RID: 37242 RVA: 0x003B90E4 File Offset: 0x003B72E4
		private static Transform FindDeepChild(Transform parent, string name)
		{
			if (parent.name == name)
			{
				return parent;
			}
			foreach (object obj in parent)
			{
				Transform transform = DetectionCameraController.FindDeepChild((Transform)obj, name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x0400956C RID: 38252
		[Header("Managed VCam")]
		[Tooltip("The single Cinemachine VCam this controller drives. Created at runtime if not assigned.")]
		public CinemachineVirtualCamera ManagedVCam;

		// Token: 0x0400956D RID: 38253
		[Header("Output Override (opzionale)")]
		[Tooltip("Camera da usare al posto di Camera.main per il brain lookup.\nPer la TV studio: assegna qui la Camera che renderizza alla RenderTexture del monitor, e mettila + il ManagedVCam su un layer dedicato (es. 'TVCameraOnly') incluso SOLO nel cullingMask di questa Camera. Cinemachine 2.10 filtra i VCam tramite il cullingMask della Camera output del brain, quindi la main brain non li vedrà e i due flussi restano isolati.")]
		public Camera OutputCameraOverride;

		// Token: 0x0400956E RID: 38254
		[Header("Input")]
		[Tooltip("Input to cycle to the next camera preset")]
		public GenericInput CycleCameraInput = new GenericInput("Mouse1", "RightStickButton", "RightStickButton");

		// Token: 0x0400956F RID: 38255
		[Header("Hint UI")]
		[Tooltip("GameObject shown when camera switching is available (NPC phase)")]
		public GameObject NpcCameraHintUI;

		// Token: 0x04009570 RID: 38256
		[Tooltip("GameObject shown when camera switching is available (Emote phase)")]
		public GameObject EmoteCameraHintUI;

		// Token: 0x04009571 RID: 38257
		[Tooltip("GameObject shown when camera switching is available (TV Studio phase)")]
		public GameObject TvCameraHintUI;

		// Token: 0x04009572 RID: 38258
		[Header("Cinemachine")]
		[Tooltip("Base priority for the managed VCam (NPC + Emote phase).")]
		public int BasePriority = 20;

		// Token: 0x04009573 RID: 38259
		[Tooltip("Priority used during TV phase. Default 100, abbastanza alto da vincere su qualunque vcam dello studio (studioViewVCam settled = 35, tvMonitorVCam toggled = 40). Senza questo boost, il TV brain con cullingMask 'condiviso' col main camera vedrebbe la studio cam come highest priority e la TV proietterebbe lo studio invece dei preset.")]
		public int TVPhasePriority = 100;

		// Token: 0x04009574 RID: 38260
		[Header("Debug")]
		public bool DebugMode;

		// Token: 0x04009575 RID: 38261
		private DetectionCameraPresetCollectionSO _activeCollection;

		// Token: 0x04009576 RID: 38262
		private int _currentPresetIndex;

		// Token: 0x04009577 RID: 38263
		private Transform _playerTransform;

		// Token: 0x04009578 RID: 38264
		private Transform _npcTransform;

		// Token: 0x04009579 RID: 38265
		private CinemachineBrain _brain;

		// Token: 0x0400957A RID: 38266
		private bool _isActive;

		// Token: 0x0400957B RID: 38267
		private bool _isFlyModeActive;

		// Token: 0x0400957C RID: 38268
		private DetectionShotCameraExtension _shotProtectionExtension;

		// Token: 0x0400957D RID: 38269
		private DetectionCameraController.CameraPhase _currentPhase;

		// Token: 0x0400957E RID: 38270
		private float _mobileTimer;

		// Token: 0x0400957F RID: 38271
		private bool _isMobilePreset;

		// Token: 0x04009580 RID: 38272
		private bool _craneFinished;

		// Token: 0x04009581 RID: 38273
		private Coroutine _dampingRestoreCoroutine;

		// Token: 0x04009582 RID: 38274
		private int _dollyCurrentWaypoint;

		// Token: 0x04009583 RID: 38275
		private int _dollyDirection = 1;

		// Token: 0x04009584 RID: 38276
		private float _dollySegmentTimer;

		// Token: 0x04009585 RID: 38277
		private bool _dollyHolding;

		// Token: 0x04009586 RID: 38278
		private float _currentCollisionPullback;

		// Token: 0x04009587 RID: 38279
		private readonly RaycastHit[] _sphereCastHits = new RaycastHit[4];

		// Token: 0x02001241 RID: 4673
		private enum CameraPhase
		{
			// Token: 0x04009589 RID: 38281
			None,
			// Token: 0x0400958A RID: 38282
			NPC,
			// Token: 0x0400958B RID: 38283
			Emote,
			// Token: 0x0400958C RID: 38284
			TV
		}
	}
}
