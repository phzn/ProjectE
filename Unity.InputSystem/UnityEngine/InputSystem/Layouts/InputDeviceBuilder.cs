using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Layouts
{
	// Token: 0x02000111 RID: 273
	internal struct InputDeviceBuilder : IDisposable
	{
		// Token: 0x06000FB7 RID: 4023 RVA: 0x0004BAAC File Offset: 0x00049CAC
		public void Setup(InternedString layout, InternedString variants, InputDeviceDescription deviceDescription = default(InputDeviceDescription))
		{
			this.m_LayoutCacheRef = InputControlLayout.CacheRef();
			this.InstantiateLayout(layout, variants, default(InternedString), null);
			this.FinalizeControlHierarchy();
			this.m_StateOffsetToControlMap.Sort();
			this.m_Device.m_Description = deviceDescription;
			this.m_Device.m_StateOffsetToControlMap = this.m_StateOffsetToControlMap.ToArray();
			this.m_Device.CallFinishSetupRecursive();
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0004BB18 File Offset: 0x00049D18
		public InputDevice Finish()
		{
			InputDevice device = this.m_Device;
			int num = 0;
			using (ReadOnlyArray<InputControl>.Enumerator enumerator = device.allControls.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.isButton)
					{
						num++;
					}
				}
			}
			device.m_ButtonControlsCheckingPressState = new List<ButtonControl>(num);
			device.m_UpdatedButtons = new HashSet<int>(num);
			this.Reset();
			return device;
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0004BB9C File Offset: 0x00049D9C
		public void Dispose()
		{
			this.m_LayoutCacheRef.Dispose();
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0004BBA9 File Offset: 0x00049DA9
		private void Reset()
		{
			this.m_Device = null;
			Dictionary<string, InputControlLayout.ControlItem> childControlOverrides = this.m_ChildControlOverrides;
			if (childControlOverrides != null)
			{
				childControlOverrides.Clear();
			}
			List<uint> stateOffsetToControlMap = this.m_StateOffsetToControlMap;
			if (stateOffsetToControlMap == null)
			{
				return;
			}
			stateOffsetToControlMap.Clear();
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0004BBD4 File Offset: 0x00049DD4
		private InputControl InstantiateLayout(InternedString layout, InternedString variants, InternedString name, InputControl parent)
		{
			InputControlLayout inputControlLayout = InputDeviceBuilder.FindOrLoadLayout(layout);
			return this.InstantiateLayout(inputControlLayout, variants, name, parent);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0004BBF8 File Offset: 0x00049DF8
		private InputControl InstantiateLayout(InputControlLayout layout, InternedString variants, InternedString name, InputControl parent)
		{
			InputControl inputControl = Activator.CreateInstance(layout.type) as InputControl;
			if (inputControl == null)
			{
				throw new InvalidOperationException(string.Format("Type '{0}' referenced by layout '{1}' is not an InputControl", layout.type.Name, layout.name));
			}
			InputDevice inputDevice = inputControl as InputDevice;
			if (inputDevice != null)
			{
				if (parent != null)
				{
					throw new InvalidOperationException(string.Format("Cannot instantiate device layout '{0}' as child of '{1}'; devices must be added at root", layout.name, parent.path));
				}
				this.m_Device = inputDevice;
				this.m_Device.m_StateBlock.byteOffset = 0U;
				this.m_Device.m_StateBlock.bitOffset = 0U;
				this.m_Device.m_StateBlock.format = layout.stateFormat;
				this.m_Device.m_AliasesForEachControl = null;
				this.m_Device.m_ChildrenForEachControl = null;
				this.m_Device.m_UpdatedButtons = null;
				this.m_Device.m_UsagesForEachControl = null;
				this.m_Device.m_UsageToControl = null;
				bool? flag = layout.m_UpdateBeforeRender;
				bool flag2 = true;
				if ((flag.GetValueOrDefault() == flag2) & (flag != null))
				{
					this.m_Device.m_DeviceFlags |= InputDevice.DeviceFlags.UpdateBeforeRender;
				}
				if (layout.canRunInBackground != null)
				{
					this.m_Device.m_DeviceFlags |= InputDevice.DeviceFlags.CanRunInBackgroundHasBeenQueried;
					flag = layout.canRunInBackground;
					flag2 = true;
					if ((flag.GetValueOrDefault() == flag2) & (flag != null))
					{
						this.m_Device.m_DeviceFlags |= InputDevice.DeviceFlags.CanRunInBackground;
					}
				}
			}
			else if (parent == null)
			{
				throw new InvalidOperationException(string.Format("Toplevel layout used with InputDeviceBuilder must be a device layout; '{0}' is a control layout", layout.name));
			}
			if (name.IsEmpty())
			{
				name = layout.name;
				int num = name.ToString().LastIndexOf(':');
				if (num != -1)
				{
					name = new InternedString(name.ToString().Substring(num + 1));
				}
			}
			if (name.ToString().IndexOf('/') != -1)
			{
				name = new InternedString(name.ToString().CleanSlashes());
			}
			if (variants.IsEmpty())
			{
				variants = layout.variants;
				if (variants.IsEmpty())
				{
					variants = InputControlLayout.DefaultVariant;
				}
			}
			inputControl.m_Name = name;
			inputControl.m_DisplayNameFromLayout = layout.m_DisplayName;
			inputControl.m_Layout = layout.name;
			inputControl.m_Variants = variants;
			inputControl.m_Parent = parent;
			inputControl.m_Device = this.m_Device;
			if (inputControl is InputDevice)
			{
				inputControl.noisy = layout.isNoisy;
			}
			bool flag3 = false;
			try
			{
				this.AddChildControls(layout, variants, inputControl, ref flag3);
			}
			catch
			{
				throw;
			}
			InputDeviceBuilder.ComputeStateLayout(inputControl);
			if (flag3)
			{
				InputControlLayout.ControlItem[] controls = layout.m_Controls;
				for (int i = 0; i < controls.Length; i++)
				{
					ref InputControlLayout.ControlItem ptr = ref controls[i];
					if (!string.IsNullOrEmpty(ptr.useStateFrom))
					{
						InputDeviceBuilder.ApplyUseStateFrom(inputControl, ref ptr, layout);
					}
				}
			}
			return inputControl;
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0004BEF0 File Offset: 0x0004A0F0
		private void AddChildControls(InputControlLayout layout, InternedString variants, InputControl parent, ref bool haveChildrenUsingStateFromOtherControls)
		{
			InputControlLayout.ControlItem[] controls = layout.m_Controls;
			if (controls == null)
			{
				return;
			}
			int num = 0;
			bool flag = false;
			for (int i = 0; i < controls.Length; i++)
			{
				if (controls[i].variants.IsEmpty() || StringHelpers.CharacterSeparatedListsHaveAtLeastOneCommonElement(controls[i].variants, variants, ";"[0]))
				{
					if (controls[i].isModifyingExistingControl)
					{
						if (controls[i].isArray)
						{
							throw new NotSupportedException(string.Format("Control '{0}' in layout '{1}' is modifying the child of another control but is marked as an array", controls[i].name, layout.name));
						}
						flag = true;
						this.InsertChildControlOverride(parent, ref controls[i]);
					}
					else if (controls[i].isArray)
					{
						num += controls[i].arraySize;
					}
					else
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				parent.m_ChildCount = 0;
				parent.m_ChildStartIndex = 0;
				haveChildrenUsingStateFromOtherControls = false;
				return;
			}
			int num2 = ArrayHelpers.GrowBy<InputControl>(ref this.m_Device.m_ChildrenForEachControl, num);
			int num3 = num2;
			foreach (InputControlLayout.ControlItem controlItem in controls)
			{
				if (!controlItem.isModifyingExistingControl && (controlItem.variants.IsEmpty() || StringHelpers.CharacterSeparatedListsHaveAtLeastOneCommonElement(controlItem.variants, variants, ";"[0])))
				{
					if (controlItem.isArray)
					{
						for (int k = 0; k < controlItem.arraySize; k++)
						{
							string text = controlItem.name + k.ToString();
							InputControl inputControl = this.AddChildControl(layout, variants, parent, ref haveChildrenUsingStateFromOtherControls, controlItem, num3, text);
							num3++;
							if (inputControl.m_StateBlock.byteOffset != 4294967295U)
							{
								InputControl inputControl2 = inputControl;
								inputControl2.m_StateBlock.byteOffset = inputControl2.m_StateBlock.byteOffset + (uint)(k * (int)inputControl.m_StateBlock.alignedSizeInBytes);
							}
						}
					}
					else
					{
						this.AddChildControl(layout, variants, parent, ref haveChildrenUsingStateFromOtherControls, controlItem, num3, null);
						num3++;
					}
				}
			}
			parent.m_ChildCount = num;
			parent.m_ChildStartIndex = num2;
			if (flag)
			{
				for (int l = 0; l < controls.Length; l++)
				{
					InputControlLayout.ControlItem controlItem2 = controls[l];
					if (controlItem2.isModifyingExistingControl && (controlItem2.variants.IsEmpty() || StringHelpers.CharacterSeparatedListsHaveAtLeastOneCommonElement(controls[l].variants, variants, ";"[0])))
					{
						this.AddChildControlIfMissing(layout, variants, parent, ref haveChildrenUsingStateFromOtherControls, ref controlItem2);
					}
				}
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0004C19C File Offset: 0x0004A39C
		private InputControl AddChildControl(InputControlLayout layout, InternedString variants, InputControl parent, ref bool haveChildrenUsingStateFromOtherControls, InputControlLayout.ControlItem controlItem, int childIndex, string nameOverride = null)
		{
			InternedString internedString = ((nameOverride != null) ? new InternedString(nameOverride) : controlItem.name);
			if (string.IsNullOrEmpty(controlItem.layout))
			{
				throw new InvalidOperationException(string.Format("Layout has not been set on control '{0}' in '{1}'", controlItem.name, layout.name));
			}
			if (this.m_ChildControlOverrides != null)
			{
				string text = this.ChildControlOverridePath(parent, internedString);
				InputControlLayout.ControlItem controlItem2;
				if (this.m_ChildControlOverrides.TryGetValue(text, out controlItem2))
				{
					controlItem = controlItem2.Merge(controlItem);
				}
			}
			InternedString layout2 = controlItem.layout;
			InputControl inputControl;
			try
			{
				inputControl = this.InstantiateLayout(layout2, variants, internedString, parent);
			}
			catch (InputControlLayout.LayoutNotFoundException ex)
			{
				throw new InputControlLayout.LayoutNotFoundException(string.Format("Cannot find layout '{0}' used in control '{1}' of layout '{2}'", ex.layout, internedString, layout.name), ex);
			}
			this.m_Device.m_ChildrenForEachControl[childIndex] = inputControl;
			inputControl.noisy = controlItem.isNoisy;
			inputControl.synthetic = controlItem.isSynthetic;
			inputControl.usesStateFromOtherControl = !string.IsNullOrEmpty(controlItem.useStateFrom);
			inputControl.dontReset = (inputControl.noisy || controlItem.dontReset) && !inputControl.usesStateFromOtherControl;
			if (inputControl.noisy)
			{
				this.m_Device.noisy = true;
			}
			inputControl.isButton = inputControl is ButtonControl;
			if (inputControl.dontReset)
			{
				this.m_Device.hasDontResetControls = true;
			}
			inputControl.m_DisplayNameFromLayout = controlItem.displayName;
			inputControl.m_ShortDisplayNameFromLayout = controlItem.shortDisplayName;
			inputControl.m_DefaultState = controlItem.defaultState;
			if (!inputControl.m_DefaultState.isEmpty)
			{
				this.m_Device.hasControlsWithDefaultState = true;
			}
			if (!controlItem.minValue.isEmpty)
			{
				inputControl.m_MinValue = controlItem.minValue;
			}
			if (!controlItem.maxValue.isEmpty)
			{
				inputControl.m_MaxValue = controlItem.maxValue;
			}
			if (!inputControl.usesStateFromOtherControl)
			{
				inputControl.m_StateBlock.byteOffset = controlItem.offset;
				inputControl.m_StateBlock.bitOffset = controlItem.bit;
				if (controlItem.sizeInBits != 0U)
				{
					inputControl.m_StateBlock.sizeInBits = controlItem.sizeInBits;
				}
				if (controlItem.format != 0)
				{
					InputDeviceBuilder.SetFormat(inputControl, controlItem);
				}
			}
			else
			{
				inputControl.m_StateBlock.sizeInBits = uint.MaxValue;
				haveChildrenUsingStateFromOtherControls = true;
			}
			ReadOnlyArray<InternedString> usages = controlItem.usages;
			if (usages.Count > 0)
			{
				int count = usages.Count;
				int num = ArrayHelpers.AppendToImmutable<InternedString>(ref this.m_Device.m_UsagesForEachControl, usages.m_Array);
				inputControl.m_UsageStartIndex = num;
				inputControl.m_UsageCount = count;
				ArrayHelpers.GrowBy<InputControl>(ref this.m_Device.m_UsageToControl, count);
				for (int i = 0; i < count; i++)
				{
					this.m_Device.m_UsageToControl[num + i] = inputControl;
				}
			}
			if (controlItem.aliases.Count > 0)
			{
				int count2 = controlItem.aliases.Count;
				int num2 = ArrayHelpers.AppendToImmutable<InternedString>(ref this.m_Device.m_AliasesForEachControl, controlItem.aliases.m_Array);
				inputControl.m_AliasStartIndex = num2;
				inputControl.m_AliasCount = count2;
			}
			if (controlItem.parameters.Count > 0)
			{
				NamedValue.ApplyAllToObject<ReadOnlyArray<NamedValue>>(inputControl, controlItem.parameters);
			}
			if (controlItem.processors.Count > 0)
			{
				InputDeviceBuilder.AddProcessors(inputControl, ref controlItem, layout.name);
			}
			return inputControl;
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0004C518 File Offset: 0x0004A718
		private void InsertChildControlOverride(InputControl parent, ref InputControlLayout.ControlItem controlItem)
		{
			if (this.m_ChildControlOverrides == null)
			{
				this.m_ChildControlOverrides = new Dictionary<string, InputControlLayout.ControlItem>();
			}
			string text = this.ChildControlOverridePath(parent, controlItem.name);
			InputControlLayout.ControlItem controlItem2;
			if (!this.m_ChildControlOverrides.TryGetValue(text, out controlItem2))
			{
				this.m_ChildControlOverrides[text] = controlItem;
				return;
			}
			controlItem2 = controlItem2.Merge(controlItem);
			this.m_ChildControlOverrides[text] = controlItem2;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0004C584 File Offset: 0x0004A784
		private string ChildControlOverridePath(InputControl parent, InternedString controlName)
		{
			string text = controlName.ToLower();
			for (InputControl inputControl = parent; inputControl != this.m_Device; inputControl = inputControl.m_Parent)
			{
				text = inputControl.m_Name.ToLower() + "/" + text;
			}
			return text;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0004C5C8 File Offset: 0x0004A7C8
		private void AddChildControlIfMissing(InputControlLayout layout, InternedString variants, InputControl parent, ref bool haveChildrenUsingStateFromOtherControls, ref InputControlLayout.ControlItem controlItem)
		{
			InputControl inputControl = InputControlPath.TryFindChild(parent, controlItem.name, 0);
			if (inputControl != null)
			{
				return;
			}
			inputControl = this.InsertChildControl(layout, variants, parent, ref haveChildrenUsingStateFromOtherControls, ref controlItem);
			if (inputControl.parent != parent)
			{
				InputDeviceBuilder.ComputeStateLayout(inputControl.parent);
			}
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0004C610 File Offset: 0x0004A810
		private InputControl InsertChildControl(InputControlLayout layout, InternedString variant, InputControl parent, ref bool haveChildrenUsingStateFromOtherControls, ref InputControlLayout.ControlItem controlItem)
		{
			string text = controlItem.name.ToString();
			int num = text.LastIndexOf('/');
			if (num == -1)
			{
				throw new InvalidOperationException("InsertChildControl has to be called with a slash-separated path");
			}
			string text2 = text.Substring(0, num);
			InputControl inputControl = InputControlPath.TryFindChild(parent, text2, 0);
			if (inputControl == null)
			{
				throw new InvalidOperationException(string.Format("Cannot find parent '{0}' of control '{1}' in layout '{2}'", text2, controlItem.name, layout.name));
			}
			string text3 = text.Substring(num + 1);
			if (text3.Length == 0)
			{
				throw new InvalidOperationException(string.Format("Path cannot end in '/' (control '{0}' in layout '{1}')", controlItem.name, layout.name));
			}
			int num2 = inputControl.m_ChildStartIndex;
			if (num2 == 0)
			{
				num2 = this.m_Device.m_ChildrenForEachControl.LengthSafe<InputControl>();
				inputControl.m_ChildStartIndex = num2;
			}
			int num3 = num2 + inputControl.m_ChildCount;
			InputDeviceBuilder.ShiftChildIndicesInHierarchyOneUp(this.m_Device, num3, inputControl);
			ArrayHelpers.InsertAt<InputControl>(ref this.m_Device.m_ChildrenForEachControl, num3, null);
			inputControl.m_ChildCount++;
			return this.AddChildControl(layout, variant, inputControl, ref haveChildrenUsingStateFromOtherControls, controlItem, num3, text3);
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0004C738 File Offset: 0x0004A938
		private static void ApplyUseStateFrom(InputControl parent, ref InputControlLayout.ControlItem controlItem, InputControlLayout layout)
		{
			InputControl inputControl = InputControlPath.TryFindChild(parent, controlItem.name, 0);
			InputControl inputControl2 = InputControlPath.TryFindChild(parent, controlItem.useStateFrom, 0);
			if (inputControl2 == null)
			{
				throw new InvalidOperationException(string.Format("Cannot find control '{0}' referenced in 'useStateFrom' of control '{1}' in layout '{2}'", controlItem.useStateFrom, controlItem.name, layout.name));
			}
			inputControl.m_StateBlock = inputControl2.m_StateBlock;
			inputControl.usesStateFromOtherControl = true;
			inputControl.dontReset = inputControl2.dontReset;
			if (inputControl.parent != inputControl2.parent)
			{
				for (InputControl inputControl3 = inputControl2.parent; inputControl3 != parent; inputControl3 = inputControl3.parent)
				{
					InputControl inputControl4 = inputControl;
					inputControl4.m_StateBlock.byteOffset = inputControl4.m_StateBlock.byteOffset + inputControl3.m_StateBlock.byteOffset;
				}
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0004C7F4 File Offset: 0x0004A9F4
		private static void ShiftChildIndicesInHierarchyOneUp(InputDevice device, int startIndex, InputControl exceptControl)
		{
			InputControl[] childrenForEachControl = device.m_ChildrenForEachControl;
			int num = childrenForEachControl.Length;
			for (int i = 0; i < num; i++)
			{
				InputControl inputControl = childrenForEachControl[i];
				if (inputControl != null && inputControl != exceptControl && inputControl.m_ChildStartIndex >= startIndex)
				{
					inputControl.m_ChildStartIndex++;
				}
			}
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0004C83C File Offset: 0x0004AA3C
		private void SetDisplayName(InputControl control, string longDisplayNameFromLayout, string shortDisplayNameFromLayout, bool shortName)
		{
			string text = (shortName ? shortDisplayNameFromLayout : longDisplayNameFromLayout);
			if (string.IsNullOrEmpty(text))
			{
				if (shortName)
				{
					if (control.parent == null || control.parent == control.device)
					{
						control.m_ShortDisplayNameFromLayout = null;
						return;
					}
					if (this.m_StringBuilder == null)
					{
						this.m_StringBuilder = new StringBuilder();
					}
					this.m_StringBuilder.Length = 0;
					InputDeviceBuilder.AddParentDisplayNameRecursive(control.parent, this.m_StringBuilder, true);
					if (this.m_StringBuilder.Length == 0)
					{
						control.m_ShortDisplayNameFromLayout = null;
						return;
					}
					if (!string.IsNullOrEmpty(longDisplayNameFromLayout))
					{
						this.m_StringBuilder.Append(longDisplayNameFromLayout);
					}
					else
					{
						this.m_StringBuilder.Append(control.name);
					}
					control.m_ShortDisplayNameFromLayout = this.m_StringBuilder.ToString();
					return;
				}
				else
				{
					text = control.name;
				}
			}
			if (control.parent != null && control.parent != control.device)
			{
				if (this.m_StringBuilder == null)
				{
					this.m_StringBuilder = new StringBuilder();
				}
				this.m_StringBuilder.Length = 0;
				InputDeviceBuilder.AddParentDisplayNameRecursive(control.parent, this.m_StringBuilder, shortName);
				this.m_StringBuilder.Append(text);
				text = this.m_StringBuilder.ToString();
			}
			if (shortName)
			{
				control.m_ShortDisplayNameFromLayout = text;
				return;
			}
			control.m_DisplayNameFromLayout = text;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0004C984 File Offset: 0x0004AB84
		private static void AddParentDisplayNameRecursive(InputControl control, StringBuilder stringBuilder, bool shortName)
		{
			if (control.parent != null && control.parent != control.device)
			{
				InputDeviceBuilder.AddParentDisplayNameRecursive(control.parent, stringBuilder, shortName);
			}
			if (shortName)
			{
				string text = control.shortDisplayName;
				if (string.IsNullOrEmpty(text))
				{
					text = control.displayName;
				}
				stringBuilder.Append(text);
			}
			else
			{
				stringBuilder.Append(control.displayName);
			}
			stringBuilder.Append(' ');
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0004C9F0 File Offset: 0x0004ABF0
		private static void AddProcessors(InputControl control, ref InputControlLayout.ControlItem controlItem, string layoutName)
		{
			int count = controlItem.processors.Count;
			for (int i = 0; i < count; i++)
			{
				string name = controlItem.processors[i].name;
				Type type = InputProcessor.s_Processors.LookupTypeRegistration(name);
				if (type == null)
				{
					throw new InvalidOperationException(string.Format("Cannot find processor '{0}' referenced by control '{1}' in layout '{2}'", name, controlItem.name, layoutName));
				}
				object obj = Activator.CreateInstance(type);
				ReadOnlyArray<NamedValue> parameters = controlItem.processors[i].parameters;
				if (parameters.Count > 0)
				{
					NamedValue.ApplyAllToObject<ReadOnlyArray<NamedValue>>(obj, parameters);
				}
				control.AddProcessor(obj);
			}
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0004CAA4 File Offset: 0x0004ACA4
		private static void SetFormat(InputControl control, InputControlLayout.ControlItem controlItem)
		{
			control.m_StateBlock.format = controlItem.format;
			if (controlItem.sizeInBits == 0U)
			{
				int sizeOfPrimitiveFormatInBits = InputStateBlock.GetSizeOfPrimitiveFormatInBits(controlItem.format);
				if (sizeOfPrimitiveFormatInBits != -1)
				{
					control.m_StateBlock.sizeInBits = (uint)sizeOfPrimitiveFormatInBits;
				}
			}
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0004CAE9 File Offset: 0x0004ACE9
		private static InputControlLayout FindOrLoadLayout(string name)
		{
			return InputControlLayout.cache.FindOrLoadLayout(name, true);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0004CAF8 File Offset: 0x0004ACF8
		private static void ComputeStateLayout(InputControl control)
		{
			ReadOnlyArray<InputControl> children = control.children;
			if (control.m_StateBlock.sizeInBits == 0U && control.m_StateBlock.format != 0)
			{
				int sizeOfPrimitiveFormatInBits = InputStateBlock.GetSizeOfPrimitiveFormatInBits(control.m_StateBlock.format);
				if (sizeOfPrimitiveFormatInBits != -1)
				{
					control.m_StateBlock.sizeInBits = (uint)sizeOfPrimitiveFormatInBits;
				}
			}
			if (control.m_StateBlock.sizeInBits == 0U && children.Count == 0)
			{
				throw new InvalidOperationException(string.Concat(new string[] { "Control '", control.path, "' with layout '", control.layout, "' has no size set and has no children to compute size from" }));
			}
			if (children.Count == 0)
			{
				return;
			}
			uint num = 0U;
			foreach (InputControl inputControl in children)
			{
				if (inputControl.m_StateBlock.sizeInBits != 4294967295U)
				{
					uint sizeInBits = inputControl.m_StateBlock.sizeInBits;
					if (sizeInBits == 0U || sizeInBits == 4294967295U)
					{
						throw new InvalidOperationException(string.Concat(new string[] { "Child '", inputControl.name, "' of '", control.name, "' has no size set!" }));
					}
					if (inputControl.m_StateBlock.byteOffset != 4294967295U && inputControl.m_StateBlock.byteOffset != 4294967294U)
					{
						if (inputControl.m_StateBlock.bitOffset == 4294967295U)
						{
							inputControl.m_StateBlock.bitOffset = 0U;
						}
						uint num2 = MemoryHelpers.ComputeFollowingByteOffset(inputControl.m_StateBlock.byteOffset, inputControl.m_StateBlock.bitOffset + sizeInBits);
						if (num2 > num)
						{
							num = num2;
						}
					}
				}
			}
			uint num3 = num;
			InputControl inputControl2 = null;
			uint num4 = 0U;
			foreach (InputControl inputControl3 in children)
			{
				if ((inputControl3.m_StateBlock.byteOffset == 4294967295U || inputControl3.m_StateBlock.byteOffset == 4294967294U) && inputControl3.m_StateBlock.sizeInBits != 4294967295U)
				{
					bool flag = inputControl3.m_StateBlock.sizeInBits % 8U > 0U;
					if (flag)
					{
						if (inputControl2 == null)
						{
							inputControl2 = inputControl3;
						}
						if (inputControl3.m_StateBlock.bitOffset == 4294967295U || inputControl3.m_StateBlock.bitOffset == 4294967294U)
						{
							inputControl3.m_StateBlock.bitOffset = num4;
							num4 += inputControl3.m_StateBlock.sizeInBits;
						}
						else
						{
							uint num5 = inputControl3.m_StateBlock.bitOffset + inputControl3.m_StateBlock.sizeInBits;
							if (num5 > num4)
							{
								num4 = num5;
							}
						}
					}
					else
					{
						if (inputControl2 != null)
						{
							num3 = MemoryHelpers.ComputeFollowingByteOffset(num3, num4);
							inputControl2 = null;
						}
						if (inputControl3.m_StateBlock.bitOffset == 4294967295U)
						{
							inputControl3.m_StateBlock.bitOffset = 0U;
						}
						num3 = MemoryHelpers.AlignNatural(num3, inputControl3.m_StateBlock.alignedSizeInBytes);
					}
					inputControl3.m_StateBlock.byteOffset = num3;
					if (!flag)
					{
						num3 = MemoryHelpers.ComputeFollowingByteOffset(num3, inputControl3.m_StateBlock.sizeInBits);
					}
				}
			}
			if (inputControl2 != null)
			{
				num3 = MemoryHelpers.ComputeFollowingByteOffset(num3, num4);
			}
			uint num6 = num3;
			control.m_StateBlock.sizeInBits = num6 * 8U;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0004CE50 File Offset: 0x0004B050
		private void FinalizeControlHierarchy()
		{
			if (this.m_StateOffsetToControlMap == null)
			{
				this.m_StateOffsetToControlMap = new List<uint>();
			}
			if ((long)this.m_Device.allControls.Count > 1024L)
			{
				throw new NotSupportedException(string.Format("Device '{0}' exceeds maximum supported control count of {1} (has {2} controls)", this.m_Device, 1024U, this.m_Device.allControls.Count));
			}
			InputDevice.ControlBitRangeNode controlBitRangeNode = new InputDevice.ControlBitRangeNode((ushort)(this.m_Device.m_StateBlock.sizeInBits - 1U));
			this.m_Device.m_ControlTreeNodes = new InputDevice.ControlBitRangeNode[1];
			this.m_Device.m_ControlTreeNodes[0] = controlBitRangeNode;
			int num = 0;
			this.FinalizeControlHierarchyRecursive(this.m_Device, -1, this.m_Device.m_ChildrenForEachControl, false, false, ref num);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0004CF20 File Offset: 0x0004B120
		private void FinalizeControlHierarchyRecursive(InputControl control, int controlIndex, InputControl[] allControls, bool noisy, bool dontReset, ref int controlIndiciesNextFreeIndex)
		{
			if (control.m_ChildCount == 0)
			{
				if (control.m_StateBlock.effectiveBitOffset >= 8192U)
				{
					throw new NotSupportedException(string.Format("Control '{0}' exceeds maximum supported state bit offset of {1} (bit offset {2})", control, 8191U, control.stateBlock.effectiveBitOffset));
				}
				if (control.m_StateBlock.sizeInBits >= 512U)
				{
					throw new NotSupportedException(string.Format("Control '{0}' exceeds maximum supported state bit size of {1} (bit offset {2})", control, 511U, control.stateBlock.sizeInBits));
				}
			}
			if (control != this.m_Device)
			{
				this.InsertControlBitRangeNode(ref this.m_Device.m_ControlTreeNodes[0], control, ref controlIndiciesNextFreeIndex, 0);
			}
			if (control.m_ChildCount == 0)
			{
				this.m_StateOffsetToControlMap.Add(InputDevice.EncodeStateOffsetToControlMapEntry((uint)controlIndex, control.m_StateBlock.effectiveBitOffset, control.m_StateBlock.sizeInBits));
			}
			string displayNameFromLayout = control.m_DisplayNameFromLayout;
			string shortDisplayNameFromLayout = control.m_ShortDisplayNameFromLayout;
			this.SetDisplayName(control, displayNameFromLayout, shortDisplayNameFromLayout, false);
			this.SetDisplayName(control, displayNameFromLayout, shortDisplayNameFromLayout, true);
			if (control != control.device)
			{
				if (noisy)
				{
					control.noisy = true;
				}
				else
				{
					noisy = control.noisy;
				}
				if (dontReset)
				{
					control.dontReset = true;
				}
				else
				{
					dontReset = control.dontReset;
				}
			}
			uint byteOffset = control.m_StateBlock.byteOffset;
			int childCount = control.m_ChildCount;
			int childStartIndex = control.m_ChildStartIndex;
			for (int i = 0; i < childCount; i++)
			{
				int num = childStartIndex + i;
				InputControl inputControl = allControls[num];
				InputControl inputControl2 = inputControl;
				inputControl2.m_StateBlock.byteOffset = inputControl2.m_StateBlock.byteOffset + byteOffset;
				this.FinalizeControlHierarchyRecursive(inputControl, num, allControls, noisy, dontReset, ref controlIndiciesNextFreeIndex);
			}
			control.isSetupFinished = true;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0004D0C8 File Offset: 0x0004B2C8
		private void InsertControlBitRangeNode(ref InputDevice.ControlBitRangeNode parent, InputControl control, ref int controlIndiciesNextFreeIndex, ushort startOffset)
		{
			InputDevice.ControlBitRangeNode controlBitRangeNode;
			InputDevice.ControlBitRangeNode controlBitRangeNode2;
			if (parent.leftChildIndex == -1)
			{
				ushort bestMidPoint = this.GetBestMidPoint(parent, startOffset);
				controlBitRangeNode = new InputDevice.ControlBitRangeNode(bestMidPoint);
				controlBitRangeNode2 = new InputDevice.ControlBitRangeNode(parent.endBitOffset);
				this.AddChildren(ref parent, controlBitRangeNode, controlBitRangeNode2);
			}
			else
			{
				controlBitRangeNode = this.m_Device.m_ControlTreeNodes[(int)parent.leftChildIndex];
				controlBitRangeNode2 = this.m_Device.m_ControlTreeNodes[(int)(parent.leftChildIndex + 1)];
			}
			if (control.m_StateBlock.effectiveBitOffset < (uint)controlBitRangeNode.endBitOffset && control.m_StateBlock.effectiveBitOffset + control.m_StateBlock.sizeInBits > (uint)controlBitRangeNode.endBitOffset)
			{
				this.AddControlToNode(control, ref controlIndiciesNextFreeIndex, (int)parent.leftChildIndex);
				this.AddControlToNode(control, ref controlIndiciesNextFreeIndex, (int)(parent.leftChildIndex + 1));
				return;
			}
			if (control.m_StateBlock.effectiveBitOffset == (uint)startOffset && control.m_StateBlock.effectiveBitOffset + control.m_StateBlock.sizeInBits == (uint)controlBitRangeNode.endBitOffset)
			{
				this.AddControlToNode(control, ref controlIndiciesNextFreeIndex, (int)parent.leftChildIndex);
				return;
			}
			if (control.m_StateBlock.effectiveBitOffset == (uint)controlBitRangeNode.endBitOffset && control.m_StateBlock.effectiveBitOffset + control.m_StateBlock.sizeInBits == (uint)controlBitRangeNode2.endBitOffset)
			{
				this.AddControlToNode(control, ref controlIndiciesNextFreeIndex, (int)(parent.leftChildIndex + 1));
				return;
			}
			if (control.m_StateBlock.effectiveBitOffset < (uint)controlBitRangeNode.endBitOffset)
			{
				this.InsertControlBitRangeNode(ref this.m_Device.m_ControlTreeNodes[(int)parent.leftChildIndex], control, ref controlIndiciesNextFreeIndex, startOffset);
				return;
			}
			this.InsertControlBitRangeNode(ref this.m_Device.m_ControlTreeNodes[(int)(parent.leftChildIndex + 1)], control, ref controlIndiciesNextFreeIndex, controlBitRangeNode.endBitOffset);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0004D268 File Offset: 0x0004B468
		private ushort GetBestMidPoint(InputDevice.ControlBitRangeNode parent, ushort startOffset)
		{
			ushort num = startOffset + ((parent.endBitOffset - startOffset - 1) / 2 + 1);
			ushort num2 = ushort.MaxValue;
			ushort num3 = ushort.MaxValue;
			InputControl[] array = this.m_Device.m_ChildrenForEachControl;
			for (int i = 0; i < array.Length; i++)
			{
				InputStateBlock stateBlock = array[i].m_StateBlock;
				if (stateBlock.effectiveBitOffset + stateBlock.sizeInBits - 1U >= (uint)startOffset && stateBlock.effectiveBitOffset < (uint)parent.endBitOffset && (ulong)stateBlock.sizeInBits <= (ulong)((long)(parent.endBitOffset - startOffset)) && stateBlock.effectiveBitOffset != (uint)startOffset && stateBlock.effectiveBitOffset + stateBlock.sizeInBits != (uint)parent.endBitOffset)
				{
					if (Math.Abs((long)((ulong)(stateBlock.effectiveBitOffset + stateBlock.sizeInBits) - (ulong)((long)num))) < (long)Math.Abs((int)(num2 - num)) && stateBlock.effectiveBitOffset + stateBlock.sizeInBits < (uint)parent.endBitOffset)
					{
						num2 = (ushort)(stateBlock.effectiveBitOffset + stateBlock.sizeInBits);
					}
					if (Math.Abs((long)((ulong)stateBlock.effectiveBitOffset - (ulong)((long)num))) < (long)Math.Abs((int)(num3 - num)) && stateBlock.effectiveBitOffset >= (uint)startOffset)
					{
						num3 = (ushort)stateBlock.effectiveBitOffset;
					}
				}
			}
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			foreach (InputControl inputControl in this.m_Device.m_ChildrenForEachControl)
			{
				if (num3 != 65535 && (uint)num3 > inputControl.m_StateBlock.effectiveBitOffset && (uint)num3 < inputControl.m_StateBlock.effectiveBitOffset + inputControl.m_StateBlock.sizeInBits)
				{
					num5++;
				}
				if (num2 != 65535 && (uint)num2 > inputControl.m_StateBlock.effectiveBitOffset && (uint)num2 < inputControl.m_StateBlock.effectiveBitOffset + inputControl.m_StateBlock.sizeInBits)
				{
					num6++;
				}
				if ((uint)num > inputControl.m_StateBlock.effectiveBitOffset && (uint)num < inputControl.m_StateBlock.effectiveBitOffset + inputControl.m_StateBlock.sizeInBits)
				{
					num4++;
				}
			}
			if (num2 != 65535 && num6 <= num5 && num6 <= num4)
			{
				return num2;
			}
			if (num3 != 65535 && num5 <= num6 && num5 <= num4)
			{
				return num3;
			}
			return num;
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0004D4A4 File Offset: 0x0004B6A4
		private void AddControlToNode(InputControl control, ref int controlIndiciesNextFreeIndex, int nodeIndex)
		{
			ref InputDevice.ControlBitRangeNode ptr = ref this.m_Device.m_ControlTreeNodes[nodeIndex];
			ushort num = ptr.controlStartIndex;
			if (ptr.controlCount == 0)
			{
				ptr.controlStartIndex = (ushort)controlIndiciesNextFreeIndex;
				num = ptr.controlStartIndex;
			}
			ArrayHelpers.InsertAt<ushort>(ref this.m_Device.m_ControlTreeIndices, (int)(ptr.controlStartIndex + (ushort)ptr.controlCount), this.GetControlIndex(control));
			ref InputDevice.ControlBitRangeNode ptr2 = ref ptr;
			ptr2.controlCount += 1;
			controlIndiciesNextFreeIndex++;
			for (int i = 0; i < this.m_Device.m_ControlTreeNodes.Length; i++)
			{
				if (this.m_Device.m_ControlTreeNodes[i].controlCount != 0 && this.m_Device.m_ControlTreeNodes[i].controlStartIndex > num)
				{
					InputDevice.ControlBitRangeNode[] controlTreeNodes = this.m_Device.m_ControlTreeNodes;
					int num2 = i;
					controlTreeNodes[num2].controlStartIndex = controlTreeNodes[num2].controlStartIndex + 1;
				}
			}
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0004D580 File Offset: 0x0004B780
		private void AddChildren(ref InputDevice.ControlBitRangeNode parent, InputDevice.ControlBitRangeNode left, InputDevice.ControlBitRangeNode right)
		{
			if (parent.leftChildIndex != -1)
			{
				return;
			}
			int num = this.m_Device.m_ControlTreeNodes.Length;
			parent.leftChildIndex = (short)num;
			Array.Resize<InputDevice.ControlBitRangeNode>(ref this.m_Device.m_ControlTreeNodes, num + 2);
			this.m_Device.m_ControlTreeNodes[num] = left;
			this.m_Device.m_ControlTreeNodes[num + 1] = right;
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0004D5E8 File Offset: 0x0004B7E8
		private ushort GetControlIndex(InputControl control)
		{
			for (int i = 0; i < this.m_Device.m_ChildrenForEachControl.Length; i++)
			{
				if (control == this.m_Device.m_ChildrenForEachControl[i])
				{
					return (ushort)i;
				}
			}
			throw new InvalidOperationException(string.Format("InputDeviceBuilder error. Couldn't find control {0}.", control));
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x0004D630 File Offset: 0x0004B830
		internal static ref InputDeviceBuilder instance
		{
			get
			{
				return ref InputDeviceBuilder.s_Instance;
			}
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0004D638 File Offset: 0x0004B838
		internal static InputDeviceBuilder.RefInstance Ref()
		{
			InputDeviceBuilder.s_InstanceRef++;
			return default(InputDeviceBuilder.RefInstance);
		}

		// Token: 0x04000690 RID: 1680
		private InputDevice m_Device;

		// Token: 0x04000691 RID: 1681
		private InputControlLayout.CacheRefInstance m_LayoutCacheRef;

		// Token: 0x04000692 RID: 1682
		private Dictionary<string, InputControlLayout.ControlItem> m_ChildControlOverrides;

		// Token: 0x04000693 RID: 1683
		private List<uint> m_StateOffsetToControlMap;

		// Token: 0x04000694 RID: 1684
		private StringBuilder m_StringBuilder;

		// Token: 0x04000695 RID: 1685
		private const uint kSizeForControlUsingStateFromOtherControl = 4294967295U;

		// Token: 0x04000696 RID: 1686
		private static InputDeviceBuilder s_Instance;

		// Token: 0x04000697 RID: 1687
		private static int s_InstanceRef;

		// Token: 0x02000236 RID: 566
		internal struct RefInstance : IDisposable
		{
			// Token: 0x060015F1 RID: 5617 RVA: 0x000651D9 File Offset: 0x000633D9
			public void Dispose()
			{
				InputDeviceBuilder.s_InstanceRef--;
				if (InputDeviceBuilder.s_InstanceRef <= 0)
				{
					InputDeviceBuilder.s_Instance.Dispose();
					InputDeviceBuilder.s_Instance = default(InputDeviceBuilder);
					InputDeviceBuilder.s_InstanceRef = 0;
					return;
				}
				InputDeviceBuilder.s_Instance.Reset();
			}
		}
	}
}
