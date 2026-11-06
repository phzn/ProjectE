using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x02000347 RID: 839
	internal class EmptyStylePropertyAnimationSystem : IStylePropertyAnimationSystem
	{
		// Token: 0x06001C3A RID: 7226 RVA: 0x0006E3B8 File Offset: 0x0006C5B8
		public bool StartTransition(VisualElement owner, StylePropertyId prop, float startValue, float endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x0006E3CC File Offset: 0x0006C5CC
		public bool StartTransition(VisualElement owner, StylePropertyId prop, int startValue, int endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x0006E3E0 File Offset: 0x0006C5E0
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Length startValue, Length endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x0006E3F4 File Offset: 0x0006C5F4
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Color startValue, Color endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x0006E408 File Offset: 0x0006C608
		public bool StartAnimationEnum(VisualElement owner, StylePropertyId prop, int startValue, int endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x0006E41C File Offset: 0x0006C61C
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Background startValue, Background endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x0006E430 File Offset: 0x0006C630
		public bool StartTransition(VisualElement owner, StylePropertyId prop, FontDefinition startValue, FontDefinition endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x0006E444 File Offset: 0x0006C644
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Font startValue, Font endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x0006E458 File Offset: 0x0006C658
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Cursor startValue, Cursor endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x0006E46C File Offset: 0x0006C66C
		public bool StartTransition(VisualElement owner, StylePropertyId prop, TextShadow startValue, TextShadow endValue, int durationMs, int delayMs, Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x0006E480 File Offset: 0x0006C680
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Scale startValue, Scale endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x0006E494 File Offset: 0x0006C694
		public bool StartTransition(VisualElement owner, StylePropertyId prop, TransformOrigin startValue, TransformOrigin endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x0006E4A8 File Offset: 0x0006C6A8
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Translate startValue, Translate endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x0006E4BC File Offset: 0x0006C6BC
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Rotate startValue, Rotate endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x0006E4D0 File Offset: 0x0006C6D0
		public bool StartTransition(VisualElement owner, StylePropertyId prop, BackgroundPosition startValue, BackgroundPosition endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x0006E4E4 File Offset: 0x0006C6E4
		public bool StartTransition(VisualElement owner, StylePropertyId prop, BackgroundRepeat startValue, BackgroundRepeat endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x0006E4F8 File Offset: 0x0006C6F8
		public bool StartTransition(VisualElement owner, StylePropertyId prop, BackgroundSize startValue, BackgroundSize endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return false;
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void CancelAllAnimations()
		{
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void CancelAllAnimations(VisualElement owner)
		{
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void CancelAnimation(VisualElement owner, StylePropertyId id)
		{
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x0006E50C File Offset: 0x0006C70C
		public bool HasRunningAnimation(VisualElement owner, StylePropertyId id)
		{
			return false;
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void UpdateAnimation(VisualElement owner, StylePropertyId id)
		{
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void GetAllAnimations(VisualElement owner, List<StylePropertyId> propertyIds)
		{
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void Update()
		{
		}
	}
}
