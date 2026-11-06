using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace GPUInstancerPro
{
	// Token: 0x0200001C RID: 28
	[CreateAssetMenu(menuName = "Rendering/GPU Instancer Pro/LOD Group Data", order = 611)]
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#GPUI_LOD_Group_Data")]
	[Serializable]
	public class GPUILODGroupData : ScriptableObject, IGPUIParameterBufferData, IGPUIDisposable, IDisposable
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000500A File Offset: 0x0000320A
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00005012 File Offset: 0x00003212
		public bool HasSkinning
		{
			get
			{
				return this._hasSkinnedMeshes;
			}
			private set
			{
				this._hasSkinnedMeshes = value;
				if (this._hasSkinnedMeshes && this.requiresTreeProxy)
				{
					this.requiresTreeProxy = false;
				}
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005032 File Offset: 0x00003232
		public GPUILODGroupData()
		{
			this.InitializeTransitionValues();
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00005063 File Offset: 0x00003263
		public int Length
		{
			get
			{
				if (this.lodDataArray != null)
				{
					return this.lodDataArray.Length;
				}
				return 0;
			}
		}

		// Token: 0x17000015 RID: 21
		public GPUILODData this[int index]
		{
			get
			{
				return this.lodDataArray[index];
			}
			set
			{
				this.lodDataArray[index] = value;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000508C File Offset: 0x0000328C
		public static GPUILODGroupData CreateLODGroupData(GPUIPrototype prototype)
		{
			if (prototype == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not create LODGroupData. Prototype is null.");
				return null;
			}
			if (prototype.prototypeType == GPUIPrototypeType.LODGroupData)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not create LODGroupData. Prototype type is already LODGroupData.");
				return null;
			}
			GPUILODGroupData gpuilodgroupData = ScriptableObject.CreateInstance<GPUILODGroupData>();
			gpuilodgroupData.name = prototype.ToString();
			gpuilodgroupData.CreateRenderersFromPrototype(prototype);
			return gpuilodgroupData;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000050EA File Offset: 0x000032EA
		public static GPUILODGroupData CreateLODGroupData(GameObject prefabObject)
		{
			if (prefabObject == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not create LODGroupData. Prefab object is null.");
				return null;
			}
			GPUILODGroupData gpuilodgroupData = ScriptableObject.CreateInstance<GPUILODGroupData>();
			gpuilodgroupData.name = prefabObject.name;
			gpuilodgroupData.CreateRenderersFromGameObject(prefabObject);
			return gpuilodgroupData;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005124 File Offset: 0x00003324
		public static GPUILODGroupData CreateLODGroupData(Mesh mesh, Material[] materials, ShadowCastingMode shadowCastingMode = ShadowCastingMode.On, int layer = 0)
		{
			if (mesh == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not create LODGroupData. Mesh is null.");
				return null;
			}
			if (materials == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not create LODGroupData. Materials is null.");
				return null;
			}
			GPUILODGroupData gpuilodgroupData = ScriptableObject.CreateInstance<GPUILODGroupData>();
			gpuilodgroupData.name = mesh.name;
			gpuilodgroupData.CreateRenderersFromMeshAndMaterial(mesh, materials, shadowCastingMode, layer);
			return gpuilodgroupData;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005188 File Offset: 0x00003388
		public bool CreateRenderersFromPrototype(GPUIPrototype prototype)
		{
			if (prototype == null)
			{
				return false;
			}
			this.prototype = prototype;
			this.lodDataArray = new GPUILODData[0];
			if (prototype.prototypeType == GPUIPrototypeType.Prefab)
			{
				if (prototype.prefabObject == null)
				{
					return false;
				}
				this.CheckTreeProxyRequirement();
				if (this.CreateRenderersFromGameObject(prototype.prefabObject))
				{
					if (prototype.isGenerateBillboard)
					{
						if (prototype.billboardAsset == null)
						{
							prototype.billboardAsset = GPUIBillboardUtility.FindBillboardAsset(prototype.prefabObject);
						}
						if (prototype.billboardAsset != null && prototype.billboardAsset.albedoAtlasTexture != null)
						{
							int length = this.Length;
							this.AddLOD(0f, 0f);
							if (!prototype.prefabObject.HasComponent<LODGroup>() || !prototype.isBillboardReplaceLODCulled)
							{
								this.transitionValues[length - 1] = 1f - prototype.billboardDistance;
							}
							this.AddRenderer(length, GPUIBillboardUtility.GenerateQuadMesh(prototype.billboardAsset), new Material[] { GPUIBillboardUtility.CreateBillboardMaterial(prototype.billboardAsset) }, Matrix4x4.identity, prototype.prefabObject.layer, ShadowCastingMode.Off, true, MotionVectorGenerationMode.Camera, false, true, 1U, LightProbeUsage.BlendProbes, -1, default(Matrix4x4));
						}
					}
					UnityAction<GPUILODGroupData> onRegeneratedRenderers = this.OnRegeneratedRenderers;
					if (onRegeneratedRenderers != null)
					{
						onRegeneratedRenderers(this);
					}
					return true;
				}
			}
			return prototype.prototypeType == GPUIPrototypeType.MeshAndMaterial && !(prototype.prototypeMesh == null) && this.CreateRenderersFromMeshAndMaterial(prototype.prototypeMesh, prototype.prototypeMaterials, ShadowCastingMode.On, prototype.layer);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005304 File Offset: 0x00003504
		private void CheckTreeProxyRequirement()
		{
			if (this.requiresTreeProxy)
			{
				return;
			}
			if (this.prototype.isRequireTreeProxy)
			{
				this.requiresTreeProxy = true;
				return;
			}
			foreach (Tree tree in this.prototype.prefabObject.GetComponentsInChildren<Tree>())
			{
				MeshRenderer meshRenderer;
				if (tree != null && tree.gameObject.HasComponent<MeshFilter>() && tree.gameObject.TryGetComponent<MeshRenderer>(out meshRenderer))
				{
					Material[] array = meshRenderer.sharedMaterials;
					int j = 0;
					while (j < array.Length)
					{
						Material material = array[j];
						if (material != null && material.shader != null)
						{
							if (material.shader.name.Contains("Tree Creator"))
							{
								GPUIRenderingSystem.Instance._hasTreeCreatorWind = true;
								return;
							}
							this.requiresTreeProxy = true;
							return;
						}
						else
						{
							j++;
						}
					}
					break;
				}
			}
			Renderer[] componentsInChildren2 = this.prototype.prefabObject.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				foreach (Material material2 in componentsInChildren2[i].sharedMaterials)
				{
					if (material2 != null && material2.shader != null && material2.shader.name.Contains("Nature/SpeedTree"))
					{
						this.requiresTreeProxy = true;
						return;
					}
				}
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005468 File Offset: 0x00003668
		public bool CreateRenderersFromGameObject(GameObject prefabObject)
		{
			if (prefabObject == null)
			{
				return false;
			}
			this._hasSkinningComponent = prefabObject.HasComponent<GPUISkinningBase>();
			this.lodDataArray = new GPUILODData[0];
			LODGroup lodgroup;
			if (prefabObject.TryGetComponent<LODGroup>(out lodgroup))
			{
				return this.GenerateRenderersFromLODGroup(lodgroup);
			}
			return this.GenerateRenderersFromMeshRenderers(prefabObject);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000054B4 File Offset: 0x000036B4
		public bool CreateRenderersFromMeshAndMaterial(Mesh mesh, Material[] materials, ShadowCastingMode shadowCastingMode, int layer)
		{
			this.lodDataArray = new GPUILODData[0];
			this.AddLOD(-1f, 0f);
			Material[] array = new Material[materials.Length];
			Array.Copy(materials, array, materials.Length);
			this.AddRenderer(0, mesh, array, Matrix4x4.identity, layer, shadowCastingMode, true, MotionVectorGenerationMode.Camera, false, false, 1U, LightProbeUsage.Off, -1, default(Matrix4x4));
			return true;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005514 File Offset: 0x00003714
		private bool GenerateRenderersFromLODGroup(LODGroup lodGroup)
		{
			LOD[] lods = lodGroup.GetLODs();
			this.lodGroupSize = lodGroup.size;
			for (int i = 0; i < lods.Length; i++)
			{
				bool flag = false;
				List<Renderer> list = new List<Renderer>();
				LOD lod = lods[i];
				Renderer[] renderers = lod.renderers;
				if (renderers != null)
				{
					foreach (Renderer renderer in renderers)
					{
						if (renderer != null)
						{
							if (renderer is MeshRenderer)
							{
								list.Add(renderer);
							}
							else if (renderer is BillboardRenderer)
							{
								flag = true;
							}
							else if (renderer is SkinnedMeshRenderer)
							{
								list.Add(renderer);
							}
						}
					}
				}
				if (list.Count == 0)
				{
					if (!flag)
					{
						Debug.LogWarning(string.Concat(new string[]
						{
							GPUIConstants.LOG_PREFIX,
							"LOD Group has no mesh renderers. Prefab: ",
							lodGroup.gameObject.name,
							" LODIndex: ",
							i.ToString()
						}), lodGroup.gameObject);
					}
				}
				else
				{
					this.AddLOD(lod.screenRelativeTransitionHeight, lod.fadeTransitionWidth);
					for (int k = 0; k < list.Count; k++)
					{
						this.AddRenderer(list[k], lodGroup.transform, i);
					}
				}
			}
			return true;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005658 File Offset: 0x00003858
		private bool GenerateRenderersFromMeshRenderers(GameObject prefabObject)
		{
			this.optionalRendererCount = 0;
			this.AddLOD(-1f, 0f);
			if (!prefabObject)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can't create renderer(s): GameObject is null");
				return false;
			}
			List<Renderer> list = new List<Renderer>();
			prefabObject.transform.GetMeshRenderers(list, true);
			if (list == null || list.Count == 0)
			{
				Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Can't create renderer(s): no MeshRenderers found in the reference GameObject <" + prefabObject.name + "> or any of its children", prefabObject);
				return false;
			}
			foreach (Renderer renderer in list)
			{
				GPUIRendererData gpuirendererData = this.AddRenderer(renderer, prefabObject.transform, 0);
				GPUIOptionalRenderer gpuioptionalRenderer;
				if (gpuirendererData != null && renderer.gameObject != prefabObject && renderer.gameObject.TryGetComponent<GPUIOptionalRenderer>(out gpuioptionalRenderer))
				{
					gpuirendererData.optionalRendererNo = gpuioptionalRenderer.optionalRendererNo;
					if (gpuirendererData.optionalRendererNo != 0)
					{
						bool flag = false;
						for (int i = 0; i < this.lodDataArray[0].Length; i++)
						{
							GPUIRendererData gpuirendererData2 = this.lodDataArray[0][i];
							if (gpuirendererData2 != null && gpuirendererData2 != gpuirendererData && gpuirendererData2.optionalRendererNo == gpuirendererData.optionalRendererNo)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.optionalRendererCount++;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000057C8 File Offset: 0x000039C8
		public GPUILODData AddLODAtIndex(int index, float transitionValue = -1f, float fadeTransitionWidth = 0f)
		{
			Array.Resize<GPUILODData>(ref this.lodDataArray, this.Length + 1);
			if (this.Length > 1)
			{
				for (int i = this.Length - 2; i >= index; i--)
				{
					this.lodDataArray[i + 1] = this.lodDataArray[i];
					this.transitionValues[i + 1] = this.transitionValues[i];
				}
			}
			this.lodDataArray[index] = new GPUILODData();
			if (transitionValue >= 0f)
			{
				this.transitionValues[index] = transitionValue;
			}
			else if (index == this.Length - 1)
			{
				this.transitionValues[index] = 0f;
			}
			else
			{
				float num = ((index == 0) ? 1f : this.transitionValues[index - 1]);
				float num2 = this.transitionValues[index + 1];
				this.transitionValues[index] = (num - num2) / 2f + num2;
			}
			this.fadeTransitionWidth[index] = fadeTransitionWidth;
			return this.lodDataArray[index];
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000058A6 File Offset: 0x00003AA6
		public GPUILODData AddLOD(float transitionValue = -1f, float fadeTransitionWidth = 0f)
		{
			return this.AddLODAtIndex(this.Length, transitionValue, fadeTransitionWidth);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000058B8 File Offset: 0x00003AB8
		public void RemoveLODAtIndex(int index)
		{
			for (int i = index; i < this.Length - 1; i++)
			{
				this.lodDataArray[i] = this.lodDataArray[i + 1];
				this.transitionValues[i] = this.transitionValues[i + 1];
			}
			for (int j = this.Length - 1; j < 8; j++)
			{
				this.transitionValues[j] = 0f;
			}
			Array.Resize<GPUILODData>(ref this.lodDataArray, this.Length - 1);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005930 File Offset: 0x00003B30
		public GPUIRendererData AddRenderer(Renderer renderer, Transform parentTransform, int lodIndex)
		{
			int num = -1;
			SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
			if (skinnedMeshRenderer != null)
			{
				if (skinnedMeshRenderer.sharedMesh == null)
				{
					Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Can't add renderer: mesh is null. Make sure that all the SkinnedMeshRenderers on the prototype has a mesh assigned.", parentTransform.gameObject);
					return null;
				}
				if (skinnedMeshRenderer.sharedMaterials == null || skinnedMeshRenderer.sharedMaterials.Length == 0)
				{
					Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Can't add renderer: no materials. Make sure that all the SkinnedMeshRenderers have their materials assigned.", parentTransform.gameObject);
					return null;
				}
				if (this._hasSkinningComponent)
				{
					this.HasSkinning = true;
				}
				return this.AddRenderer(lodIndex, skinnedMeshRenderer.sharedMesh, (Material[])renderer.sharedMaterials.Clone(), GPUIConstants.IDENTITY_Matrix4x4, renderer.gameObject.layer, renderer.shadowCastingMode, renderer.receiveShadows, renderer.motionVectorGenerationMode, this.HasSkinning, false, renderer.renderingLayerMask, renderer.lightProbeUsage, num, (skinnedMeshRenderer.rootBone != null) ? parentTransform.GetTransformOffset(skinnedMeshRenderer.rootBone.transform) : GPUIConstants.IDENTITY_Matrix4x4);
			}
			else
			{
				MeshFilter component = renderer.GetComponent<MeshFilter>();
				if (component == null)
				{
					Debug.LogWarning(GPUIConstants.LOG_PREFIX + "MeshRenderer with no MeshFilter found on GameObject <" + parentTransform.name + ">. Are you missing a component?", parentTransform.gameObject);
					return null;
				}
				if (component.sharedMesh == null)
				{
					Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Can't add renderer: mesh is null. Make sure that all the MeshFilters on the prototype has a mesh assigned.", parentTransform.gameObject);
					return null;
				}
				if (renderer.sharedMaterials == null || renderer.sharedMaterials.Length == 0)
				{
					Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Can't add renderer: no materials. Make sure that all the MeshRenderers have their materials assigned.", parentTransform.gameObject);
					return null;
				}
				Matrix4x4 transformOffset = parentTransform.GetTransformOffset(renderer.gameObject.transform);
				return this.AddRenderer(lodIndex, component.sharedMesh, (Material[])renderer.sharedMaterials.Clone(), transformOffset, renderer.gameObject.layer, renderer.shadowCastingMode, renderer.receiveShadows, renderer.motionVectorGenerationMode, false, false, renderer.renderingLayerMask, renderer.lightProbeUsage, num, transformOffset);
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005B18 File Offset: 0x00003D18
		public GPUIRendererData AddRenderer(int lodIndex, Mesh mesh, Material[] materials, Matrix4x4 transformOffset, int layer, ShadowCastingMode shadowCastingMode, bool receiveShadows = true, MotionVectorGenerationMode motionVectorGenerationMode = MotionVectorGenerationMode.Camera, bool isSkinnedMesh = false, bool doesNotContributeToBounds = false, uint renderingLayerMask = 1U, LightProbeUsage lightProbeUsage = LightProbeUsage.Off, int forceMeshLod = -1, Matrix4x4 boundsOffset = default(Matrix4x4))
		{
			if (this.Length <= lodIndex || this[lodIndex] == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can't add renderer: Invalid LOD");
				return null;
			}
			GPUIRendererData gpuirendererData = new GPUIRendererData(mesh, materials, transformOffset, layer, shadowCastingMode, receiveShadows, motionVectorGenerationMode, isSkinnedMesh, doesNotContributeToBounds, renderingLayerMask, lightProbeUsage, forceMeshLod, boundsOffset);
			this[lodIndex].Add(gpuirendererData);
			this.CalculateBounds();
			return gpuirendererData;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005B80 File Offset: 0x00003D80
		public void CalculateBounds()
		{
			if (this.lodDataArray == null || this.lodDataArray.Length == 0 || this.lodDataArray[0].rendererDataArray == null || this.lodDataArray[0].rendererDataArray.Length == 0)
			{
				return;
			}
			for (int i = 0; i < this.lodDataArray.Length; i++)
			{
				GPUILODData gpuiloddata = this.lodDataArray[i];
				for (int j = 0; j < gpuiloddata.rendererDataArray.Length; j++)
				{
					GPUIRendererData gpuirendererData = gpuiloddata.rendererDataArray[j];
					if (!gpuirendererData.doesNotContributeToBounds && !(gpuirendererData.rendererMesh == null))
					{
						Bounds matrixAppliedBoundsWithPivot = gpuirendererData.rendererMesh.bounds;
						matrixAppliedBoundsWithPivot = matrixAppliedBoundsWithPivot.GetMatrixAppliedBoundsWithPivot(gpuirendererData.boundsOffset, GPUIConstants.Vector3_ZERO);
						if (i == 0 && j == 0)
						{
							this.bounds = matrixAppliedBoundsWithPivot;
						}
						else
						{
							this.bounds.Encapsulate(matrixAppliedBoundsWithPivot);
						}
					}
				}
			}
			if (this.prototype != null && this.prototype.profile != null)
			{
				this.bounds.Expand(this.prototype.profile.boundsOffset);
			}
			this.SetParameterBufferData();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005C94 File Offset: 0x00003E94
		public void InitializeTransitionValues()
		{
			if (this.transitionValues == null)
			{
				this.transitionValues = new float[8];
			}
			else if (this.transitionValues.Length != 8)
			{
				Array.Resize<float>(ref this.transitionValues, 8);
			}
			for (int i = 0; i < 8; i++)
			{
				if (i >= this.Length)
				{
					this.transitionValues[i] = 0f;
				}
				else if (i == 0)
				{
					this.transitionValues[i] = Mathf.Clamp01(this.transitionValues[i]);
				}
				else
				{
					this.transitionValues[i] = Mathf.Clamp(this.transitionValues[i], 0f, this.transitionValues[i - 1]);
				}
			}
			if (this.fadeTransitionWidth == null)
			{
				this.fadeTransitionWidth = new float[8];
				return;
			}
			if (this.fadeTransitionWidth.Length != 8)
			{
				Array.Resize<float>(ref this.fadeTransitionWidth, 8);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005D5C File Offset: 0x00003F5C
		public void SetParameterBufferData()
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			GPUIDataBuffer<float> parameterBuffer = GPUIRenderingSystem.Instance.ParameterBuffer;
			this.InitializeTransitionValues();
			int length;
			if (this.TryGetParameterBufferIndex(out length))
			{
				parameterBuffer[length] = (float)this.Length;
				parameterBuffer[length + 1] = this.bounds.center.x;
				parameterBuffer[length + 2] = this.bounds.center.y;
				parameterBuffer[length + 3] = this.bounds.center.z;
				parameterBuffer[length + 4] = this.bounds.extents.x;
				parameterBuffer[length + 5] = this.bounds.extents.y;
				parameterBuffer[length + 6] = this.bounds.extents.z;
				parameterBuffer[length + 23] = this.lodGroupSize;
			}
			else
			{
				length = parameterBuffer.Length;
				GPUIRenderingSystem.Instance.ParameterBufferIndexes.Add(this, length);
				parameterBuffer.Add(new float[]
				{
					(float)this.Length,
					this.bounds.center.x,
					this.bounds.center.y,
					this.bounds.center.z,
					this.bounds.extents.x,
					this.bounds.extents.y,
					this.bounds.extents.z
				});
				parameterBuffer.Add(this.transitionValues);
				parameterBuffer.Add(new float[8]);
				parameterBuffer.Add(this.lodGroupSize);
				parameterBuffer.Add(this.fadeTransitionWidth);
			}
			float lodBias = QualitySettings.lodBias;
			for (int i = 0; i < 8; i++)
			{
				parameterBuffer[length + 7 + i] = this.transitionValues[i] / lodBias;
			}
			int num = 0;
			while (num < 8 && num < this.Length)
			{
				parameterBuffer[length + 15 + num] = (this.lodDataArray[num].IsShadowCasting() ? 1f : 0f);
				num++;
			}
			for (int j = 0; j < 8; j++)
			{
				parameterBuffer[length + 24 + j] = this.fadeTransitionWidth[j];
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000359B File Offset: 0x0000179B
		public bool TryGetParameterBufferIndex(out int index)
		{
			return GPUIRenderingSystem.Instance.ParameterBufferIndexes.TryGetValue(this, out index);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005FA8 File Offset: 0x000041A8
		public int GetMeshMaterialCombinationCount()
		{
			int num = 0;
			for (int i = 0; i < this.Length; i++)
			{
				GPUILODData gpuiloddata = this[i];
				for (int j = 0; j < gpuiloddata.Length; j++)
				{
					GPUIRendererData gpuirendererData = gpuiloddata[j];
					num += gpuirendererData.rendererMaterials.Length;
				}
			}
			return num;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005FF8 File Offset: 0x000041F8
		public override string ToString()
		{
			if (this.prototype != null)
			{
				return this.prototype.ToString();
			}
			if (!string.IsNullOrEmpty(base.name))
			{
				return GPUIUtility.CamelToTitleCase(base.name.Replace("_", ""));
			}
			return base.ToString();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006048 File Offset: 0x00004248
		internal bool HasObjectMotion()
		{
			for (int i = 0; i < this.Length; i++)
			{
				for (int j = 0; j < this.lodDataArray[i].Length; j++)
				{
					if (this.lodDataArray[i].rendererDataArray[j].motionVectorGenerationMode == MotionVectorGenerationMode.Object)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000024BC File Offset: 0x000006BC
		public void ReleaseBuffers()
		{
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006098 File Offset: 0x00004298
		public void Dispose()
		{
			for (int i = 0; i < this.Length; i++)
			{
				this[i].Dispose();
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000060C4 File Offset: 0x000042C4
		public void RemoveReplacementMaterials()
		{
			if (this.lodDataArray == null)
			{
				return;
			}
			GPUILODData[] array = this.lodDataArray;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveReplacementMaterials();
			}
		}

		// Token: 0x0400006E RID: 110
		public GPUILODData[] lodDataArray;

		// Token: 0x0400006F RID: 111
		public float[] transitionValues = new float[8];

		// Token: 0x04000070 RID: 112
		public float[] fadeTransitionWidth = new float[8];

		// Token: 0x04000071 RID: 113
		public Bounds bounds;

		// Token: 0x04000072 RID: 114
		public float lodGroupSize = 1f;

		// Token: 0x04000073 RID: 115
		public int optionalRendererCount;

		// Token: 0x04000074 RID: 116
		[NonSerialized]
		public GPUIPrototype prototype;

		// Token: 0x04000075 RID: 117
		[NonSerialized]
		public bool requiresTreeProxy;

		// Token: 0x04000076 RID: 118
		[NonSerialized]
		private bool _hasSkinnedMeshes;

		// Token: 0x04000077 RID: 119
		[NonSerialized]
		private bool _hasSkinningComponent;

		// Token: 0x04000078 RID: 120
		public UnityAction<GPUILODGroupData> OnRegeneratedRenderers;
	}
}
