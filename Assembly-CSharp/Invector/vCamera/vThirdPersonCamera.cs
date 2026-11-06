using System;
using System.Collections;
using System.Collections.Generic;
using Features.MainMenu;
using Invector.vCharacterController;
using UnityEngine;

namespace Invector.vCamera
{
	// Token: 0x02000636 RID: 1590
	public class vThirdPersonCamera : MonoBehaviour
	{
		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x000CC16B File Offset: 0x000CA36B
		public static vThirdPersonCamera instance
		{
			get
			{
				if (vThirdPersonCamera._instance == null)
				{
					vThirdPersonCamera._instance = Object.FindObjectOfType<vThirdPersonCamera>();
				}
				return vThirdPersonCamera._instance;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002458 RID: 9304 RVA: 0x000CC189 File Offset: 0x000CA389
		// (set) Token: 0x06002459 RID: 9305 RVA: 0x000CC191 File Offset: 0x000CA391
		public virtual float smoothBetweenState
		{
			get
			{
				return this._smoothBetweenState;
			}
			set
			{
				this._smoothBetweenState = value;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x000CC19A File Offset: 0x000CA39A
		// (set) Token: 0x0600245B RID: 9307 RVA: 0x000CC1A2 File Offset: 0x000CA3A2
		public virtual float smoothCameraRotation
		{
			get
			{
				return this._smoothCameraRotation;
			}
			set
			{
				this._smoothCameraRotation = value;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x000CC1AB File Offset: 0x000CA3AB
		// (set) Token: 0x0600245D RID: 9309 RVA: 0x000CC1B3 File Offset: 0x000CA3B3
		public virtual float smoothSwitchSide
		{
			get
			{
				return this._smoothSwitchSide;
			}
			set
			{
				this._smoothSwitchSide = value;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x000CC1BC File Offset: 0x000CA3BC
		// (set) Token: 0x0600245F RID: 9311 RVA: 0x000CC1C4 File Offset: 0x000CA3C4
		public virtual float scrollSpeed
		{
			get
			{
				return this._scrollSpeed;
			}
			set
			{
				this._scrollSpeed = value;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x000CC1CD File Offset: 0x000CA3CD
		// (set) Token: 0x06002461 RID: 9313 RVA: 0x000CC1D5 File Offset: 0x000CA3D5
		public virtual float joystickSensitivity
		{
			get
			{
				return this._joystickSensitivity;
			}
			set
			{
				this._joystickSensitivity = value;
			}
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000CC1E0 File Offset: 0x000CA3E0
		public void SetFollowHeightOffset(Object owner, Transform target, float offset)
		{
			if (owner == null || target == null || float.IsNaN(offset) || float.IsInfinity(offset))
			{
				return;
			}
			if (this.followHeightOwner != null && this.followHeightOwner != owner)
			{
				return;
			}
			this.followHeightOwner = owner;
			this.followHeightTarget = target;
			this.followHeightOffset = offset;
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000CC242 File Offset: 0x000CA442
		public void ClearFollowHeightOffset(Object owner)
		{
			if (this.followHeightOwner != owner)
			{
				return;
			}
			this.followHeightOwner = null;
			this.followHeightTarget = null;
			this.followHeightOffset = 0f;
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002464 RID: 9316 RVA: 0x000CC26C File Offset: 0x000CA46C
		private float FollowHeightOffset
		{
			get
			{
				if (!(this.followHeightOwner != null) || !(this.currentTarget == this.followHeightTarget))
				{
					return 0f;
				}
				return this.followHeightOffset;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x000CC29B File Offset: 0x000CA49B
		// (set) Token: 0x06002466 RID: 9318 RVA: 0x000CC2A3 File Offset: 0x000CA4A3
		internal virtual float mouseY
		{
			get
			{
				return this._mouseY;
			}
			set
			{
				this._mouseY = value;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x000CC2AC File Offset: 0x000CA4AC
		// (set) Token: 0x06002468 RID: 9320 RVA: 0x000CC2B4 File Offset: 0x000CA4B4
		internal virtual float mouseX
		{
			get
			{
				return this._mouseX;
			}
			set
			{
				this._mouseX = value;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002469 RID: 9321 RVA: 0x000CC2BD File Offset: 0x000CA4BD
		// (set) Token: 0x0600246A RID: 9322 RVA: 0x000CC2C5 File Offset: 0x000CA4C5
		protected virtual float currentHeight { get; set; }

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x0600246B RID: 9323 RVA: 0x000CC2CE File Offset: 0x000CA4CE
		// (set) Token: 0x0600246C RID: 9324 RVA: 0x000CC2D6 File Offset: 0x000CA4D6
		protected virtual float currentZoom { get; set; }

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x0600246D RID: 9325 RVA: 0x000CC2DF File Offset: 0x000CA4DF
		// (set) Token: 0x0600246E RID: 9326 RVA: 0x000CC2E7 File Offset: 0x000CA4E7
		protected virtual float cullingHeight { get; set; }

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x000CC2F0 File Offset: 0x000CA4F0
		// (set) Token: 0x06002470 RID: 9328 RVA: 0x000CC2F8 File Offset: 0x000CA4F8
		protected virtual float cullingDistance { get; set; }

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x000CC301 File Offset: 0x000CA501
		// (set) Token: 0x06002472 RID: 9330 RVA: 0x000CC309 File Offset: 0x000CA509
		internal virtual float switchRight
		{
			get
			{
				return this._switchRight;
			}
			set
			{
				this._switchRight = value;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002473 RID: 9331 RVA: 0x000CC312 File Offset: 0x000CA512
		// (set) Token: 0x06002474 RID: 9332 RVA: 0x000CC31A File Offset: 0x000CA51A
		protected virtual float currentSwitchRight { get; set; }

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x000CC323 File Offset: 0x000CA523
		// (set) Token: 0x06002476 RID: 9334 RVA: 0x000CC32B File Offset: 0x000CA52B
		protected virtual float heightOffset { get; set; }

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002477 RID: 9335 RVA: 0x000CC334 File Offset: 0x000CA534
		// (set) Token: 0x06002478 RID: 9336 RVA: 0x000CC33C File Offset: 0x000CA53C
		public virtual bool isInit { get; set; }

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x000CC345 File Offset: 0x000CA545
		// (set) Token: 0x0600247A RID: 9338 RVA: 0x000CC34D File Offset: 0x000CA54D
		public virtual bool useSmooth { get; set; }

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x0600247B RID: 9339 RVA: 0x000CC356 File Offset: 0x000CA556
		// (set) Token: 0x0600247C RID: 9340 RVA: 0x000CC35E File Offset: 0x000CA55E
		protected virtual bool isNewTarget { get; set; }

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600247D RID: 9341 RVA: 0x000CC367 File Offset: 0x000CA567
		// (set) Token: 0x0600247E RID: 9342 RVA: 0x000CC36F File Offset: 0x000CA56F
		protected virtual bool firstStateIsInit { get; set; }

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x0600247F RID: 9343 RVA: 0x000CC378 File Offset: 0x000CA578
		// (set) Token: 0x06002480 RID: 9344 RVA: 0x000CC380 File Offset: 0x000CA580
		protected virtual bool firstUpdated { get; set; }

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002481 RID: 9345 RVA: 0x000CC389 File Offset: 0x000CA589
		// (set) Token: 0x06002482 RID: 9346 RVA: 0x000CC391 File Offset: 0x000CA591
		protected virtual float mouseXStart { get; set; }

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002483 RID: 9347 RVA: 0x000CC39A File Offset: 0x000CA59A
		// (set) Token: 0x06002484 RID: 9348 RVA: 0x000CC3A2 File Offset: 0x000CA5A2
		protected virtual float mouseYStart { get; set; }

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002485 RID: 9349 RVA: 0x000CC3AB File Offset: 0x000CA5AB
		// (set) Token: 0x06002486 RID: 9350 RVA: 0x000CC3B3 File Offset: 0x000CA5B3
		protected virtual Vector3 startPosition { get; set; }

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06002487 RID: 9351 RVA: 0x000CC3BC File Offset: 0x000CA5BC
		// (set) Token: 0x06002488 RID: 9352 RVA: 0x000CC3C4 File Offset: 0x000CA5C4
		protected virtual Quaternion startRotation { get; set; }

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002489 RID: 9353 RVA: 0x000CC3D0 File Offset: 0x000CA5D0
		protected Transform targetLookAt
		{
			get
			{
				if (!this._lookAtTarget)
				{
					this._lookAtTarget = new GameObject("targetLookAt").transform;
					this._lookAtTarget.rotation = base.transform.rotation;
					this._lookAtTarget.position = this.mainTarget.position;
				}
				return this._lookAtTarget;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x0600248A RID: 9354 RVA: 0x000CC431 File Offset: 0x000CA631
		public Rigidbody selfRigidbody
		{
			get
			{
				if (!this._selfRigidbody)
				{
					this._selfRigidbody = base.gameObject.AddComponent<Rigidbody>();
					this._selfRigidbody.isKinematic = true;
					this._selfRigidbody.interpolation = RigidbodyInterpolation.None;
				}
				return this._selfRigidbody;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x0600248B RID: 9355 RVA: 0x000CC46F File Offset: 0x000CA66F
		// (set) Token: 0x0600248C RID: 9356 RVA: 0x000CC477 File Offset: 0x000CA677
		public bool LockCamera
		{
			get
			{
				return this.lockCamera;
			}
			set
			{
				this.lockCamera = value;
			}
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x000CC480 File Offset: 0x000CA680
		protected virtual void OnDrawGizmos()
		{
			if (this.showGizmos && this.currentTarget)
			{
				Vector3 vector = new Vector3(this.currentTarget.position.x, this.currentTarget.position.y + this.offSetPlayerPivot, this.currentTarget.position.z);
				Gizmos.DrawWireSphere(vector + Vector3.up * this.cullingHeight, this.checkHeightRadius);
				Gizmos.DrawLine(vector, vector + Vector3.up * this.cullingHeight);
			}
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x000CC51D File Offset: 0x000CA71D
		protected virtual void Start()
		{
			this.Init();
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000CC528 File Offset: 0x000CA728
		public virtual void Init()
		{
			if (this.mainTarget == null)
			{
				return;
			}
			this._targetPositionsAreLocal = false;
			this.firstUpdated = true;
			this.useSmooth = true;
			this.targetLookAt.rotation = (this.startUsingTargetRotation ? this.mainTarget.rotation : base.transform.rotation);
			this.targetLookAt.position = this.mainTarget.position;
			this.targetLookAt.hideFlags = HideFlags.HideInHierarchy;
			this.startPosition = this.selfRigidbody.position;
			this.startRotation = this.selfRigidbody.rotation;
			this.initialCameraRotation = this.smoothCameraRotation;
			if (!this.targetCamera)
			{
				this.targetCamera = Camera.main;
			}
			this.currentTarget = this.mainTarget;
			this.switchRight = 1f;
			this.currentSwitchRight = 1f;
			this.mouseXStart = base.transform.eulerAngles.NormalizeAngle().y;
			this.mouseYStart = base.transform.eulerAngles.NormalizeAngle().x;
			if (this.startSmooth)
			{
				this.distance = Vector3.Distance(this.targetLookAt.position, base.transform.position);
			}
			else
			{
				this.transformWeight = 1f;
			}
			if (this.startUsingTargetRotation)
			{
				this.mouseY = this.currentTarget.eulerAngles.NormalizeAngle().x;
				this.mouseX = this.currentTarget.eulerAngles.NormalizeAngle().y;
			}
			else
			{
				this.mouseY = base.transform.eulerAngles.NormalizeAngle().x;
				this.mouseX = base.transform.eulerAngles.NormalizeAngle().y;
			}
			this.ChangeState("Default", this.startSmooth);
			this.currentZoom = this.currentState.defaultDistance;
			this.currentHeight = this.currentState.height;
			this.currentTargetPos = new Vector3(this.currentTarget.position.x, this.currentTarget.position.y + this.offSetPlayerPivot, this.currentTarget.position.z) + this.currentTarget.transform.up * this.lerpState.height;
			this.targetLookAt.position = this.currentTargetPos;
			this.isInit = true;
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000CC7A4 File Offset: 0x000CA9A4
		public void ChangeStateList(vThirdPersonCameraListData list)
		{
			if (this.CameraStateList != list)
			{
				string stateName = this.lerpState.Name;
				this.CameraStateList = list;
				vThirdPersonCameraState vThirdPersonCameraState = this.CameraStateList.tpCameraStates.Find((vThirdPersonCameraState state) => state.Name == stateName);
				this.RestoreZoomForState(vThirdPersonCameraState);
				this.lerpState = vThirdPersonCameraState;
			}
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x000CC808 File Offset: 0x000CAA08
		public virtual void FixedUpdate()
		{
			if (this.mainTarget == null || this.targetLookAt == null || this.currentState == null || this.lerpState == null || !this.isInit || this.isFreezed)
			{
				return;
			}
			switch (this.currentState.cameraMode)
			{
			case TPCameraMode.FreeDirectional:
				this.CameraMovement(false);
				return;
			case TPCameraMode.FixedAngle:
				this.CameraMovement(false);
				return;
			case TPCameraMode.FixedPoint:
				this.CameraFixed();
				return;
			default:
				return;
			}
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000CC888 File Offset: 0x000CAA88
		public virtual void SetLockTarget(Transform lockTarget)
		{
			if (this.lockTarget != null && this.lockTarget == lockTarget)
			{
				return;
			}
			this.isNewTarget = lockTarget != this.lockTarget;
			this.lockTarget = lockTarget;
			this.lockTargetWeight = 0f;
			this.lockTargetSpeed = 1f;
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000CC8E4 File Offset: 0x000CAAE4
		public virtual void SetLockTarget(Transform lockTarget, float heightOffset, float lockSpeed = 1f)
		{
			if (this.lockTarget != null && this.lockTarget == lockTarget)
			{
				return;
			}
			this.isNewTarget = lockTarget != this.lockTarget;
			this.lockTarget = lockTarget;
			this.heightOffset = heightOffset;
			this.lockTargetWeight = 0f;
			this.lockTargetSpeed = lockSpeed;
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x000CC940 File Offset: 0x000CAB40
		public virtual void RemoveLockTarget()
		{
			this.lockTargetWeight = 0f;
			this.lockTarget = null;
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000CC954 File Offset: 0x000CAB54
		public virtual void SetTarget(Transform newTarget)
		{
			this.lockTargetWeight = 0f;
			this.currentTarget = (newTarget ? newTarget : this.mainTarget);
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x000CC978 File Offset: 0x000CAB78
		public virtual void SetMainTarget(Transform newTarget)
		{
			this.mainTarget = newTarget;
			this.currentTarget = newTarget;
			if (!this.isInit)
			{
				this.Init();
			}
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000CC996 File Offset: 0x000CAB96
		public virtual void ResetTarget()
		{
			if (this.currentTarget != this.mainTarget)
			{
				this.currentTarget = this.mainTarget;
				if (!this.isInit)
				{
					this.Init();
				}
			}
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x000CC9C8 File Offset: 0x000CABC8
		public virtual void ResetAngle()
		{
			if (this.currentTarget)
			{
				this.mouseY = this.currentTarget.eulerAngles.NormalizeAngle().x;
				this.mouseX = this.currentTarget.eulerAngles.NormalizeAngle().y;
				return;
			}
			this.mouseY = 0f;
			this.mouseX = 0f;
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x000CCA2F File Offset: 0x000CAC2F
		public virtual void ResetAngleWithoutSmooth()
		{
			this.ResetAngle();
			this.targetLookAt.forward = this.currentTarget.forward;
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x000CCA4D File Offset: 0x000CAC4D
		public virtual Ray ScreenPointToRay(Vector3 Point)
		{
			return base.GetComponent<Camera>().ScreenPointToRay(Point);
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x000CCA5B File Offset: 0x000CAC5B
		public virtual void ChangeState(string stateName)
		{
			this.ChangeState(stateName, true);
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000CCA68 File Offset: 0x000CAC68
		public virtual void ChangeState(string stateName, bool hasSmooth)
		{
			vThirdPersonCameraState vThirdPersonCameraState = (this.isInit ? (this.lerpState ?? this.currentState) : this.currentState);
			if ((vThirdPersonCameraState != null && vThirdPersonCameraState.Name.Equals(stateName)) || (!this.isInit && this.firstStateIsInit))
			{
				if (this.firstStateIsInit)
				{
					this.useSmooth = hasSmooth;
				}
				return;
			}
			this.useSmooth = ((!this.firstStateIsInit) ? this.startSmooth : hasSmooth);
			vThirdPersonCameraState vThirdPersonCameraState2 = ((this.CameraStateList != null) ? this.CameraStateList.tpCameraStates.Find((vThirdPersonCameraState obj) => obj.Name.Equals(stateName)) : new vThirdPersonCameraState("Default"));
			if (vThirdPersonCameraState2 != null)
			{
				this.RestoreZoomForState(vThirdPersonCameraState2);
				this.currentStateName = stateName;
				this.currentState.cameraMode = vThirdPersonCameraState2.cameraMode;
				this.lerpState = vThirdPersonCameraState2;
				if (!this.firstStateIsInit)
				{
					this.currentState.defaultDistance = Vector3.Distance(this.targetLookAt.position, base.transform.position);
					this.currentState.forward = this.lerpState.forward;
					this.currentState.height = vThirdPersonCameraState2.height;
					this.currentState.fov = vThirdPersonCameraState2.fov;
					if (this.useSmooth)
					{
						base.StartCoroutine(this.ResetFirstState());
					}
					else
					{
						this.distance = this.lerpState.defaultDistance;
						this.firstStateIsInit = true;
					}
				}
				if (this.currentState != null && !this.useSmooth)
				{
					this.currentState.CopyState(vThirdPersonCameraState2);
				}
			}
			else if (this.CameraStateList != null && this.CameraStateList.tpCameraStates.Count > 0)
			{
				if (this.lerpState != null)
				{
					return;
				}
				vThirdPersonCameraState2 = this.CameraStateList.tpCameraStates[0];
				this.RestoreZoomForState(vThirdPersonCameraState2);
				this.currentStateName = vThirdPersonCameraState2.Name;
				this.currentState.cameraMode = vThirdPersonCameraState2.cameraMode;
				this.lerpState = vThirdPersonCameraState2;
				if (this.currentState != null && !this.useSmooth)
				{
					this.currentState.CopyState(vThirdPersonCameraState2);
				}
			}
			if (this.currentState == null)
			{
				this.currentState = new vThirdPersonCameraState("Null");
				this.currentStateName = this.currentState.Name;
			}
			if (this.CameraStateList != null)
			{
				this.indexList = this.CameraStateList.tpCameraStates.IndexOf(vThirdPersonCameraState2);
			}
			if (this.currentState.cameraMode == TPCameraMode.FixedAngle)
			{
				this.mouseX = this.currentState.fixedAngle.x;
				this.mouseY = this.currentState.fixedAngle.y;
			}
			this.currentState.fixedAngle = new Vector3(this.mouseX, this.mouseY);
			this.indexLookPoint = 0;
			if (!this.isInit)
			{
				this.CameraMovement(true);
			}
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x000CCD5C File Offset: 0x000CAF5C
		public virtual void ChangeState(string stateName, string pointName, bool hasSmooth)
		{
			this.useSmooth = hasSmooth;
			vThirdPersonCameraState vThirdPersonCameraState = (this.isInit ? (this.lerpState ?? this.currentState) : this.currentState);
			if (vThirdPersonCameraState == null || !vThirdPersonCameraState.Name.Equals(stateName))
			{
				vThirdPersonCameraState vThirdPersonCameraState2 = this.CameraStateList.tpCameraStates.Find((vThirdPersonCameraState obj) => obj.Name.Equals(stateName));
				if (vThirdPersonCameraState2 != null)
				{
					this.RestoreZoomForState(vThirdPersonCameraState2);
					this.currentStateName = stateName;
					this.currentState.cameraMode = vThirdPersonCameraState2.cameraMode;
					this.lerpState = vThirdPersonCameraState2;
					if (this.currentState != null && !hasSmooth)
					{
						this.currentState.CopyState(vThirdPersonCameraState2);
					}
				}
				else if (this.CameraStateList.tpCameraStates.Count > 0)
				{
					vThirdPersonCameraState2 = this.CameraStateList.tpCameraStates[0];
					this.RestoreZoomForState(vThirdPersonCameraState2);
					this.currentStateName = vThirdPersonCameraState2.Name;
					this.currentState.cameraMode = vThirdPersonCameraState2.cameraMode;
					this.lerpState = vThirdPersonCameraState2;
					if (this.currentState != null && !hasSmooth)
					{
						this.currentState.CopyState(vThirdPersonCameraState2);
					}
				}
				if (this.currentState == null)
				{
					this.currentState = new vThirdPersonCameraState("Null");
					this.currentStateName = this.currentState.Name;
				}
				this.indexList = this.CameraStateList.tpCameraStates.IndexOf(vThirdPersonCameraState2);
				this.currentState.fixedAngle = new Vector3(this.mouseX, this.mouseY);
				this.indexLookPoint = 0;
			}
			if (this.currentState.cameraMode == TPCameraMode.FixedPoint)
			{
				LookPoint lookPoint = this.currentState.lookPoints.Find((LookPoint obj) => obj.pointName.Equals(pointName));
				if (lookPoint != null)
				{
					this.indexLookPoint = this.currentState.lookPoints.IndexOf(lookPoint);
					return;
				}
				this.indexLookPoint = 0;
			}
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x000CCF40 File Offset: 0x000CB140
		private void RestoreZoomForState(vThirdPersonCameraState nextState)
		{
			if (nextState == null || nextState == this.lerpState)
			{
				return;
			}
			if (this.isInit && this.lerpState != null && this.lerpState.useZoom)
			{
				this._stateZoom[this.lerpState] = Mathf.Clamp(this.currentZoom, this.lerpState.minDistance, this.lerpState.maxDistance);
			}
			float num;
			this.currentZoom = ((nextState.useZoom && this._stateZoom.TryGetValue(nextState, out num)) ? Mathf.Clamp(num, nextState.minDistance, nextState.maxDistance) : nextState.defaultDistance);
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000CCFE1 File Offset: 0x000CB1E1
		protected virtual IEnumerator ResetFirstState()
		{
			yield return new WaitForEndOfFrame();
			this.firstStateIsInit = true;
			yield break;
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x000CCFF0 File Offset: 0x000CB1F0
		public virtual void ChangePoint(string pointName)
		{
			if (this.currentState == null || this.currentState.cameraMode != TPCameraMode.FixedPoint || this.currentState.lookPoints == null)
			{
				return;
			}
			LookPoint lookPoint = this.currentState.lookPoints.Find((LookPoint obj) => obj.pointName.Equals(pointName));
			if (lookPoint != null)
			{
				this.indexLookPoint = this.currentState.lookPoints.IndexOf(lookPoint);
				return;
			}
			this.indexLookPoint = 0;
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000CD070 File Offset: 0x000CB270
		public virtual void FreezeCamera()
		{
			this.isFreezed = true;
			if (this.mainTarget && !this._targetPositionsAreLocal)
			{
				this.lastLookAtForward = this.mainTarget.InverseTransformDirection(this.targetLookAt.forward);
				this.lastLookAtPosition = this.mainTarget.InverseTransformPoint(this.targetLookAt.position);
				this.current_cPos = this.mainTarget.InverseTransformPoint(this.current_cPos);
				this.desired_cPos = this.mainTarget.InverseTransformPoint(this.desired_cPos);
				this._targetPositionsAreLocal = true;
			}
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000CD108 File Offset: 0x000CB308
		public virtual void UnFreezeCamera()
		{
			if (this.mainTarget && this._targetPositionsAreLocal)
			{
				this.targetLookAt.forward = this.mainTarget.TransformDirection(this.lastLookAtForward);
				this.targetLookAt.position = this.mainTarget.TransformPoint(this.lastLookAtPosition);
				this.current_cPos = this.mainTarget.TransformPoint(this.current_cPos);
				this.desired_cPos = this.mainTarget.TransformPoint(this.desired_cPos);
				this._targetPositionsAreLocal = false;
			}
			this.isFreezed = false;
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000CD1A0 File Offset: 0x000CB3A0
		public virtual void ApplyWorldOriginShift(Vector3 delta)
		{
			this.currentTargetPos += delta;
			this.lookPoint += delta;
			this.startPosition += delta;
			this._fixedPointWorldOffset += delta;
			if (!this._targetPositionsAreLocal)
			{
				this.current_cPos += delta;
				this.desired_cPos += delta;
			}
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000CD224 File Offset: 0x000CB424
		public virtual void Zoom(float scroolValue)
		{
			vThirdPersonCameraState vThirdPersonCameraState = this.lerpState ?? this.currentState;
			if (vThirdPersonCameraState == null || !vThirdPersonCameraState.useZoom)
			{
				return;
			}
			this.currentZoom = Mathf.Clamp(this.currentZoom - scroolValue * this.scrollSpeed, vThirdPersonCameraState.minDistance, vThirdPersonCameraState.maxDistance);
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000CD274 File Offset: 0x000CB474
		public virtual void SetZoomWithoutSmooth(float value)
		{
			vThirdPersonCameraState vThirdPersonCameraState = this.lerpState ?? this.currentState;
			this.currentZoom = ((vThirdPersonCameraState != null) ? Mathf.Clamp(value, vThirdPersonCameraState.minDistance, vThirdPersonCameraState.maxDistance) : Mathf.Max(0f, value));
			this.distance = this.currentZoom;
			this.cullingDistance = this.currentZoom;
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000CD2D4 File Offset: 0x000CB4D4
		public virtual bool SnapToCurrentStateWithoutSmooth(bool resetZoom = false)
		{
			if (!this.isInit || this.currentTarget == null || this.targetCamera == null || this.currentState == null || this.lerpState == null)
			{
				return false;
			}
			bool useSmooth = this.useSmooth;
			bool flag = this.isFreezed;
			bool flag2;
			try
			{
				this.isFreezed = false;
				this.useSmooth = false;
				this._targetPositionsAreLocal = false;
				this.currentState.CopyState(this.lerpState);
				float num = ((resetZoom || !this.currentState.useZoom) ? this.currentState.defaultDistance : Mathf.Clamp(this.currentZoom, this.currentState.minDistance, this.currentState.maxDistance));
				this.currentZoom = num;
				this.distance = num;
				this.cullingDistance = num;
				this.currentHeight = this.currentState.height;
				this.cullingHeight = this.currentState.cullingHeight;
				this.currentSwitchRight = this.switchRight;
				this.transformWeight = 1f;
				this.firstUpdated = true;
				this.cameraVelocityDamp = Vector3.zero;
				this.lookTargetAdjust = Vector3.zero;
				this.mouseXStart = this.mouseX;
				this.mouseYStart = this.mouseY;
				this.snapPoseImmediately = true;
				if (this.currentState.cameraMode == TPCameraMode.FixedPoint)
				{
					this.CameraFixed();
				}
				else
				{
					this.CameraMovement(true);
				}
				this.startPosition = this.selfRigidbody.position;
				this.startRotation = this.selfRigidbody.rotation;
				flag2 = true;
			}
			finally
			{
				this.snapPoseImmediately = false;
				this.useSmooth = useSmooth;
				this.isFreezed = flag;
			}
			return flag2;
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x000CD48C File Offset: 0x000CB68C
		public virtual void AdjustHeight(float value)
		{
			this.customHeightOffset = Mathf.Clamp(this.customHeightOffset + value * this.heightAdjustmentSpeed * Time.deltaTime, this.minHeightOffset, this.maxHeightOffset);
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x000CD4BC File Offset: 0x000CB6BC
		public virtual void CheckCameraIsRotating()
		{
			this.cameraIsRotating = (double)(base.transform.eulerAngles - this.lastCameraRotation.eulerAngles).magnitude > 0.1 || this.movementSpeed.magnitude > 0f;
			this.lastCameraRotation.eulerAngles = base.transform.eulerAngles;
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x000CD52C File Offset: 0x000CB72C
		public virtual void RotateCamera(float x, float y)
		{
			if (this.currentState.cameraMode.Equals(TPCameraMode.FixedPoint) || !this.isInit || this.transformWeight < 1f)
			{
				this.smoothCameraRotation = this.initialCameraRotation;
				return;
			}
			if (!this.currentState.cameraMode.Equals(TPCameraMode.FixedAngle))
			{
				if (this.lockTarget)
				{
					this.smoothCameraRotation = this.initialCameraRotation;
					return;
				}
				this.mouseX += x * ((vInput.instance.inputDevice == InputDevice.Joystick) ? (this.currentState.xMouseSensitivity * this.joystickSensitivity) : (this.currentState.xMouseSensitivity * GameSettingsService.LookSensitivity));
				this.mouseY -= y * ((vInput.instance.inputDevice == InputDevice.Joystick) ? (this.currentState.yMouseSensitivity * this.joystickSensitivity) : (this.currentState.yMouseSensitivity * GameSettingsService.LookSensitivity));
				this.movementSpeed.x = x;
				this.movementSpeed.y = -y;
				this.CheckCameraIsRotating();
				bool flag = (base.transform.forward - this.currentTarget.forward).magnitude <= 0.5f;
				if (!this.LockCamera && this.cameraIsRotating)
				{
					this.lastRotationTimer = Time.time;
					if (this.movementSpeed.x != 0f || this.movementSpeed.y != 0f)
					{
						this.smoothCameraRotation = this.initialCameraRotation;
					}
					this.mouseY = vExtensions.ClampAngle(this.mouseY, this.lerpState.yMinLimit, this.lerpState.yMaxLimit);
					this.mouseX = vExtensions.ClampAngle(this.mouseX, this.lerpState.xMinLimit, this.lerpState.xMaxLimit);
					return;
				}
				if (this.LockCamera || (!flag && this.autoBehindTarget))
				{
					if (this.autoBehindTarget)
					{
						this.smoothCameraRotation = Mathf.Lerp(this.smoothCameraRotation, this.behindTargetSmoothRotation, 6f * Time.fixedDeltaTime);
					}
					if (this.LockCamera || Time.time > this.lastRotationTimer + this.behindTargetDelay)
					{
						this.mouseY = this.currentTarget.eulerAngles.NormalizeAngle().x;
						this.mouseX = this.currentTarget.eulerAngles.NormalizeAngle().y;
						return;
					}
				}
			}
			else
			{
				this.smoothCameraRotation = this.initialCameraRotation;
				float x2 = this.lerpState.fixedAngle.x;
				float y2 = this.lerpState.fixedAngle.y;
				this.mouseX = (this.useSmooth ? Mathf.LerpAngle(this.mouseX, x2, this.smoothBetweenState * Time.fixedDeltaTime) : x2);
				this.mouseY = (this.useSmooth ? Mathf.LerpAngle(this.mouseY, y2, this.smoothBetweenState * Time.fixedDeltaTime) : y2);
			}
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000CD83F File Offset: 0x000CBA3F
		public virtual void SwitchRight(bool value = false)
		{
			this.switchRight = (float)(value ? (-1) : 1);
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000CD850 File Offset: 0x000CBA50
		protected virtual void CalculeLockOnPoint()
		{
			if (this.currentState.cameraMode.Equals(TPCameraMode.FixedAngle) && this.lockTarget)
			{
				return;
			}
			Collider component = this.lockTarget.GetComponent<Collider>();
			if (component == null)
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(component.bounds.center - this.desired_cPos);
			float y = quaternion.eulerAngles.y;
			float num;
			if (quaternion.eulerAngles.x < -180f)
			{
				num = quaternion.eulerAngles.x + 360f;
			}
			else if (quaternion.eulerAngles.x > 180f)
			{
				num = quaternion.eulerAngles.x - 360f;
			}
			else
			{
				num = quaternion.eulerAngles.x;
			}
			if (this.lockTargetWeight < 1f)
			{
				this.lockTargetWeight += Time.fixedDeltaTime * this.lockTargetSpeed;
			}
			this.mouseY = Mathf.LerpAngle(this.mouseY, vExtensions.ClampAngle(num, this.currentState.yMinLimit, this.currentState.yMaxLimit), this.lockTargetWeight);
			this.mouseX = Mathf.LerpAngle(this.mouseX, vExtensions.ClampAngle(y, this.currentState.xMinLimit, this.currentState.xMaxLimit), this.lockTargetWeight);
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x000CD9BC File Offset: 0x000CBBBC
		public virtual void CameraMovement(bool forceUpdate = false)
		{
			if (this.currentTarget == null || this.targetCamera == null || (!this.firstStateIsInit && !forceUpdate))
			{
				return;
			}
			this.transformWeight = Mathf.Clamp(this.transformWeight += Time.fixedDeltaTime * this.startSmoothFactor, 0f, 1f);
			if (this.useSmooth)
			{
				this.currentState.Slerp(this.lerpState, this.smoothBetweenState * Time.fixedDeltaTime);
			}
			else
			{
				this.currentState.CopyState(this.lerpState);
			}
			if (this.currentState.useZoom)
			{
				this.currentZoom = Mathf.Clamp(this.currentZoom, this.lerpState.minDistance, this.lerpState.maxDistance);
				this.distance = (this.useSmooth ? Mathf.Lerp(this.distance, this.currentZoom, this.lerpState.smooth * Time.fixedDeltaTime) : this.currentZoom);
			}
			else
			{
				this.distance = (this.useSmooth ? Mathf.Lerp(this.distance, this.currentState.defaultDistance, this.lerpState.smooth * Time.fixedDeltaTime) : this.currentState.defaultDistance);
				this.currentZoom = this.currentState.defaultDistance;
			}
			this.targetCamera.fieldOfView = this.currentState.fov;
			this.cullingDistance = Mathf.Lerp(this.cullingDistance, this.currentZoom, this.smoothBetweenState * Time.fixedDeltaTime);
			this.currentSwitchRight = Mathf.Lerp(this.currentSwitchRight, this.switchRight, this.smoothSwitchSide * Time.fixedDeltaTime);
			Vector3 normalized = (this.currentState.forward * this.targetLookAt.forward + this.currentState.right * this.currentSwitchRight * this.targetLookAt.right).normalized;
			Vector3 vector = new Vector3(this.currentTarget.position.x, this.currentTarget.position.y, this.currentTarget.position.z) + this.currentTarget.transform.up * this.offSetPlayerPivot;
			float num = this.FollowHeightOffset;
			this.currentTargetPos = vector;
			this.desired_cPos = vector + this.currentTarget.transform.up * (this.currentState.height + this.customHeightOffset + num);
			this.current_cPos = (this.firstUpdated ? (vector + this.currentTarget.transform.up * (this.currentHeight + this.customHeightOffset + num)) : Vector3.SmoothDamp(this.current_cPos, vector + this.currentTarget.transform.up * (this.currentHeight + this.customHeightOffset + num), ref this.cameraVelocityDamp, this.lerpState.smoothDamp * Time.fixedDeltaTime));
			this.firstUpdated = false;
			ClipPlanePoints clipPlanePoints = this.targetCamera.NearClipPlanePoints(this.current_cPos + normalized * this.distance, this.clipPlaneMargin);
			ClipPlanePoints clipPlanePoints2 = this.targetCamera.NearClipPlanePoints(this.desired_cPos + normalized * this.currentZoom, this.clipPlaneMargin);
			RaycastHit raycastHit;
			if (Physics.SphereCast(vector, this.checkHeightRadius, this.currentTarget.transform.up, out raycastHit, Mathf.Max(0.01f, this.currentState.cullingHeight + num + 0.2f), this.cullingLayer, QueryTriggerInteraction.Ignore))
			{
				float num2 = raycastHit.distance - 0.2f - num;
				num2 -= this.currentState.height;
				num2 /= this.currentState.cullingHeight - this.currentState.height;
				this.cullingHeight = Mathf.Lerp(this.currentState.height, this.currentState.cullingHeight, Mathf.Clamp(num2, 0f, 1f));
			}
			else
			{
				this.cullingHeight = (this.useSmooth ? Mathf.Lerp(this.cullingHeight, this.currentState.cullingHeight, this.smoothBetweenState * Time.fixedDeltaTime) : this.currentState.cullingHeight);
			}
			if (this.CullingRayCast(this.desired_cPos, clipPlanePoints2, out raycastHit, this.currentZoom + 0.2f, this.cullingLayer, Color.blue))
			{
				float num3 = raycastHit.distance;
				if (num3 < this.currentState.defaultDistance)
				{
					float num4 = num3;
					num4 -= this.currentState.cullingMinDist;
					num4 /= this.currentZoom - this.currentState.cullingMinDist;
					this.currentHeight = Mathf.Lerp(this.cullingHeight, this.currentState.height, Mathf.Clamp(num4, 0f, 1f));
					this.current_cPos = vector + this.currentTarget.transform.up * (this.currentHeight + num);
				}
			}
			else
			{
				this.currentHeight = (this.useSmooth ? Mathf.Lerp(this.currentHeight, this.currentState.height, this.smoothBetweenState * Time.fixedDeltaTime) : this.currentState.height);
			}
			if (this.cullingDistance < this.distance)
			{
				this.distance = this.cullingDistance;
			}
			if (this.CullingRayCast(this.current_cPos, clipPlanePoints, out raycastHit, this.distance, this.cullingLayer, Color.cyan))
			{
				this.distance = Mathf.Clamp(this.cullingDistance, 0f, this.currentZoom);
			}
			Vector3 vector2 = this.current_cPos + this.targetLookAt.forward * this.targetCamera.farClipPlane + this.targetLookAt.right * Vector3.Dot(normalized * this.distance, this.targetLookAt.right);
			this.targetLookAt.position = this.current_cPos;
			float num5 = Mathf.LerpAngle(this.mouseYStart, this.mouseY, this.transformWeight);
			float num6 = Mathf.LerpAngle(this.mouseXStart, this.mouseX, this.transformWeight);
			Quaternion quaternion = Quaternion.Euler(num5 + this.offsetMouse.y, num6 + this.offsetMouse.x, 0f);
			this.targetLookAt.rotation = (this.useSmooth ? Quaternion.Lerp(this.targetLookAt.rotation, quaternion, this.smoothCameraRotation * Time.fixedDeltaTime) : quaternion);
			Vector3 vector3 = Vector3.Lerp(this.startPosition, this.current_cPos + normalized * this.distance, this.transformWeight);
			if (this.snapPoseImmediately)
			{
				this.selfRigidbody.position = vector3;
				base.transform.position = vector3;
			}
			else
			{
				this.selfRigidbody.MovePosition(vector3);
			}
			Quaternion quaternion2 = Quaternion.LookRotation(vector2 - this.selfRigidbody.position);
			if (this.lockTarget)
			{
				this.CalculeLockOnPoint();
				if (!this.currentState.cameraMode.Equals(TPCameraMode.FixedAngle))
				{
					Collider component = this.lockTarget.GetComponent<Collider>();
					if (component != null)
					{
						Vector3 vector4 = Quaternion.LookRotation(component.bounds.center + Vector3.up * this.heightOffset - this.selfRigidbody.position).eulerAngles - quaternion2.eulerAngles;
						if (this.isNewTarget)
						{
							this.lookTargetAdjust.x = Mathf.LerpAngle(this.lookTargetAdjust.x, vector4.x, this.lockTargetWeight);
							this.lookTargetAdjust.y = Mathf.LerpAngle(this.lookTargetAdjust.y, vector4.y, this.lockTargetWeight);
							this.lookTargetAdjust.z = Mathf.LerpAngle(this.lookTargetAdjust.z, vector4.z, this.lockTargetWeight);
							if (Vector3.Distance(this.lookTargetAdjust, vector4) < 0.5f)
							{
								this.isNewTarget = false;
							}
						}
						else
						{
							this.lookTargetAdjust = vector4;
						}
					}
				}
			}
			else
			{
				this.lookTargetAdjust.x = Mathf.LerpAngle(this.lookTargetAdjust.x, 0f, this.currentState.smooth * Time.fixedDeltaTime);
				this.lookTargetAdjust.y = Mathf.LerpAngle(this.lookTargetAdjust.y, 0f, this.currentState.smooth * Time.fixedDeltaTime);
				this.lookTargetAdjust.z = Mathf.LerpAngle(this.lookTargetAdjust.z, 0f, this.currentState.smooth * Time.fixedDeltaTime);
			}
			Vector3 vector5 = quaternion2.eulerAngles + this.lookTargetAdjust;
			vector5.z = 0f;
			Quaternion quaternion3 = Quaternion.Euler(vector5 + this.currentState.rotationOffSet);
			Quaternion quaternion4 = Quaternion.Lerp(this.startRotation, quaternion3, this.transformWeight);
			if (this.snapPoseImmediately)
			{
				this.selfRigidbody.rotation = quaternion4;
				base.transform.rotation = quaternion4;
			}
			else
			{
				this.selfRigidbody.MoveRotation(quaternion4);
			}
			this.movementSpeed = Vector2.zero;
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000CE36C File Offset: 0x000CC56C
		protected virtual void CameraFixed()
		{
			if (this.useSmooth)
			{
				this.currentState.Slerp(this.lerpState, this.smoothBetweenState);
			}
			else
			{
				this.currentState.CopyState(this.lerpState);
			}
			this.transformWeight = Mathf.Clamp(this.transformWeight += Time.fixedDeltaTime, 0f, 1f);
			Vector3 vector = new Vector3(this.currentTarget.position.x, this.currentTarget.position.y + this.offSetPlayerPivot + this.currentState.height + this.FollowHeightOffset, this.currentTarget.position.z);
			this.currentTargetPos = (this.useSmooth ? Vector3.MoveTowards(this.currentTargetPos, vector, this.currentState.smooth * Time.fixedDeltaTime) : vector);
			this.current_cPos = this.currentTargetPos;
			Vector3 vector2 = (this.isValidFixedPoint ? (this.currentState.lookPoints[this.indexLookPoint].positionPoint + this._fixedPointWorldOffset) : base.transform.position);
			base.transform.position = Vector3.Lerp(this.startPosition, this.useSmooth ? Vector3.Lerp(base.transform.position, vector2, this.currentState.smooth * Time.fixedDeltaTime) : vector2, this.transformWeight);
			this.targetLookAt.position = this.current_cPos;
			if (this.isValidFixedPoint && this.currentState.lookPoints[this.indexLookPoint].freeRotation)
			{
				Quaternion quaternion = Quaternion.Euler(this.currentState.lookPoints[this.indexLookPoint].eulerAngle);
				base.transform.rotation = Quaternion.Lerp(this.startRotation, this.useSmooth ? Quaternion.Slerp(base.transform.rotation, quaternion, this.currentState.smooth * 0.5f * Time.fixedDeltaTime) : quaternion, this.transformWeight);
			}
			else if (this.isValidFixedPoint)
			{
				Quaternion quaternion2 = Quaternion.LookRotation(this.currentTargetPos - base.transform.position);
				base.transform.rotation = Quaternion.Lerp(this.startRotation, this.useSmooth ? Quaternion.Slerp(base.transform.rotation, quaternion2, this.currentState.smooth * Time.fixedDeltaTime) : quaternion2, this.transformWeight);
			}
			this.targetCamera.fieldOfView = this.currentState.fov;
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060024AE RID: 9390 RVA: 0x000CE618 File Offset: 0x000CC818
		protected virtual bool isValidFixedPoint
		{
			get
			{
				return this.currentState.lookPoints != null && this.currentState.cameraMode.Equals(TPCameraMode.FixedPoint) && (this.indexLookPoint < this.currentState.lookPoints.Count || this.currentState.lookPoints.Count > 0);
			}
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x000CE680 File Offset: 0x000CC880
		protected virtual bool CullingRayCast(Vector3 from, ClipPlanePoints _to, out RaycastHit hitInfo, float distance, LayerMask cullingLayer, Color color)
		{
			bool flag = false;
			if (this.showGizmos)
			{
				Debug.DrawRay(from, _to.LowerLeft - from, color);
				Debug.DrawLine(_to.LowerLeft, _to.LowerRight, color);
				Debug.DrawLine(_to.UpperLeft, _to.UpperRight, color);
				Debug.DrawLine(_to.UpperLeft, _to.LowerLeft, color);
				Debug.DrawLine(_to.UpperRight, _to.LowerRight, color);
				Debug.DrawRay(from, _to.LowerRight - from, color);
				Debug.DrawRay(from, _to.UpperLeft - from, color);
				Debug.DrawRay(from, _to.UpperRight - from, color);
			}
			if (Physics.Raycast(from, _to.LowerLeft - from, out hitInfo, distance, cullingLayer, QueryTriggerInteraction.Ignore))
			{
				flag = true;
				this.cullingDistance = hitInfo.distance;
			}
			if (Physics.Raycast(from, _to.LowerRight - from, out hitInfo, distance, cullingLayer, QueryTriggerInteraction.Ignore))
			{
				flag = true;
				if (this.cullingDistance > hitInfo.distance)
				{
					this.cullingDistance = hitInfo.distance;
				}
			}
			if (Physics.Raycast(from, _to.UpperLeft - from, out hitInfo, distance, cullingLayer, QueryTriggerInteraction.Ignore))
			{
				flag = true;
				if (this.cullingDistance > hitInfo.distance)
				{
					this.cullingDistance = hitInfo.distance;
				}
			}
			if (Physics.Raycast(from, _to.UpperRight - from, out hitInfo, distance, cullingLayer, QueryTriggerInteraction.Ignore))
			{
				flag = true;
				if (this.cullingDistance > hitInfo.distance)
				{
					this.cullingDistance = hitInfo.distance;
				}
			}
			return hitInfo.collider && flag;
		}

		// Token: 0x0400242A RID: 9258
		private static vThirdPersonCamera _instance;

		// Token: 0x0400242B RID: 9259
		public Transform mainTarget;

		// Token: 0x0400242C RID: 9260
		[Tooltip("Lerp speed between Camera States")]
		[SerializeField]
		protected float _smoothBetweenState = 6f;

		// Token: 0x0400242D RID: 9261
		[SerializeField]
		protected float _smoothCameraRotation = 12f;

		// Token: 0x0400242E RID: 9262
		[SerializeField]
		protected float _smoothSwitchSide = 2f;

		// Token: 0x0400242F RID: 9263
		[SerializeField]
		protected float _scrollSpeed = 10f;

		// Token: 0x04002430 RID: 9264
		[Tooltip("Multiplier of Mouse x and y when using joystick")]
		[SerializeField]
		protected float _joystickSensitivity = 1f;

		// Token: 0x04002431 RID: 9265
		[Tooltip("What layer will be culled")]
		public LayerMask cullingLayer = 1;

		// Token: 0x04002432 RID: 9266
		[Tooltip("Change this value If the camera pass through the wall")]
		public float clipPlaneMargin;

		// Token: 0x04002433 RID: 9267
		public float checkHeightRadius;

		// Token: 0x04002434 RID: 9268
		public bool showGizmos;

		// Token: 0x04002435 RID: 9269
		public bool startUsingTargetRotation = true;

		// Token: 0x04002436 RID: 9270
		public bool startSmooth;

		// Token: 0x04002437 RID: 9271
		[vHideInInspector("startSmooth", false)]
		public float startSmoothFactor = 1f;

		// Token: 0x04002438 RID: 9272
		[Tooltip("Returns to behind the target automatically after 'behindTargetDelay' period")]
		public bool autoBehindTarget;

		// Token: 0x04002439 RID: 9273
		[vHideInInspector("autoBehindTarget", false)]
		public float behindTargetDelay = 2f;

		// Token: 0x0400243A RID: 9274
		[vHideInInspector("autoBehindTarget", false)]
		public float behindTargetSmoothRotation = 1f;

		// Token: 0x0400243B RID: 9275
		[Tooltip("Debug purposes, lock the camera behind the character for better align the states")]
		[SerializeField]
		protected bool lockCamera;

		// Token: 0x0400243C RID: 9276
		private WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();

		// Token: 0x0400243D RID: 9277
		public Vector2 offsetMouse;

		// Token: 0x0400243E RID: 9278
		public float heightAdjustmentSpeed = 2f;

		// Token: 0x0400243F RID: 9279
		public float minHeightOffset = -0.5f;

		// Token: 0x04002440 RID: 9280
		public float maxHeightOffset = 1f;

		// Token: 0x04002441 RID: 9281
		protected float customHeightOffset;

		// Token: 0x04002442 RID: 9282
		private Object followHeightOwner;

		// Token: 0x04002443 RID: 9283
		private Transform followHeightTarget;

		// Token: 0x04002444 RID: 9284
		private float followHeightOffset;

		// Token: 0x04002445 RID: 9285
		[HideInInspector]
		public int indexList;

		// Token: 0x04002446 RID: 9286
		[HideInInspector]
		public int indexLookPoint;

		// Token: 0x04002447 RID: 9287
		[HideInInspector]
		public float offSetPlayerPivot;

		// Token: 0x04002448 RID: 9288
		[HideInInspector]
		public float distance = 5f;

		// Token: 0x04002449 RID: 9289
		[HideInInspector]
		public string currentStateName;

		// Token: 0x0400244A RID: 9290
		[HideInInspector]
		public Transform currentTarget;

		// Token: 0x0400244B RID: 9291
		[HideInInspector]
		public vThirdPersonCameraState currentState;

		// Token: 0x0400244C RID: 9292
		[HideInInspector]
		public vThirdPersonCameraListData CameraStateList;

		// Token: 0x0400244D RID: 9293
		[HideInInspector]
		public Transform lockTarget;

		// Token: 0x0400244E RID: 9294
		[HideInInspector]
		public Vector2 movementSpeed;

		// Token: 0x0400244F RID: 9295
		[HideInInspector]
		public vThirdPersonCameraState lerpState;

		// Token: 0x04002450 RID: 9296
		protected float lockTargetSpeed;

		// Token: 0x04002451 RID: 9297
		protected float lockTargetWeight;

		// Token: 0x04002452 RID: 9298
		protected float initialCameraRotation;

		// Token: 0x04002453 RID: 9299
		protected bool cameraIsRotating;

		// Token: 0x04002454 RID: 9300
		protected Quaternion lastCameraRotation;

		// Token: 0x04002455 RID: 9301
		protected float lastRotationTimer;

		// Token: 0x04002456 RID: 9302
		protected Vector3 currentTargetPos;

		// Token: 0x04002457 RID: 9303
		protected Vector3 lookPoint;

		// Token: 0x04002458 RID: 9304
		protected Vector3 current_cPos;

		// Token: 0x04002459 RID: 9305
		protected Vector3 desired_cPos;

		// Token: 0x0400245A RID: 9306
		protected Vector3 lookTargetAdjust;

		// Token: 0x0400245B RID: 9307
		internal float _mouseY;

		// Token: 0x0400245C RID: 9308
		internal float _mouseX;

		// Token: 0x0400245F RID: 9311
		private readonly Dictionary<vThirdPersonCameraState, float> _stateZoom = new Dictionary<vThirdPersonCameraState, float>();

		// Token: 0x04002462 RID: 9314
		internal float _switchRight;

		// Token: 0x0400246A RID: 9322
		protected Quaternion fixedRotation;

		// Token: 0x0400246B RID: 9323
		internal Camera targetCamera;

		// Token: 0x0400246C RID: 9324
		protected float transformWeight;

		// Token: 0x0400246D RID: 9325
		protected bool snapPoseImmediately;

		// Token: 0x04002472 RID: 9330
		protected Vector3 cameraVelocityDamp;

		// Token: 0x04002473 RID: 9331
		protected Transform _lookAtTarget;

		// Token: 0x04002474 RID: 9332
		protected Vector3 lastLookAtPosition;

		// Token: 0x04002475 RID: 9333
		protected Vector3 lastLookAtForward;

		// Token: 0x04002476 RID: 9334
		private bool _targetPositionsAreLocal;

		// Token: 0x04002477 RID: 9335
		private Vector3 _fixedPointWorldOffset;

		// Token: 0x04002478 RID: 9336
		public bool isFreezed;

		// Token: 0x04002479 RID: 9337
		protected Rigidbody _selfRigidbody;
	}
}
