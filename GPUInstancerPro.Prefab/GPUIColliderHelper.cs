using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000004 RID: 4
	[RequireComponent(typeof(Collider))]
	public abstract class GPUIColliderHelper<T> : MonoBehaviour where T : GPUIPrefabExtension
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		protected virtual void OnEnable()
		{
			this._collider = base.GetComponent<Collider>();
			this._enteredInstances = new List<T>();
			this._cachedTransform = base.transform;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020E8 File Offset: 0x000002E8
		private void Update()
		{
			if (this._collider == null)
			{
				return;
			}
			if (this._followTransform != null)
			{
				this._cachedTransform.position = this._followTransform.position;
			}
			for (int i = 0; i < this._enteredInstances.Count; i++)
			{
				T t = this._enteredInstances[i];
				if (t == null)
				{
					this._enteredInstances.RemoveAt(i);
					i--;
				}
				else if (!this.IsInsideCollider(t) && this.OnExitedCollider(t))
				{
					this._enteredInstances.RemoveAt(i);
					i--;
				}
				else
				{
					this.OnUpdate(t);
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002198 File Offset: 0x00000398
		private void OnTriggerEnter(Collider collider)
		{
			T t;
			if (collider.gameObject.TryGetComponent<T>(out t))
			{
				this._enteredInstances.Add(t);
				this.OnEnteredCollider(t);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021C8 File Offset: 0x000003C8
		protected bool IsInsideCollider(T instance)
		{
			return !(this._collider == null) && this._collider.bounds.Contains(instance.CachedTransform.position);
		}

		// Token: 0x06000007 RID: 7
		protected abstract void OnEnteredCollider(T instance);

		// Token: 0x06000008 RID: 8
		protected abstract bool OnExitedCollider(T instance);

		// Token: 0x06000009 RID: 9
		protected abstract void OnUpdate(T instance);

		// Token: 0x04000006 RID: 6
		[SerializeField]
		protected Transform _followTransform;

		// Token: 0x04000007 RID: 7
		protected Collider _collider;

		// Token: 0x04000008 RID: 8
		protected List<T> _enteredInstances;

		// Token: 0x04000009 RID: 9
		protected Transform _cachedTransform;
	}
}
