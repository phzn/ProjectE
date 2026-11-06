using System;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x0200002A RID: 42
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class CinemachineTrackedDolly : CinemachineComponentBase
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000F32E File Offset: 0x0000D52E
		public override bool IsValid
		{
			get
			{
				return base.enabled && this.m_Path != null;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000F346 File Offset: 0x0000D546
		public override CinemachineCore.Stage Stage
		{
			get
			{
				return CinemachineCore.Stage.Body;
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000F34C File Offset: 0x0000D54C
		public override float GetMaxDampTime()
		{
			Vector3 angularDamping = this.AngularDamping;
			float num = Mathf.Max(this.m_XDamping, Mathf.Max(this.m_YDamping, this.m_ZDamping));
			float num2 = Mathf.Max(angularDamping.x, Mathf.Max(angularDamping.y, angularDamping.z));
			return Mathf.Max(num, num2);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000F3A0 File Offset: 0x0000D5A0
		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			if (deltaTime < 0f || !base.VirtualCamera.PreviousStateIsValid)
			{
				this.m_PreviousPathPosition = this.m_PathPosition;
				this.m_PreviousCameraPosition = curState.RawPosition;
				this.m_PreviousOrientation = curState.RawOrientation;
			}
			if (!this.IsValid)
			{
				return;
			}
			if (this.m_AutoDolly.m_Enabled && base.FollowTarget != null)
			{
				float num = this.m_Path.ToNativePathUnits(this.m_PreviousPathPosition, this.m_PositionUnits);
				this.m_PathPosition = this.m_Path.FindClosestPoint(base.FollowTargetPosition, Mathf.FloorToInt(num), (deltaTime < 0f || this.m_AutoDolly.m_SearchRadius <= 0) ? (-1) : this.m_AutoDolly.m_SearchRadius, this.m_AutoDolly.m_SearchResolution);
				this.m_PathPosition = this.m_Path.FromPathNativeUnits(this.m_PathPosition, this.m_PositionUnits);
				this.m_PathPosition += this.m_AutoDolly.m_PositionOffset;
			}
			float num2 = this.m_PathPosition;
			if (deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid)
			{
				float num3 = this.m_Path.MaxUnit(this.m_PositionUnits);
				if (num3 > 0f)
				{
					float num4 = this.m_Path.StandardizeUnit(this.m_PreviousPathPosition, this.m_PositionUnits);
					float num5 = this.m_Path.StandardizeUnit(num2, this.m_PositionUnits);
					if (this.m_Path.Looped && Mathf.Abs(num5 - num4) > num3 / 2f)
					{
						if (num5 > num4)
						{
							num4 += num3;
						}
						else
						{
							num4 -= num3;
						}
					}
					this.m_PreviousPathPosition = num4;
					num2 = num5;
				}
				float num6 = this.m_PreviousPathPosition - num2;
				num6 = Damper.Damp(num6, this.m_ZDamping, deltaTime);
				num2 = this.m_PreviousPathPosition - num6;
			}
			this.m_PreviousPathPosition = num2;
			Quaternion quaternion = this.m_Path.EvaluateOrientationAtUnit(num2, this.m_PositionUnits);
			Vector3 vector = this.m_Path.EvaluatePositionAtUnit(num2, this.m_PositionUnits);
			Vector3 vector2 = quaternion * Vector3.right;
			Vector3 vector3 = quaternion * Vector3.up;
			Vector3 vector4 = quaternion * Vector3.forward;
			vector += this.m_PathOffset.x * vector2;
			vector += this.m_PathOffset.y * vector3;
			vector += this.m_PathOffset.z * vector4;
			if (deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid)
			{
				Vector3 previousCameraPosition = this.m_PreviousCameraPosition;
				Vector3 vector5 = previousCameraPosition - vector;
				Vector3 vector6 = Vector3.Dot(vector5, vector3) * vector3;
				Vector3 vector7 = vector5 - vector6;
				vector7 = Damper.Damp(vector7, this.m_XDamping, deltaTime);
				vector6 = Damper.Damp(vector6, this.m_YDamping, deltaTime);
				vector = previousCameraPosition - (vector7 + vector6);
			}
			curState.RawPosition = (this.m_PreviousCameraPosition = vector);
			Quaternion quaternion2 = this.GetCameraOrientationAtPathPoint(quaternion, curState.ReferenceUp);
			if (deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid)
			{
				Vector3 vector8 = (Quaternion.Inverse(this.m_PreviousOrientation) * quaternion2).eulerAngles;
				for (int i = 0; i < 3; i++)
				{
					if (vector8[i] > 180f)
					{
						ref Vector3 ptr = ref vector8;
						int num7 = i;
						ptr[num7] -= 360f;
					}
				}
				vector8 = Damper.Damp(vector8, this.AngularDamping, deltaTime);
				quaternion2 = this.m_PreviousOrientation * Quaternion.Euler(vector8);
			}
			this.m_PreviousOrientation = quaternion2;
			curState.RawOrientation = quaternion2;
			if (this.m_CameraUp != CinemachineTrackedDolly.CameraUpMode.Default)
			{
				curState.ReferenceUp = curState.RawOrientation * Vector3.up;
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000F770 File Offset: 0x0000D970
		private Quaternion GetCameraOrientationAtPathPoint(Quaternion pathOrientation, Vector3 up)
		{
			switch (this.m_CameraUp)
			{
			case CinemachineTrackedDolly.CameraUpMode.Path:
				return pathOrientation;
			case CinemachineTrackedDolly.CameraUpMode.PathNoRoll:
				return Quaternion.LookRotation(pathOrientation * Vector3.forward, up);
			case CinemachineTrackedDolly.CameraUpMode.FollowTarget:
				if (base.FollowTarget != null)
				{
					return base.FollowTargetRotation;
				}
				break;
			case CinemachineTrackedDolly.CameraUpMode.FollowTargetNoRoll:
				if (base.FollowTarget != null)
				{
					return Quaternion.LookRotation(base.FollowTargetRotation * Vector3.forward, up);
				}
				break;
			}
			return Quaternion.LookRotation(base.VirtualCamera.transform.rotation * Vector3.forward, up);
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000F810 File Offset: 0x0000DA10
		private Vector3 AngularDamping
		{
			get
			{
				switch (this.m_CameraUp)
				{
				case CinemachineTrackedDolly.CameraUpMode.Default:
					return Vector3.zero;
				case CinemachineTrackedDolly.CameraUpMode.PathNoRoll:
				case CinemachineTrackedDolly.CameraUpMode.FollowTargetNoRoll:
					return new Vector3(this.m_PitchDamping, this.m_YawDamping, 0f);
				}
				return new Vector3(this.m_PitchDamping, this.m_YawDamping, this.m_RollDamping);
			}
		}

		// Token: 0x0400016E RID: 366
		[Tooltip("The path to which the camera will be constrained.  This must be non-null.")]
		public CinemachinePathBase m_Path;

		// Token: 0x0400016F RID: 367
		[Tooltip("The position along the path at which the camera will be placed.  This can be animated directly, or set automatically by the Auto-Dolly feature to get as close as possible to the Follow target.  The value is interpreted according to the Position Units setting.")]
		public float m_PathPosition;

		// Token: 0x04000170 RID: 368
		[Tooltip("How to interpret Path Position.  If set to Path Units, values are as follows: 0 represents the first waypoint on the path, 1 is the second, and so on.  Values in-between are points on the path in between the waypoints.  If set to Distance, then Path Position represents distance along the path.")]
		public CinemachinePathBase.PositionUnits m_PositionUnits;

		// Token: 0x04000171 RID: 369
		[Tooltip("Where to put the camera relative to the path position.  X is perpendicular to the path, Y is up, and Z is parallel to the path.  This allows the camera to be offset from the path itself (as if on a tripod, for example).")]
		public Vector3 m_PathOffset = Vector3.zero;

		// Token: 0x04000172 RID: 370
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain its position in a direction perpendicular to the path.  Small numbers are more responsive, rapidly translating the camera to keep the target's x-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_XDamping;

		// Token: 0x04000173 RID: 371
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain its position in the path-local up direction.  Small numbers are more responsive, rapidly translating the camera to keep the target's y-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_YDamping;

		// Token: 0x04000174 RID: 372
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain its position in a direction parallel to the path.  Small numbers are more responsive, rapidly translating the camera to keep the target's z-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_ZDamping = 1f;

		// Token: 0x04000175 RID: 373
		[Tooltip("How to set the virtual camera's Up vector.  This will affect the screen composition, because the camera Aim behaviours will always try to respect the Up direction.")]
		public CinemachineTrackedDolly.CameraUpMode m_CameraUp;

		// Token: 0x04000176 RID: 374
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to track the target rotation's X angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
		public float m_PitchDamping;

		// Token: 0x04000177 RID: 375
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to track the target rotation's Y angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
		public float m_YawDamping;

		// Token: 0x04000178 RID: 376
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to track the target rotation's Z angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
		public float m_RollDamping;

		// Token: 0x04000179 RID: 377
		[Tooltip("Controls how automatic dollying occurs.  A Follow target is necessary to use this feature.")]
		public CinemachineTrackedDolly.AutoDolly m_AutoDolly = new CinemachineTrackedDolly.AutoDolly(false, 0f, 2, 5);

		// Token: 0x0400017A RID: 378
		private float m_PreviousPathPosition;

		// Token: 0x0400017B RID: 379
		private Quaternion m_PreviousOrientation = Quaternion.identity;

		// Token: 0x0400017C RID: 380
		private Vector3 m_PreviousCameraPosition = Vector3.zero;

		// Token: 0x0200009C RID: 156
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		public enum CameraUpMode
		{
			// Token: 0x0400033B RID: 827
			Default,
			// Token: 0x0400033C RID: 828
			Path,
			// Token: 0x0400033D RID: 829
			PathNoRoll,
			// Token: 0x0400033E RID: 830
			FollowTarget,
			// Token: 0x0400033F RID: 831
			FollowTargetNoRoll
		}

		// Token: 0x0200009D RID: 157
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		[Serializable]
		public struct AutoDolly
		{
			// Token: 0x06000447 RID: 1095 RVA: 0x0001912B File Offset: 0x0001732B
			public AutoDolly(bool enabled, float positionOffset, int searchRadius, int stepsPerSegment)
			{
				this.m_Enabled = enabled;
				this.m_PositionOffset = positionOffset;
				this.m_SearchRadius = searchRadius;
				this.m_SearchResolution = stepsPerSegment;
			}

			// Token: 0x04000340 RID: 832
			[Tooltip("If checked, will enable automatic dolly, which chooses a path position that is as close as possible to the Follow target.  Note: this can have significant performance impact")]
			public bool m_Enabled;

			// Token: 0x04000341 RID: 833
			[Tooltip("Offset, in current position units, from the closest point on the path to the follow target")]
			public float m_PositionOffset;

			// Token: 0x04000342 RID: 834
			[Tooltip("Search up to this many waypoints on either side of the current position.  Use 0 for Entire path.")]
			public int m_SearchRadius;

			// Token: 0x04000343 RID: 835
			[FormerlySerializedAs("m_StepsPerSegment")]
			[Tooltip("We search between waypoints by dividing the segment into this many straight pieces.  he higher the number, the more accurate the result, but performance is proportionally slower for higher numbers")]
			public int m_SearchResolution;
		}
	}
}
