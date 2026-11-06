using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000103 RID: 259
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonObjectId
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x0003505D File Offset: 0x0003325D
		public byte[] Value { get; }

		// Token: 0x06000D57 RID: 3415 RVA: 0x00035065 File Offset: 0x00033265
		public BsonObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new ArgumentException("An ObjectId must be 12 bytes", "value");
			}
			this.Value = value;
		}
	}
}
