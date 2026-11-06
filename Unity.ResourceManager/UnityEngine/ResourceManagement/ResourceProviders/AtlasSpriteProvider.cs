using System;
using System.ComponentModel;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.U2D;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000032 RID: 50
	[DisplayName("Sprites from Atlases Provider")]
	public class AtlasSpriteProvider : ResourceProviderBase
	{
		// Token: 0x06000163 RID: 355 RVA: 0x00006714 File Offset: 0x00004914
		public override void Provide(ProvideHandle providerInterface)
		{
			SpriteAtlas dependency = providerInterface.GetDependency<SpriteAtlas>(0);
			if (dependency == null)
			{
				providerInterface.Complete<Sprite>(null, false, new Exception("Sprite atlas failed to load for location " + providerInterface.Location.PrimaryKey + "."));
				return;
			}
			string text;
			string text2;
			ResourceManagerConfig.ExtractKeyAndSubKey(providerInterface.ResourceManager.TransformInternalId(providerInterface.Location), out text, out text2);
			string text3 = (string.IsNullOrEmpty(text2) ? text : text2);
			Sprite sprite = dependency.GetSprite(text3);
			providerInterface.Complete<Sprite>(sprite, sprite != null, (sprite != null) ? null : new Exception("Sprite failed to load for location " + providerInterface.Location.PrimaryKey + "."));
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000067D0 File Offset: 0x000049D0
		public override void Release(IResourceLocation location, object obj)
		{
			Sprite sprite = obj as Sprite;
			if (sprite != null)
			{
				Object.Destroy(sprite);
			}
		}
	}
}
