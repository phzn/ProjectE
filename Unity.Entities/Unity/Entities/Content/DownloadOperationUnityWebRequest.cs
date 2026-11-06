using System;
using System.IO;
using UnityEngine.Networking;

namespace Unity.Entities.Content
{
	// Token: 0x0200046A RID: 1130
	internal class DownloadOperationUnityWebRequest : ContentDownloadService.DownloadOperation
	{
		// Token: 0x06001BFB RID: 7163 RVA: 0x00061D9C File Offset: 0x0005FF9C
		protected override void StartDownload(string remotePath, string localTmpPath)
		{
			this.targetPath = localTmpPath;
			UnityWebRequest unityWebRequest = new UnityWebRequest(remotePath, "GET", new DownloadHandlerFile(localTmpPath, false)
			{
				removeFileOnAbort = true
			}, null);
			this.operation = unityWebRequest.SendWebRequest();
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x00061DDC File Offset: 0x0005FFDC
		protected override bool ProcessDownload(ref long downloadedBytes, ref string error)
		{
			downloadedBytes = (long)this.operation.webRequest.downloadedBytes;
			if (this.operation.isDone)
			{
				error = this.operation.webRequest.error;
				this.operation.webRequest.Dispose();
				return true;
			}
			return false;
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x00061E30 File Offset: 0x00060030
		protected override void CancelDownload()
		{
			if (this.operation != null && !this.operation.webRequest.isDone)
			{
				this.operation.webRequest.Abort();
				this.operation.webRequest.Dispose();
				if (File.Exists(this.targetPath))
				{
					File.Delete(this.targetPath);
				}
			}
		}

		// Token: 0x040009D5 RID: 2517
		private string targetPath;

		// Token: 0x040009D6 RID: 2518
		private UnityWebRequestAsyncOperation operation;
	}
}
