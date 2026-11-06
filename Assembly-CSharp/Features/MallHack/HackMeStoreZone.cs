using System;
using System.Collections.Generic;
using System.IO;
using Features.OxGram;
using UnityEngine;
using UnityEngine.Video;

namespace Features.MallHack
{
	// Token: 0x02001025 RID: 4133
	[DisallowMultipleComponent]
	public sealed class HackMeStoreZone : MonoBehaviour
	{
		// Token: 0x140000E2 RID: 226
		// (add) Token: 0x06007D36 RID: 32054 RVA: 0x00334AA0 File Offset: 0x00332CA0
		// (remove) Token: 0x06007D37 RID: 32055 RVA: 0x00334AD4 File Offset: 0x00332CD4
		public static event Action<HackMeStoreZone, string, bool> BroadcastStarted;

		// Token: 0x1700142C RID: 5164
		// (get) Token: 0x06007D38 RID: 32056 RVA: 0x00334B07 File Offset: 0x00332D07
		public string DisplayName
		{
			get
			{
				return base.gameObject.name;
			}
		}

		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x06007D39 RID: 32057 RVA: 0x00334B14 File Offset: 0x00332D14
		public bool HasActiveHack
		{
			get
			{
				return this._hasActiveHack;
			}
		}

		// Token: 0x1700142E RID: 5166
		// (get) Token: 0x06007D3A RID: 32058 RVA: 0x00334B1C File Offset: 0x00332D1C
		public bool CanProvokeNpcReactions
		{
			get
			{
				return this._hasActiveHack && this._activeHackProvokesReactions;
			}
		}

		// Token: 0x1700142F RID: 5167
		// (get) Token: 0x06007D3B RID: 32059 RVA: 0x00334B2E File Offset: 0x00332D2E
		public bool ActiveHackIsVideo
		{
			get
			{
				return this._hasActiveHack && this._activeHackIsVideo;
			}
		}

		// Token: 0x17001430 RID: 5168
		// (get) Token: 0x06007D3C RID: 32060 RVA: 0x00334B40 File Offset: 0x00332D40
		public bool IsSingleScreen
		{
			get
			{
				return this.singleScreenTarget;
			}
		}

		// Token: 0x06007D3D RID: 32061 RVA: 0x00334B48 File Offset: 0x00332D48
		public static bool IsProvocativePayload(string sourceName, bool video)
		{
			if (video)
			{
				return false;
			}
			string text = OxGramPhotoTag.FromName(sourceName);
			return text == "tits" || text == "pussy" || text == "ass" || text == "fullbody";
		}

		// Token: 0x17001431 RID: 5169
		// (get) Token: 0x06007D3E RID: 32062 RVA: 0x00334B98 File Offset: 0x00332D98
		public Vector3 WorldCenter
		{
			get
			{
				this.EnsureBounds();
				return base.transform.TransformPoint(this.localBoundsCenter);
			}
		}

		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x06007D3F RID: 32063 RVA: 0x00334BB1 File Offset: 0x00332DB1
		internal static List<HackMeStoreZone> ActiveZones
		{
			get
			{
				return HackMeStoreZone.ActiveZonesInternal;
			}
		}

		// Token: 0x17001433 RID: 5171
		// (get) Token: 0x06007D40 RID: 32064 RVA: 0x00334BB8 File Offset: 0x00332DB8
		internal float ApproximateVolume
		{
			get
			{
				this.EnsureBounds();
				Vector3 vector = this.localBoundsSize;
				return Mathf.Abs(vector.x * vector.y * vector.z);
			}
		}

		// Token: 0x06007D41 RID: 32065 RVA: 0x00334BEB File Offset: 0x00332DEB
		private void Awake()
		{
			this.EnsureBounds();
		}

		// Token: 0x06007D42 RID: 32066 RVA: 0x00334BF3 File Offset: 0x00332DF3
		private void OnEnable()
		{
			if (!HackMeStoreZone.ActiveZonesInternal.Contains(this))
			{
				HackMeStoreZone.ActiveZonesInternal.Add(this);
			}
		}

		// Token: 0x06007D43 RID: 32067 RVA: 0x00334C0D File Offset: 0x00332E0D
		private void OnDisable()
		{
			HackMeStoreZone.ActiveZonesInternal.Remove(this);
			if (this._activeHackIsVideo || this._videoPreparePending)
			{
				this._hasActiveHack = false;
				this._activeHackIsVideo = false;
			}
			this.StopVideoBroadcast();
		}

		// Token: 0x06007D44 RID: 32068 RVA: 0x00334C3F File Offset: 0x00332E3F
		private void OnDestroy()
		{
			HackMeStoreZone.ActiveZonesInternal.Remove(this);
			this.StopVideoBroadcast();
			this.DestroyOwnedTextures();
		}

		// Token: 0x06007D45 RID: 32069 RVA: 0x00334C59 File Offset: 0x00332E59
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetActiveZones()
		{
			HackMeStoreZone.ActiveZonesInternal.Clear();
			HackMeStoreZone.BroadcastStarted = null;
		}

		// Token: 0x06007D46 RID: 32070 RVA: 0x00334C6C File Offset: 0x00332E6C
		public bool Contains(Vector3 worldPosition)
		{
			if (this.storeVolume != null && this.storeVolume.enabled)
			{
				return (this.storeVolume.ClosestPoint(worldPosition) - worldPosition).sqrMagnitude <= 0.0001f;
			}
			this.EnsureBounds();
			Vector3 vector = base.transform.InverseTransformPoint(worldPosition);
			return new Bounds(this.localBoundsCenter, this.localBoundsSize).Contains(vector);
		}

