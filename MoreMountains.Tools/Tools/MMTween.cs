using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200015B RID: 347
	public class MMTween : MonoBehaviour
	{
		// Token: 0x060007B7 RID: 1975 RVA: 0x000253F3 File Offset: 0x000235F3
		public static float Tween(float currentTime, float initialTime, float endTime, float startValue, float endValue, MMTween.MMTweenCurve curve)
		{
			currentTime = MMMaths.Remap(currentTime, initialTime, endTime, 0f, 1f);
			currentTime = MMTween.TweenDelegateArray[(int)curve](currentTime);
			return startValue + currentTime * (endValue - startValue);
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00025421 File Offset: 0x00023621
		public static long Tween(float currentTime, float initialTime, float endTime, long startValue, long endValue, MMTween.MMTweenCurve curve)
		{
			currentTime = MMMaths.Remap(currentTime, initialTime, endTime, 0f, 1f);
			currentTime = MMTween.TweenDelegateArray[(int)curve](currentTime);
			return startValue + (long)(currentTime * (float)(endValue - startValue));
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00025451 File Offset: 0x00023651
		public static float Evaluate(float t, MMTween.MMTweenCurve curve)
		{
			return MMTween.TweenDelegateArray[(int)curve](t);
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00025460 File Offset: 0x00023660
		public static float Evaluate(float t, MMTweenType tweenType)
		{
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.MMTween)
			{
				return MMTween.Evaluate(t, tweenType.MMTweenCurve);
			}
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.AnimationCurve)
			{
				return tweenType.Curve.Evaluate(t);
			}
			return 0f;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00025492 File Offset: 0x00023692
		public static float LinearTween(float currentTime)
		{
			return MMTweenDefinitions.Linear_Tween(currentTime);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0002549A File Offset: 0x0002369A
		public static float AntiLinearTween(float currentTime)
		{
			return MMTweenDefinitions.LinearAnti_Tween(currentTime);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000254A2 File Offset: 0x000236A2
		public static float EaseInQuadratic(float currentTime)
		{
			return MMTweenDefinitions.EaseIn_Quadratic(currentTime);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x000254AA File Offset: 0x000236AA
		public static float EaseOutQuadratic(float currentTime)
		{
			return MMTweenDefinitions.EaseOut_Quadratic(currentTime);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x000254B2 File Offset: 0x000236B2
		public static float EaseInOutQuadratic(float currentTime)
		{
			return MMTweenDefinitions.EaseInOut_Quadratic(currentTime);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x000254BA File Offset: 0x000236BA
		public static float EaseInCubic(float currentTime)
		{
			return MMTweenDefinitions.EaseIn_Cubic(currentTime);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x000254C2 File Offset: 0x000236C2
		public static float EaseOutCubic(float currentTime)
		{
			return MMTweenDefinitions.EaseOut_Cubic(currentTime);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x000254CA File Offset: 0x000236CA
		public static float EaseInOutCubic(float currentTime)
		{
			return MMTweenDefinitions.EaseInOut_Cubic(currentTime);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x000254D2 File Offset: 0x000236D2
		public static float EaseInQuartic(float currentTime)
		{
			return MMTweenDefinitions.EaseIn_Quartic(currentTime);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000254DA File Offset: 0x000236DA
		public static float EaseOutQuartic(float currentTime)
		{
			return MMTweenDefinitions.EaseOut_Quartic(currentTime);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x000254E2 File Offset: 0x000236E2
		public static float EaseInOutQuartic(float currentTime)
		{
			return MMTweenDefinitions.EaseInOut_Quartic(currentTime);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x000254EA File Offset: 0x000236EA
		public static float EaseInQuintic(float currentTime)
		{
			return MMTweenDefinitions.EaseIn_Quintic(currentTime);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x000254F2 File Offset: 0x000236F2
		public static float EaseOutQuintic(float currentTime)
		{
			return MMTweenDefinitions.EaseOut_Quintic(currentTime);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x000254FA File Offset: 0x000236FA
		public static float EaseInOutQuintic(float currentTime)
		{
			return MMTweenDefinitions.EaseInOut_Quintic(currentTime);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00025502 File Offset: 0x00023702
		public static float EaseInSinusoidal(float currentTime)
		{
			return MMTweenDefinitions.EaseIn_Sinusoidal(currentTime);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0002550A File Offset: 0x0002370A
		public static float EaseOutSinusoidal(float currentTime)
		{
			return MMTweenDefinitions.EaseOut_Sinusoidal(currentTime);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00025512 File Offset: 0x00023712
		public static float EaseInOutSinusoidal(float currentTime)
		{
			return MMTweenDefinitions.EaseInOut_Sinusoidal(currentTime);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002551A File Offset: 0x0002371A
		public static float EaseInBounce(float currentTime)
		{
			return MMTweenDefinitions.EaseIn_Bounce(currentTime);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00025522 File Offset: 0x00023722
		public static float EaseOutBounce(float currentTime)
		{
			return MMTweenDefinitions.EaseOut_Bounce(currentTime);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002552A File Offset: 0x0002372A
		public static float EaseInOutBounce(float currentTime)
		{
			return MMTweenDefinitions.EaseInOut_Bounce(currentTime);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00025532 File Offset: 0x00023732
		public static float EaseInOverhead(float currentTime)
		{
			return MMTweenDefinitions.EaseIn_Overhead(currentTime);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002553A File Offset: 0x0002373A
		public static float EaseOutOverhead(float currentTime)
		{
			return MMTweenDefinitions.EaseOut_Overhead(currentTime);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00025542 File Offset: 0x00023742
		public static float EaseInOutOverhead(float currentTime)
		{
			return MMTweenDefinitions.EaseInOut_Overhead(currentTime);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0002554A File Offset: 0x0002374A
		public static float EaseInExponential(float currentTime)
		{
			return MMTweenDefinitions.EaseIn_Exponential(currentTime);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00025552 File Offset: 0x00023752
		public static float EaseOutExponential(float currentTime)
		{
			return MMTweenDefinitions.EaseOut_Exponential(currentTime);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0002555A File Offset: 0x0002375A
		public static float EaseInOutExponential(float currentTime)
		{
			return MMTweenDefinitions.EaseInOut_Exponential(currentTime);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00025562 File Offset: 0x00023762
		public static float EaseInElastic(float currentTime)
		{
			return MMTweenDefinitions.EaseIn_Elastic(currentTime);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0002556A File Offset: 0x0002376A
		public static float EaseOutElastic(float currentTime)
		{
			return MMTweenDefinitions.EaseOut_Elastic(currentTime);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00025572 File Offset: 0x00023772
		public static float EaseInOutElastic(float currentTime)
		{
			return MMTweenDefinitions.EaseInOut_Elastic(currentTime);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0002557A File Offset: 0x0002377A
		public static float EaseInCircular(float currentTime)
		{
			return MMTweenDefinitions.EaseIn_Circular(currentTime);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00025582 File Offset: 0x00023782
		public static float EaseOutCircular(float currentTime)
		{
			return MMTweenDefinitions.EaseOut_Circular(currentTime);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0002558A File Offset: 0x0002378A
		public static float EaseInOutCircular(float currentTime)
		{
			return MMTweenDefinitions.EaseInOut_Circular(currentTime);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00025592 File Offset: 0x00023792
		public static float AlmostIdentity(float currentTime)
		{
			return MMTweenDefinitions.AlmostIdentity(currentTime);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0002559C File Offset: 0x0002379C
		public static MMTween.TweenDelegate GetTweenMethod(MMTween.MMTweenCurve tween)
		{
			switch (tween)
			{
			case MMTween.MMTweenCurve.LinearTween:
				return new MMTween.TweenDelegate(MMTween.LinearTween);
			case MMTween.MMTweenCurve.EaseInQuadratic:
				return new MMTween.TweenDelegate(MMTween.EaseInQuadratic);
			case MMTween.MMTweenCurve.EaseOutQuadratic:
				return new MMTween.TweenDelegate(MMTween.EaseOutQuadratic);
			case MMTween.MMTweenCurve.EaseInOutQuadratic:
				return new MMTween.TweenDelegate(MMTween.EaseInOutQuadratic);
			case MMTween.MMTweenCurve.EaseInCubic:
				return new MMTween.TweenDelegate(MMTween.EaseInCubic);
			case MMTween.MMTweenCurve.EaseOutCubic:
				return new MMTween.TweenDelegate(MMTween.EaseOutCubic);
			case MMTween.MMTweenCurve.EaseInOutCubic:
				return new MMTween.TweenDelegate(MMTween.EaseInOutCubic);
			case MMTween.MMTweenCurve.EaseInQuartic:
				return new MMTween.TweenDelegate(MMTween.EaseInQuartic);
			case MMTween.MMTweenCurve.EaseOutQuartic:
				return new MMTween.TweenDelegate(MMTween.EaseOutQuartic);
			case MMTween.MMTweenCurve.EaseInOutQuartic:
				return new MMTween.TweenDelegate(MMTween.EaseInOutQuartic);
			case MMTween.MMTweenCurve.EaseInQuintic:
				return new MMTween.TweenDelegate(MMTween.EaseInQuintic);
			case MMTween.MMTweenCurve.EaseOutQuintic:
				return new MMTween.TweenDelegate(MMTween.EaseOutQuintic);
			case MMTween.MMTweenCurve.EaseInOutQuintic:
				return new MMTween.TweenDelegate(MMTween.EaseInOutQuintic);
			case MMTween.MMTweenCurve.EaseInSinusoidal:
				return new MMTween.TweenDelegate(MMTween.EaseInSinusoidal);
			case MMTween.MMTweenCurve.EaseOutSinusoidal:
				return new MMTween.TweenDelegate(MMTween.EaseOutSinusoidal);
			case MMTween.MMTweenCurve.EaseInOutSinusoidal:
				return new MMTween.TweenDelegate(MMTween.EaseInOutSinusoidal);
			case MMTween.MMTweenCurve.EaseInBounce:
				return new MMTween.TweenDelegate(MMTween.EaseInBounce);
			case MMTween.MMTweenCurve.EaseOutBounce:
				return new MMTween.TweenDelegate(MMTween.EaseOutBounce);
			case MMTween.MMTweenCurve.EaseInOutBounce:
				return new MMTween.TweenDelegate(MMTween.EaseInOutBounce);
			case MMTween.MMTweenCurve.EaseInOverhead:
				return new MMTween.TweenDelegate(MMTween.EaseInOverhead);
			case MMTween.MMTweenCurve.EaseOutOverhead:
				return new MMTween.TweenDelegate(MMTween.EaseOutOverhead);
			case MMTween.MMTweenCurve.EaseInOutOverhead:
				return new MMTween.TweenDelegate(MMTween.EaseInOutOverhead);
			case MMTween.MMTweenCurve.EaseInExponential:
				return new MMTween.TweenDelegate(MMTween.EaseInExponential);
			case MMTween.MMTweenCurve.EaseOutExponential:
				return new MMTween.TweenDelegate(MMTween.EaseOutExponential);
			case MMTween.MMTweenCurve.EaseInOutExponential:
				return new MMTween.TweenDelegate(MMTween.EaseInOutExponential);
			case MMTween.MMTweenCurve.EaseInElastic:
				return new MMTween.TweenDelegate(MMTween.EaseInElastic);
			case MMTween.MMTweenCurve.EaseOutElastic:
				return new MMTween.TweenDelegate(MMTween.EaseOutElastic);
			case MMTween.MMTweenCurve.EaseInOutElastic:
				return new MMTween.TweenDelegate(MMTween.EaseInOutElastic);
			case MMTween.MMTweenCurve.EaseInCircular:
				return new MMTween.TweenDelegate(MMTween.EaseInCircular);
			case MMTween.MMTweenCurve.EaseOutCircular:
				return new MMTween.TweenDelegate(MMTween.EaseOutCircular);
			case MMTween.MMTweenCurve.EaseInOutCircular:
				return new MMTween.TweenDelegate(MMTween.EaseInOutCircular);
			case MMTween.MMTweenCurve.AntiLinearTween:
				return new MMTween.TweenDelegate(MMTween.AntiLinearTween);
			case MMTween.MMTweenCurve.AlmostIdentity:
				return new MMTween.TweenDelegate(MMTween.AlmostIdentity);
			default:
				return new MMTween.TweenDelegate(MMTween.LinearTween);
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x000257F1 File Offset: 0x000239F1
		public static Vector2 Tween(float currentTime, float initialTime, float endTime, Vector2 startValue, Vector2 endValue, MMTween.MMTweenCurve curve)
		{
			startValue.x = MMTween.Tween(currentTime, initialTime, endTime, startValue.x, endValue.x, curve);
			startValue.y = MMTween.Tween(currentTime, initialTime, endTime, startValue.y, endValue.y, curve);
			return startValue;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00025830 File Offset: 0x00023A30
		public static Vector3 Tween(float currentTime, float initialTime, float endTime, Vector3 startValue, Vector3 endValue, MMTween.MMTweenCurve curve)
		{
			startValue.x = MMTween.Tween(currentTime, initialTime, endTime, startValue.x, endValue.x, curve);
			startValue.y = MMTween.Tween(currentTime, initialTime, endTime, startValue.y, endValue.y, curve);
			startValue.z = MMTween.Tween(currentTime, initialTime, endTime, startValue.z, endValue.z, curve);
			return startValue;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00025898 File Offset: 0x00023A98
		public static Vector4 Tween(float currentTime, float initialTime, float endTime, Vector4 startValue, Vector4 endValue, MMTween.MMTweenCurve curve)
		{
			startValue.x = MMTween.Tween(currentTime, initialTime, endTime, startValue.x, endValue.x, curve);
			startValue.y = MMTween.Tween(currentTime, initialTime, endTime, startValue.y, endValue.y, curve);
			startValue.z = MMTween.Tween(currentTime, initialTime, endTime, startValue.z, endValue.z, curve);
			startValue.w = MMTween.Tween(currentTime, initialTime, endTime, startValue.w, endValue.w, curve);
			return startValue;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00025920 File Offset: 0x00023B20
		public static Quaternion Tween(float currentTime, float initialTime, float endTime, Quaternion startValue, Quaternion endValue, MMTween.MMTweenCurve curve)
		{
			float num = MMTween.Tween(currentTime, initialTime, endTime, 0f, 1f, curve);
			startValue = Quaternion.Slerp(startValue, endValue, num);
			return startValue;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0002594E File Offset: 0x00023B4E
		public static float Tween(float currentTime, float initialTime, float endTime, float startValue, float endValue, AnimationCurve curve)
		{
			currentTime = MMMaths.Remap(currentTime, initialTime, endTime, 0f, 1f);
			currentTime = curve.Evaluate(currentTime);
			return startValue + currentTime * (endValue - startValue);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00025976 File Offset: 0x00023B76
		public static long Tween(float currentTime, float initialTime, float endTime, long startValue, long endValue, AnimationCurve curve)
		{
			currentTime = MMMaths.Remap(currentTime, initialTime, endTime, 0f, 1f);
			currentTime = curve.Evaluate(currentTime);
			return startValue + (long)currentTime * (endValue - startValue);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002599F File Offset: 0x00023B9F
		public static Vector2 Tween(float currentTime, float initialTime, float endTime, Vector2 startValue, Vector2 endValue, AnimationCurve curve)
		{
			startValue.x = MMTween.Tween(currentTime, initialTime, endTime, startValue.x, endValue.x, curve);
			startValue.y = MMTween.Tween(currentTime, initialTime, endTime, startValue.y, endValue.y, curve);
			return startValue;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000259E0 File Offset: 0x00023BE0
		public static Vector3 Tween(float currentTime, float initialTime, float endTime, Vector3 startValue, Vector3 endValue, AnimationCurve curve)
		{
			startValue.x = MMTween.Tween(currentTime, initialTime, endTime, startValue.x, endValue.x, curve);
			startValue.y = MMTween.Tween(currentTime, initialTime, endTime, startValue.y, endValue.y, curve);
			startValue.z = MMTween.Tween(currentTime, initialTime, endTime, startValue.z, endValue.z, curve);
			return startValue;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00025A48 File Offset: 0x00023C48
		public static Vector4 Tween(float currentTime, float initialTime, float endTime, Vector4 startValue, Vector4 endValue, AnimationCurve curve)
		{
			startValue.x = MMTween.Tween(currentTime, initialTime, endTime, startValue.x, endValue.x, curve);
			startValue.y = MMTween.Tween(currentTime, initialTime, endTime, startValue.y, endValue.y, curve);
			startValue.z = MMTween.Tween(currentTime, initialTime, endTime, startValue.z, endValue.z, curve);
			startValue.w = MMTween.Tween(currentTime, initialTime, endTime, startValue.w, endValue.w, curve);
			return startValue;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00025AD0 File Offset: 0x00023CD0
		public static Quaternion Tween(float currentTime, float initialTime, float endTime, Quaternion startValue, Quaternion endValue, AnimationCurve curve)
		{
			float num = MMTween.Tween(currentTime, initialTime, endTime, 0f, 1f, curve);
			startValue = Quaternion.Slerp(startValue, endValue, num);
			return startValue;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00025AFE File Offset: 0x00023CFE
		public static float Tween(float currentTime, float initialTime, float endTime, float startValue, float endValue, MMTweenType tweenType)
		{
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.MMTween)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.MMTweenCurve);
			}
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.AnimationCurve)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.Curve);
			}
			return 0f;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00025B3E File Offset: 0x00023D3E
		public static long Tween(float currentTime, float initialTime, float endTime, long startValue, long endValue, MMTweenType tweenType)
		{
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.MMTween)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.MMTweenCurve);
			}
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.AnimationCurve)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.Curve);
			}
			return 0L;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00025B7B File Offset: 0x00023D7B
		public static Vector2 Tween(float currentTime, float initialTime, float endTime, Vector2 startValue, Vector2 endValue, MMTweenType tweenType)
		{
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.MMTween)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.MMTweenCurve);
			}
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.AnimationCurve)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.Curve);
			}
			return Vector2.zero;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00025BBB File Offset: 0x00023DBB
		public static Vector3 Tween(float currentTime, float initialTime, float endTime, Vector3 startValue, Vector3 endValue, MMTweenType tweenType)
		{
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.MMTween)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.MMTweenCurve);
			}
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.AnimationCurve)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.Curve);
			}
			return Vector3.zero;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00025BFC File Offset: 0x00023DFC
		public static Vector4 Tween(float currentTime, float initialTime, float endTime, Vector4 startValue, Vector4 endValue, MMTweenType tweenType)
		{
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.MMTween)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.MMTweenCurve);
			}
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.AnimationCurve)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.Curve);
			}
			return Vector3.zero;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00025C4C File Offset: 0x00023E4C
		public static Quaternion Tween(float currentTime, float initialTime, float endTime, Quaternion startValue, Quaternion endValue, MMTweenType tweenType)
		{
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.MMTween)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.MMTweenCurve);
			}
			if (tweenType.MMTweenDefinitionType == MMTweenDefinitionTypes.AnimationCurve)
			{
				return MMTween.Tween(currentTime, initialTime, endTime, startValue, endValue, tweenType.Curve);
			}
			return Quaternion.identity;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00025C8C File Offset: 0x00023E8C
		public static Coroutine MoveTransform(MonoBehaviour mono, Transform targetTransform, Vector3 origin, Vector3 destination, WaitForSeconds delay, float delayDuration, float duration, MMTween.MMTweenCurve curve, bool ignoreTimescale = false)
		{
			return mono.StartCoroutine(MMTween.MoveTransformCo(targetTransform, origin, destination, delay, delayDuration, duration, curve, ignoreTimescale));
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00025CB4 File Offset: 0x00023EB4
		public static Coroutine MoveRectTransform(MonoBehaviour mono, RectTransform targetTransform, Vector3 origin, Vector3 destination, WaitForSeconds delay, float delayDuration, float duration, MMTween.MMTweenCurve curve, bool ignoreTimescale = false)
		{
			return mono.StartCoroutine(MMTween.MoveRectTransformCo(targetTransform, origin, destination, delay, delayDuration, duration, curve, ignoreTimescale));
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00025CDC File Offset: 0x00023EDC
		public static Coroutine MoveTransform(MonoBehaviour mono, Transform targetTransform, Transform origin, Transform destination, WaitForSeconds delay, float delayDuration, float duration, MMTween.MMTweenCurve curve, bool updatePosition = true, bool updateRotation = true, bool ignoreTimescale = false)
		{
			return mono.StartCoroutine(MMTween.MoveTransformCo(targetTransform, origin, destination, delay, delayDuration, duration, curve, updatePosition, updateRotation, ignoreTimescale));
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00025D08 File Offset: 0x00023F08
		public static Coroutine RotateTransformAround(MonoBehaviour mono, Transform targetTransform, Transform center, Transform destination, float angle, WaitForSeconds delay, float delayDuration, float duration, MMTween.MMTweenCurve curve, bool ignoreTimescale = false)
		{
			return mono.StartCoroutine(MMTween.RotateTransformAroundCo(targetTransform, center, destination, angle, delay, delayDuration, duration, curve, ignoreTimescale));
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00025D30 File Offset: 0x00023F30
		protected static IEnumerator MoveRectTransformCo(RectTransform targetTransform, Vector3 origin, Vector3 destination, WaitForSeconds delay, float delayDuration, float duration, MMTween.MMTweenCurve curve, bool ignoreTimescale = false)
		{
			if (delayDuration > 0f)
			{
				yield return delay;
			}
			float timeLeft = duration;
			while (timeLeft > 0f)
			{
				targetTransform.localPosition = MMTween.Tween(duration - timeLeft, 0f, duration, origin, destination, curve);
				timeLeft -= (ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime);
				yield return null;
			}
			targetTransform.localPosition = destination;
			yield break;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00025D80 File Offset: 0x00023F80
		protected static IEnumerator MoveTransformCo(Transform targetTransform, Vector3 origin, Vector3 destination, WaitForSeconds delay, float delayDuration, float duration, MMTween.MMTweenCurve curve, bool ignoreTimescale = false)
		{
			if (delayDuration > 0f)
			{
				yield return delay;
			}
			float timeLeft = duration;
			while (timeLeft > 0f)
			{
				targetTransform.transform.position = MMTween.Tween(duration - timeLeft, 0f, duration, origin, destination, curve);
				timeLeft -= (ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime);
				yield return null;
			}
			targetTransform.transform.position = destination;
			yield break;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00025DD0 File Offset: 0x00023FD0
		protected static IEnumerator MoveTransformCo(Transform targetTransform, Transform origin, Transform destination, WaitForSeconds delay, float delayDuration, float duration, MMTween.MMTweenCurve curve, bool updatePosition = true, bool updateRotation = true, bool ignoreTimescale = false)
		{
			if (delayDuration > 0f)
			{
				yield return delay;
			}
			float timeLeft = duration;
			while (timeLeft > 0f)
			{
				if (updatePosition)
				{
					targetTransform.transform.position = MMTween.Tween(duration - timeLeft, 0f, duration, origin.position, destination.position, curve);
				}
				if (updateRotation)
				{
					targetTransform.transform.rotation = MMTween.Tween(duration - timeLeft, 0f, duration, origin.rotation, destination.rotation, curve);
				}
				timeLeft -= (ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime);
				yield return null;
			}
			if (updatePosition)
			{
				targetTransform.transform.position = destination.position;
			}
			if (updateRotation)
			{
				targetTransform.transform.localEulerAngles = destination.localEulerAngles;
			}
			yield break;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00025E30 File Offset: 0x00024030
		protected static IEnumerator RotateTransformAroundCo(Transform targetTransform, Transform center, Transform destination, float angle, WaitForSeconds delay, float delayDuration, float duration, MMTween.MMTweenCurve curve, bool ignoreTimescale = false)
		{
			if (delayDuration > 0f)
			{
				yield return delay;
			}
			Vector3 initialRotationPosition = targetTransform.transform.position;
			Quaternion quaternion = targetTransform.transform.rotation;
			float num = 1f / duration;
			float timeSpent = 0f;
			while (timeSpent < duration)
			{
				float num2 = MMTween.Tween(timeSpent, 0f, duration, 0f, angle, curve);
				targetTransform.transform.position = initialRotationPosition;
				quaternion = targetTransform.transform.rotation;
				targetTransform.RotateAround(center.transform.position, center.transform.up, num2);
				targetTransform.transform.rotation = quaternion;
				timeSpent += (ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime);
				yield return null;
			}
			targetTransform.transform.position = destination.position;
			yield break;
		}

		// Token: 0x04000978 RID: 2424
		public static MMTween.TweenDelegate[] TweenDelegateArray = new MMTween.TweenDelegate[]
		{
			new MMTween.TweenDelegate(MMTween.LinearTween),
			new MMTween.TweenDelegate(MMTween.EaseInQuadratic),
			new MMTween.TweenDelegate(MMTween.EaseOutQuadratic),
			new MMTween.TweenDelegate(MMTween.EaseInOutQuadratic),
			new MMTween.TweenDelegate(MMTween.EaseInCubic),
			new MMTween.TweenDelegate(MMTween.EaseOutCubic),
			new MMTween.TweenDelegate(MMTween.EaseInOutCubic),
			new MMTween.TweenDelegate(MMTween.EaseInQuartic),
			new MMTween.TweenDelegate(MMTween.EaseOutQuartic),
			new MMTween.TweenDelegate(MMTween.EaseInOutQuartic),
			new MMTween.TweenDelegate(MMTween.EaseInQuintic),
			new MMTween.TweenDelegate(MMTween.EaseOutQuintic),
			new MMTween.TweenDelegate(MMTween.EaseInOutQuintic),
			new MMTween.TweenDelegate(MMTween.EaseInSinusoidal),
			new MMTween.TweenDelegate(MMTween.EaseOutSinusoidal),
			new MMTween.TweenDelegate(MMTween.EaseInOutSinusoidal),
			new MMTween.TweenDelegate(MMTween.EaseInBounce),
			new MMTween.TweenDelegate(MMTween.EaseOutBounce),
			new MMTween.TweenDelegate(MMTween.EaseInOutBounce),
			new MMTween.TweenDelegate(MMTween.EaseInOverhead),
			new MMTween.TweenDelegate(MMTween.EaseOutOverhead),
			new MMTween.TweenDelegate(MMTween.EaseInOutOverhead),
			new MMTween.TweenDelegate(MMTween.EaseInExponential),
			new MMTween.TweenDelegate(MMTween.EaseOutExponential),
			new MMTween.TweenDelegate(MMTween.EaseInOutExponential),
			new MMTween.TweenDelegate(MMTween.EaseInElastic),
			new MMTween.TweenDelegate(MMTween.EaseOutElastic),
			new MMTween.TweenDelegate(MMTween.EaseInOutElastic),
			new MMTween.TweenDelegate(MMTween.EaseInCircular),
			new MMTween.TweenDelegate(MMTween.EaseOutCircular),
			new MMTween.TweenDelegate(MMTween.EaseInOutCircular),
			new MMTween.TweenDelegate(MMTween.AntiLinearTween),
			new MMTween.TweenDelegate(MMTween.AlmostIdentity)
		};

		// Token: 0x0200045E RID: 1118
		public enum MMTweenCurve
		{
			// Token: 0x0400203A RID: 8250
			LinearTween,
			// Token: 0x0400203B RID: 8251
			EaseInQuadratic,
			// Token: 0x0400203C RID: 8252
			EaseOutQuadratic,
			// Token: 0x0400203D RID: 8253
			EaseInOutQuadratic,
			// Token: 0x0400203E RID: 8254
			EaseInCubic,
			// Token: 0x0400203F RID: 8255
			EaseOutCubic,
			// Token: 0x04002040 RID: 8256
			EaseInOutCubic,
			// Token: 0x04002041 RID: 8257
			EaseInQuartic,
			// Token: 0x04002042 RID: 8258
			EaseOutQuartic,
			// Token: 0x04002043 RID: 8259
			EaseInOutQuartic,
			// Token: 0x04002044 RID: 8260
			EaseInQuintic,
			// Token: 0x04002045 RID: 8261
			EaseOutQuintic,
			// Token: 0x04002046 RID: 8262
			EaseInOutQuintic,
			// Token: 0x04002047 RID: 8263
			EaseInSinusoidal,
			// Token: 0x04002048 RID: 8264
			EaseOutSinusoidal,
			// Token: 0x04002049 RID: 8265
			EaseInOutSinusoidal,
			// Token: 0x0400204A RID: 8266
			EaseInBounce,
			// Token: 0x0400204B RID: 8267
			EaseOutBounce,
			// Token: 0x0400204C RID: 8268
			EaseInOutBounce,
			// Token: 0x0400204D RID: 8269
			EaseInOverhead,
			// Token: 0x0400204E RID: 8270
			EaseOutOverhead,
			// Token: 0x0400204F RID: 8271
			EaseInOutOverhead,
			// Token: 0x04002050 RID: 8272
			EaseInExponential,
			// Token: 0x04002051 RID: 8273
			EaseOutExponential,
			// Token: 0x04002052 RID: 8274
			EaseInOutExponential,
			// Token: 0x04002053 RID: 8275
			EaseInElastic,
			// Token: 0x04002054 RID: 8276
			EaseOutElastic,
			// Token: 0x04002055 RID: 8277
			EaseInOutElastic,
			// Token: 0x04002056 RID: 8278
			EaseInCircular,
			// Token: 0x04002057 RID: 8279
			EaseOutCircular,
			// Token: 0x04002058 RID: 8280
			EaseInOutCircular,
			// Token: 0x04002059 RID: 8281
			AntiLinearTween,
			// Token: 0x0400205A RID: 8282
			AlmostIdentity
		}

		// Token: 0x0200045F RID: 1119
		// (Invoke) Token: 0x06001A87 RID: 6791
		public delegate float TweenDelegate(float currentTime);
	}
}
