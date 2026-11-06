using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000029 RID: 41
	public class MeshRendererStaticCullingSourceStrategy : IStaticCullingSourceStrategy
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x000061C0 File Offset: 0x000043C0
		public MeshRendererStaticCullingSourceStrategy(GameObject context)
		{
			this._context = context;
			MeshRendererCullingTarget component = context.GetComponent<MeshRendererCullingTarget>();
			if (component != null)
			{
				this._isOccluder = component.IsOccluder;
				return;
			}
			this._isOccluder = !this.AllMaterialsIsTransarent(context);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006208 File Offset: 0x00004408
		public bool Validate(out string errorMessage)
		{
			this._renderer = this._context.GetComponent<MeshRenderer>();
			if (this._renderer == null)
			{
				errorMessage = "MeshRenderer not found";
				return false;
			}
			this._filter = this._context.GetComponent<MeshFilter>();
			if (this._filter == null)
			{
				errorMessage = "MeshFilter not found";
				return false;
			}
			if (this._filter.sharedMesh == null)
			{
				errorMessage = "Mesh not found";
				return false;
			}
			errorMessage = "";
			return true;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000628C File Offset: 0x0000448C
		public bool TryGetBounds(out Bounds bounds)
		{
			if (this._renderer == null && (this._renderer = this._context.GetComponent<MeshRenderer>()) == null)
			{
				bounds = default(Bounds);
				return false;
			}
			bounds = this._renderer.bounds;
			return true;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000062DE File Offset: 0x000044DE
		public CullingTarget CreateCullingTarget()
		{
			MeshRendererCullingTarget meshRendererCullingTarget = this._context.AddComponent<MeshRendererCullingTarget>();
			meshRendererCullingTarget.Bounds = this._renderer.bounds;
			meshRendererCullingTarget.CullingMethod = this._cullingMethod;
			meshRendererCullingTarget.IsOccluder = this._isOccluder;
			return meshRendererCullingTarget;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006314 File Offset: 0x00004514
		public void PrepareForBaking()
		{
			if (!this._isOccluder)
			{
				return;
			}
			Mesh sharedMesh = this._filter.sharedMesh;
			this._collider = new GameObject("SC_Collider")
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
			this._collider.sharedMesh = sharedMesh;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000063AA File Offset: 0x000045AA
		public void ClearAfterBaking()
		{
			if (this._collider != null)
			{
				Object.DestroyImmediate(this._collider.gameObject);
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000063CC File Offset: 0x000045CC
		private bool AllMaterialsIsTransarent(GameObject context)
		{
			MeshRenderer component = context.GetComponent<MeshRenderer>();
			if (component == null)
			{
				return false;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			if (sharedMaterials == null || sharedMaterials.Length == 0)
			{
				return false;
			}
			bool flag = true;
			foreach (Material material in sharedMaterials)
			{
				if (!(material == null) && material.renderQueue != 3000)
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		private GameObject _context;

		// Token: 0x040000C7 RID: 199
		[SerializeField]
		private MeshRenderer _renderer;

		// Token: 0x040000C8 RID: 200
		[SerializeField]
		private MeshFilter _filter;

		// Token: 0x040000C9 RID: 201
		[SerializeField]
		private MeshCollider _collider;

		// Token: 0x040000CA RID: 202
		[SerializeField]
		private CullingMethod _cullingMethod;

		// Token: 0x040000CB RID: 203
		[SerializeField]
		private bool _isOccluder;
	}
}
