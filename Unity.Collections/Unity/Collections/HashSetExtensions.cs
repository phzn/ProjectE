using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000AC RID: 172
	public static class HashSetExtensions
	{
		// Token: 0x06000842 RID: 2114 RVA: 0x00018D38 File Offset: 0x00016F38
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00018D88 File Offset: 0x00016F88
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000844 RID: 2116 RVA: 0x00018E0C File Offset: 0x0001700C
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00018E5C File Offset: 0x0001705C
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00018EAC File Offset: 0x000170AC
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000847 RID: 2119 RVA: 0x00018F30 File Offset: 0x00017130
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00018F80 File Offset: 0x00017180
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00018FD0 File Offset: 0x000171D0
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x0600084A RID: 2122 RVA: 0x00019054 File Offset: 0x00017254
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x000190A4 File Offset: 0x000172A4
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x000190F4 File Offset: 0x000172F4
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x0600084D RID: 2125 RVA: 0x00019178 File Offset: 0x00017378
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000191C8 File Offset: 0x000173C8
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00019218 File Offset: 0x00017418
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000850 RID: 2128 RVA: 0x0001929C File Offset: 0x0001749C
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x000192EC File Offset: 0x000174EC
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001933C File Offset: 0x0001753C
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000853 RID: 2131 RVA: 0x000193C0 File Offset: 0x000175C0
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00019410 File Offset: 0x00017610
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00019460 File Offset: 0x00017660
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000856 RID: 2134 RVA: 0x000194E4 File Offset: 0x000176E4
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00019534 File Offset: 0x00017734
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00019584 File Offset: 0x00017784
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000859 RID: 2137 RVA: 0x00019608 File Offset: 0x00017808
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00019658 File Offset: 0x00017858
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x000196A8 File Offset: 0x000178A8
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x0600085C RID: 2140 RVA: 0x0001972C File Offset: 0x0001792C
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001977C File Offset: 0x0001797C
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000197CC File Offset: 0x000179CC
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x0600085F RID: 2143 RVA: 0x00019850 File Offset: 0x00017A50
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x000198A0 File Offset: 0x00017AA0
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x000198F0 File Offset: 0x00017AF0
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000862 RID: 2146 RVA: 0x00019974 File Offset: 0x00017B74
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x000199C4 File Offset: 0x00017BC4
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00019A14 File Offset: 0x00017C14
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000865 RID: 2149 RVA: 0x00019A98 File Offset: 0x00017C98
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList128Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00019AE8 File Offset: 0x00017CE8
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00019B38 File Offset: 0x00017D38
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000868 RID: 2152 RVA: 0x00019BBC File Offset: 0x00017DBC
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList32Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00019C0C File Offset: 0x00017E0C
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00019C5C File Offset: 0x00017E5C
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x0600086B RID: 2155 RVA: 0x00019CE0 File Offset: 0x00017EE0
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList4096Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00019D30 File Offset: 0x00017F30
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00019D80 File Offset: 0x00017F80
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x0600086E RID: 2158 RVA: 0x00019E04 File Offset: 0x00018004
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList512Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00019E54 File Offset: 0x00018054
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00019EA4 File Offset: 0x000180A4
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000871 RID: 2161 RVA: 0x00019F28 File Offset: 0x00018128
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, FixedList64Bytes<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00019F78 File Offset: 0x00018178
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00019FC8 File Offset: 0x000181C8
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000874 RID: 2164 RVA: 0x0001A04C File Offset: 0x0001824C
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeArray<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0001A09C File Offset: 0x0001829C
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0001A0EC File Offset: 0x000182EC
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000877 RID: 2167 RVA: 0x0001A170 File Offset: 0x00018370
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0001A1C0 File Offset: 0x000183C0
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0001A210 File Offset: 0x00018410
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x0600087A RID: 2170 RVA: 0x0001A294 File Offset: 0x00018494
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0001A2E4 File Offset: 0x000184E4
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0001A334 File Offset: 0x00018534
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x0600087D RID: 2173 RVA: 0x0001A3B8 File Offset: 0x000185B8
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeParallelHashSet<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0001A408 File Offset: 0x00018608
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0001A458 File Offset: 0x00018658
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000880 RID: 2176 RVA: 0x0001A4DC File Offset: 0x000186DC
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeParallelHashSet<T>.ReadOnly other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001A52C File Offset: 0x0001872C
		public static void ExceptWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Remove(t);
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001A57C File Offset: 0x0001877C
		public static void IntersectWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
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

		// Token: 0x06000883 RID: 2179 RVA: 0x0001A600 File Offset: 0x00018800
		public static void UnionWith<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(this NativeParallelHashSet<T> container, NativeList<T> other) where T : struct, ValueType, IEquatable<T>
		{
			foreach (T t in other)
			{
				container.Add(t);
			}
		}
	}
}
