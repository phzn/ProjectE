using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000039 RID: 57
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("Rendering/2D/Shadow Caster 2D")]
	[MovedFrom("UnityEngine.Experimental.Rendering.Universal")]
	public class ShadowCaster2D : ShadowCasterGroup2D, ISerializationCallbackReceiver
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0001193D File Offset: 0x0000FB3D
		public Mesh mesh
		{
			get
			{
				return this.m_Mesh;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00011945 File Offset: 0x0000FB45
		public Vector3[] shapePath
		{
			get
			{
				return this.m_ShapePath;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0001194D File Offset: 0x0000FB4D
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00011955 File Offset: 0x0000FB55
		internal int shapePathHash
		{
			get
			{
				return this.m_ShapePathHash;
			}
			set
			{
				this.m_ShapePathHash = value;
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00011960 File Offset: 0x0000FB60
		internal override void CacheValues()
		{
			this.m_CachedPosition = base.transform.position;
			this.m_CachedLossyScale = base.transform.lossyScale;
			this.m_CachedRotation = base.transform.rotation;
			this.m_CachedShadowMatrix = Matrix4x4.TRS(this.m_CachedPosition, this.m_CachedRotation, Vector3.one);
			this.m_CachedInverseShadowMatrix = this.m_CachedShadowMatrix.inverse;
			this.m_CachedLocalToWorldMatrix = base.transform.localToWorldMatrix;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000237 RID: 567 RVA: 0x000119E7 File Offset: 0x0000FBE7
		// (set) Token: 0x06000236 RID: 566 RVA: 0x000119DE File Offset: 0x0000FBDE
		public bool useRendererSilhouette
		{
			get
			{
				return this.m_UseRendererSilhouette && this.m_HasRenderer;
			}
			set
			{
				this.m_UseRendererSilhouette = value;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00011A02 File Offset: 0x0000FC02
		// (set) Token: 0x06000238 RID: 568 RVA: 0x000119F9 File Offset: 0x0000FBF9
		public bool selfShadows
		{
			get
			{
				return this.m_SelfShadows;
			}
			set
			{
				this.m_SelfShadows = value;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00011A13 File Offset: 0x0000FC13
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00011A0A File Offset: 0x0000FC0A
		public bool castsShadows
		{
			get
			{
				return this.m_CastsShadows;
			}
			set
			{
				this.m_CastsShadows = value;
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00011A1C File Offset: 0x0000FC1C
		private static int[] SetDefaultSortingLayers()
		{
			int num = SortingLayer.layers.Length;
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = SortingLayer.layers[i].id;
			}
			return array;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00011A58 File Offset: 0x0000FC58
		internal bool IsLit(Light2D light)
		{
			Vector3 vector;
			vector.x = light.m_CachedPosition.x - this.m_BoundingSphere.position.x;
			vector.y = light.m_CachedPosition.y - this.m_BoundingSphere.position.y;
			vector.z = light.m_CachedPosition.z - this.m_BoundingSphere.position.z;
			float num = Vector3.SqrMagnitude(vector);
			float num2 = light.boundingSphere.radius + this.m_BoundingSphere.radius;
			return num <= num2 * num2;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00011AF4 File Offset: 0x0000FCF4
		internal bool IsShadowedLayer(int layer)
		{
			return this.m_ApplyToSortingLayers != null && Array.IndexOf<int>(this.m_ApplyToSortingLayers, layer) >= 0;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00011B14 File Offset: 0x0000FD14
		private void Awake()
		{
			if (this.m_ApplyToSortingLayers == null)
			{
				this.m_ApplyToSortingLayers = ShadowCaster2D.SetDefaultSortingLayers();
			}
			Bounds bounds = new Bounds(base.transform.position, Vector3.one);
			Renderer component = base.GetComponent<Renderer>();
			if (component != null)
			{
				bounds = component.bounds;
			}
			else
			{
				Collider2D component2 = base.GetComponent<Collider2D>();
				if (component2 != null)
				{
					bounds = component2.bounds;
				}
			}
			Vector3 zero = Vector3.zero;
			Vector3 position = base.transform.position;
			if (base.transform.lossyScale.x != 0f && base.transform.lossyScale.y != 0f)
			{
				zero = new Vector3(1f / base.transform.lossyScale.x, 1f / base.transform.lossyScale.y);
				position = new Vector3(zero.x * -base.transform.position.x, zero.y * -base.transform.position.y);
			}
			if (this.m_ShapePath == null || this.m_ShapePath.Length == 0)
			{
				this.m_ShapePath = new Vector3[]
				{
					position + new Vector3(zero.x * bounds.min.x, zero.y * bounds.min.y),
					position + new Vector3(zero.x * bounds.min.x, zero.y * bounds.max.y),
					position + new Vector3(zero.x * bounds.max.x, zero.y * bounds.max.y),
					position + new Vector3(zero.x * bounds.max.x, zero.y * bounds.min.y)
				};
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00011D2C File Offset: 0x0000FF2C
		protected void OnEnable()
		{
			if (this.m_Mesh == null || this.m_InstanceId != base.GetInstanceID())
			{
				this.m_Mesh = new Mesh();
				this.m_LocalBounds = ShadowUtility.GenerateShadowMesh(this.m_Mesh, this.m_ShapePath);
				this.m_InstanceId = base.GetInstanceID();
			}
			this.m_ShadowCasterGroup = null;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00011D8A File Offset: 0x0000FF8A
		protected void OnDisable()
		{
			ShadowCasterGroup2DManager.RemoveFromShadowCasterGroup(this, this.m_ShadowCasterGroup);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00011D98 File Offset: 0x0000FF98
		public void Update()
		{
			Renderer renderer;
			this.m_HasRenderer = base.TryGetComponent<Renderer>(out renderer);
			if (LightUtility.CheckForChange(this.m_ShapePathHash, ref this.m_PreviousPathHash))
			{
				this.m_LocalBounds = ShadowUtility.GenerateShadowMesh(this.m_Mesh, this.m_ShapePath);
			}
			this.m_PreviousShadowCasterGroup = this.m_ShadowCasterGroup;
			if (ShadowCasterGroup2DManager.AddToShadowCasterGroup(this, ref this.m_ShadowCasterGroup) && this.m_ShadowCasterGroup != null)
			{
				if (this.m_PreviousShadowCasterGroup == this)
				{
					ShadowCasterGroup2DManager.RemoveGroup(this);
				}
				ShadowCasterGroup2DManager.RemoveFromShadowCasterGroup(this, this.m_PreviousShadowCasterGroup);
				if (this.m_ShadowCasterGroup == this)
				{
					ShadowCasterGroup2DManager.AddGroup(this);
				}
			}
			if (LightUtility.CheckForChange(this.m_ShadowGroup, ref this.m_PreviousShadowGroup))
			{
				ShadowCasterGroup2DManager.RemoveGroup(this);
				ShadowCasterGroup2DManager.AddGroup(this);
			}
			if (LightUtility.CheckForChange(this.m_CastsShadows, ref this.m_PreviousCastsShadows))
			{
				if (this.m_CastsShadows)
				{
					ShadowCasterGroup2DManager.AddGroup(this);
				}
				else
				{
					ShadowCasterGroup2DManager.RemoveGroup(this);
				}
			}
			this.UpdateBoundingSphere();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00011E87 File Offset: 0x00010087
		public void OnBeforeSerialize()
		{
			this.m_ComponentVersion = ShadowCaster2D.ComponentVersions.Version_1;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00011E90 File Offset: 0x00010090
		public void OnAfterDeserialize()
		{
			if (this.m_ComponentVersion == ShadowCaster2D.ComponentVersions.Version_Unserialized)
			{
				this.m_LocalBounds = ShadowUtility.CalculateLocalBounds(this.m_ShapePath);
				this.m_ComponentVersion = ShadowCaster2D.ComponentVersions.Version_1;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00011EB4 File Offset: 0x000100B4
		private void UpdateBoundingSphere()
		{
			Vector3 vector = base.transform.TransformPoint(this.m_LocalBounds.max);
			Vector3 vector2 = base.transform.TransformPoint(this.m_LocalBounds.min);
			Vector3 vector3 = 0.5f * (vector + vector2);
			float num = Vector3.Magnitude(vector - vector3);
			this.m_BoundingSphere = new BoundingSphere(vector3, num);
		}

		// Token: 0x0400017A RID: 378
		private const ShadowCaster2D.ComponentVersions k_CurrentComponentVersion = ShadowCaster2D.ComponentVersions.Version_1;

		// Token: 0x0400017B RID: 379
		[SerializeField]
		private ShadowCaster2D.ComponentVersions m_ComponentVersion;

		// Token: 0x0400017C RID: 380
		[SerializeField]
		private bool m_HasRenderer;

		// Token: 0x0400017D RID: 381
		[SerializeField]
		private bool m_UseRendererSilhouette = true;

		// Token: 0x0400017E RID: 382
		[SerializeField]
		private bool m_CastsShadows = true;

		// Token: 0x0400017F RID: 383
		[SerializeField]
		private bool m_SelfShadows;

		// Token: 0x04000180 RID: 384
		[SerializeField]
		private int[] m_ApplyToSortingLayers;

		// Token: 0x04000181 RID: 385
		[SerializeField]
		private Vector3[] m_ShapePath;

		// Token: 0x04000182 RID: 386
		[SerializeField]
		private int m_ShapePathHash;

		// Token: 0x04000183 RID: 387
		[SerializeField]
		private Mesh m_Mesh;

		// Token: 0x04000184 RID: 388
		[SerializeField]
		private int m_InstanceId;

		// Token: 0x04000185 RID: 389
		internal ShadowCasterGroup2D m_ShadowCasterGroup;

		// Token: 0x04000186 RID: 390
		internal ShadowCasterGroup2D m_PreviousShadowCasterGroup;

		// Token: 0x04000187 RID: 391
		[SerializeField]
		internal Bounds m_LocalBounds;

		// Token: 0x04000188 RID: 392
		internal BoundingSphere m_BoundingSphere;

		// Token: 0x04000189 RID: 393
		private int m_PreviousShadowGroup;

		// Token: 0x0400018A RID: 394
		private bool m_PreviousCastsShadows = true;

		// Token: 0x0400018B RID: 395
		private int m_PreviousPathHash;

		// Token: 0x0400018C RID: 396
		internal Vector3 m_CachedPosition;

		// Token: 0x0400018D RID: 397
		internal Vector3 m_CachedLossyScale;

		// Token: 0x0400018E RID: 398
		internal Quaternion m_CachedRotation;

		// Token: 0x0400018F RID: 399
		internal Matrix4x4 m_CachedShadowMatrix;

		// Token: 0x04000190 RID: 400
		internal Matrix4x4 m_CachedInverseShadowMatrix;

		// Token: 0x04000191 RID: 401
		internal Matrix4x4 m_CachedLocalToWorldMatrix;

		// Token: 0x02000162 RID: 354
		public enum ComponentVersions
		{
			// Token: 0x040009A8 RID: 2472
			Version_Unserialized,
			// Token: 0x040009A9 RID: 2473
			Version_1
		}
	}
}
