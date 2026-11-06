using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public class ClothInitSerializeData : ITransform
	{
		// Token: 0x06000010 RID: 16 RVA: 0x0000215C File Offset: 0x0000035C
		public bool HasData()
		{
			return this.initVersion != 0 && this.localHash != 0 && this.globalHash != 0;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000217C File Offset: 0x0000037C
		public void Clear()
		{
			this.initVersion = 0;
			this.localHash = 0;
			this.globalHash = 0;
			this.clothType = ClothProcess.ClothType.MeshCloth;
			this.clothTransformRecord = new TransformRecordSerializeData();
			this.normalAdjustmentTransformRecord = new TransformRecordSerializeData();
			this.customSkinningBoneRecords = new List<TransformRecordSerializeData>();
			this.clothSetupDataList = new List<RenderSetupSerializeData>();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000021D4 File Offset: 0x000003D4
		public ResultCode DataValidate(ClothProcess cprocess)
		{
			if (this.localHash == 0 || this.globalHash == 0)
			{
				return new ResultCode(Define.Result.InitSerializeData_InvalidHash);
			}
			if (this.initVersion == 0)
			{
				return new ResultCode(Define.Result.InitSerializeData_InvalidVersion);
			}
			if (this.clothSetupDataList == null || this.clothSetupDataList.Count == 0)
			{
				return new ResultCode(Define.Result.InitSerializeData_InvalidSetupData);
			}
			ClothSerializeData serializeData = cprocess.cloth.SerializeData;
			if (this.clothType != serializeData.clothType)
			{
				return new ResultCode(Define.Result.InitSerializeData_ClothTypeMismatch);
			}
			if (this.clothType == ClothProcess.ClothType.MeshCloth)
			{
				int count = serializeData.sourceRenderers.Count;
				if (this.clothSetupDataList.Count != count)
				{
					return new ResultCode(Define.Result.InitSerializeData_SetupCountMismatch);
				}
				for (int i = 0; i < count; i++)
				{
					if (!this.clothSetupDataList[i].DataValidateMeshCloth(serializeData.sourceRenderers[i]))
					{
						return new ResultCode(Define.Result.InitSerializeData_MeshClothSetupValidationError);
					}
				}
			}
			else if (this.clothType == ClothProcess.ClothType.BoneCloth)
			{
				if (this.clothSetupDataList.Count != 1)
				{
					return new ResultCode(Define.Result.InitSerializeData_SetupCountMismatch);
				}
				if (!this.clothSetupDataList[0].DataValidateBoneCloth(serializeData, RenderSetupData.SetupType.BoneCloth))
				{
					return new ResultCode(Define.Result.InitSerializeData_BoneClothSetupValidationError);
				}
			}
			else if (this.clothType == ClothProcess.ClothType.BoneSpring)
			{
				if (this.clothSetupDataList.Count != 1)
				{
					return new ResultCode(Define.Result.InitSerializeData_SetupCountMismatch);
				}
				if (!this.clothSetupDataList[0].DataValidateBoneCloth(serializeData, RenderSetupData.SetupType.BoneSpring))
				{
					return new ResultCode(Define.Result.InitSerializeData_BoneSpringSetupValidationError);
				}
			}
			if (serializeData.customSkinningSetting.skinningBones.Count != this.customSkinningBoneRecords.Count)
			{
				return new ResultCode(Define.Result.InitSerializeData_CustomSkinningBoneCountMismatch);
			}
			if (this.initVersion <= 1 && this.clothType == ClothProcess.ClothType.MeshCloth)
			{
				for (int j = 0; j < serializeData.sourceRenderers.Count; j++)
				{
					SkinnedMeshRenderer skinnedMeshRenderer = serializeData.sourceRenderers[j] as SkinnedMeshRenderer;
					if (skinnedMeshRenderer && skinnedMeshRenderer.sharedMesh && skinnedMeshRenderer.sharedMesh.name.Contains("(Clone)"))
					{
						return new ResultCode(Define.Result.InitSerializeData_InvalidCloneMesh);
					}
				}
			}
			return ResultCode.Success;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023E4 File Offset: 0x000005E4
		public bool Serialize(ClothSerializeData sdata, TransformRecord clothTransformRecord, TransformRecord normalAdjustmentTransformRecord, List<RenderSetupData> setupList)
		{
			this.initVersion = 2;
			this.clothType = sdata.clothType;
			this.clothTransformRecord = new TransformRecordSerializeData();
			this.clothTransformRecord.Serialize(clothTransformRecord);
			this.normalAdjustmentTransformRecord = new TransformRecordSerializeData();
			this.normalAdjustmentTransformRecord.Serialize(normalAdjustmentTransformRecord);
			this.customSkinningBoneRecords = new List<TransformRecordSerializeData>();
			int count = sdata.customSkinningSetting.skinningBones.Count;
			for (int i = 0; i < count; i++)
			{
				TransformRecord transformRecord = new TransformRecord(sdata.customSkinningSetting.skinningBones[i], true);
				TransformRecordSerializeData transformRecordSerializeData = new TransformRecordSerializeData();
				transformRecordSerializeData.Serialize(transformRecord);
				this.customSkinningBoneRecords.Add(transformRecordSerializeData);
			}
			this.clothSetupDataList = new List<RenderSetupSerializeData>();
			if (setupList != null && setupList.Count > 0)
			{
				foreach (RenderSetupData renderSetupData in setupList)
				{
					RenderSetupSerializeData renderSetupSerializeData = new RenderSetupSerializeData();
					renderSetupSerializeData.Serialize(renderSetupData);
					this.clothSetupDataList.Add(renderSetupSerializeData);
				}
			}
			this.localHash = this.GetLocalHash();
			this.globalHash = this.GetGlobalHash();
			return true;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002518 File Offset: 0x00000718
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			TransformRecordSerializeData transformRecordSerializeData = this.clothTransformRecord;
			if (transformRecordSerializeData != null)
			{
				transformRecordSerializeData.GetUsedTransform(transformSet);
			}
			TransformRecordSerializeData transformRecordSerializeData2 = this.normalAdjustmentTransformRecord;
			if (transformRecordSerializeData2 != null)
			{
				transformRecordSerializeData2.GetUsedTransform(transformSet);
			}
			List<TransformRecordSerializeData> list = this.customSkinningBoneRecords;
			if (list != null)
			{
				list.ForEach(delegate(TransformRecordSerializeData x)
				{
					x.GetUsedTransform(transformSet);
				});
			}
			List<RenderSetupSerializeData> list2 = this.clothSetupDataList;
			if (list2 == null)
			{
				return;
			}
			list2.ForEach(delegate(RenderSetupSerializeData x)
			{
				x.GetUsedTransform(transformSet);
			});
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000259C File Offset: 0x0000079C
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			TransformRecordSerializeData transformRecordSerializeData = this.clothTransformRecord;
			if (transformRecordSerializeData != null)
			{
				transformRecordSerializeData.ReplaceTransform(replaceDict);
			}
			TransformRecordSerializeData transformRecordSerializeData2 = this.normalAdjustmentTransformRecord;
			if (transformRecordSerializeData2 != null)
			{
				transformRecordSerializeData2.ReplaceTransform(replaceDict);
			}
			List<TransformRecordSerializeData> list = this.customSkinningBoneRecords;
			if (list != null)
			{
				list.ForEach(delegate(TransformRecordSerializeData x)
				{
					x.ReplaceTransform(replaceDict);
				});
			}
			List<RenderSetupSerializeData> list2 = this.clothSetupDataList;
			if (list2 == null)
			{
				return;
			}
			list2.ForEach(delegate(RenderSetupSerializeData x)
			{
				x.ReplaceTransform(replaceDict);
			});
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002620 File Offset: 0x00000820
		private int GetLocalHash()
		{
			ClothInitSerializeData.<>c__DisplayClass15_0 CS$<>8__locals1 = new ClothInitSerializeData.<>c__DisplayClass15_0();
			CS$<>8__locals1.hash = 0;
			CS$<>8__locals1.hash += this.initVersion * 9876;
			CS$<>8__locals1.hash = (int)(CS$<>8__locals1.hash + this.clothType * (ClothProcess.ClothType)5656);
			ClothInitSerializeData.<>c__DisplayClass15_0 CS$<>8__locals2 = CS$<>8__locals1;
			int hash = CS$<>8__locals1.hash;
			TransformRecordSerializeData transformRecordSerializeData = this.clothTransformRecord;
			CS$<>8__locals2.hash = hash + ((transformRecordSerializeData != null) ? transformRecordSerializeData.GetLocalHash() : 0);
			ClothInitSerializeData.<>c__DisplayClass15_0 CS$<>8__locals3 = CS$<>8__locals1;
			int hash2 = CS$<>8__locals1.hash;
			TransformRecordSerializeData transformRecordSerializeData2 = this.normalAdjustmentTransformRecord;
			CS$<>8__locals3.hash = hash2 + ((transformRecordSerializeData2 != null) ? transformRecordSerializeData2.GetLocalHash() : 0);
			List<TransformRecordSerializeData> list = this.customSkinningBoneRecords;
			if (list != null)
			{
				list.ForEach(delegate(TransformRecordSerializeData x)
				{
					CS$<>8__locals1.hash += ((x != null) ? x.GetLocalHash() : 0);
				});
			}
			List<RenderSetupSerializeData> list2 = this.clothSetupDataList;
			if (list2 != null)
			{
				list2.ForEach(delegate(RenderSetupSerializeData x)
				{
					CS$<>8__locals1.hash += ((x != null) ? x.GetLocalHash() : 0);
				});
			}
			return CS$<>8__locals1.hash;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026EC File Offset: 0x000008EC
		private int GetGlobalHash()
		{
			ClothInitSerializeData.<>c__DisplayClass16_0 CS$<>8__locals1 = new ClothInitSerializeData.<>c__DisplayClass16_0();
			CS$<>8__locals1.hash = 0;
			ClothInitSerializeData.<>c__DisplayClass16_0 CS$<>8__locals2 = CS$<>8__locals1;
			int hash = CS$<>8__locals1.hash;
			TransformRecordSerializeData transformRecordSerializeData = this.clothTransformRecord;
			CS$<>8__locals2.hash = hash + ((transformRecordSerializeData != null) ? transformRecordSerializeData.GetGlobalHash() : 0);
			ClothInitSerializeData.<>c__DisplayClass16_0 CS$<>8__locals3 = CS$<>8__locals1;
			int hash2 = CS$<>8__locals1.hash;
			TransformRecordSerializeData transformRecordSerializeData2 = this.normalAdjustmentTransformRecord;
			CS$<>8__locals3.hash = hash2 + ((transformRecordSerializeData2 != null) ? transformRecordSerializeData2.GetGlobalHash() : 0);
			List<TransformRecordSerializeData> list = this.customSkinningBoneRecords;
			if (list != null)
			{
				list.ForEach(delegate(TransformRecordSerializeData x)
				{
					CS$<>8__locals1.hash += ((x != null) ? x.GetGlobalHash() : 0);
				});
			}
			List<RenderSetupSerializeData> list2 = this.clothSetupDataList;
			if (list2 != null)
			{
				list2.ForEach(delegate(RenderSetupSerializeData x)
				{
					CS$<>8__locals1.hash += ((x != null) ? x.GetGlobalHash() : 0);
				});
			}
			return CS$<>8__locals1.hash;
		}

		// Token: 0x0400000F RID: 15
		public const int InitDataVersion = 2;

		// Token: 0x04000010 RID: 16
		public int initVersion;

		// Token: 0x04000011 RID: 17
		public int localHash;

		// Token: 0x04000012 RID: 18
		public int globalHash;

		// Token: 0x04000013 RID: 19
		public ClothProcess.ClothType clothType;

		// Token: 0x04000014 RID: 20
		public TransformRecordSerializeData clothTransformRecord;

		// Token: 0x04000015 RID: 21
		public TransformRecordSerializeData normalAdjustmentTransformRecord;

		// Token: 0x04000016 RID: 22
		public List<TransformRecordSerializeData> customSkinningBoneRecords;

		// Token: 0x04000017 RID: 23
		public List<RenderSetupSerializeData> clothSetupDataList;
	}
}
