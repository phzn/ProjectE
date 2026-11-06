using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.Initialization;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
	// Token: 0x02000006 RID: 6
	internal class InitalizationObjectsOperation : AsyncOperationBase<bool>
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002168 File Offset: 0x00000368
		public void Init(AsyncOperationHandle<ResourceManagerRuntimeData> rtdOp, AddressablesImpl addressables)
		{
			this.m_RtdOp = rtdOp;
			this.m_Addressables = addressables;
			this.m_Addressables.ResourceManager.RegisterForCallbacks();
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002188 File Offset: 0x00000388
		protected override string DebugName
		{
			get
			{
				return "InitializationObjectsOperation";
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002190 File Offset: 0x00000390
		internal bool LogRuntimeWarnings(string pathToBuildLogs)
		{
			if (!File.Exists(pathToBuildLogs))
			{
				return false;
			}
			PackedPlayModeBuildLogs packedPlayModeBuildLogs = JsonUtility.FromJson<PackedPlayModeBuildLogs>(File.ReadAllText(pathToBuildLogs));
			bool flag = false;
			foreach (PackedPlayModeBuildLogs.RuntimeBuildLog runtimeBuildLog in packedPlayModeBuildLogs.RuntimeBuildLogs)
			{
				flag = true;
				switch (runtimeBuildLog.Type)
				{
				case LogType.Error:
					Addressables.LogError(runtimeBuildLog.Message);
					break;
				case LogType.Warning:
					Addressables.LogWarning(runtimeBuildLog.Message);
					break;
				}
			}
			return flag;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000222C File Offset: 0x0000042C
		protected override bool InvokeWaitForCompletion()
		{
			if (base.IsDone)
			{
				return true;
			}
			if (this.m_RtdOp.IsValid() && !this.m_RtdOp.IsDone)
			{
				this.m_RtdOp.WaitForCompletion();
			}
			ResourceManager rm = this.m_RM;
			if (rm != null)
			{
				rm.Update(Time.unscaledDeltaTime);
			}
			if (!this.HasExecuted)
			{
				base.InvokeExecute();
			}
			if (this.m_DepOp.IsValid() && !this.m_DepOp.IsDone)
			{
				this.m_DepOp.WaitForCompletion();
			}
			ResourceManager rm2 = this.m_RM;
			if (rm2 != null)
			{
				rm2.Update(Time.unscaledDeltaTime);
			}
			return base.IsDone;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022D0 File Offset: 0x000004D0
		protected override void Execute()
		{
			ResourceManagerRuntimeData result = this.m_RtdOp.Result;
			if (result == null)
			{
				Addressables.LogError("RuntimeData is null.  Please ensure you have built the correct Player Content.");
				base.Complete(true, true, "");
				return;
			}
			List<AsyncOperationHandle> list = new List<AsyncOperationHandle>();
			foreach (ObjectInitializationData objectInitializationData in result.InitializationObjects)
			{
				if (!(objectInitializationData.ObjectType.Value == null))
				{
					try
					{
						AsyncOperationHandle asyncInitHandle = objectInitializationData.GetAsyncInitHandle(this.m_Addressables.ResourceManager, null);
						list.Add(asyncInitHandle);
					}
					catch (Exception ex)
					{
						Addressables.LogErrorFormat("Exception thrown during initialization of object {0}: {1}", new object[]
						{
							objectInitializationData,
							ex.ToString()
						});
					}
				}
			}
			if (list.Count > 0)
			{
				this.m_DepOp = this.m_Addressables.ResourceManager.CreateGenericGroupOperation(list, true);
				this.m_DepOp.Completed += delegate(AsyncOperationHandle<IList<AsyncOperationHandle>> obj)
				{
					bool flag = obj.Status == AsyncOperationStatus.Succeeded;
					base.Complete(true, flag, flag ? "" : string.Format("{0}, status={1}, result={2} failed initialization.", obj.DebugName, obj.Status, obj.Result));
					this.m_DepOp.Release();
				};
				return;
			}
			base.Complete(true, true, "");
		}

		// Token: 0x04000004 RID: 4
		private AsyncOperationHandle<ResourceManagerRuntimeData> m_RtdOp;

		// Token: 0x04000005 RID: 5
		private AddressablesImpl m_Addressables;

		// Token: 0x04000006 RID: 6
		private AsyncOperationHandle<IList<AsyncOperationHandle>> m_DepOp;
	}
}
