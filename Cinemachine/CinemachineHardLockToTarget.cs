using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000025 RID: 37
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class CinemachineHardLockToTarget : CinemachineComponentBase
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000E454 File Offset: 0x0000C654
		public override bool IsValid
		{
			get
			{
				return base.enabled && base.FollowTarget != null;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000E46C File Offset: 0x0000C66C
		public override CinemachineCore.Stage Stage
		{
			get
			{
				return CinemachineCore.Stage.Body;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000E46F File Offset: 0x0000C66F
		public override float GetMaxDampTime()
		{
			return this.m_Damping;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000E478 File Offset: 0x0000C678
		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			if (!this.IsValid)
			{
				return;
			}
			Vector3 vector = base.FollowTargetPosition;
			if (deltaTime >= 0f)
			{
				vector = this.m_PreviousTargetPosition + base.VirtualCamera.DetachedFollowTargetDamp(vector - this.m_PreviousTargetPosition, this.m_Damping, deltaTime);
			}
			this.m_PreviousTargetPosition = vector;
			curState.RawPosition = vector;
		}

		// Token: 0x04000155 RID: 341
		[Tooltip("How much time it takes for the position to catch up to the target's position")]
		public float m_Damping;

		// Token: 0x04000156 RID: 342
		private Vector3 m_PreviousTargetPosition;
	}
}
