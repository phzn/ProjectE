using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Jobs;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class GPUIPrefabPrototypeData : GPUIPrototypeData
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000270C File Offset: 0x0000090C
		internal bool hasOptionalRenderers
		{
			get
			{
				return this._optionalRendererCount > 0;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002718 File Offset: 0x00000918
		public override bool Initialize(GPUIPrototype prototype)
		{
			if (base.Initialize(prototype))
			{
				if (this.instancesToAdd == null)
				{
					this.instancesToAdd = new List<GPUIPrefab>();
				}
				if (this.indexesToRemove == null)
				{
					this.indexesToRemove = new SortedSet<int>(new GPUIPrefabPrototypeData.IntInverseComparer());
				}
				this._optionalRendererCount = 0;
				GPUIPrefab gpuiprefab;
				if (prototype.prefabObject.TryGetComponent<GPUIPrefab>(out gpuiprefab))
				{
					this.prefabID = gpuiprefab.GetPrefabID();
					if (gpuiprefab.childOptionalRenderers != null)
					{
						this._optionalRendererCount = gpuiprefab.childOptionalRenderers.Count;
						if (this._optionalRendererCount > 0)
						{
							this.optionalRendererStatusData = new NativeArray<uint>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
						}
					}
				}
				else
				{
					this.prefabID = prototype.GetKey();
				}
				this._matrixArray = new NativeArray<Matrix4x4>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.isModifiedArray = new NativeArray<int>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.minModifiedIndex = int.MaxValue;
				this.maxModifiedIndex = -1;
				return true;
			}
			return false;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000027ED File Offset: 0x000009ED
		public override void Dispose()
		{
			base.Dispose();
			this.instanceCount = 0;
			this.instanceTransforms = null;
			List<GPUIPrefab> list = this.instancesToAdd;
			if (list != null)
			{
				list.Clear();
			}
			SortedSet<int> sortedSet = this.indexesToRemove;
			if (sortedSet == null)
			{
				return;
			}
			sortedSet.Clear();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002824 File Offset: 0x00000A24
		public override void ReleaseBuffers()
		{
			base.ReleaseBuffers();
			if (this._matrixArray.IsCreated)
			{
				this._matrixArray.Dispose();
			}
			if (this.isModifiedArray.IsCreated)
			{
				this.isModifiedArray.Dispose();
			}
			if (this.transformAccessArray.isCreated)
			{
				this.transformAccessArray.Dispose();
			}
			if (this.optionalRendererStatusData.IsCreated)
			{
				this.optionalRendererStatusData.Dispose();
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002897 File Offset: 0x00000A97
		public int GetRegisteredInstanceCount()
		{
			if (this.registeredInstances != null && this.registeredInstances.prefabInstances != null)
			{
				return this.registeredInstances.prefabInstances.Length;
			}
			return 0;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000028BD File Offset: 0x00000ABD
		public override NativeArray<Matrix4x4> GetTransformationMatrixArray()
		{
			return this._matrixArray;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000028C8 File Offset: 0x00000AC8
		internal void UpdateTransformAccessArray()
		{
			if (!this.isTransformReferencesModified)
			{
				return;
			}
			if (this.transformAccessArray.isCreated)
			{
				this.transformAccessArray.Dispose();
			}
			TransformAccessArray.Allocate(this.instanceTransforms.Length, -1, out this.transformAccessArray);
			for (int i = 0; i < this.instanceCount; i++)
			{
				this.transformAccessArray.Add(this.instanceTransforms[i]);
			}
			this.isTransformReferencesModified = false;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002935 File Offset: 0x00000B35
		public bool HasMatrixArray()
		{
			return this._matrixArray.IsCreated;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002942 File Offset: 0x00000B42
		internal unsafe void* GetMatrixArrayUnsafePtr()
		{
			return this._matrixArray.GetUnsafePtr<Matrix4x4>();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000294F File Offset: 0x00000B4F
		internal void ResizeMatrixArray(int newSize)
		{
			(ref this._matrixArray).ResizeNativeArray<Matrix4x4>(newSize, Allocator.Persistent);
			(ref this.isModifiedArray).ResizeNativeArray<int>(newSize, Allocator.Persistent);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000296B File Offset: 0x00000B6B
		public int GetMatrixLength()
		{
			return this._matrixArray.Length;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002978 File Offset: 0x00000B78
		public void SetMatrix(int index, Matrix4x4 matrix)
		{
			this._matrixArray[index] = matrix;
			this.minModifiedIndex = Mathf.Min(index, this.minModifiedIndex);
			this.maxModifiedIndex = Mathf.Max(index, this.maxModifiedIndex);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000029AB File Offset: 0x00000BAB
		public void SetMatrixModified(int index)
		{
			this.minModifiedIndex = Mathf.Min(index, this.minModifiedIndex);
			this.maxModifiedIndex = Mathf.Max(index, this.maxModifiedIndex);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000029D1 File Offset: 0x00000BD1
		public void SetAllMatricesModified()
		{
			this.isMatrixArrayModified = true;
			this.minModifiedIndex = 0;
			this.maxModifiedIndex = int.MaxValue;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000029EC File Offset: 0x00000BEC
		public Matrix4x4 GetMatrix(int index)
		{
			return this._matrixArray[index];
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000029FA File Offset: 0x00000BFA
		public int GetInstanceCount()
		{
			return this.instanceCount;
		}

		// Token: 0x0400001D RID: 29
		[SerializeField]
		public bool isAutoUpdateTransformData;

		// Token: 0x0400001E RID: 30
		[SerializeField]
		public GPUIPrefabPrototypeData.GPUIPrefabInstances registeredInstances;

		// Token: 0x0400001F RID: 31
		[SerializeField]
		public bool isAutoEnableDisablePrototype;

		// Token: 0x04000020 RID: 32
		[SerializeField]
		public Vector2Int autoEnableDisableMinMaxCounts = new Vector2Int(50, 100);

		// Token: 0x04000021 RID: 33
		[NonSerialized]
		private NativeArray<Matrix4x4> _matrixArray;

		// Token: 0x04000022 RID: 34
		[NonSerialized]
		public Transform[] instanceTransforms;

		// Token: 0x04000023 RID: 35
		[NonSerialized]
		public TransformAccessArray transformAccessArray;

		// Token: 0x04000024 RID: 36
		[NonSerialized]
		public bool isTransformReferencesModified;

		// Token: 0x04000025 RID: 37
		[NonSerialized]
		public int prefabID;

		// Token: 0x04000026 RID: 38
		[NonSerialized]
		public List<GPUIPrefab> instancesToAdd;

		// Token: 0x04000027 RID: 39
		[NonSerialized]
		public SortedSet<int> indexesToRemove;

		// Token: 0x04000028 RID: 40
		[NonSerialized]
		public bool isMatrixArrayModified;

		// Token: 0x04000029 RID: 41
		[NonSerialized]
		internal int instanceCount;

		// Token: 0x0400002A RID: 42
		[NonSerialized]
		internal int _optionalRendererCount;

		// Token: 0x0400002B RID: 43
		[NonSerialized]
		internal NativeArray<uint> optionalRendererStatusData;

		// Token: 0x0400002C RID: 44
		[NonSerialized]
		internal bool isOptionalRendererStatusModified;

		// Token: 0x0400002D RID: 45
		[NonSerialized]
		public NativeArray<int> isModifiedArray;

		// Token: 0x0400002E RID: 46
		[NonSerialized]
		internal int minModifiedIndex;

		// Token: 0x0400002F RID: 47
		[NonSerialized]
		internal int maxModifiedIndex;

		// Token: 0x04000030 RID: 48
		[NonSerialized]
		internal GPUIAutoUpdateTransformsJob autoUpdateTransformsJob;

		// Token: 0x04000031 RID: 49
		[NonSerialized]
		internal bool _isAutoUpdateTransformJobsStarted;

		// Token: 0x04000032 RID: 50
		[NonSerialized]
		internal int _lastEnabledStatusChangeFrame;

		// Token: 0x0200000B RID: 11
		[Serializable]
		public class GPUIPrefabInstances
		{
			// Token: 0x04000033 RID: 51
			[SerializeField]
			public GameObject[] prefabInstances;

			// Token: 0x04000034 RID: 52
			[NonSerialized]
			public List<GPUIPrefab> runtimeCachedInstances = new List<GPUIPrefab>();
		}

		// Token: 0x0200000C RID: 12
		private class IntInverseComparer : IComparer<int>
		{
			// Token: 0x06000029 RID: 41 RVA: 0x00002A2C File Offset: 0x00000C2C
			public int Compare(int x, int y)
			{
				return y.CompareTo(x);
			}
		}
	}
}
