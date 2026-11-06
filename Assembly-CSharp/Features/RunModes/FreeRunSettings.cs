using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D75 RID: 3445
	[Serializable]
	public sealed class FreeRunSettings
	{
		// Token: 0x0600655C RID: 25948 RVA: 0x00286B24 File Offset: 0x00284D24
		public FreeRunSettings Clone()
		{
			FreeRunSettings freeRunSettings = new FreeRunSettings();
			freeRunSettings.schemaVersion = this.schemaVersion;
			freeRunSettings.detectionEnabled = this.detectionEnabled;
			freeRunSettings.npcDifficulty = this.npcDifficulty;
			freeRunSettings.homeReturnEnabled = this.homeReturnEnabled;
			freeRunSettings.infiniteStamina = this.infiniteStamina;
			freeRunSettings.lootAmount = this.lootAmount;
			freeRunSettings.useCustomWorldSeed = this.useCustomWorldSeed;
			freeRunSettings.worldSeed = this.worldSeed;
			freeRunSettings.encountersEnabled = this.encountersEnabled;
			freeRunSettings.timeProgressionEnabled = this.timeProgressionEnabled;
			freeRunSettings.variableWeather = this.variableWeather;
			freeRunSettings.startHour = this.startHour;
			freeRunSettings.dayLengthMinutes = this.dayLengthMinutes;
			freeRunSettings.weatherIndex = this.weatherIndex;
			freeRunSettings.weatherPresetName = this.weatherPresetName;
			string[] array = this.allowedWeatherPresetNames;
			freeRunSettings.allowedWeatherPresetNames = (string[])(((array != null) ? array.Clone() : null) ?? Array.Empty<string>());
			freeRunSettings.weatherChangeIntervalMinutes = this.weatherChangeIntervalMinutes;
			freeRunSettings.clothingDurabilityEnabled = this.clothingDurabilityEnabled;
			freeRunSettings.clothingDurationMultiplier = this.clothingDurationMultiplier;
			freeRunSettings.itemDurabilityEnabled = this.itemDurabilityEnabled;
			freeRunSettings.vehicleDurationEnabled = this.vehicleDurationEnabled;
			freeRunSettings.vehicleSpawnMultiplier = this.vehicleSpawnMultiplier;
			freeRunSettings.bodyPaintDurationMode = this.bodyPaintDurationMode;
			freeRunSettings.bodyPaintRainWashEnabled = this.bodyPaintRainWashEnabled;
			freeRunSettings.infiniteBodyPaintPenUses = this.infiniteBodyPaintPenUses;
			freeRunSettings.bodyPaintDurationSeconds = this.bodyPaintDurationSeconds;
			freeRunSettings.itemDurationMultiplier = this.itemDurationMultiplier;
			freeRunSettings.unlimitedDurability = this.unlimitedDurability;
			freeRunSettings.usePresetOutfit = this.usePresetOutfit;
			string[] array2 = this.startingItemIds;
			freeRunSettings.startingItemIds = (string[])(((array2 != null) ? array2.Clone() : null) ?? new string[3]);
			return freeRunSettings;
		}

		// Token: 0x0600655D RID: 25949 RVA: 0x00286CD4 File Offset: 0x00284ED4
		public void Normalize()
		{
			if (this.schemaVersion < 2)
			{
				this.clothingDurabilityEnabled = !this.unlimitedDurability;
				this.itemDurabilityEnabled = !this.unlimitedDurability;
				this.clothingDurationMultiplier = 1f;
				this.itemDurationMultiplier = 1f;
				this.startHour = 12;
				this.dayLengthMinutes = 24f;
				this.weatherChangeIntervalMinutes = 5f;
				this.schemaVersion = 2;
			}
			if (this.schemaVersion < 3)
			{
				this.encountersEnabled = true;
				this.startingItemIds = new string[3];
				this.allowedWeatherPresetNames = Array.Empty<string>();
				this.schemaVersion = 3;
			}
			if (this.schemaVersion < 4)
			{
				this.vehicleDurationEnabled = true;
				this.schemaVersion = 4;
			}
			if (this.schemaVersion < 5)
			{
				this.bodyPaintDurationMode = BodyPaintDurationMode.Authored;
				this.bodyPaintDurationSeconds = 180f;
				this.bodyPaintRainWashEnabled = true;
				this.schemaVersion = 5;
			}
			if (this.schemaVersion < 6)
			{
				this.vehicleSpawnMultiplier = 1f;
				this.schemaVersion = 6;
			}
			if (this.schemaVersion < 7)
			{
				this.infiniteStamina = false;
				this.lootAmount = FreeLootAmount.Normal;
				this.schemaVersion = 7;
			}
			if (this.schemaVersion < 8)
			{
				this.useCustomWorldSeed = false;
				this.worldSeed = 0;
				this.schemaVersion = 8;
			}
			if (this.schemaVersion < 9)
			{
				this.homeReturnEnabled = true;
				this.schemaVersion = 9;
			}
			if (!Enum.IsDefined(typeof(FreeLootAmount), this.lootAmount))
			{
				this.lootAmount = FreeLootAmount.Normal;
			}
			if (!Enum.IsDefined(typeof(FreeNpcDifficulty), this.npcDifficulty))
			{
				this.npcDifficulty = FreeNpcDifficulty.Default;
			}
			this.vehicleSpawnMultiplier = FreeRunSettings.NormalizeVehicleSpawnMultiplier(this.vehicleSpawnMultiplier);
			if (!Enum.IsDefined(typeof(BodyPaintDurationMode), this.bodyPaintDurationMode))
			{
				this.bodyPaintDurationMode = BodyPaintDurationMode.Authored;
			}
			this.bodyPaintDurationSeconds = FreeRunSettings.NormalizeBodyPaintDuration(this.bodyPaintDurationSeconds);
			this.startHour = Mathf.Clamp(this.startHour, 0, 23);
			this.dayLengthMinutes = Mathf.Clamp(this.dayLengthMinutes, 5f, 120f);
			this.weatherIndex = Mathf.Max(0, this.weatherIndex);
			if (string.IsNullOrWhiteSpace(this.weatherPresetName))
			{
				this.weatherPresetName = "SUNNY";
			}
			this.weatherChangeIntervalMinutes = Mathf.Clamp(this.weatherChangeIntervalMinutes, 1f, 20f);
			this.clothingDurationMultiplier = Mathf.Clamp(this.clothingDurationMultiplier, 0.25f, 5f);
			this.itemDurationMultiplier = Mathf.Clamp(this.itemDurationMultiplier, 0.25f, 5f);
			this.unlimitedDurability = !this.clothingDurabilityEnabled && !this.itemDurabilityEnabled;
			string[] array = new string[3];
			if (this.startingItemIds != null)
			{
				for (int i = 0; i < Mathf.Min(array.Length, this.startingItemIds.Length); i++)
				{
					string[] array2 = array;
					int num = i;
					string text = this.startingItemIds[i];
					array2[num] = ((text != null) ? text.Trim() : null) ?? string.Empty;
				}
			}
			this.startingItemIds = array;
			List<string> list = new List<string>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			if (this.allowedWeatherPresetNames != null)
			{
				foreach (string text2 in this.allowedWeatherPresetNames)
				{
					string text3 = ((text2 != null) ? text2.Trim() : null);
					string text4 = DynamicWeatherPolicy.NormalizeName(text3);
					if (!string.IsNullOrEmpty(text4) && hashSet.Add(text4))
					{
						list.Add(text3);
					}
				}
			}
			this.allowedWeatherPresetNames = list.ToArray();
		}

		// Token: 0x0600655E RID: 25950 RVA: 0x00287038 File Offset: 0x00285238
		internal static float NormalizeVehicleSpawnMultiplier(float multiplier)
		{
			if (!float.IsNaN(multiplier) && !float.IsInfinity(multiplier))
			{
				return Mathf.Clamp(multiplier, 0f, 5f);
			}
			return 1f;
		}

		// Token: 0x0600655F RID: 25951 RVA: 0x00287060 File Offset: 0x00285260
		internal static float NormalizeBodyPaintDuration(float seconds)
		{
			if (!float.IsNaN(seconds) && !float.IsInfinity(seconds))
			{
				return Mathf.Clamp(seconds, 30f, 1800f);
			}
			return 180f;
		}

		// Token: 0x040067C8 RID: 26568
		public const float MinimumBodyPaintDurationSeconds = 30f;

		// Token: 0x040067C9 RID: 26569
		public const float MaximumBodyPaintDurationSeconds = 1800f;

		// Token: 0x040067CA RID: 26570
		public const float DefaultBodyPaintDurationSeconds = 180f;

		// Token: 0x040067CB RID: 26571
		public const float MaximumVehicleSpawnMultiplier = 5f;

		// Token: 0x040067CC RID: 26572
		public int schemaVersion = 9;

		// Token: 0x040067CD RID: 26573
		public bool detectionEnabled = true;

		// Token: 0x040067CE RID: 26574
		public FreeNpcDifficulty npcDifficulty;

		// Token: 0x040067CF RID: 26575
		public bool homeReturnEnabled = true;

		// Token: 0x040067D0 RID: 26576
		public bool infiniteStamina;

		// Token: 0x040067D1 RID: 26577
		public FreeLootAmount lootAmount = FreeLootAmount.Normal;

		// Token: 0x040067D2 RID: 26578
		public bool useCustomWorldSeed;

		// Token: 0x040067D3 RID: 26579
		public int worldSeed;

		// Token: 0x040067D4 RID: 26580
		public bool encountersEnabled = true;

		// Token: 0x040067D5 RID: 26581
		public bool timeProgressionEnabled = true;

		// Token: 0x040067D6 RID: 26582
		public bool variableWeather = true;

		// Token: 0x040067D7 RID: 26583
		[Range(0f, 23f)]
		public int startHour = 12;

		// Token: 0x040067D8 RID: 26584
		[Range(5f, 120f)]
		public float dayLengthMinutes = 24f;

		// Token: 0x040067D9 RID: 26585
		public int weatherIndex;

		// Token: 0x040067DA RID: 26586
		public string weatherPresetName = "SUNNY";

		// Token: 0x040067DB RID: 26587
		public string[] allowedWeatherPresetNames = Array.Empty<string>();

		// Token: 0x040067DC RID: 26588
		[Range(1f, 20f)]
		public float weatherChangeIntervalMinutes = 5f;

		// Token: 0x040067DD RID: 26589
		public bool clothingDurabilityEnabled = true;

		// Token: 0x040067DE RID: 26590
		[Range(0.25f, 5f)]
		public float clothingDurationMultiplier = 1f;

		// Token: 0x040067DF RID: 26591
		public bool itemDurabilityEnabled = true;

		// Token: 0x040067E0 RID: 26592
		public bool vehicleDurationEnabled = true;

		// Token: 0x040067E1 RID: 26593
		[Range(0f, 5f)]
		public float vehicleSpawnMultiplier = 1f;

		// Token: 0x040067E2 RID: 26594
		public BodyPaintDurationMode bodyPaintDurationMode;

		// Token: 0x040067E3 RID: 26595
		public bool bodyPaintRainWashEnabled = true;

		// Token: 0x040067E4 RID: 26596
		public bool infiniteBodyPaintPenUses;

		// Token: 0x040067E5 RID: 26597
		[Range(30f, 1800f)]
		public float bodyPaintDurationSeconds = 180f;

		// Token: 0x040067E6 RID: 26598
		[Range(0.25f, 5f)]
		public float itemDurationMultiplier = 1f;

		// Token: 0x040067E7 RID: 26599
		public bool unlimitedDurability;

		// Token: 0x040067E8 RID: 26600
		public bool usePresetOutfit = true;

		// Token: 0x040067E9 RID: 26601
		public string[] startingItemIds = new string[3];
	}
}
