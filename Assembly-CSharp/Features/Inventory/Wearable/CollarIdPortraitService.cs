using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bozo.AnimeCharacters;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Inventory.Wearable
{
	// Token: 0x02001115 RID: 4373
	public sealed class CollarIdPortraitService : MonoBehaviour
	{
		// Token: 0x1700150A RID: 5386
		// (get) Token: 0x060084AE RID: 33966 RVA: 0x00361DF4 File Offset: 0x0035FFF4
		private static string CachePath
		{
			get
			{
				return Path.Combine(GameDataPaths.CustomizationRoot, "collar-id-portrait.cache");
			}
		}

		// Token: 0x060084AF RID: 33967 RVA: 0x00361E05 File Offset: 0x00360005
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			CollarIdPortraitService._instance = null;
		}

		// Token: 0x060084B0 RID: 33968 RVA: 0x00361E0D File Offset: 0x0036000D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			CollarIdPortraitService.EnsureInstance();
		}

		// Token: 0x060084B1 RID: 33969 RVA: 0x00361E14 File Offset: 0x00360014
		private static void EnsureInstance()
		{
			if (CollarIdPortraitService._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Collar ID portrait service");
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			Object.DontDestroyOnLoad(gameObject);
			CollarIdPortraitService._instance = gameObject.AddComponent<CollarIdPortraitService>();
		}

		// Token: 0x060084B2 RID: 33970 RVA: 0x00361E48 File Offset: 0x00360048
		public static void Register(CollarIdCard card)
		{
			if (card == null || !Application.IsPlaying(card.gameObject))
			{
				return;
			}
			CollarIdPortraitService.EnsureInstance();
			CollarIdPortraitService._instance._cards.Add(card);
			card.ApplyPortrait(CollarIdPortraitService._instance._portrait);
			CollarIdPortraitService._instance.QueueRefresh();
		}

		// Token: 0x060084B3 RID: 33971 RVA: 0x00361E9C File Offset: 0x0036009C
		public static void Unregister(CollarIdCard card)
		{
			if (CollarIdPortraitService._instance == null)
			{
				return;
			}
			CollarIdPortraitService._instance._cards.Remove(card);
			if (CollarIdPortraitService._instance._cards.Count == 0 && CollarIdPortraitService._instance._source == null && CollarIdPortraitService._instance._refresh != null)
			{
				CollarIdPortraitService._instance.StopCoroutine(CollarIdPortraitService._instance._refresh);
				CollarIdPortraitService._instance._refresh = null;
			}
		}

		// Token: 0x060084B4 RID: 33972 RVA: 0x00361F16 File Offset: 0x00360116
		private void OnEnable()
		{
			MainCharacterAppearanceEvents.InUsePresetChanged += this.RefreshDesiredPreset;
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			this.RefreshDesiredPreset();
		}

		// Token: 0x060084B5 RID: 33973 RVA: 0x00361F40 File Offset: 0x00360140
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this.RefreshDesiredPreset();
			this.BindPlayer();
			this.QueueRefresh();
		}

		// Token: 0x060084B6 RID: 33974 RVA: 0x00361F54 File Offset: 0x00360154
		private void RefreshDesiredPreset()
		{
			CustomizationPresetData customizationPresetData;
			if (!MainCharacterAppearanceService.TryGetInUseMainCharacterPreset(out customizationPresetData))
			{
				customizationPresetData = new CustomizationPresetData
				{
					isSystemPreset = true,
					stateData = new CharacterStateData()
				};
			}
			string text = CollarIdPortraitCache.BuildKey(customizationPresetData);
			if (this._desiredKey != text)
			{
				this._desiredKey = text;
				Texture2D portrait = this._portrait;
				this._portrait = CollarIdPortraitCache.Load(CollarIdPortraitService.CachePath, text);
				foreach (CollarIdCard collarIdCard in this._cards)
				{
					if (collarIdCard != null)
					{
						collarIdCard.ApplyPortrait(this._portrait);
					}
				}
				if (portrait != null)
				{
					Object.Destroy(portrait);
				}
			}
			this.BindPlayer();
			this.QueueRefresh();
		}

		// Token: 0x060084B7 RID: 33975 RVA: 0x00362028 File Offset: 0x00360228
		private void BindPlayer()
		{
			Transform player = PlayerRegistry.Player;
			MainCharacterAppearanceApplier mainCharacterAppearanceApplier = ((player != null) ? player.GetComponentInChildren<MainCharacterAppearanceApplier>(true) : null);
			if (mainCharacterAppearanceApplier != null && (!mainCharacterAppearanceApplier.UsesSavedMainCharacterPreset || mainCharacterAppearanceApplier.gameObject.scene.name == "MainMenu"))
			{
				mainCharacterAppearanceApplier = null;
			}
			if (this._source == mainCharacterAppearanceApplier)
			{
				return;
			}
			if (this._source != null)
			{
				this._source.AppearanceApplied -= this.OnAppearanceApplied;
			}
			this._source = mainCharacterAppearanceApplier;
			if (this._source != null)
			{
				this._source.AppearanceApplied += this.OnAppearanceApplied;
			}
		}

		// Token: 0x060084B8 RID: 33976 RVA: 0x003620E1 File Offset: 0x003602E1
		private void OnAppearanceApplied(MainCharacterAppearanceApplier source)
		{
			if (source == this._source)
			{
				this.QueueRefresh();
			}
		}

		// Token: 0x060084B9 RID: 33977 RVA: 0x003620F8 File Offset: 0x003602F8
		private void QueueRefresh()
		{
			if (this._refresh == null && this._portrait == null && (this._cards.Count > 0 || this._source != null) && base.isActiveAndEnabled)
			{
				this._refresh = base.StartCoroutine(this.RefreshWhenReady());
			}
		}

		// Token: 0x060084BA RID: 33978 RVA: 0x00362151 File Offset: 0x00360351
		private IEnumerator RefreshWhenReady()
		{
			try
			{
				yield return null;
				while ((this._cards.Count > 0 || this._source != null) && this._portrait == null)
				{
					this.BindPlayer();
					if (this._source != null && this._source.isActiveAndEnabled && this._desiredKey == CollarIdPortraitCache.BuildKey(this._source.LastAppliedPreset))
					{
						yield return new WaitForEndOfFrame();
						this.BindPlayer();
						if (this._portrait == null && this._source != null && this._source.isActiveAndEnabled && this._desiredKey == CollarIdPortraitCache.BuildKey(this._source.LastAppliedPreset))
						{
							this.CaptureAppliedPreset();
						}
					}
					if (this._portrait == null)
					{
						yield return new WaitForSecondsRealtime(1f);
					}
				}
			}
			finally
			{
				this._refresh = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060084BB RID: 33979 RVA: 0x00362160 File Offset: 0x00360360
		private void CaptureAppliedPreset()
		{
			MainCharacterAppearanceApplier source = this._source;
			string desiredKey = this._desiredKey;
			RenderTexture temporary = RenderTexture.GetTemporary(750, 848, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			Texture2D texture2D = null;
			try
			{
				if (CollarIdPortraitCapture.TryCapture(source.transform, temporary) && !(source != this._source) && !(desiredKey != this._desiredKey) && !(this._portrait != null))
				{
					RenderTexture active = RenderTexture.active;
					try
					{
						RenderTexture.active = temporary;
						texture2D = new Texture2D(temporary.width, temporary.height, TextureFormat.RGBA32, false)
						{
							name = "Collar ID portrait",
							wrapMode = TextureWrapMode.Clamp
						};
						texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
						texture2D.Apply(false, false);
					}
					finally
					{
						RenderTexture.active = active;
					}
					byte[] array = texture2D.EncodeToPNG();
					texture2D.Apply(false, true);
					this._portrait = texture2D;
					texture2D = null;
					foreach (CollarIdCard collarIdCard in this._cards)
					{
						if (collarIdCard != null)
						{
							collarIdCard.ApplyPortrait(this._portrait);
						}
					}
					try
					{
						CollarIdPortraitCache.Save(CollarIdPortraitService.CachePath, desiredKey, array);
					}
					catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
					{
						if (!this._reportedCacheFailure)
						{
							Debug.LogWarning("[Collar ID] Portrait is available for this session; cache could not be saved: " + ex.Message);
						}
						this._reportedCacheFailure = true;
					}
				}
			}
			finally
			{
				if (texture2D != null)
				{
					Object.Destroy(texture2D);
				}
				RenderTexture.ReleaseTemporary(temporary);
			}
		}

		// Token: 0x060084BC RID: 33980 RVA: 0x00362374 File Offset: 0x00360574
		private void OnDisable()
		{
			MainCharacterAppearanceEvents.InUsePresetChanged -= this.RefreshDesiredPreset;
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			if (this._source != null)
			{
				this._source.AppearanceApplied -= this.OnAppearanceApplied;
			}
			this._source = null;
			if (this._refresh != null)
			{
				base.StopCoroutine(this._refresh);
			}
			this._refresh = null;
		}

		// Token: 0x060084BD RID: 33981 RVA: 0x003623EA File Offset: 0x003605EA
		private void OnDestroy()
		{
			if (this._portrait != null)
			{
				Object.Destroy(this._portrait);
			}
			if (CollarIdPortraitService._instance == this)
			{
				CollarIdPortraitService._instance = null;
			}
		}

		// Token: 0x0400876C RID: 34668
		private static CollarIdPortraitService _instance;

		// Token: 0x0400876D RID: 34669
		private readonly HashSet<CollarIdCard> _cards = new HashSet<CollarIdCard>();

		// Token: 0x0400876E RID: 34670
		private MainCharacterAppearanceApplier _source;

		// Token: 0x0400876F RID: 34671
		private Texture2D _portrait;

		// Token: 0x04008770 RID: 34672
		private string _desiredKey;

		// Token: 0x04008771 RID: 34673
		private Coroutine _refresh;

		// Token: 0x04008772 RID: 34674
		private bool _reportedCacheFailure;
	}
}
