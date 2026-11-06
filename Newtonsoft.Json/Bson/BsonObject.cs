using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000106 RID: 262
	internal class BsonObject : BsonToken, IEnumerable<BsonProperty>, IEnumerable
	{
		// Token: 0x06000D80 RID: 3456 RVA: 0x00035CE4 File Offset: 0x00033EE4
		public void Add(string name, BsonToken token)
		{
			this._children.Add(new BsonProperty
			{
				Name = new BsonString(name, false),
				Value = token
			});
			token.Parent = this;
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x00035D11 File Offset: 0x00033F11
		public override BsonType Type
		{
			get
			{
				return BsonType.Object;
			}
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00035D14 File Offset: 0x00033F14
		public IEnumerator<BsonProperty> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00035D26 File Offset: 0x00033F26
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000435 RID: 1077
		private readonly List<BsonProperty> _children = new List<BsonProperty>();
	}
}
