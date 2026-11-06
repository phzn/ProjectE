using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using DA_Assets.Networking;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000356 RID: 854
	[Serializable]
	public class Authorizer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001681 RID: 5761 RVA: 0x00093CEE File Offset: 0x00091EEE
		// (set) Token: 0x06001682 RID: 5762 RVA: 0x00093CF6 File Offset: 0x00091EF6
		public int SelectedTableIndex
		{
			get
			{
				return this._selectedTableIndex;
			}
			set
			{
				base.SetValue<int>(ref this._selectedTableIndex, value);
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001683 RID: 5763 RVA: 0x00093D05 File Offset: 0x00091F05
		// (set) Token: 0x06001684 RID: 5764 RVA: 0x00093D0D File Offset: 0x00091F0D
		public GUIContent[] Options
		{
			get
			{
				return this._options;
			}
			set
			{
				base.SetValue<GUIContent[]>(ref this._options, value);
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x00093D1C File Offset: 0x00091F1C
		// (set) Token: 0x06001686 RID: 5766 RVA: 0x00093D24 File Offset: 0x00091F24
		public List<FigmaSessionItem> RecentSessions
		{
			get
			{
				return this._recentSessions;
			}
			set
			{
				base.SetValue<List<FigmaSessionItem>>(ref this._recentSessions, value);
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001687 RID: 5767 RVA: 0x00093D33 File Offset: 0x00091F33
		// (set) Token: 0x06001688 RID: 5768 RVA: 0x00093D3B File Offset: 0x00091F3B
		public FigmaSessionItem CurrentSession { get; set; }

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001689 RID: 5769 RVA: 0x00093D44 File Offset: 0x00091F44
		public string Token
		{
			get
			{
				return this.CurrentSession.AuthResult.AccessToken;
			}
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00093D67 File Offset: 0x00091F67
		public override void OnLink()
		{
			base.OnLink();
			this.Init();
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x00093D78 File Offset: 0x00091F78
		public async void Init()
		{
			if (!this.monoBeh.IsJsonNetExists())
			{
				this._options = new GUIContent[]
				{
					new GUIContent("Newtonsoft.Json is not installed.")
				};
				DALogger.LogError(FcuLocKey.log_cant_find_package.Localize(new object[] { "com.unity.nuget.newtonsoft-json" }));
			}
			else
			{
				List<FigmaSessionItem> list = await this.monoBeh.Authorizer.GetSessionItems();
				this._recentSessions = list;
				this._options = await this.GetOptions(false);
				this._options = await this.GetOptions(true);
			}
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x00093DB0 File Offset: 0x00091FB0
		private async Task<GUIContent[]> GetOptions(bool includeImages)
		{
			List<GUIContent> options = new List<GUIContent>();
			if (this._recentSessions.IsEmpty<FigmaSessionItem>())
			{
				options.Add(new GUIContent(FcuLocKey.label_no_recent_sessions.Localize(Array.Empty<object>())));
			}
			else
			{
				foreach (FigmaSessionItem session in this._recentSessions)
				{
					if (includeImages)
					{
						Texture2D texture2D = await RequestSender.LoadImage(session.User.ImgUrl);
						options.Add(new GUIContent(session.User.Name + " | " + session.User.Email, texture2D));
					}
					else
					{
						options.Add(new GUIContent(session.User.Name + " | " + session.User.Email));
					}
					session = default(FigmaSessionItem);
				}
				List<FigmaSessionItem>.Enumerator enumerator = default(List<FigmaSessionItem>.Enumerator);
			}
			return options.ToArray();
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x00093DFC File Offset: 0x00091FFC
		public void Auth()
		{
			if (!this.monoBeh.IsJsonNetExists())
			{
				DALogger.LogError(FcuLocKey.log_cant_find_package.Localize(new object[] { "com.unity.nuget.newtonsoft-json" }));
				return;
			}
			this.monoBeh.AssetTools.StopImport(StopImportReason.Hidden);
			CancellationTokenSource cancellationTokenSource = this.monoBeh.CancellationTokenController.CreateNew(TokenType.Import);
			this.AuthAsync(cancellationTokenSource);
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00093E5C File Offset: 0x0009205C
		public async Task AuthAsync(CancellationTokenSource cts)
		{
			DAResult<AuthResult> daresult = await this.StartAuthThread();
			if (daresult.Success)
			{
				this.monoBeh.Authorizer.AddNew(daresult.Object, AuthType.OAuth2);
			}
			else
			{
				DALogger.LogError(FcuLocKey.log_cant_auth.Localize(new object[]
				{
					daresult.Error.Status,
					daresult.Error.Message,
					"null"
				}));
			}
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00093EA0 File Offset: 0x000920A0
		private async Task<DAResult<AuthResult>> StartAuthThread()
		{
			string code = "";
			bool gettingCode = true;
			Thread thread = null;
			DALogger.Log(FcuLocKey.log_open_auth_page.Localize(Array.Empty<object>()));
			thread = new Thread(delegate(object x)
			{
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Any, 1923);
				socket.Bind(ipendPoint);
				socket.Listen(1);
				Socket socket2 = socket.Accept();
				byte[] array = new byte[1000];
				socket2.Receive(array);
				string @string = Encoding.UTF8.GetString(array);
				string text = "HTTP/1.1 200 OK\nContent-Type: text/html\nConnection: close\n\n\n                    <html>\n                        <head>\n                            <style type='text/css'>body,html{background-color: #000000;color: #fff;font-family: Segoe UI;text-align: center;}h2{left: 0; position: absolute; top: calc(50% - 25px); width: 100%;}</style>\n                            <title>Wait for redirect...</title>\n                            <script type='text/javascript'> window.onload=function(){window.location.href='https://figma.com';}</script>\n                        </head>\n                        <body>\n                            <h2>Authorization completed. The page will close automatically.</h2>\n                        </body>\n                    </html>";
				array = Encoding.UTF8.GetBytes(text);
				NetworkStream networkStream = new NetworkStream(socket2);
				networkStream.Write(array, 0, array.Length);
				networkStream.Flush();
				networkStream.Close();
				socket2.Close();
				socket.Close();
				code = @string.GetBetween("?code=", "&state=");
				gettingCode = false;
				thread.Abort();
			});
			thread.Start();
			int num = Random.Range(0, int.MaxValue);
			Application.OpenURL(string.Format(FcuConfig.OAuthUrl, FcuConfig.ClientId, FcuConfig.RedirectUri, num.ToString()));
			while (gettingCode)
			{
				await Task.Delay(100);
			}
			DARequest darequest = RequestCreator.CreateTokenRequest(code);
			return await this.monoBeh.RequestSender.SendRequest<AuthResult>(darequest);
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00093EE4 File Offset: 0x000920E4
		public bool IsAuthed()
		{
			return !this.CurrentSession.User.Name.IsEmpty() && !this.Token.IsEmpty();
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x00093F20 File Offset: 0x00092120
		public async Task AddNew(AuthResult authResult, AuthType authType)
		{
			DAResult<FigmaUser> daresult = await this.GetCurrentFigmaUser(authResult.AccessToken, authType);
			if (daresult.Success)
			{
				await this.SetLastSession(new FigmaSessionItem
				{
					User = daresult.Object,
					AuthResult = authResult,
					AuthType = authType
				});
				this.monoBeh.Authorizer.Init();
				DALogger.LogSuccess(FcuLocKey.log_auth_complete.Localize(Array.Empty<object>()));
			}
			else
			{
				DALogger.LogError(FcuLocKey.log_cant_auth.Localize(new object[]
				{
					daresult.Error.Status,
					daresult.Error.Message,
					daresult.Error.Exception
				}));
			}
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x00093F74 File Offset: 0x00092174
		private async Task<DAResult<FigmaUser>> GetCurrentFigmaUser(string token, AuthType authType)
		{
			DARequest darequest = new DARequest
			{
				Query = "https://api.figma.com/v1/me",
				RequestType = RequestType.Get,
				RequestHeader = this.monoBeh.RequestSender.GetRequestHeader(token, new AuthType?(authType))
			};
			return await this.monoBeh.RequestSender.SendRequest<FigmaUser>(darequest);
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x00093FC8 File Offset: 0x000921C8
		public async Task TryRestoreSession()
		{
			if (this.monoBeh.IsJsonNetExists())
			{
				if (!this.IsAuthed())
				{
					FigmaSessionItem figmaSessionItem = await this.GetLastSessionItem();
					this.CurrentSession = figmaSessionItem;
				}
			}
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0009400C File Offset: 0x0009220C
		private async Task SetLastSession(FigmaSessionItem sessionItem)
		{
			this.CurrentSession = sessionItem;
			List<FigmaSessionItem> list = await this.GetSessionItems();
			FigmaSessionItem figmaSessionItem = list.FirstOrDefault<FigmaSessionItem>((FigmaSessionItem item) => item.AuthResult.AccessToken == sessionItem.AuthResult.AccessToken);
			list.Remove(figmaSessionItem);
			list.Insert(0, sessionItem);
			if (list.Count > FcuConfig.FigmaSessionsLimit)
			{
				list = list.Take<FigmaSessionItem>(FcuConfig.FigmaSessionsLimit).ToList<FigmaSessionItem>();
			}
			this.SaveDataToPrefs(list);
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00094058 File Offset: 0x00092258
		private async Task<FigmaSessionItem> GetLastSessionItem()
		{
			return (await this.GetSessionItems()).FirstOrDefault<FigmaSessionItem>();
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x0009409C File Offset: 0x0009229C
		public async Task<List<FigmaSessionItem>> GetSessionItems()
		{
			string json = "";
			List<FigmaSessionItem> sessionItems = new List<FigmaSessionItem>();
			List<FigmaSessionItem> list;
			if (json.IsEmpty())
			{
				list = sessionItems;
			}
			else
			{
				await Task.Run(delegate
				{
					try
					{
						sessionItems = DAJson.FromJson<List<FigmaSessionItem>>(json);
					}
					catch
					{
					}
				});
				list = sessionItems;
			}
			return list;
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x00004F31 File Offset: 0x00003131
		private void SaveDataToPrefs(List<FigmaSessionItem> sessionItems)
		{
		}

		// Token: 0x04001836 RID: 6198
		[SerializeField]
		private int _selectedTableIndex;

		// Token: 0x04001837 RID: 6199
		[SerializeField]
		private GUIContent[] _options;

		// Token: 0x04001838 RID: 6200
		[SerializeField]
		private List<FigmaSessionItem> _recentSessions;
	}
}
