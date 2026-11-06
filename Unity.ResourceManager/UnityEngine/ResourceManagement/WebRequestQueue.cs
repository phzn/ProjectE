using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement
{
	// Token: 0x0200000D RID: 13
	public static class WebRequestQueue
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00003DE7 File Offset: 0x00001FE7
		public static void SetMaxConcurrentRequests(int maxRequests)
		{
			if (maxRequests < 1)
			{
				throw new ArgumentException("MaxRequests must be 1 or greater.", "maxRequests");
			}
			WebRequestQueue.s_MaxRequest = maxRequests;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003E04 File Offset: 0x00002004
		public static WebRequestQueueOperation QueueRequest(UnityWebRequest request)
		{
			WebRequestQueueOperation webRequestQueueOperation = new WebRequestQueueOperation(request);
			if (WebRequestQueue.s_ActiveRequests.Count < WebRequestQueue.s_MaxRequest)
			{
				WebRequestQueue.BeginWebRequest(webRequestQueueOperation);
			}
			else
			{
				WebRequestQueue.s_QueuedOperations.Enqueue(webRequestQueueOperation);
			}
			return webRequestQueueOperation;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003E40 File Offset: 0x00002040
		public static void WaitForRequestToBeActive(WebRequestQueueOperation request, int millisecondsTimeout)
		{
			List<UnityWebRequestAsyncOperation> list = new List<UnityWebRequestAsyncOperation>();
			while (WebRequestQueue.s_QueuedOperations.Contains(request))
			{
				list.Clear();
				foreach (UnityWebRequestAsyncOperation unityWebRequestAsyncOperation in WebRequestQueue.s_ActiveRequests)
				{
					if (UnityWebRequestUtilities.IsAssetBundleDownloaded(unityWebRequestAsyncOperation))
					{
						list.Add(unityWebRequestAsyncOperation);
					}
				}
				foreach (UnityWebRequestAsyncOperation unityWebRequestAsyncOperation2 in list)
				{
					bool flag = WebRequestQueue.s_QueuedOperations.Peek() == request;
					unityWebRequestAsyncOperation2.completed -= WebRequestQueue.OnWebAsyncOpComplete;
					WebRequestQueue.OnWebAsyncOpComplete(unityWebRequestAsyncOperation2);
					if (flag)
					{
						return;
					}
				}
				Thread.Sleep(millisecondsTimeout);
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003F20 File Offset: 0x00002120
		internal static void DequeueRequest(UnityWebRequestAsyncOperation operation)
		{
			operation.completed -= WebRequestQueue.OnWebAsyncOpComplete;
			WebRequestQueue.OnWebAsyncOpComplete(operation);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003F3A File Offset: 0x0000213A
		private static void OnWebAsyncOpComplete(AsyncOperation operation)
		{
			WebRequestQueue.OnWebAsyncOpComplete(operation as UnityWebRequestAsyncOperation);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003F47 File Offset: 0x00002147
		private static void OnWebAsyncOpComplete(UnityWebRequestAsyncOperation operation)
		{
			if (WebRequestQueue.s_ActiveRequests.Remove(operation) && WebRequestQueue.s_QueuedOperations.Count > 0)
			{
				WebRequestQueue.BeginWebRequest(WebRequestQueue.s_QueuedOperations.Dequeue());
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003F74 File Offset: 0x00002174
		private static void BeginWebRequest(WebRequestQueueOperation queueOperation)
		{
			UnityWebRequest webRequest = queueOperation.m_WebRequest;
			UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = null;
			try
			{
				unityWebRequestAsyncOperation = webRequest.SendWebRequest();
				if (unityWebRequestAsyncOperation != null)
				{
					WebRequestQueue.s_ActiveRequests.Add(unityWebRequestAsyncOperation);
					if (unityWebRequestAsyncOperation.isDone)
					{
						WebRequestQueue.OnWebAsyncOpComplete(unityWebRequestAsyncOperation);
					}
					else
					{
						unityWebRequestAsyncOperation.completed += WebRequestQueue.OnWebAsyncOpComplete;
					}
				}
				else
				{
					WebRequestQueue.OnWebAsyncOpComplete(null);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
			}
			queueOperation.Complete(unityWebRequestAsyncOperation);
		}

		// Token: 0x04000038 RID: 56
		internal static int s_MaxRequest = 3;

		// Token: 0x04000039 RID: 57
		internal static Queue<WebRequestQueueOperation> s_QueuedOperations = new Queue<WebRequestQueueOperation>();

		// Token: 0x0400003A RID: 58
		internal static List<UnityWebRequestAsyncOperation> s_ActiveRequests = new List<UnityWebRequestAsyncOperation>();
	}
}
