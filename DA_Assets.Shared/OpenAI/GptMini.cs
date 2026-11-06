using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DA_Assets.Networking;
using UnityEngine;
using UnityEngine.Networking;

namespace DA_Assets.OpenAI
{
	// Token: 0x0200001B RID: 27
	public class GptMini
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00003316 File Offset: 0x00001516
		public GptMini(string apiKey, string model = "gpt-4o-mini", int timeout = 0)
		{
			this._apiKey = apiKey;
			this._model = model;
			this._timeout = timeout;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003334 File Offset: 0x00001534
		public async Task<string> SendRequest(UnityWebRequest webRequest)
		{
			string text;
			try
			{
				UnityWebRequestAsyncOperation operation = webRequest.SendWebRequest();
				while (!operation.isDone)
				{
					await Task.Yield();
				}
				if (webRequest.result != UnityWebRequest.Result.Success)
				{
					Debug.LogError("Error: " + webRequest.error);
					text = null;
				}
				else
				{
					text = webRequest.downloadHandler.text;
				}
			}
			finally
			{
				if (webRequest != null)
				{
					((IDisposable)webRequest).Dispose();
				}
			}
			return text;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003378 File Offset: 0x00001578
		public async Task<string> InvokeChat(string prompt)
		{
			string methodPath = "GptMini.InvokeChat";
			string text = this.CreateChatRequestBody(prompt);
			Dictionary<string, string> dictionary = new Dictionary<string, string> { 
			{
				"Authorization",
				"Bearer " + this._apiKey
			} };
			string text2 = await RequestSender.Post("https://api.openai.com/v1/chat/completions", text, "application/json", dictionary, 100);
			string text3;
			if (string.IsNullOrWhiteSpace(text2))
			{
				Debug.LogError(methodPath + " | Received an empty response.");
				text3 = null;
			}
			else
			{
				Response response = JsonUtility.FromJson<Response>(text2);
				if (response.choices != null && response.choices.Length != 0)
				{
					text3 = response.choices[0].message.content;
				}
				else
				{
					Debug.LogError(methodPath + " | Invalid response structure.");
					text3 = null;
				}
			}
			return text3;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000033C4 File Offset: 0x000015C4
		private string CreateChatRequestBody(string prompt)
		{
			RequestMessage requestMessage = default(RequestMessage);
			requestMessage.role = "user";
			requestMessage.content = prompt;
			return JsonUtility.ToJson(new Request
			{
				model = this._model,
				messages = new RequestMessage[] { requestMessage }
			});
		}

		// Token: 0x0400003B RID: 59
		private string _apiKey;

		// Token: 0x0400003C RID: 60
		private int _timeout;

		// Token: 0x0400003D RID: 61
		private string _model;

		// Token: 0x0400003E RID: 62
		public const string _apiUrl = "https://api.openai.com/v1/chat/completions";
	}
}
