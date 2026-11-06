using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200003A RID: 58
	public class GPUILODGroupDataProvider : GPUIDataProvider<int, GPUILODGroupData>
	{
		// Token: 0x0600023B RID: 571 RVA: 0x0000BFBF File Offset: 0x0000A1BF
		public override void Initialize()
		{
			base.Initialize();
			if (this._generatedLODGroups == null)
			{
				this._generatedLODGroups = new List<GPUILODGroupData>();
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000BFDA File Offset: 0x0000A1DA
		public override void Dispose()
		{
			base.Dispose();
			this.DestroyGeneratedLODGroups();
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
		public void DestroyGeneratedLODGroups()
		{
			if (this._generatedLODGroups != null)
			{
				foreach (GPUILODGroupData gpuilodgroupData in this._generatedLODGroups)
				{
					gpuilodgroupData.Dispose();
					gpuilodgroupData.DestroyGeneric();
				}
				this._generatedLODGroups.Clear();
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000C054 File Offset: 0x0000A254
		public void RegenerateLODGroups()
		{
			if (base.IsInitialized)
			{
				foreach (KeyValuePair<int, GPUILODGroupData> keyValuePair in this._dataDict)
				{
					if (keyValuePair.Value != null && keyValuePair.Value.prototype != null)
					{
						keyValuePair.Value.CreateRenderersFromPrototype(keyValuePair.Value.prototype);
					}
				}
				GPUIRenderingSystem.Instance.UpdateCommandBuffers(true);
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000C0EC File Offset: 0x0000A2EC
		public void RecalculateLODGroupBounds()
		{
			if (base.IsInitialized)
			{
				foreach (KeyValuePair<int, GPUILODGroupData> keyValuePair in this._dataDict)
				{
					if (keyValuePair.Value != null && keyValuePair.Value.prototype != null)
					{
						keyValuePair.Value.CalculateBounds();
					}
				}
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000C16C File Offset: 0x0000A36C
		public void RegenerateLODGroupData(GPUIPrototype prototype)
		{
			if (base.IsInitialized)
			{
				GPUILODGroupData orCreateLODGroupData = this.GetOrCreateLODGroupData(prototype);
				if (orCreateLODGroupData != null)
				{
					orCreateLODGroupData.CreateRenderersFromPrototype(prototype);
					orCreateLODGroupData.SetParameterBufferData();
					GPUIRenderingSystem.Instance.UpdateCommandBuffers(false);
				}
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000C1AC File Offset: 0x0000A3AC
		public GPUILODGroupData GetOrCreateLODGroupData(GPUIPrototype prototype)
		{
			if (!base.IsInitialized)
			{
				this.Initialize();
			}
			int key = prototype.GetKey();
			GPUILODGroupData gpuilodgroupData;
			if (!this.TryGetData(key, out gpuilodgroupData) || gpuilodgroupData == null)
			{
				if (prototype.prototypeType == GPUIPrototypeType.LODGroupData)
				{
					gpuilodgroupData = prototype.gpuiLODGroupData;
				}
				else
				{
					gpuilodgroupData = GPUILODGroupData.CreateLODGroupData(prototype);
					this._generatedLODGroups.Add(gpuilodgroupData);
				}
				this._dataDict[key] = gpuilodgroupData;
			}
			return gpuilodgroupData;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000C218 File Offset: 0x0000A418
		public GameObject FindPrefabObjectFromPrefabID(int prefabID)
		{
			if (!base.IsInitialized)
			{
				return null;
			}
			foreach (GPUILODGroupData gpuilodgroupData in this._dataDict.Values)
			{
				GPUIPrefabBase gpuiprefabBase;
				if (gpuilodgroupData.prototype != null && !(gpuilodgroupData.prototype.prefabObject == null) && gpuilodgroupData.prototype.prefabObject.TryGetComponent<GPUIPrefabBase>(out gpuiprefabBase) && gpuiprefabBase.GetPrefabID() == prefabID)
				{
					return gpuilodgroupData.prototype.prefabObject;
				}
			}
			return null;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000C2BC File Offset: 0x0000A4BC
		public void ClearNullValues()
		{
			if (!base.IsInitialized)
			{
				return;
			}
			for (int i = 0; i < base.Count; i++)
			{
				KeyValuePair<int, GPUILODGroupData> kvpairAtIndex = this.GetKVPairAtIndex(i);
				if (kvpairAtIndex.Value == null)
				{
					this.Remove(kvpairAtIndex.Key);
					i--;
				}
			}
		}

		// Token: 0x0400016D RID: 365
		private List<GPUILODGroupData> _generatedLODGroups;
	}
}
