using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000107 RID: 263
	internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
	{
		// Token: 0x06000D85 RID: 3461 RVA: 0x00035D41 File Offset: 0x00033F41
		public void Add(BsonToken token)
		{
			this._children.Add(token);
			token.Parent = this;
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x00035D56 File Offset: 0x00033F56
		public override BsonType Type
		{
			get
			{
				return BsonType.Array;
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00035D59 File Offset: 0x00033F59
		public IEnumerator<BsonToken> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00035D6B File Offset: 0x00033F6B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000436 RID: 1078
		private readonly List<BsonToken> _children = new List<BsonToken>();
	}
}
