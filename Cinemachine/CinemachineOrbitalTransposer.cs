using System;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x02000027 RID: 39
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class CinemachineOrbitalTransposer : CinemachineTransposer
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x0000E584 File Offset: 0x0000C784
		protected override void OnValidate()
		{
			if (this.m_LegacyRadius != 3.4028235E+38f && this.m_LegacyHeightOffset != 3.4028235E+38f && this.m_LegacyHeadingBias != 3.4028235E+38f)
			{
				this.m_FollowOffset = new Vector3(0f, this.m_LegacyHeightOffset, -this.m_LegacyRadius);
				this.m_LegacyHeightOffset = (this.m_LegacyRadius = float.MaxValue);
				this.m_Heading.m_Bias = this.m_LegacyHeadingBias;
				this.m_XAxis.m_MaxSpeed = this.m_XAxis.m_MaxSpeed / 10f;
				this.m_XAxis.m_AccelTime = this.m_XAxis.m_AccelTime / 10f;
				this.m_XAxis.m_DecelTime = this.m_XAxis.m_DecelTime / 10f;
				this.m_LegacyHeadingBias = float.MaxValue;
				int definition = (int)this.m_Heading.m_Definition;
				if (this.m_RecenterToTargetHeading.LegacyUpgrade(ref definition, ref this.m_Heading.m_VelocityFilterStrength))
				{
					this.m_Heading.m_Definition = (CinemachineOrbitalTransposer.Heading.HeadingDefinition)definition;
				}
			}
			this.m_XAxis.Validate();
			this.m_RecenterToTargetHeading.Validate();
			base.OnValidate();
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000E698 File Offset: 0x0000C898
		public float UpdateHeading(float deltaTime, Vector3 up, ref AxisState axis)
		{
			return this.UpdateHeading(deltaTime, up, ref axis, ref this.m_RecenterToTargetHeading, true);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000E6AC File Offset: 0x0000C8AC
		public float UpdateHeading(float deltaTime, Vector3 up, ref AxisState axis, ref AxisState.Recentering recentering, bool isLive)
		{
			if (this.m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
			{
				axis.m_MinValue = -180f;
				axis.m_MaxValue = 180f;
			}
			if (deltaTime < 0f || !base.VirtualCamera.PreviousStateIsValid || !isLive)
			{
				axis.Reset();
				recentering.CancelRecentering();
			}
			else if (axis.Update(deltaTime))
			{
				recentering.CancelRecentering();
			}
			if (this.m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
			{
				float value = axis.Value;
				axis.Value = 0f;
				return value;
			}
			float targetHeading = this.GetTargetHeading(axis.Value, base.GetReferenceOrientation(up));
			recentering.DoRecentering(ref axis, deltaTime, targetHeading);
			return axis.Value;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000E74F File Offset: 0x0000C94F
		private void OnEnable()
		{
			this.m_PreviousTarget = null;
			this.m_LastTargetPosition = Vector3.zero;
			this.UpdateInputAxisProvider();
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000E76C File Offset: 0x0000C96C
		public void UpdateInputAxisProvider()
		{
			this.m_XAxis.SetInputAxisProvider(0, null);
			if (!this.m_HeadingIsSlave && base.VirtualCamera != null)
			{
				AxisState.IInputAxisProvider inputAxisProvider = base.VirtualCamera.GetInputAxisProvider();
				if (inputAxisProvider != null)
				{
					this.m_XAxis.SetInputAxisProvider(0, inputAxisProvider);
				}
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
		public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
			base.OnTargetObjectWarped(target, positionDelta);
			if (target == base.FollowTarget)
			{
				this.m_LastTargetPosition += positionDelta;
				this.m_LastCameraPosition += positionDelta;
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000E7F4 File Offset: 0x0000C9F4
		public override void ForceCameraPosition(Vector3 pos, Quaternion rot)
		{
			base.ForceCameraPosition(pos, rot);
			this.m_LastCameraPosition = pos;
			this.m_XAxis.Value = this.GetAxisClosestValue(pos, base.VirtualCamera.State.ReferenceUp);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000E828 File Offset: 0x0000CA28
		public override bool OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime, ref CinemachineVirtualCameraBase.TransitionParams transitionParams)
		{
			this.m_RecenterToTargetHeading.DoRecentering(ref this.m_XAxis, -1f, 0f);
			this.m_RecenterToTargetHeading.CancelRecentering();
			if (fromCam != null && this.m_BindingMode != CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp && transitionParams.m_InheritPosition && !CinemachineCore.Instance.IsLiveInBlend(base.VirtualCamera))
			{
				this.m_XAxis.Value = this.GetAxisClosestValue(fromCam.State.RawPosition, worldUp);
				return true;
			}
			return false;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000E8A4 File Offset: 0x0000CAA4
		public float GetAxisClosestValue(Vector3 cameraPos, Vector3 up)
		{
			Quaternion quaternion = base.GetReferenceOrientation(up);
			if (!(quaternion * Vector3.forward).ProjectOntoPlane(up).AlmostZero() && base.FollowTarget != null)
			{
				float num = 0f;
				if (this.m_BindingMode != CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
				{
					num += this.m_Heading.m_Bias;
				}
				quaternion *= Quaternion.AngleAxis(num, up);
				Vector3 followTargetPosition = base.FollowTargetPosition;
				Vector3 vector = (followTargetPosition + quaternion * base.EffectiveOffset - followTargetPosition).ProjectOntoPlane(up);
				Vector3 vector2 = (cameraPos - followTargetPosition).ProjectOntoPlane(up);
				return Vector3.SignedAngle(vector, vector2, up);
			}
			return this.m_LastHeading;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000E94C File Offset: 0x0000CB4C
		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			base.InitPrevFrameStateInfo(ref curState, deltaTime);
			if (base.FollowTarget != this.m_PreviousTarget)
			{
				this.m_PreviousTarget = base.FollowTarget;
				this.m_TargetRigidBody = ((this.m_PreviousTarget == null) ? null : this.m_PreviousTarget.GetComponent<Rigidbody>());
				this.m_LastTargetPosition = ((this.m_PreviousTarget == null) ? Vector3.zero : this.m_PreviousTarget.position);
				this.mHeadingTracker = null;
			}
			this.m_LastHeading = this.HeadingUpdater(this, deltaTime, curState.ReferenceUp);
			float num = this.m_LastHeading;
			if (this.IsValid)
			{
				if (this.m_BindingMode != CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
				{
					num += this.m_Heading.m_Bias;
				}
				Quaternion quaternion = Quaternion.AngleAxis(num, Vector3.up);
				Vector3 effectiveOffset = base.EffectiveOffset;
				Vector3 vector = quaternion * effectiveOffset;
				Vector3 vector2;
				Quaternion quaternion2;
				base.TrackTarget(deltaTime, curState.ReferenceUp, vector, out vector2, out quaternion2);
				vector = quaternion2 * vector;
				curState.ReferenceUp = quaternion2 * Vector3.up;
				Vector3 followTargetPosition = base.FollowTargetPosition;
				vector2 += base.GetOffsetForMinimumTargetDistance(vector2, vector, curState.RawOrientation * Vector3.forward, curState.ReferenceUp, followTargetPosition);
				curState.RawPosition = vector2 + vector;
				if (deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid)
				{
					Vector3 vector3 = followTargetPosition;
					if (base.LookAtTarget != null)
					{
						vector3 = base.LookAtTargetPosition;
					}
					Vector3 vector4 = this.m_LastCameraPosition - vector3;
					Vector3 vector5 = curState.RawPosition - vector3;
					if (vector4.sqrMagnitude > 0.01f && vector5.sqrMagnitude > 0.01f)
					{
						curState.PositionDampingBypass = UnityVectorExtensions.SafeFromToRotation(vector4, vector5, curState.ReferenceUp).eulerAngles;
					}
				}
				this.m_LastTargetPosition = followTargetPosition;
				this.m_LastCameraPosition = curState.RawPosition;
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000EB30 File Offset: 0x0000CD30
		public override Vector3 GetTargetCameraPosition(Vector3 worldUp)
		{
			if (!this.IsValid)
			{
				return Vector3.zero;
			}
			float num = this.m_LastHeading;
			if (this.m_BindingMode != CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
			{
				num += this.m_Heading.m_Bias;
			}
			Quaternion quaternion = Quaternion.AngleAxis(num, Vector3.up);
			quaternion = base.GetReferenceOrientation(worldUp) * quaternion;
			return quaternion * base.EffectiveOffset + this.m_LastTargetPosition;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000EB9A File Offset: 0x0000CD9A
		public override bool RequiresUserInput
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000EBA0 File Offset: 0x0000CDA0
		private float GetTargetHeading(float currentHeading, Quaternion targetOrientation)
		{
			if (this.m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
			{
				return 0f;
			}
			if (base.FollowTarget == null)
			{
				return currentHeading;
			}
			CinemachineOrbitalTransposer.Heading.HeadingDefinition headingDefinition = this.m_Heading.m_Definition;
			if (headingDefinition == CinemachineOrbitalTransposer.Heading.HeadingDefinition.Velocity && this.m_TargetRigidBody == null)
			{
				headingDefinition = CinemachineOrbitalTransposer.Heading.HeadingDefinition.PositionDelta;
			}
			Vector3 vector = Vector3.zero;
			switch (headingDefinition)
			{
			case CinemachineOrbitalTransposer.Heading.HeadingDefinition.PositionDelta:
				vector = base.FollowTargetPosition - this.m_LastTargetPosition;
				goto IL_0098;
			case CinemachineOrbitalTransposer.Heading.HeadingDefinition.Velocity:
				vector = this.m_TargetRigidBody.velocity;
				goto IL_0098;
			case CinemachineOrbitalTransposer.Heading.HeadingDefinition.TargetForward:
				vector = base.FollowTargetRotation * Vector3.forward;
				goto IL_0098;
			}
			return 0f;
			IL_0098:
			Vector3 vector2 = targetOrientation * Vector3.up;
			vector = vector.ProjectOntoPlane(vector2);
			if (headingDefinition != CinemachineOrbitalTransposer.Heading.HeadingDefinition.TargetForward)
			{
				int num = this.m_Heading.m_VelocityFilterStrength * 5;
				if (this.mHeadingTracker == null || this.mHeadingTracker.FilterSize != num)
				{
					this.mHeadingTracker = new HeadingTracker(num);
				}
				this.mHeadingTracker.DecayHistory();
				if (!vector.AlmostZero())
				{
					this.mHeadingTracker.Add(vector);
				}
				vector = this.mHeadingTracker.GetReliableHeading();
			}
			if (!vector.AlmostZero())
			{
				return UnityVectorExtensions.SignedAngle(targetOrientation * Vector3.forward, vector, vector2);
			}
			return currentHeading;
		}

		// Token: 0x04000157 RID: 343
		[Space]
		[OrbitalTransposerHeadingProperty]
		[Tooltip("The definition of Forward.  Camera will follow behind.")]
		public CinemachineOrbitalTransposer.Heading m_Heading = new CinemachineOrbitalTransposer.Heading(CinemachineOrbitalTransposer.Heading.HeadingDefinition.TargetForward, 4, 0f);

		// Token: 0x04000158 RID: 344
		[Tooltip("Automatic heading recentering.  The settings here defines how the camera will reposition itself in the absence of player input.")]
		public AxisState.Recentering m_RecenterToTargetHeading = new AxisState.Recentering(true, 1f, 2f);

		// Token: 0x04000159 RID: 345
		[Tooltip("Heading Control.  The settings here control the behaviour of the camera in response to the player's input.")]
		[AxisStateProperty]
		public AxisState m_XAxis = new AxisState(-180f, 180f, true, false, 300f, 0.1f, 0.1f, "Mouse X", true);

		// Token: 0x0400015A RID: 346
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("m_Radius")]
		private float m_LegacyRadius = float.MaxValue;

		// Token: 0x0400015B RID: 347
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("m_HeightOffset")]
		private float m_LegacyHeightOffset = float.MaxValue;

		// Token: 0x0400015C RID: 348
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("m_HeadingBias")]
		private float m_LegacyHeadingBias = float.MaxValue;

		// Token: 0x0400015D RID: 349
		[HideInInspector]
		[NoSaveDuringPlay]
		public bool m_HeadingIsSlave;

		// Token: 0x0400015E RID: 350
		internal CinemachineOrbitalTransposer.UpdateHeadingDelegate HeadingUpdater = (CinemachineOrbitalTransposer orbital, float deltaTime, Vector3 up) => orbital.UpdateHeading(deltaTime, up, ref orbital.m_XAxis, ref orbital.m_RecenterToTargetHeading, CinemachineCore.Instance.IsLive(orbital.VirtualCamera));

		// Token: 0x0400015F RID: 351
		private Vector3 m_LastTargetPosition = Vector3.zero;

		// Token: 0x04000160 RID: 352
		private HeadingTracker mHeadingTracker;

		// Token: 0x04000161 RID: 353
		private Rigidbody m_TargetRigidBody;

		// Token: 0x04000162 RID: 354
		private Transform m_PreviousTarget;

		// Token: 0x04000163 RID: 355
		private Vector3 m_LastCameraPosition;

		// Token: 0x04000164 RID: 356
		private float m_LastHeading;

		// Token: 0x02000098 RID: 152
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		[Serializable]
		public struct Heading
		{
			// Token: 0x0600043F RID: 1087 RVA: 0x000190DA File Offset: 0x000172DA
			public Heading(CinemachineOrbitalTransposer.Heading.HeadingDefinition def, int filterStrength, float bias)
			{
				this.m_Definition = def;
				this.m_VelocityFilterStrength = filterStrength;
				this.m_Bias = bias;
			}

			// Token: 0x04000331 RID: 817
			[FormerlySerializedAs("m_HeadingDefinition")]
			[Tooltip("How 'forward' is defined.  The camera will be placed by default behind the target.  PositionDelta will consider 'forward' to be the direction in which the target is moving.")]
			public CinemachineOrbitalTransposer.Heading.HeadingDefinition m_Definition;

			// Token: 0x04000332 RID: 818
			[Range(0f, 10f)]
			[Tooltip("Size of the velocity sampling window for target heading filter.  This filters out irregularities in the target's movement.  Used only if deriving heading from target's movement (PositionDelta or Velocity)")]
			public int m_VelocityFilterStrength;

			// Token: 0x04000333 RID: 819
			[Range(-180f, 180f)]
			[FormerlySerializedAs("m_HeadingBias")]
			[Tooltip("Where the camera is placed when the X-axis value is zero.  This is a rotation in degrees around the Y axis.  When this value is 0, the camera will be placed behind the target.  Nonzero offsets will rotate the zero position around the target.")]
			public float m_Bias;

			// Token: 0x020000ED RID: 237
			[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
			public enum HeadingDefinition
			{
				// Token: 0x040004A1 RID: 1185
				PositionDelta,
				// Token: 0x040004A2 RID: 1186
				Velocity,
				// Token: 0x040004A3 RID: 1187
				TargetForward,
				// Token: 0x040004A4 RID: 1188
				WorldForward
			}
		}

		// Token: 0x02000099 RID: 153
		// (Invoke) Token: 0x06000441 RID: 1089
		internal delegate float UpdateHeadingDelegate(CinemachineOrbitalTransposer orbital, float deltaTime, Vector3 up);
	}
}
