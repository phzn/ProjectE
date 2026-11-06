using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace GPUInstancerPro
{
	// Token: 0x02000009 RID: 9
	public interface IGPUIInstanceTransformProvider
	{
		// Token: 0x06000053 RID: 83
		int GetPrefabID(GameObject prefabObject);

		// Token: 0x06000054 RID: 84
		NativeArray<Matrix4x4> GetTransformMatrix(int prefabID);

		// Token: 0x06000055 RID: 85
		void SetTransformMatrixModified(int prefabID);

		// Token: 0x06000056 RID: 86
		TransformAccessArray GetTransformAccessArray(int prefabID);

		// Token: 0x06000057 RID: 87
		Transform[] GetInstanceTransforms(int prefabID);

		// Token: 0x06000058 RID: 88
		Transform GetInstanceTransform(int prefabID, int bufferIndex);

		// Token: 0x06000059 RID: 89
		Transform GetInstanceTransformWithRenderKey(int renderKey, int bufferIndex);

		// Token: 0x0600005A RID: 90
		int GetInstanceCount(int prefabID);
	}
}
