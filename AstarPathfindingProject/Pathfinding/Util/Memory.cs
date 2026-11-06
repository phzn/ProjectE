using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Pooling;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Pathfinding.Util
{
	// Token: 0x0200028C RID: 652
	public static class Memory
	{
		// Token: 0x06001048 RID: 4168 RVA: 0x00062830 File Offset: 0x00060A30
		public static T[] ShrinkArray<T>(T[] arr, int newLength)
		{
			newLength = Math.Min(newLength, arr.Length);
			T[] array = new T[newLength];
			Array.Copy(arr, array, newLength);
			return array;
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00062858 File Offset: 0x00060A58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x00062880 File Offset: 0x00060A80
		public static void Realloc<T>(ref NativeArray<T> arr, int newSize, Allocator allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory) where T : struct
		{
			if (arr.IsCreated && arr.Length >= newSize)
			{
				return;
			}
			NativeArray<T> nativeArray = new NativeArray<T>(newSize, allocator, options);
			if (arr.IsCreated)
			{
				NativeArray<T>.Copy(arr, nativeArray, arr.Length);
				arr.Dispose();
			}
			arr = nativeArray;
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x000628D0 File Offset: 0x00060AD0
		public static void Realloc<T>(ref T[] arr, int newSize)
		{
			if (arr == null)
			{
				arr = new T[newSize];
				return;
			}
			if (newSize > arr.Length)
			{
				T[] array = new T[newSize];
				arr.CopyTo(array, 0);
				arr = array;
			}
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x00062904 File Offset: 0x00060B04
		public unsafe static T[] UnsafeAppendBufferToArray<[IsUnmanaged] T>(UnsafeAppendBuffer src) where T : struct, ValueType
		{
			int num = src.Length / UnsafeUtility.SizeOf<T>();
			T[] array = new T[num];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			UnsafeUtility.MemCpy((void*)gchandle.AddrOfPinnedObject(), (void*)src.Ptr, (long)num * (long)UnsafeUtility.SizeOf<T>());
			gchandle.Free();
			return array;
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00062954 File Offset: 0x00060B54
		public static bool SequenceEqual<T>(T[] a, T[] b) where T : IEquatable<T>
		{
			if (a == null && b == null)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (!a[i].Equals(b[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x000629AC File Offset: 0x00060BAC
		public static void Rotate3DArray<T>(T[] arr, int3 size, int dx, int dz)
		{
			int x = size.x;
			int y = size.y;
			int z = size.z;
			dx %= x;
			dz %= z;
			if (dx != 0)
			{
				if (dx < 0)
				{
					dx = x + dx;
				}
				T[] array = ArrayPool<T>.Claim(dx);
				for (int i = 0; i < y; i++)
				{
					int num = i * x * z;
					for (int j = 0; j < z; j++)
					{
						Array.Copy(arr, num + j * x + x - dx, array, 0, dx);
						Array.Copy(arr, num + j * x, arr, num + j * x + dx, x - dx);
						Array.Copy(array, 0, arr, num + j * x, dx);
					}
				}
				ArrayPool<T>.Release(ref array, false);
			}
			if (dz != 0)
			{
				if (dz < 0)
				{
					dz = z + dz;
				}
				T[] array2 = ArrayPool<T>.Claim(dz * x);
				for (int k = 0; k < y; k++)
				{
					int num2 = k * x * z;
					Array.Copy(arr, num2 + (z - dz) * x, array2, 0, dz * x);
					Array.Copy(arr, num2, arr, num2 + dz * x, (z - dz) * x);
					Array.Copy(array2, 0, arr, num2, dz * x);
				}
				ArrayPool<T>.Release(ref array2, false);
			}
		}
	}
}
