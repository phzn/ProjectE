using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x0200004F RID: 79
	[Serializable]
	public class DC_RendererSourceSettingsStrategy : IDC_SourceSettingsStrategy
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00008F7F File Offset: 0x0000717F
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00008F87 File Offset: 0x00007187
		public bool ReadyForCulling { get; private set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00008F90 File Offset: 0x00007190
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00008F98 File Offset: 0x00007198
		public CullingMethod CullingMethod
		{
			get
			{
				return this._cullingMethod;
			}
			set
			{
				this._cullingMethod = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00008FA1 File Offset: 0x000071A1
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00008FA9 File Offset: 0x000071A9
		public bool ConvexCollider
		{
			get
			{
				return this._convexCollider;
			}
			set
			{
				this._convexCollider = value;
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008FB2 File Offset: 0x000071B2
		public DC_RendererSourceSettingsStrategy(DC_SourceSettings context)
		{
			this._context = context;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008FC4 File Offset: 0x000071C4
		public bool CheckCompatibilityAndGetComponents(out string incompatibilityReason)
		{
			if (this._renderer == null && !this._context.TryGetComponent<MeshRenderer>(out this._renderer))
			{
				incompatibilityReason = "MeshRenderer not found";
				return false;
			}
			if (this._mesh == null)
			{
				MeshFilter component = this._context.GetComponent<MeshFilter>();
				if (component == null)
				{
					incompatibilityReason = "MeshFilter not found";
					return false;
				}
				this._mesh = component.sharedMesh;
				if (this._mesh == null)
				{
					incompatibilityReason = "Mesh not found";
					return false;
				}
			}
			if (!this._convexCollider && !this._rigibodiesChecked)
			{
				Rigidbody[] componentsInParent = this._context.GetComponentsInParent<Rigidbody>();
				for (int i = 0; i < componentsInParent.Length; i++)
				{
					if (!componentsInParent[i].isKinematic)
					{
						this.ConvexCollider = true;
						break;
					}
				}
				this._rigibodiesChecked = true;
			}
			incompatibilityReason = "";
			return true;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00009098 File Offset: 0x00007298
		public void PrepareForCulling()
		{
			if (this.ReadyForCulling)
			{
				return;
			}
			this._collider = new GameObject("DC_Collider")
			{
				transform = 
				{
					parent = this._renderer.transform
				},
				layer = this._context.CullingLayer,
				transform = 
				{
					localPosition = Vector3.zero,
					localEulerAngles = Vector3.zero,
					localScale = Vector3.one
				}
			}.AddComponent<MeshCollider>();
			this._collider.sharedMesh = this._mesh;
			if (this.ConvexCollider)
			{
				this._collider.convex = true;
			}
			this.ReadyForCulling = true;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00009148 File Offset: 0x00007348
		public void ClearData()
		{
			if (!this.ReadyForCulling)
			{
				return;
			}
			if (this._collider != null)
			{
				Object.DestroyImmediate(this._collider.gameObject);
			}
			this._collider = null;
			this.ReadyForCulling = false;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000917F File Offset: 0x0000737F
		public bool TryGetBounds(ref Bounds bounds)
		{
			if (this._renderer != null)
			{
				bounds = this._renderer.bounds;
				return true;
			}
			return false;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000091A3 File Offset: 0x000073A3
		public ICullingTarget CreateCullingTarget()
		{
			if (this.CullingMethod == CullingMethod.KeepShadows)
			{
				return new DC_RendererShadowsTarget(this._renderer);
			}
			return new DC_RendererTarget(this._renderer);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000091C5 File Offset: 0x000073C5
		public IEnumerable<Collider> GetColliders()
		{
			if (this._collider == null)
			{
				yield break;
			}
			yield return this._collider;
			yield break;
		}

		// Token: 0x04000148 RID: 328
		[SerializeField]
		private DC_SourceSettings _context;

		// Token: 0x04000149 RID: 329
		[SerializeField]
		private CullingMethod _cullingMethod;

		// Token: 0x0400014A RID: 330
		[SerializeField]
		private bool _convexCollider;

		// Token: 0x0400014B RID: 331
		[SerializeField]
		private MeshRenderer _renderer;

		// Token: 0x0400014C RID: 332
		[SerializeField]
		private Mesh _mesh;

		// Token: 0x0400014D RID: 333
		[SerializeField]
		private MeshCollider _collider;

		// Token: 0x0400014E RID: 334
		[SerializeField]
		private bool _rigibodiesChecked;
	}
}
