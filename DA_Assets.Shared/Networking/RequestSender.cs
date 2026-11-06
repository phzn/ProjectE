using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace DA_Assets.Networking
{
	// Token: 0x02000004 RID: 4
	public class RequestSender
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000021DC File Offset: 0x000003DC
		public static async Task<Texture2D> LoadImage(string url)
		{
			Texture2D texture2D;
			using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
			{
				UnityWebRequestAsyncOperation asyncOp = webRequest.SendWebRequest();
				while (!asyncOp.isDone)
				{
					await Task.Yield();
				}
				if (webRequest.result != UnityWebRequest.Result.Success)
				{
					Debug.LogError("LoadImage | " + webRequest.error);
					texture2D = null;
				}
				else
				{
					texture2D = DownloadHandlerTexture.GetContent(webRequest);
				}
			}
			return texture2D;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002220 File Offset: 0x00000420
		public static async Task<string> Get(string url, int awaitDelayMs = 100)
		{
			string text;
			using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
			{
				try
				{
					webRequest.SendWebRequest();
					while (!webRequest.isDone)
					{
						await Task.Delay(awaitDelayMs);
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					return null;
				}
				if (webRequest.result != UnityWebRequest.Result.Success)
				{
					Debug.LogError("RequestSender.Get | " + webRequest.error);
					text = null;
				}
				else
				{
					text = webRequest.downloadHandler.text;
				}
			}
			return text;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000226C File Offset: 0x0000046C
		public static async Task<string> Post(string url, string postData, string contentType, Dictionary<string, string> headers = null, int awaitDelayMs = 100)
		{
			string text;
			using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(postData);
				webRequest.uploadHandler = new UploadHandlerRaw(bytes);
				webRequest.downloadHandler = new DownloadHandlerBuffer();
				webRequest.SetRequestHeader("Content-Type", contentType);
				if (headers != null)
				{
					foreach (KeyValuePair<string, string> keyValuePair in headers)
					{
						webRequest.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
					}
				}
				try
				{
					webRequest.SendWebRequest();
					while (!webRequest.isDone)
					{
						await Task.Delay(awaitDelayMs);
					}
					goto IL_01BF;
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					return null;
				}
				IL_015C:
				await Task.Delay(awaitDelayMs);
				IL_01BF:
				if (!webRequest.isDone)
				{
					goto IL_015C;
				}
				if (webRequest.result != UnityWebRequest.Result.Success)
				{
					Debug.LogError("RequestSender.Post" + " | Error: " + webRequest.error);
					text = null;
				}
				else
				{
					text = webRequest.downloadHandler.text;
				}
			}
			return text;
		}
	}
}
