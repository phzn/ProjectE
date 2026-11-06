using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200001E RID: 30
	[Serializable]
	public sealed class InputActionMap : ICloneable, ISerializationCallbackReceiver, IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00004CA4 File Offset: 0x00002EA4
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00004CAC File Offset: 0x00002EAC
		public InputActionAsset asset
		{
			get
			{
				return this.m_Asset;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00004CB4 File Offset: 0x00002EB4
		public Guid id
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_Id))
				{
					this.GenerateId();
				}
				return new Guid(this.m_Id);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00004CD4 File Offset: 0x00002ED4
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

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00004D03 File Offset: 0x00002F03
		public bool enabled
		{
			get
			{
				return this.m_EnabledActionsCount > 0;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00004D0E File Offset: 0x00002F0E
		public ReadOnlyArray<InputAction> actions
		{
			get
			{
				return new ReadOnlyArray<InputAction>(this.m_Actions);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00004D1B File Offset: 0x00002F1B
		public ReadOnlyArray<InputBinding> bindings
		{
			get
			{
				return new ReadOnlyArray<InputBinding>(this.m_Bindings);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00004D28 File Offset: 0x00002F28
		IEnumerable<InputBinding> IInputActionCollection2.bindings
		{
			get
			{
				return this.bindings;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00004D38 File Offset: 0x00002F38
		public ReadOnlyArray<InputControlScheme> controlSchemes
		{
			get
			{
				if (this.m_Asset == null)
				{
					return default(ReadOnlyArray<InputControlScheme>);
				}
				return this.m_Asset.controlSchemes;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00004D68 File Offset: 0x00002F68
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00004D70 File Offset: 0x00002F70
		public InputBinding? bindingMask
		{
			get
			{
				return this.m_BindingMask;
			}
			set
			{
				if (this.m_BindingMask == value)
				{
					return;
				}
				this.m_BindingMask = value;
				this.LazyResolveBindings(true);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00004DCC File Offset: 0x00002FCC
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00004E09 File Offset: 0x00003009
		public ReadOnlyArray<InputDevice>? devices
		{
			get
			{
				ReadOnlyArray<InputDevice>? readOnlyArray = this.m_Devices.Get();
				if (readOnlyArray != null)
				{
					return readOnlyArray;
				}
				InputActionAsset asset = this.m_Asset;
				if (asset == null)
				{
					return null;
				}
				return asset.devices;
			}
			set
			{
				if (this.m_Devices.Set(value))
				{
					this.LazyResolveBindings(false);
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		public InputAction this[string actionNameOrId]
		{
			get
			{
				if (actionNameOrId == null)
				{
					throw new ArgumentNullException("actionNameOrId");
				}
				InputAction inputAction = this.FindAction(actionNameOrId, false);
				if (inputAction == null)
				{
					throw new KeyNotFoundException("Cannot find action '" + actionNameOrId + "'");
				}
				return inputAction;
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060001A1 RID: 417 RVA: 0x00004E52 File Offset: 0x00003052
		// (remove) Token: 0x060001A2 RID: 418 RVA: 0x00004E60 File Offset: 0x00003060
		public event Action<InputAction.CallbackContext> actionTriggered
		{
			add
			{
				this.m_ActionCallbacks.AddCallback(value);
			}
			remove
			{
				this.m_ActionCallbacks.RemoveCallback(value);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00004E6E File Offset: 0x0000306E
		public InputActionMap()
		{
			InputActionMap.s_NeedToResolveBindings = true;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00004E83 File Offset: 0x00003083
		public InputActionMap(string name)
			: this()
		{
			this.m_Name = name;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00004E92 File Offset: 0x00003092
		public void Dispose()
		{
			InputActionState state = this.m_State;
			if (state == null)
			{
				return;
			}
			state.Dispose();
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00004EA4 File Offset: 0x000030A4
		internal int FindActionIndex(string nameOrId)
		{
			if (string.IsNullOrEmpty(nameOrId))
			{
				return -1;
			}
			if (this.m_Actions == null)
			{
				return -1;
			}
			this.SetUpActionLookupTable();
			int num = this.m_Actions.Length;
			if (nameOrId.StartsWith("{") && nameOrId.EndsWith("}"))
			{
				int num2 = nameOrId.Length - 2;
				for (int i = 0; i < num; i++)
				{
					if (string.Compare(this.m_Actions[i].m_Id, 0, nameOrId, 1, num2) == 0)
					{
						return i;
					}
				}
			}
			int num3;
			if (this.m_ActionIndexByNameOrId.TryGetValue(nameOrId, out num3))
			{
				return num3;
			}
			for (int j = 0; j < num; j++)
			{
				if (this.m_Actions[j].m_Id == nameOrId || string.Compare(this.m_Actions[j].m_Name, nameOrId, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return j;
				}
			}
			return -1;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00004F74 File Offset: 0x00003174
		private void SetUpActionLookupTable()
		{
			if (this.m_ActionIndexByNameOrId != null || this.m_Actions == null)
			{
				return;
			}
			this.m_ActionIndexByNameOrId = new Dictionary<string, int>();
			int num = this.m_Actions.Length;
			for (int i = 0; i < num; i++)
			{
				InputAction inputAction = this.m_Actions[i];
				inputAction.MakeSureIdIsInPlace();
				this.m_ActionIndexByNameOrId[inputAction.name] = i;
				this.m_ActionIndexByNameOrId[inputAction.m_Id] = i;
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00004FE6 File Offset: 0x000031E6
		internal void ClearActionLookupTable()
		{
			Dictionary<string, int> actionIndexByNameOrId = this.m_ActionIndexByNameOrId;
			if (actionIndexByNameOrId == null)
			{
				return;
			}
			actionIndexByNameOrId.Clear();
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00004FF8 File Offset: 0x000031F8
		private int FindActionIndex(Guid id)
		{
			if (this.m_Actions == null)
			{
				return -1;
			}
			int num = this.m_Actions.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.m_Actions[i].idDontGenerate == id)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000503C File Offset: 0x0000323C
		public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
		{
			if (actionNameOrId == null)
			{
				throw new ArgumentNullException("actionNameOrId");
			}
			int num = this.FindActionIndex(actionNameOrId);
			if (num != -1)
			{
				return this.m_Actions[num];
			}
			if (throwIfNotFound)
			{
				throw new ArgumentException(string.Format("No action '{0}' in '{1}'", actionNameOrId, this), "actionNameOrId");
			}
			return null;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00005088 File Offset: 0x00003288
		public InputAction FindAction(Guid id)
		{
			int num = this.FindActionIndex(id);
			if (num == -1)
			{
				return null;
			}
			return this.m_Actions[num];
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000050AC File Offset: 0x000032AC
		public bool IsUsableWithDevice(InputDevice device)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			if (this.m_Bindings == null)
			{
				return false;
			}
			foreach (InputBinding inputBinding in this.m_Bindings)
			{
				string effectivePath = inputBinding.effectivePath;
				if (!string.IsNullOrEmpty(effectivePath) && InputControlPath.Matches(effectivePath, device))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000510A File Offset: 0x0000330A
		public void Enable()
		{
			if (this.m_Actions == null || this.m_EnabledActionsCount == this.m_Actions.Length)
			{
				return;
			}
			this.ResolveBindingsIfNecessary();
			this.m_State.EnableAllActions(this);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00005138 File Offset: 0x00003338
		public void Disable()
		{
			if (!this.enabled)
			{
				return;
			}
			this.m_State.DisableAllActions(this);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00005150 File Offset: 0x00003350
		public InputActionMap Clone()
		{
			InputActionMap inputActionMap = new InputActionMap
			{
				m_Name = this.m_Name
			};
			if (this.m_Actions != null)
			{
				int num = this.m_Actions.Length;
				InputAction[] array = new InputAction[num];
				for (int i = 0; i < num; i++)
				{
					InputAction inputAction = this.m_Actions[i];
					array[i] = new InputAction
					{
						m_Name = inputAction.m_Name,
						m_ActionMap = inputActionMap,
						m_Type = inputAction.m_Type,
						m_Interactions = inputAction.m_Interactions,
						m_Processors = inputAction.m_Processors,
						m_ExpectedControlType = inputAction.m_ExpectedControlType,
						m_Flags = inputAction.m_Flags
					};
				}
				inputActionMap.m_Actions = array;
			}
			if (this.m_Bindings != null)
			{
				int num2 = this.m_Bindings.Length;
				InputBinding[] array2 = new InputBinding[num2];
				Array.Copy(this.m_Bindings, 0, array2, 0, num2);
				for (int j = 0; j < num2; j++)
				{
					array2[j].m_Id = null;
				}
				inputActionMap.m_Bindings = array2;
			}
			return inputActionMap;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00005259 File Offset: 0x00003459
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00005261 File Offset: 0x00003461
		public bool Contains(InputAction action)
		{
			return action != null && action.actionMap == this;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00005271 File Offset: 0x00003471
		public override string ToString()
		{
			if (this.m_Asset != null)
			{
				return string.Format("{0}:{1}", this.m_Asset, this.m_Name);
			}
			if (!string.IsNullOrEmpty(this.m_Name))
			{
				return this.m_Name;
			}
			return "<Unnamed Action Map>";
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000052B4 File Offset: 0x000034B4
		public IEnumerator<InputAction> GetEnumerator()
		{
			return this.actions.GetEnumerator();
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000052D4 File Offset: 0x000034D4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x000052DC File Offset: 0x000034DC
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x000052E9 File Offset: 0x000034E9
		private bool needToResolveBindings
		{
			get
			{
				return (this.m_Flags & InputActionMap.Flags.NeedToResolveBindings) > (InputActionMap.Flags)0;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= InputActionMap.Flags.NeedToResolveBindings;
					return;
				}
				this.m_Flags &= ~InputActionMap.Flags.NeedToResolveBindings;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000530C File Offset: 0x0000350C
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00005319 File Offset: 0x00003519
		private bool bindingResolutionNeedsFullReResolve
		{
			get
			{
				return (this.m_Flags & InputActionMap.Flags.BindingResolutionNeedsFullReResolve) > (InputActionMap.Flags)0;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= InputActionMap.Flags.BindingResolutionNeedsFullReResolve;
					return;
				}
				this.m_Flags &= ~InputActionMap.Flags.BindingResolutionNeedsFullReResolve;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000533C File Offset: 0x0000353C
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00005349 File Offset: 0x00003549
		private bool controlsForEachActionInitialized
		{
			get
			{
				return (this.m_Flags & InputActionMap.Flags.ControlsForEachActionInitialized) > (InputActionMap.Flags)0;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= InputActionMap.Flags.ControlsForEachActionInitialized;
					return;
				}
				this.m_Flags &= ~InputActionMap.Flags.ControlsForEachActionInitialized;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000536C File Offset: 0x0000356C
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00005379 File Offset: 0x00003579
		private bool bindingsForEachActionInitialized
		{
			get
			{
				return (this.m_Flags & InputActionMap.Flags.BindingsForEachActionInitialized) > (InputActionMap.Flags)0;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= InputActionMap.Flags.BindingsForEachActionInitialized;
					return;
				}
				this.m_Flags &= ~InputActionMap.Flags.BindingsForEachActionInitialized;
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000539C File Offset: 0x0000359C
		internal ReadOnlyArray<InputBinding> GetBindingsForSingleAction(InputAction action)
		{
			if (!this.bindingsForEachActionInitialized)
			{
				this.SetUpPerActionControlAndBindingArrays();
			}
			return new ReadOnlyArray<InputBinding>(this.m_BindingsForEachAction, action.m_BindingsStartIndex, action.m_BindingsCount);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000053C3 File Offset: 0x000035C3
		internal ReadOnlyArray<InputControl> GetControlsForSingleAction(InputAction action)
		{
			if (!this.controlsForEachActionInitialized)
			{
				this.SetUpPerActionControlAndBindingArrays();
			}
			return new ReadOnlyArray<InputControl>(this.m_ControlsForEachAction, action.m_ControlStartIndex, action.m_ControlCount);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000053EC File Offset: 0x000035EC
		private unsafe void SetUpPerActionControlAndBindingArrays()
		{
			if (this.m_Bindings == null)
			{
				this.m_ControlsForEachAction = null;
				this.m_BindingsForEachAction = null;
				this.controlsForEachActionInitialized = true;
				this.bindingsForEachActionInitialized = true;
				return;
			}
			if (this.m_SingletonAction != null)
			{
				this.m_BindingsForEachAction = this.m_Bindings;
				InputActionState state = this.m_State;
				this.m_ControlsForEachAction = ((state != null) ? state.controls : null);
				this.m_SingletonAction.m_BindingsStartIndex = 0;
				this.m_SingletonAction.m_BindingsCount = this.m_Bindings.Length;
				this.m_SingletonAction.m_ControlStartIndex = 0;
				InputAction singletonAction = this.m_SingletonAction;
				InputActionState state2 = this.m_State;
				singletonAction.m_ControlCount = ((state2 != null) ? state2.totalControlCount : 0);
				if (this.m_ControlsForEachAction.HaveDuplicateReferences<InputControl>(0, this.m_SingletonAction.m_ControlCount))
				{
					int num = 0;
					InputControl[] array = new InputControl[this.m_SingletonAction.m_ControlCount];
					for (int i = 0; i < this.m_SingletonAction.m_ControlCount; i++)
					{
						if (!array.ContainsReference(this.m_ControlsForEachAction[i]))
						{
							array[num] = this.m_ControlsForEachAction[i];
							num++;
						}
					}
					this.m_ControlsForEachAction = array;
					this.m_SingletonAction.m_ControlCount = num;
				}
			}
			else
			{
				InputActionState state3 = this.m_State;
				InputActionState.ActionMapIndices actionMapIndices = ((state3 != null) ? state3.FetchMapIndices(this) : default(InputActionState.ActionMapIndices));
				for (int j = 0; j < this.m_Actions.Length; j++)
				{
					InputAction inputAction = this.m_Actions[j];
					inputAction.m_BindingsCount = 0;
					inputAction.m_BindingsStartIndex = -1;
					inputAction.m_ControlCount = 0;
					inputAction.m_ControlStartIndex = -1;
				}
				int num2 = this.m_Bindings.Length;
				for (int k = 0; k < num2; k++)
				{
					InputAction inputAction2 = this.FindAction(this.m_Bindings[k].action, false);
					if (inputAction2 != null)
					{
						inputAction2.m_BindingsCount++;
					}
				}
				int num3 = 0;
				if (this.m_State != null && (this.m_ControlsForEachAction == null || this.m_ControlsForEachAction.Length != actionMapIndices.controlCount))
				{
					if (actionMapIndices.controlCount == 0)
					{
						this.m_ControlsForEachAction = null;
					}
					else
					{
						this.m_ControlsForEachAction = new InputControl[actionMapIndices.controlCount];
					}
				}
				InputBinding[] array2 = null;
				int num4 = 0;
				int l = 0;
				while (l < this.m_Bindings.Length)
				{
					InputAction inputAction3 = this.FindAction(this.m_Bindings[l].action, false);
					if (inputAction3 == null || inputAction3.m_BindingsStartIndex != -1)
					{
						l++;
					}
					else
					{
						inputAction3.m_BindingsStartIndex = ((array2 != null) ? num3 : l);
						inputAction3.m_ControlStartIndex = num4;
						int bindingsCount = inputAction3.m_BindingsCount;
						int num5 = l;
						for (int m = 0; m < bindingsCount; m++)
						{
							if (this.FindAction(this.m_Bindings[num5].action, false) != inputAction3)
							{
								if (array2 == null)
								{
									array2 = new InputBinding[this.m_Bindings.Length];
									num3 = num5;
									Array.Copy(this.m_Bindings, 0, array2, 0, num5);
								}
								do
								{
									num5++;
								}
								while (this.FindAction(this.m_Bindings[num5].action, false) != inputAction3);
							}
							else if (l == num5)
							{
								l++;
							}
							if (array2 != null)
							{
								array2[num3++] = this.m_Bindings[num5];
							}
							if (this.m_State != null && !this.m_Bindings[num5].isComposite)
							{
								ref InputActionState.BindingState ptr = ref this.m_State.bindingStates[actionMapIndices.bindingStartIndex + num5];
								int controlCount = ptr.controlCount;
								if (controlCount > 0)
								{
									int controlStartIndex = ptr.controlStartIndex;
									for (int n = 0; n < controlCount; n++)
									{
										InputControl inputControl = this.m_State.controls[controlStartIndex + n];
										if (!this.m_ControlsForEachAction.ContainsReference(inputAction3.m_ControlStartIndex, inputAction3.m_ControlCount, inputControl))
										{
											this.m_ControlsForEachAction[num4] = inputControl;
											num4++;
											inputAction3.m_ControlCount++;
										}
									}
								}
							}
							num5++;
						}
					}
				}
				if (array2 == null)
				{
					this.m_BindingsForEachAction = this.m_Bindings;
				}
				else
				{
					this.m_BindingsForEachAction = array2;
				}
			}
			this.controlsForEachActionInitialized = true;
			this.bindingsForEachActionInitialized = true;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00005804 File Offset: 0x00003A04
		internal void OnWantToChangeSetup()
		{
			if (this.asset != null)
			{
				using (ReadOnlyArray<InputActionMap>.Enumerator enumerator = this.asset.actionMaps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.enabled)
						{
							throw new InvalidOperationException(string.Format("Cannot add, remove, or change elements of InputActionAsset {0} while one or more of its actions are enabled", this.asset));
						}
					}
					return;
				}
			}
			if (this.enabled)
			{
				throw new InvalidOperationException(string.Format("Cannot add, remove, or change elements of InputActionMap {0} while one or more of its actions are enabled", this));
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000589C File Offset: 0x00003A9C
		internal void OnSetupChanged()
		{
			if (this.m_Asset != null)
			{
				this.m_Asset.MarkAsDirty();
				using (ReadOnlyArray<InputActionMap>.Enumerator enumerator = this.m_Asset.actionMaps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						InputActionMap inputActionMap = enumerator.Current;
						inputActionMap.m_State = null;
					}
					goto IL_005C;
				}
			}
			this.m_State = null;
			IL_005C:
			this.ClearCachedActionData(false);
			this.LazyResolveBindings(true);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00005924 File Offset: 0x00003B24
		internal void OnBindingModified()
		{
			this.ClearCachedActionData(false);
			this.LazyResolveBindings(true);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00005935 File Offset: 0x00003B35
		internal void ClearCachedActionData(bool onlyControls = false)
		{
			if (!onlyControls)
			{
				this.bindingsForEachActionInitialized = false;
				this.m_BindingsForEachAction = null;
				this.m_ActionIndexByNameOrId = null;
			}
			this.controlsForEachActionInitialized = false;
			this.m_ControlsForEachAction = null;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00005960 File Offset: 0x00003B60
		internal void GenerateId()
		{
			this.m_Id = Guid.NewGuid().ToString();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00005988 File Offset: 0x00003B88
		internal bool LazyResolveBindings(bool fullResolve)
		{
			this.m_ControlsForEachAction = null;
			this.controlsForEachActionInitialized = false;
			InputActionMap.s_NeedToResolveBindings = true;
			if (this.m_State == null)
			{
				return false;
			}
			this.needToResolveBindings = true;
			this.bindingResolutionNeedsFullReResolve = this.bindingResolutionNeedsFullReResolve || fullResolve;
			if (InputActionMap.s_DeferBindingResolution > 0)
			{
				return false;
			}
			this.ResolveBindings();
			return true;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000059D9 File Offset: 0x00003BD9
		internal bool ResolveBindingsIfNecessary()
		{
			if (this.m_State != null && !this.needToResolveBindings)
			{
				return false;
			}
			if (this.m_State != null && this.m_State.isProcessingControlStateChange)
			{
				return false;
			}
			this.ResolveBindings();
			return true;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00005A0C File Offset: 0x00003C0C
		internal void ResolveBindings()
		{
			using (InputActionMap.k_ResolveBindingsProfilerMarker.Auto())
			{
				using (InputActionRebindingExtensions.DeferBindingResolution())
				{
					InputActionState.UnmanagedMemory unmanagedMemory = default(InputActionState.UnmanagedMemory);
					try
					{
						InputBindingResolver inputBindingResolver = default(InputBindingResolver);
						bool flag = this.m_State == null;
						OneOrMore<InputActionMap, ReadOnlyArray<InputActionMap>> oneOrMore;
						if (this.m_Asset != null)
						{
							oneOrMore = this.m_Asset.actionMaps;
							inputBindingResolver.bindingMask = this.m_Asset.m_BindingMask;
							using (IEnumerator<InputActionMap> enumerator = oneOrMore.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									InputActionMap inputActionMap = enumerator.Current;
									flag |= inputActionMap.bindingResolutionNeedsFullReResolve;
									inputActionMap.needToResolveBindings = false;
									inputActionMap.bindingResolutionNeedsFullReResolve = false;
									inputActionMap.controlsForEachActionInitialized = false;
								}
								goto IL_00D8;
							}
						}
						oneOrMore = this;
						flag |= this.bindingResolutionNeedsFullReResolve;
						this.needToResolveBindings = false;
						this.bindingResolutionNeedsFullReResolve = false;
						this.controlsForEachActionInitialized = false;
						IL_00D8:
						bool flag2 = false;
						InputControlList<InputControl> inputControlList = default(InputControlList<InputControl>);
						if (this.m_State != null)
						{
							unmanagedMemory = this.m_State.memory.Clone();
							this.m_State.PrepareForBindingReResolution(flag, ref inputControlList, ref flag2);
							inputBindingResolver.StartWithPreviousResolve(this.m_State, flag);
							this.m_State.memory.Dispose();
						}
						foreach (InputActionMap inputActionMap2 in oneOrMore)
						{
							inputBindingResolver.AddActionMap(inputActionMap2);
						}
						if (this.m_State == null)
						{
							this.m_State = new InputActionState();
							this.m_State.Initialize(inputBindingResolver);
						}
						else
						{
							this.m_State.ClaimDataFrom(inputBindingResolver);
						}
						if (this.m_Asset != null)
						{
							foreach (InputActionMap inputActionMap3 in oneOrMore)
							{
								inputActionMap3.m_State = this.m_State;
							}
							this.m_Asset.m_SharedStateForAllMaps = this.m_State;
						}
						this.m_State.FinishBindingResolution(flag2, unmanagedMemory, inputControlList, flag);
					}
					finally
					{
						unmanagedMemory.Dispose();
					}
				}
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00005CC4 File Offset: 0x00003EC4
		public int FindBinding(InputBinding mask, out InputAction action)
		{
			int num = this.FindBindingRelativeToMap(mask);
			if (num == -1)
			{
				action = null;
				return -1;
			}
			action = this.m_SingletonAction ?? this.FindAction(this.bindings[num].action, false);
			return action.BindingIndexOnMapToBindingIndexOnAction(num);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00005D14 File Offset: 0x00003F14
		internal int FindBindingRelativeToMap(InputBinding mask)
		{
			InputBinding[] bindings = this.m_Bindings;
			int num = bindings.LengthSafe<InputBinding>();
			for (int i = 0; i < num; i++)
			{
				ref InputBinding ptr = ref bindings[i];
				if (mask.Matches(ref ptr, (InputBinding.MatchOptions)0))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00005D54 File Offset: 0x00003F54
		public static InputActionMap[] FromJson(string json)
		{
			if (json == null)
			{
				throw new ArgumentNullException("json");
			}
			return JsonUtility.FromJson<InputActionMap.ReadFileJson>(json).ToMaps();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00005D7D File Offset: 0x00003F7D
		public static string ToJson(IEnumerable<InputActionMap> maps)
		{
			if (maps == null)
			{
				throw new ArgumentNullException("maps");
			}
			return JsonUtility.ToJson(InputActionMap.WriteFileJson.FromMaps(maps), true);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00005D9E File Offset: 0x00003F9E
		public string ToJson()
		{
			return JsonUtility.ToJson(InputActionMap.WriteFileJson.FromMap(this), true);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00005DB1 File Offset: 0x00003FB1
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00005DB4 File Offset: 0x00003FB4
		public void OnAfterDeserialize()
		{
			InputActionMap.s_NeedToResolveBindings = true;
			this.m_State = null;
			this.m_MapIndexInState = -1;
			this.m_EnabledActionsCount = 0;
			if (this.m_Actions != null)
			{
				int num = this.m_Actions.Length;
				for (int i = 0; i < num; i++)
				{
					this.m_Actions[i].m_ActionMap = this;
				}
			}
			this.ClearCachedActionData(false);
			this.ClearActionLookupTable();
		}

		// Token: 0x040000A8 RID: 168
		private static readonly ProfilerMarker k_ResolveBindingsProfilerMarker = new ProfilerMarker("InputActionMap.ResolveBindings");

		// Token: 0x040000A9 RID: 169
		[SerializeField]
		internal string m_Name;

		// Token: 0x040000AA RID: 170
		[SerializeField]
		internal string m_Id;

		// Token: 0x040000AB RID: 171
		[SerializeField]
		internal InputActionAsset m_Asset;

		// Token: 0x040000AC RID: 172
		[SerializeField]
		internal InputAction[] m_Actions;

		// Token: 0x040000AD RID: 173
		[SerializeField]
		internal InputBinding[] m_Bindings;

		// Token: 0x040000AE RID: 174
		[NonSerialized]
		private InputBinding[] m_BindingsForEachAction;

		// Token: 0x040000AF RID: 175
		[NonSerialized]
		private InputControl[] m_ControlsForEachAction;

		// Token: 0x040000B0 RID: 176
		[NonSerialized]
		internal int m_EnabledActionsCount;

		// Token: 0x040000B1 RID: 177
		[NonSerialized]
		internal InputAction m_SingletonAction;

		// Token: 0x040000B2 RID: 178
		[NonSerialized]
		internal int m_MapIndexInState = -1;

		// Token: 0x040000B3 RID: 179
		[NonSerialized]
		internal InputActionState m_State;

		// Token: 0x040000B4 RID: 180
		[NonSerialized]
		internal InputBinding? m_BindingMask;

		// Token: 0x040000B5 RID: 181
		[NonSerialized]
		private InputActionMap.Flags m_Flags;

		// Token: 0x040000B6 RID: 182
		[NonSerialized]
		internal int m_ParameterOverridesCount;

		// Token: 0x040000B7 RID: 183
		[NonSerialized]
		internal InputActionRebindingExtensions.ParameterOverride[] m_ParameterOverrides;

		// Token: 0x040000B8 RID: 184
		[NonSerialized]
		internal InputActionMap.DeviceArray m_Devices;

		// Token: 0x040000B9 RID: 185
		[NonSerialized]
		internal CallbackArray<Action<InputAction.CallbackContext>> m_ActionCallbacks;

		// Token: 0x040000BA RID: 186
		[NonSerialized]
		internal Dictionary<string, int> m_ActionIndexByNameOrId;

		// Token: 0x040000BB RID: 187
		internal static int s_DeferBindingResolution;

		// Token: 0x040000BC RID: 188
		internal static bool s_NeedToResolveBindings;

		// Token: 0x02000161 RID: 353
		[Flags]
		private enum Flags
		{
			// Token: 0x040007A3 RID: 1955
			NeedToResolveBindings = 1,
			// Token: 0x040007A4 RID: 1956
			BindingResolutionNeedsFullReResolve = 2,
			// Token: 0x040007A5 RID: 1957
			ControlsForEachActionInitialized = 4,
			// Token: 0x040007A6 RID: 1958
			BindingsForEachActionInitialized = 8
		}

		// Token: 0x02000162 RID: 354
		internal struct DeviceArray
		{
			// Token: 0x06001299 RID: 4761 RVA: 0x00057BAF File Offset: 0x00055DAF
			public int IndexOf(InputDevice device)
			{
				return this.m_DeviceArray.IndexOfReference(device, this.m_DeviceCount);
			}

			// Token: 0x0600129A RID: 4762 RVA: 0x00057BC4 File Offset: 0x00055DC4
			public bool Remove(InputDevice device)
			{
				int num = this.IndexOf(device);
				if (num < 0)
				{
					return false;
				}
				this.m_DeviceArray.EraseAtWithCapacity<InputDevice>(ref this.m_DeviceCount, num);
				return true;
			}

			// Token: 0x0600129B RID: 4763 RVA: 0x00057BF4 File Offset: 0x00055DF4
			public ReadOnlyArray<InputDevice>? Get()
			{
				if (!this.m_HaveValue)
				{
					return null;
				}
				return new ReadOnlyArray<InputDevice>?(new ReadOnlyArray<InputDevice>(this.m_DeviceArray, 0, this.m_DeviceCount));
			}

			// Token: 0x0600129C RID: 4764 RVA: 0x00057C2C File Offset: 0x00055E2C
			public bool Set(ReadOnlyArray<InputDevice>? devices)
			{
				if (devices == null)
				{
					if (!this.m_HaveValue)
					{
						return false;
					}
					if (this.m_DeviceCount > 0)
					{
						Array.Clear(this.m_DeviceArray, 0, this.m_DeviceCount);
					}
					this.m_DeviceCount = 0;
					this.m_HaveValue = false;
				}
				else
				{
					ReadOnlyArray<InputDevice> value = devices.Value;
					if (this.m_HaveValue && value.Count == this.m_DeviceCount && value.HaveEqualReferences<InputDevice>(this.m_DeviceArray, this.m_DeviceCount))
					{
						return false;
					}
					if (this.m_DeviceCount > 0)
					{
						this.m_DeviceArray.Clear<InputDevice>(ref this.m_DeviceCount);
					}
					this.m_HaveValue = true;
					this.m_DeviceCount = 0;
					ArrayHelpers.AppendListWithCapacity<InputDevice, ReadOnlyArray<InputDevice>>(ref this.m_DeviceArray, ref this.m_DeviceCount, value, 10);
				}
				return true;
			}

			// Token: 0x040007A7 RID: 1959
			private bool m_HaveValue;

			// Token: 0x040007A8 RID: 1960
			private int m_DeviceCount;

			// Token: 0x040007A9 RID: 1961
			private InputDevice[] m_DeviceArray;
		}

		// Token: 0x02000163 RID: 355
		[Serializable]
		internal struct BindingOverrideListJson
		{
			// Token: 0x040007AA RID: 1962
			public List<InputActionMap.BindingOverrideJson> bindings;
		}

		// Token: 0x02000164 RID: 356
		[Serializable]
		internal struct BindingOverrideJson
		{
			// Token: 0x0600129D RID: 4765 RVA: 0x00057CEC File Offset: 0x00055EEC
			public static InputActionMap.BindingOverrideJson FromBinding(InputBinding binding, string actionName)
			{
				return new InputActionMap.BindingOverrideJson
				{
					action = actionName,
					id = binding.id.ToString(),
					path = (binding.overridePath ?? "null"),
					interactions = (binding.overrideInteractions ?? "null"),
					processors = (binding.overrideProcessors ?? "null")
				};
			}

			// Token: 0x0600129E RID: 4766 RVA: 0x00057D6B File Offset: 0x00055F6B
			public static InputActionMap.BindingOverrideJson FromBinding(InputBinding binding)
			{
				return InputActionMap.BindingOverrideJson.FromBinding(binding, binding.action);
			}

			// Token: 0x0600129F RID: 4767 RVA: 0x00057D7C File Offset: 0x00055F7C
			public static InputBinding ToBinding(InputActionMap.BindingOverrideJson bindingOverride)
			{
				return new InputBinding
				{
					overridePath = ((bindingOverride.path != "null") ? bindingOverride.path : null),
					overrideInteractions = ((bindingOverride.interactions != "null") ? bindingOverride.interactions : null),
					overrideProcessors = ((bindingOverride.processors != "null") ? bindingOverride.processors : null)
				};
			}

			// Token: 0x040007AB RID: 1963
			public string action;

			// Token: 0x040007AC RID: 1964
			public string id;

			// Token: 0x040007AD RID: 1965
			public string path;

			// Token: 0x040007AE RID: 1966
			public string interactions;

			// Token: 0x040007AF RID: 1967
			public string processors;
		}

		// Token: 0x02000165 RID: 357
		[Serializable]
		internal struct BindingJson
		{
			// Token: 0x060012A0 RID: 4768 RVA: 0x00057DF8 File Offset: 0x00055FF8
			public InputBinding ToBinding()
			{
				return new InputBinding
				{
					name = (string.IsNullOrEmpty(this.name) ? null : this.name),
					m_Id = (string.IsNullOrEmpty(this.id) ? null : this.id),
					path = this.path,
					action = (string.IsNullOrEmpty(this.action) ? null : this.action),
					interactions = (string.IsNullOrEmpty(this.interactions) ? null : this.interactions),
					processors = (string.IsNullOrEmpty(this.processors) ? null : this.processors),
					groups = (string.IsNullOrEmpty(this.groups) ? null : this.groups),
					isComposite = this.isComposite,
					isPartOfComposite = this.isPartOfComposite
				};
			}

			// Token: 0x060012A1 RID: 4769 RVA: 0x00057EE4 File Offset: 0x000560E4
			public static InputActionMap.BindingJson FromBinding(ref InputBinding binding)
			{
				return new InputActionMap.BindingJson
				{
					name = binding.name,
					id = binding.m_Id,
					path = binding.path,
					action = binding.action,
					interactions = binding.interactions,
					processors = binding.processors,
					groups = binding.groups,
					isComposite = binding.isComposite,
					isPartOfComposite = binding.isPartOfComposite
				};
			}

			// Token: 0x040007B0 RID: 1968
			public string name;

			// Token: 0x040007B1 RID: 1969
			public string id;

			// Token: 0x040007B2 RID: 1970
			public string path;

			// Token: 0x040007B3 RID: 1971
			public string interactions;

			// Token: 0x040007B4 RID: 1972
			public string processors;

			// Token: 0x040007B5 RID: 1973
			public string groups;

			// Token: 0x040007B6 RID: 1974
			public string action;

			// Token: 0x040007B7 RID: 1975
			public bool isComposite;

			// Token: 0x040007B8 RID: 1976
			public bool isPartOfComposite;
		}

		// Token: 0x02000166 RID: 358
		[Serializable]
		internal struct ReadActionJson
		{
			// Token: 0x060012A2 RID: 4770 RVA: 0x00057F70 File Offset: 0x00056170
			public InputAction ToAction(string actionName = null)
			{
				if (!string.IsNullOrEmpty(this.expectedControlLayout))
				{
					this.expectedControlType = this.expectedControlLayout;
				}
				InputActionType inputActionType = InputActionType.Value;
				if (!string.IsNullOrEmpty(this.type))
				{
					inputActionType = (InputActionType)Enum.Parse(typeof(InputActionType), this.type, true);
				}
				else if (this.passThrough)
				{
					inputActionType = InputActionType.PassThrough;
				}
				else if (this.initialStateCheck)
				{
					inputActionType = InputActionType.Value;
				}
				else if (!string.IsNullOrEmpty(this.expectedControlType) && (this.expectedControlType == "Button" || this.expectedControlType == "Key"))
				{
					inputActionType = InputActionType.Button;
				}
				return new InputAction(actionName ?? this.name, inputActionType, null, null, null, null)
				{
					m_Id = (string.IsNullOrEmpty(this.id) ? null : this.id),
					m_ExpectedControlType = ((!string.IsNullOrEmpty(this.expectedControlType)) ? this.expectedControlType : null),
					m_Processors = this.processors,
					m_Interactions = this.interactions,
					wantsInitialStateCheck = this.initialStateCheck
				};
			}

			// Token: 0x040007B9 RID: 1977
			public string name;

			// Token: 0x040007BA RID: 1978
			public string type;

			// Token: 0x040007BB RID: 1979
			public string id;

			// Token: 0x040007BC RID: 1980
			public string expectedControlType;

			// Token: 0x040007BD RID: 1981
			public string expectedControlLayout;

			// Token: 0x040007BE RID: 1982
			public string processors;

			// Token: 0x040007BF RID: 1983
			public string interactions;

			// Token: 0x040007C0 RID: 1984
			public bool passThrough;

			// Token: 0x040007C1 RID: 1985
			public bool initialStateCheck;

			// Token: 0x040007C2 RID: 1986
			public InputActionMap.BindingJson[] bindings;
		}

		// Token: 0x02000167 RID: 359
		[Serializable]
		internal struct WriteActionJson
		{
			// Token: 0x060012A3 RID: 4771 RVA: 0x00058080 File Offset: 0x00056280
			public static InputActionMap.WriteActionJson FromAction(InputAction action)
			{
				return new InputActionMap.WriteActionJson
				{
					name = action.m_Name,
					type = action.m_Type.ToString(),
					id = action.m_Id,
					expectedControlType = action.m_ExpectedControlType,
					processors = action.processors,
					interactions = action.interactions,
					initialStateCheck = action.wantsInitialStateCheck
				};
			}

			// Token: 0x040007C3 RID: 1987
			public string name;

			// Token: 0x040007C4 RID: 1988
			public string type;

			// Token: 0x040007C5 RID: 1989
			public string id;

			// Token: 0x040007C6 RID: 1990
			public string expectedControlType;

			// Token: 0x040007C7 RID: 1991
			public string processors;

			// Token: 0x040007C8 RID: 1992
			public string interactions;

			// Token: 0x040007C9 RID: 1993
			public bool initialStateCheck;
		}

		// Token: 0x02000168 RID: 360
		[Serializable]
		internal struct ReadMapJson
		{
			// Token: 0x040007CA RID: 1994
			public string name;

			// Token: 0x040007CB RID: 1995
			public string id;

			// Token: 0x040007CC RID: 1996
			public InputActionMap.ReadActionJson[] actions;

			// Token: 0x040007CD RID: 1997
			public InputActionMap.BindingJson[] bindings;
		}

		// Token: 0x02000169 RID: 361
		[Serializable]
		internal struct WriteMapJson
		{
			// Token: 0x060012A4 RID: 4772 RVA: 0x000580FC File Offset: 0x000562FC
			public static InputActionMap.WriteMapJson FromMap(InputActionMap map)
			{
				InputActionMap.WriteActionJson[] array = null;
				InputActionMap.BindingJson[] array2 = null;
				InputAction[] array3 = map.m_Actions;
				if (array3 != null)
				{
					int num = array3.Length;
					array = new InputActionMap.WriteActionJson[num];
					for (int i = 0; i < num; i++)
					{
						array[i] = InputActionMap.WriteActionJson.FromAction(array3[i]);
					}
				}
				InputBinding[] array4 = map.m_Bindings;
				if (array4 != null)
				{
					int num2 = array4.Length;
					array2 = new InputActionMap.BindingJson[num2];
					for (int j = 0; j < num2; j++)
					{
						array2[j] = InputActionMap.BindingJson.FromBinding(ref array4[j]);
					}
				}
				return new InputActionMap.WriteMapJson
				{
					name = map.name,
					id = map.id.ToString(),
					actions = array,
					bindings = array2
				};
			}

			// Token: 0x040007CE RID: 1998
			public string name;

			// Token: 0x040007CF RID: 1999
			public string id;

			// Token: 0x040007D0 RID: 2000
			public InputActionMap.WriteActionJson[] actions;

			// Token: 0x040007D1 RID: 2001
			public InputActionMap.BindingJson[] bindings;
		}

		// Token: 0x0200016A RID: 362
		[Serializable]
		internal struct WriteFileJson
		{
			// Token: 0x060012A5 RID: 4773 RVA: 0x000581C8 File Offset: 0x000563C8
			public static InputActionMap.WriteFileJson FromMap(InputActionMap map)
			{
				return new InputActionMap.WriteFileJson
				{
					maps = new InputActionMap.WriteMapJson[] { InputActionMap.WriteMapJson.FromMap(map) }
				};
			}

			// Token: 0x060012A6 RID: 4774 RVA: 0x000581F8 File Offset: 0x000563F8
			public static InputActionMap.WriteFileJson FromMaps(IEnumerable<InputActionMap> maps)
			{
				int num = maps.Count<InputActionMap>();
				if (num == 0)
				{
					return default(InputActionMap.WriteFileJson);
				}
				InputActionMap.WriteMapJson[] array = new InputActionMap.WriteMapJson[num];
				int num2 = 0;
				foreach (InputActionMap inputActionMap in maps)
				{
					array[num2++] = InputActionMap.WriteMapJson.FromMap(inputActionMap);
				}
				return new InputActionMap.WriteFileJson
				{
					maps = array
				};
			}

			// Token: 0x040007D2 RID: 2002
			public InputActionMap.WriteMapJson[] maps;
		}

		// Token: 0x0200016B RID: 363
		[Serializable]
		internal struct ReadFileJson
		{
			// Token: 0x060012A7 RID: 4775 RVA: 0x00058280 File Offset: 0x00056480
			public InputActionMap[] ToMaps()
			{
				List<InputActionMap> list = new List<InputActionMap>();
				List<List<InputAction>> list2 = new List<List<InputAction>>();
				List<List<InputBinding>> list3 = new List<List<InputBinding>>();
				InputActionMap.ReadActionJson[] array = this.actions;
				int num = ((array != null) ? array.Length : 0);
				for (int i = 0; i < num; i++)
				{
					InputActionMap.ReadActionJson readActionJson = this.actions[i];
					if (string.IsNullOrEmpty(readActionJson.name))
					{
						throw new InvalidOperationException(string.Format("Action number {0} has no name", i + 1));
					}
					string text = null;
					string text2 = readActionJson.name;
					int num2 = text2.IndexOf('/');
					if (num2 != -1)
					{
						text = text2.Substring(0, num2);
						text2 = text2.Substring(num2 + 1);
						if (string.IsNullOrEmpty(text2))
						{
							throw new InvalidOperationException("Invalid action name '" + readActionJson.name + "' (missing action name after '/')");
						}
					}
					InputActionMap inputActionMap = null;
					int j;
					for (j = 0; j < list.Count; j++)
					{
						if (string.Compare(list[j].name, text, StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							inputActionMap = list[j];
							break;
						}
					}
					if (inputActionMap == null)
					{
						inputActionMap = new InputActionMap(text);
						j = list.Count;
						list.Add(inputActionMap);
						list2.Add(new List<InputAction>());
						list3.Add(new List<InputBinding>());
					}
					InputAction inputAction = readActionJson.ToAction(text2);
					list2[j].Add(inputAction);
					if (readActionJson.bindings != null)
					{
						List<InputBinding> list4 = list3[j];
						for (int k = 0; k < readActionJson.bindings.Length; k++)
						{
							InputActionMap.BindingJson bindingJson = readActionJson.bindings[k];
							InputBinding inputBinding = bindingJson.ToBinding();
							inputBinding.action = inputAction.m_Name;
							list4.Add(inputBinding);
						}
					}
				}
				InputActionMap.ReadMapJson[] array2 = this.maps;
				int num3 = ((array2 != null) ? array2.Length : 0);
				for (int l = 0; l < num3; l++)
				{
					InputActionMap.ReadMapJson readMapJson = this.maps[l];
					string name = readMapJson.name;
					if (string.IsNullOrEmpty(name))
					{
						throw new InvalidOperationException(string.Format("Map number {0} has no name", l + 1));
					}
					InputActionMap inputActionMap2 = null;
					int m;
					for (m = 0; m < list.Count; m++)
					{
						if (string.Compare(list[m].name, name, StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							inputActionMap2 = list[m];
							break;
						}
					}
					if (inputActionMap2 == null)
					{
						inputActionMap2 = new InputActionMap(name)
						{
							m_Id = (string.IsNullOrEmpty(readMapJson.id) ? null : readMapJson.id)
						};
						m = list.Count;
						list.Add(inputActionMap2);
						list2.Add(new List<InputAction>());
						list3.Add(new List<InputBinding>());
					}
					InputActionMap.ReadActionJson[] array3 = readMapJson.actions;
					int num4 = ((array3 != null) ? array3.Length : 0);
					for (int n = 0; n < num4; n++)
					{
						InputActionMap.ReadActionJson readActionJson2 = readMapJson.actions[n];
						if (string.IsNullOrEmpty(readActionJson2.name))
						{
							throw new InvalidOperationException(string.Format("Action number {0} in map '{1}' has no name", l + 1, name));
						}
						InputAction inputAction2 = readActionJson2.ToAction(null);
						list2[m].Add(inputAction2);
						if (readActionJson2.bindings != null)
						{
							List<InputBinding> list5 = list3[m];
							for (int num5 = 0; num5 < readActionJson2.bindings.Length; num5++)
							{
								InputActionMap.BindingJson bindingJson2 = readActionJson2.bindings[num5];
								InputBinding inputBinding2 = bindingJson2.ToBinding();
								inputBinding2.action = inputAction2.m_Name;
								list5.Add(inputBinding2);
							}
						}
					}
					InputActionMap.BindingJson[] bindings = readMapJson.bindings;
					int num6 = ((bindings != null) ? bindings.Length : 0);
					List<InputBinding> list6 = list3[m];
					for (int num7 = 0; num7 < num6; num7++)
					{
						InputActionMap.BindingJson bindingJson3 = readMapJson.bindings[num7];
						InputBinding inputBinding3 = bindingJson3.ToBinding();
						list6.Add(inputBinding3);
					}
				}
				for (int num8 = 0; num8 < list.Count; num8++)
				{
					InputActionMap inputActionMap3 = list[num8];
					InputAction[] array4 = list2[num8].ToArray();
					InputBinding[] array5 = list3[num8].ToArray();
					inputActionMap3.m_Actions = array4;
					inputActionMap3.m_Bindings = array5;
					for (int num9 = 0; num9 < array4.Length; num9++)
					{
						array4[num9].m_ActionMap = inputActionMap3;
					}
				}
				return list.ToArray();
			}

			// Token: 0x040007D3 RID: 2003
			public InputActionMap.ReadActionJson[] actions;

			// Token: 0x040007D4 RID: 2004
			public InputActionMap.ReadMapJson[] maps;
		}
	}
}
