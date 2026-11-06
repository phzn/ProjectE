using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Unity.Entities.Content
{
	// Token: 0x0200046B RID: 1131
	internal class DownloadOperationHttpClient : ContentDownloadService.DownloadOperation
	{
		// Token: 0x06001BFF RID: 7167 RVA: 0x00061E97 File Offset: 0x00060097
		public DownloadOperationHttpClient(HttpClient client)
		{
			this.httpClient = client;
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x00061EA6 File Offset: 0x000600A6
		protected override void StartDownload(string remotePath, string localTmpPath)
		{
			this.targetPath = localTmpPath;
			this.downloadTask = this.httpClient.GetStreamAsync(remotePath);
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x00002287 File Offset: 0x00000487
		protected override void CancelDownload()
		{
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x00061EC4 File Offset: 0x000600C4
		protected override bool ProcessDownload(ref long downloadedBytes, ref string error)
		{
			if (this.downloadTask.IsCompleted)
			{
				if (this.writeStream == null)
				{
					this.writeStream = File.OpenWrite(this.targetPath);
					this.writeTask = this.downloadTask.Result.CopyToAsync(this.writeStream);
				}
				if (this.writeTask.IsCompleted)
				{
					downloadedBytes = this.writeStream.Length;
					this.writeStream.Dispose();
					this.downloadTask.Result.Dispose();
					return true;
				}
			}
			return false;
		}

		// Token: 0x040009D7 RID: 2519
		private HttpClient httpClient;

		// Token: 0x040009D8 RID: 2520
		private Task<Stream> downloadTask;

		// Token: 0x040009D9 RID: 2521
		private Task writeTask;

		// Token: 0x040009DA RID: 2522
		private FileStream writeStream;

		// Token: 0x040009DB RID: 2523
		private string targetPath;
	}
}
