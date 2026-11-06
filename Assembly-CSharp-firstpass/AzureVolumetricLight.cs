using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000021 RID: 33
	[ExecuteInEditMode]
	[RequireComponent(typeof(Light))]
	[AddComponentMenu("Azure[Sky] Dynamic Skybox/Azure Volumetric Light")]
	public sealed class AzureVolumetricLight : MonoBehaviour
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00006CCC File Offset: 0x00004ECC
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00006CD4 File Offset: 0x00004ED4
		public Light light
		{
			get
			{
				return this.m_light;
			}
			set
			{
				this.m_light = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00006CDD File Offset: 0x00004EDD
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00006CE5 File Offset: 0x00004EE5
		public Mesh lightMesh
		{
			get
			{
				return this.m_lightMesh;
			}
			set
			{
				this.m_lightMesh = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00006CEE File Offset: 0x00004EEE
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00006CF6 File Offset: 0x00004EF6
		public Material lightMaterial
		{
			get
			{
				return this.m_lightMaterial;
			}
			set
			{
				this.m_lightMaterial = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00006CFF File Offset: 0x00004EFF
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00006D07 File Offset: 0x00004F07
		public float mieDirectionalityFactor
		{
			get
			{
				return this.m_mieDirectionalityFactor;
			}
			set
			{
				this.m_mieDirectionalityFactor = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00006D10 File Offset: 0x00004F10
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00006D18 File Offset: 0x00004F18
		public float intensityMultiplier
		{
			get
			{
				return this.m_intensityMultiplier;
			}
			set
			{
				this.m_intensityMultiplier = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00006D21 File Offset: 0x00004F21
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00006D29 File Offset: 0x00004F29
		public float rangeMultiplier
		{
			get
			{
				return this.m_rangeMultiplier;
			}
			set
			{
				this.m_rangeMultiplier = value;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00006D32 File Offset: 0x00004F32
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00006D3A File Offset: 0x00004F3A
		public float angleMultiplier
		{
			get
			{
				return this.m_angleMultiplier;
			}
			set
			{
				this.m_angleMultiplier = value;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00006D43 File Offset: 0x00004F43
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00006D4B File Offset: 0x00004F4B
		public Matrix4x4 trsMatrix
		{
			get
			{
				return this.m_trsMatrix;
			}
			set
			{
				this.m_trsMatrix = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00006D54 File Offset: 0x00004F54
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00006D5C File Offset: 0x00004F5C
		public float distanceToCamera
		{
			get
			{
				return this.m_distanceToCamera;
			}
			set
			{
				this.m_distanceToCamera = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00006D65 File Offset: 0x00004F65
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00006D6D File Offset: 0x00004F6D
		public int isCameraInsideSphereRange
		{
			get
			{
				return this.m_isCameraInsideSphereRange;
			}
			set
			{
				this.m_isCameraInsideSphereRange = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00006D76 File Offset: 0x00004F76
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00006D7E File Offset: 0x00004F7E
		public int isCameraInsideConeRange
		{
			get
			{
				return this.m_isCameraInsideConeRange;
			}
			set
			{
				this.m_isCameraInsideConeRange = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00006D87 File Offset: 0x00004F87
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00006D8F File Offset: 0x00004F8F
		public MaterialPropertyBlock propertyBlock
		{
			get
			{
				return this.m_propertyBlock;
			}
			set
			{
				this.m_propertyBlock = value;
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00006D98 File Offset: 0x00004F98
		private void Start()
		{
			this.m_light = base.GetComponent<Light>();
			this.m_propertyBlock = new MaterialPropertyBlock();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00006DB1 File Offset: 0x00004FB1
		private void OnEnable()
		{
			AzureNotificationCenter.OnVolumetricLightPreRender += this.OnVolumetricLightPreRender;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00006DC4 File Offset: 0x00004FC4
		private void OnDisable()
		{
			AzureNotificationCenter.OnVolumetricLightPreRender -= this.OnVolumetricLightPreRender;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00006DD8 File Offset: 0x00004FD8
		private bool CheckSphereCulling(Plane[] planes, Vector3 center, float radius)
		{
			for (int i = 0; i < planes.Length; i++)
			{
				if (planes[i].normal.x * center.x + planes[i].normal.y * center.y + planes[i].normal.z * center.z + planes[i].distance < -radius)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00006E54 File Offset: 0x00005054
		private void OnVolumetricLightPreRender(AzureVolumetricLightRenderer renderer)
		{
			if (this.m_light == null)
			{
				return;
			}
			if (this.m_propertyBlock == null)
			{
				this.m_propertyBlock = new MaterialPropertyBlock();
			}
			switch (this.m_light.type)
			{
			case LightType.Spot:
			{
				this.m_spotLightAngle = this.m_light.spotAngle * 0.5f * this.m_angleMultiplier;
				this.m_lightRange = this.m_light.range * this.m_rangeMultiplier;
				if (!this.CheckSphereCulling(renderer.cameraFrustumPlanes, base.transform.position, this.m_lightRange))
				{
					return;
				}
				this.m_spotLightAngleRad = this.m_spotLightAngle * 0.017453292f;
				this.m_spotLightAngleScale = Mathf.Tan(this.m_spotLightAngleRad) * this.m_lightRange * 2f;
				this.m_trsMatrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, new Vector3(this.m_spotLightAngleScale, this.m_spotLightAngleScale, this.m_lightRange));
				float sqrMagnitude = (renderer.camera.transform.position - base.transform.position).sqrMagnitude;
				this.m_distanceToCamera = Mathf.Sqrt(sqrMagnitude);
				float num = this.m_lightRange * this.m_lightRange;
				this.m_isCameraInsideSphereRange = ((sqrMagnitude < num) ? 1 : 0);
				this.m_isCameraInsideConeRange = 0;
				if (this.m_isCameraInsideSphereRange == 1)
				{
					this.m_spotToCameraVector = renderer.camera.transform.position - base.transform.position;
					this.m_spotAngleToCamera = Vector3.Angle(base.transform.forward, this.m_spotToCameraVector);
					this.m_isCameraInsideConeRange = ((this.m_spotAngleToCamera < this.m_spotLightAngle) ? 1 : 0);
				}
				this.m_propertyBlock.SetFloat(AzureShaderUniforms.VolumetricLightIntensity, this.m_light.intensity * this.m_intensityMultiplier);
				this.m_propertyBlock.SetColor(AzureShaderUniforms.VolumetricLightColor, this.m_light.color);
				this.m_propertyBlock.SetFloat(AzureShaderUniforms.VolumetricLightExtinctionDistance, renderer.lightExtinctionDistance);
				this.m_propertyBlock.SetFloat(AzureShaderUniforms.VolumetricLightExtinctionSmoothStep, renderer.lightExtinctionSmoothStep);
				this.m_propertyBlock.SetVector(AzureShaderUniforms.VolumetricLightMieG, this.ComputeMieG());
				this.m_propertyBlock.SetVector(AzureShaderUniforms.VolumetricLightWorldPosition, base.transform.position);
				renderer.commandBuffer.DrawMesh(this.m_lightMesh, this.m_trsMatrix, this.m_lightMaterial, 0, this.m_isCameraInsideConeRange, this.m_propertyBlock);
				return;
			}
			case LightType.Directional:
				break;
			case LightType.Point:
			{
				float sqrMagnitude2 = (base.transform.position - renderer.camera.transform.position).sqrMagnitude;
				this.m_distanceToCamera = Mathf.Sqrt(sqrMagnitude2);
				if (!this.CheckSphereCulling(renderer.cameraFrustumPlanes, base.transform.position, this.m_light.range))
				{
					return;
				}
				float num2 = this.m_light.range * this.m_light.range;
				this.m_isCameraInsideSphereRange = ((sqrMagnitude2 < num2) ? 1 : 0);
				this.m_lightRange = this.m_light.range * this.m_rangeMultiplier;
				this.m_trsMatrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, new Vector3(this.m_lightRange, this.m_lightRange, this.m_lightRange));
				this.m_propertyBlock.SetFloat(AzureShaderUniforms.VolumetricLightIntensity, this.m_light.intensity * this.m_intensityMultiplier);
				this.m_propertyBlock.SetColor(AzureShaderUniforms.VolumetricLightColor, this.m_light.color);
				this.m_propertyBlock.SetFloat(AzureShaderUniforms.VolumetricLightExtinctionDistance, renderer.lightExtinctionDistance);
				this.m_propertyBlock.SetFloat(AzureShaderUniforms.VolumetricLightExtinctionSmoothStep, renderer.lightExtinctionSmoothStep);
				this.m_propertyBlock.SetVector(AzureShaderUniforms.VolumetricLightMieG, this.ComputeMieG());
				renderer.commandBuffer.DrawMesh(this.m_lightMesh, this.m_trsMatrix, this.m_lightMaterial, 0, this.m_isCameraInsideSphereRange, this.m_propertyBlock);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000726F File Offset: 0x0000546F
		private Vector3 ComputeMieG()
		{
			return new Vector3(1f - this.m_mieDirectionalityFactor * this.m_mieDirectionalityFactor, 1f + this.m_mieDirectionalityFactor * this.m_mieDirectionalityFactor, 2f * this.m_mieDirectionalityFactor);
		}

		// Token: 0x0400014D RID: 333
		[SerializeField]
		private Light m_light;

		// Token: 0x0400014E RID: 334
		[SerializeField]
		private Mesh m_lightMesh;

		// Token: 0x0400014F RID: 335
		[SerializeField]
		private Material m_lightMaterial;

		// Token: 0x04000150 RID: 336
		[Range(0f, 0.9f)]
		[SerializeField]
		private float m_mieDirectionalityFactor = 0.75f;

		// Token: 0x04000151 RID: 337
		[Range(0f, 1f)]
		[SerializeField]
		private float m_intensityMultiplier = 0.7f;

		// Token: 0x04000152 RID: 338
		[Range(0f, 1f)]
		[SerializeField]
		private float m_rangeMultiplier = 1f;

		// Token: 0x04000153 RID: 339
		[Range(0f, 5f)]
		[SerializeField]
		private float m_angleMultiplier = 1f;

		// Token: 0x04000154 RID: 340
		private Matrix4x4 m_trsMatrix = Matrix4x4.identity;

		// Token: 0x04000155 RID: 341
		private float m_distanceToCamera;

		// Token: 0x04000156 RID: 342
		private int m_isCameraInsideSphereRange;

		// Token: 0x04000157 RID: 343
		private int m_isCameraInsideConeRange;

		// Token: 0x04000158 RID: 344
		[SerializeField]
		private MaterialPropertyBlock m_propertyBlock;

		// Token: 0x04000159 RID: 345
		private float m_spotLightAngle = 1f;

		// Token: 0x0400015A RID: 346
		private float m_spotLightAngleRad = 1f;

		// Token: 0x0400015B RID: 347
		private float m_spotLightAngleScale;

		// Token: 0x0400015C RID: 348
		private float m_lightRange = 1f;

		// Token: 0x0400015D RID: 349
		private float m_spotAngleToCamera;

		// Token: 0x0400015E RID: 350
		private Vector3 m_spotToCameraVector = Vector3.zero;
	}
}
