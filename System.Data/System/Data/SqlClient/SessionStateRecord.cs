using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000207 RID: 519
	internal class SessionStateRecord
	{
		// Token: 0x04001050 RID: 4176
		internal bool _recoverable;

		// Token: 0x04001051 RID: 4177
		internal uint _version;

		// Token: 0x04001052 RID: 4178
		internal int _dataLength;

		// Token: 0x04001053 RID: 4179
		internal byte[] _data;
	}
}
