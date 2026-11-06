using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000003 RID: 3
[AddComponentMenu("")]
[ExecuteAlways]
[SaveDuringPlay]
[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineRecomposer.html")]
public class CinemachineRecomposer : CinemachineExtension
{
	// Token: 0x06000003 RID: 3 RVA: 0x00002150 File Offset: 0x00000350
	private void Reset()
	{
		this.m_ApplyAfter = CinemachineCore.Stage.Finalize;
		this.m_Tilt = 0f;
		this.m_Pan = 0f;
		this.m_Dutch = 0f;
		this.m_ZoomScale = 1f;
		this.m_FollowAttachment = 1f;
		this.m_LookAtAttachment = 1f;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000021A6 File Offset: 0x000003A6
	private void OnValidate()
	{
		this.m_ZoomScale = Mathf.Max(0.01f, this.m_ZoomScale);
		this.m_FollowAttachment = Mathf.Clamp01(this.m_FollowAttachment);
		this.m_LookAtAttachment = Mathf.Clamp01(this.m_LookAtAttachment);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000021E0 File Offset: 0x000003E0
	public override void PrePipelineMutateCameraStateCallback(CinemachineVirtualCameraBase vcam, ref CameraState curState, float deltaTime)
	{
		vcam.FollowTargetAttachment = this.m_FollowAttachment;
		vcam.LookAtTargetAttachment = this.m_LookAtAttachment;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000021FC File Offset: 0x000003FC
	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (stage == this.m_ApplyAfter)
		{
			LensSettings lens = state.Lens;
			Quaternion quaternion = state.RawOrientation * Quaternion.AngleAxis(this.m_Tilt, Vector3.right);
			Quaternion quaternion2 = Quaternion.AngleAxis(this.m_Pan, state.ReferenceUp) * quaternion;
			state.OrientationCorrection = Quaternion.Inverse(state.CorrectedOrientation) * quaternion2;
			lens.Dutch += this.m_Dutch;
			if (this.m_ZoomScale != 1f)
			{
				lens.OrthographicSize *= this.m_ZoomScale;
				lens.FieldOfView *= this.m_ZoomScale;
			}
			state.Lens = lens;
		}
	}

	// Token: 0x04000004 RID: 4
	[Tooltip("When to apply the adjustment")]
	public CinemachineCore.Stage m_ApplyAfter;

	// Token: 0x04000005 RID: 5
	[Tooltip("Tilt the camera by this much")]
	public float m_Tilt;

	// Token: 0x04000006 RID: 6
	[Tooltip("Pan the camera by this much")]
	public float m_Pan;

	// Token: 0x04000007 RID: 7
	[Tooltip("Roll the camera by this much")]
	public float m_Dutch;

	// Token: 0x04000008 RID: 8
	[Tooltip("Scale the zoom by this amount (normal = 1)")]
	public float m_ZoomScale;

	// Token: 0x04000009 RID: 9
	[Range(0f, 1f)]
	[Tooltip("Lowering this value relaxes the camera's attention to the Follow target (normal = 1)")]
	public float m_FollowAttachment;

	// Token: 0x0400000A RID: 10
	[Range(0f, 1f)]
	[Tooltip("Lowering this value relaxes the camera's attention to the LookAt target (normal = 1)")]
	public float m_LookAtAttachment;
}
