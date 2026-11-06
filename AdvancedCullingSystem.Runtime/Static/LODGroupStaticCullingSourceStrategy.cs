using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000028 RID: 40
	public class LODGroupStaticCullingSourceStrategy : IStaticCullingSourceStrategy
	{
		// Token: 0x060000ED RID: 237 RVA: 0x00005C7C File Offset: 0x00003E7C
		public LODGroupStaticCullingSourceStrategy(GameObject context)
		{
			this._context = context;
			LODGroupCullingTarget component = context.GetComponent<LODGroupCullingTarget>();
			if (component != null)
			{
				this._isOccluder = component.IsOccluder;
				return;
			}
			this._isOccluder = true;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005CBC File Offset: 0x00003EBC
		public bool Validate(out string errorMessage)
		{
			this._lodGroup = this._context.GetComponent<LODGroup>();
			if (this._lodGroup == null)
			{
				errorMessage = "LODGroup not found";
				return false;
			}
			if (this._isOccluder)
			{
				LOD lod = this._lodGroup.GetLODs()[0];
				bool flag = false;
				for (int i = 0; i < lod.renderers.Length; i++)
				{
					if (this.CheckRenderer(lod.renderers[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					errorMessage = "Not found valid Renderers on LOD0 for creating colliders";
					return false;
				}
			}
			this.CollectRenderers();
			if (this._renderers.Count == 0)
			{
				errorMessage = "Not found valid Renderers";
				return false;
			}
			errorMessage = "";
			return true;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005D64 File Offset: 0x00003F64
		public bool TryGetBounds(out Bounds bounds)
		{
			if (this._localBounds != null)
			{
				bounds = this._localBounds.Value;
				bounds.center += this._context.transform.position;
				return true;
			}
			if (this._renderers == null || this._renderers.Count == 0)
			{
				bounds = default(Bounds);
				return false;
			}
			Vector3 vector = Vector3.one * float.MaxValue;
			Vector3 vector2 = -Vector3.one * float.MaxValue;
			for (int i = 0; i < this._renderers.Count; i++)
			{
				Bounds bounds2 = this._renderers[i].bounds;
				Vector3 min = bounds2.min;
				Vector3 max = bounds2.max;
				vector.x = Mathf.Min(vector.x, min.x);
				vector.y = Mathf.Min(vector.y, min.y);
				vector.z = Mathf.Min(vector.z, min.z);
				vector2.x = Mathf.Max(vector2.x, max.x);
				vector2.y = Mathf.Max(vector2.y, max.y);
				vector2.z = Mathf.Max(vector2.z, max.z);
			}
			this._localBounds = new Bounds?(new Bounds(vector + (vector2 - vector) / 2f - this._context.transform.position, vector2 - vector));
			bounds = this._localBounds.Value;
			bounds.center += this._context.transform.position;
			return true;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005F44 File Offset: 0x00004144
		public CullingTarget CreateCullingTarget()
		{
			LODGroupCullingTarget lodgroupCullingTarget = this._context.AddComponent<LODGroupCullingTarget>();
			this.CollectRenderers();
			this._localBounds = null;
			Bounds bounds;
			this.TryGetBounds(out bounds);
			lodgroupCullingTarget.Bounds = bounds;
			lodgroupCullingTarget.SetRenderers(this._renderers);
			lodgroupCullingTarget.CullingMethod = this._cullingMethod;
			lodgroupCullingTarget.IsOccluder = this._isOccluder;
			return lodgroupCullingTarget;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005FA4 File Offset: 0x000041A4
		public void PrepareForBaking()
		{
			if (!this._isOccluder)
			{
				return;
			}
			LOD lod = this._lodGroup.GetLODs()[0];
			for (int i = 0; i < lod.renderers.Length; i++)
			{
				Renderer renderer = lod.renderers[i];
				if (this.CheckRenderer(renderer))
				{
					Collider collider = this.CreateCollider(renderer.GetComponent<MeshFilter>());
					if (this._colliders == null)
					{
						this._colliders = new List<Collider>();
					}
					this._colliders.Add(collider);
				}
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006020 File Offset: 0x00004220
		public void ClearAfterBaking()
		{
			if (this._colliders == null || this._colliders.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this._colliders.Count; i++)
			{
				Object.DestroyImmediate(this._colliders[i].gameObject);
			}
			this._colliders.Clear();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000607C File Offset: 0x0000427C
		private void CollectRenderers()
		{
			if (this._renderers == null)
			{
				this._renderers = new List<Renderer>();
			}
			else
			{
				this._renderers.Clear();
			}
			LOD[] lods = this._lodGroup.GetLODs();
			for (int i = 0; i < this._lodGroup.lodCount; i++)
			{
				LOD lod = lods[i];
				for (int j = 0; j < lod.renderers.Length; j++)
				{
					Renderer renderer = lod.renderers[j];
					if (this.CheckRenderer(renderer))
					{
						this._renderers.Add(renderer);
					}
				}
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00006108 File Offset: 0x00004308
		private bool CheckRenderer(Renderer renderer)
		{
			if (renderer == null)
			{
				return false;
			}
			MeshFilter component = renderer.GetComponent<MeshFilter>();
			return !(component == null) && !(component.sharedMesh == null);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006144 File Offset: 0x00004344
		public Collider CreateCollider(MeshFilter filter)
		{
			Mesh sharedMesh = filter.sharedMesh;
			MeshCollider meshCollider = new GameObject("SC_Collider")
			{
				layer = StaticCullingPreferences.Layer,
				transform = 
				{
					parent = this._context.transform,
					localPosition = Vector3.zero,
					localEulerAngles = Vector3.zero,
					localScale = Vector3.one
				}
			}.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = sharedMesh;
			return meshCollider;
		}

		// Token: 0x040000BF RID: 191
		[SerializeField]
		private GameObject _context;

		// Token: 0x040000C0 RID: 192
		[SerializeField]
		private CullingMethod _cullingMethod;

		// Token: 0x040000C1 RID: 193
		[SerializeField]
		private bool _isOccluder;

		// Token: 0x040000C2 RID: 194
		[SerializeField]
		private LODGroup _lodGroup;

		// Token: 0x040000C3 RID: 195
		[SerializeField]
		private List<Renderer> _renderers;

		// Token: 0x040000C4 RID: 196
		[SerializeField]
		private Bounds? _localBounds;

		// Token: 0x040000C5 RID: 197
		[SerializeField]
		private List<Collider> _colliders;
	}
}
