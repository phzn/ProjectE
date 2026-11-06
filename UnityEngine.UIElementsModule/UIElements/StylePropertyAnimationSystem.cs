using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x0200032A RID: 810
	internal class StylePropertyAnimationSystem : IStylePropertyAnimationSystem
	{
		// Token: 0x06001B7D RID: 7037 RVA: 0x0006B146 File Offset: 0x00069346
		public StylePropertyAnimationSystem()
		{
			this.m_CurrentTimeMs = Panel.TimeSinceStartupMs();
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x0006B17C File Offset: 0x0006937C
		private T GetOrCreate<T>(ref T values) where T : new()
		{
			T t = values;
			return (t != null) ? t : (values = new T());
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x0006B1B0 File Offset: 0x000693B0
		private bool StartTransition<T>(VisualElement owner, StylePropertyId prop, T startValue, T endValue, int durationMs, int delayMs, Func<float, float> easingCurve, StylePropertyAnimationSystem.Values<T> values)
		{
			this.m_PropertyToValues[prop] = values;
			bool flag = values.StartTransition(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.CurrentTimeMs());
			this.UpdateTracking<T>(values);
			return flag;
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x0006B1F4 File Offset: 0x000693F4
		public bool StartTransition(VisualElement owner, StylePropertyId prop, float startValue, float endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<float>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesFloat>(ref this.m_Floats));
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x0006B224 File Offset: 0x00069424
		public bool StartTransition(VisualElement owner, StylePropertyId prop, int startValue, int endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<int>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesInt>(ref this.m_Ints));
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x0006B254 File Offset: 0x00069454
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Length startValue, Length endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Length>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesLength>(ref this.m_Lengths));
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x0006B284 File Offset: 0x00069484
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Color startValue, Color endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Color>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesColor>(ref this.m_Colors));
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x0006B2B4 File Offset: 0x000694B4
		public bool StartAnimationEnum(VisualElement owner, StylePropertyId prop, int startValue, int endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<int>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesEnum>(ref this.m_Enums));
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x0006B2E4 File Offset: 0x000694E4
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Background startValue, Background endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Background>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesBackground>(ref this.m_Backgrounds));
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x0006B314 File Offset: 0x00069514
		public bool StartTransition(VisualElement owner, StylePropertyId prop, FontDefinition startValue, FontDefinition endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<FontDefinition>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesFontDefinition>(ref this.m_FontDefinitions));
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x0006B344 File Offset: 0x00069544
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Font startValue, Font endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Font>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesFont>(ref this.m_Fonts));
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x0006B374 File Offset: 0x00069574
		public bool StartTransition(VisualElement owner, StylePropertyId prop, TextShadow startValue, TextShadow endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<TextShadow>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesTextShadow>(ref this.m_TextShadows));
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x0006B3A4 File Offset: 0x000695A4
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Scale startValue, Scale endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Scale>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesScale>(ref this.m_Scale));
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x0006B3D4 File Offset: 0x000695D4
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Rotate startValue, Rotate endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Rotate>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesRotate>(ref this.m_Rotate));
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x0006B404 File Offset: 0x00069604
		public bool StartTransition(VisualElement owner, StylePropertyId prop, Translate startValue, Translate endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<Translate>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesTranslate>(ref this.m_Translate));
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x0006B434 File Offset: 0x00069634
		public bool StartTransition(VisualElement owner, StylePropertyId prop, TransformOrigin startValue, TransformOrigin endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<TransformOrigin>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesTransformOrigin>(ref this.m_TransformOrigin));
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x0006B464 File Offset: 0x00069664
		public bool StartTransition(VisualElement owner, StylePropertyId prop, BackgroundPosition startValue, BackgroundPosition endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<BackgroundPosition>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesBackgroundPosition>(ref this.m_BackgroundPosition));
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x0006B494 File Offset: 0x00069694
		public bool StartTransition(VisualElement owner, StylePropertyId prop, BackgroundRepeat startValue, BackgroundRepeat endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<BackgroundRepeat>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesBackgroundRepeat>(ref this.m_BackgroundRepeat));
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x0006B4C4 File Offset: 0x000696C4
		public bool StartTransition(VisualElement owner, StylePropertyId prop, BackgroundSize startValue, BackgroundSize endValue, int durationMs, int delayMs, [NotNull] Func<float, float> easingCurve)
		{
			return this.StartTransition<BackgroundSize>(owner, prop, startValue, endValue, durationMs, delayMs, easingCurve, this.GetOrCreate<StylePropertyAnimationSystem.ValuesBackgroundSize>(ref this.m_BackgroundSize));
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x0006B4F4 File Offset: 0x000696F4
		public void CancelAllAnimations()
		{
			foreach (StylePropertyAnimationSystem.Values values in this.m_AllValues)
			{
				values.CancelAllAnimations();
			}
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x0006B54C File Offset: 0x0006974C
		public void CancelAllAnimations(VisualElement owner)
		{
			foreach (StylePropertyAnimationSystem.Values values in this.m_AllValues)
			{
				values.CancelAllAnimations(owner);
			}
			Assert.AreEqual(0, owner.styleAnimation.runningAnimationCount);
			Assert.AreEqual(0, owner.styleAnimation.completedAnimationCount);
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x0006B5CC File Offset: 0x000697CC
		public void CancelAnimation(VisualElement owner, StylePropertyId id)
		{
			StylePropertyAnimationSystem.Values values;
			bool flag = this.m_PropertyToValues.TryGetValue(id, out values);
			if (flag)
			{
				values.CancelAnimation(owner, id);
			}
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x0006B5F8 File Offset: 0x000697F8
		public bool HasRunningAnimation(VisualElement owner, StylePropertyId id)
		{
			StylePropertyAnimationSystem.Values values;
			return this.m_PropertyToValues.TryGetValue(id, out values) && values.HasRunningAnimation(owner, id);
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x0006B628 File Offset: 0x00069828
		public void UpdateAnimation(VisualElement owner, StylePropertyId id)
		{
			StylePropertyAnimationSystem.Values values;
			bool flag = this.m_PropertyToValues.TryGetValue(id, out values);
			if (flag)
			{
				values.UpdateAnimation(owner, id);
			}
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x0006B654 File Offset: 0x00069854
		public void GetAllAnimations(VisualElement owner, List<StylePropertyId> propertyIds)
		{
			foreach (StylePropertyAnimationSystem.Values values in this.m_AllValues)
			{
				values.GetAllAnimations(owner, propertyIds);
			}
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x0006B6AC File Offset: 0x000698AC
		private void UpdateTracking<T>(StylePropertyAnimationSystem.Values<T> values)
		{
			bool flag = !values.isEmpty && !this.m_AllValues.Contains(values);
			if (flag)
			{
				this.m_AllValues.Add(values);
			}
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x0006B6E8 File Offset: 0x000698E8
		private long CurrentTimeMs()
		{
			return this.m_CurrentTimeMs;
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x0006B700 File Offset: 0x00069900
		public void Update()
		{
			this.m_CurrentTimeMs = Panel.TimeSinceStartupMs();
			int count = this.m_AllValues.Count;
			for (int i = 0; i < count; i++)
			{
				this.m_AllValues[i].Update(this.m_CurrentTimeMs);
			}
		}

		// Token: 0x04000B65 RID: 2917
		private long m_CurrentTimeMs = 0L;

		// Token: 0x04000B66 RID: 2918
		private StylePropertyAnimationSystem.ValuesFloat m_Floats;

		// Token: 0x04000B67 RID: 2919
		private StylePropertyAnimationSystem.ValuesInt m_Ints;

		// Token: 0x04000B68 RID: 2920
		private StylePropertyAnimationSystem.ValuesLength m_Lengths;

		// Token: 0x04000B69 RID: 2921
		private StylePropertyAnimationSystem.ValuesColor m_Colors;

		// Token: 0x04000B6A RID: 2922
		private StylePropertyAnimationSystem.ValuesEnum m_Enums;

		// Token: 0x04000B6B RID: 2923
		private StylePropertyAnimationSystem.ValuesBackground m_Backgrounds;

		// Token: 0x04000B6C RID: 2924
		private StylePropertyAnimationSystem.ValuesFontDefinition m_FontDefinitions;

		// Token: 0x04000B6D RID: 2925
		private StylePropertyAnimationSystem.ValuesFont m_Fonts;

		// Token: 0x04000B6E RID: 2926
		private StylePropertyAnimationSystem.ValuesTextShadow m_TextShadows;

		// Token: 0x04000B6F RID: 2927
		private StylePropertyAnimationSystem.ValuesScale m_Scale;

		// Token: 0x04000B70 RID: 2928
		private StylePropertyAnimationSystem.ValuesRotate m_Rotate;

		// Token: 0x04000B71 RID: 2929
		private StylePropertyAnimationSystem.ValuesTranslate m_Translate;

		// Token: 0x04000B72 RID: 2930
		private StylePropertyAnimationSystem.ValuesTransformOrigin m_TransformOrigin;

		// Token: 0x04000B73 RID: 2931
		private StylePropertyAnimationSystem.ValuesBackgroundPosition m_BackgroundPosition;

		// Token: 0x04000B74 RID: 2932
		private StylePropertyAnimationSystem.ValuesBackgroundRepeat m_BackgroundRepeat;

		// Token: 0x04000B75 RID: 2933
		private StylePropertyAnimationSystem.ValuesBackgroundSize m_BackgroundSize;

		// Token: 0x04000B76 RID: 2934
		private readonly List<StylePropertyAnimationSystem.Values> m_AllValues = new List<StylePropertyAnimationSystem.Values>();

		// Token: 0x04000B77 RID: 2935
		private readonly Dictionary<StylePropertyId, StylePropertyAnimationSystem.Values> m_PropertyToValues = new Dictionary<StylePropertyId, StylePropertyAnimationSystem.Values>();

		// Token: 0x0200032B RID: 811
		[Flags]
		private enum TransitionState
		{
			// Token: 0x04000B79 RID: 2937
			None = 0,
			// Token: 0x04000B7A RID: 2938
			Running = 1,
			// Token: 0x04000B7B RID: 2939
			Started = 2,
			// Token: 0x04000B7C RID: 2940
			Ended = 4,
			// Token: 0x04000B7D RID: 2941
			Canceled = 8
		}

		// Token: 0x0200032C RID: 812
		private struct AnimationDataSet<TTimingData, TStyleData>
		{
			// Token: 0x17000691 RID: 1681
			// (get) Token: 0x06001B99 RID: 7065 RVA: 0x0006B74F File Offset: 0x0006994F
			// (set) Token: 0x06001B9A RID: 7066 RVA: 0x0006B759 File Offset: 0x00069959
			private int capacity
			{
				get
				{
					return this.elements.Length;
				}
				set
				{
					Array.Resize<VisualElement>(ref this.elements, value);
					Array.Resize<StylePropertyId>(ref this.properties, value);
					Array.Resize<TTimingData>(ref this.timing, value);
					Array.Resize<TStyleData>(ref this.style, value);
				}
			}

			// Token: 0x06001B9B RID: 7067 RVA: 0x0006B790 File Offset: 0x00069990
			private void LocalInit()
			{
				this.elements = new VisualElement[2];
				this.properties = new StylePropertyId[2];
				this.timing = new TTimingData[2];
				this.style = new TStyleData[2];
				this.indices = new Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, int>(StylePropertyAnimationSystem.ElementPropertyPair.Comparer);
			}

			// Token: 0x06001B9C RID: 7068 RVA: 0x0006B7E0 File Offset: 0x000699E0
			public static StylePropertyAnimationSystem.AnimationDataSet<TTimingData, TStyleData> Create()
			{
				StylePropertyAnimationSystem.AnimationDataSet<TTimingData, TStyleData> animationDataSet = default(StylePropertyAnimationSystem.AnimationDataSet<TTimingData, TStyleData>);
				animationDataSet.LocalInit();
				return animationDataSet;
			}

			// Token: 0x06001B9D RID: 7069 RVA: 0x0006B804 File Offset: 0x00069A04
			public bool IndexOf(VisualElement ve, StylePropertyId prop, out int index)
			{
				return this.indices.TryGetValue(new StylePropertyAnimationSystem.ElementPropertyPair(ve, prop), out index);
			}

			// Token: 0x06001B9E RID: 7070 RVA: 0x0006B82C File Offset: 0x00069A2C
			public void Add(VisualElement owner, StylePropertyId prop, TTimingData timingData, TStyleData styleData)
			{
				bool flag = this.count >= this.capacity;
				if (flag)
				{
					this.capacity *= 2;
				}
				int num = this.count;
				this.count = num + 1;
				int num2 = num;
				this.elements[num2] = owner;
				this.properties[num2] = prop;
				this.timing[num2] = timingData;
				this.style[num2] = styleData;
				this.indices.Add(new StylePropertyAnimationSystem.ElementPropertyPair(owner, prop), num2);
			}

			// Token: 0x06001B9F RID: 7071 RVA: 0x0006B8B4 File Offset: 0x00069AB4
			public void Remove(int cancelledIndex)
			{
				int num = this.count - 1;
				this.count = num;
				int num2 = num;
				this.indices.Remove(new StylePropertyAnimationSystem.ElementPropertyPair(this.elements[cancelledIndex], this.properties[cancelledIndex]));
				bool flag = cancelledIndex != num2;
				if (flag)
				{
					VisualElement visualElement = (this.elements[cancelledIndex] = this.elements[num2]);
					StylePropertyId stylePropertyId = (this.properties[cancelledIndex] = this.properties[num2]);
					this.timing[cancelledIndex] = this.timing[num2];
					this.style[cancelledIndex] = this.style[num2];
					this.indices[new StylePropertyAnimationSystem.ElementPropertyPair(visualElement, stylePropertyId)] = cancelledIndex;
				}
				this.elements[num2] = null;
				this.properties[num2] = StylePropertyId.Unknown;
				this.timing[num2] = default(TTimingData);
				this.style[num2] = default(TStyleData);
			}

			// Token: 0x06001BA0 RID: 7072 RVA: 0x0006B9B2 File Offset: 0x00069BB2
			public void Replace(int index, TTimingData timingData, TStyleData styleData)
			{
				this.timing[index] = timingData;
				this.style[index] = styleData;
			}

			// Token: 0x06001BA1 RID: 7073 RVA: 0x0006B9D0 File Offset: 0x00069BD0
			public void RemoveAll(VisualElement ve)
			{
				int num = this.count;
				for (int i = num - 1; i >= 0; i--)
				{
					bool flag = this.elements[i] == ve;
					if (flag)
					{
						this.Remove(i);
					}
				}
			}

			// Token: 0x06001BA2 RID: 7074 RVA: 0x0006BA14 File Offset: 0x00069C14
			public void RemoveAll()
			{
				this.capacity = 2;
				int num = Mathf.Min(this.count, this.capacity);
				Array.Clear(this.elements, 0, num);
				Array.Clear(this.properties, 0, num);
				Array.Clear(this.timing, 0, num);
				Array.Clear(this.style, 0, num);
				this.count = 0;
				this.indices.Clear();
			}

			// Token: 0x06001BA3 RID: 7075 RVA: 0x0006BA88 File Offset: 0x00069C88
			public void GetActivePropertiesForElement(VisualElement ve, List<StylePropertyId> outProperties)
			{
				int num = this.count;
				for (int i = num - 1; i >= 0; i--)
				{
					bool flag = this.elements[i] == ve;
					if (flag)
					{
						outProperties.Add(this.properties[i]);
					}
				}
			}

			// Token: 0x04000B7E RID: 2942
			private const int InitialSize = 2;

			// Token: 0x04000B7F RID: 2943
			public VisualElement[] elements;

			// Token: 0x04000B80 RID: 2944
			public StylePropertyId[] properties;

			// Token: 0x04000B81 RID: 2945
			public TTimingData[] timing;

			// Token: 0x04000B82 RID: 2946
			public TStyleData[] style;

			// Token: 0x04000B83 RID: 2947
			public int count;

			// Token: 0x04000B84 RID: 2948
			private Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, int> indices;
		}

		// Token: 0x0200032D RID: 813
		private struct ElementPropertyPair
		{
			// Token: 0x06001BA4 RID: 7076 RVA: 0x0006BAD2 File Offset: 0x00069CD2
			public ElementPropertyPair(VisualElement element, StylePropertyId property)
			{
				this.element = element;
				this.property = property;
			}

			// Token: 0x04000B85 RID: 2949
			public static readonly IEqualityComparer<StylePropertyAnimationSystem.ElementPropertyPair> Comparer = new StylePropertyAnimationSystem.ElementPropertyPair.EqualityComparer();

			// Token: 0x04000B86 RID: 2950
			public readonly VisualElement element;

			// Token: 0x04000B87 RID: 2951
			public readonly StylePropertyId property;

			// Token: 0x0200032E RID: 814
			private class EqualityComparer : IEqualityComparer<StylePropertyAnimationSystem.ElementPropertyPair>
			{
				// Token: 0x06001BA6 RID: 7078 RVA: 0x0006BAF0 File Offset: 0x00069CF0
				public bool Equals(StylePropertyAnimationSystem.ElementPropertyPair x, StylePropertyAnimationSystem.ElementPropertyPair y)
				{
					return x.element == y.element && x.property == y.property;
				}

				// Token: 0x06001BA7 RID: 7079 RVA: 0x0006BB24 File Offset: 0x00069D24
				public int GetHashCode(StylePropertyAnimationSystem.ElementPropertyPair obj)
				{
					return (obj.element.GetHashCode() * 397) ^ (int)obj.property;
				}
			}
		}

		// Token: 0x0200032F RID: 815
		private abstract class Values
		{
			// Token: 0x06001BA9 RID: 7081
			public abstract void CancelAllAnimations();

			// Token: 0x06001BAA RID: 7082
			public abstract void CancelAllAnimations(VisualElement ve);

			// Token: 0x06001BAB RID: 7083
			public abstract void CancelAnimation(VisualElement ve, StylePropertyId id);

			// Token: 0x06001BAC RID: 7084
			public abstract bool HasRunningAnimation(VisualElement ve, StylePropertyId id);

			// Token: 0x06001BAD RID: 7085
			public abstract void UpdateAnimation(VisualElement ve, StylePropertyId id);

			// Token: 0x06001BAE RID: 7086
			public abstract void GetAllAnimations(VisualElement ve, List<StylePropertyId> outPropertyIds);

			// Token: 0x06001BAF RID: 7087
			public abstract void Update(long currentTimeMs);

			// Token: 0x06001BB0 RID: 7088
			protected abstract void UpdateValues();

			// Token: 0x06001BB1 RID: 7089
			protected abstract void UpdateComputedStyle();

			// Token: 0x06001BB2 RID: 7090
			protected abstract void UpdateComputedStyle(int i);
		}

		// Token: 0x02000330 RID: 816
		private abstract class Values<T> : StylePropertyAnimationSystem.Values
		{
			// Token: 0x17000692 RID: 1682
			// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x0006BB4F File Offset: 0x00069D4F
			public bool isEmpty
			{
				get
				{
					return this.running.count + this.completed.count == 0;
				}
			}

			// Token: 0x17000693 RID: 1683
			// (get) Token: 0x06001BB5 RID: 7093
			public abstract Func<T, T, bool> SameFunc { get; }

			// Token: 0x06001BB6 RID: 7094 RVA: 0x0006BB6C File Offset: 0x00069D6C
			protected virtual bool ConvertUnits(VisualElement owner, StylePropertyId prop, ref T a, ref T b)
			{
				return true;
			}

			// Token: 0x06001BB7 RID: 7095 RVA: 0x0006BB80 File Offset: 0x00069D80
			protected Values()
			{
				this.running = StylePropertyAnimationSystem.AnimationDataSet<StylePropertyAnimationSystem.Values<T>.TimingData, StylePropertyAnimationSystem.Values<T>.StyleData>.Create();
				this.completed = StylePropertyAnimationSystem.AnimationDataSet<StylePropertyAnimationSystem.Values<T>.EmptyData, T>.Create();
				this.m_CurrentTimeMs = Panel.TimeSinceStartupMs();
			}

			// Token: 0x06001BB8 RID: 7096 RVA: 0x0006BBD4 File Offset: 0x00069DD4
			private void SwapFrameStates()
			{
				StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState currentFrameEventsState = this.m_CurrentFrameEventsState;
				this.m_CurrentFrameEventsState = this.m_NextFrameEventsState;
				this.m_NextFrameEventsState = currentFrameEventsState;
			}

			// Token: 0x06001BB9 RID: 7097 RVA: 0x0006BBFC File Offset: 0x00069DFC
			private void QueueEvent(EventBase evt, StylePropertyAnimationSystem.ElementPropertyPair epp)
			{
				evt.target = epp.element;
				Queue<EventBase> pooledQueue;
				bool flag = !this.m_NextFrameEventsState.elementPropertyQueuedEvents.TryGetValue(epp, out pooledQueue);
				if (flag)
				{
					pooledQueue = StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState.GetPooledQueue();
					this.m_NextFrameEventsState.elementPropertyQueuedEvents.Add(epp, pooledQueue);
				}
				pooledQueue.Enqueue(evt);
				bool flag2 = this.m_NextFrameEventsState.panel == null;
				if (flag2)
				{
					this.m_NextFrameEventsState.panel = epp.element.panel;
				}
				this.m_NextFrameEventsState.RegisterChange();
			}

			// Token: 0x06001BBA RID: 7098 RVA: 0x0006BC88 File Offset: 0x00069E88
			private void ClearEventQueue(StylePropertyAnimationSystem.ElementPropertyPair epp)
			{
				Queue<EventBase> queue;
				bool flag = this.m_NextFrameEventsState.elementPropertyQueuedEvents.TryGetValue(epp, out queue);
				if (flag)
				{
					while (queue.Count > 0)
					{
						queue.Dequeue().Dispose();
						this.m_NextFrameEventsState.UnregisterChange();
					}
				}
			}

			// Token: 0x06001BBB RID: 7099 RVA: 0x0006BCD8 File Offset: 0x00069ED8
			private void QueueTransitionRunEvent(VisualElement ve, int runningIndex)
			{
				bool flag = !ve.HasParentEventCallbacksOrDefaultActions(EventCategory.StyleTransition);
				if (!flag)
				{
					StylePropertyId stylePropertyId = this.running.properties[runningIndex];
					StylePropertyAnimationSystem.ElementPropertyPair elementPropertyPair = new StylePropertyAnimationSystem.ElementPropertyPair(ve, stylePropertyId);
					StylePropertyAnimationSystem.TransitionState transitionState;
					bool flag2 = this.m_NextFrameEventsState.elementPropertyStateDelta.TryGetValue(elementPropertyPair, out transitionState);
					if (flag2)
					{
						this.m_NextFrameEventsState.elementPropertyStateDelta[elementPropertyPair] = transitionState | StylePropertyAnimationSystem.TransitionState.Running;
					}
					else
					{
						this.m_NextFrameEventsState.elementPropertyStateDelta.Add(elementPropertyPair, StylePropertyAnimationSystem.TransitionState.Running);
					}
					ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[runningIndex];
					int num = ((ptr.delayMs < 0) ? Mathf.Min(Mathf.Max(-ptr.delayMs, 0), ptr.durationMs) : 0);
					TransitionRunEvent pooled = TransitionEventBase<TransitionRunEvent>.GetPooled(new StylePropertyName(stylePropertyId), (double)((float)num / 1000f));
					this.QueueEvent(pooled, elementPropertyPair);
				}
			}

			// Token: 0x06001BBC RID: 7100 RVA: 0x0006BDB0 File Offset: 0x00069FB0
			private void QueueTransitionStartEvent(VisualElement ve, int runningIndex)
			{
				bool flag = !ve.HasParentEventCallbacksOrDefaultActions(EventCategory.StyleTransition);
				if (!flag)
				{
					StylePropertyId stylePropertyId = this.running.properties[runningIndex];
					StylePropertyAnimationSystem.ElementPropertyPair elementPropertyPair = new StylePropertyAnimationSystem.ElementPropertyPair(ve, stylePropertyId);
					StylePropertyAnimationSystem.TransitionState transitionState;
					bool flag2 = this.m_NextFrameEventsState.elementPropertyStateDelta.TryGetValue(elementPropertyPair, out transitionState);
					if (flag2)
					{
						this.m_NextFrameEventsState.elementPropertyStateDelta[elementPropertyPair] = transitionState | StylePropertyAnimationSystem.TransitionState.Started;
					}
					else
					{
						this.m_NextFrameEventsState.elementPropertyStateDelta.Add(elementPropertyPair, StylePropertyAnimationSystem.TransitionState.Started);
					}
					ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[runningIndex];
					int num = ((ptr.delayMs < 0) ? Mathf.Min(Mathf.Max(-ptr.delayMs, 0), ptr.durationMs) : 0);
					TransitionStartEvent pooled = TransitionEventBase<TransitionStartEvent>.GetPooled(new StylePropertyName(stylePropertyId), (double)((float)num / 1000f));
					this.QueueEvent(pooled, elementPropertyPair);
				}
			}

			// Token: 0x06001BBD RID: 7101 RVA: 0x0006BE88 File Offset: 0x0006A088
			private void QueueTransitionEndEvent(VisualElement ve, int runningIndex)
			{
				bool flag = !ve.HasParentEventCallbacksOrDefaultActions(EventCategory.StyleTransition);
				if (!flag)
				{
					StylePropertyId stylePropertyId = this.running.properties[runningIndex];
					StylePropertyAnimationSystem.ElementPropertyPair elementPropertyPair = new StylePropertyAnimationSystem.ElementPropertyPair(ve, stylePropertyId);
					StylePropertyAnimationSystem.TransitionState transitionState;
					bool flag2 = this.m_NextFrameEventsState.elementPropertyStateDelta.TryGetValue(elementPropertyPair, out transitionState);
					if (flag2)
					{
						this.m_NextFrameEventsState.elementPropertyStateDelta[elementPropertyPair] = transitionState | StylePropertyAnimationSystem.TransitionState.Ended;
					}
					else
					{
						this.m_NextFrameEventsState.elementPropertyStateDelta.Add(elementPropertyPair, StylePropertyAnimationSystem.TransitionState.Ended);
					}
					ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[runningIndex];
					TransitionEndEvent pooled = TransitionEventBase<TransitionEndEvent>.GetPooled(new StylePropertyName(stylePropertyId), (double)((float)ptr.durationMs / 1000f));
					this.QueueEvent(pooled, elementPropertyPair);
				}
			}

			// Token: 0x06001BBE RID: 7102 RVA: 0x0006BF40 File Offset: 0x0006A140
			private void QueueTransitionCancelEvent(VisualElement ve, int runningIndex, long panelElapsedMs)
			{
				bool flag = !ve.HasParentEventCallbacksOrDefaultActions(EventCategory.StyleTransition);
				if (!flag)
				{
					StylePropertyId stylePropertyId = this.running.properties[runningIndex];
					StylePropertyAnimationSystem.ElementPropertyPair elementPropertyPair = new StylePropertyAnimationSystem.ElementPropertyPair(ve, stylePropertyId);
					StylePropertyAnimationSystem.TransitionState transitionState;
					bool flag2 = this.m_NextFrameEventsState.elementPropertyStateDelta.TryGetValue(elementPropertyPair, out transitionState);
					bool flag4;
					if (flag2)
					{
						bool flag3 = transitionState == StylePropertyAnimationSystem.TransitionState.None || (transitionState & StylePropertyAnimationSystem.TransitionState.Canceled) == StylePropertyAnimationSystem.TransitionState.Canceled;
						if (flag3)
						{
							this.m_NextFrameEventsState.elementPropertyStateDelta[elementPropertyPair] = StylePropertyAnimationSystem.TransitionState.Canceled;
							this.ClearEventQueue(elementPropertyPair);
							flag4 = true;
						}
						else
						{
							this.m_NextFrameEventsState.elementPropertyStateDelta[elementPropertyPair] = StylePropertyAnimationSystem.TransitionState.None;
							this.ClearEventQueue(elementPropertyPair);
							flag4 = false;
						}
					}
					else
					{
						this.m_NextFrameEventsState.elementPropertyStateDelta.Add(elementPropertyPair, StylePropertyAnimationSystem.TransitionState.Canceled);
						flag4 = true;
					}
					bool flag5 = !flag4;
					if (!flag5)
					{
						ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[runningIndex];
						long num = (ptr.isStarted ? (panelElapsedMs - ptr.startTimeMs) : 0L);
						bool flag6 = ptr.delayMs < 0;
						if (flag6)
						{
							num = (long)(-(long)ptr.delayMs) + num;
						}
						TransitionCancelEvent pooled = TransitionEventBase<TransitionCancelEvent>.GetPooled(new StylePropertyName(stylePropertyId), (double)((float)num / 1000f));
						this.QueueEvent(pooled, elementPropertyPair);
					}
				}
			}

			// Token: 0x06001BBF RID: 7103 RVA: 0x0006C07C File Offset: 0x0006A27C
			private void SendTransitionCancelEvent(VisualElement ve, int runningIndex, long panelElapsedMs)
			{
				bool flag = !ve.HasParentEventCallbacksOrDefaultActions(EventBase<TransitionCancelEvent>.EventCategory);
				if (!flag)
				{
					ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[runningIndex];
					StylePropertyId stylePropertyId = this.running.properties[runningIndex];
					long num = (ptr.isStarted ? (panelElapsedMs - ptr.startTimeMs) : 0L);
					bool flag2 = ptr.delayMs < 0;
					if (flag2)
					{
						num = (long)(-(long)ptr.delayMs) + num;
					}
					using (TransitionCancelEvent pooled = TransitionEventBase<TransitionCancelEvent>.GetPooled(new StylePropertyName(stylePropertyId), (double)((float)num / 1000f)))
					{
						pooled.target = ve;
						ve.SendEvent(pooled);
					}
				}
			}

			// Token: 0x06001BC0 RID: 7104 RVA: 0x0006C13C File Offset: 0x0006A33C
			public sealed override void CancelAllAnimations()
			{
				int count = this.running.count;
				bool flag = count > 0;
				if (flag)
				{
					using (new EventDispatcherGate(this.running.elements[0].panel.dispatcher))
					{
						for (int i = 0; i < count; i++)
						{
							VisualElement visualElement = this.running.elements[i];
							this.SendTransitionCancelEvent(visualElement, i, this.m_CurrentTimeMs);
							this.ForceComputedStyleEndValue(i);
							IStylePropertyAnimations styleAnimation = visualElement.styleAnimation;
							int num = styleAnimation.runningAnimationCount;
							styleAnimation.runningAnimationCount = num - 1;
						}
					}
					this.running.RemoveAll();
				}
				int count2 = this.completed.count;
				for (int j = 0; j < count2; j++)
				{
					VisualElement visualElement2 = this.completed.elements[j];
					IStylePropertyAnimations styleAnimation2 = visualElement2.styleAnimation;
					int num = styleAnimation2.completedAnimationCount;
					styleAnimation2.completedAnimationCount = num - 1;
				}
				this.completed.RemoveAll();
			}

			// Token: 0x06001BC1 RID: 7105 RVA: 0x0006C264 File Offset: 0x0006A464
			public sealed override void CancelAllAnimations(VisualElement ve)
			{
				int count = this.running.count;
				bool flag = count > 0;
				if (flag)
				{
					using (new EventDispatcherGate(this.running.elements[0].panel.dispatcher))
					{
						for (int i = 0; i < count; i++)
						{
							bool flag2 = this.running.elements[i] == ve;
							if (flag2)
							{
								this.SendTransitionCancelEvent(ve, i, this.m_CurrentTimeMs);
								this.ForceComputedStyleEndValue(i);
								IStylePropertyAnimations styleAnimation = this.running.elements[i].styleAnimation;
								int num = styleAnimation.runningAnimationCount;
								styleAnimation.runningAnimationCount = num - 1;
							}
						}
					}
				}
				this.running.RemoveAll(ve);
				int count2 = this.completed.count;
				for (int j = 0; j < count2; j++)
				{
					bool flag3 = this.completed.elements[j] == ve;
					if (flag3)
					{
						IStylePropertyAnimations styleAnimation2 = this.completed.elements[j].styleAnimation;
						int num = styleAnimation2.completedAnimationCount;
						styleAnimation2.completedAnimationCount = num - 1;
					}
				}
				this.completed.RemoveAll(ve);
			}

			// Token: 0x06001BC2 RID: 7106 RVA: 0x0006C3B8 File Offset: 0x0006A5B8
			public sealed override void CancelAnimation(VisualElement ve, StylePropertyId id)
			{
				int num;
				bool flag = this.running.IndexOf(ve, id, out num);
				if (flag)
				{
					this.QueueTransitionCancelEvent(ve, num, this.m_CurrentTimeMs);
					this.ForceComputedStyleEndValue(num);
					this.running.Remove(num);
					IStylePropertyAnimations styleAnimation = ve.styleAnimation;
					int num2 = styleAnimation.runningAnimationCount;
					styleAnimation.runningAnimationCount = num2 - 1;
				}
				int num3;
				bool flag2 = this.completed.IndexOf(ve, id, out num3);
				if (flag2)
				{
					this.completed.Remove(num3);
					IStylePropertyAnimations styleAnimation2 = ve.styleAnimation;
					int num2 = styleAnimation2.completedAnimationCount;
					styleAnimation2.completedAnimationCount = num2 - 1;
				}
			}

			// Token: 0x06001BC3 RID: 7107 RVA: 0x0006C450 File Offset: 0x0006A650
			public sealed override bool HasRunningAnimation(VisualElement ve, StylePropertyId id)
			{
				int num;
				return this.running.IndexOf(ve, id, out num);
			}

			// Token: 0x06001BC4 RID: 7108 RVA: 0x0006C474 File Offset: 0x0006A674
			public sealed override void UpdateAnimation(VisualElement ve, StylePropertyId id)
			{
				int num;
				bool flag = this.running.IndexOf(ve, id, out num);
				if (flag)
				{
					this.UpdateComputedStyle(num);
				}
			}

			// Token: 0x06001BC5 RID: 7109 RVA: 0x0006C49D File Offset: 0x0006A69D
			public sealed override void GetAllAnimations(VisualElement ve, List<StylePropertyId> outPropertyIds)
			{
				this.running.GetActivePropertiesForElement(ve, outPropertyIds);
				this.completed.GetActivePropertiesForElement(ve, outPropertyIds);
			}

			// Token: 0x06001BC6 RID: 7110 RVA: 0x0006C4BC File Offset: 0x0006A6BC
			private float ComputeReversingShorteningFactor(int oldIndex)
			{
				ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[oldIndex];
				return Mathf.Clamp01(Mathf.Abs(1f - (1f - ptr.easedProgress) * ptr.reversingShorteningFactor));
			}

			// Token: 0x06001BC7 RID: 7111 RVA: 0x0006C504 File Offset: 0x0006A704
			private int ComputeReversingDuration(int newTransitionDurationMs, float newReversingShorteningFactor)
			{
				return Mathf.RoundToInt((float)newTransitionDurationMs * newReversingShorteningFactor);
			}

			// Token: 0x06001BC8 RID: 7112 RVA: 0x0006C520 File Offset: 0x0006A720
			private int ComputeReversingDelay(int delayMs, float newReversingShorteningFactor)
			{
				return (delayMs < 0) ? Mathf.RoundToInt((float)delayMs * newReversingShorteningFactor) : delayMs;
			}

			// Token: 0x06001BC9 RID: 7113 RVA: 0x0006C544 File Offset: 0x0006A744
			public bool StartTransition(VisualElement owner, StylePropertyId prop, T startValue, T endValue, int durationMs, int delayMs, Func<float, float> easingCurve, long currentTimeMs)
			{
				long num = currentTimeMs + (long)delayMs;
				StylePropertyAnimationSystem.Values<T>.TimingData timingData = new StylePropertyAnimationSystem.Values<T>.TimingData
				{
					startTimeMs = num,
					durationMs = durationMs,
					easingCurve = easingCurve,
					reversingShorteningFactor = 1f,
					delayMs = delayMs
				};
				StylePropertyAnimationSystem.Values<T>.StyleData styleData = new StylePropertyAnimationSystem.Values<T>.StyleData
				{
					startValue = startValue,
					endValue = endValue,
					currentValue = startValue,
					reversingAdjustedStartValue = startValue
				};
				int num2 = Mathf.Max(0, durationMs) + delayMs;
				bool flag = !this.ConvertUnits(owner, prop, ref styleData.startValue, ref styleData.endValue);
				bool flag2;
				if (flag)
				{
					flag2 = false;
				}
				else
				{
					int num3;
					bool flag3 = this.completed.IndexOf(owner, prop, out num3);
					if (flag3)
					{
						bool flag4 = this.SameFunc(endValue, this.completed.style[num3]);
						if (flag4)
						{
							return false;
						}
						bool flag5 = num2 <= 0;
						if (flag5)
						{
							return false;
						}
						this.completed.Remove(num3);
						IStylePropertyAnimations styleAnimation = owner.styleAnimation;
						int num4 = styleAnimation.completedAnimationCount;
						styleAnimation.completedAnimationCount = num4 - 1;
					}
					int num5;
					bool flag6 = this.running.IndexOf(owner, prop, out num5);
					if (flag6)
					{
						bool flag7 = this.SameFunc(endValue, this.running.style[num5].endValue);
						if (flag7)
						{
							flag2 = false;
						}
						else
						{
							bool flag8 = this.SameFunc(endValue, this.running.style[num5].currentValue);
							if (flag8)
							{
								this.QueueTransitionCancelEvent(owner, num5, currentTimeMs);
								this.running.Remove(num5);
								IStylePropertyAnimations styleAnimation2 = owner.styleAnimation;
								int num4 = styleAnimation2.runningAnimationCount;
								styleAnimation2.runningAnimationCount = num4 - 1;
								flag2 = false;
							}
							else
							{
								bool flag9 = num2 <= 0;
								if (flag9)
								{
									this.QueueTransitionCancelEvent(owner, num5, currentTimeMs);
									this.running.Remove(num5);
									IStylePropertyAnimations styleAnimation3 = owner.styleAnimation;
									int num4 = styleAnimation3.runningAnimationCount;
									styleAnimation3.runningAnimationCount = num4 - 1;
									flag2 = false;
								}
								else
								{
									styleData.startValue = this.running.style[num5].currentValue;
									bool flag10 = !this.ConvertUnits(owner, prop, ref styleData.startValue, ref styleData.endValue);
									if (flag10)
									{
										this.QueueTransitionCancelEvent(owner, num5, currentTimeMs);
										this.running.Remove(num5);
										IStylePropertyAnimations styleAnimation4 = owner.styleAnimation;
										int num4 = styleAnimation4.runningAnimationCount;
										styleAnimation4.runningAnimationCount = num4 - 1;
										flag2 = false;
									}
									else
									{
										styleData.currentValue = styleData.startValue;
										bool flag11 = this.SameFunc(endValue, this.running.style[num5].reversingAdjustedStartValue);
										if (flag11)
										{
											float num6 = (timingData.reversingShorteningFactor = this.ComputeReversingShorteningFactor(num5));
											timingData.startTimeMs = currentTimeMs + (long)this.ComputeReversingDelay(delayMs, num6);
											timingData.durationMs = this.ComputeReversingDuration(durationMs, num6);
											styleData.reversingAdjustedStartValue = this.running.style[num5].endValue;
										}
										this.running.timing[num5].isStarted = false;
										this.QueueTransitionCancelEvent(owner, num5, currentTimeMs);
										this.QueueTransitionRunEvent(owner, num5);
										this.running.Replace(num5, timingData, styleData);
										flag2 = true;
									}
								}
							}
						}
					}
					else
					{
						bool flag12 = num2 <= 0;
						if (flag12)
						{
							flag2 = false;
						}
						else
						{
							bool flag13 = this.SameFunc(startValue, endValue);
							if (flag13)
							{
								flag2 = false;
							}
							else
							{
								this.running.Add(owner, prop, timingData, styleData);
								IStylePropertyAnimations styleAnimation5 = owner.styleAnimation;
								int num4 = styleAnimation5.runningAnimationCount;
								styleAnimation5.runningAnimationCount = num4 + 1;
								this.QueueTransitionRunEvent(owner, this.running.count - 1);
								flag2 = true;
							}
						}
					}
				}
				return flag2;
			}

			// Token: 0x06001BCA RID: 7114 RVA: 0x0006C92C File Offset: 0x0006AB2C
			private void ForceComputedStyleEndValue(int runningIndex)
			{
				ref StylePropertyAnimationSystem.Values<T>.StyleData ptr = ref this.running.style[runningIndex];
				ptr.currentValue = ptr.endValue;
				this.UpdateComputedStyle(runningIndex);
			}

			// Token: 0x06001BCB RID: 7115 RVA: 0x0006C960 File Offset: 0x0006AB60
			public sealed override void Update(long currentTimeMs)
			{
				this.m_CurrentTimeMs = currentTimeMs;
				this.UpdateProgress(currentTimeMs);
				this.UpdateValues();
				this.UpdateComputedStyle();
				bool flag = this.m_NextFrameEventsState.StateChanged();
				if (flag)
				{
					this.ProcessEventQueue();
				}
			}

			// Token: 0x06001BCC RID: 7116 RVA: 0x0006C9A4 File Offset: 0x0006ABA4
			private void ProcessEventQueue()
			{
				this.SwapFrameStates();
				IPanel panel = this.m_CurrentFrameEventsState.panel;
				EventDispatcher eventDispatcher = ((panel != null) ? panel.dispatcher : null);
				using (new EventDispatcherGate(eventDispatcher))
				{
					foreach (KeyValuePair<StylePropertyAnimationSystem.ElementPropertyPair, Queue<EventBase>> keyValuePair in this.m_CurrentFrameEventsState.elementPropertyQueuedEvents)
					{
						StylePropertyAnimationSystem.ElementPropertyPair key = keyValuePair.Key;
						Queue<EventBase> value = keyValuePair.Value;
						VisualElement element = keyValuePair.Key.element;
						while (value.Count > 0)
						{
							EventBase eventBase = value.Dequeue();
							element.SendEvent(eventBase);
							eventBase.Dispose();
						}
					}
					this.m_CurrentFrameEventsState.Clear();
				}
			}

			// Token: 0x06001BCD RID: 7117 RVA: 0x0006CA9C File Offset: 0x0006AC9C
			private void UpdateProgress(long currentTimeMs)
			{
				int num = this.running.count;
				bool flag = num > 0;
				if (flag)
				{
					for (int i = 0; i < num; i++)
					{
						ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[i];
						bool flag2 = currentTimeMs < ptr.startTimeMs;
						if (flag2)
						{
							ptr.easedProgress = 0f;
						}
						else
						{
							bool flag3 = currentTimeMs >= ptr.startTimeMs + (long)ptr.durationMs;
							if (flag3)
							{
								ref StylePropertyAnimationSystem.Values<T>.StyleData ptr2 = ref this.running.style[i];
								ref VisualElement ptr3 = ref this.running.elements[i];
								ptr2.currentValue = ptr2.endValue;
								this.UpdateComputedStyle(i);
								this.completed.Add(ptr3, this.running.properties[i], StylePropertyAnimationSystem.Values<T>.EmptyData.Default, ptr2.endValue);
								IStylePropertyAnimations styleAnimation = ptr3.styleAnimation;
								int num2 = styleAnimation.runningAnimationCount;
								styleAnimation.runningAnimationCount = num2 - 1;
								IStylePropertyAnimations styleAnimation2 = ptr3.styleAnimation;
								num2 = styleAnimation2.completedAnimationCount;
								styleAnimation2.completedAnimationCount = num2 + 1;
								this.QueueTransitionEndEvent(ptr3, i);
								this.running.Remove(i);
								i--;
								num--;
							}
							else
							{
								bool flag4 = !ptr.isStarted;
								if (flag4)
								{
									ptr.isStarted = true;
									this.QueueTransitionStartEvent(this.running.elements[i], i);
								}
								float num3 = (float)(currentTimeMs - ptr.startTimeMs) / (float)ptr.durationMs;
								ptr.easedProgress = ptr.easingCurve(num3);
							}
						}
					}
				}
			}

			// Token: 0x04000B88 RID: 2952
			private long m_CurrentTimeMs = 0L;

			// Token: 0x04000B89 RID: 2953
			private StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState m_CurrentFrameEventsState = new StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState();

			// Token: 0x04000B8A RID: 2954
			private StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState m_NextFrameEventsState = new StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState();

			// Token: 0x04000B8B RID: 2955
			public StylePropertyAnimationSystem.AnimationDataSet<StylePropertyAnimationSystem.Values<T>.TimingData, StylePropertyAnimationSystem.Values<T>.StyleData> running;

			// Token: 0x04000B8C RID: 2956
			public StylePropertyAnimationSystem.AnimationDataSet<StylePropertyAnimationSystem.Values<T>.EmptyData, T> completed;

			// Token: 0x02000331 RID: 817
			private class TransitionEventsFrameState
			{
				// Token: 0x06001BCE RID: 7118 RVA: 0x0006CC40 File Offset: 0x0006AE40
				public static Queue<EventBase> GetPooledQueue()
				{
					return StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState.k_EventQueuePool.Get();
				}

				// Token: 0x06001BCF RID: 7119 RVA: 0x0006CC5C File Offset: 0x0006AE5C
				public void RegisterChange()
				{
					this.m_ChangesCount++;
				}

				// Token: 0x06001BD0 RID: 7120 RVA: 0x0006CC6D File Offset: 0x0006AE6D
				public void UnregisterChange()
				{
					this.m_ChangesCount--;
				}

				// Token: 0x06001BD1 RID: 7121 RVA: 0x0006CC80 File Offset: 0x0006AE80
				public bool StateChanged()
				{
					return this.m_ChangesCount > 0;
				}

				// Token: 0x06001BD2 RID: 7122 RVA: 0x0006CC9C File Offset: 0x0006AE9C
				public void Clear()
				{
					foreach (KeyValuePair<StylePropertyAnimationSystem.ElementPropertyPair, Queue<EventBase>> keyValuePair in this.elementPropertyQueuedEvents)
					{
						keyValuePair.Value.Clear();
						StylePropertyAnimationSystem.Values<T>.TransitionEventsFrameState.k_EventQueuePool.Release(keyValuePair.Value);
					}
					this.elementPropertyQueuedEvents.Clear();
					this.elementPropertyStateDelta.Clear();
					this.panel = null;
					this.m_ChangesCount = 0;
				}

				// Token: 0x04000B8D RID: 2957
				private static readonly ObjectPool<Queue<EventBase>> k_EventQueuePool = new ObjectPool<Queue<EventBase>>(() => new Queue<EventBase>(4), null, null, null, true, 10, 10000);

				// Token: 0x04000B8E RID: 2958
				public readonly Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, StylePropertyAnimationSystem.TransitionState> elementPropertyStateDelta = new Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, StylePropertyAnimationSystem.TransitionState>(StylePropertyAnimationSystem.ElementPropertyPair.Comparer);

				// Token: 0x04000B8F RID: 2959
				public readonly Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, Queue<EventBase>> elementPropertyQueuedEvents = new Dictionary<StylePropertyAnimationSystem.ElementPropertyPair, Queue<EventBase>>(StylePropertyAnimationSystem.ElementPropertyPair.Comparer);

				// Token: 0x04000B90 RID: 2960
				public IPanel panel;

				// Token: 0x04000B91 RID: 2961
				private int m_ChangesCount;
			}

			// Token: 0x02000333 RID: 819
			public struct TimingData
			{
				// Token: 0x04000B93 RID: 2963
				public long startTimeMs;

				// Token: 0x04000B94 RID: 2964
				public int durationMs;

				// Token: 0x04000B95 RID: 2965
				public Func<float, float> easingCurve;

				// Token: 0x04000B96 RID: 2966
				public float easedProgress;

				// Token: 0x04000B97 RID: 2967
				public float reversingShorteningFactor;

				// Token: 0x04000B98 RID: 2968
				public bool isStarted;

				// Token: 0x04000B99 RID: 2969
				public int delayMs;
			}

			// Token: 0x02000334 RID: 820
			public struct StyleData
			{
				// Token: 0x04000B9A RID: 2970
				public T startValue;

				// Token: 0x04000B9B RID: 2971
				public T endValue;

				// Token: 0x04000B9C RID: 2972
				public T reversingAdjustedStartValue;

				// Token: 0x04000B9D RID: 2973
				public T currentValue;
			}

			// Token: 0x02000335 RID: 821
			public struct EmptyData
			{
				// Token: 0x04000B9E RID: 2974
				public static StylePropertyAnimationSystem.Values<T>.EmptyData Default = default(StylePropertyAnimationSystem.Values<T>.EmptyData);
			}
		}

		// Token: 0x02000336 RID: 822
		private class ValuesFloat : StylePropertyAnimationSystem.Values<float>
		{
			// Token: 0x17000694 RID: 1684
			// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x0006CDA5 File Offset: 0x0006AFA5
			public override Func<float, float, bool> SameFunc { get; } = new Func<float, float, bool>(StylePropertyAnimationSystem.ValuesFloat.IsSame);

			// Token: 0x06001BDA RID: 7130 RVA: 0x0006CDAD File Offset: 0x0006AFAD
			private static bool IsSame(float a, float b)
			{
				return Mathf.Approximately(a, b);
			}

			// Token: 0x06001BDB RID: 7131 RVA: 0x0006CDB6 File Offset: 0x0006AFB6
			private static float Lerp(float a, float b, float t)
			{
				return Mathf.LerpUnclamped(a, b, t);
			}

			// Token: 0x06001BDC RID: 7132 RVA: 0x0006CDC0 File Offset: 0x0006AFC0
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<float>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<float>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesFloat.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}

			// Token: 0x06001BDD RID: 7133 RVA: 0x0006CE30 File Offset: 0x0006B030
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001BDE RID: 7134 RVA: 0x0006CEA4 File Offset: 0x0006B0A4
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x02000337 RID: 823
		private class ValuesInt : StylePropertyAnimationSystem.Values<int>
		{
			// Token: 0x17000695 RID: 1685
			// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x0006CF15 File Offset: 0x0006B115
			public override Func<int, int, bool> SameFunc { get; } = new Func<int, int, bool>(StylePropertyAnimationSystem.ValuesInt.IsSame);

			// Token: 0x06001BE1 RID: 7137 RVA: 0x0006CF1D File Offset: 0x0006B11D
			private static bool IsSame(int a, int b)
			{
				return a == b;
			}

			// Token: 0x06001BE2 RID: 7138 RVA: 0x0006CF23 File Offset: 0x0006B123
			private static int Lerp(int a, int b, float t)
			{
				return Mathf.RoundToInt(Mathf.LerpUnclamped((float)a, (float)b, t));
			}

			// Token: 0x06001BE3 RID: 7139 RVA: 0x0006CF34 File Offset: 0x0006B134
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<int>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<int>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesInt.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}

			// Token: 0x06001BE4 RID: 7140 RVA: 0x0006CFA4 File Offset: 0x0006B1A4
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001BE5 RID: 7141 RVA: 0x0006D018 File Offset: 0x0006B218
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x02000338 RID: 824
		private class ValuesLength : StylePropertyAnimationSystem.Values<Length>
		{
			// Token: 0x17000696 RID: 1686
			// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x0006D089 File Offset: 0x0006B289
			public override Func<Length, Length, bool> SameFunc { get; } = new Func<Length, Length, bool>(StylePropertyAnimationSystem.ValuesLength.IsSame);

			// Token: 0x06001BE8 RID: 7144 RVA: 0x0006D091 File Offset: 0x0006B291
			private static bool IsSame(Length a, Length b)
			{
				return a.unit == b.unit && Mathf.Approximately(a.value, b.value);
			}

			// Token: 0x06001BE9 RID: 7145 RVA: 0x0006D0BC File Offset: 0x0006B2BC
			protected sealed override bool ConvertUnits(VisualElement owner, StylePropertyId prop, ref Length a, ref Length b)
			{
				return owner.TryConvertLengthUnits(prop, ref a, ref b, 0);
			}

			// Token: 0x06001BEA RID: 7146 RVA: 0x0006D0D9 File Offset: 0x0006B2D9
			internal static Length Lerp(Length a, Length b, float t)
			{
				return new Length(Mathf.LerpUnclamped(a.value, b.value, t), b.unit);
			}

			// Token: 0x06001BEB RID: 7147 RVA: 0x0006D0FC File Offset: 0x0006B2FC
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<Length>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<Length>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesLength.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}

			// Token: 0x06001BEC RID: 7148 RVA: 0x0006D16C File Offset: 0x0006B36C
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001BED RID: 7149 RVA: 0x0006D1E0 File Offset: 0x0006B3E0
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x02000339 RID: 825
		private class ValuesColor : StylePropertyAnimationSystem.Values<Color>
		{
			// Token: 0x17000697 RID: 1687
			// (get) Token: 0x06001BEF RID: 7151 RVA: 0x0006D251 File Offset: 0x0006B451
			public override Func<Color, Color, bool> SameFunc { get; } = new Func<Color, Color, bool>(StylePropertyAnimationSystem.ValuesColor.IsSame);

			// Token: 0x06001BF0 RID: 7152 RVA: 0x0006D25C File Offset: 0x0006B45C
			private static bool IsSame(Color c, Color d)
			{
				return Mathf.Approximately(c.r, d.r) && Mathf.Approximately(c.g, d.g) && Mathf.Approximately(c.b, d.b) && Mathf.Approximately(c.a, d.a);
			}

			// Token: 0x06001BF1 RID: 7153 RVA: 0x0006D2B6 File Offset: 0x0006B4B6
			private static Color Lerp(Color a, Color b, float t)
			{
				return Color.LerpUnclamped(a, b, t);
			}

			// Token: 0x06001BF2 RID: 7154 RVA: 0x0006D2C0 File Offset: 0x0006B4C0
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<Color>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<Color>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesColor.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}

			// Token: 0x06001BF3 RID: 7155 RVA: 0x0006D330 File Offset: 0x0006B530
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001BF4 RID: 7156 RVA: 0x0006D3A4 File Offset: 0x0006B5A4
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x0200033A RID: 826
		private abstract class ValuesDiscrete<T> : StylePropertyAnimationSystem.Values<T>
		{
			// Token: 0x17000698 RID: 1688
			// (get) Token: 0x06001BF6 RID: 7158 RVA: 0x0006D415 File Offset: 0x0006B615
			public override Func<T, T, bool> SameFunc { get; } = new Func<T, T, bool>(StylePropertyAnimationSystem.ValuesDiscrete<T>.IsSame);

			// Token: 0x06001BF7 RID: 7159 RVA: 0x0006D41D File Offset: 0x0006B61D
			private static bool IsSame(T a, T b)
			{
				return EqualityComparer<T>.Default.Equals(a, b);
			}

			// Token: 0x06001BF8 RID: 7160 RVA: 0x0006D42B File Offset: 0x0006B62B
			private static T Lerp(T a, T b, float t)
			{
				return (t < 0.5f) ? a : b;
			}

			// Token: 0x06001BF9 RID: 7161 RVA: 0x0006D43C File Offset: 0x0006B63C
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<T>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<T>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesDiscrete<T>.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}
		}

		// Token: 0x0200033B RID: 827
		private class ValuesEnum : StylePropertyAnimationSystem.ValuesDiscrete<int>
		{
			// Token: 0x06001BFB RID: 7163 RVA: 0x0006D4C8 File Offset: 0x0006B6C8
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001BFC RID: 7164 RVA: 0x0006D53C File Offset: 0x0006B73C
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x0200033C RID: 828
		private class ValuesBackground : StylePropertyAnimationSystem.ValuesDiscrete<Background>
		{
			// Token: 0x06001BFE RID: 7166 RVA: 0x0006D59C File Offset: 0x0006B79C
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001BFF RID: 7167 RVA: 0x0006D610 File Offset: 0x0006B810
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x0200033D RID: 829
		private class ValuesFontDefinition : StylePropertyAnimationSystem.ValuesDiscrete<FontDefinition>
		{
			// Token: 0x06001C01 RID: 7169 RVA: 0x0006D670 File Offset: 0x0006B870
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001C02 RID: 7170 RVA: 0x0006D6E4 File Offset: 0x0006B8E4
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x0200033E RID: 830
		private class ValuesFont : StylePropertyAnimationSystem.ValuesDiscrete<Font>
		{
			// Token: 0x06001C04 RID: 7172 RVA: 0x0006D744 File Offset: 0x0006B944
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001C05 RID: 7173 RVA: 0x0006D7B8 File Offset: 0x0006B9B8
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x0200033F RID: 831
		private class ValuesTextShadow : StylePropertyAnimationSystem.Values<TextShadow>
		{
			// Token: 0x17000699 RID: 1689
			// (get) Token: 0x06001C07 RID: 7175 RVA: 0x0006D817 File Offset: 0x0006BA17
			public override Func<TextShadow, TextShadow, bool> SameFunc { get; } = new Func<TextShadow, TextShadow, bool>(StylePropertyAnimationSystem.ValuesTextShadow.IsSame);

			// Token: 0x06001C08 RID: 7176 RVA: 0x0006D81F File Offset: 0x0006BA1F
			private static bool IsSame(TextShadow a, TextShadow b)
			{
				return a == b;
			}

			// Token: 0x06001C09 RID: 7177 RVA: 0x0006D828 File Offset: 0x0006BA28
			private static TextShadow Lerp(TextShadow a, TextShadow b, float t)
			{
				return TextShadow.LerpUnclamped(a, b, t);
			}

			// Token: 0x06001C0A RID: 7178 RVA: 0x0006D834 File Offset: 0x0006BA34
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<TextShadow>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<TextShadow>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesTextShadow.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}

			// Token: 0x06001C0B RID: 7179 RVA: 0x0006D8A4 File Offset: 0x0006BAA4
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001C0C RID: 7180 RVA: 0x0006D918 File Offset: 0x0006BB18
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x02000340 RID: 832
		private class ValuesScale : StylePropertyAnimationSystem.Values<Scale>
		{
			// Token: 0x1700069A RID: 1690
			// (get) Token: 0x06001C0E RID: 7182 RVA: 0x0006D989 File Offset: 0x0006BB89
			public override Func<Scale, Scale, bool> SameFunc { get; } = new Func<Scale, Scale, bool>(StylePropertyAnimationSystem.ValuesScale.IsSame);

			// Token: 0x06001C0F RID: 7183 RVA: 0x0006D991 File Offset: 0x0006BB91
			private static bool IsSame(Scale a, Scale b)
			{
				return a == b;
			}

			// Token: 0x06001C10 RID: 7184 RVA: 0x0006D99C File Offset: 0x0006BB9C
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001C11 RID: 7185 RVA: 0x0006DA10 File Offset: 0x0006BC10
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}

			// Token: 0x06001C12 RID: 7186 RVA: 0x0006DA66 File Offset: 0x0006BC66
			private static Scale Lerp(Scale a, Scale b, float t)
			{
				return new Scale(Vector3.LerpUnclamped(a.value, b.value, t));
			}

			// Token: 0x06001C13 RID: 7187 RVA: 0x0006DA84 File Offset: 0x0006BC84
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<Scale>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<Scale>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesScale.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}
		}

		// Token: 0x02000341 RID: 833
		private class ValuesRotate : StylePropertyAnimationSystem.Values<Rotate>
		{
			// Token: 0x1700069B RID: 1691
			// (get) Token: 0x06001C15 RID: 7189 RVA: 0x0006DB0E File Offset: 0x0006BD0E
			public override Func<Rotate, Rotate, bool> SameFunc { get; } = new Func<Rotate, Rotate, bool>(StylePropertyAnimationSystem.ValuesRotate.IsSame);

			// Token: 0x06001C16 RID: 7190 RVA: 0x0006DB16 File Offset: 0x0006BD16
			private static bool IsSame(Rotate a, Rotate b)
			{
				return a == b;
			}

			// Token: 0x06001C17 RID: 7191 RVA: 0x0006DB20 File Offset: 0x0006BD20
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001C18 RID: 7192 RVA: 0x0006DB94 File Offset: 0x0006BD94
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}

			// Token: 0x06001C19 RID: 7193 RVA: 0x0006DBEC File Offset: 0x0006BDEC
			private static Rotate Lerp(Rotate a, Rotate b, float t)
			{
				return new Rotate(Mathf.LerpUnclamped(a.angle.ToDegrees(), b.angle.ToDegrees(), t));
			}

			// Token: 0x06001C1A RID: 7194 RVA: 0x0006DC28 File Offset: 0x0006BE28
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<Rotate>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<Rotate>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesRotate.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}
		}

		// Token: 0x02000342 RID: 834
		private class ValuesTranslate : StylePropertyAnimationSystem.Values<Translate>
		{
			// Token: 0x1700069C RID: 1692
			// (get) Token: 0x06001C1C RID: 7196 RVA: 0x0006DCB2 File Offset: 0x0006BEB2
			public override Func<Translate, Translate, bool> SameFunc { get; } = new Func<Translate, Translate, bool>(StylePropertyAnimationSystem.ValuesTranslate.IsSame);

			// Token: 0x06001C1D RID: 7197 RVA: 0x0006DCBA File Offset: 0x0006BEBA
			private static bool IsSame(Translate a, Translate b)
			{
				return a == b;
			}

			// Token: 0x06001C1E RID: 7198 RVA: 0x0006DCC4 File Offset: 0x0006BEC4
			protected sealed override bool ConvertUnits(VisualElement owner, StylePropertyId prop, ref Translate a, ref Translate b)
			{
				return owner.TryConvertTranslateUnits(ref a, ref b);
			}

			// Token: 0x06001C1F RID: 7199 RVA: 0x0006DCE0 File Offset: 0x0006BEE0
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001C20 RID: 7200 RVA: 0x0006DD54 File Offset: 0x0006BF54
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}

			// Token: 0x06001C21 RID: 7201 RVA: 0x0006DDAC File Offset: 0x0006BFAC
			private static Translate Lerp(Translate a, Translate b, float t)
			{
				return new Translate(StylePropertyAnimationSystem.ValuesLength.Lerp(a.x, b.x, t), StylePropertyAnimationSystem.ValuesLength.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
			}

			// Token: 0x06001C22 RID: 7202 RVA: 0x0006DDFC File Offset: 0x0006BFFC
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<Translate>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<Translate>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesTranslate.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}
		}

		// Token: 0x02000343 RID: 835
		private class ValuesTransformOrigin : StylePropertyAnimationSystem.Values<TransformOrigin>
		{
			// Token: 0x1700069D RID: 1693
			// (get) Token: 0x06001C24 RID: 7204 RVA: 0x0006DE86 File Offset: 0x0006C086
			public override Func<TransformOrigin, TransformOrigin, bool> SameFunc { get; } = new Func<TransformOrigin, TransformOrigin, bool>(StylePropertyAnimationSystem.ValuesTransformOrigin.IsSame);

			// Token: 0x06001C25 RID: 7205 RVA: 0x0006DE8E File Offset: 0x0006C08E
			private static bool IsSame(TransformOrigin a, TransformOrigin b)
			{
				return a == b;
			}

			// Token: 0x06001C26 RID: 7206 RVA: 0x0006DE98 File Offset: 0x0006C098
			protected sealed override bool ConvertUnits(VisualElement owner, StylePropertyId prop, ref TransformOrigin a, ref TransformOrigin b)
			{
				return owner.TryConvertTransformOriginUnits(ref a, ref b);
			}

			// Token: 0x06001C27 RID: 7207 RVA: 0x0006DEB4 File Offset: 0x0006C0B4
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001C28 RID: 7208 RVA: 0x0006DF28 File Offset: 0x0006C128
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}

			// Token: 0x06001C29 RID: 7209 RVA: 0x0006DF80 File Offset: 0x0006C180
			private static TransformOrigin Lerp(TransformOrigin a, TransformOrigin b, float t)
			{
				return new TransformOrigin(StylePropertyAnimationSystem.ValuesLength.Lerp(a.x, b.x, t), StylePropertyAnimationSystem.ValuesLength.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
			}

			// Token: 0x06001C2A RID: 7210 RVA: 0x0006DFD0 File Offset: 0x0006C1D0
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<TransformOrigin>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<TransformOrigin>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesTransformOrigin.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}
		}

		// Token: 0x02000344 RID: 836
		private class ValuesBackgroundPosition : StylePropertyAnimationSystem.ValuesDiscrete<BackgroundPosition>
		{
			// Token: 0x06001C2C RID: 7212 RVA: 0x0006E05C File Offset: 0x0006C25C
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001C2D RID: 7213 RVA: 0x0006E0D0 File Offset: 0x0006C2D0
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x02000345 RID: 837
		private class ValuesBackgroundRepeat : StylePropertyAnimationSystem.ValuesDiscrete<BackgroundRepeat>
		{
			// Token: 0x06001C2F RID: 7215 RVA: 0x0006E130 File Offset: 0x0006C330
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001C30 RID: 7216 RVA: 0x0006E1A4 File Offset: 0x0006C3A4
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}
		}

		// Token: 0x02000346 RID: 838
		private class ValuesBackgroundSize : StylePropertyAnimationSystem.Values<BackgroundSize>
		{
			// Token: 0x1700069E RID: 1694
			// (get) Token: 0x06001C32 RID: 7218 RVA: 0x0006E203 File Offset: 0x0006C403
			public override Func<BackgroundSize, BackgroundSize, bool> SameFunc { get; } = new Func<BackgroundSize, BackgroundSize, bool>(StylePropertyAnimationSystem.ValuesBackgroundSize.IsSame);

			// Token: 0x06001C33 RID: 7219 RVA: 0x0006E20B File Offset: 0x0006C40B
			private static bool IsSame(BackgroundSize a, BackgroundSize b)
			{
				return a == b;
			}

			// Token: 0x06001C34 RID: 7220 RVA: 0x0006E214 File Offset: 0x0006C414
			protected sealed override bool ConvertUnits(VisualElement owner, StylePropertyId prop, ref BackgroundSize a, ref BackgroundSize b)
			{
				return owner.TryConvertBackgroundSizeUnits(ref a, ref b);
			}

			// Token: 0x06001C35 RID: 7221 RVA: 0x0006E230 File Offset: 0x0006C430
			protected sealed override void UpdateComputedStyle()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
				}
			}

			// Token: 0x06001C36 RID: 7222 RVA: 0x0006E2A4 File Offset: 0x0006C4A4
			protected sealed override void UpdateComputedStyle(int i)
			{
				this.running.elements[i].computedStyle.ApplyPropertyAnimation(this.running.elements[i], this.running.properties[i], this.running.style[i].currentValue);
			}

			// Token: 0x06001C37 RID: 7223 RVA: 0x0006E2FA File Offset: 0x0006C4FA
			private static BackgroundSize Lerp(BackgroundSize a, BackgroundSize b, float t)
			{
				return new BackgroundSize(StylePropertyAnimationSystem.ValuesLength.Lerp(a.x, b.x, t), StylePropertyAnimationSystem.ValuesLength.Lerp(a.y, b.y, t));
			}

			// Token: 0x06001C38 RID: 7224 RVA: 0x0006E32C File Offset: 0x0006C52C
			protected sealed override void UpdateValues()
			{
				int count = this.running.count;
				for (int i = 0; i < count; i++)
				{
					ref StylePropertyAnimationSystem.Values<BackgroundSize>.TimingData ptr = ref this.running.timing[i];
					ref StylePropertyAnimationSystem.Values<BackgroundSize>.StyleData ptr2 = ref this.running.style[i];
					ptr2.currentValue = StylePropertyAnimationSystem.ValuesBackgroundSize.Lerp(ptr2.startValue, ptr2.endValue, ptr.easedProgress);
				}
			}
		}
	}
}
