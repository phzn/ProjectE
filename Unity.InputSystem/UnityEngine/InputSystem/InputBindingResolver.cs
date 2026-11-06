using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200002A RID: 42
	internal struct InputBindingResolver : IDisposable
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000D1F7 File Offset: 0x0000B3F7
		public int totalMapCount
		{
			get
			{
				return this.memory.mapCount;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000D204 File Offset: 0x0000B404
		public int totalActionCount
		{
			get
			{
				return this.memory.actionCount;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000D211 File Offset: 0x0000B411
		public int totalBindingCount
		{
			get
			{
				return this.memory.bindingCount;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000D21E File Offset: 0x0000B41E
		public int totalControlCount
		{
			get
			{
				return this.memory.controlCount;
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000D22B File Offset: 0x0000B42B
		public void Dispose()
		{
			this.memory.Dispose();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000D238 File Offset: 0x0000B438
		public void StartWithPreviousResolve(InputActionState state, bool isFullResolve)
		{
			this.m_IsControlOnlyResolve = !isFullResolve;
			this.maps = state.maps;
			this.interactions = state.interactions;
			this.processors = state.processors;
			this.composites = state.composites;
			this.controls = state.controls;
			if (isFullResolve)
			{
				if (this.maps != null)
				{
					Array.Clear(this.maps, 0, state.totalMapCount);
				}
				if (this.interactions != null)
				{
					Array.Clear(this.interactions, 0, state.totalInteractionCount);
				}
				if (this.processors != null)
				{
					Array.Clear(this.processors, 0, state.totalProcessorCount);
				}
				if (this.composites != null)
				{
					Array.Clear(this.composites, 0, state.totalCompositeCount);
				}
			}
			if (this.controls != null)
			{
				Array.Clear(this.controls, 0, state.totalControlCount);
			}
			state.maps = null;
			state.interactions = null;
			state.processors = null;
			state.composites = null;
			state.controls = null;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000D334 File Offset: 0x0000B534
		public unsafe void AddActionMap(InputActionMap actionMap)
		{
			InputSystem.EnsureInitialized();
			InputAction[] actions = actionMap.m_Actions;
			InputBinding[] bindings = actionMap.m_Bindings;
			int num = ((bindings != null) ? bindings.Length : 0);
			int num2 = ((actions != null) ? actions.Length : 0);
			int totalMapCount = this.totalMapCount;
			int totalActionCount = this.totalActionCount;
			int totalBindingCount = this.totalBindingCount;
			int totalControlCount = this.totalControlCount;
			int num3 = this.totalInteractionCount;
			int num4 = this.totalProcessorCount;
			int num5 = this.totalCompositeCount;
			InputActionState.UnmanagedMemory unmanagedMemory = default(InputActionState.UnmanagedMemory);
			int num6 = this.totalMapCount + 1;
			int num7 = this.totalActionCount + num2;
			int num8 = this.totalBindingCount + num;
			int num9 = this.totalInteractionCount;
			int num10 = this.totalCompositeCount;
			unmanagedMemory.Allocate(num6, num7, num8, this.totalControlCount, num9, num10);
			if (this.memory.isAllocated)
			{
				unmanagedMemory.CopyDataFrom(this.memory);
			}
			int num11 = -1;
			int num12 = -1;
			int num13 = 0;
			int num14 = -1;
			InputAction inputAction = null;
			InputBinding? inputBinding = actionMap.m_BindingMask;
			ReadOnlyArray<InputDevice>? devices = actionMap.devices;
			bool flag = actionMap.m_SingletonAction != null;
			InputControlList<InputControl> inputControlList = new InputControlList<InputControl>(Allocator.Temp, 0);
			try
			{
				for (int i = 0; i < num; i++)
				{
					InputActionState.BindingState* bindingStates = unmanagedMemory.bindingStates;
					ref InputBinding ptr = ref bindings[i];
					int num15 = totalBindingCount + i;
					bool isComposite = ptr.isComposite;
					bool flag2 = !isComposite && ptr.isPartOfComposite;
					InputActionState.BindingState* ptr2 = bindingStates + num15;
					try
					{
						int num16 = 0;
						int num17 = -1;
						int num18 = -1;
						int num19 = -1;
						int num20 = -1;
						int num21 = 0;
						int num22 = 0;
						int num23 = 0;
						if (flag2 && num11 == -1)
						{
							throw new InvalidOperationException(string.Format("Binding '{0}' is marked as being part of a composite but the preceding binding is not a composite", ptr));
						}
						int num24 = -1;
						string action = ptr.action;
						InputAction inputAction2 = null;
						if (!flag2)
						{
							if (flag)
							{
								num24 = 0;
							}
							else if (!string.IsNullOrEmpty(action))
							{
								num24 = actionMap.FindActionIndex(action);
							}
							if (num24 != -1)
							{
								inputAction2 = actions[num24];
							}
						}
						else
						{
							num24 = num14;
							inputAction2 = inputAction;
						}
						if (isComposite)
						{
							num11 = num15;
							inputAction = inputAction2;
							num14 = num24;
						}
						string effectivePath = ptr.effectivePath;
						bool flag3 = string.IsNullOrEmpty(effectivePath) || inputAction2 == null || (!isComposite && this.bindingMask != null && !this.bindingMask.Value.Matches(ref ptr, InputBinding.MatchOptions.EmptyGroupMatchesAny)) || (!isComposite && inputBinding != null && !inputBinding.Value.Matches(ref ptr, InputBinding.MatchOptions.EmptyGroupMatchesAny)) || (!isComposite && inputAction2 != null && inputAction2.m_BindingMask != null && !inputAction2.m_BindingMask.Value.Matches(ref ptr, InputBinding.MatchOptions.EmptyGroupMatchesAny));
						if (!flag3 && !isComposite)
						{
							num16 = this.memory.controlCount + inputControlList.Count;
							if (devices != null)
							{
								ReadOnlyArray<InputDevice> value = devices.Value;
								for (int j = 0; j < value.Count; j++)
								{
									InputDevice inputDevice = value[j];
									if (inputDevice.added)
									{
										num21 += InputControlPath.TryFindControls<InputControl>(inputDevice, effectivePath, 0, ref inputControlList);
									}
								}
							}
							else
							{
								num21 = InputSystem.FindControls<InputControl>(effectivePath, ref inputControlList);
							}
						}
						if (!flag3)
						{
							string effectiveProcessors = ptr.effectiveProcessors;
							if (!string.IsNullOrEmpty(effectiveProcessors))
							{
								num18 = this.InstantiateWithParameters<InputProcessor>(InputProcessor.s_Processors, effectiveProcessors, ref this.processors, ref this.totalProcessorCount, actionMap, ref ptr);
								if (num18 != -1)
								{
									num23 = this.totalProcessorCount - num18;
								}
							}
							if (!string.IsNullOrEmpty(inputAction2.m_Processors))
							{
								int num25 = this.InstantiateWithParameters<InputProcessor>(InputProcessor.s_Processors, inputAction2.m_Processors, ref this.processors, ref this.totalProcessorCount, actionMap, ref ptr);
								if (num25 != -1)
								{
									if (num18 == -1)
									{
										num18 = num25;
									}
									num23 += this.totalProcessorCount - num25;
								}
							}
							if (flag2)
							{
								if (num11 != -1)
								{
									num17 = bindingStates[num11].interactionStartIndex;
									num22 = bindingStates[num11].interactionCount;
								}
							}
							else
							{
								string effectiveInteractions = ptr.effectiveInteractions;
								if (!string.IsNullOrEmpty(effectiveInteractions))
								{
									num17 = this.InstantiateWithParameters<IInputInteraction>(InputInteraction.s_Interactions, effectiveInteractions, ref this.interactions, ref this.totalInteractionCount, actionMap, ref ptr);
									if (num17 != -1)
									{
										num22 = this.totalInteractionCount - num17;
									}
								}
								if (!string.IsNullOrEmpty(inputAction2.m_Interactions))
								{
									int num26 = this.InstantiateWithParameters<IInputInteraction>(InputInteraction.s_Interactions, inputAction2.m_Interactions, ref this.interactions, ref this.totalInteractionCount, actionMap, ref ptr);
									if (num26 != -1)
									{
										if (num17 == -1)
										{
											num17 = num26;
										}
										num22 += this.totalInteractionCount - num26;
									}
								}
							}
							if (isComposite)
							{
								InputBindingComposite inputBindingComposite = InputBindingResolver.InstantiateBindingComposite(ref ptr, actionMap);
								num12 = ArrayHelpers.AppendWithCapacity<InputBindingComposite>(ref this.composites, ref this.totalCompositeCount, inputBindingComposite, 10);
								num16 = this.memory.controlCount + inputControlList.Count;
							}
							else if (!flag2 && num11 != -1)
							{
								num13 = 0;
								num11 = -1;
								num12 = -1;
								inputAction = null;
								num14 = -1;
							}
						}
						if (flag2 && num11 != -1 && num21 > 0)
						{
							if (string.IsNullOrEmpty(ptr.name))
							{
								throw new InvalidOperationException(string.Format("Binding '{0}' that is part of composite '{1}' is missing a name", ptr, this.composites[num12]));
							}
							num20 = InputBindingResolver.AssignCompositePartIndex(this.composites[num12], ptr.name, ref num13);
							bindingStates[num11].controlCount += num21;
							num19 = bindingStates[num11].actionIndex;
						}
						else if (num24 != -1)
						{
							num19 = totalActionCount + num24;
						}
						*ptr2 = new InputActionState.BindingState
						{
							controlStartIndex = num16,
							controlCount = num21,
							interactionStartIndex = num17,
							interactionCount = num22,
							processorStartIndex = num18,
							processorCount = num23,
							isComposite = isComposite,
							isPartOfComposite = ptr.isPartOfComposite,
							partIndex = num20,
							actionIndex = num19,
							compositeOrCompositeBindingIndex = (isComposite ? num12 : num11),
							mapIndex = this.totalMapCount,
							wantsInitialStateCheck = (inputAction2 != null && inputAction2.wantsInitialStateCheck)
						};
					}
					catch (Exception ex)
					{
						Debug.LogError(string.Format("{0} while resolving binding '{1}' in action map '{2}'", ex.GetType().Name, ptr, actionMap));
						Debug.LogException(ex);
						if (ex.IsExceptionIndicatingBugInCode())
						{
							throw;
						}
					}
				}
				int count = inputControlList.Count;
				int num27 = this.memory.controlCount + count;
				if (unmanagedMemory.interactionCount != this.totalInteractionCount || unmanagedMemory.compositeCount != this.totalCompositeCount || unmanagedMemory.controlCount != num27)
				{
					InputActionState.UnmanagedMemory unmanagedMemory2 = default(InputActionState.UnmanagedMemory);
					unmanagedMemory2.Allocate(unmanagedMemory.mapCount, unmanagedMemory.actionCount, unmanagedMemory.bindingCount, num27, this.totalInteractionCount, this.totalCompositeCount);
					unmanagedMemory2.CopyDataFrom(unmanagedMemory);
					unmanagedMemory.Dispose();
					unmanagedMemory = unmanagedMemory2;
				}
				int controlCount = this.memory.controlCount;
				ArrayHelpers.AppendListWithCapacity<InputControl, InputControlList<InputControl>>(ref this.controls, ref controlCount, inputControlList, 10);
				for (int k = 0; k < num; k++)
				{
					InputActionState.BindingState* ptr3 = unmanagedMemory.bindingStates + (totalBindingCount + k);
					int controlCount2 = ptr3->controlCount;
					int controlStartIndex = ptr3->controlStartIndex;
					for (int l = 0; l < controlCount2; l++)
					{
						unmanagedMemory.controlIndexToBindingIndex[controlStartIndex + l] = totalBindingCount + k;
					}
				}
				for (int m = this.memory.interactionCount; m < unmanagedMemory.interactionCount; m++)
				{
					InputActionState.InteractionState* ptr4 = unmanagedMemory.interactionStates + m;
					ptr4->phase = InputActionPhase.Waiting;
					ptr4->triggerControlIndex = -1;
				}
				int num28 = this.memory.bindingCount;
				for (int n = 0; n < num2; n++)
				{
					InputAction inputAction3 = actions[n];
					int num29 = totalActionCount + n;
					inputAction3.m_ActionIndexInState = num29;
					unmanagedMemory.actionBindingIndicesAndCounts[num29 * 2] = (ushort)num28;
					int num30 = -1;
					int num31 = 0;
					int num32 = 0;
					for (int num33 = 0; num33 < num; num33++)
					{
						int num34 = totalBindingCount + num33;
						InputActionState.BindingState* ptr5 = unmanagedMemory.bindingStates + num34;
						if (ptr5->actionIndex == num29 && !ptr5->isPartOfComposite)
						{
							unmanagedMemory.actionBindingIndices[num28] = (ushort)num34;
							num28++;
							num31++;
							if (num30 == -1)
							{
								num30 = num34;
							}
							if (ptr5->isComposite)
							{
								if (ptr5->controlCount > 0)
								{
									num32++;
								}
							}
							else
							{
								num32 += ptr5->controlCount;
							}
						}
					}
					if (num30 == -1)
					{
						num30 = 0;
					}
					unmanagedMemory.actionBindingIndicesAndCounts[num29 * 2 + 1] = (ushort)num31;
					bool flag4 = inputAction3.type == InputActionType.PassThrough;
					bool flag5 = inputAction3.type == InputActionType.Button;
					bool flag6 = !flag4 && num32 > 1;
					unmanagedMemory.actionStates[num29] = new InputActionState.TriggerState
					{
						phase = InputActionPhase.Disabled,
						mapIndex = totalMapCount,
						controlIndex = -1,
						interactionIndex = -1,
						isPassThrough = flag4,
						isButton = flag5,
						mayNeedConflictResolution = flag6,
						bindingIndex = num30
					};
				}
				unmanagedMemory.mapIndices[totalMapCount] = new InputActionState.ActionMapIndices
				{
					actionStartIndex = totalActionCount,
					actionCount = num2,
					controlStartIndex = totalControlCount,
					controlCount = count,
					bindingStartIndex = totalBindingCount,
					bindingCount = num,
					interactionStartIndex = num3,
					interactionCount = this.totalInteractionCount - num3,
					processorStartIndex = num4,
					processorCount = this.totalProcessorCount - num4,
					compositeStartIndex = num5,
					compositeCount = this.totalCompositeCount - num5
				};
				actionMap.m_MapIndexInState = totalMapCount;
				int mapCount = this.memory.mapCount;
				ArrayHelpers.AppendWithCapacity<InputActionMap>(ref this.maps, ref mapCount, actionMap, 4);
				this.memory.Dispose();
				this.memory = unmanagedMemory;
			}
			catch (Exception)
			{
				unmanagedMemory.Dispose();
				throw;
			}
			finally
			{
				inputControlList.Dispose();
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000DD6C File Offset: 0x0000BF6C
		private int InstantiateWithParameters<TType>(TypeTable registrations, string namesAndParameters, ref TType[] array, ref int count, InputActionMap actionMap, ref InputBinding binding)
		{
			if (!NameAndParameters.ParseMultiple(namesAndParameters, ref this.m_Parameters))
			{
				return -1;
			}
			int num = count;
			for (int i = 0; i < this.m_Parameters.Count; i++)
			{
				string name = this.m_Parameters[i].name;
				Type type = registrations.LookupTypeRegistration(name);
				if (type == null)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"No ",
						typeof(TType).Name,
						" with name '",
						name,
						"' (mentioned in '",
						namesAndParameters,
						"') has been registered"
					}));
				}
				else if (!this.m_IsControlOnlyResolve)
				{
					object obj = Activator.CreateInstance(type);
					if (obj is TType)
					{
						TType ttype = (TType)((object)obj);
						InputBindingResolver.ApplyParameters(this.m_Parameters[i].parameters, ttype, actionMap, ref binding, name, namesAndParameters);
						ArrayHelpers.AppendWithCapacity<TType>(ref array, ref count, ttype, 10);
					}
					else
					{
						Debug.LogError(string.Concat(new string[]
						{
							"Type '",
							type.Name,
							"' registered as '",
							name,
							"' (mentioned in '",
							namesAndParameters,
							"') is not an ",
							typeof(TType).Name
						}));
					}
				}
				else
				{
					count++;
				}
			}
			return num;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000DEDC File Offset: 0x0000C0DC
		private static InputBindingComposite InstantiateBindingComposite(ref InputBinding binding, InputActionMap actionMap)
		{
			NameAndParameters nameAndParameters = NameAndParameters.Parse(binding.effectivePath);
			Type type = InputBindingComposite.s_Composites.LookupTypeRegistration(nameAndParameters.name);
			if (type == null)
			{
				throw new InvalidOperationException("No binding composite with name '" + nameAndParameters.name + "' has been registered");
			}
			InputBindingComposite inputBindingComposite = Activator.CreateInstance(type) as InputBindingComposite;
			if (inputBindingComposite == null)
			{
				throw new InvalidOperationException(string.Concat(new string[] { "Registered type '", type.Name, "' used for '", nameAndParameters.name, "' is not an InputBindingComposite" }));
			}
			InputBindingResolver.ApplyParameters(nameAndParameters.parameters, inputBindingComposite, actionMap, ref binding, nameAndParameters.name, binding.effectivePath);
			return inputBindingComposite;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000DF94 File Offset: 0x0000C194
		private static void ApplyParameters(ReadOnlyArray<NamedValue> parameters, object instance, InputActionMap actionMap, ref InputBinding binding, string objectRegistrationName, string namesAndParameters)
		{
			foreach (NamedValue namedValue in parameters)
			{
				FieldInfo field = instance.GetType().GetField(namedValue.name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field == null)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Type '",
						instance.GetType().Name,
						"' registered as '",
						objectRegistrationName,
						"' (mentioned in '",
						namesAndParameters,
						"') has no public field called '",
						namedValue.name,
						"'"
					}));
				}
				else
				{
					TypeCode typeCode = Type.GetTypeCode(field.FieldType);
					InputActionRebindingExtensions.ParameterOverride? parameterOverride = InputActionRebindingExtensions.ParameterOverride.Find(actionMap, ref binding, namedValue.name, objectRegistrationName);
					field.SetValue(instance, ((parameterOverride != null) ? parameterOverride.Value.value : namedValue.value).ConvertTo(typeCode).ToObject());
				}
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000E0B4 File Offset: 0x0000C2B4
		private static int AssignCompositePartIndex(object composite, string name, ref int currentCompositePartCount)
		{
			Type type = composite.GetType();
			FieldInfo field = type.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new InvalidOperationException(string.Format("Cannot find public field '{0}' used as parameter of binding composite '{1}' of type '{2}'", name, composite, type));
			}
			if (field.FieldType != typeof(int))
			{
				throw new InvalidOperationException(string.Format("Field '{0}' used as a parameter of binding composite '{1}' must be of type 'int' but is of type '{2}' instead", name, composite, type.Name));
			}
			int num = (int)field.GetValue(composite);
			if (num == 0)
			{
				int num2 = currentCompositePartCount + 1;
				currentCompositePartCount = num2;
				num = num2;
				field.SetValue(composite, num);
			}
			return num;
		}

		// Token: 0x040000F2 RID: 242
		public int totalProcessorCount;

		// Token: 0x040000F3 RID: 243
		public int totalCompositeCount;

		// Token: 0x040000F4 RID: 244
		public int totalInteractionCount;

		// Token: 0x040000F5 RID: 245
		public InputActionMap[] maps;

		// Token: 0x040000F6 RID: 246
		public InputControl[] controls;

		// Token: 0x040000F7 RID: 247
		public InputActionState.UnmanagedMemory memory;

		// Token: 0x040000F8 RID: 248
		public IInputInteraction[] interactions;

		// Token: 0x040000F9 RID: 249
		public InputProcessor[] processors;

		// Token: 0x040000FA RID: 250
		public InputBindingComposite[] composites;

		// Token: 0x040000FB RID: 251
		public InputBinding? bindingMask;

		// Token: 0x040000FC RID: 252
		private bool m_IsControlOnlyResolve;

		// Token: 0x040000FD RID: 253
		private List<NameAndParameters> m_Parameters;
	}
}
