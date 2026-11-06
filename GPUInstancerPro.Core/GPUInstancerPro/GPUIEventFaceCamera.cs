using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000059 RID: 89
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#Face_Camera_Event")]
	public class GPUIEventFaceCamera : MonoBehaviour
	{
		// Token: 0x0600034E RID: 846 RVA: 0x0000F2E9 File Offset: 0x0000D4E9
		private void Awake()
		{
			if (!GPUIRuntimeSettings.IsSupportedPlatform())
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x000122E0 File Offset: 0x000104E0
		private void OnEnable()
		{
			GPUIRenderingSystem.InitializeRenderingSystem();
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.TransformFaceCameraPos));
			GPUIRenderingSystem instance2 = GPUIRenderingSystem.Instance;
			instance2.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance2.OnPreCull, new Action<GPUICameraData>(this.TransformFaceCameraView));
			GPUIRenderingSystem instance3 = GPUIRenderingSystem.Instance;
			instance3.OnPreCull = (Action<GPUICameraData>)Delegate.Combine(instance3.OnPreCull, this.isFaceCameraPos ? new Action<GPUICameraData>(this.TransformFaceCameraPos) : new Action<GPUICameraData>(this.TransformFaceCameraView));
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0001237C File Offset: 0x0001057C
		private void OnDisable()
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.TransformFaceCameraPos));
			GPUIRenderingSystem instance2 = GPUIRenderingSystem.Instance;
			instance2.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance2.OnPreCull, new Action<GPUICameraData>(this.TransformFaceCameraView));
		}

		// Token: 0x06000351 RID: 849 RVA: 0x000123E0 File Offset: 0x000105E0
		public void TransformFaceCameraView(GPUICameraData cameraData)
		{
			ComputeShader cs_TransformModifications = GPUIConstants.CS_TransformModifications;
			GPUIShaderBuffer gpuishaderBuffer;
			if (cameraData.TryGetShaderBuffer(this.gpuiManager, this.prototypeIndex, out gpuishaderBuffer))
			{
				cs_TransformModifications.SetBuffer(6, GPUIConstants.PROP_gpuiTransformBuffer, gpuishaderBuffer.Buffer);
				cs_TransformModifications.SetInt(GPUIConstants.PROP_bufferSize, gpuishaderBuffer.BufferSize);
				cs_TransformModifications.SetMatrix(GPUIConstants.PROP_matrix44, cameraData.ActiveCamera.cameraToWorldMatrix);
				cs_TransformModifications.DispatchX(6, gpuishaderBuffer.BufferSize);
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00012450 File Offset: 0x00010650
		public void TransformFaceCameraPos(GPUICameraData cameraData)
		{
			ComputeShader cs_TransformModifications = GPUIConstants.CS_TransformModifications;
			GPUIShaderBuffer gpuishaderBuffer;
			if (cameraData.TryGetShaderBuffer(this.gpuiManager, this.prototypeIndex, out gpuishaderBuffer))
			{
				cs_TransformModifications.SetBuffer(7, GPUIConstants.PROP_gpuiTransformBuffer, gpuishaderBuffer.Buffer);
				cs_TransformModifications.SetInt(GPUIConstants.PROP_bufferSize, gpuishaderBuffer.BufferSize);
				cs_TransformModifications.SetVector(GPUIConstants.PROP_position, cameraData.ActiveCamera.transform.position);
				cs_TransformModifications.DispatchX(7, gpuishaderBuffer.BufferSize);
			}
		}

		// Token: 0x04000267 RID: 615
		public GPUIManager gpuiManager;

		// Token: 0x04000268 RID: 616
		public int prototypeIndex;

		// Token: 0x04000269 RID: 617
		public bool isFaceCameraPos;
	}
}
