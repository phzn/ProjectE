using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001A4 RID: 420
	[BurstCompile]
	internal static class ColliderMeshBuilder2D
	{
		// Token: 0x06000C3D RID: 3133 RVA: 0x0004367C File Offset: 0x0004187C
		private static int GetShapes(Collider2D coll, PhysicsShapeGroup2D group, HashSet<Rigidbody2D> handledRigidbodies)
		{
			Rigidbody2D attachedRigidbody = coll.attachedRigidbody;
			if (!(attachedRigidbody != null))
			{
				TilemapCollider2D tilemapCollider2D = coll as TilemapCollider2D;
				if (tilemapCollider2D != null)
				{
					tilemapCollider2D.ProcessTilemapChanges();
				}
				return coll.GetShapes(group);
			}
			if (handledRigidbodies.Add(attachedRigidbody))
			{
				return attachedRigidbody.GetShapes(group);
			}
			return 0;
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x000436C4 File Offset: 0x000418C4
		public unsafe static int GenerateMeshesFromColliders(Collider2D[] colliders, int numColliders, float maxError, out UnsafeSpan<float3> outputVertices, out UnsafeSpan<int> outputIndices, out UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh> outputShapeMeshes)
		{
			PhysicsShapeGroup2D physicsShapeGroup2D = new PhysicsShapeGroup2D(1, 8);
			NativeList<PhysicsShape2D> nativeList = new NativeList<PhysicsShape2D>(numColliders, Allocator.Temp);
			NativeList<Vector2> nativeList2 = new NativeList<Vector2>(numColliders * 4, Allocator.Temp);
			NativeList<Matrix4x4> nativeList3 = new NativeList<Matrix4x4>(numColliders, Allocator.Temp);
			NativeList<int> nativeList4 = new NativeList<int>(numColliders, Allocator.Temp);
			HashSet<Rigidbody2D> hashSet = new HashSet<Rigidbody2D>();
			int num = 0;
			for (int i = 0; i < numColliders; i++)
			{
				Collider2D collider2D = colliders[i];
				if (!(collider2D == null) && collider2D.shapeCount != 0)
				{
					int shapes = ColliderMeshBuilder2D.GetShapes(collider2D, physicsShapeGroup2D, hashSet);
					if (shapes != 0)
					{
						nativeList.Length += shapes;
						nativeList2.Length += physicsShapeGroup2D.vertexCount;
						NativeArray<PhysicsShape2D> subArray = nativeList.AsArray().GetSubArray(nativeList.Length - shapes, shapes);
						NativeArray<Vector2> subArray2 = nativeList2.AsArray().GetSubArray(nativeList2.Length - physicsShapeGroup2D.vertexCount, physicsShapeGroup2D.vertexCount);
						physicsShapeGroup2D.GetShapeData(subArray, subArray2);
						for (int j = 0; j < shapes; j++)
						{
							PhysicsShape2D physicsShape2D = subArray[j];
							physicsShape2D.vertexStartIndex += num;
							subArray[j] = physicsShape2D;
						}
						num += subArray2.Length;
						Matrix4x4 localToWorldMatrix = physicsShapeGroup2D.localToWorldMatrix;
						nativeList3.AddReplicate(in localToWorldMatrix, shapes);
						nativeList4.AddReplicate(in i, shapes);
					}
				}
			}
			NativeList<float3> nativeList5 = new NativeList<float3>(Allocator.Temp);
			NativeList<int3> nativeList6 = new NativeList<int3>(Allocator.Temp);
			UnsafeSpan<PhysicsShape2D> unsafeSpan = nativeList.AsUnsafeSpan<PhysicsShape2D>();
			UnsafeSpan<float2> unsafeSpan2 = nativeList2.AsUnsafeSpan<Vector2>().Reinterpret<float2>();
			UnsafeSpan<Matrix4x4> unsafeSpan3 = nativeList3.AsUnsafeSpan<Matrix4x4>();
			UnsafeSpan<int> unsafeSpan4 = nativeList4.AsUnsafeSpan<int>();
			outputShapeMeshes = new UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh>(Allocator.Persistent, nativeList.Length);
			int num2 = ColliderMeshBuilder2D.GenerateMeshesFromShapes(ref unsafeSpan, ref unsafeSpan2, ref unsafeSpan3, ref unsafeSpan4, UnsafeUtility.AsRef<UnsafeList<float3>>((void*)nativeList5.GetUnsafeList()), UnsafeUtility.AsRef<UnsafeList<int3>>((void*)nativeList6.GetUnsafeList()), ref outputShapeMeshes, maxError);
			outputVertices = nativeList5.AsUnsafeSpan<float3>().Clone(Allocator.Persistent);
			outputIndices = nativeList6.AsArray().Reinterpret<int>(12).AsUnsafeSpan<int>()
				.Clone(Allocator.Persistent);
			return num2;
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x000438F8 File Offset: 0x00041AF8
		private static void AddCapsuleMesh(float2 c1, float2 c2, ref Matrix4x4 shapeMatrix, float radius, float maxError, ref UnsafeList<float3> outputVertices, ref UnsafeList<int3> outputIndices, ref float3 mn, ref float3 mx)
		{
			int num = math.max(4, CircleGeometryUtilities.CircleSteps(shapeMatrix, radius, maxError));
			num = num / 2 + 1;
			radius *= CircleGeometryUtilities.CircleRadiusAdjustmentFactor(2 * (num - 1));
			Vector3 vector = new Vector3(c1.x, c1.y, 0f);
			Vector3 vector2 = new Vector3(c2.x, c2.y, 0f);
			float2 @float = math.normalizesafe(c2 - c1, default(float2));
			float2 float2 = new float2(-@float.y, @float.x);
			Vector3 vector3 = radius * new Vector3(float2.x, float2.y, 0f);
			Vector3 vector4 = radius * new Vector3(@float.x, @float.y, 0f);
			float num2 = 3.1415927f / (float)(num - 1);
			int length = outputVertices.Length;
			int num3 = length + num;
			outputVertices.Length += num * 2;
			for (int i = 0; i < num; i++)
			{
				float num4;
				float num5;
				math.sincos(num2 * (float)i, out num4, out num5);
				Vector3 vector5 = vector + num5 * vector3 - num4 * vector4;
				mn = math.min(mn, vector5);
				mx = math.max(mx, vector5);
				outputVertices[length + i] = vector5;
				vector5 = vector2 - num5 * vector3 + num4 * vector4;
				mn = math.min(mn, vector5);
				mx = math.max(mx, vector5);
				outputVertices[num3 + i] = vector5;
			}
			int length2 = outputIndices.Length;
			int num6 = length2 + num - 2;
			outputIndices.Length += (num - 2) * 2;
			for (int j = 1; j < num - 1; j++)
			{
				outputIndices[length2 + j - 1] = new int3(length, length + j, length + j + 1);
				outputIndices[num6 + j - 1] = new int3(num3, num3 + j, num3 + j + 1);
			}
			int3 @int = new int3(length, length + num - 1, num3);
			outputIndices.Add(in @int);
			@int = new int3(length, num3, num3 + num - 1);
			outputIndices.Add(in @int);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00043B90 File Offset: 0x00041D90
		[BurstCompile]
		public static int GenerateMeshesFromShapes(ref UnsafeSpan<PhysicsShape2D> shapes, ref UnsafeSpan<float2> vertices, ref UnsafeSpan<Matrix4x4> shapeMatrices, ref UnsafeSpan<int> groupIndices, ref UnsafeList<float3> outputVertices, ref UnsafeList<int3> outputIndices, ref UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh> outputShapeMeshes, float maxError)
		{
			return ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000B85$BurstDirectCall.Invoke(ref shapes, ref vertices, ref shapeMatrices, ref groupIndices, ref outputVertices, ref outputIndices, ref outputShapeMeshes, maxError);
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00043BA4 File Offset: 0x00041DA4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int GenerateMeshesFromShapes$BurstManaged(ref UnsafeSpan<PhysicsShape2D> shapes, ref UnsafeSpan<float2> vertices, ref UnsafeSpan<Matrix4x4> shapeMatrices, ref UnsafeSpan<int> groupIndices, ref UnsafeList<float3> outputVertices, ref UnsafeList<int3> outputIndices, ref UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh> outputShapeMeshes, float maxError)
		{
			int num = 0;
			float3 @float = new float3(float.MaxValue, float.MaxValue, float.MaxValue);
			float3 float2 = new float3(float.MinValue, float.MinValue, float.MinValue);
			int num2 = 0;
			for (int i = 0; i < shapes.Length; i++)
			{
				PhysicsShape2D physicsShape2D = *shapes[i];
				UnsafeSpan<float2> unsafeSpan = vertices.Slice(physicsShape2D.vertexStartIndex, physicsShape2D.vertexCount);
				Matrix4x4 matrix4x = *shapeMatrices[i];
				switch (physicsShape2D.shapeType)
				{
				case PhysicsShapeType2D.Circle:
				{
					int num3 = CircleGeometryUtilities.CircleSteps(matrix4x, physicsShape2D.radius, maxError);
					float num4 = physicsShape2D.radius * CircleGeometryUtilities.CircleRadiusAdjustmentFactor(num3);
					Vector3 vector = new Vector3(unsafeSpan[0].x, unsafeSpan[0].y, 0f);
					Vector3 vector2 = new Vector3(num4, 0f, 0f);
					Vector3 vector3 = new Vector3(0f, num4, 0f);
					float num5 = 6.2831855f / (float)num3;
					int length = outputVertices.Length;
					for (int j = 0; j < num3; j++)
					{
						float num6;
						float num7;
						math.sincos(num5 * (float)j, out num6, out num7);
						Vector3 vector4 = vector + num7 * vector2 + num6 * vector3;
						@float = math.min(@float, vector4);
						float2 = math.max(float2, vector4);
						float3 float3 = vector4;
						outputVertices.Add(in float3);
					}
					for (int k = 1; k < num3; k++)
					{
						int3 @int = new int3(length, length + k, length + (k + 1) % num3);
						outputIndices.Add(in @int);
					}
					break;
				}
				case PhysicsShapeType2D.Capsule:
				{
					float2 float4 = *unsafeSpan[0];
					float2 float5 = *unsafeSpan[1];
					ColliderMeshBuilder2D.AddCapsuleMesh(float4, float5, ref matrix4x, physicsShape2D.radius, maxError, ref outputVertices, ref outputIndices, ref @float, ref float2);
					break;
				}
				case PhysicsShapeType2D.Polygon:
				{
					int length2 = outputVertices.Length;
					outputVertices.Resize(length2 + physicsShape2D.vertexCount, NativeArrayOptions.UninitializedMemory);
					for (int l = 0; l < physicsShape2D.vertexCount; l++)
					{
						Vector3 vector5 = new Vector3(unsafeSpan[l].x, unsafeSpan[l].y, 0f);
						@float = math.min(@float, vector5);
						float2 = math.max(float2, vector5);
						outputVertices[length2 + l] = vector5;
					}
					outputIndices.SetCapacity(math.ceilpow2(outputIndices.Length + (physicsShape2D.vertexCount - 2)));
					for (int m = 1; m < physicsShape2D.vertexCount - 1; m++)
					{
						outputIndices.AddNoResize(new int3(length2, length2 + m, length2 + m + 1));
					}
					break;
				}
				case PhysicsShapeType2D.Edges:
					if (physicsShape2D.radius > maxError)
					{
						for (int n = 0; n < physicsShape2D.vertexCount - 1; n++)
						{
							ColliderMeshBuilder2D.AddCapsuleMesh(*unsafeSpan[n], *unsafeSpan[n + 1], ref matrix4x, physicsShape2D.radius, maxError, ref outputVertices, ref outputIndices, ref @float, ref float2);
						}
					}
					else
					{
						int length3 = outputVertices.Length;
						outputVertices.Resize(length3 + physicsShape2D.vertexCount, NativeArrayOptions.UninitializedMemory);
						for (int num8 = 0; num8 < physicsShape2D.vertexCount; num8++)
						{
							Vector3 vector6 = new Vector3(unsafeSpan[num8].x, unsafeSpan[num8].y, 0f);
							@float = math.min(@float, vector6);
							float2 = math.max(float2, vector6);
							outputVertices[length3 + num8] = vector6;
						}
						outputIndices.SetCapacity(math.ceilpow2(outputIndices.Length + (physicsShape2D.vertexCount - 1)));
						for (int num9 = 0; num9 < physicsShape2D.vertexCount - 1; num9++)
						{
							outputIndices.AddNoResize(new int3(length3 + num9, length3 + num9 + 1, length3 + num9 + 1));
						}
					}
					break;
				default:
					throw new Exception("Unexpected PhysicsShapeType2D");
				}
				if (i == shapes.Length - 1 || *groupIndices[i] != *groupIndices[i + 1] || outputIndices.Length - num > 100)
				{
					ToWorldMatrix toWorldMatrix = new ToWorldMatrix(new float3x3(matrix4x));
					Bounds bounds = new Bounds((@float + float2) * 0.5f, float2 - @float);
					bounds = toWorldMatrix.ToWorld(bounds);
					bounds.center += matrix4x.GetColumn(3);
					*outputShapeMeshes[num2++] = new ColliderMeshBuilder2D.ShapeMesh
					{
						bounds = bounds,
						matrix = matrix4x,
						startIndex = num * 3,
						endIndex = outputIndices.Length * 3,
						tag = *groupIndices[i]
					};
					@float = new float3(float.MaxValue, float.MaxValue, float.MaxValue);
					float2 = new float3(float.MinValue, float.MinValue, float.MinValue);
					num = outputIndices.Length;
				}
			}
			return num2;
		}

		// Token: 0x020001A5 RID: 421
		public struct ShapeMesh
		{
			// Token: 0x0400081B RID: 2075
			public Matrix4x4 matrix;

			// Token: 0x0400081C RID: 2076
			public Bounds bounds;

			// Token: 0x0400081D RID: 2077
			public int startIndex;

			// Token: 0x0400081E RID: 2078
			public int endIndex;

			// Token: 0x0400081F RID: 2079
			public int tag;
		}

		// Token: 0x020001A6 RID: 422
		// (Invoke) Token: 0x06000C43 RID: 3139
		internal delegate int GenerateMeshesFromShapes_00000B85$PostfixBurstDelegate(ref UnsafeSpan<PhysicsShape2D> shapes, ref UnsafeSpan<float2> vertices, ref UnsafeSpan<Matrix4x4> shapeMatrices, ref UnsafeSpan<int> groupIndices, ref UnsafeList<float3> outputVertices, ref UnsafeList<int3> outputIndices, ref UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh> outputShapeMeshes, float maxError);

		// Token: 0x020001A7 RID: 423
		internal static class GenerateMeshesFromShapes_00000B85$BurstDirectCall
		{
			// Token: 0x06000C46 RID: 3142 RVA: 0x000440FF File Offset: 0x000422FF
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000B85$BurstDirectCall.Pointer == 0)
				{
					ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000B85$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000B85$BurstDirectCall.DeferredCompilation, methodof(ColliderMeshBuilder2D.GenerateMeshesFromShapes$BurstManaged(ref UnsafeSpan<PhysicsShape2D>, ref UnsafeSpan<float2>, ref UnsafeSpan<Matrix4x4>, ref UnsafeSpan<int>, ref UnsafeList<float3>, ref UnsafeList<int3>, ref UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh>, float)).MethodHandle, typeof(ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000B85$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000B85$BurstDirectCall.Pointer;
			}

			// Token: 0x06000C47 RID: 3143 RVA: 0x0004412C File Offset: 0x0004232C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000B85$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000C48 RID: 3144 RVA: 0x00044144 File Offset: 0x00042344
			public static void Constructor()
			{
				ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000B85$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ColliderMeshBuilder2D.GenerateMeshesFromShapes(ref UnsafeSpan<PhysicsShape2D>, ref UnsafeSpan<float2>, ref UnsafeSpan<Matrix4x4>, ref UnsafeSpan<int>, ref UnsafeList<float3>, ref UnsafeList<int3>, ref UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh>, float)).MethodHandle);
			}

			// Token: 0x06000C49 RID: 3145 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000C4A RID: 3146 RVA: 0x00044155 File Offset: 0x00042355
			// Note: this type is marked as 'beforefieldinit'.
			static GenerateMeshesFromShapes_00000B85$BurstDirectCall()
			{
				ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000B85$BurstDirectCall.Constructor();
			}

			// Token: 0x06000C4B RID: 3147 RVA: 0x0004415C File Offset: 0x0004235C
			public static int Invoke(ref UnsafeSpan<PhysicsShape2D> shapes, ref UnsafeSpan<float2> vertices, ref UnsafeSpan<Matrix4x4> shapeMatrices, ref UnsafeSpan<int> groupIndices, ref UnsafeList<float3> outputVertices, ref UnsafeList<int3> outputIndices, ref UnsafeSpan<ColliderMeshBuilder2D.ShapeMesh> outputShapeMeshes, float maxError)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000B85$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(Pathfinding.Collections.UnsafeSpan`1<UnityEngine.PhysicsShape2D>&,Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float2>&,Pathfinding.Collections.UnsafeSpan`1<UnityEngine.Matrix4x4>&,Pathfinding.Collections.UnsafeSpan`1<System.Int32>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Mathematics.float3>&,Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Unity.Mathematics.int3>&,Pathfinding.Collections.UnsafeSpan`1<Pathfinding.Graphs.Navmesh.ColliderMeshBuilder2D/ShapeMesh>&,System.Single), ref shapes, ref vertices, ref shapeMatrices, ref groupIndices, ref outputVertices, ref outputIndices, ref outputShapeMeshes, maxError, functionPointer);
					}
				}
				return ColliderMeshBuilder2D.GenerateMeshesFromShapes$BurstManaged(ref shapes, ref vertices, ref shapeMatrices, ref groupIndices, ref outputVertices, ref outputIndices, ref outputShapeMeshes, maxError);
			}

			// Token: 0x04000820 RID: 2080
			private static IntPtr Pointer;

			// Token: 0x04000821 RID: 2081
			private static IntPtr DeferredCompilation;
		}
	}
}
