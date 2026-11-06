using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Diagnostics;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.AddressableAssets.Utility
{
	// Token: 0x0200001D RID: 29
	internal class ResourceManagerDiagnostics : IDisposable
	{
		// Token: 0x06000185 RID: 389 RVA: 0x00006CBA File Offset: 0x00004EBA
		public ResourceManagerDiagnostics(ResourceManager resourceManager)
		{
			resourceManager.RegisterDiagnosticCallback(new Action<ResourceManager.DiagnosticEventContext>(this.OnResourceManagerDiagnosticEvent));
			this.m_ResourceManager = resourceManager;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006CE8 File Offset: 0x00004EE8
		internal int SumDependencyNameHashCodes(AsyncOperationHandle handle)
		{
			List<AsyncOperationHandle> list = new List<AsyncOperationHandle>();
			handle.GetDependencies(list);
			int num = 0;
			foreach (AsyncOperationHandle asyncOperationHandle in list)
			{
				if (asyncOperationHandle.IsValid())
				{
					num += asyncOperationHandle.DebugName.GetHashCode() + this.SumDependencyNameHashCodes(asyncOperationHandle);
				}
			}
			return num;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006D60 File Offset: 0x00004F60
		internal int CalculateHashCode(AsyncOperationHandle handle)
		{
			if (handle.DebugName.Contains("CompletedOperation"))
			{
				return this.CalculateCompletedOperationHashcode(handle);
			}
			int num = this.SumDependencyNameHashCodes(handle);
			if (handle.DebugName.Contains("result=") && handle.DebugName.Contains("status="))
			{
				return handle.GetHashCode();
			}
			return handle.DebugName.GetHashCode() + num;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00006DD5 File Offset: 0x00004FD5
		internal int CalculateCompletedOperationHashcode(AsyncOperationHandle handle)
		{
			if (handle.Result == null)
			{
				return handle.GetHashCode();
			}
			return handle.Result.GetHashCode() + handle.Result.GetType().GetHashCode();
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00006E0C File Offset: 0x0000500C
		internal string GenerateCompletedOperationDisplayName(AsyncOperationHandle handle)
		{
			if (handle.Result == null)
			{
				return handle.DebugName;
			}
			if (handle.Result.GetType().IsGenericType)
			{
				IList list = handle.Result as IList;
				if (list != null)
				{
					string text = handle.DebugName;
					if (list.Count > 0)
					{
						StringBuilder stringBuilder = new StringBuilder("[");
						int num = 0;
						while (num < list.Count && num < 4)
						{
							object obj = list[num];
							if (30 <= obj.ToString().Length)
							{
								stringBuilder.Append(obj.ToString().Substring(0, 30));
								stringBuilder.Append("..., ");
							}
							else
							{
								stringBuilder.Append(obj.ToString().Substring(0, obj.ToString().Length));
								stringBuilder.Append(", ");
							}
							num++;
						}
						stringBuilder.Remove(stringBuilder.Length - 2, 2);
						stringBuilder.Append("]");
						text = stringBuilder.ToString();
					}
					return handle.DebugName + " Result type: List, result: " + text;
				}
			}
			string debugName = handle.DebugName;
			string text2 = " Result type: ";
			Type type = handle.Result.GetType();
			return debugName + text2 + ((type != null) ? type.ToString() : null);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006F50 File Offset: 0x00005150
		private void OnResourceManagerDiagnosticEvent(ResourceManager.DiagnosticEventContext eventContext)
		{
			int num = this.CalculateHashCode(eventContext.OperationHandle);
			DiagnosticInfo diagnosticInfo = null;
			if (eventContext.Type == ResourceManager.DiagnosticEventType.AsyncOperationDestroy)
			{
				if (this.m_cachedDiagnosticInfo.TryGetValue(num, out diagnosticInfo))
				{
					this.m_cachedDiagnosticInfo.Remove(num);
				}
			}
			else if (!this.m_cachedDiagnosticInfo.TryGetValue(num, out diagnosticInfo))
			{
				List<AsyncOperationHandle> list = new List<AsyncOperationHandle>();
				eventContext.OperationHandle.GetDependencies(list);
				int[] array = new int[list.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.CalculateHashCode(list[i]);
				}
				if (eventContext.OperationHandle.DebugName.Contains("CompletedOperation"))
				{
					string text = this.GenerateCompletedOperationDisplayName(eventContext.OperationHandle);
					Dictionary<int, DiagnosticInfo> cachedDiagnosticInfo = this.m_cachedDiagnosticInfo;
					int num2 = num;
					DiagnosticInfo diagnosticInfo2 = new DiagnosticInfo();
					diagnosticInfo2.ObjectId = num;
					diagnosticInfo2.DisplayName = text;
					diagnosticInfo2.Dependencies = array;
					diagnosticInfo = diagnosticInfo2;
					cachedDiagnosticInfo.Add(num2, diagnosticInfo2);
				}
				else
				{
					Dictionary<int, DiagnosticInfo> cachedDiagnosticInfo2 = this.m_cachedDiagnosticInfo;
					int num3 = num;
					DiagnosticInfo diagnosticInfo3 = new DiagnosticInfo();
					diagnosticInfo3.ObjectId = num;
					diagnosticInfo3.DisplayName = eventContext.OperationHandle.DebugName;
					diagnosticInfo3.Dependencies = array;
					diagnosticInfo = diagnosticInfo3;
					cachedDiagnosticInfo2.Add(num3, diagnosticInfo3);
				}
			}
			if (diagnosticInfo != null)
			{
				ComponentSingleton<DiagnosticEventCollectorSingleton>.Instance.PostEvent(diagnosticInfo.CreateEvent("ResourceManager", eventContext.Type, Time.frameCount, eventContext.EventValue));
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000070AE File Offset: 0x000052AE
		public void Dispose()
		{
			ResourceManager resourceManager = this.m_ResourceManager;
			if (resourceManager != null)
			{
				resourceManager.UnregisterDiagnosticCallback(new Action<ResourceManager.DiagnosticEventContext>(this.OnResourceManagerDiagnosticEvent));
			}
			if (ComponentSingleton<DiagnosticEventCollectorSingleton>.Exists)
			{
				ComponentSingleton<DiagnosticEventCollectorSingleton>.DestroySingleton();
			}
		}

		// Token: 0x0400004D RID: 77
		private ResourceManager m_ResourceManager;

		// Token: 0x0400004E RID: 78
		private const int k_NumberOfCompletedOpResultEntriesToShow = 4;

		// Token: 0x0400004F RID: 79
		private const int k_MaximumCompletedOpResultEntryLength = 30;

		// Token: 0x04000050 RID: 80
		private Dictionary<int, DiagnosticInfo> m_cachedDiagnosticInfo = new Dictionary<int, DiagnosticInfo>();
	}
}
