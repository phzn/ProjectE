using System;
using System.Collections;

namespace Unity.Collections
{
	// Token: 0x0200004A RID: 74
	internal struct ListPair<Key, Value> where Value : IList
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x00006930 File Offset: 0x00004B30
		public ListPair(Key k, Value v)
		{
			this.key = k;
			this.value = v;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00006940 File Offset: 0x00004B40
		public override string ToString()
		{
			string text = string.Format("{0} = [", this.key);
			for (int i = 0; i < this.value.Count; i++)
			{
				string text2 = text;
				object obj = this.value[i];
				text = text2 + ((obj != null) ? obj.ToString() : null);
				if (i < this.value.Count - 1)
				{
					text += ", ";
				}
			}
			return text + "]";
		}

		// Token: 0x040000BC RID: 188
		public Key key;

		// Token: 0x040000BD RID: 189
		public Value value;
	}
}
