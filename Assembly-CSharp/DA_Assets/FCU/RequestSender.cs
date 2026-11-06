using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.Logging;
using DA_Assets.Networking;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000368 RID: 872
	[Serializable]
	public class RequestSender : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060016C4 RID: 5828 RVA: 0x0009587A File Offset: 0x00093A7A
		public float PbarProgress
		{
			get
			{
				return this.pbarProgress;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00095882 File Offset: 0x00093A82
		public float PbarBytes
		{
			get
			{
				return this.pbarBytes;
			}
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x0009588C File Offset: 0x00093A8C
		public RequestHeader GetRequestHeader(string token, AuthType? authType = null)
		{
			AuthType authType2 = ((authType != null) ? authType.Value : this.monoBeh.Authorizer.CurrentSession.AuthType);
			RequestHeader requestHeader;
			if (authType2 == AuthType.OAuth2)
			{
				requestHeader = new RequestHeader
				{
					Name = "Authorization",
					Value = "Bearer " + token
				};
				return requestHeader;
			}
			if (authType2 != AuthType.Manual)
			{
				throw new NotImplementedException();
			}
			requestHeader = new RequestHeader
			{
				Name = "X-Figma-Token",
				Value = (token ?? "")
			};
			return requestHeader;
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00095921 File Offset: 0x00093B21
		public void RefreshLimiterData()
		{
			this._requestCount = RequestSender.requestCount;
			this._timeoutActive = RequestSender.timeoutActive;
			this._remainingTime = RequestSender.remainingTime;
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x00095944 File Offset: 0x00093B44
		private async Task CheckRateLimit()
		{
			while (RequestSender.requestCount >= FcuConfig.ApiRequestsCountLimit)
			{
				if (!RequestSender.timeoutActive)
				{
					RequestSender.timeoutActive = true;
					RequestSender.remainingTime = FcuConfig.ApiTimeoutSec;
					this.LogRemainingTime();
				}
				await Task.Delay(1000);
			}
			RequestSender.requestCount++;
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x00095988 File Offset: 0x00093B88
		private async Task LogRemainingTime()
		{
			while (RequestSender.remainingTime > 0)
			{
				DALogger.Log(FcuLocKey.log_api_waiting.Localize(new object[] { RequestSender.remainingTime }));
				this.RefreshLimiterData();
				await Task.Delay(1000);
				RequestSender.remainingTime--;
			}
			RequestSender.requestCount = 0;
			RequestSender.timeoutActive = false;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x000959CC File Offset: 0x00093BCC
		public async Task<DAResult<T>> SendRequest<T>(DARequest request)
		{
			await this.CheckRateLimit();
			UnityHttpClient webRequest;
			if (request.RequestType == RequestType.Post)
			{
				webRequest = UnityHttpClient.Post(request.Query, request.WWWForm);
			}
			else
			{
				webRequest = UnityHttpClient.Get(request.Query);
			}
			if (this.monoBeh.IsDebug())
			{
				Debug.Log(request.Query);
			}
			DAResult<T> daresult2;
			using (webRequest)
			{
				if (!request.RequestHeader.IsDefault<RequestHeader>())
				{
					webRequest.SetRequestHeader(request.RequestHeader.Name, request.RequestHeader.Value);
				}
				try
				{
					webRequest.SendWebRequest(null);
				}
				catch (InvalidOperationException)
				{
					DALogger.LogError(FcuLocKey.log_enable_http_project_settings.Localize(Array.Empty<object>()));
					this.monoBeh.AssetTools.StopImport(StopImportReason.Error);
					return default(DAResult<T>);
				}
				catch (Exception ex)
				{
					DALogger.LogException(ex);
				}
				await this.UpdateRequestProgressBar(webRequest);
				await this.MoveRequestProgressBarToEnd();
				DAResult<T> daresult = default(DAResult<T>);
				if (request.RequestType == RequestType.GetFile)
				{
					daresult.Success = true;
					daresult.Object = (T)((object)webRequest.downloadHandler.data);
				}
				else
				{
					request.WriteLog(webRequest);
					string text = webRequest.downloadHandler.text;
					if (typeof(T) == typeof(string))
					{
						daresult.Success = true;
						daresult.Object = (T)((object)text);
					}
					else
					{
						daresult = await this.TryParseResponse<T>(text, request, webRequest);
					}
				}
				daresult2 = daresult;
			}
			return daresult2;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00095A18 File Offset: 0x00093C18
		private Task<DAResult<T>> TryParseResponse<T>(string text, DARequest request, UnityHttpClient webRequest)
		{
			RequestSender.<TryParseResponse>d__17<T> <TryParseResponse>d__;
			<TryParseResponse>d__.<>t__builder = AsyncTaskMethodBuilder<DAResult<T>>.Create();
			<TryParseResponse>d__.<>4__this = this;
			<TryParseResponse>d__.text = text;
			<TryParseResponse>d__.request = request;
			<TryParseResponse>d__.webRequest = webRequest;
			<TryParseResponse>d__.<>1__state = -1;
			<TryParseResponse>d__.<>t__builder.Start<RequestSender.<TryParseResponse>d__17<T>>(ref <TryParseResponse>d__);
			return <TryParseResponse>d__.<>t__builder.Task;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00095A74 File Offset: 0x00093C74
		private async Task UpdateRequestProgressBar(UnityHttpClient webRequest)
		{
			while (!webRequest.isDone)
			{
				if (this.monoBeh.IsCancellationRequested(TokenType.Import))
				{
					return;
				}
				if (webRequest.downloadProgress == 0f || webRequest.downloadedBytes == 0L)
				{
					if (this.pbarProgress < 1f)
					{
						this.pbarProgress += 0.01f;
					}
					else
					{
						this.pbarProgress = 0f;
					}
					this.pbarBytes += 100f;
				}
				else
				{
					this.pbarProgress = webRequest.downloadProgress;
					this.pbarBytes = (float)webRequest.downloadedBytes;
				}
				await Task.Yield();
			}
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00095AC0 File Offset: 0x00093CC0
		private async Task MoveRequestProgressBarToEnd()
		{
			float num = 1f - this.pbarProgress;
			int steps = 10;
			float stepIncrement = num / (float)steps;
			for (int i = 0; i < steps; i++)
			{
				this.pbarProgress += stepIncrement;
				await Task.Yield();
			}
			this.pbarProgress = 0f;
			this.pbarBytes = 0f;
		}

		// Token: 0x04001884 RID: 6276
		[SerializeField]
		private float pbarProgress;

		// Token: 0x04001885 RID: 6277
		[SerializeField]
		private float pbarBytes;

		// Token: 0x04001886 RID: 6278
		[SerializeField]
		private int _requestCount;

		// Token: 0x04001887 RID: 6279
		[SerializeField]
		private bool _timeoutActive;

		// Token: 0x04001888 RID: 6280
		[SerializeField]
		private int _remainingTime;

		// Token: 0x04001889 RID: 6281
		private static int requestCount;

		// Token: 0x0400188A RID: 6282
		private static bool timeoutActive;

		// Token: 0x0400188B RID: 6283
		private static int remainingTime;
	}
}
