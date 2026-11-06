using System;
using Features.Detection;
using ProjectExhibitionist.FirstPerson;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x0200098E RID: 2446
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Renderer))]
	public sealed class PhotoBoothPlanarMirror : MonoBehaviour
	{
		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x060043B2 RID: 17330 RVA: 0x00195388 File Offset: 0x00193588
		public int ActiveTextureWidth
		{
			get
			{
				if (!(this._reflectionTexture != null))
				{
					return 0;
				}
				return this._reflectionTexture.width;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x060043B3 RID: 17331 RVA: 0x001953A5 File Offset: 0x001935A5
		public int ActiveTextureHeight
		{
			get
			{
				if (!(this._reflectionTexture != null))
				{
					return 0;
				}
				return this._reflectionTexture.height;
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x060043B4 RID: 17332 RVA: 0x000C43E2 File Offset: 0x000C25E2
		public bool IsReflectionEnabled
		{
			get
			{
				return base.enabled;
			}
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x001953C4 File Offset: 0x001935C4
		public void Configure(Renderer targetRenderer, Vector3 planeNormalInLocalSpace, int targetMaterialIndex = 0, Vector3? planePositionInLocalSpace = null)
		{
			this.mirrorRenderer = targetRenderer;
			this.materialIndex = Mathf.Max(0, targetMaterialIndex);
			this.useLocalPlanePosition = planePositionInLocalSpace != null;
			this.localPlanePosition = planePositionInLocalSpace.GetValueOrDefault();
			this.textureSize = Mathf.Max(this.textureSize, 2048);
			this.localPlaneNormal = ((planeNormalInLocalSpace.sqrMagnitude > 0.001f) ? planeNormalInLocalSpace.normalized : Vector3.forward);
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x00195437 File Offset: 0x00193637
		private void Awake()
		{
			if (this.mirrorRenderer == null)
			{
				this.mirrorRenderer = base.GetComponent<Renderer>();
			}
			this.SetReflectionEnabled(false);
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x0019545A File Offset: 0x0019365A
		public void SetReflectionEnabled(bool value)
		{
			if (base.enabled == value)
			{
				if (!value)
				{
					this.ReleaseReflectionResourcesAndMaterial();
				}
				return;
			}
			base.enabled = value;
			if (!value)
			{
				this.ReleaseReflectionResourcesAndMaterial();
			}
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x0019547F File Offset: 0x0019367F
		private void OnEnable()
		{
			RenderPipelineManager.beginCameraRendering -= this.OnBeginCameraRendering;
			RenderPipelineManager.beginCameraRendering += this.OnBeginCameraRendering;
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x001954A4 File Offset: 0x001936A4
		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera source)
		{
			if (!Application.isPlaying || !this.ShouldRenderReflection(source))
			{
				return;
			}
			this.EnsureMaterial();
			this.EnsureReflectionResources(source);
			if (this._reflectionCamera == null || this._reflectionTexture == null || this._runtimeMaterial == null)
			{
				return;
			}
			this.RenderReflection(context, source);
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x00195504 File Offset: 0x00193704
		private bool ShouldRenderReflection(Camera source)
		{
			if (!base.enabled || this.mirrorRenderer == null || !this.mirrorRenderer.enabled || this.mirrorRenderer.forceRenderingOff || PhotoBoothPlanarMirror.s_renderingReflection)
			{
				return false;
			}
			if (source == null || source == this._reflectionCamera || source.cameraType == CameraType.Preview)
			{
				return false;
			}
			if ((source.cullingMask & (1 << this.mirrorRenderer.gameObject.layer)) == 0)
			{
				return false;
			}
			string name = source.gameObject.name;
			return !(name == "NPC Ciel Portrait Camera") && !(name == "WantedPosterPhotoCamera") && GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(source), this.mirrorRenderer.bounds);
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x001955CC File Offset: 0x001937CC
		private void EnsureMaterial()
		{
			if (this._runtimeMaterial != null || this.mirrorRenderer == null)
			{
				return;
			}
			Shader shader = Shader.Find("Project Exhibitionist/Photo Booth/Planar Mirror");
			if (shader == null)
			{
				return;
			}
			Material[] sharedMaterials = this.mirrorRenderer.sharedMaterials;
			if (this.materialIndex >= sharedMaterials.Length)
			{
				return;
			}
			this._originalMaterial = sharedMaterials[this.materialIndex];
			this._runtimeMaterial = new Material(shader)
			{
				name = "PhotoBooth Mirror (Runtime)",
				hideFlags = HideFlags.DontSave
			};
			this._runtimeMaterial.SetColor("_Tint", new Color(0.92f, 0.96f, 1f, 1f));
			this._runtimeMaterial.SetFloat("_Brightness", 1.02f);
			sharedMaterials[this.materialIndex] = this._runtimeMaterial;
			this.mirrorRenderer.sharedMaterials = sharedMaterials;
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x001956A8 File Offset: 0x001938A8
		private void EnsureReflectionResources(Camera source)
		{
			int num = Mathf.Clamp(Mathf.Max(this.textureSize, source.pixelWidth), 256, 4096);
			int num2 = Mathf.Clamp(Mathf.RoundToInt((float)num / Mathf.Max(0.5f, source.aspect)), 256, 4096);
			if (this._reflectionTexture == null || this._lastTextureSize != num || this._lastTextureHeight != num2)
			{
				this.ReleaseTexture();
				this._reflectionTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default)
				{
					name = "PhotoBooth Mirror Reflection",
					hideFlags = HideFlags.DontSave,
					antiAliasing = 1,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					useMipMap = false,
					autoGenerateMips = false
				};
				this._reflectionTexture.Create();
				this._lastTextureSize = num;
				this._lastTextureHeight = num2;
				if (this._runtimeMaterial != null)
				{
					this._runtimeMaterial.SetTexture("_ReflectionTex", this._reflectionTexture);
				}
			}
			if (this._reflectionCamera != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("PhotoBooth Mirror Camera")
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			this._reflectionCamera = gameObject.AddComponent<Camera>();
			this._reflectionCamera.enabled = false;
			if (this._reflectionCamera.GetComponent<UniversalAdditionalCameraData>() == null)
			{
				this._reflectionCamera.gameObject.AddComponent<UniversalAdditionalCameraData>();
			}
		}

		// Token: 0x060043BD RID: 17341 RVA: 0x0019580C File Offset: 0x00193A0C
		private void RenderReflection(ScriptableRenderContext context, Camera source)
		{
			Vector3 vector = (this.useLocalPlanePosition ? base.transform.TransformPoint(this.localPlanePosition) : this.mirrorRenderer.bounds.center);
			Vector3 vector2 = base.transform.TransformDirection(this.localPlaneNormal).normalized;
			if (Vector3.Dot(vector2, source.transform.position - vector) < 0f)
			{
				vector2 = -vector2;
			}
			this._reflectionCamera.CopyFrom(source);
			this._reflectionCamera.enabled = false;
			this._reflectionCamera.targetTexture = this._reflectionTexture;
			this._reflectionCamera.aspect = source.aspect;
			this._reflectionCamera.cullingMask = this.reflectionMask;
			this._reflectionCamera.useOcclusionCulling = false;
			this._reflectionCamera.allowHDR = true;
			this._reflectionCamera.allowMSAA = false;
			float num = -Vector3.Dot(vector2, vector) - this.clipPlaneOffset;
			Matrix4x4 matrix4x = PhotoBoothPlanarMirror.CalculateReflectionMatrix(new Vector4(vector2.x, vector2.y, vector2.z, num));
			this._reflectionCamera.worldToCameraMatrix = source.worldToCameraMatrix * matrix4x;
			Vector4 vector3 = PhotoBoothPlanarMirror.CameraSpacePlane(this._reflectionCamera, vector, vector2, 1f, this.clipPlaneOffset);
			this._reflectionCamera.projectionMatrix = source.CalculateObliqueMatrix(vector3);
			bool forceRenderingOff = this.mirrorRenderer.forceRenderingOff;
			bool invertCulling = GL.invertCulling;
			try
			{
				PhotoBoothPlanarMirror.s_renderingReflection = true;
				this.mirrorRenderer.forceRenderingOff = true;
				GL.invertCulling = true;
				using (FirstPersonController.AllowHeadInAuxiliaryRendering())
				{
					using (DetectionRenderVisibilityScope.AllowAuxiliaryRendering())
					{
						UniversalRenderPipeline.RenderSingleCamera(context, this._reflectionCamera);
					}
				}
			}
			finally
			{
				GL.invertCulling = invertCulling;
				this.mirrorRenderer.forceRenderingOff = forceRenderingOff;
				PhotoBoothPlanarMirror.s_renderingReflection = false;
			}
		}

		// Token: 0x060043BE RID: 17342 RVA: 0x00195A14 File Offset: 0x00193C14
		private static Vector4 CameraSpacePlane(Camera camera, Vector3 position, Vector3 normal, float sideSign, float offset)
		{
			Vector3 vector = position + normal * offset;
			Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
			Vector3 vector2 = worldToCameraMatrix.MultiplyPoint(vector);
			Vector3 vector3 = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
			return new Vector4(vector3.x, vector3.y, vector3.z, -Vector3.Dot(vector2, vector3));
		}

		// Token: 0x060043BF RID: 17343 RVA: 0x00195A78 File Offset: 0x00193C78
		private static Matrix4x4 CalculateReflectionMatrix(Vector4 plane)
		{
			Matrix4x4 identity = Matrix4x4.identity;
			identity.m00 = 1f - 2f * plane[0] * plane[0];
			identity.m01 = -2f * plane[0] * plane[1];
			identity.m02 = -2f * plane[0] * plane[2];
			identity.m03 = -2f * plane[3] * plane[0];
			identity.m10 = -2f * plane[1] * plane[0];
			identity.m11 = 1f - 2f * plane[1] * plane[1];
			identity.m12 = -2f * plane[1] * plane[2];
			identity.m13 = -2f * plane[3] * plane[1];
			identity.m20 = -2f * plane[2] * plane[0];
			identity.m21 = -2f * plane[2] * plane[1];
			identity.m22 = 1f - 2f * plane[2] * plane[2];
			identity.m23 = -2f * plane[3] * plane[2];
			return identity;
		}

		// Token: 0x060043C0 RID: 17344 RVA: 0x00195C06 File Offset: 0x00193E06
		private void OnDisable()
		{
			RenderPipelineManager.beginCameraRendering -= this.OnBeginCameraRendering;
			this.ReleaseReflectionResourcesAndMaterial();
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x00195C1F File Offset: 0x00193E1F
		private void OnDestroy()
		{
			this.ReleaseReflectionResourcesAndMaterial();
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x00195C28 File Offset: 0x00193E28
		private void ReleaseReflectionResourcesAndMaterial()
		{
			this.ReleaseReflectionResources();
			if (this._runtimeMaterial != null)
			{
				if (this.mirrorRenderer != null)
				{
					Material[] sharedMaterials = this.mirrorRenderer.sharedMaterials;
					if (this.materialIndex < sharedMaterials.Length && sharedMaterials[this.materialIndex] == this._runtimeMaterial)
					{
						sharedMaterials[this.materialIndex] = this._originalMaterial;
						this.mirrorRenderer.sharedMaterials = sharedMaterials;
					}
				}
				PhotoBoothPlanarMirror.DestroyResource(this._runtimeMaterial);
				this._runtimeMaterial = null;
				this._originalMaterial = null;
			}
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x00195CB6 File Offset: 0x00193EB6
		private void ReleaseReflectionResources()
		{
			this.ReleaseTexture();
			if (this._reflectionCamera == null)
			{
				return;
			}
			this._reflectionCamera.enabled = false;
			PhotoBoothPlanarMirror.DestroyResource(this._reflectionCamera.gameObject);
			this._reflectionCamera = null;
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void DestroyResource(Object resource)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(resource);
				return;
			}
			Object.DestroyImmediate(resource);
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x00195CF0 File Offset: 0x00193EF0
		private void ReleaseTexture()
		{
			if (this._reflectionTexture == null)
			{
				return;
			}
			this._reflectionTexture.Release();
			PhotoBoothPlanarMirror.DestroyResource(this._reflectionTexture);
			this._reflectionTexture = null;
		}

		// Token: 0x0400436E RID: 17262
		[SerializeField]
		private Renderer mirrorRenderer;

		// Token: 0x0400436F RID: 17263
		[SerializeField]
		private Vector3 localPlaneNormal = Vector3.forward;

		// Token: 0x04004370 RID: 17264
		[SerializeField]
		[Min(0f)]
		private int materialIndex;

		// Token: 0x04004371 RID: 17265
		[SerializeField]
		private bool useLocalPlanePosition;

		// Token: 0x04004372 RID: 17266
		[SerializeField]
		private Vector3 localPlanePosition;

		// Token: 0x04004373 RID: 17267
		[SerializeField]
		[Min(256f)]
		private int textureSize = 2048;

		// Token: 0x04004374 RID: 17268
		[SerializeField]
		[Range(0.001f, 0.1f)]
		private float clipPlaneOffset = 0.015f;

		// Token: 0x04004375 RID: 17269
		[SerializeField]
		private LayerMask reflectionMask = -1;

		// Token: 0x04004376 RID: 17270
		private static bool s_renderingReflection;

		// Token: 0x04004377 RID: 17271
		private Camera _reflectionCamera;

		// Token: 0x04004378 RID: 17272
		private RenderTexture _reflectionTexture;

		// Token: 0x04004379 RID: 17273
		private Material _runtimeMaterial;

		// Token: 0x0400437A RID: 17274
		private Material _originalMaterial;

		// Token: 0x0400437B RID: 17275
		private int _lastTextureSize;

		// Token: 0x0400437C RID: 17276
		private int _lastTextureHeight;
	}
}
