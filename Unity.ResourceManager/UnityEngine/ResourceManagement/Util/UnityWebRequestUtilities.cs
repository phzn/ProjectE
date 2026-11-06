using System;
using UnityEngine.Networking;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x0200002A RID: 42
	public class UnityWebRequestUtilities
	{
		// Token: 0x06000112 RID: 274 RVA: 0x0000533C File Offset: 0x0000353C
		public static bool RequestHasErrors(UnityWebRequest webReq, out UnityWebRequestResult result)
		{
			result = null;
			if (webReq == null || !webReq.isDone)
			{
				return false;
			}
			UnityWebRequest.Result result2 = webReq.result;
			if (result2 <= UnityWebRequest.Result.Success)
			{
				return false;
			}
			if (result2 - UnityWebRequest.Result.ConnectionError > 2)
			{
				throw new NotImplementedException(string.Format("Cannot determine whether UnityWebRequest succeeded or not from result : {0}", webReq.result));
			}
			result = new UnityWebRequestResult(webReq);
			return true;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005394 File Offset: 0x00003594
		public static bool IsAssetBundleDownloaded(UnityWebRequestAsyncOperation op)
		{
			DownloadHandlerAssetBundle downloadHandlerAssetBundle = (DownloadHandlerAssetBundle)op.webRequest.downloadHandler;
			if (downloadHandlerAssetBundle != null && downloadHandlerAssetBundle.autoLoadAssetBundle)
			{
				return downloadHandlerAssetBundle.isDownloadComplete;
			}
			return op.isDone;
		}
	}
}
