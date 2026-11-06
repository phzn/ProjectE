using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000680 RID: 1664
	internal sealed class EndPointListener
	{
		// Token: 0x06003462 RID: 13410 RVA: 0x000B66F4 File Offset: 0x000B48F4
		public EndPointListener(HttpListener listener, IPAddress addr, int port, bool secure)
		{
			this.listener = listener;
			if (secure)
			{
				this.secure = secure;
				this.cert = listener.LoadCertificateAndKey(addr, port);
			}
			this.endpoint = new IPEndPoint(addr, port);
			this.sock = new Socket(addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			this.sock.Bind(this.endpoint);
			this.sock.Listen(500);
			SocketAsyncEventArgs e = new SocketAsyncEventArgs();
			e.UserToken = this;
			e.Completed += EndPointListener.OnAccept;
			Socket socket = null;
			EndPointListener.Accept(this.sock, e, ref socket);
			this.prefixes = new Hashtable();
			this.unregistered = new Dictionary<HttpConnection, HttpConnection>();
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06003463 RID: 13411 RVA: 0x000B67AE File Offset: 0x000B49AE
		internal HttpListener Listener
		{
			get
			{
				return this.listener;
			}
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x000B67B8 File Offset: 0x000B49B8
		private static void Accept(Socket socket, SocketAsyncEventArgs e, ref Socket accepted)
		{
			e.AcceptSocket = null;
			bool flag;
			try
			{
				flag = socket.AcceptAsync(e);
			}
			catch
			{
				if (accepted != null)
				{
					try
					{
						accepted.Close();
					}
					catch
					{
					}
					accepted = null;
				}
				return;
			}
			if (!flag)
			{
				EndPointListener.ProcessAccept(e);
			}
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x000B6814 File Offset: 0x000B4A14
		private static void ProcessAccept(SocketAsyncEventArgs args)
		{
			Socket socket = null;
			if (args.SocketError == SocketError.Success)
			{
				socket = args.AcceptSocket;
			}
			EndPointListener endPointListener = (EndPointListener)args.UserToken;
			EndPointListener.Accept(endPointListener.sock, args, ref socket);
			if (socket == null)
			{
				return;
			}
			if (endPointListener.secure && endPointListener.cert == null)
			{
				socket.Close();
				return;
			}
			HttpConnection httpConnection;
			try
			{
				httpConnection = new HttpConnection(socket, endPointListener, endPointListener.secure, endPointListener.cert);
			}
			catch
			{
				socket.Close();
				return;
			}
			Dictionary<HttpConnection, HttpConnection> dictionary = endPointListener.unregistered;
			lock (dictionary)
			{
				endPointListener.unregistered[httpConnection] = httpConnection;
			}
			httpConnection.BeginReadRequest();
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x000B68D8 File Offset: 0x000B4AD8
		private static void OnAccept(object sender, SocketAsyncEventArgs e)
		{
			EndPointListener.ProcessAccept(e);
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x000B68E0 File Offset: 0x000B4AE0
		internal void RemoveConnection(HttpConnection conn)
		{
			Dictionary<HttpConnection, HttpConnection> dictionary = this.unregistered;
			lock (dictionary)
			{
				this.unregistered.Remove(conn);
			}
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x000B6928 File Offset: 0x000B4B28
		public bool BindContext(HttpListenerContext context)
		{
			HttpListenerRequest request = context.Request;
			ListenerPrefix listenerPrefix;
			HttpListener httpListener = this.SearchListener(request.Url, out listenerPrefix);
			if (httpListener == null)
			{
				return false;
			}
			context.Listener = httpListener;
			context.Connection.Prefix = listenerPrefix;
			return true;
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x000B6964 File Offset: 0x000B4B64
		public void UnbindContext(HttpListenerContext context)
		{
			if (context == null || context.Request == null)
			{
				return;
			}
			context.Listener.UnregisterContext(context);
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x000B6980 File Offset: 0x000B4B80
		private HttpListener SearchListener(Uri uri, out ListenerPrefix prefix)
		{
			prefix = null;
			if (uri == null)
			{
				return null;
			}
			string host = uri.Host;
			int port = uri.Port;
			string text = WebUtility.UrlDecode(uri.AbsolutePath);
			string text2 = ((text[text.Length - 1] == '/') ? text : (text + "/"));
			HttpListener httpListener = null;
			int num = -1;
			if (host != null && host != "")
			{
				Hashtable hashtable = this.prefixes;
				foreach (object obj in hashtable.Keys)
				{
					ListenerPrefix listenerPrefix = (ListenerPrefix)obj;
					string path = listenerPrefix.Path;
					if (path.Length >= num && !(listenerPrefix.Host != host) && listenerPrefix.Port == port && (text.StartsWith(path) || text2.StartsWith(path)))
					{
						num = path.Length;
						httpListener = (HttpListener)hashtable[listenerPrefix];
						prefix = listenerPrefix;
					}
				}
				if (num != -1)
				{
					return httpListener;
				}
			}
			ArrayList arrayList = this.unhandled;
			httpListener = this.MatchFromList(host, text, arrayList, out prefix);
			if (text != text2 && httpListener == null)
			{
				httpListener = this.MatchFromList(host, text2, arrayList, out prefix);
			}
			if (httpListener != null)
			{
				return httpListener;
			}
			arrayList = this.all;
			httpListener = this.MatchFromList(host, text, arrayList, out prefix);
			if (text != text2 && httpListener == null)
			{
				httpListener = this.MatchFromList(host, text2, arrayList, out prefix);
			}
			if (httpListener != null)
			{
				return httpListener;
			}
			return null;
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x000B6B1C File Offset: 0x000B4D1C
		private HttpListener MatchFromList(string host, string path, ArrayList list, out ListenerPrefix prefix)
		{
			prefix = null;
			if (list == null)
			{
				return null;
			}
			HttpListener httpListener = null;
			int num = -1;
			foreach (object obj in list)
			{
				ListenerPrefix listenerPrefix = (ListenerPrefix)obj;
				string path2 = listenerPrefix.Path;
				if (path2.Length >= num && path.StartsWith(path2))
				{
					num = path2.Length;
					httpListener = listenerPrefix.Listener;
					prefix = listenerPrefix;
				}
			}
			return httpListener;
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x000B6BAC File Offset: 0x000B4DAC
		private void AddSpecial(ArrayList coll, ListenerPrefix prefix)
		{
			if (coll == null)
			{
				return;
			}
			using (IEnumerator enumerator = coll.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((ListenerPrefix)enumerator.Current).Path == prefix.Path)
					{
						throw new HttpListenerException(400, "Prefix already in use.");
					}
				}
			}
			coll.Add(prefix);
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x000B6C28 File Offset: 0x000B4E28
		private bool RemoveSpecial(ArrayList coll, ListenerPrefix prefix)
		{
			if (coll == null)
			{
				return false;
			}
			int count = coll.Count;
			for (int i = 0; i < count; i++)
			{
				if (((ListenerPrefix)coll[i]).Path == prefix.Path)
				{
					coll.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x000B6C78 File Offset: 0x000B4E78
		private void CheckIfRemove()
		{
			if (this.prefixes.Count > 0)
			{
				return;
			}
			ArrayList arrayList = this.unhandled;
			if (arrayList != null && arrayList.Count > 0)
			{
				return;
			}
			arrayList = this.all;
			if (arrayList != null && arrayList.Count > 0)
			{
				return;
			}
			EndPointManager.RemoveEndPoint(this, this.endpoint);
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x000B6CC8 File Offset: 0x000B4EC8
		public void Close()
		{
			this.sock.Close();
			Dictionary<HttpConnection, HttpConnection> dictionary = this.unregistered;
			lock (dictionary)
			{
				foreach (HttpConnection httpConnection in new List<HttpConnection>(this.unregistered.Keys))
				{
					httpConnection.Close(true);
				}
				this.unregistered.Clear();
			}
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x000B6D64 File Offset: 0x000B4F64
		public void AddPrefix(ListenerPrefix prefix, HttpListener listener)
		{
			if (prefix.Host == "*")
			{
				ArrayList arrayList;
				ArrayList arrayList2;
				do
				{
					arrayList = this.unhandled;
					arrayList2 = ((arrayList != null) ? ((ArrayList)arrayList.Clone()) : new ArrayList());
					prefix.Listener = listener;
					this.AddSpecial(arrayList2, prefix);
				}
				while (Interlocked.CompareExchange<ArrayList>(ref this.unhandled, arrayList2, arrayList) != arrayList);
				return;
			}
			if (prefix.Host == "+")
			{
				ArrayList arrayList;
				ArrayList arrayList2;
				do
				{
					arrayList = this.all;
					arrayList2 = ((arrayList != null) ? ((ArrayList)arrayList.Clone()) : new ArrayList());
					prefix.Listener = listener;
					this.AddSpecial(arrayList2, prefix);
				}
				while (Interlocked.CompareExchange<ArrayList>(ref this.all, arrayList2, arrayList) != arrayList);
				return;
			}
			Hashtable hashtable;
			for (;;)
			{
				hashtable = this.prefixes;
				if (hashtable.ContainsKey(prefix))
				{
					break;
				}
				Hashtable hashtable2 = (Hashtable)hashtable.Clone();
				hashtable2[prefix] = listener;
				if (Interlocked.CompareExchange<Hashtable>(ref this.prefixes, hashtable2, hashtable) == hashtable)
				{
					return;
				}
			}
			if ((HttpListener)hashtable[prefix] != listener)
			{
				throw new HttpListenerException(400, "There's another listener for " + ((prefix != null) ? prefix.ToString() : null));
			}
			return;
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x000B6E78 File Offset: 0x000B5078
		public void RemovePrefix(ListenerPrefix prefix, HttpListener listener)
		{
			if (prefix.Host == "*")
			{
				ArrayList arrayList;
				ArrayList arrayList2;
				do
				{
					arrayList = this.unhandled;
					arrayList2 = ((arrayList != null) ? ((ArrayList)arrayList.Clone()) : new ArrayList());
				}
				while (this.RemoveSpecial(arrayList2, prefix) && Interlocked.CompareExchange<ArrayList>(ref this.unhandled, arrayList2, arrayList) != arrayList);
				this.CheckIfRemove();
				return;
			}
			if (prefix.Host == "+")
			{
				ArrayList arrayList;
				ArrayList arrayList2;
				do
				{
					arrayList = this.all;
					arrayList2 = ((arrayList != null) ? ((ArrayList)arrayList.Clone()) : new ArrayList());
				}
				while (this.RemoveSpecial(arrayList2, prefix) && Interlocked.CompareExchange<ArrayList>(ref this.all, arrayList2, arrayList) != arrayList);
				this.CheckIfRemove();
				return;
			}
			Hashtable hashtable;
			Hashtable hashtable2;
			do
			{
				hashtable = this.prefixes;
				if (!hashtable.ContainsKey(prefix))
				{
					break;
				}
				hashtable2 = (Hashtable)hashtable.Clone();
				hashtable2.Remove(prefix);
			}
			while (Interlocked.CompareExchange<Hashtable>(ref this.prefixes, hashtable2, hashtable) != hashtable);
			this.CheckIfRemove();
		}

		// Token: 0x04001E8F RID: 7823
		private HttpListener listener;

		// Token: 0x04001E90 RID: 7824
		private IPEndPoint endpoint;

		// Token: 0x04001E91 RID: 7825
		private Socket sock;

		// Token: 0x04001E92 RID: 7826
		private Hashtable prefixes;

		// Token: 0x04001E93 RID: 7827
		private ArrayList unhandled;

		// Token: 0x04001E94 RID: 7828
		private ArrayList all;

		// Token: 0x04001E95 RID: 7829
		private X509Certificate cert;

		// Token: 0x04001E96 RID: 7830
		private bool secure;

		// Token: 0x04001E97 RID: 7831
		private Dictionary<HttpConnection, HttpConnection> unregistered;
	}
}
