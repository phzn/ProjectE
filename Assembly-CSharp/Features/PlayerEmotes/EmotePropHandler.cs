using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using ProjectExhibitionist.Futanari;
using UnityEngine;
using UnityEngine.VFX;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E19 RID: 3609
	public class EmotePropHandler : MonoBehaviour
	{
		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06006B64 RID: 27492 RVA: 0x002B0028 File Offset: 0x002AE228
		// (remove) Token: 0x06006B65 RID: 27493 RVA: 0x002B0060 File Offset: 0x002AE260
		public event Action<string> OnPropActivated;

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06006B66 RID: 27494 RVA: 0x002B0098 File Offset: 0x002AE298
		// (remove) Token: 0x06006B67 RID: 27495 RVA: 0x002B00D0 File Offset: 0x002AE2D0
		public event Action<string> OnPropDeactivated;

		// Token: 0x06006B68 RID: 27496 RVA: 0x002B0105 File Offset: 0x002AE305
		private void Awake()
		{
			this._animator = base.GetComponentInChildren<Animator>();
		}

		// Token: 0x06006B69 RID: 27497 RVA: 0x002B0114 File Offset: 0x002AE314
		public void SetupProps(EmoteDefinitionSO emote)
		{
			this.CleanupAllProps();
			if (emote == null || emote.PropBindings == null || emote.PropBindings.Count == 0)
			{
				return;
			}
			if (this._animator == null)
			{
				this._animator = base.GetComponentInChildren<Animator>();
			}
			for (int i = 0; i < emote.PropBindings.Count; i++)
			{
				EmotePropBinding emotePropBinding = emote.PropBindings[i];
				if (!(emotePropBinding.EffectPrefab == null))
				{
					string text;
					Transform transform = this.ResolveAttachBone(emotePropBinding, out text);
					GameObject gameObject = Object.Instantiate<GameObject>(emotePropBinding.EffectPrefab, transform);
					gameObject.transform.localPosition = emotePropBinding.LocalPositionOffset;
					gameObject.transform.localRotation = Quaternion.Euler(emotePropBinding.LocalRotationOffset);
					Vector3 localScaleOffset = emotePropBinding.LocalScaleOffset;
					if (localScaleOffset.sqrMagnitude >= 1E-06f)
					{
						gameObject.transform.localScale = localScaleOffset;
					}
					gameObject.SetActive(false);
					AudioSource[] array = EmotePropHandler.BuildDedicatedAudioSources(gameObject);
					string text2 = EmotePropHandler.NormalizeLabel(string.IsNullOrEmpty(emotePropBinding.Label) ? string.Format("prop_{0}", i) : emotePropBinding.Label);
					if (string.Equals(text2, "PeeEffect", StringComparison.OrdinalIgnoreCase))
					{
						this.AttachCustomizedStream(gameObject);
					}
					this._activeProps[text2] = new EmotePropHandler.PropInstance
					{
						Go = gameObject,
						Binding = emotePropBinding,
						IsActive = false,
						RuntimeAudioSources = array
					};
					if (this.DebugMode)
					{
						Debug.Log(string.Concat(new string[] { "[EmotePropHandler] Spawned prop '", text2, "' under ", transform.name, " (via ", text, ")" }));
					}
				}
			}
		}

		// Token: 0x06006B6A RID: 27498 RVA: 0x002B02D0 File Offset: 0x002AE4D0
		public void StartAutoTimers()
		{
			foreach (KeyValuePair<string, EmotePropHandler.PropInstance> keyValuePair in this._activeProps)
			{
				EmotePropBinding binding = keyValuePair.Value.Binding;
				if (binding.AutoActivateDelay > 0f)
				{
					this._timerCoroutines.Add(base.StartCoroutine(this.AutoActivateCoroutine(keyValuePair.Key, binding.AutoActivateDelay)));
				}
			}
		}

		// Token: 0x06006B6B RID: 27499 RVA: 0x002B035C File Offset: 0x002AE55C
		private void AttachCustomizedStream(GameObject prop)
		{
			FutanariModeController componentInChildren = base.GetComponentInChildren<FutanariModeController>(true);
			if (componentInChildren == null || !componentInChildren.FutanariMode)
			{
				return;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer.sharedMesh == null) && !(skinnedMeshRenderer.sharedMesh.name != "sao"))
				{
					SkinnedStreamAttachment skinnedStreamAttachment = prop.AddComponent<SkinnedStreamAttachment>();
					if (!skinnedStreamAttachment.Bind(skinnedMeshRenderer, "Bone.019"))
					{
						Object.Destroy(skinnedStreamAttachment);
						if (this.DebugMode)
						{
							Debug.LogWarning("[EmotePropHandler] Stream surface attachment could not be resolved.", this);
						}
					}
					return;
				}
			}
		}

		// Token: 0x06006B6C RID: 27500 RVA: 0x002B03F4 File Offset: 0x002AE5F4
		private IEnumerator AutoActivateCoroutine(string label, float delay)
		{
			yield return this.WaitUnlessPlaybackHeld(delay);
			this.ActivateProp(label);
			yield break;
		}

		// Token: 0x06006B6D RID: 27501 RVA: 0x002B0411 File Offset: 0x002AE611
		private IEnumerator AutoDeactivateCoroutine(string label, float delay)
		{
			yield return this.WaitUnlessPlaybackHeld(delay);
			this.DeactivateProp(label);
			yield break;
		}

		// Token: 0x06006B6E RID: 27502 RVA: 0x002B042E File Offset: 0x002AE62E
		private IEnumerator WaitUnlessPlaybackHeld(float seconds)
		{
			if (this._holdSource == null)
			{
				this._holdSource = base.GetComponentInParent<GameplayEmoteController>(true);
			}
			float elapsed = 0f;
			while (elapsed < seconds)
			{
				yield return null;
				if (this._holdSource == null || !this._holdSource.IsPlaybackHeld)
				{
					elapsed += Time.deltaTime;
				}
			}
			yield break;
		}

		// Token: 0x06006B6F RID: 27503 RVA: 0x002B0444 File Offset: 0x002AE644
		public void ActivateProp(string label)
		{
			label = EmotePropHandler.NormalizeLabel(label);
			if (string.IsNullOrEmpty(label))
			{
				return;
			}
			EmotePropHandler.PropInstance propInstance;
			if (!this._activeProps.TryGetValue(label, out propInstance))
			{
				return;
			}
			if (propInstance.IsActive || propInstance.Go == null)
			{
				return;
			}
			propInstance.Go.SetActive(true);
			EmotePropHandler.PlayPropEffects(propInstance.Go, propInstance.RuntimeAudioSources);
			propInstance.IsActive = true;
			if (this.DebugMode)
			{
				Debug.Log("[EmotePropHandler] Activated prop '" + label + "'");
			}
			Action<string> onPropActivated = this.OnPropActivated;
			if (onPropActivated != null)
			{
				onPropActivated(label);
			}
			if (propInstance.Binding.AutoDeactivateAfter > 0f)
			{
				this._timerCoroutines.Add(base.StartCoroutine(this.AutoDeactivateCoroutine(label, propInstance.Binding.AutoDeactivateAfter)));
			}
		}

		// Token: 0x06006B70 RID: 27504 RVA: 0x002B0514 File Offset: 0x002AE714
		public void DeactivateProp(string label)
		{
			label = EmotePropHandler.NormalizeLabel(label);
			if (string.IsNullOrEmpty(label))
			{
				return;
			}
			EmotePropHandler.PropInstance propInstance;
			if (!this._activeProps.TryGetValue(label, out propInstance))
			{
				return;
			}
			if (!propInstance.IsActive || propInstance.Go == null)
			{
				return;
			}
			EmotePropHandler.StopPropEffects(propInstance.Go, propInstance.RuntimeAudioSources);
			propInstance.Go.SetActive(false);
			propInstance.IsActive = false;
			if (this.DebugMode)
			{
				Debug.Log("[EmotePropHandler] Deactivated prop '" + label + "'");
			}
			Action<string> onPropDeactivated = this.OnPropDeactivated;
			if (onPropDeactivated == null)
			{
				return;
			}
			onPropDeactivated(label);
		}

		// Token: 0x06006B71 RID: 27505 RVA: 0x002B05AC File Offset: 0x002AE7AC
		public void CleanupAllProps()
		{
			foreach (Coroutine coroutine in this._timerCoroutines)
			{
				if (coroutine != null)
				{
					base.StopCoroutine(coroutine);
				}
			}
			this._timerCoroutines.Clear();
			foreach (KeyValuePair<string, EmotePropHandler.PropInstance> keyValuePair in this._activeProps)
			{
				if (keyValuePair.Value.Go != null)
				{
					EmotePropHandler.StopPropEffects(keyValuePair.Value.Go, keyValuePair.Value.RuntimeAudioSources);
					keyValuePair.Value.Go.SetActive(false);
					if (keyValuePair.Value.Binding.DestroyOnEnd)
					{
						Object.Destroy(keyValuePair.Value.Go);
					}
				}
			}
			this._activeProps.Clear();
		}

		// Token: 0x06006B72 RID: 27506 RVA: 0x002B06BC File Offset: 0x002AE8BC
		public void AE_EmoteProp(string label)
		{
			if (this.DebugMode)
			{
				Debug.Log("[EmotePropHandler] Animation Event: AE_EmoteProp('" + label + "')");
			}
			this.ActivateProp(label);
		}

		// Token: 0x06006B73 RID: 27507 RVA: 0x002B06E2 File Offset: 0x002AE8E2
		public void AE_EmotePropOff(string label)
		{
			if (this.DebugMode)
			{
				Debug.Log("[EmotePropHandler] Animation Event: AE_EmotePropOff('" + label + "')");
			}
			this.DeactivateProp(label);
		}

		// Token: 0x06006B74 RID: 27508 RVA: 0x002B0708 File Offset: 0x002AE908
		private void OnDestroy()
		{
			this.CleanupAllProps();
		}

		// Token: 0x06006B75 RID: 27509 RVA: 0x002B0710 File Offset: 0x002AE910
		private static string NormalizeLabel(string label)
		{
			if (!string.IsNullOrWhiteSpace(label))
			{
				return label.Trim();
			}
			return string.Empty;
		}

		// Token: 0x06006B76 RID: 27510 RVA: 0x002B0728 File Offset: 0x002AE928
		private Transform ResolveAttachBone(EmotePropBinding binding, out string source)
		{
			if (binding.UseCustomBone && !string.IsNullOrWhiteSpace(binding.CustomBonePath))
			{
				Transform transform = EmotePropHandler.FindChildByPath(base.transform, binding.CustomBonePath.Trim());
				if (transform != null)
				{
					source = "path '" + binding.CustomBonePath + "'";
					return transform;
				}
				if (this.DebugMode)
				{
					Debug.LogWarning("[EmotePropHandler] CustomBonePath '" + binding.CustomBonePath + "' not found, trying name fallback.");
				}
			}
			if (binding.UseCustomBone && !string.IsNullOrWhiteSpace(binding.CustomBoneName))
			{
				Transform transform2 = EmotePropHandler.FindChildByName(base.transform, binding.CustomBoneName.Trim());
				if (transform2 != null)
				{
					source = "name '" + binding.CustomBoneName + "'";
					return transform2;
				}
				if (this.DebugMode)
				{
					Debug.LogWarning("[EmotePropHandler] CustomBoneName '" + binding.CustomBoneName + "' not found, falling back to AttachBone.");
				}
			}
			if (this._animator != null)
			{
				Transform boneTransform = this._animator.GetBoneTransform(binding.AttachBone);
				if (boneTransform != null)
				{
					source = string.Format("humanoid {0}", binding.AttachBone);
					return boneTransform;
				}
			}
			source = "player root (fallback)";
			return base.transform;
		}

		// Token: 0x06006B77 RID: 27511 RVA: 0x002B0864 File Offset: 0x002AEA64
		private static Transform FindChildByName(Transform root, string name)
		{
			if (root == null)
			{
				return null;
			}
			if (root.name == name)
			{
				return root;
			}
			for (int i = 0; i < root.childCount; i++)
			{
				Transform transform = EmotePropHandler.FindChildByName(root.GetChild(i), name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06006B78 RID: 27512 RVA: 0x002B08B8 File Offset: 0x002AEAB8
		private static Transform FindChildByPath(Transform root, string path)
		{
			if (root == null || string.IsNullOrEmpty(path))
			{
				return null;
			}
			Transform transform = root.Find(path);
			if (transform != null)
			{
				return transform;
			}
			string[] array = path.Split('/', StringSplitOptions.None);
			string text = array[array.Length - 1];
			Transform transform2 = EmotePropHandler.FindChildByName(root, text);
			if (transform2 == null)
			{
				return null;
			}
			Transform transform3 = transform2;
			for (int i = array.Length - 2; i >= 0; i--)
			{
				transform3 = transform3.parent;
				if (transform3 == null || transform3.name != array[i])
				{
					return null;
				}
			}
			return transform2;
		}

		// Token: 0x06006B79 RID: 27513 RVA: 0x002B0950 File Offset: 0x002AEB50
		private static void PlayPropEffects(GameObject root, AudioSource[] runtimeAudioSources)
		{
			if (root == null)
			{
				return;
			}
			foreach (ParticleSystem particleSystem in root.GetComponentsInChildren<ParticleSystem>(true))
			{
				if (!(particleSystem == null))
				{
					particleSystem.Play(true);
				}
			}
			foreach (VisualEffect visualEffect in root.GetComponentsInChildren<VisualEffect>(true))
			{
				if (!(visualEffect == null))
				{
					visualEffect.Play();
				}
			}
			if (runtimeAudioSources != null)
			{
				foreach (AudioSource audioSource in runtimeAudioSources)
				{
					if (!(audioSource == null))
					{
						audioSource.Stop();
						if (audioSource.clip != null)
						{
							audioSource.Play();
						}
					}
				}
			}
		}

		// Token: 0x06006B7A RID: 27514 RVA: 0x002B0A04 File Offset: 0x002AEC04
		private static void StopPropEffects(GameObject root, AudioSource[] runtimeAudioSources)
		{
			if (root == null)
			{
				return;
			}
			foreach (ParticleSystem particleSystem in root.GetComponentsInChildren<ParticleSystem>(true))
			{
				if (!(particleSystem == null))
				{
					particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
			}
			foreach (VisualEffect visualEffect in root.GetComponentsInChildren<VisualEffect>(true))
			{
				if (!(visualEffect == null))
				{
					visualEffect.Stop();
				}
			}
			if (runtimeAudioSources != null)
			{
				foreach (AudioSource audioSource in runtimeAudioSources)
				{
					if (!(audioSource == null))
					{
						audioSource.Stop();
					}
				}
			}
		}

		// Token: 0x06006B7B RID: 27515 RVA: 0x002B0AA4 File Offset: 0x002AECA4
		private static AudioSource[] BuildDedicatedAudioSources(GameObject propRoot)
		{
			if (propRoot == null)
			{
				return Array.Empty<AudioSource>();
			}
			AudioSource[] componentsInChildren = propRoot.GetComponentsInChildren<AudioSource>(true);
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				return Array.Empty<AudioSource>();
			}
			AudioSource[] array = new AudioSource[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				AudioSource audioSource = componentsInChildren[i];
				if (!(audioSource == null))
				{
					AudioSource audioSource2 = propRoot.AddComponent<AudioSource>();
					audioSource2.clip = audioSource.clip;
					audioSource2.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
					audioSource2.mute = audioSource.mute;
					audioSource2.bypassEffects = audioSource.bypassEffects;
					audioSource2.bypassListenerEffects = audioSource.bypassListenerEffects;
					audioSource2.bypassReverbZones = audioSource.bypassReverbZones;
					audioSource2.playOnAwake = false;
					audioSource2.loop = audioSource.loop;
					audioSource2.priority = audioSource.priority;
					audioSource2.volume = audioSource.volume;
					audioSource2.pitch = audioSource.pitch;
					audioSource2.panStereo = audioSource.panStereo;
					audioSource2.spatialBlend = audioSource.spatialBlend;
					audioSource2.reverbZoneMix = audioSource.reverbZoneMix;
					audioSource2.dopplerLevel = audioSource.dopplerLevel;
					audioSource2.spread = audioSource.spread;
					audioSource2.rolloffMode = audioSource.rolloffMode;
					audioSource2.minDistance = audioSource.minDistance;
					audioSource2.maxDistance = audioSource.maxDistance;
					audioSource2.spatialize = audioSource.spatialize;
					audioSource2.spatializePostEffects = audioSource.spatializePostEffects;
					audioSource2.ignoreListenerPause = audioSource.ignoreListenerPause || NPCSimulationPause.IsPaused;
					audioSource.Stop();
					audioSource.playOnAwake = false;
					audioSource.enabled = false;
					array[i] = audioSource2;
				}
			}
			return array;
		}

		// Token: 0x04006E18 RID: 28184
		[Header("Debug")]
		public bool DebugMode;

		// Token: 0x04006E1B RID: 28187
		private Animator _animator;

		// Token: 0x04006E1C RID: 28188
		private readonly Dictionary<string, EmotePropHandler.PropInstance> _activeProps = new Dictionary<string, EmotePropHandler.PropInstance>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04006E1D RID: 28189
		private readonly List<Coroutine> _timerCoroutines = new List<Coroutine>();

		// Token: 0x04006E1E RID: 28190
		private GameplayEmoteController _holdSource;

		// Token: 0x02000E1A RID: 3610
		private class PropInstance
		{
			// Token: 0x04006E1F RID: 28191
			public GameObject Go;

			// Token: 0x04006E20 RID: 28192
			public EmotePropBinding Binding;

			// Token: 0x04006E21 RID: 28193
			public bool IsActive;

			// Token: 0x04006E22 RID: 28194
			public AudioSource[] RuntimeAudioSources;
		}
	}
}
