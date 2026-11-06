using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x02000029 RID: 41
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class CinemachineSameAsFollowTarget : CinemachineComponentBase
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000F287 File Offset: 0x0000D487
		public override bool IsValid
		{
			get
			{
				return base.enabled && base.FollowTarget != null;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000F29F File Offset: 0x0000D49F
		public override CinemachineCore.Stage Stage
		{
			get
			{
				return CinemachineCore.Stage.Aim;
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000F2A2 File Offset: 0x0000D4A2
		public override float GetMaxDampTime()
		{
			return this.m_Damping;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000F2AC File Offset: 0x0000D4AC
		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			if (!this.IsValid)
			{
				return;
			}
			Quaternion quaternion = base.FollowTargetRotation;
			if (deltaTime >= 0f)
			{
				float num = base.VirtualCamera.DetachedFollowTargetDamp(1f, this.m_Damping, deltaTime);
				quaternion = Quaternion.Slerp(this.m_PreviousReferenceOrientation, base.FollowTargetRotation, num);
			}
			this.m_PreviousReferenceOrientation = quaternion;
			curState.RawOrientation = quaternion;
			curState.ReferenceUp = quaternion * Vector3.up;
		}

		// Token: 0x0400016C RID: 364
		[Tooltip("How much time it takes for the aim to catch up to the target's rotation")]
		[FormerlySerializedAs("m_AngularDamping")]
		public float m_Damping;

		// Token: 0x0400016D RID: 365
		private Quaternion m_PreviousReferenceOrientation = Quaternion.identity;
	}
}
