using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using BoZo.CharacterCreator;
using Features.BodyPaintStudio;
using Features.CielVoice;
using Features.DebugTools;
using Features.Detection;
using Invector.vCamera;
using Invector.vCharacterController;
using MagicaCloth2;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Futanari;
using ProjectExhibitionist.Interactions;
using Unity.Collections;
using UnityEngine;

namespace Features.BoobTouch
{
	// Token: 0x020014D3 RID: 5331
	[RequireComponent(typeof(BodyPaintOrbitCamera))]
	[DefaultExecutionOrder(100)]
	public class BoobTouchModeController : MonoBehaviour
	{
		// Token: 0x1400011E RID: 286
		// (add) Token: 0x0600A3F1 RID: 41969 RVA: 0x00442020 File Offset: 0x00440220
		// (remove) Token: 0x0600A3F2 RID: 41970 RVA: 0x00442058 File Offset: 0x00440258
		public event Action<int, int, BoobTouchModeController.ExternalGrabKind, Vector3, Vector3> ExternalBreastGrabSolved;

		// Token: 0x1700192C RID: 6444
		// (get) Token: 0x0600A3F3 RID: 41971 RVA: 0x0044208D File Offset: 0x0044028D
		public bool IsActive
		{
			get
			{
				return this._active;
			}
		}

		// Token: 0x1700192D RID: 6445
		// (get) Token: 0x0600A3F4 RID: 41972 RVA: 0x00442095 File Offset: 0x00440295
		public InteractionConfigSO ModeControlsConfig
		{
			get
			{
				return this._modeControlsConfig;
			}
		}

		// Token: 0x1700192E RID: 6446
		// (get) Token: 0x0600A3F5 RID: 41973 RVA: 0x004420A0 File Offset: 0x004402A0
		public bool IsThirdHandPoseAvailable
		{
			get
			{
				if (this._futanariMode == null)
				{
					Transform transform = ((this._playerRoot != null) ? this._playerRoot : base.transform.root);
					this._futanariMode = transform.GetComponentInChildren<FutanariModeController>(true);
				}
				return this._futanariMode == null || !this._futanariMode.FutanariMode;
			}
		}

		// Token: 0x1700192F RID: 6447
		// (get) Token: 0x0600A3F6 RID: 41974 RVA: 0x00442108 File Offset: 0x00440308
		public bool IsTransitioning
		{
			get
			{
				return this._transitioning;
			}
		}

		// Token: 0x17001930 RID: 6448
		// (get) Token: 0x0600A3F7 RID: 41975 RVA: 0x00442110 File Offset: 0x00440310
		public bool IsExternalInteractorMode
		{
			get
			{
				return this._active && this._externalInteractorMode;
			}
		}

		// Token: 0x17001931 RID: 6449
		// (get) Token: 0x0600A3F8 RID: 41976 RVA: 0x00442122 File Offset: 0x00440322
		public bool IsExternalInteractionReady
		{
			get
			{
				return this.IsExternalInteractorMode && !this._transitioning && this._surfaceReady && this._breastTips.Count > 0;
			}
		}

		// Token: 0x17001932 RID: 6450
		// (get) Token: 0x0600A3F9 RID: 41977 RVA: 0x0044214C File Offset: 0x0044034C
		public bool InteractionSurfaceReady
		{
			get
			{
				return this._surfaceReady;
			}
		}

		// Token: 0x17001933 RID: 6451
		// (get) Token: 0x0600A3FA RID: 41978 RVA: 0x00442154 File Offset: 0x00440354
		public int ExternalBodyColliderCount
		{
			get
			{
				return this._externalBodyColliders.Count;
			}
		}

		// Token: 0x17001934 RID: 6452
		// (get) Token: 0x0600A3FB RID: 41979 RVA: 0x00442161 File Offset: 0x00440361
		public int ExternalGrabCount
		{
			get
			{
				return this._externalBreastGrabs.Count;
			}
		}

		// Token: 0x17001935 RID: 6453
		// (get) Token: 0x0600A3FC RID: 41980 RVA: 0x0044216E File Offset: 0x0044036E
		public int ExternalBreastAnchorCount
		{
			get
			{
				return this._breastTips.Count;
			}
		}

		// Token: 0x17001936 RID: 6454
		// (get) Token: 0x0600A3FD RID: 41981 RVA: 0x0044217B File Offset: 0x0044037B
		public Transform InteractionChestAnchor
		{
			get
			{
				return this._chestAnchor;
			}
		}

		// Token: 0x17001937 RID: 6455
		// (get) Token: 0x0600A3FE RID: 41982 RVA: 0x00442183 File Offset: 0x00440383
		public Camera InteractionCamera
		{
			get
			{
				return this._mainCamera;
			}
		}

		// Token: 0x17001938 RID: 6456
		// (get) Token: 0x0600A3FF RID: 41983 RVA: 0x0044218B File Offset: 0x0044038B
		public Animator InteractionAnimator
		{
			get
			{
				return this._animator;
			}
		}

		// Token: 0x17001939 RID: 6457
		// (get) Token: 0x0600A400 RID: 41984 RVA: 0x00442193 File Offset: 0x00440393
		public int CurrentBoobsTypeValue
		{
			get
			{
				return this._breastSizeProfile.boobsType;
			}
		}

		// Token: 0x1700193A RID: 6458
		// (get) Token: 0x0600A401 RID: 41985 RVA: 0x004421A0 File Offset: 0x004403A0
		public string CurrentBreastSizeLabel
		{
			get
			{
				return this._breastSizeProfile.label;
			}
		}

		// Token: 0x1700193B RID: 6459
		// (get) Token: 0x0600A402 RID: 41986 RVA: 0x004421AD File Offset: 0x004403AD
		public float CurrentBreastInteractionScale
		{
			get
			{
				return this._breastSizeProfile.interactionScale;
			}
		}

		// Token: 0x1700193C RID: 6460
		// (get) Token: 0x0600A403 RID: 41987 RVA: 0x004421BA File Offset: 0x004403BA
		public float CurrentBreastClearanceScale
		{
			get
			{
				return this._breastSizeProfile.clearanceScale;
			}
		}

		// Token: 0x1700193D RID: 6461
		// (get) Token: 0x0600A404 RID: 41988 RVA: 0x004421C7 File Offset: 0x004403C7
		public float CurrentBreastMagnetScale
		{
			get
			{
				return this._breastSizeProfile.magnetRadiusScale;
			}
		}

		// Token: 0x1700193E RID: 6462
		// (get) Token: 0x0600A405 RID: 41989 RVA: 0x004421D4 File Offset: 0x004403D4
		public float CurrentBreastFullGrabMotionScale
		{
			get
			{
				return this._breastSizeProfile.fullGrabMotionScale;
			}
		}

		// Token: 0x1700193F RID: 6463
		// (get) Token: 0x0600A406 RID: 41990 RVA: 0x004421E1 File Offset: 0x004403E1
		public float CurrentBreastNippleMotionScale
		{
			get
			{
				return this._breastSizeProfile.nippleMotionScale;
			}
		}

		// Token: 0x17001940 RID: 6464
		// (get) Token: 0x0600A407 RID: 41991 RVA: 0x004421EE File Offset: 0x004403EE
		public float CurrentBreastChestWidthScale
		{
			get
			{
				return this._breastSizeProfile.chestWidthScale;
			}
		}

		// Token: 0x17001941 RID: 6465
		// (get) Token: 0x0600A408 RID: 41992 RVA: 0x004421FB File Offset: 0x004403FB
		public float CurrentBreastChestDepthScale
		{
			get
			{
				return this._breastSizeProfile.chestDepthScale;
			}
		}

		// Token: 0x17001942 RID: 6466
		// (get) Token: 0x0600A409 RID: 41993 RVA: 0x00442208 File Offset: 0x00440408
		public bool IsFlatBreastPressMode
		{
			get
			{
				return this._breastSizeProfile.flatPressMode;
			}
		}

		// Token: 0x0600A40A RID: 41994 RVA: 0x00442218 File Offset: 0x00440418
		public bool TryGetNearestExternalBreastAnchor(Vector3 worldPoint, out int breastIndex, out Vector3 anchorWorld, out Vector3 outwardWorld)
		{
			breastIndex = -1;
			anchorWorld = Vector3.zero;
			outwardWorld = Vector3.forward;
			float num = float.PositiveInfinity;
			for (int i = 0; i < this._breastTips.Count; i++)
			{
				BoobTouchModeController.BreastTipVisualState breastTipVisualState = this._breastTips[i];
				if (!(breastTipVisualState.renderBone01 == null))
				{
					Vector3 vector;
					Vector3 vector2;
					this.ResolveExternalBreastAnchor(breastTipVisualState, out vector, out vector2);
					float sqrMagnitude = (worldPoint - vector).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						breastIndex = i;
						anchorWorld = vector;
						outwardWorld = vector2;
					}
				}
			}
			return breastIndex >= 0;
		}

		// Token: 0x0600A40B RID: 41995 RVA: 0x004422B8 File Offset: 0x004404B8
		public bool TryGetExternalBreastAnchor(int breastIndex, out Vector3 anchorWorld, out Vector3 outwardWorld)
		{
			anchorWorld = Vector3.zero;
			outwardWorld = Vector3.forward;
			if (breastIndex < 0 || breastIndex >= this._breastTips.Count)
			{
				return false;
			}
			BoobTouchModeController.BreastTipVisualState breastTipVisualState = this._breastTips[breastIndex];
			if (breastTipVisualState.renderBone01 == null)
			{
				return false;
			}
			this.ResolveExternalBreastAnchor(breastTipVisualState, out anchorWorld, out outwardWorld);
			return true;
		}

		// Token: 0x0600A40C RID: 41996 RVA: 0x00442318 File Offset: 0x00440518
		public bool TryGetExternalBreastBoneAnchor(int breastIndex, out Vector3 anchorWorld, out Vector3 outwardWorld)
		{
			anchorWorld = Vector3.zero;
			outwardWorld = Vector3.forward;
			if (breastIndex < 0 || breastIndex >= this._breastTips.Count)
			{
				return false;
			}
			BoobTouchModeController.BreastTipVisualState breastTipVisualState = this._breastTips[breastIndex];
			if (breastTipVisualState.renderBone01 == null)
			{
				return false;
			}
			anchorWorld = breastTipVisualState.renderBone01.position;
			outwardWorld = BoobTouchModeController.BreastOutward(breastTipVisualState);
			return true;
		}

		// Token: 0x0600A40D RID: 41997 RVA: 0x0044238C File Offset: 0x0044058C
		public bool TryAcquireExternalBreastGrab(Vector3 worldPoint, BoobTouchModeController.ExternalGrabKind kind, out int breastIndex, out Vector3 anchorWorld, out Vector3 outwardWorld)
		{
			if (!this._active || !this._externalInteractorMode || this._transitioning || !this.TryGetNearestExternalBreastAnchor(worldPoint, out breastIndex, out anchorWorld, out outwardWorld))
			{
				breastIndex = -1;
				anchorWorld = Vector3.zero;
				outwardWorld = Vector3.forward;
				return false;
			}
			float num = ((kind == BoobTouchModeController.ExternalGrabKind.Nipple) ? (this.externalNippleGrabAcquireRadius * this._breastSizeProfile.magnetRadiusScale) : (this.externalBreastGrabAcquireRadius * this._breastSizeProfile.acquireRadiusScale));
			return Vector3.Distance(worldPoint, anchorWorld) <= Mathf.Max(0.01f, num);
		}

		// Token: 0x0600A40E RID: 41998 RVA: 0x00442424 File Offset: 0x00440624
		public void UpdateExternalBreastGrab(int ownerId, int breastIndex, BoobTouchModeController.ExternalGrabKind kind, Vector3 handPointWorld, float strength)
		{
			if (!this._active || !this._externalInteractorMode || ownerId == 0 || breastIndex < 0 || breastIndex >= this._breastTips.Count)
			{
				return;
			}
			BoobTouchModeController.ExternalBreastGrabState externalBreastGrabState;
			if (!this._externalBreastGrabs.TryGetValue(ownerId, out externalBreastGrabState) || externalBreastGrabState.breastIndex != breastIndex || externalBreastGrabState.kind != kind)
			{
				externalBreastGrabState = new BoobTouchModeController.ExternalBreastGrabState
				{
					breastIndex = breastIndex,
					kind = kind
				};
				this._externalBreastGrabs[ownerId] = externalBreastGrabState;
			}
			externalBreastGrabState.handPointWorld = handPointWorld;
			externalBreastGrabState.strength = Mathf.Clamp01(strength);
			externalBreastGrabState.updatedFrame = Time.frameCount;
		}

		// Token: 0x0600A40F RID: 41999 RVA: 0x004424BA File Offset: 0x004406BA
		public void ReleaseExternalBreastGrab(int ownerId)
		{
			if (ownerId != 0)
			{
				this._externalBreastGrabs.Remove(ownerId);
			}
		}

		// Token: 0x0600A410 RID: 42000 RVA: 0x004424CC File Offset: 0x004406CC
		public void ReleaseAllExternalBreastGrabs()
		{
			this._externalBreastGrabs.Clear();
			this._staleExternalGrabOwners.Clear();
		}

		// Token: 0x0600A411 RID: 42001 RVA: 0x004424E4 File Offset: 0x004406E4
		public Vector3 ConstrainExternalGrabMotion(BoobTouchModeController.ExternalGrabKind kind, Vector3 originWorld, Vector3 desiredWorld)
		{
			return originWorld + this.ConstrainExternalGrabDelta(desiredWorld - originWorld, kind);
		}

		// Token: 0x0600A412 RID: 42002 RVA: 0x004424FA File Offset: 0x004406FA
		public void ReportExternalBreastTouch(float amount)
		{
			if (!this._active || !this._externalInteractorMode)
			{
				return;
			}
			this._externalBreastTouch = Mathf.Clamp01(amount);
			this._externalBreastTouchFrame = Time.frameCount;
		}

		// Token: 0x17001943 RID: 6467
		// (get) Token: 0x0600A413 RID: 42003 RVA: 0x00442524 File Offset: 0x00440724
		private int EffectiveModeFaceIndex
		{
			get
			{
				if (this._modeFaceOverrideIndex < 0)
				{
					return this._modeFaceIndex;
				}
				return this._modeFaceOverrideIndex;
			}
		}

		// Token: 0x0600A414 RID: 42004 RVA: 0x0044253C File Offset: 0x0044073C
		public void SetModeFaceOverride(int faceIndex)
		{
			if (this._modeFaceOverrideIndex == faceIndex)
			{
				return;
			}
			this._modeFaceOverrideIndex = faceIndex;
			this.MaintainModeFace();
		}

		// Token: 0x0600A415 RID: 42005 RVA: 0x00442555 File Offset: 0x00440755
		public void ClearModeFaceOverride()
		{
			if (this._modeFaceOverrideIndex < 0)
			{
				return;
			}
			this._modeFaceOverrideIndex = -1;
			this.MaintainModeFace();
		}

		// Token: 0x0600A416 RID: 42006 RVA: 0x0044256E File Offset: 0x0044076E
		public void SetModeEyesOpenOverride(bool open)
		{
			if (!this._externalInteractorMode || this._modeEyesOpenOverride == open)
			{
				return;
			}
			this._modeEyesOpenOverride = open;
			this.RestoreClosedEyes();
			if (open)
			{
				this.ApplyOpenEyes();
			}
		}

		// Token: 0x17001944 RID: 6468
		// (get) Token: 0x0600A417 RID: 42007 RVA: 0x00442598 File Offset: 0x00440798
		public int CurrentModeFaceIndex
		{
			get
			{
				return this.EffectiveModeFaceIndex;
			}
		}

		// Token: 0x17001945 RID: 6469
		// (get) Token: 0x0600A418 RID: 42008 RVA: 0x004425A0 File Offset: 0x004407A0
		public bool IsModeEyesOpenOverridden
		{
			get
			{
				return this._modeEyesOpenOverride;
			}
		}

		// Token: 0x17001946 RID: 6470
		// (get) Token: 0x0600A419 RID: 42009 RVA: 0x004425A8 File Offset: 0x004407A8
		public float CurrentNippleContact
		{
			get
			{
				if (!this._active || this._transitioning)
				{
					return 0f;
				}
				if (this._externalInteractorMode)
				{
					if (Time.frameCount - this._externalBreastTouchFrame > 2)
					{
						return 0f;
					}
					return this._externalBreastTouch;
				}
				else
				{
					if (this._pressureColliderTransform == null)
					{
						return 0f;
					}
					float num = 0f;
					foreach (BoobTouchModeController.BreastTipVisualState breastTipVisualState in this._breastTips)
					{
						if (!(breastTipVisualState.renderBone01 == null))
						{
							float num2 = this.BreastTipSoapProximity(breastTipVisualState.renderBone01);
							if (num2 > num)
							{
								num = num2;
							}
						}
					}
					return num;
				}
			}
		}

		// Token: 0x0600A41A RID: 42010 RVA: 0x0044266C File Offset: 0x0044086C
		private void Awake()
		{
			this._orbitCamera = base.GetComponent<BodyPaintOrbitCamera>();
			this._playerInput = base.GetComponentInParent<vThirdPersonInput>();
			this._animator = base.GetComponentInParent<Animator>();
			this._playerRoot = ((this._playerInput != null) ? this._playerInput.transform : ((this._animator != null) ? this._animator.transform : base.transform.root));
			this._actionAnimator = this._playerRoot.GetComponentInChildren<PlayerActionAnimator>(true);
			if (this._actionAnimator == null)
			{
				this._actionAnimator = Object.FindObjectOfType<PlayerActionAnimator>();
			}
			this._emotionController = this._playerRoot.GetComponentInChildren<PlayerEmotionController>(true);
			this._wetController = this._playerRoot.GetComponentInChildren<CielWetController>(true);
			this._boobsController = this._playerRoot.GetComponentInChildren<BoobsController>(true);
			this.RefreshBreastSizeProfile();
		}

		// Token: 0x0600A41B RID: 42011 RVA: 0x0044274C File Offset: 0x0044094C
		public bool RefreshBreastSizeProfile()
		{
			if (this._boobsController == null && this._playerRoot != null)
			{
				this._boobsController = this._playerRoot.GetComponentInChildren<BoobsController>(true);
			}
			int num = ((this._boobsController != null) ? this._boobsController.CurrentBoobsTypeValue : 3);
			if (this._breastSizeProfile.boobsType == num)
			{
				return false;
			}
			this._breastSizeProfile = BreastInteractionSizing.Resolve(num);
			return true;
		}

		// Token: 0x0600A41C RID: 42012 RVA: 0x004427C4 File Offset: 0x004409C4
		private void Update()
		{
			if (this.toggleKey != KeyCode.None && GameplayInput.GetKeyDown(this.toggleKey) && this.CanToggle())
			{
				if (this._active)
				{
					this.ExitMode();
					return;
				}
				this.EnterMode();
				return;
			}
			else
			{
				if (!this._active)
				{
					return;
				}
				if (this._externalInteractorMode && this._modeControlsThirdPoseAvailable != this.IsThirdHandPoseAvailable)
				{
					this.ShowModeControlsHud();
				}
				if (this._externalInteractorMode && this.RefreshBreastSizeProfile())
				{
					this.ReleaseAllExternalBreastGrabs();
					this._orbitCamera.EnableZoom(this.gropingCameraDistance * this._breastSizeProfile.cameraDistanceScale, this.gropingCameraMinDistance * this._breastSizeProfile.cameraDistanceScale, this.gropingCameraMaxDistance * this._breastSizeProfile.cameraDistanceScale);
				}
				if (!this._externalInteractorMode && !this._transitioning && EscapeInputCoordinator.TryConsume(this))
				{
					this.ExitMode();
					return;
				}
				return;
			}
		}

		// Token: 0x0600A41D RID: 42013 RVA: 0x0044289F File Offset: 0x00440A9F
		private void LateUpdate()
		{
			if (this._active)
			{
				if (!this._externalInteractorMode)
				{
					this.TickBall();
				}
				this.MaintainModeFace();
				this.MaintainClosedEyes();
			}
		}

		// Token: 0x0600A41E RID: 42014 RVA: 0x004428C4 File Offset: 0x00440AC4
		private void FixedUpdate()
		{
			if (!this._active || this._soapRb == null || !this._physTargetValid)
			{
				return;
			}
			if (this._autoPushRoutine != null)
			{
				this._soapRb.position = this._ballRoot.transform.position + this._surfaceOffset;
				this._soapRb.velocity = Vector3.zero;
				this._soapRb.angularVelocity = Vector3.zero;
				return;
			}
			float num = this.soapDragGain * Mathf.Lerp(1f, this.soapPressDriveMultiplier, this._pressAmount);
			Vector3 vector = Vector3.ClampMagnitude((this._physTargetPos - this._soapRb.position) * num, this.soapMaxSpeed);
			float num2 = BoobTouchModeController.ExpBlend(this.soapVelocityResponse, Time.fixedDeltaTime);
			this._soapRb.velocity = Vector3.Lerp(this._soapRb.velocity, vector, num2);
			float num3;
			Vector3 vector2;
			(this._physTargetRot * Quaternion.Inverse(this._soapRb.rotation)).ToAngleAxis(out num3, out vector2);
			if (num3 > 180f)
			{
				num3 -= 360f;
			}
			if (!float.IsNaN(vector2.x) && !float.IsInfinity(vector2.x) && vector2.sqrMagnitude > 0.5f)
			{
				float num4 = this.soapRotationGain * Mathf.Lerp(1f, this.soapPressRotationMultiplier, this._pressAmount);
				Vector3 vector3 = vector2.normalized * (num3 * 0.017453292f * num4);
				float num5 = BoobTouchModeController.ExpBlend(this.soapAngularResponse, Time.fixedDeltaTime);
				this._soapRb.angularVelocity = Vector3.Lerp(this._soapRb.angularVelocity, vector3, num5);
			}
		}

		// Token: 0x0600A41F RID: 42015 RVA: 0x00442A83 File Offset: 0x00440C83
		private static float ExpBlend(float sharpness, float deltaTime)
		{
			if (sharpness > 0f)
			{
				return 1f - Mathf.Exp(-sharpness * Mathf.Max(0f, deltaTime));
			}
			return 1f;
		}

