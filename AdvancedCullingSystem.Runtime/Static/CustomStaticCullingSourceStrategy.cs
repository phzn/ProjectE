using System;
using System.Collections.Generic;
using NGS.AdvancedCullingSystem.Utils;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000026 RID: 38
	public class CustomStaticCullingSourceStrategy : IStaticCullingSourceStrategy
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000057FC File Offset: 0x000039FC
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00005804 File Offset: 0x00003A04
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

		// Token: 0x060000DE RID: 222 RVA: 0x00005810 File Offset: 0x00003A10
		public CustomStaticCullingSourceStrategy(GameObject context)
		{
			this._context = context;
			CustomCullingTarget component = context.GetComponent<CustomCullingTarget>();
			if (component != null)
			{
				this._localBounds = new Bounds(component.Bounds.center - component.transform.position, component.Bounds.size);
				this._onVisible = component.OnVisible;
				this._onInvisible = component.OnInvisible;
				this._isOccluder = component.IsOccluder;
			}
			else
			{
				this._localBounds = new Bounds(Vector3.zero, Vector3.one * 3f);
				this._isOccluder = false;
			}
			this._onVisible = new CustomTargetEvent();
			this._onInvisible = new CustomTargetEvent();
			this._colliders = new List<Collider>();
			this._createdColliders = new List<Collider>();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000058E9 File Offset: 0x00003AE9
		public bool TryGetBounds(out Bounds bounds)
		{
			bounds = this._localBounds;
			bounds.center += this._context.transform.position;
			return true;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000591C File Offset: 0x00003B1C
		public bool Validate(out string errorMessage)
		{
			if (this._onVisible == null && this._onInvisible == null)
			{
				errorMessage = "Visible and Invisible actions not assigned";
				return false;
			}
			if (this._isOccluder && (this._colliders == null || this._colliders.Count == 0))
			{
				errorMessage = "Source marked as occluder but colliders not assigned";
				return false;
			}
			errorMessage = "";
			return true;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005970 File Offset: 0x00003B70
		public CullingTarget CreateCullingTarget()
		{
			CustomCullingTarget customCullingTarget = this._context.AddComponent<CustomCullingTarget>();
			Bounds bounds;
			this.TryGetBounds(out bounds);
			customCullingTarget.Bounds = bounds;
			customCullingTarget.SetActions(this._onVisible, this._onInvisible);
			customCullingTarget.IsOccluder = this._isOccluder;
			return customCullingTarget;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000059B8 File Offset: 0x00003BB8
		public void PrepareForBaking()
		{
			if (!this._isOccluder)
			{
				return;
			}
			if (this._colliders == null || this._colliders.Count == 0)
			{
				return;
			}
			this._createdColliders.Clear();
			for (int i = 0; i < this._colliders.Count; i++)
			{
				this._createdColliders.Add(this.CreateCollider(this._colliders[i]));
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005A24 File Offset: 0x00003C24
		public void ClearAfterBaking()
		{
			if (this._createdColliders == null || this._createdColliders.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this._createdColliders.Count; i++)
			{
				Object.DestroyImmediate(this._createdColliders[i].gameObject);
			}
			this._createdColliders.Clear();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005A80 File Offset: 0x00003C80
		public Collider CreateCollider(Collider source)
		{
			Collider collider = ColliderUtils.Duplicate(source);
			GameObject gameObject = collider.gameObject;
			gameObject.transform.localPosition = source.transform.position;
			gameObject.transform.localEulerAngles = source.transform.eulerAngles;
			gameObject.transform.localScale = source.transform.lossyScale;
			gameObject.layer = StaticCullingPreferences.Layer;
			gameObject.transform.parent = this._context.transform;
			return collider;
		}

		// Token: 0x040000B5 RID: 181
		[SerializeField]
		private GameObject _context;

		// Token: 0x040000B6 RID: 182
		[SerializeField]
		private bool _isOccluder;

		// Token: 0x040000B7 RID: 183
		[SerializeField]
		private Bounds _localBounds;

		// Token: 0x040000B8 RID: 184
		[SerializeField]
		private CustomTargetEvent _onVisible;

		// Token: 0x040000B9 RID: 185
		[SerializeField]
		private CustomTargetEvent _onInvisible;

		// Token: 0x040000BA RID: 186
		[SerializeField]
		private List<Collider> _colliders;

		// Token: 0x040000BB RID: 187
		[SerializeField]
		private List<Collider> _createdColliders;
	}
}
