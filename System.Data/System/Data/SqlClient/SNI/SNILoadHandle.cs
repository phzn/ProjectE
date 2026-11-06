using System;
using System.Threading;

namespace System.Data.SqlClient.SNI
{
	// Token: 0x02000291 RID: 657
	internal class SNILoadHandle
	{
		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001E4B RID: 7755 RVA: 0x0008F420 File Offset: 0x0008D620
		// (set) Token: 0x06001E4C RID: 7756 RVA: 0x0008F42D File Offset: 0x0008D62D
		public SNIError LastError
		{
			get
			{
				return this._lastError.Value;
			}
			set
			{
				this._lastError.Value = value;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001E4D RID: 7757 RVA: 0x0008F43B File Offset: 0x0008D63B
		public uint Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001E4E RID: 7758 RVA: 0x0008F443 File Offset: 0x0008D643
		public EncryptionOptions Options
		{
			get
			{
				return this._encryptionOption;
			}
		}

		// Token: 0x04001502 RID: 5378
		public static readonly SNILoadHandle SingletonInstance = new SNILoadHandle();

		// Token: 0x04001503 RID: 5379
		public readonly EncryptionOptions _encryptionOption;

		// Token: 0x04001504 RID: 5380
		public ThreadLocal<SNIError> _lastError = new ThreadLocal<SNIError>(() => new SNIError(SNIProviders.INVALID_PROV, 0U, 0U, string.Empty));

		// Token: 0x04001505 RID: 5381
		private readonly uint _status;
	}
}
