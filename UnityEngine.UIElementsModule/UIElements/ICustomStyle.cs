using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002CC RID: 716
	public interface ICustomStyle
	{
		// Token: 0x0600155C RID: 5468
		bool TryGetValue(CustomStyleProperty<float> property, out float value);

		// Token: 0x0600155D RID: 5469
		bool TryGetValue(CustomStyleProperty<int> property, out int value);

		// Token: 0x0600155E RID: 5470
		bool TryGetValue(CustomStyleProperty<bool> property, out bool value);

		// Token: 0x0600155F RID: 5471
		bool TryGetValue(CustomStyleProperty<Color> property, out Color value);

		// Token: 0x06001560 RID: 5472
		bool TryGetValue(CustomStyleProperty<Texture2D> property, out Texture2D value);

		// Token: 0x06001561 RID: 5473
		bool TryGetValue(CustomStyleProperty<Sprite> property, out Sprite value);

		// Token: 0x06001562 RID: 5474
		bool TryGetValue(CustomStyleProperty<VectorImage> property, out VectorImage value);

		// Token: 0x06001563 RID: 5475
		bool TryGetValue<T>(CustomStyleProperty<T> property, out T value) where T : Object;

		// Token: 0x06001564 RID: 5476
		bool TryGetValue(CustomStyleProperty<string> property, out string value);
	}
}
