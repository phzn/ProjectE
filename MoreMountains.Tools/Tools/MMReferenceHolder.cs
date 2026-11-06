using System;
using System.Collections.Generic;

namespace MoreMountains.Tools
{
	// Token: 0x02000153 RID: 339
	public struct MMReferenceHolder<T> : IDisposable where T : class
	{
		// Token: 0x06000783 RID: 1923 RVA: 0x000240DC File Offset: 0x000222DC
		public void Reference(T instance, bool cleanUp = false)
		{
			MMReferenceHolder<T>._instances = MMReferenceHolder<T>._instances ?? new List<WeakReference<T>>(1);
			if (cleanUp)
			{
				MMReferenceHolder<T>.CleanUp();
			}
			if (instance != null)
			{
				this._instance = new WeakReference<T>(instance);
				MMReferenceHolder<T>._instances.Add(this._instance);
			}
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00024129 File Offset: 0x00022329
		public void Dispose()
		{
			if (this._instance != null)
			{
				List<WeakReference<T>> instances = MMReferenceHolder<T>._instances;
				if (instances == null)
				{
					return;
				}
				instances.Remove(this._instance);
			}
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00024149 File Offset: 0x00022349
		public static void CleanUp()
		{
			MMReferenceHolder<T>.RepackNonNullReferences();
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00024150 File Offset: 0x00022350
		private static void RepackNonNullReferences()
		{
			if (MMReferenceHolder<T>._instances == null)
			{
				return;
			}
			for (int i = MMReferenceHolder<T>._instances.Count - 1; i >= 0; i--)
			{
				T t;
				if (!MMReferenceHolder<T>._instances[i].TryGetTarget(out t))
				{
					MMReferenceHolder<T>._instances.RemoveAt(i);
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0002419C File Offset: 0x0002239C
		public static T Any
		{
			get
			{
				T t;
				if (MMReferenceHolder<T>._instances == null || MMReferenceHolder<T>._instances.Count <= 0 || !MMReferenceHolder<T>._instances[0].TryGetTarget(out t))
				{
					return default(T);
				}
				return t;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x000241DC File Offset: 0x000223DC
		public static IEnumerator<T> All
		{
			get
			{
				if (MMReferenceHolder<T>._instances == null)
				{
					yield break;
				}
				using (List<WeakReference<T>>.Enumerator enumerator = MMReferenceHolder<T>._instances.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						T t;
						if (enumerator.Current.TryGetTarget(out t))
						{
							yield return t;
						}
					}
				}
				List<WeakReference<T>>.Enumerator enumerator = default(List<WeakReference<T>>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x000241E4 File Offset: 0x000223E4
		public static T First(Func<T, bool> selector)
		{
			if (MMReferenceHolder<T>._instances == null)
			{
				return default(T);
			}
			if (selector == null)
			{
				return MMReferenceHolder<T>.Any;
			}
			using (List<WeakReference<T>>.Enumerator enumerator = MMReferenceHolder<T>._instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t;
					if (enumerator.Current.TryGetTarget(out t) && selector(t))
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x04000929 RID: 2345
		private static List<WeakReference<T>> _instances = new List<WeakReference<T>>(2);

		// Token: 0x0400092A RID: 2346
		private WeakReference<T> _instance;
	}
}
