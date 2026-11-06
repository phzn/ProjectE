using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x0200004A RID: 74
	[Serializable]
	public class DC_CustomSourceSettingsStrategy : IDC_SourceSettingsStrategy
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001BA RID: 442 RVA: 0x000080BB File Offset: 0x000062BB
		// (set) Token: 0x060001BB RID: 443 RVA: 0x000080C3 File Offset: 0x000062C3
		public bool ReadyForCulling { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000080CC File Offset: 0x000062CC
		// (set) Token: 0x060001BD RID: 445 RVA: 0x000080D4 File Offset: 0x000062D4
		public Bounds LocalBounds
		{
			get
			{
				return this._localBounds;
			}
			set
			{
				this._localBounds = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001BE RID: 446 RVA: 0x000080DD File Offset: 0x000062DD
		public DC_CustomTargetEvent OnVisibleEvent
		{
			get
			{
				return this._onVisible;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001BF RID: 447 RVA: 0x000080E5 File Offset: 0x000062E5
		public DC_CustomTargetEvent OnInvisibleEvent
		{
			get
			{
				return this._onInvisible;
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000080F0 File Offset: 0x000062F0
		public DC_CustomSourceSettingsStrategy(DC_SourceSettings context)
		{
			this._context = context;
			this._localBounds = new Bounds(Vector3.zero, Vector3.one);
			this._renderers = new List<Renderer>();
			this._useNewBoundsCode = true;
			this._onVisible = new DC_CustomTargetEvent();
			this._onInvisible = new DC_CustomTargetEvent();
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00008147 File Offset: 0x00006347
		public bool CheckCompatibilityAndGetComponents(out string incompatibilityReason)
		{
			incompatibilityReason = "";
			return true;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008154 File Offset: 0x00006354
		public void PrepareForCulling()
		{
			if (this.ReadyForCulling)
			{
				return;
			}
			GameObject gameObject = new GameObject("DC_Collider");
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.parent = this._context.transform;
			gameObject.layer = this._context.CullingLayer;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.eulerAngles = Vector3.zero;
			if (this._alignRotation)
			{
				gameObject.transform.localEulerAngles = Vector3.zero;
			}
			if (this._useNewBoundsCode)
			{
				gameObject.transform.localScale = Vector3.one;
			}
			this._collider = gameObject.AddComponent<BoxCollider>();
			this._collider.center = this._localBounds.center;
			this._collider.size = this._localBounds.size;
			this.ReadyForCulling = true;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000823B File Offset: 0x0000643B
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

		// Token: 0x060001C4 RID: 452 RVA: 0x00008272 File Offset: 0x00006472
		public bool TryGetBounds(ref Bounds bounds)
		{
			bounds.center = this._context.transform.position + this._localBounds.center;
			bounds.size = this._localBounds.size;
			return true;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000082AC File Offset: 0x000064AC
		public ICullingTarget CreateCullingTarget()
		{
			Bounds bounds = default(Bounds);
			this.TryGetBounds(ref bounds);
			this.RegisterRenderersInEvents();
			return new DC_CustomTarget(this._context.gameObject, bounds, this._onVisible, this._onInvisible);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000082ED File Offset: 0x000064ED
		public IEnumerable<Collider> GetColliders()
		{
			if (this._collider == null)
			{
				yield break;
			}
			yield return this._collider;
			yield break;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00008300 File Offset: 0x00006500
		public void AlignLocalBoundsByChildRenderers()
		{
			Renderer[] componentsInChildren = this._context.GetComponentsInChildren<Renderer>();
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				return;
			}
			Bounds bounds = default(Bounds);
			if (this._useNewBoundsCode)
			{
				bool flag = false;
				Renderer[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					Bounds bounds2 = array[i].bounds;
					Vector3 center = bounds2.center;
					Vector3 extents = bounds2.extents;
					foreach (Vector3 vector in new Vector3[]
					{
						new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z),
						new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z),
						new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z),
						new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z),
						new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z),
						new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z),
						new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z),
						new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z)
					})
					{
						Vector3 vector2 = this._context.transform.InverseTransformPoint(vector);
						if (!flag)
						{
							bounds = new Bounds(vector2, Vector3.zero);
							flag = true;
						}
						else
						{
							bounds.Encapsulate(vector2);
						}
					}
				}
			}
			else
			{
				bounds = componentsInChildren[0].bounds;
				for (int k = 1; k < componentsInChildren.Length; k++)
				{
					bounds.Encapsulate(componentsInChildren[k].bounds);
				}
				bounds.center -= this._context.transform.position;
			}
			this._localBounds = bounds;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000085E4 File Offset: 0x000067E4
		public void AlignLocalBoundsByChildColliders()
		{
			Collider[] componentsInChildren = this._context.GetComponentsInChildren<Collider>();
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				return;
			}
			Bounds bounds = default(Bounds);
			if (this._useNewBoundsCode)
			{
				bool flag = false;
				Collider[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					Bounds bounds2 = array[i].bounds;
					Vector3 center = bounds2.center;
					Vector3 extents = bounds2.extents;
					foreach (Vector3 vector in new Vector3[]
					{
						new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z),
						new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z),
						new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z),
						new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z),
						new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z),
						new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z),
						new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z),
						new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z)
					})
					{
						Vector3 vector2 = this._context.transform.InverseTransformPoint(vector);
						if (!flag)
						{
							bounds = new Bounds(vector2, Vector3.zero);
							flag = true;
						}
						else
						{
							bounds.Encapsulate(vector2);
						}
					}
				}
			}
			else
			{
				bounds = componentsInChildren[0].bounds;
				for (int k = 1; k < componentsInChildren.Length; k++)
				{
					bounds.Encapsulate(componentsInChildren[k].bounds);
				}
				bounds.center -= this._context.transform.position;
			}
			this._localBounds = bounds;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000088C8 File Offset: 0x00006AC8
		public void AddChildRenderersToList()
		{
			foreach (Renderer renderer in this._context.GetComponentsInChildren<Renderer>())
			{
				if (!this._renderers.Contains(renderer))
				{
					this._renderers.Add(renderer);
				}
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008910 File Offset: 0x00006B10
		public void RemoveChildRenderersFromList()
		{
			foreach (Renderer renderer in this._context.GetComponentsInChildren<Renderer>())
			{
				this._renderers.Remove(renderer);
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008948 File Offset: 0x00006B48
		private void RegisterRenderersInEvents()
		{
			if (this._renderers == null)
			{
				return;
			}
			using (List<Renderer>.Enumerator enumerator = this._renderers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Renderer renderer = enumerator.Current;
					if (!(renderer == null))
					{
						this._onVisible.AddListener(delegate(DC_CustomTarget t)
						{
							renderer.enabled = true;
						});
						this._onInvisible.AddListener(delegate(DC_CustomTarget t)
						{
							renderer.enabled = false;
						});
					}
				}
			}
		}

		// Token: 0x0400012C RID: 300
		[SerializeField]
		private DC_SourceSettings _context;

		// Token: 0x0400012D RID: 301
		[SerializeField]
		private Bounds _localBounds;

		// Token: 0x0400012E RID: 302
		[SerializeField]
		private bool _alignRotation;

		// Token: 0x0400012F RID: 303
		[SerializeField]
		private bool _useNewBoundsCode;

		// Token: 0x04000130 RID: 304
		[SerializeField]
		private List<Renderer> _renderers;

		// Token: 0x04000131 RID: 305
		[SerializeField]
		private DC_CustomTargetEvent _onVisible;

		// Token: 0x04000132 RID: 306
		[SerializeField]
		private DC_CustomTargetEvent _onInvisible;

		// Token: 0x04000133 RID: 307
		[SerializeField]
		private BoxCollider _collider;
	}
}
