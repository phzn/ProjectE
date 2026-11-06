using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000024 RID: 36
	internal class InputActionState : IInputStateChangeMonitor, ICloneable, IDisposable
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600023A RID: 570 RVA: 0x000085D8 File Offset: 0x000067D8
		public int totalCompositeCount
		{
			get
			{
				return this.memory.compositeCount;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600023B RID: 571 RVA: 0x000085E5 File Offset: 0x000067E5
		public int totalMapCount
		{
			get
			{
				return this.memory.mapCount;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600023C RID: 572 RVA: 0x000085F2 File Offset: 0x000067F2
		public int totalActionCount
		{
			get
			{
				return this.memory.actionCount;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600023D RID: 573 RVA: 0x000085FF File Offset: 0x000067FF
		public int totalBindingCount
		{
			get
			{
				return this.memory.bindingCount;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000860C File Offset: 0x0000680C
		public int totalInteractionCount
		{
			get
			{
				return this.memory.interactionCount;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00008619 File Offset: 0x00006819
		public int totalControlCount
		{
			get
			{
				return this.memory.controlCount;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00008626 File Offset: 0x00006826
		public unsafe InputActionState.ActionMapIndices* mapIndices
		{
			get
			{
				return this.memory.mapIndices;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00008633 File Offset: 0x00006833
		public unsafe InputActionState.TriggerState* actionStates
		{
			get
			{
				return this.memory.actionStates;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00008640 File Offset: 0x00006840
		public unsafe InputActionState.BindingState* bindingStates
		{
			get
			{
				return this.memory.bindingStates;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000864D File Offset: 0x0000684D
		public unsafe InputActionState.InteractionState* interactionStates
		{
			get
			{
				return this.memory.interactionStates;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000865A File Offset: 0x0000685A
		public unsafe int* controlIndexToBindingIndex
		{
			get
			{
				return this.memory.controlIndexToBindingIndex;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00008667 File Offset: 0x00006867
		public unsafe ushort* controlGroupingAndComplexity
		{
			get
			{
				return this.memory.controlGroupingAndComplexity;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00008674 File Offset: 0x00006874
		public unsafe float* controlMagnitudes
		{
			get
			{
				return this.memory.controlMagnitudes;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00008681 File Offset: 0x00006881
		public unsafe uint* enabledControls
		{
			get
			{
				return (uint*)this.memory.enabledControls;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000868E File Offset: 0x0000688E
		public bool isProcessingControlStateChange
		{
			get
			{
				return this.m_InProcessControlStateChange;
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00008696 File Offset: 0x00006896
		public void Initialize(InputBindingResolver resolver)
		{
			this.ClaimDataFrom(resolver);
			this.AddToGlobalList();
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000086A8 File Offset: 0x000068A8
		private unsafe void ComputeControlGroupingIfNecessary()
		{
			if (this.memory.controlGroupingInitialized)
			{
				return;
			}
			bool flag = !InputSystem.settings.shortcutKeysConsumeInput;
			uint num = 1U;
			for (int i = 0; i < this.totalControlCount; i++)
			{
				InputControl inputControl = this.controls[i];
				int num2 = this.controlIndexToBindingIndex[i];
				ref InputActionState.BindingState ptr = ref this.bindingStates[num2];
				int num3 = 1;
				if (ptr.isPartOfComposite && !flag)
				{
					int compositeOrCompositeBindingIndex = ptr.compositeOrCompositeBindingIndex;
					for (int j = compositeOrCompositeBindingIndex + 1; j < this.totalBindingCount; j++)
					{
						ref InputActionState.BindingState ptr2 = ref this.bindingStates[j];
						if (!ptr2.isPartOfComposite || ptr2.compositeOrCompositeBindingIndex != compositeOrCompositeBindingIndex)
						{
							break;
						}
						num3++;
					}
				}
				this.controlGroupingAndComplexity[i * 2 + 1] = (ushort)num3;
				if (this.controlGroupingAndComplexity[i * 2] == 0)
				{
					if (!flag)
					{
						for (int k = 0; k < this.totalControlCount; k++)
						{
							InputControl inputControl2 = this.controls[k];
							if (inputControl == inputControl2)
							{
								this.controlGroupingAndComplexity[k * 2] = (ushort)num;
							}
						}
					}
					this.controlGroupingAndComplexity[i * 2] = (ushort)num;
					num += 1U;
				}
			}
			this.memory.controlGroupingInitialized = true;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000087F0 File Offset: 0x000069F0
		public void ClaimDataFrom(InputBindingResolver resolver)
		{
			this.totalProcessorCount = resolver.totalProcessorCount;
			this.maps = resolver.maps;
			this.interactions = resolver.interactions;
			this.processors = resolver.processors;
			this.composites = resolver.composites;
			this.controls = resolver.controls;
			this.memory = resolver.memory;
			resolver.memory = default(InputActionState.UnmanagedMemory);
			this.ComputeControlGroupingIfNecessary();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00008864 File Offset: 0x00006A64
		~InputActionState()
		{
			this.Destroy(true);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00008894 File Offset: 0x00006A94
		public void Dispose()
		{
			this.Destroy(false);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000088A0 File Offset: 0x00006AA0
		private unsafe void Destroy(bool isFinalizing = false)
		{
			if (!isFinalizing)
			{
				for (int i = 0; i < this.totalMapCount; i++)
				{
					InputActionMap inputActionMap = this.maps[i];
					if (inputActionMap.enabled)
					{
						this.DisableControls(i, this.mapIndices[i].controlStartIndex, this.mapIndices[i].controlCount);
					}
					if (inputActionMap.m_Asset != null)
					{
						inputActionMap.m_Asset.m_SharedStateForAllMaps = null;
					}
					inputActionMap.m_State = null;
					inputActionMap.m_MapIndexInState = -1;
					inputActionMap.m_EnabledActionsCount = 0;
					InputAction[] actions = inputActionMap.m_Actions;
					if (actions != null)
					{
						for (int j = 0; j < actions.Length; j++)
						{
							actions[j].m_ActionIndexInState = -1;
						}
					}
				}
				this.RemoveMapFromGlobalList();
			}
			this.memory.Dispose();
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00008970 File Offset: 0x00006B70
		public InputActionState Clone()
		{
			return new InputActionState
			{
				maps = ArrayHelpers.Copy<InputActionMap>(this.maps),
				controls = ArrayHelpers.Copy<InputControl>(this.controls),
				interactions = ArrayHelpers.Copy<IInputInteraction>(this.interactions),
				processors = ArrayHelpers.Copy<InputProcessor>(this.processors),
				composites = ArrayHelpers.Copy<InputBindingComposite>(this.composites),
				totalProcessorCount = this.totalProcessorCount,
				memory = this.memory.Clone()
			};
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000089F4 File Offset: 0x00006BF4
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000089FC File Offset: 0x00006BFC
		private bool IsUsingDevice(InputDevice device)
		{
			bool flag = false;
			for (int i = 0; i < this.totalMapCount; i++)
			{
				ReadOnlyArray<InputDevice>? devices = this.maps[i].devices;
				if (devices == null)
				{
					flag = true;
				}
				else if (devices.Value.Contains(device))
				{
					return true;
				}
			}
			if (!flag)
			{
				return false;
			}
			for (int j = 0; j < this.totalControlCount; j++)
			{
				if (this.controls[j].device == device)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00008A78 File Offset: 0x00006C78
		private bool CanUseDevice(InputDevice device)
		{
			bool flag = false;
			for (int i = 0; i < this.totalMapCount; i++)
			{
				ReadOnlyArray<InputDevice>? devices = this.maps[i].devices;
				if (devices == null)
				{
					flag = true;
				}
				else if (devices.Value.Contains(device))
				{
					return true;
				}
			}
			if (!flag)
			{
				return false;
			}
			for (int j = 0; j < this.totalMapCount; j++)
			{
				InputBinding[] bindings = this.maps[j].m_Bindings;
				if (bindings != null)
				{
					int num = bindings.Length;
					for (int k = 0; k < num; k++)
					{
						if (InputControlPath.TryFindControl(device, bindings[k].effectivePath, 0) != null)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00008B24 File Offset: 0x00006D24
		public bool HasEnabledActions()
		{
			for (int i = 0; i < this.totalMapCount; i++)
			{
				if (this.maps[i].enabled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00008B54 File Offset: 0x00006D54
		private unsafe void FinishBindingCompositeSetups()
		{
			for (int i = 0; i < this.totalBindingCount; i++)
			{
				ref InputActionState.BindingState ptr = ref this.bindingStates[i];
				if (ptr.isComposite && ptr.compositeOrCompositeBindingIndex != -1)
				{
					InputBindingComposite inputBindingComposite = this.composites[ptr.compositeOrCompositeBindingIndex];
					InputBindingCompositeContext inputBindingCompositeContext = new InputBindingCompositeContext
					{
						m_State = this,
						m_BindingIndex = i
					};
					inputBindingComposite.CallFinishSetup(ref inputBindingCompositeContext);
				}
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00008BC4 File Offset: 0x00006DC4
		internal unsafe void PrepareForBindingReResolution(bool needFullResolve, ref InputControlList<InputControl> activeControls, ref bool hasEnabledActions)
		{
			bool flag = false;
			for (int i = 0; i < this.totalMapCount; i++)
			{
				InputActionMap inputActionMap = this.maps[i];
				if (inputActionMap.enabled)
				{
					hasEnabledActions = true;
					if (needFullResolve)
					{
						this.DisableAllActions(inputActionMap);
					}
					else
					{
						foreach (InputAction inputAction in inputActionMap.actions)
						{
							if (inputAction.phase.IsInProgress())
							{
								if (inputAction.ActiveControlIsValid(inputAction.activeControl))
								{
									if (!flag)
									{
										activeControls = new InputControlList<InputControl>(Allocator.Temp, 0);
										activeControls.Resize(this.totalControlCount);
										flag = true;
									}
									ref InputActionState.TriggerState ptr = ref this.actionStates[inputAction.m_ActionIndexInState];
									int num = ptr.controlIndex;
									activeControls[num] = this.controls[num];
									InputActionState.BindingState bindingState = this.bindingStates[ptr.bindingIndex];
									for (int j = 0; j < bindingState.interactionCount; j++)
									{
										int num2 = bindingState.interactionStartIndex + j;
										if (this.interactionStates[num2].phase.IsInProgress())
										{
											num = this.interactionStates[num2].triggerControlIndex;
											if (inputAction.ActiveControlIsValid(this.controls[num]))
											{
												activeControls[num] = this.controls[num];
											}
											else
											{
												this.ResetInteractionState(num2);
											}
										}
									}
								}
								else
								{
									this.ResetActionState(inputAction.m_ActionIndexInState, InputActionPhase.Waiting, false);
								}
							}
						}
						this.DisableControls(inputActionMap);
					}
				}
				inputActionMap.ClearCachedActionData(!needFullResolve);
			}
			this.NotifyListenersOfActionChange(InputActionChange.BoundControlsAboutToChange);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00008DA4 File Offset: 0x00006FA4
		public void FinishBindingResolution(bool hasEnabledActions, InputActionState.UnmanagedMemory oldMemory, InputControlList<InputControl> activeControls, bool isFullResolve)
		{
			this.FinishBindingCompositeSetups();
			if (hasEnabledActions)
			{
				this.RestoreActionStatesAfterReResolvingBindings(oldMemory, activeControls, isFullResolve);
				return;
			}
			this.NotifyListenersOfActionChange(InputActionChange.BoundControlsChanged);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00008DC4 File Offset: 0x00006FC4
		private unsafe void RestoreActionStatesAfterReResolvingBindings(InputActionState.UnmanagedMemory oldState, InputControlList<InputControl> activeControls, bool isFullResolve)
		{
			for (int i = 0; i < this.totalActionCount; i++)
			{
				ref InputActionState.TriggerState ptr = ref oldState.actionStates[i];
				ref InputActionState.TriggerState ptr2 = ref this.actionStates[i];
				ptr2.lastCanceledInUpdate = ptr.lastCanceledInUpdate;
				ptr2.lastPerformedInUpdate = ptr.lastPerformedInUpdate;
				ptr2.lastCompletedInUpdate = ptr.lastCompletedInUpdate;
				ptr2.pressedInUpdate = ptr.pressedInUpdate;
				ptr2.releasedInUpdate = ptr.releasedInUpdate;
				ptr2.startTime = ptr.startTime;
				ptr2.framePerformed = ptr.framePerformed;
				ptr2.frameCompleted = ptr.frameCompleted;
				ptr2.framePressed = ptr.framePressed;
				ptr2.frameReleased = ptr.frameReleased;
				ptr2.bindingIndex = ptr.bindingIndex;
				if (ptr.phase != InputActionPhase.Disabled)
				{
					ptr2.phase = InputActionPhase.Waiting;
					if (isFullResolve)
					{
						this.maps[ptr2.mapIndex].m_EnabledActionsCount++;
					}
				}
			}
			for (int j = 0; j < this.totalBindingCount; j++)
			{
				ref InputActionState.BindingState ptr3 = ref this.memory.bindingStates[j];
				if (!ptr3.isPartOfComposite)
				{
					if (ptr3.isComposite)
					{
						int compositeOrCompositeBindingIndex = ptr3.compositeOrCompositeBindingIndex;
						if (oldState.compositeMagnitudes != null)
						{
							this.memory.compositeMagnitudes[compositeOrCompositeBindingIndex] = oldState.compositeMagnitudes[compositeOrCompositeBindingIndex];
						}
					}
					int actionIndex = ptr3.actionIndex;
					if (actionIndex != -1)
					{
						ref InputActionState.TriggerState ptr4 = ref this.actionStates[actionIndex];
						if (!ptr4.isDisabled)
						{
							ptr3.initialStateCheckPending = ptr3.wantsInitialStateCheck;
							this.EnableControls(ptr3.mapIndex, ptr3.controlStartIndex, ptr3.controlCount);
							if (!isFullResolve)
							{
								ref InputActionState.BindingState ptr5 = ref this.memory.bindingStates[j];
								ptr3.triggerEventIdForComposite = ptr5.triggerEventIdForComposite;
								ref InputActionState.TriggerState ptr6 = ref oldState.actionStates[actionIndex];
								if (j == ptr6.bindingIndex && ptr6.phase.IsInProgress() && activeControls.Count > 0 && activeControls[ptr6.controlIndex] != null)
								{
									InputControl inputControl = activeControls[ptr6.controlIndex];
									int num = this.FindControlIndexOnBinding(j, inputControl);
									if (num != -1)
									{
										ptr4.phase = ptr6.phase;
										ptr4.controlIndex = num;
										ptr4.magnitude = ptr6.magnitude;
										ptr4.interactionIndex = ptr6.interactionIndex;
										this.memory.controlMagnitudes[num] = ptr6.magnitude;
									}
									for (int k = 0; k < ptr3.interactionCount; k++)
									{
										ref InputActionState.InteractionState ptr7 = ref oldState.interactionStates[ptr5.interactionStartIndex + k];
										if (ptr7.phase.IsInProgress())
										{
											inputControl = activeControls[ptr7.triggerControlIndex];
											if (inputControl != null)
											{
												num = this.FindControlIndexOnBinding(j, inputControl);
												ref InputActionState.InteractionState ptr8 = ref this.interactionStates[ptr3.interactionStartIndex + k];
												ptr8.phase = ptr7.phase;
												ptr8.performedTime = ptr7.performedTime;
												ptr8.startTime = ptr7.startTime;
												ptr8.triggerControlIndex = num;
												if (ptr7.isTimerRunning)
												{
													InputActionState.TriggerState triggerState = new InputActionState.TriggerState
													{
														mapIndex = ptr3.mapIndex,
														controlIndex = num,
														bindingIndex = j,
														time = ptr7.timerStartTime,
														interactionIndex = ptr3.interactionStartIndex + k
													};
													this.StartTimeout(ptr7.timerDuration, ref triggerState);
													ptr8.totalTimeoutCompletionDone = ptr7.totalTimeoutCompletionDone;
													ptr8.totalTimeoutCompletionTimeRemaining = ptr7.totalTimeoutCompletionTimeRemaining;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			this.HookOnBeforeUpdate();
			this.NotifyListenersOfActionChange(InputActionChange.BoundControlsChanged);
			if (isFullResolve && InputActionState.s_GlobalState.onActionChange.length > 0)
			{
				for (int l = 0; l < this.totalMapCount; l++)
				{
					InputActionMap inputActionMap = this.maps[l];
					if (inputActionMap.m_SingletonAction == null && inputActionMap.m_EnabledActionsCount == inputActionMap.m_Actions.LengthSafe<InputAction>())
					{
						InputActionState.NotifyListenersOfActionChange(InputActionChange.ActionMapEnabled, inputActionMap);
					}
					else
					{
						foreach (InputAction inputAction in inputActionMap.actions)
						{
							if (inputAction.enabled)
							{
								InputActionState.NotifyListenersOfActionChange(InputActionChange.ActionEnabled, inputAction);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00009278 File Offset: 0x00007478
		private unsafe bool IsActiveControl(int bindingIndex, int controlIndex)
		{
			ref InputActionState.BindingState ptr = ref this.bindingStates[bindingIndex];
			int actionIndex = ptr.actionIndex;
			if (actionIndex == -1)
			{
				return false;
			}
			if (this.actionStates[actionIndex].controlIndex == controlIndex)
			{
				return true;
			}
			for (int i = 0; i < ptr.interactionCount; i++)
			{
				if (this.interactionStates[this.bindingStates->interactionStartIndex + i].triggerControlIndex == controlIndex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x000092F8 File Offset: 0x000074F8
		private unsafe int FindControlIndexOnBinding(int bindingIndex, InputControl control)
		{
			int controlStartIndex = this.bindingStates[bindingIndex].controlStartIndex;
			int controlCount = this.bindingStates[bindingIndex].controlCount;
			for (int i = 0; i < controlCount; i++)
			{
				if (control == this.controls[controlStartIndex + i])
				{
					return controlStartIndex + i;
				}
			}
			return -1;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00009350 File Offset: 0x00007550
		private unsafe void ResetActionStatesDrivenBy(InputDevice device)
		{
			using (InputActionRebindingExtensions.DeferBindingResolution())
			{
				for (int i = 0; i < this.totalActionCount; i++)
				{
					InputActionState.TriggerState* ptr = this.actionStates + i;
					if (ptr->phase != InputActionPhase.Waiting && ptr->phase != InputActionPhase.Disabled)
					{
						if (ptr->isPassThrough)
						{
							if (!this.IsActionBoundToControlFromDevice(device, i))
							{
								goto IL_0065;
							}
						}
						else
						{
							int controlIndex = ptr->controlIndex;
							if (controlIndex == -1 || this.controls[controlIndex].device != device)
							{
								goto IL_0065;
							}
						}
						this.ResetActionState(i, InputActionPhase.Waiting, false);
					}
					IL_0065:;
				}
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000093EC File Offset: 0x000075EC
		private unsafe bool IsActionBoundToControlFromDevice(InputDevice device, int actionIndex)
		{
			bool flag = false;
			ushort num;
			ushort actionBindingStartIndexAndCount = this.GetActionBindingStartIndexAndCount(actionIndex, out num);
			for (int i = 0; i < (int)num; i++)
			{
				ushort num2 = this.memory.actionBindingIndices[(int)actionBindingStartIndexAndCount + i];
				int controlCount = this.bindingStates[num2].controlCount;
				int controlStartIndex = this.bindingStates[num2].controlStartIndex;
				for (int j = 0; j < controlCount; j++)
				{
					if (this.controls[controlStartIndex + j].device == device)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00009480 File Offset: 0x00007680
		public unsafe void ResetActionState(int actionIndex, InputActionPhase toPhase = InputActionPhase.Waiting, bool hardReset = false)
		{
			InputActionState.TriggerState* ptr = this.actionStates + actionIndex;
			if (ptr->phase != InputActionPhase.Waiting && ptr->phase != InputActionPhase.Disabled)
			{
				ptr->time = InputState.currentTime;
				if (ptr->interactionIndex != -1)
				{
					int bindingIndex = ptr->bindingIndex;
					if (bindingIndex != -1)
					{
						int mapIndex = ptr->mapIndex;
						int interactionCount = this.bindingStates[bindingIndex].interactionCount;
						int interactionStartIndex = this.bindingStates[bindingIndex].interactionStartIndex;
						for (int i = 0; i < interactionCount; i++)
						{
							int num = interactionStartIndex + i;
							this.ResetInteractionStateAndCancelIfNecessary(mapIndex, bindingIndex, num, toPhase);
						}
					}
				}
				else if (ptr->phase != InputActionPhase.Canceled)
				{
					this.ChangePhaseOfAction(InputActionPhase.Canceled, ref this.actionStates[actionIndex], toPhase);
				}
			}
			ptr->phase = toPhase;
			ptr->controlIndex = -1;
			ushort num2 = this.memory.actionBindingIndicesAndCounts[actionIndex];
			ptr->bindingIndex = (int)((this.memory.actionBindingIndices != null) ? this.memory.actionBindingIndices[num2] : 0);
			ptr->interactionIndex = -1;
			ptr->startTime = 0.0;
			ptr->time = 0.0;
			ptr->hasMultipleConcurrentActuations = false;
			ptr->inProcessing = false;
			ptr->isPressed = false;
			if (hardReset)
			{
				ptr->lastCanceledInUpdate = 0U;
				ptr->lastPerformedInUpdate = 0U;
				ptr->lastCompletedInUpdate = 0U;
				ptr->pressedInUpdate = 0U;
				ptr->releasedInUpdate = 0U;
				ptr->framePerformed = 0;
				ptr->frameCompleted = 0;
				ptr->framePressed = 0;
				ptr->frameReleased = 0;
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009616 File Offset: 0x00007816
		public unsafe ref InputActionState.TriggerState FetchActionState(InputAction action)
		{
			return ref this.actionStates[action.m_ActionIndexInState];
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000962D File Offset: 0x0000782D
		public unsafe InputActionState.ActionMapIndices FetchMapIndices(InputActionMap map)
		{
			return this.mapIndices[map.m_MapIndexInState];
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000964C File Offset: 0x0000784C
		public unsafe void EnableAllActions(InputActionMap map)
		{
			this.EnableControls(map);
			int mapIndexInState = map.m_MapIndexInState;
			int actionCount = this.mapIndices[mapIndexInState].actionCount;
			int actionStartIndex = this.mapIndices[mapIndexInState].actionStartIndex;
			for (int i = 0; i < actionCount; i++)
			{
				int num = actionStartIndex + i;
				InputActionState.TriggerState* ptr = this.actionStates + num;
				if (ptr->isDisabled)
				{
					ptr->phase = InputActionPhase.Waiting;
				}
				ptr->inProcessing = false;
			}
			map.m_EnabledActionsCount = actionCount;
			this.HookOnBeforeUpdate();
			if (map.m_SingletonAction != null)
			{
				InputActionState.NotifyListenersOfActionChange(InputActionChange.ActionEnabled, map.m_SingletonAction);
				return;
			}
			InputActionState.NotifyListenersOfActionChange(InputActionChange.ActionMapEnabled, map);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x000096FC File Offset: 0x000078FC
		private unsafe void EnableControls(InputActionMap map)
		{
			int mapIndexInState = map.m_MapIndexInState;
			int controlCount = this.mapIndices[mapIndexInState].controlCount;
			int controlStartIndex = this.mapIndices[mapIndexInState].controlStartIndex;
			if (controlCount > 0)
			{
				this.EnableControls(mapIndexInState, controlStartIndex, controlCount);
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000974C File Offset: 0x0000794C
		public unsafe void EnableSingleAction(InputAction action)
		{
			this.EnableControls(action);
			int actionIndexInState = action.m_ActionIndexInState;
			this.actionStates[actionIndexInState].phase = InputActionPhase.Waiting;
			action.m_ActionMap.m_EnabledActionsCount++;
			this.HookOnBeforeUpdate();
			InputActionState.NotifyListenersOfActionChange(InputActionChange.ActionEnabled, action);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000097A0 File Offset: 0x000079A0
		private unsafe void EnableControls(InputAction action)
		{
			int actionIndexInState = action.m_ActionIndexInState;
			int mapIndexInState = action.m_ActionMap.m_MapIndexInState;
			int bindingStartIndex = this.mapIndices[mapIndexInState].bindingStartIndex;
			int bindingCount = this.mapIndices[mapIndexInState].bindingCount;
			InputActionState.BindingState* bindingStates = this.memory.bindingStates;
			for (int i = 0; i < bindingCount; i++)
			{
				int num = bindingStartIndex + i;
				InputActionState.BindingState* ptr = bindingStates + num;
				if (ptr->actionIndex == actionIndexInState && !ptr->isPartOfComposite)
				{
					int controlCount = ptr->controlCount;
					if (controlCount != 0)
					{
						this.EnableControls(mapIndexInState, ptr->controlStartIndex, controlCount);
					}
				}
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009850 File Offset: 0x00007A50
		public unsafe void DisableAllActions(InputActionMap map)
		{
			this.DisableControls(map);
			int mapIndexInState = map.m_MapIndexInState;
			int actionStartIndex = this.mapIndices[mapIndexInState].actionStartIndex;
			int actionCount = this.mapIndices[mapIndexInState].actionCount;
			bool flag = map.m_EnabledActionsCount == actionCount;
			for (int i = 0; i < actionCount; i++)
			{
				int num = actionStartIndex + i;
				if (this.actionStates[num].phase != InputActionPhase.Disabled)
				{
					this.ResetActionState(num, InputActionPhase.Disabled, false);
					if (!flag)
					{
						InputActionState.NotifyListenersOfActionChange(InputActionChange.ActionDisabled, map.m_Actions[i]);
					}
				}
			}
			map.m_EnabledActionsCount = 0;
			if (map.m_SingletonAction != null)
			{
				InputActionState.NotifyListenersOfActionChange(InputActionChange.ActionDisabled, map.m_SingletonAction);
				return;
			}
			if (flag)
			{
				InputActionState.NotifyListenersOfActionChange(InputActionChange.ActionMapDisabled, map);
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009914 File Offset: 0x00007B14
		public unsafe void DisableControls(InputActionMap map)
		{
			int mapIndexInState = map.m_MapIndexInState;
			int controlCount = this.mapIndices[mapIndexInState].controlCount;
			int controlStartIndex = this.mapIndices[mapIndexInState].controlStartIndex;
			if (controlCount > 0)
			{
				this.DisableControls(mapIndexInState, controlStartIndex, controlCount);
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009961 File Offset: 0x00007B61
		public void DisableSingleAction(InputAction action)
		{
			this.DisableControls(action);
			this.ResetActionState(action.m_ActionIndexInState, InputActionPhase.Disabled, false);
			action.m_ActionMap.m_EnabledActionsCount--;
			InputActionState.NotifyListenersOfActionChange(InputActionChange.ActionDisabled, action);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009994 File Offset: 0x00007B94
		private unsafe void DisableControls(InputAction action)
		{
			int actionIndexInState = action.m_ActionIndexInState;
			int mapIndexInState = action.m_ActionMap.m_MapIndexInState;
			int bindingStartIndex = this.mapIndices[mapIndexInState].bindingStartIndex;
			int bindingCount = this.mapIndices[mapIndexInState].bindingCount;
			InputActionState.BindingState* bindingStates = this.memory.bindingStates;
			for (int i = 0; i < bindingCount; i++)
			{
				int num = bindingStartIndex + i;
				InputActionState.BindingState* ptr = bindingStates + num;
				if (ptr->actionIndex == actionIndexInState && !ptr->isPartOfComposite)
				{
					int controlCount = ptr->controlCount;
					if (controlCount != 0)
					{
						this.DisableControls(mapIndexInState, ptr->controlStartIndex, controlCount);
					}
				}
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009A44 File Offset: 0x00007C44
		private unsafe void EnableControls(int mapIndex, int controlStartIndex, int numControls)
		{
			InputManager s_Manager = InputSystem.s_Manager;
			for (int i = 0; i < numControls; i++)
			{
				int num = controlStartIndex + i;
				if (!this.IsControlEnabled(num))
				{
					int num2 = this.controlIndexToBindingIndex[num];
					long num3 = this.ToCombinedMapAndControlAndBindingIndex(mapIndex, num, num2);
					InputActionState.BindingState* ptr = this.bindingStates + num2;
					if (ptr->wantsInitialStateCheck)
					{
						this.SetInitialStateCheckPending(ptr, true);
					}
					s_Manager.AddStateChangeMonitor(this.controls[num], this, num3, (uint)this.controlGroupingAndComplexity[num * 2]);
					this.SetControlEnabled(num, true);
				}
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00009AD4 File Offset: 0x00007CD4
		private unsafe void DisableControls(int mapIndex, int controlStartIndex, int numControls)
		{
			InputManager s_Manager = InputSystem.s_Manager;
			for (int i = 0; i < numControls; i++)
			{
				int num = controlStartIndex + i;
				if (this.IsControlEnabled(num))
				{
					int num2 = this.controlIndexToBindingIndex[num];
					long num3 = this.ToCombinedMapAndControlAndBindingIndex(mapIndex, num, num2);
					InputActionState.BindingState* ptr = this.bindingStates + num2;
					if (ptr->wantsInitialStateCheck)
					{
						this.SetInitialStateCheckPending(ptr, false);
					}
					s_Manager.RemoveStateChangeMonitor(this.controls[num], this, num3);
					ptr->pressTime = 0.0;
					this.SetControlEnabled(num, false);
				}
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00009B68 File Offset: 0x00007D68
		public unsafe void SetInitialStateCheckPending(int actionIndex, bool value = true)
		{
			int mapIndex = this.actionStates[actionIndex].mapIndex;
			int bindingStartIndex = this.mapIndices[mapIndex].bindingStartIndex;
			int bindingCount = this.mapIndices[mapIndex].bindingCount;
			for (int i = 0; i < bindingCount; i++)
			{
				ref InputActionState.BindingState ptr = ref this.bindingStates[bindingStartIndex + i];
				if (ptr.actionIndex == actionIndex && !ptr.isPartOfComposite)
				{
					ptr.initialStateCheckPending = value;
				}
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00009BF4 File Offset: 0x00007DF4
		private unsafe void SetInitialStateCheckPending(InputActionState.BindingState* bindingStatePtr, bool value)
		{
			if (bindingStatePtr->isPartOfComposite)
			{
				int compositeOrCompositeBindingIndex = bindingStatePtr->compositeOrCompositeBindingIndex;
				this.bindingStates[compositeOrCompositeBindingIndex].initialStateCheckPending = value;
				return;
			}
			bindingStatePtr->initialStateCheckPending = value;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00009C30 File Offset: 0x00007E30
		private unsafe bool IsControlEnabled(int controlIndex)
		{
			int num = controlIndex / 32;
			uint num2 = 1U << controlIndex % 32;
			return (this.enabledControls[num] & num2) > 0U;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00009C60 File Offset: 0x00007E60
		private unsafe void SetControlEnabled(int controlIndex, bool state)
		{
			int num = controlIndex / 32;
			uint num2 = 1U << controlIndex % 32;
			if (state)
			{
				this.enabledControls[num] |= num2;
				return;
			}
			this.enabledControls[num] &= ~num2;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00009CA1 File Offset: 0x00007EA1
		private void HookOnBeforeUpdate()
		{
			if (this.m_OnBeforeUpdateHooked)
			{
				return;
			}
			if (this.m_OnBeforeUpdateDelegate == null)
			{
				this.m_OnBeforeUpdateDelegate = new Action(this.OnBeforeInitialUpdate);
			}
			InputSystem.s_Manager.onBeforeUpdate += this.m_OnBeforeUpdateDelegate;
			this.m_OnBeforeUpdateHooked = true;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00009CDD File Offset: 0x00007EDD
		private void UnhookOnBeforeUpdate()
		{
			if (!this.m_OnBeforeUpdateHooked)
			{
				return;
			}
			InputSystem.s_Manager.onBeforeUpdate -= this.m_OnBeforeUpdateDelegate;
			this.m_OnBeforeUpdateHooked = false;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00009D00 File Offset: 0x00007F00
		private unsafe void OnBeforeInitialUpdate()
		{
			if (InputState.currentUpdateType == InputUpdateType.BeforeRender)
			{
				return;
			}
			this.UnhookOnBeforeUpdate();
			double currentTime = InputState.currentTime;
			InputManager s_Manager = InputSystem.s_Manager;
			for (int i = 0; i < this.totalBindingCount; i++)
			{
				ref InputActionState.BindingState ptr = ref this.bindingStates[i];
				if (ptr.initialStateCheckPending)
				{
					ptr.initialStateCheckPending = false;
					int controlStartIndex = ptr.controlStartIndex;
					int controlCount = ptr.controlCount;
					bool isComposite = ptr.isComposite;
					bool flag = false;
					for (int j = 0; j < controlCount; j++)
					{
						int num = controlStartIndex + j;
						InputControl inputControl = this.controls[num];
						if (!this.IsActiveControl(i, num) && !inputControl.CheckStateIsAtDefault())
						{
							if (inputControl.IsValueConsideredPressed(inputControl.magnitude) && (ptr.pressTime == 0.0 || ptr.pressTime > currentTime))
							{
								ptr.pressTime = currentTime;
							}
							if (!isComposite || !flag)
							{
								s_Manager.SignalStateChangeMonitor(inputControl, this);
								flag = true;
							}
						}
					}
				}
			}
			s_Manager.FireStateChangeNotifications();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00009E00 File Offset: 0x00008000
		void IInputStateChangeMonitor.NotifyControlStateChanged(InputControl control, double time, InputEventPtr eventPtr, long mapControlAndBindingIndex)
		{
			int num;
			int num2;
			int num3;
			this.SplitUpMapAndControlAndBindingIndex(mapControlAndBindingIndex, out num, out num2, out num3);
			this.ProcessControlStateChange(num, num2, num3, time, eventPtr);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00009E28 File Offset: 0x00008028
		void IInputStateChangeMonitor.NotifyTimerExpired(InputControl control, double time, long mapControlAndBindingIndex, int interactionIndex)
		{
			int num;
			int num2;
			int num3;
			this.SplitUpMapAndControlAndBindingIndex(mapControlAndBindingIndex, out num, out num2, out num3);
			this.ProcessTimeout(time, num, num2, num3, interactionIndex);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00009E50 File Offset: 0x00008050
		private unsafe long ToCombinedMapAndControlAndBindingIndex(int mapIndex, int controlIndex, int bindingIndex)
		{
			ushort num = this.controlGroupingAndComplexity[controlIndex * 2 + 1];
			return (long)controlIndex | ((long)bindingIndex << 24) | ((long)mapIndex << 40) | (long)((long)((ulong)num) << 48);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00009E82 File Offset: 0x00008082
		private void SplitUpMapAndControlAndBindingIndex(long mapControlAndBindingIndex, out int mapIndex, out int controlIndex, out int bindingIndex)
		{
			controlIndex = (int)(mapControlAndBindingIndex & 16777215L);
			bindingIndex = (int)((mapControlAndBindingIndex >> 24) & 65535L);
			mapIndex = (int)((mapControlAndBindingIndex >> 40) & 255L);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00009EAC File Offset: 0x000080AC
		internal static int GetComplexityFromMonitorIndex(long mapControlAndBindingIndex)
		{
			return (int)((mapControlAndBindingIndex >> 48) & 255L);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00009EBC File Offset: 0x000080BC
		private unsafe void ProcessControlStateChange(int mapIndex, int controlIndex, int bindingIndex, double time, InputEventPtr eventPtr)
		{
			using (InputActionRebindingExtensions.DeferBindingResolution())
			{
				this.m_InProcessControlStateChange = true;
				this.m_CurrentlyProcessingThisEvent = eventPtr;
				try
				{
					InputActionState.BindingState* ptr = this.bindingStates + bindingIndex;
					int actionIndex = ptr->actionIndex;
					InputActionState.TriggerState triggerState = new InputActionState.TriggerState
					{
						mapIndex = mapIndex,
						controlIndex = controlIndex,
						bindingIndex = bindingIndex,
						interactionIndex = -1,
						time = time,
						startTime = time,
						isPassThrough = (actionIndex != -1 && this.actionStates[actionIndex].isPassThrough),
						isButton = (actionIndex != -1 && this.actionStates[actionIndex].isButton)
					};
					if (this.m_OnBeforeUpdateHooked)
					{
						ptr->initialStateCheckPending = false;
					}
					InputControl inputControl = this.controls[controlIndex];
					triggerState.magnitude = (inputControl.CheckStateIsAtDefault() ? 0f : inputControl.magnitude);
					this.controlMagnitudes[controlIndex] = triggerState.magnitude;
					if (inputControl.IsValueConsideredPressed(triggerState.magnitude) && (ptr->pressTime == 0.0 || ptr->pressTime > triggerState.time))
					{
						ptr->pressTime = triggerState.time;
					}
					bool flag = false;
					if (ptr->isPartOfComposite)
					{
						int compositeOrCompositeBindingIndex = ptr->compositeOrCompositeBindingIndex;
						InputActionState.BindingState* ptr2 = this.bindingStates + compositeOrCompositeBindingIndex;
						if (InputActionState.ShouldIgnoreInputOnCompositeBinding(ptr2, eventPtr))
						{
							return;
						}
						int compositeOrCompositeBindingIndex2 = this.bindingStates[compositeOrCompositeBindingIndex].compositeOrCompositeBindingIndex;
						InputBindingCompositeContext inputBindingCompositeContext = new InputBindingCompositeContext
						{
							m_State = this,
							m_BindingIndex = compositeOrCompositeBindingIndex
						};
						triggerState.magnitude = this.composites[compositeOrCompositeBindingIndex2].EvaluateMagnitude(ref inputBindingCompositeContext);
						this.memory.compositeMagnitudes[compositeOrCompositeBindingIndex2] = triggerState.magnitude;
						int interactionCount = ptr2->interactionCount;
						if (interactionCount > 0)
						{
							flag = true;
							this.ProcessInteractions(ref triggerState, ptr2->interactionStartIndex, interactionCount);
						}
					}
					bool flag2 = this.IsConflictingInput(ref triggerState, actionIndex);
					ptr = this.bindingStates + triggerState.bindingIndex;
					if (!flag2)
					{
						this.ProcessButtonState(ref triggerState, actionIndex, ptr);
					}
					int interactionCount2 = ptr->interactionCount;
					if (interactionCount2 > 0 && !ptr->isPartOfComposite)
					{
						this.ProcessInteractions(ref triggerState, ptr->interactionStartIndex, interactionCount2);
					}
					else if (!flag && !flag2)
					{
						this.ProcessDefaultInteraction(ref triggerState, actionIndex);
					}
				}
				finally
				{
					this.m_InProcessControlStateChange = false;
					this.m_CurrentlyProcessingThisEvent = default(InputEventPtr);
				}
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000A184 File Offset: 0x00008384
		private unsafe void ProcessButtonState(ref InputActionState.TriggerState trigger, int actionIndex, InputActionState.BindingState* bindingStatePtr)
		{
			InputControl inputControl = this.controls[trigger.controlIndex];
			float num = (inputControl.isButton ? ((ButtonControl)inputControl).pressPointOrDefault : ButtonControl.s_GlobalDefaultButtonPressPoint);
			if (this.controlMagnitudes[trigger.controlIndex] <= num * ButtonControl.s_GlobalDefaultButtonReleaseThreshold)
			{
				bindingStatePtr->pressTime = 0.0;
			}
			float magnitude = trigger.magnitude;
			InputActionState.TriggerState* ptr = this.actionStates + actionIndex;
			if (!ptr->isPressed && magnitude >= num)
			{
				ptr->framePressed = Time.frameCount;
				ptr->pressedInUpdate = InputUpdate.s_UpdateStepCount;
				ptr->isPressed = true;
				return;
			}
			if (ptr->isPressed)
			{
				float num2 = num * ButtonControl.s_GlobalDefaultButtonReleaseThreshold;
				if (magnitude <= num2)
				{
					ptr->frameReleased = Time.frameCount;
					ptr->releasedInUpdate = InputUpdate.s_UpdateStepCount;
					ptr->isPressed = false;
				}
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A25C File Offset: 0x0000845C
		private unsafe static bool ShouldIgnoreInputOnCompositeBinding(InputActionState.BindingState* binding, InputEvent* eventPtr)
		{
			if (eventPtr == null)
			{
				return false;
			}
			int eventId = eventPtr->eventId;
			if (eventId != 0 && binding->triggerEventIdForComposite == eventId)
			{
				return true;
			}
			binding->triggerEventIdForComposite = eventId;
			return false;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000A290 File Offset: 0x00008490
		private unsafe bool IsConflictingInput(ref InputActionState.TriggerState trigger, int actionIndex)
		{
			InputActionState.TriggerState* ptr = this.actionStates + actionIndex;
			if (!ptr->mayNeedConflictResolution)
			{
				return false;
			}
			int num = trigger.controlIndex;
			if (this.bindingStates[trigger.bindingIndex].isPartOfComposite)
			{
				int compositeOrCompositeBindingIndex = this.bindingStates[trigger.bindingIndex].compositeOrCompositeBindingIndex;
				num = this.bindingStates[compositeOrCompositeBindingIndex].controlStartIndex;
			}
			int num2 = ptr->controlIndex;
			if (this.bindingStates[ptr->bindingIndex].isPartOfComposite)
			{
				int compositeOrCompositeBindingIndex2 = this.bindingStates[ptr->bindingIndex].compositeOrCompositeBindingIndex;
				num2 = this.bindingStates[compositeOrCompositeBindingIndex2].controlStartIndex;
			}
			if (num2 == -1)
			{
				ptr->magnitude = trigger.magnitude;
				return false;
			}
			bool flag = num == num2 || this.controls[num] == this.controls[num2];
			if (trigger.magnitude > ptr->magnitude)
			{
				if (trigger.magnitude > 0f && !flag && ptr->magnitude > 0f)
				{
					ptr->hasMultipleConcurrentActuations = true;
				}
				ptr->magnitude = trigger.magnitude;
				return false;
			}
			if (trigger.magnitude < ptr->magnitude)
			{
				if (!flag)
				{
					if (trigger.magnitude > 0f)
					{
						ptr->hasMultipleConcurrentActuations = true;
					}
					return true;
				}
				if (!ptr->hasMultipleConcurrentActuations)
				{
					ptr->magnitude = trigger.magnitude;
					return false;
				}
				ushort num3;
				ushort actionBindingStartIndexAndCount = this.GetActionBindingStartIndexAndCount(actionIndex, out num3);
				float num4 = trigger.magnitude;
				int num5 = -1;
				int num6 = -1;
				int num7 = 0;
				for (int i = 0; i < (int)num3; i++)
				{
					ushort num8 = this.memory.actionBindingIndices[(int)actionBindingStartIndexAndCount + i];
					InputActionState.BindingState* ptr2 = this.memory.bindingStates + num8;
					if (ptr2->isComposite)
					{
						int controlStartIndex = ptr2->controlStartIndex;
						int compositeOrCompositeBindingIndex3 = ptr2->compositeOrCompositeBindingIndex;
						float num9 = this.memory.compositeMagnitudes[compositeOrCompositeBindingIndex3];
						if (num9 > 0f)
						{
							num7++;
						}
						if (num9 > num4)
						{
							num5 = controlStartIndex;
							num6 = this.controlIndexToBindingIndex[controlStartIndex];
							num4 = num9;
						}
					}
					else if (!ptr2->isPartOfComposite)
					{
						for (int j = 0; j < ptr2->controlCount; j++)
						{
							int num10 = ptr2->controlStartIndex + j;
							float num11 = this.memory.controlMagnitudes[num10];
							if (num11 > 0f)
							{
								num7++;
							}
							if (num11 > num4)
							{
								num5 = num10;
								num6 = (int)num8;
								num4 = num11;
							}
						}
					}
				}
				if (num7 <= 1)
				{
					ptr->hasMultipleConcurrentActuations = false;
				}
				if (num5 != -1)
				{
					trigger.controlIndex = num5;
					trigger.bindingIndex = num6;
					trigger.magnitude = num4;
					if (ptr->bindingIndex != num6)
					{
						if (ptr->interactionIndex != -1)
						{
							this.ResetInteractionState(ptr->interactionIndex);
						}
						InputActionState.BindingState* ptr3 = this.bindingStates + num6;
						int interactionCount = ptr3->interactionCount;
						int interactionStartIndex = ptr3->interactionStartIndex;
						for (int k = 0; k < interactionCount; k++)
						{
							if (this.interactionStates[interactionStartIndex + k].phase.IsInProgress())
							{
								ptr->interactionIndex = interactionStartIndex + k;
								trigger.interactionIndex = interactionStartIndex + k;
								break;
							}
						}
					}
					ptr->controlIndex = num5;
					ptr->bindingIndex = num6;
					ptr->magnitude = num4;
					return false;
				}
			}
			if (!flag && Mathf.Approximately(trigger.magnitude, ptr->magnitude))
			{
				if (trigger.magnitude > 0f)
				{
					ptr->hasMultipleConcurrentActuations = true;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000A62D File Offset: 0x0000882D
		private unsafe ushort GetActionBindingStartIndexAndCount(int actionIndex, out ushort bindingCount)
		{
			bindingCount = this.memory.actionBindingIndicesAndCounts[actionIndex * 2 + 1];
			return this.memory.actionBindingIndicesAndCounts[actionIndex * 2];
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A65C File Offset: 0x0000885C
		private unsafe void ProcessDefaultInteraction(ref InputActionState.TriggerState trigger, int actionIndex)
		{
			InputActionState.TriggerState* ptr = this.actionStates + actionIndex;
			switch (ptr->phase)
			{
			case InputActionPhase.Waiting:
				if (trigger.isPassThrough)
				{
					this.ChangePhaseOfAction(InputActionPhase.Performed, ref trigger, InputActionPhase.Waiting);
					return;
				}
				if (trigger.isButton)
				{
					float magnitude = trigger.magnitude;
					if (magnitude > 0f)
					{
						this.ChangePhaseOfAction(InputActionPhase.Started, ref trigger, InputActionPhase.Waiting);
					}
					ButtonControl buttonControl = this.controls[trigger.controlIndex] as ButtonControl;
					float num = ((buttonControl != null) ? buttonControl.pressPointOrDefault : ButtonControl.s_GlobalDefaultButtonPressPoint);
					if (magnitude >= num)
					{
						this.ChangePhaseOfAction(InputActionPhase.Performed, ref trigger, InputActionPhase.Performed);
						return;
					}
				}
				else if (InputActionState.IsActuated(ref trigger, 0f))
				{
					this.ChangePhaseOfAction(InputActionPhase.Started, ref trigger, InputActionPhase.Waiting);
					this.ChangePhaseOfAction(InputActionPhase.Performed, ref trigger, InputActionPhase.Started);
					return;
				}
				break;
			case InputActionPhase.Started:
				if (ptr->isButton)
				{
					float magnitude2 = trigger.magnitude;
					ButtonControl buttonControl2 = this.controls[trigger.controlIndex] as ButtonControl;
					float num2 = ((buttonControl2 != null) ? buttonControl2.pressPointOrDefault : ButtonControl.s_GlobalDefaultButtonPressPoint);
					if (magnitude2 >= num2)
					{
						this.ChangePhaseOfAction(InputActionPhase.Performed, ref trigger, InputActionPhase.Performed);
						return;
					}
					if (Mathf.Approximately(magnitude2, 0f))
					{
						this.ChangePhaseOfAction(InputActionPhase.Canceled, ref trigger, InputActionPhase.Waiting);
						return;
					}
				}
				else
				{
					if (!InputActionState.IsActuated(ref trigger, 0f))
					{
						this.ChangePhaseOfAction(InputActionPhase.Canceled, ref trigger, InputActionPhase.Waiting);
						return;
					}
					this.ChangePhaseOfAction(InputActionPhase.Performed, ref trigger, InputActionPhase.Started);
					return;
				}
				break;
			case InputActionPhase.Performed:
				if (ptr->isButton)
				{
					float magnitude3 = trigger.magnitude;
					ButtonControl buttonControl3 = this.controls[trigger.controlIndex] as ButtonControl;
					float num3 = ((buttonControl3 != null) ? buttonControl3.pressPointOrDefault : ButtonControl.s_GlobalDefaultButtonPressPoint);
					if (Mathf.Approximately(0f, magnitude3))
					{
						this.ChangePhaseOfAction(InputActionPhase.Canceled, ref trigger, InputActionPhase.Waiting);
						return;
					}
					float num4 = num3 * ButtonControl.s_GlobalDefaultButtonReleaseThreshold;
					if (magnitude3 <= num4)
					{
						this.ChangePhaseOfAction(InputActionPhase.Started, ref trigger, InputActionPhase.Waiting);
						return;
					}
				}
				else if (ptr->isPassThrough)
				{
					this.ChangePhaseOfAction(InputActionPhase.Performed, ref trigger, InputActionPhase.Performed);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A82C File Offset: 0x00008A2C
		private unsafe void ProcessInteractions(ref InputActionState.TriggerState trigger, int interactionStartIndex, int interactionCount)
		{
			InputInteractionContext inputInteractionContext = new InputInteractionContext
			{
				m_State = this,
				m_TriggerState = trigger
			};
			for (int i = 0; i < interactionCount; i++)
			{
				int num = interactionStartIndex + i;
				InputActionState.InteractionState interactionState = this.interactionStates[num];
				IInputInteraction inputInteraction = this.interactions[num];
				inputInteractionContext.m_TriggerState.phase = interactionState.phase;
				inputInteractionContext.m_TriggerState.startTime = interactionState.startTime;
				inputInteractionContext.m_TriggerState.interactionIndex = num;
				inputInteraction.Process(ref inputInteractionContext);
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A8C4 File Offset: 0x00008AC4
		private unsafe void ProcessTimeout(double time, int mapIndex, int controlIndex, int bindingIndex, int interactionIndex)
		{
			ref InputActionState.InteractionState ptr = ref this.interactionStates[interactionIndex];
			InputInteractionContext inputInteractionContext = new InputInteractionContext
			{
				m_State = this,
				m_TriggerState = new InputActionState.TriggerState
				{
					phase = ptr.phase,
					time = time,
					mapIndex = mapIndex,
					controlIndex = controlIndex,
					bindingIndex = bindingIndex,
					interactionIndex = interactionIndex,
					startTime = ptr.startTime
				},
				timerHasExpired = true
			};
			ptr.isTimerRunning = false;
			ptr.totalTimeoutCompletionTimeRemaining = Mathf.Max(ptr.totalTimeoutCompletionTimeRemaining - ptr.timerDuration, 0f);
			ptr.timerDuration = 0f;
			this.interactions[interactionIndex].Process(ref inputInteractionContext);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A990 File Offset: 0x00008B90
		internal unsafe void SetTotalTimeoutCompletionTime(float seconds, ref InputActionState.TriggerState trigger)
		{
			InputActionState.InteractionState* ptr = this.interactionStates + trigger.interactionIndex;
			ptr->totalTimeoutCompletionDone = 0f;
			ptr->totalTimeoutCompletionTimeRemaining = seconds;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A9B8 File Offset: 0x00008BB8
		internal unsafe void StartTimeout(float seconds, ref InputActionState.TriggerState trigger)
		{
			InputManager s_Manager = InputSystem.s_Manager;
			double time = trigger.time;
			InputControl inputControl = this.controls[trigger.controlIndex];
			int interactionIndex = trigger.interactionIndex;
			long num = this.ToCombinedMapAndControlAndBindingIndex(trigger.mapIndex, trigger.controlIndex, trigger.bindingIndex);
			InputActionState.InteractionState* ptr = this.interactionStates + interactionIndex;
			if (ptr->isTimerRunning)
			{
				this.StopTimeout(interactionIndex);
			}
			s_Manager.AddStateChangeMonitorTimeout(inputControl, this, time + (double)seconds, num, interactionIndex);
			ptr->isTimerRunning = true;
			ptr->timerStartTime = time;
			ptr->timerDuration = seconds;
			ptr->timerMonitorIndex = num;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000AA4C File Offset: 0x00008C4C
		private unsafe void StopTimeout(int interactionIndex)
		{
			ref InputActionState.InteractionState ptr = ref this.interactionStates[interactionIndex];
			InputSystem.s_Manager.RemoveStateChangeMonitorTimeout(this, ptr.timerMonitorIndex, interactionIndex);
			ptr.isTimerRunning = false;
			ptr.totalTimeoutCompletionDone += ptr.timerDuration;
			ptr.totalTimeoutCompletionTimeRemaining = Mathf.Max(ptr.totalTimeoutCompletionTimeRemaining - ptr.timerDuration, 0f);
			ptr.timerDuration = 0f;
			ptr.timerStartTime = 0.0;
			ptr.timerMonitorIndex = 0L;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000AAD8 File Offset: 0x00008CD8
		internal unsafe void ChangePhaseOfInteraction(InputActionPhase newPhase, ref InputActionState.TriggerState trigger, InputActionPhase phaseAfterPerformed = InputActionPhase.Waiting, InputActionPhase phaseAfterCanceled = InputActionPhase.Waiting, bool processNextInteractionOnCancel = true)
		{
			int interactionIndex = trigger.interactionIndex;
			int bindingIndex = trigger.bindingIndex;
			InputActionPhase inputActionPhase = InputActionPhase.Waiting;
			if (newPhase == InputActionPhase.Performed)
			{
				inputActionPhase = phaseAfterPerformed;
			}
			else if (newPhase == InputActionPhase.Canceled)
			{
				inputActionPhase = phaseAfterCanceled;
			}
			ref InputActionState.InteractionState ptr = ref this.interactionStates[interactionIndex];
			if (ptr.isTimerRunning)
			{
				this.StopTimeout(trigger.interactionIndex);
			}
			ptr.phase = newPhase;
			ptr.triggerControlIndex = trigger.controlIndex;
			ptr.startTime = trigger.startTime;
			if (newPhase == InputActionPhase.Performed)
			{
				ptr.performedTime = trigger.time;
			}
			int actionIndex = this.bindingStates[bindingIndex].actionIndex;
			if (actionIndex != -1)
			{
				if (this.actionStates[actionIndex].phase == InputActionPhase.Waiting)
				{
					if (!this.ChangePhaseOfAction(newPhase, ref trigger, inputActionPhase))
					{
						return;
					}
				}
				else if (newPhase == InputActionPhase.Canceled && this.actionStates[actionIndex].interactionIndex == trigger.interactionIndex)
				{
					if (!this.ChangePhaseOfAction(newPhase, ref trigger, inputActionPhase))
					{
						return;
					}
					if (!processNextInteractionOnCancel)
					{
						return;
					}
					int interactionStartIndex = this.bindingStates[bindingIndex].interactionStartIndex;
					int interactionCount = this.bindingStates[bindingIndex].interactionCount;
					int i = 0;
					while (i < interactionCount)
					{
						int num = interactionStartIndex + i;
						if (num != trigger.interactionIndex && (this.interactionStates[num].phase == InputActionPhase.Started || this.interactionStates[num].phase == InputActionPhase.Performed))
						{
							double startTime = this.interactionStates[num].startTime;
							InputActionState.TriggerState triggerState = new InputActionState.TriggerState
							{
								phase = InputActionPhase.Started,
								controlIndex = this.interactionStates[num].triggerControlIndex,
								bindingIndex = trigger.bindingIndex,
								interactionIndex = num,
								mapIndex = trigger.mapIndex,
								time = startTime,
								startTime = startTime
							};
							if (!this.ChangePhaseOfAction(InputActionPhase.Started, ref triggerState, inputActionPhase))
							{
								return;
							}
							if (this.interactionStates[num].phase != InputActionPhase.Performed)
							{
								break;
							}
							triggerState = new InputActionState.TriggerState
							{
								phase = InputActionPhase.Performed,
								controlIndex = this.interactionStates[num].triggerControlIndex,
								bindingIndex = trigger.bindingIndex,
								interactionIndex = num,
								mapIndex = trigger.mapIndex,
								time = this.interactionStates[num].performedTime,
								startTime = startTime
							};
							if (!this.ChangePhaseOfAction(InputActionPhase.Performed, ref triggerState, inputActionPhase))
							{
								return;
							}
							while (i < interactionCount)
							{
								num = interactionStartIndex + i;
								this.ResetInteractionState(num);
								i++;
							}
							break;
						}
						else
						{
							i++;
						}
					}
				}
				else if (this.actionStates[actionIndex].interactionIndex == trigger.interactionIndex)
				{
					if (!this.ChangePhaseOfAction(newPhase, ref trigger, inputActionPhase))
					{
						return;
					}
					if (newPhase == InputActionPhase.Performed)
					{
						int interactionStartIndex2 = this.bindingStates[bindingIndex].interactionStartIndex;
						int interactionCount2 = this.bindingStates[bindingIndex].interactionCount;
						for (int j = 0; j < interactionCount2; j++)
						{
							int num2 = interactionStartIndex2 + j;
							if (num2 != trigger.interactionIndex)
							{
								this.ResetInteractionState(num2);
							}
						}
					}
				}
			}
			if (newPhase != InputActionPhase.Performed || actionIndex == -1 || this.actionStates[actionIndex].isPerformed || this.actionStates[actionIndex].interactionIndex == trigger.interactionIndex)
			{
				if (newPhase == InputActionPhase.Performed && phaseAfterPerformed != InputActionPhase.Waiting)
				{
					ptr.phase = phaseAfterPerformed;
					return;
				}
				if (newPhase == InputActionPhase.Performed || newPhase == InputActionPhase.Canceled)
				{
					this.ResetInteractionState(trigger.interactionIndex);
				}
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000AEA4 File Offset: 0x000090A4
		private unsafe bool ChangePhaseOfAction(InputActionPhase newPhase, ref InputActionState.TriggerState trigger, InputActionPhase phaseAfterPerformedOrCanceled = InputActionPhase.Waiting)
		{
			int actionIndex = this.bindingStates[trigger.bindingIndex].actionIndex;
			if (actionIndex == -1)
			{
				return true;
			}
			InputActionState.TriggerState* ptr = this.actionStates + actionIndex;
			if (ptr->isDisabled)
			{
				return true;
			}
			ptr->inProcessing = true;
			try
			{
				if (ptr->isPassThrough && trigger.interactionIndex == -1)
				{
					this.ChangePhaseOfActionInternal(actionIndex, ptr, newPhase, ref trigger, newPhase == InputActionPhase.Canceled && phaseAfterPerformedOrCanceled == InputActionPhase.Disabled);
					if (!ptr->inProcessing)
					{
						return false;
					}
				}
				else if (newPhase == InputActionPhase.Performed && ptr->phase == InputActionPhase.Waiting)
				{
					this.ChangePhaseOfActionInternal(actionIndex, ptr, InputActionPhase.Started, ref trigger, false);
					if (!ptr->inProcessing)
					{
						return false;
					}
					this.ChangePhaseOfActionInternal(actionIndex, ptr, newPhase, ref trigger, false);
					if (!ptr->inProcessing)
					{
						return false;
					}
					if (phaseAfterPerformedOrCanceled == InputActionPhase.Waiting)
					{
						this.ChangePhaseOfActionInternal(actionIndex, ptr, InputActionPhase.Canceled, ref trigger, false);
					}
					if (!ptr->inProcessing)
					{
						return false;
					}
					ptr->phase = phaseAfterPerformedOrCanceled;
				}
				else if (ptr->phase != newPhase || newPhase == InputActionPhase.Performed)
				{
					this.ChangePhaseOfActionInternal(actionIndex, ptr, newPhase, ref trigger, newPhase == InputActionPhase.Canceled && phaseAfterPerformedOrCanceled == InputActionPhase.Disabled);
					if (!ptr->inProcessing)
					{
						return false;
					}
					if (newPhase == InputActionPhase.Performed || newPhase == InputActionPhase.Canceled)
					{
						ptr->phase = phaseAfterPerformedOrCanceled;
					}
				}
			}
			finally
			{
				ptr->inProcessing = false;
			}
			if (ptr->phase == InputActionPhase.Waiting)
			{
				ptr->controlIndex = -1;
				ptr->flags &= ~InputActionState.TriggerState.Flags.HaveMagnitude;
			}
			return true;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000B00C File Offset: 0x0000920C
		private unsafe void ChangePhaseOfActionInternal(int actionIndex, InputActionState.TriggerState* actionState, InputActionPhase newPhase, ref InputActionState.TriggerState trigger, bool isDisablingAction = false)
		{
			InputActionState.TriggerState triggerState = trigger;
			triggerState.flags = actionState->flags;
			if (newPhase != InputActionPhase.Canceled)
			{
				triggerState.magnitude = trigger.magnitude;
			}
			else
			{
				triggerState.magnitude = 0f;
			}
			triggerState.phase = newPhase;
			if (newPhase == InputActionPhase.Performed)
			{
				triggerState.framePerformed = Time.frameCount;
				triggerState.lastPerformedInUpdate = InputUpdate.s_UpdateStepCount;
				triggerState.lastCanceledInUpdate = actionState->lastCanceledInUpdate;
				if (this.controlGroupingAndComplexity[trigger.controlIndex * 2 + 1] > 1 && this.m_CurrentlyProcessingThisEvent.valid)
				{
					this.m_CurrentlyProcessingThisEvent.handled = true;
				}
			}
			else if (newPhase == InputActionPhase.Canceled)
			{
				triggerState.lastCanceledInUpdate = InputUpdate.s_UpdateStepCount;
				triggerState.lastPerformedInUpdate = actionState->lastPerformedInUpdate;
				triggerState.framePerformed = actionState->framePerformed;
			}
			else
			{
				triggerState.lastPerformedInUpdate = actionState->lastPerformedInUpdate;
				triggerState.framePerformed = actionState->framePerformed;
				triggerState.lastCanceledInUpdate = actionState->lastCanceledInUpdate;
			}
			if (actionState->phase == InputActionPhase.Performed && newPhase != InputActionPhase.Performed && !isDisablingAction)
			{
				triggerState.frameCompleted = Time.frameCount;
				triggerState.lastCompletedInUpdate = InputUpdate.s_UpdateStepCount;
			}
			else
			{
				triggerState.lastCompletedInUpdate = actionState->lastCompletedInUpdate;
				triggerState.frameCompleted = actionState->frameCompleted;
			}
			triggerState.pressedInUpdate = actionState->pressedInUpdate;
			triggerState.framePressed = actionState->framePressed;
			triggerState.releasedInUpdate = actionState->releasedInUpdate;
			triggerState.frameReleased = actionState->frameReleased;
			if (newPhase == InputActionPhase.Started)
			{
				triggerState.startTime = triggerState.time;
			}
			*actionState = triggerState;
			InputActionMap inputActionMap = this.maps[trigger.mapIndex];
			InputAction inputAction = inputActionMap.m_Actions[actionIndex - this.mapIndices[trigger.mapIndex].actionStartIndex];
			trigger.phase = newPhase;
			switch (newPhase)
			{
			case InputActionPhase.Started:
				this.CallActionListeners(actionIndex, inputActionMap, newPhase, ref inputAction.m_OnStarted, "started");
				return;
			case InputActionPhase.Performed:
				this.CallActionListeners(actionIndex, inputActionMap, newPhase, ref inputAction.m_OnPerformed, "performed");
				return;
			case InputActionPhase.Canceled:
				this.CallActionListeners(actionIndex, inputActionMap, newPhase, ref inputAction.m_OnCanceled, "canceled");
				return;
			default:
				return;
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000B228 File Offset: 0x00009428
		private void CallActionListeners(int actionIndex, InputActionMap actionMap, InputActionPhase phase, ref CallbackArray<Action<InputAction.CallbackContext>> listeners, string callbackName)
		{
			CallbackArray<Action<InputAction.CallbackContext>> actionCallbacks = actionMap.m_ActionCallbacks;
			if (listeners.length == 0 && actionCallbacks.length == 0 && InputActionState.s_GlobalState.onActionChange.length == 0)
			{
				return;
			}
			InputAction.CallbackContext callbackContext = new InputAction.CallbackContext
			{
				m_State = this,
				m_ActionIndex = actionIndex
			};
			InputAction action = callbackContext.action;
			if (InputActionState.s_GlobalState.onActionChange.length > 0)
			{
				InputActionChange inputActionChange;
				switch (phase)
				{
				case InputActionPhase.Started:
					inputActionChange = InputActionChange.ActionStarted;
					break;
				case InputActionPhase.Performed:
					inputActionChange = InputActionChange.ActionPerformed;
					break;
				case InputActionPhase.Canceled:
					inputActionChange = InputActionChange.ActionCanceled;
					break;
				default:
					return;
				}
				DelegateHelpers.InvokeCallbacksSafe<object, InputActionChange>(ref InputActionState.s_GlobalState.onActionChange, action, inputActionChange, InputActionState.k_InputOnActionChangeMarker, "InputSystem.onActionChange", null);
			}
			DelegateHelpers.InvokeCallbacksSafe<InputAction.CallbackContext>(ref listeners, callbackContext, callbackName, action);
			DelegateHelpers.InvokeCallbacksSafe<InputAction.CallbackContext>(ref actionCallbacks, callbackContext, callbackName, actionMap);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000B2EC File Offset: 0x000094EC
		private object GetActionOrNoneString(ref InputActionState.TriggerState trigger)
		{
			InputAction actionOrNull = this.GetActionOrNull(ref trigger);
			if (actionOrNull == null)
			{
				return "<none>";
			}
			return actionOrNull;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000B30C File Offset: 0x0000950C
		internal unsafe InputAction GetActionOrNull(int bindingIndex)
		{
			int actionIndex = this.bindingStates[bindingIndex].actionIndex;
			if (actionIndex == -1)
			{
				return null;
			}
			int mapIndex = this.bindingStates[bindingIndex].mapIndex;
			int actionStartIndex = this.mapIndices[mapIndex].actionStartIndex;
			return this.maps[mapIndex].m_Actions[actionIndex - actionStartIndex];
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000B374 File Offset: 0x00009574
		internal unsafe InputAction GetActionOrNull(ref InputActionState.TriggerState trigger)
		{
			int actionIndex = this.bindingStates[trigger.bindingIndex].actionIndex;
			if (actionIndex == -1)
			{
				return null;
			}
			int actionStartIndex = this.mapIndices[trigger.mapIndex].actionStartIndex;
			return this.maps[trigger.mapIndex].m_Actions[actionIndex - actionStartIndex];
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000B3D3 File Offset: 0x000095D3
		internal InputControl GetControl(ref InputActionState.TriggerState trigger)
		{
			return this.controls[trigger.controlIndex];
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000B3E2 File Offset: 0x000095E2
		private IInputInteraction GetInteractionOrNull(ref InputActionState.TriggerState trigger)
		{
			if (trigger.interactionIndex == -1)
			{
				return null;
			}
			return this.interactions[trigger.interactionIndex];
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000B3FC File Offset: 0x000095FC
		internal unsafe int GetBindingIndexInMap(int bindingIndex)
		{
			int mapIndex = this.bindingStates[bindingIndex].mapIndex;
			int bindingStartIndex = this.mapIndices[mapIndex].bindingStartIndex;
			return bindingIndex - bindingStartIndex;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000B438 File Offset: 0x00009638
		internal unsafe int GetBindingIndexInState(int mapIndex, int bindingIndexInMap)
		{
			return this.mapIndices[mapIndex].bindingStartIndex + bindingIndexInMap;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000B451 File Offset: 0x00009651
		internal unsafe ref InputActionState.BindingState GetBindingState(int bindingIndex)
		{
			return ref this.bindingStates[bindingIndex];
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000B464 File Offset: 0x00009664
		internal unsafe ref InputBinding GetBinding(int bindingIndex)
		{
			int mapIndex = this.bindingStates[bindingIndex].mapIndex;
			int bindingStartIndex = this.mapIndices[mapIndex].bindingStartIndex;
			return ref this.maps[mapIndex].m_Bindings[bindingIndex - bindingStartIndex];
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000B4B4 File Offset: 0x000096B4
		internal unsafe InputActionMap GetActionMap(int bindingIndex)
		{
			int mapIndex = this.bindingStates[bindingIndex].mapIndex;
			return this.maps[mapIndex];
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000B4E0 File Offset: 0x000096E0
		private unsafe void ResetInteractionStateAndCancelIfNecessary(int mapIndex, int bindingIndex, int interactionIndex, InputActionPhase phaseAfterCanceled)
		{
			int actionIndex = this.bindingStates[bindingIndex].actionIndex;
			if (this.actionStates[actionIndex].interactionIndex == interactionIndex)
			{
				InputActionPhase phase = this.interactionStates[interactionIndex].phase;
				if (phase - InputActionPhase.Started <= 1)
				{
					this.ChangePhaseOfInteraction(InputActionPhase.Canceled, ref this.actionStates[actionIndex], InputActionPhase.Waiting, phaseAfterCanceled, false);
				}
				this.actionStates[actionIndex].interactionIndex = -1;
			}
			this.ResetInteractionState(interactionIndex);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000B570 File Offset: 0x00009770
		private unsafe void ResetInteractionState(int interactionIndex)
		{
			this.interactions[interactionIndex].Reset();
			if (this.interactionStates[interactionIndex].isTimerRunning)
			{
				this.StopTimeout(interactionIndex);
			}
			this.interactionStates[interactionIndex] = new InputActionState.InteractionState
			{
				phase = InputActionPhase.Waiting,
				triggerControlIndex = -1
			};
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000B5D8 File Offset: 0x000097D8
		internal unsafe int GetValueSizeInBytes(int bindingIndex, int controlIndex)
		{
			if (this.bindingStates[bindingIndex].isPartOfComposite)
			{
				int compositeOrCompositeBindingIndex = this.bindingStates[bindingIndex].compositeOrCompositeBindingIndex;
				int compositeOrCompositeBindingIndex2 = this.bindingStates[compositeOrCompositeBindingIndex].compositeOrCompositeBindingIndex;
				return this.composites[compositeOrCompositeBindingIndex2].valueSizeInBytes;
			}
			return this.controls[controlIndex].valueSizeInBytes;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000B644 File Offset: 0x00009844
		internal unsafe Type GetValueType(int bindingIndex, int controlIndex)
		{
			if (this.bindingStates[bindingIndex].isPartOfComposite)
			{
				int compositeOrCompositeBindingIndex = this.bindingStates[bindingIndex].compositeOrCompositeBindingIndex;
				int compositeOrCompositeBindingIndex2 = this.bindingStates[compositeOrCompositeBindingIndex].compositeOrCompositeBindingIndex;
				return this.composites[compositeOrCompositeBindingIndex2].valueType;
			}
			return this.controls[controlIndex].valueType;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000B6B0 File Offset: 0x000098B0
		internal static bool IsActuated(ref InputActionState.TriggerState trigger, float threshold = 0f)
		{
			float magnitude = trigger.magnitude;
			if (magnitude < 0f)
			{
				return true;
			}
			if (Mathf.Approximately(threshold, 0f))
			{
				return magnitude > 0f;
			}
			return magnitude >= threshold;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000B6EC File Offset: 0x000098EC
		internal unsafe void ReadValue(int bindingIndex, int controlIndex, void* buffer, int bufferSize, bool ignoreComposites = false)
		{
			InputControl inputControl = null;
			if (!ignoreComposites && this.bindingStates[bindingIndex].isPartOfComposite)
			{
				int compositeOrCompositeBindingIndex = this.bindingStates[bindingIndex].compositeOrCompositeBindingIndex;
				int compositeOrCompositeBindingIndex2 = this.bindingStates[compositeOrCompositeBindingIndex].compositeOrCompositeBindingIndex;
				InputBindingComposite inputBindingComposite = this.composites[compositeOrCompositeBindingIndex2];
				InputBindingCompositeContext inputBindingCompositeContext = new InputBindingCompositeContext
				{
					m_State = this,
					m_BindingIndex = compositeOrCompositeBindingIndex
				};
				inputBindingComposite.ReadValue(ref inputBindingCompositeContext, buffer, bufferSize);
				bindingIndex = compositeOrCompositeBindingIndex;
			}
			else
			{
				inputControl = this.controls[controlIndex];
				inputControl.ReadValueIntoBuffer(buffer, bufferSize);
			}
			int processorCount = this.bindingStates[bindingIndex].processorCount;
			if (processorCount > 0)
			{
				int processorStartIndex = this.bindingStates[bindingIndex].processorStartIndex;
				for (int i = 0; i < processorCount; i++)
				{
					this.processors[processorStartIndex + i].Process(buffer, bufferSize, inputControl);
				}
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000B7E0 File Offset: 0x000099E0
		internal unsafe TValue ReadValue<TValue>(int bindingIndex, int controlIndex, bool ignoreComposites = false) where TValue : struct
		{
			TValue tvalue = default(TValue);
			InputControl<TValue> inputControl = null;
			if (!ignoreComposites && this.bindingStates[bindingIndex].isPartOfComposite)
			{
				int compositeOrCompositeBindingIndex = this.bindingStates[bindingIndex].compositeOrCompositeBindingIndex;
				int compositeOrCompositeBindingIndex2 = this.bindingStates[compositeOrCompositeBindingIndex].compositeOrCompositeBindingIndex;
				InputBindingComposite inputBindingComposite = this.composites[compositeOrCompositeBindingIndex2];
				InputBindingCompositeContext inputBindingCompositeContext = new InputBindingCompositeContext
				{
					m_State = this,
					m_BindingIndex = compositeOrCompositeBindingIndex
				};
				InputBindingComposite<TValue> inputBindingComposite2 = inputBindingComposite as InputBindingComposite<TValue>;
				if (inputBindingComposite2 == null)
				{
					Type valueType = inputBindingComposite.valueType;
					if (!valueType.IsAssignableFrom(typeof(TValue)))
					{
						throw new InvalidOperationException(string.Format("Cannot read value of type '{0}' from composite '{1}' bound to action '{2}' (composite is a '{3}' with value type '{4}')", new object[]
						{
							typeof(TValue).Name,
							inputBindingComposite,
							this.GetActionOrNull(bindingIndex),
							compositeOrCompositeBindingIndex2.GetType().Name,
							valueType.GetNiceTypeName()
						}));
					}
					inputBindingComposite.ReadValue(ref inputBindingCompositeContext, UnsafeUtility.AddressOf<TValue>(ref tvalue), UnsafeUtility.SizeOf<TValue>());
				}
				else
				{
					tvalue = inputBindingComposite2.ReadValue(ref inputBindingCompositeContext);
				}
				bindingIndex = compositeOrCompositeBindingIndex;
			}
			else if (controlIndex != -1)
			{
				InputControl inputControl2 = this.controls[controlIndex];
				inputControl = inputControl2 as InputControl<TValue>;
				if (inputControl == null)
				{
					throw new InvalidOperationException(string.Format("Cannot read value of type '{0}' from control '{1}' bound to action '{2}' (control is a '{3}' with value type '{4}')", new object[]
					{
						typeof(TValue).GetNiceTypeName(),
						inputControl2.path,
						this.GetActionOrNull(bindingIndex),
						inputControl2.GetType().Name,
						inputControl2.valueType.GetNiceTypeName()
					}));
				}
				tvalue = *inputControl.value;
			}
			return this.ApplyProcessors<TValue>(bindingIndex, tvalue, inputControl);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000B998 File Offset: 0x00009B98
		internal unsafe TValue ApplyProcessors<TValue>(int bindingIndex, TValue value, InputControl<TValue> controlOfType = null) where TValue : struct
		{
			if (this.totalBindingCount == 0)
			{
				return value;
			}
			int processorCount = this.bindingStates[bindingIndex].processorCount;
			if (processorCount > 0)
			{
				int processorStartIndex = this.bindingStates[bindingIndex].processorStartIndex;
				for (int i = 0; i < processorCount; i++)
				{
					InputProcessor<TValue> inputProcessor = this.processors[processorStartIndex + i] as InputProcessor<TValue>;
					if (inputProcessor != null)
					{
						value = inputProcessor.Process(value, controlOfType);
					}
				}
			}
			return value;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000BA0C File Offset: 0x00009C0C
		public unsafe float EvaluateCompositePartMagnitude(int bindingIndex, int partNumber)
		{
			int num = bindingIndex + 1;
			float num2 = float.MinValue;
			int num3 = num;
			while (num3 < this.totalBindingCount && this.bindingStates[num3].isPartOfComposite)
			{
				if (this.bindingStates[num3].partIndex == partNumber)
				{
					int controlCount = this.bindingStates[num3].controlCount;
					int controlStartIndex = this.bindingStates[num3].controlStartIndex;
					for (int i = 0; i < controlCount; i++)
					{
						num2 = Mathf.Max(this.controls[controlStartIndex + i].magnitude, num2);
					}
				}
				num3++;
			}
			return num2;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000BAB8 File Offset: 0x00009CB8
		internal unsafe double GetCompositePartPressTime(int bindingIndex, int partNumber)
		{
			int num = bindingIndex + 1;
			double num2 = double.MaxValue;
			int num3 = num;
			while (num3 < this.totalBindingCount && this.bindingStates[num3].isPartOfComposite)
			{
				ref InputActionState.BindingState ptr = ref this.bindingStates[num3];
				if (ptr.partIndex == partNumber && ptr.pressTime != 0.0 && ptr.pressTime < num2)
				{
					num2 = ptr.pressTime;
				}
				num3++;
			}
			if (num2 == 1.7976931348623157E+308)
			{
				return -1.0;
			}
			return num2;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000BB4C File Offset: 0x00009D4C
		internal unsafe TValue ReadCompositePartValue<TValue, TComparer>(int bindingIndex, int partNumber, bool* buttonValuePtr, out int controlIndex, TComparer comparer = default(TComparer)) where TValue : struct where TComparer : IComparer<TValue>
		{
			TValue tvalue = default(TValue);
			int num = bindingIndex + 1;
			bool flag = true;
			controlIndex = -1;
			int num2 = num;
			while (num2 < this.totalBindingCount && this.bindingStates[num2].isPartOfComposite)
			{
				if (this.bindingStates[num2].partIndex == partNumber)
				{
					int controlCount = this.bindingStates[num2].controlCount;
					int controlStartIndex = this.bindingStates[num2].controlStartIndex;
					for (int i = 0; i < controlCount; i++)
					{
						int num3 = controlStartIndex + i;
						TValue tvalue2 = this.ReadValue<TValue>(num2, num3, true);
						if (flag)
						{
							tvalue = tvalue2;
							controlIndex = num3;
							flag = false;
						}
						else if (comparer.Compare(tvalue2, tvalue) > 0)
						{
							tvalue = tvalue2;
							controlIndex = num3;
						}
						if (buttonValuePtr != null && controlIndex == num3)
						{
							InputControl inputControl = this.controls[num3];
							ButtonControl buttonControl = inputControl as ButtonControl;
							if (buttonControl != null)
							{
								*buttonValuePtr = buttonControl.isPressed;
							}
							else if (inputControl is InputControl<float>)
							{
								void* ptr = UnsafeUtility.AddressOf<TValue>(ref tvalue2);
								*buttonValuePtr = *(float*)ptr >= ButtonControl.s_GlobalDefaultButtonPressPoint;
							}
						}
					}
				}
				num2++;
			}
			return tvalue;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000BC84 File Offset: 0x00009E84
		internal unsafe bool ReadCompositePartValue(int bindingIndex, int partNumber, void* buffer, int bufferSize)
		{
			int num = bindingIndex + 1;
			float num2 = float.MinValue;
			int num3 = num;
			while (num3 < this.totalBindingCount && this.bindingStates[num3].isPartOfComposite)
			{
				if (this.bindingStates[num3].partIndex == partNumber)
				{
					int controlCount = this.bindingStates[num3].controlCount;
					int controlStartIndex = this.bindingStates[num3].controlStartIndex;
					for (int i = 0; i < controlCount; i++)
					{
						int num4 = controlStartIndex + i;
						float magnitude = this.controls[num4].magnitude;
						if (magnitude >= num2)
						{
							this.ReadValue(num3, num4, buffer, bufferSize, true);
							num2 = magnitude;
						}
					}
				}
				num3++;
			}
			return num2 > float.MinValue;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000BD50 File Offset: 0x00009F50
		internal unsafe object ReadCompositePartValueAsObject(int bindingIndex, int partNumber)
		{
			int num = bindingIndex + 1;
			float num2 = float.MinValue;
			object obj = null;
			int num3 = num;
			while (num3 < this.totalBindingCount && this.bindingStates[num3].isPartOfComposite)
			{
				if (this.bindingStates[num3].partIndex == partNumber)
				{
					int controlCount = this.bindingStates[num3].controlCount;
					int controlStartIndex = this.bindingStates[num3].controlStartIndex;
					for (int i = 0; i < controlCount; i++)
					{
						int num4 = controlStartIndex + i;
						float magnitude = this.controls[num4].magnitude;
						if (magnitude >= num2)
						{
							obj = this.ReadValueAsObject(num3, num4, true);
							num2 = magnitude;
						}
					}
				}
				num3++;
			}
			return obj;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000BE18 File Offset: 0x0000A018
		internal unsafe object ReadValueAsObject(int bindingIndex, int controlIndex, bool ignoreComposites = false)
		{
			InputControl inputControl = null;
			object obj = null;
			if (!ignoreComposites && this.bindingStates[bindingIndex].isPartOfComposite)
			{
				int compositeOrCompositeBindingIndex = this.bindingStates[bindingIndex].compositeOrCompositeBindingIndex;
				int compositeOrCompositeBindingIndex2 = this.bindingStates[compositeOrCompositeBindingIndex].compositeOrCompositeBindingIndex;
				InputBindingComposite inputBindingComposite = this.composites[compositeOrCompositeBindingIndex2];
				InputBindingCompositeContext inputBindingCompositeContext = new InputBindingCompositeContext
				{
					m_State = this,
					m_BindingIndex = compositeOrCompositeBindingIndex
				};
				obj = inputBindingComposite.ReadValueAsObject(ref inputBindingCompositeContext);
				bindingIndex = compositeOrCompositeBindingIndex;
			}
			else if (controlIndex != -1)
			{
				inputControl = this.controls[controlIndex];
				obj = inputControl.ReadValueAsObject();
			}
			if (obj != null)
			{
				int processorCount = this.bindingStates[bindingIndex].processorCount;
				if (processorCount > 0)
				{
					int processorStartIndex = this.bindingStates[bindingIndex].processorStartIndex;
					for (int i = 0; i < processorCount; i++)
					{
						obj = this.processors[processorStartIndex + i].ProcessAsObject(obj, inputControl);
					}
				}
			}
			return obj;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000BF10 File Offset: 0x0000A110
		internal unsafe bool ReadValueAsButton(int bindingIndex, int controlIndex)
		{
			ButtonControl buttonControl = null;
			if (!this.bindingStates[bindingIndex].isPartOfComposite)
			{
				buttonControl = this.controls[controlIndex] as ButtonControl;
			}
			float num = this.ReadValue<float>(bindingIndex, controlIndex, false);
			if (buttonControl != null)
			{
				return num >= buttonControl.pressPointOrDefault;
			}
			return num >= ButtonControl.s_GlobalDefaultButtonPressPoint;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000BF6C File Offset: 0x0000A16C
		internal static ISavedState SaveAndResetState()
		{
			ISavedState savedState = new SavedStructState<InputActionState.GlobalState>(ref InputActionState.s_GlobalState, delegate(ref InputActionState.GlobalState state)
			{
				InputActionState.s_GlobalState = state;
			}, delegate
			{
				InputActionState.ResetGlobals();
			});
			InputActionState.s_GlobalState = default(InputActionState.GlobalState);
			return savedState;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000BFCC File Offset: 0x0000A1CC
		private void AddToGlobalList()
		{
			InputActionState.CompactGlobalList();
			GCHandle gchandle = GCHandle.Alloc(this, GCHandleType.Weak);
			InputActionState.s_GlobalState.globalList.AppendWithCapacity(gchandle, 10);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000BFFC File Offset: 0x0000A1FC
		private void RemoveMapFromGlobalList()
		{
			int length = InputActionState.s_GlobalState.globalList.length;
			for (int i = 0; i < length; i++)
			{
				if (InputActionState.s_GlobalState.globalList[i].Target == this)
				{
					InputActionState.s_GlobalState.globalList[i].Free();
					InputActionState.s_GlobalState.globalList.RemoveAtByMovingTailWithCapacity(i);
					return;
				}
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000C06C File Offset: 0x0000A26C
		private static void CompactGlobalList()
		{
			int length = InputActionState.s_GlobalState.globalList.length;
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				GCHandle gchandle = InputActionState.s_GlobalState.globalList[i];
				if (gchandle.IsAllocated && gchandle.Target != null)
				{
					if (num != i)
					{
						InputActionState.s_GlobalState.globalList[num] = gchandle;
					}
					num++;
				}
				else
				{
					if (gchandle.IsAllocated)
					{
						InputActionState.s_GlobalState.globalList[i].Free();
					}
					InputActionState.s_GlobalState.globalList[i] = default(GCHandle);
				}
			}
			InputActionState.s_GlobalState.globalList.length = num;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000C124 File Offset: 0x0000A324
		internal void NotifyListenersOfActionChange(InputActionChange change)
		{
			for (int i = 0; i < this.totalMapCount; i++)
			{
				InputActionMap inputActionMap = this.maps[i];
				if (inputActionMap.m_SingletonAction != null)
				{
					InputActionState.NotifyListenersOfActionChange(change, inputActionMap.m_SingletonAction);
				}
				else
				{
					if (!(inputActionMap.m_Asset == null))
					{
						InputActionState.NotifyListenersOfActionChange(change, inputActionMap.m_Asset);
						return;
					}
					InputActionState.NotifyListenersOfActionChange(change, inputActionMap);
				}
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000C185 File Offset: 0x0000A385
		internal static void NotifyListenersOfActionChange(InputActionChange change, object actionOrMapOrAsset)
		{
			DelegateHelpers.InvokeCallbacksSafe<object, InputActionChange>(ref InputActionState.s_GlobalState.onActionChange, actionOrMapOrAsset, change, InputActionState.k_InputOnActionChangeMarker, "InputSystem.onActionChange", null);
			if (change == InputActionChange.BoundControlsChanged)
			{
				DelegateHelpers.InvokeCallbacksSafe<object>(ref InputActionState.s_GlobalState.onActionControlsChanged, actionOrMapOrAsset, "onActionControlsChange", null);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000C1C0 File Offset: 0x0000A3C0
		private static void ResetGlobals()
		{
			InputActionState.DestroyAllActionMapStates();
			for (int i = 0; i < InputActionState.s_GlobalState.globalList.length; i++)
			{
				if (InputActionState.s_GlobalState.globalList[i].IsAllocated)
				{
					InputActionState.s_GlobalState.globalList[i].Free();
				}
			}
			InputActionState.s_GlobalState.globalList.length = 0;
			InputActionState.s_GlobalState.onActionChange.Clear();
			InputActionState.s_GlobalState.onActionControlsChanged.Clear();
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000C24C File Offset: 0x0000A44C
		internal unsafe static int FindAllEnabledActions(List<InputAction> result)
		{
			int num = 0;
			int length = InputActionState.s_GlobalState.globalList.length;
			for (int i = 0; i < length; i++)
			{
				GCHandle gchandle = InputActionState.s_GlobalState.globalList[i];
				if (gchandle.IsAllocated)
				{
					InputActionState inputActionState = (InputActionState)gchandle.Target;
					if (inputActionState != null)
					{
						int totalMapCount = inputActionState.totalMapCount;
						InputActionMap[] array = inputActionState.maps;
						for (int j = 0; j < totalMapCount; j++)
						{
							InputActionMap inputActionMap = array[j];
							if (inputActionMap.enabled)
							{
								InputAction[] actions = inputActionMap.m_Actions;
								int num2 = actions.Length;
								if (inputActionMap.m_EnabledActionsCount == num2)
								{
									result.AddRange(actions);
									num += num2;
								}
								else
								{
									int actionStartIndex = inputActionState.mapIndices[inputActionMap.m_MapIndexInState].actionStartIndex;
									for (int k = 0; k < num2; k++)
									{
										if (inputActionState.actionStates[actionStartIndex + k].phase != InputActionPhase.Disabled)
										{
											result.Add(actions[k]);
											num++;
										}
									}
								}
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000C370 File Offset: 0x0000A570
		internal static void OnDeviceChange(InputDevice device, InputDeviceChange change)
		{
			for (int i = 0; i < InputActionState.s_GlobalState.globalList.length; i++)
			{
				GCHandle gchandle = InputActionState.s_GlobalState.globalList[i];
				if (!gchandle.IsAllocated || gchandle.Target == null)
				{
					if (gchandle.IsAllocated)
					{
						InputActionState.s_GlobalState.globalList[i].Free();
					}
					InputActionState.s_GlobalState.globalList.RemoveAtWithCapacity(i);
					i--;
				}
				else
				{
					InputActionState inputActionState = (InputActionState)gchandle.Target;
					bool flag = true;
					switch (change)
					{
					case InputDeviceChange.Added:
						if (!inputActionState.CanUseDevice(device))
						{
							goto IL_0155;
						}
						flag = false;
						break;
					case InputDeviceChange.Removed:
					{
						if (!inputActionState.IsUsingDevice(device))
						{
							goto IL_0155;
						}
						for (int j = 0; j < inputActionState.totalMapCount; j++)
						{
							InputActionMap inputActionMap = inputActionState.maps[j];
							inputActionMap.m_Devices.Remove(device);
							InputActionAsset asset = inputActionMap.asset;
							if (asset != null)
							{
								asset.m_Devices.Remove(device);
							}
						}
						flag = false;
						break;
					}
					case InputDeviceChange.UsageChanged:
					case InputDeviceChange.ConfigurationChanged:
						if (!inputActionState.IsUsingDevice(device) && !inputActionState.CanUseDevice(device))
						{
							goto IL_0155;
						}
						break;
					case InputDeviceChange.SoftReset:
					case InputDeviceChange.HardReset:
						if (inputActionState.IsUsingDevice(device))
						{
							inputActionState.ResetActionStatesDrivenBy(device);
							goto IL_0155;
						}
						goto IL_0155;
					}
					int num = 0;
					while (num < inputActionState.totalMapCount && !inputActionState.maps[num].LazyResolveBindings(flag))
					{
						num++;
					}
				}
				IL_0155:;
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000C4EC File Offset: 0x0000A6EC
		internal static void DeferredResolutionOfBindings()
		{
			InputActionMap.s_DeferBindingResolution++;
			try
			{
				if (InputActionMap.s_NeedToResolveBindings)
				{
					for (int i = 0; i < InputActionState.s_GlobalState.globalList.length; i++)
					{
						GCHandle gchandle = InputActionState.s_GlobalState.globalList[i];
						InputActionState inputActionState = (gchandle.IsAllocated ? ((InputActionState)gchandle.Target) : null);
						if (inputActionState == null)
						{
							if (gchandle.IsAllocated)
							{
								InputActionState.s_GlobalState.globalList[i].Free();
							}
							InputActionState.s_GlobalState.globalList.RemoveAtWithCapacity(i);
							i--;
						}
						else
						{
							for (int j = 0; j < inputActionState.totalMapCount; j++)
							{
								inputActionState.maps[j].ResolveBindingsIfNecessary();
							}
						}
					}
					InputActionMap.s_NeedToResolveBindings = false;
				}
			}
			finally
			{
				InputActionMap.s_DeferBindingResolution--;
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000C5DC File Offset: 0x0000A7DC
		internal static void DisableAllActions()
		{
			for (int i = 0; i < InputActionState.s_GlobalState.globalList.length; i++)
			{
				GCHandle gchandle = InputActionState.s_GlobalState.globalList[i];
				if (gchandle.IsAllocated && gchandle.Target != null)
				{
					InputActionState inputActionState = (InputActionState)gchandle.Target;
					int totalMapCount = inputActionState.totalMapCount;
					InputActionMap[] array = inputActionState.maps;
					for (int j = 0; j < totalMapCount; j++)
					{
						array[j].Disable();
					}
				}
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000C658 File Offset: 0x0000A858
		internal static void DestroyAllActionMapStates()
		{
			while (InputActionState.s_GlobalState.globalList.length > 0)
			{
				int num = InputActionState.s_GlobalState.globalList.length - 1;
				GCHandle gchandle = InputActionState.s_GlobalState.globalList[num];
				if (!gchandle.IsAllocated || gchandle.Target == null)
				{
					if (gchandle.IsAllocated)
					{
						InputActionState.s_GlobalState.globalList[num].Free();
					}
					InputActionState.s_GlobalState.globalList.RemoveAtWithCapacity(num);
				}
				else
				{
					((InputActionState)gchandle.Target).Destroy(false);
				}
			}
		}

		// Token: 0x040000CA RID: 202
		public const int kInvalidIndex = -1;

		// Token: 0x040000CB RID: 203
		public InputActionMap[] maps;

		// Token: 0x040000CC RID: 204
		public InputControl[] controls;

		// Token: 0x040000CD RID: 205
		public IInputInteraction[] interactions;

		// Token: 0x040000CE RID: 206
		public InputProcessor[] processors;

		// Token: 0x040000CF RID: 207
		public InputBindingComposite[] composites;

		// Token: 0x040000D0 RID: 208
		public int totalProcessorCount;

		// Token: 0x040000D1 RID: 209
		public InputActionState.UnmanagedMemory memory;

		// Token: 0x040000D2 RID: 210
		private bool m_OnBeforeUpdateHooked;

		// Token: 0x040000D3 RID: 211
		private bool m_OnAfterUpdateHooked;

		// Token: 0x040000D4 RID: 212
		private bool m_InProcessControlStateChange;

		// Token: 0x040000D5 RID: 213
		private InputEventPtr m_CurrentlyProcessingThisEvent;

		// Token: 0x040000D6 RID: 214
		private Action m_OnBeforeUpdateDelegate;

		// Token: 0x040000D7 RID: 215
		private Action m_OnAfterUpdateDelegate;

		// Token: 0x040000D8 RID: 216
		private static readonly ProfilerMarker k_InputInitialActionStateCheckMarker = new ProfilerMarker("InitialActionStateCheck");

		// Token: 0x040000D9 RID: 217
		private static readonly ProfilerMarker k_InputActionResolveConflictMarker = new ProfilerMarker("InputActionResolveConflict");

		// Token: 0x040000DA RID: 218
		private static readonly ProfilerMarker k_InputActionCallbackMarker = new ProfilerMarker("InputActionCallback");

		// Token: 0x040000DB RID: 219
		private static readonly ProfilerMarker k_InputOnActionChangeMarker = new ProfilerMarker("InpustSystem.onActionChange");

		// Token: 0x040000DC RID: 220
		private static readonly ProfilerMarker k_InputOnDeviceChangeMarker = new ProfilerMarker("InpustSystem.onDeviceChange");

		// Token: 0x040000DD RID: 221
		internal static InputActionState.GlobalState s_GlobalState;

		// Token: 0x02000177 RID: 375
		[StructLayout(LayoutKind.Explicit, Size = 48)]
		internal struct InteractionState
		{
			// Token: 0x17000517 RID: 1303
			// (get) Token: 0x06001329 RID: 4905 RVA: 0x0005ACB6 File Offset: 0x00058EB6
			// (set) Token: 0x0600132A RID: 4906 RVA: 0x0005ACCD File Offset: 0x00058ECD
			public int triggerControlIndex
			{
				get
				{
					if (this.m_TriggerControlIndex == 65535)
					{
						return -1;
					}
					return (int)this.m_TriggerControlIndex;
				}
				set
				{
					if (value == -1)
					{
						this.m_TriggerControlIndex = ushort.MaxValue;
						return;
					}
					if (value < 0 || value >= 65535)
					{
						throw new NotSupportedException("More than ushort.MaxValue-1 controls in a single InputActionState");
					}
					this.m_TriggerControlIndex = (ushort)value;
				}
			}

			// Token: 0x17000518 RID: 1304
			// (get) Token: 0x0600132B RID: 4907 RVA: 0x0005ACFE File Offset: 0x00058EFE
			// (set) Token: 0x0600132C RID: 4908 RVA: 0x0005AD06 File Offset: 0x00058F06
			public double startTime
			{
				get
				{
					return this.m_StartTime;
				}
				set
				{
					this.m_StartTime = value;
				}
			}

			// Token: 0x17000519 RID: 1305
			// (get) Token: 0x0600132D RID: 4909 RVA: 0x0005AD0F File Offset: 0x00058F0F
			// (set) Token: 0x0600132E RID: 4910 RVA: 0x0005AD17 File Offset: 0x00058F17
			public double performedTime
			{
				get
				{
					return this.m_PerformedTime;
				}
				set
				{
					this.m_PerformedTime = value;
				}
			}

			// Token: 0x1700051A RID: 1306
			// (get) Token: 0x0600132F RID: 4911 RVA: 0x0005AD20 File Offset: 0x00058F20
			// (set) Token: 0x06001330 RID: 4912 RVA: 0x0005AD28 File Offset: 0x00058F28
			public double timerStartTime
			{
				get
				{
					return this.m_TimerStartTime;
				}
				set
				{
					this.m_TimerStartTime = value;
				}
			}

			// Token: 0x1700051B RID: 1307
			// (get) Token: 0x06001331 RID: 4913 RVA: 0x0005AD31 File Offset: 0x00058F31
			// (set) Token: 0x06001332 RID: 4914 RVA: 0x0005AD39 File Offset: 0x00058F39
			public float timerDuration
			{
				get
				{
					return this.m_TimerDuration;
				}
				set
				{
					this.m_TimerDuration = value;
				}
			}

			// Token: 0x1700051C RID: 1308
			// (get) Token: 0x06001333 RID: 4915 RVA: 0x0005AD42 File Offset: 0x00058F42
			// (set) Token: 0x06001334 RID: 4916 RVA: 0x0005AD4A File Offset: 0x00058F4A
			public float totalTimeoutCompletionDone
			{
				get
				{
					return this.m_TotalTimeoutCompletionTimeDone;
				}
				set
				{
					this.m_TotalTimeoutCompletionTimeDone = value;
				}
			}

			// Token: 0x1700051D RID: 1309
			// (get) Token: 0x06001335 RID: 4917 RVA: 0x0005AD53 File Offset: 0x00058F53
			// (set) Token: 0x06001336 RID: 4918 RVA: 0x0005AD5B File Offset: 0x00058F5B
			public float totalTimeoutCompletionTimeRemaining
			{
				get
				{
					return this.m_TotalTimeoutCompletionTimeRemaining;
				}
				set
				{
					this.m_TotalTimeoutCompletionTimeRemaining = value;
				}
			}

			// Token: 0x1700051E RID: 1310
			// (get) Token: 0x06001337 RID: 4919 RVA: 0x0005AD64 File Offset: 0x00058F64
			// (set) Token: 0x06001338 RID: 4920 RVA: 0x0005AD6C File Offset: 0x00058F6C
			public long timerMonitorIndex
			{
				get
				{
					return this.m_TimerMonitorIndex;
				}
				set
				{
					this.m_TimerMonitorIndex = value;
				}
			}

			// Token: 0x1700051F RID: 1311
			// (get) Token: 0x06001339 RID: 4921 RVA: 0x0005AD75 File Offset: 0x00058F75
			// (set) Token: 0x0600133A RID: 4922 RVA: 0x0005AD84 File Offset: 0x00058F84
			public bool isTimerRunning
			{
				get
				{
					return (this.m_Flags & 1) == 1;
				}
				set
				{
					if (value)
					{
						this.m_Flags |= 1;
						return;
					}
					InputActionState.InteractionState.Flags flags = ~InputActionState.InteractionState.Flags.TimerRunning;
					this.m_Flags &= (byte)flags;
				}
			}

			// Token: 0x17000520 RID: 1312
			// (get) Token: 0x0600133B RID: 4923 RVA: 0x0005ADB7 File Offset: 0x00058FB7
			// (set) Token: 0x0600133C RID: 4924 RVA: 0x0005ADBF File Offset: 0x00058FBF
			public InputActionPhase phase
			{
				get
				{
					return (InputActionPhase)this.m_Phase;
				}
				set
				{
					this.m_Phase = (byte)value;
				}
			}

			// Token: 0x0400081E RID: 2078
			[FieldOffset(0)]
			private ushort m_TriggerControlIndex;

			// Token: 0x0400081F RID: 2079
			[FieldOffset(2)]
			private byte m_Phase;

			// Token: 0x04000820 RID: 2080
			[FieldOffset(3)]
			private byte m_Flags;

			// Token: 0x04000821 RID: 2081
			[FieldOffset(4)]
			private float m_TimerDuration;

			// Token: 0x04000822 RID: 2082
			[FieldOffset(8)]
			private double m_StartTime;

			// Token: 0x04000823 RID: 2083
			[FieldOffset(16)]
			private double m_TimerStartTime;

			// Token: 0x04000824 RID: 2084
			[FieldOffset(24)]
			private double m_PerformedTime;

			// Token: 0x04000825 RID: 2085
			[FieldOffset(32)]
			private float m_TotalTimeoutCompletionTimeDone;

			// Token: 0x04000826 RID: 2086
			[FieldOffset(36)]
			private float m_TotalTimeoutCompletionTimeRemaining;

			// Token: 0x04000827 RID: 2087
			[FieldOffset(40)]
			private long m_TimerMonitorIndex;

			// Token: 0x0200026D RID: 621
			[Flags]
			private enum Flags
			{
				// Token: 0x04000CDC RID: 3292
				TimerRunning = 1
			}
		}

		// Token: 0x02000178 RID: 376
		[StructLayout(LayoutKind.Explicit, Size = 32)]
		internal struct BindingState
		{
			// Token: 0x17000521 RID: 1313
			// (get) Token: 0x0600133D RID: 4925 RVA: 0x0005ADC9 File Offset: 0x00058FC9
			// (set) Token: 0x0600133E RID: 4926 RVA: 0x0005ADD4 File Offset: 0x00058FD4
			public int controlStartIndex
			{
				get
				{
					return (int)this.m_ControlStartIndex;
				}
				set
				{
					if (value >= 65535)
					{
						throw new NotSupportedException("Total control count in state cannot exceed byte.MaxValue=" + ushort.MaxValue.ToString());
					}
					this.m_ControlStartIndex = (ushort)value;
				}
			}

			// Token: 0x17000522 RID: 1314
			// (get) Token: 0x0600133F RID: 4927 RVA: 0x0005AE0E File Offset: 0x0005900E
			// (set) Token: 0x06001340 RID: 4928 RVA: 0x0005AE18 File Offset: 0x00059018
			public int controlCount
			{
				get
				{
					return (int)this.m_ControlCount;
				}
				set
				{
					if (value >= 255)
					{
						throw new NotSupportedException("Control count per binding cannot exceed byte.MaxValue=" + byte.MaxValue.ToString());
					}
					this.m_ControlCount = (byte)value;
				}
			}

			// Token: 0x17000523 RID: 1315
			// (get) Token: 0x06001341 RID: 4929 RVA: 0x0005AE52 File Offset: 0x00059052
			// (set) Token: 0x06001342 RID: 4930 RVA: 0x0005AE6C File Offset: 0x0005906C
			public int interactionStartIndex
			{
				get
				{
					if (this.m_InteractionStartIndex == 65535)
					{
						return -1;
					}
					return (int)this.m_InteractionStartIndex;
				}
				set
				{
					if (value == -1)
					{
						this.m_InteractionStartIndex = ushort.MaxValue;
						return;
					}
					if (value >= 65535)
					{
						throw new NotSupportedException("Interaction count cannot exceed ushort.MaxValue=" + ushort.MaxValue.ToString());
					}
					this.m_InteractionStartIndex = (ushort)value;
				}
			}

			// Token: 0x17000524 RID: 1316
			// (get) Token: 0x06001343 RID: 4931 RVA: 0x0005AEB6 File Offset: 0x000590B6
			// (set) Token: 0x06001344 RID: 4932 RVA: 0x0005AEC0 File Offset: 0x000590C0
			public int interactionCount
			{
				get
				{
					return (int)this.m_InteractionCount;
				}
				set
				{
					if (value >= 255)
					{
						throw new NotSupportedException("Interaction count per binding cannot exceed byte.MaxValue=" + byte.MaxValue.ToString());
					}
					this.m_InteractionCount = (byte)value;
				}
			}

			// Token: 0x17000525 RID: 1317
			// (get) Token: 0x06001345 RID: 4933 RVA: 0x0005AEFA File Offset: 0x000590FA
			// (set) Token: 0x06001346 RID: 4934 RVA: 0x0005AF14 File Offset: 0x00059114
			public int processorStartIndex
			{
				get
				{
					if (this.m_ProcessorStartIndex == 65535)
					{
						return -1;
					}
					return (int)this.m_ProcessorStartIndex;
				}
				set
				{
					if (value == -1)
					{
						this.m_ProcessorStartIndex = ushort.MaxValue;
						return;
					}
					if (value >= 65535)
					{
						throw new NotSupportedException("Processor count cannot exceed ushort.MaxValue=" + ushort.MaxValue.ToString());
					}
					this.m_ProcessorStartIndex = (ushort)value;
				}
			}

			// Token: 0x17000526 RID: 1318
			// (get) Token: 0x06001347 RID: 4935 RVA: 0x0005AF5E File Offset: 0x0005915E
			// (set) Token: 0x06001348 RID: 4936 RVA: 0x0005AF68 File Offset: 0x00059168
			public int processorCount
			{
				get
				{
					return (int)this.m_ProcessorCount;
				}
				set
				{
					if (value >= 255)
					{
						throw new NotSupportedException("Processor count per binding cannot exceed byte.MaxValue=" + byte.MaxValue.ToString());
					}
					this.m_ProcessorCount = (byte)value;
				}
			}

			// Token: 0x17000527 RID: 1319
			// (get) Token: 0x06001349 RID: 4937 RVA: 0x0005AFA2 File Offset: 0x000591A2
			// (set) Token: 0x0600134A RID: 4938 RVA: 0x0005AFBC File Offset: 0x000591BC
			public int actionIndex
			{
				get
				{
					if (this.m_ActionIndex == 65535)
					{
						return -1;
					}
					return (int)this.m_ActionIndex;
				}
				set
				{
					if (value == -1)
					{
						this.m_ActionIndex = ushort.MaxValue;
						return;
					}
					if (value >= 65535)
					{
						throw new NotSupportedException("Action count cannot exceed ushort.MaxValue=" + ushort.MaxValue.ToString());
					}
					this.m_ActionIndex = (ushort)value;
				}
			}

			// Token: 0x17000528 RID: 1320
			// (get) Token: 0x0600134B RID: 4939 RVA: 0x0005B006 File Offset: 0x00059206
			// (set) Token: 0x0600134C RID: 4940 RVA: 0x0005B010 File Offset: 0x00059210
			public int mapIndex
			{
				get
				{
					return (int)this.m_MapIndex;
				}
				set
				{
					if (value >= 255)
					{
						throw new NotSupportedException("Map count cannot exceed byte.MaxValue=" + byte.MaxValue.ToString());
					}
					this.m_MapIndex = (byte)value;
				}
			}

			// Token: 0x17000529 RID: 1321
			// (get) Token: 0x0600134D RID: 4941 RVA: 0x0005B04A File Offset: 0x0005924A
			// (set) Token: 0x0600134E RID: 4942 RVA: 0x0005B064 File Offset: 0x00059264
			public int compositeOrCompositeBindingIndex
			{
				get
				{
					if (this.m_CompositeOrCompositeBindingIndex == 65535)
					{
						return -1;
					}
					return (int)this.m_CompositeOrCompositeBindingIndex;
				}
				set
				{
					if (value == -1)
					{
						this.m_CompositeOrCompositeBindingIndex = ushort.MaxValue;
						return;
					}
					if (value >= 65535)
					{
						throw new NotSupportedException("Composite count cannot exceed ushort.MaxValue=" + ushort.MaxValue.ToString());
					}
					this.m_CompositeOrCompositeBindingIndex = (ushort)value;
				}
			}

			// Token: 0x1700052A RID: 1322
			// (get) Token: 0x0600134F RID: 4943 RVA: 0x0005B0AE File Offset: 0x000592AE
			// (set) Token: 0x06001350 RID: 4944 RVA: 0x0005B0B6 File Offset: 0x000592B6
			public int triggerEventIdForComposite
			{
				get
				{
					return this.m_TriggerEventIdForComposite;
				}
				set
				{
					this.m_TriggerEventIdForComposite = value;
				}
			}

			// Token: 0x1700052B RID: 1323
			// (get) Token: 0x06001351 RID: 4945 RVA: 0x0005B0BF File Offset: 0x000592BF
			// (set) Token: 0x06001352 RID: 4946 RVA: 0x0005B0C7 File Offset: 0x000592C7
			public double pressTime
			{
				get
				{
					return this.m_PressTime;
				}
				set
				{
					this.m_PressTime = value;
				}
			}

			// Token: 0x1700052C RID: 1324
			// (get) Token: 0x06001353 RID: 4947 RVA: 0x0005B0D0 File Offset: 0x000592D0
			// (set) Token: 0x06001354 RID: 4948 RVA: 0x0005B0D8 File Offset: 0x000592D8
			public InputActionState.BindingState.Flags flags
			{
				get
				{
					return (InputActionState.BindingState.Flags)this.m_Flags;
				}
				set
				{
					this.m_Flags = (byte)value;
				}
			}

			// Token: 0x1700052D RID: 1325
			// (get) Token: 0x06001355 RID: 4949 RVA: 0x0005B0E2 File Offset: 0x000592E2
			// (set) Token: 0x06001356 RID: 4950 RVA: 0x0005B0EF File Offset: 0x000592EF
			public bool chainsWithNext
			{
				get
				{
					return (this.flags & InputActionState.BindingState.Flags.ChainsWithNext) == InputActionState.BindingState.Flags.ChainsWithNext;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.BindingState.Flags.ChainsWithNext;
						return;
					}
					this.flags &= ~InputActionState.BindingState.Flags.ChainsWithNext;
				}
			}

			// Token: 0x1700052E RID: 1326
			// (get) Token: 0x06001357 RID: 4951 RVA: 0x0005B112 File Offset: 0x00059312
			// (set) Token: 0x06001358 RID: 4952 RVA: 0x0005B11F File Offset: 0x0005931F
			public bool isEndOfChain
			{
				get
				{
					return (this.flags & InputActionState.BindingState.Flags.EndOfChain) == InputActionState.BindingState.Flags.EndOfChain;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.BindingState.Flags.EndOfChain;
						return;
					}
					this.flags &= ~InputActionState.BindingState.Flags.EndOfChain;
				}
			}

			// Token: 0x1700052F RID: 1327
			// (get) Token: 0x06001359 RID: 4953 RVA: 0x0005B142 File Offset: 0x00059342
			public bool isPartOfChain
			{
				get
				{
					return this.chainsWithNext || this.isEndOfChain;
				}
			}

			// Token: 0x17000530 RID: 1328
			// (get) Token: 0x0600135A RID: 4954 RVA: 0x0005B154 File Offset: 0x00059354
			// (set) Token: 0x0600135B RID: 4955 RVA: 0x0005B161 File Offset: 0x00059361
			public bool isComposite
			{
				get
				{
					return (this.flags & InputActionState.BindingState.Flags.Composite) == InputActionState.BindingState.Flags.Composite;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.BindingState.Flags.Composite;
						return;
					}
					this.flags &= ~InputActionState.BindingState.Flags.Composite;
				}
			}

			// Token: 0x17000531 RID: 1329
			// (get) Token: 0x0600135C RID: 4956 RVA: 0x0005B184 File Offset: 0x00059384
			// (set) Token: 0x0600135D RID: 4957 RVA: 0x0005B191 File Offset: 0x00059391
			public bool isPartOfComposite
			{
				get
				{
					return (this.flags & InputActionState.BindingState.Flags.PartOfComposite) == InputActionState.BindingState.Flags.PartOfComposite;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.BindingState.Flags.PartOfComposite;
						return;
					}
					this.flags &= ~InputActionState.BindingState.Flags.PartOfComposite;
				}
			}

			// Token: 0x17000532 RID: 1330
			// (get) Token: 0x0600135E RID: 4958 RVA: 0x0005B1B4 File Offset: 0x000593B4
			// (set) Token: 0x0600135F RID: 4959 RVA: 0x0005B1C2 File Offset: 0x000593C2
			public bool initialStateCheckPending
			{
				get
				{
					return (this.flags & InputActionState.BindingState.Flags.InitialStateCheckPending) > (InputActionState.BindingState.Flags)0;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.BindingState.Flags.InitialStateCheckPending;
						return;
					}
					this.flags &= ~InputActionState.BindingState.Flags.InitialStateCheckPending;
				}
			}

			// Token: 0x17000533 RID: 1331
			// (get) Token: 0x06001360 RID: 4960 RVA: 0x0005B1E6 File Offset: 0x000593E6
			// (set) Token: 0x06001361 RID: 4961 RVA: 0x0005B1F4 File Offset: 0x000593F4
			public bool wantsInitialStateCheck
			{
				get
				{
					return (this.flags & InputActionState.BindingState.Flags.WantsInitialStateCheck) > (InputActionState.BindingState.Flags)0;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.BindingState.Flags.WantsInitialStateCheck;
						return;
					}
					this.flags &= ~InputActionState.BindingState.Flags.WantsInitialStateCheck;
				}
			}

			// Token: 0x17000534 RID: 1332
			// (get) Token: 0x06001362 RID: 4962 RVA: 0x0005B218 File Offset: 0x00059418
			// (set) Token: 0x06001363 RID: 4963 RVA: 0x0005B220 File Offset: 0x00059420
			public int partIndex
			{
				get
				{
					return (int)this.m_PartIndex;
				}
				set
				{
					if (this.partIndex < 0)
					{
						throw new ArgumentOutOfRangeException("value", "Part index must not be negative");
					}
					if (this.partIndex > 255)
					{
						throw new InvalidOperationException("Part count must not exceed byte.MaxValue=" + byte.MaxValue.ToString());
					}
					this.m_PartIndex = (byte)value;
				}
			}

			// Token: 0x04000828 RID: 2088
			[FieldOffset(0)]
			private byte m_ControlCount;

			// Token: 0x04000829 RID: 2089
			[FieldOffset(1)]
			private byte m_InteractionCount;

			// Token: 0x0400082A RID: 2090
			[FieldOffset(2)]
			private byte m_ProcessorCount;

			// Token: 0x0400082B RID: 2091
			[FieldOffset(3)]
			private byte m_MapIndex;

			// Token: 0x0400082C RID: 2092
			[FieldOffset(4)]
			private byte m_Flags;

			// Token: 0x0400082D RID: 2093
			[FieldOffset(5)]
			private byte m_PartIndex;

			// Token: 0x0400082E RID: 2094
			[FieldOffset(6)]
			private ushort m_ActionIndex;

			// Token: 0x0400082F RID: 2095
			[FieldOffset(8)]
			private ushort m_CompositeOrCompositeBindingIndex;

			// Token: 0x04000830 RID: 2096
			[FieldOffset(10)]
			private ushort m_ProcessorStartIndex;

			// Token: 0x04000831 RID: 2097
			[FieldOffset(12)]
			private ushort m_InteractionStartIndex;

			// Token: 0x04000832 RID: 2098
			[FieldOffset(14)]
			private ushort m_ControlStartIndex;

			// Token: 0x04000833 RID: 2099
			[FieldOffset(16)]
			private double m_PressTime;

			// Token: 0x04000834 RID: 2100
			[FieldOffset(24)]
			private int m_TriggerEventIdForComposite;

			// Token: 0x04000835 RID: 2101
			[FieldOffset(28)]
			private int __padding;

			// Token: 0x0200026E RID: 622
			[Flags]
			public enum Flags
			{
				// Token: 0x04000CDE RID: 3294
				ChainsWithNext = 1,
				// Token: 0x04000CDF RID: 3295
				EndOfChain = 2,
				// Token: 0x04000CE0 RID: 3296
				Composite = 4,
				// Token: 0x04000CE1 RID: 3297
				PartOfComposite = 8,
				// Token: 0x04000CE2 RID: 3298
				InitialStateCheckPending = 16,
				// Token: 0x04000CE3 RID: 3299
				WantsInitialStateCheck = 32
			}
		}

		// Token: 0x02000179 RID: 377
		[StructLayout(LayoutKind.Explicit, Size = 56)]
		public struct TriggerState
		{
			// Token: 0x17000535 RID: 1333
			// (get) Token: 0x06001364 RID: 4964 RVA: 0x0005B278 File Offset: 0x00059478
			// (set) Token: 0x06001365 RID: 4965 RVA: 0x0005B280 File Offset: 0x00059480
			public InputActionPhase phase
			{
				get
				{
					return (InputActionPhase)this.m_Phase;
				}
				set
				{
					this.m_Phase = (byte)value;
				}
			}

			// Token: 0x17000536 RID: 1334
			// (get) Token: 0x06001366 RID: 4966 RVA: 0x0005B28A File Offset: 0x0005948A
			public bool isDisabled
			{
				get
				{
					return this.phase == InputActionPhase.Disabled;
				}
			}

			// Token: 0x17000537 RID: 1335
			// (get) Token: 0x06001367 RID: 4967 RVA: 0x0005B295 File Offset: 0x00059495
			public bool isWaiting
			{
				get
				{
					return this.phase == InputActionPhase.Waiting;
				}
			}

			// Token: 0x17000538 RID: 1336
			// (get) Token: 0x06001368 RID: 4968 RVA: 0x0005B2A0 File Offset: 0x000594A0
			public bool isStarted
			{
				get
				{
					return this.phase == InputActionPhase.Started;
				}
			}

			// Token: 0x17000539 RID: 1337
			// (get) Token: 0x06001369 RID: 4969 RVA: 0x0005B2AB File Offset: 0x000594AB
			public bool isPerformed
			{
				get
				{
					return this.phase == InputActionPhase.Performed;
				}
			}

			// Token: 0x1700053A RID: 1338
			// (get) Token: 0x0600136A RID: 4970 RVA: 0x0005B2B6 File Offset: 0x000594B6
			public bool isCanceled
			{
				get
				{
					return this.phase == InputActionPhase.Canceled;
				}
			}

			// Token: 0x1700053B RID: 1339
			// (get) Token: 0x0600136B RID: 4971 RVA: 0x0005B2C1 File Offset: 0x000594C1
			// (set) Token: 0x0600136C RID: 4972 RVA: 0x0005B2C9 File Offset: 0x000594C9
			public double time
			{
				get
				{
					return this.m_Time;
				}
				set
				{
					this.m_Time = value;
				}
			}

			// Token: 0x1700053C RID: 1340
			// (get) Token: 0x0600136D RID: 4973 RVA: 0x0005B2D2 File Offset: 0x000594D2
			// (set) Token: 0x0600136E RID: 4974 RVA: 0x0005B2DA File Offset: 0x000594DA
			public double startTime
			{
				get
				{
					return this.m_StartTime;
				}
				set
				{
					this.m_StartTime = value;
				}
			}

			// Token: 0x1700053D RID: 1341
			// (get) Token: 0x0600136F RID: 4975 RVA: 0x0005B2E3 File Offset: 0x000594E3
			// (set) Token: 0x06001370 RID: 4976 RVA: 0x0005B2EB File Offset: 0x000594EB
			public float magnitude
			{
				get
				{
					return this.m_Magnitude;
				}
				set
				{
					this.flags |= InputActionState.TriggerState.Flags.HaveMagnitude;
					this.m_Magnitude = value;
				}
			}

			// Token: 0x1700053E RID: 1342
			// (get) Token: 0x06001371 RID: 4977 RVA: 0x0005B302 File Offset: 0x00059502
			public bool haveMagnitude
			{
				get
				{
					return (this.flags & InputActionState.TriggerState.Flags.HaveMagnitude) > (InputActionState.TriggerState.Flags)0;
				}
			}

			// Token: 0x1700053F RID: 1343
			// (get) Token: 0x06001372 RID: 4978 RVA: 0x0005B30F File Offset: 0x0005950F
			// (set) Token: 0x06001373 RID: 4979 RVA: 0x0005B317 File Offset: 0x00059517
			public int mapIndex
			{
				get
				{
					return (int)this.m_MapIndex;
				}
				set
				{
					if (value < 0 || value > 255)
					{
						throw new NotSupportedException("More than byte.MaxValue InputActionMaps in a single InputActionState");
					}
					this.m_MapIndex = (byte)value;
				}
			}

			// Token: 0x17000540 RID: 1344
			// (get) Token: 0x06001374 RID: 4980 RVA: 0x0005B338 File Offset: 0x00059538
			// (set) Token: 0x06001375 RID: 4981 RVA: 0x0005B34F File Offset: 0x0005954F
			public int controlIndex
			{
				get
				{
					if (this.m_ControlIndex == 65535)
					{
						return -1;
					}
					return (int)this.m_ControlIndex;
				}
				set
				{
					if (value == -1)
					{
						this.m_ControlIndex = ushort.MaxValue;
						return;
					}
					if (value < 0 || value >= 65535)
					{
						throw new NotSupportedException("More than ushort.MaxValue-1 controls in a single InputActionState");
					}
					this.m_ControlIndex = (ushort)value;
				}
			}

			// Token: 0x17000541 RID: 1345
			// (get) Token: 0x06001376 RID: 4982 RVA: 0x0005B380 File Offset: 0x00059580
			// (set) Token: 0x06001377 RID: 4983 RVA: 0x0005B388 File Offset: 0x00059588
			public int bindingIndex
			{
				get
				{
					return (int)this.m_BindingIndex;
				}
				set
				{
					if (value < 0 || value > 65535)
					{
						throw new NotSupportedException("More than ushort.MaxValue bindings in a single InputActionState");
					}
					this.m_BindingIndex = (ushort)value;
				}
			}

			// Token: 0x17000542 RID: 1346
			// (get) Token: 0x06001378 RID: 4984 RVA: 0x0005B3A9 File Offset: 0x000595A9
			// (set) Token: 0x06001379 RID: 4985 RVA: 0x0005B3C0 File Offset: 0x000595C0
			public int interactionIndex
			{
				get
				{
					if (this.m_InteractionIndex == 65535)
					{
						return -1;
					}
					return (int)this.m_InteractionIndex;
				}
				set
				{
					if (value == -1)
					{
						this.m_InteractionIndex = ushort.MaxValue;
						return;
					}
					if (value < 0 || value >= 65535)
					{
						throw new NotSupportedException("More than ushort.MaxValue-1 interactions in a single InputActionState");
					}
					this.m_InteractionIndex = (ushort)value;
				}
			}

			// Token: 0x17000543 RID: 1347
			// (get) Token: 0x0600137A RID: 4986 RVA: 0x0005B3F1 File Offset: 0x000595F1
			// (set) Token: 0x0600137B RID: 4987 RVA: 0x0005B3F9 File Offset: 0x000595F9
			public uint lastPerformedInUpdate
			{
				get
				{
					return this.m_LastPerformedInUpdate;
				}
				set
				{
					this.m_LastPerformedInUpdate = value;
				}
			}

			// Token: 0x17000544 RID: 1348
			// (get) Token: 0x0600137C RID: 4988 RVA: 0x0005B402 File Offset: 0x00059602
			// (set) Token: 0x0600137D RID: 4989 RVA: 0x0005B40A File Offset: 0x0005960A
			public uint lastCompletedInUpdate
			{
				get
				{
					return this.m_LastCompletedInUpdate;
				}
				set
				{
					this.m_LastCompletedInUpdate = value;
				}
			}

			// Token: 0x17000545 RID: 1349
			// (get) Token: 0x0600137E RID: 4990 RVA: 0x0005B413 File Offset: 0x00059613
			// (set) Token: 0x0600137F RID: 4991 RVA: 0x0005B41B File Offset: 0x0005961B
			public uint lastCanceledInUpdate
			{
				get
				{
					return this.m_LastCanceledInUpdate;
				}
				set
				{
					this.m_LastCanceledInUpdate = value;
				}
			}

			// Token: 0x17000546 RID: 1350
			// (get) Token: 0x06001380 RID: 4992 RVA: 0x0005B424 File Offset: 0x00059624
			// (set) Token: 0x06001381 RID: 4993 RVA: 0x0005B42C File Offset: 0x0005962C
			public uint pressedInUpdate
			{
				get
				{
					return this.m_PressedInUpdate;
				}
				set
				{
					this.m_PressedInUpdate = value;
				}
			}

			// Token: 0x17000547 RID: 1351
			// (get) Token: 0x06001382 RID: 4994 RVA: 0x0005B435 File Offset: 0x00059635
			// (set) Token: 0x06001383 RID: 4995 RVA: 0x0005B43D File Offset: 0x0005963D
			public uint releasedInUpdate
			{
				get
				{
					return this.m_ReleasedInUpdate;
				}
				set
				{
					this.m_ReleasedInUpdate = value;
				}
			}

			// Token: 0x17000548 RID: 1352
			// (get) Token: 0x06001384 RID: 4996 RVA: 0x0005B446 File Offset: 0x00059646
			// (set) Token: 0x06001385 RID: 4997 RVA: 0x0005B453 File Offset: 0x00059653
			public bool isPassThrough
			{
				get
				{
					return (this.flags & InputActionState.TriggerState.Flags.PassThrough) > (InputActionState.TriggerState.Flags)0;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.TriggerState.Flags.PassThrough;
						return;
					}
					this.flags &= ~InputActionState.TriggerState.Flags.PassThrough;
				}
			}

			// Token: 0x17000549 RID: 1353
			// (get) Token: 0x06001386 RID: 4998 RVA: 0x0005B476 File Offset: 0x00059676
			// (set) Token: 0x06001387 RID: 4999 RVA: 0x0005B484 File Offset: 0x00059684
			public bool isButton
			{
				get
				{
					return (this.flags & InputActionState.TriggerState.Flags.Button) > (InputActionState.TriggerState.Flags)0;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.TriggerState.Flags.Button;
						return;
					}
					this.flags &= ~InputActionState.TriggerState.Flags.Button;
				}
			}

			// Token: 0x1700054A RID: 1354
			// (get) Token: 0x06001388 RID: 5000 RVA: 0x0005B4A8 File Offset: 0x000596A8
			// (set) Token: 0x06001389 RID: 5001 RVA: 0x0005B4B6 File Offset: 0x000596B6
			public bool isPressed
			{
				get
				{
					return (this.flags & InputActionState.TriggerState.Flags.Pressed) > (InputActionState.TriggerState.Flags)0;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.TriggerState.Flags.Pressed;
						return;
					}
					this.flags &= ~InputActionState.TriggerState.Flags.Pressed;
				}
			}

			// Token: 0x1700054B RID: 1355
			// (get) Token: 0x0600138A RID: 5002 RVA: 0x0005B4DA File Offset: 0x000596DA
			// (set) Token: 0x0600138B RID: 5003 RVA: 0x0005B4E7 File Offset: 0x000596E7
			public bool mayNeedConflictResolution
			{
				get
				{
					return (this.flags & InputActionState.TriggerState.Flags.MayNeedConflictResolution) > (InputActionState.TriggerState.Flags)0;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.TriggerState.Flags.MayNeedConflictResolution;
						return;
					}
					this.flags &= ~InputActionState.TriggerState.Flags.MayNeedConflictResolution;
				}
			}

			// Token: 0x1700054C RID: 1356
			// (get) Token: 0x0600138C RID: 5004 RVA: 0x0005B50A File Offset: 0x0005970A
			// (set) Token: 0x0600138D RID: 5005 RVA: 0x0005B517 File Offset: 0x00059717
			public bool hasMultipleConcurrentActuations
			{
				get
				{
					return (this.flags & InputActionState.TriggerState.Flags.HasMultipleConcurrentActuations) > (InputActionState.TriggerState.Flags)0;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.TriggerState.Flags.HasMultipleConcurrentActuations;
						return;
					}
					this.flags &= ~InputActionState.TriggerState.Flags.HasMultipleConcurrentActuations;
				}
			}

			// Token: 0x1700054D RID: 1357
			// (get) Token: 0x0600138E RID: 5006 RVA: 0x0005B53A File Offset: 0x0005973A
			// (set) Token: 0x0600138F RID: 5007 RVA: 0x0005B548 File Offset: 0x00059748
			public bool inProcessing
			{
				get
				{
					return (this.flags & InputActionState.TriggerState.Flags.InProcessing) > (InputActionState.TriggerState.Flags)0;
				}
				set
				{
					if (value)
					{
						this.flags |= InputActionState.TriggerState.Flags.InProcessing;
						return;
					}
					this.flags &= ~InputActionState.TriggerState.Flags.InProcessing;
				}
			}

			// Token: 0x1700054E RID: 1358
			// (get) Token: 0x06001390 RID: 5008 RVA: 0x0005B56C File Offset: 0x0005976C
			// (set) Token: 0x06001391 RID: 5009 RVA: 0x0005B574 File Offset: 0x00059774
			public InputActionState.TriggerState.Flags flags
			{
				get
				{
					return (InputActionState.TriggerState.Flags)this.m_Flags;
				}
				set
				{
					this.m_Flags = (byte)value;
				}
			}

			// Token: 0x04000836 RID: 2102
			public const int kMaxNumMaps = 255;

			// Token: 0x04000837 RID: 2103
			public const int kMaxNumControls = 65535;

			// Token: 0x04000838 RID: 2104
			public const int kMaxNumBindings = 65535;

			// Token: 0x04000839 RID: 2105
			[FieldOffset(0)]
			private byte m_Phase;

			// Token: 0x0400083A RID: 2106
			[FieldOffset(1)]
			private byte m_Flags;

			// Token: 0x0400083B RID: 2107
			[FieldOffset(2)]
			private byte m_MapIndex;

			// Token: 0x0400083C RID: 2108
			[FieldOffset(4)]
			private ushort m_ControlIndex;

			// Token: 0x0400083D RID: 2109
			[FieldOffset(8)]
			private double m_Time;

			// Token: 0x0400083E RID: 2110
			[FieldOffset(16)]
			private double m_StartTime;

			// Token: 0x0400083F RID: 2111
			[FieldOffset(24)]
			private ushort m_BindingIndex;

			// Token: 0x04000840 RID: 2112
			[FieldOffset(26)]
			private ushort m_InteractionIndex;

			// Token: 0x04000841 RID: 2113
			[FieldOffset(28)]
			private float m_Magnitude;

			// Token: 0x04000842 RID: 2114
			[FieldOffset(32)]
			private uint m_LastPerformedInUpdate;

			// Token: 0x04000843 RID: 2115
			[FieldOffset(36)]
			private uint m_LastCanceledInUpdate;

			// Token: 0x04000844 RID: 2116
			[FieldOffset(40)]
			private uint m_PressedInUpdate;

			// Token: 0x04000845 RID: 2117
			[FieldOffset(44)]
			private uint m_ReleasedInUpdate;

			// Token: 0x04000846 RID: 2118
			[FieldOffset(48)]
			private uint m_LastCompletedInUpdate;

			// Token: 0x04000847 RID: 2119
			[FieldOffset(52)]
			internal int framePerformed;

			// Token: 0x04000848 RID: 2120
			[FieldOffset(56)]
			internal int framePressed;

			// Token: 0x04000849 RID: 2121
			[FieldOffset(60)]
			internal int frameReleased;

			// Token: 0x0400084A RID: 2122
			[FieldOffset(64)]
			internal int frameCompleted;

			// Token: 0x0200026F RID: 623
			[Flags]
			public enum Flags
			{
				// Token: 0x04000CE5 RID: 3301
				HaveMagnitude = 1,
				// Token: 0x04000CE6 RID: 3302
				PassThrough = 2,
				// Token: 0x04000CE7 RID: 3303
				MayNeedConflictResolution = 4,
				// Token: 0x04000CE8 RID: 3304
				HasMultipleConcurrentActuations = 8,
				// Token: 0x04000CE9 RID: 3305
				InProcessing = 16,
				// Token: 0x04000CEA RID: 3306
				Button = 32,
				// Token: 0x04000CEB RID: 3307
				Pressed = 64
			}
		}

		// Token: 0x0200017A RID: 378
		public struct ActionMapIndices
		{
			// Token: 0x0400084B RID: 2123
			public int actionStartIndex;

			// Token: 0x0400084C RID: 2124
			public int actionCount;

			// Token: 0x0400084D RID: 2125
			public int controlStartIndex;

			// Token: 0x0400084E RID: 2126
			public int controlCount;

			// Token: 0x0400084F RID: 2127
			public int bindingStartIndex;

			// Token: 0x04000850 RID: 2128
			public int bindingCount;

			// Token: 0x04000851 RID: 2129
			public int interactionStartIndex;

			// Token: 0x04000852 RID: 2130
			public int interactionCount;

			// Token: 0x04000853 RID: 2131
			public int processorStartIndex;

			// Token: 0x04000854 RID: 2132
			public int processorCount;

			// Token: 0x04000855 RID: 2133
			public int compositeStartIndex;

			// Token: 0x04000856 RID: 2134
			public int compositeCount;
		}

		// Token: 0x0200017B RID: 379
		public struct UnmanagedMemory : IDisposable
		{
			// Token: 0x1700054F RID: 1359
			// (get) Token: 0x06001392 RID: 5010 RVA: 0x0005B57E File Offset: 0x0005977E
			public bool isAllocated
			{
				get
				{
					return this.basePtr != null;
				}
			}

			// Token: 0x17000550 RID: 1360
			// (get) Token: 0x06001393 RID: 5011 RVA: 0x0005B590 File Offset: 0x00059790
			public unsafe int sizeInBytes
			{
				get
				{
					return this.mapCount * sizeof(InputActionState.ActionMapIndices) + this.actionCount * sizeof(InputActionState.TriggerState) + this.bindingCount * sizeof(InputActionState.BindingState) + this.interactionCount * sizeof(InputActionState.InteractionState) + this.controlCount * 4 + this.compositeCount * 4 + this.controlCount * 4 + this.controlCount * 2 * 2 + this.actionCount * 2 * 2 + this.bindingCount * 2 + (this.controlCount + 31) / 32 * 4;
				}
			}

			// Token: 0x06001394 RID: 5012 RVA: 0x0005B61D File Offset: 0x0005981D
			private unsafe static byte* AllocFromBlob(ref byte* top, int size)
			{
				if (size == 0)
				{
					return null;
				}
				byte* ptr = top;
				top += (IntPtr)size;
				return ptr;
			}

			// Token: 0x06001395 RID: 5013 RVA: 0x0005B630 File Offset: 0x00059830
			public unsafe void Allocate(int mapCount, int actionCount, int bindingCount, int controlCount, int interactionCount, int compositeCount)
			{
				this.mapCount = mapCount;
				this.actionCount = actionCount;
				this.interactionCount = interactionCount;
				this.bindingCount = bindingCount;
				this.controlCount = controlCount;
				this.compositeCount = compositeCount;
				int sizeInBytes = this.sizeInBytes;
				byte* ptr = (byte*)UnsafeUtility.Malloc((long)sizeInBytes, 8, Allocator.Persistent);
				UnsafeUtility.MemClear((void*)ptr, (long)sizeInBytes);
				this.basePtr = (void*)ptr;
				this.actionStates = (InputActionState.TriggerState*)InputActionState.UnmanagedMemory.AllocFromBlob(ref ptr, actionCount * sizeof(InputActionState.TriggerState));
				this.interactionStates = (InputActionState.InteractionState*)InputActionState.UnmanagedMemory.AllocFromBlob(ref ptr, interactionCount * sizeof(InputActionState.InteractionState));
				this.bindingStates = (InputActionState.BindingState*)InputActionState.UnmanagedMemory.AllocFromBlob(ref ptr, bindingCount * sizeof(InputActionState.BindingState));
				this.mapIndices = (InputActionState.ActionMapIndices*)InputActionState.UnmanagedMemory.AllocFromBlob(ref ptr, mapCount * sizeof(InputActionState.ActionMapIndices));
				this.controlMagnitudes = (float*)InputActionState.UnmanagedMemory.AllocFromBlob(ref ptr, controlCount * 4);
				this.compositeMagnitudes = (float*)InputActionState.UnmanagedMemory.AllocFromBlob(ref ptr, compositeCount * 4);
				this.controlIndexToBindingIndex = (int*)InputActionState.UnmanagedMemory.AllocFromBlob(ref ptr, controlCount * 4);
				this.controlGroupingAndComplexity = (ushort*)InputActionState.UnmanagedMemory.AllocFromBlob(ref ptr, controlCount * 2 * 2);
				this.actionBindingIndicesAndCounts = (ushort*)InputActionState.UnmanagedMemory.AllocFromBlob(ref ptr, actionCount * 2 * 2);
				this.actionBindingIndices = (ushort*)InputActionState.UnmanagedMemory.AllocFromBlob(ref ptr, bindingCount * 2);
				this.enabledControls = (int*)InputActionState.UnmanagedMemory.AllocFromBlob(ref ptr, (controlCount + 31) / 32 * 4);
			}

			// Token: 0x06001396 RID: 5014 RVA: 0x0005B760 File Offset: 0x00059960
			public void Dispose()
			{
				if (this.basePtr == null)
				{
					return;
				}
				UnsafeUtility.Free(this.basePtr, Allocator.Persistent);
				this.basePtr = null;
				this.actionStates = null;
				this.interactionStates = null;
				this.bindingStates = null;
				this.mapIndices = null;
				this.controlMagnitudes = null;
				this.compositeMagnitudes = null;
				this.controlIndexToBindingIndex = null;
				this.controlGroupingAndComplexity = null;
				this.actionBindingIndices = null;
				this.actionBindingIndicesAndCounts = null;
				this.mapCount = 0;
				this.actionCount = 0;
				this.bindingCount = 0;
				this.controlCount = 0;
				this.interactionCount = 0;
				this.compositeCount = 0;
			}

			// Token: 0x06001397 RID: 5015 RVA: 0x0005B808 File Offset: 0x00059A08
			public unsafe void CopyDataFrom(InputActionState.UnmanagedMemory memory)
			{
				UnsafeUtility.MemCpy((void*)this.mapIndices, (void*)memory.mapIndices, (long)(memory.mapCount * sizeof(InputActionState.ActionMapIndices)));
				UnsafeUtility.MemCpy((void*)this.actionStates, (void*)memory.actionStates, (long)(memory.actionCount * sizeof(InputActionState.TriggerState)));
				UnsafeUtility.MemCpy((void*)this.bindingStates, (void*)memory.bindingStates, (long)(memory.bindingCount * sizeof(InputActionState.BindingState)));
				UnsafeUtility.MemCpy((void*)this.interactionStates, (void*)memory.interactionStates, (long)(memory.interactionCount * sizeof(InputActionState.InteractionState)));
				UnsafeUtility.MemCpy((void*)this.controlMagnitudes, (void*)memory.controlMagnitudes, (long)(memory.controlCount * 4));
				UnsafeUtility.MemCpy((void*)this.compositeMagnitudes, (void*)memory.compositeMagnitudes, (long)(memory.compositeCount * 4));
				UnsafeUtility.MemCpy((void*)this.controlIndexToBindingIndex, (void*)memory.controlIndexToBindingIndex, (long)(memory.controlCount * 4));
				UnsafeUtility.MemCpy((void*)this.controlGroupingAndComplexity, (void*)memory.controlGroupingAndComplexity, (long)(memory.controlCount * 2 * 2));
				UnsafeUtility.MemCpy((void*)this.actionBindingIndicesAndCounts, (void*)memory.actionBindingIndicesAndCounts, (long)(memory.actionCount * 2 * 2));
				UnsafeUtility.MemCpy((void*)this.actionBindingIndices, (void*)memory.actionBindingIndices, (long)(memory.bindingCount * 2));
				UnsafeUtility.MemCpy((void*)this.enabledControls, (void*)memory.enabledControls, (long)((memory.controlCount + 31) / 32 * 4));
			}

			// Token: 0x06001398 RID: 5016 RVA: 0x0005B954 File Offset: 0x00059B54
			public InputActionState.UnmanagedMemory Clone()
			{
				if (!this.isAllocated)
				{
					return default(InputActionState.UnmanagedMemory);
				}
				InputActionState.UnmanagedMemory unmanagedMemory = default(InputActionState.UnmanagedMemory);
				int num = this.mapCount;
				int num2 = this.actionCount;
				int num3 = this.controlCount;
				unmanagedMemory.Allocate(num, num2, this.bindingCount, num3, this.interactionCount, this.compositeCount);
				unmanagedMemory.CopyDataFrom(this);
				return unmanagedMemory;
			}

			// Token: 0x04000857 RID: 2135
			public unsafe void* basePtr;

			// Token: 0x04000858 RID: 2136
			public int mapCount;

			// Token: 0x04000859 RID: 2137
			public int actionCount;

			// Token: 0x0400085A RID: 2138
			public int interactionCount;

			// Token: 0x0400085B RID: 2139
			public int bindingCount;

			// Token: 0x0400085C RID: 2140
			public int controlCount;

			// Token: 0x0400085D RID: 2141
			public int compositeCount;

			// Token: 0x0400085E RID: 2142
			public unsafe InputActionState.TriggerState* actionStates;

			// Token: 0x0400085F RID: 2143
			public unsafe InputActionState.BindingState* bindingStates;

			// Token: 0x04000860 RID: 2144
			public unsafe InputActionState.InteractionState* interactionStates;

			// Token: 0x04000861 RID: 2145
			public unsafe float* controlMagnitudes;

			// Token: 0x04000862 RID: 2146
			public unsafe float* compositeMagnitudes;

			// Token: 0x04000863 RID: 2147
			public unsafe int* enabledControls;

			// Token: 0x04000864 RID: 2148
			public unsafe ushort* actionBindingIndicesAndCounts;

			// Token: 0x04000865 RID: 2149
			public unsafe ushort* actionBindingIndices;

			// Token: 0x04000866 RID: 2150
			public unsafe int* controlIndexToBindingIndex;

			// Token: 0x04000867 RID: 2151
			public unsafe ushort* controlGroupingAndComplexity;

			// Token: 0x04000868 RID: 2152
			public bool controlGroupingInitialized;

			// Token: 0x04000869 RID: 2153
			public unsafe InputActionState.ActionMapIndices* mapIndices;
		}

		// Token: 0x0200017C RID: 380
		internal struct GlobalState
		{
			// Token: 0x0400086A RID: 2154
			internal InlinedArray<GCHandle> globalList;

			// Token: 0x0400086B RID: 2155
			internal CallbackArray<Action<object, InputActionChange>> onActionChange;

			// Token: 0x0400086C RID: 2156
			internal CallbackArray<Action<object>> onActionControlsChanged;
		}
	}
}
