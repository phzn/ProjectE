using System;
using System.Collections.Generic;
using GPUInstancerPro;
using GPUInstancerPro.PrefabModule;
using GPUInstancerPro.TerrainModule;
using Unity.Collections;
using UnityEngine;

namespace Features.RenderingPrecision
{
	// Token: 0x02000D83 RID: 3459
	public static class WorldOriginInstancing
	{
		// Token: 0x060065DC RID: 26076 RVA: 0x0028A6D8 File Offset: 0x002888D8
		public static void PrepareForShift()
		{
			if (WorldOriginInstancing._pendingHistoryOwner != null)
			{
				GPUIRenderingSystem pendingHistoryOwner = WorldOriginInstancing._pendingHistoryOwner;
				pendingHistoryOwner.OnPreRender = (Action<GPUICameraData>)Delegate.Remove(pendingHistoryOwner.OnPreRender, new Action<GPUICameraData>(WorldOriginInstancing.ResetHistoryBeforeDraw));
			}
			WorldOriginInstancing._pendingHistoryOwner = null;
			WorldOriginInstancing.Managers.Clear();
			WorldOriginInstancing.Buffers.Clear();
			WorldOriginInstancing.RenderKeys.Clear();
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			foreach (GPUIManager gpuimanager in GPUIRenderingSystem.Instance.ActiveGPUIManagers)
			{
				if (gpuimanager.IsInitialized)
				{
					WorldOriginInstancing.Managers.Add(gpuimanager);
					for (int i = 0; i < gpuimanager.GetPrototypeCount(); i++)
					{
						GPUIPrefabManager gpuiprefabManager = gpuimanager as GPUIPrefabManager;
						if (gpuiprefabManager != null)
						{
							gpuiprefabManager.GetTransformMatrix(gpuiprefabManager.GetPrefabID(i));
						}
						GPUITransformBufferData gpuitransformBufferData;
						int num;
						int num2;
						if (GPUIRenderingSystem.TryGetTransformBufferData(gpuimanager.GetRenderKey(i), out gpuitransformBufferData, out num, out num2, false) && WorldOriginInstancing.Buffers.Add(gpuitransformBufferData) && gpuitransformBufferData.TransformBufferValues != null)
						{
							foreach (GPUIShaderBuffer gpuishaderBuffer in gpuitransformBufferData.TransformBufferValues)
							{
								if (gpuishaderBuffer != null)
								{
									gpuishaderBuffer.CompleteAsyncRequests();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060065DD RID: 26077 RVA: 0x0028A848 File Offset: 0x00288A48
		public static void ApplyShift(Vector3 delta)
		{
			Matrix4x4 matrix4x = Matrix4x4.Translate(delta);
			foreach (GPUIManager gpuimanager in WorldOriginInstancing.Managers)
			{
				if (!(gpuimanager == null) && gpuimanager.IsInitialized)
				{
					GPUIPrefabManager gpuiprefabManager = gpuimanager as GPUIPrefabManager;
					if (gpuiprefabManager != null)
					{
						for (int i = 0; i < gpuiprefabManager.GetPrototypeCount(); i++)
						{
							int prefabID = gpuiprefabManager.GetPrefabID(i);
							NativeArray<Matrix4x4> transformMatrix = gpuiprefabManager.GetTransformMatrix(prefabID);
							if (transformMatrix.IsCreated)
							{
								for (int j = 0; j < transformMatrix.Length; j++)
								{
									Matrix4x4 matrix4x2 = transformMatrix[j];
									if (matrix4x2.m33 != 0f)
									{
										matrix4x2.m03 += delta.x;
										matrix4x2.m13 += delta.y;
										matrix4x2.m23 += delta.z;
										transformMatrix[j] = matrix4x2;
									}
								}
								gpuiprefabManager.SetTransformMatrixModified(prefabID);
							}
						}
					}
					GPUITreeManager gpuitreeManager = gpuimanager as GPUITreeManager;
					if (gpuitreeManager != null)
					{
						using (Dictionary<int, GPUITerrain>.ValueCollection.Enumerator enumerator2 = gpuitreeManager.GetActiveTerrainValues().GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								GPUITerrain gpuiterrain = enumerator2.Current;
								if (gpuiterrain != null)
								{
									gpuiterrain.NotifyTransformChanges();
								}
							}
							continue;
						}
					}
					GPUIDetailManager gpuidetailManager = gpuimanager as GPUIDetailManager;
					if (gpuidetailManager != null)
					{
						using (Dictionary<int, GPUITerrain>.ValueCollection.Enumerator enumerator2 = gpuidetailManager.GetActiveTerrainValues().GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								GPUITerrain gpuiterrain2 = enumerator2.Current;
								if (gpuiterrain2 != null)
								{
									gpuiterrain2.NotifyTransformChanges();
								}
							}
							continue;
						}
					}
					for (int k = 0; k < gpuimanager.GetPrototypeCount(); k++)
					{
						int renderKey = gpuimanager.GetRenderKey(k);
						if (WorldOriginInstancing.RenderKeys.Add(renderKey))
						{
							GPUITransformBufferUtility.ApplyMatrixOffsetToTransforms(renderKey, matrix4x);
						}
					}
				}
			}
			Delegate[] array;
			if (!GPUIRenderingSystem.IsActive)
			{
				array = null;
			}
			else
			{
				Action<GPUICameraData> onPreCull = GPUIRenderingSystem.Instance.OnPreCull;
				array = ((onPreCull != null) ? onPreCull.GetInvocationList() : null);
			}
			Delegate[] array2 = array;
			if (array2 != null)
			{
				Delegate[] array3 = array2;
				for (int l = 0; l < array3.Length; l++)
				{
					GPUIEventFloatingOrigin gpuieventFloatingOrigin = array3[l].Target as GPUIEventFloatingOrigin;
					if (gpuieventFloatingOrigin != null && gpuieventFloatingOrigin.isActiveAndEnabled && !(gpuieventFloatingOrigin.floatingOrigin == null) && !(gpuieventFloatingOrigin.manager == null))
					{
						gpuieventFloatingOrigin.enabled = false;
						gpuieventFloatingOrigin.enabled = true;
					}
				}
			}
			foreach (GPUITransformBufferData gpuitransformBufferData in WorldOriginInstancing.Buffers)
			{
				gpuitransformBufferData.OnTransformDataModified();
				gpuitransformBufferData.RequireInstancingBoundsUpdate();
			}
			if (WorldOriginInstancing._pendingHistoryOwner != null)
			{
				GPUIRenderingSystem pendingHistoryOwner = WorldOriginInstancing._pendingHistoryOwner;
				pendingHistoryOwner.OnPreRender = (Action<GPUICameraData>)Delegate.Remove(pendingHistoryOwner.OnPreRender, new Action<GPUICameraData>(WorldOriginInstancing.ResetHistoryBeforeDraw));
			}
			WorldOriginInstancing._pendingHistoryOwner = (GPUIRenderingSystem.IsActive ? GPUIRenderingSystem.Instance : null);
			if (WorldOriginInstancing._pendingHistoryOwner != null)
			{
				GPUIRenderingSystem pendingHistoryOwner2 = WorldOriginInstancing._pendingHistoryOwner;
				pendingHistoryOwner2.OnPreRender = (Action<GPUICameraData>)Delegate.Combine(pendingHistoryOwner2.OnPreRender, new Action<GPUICameraData>(WorldOriginInstancing.ResetHistoryBeforeDraw));
				return;
			}
			WorldOriginInstancing.Clear();
		}

		// Token: 0x060065DE RID: 26078 RVA: 0x0028ABE0 File Offset: 0x00288DE0
		private static void ResetHistoryBeforeDraw(GPUICameraData camera)
		{
			foreach (GPUITransformBufferData gpuitransformBufferData in WorldOriginInstancing.Buffers)
			{
				if (gpuitransformBufferData.TransformBufferValues != null)
				{
					gpuitransformBufferData.ResetPreviousFrameBuffer();
				}
			}
			if (WorldOriginInstancing._pendingHistoryOwner != null)
			{
				GPUIRenderingSystem pendingHistoryOwner = WorldOriginInstancing._pendingHistoryOwner;
				pendingHistoryOwner.OnPreRender = (Action<GPUICameraData>)Delegate.Remove(pendingHistoryOwner.OnPreRender, new Action<GPUICameraData>(WorldOriginInstancing.ResetHistoryBeforeDraw));
			}
			WorldOriginInstancing._pendingHistoryOwner = null;
			WorldOriginInstancing.Clear();
		}

		// Token: 0x060065DF RID: 26079 RVA: 0x0028AC78 File Offset: 0x00288E78
		private static void Clear()
		{
			WorldOriginInstancing.Managers.Clear();
			WorldOriginInstancing.Buffers.Clear();
			WorldOriginInstancing.RenderKeys.Clear();
		}

		// Token: 0x060065E0 RID: 26080 RVA: 0x0028AC98 File Offset: 0x00288E98
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			if (WorldOriginInstancing._pendingHistoryOwner != null)
			{
				GPUIRenderingSystem pendingHistoryOwner = WorldOriginInstancing._pendingHistoryOwner;
				pendingHistoryOwner.OnPreRender = (Action<GPUICameraData>)Delegate.Remove(pendingHistoryOwner.OnPreRender, new Action<GPUICameraData>(WorldOriginInstancing.ResetHistoryBeforeDraw));
			}
			WorldOriginInstancing._pendingHistoryOwner = null;
			WorldOriginInstancing.Clear();
		}

		// Token: 0x04006866 RID: 26726
		private static readonly List<GPUIManager> Managers = new List<GPUIManager>();

		// Token: 0x04006867 RID: 26727
		private static readonly HashSet<GPUITransformBufferData> Buffers = new HashSet<GPUITransformBufferData>();

		// Token: 0x04006868 RID: 26728
		private static readonly HashSet<int> RenderKeys = new HashSet<int>();

		// Token: 0x04006869 RID: 26729
		private static GPUIRenderingSystem _pendingHistoryOwner;
	}
}
