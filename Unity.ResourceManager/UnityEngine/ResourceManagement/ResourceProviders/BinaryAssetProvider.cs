using System;
using System.ComponentModel;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000033 RID: 51
	[DisplayName("Binary Asset Provider")]
	internal class BinaryAssetProvider<TAdapter> : BinaryDataProvider where TAdapter : BinaryStorageBuffer.ISerializationAdapter, new()
	{
		// Token: 0x06000166 RID: 358 RVA: 0x000067F5 File Offset: 0x000049F5
		public override object Convert(Type type, byte[] data)
		{
			return new BinaryStorageBuffer.Reader(data, 512, new BinaryStorageBuffer.ISerializationAdapter[]
			{
				new TAdapter()
			}).ReadObject(type, 0U, false);
		}
	}
}
