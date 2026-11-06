using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MapGeneration.Runtime
{
	// Token: 0x0200076C RID: 1900
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-500)]
	public sealed class MallRuntimeLightBudget : MonoBehaviour
	{
		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x001146CA File Offset: 0x001128CA
		public int CurrentActiveLights
		{
			get
			{
				return this.currentActiveLights;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06003130 RID: 12592 RVA: 0x001146D2 File Offset: 0x001128D2
		public int TotalLights
		{
			get
			{
				return this._lights.Length;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06003131 RID: 12593 RVA: 0x001146DC File Offset: 0x001128DC
		public int CurrentActiveReflectionProbes
		{
			get
			{
				return this.currentActiveReflectionProbes;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003132 RID: 12594 RVA: 0x001146E4 File Offset: 0x001128E4
		public int TotalReflectionProbes
		{
			get
			{
				return this._reflectionProbes.Length;
			}
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x001146EE File Offset: 0x001128EE
		private void OnEnable()
		{
			this.CacheLights();
			this.CacheAndApplyReflectionProbeBudget();
			this.RefreshLightStatistics();
			this.ApplySceneRenderingBudget();
			this._nextUpdateTime = 0f;
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x00114713 File Offset: 0x00112913
		private void Update()
		{
			if (Time.unscaledTime < this._nextUpdateTime)
			{
				return;
			}
			this._nextUpdateTime = Time.unscaledTime + this.updateInterval;
			this.RefreshLightStatistics();
			this.RefreshReflectionProbeStatistics();
			this.ApplySceneRenderingBudget();
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x00114747 File Offset: 0x00112947
		private void OnDisable()
		{
			this.RestoreReflectionProbes();
			this.RestoreSceneRenderingBudget();
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x00114755 File Offset: 0x00112955
		private void CacheLights()
		{
			this._lights = base.GetComponentsInChildren<Light>(true);
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x00114764 File Offset: 0x00112964
		private void CacheAndApplyReflectionProbeBudget()
		{
			ReflectionProbe[] array = Object.FindObjectsOfType<ReflectionProbe>(true);
			List<ReflectionProbe> list = new List<ReflectionProbe>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].gameObject.scene == base.gameObject.scene)
				{
					list.Add(array[i]);
				}
			}
			this.ApplyReflectionProbeBudget(list.ToArray());
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x001147D0 File Offset: 0x001129D0
		private void ApplyReflectionProbeBudget(ReflectionProbe[] sceneProbes)
		{
			this._reflectionProbes = sceneProbes;
			int num = this._reflectionProbes.Length;
			this._authoredProbeEnabled = new bool[num];
			this._authoredProbeResolution = new int[num];
			this._managedRealtimeProbes = new bool[num];
			this._probeIndices = new int[num];
			this._probeScores = new float[num];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				ReflectionProbe reflectionProbe = this._reflectionProbes[i];
				if (!(reflectionProbe == null))
				{
					this._authoredProbeEnabled[i] = reflectionProbe.enabled;
					this._authoredProbeResolution[i] = reflectionProbe.resolution;
					this._managedRealtimeProbes[i] = reflectionProbe.mode == ReflectionProbeMode.Realtime && reflectionProbe.isActiveAndEnabled;
					if (this._managedRealtimeProbes[i])
					{
						Vector3 size = reflectionProbe.bounds.size;
						this._probeIndices[num2] = i;
						this._probeScores[num2] = -(size.x * size.y * size.z);
						num2++;
					}
				}
			}
			Array.Sort<float, int>(this._probeScores, this._probeIndices, 0, num2);
			int num3 = Mathf.Min((this.maxActiveReflectionProbes > 0) ? Mathf.Min(this.maxActiveReflectionProbes, 6) : 6, num2);
			bool[] array = new bool[num];
			for (int j = 0; j < num3; j++)
			{
				array[this._probeIndices[j]] = true;
			}
			for (int k = 0; k < num; k++)
			{
				ReflectionProbe reflectionProbe2 = this._reflectionProbes[k];
				if (!(reflectionProbe2 == null) && this._managedRealtimeProbes[k])
				{
					bool flag = this._authoredProbeEnabled[k] && array[k];
					reflectionProbe2.enabled = flag;
					if (flag)
					{
						int num4 = ((this.maxRealtimeProbeResolution >= 16) ? Mathf.Min(this.maxRealtimeProbeResolution, 128) : 128);
						reflectionProbe2.resolution = Mathf.Min(reflectionProbe2.resolution, num4);
					}
				}
			}
			this.RefreshReflectionProbeStatistics();
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x001149C8 File Offset: 0x00112BC8
		private void RefreshReflectionProbeStatistics()
		{
			this.currentActiveReflectionProbes = 0;
			for (int i = 0; i < this._reflectionProbes.Length; i++)
			{
				if (this._reflectionProbes[i] != null && this._reflectionProbes[i].isActiveAndEnabled)
				{
					this.currentActiveReflectionProbes++;
				}
			}
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x00114A1C File Offset: 0x00112C1C
		private void RefreshLightStatistics()
		{
			this.currentActiveLights = 0;
			for (int i = 0; i < this._lights.Length; i++)
			{
				Light light = this._lights[i];
				if (light != null && light.isActiveAndEnabled)
				{
					this.currentActiveLights++;
				}
			}
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x00114A6C File Offset: 0x00112C6C
		private void RestoreReflectionProbes()
		{
			int num = Mathf.Min(this._reflectionProbes.Length, this._authoredProbeEnabled.Length);
			for (int i = 0; i < num; i++)
			{
				ReflectionProbe reflectionProbe = this._reflectionProbes[i];
				if (!(reflectionProbe == null) && i < this._managedRealtimeProbes.Length && this._managedRealtimeProbes[i] && reflectionProbe.mode == ReflectionProbeMode.Realtime)
				{
					reflectionProbe.enabled = this._authoredProbeEnabled[i];
					if (i < this._authoredProbeResolution.Length)
					{
						reflectionProbe.resolution = this._authoredProbeResolution[i];
					}
					this._managedRealtimeProbes[i] = false;
				}
			}
			this.RefreshReflectionProbeStatistics();
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x00114B04 File Offset: 0x00112D04
		private void ApplySceneRenderingBudget()
		{
			this.ApplyCameraRenderingBudget();
			UniversalRenderPipelineAsset universalRenderPipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
			if (universalRenderPipelineAsset == null)
			{
				universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
			}
			this.ApplyPipelineBudget(universalRenderPipelineAsset);
			this.ApplyFaceCameraBudget();
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x00114B44 File Offset: 0x00112D44
		private void ApplyCameraRenderingBudget()
		{
			if (this._targetCamera == null || !this._targetCamera.isActiveAndEnabled)
			{
				this._targetCamera = Camera.main;
			}
			if (this._targetCamera != null && (!this._cameraBudgetCaptured || this._budgetCamera != this._targetCamera))
			{
				this.RestoreCameraRenderingBudget();
				this._budgetCamera = this._targetCamera;
				this._budgetCameraData = this._budgetCamera.GetComponent<UniversalAdditionalCameraData>();
				if (this._budgetCameraData != null)
				{
					this._authoredCameraAntialiasing = this._budgetCameraData.antialiasing;
					this._cameraBudgetCaptured = true;
				}
			}
			if (this._budgetCameraData != null && this._budgetCameraData.antialiasing == AntialiasingMode.None)
			{
				this._budgetCameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
			}
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x00114C10 File Offset: 0x00112E10
		private void RestoreSceneRenderingBudget()
		{
			this.RestoreCameraRenderingBudget();
			this.RestorePipelineBudget();
			this.RestoreFaceCameraBudget();
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x00114C24 File Offset: 0x00112E24
		private void RestoreCameraRenderingBudget()
		{
			if (this._cameraBudgetCaptured && this._budgetCameraData != null)
			{
				this._budgetCameraData.antialiasing = this._authoredCameraAntialiasing;
			}
			this._budgetCamera = null;
			this._budgetCameraData = null;
			this._cameraBudgetCaptured = false;
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x00114C64 File Offset: 0x00112E64
		private void ApplyPipelineBudget(UniversalRenderPipelineAsset activeAsset)
		{
			if (activeAsset == null)
			{
				return;
			}
			if (!this._pipelineBudgetCaptured || this._budgetPipelineAsset != activeAsset)
			{
				this.RestorePipelineBudget();
				this._budgetPipelineAsset = activeAsset;
				this._authoredMsaaSamples = activeAsset.msaaSampleCount;
				this._pipelineBudgetCaptured = true;
			}
			int num = ((this.maxMsaaSamples <= 1) ? 1 : 2);
			activeAsset.msaaSampleCount = Mathf.Min(this._authoredMsaaSamples, num);
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x00114CD1 File Offset: 0x00112ED1
		private void RestorePipelineBudget()
		{
			if (this._pipelineBudgetCaptured && this._budgetPipelineAsset != null)
			{
				this._budgetPipelineAsset.msaaSampleCount = this._authoredMsaaSamples;
			}
			this._budgetPipelineAsset = null;
			this._pipelineBudgetCaptured = false;
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x00114D08 File Offset: 0x00112F08
		private void ApplyFaceCameraBudget()
		{
			if (this._faceCamera == null)
			{
				foreach (Camera camera in Resources.FindObjectsOfTypeAll<Camera>())
				{
					if (camera != null && camera.name == "PlayerFaceCam" && camera.gameObject.scene.IsValid())
					{
						this._faceCamera = camera;
						break;
					}
				}
			}
			if (this._faceCamera == null)
			{
				return;
			}
			if (!this._faceCameraBudgetCaptured)
			{
				this._faceCameraData = this._faceCamera.GetComponent<UniversalAdditionalCameraData>();
				this._authoredFaceHdr = this._faceCamera.allowHDR;
				this._authoredFaceMsaa = this._faceCamera.allowMSAA;
				if (this._faceCameraData != null)
				{
					this._authoredFaceRenderShadows = this._faceCameraData.renderShadows;
					this._authoredFacePostProcessing = this._faceCameraData.renderPostProcessing;
					this._authoredFaceDepthOption = this._faceCameraData.requiresDepthOption;
					this._authoredFaceColorOption = this._faceCameraData.requiresColorOption;
				}
				this._faceCameraBudgetCaptured = true;
			}
			this._faceCamera.allowHDR = false;
			this._faceCamera.allowMSAA = false;
			if (this._faceCameraData != null)
			{
				this._faceCameraData.renderShadows = false;
				this._faceCameraData.renderPostProcessing = false;
				this._faceCameraData.requiresDepthOption = CameraOverrideOption.Off;
				this._faceCameraData.requiresColorOption = CameraOverrideOption.Off;
			}
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x00114E70 File Offset: 0x00113070
		private void RestoreFaceCameraBudget()
		{
			if (this._faceCameraBudgetCaptured && this._faceCamera != null)
			{
				this._faceCamera.allowHDR = this._authoredFaceHdr;
				this._faceCamera.allowMSAA = this._authoredFaceMsaa;
				if (this._faceCameraData != null)
				{
					this._faceCameraData.renderShadows = this._authoredFaceRenderShadows;
					this._faceCameraData.renderPostProcessing = this._authoredFacePostProcessing;
					this._faceCameraData.requiresDepthOption = this._authoredFaceDepthOption;
					this._faceCameraData.requiresColorOption = this._authoredFaceColorOption;
				}
			}
			this._faceCamera = null;
			this._faceCameraData = null;
			this._faceCameraBudgetCaptured = false;
		}

		// Token: 0x04003191 RID: 12689
		[SerializeField]
		[HideInInspector]
		private int maxActiveLights = 16;

		// Token: 0x04003192 RID: 12690
		[SerializeField]
		[HideInInspector]
		private float maxDistance = 42f;

		// Token: 0x04003193 RID: 12691
		[SerializeField]
		[Min(0.05f)]
		private float updateInterval = 0.2f;

		// Token: 0x04003194 RID: 12692
		[Header("Realtime Reflection Probe Budget")]
		[SerializeField]
		[Min(1f)]
		private int maxActiveReflectionProbes = 6;

		// Token: 0x04003195 RID: 12693
		[SerializeField]
		[Min(16f)]
		private int maxRealtimeProbeResolution = 128;

		// Token: 0x04003196 RID: 12694
		[Header("Scene GPU Budget")]
		[SerializeField]
		[Range(1f, 4f)]
		private int maxMsaaSamples = 2;

		// Token: 0x04003197 RID: 12695
		[SerializeField]
		[HideInInspector]
		private int maxAdditionalLightsPerObject = 2;

		// Token: 0x04003198 RID: 12696
		[SerializeField]
		[HideInInspector]
		private int currentActiveLights;

		// Token: 0x04003199 RID: 12697
		[SerializeField]
		[HideInInspector]
		private int currentActiveReflectionProbes;

		// Token: 0x0400319A RID: 12698
		private Light[] _lights = Array.Empty<Light>();

		// Token: 0x0400319B RID: 12699
		private ReflectionProbe[] _reflectionProbes = Array.Empty<ReflectionProbe>();

		// Token: 0x0400319C RID: 12700
		private bool[] _authoredProbeEnabled = Array.Empty<bool>();

		// Token: 0x0400319D RID: 12701
		private int[] _authoredProbeResolution = Array.Empty<int>();

		// Token: 0x0400319E RID: 12702
		private bool[] _managedRealtimeProbes = Array.Empty<bool>();

		// Token: 0x0400319F RID: 12703
		private int[] _probeIndices = Array.Empty<int>();

		// Token: 0x040031A0 RID: 12704
		private float[] _probeScores = Array.Empty<float>();

		// Token: 0x040031A1 RID: 12705
		private Camera _targetCamera;

		// Token: 0x040031A2 RID: 12706
		private Camera _budgetCamera;

		// Token: 0x040031A3 RID: 12707
		private UniversalAdditionalCameraData _budgetCameraData;

		// Token: 0x040031A4 RID: 12708
		private AntialiasingMode _authoredCameraAntialiasing;

		// Token: 0x040031A5 RID: 12709
		private bool _cameraBudgetCaptured;

		// Token: 0x040031A6 RID: 12710
		private UniversalRenderPipelineAsset _budgetPipelineAsset;

		// Token: 0x040031A7 RID: 12711
		private int _authoredMsaaSamples;

		// Token: 0x040031A8 RID: 12712
		private bool _pipelineBudgetCaptured;

		// Token: 0x040031A9 RID: 12713
		private Camera _faceCamera;

		// Token: 0x040031AA RID: 12714
		private UniversalAdditionalCameraData _faceCameraData;

		// Token: 0x040031AB RID: 12715
		private bool _authoredFaceHdr;

		// Token: 0x040031AC RID: 12716
		private bool _authoredFaceMsaa;

		// Token: 0x040031AD RID: 12717
		private bool _authoredFaceRenderShadows;

		// Token: 0x040031AE RID: 12718
		private bool _authoredFacePostProcessing;

		// Token: 0x040031AF RID: 12719
		private CameraOverrideOption _authoredFaceDepthOption;

		// Token: 0x040031B0 RID: 12720
		private CameraOverrideOption _authoredFaceColorOption;

		// Token: 0x040031B1 RID: 12721
		private bool _faceCameraBudgetCaptured;

		// Token: 0x040031B2 RID: 12722
		private float _nextUpdateTime;
	}
}
