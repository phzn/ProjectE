using System;
using UnityEngine;

namespace Cinemachine.PostFX
{
	// Token: 0x0200005D RID: 93
	[SaveDuringPlay]
	[AddComponentMenu("")]
	public class CinemachinePostProcessing : CinemachineExtension
	{
		// Token: 0x060003AF RID: 943 RVA: 0x00016AAB File Offset: 0x00014CAB
		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
		}
	}
}
