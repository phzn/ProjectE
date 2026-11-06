using System;
using System.Collections;

namespace System.Runtime.Remoting
{
	// Token: 0x02000567 RID: 1383
	internal class ChannelData
	{
		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x0600365E RID: 13918 RVA: 0x000C44B5 File Offset: 0x000C26B5
		internal ArrayList ServerProviders
		{
			get
			{
				if (this._serverProviders == null)
				{
					this._serverProviders = new ArrayList();
				}
				return this._serverProviders;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x0600365F RID: 13919 RVA: 0x000C44D0 File Offset: 0x000C26D0
		public ArrayList ClientProviders
		{
			get
			{
				if (this._clientProviders == null)
				{
					this._clientProviders = new ArrayList();
				}
				return this._clientProviders;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06003660 RID: 13920 RVA: 0x000C44EB File Offset: 0x000C26EB
		public Hashtable CustomProperties
		{
			get
			{
				if (this._customProperties == null)
				{
					this._customProperties = new Hashtable();
				}
				return this._customProperties;
			}
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x000C4508 File Offset: 0x000C2708
		public void CopyFrom(ChannelData other)
		{
			if (this.Ref == null)
			{
				this.Ref = other.Ref;
			}
			if (this.Id == null)
			{
				this.Id = other.Id;
			}
			if (this.Type == null)
			{
				this.Type = other.Type;
			}
			if (this.DelayLoadAsClientChannel == null)
			{
				this.DelayLoadAsClientChannel = other.DelayLoadAsClientChannel;
			}
			if (other._customProperties != null)
			{
				foreach (object obj in other._customProperties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (!this.CustomProperties.ContainsKey(dictionaryEntry.Key))
					{
						this.CustomProperties[dictionaryEntry.Key] = dictionaryEntry.Value;
					}
				}
			}
			if (this._serverProviders == null && other._serverProviders != null)
			{
				foreach (object obj2 in other._serverProviders)
				{
					ProviderData providerData = (ProviderData)obj2;
					ProviderData providerData2 = new ProviderData();
					providerData2.CopyFrom(providerData);
					this.ServerProviders.Add(providerData2);
				}
			}
			if (this._clientProviders == null && other._clientProviders != null)
			{
				foreach (object obj3 in other._clientProviders)
				{
					ProviderData providerData3 = (ProviderData)obj3;
					ProviderData providerData4 = new ProviderData();
					providerData4.CopyFrom(providerData3);
					this.ClientProviders.Add(providerData4);
				}
			}
		}

		// Token: 0x04002542 RID: 9538
		internal string Ref;

		// Token: 0x04002543 RID: 9539
		internal string Type;

		// Token: 0x04002544 RID: 9540
		internal string Id;

		// Token: 0x04002545 RID: 9541
		internal string DelayLoadAsClientChannel;

		// Token: 0x04002546 RID: 9542
		private ArrayList _serverProviders = new ArrayList();

		// Token: 0x04002547 RID: 9543
		private ArrayList _clientProviders = new ArrayList();

		// Token: 0x04002548 RID: 9544
		private Hashtable _customProperties = new Hashtable();
	}
}
