using System;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008B8 RID: 2232
	public interface IOutfitExtension
	{
		// Token: 0x06003BE2 RID: 15330
		string GetID();

		// Token: 0x06003BE3 RID: 15331
		void Initalize(OutfitSystem outfitSystem, Outfit outfit);

		// Token: 0x06003BE4 RID: 15332
		void Execute(OutfitSystem outfitSystem, Outfit outfit);

		// Token: 0x06003BE5 RID: 15333
		object GetValue();

		// Token: 0x06003BE6 RID: 15334
		Type GetValueType();
	}
}
