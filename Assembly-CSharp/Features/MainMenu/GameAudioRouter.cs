using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Features.Audio;
using Features.NPC;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Features.MainMenu
{
	// Token: 0x02001037 RID: 4151
	[DefaultExecutionOrder(-9000)]
	public sealed class GameAudioRouter : MonoBehaviour
	{
		// Token: 0x06007DD4 RID: 32212 RVA: 0x003388DA File Offset: 0x00336ADA
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Bootstrap()
		{
			GameAudioRouter.EnsureInstance();
		}

		// Token: 0x06007DD5 RID: 32213 RVA: 0x003388E4 File Offset: 0x00336AE4
		public static void ApplyMixerVolumes()
		{
			GameAudioRouter.EnsureMixer();
			if (GameAudioRouter._mixer == null)
			{
				return;
			}
			GameAudioRouter.SetDb("MasterVolume", GameSettingsService.Muted ? 0f : GameSettingsService.MasterVolume);
			GameAudioRouter.SetDb("HeartbeatVolume", GameSettingsService.HeartbeatVolume);
			GameAudioRouter.SetDb("CielVoiceVolume", GameSettingsService.CielVoiceVolume);
			GameAudioRouter.SetDb("NPCReactionVolume", GameSettingsService.NpcReactionVolume);
			GameAudioRouter.SetDb("MusicVolume", GameSettingsService.MusicVolume);
			GameAudioRouter.SetDb("SFXVolume", GameSettingsService.SfxVolume);
		}

		// Token: 0x06007DD6 RID: 32214 RVA: 0x0033896C File Offset: 0x00336B6C
		public static void Route(AudioSource source, GameAudioRouter.Channel channel)
		{
			if (source == null)
			{
				return;
			}
			GameAudioRouter.EnsureMixer();
			AudioMixerGroup audioMixerGroup;
			if (GameAudioRouter.Groups.TryGetValue(channel, out audioMixerGroup) && audioMixerGroup != null)
			{
				source.outputAudioMixerGroup = audioMixerGroup;
			}
		}

		// Token: 0x06007DD7 RID: 32215 RVA: 0x003389A7 File Offset: 0x00336BA7
		private static void EnsureInstance()
		{
			if (GameAudioRouter._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("GameAudioRouter");
			Object.DontDestroyOnLoad(gameObject);
			GameAudioRouter._instance = gameObject.AddComponent<GameAudioRouter>();
		}

		// Token: 0x06007DD8 RID: 32216 RVA: 0x003389D4 File Offset: 0x00336BD4
		private static void EnsureMixer()
		{
			if (GameAudioRouter._mixer != null)
			{
				return;
			}
			GameAudioRouter._mixer = Resources.Load<AudioMixer>("Audio/GameAudioMixer");
			if (GameAudioRouter._mixer == null)
			{
				Debug.LogError("[GameAudioRouter] Resources/Audio/GameAudioMixer.mixer not found.");
				return;
			}
			GameAudioRouter.Groups.Clear();
			GameAudioRouter.CacheGroup(GameAudioRouter.Channel.Heartbeat, "Heartbeat");
			GameAudioRouter.CacheGroup(GameAudioRouter.Channel.CielVoice, "CielVoice");
			GameAudioRouter.CacheGroup(GameAudioRouter.Channel.NpcReaction, "NPCReaction");
			GameAudioRouter.CacheGroup(GameAudioRouter.Channel.Music, "Music");
			GameAudioRouter.CacheGroup(GameAudioRouter.Channel.Sfx, "SFX");
		}

		// Token: 0x06007DD9 RID: 32217 RVA: 0x00338A58 File Offset: 0x00336C58
		private static void CacheGroup(GameAudioRouter.Channel channel, string name)
		{
			AudioMixerGroup[] array = GameAudioRouter._mixer.FindMatchingGroups(name);
			if (array != null && array.Length != 0)
			{
				GameAudioRouter.Groups[channel] = array[0];
			}
		}

		// Token: 0x06007DDA RID: 32218 RVA: 0x00338A88 File Offset: 0x00336C88
		private static void SetDb(string parameter, float linear)
		{
			float num = ((linear <= 0.0001f) ? (-80f) : (Mathf.Log10(Mathf.Clamp01(linear)) * 20f));
			GameAudioRouter._mixer.SetFloat(parameter, num);
		}

		// Token: 0x06007DDB RID: 32219 RVA: 0x00338AC4 File Offset: 0x00336CC4
		private void Awake()
		{
			if (GameAudioRouter._instance != null && GameAudioRouter._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			GameAudioRouter._instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			SceneManager.sceneLoaded += this.HandleSceneLoaded;
			GameAudioRouter.ApplyMixerVolumes();
			base.StartCoroutine(this.ScanLoop());
		}

		// Token: 0x06007DDC RID: 32220 RVA: 0x00338B2B File Offset: 0x00336D2B
		private void OnDestroy()
		{
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			if (GameAudioRouter._instance == this)
			{
				GameAudioRouter._instance = null;
			}
		}

		// Token: 0x06007DDD RID: 32221 RVA: 0x00338B51 File Offset: 0x00336D51
		private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			GameAudioRouter.ApplyMixerVolumes();
			GameAudioRouter.RouteAllSources();
		}

		// Token: 0x06007DDE RID: 32222 RVA: 0x00338B5D File Offset: 0x00336D5D
		private IEnumerator ScanLoop()
		{
			WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.35f);
			for (;;)
			{
				if (!NPCSimulationPause.IsPaused)
				{
					GameAudioRouter.RouteAllSources();
				}
				yield return wait;
			}
			yield break;
		}

		// Token: 0x06007DDF RID: 32223 RVA: 0x00338B68 File Offset: 0x00336D68
		private static void RouteAllSources()
		{
			foreach (AudioSource audioSource in Object.FindObjectsByType<AudioSource>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (!(audioSource == null))
				{
					int instanceID = audioSource.GetInstanceID();
					int num = ((audioSource.clip != null) ? audioSource.clip.GetInstanceID() : 0);
					int num2;
					GameAudioRouter.Channel channel;
					AudioMixerGroup audioMixerGroup;
					if (!GameAudioRouter.LastClipBySource.TryGetValue(instanceID, out num2) || num2 != num || !GameAudioRouter.LastChannelBySource.TryGetValue(instanceID, out channel) || !GameAudioRouter.Groups.TryGetValue(channel, out audioMixerGroup) || !(audioSource.outputAudioMixerGroup == audioMixerGroup))
					{
						GameAudioRouter.Channel channel2 = GameAudioRouter.Classify(audioSource);
						GameAudioRouter.Route(audioSource, channel2);
						GameAudioRouter.LastClipBySource[instanceID] = num;
						GameAudioRouter.LastChannelBySource[instanceID] = channel2;
					}
				}
			}
		}

		// Token: 0x06007DE0 RID: 32224 RVA: 0x00338C34 File Offset: 0x00336E34
		private static GameAudioRouter.Channel Classify(AudioSource source)
		{
			FootstepController component = source.GetComponent<FootstepController>();
			if (component != null && component.OwnsAudioSource(source))
			{
				return GameAudioRouter.Channel.Sfx;
			}
			MudFootstepAudioController component2 = source.GetComponent<MudFootstepAudioController>();
			if (component2 != null && component2.OwnsAudioSource(source))
			{
				return GameAudioRouter.Channel.Sfx;
			}
			if (source.GetComponentInParent<AmbientAudioController>() != null || source.GetComponentInParent<MallAmbientAudio>() != null)
			{
				return GameAudioRouter.Channel.Sfx;
			}
			string text = GameAudioRouter.BuildContext(source);
			if (GameAudioRouter.Contains(text, "detectionstingersource") || GameAudioRouter.Contains(text, "tvnewsintrocontroller") || GameAudioRouter.Contains(text, "smartphoneflash"))
			{
				return GameAudioRouter.Channel.Sfx;
			}
			if (GameAudioRouter.Contains(text, "heartbeat") || GameAudioRouter.Contains(text, "heart beat"))
			{
				return GameAudioRouter.Channel.Heartbeat;
			}
			if (GameAudioRouter.Contains(text, "cielvoice") || GameAudioRouter.Contains(text, "ciel voice") || GameAudioRouter.Contains(text, "lewd breathing") || GameAudioRouter.Contains(text, "perpwalkvoice") || GameAudioRouter.Contains(text, "runningnude"))
			{
				return GameAudioRouter.Channel.CielVoice;
			}
			if (GameAudioRouter.ContainsAny(text, GameAudioRouter.NpcTokens) || (GameAudioRouter.Contains(text, "npc") && (GameAudioRouter.Contains(text, "audio") || GameAudioRouter.Contains(text, "voice") || GameAudioRouter.Contains(text, "reaction"))))
			{
				return GameAudioRouter.Channel.NpcReaction;
			}
			if (GameAudioRouter.ContainsAny(text, GameAudioRouter.MusicTokens) || GameAudioRouter.Contains(text, "detectionambientsource") || GameAudioRouter.Contains(text, "gameoverambientsource") || GameAudioRouter.Contains(text, "tvstudiocontroller") || (source.loop && source.spatialBlend <= 0.01f && (GameAudioRouter.Contains(text, "ambient") || GameAudioRouter.Contains(text, "intro"))))
			{
				return GameAudioRouter.Channel.Music;
			}
			return GameAudioRouter.Channel.Sfx;
		}

		// Token: 0x06007DE1 RID: 32225 RVA: 0x00338DD0 File Offset: 0x00336FD0
		private static string BuildContext(AudioSource source)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			Transform transform = source.transform;
			int num = 0;
			while (transform != null && num < 5)
			{
				stringBuilder.Append(' ').Append(transform.name);
				num++;
				transform = transform.parent;
			}
			if (source.clip != null)
			{
				stringBuilder.Append(' ').Append(source.clip.name);
			}
			MonoBehaviour[] componentsInParent = source.GetComponentsInParent<MonoBehaviour>(true);
			for (int i = 0; i < componentsInParent.Length; i++)
			{
				if (componentsInParent[i] != null)
				{
					stringBuilder.Append(' ').Append(componentsInParent[i].GetType().FullName);
				}
			}
			return stringBuilder.ToString().ToLowerInvariant();
		}

		// Token: 0x06007DE2 RID: 32226 RVA: 0x00338E93 File Offset: 0x00337093
		private static bool Contains(string value, string token)
		{
			return value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06007DE3 RID: 32227 RVA: 0x00338EA4 File Offset: 0x003370A4
		private static bool ContainsAny(string value, string[] tokens)
		{
			for (int i = 0; i < tokens.Length; i++)
			{
				if (GameAudioRouter.Contains(value, tokens[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008182 RID: 33154
		private const string MixerResourcePath = "Audio/GameAudioMixer";

		// Token: 0x04008183 RID: 33155
		private const float ScanInterval = 0.35f;

		// Token: 0x04008184 RID: 33156
		private static GameAudioRouter _instance;

		// Token: 0x04008185 RID: 33157
		private static AudioMixer _mixer;

		// Token: 0x04008186 RID: 33158
		private static readonly Dictionary<GameAudioRouter.Channel, AudioMixerGroup> Groups = new Dictionary<GameAudioRouter.Channel, AudioMixerGroup>();

		// Token: 0x04008187 RID: 33159
		private static readonly Dictionary<int, int> LastClipBySource = new Dictionary<int, int>();

		// Token: 0x04008188 RID: 33160
		private static readonly Dictionary<int, GameAudioRouter.Channel> LastChannelBySource = new Dictionary<int, GameAudioRouter.Channel>();

		// Token: 0x04008189 RID: 33161
		private static readonly string[] MusicTokens = new string[] { "music", "score", "soundtrack", "bgm", "theme", "gameoverloop", "game over loop", "ambient score" };

		// Token: 0x0400818A RID: 33162
		private static readonly string[] NpcTokens = new string[] { "npcreaction", "npc reaction", "npcvoice", "npc voice", "scream", "crowd reaction", "citizen reaction", "driver reaction" };

		// Token: 0x02001038 RID: 4152
		public enum Channel
		{
			// Token: 0x0400818C RID: 33164
			Heartbeat,
			// Token: 0x0400818D RID: 33165
			CielVoice,
			// Token: 0x0400818E RID: 33166
			NpcReaction,
			// Token: 0x0400818F RID: 33167
			Music,
			// Token: 0x04008190 RID: 33168
			Sfx
		}
	}
}
