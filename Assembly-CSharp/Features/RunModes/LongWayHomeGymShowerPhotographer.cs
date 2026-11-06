using System;
using System.Collections.Generic;
using System.Reflection;
using Bozo.AnimeCharacters;
using Features.MainMenu;
using Features.NPC;
using Features.NPCs;
using ProjectExhibitionist.NPCs;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000CBE RID: 3262
	public sealed class LongWayHomeGymShowerPhotographer
	{
		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06005EAB RID: 24235 RVA: 0x002572AC File Offset: 0x002554AC
		// (remove) Token: 0x06005EAC RID: 24236 RVA: 0x002572E4 File Offset: 0x002554E4
		public event Action PhotoTaken;

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x06005EAD RID: 24237 RVA: 0x00257319 File Offset: 0x00255519
		// (set) Token: 0x06005EAE RID: 24238 RVA: 0x00257321 File Offset: 0x00255521
		public string DisplayName { get; private set; }

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x06005EAF RID: 24239 RVA: 0x0025732A File Offset: 0x0025552A
		// (set) Token: 0x06005EB0 RID: 24240 RVA: 0x00257332 File Offset: 0x00255532
		public string ResourceName { get; private set; }

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x06005EB1 RID: 24241 RVA: 0x0025733B File Offset: 0x0025553B
		public Transform Transform
		{
			get
			{
				if (!(this._root != null))
				{
					return null;
				}
				return this._root.transform;
			}
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x06005EB2 RID: 24242 RVA: 0x00257358 File Offset: 0x00255558
		public bool IsValid
		{
			get
			{
				return this._root != null && this._graph.IsValid();
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x06005EB3 RID: 24243 RVA: 0x00257375 File Offset: 0x00255575
		public LongWayHomeGymShowerPhotographer.Pose CurrentPose
		{
			get
			{
				return this._pose;
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06005EB4 RID: 24244 RVA: 0x0025737D File Offset: 0x0025557D
		public float CurrentPoseTime
		{
			get
			{
				if (!this._graph.IsValid())
				{
					return 0f;
				}
				return (float)this._playables[(int)this._pose].GetTime<AnimationClipPlayable>();
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x06005EB5 RID: 24245 RVA: 0x002573A9 File Offset: 0x002555A9
		public float PickUpLength
		{
			get
			{
				if (!(this._clips[4] != null))
				{
					return 0f;
				}
				return this._clips[4].length;
			}
		}

		// Token: 0x06005EB6 RID: 24246 RVA: 0x002573D0 File Offset: 0x002555D0
		private LongWayHomeGymShowerPhotographer(LongWayHomeGymShowerPhotographer.Clips clips)
		{
			this._shutter = clips.shutter;
			this._footsteps = clips.footsteps;
			this._footstepVolume = clips.footstepVolume;
			this._clips[0] = clips.idle;
			this._clips[1] = clips.walk;
			this._clips[2] = clips.run;
			this._clips[3] = clips.photo;
			this._clips[4] = ((clips.pickUp != null) ? clips.pickUp : clips.idle);
		}

		// Token: 0x06005EB7 RID: 24247 RVA: 0x002574C0 File Offset: 0x002556C0
		public static LongWayHomeGymShowerPhotographer.Clips ClipsFrom(LongWayHomeGymShowerConfigSO config)
		{
			return new LongWayHomeGymShowerPhotographer.Clips
			{
				idle = config.npcIdle,
				walk = config.npcWalk,
				run = config.npcRun,
				photo = config.npcPhoto,
				pickUp = config.npcPickUp,
				shutter = config.npcShutter,
				footsteps = config.npcFootsteps,
				footstepVolume = config.npcFootstepVolume
			};
		}

		// Token: 0x06005EB8 RID: 24248 RVA: 0x00257532 File Offset: 0x00255732
		public static LongWayHomeGymShowerPhotographer Create(Scene scene, LongWayHomeGymShowerConfigSO config, out string error)
		{
			return LongWayHomeGymShowerPhotographer.Create(scene, LongWayHomeGymShowerPhotographer.ClipsFrom(config), true, null, "Gym Photographer", out error);
		}

		// Token: 0x06005EB9 RID: 24249 RVA: 0x00257548 File Offset: 0x00255748
		public static LongWayHomeGymShowerPhotographer Create(Scene scene, LongWayHomeGymShowerPhotographer.Clips clips, bool preferMen, ICollection<string> excludedResources, string label, out string error)
		{
			error = null;
			GameObject gameObject;
			NPCRosterEntry npcrosterEntry = LongWayHomeGymShowerPhotographer.ChooseEntry(NPCRosterSession.GetCurrentSnapshot(), preferMen, excludedResources, out gameObject);
			if (npcrosterEntry == null || gameObject == null)
			{
				error = "The selected NPC population has no civilian for the " + label.ToLowerInvariant() + ".";
				return null;
			}
			GameObject gameObject2 = new GameObject(label + " (staging)");
			gameObject2.SetActive(false);
			SceneManager.MoveGameObjectToScene(gameObject2, scene);
			GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject, gameObject2.transform);
			gameObject3.name = label + " - " + npcrosterEntry.resourceName;
			LongWayHomeGymShowerPhotographer longWayHomeGymShowerPhotographer = new LongWayHomeGymShowerPhotographer(clips);
			try
			{
				longWayHomeGymShowerPhotographer.Build(gameObject3, npcrosterEntry);
			}
			catch (Exception ex)
			{
				Object.Destroy(gameObject2);
				error = "The " + label.ToLowerInvariant() + " could not be prepared: " + ex.Message;
				return null;
			}
			gameObject3.SetActive(false);
			gameObject3.transform.SetParent(null, false);
			Object.Destroy(gameObject2);
			return longWayHomeGymShowerPhotographer;
		}

		// Token: 0x06005EBA RID: 24250 RVA: 0x00257644 File Offset: 0x00255844
		private static NPCRosterEntry ChooseEntry(NPCRosterSnapshot roster, bool preferMen, ICollection<string> excluded, out GameObject prefab)
		{
			prefab = null;
			if (((roster != null) ? roster.entries : null) == null)
			{
				return null;
			}
			List<NPCRosterEntry> list = new List<NPCRosterEntry>();
			foreach (NPCRosterEntry npcrosterEntry in roster.entries)
			{
				if (npcrosterEntry != null && !npcrosterEntry.isCustom && !string.IsNullOrEmpty(npcrosterEntry.resourceName) && NPCRosterSession.MatchesPopulation(roster.populationMode, npcrosterEntry.gender))
				{
					list.Add(npcrosterEntry);
				}
			}
			if (excluded != null && excluded.Count > 0)
			{
				List<NPCRosterEntry> list2 = list.FindAll((NPCRosterEntry entry) => !excluded.Contains(entry.resourceName));
				if (list2.Count > 0)
				{
					list = list2;
				}
			}
			List<NPCRosterEntry> list3;
			if (!preferMen)
			{
				list3 = null;
			}
			else
			{
				list3 = list.FindAll((NPCRosterEntry entry) => entry.gender == Gender.Male);
			}
			List<NPCRosterEntry> list4 = list3;
			List<NPCRosterEntry> list5 = ((list4 != null && list4.Count > 0) ? list4 : list);
			if (list5.Count == 0)
			{
				return null;
			}
			int num = Random.Range(0, list5.Count);
			for (int i = 0; i < list5.Count; i++)
			{
				NPCRosterEntry npcrosterEntry2 = list5[(num + i) % list5.Count];
				prefab = NPCBakedResourceIndex.LoadPrefab(npcrosterEntry2.resourceName);
				if (prefab != null && prefab.GetComponent<Animator>() != null)
				{
					return npcrosterEntry2;
				}
			}
			prefab = null;
			return null;
		}

		// Token: 0x06005EBB RID: 24251 RVA: 0x002577D4 File Offset: 0x002559D4
		private void Build(GameObject root, NPCRosterEntry entry)
		{
			this._root = root;
			this.ResourceName = entry.resourceName;
			this._animator = root.GetComponent<Animator>();
			if (this._animator == null || this._animator.avatar == null || !this._animator.avatar.isHuman)
			{
				throw new InvalidOperationException("the baked civilian has no Humanoid Animator.");
			}
			AnimationClip[] clips = this._clips;
			for (int i = 0; i < clips.Length; i++)
			{
				if (clips[i] == null)
				{
					throw new InvalidOperationException("a photographer clip is missing.");
				}
			}
			NPCProp[] componentsInChildren = root.GetComponentsInChildren<NPCProp>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(false);
			}
			this.CapturePhone(root);
			LongWayHomeGymShowerPhotographer.StripToVisual(root);
			LongWayHomeGymShowerPhotoEvents longWayHomeGymShowerPhotoEvents = root.AddComponent<LongWayHomeGymShowerPhotoEvents>();
			longWayHomeGymShowerPhotoEvents.Captured += this.OnPhotoEvent;
			longWayHomeGymShowerPhotoEvents.Footstep += this.OnFootstep;
			this._audio = root.AddComponent<AudioSource>();
			this._audio.playOnAwake = false;
			this._audio.spatialBlend = 0.8f;
			this._audio.minDistance = 1.5f;
			this._audio.maxDistance = 18f;
			GameAudioRouter.Route(this._audio, GameAudioRouter.Channel.Sfx);
			this._animator.runtimeAnimatorController = null;
			this._animator.applyRootMotion = false;
			this._animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			CharacterObject characterObject = NPCBakedResourceIndex.LoadCharacter(entry.resourceName);
			string text;
			if (characterObject == null)
			{
				text = null;
			}
			else
			{
				CharacterData data = characterObject.data;
				if (data == null)
				{
					text = null;
				}
				else
				{
					CharacterGeneralInfo generalInfo = data.generalInfo;
					text = ((generalInfo != null) ? generalInfo.characterName : null);
				}
			}
			string text2 = text;
			this.DisplayName = ((!string.IsNullOrWhiteSpace(entry.displayName)) ? entry.displayName : ((!string.IsNullOrWhiteSpace(text2)) ? text2 : "???"));
			this._graph = PlayableGraph.Create("Long Way Home - Gym Photographer");
			this._graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
			this._mixer = AnimationMixerPlayable.Create(this._graph, 5);
			for (int j = 0; j < 5; j++)
			{
				this._playables[j] = AnimationClipPlayable.Create(this._graph, this._clips[j]);
				this._playables[j].SetApplyFootIK(false);
				this._graph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(this._playables[j], 0, this._mixer, j);
			}
			this._weights[0] = 1f;
			this.ApplyWeights();
			AnimationPlayableOutput.Create(this._graph, "Gym Photographer", this._animator).SetSourcePlayable(this._mixer);
			if (this._phone != null)
			{
				this._phone.SetActive(false);
			}
		}

		// Token: 0x06005EBC RID: 24252 RVA: 0x00257A7C File Offset: 0x00255C7C
		private void CapturePhone(GameObject root)
		{
			SmartphoneFlash componentInChildren = root.GetComponentInChildren<SmartphoneFlash>(true);
			Transform transform = LongWayHomeGymShowerPhotographer.FindDeep(root.transform, "Smartphone");
			if (transform == null && componentInChildren != null)
			{
				transform = componentInChildren.transform;
			}
			this._phone = ((transform != null) ? transform.gameObject : null);
			if (componentInChildren == null)
			{
				if (this._phone != null)
				{
					foreach (Light light in this._phone.GetComponentsInChildren<Light>(true))
					{
						this.AddFlashLight(light, (light.type == LightType.Spot) ? 8f : 3f);
					}
				}
				return;
			}
			this.AddFlashLight(LongWayHomeGymShowerPhotographer.Field<Light>(componentInChildren, "spotLight"), LongWayHomeGymShowerPhotographer.Field(componentInChildren, "spotIntensity", 8f));
			this.AddFlashLight(LongWayHomeGymShowerPhotographer.Field<Light>(componentInChildren, "pointLight"), LongWayHomeGymShowerPhotographer.Field(componentInChildren, "pointIntensity", 3f));
			this._shutter = LongWayHomeGymShowerPhotographer.Field<AudioClip>(componentInChildren, "flashSound") ?? this._shutter;
			this._shutterVolume = LongWayHomeGymShowerPhotographer.Field(componentInChildren, "flashVolume", 1f);
			this._flashDuration = Mathf.Max(0.05f, LongWayHomeGymShowerPhotographer.Field(componentInChildren, "flashDuration", 0.15f));
		}

		// Token: 0x06005EBD RID: 24253 RVA: 0x00257BBB File Offset: 0x00255DBB
		private void AddFlashLight(Light light, float intensity)
		{
			if (light == null)
			{
				return;
			}
			light.intensity = 0f;
			light.enabled = true;
			this._flashLights.Add(light);
			this._flashIntensities.Add(intensity);
		}

		// Token: 0x06005EBE RID: 24254 RVA: 0x00257BF1 File Offset: 0x00255DF1
		private static T Field<T>(object target, string name) where T : class
		{
			FieldInfo field = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			return ((field != null) ? field.GetValue(target) : null) as T;
		}

		// Token: 0x06005EBF RID: 24255 RVA: 0x00257C18 File Offset: 0x00255E18
		private static float Field(object target, string name, float fallback)
		{
			FieldInfo field = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			object obj = ((field != null) ? field.GetValue(target) : null);
			if (obj is float)
			{
				return (float)obj;
			}
			return fallback;
		}

		// Token: 0x06005EC0 RID: 24256 RVA: 0x00257C58 File Offset: 0x00255E58
		private static Transform FindDeep(Transform parent, string name)
		{
			if (parent.name == name)
			{
				return parent;
			}
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform transform = LongWayHomeGymShowerPhotographer.FindDeep(parent.GetChild(i), name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06005EC1 RID: 24257 RVA: 0x00257CA0 File Offset: 0x00255EA0
		public static int StripToVisual(GameObject root)
		{
			int num = 0;
			for (int i = 0; i < 12; i++)
			{
				Component[] componentsInChildren = root.GetComponentsInChildren<Component>(true);
				HashSet<KeyValuePair<GameObject, Type>> hashSet = new HashSet<KeyValuePair<GameObject, Type>>();
				foreach (Component component in componentsInChildren)
				{
					if (!(component == null))
					{
						foreach (RequireComponent requireComponent in component.GetType().GetCustomAttributes(typeof(RequireComponent), true))
						{
							LongWayHomeGymShowerPhotographer.AddRequirement(hashSet, component.gameObject, requireComponent.m_Type0);
							LongWayHomeGymShowerPhotographer.AddRequirement(hashSet, component.gameObject, requireComponent.m_Type1);
							LongWayHomeGymShowerPhotographer.AddRequirement(hashSet, component.gameObject, requireComponent.m_Type2);
						}
					}
				}
				int num2 = 0;
				foreach (Component component2 in componentsInChildren)
				{
					if (!(component2 == null) && !LongWayHomeGymShowerPhotographer.IsVisual(component2) && !LongWayHomeGymShowerPhotographer.IsRequired(hashSet, component2))
					{
						Object.DestroyImmediate(component2);
						num2++;
					}
				}
				num += num2;
				if (num2 == 0)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x06005EC2 RID: 24258 RVA: 0x00257DC5 File Offset: 0x00255FC5
		private static void AddRequirement(HashSet<KeyValuePair<GameObject, Type>> required, GameObject owner, Type type)
		{
			if (type != null)
			{
				required.Add(new KeyValuePair<GameObject, Type>(owner, type));
			}
		}

		// Token: 0x06005EC3 RID: 24259 RVA: 0x00257DE0 File Offset: 0x00255FE0
		private static bool IsRequired(HashSet<KeyValuePair<GameObject, Type>> required, Component component)
		{
			foreach (KeyValuePair<GameObject, Type> keyValuePair in required)
			{
				if (keyValuePair.Key == component.gameObject && keyValuePair.Value.IsInstanceOfType(component) && component.GetComponents(keyValuePair.Value).Length <= 1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005EC4 RID: 24260 RVA: 0x00257E64 File Offset: 0x00256064
		private static bool IsVisual(Component component)
		{
			Type type = component.GetType();
			using (HashSet<Type>.Enumerator enumerator = LongWayHomeGymShowerPhotographer.VisualComponents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsAssignableFrom(type))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005EC5 RID: 24261 RVA: 0x00257EC4 File Offset: 0x002560C4
		public void Show(Vector3 position, Quaternion rotation)
		{
			if (this._root == null)
			{
				return;
			}
			this._root.transform.SetPositionAndRotation(position, rotation);
			this._root.SetActive(true);
			if (this._graph.IsValid() && !this._graph.IsPlaying())
			{
				this._graph.Play();
			}
			this.SetPose(LongWayHomeGymShowerPhotographer.Pose.Idle, 0.01f);
		}

		// Token: 0x06005EC6 RID: 24262 RVA: 0x00257F2F File Offset: 0x0025612F
		public void Hide()
		{
			if (this._graph.IsValid())
			{
				this._graph.Stop();
			}
			if (this._root == null)
			{
				return;
			}
			this.SetFlashIntensity(0f);
			this._root.SetActive(false);
		}

		// Token: 0x06005EC7 RID: 24263 RVA: 0x00257F70 File Offset: 0x00256170
		public void SetPose(LongWayHomeGymShowerPhotographer.Pose pose, float fadeSeconds = 0.25f)
		{
			if (!this._graph.IsValid())
			{
				return;
			}
			bool flag = pose != this._pose || pose == LongWayHomeGymShowerPhotographer.Pose.PickUp;
			this._pose = pose;
			this._fadeSeconds = Mathf.Max(0.01f, fadeSeconds);
			if (flag && (pose == LongWayHomeGymShowerPhotographer.Pose.Photo || pose == LongWayHomeGymShowerPhotographer.Pose.PickUp))
			{
				this._playables[(int)pose].SetTime(0.0);
				this._playables[(int)pose].SetTime(0.0);
			}
			if (pose == LongWayHomeGymShowerPhotographer.Pose.Photo)
			{
				this._photoClock = 0f;
				this._nextTimedPhoto = 1.6f;
			}
			if (this._phone != null)
			{
				this._phone.SetActive(pose == LongWayHomeGymShowerPhotographer.Pose.Photo);
			}
		}

		// Token: 0x06005EC8 RID: 24264 RVA: 0x00258028 File Offset: 0x00256228
		public void SetPoseSpeed(LongWayHomeGymShowerPhotographer.Pose pose, float speed)
		{
			if (this._graph.IsValid())
			{
				this._playables[(int)pose].SetSpeed((double)speed);
			}
		}

		// Token: 0x06005EC9 RID: 24265 RVA: 0x0025804A File Offset: 0x0025624A
		public void SetPoseTime(LongWayHomeGymShowerPhotographer.Pose pose, float seconds)
		{
			if (!this._graph.IsValid())
			{
				return;
			}
			this._playables[(int)pose].SetTime((double)seconds);
			this._playables[(int)pose].SetTime((double)seconds);
		}

		// Token: 0x06005ECA RID: 24266 RVA: 0x00258080 File Offset: 0x00256280
		public bool MoveTowards(Vector3 target, float metersPerSecond, float degreesPerSecond, float deltaTime)
		{
			if (this._root == null)
			{
				return true;
			}
			Transform transform = this._root.transform;
			target.y = transform.position.y;
			Vector3 vector = target - transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > 0.0001f)
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(vector), degreesPerSecond * deltaTime);
			}
			transform.position = Vector3.MoveTowards(transform.position, target, metersPerSecond * deltaTime);
			return (transform.position - target).sqrMagnitude < 0.0004f;
		}

		// Token: 0x06005ECB RID: 24267 RVA: 0x00258130 File Offset: 0x00256330
		public bool TurnTowards(Vector3 point, float degreesPerSecond, float deltaTime)
		{
			if (this._root == null)
			{
				return true;
			}
			Transform transform = this._root.transform;
			Vector3 vector = point - transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				return true;
			}
			Quaternion quaternion = Quaternion.LookRotation(vector);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternion, degreesPerSecond * deltaTime);
			return Quaternion.Angle(transform.rotation, quaternion) < 1f;
		}

		// Token: 0x06005ECC RID: 24268 RVA: 0x002581B4 File Offset: 0x002563B4
		public void Carry(Transform item)
		{
			if (item == null || this._animator == null)
			{
				return;
			}
			Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.RightHand);
			if (boneTransform == null)
			{
				return;
			}
			item.SetParent(boneTransform, true);
			this._carried = item;
			this._carriedFromPosition = item.localPosition;
			this._carriedFromRotation = item.localRotation;
			this._carriedBlend = 0f;
		}

		// Token: 0x06005ECD RID: 24269 RVA: 0x00258224 File Offset: 0x00256424
		public void Tick(float deltaTime)
		{
			if (!this._graph.IsValid() || this._root == null || !this._root.activeInHierarchy)
			{
				return;
			}
			float num = deltaTime / this._fadeSeconds;
			for (int i = 0; i < 5; i++)
			{
				this._weights[i] = Mathf.MoveTowards(this._weights[i], (i == (int)this._pose) ? 1f : 0f, num);
			}
			this.ApplyWeights();
			if (this._pose == LongWayHomeGymShowerPhotographer.Pose.Photo)
			{
				this._photoClock += deltaTime;
				if (!this._eventsSeen && this._photoClock >= this._nextTimedPhoto)
				{
					this._nextTimedPhoto = this._photoClock + 1.1f;
					this.FirePhoto();
				}
			}
			if (this._flashTimer > 0f)
			{
				this._flashTimer = Mathf.Max(0f, this._flashTimer - deltaTime);
				this.SetFlashIntensity(this._flashTimer / this._flashDuration);
			}
			if (this._carried != null && this._carriedBlend < 1f)
			{
				this._carriedBlend = Mathf.MoveTowards(this._carriedBlend, 1f, deltaTime / 0.25f);
				float num2 = Mathf.SmoothStep(0f, 1f, this._carriedBlend);
				this._carried.localPosition = Vector3.Lerp(this._carriedFromPosition, new Vector3(0.02f, 0.06f, 0.08f), num2);
				this._carried.localRotation = Quaternion.Slerp(this._carriedFromRotation, Quaternion.Euler(-70f, 0f, 90f), num2);
			}
		}

		// Token: 0x06005ECE RID: 24270 RVA: 0x002583C8 File Offset: 0x002565C8
		private void ApplyWeights()
		{
			float num = 0f;
			for (int i = 0; i < 5; i++)
			{
				num += this._weights[i];
			}
			for (int j = 0; j < 5; j++)
			{
				this._mixer.SetInputWeight(j, (num > 0f) ? (this._weights[j] / num) : ((j == 0) ? 1f : 0f));
			}
		}

		// Token: 0x06005ECF RID: 24271 RVA: 0x00258430 File Offset: 0x00256630
		private void OnFootstep()
		{
			if (this._audio == null || this._footsteps == null || this._footsteps.Length == 0)
			{
				return;
			}
			AudioClip[] footsteps = this._footsteps;
			int footstepIndex = this._footstepIndex;
			this._footstepIndex = footstepIndex + 1;
			AudioClip audioClip = footsteps[footstepIndex % this._footsteps.Length];
			if (audioClip != null)
			{
				this._audio.PlayOneShot(audioClip, this._footstepVolume);
			}
		}

		// Token: 0x06005ED0 RID: 24272 RVA: 0x0025849A File Offset: 0x0025669A
		private void OnPhotoEvent()
		{
			this._eventsSeen = true;
			if (this._pose == LongWayHomeGymShowerPhotographer.Pose.Photo && this._weights[3] > 0.5f)
			{
				this.FirePhoto();
			}
		}

		// Token: 0x06005ED1 RID: 24273 RVA: 0x002584C4 File Offset: 0x002566C4
		private void FirePhoto()
		{
			this._flashTimer = this._flashDuration;
			this.SetFlashIntensity(1f);
			if (this._audio != null && this._shutter != null)
			{
				this._audio.PlayOneShot(this._shutter, this._shutterVolume);
			}
			Action photoTaken = this.PhotoTaken;
			if (photoTaken == null)
			{
				return;
			}
			photoTaken();
		}

		// Token: 0x06005ED2 RID: 24274 RVA: 0x0025852C File Offset: 0x0025672C
		private void SetFlashIntensity(float normalized)
		{
			for (int i = 0; i < this._flashLights.Count; i++)
			{
				if (this._flashLights[i] != null)
				{
					this._flashLights[i].intensity = this._flashIntensities[i] * normalized;
				}
			}
		}

		// Token: 0x06005ED3 RID: 24275 RVA: 0x00258584 File Offset: 0x00256784
		public void Destroy()
		{
			this.PhotoTaken = null;
			if (this._graph.IsValid())
			{
				this._graph.Destroy();
			}
			if (this._root != null)
			{
				Object.Destroy(this._root);
			}
			this._root = null;
			this._carried = null;
		}

		// Token: 0x04005FF5 RID: 24565
		private const int PoseCount = 5;

		// Token: 0x04005FF6 RID: 24566
		private static readonly HashSet<Type> VisualComponents = new HashSet<Type>
		{
			typeof(Transform),
			typeof(Animator),
			typeof(SkinnedMeshRenderer),
			typeof(MeshRenderer),
			typeof(MeshFilter),
			typeof(LODGroup),
			typeof(Light),
			typeof(AudioSource)
		};

		// Token: 0x04005FF7 RID: 24567
		private readonly AnimationClip[] _clips = new AnimationClip[5];

		// Token: 0x04005FF8 RID: 24568
		private readonly AnimationClipPlayable[] _playables = new AnimationClipPlayable[5];

		// Token: 0x04005FF9 RID: 24569
		private readonly float[] _weights = new float[5];

		// Token: 0x04005FFA RID: 24570
		private readonly List<Light> _flashLights = new List<Light>();

		// Token: 0x04005FFB RID: 24571
		private readonly List<float> _flashIntensities = new List<float>();

		// Token: 0x04005FFC RID: 24572
		private GameObject _root;

		// Token: 0x04005FFD RID: 24573
		private Animator _animator;

		// Token: 0x04005FFE RID: 24574
		private PlayableGraph _graph;

		// Token: 0x04005FFF RID: 24575
		private AnimationMixerPlayable _mixer;

		// Token: 0x04006000 RID: 24576
		private LongWayHomeGymShowerPhotographer.Pose _pose;

		// Token: 0x04006001 RID: 24577
		private float _fadeSeconds = 0.25f;

		// Token: 0x04006002 RID: 24578
		private GameObject _phone;

		// Token: 0x04006003 RID: 24579
		private AudioSource _audio;

		// Token: 0x04006004 RID: 24580
		private AudioClip _shutter;

		// Token: 0x04006005 RID: 24581
		private float _shutterVolume = 1f;

		// Token: 0x04006006 RID: 24582
		private float _flashDuration = 0.15f;

		// Token: 0x04006007 RID: 24583
		private float _flashTimer;

		// Token: 0x04006008 RID: 24584
		private bool _eventsSeen;

		// Token: 0x04006009 RID: 24585
		private float _photoClock;

		// Token: 0x0400600A RID: 24586
		private float _nextTimedPhoto;

		// Token: 0x0400600B RID: 24587
		private Transform _carried;

		// Token: 0x0400600C RID: 24588
		private AudioClip[] _footsteps;

		// Token: 0x0400600D RID: 24589
		private float _footstepVolume;

		// Token: 0x0400600E RID: 24590
		private int _footstepIndex;

		// Token: 0x0400600F RID: 24591
		private Vector3 _carriedFromPosition;

		// Token: 0x04006010 RID: 24592
		private Quaternion _carriedFromRotation;

		// Token: 0x04006011 RID: 24593
		private float _carriedBlend;

		// Token: 0x02000CBF RID: 3263
		public enum Pose
		{
			// Token: 0x04006016 RID: 24598
			Idle,
			// Token: 0x04006017 RID: 24599
			Walk,
			// Token: 0x04006018 RID: 24600
			Run,
			// Token: 0x04006019 RID: 24601
			Photo,
			// Token: 0x0400601A RID: 24602
			PickUp
		}

		// Token: 0x02000CC0 RID: 3264
		public sealed class Clips
		{
			// Token: 0x0400601B RID: 24603
			public AnimationClip idle;

			// Token: 0x0400601C RID: 24604
			public AnimationClip walk;

			// Token: 0x0400601D RID: 24605
			public AnimationClip run;

			// Token: 0x0400601E RID: 24606
			public AnimationClip photo;

			// Token: 0x0400601F RID: 24607
			public AnimationClip pickUp;

			// Token: 0x04006020 RID: 24608
			public AudioClip shutter;

			// Token: 0x04006021 RID: 24609
			public AudioClip[] footsteps;

			// Token: 0x04006022 RID: 24610
			public float footstepVolume = 0.3f;
		}
	}
}
