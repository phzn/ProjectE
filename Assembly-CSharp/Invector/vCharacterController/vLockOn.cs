using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController
{
	// Token: 0x020006BB RID: 1723
	[vClassHeader("MELEE LOCK-ON", true, "icon_v2", false, "")]
	public class vLockOn : vLockOnBehaviour
	{
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06002A7D RID: 10877 RVA: 0x000EDF10 File Offset: 0x000EC110
		// (set) Token: 0x06002A7E RID: 10878 RVA: 0x000EDF18 File Offset: 0x000EC118
		protected virtual bool inTarget
		{
			get
			{
				return this._inTarget;
			}
			set
			{
				this._inTarget = value;
			}
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x000EDF24 File Offset: 0x000EC124
		protected override void Start()
		{
			base.Start();
			base.Init();
			this.tpInput = base.GetComponent<vThirdPersonInput>();
			if (this.tpInput)
			{
				this.tpInput.onUpdate -= this.UpdateLockOn;
				this.tpInput.onUpdate += this.UpdateLockOn;
				base.GetComponent<vHealthController>().onDead.AddListener(delegate(GameObject g)
				{
					this.isLockingOn = false;
					this.LockOn(false);
					this.UpdateLockOn();
				});
			}
			if (!this.aimImageContainer)
			{
				this.aimImageContainer = base.gameObject.GetComponentInChildren<Canvas>(true);
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x000EDFC4 File Offset: 0x000EC1C4
		public RectTransform aimImage
		{
			get
			{
				if (this._aimImage)
				{
					return this._aimImage;
				}
				if (this.aimImageContainer)
				{
					this._aimImage = Object.Instantiate<RectTransform>(this.aimImagePrefab, Vector2.zero, Quaternion.identity);
					this._aimImage.SetParent(this.aimImageContainer.transform);
					return this._aimImage;
				}
				Debug.LogWarning("Missing UI Canvas in the scene, please add one");
				return null;
			}
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x000EE03A File Offset: 0x000EC23A
		protected virtual void UpdateLockOn()
		{
			if (this.tpInput == null)
			{
				return;
			}
			this.LockOnInput();
			this.SwitchTargetsInput();
			this.CheckForTargetDistance();
			this.CheckForCharacterAlive();
			this.UpdateAimImage();
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000EE06C File Offset: 0x000EC26C
		protected virtual void LockOnInput()
		{
			if (this.tpInput.tpCamera == null || this.tpInput.cc == null)
			{
				return;
			}
			if (this.lockOnInput.GetButtonDown() && !this.tpInput.cc.customAction)
			{
				this.isLockingOn = !this.isLockingOn;
				this.LockOn(this.isLockingOn);
			}
			else if ((this.isLockingOn && this.tpInput.tpCamera.lockTarget == null) || this.LostTargetDistance())
			{
				this.isLockingOn = false;
				this.LockOn(false);
			}
			if (this.strafeWhileLockOn && !this.tpInput.cc.locomotionType.Equals(vThirdPersonMotor.LocomotionType.OnlyStrafe))
			{
				if (this.isLockingOn && this.tpInput.tpCamera.lockTarget != null)
				{
					this.tpInput.cc.lockInStrafe = true;
					this.tpInput.cc.isStrafing = true;
					return;
				}
				this.tpInput.cc.lockInStrafe = false;
				this.tpInput.cc.isStrafing = false;
			}
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x000EE1A8 File Offset: 0x000EC3A8
		protected virtual bool LostTargetDistance()
		{
			return this.isLockingOn && !(this.currentTarget == null) && Vector3.Distance(base.transform.position, this.currentTarget.position) > this.rangeToExitLockOn;
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x000EE1E8 File Offset: 0x000EC3E8
		protected override void SetTarget()
		{
			if (this.tpInput.tpCamera != null)
			{
				this.tpInput.tpCamera.SetLockTarget(this.currentTarget.transform, this.cameraHeightOffset, this.lockSpeed);
				this.onLockOnTarget.Invoke(this.currentTarget);
			}
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000EE240 File Offset: 0x000EC440
		protected virtual void SwitchTargetsInput()
		{
			if (this.tpInput.tpCamera == null)
			{
				return;
			}
			if (this.tpInput.tpCamera.lockTarget)
			{
				if (this.previousTargetInput.GetButtonDown())
				{
					this.PreviousTarget();
					return;
				}
				if (this.nexTargetInput.GetButtonDown())
				{
					this.NextTarget();
				}
			}
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x000EE2A0 File Offset: 0x000EC4A0
		protected virtual void CheckForTargetDistance()
		{
			if (!this.isLockingOn || this.currentTarget == null)
			{
				return;
			}
			if (Vector3.Distance(base.transform.position, this.currentTarget.position) > this.rangeToExitLockOn)
			{
				this.isLockingOn = false;
				this.LockOn(false);
			}
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x000EE2F8 File Offset: 0x000EC4F8
		protected virtual void CheckForCharacterAlive()
		{
			if ((this.currentTarget && !this.isCharacterAlive() && this.inTarget) || (this.inTarget && !this.isCharacterAlive()))
			{
				this.ResetLockOn();
				this.inTarget = false;
				this.LockOn(true);
				this.StopLockOn();
			}
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000EE34C File Offset: 0x000EC54C
		protected virtual void LockOn(bool value)
		{
			base.UpdateLockOn(value);
			if (!this.inTarget && this.currentTarget)
			{
				this.inTarget = true;
				this.SetTarget();
				return;
			}
			if (this.inTarget && !this.currentTarget)
			{
				this.inTarget = false;
				this.StopLockOn();
			}
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000EE3A8 File Offset: 0x000EC5A8
		protected virtual void UpdateAimImage()
		{
			if (!this.aimImageContainer || !this.aimImage)
			{
				return;
			}
			if (this.hideSprite)
			{
				this.aimImage.sizeDelta = this.aimImageSize;
				if (this.currentTarget && !this.aimImage.transform.gameObject.activeSelf && this.isCharacterAlive())
				{
					this.aimImage.transform.gameObject.SetActive(true);
				}
				else if (!this.currentTarget && this.aimImage.transform.gameObject.activeSelf)
				{
					this.aimImage.transform.gameObject.SetActive(false);
				}
				else if (this._aimImage.transform.gameObject.activeSelf && !this.isCharacterAlive())
				{
					this.aimImage.transform.gameObject.SetActive(false);
				}
			}
			if (this.currentTarget && this.aimImage && this.aimImageContainer)
			{
				this.aimImage.anchoredPosition = this.currentTarget.GetScreenPointOffBoundsCenter(this.aimImageContainer, this.tpCamera.targetCamera, this.spriteHeight);
				return;
			}
			if (this.aimImageContainer)
			{
				this.aimImage.anchoredPosition = Vector2.zero;
			}
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000EE514 File Offset: 0x000EC714
		public virtual void StopLockOn()
		{
			if (this.currentTarget == null && this.tpInput.tpCamera != null)
			{
				this.onUnLockOnTarget.Invoke(this.tpInput.tpCamera.lockTarget);
				this.tpInput.tpCamera.RemoveLockTarget();
				this.isLockingOn = false;
				this.inTarget = false;
			}
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000EE57B File Offset: 0x000EC77B
		public virtual void NextTarget()
		{
			base.ChangeTarget(1);
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000EE584 File Offset: 0x000EC784
		public virtual void PreviousTarget()
		{
			base.ChangeTarget(-1);
		}

		// Token: 0x04002A69 RID: 10857
		[Tooltip("Make sure to disable or change the StrafeInput to a different key at the Player Input component")]
		public bool strafeWhileLockOn = true;

		// Token: 0x04002A6A RID: 10858
		[Tooltip("Create a Image inside the UI and assign here")]
		public RectTransform aimImagePrefab;

		// Token: 0x04002A6B RID: 10859
		public Canvas aimImageContainer;

		// Token: 0x04002A6C RID: 10860
		public Vector2 aimImageSize = new Vector2(30f, 30f);

		// Token: 0x04002A6D RID: 10861
		[Tooltip("True: Hide the sprite when not Lock On, False: Always show the Sprite")]
		public bool hideSprite = true;

		// Token: 0x04002A6E RID: 10862
		[Tooltip("Create a offset for the sprite based at the center of the target")]
		[Range(-0.5f, 0.5f)]
		public float spriteHeight = 0.25f;

		// Token: 0x04002A6F RID: 10863
		[Tooltip("Offset for the camera height")]
		public float cameraHeightOffset;

		// Token: 0x04002A70 RID: 10864
		[Tooltip("Transition Speed for the Camera")]
		public float lockSpeed = 0.5f;

		// Token: 0x04002A71 RID: 10865
		[Header("LockOn Inputs")]
		public GenericInput lockOnInput = new GenericInput("Tab", "RightStickClick", "RightStickClick");

		// Token: 0x04002A72 RID: 10866
		public GenericInput nexTargetInput = new GenericInput("X", false, false, "RightAnalogHorizontal", true, false, "X", false, false);

		// Token: 0x04002A73 RID: 10867
		public GenericInput previousTargetInput = new GenericInput("Z", false, false, "RightAnalogHorizontal", true, true, "Z", false, false);

		// Token: 0x04002A74 RID: 10868
		internal bool isLockingOn;

		// Token: 0x04002A75 RID: 10869
		public vLockOn.LockOnEvent onLockOnTarget;

		// Token: 0x04002A76 RID: 10870
		public vLockOn.LockOnEvent onUnLockOnTarget;

		// Token: 0x04002A77 RID: 10871
		protected Canvas _aimCanvas;

		// Token: 0x04002A78 RID: 10872
		protected RectTransform _aimImage;

		// Token: 0x04002A79 RID: 10873
		protected bool _inTarget;

		// Token: 0x04002A7A RID: 10874
		protected vThirdPersonInput tpInput;

		// Token: 0x020006BC RID: 1724
		[Serializable]
		public class LockOnEvent : UnityEvent<Transform>
		{
		}
	}
}
