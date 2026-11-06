using System;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x020004BE RID: 1214
	public interface ITransitionAnimations
	{
		// Token: 0x060025DC RID: 9692
		ValueAnimation<float> Start(float from, float to, int durationMs, Action<VisualElement, float> onValueChanged);

		// Token: 0x060025DD RID: 9693
		ValueAnimation<Rect> Start(Rect from, Rect to, int durationMs, Action<VisualElement, Rect> onValueChanged);

		// Token: 0x060025DE RID: 9694
		ValueAnimation<Color> Start(Color from, Color to, int durationMs, Action<VisualElement, Color> onValueChanged);

		// Token: 0x060025DF RID: 9695
		ValueAnimation<Vector3> Start(Vector3 from, Vector3 to, int durationMs, Action<VisualElement, Vector3> onValueChanged);

		// Token: 0x060025E0 RID: 9696
		ValueAnimation<Vector2> Start(Vector2 from, Vector2 to, int durationMs, Action<VisualElement, Vector2> onValueChanged);

		// Token: 0x060025E1 RID: 9697
		ValueAnimation<Quaternion> Start(Quaternion from, Quaternion to, int durationMs, Action<VisualElement, Quaternion> onValueChanged);

		// Token: 0x060025E2 RID: 9698
		ValueAnimation<StyleValues> Start(StyleValues from, StyleValues to, int durationMs);

		// Token: 0x060025E3 RID: 9699
		ValueAnimation<StyleValues> Start(StyleValues to, int durationMs);

		// Token: 0x060025E4 RID: 9700
		ValueAnimation<float> Start(Func<VisualElement, float> fromValueGetter, float to, int durationMs, Action<VisualElement, float> onValueChanged);

		// Token: 0x060025E5 RID: 9701
		ValueAnimation<Rect> Start(Func<VisualElement, Rect> fromValueGetter, Rect to, int durationMs, Action<VisualElement, Rect> onValueChanged);

		// Token: 0x060025E6 RID: 9702
		ValueAnimation<Color> Start(Func<VisualElement, Color> fromValueGetter, Color to, int durationMs, Action<VisualElement, Color> onValueChanged);

		// Token: 0x060025E7 RID: 9703
		ValueAnimation<Vector3> Start(Func<VisualElement, Vector3> fromValueGetter, Vector3 to, int durationMs, Action<VisualElement, Vector3> onValueChanged);

		// Token: 0x060025E8 RID: 9704
		ValueAnimation<Vector2> Start(Func<VisualElement, Vector2> fromValueGetter, Vector2 to, int durationMs, Action<VisualElement, Vector2> onValueChanged);

		// Token: 0x060025E9 RID: 9705
		ValueAnimation<Quaternion> Start(Func<VisualElement, Quaternion> fromValueGetter, Quaternion to, int durationMs, Action<VisualElement, Quaternion> onValueChanged);

		// Token: 0x060025EA RID: 9706
		ValueAnimation<Rect> Layout(Rect to, int durationMs);

		// Token: 0x060025EB RID: 9707
		ValueAnimation<Vector2> TopLeft(Vector2 to, int durationMs);

		// Token: 0x060025EC RID: 9708
		ValueAnimation<Vector2> Size(Vector2 to, int durationMs);

		// Token: 0x060025ED RID: 9709
		ValueAnimation<float> Scale(float to, int duration);

		// Token: 0x060025EE RID: 9710
		ValueAnimation<Vector3> Position(Vector3 to, int duration);

		// Token: 0x060025EF RID: 9711
		ValueAnimation<Quaternion> Rotation(Quaternion to, int duration);
	}
}
