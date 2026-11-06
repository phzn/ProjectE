using System;
using System.Text;
using UnityEngine.Networking;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x0200002B RID: 43
	public class UnityWebRequestResult
	{
		// Token: 0x06000115 RID: 277 RVA: 0x000053D4 File Offset: 0x000035D4
		public UnityWebRequestResult(UnityWebRequest request)
		{
			string text = request.error;
			if (request.result == UnityWebRequest.Result.DataProcessingError && request.downloadHandler != null)
			{
				text = text + " : " + request.downloadHandler.error;
			}
			this.Result = request.result;
			this.Error = text;
			this.ResponseCode = request.responseCode;
			this.Method = request.method;
			this.Url = request.url;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005450 File Offset: 0x00003650
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("{0} : {1}", this.Result, this.Error));
			if (this.ResponseCode > 0L)
			{
				stringBuilder.AppendLine(string.Format("ResponseCode : {0}, Method : {1}", this.ResponseCode, this.Method));
			}
			stringBuilder.AppendLine("url : " + this.Url);
			return stringBuilder.ToString();
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000117 RID: 279 RVA: 0x000054CE File Offset: 0x000036CE
		// (set) Token: 0x06000118 RID: 280 RVA: 0x000054D6 File Offset: 0x000036D6
		public string Error { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000119 RID: 281 RVA: 0x000054DF File Offset: 0x000036DF
		public long ResponseCode { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000054E7 File Offset: 0x000036E7
		public UnityWebRequest.Result Result { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000054EF File Offset: 0x000036EF
		public string Method { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000054F7 File Offset: 0x000036F7
		public string Url { get; }

		// Token: 0x0600011D RID: 285 RVA: 0x00005500 File Offset: 0x00003700
		public bool ShouldRetryDownloadError()
		{
			return string.IsNullOrEmpty(this.Error) || (!(this.Error == "Request aborted") && !(this.Error == "Unable to write data") && !(this.Error == "Malformed URL") && !(this.Error == "Out of memory") && !(this.Error == "Encountered invalid redirect (missing Location header?)") && !(this.Error == "Cannot modify request at this time") && !(this.Error == "Unsupported Protocol") && !(this.Error == "Destination host has an erroneous SSL certificate") && !(this.Error == "Unable to load SSL Cipher for verification") && !(this.Error == "SSL CA certificate error") && !(this.Error == "Unrecognized content-encoding") && !(this.Error == "Request already transmitted") && !(this.Error == "Invalid HTTP Method") && !(this.Error == "Header name contains invalid characters") && !(this.Error == "Header value contains invalid characters") && !(this.Error == "Cannot override system-specified headers") && !(this.Error == "Insecure connection not allowed"));
		}
	}
}
