using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200001F RID: 31
	public static class InputActionRebindingExtensions
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x00005E28 File Offset: 0x00004028
		public static PrimitiveValue? GetParameterValue(this InputAction action, string name, InputBinding bindingMask = default(InputBinding))
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			return action.GetParameterValue(new InputActionRebindingExtensions.ParameterOverride(name, bindingMask, default(PrimitiveValue)));
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00005E6C File Offset: 0x0000406C
		private static PrimitiveValue? GetParameterValue(this InputAction action, InputActionRebindingExtensions.ParameterOverride parameterOverride)
		{
			parameterOverride.bindingMask.action = action.name;
			InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
			orCreateActionMap.ResolveBindingsIfNecessary();
			using (InputActionRebindingExtensions.ParameterEnumerator enumerator = new InputActionRebindingExtensions.ParameterEnumerable(orCreateActionMap.m_State, parameterOverride, orCreateActionMap.m_MapIndexInState).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					InputActionRebindingExtensions.Parameter parameter = enumerator.Current;
					return new PrimitiveValue?(PrimitiveValue.FromObject(parameter.field.GetValue(parameter.instance)));
				}
			}
			return null;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00005F14 File Offset: 0x00004114
		public static PrimitiveValue? GetParameterValue(this InputAction action, string name, int bindingIndex)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			if (bindingIndex < 0)
			{
				throw new ArgumentOutOfRangeException("bindingIndex");
			}
			int num = action.BindingIndexOnActionToBindingIndexOnMap(bindingIndex);
			InputBinding inputBinding = new InputBinding
			{
				id = action.GetOrCreateActionMap().bindings[num].id
			};
			return action.GetParameterValue(name, inputBinding);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00005F90 File Offset: 0x00004190
		public unsafe static TValue? GetParameterValue<TObject, TValue>(this InputAction action, Expression<Func<TObject, TValue>> expr, InputBinding bindingMask = default(InputBinding)) where TValue : struct
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (expr == null)
			{
				throw new ArgumentNullException("expr");
			}
			InputActionRebindingExtensions.ParameterOverride parameterOverride = InputActionRebindingExtensions.ExtractParameterOverride<TObject, TValue>(expr, bindingMask, default(PrimitiveValue));
			PrimitiveValue? parameterValue = action.GetParameterValue(parameterOverride);
			if (parameterValue == null)
			{
				return null;
			}
			if (Type.GetTypeCode(typeof(TValue)) == parameterValue.Value.type)
			{
				PrimitiveValue value = parameterValue.Value;
				TValue tvalue = default(TValue);
				UnsafeUtility.MemCpy(UnsafeUtility.AddressOf<TValue>(ref tvalue), (void*)value.valuePtr, (long)UnsafeUtility.SizeOf<TValue>());
				return new TValue?(tvalue);
			}
			return new TValue?((TValue)((object)Convert.ChangeType(parameterValue.Value.ToObject(), typeof(TValue))));
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00006060 File Offset: 0x00004260
		public static void ApplyParameterOverride<TObject, TValue>(this InputAction action, Expression<Func<TObject, TValue>> expr, TValue value, InputBinding bindingMask = default(InputBinding)) where TValue : struct
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (expr == null)
			{
				throw new ArgumentNullException("expr");
			}
			InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
			orCreateActionMap.ResolveBindingsIfNecessary();
			bindingMask.action = action.name;
			InputActionRebindingExtensions.ParameterOverride parameterOverride = InputActionRebindingExtensions.ExtractParameterOverride<TObject, TValue>(expr, bindingMask, PrimitiveValue.From<TValue>(value));
			InputActionRebindingExtensions.ApplyParameterOverride(orCreateActionMap.m_State, orCreateActionMap.m_MapIndexInState, ref orCreateActionMap.m_ParameterOverrides, ref orCreateActionMap.m_ParameterOverridesCount, parameterOverride);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000060D0 File Offset: 0x000042D0
		public static void ApplyParameterOverride<TObject, TValue>(this InputActionMap actionMap, Expression<Func<TObject, TValue>> expr, TValue value, InputBinding bindingMask = default(InputBinding)) where TValue : struct
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			if (expr == null)
			{
				throw new ArgumentNullException("expr");
			}
			actionMap.ResolveBindingsIfNecessary();
			InputActionRebindingExtensions.ParameterOverride parameterOverride = InputActionRebindingExtensions.ExtractParameterOverride<TObject, TValue>(expr, bindingMask, PrimitiveValue.From<TValue>(value));
			InputActionRebindingExtensions.ApplyParameterOverride(actionMap.m_State, actionMap.m_MapIndexInState, ref actionMap.m_ParameterOverrides, ref actionMap.m_ParameterOverridesCount, parameterOverride);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000612C File Offset: 0x0000432C
		public static void ApplyParameterOverride<TObject, TValue>(this InputActionAsset asset, Expression<Func<TObject, TValue>> expr, TValue value, InputBinding bindingMask = default(InputBinding)) where TValue : struct
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (expr == null)
			{
				throw new ArgumentNullException("expr");
			}
			asset.ResolveBindingsIfNecessary();
			InputActionRebindingExtensions.ParameterOverride parameterOverride = InputActionRebindingExtensions.ExtractParameterOverride<TObject, TValue>(expr, bindingMask, PrimitiveValue.From<TValue>(value));
			InputActionRebindingExtensions.ApplyParameterOverride(asset.m_SharedStateForAllMaps, -1, ref asset.m_ParameterOverrides, ref asset.m_ParameterOverridesCount, parameterOverride);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00006188 File Offset: 0x00004388
		private static InputActionRebindingExtensions.ParameterOverride ExtractParameterOverride<TObject, TValue>(Expression<Func<TObject, TValue>> expr, InputBinding bindingMask = default(InputBinding), PrimitiveValue value = default(PrimitiveValue))
		{
			if (expr == null)
			{
				throw new ArgumentException("Expression must be a LambdaExpression but was a " + expr.GetType().Name + " instead", "expr");
			}
			MemberExpression memberExpression = expr.Body as MemberExpression;
			if (memberExpression == null)
			{
				UnaryExpression unaryExpression = expr.Body as UnaryExpression;
				if (unaryExpression != null && unaryExpression.NodeType == ExpressionType.Convert)
				{
					MemberExpression memberExpression2 = unaryExpression.Operand as MemberExpression;
					if (memberExpression2 != null)
					{
						memberExpression = memberExpression2;
						goto IL_008D;
					}
				}
				throw new ArgumentException("Body in LambdaExpression must be a MemberExpression (x.name) but was a " + expr.GetType().Name + " instead", "expr");
			}
			IL_008D:
			string text;
			if (typeof(InputProcessor).IsAssignableFrom(typeof(TObject)))
			{
				text = InputProcessor.s_Processors.FindNameForType(typeof(TObject));
			}
			else if (typeof(IInputInteraction).IsAssignableFrom(typeof(TObject)))
			{
				text = InputInteraction.s_Interactions.FindNameForType(typeof(TObject));
			}
			else
			{
				if (!typeof(InputBindingComposite).IsAssignableFrom(typeof(TObject)))
				{
					throw new ArgumentException("Given type must be an InputProcessor, IInputInteraction, or InputBindingComposite (was " + typeof(TObject).Name + ")", "TObject");
				}
				text = InputBindingComposite.s_Composites.FindNameForType(typeof(TObject));
			}
			return new InputActionRebindingExtensions.ParameterOverride(text, memberExpression.Member.Name, bindingMask, value);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00006308 File Offset: 0x00004508
		public static void ApplyParameterOverride(this InputActionMap actionMap, string name, PrimitiveValue value, InputBinding bindingMask = default(InputBinding))
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			actionMap.ResolveBindingsIfNecessary();
			InputActionRebindingExtensions.ApplyParameterOverride(actionMap.m_State, actionMap.m_MapIndexInState, ref actionMap.m_ParameterOverrides, ref actionMap.m_ParameterOverridesCount, new InputActionRebindingExtensions.ParameterOverride(name, bindingMask, value));
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00006364 File Offset: 0x00004564
		public static void ApplyParameterOverride(this InputActionAsset asset, string name, PrimitiveValue value, InputBinding bindingMask = default(InputBinding))
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			asset.ResolveBindingsIfNecessary();
			InputActionRebindingExtensions.ApplyParameterOverride(asset.m_SharedStateForAllMaps, -1, ref asset.m_ParameterOverrides, ref asset.m_ParameterOverridesCount, new InputActionRebindingExtensions.ParameterOverride(name, bindingMask, value));
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000063C0 File Offset: 0x000045C0
		public static void ApplyParameterOverride(this InputAction action, string name, PrimitiveValue value, InputBinding bindingMask = default(InputBinding))
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
			orCreateActionMap.ResolveBindingsIfNecessary();
			bindingMask.action = action.name;
			InputActionRebindingExtensions.ApplyParameterOverride(orCreateActionMap.m_State, orCreateActionMap.m_MapIndexInState, ref orCreateActionMap.m_ParameterOverrides, ref orCreateActionMap.m_ParameterOverridesCount, new InputActionRebindingExtensions.ParameterOverride(name, bindingMask, value));
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000642C File Offset: 0x0000462C
		public static void ApplyParameterOverride(this InputAction action, string name, PrimitiveValue value, int bindingIndex)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			if (bindingIndex < 0)
			{
				throw new ArgumentOutOfRangeException("bindingIndex");
			}
			int num = action.BindingIndexOnActionToBindingIndexOnMap(bindingIndex);
			InputBinding inputBinding = new InputBinding
			{
				id = action.GetOrCreateActionMap().bindings[num].id
			};
			action.ApplyParameterOverride(name, value, inputBinding);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000064A8 File Offset: 0x000046A8
		private static void ApplyParameterOverride(InputActionState state, int mapIndex, ref InputActionRebindingExtensions.ParameterOverride[] parameterOverrides, ref int parameterOverridesCount, InputActionRebindingExtensions.ParameterOverride parameterOverride)
		{
			bool flag = false;
			if (parameterOverrides != null)
			{
				for (int i = 0; i < parameterOverridesCount; i++)
				{
					ref InputActionRebindingExtensions.ParameterOverride ptr = ref parameterOverrides[i];
					if (string.Equals(ptr.objectRegistrationName, parameterOverride.objectRegistrationName, StringComparison.OrdinalIgnoreCase) && string.Equals(ptr.parameter, parameterOverride.parameter, StringComparison.OrdinalIgnoreCase) && ptr.bindingMask == parameterOverride.bindingMask)
					{
						flag = true;
						ptr = parameterOverride;
						break;
					}
				}
			}
			if (!flag)
			{
				ArrayHelpers.AppendWithCapacity<InputActionRebindingExtensions.ParameterOverride>(ref parameterOverrides, ref parameterOverridesCount, parameterOverride, 10);
			}
			foreach (InputActionRebindingExtensions.Parameter parameter in new InputActionRebindingExtensions.ParameterEnumerable(state, parameterOverride, mapIndex))
			{
				InputActionMap actionMap = state.GetActionMap(parameter.bindingIndex);
				ref InputBinding binding = ref state.GetBinding(parameter.bindingIndex);
				InputActionRebindingExtensions.ParameterOverride? parameterOverride2 = InputActionRebindingExtensions.ParameterOverride.Find(actionMap, ref binding, parameterOverride.parameter, parameterOverride.objectRegistrationName);
				if (parameterOverride2 != null)
				{
					TypeCode typeCode = Type.GetTypeCode(parameter.field.FieldType);
					parameter.field.SetValue(parameter.instance, parameterOverride2.Value.value.ConvertTo(typeCode).ToObject());
				}
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000065FC File Offset: 0x000047FC
		public static int GetBindingIndex(this InputAction action, InputBinding bindingMask)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			ReadOnlyArray<InputBinding> bindings = action.bindings;
			for (int i = 0; i < bindings.Count; i++)
			{
				if (bindingMask.Matches(bindings[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00006644 File Offset: 0x00004844
		public static int GetBindingIndex(this InputActionMap actionMap, InputBinding bindingMask)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			ReadOnlyArray<InputBinding> bindings = actionMap.bindings;
			for (int i = 0; i < bindings.Count; i++)
			{
				if (bindingMask.Matches(bindings[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000668C File Offset: 0x0000488C
		public static int GetBindingIndex(this InputAction action, string group = null, string path = null)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			return action.GetBindingIndex(new InputBinding(path, null, group, null, null, null));
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000066BC File Offset: 0x000048BC
		public static InputBinding? GetBindingForControl(this InputAction action, InputControl control)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			int bindingIndexForControl = action.GetBindingIndexForControl(control);
			if (bindingIndexForControl == -1)
			{
				return null;
			}
			return new InputBinding?(action.bindings[bindingIndexForControl]);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00006710 File Offset: 0x00004910
		public unsafe static int GetBindingIndexForControl(this InputAction action, InputControl control)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
			orCreateActionMap.ResolveBindingsIfNecessary();
			InputActionState state = orCreateActionMap.m_State;
			InputControl[] controls = state.controls;
			int totalControlCount = state.totalControlCount;
			InputActionState.BindingState* bindingStates = state.bindingStates;
			int* controlIndexToBindingIndex = state.controlIndexToBindingIndex;
			int actionIndexInState = action.m_ActionIndexInState;
			for (int i = 0; i < totalControlCount; i++)
			{
				if (controls[i] == control)
				{
					int num = controlIndexToBindingIndex[i];
					if (bindingStates[num].actionIndex == actionIndexInState)
					{
						int bindingIndexInMap = state.GetBindingIndexInMap(num);
						return action.BindingIndexOnMapToBindingIndexOnAction(bindingIndexInMap);
					}
				}
			}
			return -1;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000067BC File Offset: 0x000049BC
		public static string GetBindingDisplayString(this InputAction action, InputBinding.DisplayStringOptions options = (InputBinding.DisplayStringOptions)0, string group = null)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			InputBinding inputBinding;
			if (!string.IsNullOrEmpty(group))
			{
				inputBinding = InputBinding.MaskByGroup(group);
			}
			else
			{
				InputBinding? inputBinding2 = action.FindEffectiveBindingMask();
				if (inputBinding2 != null)
				{
					inputBinding = inputBinding2.Value;
				}
				else
				{
					inputBinding = default(InputBinding);
				}
			}
			return action.GetBindingDisplayString(inputBinding, options);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00006814 File Offset: 0x00004A14
		public static string GetBindingDisplayString(this InputAction action, InputBinding bindingMask, InputBinding.DisplayStringOptions options = (InputBinding.DisplayStringOptions)0)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			string text = string.Empty;
			ReadOnlyArray<InputBinding> bindings = action.bindings;
			for (int i = 0; i < bindings.Count; i++)
			{
				if (!bindings[i].isPartOfComposite && bindingMask.Matches(bindings[i]))
				{
					string bindingDisplayString = action.GetBindingDisplayString(i, options);
					if (text != "")
					{
						text = text + " | " + bindingDisplayString;
					}
					else
					{
						text = bindingDisplayString;
					}
				}
			}
			return text;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000689C File Offset: 0x00004A9C
		public static string GetBindingDisplayString(this InputAction action, int bindingIndex, InputBinding.DisplayStringOptions options = (InputBinding.DisplayStringOptions)0)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			string text;
			string text2;
			return action.GetBindingDisplayString(bindingIndex, out text, out text2, options);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000068C4 File Offset: 0x00004AC4
		public unsafe static string GetBindingDisplayString(this InputAction action, int bindingIndex, out string deviceLayoutName, out string controlPath, InputBinding.DisplayStringOptions options = (InputBinding.DisplayStringOptions)0)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			deviceLayoutName = null;
			controlPath = null;
			ReadOnlyArray<InputBinding> bindings = action.bindings;
			int count = bindings.Count;
			if (bindingIndex < 0 || bindingIndex >= count)
			{
				throw new ArgumentOutOfRangeException(string.Format("Binding index {0} is out of range on action '{1}' with {2} bindings", bindingIndex, action, bindings.Count), "bindingIndex");
			}
			if (!bindings[bindingIndex].isComposite)
			{
				InputControl inputControl = null;
				InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
				orCreateActionMap.ResolveBindingsIfNecessary();
				InputActionState state = orCreateActionMap.m_State;
				int num = action.BindingIndexOnActionToBindingIndexOnMap(bindingIndex);
				int bindingIndexInState = state.GetBindingIndexInState(orCreateActionMap.m_MapIndexInState, num);
				InputActionState.BindingState* ptr = state.bindingStates + bindingIndexInState;
				if (ptr->controlCount > 0)
				{
					inputControl = state.controls[ptr->controlStartIndex];
				}
				InputBinding inputBinding = bindings[bindingIndex];
				if (string.IsNullOrEmpty(inputBinding.effectiveInteractions))
				{
					inputBinding.overrideInteractions = action.interactions;
				}
				else if (!string.IsNullOrEmpty(action.interactions))
				{
					inputBinding.overrideInteractions = inputBinding.effectiveInteractions + ";action.interactions";
				}
				return inputBinding.ToDisplayString(out deviceLayoutName, out controlPath, options, inputControl);
			}
			string name = NameAndParameters.Parse(bindings[bindingIndex].effectivePath).name;
			int firstPartIndex = bindingIndex + 1;
			int num2 = firstPartIndex;
			while (num2 < count && bindings[num2].isPartOfComposite)
			{
				num2++;
			}
			int partCount = num2 - firstPartIndex;
			string[] partStrings = new string[partCount];
			for (int i = 0; i < partCount; i++)
			{
				string text = action.GetBindingDisplayString(firstPartIndex + i, options);
				if (string.IsNullOrEmpty(text))
				{
					text = " ";
				}
				partStrings[i] = text;
			}
			string displayFormatString = InputBindingComposite.GetDisplayFormatString(name);
			if (string.IsNullOrEmpty(displayFormatString))
			{
				return StringHelpers.Join<string>("/", partStrings);
			}
			return StringHelpers.ExpandTemplateString(displayFormatString, delegate(string fragment)
			{
				string text2 = string.Empty;
				for (int j = 0; j < partCount; j++)
				{
					if (string.Equals(bindings[firstPartIndex + j].name, fragment, StringComparison.InvariantCultureIgnoreCase))
					{
						if (!string.IsNullOrEmpty(text2))
						{
							text2 = text2 + "|" + partStrings[j];
						}
						else
						{
							text2 = partStrings[j];
						}
					}
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 = " ";
				}
				return text2;
			});
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00006B04 File Offset: 0x00004D04
		public static void ApplyBindingOverride(this InputAction action, string newPath, string group = null, string path = null)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			action.ApplyBindingOverride(new InputBinding
			{
				overridePath = newPath,
				groups = group,
				path = path
			});
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00006B48 File Offset: 0x00004D48
		public static void ApplyBindingOverride(this InputAction action, InputBinding bindingOverride)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			bool enabled = action.enabled;
			if (enabled)
			{
				action.Disable();
			}
			bindingOverride.action = action.name;
			action.GetOrCreateActionMap().ApplyBindingOverride(bindingOverride);
			if (enabled)
			{
				action.Enable();
				action.RequestInitialStateCheckOnEnabledAction();
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00006B9C File Offset: 0x00004D9C
		public static void ApplyBindingOverride(this InputAction action, int bindingIndex, InputBinding bindingOverride)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			int num = action.BindingIndexOnActionToBindingIndexOnMap(bindingIndex);
			bindingOverride.action = action.name;
			action.GetOrCreateActionMap().ApplyBindingOverride(num, bindingOverride);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00006BDC File Offset: 0x00004DDC
		public static void ApplyBindingOverride(this InputAction action, int bindingIndex, string path)
		{
			if (path == null)
			{
				throw new ArgumentException("Binding path cannot be null", "path");
			}
			action.ApplyBindingOverride(bindingIndex, new InputBinding
			{
				overridePath = path
			});
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00006C14 File Offset: 0x00004E14
		public static int ApplyBindingOverride(this InputActionMap actionMap, InputBinding bindingOverride)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			InputBinding[] bindings = actionMap.m_Bindings;
			if (bindings == null)
			{
				return 0;
			}
			int num = bindings.Length;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (bindingOverride.Matches(ref bindings[i], (InputBinding.MatchOptions)0))
				{
					bindings[i].overridePath = bindingOverride.overridePath;
					bindings[i].overrideInteractions = bindingOverride.overrideInteractions;
					bindings[i].overrideProcessors = bindingOverride.overrideProcessors;
					num2++;
				}
			}
			if (num2 > 0)
			{
				actionMap.OnBindingModified();
			}
			return num2;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00006CA8 File Offset: 0x00004EA8
		public static void ApplyBindingOverride(this InputActionMap actionMap, int bindingIndex, InputBinding bindingOverride)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			InputBinding[] bindings = actionMap.m_Bindings;
			int num = ((bindings != null) ? bindings.Length : 0);
			if (bindingIndex < 0 || bindingIndex >= num)
			{
				throw new ArgumentOutOfRangeException("bindingIndex", string.Format("Cannot apply override to binding at index {0} in map '{1}' with only {2} bindings", bindingIndex, actionMap, num));
			}
			actionMap.m_Bindings[bindingIndex].overridePath = bindingOverride.overridePath;
			actionMap.m_Bindings[bindingIndex].overrideInteractions = bindingOverride.overrideInteractions;
			actionMap.m_Bindings[bindingIndex].overrideProcessors = bindingOverride.overrideProcessors;
			actionMap.OnBindingModified();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00006D4C File Offset: 0x00004F4C
		public static void RemoveBindingOverride(this InputAction action, int bindingIndex)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			action.ApplyBindingOverride(bindingIndex, default(InputBinding));
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00006D77 File Offset: 0x00004F77
		public static void RemoveBindingOverride(this InputAction action, InputBinding bindingMask)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			bindingMask.overridePath = null;
			bindingMask.overrideInteractions = null;
			bindingMask.overrideProcessors = null;
			action.ApplyBindingOverride(bindingMask);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00006DA6 File Offset: 0x00004FA6
		private static void RemoveBindingOverride(this InputActionMap actionMap, InputBinding bindingMask)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			bindingMask.overridePath = null;
			bindingMask.overrideInteractions = null;
			bindingMask.overrideProcessors = null;
			actionMap.ApplyBindingOverride(bindingMask);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00006DD8 File Offset: 0x00004FD8
		public static void RemoveAllBindingOverrides(this IInputActionCollection2 actions)
		{
			if (actions == null)
			{
				throw new ArgumentNullException("actions");
			}
			using (InputActionRebindingExtensions.DeferBindingResolution())
			{
				foreach (InputAction inputAction in actions)
				{
					InputActionMap orCreateActionMap = inputAction.GetOrCreateActionMap();
					InputBinding[] bindings = orCreateActionMap.m_Bindings;
					int num = bindings.LengthSafe<InputBinding>();
					for (int i = 0; i < num; i++)
					{
						ref InputBinding ptr = ref bindings[i];
						if (ptr.TriggersAction(inputAction))
						{
							ptr.RemoveOverrides();
						}
					}
					orCreateActionMap.OnBindingModified();
				}
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00006E90 File Offset: 0x00005090
		public static void RemoveAllBindingOverrides(this InputAction action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			string name = action.name;
			InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
			InputBinding[] bindings = orCreateActionMap.m_Bindings;
			if (bindings == null)
			{
				return;
			}
			int num = bindings.Length;
			for (int i = 0; i < num; i++)
			{
				if (string.Compare(bindings[i].action, name, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					bindings[i].overridePath = null;
					bindings[i].overrideInteractions = null;
					bindings[i].overrideProcessors = null;
				}
			}
			orCreateActionMap.OnBindingModified();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00006F20 File Offset: 0x00005120
		public static void ApplyBindingOverrides(this InputActionMap actionMap, IEnumerable<InputBinding> overrides)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			if (overrides == null)
			{
				throw new ArgumentNullException("overrides");
			}
			foreach (InputBinding inputBinding in overrides)
			{
				actionMap.ApplyBindingOverride(inputBinding);
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00006F88 File Offset: 0x00005188
		public static void RemoveBindingOverrides(this InputActionMap actionMap, IEnumerable<InputBinding> overrides)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			if (overrides == null)
			{
				throw new ArgumentNullException("overrides");
			}
			foreach (InputBinding inputBinding in overrides)
			{
				actionMap.RemoveBindingOverride(inputBinding);
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00006FEC File Offset: 0x000051EC
		public static int ApplyBindingOverridesOnMatchingControls(this InputAction action, InputControl control)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			ReadOnlyArray<InputBinding> bindings = action.bindings;
			int count = bindings.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				InputControl inputControl = InputControlPath.TryFindControl(control, bindings[i].path, 0);
				if (inputControl != null)
				{
					action.ApplyBindingOverride(i, inputControl.path);
					num++;
				}
			}
			return num;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00007064 File Offset: 0x00005264
		public static int ApplyBindingOverridesOnMatchingControls(this InputActionMap actionMap, InputControl control)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			ReadOnlyArray<InputAction> actions = actionMap.actions;
			int count = actions.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				num = actions[i].ApplyBindingOverridesOnMatchingControls(control);
			}
			return num;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000070BC File Offset: 0x000052BC
		public static string SaveBindingOverridesAsJson(this IInputActionCollection2 actions)
		{
			if (actions == null)
			{
				throw new ArgumentNullException("actions");
			}
			List<InputActionMap.BindingOverrideJson> list = new List<InputActionMap.BindingOverrideJson>();
			foreach (InputBinding inputBinding in actions.bindings)
			{
				actions.AddBindingOverrideJsonTo(inputBinding, list, null);
			}
			if (list.Count == 0)
			{
				return string.Empty;
			}
			return JsonUtility.ToJson(new InputActionMap.BindingOverrideListJson
			{
				bindings = list
			});
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00007148 File Offset: 0x00005348
		public static string SaveBindingOverridesAsJson(this InputAction action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			bool isSingletonAction = action.isSingletonAction;
			InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
			List<InputActionMap.BindingOverrideJson> list = new List<InputActionMap.BindingOverrideJson>();
			foreach (InputBinding inputBinding in action.bindings)
			{
				if (isSingletonAction || inputBinding.TriggersAction(action))
				{
					orCreateActionMap.AddBindingOverrideJsonTo(inputBinding, list, isSingletonAction ? action : null);
				}
			}
			if (list.Count == 0)
			{
				return string.Empty;
			}
			return JsonUtility.ToJson(new InputActionMap.BindingOverrideListJson
			{
				bindings = list
			});
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00007204 File Offset: 0x00005404
		private static void AddBindingOverrideJsonTo(this IInputActionCollection2 actions, InputBinding binding, List<InputActionMap.BindingOverrideJson> list, InputAction action = null)
		{
			if (!binding.hasOverrides)
			{
				return;
			}
			if (action == null)
			{
				action = actions.FindAction(binding.action, false);
			}
			string text = ((action != null && !action.isSingletonAction) ? (action.actionMap.name + "/" + action.name) : "");
			InputActionMap.BindingOverrideJson bindingOverrideJson = InputActionMap.BindingOverrideJson.FromBinding(binding, text);
			list.Add(bindingOverrideJson);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000726C File Offset: 0x0000546C
		public static void LoadBindingOverridesFromJson(this IInputActionCollection2 actions, string json, bool removeExisting = true)
		{
			if (actions == null)
			{
				throw new ArgumentNullException("actions");
			}
			using (InputActionRebindingExtensions.DeferBindingResolution())
			{
				if (removeExisting)
				{
					actions.RemoveAllBindingOverrides();
				}
				actions.LoadBindingOverridesFromJsonInternal(json);
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000072BC File Offset: 0x000054BC
		public static void LoadBindingOverridesFromJson(this InputAction action, string json, bool removeExisting = true)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			using (InputActionRebindingExtensions.DeferBindingResolution())
			{
				if (removeExisting)
				{
					action.RemoveAllBindingOverrides();
				}
				action.GetOrCreateActionMap().LoadBindingOverridesFromJsonInternal(json);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00007310 File Offset: 0x00005510
		private static void LoadBindingOverridesFromJsonInternal(this IInputActionCollection2 actions, string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				return;
			}
			foreach (InputActionMap.BindingOverrideJson bindingOverrideJson in JsonUtility.FromJson<InputActionMap.BindingOverrideListJson>(json).bindings)
			{
				if (!string.IsNullOrEmpty(bindingOverrideJson.id))
				{
					InputAction inputAction;
					int num = actions.FindBinding(new InputBinding
					{
						m_Id = bindingOverrideJson.id
					}, out inputAction);
					if (num != -1)
					{
						inputAction.ApplyBindingOverride(num, InputActionMap.BindingOverrideJson.ToBinding(bindingOverrideJson));
						continue;
					}
				}
				Debug.LogWarning("Could not override binding as no existing binding was found with the id: " + bindingOverrideJson.id);
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000073C0 File Offset: 0x000055C0
		public static InputActionRebindingExtensions.RebindingOperation PerformInteractiveRebinding(this InputAction action, int bindingIndex = -1)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			InputActionRebindingExtensions.RebindingOperation rebindingOperation = new InputActionRebindingExtensions.RebindingOperation().WithAction(action).OnMatchWaitForAnother(0.05f).WithControlsExcluding("<Pointer>/delta")
				.WithControlsExcluding("<Pointer>/position")
				.WithControlsExcluding("<Touchscreen>/touch*/position")
				.WithControlsExcluding("<Touchscreen>/touch*/delta")
				.WithControlsExcluding("<Mouse>/clickCount")
				.WithMatchingEventsBeingSuppressed(true);
			if (rebindingOperation.expectedControlType != "Button")
			{
				rebindingOperation.WithCancelingThrough("<Keyboard>/escape");
			}
			if (bindingIndex >= 0)
			{
				ReadOnlyArray<InputBinding> bindings = action.bindings;
				if (bindingIndex >= bindings.Count)
				{
					throw new ArgumentOutOfRangeException(string.Format("Binding index {0} is out of range for action '{1}' with {2} bindings", bindingIndex, action, bindings.Count), "bindings");
				}
				if (bindings[bindingIndex].isComposite)
				{
					throw new InvalidOperationException(string.Format("Cannot perform rebinding on composite binding '{0}' of '{1}'", bindings[bindingIndex], action));
				}
				rebindingOperation.WithTargetBinding(bindingIndex);
			}
			return rebindingOperation;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000074BD File Offset: 0x000056BD
		internal static InputActionRebindingExtensions.DeferBindingResolutionWrapper DeferBindingResolution()
		{
			if (InputActionRebindingExtensions.s_DeferBindingResolutionWrapper == null)
			{
				InputActionRebindingExtensions.s_DeferBindingResolutionWrapper = new InputActionRebindingExtensions.DeferBindingResolutionWrapper();
			}
			InputActionRebindingExtensions.s_DeferBindingResolutionWrapper.Acquire();
			return InputActionRebindingExtensions.s_DeferBindingResolutionWrapper;
		}

		// Token: 0x040000BD RID: 189
		private static InputActionRebindingExtensions.DeferBindingResolutionWrapper s_DeferBindingResolutionWrapper;

		// Token: 0x0200016C RID: 364
		internal struct Parameter
		{
			// Token: 0x040007D5 RID: 2005
			public object instance;

			// Token: 0x040007D6 RID: 2006
			public FieldInfo field;

			// Token: 0x040007D7 RID: 2007
			public int bindingIndex;
		}

		// Token: 0x0200016D RID: 365
		private struct ParameterEnumerable : IEnumerable<InputActionRebindingExtensions.Parameter>, IEnumerable
		{
			// Token: 0x060012A8 RID: 4776 RVA: 0x000586BF File Offset: 0x000568BF
			public ParameterEnumerable(InputActionState state, InputActionRebindingExtensions.ParameterOverride parameter, int mapIndex = -1)
			{
				this.m_State = state;
				this.m_Parameter = parameter;
				this.m_MapIndex = mapIndex;
			}

			// Token: 0x060012A9 RID: 4777 RVA: 0x000586D6 File Offset: 0x000568D6
			public InputActionRebindingExtensions.ParameterEnumerator GetEnumerator()
			{
				return new InputActionRebindingExtensions.ParameterEnumerator(this.m_State, this.m_Parameter, this.m_MapIndex);
			}

			// Token: 0x060012AA RID: 4778 RVA: 0x000586EF File Offset: 0x000568EF
			IEnumerator<InputActionRebindingExtensions.Parameter> IEnumerable<InputActionRebindingExtensions.Parameter>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x060012AB RID: 4779 RVA: 0x000586FC File Offset: 0x000568FC
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040007D8 RID: 2008
			private InputActionState m_State;

			// Token: 0x040007D9 RID: 2009
			private InputActionRebindingExtensions.ParameterOverride m_Parameter;

			// Token: 0x040007DA RID: 2010
			private int m_MapIndex;
		}

		// Token: 0x0200016E RID: 366
		private struct ParameterEnumerator : IEnumerator<InputActionRebindingExtensions.Parameter>, IEnumerator, IDisposable
		{
			// Token: 0x060012AC RID: 4780 RVA: 0x0005870C File Offset: 0x0005690C
			public ParameterEnumerator(InputActionState state, InputActionRebindingExtensions.ParameterOverride parameter, int mapIndex = -1)
			{
				this = default(InputActionRebindingExtensions.ParameterEnumerator);
				this.m_State = state;
				this.m_ParameterName = parameter.parameter;
				this.m_MapIndex = mapIndex;
				this.m_ObjectType = parameter.objectType;
				this.m_MayBeComposite = this.m_ObjectType == null || typeof(InputBindingComposite).IsAssignableFrom(this.m_ObjectType);
				this.m_MayBeProcessor = this.m_ObjectType == null || typeof(InputProcessor).IsAssignableFrom(this.m_ObjectType);
				this.m_MayBeInteraction = this.m_ObjectType == null || typeof(IInputInteraction).IsAssignableFrom(this.m_ObjectType);
				this.m_BindingMask = parameter.bindingMask;
				this.Reset();
			}

			// Token: 0x060012AD RID: 4781 RVA: 0x000587E0 File Offset: 0x000569E0
			private bool MoveToNextBinding()
			{
				ref InputActionState.BindingState bindingState;
				for (;;)
				{
					this.m_BindingCurrentIndex++;
					if (this.m_BindingCurrentIndex >= this.m_BindingEndIndex)
					{
						break;
					}
					ref InputBinding binding = ref this.m_State.GetBinding(this.m_BindingCurrentIndex);
					bindingState = this.m_State.GetBindingState(this.m_BindingCurrentIndex);
					if ((bindingState.processorCount != 0 || bindingState.interactionCount != 0 || binding.isComposite) && (!this.m_MayBeComposite || this.m_MayBeProcessor || this.m_MayBeInteraction || binding.isComposite) && (!this.m_MayBeProcessor || this.m_MayBeComposite || this.m_MayBeInteraction || bindingState.processorCount != 0) && (!this.m_MayBeInteraction || this.m_MayBeComposite || this.m_MayBeProcessor || bindingState.interactionCount != 0) && this.m_BindingMask.Matches(ref binding, (InputBinding.MatchOptions)0))
					{
						goto Block_12;
					}
				}
				return false;
				Block_12:
				if (this.m_MayBeComposite)
				{
					ref InputBinding binding;
					this.m_CurrentBindingIsComposite = binding.isComposite;
				}
				this.m_ProcessorCurrentIndex = bindingState.processorStartIndex - 1;
				this.m_ProcessorEndIndex = bindingState.processorStartIndex + bindingState.processorCount;
				this.m_InteractionCurrentIndex = bindingState.interactionStartIndex - 1;
				this.m_InteractionEndIndex = bindingState.interactionStartIndex + bindingState.interactionCount;
				return true;
			}

			// Token: 0x060012AE RID: 4782 RVA: 0x00058918 File Offset: 0x00056B18
			private bool MoveToNextInteraction()
			{
				while (this.m_InteractionCurrentIndex < this.m_InteractionEndIndex)
				{
					this.m_InteractionCurrentIndex++;
					if (this.m_InteractionCurrentIndex == this.m_InteractionEndIndex)
					{
						break;
					}
					IInputInteraction inputInteraction = this.m_State.interactions[this.m_InteractionCurrentIndex];
					if (this.FindParameter(inputInteraction))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060012AF RID: 4783 RVA: 0x00058970 File Offset: 0x00056B70
			private bool MoveToNextProcessor()
			{
				while (this.m_ProcessorCurrentIndex < this.m_ProcessorEndIndex)
				{
					this.m_ProcessorCurrentIndex++;
					if (this.m_ProcessorCurrentIndex == this.m_ProcessorEndIndex)
					{
						break;
					}
					InputProcessor inputProcessor = this.m_State.processors[this.m_ProcessorCurrentIndex];
					if (this.FindParameter(inputProcessor))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060012B0 RID: 4784 RVA: 0x000589C8 File Offset: 0x00056BC8
			private bool FindParameter(object instance)
			{
				if (this.m_ObjectType != null && !this.m_ObjectType.IsInstanceOfType(instance))
				{
					return false;
				}
				FieldInfo field = instance.GetType().GetField(this.m_ParameterName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
				if (field == null)
				{
					return false;
				}
				this.m_CurrentParameter = field;
				this.m_CurrentObject = instance;
				return true;
			}

			// Token: 0x060012B1 RID: 4785 RVA: 0x00058A24 File Offset: 0x00056C24
			public bool MoveNext()
			{
				while (!this.m_MayBeInteraction || !this.MoveToNextInteraction())
				{
					if (this.m_MayBeProcessor && this.MoveToNextProcessor())
					{
						return true;
					}
					if (!this.MoveToNextBinding())
					{
						return false;
					}
					if (this.m_MayBeComposite && this.m_CurrentBindingIsComposite)
					{
						int compositeOrCompositeBindingIndex = this.m_State.GetBindingState(this.m_BindingCurrentIndex).compositeOrCompositeBindingIndex;
						InputBindingComposite inputBindingComposite = this.m_State.composites[compositeOrCompositeBindingIndex];
						if (this.FindParameter(inputBindingComposite))
						{
							return true;
						}
					}
				}
				return true;
			}

			// Token: 0x060012B2 RID: 4786 RVA: 0x00058AA0 File Offset: 0x00056CA0
			public unsafe void Reset()
			{
				this.m_CurrentObject = null;
				this.m_CurrentParameter = null;
				this.m_InteractionCurrentIndex = 0;
				this.m_InteractionEndIndex = 0;
				this.m_ProcessorCurrentIndex = 0;
				this.m_ProcessorEndIndex = 0;
				this.m_CurrentBindingIsComposite = false;
				if (this.m_MapIndex < 0)
				{
					this.m_BindingCurrentIndex = -1;
					this.m_BindingEndIndex = this.m_State.totalBindingCount;
					return;
				}
				this.m_BindingCurrentIndex = this.m_State.mapIndices[this.m_MapIndex].bindingStartIndex - 1;
				this.m_BindingEndIndex = this.m_State.mapIndices[this.m_MapIndex].bindingStartIndex + this.m_State.mapIndices[this.m_MapIndex].bindingCount;
			}

			// Token: 0x17000504 RID: 1284
			// (get) Token: 0x060012B3 RID: 4787 RVA: 0x00058B6C File Offset: 0x00056D6C
			public InputActionRebindingExtensions.Parameter Current
			{
				get
				{
					return new InputActionRebindingExtensions.Parameter
					{
						instance = this.m_CurrentObject,
						field = this.m_CurrentParameter,
						bindingIndex = this.m_BindingCurrentIndex
					};
				}
			}

			// Token: 0x17000505 RID: 1285
			// (get) Token: 0x060012B4 RID: 4788 RVA: 0x00058BA9 File Offset: 0x00056DA9
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060012B5 RID: 4789 RVA: 0x00058BB6 File Offset: 0x00056DB6
			public void Dispose()
			{
			}

			// Token: 0x040007DB RID: 2011
			private InputActionState m_State;

			// Token: 0x040007DC RID: 2012
			private int m_MapIndex;

			// Token: 0x040007DD RID: 2013
			private int m_BindingCurrentIndex;

			// Token: 0x040007DE RID: 2014
			private int m_BindingEndIndex;

			// Token: 0x040007DF RID: 2015
			private int m_InteractionCurrentIndex;

			// Token: 0x040007E0 RID: 2016
			private int m_InteractionEndIndex;

			// Token: 0x040007E1 RID: 2017
			private int m_ProcessorCurrentIndex;

			// Token: 0x040007E2 RID: 2018
			private int m_ProcessorEndIndex;

			// Token: 0x040007E3 RID: 2019
			private InputBinding m_BindingMask;

			// Token: 0x040007E4 RID: 2020
			private Type m_ObjectType;

			// Token: 0x040007E5 RID: 2021
			private string m_ParameterName;

			// Token: 0x040007E6 RID: 2022
			private bool m_MayBeInteraction;

			// Token: 0x040007E7 RID: 2023
			private bool m_MayBeProcessor;

			// Token: 0x040007E8 RID: 2024
			private bool m_MayBeComposite;

			// Token: 0x040007E9 RID: 2025
			private bool m_CurrentBindingIsComposite;

			// Token: 0x040007EA RID: 2026
			private object m_CurrentObject;

			// Token: 0x040007EB RID: 2027
			private FieldInfo m_CurrentParameter;
		}

		// Token: 0x0200016F RID: 367
		internal struct ParameterOverride
		{
			// Token: 0x17000506 RID: 1286
			// (get) Token: 0x060012B6 RID: 4790 RVA: 0x00058BB8 File Offset: 0x00056DB8
			public Type objectType
			{
				get
				{
					Type type;
					if ((type = InputProcessor.s_Processors.LookupTypeRegistration(this.objectRegistrationName)) == null)
					{
						type = InputInteraction.s_Interactions.LookupTypeRegistration(this.objectRegistrationName) ?? InputBindingComposite.s_Composites.LookupTypeRegistration(this.objectRegistrationName);
					}
					return type;
				}
			}

			// Token: 0x060012B7 RID: 4791 RVA: 0x00058BF4 File Offset: 0x00056DF4
			public ParameterOverride(string parameterName, InputBinding bindingMask, PrimitiveValue value = default(PrimitiveValue))
			{
				int num = parameterName.IndexOf(':');
				if (num < 0)
				{
					this.objectRegistrationName = null;
					this.parameter = parameterName;
				}
				else
				{
					this.objectRegistrationName = parameterName.Substring(0, num);
					this.parameter = parameterName.Substring(num + 1);
				}
				this.bindingMask = bindingMask;
				this.value = value;
			}

			// Token: 0x060012B8 RID: 4792 RVA: 0x00058C49 File Offset: 0x00056E49
			public ParameterOverride(string objectRegistrationName, string parameterName, InputBinding bindingMask, PrimitiveValue value = default(PrimitiveValue))
			{
				this.objectRegistrationName = objectRegistrationName;
				this.parameter = parameterName;
				this.bindingMask = bindingMask;
				this.value = value;
			}

			// Token: 0x060012B9 RID: 4793 RVA: 0x00058C68 File Offset: 0x00056E68
			public static InputActionRebindingExtensions.ParameterOverride? Find(InputActionMap actionMap, ref InputBinding binding, string parameterName, string objectRegistrationName)
			{
				InputActionRebindingExtensions.ParameterOverride? parameterOverride = InputActionRebindingExtensions.ParameterOverride.Find(actionMap.m_ParameterOverrides, actionMap.m_ParameterOverridesCount, ref binding, parameterName, objectRegistrationName);
				InputActionAsset asset = actionMap.asset;
				InputActionRebindingExtensions.ParameterOverride? parameterOverride2 = ((asset != null) ? InputActionRebindingExtensions.ParameterOverride.Find(asset.m_ParameterOverrides, asset.m_ParameterOverridesCount, ref binding, parameterName, objectRegistrationName) : null);
				return InputActionRebindingExtensions.ParameterOverride.PickMoreSpecificOne(parameterOverride, parameterOverride2);
			}

			// Token: 0x060012BA RID: 4794 RVA: 0x00058CC0 File Offset: 0x00056EC0
			private static InputActionRebindingExtensions.ParameterOverride? Find(InputActionRebindingExtensions.ParameterOverride[] overrides, int overrideCount, ref InputBinding binding, string parameterName, string objectRegistrationName)
			{
				InputActionRebindingExtensions.ParameterOverride? parameterOverride = null;
				for (int i = 0; i < overrideCount; i++)
				{
					ref InputActionRebindingExtensions.ParameterOverride ptr = ref overrides[i];
					if (string.Equals(parameterName, ptr.parameter, StringComparison.OrdinalIgnoreCase) && ptr.bindingMask.Matches(binding) && (ptr.objectRegistrationName == null || string.Equals(ptr.objectRegistrationName, objectRegistrationName, StringComparison.OrdinalIgnoreCase)))
					{
						if (parameterOverride == null)
						{
							parameterOverride = new InputActionRebindingExtensions.ParameterOverride?(ptr);
						}
						else
						{
							parameterOverride = InputActionRebindingExtensions.ParameterOverride.PickMoreSpecificOne(parameterOverride, new InputActionRebindingExtensions.ParameterOverride?(ptr));
						}
					}
				}
				return parameterOverride;
			}

			// Token: 0x060012BB RID: 4795 RVA: 0x00058D50 File Offset: 0x00056F50
			private static InputActionRebindingExtensions.ParameterOverride? PickMoreSpecificOne(InputActionRebindingExtensions.ParameterOverride? first, InputActionRebindingExtensions.ParameterOverride? second)
			{
				if (first == null)
				{
					return second;
				}
				if (second == null)
				{
					return first;
				}
				if (first.Value.objectRegistrationName != null && second.Value.objectRegistrationName == null)
				{
					return first;
				}
				if (second.Value.objectRegistrationName != null && first.Value.objectRegistrationName == null)
				{
					return second;
				}
				InputActionRebindingExtensions.ParameterOverride parameterOverride = first.Value;
				if (parameterOverride.bindingMask.effectivePath != null)
				{
					parameterOverride = second.Value;
					if (parameterOverride.bindingMask.effectivePath == null)
					{
						return first;
					}
				}
				parameterOverride = second.Value;
				if (parameterOverride.bindingMask.effectivePath != null)
				{
					parameterOverride = first.Value;
					if (parameterOverride.bindingMask.effectivePath == null)
					{
						return second;
					}
				}
				parameterOverride = first.Value;
				if (parameterOverride.bindingMask.action != null)
				{
					parameterOverride = second.Value;
					if (parameterOverride.bindingMask.action == null)
					{
						return first;
					}
				}
				parameterOverride = second.Value;
				if (parameterOverride.bindingMask.action != null)
				{
					parameterOverride = first.Value;
					if (parameterOverride.bindingMask.action == null)
					{
						return second;
					}
				}
				return first;
			}

			// Token: 0x040007EC RID: 2028
			public string objectRegistrationName;

			// Token: 0x040007ED RID: 2029
			public string parameter;

			// Token: 0x040007EE RID: 2030
			public InputBinding bindingMask;

			// Token: 0x040007EF RID: 2031
			public PrimitiveValue value;
		}

		// Token: 0x02000170 RID: 368
		public sealed class RebindingOperation : IDisposable
		{
			// Token: 0x17000507 RID: 1287
			// (get) Token: 0x060012BC RID: 4796 RVA: 0x00058E68 File Offset: 0x00057068
			public InputAction action
			{
				get
				{
					return this.m_ActionToRebind;
				}
			}

			// Token: 0x17000508 RID: 1288
			// (get) Token: 0x060012BD RID: 4797 RVA: 0x00058E70 File Offset: 0x00057070
			public InputBinding? bindingMask
			{
				get
				{
					return this.m_BindingMask;
				}
			}

			// Token: 0x17000509 RID: 1289
			// (get) Token: 0x060012BE RID: 4798 RVA: 0x00058E78 File Offset: 0x00057078
			public InputControlList<InputControl> candidates
			{
				get
				{
					return this.m_Candidates;
				}
			}

			// Token: 0x1700050A RID: 1290
			// (get) Token: 0x060012BF RID: 4799 RVA: 0x00058E80 File Offset: 0x00057080
			public ReadOnlyArray<float> scores
			{
				get
				{
					return new ReadOnlyArray<float>(this.m_Scores, 0, this.m_Candidates.Count);
				}
			}

			// Token: 0x1700050B RID: 1291
			// (get) Token: 0x060012C0 RID: 4800 RVA: 0x00058E99 File Offset: 0x00057099
			public ReadOnlyArray<float> magnitudes
			{
				get
				{
					return new ReadOnlyArray<float>(this.m_Magnitudes, 0, this.m_Candidates.Count);
				}
			}

			// Token: 0x1700050C RID: 1292
			// (get) Token: 0x060012C1 RID: 4801 RVA: 0x00058EB2 File Offset: 0x000570B2
			public InputControl selectedControl
			{
				get
				{
					if (this.m_Candidates.Count == 0)
					{
						return null;
					}
					return this.m_Candidates[0];
				}
			}

			// Token: 0x1700050D RID: 1293
			// (get) Token: 0x060012C2 RID: 4802 RVA: 0x00058ECF File Offset: 0x000570CF
			public bool started
			{
				get
				{
					return (this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.Started) > (InputActionRebindingExtensions.RebindingOperation.Flags)0;
				}
			}

			// Token: 0x1700050E RID: 1294
			// (get) Token: 0x060012C3 RID: 4803 RVA: 0x00058EDC File Offset: 0x000570DC
			public bool completed
			{
				get
				{
					return (this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.Completed) > (InputActionRebindingExtensions.RebindingOperation.Flags)0;
				}
			}

			// Token: 0x1700050F RID: 1295
			// (get) Token: 0x060012C4 RID: 4804 RVA: 0x00058EE9 File Offset: 0x000570E9
			public bool canceled
			{
				get
				{
					return (this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.Canceled) > (InputActionRebindingExtensions.RebindingOperation.Flags)0;
				}
			}

			// Token: 0x17000510 RID: 1296
			// (get) Token: 0x060012C5 RID: 4805 RVA: 0x00058EF6 File Offset: 0x000570F6
			public double startTime
			{
				get
				{
					return this.m_StartTime;
				}
			}

			// Token: 0x17000511 RID: 1297
			// (get) Token: 0x060012C6 RID: 4806 RVA: 0x00058EFE File Offset: 0x000570FE
			public float timeout
			{
				get
				{
					return this.m_Timeout;
				}
			}

			// Token: 0x17000512 RID: 1298
			// (get) Token: 0x060012C7 RID: 4807 RVA: 0x00058F06 File Offset: 0x00057106
			public string expectedControlType
			{
				get
				{
					return this.m_ExpectedLayout;
				}
			}

			// Token: 0x060012C8 RID: 4808 RVA: 0x00058F14 File Offset: 0x00057114
			public InputActionRebindingExtensions.RebindingOperation WithAction(InputAction action)
			{
				this.ThrowIfRebindInProgress();
				if (action == null)
				{
					throw new ArgumentNullException("action");
				}
				if (action.enabled)
				{
					throw new InvalidOperationException(string.Format("Cannot rebind action '{0}' while it is enabled", action));
				}
				this.m_ActionToRebind = action;
				if (!string.IsNullOrEmpty(action.expectedControlType))
				{
					this.WithExpectedControlType(action.expectedControlType);
				}
				else if (action.type == InputActionType.Button)
				{
					this.WithExpectedControlType("Button");
				}
				return this;
			}

			// Token: 0x060012C9 RID: 4809 RVA: 0x00058F87 File Offset: 0x00057187
			public InputActionRebindingExtensions.RebindingOperation WithMatchingEventsBeingSuppressed(bool value = true)
			{
				this.ThrowIfRebindInProgress();
				if (value)
				{
					this.m_Flags |= InputActionRebindingExtensions.RebindingOperation.Flags.SuppressMatchingEvents;
				}
				else
				{
					this.m_Flags &= ~InputActionRebindingExtensions.RebindingOperation.Flags.SuppressMatchingEvents;
				}
				return this;
			}

			// Token: 0x060012CA RID: 4810 RVA: 0x00058FB9 File Offset: 0x000571B9
			public InputActionRebindingExtensions.RebindingOperation WithCancelingThrough(string binding)
			{
				this.ThrowIfRebindInProgress();
				this.m_CancelBinding = binding;
				return this;
			}

			// Token: 0x060012CB RID: 4811 RVA: 0x00058FC9 File Offset: 0x000571C9
			public InputActionRebindingExtensions.RebindingOperation WithCancelingThrough(InputControl control)
			{
				this.ThrowIfRebindInProgress();
				if (control == null)
				{
					throw new ArgumentNullException("control");
				}
				return this.WithCancelingThrough(control.path);
			}

			// Token: 0x060012CC RID: 4812 RVA: 0x00058FEB File Offset: 0x000571EB
			public InputActionRebindingExtensions.RebindingOperation WithExpectedControlType(string layoutName)
			{
				this.ThrowIfRebindInProgress();
				this.m_ExpectedLayout = new InternedString(layoutName);
				return this;
			}

			// Token: 0x060012CD RID: 4813 RVA: 0x00059000 File Offset: 0x00057200
			public InputActionRebindingExtensions.RebindingOperation WithExpectedControlType(Type type)
			{
				this.ThrowIfRebindInProgress();
				if (type != null && !typeof(InputControl).IsAssignableFrom(type))
				{
					throw new ArgumentException("Type '" + type.Name + "' is not an InputControl", "type");
				}
				this.m_ControlType = type;
				return this;
			}

			// Token: 0x060012CE RID: 4814 RVA: 0x00059056 File Offset: 0x00057256
			public InputActionRebindingExtensions.RebindingOperation WithExpectedControlType<TControl>() where TControl : InputControl
			{
				this.ThrowIfRebindInProgress();
				return this.WithExpectedControlType(typeof(TControl));
			}

			// Token: 0x060012CF RID: 4815 RVA: 0x00059070 File Offset: 0x00057270
			public InputActionRebindingExtensions.RebindingOperation WithTargetBinding(int bindingIndex)
			{
				if (bindingIndex < 0)
				{
					throw new ArgumentOutOfRangeException("bindingIndex");
				}
				this.m_TargetBindingIndex = bindingIndex;
				if (this.m_ActionToRebind != null && bindingIndex < this.m_ActionToRebind.bindings.Count)
				{
					InputBinding inputBinding = this.m_ActionToRebind.bindings[bindingIndex];
					if (inputBinding.isPartOfComposite)
					{
						string nameOfComposite = this.m_ActionToRebind.ChangeBinding(bindingIndex).PreviousCompositeBinding(null).binding.GetNameOfComposite();
						string name = inputBinding.name;
						string expectedControlLayoutName = InputBindingComposite.GetExpectedControlLayoutName(nameOfComposite, name);
						if (!string.IsNullOrEmpty(expectedControlLayoutName))
						{
							this.WithExpectedControlType(expectedControlLayoutName);
						}
					}
					InputActionMap actionMap = this.action.actionMap;
					InputActionAsset inputActionAsset = ((actionMap != null) ? actionMap.asset : null);
					if (inputActionAsset != null && !string.IsNullOrEmpty(inputBinding.groups))
					{
						string[] array = inputBinding.groups.Split(';', StringSplitOptions.None);
						for (int i = 0; i < array.Length; i++)
						{
							string group = array[i];
							int num = inputActionAsset.controlSchemes.IndexOf((InputControlScheme x) => group.Equals(x.bindingGroup, StringComparison.InvariantCultureIgnoreCase));
							if (num != -1)
							{
								foreach (InputControlScheme.DeviceRequirement deviceRequirement in inputActionAsset.controlSchemes[num].deviceRequirements)
								{
									this.WithControlsHavingToMatchPath(deviceRequirement.controlPath);
								}
							}
						}
					}
				}
				return this;
			}

			// Token: 0x060012D0 RID: 4816 RVA: 0x0005921C File Offset: 0x0005741C
			public InputActionRebindingExtensions.RebindingOperation WithBindingMask(InputBinding? bindingMask)
			{
				this.m_BindingMask = bindingMask;
				return this;
			}

			// Token: 0x060012D1 RID: 4817 RVA: 0x00059228 File Offset: 0x00057428
			public InputActionRebindingExtensions.RebindingOperation WithBindingGroup(string group)
			{
				return this.WithBindingMask(new InputBinding?(new InputBinding
				{
					groups = group
				}));
			}

			// Token: 0x060012D2 RID: 4818 RVA: 0x00059251 File Offset: 0x00057451
			public InputActionRebindingExtensions.RebindingOperation WithoutGeneralizingPathOfSelectedControl()
			{
				this.m_Flags |= InputActionRebindingExtensions.RebindingOperation.Flags.DontGeneralizePathOfSelectedControl;
				return this;
			}

			// Token: 0x060012D3 RID: 4819 RVA: 0x00059266 File Offset: 0x00057466
			public InputActionRebindingExtensions.RebindingOperation WithRebindAddingNewBinding(string group = null)
			{
				this.m_Flags |= InputActionRebindingExtensions.RebindingOperation.Flags.AddNewBinding;
				this.m_BindingGroupForNewBinding = group;
				return this;
			}

			// Token: 0x060012D4 RID: 4820 RVA: 0x00059282 File Offset: 0x00057482
			public InputActionRebindingExtensions.RebindingOperation WithMagnitudeHavingToBeGreaterThan(float magnitude)
			{
				this.ThrowIfRebindInProgress();
				if (magnitude < 0f)
				{
					throw new ArgumentException(string.Format("Magnitude has to be positive but was {0}", magnitude), "magnitude");
				}
				this.m_MagnitudeThreshold = magnitude;
				return this;
			}

			// Token: 0x060012D5 RID: 4821 RVA: 0x000592B5 File Offset: 0x000574B5
			public InputActionRebindingExtensions.RebindingOperation WithoutIgnoringNoisyControls()
			{
				this.ThrowIfRebindInProgress();
				this.m_Flags |= InputActionRebindingExtensions.RebindingOperation.Flags.DontIgnoreNoisyControls;
				return this;
			}

			// Token: 0x060012D6 RID: 4822 RVA: 0x000592D0 File Offset: 0x000574D0
			public InputActionRebindingExtensions.RebindingOperation WithControlsHavingToMatchPath(string path)
			{
				this.ThrowIfRebindInProgress();
				if (string.IsNullOrEmpty(path))
				{
					throw new ArgumentNullException("path");
				}
				for (int i = 0; i < this.m_IncludePathCount; i++)
				{
					if (string.Compare(this.m_IncludePaths[i], path, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						return this;
					}
				}
				ArrayHelpers.AppendWithCapacity<string>(ref this.m_IncludePaths, ref this.m_IncludePathCount, path, 10);
				return this;
			}

			// Token: 0x060012D7 RID: 4823 RVA: 0x00059330 File Offset: 0x00057530
			public InputActionRebindingExtensions.RebindingOperation WithControlsExcluding(string path)
			{
				this.ThrowIfRebindInProgress();
				if (string.IsNullOrEmpty(path))
				{
					throw new ArgumentNullException("path");
				}
				for (int i = 0; i < this.m_ExcludePathCount; i++)
				{
					if (string.Compare(this.m_ExcludePaths[i], path, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						return this;
					}
				}
				ArrayHelpers.AppendWithCapacity<string>(ref this.m_ExcludePaths, ref this.m_ExcludePathCount, path, 10);
				return this;
			}

			// Token: 0x060012D8 RID: 4824 RVA: 0x00059390 File Offset: 0x00057590
			public InputActionRebindingExtensions.RebindingOperation WithTimeout(float timeInSeconds)
			{
				this.m_Timeout = timeInSeconds;
				return this;
			}

			// Token: 0x060012D9 RID: 4825 RVA: 0x0005939A File Offset: 0x0005759A
			public InputActionRebindingExtensions.RebindingOperation OnComplete(Action<InputActionRebindingExtensions.RebindingOperation> callback)
			{
				this.m_OnComplete = callback;
				return this;
			}

			// Token: 0x060012DA RID: 4826 RVA: 0x000593A4 File Offset: 0x000575A4
			public InputActionRebindingExtensions.RebindingOperation OnCancel(Action<InputActionRebindingExtensions.RebindingOperation> callback)
			{
				this.m_OnCancel = callback;
				return this;
			}

			// Token: 0x060012DB RID: 4827 RVA: 0x000593AE File Offset: 0x000575AE
			public InputActionRebindingExtensions.RebindingOperation OnPotentialMatch(Action<InputActionRebindingExtensions.RebindingOperation> callback)
			{
				this.m_OnPotentialMatch = callback;
				return this;
			}

			// Token: 0x060012DC RID: 4828 RVA: 0x000593B8 File Offset: 0x000575B8
			public InputActionRebindingExtensions.RebindingOperation OnGeneratePath(Func<InputControl, string> callback)
			{
				this.m_OnGeneratePath = callback;
				return this;
			}

			// Token: 0x060012DD RID: 4829 RVA: 0x000593C2 File Offset: 0x000575C2
			public InputActionRebindingExtensions.RebindingOperation OnComputeScore(Func<InputControl, InputEventPtr, float> callback)
			{
				this.m_OnComputeScore = callback;
				return this;
			}

			// Token: 0x060012DE RID: 4830 RVA: 0x000593CC File Offset: 0x000575CC
			public InputActionRebindingExtensions.RebindingOperation OnApplyBinding(Action<InputActionRebindingExtensions.RebindingOperation, string> callback)
			{
				this.m_OnApplyBinding = callback;
				return this;
			}

			// Token: 0x060012DF RID: 4831 RVA: 0x000593D6 File Offset: 0x000575D6
			public InputActionRebindingExtensions.RebindingOperation OnMatchWaitForAnother(float seconds)
			{
				this.m_WaitSecondsAfterMatch = seconds;
				return this;
			}

			// Token: 0x060012E0 RID: 4832 RVA: 0x000593E0 File Offset: 0x000575E0
			public InputActionRebindingExtensions.RebindingOperation Start()
			{
				if (this.started)
				{
					return this;
				}
				if (this.m_ActionToRebind != null && this.m_ActionToRebind.bindings.Count == 0 && (this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.AddNewBinding) == (InputActionRebindingExtensions.RebindingOperation.Flags)0)
				{
					throw new InvalidOperationException(string.Format("Action '{0}' must have at least one existing binding or must be used with WithRebindingAddNewBinding()", this.action));
				}
				if (this.m_ActionToRebind == null && this.m_OnApplyBinding == null)
				{
					throw new InvalidOperationException("Must either have an action (call WithAction()) to apply binding to or have a custom callback to apply the binding (call OnApplyBinding())");
				}
				this.m_StartTime = InputState.currentTime;
				if (this.m_WaitSecondsAfterMatch > 0f || this.m_Timeout > 0f)
				{
					this.HookOnAfterUpdate();
					this.m_LastMatchTime = -1.0;
				}
				this.HookOnEvent();
				this.m_Flags |= InputActionRebindingExtensions.RebindingOperation.Flags.Started;
				this.m_Flags &= ~InputActionRebindingExtensions.RebindingOperation.Flags.Canceled;
				this.m_Flags &= ~InputActionRebindingExtensions.RebindingOperation.Flags.Completed;
				return this;
			}

			// Token: 0x060012E1 RID: 4833 RVA: 0x000594C0 File Offset: 0x000576C0
			public void Cancel()
			{
				if (!this.started)
				{
					return;
				}
				this.OnCancel();
			}

			// Token: 0x060012E2 RID: 4834 RVA: 0x000594D1 File Offset: 0x000576D1
			public void Complete()
			{
				if (!this.started)
				{
					return;
				}
				this.OnComplete();
			}

			// Token: 0x060012E3 RID: 4835 RVA: 0x000594E4 File Offset: 0x000576E4
			public void AddCandidate(InputControl control, float score, float magnitude = -1f)
			{
				if (control == null)
				{
					throw new ArgumentNullException("control");
				}
				int num = this.m_Candidates.IndexOf(control);
				if (num != -1)
				{
					this.m_Scores[num] = score;
				}
				else
				{
					int count = this.m_Candidates.Count;
					int count2 = this.m_Candidates.Count;
					this.m_Candidates.Add(control);
					ArrayHelpers.AppendWithCapacity<float>(ref this.m_Scores, ref count, score, 10);
					ArrayHelpers.AppendWithCapacity<float>(ref this.m_Magnitudes, ref count2, magnitude, 10);
				}
				this.SortCandidatesByScore();
			}

			// Token: 0x060012E4 RID: 4836 RVA: 0x00059568 File Offset: 0x00057768
			public void RemoveCandidate(InputControl control)
			{
				if (control == null)
				{
					throw new ArgumentNullException("control");
				}
				int num = this.m_Candidates.IndexOf(control);
				if (num == -1)
				{
					return;
				}
				int count = this.m_Candidates.Count;
				this.m_Candidates.RemoveAt(num);
				this.m_Scores.EraseAtWithCapacity<float>(ref count, num);
			}

			// Token: 0x060012E5 RID: 4837 RVA: 0x000595BB File Offset: 0x000577BB
			public void Dispose()
			{
				this.UnhookOnEvent();
				this.UnhookOnAfterUpdate();
				this.m_Candidates.Dispose();
				this.m_LayoutCache.Clear();
			}

			// Token: 0x060012E6 RID: 4838 RVA: 0x000595E0 File Offset: 0x000577E0
			~RebindingOperation()
			{
				this.Dispose();
			}

			// Token: 0x060012E7 RID: 4839 RVA: 0x0005960C File Offset: 0x0005780C
			public InputActionRebindingExtensions.RebindingOperation Reset()
			{
				this.Cancel();
				this.m_ActionToRebind = null;
				this.m_BindingMask = null;
				this.m_ControlType = null;
				this.m_ExpectedLayout = default(InternedString);
				this.m_IncludePathCount = 0;
				this.m_ExcludePathCount = 0;
				this.m_TargetBindingIndex = -1;
				this.m_BindingGroupForNewBinding = null;
				this.m_CancelBinding = null;
				this.m_MagnitudeThreshold = 0.2f;
				this.m_Timeout = 0f;
				this.m_WaitSecondsAfterMatch = 0f;
				this.m_Flags = (InputActionRebindingExtensions.RebindingOperation.Flags)0;
				Dictionary<InputControl, float> startingActuations = this.m_StartingActuations;
				if (startingActuations != null)
				{
					startingActuations.Clear();
				}
				return this;
			}

			// Token: 0x060012E8 RID: 4840 RVA: 0x000596A4 File Offset: 0x000578A4
			private void HookOnEvent()
			{
				if ((this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.OnEventHooked) != (InputActionRebindingExtensions.RebindingOperation.Flags)0)
				{
					return;
				}
				if (this.m_OnEventDelegate == null)
				{
					this.m_OnEventDelegate = new Action<InputEventPtr, InputDevice>(this.OnEvent);
				}
				InputSystem.onEvent += this.m_OnEventDelegate;
				this.m_Flags |= InputActionRebindingExtensions.RebindingOperation.Flags.OnEventHooked;
			}

			// Token: 0x060012E9 RID: 4841 RVA: 0x000596F9 File Offset: 0x000578F9
			private void UnhookOnEvent()
			{
				if ((this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.OnEventHooked) == (InputActionRebindingExtensions.RebindingOperation.Flags)0)
				{
					return;
				}
				InputSystem.onEvent -= this.m_OnEventDelegate;
				this.m_Flags &= ~InputActionRebindingExtensions.RebindingOperation.Flags.OnEventHooked;
			}

			// Token: 0x060012EA RID: 4842 RVA: 0x0005972C File Offset: 0x0005792C
			private unsafe void OnEvent(InputEventPtr eventPtr, InputDevice device)
			{
				FourCC type = eventPtr.type;
				if (type != 1398030676 && type != 1145852993)
				{
					return;
				}
				bool flag = false;
				bool flag2 = false;
				InputControlExtensions.Enumerate enumerate = InputControlExtensions.Enumerate.IncludeSyntheticControls | InputControlExtensions.Enumerate.IncludeNonLeafControls;
				if ((this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.DontIgnoreNoisyControls) != (InputActionRebindingExtensions.RebindingOperation.Flags)0)
				{
					enumerate |= InputControlExtensions.Enumerate.IncludeNoisyControls;
				}
				foreach (InputControl inputControl in eventPtr.EnumerateControls(enumerate, device, 0f))
				{
					void* statePtrFromStateEventUnchecked = inputControl.GetStatePtrFromStateEventUnchecked(eventPtr, type);
					if (!string.IsNullOrEmpty(this.m_CancelBinding) && InputControlPath.Matches(this.m_CancelBinding, inputControl) && inputControl.HasValueChangeInState(statePtrFromStateEventUnchecked))
					{
						this.OnCancel();
						break;
					}
					if ((this.m_ExcludePathCount <= 0 || !InputActionRebindingExtensions.RebindingOperation.HavePathMatch(inputControl, this.m_ExcludePaths, this.m_ExcludePathCount)) && (this.m_IncludePathCount <= 0 || InputActionRebindingExtensions.RebindingOperation.HavePathMatch(inputControl, this.m_IncludePaths, this.m_IncludePathCount)) && (!(this.m_ControlType != null) || this.m_ControlType.IsInstanceOfType(inputControl)) && (this.m_ExpectedLayout.IsEmpty() || !(this.m_ExpectedLayout != inputControl.m_Layout) || InputControlLayout.s_Layouts.IsBasedOn(this.m_ExpectedLayout, inputControl.m_Layout)))
					{
						if (inputControl.CheckStateIsAtDefault(statePtrFromStateEventUnchecked, null))
						{
							if (!this.m_StartingActuations.ContainsKey(inputControl))
							{
								this.m_StartingActuations.Add(inputControl, 0f);
							}
							this.m_StartingActuations[inputControl] = 0f;
						}
						else
						{
							flag2 = true;
							float num = inputControl.EvaluateMagnitude(statePtrFromStateEventUnchecked);
							if (num >= 0f)
							{
								float magnitude;
								if (!this.m_StartingActuations.TryGetValue(inputControl, out magnitude))
								{
									magnitude = inputControl.magnitude;
									this.m_StartingActuations.Add(inputControl, magnitude);
								}
								if (Mathf.Abs(magnitude - num) < this.m_MagnitudeThreshold)
								{
									continue;
								}
							}
							float num2;
							if (this.m_OnComputeScore != null)
							{
								num2 = this.m_OnComputeScore(inputControl, eventPtr);
							}
							else
							{
								num2 = num;
								if (!inputControl.synthetic)
								{
									num2 += 1f;
								}
							}
							int num3 = this.m_Candidates.IndexOf(inputControl);
							if (num3 != -1)
							{
								if (this.m_Scores[num3] < num2)
								{
									flag = true;
									this.m_Scores[num3] = num2;
									if (this.m_WaitSecondsAfterMatch > 0f)
									{
										this.m_LastMatchTime = InputState.currentTime;
									}
								}
							}
							else
							{
								int count = this.m_Candidates.Count;
								int count2 = this.m_Candidates.Count;
								this.m_Candidates.Add(inputControl);
								ArrayHelpers.AppendWithCapacity<float>(ref this.m_Scores, ref count, num2, 10);
								ArrayHelpers.AppendWithCapacity<float>(ref this.m_Magnitudes, ref count2, num, 10);
								flag = true;
								if (this.m_WaitSecondsAfterMatch > 0f)
								{
									this.m_LastMatchTime = InputState.currentTime;
								}
							}
						}
					}
				}
				if (flag2 && (this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.SuppressMatchingEvents) != (InputActionRebindingExtensions.RebindingOperation.Flags)0)
				{
					eventPtr.handled = true;
				}
				if (flag && !this.canceled)
				{
					if (this.m_OnPotentialMatch != null)
					{
						this.SortCandidatesByScore();
						this.m_OnPotentialMatch(this);
						return;
					}
					if (this.m_WaitSecondsAfterMatch <= 0f)
					{
						this.OnComplete();
						return;
					}
					this.SortCandidatesByScore();
				}
			}

			// Token: 0x060012EB RID: 4843 RVA: 0x00059A8C File Offset: 0x00057C8C
			private void SortCandidatesByScore()
			{
				int count = this.m_Candidates.Count;
				if (count <= 1)
				{
					return;
				}
				for (int i = 1; i < count; i++)
				{
					int num = i;
					while (num > 0 && this.m_Scores[num - 1] < this.m_Scores[num])
					{
						int num2 = num - 1;
						this.m_Scores.SwapElements<float>(num, num2);
						this.m_Candidates.SwapElements(num, num2);
						this.m_Magnitudes.SwapElements<float>(num, num2);
						num--;
					}
				}
			}

			// Token: 0x060012EC RID: 4844 RVA: 0x00059B04 File Offset: 0x00057D04
			private static bool HavePathMatch(InputControl control, string[] paths, int pathCount)
			{
				for (int i = 0; i < pathCount; i++)
				{
					if (InputControlPath.MatchesPrefix(paths[i], control))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060012ED RID: 4845 RVA: 0x00059B2C File Offset: 0x00057D2C
			private void HookOnAfterUpdate()
			{
				if ((this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.OnAfterUpdateHooked) != (InputActionRebindingExtensions.RebindingOperation.Flags)0)
				{
					return;
				}
				if (this.m_OnAfterUpdateDelegate == null)
				{
					this.m_OnAfterUpdateDelegate = new Action(this.OnAfterUpdate);
				}
				InputSystem.onAfterUpdate += this.m_OnAfterUpdateDelegate;
				this.m_Flags |= InputActionRebindingExtensions.RebindingOperation.Flags.OnAfterUpdateHooked;
			}

			// Token: 0x060012EE RID: 4846 RVA: 0x00059B79 File Offset: 0x00057D79
			private void UnhookOnAfterUpdate()
			{
				if ((this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.OnAfterUpdateHooked) == (InputActionRebindingExtensions.RebindingOperation.Flags)0)
				{
					return;
				}
				InputSystem.onAfterUpdate -= this.m_OnAfterUpdateDelegate;
				this.m_Flags &= ~InputActionRebindingExtensions.RebindingOperation.Flags.OnAfterUpdateHooked;
			}

			// Token: 0x060012EF RID: 4847 RVA: 0x00059BA4 File Offset: 0x00057DA4
			private void OnAfterUpdate()
			{
				if (this.m_LastMatchTime < 0.0 && this.m_Timeout > 0f && InputState.currentTime - this.m_StartTime > (double)this.m_Timeout)
				{
					this.Cancel();
					return;
				}
				if (this.m_WaitSecondsAfterMatch <= 0f)
				{
					return;
				}
				if (this.m_LastMatchTime < 0.0)
				{
					return;
				}
				if (InputState.currentTime >= this.m_LastMatchTime + (double)this.m_WaitSecondsAfterMatch)
				{
					this.Complete();
				}
			}

			// Token: 0x060012F0 RID: 4848 RVA: 0x00059C28 File Offset: 0x00057E28
			private void OnComplete()
			{
				this.SortCandidatesByScore();
				if (this.m_Candidates.Count > 0)
				{
					InputControl inputControl = this.m_Candidates[0];
					string text = inputControl.path;
					if (this.m_OnGeneratePath != null)
					{
						string text2 = this.m_OnGeneratePath(inputControl);
						if (!string.IsNullOrEmpty(text2))
						{
							text = text2;
						}
						else if ((this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.DontGeneralizePathOfSelectedControl) == (InputActionRebindingExtensions.RebindingOperation.Flags)0)
						{
							text = this.GeneratePathForControl(inputControl);
						}
					}
					else if ((this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.DontGeneralizePathOfSelectedControl) == (InputActionRebindingExtensions.RebindingOperation.Flags)0)
					{
						text = this.GeneratePathForControl(inputControl);
					}
					if (this.m_OnApplyBinding != null)
					{
						this.m_OnApplyBinding(this, text);
					}
					else if ((this.m_Flags & InputActionRebindingExtensions.RebindingOperation.Flags.AddNewBinding) != (InputActionRebindingExtensions.RebindingOperation.Flags)0)
					{
						this.m_ActionToRebind.AddBinding(text, null, null, this.m_BindingGroupForNewBinding);
					}
					else if (this.m_TargetBindingIndex >= 0)
					{
						if (this.m_TargetBindingIndex >= this.m_ActionToRebind.bindings.Count)
						{
							throw new InvalidOperationException(string.Format("Target binding index {0} out of range for action '{1}' with {2} bindings", this.m_TargetBindingIndex, this.m_ActionToRebind, this.m_ActionToRebind.bindings.Count));
						}
						this.m_ActionToRebind.ApplyBindingOverride(this.m_TargetBindingIndex, text);
					}
					else if (this.m_BindingMask != null)
					{
						InputBinding value = this.m_BindingMask.Value;
						value.overridePath = text;
						this.m_ActionToRebind.ApplyBindingOverride(value);
					}
					else
					{
						this.m_ActionToRebind.ApplyBindingOverride(text, null, null);
					}
				}
				this.m_Flags |= InputActionRebindingExtensions.RebindingOperation.Flags.Completed;
				Action<InputActionRebindingExtensions.RebindingOperation> onComplete = this.m_OnComplete;
				if (onComplete != null)
				{
					onComplete(this);
				}
				this.ResetAfterMatchCompleted();
			}

			// Token: 0x060012F1 RID: 4849 RVA: 0x00059DC7 File Offset: 0x00057FC7
			private void OnCancel()
			{
				this.m_Flags |= InputActionRebindingExtensions.RebindingOperation.Flags.Canceled;
				Action<InputActionRebindingExtensions.RebindingOperation> onCancel = this.m_OnCancel;
				if (onCancel != null)
				{
					onCancel(this);
				}
				this.ResetAfterMatchCompleted();
			}

			// Token: 0x060012F2 RID: 4850 RVA: 0x00059DF0 File Offset: 0x00057FF0
			private void ResetAfterMatchCompleted()
			{
				this.m_Flags &= ~InputActionRebindingExtensions.RebindingOperation.Flags.Started;
				this.m_Candidates.Clear();
				this.m_Candidates.Capacity = 0;
				this.m_StartTime = -1.0;
				this.m_StartingActuations.Clear();
				this.UnhookOnEvent();
				this.UnhookOnAfterUpdate();
			}

			// Token: 0x060012F3 RID: 4851 RVA: 0x00059E49 File Offset: 0x00058049
			private void ThrowIfRebindInProgress()
			{
				if (this.started)
				{
					throw new InvalidOperationException("Cannot reconfigure rebinding while operation is in progress");
				}
			}

			// Token: 0x060012F4 RID: 4852 RVA: 0x00059E60 File Offset: 0x00058060
			private string GeneratePathForControl(InputControl control)
			{
				InputDevice device = control.device;
				InternedString internedString = InputControlLayout.s_Layouts.FindLayoutThatIntroducesControl(control, this.m_LayoutCache);
				if (this.m_PathBuilder == null)
				{
					this.m_PathBuilder = new StringBuilder();
				}
				else
				{
					this.m_PathBuilder.Length = 0;
				}
				control.BuildPath(internedString, this.m_PathBuilder);
				return this.m_PathBuilder.ToString();
			}

			// Token: 0x040007F0 RID: 2032
			public const float kDefaultMagnitudeThreshold = 0.2f;

			// Token: 0x040007F1 RID: 2033
			private InputAction m_ActionToRebind;

			// Token: 0x040007F2 RID: 2034
			private InputBinding? m_BindingMask;

			// Token: 0x040007F3 RID: 2035
			private Type m_ControlType;

			// Token: 0x040007F4 RID: 2036
			private InternedString m_ExpectedLayout;

			// Token: 0x040007F5 RID: 2037
			private int m_IncludePathCount;

			// Token: 0x040007F6 RID: 2038
			private string[] m_IncludePaths;

			// Token: 0x040007F7 RID: 2039
			private int m_ExcludePathCount;

			// Token: 0x040007F8 RID: 2040
			private string[] m_ExcludePaths;

			// Token: 0x040007F9 RID: 2041
			private int m_TargetBindingIndex = -1;

			// Token: 0x040007FA RID: 2042
			private string m_BindingGroupForNewBinding;

			// Token: 0x040007FB RID: 2043
			private string m_CancelBinding;

			// Token: 0x040007FC RID: 2044
			private float m_MagnitudeThreshold = 0.2f;

			// Token: 0x040007FD RID: 2045
			private float[] m_Scores;

			// Token: 0x040007FE RID: 2046
			private float[] m_Magnitudes;

			// Token: 0x040007FF RID: 2047
			private double m_LastMatchTime;

			// Token: 0x04000800 RID: 2048
			private double m_StartTime;

			// Token: 0x04000801 RID: 2049
			private float m_Timeout;

			// Token: 0x04000802 RID: 2050
			private float m_WaitSecondsAfterMatch;

			// Token: 0x04000803 RID: 2051
			private InputControlList<InputControl> m_Candidates;

			// Token: 0x04000804 RID: 2052
			private Action<InputActionRebindingExtensions.RebindingOperation> m_OnComplete;

			// Token: 0x04000805 RID: 2053
			private Action<InputActionRebindingExtensions.RebindingOperation> m_OnCancel;

			// Token: 0x04000806 RID: 2054
			private Action<InputActionRebindingExtensions.RebindingOperation> m_OnPotentialMatch;

			// Token: 0x04000807 RID: 2055
			private Func<InputControl, string> m_OnGeneratePath;

			// Token: 0x04000808 RID: 2056
			private Func<InputControl, InputEventPtr, float> m_OnComputeScore;

			// Token: 0x04000809 RID: 2057
			private Action<InputActionRebindingExtensions.RebindingOperation, string> m_OnApplyBinding;

			// Token: 0x0400080A RID: 2058
			private Action<InputEventPtr, InputDevice> m_OnEventDelegate;

			// Token: 0x0400080B RID: 2059
			private Action m_OnAfterUpdateDelegate;

			// Token: 0x0400080C RID: 2060
			private InputControlLayout.Cache m_LayoutCache;

			// Token: 0x0400080D RID: 2061
			private StringBuilder m_PathBuilder;

			// Token: 0x0400080E RID: 2062
			private InputActionRebindingExtensions.RebindingOperation.Flags m_Flags;

			// Token: 0x0400080F RID: 2063
			private Dictionary<InputControl, float> m_StartingActuations = new Dictionary<InputControl, float>();

			// Token: 0x0200026B RID: 619
			[Flags]
			private enum Flags
			{
				// Token: 0x04000CD1 RID: 3281
				Started = 1,
				// Token: 0x04000CD2 RID: 3282
				Completed = 2,
				// Token: 0x04000CD3 RID: 3283
				Canceled = 4,
				// Token: 0x04000CD4 RID: 3284
				OnEventHooked = 8,
				// Token: 0x04000CD5 RID: 3285
				OnAfterUpdateHooked = 16,
				// Token: 0x04000CD6 RID: 3286
				DontIgnoreNoisyControls = 64,
				// Token: 0x04000CD7 RID: 3287
				DontGeneralizePathOfSelectedControl = 128,
				// Token: 0x04000CD8 RID: 3288
				AddNewBinding = 256,
				// Token: 0x04000CD9 RID: 3289
				SuppressMatchingEvents = 512
			}
		}

		// Token: 0x02000171 RID: 369
		internal class DeferBindingResolutionWrapper : IDisposable
		{
			// Token: 0x060012F6 RID: 4854 RVA: 0x00059EEA File Offset: 0x000580EA
			public void Acquire()
			{
				InputActionMap.s_DeferBindingResolution++;
			}

			// Token: 0x060012F7 RID: 4855 RVA: 0x00059EF8 File Offset: 0x000580F8
			public void Dispose()
			{
				if (InputActionMap.s_DeferBindingResolution > 0)
				{
					InputActionMap.s_DeferBindingResolution--;
				}
				if (InputActionMap.s_DeferBindingResolution == 0)
				{
					InputActionState.DeferredResolutionOfBindings();
				}
			}
		}
	}
}
