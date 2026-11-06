using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000219 RID: 537
	public class MMFloatingText : MonoBehaviour
	{
		// Token: 0x06000EBC RID: 3772 RVA: 0x0004199F File Offset: 0x0003FB9F
		public virtual float GetTime()
		{
			if (!this._useUnscaledTime)
			{
				return Time.time;
			}
			return Time.unscaledTime;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x000419B4 File Offset: 0x0003FBB4
		public virtual float GetDeltaTime()
		{
			if (!this._useUnscaledTime)
			{
				return Time.unscaledTime;
			}
			return Time.unscaledDeltaTime;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x000419C9 File Offset: 0x0003FBC9
		protected virtual void OnEnable()
		{
			this.Initialization();
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x000419D1 File Offset: 0x0003FBD1
		public virtual void SetUseUnscaledTime(bool status, bool resetStartedAt)
		{
			this._useUnscaledTime = status;
			if (resetStartedAt)
			{
				this._startedAt = this.GetTime();
			}
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x000419E9 File Offset: 0x0003FBE9
		protected virtual void Initialization()
		{
			this._startedAt = this.GetTime();
			if (this.TargetTextMesh != null)
			{
				this._initialTextColor = this.TargetTextMesh.color;
			}
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00041A16 File Offset: 0x0003FC16
		protected virtual void Update()
		{
			this.UpdateFloatingText();
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00041A20 File Offset: 0x0003FC20
		protected virtual void UpdateFloatingText()
		{
			this._elapsedTime = this.GetTime() - this._startedAt;
			this._remappedTime = MMMaths.Remap(this._elapsedTime, 0f, this._lifetime, 0f, 1f);
			if (this._elapsedTime > this._lifetime)
			{
				this.TurnOff();
			}
			this.HandleMovement();
			this.HandleColor();
			this.HandleOpacity();
			this.HandleScale();
			this.HandleAlignment();
			this.HandleBillboard();
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00041AA0 File Offset: 0x0003FCA0
		protected virtual void HandleMovement()
		{
			if (this._animateMovement)
			{
				base.transform.up = this.Direction;
				this._newPosition.x = (this._animateX ? MMMaths.Remap(this._animateXCurve.Evaluate(this._remappedTime), 0f, 1f, this._remapXZero, this._remapXOne) : 0f);
				this._newPosition.y = (this._animateY ? MMMaths.Remap(this._animateYCurve.Evaluate(this._remappedTime), 0f, 1f, this._remapYZero, this._remapYOne) : 0f);
				this._newPosition.z = (this._animateZ ? MMMaths.Remap(this._animateZCurve.Evaluate(this._remappedTime), 0f, 1f, this._remapZZero, this._remapZOne) : 0f);
				this.MovingPart.transform.localPosition = this._newPosition;
				if (Vector3.Distance(this._movingPartPositionLastFrame, this.MovingPart.position) > 0.5f)
				{
					this._movingPartPositionLastFrame = this.MovingPart.position;
				}
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00041BDF File Offset: 0x0003FDDF
		protected virtual void HandleColor()
		{
			if (this._animateColor)
			{
				this._newColor = this._animateColorGradient.Evaluate(this._remappedTime);
				this.SetColor(this._newColor);
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00041C0C File Offset: 0x0003FE0C
		protected virtual void HandleOpacity()
		{
			if (this._animateOpacity)
			{
				float num = MMMaths.Remap(this._animateOpacityCurve.Evaluate(this._remappedTime), 0f, 1f, this._remapOpacityZero, this._remapOpacityOne);
				this.SetOpacity(num);
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00041C58 File Offset: 0x0003FE58
		protected virtual void HandleScale()
		{
			if (this._animateScale)
			{
				this._newScale = Vector3.one * MMMaths.Remap(this._animateScaleCurve.Evaluate(this._remappedTime), 0f, 1f, this._remapScaleZero, this._remapScaleOne);
				this.MovingPart.transform.localScale = this._newScale;
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00041CC0 File Offset: 0x0003FEC0
		protected virtual void HandleAlignment()
		{
			if (this._alignmentMode == MMFloatingTextSpawner.AlignmentModes.Fixed)
			{
				this.MovingPart.transform.up = this._fixedAlignment;
				return;
			}
			if (this._alignmentMode == MMFloatingTextSpawner.AlignmentModes.MatchInitialDirection)
			{
				this.MovingPart.transform.up = base.transform.up;
				return;
			}
			if (this._alignmentMode == MMFloatingTextSpawner.AlignmentModes.MatchMovementDirection)
			{
				this._movementDirection = this.MovingPart.position - this._movingPartPositionLastFrame;
				this.MovingPart.transform.up = this._movementDirection.normalized;
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00041D54 File Offset: 0x0003FF54
		protected virtual void HandleBillboard()
		{
			if (this._alwaysFaceCamera)
			{
				this._targetCameraRotation = this._targetCamera.transform.rotation;
				this.Billboard.transform.LookAt(this.MovingPart.transform.position + this._targetCameraRotation * Vector3.forward, this._targetCameraRotation * this.MovingPart.up);
			}
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00041DCC File Offset: 0x0003FFCC
		public virtual void SetProperties(string value, float lifetime, Vector3 direction, bool animateMovement, MMFloatingTextSpawner.AlignmentModes alignmentMode, Vector3 fixedAlignment, bool alwaysFaceCamera, Camera targetCamera, bool animateX, AnimationCurve animateXCurve, float remapXZero, float remapXOne, bool animateY, AnimationCurve animateYCurve, float remapYZero, float remapYOne, bool animateZ, AnimationCurve animateZCurve, float remapZZero, float remapZOne, bool animateOpacity, AnimationCurve animateOpacityCurve, float remapOpacityZero, float remapOpacityOne, bool animateScale, AnimationCurve animateScaleCurve, float remapScaleZero, float remapScaleOne, bool animateColor, Gradient animateColorGradient)
		{
			this.SetText(value);
			this._lifetime = lifetime;
			this.Direction = direction;
			this._animateMovement = animateMovement;
			this._animateX = animateX;
			this._animateXCurve = animateXCurve;
			this._remapXZero = remapXZero;
			this._remapXOne = remapXOne;
			this._animateY = animateY;
			this._animateYCurve = animateYCurve;
			this._remapYZero = remapYZero;
			this._remapYOne = remapYOne;
			this._animateZ = animateZ;
			this._animateZCurve = animateZCurve;
			this._remapZZero = remapZZero;
			this._remapZOne = remapZOne;
			this._alignmentMode = alignmentMode;
			this._fixedAlignment = fixedAlignment;
			this._alwaysFaceCamera = alwaysFaceCamera;
			this._targetCamera = targetCamera;
			this._animateOpacity = animateOpacity;
			this._animateOpacityCurve = animateOpacityCurve;
			this._remapOpacityZero = remapOpacityZero;
			this._remapOpacityOne = remapOpacityOne;
			this._animateScale = animateScale;
			this._animateScaleCurve = animateScaleCurve;
			this._remapScaleZero = remapScaleZero;
			this._remapScaleOne = remapScaleOne;
			this._animateColor = animateColor;
			this._animateColorGradient = animateColorGradient;
			this.UpdateFloatingText();
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00041ECC File Offset: 0x000400CC
		public virtual void ResetPosition()
		{
			if (this._animateMovement)
			{
				this.MovingPart.transform.localPosition = Vector3.zero;
			}
			this._movingPartPositionLastFrame = this.MovingPart.position - this.Direction;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00041F07 File Offset: 0x00040107
		public virtual void SetText(string newValue)
		{
			this.TargetTextMesh.text = newValue;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00041F15 File Offset: 0x00040115
		public virtual void SetColor(Color newColor)
		{
			this.TargetTextMesh.color = newColor;
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00041F23 File Offset: 0x00040123
		public virtual void SetOpacity(float newOpacity)
		{
			this._newColor = this.TargetTextMesh.color;
			this._newColor.a = newOpacity;
			this.TargetTextMesh.color = this._newColor;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00041F53 File Offset: 0x00040153
		protected virtual void TurnOff()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04001113 RID: 4371
		[Header("Bindings")]
		[Tooltip("the part of the prefab that we'll move")]
		public Transform MovingPart;

		// Token: 0x04001114 RID: 4372
		[Tooltip("the part of the prefab that we'll rotate to face the target camera")]
		public Transform Billboard;

		// Token: 0x04001115 RID: 4373
		[Tooltip("the TextMesh used to display the value")]
		public TextMesh TargetTextMesh;

		// Token: 0x04001116 RID: 4374
		[Header("Debug")]
		[Tooltip("the direction of this floating text, used for debug only")]
		[MMReadOnly]
		public Vector3 Direction = Vector3.up;

		// Token: 0x04001117 RID: 4375
		protected bool _useUnscaledTime;

		// Token: 0x04001118 RID: 4376
		protected float _startedAt;

		// Token: 0x04001119 RID: 4377
		protected float _lifetime;

		// Token: 0x0400111A RID: 4378
		protected Vector3 _newPosition;

		// Token: 0x0400111B RID: 4379
		protected Color _initialTextColor;

		// Token: 0x0400111C RID: 4380
		protected bool _animateMovement;

		// Token: 0x0400111D RID: 4381
		protected bool _animateX;

		// Token: 0x0400111E RID: 4382
		protected AnimationCurve _animateXCurve;

		// Token: 0x0400111F RID: 4383
		protected float _remapXZero;

		// Token: 0x04001120 RID: 4384
		protected float _remapXOne;

		// Token: 0x04001121 RID: 4385
		protected bool _animateY;

		// Token: 0x04001122 RID: 4386
		protected AnimationCurve _animateYCurve;

		// Token: 0x04001123 RID: 4387
		protected float _remapYZero;

		// Token: 0x04001124 RID: 4388
		protected float _remapYOne;

		// Token: 0x04001125 RID: 4389
		protected bool _animateZ;

		// Token: 0x04001126 RID: 4390
		protected AnimationCurve _animateZCurve;

		// Token: 0x04001127 RID: 4391
		protected float _remapZZero;

		// Token: 0x04001128 RID: 4392
		protected float _remapZOne;

		// Token: 0x04001129 RID: 4393
		protected MMFloatingTextSpawner.AlignmentModes _alignmentMode;

		// Token: 0x0400112A RID: 4394
		protected Vector3 _fixedAlignment;

		// Token: 0x0400112B RID: 4395
		protected Vector3 _movementDirection;

		// Token: 0x0400112C RID: 4396
		protected Vector3 _movingPartPositionLastFrame;

		// Token: 0x0400112D RID: 4397
		protected bool _alwaysFaceCamera;

		// Token: 0x0400112E RID: 4398
		protected Camera _targetCamera;

		// Token: 0x0400112F RID: 4399
		protected Quaternion _targetCameraRotation;

		// Token: 0x04001130 RID: 4400
		protected bool _animateOpacity;

		// Token: 0x04001131 RID: 4401
		protected AnimationCurve _animateOpacityCurve;

		// Token: 0x04001132 RID: 4402
		protected float _remapOpacityZero;

		// Token: 0x04001133 RID: 4403
		protected float _remapOpacityOne;

		// Token: 0x04001134 RID: 4404
		protected bool _animateScale;

		// Token: 0x04001135 RID: 4405
		protected AnimationCurve _animateScaleCurve;

		// Token: 0x04001136 RID: 4406
		protected float _remapScaleZero;

		// Token: 0x04001137 RID: 4407
		protected float _remapScaleOne;

		// Token: 0x04001138 RID: 4408
		protected bool _animateColor;

		// Token: 0x04001139 RID: 4409
		protected Gradient _animateColorGradient;

		// Token: 0x0400113A RID: 4410
		protected Vector3 _newScale;

		// Token: 0x0400113B RID: 4411
		protected Color _newColor;

		// Token: 0x0400113C RID: 4412
		protected float _elapsedTime;

		// Token: 0x0400113D RID: 4413
		protected float _remappedTime;
	}
}
