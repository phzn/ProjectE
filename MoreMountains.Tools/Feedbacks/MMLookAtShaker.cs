using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200024D RID: 589
	public class MMLookAtShaker : MMShaker
	{
		// Token: 0x06001033 RID: 4147 RVA: 0x00047EA0 File Offset: 0x000460A0
		protected override void Initialization()
		{
			base.Initialization();
			if (this.TransformToRotate == null)
			{
				this.TransformToRotate = base.transform;
			}
			this._initialRotation = this.TransformToRotate.rotation;
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x00047ED3 File Offset: 0x000460D3
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.5f;
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00047EE0 File Offset: 0x000460E0
		protected override void Shake()
		{
			this.ApplyRotation(this._journey);
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x00047EEE File Offset: 0x000460EE
		protected override void ShakeComplete()
		{
			this.ApplyRotation(1f);
			base.ShakeComplete();
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x00047F04 File Offset: 0x00046104
		protected virtual void ApplyRotation(float journey)
		{
			float num = Mathf.Clamp01(journey / this.ShakeDuration);
			num = this.LookAtTween.Evaluate(num);
			switch (this.LookAtTargetMode)
			{
			case MMF_LookAt.LookAtTargetModes.Transform:
				this._lookAtPosition = this.LookAtTarget.position;
				break;
			case MMF_LookAt.LookAtTargetModes.TargetWorldPosition:
				this._lookAtPosition = this.LookAtTargetWorldPosition;
				break;
			case MMF_LookAt.LookAtTargetModes.Direction:
				this._lookAtPosition = this.TransformToRotate.position + this.LookAtDirection;
				break;
			}
			if (this.LockXAxis)
			{
				this._lookAtPosition.x = this.TransformToRotate.position.x;
			}
			if (this.LockYAxis)
			{
				this._lookAtPosition.y = this.TransformToRotate.position.y;
			}
			if (this.LockZAxis)
			{
				this._lookAtPosition.z = this.TransformToRotate.position.z;
			}
			this._direction = this._lookAtPosition - this.TransformToRotate.position;
			this._newRotation = Quaternion.LookRotation(this._direction, this._upwards);
			this.TransformToRotate.transform.rotation = Quaternion.SlerpUnclamped(this._initialRotation, this._newRotation, num);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x00048044 File Offset: 0x00046244
		public virtual void OnMMLookAtShakeEvent(float duration, bool lockXAxis, bool lockYAxis, bool lockZAxis, MMF_LookAt.UpwardVectors upwardVector, MMF_LookAt.LookAtTargetModes lookAtTargetMode, Transform lookAtTarget, Vector3 lookAtTargetWorldPosition, Vector3 lookAtDirection, Transform transformToRotate, MMTweenType lookAtTween, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false)
		{
			if (!this.CheckEventAllowed(channelData, useRange, rangeDistance, rangePosition) || (!this.Interruptible && this.Shaking))
			{
				return;
			}
			if (stop)
			{
				this.Stop();
				return;
			}
			this._resetShakerValuesAfterShake = resetShakerValuesAfterShake;
			this._resetTargetValuesAfterShake = resetTargetValuesAfterShake;
			if (resetShakerValuesAfterShake)
			{
				this._originalDuration = this.ShakeDuration;
				this._originalLookAtTween = this.LookAtTween;
				this._originalLockXAxis = this.LockXAxis;
				this._originalLockYAxis = this.LockYAxis;
				this._originalLockZAxis = this.LockZAxis;
				this._originalUpwardVector = this.UpwardVector;
				this._originalLookAtTargetMode = this.LookAtTargetMode;
				this._originalLookAtTarget = this.LookAtTarget;
				this._originalLookAtTargetWorldPosition = this.LookAtTargetWorldPosition;
				this._originalLookAtDirection = this.LookAtDirection;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.LookAtTween = lookAtTween;
				this.LockXAxis = lockXAxis;
				this.LockYAxis = lockYAxis;
				this.LockZAxis = lockZAxis;
				this.UpwardVector = upwardVector;
				this.LookAtTargetMode = lookAtTargetMode;
				this.LookAtTarget = lookAtTarget;
				this.LookAtTargetWorldPosition = lookAtTargetWorldPosition;
				this.LookAtDirection = lookAtDirection;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00048174 File Offset: 0x00046374
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this.TransformToRotate.rotation = this._initialRotation;
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x00048190 File Offset: 0x00046390
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalDuration;
			this.LookAtTween = this._originalLookAtTween;
			this.LockXAxis = this._originalLockXAxis;
			this.LockYAxis = this._originalLockYAxis;
			this.LockZAxis = this._originalLockZAxis;
			this.UpwardVector = this._originalUpwardVector;
			this.LookAtTargetMode = this._originalLookAtTargetMode;
			this.LookAtTarget = this._originalLookAtTarget;
			this.LookAtTargetWorldPosition = this._originalLookAtTargetWorldPosition;
			this.LookAtDirection = this._originalLookAtDirection;
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0004821B File Offset: 0x0004641B
		public override void StartListening()
		{
			base.StartListening();
			MMLookAtShaker.MMLookAtShakeEvent.Register(new MMLookAtShaker.MMLookAtShakeEvent.Delegate(this.OnMMLookAtShakeEvent));
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x00048235 File Offset: 0x00046435
		public override void StopListening()
		{
			base.StopListening();
			MMLookAtShaker.MMLookAtShakeEvent.Unregister(new MMLookAtShaker.MMLookAtShakeEvent.Delegate(this.OnMMLookAtShakeEvent));
		}

		// Token: 0x040012BC RID: 4796
		[MMInspectorGroup("Look at settings", true, 37, false)]
		[Tooltip("the duration of this shake, in seconds")]
		public float Duration = 1f;

		// Token: 0x040012BD RID: 4797
		[Tooltip("the curve over which to animate the look at transition")]
		public MMTweenType LookAtTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x040012BE RID: 4798
		[Tooltip("whether or not to lock rotation on the x axis")]
		public bool LockXAxis;

		// Token: 0x040012BF RID: 4799
		[Tooltip("whether or not to lock rotation on the y axis")]
		public bool LockYAxis;

		// Token: 0x040012C0 RID: 4800
		[Tooltip("whether or not to lock rotation on the z axis")]
		public bool LockZAxis;

		// Token: 0x040012C1 RID: 4801
		[MMInspectorGroup("What we want to rotate", true, 37, false)]
		[Tooltip("in Direct mode, the transform to rotate to have it look at our target - if left empty, will be the transform this shaker is on")]
		public Transform TransformToRotate;

		// Token: 0x040012C2 RID: 4802
		public MMF_LookAt.UpwardVectors UpwardVector = MMF_LookAt.UpwardVectors.Up;

		// Token: 0x040012C3 RID: 4803
		[MMInspectorGroup("What we want to look at", true, 37, false)]
		[Tooltip("the different target modes : either a specific transform to look at, the coordinates of a world position, or a direction vector")]
		public MMF_LookAt.LookAtTargetModes LookAtTargetMode;

		// Token: 0x040012C4 RID: 4804
		[Tooltip("the transform we want to look at")]
		[MMFEnumCondition("LookAtTargetMode", new int[] { 0 })]
		public Transform LookAtTarget;

		// Token: 0x040012C5 RID: 4805
		[Tooltip("the coordinates of a point the world that we want to look at")]
		[MMFEnumCondition("LookAtTargetMode", new int[] { 1 })]
		public Vector3 LookAtTargetWorldPosition = Vector3.forward;

		// Token: 0x040012C6 RID: 4806
		[Tooltip("a direction (from our rotating object) that we want to look at")]
		[MMFEnumCondition("LookAtTargetMode", new int[] { 2 })]
		public Vector3 LookAtDirection = Vector3.forward;

		// Token: 0x040012C7 RID: 4807
		[MMInspectorGroup("Test", true, 46, false)]
		[MMInspectorButton("StartShaking")]
		public bool StartShakingButton;

		// Token: 0x040012C8 RID: 4808
		protected Quaternion _newRotation;

		// Token: 0x040012C9 RID: 4809
		protected Vector3 _lookAtPosition;

		// Token: 0x040012CA RID: 4810
		protected Vector3 _upwards;

		// Token: 0x040012CB RID: 4811
		protected Vector3 _direction;

		// Token: 0x040012CC RID: 4812
		protected Quaternion _initialRotation;

		// Token: 0x040012CD RID: 4813
		protected float _originalDuration = 1f;

		// Token: 0x040012CE RID: 4814
		protected MMTweenType _originalLookAtTween;

		// Token: 0x040012CF RID: 4815
		protected bool _originalLockXAxis;

		// Token: 0x040012D0 RID: 4816
		protected bool _originalLockYAxis;

		// Token: 0x040012D1 RID: 4817
		protected bool _originalLockZAxis;

		// Token: 0x040012D2 RID: 4818
		protected MMF_LookAt.UpwardVectors _originalUpwardVector;

		// Token: 0x040012D3 RID: 4819
		protected MMF_LookAt.LookAtTargetModes _originalLookAtTargetMode;

		// Token: 0x040012D4 RID: 4820
		protected Transform _originalLookAtTarget;

		// Token: 0x040012D5 RID: 4821
		protected Vector3 _originalLookAtTargetWorldPosition;

		// Token: 0x040012D6 RID: 4822
		protected Vector3 _originalLookAtDirection;

		// Token: 0x0200051E RID: 1310
		public struct MMLookAtShakeEvent
		{
			// Token: 0x14000050 RID: 80
			// (add) Token: 0x06001C7E RID: 7294 RVA: 0x00073A74 File Offset: 0x00071C74
			// (remove) Token: 0x06001C7F RID: 7295 RVA: 0x00073AA8 File Offset: 0x00071CA8
			private static event MMLookAtShaker.MMLookAtShakeEvent.Delegate OnEvent;

			// Token: 0x06001C80 RID: 7296 RVA: 0x00073ADB File Offset: 0x00071CDB
			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void RuntimeInitialization()
			{
				MMLookAtShaker.MMLookAtShakeEvent.OnEvent = null;
			}

			// Token: 0x06001C81 RID: 7297 RVA: 0x00073AE3 File Offset: 0x00071CE3
			public static void Register(MMLookAtShaker.MMLookAtShakeEvent.Delegate callback)
			{
				MMLookAtShaker.MMLookAtShakeEvent.OnEvent += callback;
			}

			// Token: 0x06001C82 RID: 7298 RVA: 0x00073AEB File Offset: 0x00071CEB
			public static void Unregister(MMLookAtShaker.MMLookAtShakeEvent.Delegate callback)
			{
				MMLookAtShaker.MMLookAtShakeEvent.OnEvent -= callback;
			}

			// Token: 0x06001C83 RID: 7299 RVA: 0x00073AF4 File Offset: 0x00071CF4
			public static void Trigger(float duration, bool lockXAxis, bool lockYAxis, bool lockZAxis, MMF_LookAt.UpwardVectors upwardVector, MMF_LookAt.LookAtTargetModes lookAtTargetMode, Transform lookAtTarget, Vector3 lookAtTargetWorldPosition, Vector3 lookAtDirection, Transform transformToRotate, MMTweenType lookAtTween, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false)
			{
				MMLookAtShaker.MMLookAtShakeEvent.Delegate onEvent = MMLookAtShaker.MMLookAtShakeEvent.OnEvent;
				if (onEvent == null)
				{
					return;
				}
				onEvent(duration, lockXAxis, lockYAxis, lockZAxis, upwardVector, lookAtTargetMode, lookAtTarget, lookAtTargetWorldPosition, lookAtDirection, transformToRotate, lookAtTween, useRange, rangeDistance, useRangeFalloff, rangeFalloff, remapRangeFalloff, rangePosition, feedbacksIntensity, channelData, resetShakerValuesAfterShake, resetTargetValuesAfterShake, forwardDirection, timescaleMode, stop);
			}

			// Token: 0x020005A7 RID: 1447
			// (Invoke) Token: 0x06001DFB RID: 7675
			public delegate void Delegate(float duration, bool lockXAxis, bool lockYAxis, bool lockZAxis, MMF_LookAt.UpwardVectors upwardVector, MMF_LookAt.LookAtTargetModes lookAtTargetMode, Transform lookAtTarget, Vector3 lookAtTargetWorldPosition, Vector3 lookAtDirection, Transform transformToRotate, MMTweenType lookAtTween, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false);
		}
	}
}
