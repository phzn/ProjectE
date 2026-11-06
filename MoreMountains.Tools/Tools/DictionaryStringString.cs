using System;
using System.Runtime.Serialization;

namespace MoreMountains.Tools
{
	// Token: 0x02000134 RID: 308
	[Serializable]
	public class DictionaryStringString : MMSerializableDictionary<string, string>
	{
		// Token: 0x060006F0 RID: 1776 RVA: 0x00022199 File Offset: 0x00020399
		public DictionaryStringString()
		{
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x000221A1 File Offset: 0x000203A1
		protected DictionaryStringString(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
