using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Jobs;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000019 RID: 25
	[BurstCompile]
	internal struct GPUIAutoUpdateTransformsJob : IJobParallelForTransform
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00005A70 File Offset: 0x00003C70
		public void Execute(int index, TransformAccess transform)
		{
			if (index >= this.instanceCount)
			{
				return;
			}
			Matrix4x4 matrix4x = UnsafeUtility.ReadArrayElementWithStride<Matrix4x4>(this.p_matrixArray, index, 64);
			if (transform.isValid)
			{
				Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
				if (!localToWorldMatrix.EqualsMatrix4x4(matrix4x))
				{
					UnsafeUtility.WriteArrayElementWithStride<int>(this.p_isModifiedArray, index, 4, 1);
					UnsafeUtility.WriteArrayElementWithStride<Matrix4x4>(this.p_matrixArray, index, 64, localToWorldMatrix);
					return;
				}
				UnsafeUtility.WriteArrayElementWithStride<int>(this.p_isModifiedArray, index, 4, 0);
				return;
			}
			else
			{
				if (!this.zeroMatrix.EqualsMatrix4x4(matrix4x))
				{
					UnsafeUtility.WriteArrayElementWithStride<int>(this.p_isModifiedArray, index, 4, 1);
					UnsafeUtility.WriteArrayElementWithStride<Matrix4x4>(this.p_matrixArray, index, 64, this.zeroMatrix);
					return;
				}
				UnsafeUtility.WriteArrayElementWithStride<int>(this.p_isModifiedArray, index, 4, 0);
				return;
			}
		}

		// Token: 0x0400007A RID: 122
		[ReadOnly]
		public int instanceCount;

		// Token: 0x0400007B RID: 123
		[ReadOnly]
		public Matrix4x4 zeroMatrix;

		// Token: 0x0400007C RID: 124
		[NativeDisableUnsafePtrRestriction]
		internal unsafe void* p_matrixArray;

		// Token: 0x0400007D RID: 125
		[NativeDisableUnsafePtrRestriction]
		internal unsafe void* p_isModifiedArray;
	}
}