		// Token: 0x0600A420 RID: 42016 RVA: 0x00442AAC File Offset: 0x00440CAC
		private bool CanToggle()
		{
			if (this._transitioning)
			{
				return false;
			}
			if (this._active)
			{
				return true;
			}
			if (!PlayerActivityGate.Allows(PlayerActivity.Interact) || PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem))
			{
				return false;
			}
			if (FlyCamera.IsActive)
			{
				return false;
			}
			if (this._playerInput == null || this._playerInput.cc == null)
			{
				return false;
			}
			vThirdPersonController cc = this._playerInput.cc;
			return cc.isGrounded && !cc.customAction && !cc.ragdolled;
		}

		// Token: 0x0600A421 RID: 42017 RVA: 0x00442B34 File Offset: 0x00440D34
		public bool TryEnterMode()
		{
			if (this._active || !this.CanToggle())
			{
				return false;
			}
			this.EnterMode();
			return this._active;
		}

		// Token: 0x0600A422 RID: 42018 RVA: 0x00442B54 File Offset: 0x00440D54
		public bool TryEnterGropingMode()
		{
			if (this._active || this._transitioning || !this.CanToggle())
			{
				return false;
			}
			this._externalInteractorMode = true;
			this.EnterMode();
			if (!this._active)
			{
				this._externalInteractorMode = false;
			}
			return this._active;
		}

		// Token: 0x0600A423 RID: 42019 RVA: 0x00442B92 File Offset: 0x00440D92
		public void ExitGropingMode()
		{
			if (this._active && this._externalInteractorMode)
			{
				this.ExitMode();
			}
		}

		// Token: 0x0600A424 RID: 42020 RVA: 0x00442BAC File Offset: 0x00440DAC
		public void EnterMode()
		{
			if (this._active || !this.CanToggle())
			{
				return;
			}
			if (!this.FindBoobCloths())
			{
				Debug.LogWarning("[BoobTouchModeController] Cloth tette non trovati: esegui Tools > Boob Touch > Setup Player Prefab.", this);
				return;
			}
			this.RefreshBreastSizeProfile();
			if (!this.TryAcquireActivityContext())
			{
				return;
			}
			if (!base.isActiveAndEnabled || this._modeLease == null || !this._modeLease.IsActive)
			{
				this.ReleaseActivityContext();
				return;
			}
			try
			{
				this.SuspendFirstPersonView();
				this._active = true;
				this._modeEyesOpenOverride = false;
				if (this._playerInput != null)
				{
					this._prevLockInput = this._playerInput.lockInput;
					this._prevLockCamera = this._playerInput.lockCameraInput;
					this._playerInput.SetLockAllInput(true);
					this._playerInput.SetLockCameraInput(true);
					if (this._playerInput.cc != null)
					{
						this._playerInput.cc.input = Vector2.zero;
						this._playerInput.cc.isSprinting = false;
					}
				}
				Cursor.lockState = (this._externalInteractorMode ? CursorLockMode.Confined : CursorLockMode.None);
				Cursor.visible = false;
				if (this._actionAnimator != null)
				{
					this._actionAnimator.DisableEmotionLayers();
				}
				this.ApplyModeFace();
				if (this._externalInteractorMode)
				{
					this.RestoreClosedEyes();
				}
				else
				{
					this.ApplyClosedEyes();
				}
				if (!this._externalInteractorMode)
				{
					this.AcquireModeWet();
				}
				this.SetPose(true);
				this._mainCamera = Camera.main;
				vThirdPersonCamera vThirdPersonCamera = ((this._playerInput != null) ? this._playerInput.tpCamera : vThirdPersonCamera.instance);
				this._orbitCamera.Activate(vThirdPersonCamera, this._playerRoot);
				if (this._externalInteractorMode)
				{
					this._orbitCamera.EnableZoom(this.gropingCameraDistance * this._breastSizeProfile.cameraDistanceScale, this.gropingCameraMinDistance * this._breastSizeProfile.cameraDistanceScale, this.gropingCameraMaxDistance * this._breastSizeProfile.cameraDistanceScale);
					this._orbitCamera.FollowAnimatedPivotFull(this._chestAnchor, Vector3.zero);
				}
				else
				{
					this._orbitCamera.EnableZoom(this.soapCameraDistance, this.soapCameraMinDistance, this.soapCameraMaxDistance);
					this._orbitCamera.FollowAnimatedPivot(this._chestAnchor);
				}
				this._orbitCamera.EnableObstacleAvoidance(true);
				this.HideGameHUD();
				this.ShowModeControlsHud();
				if (this._canvasService == null)
				{
					this._canvasService = this._playerRoot.GetComponentInChildren<BodyPaintCanvasService>(true);
				}
				if (this._canvasService == null)
				{
					Debug.LogWarning("[BoobTouchModeController] BodyPaintCanvasService non trovato: il soap usera' solo il piano di fallback.", this);
				}
				else
				{
					this._canvasService.EnsureOverlays();
					this._canvasService.DestroyPaintColliders();
				}
				this.SubscribeSurfaceBake();
				this._surfaceReady = false;
				if (this._externalInteractorMode)
				{
					this._surfaceOffset = ((this._canvasService != null) ? this._canvasService.ColliderOffset : Vector3.zero);
					this.CreateSoapSurfaceProxy();
				}
				else
				{
					this.CreateBall();
				}
				this._clothRoutine = base.StartCoroutine(this.EnterClothRoutine());
				if (this.debugLogs)
				{
					Debug.Log("[BoobTouchModeController] Enter boob touch mode");
				}
			}
			catch
			{
				this.ExitMode();
				throw;
			}
		}

		// Token: 0x0600A425 RID: 42021 RVA: 0x00442EDC File Offset: 0x004410DC
		public void ExitMode()
		{
			if (!this._active)
			{
				return;
			}
			ControllerWorldPointer.Release(this);
			this.ReleaseAllExternalBreastGrabs();
			this._externalBreastTouch = 0f;
			this._externalBreastTouchFrame = -10;
			this.RestoreBreastTipRotations();
			this._active = false;
			this._modeEyesOpenOverride = false;
			this.RestoreClosedEyes();
			this.RestoreModeFace();
			this.ReleaseModeWet();
			if (this._clothRoutine != null)
			{
				base.StopCoroutine(this._clothRoutine);
				this._clothRoutine = null;
			}
			this.UnsubscribeSurfaceBake();
			this._breastTips.Clear();
			this._surfaceReady = false;
			if (this._canvasService != null)
			{
				this._canvasService.DestroyPaintColliders();
			}
			this.UnregisterBallCollider();
			this.RestoreClothSetup();
			this.DestroyBall();
			this.HideModeControlsHud();
			this.RestoreGameHUD();
			this.SetPose(false);
			if (this._actionAnimator != null)
			{
				this._actionAnimator.RestoreOverrideLayer();
			}
			this._orbitCamera.Deactivate();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			if (this._playerInput != null)
			{
				this._playerInput.SetLockAllInput(this._prevLockInput);
				this._playerInput.SetLockCameraInput(this._prevLockCamera);
			}
			this.ReleaseActivityContext();
			this._externalInteractorMode = false;
			if (this.debugLogs)
			{
				Debug.Log("[BoobTouchModeController] Exit boob touch mode");
			}
		}

		// Token: 0x0600A426 RID: 42022 RVA: 0x00443028 File Offset: 0x00441228
		private void OnDisable()
		{
			if (this._active)
			{
				this.ExitMode();
			}
			else
			{
				this.ReleaseAllExternalBreastGrabs();
				this.RestoreBreastTipRotations();
				this._breastTips.Clear();
				this.UnsubscribeSurfaceBake();
				this._modeEyesOpenOverride = false;
				this.RestoreClosedEyes();
				this.RestoreModeFace();
				this.ReleaseModeWet();
			}
			this.ReleaseActivityContext();
		}

		// Token: 0x0600A427 RID: 42023 RVA: 0x00443081 File Offset: 0x00441281
		private void OnDestroy()
		{
			this.ReleaseActivityContext();
		}

		// Token: 0x0600A428 RID: 42024 RVA: 0x0044308C File Offset: 0x0044128C
		private bool TryAcquireActivityContext()
		{
			if (this._modeLease != null)
			{
				if (this._modeLease.IsActive)
				{
					return true;
				}
				this._modeLease = null;
			}
			return PlayerActivityGate.TryAcquire("BoobTouch#" + base.GetInstanceID().ToString(), this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._modeLease, false, null);
		}

		// Token: 0x0600A429 RID: 42025 RVA: 0x004430E6 File Offset: 0x004412E6
		private void ReleaseActivityContext()
		{
			PlayerActivityLease modeLease = this._modeLease;
			this._modeLease = null;
			if (modeLease != null)
			{
				modeLease.Dispose();
			}
			if (this._interactionFirstPerson != null)
			{
				this._interactionFirstPerson.ResumeAfterInteraction(this);
			}
			this._interactionFirstPerson = null;
		}

		// Token: 0x0600A42A RID: 42026 RVA: 0x00443124 File Offset: 0x00441324
		private void SuspendFirstPersonView()
		{
			if (this._interactionFirstPerson == null && this._playerRoot != null)
			{
				this._interactionFirstPerson = this._playerRoot.GetComponentInChildren<FirstPersonController>(true);
			}
			if (this._interactionFirstPerson == null)
			{
				this._interactionFirstPerson = base.GetComponentInParent<FirstPersonController>(true);
			}
			if (this._interactionFirstPerson != null)
			{
				this._interactionFirstPerson.SuspendForInteraction(this);
			}
		}

		// Token: 0x0600A42B RID: 42027 RVA: 0x00443194 File Offset: 0x00441394
		private void ApplyModeFace()
		{
			if (this._faceOverridden)
			{
				return;
			}
			if (this._emotionController == null)
			{
				this._emotionController = this._playerRoot.GetComponentInChildren<PlayerEmotionController>(true);
			}
			this._faceAnimator = ((this._emotionController != null && this._emotionController.PlayerAnimator != null) ? this._emotionController.PlayerAnimator : this._animator);
			this._faceParamName = ((this._emotionController != null && !string.IsNullOrEmpty(this._emotionController.ExpressionParamName)) ? this._emotionController.ExpressionParamName : "FaceExpressionIndex");
			this._modeFaceIndex = ((this._emotionController != null) ? this._emotionController.NeutralExpressionIndex : 0);
			if (this._faceAnimator != null)
			{
				this._savedFaceIndex = this._faceAnimator.GetInteger(this._faceParamName);
			}
			if (this._emotionController != null)
			{
				this._faceOverrideWasAlreadyHeld = this._emotionController.IsFacialExpressionOverriddenForAction;
				this._emotionController.DisableFacialExpressionForAction();
				this._emotionController.SetExpression(this.EffectiveModeFaceIndex);
			}
			else if (this._faceAnimator != null)
			{
				this._faceOverrideWasAlreadyHeld = false;
				this._faceAnimator.SetInteger(this._faceParamName, this.EffectiveModeFaceIndex);
			}
			this._faceOverridden = this._faceAnimator != null || this._emotionController != null;
		}

		// Token: 0x0600A42C RID: 42028 RVA: 0x00443310 File Offset: 0x00441510
		private void MaintainModeFace()
		{
			if (!this._faceOverridden || this._faceAnimator == null)
			{
				return;
			}
			int effectiveModeFaceIndex = this.EffectiveModeFaceIndex;
			if (this._faceAnimator.GetInteger(this._faceParamName) != effectiveModeFaceIndex)
			{
				this._faceAnimator.SetInteger(this._faceParamName, effectiveModeFaceIndex);
			}
		}

		// Token: 0x0600A42D RID: 42029 RVA: 0x00443364 File Offset: 0x00441564
		private void RestoreModeFace()
		{
			if (!this._faceOverridden)
			{
				return;
			}
			if (this._emotionController != null)
			{
				if (this._faceOverrideWasAlreadyHeld)
				{
					if (this._faceAnimator != null)
					{
						this._faceAnimator.SetInteger(this._faceParamName, this._savedFaceIndex);
					}
				}
				else
				{
					this._emotionController.RestoreFacialExpression();
				}
			}
			else if (this._faceAnimator != null)
			{
				this._faceAnimator.SetInteger(this._faceParamName, this._savedFaceIndex);
			}
			this._faceAnimator = null;
			this._faceOverridden = false;
			this._faceOverrideWasAlreadyHeld = false;
		}

		// Token: 0x0600A42E RID: 42030 RVA: 0x00443400 File Offset: 0x00441600
		private void ApplyClosedEyes()
		{
			this.RestoreClosedEyes();
			if (!this.closeEyesDuringMode || this._playerRoot == null)
			{
				return;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._playerRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null))
				{
					this.AddEyeBlendShapeOverrides(skinnedMeshRenderer, this.eyeCloseBlendShapeNames, this.eyeCloseBlendShapeWeight);
					this.AddEyeBlendShapeOverrides(skinnedMeshRenderer, this.eyeBlendShapeNamesToClear, 0f);
					this.AddEyeBlendShapeOverrides(skinnedMeshRenderer, BoobTouchModeController.EyelidShapesAlwaysCleared, 0f);
				}
			}
			this.MaintainClosedEyes();
			if (this._eyeBlendShapeOverrides.Count == 0)
			{
				Debug.LogWarning("[BoobTouchModeController] Blendshape occhi chiusi non trovati sui renderer del player.", this);
			}
		}

		// Token: 0x0600A42F RID: 42031 RVA: 0x004434B4 File Offset: 0x004416B4
		private void ApplyOpenEyes()
		{
			if (this._playerRoot == null)
			{
				return;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._playerRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null))
				{
					this.AddEyeBlendShapeOverrides(skinnedMeshRenderer, BoobTouchModeController.EyeCloseShapesForOpenGropingEyes, 0f);
				}
			}
			this.MaintainClosedEyes();
		}

		// Token: 0x0600A430 RID: 42032 RVA: 0x00443520 File Offset: 0x00441720
		private void AddEyeBlendShapeOverrides(SkinnedMeshRenderer renderer, string[] blendShapeNames, float targetWeight)
		{
			if (blendShapeNames == null)
			{
				return;
			}
			foreach (string text in blendShapeNames)
			{
				if (!string.IsNullOrEmpty(text))
				{
					int blendShapeIndex = renderer.sharedMesh.GetBlendShapeIndex(text);
					if (blendShapeIndex >= 0 && !this.HasEyeBlendShapeOverride(renderer, blendShapeIndex))
					{
						this._eyeBlendShapeOverrides.Add(new BoobTouchModeController.EyeBlendShapeOverride
						{
							renderer = renderer,
							index = blendShapeIndex,
							savedWeight = renderer.GetBlendShapeWeight(blendShapeIndex),
							targetWeight = targetWeight
						});
					}
				}
			}
		}

		// Token: 0x0600A431 RID: 42033 RVA: 0x0044359C File Offset: 0x0044179C
		private bool HasEyeBlendShapeOverride(SkinnedMeshRenderer renderer, int index)
		{
			foreach (BoobTouchModeController.EyeBlendShapeOverride eyeBlendShapeOverride in this._eyeBlendShapeOverrides)
			{
				if (eyeBlendShapeOverride.renderer == renderer && eyeBlendShapeOverride.index == index)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A432 RID: 42034 RVA: 0x00443608 File Offset: 0x00441808
		private void MaintainClosedEyes()
		{
			foreach (BoobTouchModeController.EyeBlendShapeOverride eyeBlendShapeOverride in this._eyeBlendShapeOverrides)
			{
				if (eyeBlendShapeOverride.renderer != null)
				{
					eyeBlendShapeOverride.renderer.SetBlendShapeWeight(eyeBlendShapeOverride.index, eyeBlendShapeOverride.targetWeight);
				}
			}
		}

		// Token: 0x0600A433 RID: 42035 RVA: 0x0044367C File Offset: 0x0044187C
		private void RestoreClosedEyes()
		{
			foreach (BoobTouchModeController.EyeBlendShapeOverride eyeBlendShapeOverride in this._eyeBlendShapeOverrides)
			{
				if (eyeBlendShapeOverride.renderer != null)
				{
					eyeBlendShapeOverride.renderer.SetBlendShapeWeight(eyeBlendShapeOverride.index, eyeBlendShapeOverride.savedWeight);
				}
			}
			this._eyeBlendShapeOverrides.Clear();
		}

		// Token: 0x0600A434 RID: 42036 RVA: 0x004436F8 File Offset: 0x004418F8
		private void AcquireModeWet()
		{
			if (this._wetHeld)
			{
				return;
			}
			if (this._wetController == null)
			{
				this._wetController = this._playerRoot.GetComponentInChildren<CielWetController>(true);
			}
			if (this._wetController == null)
			{
				Debug.LogWarning("[BoobTouchModeController] CielWetController non trovato: il wet della shower non puo' essere applicato.", this);
				return;
			}
			this._wetController.AcquireWet("BoobTouchMode");
			this._wetHeld = true;
		}

		// Token: 0x0600A435 RID: 42037 RVA: 0x0044375F File Offset: 0x0044195F
		private void ReleaseModeWet()
		{
			if (!this._wetHeld)
			{
				return;
			}
			if (this._wetController != null)
			{
				this._wetController.ReleaseWet("BoobTouchMode");
			}
			this._wetHeld = false;
		}

		// Token: 0x0600A436 RID: 42038 RVA: 0x00443790 File Offset: 0x00441990
		private bool FindBoobCloths()
		{
			this._cloths.Clear();
			this._breastTips.Clear();
			this.ReleaseAllExternalBreastGrabs();
			this._hairCloths.Clear();
			this._chestAnchor = null;
			foreach (MagicaCloth magicaCloth in this._playerRoot.GetComponentsInChildren<MagicaCloth>(true))
			{
				foreach (string text in this.hairClothNames)
				{
					if (magicaCloth.gameObject.name == text && magicaCloth.isActiveAndEnabled)
					{
						this._hairCloths.Add(magicaCloth);
						break;
					}
				}
				bool flag = false;
				foreach (string text2 in this.boobClothNames)
				{
					if (magicaCloth.gameObject.name == text2)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					ClothSerializeData serializeData = magicaCloth.SerializeData;
					if (serializeData.rootBones.Count != 0 && !(serializeData.rootBones[0] == null))
					{
						bool flag2 = magicaCloth.gameObject.name.Contains("Butt");
						this._cloths.Add(new BoobTouchModeController.ClothBackup
						{
							cloth = magicaCloth,
							isButt = flag2
						});
						if (!flag2)
						{
							foreach (Transform transform in serializeData.rootBones)
							{
								this.RegisterBreastTip(transform);
							}
						}
						if (this._chestAnchor == null && serializeData.rootBones.Count > 0 && serializeData.rootBones[0] != null)
						{
							this._chestAnchor = ((serializeData.rootBones[0].parent != null) ? serializeData.rootBones[0].parent : serializeData.rootBones[0]);
						}
					}
				}
			}
			return this._cloths.Count > 0 && this._chestAnchor != null;
		}

		// Token: 0x0600A437 RID: 42039 RVA: 0x004439C4 File Offset: 0x00441BC4
		private void RegisterBreastTip(Transform baseBone)
		{
			if (baseBone == null || baseBone.parent == null || !baseBone.name.Contains("_01"))
			{
				return;
			}
			Transform transform = baseBone.parent.Find(baseBone.name.Replace("_01", "_02"));
			if (transform == null)
			{
				Debug.LogWarning("[BoobTouchModeController] Supporto _02 di " + baseBone.name + " mancante: correzione visiva punta disabilitata per questo lato.", this);
				return;
			}
			using (List<BoobTouchModeController.BreastTipVisualState>.Enumerator enumerator = this._breastTips.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.renderBone01 == baseBone)
					{
						return;
					}
				}
			}
			this._breastTips.Add(new BoobTouchModeController.BreastTipVisualState
			{
				renderBone01 = baseBone,
				supportBone02 = transform
			});
		}

		// Token: 0x0600A438 RID: 42040 RVA: 0x00443AAC File Offset: 0x00441CAC
		private IEnumerator EnterClothRoutine()
		{
			this._transitioning = true;
			foreach (BoobTouchModeController.ClothBackup clothBackup in this._cloths)
			{
				clothBackup.go = clothBackup.cloth.gameObject;
				clothBackup.originalData = clothBackup.cloth.SerializeData;
				clothBackup.originalData2 = clothBackup.cloth.GetSerializeData2();
				Object.Destroy(clothBackup.cloth);
			}
			yield return null;
			yield return null;
			foreach (BoobTouchModeController.ClothBackup clothBackup2 in this._cloths)
			{
				MagicaCloth magicaCloth = clothBackup2.go.AddComponent<MagicaCloth>();
				magicaCloth.DisableAutoBuild();
				ClothSerializeData serializeData = magicaCloth.SerializeData;
				serializeData.Import(clothBackup2.originalData, true);
				serializeData.springConstraint = clothBackup2.originalData.springConstraint.Clone();
				serializeData.meshWriteMode = clothBackup2.originalData.meshWriteMode;
				ClothSerializeData2 serializeData2 = magicaCloth.GetSerializeData2();
				ColliderCollisionConstraint.SerializeData colliderCollisionConstraint = serializeData.colliderCollisionConstraint;
				foreach (Transform transform in serializeData.rootBones.ToArray())
				{
					if (!(transform == null) && !(transform.parent == null) && transform.name.Contains("_01"))
					{
						Transform transform2 = transform.parent.Find(transform.name.Replace("_01", "_02"));
						if (transform2 != null && !serializeData.rootBones.Contains(transform2))
						{
							serializeData.rootBones.Add(transform2);
						}
					}
				}
				colliderCollisionConstraint.collisionBones.Clear();
				Dictionary<Transform, VertexAttribute> boneAttributeDict = serializeData2.boneAttributeDict;
				foreach (Transform transform3 in serializeData.rootBones)
				{
					if (!(transform3 == null))
					{
						colliderCollisionConstraint.collisionBones.Add(transform3);
						boneAttributeDict[transform3] = VertexAttribute.Fixed;
						for (int j = 0; j < transform3.childCount; j++)
						{
							colliderCollisionConstraint.collisionBones.Add(transform3.GetChild(j));
							boneAttributeDict[transform3.GetChild(j)] = VertexAttribute.Fixed;
						}
					}
				}
				float num = (this._externalInteractorMode ? this._breastSizeProfile.clothRangeScale : 1f);
				float num2 = (clothBackup2.isButt ? (this.pushLimitDistance * this.buttPushMultiplier) : (this.pushLimitDistance * num));
				colliderCollisionConstraint.limitDistance.value = num2;
				float num3 = (clothBackup2.isButt ? this.springLimitDistance : (this.springLimitDistance * num));
				serializeData.springConstraint.limitDistance = Mathf.Max(num3, num2);
				if (clothBackup2.isButt)
				{
					serializeData.radius.value = Mathf.Max(this.buttParticleRadius, serializeData.radius.value);
				}
				else
				{
					serializeData.radius.value = Mathf.Max(this.breastParticleRadius * (this._externalInteractorMode ? this._breastSizeProfile.particleRadiusScale : 1f), serializeData.radius.value);
				}
				clothBackup2.cloth = magicaCloth;
				bool flag = magicaCloth.BuildAndRun();
				if (this.debugLogs)
				{
					Debug.Log("[BoobTouchModeController] " + clothBackup2.go.name + ": build modalita' " + string.Format("(roots={0}) -> {1}", serializeData.rootBones.Count, flag));
				}
			}
			yield return new WaitForSeconds(0.7f);
			this.RegisterBallCollider();
			if (this._canvasService != null)
			{
				if (this.BakeSoapSurfaceProxy())
				{
					this._canvasService.BuildPaintColliders(new BodyPaintCanvasService.PaintChannel?(BodyPaintCanvasService.PaintChannel.Head));
				}
				else
				{
					this._canvasService.BuildPaintColliders(null);
				}
				Physics.SyncTransforms();
				this._lastBakeTime = Time.unscaledTime;
				this._surfaceReady = true;
			}
			this._transitioning = false;
			this._clothRoutine = null;
			yield break;
		}

		// Token: 0x0600A439 RID: 42041 RVA: 0x00443ABB File Offset: 0x00441CBB
		private void RestoreClothSetup()
		{
			if (base.isActiveAndEnabled && base.gameObject.activeInHierarchy)
			{
				this._clothRoutine = base.StartCoroutine(this.ExitClothRoutine());
				return;
			}
			this._cloths.Clear();
		}

		// Token: 0x0600A43A RID: 42042 RVA: 0x00443AF0 File Offset: 0x00441CF0
		private IEnumerator ExitClothRoutine()
		{
			this._transitioning = true;
			foreach (BoobTouchModeController.ClothBackup clothBackup in this._cloths)
			{
				if (clothBackup.cloth != null)
				{
					Object.Destroy(clothBackup.cloth);
				}
			}
			yield return null;
			yield return null;
			foreach (BoobTouchModeController.ClothBackup clothBackup2 in this._cloths)
			{
				if (!(clothBackup2.go == null) && clothBackup2.originalData != null)
				{
					MagicaCloth magicaCloth = clothBackup2.go.AddComponent<MagicaCloth>();
					magicaCloth.DisableAutoBuild();
					Type typeFromHandle = typeof(MagicaCloth);
					typeFromHandle.GetField("serializeData", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(magicaCloth, clothBackup2.originalData);
					typeFromHandle.GetField("serializeData2", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(magicaCloth, clothBackup2.originalData2);
					bool flag = magicaCloth.BuildAndRun();
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[BoobTouchModeController] {0}: ripristino gameplay -> {1}", clothBackup2.go.name, flag));
					}
				}
			}
			this._cloths.Clear();
			this._transitioning = false;
			this._clothRoutine = null;
			yield break;
		}

		// Token: 0x0600A43B RID: 42043 RVA: 0x00443B00 File Offset: 0x00441D00
		private void RegisterBallCollider()
		{
			foreach (BoobTouchModeController.ClothBackup clothBackup in this._cloths)
			{
				if (!(clothBackup.cloth == null) && clothBackup.cloth.isActiveAndEnabled)
				{
					List<ColliderComponent> colliderList = clothBackup.cloth.SerializeData.colliderCollisionConstraint.colliderList;
					if (this._ballCollider != null && !colliderList.Contains(this._ballCollider))
					{
						colliderList.Add(this._ballCollider);
					}
					foreach (ColliderComponent colliderComponent in this._externalBodyColliders)
					{
						if (colliderComponent != null && !colliderList.Contains(colliderComponent))
						{
							colliderList.Add(colliderComponent);
						}
					}
					clothBackup.cloth.SetParameterChange();
				}
			}
			foreach (MagicaCloth magicaCloth in this._hairCloths)
			{
				if (!(magicaCloth == null) && magicaCloth.isActiveAndEnabled)
				{
					List<ColliderComponent> colliderList2 = magicaCloth.SerializeData.colliderCollisionConstraint.colliderList;
					if (this._hairBallCollider != null && !colliderList2.Contains(this._hairBallCollider))
					{
						colliderList2.Add(this._hairBallCollider);
					}
					foreach (ColliderComponent colliderComponent2 in this._externalHairColliders)
					{
						if (colliderComponent2 != null && !colliderList2.Contains(colliderComponent2))
						{
							colliderList2.Add(colliderComponent2);
						}
					}
					magicaCloth.SetParameterChange();
				}
			}
			if (this.debugLogs)
			{
				Debug.Log("[BoobTouchModeController] Collider interazione registrati sui cloth");
			}
		}

		// Token: 0x0600A43C RID: 42044 RVA: 0x00443D20 File Offset: 0x00441F20
		private void UnregisterBallCollider()
		{
			foreach (BoobTouchModeController.ClothBackup clothBackup in this._cloths)
			{
				if (!(clothBackup.cloth == null))
				{
					if (this._ballCollider != null)
					{
						clothBackup.cloth.SerializeData.colliderCollisionConstraint.colliderList.Remove(this._ballCollider);
					}
					foreach (ColliderComponent colliderComponent in this._externalBodyColliders)
					{
						if (colliderComponent != null)
						{
							clothBackup.cloth.SerializeData.colliderCollisionConstraint.colliderList.Remove(colliderComponent);
						}
					}
					clothBackup.cloth.SetParameterChange();
				}
			}
			foreach (MagicaCloth magicaCloth in this._hairCloths)
			{
				if (!(magicaCloth == null))
				{
					if (this._hairBallCollider != null)
					{
						magicaCloth.SerializeData.colliderCollisionConstraint.colliderList.Remove(this._hairBallCollider);
					}
					foreach (ColliderComponent colliderComponent2 in this._externalHairColliders)
					{
						if (colliderComponent2 != null)
						{
							magicaCloth.SerializeData.colliderCollisionConstraint.colliderList.Remove(colliderComponent2);
						}
					}
					magicaCloth.SetParameterChange();
				}
			}
			this._hairCloths.Clear();
			this._externalBodyColliders.Clear();
			this._externalHairColliders.Clear();
		}

		// Token: 0x0600A43D RID: 42045 RVA: 0x00443F20 File Offset: 0x00442120
		public void RegisterExternalCollider(ColliderComponent collider, bool affectsHair = false)
		{
			if (collider == null)
			{
				return;
			}
			List<ColliderComponent> list = (affectsHair ? this._externalHairColliders : this._externalBodyColliders);
			if (!list.Contains(collider))
			{
				list.Add(collider);
			}
			if (this._transitioning)
			{
				return;
			}
			if (affectsHair)
			{
				foreach (MagicaCloth magicaCloth in this._hairCloths)
				{
					if (!(magicaCloth == null) && magicaCloth.isActiveAndEnabled)
					{
						List<ColliderComponent> colliderList = magicaCloth.SerializeData.colliderCollisionConstraint.colliderList;
						if (!colliderList.Contains(collider))
						{
							colliderList.Add(collider);
						}
						magicaCloth.SetParameterChange();
					}
				}
				return;
			}
			foreach (BoobTouchModeController.ClothBackup clothBackup in this._cloths)
			{
				if (!(clothBackup.cloth == null) && clothBackup.cloth.isActiveAndEnabled)
				{
					List<ColliderComponent> colliderList2 = clothBackup.cloth.SerializeData.colliderCollisionConstraint.colliderList;
					if (!colliderList2.Contains(collider))
					{
						colliderList2.Add(collider);
					}
					clothBackup.cloth.SetParameterChange();
				}
			}
		}

		// Token: 0x0600A43E RID: 42046 RVA: 0x00444070 File Offset: 0x00442270
		public void UnregisterExternalCollider(ColliderComponent collider)
		{
			if (collider == null)
			{
				return;
			}
			this._externalBodyColliders.Remove(collider);
			this._externalHairColliders.Remove(collider);
			foreach (BoobTouchModeController.ClothBackup clothBackup in this._cloths)
			{
				if (!(clothBackup.cloth == null))
				{
					clothBackup.cloth.SerializeData.colliderCollisionConstraint.colliderList.Remove(collider);
					clothBackup.cloth.SetParameterChange();
				}
			}
			foreach (MagicaCloth magicaCloth in this._hairCloths)
			{
				if (!(magicaCloth == null))
				{
					magicaCloth.SerializeData.colliderCollisionConstraint.colliderList.Remove(collider);
					magicaCloth.SetParameterChange();
				}
			}
		}

		// Token: 0x0600A43F RID: 42047 RVA: 0x00444178 File Offset: 0x00442378
		private void SubscribeSurfaceBake()
		{
			if (this._surfaceBakeSubscribed)
			{
				return;
			}
			MagicaManager.OnPreSimulation = (Action)Delegate.Combine(MagicaManager.OnPreSimulation, new Action(this.HandlePreSimulationBreastTips));
			MagicaManager.OnPostSimulation = (Action)Delegate.Combine(MagicaManager.OnPostSimulation, new Action(this.HandlePostSimulationSurfaceBake));
			this._surfaceBakeSubscribed = true;
		}

		// Token: 0x0600A440 RID: 42048 RVA: 0x004441D8 File Offset: 0x004423D8
		private void UnsubscribeSurfaceBake()
		{
			if (!this._surfaceBakeSubscribed)
			{
				return;
			}
			MagicaManager.OnPreSimulation = (Action)Delegate.Remove(MagicaManager.OnPreSimulation, new Action(this.HandlePreSimulationBreastTips));
			MagicaManager.OnPostSimulation = (Action)Delegate.Remove(MagicaManager.OnPostSimulation, new Action(this.HandlePostSimulationSurfaceBake));
			this._surfaceBakeSubscribed = false;
		}

		// Token: 0x0600A441 RID: 42049 RVA: 0x00444238 File Offset: 0x00442438
		private void HandlePreSimulationBreastTips()
		{
			if (!this._active)
			{
				return;
			}
			foreach (BoobTouchModeController.BreastTipVisualState breastTipVisualState in this._breastTips)
			{
				if (!(breastTipVisualState.renderBone01 == null) && !(breastTipVisualState.supportBone02 == null))
				{
					breastTipVisualState.preRenderPosition = breastTipVisualState.renderBone01.position;
					breastTipVisualState.preSupportPosition = breastTipVisualState.supportBone02.position;
					breastTipVisualState.preFrame = Time.frameCount;
				}
			}
		}

		// Token: 0x0600A442 RID: 42050 RVA: 0x004442D8 File Offset: 0x004424D8
		private static Vector3 BreastOutward(BoobTouchModeController.BreastTipVisualState state)
		{
			if (state == null || state.renderBone01 == null)
			{
				return Vector3.forward;
			}
			Vector3 vector = ((state.renderBone01.parent != null) ? state.renderBone01.parent.forward : state.renderBone01.forward);
			if (vector.sqrMagnitude <= 1E-06f)
			{
				return Vector3.forward;
			}
			return vector.normalized;
		}

		// Token: 0x0600A443 RID: 42051 RVA: 0x00444348 File Offset: 0x00442548
		private void ResolveExternalBreastAnchor(BoobTouchModeController.BreastTipVisualState state, out Vector3 anchorWorld, out Vector3 outwardWorld)
		{
			outwardWorld = BoobTouchModeController.BreastOutward(state);
			if (this._breastSizeProfile.boobsType == 3)
			{
				anchorWorld = ((state != null && state.renderBone01 != null) ? state.renderBone01.position : Vector3.zero);
				return;
			}
			Vector3 vector;
			if (this.TryEvaluateNippleSurfaceAnchor(state, out anchorWorld, out vector))
			{
				if (Vector3.Dot(vector, outwardWorld) < 0f)
				{
					vector = -vector;
				}
				if (vector.sqrMagnitude > 1E-06f)
				{
					outwardWorld = vector.normalized;
				}
				return;
			}
			anchorWorld = ((state != null && state.renderBone01 != null) ? state.renderBone01.position : Vector3.zero);
		}

		// Token: 0x0600A444 RID: 42052 RVA: 0x00444404 File Offset: 0x00442604
		private bool TryEvaluateNippleSurfaceAnchor(BoobTouchModeController.BreastTipVisualState state, out Vector3 anchorWorld, out Vector3 normalWorld)
		{
			anchorWorld = Vector3.zero;
			normalWorld = Vector3.forward;
			if (state == null || !state.hasSurfaceAnchor || this._soapSurfaceRenderer == null || this._soapSurfaceTriangles == null || this._soapSurfaceVertices.Count == 0)
			{
				return false;
			}
			int num = state.surfaceTriangleIndex * 3;
			if (num < 0 || num + 2 >= this._soapSurfaceTriangles.Length)
			{
				return false;
			}
			int num2 = this._soapSurfaceTriangles[num];
			int num3 = this._soapSurfaceTriangles[num + 1];
			int num4 = this._soapSurfaceTriangles[num + 2];
			if (num2 < 0 || num3 < 0 || num4 < 0 || num2 >= this._soapSurfaceVertices.Count || num3 >= this._soapSurfaceVertices.Count || num4 >= this._soapSurfaceVertices.Count)
			{
				return false;
			}
			Vector3 surfaceBarycentric = state.surfaceBarycentric;
			Vector3 vector = this._soapSurfaceVertices[num2] * surfaceBarycentric.x + this._soapSurfaceVertices[num3] * surfaceBarycentric.y + this._soapSurfaceVertices[num4] * surfaceBarycentric.z;
			anchorWorld = this._soapSurfaceRenderer.transform.TransformPoint(vector);
			if (num2 < this._soapSurfaceNormals.Count && num3 < this._soapSurfaceNormals.Count && num4 < this._soapSurfaceNormals.Count)
			{
				Vector3 vector2 = this._soapSurfaceNormals[num2] * surfaceBarycentric.x + this._soapSurfaceNormals[num3] * surfaceBarycentric.y + this._soapSurfaceNormals[num4] * surfaceBarycentric.z;
				if (vector2.sqrMagnitude > 1E-06f)
				{
					normalWorld = this._soapSurfaceRenderer.transform.TransformDirection(vector2.normalized).normalized;
				}
			}
			return true;
		}

		// Token: 0x0600A445 RID: 42053 RVA: 0x004445FC File Offset: 0x004427FC
		private void EnsureNippleSurfaceAnchors()
		{
			if (this._soapSurfaceCollider == null || !this._soapSurfaceCollider.enabled || this._soapSurfaceTriangles == null || this._soapSurfaceVertices.Count == 0)
			{
				return;
			}
			foreach (BoobTouchModeController.BreastTipVisualState breastTipVisualState in this._breastTips)
			{
				if (breastTipVisualState != null && !(breastTipVisualState.renderBone01 == null) && !breastTipVisualState.hasSurfaceAnchor)
				{
					this.BindNippleSurfaceAnchor(breastTipVisualState);
				}
			}
		}

		// Token: 0x0600A446 RID: 42054 RVA: 0x0044469C File Offset: 0x0044289C
		private bool BindNippleSurfaceAnchor(BoobTouchModeController.BreastTipVisualState state)
		{
			Vector3 vector = BoobTouchModeController.BreastOutward(state);
			Transform transform = ((this._chestAnchor != null) ? this._chestAnchor : state.renderBone01.parent);
			Vector3 vector2 = ((transform != null) ? Vector3.ProjectOnPlane(transform.right, vector) : Vector3.ProjectOnPlane(Vector3.right, vector));
			Vector3 vector3 = ((transform != null) ? Vector3.ProjectOnPlane(transform.up, vector) : Vector3.ProjectOnPlane(Vector3.up, vector));
			if (vector2.sqrMagnitude < 1E-06f)
			{
				vector2 = Vector3.Cross(Vector3.up, vector);
			}
			if (vector3.sqrMagnitude < 1E-06f)
			{
				vector3 = Vector3.Cross(vector, vector2);
			}
			vector2.Normalize();
			vector3.Normalize();
			float num = Mathf.Clamp(0.035f * this._breastSizeProfile.interactionScale, 0.018f, 0.06f);
			float num2 = Mathf.Clamp(0.34f * this._breastSizeProfile.interactionScale, 0.22f, 0.55f);
			Vector3 position = state.renderBone01.position;
			bool flag = false;
			float num3 = float.NegativeInfinity;
			RaycastHit raycastHit = default(RaycastHit);
			for (int i = -2; i <= 2; i++)
			{
				for (int j = -2; j <= 2; j++)
				{
					Vector3 vector4 = vector2 * ((float)i * num / 2f) + vector3 * ((float)j * num / 2f);
					Vector3 vector5 = position + vector4 + vector * num2;
					Ray ray = new Ray(vector5 + this._surfaceOffset, -vector);
					RaycastHit raycastHit2;
					if (this._soapSurfaceCollider.Raycast(ray, out raycastHit2, num2 * 2f))
					{
						Vector3 vector6 = raycastHit2.point - this._surfaceOffset - position;
						float num4 = Vector3.Dot(vector6, vector);
						float magnitude = Vector3.ProjectOnPlane(vector6, vector).magnitude;
						if (magnitude <= num * 1.75f)
						{
							float num5 = num4 - magnitude * 0.55f;
							if (num5 > num3)
							{
								num3 = num5;
								raycastHit = raycastHit2;
								flag = true;
							}
						}
					}
				}
			}
			if (!flag || raycastHit.triangleIndex < 0)
			{
				return false;
			}
			state.surfaceTriangleIndex = raycastHit.triangleIndex;
			state.surfaceBarycentric = raycastHit.barycentricCoordinate;
			state.hasSurfaceAnchor = true;
			return true;
		}

		// Token: 0x0600A447 RID: 42055 RVA: 0x004448EC File Offset: 0x00442AEC
		private void ApplyExternalBreastGrabsPostSimulation()
		{
			if (!this._externalInteractorMode || this._transitioning || this._externalBreastGrabs.Count == 0 || this._breastTips.Count == 0)
			{
				return;
			}
			int count = this._breastTips.Count;
			Vector3[] array = new Vector3[count];
			Vector3[] array2 = new Vector3[count];
			float[] array3 = new float[count];
			float num = Mathf.Max(0.0001f, Time.unscaledDeltaTime);
			float num2 = BoobTouchModeController.ExpBlend(this.externalGrabSharpness, num);
			this._staleExternalGrabOwners.Clear();
			foreach (KeyValuePair<int, BoobTouchModeController.ExternalBreastGrabState> keyValuePair in this._externalBreastGrabs)
			{
				BoobTouchModeController.ExternalBreastGrabState value = keyValuePair.Value;
				if (Time.frameCount - value.updatedFrame > 2 || value.breastIndex < 0 || value.breastIndex >= count)
				{
					this._staleExternalGrabOwners.Add(keyValuePair.Key);
				}
				else
				{
					BoobTouchModeController.BreastTipVisualState breastTipVisualState = this._breastTips[value.breastIndex];
					if (breastTipVisualState.renderBone01 == null || breastTipVisualState.supportBone02 == null)
					{
						this._staleExternalGrabOwners.Add(keyValuePair.Key);
					}
					else
					{
						Vector3 position = breastTipVisualState.renderBone01.position;
						if (!value.hasAnchorOffset)
						{
							value.anchorOffset = position - value.handPointWorld;
							value.filteredDelta = Vector3.zero;
							value.hasAnchorOffset = true;
						}
						Vector3 vector = value.handPointWorld + value.anchorOffset;
						Vector3 freshBreastReference = this.GetFreshBreastReference(breastTipVisualState, false);
						vector = this.ConstrainBreastGrabPosition(vector, freshBreastReference, value.kind);
						Vector3 vector2 = vector - position;
						value.filteredDelta = Vector3.Lerp(value.filteredDelta, vector2, num2);
						float num3 = Mathf.Max(0.01f, value.strength);
						float num4 = ((value.kind == BoobTouchModeController.ExternalGrabKind.Nipple) ? Mathf.Clamp01(this.externalNippleSupportFollow) : Mathf.Clamp01(this.externalBreastSupportFollow));
						num4 *= this._breastSizeProfile.supportMotionScale;
						array[value.breastIndex] += value.filteredDelta * num3;
						array2[value.breastIndex] += value.filteredDelta * (num3 * num4);
						array3[value.breastIndex] += num3;
					}
				}
			}
			foreach (int num5 in this._staleExternalGrabOwners)
			{
				this._externalBreastGrabs.Remove(num5);
			}
			for (int i = 0; i < count; i++)
			{
				if (array3[i] > 0f)
				{
					BoobTouchModeController.BreastTipVisualState breastTipVisualState2 = this._breastTips[i];
					if (!(breastTipVisualState2.renderBone01 == null) && !(breastTipVisualState2.supportBone02 == null))
					{
						Vector3 vector3 = array[i] / array3[i];
						Vector3 vector4 = array2[i] / array3[i];
						Vector3 freshBreastReference2 = this.GetFreshBreastReference(breastTipVisualState2, false);
						Vector3 freshBreastReference3 = this.GetFreshBreastReference(breastTipVisualState2, true);
						breastTipVisualState2.renderBone01.position = this.ConstrainBreastGrabPosition(breastTipVisualState2.renderBone01.position + vector3, freshBreastReference2, BoobTouchModeController.ExternalGrabKind.Breast);
						breastTipVisualState2.supportBone02.position = freshBreastReference3 + Vector3.ClampMagnitude(breastTipVisualState2.supportBone02.position + vector4 - freshBreastReference3, Mathf.Max(0.005f, this.externalSupportMaxOffset * this._breastSizeProfile.supportMotionScale));
					}
				}
			}
		}

		// Token: 0x0600A448 RID: 42056 RVA: 0x00444CF8 File Offset: 0x00442EF8
		private Vector3 GetFreshBreastReference(BoobTouchModeController.BreastTipVisualState breast, bool supportBone)
		{
			if (breast.preFrame >= 0 && Time.frameCount - breast.preFrame <= 1)
			{
				if (!supportBone)
				{
					return breast.preRenderPosition;
				}
				return breast.preSupportPosition;
			}
			else
			{
				Transform transform = (supportBone ? breast.supportBone02 : breast.renderBone01);
				if (!(transform != null))
				{
					return Vector3.zero;
				}
				return transform.position;
			}
		}

		// Token: 0x0600A449 RID: 42057 RVA: 0x00444D5D File Offset: 0x00442F5D
		private Vector3 ConstrainBreastGrabPosition(Vector3 desiredWorld, Vector3 anatomicalReference, BoobTouchModeController.ExternalGrabKind kind)
		{
			return anatomicalReference + this.ConstrainExternalGrabDelta(desiredWorld - anatomicalReference, kind);
		}

		// Token: 0x0600A44A RID: 42058 RVA: 0x00444D74 File Offset: 0x00442F74
		private Vector3 ConstrainExternalGrabDelta(Vector3 desiredWorldDelta, BoobTouchModeController.ExternalGrabKind kind)
		{
			Transform chestAnchor = this._chestAnchor;
			Quaternion quaternion = ((chestAnchor != null) ? chestAnchor.rotation : Quaternion.identity);
			Vector3 vector = Quaternion.Inverse(quaternion) * desiredWorldDelta;
			bool flag = kind == BoobTouchModeController.ExternalGrabKind.Nipple;
			float num = (flag ? this._breastSizeProfile.nippleMotionScale : this._breastSizeProfile.fullGrabMotionScale);
			float num2 = Mathf.Max(0.005f, (flag ? this.externalNippleGrabLateralLimit : this.externalBreastGrabLateralLimit) * num);
			float num3 = Mathf.Max(0.005f, (flag ? this.externalNippleGrabVerticalLimit : this.externalBreastGrabVerticalLimit) * num);
			float num4 = Mathf.Max(0.005f, ((vector.z >= 0f) ? (flag ? this.externalNippleGrabOutwardLimit : this.externalBreastGrabOutwardLimit) : (flag ? this.externalNippleGrabInwardLimit : this.externalBreastGrabInwardLimit)) * num);
			float num5 = vector.x * vector.x / (num2 * num2) + vector.y * vector.y / (num3 * num3) + vector.z * vector.z / (num4 * num4);
			if (num5 > 1f)
			{
				vector /= Mathf.Sqrt(num5);
			}
			Vector3 vector2 = quaternion * vector;
			float num6 = Mathf.Max(0.01f, (flag ? this.externalNippleGrabMaxOffset : this.externalBreastGrabMaxOffset) * num);
			return Vector3.ClampMagnitude(vector2, num6);
		}

		// Token: 0x0600A44B RID: 42059 RVA: 0x00444ECC File Offset: 0x004430CC
		private void NotifyExternalBreastGrabsSolved()
		{
			if (this.ExternalBreastGrabSolved == null || !this._externalInteractorMode || this._externalBreastGrabs.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<int, BoobTouchModeController.ExternalBreastGrabState> keyValuePair in this._externalBreastGrabs)
			{
				BoobTouchModeController.ExternalBreastGrabState value = keyValuePair.Value;
				if (Time.frameCount - value.updatedFrame <= 2 && value.breastIndex >= 0 && value.breastIndex < this._breastTips.Count)
				{
					BoobTouchModeController.BreastTipVisualState breastTipVisualState = this._breastTips[value.breastIndex];
					if (!(breastTipVisualState.renderBone01 == null))
					{
						Vector3 vector;
						Vector3 vector2;
						this.ResolveExternalBreastAnchor(breastTipVisualState, out vector, out vector2);
						this.ExternalBreastGrabSolved(keyValuePair.Key, value.breastIndex, value.kind, vector, vector2);
					}
				}
			}
		}

		// Token: 0x0600A44C RID: 42060 RVA: 0x00444FBC File Offset: 0x004431BC
		public string DebugDescribeExternalBreastConstraints()
		{
			if (this._breastTips.Count == 0)
			{
				return "breasts=0";
			}
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < this._breastTips.Count; i++)
			{
				BoobTouchModeController.BreastTipVisualState breastTipVisualState = this._breastTips[i];
				if (!(breastTipVisualState.renderBone01 == null) && !(breastTipVisualState.supportBone02 == null))
				{
					num = Mathf.Max(num, Vector3.Distance(breastTipVisualState.renderBone01.position, this.GetFreshBreastReference(breastTipVisualState, false)));
					num2 = Mathf.Max(num2, Vector3.Distance(breastTipVisualState.supportBone02.position, this.GetFreshBreastReference(breastTipVisualState, true)));
				}
			}
			return string.Concat(new string[]
			{
				string.Format("size={0}({1}), ", this._breastSizeProfile.label, this._breastSizeProfile.boobsType),
				string.Format("renderOffsetMax={0:F4}/", num),
				string.Format("{0:F4}, ", this.externalBreastGrabMaxOffset * this._breastSizeProfile.fullGrabMotionScale),
				string.Format("supportOffsetMax={0:F4}/", num2),
				string.Format("{0:F4}, ", this.externalSupportMaxOffset * this._breastSizeProfile.supportMotionScale),
				string.Format("supportFollow={0:F2}, ", this.externalBreastSupportFollow),
				string.Format("nippleSupportFollow={0:F2}, ", this.externalNippleSupportFollow),
				string.Format("surfaceAnchors={0}/{1}", this.CountBoundNippleSurfaceAnchors(), this._breastTips.Count)
			});
		}

		// Token: 0x0600A44D RID: 42061 RVA: 0x00445160 File Offset: 0x00443360
		public string DebugDescribeAdaptiveBreastSizing()
		{
			return string.Concat(new string[]
			{
				string.Format("size={0}({1}), ", this._breastSizeProfile.label, this._breastSizeProfile.boobsType),
				string.Format("interaction={0:F2}, ", this._breastSizeProfile.interactionScale),
				string.Format("clearance={0:F2}, ", this._breastSizeProfile.clearanceScale),
				string.Format("acquire={0:F2}, ", this._breastSizeProfile.acquireRadiusScale),
				string.Format("magnet={0:F2}, ", this._breastSizeProfile.magnetRadiusScale),
				string.Format("fullMotion={0:F2}, ", this._breastSizeProfile.fullGrabMotionScale),
				string.Format("pinchMotion={0:F2}, ", this._breastSizeProfile.nippleMotionScale),
				string.Format("cloth={0:F2}, ", this._breastSizeProfile.clothRangeScale),
				string.Format("camera={0:F2}, ", this._breastSizeProfile.cameraDistanceScale),
				string.Format("flatPress={0}, ", this._breastSizeProfile.flatPressMode),
				string.Format("surfaceAnchors={0}/{1}", this.CountBoundNippleSurfaceAnchors(), this._breastTips.Count)
			});
		}

		// Token: 0x0600A44E RID: 42062 RVA: 0x004452D0 File Offset: 0x004434D0
		private int CountBoundNippleSurfaceAnchors()
		{
			int num = 0;
			foreach (BoobTouchModeController.BreastTipVisualState breastTipVisualState in this._breastTips)
			{
				if (breastTipVisualState != null && breastTipVisualState.hasSurfaceAnchor)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600A44F RID: 42063 RVA: 0x00445330 File Offset: 0x00443530
		private void ApplyBreastTipTiltPostSimulation()
		{
			if (!this.breastTipTiltEnabled || this._transitioning)
			{
				foreach (BoobTouchModeController.BreastTipVisualState breastTipVisualState in this._breastTips)
				{
					breastTipVisualState.filteredTiltVector = Vector3.zero;
					breastTipVisualState.lastAppliedAngle = 0f;
					breastTipVisualState.lastProximity = 0f;
					breastTipVisualState.lastMarkerDisplacement = 0f;
				}
				return;
			}
			float num = Mathf.Max(0.0001f, Time.unscaledDeltaTime);
			float num2 = Mathf.Max(0f, this.breastTipTiltMaxDegrees);
			float num3 = Mathf.Max(0f, this.breastTipTiltDeadZone);
			foreach (BoobTouchModeController.BreastTipVisualState breastTipVisualState2 in this._breastTips)
			{
				if (!(breastTipVisualState2.renderBone01 == null) && !(breastTipVisualState2.supportBone02 == null) && breastTipVisualState2.preFrame == Time.frameCount)
				{
					Quaternion rotation = breastTipVisualState2.renderBone01.rotation;
					Vector3 position = breastTipVisualState2.renderBone01.position;
					breastTipVisualState2.lastUntiltedRotation = rotation;
					breastTipVisualState2.lastUntiltedPosition = position;
					breastTipVisualState2.hasUntiltedRotation = true;
					Vector3 vector = ((breastTipVisualState2.renderBone01.parent != null) ? breastTipVisualState2.renderBone01.parent.forward : (rotation * Vector3.forward));
					if (vector.sqrMagnitude >= 1E-06f)
					{
						vector.Normalize();
						Vector3 vector2 = breastTipVisualState2.renderBone01.position - breastTipVisualState2.preRenderPosition;
						Vector3 vector3 = breastTipVisualState2.supportBone02.position - breastTipVisualState2.preSupportPosition;
						Vector3 vector4 = Vector3.ProjectOnPlane(vector2 - vector3 * Mathf.Clamp01(this.breastTipBaseMotionCancellation), vector);
						float magnitude = vector4.magnitude;
						float num4 = this.BreastTipSoapProximity(breastTipVisualState2.renderBone01);
						float num5 = Mathf.Clamp((magnitude - num3) * Mathf.Max(0f, this.breastTipTiltDegreesPerMeter), 0f, num2) * num4;
						Vector3 vector5 = ((magnitude > 1E-06f) ? (vector4 / magnitude * num5) : Vector3.zero);
						Vector3 vector6 = Vector3.ProjectOnPlane(breastTipVisualState2.filteredTiltVector, vector);
						float num6 = ((vector5.sqrMagnitude > vector6.sqrMagnitude) ? this.breastTipTiltAttackSharpness : this.breastTipTiltReleaseSharpness);
						breastTipVisualState2.filteredTiltVector = Vector3.Lerp(vector6, vector5, BoobTouchModeController.ExpBlend(num6, num));
						breastTipVisualState2.filteredTiltVector = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(breastTipVisualState2.filteredTiltVector, vector), num2);
						float magnitude2 = breastTipVisualState2.filteredTiltVector.magnitude;
						if (magnitude2 > 0.0001f)
						{
							Vector3 vector7 = breastTipVisualState2.filteredTiltVector / magnitude2;
							float num7 = magnitude2 * 0.017453292f;
							Vector3 vector8 = vector * Mathf.Cos(num7) + vector7 * Mathf.Sin(num7);
							Quaternion quaternion = Quaternion.FromToRotation(vector, vector8.normalized);
							float num8 = Mathf.Max(0f, this.breastTipVirtualPivotBack);
							Vector3 vector9 = position - vector * num8;
							breastTipVisualState2.renderBone01.position = vector9 + quaternion * (position - vector9);
							breastTipVisualState2.renderBone01.rotation = quaternion * rotation;
							Vector3 vector10 = position + vector * 0.04f;
							Vector3 vector11 = breastTipVisualState2.renderBone01.position + vector8.normalized * 0.04f;
							breastTipVisualState2.lastMarkerDisplacement = Vector3.Distance(vector10, vector11);
						}
						else
						{
							breastTipVisualState2.lastMarkerDisplacement = 0f;
						}
						breastTipVisualState2.lastAppliedAngle = magnitude2;
						breastTipVisualState2.lastProximity = num4;
					}
				}
			}
		}

		// Token: 0x0600A450 RID: 42064 RVA: 0x0044573C File Offset: 0x0044393C
		private float BreastTipSoapProximity(Transform tip)
		{
			if (this._pressureColliderTransform == null || tip == null)
			{
				return 0f;
			}
			Vector3 lossyScale = tip.lossyScale;
			float num = Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Max(Mathf.Abs(lossyScale.y), Mathf.Abs(lossyScale.z)));
			float num2 = Mathf.Max(0f, this._colliderRadius) + Mathf.Max(0f, this.breastParticleRadius) * num;
			float num3 = Vector3.Distance(this._pressureColliderTransform.position, tip.position);
			return 1f - Mathf.InverseLerp(num2, num2 + 0.03f, num3);
		}

		// Token: 0x0600A451 RID: 42065 RVA: 0x004457E8 File Offset: 0x004439E8
		private void RestoreBreastTipRotations()
		{
			foreach (BoobTouchModeController.BreastTipVisualState breastTipVisualState in this._breastTips)
			{
				if (breastTipVisualState.renderBone01 != null && breastTipVisualState.hasUntiltedRotation && breastTipVisualState.lastAppliedAngle > 0.0001f)
				{
					breastTipVisualState.renderBone01.position = breastTipVisualState.lastUntiltedPosition;
					breastTipVisualState.renderBone01.rotation = breastTipVisualState.lastUntiltedRotation;
				}
				breastTipVisualState.filteredTiltVector = Vector3.zero;
				breastTipVisualState.lastAppliedAngle = 0f;
				breastTipVisualState.lastProximity = 0f;
				breastTipVisualState.lastMarkerDisplacement = 0f;
				breastTipVisualState.preFrame = -1;
				breastTipVisualState.hasUntiltedRotation = false;
			}
		}

		// Token: 0x0600A452 RID: 42066 RVA: 0x004458BC File Offset: 0x00443ABC
		private void HandlePostSimulationSurfaceBake()
		{
			if (!this._active)
			{
				return;
			}
			this.ApplyExternalBreastGrabsPostSimulation();
			this.ApplyBreastTipTiltPostSimulation();
			if (this._surfaceReady && this._canvasService != null)
			{
				if (!this.BakeSoapSurfaceProxy())
				{
					this._canvasService.BuildPaintColliders(new BodyPaintCanvasService.PaintChannel?(BodyPaintCanvasService.PaintChannel.Body));
				}
				if (Time.unscaledTime - this._lastBakeTime > Mathf.Max(0.2f, this.surfaceRebakeInterval))
				{
					this._canvasService.BuildPaintColliders(new BodyPaintCanvasService.PaintChannel?(BodyPaintCanvasService.PaintChannel.Head));
					this._lastBakeTime = Time.unscaledTime;
				}
			}
			this.NotifyExternalBreastGrabsSolved();
		}

		// Token: 0x0600A453 RID: 42067 RVA: 0x00445950 File Offset: 0x00443B50
		private bool CreateSoapSurfaceProxy()
		{
			if (this.soapCollisionMesh == null || this._canvasService == null)
			{
				return false;
			}
			SkinnedMeshRenderer sourceRenderer = this._canvasService.GetSourceRenderer(BodyPaintCanvasService.PaintChannel.Body);
			if (sourceRenderer == null || sourceRenderer.sharedMesh == null)
			{
				return false;
			}
			this._soapSurfaceSourceRenderer = sourceRenderer;
			this._soapSurfaceSkinGO = new GameObject("SoapSurfaceSkin_LowPoly");
			this._soapSurfaceSkinGO.layer = 2;
			this._soapSurfaceSkinGO.transform.SetParent(sourceRenderer.transform.parent, false);
			this._soapSurfaceSkinGO.transform.localPosition = sourceRenderer.transform.localPosition;
			this._soapSurfaceSkinGO.transform.localRotation = sourceRenderer.transform.localRotation;
			this._soapSurfaceSkinGO.transform.localScale = sourceRenderer.transform.localScale;
			this._soapSurfaceRenderer = this._soapSurfaceSkinGO.AddComponent<SkinnedMeshRenderer>();
			this._soapSurfaceRenderer.sharedMesh = this.soapCollisionMesh;
			this._soapSurfaceRenderer.bones = sourceRenderer.bones;
			this._soapSurfaceRenderer.rootBone = sourceRenderer.rootBone;
			this._soapSurfaceRenderer.localBounds = sourceRenderer.localBounds;
			this._soapSurfaceRenderer.quality = sourceRenderer.quality;
			this._soapSurfaceRenderer.updateWhenOffscreen = true;
			this._soapSurfaceRenderer.enabled = false;
			this._soapBlendSourceIndices = new int[this.soapCollisionMesh.blendShapeCount];
			for (int i = 0; i < this.soapCollisionMesh.blendShapeCount; i++)
			{
				string blendShapeName = this.soapCollisionMesh.GetBlendShapeName(i);
				int blendShapeIndex = sourceRenderer.sharedMesh.GetBlendShapeIndex(blendShapeName);
				this._soapBlendSourceIndices[i] = blendShapeIndex;
				if (blendShapeIndex >= 0)
				{
					this._soapSurfaceRenderer.SetBlendShapeWeight(i, sourceRenderer.GetBlendShapeWeight(blendShapeIndex));
				}
			}
			this._soapSurfaceBakedMesh = new Mesh
			{
				name = "SoapSurfaceBaked_LowPoly"
			};
			this._soapSurfaceBakedMesh.MarkDynamic();
			this._soapSurfaceTriangles = this.soapCollisionMesh.triangles;
			this._soapSurfaceColliderGO = new GameObject("SoapSurfaceCollider_LowPoly");
			this._soapSurfaceColliderGO.layer = 2;
			this._soapSurfaceCollider = this._soapSurfaceColliderGO.AddComponent<MeshCollider>();
			this._soapSurfaceCollider.cookingOptions = MeshColliderCookingOptions.UseFastMidphase;
			this._soapSurfaceColliderGO.AddComponent<Rigidbody>().isKinematic = true;
			this._soapSurfaceCollider.contactOffset = 0.003f;
			if (this.BakeSoapSurfaceProxy())
			{
				return true;
			}
			this.DestroySoapSurfaceProxy();
			return false;
		}

		// Token: 0x0600A454 RID: 42068 RVA: 0x00445BB4 File Offset: 0x00443DB4
		private bool BakeSoapSurfaceProxy()
		{
			if (this._soapSurfaceRenderer == null || this._soapSurfaceCollider == null || this._soapSurfaceBakedMesh == null)
			{
				if (this._soapSurfaceCollider != null)
				{
					this._soapSurfaceCollider.enabled = false;
				}
				return false;
			}
			if (this._soapSurfaceSourceRenderer != null && this._soapBlendSourceIndices != null)
			{
				for (int i = 0; i < this._soapBlendSourceIndices.Length; i++)
				{
					int num = this._soapBlendSourceIndices[i];
					if (num >= 0)
					{
						float blendShapeWeight = this._soapSurfaceSourceRenderer.GetBlendShapeWeight(num);
						if (!Mathf.Approximately(this._soapSurfaceRenderer.GetBlendShapeWeight(i), blendShapeWeight))
						{
							this._soapSurfaceRenderer.SetBlendShapeWeight(i, blendShapeWeight);
						}
					}
				}
			}
			this._soapSurfaceRenderer.BakeMesh(this._soapSurfaceBakedMesh, true);
			if (this._soapSurfaceBakedMesh.vertexCount == 0)
			{
				this._soapSurfaceCollider.enabled = false;
				return false;
			}
			this._soapSurfaceBakedMesh.GetVertices(this._soapSurfaceVertices);
			this._soapSurfaceBakedMesh.GetNormals(this._soapSurfaceNormals);
			Transform transform = this._soapSurfaceRenderer.transform;
			Transform transform2 = this._soapSurfaceColliderGO.transform;
			Vector3 vector = transform.position + this._surfaceOffset;
			Quaternion rotation = transform.rotation;
			Vector3 lossyScale = transform.lossyScale;
			bool flag = Vector3.SqrMagnitude(transform2.position - vector) > 1E-08f || Quaternion.Angle(transform2.rotation, rotation) > 0.01f || Vector3.SqrMagnitude(transform2.localScale - lossyScale) > 1E-08f;
			if (flag)
			{
				transform2.SetPositionAndRotation(vector, rotation);
				transform2.localScale = lossyScale;
			}
			this._soapSurfaceCollider.sharedMesh = null;
			this._soapSurfaceCollider.sharedMesh = this._soapSurfaceBakedMesh;
			this._soapSurfaceCollider.contactOffset = 0.003f;
			this._soapSurfaceCollider.enabled = true;
			this._soapSurfaceColliderGO.SetActive(true);
			if (flag)
			{
				Physics.SyncTransforms();
			}
			this.EnsureNippleSurfaceAnchors();
			return true;
		}

		// Token: 0x0600A455 RID: 42069 RVA: 0x00445DA8 File Offset: 0x00443FA8
		private void DestroySoapSurfaceProxy()
		{
			if (this._soapSurfaceColliderGO != null)
			{
				Object.Destroy(this._soapSurfaceColliderGO);
			}
			if (this._soapSurfaceSkinGO != null)
			{
				Object.Destroy(this._soapSurfaceSkinGO);
			}
			if (this._soapSurfaceBakedMesh != null)
			{
				Object.Destroy(this._soapSurfaceBakedMesh);
			}
			this._soapSurfaceColliderGO = null;
			this._soapSurfaceCollider = null;
			this._soapSurfaceSkinGO = null;
			this._soapSurfaceSourceRenderer = null;
			this._soapSurfaceRenderer = null;
			this._soapSurfaceBakedMesh = null;
			this._soapSurfaceTriangles = null;
			this._soapBlendSourceIndices = null;
			this._soapSurfaceVertices.Clear();
			this._soapSurfaceNormals.Clear();
			foreach (BoobTouchModeController.BreastTipVisualState breastTipVisualState in this._breastTips)
			{
				breastTipVisualState.hasSurfaceAnchor = false;
				breastTipVisualState.surfaceTriangleIndex = -1;
				breastTipVisualState.surfaceBarycentric = Vector3.zero;
			}
		}

		// Token: 0x0600A456 RID: 42070 RVA: 0x00445EA4 File Offset: 0x004440A4
		private void CreateBall()
		{
			this._ballRoot = new GameObject("BoobTouchBall");
			this._usingSoap = false;
			this._colliderRadius = this.ballRadius;
			Vector3 vector = Vector3.one * (this.ballRadius * 2f);
			GameObject gameObject = ((this.soapPrefab != null) ? this.soapPrefab : this.FindSoapSource());
			if (gameObject != null)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject);
				gameObject2.name = "SoapVisual";
				gameObject2.transform.SetParent(this._ballRoot.transform, false);
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = gameObject.transform.lossyScale;
				gameObject2.SetActive(true);
				Collider[] componentsInChildren = gameObject2.GetComponentsInChildren<Collider>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					Object.Destroy(componentsInChildren[i]);
				}
				Rigidbody[] componentsInChildren2 = gameObject2.GetComponentsInChildren<Rigidbody>(true);
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					Object.Destroy(componentsInChildren2[i]);
				}
				MonoBehaviour[] componentsInChildren3 = gameObject2.GetComponentsInChildren<MonoBehaviour>(true);
				for (int i = 0; i < componentsInChildren3.Length; i++)
				{
					Object.Destroy(componentsInChildren3[i]);
				}
				Renderer componentInChildren = gameObject2.GetComponentInChildren<Renderer>();
				if (componentInChildren != null && this.soapTargetSize > 0f)
				{
					float num = Mathf.Max(new float[]
					{
						componentInChildren.bounds.size.x,
						componentInChildren.bounds.size.y,
						componentInChildren.bounds.size.z
					});
					if (num > 0.0001f)
					{
						gameObject2.transform.localScale *= this.soapTargetSize / num;
					}
				}
				if (componentInChildren != null)
				{
					gameObject2.transform.localPosition -= componentInChildren.bounds.center - this._ballRoot.transform.position;
					vector = componentInChildren.bounds.size;
				}
				Shader shader = Shader.Find("Universal Render Pipeline/Lit");
				if (shader != null)
				{
					Material material = new Material(shader);
					material.SetColor("_BaseColor", this.ballColor);
					material.SetFloat("_Smoothness", 0.7f);
					foreach (Renderer renderer in gameObject2.GetComponentsInChildren<Renderer>())
					{
						Material[] array = new Material[renderer.sharedMaterials.Length];
						for (int j = 0; j < array.Length; j++)
						{
							array[j] = material;
						}
						renderer.sharedMaterials = array;
					}
				}
				this._ballVisual = gameObject2.transform;
				this._usingSoap = true;
				float num2 = ((componentInChildren != null) ? Mathf.Clamp(componentInChildren.bounds.extents.magnitude * 0.6f, 0.03f, 0.1f) : this.ballRadius);
				this._colliderRadius = ((this.soapColliderRadiusOverride > 0f) ? this.soapColliderRadiusOverride : num2);
				if (this.debugLogs)
				{
					Debug.Log("[BoobTouchModeController] Soap da '" + gameObject.name + "', " + string.Format("raggio collider {0:F3}", this._colliderRadius));
				}
			}
			else
			{
				Debug.LogWarning("[BoobTouchModeController] Oggetto '" + this.soapObjectName + "' non trovato in scena: uso la sfera di fallback.");
				GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				gameObject3.name = "Visual";
				Object.Destroy(gameObject3.GetComponent<SphereCollider>());
				gameObject3.transform.SetParent(this._ballRoot.transform, false);
				gameObject3.transform.localScale = Vector3.one * (this.ballRadius * 2f);
				this._ballVisual = gameObject3.transform;
				Renderer component = gameObject3.GetComponent<Renderer>();
				Shader shader2 = Shader.Find("Universal Render Pipeline/Lit");
				if (shader2 != null)
				{
					Material material2 = new Material(shader2);
					material2.SetColor("_BaseColor", this.ballColor);
					material2.SetFloat("_Smoothness", 0.65f);
					component.material = material2;
				}
			}
			this._hairBallCollider = this._ballRoot.AddComponent<MagicaSphereCollider>();
			this._hairBallCollider.SetSize(new Vector3(this._colliderRadius, 0f, 0f));
			GameObject gameObject4 = new GameObject("BoobTouchPressureVolume");
			gameObject4.transform.SetParent(this._ballRoot.transform, false);
			this._pressureColliderTransform = gameObject4.transform;
			this._ballCollider = gameObject4.AddComponent<MagicaSphereCollider>();
			this._ballCollider.SetSize(new Vector3(this._colliderRadius, 0f, 0f));
			Vector3 vector2 = ((this._mainCamera != null) ? this._mainCamera.transform.forward : this._playerRoot.forward);
			this._ballRoot.transform.position = this._chestAnchor.position - vector2 * this.ballStartOffset;
			this._hasSurfacePoint = false;
			this._hasPrevTarget = false;
			this._hasSmoothedTarget = false;
			this._hasSurfaceFrame = false;
			this._pressAmount = 0f;
			this._debugPressUntil = -1f;
			this._physTargetValid = false;
			this._surfaceOffset = ((this._canvasService != null) ? this._canvasService.ColliderOffset : Vector3.zero);
			this.CreateSoapSurfaceProxy();
			GameObject gameObject5 = new GameObject("SoapPhysicsProxy");
			gameObject5.layer = 2;
			gameObject5.transform.SetPositionAndRotation(this._ballRoot.transform.position + this._surfaceOffset, this._ballRoot.transform.rotation);
			BoxCollider boxCollider = gameObject5.AddComponent<BoxCollider>();
			boxCollider.size = vector;
			boxCollider.isTrigger = true;
			this._soapRb = gameObject5.AddComponent<Rigidbody>();
			this._soapRb.useGravity = false;
			this._soapRb.mass = 0.25f;
			this._soapRb.drag = 4f;
			this._soapRb.angularDrag = 6f;
			this._soapRb.collisionDetectionMode = CollisionDetectionMode.Discrete;
			this._soapRb.interpolation = RigidbodyInterpolation.Interpolate;
			this._soapRb.maxAngularVelocity = 40f;
			this._soapHalfThickness = Mathf.Min(vector.x, Mathf.Min(vector.y, vector.z)) * 0.5f;
			if (this.foamEnabled)
			{
				this.CreateFoam();
			}
		}

		// Token: 0x0600A457 RID: 42071 RVA: 0x0044655F File Offset: 0x0044475F
		private GameObject FindSoapSource()
		{
			if (string.IsNullOrEmpty(this.soapObjectName))
			{
				return null;
			}
			return GameObject.Find(this.soapObjectName);
		}

		// Token: 0x0600A458 RID: 42072 RVA: 0x0044657C File Offset: 0x0044477C
		private void DestroyBall()
		{
			this.DestroySoapSurfaceProxy();
			if (this._ballRoot != null)
			{
				Object.Destroy(this._ballRoot);
				this._ballRoot = null;
				this._ballCollider = null;
				this._hairBallCollider = null;
				this._ballVisual = null;
				this._pressureColliderTransform = null;
				this._foam = null;
			}
			if (this._soapRb != null)
			{
				Object.Destroy(this._soapRb.gameObject);
				this._soapRb = null;
			}
			this._physTargetValid = false;
			this._hasSmoothedTarget = false;
			this._hasSurfaceFrame = false;
			this._pressAmount = 0f;
			this._debugPressUntil = -1f;
			this._hasLastSoapLocalPos = false;
			this._foamTravelSinceEmit = 0f;
		}

		// Token: 0x0600A459 RID: 42073 RVA: 0x00446634 File Offset: 0x00444834
		private bool RaycastSoapSurface(Ray worldRay, out Vector3 worldPoint, out Vector3 worldNormal)
		{
			worldPoint = Vector3.zero;
			worldNormal = Vector3.up;
			bool flag = false;
			float num = float.MaxValue;
			bool flag2 = this._soapSurfaceCollider != null && this._soapSurfaceCollider.enabled && this._soapSurfaceCollider.gameObject.activeInHierarchy;
			if (flag2)
			{
				Ray ray = new Ray(worldRay.origin + this._surfaceOffset, worldRay.direction);
				RaycastHit raycastHit;
				if (this._soapSurfaceCollider.Raycast(ray, out raycastHit, 100f))
				{
					flag = true;
					num = raycastHit.distance;
					worldPoint = raycastHit.point - this._surfaceOffset;
					worldNormal = this.InterpolateSoapSurfaceNormal(raycastHit);
				}
			}
			BodyPaintCanvasService.PaintChannel paintChannel;
			Vector2 vector;
			Vector3 vector2;
			Vector3 vector3;
			if (this._canvasService != null && this._canvasService.RaycastPaintSurface(worldRay, out paintChannel, out vector, out vector2, out vector3) && (!flag2 || paintChannel == BodyPaintCanvasService.PaintChannel.Head))
			{
				float num2 = Vector3.Dot(vector2 - worldRay.origin, worldRay.direction);
				if (num2 >= 0f && num2 < num)
				{
					flag = true;
					worldPoint = vector2;
					worldNormal = vector3;
				}
			}
			return flag;
		}

		// Token: 0x0600A45A RID: 42074 RVA: 0x00446762 File Offset: 0x00444962
		public bool TryRaycastInteractionSurface(Ray worldRay, out Vector3 worldPoint, out Vector3 worldNormal)
		{
			worldPoint = Vector3.zero;
			worldNormal = Vector3.up;
			return this._active && this._externalInteractorMode && this._surfaceReady && this.RaycastSoapSurface(worldRay, out worldPoint, out worldNormal);
		}

		// Token: 0x0600A45B RID: 42075 RVA: 0x004467A0 File Offset: 0x004449A0
		private Vector3 InterpolateSoapSurfaceNormal(RaycastHit hit)
		{
			int num = hit.triangleIndex * 3;
			if (this._soapSurfaceTriangles == null || num < 0 || num + 2 >= this._soapSurfaceTriangles.Length)
			{
				return hit.normal;
			}
			int num2 = this._soapSurfaceTriangles[num];
			int num3 = this._soapSurfaceTriangles[num + 1];
			int num4 = this._soapSurfaceTriangles[num + 2];
			if (num2 < 0 || num3 < 0 || num4 < 0 || num2 >= this._soapSurfaceNormals.Count || num3 >= this._soapSurfaceNormals.Count || num4 >= this._soapSurfaceNormals.Count)
			{
				return hit.normal;
			}
			Vector3 barycentricCoordinate = hit.barycentricCoordinate;
			Vector3 vector = this._soapSurfaceNormals[num2] * barycentricCoordinate.x + this._soapSurfaceNormals[num3] * barycentricCoordinate.y + this._soapSurfaceNormals[num4] * barycentricCoordinate.z;
			if (vector.sqrMagnitude < 1E-06f)
			{
				return hit.normal;
			}
			Vector3 vector2 = this._soapSurfaceCollider.transform.TransformDirection(vector.normalized).normalized;
			if (Vector3.Dot(vector2, hit.normal) < 0f)
			{
				vector2 = -vector2;
			}
			return vector2;
		}

		// Token: 0x0600A45C RID: 42076 RVA: 0x004468EC File Offset: 0x00444AEC
		private void TickBall()
		{
			if (this._ballRoot == null || this._mainCamera == null || this._chestAnchor == null)
			{
				return;
			}
			if (this._autoPushRoutine != null)
			{
				return;
			}
			bool flag = this._surfaceReady && this._canvasService != null;
			Ray ray = this._mainCamera.ScreenPointToRay(ControllerWorldPointer.Position(this));
			Ray ray2 = new Ray(ray.origin - ray.direction * 2f, ray.direction);
			Vector3 position = this._mainCamera.transform.position;
			bool flag2 = false;
			bool flag3 = false;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.up;
			if (flag)
			{
				flag2 = this.RaycastSoapSurface(ray2, out vector, out vector2);
				if (flag2 && Vector3.Dot(vector - position, ray.direction) < 0f)
				{
					flag2 = false;
				}
				if (!flag2)
				{
					float num = Mathf.Max(0.3f, Vector3.Dot(this._chestAnchor.position - position, ray.direction));
					Vector3 vector3 = position + ray.direction * num;
					Vector3 vector4 = new Vector3(this._playerRoot.position.x, vector3.y, this._playerRoot.position.z) - vector3;
					if (vector4.sqrMagnitude > 0.0004f)
					{
						vector4.Normalize();
						Ray ray3 = new Ray(vector3 - vector4 * 0.8f, vector4);
						flag2 = this.RaycastSoapSurface(ray3, out vector, out vector2);
					}
				}
				flag3 = flag2;
				if (flag2)
				{
					this._lastSurfaceLocalPos = this._chestAnchor.InverseTransformPoint(vector);
					this._lastSurfaceLocalNormal = this._chestAnchor.InverseTransformDirection(vector2);
					this._hasSurfacePoint = true;
				}
				else if (this._hasSurfacePoint)
				{
					Vector3 vector5 = this._chestAnchor.TransformPoint(this._lastSurfaceLocalPos);
					Vector3 normalized = (vector5 - position).normalized;
					Ray ray4 = new Ray(position - normalized * 2f, normalized);
					Vector3 vector6;
					Vector3 vector7;
					if (this.RaycastSoapSurface(ray4, out vector6, out vector7) && Vector3.Distance(vector6, vector5) < 0.15f)
					{
						vector = vector6;
						vector2 = vector7;
						this._lastSurfaceLocalPos = this._chestAnchor.InverseTransformPoint(vector);
						this._lastSurfaceLocalNormal = this._chestAnchor.InverseTransformDirection(vector2);
					}
					else
					{
						vector = vector5;
						vector2 = this._chestAnchor.TransformDirection(this._lastSurfaceLocalNormal).normalized;
					}
					flag2 = true;
				}
			}
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			bool flag4 = flag2 && ((ControllerInput.IsActive ? ControllerWorldPointer.Held(ControllerButton.RightTrigger) : GameplayInput.GetMouseButton(0)) || Time.unscaledTime < this._debugPressUntil);
			float num2 = (flag4 ? this.soapPressInTime : this.soapPressOutTime);
			float num3 = ((num2 <= 0.0001f) ? 1f : (unscaledDeltaTime / num2));
			this._pressAmount = Mathf.MoveTowards(this._pressAmount, flag4 ? 1f : 0f, num3);
			float num4 = Mathf.SmoothStep(0f, 1f, this._pressAmount);
			float num5 = 0f;
			float num6 = Mathf.Max(0.004f, this._soapHalfThickness * (1f - this.surfacePressRatio));
			Quaternion quaternion;
			Vector3 vector13;
			if (flag2)
			{
				Vector3 vector8;
				quaternion = this.UpdateSurfaceFrame(vector2, unscaledDeltaTime, out vector8);
				Vector3 vector9 = vector + vector8 * num6;
				Vector2 vector10 = ControllerWorldPointer.Position(this);
				if (this._hasPrevTarget && (vector10 - this._prevMouse).sqrMagnitude < 4f && flag3)
				{
					Vector3 vector11 = this._chestAnchor.TransformPoint(this._prevTargetLocal);
					if (Vector3.Dot(vector11 - vector, vector8) > num6)
					{
						vector9 = vector11;
					}
				}
				this._prevMouse = vector10;
				this._prevTargetLocal = this._chestAnchor.InverseTransformPoint(vector9);
				this._hasPrevTarget = true;
				float num7 = this.soapPressDepth * num4;
				float num8 = Mathf.Min(num7, Mathf.Max(0f, this.soapVisualPressDepth) * num4);
				Vector3 vector12 = -vector8;
				num5 = Mathf.Min(Mathf.Max(0f, this.soapSoftPressDepth), Mathf.Max(0f, num7 - num8));
				vector13 = vector9 + vector12 * num8;
			}
			else
			{
				Vector3 forward = this._mainCamera.transform.forward;
				Plane plane = new Plane(-forward, this._chestAnchor.position - forward * this.ballStartOffset);
				float num9;
				vector13 = (plane.Raycast(ray2, out num9) ? ray2.GetPoint(num9) : this._ballRoot.transform.position);
				float y = this._mainCamera.transform.eulerAngles.y;
				quaternion = Quaternion.Euler(0f, y, 0f) * Quaternion.Euler(this.soapRotationEuler);
			}
			this.SetSoftPressureDepth(num5);
			vector13 = this.FilterTargetPosition(vector13, unscaledDeltaTime);
			this._physTargetPos = vector13 + this._surfaceOffset;
			this._physTargetRot = quaternion;
			this._physTargetValid = true;
			if (this._soapRb != null)
			{
				this._ballRoot.transform.SetPositionAndRotation(this._soapRb.transform.position - this._surfaceOffset, this._soapRb.transform.rotation);
			}
			this._touchingBody = flag2 && Vector3.Distance(this._ballRoot.transform.position, vector) < this._soapHalfThickness + 0.012f + num5;
			this._contactPoint = vector;
			this.TickFoam();
			if (!this._usingSoap && this._ballVisual != null)
			{
				float num10 = this.ballRadius * 2f;
				if (!Mathf.Approximately(this._ballVisual.localScale.x, num10))
				{
					this._ballVisual.localScale = Vector3.one * num10;
					this._colliderRadius = this.ballRadius;
					this._ballCollider.SetSize(new Vector3(this.ballRadius, 0f, 0f));
					if (this._hairBallCollider != null)
					{
						this._hairBallCollider.SetSize(new Vector3(this.ballRadius, 0f, 0f));
					}
				}
			}
		}

		// Token: 0x0600A45D RID: 42077 RVA: 0x00446F74 File Offset: 0x00445174
		private void CreateFoam()
		{
			GameObject gameObject = new GameObject("SoapContactFoam");
			gameObject.transform.SetParent(this._ballRoot.transform, false);
			this._foam = gameObject.AddComponent<ParticleSystem>();
			ParticleSystem.MainModule main = this._foam.main;
			main.loop = false;
			main.playOnAwake = false;
			main.simulationSpace = ParticleSystemSimulationSpace.World;
			main.startLifetime = new ParticleSystem.MinMaxCurve(0.35f, 0.8f);
			main.startSpeed = 0f;
			main.startSize = new ParticleSystem.MinMaxCurve(0.015f, 0.045f);
			main.gravityModifier = 0f;
			main.startColor = new Color(0.97f, 0.99f, 1f, 0.75f);
			main.maxParticles = 600;
			this._foam.emission.rateOverTime = 0f;
			this._foam.shape.enabled = false;
			ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = this._foam.sizeOverLifetime;
			sizeOverLifetime.enabled = true;
			AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.55f),
				new Keyframe(0.35f, 1f),
				new Keyframe(1f, 1.1f)
			});
			sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, animationCurve);
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = this._foam.colorOverLifetime;
			colorOverLifetime.enabled = true;
			Gradient gradient = new Gradient();
			gradient.SetKeys(new GradientColorKey[]
			{
				new GradientColorKey(Color.white, 0f),
				new GradientColorKey(Color.white, 1f)
			}, new GradientAlphaKey[]
			{
				new GradientAlphaKey(0.75f, 0f),
				new GradientAlphaKey(0.55f, 0.55f),
				new GradientAlphaKey(0f, 1f)
			});
			colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
			ParticleSystemRenderer component = gameObject.GetComponent<ParticleSystemRenderer>();
			if (this.foamMaterial != null)
			{
				component.sharedMaterial = this.foamMaterial;
			}
			else
			{
				Debug.LogWarning("[BoobTouchModeController] FoamParticle.mat non assegnato: la schiuma soap resta disabilitata visivamente.", this);
			}
			this._lastSoapLocalPos = ((this._chestAnchor != null) ? this._chestAnchor.InverseTransformPoint(this._ballRoot.transform.position) : this._ballRoot.transform.position);
			this._hasLastSoapLocalPos = true;
			this._foamTravelSinceEmit = 0f;
		}

		// Token: 0x0600A45E RID: 42078 RVA: 0x00447228 File Offset: 0x00445428
		private void TickFoam()
		{
			if (!this.foamEnabled || this._foam == null || this._ballRoot == null)
			{
				return;
			}
			Vector3 vector = ((this._chestAnchor != null) ? this._chestAnchor.InverseTransformPoint(this._ballRoot.transform.position) : this._ballRoot.transform.position);
			if (!this._hasLastSoapLocalPos)
			{
				this._lastSoapLocalPos = vector;
				this._hasLastSoapLocalPos = true;
				return;
			}
			float num = Vector3.Distance(vector, this._lastSoapLocalPos);
			this._lastSoapLocalPos = vector;
			if (!this._touchingBody)
			{
				this._foamTravelSinceEmit = 0f;
				return;
			}
			this._foamTravelSinceEmit += num;
			if (this._foamTravelSinceEmit < 0.0025f)
			{
				return;
			}
			num = this._foamTravelSinceEmit;
			this._foamTravelSinceEmit = 0f;
			int num2 = Mathf.Clamp(Mathf.CeilToInt(num * 100f * Mathf.Max(0f, this.foamPerCm)), 1, 8);
			Vector3 vector2 = ((this._hasSurfaceFrame && this._smoothedSurfaceNormal.sqrMagnitude > 0.5f) ? this._smoothedSurfaceNormal.normalized : ((this._playerRoot != null) ? this._playerRoot.forward : Vector3.forward));
			Vector3 normalized = Vector3.Cross(vector2, (Mathf.Abs(vector2.y) < 0.9f) ? Vector3.up : Vector3.right).normalized;
			Vector3 normalized2 = Vector3.Cross(vector2, normalized).normalized;
			float num3 = Mathf.Max(0.003f, this._colliderRadius * 0.45f);
			ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
			for (int i = 0; i < num2; i++)
			{
				Vector2 vector3 = Random.insideUnitCircle * num3;
				emitParams.position = this._contactPoint + vector2 * 0.001f + normalized * vector3.x + normalized2 * vector3.y;
				this._foam.Emit(emitParams, 1);
			}
		}

		// Token: 0x0600A45F RID: 42079 RVA: 0x00447444 File Offset: 0x00445644
		private void SetSoftPressureDepth(float depth)
		{
			if (this._pressureColliderTransform == null)
			{
				return;
			}
			Vector3 vector = Vector3.up * Mathf.Clamp(depth, 0f, Mathf.Max(0f, this.soapSoftPressDepth));
			if ((this._pressureColliderTransform.localPosition - vector).sqrMagnitude > 1E-10f)
			{
				this._pressureColliderTransform.localPosition = vector;
			}
		}

		// Token: 0x0600A460 RID: 42080 RVA: 0x004474B4 File Offset: 0x004456B4
		private Quaternion UpdateSurfaceFrame(Vector3 rawOutwardNormal, float deltaTime, out Vector3 filteredOutwardNormal)
		{
			Vector3 vector = ((rawOutwardNormal.sqrMagnitude > 1E-06f) ? rawOutwardNormal.normalized : (this._hasSurfaceFrame ? this._smoothedSurfaceNormal : Vector3.up));
			if (!this._hasSurfaceFrame || Vector3.Dot(this._smoothedSurfaceNormal, vector) < 0.35f)
			{
				this._smoothedSurfaceNormal = vector;
				this._surfaceForward = this.PreferredSurfaceForward(vector);
				this._hasSurfaceFrame = true;
			}
			else
			{
				float num = BoobTouchModeController.ExpBlend(this.soapNormalSharpness, deltaTime);
				this._smoothedSurfaceNormal = Vector3.Slerp(this._smoothedSurfaceNormal, vector, num).normalized;
				Vector3 vector2 = Vector3.ProjectOnPlane(this._surfaceForward, this._smoothedSurfaceNormal);
				Vector3 vector3 = this.PreferredSurfaceForward(this._smoothedSurfaceNormal);
				if (vector2.sqrMagnitude < 1E-06f)
				{
					vector2 = vector3;
				}
				vector2.Normalize();
				if (Vector3.Dot(vector2, vector3) < 0f)
				{
					vector3 = -vector3;
				}
				float num2 = BoobTouchModeController.ExpBlend(this.soapRollSharpness, deltaTime);
				this._surfaceForward = Vector3.Slerp(vector2, vector3, num2).normalized;
			}
			filteredOutwardNormal = this._smoothedSurfaceNormal;
			Vector3 vector4 = -filteredOutwardNormal;
			return Quaternion.LookRotation(this._surfaceForward, vector4);
		}

		// Token: 0x0600A461 RID: 42081 RVA: 0x004475F4 File Offset: 0x004457F4
		private Vector3 PreferredSurfaceForward(Vector3 outwardNormal)
		{
			Vector3 vector = Vector3.ProjectOnPlane((this._playerRoot != null) ? (-this._playerRoot.up) : Vector3.down, outwardNormal);
			if (vector.sqrMagnitude < 1E-06f && this._mainCamera != null)
			{
				vector = Vector3.ProjectOnPlane(-this._mainCamera.transform.up, outwardNormal);
			}
			if (vector.sqrMagnitude < 1E-06f)
			{
				vector = Vector3.ProjectOnPlane((Mathf.Abs(outwardNormal.y) < 0.9f) ? Vector3.up : Vector3.forward, outwardNormal);
			}
			return vector.normalized;
		}

		// Token: 0x0600A462 RID: 42082 RVA: 0x004476A0 File Offset: 0x004458A0
		private Vector3 FilterTargetPosition(Vector3 rawTarget, float deltaTime)
		{
			if (!this._hasSmoothedTarget || Vector3.SqrMagnitude(rawTarget - this._smoothedTargetPos) > 0.0225f)
			{
				this._smoothedTargetPos = rawTarget;
				this._hasSmoothedTarget = true;
				return rawTarget;
			}
			float num = BoobTouchModeController.ExpBlend(this.soapTargetSharpness, deltaTime);
			this._smoothedTargetPos = Vector3.Lerp(this._smoothedTargetPos, rawTarget, num);
			return this._smoothedTargetPos;
		}

		// Token: 0x0600A463 RID: 42083 RVA: 0x00447704 File Offset: 0x00445904
		public void DebugDump()
		{
			Debug.Log(string.Format("[BoobTouch DUMP] active={0}, ball={1}, ", this._active, this._ballRoot ? this._ballRoot.transform.position.ToString("F3") : "null") + string.Format("chestAnchor={0}, cloths={1}", this._chestAnchor ? this._chestAnchor.name : "null", this._cloths.Count));
			this.DebugLogSoapMetrics("[BoobTouch SOAP]");
			Animator animator = ((this._faceAnimator != null) ? this._faceAnimator : this._animator);
			int num = ((animator != null) ? animator.GetInteger(this._faceParamName) : (-1));
			ShowerBubbleSpawner componentInChildren = this._playerRoot.GetComponentInChildren<ShowerBubbleSpawner>(true);
			bool flag = this._ballRoot != null && this._ballRoot.GetComponentInChildren<ParticleSystem>(true) != null;
			Debug.Log(string.Concat(new string[]
			{
				string.Format("[BoobTouch LOOK] wetHeld={0}, ", this._wetHeld),
				string.Format("wetRequested={0}, ", this._wetController != null && this._wetController.IsWetRequested),
				string.Format("wet01={0:F2}, ", (this._wetController != null) ? this._wetController.WetAmount : (-1f)),
				string.Format("faceHeld={0}, faceIndex={1}, neutral={2}, ", this._faceOverridden, num, this._modeFaceIndex),
				string.Format("emotionFaceLocked={0}, ", this._emotionController != null && this._emotionController.IsFacialExpressionOverriddenForAction),
				string.Format("showerBubbles={0}, ", componentInChildren != null && componentInChildren.BubblesActive),
				string.Format("soapParticles={0}", flag)
			}));
			foreach (BoobTouchModeController.BreastTipVisualState breastTipVisualState in this._breastTips)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[BoobTouch TIP] render01=",
					breastTipVisualState.renderBone01 ? breastTipVisualState.renderBone01.name : "null",
					", support02=",
					breastTipVisualState.supportBone02 ? breastTipVisualState.supportBone02.name : "null",
					", ",
					string.Format("tilt={0:F2}deg, proximity={1:F2}, ", breastTipVisualState.lastAppliedAngle, breastTipVisualState.lastProximity),
					string.Format("marker={0:F1}mm, ", breastTipVisualState.lastMarkerDisplacement * 1000f),
					string.Format("preFrame={0}, now={1}", breastTipVisualState.preFrame, Time.frameCount)
				}));
			}
			foreach (MagicaCloth magicaCloth in Object.FindObjectsOfType<MagicaCloth>(true))
			{
				if (magicaCloth.gameObject.name.Contains("Boob"))
				{
					ClothSerializeData serializeData = magicaCloth.SerializeData;
					ColliderCollisionConstraint.SerializeData colliderCollisionConstraint = serializeData.colliderCollisionConstraint;
					bool flag2 = magicaCloth.transform.IsChildOf(this._playerRoot);
					string text = ((serializeData.rootBones.Count > 0 && serializeData.rootBones[0]) ? serializeData.rootBones[0].name : "null");
					string text2 = string.Join(",", colliderCollisionConstraint.colliderList.ConvertAll<string>(delegate(ColliderComponent c)
					{
						if (!c)
						{
							return "null";
						}
						return c.name;
					}));
					Debug.Log(string.Concat(new string[]
					{
						string.Format("[BoobTouch DUMP] {0} (id {1}): underPlayer={2}, ", magicaCloth.name, magicaCloth.GetInstanceID(), flag2),
						string.Format("activeInHierarchy={0}, enabled={1}, ", magicaCloth.gameObject.activeInHierarchy, magicaCloth.enabled),
						string.Format("clothType={0}, rootBone={1}, ", serializeData.clothType, text),
						string.Format("collisionBones={0}, colliderList=[{1}], ", colliderCollisionConstraint.collisionBones.Count, text2),
						string.Format("radius={0}, pushLimit={1}, ", serializeData.radius.value, colliderCollisionConstraint.limitDistance.value),
						string.Format("springLimit={0}, ", serializeData.springConstraint.limitDistance),
						string.Format("initData.HasData={0}", magicaCloth.GetSerializeData2().initData.HasData())
					}), magicaCloth);
				}
			}
			if (this._ballCollider != null)
			{
				Debug.Log(string.Format("[BoobTouch DUMP] ballCollider: size={0}, ", this._ballCollider.GetSize()) + string.Format("scale={0}, pos={1:F3}", this._ballCollider.GetScale(), this._ballCollider.transform.position));
			}
		}

		// Token: 0x0600A464 RID: 42084 RVA: 0x00447C90 File Offset: 0x00445E90
		public void DebugPulsePress(float seconds = 2f)
		{
			if (!this._active)
			{
				Debug.LogWarning("[BoobTouch PRESS] Modalita' non attiva: prima Enter Mode.");
				return;
			}
			float num = Mathf.Max(0.1f, seconds);
			this._debugPressUntil = Time.unscaledTime + num;
			Debug.Log(string.Format("[BoobTouch PRESS] Pressione sintetica attiva per {0:F1}s.", num));
			base.StartCoroutine(this.DebugPressureSamples(num));
		}

		// Token: 0x0600A465 RID: 42085 RVA: 0x00447CEC File Offset: 0x00445EEC
		public void DebugStartCleavagePressure()
		{
			if (!this._active)
			{
				Debug.LogWarning("[BoobTouch CLEAVAGE] Modalita' non attiva: prima Enter Mode.");
				return;
			}
			if (this._autoPushRoutine != null)
			{
				return;
			}
			this._autoPushRoutine = base.StartCoroutine(this.CleavagePressureRoutine());
		}

		// Token: 0x0600A466 RID: 42086 RVA: 0x00447D1C File Offset: 0x00445F1C
		private IEnumerator CleavagePressureRoutine()
		{
			Transform leftBase = null;
			Transform rightBase = null;
			Transform leftTip = null;
			Transform rightTip = null;
			foreach (BoobTouchModeController.ClothBackup clothBackup in this._cloths)
			{
				if (!clothBackup.isButt && !(clothBackup.cloth == null))
				{
					foreach (Transform transform in clothBackup.cloth.SerializeData.rootBones)
					{
						if (!(transform == null))
						{
							bool flag = transform.name.EndsWith(".l");
							bool flag2 = transform.name.EndsWith(".r");
							if (transform.name.Contains("_01"))
							{
								if (flag)
								{
									leftBase = transform;
								}
								if (flag2)
								{
									rightBase = transform;
								}
							}
							else if (transform.name.Contains("_02"))
							{
								if (flag)
								{
									leftTip = transform;
								}
								if (flag2)
								{
									rightTip = transform;
								}
							}
						}
					}
				}
			}
			if (leftBase == null || rightBase == null || this._pressureColliderTransform == null)
			{
				Debug.LogError("[BoobTouch CLEAVAGE] Bone seno o volume pressione non disponibili.");
				this._autoPushRoutine = null;
				yield break;
			}
			if (leftTip == null)
			{
				leftTip = leftBase;
			}
			if (rightTip == null)
			{
				rightTip = rightBase;
			}
			Vector3 leftBaseRest = leftBase.position;
			Vector3 rightBaseRest = rightBase.position;
			Vector3 leftTipRest = leftTip.position;
			Vector3 rightTipRest = rightTip.position;
			float restTipSeparation = Vector3.Distance(leftTipRest, rightTipRest);
			Vector3 vector = (leftTipRest + rightTipRest) * 0.5f;
			Vector3 normalized = (vector - this._mainCamera.transform.position).normalized;
			Vector3 vector2 = Vector3.ProjectOnPlane(-this._playerRoot.up, normalized);
			if (vector2.sqrMagnitude < 1E-06f)
			{
				vector2 = Vector3.ProjectOnPlane(this._mainCamera.transform.up, normalized);
			}
			vector2.Normalize();
			Vector3 savedPosition = this._ballRoot.transform.position;
			Quaternion savedRotation = this._ballRoot.transform.rotation;
			float num = Mathf.Max(this._soapHalfThickness, this.soapSoftPressDepth + 0.005f);
			Vector3 vector3 = vector - normalized * num;
			this._ballRoot.transform.SetPositionAndRotation(vector3, Quaternion.LookRotation(vector2, normalized));
			this._pressAmount = 0f;
			this.SetSoftPressureDepth(0f);
			yield return new WaitForSecondsRealtime(0.25f);
			float maxLeftBase = 0f;
			float maxRightBase = 0f;
			float maxLeftTip = 0f;
			float maxRightTip = 0f;
			float maxTipSeparationGain = 0f;
			float elapsed = 0f;
			while (elapsed < 3f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num2;
				if (elapsed < 0.8f)
				{
					num2 = elapsed / 0.8f;
				}
				else if (elapsed < 2.3f)
				{
					num2 = 1f;
				}
				else
				{
					num2 = 1f - (elapsed - 2.3f) / 0.70000005f;
				}
				this._pressAmount = Mathf.Clamp01(num2);
				this.SetSoftPressureDepth(this.soapSoftPressDepth * Mathf.SmoothStep(0f, 1f, this._pressAmount));
				yield return new WaitForEndOfFrame();
				maxLeftBase = Mathf.Max(maxLeftBase, Vector3.Distance(leftBase.position, leftBaseRest));
				maxRightBase = Mathf.Max(maxRightBase, Vector3.Distance(rightBase.position, rightBaseRest));
				maxLeftTip = Mathf.Max(maxLeftTip, Vector3.Distance(leftTip.position, leftTipRest));
				maxRightTip = Mathf.Max(maxRightTip, Vector3.Distance(rightTip.position, rightTipRest));
				maxTipSeparationGain = Mathf.Max(maxTipSeparationGain, Vector3.Distance(leftTip.position, rightTip.position) - restTipSeparation);
				yield return null;
			}
			this._pressAmount = 0f;
			this.SetSoftPressureDepth(0f);
			this._ballRoot.transform.SetPositionAndRotation(savedPosition, savedRotation);
			if (this._soapRb != null)
			{
				this._soapRb.position = savedPosition + this._surfaceOffset;
				this._soapRb.rotation = savedRotation;
				this._soapRb.velocity = Vector3.zero;
				this._soapRb.angularVelocity = Vector3.zero;
			}
			bool flag3 = maxLeftTip > 0.02f && maxRightTip > 0.02f;
			Debug.Log(string.Concat(new string[]
			{
				string.Format("[BoobTouch CLEAVAGE] softDepth={0:F2}cm, ", this.soapSoftPressDepth * 100f),
				string.Format("baseL/R={0:F2}/{1:F2}cm, ", maxLeftBase * 100f, maxRightBase * 100f),
				string.Format("tipL/R={0:F2}/{1:F2}cm, ", maxLeftTip * 100f, maxRightTip * 100f),
				string.Format("aperturaPunte={0:F2}cm -> ", maxTipSeparationGain * 100f),
				flag3 ? "PRESSIONE IMPORTANTE OK" : "PRESSIONE TROPPO DEBOLE"
			}));
			this._autoPushRoutine = null;
			yield break;
		}

		// Token: 0x0600A467 RID: 42087 RVA: 0x00447D2B File Offset: 0x00445F2B
		private IEnumerator DebugPressureSamples(float duration)
		{
			float sampleDelay = Mathf.Min(0.5f, duration * 0.5f);
			yield return new WaitForSecondsRealtime(sampleDelay);
			this.DebugLogSoapMetrics("[BoobTouch PRESS SAMPLE]");
			yield return new WaitForSecondsRealtime(Mathf.Max(0f, duration - sampleDelay) + this.soapPressOutTime + 0.1f);
			this.DebugLogSoapMetrics("[BoobTouch PRESS RELEASE]");
			yield break;
		}

		// Token: 0x0600A468 RID: 42088 RVA: 0x00447D44 File Offset: 0x00445F44
		private void DebugLogSoapMetrics(string prefix)
		{
			if (this._soapRb == null)
			{
				Debug.Log(prefix + " soap non disponibile.");
				return;
			}
			float num = (this._hasSurfaceFrame ? Vector3.Dot(this._soapRb.transform.up, -this._smoothedSurfaceNormal) : float.NaN);
			float num2 = (this._physTargetValid ? Vector3.Distance(this._soapRb.position, this._physTargetPos) : float.NaN);
			float num3 = (this._physTargetValid ? Quaternion.Angle(this._soapRb.rotation, this._physTargetRot) : float.NaN);
			float num4 = Mathf.SmoothStep(0f, 1f, this._pressAmount);
			float num5 = this.soapPressDepth * num4;
			float num6 = Mathf.Min(num5, Mathf.Max(0f, this.soapVisualPressDepth) * num4);
			float num7 = ((this._pressureColliderTransform != null) ? this._pressureColliderTransform.localPosition.y : 0f);
			float num8 = ((this._ballRoot != null) ? Vector3.Distance(this._ballRoot.transform.position, this._contactPoint) : float.NaN);
			Debug.Log(string.Concat(new string[]
			{
				string.Format("{0} press={1:F3}, requestedDepth={2:F2}cm, ", prefix, this._pressAmount, num5 * 100f),
				string.Format("visualDepth={0:F2}cm, ", num6 * 100f),
				string.Format("softDepth={0:F2}cm, ", num7 * 100f),
				string.Format("surfaceGap={0:F2}cm, ", num8 * 100f),
				string.Format("faceAlignment={0:F3}, targetError={1:F2}cm, ", num, num2 * 100f),
				string.Format("rotationError={0:F2}deg, speed={1:F3}m/s, ", num3, this._soapRb.velocity.magnitude),
				string.Format("geometricContact={0}", this._touchingBody)
			}));
		}

		// Token: 0x0600A469 RID: 42089 RVA: 0x00447F67 File Offset: 0x00446167
		public void DebugStartAutoPush()
		{
			if (!this._active)
			{
				Debug.LogWarning("[BoobTouch AUTOPUSH] Modalita' non attiva: prima EnterMode.");
				return;
			}
			if (this._autoPushRoutine != null)
			{
				return;
			}
			this._autoPushRoutine = base.StartCoroutine(this.AutoPushRoutine());
		}

		// Token: 0x0600A46A RID: 42090 RVA: 0x00447F97 File Offset: 0x00446197
		public void DebugDeepDump()
		{
			base.StartCoroutine(this.DeepDumpTwice());
		}

		// Token: 0x0600A46B RID: 42091 RVA: 0x00447FA6 File Offset: 0x004461A6
		private IEnumerator DeepDumpTwice()
		{
			this.DeepDumpOnce("A");
			yield return new WaitForSecondsRealtime(1f);
			this.DeepDumpOnce("B (dopo 1s: time deve avanzare)");
			yield break;
		}

		// Token: 0x0600A46C RID: 42092 RVA: 0x00447FB8 File Offset: 0x004461B8
		private void DeepDumpOnce(string label)
		{
			TeamManager team = MagicaManager.Team;
			VirtualMeshManager vmesh = MagicaManager.VMesh;
			if (team == null || vmesh == null)
			{
				Debug.LogWarning("[BoobTouch DEEP] MagicaManager non inizializzato.");
				return;
			}
			TimeManager time = MagicaManager.Time;
			string text = "n/a";
			if (time != null)
			{
				Type type = time.GetType();
				PropertyInfo property = type.GetProperty("FixedUpdateCount", BindingFlags.Instance | BindingFlags.NonPublic);
				object obj = ((property != null) ? property.GetValue(time) : null);
				FieldInfo field = type.GetField("GlobalTimeScale", BindingFlags.Instance | BindingFlags.NonPublic);
				object obj2 = ((field != null) ? field.GetValue(time) : null);
				PropertyInfo property2 = type.GetProperty("SimulationDeltaTime", BindingFlags.Instance | BindingFlags.NonPublic);
				object obj3 = ((property2 != null) ? property2.GetValue(time) : null);
				text = string.Format("FixedUpdateCount={0}, GlobalTimeScale={1}, SimDelta={2}", obj, obj2, obj3);
			}
			Debug.Log(string.Concat(new string[]
			{
				string.Format("[BoobTouch DEEP {0}] Unity: timeScale={1}, dt={2:F4}, ", label, Time.timeScale, Time.deltaTime),
				"animator.updateMode=",
				this._animator ? this._animator.updateMode.ToString() : "null",
				", animator.cullingMode=",
				this._animator ? this._animator.cullingMode.ToString() : "null",
				" | TM: ",
				text
			}));
			foreach (MagicaCloth magicaCloth in this._playerRoot.GetComponentsInChildren<MagicaCloth>())
			{
				string name = magicaCloth.gameObject.name;
				if (name.Contains("Boob") || name.Contains("Butt"))
				{
					int teamId = magicaCloth.Process.TeamId;
					if (teamId <= 0)
					{
						Debug.LogWarning(string.Format("[BoobTouch DEEP] {0}: TeamId={1} (cloth non registrato!)", magicaCloth.name, teamId));
					}
					else
					{
						BoobTouchModeController.ClothBackup clothBackup = new BoobTouchModeController.ClothBackup
						{
							cloth = magicaCloth
						};
						TeamManager.TeamData teamData = team.teamDataArray.GetNativeArray()[teamId];
						NativeArray<VertexAttribute> nativeArray = vmesh.attributes.GetNativeArray();
						StringBuilder stringBuilder = new StringBuilder();
						int startIndex = teamData.proxyCommonChunk.startIndex;
						int dataLength = teamData.proxyCommonChunk.dataLength;
						for (int j = 0; j < dataLength; j++)
						{
							stringBuilder.Append(nativeArray[startIndex + j].Value.ToString("X2")).Append(' ');
						}
						Debug.Log(string.Concat(new string[]
						{
							string.Format("[BoobTouch DEEP] {0}: teamId={1}, valid={2}, ", clothBackup.cloth.name, teamId, teamData.IsValid),
							string.Format("enable={0}, process={1}, running={2}, ", teamData.IsEnable, teamData.IsProcess, teamData.IsRunning),
							string.Format("spring={0}, cullingInvisible={1}, ", teamData.IsSpring, teamData.IsCullingInvisible),
							string.Format("colliderCount={0}, particles={1}, ", teamData.colliderCount, teamData.particleChunk.dataLength),
							string.Format("proxyVerts={0}, attrs=[{1}], ", dataLength, stringBuilder),
							string.Format("timeScale={0}, time={1:F3}, updateCount={2}, ", teamData.timeScale, teamData.time, teamData.updateCount),
							string.Format("frameUpdateTime={0:F4}, updateMode={1}, ", teamData.frameUpdateTime, teamData.updateMode),
							string.Format("springPower={0}, ", magicaCloth.SerializeData.springConstraint.springPower),
							string.Format("springNoise={0}, ", magicaCloth.SerializeData.springConstraint.springNoise),
							string.Format("springLimit={0}", magicaCloth.SerializeData.springConstraint.limitDistance)
						}), clothBackup.cloth);
					}
				}
			}
		}

		// Token: 0x0600A46D RID: 42093 RVA: 0x004483CA File Offset: 0x004465CA
		public void DebugGravityJolt()
		{
			if (!this._active || this._autoPushRoutine != null)
			{
				return;
			}
			this._autoPushRoutine = base.StartCoroutine(this.GravityJoltRoutine());
		}

		// Token: 0x0600A46E RID: 42094 RVA: 0x004483EF File Offset: 0x004465EF
		private IEnumerator GravityJoltRoutine()
		{
			List<float> saved = new List<float>();
			List<Transform> bones = new List<Transform>();
			List<Vector3> rest = new List<Vector3>();
			foreach (BoobTouchModeController.ClothBackup clothBackup in this._cloths)
			{
				if (!(clothBackup.cloth == null))
				{
					saved.Add(clothBackup.cloth.SerializeData.gravity);
					bones.Add(clothBackup.cloth.SerializeData.rootBones[0]);
					rest.Add(clothBackup.cloth.SerializeData.rootBones[0].position);
					clothBackup.cloth.SerializeData.gravity = 30f;
					clothBackup.cloth.SetParameterChange();
				}
			}
			Debug.Log("[BoobTouch GRAVITY] gravita' 30 per 1.5s...");
			float maxDelta = 0f;
			float t = 0f;
			while (t < 1.5f)
			{
				t += Time.deltaTime;
				for (int i = 0; i < bones.Count; i++)
				{
					maxDelta = Mathf.Max(maxDelta, Vector3.Distance(bones[i].position, rest[i]));
				}
				yield return null;
			}
			int num = 0;
			foreach (BoobTouchModeController.ClothBackup clothBackup2 in this._cloths)
			{
				if (!(clothBackup2.cloth == null))
				{
					clothBackup2.cloth.SerializeData.gravity = saved[num++];
					clothBackup2.cloth.SetParameterChange();
				}
			}
			Debug.Log(string.Format("[BoobTouch GRAVITY] spostamento max={0:F2}cm -> ", maxDelta * 100f) + ((maxDelta > 0.005f) ? "LA SIMULAZIONE STEPPA (problema = collisione)" : "SIM FERMA (problema = stepping, non collisione)"));
			this._autoPushRoutine = null;
			yield break;
		}

		// Token: 0x0600A46F RID: 42095 RVA: 0x004483FE File Offset: 0x004465FE
		public void DebugRebuildAndPush()
		{
			if (!this._active)
			{
				Debug.LogWarning("[BoobTouch REBUILD] Modalita' non attiva: prima EnterMode.");
				return;
			}
			if (this._autoPushRoutine != null)
			{
				return;
			}
			this._autoPushRoutine = base.StartCoroutine(this.RebuildAndPushRoutine());
		}

		// Token: 0x0600A470 RID: 42096 RVA: 0x0044842E File Offset: 0x0044662E
		private IEnumerator RebuildAndPushRoutine()
		{
			Debug.Log("[BoobTouch REBUILD] Disable -> enable dei cloth (rebuild col collider gia' in lista)...");
			foreach (BoobTouchModeController.ClothBackup clothBackup in this._cloths)
			{
				if (clothBackup.cloth != null)
				{
					clothBackup.cloth.enabled = false;
				}
			}
			yield return new WaitForSeconds(0.3f);
			foreach (BoobTouchModeController.ClothBackup clothBackup2 in this._cloths)
			{
				if (clothBackup2.cloth != null)
				{
					clothBackup2.cloth.enabled = true;
				}
			}
			yield return new WaitForSeconds(2f);
			Debug.Log("[BoobTouch REBUILD] Rebuild completato, parte l'auto push.");
			yield return this.AutoPushRoutine();
			yield break;
		}

		// Token: 0x0600A471 RID: 42097 RVA: 0x0044843D File Offset: 0x0044663D
		private IEnumerator AutoPushRoutine()
		{
			Debug.Log("[BoobTouch AUTOPUSH] Start: 2 spinte da 2s per tetta lungo l'asse camera.");
			Vector3 camForward = this._mainCamera.transform.forward;
			foreach (BoobTouchModeController.ClothBackup entry in this._cloths)
			{
				if (!(entry.cloth == null) && entry.cloth.SerializeData.rootBones.Count != 0)
				{
					foreach (Transform bone in entry.cloth.SerializeData.rootBones)
					{
						if (!(bone == null))
						{
							Vector3 restPos = bone.position;
							Quaternion restRot = bone.rotation;
							Vector3 camRight = this._mainCamera.transform.right;
							float maxDelta = 0f;
							float maxAngle = 0f;
							float maxVisualTipTilt = 0f;
							float maxVisualMarker = 0f;
							float maxFrontMarker = 0f;
							float maxLateralMarker = 0f;
							BoobTouchModeController.BreastTipVisualState visualTip = this.FindBreastTipState(bone);
							float t = 0f;
							while (t < 4f)
							{
								t += Time.deltaTime;
								if (t < 2f)
								{
									float num = Mathf.Sin(t / 2f * 3.1415927f);
									float num2 = Mathf.Lerp(0.25f, -0.03f, num);
									this._ballRoot.transform.position = restPos - camForward * num2;
								}
								else
								{
									float num3 = Mathf.Sin((t - 2f) * 3.1415927f * 2f / 1.5f) * 0.08f;
									this._ballRoot.transform.position = restPos + camForward * 0.01f + camRight * num3;
								}
								yield return new WaitForEndOfFrame();
								maxDelta = Mathf.Max(maxDelta, Vector3.Distance(bone.position, restPos));
								maxAngle = Mathf.Max(maxAngle, Quaternion.Angle(restRot, bone.rotation));
								if (visualTip != null)
								{
									maxVisualTipTilt = Mathf.Max(maxVisualTipTilt, visualTip.lastAppliedAngle);
									maxVisualMarker = Mathf.Max(maxVisualMarker, visualTip.lastMarkerDisplacement);
									if (t < 2f)
									{
										maxFrontMarker = Mathf.Max(maxFrontMarker, visualTip.lastMarkerDisplacement);
									}
									else
									{
										maxLateralMarker = Mathf.Max(maxLateralMarker, visualTip.lastMarkerDisplacement);
									}
								}
								yield return null;
							}
							Debug.Log(string.Concat(new string[]
							{
								"[BoobTouch AUTOPUSH] ",
								entry.cloth.name,
								"/",
								bone.name,
								": ",
								string.Format("spostamento max={0:F2}cm, ROTAZIONE max={1:F1} gradi -> ", maxDelta * 100f, maxAngle),
								(maxDelta > 0.01f) ? "spinta OK" : "spinta KO",
								", ",
								(maxAngle > 3f) ? "rotazione OK" : "rotazione KO",
								(visualTip != null) ? (string.Format(", TIP_TILT max={0:F2} gradi, ", maxVisualTipTilt) + string.Format("marker max={0:F1}mm ", maxVisualMarker * 1000f) + string.Format("(front={0:F1}, lateral={1:F1})", maxFrontMarker * 1000f, maxLateralMarker * 1000f)) : ""
							}));
							restPos = default(Vector3);
							restRot = default(Quaternion);
							camRight = default(Vector3);
							visualTip = null;
							bone = null;
						}
					}
					List<Transform>.Enumerator enumerator2 = default(List<Transform>.Enumerator);
					entry = null;
				}
			}
			List<BoobTouchModeController.ClothBackup>.Enumerator enumerator = default(List<BoobTouchModeController.ClothBackup>.Enumerator);
			this._autoPushRoutine = null;
			yield break;
			yield break;
		}

		// Token: 0x0600A472 RID: 42098 RVA: 0x0044844C File Offset: 0x0044664C
		private BoobTouchModeController.BreastTipVisualState FindBreastTipState(Transform bone)
		{
			foreach (BoobTouchModeController.BreastTipVisualState breastTipVisualState in this._breastTips)
			{
				if (breastTipVisualState.renderBone01 == bone || breastTipVisualState.supportBone02 == bone)
				{
					return breastTipVisualState;
				}
			}
			return null;
		}

		// Token: 0x0600A473 RID: 42099 RVA: 0x004484BC File Offset: 0x004466BC
		private void SetPose(bool enter)
		{
			if (this._animator == null || !this._animator.gameObject.activeInHierarchy)
			{
				return;
			}
			if (!enter)
			{
				string[] array = new string[] { "Free Movement", "Locomotion", "Idle", "Grounded" };
				for (int i = 0; i < array.Length; i++)
				{
					int num = Animator.StringToHash(array[i]);
					if (this._animator.HasState(0, num))
					{
						this._animator.CrossFadeInFixedTime(num, this.poseBlendSeconds, 0);
						return;
					}
				}
				return;
			}
			string text = (this._externalInteractorMode ? this.gropingPoseStateName : this.poseStateName);
			int num2 = Animator.StringToHash(text);
			if (!this._animator.HasState(0, num2))
			{
				Debug.LogWarning("[BoobTouchModeController] Stato '" + text + "' non trovato sul Base Layer. Continuo senza animazione.", this);
				return;
			}
			this._animator.CrossFadeInFixedTime(num2, this.poseBlendSeconds, 0);
		}

		// Token: 0x0600A474 RID: 42100 RVA: 0x004485AC File Offset: 0x004467AC
		private void HideGameHUD()
		{
			this._disabledCanvases.Clear();
			this._hiddenHudSiblings.Clear();
			Transform transform = ((InteractionHUDController.Instance != null) ? InteractionHUDController.Instance.transform : null);
			GropingFaceCameraOverlay component = base.GetComponent<GropingFaceCameraOverlay>();
			foreach (Canvas canvas in Object.FindObjectsOfType<Canvas>())
			{
				if (canvas.isRootCanvas && canvas.enabled && canvas.renderMode != RenderMode.WorldSpace && (!(component != null) || !component.OwnsCanvas(canvas)) && !(canvas.GetComponentInChildren<CielVoiceSubtitleUI>(true) != null))
				{
					if (transform != null && transform.IsChildOf(canvas.transform))
					{
						this.HideAllExceptBranch(canvas.transform, transform);
					}
					else
					{
						canvas.enabled = false;
						this._disabledCanvases.Add(canvas);
					}
				}
			}
		}

		// Token: 0x0600A475 RID: 42101 RVA: 0x0044868C File Offset: 0x0044688C
		private void HideAllExceptBranch(Transform root, Transform branch)
		{
			BoobTouchModeController.<>c__DisplayClass341_0 CS$<>8__locals1;
			CS$<>8__locals1.branch = branch;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.path = new HashSet<Transform>();
			Transform transform = CS$<>8__locals1.branch;
			while (transform != null)
			{
				CS$<>8__locals1.path.Add(transform);
				transform = transform.parent;
			}
			this.<HideAllExceptBranch>g__Recurse|341_0(root, ref CS$<>8__locals1);
		}

		// Token: 0x0600A476 RID: 42102 RVA: 0x004486E4 File Offset: 0x004468E4
		private void RestoreGameHUD()
		{
			foreach (Canvas canvas in this._disabledCanvases)
			{
				if (canvas != null)
				{
					canvas.enabled = true;
				}
			}
			this._disabledCanvases.Clear();
			foreach (GameObject gameObject in this._hiddenHudSiblings)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
			this._hiddenHudSiblings.Clear();
		}

		// Token: 0x0600A477 RID: 42103 RVA: 0x004487A4 File Offset: 0x004469A4
		private void ShowModeControlsHud()
		{
			if (InteractionHUDController.Instance == null)
			{
				return;
			}
			if (this._modeControlsConfig == null)
			{
				this._modeControlsConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			}
			this._modeControlsConfig.usableActions.Clear();
			if (this._externalInteractorMode)
			{
				this._modeControlsThirdPoseAvailable = this.IsThirdHandPoseAvailable;
				this._modeControlsConfig.usableActions.Add(new UsableAction
				{
					actionName = "Hand Pose",
					keyDisplayLabel = (this._modeControlsThirdPoseAvailable ? "1 / 2 / 3" : "1 / 2")
				});
				this._modeControlsConfig.usableActions.Add(new UsableAction
				{
					actionName = "Grab (Hold)",
					keyDisplayLabel = "LMB"
				});
				if (this._modeControlsThirdPoseAvailable)
				{
					this._modeControlsConfig.usableActions.Add(new UsableAction
					{
						actionName = "Push / Pull Two Fingers",
						keyDisplayLabel = "LMB + MOUSE Y"
					});
				}
				this._modeControlsConfig.usableActions.Add(new UsableAction
				{
					actionName = "Place / Reset Hands",
					keyDisplayLabel = "SPACE"
				});
			}
			else
			{
				this._modeControlsConfig.usableActions.Add(new UsableAction
				{
					actionName = this.pressureActionLabel,
					keyDisplayLabel = this.pressureKeyLabel
				});
			}
			this._modeControlsConfig.usableActions.Add(new UsableAction
			{
				actionName = this.exitActionLabel,
				keyDisplayLabel = (this._externalInteractorMode ? "X" : this.exitKeyLabel)
			});
			this._modeControlsConfig.usableActions.Add(new UsableAction
			{
				actionName = "Orbit Camera",
				keyDisplayLabel = "RMB + DRAG"
			});
			this._modeControlsConfig.usableActions.Add(new UsableAction
			{
				actionName = "Lower / Raise Camera",
				keyDisplayLabel = "MMB + DRAG"
			});
			this._modeControlsConfig.usableActions.Add(new UsableAction
			{
				actionName = "Zoom Camera",
				keyDisplayLabel = "WHEEL"
			});
			ControllerContextBindings.SetLabel(this._modeControlsConfig, "RMB + DRAG", "LS", false);
			ControllerContextBindings.SetLabel(this._modeControlsConfig, "MMB + DRAG", "LB / RB", false);
			ControllerContextBindings.SetLabel(this._modeControlsConfig, "WHEEL", "LT + RS", false);
			ControllerContextBindings.SetLabel(this._modeControlsConfig, this._externalInteractorMode ? "X" : this.exitKeyLabel, "B", false);
			if (this._externalInteractorMode)
			{
				ControllerContextBindings.SetLabel(this._modeControlsConfig, "1 / 2", "D-Pad Left / D-Pad Up", false);
				ControllerContextBindings.SetLabel(this._modeControlsConfig, "1 / 2 / 3", "D-Pad Left / D-Pad Up / D-Pad Right", false);
				ControllerContextBindings.SetLabel(this._modeControlsConfig, "LMB", "RT", false);
				ControllerContextBindings.SetLabel(this._modeControlsConfig, "LMB + MOUSE Y", "RT + RS", false);
				ControllerContextBindings.SetLabel(this._modeControlsConfig, "SPACE", "A", false);
			}
			else
			{
				ControllerContextBindings.SetLabel(this._modeControlsConfig, this.pressureKeyLabel, "RT", false);
			}
			InteractionHUDController.Instance.Show(this._modeControlsConfig);
		}

		// Token: 0x0600A478 RID: 42104 RVA: 0x00170130 File Offset: 0x0016E330
		private void HideModeControlsHud()
		{
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Hide();
			}
		}

		// Token: 0x0600A47B RID: 42107 RVA: 0x004490B8 File Offset: 0x004472B8
		[CompilerGenerated]
		private void <HideAllExceptBranch>g__Recurse|341_0(Transform parent, ref BoobTouchModeController.<>c__DisplayClass341_0 A_2)
		{
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				if (!(transform == A_2.branch))
				{
					if (A_2.path.Contains(transform))
					{
						this.<HideAllExceptBranch>g__Recurse|341_0(transform, ref A_2);
					}
					else if (transform.gameObject.activeSelf)
					{
						transform.gameObject.SetActive(false);
						this._hiddenHudSiblings.Add(transform.gameObject);
					}
				}
			}
		}

		// Token: 0x0400A7CD RID: 42957
		private const string GateReason = "BoobTouch";

		// Token: 0x0400A7CE RID: 42958
		private const string WetOwner = "BoobTouchMode";

		// Token: 0x0400A7CF RID: 42959
		private const PlayerActivity RequiredActivity = PlayerActivity.Interact;

		// Token: 0x0400A7D0 RID: 42960
		private const PlayerContextResource ModeResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem;

		// Token: 0x0400A7D1 RID: 42961
		[Header("Input")]
		[Tooltip("None = si entra tenendo premuto E sulla doccia di casa (ESC per uscire). Impostare un tasto solo per debug.")]
		[SerializeField]
		private KeyCode toggleKey;

		// Token: 0x0400A7D2 RID: 42962
		[Header("Camera")]
		[Tooltip("Distanza iniziale della camera orbitale in modalita' saponetta.")]
		[SerializeField]
		private float soapCameraDistance = 0.5f;

		// Token: 0x0400A7D3 RID: 42963
		[Tooltip("Close-up massimo consentito: evita che la camera entri nel torace.")]
		[SerializeField]
		private float soapCameraMinDistance = 0.28f;

		// Token: 0x0400A7D4 RID: 42964
		[Tooltip("Distanza massima dello zoom, mantenendo il corpo comodo da raggiungere.")]
		[SerializeField]
		private float soapCameraMaxDistance = 0.85f;

		// Token: 0x0400A7D5 RID: 42965
		[Tooltip("Close-up iniziale usato dalla modalita' groping.")]
		[SerializeField]
		private float gropingCameraDistance = 0.42f;

		// Token: 0x0400A7D6 RID: 42966
		[SerializeField]
		private float gropingCameraMinDistance = 0.32f;

		// Token: 0x0400A7D7 RID: 42967
		[SerializeField]
		private float gropingCameraMaxDistance = 0.58f;

		// Token: 0x0400A7D8 RID: 42968
		[Header("Animazione")]
		[Tooltip("Stato del Base Layer riprodotto in modalita': l'anim doccia in loop (SENZA bolle: lo spawner delle bolle doccia non viene toccato)")]
		[SerializeField]
		private string poseStateName = "ShowerAnim_1";

		// Token: 0x0400A7D9 RID: 42969
		[Tooltip("Posa ferma usata dalla modalita' groping.")]
		[SerializeField]
		private string gropingPoseStateName = "BodyPaintPose_1";

		// Token: 0x0400A7DA RID: 42970
		[SerializeField]
		private float poseBlendSeconds = 0.25f;

		// Token: 0x0400A7DB RID: 42971
		[Header("Aderenza al corpo")]
		[Tooltip("Quanto la faccia della saponetta resta vicina alla superficie a riposo, come frazione del suo mezzo-spessore.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float surfacePressRatio;

		// Token: 0x0400A7DC RID: 42972
		[Tooltip("Ogni quanto ri-bakare i collider dei canali NON-Body (testa). Il canale Body usa il proxy skinned ottimizzato e viene aggiornato ogni frame.")]
		[SerializeField]
		private float surfaceRebakeInterval = 0.5f;

		// Token: 0x0400A7DD RID: 42973
		[Header("Pressione (tasto sinistro)")]
		[Tooltip("Pressione totale richiesta tenendo premuto LMB. Comprende il piccolo affondo visibile e l'anticipo invisibile del volume Magica che muove i tessuti.")]
		[SerializeField]
		private float soapPressDepth = 0.03f;

		// Token: 0x0400A7DE RID: 42974
		[Tooltip("Affondo massimo VISIBILE della saponetta. Il limite la mantiene vicino alla superficie anche quando la pressione totale e' molto piu' forte.")]
		[SerializeField]
		private float soapVisualPressDepth = 0.01f;

		// Token: 0x0400A7DF RID: 42975
		[Tooltip("Anticipo massimo del volume Magica rispetto al visual. Permette di superare gli ostacoli e continuare a spostare i tessuti senza incastrare la saponetta.")]
		[SerializeField]
		private float soapSoftPressDepth = 0.02f;

		// Token: 0x0400A7E0 RID: 42976
		[Tooltip("Secondi per arrivare alla pressione massima.")]
		[SerializeField]
		private float soapPressInTime = 0.2f;

		// Token: 0x0400A7E1 RID: 42977
		[Tooltip("Secondi per rilasciare completamente la pressione.")]
		[SerializeField]
		private float soapPressOutTime = 0.18f;

		// Token: 0x0400A7E2 RID: 42978
		[Tooltip("Moltiplicatore della risposta del trascinamento quando LMB e' premuto.")]
		[SerializeField]
		private float soapPressDriveMultiplier = 1.4f;

		// Token: 0x0400A7E3 RID: 42979
		[Tooltip("Tenuta dell'orientamento durante la spinta: evita che i contatti forti sollevino un bordo della saponetta.")]
		[SerializeField]
		private float soapPressRotationMultiplier = 1.75f;

		// Token: 0x0400A7E4 RID: 42980
		[Header("Stabilita' aderenza")]
		[Tooltip("Filtro del punto bersaglio. Piu' alto = segue piu' rapidamente il mouse.")]
		[SerializeField]
		private float soapTargetSharpness = 35f;

		// Token: 0x0400A7E5 RID: 42981
		[Tooltip("Filtro delle normali triangolari della pelle: elimina scatti tra facce vicine.")]
		[SerializeField]
		private float soapNormalSharpness = 20f;

		// Token: 0x0400A7E6 RID: 42982
		[Tooltip("Velocita' con cui il roll torna verticale senza flip sulle superfici curve.")]
		[SerializeField]
		private float soapRollSharpness = 12f;

		// Token: 0x0400A7E7 RID: 42983
		[Header("Proxy pelle ottimizzato")]
		[Tooltip("Versione skinned semplificata del corpo, generata dal setup. Mantiene collisione e deformazioni ogni frame senza ricuocere i ~35k triangoli del render mesh.")]
		[SerializeField]
		private Mesh soapCollisionMesh;

		// Token: 0x0400A7E8 RID: 42984
		[Header("Movimento saponetta (rigidbody non bloccante)")]
		[Tooltip("Guadagno del trascinamento: velocita' = errore * gain (piu' alto = piu' incollata al mouse, mai teleport)")]
		[SerializeField]
		private float soapDragGain = 30f;

		// Token: 0x0400A7E9 RID: 42985
		[Tooltip("Velocita' massima (m/s): limita gli strappi quando il mouse scatta lontano")]
		[SerializeField]
		private float soapMaxSpeed = 5f;

		// Token: 0x0400A7EA RID: 42986
		[Tooltip("Smorzamento dell'accelerazione verso la velocita' richiesta.")]
		[SerializeField]
		private float soapVelocityResponse = 40f;

		// Token: 0x0400A7EB RID: 42987
		[Tooltip("Molla di rotazione verso l'allineamento alla pelle (i contatti possono ancora inclinare la saponetta)")]
		[SerializeField]
		private float soapRotationGain = 16f;

		// Token: 0x0400A7EC RID: 42988
		[Tooltip("Smorzamento della velocita' angolare verso l'orientamento richiesto.")]
		[SerializeField]
		private float soapAngularResponse = 35f;

		// Token: 0x0400A7ED RID: 42989
		[Header("Pallina")]
		[Tooltip("Raggio del collider Magica (il visual segue). ~4-5cm reali")]
		[SerializeField]
		private float ballRadius = 0.045f;

		// Token: 0x0400A7EE RID: 42990
		[Tooltip("Distanza del piano di FALLBACK dal petto verso la camera (usato solo quando il mouse non punta il corpo)")]
		[SerializeField]
		private float ballStartOffset = 0.12f;

		// Token: 0x0400A7EF RID: 42991
		[SerializeField]
		private Color ballColor = new Color(1f, 0.55f, 0.75f, 1f);

		// Token: 0x0400A7F0 RID: 42992
		[Header("Fisica spinta (solo durante la modalita')")]
		[Tooltip("Corsa massima locale con cui il collider puo' spingere il seno. Con la scala 1.4 di Ciel, 0.035 limita la deformazione a circa 4.9 cm.")]
		[SerializeField]
		private float pushLimitDistance = 0.035f;

		// Token: 0x0400A7F1 RID: 42993
		[Tooltip("Corsa massima della molla (springConstraint.limitDistance). Deve essere >= pushLimitDistance; 0.04 equivale a circa 5.6 cm reali.")]
		[SerializeField]
		private float springLimitDistance = 0.04f;

		// Token: 0x0400A7F2 RID: 42994
		[Header("Cloth bersaglio")]
		[Tooltip("BoneSpring (tette/sedere): collisionBones dal setup tool + limiti alzati in modalita'")]
		[SerializeField]
		private string[] boobClothNames = new string[] { "BoobL", "BoobR", "ButtL", "ButtR" };

		// Token: 0x0400A7F3 RID: 42995
		[Tooltip("BoneCloth (capelli/ciocche): collisione nativa sulle particelle Move, basta registrare il collider a runtime")]
		[SerializeField]
		private string[] hairClothNames = new string[] { "HairPhysics", "Sao_Physics", "Tama_Physics" };

		// Token: 0x0400A7F4 RID: 42996
		[Header("Seno (volume collisionale in modalita')")]
		[Tooltip("Raggio particella dei BoneSpring seno. A 0.035 il volume centrale raggiunge entrambi i lati del solco senza gonfiare il collider visibile.")]
		[SerializeField]
		private float breastParticleRadius = 0.035f;

		// Token: 0x0400A7F5 RID: 42997
		[Header("Risposta visiva punta seno")]
		[Tooltip("Inclina c_breast_01, il bone realmente pesato sui capezzoli, nella direzione della spinta dopo Magica. _02 e _end restano nel solver senza pendoli Move.")]
		[SerializeField]
		private bool breastTipTiltEnabled = true;

		// Token: 0x0400A7F6 RID: 42998
		[Tooltip("Angolo additivo massimo sul vero driver render _01. Cinque gradi sono visibili sulla punta senza la deformazione del vecchio pendolo da circa 30 gradi.")]
		[SerializeField]
		private float breastTipTiltMaxDegrees = 5f;

		// Token: 0x0400A7F7 RID: 42999
		[Tooltip("Guadagno angolare dello spostamento laterale, in gradi per metro.")]
		[SerializeField]
		private float breastTipTiltDegreesPerMeter = 300f;

		// Token: 0x0400A7F8 RID: 43000
		[Tooltip("Arretramento WORLD del pivot visuale di _01. A 3 cm, 5 gradi producono una deviazione leggibile della punta senza aumentare l'angolo/deformazione.")]
		[SerializeField]
		private float breastTipVirtualPivotBack = 0.03f;

		// Token: 0x0400A7F9 RID: 43001
		[Tooltip("Spostamento laterale ignorato per non trasformare il jiggle minimo in rotazione.")]
		[SerializeField]
		private float breastTipTiltDeadZone = 0.0025f;

		// Token: 0x0400A7FA RID: 43002
		[Tooltip("Velocita' con cui la punta segue una nuova spinta.")]
		[SerializeField]
		private float breastTipTiltAttackSharpness = 16f;

		// Token: 0x0400A7FB RID: 43003
		[Tooltip("Velocita' con cui la punta torna frontale al rilascio.")]
		[SerializeField]
		private float breastTipTiltReleaseSharpness = 10f;

		// Token: 0x0400A7FC RID: 43004
		[Tooltip("Quota del moto del supporto _02 rimossa dal delta del driver capezzolo _01. Evita che il moto comune del seno venga scambiato per un cambio di direzione.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float breastTipBaseMotionCancellation = 0.25f;

		// Token: 0x0400A7FD RID: 43005
		[Header("Presa mani esterne (groping)")]
		[Tooltip("Distanza WORLD massima dalla punta _01 per agganciare l'intero seno.")]
		[SerializeField]
		[Min(0.01f)]
		private float externalBreastGrabAcquireRadius = 0.16f;

		// Token: 0x0400A7FE RID: 43006
		[Tooltip("Il pinch e' volutamente piu' selettivo: fuori da questo raggio non aggancia.")]
		[SerializeField]
		[Min(0.01f)]
		private float externalNippleGrabAcquireRadius = 0.065f;

		// Token: 0x0400A7FF RID: 43007
		[Tooltip("Raggio WORLD di sicurezza della presa intera rispetto alla posa fresca di Magica.")]
		[SerializeField]
		[Min(0.01f)]
		private float externalBreastGrabMaxOffset = 0.075f;

		// Token: 0x0400A800 RID: 43008
		[Tooltip("Raggio WORLD di sicurezza del driver capezzolo durante il pinch.")]
		[SerializeField]
		[Min(0.01f)]
		private float externalNippleGrabMaxOffset = 0.055f;

		// Token: 0x0400A801 RID: 43009
		[Tooltip("Escursione laterale massima del seno nel frame locale del torace.")]
		[SerializeField]
		[Min(0.005f)]
		private float externalBreastGrabLateralLimit = 0.065f;

		// Token: 0x0400A802 RID: 43010
		[Tooltip("Escursione verticale massima del seno nel frame locale del torace.")]
		[SerializeField]
		[Min(0.005f)]
		private float externalBreastGrabVerticalLimit = 0.06f;

		// Token: 0x0400A803 RID: 43011
		[Tooltip("Escursione massima del seno verso l'esterno del torace.")]
		[SerializeField]
		[Min(0.005f)]
		private float externalBreastGrabOutwardLimit = 0.075f;

		// Token: 0x0400A804 RID: 43012
		[Tooltip("Compressione massima del seno verso il torace.")]
		[SerializeField]
		[Min(0.005f)]
		private float externalBreastGrabInwardLimit = 0.025f;

		// Token: 0x0400A805 RID: 43013
		[Tooltip("Escursione laterale massima durante il pinch del capezzolo.")]
		[SerializeField]
		[Min(0.005f)]
		private float externalNippleGrabLateralLimit = 0.045f;

		// Token: 0x0400A806 RID: 43014
		[Tooltip("Escursione verticale massima durante il pinch del capezzolo.")]
		[SerializeField]
		[Min(0.005f)]
		private float externalNippleGrabVerticalLimit = 0.045f;

		// Token: 0x0400A807 RID: 43015
		[Tooltip("Escursione verso l'esterno massima durante il pinch del capezzolo.")]
		[SerializeField]
		[Min(0.005f)]
		private float externalNippleGrabOutwardLimit = 0.055f;

		// Token: 0x0400A808 RID: 43016
		[Tooltip("Compressione verso il torace massima durante il pinch del capezzolo.")]
		[SerializeField]
		[Min(0.005f)]
		private float externalNippleGrabInwardLimit = 0.02f;

		// Token: 0x0400A809 RID: 43017
		[Tooltip("Rigidita' dell'aggancio post-simulazione. Piu' alto = seno piu' solidale alla mano.")]
		[SerializeField]
		[Min(0f)]
		private float externalGrabSharpness = 28f;

		// Token: 0x0400A80A RID: 43018
		[Tooltip("Quota della presa intera trasferita al supporto _02. Il limite evita di trascinare il busto.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float externalBreastSupportFollow = 0.35f;

		// Token: 0x0400A80B RID: 43019
		[Tooltip("Quota del movimento pinch trasferita al supporto _02: muove tutto il seno lasciando alla punta un piccolo vantaggio.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float externalNippleSupportFollow = 0.25f;

		// Token: 0x0400A80C RID: 43020
		[Tooltip("Escursione WORLD assoluta massima del supporto _02 dalla posa anatomica.")]
		[SerializeField]
		[Min(0.005f)]
		private float externalSupportMaxOffset = 0.025f;

		// Token: 0x0400A80D RID: 43021
		[Header("Sedere (pesi skin deboli: serve piu' corsa)")]
		[Tooltip("La carne delle chiappe e' pesata solo in parte su Butt_L/R (467 vertici vs i ~6000 delle tette): il bone deve muoversi di PIU' per un effetto visibile")]
		[SerializeField]
		private float buttPushMultiplier = 2f;

		// Token: 0x0400A80E RID: 43022
		[Tooltip("Raggio particella dei cloth Butt durante la modalita' (default cloth 0.02: il bone sta in profondita', con un raggio piu' largo il contatto arriva prima)")]
		[SerializeField]
		private float buttParticleRadius = 0.05f;

		// Token: 0x0400A80F RID: 43023
		[Header("Soap")]
		[Tooltip("Modello della saponetta (assegnato dal setup tool: Assets/BathSoap.obj). Ha priorita' sul nome in scena")]
		[SerializeField]
		private GameObject soapPrefab;

		// Token: 0x0400A810 RID: 43024
		[Tooltip("Dimensione massima della saponetta in metri (il modello viene riscalato)")]
		[SerializeField]
		private float soapTargetSize = 0.055f;

		// Token: 0x0400A811 RID: 43025
		[Tooltip("Orientamento della saponetta (euler), applicato RELATIVO alla yaw della camera: orbitando la vedi sempre cosi'. Preso dal BathSoap posato in scena dall'utente (X~85 = faccia piatta in avanti)")]
		[SerializeField]
		private Vector3 soapRotationEuler = new Vector3(85.45f, 0f, 0f);

		// Token: 0x0400A812 RID: 43026
		[Tooltip("Fallback: nome di un oggetto in scena da clonare (se il prefab manca)")]
		[SerializeField]
		private string soapObjectName = "BathSoap";

		// Token: 0x0400A813 RID: 43027
		[Tooltip("Raggio del collider Magica quando si usa il Soap (0 = auto dai bounds)")]
		[SerializeField]
		private float soapColliderRadiusOverride = 0.04f;

		// Token: 0x0400A814 RID: 43028
		[Header("Schiuma locale della saponetta")]
		[Tooltip("Emette le particelle soltanto mentre la saponetta si muove realmente a contatto col corpo. Non attiva mai le bolle della doccia.")]
		[SerializeField]
		private bool foamEnabled = true;

		// Token: 0x0400A815 RID: 43029
		[Tooltip("Particelle di schiuma per cm di movimento del soap sul corpo.")]
		[SerializeField]
		private float foamPerCm = 0.8f;

		// Token: 0x0400A816 RID: 43030
		[Tooltip("Materiale URP delle particelle, assegnato dal setup tool.")]
		[SerializeField]
		private Material foamMaterial;

		// Token: 0x0400A817 RID: 43031
		[Header("Occhi chiusi durante la modalita'")]
		[Tooltip("Forza i blendshape delle palpebre dopo l'Animator per tutta la modalita' soap.")]
		[SerializeField]
		private bool closeEyesDuringMode = true;

		// Token: 0x0400A818 RID: 43032
		[SerializeField]
		[Range(0f, 100f)]
		private float eyeCloseBlendShapeWeight = 100f;

		// Token: 0x0400A819 RID: 43033
		[SerializeField]
		private string[] eyeCloseBlendShapeNames = new string[] { "eye_close_sleep_L", "eye_close_sleep_R" };

		// Token: 0x0400A81A RID: 43034
		[Tooltip("Blendshape oculari che potrebbero contrastare la chiusura e vengono azzerati temporaneamente. Ogni valore originale viene ripristinato all'uscita.")]
		[SerializeField]
		private string[] eyeBlendShapeNamesToClear = new string[] { "eye_zito_L", "eye_zito_R", "eye_zito2_L", "eye_zito2_R", "eye_pleasure_L", "eye_pleasure_R", "eye_under_up_L", "eye_under_up_R", "vrc.blink", "blink" };

		// Token: 0x0400A81B RID: 43035
		private static readonly string[] EyelidShapesAlwaysCleared = new string[]
		{
			"eye_close_smile_L", "eye_close_smile_R", "eye_close_nagomi_L", "eye_close_nagomi_R", "eye_close_nagomi2_L", "eye_close_nagomi2_R", "eye_surprise_L", "eye_surprise_R", "eye_sad_L", "eye_sad_R",
			"eye_angry_L", "eye_angry_R", "eye_tare_L", "eye_tare_R", "eye_><", "eye_under_down_L", "eye_under_down_R", "eye_under_down2_L", "eye_under_down2_R", "eye_under_up2_L",
			"eye_under_up2_R", "eye_under_up3_L", "eye_under_up3_R"
		};

		// Token: 0x0400A81C RID: 43036
		private static readonly string[] EyeCloseShapesForOpenGropingEyes = new string[]
		{
			"eye_close_smile_L", "eye_close_smile_R", "eye_close_nagomi_L", "eye_close_nagomi_R", "eye_close_nagomi2_L", "eye_close_nagomi2_R", "eye_close_sleep_L", "eye_close_sleep_R", "eye_><", "vrc.blink",
			"blink"
		};

		// Token: 0x0400A81D RID: 43037
		[Header("HUD azioni in modalita' (pannello destro condiviso)")]
		[Tooltip("Label inglese dell'azione pressione mostrata nell'InteractionHUDController.")]
		[SerializeField]
		private string pressureActionLabel = "Apply Pressure (Hold)";

		// Token: 0x0400A81E RID: 43038
		[Tooltip("Keybind mostrato per la pressione.")]
		[SerializeField]
		private string pressureKeyLabel = "LMB";

		// Token: 0x0400A81F RID: 43039
		[Tooltip("Label inglese dell'azione di uscita.")]
		[SerializeField]
		private string exitActionLabel = "Exit";

		// Token: 0x0400A820 RID: 43040
		[Tooltip("Keybind mostrato per l'uscita.")]
		[SerializeField]
		private string exitKeyLabel = "ESC";

		// Token: 0x0400A821 RID: 43041
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400A822 RID: 43042
		[SerializeField]
		[HideInInspector]
		private int setupRevision;

		// Token: 0x0400A823 RID: 43043
		private BodyPaintOrbitCamera _orbitCamera;

		// Token: 0x0400A824 RID: 43044
		private vThirdPersonInput _playerInput;

		// Token: 0x0400A825 RID: 43045
		private Animator _animator;

		// Token: 0x0400A826 RID: 43046
		private Transform _playerRoot;

		// Token: 0x0400A827 RID: 43047
		private FutanariModeController _futanariMode;

		// Token: 0x0400A828 RID: 43048
		private Camera _mainCamera;

		// Token: 0x0400A829 RID: 43049
		private PlayerActionAnimator _actionAnimator;

		// Token: 0x0400A82A RID: 43050
		private PlayerEmotionController _emotionController;

		// Token: 0x0400A82B RID: 43051
		private CielWetController _wetController;

		// Token: 0x0400A82C RID: 43052
		private Animator _faceAnimator;

		// Token: 0x0400A82D RID: 43053
		private string _faceParamName = "FaceExpressionIndex";

		// Token: 0x0400A82E RID: 43054
		private int _savedFaceIndex;

		// Token: 0x0400A82F RID: 43055
		private int _modeFaceIndex;

		// Token: 0x0400A830 RID: 43056
		private int _modeFaceOverrideIndex = -1;

		// Token: 0x0400A831 RID: 43057
		private bool _faceOverridden;

		// Token: 0x0400A832 RID: 43058
		private bool _faceOverrideWasAlreadyHeld;

		// Token: 0x0400A833 RID: 43059
		private bool _modeEyesOpenOverride;

		// Token: 0x0400A834 RID: 43060
		private bool _wetHeld;

		// Token: 0x0400A835 RID: 43061
		private readonly List<BoobTouchModeController.EyeBlendShapeOverride> _eyeBlendShapeOverrides = new List<BoobTouchModeController.EyeBlendShapeOverride>();

		// Token: 0x0400A836 RID: 43062
		private readonly List<BoobTouchModeController.ClothBackup> _cloths = new List<BoobTouchModeController.ClothBackup>();

		// Token: 0x0400A837 RID: 43063
		private readonly List<BoobTouchModeController.BreastTipVisualState> _breastTips = new List<BoobTouchModeController.BreastTipVisualState>();

		// Token: 0x0400A838 RID: 43064
		private Coroutine _clothRoutine;

		// Token: 0x0400A839 RID: 43065
		private bool _transitioning;

		// Token: 0x0400A83A RID: 43066
		private readonly List<MagicaCloth> _hairCloths = new List<MagicaCloth>();

		// Token: 0x0400A83B RID: 43067
		private GameObject _ballRoot;

		// Token: 0x0400A83C RID: 43068
		private Transform _ballVisual;

		// Token: 0x0400A83D RID: 43069
		private Transform _pressureColliderTransform;

		// Token: 0x0400A83E RID: 43070
		private MagicaSphereCollider _ballCollider;

		// Token: 0x0400A83F RID: 43071
		private MagicaSphereCollider _hairBallCollider;

		// Token: 0x0400A840 RID: 43072
		private Transform _chestAnchor;

		// Token: 0x0400A841 RID: 43073
		private bool _usingSoap;

		// Token: 0x0400A842 RID: 43074
		private float _colliderRadius;

		// Token: 0x0400A843 RID: 43075
		private Rigidbody _soapRb;

		// Token: 0x0400A844 RID: 43076
		private float _soapHalfThickness = 0.015f;

		// Token: 0x0400A845 RID: 43077
		private Vector3 _surfaceOffset;

		// Token: 0x0400A846 RID: 43078
		private GameObject _soapSurfaceSkinGO;

		// Token: 0x0400A847 RID: 43079
		private SkinnedMeshRenderer _soapSurfaceSourceRenderer;

		// Token: 0x0400A848 RID: 43080
		private SkinnedMeshRenderer _soapSurfaceRenderer;

		// Token: 0x0400A849 RID: 43081
		private GameObject _soapSurfaceColliderGO;

		// Token: 0x0400A84A RID: 43082
		private MeshCollider _soapSurfaceCollider;

		// Token: 0x0400A84B RID: 43083
		private Mesh _soapSurfaceBakedMesh;

		// Token: 0x0400A84C RID: 43084
		private int[] _soapSurfaceTriangles;

		// Token: 0x0400A84D RID: 43085
		private int[] _soapBlendSourceIndices;

		// Token: 0x0400A84E RID: 43086
		private readonly List<Vector3> _soapSurfaceVertices = new List<Vector3>();

		// Token: 0x0400A84F RID: 43087
		private readonly List<Vector3> _soapSurfaceNormals = new List<Vector3>();

		// Token: 0x0400A850 RID: 43088
		private BoobsController _boobsController;

		// Token: 0x0400A851 RID: 43089
		private BreastInteractionSizeProfile _breastSizeProfile = BreastInteractionSizing.Resolve(3);

		// Token: 0x0400A852 RID: 43090
		private Vector3 _physTargetPos;

		// Token: 0x0400A853 RID: 43091
		private Quaternion _physTargetRot = Quaternion.identity;

		// Token: 0x0400A854 RID: 43092
		private bool _physTargetValid;

		// Token: 0x0400A855 RID: 43093
		private Vector3 _smoothedTargetPos;

		// Token: 0x0400A856 RID: 43094
		private Vector3 _smoothedSurfaceNormal;

		// Token: 0x0400A857 RID: 43095
		private Vector3 _surfaceForward;

		// Token: 0x0400A858 RID: 43096
		private bool _hasSmoothedTarget;

		// Token: 0x0400A859 RID: 43097
		private bool _hasSurfaceFrame;

		// Token: 0x0400A85A RID: 43098
		private float _pressAmount;

		// Token: 0x0400A85B RID: 43099
		private float _debugPressUntil = -1f;

		// Token: 0x0400A85C RID: 43100
		private bool _touchingBody;

		// Token: 0x0400A85D RID: 43101
		private Vector3 _contactPoint;

		// Token: 0x0400A85E RID: 43102
		private ParticleSystem _foam;

		// Token: 0x0400A85F RID: 43103
		private Vector3 _lastSoapLocalPos;

		// Token: 0x0400A860 RID: 43104
		private bool _hasLastSoapLocalPos;

		// Token: 0x0400A861 RID: 43105
		private float _foamTravelSinceEmit;

		// Token: 0x0400A862 RID: 43106
		private BodyPaintCanvasService _canvasService;

		// Token: 0x0400A863 RID: 43107
		private bool _surfaceReady;

		// Token: 0x0400A864 RID: 43108
		private bool _surfaceBakeSubscribed;

		// Token: 0x0400A865 RID: 43109
		private float _lastBakeTime;

		// Token: 0x0400A866 RID: 43110
		private Vector3 _lastSurfaceLocalPos;

		// Token: 0x0400A867 RID: 43111
		private Vector3 _lastSurfaceLocalNormal;

		// Token: 0x0400A868 RID: 43112
		private bool _hasSurfacePoint;

		// Token: 0x0400A869 RID: 43113
		private Vector3 _prevTargetLocal;

		// Token: 0x0400A86A RID: 43114
		private bool _hasPrevTarget;

		// Token: 0x0400A86B RID: 43115
		private Vector2 _prevMouse;

		// Token: 0x0400A86C RID: 43116
		private bool _active;

		// Token: 0x0400A86D RID: 43117
		private bool _prevLockInput;

		// Token: 0x0400A86E RID: 43118
		private bool _prevLockCamera;

		// Token: 0x0400A86F RID: 43119
		private PlayerActivityLease _modeLease;

		// Token: 0x0400A870 RID: 43120
		private readonly List<Canvas> _disabledCanvases = new List<Canvas>();

		// Token: 0x0400A871 RID: 43121
		private readonly List<GameObject> _hiddenHudSiblings = new List<GameObject>();

		// Token: 0x0400A872 RID: 43122
		private InteractionConfigSO _modeControlsConfig;

		// Token: 0x0400A873 RID: 43123
		private bool _modeControlsThirdPoseAvailable;

		// Token: 0x0400A874 RID: 43124
		private bool _externalInteractorMode;

		// Token: 0x0400A875 RID: 43125
		private readonly List<ColliderComponent> _externalBodyColliders = new List<ColliderComponent>();

		// Token: 0x0400A876 RID: 43126
		private readonly List<ColliderComponent> _externalHairColliders = new List<ColliderComponent>();

		// Token: 0x0400A877 RID: 43127
		private readonly Dictionary<int, BoobTouchModeController.ExternalBreastGrabState> _externalBreastGrabs = new Dictionary<int, BoobTouchModeController.ExternalBreastGrabState>();

		// Token: 0x0400A878 RID: 43128
		private readonly List<int> _staleExternalGrabOwners = new List<int>(2);

		// Token: 0x0400A879 RID: 43129
		private float _externalBreastTouch;

		// Token: 0x0400A87A RID: 43130
		private int _externalBreastTouchFrame = -10;

		// Token: 0x0400A87C RID: 43132
		private FirstPersonController _interactionFirstPerson;

		// Token: 0x0400A87D RID: 43133
		private Coroutine _autoPushRoutine;

		// Token: 0x020014D4 RID: 5332
		private class EyeBlendShapeOverride
		{
			// Token: 0x0400A87E RID: 43134
			public SkinnedMeshRenderer renderer;

			// Token: 0x0400A87F RID: 43135
			public int index;

			// Token: 0x0400A880 RID: 43136
			public float savedWeight;

			// Token: 0x0400A881 RID: 43137
			public float targetWeight;
		}

		// Token: 0x020014D5 RID: 5333
		private class ClothBackup
		{
			// Token: 0x0400A882 RID: 43138
			public MagicaCloth cloth;

			// Token: 0x0400A883 RID: 43139
			public GameObject go;

			// Token: 0x0400A884 RID: 43140
			public ClothSerializeData originalData;

			// Token: 0x0400A885 RID: 43141
			public ClothSerializeData2 originalData2;

			// Token: 0x0400A886 RID: 43142
			public bool isButt;
		}

		// Token: 0x020014D6 RID: 5334
		private class BreastTipVisualState
		{
			// Token: 0x0400A887 RID: 43143
			public Transform renderBone01;

			// Token: 0x0400A888 RID: 43144
			public Transform supportBone02;

			// Token: 0x0400A889 RID: 43145
			public Vector3 preRenderPosition;

			// Token: 0x0400A88A RID: 43146
			public Vector3 preSupportPosition;

			// Token: 0x0400A88B RID: 43147
			public int preFrame = -1;

			// Token: 0x0400A88C RID: 43148
			public Vector3 filteredTiltVector;

			// Token: 0x0400A88D RID: 43149
			public Quaternion lastUntiltedRotation = Quaternion.identity;

			// Token: 0x0400A88E RID: 43150
			public Vector3 lastUntiltedPosition;

			// Token: 0x0400A88F RID: 43151
			public bool hasUntiltedRotation;

			// Token: 0x0400A890 RID: 43152
			public float lastAppliedAngle;

			// Token: 0x0400A891 RID: 43153
			public float lastProximity;

			// Token: 0x0400A892 RID: 43154
			public float lastMarkerDisplacement;

			// Token: 0x0400A893 RID: 43155
			public int surfaceTriangleIndex = -1;

			// Token: 0x0400A894 RID: 43156
			public Vector3 surfaceBarycentric;

			// Token: 0x0400A895 RID: 43157
			public bool hasSurfaceAnchor;
		}

		// Token: 0x020014D7 RID: 5335
		public enum ExternalGrabKind
		{
			// Token: 0x0400A897 RID: 43159
			Breast = 1,
			// Token: 0x0400A898 RID: 43160
			Nipple
		}

		// Token: 0x020014D8 RID: 5336
		private sealed class ExternalBreastGrabState
		{
			// Token: 0x0400A899 RID: 43161
			public int breastIndex;

			// Token: 0x0400A89A RID: 43162
			public BoobTouchModeController.ExternalGrabKind kind;

			// Token: 0x0400A89B RID: 43163
			public Vector3 handPointWorld;

			// Token: 0x0400A89C RID: 43164
			public float strength;

			// Token: 0x0400A89D RID: 43165
			public int updatedFrame;

			// Token: 0x0400A89E RID: 43166
			public bool hasAnchorOffset;

			// Token: 0x0400A89F RID: 43167
			public Vector3 anchorOffset;

			// Token: 0x0400A8A0 RID: 43168
			public Vector3 filteredDelta;
		}
	}
}
