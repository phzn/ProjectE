using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x02000494 RID: 1172
	internal class StyleSheetBuilder
	{
		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x060024AD RID: 9389 RVA: 0x00099F74 File Offset: 0x00098174
		public StyleProperty currentProperty
		{
			get
			{
				return this.m_CurrentProperty;
			}
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x00099F7C File Offset: 0x0009817C
		public StyleRule BeginRule(int ruleLine)
		{
			StyleSheetBuilder.Log("Beginning rule");
			Debug.Assert(this.m_BuilderState == StyleSheetBuilder.BuilderState.Init);
			this.m_BuilderState = StyleSheetBuilder.BuilderState.Rule;
			this.m_CurrentRule = new StyleRule
			{
				line = ruleLine
			};
			return this.m_CurrentRule;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x00099FC8 File Offset: 0x000981C8
		public StyleSheetBuilder.ComplexSelectorScope BeginComplexSelector(int specificity)
		{
			StyleSheetBuilder.Log("Begin complex selector with specificity " + specificity.ToString());
			Debug.Assert(this.m_BuilderState == StyleSheetBuilder.BuilderState.Rule);
			this.m_BuilderState = StyleSheetBuilder.BuilderState.ComplexSelector;
			this.m_CurrentComplexSelector = new StyleComplexSelector();
			this.m_CurrentComplexSelector.specificity = specificity;
			this.m_CurrentComplexSelector.ruleIndex = this.m_Rules.Count;
			return new StyleSheetBuilder.ComplexSelectorScope(this);
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x0009A03C File Offset: 0x0009823C
		public void AddSimpleSelector(StyleSelectorPart[] parts, StyleSelectorRelationship previousRelationsip)
		{
			Debug.Assert(this.m_BuilderState == StyleSheetBuilder.BuilderState.ComplexSelector);
			StyleSelector styleSelector = new StyleSelector();
			styleSelector.parts = parts;
			styleSelector.previousRelationship = previousRelationsip;
			string text = "Add simple selector ";
			StyleSelector styleSelector2 = styleSelector;
			StyleSheetBuilder.Log(text + ((styleSelector2 != null) ? styleSelector2.ToString() : null));
			this.m_CurrentSelectors.Add(styleSelector);
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x0009A09C File Offset: 0x0009829C
		public void EndComplexSelector()
		{
			StyleSheetBuilder.Log("Ending complex selector");
			Debug.Assert(this.m_BuilderState == StyleSheetBuilder.BuilderState.ComplexSelector);
			this.m_BuilderState = StyleSheetBuilder.BuilderState.Rule;
			bool flag = this.m_CurrentSelectors.Count > 0;
			if (flag)
			{
				this.m_CurrentComplexSelector.selectors = this.m_CurrentSelectors.ToArray();
				this.m_ComplexSelectors.Add(this.m_CurrentComplexSelector);
				this.m_CurrentSelectors.Clear();
			}
			this.m_CurrentComplexSelector = null;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x0009A11C File Offset: 0x0009831C
		public StyleProperty BeginProperty(string name, int line = -1)
		{
			StyleSheetBuilder.Log("Begin property named " + name);
			Debug.Assert(this.m_BuilderState == StyleSheetBuilder.BuilderState.Rule);
			this.m_BuilderState = StyleSheetBuilder.BuilderState.Property;
			this.m_CurrentProperty = new StyleProperty
			{
				name = name,
				line = line
			};
			this.m_CurrentProperties.Add(this.m_CurrentProperty);
			return this.m_CurrentProperty;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x0009A188 File Offset: 0x00098388
		public void AddImport(StyleSheet.ImportStruct importStruct)
		{
			this.m_Imports.Add(importStruct);
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x0009A198 File Offset: 0x00098398
		public void AddValue(float value)
		{
			this.RegisterValue<float>(this.m_Floats, StyleValueType.Float, value);
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x0009A1AA File Offset: 0x000983AA
		public void AddValue(Dimension value)
		{
			this.RegisterValue<Dimension>(this.m_Dimensions, StyleValueType.Dimension, value);
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x0009A1BC File Offset: 0x000983BC
		public void AddValue(StyleValueKeyword keyword)
		{
			this.m_CurrentValues.Add(new StyleValueHandle((int)keyword, StyleValueType.Keyword));
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x0009A1D2 File Offset: 0x000983D2
		public void AddValue(StyleValueFunction function)
		{
			this.m_CurrentValues.Add(new StyleValueHandle((int)function, StyleValueType.Function));
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x0009A1E9 File Offset: 0x000983E9
		public void AddCommaSeparator()
		{
			this.m_CurrentValues.Add(new StyleValueHandle(0, StyleValueType.CommaSeparator));
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x0009A200 File Offset: 0x00098400
		public void AddValue(string value, StyleValueType type)
		{
			bool flag = type == StyleValueType.Variable;
			if (flag)
			{
				this.RegisterVariable(value);
			}
			else
			{
				this.RegisterValue<string>(this.m_Strings, type, value);
			}
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x0009A22F File Offset: 0x0009842F
		public void AddValue(Color value)
		{
			this.RegisterValue<Color>(this.m_Colors, StyleValueType.Color, value);
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x0009A241 File Offset: 0x00098441
		public void AddValue(Object value)
		{
			this.RegisterValue<Object>(this.m_Assets, StyleValueType.AssetReference, value);
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x0009A253 File Offset: 0x00098453
		public void AddValue(ScalableImage value)
		{
			this.RegisterValue<ScalableImage>(this.m_ScalableImages, StyleValueType.ScalableImage, value);
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0009A268 File Offset: 0x00098468
		public void EndProperty()
		{
			StyleSheetBuilder.Log("Ending property");
			Debug.Assert(this.m_BuilderState == StyleSheetBuilder.BuilderState.Property);
			this.m_BuilderState = StyleSheetBuilder.BuilderState.Rule;
			this.m_CurrentProperty.values = this.m_CurrentValues.ToArray();
			this.m_CurrentProperty = null;
			this.m_CurrentValues.Clear();
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x0009A2C4 File Offset: 0x000984C4
		public int EndRule()
		{
			StyleSheetBuilder.Log("Ending rule");
			Debug.Assert(this.m_BuilderState == StyleSheetBuilder.BuilderState.Rule);
			this.m_BuilderState = StyleSheetBuilder.BuilderState.Init;
			this.m_CurrentRule.properties = this.m_CurrentProperties.ToArray();
			this.m_Rules.Add(this.m_CurrentRule);
			this.m_CurrentRule = null;
			this.m_CurrentProperties.Clear();
			return this.m_Rules.Count - 1;
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x0009A340 File Offset: 0x00098540
		public void BuildTo(StyleSheet writeTo)
		{
			Debug.Assert(this.m_BuilderState == StyleSheetBuilder.BuilderState.Init);
			writeTo.floats = this.m_Floats.ToArray();
			writeTo.dimensions = this.m_Dimensions.ToArray();
			writeTo.colors = this.m_Colors.ToArray();
			writeTo.strings = this.m_Strings.ToArray();
			writeTo.rules = this.m_Rules.ToArray();
			writeTo.assets = this.m_Assets.ToArray();
			writeTo.scalableImages = this.m_ScalableImages.ToArray();
			writeTo.complexSelectors = this.m_ComplexSelectors.ToArray();
			writeTo.imports = this.m_Imports.ToArray();
			bool flag = writeTo.imports.Length != 0;
			if (flag)
			{
				writeTo.FlattenImportedStyleSheetsRecursive();
			}
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x0009A410 File Offset: 0x00098610
		private void RegisterVariable(string value)
		{
			StyleSheetBuilder.Log("Add variable : " + value);
			Debug.Assert(this.m_BuilderState == StyleSheetBuilder.BuilderState.Property);
			int num = this.m_Strings.IndexOf(value);
			bool flag = num < 0;
			if (flag)
			{
				this.m_Strings.Add(value);
				num = this.m_Strings.Count - 1;
			}
			this.m_CurrentValues.Add(new StyleValueHandle(num, StyleValueType.Variable));
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x0009A484 File Offset: 0x00098684
		private void RegisterValue<T>(List<T> list, StyleValueType type, T value)
		{
			string text = "Add value of type ";
			string text2 = type.ToString();
			string text3 = " : ";
			T t = value;
			StyleSheetBuilder.Log(text + text2 + text3 + ((t != null) ? t.ToString() : null));
			Debug.Assert(this.m_BuilderState == StyleSheetBuilder.BuilderState.Property);
			list.Add(value);
			this.m_CurrentValues.Add(new StyleValueHandle(list.Count - 1, type));
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x00003CD2 File Offset: 0x00001ED2
		private static void Log(string msg)
		{
		}

		// Token: 0x04001198 RID: 4504
		private StyleSheetBuilder.BuilderState m_BuilderState;

		// Token: 0x04001199 RID: 4505
		private List<float> m_Floats = new List<float>();

		// Token: 0x0400119A RID: 4506
		private List<Dimension> m_Dimensions = new List<Dimension>();

		// Token: 0x0400119B RID: 4507
		private List<Color> m_Colors = new List<Color>();

		// Token: 0x0400119C RID: 4508
		private List<string> m_Strings = new List<string>();

		// Token: 0x0400119D RID: 4509
		private List<StyleRule> m_Rules = new List<StyleRule>();

		// Token: 0x0400119E RID: 4510
		private List<Object> m_Assets = new List<Object>();

		// Token: 0x0400119F RID: 4511
		private List<ScalableImage> m_ScalableImages = new List<ScalableImage>();

		// Token: 0x040011A0 RID: 4512
		private List<StyleComplexSelector> m_ComplexSelectors = new List<StyleComplexSelector>();

		// Token: 0x040011A1 RID: 4513
		private List<StyleProperty> m_CurrentProperties = new List<StyleProperty>();

		// Token: 0x040011A2 RID: 4514
		private List<StyleValueHandle> m_CurrentValues = new List<StyleValueHandle>();

		// Token: 0x040011A3 RID: 4515
		private StyleComplexSelector m_CurrentComplexSelector;

		// Token: 0x040011A4 RID: 4516
		private List<StyleSelector> m_CurrentSelectors = new List<StyleSelector>();

		// Token: 0x040011A5 RID: 4517
		private StyleProperty m_CurrentProperty;

		// Token: 0x040011A6 RID: 4518
		private StyleRule m_CurrentRule;

		// Token: 0x040011A7 RID: 4519
		private List<StyleSheet.ImportStruct> m_Imports = new List<StyleSheet.ImportStruct>();

		// Token: 0x02000495 RID: 1173
		public struct ComplexSelectorScope : IDisposable
		{
			// Token: 0x060024C4 RID: 9412 RVA: 0x0009A598 File Offset: 0x00098798
			public ComplexSelectorScope(StyleSheetBuilder builder)
			{
				this.m_Builder = builder;
			}

			// Token: 0x060024C5 RID: 9413 RVA: 0x0009A5A2 File Offset: 0x000987A2
			public void Dispose()
			{
				this.m_Builder.EndComplexSelector();
			}

			// Token: 0x040011A8 RID: 4520
			private StyleSheetBuilder m_Builder;
		}

		// Token: 0x02000496 RID: 1174
		private enum BuilderState
		{
			// Token: 0x040011AA RID: 4522
			Init,
			// Token: 0x040011AB RID: 4523
			Rule,
			// Token: 0x040011AC RID: 4524
			ComplexSelector,
			// Token: 0x040011AD RID: 4525
			Property
		}
	}
}