		// Token: 0x06007D47 RID: 32071 RVA: 0x00334CE8 File Offset: 0x00332EE8
		public float DistanceToStore(Vector3 worldPosition, out float verticalDistance)
		{
			if (this.storeVolume != null && this.storeVolume.enabled)
			{
				Vector3 vector = this.storeVolume.ClosestPoint(worldPosition);
				verticalDistance = Mathf.Abs(vector.y - worldPosition.y);
				vector.y = worldPosition.y;
				return Vector3.Distance(vector, worldPosition);
			}
			this.EnsureBounds();
			Vector3 vector2 = base.transform.InverseTransformPoint(worldPosition);
			Bounds bounds = new Bounds(this.localBoundsCenter, this.localBoundsSize);
			Vector3 vector3 = bounds.ClosestPoint(vector2);
			Vector3 vector4 = base.transform.TransformPoint(vector3);
			verticalDistance = Mathf.Abs(vector4.y - worldPosition.y);
			vector4.y = worldPosition.y;
			return Vector3.Distance(vector4, worldPosition);
		}

		// Token: 0x06007D48 RID: 32072 RVA: 0x00334DB0 File Offset: 0x00332FB0
		public bool TryGetObservationTarget(Vector3 npcPosition, bool preferLargeDisplay, out HackMeObservationTarget target)
		{
			target = default(HackMeObservationTarget);
			if (!this.CanProvokeNpcReactions || !base.isActiveAndEnabled)
			{
				return false;
			}
			this.EnsureBindings();
			if (this._bindings.Count == 0)
			{
				return false;
			}
			HackMeStoreZone.DisplayBinding displayBinding = default(HackMeStoreZone.DisplayBinding);
			if ((!this.TryPickBinding(preferLargeDisplay, out displayBinding) && !this.TryPickBinding(!preferLargeDisplay, out displayBinding)) || !displayBinding.IsValid)
			{
				return false;
			}
			Vector3 center = displayBinding.renderer.bounds.center;
			Vector3 vector = this.WorldCenter - center;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.36f)
			{
				vector = npcPosition - center;
				vector.y = 0f;
			}
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = base.transform.forward;
			}
			vector.Normalize();
			float num = (displayBinding.profile.fullTexture ? 2.15f : 1.25f);
			Vector3 vector2 = center + vector * num;
			vector2.y = npcPosition.y;
			target = new HackMeObservationTarget(displayBinding.renderer, vector2, center, displayBinding.profile.fullTexture);
			return true;
		}

		// Token: 0x06007D49 RID: 32073 RVA: 0x00334EE4 File Offset: 0x003330E4
		private bool TryPickBinding(bool largeDisplay, out HackMeStoreZone.DisplayBinding selected)
		{
			selected = default(HackMeStoreZone.DisplayBinding);
			int num = 0;
			HashSet<int> hashSet = new HashSet<int>();
			for (int i = 0; i < this._bindings.Count; i++)
			{
				HackMeStoreZone.DisplayBinding displayBinding = this._bindings[i];
				if (displayBinding.IsValid && displayBinding.profile.fullTexture == largeDisplay && displayBinding.renderer.enabled && displayBinding.renderer.gameObject.activeInHierarchy && hashSet.Add(displayBinding.renderer.GetInstanceID()))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return false;
			}
			int num2 = Random.Range(0, num);
			hashSet.Clear();
			for (int j = 0; j < this._bindings.Count; j++)
			{
				HackMeStoreZone.DisplayBinding displayBinding2 = this._bindings[j];
				if (displayBinding2.IsValid && displayBinding2.profile.fullTexture == largeDisplay && displayBinding2.renderer.enabled && displayBinding2.renderer.gameObject.activeInHierarchy && hashSet.Add(displayBinding2.renderer.GetInstanceID()) && num2-- <= 0)
				{
					selected = displayBinding2;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007D4A RID: 32074 RVA: 0x00335014 File Offset: 0x00333214
		public int ApplyWallpaper(Texture photo, string sourceName = null)
		{
			if (photo == null)
			{
				return 0;
			}
			this.EnsureBindings();
			if (this._bindings.Count == 0)
			{
				return 0;
			}
			bool flag = this._hasActiveHack && !this._activeHackIsVideo;
			this.StopVideoBroadcast();
			this._hasActiveHack = flag;
			this._activeHackIsVideo = false;
			List<Texture2D> list = new List<Texture2D>();
			Texture2D texture2D = HackMeTextureComposer.CopyPhoto(photo);
			if (texture2D == null)
			{
				return 0;
			}
			list.Add(texture2D);
			Dictionary<HackMeStoreZone.AtlasCacheKey, Texture2D> dictionary = new Dictionary<HackMeStoreZone.AtlasCacheKey, Texture2D>();
			int num = 0;
			try
			{
				for (int i = 0; i < this._bindings.Count; i++)
				{
					HackMeStoreZone.DisplayBinding displayBinding = this._bindings[i];
					if (displayBinding.IsValid)
					{
						Texture texture = texture2D;
						if (!displayBinding.profile.fullTexture)
						{
							HackMeStoreZone.AtlasCacheKey atlasCacheKey = new HackMeStoreZone.AtlasCacheKey(displayBinding.profile.sourceTexture, displayBinding.profile.atlasUvRect, displayBinding.profile.rotate180);
							Texture2D texture2D2;
							if (!dictionary.TryGetValue(atlasCacheKey, out texture2D2) || texture2D2 == null)
							{
								texture2D2 = HackMeTextureComposer.CompositeAtlas(displayBinding.profile.sourceTexture, texture2D, displayBinding.profile.atlasUvRect, displayBinding.profile.rotate180);
								if (texture2D2 == null)
								{
									goto IL_0146;
								}
								dictionary[atlasCacheKey] = texture2D2;
								list.Add(texture2D2);
							}
							texture = texture2D2;
						}
						if (HackMeStoreZone.ApplyTexture(displayBinding, texture))
						{
							num++;
						}
					}
					IL_0146:;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[HackMe] Failed while updating '" + this.DisplayName + "': " + ex.Message, this);
			}
			if (num <= 0)
			{
				HackMeStoreZone.DestroyTextures(list);
				return 0;
			}
			this.DestroyOwnedTextures();
			this._ownedTextures.AddRange(list);
			this._hasActiveHack = true;
			this._activeHackIsVideo = false;
			this.RaiseBroadcastStarted(string.IsNullOrEmpty(sourceName) ? photo.name : sourceName, false);
			return num;
		}

		// Token: 0x06007D4B RID: 32075 RVA: 0x00335200 File Offset: 0x00333400
		public int ApplyVideo(string videoPath)
		{
			if (string.IsNullOrWhiteSpace(videoPath) || !File.Exists(videoPath))
			{
				return 0;
			}
			this.EnsureBindings();
			int num = this.CountValidBindings();
			if (num == 0)
			{
				return 0;
			}
			bool flag = this._hasActiveHack && !this._activeHackIsVideo;
			this.StopVideoBroadcast();
			this._hasActiveHack = flag;
			this._activeHackIsVideo = false;
			this._videoPreparePending = true;
			this._pendingVideoSource = videoPath;
			this._videoHost = new GameObject("[HackMe] Store Video");
			this._videoHost.hideFlags = HideFlags.DontSave;
			this._videoHost.transform.SetParent(base.transform, false);
			this._videoPlayer = this._videoHost.AddComponent<VideoPlayer>();
			this._videoPlayer.playOnAwake = false;
			this._videoPlayer.source = VideoSource.Url;
			this._videoPlayer.renderMode = VideoRenderMode.APIOnly;
			this._videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
			this._videoPlayer.waitForFirstFrame = true;
			this._videoPlayer.skipOnDrop = true;
			this._videoPlayer.isLooping = true;
			this._videoPlayer.url = videoPath;
			this._videoPlayer.prepareCompleted += this.OnVideoPrepared;
			this._videoPlayer.errorReceived += this.OnVideoError;
			this._videoPlayer.Prepare();
			return num;
		}

		// Token: 0x06007D4C RID: 32076 RVA: 0x00335348 File Offset: 0x00333548
		private void Update()
		{
			if (this._videoPlayer == null || !this._videoPlayer.isPrepared || this._videoPlayer.texture == null)
			{
				return;
			}
			Texture texture = this._videoPlayer.texture;
			if (this._lastVideoTexture != texture)
			{
				this._lastVideoTexture = texture;
				for (int i = 0; i < this._videoFullBindings.Count; i++)
				{
					HackMeStoreZone.ApplyTexture(this._videoFullBindings[i], texture);
				}
				this._lastVideoFrame = long.MinValue;
			}
			long frame = this._videoPlayer.frame;
			if (frame >= 0L && frame == this._lastVideoFrame)
			{
				return;
			}
			this._lastVideoFrame = frame;
			for (int j = 0; j < this._videoAtlasOutputs.Count; j++)
			{
				HackMeStoreZone.UpdateVideoAtlas(this._videoAtlasOutputs[j], texture);
			}
		}

		// Token: 0x06007D4D RID: 32077 RVA: 0x00335428 File Offset: 0x00333628
		private void OnVideoPrepared(VideoPlayer source)
		{
			if (source == null || source != this._videoPlayer || source.texture == null)
			{
				return;
			}
			Dictionary<HackMeStoreZone.AtlasCacheKey, HackMeStoreZone.VideoAtlasOutput> dictionary = new Dictionary<HackMeStoreZone.AtlasCacheKey, HackMeStoreZone.VideoAtlasOutput>();
			int num = 0;
			for (int i = 0; i < this._bindings.Count; i++)
			{
				HackMeStoreZone.DisplayBinding displayBinding = this._bindings[i];
				if (displayBinding.IsValid)
				{
					if (displayBinding.profile.fullTexture)
					{
						if (HackMeStoreZone.ApplyTexture(displayBinding, source.texture))
						{
							this._videoFullBindings.Add(displayBinding);
							num++;
						}
					}
					else
					{
						HackMeStoreZone.AtlasCacheKey atlasCacheKey = new HackMeStoreZone.AtlasCacheKey(displayBinding.profile.sourceTexture, displayBinding.profile.atlasUvRect, displayBinding.profile.rotate180);
						HackMeStoreZone.VideoAtlasOutput videoAtlasOutput;
						if (!dictionary.TryGetValue(atlasCacheKey, out videoAtlasOutput) || videoAtlasOutput == null)
						{
							videoAtlasOutput = this.CreateVideoAtlasOutput(displayBinding.profile);
							if (videoAtlasOutput == null)
							{
								goto IL_0102;
							}
							dictionary[atlasCacheKey] = videoAtlasOutput;
							this._videoAtlasOutputs.Add(videoAtlasOutput);
							HackMeStoreZone.UpdateVideoAtlas(videoAtlasOutput, source.texture);
						}
						if (HackMeStoreZone.ApplyTexture(displayBinding, videoAtlasOutput.atlasTexture))
						{
							num++;
						}
					}
				}
				IL_0102:;
			}
			if (num == 0)
			{
				Debug.LogWarning("[HackMe] Video prepared but '" + this.DisplayName + "' has no writable displays.", this);
				this.StopVideoBroadcast();
				return;
			}
			this.DestroyOwnedTextures();
			this._lastVideoTexture = source.texture;
			this._lastVideoFrame = source.frame;
			this._videoPreparePending = false;
			this._hasActiveHack = true;
			this._activeHackIsVideo = true;
			source.Play();
			this.RaiseBroadcastStarted(this._pendingVideoSource, true);
		}

		// Token: 0x06007D4E RID: 32078 RVA: 0x003355B8 File Offset: 0x003337B8
		private void RaiseBroadcastStarted(string sourceName, bool video)
		{
			this._activeHackProvokesReactions = HackMeStoreZone.IsProvocativePayload(sourceName, video);
			if (this.CanProvokeNpcReactions)
			{
				HackMeNpcDirector.EnsureFor(this);
			}
			try
			{
				Action<HackMeStoreZone, string, bool> broadcastStarted = HackMeStoreZone.BroadcastStarted;
				if (broadcastStarted != null)
				{
					broadcastStarted(this, sourceName ?? string.Empty, video);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex, this);
			}
		}

		// Token: 0x06007D4F RID: 32079 RVA: 0x00335618 File Offset: 0x00333818
		private void OnVideoError(VideoPlayer source, string message)
		{
			if (source != this._videoPlayer)
			{
				return;
			}
			Debug.LogWarning("[HackMe] Could not play video in '" + this.DisplayName + "': " + message, this);
			this.StopVideoBroadcast();
		}

		// Token: 0x06007D50 RID: 32080 RVA: 0x0033564C File Offset: 0x0033384C
		private HackMeStoreZone.VideoAtlasOutput CreateVideoAtlasOutput(HackMeStoreZone.DisplayProfile profile)
		{
			if (profile.sourceTexture == null)
			{
				return null;
			}
			float num = Mathf.Min(1f, 2048f / (float)Mathf.Max(profile.sourceTexture.width, profile.sourceTexture.height));
			int num2 = Mathf.Max(1, Mathf.RoundToInt((float)profile.sourceTexture.width * num));
			int num3 = Mathf.Max(1, Mathf.RoundToInt((float)profile.sourceTexture.height * num));
			int num4 = Mathf.Clamp(Mathf.RoundToInt(profile.atlasUvRect.x * (float)num2), 0, num2 - 1);
			int num5 = Mathf.Clamp(Mathf.RoundToInt(profile.atlasUvRect.y * (float)num3), 0, num3 - 1);
			int num6 = Mathf.Clamp(Mathf.RoundToInt(profile.atlasUvRect.width * (float)num2), 1, num2 - num4);
			int num7 = Mathf.Clamp(Mathf.RoundToInt(profile.atlasUvRect.height * (float)num3), 1, num3 - num5);
			RenderTexture renderTexture = HackMeStoreZone.CreateRenderTexture(num2, num3, profile.sourceTexture.name + " (HackMe Video)");
			RenderTexture renderTexture2 = HackMeStoreZone.CreateRenderTexture(num6, num7, profile.sourceTexture.name + " (HackMe Video Screen)");
			if (renderTexture == null || renderTexture2 == null)
			{
				HackMeStoreZone.ReleaseRenderTexture(renderTexture);
				HackMeStoreZone.ReleaseRenderTexture(renderTexture2);
				return null;
			}
			Graphics.Blit(profile.sourceTexture, renderTexture);
			return new HackMeStoreZone.VideoAtlasOutput(renderTexture, renderTexture2, new RectInt(num4, num5, num6, num7), profile.rotate180);
		}

		// Token: 0x06007D51 RID: 32081 RVA: 0x003357DC File Offset: 0x003339DC
		private static RenderTexture CreateRenderTexture(int width, int height, string textureName)
		{
			RenderTexture renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default)
			{
				name = textureName,
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp,
				useMipMap = false,
				autoGenerateMips = false,
				hideFlags = HideFlags.DontSave
			};
			if (renderTexture.Create())
			{
				return renderTexture;
			}
			Object.Destroy(renderTexture);
			return null;
		}

		// Token: 0x06007D52 RID: 32082 RVA: 0x00335830 File Offset: 0x00333A30
		private static void UpdateVideoAtlas(HackMeStoreZone.VideoAtlasOutput output, Texture videoFrame)
		{
			if (output == null || output.atlasTexture == null || output.screenTexture == null || videoFrame == null)
			{
				return;
			}
			Vector2 vector;
			Vector2 vector2;
			HackMeTextureComposer.CalculateCoverTransform(videoFrame, output.screenTexture.width, output.screenTexture.height, output.rotate180, out vector, out vector2);
			Graphics.Blit(videoFrame, output.screenTexture, vector, vector2);
			RectInt destination = output.destination;
			Graphics.CopyTexture(output.screenTexture, 0, 0, 0, 0, destination.width, destination.height, output.atlasTexture, 0, 0, destination.x, destination.y);
		}

		// Token: 0x06007D53 RID: 32083 RVA: 0x003358D4 File Offset: 0x00333AD4
		private int CountValidBindings()
		{
			int num = 0;
			for (int i = 0; i < this._bindings.Count; i++)
			{
				if (this._bindings[i].IsValid)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06007D54 RID: 32084 RVA: 0x00335914 File Offset: 0x00333B14
		private void StopVideoBroadcast()
		{
			if (this._videoPlayer != null)
			{
				this._videoPlayer.prepareCompleted -= this.OnVideoPrepared;
				this._videoPlayer.errorReceived -= this.OnVideoError;
				this._videoPlayer.Stop();
			}
			for (int i = 0; i < this._videoAtlasOutputs.Count; i++)
			{
				HackMeStoreZone.VideoAtlasOutput videoAtlasOutput = this._videoAtlasOutputs[i];
				if (videoAtlasOutput != null)
				{
					HackMeStoreZone.ReleaseRenderTexture(videoAtlasOutput.atlasTexture);
					HackMeStoreZone.ReleaseRenderTexture(videoAtlasOutput.screenTexture);
				}
			}
			this._videoAtlasOutputs.Clear();
			this._videoFullBindings.Clear();
			this._lastVideoTexture = null;
			this._lastVideoFrame = long.MinValue;
			this._videoPreparePending = false;
			if (this._videoHost != null)
			{
				Object.Destroy(this._videoHost);
			}
			this._videoHost = null;
			this._videoPlayer = null;
		}

		// Token: 0x06007D55 RID: 32085 RVA: 0x003359FE File Offset: 0x00333BFE
		private static void ReleaseRenderTexture(RenderTexture texture)
		{
			if (texture == null)
			{
				return;
			}
			if (texture.IsCreated())
			{
				texture.Release();
			}
			Object.Destroy(texture);
		}

		// Token: 0x06007D56 RID: 32086 RVA: 0x00335A1E File Offset: 0x00333C1E
		[ContextMenu("HackMe/Recalculate Store Bounds")]
		public void RecalculateBounds()
		{
			this._boundsReady = false;
			this.EnsureBounds();
		}

		// Token: 0x06007D57 RID: 32087 RVA: 0x00335A2D File Offset: 0x00333C2D
		[ContextMenu("HackMe/Refresh Display Cache")]
		public void RefreshDisplayCache()
		{
			this._bindingsReady = false;
			this.EnsureBindings();
		}

		// Token: 0x06007D58 RID: 32088 RVA: 0x00335A3C File Offset: 0x00333C3C
		private void EnsureBounds()
		{
			if (this._boundsReady)
			{
				return;
			}
			this._boundsReady = true;
			if (!this.calculateBoundsFromRenderers)
			{
				return;
			}
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>(true);
			bool flag = false;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			foreach (Renderer renderer in componentsInChildren)
			{
				if (!(renderer == null) && !(renderer is ParticleSystemRenderer) && !(renderer is SpriteRenderer))
				{
					Bounds bounds = renderer.bounds;
					for (int j = 0; j < 8; j++)
					{
						Vector3 vector3 = new Vector3(((j & 1) == 0) ? (-1f) : 1f, ((j & 2) == 0) ? (-1f) : 1f, ((j & 4) == 0) ? (-1f) : 1f);
						Vector3 vector4 = base.transform.InverseTransformPoint(bounds.center + Vector3.Scale(bounds.extents, vector3));
						if (!flag)
						{
							vector2 = (vector = vector4);
							flag = true;
						}
						else
						{
							vector = Vector3.Min(vector, vector4);
							vector2 = Vector3.Max(vector2, vector4);
						}
					}
				}
			}
			if (!flag)
			{
				return;
			}
			Vector3 vector5 = new Vector3(this.horizontalBoundsPadding, this.verticalBoundsPadding, this.horizontalBoundsPadding);
			vector -= vector5;
			vector2 += vector5;
			this.localBoundsCenter = (vector + vector2) * 0.5f;
			this.localBoundsSize = vector2 - vector;
		}

		// Token: 0x06007D59 RID: 32089 RVA: 0x00335BB0 File Offset: 0x00333DB0
		private void EnsureBindings()
		{
			if (this._bindingsReady)
			{
				return;
			}
			this._bindingsReady = true;
			this._bindings.Clear();
			HashSet<HackMeStoreZone.DisplaySlotKey> hashSet = new HashSet<HackMeStoreZone.DisplaySlotKey>();
			for (int i = 0; i < this.displayOverrides.Count; i++)
			{
				HackMeDisplayOverride hackMeDisplayOverride = this.displayOverrides[i];
				if (hackMeDisplayOverride != null && !(hackMeDisplayOverride.renderer == null))
				{
					Material[] sharedMaterials = hackMeDisplayOverride.renderer.sharedMaterials;
					if (hackMeDisplayOverride.materialIndex >= 0 && hackMeDisplayOverride.materialIndex < sharedMaterials.Length)
					{
						Material material = sharedMaterials[hackMeDisplayOverride.materialIndex];
						HackMeStoreZone.DisplayProfile displayProfile;
						if (!(material == null) && HackMeStoreZone.TryBuildConfiguredProfile(hackMeDisplayOverride, material, out displayProfile))
						{
							this.AddBinding(hackMeDisplayOverride.renderer, hackMeDisplayOverride.materialIndex, displayProfile, hashSet);
						}
					}
				}
			}
			if (!this.autoDiscoverKnownDisplays)
			{
				return;
			}
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
			{
				if (!(renderer == null) && !(renderer is ParticleSystemRenderer) && !(renderer is SpriteRenderer))
				{
					Material[] sharedMaterials2 = renderer.sharedMaterials;
					for (int k = 0; k < sharedMaterials2.Length; k++)
					{
						Material material2 = sharedMaterials2[k];
						HackMeStoreZone.DisplayProfile displayProfile2;
						if (HackMeStoreZone.TryGetAutomaticProfile(material2, out displayProfile2) || HackMeStoreZone.TryGetDeviceScreenProfile(renderer, k, material2, out displayProfile2))
						{
							this.AddBinding(renderer, k, displayProfile2, hashSet);
						}
					}
				}
			}
		}

		// Token: 0x06007D5A RID: 32090 RVA: 0x00335CFC File Offset: 0x00333EFC
		private void AddBinding(Renderer renderer, int materialIndex, HackMeStoreZone.DisplayProfile profile, HashSet<HackMeStoreZone.DisplaySlotKey> occupiedSlots)
		{
			HackMeStoreZone.DisplaySlotKey displaySlotKey = new HackMeStoreZone.DisplaySlotKey(renderer, materialIndex);
			if (!occupiedSlots.Add(displaySlotKey))
			{
				return;
			}
			this._bindings.Add(new HackMeStoreZone.DisplayBinding(renderer, materialIndex, profile));
		}

		// Token: 0x06007D5B RID: 32091 RVA: 0x00335D30 File Offset: 0x00333F30
		private static bool TryBuildConfiguredProfile(HackMeDisplayOverride configured, Material material, out HackMeStoreZone.DisplayProfile profile)
		{
			if (configured.mode == HackMeDisplayMode.Auto)
			{
				return HackMeStoreZone.TryGetAutomaticProfile(material, out profile);
			}
			Texture baseTexture = HackMeStoreZone.GetBaseTexture(material);
			if (configured.mode == HackMeDisplayMode.FullTexture)
			{
				profile = HackMeStoreZone.DisplayProfile.Full(baseTexture);
				return true;
			}
			if (baseTexture != null)
			{
				profile = HackMeStoreZone.DisplayProfile.Atlas(baseTexture, configured.atlasUvRect, configured.rotateAtlasRegion180);
				return true;
			}
			profile = default(HackMeStoreZone.DisplayProfile);
			return false;
		}

		// Token: 0x06007D5C RID: 32092 RVA: 0x00335D98 File Offset: 0x00333F98
		private static bool TryGetAutomaticProfile(Material material, out HackMeStoreZone.DisplayProfile profile)
		{
			profile = default(HackMeStoreZone.DisplayProfile);
			if (material == null)
			{
				return false;
			}
			Texture baseTexture = HackMeStoreZone.GetBaseTexture(material);
			if (baseTexture == null)
			{
				return false;
			}
			string text = HackMeStoreZone.NormalizeName(baseTexture.name);
			string text2 = HackMeStoreZone.NormalizeName(material.name);
			if (text == "screen" || text == "screen 2" || text == "display 1" || text2 == "screen" || text2 == "screen 2" || text2 == "display 1")
			{
				profile = HackMeStoreZone.DisplayProfile.Full(baseTexture);
				return true;
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 313093259U)
			{
				if (num <= 269297507U)
				{
					if (num != 252519888U)
					{
						if (num != 269297507U)
						{
							return false;
						}
						if (!(text == "tablet 2"))
						{
							return false;
						}
						goto IL_0257;
					}
					else
					{
						if (!(text == "tablet 3"))
						{
							return false;
						}
						profile = HackMeStoreZone.DisplayProfile.Atlas(baseTexture, new Rect(0.03f, 0.074f, 0.428f, 0.6f), true);
						return true;
					}
				}
				else if (num != 296315640U)
				{
					if (num != 313093259U)
					{
						return false;
					}
					if (!(text == "phone 4"))
					{
						return false;
					}
					goto IL_0207;
				}
				else if (!(text == "phone 5"))
				{
					return false;
				}
			}
			else if (num <= 363426116U)
			{
				if (num != 346648497U)
				{
					if (num != 363426116U)
					{
						return false;
					}
					if (!(text == "phone 1"))
					{
						return false;
					}
				}
				else
				{
					if (!(text == "phone 6"))
					{
						return false;
					}
					profile = HackMeStoreZone.DisplayProfile.Atlas(baseTexture, new Rect(0.066f, 0.143f, 0.351f, 0.71f), false);
					return true;
				}
			}
			else if (num != 396981354U)
			{
				if (num != 413758973U)
				{
					if (num != 529075761U)
					{
						return false;
					}
					if (!(text == "tablet"))
					{
						return false;
					}
					goto IL_0257;
				}
				else
				{
					if (!(text == "phone 2"))
					{
						return false;
					}
					goto IL_0207;
				}
			}
			else
			{
				if (!(text == "phone 3"))
				{
					return false;
				}
				goto IL_0207;
			}
			profile = HackMeStoreZone.DisplayProfile.Atlas(baseTexture, new Rect(0.073f, 0.14f, 0.34f, 0.711f), false);
			return true;
			IL_0207:
			profile = HackMeStoreZone.DisplayProfile.Atlas(baseTexture, new Rect(0.075f, 0.145f, 0.335f, 0.695f), false);
			return true;
			IL_0257:
			profile = HackMeStoreZone.DisplayProfile.Atlas(baseTexture, new Rect(0.12f, 0.157f, 0.352f, 0.682f), false);
			return true;
		}

		// Token: 0x06007D5D RID: 32093 RVA: 0x00336050 File Offset: 0x00334250
		private static bool TryGetDeviceScreenProfile(Renderer renderer, int materialIndex, Material material, out HackMeStoreZone.DisplayProfile profile)
		{
			profile = default(HackMeStoreZone.DisplayProfile);
			if (renderer == null || materialIndex != 1 || material == null || !HackMeStoreZone.HasDisplayDeviceAncestor(renderer.transform))
			{
				return false;
			}
			Texture baseTexture = HackMeStoreZone.GetBaseTexture(material);
			if (baseTexture == null)
			{
				return false;
			}
			profile = HackMeStoreZone.DisplayProfile.Full(baseTexture);
			return true;
		}

		// Token: 0x06007D5E RID: 32094 RVA: 0x003360A8 File Offset: 0x003342A8
		private static bool HasDisplayDeviceAncestor(Transform current)
		{
			while (current != null)
			{
				string text = HackMeStoreZone.NormalizeName(current.name);
				if (HackMeStoreZone.IsNamedInstance(text, "tv") || HackMeStoreZone.IsNamedInstance(text, "tv wall") || HackMeStoreZone.IsNamedInstance(text, "monitor") || HackMeStoreZone.IsNamedInstance(text, "monitor 3"))
				{
					return true;
				}
				current = current.parent;
			}
			return false;
		}

		// Token: 0x06007D5F RID: 32095 RVA: 0x0033610B File Offset: 0x0033430B
		private static bool IsNamedInstance(string objectName, string baseName)
		{
			return objectName == baseName || (objectName.StartsWith(baseName + " (", StringComparison.Ordinal) && objectName.EndsWith(")", StringComparison.Ordinal));
		}

		// Token: 0x06007D60 RID: 32096 RVA: 0x0033613C File Offset: 0x0033433C
		private static Texture GetBaseTexture(Material material)
		{
			if (material == null)
			{
				return null;
			}
			if (material.HasProperty("_BaseMap"))
			{
				Texture texture = material.GetTexture("_BaseMap");
				if (texture != null)
				{
					return texture;
				}
			}
			if (material.HasProperty("_BaseColorMap"))
			{
				Texture texture2 = material.GetTexture("_BaseColorMap");
				if (texture2 != null)
				{
					return texture2;
				}
			}
			if (!material.HasProperty("_MainTex"))
			{
				return null;
			}
			return material.GetTexture("_MainTex");
		}

		// Token: 0x06007D61 RID: 32097 RVA: 0x003361B8 File Offset: 0x003343B8
		private static string NormalizeName(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}
			string text = value.Trim().ToLowerInvariant();
			if (text.EndsWith(" (instance)", StringComparison.Ordinal))
			{
				text = text.Substring(0, text.Length - " (instance)".Length);
			}
			return text;
		}

		// Token: 0x06007D62 RID: 32098 RVA: 0x00336208 File Offset: 0x00334408
		private static bool ApplyTexture(HackMeStoreZone.DisplayBinding binding, Texture texture)
		{
			Material[] sharedMaterials = binding.renderer.sharedMaterials;
			if (binding.materialIndex < 0 || binding.materialIndex >= sharedMaterials.Length)
			{
				return false;
			}
			Material material = sharedMaterials[binding.materialIndex];
			if (material == null)
			{
				return false;
			}
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			binding.renderer.GetPropertyBlock(materialPropertyBlock, binding.materialIndex);
			if (!(false | HackMeStoreZone.SetTextureIfPresent(material, materialPropertyBlock, "_BaseMap", texture) | HackMeStoreZone.SetTextureIfPresent(material, materialPropertyBlock, "_BaseColorMap", texture) | HackMeStoreZone.SetTextureIfPresent(material, materialPropertyBlock, "_MainTex", texture)))
			{
				return false;
			}
			binding.renderer.SetPropertyBlock(materialPropertyBlock, binding.materialIndex);
			return true;
		}

		// Token: 0x06007D63 RID: 32099 RVA: 0x003362A5 File Offset: 0x003344A5
		private static bool SetTextureIfPresent(Material material, MaterialPropertyBlock block, string propertyName, Texture texture)
		{
			if (!material.HasProperty(propertyName))
			{
				return false;
			}
			block.SetTexture(propertyName, texture);
			return true;
		}

		// Token: 0x06007D64 RID: 32100 RVA: 0x003362BB File Offset: 0x003344BB
		private void DestroyOwnedTextures()
		{
			HackMeStoreZone.DestroyTextures(this._ownedTextures);
			this._ownedTextures.Clear();
		}

		// Token: 0x06007D65 RID: 32101 RVA: 0x003362D4 File Offset: 0x003344D4
		private static void DestroyTextures(List<Texture2D> textures)
		{
			for (int i = 0; i < textures.Count; i++)
			{
				if (textures[i] != null)
				{
					Object.Destroy(textures[i]);
				}
			}
		}

		// Token: 0x0400812B RID: 33067
		private const int MaxVideoAtlasDimension = 2048;

		// Token: 0x0400812C RID: 33068
		private static readonly List<HackMeStoreZone> ActiveZonesInternal = new List<HackMeStoreZone>();

		// Token: 0x0400812D RID: 33069
		[Header("Store volume")]
		[Tooltip("Optional authored volume. When omitted, the zone derives a local volume from its renderers.")]
		[SerializeField]
		private Collider storeVolume;

		// Token: 0x0400812E RID: 33070
		[SerializeField]
		private bool calculateBoundsFromRenderers = true;

		// Token: 0x0400812F RID: 33071
		[SerializeField]
		private Vector3 localBoundsCenter;

		// Token: 0x04008130 RID: 33072
		[SerializeField]
		private Vector3 localBoundsSize = new Vector3(10f, 5f, 10f);

		// Token: 0x04008131 RID: 33073
		[SerializeField]
		[Min(0f)]
		private float horizontalBoundsPadding = 0.6f;

		// Token: 0x04008132 RID: 33074
		[SerializeField]
		[Min(0f)]
		private float verticalBoundsPadding = 1f;

		// Token: 0x04008133 RID: 33075
		[Header("Displays")]
		[Tooltip("Automatically finds the known phone/tablet/monitor/TV textures from the electronics-store pack.")]
		[SerializeField]
		private bool autoDiscoverKnownDisplays = true;

		// Token: 0x04008134 RID: 33076
		[Tooltip("Optional additions or overrides for displays from other asset packs.")]
		[SerializeField]
		private List<HackMeDisplayOverride> displayOverrides = new List<HackMeDisplayOverride>();

		// Token: 0x04008135 RID: 33077
		[Header("Target")]
		[Tooltip("One public screen rather than an electronics store: HackMe uses screen wording for this target.")]
		[SerializeField]
		private bool singleScreenTarget;

		// Token: 0x04008137 RID: 33079
		private readonly List<HackMeStoreZone.DisplayBinding> _bindings = new List<HackMeStoreZone.DisplayBinding>();

		// Token: 0x04008138 RID: 33080
		private readonly List<Texture2D> _ownedTextures = new List<Texture2D>();

		// Token: 0x04008139 RID: 33081
		private readonly List<HackMeStoreZone.DisplayBinding> _videoFullBindings = new List<HackMeStoreZone.DisplayBinding>();

		// Token: 0x0400813A RID: 33082
		private readonly List<HackMeStoreZone.VideoAtlasOutput> _videoAtlasOutputs = new List<HackMeStoreZone.VideoAtlasOutput>();

		// Token: 0x0400813B RID: 33083
		private GameObject _videoHost;

		// Token: 0x0400813C RID: 33084
		private VideoPlayer _videoPlayer;

		// Token: 0x0400813D RID: 33085
		private Texture _lastVideoTexture;

		// Token: 0x0400813E RID: 33086
		private long _lastVideoFrame = long.MinValue;

		// Token: 0x0400813F RID: 33087
		private bool _boundsReady;

		// Token: 0x04008140 RID: 33088
		private bool _bindingsReady;

		// Token: 0x04008141 RID: 33089
		private bool _hasActiveHack;

		// Token: 0x04008142 RID: 33090
		private bool _activeHackIsVideo;

		// Token: 0x04008143 RID: 33091
		private bool _activeHackProvokesReactions;

		// Token: 0x04008144 RID: 33092
		private bool _videoPreparePending;

		// Token: 0x04008145 RID: 33093
		private string _pendingVideoSource;

		// Token: 0x02001026 RID: 4134
		private sealed class VideoAtlasOutput
		{
			// Token: 0x06007D68 RID: 32104 RVA: 0x003363B3 File Offset: 0x003345B3
			public VideoAtlasOutput(RenderTexture atlasTexture, RenderTexture screenTexture, RectInt destination, bool rotate180)
			{
				this.atlasTexture = atlasTexture;
				this.screenTexture = screenTexture;
				this.destination = destination;
				this.rotate180 = rotate180;
			}

			// Token: 0x04008146 RID: 33094
			public readonly RenderTexture atlasTexture;

			// Token: 0x04008147 RID: 33095
			public readonly RenderTexture screenTexture;

			// Token: 0x04008148 RID: 33096
			public readonly RectInt destination;

			// Token: 0x04008149 RID: 33097
			public readonly bool rotate180;
		}

		// Token: 0x02001027 RID: 4135
		private readonly struct DisplayBinding
		{
			// Token: 0x06007D69 RID: 32105 RVA: 0x003363D8 File Offset: 0x003345D8
			public DisplayBinding(Renderer renderer, int materialIndex, HackMeStoreZone.DisplayProfile profile)
			{
				this.renderer = renderer;
				this.materialIndex = materialIndex;
				this.profile = profile;
			}

			// Token: 0x17001434 RID: 5172
			// (get) Token: 0x06007D6A RID: 32106 RVA: 0x003363EF File Offset: 0x003345EF
			public bool IsValid
			{
				get
				{
					return this.renderer != null && this.materialIndex >= 0;
				}
			}

			// Token: 0x0400814A RID: 33098
			public readonly Renderer renderer;

			// Token: 0x0400814B RID: 33099
			public readonly int materialIndex;

			// Token: 0x0400814C RID: 33100
			public readonly HackMeStoreZone.DisplayProfile profile;
		}

		// Token: 0x02001028 RID: 4136
		private readonly struct DisplayProfile
		{
			// Token: 0x06007D6B RID: 32107 RVA: 0x0033640D File Offset: 0x0033460D
			private DisplayProfile(Texture sourceTexture, bool fullTexture, Rect atlasUvRect, bool rotate180)
			{
				this.sourceTexture = sourceTexture;
				this.fullTexture = fullTexture;
				this.atlasUvRect = atlasUvRect;
				this.rotate180 = rotate180;
			}

			// Token: 0x06007D6C RID: 32108 RVA: 0x0033642C File Offset: 0x0033462C
			public static HackMeStoreZone.DisplayProfile Full(Texture source)
			{
				return new HackMeStoreZone.DisplayProfile(source, true, new Rect(0f, 0f, 1f, 1f), false);
			}

			// Token: 0x06007D6D RID: 32109 RVA: 0x0033644F File Offset: 0x0033464F
			public static HackMeStoreZone.DisplayProfile Atlas(Texture source, Rect rect, bool rotate180 = false)
			{
				return new HackMeStoreZone.DisplayProfile(source, false, rect, rotate180);
			}

			// Token: 0x0400814D RID: 33101
			public readonly Texture sourceTexture;

			// Token: 0x0400814E RID: 33102
			public readonly bool fullTexture;

			// Token: 0x0400814F RID: 33103
			public readonly Rect atlasUvRect;

			// Token: 0x04008150 RID: 33104
			public readonly bool rotate180;
		}

		// Token: 0x02001029 RID: 4137
		private readonly struct DisplaySlotKey : IEquatable<HackMeStoreZone.DisplaySlotKey>
		{
			// Token: 0x06007D6E RID: 32110 RVA: 0x0033645A File Offset: 0x0033465A
			public DisplaySlotKey(Renderer renderer, int materialIndex)
			{
				this._rendererId = ((renderer != null) ? renderer.GetInstanceID() : 0);
				this._materialIndex = materialIndex;
			}

			// Token: 0x06007D6F RID: 32111 RVA: 0x0033647B File Offset: 0x0033467B
			public bool Equals(HackMeStoreZone.DisplaySlotKey other)
			{
				return this._rendererId == other._rendererId && this._materialIndex == other._materialIndex;
			}

			// Token: 0x06007D70 RID: 32112 RVA: 0x0033649C File Offset: 0x0033469C
			public override bool Equals(object obj)
			{
				if (obj is HackMeStoreZone.DisplaySlotKey)
				{
					HackMeStoreZone.DisplaySlotKey displaySlotKey = (HackMeStoreZone.DisplaySlotKey)obj;
					return this.Equals(displaySlotKey);
				}
				return false;
			}

			// Token: 0x06007D71 RID: 32113 RVA: 0x003364C1 File Offset: 0x003346C1
			public override int GetHashCode()
			{
				return (this._rendererId * 397) ^ this._materialIndex;
			}

			// Token: 0x04008151 RID: 33105
			private readonly int _rendererId;

			// Token: 0x04008152 RID: 33106
			private readonly int _materialIndex;
		}

		// Token: 0x0200102A RID: 4138
		private readonly struct AtlasCacheKey : IEquatable<HackMeStoreZone.AtlasCacheKey>
		{
			// Token: 0x06007D72 RID: 32114 RVA: 0x003364D6 File Offset: 0x003346D6
			public AtlasCacheKey(Texture source, Rect rect, bool rotate180)
			{
				this._textureId = ((source != null) ? source.GetInstanceID() : 0);
				this._rect = rect;
				this._rotate180 = rotate180;
			}

			// Token: 0x06007D73 RID: 32115 RVA: 0x00336500 File Offset: 0x00334700
			public bool Equals(HackMeStoreZone.AtlasCacheKey other)
			{
				return this._textureId == other._textureId && this._rect.Equals(other._rect) && this._rotate180 == other._rotate180;
			}

			// Token: 0x06007D74 RID: 32116 RVA: 0x00336544 File Offset: 0x00334744
			public override bool Equals(object obj)
			{
				if (obj is HackMeStoreZone.AtlasCacheKey)
				{
					HackMeStoreZone.AtlasCacheKey atlasCacheKey = (HackMeStoreZone.AtlasCacheKey)obj;
					return this.Equals(atlasCacheKey);
				}
				return false;
			}

			// Token: 0x06007D75 RID: 32117 RVA: 0x0033656C File Offset: 0x0033476C
			public override int GetHashCode()
			{
				return (((this._textureId * 397) ^ this._rect.GetHashCode()) * 397) ^ this._rotate180.GetHashCode();
			}

			// Token: 0x04008153 RID: 33107
			private readonly int _textureId;

			// Token: 0x04008154 RID: 33108
			private readonly Rect _rect;

			// Token: 0x04008155 RID: 33109
			private readonly bool _rotate180;
		}
	}
}
