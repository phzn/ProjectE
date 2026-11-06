using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000023 RID: 35
	public static class InputActionSetupExtensions
	{
		// Token: 0x06000217 RID: 535 RVA: 0x00007940 File Offset: 0x00005B40
		public static InputActionMap AddActionMap(this InputActionAsset asset, string name)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			if (asset.FindActionMap(name, false) != null)
			{
				throw new InvalidOperationException("An action map called '" + name + "' already exists in the asset");
			}
			InputActionMap inputActionMap = new InputActionMap(name);
			inputActionMap.GenerateId();
			asset.AddActionMap(inputActionMap);
			return inputActionMap;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000079AC File Offset: 0x00005BAC
		public static void AddActionMap(this InputActionAsset asset, InputActionMap map)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (map == null)
			{
				throw new ArgumentNullException("map");
			}
			if (string.IsNullOrEmpty(map.name))
			{
				throw new InvalidOperationException("Maps added to an input action asset must be named");
			}
			if (map.asset != null)
			{
				throw new InvalidOperationException(string.Format("Cannot add map '{0}' to asset '{1}' as it has already been added to asset '{2}'", map, asset, map.asset));
			}
			if (asset.FindActionMap(map.name, false) != null)
			{
				throw new InvalidOperationException("An action map called '" + map.name + "' already exists in the asset");
			}
			map.OnWantToChangeSetup();
			asset.OnWantToChangeSetup();
			ArrayHelpers.Append<InputActionMap>(ref asset.m_ActionMaps, map);
			map.m_Asset = asset;
			asset.OnSetupChanged();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00007A6C File Offset: 0x00005C6C
		public static void RemoveActionMap(this InputActionAsset asset, InputActionMap map)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (map == null)
			{
				throw new ArgumentNullException("map");
			}
			map.OnWantToChangeSetup();
			asset.OnWantToChangeSetup();
			if (map.m_Asset != asset)
			{
				return;
			}
			ArrayHelpers.Erase<InputActionMap>(ref asset.m_ActionMaps, map);
			map.m_Asset = null;
			asset.OnSetupChanged();
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00007AD0 File Offset: 0x00005CD0
		public static void RemoveActionMap(this InputActionAsset asset, string nameOrId)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (nameOrId == null)
			{
				throw new ArgumentNullException("nameOrId");
			}
			InputActionMap inputActionMap = asset.FindActionMap(nameOrId, false);
			if (inputActionMap != null)
			{
				asset.RemoveActionMap(inputActionMap);
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00007B14 File Offset: 0x00005D14
		public static InputAction AddAction(this InputActionMap map, string name, InputActionType type = InputActionType.Value, string binding = null, string interactions = null, string processors = null, string groups = null, string expectedControlLayout = null)
		{
			if (map == null)
			{
				throw new ArgumentNullException("map");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Action must have name", "name");
			}
			map.OnWantToChangeSetup();
			if (map.FindAction(name, false) != null)
			{
				throw new InvalidOperationException(string.Concat(new string[] { "Cannot add action with duplicate name '", name, "' to set '", map.name, "'" }));
			}
			InputAction inputAction = new InputAction(name, type, null, null, null, null)
			{
				expectedControlType = expectedControlLayout
			};
			inputAction.GenerateId();
			ArrayHelpers.Append<InputAction>(ref map.m_Actions, inputAction);
			inputAction.m_ActionMap = map;
			if (!string.IsNullOrEmpty(binding))
			{
				inputAction.AddBinding(binding, interactions, processors, groups);
			}
			else
			{
				if (!string.IsNullOrEmpty(groups))
				{
					throw new ArgumentException(string.Format("No binding path was specified for action '{0}' but groups was specified ('{1}'); cannot apply groups without binding", inputAction, groups), "groups");
				}
				inputAction.m_Interactions = interactions;
				inputAction.m_Processors = processors;
				map.OnSetupChanged();
			}
			return inputAction;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00007C0C File Offset: 0x00005E0C
		public static void RemoveAction(this InputAction action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			InputActionMap actionMap = action.actionMap;
			if (actionMap == null)
			{
				throw new ArgumentException(string.Format("Action '{0}' does not belong to an action map; nowhere to remove from", action), "action");
			}
			actionMap.OnWantToChangeSetup();
			InputBinding[] array = action.bindings.ToArray();
			int num = actionMap.m_Actions.IndexOfReference(action, -1);
			ArrayHelpers.EraseAt<InputAction>(ref actionMap.m_Actions, num);
			action.m_ActionMap = null;
			action.m_SingletonActionBindings = array;
			int num2 = actionMap.m_Bindings.Length - array.Length;
			if (num2 == 0)
			{
				actionMap.m_Bindings = null;
			}
			else
			{
				InputBinding[] array2 = new InputBinding[num2];
				InputBinding[] bindings = actionMap.m_Bindings;
				int num3 = 0;
				for (int i = 0; i < bindings.Length; i++)
				{
					InputBinding binding = bindings[i];
					if (array.IndexOf<InputBinding>((InputBinding b) => b == binding) == -1)
					{
						array2[num3++] = binding;
					}
				}
				actionMap.m_Bindings = array2;
			}
			actionMap.OnSetupChanged();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00007D14 File Offset: 0x00005F14
		public static void RemoveAction(this InputActionAsset asset, string nameOrId)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (nameOrId == null)
			{
				throw new ArgumentNullException("nameOrId");
			}
			InputAction inputAction = asset.FindAction(nameOrId, false);
			if (inputAction == null)
			{
				return;
			}
			inputAction.RemoveAction();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00007D4C File Offset: 0x00005F4C
		public static InputActionSetupExtensions.BindingSyntax AddBinding(this InputAction action, string path, string interactions = null, string processors = null, string groups = null)
		{
			return action.AddBinding(new InputBinding
			{
				path = path,
				interactions = interactions,
				processors = processors,
				groups = groups
			});
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00007D89 File Offset: 0x00005F89
		public static InputActionSetupExtensions.BindingSyntax AddBinding(this InputAction action, InputControl control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			return action.AddBinding(control.path, null, null, null);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00007DA8 File Offset: 0x00005FA8
		public static InputActionSetupExtensions.BindingSyntax AddBinding(this InputAction action, InputBinding binding = default(InputBinding))
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			binding.action = action.name;
			InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
			int num = InputActionSetupExtensions.AddBindingInternal(orCreateActionMap, binding, -1);
			return new InputActionSetupExtensions.BindingSyntax(orCreateActionMap, num, null);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00007DE8 File Offset: 0x00005FE8
		public static InputActionSetupExtensions.BindingSyntax AddBinding(this InputActionMap actionMap, string path, string interactions = null, string groups = null, string action = null, string processors = null)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path", "Binding path cannot be null");
			}
			return actionMap.AddBinding(new InputBinding
			{
				path = path,
				interactions = interactions,
				groups = groups,
				action = action,
				processors = processors
			});
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00007E44 File Offset: 0x00006044
		public static InputActionSetupExtensions.BindingSyntax AddBinding(this InputActionMap actionMap, string path, InputAction action, string interactions = null, string groups = null)
		{
			if (action != null && action.actionMap != actionMap)
			{
				throw new ArgumentException(string.Format("Action '{0}' is not part of action map '{1}'", action, actionMap), "action");
			}
			if (action == null)
			{
				return actionMap.AddBinding(path, interactions, groups, null, null);
			}
			return actionMap.AddBinding(path, action.id, interactions, groups);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00007E98 File Offset: 0x00006098
		public static InputActionSetupExtensions.BindingSyntax AddBinding(this InputActionMap actionMap, string path, Guid action, string interactions = null, string groups = null)
		{
			if (action == Guid.Empty)
			{
				return actionMap.AddBinding(path, interactions, groups, null, null);
			}
			return actionMap.AddBinding(path, interactions, groups, action.ToString(), null);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00007ECC File Offset: 0x000060CC
		public static InputActionSetupExtensions.BindingSyntax AddBinding(this InputActionMap actionMap, InputBinding binding)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			if (binding.path == null)
			{
				throw new ArgumentException("Binding path cannot be null", "binding");
			}
			int num = InputActionSetupExtensions.AddBindingInternal(actionMap, binding, -1);
			return new InputActionSetupExtensions.BindingSyntax(actionMap, num, null);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00007F14 File Offset: 0x00006114
		public static InputActionSetupExtensions.CompositeSyntax AddCompositeBinding(this InputAction action, string composite, string interactions = null, string processors = null)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (string.IsNullOrEmpty(composite))
			{
				throw new ArgumentException("Composite name cannot be null or empty", "composite");
			}
			InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
			InputBinding inputBinding = new InputBinding
			{
				name = NameAndParameters.ParseName(composite),
				path = composite,
				interactions = interactions,
				processors = processors,
				isComposite = true,
				action = action.name
			};
			int num = InputActionSetupExtensions.AddBindingInternal(orCreateActionMap, inputBinding, -1);
			return new InputActionSetupExtensions.CompositeSyntax(orCreateActionMap, action, num);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00007FA4 File Offset: 0x000061A4
		private static int AddBindingInternal(InputActionMap map, InputBinding binding, int bindingIndex = -1)
		{
			if (string.IsNullOrEmpty(binding.m_Id))
			{
				binding.GenerateId();
			}
			if (bindingIndex < 0)
			{
				bindingIndex = ArrayHelpers.Append<InputBinding>(ref map.m_Bindings, binding);
			}
			else
			{
				ArrayHelpers.InsertAt<InputBinding>(ref map.m_Bindings, bindingIndex, binding);
			}
			if (map.asset != null)
			{
				map.asset.MarkAsDirty();
			}
			if (map.m_SingletonAction != null)
			{
				map.m_SingletonAction.m_SingletonActionBindings = map.m_Bindings;
			}
			map.OnBindingModified();
			return bindingIndex;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00008020 File Offset: 0x00006220
		public static InputActionSetupExtensions.BindingSyntax ChangeBinding(this InputAction action, int index)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			int num = action.BindingIndexOnActionToBindingIndexOnMap(index);
			return new InputActionSetupExtensions.BindingSyntax(action.GetOrCreateActionMap(), num, action);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008050 File Offset: 0x00006250
		public static InputActionSetupExtensions.BindingSyntax ChangeBinding(this InputAction action, string name)
		{
			return action.ChangeBinding(new InputBinding
			{
				name = name
			});
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00008074 File Offset: 0x00006274
		public static InputActionSetupExtensions.BindingSyntax ChangeBinding(this InputActionMap actionMap, int index)
		{
			if (actionMap == null)
			{
				throw new ArgumentNullException("actionMap");
			}
			if (index < 0 || index >= actionMap.m_Bindings.LengthSafe<InputBinding>())
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return new InputActionSetupExtensions.BindingSyntax(actionMap, index, null);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000080AC File Offset: 0x000062AC
		public static InputActionSetupExtensions.BindingSyntax ChangeBindingWithId(this InputAction action, string id)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			return action.ChangeBinding(new InputBinding
			{
				m_Id = id
			});
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000080E0 File Offset: 0x000062E0
		public static InputActionSetupExtensions.BindingSyntax ChangeBindingWithId(this InputAction action, Guid id)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			return action.ChangeBinding(new InputBinding
			{
				id = id
			});
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00008114 File Offset: 0x00006314
		public static InputActionSetupExtensions.BindingSyntax ChangeBindingWithGroup(this InputAction action, string group)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			return action.ChangeBinding(new InputBinding
			{
				groups = group
			});
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00008148 File Offset: 0x00006348
		public static InputActionSetupExtensions.BindingSyntax ChangeBindingWithPath(this InputAction action, string path)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			return action.ChangeBinding(new InputBinding
			{
				path = path
			});
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000817C File Offset: 0x0000637C
		public static InputActionSetupExtensions.BindingSyntax ChangeBinding(this InputAction action, InputBinding match)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
			Guid idDontGenerate = action.idDontGenerate;
			match.action = action.id.ToString();
			int num = orCreateActionMap.FindBindingRelativeToMap(match);
			if (num == -1)
			{
				match.action = action.name;
				num = orCreateActionMap.FindBindingRelativeToMap(match);
			}
			if (num == -1)
			{
				return default(InputActionSetupExtensions.BindingSyntax);
			}
			return new InputActionSetupExtensions.BindingSyntax(orCreateActionMap, num, action);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000081FC File Offset: 0x000063FC
		public static InputActionSetupExtensions.BindingSyntax ChangeCompositeBinding(this InputAction action, string compositeName)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (string.IsNullOrEmpty(compositeName))
			{
				throw new ArgumentNullException("compositeName");
			}
			InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
			InputBinding[] bindings = orCreateActionMap.m_Bindings;
			int num = bindings.LengthSafe<InputBinding>();
			for (int i = 0; i < num; i++)
			{
				ref InputBinding ptr = ref bindings[i];
				if (ptr.isComposite && ptr.TriggersAction(action) && (compositeName.Equals(ptr.name, StringComparison.InvariantCultureIgnoreCase) || compositeName.Equals(NameAndParameters.ParseName(ptr.path), StringComparison.InvariantCultureIgnoreCase)))
				{
					return new InputActionSetupExtensions.BindingSyntax(orCreateActionMap, i, action);
				}
			}
			return default(InputActionSetupExtensions.BindingSyntax);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000082A0 File Offset: 0x000064A0
		public static void Rename(this InputAction action, string newName)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (string.IsNullOrEmpty(newName))
			{
				throw new ArgumentNullException("newName");
			}
			if (action.name == newName)
			{
				return;
			}
			InputActionMap actionMap = action.actionMap;
			if (((actionMap != null) ? actionMap.FindAction(newName, false) : null) != null)
			{
				throw new InvalidOperationException(string.Format("Cannot rename '{0}' to '{1}' in map '{2}' as the map already contains an action with that name", action, newName, actionMap));
			}
			string name = action.m_Name;
			action.m_Name = newName;
			if (actionMap != null)
			{
				actionMap.ClearActionLookupTable();
			}
			if (((actionMap != null) ? actionMap.asset : null) != null && actionMap != null)
			{
				actionMap.asset.MarkAsDirty();
			}
			InputBinding[] bindings = action.GetOrCreateActionMap().m_Bindings;
			int num = bindings.LengthSafe<InputBinding>();
			for (int i = 0; i < num; i++)
			{
				if (string.Compare(bindings[i].action, name, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					bindings[i].action = newName;
				}
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008388 File Offset: 0x00006588
		public static void AddControlScheme(this InputActionAsset asset, InputControlScheme controlScheme)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (string.IsNullOrEmpty(controlScheme.name))
			{
				throw new ArgumentException("Cannot add control scheme without name to asset " + asset.name, "controlScheme");
			}
			if (asset.FindControlScheme(controlScheme.name) != null)
			{
				throw new InvalidOperationException(string.Concat(new string[] { "Asset '", asset.name, "' already contains a control scheme called '", controlScheme.name, "'" }));
			}
			ArrayHelpers.Append<InputControlScheme>(ref asset.m_ControlSchemes, controlScheme);
			asset.MarkAsDirty();
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00008438 File Offset: 0x00006638
		public static InputActionSetupExtensions.ControlSchemeSyntax AddControlScheme(this InputActionAsset asset, string name)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			int count = asset.controlSchemes.Count;
			asset.AddControlScheme(new InputControlScheme(name, null, null));
			return new InputActionSetupExtensions.ControlSchemeSyntax(asset, count);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00008490 File Offset: 0x00006690
		public static void RemoveControlScheme(this InputActionAsset asset, string name)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			int num = asset.FindControlSchemeIndex(name);
			if (num != -1)
			{
				ArrayHelpers.EraseAt<InputControlScheme>(ref asset.m_ControlSchemes, num);
			}
			asset.MarkAsDirty();
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000084E4 File Offset: 0x000066E4
		public static InputControlScheme WithBindingGroup(this InputControlScheme scheme, string bindingGroup)
		{
			return new InputActionSetupExtensions.ControlSchemeSyntax(scheme).WithBindingGroup(bindingGroup).Done();
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00008508 File Offset: 0x00006708
		public static InputControlScheme WithDevice(this InputControlScheme scheme, string controlPath, bool required)
		{
			if (required)
			{
				return new InputActionSetupExtensions.ControlSchemeSyntax(scheme).WithRequiredDevice(controlPath).Done();
			}
			return new InputActionSetupExtensions.ControlSchemeSyntax(scheme).WithOptionalDevice(controlPath).Done();
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00008548 File Offset: 0x00006748
		public static InputControlScheme WithRequiredDevice(this InputControlScheme scheme, string controlPath)
		{
			return new InputActionSetupExtensions.ControlSchemeSyntax(scheme).WithRequiredDevice(controlPath).Done();
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000856C File Offset: 0x0000676C
		public static InputControlScheme WithOptionalDevice(this InputControlScheme scheme, string controlPath)
		{
			return new InputActionSetupExtensions.ControlSchemeSyntax(scheme).WithOptionalDevice(controlPath).Done();
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00008590 File Offset: 0x00006790
		public static InputControlScheme OrWithRequiredDevice(this InputControlScheme scheme, string controlPath)
		{
			return new InputActionSetupExtensions.ControlSchemeSyntax(scheme).OrWithRequiredDevice(controlPath).Done();
		}

		// Token: 0x06000239 RID: 569 RVA: 0x000085B4 File Offset: 0x000067B4
		public static InputControlScheme OrWithOptionalDevice(this InputControlScheme scheme, string controlPath)
		{
			return new InputActionSetupExtensions.ControlSchemeSyntax(scheme).OrWithOptionalDevice(controlPath).Done();
		}

		// Token: 0x02000173 RID: 371
		public struct BindingSyntax
		{
			// Token: 0x17000513 RID: 1299
			// (get) Token: 0x060012FB RID: 4859 RVA: 0x00059FAA File Offset: 0x000581AA
			public bool valid
			{
				get
				{
					return this.m_ActionMap != null && this.m_BindingIndexInMap >= 0 && this.m_BindingIndexInMap < this.m_ActionMap.m_Bindings.LengthSafe<InputBinding>();
				}
			}

			// Token: 0x17000514 RID: 1300
			// (get) Token: 0x060012FC RID: 4860 RVA: 0x00059FD7 File Offset: 0x000581D7
			public int bindingIndex
			{
				get
				{
					if (!this.valid)
					{
						return -1;
					}
					if (this.m_Action != null)
					{
						return this.m_Action.BindingIndexOnMapToBindingIndexOnAction(this.m_BindingIndexInMap);
					}
					return this.m_BindingIndexInMap;
				}
			}

			// Token: 0x17000515 RID: 1301
			// (get) Token: 0x060012FD RID: 4861 RVA: 0x0005A003 File Offset: 0x00058203
			public InputBinding binding
			{
				get
				{
					if (!this.valid)
					{
						throw new InvalidOperationException("BindingSyntax accessor is not valid");
					}
					return this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap];
				}
			}

			// Token: 0x060012FE RID: 4862 RVA: 0x0005A02E File Offset: 0x0005822E
			internal BindingSyntax(InputActionMap map, int bindingIndexInMap, InputAction action = null)
			{
				this.m_ActionMap = map;
				this.m_BindingIndexInMap = bindingIndexInMap;
				this.m_Action = action;
			}

			// Token: 0x060012FF RID: 4863 RVA: 0x0005A048 File Offset: 0x00058248
			public InputActionSetupExtensions.BindingSyntax WithName(string name)
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].name = name;
				this.m_ActionMap.OnBindingModified();
				return this;
			}

			// Token: 0x06001300 RID: 4864 RVA: 0x0005A098 File Offset: 0x00058298
			public InputActionSetupExtensions.BindingSyntax WithPath(string path)
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].path = path;
				this.m_ActionMap.OnBindingModified();
				return this;
			}

			// Token: 0x06001301 RID: 4865 RVA: 0x0005A0E8 File Offset: 0x000582E8
			public InputActionSetupExtensions.BindingSyntax WithGroup(string group)
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				if (string.IsNullOrEmpty(group))
				{
					throw new ArgumentException("Group name cannot be null or empty", "group");
				}
				if (group.IndexOf(';') != -1)
				{
					throw new ArgumentException(string.Format("Group name cannot contain separator character '{0}'", ';'), "group");
				}
				return this.WithGroups(group);
			}

			// Token: 0x06001302 RID: 4866 RVA: 0x0005A150 File Offset: 0x00058350
			public InputActionSetupExtensions.BindingSyntax WithGroups(string groups)
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				if (string.IsNullOrEmpty(groups))
				{
					return this;
				}
				string groups2 = this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].groups;
				if (!string.IsNullOrEmpty(groups2))
				{
					groups = string.Join(";", new string[] { groups2, groups });
				}
				this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].groups = groups;
				this.m_ActionMap.OnBindingModified();
				return this;
			}

			// Token: 0x06001303 RID: 4867 RVA: 0x0005A1EC File Offset: 0x000583EC
			public InputActionSetupExtensions.BindingSyntax WithInteraction(string interaction)
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				if (string.IsNullOrEmpty(interaction))
				{
					throw new ArgumentException("Interaction cannot be null or empty", "interaction");
				}
				if (interaction.IndexOf(';') != -1)
				{
					throw new ArgumentException(string.Format("Interaction string cannot contain separator character '{0}'", ';'), "interaction");
				}
				return this.WithInteractions(interaction);
			}

			// Token: 0x06001304 RID: 4868 RVA: 0x0005A254 File Offset: 0x00058454
			public InputActionSetupExtensions.BindingSyntax WithInteractions(string interactions)
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				if (string.IsNullOrEmpty(interactions))
				{
					return this;
				}
				string interactions2 = this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].interactions;
				if (!string.IsNullOrEmpty(interactions2))
				{
					interactions = string.Join(";", new string[] { interactions2, interactions });
				}
				this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].interactions = interactions;
				this.m_ActionMap.OnBindingModified();
				return this;
			}

			// Token: 0x06001305 RID: 4869 RVA: 0x0005A2F0 File Offset: 0x000584F0
			public InputActionSetupExtensions.BindingSyntax WithInteraction<TInteraction>() where TInteraction : IInputInteraction
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				InternedString internedString = InputInteraction.s_Interactions.FindNameForType(typeof(TInteraction));
				if (internedString.IsEmpty())
				{
					throw new NotSupportedException(string.Format("Type '{0}' has not been registered as a interaction", typeof(TInteraction)));
				}
				return this.WithInteraction(internedString);
			}

			// Token: 0x06001306 RID: 4870 RVA: 0x0005A354 File Offset: 0x00058554
			public InputActionSetupExtensions.BindingSyntax WithProcessor(string processor)
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				if (string.IsNullOrEmpty(processor))
				{
					throw new ArgumentException("Processor cannot be null or empty", "processor");
				}
				if (processor.IndexOf(';') != -1)
				{
					throw new ArgumentException(string.Format("Processor string cannot contain separator character '{0}'", ';'), "processor");
				}
				return this.WithProcessors(processor);
			}

			// Token: 0x06001307 RID: 4871 RVA: 0x0005A3BC File Offset: 0x000585BC
			public InputActionSetupExtensions.BindingSyntax WithProcessors(string processors)
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				if (string.IsNullOrEmpty(processors))
				{
					return this;
				}
				string processors2 = this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].processors;
				if (!string.IsNullOrEmpty(processors2))
				{
					processors = string.Join(";", new string[] { processors2, processors });
				}
				this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].processors = processors;
				this.m_ActionMap.OnBindingModified();
				return this;
			}

			// Token: 0x06001308 RID: 4872 RVA: 0x0005A458 File Offset: 0x00058658
			public InputActionSetupExtensions.BindingSyntax WithProcessor<TProcessor>()
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				InternedString internedString = InputProcessor.s_Processors.FindNameForType(typeof(TProcessor));
				if (internedString.IsEmpty())
				{
					throw new NotSupportedException(string.Format("Type '{0}' has not been registered as a processor", typeof(TProcessor)));
				}
				return this.WithProcessor(internedString);
			}

			// Token: 0x06001309 RID: 4873 RVA: 0x0005A4BC File Offset: 0x000586BC
			public InputActionSetupExtensions.BindingSyntax Triggering(InputAction action)
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				if (action == null)
				{
					throw new ArgumentNullException("action");
				}
				if (action.isSingletonAction)
				{
					throw new ArgumentException(string.Format("Cannot change the action a binding triggers on singleton action '{0}'", action), "action");
				}
				this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].action = action.name;
				this.m_ActionMap.OnBindingModified();
				return this;
			}

			// Token: 0x0600130A RID: 4874 RVA: 0x0005A53C File Offset: 0x0005873C
			public InputActionSetupExtensions.BindingSyntax To(InputBinding binding)
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Accessor is not valid");
				}
				this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap] = binding;
				if (this.m_ActionMap.m_SingletonAction != null)
				{
					this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].action = this.m_ActionMap.m_SingletonAction.name;
				}
				this.m_ActionMap.OnBindingModified();
				return this;
			}

			// Token: 0x0600130B RID: 4875 RVA: 0x0005A5BC File Offset: 0x000587BC
			public InputActionSetupExtensions.BindingSyntax NextBinding()
			{
				return this.Iterate(true);
			}

			// Token: 0x0600130C RID: 4876 RVA: 0x0005A5C5 File Offset: 0x000587C5
			public InputActionSetupExtensions.BindingSyntax PreviousBinding()
			{
				return this.Iterate(false);
			}

			// Token: 0x0600130D RID: 4877 RVA: 0x0005A5CE File Offset: 0x000587CE
			public InputActionSetupExtensions.BindingSyntax NextPartBinding(string partName)
			{
				if (string.IsNullOrEmpty(partName))
				{
					throw new ArgumentNullException("partName");
				}
				return this.IteratePartBinding(true, partName);
			}

			// Token: 0x0600130E RID: 4878 RVA: 0x0005A5EB File Offset: 0x000587EB
			public InputActionSetupExtensions.BindingSyntax PreviousPartBinding(string partName)
			{
				if (string.IsNullOrEmpty(partName))
				{
					throw new ArgumentNullException("partName");
				}
				return this.IteratePartBinding(false, partName);
			}

			// Token: 0x0600130F RID: 4879 RVA: 0x0005A608 File Offset: 0x00058808
			public InputActionSetupExtensions.BindingSyntax NextCompositeBinding(string compositeName = null)
			{
				return this.IterateCompositeBinding(true, compositeName);
			}

			// Token: 0x06001310 RID: 4880 RVA: 0x0005A612 File Offset: 0x00058812
			public InputActionSetupExtensions.BindingSyntax PreviousCompositeBinding(string compositeName = null)
			{
				return this.IterateCompositeBinding(false, compositeName);
			}

			// Token: 0x06001311 RID: 4881 RVA: 0x0005A61C File Offset: 0x0005881C
			private InputActionSetupExtensions.BindingSyntax Iterate(bool next)
			{
				if (this.m_ActionMap == null)
				{
					return default(InputActionSetupExtensions.BindingSyntax);
				}
				InputBinding[] bindings = this.m_ActionMap.m_Bindings;
				if (bindings == null)
				{
					return default(InputActionSetupExtensions.BindingSyntax);
				}
				int num = this.m_BindingIndexInMap;
				for (;;)
				{
					num += (next ? 1 : (-1));
					if (num < 0 || num >= bindings.Length)
					{
						break;
					}
					if (this.m_Action == null || bindings[num].TriggersAction(this.m_Action))
					{
						goto IL_006C;
					}
				}
				return default(InputActionSetupExtensions.BindingSyntax);
				IL_006C:
				return new InputActionSetupExtensions.BindingSyntax(this.m_ActionMap, num, this.m_Action);
			}

			// Token: 0x06001312 RID: 4882 RVA: 0x0005A6A8 File Offset: 0x000588A8
			private InputActionSetupExtensions.BindingSyntax IterateCompositeBinding(bool next, string compositeName)
			{
				InputActionSetupExtensions.BindingSyntax bindingSyntax = this.Iterate(next);
				while (bindingSyntax.valid)
				{
					if (bindingSyntax.binding.isComposite)
					{
						if (compositeName == null)
						{
							return bindingSyntax;
						}
						if (compositeName.Equals(bindingSyntax.binding.name, StringComparison.InvariantCultureIgnoreCase))
						{
							return bindingSyntax;
						}
						string text = NameAndParameters.ParseName(bindingSyntax.binding.path);
						if (compositeName.Equals(text, StringComparison.InvariantCultureIgnoreCase))
						{
							return bindingSyntax;
						}
					}
					bindingSyntax = bindingSyntax.Iterate(next);
				}
				return default(InputActionSetupExtensions.BindingSyntax);
			}

			// Token: 0x06001313 RID: 4883 RVA: 0x0005A72C File Offset: 0x0005892C
			private InputActionSetupExtensions.BindingSyntax IteratePartBinding(bool next, string partName)
			{
				if (!this.valid)
				{
					return default(InputActionSetupExtensions.BindingSyntax);
				}
				if (this.binding.isComposite)
				{
					if (!next)
					{
						return default(InputActionSetupExtensions.BindingSyntax);
					}
				}
				else if (!this.binding.isPartOfComposite)
				{
					return default(InputActionSetupExtensions.BindingSyntax);
				}
				InputActionSetupExtensions.BindingSyntax bindingSyntax = this.Iterate(next);
				while (bindingSyntax.valid)
				{
					if (!bindingSyntax.binding.isPartOfComposite)
					{
						return default(InputActionSetupExtensions.BindingSyntax);
					}
					if (partName.Equals(bindingSyntax.binding.name, StringComparison.InvariantCultureIgnoreCase))
					{
						return bindingSyntax;
					}
					bindingSyntax = bindingSyntax.Iterate(next);
				}
				return default(InputActionSetupExtensions.BindingSyntax);
			}

			// Token: 0x06001314 RID: 4884 RVA: 0x0005A7DC File Offset: 0x000589DC
			public void Erase()
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("Instance not valid");
				}
				bool isComposite = this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].isComposite;
				ArrayHelpers.EraseAt<InputBinding>(ref this.m_ActionMap.m_Bindings, this.m_BindingIndexInMap);
				if (isComposite)
				{
					while (this.m_BindingIndexInMap < this.m_ActionMap.m_Bindings.LengthSafe<InputBinding>() && this.m_ActionMap.m_Bindings[this.m_BindingIndexInMap].isPartOfComposite)
					{
						ArrayHelpers.EraseAt<InputBinding>(ref this.m_ActionMap.m_Bindings, this.m_BindingIndexInMap);
					}
				}
				this.m_Action.m_BindingsCount = this.m_ActionMap.m_Bindings.LengthSafe<InputBinding>();
				this.m_ActionMap.OnBindingModified();
				if (this.m_ActionMap.m_SingletonAction != null)
				{
					this.m_ActionMap.m_SingletonAction.m_SingletonActionBindings = this.m_ActionMap.m_Bindings;
				}
			}

			// Token: 0x06001315 RID: 4885 RVA: 0x0005A8CC File Offset: 0x00058ACC
			public InputActionSetupExtensions.BindingSyntax InsertPartBinding(string partName, string path)
			{
				if (string.IsNullOrEmpty(partName))
				{
					throw new ArgumentNullException("partName");
				}
				if (!this.valid)
				{
					throw new InvalidOperationException("Binding accessor is not valid");
				}
				InputBinding binding = this.binding;
				if (!binding.isPartOfComposite && !binding.isComposite)
				{
					throw new InvalidOperationException("Binding accessor must point to composite or part binding");
				}
				InputActionMap actionMap = this.m_ActionMap;
				InputBinding inputBinding = default(InputBinding);
				inputBinding.path = path;
				inputBinding.isPartOfComposite = true;
				inputBinding.name = partName;
				InputAction action = this.m_Action;
				inputBinding.action = ((action != null) ? action.name : null);
				InputActionSetupExtensions.AddBindingInternal(actionMap, inputBinding, this.m_BindingIndexInMap + 1);
				return new InputActionSetupExtensions.BindingSyntax(this.m_ActionMap, this.m_BindingIndexInMap + 1, this.m_Action);
			}

			// Token: 0x04000814 RID: 2068
			private readonly InputActionMap m_ActionMap;

			// Token: 0x04000815 RID: 2069
			private readonly InputAction m_Action;

			// Token: 0x04000816 RID: 2070
			internal readonly int m_BindingIndexInMap;
		}

		// Token: 0x02000174 RID: 372
		public struct CompositeSyntax
		{
			// Token: 0x17000516 RID: 1302
			// (get) Token: 0x06001316 RID: 4886 RVA: 0x0005A98A File Offset: 0x00058B8A
			public int bindingIndex
			{
				get
				{
					if (this.m_ActionMap == null)
					{
						return -1;
					}
					if (this.m_Action != null)
					{
						return this.m_Action.BindingIndexOnMapToBindingIndexOnAction(this.m_BindingIndexInMap);
					}
					return this.m_BindingIndexInMap;
				}
			}

			// Token: 0x06001317 RID: 4887 RVA: 0x0005A9B6 File Offset: 0x00058BB6
			internal CompositeSyntax(InputActionMap map, InputAction action, int compositeIndex)
			{
				this.m_Action = action;
				this.m_ActionMap = map;
				this.m_BindingIndexInMap = compositeIndex;
			}

			// Token: 0x06001318 RID: 4888 RVA: 0x0005A9D0 File Offset: 0x00058BD0
			public InputActionSetupExtensions.CompositeSyntax With(string name, string binding, string groups = null, string processors = null)
			{
				using (InputActionRebindingExtensions.DeferBindingResolution())
				{
					int num;
					if (this.m_Action != null)
					{
						num = this.m_Action.AddBinding(binding, null, processors, groups).m_BindingIndexInMap;
					}
					else
					{
						num = this.m_ActionMap.AddBinding(binding, null, groups, null, processors).m_BindingIndexInMap;
					}
					this.m_ActionMap.m_Bindings[num].name = name;
					this.m_ActionMap.m_Bindings[num].isPartOfComposite = true;
				}
				return this;
			}

			// Token: 0x04000817 RID: 2071
			private readonly InputAction m_Action;

			// Token: 0x04000818 RID: 2072
			private readonly InputActionMap m_ActionMap;

			// Token: 0x04000819 RID: 2073
			private int m_BindingIndexInMap;
		}

		// Token: 0x02000175 RID: 373
		public struct ControlSchemeSyntax
		{
			// Token: 0x06001319 RID: 4889 RVA: 0x0005AA6C File Offset: 0x00058C6C
			internal ControlSchemeSyntax(InputActionAsset asset, int index)
			{
				this.m_Asset = asset;
				this.m_ControlSchemeIndex = index;
				this.m_ControlScheme = default(InputControlScheme);
			}

			// Token: 0x0600131A RID: 4890 RVA: 0x0005AA88 File Offset: 0x00058C88
			internal ControlSchemeSyntax(InputControlScheme controlScheme)
			{
				this.m_Asset = null;
				this.m_ControlSchemeIndex = -1;
				this.m_ControlScheme = controlScheme;
			}

			// Token: 0x0600131B RID: 4891 RVA: 0x0005AAA0 File Offset: 0x00058CA0
			public InputActionSetupExtensions.ControlSchemeSyntax WithBindingGroup(string bindingGroup)
			{
				if (string.IsNullOrEmpty(bindingGroup))
				{
					throw new ArgumentNullException("bindingGroup");
				}
				if (this.m_Asset == null)
				{
					this.m_ControlScheme.m_BindingGroup = bindingGroup;
				}
				else
				{
					this.m_Asset.m_ControlSchemes[this.m_ControlSchemeIndex].bindingGroup = bindingGroup;
				}
				return this;
			}

			// Token: 0x0600131C RID: 4892 RVA: 0x0005AAFE File Offset: 0x00058CFE
			public InputActionSetupExtensions.ControlSchemeSyntax WithRequiredDevice<TDevice>() where TDevice : InputDevice
			{
				return this.WithRequiredDevice(this.DeviceTypeToControlPath<TDevice>());
			}

			// Token: 0x0600131D RID: 4893 RVA: 0x0005AB0C File Offset: 0x00058D0C
			public InputActionSetupExtensions.ControlSchemeSyntax WithOptionalDevice<TDevice>() where TDevice : InputDevice
			{
				return this.WithOptionalDevice(this.DeviceTypeToControlPath<TDevice>());
			}

			// Token: 0x0600131E RID: 4894 RVA: 0x0005AB1A File Offset: 0x00058D1A
			public InputActionSetupExtensions.ControlSchemeSyntax OrWithRequiredDevice<TDevice>() where TDevice : InputDevice
			{
				return this.OrWithRequiredDevice(this.DeviceTypeToControlPath<TDevice>());
			}

			// Token: 0x0600131F RID: 4895 RVA: 0x0005AB28 File Offset: 0x00058D28
			public InputActionSetupExtensions.ControlSchemeSyntax OrWithOptionalDevice<TDevice>() where TDevice : InputDevice
			{
				return this.OrWithOptionalDevice(this.DeviceTypeToControlPath<TDevice>());
			}

			// Token: 0x06001320 RID: 4896 RVA: 0x0005AB36 File Offset: 0x00058D36
			public InputActionSetupExtensions.ControlSchemeSyntax WithRequiredDevice(string controlPath)
			{
				this.AddDeviceEntry(controlPath, InputControlScheme.DeviceRequirement.Flags.None);
				return this;
			}

			// Token: 0x06001321 RID: 4897 RVA: 0x0005AB46 File Offset: 0x00058D46
			public InputActionSetupExtensions.ControlSchemeSyntax WithOptionalDevice(string controlPath)
			{
				this.AddDeviceEntry(controlPath, InputControlScheme.DeviceRequirement.Flags.Optional);
				return this;
			}

			// Token: 0x06001322 RID: 4898 RVA: 0x0005AB56 File Offset: 0x00058D56
			public InputActionSetupExtensions.ControlSchemeSyntax OrWithRequiredDevice(string controlPath)
			{
				this.AddDeviceEntry(controlPath, InputControlScheme.DeviceRequirement.Flags.Or);
				return this;
			}

			// Token: 0x06001323 RID: 4899 RVA: 0x0005AB66 File Offset: 0x00058D66
			public InputActionSetupExtensions.ControlSchemeSyntax OrWithOptionalDevice(string controlPath)
			{
				this.AddDeviceEntry(controlPath, InputControlScheme.DeviceRequirement.Flags.Optional | InputControlScheme.DeviceRequirement.Flags.Or);
				return this;
			}

			// Token: 0x06001324 RID: 4900 RVA: 0x0005AB78 File Offset: 0x00058D78
			private string DeviceTypeToControlPath<TDevice>() where TDevice : InputDevice
			{
				string text = InputControlLayout.s_Layouts.TryFindLayoutForType(typeof(TDevice)).ToString();
				if (string.IsNullOrEmpty(text))
				{
					text = typeof(TDevice).Name;
				}
				return "<" + text + ">";
			}

			// Token: 0x06001325 RID: 4901 RVA: 0x0005ABD0 File Offset: 0x00058DD0
			public InputControlScheme Done()
			{
				if (this.m_Asset != null)
				{
					return this.m_Asset.m_ControlSchemes[this.m_ControlSchemeIndex];
				}
				return this.m_ControlScheme;
			}

			// Token: 0x06001326 RID: 4902 RVA: 0x0005AC00 File Offset: 0x00058E00
			private void AddDeviceEntry(string controlPath, InputControlScheme.DeviceRequirement.Flags flags)
			{
				if (string.IsNullOrEmpty(controlPath))
				{
					throw new ArgumentNullException("controlPath");
				}
				InputControlScheme inputControlScheme = ((this.m_Asset != null) ? this.m_Asset.m_ControlSchemes[this.m_ControlSchemeIndex] : this.m_ControlScheme);
				ArrayHelpers.Append<InputControlScheme.DeviceRequirement>(ref inputControlScheme.m_DeviceRequirements, new InputControlScheme.DeviceRequirement
				{
					m_ControlPath = controlPath,
					m_Flags = flags
				});
				if (this.m_Asset == null)
				{
					this.m_ControlScheme = inputControlScheme;
					return;
				}
				this.m_Asset.m_ControlSchemes[this.m_ControlSchemeIndex] = inputControlScheme;
			}

			// Token: 0x0400081A RID: 2074
			private readonly InputActionAsset m_Asset;

			// Token: 0x0400081B RID: 2075
			private readonly int m_ControlSchemeIndex;

			// Token: 0x0400081C RID: 2076
			private InputControlScheme m_ControlScheme;
		}
	}
}
