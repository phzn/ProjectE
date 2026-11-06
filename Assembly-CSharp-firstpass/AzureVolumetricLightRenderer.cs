using System;
using UnityEngine.Rendering;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000022 RID: 34
	[ExecuteInEditMode]
	[ImageEffectAllowedInSceneView]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Azure[Sky] Dynamic Skybox/Azure Volumetric Light Renderer")]
	public sealed class AzureVolumetricLightRenderer : MonoBehaviour
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000731E File Offset: 0x0000551E
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00007326 File Offset: 0x00005526
		public Camera camera
		{
			get
			{
				return this.m_camera;
			}
			set
			{
				this.m_camera = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000732F File Offset: 0x0000552F
		// (set) Token: 0x060001BE RID: 446 RVA: 0x00007337 File Offset: 0x00005537
		public Plane[] cameraFrustumPlanes
		{
			get
			{
				return this.m_cameraFrustumPlanes;
			}
			set
			{
				this.m_cameraFrustumPlanes = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00007340 File Offset: 0x00005540
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x00007348 File Offset: 0x00005548
		public CommandBuffer commandBuffer
		{
			get
			{
				return this.m_commandBuffer;
			}
			set
			{
				this.m_commandBuffer = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00007351 File Offset: 0x00005551
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00007359 File Offset: 0x00005559
		public float lightExtinctionDistance
		{
			get
			{
				return this.m_lightExtinctionDistance;
			}
			set
			{
				this.m_lightExtinctionDistance = value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00007362 File Offset: 0x00005562
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x0000736A File Offset: 0x0000556A
		public float lightExtinctionSmoothStep
		{
			get
			{
				return this.m_lightExtinctionSmoothStep;
			}
			set
			{
				this.m_lightExtinctionSmoothStep = value;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00007374 File Offset: 0x00005574
		private void Start()
		{
			this.m_camera = base.GetComponent<Camera>();
			this.m_commandBuffer = new CommandBuffer();
			this.m_commandBuffer.name = "Azure Volumetric Light Command Buffer";
			this.m_camera.RemoveCommandBuffer(CameraEvent.AfterSkybox, this.m_commandBuffer);
			this.m_camera.AddCommandBuffer(CameraEvent.AfterSkybox, this.m_commandBuffer);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000073D0 File Offset: 0x000055D0
		private void OnEnable()
		{
			RenderPipelineManager.beginCameraRendering += this.CustomOnPreRender;
			if (this.m_commandBuffer == null || this.m_camera == null)
			{
				return;
			}
			this.m_camera.RemoveCommandBuffer(CameraEvent.AfterSkybox, this.m_commandBuffer);
			this.m_camera.AddCommandBuffer(CameraEvent.AfterSkybox, this.m_commandBuffer);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000742B File Offset: 0x0000562B
		private void OnDisable()
		{
			RenderPipelineManager.beginCameraRendering -= this.CustomOnPreRender;
			if (this.m_commandBuffer == null || this.m_camera == null)
			{
				return;
			}
			this.m_camera.RemoveCommandBuffer(CameraEvent.AfterSkybox, this.m_commandBuffer);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00007468 File Offset: 0x00005668
		private void CustomOnPreRender(ScriptableRenderContext context, Camera camera)
		{
			if (this.m_commandBuffer == null || this.m_camera == null)
			{
				return;
			}
			if (camera != this.m_camera)
			{
				return;
			}
			this.m_commandBuffer.Clear();
			this.m_cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(this.m_camera);
			AzureNotificationCenter.Invoke.OnVolumetricLightPreRenderCallback(this);
		}

		// Token: 0x0400015F RID: 351
		private Camera m_camera;

		// Token: 0x04000160 RID: 352
		private Plane[] m_cameraFrustumPlanes;

		// Token: 0x04000161 RID: 353
		[SerializeField]
		private CommandBuffer m_commandBuffer;

		// Token: 0x04000162 RID: 354
		[SerializeField]
		private float m_lightExtinctionDistance = 1000f;

		// Token: 0x04000163 RID: 355
		[SerializeField]
		private float m_lightExtinctionSmoothStep = 0.5f;
	}
}
