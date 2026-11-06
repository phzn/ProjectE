using System;
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;
using UnityEngine.UIElements.StyleSheets.Syntax;

namespace UnityEngine.UIElements
{
	// Token: 0x02000361 RID: 865
	internal class StyleVariableResolver
	{
		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06001CD0 RID: 7376 RVA: 0x0006FE39 File Offset: 0x0006E039
		private StyleSheet currentSheet
		{
			get
			{
				return this.m_CurrentContext.sheet;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x0006FE46 File Offset: 0x0006E046
		private StyleValueHandle[] currentHandles
		{
			get
			{
				return this.m_CurrentContext.handles;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001CD2 RID: 7378 RVA: 0x0006FE53 File Offset: 0x0006E053
		public List<StylePropertyValue> resolvedValues
		{
			get
			{
				return this.m_ResolvedValues;
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x0006FE5B File Offset: 0x0006E05B
		// (set) Token: 0x06001CD4 RID: 7380 RVA: 0x0006FE63 File Offset: 0x0006E063
		public StyleVariableContext variableContext { get; set; }

		// Token: 0x06001CD5 RID: 7381 RVA: 0x0006FE6C File Offset: 0x0006E06C
		public void Init(StyleProperty property, StyleSheet sheet, StyleValueHandle[] handles)
		{
			this.m_ResolvedValues.Clear();
			this.m_ContextStack.Clear();
			this.m_Property = property;
			this.PushContext(sheet, handles);
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x0006FE98 File Offset: 0x0006E098
		private void PushContext(StyleSheet sheet, StyleValueHandle[] handles)
		{
			this.m_CurrentContext = new StyleVariableResolver.ResolveContext
			{
				sheet = sheet,
				handles = handles
			};
			this.m_ContextStack.Push(this.m_CurrentContext);
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x0006FED7 File Offset: 0x0006E0D7
		private void PopContext()
		{
			this.m_ContextStack.Pop();
			this.m_CurrentContext = this.m_ContextStack.Peek();
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x0006FEF8 File Offset: 0x0006E0F8
		public void AddValue(StyleValueHandle handle)
		{
			this.m_ResolvedValues.Add(new StylePropertyValue
			{
				sheet = this.currentSheet,
				handle = handle
			});
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x0006FF30 File Offset: 0x0006E130
		public bool ResolveVarFunction(ref int index)
		{
			this.m_ResolvedVarStack.Clear();
			int num;
			string text;
			StyleVariableResolver.ParseVarFunction(this.currentSheet, this.currentHandles, ref index, out num, out text);
			StyleVariableResolver.Result result = this.ResolveVarFunction(ref index, num, text);
			return result == StyleVariableResolver.Result.Valid;
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x0006FF74 File Offset: 0x0006E174
		private StyleVariableResolver.Result ResolveVarFunction(ref int index, int argc, string varName)
		{
			StyleVariableResolver.Result result = this.ResolveVariable(varName);
			bool flag = result == StyleVariableResolver.Result.NotFound && argc > 1;
			if (flag)
			{
				StyleValueHandle[] currentHandles = this.currentHandles;
				int num = index + 1;
				index = num;
				StyleValueHandle styleValueHandle = currentHandles[num];
				Debug.Assert(styleValueHandle.valueType == StyleValueType.CommaSeparator, string.Format("Unexpected value type {0} in var function", styleValueHandle.valueType));
				bool flag2 = styleValueHandle.valueType == StyleValueType.CommaSeparator && index + 1 < this.currentHandles.Length;
				if (flag2)
				{
					index++;
					result = this.ResolveFallback(ref index);
				}
			}
			return result;
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x00070010 File Offset: 0x0006E210
		public bool ValidateResolvedValues()
		{
			bool isCustomProperty = this.m_Property.isCustomProperty;
			bool flag;
			if (isCustomProperty)
			{
				flag = true;
			}
			else
			{
				string text;
				bool flag2 = !StylePropertyCache.TryGetSyntax(this.m_Property.name, out text);
				if (flag2)
				{
					Debug.LogAssertion("Unknown style property " + this.m_Property.name);
					flag = false;
				}
				else
				{
					Expression expression = StyleVariableResolver.s_SyntaxParser.Parse(text);
					flag = this.m_Matcher.Match(expression, this.m_ResolvedValues).success;
				}
			}
			return flag;
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x0007009C File Offset: 0x0006E29C
		private StyleVariableResolver.Result ResolveVariable(string variableName)
		{
			StyleVariable styleVariable;
			bool flag = !this.variableContext.TryFindVariable(variableName, out styleVariable);
			StyleVariableResolver.Result result;
			if (flag)
			{
				result = StyleVariableResolver.Result.NotFound;
			}
			else
			{
				bool flag2 = this.m_ResolvedVarStack.Contains(styleVariable.name);
				if (flag2)
				{
					result = StyleVariableResolver.Result.NotFound;
				}
				else
				{
					this.m_ResolvedVarStack.Push(styleVariable.name);
					StyleVariableResolver.Result result2 = StyleVariableResolver.Result.Valid;
					int num = 0;
					while (num < styleVariable.handles.Length && result2 == StyleVariableResolver.Result.Valid)
					{
						bool flag3 = this.m_ResolvedValues.Count + 1 > 100;
						if (flag3)
						{
							return StyleVariableResolver.Result.Invalid;
						}
						StyleValueHandle styleValueHandle = styleVariable.handles[num];
						bool flag4 = styleValueHandle.IsVarFunction();
						if (flag4)
						{
							this.PushContext(styleVariable.sheet, styleVariable.handles);
							int num2;
							string text;
							StyleVariableResolver.ParseVarFunction(styleVariable.sheet, styleVariable.handles, ref num, out num2, out text);
							result2 = this.ResolveVarFunction(ref num, num2, text);
							this.PopContext();
						}
						else
						{
							this.m_ResolvedValues.Add(new StylePropertyValue
							{
								sheet = styleVariable.sheet,
								handle = styleValueHandle
							});
						}
						num++;
					}
					this.m_ResolvedVarStack.Pop();
					result = result2;
				}
			}
			return result;
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000701E4 File Offset: 0x0006E3E4
		private StyleVariableResolver.Result ResolveFallback(ref int index)
		{
			StyleVariableResolver.Result result = StyleVariableResolver.Result.Valid;
			while (index < this.currentHandles.Length && result == StyleVariableResolver.Result.Valid)
			{
				StyleValueHandle styleValueHandle = this.currentHandles[index];
				bool flag = styleValueHandle.IsVarFunction();
				if (flag)
				{
					int num;
					string text;
					StyleVariableResolver.ParseVarFunction(this.currentSheet, this.currentHandles, ref index, out num, out text);
					result = this.ResolveVariable(text);
					bool flag2 = result == StyleVariableResolver.Result.NotFound;
					if (flag2)
					{
						bool flag3 = num > 1;
						if (flag3)
						{
							StyleValueHandle[] currentHandles = this.currentHandles;
							int num2 = index + 1;
							index = num2;
							styleValueHandle = currentHandles[num2];
							Debug.Assert(styleValueHandle.valueType == StyleValueType.CommaSeparator, string.Format("Unexpected value type {0} in var function", styleValueHandle.valueType));
							bool flag4 = styleValueHandle.valueType == StyleValueType.CommaSeparator && index + 1 < this.currentHandles.Length;
							if (flag4)
							{
								index++;
								result = this.ResolveFallback(ref index);
							}
						}
					}
				}
				else
				{
					this.m_ResolvedValues.Add(new StylePropertyValue
					{
						sheet = this.currentSheet,
						handle = styleValueHandle
					});
				}
				index++;
			}
			return result;
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x0007031C File Offset: 0x0006E51C
		private static void ParseVarFunction(StyleSheet sheet, StyleValueHandle[] handles, ref int index, out int argCount, out string variableName)
		{
			int num = index + 1;
			index = num;
			argCount = (int)sheet.ReadFloat(handles[num]);
			num = index + 1;
			index = num;
			variableName = sheet.ReadVariable(handles[num]);
		}

		// Token: 0x04000C1E RID: 3102
		internal const int kMaxResolves = 100;

		// Token: 0x04000C1F RID: 3103
		private static StyleSyntaxParser s_SyntaxParser = new StyleSyntaxParser();

		// Token: 0x04000C20 RID: 3104
		private StylePropertyValueMatcher m_Matcher = new StylePropertyValueMatcher();

		// Token: 0x04000C21 RID: 3105
		private List<StylePropertyValue> m_ResolvedValues = new List<StylePropertyValue>();

		// Token: 0x04000C22 RID: 3106
		private Stack<string> m_ResolvedVarStack = new Stack<string>();

		// Token: 0x04000C23 RID: 3107
		private StyleProperty m_Property;

		// Token: 0x04000C24 RID: 3108
		private Stack<StyleVariableResolver.ResolveContext> m_ContextStack = new Stack<StyleVariableResolver.ResolveContext>();

		// Token: 0x04000C25 RID: 3109
		private StyleVariableResolver.ResolveContext m_CurrentContext;

		// Token: 0x02000362 RID: 866
		private enum Result
		{
			// Token: 0x04000C28 RID: 3112
			Valid,
			// Token: 0x04000C29 RID: 3113
			Invalid,
			// Token: 0x04000C2A RID: 3114
			NotFound
		}

		// Token: 0x02000363 RID: 867
		private struct ResolveContext
		{
			// Token: 0x04000C2B RID: 3115
			public StyleSheet sheet;

			// Token: 0x04000C2C RID: 3116
			public StyleValueHandle[] handles;
		}
	}
}
