using System;
using Features.CielVoice;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x0200103A RID: 4154
	public static class GameSettingsService
	{
		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x06007DEC RID: 32236 RVA: 0x00339008 File Offset: 0x00337208
		// (remove) Token: 0x06007DED RID: 32237 RVA: 0x0033903C File Offset: 0x0033723C
		public static event Action SettingsChanged;

		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x06007DEE RID: 32238 RVA: 0x0033906F File Offset: 0x0033726F
		public static float MasterVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Settings.Audio.Master", 0.9f);
			}
		}

		// Token: 0x1700143B RID: 5179
		// (get) Token: 0x06007DEF RID: 32239 RVA: 0x00339080 File Offset: 0x00337280
		public static float HeartbeatVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Settings.Audio.Heartbeat", 0.9f);
			}
		}

		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x06007DF0 RID: 32240 RVA: 0x00339091 File Offset: 0x00337291
		public static float MusicVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Settings.Audio.Music", 0.72f);
			}
		}

		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x06007DF1 RID: 32241 RVA: 0x003390A2 File Offset: 0x003372A2
		public static float SfxVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Settings.Audio.SFX", 0.82f);
			}
		}

		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x06007DF2 RID: 32242 RVA: 0x003390B3 File Offset: 0x003372B3
		public static float CielVoiceVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Settings.Audio.CielVoice", 1f);
			}
		}

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x06007DF3 RID: 32243 RVA: 0x003390C4 File Offset: 0x003372C4
		public static float NpcReactionVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Settings.Audio.NPCReaction", 1f);
			}
		}

		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x06007DF4 RID: 32244 RVA: 0x003390D5 File Offset: 0x003372D5
		public static float VoiceVolume
		{
			get
			{
				return GameSettingsService.CielVoiceVolume;
			}
		}

		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x06007DF5 RID: 32245 RVA: 0x003390DC File Offset: 0x003372DC
		public static bool Muted
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Audio.Muted", 0) != 0;
			}
		}

		// Token: 0x17001442 RID: 5186
		// (get) Token: 0x06007DF6 RID: 32246 RVA: 0x003390EC File Offset: 0x003372EC
		public static float LookSensitivity
		{
			get
			{
				return PlayerPrefs.GetFloat("Settings.Gameplay.Sensitivity", 1f);
			}
		}

		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x06007DF7 RID: 32247 RVA: 0x003390FD File Offset: 0x003372FD
		public static bool InvertMouseX
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.InvertMouseX", 0) != 0;
			}
		}

		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x06007DF8 RID: 32248 RVA: 0x0033910D File Offset: 0x0033730D
		public static bool InvertMouseY
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.InvertMouseY", 0) != 0;
			}
		}

		// Token: 0x06007DF9 RID: 32249 RVA: 0x0033911D File Offset: 0x0033731D
		public static Vector2 ApplyMouseLookInversion(Vector2 delta)
		{
			return new Vector2(GameSettingsService.InvertMouseX ? (-delta.x) : delta.x, GameSettingsService.InvertMouseY ? (-delta.y) : delta.y);
		}

		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x06007DFA RID: 32250 RVA: 0x00339150 File Offset: 0x00337350
		public static bool CielVoiceSubtitles
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.CielVoiceSubtitles", PlayerPrefs.GetInt("Settings.Gameplay.Subtitles", 1)) != 0;
			}
		}

		// Token: 0x17001446 RID: 5190
		// (get) Token: 0x06007DFB RID: 32251 RVA: 0x0033916A File Offset: 0x0033736A
		public static bool Subtitles
		{
			get
			{
				return GameSettingsService.CielVoiceSubtitles;
			}
		}

		// Token: 0x17001447 RID: 5191
		// (get) Token: 0x06007DFC RID: 32252 RVA: 0x00339171 File Offset: 0x00337371
		public static bool CameraShake
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.CameraShake", 1) != 0;
			}
		}

		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x06007DFD RID: 32253 RVA: 0x00339181 File Offset: 0x00337381
		public static bool GarbageMudBodyPaint
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.GarbageMudBodyPaint", 1) != 0;
			}
		}

		// Token: 0x17001449 RID: 5193
		// (get) Token: 0x06007DFE RID: 32254 RVA: 0x00339191 File Offset: 0x00337391
		public static bool StreamerMode
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.StreamerMode", PlayerPrefs.GetInt("CensorBars.Enabled.v2", 0)) != 0;
			}
		}

		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x06007DFF RID: 32255 RVA: 0x003391AB File Offset: 0x003373AB
		public static int StreamerPixelSize
		{
			get
			{
				return Mathf.Clamp(PlayerPrefs.GetInt("Settings.Gameplay.StreamerPixelSize", 32), 12, 80);
			}
		}

		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x06007E00 RID: 32256 RVA: 0x003391C2 File Offset: 0x003373C2
		public static bool ShowFps
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.ShowFPS", 0) != 0;
			}
		}

		// Token: 0x1700144C RID: 5196
		// (get) Token: 0x06007E01 RID: 32257 RVA: 0x003391D2 File Offset: 0x003373D2
		public static bool ShowGameplayUI
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.ShowGameplayUI", 1) != 0;
			}
		}

		// Token: 0x1700144D RID: 5197
		// (get) Token: 0x06007E02 RID: 32258 RVA: 0x003391E2 File Offset: 0x003373E2
		public static bool ShowFaceCamera
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.ShowFaceCamera", 1) != 0;
			}
		}

		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x06007E03 RID: 32259 RVA: 0x003391F2 File Offset: 0x003373F2
		public static bool ShowMinimap
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.ShowMinimap", 1) != 0;
			}
		}

		// Token: 0x1700144F RID: 5199
		// (get) Token: 0x06007E04 RID: 32260 RVA: 0x00339202 File Offset: 0x00337402
		public static bool ShowDistanceAndScore
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.ShowDistanceAndScore", 1) != 0;
			}
		}

		// Token: 0x17001450 RID: 5200
		// (get) Token: 0x06007E05 RID: 32261 RVA: 0x00339212 File Offset: 0x00337412
		public static GameSettingsService.BodyPhysicsStyle BodyPhysics
		{
			get
			{
				return (GameSettingsService.BodyPhysicsStyle)Mathf.Clamp(PlayerPrefs.GetInt("Settings.Gameplay.BodyPhysics", 0), 0, 1);
			}
		}

		// Token: 0x06007E06 RID: 32262 RVA: 0x00339226 File Offset: 0x00337426
		public static void SetShowGameplayUI(bool value)
		{
			GameSettingsService.SetVisibility("Settings.Gameplay.ShowGameplayUI", value);
		}

		// Token: 0x06007E07 RID: 32263 RVA: 0x00339233 File Offset: 0x00337433
		public static void SetShowFaceCamera(bool value)
		{
			GameSettingsService.SetVisibility("Settings.Gameplay.ShowFaceCamera", value);
		}

		// Token: 0x06007E08 RID: 32264 RVA: 0x00339240 File Offset: 0x00337440
		public static void SetShowMinimap(bool value)
		{
			GameSettingsService.SetVisibility("Settings.Gameplay.ShowMinimap", value);
		}

		// Token: 0x06007E09 RID: 32265 RVA: 0x0033924D File Offset: 0x0033744D
		public static void SetShowDistanceAndScore(bool value)
		{
			GameSettingsService.SetVisibility("Settings.Gameplay.ShowDistanceAndScore", value);
		}

		// Token: 0x06007E0A RID: 32266 RVA: 0x0033925A File Offset: 0x0033745A
		public static void ApplyLowGraphicsHudDefault(bool lowQuality, bool selectingPreset = false)
		{
			if (lowQuality && (selectingPreset || !PlayerPrefs.HasKey("Settings.Gameplay.ShowFaceCamera")))
			{
				GameSettingsService.SetShowFaceCamera(false);
			}
		}

		// Token: 0x06007E0B RID: 32267 RVA: 0x00339274 File Offset: 0x00337474
		private static void SetVisibility(string key, bool value)
		{
			PlayerPrefs.SetInt(key, value ? 1 : 0);
			PlayerPrefs.Save();
			Action settingsChanged = GameSettingsService.SettingsChanged;
			if (settingsChanged == null)
			{
				return;
			}
			settingsChanged();
		}

		// Token: 0x17001451 RID: 5201
		// (get) Token: 0x06007E0C RID: 32268 RVA: 0x00339297 File Offset: 0x00337497
		public static int PhoneVideoHeight
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.PhoneVideoHeight", 1080);
			}
		}

		// Token: 0x17001452 RID: 5202
		// (get) Token: 0x06007E0D RID: 32269 RVA: 0x003392A8 File Offset: 0x003374A8
		public static int PhoneVideoFps
		{
			get
			{
				return PlayerPrefs.GetInt("Settings.Gameplay.PhoneVideoFps", 30);
			}
		}

		// Token: 0x06007E0E RID: 32270 RVA: 0x003392B6 File Offset: 0x003374B6
		public static void SetMasterVolume(float value)
		{
			GameSettingsService.SetAudio("Settings.Audio.Master", value);
		}

		// Token: 0x06007E0F RID: 32271 RVA: 0x003392C3 File Offset: 0x003374C3
		public static void SetHeartbeatVolume(float value)
		{
			GameSettingsService.SetAudio("Settings.Audio.Heartbeat", value);
		}

		// Token: 0x06007E10 RID: 32272 RVA: 0x003392D0 File Offset: 0x003374D0
		public static void SetMusicVolume(float value)
		{
			GameSettingsService.SetAudio("Settings.Audio.Music", value);
		}

		// Token: 0x06007E11 RID: 32273 RVA: 0x003392DD File Offset: 0x003374DD
		public static void SetSfxVolume(float value)
		{
			GameSettingsService.SetAudio("Settings.Audio.SFX", value);
		}

		// Token: 0x06007E12 RID: 32274 RVA: 0x003392EA File Offset: 0x003374EA
		public static void SetCielVoiceVolume(float value)
		{
			GameSettingsService.SetAudio("Settings.Audio.CielVoice", value);
		}

		// Token: 0x06007E13 RID: 32275 RVA: 0x003392F7 File Offset: 0x003374F7
		public static void SetVoiceVolume(float value)
		{
			GameSettingsService.SetCielVoiceVolume(value);
		}

		// Token: 0x06007E14 RID: 32276 RVA: 0x003392FF File Offset: 0x003374FF
		public static void SetNpcReactionVolume(float value)
		{
			GameSettingsService.SetAudio("Settings.Audio.NPCReaction", value);
		}

		// Token: 0x06007E15 RID: 32277 RVA: 0x0033930C File Offset: 0x0033750C
		public static void SetMuted(bool value)
		{
			PlayerPrefs.SetInt("Settings.Audio.Muted", value ? 1 : 0);
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E16 RID: 32278 RVA: 0x00339324 File Offset: 0x00337524
		public static void SetLookSensitivity(float value)
		{
			PlayerPrefs.SetFloat("Settings.Gameplay.Sensitivity", Mathf.Clamp(value, 0.1f, 3f));
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E17 RID: 32279 RVA: 0x00339345 File Offset: 0x00337545
		public static void SetInvertMouseX(bool value)
		{
			PlayerPrefs.SetInt("Settings.Gameplay.InvertMouseX", value ? 1 : 0);
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E18 RID: 32280 RVA: 0x0033935D File Offset: 0x0033755D
		public static void SetInvertMouseY(bool value)
		{
			PlayerPrefs.SetInt("Settings.Gameplay.InvertMouseY", value ? 1 : 0);
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E19 RID: 32281 RVA: 0x00339375 File Offset: 0x00337575
		public static void SetCielVoiceSubtitles(bool value)
		{
			PlayerPrefs.SetInt("Settings.Gameplay.CielVoiceSubtitles", value ? 1 : 0);
			PlayerPrefs.DeleteKey("Settings.Gameplay.Subtitles");
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E1A RID: 32282 RVA: 0x00339397 File Offset: 0x00337597
		public static void SetSubtitles(bool value)
		{
			GameSettingsService.SetCielVoiceSubtitles(value);
		}

		// Token: 0x06007E1B RID: 32283 RVA: 0x0033939F File Offset: 0x0033759F
		public static void SetCameraShake(bool value)
		{
			PlayerPrefs.SetInt("Settings.Gameplay.CameraShake", value ? 1 : 0);
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E1C RID: 32284 RVA: 0x003393B7 File Offset: 0x003375B7
		public static void SetGarbageMudBodyPaint(bool value)
		{
			PlayerPrefs.SetInt("Settings.Gameplay.GarbageMudBodyPaint", value ? 1 : 0);
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E1D RID: 32285 RVA: 0x003393CF File Offset: 0x003375CF
		public static void SetStreamerMode(bool value)
		{
			PlayerPrefs.SetInt("Settings.Gameplay.StreamerMode", value ? 1 : 0);
			PlayerPrefs.SetInt("CensorBars.Enabled.v2", value ? 1 : 0);
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E1E RID: 32286 RVA: 0x003393F8 File Offset: 0x003375F8
		public static void SetStreamerPixelSize(float value)
		{
			if (float.IsNaN(value) || float.IsInfinity(value))
			{
				value = 32f;
			}
			PlayerPrefs.SetInt("Settings.Gameplay.StreamerPixelSize", Mathf.Clamp(Mathf.RoundToInt(value), 12, 80));
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E1F RID: 32287 RVA: 0x0033942F File Offset: 0x0033762F
		public static void SetShowFps(bool value)
		{
			PlayerPrefs.SetInt("Settings.Gameplay.ShowFPS", value ? 1 : 0);
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E20 RID: 32288 RVA: 0x00339447 File Offset: 0x00337647
		public static void SetBodyPhysics(GameSettingsService.BodyPhysicsStyle value)
		{
			PlayerPrefs.SetInt("Settings.Gameplay.BodyPhysics", Mathf.Clamp((int)value, 0, 1));
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E21 RID: 32289 RVA: 0x00339460 File Offset: 0x00337660
		public static void SetPhoneVideoHeight(int value)
		{
			PlayerPrefs.SetInt("Settings.Gameplay.PhoneVideoHeight", Mathf.Clamp(value, 360, 2160));
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E22 RID: 32290 RVA: 0x00339481 File Offset: 0x00337681
		public static void SetPhoneVideoFps(int value)
		{
			PlayerPrefs.SetInt("Settings.Gameplay.PhoneVideoFps", Mathf.Clamp(value, 12, 60));
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E23 RID: 32291 RVA: 0x0033949C File Offset: 0x0033769C
		public static void ApplyAll()
		{
			if (PlayerPrefs.HasKey("Settings.Gameplay.ShowGameplayUI"))
			{
				PlayerPrefs.DeleteKey("Settings.Gameplay.ShowGameplayUI");
				PlayerPrefs.Save();
			}
			GameSettingsService.ApplyAudio();
			if (CielVoiceController.Instance != null)
			{
				CielVoiceController.Instance.SetSubtitlesEnabled(GameSettingsService.CielVoiceSubtitles);
			}
			Action settingsChanged = GameSettingsService.SettingsChanged;
			if (settingsChanged == null)
			{
				return;
			}
			settingsChanged();
		}

		// Token: 0x06007E24 RID: 32292 RVA: 0x003394F4 File Offset: 0x003376F4
		public static void ApplyAudio()
		{
			AudioListener.volume = 1f;
			GameAudioRouter.ApplyMixerVolumes();
		}

		// Token: 0x06007E25 RID: 32293 RVA: 0x00339508 File Offset: 0x00337708
		public static void ResetAudioAndGameplay()
		{
			string[] array = new string[]
			{
				"Settings.Audio.Master", "Settings.Audio.Heartbeat", "Settings.Audio.Music", "Settings.Audio.SFX", "Settings.Audio.CielVoice", "Settings.Audio.NPCReaction", "Settings.Audio.Muted", "Settings.Gameplay.Sensitivity", "Settings.Gameplay.InvertMouseX", "Settings.Gameplay.InvertMouseY",
				"Settings.Gameplay.CielVoiceSubtitles", "Settings.Gameplay.Subtitles", "Settings.Gameplay.CameraShake", "Settings.Gameplay.GarbageMudBodyPaint", "Settings.Gameplay.StreamerMode", "Settings.Gameplay.StreamerPixelSize", "CensorBars.Enabled.v2", "Settings.Gameplay.ShowFPS", "Settings.Gameplay.ShowGameplayUI", "Settings.Gameplay.ShowFaceCamera",
				"Settings.Gameplay.ShowMinimap", "Settings.Gameplay.ShowDistanceAndScore", "Settings.Gameplay.PhoneVideoHeight", "Settings.Gameplay.PhoneVideoFps", "Settings.Gameplay.BodyPhysics"
			};
			for (int i = 0; i < array.Length; i++)
			{
				PlayerPrefs.DeleteKey(array[i]);
			}
			PlayerPrefs.Save();
			GameSettingsService.ApplyAll();
		}

		// Token: 0x06007E26 RID: 32294 RVA: 0x00339615 File Offset: 0x00337815
		private static void SetAudio(string key, float value)
		{
			PlayerPrefs.SetFloat(key, Mathf.Clamp01(value));
			GameSettingsService.SaveAndApply();
		}

		// Token: 0x06007E27 RID: 32295 RVA: 0x00339628 File Offset: 0x00337828
		private static void SaveAndApply()
		{
			PlayerPrefs.Save();
			GameSettingsService.ApplyAll();
		}

		// Token: 0x04008194 RID: 33172
		private const string MasterKey = "Settings.Audio.Master";

		// Token: 0x04008195 RID: 33173
		private const string HeartbeatKey = "Settings.Audio.Heartbeat";

		// Token: 0x04008196 RID: 33174
		private const string MusicKey = "Settings.Audio.Music";

		// Token: 0x04008197 RID: 33175
		private const string SfxKey = "Settings.Audio.SFX";

		// Token: 0x04008198 RID: 33176
		private const string CielVoiceKey = "Settings.Audio.CielVoice";

		// Token: 0x04008199 RID: 33177
		private const string NpcReactionKey = "Settings.Audio.NPCReaction";

		// Token: 0x0400819A RID: 33178
		private const string MutedKey = "Settings.Audio.Muted";

		// Token: 0x0400819B RID: 33179
		private const string SensitivityKey = "Settings.Gameplay.Sensitivity";

		// Token: 0x0400819C RID: 33180
		private const string InvertMouseXKey = "Settings.Gameplay.InvertMouseX";

		// Token: 0x0400819D RID: 33181
		private const string InvertMouseYKey = "Settings.Gameplay.InvertMouseY";

		// Token: 0x0400819E RID: 33182
		private const string CielSubtitlesKey = "Settings.Gameplay.CielVoiceSubtitles";

		// Token: 0x0400819F RID: 33183
		private const string LegacySubtitlesKey = "Settings.Gameplay.Subtitles";

		// Token: 0x040081A0 RID: 33184
		private const string CameraShakeKey = "Settings.Gameplay.CameraShake";

		// Token: 0x040081A1 RID: 33185
		private const string GarbageMudBodyPaintKey = "Settings.Gameplay.GarbageMudBodyPaint";

		// Token: 0x040081A2 RID: 33186
		public const string StreamerPixelSizeKey = "Settings.Gameplay.StreamerPixelSize";

		// Token: 0x040081A3 RID: 33187
		public const int DefaultStreamerPixelSize = 32;

		// Token: 0x040081A4 RID: 33188
		public const int MinStreamerPixelSize = 12;

		// Token: 0x040081A5 RID: 33189
		public const int MaxStreamerPixelSize = 80;

		// Token: 0x040081A6 RID: 33190
		private const string StreamerModeKey = "Settings.Gameplay.StreamerMode";

		// Token: 0x040081A7 RID: 33191
		private const string ShowFpsKey = "Settings.Gameplay.ShowFPS";

		// Token: 0x040081A8 RID: 33192
		private const string ShowGameplayUIKey = "Settings.Gameplay.ShowGameplayUI";

		// Token: 0x040081A9 RID: 33193
		private const string ShowFaceCameraKey = "Settings.Gameplay.ShowFaceCamera";

		// Token: 0x040081AA RID: 33194
		private const string ShowMinimapKey = "Settings.Gameplay.ShowMinimap";

		// Token: 0x040081AB RID: 33195
		private const string ShowDistanceAndScoreKey = "Settings.Gameplay.ShowDistanceAndScore";

		// Token: 0x040081AC RID: 33196
		private const string PhoneVideoHeightKey = "Settings.Gameplay.PhoneVideoHeight";

		// Token: 0x040081AD RID: 33197
		private const string PhoneVideoFpsKey = "Settings.Gameplay.PhoneVideoFps";

		// Token: 0x040081AE RID: 33198
		public const string BodyPhysicsKey = "Settings.Gameplay.BodyPhysics";

		// Token: 0x040081AF RID: 33199
		public const float DefaultMasterVolume = 0.9f;

		// Token: 0x040081B0 RID: 33200
		public const float DefaultHeartbeatVolume = 0.9f;

		// Token: 0x040081B1 RID: 33201
		public const float DefaultMusicVolume = 0.72f;

		// Token: 0x040081B2 RID: 33202
		public const float DefaultSfxVolume = 0.82f;

		// Token: 0x040081B3 RID: 33203
		public const float DefaultCielVoiceVolume = 1f;

		// Token: 0x040081B4 RID: 33204
		public const float DefaultNpcReactionVolume = 1f;

		// Token: 0x040081B5 RID: 33205
		public const float DefaultLookSensitivity = 1f;

		// Token: 0x040081B6 RID: 33206
		public const float DefaultVoiceVolume = 1f;

		// Token: 0x040081B7 RID: 33207
		public const int DefaultPhoneVideoHeight = 1080;

		// Token: 0x040081B8 RID: 33208
		public const int DefaultPhoneVideoFps = 30;

		// Token: 0x0200103B RID: 4155
		public enum BodyPhysicsStyle
		{
			// Token: 0x040081BB RID: 33211
			Anime,
			// Token: 0x040081BC RID: 33212
			Classic
		}
	}
}
