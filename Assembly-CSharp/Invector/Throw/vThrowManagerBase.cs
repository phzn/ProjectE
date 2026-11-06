using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.Throw
{
	// Token: 0x020004F7 RID: 1271
	public abstract class vThrowManagerBase : vMonoBehaviour
	{
		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001EA8 RID: 7848 RVA: 0x000B8355 File Offset: 0x000B6555
		public virtual int MaxThrowObjects { get; }

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001EA9 RID: 7849
		public abstract int CurrentThrowAmount { get; }

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001EAA RID: 7850 RVA: 0x000B835D File Offset: 0x000B655D
		public virtual Sprite CurrentThrowableSprite { get; }

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001EAB RID: 7851 RVA: 0x000B8365 File Offset: 0x000B6565
		// (set) Token: 0x06001EAC RID: 7852 RVA: 0x000B836D File Offset: 0x000B656D
		protected virtual Collider[] selfColliders { get; set; }

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001EAD RID: 7853 RVA: 0x000B8376 File Offset: 0x000B6576
		// (set) Token: 0x06001EAE RID: 7854 RVA: 0x000B837E File Offset: 0x000B657E
		protected virtual bool isAiming { get; set; }

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001EAF RID: 7855 RVA: 0x000B8387 File Offset: 0x000B6587
		// (set) Token: 0x06001EB0 RID: 7856 RVA: 0x000B838F File Offset: 0x000B658F
		protected virtual bool isThrowing { get; set; }

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x000B8398 File Offset: 0x000B6598
		// (set) Token: 0x06001EB2 RID: 7858 RVA: 0x000B83A0 File Offset: 0x000B65A0
		protected virtual bool pressThrowInput { get; set; }

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x000B83A9 File Offset: 0x000B65A9
		// (set) Token: 0x06001EB4 RID: 7860 RVA: 0x000B83B1 File Offset: 0x000B65B1
		protected virtual bool wasAiming { get; set; }

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x000B83BA File Offset: 0x000B65BA
		// (set) Token: 0x06001EB6 RID: 7862 RVA: 0x000B83C2 File Offset: 0x000B65C2
		protected virtual bool inEnterThrowMode { get; set; }

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000B83CB File Offset: 0x000B65CB
		// (set) Token: 0x06001EB8 RID: 7864 RVA: 0x000B83D3 File Offset: 0x000B65D3
		protected virtual Vector3 surfaceNormal { get; set; }

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000B83DC File Offset: 0x000B65DC
		// (set) Token: 0x06001EBA RID: 7866 RVA: 0x000B83E4 File Offset: 0x000B65E4
		protected virtual Transform rightUpperArm { get; set; }

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x000B83ED File Offset: 0x000B65ED
		// (set) Token: 0x06001EBC RID: 7868 RVA: 0x000B83F5 File Offset: 0x000B65F5
		protected virtual vThirdPersonInput tpInput { get; set; }

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001EBD RID: 7869
		public abstract vThrowableObject ObjectToThrow { get; }

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x000B83FE File Offset: 0x000B65FE
		// (set) Token: 0x06001EBF RID: 7871 RVA: 0x000B8406 File Offset: 0x000B6606
		protected virtual vThrowableObject lastThrowObject { get; set; }

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x000B8410 File Offset: 0x000B6610
		public virtual vThrowSettings CurrentSettings
		{
			get
			{
				vThrowSettings vThrowSettings = ((this.ObjectToThrow && this.ObjectToThrow.throwSettings) ? this.ObjectToThrow.throwSettings : this.defaultSettings);
				if (vThrowSettings != this.lastSettings)
				{
					this.lastSettings = vThrowSettings;
					this.onChangeSettings.Invoke(this.lastSettings);
				}
				return vThrowSettings;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001EC1 RID: 7873 RVA: 0x000B8477 File Offset: 0x000B6677
		// (set) Token: 0x06001EC2 RID: 7874 RVA: 0x000B847F File Offset: 0x000B667F
		protected virtual vThrowSettings lastSettings { get; set; }

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x000B8488 File Offset: 0x000B6688
		public virtual string cameraState
		{
			get
			{
				if (!this.tpInput.cc.isCrouching)
				{
					return this.CurrentSettings.cameraStateStanding;
				}
				return this.CurrentSettings.cameraStateCrouching;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001EC4 RID: 7876 RVA: 0x000B84B3 File Offset: 0x000B66B3
		public virtual bool alignToSurfaceNormal
		{
			get
			{
				return this.CurrentSettings.alignToSurfaceNormal;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x000B84C0 File Offset: 0x000B66C0
		public virtual bool alignToViewWhenNotHit
		{
			get
			{
				return this.CurrentSettings.alignToViewWhenNotHit;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x000B84CD File Offset: 0x000B66CD
		public virtual bool disableEndPointWhenNotHit
		{
			get
			{
				return this.CurrentSettings.disableEndPointWhenNotHit;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x000B84DA File Offset: 0x000B66DA
		public virtual float alignmentSmooth
		{
			get
			{
				return this.CurrentSettings.alignmentSmooth;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001EC8 RID: 7880 RVA: 0x000B84E7 File Offset: 0x000B66E7
		public virtual float metersPerSeconds
		{
			get
			{
				return this.CurrentSettings.metersPerSeconds;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x000B84F4 File Offset: 0x000B66F4
		public virtual Vector2 minMaxTime
		{
			get
			{
				return this.CurrentSettings.minMaxTime;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001ECA RID: 7882 RVA: 0x000B8501 File Offset: 0x000B6701
		public virtual float maxDistance
		{
			get
			{
				return this.CurrentSettings.maxDistance;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001ECB RID: 7883 RVA: 0x000B850E File Offset: 0x000B670E
		public virtual float maxVelocity
		{
			get
			{
				return this.CurrentSettings.maxVelocity;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001ECC RID: 7884 RVA: 0x000B851B File Offset: 0x000B671B
		public virtual float throwMaxForce
		{
			get
			{
				return this.CurrentSettings.throwMaxForce;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x000B8528 File Offset: 0x000B6728
		public virtual float throwDelayTime
		{
			get
			{
				return this.CurrentSettings.throwDelayTime;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x000B8535 File Offset: 0x000B6735
		public virtual float lineStepPerTime
		{
			get
			{
				return this.CurrentSettings.lineStepPerTime;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001ECF RID: 7887 RVA: 0x000B8542 File Offset: 0x000B6742
		public virtual float lineMaxLength
		{
			get
			{
				return this.CurrentSettings.maxLineLength;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001ED0 RID: 7888 RVA: 0x000B854F File Offset: 0x000B674F
		public virtual float exitThrowModeDelay
		{
			get
			{
				return this.CurrentSettings.exitThrowModeDelay;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x000B855C File Offset: 0x000B675C
		public virtual string throwAnimation
		{
			get
			{
				return this.CurrentSettings.throwAnimation;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001ED2 RID: 7890 RVA: 0x000B8569 File Offset: 0x000B6769
		public virtual string holdingAnimation
		{
			get
			{
				return this.CurrentSettings.holdingAnimation;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001ED3 RID: 7891 RVA: 0x000B8576 File Offset: 0x000B6776
		public virtual string cancelAnimation
		{
			get
			{
				return this.CurrentSettings.cancelAnimation;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001ED4 RID: 7892 RVA: 0x000B8584 File Offset: 0x000B6784
		public virtual vThrowVisualSettings CurrentVisualSettings
		{
			get
			{
				vThrowVisualSettings vThrowVisualSettings = ((this.ObjectToThrow && this.ObjectToThrow.throwVisualSettings) ? this.ObjectToThrow.throwVisualSettings : this.defaultVisualSettings);
				if (vThrowVisualSettings != this.lastSettings)
				{
					this.lastVisualSettings = vThrowVisualSettings;
					this.onChangeVisualSettings.Invoke(this.lastVisualSettings);
				}
				return vThrowVisualSettings;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001ED5 RID: 7893 RVA: 0x000B85EB File Offset: 0x000B67EB
		// (set) Token: 0x06001ED6 RID: 7894 RVA: 0x000B85F3 File Offset: 0x000B67F3
		protected virtual vThrowVisualSettings lastVisualSettings { get; set; }

		// Token: 0x06001ED7 RID: 7895 RVA: 0x000B85FC File Offset: 0x000B67FC
		protected virtual IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			if (this.lineRenderer == null)
			{
				this.lineRenderer = base.GetComponentInChildren<LineRenderer>(true);
			}
			if (this.lineRenderer)
			{
				this.lineRenderer.useWorldSpace = false;
			}
			if (this.lineRenderer && this.lineRenderer.gameObject.activeInHierarchy)
			{
				this.lineRenderer.gameObject.SetActive(false);
			}
			if (this.throwEnd && this.throwEnd.activeSelf)
			{
				this.throwEnd.SetActive(false);
			}
			vDrawHideMeleeWeapons drawHide;
			if (base.transform.root.TryGetComponent<vDrawHideMeleeWeapons>(out drawHide))
			{
				this.onEquipThrowable.AddListener(delegate
				{
					drawHide.HideWeapons(true);
				});
			}
			this.canUseThrow = true;
			this.tpInput = base.GetComponentInParent<vThirdPersonInput>();
			if (this.tpInput)
			{
				this.selfColliders = this.tpInput.GetComponentsInChildren<Collider>(true);
				this.tpInput.onUpdate -= this.UpdateThrowInput;
				this.tpInput.onUpdate += this.UpdateThrowInput;
				this.tpInput.onFixedUpdate -= this.UpdateThrowBehavior;
				this.tpInput.onFixedUpdate += this.UpdateThrowBehavior;
				this.rightUpperArm = this.tpInput.animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
				if (this.cameraStyle == vThrowManagerBase.CameraStyle.SideScroll)
				{
					this.rotateWhileAiming = true;
				}
			}
			if (this.cameraStyle != vThrowManagerBase.CameraStyle.ThirdPerson)
			{
				this.useThrowStartRightOffset = false;
				this.rotateWhileAiming = true;
				this.strafeWhileAiming = true;
			}
			yield break;
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x000B860B File Offset: 0x000B680B
		public virtual void CanUseThrow(bool value)
		{
			this.canUseThrow = value;
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x000B8614 File Offset: 0x000B6814
		protected virtual bool ThrowConditions
		{
			get
			{
				return !(this.ObjectToThrow == null) && this.CurrentThrowAmount > 0 && this.tpInput.enabled && !this.tpInput.cc.customAction && this.canUseThrow;
			}
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x000B8654 File Offset: 0x000B6854
		protected virtual void UpdateThrowBehavior()
		{
			this.CheckThrowObjectChanges();
			this.UpdateThrow();
			this.MoveAndRotate();
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x000B8668 File Offset: 0x000B6868
		protected virtual void CheckThrowObjectChanges()
		{
			if (this.ObjectToThrow != this.lastThrowObject)
			{
				this.lastThrowObject = this.ObjectToThrow;
				this.onSetActiveLine.Invoke(this.CurrentVisualSettings.useLine);
				this.onSetActiveIndicator.Invoke(this.CurrentVisualSettings.useIndicator);
			}
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x000B86C0 File Offset: 0x000B68C0
		protected virtual void UpdateThrow()
		{
			if (this.isAiming)
			{
				this.tpInput.CrouchInput();
				this.wasAiming = true;
				this.tpInput.SetWalkByDefault(this.walkWhileAiming);
				if (!string.IsNullOrEmpty(this.cameraState) && this.tpInput.customCameraState != this.cameraState)
				{
					this.tpInput.ChangeCameraStateWithLerp(this.cameraState);
				}
				this.CalculateAimPoint();
				if (this.drawTrajectory)
				{
					this.DrawTrajectory();
				}
				if (this.debugMode)
				{
					Debug.DrawLine(this.startPoint, this.aimPoint, Color.cyan);
				}
			}
			else if (!this.inEnterThrowMode)
			{
				if (this.wasAiming)
				{
					this.tpInput.ResetWalkByDefault();
					if (!string.IsNullOrEmpty(this.cameraState) && this.tpInput.customCameraState == this.cameraState)
					{
						this.tpInput.ResetCameraState();
					}
					if (this.lineRenderer && this.lineRenderer.gameObject.activeInHierarchy)
					{
						this.lineRenderer.gameObject.SetActive(false);
					}
					if (this.throwEnd && this.throwEnd.activeSelf)
					{
						this.throwEnd.SetActive(false);
					}
				}
				this.wasAiming = false;
			}
			if (this.pressThrowInput)
			{
				this.isThrowing = true;
				this.pressThrowInput = false;
				this.tpInput.animator.SetBool("IsAiming", false);
				this.tpInput.animator.CrossFadeInFixedTime(this.throwAnimation, 0.2f);
			}
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x000B8860 File Offset: 0x000B6A60
		protected virtual void MoveAndRotate()
		{
			if (this.isAiming || this.isThrowing || this.inEnterThrowMode)
			{
				this.tpInput.MoveInput();
				switch (this.cameraStyle)
				{
				case vThrowManagerBase.CameraStyle.ThirdPerson:
					if ((this.rotateWhileAiming && !this.isThrowing) || (this.isThrowing && this.rotateWhileThrowing))
					{
						this.tpInput.cc.RotateToDirection(this.tpInput.cameraMain.transform.forward);
						return;
					}
					break;
				case vThrowManagerBase.CameraStyle.TopDown:
				{
					Vector3 aimDirection = this.aimDirection;
					aimDirection.y = 0f;
					if (this.isThrowing || this.rotateWhileAiming)
					{
						this.tpInput.cc.RotateToDirection(aimDirection);
					}
					break;
				}
				case vThrowManagerBase.CameraStyle.SideScroll:
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x000B8928 File Offset: 0x000B6B28
		protected virtual void UpdateThrowInput()
		{
			if (!this.ThrowConditions)
			{
				return;
			}
			if (this.aimThrowInput.GetButtonDown() && !this.inEnterThrowMode && !this.isThrowing && !this.isAiming)
			{
				this.EnterThrowMode();
				return;
			}
			if (this.aimThrowInput.GetButtonUp() && this.aimHoldingButton && (this.isAiming || this.inEnterThrowMode) && !this.isThrowing)
			{
				this.ExitThrowMode();
			}
			if (this.isAiming && !this.isThrowing && !this.pressThrowInput && this.throwInput.GetButtonDown())
			{
				this.pressThrowInput = true;
			}
			if (!this.aimHoldingButton && this.aimThrowInput.GetButtonDown() && !this.pressThrowInput && (this.isAiming || this.inEnterThrowMode) && !this.isThrowing)
			{
				this.ExitThrowMode();
			}
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x000B8A04 File Offset: 0x000B6C04
		protected virtual vThrowableObject GetInstanceOfThrowable()
		{
			return Object.Instantiate<vThrowableObject>(this.ObjectToThrow, this.startPoint, Quaternion.identity);
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x000B8A1C File Offset: 0x000B6C1C
		protected virtual void Throw()
		{
			this.LaunchObject(this.GetInstanceOfThrowable());
			this.ExitThrowMode();
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x000B8A30 File Offset: 0x000B6C30
		protected virtual void LaunchObject(vThrowableObject throwable)
		{
			if (this.setIgnoreCollision)
			{
				Collider[] componentsInChildren = throwable.GetComponentsInChildren<Collider>();
				if (componentsInChildren.Length != 0)
				{
					foreach (Collider collider in componentsInChildren)
					{
						for (int j = 0; j < this.selfColliders.Length; j++)
						{
							Physics.IgnoreCollision(collider, this.selfColliders[j], true);
						}
					}
				}
			}
			throwable.transform.position = this.startPoint;
			throwable.isKinematic = false;
			throwable.transform.parent = null;
			throwable.onThrow.Invoke(this.tpInput.transform);
			throwable.selfRigidbody.velocity = this.StartVelocity;
			this.onThrowObject.Invoke();
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x000B8AE4 File Offset: 0x000B6CE4
		protected virtual void DrawTrajectory()
		{
			Mathf.Clamp(Vector3.Distance(this.startPoint, this.aimPoint) * this.metersPerSeconds, this.minMaxTime.x, this.minMaxTime.y);
			Vector3? vector = null;
			List<Vector3> trajectoryPoints = this.GetTrajectoryPoints(this.startPoint, this.StartVelocity, this.lineStepPerTime, this.lineMaxLength, ref vector);
			if (this.lineRenderer)
			{
				this.lineRenderer.transform.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(90f, base.transform.right) * (trajectoryPoints[trajectoryPoints.Count - 1] - this.startPoint).normalized);
				if (!this.lineRenderer.gameObject.activeInHierarchy)
				{
					this.lineRenderer.gameObject.SetActive(true);
				}
				this.lineRenderer.positionCount = trajectoryPoints.Count;
				for (int i = 0; i < this.lineRenderer.positionCount; i++)
				{
					this.lineRenderer.SetPosition(i, this.lineRenderer.transform.InverseTransformPoint(trajectoryPoints[i]));
				}
			}
			if (this.throwEnd)
			{
				if (!this.throwEnd.activeSelf)
				{
					if (!this.disableEndPointWhenNotHit || vector != null)
					{
						this.throwEnd.SetActive(true);
					}
				}
				else if (this.disableEndPointWhenNotHit && vector == null)
				{
					this.throwEnd.SetActive(false);
				}
				if (trajectoryPoints.Count > 1)
				{
					this.throwEnd.transform.position = trajectoryPoints[trajectoryPoints.Count - 1];
					this.throwEnd.transform.rotation = Quaternion.Lerp(this.throwEnd.transform.rotation, Quaternion.LookRotation(this.surfaceNormal, base.transform.up), this.alignmentSmooth * Time.deltaTime);
				}
			}
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x000B8CE4 File Offset: 0x000B6EE4
		internal virtual void TriggerAnimatorEvent(vThrowAnimatorEvent.ThrowEventType eventType)
		{
			switch (eventType)
			{
			case vThrowAnimatorEvent.ThrowEventType.EquipThrowable:
				this.onEquipThrowable.Invoke();
				this.EquipThrowObject();
				return;
			case vThrowAnimatorEvent.ThrowEventType.EnableAiming:
				this.EnableAimMode();
				return;
			case vThrowAnimatorEvent.ThrowEventType.CancelAiming:
				this.DisableAimMode();
				this.ExitThrowMode();
				return;
			case vThrowAnimatorEvent.ThrowEventType.StartLaunch:
				this.DisableAimMode();
				this.StartThrow();
				return;
			case vThrowAnimatorEvent.ThrowEventType.FinishLaunch:
				this.Throw();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x000B8D45 File Offset: 0x000B6F45
		protected virtual void EquipThrowObject()
		{
			this.onEquipThrowable.Invoke();
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x000B8D52 File Offset: 0x000B6F52
		protected virtual void EnableAimMode()
		{
			this.inEnterThrowMode = false;
			this.isAiming = true;
			this.onEnableAim.Invoke();
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x000B8D6D File Offset: 0x000B6F6D
		protected virtual void DisableAimMode()
		{
			this.inEnterThrowMode = false;
			this.isAiming = false;
			this.onCancelAim.Invoke();
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x000B8D88 File Offset: 0x000B6F88
		protected virtual void EnterThrowMode()
		{
			this.inEnterThrowMode = true;
			this.tpInput.animator.CrossFadeInFixedTime(this.holdingAnimation, 0.2f);
			this.tpInput.SetLockAllInput(true);
			this.tpInput.SetStrafeLocomotion(this.strafeWhileAiming);
			if (!string.IsNullOrEmpty(this.cameraState) && this.tpInput.customCameraState != this.cameraState)
			{
				this.tpInput.ChangeCameraStateWithLerp(this.cameraState);
			}
			this.tpInput.cc.isSprinting = false;
			this.tpInput.animator.SetInteger("ActionState", 1);
			if (this.cameraStyle == vThrowManagerBase.CameraStyle.SideScroll)
			{
				this.tpInput.cc.strafeSpeed.rotateWithCamera = true;
			}
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x000B8E50 File Offset: 0x000B7050
		protected virtual void StartThrow()
		{
			this.onStartThrowObject.Invoke();
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x000B8E60 File Offset: 0x000B7060
		protected virtual void ExitThrowMode()
		{
			this.isThrowing = false;
			this.tpInput.SetLockAllInput(false);
			this.tpInput.SetStrafeLocomotion(false);
			this.tpInput.cc.isSprinting = false;
			this.tpInput.animator.SetInteger("ActionState", 0);
			this.onFinishThrow.Invoke();
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x000B8EBE File Offset: 0x000B70BE
		public virtual bool CanCollectThrowable(string throwableName, out int remainingAmount)
		{
			remainingAmount = 0;
			return false;
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void OnCollectThrowable(string throwableName, int amount = 1)
		{
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001EEC RID: 7916 RVA: 0x000B8EC4 File Offset: 0x000B70C4
		protected virtual Vector3 thirdPersonAimPoint
		{
			get
			{
				Vector3 vector = this.tpInput.cameraMain.transform.position + this.tpInput.cameraMain.transform.forward * this.maxDistance;
				RaycastHit raycastHit;
				if (Physics.Linecast(this.tpInput.cameraMain.transform.position, vector, out raycastHit, this.obstacles))
				{
					vector = raycastHit.point;
				}
				return vector;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001EED RID: 7917 RVA: 0x000B8F3F File Offset: 0x000B713F
		protected virtual Vector3 topDownAimPoint
		{
			get
			{
				return vMousePositionHandler.Instance.WorldMousePosition(this.obstacles);
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001EEE RID: 7918 RVA: 0x000B8F54 File Offset: 0x000B7154
		protected virtual Vector3 sideScrollAimPoint
		{
			get
			{
				Vector3 vector = vMousePositionHandler.Instance.mousePosition;
				vector.z = this.tpInput.cameraMain.WorldToScreenPoint(this.startPoint).z;
				Vector3 vector2 = this.tpInput.cameraMain.ScreenToWorldPoint(vector, Camera.MonoOrStereoscopicEye.Mono);
				Vector3 vector3 = base.transform.InverseTransformVector(vector2);
				Vector3 vector4 = base.transform.InverseTransformVector(this.startPoint);
				vector3.x = vector4.x;
				return base.transform.TransformVector(vector3);
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001EEF RID: 7919 RVA: 0x000B8FE0 File Offset: 0x000B71E0
		protected virtual Vector3 startPoint
		{
			get
			{
				Vector3 position = this.throwStartPoint.position;
				Vector3 vector = this.aimPoint - this.throwStartPoint.position;
				Vector3 zero = Vector3.zero;
				zero.y = this.throwStartPoint.localPosition.y;
				Vector3 vector2 = base.transform.InverseTransformPoint(position);
				Vector3 vector3 = base.transform.TransformPoint(zero) + vector.normalized * vector2.z;
				if (this.useThrowStartRightOffset && this.tpInput && this.tpInput.tpCamera && this.tpInput.tpCamera.lerpState != null)
				{
					vector3 += this.tpInput.tpCamera.transform.right * this.tpInput.tpCamera.lerpState.right * this.throwStartRightOffsetMultiplier * this.tpInput.tpCamera.switchRight;
				}
				Vector3 vector4 = vector3 - base.transform.TransformPoint(zero);
				RaycastHit raycastHit;
				if (Physics.Linecast(base.transform.TransformPoint(zero), vector3, out raycastHit, this.obstacles))
				{
					vector3 = raycastHit.point - vector4.normalized * 0.2f;
				}
				return vector3;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001EF0 RID: 7920 RVA: 0x000B914C File Offset: 0x000B734C
		protected virtual Vector3 StartVelocity
		{
			get
			{
				return this.GetStartVelocity(null);
			}
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x000B9168 File Offset: 0x000B7368
		protected virtual void CalculateAimPoint()
		{
			switch (this.cameraStyle)
			{
			case vThrowManagerBase.CameraStyle.ThirdPerson:
				this.aimPoint = this.thirdPersonAimPoint;
				return;
			case vThrowManagerBase.CameraStyle.TopDown:
				this.aimPoint = this.topDownAimPoint;
				return;
			case vThrowManagerBase.CameraStyle.SideScroll:
				this.aimPoint = this.sideScrollAimPoint;
				return;
			default:
				return;
			}
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x000B91B8 File Offset: 0x000B73B8
		protected virtual Vector3 GetStartVelocity(Vector3? startPoint = null)
		{
			Vector3 vector = this.aimPoint - ((startPoint != null) ? startPoint.Value : this.startPoint);
			Vector3 vector2 = vector;
			float y = vector.y;
			float magnitude = vector2.magnitude;
			float num = Mathf.Clamp(vector.magnitude / this.metersPerSeconds, this.minMaxTime.x, this.minMaxTime.y);
			float num2 = magnitude / num;
			float num3 = y / num + 0.5f * Mathf.Abs(Physics.gravity.y) * num;
			Vector3 vector3 = vector2.normalized;
			vector3 *= num2;
			vector3.y = num3;
			return vector3.normalized * Mathf.Min(this.maxVelocity, vector3.magnitude);
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x000B927B File Offset: 0x000B747B
		protected virtual Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
		{
			return start + startVelocity * time + Physics.gravity * time * time * 0.5f;
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x000B92AC File Offset: 0x000B74AC
		protected virtual List<Vector3> GetTrajectoryPoints(Vector3 start, Vector3 startVelocity, float timestep, float maxLength, ref Vector3? hitPoint)
		{
			Vector3 vector = start;
			List<Vector3> list = new List<Vector3>();
			list.Add(vector);
			this.surfaceNormal = (this.alignToViewWhenNotHit ? (this.tpInput.cameraMain.transform.position - this.aimPoint).normalized : Vector3.up);
			float num = 0f;
			int num2 = 1;
			RaycastHit raycastHit;
			for (;;)
			{
				float num3 = timestep * (float)num2;
				Vector3 vector2 = this.PlotTrajectoryAtTime(start, startVelocity, num3);
				Vector3 normalized = (vector2 - vector).normalized;
				if (Physics.Linecast(vector, vector2 + normalized * 0.1f, out raycastHit, this.obstacles))
				{
					break;
				}
				if (this.debugMode)
				{
					Debug.DrawLine(vector, vector2, Color.red);
				}
				list.Add(vector2);
				num += (vector2 - vector).magnitude;
				vector = vector2;
				if (num > maxLength)
				{
					return list;
				}
				num2++;
			}
			list.Add(raycastHit.point);
			if (this.alignToSurfaceNormal)
			{
				this.surfaceNormal = raycastHit.normal;
			}
			hitPoint = new Vector3?(raycastHit.point);
			if (this.debugMode)
			{
				Debug.DrawLine(vector, raycastHit.point, Color.red);
			}
			return list;
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001EF5 RID: 7925 RVA: 0x000B93F0 File Offset: 0x000B75F0
		// (set) Token: 0x06001EF6 RID: 7926 RVA: 0x000B93F8 File Offset: 0x000B75F8
		public virtual Vector3 aimPoint { get; protected set; }

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x000B9401 File Offset: 0x000B7601
		public virtual Vector3 aimDirection
		{
			get
			{
				return this.aimPoint - this.startPoint;
			}
		}

		// Token: 0x04001F77 RID: 8055
		[vEditorToolbar("Settings", false, "", false, false)]
		public vThrowManagerBase.CameraStyle cameraStyle;

		// Token: 0x04001F78 RID: 8056
		[Tooltip("Use this to enable/disable if you can or not enter ThrowMode")]
		public bool canUseThrow;

		// Token: 0x04001F79 RID: 8057
		[Tooltip("Check this to use the Line that draws the Throw Trajectory")]
		public bool drawTrajectory = true;

		// Token: 0x04001F7A RID: 8058
		public LineRenderer lineRenderer;

		// Token: 0x04001F7B RID: 8059
		[Tooltip("This is the Starting point of the Throw")]
		public Transform throwStartPoint;

		// Token: 0x04001F7C RID: 8060
		[Tooltip("This is the Ending point (landing) of the Throw")]
		public GameObject throwEnd;

		// Token: 0x04001F7D RID: 8061
		public LayerMask obstacles = 1;

		// Token: 0x04001F7E RID: 8062
		public vThrowSettings defaultSettings;

		// Token: 0x04001F7F RID: 8063
		public vThrowVisualSettings defaultVisualSettings;

		// Token: 0x04001F80 RID: 8064
		[Tooltip("Set ignore collision to the grenade to not collide with the Player")]
		public bool setIgnoreCollision;

		// Token: 0x04001F81 RID: 8065
		public bool debugMode;

		// Token: 0x04001F82 RID: 8066
		[vSeparator("Only for ThirdPerson Camera Style", "")]
		[Tooltip("The Third person camera right will be applied as offset to throw start point")]
		public bool useThrowStartRightOffset;

		// Token: 0x04001F83 RID: 8067
		[Tooltip("Sets a Offset right for the Throw Start Point")]
		[vHideInInspector("useThrowStartRightOffset", false)]
		public float throwStartRightOffsetMultiplier = 1f;

		// Token: 0x04001F84 RID: 8068
		[vSeparator("Controller Settings", "")]
		[Tooltip("Force the controller to walk while aiming")]
		public bool walkWhileAiming;

		// Token: 0x04001F85 RID: 8069
		[Tooltip("Rotate to aim point while aiming")]
		public bool rotateWhileAiming = true;

		// Token: 0x04001F86 RID: 8070
		public bool rotateWhileThrowing = true;

		// Token: 0x04001F87 RID: 8071
		public bool strafeWhileAiming = true;

		// Token: 0x04001F88 RID: 8072
		[vEditorToolbar("Inputs", false, "", false, false)]
		public GenericInput throwInput = new GenericInput("Mouse0", "RB", "RB");

		// Token: 0x04001F89 RID: 8073
		public GenericInput aimThrowInput = new GenericInput("G", "LB", "LB");

		// Token: 0x04001F8A RID: 8074
		public bool aimHoldingButton = true;

		// Token: 0x04001F8B RID: 8075
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onEquipThrowable;

		// Token: 0x04001F8C RID: 8076
		public UnityEvent onEnableAim;

		// Token: 0x04001F8D RID: 8077
		public UnityEvent onCancelAim;

		// Token: 0x04001F8E RID: 8078
		public UnityEvent onStartThrowObject;

		// Token: 0x04001F8F RID: 8079
		public UnityEvent onThrowObject;

		// Token: 0x04001F90 RID: 8080
		public UnityEvent onCollectObject;

		// Token: 0x04001F91 RID: 8081
		public UnityEvent onFinishThrow;

		// Token: 0x04001F92 RID: 8082
		[vEditorToolbar("Visual Effect Events", false, "", false, false)]
		[vSeparator("Line", "")]
		public vThrowManagerBase.ActiveEvent onSetActiveLine;

		// Token: 0x04001F93 RID: 8083
		[vSeparator("Indicator", "")]
		public vThrowManagerBase.ActiveEvent onSetActiveIndicator;

		// Token: 0x04001F94 RID: 8084
		public vThrowManagerBase.SettingsEvent onChangeSettings;

		// Token: 0x04001F95 RID: 8085
		public vThrowManagerBase.VisualEvent onChangeVisualSettings;

		// Token: 0x020004F8 RID: 1272
		[Serializable]
		public class VisualEvent : UnityEvent<vThrowVisualSettings>
		{
		}

		// Token: 0x020004F9 RID: 1273
		[Serializable]
		public class SettingsEvent : UnityEvent<vThrowSettings>
		{
		}

		// Token: 0x020004FA RID: 1274
		[Serializable]
		public class ActiveEvent : UnityEvent<bool>
		{
		}

		// Token: 0x020004FB RID: 1275
		public enum CameraStyle
		{
			// Token: 0x04001FA6 RID: 8102
			ThirdPerson,
			// Token: 0x04001FA7 RID: 8103
			TopDown,
			// Token: 0x04001FA8 RID: 8104
			SideScroll
		}
	}
}
