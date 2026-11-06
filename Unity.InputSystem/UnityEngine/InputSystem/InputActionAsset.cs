using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200001C RID: 28
	public class InputActionAsset : ScriptableObject, IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000171 RID: 369 RVA: 0x000041C4 File Offset: 0x000023C4
		public bool enabled
		{
			get
			{
				using (ReadOnlyArray<InputActionMap>.Enumerator enumerator = this.actionMaps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.enabled)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00004220 File Offset: 0x00002420
		public ReadOnlyArray<InputActionMap> actionMaps
		{
			get
			{
				return new ReadOnlyArray<InputActionMap>(this.m_ActionMaps);
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000173 RID: 371 RVA: 0x0000422D File Offset: 0x0000242D
		public ReadOnlyArray<InputControlScheme> controlSchemes
		{
			get
			{
				return new ReadOnlyArray<InputControlScheme>(this.m_ControlSchemes);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0000423A File Offset: 0x0000243A
		public IEnumerable<InputBinding> bindings
		{
			get
			{
				int numActionMaps = this.m_ActionMaps.LengthSafe<InputActionMap>();
				if (numActionMaps == 0)
				{
					yield break;
				}
				int num;
				for (int i = 0; i < numActionMaps; i = num)
				{
					InputActionMap inputActionMap = this.m_ActionMaps[i];
					InputBinding[] bindings = inputActionMap.m_Bindings;
					int numBindings = bindings.LengthSafe<InputBinding>();
					for (int j = 0; j < numBindings; j = num)
					{
						yield return bindings[j];
						num = j + 1;
					}
					bindings = null;
					num = i + 1;
				}
				yield break;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000175 RID: 373 RVA: 0x0000424A File Offset: 0x0000244A
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00004254 File Offset: 0x00002454
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
				this.ReResolveIfNecessary(true);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000042AD File Offset: 0x000024AD
		// (set) Token: 0x06000178 RID: 376 RVA: 0x000042BA File Offset: 0x000024BA
		public ReadOnlyArray<InputDevice>? devices
		{
			get
			{
				return this.m_Devices.Get();
			}
			set
			{
				if (this.m_Devices.Set(value))
				{
					this.ReResolveIfNecessary(false);
				}
			}
		}

		// Token: 0x1700009D RID: 157
		public InputAction this[string actionNameOrId]
		{
			get
			{
				InputAction inputAction = this.FindAction(actionNameOrId, false);
				if (inputAction == null)
				{
					throw new KeyNotFoundException(string.Format("Cannot find action '{0}' in '{1}'", actionNameOrId, this));
				}
				return inputAction;
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000042F0 File Offset: 0x000024F0
		public string ToJson()
		{
			bool flag = this.m_ActionMaps.LengthSafe<InputActionMap>() > 0 || this.m_ControlSchemes.LengthSafe<InputControlScheme>() > 0;
			return JsonUtility.ToJson(new InputActionAsset.WriteFileJson
			{
				version = (flag ? 1 : 0),
				name = base.name,
				maps = InputActionMap.WriteFileJson.FromMaps(this.m_ActionMaps).maps,
				controlSchemes = InputControlScheme.SchemeJson.ToJson(this.m_ControlSchemes)
			}, true);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00004378 File Offset: 0x00002578
		public void LoadFromJson(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				throw new ArgumentNullException("json");
			}
			InputActionAsset.ReadFileJson readFileJson = JsonUtility.FromJson<InputActionAsset.ReadFileJson>(json);
			this.MigrateJson(ref readFileJson);
			readFileJson.ToAsset(this);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000043AF File Offset: 0x000025AF
		public static InputActionAsset FromJson(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				throw new ArgumentNullException("json");
			}
			InputActionAsset inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
			inputActionAsset.LoadFromJson(json);
			return inputActionAsset;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000043D0 File Offset: 0x000025D0
		public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
		{
			if (actionNameOrId == null)
			{
				throw new ArgumentNullException("actionNameOrId");
			}
			if (this.m_ActionMaps != null)
			{
				int num = actionNameOrId.IndexOf('/');
				if (num >= 0)
				{
					Substring substring = new Substring(actionNameOrId, 0, num);
					Substring substring2 = new Substring(actionNameOrId, num + 1);
					if (substring.isEmpty || substring2.isEmpty)
					{
						throw new ArgumentException("Malformed action path: " + actionNameOrId, "actionNameOrId");
					}
					int i = 0;
					while (i < this.m_ActionMaps.Length)
					{
						InputActionMap inputActionMap = this.m_ActionMaps[i];
						if (Substring.Compare(inputActionMap.name, substring, StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							InputAction[] actions = inputActionMap.m_Actions;
							if (actions != null)
							{
								foreach (InputAction inputAction in actions)
								{
									if (Substring.Compare(inputAction.name, substring2, StringComparison.InvariantCultureIgnoreCase) == 0)
									{
										return inputAction;
									}
								}
								break;
							}
							break;
						}
						else
						{
							i++;
						}
					}
				}
				InputAction inputAction2 = null;
				for (int k = 0; k < this.m_ActionMaps.Length; k++)
				{
					InputAction inputAction3 = this.m_ActionMaps[k].FindAction(actionNameOrId, false);
					if (inputAction3 != null)
					{
						if (inputAction3.enabled || inputAction3.m_Id == actionNameOrId)
						{
							return inputAction3;
						}
						if (inputAction2 == null)
						{
							inputAction2 = inputAction3;
						}
					}
				}
				if (inputAction2 != null)
				{
					return inputAction2;
				}
			}
			if (throwIfNotFound)
			{
				throw new ArgumentException(string.Format("No action '{0}' in '{1}'", actionNameOrId, this));
			}
			return null;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00004528 File Offset: 0x00002728
		public int FindBinding(InputBinding mask, out InputAction action)
		{
			int num = this.m_ActionMaps.LengthSafe<InputActionMap>();
			for (int i = 0; i < num; i++)
			{
				int num2 = this.m_ActionMaps[i].FindBinding(mask, out action);
				if (num2 >= 0)
				{
					return num2;
				}
			}
			action = null;
			return -1;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00004568 File Offset: 0x00002768
		public InputActionMap FindActionMap(string nameOrId, bool throwIfNotFound = false)
		{
			if (nameOrId == null)
			{
				throw new ArgumentNullException("nameOrId");
			}
			if (this.m_ActionMaps == null)
			{
				return null;
			}
			Guid guid;
			if (nameOrId.Contains('-') && Guid.TryParse(nameOrId, out guid))
			{
				for (int i = 0; i < this.m_ActionMaps.Length; i++)
				{
					InputActionMap inputActionMap = this.m_ActionMaps[i];
					if (inputActionMap.idDontGenerate == guid)
					{
						return inputActionMap;
					}
				}
			}
			for (int j = 0; j < this.m_ActionMaps.Length; j++)
			{
				InputActionMap inputActionMap2 = this.m_ActionMaps[j];
				if (string.Compare(nameOrId, inputActionMap2.name, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return inputActionMap2;
				}
			}
			if (throwIfNotFound)
			{
				throw new ArgumentException(string.Format("Cannot find action map '{0}' in '{1}'", nameOrId, this));
			}
			return null;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00004614 File Offset: 0x00002814
		public InputActionMap FindActionMap(Guid id)
		{
			if (this.m_ActionMaps == null)
			{
				return null;
			}
			for (int i = 0; i < this.m_ActionMaps.Length; i++)
			{
				InputActionMap inputActionMap = this.m_ActionMaps[i];
				if (inputActionMap.idDontGenerate == id)
				{
					return inputActionMap;
				}
			}
			return null;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00004658 File Offset: 0x00002858
		public InputAction FindAction(Guid guid)
		{
			if (this.m_ActionMaps == null)
			{
				return null;
			}
			for (int i = 0; i < this.m_ActionMaps.Length; i++)
			{
				InputAction inputAction = this.m_ActionMaps[i].FindAction(guid);
				if (inputAction != null)
				{
					return inputAction;
				}
			}
			return null;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00004698 File Offset: 0x00002898
		public int FindControlSchemeIndex(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			if (this.m_ControlSchemes == null)
			{
				return -1;
			}
			for (int i = 0; i < this.m_ControlSchemes.Length; i++)
			{
				if (string.Compare(name, this.m_ControlSchemes[i].name, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000046F4 File Offset: 0x000028F4
		public InputControlScheme? FindControlScheme(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			int num = this.FindControlSchemeIndex(name);
			if (num == -1)
			{
				return null;
			}
			return new InputControlScheme?(this.m_ControlSchemes[num]);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000473C File Offset: 0x0000293C
		public bool IsUsableWithDevice(InputDevice device)
		{
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			int num = this.m_ControlSchemes.LengthSafe<InputControlScheme>();
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					if (this.m_ControlSchemes[i].SupportsDevice(device))
					{
						return true;
					}
				}
			}
			else
			{
				int num2 = this.m_ActionMaps.LengthSafe<InputActionMap>();
				for (int j = 0; j < num2; j++)
				{
					if (this.m_ActionMaps[j].IsUsableWithDevice(device))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000047B8 File Offset: 0x000029B8
		public void Enable()
		{
			foreach (InputActionMap inputActionMap in this.actionMaps)
			{
				inputActionMap.Enable();
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000480C File Offset: 0x00002A0C
		public void Disable()
		{
			foreach (InputActionMap inputActionMap in this.actionMaps)
			{
				inputActionMap.Disable();
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00004860 File Offset: 0x00002A60
		public bool Contains(InputAction action)
		{
			InputActionMap inputActionMap = ((action != null) ? action.actionMap : null);
			return inputActionMap != null && inputActionMap.asset == this;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000488B File Offset: 0x00002A8B
		public IEnumerator<InputAction> GetEnumerator()
		{
			if (this.m_ActionMaps == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.m_ActionMaps.Length; i = num)
			{
				ReadOnlyArray<InputAction> actions = this.m_ActionMaps[i].actions;
				int actionCount = actions.Count;
				for (int j = 0; j < actionCount; j = num)
				{
					yield return actions[j];
					num = j + 1;
				}
				actions = default(ReadOnlyArray<InputAction>);
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000489A File Offset: 0x00002A9A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000048A2 File Offset: 0x00002AA2
		internal void MarkAsDirty()
		{
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000048A4 File Offset: 0x00002AA4
		internal bool IsEmpty()
		{
			return this.actionMaps.Count == 0 && this.controlSchemes.Count == 0;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000048D4 File Offset: 0x00002AD4
		internal void OnWantToChangeSetup()
		{
			if (this.m_ActionMaps.LengthSafe<InputActionMap>() > 0)
			{
				this.m_ActionMaps[0].OnWantToChangeSetup();
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000048F1 File Offset: 0x00002AF1
		internal void OnSetupChanged()
		{
			this.MarkAsDirty();
			if (this.m_ActionMaps.LengthSafe<InputActionMap>() > 0)
			{
				this.m_ActionMaps[0].OnSetupChanged();
				return;
			}
			this.m_SharedStateForAllMaps = null;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000491C File Offset: 0x00002B1C
		private void ReResolveIfNecessary(bool fullResolve)
		{
			if (this.m_SharedStateForAllMaps == null)
			{
				return;
			}
			this.m_ActionMaps[0].LazyResolveBindings(fullResolve);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00004938 File Offset: 0x00002B38
		internal void ResolveBindingsIfNecessary()
		{
			if (this.m_ActionMaps.LengthSafe<InputActionMap>() > 0)
			{
				InputActionMap[] actionMaps = this.m_ActionMaps;
				int num = 0;
				while (num < actionMaps.Length && !actionMaps[num].ResolveBindingsIfNecessary())
				{
					num++;
				}
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00004972 File Offset: 0x00002B72
		private void OnDestroy()
		{
			this.Disable();
			if (this.m_SharedStateForAllMaps != null)
			{
				this.m_SharedStateForAllMaps.Dispose();
				this.m_SharedStateForAllMaps = null;
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00004994 File Offset: 0x00002B94
		internal void MigrateJson(ref InputActionAsset.ReadFileJson parsedJson)
		{
			if (parsedJson.version >= 1)
			{
				return;
			}
			InputActionMap.ReadMapJson[] maps = parsedJson.maps;
			if (((maps != null) ? maps.Length : 0) > 0 && parsedJson.version < 1)
			{
				for (int i = 0; i < parsedJson.maps.Length; i++)
				{
					InputActionMap.ReadMapJson readMapJson = parsedJson.maps[i];
					for (int j = 0; j < readMapJson.actions.Length; j++)
					{
						InputActionMap.ReadActionJson readActionJson = readMapJson.actions[j];
						string processors = readActionJson.processors;
						if (!string.IsNullOrEmpty(processors))
						{
							List<NameAndParameters> list = NameAndParameters.ParseMultiple(processors).ToList<NameAndParameters>();
							List<string> list2 = new List<string>(list.Count);
							foreach (NameAndParameters nameAndParameters in list)
							{
								Type type = InputSystem.TryGetProcessor(nameAndParameters.name);
								if (nameAndParameters.parameters.Count == 0 || type == null)
								{
									list2.Add(nameAndParameters.ToString());
								}
								else
								{
									Dictionary<string, string> dictionary = nameAndParameters.parameters.ToDictionary<NamedValue, string, string>((NamedValue p) => p.name, (NamedValue p) => p.value.ToString());
									bool flag = false;
									foreach (FieldInfo fieldInfo in from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public)
										where f.FieldType.IsEnum
										select f)
									{
										string text;
										int num;
										if (dictionary.TryGetValue(fieldInfo.Name, out text) && int.TryParse(text, out num))
										{
											object[] array = Enum.GetValues(fieldInfo.FieldType).Cast<object>().ToArray<object>();
											if (num >= 0 && num < array.Length)
											{
												dictionary[fieldInfo.Name] = Convert.ToInt32(array[num]).ToString();
												flag = true;
											}
										}
									}
									if (!flag)
									{
										list2.Add(nameAndParameters.ToString());
									}
									else
									{
										string text2 = string.Join(",", dictionary.Select<KeyValuePair<string, string>, string>((KeyValuePair<string, string> kv) => kv.Key + "=" + kv.Value));
										list2.Add(nameAndParameters.name + "(" + text2 + ")");
									}
								}
							}
							readActionJson.processors = string.Join(";", list2);
							readMapJson.actions[j] = readActionJson;
						}
					}
					parsedJson.maps[i] = readMapJson;
				}
			}
			parsedJson.version = 1;
		}

		// Token: 0x04000094 RID: 148
		public const string Extension = "inputactions";

		// Token: 0x04000095 RID: 149
		internal const string kDefaultAssetLayoutJson = "{}";

		// Token: 0x04000096 RID: 150
		[SerializeField]
		internal InputActionMap[] m_ActionMaps;

		// Token: 0x04000097 RID: 151
		[SerializeField]
		internal InputControlScheme[] m_ControlSchemes;

		// Token: 0x04000098 RID: 152
		[SerializeField]
		internal bool m_IsProjectWide;

		// Token: 0x04000099 RID: 153
		[NonSerialized]
		internal InputActionState m_SharedStateForAllMaps;

		// Token: 0x0400009A RID: 154
		[NonSerialized]
		internal InputBinding? m_BindingMask;

		// Token: 0x0400009B RID: 155
		[NonSerialized]
		internal int m_ParameterOverridesCount;

		// Token: 0x0400009C RID: 156
		[NonSerialized]
		internal InputActionRebindingExtensions.ParameterOverride[] m_ParameterOverrides;

		// Token: 0x0400009D RID: 157
		[NonSerialized]
		internal InputActionMap.DeviceArray m_Devices;

		// Token: 0x0200015A RID: 346
		private static class JsonVersion
		{
			// Token: 0x04000780 RID: 1920
			public const int Version0 = 0;

			// Token: 0x04000781 RID: 1921
			public const int Version1 = 1;

			// Token: 0x04000782 RID: 1922
			public const int Current = 1;
		}

		// Token: 0x0200015B RID: 347
		[Serializable]
		internal struct WriteFileJson
		{
			// Token: 0x04000783 RID: 1923
			public int version;

			// Token: 0x04000784 RID: 1924
			public string name;

			// Token: 0x04000785 RID: 1925
			public InputActionMap.WriteMapJson[] maps;

			// Token: 0x04000786 RID: 1926
			public InputControlScheme.SchemeJson[] controlSchemes;
		}

		// Token: 0x0200015C RID: 348
		[Serializable]
		internal struct WriteFileJsonNoName
		{
			// Token: 0x04000787 RID: 1927
			public InputActionMap.WriteMapJson[] maps;

			// Token: 0x04000788 RID: 1928
			public InputControlScheme.SchemeJson[] controlSchemes;
		}

		// Token: 0x0200015D RID: 349
		[Serializable]
		internal struct ReadFileJson
		{
			// Token: 0x06001284 RID: 4740 RVA: 0x00057848 File Offset: 0x00055A48
			public void ToAsset(InputActionAsset asset)
			{
				asset.name = this.name;
				InputActionMap.ReadFileJson readFileJson = default(InputActionMap.ReadFileJson);
				readFileJson.maps = this.maps;
				asset.m_ActionMaps = readFileJson.ToMaps();
				asset.m_ControlSchemes = InputControlScheme.SchemeJson.ToSchemes(this.controlSchemes);
				if (asset.m_ActionMaps != null)
				{
					InputActionMap[] actionMaps = asset.m_ActionMaps;
					for (int i = 0; i < actionMaps.Length; i++)
					{
						actionMaps[i].m_Asset = asset;
					}
				}
			}

			// Token: 0x04000789 RID: 1929
			public int version;

			// Token: 0x0400078A RID: 1930
			public string name;

			// Token: 0x0400078B RID: 1931
			public InputActionMap.ReadMapJson[] maps;

			// Token: 0x0400078C RID: 1932
			public InputControlScheme.SchemeJson[] controlSchemes;
		}
	}
}
