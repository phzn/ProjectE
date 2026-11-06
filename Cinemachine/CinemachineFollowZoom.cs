using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000014 RID: 20
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineFollowZoom.html")]
	public class CinemachineFollowZoom : CinemachineExtension
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x000069B0 File Offset: 0x00004BB0
		private void OnValidate()
		{
			this.m_Width = Mathf.Max(0f, this.m_Width);
			this.m_MaxFOV = Mathf.Clamp(this.m_MaxFOV, 1f, 179f);
			this.m_MinFOV = Mathf.Clamp(this.m_MinFOV, 1f, this.m_MaxFOV);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00006A0A File Offset: 0x00004C0A
		public override float GetMaxDampTime()
		{
			return this.m_Damping;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00006A14 File Offset: 0x00004C14
		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			CinemachineFollowZoom.VcamExtraState extraState = base.GetExtraState<CinemachineFollowZoom.VcamExtraState>(vcam);
			if (deltaTime < 0f || !base.VirtualCamera.PreviousStateIsValid)
			{
				extraState.m_previousFrameZoom = state.Lens.FieldOfView;
			}
			if (stage == CinemachineCore.Stage.Body)
			{
				float num = Mathf.Max(this.m_Width, 0f);
				float num2 = 179f;
				float num3 = Vector3.Distance(state.CorrectedPosition, state.ReferenceLookAt);
				if (num3 > 0.0001f)
				{
					float num4 = num3 * 2f * Mathf.Tan(this.m_MinFOV * 0.017453292f / 2f);
					float num5 = num3 * 2f * Mathf.Tan(this.m_MaxFOV * 0.017453292f / 2f);
					num = Mathf.Clamp(num, num4, num5);
					if (deltaTime >= 0f && this.m_Damping > 0f && base.VirtualCamera.PreviousStateIsValid)
					{
						float num6 = num3 * 2f * Mathf.Tan(extraState.m_previousFrameZoom * 0.017453292f / 2f);
						float num7 = num - num6;
						num7 = base.VirtualCamera.DetachedLookAtTargetDamp(num7, this.m_Damping, deltaTime);
						num = num6 + num7;
					}
					num2 = 2f * Mathf.Atan(num / (2f * num3)) * 57.29578f;
				}
				LensSettings lens = state.Lens;
				lens.FieldOfView = (extraState.m_previousFrameZoom = Mathf.Clamp(num2, this.m_MinFOV, this.m_MaxFOV));
				state.Lens = lens;
			}
		}

		// Token: 0x0400007B RID: 123
		[Tooltip("The shot width to maintain, in world units, at target distance.")]
		public float m_Width = 2f;

		// Token: 0x0400007C RID: 124
		[Range(0f, 20f)]
		[Tooltip("Increase this value to soften the aggressiveness of the follow-zoom.  Small numbers are more responsive, larger numbers give a more heavy slowly responding camera.")]
		public float m_Damping = 1f;

		// Token: 0x0400007D RID: 125
		[Range(1f, 179f)]
		[Tooltip("Lower limit for the FOV that this behaviour will generate.")]
		public float m_MinFOV = 3f;

		// Token: 0x0400007E RID: 126
		[Range(1f, 179f)]
		[Tooltip("Upper limit for the FOV that this behaviour will generate.")]
		public float m_MaxFOV = 60f;

		// Token: 0x0200007E RID: 126
		private class VcamExtraState
		{
			// Token: 0x040002E8 RID: 744
			public float m_previousFrameZoom;
		}
	}
}
