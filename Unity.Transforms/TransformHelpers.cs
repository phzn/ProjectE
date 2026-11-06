using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;

namespace Unity.Transforms
{
	// Token: 0x0200001B RID: 27
	[BurstCompile]
	public static class TransformHelpers
	{
		// Token: 0x0600007A RID: 122 RVA: 0x00003D37 File Offset: 0x00001F37
		public static float3 Forward(this float4x4 m)
		{
			return new float3(m.c2.x, m.c2.y, m.c2.z);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003D5F File Offset: 0x00001F5F
		public static float3 Back(this float4x4 m)
		{
			return -(in m).Forward();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003D6C File Offset: 0x00001F6C
		public static float3 Up(this float4x4 m)
		{
			return new float3(m.c1.x, m.c1.y, m.c1.z);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003D94 File Offset: 0x00001F94
		public static float3 Down(this float4x4 m)
		{
			return -(in m).Up();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003DA1 File Offset: 0x00001FA1
		public static float3 Right(this float4x4 m)
		{
			return new float3(m.c0.x, m.c0.y, m.c0.z);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003DC9 File Offset: 0x00001FC9
		public static float3 Left(this float4x4 m)
		{
			return -(in m).Right();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003DD6 File Offset: 0x00001FD6
		public static float3 Translation(this float4x4 m)
		{
			return new float3(m.c3.x, m.c3.y, m.c3.z);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003DFE File Offset: 0x00001FFE
		public static quaternion Rotation(this float4x4 m)
		{
			return new quaternion(math.orthonormalize(new float3x3(m)));
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003E18 File Offset: 0x00002018
		public static float3 Scale(this float4x4 m)
		{
			float4 @float = m.c0;
			float num = math.length(@float.xyz);
			@float = m.c1;
			float num2 = math.length(@float.xyz);
			@float = m.c2;
			return new float3(num, num2, math.length(@float.xyz));
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003E64 File Offset: 0x00002064
		public static float3 TransformPoint(this float4x4 m, in float3 p)
		{
			return math.mul(m, new float4(p, 1f)).xyz;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003E94 File Offset: 0x00002094
		public static float3 TransformDirection(this float4x4 m, in float3 d)
		{
			return math.rotate(m, d);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003EA7 File Offset: 0x000020A7
		public static quaternion TransformRotation(this float4x4 m, in quaternion q)
		{
			return math.mul(new quaternion(math.orthonormalize(new float3x3(m))), q);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003ECC File Offset: 0x000020CC
		public static float3 InverseTransformPoint(this float4x4 m, in float3 p)
		{
			return math.mul(math.inverse(m), new float4(p, 1f)).xyz;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003F01 File Offset: 0x00002101
		public static float3 InverseTransformDirection(this float4x4 m, in float3 d)
		{
			return math.rotate(math.inverse(m), d);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003F19 File Offset: 0x00002119
		public static quaternion InverseTransformRotation(this float4x4 m, in quaternion q)
		{
			return math.mul(new quaternion(math.orthonormalize(math.inverse(new float3x3(m)))), q);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003F40 File Offset: 0x00002140
		public static quaternion LookAtRotation(in float3 eyeWorldPosition, float3 targetWorldPosition, float3 worldUp)
		{
			return quaternion.LookRotationSafe(targetWorldPosition - eyeWorldPosition, worldUp);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003F54 File Offset: 0x00002154
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		public static void ComputeWorldTransformMatrix(in Entity entity, out float4x4 outputMatrix, ref ComponentLookup<LocalTransform> localTransformLookup, ref ComponentLookup<Parent> parentLookup, ref ComponentLookup<PostTransformMatrix> scaleLookup)
		{
			TransformHelpers.ComputeWorldTransformMatrix_0000005E$BurstDirectCall.Invoke(in entity, out outputMatrix, ref localTransformLookup, ref parentLookup, ref scaleLookup);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003F64 File Offset: 0x00002164
		[BurstCompile]
		[GenerateTestsForBurstCompatibility]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void ComputeWorldTransformMatrix$BurstManaged(in Entity entity, out float4x4 outputMatrix, ref ComponentLookup<LocalTransform> localTransformLookup, ref ComponentLookup<Parent> parentLookup, ref ComponentLookup<PostTransformMatrix> scaleLookup)
		{
			Entity* ptr;
			NativeList<Entity> nativeList;
			bool flag;
			bool flag2;
			int num;
			Entity entity2;
			checked
			{
				ptr = stackalloc Entity[unchecked((UIntPtr)16) * (UIntPtr)sizeof(Entity)];
				nativeList = default(NativeList<Entity>);
				flag = true;
				flag2 = scaleLookup.HasComponent(entity);
				*ptr = entity;
				num = 1;
				entity2 = entity;
			}
			Parent parent;
			while (parentLookup.TryGetComponent(entity2, out parent))
			{
				if (Hint.Unlikely(flag && num == 16))
				{
					flag = false;
					nativeList = new NativeList<Entity>(32, Allocator.TempJob);
					for (int i = 0; i < num; i++)
					{
						nativeList.AddNoResize(ptr[i]);
					}
				}
				entity2 = parent.Value;
				if (Hint.Likely(flag))
				{
					ptr[num] = entity2;
				}
				else
				{
					nativeList.Add(in entity2);
				}
				num++;
				if (Hint.Likely(!flag2))
				{
					flag2 = scaleLookup.HasComponent(entity2);
				}
			}
			if (Hint.Unlikely(!flag))
			{
				ptr = nativeList.GetUnsafeReadOnlyPtr<Entity>();
			}
			if (Hint.Unlikely(flag2))
			{
				outputMatrix = float4x4.identity;
				for (int j = num - 1; j >= 0; j--)
				{
					entity2 = ptr[j];
					LocalTransform localTransform;
					if (!Hint.Likely(localTransformLookup.TryGetComponent(entity2, out localTransform)))
					{
						if (Hint.Unlikely(nativeList.IsCreated))
						{
							nativeList.Dispose();
						}
						throw new InvalidOperationException(string.Format("Entity {0} does not have the required LocalTransform component", entity2));
					}
					outputMatrix = math.mul(outputMatrix, localTransform.ToMatrix());
					PostTransformMatrix postTransformMatrix;
					if (Hint.Unlikely(scaleLookup.TryGetComponent(entity2, out postTransformMatrix)))
					{
						outputMatrix = math.mul(outputMatrix, postTransformMatrix.Value);
					}
				}
			}
			else
			{
				outputMatrix = float4x4.identity;
				for (int k = num - 1; k >= 0; k--)
				{
					entity2 = ptr[k];
					LocalTransform localTransform2;
					if (!Hint.Likely(localTransformLookup.TryGetComponent(entity2, out localTransform2)))
					{
						if (Hint.Unlikely(nativeList.IsCreated))
						{
							nativeList.Dispose();
						}
						throw new InvalidOperationException(string.Format("Entity {0} does not have the required LocalTransform component", entity2));
					}
					outputMatrix = math.mul(outputMatrix, localTransform2.ToMatrix());
				}
			}
			if (Hint.Unlikely(nativeList.IsCreated))
			{
				nativeList.Dispose();
			}
		}

		// Token: 0x0200001C RID: 28
		// (Invoke) Token: 0x0600008D RID: 141
		internal delegate void ComputeWorldTransformMatrix_0000005E$PostfixBurstDelegate(in Entity entity, out float4x4 outputMatrix, ref ComponentLookup<LocalTransform> localTransformLookup, ref ComponentLookup<Parent> parentLookup, ref ComponentLookup<PostTransformMatrix> scaleLookup);

		// Token: 0x0200001D RID: 29
		internal static class ComputeWorldTransformMatrix_0000005E$BurstDirectCall
		{
			// Token: 0x06000090 RID: 144 RVA: 0x000041D2 File Offset: 0x000023D2
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (TransformHelpers.ComputeWorldTransformMatrix_0000005E$BurstDirectCall.Pointer == 0)
				{
					TransformHelpers.ComputeWorldTransformMatrix_0000005E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(TransformHelpers.ComputeWorldTransformMatrix_0000005E$BurstDirectCall.DeferredCompilation, methodof(TransformHelpers.ComputeWorldTransformMatrix$BurstManaged(ref Entity, ref float4x4, ref ComponentLookup<LocalTransform>, ref ComponentLookup<Parent>, ref ComponentLookup<PostTransformMatrix>)).MethodHandle, typeof(TransformHelpers.ComputeWorldTransformMatrix_0000005E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = TransformHelpers.ComputeWorldTransformMatrix_0000005E$BurstDirectCall.Pointer;
			}

			// Token: 0x06000091 RID: 145 RVA: 0x00004200 File Offset: 0x00002400
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				TransformHelpers.ComputeWorldTransformMatrix_0000005E$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000092 RID: 146 RVA: 0x00004218 File Offset: 0x00002418
			public static void Constructor()
			{
				TransformHelpers.ComputeWorldTransformMatrix_0000005E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(TransformHelpers.ComputeWorldTransformMatrix(ref Entity, ref float4x4, ref ComponentLookup<LocalTransform>, ref ComponentLookup<Parent>, ref ComponentLookup<PostTransformMatrix>)).MethodHandle);
			}

			// Token: 0x06000093 RID: 147 RVA: 0x0000283B File Offset: 0x00000A3B
			public static void Initialize()
			{
			}

			// Token: 0x06000094 RID: 148 RVA: 0x00004229 File Offset: 0x00002429
			// Note: this type is marked as 'beforefieldinit'.
			static ComputeWorldTransformMatrix_0000005E$BurstDirectCall()
			{
				TransformHelpers.ComputeWorldTransformMatrix_0000005E$BurstDirectCall.Constructor();
			}

			// Token: 0x06000095 RID: 149 RVA: 0x00004230 File Offset: 0x00002430
			public static void Invoke(in Entity entity, out float4x4 outputMatrix, ref ComponentLookup<LocalTransform> localTransformLookup, ref ComponentLookup<Parent> parentLookup, ref ComponentLookup<PostTransformMatrix> scaleLookup)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = TransformHelpers.ComputeWorldTransformMatrix_0000005E$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Entities.Entity&,Unity.Mathematics.float4x4&,Unity.Entities.ComponentLookup`1<Unity.Transforms.LocalTransform>&,Unity.Entities.ComponentLookup`1<Unity.Transforms.Parent>&,Unity.Entities.ComponentLookup`1<Unity.Transforms.PostTransformMatrix>&), ref entity, ref outputMatrix, ref localTransformLookup, ref parentLookup, ref scaleLookup, functionPointer);
						return;
					}
				}
				TransformHelpers.ComputeWorldTransformMatrix$BurstManaged(in entity, out outputMatrix, ref localTransformLookup, ref parentLookup, ref scaleLookup);
			}

			// Token: 0x0400004F RID: 79
			private static IntPtr Pointer;

			// Token: 0x04000050 RID: 80
			private static IntPtr DeferredCompilation;
		}
	}
}
