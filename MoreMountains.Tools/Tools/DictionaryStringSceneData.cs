using System;
using System.Runtime.Serialization;

namespace MoreMountains.Tools
{
	// Token: 0x02000133 RID: 307
	[Serializable]
	public class DictionaryStringSceneData : MMSerializableDictionary<string, MMPersistenceSceneData>
	{
		// Token: 0x060006EE RID: 1774 RVA: 0x00022187 File Offset: 0x00020387
		public DictionaryStringSceneData()
		{
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0002218F File Offset: 0x0002038F
		protected DictionaryStringSceneData(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
