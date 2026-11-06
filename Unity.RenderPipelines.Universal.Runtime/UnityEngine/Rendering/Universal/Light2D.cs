using System;
using UnityEngine.Animations;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using UnityEngine.U2D;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000025 RID: 37
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[MovedFrom("UnityEngine.Experimental.Rendering.Universal")]
	[AddComponentMenu("Rendering/2D/Light 2D")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/manual/2DLightProperties.html")]
	public sealed class Light2D : Light2DBase, ISerializationCallbackReceiver
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600011F RID: 287 RVA: 0x0000BB99 File Offset: 0x00009D99
		// (set) Token: 0x06000120 RID: 288 RVA: 0x0000BBA1 File Offset: 0x00009DA1
		internal LightUtility.LightMeshVertex[] vertices
		{
			get
			{
				return this.m_Vertices;
			}
			set
			{
				this.m_Vertices = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000121 RID: 289 RVA: 0x0000BBAA File Offset: 0x00009DAA
		// (set) Token: 0x06000122 RID: 290 RVA: 0x0000BBB2 File Offset: 0x00009DB2
		internal ushort[] indices
		{
			get
			{
				return this.m_Triangles;
			}
			set
			{
				this.m_Triangles = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000BBBB File Offset: 0x00009DBB
		internal int[] affectedSortingLayers
		{
			get
			{
				return this.m_ApplyToSortingLayers;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000124 RID: 292 RVA: 0x0000BBC3 File Offset: 0x00009DC3
		private int lightCookieSpriteInstanceID
		{
			get
			{
				Sprite lightCookieSprite = this.m_LightCookieSprite;
				if (lightCookieSprite == null)
				{
					return 0;
				}
				return lightCookieSprite.GetInstanceID();
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000125 RID: 293 RVA: 0x0000BBD6 File Offset: 0x00009DD6
		// (set) Token: 0x06000126 RID: 294 RVA: 0x0000BBDE File Offset: 0x00009DDE
		internal BoundingSphere boundingSphere { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000127 RID: 295 RVA: 0x0000BBE7 File Offset: 0x00009DE7
		internal Mesh lightMesh
		{
			get
			{
				if (null == this.m_Mesh)
				{
					this.m_Mesh = new Mesh();
				}
				return this.m_Mesh;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000BC08 File Offset: 0x00009E08
		internal bool hasCachedMesh
		{
			get
			{
				return this.vertices.Length > 1 && this.indices.Length > 1;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0000BC22 File Offset: 0x00009E22
		// (set) Token: 0x0600012A RID: 298 RVA: 0x0000BC2A File Offset: 0x00009E2A
		public Light2D.LightType lightType
		{
			get
			{
				return this.m_LightType;
			}
			set
			{
				if (this.m_LightType != value)
				{
					this.UpdateMesh(false);
				}
				this.m_LightType = value;
				Light2DManager.ErrorIfDuplicateGlobalLight(this);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000BC49 File Offset: 0x00009E49
		// (set) Token: 0x0600012C RID: 300 RVA: 0x0000BC51 File Offset: 0x00009E51
		public int blendStyleIndex
		{
			get
			{
				return this.m_BlendStyleIndex;
			}
			set
			{
				this.m_BlendStyleIndex = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000BC5A File Offset: 0x00009E5A
		// (set) Token: 0x0600012E RID: 302 RVA: 0x0000BC62 File Offset: 0x00009E62
		public float shadowIntensity
		{
			get
			{
				return this.m_ShadowIntensity;
			}
			set
			{
				this.m_ShadowIntensity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000BC70 File Offset: 0x00009E70
		// (set) Token: 0x06000130 RID: 304 RVA: 0x0000BC78 File Offset: 0x00009E78
		public bool shadowsEnabled
		{
			get
			{
				return this.m_ShadowIntensityEnabled;
			}
			set
			{
				this.m_ShadowIntensityEnabled = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000131 RID: 305 RVA: 0x0000BC81 File Offset: 0x00009E81
		// (set) Token: 0x06000132 RID: 306 RVA: 0x0000BC89 File Offset: 0x00009E89
		public float shadowVolumeIntensity
		{
			get
			{
				return this.m_ShadowVolumeIntensity;
			}
			set
			{
				this.m_ShadowVolumeIntensity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000133 RID: 307 RVA: 0x0000BC97 File Offset: 0x00009E97
		// (set) Token: 0x06000134 RID: 308 RVA: 0x0000BC9F File Offset: 0x00009E9F
		public bool volumetricShadowsEnabled
		{
			get
			{
				return this.m_ShadowVolumeIntensityEnabled;
			}
			set
			{
				this.m_ShadowVolumeIntensityEnabled = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000BCA8 File Offset: 0x00009EA8
		// (set) Token: 0x06000136 RID: 310 RVA: 0x0000BCB0 File Offset: 0x00009EB0
		public Color color
		{
			get
			{
				return this.m_Color;
			}
			set
			{
				this.m_Color = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000BCB9 File Offset: 0x00009EB9
		// (set) Token: 0x06000138 RID: 312 RVA: 0x0000BCC1 File Offset: 0x00009EC1
		public float intensity
		{
			get
			{
				return this.m_Intensity;
			}
			set
			{
				this.m_Intensity = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000BCCA File Offset: 0x00009ECA
		[Obsolete]
		public float volumeOpacity
		{
			get
			{
				return this.m_LightVolumeIntensity;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000BCD2 File Offset: 0x00009ED2
		// (set) Token: 0x0600013B RID: 315 RVA: 0x0000BCDA File Offset: 0x00009EDA
		public float volumeIntensity
		{
			get
			{
				return this.m_LightVolumeIntensity;
			}
			set
			{
				this.m_LightVolumeIntensity = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000BCE3 File Offset: 0x00009EE3
		// (set) Token: 0x0600013D RID: 317 RVA: 0x0000BCEB File Offset: 0x00009EEB
		public bool volumeIntensityEnabled
		{
			get
			{
				return this.m_LightVolumeIntensityEnabled;
			}
			set
			{
				this.m_LightVolumeIntensityEnabled = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000BCF4 File Offset: 0x00009EF4
		// (set) Token: 0x0600013F RID: 319 RVA: 0x0000BD0C File Offset: 0x00009F0C
		public Sprite lightCookieSprite
		{
			get
			{
				if (this.m_LightType == Light2D.LightType.Point)
				{
					return this.m_DeprecatedPointLightCookieSprite;
				}
				return this.m_LightCookieSprite;
			}
			set
			{
				this.m_LightCookieSprite = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000BD15 File Offset: 0x00009F15
		// (set) Token: 0x06000141 RID: 321 RVA: 0x0000BD1D File Offset: 0x00009F1D
		public float falloffIntensity
		{
			get
			{
				return this.m_FalloffIntensity;
			}
			set
			{
				this.m_FalloffIntensity = Mathf.Clamp(value, 0f, 1f);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000BD35 File Offset: 0x00009F35
		[Obsolete]
		public bool alphaBlendOnOverlap
		{
			get
			{
				return this.m_OverlapOperation == Light2D.OverlapOperation.AlphaBlend;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000BD40 File Offset: 0x00009F40
		// (set) Token: 0x06000144 RID: 324 RVA: 0x0000BD48 File Offset: 0x00009F48
		public Light2D.OverlapOperation overlapOperation
		{
			get
			{
				return this.m_OverlapOperation;
			}
			set
			{
				this.m_OverlapOperation = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000BD51 File Offset: 0x00009F51
		// (set) Token: 0x06000146 RID: 326 RVA: 0x0000BD59 File Offset: 0x00009F59
		public int lightOrder
		{
			get
			{
				return this.m_LightOrder;
			}
			set
			{
				this.m_LightOrder = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000BD62 File Offset: 0x00009F62
		public float normalMapDistance
		{
			get
			{
				return this.m_NormalMapDistance;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000BD6A File Offset: 0x00009F6A
		public Light2D.NormalMapQuality normalMapQuality
		{
			get
			{
				return this.m_NormalMapQuality;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000BD72 File Offset: 0x00009F72
		public bool renderVolumetricShadows
		{
			get
			{
				return this.volumetricShadowsEnabled && this.shadowVolumeIntensity > 0f;
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000BD8B File Offset: 0x00009F8B
		internal void MarkForUpdate()
		{
			this.forceUpdate = true;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000BD94 File Offset: 0x00009F94
		internal void CacheValues()
		{
			this.m_CachedPosition = base.transform.position;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000BDA8 File Offset: 0x00009FA8
		internal int GetTopMostLitLayer()
		{
			int num = int.MinValue;
			int num2 = 0;
			SortingLayer[] cachedSortingLayer = Light2DManager.GetCachedSortingLayer();
			for (int i = 0; i < this.m_ApplyToSortingLayers.Length; i++)
			{
				for (int j = cachedSortingLayer.Length - 1; j >= num2; j--)
				{
					if (cachedSortingLayer[j].id == this.m_ApplyToSortingLayers[i])
					{
						num = cachedSortingLayer[j].value;
						num2 = j;
					}
				}
			}
			return num;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000BE14 File Offset: 0x0000A014
		internal Bounds UpdateSpriteMesh()
		{
			if (this.m_LightCookieSprite == null && (this.m_Vertices.Length != 1 || this.m_Triangles.Length != 1))
			{
				this.m_Vertices = new LightUtility.LightMeshVertex[1];
				this.m_Triangles = new ushort[1];
			}
			return LightUtility.GenerateSpriteMesh(this, this.m_LightCookieSprite);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000BE6C File Offset: 0x0000A06C
		internal void UpdateMesh(bool forceUpdate = false)
		{
			int shapePathHash = LightUtility.GetShapePathHash(this.shapePath);
			bool flag = LightUtility.CheckForChange(this.m_ShapeLightFalloffSize, ref this.m_PreviousShapeLightFalloffSize);
			bool flag2 = LightUtility.CheckForChange(this.m_ShapeLightParametricRadius, ref this.m_PreviousShapeLightParametricRadius);
			bool flag3 = LightUtility.CheckForChange(this.m_ShapeLightParametricSides, ref this.m_PreviousShapeLightParametricSides);
			bool flag4 = LightUtility.CheckForChange(this.m_ShapeLightParametricAngleOffset, ref this.m_PreviousShapeLightParametricAngleOffset);
			bool flag5 = LightUtility.CheckForChange(this.lightCookieSpriteInstanceID, ref this.m_PreviousLightCookieSprite);
			bool flag6 = LightUtility.CheckForChange(shapePathHash, ref this.m_PreviousShapePathHash);
			bool flag7 = LightUtility.CheckForChange(this.m_LightType, ref this.m_PreviousLightType);
			if (flag || flag2 || flag3 || flag4 || flag5 || flag6 || flag7 || forceUpdate)
			{
				switch (this.m_LightType)
				{
				case Light2D.LightType.Parametric:
					this.m_LocalBounds = LightUtility.GenerateParametricMesh(this, this.m_ShapeLightParametricRadius, this.m_ShapeLightFalloffSize, this.m_ShapeLightParametricAngleOffset, this.m_ShapeLightParametricSides);
					return;
				case Light2D.LightType.Freeform:
					this.m_LocalBounds = LightUtility.GenerateShapeMesh(this, this.m_ShapePath, this.m_ShapeLightFalloffSize);
					return;
				case Light2D.LightType.Sprite:
					this.m_LocalBounds = this.UpdateSpriteMesh();
					return;
				case Light2D.LightType.Point:
					this.m_LocalBounds = LightUtility.GenerateParametricMesh(this, 1.412135f, 0f, 0f, 4);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
		internal void UpdateBoundingSphere()
		{
			if (this.isPointLight)
			{
				this.boundingSphere = new BoundingSphere(base.transform.position, this.m_PointLightOuterRadius);
				return;
			}
			Vector3 vector = base.transform.TransformPoint(Vector3.Max(this.m_LocalBounds.max, this.m_LocalBounds.max + this.m_ShapeLightFalloffOffset));
			Vector3 vector2 = base.transform.TransformPoint(Vector3.Min(this.m_LocalBounds.min, this.m_LocalBounds.min + this.m_ShapeLightFalloffOffset));
			Vector3 vector3 = 0.5f * (vector + vector2);
			float num = Vector3.Magnitude(vector - vector3);
			this.boundingSphere = new BoundingSphere(vector3, num);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000C06C File Offset: 0x0000A26C
		internal bool IsLitLayer(int layer)
		{
			if (this.m_ApplyToSortingLayers == null)
			{
				return false;
			}
			for (int i = 0; i < this.m_ApplyToSortingLayers.Length; i++)
			{
				if (this.m_ApplyToSortingLayers[i] == layer)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000C0A4 File Offset: 0x0000A2A4
		private void Awake()
		{
			if (this.m_ApplyToSortingLayers == null)
			{
				this.m_ApplyToSortingLayers = new int[SortingLayer.layers.Length];
				for (int i = 0; i < this.m_ApplyToSortingLayers.Length; i++)
				{
					this.m_ApplyToSortingLayers[i] = SortingLayer.layers[i].id;
				}
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000C0F6 File Offset: 0x0000A2F6
		private void OnEnable()
		{
			this.m_PreviousLightCookieSprite = this.lightCookieSpriteInstanceID;
			Light2DManager.RegisterLight(this);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000C10A File Offset: 0x0000A30A
		private void OnDisable()
		{
			Light2DManager.DeregisterLight(this);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000C112 File Offset: 0x0000A312
		private void LateUpdate()
		{
			if (this.m_LightType == Light2D.LightType.Global)
			{
				return;
			}
			this.UpdateMesh(this.forceUpdate);
			this.UpdateBoundingSphere();
			this.forceUpdate = false;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000C137 File Offset: 0x0000A337
		public void OnBeforeSerialize()
		{
			this.m_ComponentVersion = Light2D.ComponentVersions.Version_1;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000C140 File Offset: 0x0000A340
		public void OnAfterDeserialize()
		{
			if (this.m_ComponentVersion == Light2D.ComponentVersions.Version_Unserialized)
			{
				this.m_ShadowVolumeIntensityEnabled = this.m_ShadowVolumeIntensity > 0f;
				this.m_ShadowIntensityEnabled = this.m_ShadowIntensity > 0f;
				this.m_LightVolumeIntensityEnabled = this.m_LightVolumeIntensity > 0f;
				this.m_NormalMapQuality = ((!this.m_UseNormalMap) ? Light2D.NormalMapQuality.Disabled : this.m_NormalMapQuality);
				this.m_OverlapOperation = (this.m_AlphaBlendOnOverlap ? Light2D.OverlapOperation.AlphaBlend : this.m_OverlapOperation);
				this.m_ComponentVersion = Light2D.ComponentVersions.Version_1;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000C1C3 File Offset: 0x0000A3C3
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000C1CB File Offset: 0x0000A3CB
		public float pointLightInnerAngle
		{
			get
			{
				return this.m_PointLightInnerAngle;
			}
			set
			{
				this.m_PointLightInnerAngle = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000C1D4 File Offset: 0x0000A3D4
		// (set) Token: 0x0600015A RID: 346 RVA: 0x0000C1DC File Offset: 0x0000A3DC
		public float pointLightOuterAngle
		{
			get
			{
				return this.m_PointLightOuterAngle;
			}
			set
			{
				this.m_PointLightOuterAngle = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000C1E5 File Offset: 0x0000A3E5
		// (set) Token: 0x0600015C RID: 348 RVA: 0x0000C1ED File Offset: 0x0000A3ED
		public float pointLightInnerRadius
		{
			get
			{
				return this.m_PointLightInnerRadius;
			}
			set
			{
				this.m_PointLightInnerRadius = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600015D RID: 349 RVA: 0x0000C1F6 File Offset: 0x0000A3F6
		// (set) Token: 0x0600015E RID: 350 RVA: 0x0000C1FE File Offset: 0x0000A3FE
		public float pointLightOuterRadius
		{
			get
			{
				return this.m_PointLightOuterRadius;
			}
			set
			{
				this.m_PointLightOuterRadius = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000C207 File Offset: 0x0000A407
		[Obsolete("pointLightDistance has been changed to normalMapDistance", true)]
		public float pointLightDistance
		{
			get
			{
				return this.m_NormalMapDistance;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000C20F File Offset: 0x0000A40F
		[Obsolete("pointLightQuality has been changed to normalMapQuality", true)]
		public Light2D.NormalMapQuality pointLightQuality
		{
			get
			{
				return this.m_NormalMapQuality;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000C217 File Offset: 0x0000A417
		internal bool isPointLight
		{
			get
			{
				return this.m_LightType == Light2D.LightType.Point;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000C222 File Offset: 0x0000A422
		public int shapeLightParametricSides
		{
			get
			{
				return this.m_ShapeLightParametricSides;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000C22A File Offset: 0x0000A42A
		public float shapeLightParametricAngleOffset
		{
			get
			{
				return this.m_ShapeLightParametricAngleOffset;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000C232 File Offset: 0x0000A432
		// (set) Token: 0x06000165 RID: 357 RVA: 0x0000C23A File Offset: 0x0000A43A
		public float shapeLightParametricRadius
		{
			get
			{
				return this.m_ShapeLightParametricRadius;
			}
			internal set
			{
				this.m_ShapeLightParametricRadius = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000C243 File Offset: 0x0000A443
		// (set) Token: 0x06000167 RID: 359 RVA: 0x0000C24B File Offset: 0x0000A44B
		public float shapeLightFalloffSize
		{
			get
			{
				return this.m_ShapeLightFalloffSize;
			}
			set
			{
				this.m_ShapeLightFalloffSize = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000168 RID: 360 RVA: 0x0000C25E File Offset: 0x0000A45E
		// (set) Token: 0x06000169 RID: 361 RVA: 0x0000C266 File Offset: 0x0000A466
		public Vector3[] shapePath
		{
			get
			{
				return this.m_ShapePath;
			}
			internal set
			{
				this.m_ShapePath = value;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000C26F File Offset: 0x0000A46F
		public void SetShapePath(Vector3[] path)
		{
			this.m_ShapePath = path;
		}

		// Token: 0x040000AB RID: 171
		private const Light2D.ComponentVersions k_CurrentComponentVersion = Light2D.ComponentVersions.Version_1;

		// Token: 0x040000AC RID: 172
		[SerializeField]
		private Light2D.ComponentVersions m_ComponentVersion;

		// Token: 0x040000AD RID: 173
		[NotKeyable]
		[SerializeField]
		private Light2D.LightType m_LightType = Light2D.LightType.Point;

		// Token: 0x040000AE RID: 174
		[SerializeField]
		[FormerlySerializedAs("m_LightOperationIndex")]
		private int m_BlendStyleIndex;

		// Token: 0x040000AF RID: 175
		[SerializeField]
		private float m_FalloffIntensity = 0.5f;

		// Token: 0x040000B0 RID: 176
		[ColorUsage(true)]
		[SerializeField]
		private Color m_Color = Color.white;

		// Token: 0x040000B1 RID: 177
		[SerializeField]
		private float m_Intensity = 1f;

		// Token: 0x040000B2 RID: 178
		[FormerlySerializedAs("m_LightVolumeOpacity")]
		[SerializeField]
		private float m_LightVolumeIntensity = 1f;

		// Token: 0x040000B3 RID: 179
		[SerializeField]
		private bool m_LightVolumeIntensityEnabled;

		// Token: 0x040000B4 RID: 180
		[SerializeField]
		private int[] m_ApplyToSortingLayers;

		// Token: 0x040000B5 RID: 181
		[Reload("Textures/2D/Sparkle.png", ReloadAttribute.Package.Root)]
		[SerializeField]
		private Sprite m_LightCookieSprite;

		// Token: 0x040000B6 RID: 182
		[FormerlySerializedAs("m_LightCookieSprite")]
		[SerializeField]
		private Sprite m_DeprecatedPointLightCookieSprite;

		// Token: 0x040000B7 RID: 183
		[SerializeField]
		private int m_LightOrder;

		// Token: 0x040000B8 RID: 184
		[SerializeField]
		private bool m_AlphaBlendOnOverlap;

		// Token: 0x040000B9 RID: 185
		[SerializeField]
		private Light2D.OverlapOperation m_OverlapOperation;

		// Token: 0x040000BA RID: 186
		[FormerlySerializedAs("m_PointLightDistance")]
		[SerializeField]
		private float m_NormalMapDistance = 3f;

		// Token: 0x040000BB RID: 187
		[NotKeyable]
		[FormerlySerializedAs("m_PointLightQuality")]
		[SerializeField]
		private Light2D.NormalMapQuality m_NormalMapQuality = Light2D.NormalMapQuality.Disabled;

		// Token: 0x040000BC RID: 188
		[SerializeField]
		private bool m_UseNormalMap;

		// Token: 0x040000BD RID: 189
		[SerializeField]
		private bool m_ShadowIntensityEnabled;

		// Token: 0x040000BE RID: 190
		[Range(0f, 1f)]
		[SerializeField]
		private float m_ShadowIntensity = 0.75f;

		// Token: 0x040000BF RID: 191
		[SerializeField]
		private bool m_ShadowVolumeIntensityEnabled;

		// Token: 0x040000C0 RID: 192
		[Range(0f, 1f)]
		[SerializeField]
		private float m_ShadowVolumeIntensity = 0.75f;

		// Token: 0x040000C1 RID: 193
		private Mesh m_Mesh;

		// Token: 0x040000C2 RID: 194
		[NonSerialized]
		private LightUtility.LightMeshVertex[] m_Vertices = new LightUtility.LightMeshVertex[1];

		// Token: 0x040000C3 RID: 195
		[NonSerialized]
		private ushort[] m_Triangles = new ushort[1];

		// Token: 0x040000C4 RID: 196
		private int m_PreviousLightCookieSprite;

		// Token: 0x040000C5 RID: 197
		internal Vector3 m_CachedPosition;

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		private Bounds m_LocalBounds;

		// Token: 0x040000C8 RID: 200
		internal bool forceUpdate;

		// Token: 0x040000C9 RID: 201
		[SerializeField]
		private float m_PointLightInnerAngle = 360f;

		// Token: 0x040000CA RID: 202
		[SerializeField]
		private float m_PointLightOuterAngle = 360f;

		// Token: 0x040000CB RID: 203
		[SerializeField]
		private float m_PointLightInnerRadius;

		// Token: 0x040000CC RID: 204
		[SerializeField]
		private float m_PointLightOuterRadius = 1f;

		// Token: 0x040000CD RID: 205
		[SerializeField]
		private int m_ShapeLightParametricSides = 5;

		// Token: 0x040000CE RID: 206
		[SerializeField]
		private float m_ShapeLightParametricAngleOffset;

		// Token: 0x040000CF RID: 207
		[SerializeField]
		private float m_ShapeLightParametricRadius = 1f;

		// Token: 0x040000D0 RID: 208
		[SerializeField]
		private float m_ShapeLightFalloffSize = 0.5f;

		// Token: 0x040000D1 RID: 209
		[SerializeField]
		private Vector2 m_ShapeLightFalloffOffset = Vector2.zero;

		// Token: 0x040000D2 RID: 210
		[SerializeField]
		private Vector3[] m_ShapePath;

		// Token: 0x040000D3 RID: 211
		private float m_PreviousShapeLightFalloffSize = -1f;

		// Token: 0x040000D4 RID: 212
		private int m_PreviousShapeLightParametricSides = -1;

		// Token: 0x040000D5 RID: 213
		private float m_PreviousShapeLightParametricAngleOffset = -1f;

		// Token: 0x040000D6 RID: 214
		private float m_PreviousShapeLightParametricRadius = -1f;

		// Token: 0x040000D7 RID: 215
		private int m_PreviousShapePathHash = -1;

		// Token: 0x040000D8 RID: 216
		private Light2D.LightType m_PreviousLightType;

		// Token: 0x02000150 RID: 336
		public enum DeprecatedLightType
		{
			// Token: 0x0400096B RID: 2411
			Parametric
		}

		// Token: 0x02000151 RID: 337
		public enum LightType
		{
			// Token: 0x0400096D RID: 2413
			Parametric,
			// Token: 0x0400096E RID: 2414
			Freeform,
			// Token: 0x0400096F RID: 2415
			Sprite,
			// Token: 0x04000970 RID: 2416
			Point,
			// Token: 0x04000971 RID: 2417
			Global
		}

		// Token: 0x02000152 RID: 338
		public enum NormalMapQuality
		{
			// Token: 0x04000973 RID: 2419
			Disabled = 2,
			// Token: 0x04000974 RID: 2420
			Fast = 0,
			// Token: 0x04000975 RID: 2421
			Accurate
		}

		// Token: 0x02000153 RID: 339
		public enum OverlapOperation
		{
			// Token: 0x04000977 RID: 2423
			Additive,
			// Token: 0x04000978 RID: 2424
			AlphaBlend
		}

		// Token: 0x02000154 RID: 340
		private enum ComponentVersions
		{
			// Token: 0x0400097A RID: 2426
			Version_Unserialized,
			// Token: 0x0400097B RID: 2427
			Version_1
		}
	}
}
