using System;
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x02000417 RID: 1047
	internal class VisualTreeStyleUpdaterTraversal : HierarchyTraversal
	{
		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002160 RID: 8544 RVA: 0x0007E3C6 File Offset: 0x0007C5C6
		// (set) Token: 0x06002161 RID: 8545 RVA: 0x0007E3CE File Offset: 0x0007C5CE
		private float currentPixelsPerPoint { get; set; } = 1f;

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002162 RID: 8546 RVA: 0x0007E3D7 File Offset: 0x0007C5D7
		public StyleMatchingContext styleMatchingContext
		{
			get
			{
				return this.m_StyleMatchingContext;
			}
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x0007E3DF File Offset: 0x0007C5DF
		public void PrepareTraversal(float pixelsPerPoint)
		{
			this.currentPixelsPerPoint = pixelsPerPoint;
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x0007E3EC File Offset: 0x0007C5EC
		public void AddChangedElement(VisualElement ve, VersionChangeType versionChangeType)
		{
			this.m_UpdateList.Add(ve);
			bool flag = (versionChangeType & VersionChangeType.StyleSheet) == VersionChangeType.StyleSheet;
			if (flag)
			{
				this.PropagateToChildren(ve);
			}
			this.PropagateToParents(ve);
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x0007E423 File Offset: 0x0007C623
		public void Clear()
		{
			this.m_UpdateList.Clear();
			this.m_ParentList.Clear();
			this.m_TempMatchResults.Clear();
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0007E44C File Offset: 0x0007C64C
		private void PropagateToChildren(VisualElement ve)
		{
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				VisualElement visualElement = ve.hierarchy[i];
				bool flag = this.m_UpdateList.Add(visualElement);
				bool flag2 = flag;
				if (flag2)
				{
					this.PropagateToChildren(visualElement);
				}
			}
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x0007E4AC File Offset: 0x0007C6AC
		private void PropagateToParents(VisualElement ve)
		{
			for (VisualElement visualElement = ve.hierarchy.parent; visualElement != null; visualElement = visualElement.hierarchy.parent)
			{
				bool flag = !this.m_ParentList.Add(visualElement);
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x0007E4FA File Offset: 0x0007C6FA
		private static void OnProcessMatchResult(VisualElement current, MatchResultInfo info)
		{
			current.triggerPseudoMask |= info.triggerPseudoMask;
			current.dependencyPseudoMask |= info.dependencyPseudoMask;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x0007E524 File Offset: 0x0007C724
		public override void TraverseRecursive(VisualElement element, int depth)
		{
			bool flag = this.ShouldSkipElement(element);
			if (!flag)
			{
				bool flag2 = this.m_UpdateList.Contains(element);
				bool flag3 = flag2;
				if (flag3)
				{
					element.triggerPseudoMask = (PseudoStates)0;
					element.dependencyPseudoMask = (PseudoStates)0;
				}
				int styleSheetCount = this.m_StyleMatchingContext.styleSheetCount;
				bool flag4 = element.styleSheetList != null;
				if (flag4)
				{
					for (int i = 0; i < element.styleSheetList.Count; i++)
					{
						StyleSheet styleSheet = element.styleSheetList[i];
						bool flag5 = styleSheet.flattenedRecursiveImports != null;
						if (flag5)
						{
							for (int j = 0; j < styleSheet.flattenedRecursiveImports.Count; j++)
							{
								this.m_StyleMatchingContext.AddStyleSheet(styleSheet.flattenedRecursiveImports[j]);
							}
						}
						this.m_StyleMatchingContext.AddStyleSheet(styleSheet);
					}
				}
				StyleVariableContext variableContext = this.m_StyleMatchingContext.variableContext;
				int customPropertiesCount = element.computedStyle.customPropertiesCount;
				bool flag6 = flag2;
				if (flag6)
				{
					this.m_StyleMatchingContext.currentElement = element;
					StyleSelectorHelper.FindMatches(this.m_StyleMatchingContext, this.m_TempMatchResults, styleSheetCount - 1);
					ComputedStyle computedStyle = this.ProcessMatchedRules(element, this.m_TempMatchResults);
					computedStyle.Acquire();
					bool hasInlineStyle = element.hasInlineStyle;
					if (hasInlineStyle)
					{
						element.inlineStyleAccess.ApplyInlineStyles(ref computedStyle);
					}
					ComputedTransitionUtils.UpdateComputedTransitions(ref computedStyle);
					bool flag7 = element.hasRunningAnimations && !ComputedTransitionUtils.SameTransitionProperty(element.computedStyle, ref computedStyle);
					if (flag7)
					{
						this.CancelAnimationsWithNoTransitionProperty(element, ref computedStyle);
					}
					bool flag8 = computedStyle.hasTransition && element.styleInitialized;
					if (flag8)
					{
						this.ProcessTransitions(element, element.computedStyle, ref computedStyle);
						element.SetComputedStyle(ref computedStyle);
						this.ForceUpdateTransitions(element);
					}
					else
					{
						element.SetComputedStyle(ref computedStyle);
					}
					computedStyle.Release();
					element.styleInitialized = true;
					element.inheritedStylesHash = element.computedStyle.inheritedData.GetHashCode();
					this.m_StyleMatchingContext.currentElement = null;
					this.m_TempMatchResults.Clear();
				}
				else
				{
					this.m_StyleMatchingContext.variableContext = element.variableContext;
				}
				bool flag9 = flag2 && (customPropertiesCount > 0 || element.computedStyle.customPropertiesCount > 0) && element.HasEventCallbacksOrDefaultActions(EventBase<CustomStyleResolvedEvent>.EventCategory);
				if (flag9)
				{
					using (CustomStyleResolvedEvent pooled = EventBase<CustomStyleResolvedEvent>.GetPooled())
					{
						pooled.target = element;
						element.HandleEventAtTargetAndDefaultPhase(pooled);
					}
				}
				this.m_StyleMatchingContext.ancestorFilter.PushElement(element);
				base.Recurse(element, depth);
				this.m_StyleMatchingContext.ancestorFilter.PopElement();
				this.m_StyleMatchingContext.variableContext = variableContext;
				bool flag10 = this.m_StyleMatchingContext.styleSheetCount > styleSheetCount;
				if (flag10)
				{
					this.m_StyleMatchingContext.RemoveStyleSheetRange(styleSheetCount, this.m_StyleMatchingContext.styleSheetCount - styleSheetCount);
				}
			}
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x0007E834 File Offset: 0x0007CA34
		private void ProcessTransitions(VisualElement element, ref ComputedStyle oldStyle, ref ComputedStyle newStyle)
		{
			for (int i = newStyle.computedTransitions.Length - 1; i >= 0; i--)
			{
				ComputedTransitionProperty computedTransitionProperty = newStyle.computedTransitions[i];
				bool flag = element.hasInlineStyle && element.inlineStyleAccess.IsValueSet(computedTransitionProperty.id);
				if (!flag)
				{
					ComputedStyle.StartAnimation(element, computedTransitionProperty.id, ref oldStyle, ref newStyle, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
				}
			}
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x0007E8B0 File Offset: 0x0007CAB0
		private void ForceUpdateTransitions(VisualElement element)
		{
			element.styleAnimation.GetAllAnimations(this.m_AnimatedProperties);
			bool flag = this.m_AnimatedProperties.Count > 0;
			if (flag)
			{
				foreach (StylePropertyId stylePropertyId in this.m_AnimatedProperties)
				{
					element.styleAnimation.UpdateAnimation(stylePropertyId);
				}
				this.m_AnimatedProperties.Clear();
			}
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x0007E940 File Offset: 0x0007CB40
		internal void CancelAnimationsWithNoTransitionProperty(VisualElement element, ref ComputedStyle newStyle)
		{
			element.styleAnimation.GetAllAnimations(this.m_AnimatedProperties);
			foreach (StylePropertyId stylePropertyId in this.m_AnimatedProperties)
			{
				bool flag = !(ref newStyle).HasTransitionProperty(stylePropertyId);
				if (flag)
				{
					element.styleAnimation.CancelAnimation(stylePropertyId);
				}
			}
			this.m_AnimatedProperties.Clear();
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x0007E9CC File Offset: 0x0007CBCC
		protected bool ShouldSkipElement(VisualElement element)
		{
			return !this.m_ParentList.Contains(element) && !this.m_UpdateList.Contains(element);
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x0007EA00 File Offset: 0x0007CC00
		private ComputedStyle ProcessMatchedRules(VisualElement element, List<SelectorMatchRecord> matchingSelectors)
		{
			matchingSelectors.Sort((SelectorMatchRecord a, SelectorMatchRecord b) => SelectorMatchRecord.Compare(a, b));
			long num = (long)element.fullTypeName.GetHashCode();
			num = (num * 397L) ^ (long)this.currentPixelsPerPoint.GetHashCode();
			int variableHash = this.m_StyleMatchingContext.variableContext.GetVariableHash();
			int num2 = 0;
			foreach (SelectorMatchRecord selectorMatchRecord in matchingSelectors)
			{
				num2 += selectorMatchRecord.complexSelector.rule.customPropertiesCount;
			}
			bool flag = num2 > 0;
			if (flag)
			{
				this.m_ProcessVarContext.AddInitialRange(this.m_StyleMatchingContext.variableContext);
			}
			foreach (SelectorMatchRecord selectorMatchRecord2 in matchingSelectors)
			{
				StyleSheet sheet = selectorMatchRecord2.sheet;
				int ruleIndex = selectorMatchRecord2.complexSelector.ruleIndex;
				int specificity = selectorMatchRecord2.complexSelector.specificity;
				num = (num * 397L) ^ (long)sheet.contentHash;
				num = (num * 397L) ^ (long)ruleIndex;
				num = (num * 397L) ^ (long)specificity;
				StyleRule rule = selectorMatchRecord2.complexSelector.rule;
				bool flag2 = rule.customPropertiesCount > 0;
				if (flag2)
				{
					this.ProcessMatchedVariables(selectorMatchRecord2.sheet, rule);
				}
			}
			VisualElement parent = element.hierarchy.parent;
			int num3 = ((parent != null) ? parent.inheritedStylesHash : 0);
			num = (num * 397L) ^ (long)num3;
			int num4 = variableHash;
			bool flag3 = num2 > 0;
			if (flag3)
			{
				num4 = this.m_ProcessVarContext.GetVariableHash();
			}
			num = (num * 397L) ^ (long)num4;
			bool flag4 = variableHash != num4;
			if (flag4)
			{
				StyleVariableContext styleVariableContext;
				bool flag5 = !StyleCache.TryGetValue(num4, out styleVariableContext);
				if (flag5)
				{
					styleVariableContext = new StyleVariableContext(this.m_ProcessVarContext);
					StyleCache.SetValue(num4, styleVariableContext);
				}
				this.m_StyleMatchingContext.variableContext = styleVariableContext;
			}
			element.variableContext = this.m_StyleMatchingContext.variableContext;
			this.m_ProcessVarContext.Clear();
			ComputedStyle computedStyle;
			bool flag6 = !StyleCache.TryGetValue(num, out computedStyle);
			if (flag6)
			{
				ref ComputedStyle ptr;
				if (parent != null)
				{
					ref ComputedStyle computedStyle2 = ref parent.computedStyle;
					ptr = parent.computedStyle;
				}
				else
				{
					ptr = InitialStyle.Get();
				}
				ref ComputedStyle ptr2 = ref ptr;
				computedStyle = ComputedStyle.Create(ref ptr2);
				computedStyle.matchingRulesHash = num;
				float scaledPixelsPerPoint = element.scaledPixelsPerPoint;
				foreach (SelectorMatchRecord selectorMatchRecord3 in matchingSelectors)
				{
					this.m_StylePropertyReader.SetContext(selectorMatchRecord3.sheet, selectorMatchRecord3.complexSelector, this.m_StyleMatchingContext.variableContext, scaledPixelsPerPoint);
					computedStyle.ApplyProperties(this.m_StylePropertyReader, ref ptr2);
				}
				computedStyle.FinalizeApply(ref ptr2);
				StyleCache.SetValue(num, ref computedStyle);
			}
			return computedStyle;
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x0007ED3C File Offset: 0x0007CF3C
		private void ProcessMatchedVariables(StyleSheet sheet, StyleRule rule)
		{
			foreach (StyleProperty styleProperty in rule.properties)
			{
				bool isCustomProperty = styleProperty.isCustomProperty;
				if (isCustomProperty)
				{
					StyleVariable styleVariable = new StyleVariable(styleProperty.name, sheet, styleProperty.values);
					this.m_ProcessVarContext.Add(styleVariable);
				}
			}
		}

		// Token: 0x04000E2F RID: 3631
		private StyleVariableContext m_ProcessVarContext = new StyleVariableContext();

		// Token: 0x04000E30 RID: 3632
		private HashSet<VisualElement> m_UpdateList = new HashSet<VisualElement>();

		// Token: 0x04000E31 RID: 3633
		private HashSet<VisualElement> m_ParentList = new HashSet<VisualElement>();

		// Token: 0x04000E32 RID: 3634
		private List<SelectorMatchRecord> m_TempMatchResults = new List<SelectorMatchRecord>();

		// Token: 0x04000E34 RID: 3636
		private StyleMatchingContext m_StyleMatchingContext = new StyleMatchingContext(new Action<VisualElement, MatchResultInfo>(VisualTreeStyleUpdaterTraversal.OnProcessMatchResult));

		// Token: 0x04000E35 RID: 3637
		private StylePropertyReader m_StylePropertyReader = new StylePropertyReader();

		// Token: 0x04000E36 RID: 3638
		private readonly List<StylePropertyId> m_AnimatedProperties = new List<StylePropertyId>();
	}
}
