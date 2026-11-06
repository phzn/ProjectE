using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000023 RID: 35
	[NativeHeader("Modules/Physics2D/Public/CustomCollider2D.h")]
	public sealed class CustomCollider2D : Collider2D
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000387 RID: 903
		[NativeMethod("CustomShapeCount_Binding")]
		public extern int customShapeCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000388 RID: 904
		[NativeMethod("CustomVertexCount_Binding")]
		public extern int customVertexCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00007E5C File Offset: 0x0000605C
		public int GetCustomShapes(PhysicsShapeGroup2D physicsShapeGroup)
		{
			int customShapeCount = this.customShapeCount;
			bool flag = customShapeCount > 0;
			int num;
			if (flag)
			{
				num = this.GetCustomShapes_Internal(ref physicsShapeGroup.m_GroupState, 0, customShapeCount);
			}
			else
			{
				physicsShapeGroup.Clear();
				num = 0;
			}
			return num;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00007E98 File Offset: 0x00006098
		public int GetCustomShapes(PhysicsShapeGroup2D physicsShapeGroup, int shapeIndex, [DefaultValue("1")] int shapeCount = 1)
		{
			int customShapeCount = this.customShapeCount;
			bool flag = shapeIndex < 0 || shapeIndex >= customShapeCount || shapeCount < 1 || shapeIndex + shapeCount > customShapeCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot get shape range from {0} to {1} as CustomCollider2D only has {2} shape(s).", shapeIndex, shapeIndex + shapeCount - 1, customShapeCount));
			}
			return this.GetCustomShapes_Internal(ref physicsShapeGroup.m_GroupState, shapeIndex, shapeCount);
		}

		// Token: 0x0600038B RID: 907
		[NativeMethod("GetCustomShapes_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetCustomShapes_Internal(ref PhysicsShapeGroup2D.GroupState physicsShapeGroupState, int shapeIndex, int shapeCount);

		// Token: 0x0600038C RID: 908 RVA: 0x00007F00 File Offset: 0x00006100
		public int GetCustomShapes(NativeArray<PhysicsShape2D> shapes, NativeArray<Vector2> vertices)
		{
			bool flag = !shapes.IsCreated || shapes.Length != this.customShapeCount;
			if (flag)
			{
				throw new ArgumentException(string.Format("Cannot get custom shapes as the native shapes array length must be identical to the current custom shape count of {0}.", this.customShapeCount), "shapes");
			}
			bool flag2 = !vertices.IsCreated || vertices.Length != this.customVertexCount;
			if (flag2)
			{
				throw new ArgumentException(string.Format("Cannot get custom shapes as the native vertices array length must be identical to the current custom vertex count of {0}.", this.customVertexCount), "vertices");
			}
			return this.GetCustomShapesNative_Internal((IntPtr)shapes.GetUnsafeReadOnlyPtr<PhysicsShape2D>(), shapes.Length, (IntPtr)vertices.GetUnsafeReadOnlyPtr<Vector2>(), vertices.Length);
		}

		// Token: 0x0600038D RID: 909
		[NativeMethod("GetCustomShapesAllNative_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetCustomShapesNative_Internal(IntPtr shapesPtr, int shapeCount, IntPtr verticesPtr, int vertexCount);

		// Token: 0x0600038E RID: 910 RVA: 0x00007FC0 File Offset: 0x000061C0
		public void SetCustomShapes(PhysicsShapeGroup2D physicsShapeGroup)
		{
			bool flag = physicsShapeGroup.shapeCount > 0;
			if (flag)
			{
				this.SetCustomShapesAll_Internal(ref physicsShapeGroup.m_GroupState);
			}
			else
			{
				this.ClearCustomShapes();
			}
		}

		// Token: 0x0600038F RID: 911
		[NativeMethod("SetCustomShapesAll_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetCustomShapesAll_Internal(ref PhysicsShapeGroup2D.GroupState physicsShapeGroupState);

		// Token: 0x06000390 RID: 912 RVA: 0x00007FF4 File Offset: 0x000061F4
		public void SetCustomShapes(NativeArray<PhysicsShape2D> shapes, NativeArray<Vector2> vertices)
		{
			bool flag = !shapes.IsCreated || shapes.Length == 0;
			if (flag)
			{
				throw new ArgumentException("Cannot set custom shapes as the native shapes array is empty.", "shapes");
			}
			bool flag2 = !vertices.IsCreated || vertices.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("Cannot set custom shapes as the native vertices array is empty.", "vertices");
			}
			this.SetCustomShapesNative_Internal((IntPtr)shapes.GetUnsafeReadOnlyPtr<PhysicsShape2D>(), shapes.Length, (IntPtr)vertices.GetUnsafeReadOnlyPtr<Vector2>(), vertices.Length);
		}

		// Token: 0x06000391 RID: 913
		[NativeMethod("SetCustomShapesAllNative_Binding", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetCustomShapesNative_Internal(IntPtr shapesPtr, int shapeCount, IntPtr verticesPtr, int vertexCount);

		// Token: 0x06000392 RID: 914 RVA: 0x00008084 File Offset: 0x00006284
		public void SetCustomShape(PhysicsShapeGroup2D physicsShapeGroup, int srcShapeIndex, int dstShapeIndex)
		{
			bool flag = srcShapeIndex < 0 || srcShapeIndex >= physicsShapeGroup.shapeCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot set custom shape at {0} as the shape group only has {1} shape(s).", srcShapeIndex, physicsShapeGroup.shapeCount));
			}
			PhysicsShape2D shape = physicsShapeGroup.GetShape(srcShapeIndex);
			bool flag2 = shape.vertexStartIndex < 0 || shape.vertexStartIndex >= physicsShapeGroup.vertexCount || shape.vertexCount < 1 || shape.vertexStartIndex + shape.vertexCount > physicsShapeGroup.vertexCount;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot set custom shape at {0} as its shape indices are out of the available vertices ranges.", srcShapeIndex));
			}
			bool flag3 = dstShapeIndex < 0 || dstShapeIndex >= this.customShapeCount;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot set custom shape at destination {0} as CustomCollider2D only has {1} custom shape(s). The destination index must be within the existing shape range.", dstShapeIndex, this.customShapeCount));
			}
			this.SetCustomShape_Internal(ref physicsShapeGroup.m_GroupState, srcShapeIndex, dstShapeIndex);
		}

		// Token: 0x06000393 RID: 915
		[NativeMethod("SetCustomShape_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetCustomShape_Internal(ref PhysicsShapeGroup2D.GroupState physicsShapeGroupState, int srcShapeIndex, int dstShapeIndex);

		// Token: 0x06000394 RID: 916 RVA: 0x00008170 File Offset: 0x00006370
		public void SetCustomShape(NativeArray<PhysicsShape2D> shapes, NativeArray<Vector2> vertices, int srcShapeIndex, int dstShapeIndex)
		{
			bool flag = !shapes.IsCreated || shapes.Length == 0;
			if (flag)
			{
				throw new ArgumentException("Cannot set custom shapes as the native shapes array is empty.", "shapes");
			}
			bool flag2 = !vertices.IsCreated || vertices.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("Cannot set custom shapes as the native vertices array is empty.", "vertices");
			}
			bool flag3 = srcShapeIndex < 0 || srcShapeIndex >= shapes.Length;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot set custom shape at {0} as the shape native array only has {1} shape(s).", srcShapeIndex, shapes.Length));
			}
			PhysicsShape2D physicsShape2D = shapes[srcShapeIndex];
			bool flag4 = physicsShape2D.vertexStartIndex < 0 || physicsShape2D.vertexStartIndex >= vertices.Length || physicsShape2D.vertexCount < 1 || physicsShape2D.vertexStartIndex + physicsShape2D.vertexCount > vertices.Length;
			if (flag4)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot set custom shape at {0} as its shape indices are out of the available vertices ranges.", srcShapeIndex));
			}
			bool flag5 = dstShapeIndex < 0 || dstShapeIndex >= this.customShapeCount;
			if (flag5)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot set custom shape at destination {0} as CustomCollider2D only has {1} custom shape(s). The destination index must be within the existing shape range.", dstShapeIndex, this.customShapeCount));
			}
			this.SetCustomShapeNative_Internal((IntPtr)shapes.GetUnsafeReadOnlyPtr<PhysicsShape2D>(), shapes.Length, (IntPtr)vertices.GetUnsafeReadOnlyPtr<Vector2>(), vertices.Length, srcShapeIndex, dstShapeIndex);
		}

		// Token: 0x06000395 RID: 917
		[NativeMethod("SetCustomShapeNative_Binding", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetCustomShapeNative_Internal(IntPtr shapesPtr, int shapeCount, IntPtr verticesPtr, int vertexCount, int srcShapeIndex, int dstShapeIndex);

		// Token: 0x06000396 RID: 918 RVA: 0x000082DC File Offset: 0x000064DC
		public void ClearCustomShapes(int shapeIndex, int shapeCount)
		{
			int customShapeCount = this.customShapeCount;
			bool flag = shapeIndex < 0 || shapeIndex >= customShapeCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot clear custom shape(s) at index {0} as the CustomCollider2D only has {1} shape(s).", shapeIndex, customShapeCount));
			}
			bool flag2 = shapeIndex + shapeCount < 0 || shapeIndex + shapeCount > this.customShapeCount;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot clear custom shape(s) in the range (index {0}, count {1}) as this range is outside range of the existing {2} shape(s).", shapeIndex, shapeCount, this.customShapeCount));
			}
			this.ClearCustomShapes_Internal(shapeIndex, shapeCount);
		}

		// Token: 0x06000397 RID: 919
		[NativeMethod("ClearCustomShapes_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ClearCustomShapes_Internal(int shapeIndex, int shapeCount);

		// Token: 0x06000398 RID: 920
		[NativeMethod("ClearCustomShapes_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearCustomShapes();
	}
}
