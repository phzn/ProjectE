using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000152 RID: 338
	public class MMReferencedScriptableObject<T> : ScriptableObject where T : ScriptableObject
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x00024070 File Offset: 0x00022270
		protected virtual T Typed
		{
			get
			{
				T t;
				if ((t = this._typed) == null)
				{
					t = this as T;
				}
				return this._typed = t;
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x000240A0 File Offset: 0x000222A0
		protected virtual void OnReferenced()
		{
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x000240A2 File Offset: 0x000222A2
		protected virtual void OnEnable()
		{
			this._instances.Reference(this.Typed, false);
			this.OnReferenced();
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x000240BC File Offset: 0x000222BC
		protected virtual void OnDisposed()
		{
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x000240BE File Offset: 0x000222BE
		protected virtual void OnDisable()
		{
			this._instances.Dispose();
			this.OnDisposed();
		}

		// Token: 0x04000927 RID: 2343
		private MMReferenceHolder<T> _instances;

		// Token: 0x04000928 RID: 2344
		private T _typed;
	}
}
