using System;
using System.Collections.Generic;
using Features.Audio;
using Features.MainMenu;
using Invector.vCharacterController;
using UnityEngine;

namespace PoolWaterTools
{
	// Token: 0x0200072A RID: 1834
	[DisallowMultipleComponent]
	[RequireComponent(typeof(BoxCollider))]
	public sealed class ShallowPoolWater : MonoBehaviour
	{
		// Token: 0x06002F7F RID: 12159 RVA: 0x0010CF64 File Offset: 0x0010B164
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			switch (field)
			{
			case UniversityGameplayField.ShallowPoolWater_footstepSplash:
				return this.footstepSplash;
			case UniversityGameplayField.ShallowPoolWater_footstepVariations:
				return this.footstepVariations[elementIndex];
			case UniversityGameplayField.ShallowPoolWater_ripplePrefab:
				return this.ripplePrefab;
			case UniversityGameplayField.ShallowPoolWater_splashPrefab:
				return this.splashPrefab;
			default:
				throw new InvalidOperationException("Unsupported ShallowPoolWater POI binding: " + field.ToString());
			}
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x0010CFC8 File Offset: 0x0010B1C8
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			switch (field)
			{
			case UniversityGameplayField.ShallowPoolWater_footstepSplash:
				this.footstepSplash = (AudioClip)asset;
				return;
			case UniversityGameplayField.ShallowPoolWater_footstepVariations:
				this.footstepVariations[elementIndex] = (AudioClip)asset;
				return;
			case UniversityGameplayField.ShallowPoolWater_ripplePrefab:
				this.ripplePrefab = (GameObject)asset;
				return;
			case UniversityGameplayField.ShallowPoolWater_splashPrefab:
				this.splashPrefab = (GameObject)asset;
				return;
			default:
				throw new InvalidOperationException("Unsupported ShallowPoolWater POI binding: " + field.ToString());
			}
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x0010D042 File Offset: 0x0010B242
		internal int GetPoiGameplayElementCount(UniversityGameplayField field)
		{
			if (field != UniversityGameplayField.ShallowPoolWater_footstepVariations)
			{
				throw new InvalidOperationException("Unsupported ShallowPoolWater indexed POI binding: " + field.ToString());
			}
			if (this.footstepVariations == null)
			{
				return 0;
			}
			return this.footstepVariations.Length;
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x0010D078 File Offset: 0x0010B278
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRecoveryVolumes()
		{
			ShallowPoolWater.RecoveryVolumes.Clear();
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x0010D084 File Offset: 0x0010B284
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void SeedRecoveryVolumes()
		{
			foreach (ShallowPoolWater shallowPoolWater in Object.FindObjectsByType<ShallowPoolWater>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				ShallowPoolWater.RecoveryVolumes.Add(shallowPoolWater);
			}
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x0010D0B8 File Offset: 0x0010B2B8
		internal static void AppendRecoveryVolumes(List<Collider> destination)
		{
			foreach (ShallowPoolWater shallowPoolWater in ShallowPoolWater.RecoveryVolumes)
			{
				if (!(shallowPoolWater == null) && shallowPoolWater.gameObject.activeInHierarchy)
				{
					Collider collider = ((shallowPoolWater.waterVolume != null) ? shallowPoolWater.waterVolume : shallowPoolWater.GetComponent<Collider>());
					if (collider != null && collider.enabled && collider.gameObject.activeInHierarchy && !destination.Contains(collider))
					{
						destination.Add(collider);
					}
				}
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06002F85 RID: 12165 RVA: 0x0010D164 File Offset: 0x0010B364
		public BoxCollider WaterVolume
		{
			get
			{
				return this.waterVolume;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06002F86 RID: 12166 RVA: 0x0010D16C File Offset: 0x0010B36C
		public float SurfaceY
		{
			get
			{
				if (!(this.waterVolume != null))
				{
					return base.transform.position.y;
				}
				return this.waterVolume.transform.TransformPoint(this.waterVolume.center + Vector3.up * this.waterVolume.size.y * 0.5f).y;
			}
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x0010D1E1 File Offset: 0x0010B3E1
		public void Configure(BoxCollider volume, GameObject ripple, GameObject splash, AudioClip sound, AudioClip[] variations = null)
		{
			this.waterVolume = volume;
			this.ripplePrefab = ripple;
			this.splashPrefab = splash;
			this.footstepSplash = sound;
			this.footstepVariations = variations;
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x0010D208 File Offset: 0x0010B408
		private void Awake()
		{
			ShallowPoolWater.RecoveryVolumes.Add(this);
			if (this.waterVolume == null)
			{
				this.waterVolume = base.GetComponent<BoxCollider>();
			}
			this.rippleTemplate = this.CreateVisualTemplate(this.ripplePrefab);
			this.splashTemplate = this.CreateVisualTemplate(this.splashPrefab);
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x0010D260 File Offset: 0x0010B460
		private AudioSource CreateAudioSource(string sourceName)
		{
			GameObject gameObject = new GameObject(sourceName);
			gameObject.transform.SetParent(base.transform, false);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.spatialBlend = 1f;
			audioSource.dopplerLevel = 0f;
			audioSource.minDistance = 2.5f;
			audioSource.maxDistance = 18f;
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			GameAudioRouter.Route(audioSource, GameAudioRouter.Channel.Sfx);
			return audioSource;
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x0010D2CC File Offset: 0x0010B4CC
		public static bool TryPlayFootstep(Transform character, float speed)
		{
			if (character == null || speed < 0.1f)
			{
				return false;
			}
			foreach (ShallowPoolWater shallowPoolWater in ShallowPoolWater.RecoveryVolumes)
			{
				if (!(shallowPoolWater == null) && shallowPoolWater.isActiveAndEnabled && !(shallowPoolWater.waterVolume == null) && shallowPoolWater.waterVolume.enabled && shallowPoolWater.ContainsFeet(character.position))
				{
					float num;
					if (!shallowPoolWater.animatedContacts.TryGetValue(character, out num) || Time.time - num >= 0.12f)
					{
						shallowPoolWater.animatedContacts[character] = Time.time;
						shallowPoolWater.PlayContact(character.position, speed, false);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x0010D3B0 File Offset: 0x0010B5B0
		public bool ContainsFeet(Vector3 feet)
		{
			if (this.waterVolume == null)
			{
				return false;
			}
			Vector3 vector = this.waterVolume.transform.InverseTransformPoint(feet) - this.waterVolume.center;
			Vector3 vector2 = this.waterVolume.size * 0.5f;
			return Mathf.Abs(vector.x) <= vector2.x && Mathf.Abs(vector.z) <= vector2.z && vector.y >= -vector2.y && feet.y <= this.SurfaceY + 0.045f;
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x0010D454 File Offset: 0x0010B654
		private void OnTriggerStay(Collider other)
		{
			vThirdPersonController componentInParent = other.GetComponentInParent<vThirdPersonController>();
			if (componentInParent == null || !componentInParent.isActiveAndEnabled)
			{
				return;
			}
			Vector3 position = componentInParent.transform.position;
			ShallowPoolWater.Wader wader;
			if (!this.waders.TryGetValue(componentInParent, out wader))
			{
				wader = new ShallowPoolWater.Wader
				{
					footsteps = componentInParent.GetComponent<FootstepController>()
				};
				this.waders.Add(componentInParent, wader);
			}
			if (wader.footsteps != null && wader.footsteps.isActiveAndEnabled)
			{
				return;
			}
			bool flag;
			if (!wader.TryStep(position, this.ContainsFeet(position), Time.time, this.stepDistance, this.minimumStepInterval, out flag))
			{
				return;
			}
			this.PlayContact(position, 1f, flag);
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x0010D504 File Offset: 0x0010B704
		private void PlayContact(Vector3 feet, float speed, bool entered)
		{
			float num = Mathf.InverseLerp(0.5f, 2f, speed);
			Vector3 vector = new Vector3(feet.x, this.SurfaceY + 0.015f, feet.z);
			ShallowPoolWater.SpawnEffect(this.rippleTemplate, vector, Mathf.Lerp(0.42f, 0.6f, num));
			ShallowPoolWater.SpawnEffect(this.splashTemplate, vector, entered ? 0.23f : Mathf.Lerp(0.1f, 0.17f, num));
			AudioClip audioClip = this.SelectFootstepClip();
			if (audioClip == null)
			{
				return;
			}
			if (this.audioSource == null)
			{
				this.audioSource = this.CreateAudioSource("ShallowWaterFootsteps");
			}
			if (this.alternateAudioSource == null)
			{
				this.alternateAudioSource = this.CreateAudioSource("ShallowWaterFootsteps_Alternate");
			}
			object obj = (this.useAlternateSource ? this.alternateAudioSource : this.audioSource);
			this.useAlternateSource = !this.useAlternateSource;
			object obj2 = obj;
			obj2.transform.position = vector;
			obj2.pitch = Mathf.Lerp(0.98f, 1.04f, num) + Random.Range(-0.03f, 0.03f);
			obj2.volume = this.splashVolume * Mathf.Lerp(0.7f, 1f, num);
			obj2.clip = audioClip;
			obj2.Play();
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x0010D654 File Offset: 0x0010B854
		private AudioClip SelectFootstepClip()
		{
			int num = ((this.footstepVariations != null) ? this.footstepVariations.Length : 0);
			if (num == 0)
			{
				return this.footstepSplash;
			}
			int num2 = ((this.lastClipIndex >= 0 && num > 1) ? Random.Range(0, num - 1) : Random.Range(0, num));
			if (num > 1 && this.lastClipIndex >= 0 && num2 >= this.lastClipIndex)
			{
				num2++;
			}
			this.lastClipIndex = num2;
			if (!(this.footstepVariations[num2] != null))
			{
				return this.footstepSplash;
			}
			return this.footstepVariations[num2];
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x0010D6E0 File Offset: 0x0010B8E0
		private void OnTriggerExit(Collider other)
		{
			vThirdPersonController componentInParent = other.GetComponentInParent<vThirdPersonController>();
			ShallowPoolWater.Wader wader;
			if (componentInParent != null && !this.ContainsFeet(componentInParent.transform.position) && this.waders.TryGetValue(componentInParent, out wader))
			{
				wader.touchingWater = false;
			}
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x0010D728 File Offset: 0x0010B928
		private GameObject CreateVisualTemplate(GameObject prefab)
		{
			if (prefab == null)
			{
				return null;
			}
			if (this.effectTemplates == null)
			{
				GameObject gameObject = new GameObject("ShallowWaterVisualTemplates");
				gameObject.SetActive(false);
				gameObject.transform.SetParent(base.transform, false);
				this.effectTemplates = gameObject.transform;
			}
			GameObject gameObject2 = Object.Instantiate<GameObject>(prefab, this.effectTemplates, false);
			gameObject2.SetActive(false);
			foreach (AudioSource audioSource in gameObject2.GetComponentsInChildren<AudioSource>(true))
			{
				audioSource.playOnAwake = false;
				audioSource.mute = true;
				audioSource.enabled = false;
			}
			ParticleSystem[] componentsInChildren2 = gameObject2.GetComponentsInChildren<ParticleSystem>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].main.scalingMode = ParticleSystemScalingMode.Hierarchy;
			}
			return gameObject2;
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x0010D7EC File Offset: 0x0010B9EC
		private static void SpawnEffect(GameObject template, Vector3 position, float scale)
		{
			if (template == null)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(template, position, Quaternion.identity);
			gameObject.transform.localScale *= scale;
			gameObject.SetActive(true);
			Object.Destroy(gameObject, 4f);
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x0010D82C File Offset: 0x0010BA2C
		private void OnDisable()
		{
			this.waders.Clear();
			this.animatedContacts.Clear();
			if (this.audioSource != null)
			{
				this.audioSource.Stop();
			}
			if (this.alternateAudioSource != null)
			{
				this.alternateAudioSource.Stop();
			}
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x0010D881 File Offset: 0x0010BA81
		private void OnDestroy()
		{
			ShallowPoolWater.RecoveryVolumes.Remove(this);
		}

		// Token: 0x04002FF4 RID: 12276
		private static readonly HashSet<ShallowPoolWater> RecoveryVolumes = new HashSet<ShallowPoolWater>();

		// Token: 0x04002FF5 RID: 12277
		[SerializeField]
		private BoxCollider waterVolume;

		// Token: 0x04002FF6 RID: 12278
		[SerializeField]
		private GameObject ripplePrefab;

		// Token: 0x04002FF7 RID: 12279
		[SerializeField]
		private GameObject splashPrefab;

		// Token: 0x04002FF8 RID: 12280
		[Tooltip("Short, gentle wading sound. Do not assign the full-body dive splash.")]
		[SerializeField]
		private AudioClip footstepSplash;

		// Token: 0x04002FF9 RID: 12281
		[Tooltip("Short isolated water contacts, varied without consecutive repeats. Falls back to footstepSplash.")]
		[SerializeField]
		private AudioClip[] footstepVariations;

		// Token: 0x04002FFA RID: 12282
		[SerializeField]
		[Min(0.1f)]
		private float stepDistance = 0.65f;

		// Token: 0x04002FFB RID: 12283
		[SerializeField]
		[Min(0.05f)]
		private float minimumStepInterval = 0.35f;

		// Token: 0x04002FFC RID: 12284
		[SerializeField]
		[Range(0f, 1f)]
		private float splashVolume = 0.28f;

		// Token: 0x04002FFD RID: 12285
		private readonly Dictionary<vThirdPersonController, ShallowPoolWater.Wader> waders = new Dictionary<vThirdPersonController, ShallowPoolWater.Wader>();

		// Token: 0x04002FFE RID: 12286
		private AudioSource audioSource;

		// Token: 0x04002FFF RID: 12287
		private AudioSource alternateAudioSource;

		// Token: 0x04003000 RID: 12288
		private bool useAlternateSource;

		// Token: 0x04003001 RID: 12289
		private int lastClipIndex = -1;

		// Token: 0x04003002 RID: 12290
		private readonly Dictionary<Transform, float> animatedContacts = new Dictionary<Transform, float>();

		// Token: 0x04003003 RID: 12291
		private Transform effectTemplates;

		// Token: 0x04003004 RID: 12292
		private GameObject rippleTemplate;

		// Token: 0x04003005 RID: 12293
		private GameObject splashTemplate;

		// Token: 0x0200072B RID: 1835
		private sealed class Wader
		{
			// Token: 0x06002F96 RID: 12182 RVA: 0x0010D8F0 File Offset: 0x0010BAF0
			public bool TryStep(Vector3 feet, bool touching, float time, float distance, float interval, out bool entered)
			{
				entered = touching && !this.touchingWater;
				this.touchingWater = touching;
				if (!touching)
				{
					return false;
				}
				Vector3 vector = feet - this.lastStep;
				vector.y = 0f;
				if (entered)
				{
					this.lastStep = feet;
				}
				if (time < this.nextStep || (!entered && vector.sqrMagnitude < distance * distance))
				{
					return false;
				}
				this.lastStep = feet;
				this.nextStep = time + interval;
				return true;
			}

			// Token: 0x04003006 RID: 12294
			private Vector3 lastStep;

			// Token: 0x04003007 RID: 12295
			private float nextStep;

			// Token: 0x04003008 RID: 12296
			public bool touchingWater;

			// Token: 0x04003009 RID: 12297
			public FootstepController footsteps;
		}
	}
}
