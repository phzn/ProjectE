using System;
using Cinemachine;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200124B RID: 4683
	[DisallowMultipleComponent]
	public sealed class DetectionShotCameraExtension : CinemachineExtension
	{
		// Token: 0x06009199 RID: 37273 RVA: 0x003B9A38 File Offset: 0x003B7C38
		internal void ResetShot()
		{
			this.Candidate = 0;
			this.HasSolution = false;
			this.NextEvaluation = 0f;
			this.Yaw = (this.Lift = 0f);
			this.DistanceScale = 1f;
			this.HasSafePosition = false;
			this.ResetBlend();
		}

		// Token: 0x0600919A RID: 37274 RVA: 0x003B9A8C File Offset: 0x003B7C8C
		internal void ResetBlend()
		{
			this.HoldBlendDestination = false;
			this._blendFrom = (this._blendTo = null);
			this._blendTime = 0f;
		}

		// Token: 0x0600919B RID: 37275 RVA: 0x003B9ABC File Offset: 0x003B7CBC
		internal void ObserveBlend(CinemachineBlend blend)
		{
			if (blend == null)
			{
				this.ResetBlend();
				return;
			}
			if (this._blendFrom != blend.CamA || this._blendTo != blend.CamB || blend.TimeInBlend < this._blendTime)
			{
				this.HoldBlendDestination = false;
			}
			this._blendFrom = blend.CamA;
			this._blendTo = blend.CamB;
			this._blendTime = blend.TimeInBlend;
		}

		// Token: 0x0600919C RID: 37276 RVA: 0x003B9B28 File Offset: 0x003B7D28
		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage == CinemachineCore.Stage.Finalize && this.Owner != null && this.Subject != null)
			{
				this.Owner.CorrectShot(this, ref state, deltaTime);
			}
		}

		// Token: 0x040095DB RID: 38363
		internal DetectionShotProtection Owner;

		// Token: 0x040095DC RID: 38364
		internal Transform Subject;

		// Token: 0x040095DD RID: 38365
		internal bool PreserveSubjectCloseUp;

		// Token: 0x040095DE RID: 38366
		internal int Candidate;

		// Token: 0x040095DF RID: 38367
		internal bool HasSolution;

		// Token: 0x040095E0 RID: 38368
		internal float NextEvaluation;

		// Token: 0x040095E1 RID: 38369
		internal float Yaw;

		// Token: 0x040095E2 RID: 38370
		internal float Lift;

		// Token: 0x040095E3 RID: 38371
		internal float DistanceScale = 1f;

		// Token: 0x040095E4 RID: 38372
		internal bool HasSafePosition;

		// Token: 0x040095E5 RID: 38373
		internal Vector3 LastSafePosition;

		// Token: 0x040095E6 RID: 38374
		internal bool HoldBlendDestination;

		// Token: 0x040095E7 RID: 38375
		private ICinemachineCamera _blendFrom;

		// Token: 0x040095E8 RID: 38376
		private ICinemachineCamera _blendTo;

		// Token: 0x040095E9 RID: 38377
		private float _blendTime;
	}
}
