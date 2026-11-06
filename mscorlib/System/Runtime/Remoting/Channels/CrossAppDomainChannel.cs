using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005AD RID: 1453
	[Serializable]
	internal class CrossAppDomainChannel : IChannel, IChannelSender, IChannelReceiver
	{
		// Token: 0x06003853 RID: 14419 RVA: 0x000CA2F4 File Offset: 0x000C84F4
		internal static void RegisterCrossAppDomainChannel()
		{
			object obj = CrossAppDomainChannel.s_lock;
			lock (obj)
			{
				ChannelServices.RegisterChannel(new CrossAppDomainChannel());
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06003854 RID: 14420 RVA: 0x000CA338 File Offset: 0x000C8538
		public virtual string ChannelName
		{
			get
			{
				return "MONOCAD";
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06003855 RID: 14421 RVA: 0x000CA33F File Offset: 0x000C853F
		public virtual int ChannelPriority
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x000CA343 File Offset: 0x000C8543
		public string Parse(string url, out string objectURI)
		{
			objectURI = url;
			return null;
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06003857 RID: 14423 RVA: 0x000CA349 File Offset: 0x000C8549
		public virtual object ChannelData
		{
			get
			{
				return new CrossAppDomainData(Thread.GetDomainID());
			}
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x000CA355 File Offset: 0x000C8555
		public virtual string[] GetUrlsForUri(string objectURI)
		{
			throw new NotSupportedException("CrossAppdomain channel dont support UrlsForUri");
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public virtual void StartListening(object data)
		{
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public virtual void StopListening(object data)
		{
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x000CA364 File Offset: 0x000C8564
		public virtual IMessageSink CreateMessageSink(string url, object data, out string uri)
		{
			uri = null;
			if (data != null)
			{
				CrossAppDomainData crossAppDomainData = data as CrossAppDomainData;
				if (crossAppDomainData != null && crossAppDomainData.ProcessID == RemotingConfiguration.ProcessId)
				{
					return CrossAppDomainSink.GetSink(crossAppDomainData.DomainID);
				}
			}
			if (url != null && url.StartsWith("MONOCAD"))
			{
				throw new NotSupportedException("Can't create a named channel via crossappdomain");
			}
			return null;
		}

		// Token: 0x040025E0 RID: 9696
		private const string _strName = "MONOCAD";

		// Token: 0x040025E1 RID: 9697
		private static object s_lock = new object();
	}
}
