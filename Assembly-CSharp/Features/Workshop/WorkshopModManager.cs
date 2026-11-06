using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Features.RunModes;
using ProjectExhibitionist.ModApi;
using UnityEngine;

namespace Features.Workshop
{
	// Token: 0x02000B46 RID: 2886
	public static class WorkshopModManager
	{
		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x060051F0 RID: 20976 RVA: 0x001FB866 File Offset: 0x001F9A66
		public static bool HasModdedSession
		{
			get
			{
				return WorkshopModManager._processContaminated;
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x060051F1 RID: 20977 RVA: 0x001FB870 File Offset: 0x001F9A70
		public static bool RequiresRestart
		{
			get
			{
				WorkshopModManager.EnsureSettings();
				if (WorkshopModManager.Enabled.Except<ulong>(WorkshopModManager.Loaded).Any<ulong>() || WorkshopModManager.Attempted.Except<ulong>(WorkshopModManager.Enabled).Any<ulong>())
				{
					return true;
				}
				if (WorkshopService.IsAvailable)
				{
					foreach (ulong num in WorkshopModManager.Loaded)
					{
						WorkshopInstalledItem workshopInstalledItem;
						WorkshopModManager.Package package;
						if (!WorkshopService.TryGetInstalledItem(num, out workshopInstalledItem) || !WorkshopModManager.Packages.TryGetValue(num, out package) || workshopInstalledItem.UpdatedAt != package.Item.UpdatedAt || workshopInstalledItem.SizeBytes != package.Item.SizeBytes)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x060051F2 RID: 20978 RVA: 0x0002AE8C File Offset: 0x0002908C
		public static bool SupportsCodeMods
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x001FB940 File Offset: 0x001F9B40
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetSession()
		{
			AppDomain.CurrentDomain.AssemblyResolve -= WorkshopModManager.ResolveDependency;
			WorkshopModManager.Enabled.Clear();
			WorkshopModManager.Loaded.Clear();
			WorkshopModManager.Attempted.Clear();
			WorkshopModManager.Failures.Clear();
			WorkshopModManager.Packages.Clear();
			WorkshopModManager.AssemblyOwners.Clear();
			WorkshopModManager.Instances.Clear();
			WorkshopModManager._settingsAccount = null;
			WorkshopModManager._settingsError = null;
			WorkshopModManager._started = false;
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x001FB9BC File Offset: 0x001F9BBC
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Bootstrap()
		{
			if (WorkshopModManager._started)
			{
				return;
			}
			WorkshopModManager._started = true;
			if (Environment.GetCommandLineArgs().Any<string>((string argument) => string.Equals(argument, "--no-workshop-mods", StringComparison.OrdinalIgnoreCase)))
			{
				return;
			}
			if (!WorkshopModManager.SupportsCodeMods || !WorkshopService.TryConnect())
			{
				return;
			}
			WorkshopModManager.EnsureSettings();
			if (WorkshopModManager.Enabled.Count == 0 || !string.IsNullOrEmpty(WorkshopModManager._settingsError))
			{
				return;
			}
			foreach (WorkshopInstalledItem workshopInstalledItem in WorkshopService.GetInstalledSubscriptions())
			{
				if (WorkshopModManager.Enabled.Contains(workshopInstalledItem.Id))
				{
					WorkshopModManifest workshopModManifest;
					string text;
					string text2;
					if (!WorkshopModManifest.TryRead(workshopInstalledItem.Folder, out workshopModManifest, out text, out text2))
					{
						WorkshopModManager.Failures[workshopInstalledItem.Id] = text2;
					}
					else
					{
						WorkshopModManager.Packages[workshopInstalledItem.Id] = new WorkshopModManager.Package
						{
							Item = workshopInstalledItem,
							Manifest = workshopModManifest,
							AssemblyPath = text
						};
					}
				}
			}
			Dictionary<ulong, string> dictionary;
			List<ulong> list = WorkshopModManager.BuildLoadOrder(WorkshopModManager.Packages.ToDictionary<KeyValuePair<ulong, WorkshopModManager.Package>, ulong, WorkshopModManifest>((KeyValuePair<ulong, WorkshopModManager.Package> pair) => pair.Key, (KeyValuePair<ulong, WorkshopModManager.Package> pair) => pair.Value.Manifest), out dictionary);
			foreach (KeyValuePair<ulong, string> keyValuePair in dictionary)
			{
				WorkshopModManager.Failures[keyValuePair.Key] = keyValuePair.Value;
			}
			AppDomain.CurrentDomain.AssemblyResolve += WorkshopModManager.ResolveDependency;
			using (List<ulong>.Enumerator enumerator3 = list.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					ulong id = enumerator3.Current;
					WorkshopModManager.Package package = WorkshopModManager.Packages[id];
					if ((package.Manifest.requiredWorkshopItems ?? Array.Empty<string>()).Any<string>((string dependency) => !WorkshopModManager.Loaded.Contains(ulong.Parse(dependency))))
					{
						WorkshopModManager.Failures[id] = "A required mod did not load successfully.";
					}
					else
					{
						WorkshopModManager.MarkSessionModded();
						WorkshopModManager.Attempted.Add(id);
						try
						{
							AssemblyName identity = AssemblyName.GetAssemblyName(package.AssemblyPath);
							if (AppDomain.CurrentDomain.GetAssemblies().Any<Assembly>((Assembly existing) => string.Equals(existing.GetName().Name, identity.Name, StringComparison.OrdinalIgnoreCase)))
							{
								throw new InvalidDataException("The entry assembly name is already loaded. Use a unique mod assembly name and restart the game.");
							}
							Assembly assembly = Assembly.LoadFrom(package.AssemblyPath);
							WorkshopModManager.AssemblyOwners[assembly] = id;
							Type type = assembly.GetType(package.Manifest.entryType, true, false);
							if (!typeof(IWorkshopMod).IsAssignableFrom(type) || type.IsAbstract || type.GetConstructor(Type.EmptyTypes) == null)
							{
								throw new InvalidDataException("entryType must implement IWorkshopMod with a public parameterless constructor.");
							}
							IWorkshopMod workshopMod = (IWorkshopMod)Activator.CreateInstance(type);
							string text3 = Path.Combine(Application.persistentDataPath, "Workshop", "ModData", id.ToString());
							Directory.CreateDirectory(text3);
							workshopMod.OnLoad(new WorkshopModContext(id, package.Item.Folder, text3, Application.version, delegate(string message)
							{
								Debug.Log("[Workshop/" + id.ToString() + "] " + message);
							}));
							WorkshopModManager.Instances.Add(workshopMod);
							WorkshopModManager.Loaded.Add(id);
							Debug.Log(string.Concat(new string[]
							{
								"[Workshop] Loaded ",
								package.Manifest.name,
								" ",
								package.Manifest.version,
								" (",
								id.ToString(),
								")."
							}));
						}
						catch (Exception ex)
						{
							WorkshopModManager.Failures[id] = ex.GetBaseException().Message;
							Debug.LogWarning("[Workshop] Mod " + id.ToString() + " failed to load: " + WorkshopModManager.Failures[id]);
						}
					}
				}
			}
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x001FBE7C File Offset: 0x001FA07C
		internal static void MarkSessionModded()
		{
			bool isRankedRun = RunModeService.IsRankedRun;
			WorkshopModManager._processContaminated = true;
			if (isRankedRun)
			{
				RunModeService.DisqualifyLeaderboardRun(RankedPlatformPolicy.SubmissionDisabledReason);
			}
		}

		// Token: 0x060051F6 RID: 20982 RVA: 0x001FBE98 File Offset: 0x001FA098
		public static WorkshopModStatus GetStatus(ulong id)
		{
			WorkshopModManager.EnsureSettings();
			WorkshopModStatus workshopModStatus = new WorkshopModStatus
			{
				Enabled = WorkshopModManager.Enabled.Contains(id),
				Loaded = WorkshopModManager.Loaded.Contains(id)
			};
			if (workshopModStatus.Loaded)
			{
				workshopModStatus.Supported = true;
				return WorkshopModManager.SetStatus(workshopModStatus, "loaded", "Active this session");
			}
			if (!WorkshopModManager.SupportsCodeMods)
			{
				return WorkshopModManager.SetStatus(workshopModStatus, "runtime", "Code mods require the Mono version of the game.");
			}
			string text;
			if (WorkshopModManager.Failures.TryGetValue(id, out text))
			{
				workshopModStatus.Detail = text;
				return WorkshopModManager.SetStatus(workshopModStatus, "failed", "Could not load this mod. Check its compatibility.");
			}
			WorkshopInstalledItem workshopInstalledItem;
			if (!WorkshopService.TryGetInstalledItem(id, out workshopInstalledItem))
			{
				return WorkshopModManager.SetStatus(workshopModStatus, "missing", "Download this mod before enabling it.");
			}
			WorkshopModManifest workshopModManifest;
			string text2;
			string text3;
			if (!WorkshopModManifest.TryRead(workshopInstalledItem.Folder, out workshopModManifest, out text2, out text3))
			{
				workshopModStatus.Detail = text3;
				return WorkshopModManager.SetStatus(workshopModStatus, "invalid", "This mod does not contain a supported package.");
			}
			workshopModStatus.Supported = true;
			if (!workshopModStatus.Enabled)
			{
				return WorkshopModManager.SetStatus(workshopModStatus, "ready", "Ready to enable");
			}
			return WorkshopModManager.SetStatus(workshopModStatus, "enabled", "Enabled · restart required");
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x001FBFA7 File Offset: 0x001FA1A7
		private static WorkshopModStatus SetStatus(WorkshopModStatus status, string key, string fallback)
		{
			status.StatusKey = "workshop.mod." + key;
			status.StatusFallback = fallback;
			return status;
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x001FBFC4 File Offset: 0x001FA1C4
		public static bool SetEnabled(ulong id, bool enabled, out string error)
		{
			error = string.Empty;
			WorkshopModManager.EnsureSettings();
			if (id == 0UL || string.IsNullOrEmpty(WorkshopModManager._settingsAccount) || !string.IsNullOrEmpty(WorkshopModManager._settingsError))
			{
				error = WorkshopModManager._settingsError ?? "Connect to Steam before changing mod activation.";
				return false;
			}
			if (enabled && !WorkshopModManager.GetStatus(id).Supported)
			{
				error = "Only compatible, fully installed Workshop packages can be enabled.";
				return false;
			}
			HashSet<ulong> hashSet = new HashSet<ulong>(WorkshopModManager.Enabled);
			if (enabled)
			{
				hashSet.Add(id);
			}
			else
			{
				hashSet.Remove(id);
			}
			bool flag;
			try
			{
				string text = WorkshopModManager.SettingsPath(WorkshopModManager._settingsAccount);
				Directory.CreateDirectory(Path.GetDirectoryName(text));
				string text2 = text + ".tmp";
				string text3 = text2;
				WorkshopModManager.ActivationSettings activationSettings = new WorkshopModManager.ActivationSettings();
				activationSettings.enabled = (from value in hashSet
					orderby value
					select value.ToString()).ToArray<string>();
				File.WriteAllText(text3, JsonUtility.ToJson(activationSettings, true));
				if (File.Exists(text))
				{
					File.Replace(text2, text, null);
				}
				else
				{
					File.Move(text2, text);
				}
				WorkshopModManager.Enabled.Clear();
				WorkshopModManager.Enabled.UnionWith(hashSet);
				flag = true;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				error = ex.Message;
				flag = false;
			}
			return flag;
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x001FC148 File Offset: 0x001FA348
		private static string SettingsPath(string account)
		{
			return Path.Combine(Application.persistentDataPath, "Workshop", "enabled-" + account + ".json");
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x001FC16C File Offset: 0x001FA36C
		private static void EnsureSettings()
		{
			string currentAccountId = RankedLeaderboardService.CurrentAccountId;
			ulong num;
			if (string.IsNullOrEmpty(currentAccountId) || !ulong.TryParse(currentAccountId, out num))
			{
				return;
			}
			if (string.Equals(currentAccountId, WorkshopModManager._settingsAccount, StringComparison.Ordinal))
			{
				return;
			}
			WorkshopModManager._settingsAccount = currentAccountId;
			WorkshopModManager._settingsError = null;
			WorkshopModManager.Enabled.Clear();
			try
			{
				string text = WorkshopModManager.SettingsPath(currentAccountId);
				if (File.Exists(text))
				{
					if (new FileInfo(text).Length > 65536L)
					{
						throw new InvalidDataException("The activation settings file is too large.");
					}
					WorkshopModManager.ActivationSettings activationSettings = JsonUtility.FromJson<WorkshopModManager.ActivationSettings>(File.ReadAllText(text));
					if (activationSettings == null)
					{
						throw new InvalidDataException("The activation settings file is invalid.");
					}
					string[] array = activationSettings.enabled ?? Array.Empty<string>();
					for (int i = 0; i < array.Length; i++)
					{
						ulong num2;
						if (ulong.TryParse(array[i], out num2) && num2 != 0UL)
						{
							WorkshopModManager.Enabled.Add(num2);
						}
					}
				}
			}
			catch (Exception ex) when (ex is IOException || ex is InvalidDataException || ex is ArgumentException || ex is UnauthorizedAccessException)
			{
				WorkshopModManager._settingsError = ex.Message;
				Debug.LogWarning("[Workshop] Cannot read activation settings: " + WorkshopModManager._settingsError);
			}
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x001FC2B4 File Offset: 0x001FA4B4
		public static List<ulong> BuildLoadOrder(IReadOnlyDictionary<ulong, WorkshopModManifest> manifests, out Dictionary<ulong, string> errors)
		{
			List<ulong> result = new List<ulong>();
			Dictionary<ulong, string> failures = new Dictionary<ulong, string>();
			HashSet<ulong> visited = new HashSet<ulong>();
			HashSet<ulong> visiting = new HashSet<ulong>();
			Func<ulong, bool> visit = null;
			visit = delegate(ulong id)
			{
				if (failures.ContainsKey(id))
				{
					return false;
				}
				if (visited.Contains(id))
				{
					return true;
				}
				WorkshopModManifest workshopModManifest;
				if (!manifests.TryGetValue(id, out workshopModManifest))
				{
					return false;
				}
				if (!visiting.Add(id))
				{
					failures[id] = "Cyclic mod dependency.";
					return false;
				}
				foreach (string text in workshopModManifest.requiredWorkshopItems ?? Array.Empty<string>())
				{
					ulong num2;
					if (!ulong.TryParse(text, out num2) || !visit(num2))
					{
						failures[id] = "A required mod is missing, disabled, invalid or cyclic: " + text;
						visiting.Remove(id);
						return false;
					}
				}
				visiting.Remove(id);
				visited.Add(id);
				result.Add(id);
				return true;
			};
			foreach (ulong num in manifests.Keys.OrderBy<ulong, ulong>((ulong value) => value))
			{
				visit(num);
			}
			errors = failures;
			return result;
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x001FC390 File Offset: 0x001FA590
		private static Assembly ResolveDependency(object sender, ResolveEventArgs args)
		{
			ulong num;
			WorkshopModManager.Package package;
			if (args.RequestingAssembly == null || !WorkshopModManager.AssemblyOwners.TryGetValue(args.RequestingAssembly, out num) || !WorkshopModManager.Packages.TryGetValue(num, out package))
			{
				return null;
			}
			AssemblyName assemblyName = new AssemblyName(args.Name);
			if (string.IsNullOrEmpty(assemblyName.Name) || assemblyName.Name.IndexOfAny(new char[] { '/', '\\', ':' }) >= 0)
			{
				return null;
			}
			string directoryName = Path.GetDirectoryName(package.Manifest.entryAssembly.Replace('\\', '/'));
			foreach (string text in new string[]
			{
				Path.Combine(directoryName ?? string.Empty, assemblyName.Name + ".dll"),
				"lib/" + assemblyName.Name + ".dll"
			})
			{
				try
				{
					string text2 = WorkshopModManifest.ResolveContentPath(package.Item.Folder, text);
					if (File.Exists(text2) && AssemblyName.ReferenceMatchesDefinition(assemblyName, AssemblyName.GetAssemblyName(text2)))
					{
						Assembly assembly = Assembly.LoadFrom(text2);
						WorkshopModManager.AssemblyOwners[assembly] = num;
						return assembly;
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[Workshop] Dependency could not be loaded: " + ex.GetBaseException().Message);
					return null;
				}
			}
			return null;
		}

		// Token: 0x0400528A RID: 21130
		private static readonly HashSet<ulong> Enabled = new HashSet<ulong>();

		// Token: 0x0400528B RID: 21131
		private static readonly HashSet<ulong> Loaded = new HashSet<ulong>();

		// Token: 0x0400528C RID: 21132
		private static readonly HashSet<ulong> Attempted = new HashSet<ulong>();

		// Token: 0x0400528D RID: 21133
		private static readonly Dictionary<ulong, string> Failures = new Dictionary<ulong, string>();

		// Token: 0x0400528E RID: 21134
		private static readonly Dictionary<ulong, WorkshopModManager.Package> Packages = new Dictionary<ulong, WorkshopModManager.Package>();

		// Token: 0x0400528F RID: 21135
		private static readonly Dictionary<Assembly, ulong> AssemblyOwners = new Dictionary<Assembly, ulong>();

		// Token: 0x04005290 RID: 21136
		private static readonly List<IWorkshopMod> Instances = new List<IWorkshopMod>();

		// Token: 0x04005291 RID: 21137
		private static string _settingsAccount;

		// Token: 0x04005292 RID: 21138
		private static string _settingsError;

		// Token: 0x04005293 RID: 21139
		private static bool _started;

		// Token: 0x04005294 RID: 21140
		private static bool _processContaminated;

		// Token: 0x02000B47 RID: 2887
		[Serializable]
		private sealed class ActivationSettings
		{
			// Token: 0x04005295 RID: 21141
			public string[] enabled = Array.Empty<string>();
		}

		// Token: 0x02000B48 RID: 2888
		private sealed class Package
		{
			// Token: 0x04005296 RID: 21142
			public WorkshopInstalledItem Item;

			// Token: 0x04005297 RID: 21143
			public WorkshopModManifest Manifest;

			// Token: 0x04005298 RID: 21144
			public string AssemblyPath;
		}
	}
}
