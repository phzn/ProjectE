using System;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x02000011 RID: 17
	[Serializable]
	public class AssetReferenceSprite : AssetReferenceT<Sprite>
	{
		// Token: 0x06000137 RID: 311 RVA: 0x00005B3B File Offset: 0x00003D3B
		public AssetReferenceSprite(string guid)
			: base(guid)
		{
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005B44 File Offset: 0x00003D44
		public override bool ValidateAsset(string path)
		{
			return false;
		}
	}
}
