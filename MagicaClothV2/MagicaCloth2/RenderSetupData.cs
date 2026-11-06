using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering;

namespace MagicaCloth2
{
	// Token: 0x02000088 RID: 136
	public class RenderSetupData : IDisposable, ITransform
	{
		// Token: 0x06000258 RID: 600 RVA: 0x00012FC8 File Offset: 0x000111C8
		public bool IsSuccess()
		{
			return this.result.IsSuccess();
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00012FD5 File Offset: 0x000111D5
		public bool IsFaild()
		{
			return this.result.IsFaild();
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00012FE2 File Offset: 0x000111E2
		public int TransformCount
		{
			get
			{
				List<Transform> list = this.transformList;
				if (list == null)
				{
					return 0;
				}
				return list.Count;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00012FF5 File Offset: 0x000111F5
		public bool HasMeshDataArray
		{
			get
			{
				return this.meshDataArray.Length > 0;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00013005 File Offset: 0x00011205
		public bool HasLocalPositions
		{
			get
			{
				return this.localPositions.IsCreated;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00013012 File Offset: 0x00011212
		public bool HasTangent
		{
			get
			{
				return this.localTangents.IsCreated && this.localTangents.Length > 0;
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00013031 File Offset: 0x00011231
		public RenderSetupData()
		{
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00013044 File Offset: 0x00011244
		public RenderSetupData(RenderSetupSerializeData referenceInitSetupData, Renderer ren)
		{
			this.result.Clear();
			this.setupType = RenderSetupData.SetupType.MeshCloth;
			if (ren == null)
			{
				object obj = "Renderer is null!";
				Develop.LogWarning(in obj);
				this.result.SetError(Define.Result.RenderSetup_InvalidSource);
				return;
			}
			bool flag = referenceInitSetupData != null;
			this.name = ren.name;
			SkinnedMeshRenderer skinnedMeshRenderer = ren as SkinnedMeshRenderer;
			this.hasSkinnedMesh = skinnedMeshRenderer;
			this.hasBoneWeight = false;
			this.skinRenderer = skinnedMeshRenderer;
			Transform transform = ren.transform;
			if (flag)
			{
				this.skinBoneCount = referenceInitSetupData.skinBoneCount;
				this.skinRootBoneIndex = referenceInitSetupData.skinRootBoneIndex;
				this.renderTransformIndex = referenceInitSetupData.renderTransformIndex;
				this.hasBoneWeight = referenceInitSetupData.hasBoneWeight;
				this.transformList = new List<Transform>(new Transform[referenceInitSetupData.transformCount]);
				int useTransformCount = referenceInitSetupData.useTransformCount;
				for (int i = 0; i < useTransformCount; i++)
				{
					int num = referenceInitSetupData.useTransformIndexArray[i];
					this.transformList[num] = referenceInitSetupData.transformArray[i];
				}
				if (skinnedMeshRenderer && referenceInitSetupData.originalMesh && skinnedMeshRenderer.sharedMesh != referenceInitSetupData.originalMesh && this.skinBoneCount > 0)
				{
					skinnedMeshRenderer.sharedMesh = referenceInitSetupData.originalMesh;
					Transform[] array = new Transform[this.skinBoneCount];
					this.transformList.CopyTo(0, array, 0, this.skinBoneCount);
					skinnedMeshRenderer.bones = array;
					skinnedMeshRenderer.rootBone = this.transformList[this.skinRootBoneIndex];
				}
			}
			else if (skinnedMeshRenderer)
			{
				Transform[] bones = skinnedMeshRenderer.bones;
				if (bones == null || bones.Length == 0)
				{
					this.skinBoneCount = 1;
					this.transformList = new List<Transform>(this.skinBoneCount);
					this.transformList.Add(transform);
					this.skinRootBoneIndex = 0;
					this.renderTransformIndex = 0;
				}
				else
				{
					this.skinBoneCount = bones.Length;
					this.transformList = new List<Transform>(this.skinBoneCount + 2);
					this.transformList.AddRange(bones);
					Transform transform2 = (skinnedMeshRenderer.rootBone ? skinnedMeshRenderer.rootBone : transform);
					this.skinRootBoneIndex = this.transformList.Count;
					this.transformList.Add(transform2);
					this.renderTransformIndex = this.transformList.Count;
					this.transformList.Add(transform);
					this.hasBoneWeight = true;
				}
			}
			else
			{
				this.skinBoneCount = 1;
				this.transformList = new List<Transform>(this.skinBoneCount);
				this.transformList.Add(transform);
				this.skinRootBoneIndex = 0;
				this.renderTransformIndex = 0;
			}
			this.ReadTransformInformation(false, referenceInitSetupData, ren.transform);
			Mesh mesh;
			if (skinnedMeshRenderer)
			{
				mesh = skinnedMeshRenderer.sharedMesh;
				if (mesh == null)
				{
					object obj = "SkinnedMeshRenderer.sharedMesh is null!";
					Develop.LogWarning(in obj);
					this.result.SetError(Define.Result.RenderSetup_NoMeshOnRenderer);
					return;
				}
				if (this.hasBoneWeight)
				{
					this.bindPoseList = new List<Matrix4x4>(mesh.bindposes);
					using (NativeArray<BoneWeight1> allBoneWeights = mesh.GetAllBoneWeights())
					{
						using (NativeArray<byte> bonesPerVertex = mesh.GetBonesPerVertex())
						{
							this.boneWeightArray = new NativeArray<BoneWeight1>(allBoneWeights, Allocator.Persistent);
							this.bonesPerVertexArray = new NativeArray<byte>(bonesPerVertex, Allocator.Persistent);
							goto IL_03E0;
						}
					}
				}
				this.bindPoseList = new List<Matrix4x4>(1);
				this.bindPoseList.Add(Matrix4x4.identity);
			}
			else
			{
				MeshFilter meshFilter;
				if (!ren.TryGetComponent<MeshFilter>(out meshFilter))
				{
					this.result.SetError(Define.Result.RenderSetup_InvalidSource);
					return;
				}
				mesh = meshFilter.sharedMesh;
				if (mesh == null)
				{
					object obj = "MeshFilter.sharedMesh is null!";
					Develop.LogWarning(in obj);
					this.result.SetError(Define.Result.RenderSetup_NoMeshOnRenderer);
					return;
				}
				this.bindPoseList = new List<Matrix4x4>(1);
				this.bindPoseList.Add(Matrix4x4.identity);
				this.meshFilter = meshFilter;
			}
			IL_03E0:
			if (!mesh.isReadable)
			{
				this.result.SetError(Define.Result.RenderSetup_Unreadable);
				return;
			}
			if (mesh.vertexCount > 65535)
			{
				this.result.SetError(Define.Result.RenderSetup_Over65535vertices);
				return;
			}
			this.meshDataArray = Mesh.AcquireReadOnlyMeshData(mesh);
			this.renderer = ren;
			this.originalMesh = mesh;
			this.vertexCount = mesh.vertexCount;
			this.result.SetSuccess();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x000134B8 File Offset: 0x000116B8
		public RenderSetupData(RenderSetupSerializeData referenceInitSetupData, RenderSetupData.SetupType setType, Transform renderTransform, List<Transform> rootTransforms, List<Transform> collisionBones, RenderSetupData.BoneConnectionMode connectionMode = RenderSetupData.BoneConnectionMode.Line, string name = "(no name)")
		{
			this.result.Clear();
			try
			{
				bool flag = referenceInitSetupData != null;
				this.setupType = setType;
				this.boneConnectionMode = connectionMode;
				if (renderTransform == null)
				{
					this.result.SetError(Define.Result.RenderSetup_InvalidSource);
				}
				else if (rootTransforms == null || rootTransforms.Count == 0)
				{
					this.result.SetError(Define.Result.RenderSetup_InvalidSource);
				}
				else
				{
					this.name = name;
					if (flag)
					{
						this.transformList = new List<Transform>(referenceInitSetupData.transformArray);
						this.skinBoneCount = referenceInitSetupData.skinBoneCount;
						this.renderTransformIndex = referenceInitSetupData.renderTransformIndex;
					}
					else
					{
						Dictionary<Transform, int> dictionary = new Dictionary<Transform, int>(256);
						this.transformList = new List<Transform>(256);
						Stack<Transform> stack = new Stack<Transform>(256);
						using (List<Transform>.Enumerator enumerator = rootTransforms.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Transform transform = enumerator.Current;
								stack.Push(transform);
							}
							goto IL_0162;
						}
						IL_0103:
						Transform transform2 = stack.Pop();
						if (!dictionary.ContainsKey(transform2))
						{
							int count = this.transformList.Count;
							this.transformList.Add(transform2);
							dictionary.Add(transform2, count);
							int childCount = transform2.childCount;
							for (int i = 0; i < childCount; i++)
							{
								stack.Push(transform2.GetChild(i));
							}
						}
						IL_0162:
						if (stack.Count > 0)
						{
							goto IL_0103;
						}
						this.skinBoneCount = this.transformList.Count;
						this.renderTransformIndex = this.transformList.Count;
						this.transformList.Add(renderTransform);
					}
					this.rootTransformIdList = new List<int>(rootTransforms.Count);
					foreach (Transform transform3 in rootTransforms)
					{
						this.rootTransformIdList.Add(transform3.GetInstanceID());
					}
					if (collisionBones != null)
					{
						this.collisionBoneIndexList = new List<int>(collisionBones.Count);
						foreach (Transform transform4 in collisionBones)
						{
							if (transform4)
							{
								int num = this.transformList.IndexOf(transform4);
								this.collisionBoneIndexList.Add(num);
							}
						}
					}
					this.ReadTransformInformation(true, referenceInitSetupData, renderTransform);
					this.result.SetSuccess();
				}
			}
			catch (MagicaClothProcessingException)
			{
				if (!this.result.IsError())
				{
					this.result.SetError(Define.Result.RenderSetup_UnknownError);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				this.result.SetError(Define.Result.RenderSetup_Exception);
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000137E0 File Offset: 0x000119E0
		private void ReadTransformInformation(bool includeChilds, RenderSetupSerializeData referenceInitSetupData, Transform rendererTransform)
		{
			int count = this.transformList.Count;
			bool flag = referenceInitSetupData != null;
			this.transformPositions = new NativeArray<float3>(count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.transformRotations = new NativeArray<quaternion>(count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.transformLocalPositions = new NativeArray<float3>(count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.transformLocalRotations = new NativeArray<quaternion>(count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.transformScales = new NativeArray<float3>(count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.transformInverseRotations = new NativeArray<quaternion>(count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			if (flag)
			{
				int useTransformCount = referenceInitSetupData.useTransformCount;
				if (useTransformCount == count)
				{
					NativeArray<float3>.Copy(referenceInitSetupData.transformPositions, 0, this.transformPositions, 0, useTransformCount);
					NativeArray<quaternion>.Copy(referenceInitSetupData.transformRotations, 0, this.transformRotations, 0, useTransformCount);
					NativeArray<float3>.Copy(referenceInitSetupData.transformLocalPositions, 0, this.transformLocalPositions, 0, useTransformCount);
					NativeArray<quaternion>.Copy(referenceInitSetupData.transformLocalRotations, 0, this.transformLocalRotations, 0, useTransformCount);
					NativeArray<float3>.Copy(referenceInitSetupData.transformScales, 0, this.transformScales, 0, useTransformCount);
				}
				else
				{
					for (int i = 0; i < useTransformCount; i++)
					{
						int num = referenceInitSetupData.useTransformIndexArray[i];
						this.transformPositions[num] = referenceInitSetupData.transformPositions[i];
						this.transformRotations[num] = referenceInitSetupData.transformRotations[i];
						this.transformLocalPositions[num] = referenceInitSetupData.transformLocalPositions[i];
						this.transformLocalRotations[num] = referenceInitSetupData.transformLocalRotations[i];
						this.transformScales[num] = referenceInitSetupData.transformScales[i];
					}
				}
				new RenderSetupData.CalcInverseRotationJob
				{
					rotations = this.transformRotations,
					inverseRotations = this.transformInverseRotations
				}.Run(count);
				this.initRenderLocalToWorld = referenceInitSetupData.initRenderLocalToWorld;
				this.initRenderWorldtoLocal = referenceInitSetupData.initRenderWorldtoLocal;
				this.initRenderRotation = referenceInitSetupData.initRenderRotation;
				this.initRenderScale = referenceInitSetupData.initRenderScale;
			}
			else
			{
				using (TransformAccessArray transformAccessArray = new TransformAccessArray(this.transformList.Count, -1))
				{
					int count2 = this.transformList.Count;
					for (int j = 0; j < count2; j++)
					{
						Transform transform = this.transformList[j];
						if (transform == null)
						{
							transform = rendererTransform;
						}
						transformAccessArray.Add(transform);
					}
					new RenderSetupData.ReadTransformJob
					{
						positions = this.transformPositions,
						rotations = this.transformRotations,
						scales = this.transformScales,
						localPositions = this.transformLocalPositions,
						localRotations = this.transformLocalRotations,
						inverseRotations = this.transformInverseRotations
					}.RunReadOnly(transformAccessArray);
					this.initRenderLocalToWorld = this.GetRendeerLocalToWorldMatrix();
					this.initRenderWorldtoLocal = math.inverse(this.initRenderLocalToWorld);
					this.initRenderRotation = this.transformRotations[this.renderTransformIndex];
					this.initRenderScale = this.transformScales[this.renderTransformIndex];
				}
			}
			this.transformIdList = new List<int>(count);
			this.transformParentIdList = new List<int>(count);
			for (int k = 0; k < count; k++)
			{
				int num2 = 0;
				int num3 = 0;
				Transform transform2 = this.transformList[k];
				if (transform2)
				{
					num2 = transform2.GetInstanceID();
					if (includeChilds && transform2.parent)
					{
						num3 = transform2.parent.GetInstanceID();
					}
				}
				this.transformIdList.Add(num2);
				this.transformParentIdList.Add(num3);
			}
			if (includeChilds)
			{
				this.transformChildIdList = new List<FixedList512Bytes<int>>(count);
				for (int l = 0; l < count; l++)
				{
					Transform transform3 = this.transformList[l];
					FixedList512Bytes<int> fixedList512Bytes = default(FixedList512Bytes<int>);
					if (transform3 && transform3.childCount > 0)
					{
						for (int m = 0; m < transform3.childCount; m++)
						{
							Transform child = transform3.GetChild(m);
							int instanceID = child.GetInstanceID();
							fixedList512Bytes.Add(in instanceID);
						}
					}
					this.transformChildIdList.Add(fixedList512Bytes);
				}
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00013BF0 File Offset: 0x00011DF0
		public void Dispose()
		{
			if (this.isManaged)
			{
				return;
			}
			(ref this.bonesPerVertexArray).MC2DisposeSafe<byte>();
			(ref this.boneWeightArray).MC2DisposeSafe<BoneWeight1>();
			(ref this.localPositions).MC2DisposeSafe<Vector3>();
			(ref this.localNormals).MC2DisposeSafe<Vector3>();
			(ref this.localTangents).MC2DisposeSafe<Vector4>();
			(ref this.transformPositions).MC2DisposeSafe<float3>();
			(ref this.transformRotations).MC2DisposeSafe<quaternion>();
			(ref this.transformLocalPositions).MC2DisposeSafe<float3>();
			(ref this.transformLocalRotations).MC2DisposeSafe<quaternion>();
			(ref this.transformScales).MC2DisposeSafe<float3>();
			(ref this.transformInverseRotations).MC2DisposeSafe<quaternion>();
			if (this.setupType == RenderSetupData.SetupType.MeshCloth && this.meshDataArray.Length > 0)
			{
				this.meshDataArray.Dispose();
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00013CA0 File Offset: 0x00011EA0
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			if (this.transformList != null)
			{
				foreach (Transform transform in this.transformList)
				{
					if (transform)
					{
						transformSet.Add(transform);
					}
				}
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00013D04 File Offset: 0x00011F04
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			if (this.rootTransformIdList != null)
			{
				for (int i = 0; i < this.rootTransformIdList.Count; i++)
				{
					int num = this.rootTransformIdList[i];
					if (replaceDict.ContainsKey(num))
					{
						this.rootTransformIdList[i] = replaceDict[num].GetInstanceID();
					}
				}
			}
			for (int j = 0; j < this.TransformCount; j++)
			{
				int num2 = this.transformIdList[j];
				if (replaceDict.ContainsKey(num2))
				{
					Transform transform = replaceDict[num2];
					this.transformIdList[j] = transform.GetInstanceID();
					this.transformList[j] = transform;
				}
				int num3 = this.transformParentIdList[j];
				if (replaceDict.ContainsKey(num3))
				{
					Transform transform2 = replaceDict[num3];
					this.transformParentIdList[j] = transform2.GetInstanceID();
				}
				if (this.transformChildIdList != null)
				{
					FixedList512Bytes<int> fixedList512Bytes = this.transformChildIdList[j];
					for (int k = 0; k < fixedList512Bytes.Length; k++)
					{
						int num4 = fixedList512Bytes[k];
						if (replaceDict.ContainsKey(num4))
						{
							Transform transform3 = replaceDict[num4];
							fixedList512Bytes[k] = transform3.GetInstanceID();
						}
					}
					this.transformChildIdList[j] = fixedList512Bytes;
				}
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00013E52 File Offset: 0x00012052
		public Transform GetRendeerTransform()
		{
			return this.transformList[this.renderTransformIndex];
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00013E65 File Offset: 0x00012065
		public int GetRenderTransformId()
		{
			return this.transformIdList[this.renderTransformIndex];
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00013E78 File Offset: 0x00012078
		public float4x4 GetRendeerLocalToWorldMatrix()
		{
			int num = this.renderTransformIndex;
			float3 @float = this.transformPositions[num];
			quaternion quaternion = this.transformRotations[num];
			float3 float2 = this.transformScales[num];
			return float4x4.TRS(@float, quaternion, float2);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00013EB9 File Offset: 0x000120B9
		public Transform GetSkinRootTransform()
		{
			return this.transformList[this.skinRootBoneIndex];
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00013ECC File Offset: 0x000120CC
		public int GetSkinRootTransformId()
		{
			return this.transformIdList[this.skinRootBoneIndex];
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00013EDF File Offset: 0x000120DF
		public int GetTransformIndexFromId(int id)
		{
			return this.transformIdList.IndexOf(id);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00013EF0 File Offset: 0x000120F0
		public int GetParentTransformIndex(int index, bool centerExcluded)
		{
			int num = this.transformParentIdList[index];
			int num2 = this.transformIdList.IndexOf(num);
			if (centerExcluded && num2 == this.renderTransformIndex)
			{
				num2 = -1;
			}
			return num2;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00013F28 File Offset: 0x00012128
		public void GetBoneWeightsRun(NativeArray<BoneWeight> weights)
		{
			new RenderSetupData.GetBoneWeightJos
			{
				vcnt = this.vertexCount,
				bonesPerVertexArray = this.bonesPerVertexArray,
				boneWeightArray = this.boneWeightArray,
				boneWeights = weights
			}.Run<RenderSetupData.GetBoneWeightJos>();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00013F74 File Offset: 0x00012174
		public RenderSetupData.ShareSerializationData ShareSerialize()
		{
			RenderSetupData.ShareSerializationData shareSerializationData = new RenderSetupData.ShareSerializationData();
			try
			{
				shareSerializationData.result = this.result;
				shareSerializationData.name = this.name;
				shareSerializationData.setupType = this.setupType;
				shareSerializationData.originalMesh = this.originalMesh;
				shareSerializationData.vertexCount = this.vertexCount;
				shareSerializationData.hasSkinnedMesh = this.hasSkinnedMesh;
				shareSerializationData.hasBoneWeight = this.hasBoneWeight;
				shareSerializationData.skinRootBoneIndex = this.skinRootBoneIndex;
				shareSerializationData.skinBoneCount = this.skinBoneCount;
				shareSerializationData.bindPoseList = new List<Matrix4x4>(this.bindPoseList);
				shareSerializationData.bonesPerVertexArray = (ref this.bonesPerVertexArray).MC2ToRawBytes<byte>();
				shareSerializationData.boneWeightArray = (ref this.boneWeightArray).MC2ToRawBytes<BoneWeight1>();
				shareSerializationData.localPositions = this.originalMesh.vertices;
				shareSerializationData.localNormals = this.originalMesh.normals;
				if (this.originalMesh.HasVertexAttribute(VertexAttribute.Tangent))
				{
					shareSerializationData.localTangents = this.originalMesh.tangents;
				}
				shareSerializationData.boneConnectionMode = this.boneConnectionMode;
				shareSerializationData.renderTransformIndex = this.renderTransformIndex;
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			return shareSerializationData;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0001409C File Offset: 0x0001229C
		public static RenderSetupData ShareDeserialize(RenderSetupData.ShareSerializationData sdata)
		{
			RenderSetupData renderSetupData = new RenderSetupData();
			renderSetupData.isManaged = true;
			try
			{
				renderSetupData.name = sdata.name;
				renderSetupData.setupType = sdata.setupType;
				renderSetupData.originalMesh = sdata.originalMesh;
				renderSetupData.vertexCount = sdata.vertexCount;
				renderSetupData.hasSkinnedMesh = sdata.hasSkinnedMesh;
				renderSetupData.hasBoneWeight = sdata.hasBoneWeight;
				renderSetupData.skinRootBoneIndex = sdata.skinRootBoneIndex;
				renderSetupData.skinBoneCount = sdata.skinBoneCount;
				renderSetupData.bindPoseList = new List<Matrix4x4>(sdata.bindPoseList);
				renderSetupData.bonesPerVertexArray = NativeArrayExtensions.MC2FromRawBytes<byte>(sdata.bonesPerVertexArray, Allocator.Persistent);
				renderSetupData.boneWeightArray = NativeArrayExtensions.MC2FromRawBytes<BoneWeight1>(sdata.boneWeightArray, Allocator.Persistent);
				renderSetupData.localPositions = new NativeArray<Vector3>(sdata.localPositions, Allocator.Persistent);
				renderSetupData.localNormals = new NativeArray<Vector3>(sdata.localNormals, Allocator.Persistent);
				if (sdata.HasTangent)
				{
					renderSetupData.localTangents = new NativeArray<Vector4>(sdata.localTangents, Allocator.Persistent);
				}
				renderSetupData.boneConnectionMode = sdata.boneConnectionMode;
				renderSetupData.renderTransformIndex = sdata.renderTransformIndex;
				renderSetupData.result.SetSuccess();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				renderSetupData.result.SetError(Define.Result.PreBuild_InvalidRenderSetupData);
			}
			return renderSetupData;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x000141D8 File Offset: 0x000123D8
		public RenderSetupData.UniqueSerializationData UniqueSerialize()
		{
			RenderSetupData.UniqueSerializationData uniqueSerializationData = new RenderSetupData.UniqueSerializationData();
			try
			{
				uniqueSerializationData.result = this.result;
				uniqueSerializationData.renderer = this.renderer;
				uniqueSerializationData.skinRenderer = this.skinRenderer;
				uniqueSerializationData.meshFilter = this.meshFilter;
				uniqueSerializationData.originalMesh = this.originalMesh;
				uniqueSerializationData.transformList = new List<Transform>(this.transformList);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			return uniqueSerializationData;
		}

		// Token: 0x040003AC RID: 940
		public ResultCode result;

		// Token: 0x040003AD RID: 941
		public string name = string.Empty;

		// Token: 0x040003AE RID: 942
		public bool isManaged;

		// Token: 0x040003AF RID: 943
		public RenderSetupData.SetupType setupType;

		// Token: 0x040003B0 RID: 944
		public Renderer renderer;

		// Token: 0x040003B1 RID: 945
		public SkinnedMeshRenderer skinRenderer;

		// Token: 0x040003B2 RID: 946
		public MeshFilter meshFilter;

		// Token: 0x040003B3 RID: 947
		public Mesh originalMesh;

		// Token: 0x040003B4 RID: 948
		public int vertexCount;

		// Token: 0x040003B5 RID: 949
		public bool hasSkinnedMesh;

		// Token: 0x040003B6 RID: 950
		public bool hasBoneWeight;

		// Token: 0x040003B7 RID: 951
		public Mesh.MeshDataArray meshDataArray;

		// Token: 0x040003B8 RID: 952
		public int skinRootBoneIndex;

		// Token: 0x040003B9 RID: 953
		public int skinBoneCount;

		// Token: 0x040003BA RID: 954
		public List<Matrix4x4> bindPoseList;

		// Token: 0x040003BB RID: 955
		public NativeArray<byte> bonesPerVertexArray;

		// Token: 0x040003BC RID: 956
		public NativeArray<BoneWeight1> boneWeightArray;

		// Token: 0x040003BD RID: 957
		public NativeArray<Vector3> localPositions;

		// Token: 0x040003BE RID: 958
		public NativeArray<Vector3> localNormals;

		// Token: 0x040003BF RID: 959
		public NativeArray<Vector4> localTangents;

		// Token: 0x040003C0 RID: 960
		public List<int> rootTransformIdList;

		// Token: 0x040003C1 RID: 961
		public RenderSetupData.BoneConnectionMode boneConnectionMode;

		// Token: 0x040003C2 RID: 962
		public List<int> collisionBoneIndexList;

		// Token: 0x040003C3 RID: 963
		public List<Transform> transformList;

		// Token: 0x040003C4 RID: 964
		public List<int> transformIdList;

		// Token: 0x040003C5 RID: 965
		public List<int> transformParentIdList;

		// Token: 0x040003C6 RID: 966
		public List<FixedList512Bytes<int>> transformChildIdList;

		// Token: 0x040003C7 RID: 967
		public NativeArray<float3> transformPositions;

		// Token: 0x040003C8 RID: 968
		public NativeArray<quaternion> transformRotations;

		// Token: 0x040003C9 RID: 969
		public NativeArray<float3> transformLocalPositions;

		// Token: 0x040003CA RID: 970
		public NativeArray<quaternion> transformLocalRotations;

		// Token: 0x040003CB RID: 971
		public NativeArray<float3> transformScales;

		// Token: 0x040003CC RID: 972
		public NativeArray<quaternion> transformInverseRotations;

		// Token: 0x040003CD RID: 973
		public int renderTransformIndex;

		// Token: 0x040003CE RID: 974
		public float4x4 initRenderLocalToWorld;

		// Token: 0x040003CF RID: 975
		public float4x4 initRenderWorldtoLocal;

		// Token: 0x040003D0 RID: 976
		public quaternion initRenderRotation;

		// Token: 0x040003D1 RID: 977
		public float3 initRenderScale;

		// Token: 0x040003D2 RID: 978
		private static readonly ProfilerMarker readTransformProfiler = new ProfilerMarker("readTransform");

		// Token: 0x02000089 RID: 137
		public enum SetupType
		{
			// Token: 0x040003D4 RID: 980
			MeshCloth,
			// Token: 0x040003D5 RID: 981
			BoneCloth,
			// Token: 0x040003D6 RID: 982
			BoneSpring
		}

		// Token: 0x0200008A RID: 138
		public enum BoneConnectionMode
		{
			// Token: 0x040003D8 RID: 984
			Line,
			// Token: 0x040003D9 RID: 985
			AutomaticMesh,
			// Token: 0x040003DA RID: 986
			SequentialLoopMesh,
			// Token: 0x040003DB RID: 987
			SequentialNonLoopMesh
		}

		// Token: 0x0200008B RID: 139
		[BurstCompile]
		private struct CalcInverseRotationJob : IJobParallelFor
		{
			// Token: 0x06000271 RID: 625 RVA: 0x00014268 File Offset: 0x00012468
			public void Execute(int index)
			{
				quaternion quaternion = this.rotations[index];
				if (math.any(quaternion.value))
				{
					quaternion quaternion2 = math.inverse(quaternion);
					this.inverseRotations[index] = quaternion2;
				}
			}

			// Token: 0x040003DC RID: 988
			[ReadOnly]
			public NativeArray<quaternion> rotations;

			// Token: 0x040003DD RID: 989
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<quaternion> inverseRotations;
		}

		// Token: 0x0200008C RID: 140
		[BurstCompile]
		private struct ReadTransformJob : IJobParallelForTransform
		{
			// Token: 0x06000272 RID: 626 RVA: 0x000142A4 File Offset: 0x000124A4
			public void Execute(int index, TransformAccess transform)
			{
				if (!transform.isValid)
				{
					return;
				}
				Vector3 vector;
				Quaternion quaternion;
				transform.GetPositionAndRotation(out vector, out quaternion);
				float4x4 float4x = transform.localToWorldMatrix;
				this.positions[index] = vector;
				this.rotations[index] = quaternion;
				this.localPositions[index] = transform.localPosition;
				this.localRotations[index] = transform.localRotation;
				quaternion quaternion2 = math.inverse(quaternion);
				float4x4 float4x2 = math.mul(new float4x4(quaternion2, float3.zero), float4x);
				float3 @float = new float3(float4x2.c0.x, float4x2.c1.y, float4x2.c2.z);
				this.scales[index] = @float;
				this.inverseRotations[index] = quaternion2;
			}

			// Token: 0x040003DE RID: 990
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> positions;

			// Token: 0x040003DF RID: 991
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<quaternion> rotations;

			// Token: 0x040003E0 RID: 992
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> scales;

			// Token: 0x040003E1 RID: 993
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x040003E2 RID: 994
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<quaternion> localRotations;

			// Token: 0x040003E3 RID: 995
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<quaternion> inverseRotations;
		}

		// Token: 0x0200008D RID: 141
		[BurstCompile]
		private struct GetBoneWeightJos : IJob
		{
			// Token: 0x06000273 RID: 627 RVA: 0x0001438C File Offset: 0x0001258C
			public void Execute()
			{
				int num = 0;
				for (int i = 0; i < this.vcnt; i++)
				{
					BoneWeight boneWeight = default(BoneWeight);
					byte b = this.bonesPerVertexArray[i];
					for (int j = 0; j < (int)b; j++)
					{
						BoneWeight1 boneWeight2 = this.boneWeightArray[num];
						num++;
						switch (j)
						{
						case 0:
							boneWeight.weight0 = boneWeight2.weight;
							boneWeight.boneIndex0 = boneWeight2.boneIndex;
							break;
						case 1:
							boneWeight.weight1 = boneWeight2.weight;
							boneWeight.boneIndex1 = boneWeight2.boneIndex;
							break;
						case 2:
							boneWeight.weight2 = boneWeight2.weight;
							boneWeight.boneIndex2 = boneWeight2.boneIndex;
							break;
						case 3:
							boneWeight.weight3 = boneWeight2.weight;
							boneWeight.boneIndex3 = boneWeight2.boneIndex;
							break;
						}
					}
					this.boneWeights[i] = boneWeight;
				}
			}

			// Token: 0x040003E4 RID: 996
			public int vcnt;

			// Token: 0x040003E5 RID: 997
			[ReadOnly]
			public NativeArray<byte> bonesPerVertexArray;

			// Token: 0x040003E6 RID: 998
			[ReadOnly]
			public NativeArray<BoneWeight1> boneWeightArray;

			// Token: 0x040003E7 RID: 999
			[WriteOnly]
			public NativeArray<BoneWeight> boneWeights;
		}

		// Token: 0x0200008E RID: 142
		[Serializable]
		public class ShareSerializationData
		{
			// Token: 0x1700003C RID: 60
			// (get) Token: 0x06000274 RID: 628 RVA: 0x0001448B File Offset: 0x0001268B
			public bool HasTangent
			{
				get
				{
					Vector4[] array = this.localTangents;
					return array != null && array.Length != 0;
				}
			}

			// Token: 0x040003E8 RID: 1000
			public ResultCode result;

			// Token: 0x040003E9 RID: 1001
			public string name;

			// Token: 0x040003EA RID: 1002
			public RenderSetupData.SetupType setupType;

			// Token: 0x040003EB RID: 1003
			public Mesh originalMesh;

			// Token: 0x040003EC RID: 1004
			public int vertexCount;

			// Token: 0x040003ED RID: 1005
			public bool hasSkinnedMesh;

			// Token: 0x040003EE RID: 1006
			public bool hasBoneWeight;

			// Token: 0x040003EF RID: 1007
			public int skinRootBoneIndex;

			// Token: 0x040003F0 RID: 1008
			public int skinBoneCount;

			// Token: 0x040003F1 RID: 1009
			public List<Matrix4x4> bindPoseList;

			// Token: 0x040003F2 RID: 1010
			public byte[] bonesPerVertexArray;

			// Token: 0x040003F3 RID: 1011
			public byte[] boneWeightArray;

			// Token: 0x040003F4 RID: 1012
			public Vector3[] localPositions;

			// Token: 0x040003F5 RID: 1013
			public Vector3[] localNormals;

			// Token: 0x040003F6 RID: 1014
			public Vector4[] localTangents;

			// Token: 0x040003F7 RID: 1015
			public RenderSetupData.BoneConnectionMode boneConnectionMode;

			// Token: 0x040003F8 RID: 1016
			public int renderTransformIndex;
		}

		// Token: 0x0200008F RID: 143
		[Serializable]
		public class UniqueSerializationData : ITransform
		{
			// Token: 0x06000276 RID: 630 RVA: 0x000144A0 File Offset: 0x000126A0
			public void GetUsedTransform(HashSet<Transform> transformSet)
			{
				List<Transform> list = this.transformList;
				if (list == null)
				{
					return;
				}
				list.ForEach(delegate(Transform x)
				{
					if (x)
					{
						transformSet.Add(x);
					}
				});
			}

			// Token: 0x06000277 RID: 631 RVA: 0x000144D8 File Offset: 0x000126D8
			public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
			{
				if (this.transformList != null)
				{
					for (int i = 0; i < this.transformList.Count; i++)
					{
						Transform transform = this.transformList[i];
						if (transform)
						{
							int instanceID = transform.GetInstanceID();
							if (instanceID != 0 && replaceDict.ContainsKey(instanceID))
							{
								this.transformList[i] = replaceDict[instanceID];
							}
						}
					}
				}
			}

			// Token: 0x040003F9 RID: 1017
			public ResultCode result;

			// Token: 0x040003FA RID: 1018
			public Renderer renderer;

			// Token: 0x040003FB RID: 1019
			public SkinnedMeshRenderer skinRenderer;

			// Token: 0x040003FC RID: 1020
			public MeshFilter meshFilter;

			// Token: 0x040003FD RID: 1021
			public Mesh originalMesh;

			// Token: 0x040003FE RID: 1022
			public List<Transform> transformList;
		}
	}
}
