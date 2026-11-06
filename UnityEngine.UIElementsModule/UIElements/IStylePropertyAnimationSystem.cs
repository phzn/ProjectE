using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x02000329 RID: 809
	internal interface IStylePropertyAnimationSystem
	{
		// Token: 0x06001B66 RID: 7014
		bool StartTransition(VisualElement owner, StylePropertyId prop, float startValue, float endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B67 RID: 7015
		bool StartTransition(VisualElement owner, StylePropertyId prop, int startValue, int endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B68 RID: 7016
		bool StartTransition(VisualElement owner, StylePropertyId prop, Length startValue, Length endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B69 RID: 7017
		bool StartTransition(VisualElement owner, StylePropertyId prop, Color startValue, Color endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B6A RID: 7018
		bool StartAnimationEnum(VisualElement owner, StylePropertyId prop, int startValue, int endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B6B RID: 7019
		bool StartTransition(VisualElement owner, StylePropertyId prop, Background startValue, Background endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B6C RID: 7020
		bool StartTransition(VisualElement owner, StylePropertyId prop, FontDefinition startValue, FontDefinition endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B6D RID: 7021
		bool StartTransition(VisualElement owner, StylePropertyId prop, Font startValue, Font endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B6E RID: 7022
		bool StartTransition(VisualElement owner, StylePropertyId prop, TextShadow startValue, TextShadow endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B6F RID: 7023
		bool StartTransition(VisualElement owner, StylePropertyId prop, Scale startValue, Scale endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B70 RID: 7024
		bool StartTransition(VisualElement owner, StylePropertyId prop, TransformOrigin startValue, TransformOrigin endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B71 RID: 7025
		bool StartTransition(VisualElement owner, StylePropertyId prop, Translate startValue, Translate endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B72 RID: 7026
		bool StartTransition(VisualElement owner, StylePropertyId prop, Rotate startValue, Rotate endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B73 RID: 7027
		bool StartTransition(VisualElement owner, StylePropertyId prop, BackgroundPosition startValue, BackgroundPosition endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B74 RID: 7028
		bool StartTransition(VisualElement owner, StylePropertyId prop, BackgroundRepeat startValue, BackgroundRepeat endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B75 RID: 7029
		bool StartTransition(VisualElement owner, StylePropertyId prop, BackgroundSize startValue, BackgroundSize endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve);

		// Token: 0x06001B76 RID: 7030
		void CancelAllAnimations();

		// Token: 0x06001B77 RID: 7031
		void CancelAllAnimations(VisualElement owner);

		// Token: 0x06001B78 RID: 7032
		void CancelAnimation(VisualElement owner, StylePropertyId id);

		// Token: 0x06001B79 RID: 7033
		bool HasRunningAnimation(VisualElement owner, StylePropertyId id);

		// Token: 0x06001B7A RID: 7034
		void UpdateAnimation(VisualElement owner, StylePropertyId id);

		// Token: 0x06001B7B RID: 7035
		void GetAllAnimations(VisualElement owner, List<StylePropertyId> propertyIds);

		// Token: 0x06001B7C RID: 7036
		void Update();
	}
}
