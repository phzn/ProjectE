using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.Bindings;
using UnityEngineInternal;

namespace UnityEngine.Networking
{
	// Token: 0x0200000E RID: 14
	[NativeHeader("Modules/UnityWebRequest/Public/UnityWebRequest.h")]
	[StructLayout(LayoutKind.Sequential)]
	public class UnityWebRequest : IDisposable
	{
		// Token: 0x06000082 RID: 130
		[NativeMethod(IsThreadSafe = true)]
		[NativeConditional("ENABLE_UNITYWEBREQUEST")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetWebErrorString(UnityWebRequest.UnityWebRequestError err);

		// Token: 0x06000083 RID: 131
		[VisibleToOtherModules]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetHTTPStatusString(long responseCode);

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003C1A File Offset: 0x00001E1A
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003C22 File Offset: 0x00001E22
		public bool disposeCertificateHandlerOnDispose { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003C2B File Offset: 0x00001E2B
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00003C33 File Offset: 0x00001E33
		public bool disposeDownloadHandlerOnDispose { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003C3C File Offset: 0x00001E3C
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00003C44 File Offset: 0x00001E44
		public bool disposeUploadHandlerOnDispose { get; set; }

		// Token: 0x0600008A RID: 138 RVA: 0x00003C4D File Offset: 0x00001E4D
		public static void ClearCookieCache()
		{
			UnityWebRequest.ClearCookieCache(null, null);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003C58 File Offset: 0x00001E58
		public static void ClearCookieCache(Uri uri)
		{
			bool flag = uri == null;
			if (flag)
			{
				UnityWebRequest.ClearCookieCache(null, null);
			}
			else
			{
				string host = uri.Host;
				string text = uri.AbsolutePath;
				bool flag2 = text == "/";
				if (flag2)
				{
					text = null;
				}
				UnityWebRequest.ClearCookieCache(host, text);
			}
		}

		// Token: 0x0600008C RID: 140
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearCookieCache(string domain, string path);

		// Token: 0x0600008D RID: 141
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr Create();

		// Token: 0x0600008E RID: 142
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Release();

		// Token: 0x0600008F RID: 143 RVA: 0x00003CA4 File Offset: 0x00001EA4
		internal void InternalDestroy()
		{
			bool flag = this.m_Ptr != IntPtr.Zero;
			if (flag)
			{
				this.Abort();
				this.Release();
				this.m_Ptr = IntPtr.Zero;
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003CE1 File Offset: 0x00001EE1
		private void InternalSetDefaults()
		{
			this.disposeDownloadHandlerOnDispose = true;
			this.disposeUploadHandlerOnDispose = true;
			this.disposeCertificateHandlerOnDispose = true;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003CFC File Offset: 0x00001EFC
		public UnityWebRequest()
		{
			this.m_Ptr = UnityWebRequest.Create();
			this.InternalSetDefaults();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003D18 File Offset: 0x00001F18
		public UnityWebRequest(string url)
		{
			this.m_Ptr = UnityWebRequest.Create();
			this.InternalSetDefaults();
			this.url = url;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003D3C File Offset: 0x00001F3C
		public UnityWebRequest(Uri uri)
		{
			this.m_Ptr = UnityWebRequest.Create();
			this.InternalSetDefaults();
			this.uri = uri;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003D60 File Offset: 0x00001F60
		public UnityWebRequest(string url, string method)
		{
			this.m_Ptr = UnityWebRequest.Create();
			this.InternalSetDefaults();
			this.url = url;
			this.method = method;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003D8C File Offset: 0x00001F8C
		public UnityWebRequest(Uri uri, string method)
		{
			this.m_Ptr = UnityWebRequest.Create();
			this.InternalSetDefaults();
			this.uri = uri;
			this.method = method;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003DB8 File Offset: 0x00001FB8
		public UnityWebRequest(string url, string method, DownloadHandler downloadHandler, UploadHandler uploadHandler)
		{
			this.m_Ptr = UnityWebRequest.Create();
			this.InternalSetDefaults();
			this.url = url;
			this.method = method;
			this.downloadHandler = downloadHandler;
			this.uploadHandler = uploadHandler;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003DF5 File Offset: 0x00001FF5
		public UnityWebRequest(Uri uri, string method, DownloadHandler downloadHandler, UploadHandler uploadHandler)
		{
			this.m_Ptr = UnityWebRequest.Create();
			this.InternalSetDefaults();
			this.uri = uri;
			this.method = method;
			this.downloadHandler = downloadHandler;
			this.uploadHandler = uploadHandler;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003E34 File Offset: 0x00002034
		~UnityWebRequest()
		{
			this.DisposeHandlers();
			this.InternalDestroy();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003E6C File Offset: 0x0000206C
		public void Dispose()
		{
			this.DisposeHandlers();
			this.InternalDestroy();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003E84 File Offset: 0x00002084
		private void DisposeHandlers()
		{
			bool disposeDownloadHandlerOnDispose = this.disposeDownloadHandlerOnDispose;
			if (disposeDownloadHandlerOnDispose)
			{
				DownloadHandler downloadHandler = this.downloadHandler;
				bool flag = downloadHandler != null;
				if (flag)
				{
					downloadHandler.Dispose();
				}
			}
			bool disposeUploadHandlerOnDispose = this.disposeUploadHandlerOnDispose;
			if (disposeUploadHandlerOnDispose)
			{
				UploadHandler uploadHandler = this.uploadHandler;
				bool flag2 = uploadHandler != null;
				if (flag2)
				{
					uploadHandler.Dispose();
				}
			}
			bool disposeCertificateHandlerOnDispose = this.disposeCertificateHandlerOnDispose;
			if (disposeCertificateHandlerOnDispose)
			{
				CertificateHandler certificateHandler = this.certificateHandler;
				bool flag3 = certificateHandler != null;
				if (flag3)
				{
					certificateHandler.Dispose();
				}
			}
		}

		// Token: 0x0600009B RID: 155
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern UnityWebRequestAsyncOperation BeginWebRequest();

		// Token: 0x0600009C RID: 156 RVA: 0x00003F0C File Offset: 0x0000210C
		[Obsolete("Use SendWebRequest.  It returns a UnityWebRequestAsyncOperation which contains a reference to the WebRequest object.", false)]
		public AsyncOperation Send()
		{
			return this.SendWebRequest();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003F24 File Offset: 0x00002124
		public UnityWebRequestAsyncOperation SendWebRequest()
		{
			UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = this.BeginWebRequest();
			bool flag = unityWebRequestAsyncOperation != null;
			if (flag)
			{
				unityWebRequestAsyncOperation.webRequest = this;
			}
			return unityWebRequestAsyncOperation;
		}

		// Token: 0x0600009E RID: 158
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Abort();

		// Token: 0x0600009F RID: 159
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern UnityWebRequest.UnityWebRequestError SetMethod(UnityWebRequest.UnityWebRequestMethod methodType);

		// Token: 0x060000A0 RID: 160 RVA: 0x00003F50 File Offset: 0x00002150
		internal void InternalSetMethod(UnityWebRequest.UnityWebRequestMethod methodType)
		{
			bool flag = !this.isModifiable;
			if (flag)
			{
				throw new InvalidOperationException("UnityWebRequest has already been sent and its request method can no longer be altered");
			}
			UnityWebRequest.UnityWebRequestError unityWebRequestError = this.SetMethod(methodType);
			bool flag2 = unityWebRequestError > UnityWebRequest.UnityWebRequestError.OK;
			if (flag2)
			{
				throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(unityWebRequestError));
			}
		}

		// Token: 0x060000A1 RID: 161
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern UnityWebRequest.UnityWebRequestError SetCustomMethod(string customMethodName);

		// Token: 0x060000A2 RID: 162 RVA: 0x00003F94 File Offset: 0x00002194
		internal void InternalSetCustomMethod(string customMethodName)
		{
			bool flag = !this.isModifiable;
			if (flag)
			{
				throw new InvalidOperationException("UnityWebRequest has already been sent and its request method can no longer be altered");
			}
			UnityWebRequest.UnityWebRequestError unityWebRequestError = this.SetCustomMethod(customMethodName);
			bool flag2 = unityWebRequestError > UnityWebRequest.UnityWebRequestError.OK;
			if (flag2)
			{
				throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(unityWebRequestError));
			}
		}

		// Token: 0x060000A3 RID: 163
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern UnityWebRequest.UnityWebRequestMethod GetMethod();

		// Token: 0x060000A4 RID: 164
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string GetCustomMethod();

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003FD8 File Offset: 0x000021D8
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00004034 File Offset: 0x00002234
		public string method
		{
			get
			{
				string text;
				switch (this.GetMethod())
				{
				case UnityWebRequest.UnityWebRequestMethod.Get:
					text = "GET";
					break;
				case UnityWebRequest.UnityWebRequestMethod.Post:
					text = "POST";
					break;
				case UnityWebRequest.UnityWebRequestMethod.Put:
					text = "PUT";
					break;
				case UnityWebRequest.UnityWebRequestMethod.Head:
					text = "HEAD";
					break;
				default:
					text = this.GetCustomMethod();
					break;
				}
				return text;
			}
			set
			{
				bool flag = string.IsNullOrEmpty(value);
				if (flag)
				{
					throw new ArgumentException("Cannot set a UnityWebRequest's method to an empty or null string");
				}
				string text = value.ToUpper();
				string text2 = text;
				if (!(text2 == "GET"))
				{
					if (!(text2 == "POST"))
					{
						if (!(text2 == "PUT"))
						{
							if (!(text2 == "HEAD"))
							{
								this.InternalSetCustomMethod(value.ToUpper());
							}
							else
							{
								this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Head);
							}
						}
						else
						{
							this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Put);
						}
					}
					else
					{
						this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Post);
					}
				}
				else
				{
					this.InternalSetMethod(UnityWebRequest.UnityWebRequestMethod.Get);
				}
			}
		}

		// Token: 0x060000A7 RID: 167
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern UnityWebRequest.UnityWebRequestError GetError();

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000040D0 File Offset: 0x000022D0
		public string error
		{
			get
			{
				UnityWebRequest.Result result = this.result;
				UnityWebRequest.Result result2 = result;
				string text;
				if (result2 > UnityWebRequest.Result.Success)
				{
					if (result2 != UnityWebRequest.Result.ProtocolError)
					{
						text = UnityWebRequest.GetWebErrorString(this.GetError());
					}
					else
					{
						text = string.Format("HTTP/1.1 {0} {1}", this.responseCode, UnityWebRequest.GetHTTPStatusString(this.responseCode));
					}
				}
				else
				{
					text = null;
				}
				return text;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A9 RID: 169
		// (set) Token: 0x060000AA RID: 170
		private extern bool use100Continue
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000412C File Offset: 0x0000232C
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00004144 File Offset: 0x00002344
		public bool useHttpContinue
		{
			get
			{
				return this.use100Continue;
			}
			set
			{
				bool flag = !this.isModifiable;
				if (flag)
				{
					throw new InvalidOperationException("UnityWebRequest has already been sent and its 100-Continue setting cannot be altered");
				}
				this.use100Continue = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004174 File Offset: 0x00002374
		// (set) Token: 0x060000AE RID: 174 RVA: 0x0000418C File Offset: 0x0000238C
		public string url
		{
			get
			{
				return this.GetUrl();
			}
			set
			{
				string text = "https://localhost/";
				this.InternalSetUrl(WebRequestUtils.MakeInitialUrl(value, text));
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000041B0 File Offset: 0x000023B0
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000041D0 File Offset: 0x000023D0
		public Uri uri
		{
			get
			{
				return new Uri(this.GetUrl());
			}
			set
			{
				bool flag = !value.IsAbsoluteUri;
				if (flag)
				{
					throw new ArgumentException("URI must be absolute");
				}
				this.InternalSetUrl(WebRequestUtils.MakeUriString(value, value.OriginalString, false));
				this.m_Uri = value;
			}
		}

		// Token: 0x060000B1 RID: 177
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetUrl();

		// Token: 0x060000B2 RID: 178
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern UnityWebRequest.UnityWebRequestError SetUrl(string url);

		// Token: 0x060000B3 RID: 179 RVA: 0x00004214 File Offset: 0x00002414
		private void InternalSetUrl(string url)
		{
			bool flag = !this.isModifiable;
			if (flag)
			{
				throw new InvalidOperationException("UnityWebRequest has already been sent and its URL cannot be altered");
			}
			UnityWebRequest.UnityWebRequestError unityWebRequestError = this.SetUrl(url);
			bool flag2 = unityWebRequestError > UnityWebRequest.UnityWebRequestError.OK;
			if (flag2)
			{
				throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(unityWebRequestError));
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000B4 RID: 180
		public extern long responseCode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060000B5 RID: 181
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern float GetUploadProgress();

		// Token: 0x060000B6 RID: 182
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsExecuting();

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004258 File Offset: 0x00002458
		public float uploadProgress
		{
			get
			{
				bool flag = !this.IsExecuting() && !this.isDone;
				float num;
				if (flag)
				{
					num = -1f;
				}
				else
				{
					num = this.GetUploadProgress();
				}
				return num;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B8 RID: 184
		public extern bool isModifiable
		{
			[NativeMethod("IsModifiable")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004290 File Offset: 0x00002490
		public bool isDone
		{
			get
			{
				return this.result > UnityWebRequest.Result.InProgress;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000042AC File Offset: 0x000024AC
		[Obsolete("UnityWebRequest.isNetworkError is deprecated. Use (UnityWebRequest.result == UnityWebRequest.Result.ConnectionError) instead.", false)]
		public bool isNetworkError
		{
			get
			{
				return this.result == UnityWebRequest.Result.ConnectionError;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000042C8 File Offset: 0x000024C8
		[Obsolete("UnityWebRequest.isHttpError is deprecated. Use (UnityWebRequest.result == UnityWebRequest.Result.ProtocolError) instead.", false)]
		public bool isHttpError
		{
			get
			{
				return this.result == UnityWebRequest.Result.ProtocolError;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000BC RID: 188
		public extern UnityWebRequest.Result result
		{
			[NativeMethod("GetResult")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060000BD RID: 189
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern float GetDownloadProgress();

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000042E4 File Offset: 0x000024E4
		public float downloadProgress
		{
			get
			{
				bool flag = !this.IsExecuting() && !this.isDone;
				float num;
				if (flag)
				{
					num = -1f;
				}
				else
				{
					num = this.GetDownloadProgress();
				}
				return num;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BF RID: 191
		public extern ulong uploadedBytes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C0 RID: 192
		public extern ulong downloadedBytes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060000C1 RID: 193
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetRedirectLimit();

		// Token: 0x060000C2 RID: 194
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRedirectLimitFromScripting(int limit);

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000431C File Offset: 0x0000251C
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00004334 File Offset: 0x00002534
		public int redirectLimit
		{
			get
			{
				return this.GetRedirectLimit();
			}
			set
			{
				this.SetRedirectLimitFromScripting(value);
			}
		}

		// Token: 0x060000C5 RID: 197
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetChunked();

		// Token: 0x060000C6 RID: 198
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern UnityWebRequest.UnityWebRequestError SetChunked(bool chunked);

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004340 File Offset: 0x00002540
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00004358 File Offset: 0x00002558
		[Obsolete("HTTP/2 and many HTTP/1.1 servers don't support this; we recommend leaving it set to false (default).", false)]
		public bool chunkedTransfer
		{
			get
			{
				return this.GetChunked();
			}
			set
			{
				bool flag = !this.isModifiable;
				if (flag)
				{
					throw new InvalidOperationException("UnityWebRequest has already been sent and its chunked transfer encoding setting cannot be altered");
				}
				UnityWebRequest.UnityWebRequestError unityWebRequestError = this.SetChunked(value);
				bool flag2 = unityWebRequestError > UnityWebRequest.UnityWebRequestError.OK;
				if (flag2)
				{
					throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(unityWebRequestError));
				}
			}
		}

		// Token: 0x060000C9 RID: 201
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetRequestHeader(string name);

		// Token: 0x060000CA RID: 202
		[NativeMethod("SetRequestHeader")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern UnityWebRequest.UnityWebRequestError InternalSetRequestHeader(string name, string value);

		// Token: 0x060000CB RID: 203 RVA: 0x0000439C File Offset: 0x0000259C
		public void SetRequestHeader(string name, string value)
		{
			bool flag = string.IsNullOrEmpty(name);
			if (flag)
			{
				throw new ArgumentException("Cannot set a Request Header with a null or empty name");
			}
			bool flag2 = value == null;
			if (flag2)
			{
				throw new ArgumentException("Cannot set a Request header with a null");
			}
			bool flag3 = !this.isModifiable;
			if (flag3)
			{
				throw new InvalidOperationException("UnityWebRequest has already been sent and its request headers cannot be altered");
			}
			UnityWebRequest.UnityWebRequestError unityWebRequestError = this.InternalSetRequestHeader(name, value);
			bool flag4 = unityWebRequestError > UnityWebRequest.UnityWebRequestError.OK;
			if (flag4)
			{
				throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(unityWebRequestError));
			}
		}

		// Token: 0x060000CC RID: 204
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetResponseHeader(string name);

		// Token: 0x060000CD RID: 205
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string[] GetResponseHeaderKeys();

		// Token: 0x060000CE RID: 206 RVA: 0x0000440C File Offset: 0x0000260C
		public Dictionary<string, string> GetResponseHeaders()
		{
			string[] responseHeaderKeys = this.GetResponseHeaderKeys();
			bool flag = responseHeaderKeys == null || responseHeaderKeys.Length == 0;
			Dictionary<string, string> dictionary;
			if (flag)
			{
				dictionary = null;
			}
			else
			{
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>(responseHeaderKeys.Length, StringComparer.OrdinalIgnoreCase);
				for (int i = 0; i < responseHeaderKeys.Length; i++)
				{
					string responseHeader = this.GetResponseHeader(responseHeaderKeys[i]);
					dictionary2.Add(responseHeaderKeys[i], responseHeader);
				}
				dictionary = dictionary2;
			}
			return dictionary;
		}

		// Token: 0x060000CF RID: 207
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern UnityWebRequest.UnityWebRequestError SetUploadHandler(UploadHandler uh);

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000447C File Offset: 0x0000267C
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00004494 File Offset: 0x00002694
		public UploadHandler uploadHandler
		{
			get
			{
				return this.m_UploadHandler;
			}
			set
			{
				bool flag = !this.isModifiable;
				if (flag)
				{
					throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the upload handler");
				}
				UnityWebRequest.UnityWebRequestError unityWebRequestError = this.SetUploadHandler(value);
				bool flag2 = unityWebRequestError > UnityWebRequest.UnityWebRequestError.OK;
				if (flag2)
				{
					throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(unityWebRequestError));
				}
				this.m_UploadHandler = value;
			}
		}

		// Token: 0x060000D2 RID: 210
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern UnityWebRequest.UnityWebRequestError SetDownloadHandler(DownloadHandler dh);

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000044E0 File Offset: 0x000026E0
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x000044F8 File Offset: 0x000026F8
		public DownloadHandler downloadHandler
		{
			get
			{
				return this.m_DownloadHandler;
			}
			set
			{
				bool flag = !this.isModifiable;
				if (flag)
				{
					throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the download handler");
				}
				UnityWebRequest.UnityWebRequestError unityWebRequestError = this.SetDownloadHandler(value);
				bool flag2 = unityWebRequestError > UnityWebRequest.UnityWebRequestError.OK;
				if (flag2)
				{
					throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(unityWebRequestError));
				}
				this.m_DownloadHandler = value;
			}
		}

		// Token: 0x060000D5 RID: 213
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern UnityWebRequest.UnityWebRequestError SetCertificateHandler(CertificateHandler ch);

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004544 File Offset: 0x00002744
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x0000455C File Offset: 0x0000275C
		public CertificateHandler certificateHandler
		{
			get
			{
				return this.m_CertificateHandler;
			}
			set
			{
				bool flag = !this.isModifiable;
				if (flag)
				{
					throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the certificate handler");
				}
				UnityWebRequest.UnityWebRequestError unityWebRequestError = this.SetCertificateHandler(value);
				bool flag2 = unityWebRequestError > UnityWebRequest.UnityWebRequestError.OK;
				if (flag2)
				{
					throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(unityWebRequestError));
				}
				this.m_CertificateHandler = value;
			}
		}

		// Token: 0x060000D8 RID: 216
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetTimeoutMsec();

		// Token: 0x060000D9 RID: 217
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern UnityWebRequest.UnityWebRequestError SetTimeoutMsec(int timeout);

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000045A8 File Offset: 0x000027A8
		// (set) Token: 0x060000DB RID: 219 RVA: 0x000045C8 File Offset: 0x000027C8
		public int timeout
		{
			get
			{
				return this.GetTimeoutMsec() / 1000;
			}
			set
			{
				bool flag = !this.isModifiable;
				if (flag)
				{
					throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the timeout");
				}
				value = Math.Max(value, 0);
				UnityWebRequest.UnityWebRequestError unityWebRequestError = this.SetTimeoutMsec(value * 1000);
				bool flag2 = unityWebRequestError > UnityWebRequest.UnityWebRequestError.OK;
				if (flag2)
				{
					throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(unityWebRequestError));
				}
			}
		}

		// Token: 0x060000DC RID: 220
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetSuppressErrorsToConsole();

		// Token: 0x060000DD RID: 221
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern UnityWebRequest.UnityWebRequestError SetSuppressErrorsToConsole(bool suppress);

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000DE RID: 222 RVA: 0x0000461C File Offset: 0x0000281C
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00004634 File Offset: 0x00002834
		internal bool suppressErrorsToConsole
		{
			get
			{
				return this.GetSuppressErrorsToConsole();
			}
			set
			{
				bool flag = !this.isModifiable;
				if (flag)
				{
					throw new InvalidOperationException("UnityWebRequest has already been sent; cannot modify the timeout");
				}
				UnityWebRequest.UnityWebRequestError unityWebRequestError = this.SetSuppressErrorsToConsole(value);
				bool flag2 = unityWebRequestError > UnityWebRequest.UnityWebRequestError.OK;
				if (flag2)
				{
					throw new InvalidOperationException(UnityWebRequest.GetWebErrorString(unityWebRequestError));
				}
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004678 File Offset: 0x00002878
		public static UnityWebRequest Get(string uri)
		{
			return new UnityWebRequest(uri, "GET", new DownloadHandlerBuffer(), null);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000046A0 File Offset: 0x000028A0
		public static UnityWebRequest Get(Uri uri)
		{
			return new UnityWebRequest(uri, "GET", new DownloadHandlerBuffer(), null);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000046C8 File Offset: 0x000028C8
		public static UnityWebRequest Delete(string uri)
		{
			return new UnityWebRequest(uri, "DELETE");
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000046E8 File Offset: 0x000028E8
		public static UnityWebRequest Delete(Uri uri)
		{
			return new UnityWebRequest(uri, "DELETE");
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004708 File Offset: 0x00002908
		public static UnityWebRequest Head(string uri)
		{
			return new UnityWebRequest(uri, "HEAD");
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004728 File Offset: 0x00002928
		public static UnityWebRequest Head(Uri uri)
		{
			return new UnityWebRequest(uri, "HEAD");
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004747 File Offset: 0x00002947
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestTexture.GetTexture(*)", true)]
		public static UnityWebRequest GetTexture(string uri)
		{
			throw new NotSupportedException("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead.");
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004747 File Offset: 0x00002947
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestTexture.GetTexture(*)", true)]
		public static UnityWebRequest GetTexture(string uri, bool nonReadable)
		{
			throw new NotSupportedException("UnityWebRequest.GetTexture is obsolete. Use UnityWebRequestTexture.GetTexture instead.");
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004754 File Offset: 0x00002954
		[Obsolete("UnityWebRequest.GetAudioClip is obsolete. Use UnityWebRequestMultimedia.GetAudioClip instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestMultimedia.GetAudioClip(*)", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static UnityWebRequest GetAudioClip(string uri, AudioType audioType)
		{
			return null;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004768 File Offset: 0x00002968
		[Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static UnityWebRequest GetAssetBundle(string uri)
		{
			return null;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000477C File Offset: 0x0000297C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
		public static UnityWebRequest GetAssetBundle(string uri, uint crc)
		{
			return null;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004790 File Offset: 0x00002990
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
		public static UnityWebRequest GetAssetBundle(string uri, uint version, uint crc)
		{
			return null;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000047A4 File Offset: 0x000029A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
		public static UnityWebRequest GetAssetBundle(string uri, Hash128 hash, uint crc)
		{
			return null;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000047B8 File Offset: 0x000029B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("UnityWebRequest.GetAssetBundle is obsolete. Use UnityWebRequestAssetBundle.GetAssetBundle instead (UnityUpgradable) -> [UnityEngine] UnityWebRequestAssetBundle.GetAssetBundle(*)", true)]
		public static UnityWebRequest GetAssetBundle(string uri, CachedAssetBundle cachedAssetBundle, uint crc)
		{
			return null;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000047CC File Offset: 0x000029CC
		public static UnityWebRequest Put(string uri, byte[] bodyData)
		{
			return new UnityWebRequest(uri, "PUT", new DownloadHandlerBuffer(), new UploadHandlerRaw(bodyData));
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000047F8 File Offset: 0x000029F8
		public static UnityWebRequest Put(Uri uri, byte[] bodyData)
		{
			return new UnityWebRequest(uri, "PUT", new DownloadHandlerBuffer(), new UploadHandlerRaw(bodyData));
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004824 File Offset: 0x00002A24
		public static UnityWebRequest Put(string uri, string bodyData)
		{
			return new UnityWebRequest(uri, "PUT", new DownloadHandlerBuffer(), new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyData)));
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004858 File Offset: 0x00002A58
		public static UnityWebRequest Put(Uri uri, string bodyData)
		{
			return new UnityWebRequest(uri, "PUT", new DownloadHandlerBuffer(), new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyData)));
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000488C File Offset: 0x00002A8C
		[Obsolete("UnityWebRequest.Post with only a string data is obsolete. Use UnityWebRequest.Post with content type argument or UnityWebRequest.PostWwwForm instead (UnityUpgradable) -> [UnityEngine] UnityWebRequest.PostWwwForm(*)", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static UnityWebRequest Post(string uri, string postData)
		{
			return UnityWebRequest.PostWwwForm(uri, postData);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000048A8 File Offset: 0x00002AA8
		[Obsolete("UnityWebRequest.Post with only a string data is obsolete. Use UnityWebRequest.Post with content type argument or UnityWebRequest.PostWwwForm instead (UnityUpgradable) -> [UnityEngine] UnityWebRequest.PostWwwForm(*)", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static UnityWebRequest Post(Uri uri, string postData)
		{
			return UnityWebRequest.PostWwwForm(uri, postData);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000048C4 File Offset: 0x00002AC4
		public static UnityWebRequest PostWwwForm(string uri, string form)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			UnityWebRequest.SetupPostWwwForm(unityWebRequest, form);
			return unityWebRequest;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000048EC File Offset: 0x00002AEC
		public static UnityWebRequest PostWwwForm(Uri uri, string form)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			UnityWebRequest.SetupPostWwwForm(unityWebRequest, form);
			return unityWebRequest;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004914 File Offset: 0x00002B14
		private static void SetupPostWwwForm(UnityWebRequest request, string postData)
		{
			request.downloadHandler = new DownloadHandlerBuffer();
			bool flag = string.IsNullOrEmpty(postData);
			if (!flag)
			{
				string text = WWWTranscoder.DataEncode(postData, Encoding.UTF8);
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				request.uploadHandler = new UploadHandlerRaw(bytes);
				request.uploadHandler.contentType = "application/x-www-form-urlencoded";
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004974 File Offset: 0x00002B74
		public static UnityWebRequest Post(string uri, string postData, string contentType)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			UnityWebRequest.SetupPost(unityWebRequest, postData, contentType);
			return unityWebRequest;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000499C File Offset: 0x00002B9C
		public static UnityWebRequest Post(Uri uri, string postData, string contentType)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			UnityWebRequest.SetupPost(unityWebRequest, postData, contentType);
			return unityWebRequest;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000049C4 File Offset: 0x00002BC4
		private static void SetupPost(UnityWebRequest request, string postData, string contentType)
		{
			request.downloadHandler = new DownloadHandlerBuffer();
			bool flag = string.IsNullOrEmpty(postData);
			if (flag)
			{
				request.SetRequestHeader("Content-Type", contentType);
			}
			else
			{
				byte[] bytes = Encoding.UTF8.GetBytes(postData);
				request.uploadHandler = new UploadHandlerRaw(bytes);
				request.uploadHandler.contentType = contentType;
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004A20 File Offset: 0x00002C20
		public static UnityWebRequest Post(string uri, WWWForm formData)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			UnityWebRequest.SetupPost(unityWebRequest, formData);
			return unityWebRequest;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004A48 File Offset: 0x00002C48
		public static UnityWebRequest Post(Uri uri, WWWForm formData)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			UnityWebRequest.SetupPost(unityWebRequest, formData);
			return unityWebRequest;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004A70 File Offset: 0x00002C70
		private static void SetupPost(UnityWebRequest request, WWWForm formData)
		{
			request.downloadHandler = new DownloadHandlerBuffer();
			bool flag = formData == null;
			if (!flag)
			{
				byte[] array = formData.data;
				bool flag2 = array.Length == 0;
				if (flag2)
				{
					array = null;
				}
				bool flag3 = array != null;
				if (flag3)
				{
					request.uploadHandler = new UploadHandlerRaw(array);
				}
				Dictionary<string, string> headers = formData.headers;
				foreach (KeyValuePair<string, string> keyValuePair in headers)
				{
					request.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004B1C File Offset: 0x00002D1C
		public static UnityWebRequest Post(string uri, List<IMultipartFormSection> multipartFormSections)
		{
			byte[] array = UnityWebRequest.GenerateBoundary();
			return UnityWebRequest.Post(uri, multipartFormSections, array);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004B3C File Offset: 0x00002D3C
		public static UnityWebRequest Post(Uri uri, List<IMultipartFormSection> multipartFormSections)
		{
			byte[] array = UnityWebRequest.GenerateBoundary();
			return UnityWebRequest.Post(uri, multipartFormSections, array);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004B5C File Offset: 0x00002D5C
		public static UnityWebRequest Post(string uri, List<IMultipartFormSection> multipartFormSections, byte[] boundary)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			UnityWebRequest.SetupPost(unityWebRequest, multipartFormSections, boundary);
			return unityWebRequest;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004B84 File Offset: 0x00002D84
		public static UnityWebRequest Post(Uri uri, List<IMultipartFormSection> multipartFormSections, byte[] boundary)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			UnityWebRequest.SetupPost(unityWebRequest, multipartFormSections, boundary);
			return unityWebRequest;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004BAC File Offset: 0x00002DAC
		private static void SetupPost(UnityWebRequest request, List<IMultipartFormSection> multipartFormSections, byte[] boundary)
		{
			request.downloadHandler = new DownloadHandlerBuffer();
			byte[] array = null;
			bool flag = multipartFormSections != null && multipartFormSections.Count != 0;
			if (flag)
			{
				array = UnityWebRequest.SerializeFormSections(multipartFormSections, boundary);
			}
			bool flag2 = array == null;
			if (!flag2)
			{
				request.uploadHandler = new UploadHandlerRaw(array)
				{
					contentType = "multipart/form-data; boundary=" + Encoding.UTF8.GetString(boundary, 0, boundary.Length)
				};
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004C1C File Offset: 0x00002E1C
		public static UnityWebRequest Post(string uri, Dictionary<string, string> formFields)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			UnityWebRequest.SetupPost(unityWebRequest, formFields);
			return unityWebRequest;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004C44 File Offset: 0x00002E44
		public static UnityWebRequest Post(Uri uri, Dictionary<string, string> formFields)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(uri, "POST");
			UnityWebRequest.SetupPost(unityWebRequest, formFields);
			return unityWebRequest;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004C6C File Offset: 0x00002E6C
		private static void SetupPost(UnityWebRequest request, Dictionary<string, string> formFields)
		{
			request.downloadHandler = new DownloadHandlerBuffer();
			byte[] array = null;
			bool flag = formFields != null && formFields.Count != 0;
			if (flag)
			{
				array = UnityWebRequest.SerializeSimpleForm(formFields);
			}
			bool flag2 = array == null;
			if (!flag2)
			{
				request.uploadHandler = new UploadHandlerRaw(array)
				{
					contentType = "application/x-www-form-urlencoded"
				};
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public static string EscapeURL(string s)
		{
			return UnityWebRequest.EscapeURL(s, Encoding.UTF8);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004CE8 File Offset: 0x00002EE8
		public static string EscapeURL(string s, Encoding e)
		{
			bool flag = s == null;
			string text;
			if (flag)
			{
				text = null;
			}
			else
			{
				bool flag2 = s == "";
				if (flag2)
				{
					text = "";
				}
				else
				{
					bool flag3 = e == null;
					if (flag3)
					{
						text = null;
					}
					else
					{
						byte[] bytes = e.GetBytes(s);
						byte[] array = WWWTranscoder.URLEncode(bytes);
						text = e.GetString(array);
					}
				}
			}
			return text;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004D44 File Offset: 0x00002F44
		public static string UnEscapeURL(string s)
		{
			return UnityWebRequest.UnEscapeURL(s, Encoding.UTF8);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004D64 File Offset: 0x00002F64
		public static string UnEscapeURL(string s, Encoding e)
		{
			bool flag = s == null;
			string text;
			if (flag)
			{
				text = null;
			}
			else
			{
				bool flag2 = s.IndexOf('%') == -1 && s.IndexOf('+') == -1;
				if (flag2)
				{
					text = s;
				}
				else
				{
					byte[] bytes = e.GetBytes(s);
					byte[] array = WWWTranscoder.URLDecode(bytes);
					text = e.GetString(array);
				}
			}
			return text;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004DBC File Offset: 0x00002FBC
		public static byte[] SerializeFormSections(List<IMultipartFormSection> multipartFormSections, byte[] boundary)
		{
			bool flag = multipartFormSections == null || multipartFormSections.Count == 0;
			byte[] array;
			if (flag)
			{
				array = null;
			}
			else
			{
				byte[] bytes = Encoding.UTF8.GetBytes("\r\n");
				byte[] bytes2 = WWWForm.DefaultEncoding.GetBytes("--");
				int num = 0;
				foreach (IMultipartFormSection multipartFormSection in multipartFormSections)
				{
					num += 64 + multipartFormSection.sectionData.Length;
				}
				List<byte> list = new List<byte>(num);
				foreach (IMultipartFormSection multipartFormSection2 in multipartFormSections)
				{
					string text = "form-data";
					string sectionName = multipartFormSection2.sectionName;
					string fileName = multipartFormSection2.fileName;
					string text2 = "Content-Disposition: " + text;
					bool flag2 = !string.IsNullOrEmpty(sectionName);
					if (flag2)
					{
						text2 = text2 + "; name=\"" + sectionName + "\"";
					}
					bool flag3 = !string.IsNullOrEmpty(fileName);
					if (flag3)
					{
						text2 = text2 + "; filename=\"" + fileName + "\"";
					}
					text2 += "\r\n";
					string contentType = multipartFormSection2.contentType;
					bool flag4 = !string.IsNullOrEmpty(contentType);
					if (flag4)
					{
						text2 = text2 + "Content-Type: " + contentType + "\r\n";
					}
					list.AddRange(bytes);
					list.AddRange(bytes2);
					list.AddRange(boundary);
					list.AddRange(bytes);
					list.AddRange(Encoding.UTF8.GetBytes(text2));
					list.AddRange(bytes);
					list.AddRange(multipartFormSection2.sectionData);
				}
				list.AddRange(bytes);
				list.AddRange(bytes2);
				list.AddRange(boundary);
				list.AddRange(bytes2);
				list.AddRange(bytes);
				array = list.ToArray();
			}
			return array;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004FEC File Offset: 0x000031EC
		public static byte[] GenerateBoundary()
		{
			byte[] array = new byte[40];
			for (int i = 0; i < 40; i++)
			{
				int num = Random.Range(48, 110);
				bool flag = num > 57;
				if (flag)
				{
					num += 7;
				}
				bool flag2 = num > 90;
				if (flag2)
				{
					num += 6;
				}
				array[i] = (byte)num;
			}
			return array;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000504C File Offset: 0x0000324C
		public static byte[] SerializeSimpleForm(Dictionary<string, string> formFields)
		{
			string text = "";
			foreach (KeyValuePair<string, string> keyValuePair in formFields)
			{
				bool flag = text.Length > 0;
				if (flag)
				{
					text += "&";
				}
				text = text + WWWTranscoder.DataEncode(keyValuePair.Key) + "=" + WWWTranscoder.DataEncode(keyValuePair.Value);
			}
			return Encoding.UTF8.GetBytes(text);
		}

		// Token: 0x04000024 RID: 36
		[NonSerialized]
		internal IntPtr m_Ptr;

		// Token: 0x04000025 RID: 37
		[NonSerialized]
		internal DownloadHandler m_DownloadHandler;

		// Token: 0x04000026 RID: 38
		[NonSerialized]
		internal UploadHandler m_UploadHandler;

		// Token: 0x04000027 RID: 39
		[NonSerialized]
		internal CertificateHandler m_CertificateHandler;

		// Token: 0x04000028 RID: 40
		[NonSerialized]
		internal Uri m_Uri;

		// Token: 0x04000029 RID: 41
		public const string kHttpVerbGET = "GET";

		// Token: 0x0400002A RID: 42
		public const string kHttpVerbHEAD = "HEAD";

		// Token: 0x0400002B RID: 43
		public const string kHttpVerbPOST = "POST";

		// Token: 0x0400002C RID: 44
		public const string kHttpVerbPUT = "PUT";

		// Token: 0x0400002D RID: 45
		public const string kHttpVerbCREATE = "CREATE";

		// Token: 0x0400002E RID: 46
		public const string kHttpVerbDELETE = "DELETE";

		// Token: 0x0200000F RID: 15
		internal enum UnityWebRequestMethod
		{
			// Token: 0x04000033 RID: 51
			Get,
			// Token: 0x04000034 RID: 52
			Post,
			// Token: 0x04000035 RID: 53
			Put,
			// Token: 0x04000036 RID: 54
			Head,
			// Token: 0x04000037 RID: 55
			Custom
		}

		// Token: 0x02000010 RID: 16
		internal enum UnityWebRequestError
		{
			// Token: 0x04000039 RID: 57
			OK,
			// Token: 0x0400003A RID: 58
			OKCached,
			// Token: 0x0400003B RID: 59
			Unknown,
			// Token: 0x0400003C RID: 60
			SDKError,
			// Token: 0x0400003D RID: 61
			UnsupportedProtocol,
			// Token: 0x0400003E RID: 62
			MalformattedUrl,
			// Token: 0x0400003F RID: 63
			CannotResolveProxy,
			// Token: 0x04000040 RID: 64
			CannotResolveHost,
			// Token: 0x04000041 RID: 65
			CannotConnectToHost,
			// Token: 0x04000042 RID: 66
			AccessDenied,
			// Token: 0x04000043 RID: 67
			GenericHttpError,
			// Token: 0x04000044 RID: 68
			WriteError,
			// Token: 0x04000045 RID: 69
			ReadError,
			// Token: 0x04000046 RID: 70
			OutOfMemory,
			// Token: 0x04000047 RID: 71
			Timeout,
			// Token: 0x04000048 RID: 72
			HTTPPostError,
			// Token: 0x04000049 RID: 73
			SSLCannotConnect,
			// Token: 0x0400004A RID: 74
			Aborted,
			// Token: 0x0400004B RID: 75
			TooManyRedirects,
			// Token: 0x0400004C RID: 76
			ReceivedNoData,
			// Token: 0x0400004D RID: 77
			SSLNotSupported,
			// Token: 0x0400004E RID: 78
			FailedToSendData,
			// Token: 0x0400004F RID: 79
			FailedToReceiveData,
			// Token: 0x04000050 RID: 80
			SSLCertificateError,
			// Token: 0x04000051 RID: 81
			SSLCipherNotAvailable,
			// Token: 0x04000052 RID: 82
			SSLCACertError,
			// Token: 0x04000053 RID: 83
			UnrecognizedContentEncoding,
			// Token: 0x04000054 RID: 84
			LoginFailed,
			// Token: 0x04000055 RID: 85
			SSLShutdownFailed,
			// Token: 0x04000056 RID: 86
			RedirectLimitInvalid,
			// Token: 0x04000057 RID: 87
			InvalidRedirect,
			// Token: 0x04000058 RID: 88
			CannotModifyRequest,
			// Token: 0x04000059 RID: 89
			HeaderNameContainsInvalidCharacters,
			// Token: 0x0400005A RID: 90
			HeaderValueContainsInvalidCharacters,
			// Token: 0x0400005B RID: 91
			CannotOverrideSystemHeaders,
			// Token: 0x0400005C RID: 92
			AlreadySent,
			// Token: 0x0400005D RID: 93
			InvalidMethod,
			// Token: 0x0400005E RID: 94
			NotImplemented,
			// Token: 0x0400005F RID: 95
			NoInternetConnection,
			// Token: 0x04000060 RID: 96
			DataProcessingError,
			// Token: 0x04000061 RID: 97
			InsecureConnectionNotAllowed
		}

		// Token: 0x02000011 RID: 17
		public enum Result
		{
			// Token: 0x04000063 RID: 99
			InProgress,
			// Token: 0x04000064 RID: 100
			Success,
			// Token: 0x04000065 RID: 101
			ConnectionError,
			// Token: 0x04000066 RID: 102
			ProtocolError,
			// Token: 0x04000067 RID: 103
			DataProcessingError
		}
	}
}
