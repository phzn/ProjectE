using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000204 RID: 516
	[GenerateTestsForBurstCompatibility]
	[BurstCompile]
	internal static class EnabledBitUtility
	{
		// Token: 0x06000BEE RID: 3054 RVA: 0x00037A2C File Offset: 0x00035C2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool TryGetNextRange(v128 mask, int firstIndexToCheck, out int nextRangeBegin, out int nextRangeEnd)
		{
			mask = EnabledBitUtility.ShiftRight(in mask, firstIndexToCheck);
			int num = EnabledBitUtility.tzcnt_u128(mask);
			mask = EnabledBitUtility.ShiftRight(in mask, num);
			int num2 = EnabledBitUtility.tzcnt_u128(new v128(~mask.ULong0, ~mask.ULong1));
			nextRangeBegin = firstIndexToCheck + num;
			nextRangeEnd = nextRangeBegin + num2;
			return num2 != 0;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00037A7C File Offset: 0x00035C7C
		internal static v128 ShiftRight(in v128 v, int n)
		{
			if (Hint.Unlikely(n >= 128))
			{
				return default(v128);
			}
			if (Hint.Unlikely(n == 0))
			{
				return v;
			}
			if (n >= 64)
			{
				return new v128(v.ULong1 >> n - 64, 0UL);
			}
			ulong num = v.ULong0 >> n;
			ulong num2 = v.ULong1 << 64 - n;
			return new v128(num | num2, v.ULong1 >> n);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00037AFE File Offset: 0x00035CFE
		[BurstCompile]
		internal static void ShiftRightBurstForTests(ref v128 v, out v128 vOut, int n)
		{
			EnabledBitUtility.ShiftRightBurstForTests_00000A64$BurstDirectCall.Invoke(ref v, out vOut, n);
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00037B08 File Offset: 0x00035D08
		internal static int tzcnt_u128(v128 u)
		{
			int num = math.tzcnt(u.ULong1) + 64;
			int num2 = math.tzcnt(u.ULong0);
			if (num2 != 64)
			{
				return num2;
			}
			return num;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00037B38 File Offset: 0x00035D38
		internal static int countbits(v128 u)
		{
			return math.countbits(u.ULong0) + math.countbits(u.ULong1);
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00037B51 File Offset: 0x00035D51
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ShiftRightBurstForTests$BurstManaged(ref v128 v, out v128 vOut, int n)
		{
			vOut = EnabledBitUtility.ShiftRight(in v, n);
		}

		// Token: 0x02000205 RID: 517
		// (Invoke) Token: 0x06000BF5 RID: 3061
		internal delegate void ShiftRightBurstForTests_00000A64$PostfixBurstDelegate(ref v128 v, out v128 vOut, int n);

		// Token: 0x02000206 RID: 518
		internal static class ShiftRightBurstForTests_00000A64$BurstDirectCall
		{
			// Token: 0x06000BF8 RID: 3064 RVA: 0x00037B60 File Offset: 0x00035D60
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EnabledBitUtility.ShiftRightBurstForTests_00000A64$BurstDirectCall.Pointer == 0)
				{
					EnabledBitUtility.ShiftRightBurstForTests_00000A64$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EnabledBitUtility.ShiftRightBurstForTests_00000A64$BurstDirectCall.DeferredCompilation, methodof(EnabledBitUtility.ShiftRightBurstForTests$BurstManaged(ref v128, ref v128, int)).MethodHandle, typeof(EnabledBitUtility.ShiftRightBurstForTests_00000A64$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EnabledBitUtility.ShiftRightBurstForTests_00000A64$BurstDirectCall.Pointer;
			}

			// Token: 0x06000BF9 RID: 3065 RVA: 0x00037B8C File Offset: 0x00035D8C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EnabledBitUtility.ShiftRightBurstForTests_00000A64$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000BFA RID: 3066 RVA: 0x00037BA4 File Offset: 0x00035DA4
			public static void Constructor()
			{
				EnabledBitUtility.ShiftRightBurstForTests_00000A64$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EnabledBitUtility.ShiftRightBurstForTests(ref v128, ref v128, int)).MethodHandle);
			}

			// Token: 0x06000BFB RID: 3067 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000BFC RID: 3068 RVA: 0x00037BB5 File Offset: 0x00035DB5
			// Note: this type is marked as 'beforefieldinit'.
			static ShiftRightBurstForTests_00000A64$BurstDirectCall()
			{
				EnabledBitUtility.ShiftRightBurstForTests_00000A64$BurstDirectCall.Constructor();
			}

			// Token: 0x06000BFD RID: 3069 RVA: 0x00037BBC File Offset: 0x00035DBC
			public static void Invoke(ref v128 v, out v128 vOut, int n)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EnabledBitUtility.ShiftRightBurstForTests_00000A64$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Burst.Intrinsics.v128&,Unity.Burst.Intrinsics.v128&,System.Int32), ref v, ref vOut, n, functionPointer);
						return;
					}
				}
				EnabledBitUtility.ShiftRightBurstForTests$BurstManaged(ref v, out vOut, n);
			}

			// Token: 0x04000554 RID: 1364
			private static IntPtr Pointer;

			// Token: 0x04000555 RID: 1365
			private static IntPtr DeferredCompilation;
		}
	}
}
