using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000084 RID: 132
public static class MapResidencyPilotSelection
{
	// Token: 0x1700015E RID: 350
	// (get) Token: 0x060005DA RID: 1498 RVA: 0x0002ABE9 File Offset: 0x00028DE9
	public static bool UniversityEnabled
	{
		get
		{
			return MapResidencyPilotSelection.IsFamilyEnabled("university");
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x060005DB RID: 1499 RVA: 0x0002ABF5 File Offset: 0x00028DF5
	public static string Mode
	{
		get
		{
			string text;
			if ((text = MapResidencyPilotSelection._mode) == null)
			{
				text = (MapResidencyPilotSelection._mode = MapResidencyPilotSelection.ParseMode(Environment.GetCommandLineArgs()));
			}
			return text;
		}
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0002AC10 File Offset: 0x00028E10
	public static bool IsFamilyEnabled(string family)
	{
		return MapResidencyPilotSelection.IsFamilyEnabled(family, MapResidencyPilotSelection.Mode);
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0002AC1D File Offset: 0x00028E1D
	public static bool IsFamilyEnabled(string family, string mode)
	{
		return PoiResidencyCatalog.IsFamily(family) && (mode == "all" || mode == family);
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0002AC3F File Offset: 0x00028E3F
	public static MapResidencyPilotSelection.IGameplayManifestRequest EnsureGameplayManifest(string address)
	{
		if (MapResidencyPilotSelection._gameplayDefinitions == null)
		{
			MapResidencyPilotSelection._gameplayDefinitions = new MapResidencyPilotSelection.GameplayManifestCache((string key) => new MapResidencyPilotSelection.ResourcesManifestRequest(key));
		}
		return MapResidencyPilotSelection._gameplayDefinitions.GetOrStart(address);
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0002AC7C File Offset: 0x00028E7C
	public static string GameplayResourceKey(string address)
	{
		PoiResidencyCatalog.Entry entry;
		if (!PoiResidencyCatalog.TryGetByAddress(address, out entry))
		{
			throw new ArgumentException("Unknown POI payload address.", "address");
		}
		return entry.manifestResourceKey;
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0002ACA9 File Offset: 0x00028EA9
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetSelection()
	{
		MapResidencyPilotSelection._mode = null;
		MapResidencyPilotSelection._gameplayDefinitions = null;
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0002ACB7 File Offset: 0x00028EB7
	public static bool ParseUniversityEnabled(string[] arguments)
	{
		return MapResidencyPilotSelection.IsFamilyEnabled("university", MapResidencyPilotSelection.ParseMode(arguments));
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0002ACCC File Offset: 0x00028ECC
	public static string ParseMode(string[] arguments)
	{
		string text = "off";
		bool flag = false;
		if (arguments == null)
		{
			return text;
		}
		int i = 0;
		while (i < arguments.Length)
		{
			string text2;
			if (string.Equals(arguments[i], "-mapResidencyPilot", StringComparison.Ordinal))
			{
				if (++i >= arguments.Length)
				{
					throw new ArgumentException("Missing value for -mapResidencyPilot");
				}
				text2 = arguments[i];
				goto IL_0069;
			}
			else if (arguments[i] != null && arguments[i].StartsWith("-mapResidencyPilot=", StringComparison.Ordinal))
			{
				text2 = arguments[i].Substring("-mapResidencyPilot".Length + 1);
				goto IL_0069;
			}
			IL_00AA:
			i++;
			continue;
			IL_0069:
			if (flag)
			{
				throw new ArgumentException("Duplicate -mapResidencyPilot");
			}
			flag = true;
			if (text2 == "off" || text2 == "all" || PoiResidencyCatalog.IsFamily(text2))
			{
				text = text2;
				goto IL_00AA;
			}
			throw new ArgumentException("Expected off, all or an exact POI family for -mapResidencyPilot");
		}
		return text;
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0002AD91 File Offset: 0x00028F91
	public static bool TryResolveAddress(string resourcePath, out string address)
	{
		return MapResidencyPilotSelection.TryResolveAddress(resourcePath, MapResidencyPilotSelection.Mode, out address);
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0002AD9F File Offset: 0x00028F9F
	public static bool TryResolveAddress(string resourcePath, bool universityEnabled, out string address)
	{
		return MapResidencyPilotSelection.TryResolveAddress(resourcePath, universityEnabled ? "university" : "off", out address);
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0002ADB8 File Offset: 0x00028FB8
	public static bool TryResolveAddress(string resourcePath, string mode, out string address)
	{
		address = null;
		PoiResidencyCatalog.Entry entry;
		if (!PoiResidencyCatalog.TryGetByResourcePath(resourcePath, out entry) || !MapResidencyPilotSelection.IsFamilyEnabled(entry.family, mode))
		{
			return false;
		}
		address = entry.address;
		return true;
	}

	// Token: 0x04000480 RID: 1152
	public const string ArgumentName = "-mapResidencyPilot";

	// Token: 0x04000481 RID: 1153
	public const string AddressPrefix = "map-residency/university/";

	// Token: 0x04000482 RID: 1154
	public const string GameplayResourcePrefix = "MapResidencyGameplay/university/";

	// Token: 0x04000483 RID: 1155
	private static string _mode;

	// Token: 0x04000484 RID: 1156
	private static MapResidencyPilotSelection.GameplayManifestCache _gameplayDefinitions;

	// Token: 0x02000085 RID: 133
	public interface IGameplayManifestRequest
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060005E6 RID: 1510
		bool IsDone { get; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060005E7 RID: 1511
		UniversityGameplayManifest Asset { get; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060005E8 RID: 1512
		string Error { get; }
	}

	// Token: 0x02000086 RID: 134
	public sealed class GameplayManifestCache
	{
		// Token: 0x060005E9 RID: 1513 RVA: 0x0002ADEB File Offset: 0x00028FEB
		public GameplayManifestCache(Func<string, MapResidencyPilotSelection.IGameplayManifestRequest> load)
		{
			if (load == null)
			{
				throw new ArgumentNullException("load");
			}
			this._load = load;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0002AE1C File Offset: 0x0002901C
		public MapResidencyPilotSelection.IGameplayManifestRequest GetOrStart(string address)
		{
			string text = MapResidencyPilotSelection.GameplayResourceKey(address);
			MapResidencyPilotSelection.IGameplayManifestRequest gameplayManifestRequest;
			if (this._requests.TryGetValue(text, out gameplayManifestRequest))
			{
				return gameplayManifestRequest;
			}
			MapResidencyPilotSelection.IGameplayManifestRequest gameplayManifestRequest2;
			try
			{
				gameplayManifestRequest2 = this._load(text) ?? new MapResidencyPilotSelection.FailedManifestRequest("Manifest loader returned no request.");
			}
			catch (Exception ex)
			{
				gameplayManifestRequest2 = new MapResidencyPilotSelection.FailedManifestRequest(ex.Message);
			}
			this._requests.Add(text, gameplayManifestRequest2);
			return gameplayManifestRequest2;
		}

		// Token: 0x04000485 RID: 1157
		private readonly Func<string, MapResidencyPilotSelection.IGameplayManifestRequest> _load;

		// Token: 0x04000486 RID: 1158
		private readonly Dictionary<string, MapResidencyPilotSelection.IGameplayManifestRequest> _requests = new Dictionary<string, MapResidencyPilotSelection.IGameplayManifestRequest>(StringComparer.Ordinal);
	}

	// Token: 0x02000087 RID: 135
	private sealed class FailedManifestRequest : MapResidencyPilotSelection.IGameplayManifestRequest
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0002AE8C File Offset: 0x0002908C
		public bool IsDone
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x0002AE8F File Offset: 0x0002908F
		public UniversityGameplayManifest Asset
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0002AE92 File Offset: 0x00029092
		public string Error { get; }

		// Token: 0x060005EE RID: 1518 RVA: 0x0002AE9A File Offset: 0x0002909A
		public FailedManifestRequest(string error)
		{
			this.Error = error;
		}
	}

	// Token: 0x02000088 RID: 136
	private sealed class ResourcesManifestRequest : MapResidencyPilotSelection.IGameplayManifestRequest
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0002AEA9 File Offset: 0x000290A9
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x0002AEB1 File Offset: 0x000290B1
		public bool IsDone { get; private set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0002AEBA File Offset: 0x000290BA
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x0002AEC2 File Offset: 0x000290C2
		public UniversityGameplayManifest Asset { get; private set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x0002AECB File Offset: 0x000290CB
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x0002AED3 File Offset: 0x000290D3
		public string Error { get; private set; }

		// Token: 0x060005F5 RID: 1525 RVA: 0x0002AEDC File Offset: 0x000290DC
		public ResourcesManifestRequest(string key)
		{
			this._key = key;
			HeavyZoneStreamingProxy.BeginSharedPayloadLoad();
			try
			{
				this._request = Resources.LoadAsync<UniversityGameplayManifest>(key);
				if (this._request.isDone)
				{
					this.Complete(this._request);
				}
				else
				{
					this._request.completed += this.Complete;
				}
			}
			catch
			{
				HeavyZoneStreamingProxy.EndSharedPayloadLoad();
				throw;
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0002AF54 File Offset: 0x00029154
		private void Complete(AsyncOperation operation)
		{
			if (this.IsDone)
			{
				return;
			}
			try
			{
				this.Asset = this._request.asset as UniversityGameplayManifest;
				if (this.Asset == null)
				{
					this.Error = "Original gameplay manifest missing: " + this._key;
				}
			}
			catch (Exception ex)
			{
				this.Error = ex.Message;
			}
			finally
			{
				this.IsDone = true;
				this._request = null;
				HeavyZoneStreamingProxy.EndSharedPayloadLoad();
			}
		}

		// Token: 0x0400048B RID: 1163
		private ResourceRequest _request;

		// Token: 0x0400048C RID: 1164
		private readonly string _key;
	}
}
