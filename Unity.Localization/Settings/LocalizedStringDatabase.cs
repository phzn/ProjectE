using System;
using System.Collections.Generic;
using UnityEngine.Localization.Operations;
using UnityEngine.Localization.Pseudo;
using UnityEngine.Localization.SmartFormat;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x0200009C RID: 156
	[Serializable]
	public class LocalizedStringDatabase : LocalizedDatabase<StringTable, StringTableEntry>
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000432 RID: 1074 RVA: 0x0000F3A8 File Offset: 0x0000D5A8
		// (remove) Token: 0x06000433 RID: 1075 RVA: 0x0000F3E0 File Offset: 0x0000D5E0
		public event LocalizedStringDatabase.MissingTranslation TranslationNotFound;

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0000F415 File Offset: 0x0000D615
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x0000F41D File Offset: 0x0000D61D
		public string NoTranslationFoundMessage
		{
			get
			{
				return this.m_NoTranslationFoundMessage;
			}
			set
			{
				this.m_NoTranslationFoundMessage = value;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000F426 File Offset: 0x0000D626
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x0000F42E File Offset: 0x0000D62E
		public MissingTranslationBehavior MissingTranslationState
		{
			get
			{
				return this.m_MissingTranslationState;
			}
			set
			{
				this.m_MissingTranslationState = value;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0000F437 File Offset: 0x0000D637
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x0000F43F File Offset: 0x0000D63F
		public SmartFormatter SmartFormatter
		{
			get
			{
				return this.m_SmartFormat;
			}
			set
			{
				this.m_SmartFormat = value;
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000F448 File Offset: 0x0000D648
		public AsyncOperationHandle<string> GetLocalizedStringAsync(TableEntryReference tableEntryReference, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings, params object[] arguments)
		{
			return this.GetLocalizedStringAsyncInternal(base.GetDefaultTable(), tableEntryReference, arguments, locale, fallbackBehavior, null, true);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000F45D File Offset: 0x0000D65D
		public string GetLocalizedString(TableEntryReference tableEntryReference, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings, params object[] arguments)
		{
			return this.GetLocalizedString(base.GetDefaultTable(), tableEntryReference, arguments, locale, fallbackBehavior);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000F470 File Offset: 0x0000D670
		public AsyncOperationHandle<string> GetLocalizedStringAsync(TableEntryReference tableEntryReference, IList<object> arguments, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings)
		{
			return this.GetLocalizedStringAsyncInternal(base.GetDefaultTable(), tableEntryReference, arguments, locale, fallbackBehavior, null, true);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000F485 File Offset: 0x0000D685
		public string GetLocalizedString(TableEntryReference tableEntryReference, IList<object> arguments, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings)
		{
			return this.GetLocalizedString(base.GetDefaultTable(), tableEntryReference, arguments, locale, fallbackBehavior);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000F498 File Offset: 0x0000D698
		public virtual AsyncOperationHandle<string> GetLocalizedStringAsync(TableReference tableReference, TableEntryReference tableEntryReference, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings, params object[] arguments)
		{
			return this.GetLocalizedStringAsyncInternal(tableReference, tableEntryReference, arguments, locale, fallbackBehavior, null, true);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000F4A9 File Offset: 0x0000D6A9
		public virtual string GetLocalizedString(TableReference tableReference, TableEntryReference tableEntryReference, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings, params object[] arguments)
		{
			return this.GetLocalizedString(tableReference, tableEntryReference, arguments, locale, fallbackBehavior);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000F4B8 File Offset: 0x0000D6B8
		public virtual AsyncOperationHandle<string> GetLocalizedStringAsync(TableReference tableReference, TableEntryReference tableEntryReference, IList<object> arguments, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings, IVariableGroup localVariables = null)
		{
			return this.GetLocalizedStringAsyncInternal(tableReference, tableEntryReference, arguments, locale, fallbackBehavior, localVariables, true);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000F4CC File Offset: 0x0000D6CC
		internal virtual AsyncOperationHandle<string> GetLocalizedStringAsyncInternal(TableReference tableReference, TableEntryReference tableEntryReference, IList<object> arguments, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings, IVariableGroup localVariables = null, bool autoRelease = true)
		{
			AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult> tableEntryAsync = this.GetTableEntryAsync(tableReference, tableEntryReference, locale, fallbackBehavior);
			GetLocalizedStringOperation getLocalizedStringOperation = GetLocalizedStringOperation.Pool.Get();
			getLocalizedStringOperation.Dependency = tableEntryAsync;
			getLocalizedStringOperation.Init(tableEntryAsync, locale, this, tableReference, tableEntryReference, arguments, localVariables, autoRelease);
			return AddressablesInterface.ResourceManager.StartOperation<string>(getLocalizedStringOperation, tableEntryAsync);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000F520 File Offset: 0x0000D720
		public virtual string GetLocalizedString(TableReference tableReference, TableEntryReference tableEntryReference, IList<object> arguments, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings)
		{
			AsyncOperationHandle<string> localizedStringAsyncInternal = this.GetLocalizedStringAsyncInternal(tableReference, tableEntryReference, arguments, locale, fallbackBehavior, null, false);
			string text = AsyncOperationUtility.SynchronousLoad<string>(localizedStringAsyncInternal);
			AddressablesInterface.Release(localizedStringAsyncInternal);
			return text;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000F550 File Offset: 0x0000D750
		protected internal virtual string GenerateLocalizedString(StringTable table, StringTableEntry entry, TableReference tableReference, TableEntryReference tableEntryReference, Locale locale, IList<object> arguments)
		{
			string text = ((entry != null) ? entry.GetLocalizedString(locale, arguments, locale as PseudoLocale) : null);
			if (string.IsNullOrEmpty(text))
			{
				SharedTableData sharedTableData = ((table != null) ? table.SharedData : null);
				if (sharedTableData == null && tableReference.ReferenceType == TableReference.Type.Guid)
				{
					AsyncOperationHandle<SharedTableData> sharedTableData2 = base.GetSharedTableData(tableReference.TableCollectionNameGuid);
					if (sharedTableData2.IsDone)
					{
						sharedTableData = sharedTableData2.Result;
					}
				}
				string text2 = tableEntryReference.ResolveKeyName(sharedTableData);
				return this.ProcessUntranslatedText(text2, tableEntryReference.KeyId, tableReference, table, locale);
			}
			return text;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000F5DC File Offset: 0x0000D7DC
		private StringTable GetUntranslatedTextTempTable(TableReference tableReference)
		{
			if (this.m_MissingTranslationTable == null)
			{
				this.m_MissingTranslationTable = ScriptableObject.CreateInstance<StringTable>();
				this.m_MissingTranslationTable.SharedData = ScriptableObject.CreateInstance<SharedTableData>();
			}
			if (tableReference.ReferenceType == TableReference.Type.Guid)
			{
				this.m_MissingTranslationTable.SharedData.TableCollectionNameGuid = tableReference;
				AsyncOperationHandle<SharedTableData> sharedTableData = base.GetSharedTableData(tableReference.TableCollectionNameGuid);
				if (sharedTableData.IsDone && sharedTableData.Result != null)
				{
					this.m_MissingTranslationTable.SharedData.TableCollectionName = sharedTableData.Result.TableCollectionName;
				}
				else
				{
					this.m_MissingTranslationTable.SharedData.TableCollectionName = tableReference.TableCollectionNameGuid.ToString();
				}
			}
			else if (tableReference.ReferenceType == TableReference.Type.Name)
			{
				this.m_MissingTranslationTable.SharedData.TableCollectionName = tableReference.TableCollectionName;
				this.m_MissingTranslationTable.SharedData.TableCollectionNameGuid = Guid.Empty;
			}
			return this.m_MissingTranslationTable;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000F6DC File Offset: 0x0000D8DC
		internal string ProcessUntranslatedText(string key, long keyId, TableReference tableReference, StringTable table, Locale locale)
		{
			if (table == null)
			{
				table = this.GetUntranslatedTextTempTable(tableReference);
			}
			if (this.MissingTranslationState != (MissingTranslationBehavior)0 || this.TranslationNotFound != null)
			{
				Dictionary<string, object> dictionary;
				using (CollectionPool<Dictionary<string, object>, KeyValuePair<string, object>>.Get(out dictionary))
				{
					dictionary["key"] = key;
					dictionary["keyId"] = keyId;
					dictionary["table"] = table;
					dictionary["locale"] = locale;
					string text = this.m_SmartFormat.Format(string.IsNullOrEmpty(this.NoTranslationFoundMessage) ? "No translation found for '{key}' in {table.TableCollectionName}" : this.NoTranslationFoundMessage, new object[] { dictionary });
					LocalizedStringDatabase.MissingTranslation translationNotFound = this.TranslationNotFound;
					if (translationNotFound != null)
					{
						translationNotFound(key, keyId, tableReference, table, locale, text);
					}
					if (this.MissingTranslationState.HasFlag(MissingTranslationBehavior.PrintWarning))
					{
						Debug.LogWarning(text);
					}
					if (this.MissingTranslationState.HasFlag(MissingTranslationBehavior.ShowMissingTranslationMessage))
					{
						return text;
					}
				}
			}
			return string.Empty;
		}

		// Token: 0x04000137 RID: 311
		[SerializeField]
		private MissingTranslationBehavior m_MissingTranslationState = MissingTranslationBehavior.ShowMissingTranslationMessage;

		// Token: 0x04000139 RID: 313
		private const string k_DefaultNoTranslationMessage = "No translation found for '{key}' in {table.TableCollectionName}";

		// Token: 0x0400013A RID: 314
		[SerializeField]
		[Tooltip("The string that will be used when a localized value is missing. This is a Smart String which has access to the following placeholders:\n\t{key}: The name of the key\n\t{keyId}: The numeric Id of the key\n\t{table}: The table object, this can be further queried, for example {table.TableCollectionName}\n\t{locale}: The locale asset, this can be further queried, for example {locale.name}")]
		private string m_NoTranslationFoundMessage = "No translation found for '{key}' in {table.TableCollectionName}";

		// Token: 0x0400013B RID: 315
		[SerializeReference]
		private SmartFormatter m_SmartFormat = Smart.CreateDefaultSmartFormat();

		// Token: 0x0400013C RID: 316
		private StringTable m_MissingTranslationTable;

		// Token: 0x02000148 RID: 328
		// (Invoke) Token: 0x060007D1 RID: 2001
		public delegate void MissingTranslation(string key, long keyId, TableReference tableReference, StringTable table, Locale locale, string noTranslationFoundMessage);
	}
}
