using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.Localization
{
	// Token: 0x020010BE RID: 4286
	public static class GameLocalization
	{
		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x06008209 RID: 33289 RVA: 0x003522A8 File Offset: 0x003504A8
		// (remove) Token: 0x0600820A RID: 33290 RVA: 0x003522DC File Offset: 0x003504DC
		public static event Action LocaleChanged;

		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x0600820B RID: 33291 RVA: 0x00352310 File Offset: 0x00350510
		// (remove) Token: 0x0600820C RID: 33292 RVA: 0x00352344 File Offset: 0x00350544
		public static event Action LoadingChanged;

		// Token: 0x170014A7 RID: 5287
		// (get) Token: 0x0600820D RID: 33293 RVA: 0x00352377 File Offset: 0x00350577
		public static string CurrentCode
		{
			get
			{
				return GameLocalization._currentCode;
			}
		}

		// Token: 0x170014A8 RID: 5288
		// (get) Token: 0x0600820E RID: 33294 RVA: 0x0035237E File Offset: 0x0035057E
		public static string RequestedCode
		{
			get
			{
				if (!GameLocalization._loading)
				{
					return GameLocalization._currentCode;
				}
				return GameLocalization._requestedCode;
			}
		}

		// Token: 0x170014A9 RID: 5289
		// (get) Token: 0x0600820F RID: 33295 RVA: 0x00352392 File Offset: 0x00350592
		public static bool IsLoading
		{
			get
			{
				return GameLocalization._loading;
			}
		}

		// Token: 0x170014AA RID: 5290
		// (get) Token: 0x06008210 RID: 33296 RVA: 0x00352399 File Offset: 0x00350599
		public static GameLocalizationConfig Config
		{
			get
			{
				return GameLocalization._config;
			}
		}

		// Token: 0x170014AB RID: 5291
		// (get) Token: 0x06008211 RID: 33297 RVA: 0x003523A0 File Offset: 0x003505A0
		public static bool NeedsLanguageSelection
		{
			get
			{
				return !PlayerPrefs.HasKey("Settings.Localization.Language") || !GameLocalization.IsSupported(PlayerPrefs.GetString("Settings.Localization.Language"));
			}
		}

		// Token: 0x06008212 RID: 33298 RVA: 0x003523C2 File Offset: 0x003505C2
		public static bool ConfirmCurrentLanguage()
		{
			if (GameLocalization.IsLoading || !GameLocalization.HasLoadedTable || !GameLocalization.IsSupported(GameLocalization.CurrentCode))
			{
				return false;
			}
			PlayerPrefs.SetString("Settings.Localization.Language", GameLocalization.CurrentCode);
			PlayerPrefs.Save();
			return true;
		}

		// Token: 0x06008213 RID: 33299 RVA: 0x003523F8 File Offset: 0x003505F8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			GameLocalization.ReleaseOwnedTable();
			LocalizationSettings.SelectedLocaleChanged -= GameLocalization.OnUnityLocaleChanged;
			SceneManager.sceneLoaded -= GameLocalization.BindScene;
			GameLocalization.SourceKeys.Clear();
			GameLocalization._config = null;
			GameLocalization._runner = null;
			GameLocalization._table = null;
			GameLocalization._currentCode = "en";
			GameLocalization._requestedCode = "en";
			GameLocalization._request = 0;
			GameLocalization._loading = false;
			GameLocalization._retryLoad = false;
			GameLocalization.LocaleChanged = null;
			GameLocalization.LoadingChanged = null;
		}

		// Token: 0x06008214 RID: 33300 RVA: 0x0035247C File Offset: 0x0035067C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Bootstrap()
		{
			GameLocalization._config = Resources.Load<GameLocalizationConfig>("Localization/GameLocalizationConfig");
			if (GameLocalization._config == null)
			{
				Debug.LogError("Localization configuration is missing. English source text will be used.");
				return;
			}
			foreach (GameLocalizationConfig.SourceEntry sourceEntry in GameLocalization._config.sources)
			{
				GameLocalization.SourceKeys.Add(GameLocalization.SourceId(sourceEntry.scope, sourceEntry.english), sourceEntry.key);
			}
			GameObject gameObject = new GameObject("Game Localization");
			Object.DontDestroyOnLoad(gameObject);
			GameLocalization._runner = gameObject.AddComponent<GameLocalizationRunner>();
			LocalizationSettings.SelectedLocaleChanged += GameLocalization.OnUnityLocaleChanged;
			SceneManager.sceneLoaded += GameLocalization.BindScene;
			string @string = PlayerPrefs.GetString("Settings.Localization.Language", "en");
			GameLocalization._requestedCode = (GameLocalization.IsSupported(@string) ? @string : "en");
			GameLocalization._runner.StartCoroutine(GameLocalization.LoadInitialLocale(GameLocalization._requestedCode, ++GameLocalization._request));
		}

		// Token: 0x06008215 RID: 33301 RVA: 0x00352574 File Offset: 0x00350774
		public static bool IsSupported(string code)
		{
			if (GameLocalization._config == null)
			{
				return code == "en";
			}
			GameLocalizationConfig.Language[] languages = GameLocalization._config.languages;
			for (int i = 0; i < languages.Length; i++)
			{
				if (languages[i].code == code)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008216 RID: 33302 RVA: 0x003525C6 File Offset: 0x003507C6
		public static bool SetLocale(string code, bool save = true)
		{
			if (GameLocalization._runner == null || !GameLocalization.IsSupported(code))
			{
				return false;
			}
			GameLocalization._requestedCode = code;
			GameLocalization._runner.StartCoroutine(GameLocalization.LoadLocale(code, save, ++GameLocalization._request));
			return true;
		}

		// Token: 0x06008217 RID: 33303 RVA: 0x00352605 File Offset: 0x00350805
		private static IEnumerator LoadLocale(string code, bool save, int request)
		{
			return GameLocalization.LoadLocaleRequest(code, save, request, false);
		}

		// Token: 0x06008218 RID: 33304 RVA: 0x00352610 File Offset: 0x00350810
		private static IEnumerator LoadInitialLocale(string code, int request)
		{
			return GameLocalization.LoadLocaleRequest(code, false, request, true);
		}

		// Token: 0x06008219 RID: 33305 RVA: 0x0035261B File Offset: 0x0035081B
		private static IEnumerator LoadLocaleRequest(string code, bool save, int request, bool initial)
		{
			GameLocalization._loading = true;
			GameLocalization.Notify(GameLocalization.LoadingChanged);
			if (initial)
			{
				yield return null;
			}
			int attempts = (initial ? 2 : 1);
			int attempt = 0;
			while (attempt < attempts && request == GameLocalization._request)
			{
				float started = Time.realtimeSinceStartup;
				IEnumerator routine = GameLocalization.LoadLocaleRoutine(code, save, request);
				Exception failure = null;
				try
				{
					while (request == GameLocalization._request)
					{
						bool flag = false;
						object obj = null;
						try
						{
							flag = routine.MoveNext();
							if (flag)
							{
								obj = routine.Current;
							}
						}
						catch (Exception ex)
						{
							failure = ex;
						}
						if (failure != null || !flag)
						{
							break;
						}
						yield return obj;
					}
				}
				finally
				{
					try
					{
						IDisposable disposable = routine as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					catch (Exception ex2)
					{
						if (failure == null)
						{
							failure = ex2;
						}
					}
				}
				if (failure == null || request != GameLocalization._request)
				{
					yield break;
				}
				string text = string.Format("[Localization] Request {0} ({1}) failed after ", request, code) + string.Format("{0:F2}s; build={1}.", Time.realtimeSinceStartup - started, Application.buildGUID);
				if (attempt + 1 < attempts)
				{
					GameLocalization._retryLoad = true;
					Debug.LogWarning(text + string.Format(" Retrying startup automatically. {0}", failure));
					yield return null;
				}
				else
				{
					string text2 = text;
					string text3 = " ";
					Exception ex3 = failure;
					GameLocalization.FailSwitch(text2 + text3 + ((ex3 != null) ? ex3.ToString() : null));
				}
				routine = null;
				failure = null;
				int num = attempt;
				attempt = num + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600821A RID: 33306 RVA: 0x0035263F File Offset: 0x0035083F
		private static IEnumerator LoadLocaleRoutine(string code, bool save, int request)
		{
			if (GameLocalization._retryLoad)
			{
				GameLocalization.ResetFailedLoad();
				GameLocalization._retryLoad = false;
			}
			AsyncOperationHandle initialization = Addressables.ResourceManager.Acquire<LocalizationSettings>(LocalizationSettings.InitializationOperation);
			try
			{
				float deadline = Time.realtimeSinceStartup + 30f;
				while (request == GameLocalization._request && !initialization.IsDone && Time.realtimeSinceStartup < deadline)
				{
					yield return null;
				}
				if (request != GameLocalization._request)
				{
					yield break;
				}
				if (!initialization.IsDone)
				{
					throw new TimeoutException("Localization initialization timed out: " + code);
				}
				if (initialization.Status != AsyncOperationStatus.Succeeded)
				{
					throw new InvalidOperationException("Localization initialization failed: " + code, initialization.OperationException);
				}
			}
			finally
			{
				if (initialization.IsValid())
				{
					Addressables.Release(initialization);
				}
			}
			Locale locale = LocalizationSettings.AvailableLocales.GetLocale(code);
			if (locale == null)
			{
				throw new InvalidOperationException("Localization locale is missing: " + code);
			}
			AsyncOperationHandle<StringTable> operation = Addressables.ResourceManager.Acquire<StringTable>(LocalizationSettings.StringDatabase.GetTableAsync(GameLocalization._config.tableName, locale));
			try
			{
				float deadline = Time.realtimeSinceStartup + 30f;
				while (request == GameLocalization._request && !operation.IsDone && Time.realtimeSinceStartup < deadline)
				{
					yield return null;
				}
				if (request != GameLocalization._request)
				{
					yield break;
				}
				if (!operation.IsDone)
				{
					throw new TimeoutException("Localization table load timed out: " + code);
				}
				if (operation.Status != AsyncOperationStatus.Succeeded || operation.Result == null || operation.Result.LocaleIdentifier.Code != code)
				{
					throw new InvalidOperationException("Localization table could not be loaded: " + code, operation.OperationException);
				}
				LocalizationSettings.SelectedLocale = locale;
				if (request != GameLocalization._request)
				{
					yield break;
				}
				StringTable result = operation.Result;
				AsyncOperationHandle<StringTable> tableHandle = GameLocalization._tableHandle;
				GameLocalization._tableHandle = operation;
				operation = default(AsyncOperationHandle<StringTable>);
				GameLocalization._table = result;
				GameLocalization._currentCode = code;
				if (tableHandle.IsValid())
				{
					Addressables.Release<StringTable>(tableHandle);
				}
				if (save)
				{
					PlayerPrefs.SetString("Settings.Localization.Language", code);
					PlayerPrefs.Save();
				}
				GameLocalization._loading = false;
				Debug.Log(string.Format("[Localization] Applied request {0}: locale={1}, ", request, code) + string.Format("table={0}, saved={1}; build={2}.", GameLocalization._config.tableName, save, Application.buildGUID));
				GameLocalization.Notify(GameLocalization.LocaleChanged);
				GameLocalization.Notify(GameLocalization.LoadingChanged);
			}
			finally
			{
				if (operation.IsValid())
				{
					Addressables.Release<StringTable>(operation);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600821B RID: 33307 RVA: 0x0035265C File Offset: 0x0035085C
		private static void ResetFailedLoad()
		{
			LocalizationSettings instance = LocalizationSettings.Instance;
			AsyncOperationHandle<LocalizationSettings> initializationOperation = LocalizationSettings.InitializationOperation;
			if (initializationOperation.IsDone && initializationOperation.Status != AsyncOperationStatus.Succeeded)
			{
				((IDisposable)instance).Dispose();
				instance.ResetState();
				return;
			}
			LocalizationSettings.StringDatabase.ReleaseAllTables(null);
		}

		// Token: 0x0600821C RID: 33308 RVA: 0x003526A0 File Offset: 0x003508A0
		private static void Notify(Action listeners)
		{
			if (listeners == null)
			{
				return;
			}
			foreach (Action action in listeners.GetInvocationList())
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
		}

		// Token: 0x0600821D RID: 33309 RVA: 0x003526F0 File Offset: 0x003508F0
		private static void FailSwitch(string message)
		{
			int request = GameLocalization._request;
			GameLocalization._retryLoad = true;
			try
			{
				if (GameLocalization.HasLoadedTable)
				{
					Locale locale = LocalizationSettings.AvailableLocales.GetLocale(GameLocalization._currentCode);
					if (locale != null)
					{
						LocalizationSettings.SelectedLocale = locale;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			finally
			{
				Debug.LogError(message);
				if (request == GameLocalization._request)
				{
					GameLocalization._loading = false;
					GameLocalization.Notify(GameLocalization.LoadingChanged);
				}
			}
		}

		// Token: 0x0600821E RID: 33310 RVA: 0x00352778 File Offset: 0x00350978
		internal static void ReleaseOwnedTable()
		{
			if (GameLocalization._tableHandle.IsValid())
			{
				Addressables.Release<StringTable>(GameLocalization._tableHandle);
			}
			GameLocalization._tableHandle = default(AsyncOperationHandle<StringTable>);
			GameLocalization._table = null;
		}

		// Token: 0x170014AC RID: 5292
		// (get) Token: 0x0600821F RID: 33311 RVA: 0x003527A1 File Offset: 0x003509A1
		public static bool HasLoadedTable
		{
			get
			{
				return GameLocalization._table != null && GameLocalization._tableHandle.IsValid();
			}
		}

		// Token: 0x06008220 RID: 33312 RVA: 0x003527BC File Offset: 0x003509BC
		private static void OnUnityLocaleChanged(Locale locale)
		{
			if (GameLocalization._loading || locale == null || locale.Identifier.Code == GameLocalization._currentCode)
			{
				return;
			}
			AsyncOperationHandle<Locale> selectedLocaleAsync = LocalizationSettings.SelectedLocaleAsync;
			if (selectedLocaleAsync.IsValid() && selectedLocaleAsync.IsDone && selectedLocaleAsync.Result != null && selectedLocaleAsync.Result.Identifier == locale.Identifier)
			{
				GameLocalization.SetLocale(locale.Identifier.Code, false);
			}
		}

		// Token: 0x06008221 RID: 33313 RVA: 0x0035284C File Offset: 0x00350A4C
		private static void BindScene(Scene scene, LoadSceneMode mode)
		{
			if (GameLocalization._config == null)
			{
				return;
			}
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			foreach (GameLocalizationConfig.SceneText sceneText in GameLocalization._config.sceneTexts)
			{
				if (!(sceneText.scene != scene.path))
				{
					Transform transform = null;
					foreach (GameObject gameObject in rootGameObjects)
					{
						if (gameObject.name == sceneText.rootName)
						{
							transform = gameObject.transform;
							break;
						}
					}
					int num = 0;
					while (num < sceneText.childNames.Length && transform != null)
					{
						Transform transform2 = null;
						int num2 = 0;
						foreach (object obj in transform)
						{
							Transform transform3 = (Transform)obj;
							if (transform3.name == sceneText.childNames[num] && num2++ == sceneText.childIndices[num])
							{
								transform2 = transform3;
								break;
							}
						}
						transform = transform2;
						num++;
					}
					if (!(transform == null) && !(transform.name != sceneText.objectName))
					{
						TMP_Text component = transform.GetComponent<TMP_Text>();
						if (component != null && component.text == sceneText.english && component.GetComponent<LocalizedGameText>() == null)
						{
							LocalizedGameText localizedGameText = LocalizedGameText.Bind(component, sceneText.key, sceneText.english, null);
							if (sceneText.fitToRectangle)
							{
								localizedGameText.FitTranslationToRectangle(sceneText.singleLine);
							}
						}
						Text component2 = transform.GetComponent<Text>();
						if (component2 != null && component2.text == sceneText.english && component2.GetComponent<LocalizedLegacyGameText>() == null)
						{
							LocalizedLegacyGameText.Bind(component2, sceneText.key, sceneText.english);
						}
					}
				}
			}
		}

		// Token: 0x06008222 RID: 33314 RVA: 0x00352A60 File Offset: 0x00350C60
		public static string Get(string key, string english)
		{
			if (GameLocalization._currentCode == "en" || GameLocalization._table == null || string.IsNullOrEmpty(key))
			{
				return english;
			}
			StringTableEntry entry = GameLocalization._table.GetEntry(key);
			if (entry != null && !string.IsNullOrEmpty(entry.Value))
			{
				return entry.Value;
			}
			return english;
		}

		// Token: 0x06008223 RID: 33315 RVA: 0x00352AB9 File Offset: 0x00350CB9
		public static bool TryGetSourceKey(string scope, string english, out string key)
		{
			return GameLocalization.SourceKeys.TryGetValue(GameLocalization.SourceId(scope, english), out key);
		}

		// Token: 0x06008224 RID: 33316 RVA: 0x00352AD0 File Offset: 0x00350CD0
		public static string GetSource(string scope, string english)
		{
			string text;
			if (!GameLocalization.TryGetSourceKey(scope, english, out text))
			{
				return english;
			}
			return GameLocalization.Get(text, english);
		}

		// Token: 0x06008225 RID: 33317 RVA: 0x00352AF4 File Offset: 0x00350CF4
		public static string FormatSource(string scope, string english, params object[] arguments)
		{
			string text;
			GameLocalization.TryGetSourceKey(scope, english, out text);
			return GameLocalization.Format(text, english, arguments);
		}

		// Token: 0x06008226 RID: 33318 RVA: 0x00352B14 File Offset: 0x00350D14
		public static string Format(string key, string english, params object[] arguments)
		{
			if (GameLocalization._currentCode != "en" && GameLocalization._table != null && !string.IsNullOrEmpty(key))
			{
				StringTableEntry entry = GameLocalization._table.GetEntry(key);
				if (entry != null && !string.IsNullOrEmpty(entry.Value))
				{
					if (!entry.IsSmart)
					{
						return string.Format(GameLocalization._table.LocaleIdentifier.CultureInfo, entry.Value, arguments);
					}
					return entry.GetLocalizedString(GameLocalization._table.LocaleIdentifier.CultureInfo, arguments, null);
				}
			}
			return string.Format(CultureInfo.CurrentCulture, GameLocalization.Get(key, english), arguments);
		}

		// Token: 0x06008227 RID: 33319 RVA: 0x00352BB8 File Offset: 0x00350DB8
		public static TMP_FontAsset GetFont(TMP_FontAsset original)
		{
			if (GameLocalization._config != null && GameLocalization._currentCode != "en")
			{
				foreach (GameLocalizationConfig.Language language in GameLocalization._config.languages)
				{
					if (language.code == GameLocalization._currentCode && language.font != null)
					{
						return language.font;
					}
				}
			}
			return original;
		}

		// Token: 0x06008228 RID: 33320 RVA: 0x00352C28 File Offset: 0x00350E28
		private static string SourceId(string scope, string english)
		{
			return scope + "\u001f" + english;
		}

		// Token: 0x04008545 RID: 34117
		public const string PreferenceKey = "Settings.Localization.Language";

		// Token: 0x04008546 RID: 34118
		private static readonly Dictionary<string, string> SourceKeys = new Dictionary<string, string>(StringComparer.Ordinal);

		// Token: 0x04008547 RID: 34119
		private static GameLocalizationConfig _config;

		// Token: 0x04008548 RID: 34120
		private static GameLocalizationRunner _runner;

		// Token: 0x04008549 RID: 34121
		private static StringTable _table;

		// Token: 0x0400854A RID: 34122
		private static AsyncOperationHandle<StringTable> _tableHandle;

		// Token: 0x0400854B RID: 34123
		private static string _currentCode = "en";

		// Token: 0x0400854C RID: 34124
		private static string _requestedCode = "en";

		// Token: 0x0400854D RID: 34125
		private static int _request;

		// Token: 0x0400854E RID: 34126
		private static bool _loading;

		// Token: 0x0400854F RID: 34127
		private static bool _retryLoad;

		// Token: 0x04008550 RID: 34128
		private const float LoadTimeoutSeconds = 30f;
	}
}
