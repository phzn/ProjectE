using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x0200004D RID: 77
	public class DC_LODGroupSourceSettingsStrategy : IDC_SourceSettingsStrategy
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00008ACF File Offset: 0x00006CCF
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00008AD7 File Offset: 0x00006CD7
		public bool ReadyForCulling { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00008AE0 File Offset: 0x00006CE0
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00008AE8 File Offset: 0x00006CE8
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

		// Token: 0x060001DB RID: 475 RVA: 0x00008AF1 File Offset: 0x00006CF1
		public DC_LODGroupSourceSettingsStrategy(DC_SourceSettings context)
		{
			this._context = context;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00008B00 File Offset: 0x00006D00
		public void PrepareForCulling()
		{
			if (this.ReadyForCulling)
			{
				return;
			}
			this._colliders = this._group.GetLODs()[0].renderers.Where<Renderer>((Renderer r) => r != null && this.IsCompatibleRenderer(r)).Select<Renderer, MeshCollider>(delegate(Renderer r)
			{
				MeshCollider meshCollider = new GameObject("DC_Collider")
				{
					transform = 
					{
						parent = r.transform
					},
					layer = this._context.CullingLayer,
					transform = 
					{
						localPosition = Vector3.zero,
						localEulerAngles = Vector3.zero,
						localScale = Vector3.one
					}
				}.AddComponent<MeshCollider>();
				meshCollider.sharedMesh = r.GetComponent<MeshFilter>().sharedMesh;
				meshCollider.convex = this._convexCollider;
				return meshCollider;
			}).ToArray<MeshCollider>();
			this.ReadyForCulling = true;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008B60 File Offset: 0x00006D60
		public void ClearData()
		{
			if (!this.ReadyForCulling)
			{
				return;
			}
			for (int i = 0; i < this._colliders.Length; i++)
			{
				Collider collider = this._colliders[i];
				if (!(collider == null) && !(collider.gameObject == null))
				{
					Object.DestroyImmediate(collider.gameObject);
				}
			}
			this._colliders = null;
			this.ReadyForCulling = false;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00008BC2 File Offset: 0x00006DC2
		public bool TryGetBounds(ref Bounds bounds)
		{
			if (this._renderers != null)
			{
				bounds = this._bounds;
				return true;
			}
			return false;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008BDB File Offset: 0x00006DDB
		public ICullingTarget CreateCullingTarget()
		{
			if (this.CullingMethod == CullingMethod.KeepShadows)
			{
				return new DC_LODGroupShadowsTarget(this._group, this._renderers, this._bounds);
			}
			return new DC_LODGroupTarget(this._group, this._renderers, this._bounds);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008C15 File Offset: 0x00006E15
		public IEnumerable<Collider> GetColliders()
		{
			if (this._colliders == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this._colliders.Length; i = num + 1)
			{
				yield return this._colliders[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008C28 File Offset: 0x00006E28
		public bool CheckCompatibilityAndGetComponents(out string incompatibilityReason)
		{
			if (this._group == null && !this._context.TryGetComponent<LODGroup>(out this._group))
			{
				incompatibilityReason = "LODGroup not found";
				return false;
			}
			if (this._renderers == null)
			{
				LOD[] lods = this._group.GetLODs();
				int num = lods.Count(new Func<Renderer, bool>(this.IsCompatibleRenderer));
				if (num == 0)
				{
					incompatibilityReason = "Can't find any compatible renderer";
					return false;
				}
				this._renderers = new Renderer[num];
				this._bounds = new Bounds(this._group.transform.position, Vector3.zero);
				int num2 = 0;
				for (int i = 0; i < lods.Length; i++)
				{
					foreach (Renderer renderer in lods[i].renderers)
					{
						if (renderer != null && this.IsCompatibleRenderer(renderer))
						{
							this._renderers[num2++] = renderer;
							this._bounds.Encapsulate(renderer.bounds);
						}
					}
				}
			}
			else
			{
				for (int k = 0; k < this._renderers.Length; k++)
				{
					if (this._renderers[k] == null)
					{
						incompatibilityReason = "Missing renderer at index : " + k.ToString();
						return false;
					}
				}
			}
			if (!this._convexCollider && !this._rigibodiesChecked)
			{
				Rigidbody[] componentsInParent = this._context.GetComponentsInParent<Rigidbody>();
				for (int l = 0; l < componentsInParent.Length; l++)
				{
					if (!componentsInParent[l].isKinematic)
					{
						this._convexCollider = true;
						break;
					}
				}
				this._rigibodiesChecked = true;
			}
			incompatibilityReason = "";
			return true;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00008DC0 File Offset: 0x00006FC0
		private bool IsCompatibleRenderer(Renderer renderer)
		{
			MeshFilter component = renderer.GetComponent<MeshFilter>();
			return component != null && component.sharedMesh != null;
		}

		// Token: 0x0400013A RID: 314
		[SerializeField]
		private DC_SourceSettings _context;

		// Token: 0x0400013B RID: 315
		[SerializeField]
		private CullingMethod _cullingMethod;

		// Token: 0x0400013C RID: 316
		[SerializeField]
		private LODGroup _group;

		// Token: 0x0400013D RID: 317
		[SerializeField]
		private Renderer[] _renderers;

		// Token: 0x0400013E RID: 318
		[SerializeField]
		private MeshCollider[] _colliders;

		// Token: 0x0400013F RID: 319
		[SerializeField]
		private Bounds _bounds;

		// Token: 0x04000140 RID: 320
		[SerializeField]
		private bool _convexCollider;

		// Token: 0x04000141 RID: 321
		[SerializeField]
		private bool _rigibodiesChecked;
	}
}
