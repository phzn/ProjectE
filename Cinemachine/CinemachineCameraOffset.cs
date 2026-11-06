using System;
using Cinemachine;
using Cinemachine.Utility;
using UnityEngine;

// Token: 0x02000002 RID: 2
[AddComponentMenu("")]
[ExecuteAlways]
[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/api/Cinemachine.CinemachineCameraOffset.html")]
[SaveDuringPlay]
public class CinemachineCameraOffset : CinemachineExtension
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (stage == this.m_ApplyAfter)
		{
			object obj = this.m_PreserveComposition && state.HasLookAt && stage > CinemachineCore.Stage.Body;
			Vector3 vector = Vector2.zero;
			object obj2 = obj;
			if (obj2 != null)
			{
				vector = state.RawOrientation.GetCameraRotationToTarget(state.ReferenceLookAt - state.CorrectedPosition, state.ReferenceUp);
			}
			Vector3 vector2 = state.RawOrientation * this.m_Offset;
			state.PositionCorrection += vector2;
			if (obj2 == null)
			{
				state.ReferenceLookAt += vector2;
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(state.ReferenceLookAt - state.CorrectedPosition, state.ReferenceUp);
			quaternion = quaternion.ApplyCameraRotation(-vector, state.ReferenceUp);
			state.RawOrientation = quaternion;
		}
	}

	// Token: 0x04000001 RID: 1
	[Tooltip("Offset the camera's position by this much (camera space)")]
	public Vector3 m_Offset = Vector3.zero;

	// Token: 0x04000002 RID: 2
	[Tooltip("When to apply the offset")]
	public CinemachineCore.Stage m_ApplyAfter = CinemachineCore.Stage.Aim;

	// Token: 0x04000003 RID: 3
	[Tooltip("If applying offset after aim, re-adjust the aim to preserve the screen position of the LookAt target as much as possible")]
	public bool m_PreserveComposition;
}
