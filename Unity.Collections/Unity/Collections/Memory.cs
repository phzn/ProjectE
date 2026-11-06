using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Collections
{
	// Token: 0x02000083 RID: 131
	[GenerateTestsForBurstCompatibility]
	internal struct Memory
	{
		// Token: 0x060006DF RID: 1759 RVA: 0x00016CD4 File Offset: 0x00014ED4
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal static void CheckByteCountIsReasonable(long size)
		{
			if (size < 0L)
			{
				throw new InvalidOperationException(string.Format("Attempted to operate on {0} bytes of memory: negative size", size));
			}
			if (size > 1099511627776L)
			{
				throw new InvalidOperationException(string.Format("Attempted to operate on {0} bytes of memory: size too big", size));
			}
		}

		// Token: 0x040003AD RID: 941
		internal const long k_MaximumRamSizeInBytes = 1099511627776L;

		// Token: 0x02000084 RID: 132
		[GenerateTestsForBurstCompatibility]
		internal struct Unmanaged
		{
			// Token: 0x060006E0 RID: 1760 RVA: 0x00016D13 File Offset: 0x00014F13
			internal unsafe static void* Allocate(long size, int align, AllocatorManager.AllocatorHandle allocator)
			{
				return Memory.Unmanaged.Array.Resize(null, 0L, 1L, allocator, size, align);
			}

			// Token: 0x060006E1 RID: 1761 RVA: 0x00016D23 File Offset: 0x00014F23
			internal unsafe static void Free(void* pointer, AllocatorManager.AllocatorHandle allocator)
			{
				if (pointer == null)
				{
					return;
				}
				Memory.Unmanaged.Array.Resize(pointer, 1L, 0L, allocator, 1L, 1);
			}

			// Token: 0x060006E2 RID: 1762 RVA: 0x00016D3A File Offset: 0x00014F3A
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			internal unsafe static T* Allocate<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
			{
				return Memory.Unmanaged.Array.Resize<T>(null, 0L, 1L, allocator);
			}

			// Token: 0x060006E3 RID: 1763 RVA: 0x00016D48 File Offset: 0x00014F48
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			internal unsafe static void Free<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T* pointer, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
			{
				if (pointer == null)
				{
					return;
				}
				Memory.Unmanaged.Array.Resize<T>(pointer, 1L, 0L, allocator);
			}

			// Token: 0x02000085 RID: 133
			[GenerateTestsForBurstCompatibility]
			internal struct Array
			{
				// Token: 0x060006E4 RID: 1764 RVA: 0x00002FD9 File Offset: 0x000011D9
				private static bool IsCustom(AllocatorManager.AllocatorHandle allocator)
				{
					return allocator.Index >= 64;
				}

				// Token: 0x060006E5 RID: 1765 RVA: 0x00016D5C File Offset: 0x00014F5C
				private unsafe static void* CustomResize(void* oldPointer, long oldCount, long newCount, AllocatorManager.AllocatorHandle allocator, long size, int align)
				{
					AllocatorManager.Block block = default(AllocatorManager.Block);
					block.Range.Allocator = allocator;
					block.Range.Items = (int)newCount;
					block.Range.Pointer = (IntPtr)oldPointer;
					block.BytesPerItem = (int)size;
					block.Alignment = align;
					block.AllocatedItems = (int)oldCount;
					AllocatorManager.Try(ref block);
					return (void*)block.Range.Pointer;
				}

				// Token: 0x060006E6 RID: 1766 RVA: 0x00016DD4 File Offset: 0x00014FD4
				internal unsafe static void* Resize(void* oldPointer, long oldCount, long newCount, AllocatorManager.AllocatorHandle allocator, long size, int align)
				{
					int num = math.max(64, align);
					if (Memory.Unmanaged.Array.IsCustom(allocator))
					{
						return Memory.Unmanaged.Array.CustomResize(oldPointer, oldCount, newCount, allocator, size, num);
					}
					void* ptr = default(void*);
					if (newCount > 0L)
					{
						ptr = UnsafeUtility.MallocTracked(newCount * size, num, allocator.ToAllocator, 0);
						if (oldCount > 0L)
						{
							long num2 = math.min(oldCount, newCount) * size;
							UnsafeUtility.MemCpy(ptr, oldPointer, num2);
						}
					}
					if (oldCount > 0L)
					{
						UnsafeUtility.FreeTracked(oldPointer, allocator.ToAllocator);
					}
					return ptr;
				}

				// Token: 0x060006E7 RID: 1767 RVA: 0x00016E4B File Offset: 0x0001504B
				[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
				internal unsafe static T* Resize<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T* oldPointer, long oldCount, long newCount, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
				{
					return (T*)Memory.Unmanaged.Array.Resize((void*)oldPointer, oldCount, newCount, allocator, (long)UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
				}

				// Token: 0x060006E8 RID: 1768 RVA: 0x00016E61 File Offset: 0x00015061
				[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
				internal unsafe static T* Allocate<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(long count, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
				{
					return Memory.Unmanaged.Array.Resize<T>(null, 0L, count, allocator);
				}

				// Token: 0x060006E9 RID: 1769 RVA: 0x00016E6E File Offset: 0x0001506E
				[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
				internal unsafe static void Free<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T* pointer, long count, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
				{
					if (pointer == null)
					{
						return;
					}
					Memory.Unmanaged.Array.Resize<T>(pointer, count, 0L, allocator);
				}
			}
		}

		// Token: 0x02000086 RID: 134
		[GenerateTestsForBurstCompatibility]
		internal struct Array
		{
			// Token: 0x060006EA RID: 1770 RVA: 0x00016E84 File Offset: 0x00015084
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			internal unsafe static void Set<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T* pointer, long count, T t = default(T)) where T : struct, ValueType
			{
				UnsafeUtility.SizeOf<T>();
				int num = 0;
				while ((long)num < count)
				{
					pointer[(IntPtr)num * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = t;
					num++;
				}
			}

			// Token: 0x060006EB RID: 1771 RVA: 0x00016EB8 File Offset: 0x000150B8
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			internal unsafe static void Clear<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T* pointer, long count) where T : struct, ValueType
			{
				long num = count * (long)UnsafeUtility.SizeOf<T>();
				UnsafeUtility.MemClear((void*)pointer, num);
			}

			// Token: 0x060006EC RID: 1772 RVA: 0x00016ED8 File Offset: 0x000150D8
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			internal unsafe static void Copy<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T* dest, T* src, long count) where T : struct, ValueType
			{
				long num = count * (long)UnsafeUtility.SizeOf<T>();
				UnsafeUtility.MemCpy((void*)dest, (void*)src, num);
			}
		}
	}
}
