using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200089E RID: 2206
	public static class PresetIconCache
	{
		// Token: 0x06003B5B RID: 15195 RVA: 0x00161058 File Offset: 0x0015F258
		public static void Load(string iconFileName, Action<Sprite> onLoaded)
		{
			Sprite sprite;
			if (PresetIconCache.TryGet(iconFileName, out sprite))
			{
				if (onLoaded != null)
				{
					onLoaded(sprite);
				}
				return;
			}
			if (!Application.isPlaying)
			{
				if (onLoaded != null)
				{
					onLoaded(PresetIconCache.LoadSync(iconFileName));
				}
				return;
			}
			if (PresetIconCache._loadHost == null)
			{
				GameObject gameObject = new GameObject("Preset Icon Loader");
				gameObject.hideFlags = HideFlags.HideInHierarchy;
				Object.DontDestroyOnLoad(gameObject);
				PresetIconCache._loadHost = gameObject.AddComponent<PresetIconLoadHost>();
			}
			PresetIconCache._loadHost.StartCoroutine(PresetIconCache.LoadAsync(iconFileName, onLoaded));
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x001610D1 File Offset: 0x0015F2D1
		internal static void ReleaseLoadHost(PresetIconLoadHost host)
		{
			if (PresetIconCache._loadHost != host)
			{
				return;
			}
			PresetIconCache._pending.Clear();
			PresetIconCache._loadHost = null;
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x001610F1 File Offset: 0x0015F2F1
		public static bool TryGet(string iconFileName, out Sprite sprite)
		{
			sprite = null;
			return !string.IsNullOrWhiteSpace(iconFileName) && PresetIconCache._cache.TryGetValue(iconFileName, out sprite) && sprite != null;
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x00161118 File Offset: 0x0015F318
		private static void ProtectPhoto(Sprite sprite)
		{
			if (sprite != null && sprite.texture != null && sprite.texture.mipmapCount > 1 && !sprite.texture.ignoreMipmapLimit)
			{
				sprite.texture.ignoreMipmapLimit = true;
			}
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x00161158 File Offset: 0x0015F358
		public static void Put(string iconFileName, Sprite sprite)
		{
			if (string.IsNullOrWhiteSpace(iconFileName) || sprite == null)
			{
				return;
			}
			PresetIconCache.ProtectPhoto(sprite);
			PresetIconCache._cache[iconFileName] = sprite;
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x00161180 File Offset: 0x0015F380
		public static void Invalidate(string iconFileName)
		{
			if (string.IsNullOrWhiteSpace(iconFileName))
			{
				return;
			}
			PresetIconCache._pending.Remove(iconFileName);
			Sprite sprite;
			if (PresetIconCache._cache.TryGetValue(iconFileName, out sprite))
			{
				PresetIconCache._cache.Remove(iconFileName);
				if (sprite != null)
				{
					if (sprite.texture != null)
					{
						Object.Destroy(sprite.texture);
					}
					Object.Destroy(sprite);
				}
			}
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x001611E5 File Offset: 0x0015F3E5
		public static IEnumerator LoadAsync(string iconFileName, Action<Sprite> onLoaded)
		{
			if (string.IsNullOrWhiteSpace(iconFileName))
			{
				if (onLoaded != null)
				{
					onLoaded(null);
				}
				yield break;
			}
			Sprite sprite;
			if (PresetIconCache._cache.TryGetValue(iconFileName, out sprite) && sprite != null)
			{
				if (onLoaded != null)
				{
					onLoaded(sprite);
				}
				yield break;
			}
			List<Action<Sprite>> list;
			if (PresetIconCache._pending.TryGetValue(iconFileName, out list))
			{
				if (onLoaded != null)
				{
					list.Add(onLoaded);
				}
				yield break;
			}
			List<Action<Sprite>> callbacks = new List<Action<Sprite>>();
			if (onLoaded != null)
			{
				callbacks.Add(onLoaded);
			}
			PresetIconCache._pending[iconFileName] = callbacks;
			string iconPath = Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, iconFileName);
			Sprite result = null;
			bool ownsResult = false;
			if (File.Exists(iconPath))
			{
				string absoluteUri = new Uri(iconPath).AbsoluteUri;
				using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(absoluteUri))
				{
					yield return request.SendWebRequest();
					if (request.result == UnityWebRequest.Result.Success)
					{
						Texture2D content = DownloadHandlerTexture.GetContent(request);
						if (content != null)
						{
							result = Sprite.Create(content, new Rect(0f, 0f, (float)content.width, (float)content.height), Vector2.one * 0.5f);
							ownsResult = true;
						}
					}
					else
					{
						result = PresetIconCache.LoadSpriteSync(iconPath);
						ownsResult = result != null;
					}
				}
				UnityWebRequest request = null;
			}
			if (result == null)
			{
				result = PresetIconCache.LoadFromResources(iconFileName);
			}
			List<Action<Sprite>> list2;
			if (!PresetIconCache._pending.TryGetValue(iconFileName, out list2) || list2 != callbacks)
			{
				if (ownsResult && result != null)
				{
					Object.Destroy(result.texture);
					Object.Destroy(result);
				}
				yield break;
			}
			if (result != null)
			{
				PresetIconCache.ProtectPhoto(result);
				PresetIconCache._cache[iconFileName] = result;
			}
			PresetIconCache._pending.Remove(iconFileName);
			for (int i = 0; i < callbacks.Count; i++)
			{
				Action<Sprite> action = callbacks[i];
				if (action != null)
				{
					action(result);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x001611FC File Offset: 0x0015F3FC
		public static Sprite LoadSync(string iconFileName)
		{
			if (string.IsNullOrWhiteSpace(iconFileName))
			{
				return null;
			}
			Sprite sprite;
			if (PresetIconCache._cache.TryGetValue(iconFileName, out sprite) && sprite != null)
			{
				return sprite;
			}
			Sprite sprite2 = PresetIconCache.LoadSpriteSync(Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, iconFileName));
			if (sprite2 == null)
			{
				sprite2 = PresetIconCache.LoadFromResources(iconFileName);
			}
			if (sprite2 != null)
			{
				PresetIconCache.ProtectPhoto(sprite2);
				PresetIconCache._cache[iconFileName] = sprite2;
			}
			return sprite2;
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x0016126C File Offset: 0x0015F46C
		private static Sprite LoadFromResources(string iconFileName)
		{
			if (string.IsNullOrWhiteSpace(iconFileName))
			{
				return null;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(iconFileName);
			return Resources.Load<Sprite>("NpcPresetIcons/" + fileNameWithoutExtension);
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x0016129C File Offset: 0x0015F49C
		private static Sprite LoadSpriteSync(string iconPath)
		{
			if (string.IsNullOrWhiteSpace(iconPath) || !File.Exists(iconPath))
			{
				return null;
			}
			Sprite sprite;
			try
			{
				byte[] array = File.ReadAllBytes(iconPath);
				Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
				if (!texture2D.LoadImage(array))
				{
					Object.Destroy(texture2D);
					sprite = null;
				}
				else
				{
					sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.one * 0.5f);
				}
			}
			catch
			{
				sprite = null;
			}
			return sprite;
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x0016132C File Offset: 0x0015F52C
		public static void Clear()
		{
			foreach (KeyValuePair<string, Sprite> keyValuePair in PresetIconCache._cache)
			{
				Sprite value = keyValuePair.Value;
				if (value != null)
				{
					if (value.texture != null)
					{
						Object.Destroy(value.texture);
					}
					Object.Destroy(value);
				}
			}
			PresetIconCache._cache.Clear();
			PresetIconCache._pending.Clear();
		}

		// Token: 0x04003B8C RID: 15244
		private const string ResourcesIconsFolder = "NpcPresetIcons";

		// Token: 0x04003B8D RID: 15245
		private static readonly Dictionary<string, Sprite> _cache = new Dictionary<string, Sprite>(StringComparer.Ordinal);

		// Token: 0x04003B8E RID: 15246
		private static readonly Dictionary<string, List<Action<Sprite>>> _pending = new Dictionary<string, List<Action<Sprite>>>(StringComparer.Ordinal);

		// Token: 0x04003B8F RID: 15247
		private static PresetIconLoadHost _loadHost;
	}
}
