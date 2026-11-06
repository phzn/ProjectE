using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000216 RID: 534
	public interface IPointerEvent
	{
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000F78 RID: 3960
		int pointerId { get; }

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000F79 RID: 3961
		string pointerType { get; }

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000F7A RID: 3962
		bool isPrimary { get; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000F7B RID: 3963
		int button { get; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000F7C RID: 3964
		int pressedButtons { get; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000F7D RID: 3965
		Vector3 position { get; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000F7E RID: 3966
		Vector3 localPosition { get; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000F7F RID: 3967
		Vector3 deltaPosition { get; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000F80 RID: 3968
		float deltaTime { get; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000F81 RID: 3969
		int clickCount { get; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000F82 RID: 3970
		float pressure { get; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000F83 RID: 3971
		float tangentialPressure { get; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000F84 RID: 3972
		float altitudeAngle { get; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000F85 RID: 3973
		float azimuthAngle { get; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000F86 RID: 3974
		float twist { get; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000F87 RID: 3975
		Vector2 tilt { get; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000F88 RID: 3976
		PenStatus penStatus { get; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000F89 RID: 3977
		Vector2 radius { get; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000F8A RID: 3978
		Vector2 radiusVariance { get; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000F8B RID: 3979
		EventModifiers modifiers { get; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000F8C RID: 3980
		bool shiftKey { get; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000F8D RID: 3981
		bool ctrlKey { get; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000F8E RID: 3982
		bool commandKey { get; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000F8F RID: 3983
		bool altKey { get; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000F90 RID: 3984
		bool actionKey { get; }
	}
}
