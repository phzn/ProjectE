using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Device;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Operations;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x020000A2 RID: 162
	public class LocalizationSettings : ScriptableObject, IReset, IDisposable
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0000FAF5 File Offset: 0x0000DCF5
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x0000FAFD File Offset: 0x0000DCFD
		internal bool IsChangingSelectedLocale { get; private set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x0000FB06 File Offset: 0x0000DD06
		internal bool HasSelectedLocaleChangedSubscribers
		{
			get
			{
				return this.m_SelectedLocaleChanged.Length != 0;
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600045D RID: 1117 RVA: 0x0000FB16 File Offset: 0x0000DD16
		// (remove) Token: 0x0600045E RID: 1118 RVA: 0x0000FB25 File Offset: 0x0000DD25
		public event Action<Locale> OnSelectedLocaleChanged
		{
			add
			{
				this.m_SelectedLocaleChanged.Add(value, 5);
			}
			remove
			{
				this.m_SelectedLocaleChanged.RemoveByMovingTail(value);
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0000FB33 File Offset: 0x0000DD33
		public static bool HasSettings
		{
			get
			{
				if (LocalizationSettings.s_Instance == null)
				{
					LocalizationSettings.s_Instance = LocalizationSettings.GetInstanceDontCreateDefault();
				}
				return LocalizationSettings.s_Instance != null;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x0000FB4E File Offset: 0x0000DD4E
		public static AsyncOperationHandle<LocalizationSettings> InitializationOperation
		{
			get
			{
				return LocalizationSettings.Instance.GetInitializationOperation();
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x0000FB5A File Offset: 0x0000DD5A
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x0000FB72 File Offset: 0x0000DD72
		public static LocalizationSettings Instance
		{
			get
			{
				if (LocalizationSettings.s_Instance == null)
				{
					LocalizationSettings.s_Instance = LocalizationSettings.GetOrCreateSettings();
				}
				return LocalizationSettings.s_Instance;
			}
			set
			{
				LocalizationSettings.s_Instance = value;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0000FB7A File Offset: 0x0000DD7A
		public static List<IStartupLocaleSelector> StartupLocaleSelectors
		{
			get
			{
				return LocalizationSettings.Instance.GetStartupLocaleSelectors();
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0000FB86 File Offset: 0x0000DD86
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x0000FB92 File Offset: 0x0000DD92
		public static ILocalesProvider AvailableLocales
		{
			get
			{
				return LocalizationSettings.Instance.GetAvailableLocales();
			}
			set
			{
				LocalizationSettings.Instance.SetAvailableLocales(value);
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0000FB9F File Offset: 0x0000DD9F
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x0000FBAB File Offset: 0x0000DDAB
		public static LocalizedAssetDatabase AssetDatabase
		{
			get
			{
				return LocalizationSettings.Instance.GetAssetDatabase();
			}
			set
			{
				LocalizationSettings.Instance.SetAssetDatabase(value);
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0000FBB8 File Offset: 0x0000DDB8
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x0000FBC4 File Offset: 0x0000DDC4
		public static LocalizedStringDatabase StringDatabase
		{
			get
			{
				return LocalizationSettings.Instance.GetStringDatabase();
			}
			set
			{
				LocalizationSettings.Instance.SetStringDatabase(value);
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0000FBD1 File Offset: 0x0000DDD1
		public static MetadataCollection Metadata
		{
			get
			{
				return LocalizationSettings.Instance.GetMetadata();
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x0000FBDD File Offset: 0x0000DDDD
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x0000FBE9 File Offset: 0x0000DDE9
		public static Locale SelectedLocale
		{
			get
			{
				return LocalizationSettings.Instance.GetSelectedLocale();
			}
			set
			{
				LocalizationSettings.Instance.SetSelectedLocale(value);
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x0000FBF6 File Offset: 0x0000DDF6
		public static AsyncOperationHandle<Locale> SelectedLocaleAsync
		{
			get
			{
				return LocalizationSettings.Instance.GetSelectedLocaleAsync();
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600046E RID: 1134 RVA: 0x0000FC02 File Offset: 0x0000DE02
		// (remove) Token: 0x0600046F RID: 1135 RVA: 0x0000FC0F File Offset: 0x0000DE0F
		public static event Action<Locale> SelectedLocaleChanged
		{
			add
			{
				LocalizationSettings.Instance.OnSelectedLocaleChanged += value;
			}
			remove
			{
				LocalizationSettings.Instance.OnSelectedLocaleChanged -= value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0000FC1C File Offset: 0x0000DE1C
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x0000FCBC File Offset: 0x0000DEBC
		public static Locale ProjectLocale
		{
			get
			{
				if (LocalizationSettings.Instance.m_ProjectLocale == null || LocalizationSettings.Instance.m_ProjectLocale.Identifier != LocalizationSettings.Instance.m_ProjectLocaleIdentifier)
				{
					IPreloadRequired preloadRequired = LocalizationSettings.Instance.GetAvailableLocales() as IPreloadRequired;
					if (preloadRequired != null && !preloadRequired.PreloadOperation.IsDone)
					{
						preloadRequired.PreloadOperation.WaitForCompletion();
					}
					LocalizationSettings instance = LocalizationSettings.Instance;
					ILocalesProvider availableLocales = LocalizationSettings.AvailableLocales;
					instance.m_ProjectLocale = ((availableLocales != null) ? availableLocales.GetLocale(LocalizationSettings.Instance.m_ProjectLocaleIdentifier) : null);
				}
				return LocalizationSettings.Instance.m_ProjectLocale;
			}
			set
			{
				LocalizationSettings.Instance.m_ProjectLocale = value;
				LocalizationSettings.Instance.m_ProjectLocaleIdentifier = ((value != null) ? value.Identifier : default(LocaleIdentifier));
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x0000FCF8 File Offset: 0x0000DEF8
		// (set) Token: 0x06000473 RID: 1139 RVA: 0x0000FD04 File Offset: 0x0000DF04
		public static bool InitializeSynchronously
		{
			get
			{
				return LocalizationSettings.Instance.m_InitializeSynchronously;
			}
			set
			{
				LocalizationSettings.Instance.m_InitializeSynchronously = value;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0000FD11 File Offset: 0x0000DF11
		// (set) Token: 0x06000475 RID: 1141 RVA: 0x0000FD1D File Offset: 0x0000DF1D
		public static PreloadBehavior PreloadBehavior
		{
			get
			{
				return LocalizationSettings.Instance.m_PreloadBehavior;
			}
			set
			{
				LocalizationSettings.Instance.m_PreloadBehavior = value;
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000FD2A File Offset: 0x0000DF2A
		internal virtual void OnEnable()
		{
			if (LocalizationSettings.s_Instance == null)
			{
				LocalizationSettings.s_Instance = this;
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000FD3F File Offset: 0x0000DF3F
		internal static void ValidateSettingsExist(string error = "")
		{
			if (!LocalizationSettings.HasSettings)
			{
				throw new Exception("There is no active LocalizationSettings.\n " + error);
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000FD5C File Offset: 0x0000DF5C
		public virtual AsyncOperationHandle<LocalizationSettings> GetInitializationOperation()
		{
			if (!this.m_InitializingOperationHandle.IsValid())
			{
				InitializationOperation initializationOperation = UnityEngine.Localization.Operations.InitializationOperation.Pool.Get();
				initializationOperation.Init(this);
				initializationOperation.Dependency = AddressablesInterface.Instance.InitializeAddressablesAsync();
				this.m_InitializingOperationHandle = AddressablesInterface.ResourceManager.StartOperation<LocalizationSettings>(initializationOperation, initializationOperation.Dependency);
				if (!this.m_InitializingOperationHandle.IsDone && this.m_InitializeSynchronously && PlaymodeState.IsPlaying)
				{
					this.m_InitializingOperationHandle.WaitForCompletion();
				}
			}
			return this.m_InitializingOperationHandle;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x0000FDE2 File Offset: 0x0000DFE2
		internal virtual RuntimePlatform Platform
		{
			get
			{
				return Application.platform;
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000FDE9 File Offset: 0x0000DFE9
		public List<IStartupLocaleSelector> GetStartupLocaleSelectors()
		{
			return this.m_StartupSelectors;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000FDF1 File Offset: 0x0000DFF1
		public void SetAvailableLocales(ILocalesProvider available)
		{
			this.m_AvailableLocales = available;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000FDFA File Offset: 0x0000DFFA
		public virtual ILocalesProvider GetAvailableLocales()
		{
			return this.m_AvailableLocales;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000FE02 File Offset: 0x0000E002
		public void SetAssetDatabase(LocalizedAssetDatabase database)
		{
			this.m_AssetDatabase = database;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000FE0B File Offset: 0x0000E00B
		public virtual LocalizedAssetDatabase GetAssetDatabase()
		{
			return this.m_AssetDatabase;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000FE13 File Offset: 0x0000E013
		public void SetStringDatabase(LocalizedStringDatabase database)
		{
			this.m_StringDatabase = database;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000FE1C File Offset: 0x0000E01C
		public virtual LocalizedStringDatabase GetStringDatabase()
		{
			return this.m_StringDatabase;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000FE24 File Offset: 0x0000E024
		public MetadataCollection GetMetadata()
		{
			return this.m_Metadata;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000FE2C File Offset: 0x0000E02C
		public void ForceRefresh()
		{
			if (this.m_SelectedLocaleAsync.IsValid() && this.m_SelectedLocaleAsync.IsDone)
			{
				this.InvokeSelectedLocaleChanged(this.m_SelectedLocaleAsync.Result);
				return;
			}
			this.InvokeSelectedLocaleChanged(null);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000FE64 File Offset: 0x0000E064
		internal void SendLocaleChangedEvents(Locale locale)
		{
			LocalizedStringDatabase stringDatabase = this.m_StringDatabase;
			if (stringDatabase != null)
			{
				stringDatabase.OnLocaleChanged(locale);
			}
			LocalizedAssetDatabase assetDatabase = this.m_AssetDatabase;
			if (assetDatabase != null)
			{
				assetDatabase.OnLocaleChanged(locale);
			}
			if (this.m_InitializingOperationHandle.IsValid())
			{
				AddressablesInterface.SafeRelease(this.m_InitializingOperationHandle);
				this.m_InitializingOperationHandle = default(AsyncOperationHandle<LocalizationSettings>);
			}
			if (this.GetInitializationOperation().Status == AsyncOperationStatus.Succeeded)
			{
				this.InvokeSelectedLocaleChanged(locale);
				return;
			}
			ComponentSingleton<LocalizationBehaviour>.Instance.StartCoroutine(this.InitializeAndCallSelectedLocaleChangedCoroutine(locale));
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000FEE9 File Offset: 0x0000E0E9
		private IEnumerator InitializeAndCallSelectedLocaleChangedCoroutine(Locale locale)
		{
			yield return this.m_InitializingOperationHandle;
			this.InvokeSelectedLocaleChanged(locale);
			yield break;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000FF00 File Offset: 0x0000E100
		private void InvokeSelectedLocaleChanged(Locale locale)
		{
			this.IsChangingSelectedLocale = true;
			try
			{
				this.m_SelectedLocaleChanged.LockForChanges();
				int length = this.m_SelectedLocaleChanged.Length;
				if (length == 1)
				{
					this.m_SelectedLocaleChanged.SingleDelegate(locale);
				}
				else if (length > 1)
				{
					Action<Locale>[] multiDelegates = this.m_SelectedLocaleChanged.MultiDelegates;
					for (int i = 0; i < length; i++)
					{
						multiDelegates[i](locale);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			this.IsChangingSelectedLocale = false;
			this.m_SelectedLocaleChanged.UnlockForChanges();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000FF94 File Offset: 0x0000E194
		private Locale SelectActiveLocale()
		{
			if (this.m_AvailableLocales == null)
			{
				Debug.LogError("AvailableLocales is null, can not pick a Locale.");
				return null;
			}
			if (this.m_AvailableLocales.Locales == null)
			{
				Debug.LogError("AvailableLocales.Locales is null, can not pick a Locale.");
				return null;
			}
			return this.SelectLocaleUsingStartupSelectors();
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000FFCC File Offset: 0x0000E1CC
		protected internal virtual Locale SelectLocaleUsingStartupSelectors()
		{
			foreach (IStartupLocaleSelector startupLocaleSelector in this.m_StartupSelectors)
			{
				Locale startupLocale = startupLocaleSelector.GetStartupLocale(this.GetAvailableLocales());
				if (startupLocale != null)
				{
					return startupLocale;
				}
			}
			StringBuilder stringBuilder;
			using (StringBuilderPool.Get(out stringBuilder))
			{
				stringBuilder.AppendLine("No Locale could be selected:");
				if (this.m_AvailableLocales.Locales.Count == 0)
				{
					stringBuilder.AppendLine("No Locales were available. Did you build the Addressables?");
				}
				else
				{
					stringBuilder.AppendLine(string.Format("The following ({0}) Locales were considered:", this.m_AvailableLocales.Locales.Count));
					foreach (Locale locale in this.m_AvailableLocales.Locales)
					{
						stringBuilder.AppendLine(string.Format("\t{0}", locale));
					}
				}
				stringBuilder.AppendLine(string.Format("The following ({0}) IStartupLocaleSelectors were used:", this.m_StartupSelectors.Count));
				foreach (IStartupLocaleSelector startupLocaleSelector2 in this.m_StartupSelectors)
				{
					stringBuilder.AppendLine(string.Format("\t{0}", startupLocaleSelector2));
				}
				Debug.LogError(stringBuilder.ToString(), this);
			}
			return null;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000101B0 File Offset: 0x0000E3B0
		public void SetSelectedLocale(Locale locale)
		{
			if (this.m_SelectedLocaleAsync.IsValid())
			{
				Locale result = this.m_SelectedLocaleAsync.Result;
				if (result == locale)
				{
					return;
				}
				if (result != null && locale != null && result.name == locale.name && result.Identifier.Code == locale.Identifier.Code)
				{
					return;
				}
			}
			this.GetInitializationOperation();
			if (locale == null && PlaymodeState.IsPlayingOrWillChangePlaymode)
			{
				return;
			}
			if (!this.m_SelectedLocaleAsync.IsValid() || this.m_SelectedLocaleAsync.Result != locale)
			{
				if (this.m_SelectedLocaleAsync.IsValid())
				{
					AddressablesInterface.Release(this.m_SelectedLocaleAsync);
				}
				this.m_SelectedLocaleAsync = AddressablesInterface.ResourceManager.CreateCompletedOperation<Locale>(locale, null);
				this.SendLocaleChangedEvents(locale);
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00010290 File Offset: 0x0000E490
		public virtual AsyncOperationHandle<Locale> GetSelectedLocaleAsync()
		{
			if (!this.m_SelectedLocaleAsync.IsValid())
			{
				IPreloadRequired preloadRequired = this.GetAvailableLocales() as IPreloadRequired;
				if (preloadRequired != null && !preloadRequired.PreloadOperation.IsDone)
				{
					this.m_SelectedLocaleAsync = AddressablesInterface.ResourceManager.CreateChainOperation<Locale>(preloadRequired.PreloadOperation, (AsyncOperationHandle op) => AddressablesInterface.ResourceManager.CreateCompletedOperation<Locale>(this.SelectActiveLocale(), null));
				}
				else
				{
					this.m_SelectedLocaleAsync = AddressablesInterface.ResourceManager.CreateCompletedOperation<Locale>(this.SelectActiveLocale(), null);
				}
			}
			return this.m_SelectedLocaleAsync;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0001030A File Offset: 0x0000E50A
		public virtual Locale GetSelectedLocale()
		{
			return AsyncOperationUtility.SynchronousLoad<Locale>(this.GetSelectedLocaleAsync());
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00010317 File Offset: 0x0000E517
		public virtual void OnLocaleRemoved(Locale locale)
		{
			if (this.m_SelectedLocaleAsync.IsValid() && this.m_SelectedLocaleAsync.Result == locale)
			{
				AddressablesInterface.Release(this.m_SelectedLocaleAsync);
				this.m_SelectedLocaleAsync = default(AsyncOperationHandle<Locale>);
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00010350 File Offset: 0x0000E550
		public void ResetState()
		{
			this.m_SelectedLocaleAsync = default(AsyncOperationHandle<Locale>);
			this.m_InitializingOperationHandle = default(AsyncOperationHandle<LocalizationSettings>);
			IReset reset = this.m_AvailableLocales as IReset;
			if (reset != null)
			{
				reset.ResetState();
			}
			LocalizedAssetDatabase assetDatabase = this.m_AssetDatabase;
			if (assetDatabase != null)
			{
				((IReset)assetDatabase).ResetState();
			}
			LocalizedStringDatabase stringDatabase = this.m_StringDatabase;
			if (stringDatabase == null)
			{
				return;
			}
			((IReset)stringDatabase).ResetState();
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000103AC File Offset: 0x0000E5AC
		void IDisposable.Dispose()
		{
			if (this.m_InitializingOperationHandle.IsValid())
			{
				if (!this.m_InitializingOperationHandle.IsDone)
				{
					this.m_InitializingOperationHandle.WaitForCompletion();
				}
				AddressablesInterface.Release(this.m_InitializingOperationHandle);
			}
			if (this.m_SelectedLocaleAsync.IsValid())
			{
				AddressablesInterface.Release(this.m_SelectedLocaleAsync);
			}
			this.m_InitializingOperationHandle = default(AsyncOperationHandle<LocalizationSettings>);
			this.m_SelectedLocaleAsync = default(AsyncOperationHandle<Locale>);
			IDisposable disposable = this.m_AvailableLocales as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			LocalizedAssetDatabase assetDatabase = this.m_AssetDatabase;
			if (assetDatabase != null)
			{
				((IDisposable)assetDatabase).Dispose();
			}
			LocalizedStringDatabase stringDatabase = this.m_StringDatabase;
			if (stringDatabase != null)
			{
				((IDisposable)stringDatabase).Dispose();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00010462 File Offset: 0x0000E662
		public static LocalizationSettings GetInstanceDontCreateDefault()
		{
			if (LocalizationSettings.s_Instance != null)
			{
				return LocalizationSettings.s_Instance;
			}
			return Object.FindObjectOfType<LocalizationSettings>();
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00010478 File Offset: 0x0000E678
		private static LocalizationSettings GetOrCreateSettings()
		{
			LocalizationSettings localizationSettings = LocalizationSettings.GetInstanceDontCreateDefault();
			if (localizationSettings == null)
			{
				localizationSettings = ScriptableObject.CreateInstance<LocalizationSettings>();
				localizationSettings.name = "Default Localization Settings";
			}
			return localizationSettings;
		}

		// Token: 0x0400013F RID: 319
		internal const string ConfigName = "com.unity.localization.settings";

		// Token: 0x04000140 RID: 320
		internal const string ConfigEditorLocale = "com.unity.localization-edit-locale";

		// Token: 0x04000141 RID: 321
		internal const string IgnoreSettings = "IgnoreSettings";

		// Token: 0x04000142 RID: 322
		internal const string LocaleLabel = "Locale";

		// Token: 0x04000143 RID: 323
		internal const string PreloadLabel = "Preload";

		// Token: 0x04000144 RID: 324
		[SerializeReference]
		private List<IStartupLocaleSelector> m_StartupSelectors = new List<IStartupLocaleSelector>
		{
			new CommandLineLocaleSelector(),
			new SystemLocaleSelector(),
			new SpecificLocaleSelector()
		};

		// Token: 0x04000145 RID: 325
		[SerializeReference]
		private ILocalesProvider m_AvailableLocales = new LocalesProvider();

		// Token: 0x04000146 RID: 326
		[SerializeReference]
		private LocalizedAssetDatabase m_AssetDatabase = new LocalizedAssetDatabase();

		// Token: 0x04000147 RID: 327
		[SerializeReference]
		private LocalizedStringDatabase m_StringDatabase = new LocalizedStringDatabase();

		// Token: 0x04000148 RID: 328
		[MetadataType(MetadataType.LocalizationSettings)]
		[SerializeField]
		private MetadataCollection m_Metadata = new MetadataCollection();

		// Token: 0x04000149 RID: 329
		[SerializeField]
		internal LocaleIdentifier m_ProjectLocaleIdentifier = "en";

		// Token: 0x0400014A RID: 330
		[SerializeField]
		private PreloadBehavior m_PreloadBehavior = PreloadBehavior.PreloadSelectedLocale;

		// Token: 0x0400014B RID: 331
		[SerializeField]
		private bool m_InitializeSynchronously;

		// Token: 0x0400014C RID: 332
		internal AsyncOperationHandle<LocalizationSettings> m_InitializingOperationHandle;

		// Token: 0x0400014D RID: 333
		private AsyncOperationHandle<Locale> m_SelectedLocaleAsync;

		// Token: 0x0400014E RID: 334
		private Locale m_ProjectLocale;

		// Token: 0x0400014F RID: 335
		private CallbackArray<Action<Locale>> m_SelectedLocaleChanged;

		// Token: 0x04000151 RID: 337
		internal static LocalizationSettings s_Instance;
	}
}
