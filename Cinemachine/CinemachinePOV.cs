using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000028 RID: 40
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class CinemachinePOV : CinemachineComponentBase
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000ED8C File Offset: 0x0000CF8C
		public override bool IsValid
		{
			get
			{
				return base.enabled;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000ED94 File Offset: 0x0000CF94
		public override CinemachineCore.Stage Stage
		{
			get
			{
				return CinemachineCore.Stage.Aim;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000ED97 File Offset: 0x0000CF97
		private void OnValidate()
		{
			this.m_VerticalAxis.Validate();
			this.m_VerticalRecentering.Validate();
			this.m_HorizontalAxis.Validate();
			this.m_HorizontalRecentering.Validate();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000EDC5 File Offset: 0x0000CFC5
		private void OnEnable()
		{
			this.UpdateInputAxisProvider();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000EDD0 File Offset: 0x0000CFD0
		public void UpdateInputAxisProvider()
		{
			this.m_HorizontalAxis.SetInputAxisProvider(0, null);
			this.m_VerticalAxis.SetInputAxisProvider(1, null);
			if (base.VirtualCamera != null)
			{
				AxisState.IInputAxisProvider inputAxisProvider = base.VirtualCamera.GetInputAxisProvider();
				if (inputAxisProvider != null)
				{
					this.m_HorizontalAxis.SetInputAxisProvider(0, inputAxisProvider);
					this.m_VerticalAxis.SetInputAxisProvider(1, inputAxisProvider);
				}
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000EE2E File Offset: 0x0000D02E
		public override void PrePipelineMutateCameraState(ref CameraState state, float deltaTime)
		{
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000EE30 File Offset: 0x0000D030
		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			if (!this.IsValid)
			{
				return;
			}
			if (deltaTime >= 0f && (!base.VirtualCamera.PreviousStateIsValid || !CinemachineCore.Instance.IsLive(base.VirtualCamera)))
			{
				deltaTime = -1f;
			}
			if (deltaTime >= 0f)
			{
				if (this.m_HorizontalAxis.Update(deltaTime))
				{
					this.m_HorizontalRecentering.CancelRecentering();
				}
				if (this.m_VerticalAxis.Update(deltaTime))
				{
					this.m_VerticalRecentering.CancelRecentering();
				}
			}
			Vector2 recenterTarget = this.GetRecenterTarget();
			this.m_HorizontalRecentering.DoRecentering(ref this.m_HorizontalAxis, deltaTime, recenterTarget.x);
			this.m_VerticalRecentering.DoRecentering(ref this.m_VerticalAxis, deltaTime, recenterTarget.y);
			Quaternion quaternion = Quaternion.Euler(this.m_VerticalAxis.Value, this.m_HorizontalAxis.Value, 0f);
			Transform parent = base.VirtualCamera.transform.parent;
			if (parent != null)
			{
				quaternion = parent.rotation * quaternion;
			}
			else
			{
				quaternion = Quaternion.FromToRotation(Vector3.up, curState.ReferenceUp) * quaternion;
			}
			curState.RawOrientation = quaternion;
			if (base.VirtualCamera.PreviousStateIsValid)
			{
				curState.PositionDampingBypass = UnityVectorExtensions.SafeFromToRotation(this.m_PreviousCameraRotation * Vector3.forward, quaternion * Vector3.forward, curState.ReferenceUp).eulerAngles;
			}
			this.m_PreviousCameraRotation = quaternion;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000EF98 File Offset: 0x0000D198
		public Vector2 GetRecenterTarget()
		{
			Transform transform = null;
			CinemachinePOV.RecenterTargetMode recenterTarget = this.m_RecenterTarget;
			if (recenterTarget != CinemachinePOV.RecenterTargetMode.FollowTargetForward)
			{
				if (recenterTarget == CinemachinePOV.RecenterTargetMode.LookAtTargetForward)
				{
					transform = base.VirtualCamera.LookAt;
				}
			}
			else
			{
				transform = base.VirtualCamera.Follow;
			}
			if (transform != null)
			{
				Vector3 vector = transform.forward;
				Transform parent = base.VirtualCamera.transform.parent;
				if (parent != null)
				{
					vector = parent.rotation * vector;
				}
				Vector3 eulerAngles = Quaternion.FromToRotation(Vector3.forward, vector).eulerAngles;
				return new Vector2(CinemachinePOV.NormalizeAngle(eulerAngles.y), CinemachinePOV.NormalizeAngle(eulerAngles.x));
			}
			return Vector2.zero;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000F042 File Offset: 0x0000D242
		private static float NormalizeAngle(float angle)
		{
			return (angle + 180f) % 360f - 180f;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000F057 File Offset: 0x0000D257
		public override void ForceCameraPosition(Vector3 pos, Quaternion rot)
		{
			this.SetAxesForRotation(rot);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000F060 File Offset: 0x0000D260
		public override bool OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime, ref CinemachineVirtualCameraBase.TransitionParams transitionParams)
		{
			this.m_HorizontalRecentering.DoRecentering(ref this.m_HorizontalAxis, -1f, 0f);
			this.m_VerticalRecentering.DoRecentering(ref this.m_VerticalAxis, -1f, 0f);
			this.m_HorizontalRecentering.CancelRecentering();
			this.m_VerticalRecentering.CancelRecentering();
			if (fromCam != null && transitionParams.m_InheritPosition && !CinemachineCore.Instance.IsLiveInBlend(base.VirtualCamera))
			{
				this.SetAxesForRotation(fromCam.State.RawOrientation);
				return true;
			}
			return false;
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000F0EB File Offset: 0x0000D2EB
		public override bool RequiresUserInput
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
		private void SetAxesForRotation(Quaternion targetRot)
		{
			Vector3 referenceUp = base.VcamState.ReferenceUp;
			Vector3 vector = Vector3.forward;
			Transform parent = base.VirtualCamera.transform.parent;
			if (parent != null)
			{
				vector = parent.rotation * vector;
			}
			this.m_HorizontalAxis.Value = 0f;
			this.m_HorizontalAxis.Reset();
			Vector3 vector2 = targetRot * Vector3.forward;
			Vector3 vector3 = vector.ProjectOntoPlane(referenceUp);
			Vector3 vector4 = vector2.ProjectOntoPlane(referenceUp);
			if (!vector3.AlmostZero() && !vector4.AlmostZero())
			{
				this.m_HorizontalAxis.Value = Vector3.SignedAngle(vector3, vector4, referenceUp);
			}
			this.m_VerticalAxis.Value = 0f;
			this.m_VerticalAxis.Reset();
			vector = Quaternion.AngleAxis(this.m_HorizontalAxis.Value, referenceUp) * vector;
			Vector3 vector5 = Vector3.Cross(referenceUp, vector);
			if (!vector5.AlmostZero())
			{
				this.m_VerticalAxis.Value = Vector3.SignedAngle(vector, vector2, vector5);
			}
		}

		// Token: 0x04000165 RID: 357
		public CinemachinePOV.RecenterTargetMode m_RecenterTarget;

		// Token: 0x04000166 RID: 358
		[Tooltip("The Vertical axis.  Value is -90..90. Controls the vertical orientation")]
		[AxisStateProperty]
		public AxisState m_VerticalAxis = new AxisState(-70f, 70f, false, false, 300f, 0.1f, 0.1f, "Mouse Y", true);

		// Token: 0x04000167 RID: 359
		[Tooltip("Controls how automatic recentering of the Vertical axis is accomplished")]
		public AxisState.Recentering m_VerticalRecentering = new AxisState.Recentering(false, 1f, 2f);

		// Token: 0x04000168 RID: 360
		[Tooltip("The Horizontal axis.  Value is -180..180.  Controls the horizontal orientation")]
		[AxisStateProperty]
		public AxisState m_HorizontalAxis = new AxisState(-180f, 180f, true, false, 300f, 0.1f, 0.1f, "Mouse X", false);

		// Token: 0x04000169 RID: 361
		[Tooltip("Controls how automatic recentering of the Horizontal axis is accomplished")]
		public AxisState.Recentering m_HorizontalRecentering = new AxisState.Recentering(false, 1f, 2f);

		// Token: 0x0400016A RID: 362
		[HideInInspector]
		[Tooltip("Obsolete - no longer used")]
		public bool m_ApplyBeforeBody;

		// Token: 0x0400016B RID: 363
		private Quaternion m_PreviousCameraRotation;

		// Token: 0x0200009B RID: 155
		public enum RecenterTargetMode
		{
			// Token: 0x04000337 RID: 823
			None,
			// Token: 0x04000338 RID: 824
			FollowTargetForward,
			// Token: 0x04000339 RID: 825
			LookAtTargetForward
		}
	}
}
