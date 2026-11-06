using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x0200005A RID: 90
	[BurstCompile]
	public struct BinaryHeap
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00010AB3 File Offset: 0x0000ECB3
		public bool isEmpty
		{
			get
			{
				return this.numberOfItems <= 0;
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00010AC1 File Offset: 0x0000ECC1
		private static int RoundUpToNextMultipleMod1(int v)
		{
			return v + (4 - (v - 1) % 4) % 4;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00010ACE File Offset: 0x0000ECCE
		public BinaryHeap(int capacity)
		{
			capacity = BinaryHeap.RoundUpToNextMultipleMod1(capacity);
			this.heap = new UnsafeSpan<BinaryHeap.HeapNode>(Allocator.Persistent, capacity);
			this.numberOfItems = 0;
			this.insertionOrder = 0U;
			this.tieBreaking = BinaryHeap.TieBreaking.HScore;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00010AFA File Offset: 0x0000ECFA
		public void Dispose()
		{
			AllocatorManager.Free<BinaryHeap.HeapNode>(Allocator.Persistent, this.heap.ptr, this.heap.Length);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00010B20 File Offset: 0x0000ED20
		public void Clear(UnsafeSpan<PathNode> pathNodes)
		{
			for (int i = 0; i < this.numberOfItems; i++)
			{
				pathNodes[this.heap[i].pathNodeIndex].heapIndex = ushort.MaxValue;
			}
			this.numberOfItems = 0;
			this.insertionOrder = 0U;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00010B6E File Offset: 0x0000ED6E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetPathNodeIndex(int heapIndex)
		{
			return this.heap[heapIndex].pathNodeIndex;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00010B81 File Offset: 0x0000ED81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetH(int heapIndex)
		{
			if (this.tieBreaking != BinaryHeap.TieBreaking.HScore)
			{
				return 0U;
			}
			return this.heap[heapIndex].TieBreaker;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00010B9E File Offset: 0x0000ED9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetF(int heapIndex)
		{
			return this.heap[heapIndex].F;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00010BB4 File Offset: 0x0000EDB4
		public void SetH(int heapIndex, uint h)
		{
			ref BinaryHeap.HeapNode ptr = ref this.heap[heapIndex];
			uint num = ptr.F - ptr.TieBreaker;
			ptr.TieBreaker = h;
			ptr.F = num + h;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00010BEC File Offset: 0x0000EDEC
		private static void Expand(ref UnsafeSpan<BinaryHeap.HeapNode> heap)
		{
			int num = math.max(heap.Length + 4, math.min(65533, (int)math.round((float)heap.Length * 2f)));
			num = BinaryHeap.RoundUpToNextMultipleMod1(num);
			if (num > 65534)
			{
				throw new Exception("Binary Heap Size really large (>65534). A heap size this large is probably the cause of pathfinding running in an infinite loop. ");
			}
			UnsafeSpan<BinaryHeap.HeapNode> unsafeSpan = new UnsafeSpan<BinaryHeap.HeapNode>(Allocator.Persistent, num);
			unsafeSpan.CopyFrom<BinaryHeap.HeapNode>(heap);
			AllocatorManager.Free<BinaryHeap.HeapNode>(Allocator.Persistent, heap.ptr, heap.Length);
			heap = unsafeSpan;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00010C74 File Offset: 0x0000EE74
		public void Add(UnsafeSpan<PathNode> nodes, uint pathNodeIndex, uint g, uint h)
		{
			uint num = this.insertionOrder;
			this.insertionOrder = num + 1U;
			BinaryHeap.Add(ref this, ref nodes, pathNodeIndex, g, h, num, this.tieBreaking);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00010CA4 File Offset: 0x0000EEA4
		[BurstCompile]
		private static void Add(ref BinaryHeap binaryHeap, ref UnsafeSpan<PathNode> nodes, uint pathNodeIndex, uint g, uint h, uint insertionOrder, BinaryHeap.TieBreaking tieBreaking)
		{
			BinaryHeap.Add_00000373$BurstDirectCall.Invoke(ref binaryHeap, ref nodes, pathNodeIndex, g, h, insertionOrder, tieBreaking);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00010CB8 File Offset: 0x0000EEB8
		private unsafe static void DecreaseKey(UnsafeSpan<BinaryHeap.HeapNode> heap, UnsafeSpan<PathNode> nodes, BinaryHeap.HeapNode node, ushort index)
		{
			uint num;
			uint num2;
			for (num = (uint)index; num != 0U; num = num2)
			{
				num2 = (num - 1U) / 4U;
				Hint.Assume(num2 < heap.length);
				Hint.Assume(num < heap.length);
				if (node.sortKey >= heap[num2].sortKey)
				{
					break;
				}
				*heap[num] = *heap[num2];
				nodes[heap[num].pathNodeIndex].heapIndex = (ushort)num;
			}
			Hint.Assume(num < heap.length);
			*heap[num] = node;
			nodes[node.pathNodeIndex].heapIndex = (ushort)num;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00010D6C File Offset: 0x0000EF6C
		public uint Remove(UnsafeSpan<PathNode> nodes, out uint g, out uint h)
		{
			uint num2;
			uint num3;
			uint num = BinaryHeap.Remove(ref nodes, ref this, out num2, out num3);
			h = ((this.tieBreaking == BinaryHeap.TieBreaking.HScore) ? num2 : 0U);
			g = num3 - h;
			return num;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00010D99 File Offset: 0x0000EF99
		[BurstCompile]
		private static uint Remove(ref UnsafeSpan<PathNode> nodes, ref BinaryHeap binaryHeap, [NoAlias] out uint removedTieBreaker, [NoAlias] out uint removedF)
		{
			return BinaryHeap.Remove_00000376$BurstDirectCall.Invoke(ref nodes, ref binaryHeap, out removedTieBreaker, out removedF);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00010DA4 File Offset: 0x0000EFA4
		[Conditional("VALIDATE_BINARY_HEAP")]
		private static void Validate(ref UnsafeSpan<PathNode> nodes, ref BinaryHeap binaryHeap)
		{
			for (int i = 1; i < binaryHeap.numberOfItems; i++)
			{
				int num = (i - 1) / 4;
				if (binaryHeap.heap[num].F > binaryHeap.heap[i].F)
				{
					throw new Exception(string.Concat(new string[]
					{
						"Invalid state at ",
						i.ToString(),
						":",
						num.ToString(),
						" ( ",
						binaryHeap.heap[num].F.ToString(),
						" > ",
						binaryHeap.heap[i].F.ToString(),
						" ) "
					}));
				}
				if (binaryHeap.heap[num].sortKey > binaryHeap.heap[i].sortKey)
				{
					throw new Exception(string.Concat(new string[]
					{
						"Invalid state at ",
						i.ToString(),
						":",
						num.ToString(),
						" ( ",
						binaryHeap.heap[num].F.ToString(),
						" > ",
						binaryHeap.heap[i].F.ToString(),
						" ) "
					}));
				}
				if ((int)nodes[binaryHeap.heap[i].pathNodeIndex].heapIndex != i)
				{
					throw new Exception("Invalid heap index");
				}
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00010F54 File Offset: 0x0000F154
		public unsafe void Rebuild(UnsafeSpan<PathNode> nodes)
		{
			for (int i = 2; i < this.numberOfItems; i++)
			{
				int num = i;
				BinaryHeap.HeapNode heapNode = *this.heap[i];
				uint f = heapNode.F;
				while (num != 1)
				{
					int num2 = num / 4;
					if (f >= this.heap[num2].F)
					{
						break;
					}
					*this.heap[num] = *this.heap[num2];
					nodes[this.heap[num].pathNodeIndex].heapIndex = (ushort)num;
					*this.heap[num2] = heapNode;
					nodes[this.heap[num2].pathNodeIndex].heapIndex = (ushort)num2;
					num = num2;
				}
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00011038 File Offset: 0x0000F238
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void Add$BurstManaged(ref BinaryHeap binaryHeap, ref UnsafeSpan<PathNode> nodes, uint pathNodeIndex, uint g, uint h, uint insertionOrder, BinaryHeap.TieBreaking tieBreaking)
		{
			ref int ptr = ref binaryHeap.numberOfItems;
			ref UnsafeSpan<BinaryHeap.HeapNode> ptr2 = ref binaryHeap.heap;
			uint num = g + h;
			uint num2 = ((tieBreaking == BinaryHeap.TieBreaking.HScore) ? h : (insertionOrder << 2));
			ref PathNode ptr3 = ref nodes[pathNodeIndex];
			if (ptr3.heapIndex != 65535)
			{
				BinaryHeap.HeapNode heapNode = new BinaryHeap.HeapNode(pathNodeIndex, num2, num);
				BinaryHeap.DecreaseKey(ptr2, nodes, heapNode, ptr3.heapIndex);
				return;
			}
			if (ptr == ptr2.Length)
			{
				BinaryHeap.Expand(ref ptr2);
			}
			BinaryHeap.DecreaseKey(ptr2, nodes, new BinaryHeap.HeapNode(pathNodeIndex, num2, num), (ushort)ptr);
			ptr++;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000110D4 File Offset: 0x0000F2D4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static uint Remove$BurstManaged(ref UnsafeSpan<PathNode> nodes, ref BinaryHeap binaryHeap, [NoAlias] out uint removedTieBreaker, [NoAlias] out uint removedF)
		{
			ref int ptr = ref binaryHeap.numberOfItems;
			UnsafeSpan<BinaryHeap.HeapNode> unsafeSpan = binaryHeap.heap;
			if (ptr == 0)
			{
				throw new InvalidOperationException("Removing item from empty heap");
			}
			Hint.Assume(0UL < (ulong)unsafeSpan.length);
			uint pathNodeIndex = unsafeSpan[0].pathNodeIndex;
			nodes[pathNodeIndex].heapIndex = ushort.MaxValue;
			removedTieBreaker = unsafeSpan[0].TieBreaker;
			removedF = unsafeSpan[0].F;
			ptr--;
			if (ptr == 0)
			{
				return pathNodeIndex;
			}
			Hint.Assume(ptr < (int)unsafeSpan.length);
			BinaryHeap.HeapNode heapNode = *unsafeSpan[ptr];
			uint num = 0U;
			ulong num2 = heapNode.sortKey & 18446744073709551612UL;
			for (;;)
			{
				uint num3 = num;
				uint num4 = num3 * 4U + 1U;
				if ((ulong)num4 >= (ulong)((long)ptr))
				{
					break;
				}
				Hint.Assume(num4 < unsafeSpan.length);
				ulong num5 = (unsafeSpan[num4].sortKey & 18446744073709551612UL) | 0UL;
				Hint.Assume(num4 + 1U < unsafeSpan.length);
				ulong num6 = (unsafeSpan[num4 + 1U].sortKey & 18446744073709551612UL) | 1UL;
				Hint.Assume(num4 + 2U < unsafeSpan.length);
				ulong num7 = (unsafeSpan[num4 + 2U].sortKey & 18446744073709551612UL) | 2UL;
				Hint.Assume(num4 + 3U < unsafeSpan.length);
				ulong num8 = (unsafeSpan[num4 + 3U].sortKey & 18446744073709551612UL) | 3UL;
				ulong num9 = num5;
				if ((ulong)(num4 + 1U) < (ulong)((long)ptr))
				{
					num9 = math.min(num9, num6);
				}
				if ((ulong)(num4 + 2U) < (ulong)((long)ptr))
				{
					num9 = math.min(num9, num7);
				}
				if ((ulong)(num4 + 3U) < (ulong)((long)ptr))
				{
					num9 = math.min(num9, num8);
				}
				if ((num9 & 18446744073709551612UL) >= num2)
				{
					break;
				}
				num = num4 + (uint)(num9 & 3UL);
				Hint.Assume(num3 < unsafeSpan.length);
				Hint.Assume(num < unsafeSpan.length);
				*unsafeSpan[num3] = *unsafeSpan[num];
				Hint.Assume(num < unsafeSpan.length);
				nodes[unsafeSpan[num].pathNodeIndex].heapIndex = (ushort)num3;
			}
			Hint.Assume(num < unsafeSpan.length);
			*unsafeSpan[num] = heapNode;
			nodes[heapNode.pathNodeIndex].heapIndex = (ushort)num;
			return pathNodeIndex;
		}

		// Token: 0x0400020C RID: 524
		private UnsafeSpan<BinaryHeap.HeapNode> heap;

		// Token: 0x0400020D RID: 525
		public int numberOfItems;

		// Token: 0x0400020E RID: 526
		private uint insertionOrder;

		// Token: 0x0400020F RID: 527
		public BinaryHeap.TieBreaking tieBreaking;

		// Token: 0x04000210 RID: 528
		public const float GrowthFactor = 2f;

		// Token: 0x04000211 RID: 529
		private const int D = 4;

		// Token: 0x04000212 RID: 530
		public const ushort NotInHeap = 65535;

		// Token: 0x0200005B RID: 91
		public enum TieBreaking : byte
		{
			// Token: 0x04000214 RID: 532
			HScore,
			// Token: 0x04000215 RID: 533
			InsertionOrder
		}

		// Token: 0x0200005C RID: 92
		private struct HeapNode
		{
			// Token: 0x0600039C RID: 924 RVA: 0x00011330 File Offset: 0x0000F530
			public HeapNode(uint pathNodeIndex, uint tieBreaker, uint f)
			{
				this.pathNodeIndex = pathNodeIndex;
				this.sortKey = ((ulong)f << 32) | (ulong)tieBreaker;
			}

			// Token: 0x170000D2 RID: 210
			// (get) Token: 0x0600039D RID: 925 RVA: 0x00011347 File Offset: 0x0000F547
			// (set) Token: 0x0600039E RID: 926 RVA: 0x00011353 File Offset: 0x0000F553
			public uint F
			{
				get
				{
					return (uint)(this.sortKey >> 32);
				}
				set
				{
					this.sortKey = (this.sortKey & (ulong)(-1)) | ((ulong)value << 32);
				}
			}

			// Token: 0x170000D3 RID: 211
			// (get) Token: 0x0600039F RID: 927 RVA: 0x0001136A File Offset: 0x0000F56A
			// (set) Token: 0x060003A0 RID: 928 RVA: 0x00011373 File Offset: 0x0000F573
			public uint TieBreaker
			{
				get
				{
					return (uint)this.sortKey;
				}
				set
				{
					this.sortKey = (this.sortKey & 18446744069414584320UL) | (ulong)value;
				}
			}

			// Token: 0x04000216 RID: 534
			public uint pathNodeIndex;

			// Token: 0x04000217 RID: 535
			public ulong sortKey;
		}

		// Token: 0x0200005D RID: 93
		// (Invoke) Token: 0x060003A2 RID: 930
		internal delegate void Add_00000373$PostfixBurstDelegate(ref BinaryHeap binaryHeap, ref UnsafeSpan<PathNode> nodes, uint pathNodeIndex, uint g, uint h, uint insertionOrder, BinaryHeap.TieBreaking tieBreaking);

		// Token: 0x0200005E RID: 94
		internal static class Add_00000373$BurstDirectCall
		{
			// Token: 0x060003A5 RID: 933 RVA: 0x0001138E File Offset: 0x0000F58E
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (BinaryHeap.Add_00000373$BurstDirectCall.Pointer == 0)
				{
					BinaryHeap.Add_00000373$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BinaryHeap.Add_00000373$BurstDirectCall.DeferredCompilation, methodof(BinaryHeap.Add$BurstManaged(ref BinaryHeap, ref UnsafeSpan<PathNode>, uint, uint, uint, uint, BinaryHeap.TieBreaking)).MethodHandle, typeof(BinaryHeap.Add_00000373$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = BinaryHeap.Add_00000373$BurstDirectCall.Pointer;
			}

			// Token: 0x060003A6 RID: 934 RVA: 0x000113BC File Offset: 0x0000F5BC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				BinaryHeap.Add_00000373$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060003A7 RID: 935 RVA: 0x000113D4 File Offset: 0x0000F5D4
			public static void Constructor()
			{
				BinaryHeap.Add_00000373$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BinaryHeap.Add(ref BinaryHeap, ref UnsafeSpan<PathNode>, uint, uint, uint, uint, BinaryHeap.TieBreaking)).MethodHandle);
			}

			// Token: 0x060003A8 RID: 936 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x060003A9 RID: 937 RVA: 0x000113E5 File Offset: 0x0000F5E5
			// Note: this type is marked as 'beforefieldinit'.
			static Add_00000373$BurstDirectCall()
			{
				BinaryHeap.Add_00000373$BurstDirectCall.Constructor();
			}

			// Token: 0x060003AA RID: 938 RVA: 0x000113EC File Offset: 0x0000F5EC
			public static void Invoke(ref BinaryHeap binaryHeap, ref UnsafeSpan<PathNode> nodes, uint pathNodeIndex, uint g, uint h, uint insertionOrder, BinaryHeap.TieBreaking tieBreaking)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = BinaryHeap.Add_00000373$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.BinaryHeap&,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.PathNode>&,System.UInt32,System.UInt32,System.UInt32,System.UInt32,Pathfinding.BinaryHeap/TieBreaking), ref binaryHeap, ref nodes, pathNodeIndex, g, h, insertionOrder, tieBreaking, functionPointer);
						return;
					}
				}
				BinaryHeap.Add$BurstManaged(ref binaryHeap, ref nodes, pathNodeIndex, g, h, insertionOrder, tieBreaking);
			}

			// Token: 0x04000218 RID: 536
			private static IntPtr Pointer;

			// Token: 0x04000219 RID: 537
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200005F RID: 95
		// (Invoke) Token: 0x060003AC RID: 940
		internal delegate uint Remove_00000376$PostfixBurstDelegate(ref UnsafeSpan<PathNode> nodes, ref BinaryHeap binaryHeap, [NoAlias] out uint removedTieBreaker, [NoAlias] out uint removedF);

		// Token: 0x02000060 RID: 96
		internal static class Remove_00000376$BurstDirectCall
		{
			// Token: 0x060003AF RID: 943 RVA: 0x0001142F File Offset: 0x0000F62F
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (BinaryHeap.Remove_00000376$BurstDirectCall.Pointer == 0)
				{
					BinaryHeap.Remove_00000376$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BinaryHeap.Remove_00000376$BurstDirectCall.DeferredCompilation, methodof(BinaryHeap.Remove$BurstManaged(ref UnsafeSpan<PathNode>, ref BinaryHeap, ref uint, ref uint)).MethodHandle, typeof(BinaryHeap.Remove_00000376$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = BinaryHeap.Remove_00000376$BurstDirectCall.Pointer;
			}

			// Token: 0x060003B0 RID: 944 RVA: 0x0001145C File Offset: 0x0000F65C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				BinaryHeap.Remove_00000376$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060003B1 RID: 945 RVA: 0x00011474 File Offset: 0x0000F674
			public static void Constructor()
			{
				BinaryHeap.Remove_00000376$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BinaryHeap.Remove(ref UnsafeSpan<PathNode>, ref BinaryHeap, ref uint, ref uint)).MethodHandle);
			}

			// Token: 0x060003B2 RID: 946 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x060003B3 RID: 947 RVA: 0x00011485 File Offset: 0x0000F685
			// Note: this type is marked as 'beforefieldinit'.
			static Remove_00000376$BurstDirectCall()
			{
				BinaryHeap.Remove_00000376$BurstDirectCall.Constructor();
			}

			// Token: 0x060003B4 RID: 948 RVA: 0x0001148C File Offset: 0x0000F68C
			public static uint Invoke(ref UnsafeSpan<PathNode> nodes, ref BinaryHeap binaryHeap, [NoAlias] out uint removedTieBreaker, [NoAlias] out uint removedF)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = BinaryHeap.Remove_00000376$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.UInt32(Pathfinding.Collections.UnsafeSpan`1<Pathfinding.PathNode>&,Pathfinding.BinaryHeap&,System.UInt32&,System.UInt32&), ref nodes, ref binaryHeap, ref removedTieBreaker, ref removedF, functionPointer);
					}
				}
				return BinaryHeap.Remove$BurstManaged(ref nodes, ref binaryHeap, out removedTieBreaker, out removedF);
			}

			// Token: 0x0400021A RID: 538
			private static IntPtr Pointer;

			// Token: 0x0400021B RID: 539
			private static IntPtr DeferredCompilation;
		}
	}
}
