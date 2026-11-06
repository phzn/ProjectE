using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000091 RID: 145
	[Serializable]
	public class RenderSetupSerializeData : ITransform
	{
		// Token: 0x0600027B RID: 635 RVA: 0x00014558 File Offset: 0x00012758
		public bool DataValidateMeshCloth(Renderer ren)
		{
			if (this.setupType != RenderSetupData.SetupType.MeshCloth)
			{
				return false;
			}
			if (ren == null)
			{
				return false;
			}
			if (this.useTransformIndexArray == null || this.useTransformIndexArray.Length == 0)
			{
				return false;
			}
			if (ren is SkinnedMeshRenderer)
			{
				if (!this.hasSkinnedMesh)
				{
					return false;
				}
				if ((ren as SkinnedMeshRenderer).sharedMesh == null)
				{
					return false;
				}
			}
			else
			{
				if (this.hasSkinnedMesh)
				{
					return false;
				}
				MeshFilter meshFilter;
				if (!ren.TryGetComponent<MeshFilter>(out meshFilter))
				{
					return false;
				}
				if (meshFilter.sharedMesh == null)
				{
					return false;
				}
			}
			return this.DataValidateTransform();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000145E8 File Offset: 0x000127E8
		public bool DataValidateBoneCloth(ClothSerializeData sdata, RenderSetupData.SetupType clothType)
		{
			if (this.setupType != clothType)
			{
				return false;
			}
			if (this.hasSkinnedMesh)
			{
				return false;
			}
			if (this.hasBoneWeight)
			{
				return false;
			}
			if (!this.DataValidateTransform())
			{
				return false;
			}
			using (List<Transform>.Enumerator enumerator = sdata.rootBones.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform rootBone = enumerator.Current;
					if (rootBone == null || Array.FindIndex<Transform>(this.transformArray, (Transform x) => x == rootBone) < 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00014698 File Offset: 0x00012898
		public bool DataValidateTransform()
		{
			if (this.transformCount == 0)
			{
				return false;
			}
			if (this.useTransformCount == 0)
			{
				return false;
			}
			int num = this.useTransformCount;
			int num2 = num;
			int[] array = this.useTransformIndexArray;
			if (num2 != ((array != null) ? array.Length : 0))
			{
				return false;
			}
			int num3 = num;
			Transform[] array2 = this.transformArray;
			if (num3 != ((array2 != null) ? array2.Length : 0))
			{
				return false;
			}
			if (Array.FindIndex<Transform>(this.transformArray, (Transform x) => x == null) >= 0)
			{
				return false;
			}
			int num4 = num;
			float3[] array3 = this.transformPositions;
			if (num4 != ((array3 != null) ? array3.Length : 0))
			{
				return false;
			}
			int num5 = num;
			quaternion[] array4 = this.transformRotations;
			if (num5 != ((array4 != null) ? array4.Length : 0))
			{
				return false;
			}
			int num6 = num;
			float3[] array5 = this.transformLocalPositions;
			if (num6 != ((array5 != null) ? array5.Length : 0))
			{
				return false;
			}
			int num7 = num;
			quaternion[] array6 = this.transformLocalRotations;
			if (num7 != ((array6 != null) ? array6.Length : 0))
			{
				return false;
			}
			int num8 = num;
			float3[] array7 = this.transformScales;
			return num8 == ((array7 != null) ? array7.Length : 0);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0001477C File Offset: 0x0001297C
		public bool Serialize(RenderSetupData sd)
		{
			this.setupType = sd.setupType;
			this.vertexCount = sd.vertexCount;
			this.hasSkinnedMesh = sd.hasSkinnedMesh;
			this.hasBoneWeight = sd.hasBoneWeight;
			this.skinRootBoneIndex = sd.skinRootBoneIndex;
			this.renderTransformIndex = sd.renderTransformIndex;
			this.skinBoneCount = sd.skinBoneCount;
			this.transformCount = sd.TransformCount;
			this.useTransformCount = 0;
			this.originalMesh = null;
			if (sd.TransformCount > 0)
			{
				int num = sd.TransformCount;
				using (NativeList<int> nativeList = new NativeList<int>(num, Allocator.TempJob))
				{
					if (this.setupType == RenderSetupData.SetupType.MeshCloth)
					{
						if (this.hasBoneWeight)
						{
							new RenderSetupSerializeData.CalcUseBoneArrayJob2
							{
								boneCount = this.skinBoneCount,
								boneWeightArray = sd.boneWeightArray,
								useBoneIndexList = nativeList
							}.Run<RenderSetupSerializeData.CalcUseBoneArrayJob2>();
							if (this.skinRootBoneIndex >= this.skinBoneCount)
							{
								nativeList.Add(in this.skinRootBoneIndex);
							}
							nativeList.Add(in this.renderTransformIndex);
						}
						else
						{
							for (int i = 0; i < num; i++)
							{
								nativeList.Add(in i);
							}
						}
						if (sd.originalMesh && !sd.originalMesh.name.Contains("(Clone)"))
						{
							this.originalMesh = sd.originalMesh;
						}
					}
					else
					{
						for (int j = 0; j < num; j++)
						{
							nativeList.Add(in j);
						}
					}
					using (NativeArray<int> nativeArray = nativeList.ToArray(Allocator.TempJob))
					{
						this.useTransformIndexArray = nativeArray.ToArray();
						int num2 = this.useTransformIndexArray.Length;
						this.useTransformCount = num2;
						this.transformArray = new Transform[num2];
						this.transformPositions = new float3[num2];
						this.transformRotations = new quaternion[num2];
						this.transformLocalPositions = new float3[num2];
						this.transformLocalRotations = new quaternion[num2];
						this.transformScales = new float3[num2];
						for (int k = 0; k < num2; k++)
						{
							int num3 = this.useTransformIndexArray[k];
							this.transformArray[k] = sd.transformList[num3];
							this.transformPositions[k] = sd.transformPositions[num3];
							this.transformRotations[k] = sd.transformRotations[num3];
							this.transformLocalPositions[k] = sd.transformLocalPositions[num3];
							this.transformLocalRotations[k] = sd.transformLocalRotations[num3];
							this.transformScales[k] = sd.transformScales[num3];
						}
					}
				}
			}
			this.initRenderLocalToWorld = sd.initRenderLocalToWorld;
			this.initRenderWorldtoLocal = sd.initRenderWorldtoLocal;
			this.initRenderRotation = sd.initRenderRotation;
			this.initRenderScale = sd.initRenderScale;
			return true;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00014A9C File Offset: 0x00012C9C
		public int GetLocalHash()
		{
			int num = 0;
			num = (int)(num + this.setupType * (RenderSetupData.SetupType)100);
			num += this.vertexCount;
			num += (this.hasSkinnedMesh ? 1 : 0);
			num += (this.hasBoneWeight ? 1 : 0);
			num += this.skinRootBoneIndex;
			num += this.renderTransformIndex;
			num += this.skinBoneCount;
			num += this.transformCount;
			num += this.useTransformCount;
			int num2 = num;
			int[] array = this.useTransformIndexArray;
			num = num2 + ((array != null) ? array.Length : 0);
			int num3 = num;
			Transform[] array2 = this.transformArray;
			num = num3 + ((array2 != null) ? array2.Length : 0);
			int num4 = num;
			float3[] array3 = this.transformPositions;
			num = num4 + ((array3 != null) ? array3.Length : 0);
			int num5 = num;
			quaternion[] array4 = this.transformRotations;
			num = num5 + ((array4 != null) ? array4.Length : 0);
			int num6 = num;
			float3[] array5 = this.transformLocalPositions;
			num = num6 + ((array5 != null) ? array5.Length : 0);
			int num7 = num;
			quaternion[] array6 = this.transformLocalRotations;
			num = num7 + ((array6 != null) ? array6.Length : 0);
			int num8 = num;
			float3[] array7 = this.transformScales;
			num = num8 + ((array7 != null) ? array7.Length : 0);
			if (this.transformArray != null)
			{
				foreach (Transform transform in this.transformArray)
				{
					num += ((transform != null) ? (456 + transform.childCount * 789) : 0);
				}
			}
			if (this.originalMesh != null)
			{
				num += this.originalMesh.vertexCount;
			}
			return num;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00014BE8 File Offset: 0x00012DE8
		public int GetGlobalHash()
		{
			int num = 0;
			if (this.transformArray != null)
			{
				foreach (Transform transform in this.transformArray)
				{
					if (transform)
					{
						num += transform.localPosition.GetHashCode();
						num += transform.localRotation.GetHashCode();
						num += transform.localScale.GetHashCode();
					}
				}
			}
			return num + ((int3)math.round(this.initRenderScale * 1000f)).GetHashCode();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00014C9C File Offset: 0x00012E9C
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			if (this.transformArray != null)
			{
				foreach (Transform transform in this.transformArray)
				{
					if (transform)
					{
						transformSet.Add(transform);
					}
				}
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00014CDC File Offset: 0x00012EDC
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			if (this.transformArray == null)
			{
				return;
			}
			for (int i = 0; i < this.transformArray.Length; i++)
			{
				Transform transform = this.transformArray[i];
				if (transform && replaceDict.ContainsKey(transform.GetInstanceID()))
				{
					this.transformArray[i] = replaceDict[transform.GetInstanceID()];
				}
			}
		}

		// Token: 0x04000400 RID: 1024
		public RenderSetupData.SetupType setupType;

		// Token: 0x04000401 RID: 1025
		public int vertexCount;

		// Token: 0x04000402 RID: 1026
		public bool hasSkinnedMesh;

		// Token: 0x04000403 RID: 1027
		public bool hasBoneWeight;

		// Token: 0x04000404 RID: 1028
		public int skinRootBoneIndex;

		// Token: 0x04000405 RID: 1029
		public int renderTransformIndex;

		// Token: 0x04000406 RID: 1030
		public int skinBoneCount;

		// Token: 0x04000407 RID: 1031
		public int transformCount;

		// Token: 0x04000408 RID: 1032
		public int useTransformCount;

		// Token: 0x04000409 RID: 1033
		public int[] useTransformIndexArray;

		// Token: 0x0400040A RID: 1034
		public Transform[] transformArray;

		// Token: 0x0400040B RID: 1035
		public float3[] transformPositions;

		// Token: 0x0400040C RID: 1036
		public quaternion[] transformRotations;

		// Token: 0x0400040D RID: 1037
		public float3[] transformLocalPositions;

		// Token: 0x0400040E RID: 1038
		public quaternion[] transformLocalRotations;

		// Token: 0x0400040F RID: 1039
		public float3[] transformScales;

		// Token: 0x04000410 RID: 1040
		public float4x4 initRenderLocalToWorld;

		// Token: 0x04000411 RID: 1041
		public float4x4 initRenderWorldtoLocal;

		// Token: 0x04000412 RID: 1042
		public quaternion initRenderRotation;

		// Token: 0x04000413 RID: 1043
		public float3 initRenderScale;

		// Token: 0x04000414 RID: 1044
		public Mesh originalMesh;

		// Token: 0x02000092 RID: 146
		[BurstCompile]
		private struct CalcUseBoneArrayJob2 : IJob
		{
			// Token: 0x06000284 RID: 644 RVA: 0x00014D38 File Offset: 0x00012F38
			public void Execute()
			{
				NativeArray<byte> nativeArray = new NativeArray<byte>(this.boneCount, Allocator.Temp, NativeArrayOptions.ClearMemory);
				int length = this.boneWeightArray.Length;
				int num = 0;
				for (int i = 0; i < length; i++)
				{
					BoneWeight1 boneWeight = this.boneWeightArray[i];
					if (boneWeight.weight > 0f)
					{
						if (nativeArray[boneWeight.boneIndex] == 0)
						{
							num++;
						}
						nativeArray[boneWeight.boneIndex] = 1;
					}
				}
				for (int j = 0; j < this.boneCount; j++)
				{
					if (nativeArray[j] != 0)
					{
						this.useBoneIndexList.Add(in j);
					}
				}
				nativeArray.Dispose();
			}

			// Token: 0x04000415 RID: 1045
			public int boneCount;

			// Token: 0x04000416 RID: 1046
			[ReadOnly]
			public NativeArray<BoneWeight1> boneWeightArray;

			// Token: 0x04000417 RID: 1047
			public NativeList<int> useBoneIndexList;
		}
	}
}
