using System;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000079 RID: 121
	public class PreBuildManager : IManager, IDisposable, IValid
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x00010CE4 File Offset: 0x0000EEE4
		public void Dispose()
		{
			foreach (KeyValuePair<SharePreBuildData, PreBuildManager.ShareDeserializationData> keyValuePair in this.deserializationDict)
			{
				keyValuePair.Value.Dispose();
			}
			this.deserializationDict.Clear();
			this.isValid = false;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00010D50 File Offset: 0x0000EF50
		public void EnterdEditMode()
		{
			this.Dispose();
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00010D58 File Offset: 0x0000EF58
		public void Initialize()
		{
			this.isValid = true;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00010D61 File Offset: 0x0000EF61
		public bool IsValid()
		{
			return this.isValid;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00010D6C File Offset: 0x0000EF6C
		public void InformationLog(StringBuilder allsb)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("========== PreBuild Manager ==========");
			if (!this.IsValid())
			{
				stringBuilder.AppendLine("PreBuild Manager. Invalid.");
			}
			else
			{
				int count = this.deserializationDict.Count;
				stringBuilder.AppendLine(string.Format("Count:{0}", count));
				foreach (KeyValuePair<SharePreBuildData, PreBuildManager.ShareDeserializationData> keyValuePair in this.deserializationDict)
				{
					stringBuilder.AppendLine(string.Format("[{0}] refcnt:{1}, result:{2}, proxyMesh:{3}", new object[]
					{
						keyValuePair.Key.buildId,
						keyValuePair.Value.referenceCount,
						keyValuePair.Value.result.GetResultString(),
						keyValuePair.Value.proxyMesh != null
					}));
				}
			}
			stringBuilder.AppendLine();
			Debug.Log(stringBuilder.ToString());
			allsb.Append(stringBuilder);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00010E88 File Offset: 0x0000F088
		internal PreBuildManager.ShareDeserializationData RegisterPreBuildData(SharePreBuildData sdata, bool referenceIncrement)
		{
			if (!this.isValid)
			{
				return null;
			}
			if (sdata == null)
			{
				return null;
			}
			if (!this.deserializationDict.ContainsKey(sdata))
			{
				PreBuildManager.ShareDeserializationData shareDeserializationData = new PreBuildManager.ShareDeserializationData();
				shareDeserializationData.buildId = sdata.buildId;
				shareDeserializationData.Deserialize(sdata);
				this.deserializationDict.Add(sdata, shareDeserializationData);
			}
			PreBuildManager.ShareDeserializationData shareDeserializationData2 = this.deserializationDict[sdata];
			if (referenceIncrement)
			{
				shareDeserializationData2.referenceCount++;
			}
			return shareDeserializationData2;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00010EF7 File Offset: 0x0000F0F7
		internal PreBuildManager.ShareDeserializationData GetPreBuildData(SharePreBuildData sdata)
		{
			if (sdata == null)
			{
				return null;
			}
			if (this.deserializationDict.ContainsKey(sdata))
			{
				return this.deserializationDict[sdata];
			}
			return null;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00010F1A File Offset: 0x0000F11A
		internal void UnregisterPreBuildData(SharePreBuildData sdata)
		{
			if (!this.isValid)
			{
				return;
			}
			if (sdata == null)
			{
				return;
			}
			if (this.deserializationDict.ContainsKey(sdata))
			{
				this.deserializationDict[sdata].referenceCount--;
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00010F50 File Offset: 0x0000F150
		internal void UnloadUnusedData()
		{
			List<SharePreBuildData> list = new List<SharePreBuildData>();
			foreach (KeyValuePair<SharePreBuildData, PreBuildManager.ShareDeserializationData> keyValuePair in this.deserializationDict)
			{
				if (keyValuePair.Value.referenceCount <= 0)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (SharePreBuildData sharePreBuildData in list)
			{
				this.deserializationDict[sharePreBuildData].Dispose();
				this.deserializationDict.Remove(sharePreBuildData);
			}
			list.Clear();
		}

		// Token: 0x04000352 RID: 850
		private Dictionary<SharePreBuildData, PreBuildManager.ShareDeserializationData> deserializationDict = new Dictionary<SharePreBuildData, PreBuildManager.ShareDeserializationData>();

		// Token: 0x04000353 RID: 851
		private bool isValid;

		// Token: 0x04000354 RID: 852
		private static readonly ProfilerMarker deserializationProfiler = new ProfilerMarker("PreBuild.Deserialization");

		// Token: 0x0200007A RID: 122
		internal class ShareDeserializationData : IDisposable
		{
			// Token: 0x060001DC RID: 476 RVA: 0x00011040 File Offset: 0x0000F240
			public void Dispose()
			{
				foreach (RenderSetupData renderSetupData in this.renderSetupDataList)
				{
					if (renderSetupData != null)
					{
						renderSetupData.isManaged = false;
						renderSetupData.Dispose();
					}
				}
				this.renderSetupDataList.Clear();
				if (this.proxyMesh != null)
				{
					this.proxyMesh.isManaged = false;
					this.proxyMesh.Dispose();
					this.proxyMesh = null;
				}
				foreach (VirtualMesh virtualMesh in this.renderMeshList)
				{
					if (virtualMesh != null)
					{
						virtualMesh.isManaged = false;
						virtualMesh.Dispose();
					}
				}
				this.renderMeshList.Clear();
				this.distanceConstraintData = null;
				this.bendingConstraintData = null;
				this.inertiaConstraintData = null;
				this.buildId = string.Empty;
				this.result.Clear();
				this.referenceCount = 0;
			}

			// Token: 0x060001DD RID: 477 RVA: 0x00011158 File Offset: 0x0000F358
			public void Deserialize(SharePreBuildData sharePreBuilddata)
			{
				this.result.SetProcess();
				try
				{
					ResultCode resultCode = sharePreBuilddata.DataValidate();
					if (resultCode.IsFaild())
					{
						this.result.Merge(resultCode);
						throw new MagicaClothProcessingException();
					}
					foreach (RenderSetupData.ShareSerializationData shareSerializationData in sharePreBuilddata.renderSetupDataList)
					{
						this.renderSetupDataList.Add(RenderSetupData.ShareDeserialize(shareSerializationData));
					}
					this.proxyMesh = VirtualMesh.ShareDeserialize(sharePreBuilddata.proxyMesh);
					foreach (VirtualMesh.ShareSerializationData shareSerializationData2 in sharePreBuilddata.renderMeshList)
					{
						this.renderMeshList.Add(VirtualMesh.ShareDeserialize(shareSerializationData2));
					}
					this.distanceConstraintData = sharePreBuilddata.distanceConstraintData;
					this.bendingConstraintData = sharePreBuilddata.bendingConstraintData;
					this.inertiaConstraintData = sharePreBuilddata.inertiaConstraintData;
					this.result.SetSuccess();
				}
				catch (MagicaClothProcessingException)
				{
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					this.result.SetError(Define.Result.Deserialization_Exception);
				}
			}

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x060001DE RID: 478 RVA: 0x000112A4 File Offset: 0x0000F4A4
			public int RenderMeshCount
			{
				get
				{
					List<VirtualMesh> list = this.renderMeshList;
					if (list == null)
					{
						return 0;
					}
					return list.Count;
				}
			}

			// Token: 0x060001DF RID: 479 RVA: 0x000112B7 File Offset: 0x0000F4B7
			public VirtualMeshContainer GetProxyMeshContainer()
			{
				return new VirtualMeshContainer
				{
					shareVirtualMesh = this.proxyMesh,
					uniqueData = null
				};
			}

			// Token: 0x060001E0 RID: 480 RVA: 0x000112D1 File Offset: 0x0000F4D1
			public VirtualMeshContainer GetRenderMeshContainer(int index)
			{
				if (index >= this.RenderMeshCount)
				{
					return null;
				}
				return new VirtualMeshContainer
				{
					shareVirtualMesh = this.renderMeshList[index],
					uniqueData = null
				};
			}

			// Token: 0x04000355 RID: 853
			internal string buildId;

			// Token: 0x04000356 RID: 854
			internal ResultCode result;

			// Token: 0x04000357 RID: 855
			internal int referenceCount;

			// Token: 0x04000358 RID: 856
			internal List<RenderSetupData> renderSetupDataList = new List<RenderSetupData>();

			// Token: 0x04000359 RID: 857
			internal VirtualMesh proxyMesh;

			// Token: 0x0400035A RID: 858
			internal List<VirtualMesh> renderMeshList = new List<VirtualMesh>();

			// Token: 0x0400035B RID: 859
			internal DistanceConstraint.ConstraintData distanceConstraintData;

			// Token: 0x0400035C RID: 860
			internal TriangleBendingConstraint.ConstraintData bendingConstraintData;

			// Token: 0x0400035D RID: 861
			internal InertiaConstraint.ConstraintData inertiaConstraintData;
		}
	}
}
