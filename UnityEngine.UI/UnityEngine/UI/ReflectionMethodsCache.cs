using System;
using System.Reflection;

namespace UnityEngine.UI
{
	// Token: 0x0200003D RID: 61
	internal class ReflectionMethodsCache
	{
		// Token: 0x06000492 RID: 1170 RVA: 0x00015F74 File Offset: 0x00014174
		public ReflectionMethodsCache()
		{
			MethodInfo method = typeof(Physics).GetMethod("Raycast", new Type[]
			{
				typeof(Ray),
				typeof(RaycastHit).MakeByRefType(),
				typeof(float),
				typeof(int)
			});
			if (method != null)
			{
				this.raycast3D = (ReflectionMethodsCache.Raycast3DCallback)Delegate.CreateDelegate(typeof(ReflectionMethodsCache.Raycast3DCallback), method);
			}
			MethodInfo method2 = typeof(Physics).GetMethod("RaycastAll", new Type[]
			{
				typeof(Ray),
				typeof(float),
				typeof(int)
			});
			if (method2 != null)
			{
				this.raycast3DAll = (ReflectionMethodsCache.RaycastAllCallback)Delegate.CreateDelegate(typeof(ReflectionMethodsCache.RaycastAllCallback), method2);
			}
			MethodInfo method3 = typeof(Physics).GetMethod("RaycastNonAlloc", new Type[]
			{
				typeof(Ray),
				typeof(RaycastHit[]),
				typeof(float),
				typeof(int)
			});
			if (method3 != null)
			{
				this.getRaycastNonAlloc = (ReflectionMethodsCache.GetRaycastNonAllocCallback)Delegate.CreateDelegate(typeof(ReflectionMethodsCache.GetRaycastNonAllocCallback), method3);
			}
			MethodInfo method4 = typeof(Physics2D).GetMethod("Raycast", new Type[]
			{
				typeof(Vector2),
				typeof(Vector2),
				typeof(float),
				typeof(int)
			});
			if (method4 != null)
			{
				this.raycast2D = (ReflectionMethodsCache.Raycast2DCallback)Delegate.CreateDelegate(typeof(ReflectionMethodsCache.Raycast2DCallback), method4);
			}
			MethodInfo method5 = typeof(Physics2D).GetMethod("GetRayIntersectionAll", new Type[]
			{
				typeof(Ray),
				typeof(float),
				typeof(int)
			});
			if (method5 != null)
			{
				this.getRayIntersectionAll = (ReflectionMethodsCache.GetRayIntersectionAllCallback)Delegate.CreateDelegate(typeof(ReflectionMethodsCache.GetRayIntersectionAllCallback), method5);
			}
			MethodInfo method6 = typeof(Physics2D).GetMethod("GetRayIntersectionNonAlloc", new Type[]
			{
				typeof(Ray),
				typeof(RaycastHit2D[]),
				typeof(float),
				typeof(int)
			});
			if (method6 != null)
			{
				this.getRayIntersectionAllNonAlloc = (ReflectionMethodsCache.GetRayIntersectionAllNonAllocCallback)Delegate.CreateDelegate(typeof(ReflectionMethodsCache.GetRayIntersectionAllNonAllocCallback), method6);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0001622A File Offset: 0x0001442A
		public static ReflectionMethodsCache Singleton
		{
			get
			{
				if (ReflectionMethodsCache.s_ReflectionMethodsCache == null)
				{
					ReflectionMethodsCache.s_ReflectionMethodsCache = new ReflectionMethodsCache();
				}
				return ReflectionMethodsCache.s_ReflectionMethodsCache;
			}
		}

		// Token: 0x0400017D RID: 381
		public ReflectionMethodsCache.Raycast3DCallback raycast3D;

		// Token: 0x0400017E RID: 382
		public ReflectionMethodsCache.RaycastAllCallback raycast3DAll;

		// Token: 0x0400017F RID: 383
		public ReflectionMethodsCache.GetRaycastNonAllocCallback getRaycastNonAlloc;

		// Token: 0x04000180 RID: 384
		public ReflectionMethodsCache.Raycast2DCallback raycast2D;

		// Token: 0x04000181 RID: 385
		public ReflectionMethodsCache.GetRayIntersectionAllCallback getRayIntersectionAll;

		// Token: 0x04000182 RID: 386
		public ReflectionMethodsCache.GetRayIntersectionAllNonAllocCallback getRayIntersectionAllNonAlloc;

		// Token: 0x04000183 RID: 387
		private static ReflectionMethodsCache s_ReflectionMethodsCache;

		// Token: 0x020000B5 RID: 181
		// (Invoke) Token: 0x06000706 RID: 1798
		public delegate bool Raycast3DCallback(Ray r, out RaycastHit hit, float f, int i);

		// Token: 0x020000B6 RID: 182
		// (Invoke) Token: 0x0600070A RID: 1802
		public delegate RaycastHit[] RaycastAllCallback(Ray r, float f, int i);

		// Token: 0x020000B7 RID: 183
		// (Invoke) Token: 0x0600070E RID: 1806
		public delegate int GetRaycastNonAllocCallback(Ray r, RaycastHit[] results, float f, int i);

		// Token: 0x020000B8 RID: 184
		// (Invoke) Token: 0x06000712 RID: 1810
		public delegate RaycastHit2D Raycast2DCallback(Vector2 p1, Vector2 p2, float f, int i);

		// Token: 0x020000B9 RID: 185
		// (Invoke) Token: 0x06000716 RID: 1814
		public delegate RaycastHit2D[] GetRayIntersectionAllCallback(Ray r, float f, int i);

		// Token: 0x020000BA RID: 186
		// (Invoke) Token: 0x0600071A RID: 1818
		public delegate int GetRayIntersectionAllNonAllocCallback(Ray r, RaycastHit2D[] results, float f, int i);
	}
}
