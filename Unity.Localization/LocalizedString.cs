using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	public class LocalizedString : LocalizedReference, IVariableGroup, IDictionary<string, IVariable>, ICollection<KeyValuePair<string, IVariable>>, IEnumerable<KeyValuePair<string, IVariable>>, IEnumerable, IVariableValueChanged, IVariable, IDisposable
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600008D RID: 141 RVA: 0x00002D94 File Offset: 0x00000F94
		// (remove) Token: 0x0600008E RID: 142 RVA: 0x00002DCC File Offset: 0x00000FCC
		public event Action<IVariable> ValueChanged;

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00002E01 File Offset: 0x00001001
		internal override bool ForceSynchronous
		{
			get
			{
				return this.WaitForCompletion || LocalizationSettings.StringDatabase.AsynchronousBehaviour == AsynchronousBehaviour.ForceSynchronous;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00002E1A File Offset: 0x0000101A
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00002E22 File Offset: 0x00001022
		public IList<object> Arguments { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00002E2B File Offset: 0x0000102B
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00002E33 File Offset: 0x00001033
		public AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult> CurrentLoadingOperationHandle { get; internal set; }

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000094 RID: 148 RVA: 0x00002E3C File Offset: 0x0000103C
		// (remove) Token: 0x06000095 RID: 149 RVA: 0x00002EB5 File Offset: 0x000010B5
		public event LocalizedString.ChangeHandler StringChanged
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.m_ChangeHandler.Add(value, 5);
				if (this.m_ChangeHandler.Length == 1)
				{
					LocalizationSettings.ValidateSettingsExist("");
					this.ForceUpdate();
					LocalizationSettings.SelectedLocaleChanged += this.m_SelectedLocaleChanged;
					return;
				}
				if (this.CurrentLoadingOperationHandle.IsValid() && this.CurrentLoadingOperationHandle.IsDone)
				{
					value(this.m_CurrentStringChangedValue);
				}
			}
			remove
			{
				this.m_ChangeHandler.RemoveByMovingTail(value);
				if (this.m_ChangeHandler.Length == 0)
				{
					LocalizationSettings.SelectedLocaleChanged -= this.m_SelectedLocaleChanged;
					this.ClearLoadingOperation();
					this.ClearVariableListeners();
				}
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00002EE7 File Offset: 0x000010E7
		public bool HasChangeHandler
		{
			get
			{
				return this.m_ChangeHandler.Length != 0;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002EF8 File Offset: 0x000010F8
		public LocalizedString()
		{
			this.m_SelectedLocaleChanged = new Action<Locale>(this.HandleLocaleChange);
			this.m_OnVariableChanged = new Action<IVariable>(this.OnVariableChanged);
			this.m_AutomaticLoadingCompleted = new Action<AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult>>(this.AutomaticLoadingCompleted);
			this.m_CompletedSourceValue = new Action<AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult>>(this.CompletedSourceValue);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002F74 File Offset: 0x00001174
		public LocalizedString(TableReference tableReference, TableEntryReference entryReference)
			: this()
		{
			base.TableReference = tableReference;
			base.TableEntryReference = entryReference;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002F8C File Offset: 0x0000118C
		public bool RefreshString()
		{
			if (this.m_ChangeHandler.Length == 0 || !this.CurrentLoadingOperationHandle.IsValid())
			{
				return false;
			}
			if (this.CurrentLoadingOperationHandle.IsDone)
			{
				StringTableEntry entry = this.CurrentLoadingOperationHandle.Result.Entry;
				FormatCache formatCache = ((entry != null) ? entry.GetOrCreateFormatCache() : null);
				if (formatCache != null)
				{
					formatCache.LocalVariables = this;
					formatCache.VariableTriggers.Clear();
				}
				string text = LocalizationSettings.StringDatabase.GenerateLocalizedString(this.CurrentLoadingOperationHandle.Result.Table, entry, base.TableReference, base.TableEntryReference, LocalizationSettings.SelectedLocale, this.Arguments);
				if (formatCache != null)
				{
					formatCache.LocalVariables = null;
					List<IVariableValueChanged> list;
					if (entry == null)
					{
						list = null;
					}
					else
					{
						FormatCache formatCache2 = entry.FormatCache;
						list = ((formatCache2 != null) ? formatCache2.VariableTriggers : null);
					}
					this.UpdateVariableListeners(list);
				}
				this.m_CurrentStringChangedValue = text;
				this.InvokeChangeHandler(this.m_CurrentStringChangedValue);
				return true;
			}
			if (this.ForceSynchronous)
			{
				this.CurrentLoadingOperationHandle.WaitForCompletion();
				return true;
			}
			return false;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003093 File Offset: 0x00001293
		public AsyncOperationHandle<string> GetLocalizedStringAsync()
		{
			return this.GetLocalizedStringAsync(this.Arguments);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000030A1 File Offset: 0x000012A1
		public string GetLocalizedString()
		{
			return AsyncOperationUtility.SynchronousLoad<string>(this.GetLocalizedStringAsync());
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000030AE File Offset: 0x000012AE
		public AsyncOperationHandle<string> GetLocalizedStringAsync(params object[] arguments)
		{
			return this.GetLocalizedStringAsync(arguments);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000030B7 File Offset: 0x000012B7
		public string GetLocalizedString(params object[] arguments)
		{
			return AsyncOperationUtility.SynchronousLoad<string>(this.GetLocalizedStringAsync(arguments));
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000030C5 File Offset: 0x000012C5
		public string GetLocalizedString(IList<object> arguments)
		{
			return AsyncOperationUtility.SynchronousLoad<string>(this.GetLocalizedStringAsync(arguments));
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000030D4 File Offset: 0x000012D4
		public AsyncOperationHandle<string> GetLocalizedStringAsync(IList<object> arguments)
		{
			LocalizationSettings.ValidateSettingsExist("");
			return LocalizationSettings.StringDatabase.GetLocalizedStringAsync(base.TableReference, base.TableEntryReference, arguments, base.LocaleOverride, base.FallbackState, (this.m_LocalVariables.Count > 0) ? this : null);
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003120 File Offset: 0x00001320
		public int Count
		{
			get
			{
				return this.m_VariableLookup.Count;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000312D File Offset: 0x0000132D
		public ICollection<string> Keys
		{
			get
			{
				return this.m_VariableLookup.Keys;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000313A File Offset: 0x0000133A
		public ICollection<IVariable> Values
		{
			get
			{
				return this.m_VariableLookup.Values.Select<VariableNameValuePair, IVariable>((VariableNameValuePair s) => s.variable).ToList<IVariable>();
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003170 File Offset: 0x00001370
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000022 RID: 34
		public IVariable this[string name]
		{
			get
			{
				return this.m_VariableLookup[name].variable;
			}
			set
			{
				this.Add(name, value);
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003190 File Offset: 0x00001390
		public bool TryGetValue(string name, out IVariable value)
		{
			VariableNameValuePair variableNameValuePair;
			if (this.m_VariableLookup.TryGetValue(name, out variableNameValuePair))
			{
				value = variableNameValuePair.variable;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000031BC File Offset: 0x000013BC
		public void Add(string name, IVariable variable)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("name", "Name must not be null or empty.");
			}
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			name = name.ReplaceWhiteSpaces("-");
			VariableNameValuePair variableNameValuePair;
			if (this.m_VariableLookup.TryGetValue(name, out variableNameValuePair))
			{
				if (variableNameValuePair.variable == variable)
				{
					return;
				}
				this.m_LocalVariables.Remove(variableNameValuePair);
			}
			VariableNameValuePair variableNameValuePair2 = new VariableNameValuePair
			{
				name = name,
				variable = variable
			};
			this.m_VariableLookup[name] = variableNameValuePair2;
			this.m_LocalVariables.Add(variableNameValuePair2);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003250 File Offset: 0x00001450
		public void Add(KeyValuePair<string, IVariable> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003268 File Offset: 0x00001468
		public bool Remove(string name)
		{
			VariableNameValuePair variableNameValuePair;
			if (this.m_VariableLookup.TryGetValue(name, out variableNameValuePair))
			{
				this.m_LocalVariables.Remove(variableNameValuePair);
				this.m_VariableLookup.Remove(name);
				return true;
			}
			return false;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000032A2 File Offset: 0x000014A2
		public bool Remove(KeyValuePair<string, IVariable> item)
		{
			return this.Remove(item.Key);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000032B1 File Offset: 0x000014B1
		public bool ContainsKey(string name)
		{
			return this.m_VariableLookup.ContainsKey(name);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000032C0 File Offset: 0x000014C0
		public bool Contains(KeyValuePair<string, IVariable> item)
		{
			IVariable variable;
			return this.TryGetValue(item.Key, out variable) && variable == item.Value;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000032EC File Offset: 0x000014EC
		public void CopyTo(KeyValuePair<string, IVariable>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			foreach (KeyValuePair<string, VariableNameValuePair> keyValuePair in this.m_VariableLookup)
			{
				array[arrayIndex++] = new KeyValuePair<string, IVariable>(keyValuePair.Key, keyValuePair.Value.variable);
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000336C File Offset: 0x0000156C
		IEnumerator<KeyValuePair<string, IVariable>> IEnumerable<KeyValuePair<string, IVariable>>.GetEnumerator()
		{
			foreach (KeyValuePair<string, VariableNameValuePair> keyValuePair in this.m_VariableLookup)
			{
				yield return new KeyValuePair<string, IVariable>(keyValuePair.Key, keyValuePair.Value.variable);
			}
			Dictionary<string, VariableNameValuePair>.Enumerator enumerator = default(Dictionary<string, VariableNameValuePair>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000337B File Offset: 0x0000157B
		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<string, VariableNameValuePair> keyValuePair in this.m_VariableLookup)
			{
				yield return new KeyValuePair<string, IVariable>(keyValuePair.Key, keyValuePair.Value.variable);
			}
			Dictionary<string, VariableNameValuePair>.Enumerator enumerator = default(Dictionary<string, VariableNameValuePair>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000338A File Offset: 0x0000158A
		public void Clear()
		{
			this.m_VariableLookup.Clear();
			this.m_LocalVariables.Clear();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000033A4 File Offset: 0x000015A4
		public object GetSourceValue(ISelectorInfo selector)
		{
			if (base.IsEmpty)
			{
				throw new DataNotReadyException("{Empty}");
			}
			Locale locale = base.LocaleOverride;
			if (locale == null && selector.FormatDetails.FormatCache != null)
			{
				locale = LocalizationSettings.AvailableLocales.GetLocale(selector.FormatDetails.FormatCache.Table.LocaleIdentifier);
			}
			if (locale == null && LocalizationSettings.SelectedLocaleAsync.IsDone)
			{
				locale = LocalizationSettings.SelectedLocaleAsync.Result;
			}
			if (locale == null)
			{
				throw new DataNotReadyException("{No Available Locale}");
			}
			AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult> tableEntryAsync = LocalizationSettings.StringDatabase.GetTableEntryAsync(base.TableReference, base.TableEntryReference, locale, base.FallbackState);
			if (!tableEntryAsync.IsDone)
			{
				tableEntryAsync.Completed += this.m_CompletedSourceValue;
				throw new DataNotReadyException();
			}
			StringTableEntry entry = tableEntryAsync.Result.Entry;
			if (entry == null)
			{
				throw new DataNotReadyException("{Missing Entry}");
			}
			if (!entry.IsSmart)
			{
				return new LocalizedString.StringTableEntryVariable(LocalizationSettings.StringDatabase.GenerateLocalizedString(tableEntryAsync.Result.Table, entry, base.TableReference, base.TableEntryReference, locale, this.Arguments), entry, this);
			}
			FormatCache formatCache = ((entry != null) ? entry.GetOrCreateFormatCache() : null);
			if (formatCache != null)
			{
				formatCache.VariableTriggers.Clear();
				if (this.m_VariableLookup.Count > 0)
				{
					formatCache.LocalVariables = new LocalizedString.ChainedLocalVariablesGroup(this, selector.FormatDetails.FormatCache.LocalVariables);
				}
				else
				{
					formatCache.LocalVariables = selector.FormatDetails.FormatCache.LocalVariables;
				}
			}
			List<object> list;
			object obj;
			using (CollectionPool<List<object>, object>.Get(out list))
			{
				if (selector.CurrentValue != null)
				{
					list.Add(selector.CurrentValue);
				}
				if (this.Arguments != null)
				{
					list.AddRange(this.Arguments);
				}
				string text = LocalizationSettings.StringDatabase.GenerateLocalizedString(tableEntryAsync.Result.Table, entry, base.TableReference, base.TableEntryReference, locale, list);
				if (formatCache != null)
				{
					formatCache.LocalVariables = null;
					this.UpdateVariableListeners(formatCache.VariableTriggers);
				}
				obj = new LocalizedString.StringTableEntryVariable(text, entry, this);
			}
			return obj;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000035E0 File Offset: 0x000017E0
		private void CompletedSourceValue(AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult> _)
		{
			Action<IVariable> valueChanged = this.ValueChanged;
			if (valueChanged == null)
			{
				return;
			}
			valueChanged(this);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000035F3 File Offset: 0x000017F3
		protected internal override void ForceUpdate()
		{
			if (this.m_ChangeHandler.Length != 0)
			{
				this.HandleLocaleChange(null);
			}
			Action<IVariable> valueChanged = this.ValueChanged;
			if (valueChanged == null)
			{
				return;
			}
			valueChanged(this);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000361C File Offset: 0x0000181C
		private void ClearVariableListeners()
		{
			foreach (IVariableValueChanged variableValueChanged in this.m_UsedVariables)
			{
				variableValueChanged.ValueChanged -= this.m_OnVariableChanged;
			}
			this.m_UsedVariables.Clear();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003680 File Offset: 0x00001880
		private void UpdateVariableListeners(List<IVariableValueChanged> variables)
		{
			this.ClearVariableListeners();
			if (variables == null)
			{
				return;
			}
			foreach (IVariableValueChanged variableValueChanged in variables)
			{
				this.m_UsedVariables.Add(variableValueChanged);
				variableValueChanged.ValueChanged += this.m_OnVariableChanged;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000036EC File Offset: 0x000018EC
		private void OnVariableChanged(IVariable globalVariable)
		{
			if (this.m_WaitingForVariablesEndUpdate)
			{
				return;
			}
			if (PersistentVariablesSource.IsUpdating)
			{
				this.m_WaitingForVariablesEndUpdate = true;
				PersistentVariablesSource.EndUpdate += this.OnVariablesSourceUpdateCompleted;
				return;
			}
			this.RefreshString();
			Action<IVariable> valueChanged = this.ValueChanged;
			if (valueChanged == null)
			{
				return;
			}
			valueChanged(this);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000373A File Offset: 0x0000193A
		private void OnVariablesSourceUpdateCompleted()
		{
			PersistentVariablesSource.EndUpdate -= this.OnVariablesSourceUpdateCompleted;
			this.m_WaitingForVariablesEndUpdate = false;
			this.RefreshString();
			Action<IVariable> valueChanged = this.ValueChanged;
			if (valueChanged == null)
			{
				return;
			}
			valueChanged(this);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000376C File Offset: 0x0000196C
		private void InvokeChangeHandler(string value)
		{
			try
			{
				this.m_ChangeHandler.LockForChanges();
				int length = this.m_ChangeHandler.Length;
				if (length == 1)
				{
					this.m_ChangeHandler.SingleDelegate(value);
				}
				else if (length > 1)
				{
					LocalizedString.ChangeHandler[] multiDelegates = this.m_ChangeHandler.MultiDelegates;
					for (int i = 0; i < length; i++)
					{
						multiDelegates[i](value);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			this.m_ChangeHandler.UnlockForChanges();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000037F0 File Offset: 0x000019F0
		private void HandleLocaleChange(Locale locale)
		{
			this.ClearLoadingOperation();
			this.m_CurrentStringChangedValue = null;
			if (base.IsEmpty)
			{
				return;
			}
			this.CurrentLoadingOperationHandle = LocalizationSettings.StringDatabase.GetTableEntryAsync(base.TableReference, base.TableEntryReference, base.LocaleOverride, base.FallbackState);
			AddressablesInterface.Acquire(this.CurrentLoadingOperationHandle);
			if (!this.CurrentLoadingOperationHandle.IsDone)
			{
				if (!this.ForceSynchronous)
				{
					this.CurrentLoadingOperationHandle.Completed += this.m_AutomaticLoadingCompleted;
					return;
				}
				this.CurrentLoadingOperationHandle.WaitForCompletion();
			}
			this.AutomaticLoadingCompleted(this.CurrentLoadingOperationHandle);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003898 File Offset: 0x00001A98
		private void AutomaticLoadingCompleted(AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult> loadOperation)
		{
			if (loadOperation.Status != AsyncOperationStatus.Succeeded)
			{
				this.CurrentLoadingOperationHandle = default(AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult>);
				return;
			}
			this.RefreshString();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000038C8 File Offset: 0x00001AC8
		private void ClearLoadingOperation()
		{
			if (this.CurrentLoadingOperationHandle.IsValid())
			{
				if (!this.CurrentLoadingOperationHandle.IsDone)
				{
					this.CurrentLoadingOperationHandle.Completed -= this.m_AutomaticLoadingCompleted;
				}
				AddressablesInterface.Release(this.CurrentLoadingOperationHandle);
				this.CurrentLoadingOperationHandle = default(AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult>);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003928 File Offset: 0x00001B28
		protected override void Reset()
		{
			this.ClearLoadingOperation();
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003930 File Offset: 0x00001B30
		public override void OnAfterDeserialize()
		{
			this.m_VariableLookup.Clear();
			foreach (VariableNameValuePair variableNameValuePair in this.m_LocalVariables)
			{
				if (!string.IsNullOrEmpty(variableNameValuePair.name))
				{
					this.m_VariableLookup[variableNameValuePair.name] = variableNameValuePair;
				}
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000039A8 File Offset: 0x00001BA8
		~LocalizedString()
		{
			this.ClearLoadingOperation();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000039D4 File Offset: 0x00001BD4
		void IDisposable.Dispose()
		{
			this.m_ChangeHandler.Clear();
			this.ClearLoadingOperation();
			this.ClearVariableListeners();
			LocalizationSettings.SelectedLocaleChanged -= this.m_SelectedLocaleChanged;
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000017 RID: 23
		[SerializeField]
		private List<VariableNameValuePair> m_LocalVariables = new List<VariableNameValuePair>();

		// Token: 0x04000018 RID: 24
		private CallbackArray<LocalizedString.ChangeHandler> m_ChangeHandler;

		// Token: 0x04000019 RID: 25
		private string m_CurrentStringChangedValue;

		// Token: 0x0400001A RID: 26
		private readonly Dictionary<string, VariableNameValuePair> m_VariableLookup = new Dictionary<string, VariableNameValuePair>();

		// Token: 0x0400001B RID: 27
		private readonly List<IVariableValueChanged> m_UsedVariables = new List<IVariableValueChanged>();

		// Token: 0x0400001C RID: 28
		private readonly Action<IVariable> m_OnVariableChanged;

		// Token: 0x0400001D RID: 29
		private readonly Action<Locale> m_SelectedLocaleChanged;

		// Token: 0x0400001E RID: 30
		private readonly Action<AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult>> m_AutomaticLoadingCompleted;

		// Token: 0x0400001F RID: 31
		private readonly Action<AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult>> m_CompletedSourceValue;

		// Token: 0x04000020 RID: 32
		private bool m_WaitingForVariablesEndUpdate;

		// Token: 0x02000110 RID: 272
		// (Invoke) Token: 0x060006E1 RID: 1761
		public delegate void ChangeHandler(string value);

		// Token: 0x02000111 RID: 273
		private struct StringTableEntryVariable : IVariableGroup
		{
			// Token: 0x060006E4 RID: 1764 RVA: 0x0001764A File Offset: 0x0001584A
			public StringTableEntryVariable(string localized, StringTableEntry entry, IVariableGroup localVariables)
			{
				this.m_Localized = localized;
				this.m_StringTableEntry = entry;
				this.m_LocalVariables = localVariables;
			}

			// Token: 0x060006E5 RID: 1765 RVA: 0x00017664 File Offset: 0x00015864
			public bool TryGetValue(string key, out IVariable value)
			{
				foreach (IMetadata metadata in this.m_StringTableEntry.MetadataEntries)
				{
					IMetadataVariable metadataVariable = metadata as IMetadataVariable;
					if (metadataVariable != null && metadataVariable.VariableName == key)
					{
						value = metadataVariable;
						return true;
					}
				}
				if (this.m_LocalVariables != null && this.m_LocalVariables.TryGetValue(key, out value))
				{
					return true;
				}
				value = null;
				return false;
			}

			// Token: 0x060006E6 RID: 1766 RVA: 0x000176EC File Offset: 0x000158EC
			public override string ToString()
			{
				return this.m_Localized;
			}

			// Token: 0x04000255 RID: 597
			private readonly string m_Localized;

			// Token: 0x04000256 RID: 598
			private readonly StringTableEntry m_StringTableEntry;

			// Token: 0x04000257 RID: 599
			private readonly IVariableGroup m_LocalVariables;
		}

		// Token: 0x02000112 RID: 274
		private struct ChainedLocalVariablesGroup : IVariableGroup
		{
			// Token: 0x17000170 RID: 368
			// (get) Token: 0x060006E7 RID: 1767 RVA: 0x000176F4 File Offset: 0x000158F4
			// (set) Token: 0x060006E8 RID: 1768 RVA: 0x000176FC File Offset: 0x000158FC
			private IVariableGroup ParentGroup { readonly get; set; }

			// Token: 0x17000171 RID: 369
			// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00017705 File Offset: 0x00015905
			// (set) Token: 0x060006EA RID: 1770 RVA: 0x0001770D File Offset: 0x0001590D
			private IVariableGroup Group { readonly get; set; }

			// Token: 0x060006EB RID: 1771 RVA: 0x00017716 File Offset: 0x00015916
			public ChainedLocalVariablesGroup(IVariableGroup group, IVariableGroup parent)
			{
				this.Group = group;
				this.ParentGroup = parent;
			}

			// Token: 0x060006EC RID: 1772 RVA: 0x00017726 File Offset: 0x00015926
			public bool TryGetValue(string key, out IVariable value)
			{
				if (this.Group.TryGetValue(key, out value))
				{
					return true;
				}
				if (this.ParentGroup.TryGetValue(key, out value))
				{
					return true;
				}
				value = null;
				return false;
			}
		}
	}
}
