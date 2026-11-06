using System;
using Features.Inventory;
using Features.Inventory.Wearable;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Features.WardrobeFeedback
{
	// Token: 0x02000B53 RID: 2899
	public class WardrobeFeedback : MonoBehaviour
	{
		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x0600522C RID: 21036 RVA: 0x001FCF66 File Offset: 0x001FB166
		public static int ActiveParticleCount
		{
			get
			{
				if (!(WardrobeFeedback._instance != null) || !(WardrobeFeedback._instance._particles != null))
				{
					return 0;
				}
				return WardrobeFeedback._instance._particles.particleCount;
			}
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x001FCF98 File Offset: 0x001FB198
		public static IDisposable SuppressInventoryRemovalFeedback()
		{
			WardrobeFeedback._inventoryRemovalFeedbackSuppressionDepth++;
			return new WardrobeFeedback.InventoryRemovalFeedbackScope();
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x001FCFAB File Offset: 0x001FB1AB
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (WardrobeFeedback._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("WardrobeFeedback");
			Object.DontDestroyOnLoad(gameObject);
			WardrobeFeedback._instance = gameObject.AddComponent<WardrobeFeedback>();
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x001FCFD8 File Offset: 0x001FB1D8
		private void Awake()
		{
			this._audio = base.gameObject.AddComponent<AudioSource>();
			this._audio.playOnAwake = false;
			this._audio.spatialBlend = 0f;
			this._audio.volume = 1f;
			this._popPickup = WardrobeFeedback.SynthPop("WF_PopPickup", 380f, 820f, 0.11f, 0.3f);
			this._popWear = WardrobeFeedback.SynthPop("WF_PopWear", 260f, 620f, 0.16f, 0.35f);
			this._popUnwear = WardrobeFeedback.SynthPop("WF_PopUnwear", 620f, 240f, 0.16f, 0.35f);
			this._comboReveal = WardrobeFeedback.SynthPop("WF_ComboReveal", 300f, 1320f, 0.34f, 0.48f);
			this.BuildParticles();
			this._muteUntil = Time.unscaledTime + this.bootMute;
			SceneManager.sceneLoaded += this.HandleSceneLoaded;
		}

		// Token: 0x06005230 RID: 21040 RVA: 0x001FD0DC File Offset: 0x001FB2DC
		private void OnDestroy()
		{
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			this.Unsubscribe();
			if (this._particleMat != null)
			{
				Object.Destroy(this._particleMat);
			}
			if (this._circleTex != null)
			{
				Object.Destroy(this._circleTex);
			}
			if (WardrobeFeedback._instance == this)
			{
				WardrobeFeedback._instance = null;
			}
		}

		// Token: 0x06005231 RID: 21041 RVA: 0x001FD145 File Offset: 0x001FB345
		private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this._muteUntil = Time.unscaledTime + this.bootMute;
		}

		// Token: 0x06005232 RID: 21042 RVA: 0x001FD15C File Offset: 0x001FB35C
		private void Update()
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this._wear))
			{
				if (this._audio != null && this._audio.isPlaying)
				{
					this._audio.Stop();
				}
				if (this._particles != null && this._particles.particleCount > 0)
				{
					this._particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
				return;
			}
			if ((this._inventory == null || this._wear == null) && Time.unscaledTime >= this._nextResolveTime)
			{
				this._nextResolveTime = Time.unscaledTime + 1f;
				this.ResolveAndSubscribe();
			}
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x001FD208 File Offset: 0x001FB408
		private void ResolveAndSubscribe()
		{
			PlayerInventoryManager playerInventoryManager = Object.FindObjectOfType<PlayerInventoryManager>();
			WearableUseController wearableUseController = Object.FindObjectOfType<WearableUseController>();
			if (playerInventoryManager == this._inventory && wearableUseController == this._wear)
			{
				return;
			}
			this.Unsubscribe();
			this._inventory = playerInventoryManager;
			this._wear = wearableUseController;
			if (this._inventory != null)
			{
				this._inventory.OnItemAdded += this.HandleItemAdded;
				this._inventory.OnClothAdded += this.HandleClothAdded;
				this._inventory.OnItemRemoved += this.HandleItemRemoved;
				this._inventory.OnItemStackChanged += this.HandleItemStackChanged;
				this._inventory.OnClothRemoved += this.HandleClothRemoved;
				this._playerAnimator = this._inventory.GetComponentInParent<Animator>();
				if (this._playerAnimator == null)
				{
					this._playerAnimator = this._inventory.transform.root.GetComponentInChildren<Animator>();
				}
			}
			if (this._wear != null)
			{
				this._wear.OnItemWorn += this.HandleItemWorn;
				this._wear.OnItemUnworn += this.HandleItemUnworn;
			}
			if (this._inventory != null || this._wear != null)
			{
				this._muteUntil = Mathf.Max(this._muteUntil, Time.unscaledTime + 0.5f);
			}
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x001FD384 File Offset: 0x001FB584
		private void Unsubscribe()
		{
			if (this._inventory != null)
			{
				this._inventory.OnItemAdded -= this.HandleItemAdded;
				this._inventory.OnClothAdded -= this.HandleClothAdded;
				this._inventory.OnItemRemoved -= this.HandleItemRemoved;
				this._inventory.OnItemStackChanged -= this.HandleItemStackChanged;
				this._inventory.OnClothRemoved -= this.HandleClothRemoved;
			}
			if (this._wear != null)
			{
				this._wear.OnItemWorn -= this.HandleItemWorn;
				this._wear.OnItemUnworn -= this.HandleItemUnworn;
			}
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x001FD44E File Offset: 0x001FB64E
		private void HandleItemAdded(int slot, InventoryItemSO item)
		{
			this.Play(this._popPickup, this.pickupColor, 1f, null);
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x001FD44E File Offset: 0x001FB64E
		private void HandleClothAdded(int slot, InventoryItemSO item)
		{
			this.Play(this._popPickup, this.pickupColor, 1f, null);
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x001FD468 File Offset: 0x001FB668
		private void HandleItemWorn(WearableItemSO item)
		{
			this.Play(this._popWear, this.wearColor, 1f, item);
		}

		// Token: 0x06005238 RID: 21048 RVA: 0x001FD482 File Offset: 0x001FB682
		private void HandleItemUnworn(WearableItemSO item)
		{
			this.Play(this._popUnwear, this.unwearColor, 1f, item);
		}

		// Token: 0x06005239 RID: 21049 RVA: 0x001FD49C File Offset: 0x001FB69C
		private void HandleItemRemoved(int slot)
		{
			if (WardrobeFeedback._inventoryRemovalFeedbackSuppressionDepth > 0)
			{
				return;
			}
			this.Play(this._popUnwear, this.unwearColor, 0.85f, null);
		}

		// Token: 0x0600523A RID: 21050 RVA: 0x001FD4BF File Offset: 0x001FB6BF
		private void HandleClothRemoved(int slot)
		{
			this.Play(this._popUnwear, this.unwearColor, 0.85f, null);
		}

		// Token: 0x0600523B RID: 21051 RVA: 0x001FD4D9 File Offset: 0x001FB6D9
		private void HandleItemStackChanged(int slot, int previous, int current)
		{
			if (current > previous)
			{
				this.Play(this._popPickup, this.pickupColor, 1f, null);
			}
		}

		// Token: 0x0600523C RID: 21052 RVA: 0x001FD4F8 File Offset: 0x001FB6F8
		public static bool PlayCosmeticApply(Color color)
		{
			if (WardrobeFeedback._instance == null || WardrobeGameplayContext.IsPresentationOnly(WardrobeFeedback._instance._wear))
			{
				return false;
			}
			if (WardrobeFeedback._instance._playerAnimator == null)
			{
				WardrobeFeedback._instance.ResolveAndSubscribe();
			}
			WardrobeFeedback._instance._lastFeedbackTime = Time.unscaledTime;
			if (WardrobeFeedback._instance._audio != null && WardrobeFeedback._instance._popWear != null)
			{
				WardrobeFeedback._instance._audio.PlayOneShot(WardrobeFeedback._instance._popWear, Mathf.Clamp01(WardrobeFeedback._instance.volume));
			}
			WardrobeFeedback._instance.EmitPuff(color, null);
			return true;
		}

		// Token: 0x0600523D RID: 21053 RVA: 0x001FD5AC File Offset: 0x001FB7AC
		public static bool PlayComboReveal(Color color)
		{
			if (WardrobeFeedback._instance == null || WardrobeGameplayContext.IsPresentationOnly(WardrobeFeedback._instance._wear))
			{
				return false;
			}
			if (WardrobeFeedback._instance._playerAnimator == null)
			{
				WardrobeFeedback._instance.ResolveAndSubscribe();
			}
			WardrobeFeedback._instance._lastFeedbackTime = Time.unscaledTime;
			if (WardrobeFeedback._instance._audio != null && WardrobeFeedback._instance._comboReveal != null)
			{
				WardrobeFeedback._instance._audio.PlayOneShot(WardrobeFeedback._instance._comboReveal, Mathf.Clamp01(WardrobeFeedback._instance.volume * 1.15f));
			}
			int num = WardrobeFeedback._instance.burstCount;
			WardrobeFeedback._instance.burstCount = Mathf.Max(num, num * 2);
			WardrobeFeedback._instance.EmitPuff(color, null);
			WardrobeFeedback._instance.burstCount = num;
			return true;
		}

		// Token: 0x0600523E RID: 21054 RVA: 0x001FD68C File Offset: 0x001FB88C
		public static bool PlayItemRefreshAt(Vector3 worldPosition, Color color)
		{
			if (WardrobeFeedback._instance == null || WardrobeGameplayContext.IsPresentationOnly(WardrobeFeedback._instance._wear))
			{
				return false;
			}
			if (WardrobeFeedback._instance._audio != null && WardrobeFeedback._instance._popPickup != null)
			{
				WardrobeFeedback._instance._audio.PlayOneShot(WardrobeFeedback._instance._popPickup, Mathf.Clamp01(WardrobeFeedback._instance.volume * 0.75f));
			}
			if (WardrobeFeedback._instance._particles == null)
			{
				return true;
			}
			WardrobeFeedback._instance._particles.main.startColor = color;
			WardrobeFeedback._instance._particles.transform.position = worldPosition;
			WardrobeFeedback._instance._particles.Emit(Mathf.Max(1, WardrobeFeedback._instance.burstCount));
			return true;
		}

		// Token: 0x0600523F RID: 21055 RVA: 0x001FD774 File Offset: 0x001FB974
		public static bool PlayItemDiscardAt(Vector3 worldPosition, Color color)
		{
			if (WardrobeFeedback._instance == null || WardrobeGameplayContext.IsPresentationOnly(WardrobeFeedback._instance._wear))
			{
				return false;
			}
			WardrobeFeedback._instance._lastFeedbackTime = Time.unscaledTime;
			if (WardrobeFeedback._instance._audio != null && WardrobeFeedback._instance._popUnwear != null)
			{
				WardrobeFeedback._instance._audio.PlayOneShot(WardrobeFeedback._instance._popUnwear, Mathf.Clamp01(WardrobeFeedback._instance.volume * 0.9f));
			}
			if (WardrobeFeedback._instance._particles == null)
			{
				return true;
			}
			WardrobeFeedback._instance._particles.main.startColor = color;
			WardrobeFeedback._instance._particles.transform.position = worldPosition;
			WardrobeFeedback._instance._particles.Emit(Mathf.Max(1, WardrobeFeedback._instance.burstCount + 6));
			return true;
		}

		// Token: 0x06005240 RID: 21056 RVA: 0x001FD86C File Offset: 0x001FBA6C
		private void Play(AudioClip clip, Color color, float volumeScale, WearableItemSO zoneItem)
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this._wear))
			{
				return;
			}
			float unscaledTime = Time.unscaledTime;
			if (unscaledTime < this._muteUntil)
			{
				return;
			}
			if (unscaledTime - this._lastFeedbackTime < this.cooldown)
			{
				return;
			}
			this._lastFeedbackTime = unscaledTime;
			if (this._audio != null && clip != null)
			{
				this._audio.PlayOneShot(clip, Mathf.Clamp01(this.volume * volumeScale));
			}
			this.EmitPuff(color, zoneItem);
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x001FD8E8 File Offset: 0x001FBAE8
		private void EmitPuff(Color color, WearableItemSO zoneItem)
		{
			if (this._particles == null)
			{
				return;
			}
			this._particles.main.startColor = color;
			WardrobeFeedback.BodyZone bodyZone = WardrobeFeedback.ResolveZone(zoneItem);
			if (bodyZone == WardrobeFeedback.BodyZone.Feet)
			{
				Vector3 vector = this.ResolveBonePosition(HumanBodyBones.LeftFoot);
				Vector3 vector2 = this.ResolveBonePosition(HumanBodyBones.RightFoot);
				int num = Mathf.Max(1, this.burstCount / 2);
				this._particles.transform.position = vector;
				this._particles.Emit(num);
				this._particles.transform.position = vector2;
				this._particles.Emit(num);
				return;
			}
			this._particles.transform.position = this.ResolveZonePosition(bodyZone);
			this._particles.Emit(Mathf.Max(1, this.burstCount));
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x001FD9B4 File Offset: 0x001FBBB4
		private static WardrobeFeedback.BodyZone ResolveZone(WearableItemSO item)
		{
			string text = ((item != null) ? item.mcStylePartId : null);
			if (string.IsNullOrEmpty(text))
			{
				return WardrobeFeedback.BodyZone.Chest;
			}
			if (text.Contains("Shoes") || text.Contains("Socks"))
			{
				return WardrobeFeedback.BodyZone.Feet;
			}
			if (text.Contains("Underwear") || text.Contains("Bottom"))
			{
				return WardrobeFeedback.BodyZone.Hips;
			}
			if (text.Contains("Mask") || text.Contains("Blindfold") || text.Contains("HeadBag") || text.Contains("Eyewear") || text.Contains("Headpin") || text.Contains("Hair"))
			{
				return WardrobeFeedback.BodyZone.Head;
			}
			return WardrobeFeedback.BodyZone.Chest;
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x001FDA67 File Offset: 0x001FBC67
		private Vector3 ResolveZonePosition(WardrobeFeedback.BodyZone zone)
		{
			if (zone == WardrobeFeedback.BodyZone.Hips)
			{
				return this.ResolveBonePosition(HumanBodyBones.Hips);
			}
			if (zone != WardrobeFeedback.BodyZone.Head)
			{
				return this.ResolveBonePosition(HumanBodyBones.Chest);
			}
			return this.ResolveBonePosition(HumanBodyBones.Head);
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x001FDA8C File Offset: 0x001FBC8C
		private Vector3 ResolveBonePosition(HumanBodyBones bone)
		{
			if (this._playerAnimator != null && this._playerAnimator.isHuman)
			{
				Transform boneTransform = this._playerAnimator.GetBoneTransform(bone);
				if (boneTransform != null)
				{
					return boneTransform.position;
				}
			}
			float num = ((bone == HumanBodyBones.Head) ? 1.55f : ((bone == HumanBodyBones.Hips) ? 0.9f : ((bone == HumanBodyBones.LeftFoot || bone == HumanBodyBones.RightFoot) ? 0.1f : 1.15f)));
			Transform transform = ((this._inventory != null) ? this._inventory.transform.root : ((this._wear != null) ? this._wear.transform.root : null));
			if (!(transform != null))
			{
				return base.transform.position;
			}
			return transform.position + Vector3.up * num;
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x001FDB6C File Offset: 0x001FBD6C
		private static AudioClip SynthPop(string name, float f0, float f1, float duration, float harmonic2)
		{
			int num = Mathf.CeilToInt(44100f * duration);
			float[] array = new float[num];
			double num2 = 0.0;
			for (int i = 0; i < num; i++)
			{
				float num3 = (float)i / (float)num;
				float num4 = f0 * Mathf.Pow(f1 / f0, num3);
				num2 += 6.283185307179586 * (double)num4 / 44100.0;
				float num5 = (float)i / 44100f;
				float num6 = Mathf.Clamp01(num5 / 0.003f);
				float num7 = Mathf.Exp(-num5 * (5f / duration));
				float num8 = (float)(Math.Sin(num2) + (double)harmonic2 * Math.Sin(2.0 * num2));
				array[i] = num8 * num6 * num7 * 0.8f;
			}
			int num9 = Math.Min(num, 128);
			for (int j = 0; j < num9; j++)
			{
				array[num - 1 - j] *= (float)j / (float)num9;
			}
			AudioClip audioClip = AudioClip.Create(name, num, 1, 44100, false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x06005246 RID: 21062 RVA: 0x001FDC80 File Offset: 0x001FBE80
		private void BuildParticles()
		{
			GameObject gameObject = new GameObject("WardrobePopFX");
			gameObject.transform.SetParent(base.transform, false);
			this._particles = gameObject.AddComponent<ParticleSystem>();
			this._particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			ParticleSystem.MainModule main = this._particles.main;
			main.loop = false;
			main.playOnAwake = false;
			main.simulationSpace = ParticleSystemSimulationSpace.World;
			main.startLifetime = new ParticleSystem.MinMaxCurve(0.25f, 0.45f);
			main.startSpeed = new ParticleSystem.MinMaxCurve(0.7f, 1.5f);
			main.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.1f);
			main.gravityModifier = -0.06f;
			main.maxParticles = 128;
			this._particles.emission.rateOverTime = 0f;
			ParticleSystem.ShapeModule shape = this._particles.shape;
			shape.enabled = true;
			shape.shapeType = ParticleSystemShapeType.Sphere;
			shape.radius = 0.12f;
			ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = this._particles.sizeOverLifetime;
			sizeOverLifetime.enabled = true;
			AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.4f),
				new Keyframe(0.15f, 1f),
				new Keyframe(1f, 0f)
			});
			sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, animationCurve);
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = this._particles.colorOverLifetime;
			colorOverLifetime.enabled = true;
			Gradient gradient = new Gradient();
			gradient.SetKeys(new GradientColorKey[]
			{
				new GradientColorKey(Color.white, 0f),
				new GradientColorKey(Color.white, 1f)
			}, new GradientAlphaKey[]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(0.9f, 0.4f),
				new GradientAlphaKey(0f, 1f)
			});
			colorOverLifetime.color = gradient;
			this._circleTex = WardrobeFeedback.MakeSoftCircleTexture(64);
			this._particleMat = WardrobeFeedback.MakeParticleMaterial(this._circleTex);
			this._particlesRenderer = gameObject.GetComponent<ParticleSystemRenderer>();
			this._particlesRenderer.renderMode = ParticleSystemRenderMode.Billboard;
			this._particlesRenderer.material = this._particleMat;
			this._particlesRenderer.shadowCastingMode = ShadowCastingMode.Off;
			this._particlesRenderer.receiveShadows = false;
		}

		// Token: 0x06005247 RID: 21063 RVA: 0x001FDF0C File Offset: 0x001FC10C
		private static Texture2D MakeSoftCircleTexture(int size)
		{
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, false)
			{
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear,
				hideFlags = HideFlags.HideAndDontSave
			};
			float num = (float)(size - 1) * 0.5f;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					float num2 = Vector2.Distance(new Vector2((float)j, (float)i), new Vector2(num, num)) / num;
					float num3 = Mathf.SmoothStep(1f, 0f, Mathf.Clamp01(num2));
					texture2D.SetPixel(j, i, new Color(1f, 1f, 1f, num3 * num3));
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06005248 RID: 21064 RVA: 0x001FDFB8 File Offset: 0x001FC1B8
		private static Material MakeParticleMaterial(Texture2D tex)
		{
			Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
			if (shader == null)
			{
				shader = Shader.Find("Particles/Standard Unlit");
			}
			Material material = new Material(shader)
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			if (material.HasProperty("_BaseMap"))
			{
				material.SetTexture("_BaseMap", tex);
			}
			else
			{
				material.mainTexture = tex;
			}
			if (material.HasProperty("_BaseColor"))
			{
				material.SetColor("_BaseColor", Color.white);
			}
			if (material.HasProperty("_Surface"))
			{
				material.SetFloat("_Surface", 1f);
			}
			material.SetOverrideTag("RenderType", "Transparent");
			if (material.HasProperty("_SrcBlend"))
			{
				material.SetInt("_SrcBlend", 5);
			}
			if (material.HasProperty("_DstBlend"))
			{
				material.SetInt("_DstBlend", 1);
			}
			if (material.HasProperty("_ZWrite"))
			{
				material.SetInt("_ZWrite", 0);
			}
			material.renderQueue = 3000;
			return material;
		}

		// Token: 0x040052BB RID: 21179
		private static WardrobeFeedback _instance;

		// Token: 0x040052BC RID: 21180
		private static int _inventoryRemovalFeedbackSuppressionDepth;

		// Token: 0x040052BD RID: 21181
		[Header("Audio")]
		[Tooltip("Volume dei pop.")]
		public float volume = 0.55f;

		// Token: 0x040052BE RID: 21182
		[Tooltip("Cooldown globale tra due feedback (unisce gli eventi a raffica in UN pop).")]
		public float cooldown = 0.15f;

		// Token: 0x040052BF RID: 21183
		[Tooltip("Secondi di mute dopo boot/cambio scena (ignora il sync iniziale del wardrobe).")]
		public float bootMute = 2f;

		// Token: 0x040052C0 RID: 21184
		[Header("Particelle")]
		[Tooltip("Quante particelle per sbuffo.")]
		public int burstCount = 14;

		// Token: 0x040052C1 RID: 21185
		public Color pickupColor = new Color(0.75f, 0.9f, 1f, 1f);

		// Token: 0x040052C2 RID: 21186
		public Color wearColor = new Color(1f, 0.95f, 0.75f, 1f);

		// Token: 0x040052C3 RID: 21187
		public Color unwearColor = new Color(1f, 0.75f, 0.85f, 1f);

		// Token: 0x040052C4 RID: 21188
		private PlayerInventoryManager _inventory;

		// Token: 0x040052C5 RID: 21189
		private WearableUseController _wear;

		// Token: 0x040052C6 RID: 21190
		private Animator _playerAnimator;

		// Token: 0x040052C7 RID: 21191
		private AudioSource _audio;

		// Token: 0x040052C8 RID: 21192
		private AudioClip _popPickup;

		// Token: 0x040052C9 RID: 21193
		private AudioClip _popWear;

		// Token: 0x040052CA RID: 21194
		private AudioClip _popUnwear;

		// Token: 0x040052CB RID: 21195
		private AudioClip _comboReveal;

		// Token: 0x040052CC RID: 21196
		private ParticleSystem _particles;

		// Token: 0x040052CD RID: 21197
		private ParticleSystemRenderer _particlesRenderer;

		// Token: 0x040052CE RID: 21198
		private Material _particleMat;

		// Token: 0x040052CF RID: 21199
		private Texture2D _circleTex;

		// Token: 0x040052D0 RID: 21200
		private float _muteUntil;

		// Token: 0x040052D1 RID: 21201
		private float _lastFeedbackTime = -999f;

		// Token: 0x040052D2 RID: 21202
		private float _nextResolveTime;

		// Token: 0x02000B54 RID: 2900
		private sealed class InventoryRemovalFeedbackScope : IDisposable
		{
			// Token: 0x0600524A RID: 21066 RVA: 0x001FE15C File Offset: 0x001FC35C
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				WardrobeFeedback._inventoryRemovalFeedbackSuppressionDepth = Mathf.Max(0, WardrobeFeedback._inventoryRemovalFeedbackSuppressionDepth - 1);
			}

			// Token: 0x040052D3 RID: 21203
			private bool _disposed;
		}

		// Token: 0x02000B55 RID: 2901
		private enum BodyZone
		{
			// Token: 0x040052D5 RID: 21205
			Chest,
			// Token: 0x040052D6 RID: 21206
			Hips,
			// Token: 0x040052D7 RID: 21207
			Feet,
			// Token: 0x040052D8 RID: 21208
			Head
		}
	}
}
