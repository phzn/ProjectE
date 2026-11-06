using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x0200001D RID: 29
	public static class GPUITerrainUtility
	{
		// Token: 0x06000168 RID: 360 RVA: 0x00009D14 File Offset: 0x00007F14
		public static RenderTexture CreateDetailRenderTexture(int resolution, string name)
		{
			RenderTexture renderTexture = new RenderTexture(resolution, resolution, 0, GPUITerrainConstants.R8_RenderTextureFormat, RenderTextureReadWrite.Linear);
			renderTexture.name = name;
			renderTexture.isPowerOfTwo = false;
			renderTexture.enableRandomWrite = true;
			renderTexture.filterMode = FilterMode.Point;
			renderTexture.useMipMap = false;
			renderTexture.autoGenerateMips = false;
			renderTexture.Create();
			renderTexture.ClearRenderTexture();
			return renderTexture;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00009D68 File Offset: 0x00007F68
		public static void CaptureTerrainDetailToRenderTexture(TerrainData terrainData, int detailLayer, RenderTexture renderTexture, bool sampleTerrainHoles)
		{
			int detailResolution = terrainData.detailResolution;
			int[,] detailLayer2 = terrainData.GetDetailLayer(0, 0, detailResolution, detailResolution, detailLayer);
			GPUITerrainUtility.CaptureTerrainDetailToRenderTexture(terrainData, detailResolution, detailLayer2, renderTexture, sampleTerrainHoles);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00009D92 File Offset: 0x00007F92
		public static void CaptureTerrainDetailToRenderTexture(TerrainData terrainData, int detailResolution, int[,] details, RenderTexture renderTexture, bool sampleTerrainHoles)
		{
			GPUITerrainUtility.CaptureTerrainDetailToRenderTexture(detailResolution, details, renderTexture, sampleTerrainHoles, terrainData.holesTexture);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00009DA4 File Offset: 0x00007FA4
		public static void CaptureTerrainDetailToRenderTexture(int detailResolution, int[,] details, RenderTexture renderTexture, bool sampleTerrainHoles, Texture holesTexture)
		{
			int num = detailResolution * detailResolution;
			GraphicsBuffer graphicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, num, 4);
			graphicsBuffer.SetData(details);
			ComputeShader cs_TerrainDetailCapture = GPUITerrainConstants.CS_TerrainDetailCapture;
			if (sampleTerrainHoles && holesTexture != null)
			{
				cs_TerrainDetailCapture.EnableKeyword(GPUITerrainConstants.Kw_GPUI_TERRAIN_HOLES);
				cs_TerrainDetailCapture.SetTexture(0, GPUITerrainConstants.PROP_terrainHoleTexture, holesTexture);
			}
			else
			{
				cs_TerrainDetailCapture.DisableKeyword(GPUITerrainConstants.Kw_GPUI_TERRAIN_HOLES);
			}
			cs_TerrainDetailCapture.SetTexture(0, GPUITerrainConstants.PROP_terrainDetailTexture, renderTexture);
			cs_TerrainDetailCapture.SetBuffer(0, GPUITerrainConstants.PROP_detailLayerBuffer, graphicsBuffer);
			cs_TerrainDetailCapture.SetInt(GPUITerrainConstants.PROP_detailResolution, detailResolution);
			cs_TerrainDetailCapture.DispatchX(0, num);
			graphicsBuffer.Dispose();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00009E34 File Offset: 0x00008034
		public static void CaptureTerrainDetailToRenderTextureWithComputeDetailInstanceTransforms(TerrainData terrainData, int detailLayer, float density, RenderTexture renderTexture, bool sampleTerrainHoles)
		{
			int detailPatchCount = terrainData.detailPatchCount;
			DetailInstanceTransform[] array = new DetailInstanceTransform[0];
			int num = 0;
			for (int i = 0; i < detailPatchCount; i++)
			{
				for (int j = 0; j < detailPatchCount; j++)
				{
					Bounds bounds;
					DetailInstanceTransform[] array2 = terrainData.ComputeDetailInstanceTransforms(j, i, detailLayer, density, out bounds);
					int num2 = array2.Length;
					if (num2 != 0)
					{
						Array.Resize<DetailInstanceTransform>(ref array, num + num2);
						Array.Copy(array2, 0, array, num, num2);
						num += num2;
					}
				}
			}
			if (num == 0)
			{
				return;
			}
			int detailResolution = terrainData.detailResolution;
			ComputeShader computeShader = GPUITerrainConstants.CS_TerrainDetailCaptureFromInstanceTransforms;
			GraphicsBuffer graphicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, detailResolution * detailResolution, 4);
			GraphicsBuffer graphicsBuffer2 = new GraphicsBuffer(GraphicsBuffer.Target.Structured, num, 24);
			graphicsBuffer2.SetData(array);
			computeShader.SetBuffer(1, GPUITerrainConstants.PROP_detailLayerBuffer, graphicsBuffer);
			computeShader.SetInt(GPUIConstants.PROP_bufferSize, graphicsBuffer.count);
			computeShader.DispatchX(1, graphicsBuffer.count);
			computeShader.SetBuffer(0, GPUITerrainConstants.PROP_detailLayerBuffer, graphicsBuffer);
			computeShader.SetBuffer(0, "detailInstanceTransformBuffer", graphicsBuffer2);
			computeShader.SetInt(GPUITerrainConstants.PROP_detailResolution, detailResolution);
			computeShader.SetVector(GPUITerrainConstants.PROP_terrainSize, terrainData.size);
			computeShader.SetInt(GPUIConstants.PROP_bufferSize, num);
			computeShader.DispatchX(0, num);
			graphicsBuffer2.Dispose();
			computeShader = GPUITerrainConstants.CS_TerrainDetailCapture;
			if (sampleTerrainHoles)
			{
				computeShader.EnableKeyword(GPUITerrainConstants.Kw_GPUI_TERRAIN_HOLES);
				computeShader.SetTexture(0, GPUITerrainConstants.PROP_terrainHoleTexture, terrainData.holesTexture);
			}
			else
			{
				computeShader.DisableKeyword(GPUITerrainConstants.Kw_GPUI_TERRAIN_HOLES);
			}
			computeShader.SetTexture(0, GPUITerrainConstants.PROP_terrainDetailTexture, renderTexture);
			computeShader.SetBuffer(0, GPUITerrainConstants.PROP_detailLayerBuffer, graphicsBuffer);
			computeShader.SetInt(GPUITerrainConstants.PROP_detailResolution, detailResolution);
			computeShader.DispatchX(0, detailResolution * detailResolution);
			graphicsBuffer.Dispose();
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00009FDE File Offset: 0x000081DE
		public static void UpdateTerrainDetailWithRenderTexture(Terrain terrain, int detailLayer, RenderTexture renderTexture)
		{
			GPUITerrainUtility.UpdateTerrainDetailWithRenderTexture(terrain.terrainData, detailLayer, renderTexture, 1f);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00009FF2 File Offset: 0x000081F2
		public static void UpdateTerrainDetailWithRenderTexture(TerrainData terrainData, int detailLayer, RenderTexture renderTexture, float densityMultiplier = 1f)
		{
			GPUITerrainUtility.UpdateTerrainDetailWithTexture2D(terrainData, detailLayer, GPUITextureUtility.RenderTextureToTexture2D(renderTexture, TextureFormat.R8, true, FilterMode.Point), densityMultiplier);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000A008 File Offset: 0x00008208
		public static void UpdateTerrainDetailWithTexture2D(TerrainData terrainData, int detailLayer, Texture2D detailTexture, float densityMultiplier = 1f)
		{
			if (detailTexture == null)
			{
				return;
			}
			int detailResolution = terrainData.detailResolution;
			int num = detailTexture.width;
			int num2 = detailTexture.height;
			Color[] array;
			if (detailTexture.isReadable)
			{
				array = detailTexture.GetPixels();
			}
			else
			{
				RenderTexture renderTexture = GPUITerrainUtility.CreateDetailRenderTexture(num, null);
				GPUITextureUtility.CopyTextureSamplerWithComputeShader(detailTexture, renderTexture);
				detailTexture = GPUITextureUtility.RenderTextureToTexture2D(renderTexture, TextureFormat.R8, true, FilterMode.Point);
				renderTexture.DestroyRenderTexture();
				num = detailTexture.width;
				num2 = detailTexture.height;
				array = detailTexture.GetPixels();
				detailTexture.DestroyGeneric();
			}
			int[,] array2 = new int[detailResolution, detailResolution];
			for (int i = 0; i < detailResolution; i++)
			{
				for (int j = 0; j < detailResolution; j++)
				{
					int num3 = Mathf.FloorToInt((float)j / (float)detailResolution * (float)num);
					int num4 = Mathf.FloorToInt((float)i / (float)detailResolution * (float)num2);
					num3 = Mathf.Clamp(num3, 0, num - 1);
					num4 = Mathf.Clamp(num4, 0, num2 - 1);
					float num5 = array[num4 * num + num3].r * 255f * densityMultiplier;
					array2[i, j] = Mathf.Clamp((int)num5, 0, 16);
				}
			}
			terrainData.SetDetailLayer(0, 0, detailLayer, array2);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000A12C File Offset: 0x0000832C
		public static Mesh CreateCrossQuadsMesh(string name, int quadCount)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(MeshFilter) });
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.transform.position = Vector3.zero;
			CombineInstance[] array = new CombineInstance[quadCount];
			for (int i = 0; i < quadCount; i++)
			{
				GameObject gameObject2 = new GameObject("quadToCombine_" + i.ToString(), new Type[] { typeof(MeshFilter) });
				Mesh mesh = GPUIUtility.GenerateQuadMesh(1f, 1f, new Rect?(new Rect(0f, 0f, 1f, 1f)), true, 0f, 0f, true);
				for (int j = 0; j < mesh.normals.Length; j++)
				{
					mesh.normals[i] = Vector3.up;
				}
				gameObject2.GetComponent<MeshFilter>().sharedMesh = mesh;
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity * Quaternion.AngleAxis(180f / (float)quadCount * (float)i, Vector3.up);
				gameObject2.transform.localScale = Vector3.one;
				array[i] = new CombineInstance
				{
					mesh = gameObject2.GetComponent<MeshFilter>().sharedMesh,
					transform = gameObject2.transform.localToWorldMatrix
				};
			}
			gameObject.GetComponent<MeshFilter>().sharedMesh = new Mesh();
			gameObject.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(array, true, true);
			Mesh sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
			sharedMesh.name = name;
			Object.DestroyImmediate(gameObject);
			return sharedMesh;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A2EC File Offset: 0x000084EC
		public static Mesh GenerateBladeMesh(Vector2 size, int segmentCount, float bendMultiplier, float bendLowerAmount, AnimationCurve bladeBendCurve, AnimationCurve bladeWidthCurve)
		{
			Mesh mesh = new Mesh();
			mesh.name = "BladeMesh";
			int num = 4 + 2 * (segmentCount - 1) + 1;
			float num2 = size.y / (float)(segmentCount + 1);
			Vector3[] array = new Vector3[num];
			Vector2[] array2 = new Vector2[num];
			int[] array3 = new int[segmentCount * 6 + 3];
			for (int i = 0; i < segmentCount; i++)
			{
				float num3 = bladeBendCurve.Evaluate(((float)i + 1f) / (float)(segmentCount + 1)) * size.y * bendMultiplier;
				float num4 = num3 * bendLowerAmount;
				float num5 = size.x * bladeWidthCurve.Evaluate(((float)i + 1f) / (float)segmentCount);
				array[(i + 1) * 2] = new Vector3(-num5, num2 * (float)(i + 1) - num4, num3);
				array[(i + 1) * 2 + 1] = new Vector3(num5, num2 * (float)(i + 1) - num4, num3);
				array2[(i + 1) * 2] = new Vector2(0f, ((float)i + 1f) / (float)(segmentCount + 1));
				array2[(i + 1) * 2 + 1] = new Vector2(1f, ((float)i + 1f) / (float)(segmentCount + 1));
				array3[i * 6] = i * 2;
				array3[i * 6 + 1] = (i + 1) * 2;
				array3[i * 6 + 2] = i * 2 + 1;
				array3[i * 6 + 3] = i * 2 + 1;
				array3[i * 6 + 4] = (i + 1) * 2;
				array3[i * 6 + 5] = (i + 1) * 2 + 1;
			}
			float num6 = size.x * bladeWidthCurve.Evaluate(0f);
			array[0] = new Vector3(-num6, 0f, 0f);
			array[1] = new Vector3(num6, 0f, 0f);
			float num7 = bladeBendCurve.Evaluate(1f) * size.y * bendMultiplier;
			array[num - 1] = new Vector3(0f, size.y - num7 * bendLowerAmount, num7);
			mesh.vertices = array;
			array2[0] = Vector2.zero;
			array2[1] = new Vector2(1f, 0f);
			array2[num - 1] = Vector2.one;
			mesh.uv = array2;
			array3[array3.Length - 3] = segmentCount * 2;
			array3[array3.Length - 2] = num - 1;
			array3[array3.Length - 1] = segmentCount * 2 + 1;
			mesh.triangles = array3;
			Vector3 vector = new Vector3(0f, 0f, -1f);
			Vector4 vector2 = new Vector4(1f, 0f, 0f, -1f);
			Vector3[] array4 = new Vector3[num];
			for (int j = 0; j < num; j++)
			{
				array4[j] = vector;
			}
			mesh.normals = array4;
			Vector4[] array5 = new Vector4[num];
			for (int k = 0; k < num; k++)
			{
				array5[k] = vector2;
			}
			mesh.tangents = array5;
			Color[] array6 = new Color[num];
			for (int l = 0; l < num; l++)
			{
				array6[l] = Color.Lerp(Color.clear, Color.red, mesh.vertices[l].y / size.y);
			}
			mesh.colors = array6;
			return mesh;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A640 File Offset: 0x00008840
		public static void SetDetailDensityInsideCollider(GPUIDetailManager detailManager, Collider collider, float valueToSet, float offset = 0f, List<int> prototypeIndexFilter = null)
		{
			if (!detailManager.IsInitialized)
			{
				Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Detail Manager is not initialized. Can not modify the detail density!");
				return;
			}
			BoxCollider boxCollider = collider as BoxCollider;
			if (boxCollider != null)
			{
				GPUITerrainUtility.SetDetailDensityInsideBoxCollider(detailManager, valueToSet, boxCollider, offset, prototypeIndexFilter);
			}
			else
			{
				SphereCollider sphereCollider = collider as SphereCollider;
				if (sphereCollider != null)
				{
					GPUITerrainUtility.SetDetailDensityInsideSphereCollider(detailManager, valueToSet, sphereCollider, offset, prototypeIndexFilter);
				}
				else
				{
					CapsuleCollider capsuleCollider = collider as CapsuleCollider;
					if (capsuleCollider != null)
					{
						GPUITerrainUtility.SetDetailDensityInsideCapsuleCollider(detailManager, valueToSet, capsuleCollider, offset, prototypeIndexFilter);
					}
					else
					{
						GPUITerrainUtility.SetDetailDensityInsideBounds(detailManager, valueToSet, collider.bounds, offset, prototypeIndexFilter);
					}
				}
			}
			detailManager.RequireUpdate();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A6C8 File Offset: 0x000088C8
		private static void SetCommonCSParams(ComputeShader cs, int kernelIndex, GPUITerrain gpuiTerrain, RenderTexture detailTexture, int textureSize, Texture heightmapTexture)
		{
			if (gpuiTerrain.HasTwoChannelHeightmap())
			{
				cs.EnableKeyword(GPUITerrainConstants.Kw_GPUI_TWO_CHANNEL_HEIGHTMAP);
			}
			else
			{
				cs.DisableKeyword(GPUITerrainConstants.Kw_GPUI_TWO_CHANNEL_HEIGHTMAP);
			}
			cs.SetTexture(kernelIndex, GPUITerrainConstants.PROP_terrainDetailTexture, detailTexture);
			cs.SetTexture(kernelIndex, GPUITerrainConstants.PROP_heightmapTexture, heightmapTexture);
			cs.SetInt(GPUITerrainConstants.PROP_heightmapTextureSize, heightmapTexture.width);
			cs.SetInt(GPUITerrainConstants.PROP_detailTextureSize, textureSize);
			cs.SetVector(GPUITerrainConstants.PROP_terrainPosition, gpuiTerrain.GetPosition());
			cs.SetVector(GPUITerrainConstants.PROP_terrainSize, gpuiTerrain.GetSize());
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A75C File Offset: 0x0000895C
		public static void SetDetailDensityInsideBounds(GPUIDetailManager detailManager, float valueToSet, Bounds bounds, float offset, List<int> prototypeIndexFilter)
		{
			if (!detailManager.IsInitialized)
			{
				Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Detail Manager is not initialized. Can not modify the detail density!");
				return;
			}
			valueToSet /= 255f;
			int prototypeCount = detailManager.GetPrototypeCount();
			ComputeShader cs_TerrainDetailDensityModifier = GPUITerrainConstants.CS_TerrainDetailDensityModifier;
			int num = 0;
			foreach (GPUITerrain gpuiterrain in detailManager.GetActiveTerrainValues())
			{
				if (gpuiterrain.GetTerrainWorldBounds().Intersects(bounds))
				{
					Texture heightmapTexture = gpuiterrain.GetHeightmapTexture();
					if (!(heightmapTexture == null))
					{
						if (!gpuiterrain.IsDetailDensityTexturesLoaded)
						{
							gpuiterrain.CreateDetailTextures();
						}
						for (int i = 0; i < prototypeCount; i++)
						{
							if (prototypeIndexFilter == null || prototypeIndexFilter.Count <= 0 || prototypeIndexFilter.Contains(i))
							{
								RenderTexture detailDensityTexture = gpuiterrain.GetDetailDensityTexture(gpuiterrain.GetFirstTerrainDetailPrototypeIndex(i));
								if (!(detailDensityTexture == null))
								{
									int width = detailDensityTexture.width;
									GPUITerrainUtility.SetCommonCSParams(cs_TerrainDetailDensityModifier, num, gpuiterrain, detailDensityTexture, width, heightmapTexture);
									cs_TerrainDetailDensityModifier.SetFloat(GPUIConstants.PROP_valueToSet, valueToSet);
									cs_TerrainDetailDensityModifier.SetVector(GPUIConstants.PROP_boundsCenter, bounds.center);
									cs_TerrainDetailDensityModifier.SetVector(GPUIConstants.PROP_boundsExtents, bounds.extents + Vector3.one * offset);
									cs_TerrainDetailDensityModifier.DispatchXY(num, width, width);
								}
							}
						}
					}
				}
			}
			detailManager.RequireUpdate();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A8F0 File Offset: 0x00008AF0
		public static void SetDetailDensityInsideBoxCollider(GPUIDetailManager detailManager, float valueToSet, BoxCollider boxCollider, float offset, List<int> prototypeIndexFilter)
		{
			if (!detailManager.IsInitialized)
			{
				Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Detail Manager is not initialized. Can not modify the detail density!");
				return;
			}
			valueToSet /= 255f;
			int prototypeCount = detailManager.GetPrototypeCount();
			ComputeShader cs_TerrainDetailDensityModifier = GPUITerrainConstants.CS_TerrainDetailDensityModifier;
			int num = 1;
			Vector3 center = boxCollider.center;
			Vector3 vector = boxCollider.size / 2f + Vector3.one * offset;
			Matrix4x4 localToWorldMatrix = boxCollider.transform.localToWorldMatrix;
			Bounds bounds = boxCollider.bounds;
			foreach (GPUITerrain gpuiterrain in detailManager.GetActiveTerrainValues())
			{
				if (gpuiterrain.GetTerrainWorldBounds().Intersects(bounds))
				{
					Texture heightmapTexture = gpuiterrain.GetHeightmapTexture();
					if (!(heightmapTexture == null))
					{
						if (!gpuiterrain.IsDetailDensityTexturesLoaded)
						{
							gpuiterrain.CreateDetailTextures();
						}
						for (int i = 0; i < prototypeCount; i++)
						{
							if (prototypeIndexFilter == null || prototypeIndexFilter.Count <= 0 || prototypeIndexFilter.Contains(i))
							{
								RenderTexture detailDensityTexture = gpuiterrain.GetDetailDensityTexture(gpuiterrain.GetFirstTerrainDetailPrototypeIndex(i));
								if (!(detailDensityTexture == null))
								{
									int width = detailDensityTexture.width;
									GPUITerrainUtility.SetCommonCSParams(cs_TerrainDetailDensityModifier, num, gpuiterrain, detailDensityTexture, width, heightmapTexture);
									cs_TerrainDetailDensityModifier.SetFloat(GPUIConstants.PROP_valueToSet, valueToSet);
									cs_TerrainDetailDensityModifier.SetVector(GPUIConstants.PROP_boundsCenter, center);
									cs_TerrainDetailDensityModifier.SetVector(GPUIConstants.PROP_boundsExtents, vector);
									cs_TerrainDetailDensityModifier.SetMatrix(GPUIConstants.PROP_modifierTransform, localToWorldMatrix);
									cs_TerrainDetailDensityModifier.DispatchXY(num, width, width);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000AAAC File Offset: 0x00008CAC
		public static void SetDetailDensityInsideSphereCollider(GPUIDetailManager detailManager, float valueToSet, SphereCollider sphereCollider, float offset, List<int> prototypeIndexFilter)
		{
			if (!detailManager.IsInitialized)
			{
				Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Detail Manager is not initialized. Can not modify the detail density!");
				return;
			}
			valueToSet /= 255f;
			int prototypeCount = detailManager.GetPrototypeCount();
			ComputeShader cs_TerrainDetailDensityModifier = GPUITerrainConstants.CS_TerrainDetailDensityModifier;
			int num = 2;
			Vector3 vector = sphereCollider.center + sphereCollider.transform.position;
			Vector3 lossyScale = sphereCollider.transform.lossyScale;
			float num2 = sphereCollider.radius * Mathf.Max(Mathf.Max(lossyScale.x, lossyScale.y), lossyScale.z) + offset;
			Bounds bounds = sphereCollider.bounds;
			foreach (GPUITerrain gpuiterrain in detailManager.GetActiveTerrainValues())
			{
				if (gpuiterrain.GetTerrainWorldBounds().Intersects(bounds))
				{
					Texture heightmapTexture = gpuiterrain.GetHeightmapTexture();
					if (!(heightmapTexture == null))
					{
						if (!gpuiterrain.IsDetailDensityTexturesLoaded)
						{
							gpuiterrain.CreateDetailTextures();
						}
						for (int i = 0; i < prototypeCount; i++)
						{
							if (prototypeIndexFilter == null || prototypeIndexFilter.Count <= 0 || prototypeIndexFilter.Contains(i))
							{
								RenderTexture detailDensityTexture = gpuiterrain.GetDetailDensityTexture(gpuiterrain.GetFirstTerrainDetailPrototypeIndex(i));
								if (!(detailDensityTexture == null))
								{
									int width = detailDensityTexture.width;
									GPUITerrainUtility.SetCommonCSParams(cs_TerrainDetailDensityModifier, num, gpuiterrain, detailDensityTexture, width, heightmapTexture);
									cs_TerrainDetailDensityModifier.SetFloat(GPUIConstants.PROP_valueToSet, valueToSet);
									cs_TerrainDetailDensityModifier.SetVector(GPUIConstants.PROP_boundsCenter, vector);
									cs_TerrainDetailDensityModifier.SetFloat(GPUIConstants.PROP_modifierRadius, num2);
									cs_TerrainDetailDensityModifier.DispatchXY(num, width, width);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000AC60 File Offset: 0x00008E60
		public static void SetDetailDensityInsideCapsuleCollider(GPUIDetailManager detailManager, float valueToSet, CapsuleCollider capsuleCollider, float offset, List<int> prototypeIndexFilter)
		{
			if (!detailManager.IsInitialized)
			{
				Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Detail Manager is not initialized. Can not modify the detail density!");
				return;
			}
			valueToSet /= 255f;
			int prototypeCount = detailManager.GetPrototypeCount();
			ComputeShader cs_TerrainDetailDensityModifier = GPUITerrainConstants.CS_TerrainDetailDensityModifier;
			int num = 3;
			Vector3 center = capsuleCollider.center;
			Vector3 lossyScale = capsuleCollider.transform.lossyScale;
			float num2 = capsuleCollider.radius * Mathf.Max(Mathf.Max((capsuleCollider.direction == 0) ? 0f : lossyScale.x, (capsuleCollider.direction == 1) ? 0f : lossyScale.y), (capsuleCollider.direction == 2) ? 0f : lossyScale.z) + offset;
			float num3 = capsuleCollider.height * ((capsuleCollider.direction == 0) ? lossyScale.x : ((capsuleCollider.direction == 1) ? lossyScale.y : ((capsuleCollider.direction == 2) ? lossyScale.z : 0f)));
			Bounds bounds = capsuleCollider.bounds;
			foreach (GPUITerrain gpuiterrain in detailManager.GetActiveTerrainValues())
			{
				if (gpuiterrain.GetTerrainWorldBounds().Intersects(bounds))
				{
					Texture heightmapTexture = gpuiterrain.GetHeightmapTexture();
					if (!(heightmapTexture == null))
					{
						if (!gpuiterrain.IsDetailDensityTexturesLoaded)
						{
							gpuiterrain.CreateDetailTextures();
						}
						for (int i = 0; i < prototypeCount; i++)
						{
							if (prototypeIndexFilter == null || prototypeIndexFilter.Contains(i))
							{
								RenderTexture detailDensityTexture = gpuiterrain.GetDetailDensityTexture(gpuiterrain.GetFirstTerrainDetailPrototypeIndex(i));
								if (!(detailDensityTexture == null))
								{
									int width = detailDensityTexture.width;
									GPUITerrainUtility.SetCommonCSParams(cs_TerrainDetailDensityModifier, num, gpuiterrain, detailDensityTexture, width, heightmapTexture);
									cs_TerrainDetailDensityModifier.SetFloat(GPUIConstants.PROP_valueToSet, valueToSet);
									cs_TerrainDetailDensityModifier.SetVector(GPUIConstants.PROP_boundsCenter, center);
									cs_TerrainDetailDensityModifier.SetFloat(GPUIConstants.PROP_modifierRadius, num2);
									cs_TerrainDetailDensityModifier.SetFloat(GPUIConstants.PROP_modifierHeight, num3);
									cs_TerrainDetailDensityModifier.DispatchXY(num, width, width);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000AE7C File Offset: 0x0000907C
		public static Material GetDefaultTerrainMaterial()
		{
			Material material;
			if (GPUIRuntimeSettings.Instance.IsURP)
			{
				material = new Material(Shader.Find("Universal Render Pipeline/Terrain/Lit"));
			}
			else if (GPUIRuntimeSettings.Instance.IsHDRP)
			{
				material = new Material(Shader.Find("HDRP/TerrainLit"));
			}
			else
			{
				material = new Material(Shader.Find("Nature/Terrain/Standard"));
			}
			return material;
		}

		// Token: 0x0400012F RID: 303
		public const float DETAIL_DENSITY_VALUE_DIVIDER = 255f;
	}
}
