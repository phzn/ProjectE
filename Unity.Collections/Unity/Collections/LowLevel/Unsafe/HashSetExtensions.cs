using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000135 RID: 309
	public static class HashSetExtensions
	{
		// Token: 0x06000CC4 RID: 3268 RVA: 0x00026090 File Offset: 0x00024290
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x000260E0 File Offset: 0x000242E0
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00026164 File Offset: 0x00024364
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x000261B4 File Offset: 0x000243B4
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00026204 File Offset: 0x00024404
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00026288 File Offset: 0x00024488
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x000262D8 File Offset: 0x000244D8
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00026328 File Offset: 0x00024528
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x000263AC File Offset: 0x000245AC
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x000263FC File Offset: 0x000245FC
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0002644C File Offset: 0x0002464C
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x000264D0 File Offset: 0x000246D0
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00026520 File Offset: 0x00024720
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00026570 File Offset: 0x00024770
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x000265F4 File Offset: 0x000247F4
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00026644 File Offset: 0x00024844
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00026694 File Offset: 0x00024894
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00026718 File Offset: 0x00024918
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00026768 File Offset: 0x00024968
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x000267B8 File Offset: 0x000249B8
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0002683C File Offset: 0x00024A3C
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0002688C File Offset: 0x00024A8C
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x000268DC File Offset: 0x00024ADC
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x00026960 File Offset: 0x00024B60
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x000269B0 File Offset: 0x00024BB0
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00026A00 File Offset: 0x00024C00
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00026A84 File Offset: 0x00024C84
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00026AD4 File Offset: 0x00024CD4
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00026B24 File Offset: 0x00024D24
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00026BA8 File Offset: 0x00024DA8
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00026BF8 File Offset: 0x00024DF8
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00026C48 File Offset: 0x00024E48
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00026CCC File Offset: 0x00024ECC
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00026D1C File Offset: 0x00024F1C
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00026D6C File Offset: 0x00024F6C
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00026DF0 File Offset: 0x00024FF0
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00026E40 File Offset: 0x00025040
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00026E90 File Offset: 0x00025090
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00026F14 File Offset: 0x00025114
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00026F64 File Offset: 0x00025164
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00026FB4 File Offset: 0x000251B4
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00027038 File Offset: 0x00025238
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00027088 File Offset: 0x00025288
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x000270D8 File Offset: 0x000252D8
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0002715C File Offset: 0x0002535C
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x000271AC File Offset: 0x000253AC
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x000271FC File Offset: 0x000253FC
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00027280 File Offset: 0x00025480
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x000272D0 File Offset: 0x000254D0
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00027320 File Offset: 0x00025520
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x000273A4 File Offset: 0x000255A4
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x000273F4 File Offset: 0x000255F4
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00027444 File Offset: 0x00025644
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000274C8 File Offset: 0x000256C8
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00027518 File Offset: 0x00025718
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00027568 File Offset: 0x00025768
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x000275EC File Offset: 0x000257EC
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0002763C File Offset: 0x0002583C
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0002768C File Offset: 0x0002588C
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00027710 File Offset: 0x00025910
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00027760 File Offset: 0x00025960
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x000277B0 File Offset: 0x000259B0
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00027834 File Offset: 0x00025A34
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00027884 File Offset: 0x00025A84
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x000278D4 File Offset: 0x00025AD4
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00027958 File Offset: 0x00025B58
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x000279A8 File Offset: 0x00025BA8
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x000279F8 File Offset: 0x00025BF8
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00027A7C File Offset: 0x00025C7C
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00027ACC File Offset: 0x00025CCC
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00027B1C File Offset: 0x00025D1C
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00027BA0 File Offset: 0x00025DA0
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00027BF0 File Offset: 0x00025DF0
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00027C40 File Offset: 0x00025E40
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00027CC4 File Offset: 0x00025EC4
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00027D14 File Offset: 0x00025F14
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00027D64 File Offset: 0x00025F64
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00027DE8 File Offset: 0x00025FE8
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00027E38 File Offset: 0x00026038
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00027E88 File Offset: 0x00026088
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00027F0C File Offset: 0x0002610C
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00027F5C File Offset: 0x0002615C
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00027FAC File Offset: 0x000261AC
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00028030 File Offset: 0x00026230
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00028080 File Offset: 0x00026280
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x000280D0 File Offset: 0x000262D0
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00028154 File Offset: 0x00026354
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x000281A4 File Offset: 0x000263A4
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x000281F4 File Offset: 0x000263F4
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00028278 File Offset: 0x00026478
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x000282C8 File Offset: 0x000264C8
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00028318 File Offset: 0x00026518
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0002839C File Offset: 0x0002659C
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x000283EC File Offset: 0x000265EC
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0002843C File Offset: 0x0002663C
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x000284C0 File Offset: 0x000266C0
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00028510 File Offset: 0x00026710
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00028560 File Offset: 0x00026760
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x000285E4 File Offset: 0x000267E4
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00028634 File Offset: 0x00026834
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00028684 File Offset: 0x00026884
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00028708 File Offset: 0x00026908
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00028758 File Offset: 0x00026958
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x000287A8 File Offset: 0x000269A8
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0002882C File Offset: 0x00026A2C
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0002887C File Offset: 0x00026A7C
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x000288CC File Offset: 0x00026ACC
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00028950 File Offset: 0x00026B50
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x000289A0 File Offset: 0x00026BA0
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x000289F0 File Offset: 0x00026BF0
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00028A74 File Offset: 0x00026C74
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00028AC4 File Offset: 0x00026CC4
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00028B14 File Offset: 0x00026D14
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00028B98 File Offset: 0x00026D98
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00028BE8 File Offset: 0x00026DE8
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00028C38 File Offset: 0x00026E38
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00028CBC File Offset: 0x00026EBC
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00028D0C File Offset: 0x00026F0C
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00028D5C File Offset: 0x00026F5C
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00028DE0 File Offset: 0x00026FE0
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00028E30 File Offset: 0x00027030
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00028E80 File Offset: 0x00027080
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00028F04 File Offset: 0x00027104
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00028F54 File Offset: 0x00027154
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00028FA4 File Offset: 0x000271A4
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			UnsafeList<T> unsafeList = new UnsafeList<T>(container.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			foreach (T t in other)
			{
				if (container.Contains(t))
				{
					unsafeList.Add(in t);
				}
			}
			container.Clear();
			(ref container).UnionWith<T>(unsafeList);
			unsafeList.Dispose();
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00029028 File Offset: 0x00027228
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this UnsafeParallelHashSet<T> container, UnsafeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}
	}
}
