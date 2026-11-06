using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000065 RID: 101
	public static class GPUITransformBufferUtility
	{
		// Token: 0x060003C4 RID: 964 RVA: 0x00014C54 File Offset: 0x00012E54
		public static void CullInstancesInsideBounds(Bounds bounds, float offset = 0f)
		{
			foreach (GPUIRenderSourceGroup gpuirenderSourceGroup in GPUIRenderingSystem.Instance.RenderSourceGroupProvider.Values)
			{
				if (gpuirenderSourceGroup.TransformBufferData != null)
				{
					GPUITransformBufferUtility.CullInstancesInsideBounds(gpuirenderSourceGroup.TransformBufferData, 0, gpuirenderSourceGroup.BufferSize, bounds, offset);
				}
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00014CC8 File Offset: 0x00012EC8
		public static void CullInstancesInsideBounds(GPUIManager gpuiManager, Bounds bounds, List<int> prototypeIndexFilter = null, float offset = 0f)
		{
			int prototypeCount = gpuiManager.GetPrototypeCount();
			for (int i = 0; i < prototypeCount; i++)
			{
				if (prototypeIndexFilter == null || prototypeIndexFilter.Contains(i))
				{
					GPUITransformBufferUtility.CullInstancesInsideBounds(gpuiManager.GetRenderKey(i), bounds, offset);
				}
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00014D04 File Offset: 0x00012F04
		public static void CullInstancesInsideBounds(int renderKey, Bounds bounds, float offset = 0f)
		{
			GPUITransformBufferData gpuitransformBufferData;
			int num;
			int num2;
			if (GPUIRenderingSystem.TryGetTransformBufferData(renderKey, out gpuitransformBufferData, out num, out num2, false))
			{
				GPUITransformBufferUtility.CullInstancesInsideBounds(gpuitransformBufferData, num, num2, bounds, offset);
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00014D2C File Offset: 0x00012F2C
		public static void CullInstancesInsideBounds(GPUITransformBufferData transformBufferData, int bufferStartIndex, int bufferSize, Bounds bounds, float offset = 0f)
		{
			if (bufferSize == 0)
			{
				return;
			}
			Dictionary<int, GPUIShaderBuffer>.ValueCollection transformBufferValues = transformBufferData.TransformBufferValues;
			if (transformBufferValues == null)
			{
				return;
			}
			Vector3 center = bounds.center;
			Vector3 vector = bounds.extents + Vector3.one * offset;
			ComputeShader cs_TransformModifications = GPUIConstants.CS_TransformModifications;
			int num = 2;
			foreach (GPUIShaderBuffer gpuishaderBuffer in transformBufferValues)
			{
				if (gpuishaderBuffer != null && gpuishaderBuffer.Buffer != null)
				{
					cs_TransformModifications.SetBuffer(num, GPUIConstants.PROP_gpuiTransformBuffer, gpuishaderBuffer.Buffer);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_startIndex, bufferStartIndex);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_bufferSize, bufferSize);
					cs_TransformModifications.SetVector(GPUIConstants.PROP_boundsCenter, center);
					cs_TransformModifications.SetVector(GPUIConstants.PROP_boundsExtents, vector);
					cs_TransformModifications.DispatchX(num, bufferSize);
				}
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00014E14 File Offset: 0x00013014
		public static void CullInstancesInsideCollider(Collider collider, float offset = 0f)
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			foreach (GPUIRenderSourceGroup gpuirenderSourceGroup in GPUIRenderingSystem.Instance.RenderSourceGroupProvider.Values)
			{
				if (gpuirenderSourceGroup.TransformBufferData != null)
				{
					GPUITransformBufferUtility.CullInstancesInsideCollider(gpuirenderSourceGroup.TransformBufferData, 0, gpuirenderSourceGroup.BufferSize, collider, offset);
				}
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00014E90 File Offset: 0x00013090
		public static void CullInstancesInsideCollider(GPUIManager gpuiManager, Collider collider, List<int> prototypeIndexFilter = null, float offset = 0f)
		{
			int prototypeCount = gpuiManager.GetPrototypeCount();
			for (int i = 0; i < prototypeCount; i++)
			{
				if (prototypeIndexFilter == null || prototypeIndexFilter.Contains(i))
				{
					GPUITransformBufferUtility.CullInstancesInsideCollider(gpuiManager.GetRenderKey(i), collider, offset);
				}
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00014ECC File Offset: 0x000130CC
		public static void CullInstancesInsideCollider(int renderKey, Collider collider, float offset = 0f)
		{
			GPUITransformBufferData gpuitransformBufferData;
			int num;
			int num2;
			if (GPUIRenderingSystem.TryGetTransformBufferData(renderKey, out gpuitransformBufferData, out num, out num2, false))
			{
				GPUITransformBufferUtility.CullInstancesInsideCollider(gpuitransformBufferData, num, num2, collider, offset);
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00014EF4 File Offset: 0x000130F4
		public static void CullInstancesInsideCollider(GPUITransformBufferData transformBufferData, int bufferStartIndex, int bufferSize, Collider collider, float offset = 0f)
		{
			if (collider == null || bufferSize == 0)
			{
				return;
			}
			BoxCollider boxCollider = collider as BoxCollider;
			if (boxCollider != null)
			{
				GPUITransformBufferUtility.CullInstancesInsideBoxCollider(transformBufferData, bufferStartIndex, bufferSize, boxCollider, offset);
				return;
			}
			SphereCollider sphereCollider = collider as SphereCollider;
			if (sphereCollider != null)
			{
				GPUITransformBufferUtility.CullInstancesInsideSphereCollider(transformBufferData, bufferStartIndex, bufferSize, sphereCollider, offset);
				return;
			}
			CapsuleCollider capsuleCollider = collider as CapsuleCollider;
			if (capsuleCollider != null)
			{
				GPUITransformBufferUtility.CullInstancesInsideCapsuleCollider(transformBufferData, bufferStartIndex, bufferSize, capsuleCollider, offset);
				return;
			}
			GPUITransformBufferUtility.CullInstancesInsideBounds(transformBufferData, bufferStartIndex, bufferSize, collider.bounds, offset);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00014F60 File Offset: 0x00013160
		public static void CullInstancesInsideBoxCollider(GPUIManager gpuiManager, BoxCollider boxCollider, List<int> prototypeIndexFilter = null, float offset = 0f)
		{
			int prototypeCount = gpuiManager.GetPrototypeCount();
			for (int i = 0; i < prototypeCount; i++)
			{
				if (prototypeIndexFilter == null || prototypeIndexFilter.Contains(i))
				{
					GPUITransformBufferUtility.CullInstancesInsideBoxCollider(gpuiManager.GetRenderKey(i), boxCollider, offset);
				}
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00014F9C File Offset: 0x0001319C
		public static void CullInstancesInsideBoxCollider(int renderKey, BoxCollider boxCollider, float offset = 0f)
		{
			GPUITransformBufferData gpuitransformBufferData;
			int num;
			int num2;
			if (GPUIRenderingSystem.TryGetTransformBufferData(renderKey, out gpuitransformBufferData, out num, out num2, false))
			{
				GPUITransformBufferUtility.CullInstancesInsideBoxCollider(gpuitransformBufferData, num, num2, boxCollider, offset);
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00014FC4 File Offset: 0x000131C4
		public static void CullInstancesInsideBoxCollider(GPUITransformBufferData transformBufferData, int bufferStartIndex, int bufferSize, BoxCollider boxCollider, float offset = 0f)
		{
			if (bufferSize == 0)
			{
				return;
			}
			Dictionary<int, GPUIShaderBuffer>.ValueCollection transformBufferValues = transformBufferData.TransformBufferValues;
			if (transformBufferValues == null)
			{
				return;
			}
			Vector3 center = boxCollider.center;
			Vector3 vector = boxCollider.size / 2f + Vector3.one * offset;
			Matrix4x4 localToWorldMatrix = boxCollider.transform.localToWorldMatrix;
			ComputeShader cs_TransformModifications = GPUIConstants.CS_TransformModifications;
			int num = 3;
			foreach (GPUIShaderBuffer gpuishaderBuffer in transformBufferValues)
			{
				if (gpuishaderBuffer != null && gpuishaderBuffer.Buffer != null)
				{
					cs_TransformModifications.SetBuffer(num, GPUIConstants.PROP_gpuiTransformBuffer, gpuishaderBuffer.Buffer);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_startIndex, bufferStartIndex);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_bufferSize, bufferSize);
					cs_TransformModifications.SetVector(GPUIConstants.PROP_boundsCenter, center);
					cs_TransformModifications.SetVector(GPUIConstants.PROP_boundsExtents, vector);
					cs_TransformModifications.SetMatrix(GPUIConstants.PROP_modifierTransform, localToWorldMatrix);
					cs_TransformModifications.DispatchX(num, bufferSize);
				}
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000150D8 File Offset: 0x000132D8
		public static void CullInstancesInsideSphereCollider(GPUIManager gpuiManager, SphereCollider sphereCollider, List<int> prototypeIndexFilter = null, float offset = 0f)
		{
			int prototypeCount = gpuiManager.GetPrototypeCount();
			for (int i = 0; i < prototypeCount; i++)
			{
				if (prototypeIndexFilter == null || prototypeIndexFilter.Contains(i))
				{
					GPUITransformBufferUtility.CullInstancesInsideSphereCollider(gpuiManager.GetRenderKey(i), sphereCollider, offset);
				}
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00015114 File Offset: 0x00013314
		public static void CullInstancesInsideSphereCollider(int renderKey, SphereCollider sphereCollider, float offset = 0f)
		{
			GPUITransformBufferData gpuitransformBufferData;
			int num;
			int num2;
			if (GPUIRenderingSystem.TryGetTransformBufferData(renderKey, out gpuitransformBufferData, out num, out num2, false))
			{
				GPUITransformBufferUtility.CullInstancesInsideSphereCollider(gpuitransformBufferData, num, num2, sphereCollider, offset);
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001513C File Offset: 0x0001333C
		public static void CullInstancesInsideSphereCollider(GPUITransformBufferData transformBufferData, int bufferStartIndex, int bufferSize, SphereCollider sphereCollider, float offset = 0f)
		{
			if (bufferSize == 0)
			{
				return;
			}
			Dictionary<int, GPUIShaderBuffer>.ValueCollection transformBufferValues = transformBufferData.TransformBufferValues;
			if (transformBufferValues == null)
			{
				return;
			}
			Vector3 vector = sphereCollider.center + sphereCollider.transform.position;
			Vector3 localScale = sphereCollider.transform.localScale;
			float num = sphereCollider.radius * Mathf.Max(Mathf.Max(localScale.x, localScale.y), localScale.z) + offset;
			ComputeShader cs_TransformModifications = GPUIConstants.CS_TransformModifications;
			int num2 = 4;
			foreach (GPUIShaderBuffer gpuishaderBuffer in transformBufferValues)
			{
				if (gpuishaderBuffer != null && gpuishaderBuffer.Buffer != null)
				{
					cs_TransformModifications.SetBuffer(num2, GPUIConstants.PROP_gpuiTransformBuffer, gpuishaderBuffer.Buffer);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_startIndex, bufferStartIndex);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_bufferSize, bufferSize);
					cs_TransformModifications.SetVector(GPUIConstants.PROP_boundsCenter, vector);
					cs_TransformModifications.SetFloat(GPUIConstants.PROP_modifierRadius, num);
					cs_TransformModifications.DispatchX(num2, bufferSize);
				}
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00015250 File Offset: 0x00013450
		public static void CullInstancesInsideCapsuleCollider(GPUIManager gpuiManager, CapsuleCollider capsuleCollider, List<int> prototypeIndexFilter = null, float offset = 0f)
		{
			int prototypeCount = gpuiManager.GetPrototypeCount();
			for (int i = 0; i < prototypeCount; i++)
			{
				if (prototypeIndexFilter == null || prototypeIndexFilter.Contains(i))
				{
					GPUITransformBufferUtility.CullInstancesInsideCapsuleCollider(gpuiManager.GetRenderKey(i), capsuleCollider, offset);
				}
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001528C File Offset: 0x0001348C
		public static void CullInstancesInsideCapsuleCollider(int renderKey, CapsuleCollider capsuleCollider, float offset = 0f)
		{
			GPUITransformBufferData gpuitransformBufferData;
			int num;
			int num2;
			if (GPUIRenderingSystem.TryGetTransformBufferData(renderKey, out gpuitransformBufferData, out num, out num2, false))
			{
				GPUITransformBufferUtility.CullInstancesInsideCapsuleCollider(gpuitransformBufferData, num, num2, capsuleCollider, offset);
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000152B4 File Offset: 0x000134B4
		public static void CullInstancesInsideCapsuleCollider(GPUITransformBufferData transformBufferData, int bufferStartIndex, int bufferSize, CapsuleCollider capsuleCollider, float offset = 0f)
		{
			if (bufferSize == 0)
			{
				return;
			}
			Dictionary<int, GPUIShaderBuffer>.ValueCollection transformBufferValues = transformBufferData.TransformBufferValues;
			if (transformBufferValues == null)
			{
				return;
			}
			Vector3 center = capsuleCollider.center;
			Vector3 localScale = capsuleCollider.transform.localScale;
			float num = capsuleCollider.radius * Mathf.Max(Mathf.Max((capsuleCollider.direction == 0) ? 0f : localScale.x, (capsuleCollider.direction == 1) ? 0f : localScale.y), (capsuleCollider.direction == 2) ? 0f : localScale.z) + offset;
			float num2 = capsuleCollider.height * ((capsuleCollider.direction == 0) ? localScale.x : ((capsuleCollider.direction == 1) ? localScale.y : ((capsuleCollider.direction == 2) ? localScale.z : 0f)));
			ComputeShader cs_TransformModifications = GPUIConstants.CS_TransformModifications;
			int num3 = 5;
			foreach (GPUIShaderBuffer gpuishaderBuffer in transformBufferValues)
			{
				if (gpuishaderBuffer != null && gpuishaderBuffer.Buffer != null)
				{
					cs_TransformModifications.SetBuffer(num3, GPUIConstants.PROP_gpuiTransformBuffer, gpuishaderBuffer.Buffer);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_startIndex, bufferStartIndex);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_bufferSize, bufferSize);
					cs_TransformModifications.SetVector(GPUIConstants.PROP_boundsCenter, center);
					cs_TransformModifications.SetFloat(GPUIConstants.PROP_modifierRadius, num);
					cs_TransformModifications.SetFloat(GPUIConstants.PROP_modifierHeight, num2);
					cs_TransformModifications.DispatchX(num3, bufferSize);
				}
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00015438 File Offset: 0x00013638
		public static void ResetAllCulledInstances()
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			foreach (GPUIRenderSourceGroup gpuirenderSourceGroup in GPUIRenderingSystem.Instance.RenderSourceGroupProvider.Values)
			{
				if (gpuirenderSourceGroup.TransformBufferData != null)
				{
					GPUITransformBufferUtility.ResetCulledInstances(gpuirenderSourceGroup.TransformBufferData, 0, gpuirenderSourceGroup.BufferSize);
				}
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000154B0 File Offset: 0x000136B0
		public static void ResetCulledInstances(GPUIManager gpuiManager, List<int> prototypeIndexFilter = null)
		{
			int prototypeCount = gpuiManager.GetPrototypeCount();
			for (int i = 0; i < prototypeCount; i++)
			{
				if (prototypeIndexFilter == null || prototypeIndexFilter.Contains(i))
				{
					GPUITransformBufferUtility.ResetCulledInstances(gpuiManager.GetRenderKey(i));
				}
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000154E8 File Offset: 0x000136E8
		public static void ResetCulledInstances(int renderKey)
		{
			GPUITransformBufferData gpuitransformBufferData;
			int num;
			int num2;
			if (GPUIRenderingSystem.TryGetTransformBufferData(renderKey, out gpuitransformBufferData, out num, out num2, false))
			{
				GPUITransformBufferUtility.ResetCulledInstances(gpuitransformBufferData, num, num2);
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001550C File Offset: 0x0001370C
		public static void ResetCulledInstances(GPUITransformBufferData transformBufferData, int bufferStartIndex, int bufferSize)
		{
			if (bufferSize == 0)
			{
				return;
			}
			Dictionary<int, GPUIShaderBuffer>.ValueCollection transformBufferValues = transformBufferData.TransformBufferValues;
			if (transformBufferValues == null)
			{
				return;
			}
			ComputeShader cs_TransformModifications = GPUIConstants.CS_TransformModifications;
			int num = 9;
			foreach (GPUIShaderBuffer gpuishaderBuffer in transformBufferValues)
			{
				if (gpuishaderBuffer != null && gpuishaderBuffer.Buffer != null)
				{
					cs_TransformModifications.SetBuffer(num, GPUIConstants.PROP_gpuiTransformBuffer, gpuishaderBuffer.Buffer);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_startIndex, bufferStartIndex);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_bufferSize, bufferSize);
					cs_TransformModifications.DispatchX(num, bufferSize);
				}
			}
			transformBufferData.OnTransformDataModified();
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000155B4 File Offset: 0x000137B4
		public static void ApplyMatrixOffsetToTransforms(GPUIManager gpuiManager, Matrix4x4 matrixOffset)
		{
			int prototypeCount = gpuiManager.GetPrototypeCount();
			for (int i = 0; i < prototypeCount; i++)
			{
				GPUITransformBufferUtility.ApplyMatrixOffsetToTransforms(gpuiManager.GetRenderKey(i), matrixOffset);
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000155E4 File Offset: 0x000137E4
		public static void ApplyMatrixOffsetToTransforms(int renderKey, Matrix4x4 matrixOffset)
		{
			GPUITransformBufferData gpuitransformBufferData;
			int num;
			int num2;
			if (GPUIRenderingSystem.TryGetTransformBufferData(renderKey, out gpuitransformBufferData, out num, out num2, false))
			{
				GPUITransformBufferUtility.ApplyMatrixOffsetToTransforms(gpuitransformBufferData, num, num2, matrixOffset);
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001560C File Offset: 0x0001380C
		public static void ApplyMatrixOffsetToTransforms(GPUITransformBufferData transformBufferData, int bufferStartIndex, int bufferSize, Matrix4x4 matrixOffset)
		{
			if (bufferSize == 0)
			{
				return;
			}
			Dictionary<int, GPUIShaderBuffer>.ValueCollection transformBufferValues = transformBufferData.TransformBufferValues;
			if (transformBufferValues == null)
			{
				return;
			}
			ComputeShader cs_TransformModifications = GPUIConstants.CS_TransformModifications;
			foreach (GPUIShaderBuffer gpuishaderBuffer in transformBufferValues)
			{
				if (gpuishaderBuffer != null && gpuishaderBuffer.Buffer != null)
				{
					cs_TransformModifications.SetBuffer(1, GPUIConstants.PROP_gpuiTransformBuffer, gpuishaderBuffer.Buffer);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_startIndex, bufferStartIndex);
					cs_TransformModifications.SetInt(GPUIConstants.PROP_bufferSize, bufferSize);
					cs_TransformModifications.SetMatrix(GPUIConstants.PROP_matrix44, matrixOffset);
					cs_TransformModifications.DispatchX(1, bufferSize);
				}
			}
			transformBufferData.OnTransformDataModified();
		}
	}
}
