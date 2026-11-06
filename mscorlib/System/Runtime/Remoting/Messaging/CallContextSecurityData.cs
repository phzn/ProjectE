using System;
using System.Security.Principal;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000606 RID: 1542
	[Serializable]
	internal class CallContextSecurityData : ICloneable
	{
		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06003A6E RID: 14958 RVA: 0x000CCE9C File Offset: 0x000CB09C
		// (set) Token: 0x06003A6F RID: 14959 RVA: 0x000CCEA4 File Offset: 0x000CB0A4
		internal IPrincipal Principal
		{
			get
			{
				return this._principal;
			}
			set
			{
				this._principal = value;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06003A70 RID: 14960 RVA: 0x000CCEAD File Offset: 0x000CB0AD
		internal bool HasInfo
		{
			get
			{
				return this._principal != null;
			}
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x000CCEB8 File Offset: 0x000CB0B8
		public object Clone()
		{
			return new CallContextSecurityData
			{
				_principal = this._principal
			};
		}

		// Token: 0x04002657 RID: 9815
		private IPrincipal _principal;
	}
}
