using System;
using System.Collections;

namespace System.Net.Configuration
{
	// Token: 0x02000762 RID: 1890
	internal class ConnectionManagementData
	{
		// Token: 0x06003BA6 RID: 15270 RVA: 0x000CC69C File Offset: 0x000CA89C
		public ConnectionManagementData(object parent)
		{
			this.data = new Hashtable(CaseInsensitiveHashCodeProvider.DefaultInvariant, CaseInsensitiveComparer.DefaultInvariant);
			if (parent != null && parent is ConnectionManagementData)
			{
				ConnectionManagementData connectionManagementData = (ConnectionManagementData)parent;
				foreach (object obj in connectionManagementData.data.Keys)
				{
					string text = (string)obj;
					this.data[text] = connectionManagementData.data[text];
				}
			}
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x000CC738 File Offset: 0x000CA938
		public void Add(string address, string nconns)
		{
			if (nconns == null || nconns == "")
			{
				nconns = "2";
			}
			this.data[address] = uint.Parse(nconns);
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x000CC768 File Offset: 0x000CA968
		public void Add(string address, int nconns)
		{
			this.data[address] = (uint)nconns;
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x000CC77C File Offset: 0x000CA97C
		public void Remove(string address)
		{
			this.data.Remove(address);
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x000CC78A File Offset: 0x000CA98A
		public void Clear()
		{
			this.data.Clear();
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x000CC798 File Offset: 0x000CA998
		public uint GetMaxConnections(string hostOrIP)
		{
			object obj = this.data[hostOrIP];
			if (obj == null)
			{
				obj = this.data["*"];
			}
			if (obj == null)
			{
				return 2U;
			}
			return (uint)obj;
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06003BAC RID: 15276 RVA: 0x000CC7D1 File Offset: 0x000CA9D1
		public Hashtable Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x04002383 RID: 9091
		private Hashtable data;

		// Token: 0x04002384 RID: 9092
		private const int defaultMaxConnections = 2;
	}
}
