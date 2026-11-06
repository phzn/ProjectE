using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Cinemachine
{
	// Token: 0x02000019 RID: 25
	[AddComponentMenu("")]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachinePixelPerfect.html")]
	public class CinemachinePixelPerfect : CinemachineExtension
	{
		// Token: 0x06000106 RID: 262 RVA: 0x00008AA4 File Offset: 0x00006CA4
		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage != CinemachineCore.Stage.Body)
			{
				return;
			}
			CinemachineBrain cinemachineBrain = CinemachineCore.Instance.FindPotentialTargetBrain(vcam);
			if (cinemachineBrain == null || !cinemachineBrain.IsLive(vcam, false))
			{
				return;
			}
			PixelPerfectCamera pixelPerfectCamera;
			cinemachineBrain.TryGetComponent<PixelPerfectCamera>(out pixelPerfectCamera);
			if (pixelPerfectCamera == null || !pixelPerfectCamera.isActiveAndEnabled)
			{
				return;
			}
			LensSettings lens = state.Lens;
			lens.OrthographicSize = pixelPerfectCamera.CorrectCinemachineOrthoSize(lens.OrthographicSize);
			state.Lens = lens;
		}
	}
}
