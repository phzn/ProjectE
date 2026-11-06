using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000062 RID: 98
	public static class GPUICoreAPI
	{
		// Token: 0x06000396 RID: 918 RVA: 0x000143EC File Offset: 0x000125EC
		public static void InitializeRenderingSystem()
		{
			GPUIRenderingSystem.InitializeRenderingSystem();
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000143F3 File Offset: 0x000125F3
		public static void RegenerateRenderers()
		{
			GPUIRenderingSystem.RegenerateRenderers();
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000143FA File Offset: 0x000125FA
		public static void UpdateParameterBufferData()
		{
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem.Instance.UpdateParameterBufferData();
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001440D File Offset: 0x0001260D
		public static void DisposeAll()
		{
			GPUIRenderingSystem.Instance.DestroyGeneric();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00014419 File Offset: 0x00012619
		public static void ResetMaterials()
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			GPUIRenderingSystem.Instance.MaterialProvider.Reset();
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00014432 File Offset: 0x00012632
		public static bool RegisterRenderer(Object source, GameObject prefab, out int rendererKey)
		{
			return GPUIRenderingSystem.RegisterRenderer(source, prefab, out rendererKey, 0, GPUITransformBufferType.Default, null);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0001443F File Offset: 0x0001263F
		public static bool RegisterRenderer(Object source, GameObject prefab, GPUIProfile profile, out int rendererKey)
		{
			return GPUIRenderingSystem.RegisterRenderer(source, prefab, profile, out rendererKey, 0, GPUITransformBufferType.Default, null);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001444D File Offset: 0x0001264D
		public static bool RegisterRenderer(Object source, GPUIPrototype prototype, out int rendererKey)
		{
			return GPUIRenderingSystem.RegisterRenderer(source, prototype, out rendererKey, 0, GPUITransformBufferType.Default, null);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001445A File Offset: 0x0001265A
		public static void DisposeRenderer(int rendererKey)
		{
			GPUIRenderingSystem.DisposeRenderer(rendererKey);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00014462 File Offset: 0x00012662
		public static bool SetTransformBufferData(int rendererKey, NativeArray<Matrix4x4> matrices, int managedBufferStartIndex = 0, int graphicsBufferStartIndex = 0, int count = 0, bool isOverwritePreviousFrameBuffer = true)
		{
			return GPUIRenderingSystem.SetTransformBufferData<Matrix4x4>(rendererKey, matrices, managedBufferStartIndex, graphicsBufferStartIndex, (count > 0) ? count : matrices.Length, isOverwritePreviousFrameBuffer);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0001447F File Offset: 0x0001267F
		public static bool SetTransformBufferData(int rendererKey, Matrix4x4[] matrices, int managedBufferStartIndex = 0, int graphicsBufferStartIndex = 0, int count = 0, bool isOverwritePreviousFrameBuffer = true)
		{
			return GPUIRenderingSystem.SetTransformBufferData<Matrix4x4>(rendererKey, matrices, managedBufferStartIndex, graphicsBufferStartIndex, (count > 0) ? count : matrices.Length, isOverwritePreviousFrameBuffer);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00014498 File Offset: 0x00012698
		public static bool SetTransformBufferData(int rendererKey, List<Matrix4x4> matrices, int managedBufferStartIndex = 0, int graphicsBufferStartIndex = 0, int count = 0, bool isOverwritePreviousFrameBuffer = true)
		{
			return GPUIRenderingSystem.SetTransformBufferData<Matrix4x4>(rendererKey, matrices, managedBufferStartIndex, graphicsBufferStartIndex, (count > 0) ? count : matrices.Count, isOverwritePreviousFrameBuffer);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x000144B4 File Offset: 0x000126B4
		public static bool TryGetTransformBuffer(int rendererKey, out GraphicsBuffer transformBuffer, out int bufferStartIndex)
		{
			int num;
			return GPUICoreAPI.TryGetTransformBuffer(rendererKey, out transformBuffer, out bufferStartIndex, out num);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x000144CC File Offset: 0x000126CC
		public static bool TryGetTransformBuffer(int rendererKey, out GraphicsBuffer transformBuffer, out int bufferStartIndex, out int bufferSize)
		{
			transformBuffer = null;
			GPUIShaderBuffer gpuishaderBuffer;
			bool flag = GPUIRenderingSystem.TryGetTransformBuffer(rendererKey, out gpuishaderBuffer, out bufferStartIndex, out bufferSize, null, false);
			if (flag)
			{
				transformBuffer = gpuishaderBuffer.Buffer;
			}
			return flag;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000144F3 File Offset: 0x000126F3
		public static bool TryGetTransformBufferData(int rendererKey, out GPUITransformBufferData transformBufferData, out int bufferStartIndex, out int bufferSize, bool resetCrossFade = false)
		{
			return GPUIRenderingSystem.TryGetTransformBufferData(rendererKey, out transformBufferData, out bufferStartIndex, out bufferSize, resetCrossFade);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00014500 File Offset: 0x00012700
		public static bool SetBufferSize(int rendererKey, int bufferSize)
		{
			return GPUIRenderingSystem.SetBufferSize(rendererKey, bufferSize, true);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001450A File Offset: 0x0001270A
		public static bool SetInstanceCount(int rendererKey, int instanceCount)
		{
			return GPUIRenderingSystem.SetInstanceCount(rendererKey, instanceCount);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00014513 File Offset: 0x00012713
		public static void AddMaterialPropertyOverride(int rendererKey, string propertyName, object propertyValue, int lodIndex = -1, int rendererIndex = -1)
		{
			GPUIRenderingSystem.AddMaterialPropertyOverride(rendererKey, propertyName, propertyValue, lodIndex, rendererIndex, false);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00014521 File Offset: 0x00012721
		public static void AddMaterialPropertyOverride(int rendererKey, int nameID, object propertyValue, int lodIndex = -1, int rendererIndex = -1)
		{
			GPUIRenderingSystem.AddMaterialPropertyOverride(rendererKey, nameID, propertyValue, lodIndex, rendererIndex, false);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001452F File Offset: 0x0001272F
		public static void RemoveMaterialPropertyOverrides(int rendererKey, string propertyName)
		{
			GPUIRenderingSystem.RemoveMaterialPropertyOverrides(rendererKey, propertyName);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00014538 File Offset: 0x00012738
		public static void RemoveMaterialPropertyOverrides(int rendererKey, int nameID)
		{
			GPUIRenderingSystem.RemoveMaterialPropertyOverrides(rendererKey, nameID);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00014541 File Offset: 0x00012741
		public static void ClearMaterialPropertyOverrides(int rendererKey)
		{
			GPUIRenderingSystem.ClearMaterialPropertyOverrides(rendererKey);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00014549 File Offset: 0x00012749
		public static void SetLODColorDebuggingEnabled(int rendererKey, bool enabled, string colorPropertyName = null)
		{
			GPUIRenderingSystem.SetLODColorDebuggingEnabled(rendererKey, enabled, colorPropertyName);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00014553 File Offset: 0x00012753
		public static int AddPrototype(GPUIManager gpuiManager, GPUIPrototype prototype)
		{
			return gpuiManager.AddPrototype(prototype);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001455C File Offset: 0x0001275C
		public static int AddPrototype(GPUIManager gpuiManager, GameObject prefab)
		{
			return gpuiManager.AddPrototype(prefab);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00014565 File Offset: 0x00012765
		public static int AddPrototype(GPUIManager gpuiManager, GameObject prefab, GPUIProfile profile)
		{
			return gpuiManager.AddPrototype(prefab, profile);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0001456F File Offset: 0x0001276F
		public static void AddCameraEventOnPreCull(Action<GPUICameraData> cameraEvent)
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem.InitializeRenderingSystem();
			}
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.OnPreCull = (Action<GPUICameraData>)Delegate.Combine(instance.OnPreCull, cameraEvent);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00014598 File Offset: 0x00012798
		public static void RemoveCameraEventOnPreCull(Action<GPUICameraData> cameraEvent)
		{
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
				instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, cameraEvent);
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x000145BC File Offset: 0x000127BC
		public static void AddCameraEventOnPreRender(Action<GPUICameraData> cameraEvent)
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem.InitializeRenderingSystem();
			}
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.OnPreRender = (Action<GPUICameraData>)Delegate.Combine(instance.OnPreRender, cameraEvent);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x000145E5 File Offset: 0x000127E5
		public static void RemoveCameraEventOnPreRender(Action<GPUICameraData> cameraEvent)
		{
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
				instance.OnPreRender = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreRender, cameraEvent);
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00014609 File Offset: 0x00012809
		public static void AddCameraEventOnPostRender(Action<GPUICameraData> cameraEvent)
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem.InitializeRenderingSystem();
			}
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.OnPostRender = (Action<GPUICameraData>)Delegate.Combine(instance.OnPostRender, cameraEvent);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00014632 File Offset: 0x00012832
		public static void RemoveCameraEventOnPostRender(Action<GPUICameraData> cameraEvent)
		{
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
				instance.OnPostRender = (Action<GPUICameraData>)Delegate.Remove(instance.OnPostRender, cameraEvent);
			}
		}
	}
}
