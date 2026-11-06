using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace DA_Assets.Networking
{
	// Token: 0x02000005 RID: 5
	public class UnityHttpClient : IDisposable
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000022D8 File Offset: 0x000004D8
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000022E0 File Offset: 0x000004E0
		public DownloadHandler downloadHandler { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022E9 File Offset: 0x000004E9
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000022F1 File Offset: 0x000004F1
		public long downloadedBytes { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022FA File Offset: 0x000004FA
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002302 File Offset: 0x00000502
		public float downloadProgress { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000230B File Offset: 0x0000050B
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002313 File Offset: 0x00000513
		public string error { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000231C File Offset: 0x0000051C
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002324 File Offset: 0x00000524
		public bool isDone { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000232D File Offset: 0x0000052D
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002335 File Offset: 0x00000535
		public HttpStatusCode responseCode { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000233E File Offset: 0x0000053E
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002346 File Offset: 0x00000546
		public WR_Result result { get; private set; }

		// Token: 0x0600001B RID: 27 RVA: 0x00002350 File Offset: 0x00000550
		private UnityHttpClient(HttpMethod method, string url, HttpContent content = null)
		{
			this._httpClient = new HttpClient
			{
				Timeout = TimeSpan.FromMinutes(5.0)
			};
			this._requestMessage = new HttpRequestMessage(method, url);
			if (content != null)
			{
				this._requestMessage.Content = content;
			}
			this.downloadHandler = new DownloadHandler();
			this.downloadedBytes = 0L;
			this.error = string.Empty;
			this.isDone = false;
			this.responseCode = (HttpStatusCode)0;
			this.result = WR_Result.InProgress;
			this.downloadProgress = 0f;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000023DC File Offset: 0x000005DC
		public static UnityHttpClient Get(string url)
		{
			return new UnityHttpClient(HttpMethod.Get, url, null);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000023EC File Offset: 0x000005EC
		public static UnityHttpClient Post(string url, WWWForm form)
		{
			ByteArrayContent byteArrayContent = new ByteArrayContent(form.data);
			foreach (KeyValuePair<string, string> keyValuePair in form.headers)
			{
				byteArrayContent.Headers.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return new UnityHttpClient(HttpMethod.Post, url, byteArrayContent);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000246C File Offset: 0x0000066C
		public void SetRequestHeader(string name, string value)
		{
			if (this._requestMessage.Headers.Contains(name))
			{
				this._requestMessage.Headers.Remove(name);
			}
			this._requestMessage.Headers.Add(name, value);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024A8 File Offset: 0x000006A8
		public async Task SendWebRequest(CancellationTokenSource cancellationTokenSource = null)
		{
			if (cancellationTokenSource == null)
			{
				this._cancellationTokenSource = new CancellationTokenSource();
			}
			else
			{
				this._cancellationTokenSource = cancellationTokenSource;
			}
			try
			{
				HttpResponseMessage httpResponseMessage = await this._httpClient.SendAsync(this._requestMessage, HttpCompletionOption.ResponseHeadersRead, this._cancellationTokenSource.Token).ConfigureAwait(false);
				using (HttpResponseMessage response = httpResponseMessage)
				{
					this.responseCode = response.StatusCode;
					if (response.IsSuccessStatusCode)
					{
						long? contentLength = response.Content.Headers.ContentLength;
						using (Stream responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
						{
							using (MemoryStream memoryStream = new MemoryStream())
							{
								if (contentLength != null)
								{
									await responseStream.CopyToAsync(memoryStream, 81920, new Progress<long>(delegate(long totalBytes)
									{
										this.downloadedBytes = totalBytes;
										this.downloadProgress = (float)this.downloadedBytes / (float)contentLength.Value;
									}), this._cancellationTokenSource.Token).ConfigureAwait(false);
									this.downloadProgress = 1f;
								}
								else
								{
									await responseStream.CopyToAsync(memoryStream, this._cancellationTokenSource.Token).ConfigureAwait(false);
									this.downloadProgress = -1f;
								}
								byte[] array = memoryStream.ToArray();
								Encoding encoding = Encoding.UTF8;
								MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
								string text = ((contentType != null) ? contentType.CharSet : null);
								if (!string.IsNullOrEmpty(text))
								{
									try
									{
										encoding = Encoding.GetEncoding(text);
									}
									catch (Exception)
									{
									}
								}
								string @string = encoding.GetString(array);
								this.downloadHandler.SetData(array);
								this.downloadHandler.SetText(@string);
								this.result = WR_Result.Success;
							}
							MemoryStream memoryStream = null;
						}
						Stream responseStream = null;
					}
					else
					{
						string text2 = string.Format("Error: {0}", response.StatusCode);
						this.downloadHandler.SetError(text2);
						this.error = text2;
						this.result = WR_Result.ProtocolError;
					}
				}
				HttpResponseMessage response = null;
			}
			catch (Exception ex)
			{
				this.downloadHandler.SetError(ex.Message);
				this.error = ex.Message;
				this.result = WR_Result.ConnectionError;
			}
			finally
			{
				this.isDone = true;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000024F4 File Offset: 0x000006F4
		public void Dispose()
		{
			HttpRequestMessage requestMessage = this._requestMessage;
			if (requestMessage != null)
			{
				requestMessage.Dispose();
			}
			HttpClient httpClient = this._httpClient;
			if (httpClient != null)
			{
				httpClient.Dispose();
			}
			CancellationTokenSource cancellationTokenSource = this._cancellationTokenSource;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			CancellationTokenSource cancellationTokenSource2 = this._cancellationTokenSource;
			if (cancellationTokenSource2 == null)
			{
				return;
			}
			cancellationTokenSource2.Dispose();
		}

		// Token: 0x04000003 RID: 3
		private readonly HttpClient _httpClient;

		// Token: 0x04000004 RID: 4
		private HttpRequestMessage _requestMessage;

		// Token: 0x04000005 RID: 5
		private CancellationTokenSource _cancellationTokenSource;
	}
}
