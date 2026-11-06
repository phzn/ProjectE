using System;
using UnityEngine.U2D;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	public class AssetReferenceAtlasedSprite : AssetReferenceT<Sprite>
	{
		// Token: 0x06000139 RID: 313 RVA: 0x00005B47 File Offset: 0x00003D47
		public AssetReferenceAtlasedSprite(string guid)
			: base(guid)
		{
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005B50 File Offset: 0x00003D50
		public override bool ValidateAsset(Object obj)
		{
			return obj is SpriteAtlas;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00005B5B File Offset: 0x00003D5B
		public override bool ValidateAsset(string path)
		{
			return false;
		}
	}
}
