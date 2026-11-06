using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000011 RID: 17
	public interface IAnimatableProperty<T>
	{
		// Token: 0x06000043 RID: 67
		T Get(AnimationStream stream);

		// Token: 0x06000044 RID: 68
		void Set(AnimationStream stream, T value);
	}
}
