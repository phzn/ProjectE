using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200001B RID: 27
	[Serializable]
	public sealed class InputAction : ICloneable, IDisposable
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00003411 File Offset: 0x00001611
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00003419 File Offset: 0x00001619
		public InputActionType type
		{
			get
			{
				return this.m_Type;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00003421 File Offset: 0x00001621
		public Guid id
		{
			get
			{
				this.MakeSureIdIsInPlace();
				return new Guid(this.m_Id);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00003438 File Offset: 0x00001638
		internal Guid idDontGenerate
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_Id))
				{
					return default(Guid);
				}
				return new Guid(this.m_Id);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00003467 File Offset: 0x00001667
		// (set) Token: 0x06000137 RID: 311 RVA: 0x0000346F File Offset: 0x0000166F
		public string expectedControlType
		{
			get
			{
				return this.m_ExpectedControlType;
			}
			set
			{
				this.m_ExpectedControlType = value;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00003478 File Offset: 0x00001678
		public string processors
		{
			get
			{
				return this.m_Processors;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00003480 File Offset: 0x00001680
		public string interactions
		{
			get
			{
				return this.m_Interactions;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00003488 File Offset: 0x00001688
		public InputActionMap actionMap
		{
			get
			{
				if (!this.isSingletonAction)
				{
					return this.m_ActionMap;
				}
				return null;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000349A File Offset: 0x0000169A
		// (set) Token: 0x0600013C RID: 316 RVA: 0x000034A4 File Offset: 0x000016A4
		public InputBinding? bindingMask
		{
			get
			{
				return this.m_BindingMask;
			}
			set
			{
				if (value == this.m_BindingMask)
				{
					return;
				}
				if (value != null)
				{
					InputBinding value2 = value.Value;
					value2.action = this.name;
					value = new InputBinding?(value2);
				}
				this.m_BindingMask = value;
				InputActionMap orCreateActionMap = this.GetOrCreateActionMap();
				if (orCreateActionMap.m_State != null)
				{
					orCreateActionMap.LazyResolveBindings(true);
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00003533 File Offset: 0x00001733
		public ReadOnlyArray<InputBinding> bindings
		{
			get
			{
				return this.GetOrCreateActionMap().GetBindingsForSingleAction(this);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00003541 File Offset: 0x00001741
		public ReadOnlyArray<InputControl> controls
		{
			get
			{
				InputActionMap orCreateActionMap = this.GetOrCreateActionMap();
				orCreateActionMap.ResolveBindingsIfNecessary();
				return orCreateActionMap.GetControlsForSingleAction(this);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00003558 File Offset: 0x00001758
		public InputActionPhase phase
		{
			get
			{
				return this.currentState.phase;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00003573 File Offset: 0x00001773
		public bool inProgress
		{
			get
			{
				return this.phase.IsInProgress();
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00003580 File Offset: 0x00001780
		public bool enabled
		{
			get
			{
				return this.phase > InputActionPhase.Disabled;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000142 RID: 322 RVA: 0x0000358B File Offset: 0x0000178B
		// (remove) Token: 0x06000143 RID: 323 RVA: 0x00003599 File Offset: 0x00001799
		public event Action<InputAction.CallbackContext> started
		{
			add
			{
				this.m_OnStarted.AddCallback(value);
			}
			remove
			{
				this.m_OnStarted.RemoveCallback(value);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000144 RID: 324 RVA: 0x000035A7 File Offset: 0x000017A7
		// (remove) Token: 0x06000145 RID: 325 RVA: 0x000035B5 File Offset: 0x000017B5
		public event Action<InputAction.CallbackContext> canceled
		{
			add
			{
				this.m_OnCanceled.AddCallback(value);
			}
			remove
			{
				this.m_OnCanceled.RemoveCallback(value);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000146 RID: 326 RVA: 0x000035C3 File Offset: 0x000017C3
		// (remove) Token: 0x06000147 RID: 327 RVA: 0x000035D1 File Offset: 0x000017D1
		public event Action<InputAction.CallbackContext> performed
		{
			add
			{
				this.m_OnPerformed.AddCallback(value);
			}
			remove
			{
				this.m_OnPerformed.RemoveCallback(value);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000148 RID: 328 RVA: 0x000035DF File Offset: 0x000017DF
		public bool triggered
		{
			get
			{
				return this.WasPerformedThisFrame();
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000035E8 File Offset: 0x000017E8
		public unsafe InputControl activeControl
		{
			get
			{
				InputActionState state = this.GetOrCreateActionMap().m_State;
				if (state != null)
				{
					int controlIndex = state.actionStates[this.m_ActionIndexInState].controlIndex;
					if (controlIndex != -1)
					{
						return state.controls[controlIndex];
					}
				}
				return null;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00003630 File Offset: 0x00001830
		public unsafe Type activeValueType
		{
			get
			{
				InputActionState state = this.GetOrCreateActionMap().m_State;
				if (state != null)
				{
					InputActionState.TriggerState* ptr = state.actionStates + this.m_ActionIndexInState;
					int controlIndex = ptr->controlIndex;
					if (controlIndex != -1)
					{
						return state.GetValueType(ptr->bindingIndex, controlIndex);
					}
				}
				return null;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000367D File Offset: 0x0000187D
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00003694 File Offset: 0x00001894
		public bool wantsInitialStateCheck
		{
			get
			{
				return this.type == InputActionType.Value || (this.m_Flags & InputAction.ActionFlags.WantsInitialStateCheck) > (InputAction.ActionFlags)0;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= InputAction.ActionFlags.WantsInitialStateCheck;
					return;
				}
				this.m_Flags &= ~InputAction.ActionFlags.WantsInitialStateCheck;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000036B8 File Offset: 0x000018B8
		public InputAction()
		{
			this.m_Id = Guid.NewGuid().ToString();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000036EC File Offset: 0x000018EC
		public InputAction(string name = null, InputActionType type = InputActionType.Value, string binding = null, string interactions = null, string processors = null, string expectedControlType = null)
		{
			this.m_Name = name;
			this.m_Type = type;
			if (!string.IsNullOrEmpty(binding))
			{
				this.m_SingletonActionBindings = new InputBinding[]
				{
					new InputBinding
					{
						path = binding,
						interactions = interactions,
						processors = processors,
						action = this.m_Name,
						id = Guid.NewGuid()
					}
				};
				this.m_BindingsStartIndex = 0;
				this.m_BindingsCount = 1;
			}
			else
			{
				this.m_Interactions = interactions;
				this.m_Processors = processors;
			}
			this.m_ExpectedControlType = expectedControlType;
			this.m_Id = Guid.NewGuid().ToString();
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000037AC File Offset: 0x000019AC
		public void Dispose()
		{
			InputActionMap actionMap = this.m_ActionMap;
			if (actionMap == null)
			{
				return;
			}
			InputActionState state = actionMap.m_State;
			if (state == null)
			{
				return;
			}
			state.Dispose();
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000037C8 File Offset: 0x000019C8
		public override string ToString()
		{
			string text;
			if (this.m_Name == null)
			{
				text = "<Unnamed>";
			}
			else if (this.m_ActionMap != null && !this.isSingletonAction && !string.IsNullOrEmpty(this.m_ActionMap.name))
			{
				text = this.m_ActionMap.name + "/" + this.m_Name;
			}
			else
			{
				text = this.m_Name;
			}
			ReadOnlyArray<InputControl> controls = this.controls;
			if (controls.Count > 0)
			{
				text += "[";
				bool flag = true;
				foreach (InputControl inputControl in controls)
				{
					if (!flag)
					{
						text += ",";
					}
					text += inputControl.path;
					flag = false;
				}
				text += "]";
			}
			return text;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000038B4 File Offset: 0x00001AB4
		public void Enable()
		{
			using (InputAction.k_InputActionEnableProfilerMarker.Auto())
			{
				if (!this.enabled)
				{
					InputActionMap orCreateActionMap = this.GetOrCreateActionMap();
					orCreateActionMap.ResolveBindingsIfNecessary();
					orCreateActionMap.m_State.EnableSingleAction(this);
				}
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00003914 File Offset: 0x00001B14
		public void Disable()
		{
			using (InputAction.k_InputActionDisableProfilerMarker.Auto())
			{
				if (this.enabled)
				{
					this.m_ActionMap.m_State.DisableSingleAction(this);
				}
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000396C File Offset: 0x00001B6C
		public InputAction Clone()
		{
			return new InputAction(this.m_Name, this.m_Type, null, null, null, null)
			{
				m_SingletonActionBindings = this.bindings.ToArray(),
				m_BindingsCount = this.m_BindingsCount,
				m_ExpectedControlType = this.m_ExpectedControlType,
				m_Interactions = this.m_Interactions,
				m_Processors = this.m_Processors,
				m_Flags = this.m_Flags
			};
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000039DE File Offset: 0x00001BDE
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000039E8 File Offset: 0x00001BE8
		public unsafe TValue ReadValue<TValue>() where TValue : struct
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			if (state == null)
			{
				return default(TValue);
			}
			InputActionState.TriggerState* ptr = state.actionStates + this.m_ActionIndexInState;
			if (!ptr->phase.IsInProgress())
			{
				return state.ApplyProcessors<TValue>(ptr->bindingIndex, default(TValue), null);
			}
			return state.ReadValue<TValue>(ptr->bindingIndex, ptr->controlIndex, false);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00003A5C File Offset: 0x00001C5C
		public unsafe object ReadValueAsObject()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			if (state == null)
			{
				return null;
			}
			InputActionState.TriggerState* ptr = state.actionStates + this.m_ActionIndexInState;
			if (ptr->phase.IsInProgress())
			{
				int controlIndex = ptr->controlIndex;
				if (controlIndex != -1)
				{
					return state.ReadValueAsObject(ptr->bindingIndex, controlIndex, false);
				}
			}
			return null;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00003ABC File Offset: 0x00001CBC
		public unsafe float GetControlMagnitude()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			if (state != null)
			{
				InputActionState.TriggerState* ptr = state.actionStates + this.m_ActionIndexInState;
				if (ptr->haveMagnitude)
				{
					return ptr->magnitude;
				}
			}
			return 0f;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00003B03 File Offset: 0x00001D03
		public void Reset()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			if (state == null)
			{
				return;
			}
			state.ResetActionState(this.m_ActionIndexInState, this.enabled ? InputActionPhase.Waiting : InputActionPhase.Disabled, true);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00003B30 File Offset: 0x00001D30
		public unsafe bool IsPressed()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			return state != null && state.actionStates[this.m_ActionIndexInState].isPressed;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00003B6C File Offset: 0x00001D6C
		public unsafe bool IsInProgress()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			return state != null && state.actionStates[this.m_ActionIndexInState].phase.IsInProgress();
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00003BAC File Offset: 0x00001DAC
		private int ExpectedFrame()
		{
			int num = ((InputSystem.settings.updateMode == InputSettings.UpdateMode.ProcessEventsManually) ? 1 : 0);
			return Time.frameCount - num;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00003BD4 File Offset: 0x00001DD4
		public unsafe bool WasPressedThisFrame()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			if (state != null)
			{
				ref InputActionState.TriggerState ptr = ref state.actionStates[this.m_ActionIndexInState];
				uint s_UpdateStepCount = InputUpdate.s_UpdateStepCount;
				return ptr.pressedInUpdate == s_UpdateStepCount && s_UpdateStepCount > 0U;
			}
			return false;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00003C1C File Offset: 0x00001E1C
		public unsafe bool WasPressedThisDynamicUpdate()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			return state != null && state.actionStates[this.m_ActionIndexInState].framePressed == this.ExpectedFrame();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00003C60 File Offset: 0x00001E60
		public unsafe bool WasReleasedThisFrame()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			if (state != null)
			{
				ref InputActionState.TriggerState ptr = ref state.actionStates[this.m_ActionIndexInState];
				uint s_UpdateStepCount = InputUpdate.s_UpdateStepCount;
				return ptr.releasedInUpdate == s_UpdateStepCount && s_UpdateStepCount > 0U;
			}
			return false;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00003CA8 File Offset: 0x00001EA8
		public unsafe bool WasReleasedThisDynamicUpdate()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			return state != null && state.actionStates[this.m_ActionIndexInState].frameReleased == this.ExpectedFrame();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00003CEC File Offset: 0x00001EEC
		public unsafe bool WasPerformedThisFrame()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			if (state != null)
			{
				ref InputActionState.TriggerState ptr = ref state.actionStates[this.m_ActionIndexInState];
				uint s_UpdateStepCount = InputUpdate.s_UpdateStepCount;
				return ptr.lastPerformedInUpdate == s_UpdateStepCount && s_UpdateStepCount > 0U;
			}
			return false;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00003D34 File Offset: 0x00001F34
		public unsafe bool WasPerformedThisDynamicUpdate()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			return state != null && state.actionStates[this.m_ActionIndexInState].framePerformed == this.ExpectedFrame();
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00003D78 File Offset: 0x00001F78
		public unsafe bool WasCompletedThisFrame()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			if (state != null)
			{
				ref InputActionState.TriggerState ptr = ref state.actionStates[this.m_ActionIndexInState];
				uint s_UpdateStepCount = InputUpdate.s_UpdateStepCount;
				return ptr.lastCompletedInUpdate == s_UpdateStepCount && s_UpdateStepCount > 0U;
			}
			return false;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00003DC0 File Offset: 0x00001FC0
		public unsafe bool WasCompletedThisDynamicUpdate()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			return state != null && state.actionStates[this.m_ActionIndexInState].frameCompleted == this.ExpectedFrame();
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00003E04 File Offset: 0x00002004
		public unsafe float GetTimeoutCompletionPercentage()
		{
			InputActionState state = this.GetOrCreateActionMap().m_State;
			if (state == null)
			{
				return 0f;
			}
			ref InputActionState.TriggerState ptr = ref state.actionStates[this.m_ActionIndexInState];
			int interactionIndex = ptr.interactionIndex;
			if (interactionIndex == -1)
			{
				return (float)((ptr.phase == InputActionPhase.Performed) ? 1 : 0);
			}
			ref InputActionState.InteractionState ptr2 = ref state.interactionStates[interactionIndex];
			InputActionPhase phase = ptr2.phase;
			if (phase != InputActionPhase.Started)
			{
				if (phase != InputActionPhase.Performed)
				{
					return 0f;
				}
				return 1f;
			}
			else
			{
				float num = 0f;
				if (ptr2.isTimerRunning)
				{
					float timerDuration = ptr2.timerDuration;
					double num2 = ptr2.timerStartTime + (double)timerDuration - InputState.currentTime;
					if (num2 <= 0.0)
					{
						num = 1f;
					}
					else
					{
						num = (float)(((double)timerDuration - num2) / (double)timerDuration);
					}
				}
				if (ptr2.totalTimeoutCompletionTimeRemaining > 0f)
				{
					return (ptr2.totalTimeoutCompletionDone + num * ptr2.timerDuration) / (ptr2.totalTimeoutCompletionDone + ptr2.totalTimeoutCompletionTimeRemaining);
				}
				return num;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00003F07 File Offset: 0x00002107
		internal bool isSingletonAction
		{
			get
			{
				return this.m_ActionMap == null || this.m_ActionMap.m_SingletonAction == this;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00003F24 File Offset: 0x00002124
		private unsafe InputActionState.TriggerState currentState
		{
			get
			{
				if (this.m_ActionIndexInState == -1)
				{
					return default(InputActionState.TriggerState);
				}
				return *this.m_ActionMap.m_State.FetchActionState(this);
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00003F5A File Offset: 0x0000215A
		internal string MakeSureIdIsInPlace()
		{
			if (string.IsNullOrEmpty(this.m_Id))
			{
				this.GenerateId();
			}
			return this.m_Id;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00003F78 File Offset: 0x00002178
		internal void GenerateId()
		{
			this.m_Id = Guid.NewGuid().ToString();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00003F9E File Offset: 0x0000219E
		internal InputActionMap GetOrCreateActionMap()
		{
			if (this.m_ActionMap == null)
			{
				this.CreateInternalActionMapForSingletonAction();
			}
			return this.m_ActionMap;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00003FB4 File Offset: 0x000021B4
		private void CreateInternalActionMapForSingletonAction()
		{
			this.m_ActionMap = new InputActionMap
			{
				m_Actions = new InputAction[] { this },
				m_SingletonAction = this,
				m_Bindings = this.m_SingletonActionBindings
			};
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00003FF1 File Offset: 0x000021F1
		internal void RequestInitialStateCheckOnEnabledAction()
		{
			this.GetOrCreateActionMap().m_State.SetInitialStateCheckPending(this.m_ActionIndexInState, true);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000400C File Offset: 0x0000220C
		internal bool ActiveControlIsValid(InputControl control)
		{
			if (control == null)
			{
				return false;
			}
			InputDevice device = control.device;
			if (!device.added)
			{
				return false;
			}
			ReadOnlyArray<InputDevice>? devices = this.GetOrCreateActionMap().devices;
			return devices == null || devices.Value.ContainsReference(device);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00004058 File Offset: 0x00002258
		internal InputBinding? FindEffectiveBindingMask()
		{
			if (this.m_BindingMask != null)
			{
				return this.m_BindingMask;
			}
			InputActionMap actionMap = this.m_ActionMap;
			if (actionMap != null && actionMap.m_BindingMask != null)
			{
				return this.m_ActionMap.m_BindingMask;
			}
			InputActionMap actionMap2 = this.m_ActionMap;
			if (actionMap2 == null)
			{
				return null;
			}
			InputActionAsset asset = actionMap2.m_Asset;
			if (asset == null)
			{
				return null;
			}
			return asset.m_BindingMask;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000040CC File Offset: 0x000022CC
		internal int BindingIndexOnActionToBindingIndexOnMap(int indexOfBindingOnAction)
		{
			InputBinding[] bindings = this.GetOrCreateActionMap().m_Bindings;
			int num = bindings.LengthSafe<InputBinding>();
			string name = this.name;
			int num2 = -1;
			for (int i = 0; i < num; i++)
			{
				if (bindings[i].TriggersAction(this))
				{
					num2++;
					if (num2 == indexOfBindingOnAction)
					{
						return i;
					}
				}
			}
			throw new ArgumentOutOfRangeException("indexOfBindingOnAction", string.Format("Binding index {0} is out of range for action '{1}' with {2} bindings", indexOfBindingOnAction, this, num2 + 1));
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00004140 File Offset: 0x00002340
		internal int BindingIndexOnMapToBindingIndexOnAction(int indexOfBindingOnMap)
		{
			InputBinding[] bindings = this.GetOrCreateActionMap().m_Bindings;
			string name = this.name;
			int num = 0;
			for (int i = indexOfBindingOnMap - 1; i >= 0; i--)
			{
				ref InputBinding ptr = ref bindings[i];
				if (string.Compare(ptr.action, name, StringComparison.InvariantCultureIgnoreCase) == 0 || ptr.action == this.m_Id)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04000080 RID: 128
		private static readonly ProfilerMarker k_InputActionEnableProfilerMarker = new ProfilerMarker("InputAction.Enable");

		// Token: 0x04000081 RID: 129
		private static readonly ProfilerMarker k_InputActionDisableProfilerMarker = new ProfilerMarker("InputAction.Disable");

		// Token: 0x04000082 RID: 130
		[Tooltip("Human readable name of the action. Must be unique within its action map (case is ignored). Can be changed without breaking references to the action.")]
		[SerializeField]
		internal string m_Name;

		// Token: 0x04000083 RID: 131
		[Tooltip("Determines how the action triggers.\n\nA Value action will start and perform when a control moves from its default value and then perform on every value change. It will cancel when controls go back to default value. Also, when enabled, a Value action will respond right away to a control's current value.\n\nA Button action will start when a button is pressed and perform when the press threshold (see 'Default Button Press Point' in settings) is reached. It will cancel when the button is going below the release threshold (see 'Button Release Threshold' in settings). Also, if a button is already pressed when the action is enabled, the button has to be released first.\n\nA Pass-Through action will not explicitly start and will never cancel. Instead, for every value change on any bound control, the action will perform.")]
		[SerializeField]
		internal InputActionType m_Type;

		// Token: 0x04000084 RID: 132
		[FormerlySerializedAs("m_ExpectedControlLayout")]
		[Tooltip("The type of control expected by the action (e.g. \"Button\" or \"Stick\"). This will limit the controls shown when setting up bindings in the UI and will also limit which controls can be bound interactively to the action.")]
		[SerializeField]
		internal string m_ExpectedControlType;

		// Token: 0x04000085 RID: 133
		[Tooltip("Unique ID of the action (GUID). Used to reference the action from bindings such that actions can be renamed without breaking references.")]
		[SerializeField]
		internal string m_Id;

		// Token: 0x04000086 RID: 134
		[SerializeField]
		internal string m_Processors;

		// Token: 0x04000087 RID: 135
		[SerializeField]
		internal string m_Interactions;

		// Token: 0x04000088 RID: 136
		[SerializeField]
		internal InputBinding[] m_SingletonActionBindings;

		// Token: 0x04000089 RID: 137
		[SerializeField]
		internal InputAction.ActionFlags m_Flags;

		// Token: 0x0400008A RID: 138
		[NonSerialized]
		internal InputBinding? m_BindingMask;

		// Token: 0x0400008B RID: 139
		[NonSerialized]
		internal int m_BindingsStartIndex;

		// Token: 0x0400008C RID: 140
		[NonSerialized]
		internal int m_BindingsCount;

		// Token: 0x0400008D RID: 141
		[NonSerialized]
		internal int m_ControlStartIndex;

		// Token: 0x0400008E RID: 142
		[NonSerialized]
		internal int m_ControlCount;

		// Token: 0x0400008F RID: 143
		[NonSerialized]
		internal int m_ActionIndexInState = -1;

		// Token: 0x04000090 RID: 144
		[NonSerialized]
		internal InputActionMap m_ActionMap;

		// Token: 0x04000091 RID: 145
		[NonSerialized]
		internal CallbackArray<Action<InputAction.CallbackContext>> m_OnStarted;

		// Token: 0x04000092 RID: 146
		[NonSerialized]
		internal CallbackArray<Action<InputAction.CallbackContext>> m_OnCanceled;

		// Token: 0x04000093 RID: 147
		[NonSerialized]
		internal CallbackArray<Action<InputAction.CallbackContext>> m_OnPerformed;

		// Token: 0x02000158 RID: 344
		[Flags]
		internal enum ActionFlags
		{
			// Token: 0x0400077D RID: 1917
			WantsInitialStateCheck = 1
		}

		// Token: 0x02000159 RID: 345
		public struct CallbackContext
		{
			// Token: 0x170004F0 RID: 1264
			// (get) Token: 0x0600126F RID: 4719 RVA: 0x000574C0 File Offset: 0x000556C0
			private int actionIndex
			{
				get
				{
					return this.m_ActionIndex;
				}
			}

			// Token: 0x170004F1 RID: 1265
			// (get) Token: 0x06001270 RID: 4720 RVA: 0x000574C8 File Offset: 0x000556C8
			private unsafe int bindingIndex
			{
				get
				{
					return this.m_State.actionStates[this.actionIndex].bindingIndex;
				}
			}

			// Token: 0x170004F2 RID: 1266
			// (get) Token: 0x06001271 RID: 4721 RVA: 0x000574E9 File Offset: 0x000556E9
			private unsafe int controlIndex
			{
				get
				{
					return this.m_State.actionStates[this.actionIndex].controlIndex;
				}
			}

			// Token: 0x170004F3 RID: 1267
			// (get) Token: 0x06001272 RID: 4722 RVA: 0x0005750A File Offset: 0x0005570A
			private unsafe int interactionIndex
			{
				get
				{
					return this.m_State.actionStates[this.actionIndex].interactionIndex;
				}
			}

			// Token: 0x170004F4 RID: 1268
			// (get) Token: 0x06001273 RID: 4723 RVA: 0x0005752B File Offset: 0x0005572B
			public unsafe InputActionPhase phase
			{
				get
				{
					if (this.m_State == null)
					{
						return InputActionPhase.Disabled;
					}
					return this.m_State.actionStates[this.actionIndex].phase;
				}
			}

			// Token: 0x170004F5 RID: 1269
			// (get) Token: 0x06001274 RID: 4724 RVA: 0x00057556 File Offset: 0x00055756
			public bool started
			{
				get
				{
					return this.phase == InputActionPhase.Started;
				}
			}

			// Token: 0x170004F6 RID: 1270
			// (get) Token: 0x06001275 RID: 4725 RVA: 0x00057561 File Offset: 0x00055761
			public bool performed
			{
				get
				{
					return this.phase == InputActionPhase.Performed;
				}
			}

			// Token: 0x170004F7 RID: 1271
			// (get) Token: 0x06001276 RID: 4726 RVA: 0x0005756C File Offset: 0x0005576C
			public bool canceled
			{
				get
				{
					return this.phase == InputActionPhase.Canceled;
				}
			}

			// Token: 0x170004F8 RID: 1272
			// (get) Token: 0x06001277 RID: 4727 RVA: 0x00057577 File Offset: 0x00055777
			public InputAction action
			{
				get
				{
					InputActionState state = this.m_State;
					if (state == null)
					{
						return null;
					}
					return state.GetActionOrNull(this.bindingIndex);
				}
			}

			// Token: 0x170004F9 RID: 1273
			// (get) Token: 0x06001278 RID: 4728 RVA: 0x00057590 File Offset: 0x00055790
			public InputControl control
			{
				get
				{
					InputActionState state = this.m_State;
					if (state == null)
					{
						return null;
					}
					return state.controls[this.controlIndex];
				}
			}

			// Token: 0x170004FA RID: 1274
			// (get) Token: 0x06001279 RID: 4729 RVA: 0x000575AC File Offset: 0x000557AC
			public IInputInteraction interaction
			{
				get
				{
					if (this.m_State == null)
					{
						return null;
					}
					int interactionIndex = this.interactionIndex;
					if (interactionIndex == -1)
					{
						return null;
					}
					return this.m_State.interactions[interactionIndex];
				}
			}

			// Token: 0x170004FB RID: 1275
			// (get) Token: 0x0600127A RID: 4730 RVA: 0x000575DD File Offset: 0x000557DD
			public unsafe double time
			{
				get
				{
					if (this.m_State == null)
					{
						return 0.0;
					}
					return this.m_State.actionStates[this.actionIndex].time;
				}
			}

			// Token: 0x170004FC RID: 1276
			// (get) Token: 0x0600127B RID: 4731 RVA: 0x00057610 File Offset: 0x00055810
			public unsafe double startTime
			{
				get
				{
					if (this.m_State == null)
					{
						return 0.0;
					}
					return this.m_State.actionStates[this.actionIndex].startTime;
				}
			}

			// Token: 0x170004FD RID: 1277
			// (get) Token: 0x0600127C RID: 4732 RVA: 0x00057643 File Offset: 0x00055843
			public double duration
			{
				get
				{
					return this.time - this.startTime;
				}
			}

			// Token: 0x170004FE RID: 1278
			// (get) Token: 0x0600127D RID: 4733 RVA: 0x00057652 File Offset: 0x00055852
			public Type valueType
			{
				get
				{
					InputActionState state = this.m_State;
					if (state == null)
					{
						return null;
					}
					return state.GetValueType(this.bindingIndex, this.controlIndex);
				}
			}

			// Token: 0x170004FF RID: 1279
			// (get) Token: 0x0600127E RID: 4734 RVA: 0x00057671 File Offset: 0x00055871
			public int valueSizeInBytes
			{
				get
				{
					if (this.m_State == null)
					{
						return 0;
					}
					return this.m_State.GetValueSizeInBytes(this.bindingIndex, this.controlIndex);
				}
			}

			// Token: 0x0600127F RID: 4735 RVA: 0x00057694 File Offset: 0x00055894
			public unsafe void ReadValue(void* buffer, int bufferSize)
			{
				if (buffer == null)
				{
					throw new ArgumentNullException("buffer");
				}
				if (this.m_State != null && this.phase.IsInProgress())
				{
					this.m_State.ReadValue(this.bindingIndex, this.controlIndex, buffer, bufferSize, false);
					return;
				}
				int valueSizeInBytes = this.valueSizeInBytes;
				if (bufferSize < valueSizeInBytes)
				{
					throw new ArgumentException(string.Format("Expected buffer of at least {0} bytes but got buffer of only {1} bytes", valueSizeInBytes, bufferSize), "bufferSize");
				}
				UnsafeUtility.MemClear(buffer, (long)this.valueSizeInBytes);
			}

			// Token: 0x06001280 RID: 4736 RVA: 0x0005771C File Offset: 0x0005591C
			public TValue ReadValue<TValue>() where TValue : struct
			{
				TValue tvalue = default(TValue);
				if (this.m_State != null)
				{
					tvalue = (this.phase.IsInProgress() ? this.m_State.ReadValue<TValue>(this.bindingIndex, this.controlIndex, false) : this.m_State.ApplyProcessors<TValue>(this.bindingIndex, tvalue, null));
				}
				return tvalue;
			}

			// Token: 0x06001281 RID: 4737 RVA: 0x00057778 File Offset: 0x00055978
			public bool ReadValueAsButton()
			{
				bool flag = false;
				if (this.m_State != null && this.phase.IsInProgress())
				{
					flag = this.m_State.ReadValueAsButton(this.bindingIndex, this.controlIndex);
				}
				return flag;
			}

			// Token: 0x06001282 RID: 4738 RVA: 0x000577B5 File Offset: 0x000559B5
			public object ReadValueAsObject()
			{
				if (this.m_State != null && this.phase.IsInProgress())
				{
					return this.m_State.ReadValueAsObject(this.bindingIndex, this.controlIndex, false);
				}
				return null;
			}

			// Token: 0x06001283 RID: 4739 RVA: 0x000577E8 File Offset: 0x000559E8
			public override string ToString()
			{
				return string.Format("{{ action={0} phase={1} time={2} control={3} value={4} interaction={5} }}", new object[]
				{
					this.action,
					this.phase,
					this.time,
					this.control,
					this.ReadValueAsObject(),
					this.interaction
				});
			}

			// Token: 0x0400077E RID: 1918
			internal InputActionState m_State;

			// Token: 0x0400077F RID: 1919
			internal int m_ActionIndex;
		}
	}
}
