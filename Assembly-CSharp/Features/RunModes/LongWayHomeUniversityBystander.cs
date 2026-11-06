using System;
using System.Collections.Generic;
using System.Reflection;
using Bozo.AnimeCharacters;
using Features.Audio;
using Features.MainMenu;
using Features.NPC;
using Features.NPC.Audio;
using Features.NPCs;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000D1C RID: 3356
	public sealed class LongWayHomeUniversityBystander
	{
		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x060061F6 RID: 25078 RVA: 0x002701AB File Offset: 0x0026E3AB
		// (set) Token: 0x060061F7 RID: 25079 RVA: 0x002701B3 File Offset: 0x0026E3B3
		public string ResourceName { get; private set; }

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x060061F8 RID: 25080 RVA: 0x002701BC File Offset: 0x0026E3BC
		// (set) Token: 0x060061F9 RID: 25081 RVA: 0x002701C4 File Offset: 0x0026E3C4
		public Gender Gender { get; private set; }

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x060061FA RID: 25082 RVA: 0x002701CD File Offset: 0x0026E3CD
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

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x060061FB RID: 25083 RVA: 0x002701EA File Offset: 0x0026E3EA
		public bool IsValid
		{
			get
			{
				return this._root != null && this._graph.IsValid();
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x060061FC RID: 25084 RVA: 0x00270207 File Offset: 0x0026E407
		public bool IsReacting
		{
			get
			{
				return this._reacting;
			}
		}

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x060061FD RID: 25085 RVA: 0x0027020F File Offset: 0x0026E40F
		public bool HasVoice
		{
			get
			{
				return this._voice;
			}
		}

		// Token: 0x060061FE RID: 25086 RVA: 0x00270218 File Offset: 0x0026E418
		private LongWayHomeUniversityBystander()
		{
		}

		// Token: 0x060061FF RID: 25087 RVA: 0x00270288 File Offset: 0x0026E488
		public static LongWayHomeUniversityBystander Create(Scene scene, LongWayHomeUniversityBystander.Clips clips, LongWayHomeUniversityBystander.Preference preference, ICollection<string> excludedResources, LongWayHomeUniversityBystander.Reaction reaction, bool phoneWalk, bool voice, out string error)
		{
			error = null;
			GameObject gameObject;
			NPCRosterEntry npcrosterEntry = LongWayHomeUniversityBystander.ChooseEntry(NPCRosterSession.GetCurrentSnapshot(), preference, excludedResources, out gameObject);
			if (npcrosterEntry == null || gameObject == null)
			{
				error = "The selected NPC population has no civilian for the University students.";
				return null;
			}
			GameObject gameObject2 = new GameObject("University Student (staging)");
			gameObject2.SetActive(false);
			SceneManager.MoveGameObjectToScene(gameObject2, scene);
			GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject, gameObject2.transform);
			gameObject3.name = "University Student - " + npcrosterEntry.resourceName;
			LongWayHomeUniversityBystander longWayHomeUniversityBystander = new LongWayHomeUniversityBystander();
			try
			{
				longWayHomeUniversityBystander.Build(gameObject3, npcrosterEntry, clips, reaction, phoneWalk, voice);
			}
			catch (Exception ex)
			{
				longWayHomeUniversityBystander.Destroy();
				Object.Destroy(gameObject2);
				error = "A University student could not be prepared: " + ex.Message;
				return null;
			}
			gameObject3.SetActive(false);
			gameObject3.transform.SetParent(null, false);
			Object.Destroy(gameObject2);
			return longWayHomeUniversityBystander;
		}

		// Token: 0x06006200 RID: 25088 RVA: 0x0027036C File Offset: 0x0026E56C
		private static NPCRosterEntry ChooseEntry(NPCRosterSnapshot roster, LongWayHomeUniversityBystander.Preference preference, ICollection<string> excluded, out GameObject prefab)
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
			if (preference != LongWayHomeUniversityBystander.Preference.Any)
			{
				Gender wanted = ((preference == LongWayHomeUniversityBystander.Preference.Man) ? Gender.Male : Gender.Female);
				List<NPCRosterEntry> list3 = list.FindAll((NPCRosterEntry entry) => entry.gender == wanted);
				if (list3.Count > 0)
				{
					list = list3;
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			int num = Random.Range(0, list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				NPCRosterEntry npcrosterEntry2 = list[(num + i) % list.Count];
				prefab = NPCBakedResourceIndex.LoadPrefab(npcrosterEntry2.resourceName);
				if (prefab != null && prefab.GetComponent<Animator>() != null)
				{
					return npcrosterEntry2;
				}
			}
			prefab = null;
			return null;
		}

		// Token: 0x06006201 RID: 25089 RVA: 0x002704F8 File Offset: 0x0026E6F8
		private void Build(GameObject root, NPCRosterEntry entry, LongWayHomeUniversityBystander.Clips clips, LongWayHomeUniversityBystander.Reaction reaction, bool phoneWalk, bool voice)
		{
			this._root = root;
			this.ResourceName = entry.resourceName;
			this.Gender = entry.gender;
			this._reaction = reaction;
			this._phoneWalk = phoneWalk && clips.phoneWalk != null;
			this._voices = clips.voices;
			this._voice = voice && this._voices != null;
			this._shutter = clips.shutter;
			bool flag = entry.gender == Gender.Female;
			this._clips[0] = (flag ? clips.idleFemale : clips.idleMale);
			this._clips[1] = (this._phoneWalk ? clips.phoneWalk : (flag ? clips.walkFemale : clips.walkMale));
			this._clips[2] = ((reaction == LongWayHomeUniversityBystander.Reaction.Video) ? clips.video : clips.photo);
			AnimationClip[] clips2 = this._clips;
			for (int i = 0; i < clips2.Length; i++)
			{
				if (clips2[i] == null)
				{
					throw new InvalidOperationException("a student clip is missing.");
				}
			}
			this._contacts = LongWayHomeUniversitySequence.FootContacts(this._phoneWalk ? clips.phoneWalk : clips.walkMale);
			Animator component = root.GetComponent<Animator>();
			if (component == null || component.avatar == null || !component.avatar.isHuman)
			{
				throw new InvalidOperationException("the baked civilian has no Humanoid Animator.");
			}
			NPCProp[] componentsInChildren = root.GetComponentsInChildren<NPCProp>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.SetActive(false);
			}
			this.CaptureSteps(root);
			this.CapturePhone(root);
			LongWayHomeGymShowerPhotographer.StripToVisual(root);
			LongWayHomeUniversityBystanderEvents longWayHomeUniversityBystanderEvents = root.AddComponent<LongWayHomeUniversityBystanderEvents>();
			longWayHomeUniversityBystanderEvents.Flash += this.OnFlash;
			longWayHomeUniversityBystanderEvents.VideoLightOn += delegate
			{
				this._videoTarget = 1f;
			};
			longWayHomeUniversityBystanderEvents.VideoLightOff += delegate
			{
				this._videoTarget = 0f;
			};
			this._voiceSource = root.GetComponent<AudioSource>();
			if (this._voiceSource == null)
			{
				this._voiceSource = root.AddComponent<AudioSource>();
			}
			this._voiceSource.playOnAwake = false;
			this._voiceSource.spatialBlend = 1f;
			this._voiceSource.rolloffMode = AudioRolloffMode.Linear;
			this._voiceSource.minDistance = 1f;
			this._voiceSource.maxDistance = 20f;
			GameAudioRouter.Route(this._voiceSource, GameAudioRouter.Channel.NpcReaction);
			this._sfx = root.AddComponent<AudioSource>();
			this._sfx.playOnAwake = false;
			this._sfx.spatialBlend = 0.8f;
			this._sfx.minDistance = 1.5f;
			this._sfx.maxDistance = 18f;
			GameAudioRouter.Route(this._sfx, GameAudioRouter.Channel.Sfx);
			component.runtimeAnimatorController = null;
			component.applyRootMotion = false;
			component.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			this._graph = PlayableGraph.Create("Long Way Home - University Student");
			this._graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
			this._body = AnimationMixerPlayable.Create(this._graph, 3);
			for (int j = 0; j < 3; j++)
			{
				this._playables[j] = AnimationClipPlayable.Create(this._graph, this._clips[j]);
				this._playables[j].SetApplyFootIK(false);
				this._graph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(this._playables[j], 0, this._body, j);
			}
			this._weights[0] = 1f;
			this.ApplyWeights();
			this._layers = AnimationLayerMixerPlayable.Create(this._graph, 2);
			this._graph.Connect<AnimationMixerPlayable, AnimationLayerMixerPlayable>(this._body, 0, this._layers, 0);
			this._layers.SetInputWeight(0, 1f);
			if (clips.surprisedFace != null)
			{
				this._faceMask = new AvatarMask();
				for (int k = 0; k < 13; k++)
				{
					this._faceMask.SetHumanoidBodyPartActive((AvatarMaskBodyPart)k, false);
				}
				this._face = AnimationClipPlayable.Create(this._graph, clips.surprisedFace);
				this._graph.Connect<AnimationClipPlayable, AnimationLayerMixerPlayable>(this._face, 0, this._layers, 1);
				this._layers.SetLayerMaskFromAvatarMask(1U, this._faceMask);
			}
			this._layers.SetInputWeight(1, 0f);
			AnimationPlayableOutput.Create(this._graph, "University Student", component).SetSourcePlayable(this._layers);
			if (this._phone != null)
			{
				this._phone.SetActive(false);
			}
		}

		// Token: 0x06006202 RID: 25090 RVA: 0x00270960 File Offset: 0x0026EB60
		private void CaptureSteps(GameObject root)
		{
			FootstepController componentInChildren = root.GetComponentInChildren<FootstepController>(true);
			if (componentInChildren == null)
			{
				return;
			}
			FieldInfo field = typeof(FootstepController).GetField("footstepClips", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			this._steps = ((field != null) ? field.GetValue(componentInChildren) : null) as AudioClip[];
			FieldInfo field2 = typeof(FootstepController).GetField("volume", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			object obj = ((field2 != null) ? field2.GetValue(componentInChildren) : null);
			if (obj is float)
			{
				float num = (float)obj;
				this._stepVolume = num;
			}
		}

		// Token: 0x06006203 RID: 25091 RVA: 0x002709E8 File Offset: 0x0026EBE8
		private void CapturePhone(GameObject root)
		{
			Transform transform = LongWayHomeUniversityBystander.FindDeep(root.transform, "Smartphone");
			this._phone = ((transform != null) ? transform.gameObject : null);
			if (this._phone == null)
			{
				return;
			}
			foreach (Light light in this._phone.GetComponentsInChildren<Light>(true))
			{
				light.intensity = 0f;
				light.enabled = true;
				((light.type == LightType.Spot) ? this._spots : this._points).Add(light);
			}
		}

		// Token: 0x06006204 RID: 25092 RVA: 0x00270A7C File Offset: 0x0026EC7C
		private static Transform FindDeep(Transform parent, string name)
		{
			if (parent.name == name)
			{
				return parent;
			}
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform transform = LongWayHomeUniversityBystander.FindDeep(parent.GetChild(i), name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06006205 RID: 25093 RVA: 0x00270AC4 File Offset: 0x0026ECC4
		public void Show(Vector3 from, Vector3 to, float yaw, float walkMetersPerSecond)
		{
			if (this._root == null)
			{
				return;
			}
			this._from = from;
			this._to = to;
			this._walks = LongWayHomeUniversityBystander.Flat(to - from).sqrMagnitude > 0.04f;
			this._speed = Mathf.Max(0.1f, walkMetersPerSecond);
			this._reacting = false;
			this._pause = 0f;
			this._faceWeight = (this._flash = (this._video = (this._videoTarget = 0f)));
			Vector3 vector = from;
			Quaternion quaternion = Quaternion.Euler(0f, yaw, 0f);
			if (this._walks)
			{
				vector = Vector3.Lerp(from, to, Random.value);
				this._target = ((Random.value < 0.5f) ? to : from);
				quaternion = Quaternion.LookRotation(LongWayHomeUniversityBystander.Flat(this._target - vector));
			}
			this._root.transform.SetPositionAndRotation(vector, quaternion);
			this._root.SetActive(true);
			if (this._graph.IsValid() && !this._graph.IsPlaying())
			{
				this._graph.Play();
			}
			this._pose = (this._walks ? 1 : 0);
			for (int i = 0; i < 3; i++)
			{
				this._weights[i] = ((i == this._pose) ? 1f : 0f);
			}
			this.ApplyWeights();
			this.SetTime(0, Random.Range(0f, this._clips[0].length));
			this.SetTime(1, Random.Range(0f, this._clips[1].length));
			this._lastWalkTime = this._playables[1].GetTime<AnimationClipPlayable>();
			if (this._face.IsValid<AnimationClipPlayable>())
			{
				this._layers.SetInputWeight(1, 0f);
			}
			if (this._phone != null)
			{
				this._phone.SetActive(this._walks && this._phoneWalk);
			}
			this.SetLights();
		}

		// Token: 0x06006206 RID: 25094 RVA: 0x00270CDC File Offset: 0x0026EEDC
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
			this._flash = (this._video = (this._videoTarget = 0f));
			this.SetLights();
			this._root.SetActive(false);
		}

		// Token: 0x06006207 RID: 25095 RVA: 0x00270D40 File Offset: 0x0026EF40
		public void Tick(float deltaTime, Transform ciel, float reactDistance)
		{
			if (!this._graph.IsValid() || this._root == null || !this._root.activeInHierarchy || ciel == null)
			{
				return;
			}
			Transform transform = this._root.transform;
			if (!this._reacting && this._reaction != LongWayHomeUniversityBystander.Reaction.None && LongWayHomeUniversityBystander.Notices(transform.position, ciel.position, reactDistance) && LongWayHomeUniversityBystander.Sees(transform.position, ciel))
			{
				this.StartReaction();
			}
			if (this._reacting)
			{
				LongWayHomeUniversityBystander.TurnTowards(transform, ciel.position, 240f * deltaTime);
			}
			else if (this._walks)
			{
				this.Wander(transform, deltaTime);
			}
			float num = deltaTime / this._fade;
			for (int i = 0; i < 3; i++)
			{
				this._weights[i] = Mathf.MoveTowards(this._weights[i], (i == this._pose) ? 1f : 0f, num);
			}
			this.ApplyWeights();
			this._faceWeight = Mathf.MoveTowards(this._faceWeight, this._reacting ? 1f : 0f, deltaTime / 0.25f);
			if (this._face.IsValid<AnimationClipPlayable>())
			{
				this._layers.SetInputWeight(1, this._faceWeight);
			}
			this.TickSteps();
			this.TickLights(deltaTime);
		}

		// Token: 0x06006208 RID: 25096 RVA: 0x00270E90 File Offset: 0x0026F090
		public static bool Notices(Vector3 position, Vector3 ciel, float distance)
		{
			Vector3 vector = ciel - position;
			if (Mathf.Abs(vector.y) > 2.5f)
			{
				return false;
			}
			vector.y = 0f;
			return vector.sqrMagnitude <= distance * distance;
		}

		// Token: 0x06006209 RID: 25097 RVA: 0x00270ED4 File Offset: 0x0026F0D4
		public static bool Sees(Vector3 position, Transform ciel)
		{
			Vector3 vector = position + Vector3.up * 1.55f;
			Vector3 vector2 = ciel.position + Vector3.up * 1.2f - vector;
			int num = Physics.RaycastNonAlloc(vector, vector2, LongWayHomeUniversityBystander.SightHits, vector2.magnitude, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				Transform transform = LongWayHomeUniversityBystander.SightHits[i].transform;
				if (transform != null && transform != ciel && !transform.IsChildOf(ciel))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600620A RID: 25098 RVA: 0x00270F70 File Offset: 0x0026F170
		private void StartReaction()
		{
			this._reacting = true;
			this.SetPose(2);
			this.SetTime(2, 0f);
			if (this._phone != null)
			{
				this._phone.SetActive(true);
			}
			if (this._voice)
			{
				this.PlayVoice();
			}
		}

		// Token: 0x0600620B RID: 25099 RVA: 0x00270FC0 File Offset: 0x0026F1C0
		private void Wander(Transform t, float deltaTime)
		{
			Vector3 position = t.position;
			Vector3 vector = LongWayHomeUniversityBystander.Flat(this._target - position);
			if (this._pause > 0f)
			{
				this._pause -= deltaTime;
				LongWayHomeUniversityBystander.TurnTowards(t, this._target, 180f * deltaTime);
				bool flag = Vector3.Angle(t.forward, vector) < 15f;
				if (this._pause <= 0f && !flag)
				{
					this._pause = 0.05f;
					return;
				}
				if (this._pause <= 0f)
				{
					this.SetPose(1);
				}
				return;
			}
			else
			{
				float magnitude = vector.magnitude;
				if (magnitude <= 0.05f)
				{
					this._target = (((this._target - this._to).sqrMagnitude < 0.0001f) ? this._from : this._to);
					this._pause = Random.Range(0.8f, 2f);
					this.SetPose(0);
					return;
				}
				LongWayHomeUniversityBystander.TurnTowards(t, this._target, 180f * deltaTime);
				float num = Mathf.Min(magnitude, this._speed * deltaTime * Mathf.Clamp01(this._weights[1] * 1.25f));
				Vector3 vector2 = position + vector / magnitude * num;
				vector2.y = Mathf.MoveTowards(position.y, this._target.y, num);
				t.position = vector2;
				return;
			}
		}

		// Token: 0x0600620C RID: 25100 RVA: 0x00271130 File Offset: 0x0026F330
		private static void TurnTowards(Transform t, Vector3 point, float maxDegrees)
		{
			Vector3 vector = LongWayHomeUniversityBystander.Flat(point - t.position);
			if (vector.sqrMagnitude < 0.0001f)
			{
				return;
			}
			t.rotation = Quaternion.RotateTowards(t.rotation, Quaternion.LookRotation(vector), maxDegrees);
		}

		// Token: 0x0600620D RID: 25101 RVA: 0x00271176 File Offset: 0x0026F376
		private static Vector3 Flat(Vector3 vector)
		{
			vector.y = 0f;
			return vector;
		}

		// Token: 0x0600620E RID: 25102 RVA: 0x00271188 File Offset: 0x0026F388
		private void SetPose(int pose)
		{
			this._pose = pose;
			this._fade = 0.3f;
			if (this._phone != null && !this._reacting)
			{
				this._phone.SetActive(pose == 1 && this._phoneWalk);
			}
		}

		// Token: 0x0600620F RID: 25103 RVA: 0x002711D5 File Offset: 0x0026F3D5
		private void SetTime(int slot, float seconds)
		{
			if (!this._graph.IsValid())
			{
				return;
			}
			this._playables[slot].SetTime((double)seconds);
			this._playables[slot].SetTime((double)seconds);
		}

		// Token: 0x06006210 RID: 25104 RVA: 0x0027120C File Offset: 0x0026F40C
		private void ApplyWeights()
		{
			float num = 0f;
			for (int i = 0; i < 3; i++)
			{
				num += this._weights[i];
			}
			for (int j = 0; j < 3; j++)
			{
				this._body.SetInputWeight(j, (num > 0f) ? (this._weights[j] / num) : ((j == 0) ? 1f : 0f));
			}
		}

		// Token: 0x06006211 RID: 25105 RVA: 0x00271274 File Offset: 0x0026F474
		private void PlayVoice()
		{
			WeightedAudioClip stopAndStareAudio = this._voices.GetStopAndStareAudio(this.Gender);
			if (stopAndStareAudio == null || stopAndStareAudio.clip == null || this._voiceSource == null)
			{
				return;
			}
			this._voiceSource.pitch = 1f + Random.Range(-this._voices.pitchVariation, this._voices.pitchVariation);
			this._voiceSource.PlayOneShot(stopAndStareAudio.clip, stopAndStareAudio.volume * this._voices.baseVolume);
		}

		// Token: 0x06006212 RID: 25106 RVA: 0x00271304 File Offset: 0x0026F504
		private void TickSteps()
		{
			double time = this._playables[1].GetTime<AnimationClipPlayable>();
			if (this._weights[1] > 0.5f && this._steps != null && this._steps.Length != 0 && this._sfx != null)
			{
				bool flag = false;
				foreach (float num in this._contacts)
				{
					flag |= LongWayHomeUniversitySequence.CrossedFootContact(this._lastWalkTime, time, this._clips[1].length, num);
				}
				if (flag)
				{
					AudioClip[] steps = this._steps;
					int i = this._stepIndex;
					this._stepIndex = i + 1;
					AudioClip audioClip = steps[i % this._steps.Length];
					if (audioClip != null)
					{
						this._sfx.PlayOneShot(audioClip, this._stepVolume);
					}
				}
			}
			this._lastWalkTime = time;
		}

		// Token: 0x06006213 RID: 25107 RVA: 0x002713E0 File Offset: 0x0026F5E0
		private void OnFlash()
		{
			if (!this._reacting || this._reaction != LongWayHomeUniversityBystander.Reaction.Photo || this._weights[2] < 0.5f)
			{
				return;
			}
			this._flash = 1f;
			if (this._sfx != null && this._shutter != null)
			{
				this._sfx.PlayOneShot(this._shutter);
			}
		}

		// Token: 0x06006214 RID: 25108 RVA: 0x00271448 File Offset: 0x0026F648
		private void TickLights(float deltaTime)
		{
			this._flash = Mathf.MoveTowards(this._flash, 0f, deltaTime / 0.15f);
			float num = ((this._reacting && this._reaction == LongWayHomeUniversityBystander.Reaction.Video) ? this._videoTarget : 0f);
			this._video = ((num > this._video) ? num : Mathf.MoveTowards(this._video, num, deltaTime / 0.3f));
			this.SetLights();
		}

		// Token: 0x06006215 RID: 25109 RVA: 0x002714BC File Offset: 0x0026F6BC
		private void SetLights()
		{
			float num = Mathf.Max(8f * this._flash, 4f * this._video);
			float num2 = Mathf.Max(3f * this._flash, 1.5f * this._video);
			foreach (Light light in this._spots)
			{
				if (light != null)
				{
					light.intensity = num;
				}
			}
			foreach (Light light2 in this._points)
			{
				if (light2 != null)
				{
					light2.intensity = num2;
				}
			}
		}

		// Token: 0x06006216 RID: 25110 RVA: 0x002715A4 File Offset: 0x0026F7A4
		public void Destroy()
		{
			if (this._graph.IsValid())
			{
				this._graph.Destroy();
			}
			if (this._faceMask != null)
			{
				Object.Destroy(this._faceMask);
			}
			if (this._root != null)
			{
				Object.Destroy(this._root);
			}
			this._faceMask = null;
			this._root = null;
		}

		// Token: 0x04006425 RID: 25637
		private const int Idle = 0;

		// Token: 0x04006426 RID: 25638
		private const int Walk = 1;

		// Token: 0x04006427 RID: 25639
		private const int React = 2;

		// Token: 0x04006428 RID: 25640
		private const int SlotCount = 3;

		// Token: 0x04006429 RID: 25641
		private const float FlashSpot = 8f;

		// Token: 0x0400642A RID: 25642
		private const float FlashPoint = 3f;

		// Token: 0x0400642B RID: 25643
		private const float VideoSpot = 4f;

		// Token: 0x0400642C RID: 25644
		private const float VideoPoint = 1.5f;

		// Token: 0x0400642D RID: 25645
		private const float FlashSeconds = 0.15f;

		// Token: 0x0400642E RID: 25646
		private const float VideoFadeSeconds = 0.3f;

		// Token: 0x0400642F RID: 25647
		private const float WalkTurnDegrees = 180f;

		// Token: 0x04006430 RID: 25648
		private const float ReactTurnDegrees = 240f;

		// Token: 0x04006431 RID: 25649
		private const float PoseFadeSeconds = 0.3f;

		// Token: 0x04006432 RID: 25650
		private const float FaceFadeSeconds = 0.25f;

		// Token: 0x04006433 RID: 25651
		private const float EyeHeight = 1.55f;

		// Token: 0x04006434 RID: 25652
		private const float ChestHeight = 1.2f;

		// Token: 0x04006435 RID: 25653
		private static readonly RaycastHit[] SightHits = new RaycastHit[8];

		// Token: 0x04006436 RID: 25654
		private readonly AnimationClip[] _clips = new AnimationClip[3];

		// Token: 0x04006437 RID: 25655
		private readonly AnimationClipPlayable[] _playables = new AnimationClipPlayable[3];

		// Token: 0x04006438 RID: 25656
		private readonly float[] _weights = new float[3];

		// Token: 0x04006439 RID: 25657
		private readonly List<Light> _spots = new List<Light>();

		// Token: 0x0400643A RID: 25658
		private readonly List<Light> _points = new List<Light>();

		// Token: 0x0400643B RID: 25659
		private GameObject _root;

		// Token: 0x0400643C RID: 25660
		private PlayableGraph _graph;

		// Token: 0x0400643D RID: 25661
		private AnimationMixerPlayable _body;

		// Token: 0x0400643E RID: 25662
		private AnimationLayerMixerPlayable _layers;

		// Token: 0x0400643F RID: 25663
		private AnimationClipPlayable _face;

		// Token: 0x04006440 RID: 25664
		private AvatarMask _faceMask;

		// Token: 0x04006441 RID: 25665
		private GameObject _phone;

		// Token: 0x04006442 RID: 25666
		private AudioSource _voiceSource;

		// Token: 0x04006443 RID: 25667
		private AudioSource _sfx;

		// Token: 0x04006444 RID: 25668
		private AudioClip _shutter;

		// Token: 0x04006445 RID: 25669
		private AudioClip[] _steps;

		// Token: 0x04006446 RID: 25670
		private float _stepVolume = 0.3f;

		// Token: 0x04006447 RID: 25671
		private int _stepIndex;

		// Token: 0x04006448 RID: 25672
		private float[] _contacts;

		// Token: 0x04006449 RID: 25673
		private double _lastWalkTime;

		// Token: 0x0400644A RID: 25674
		private NPCReactionAudioProfileSO _voices;

		// Token: 0x0400644B RID: 25675
		private LongWayHomeUniversityBystander.Reaction _reaction;

		// Token: 0x0400644C RID: 25676
		private bool _phoneWalk;

		// Token: 0x0400644D RID: 25677
		private bool _voice;

		// Token: 0x0400644E RID: 25678
		private Vector3 _from;

		// Token: 0x0400644F RID: 25679
		private Vector3 _to;

		// Token: 0x04006450 RID: 25680
		private Vector3 _target;

		// Token: 0x04006451 RID: 25681
		private bool _walks;

		// Token: 0x04006452 RID: 25682
		private bool _reacting;

		// Token: 0x04006453 RID: 25683
		private int _pose;

		// Token: 0x04006454 RID: 25684
		private float _fade = 0.3f;

		// Token: 0x04006455 RID: 25685
		private float _speed = 1.2f;

		// Token: 0x04006456 RID: 25686
		private float _pause;

		// Token: 0x04006457 RID: 25687
		private float _faceWeight;

		// Token: 0x04006458 RID: 25688
		private float _flash;

		// Token: 0x04006459 RID: 25689
		private float _video;

		// Token: 0x0400645A RID: 25690
		private float _videoTarget;

		// Token: 0x02000D1D RID: 3357
		public enum Reaction
		{
			// Token: 0x0400645E RID: 25694
			None,
			// Token: 0x0400645F RID: 25695
			Photo,
			// Token: 0x04006460 RID: 25696
			Video
		}

		// Token: 0x02000D1E RID: 3358
		public enum Preference
		{
			// Token: 0x04006462 RID: 25698
			Any,
			// Token: 0x04006463 RID: 25699
			Man,
			// Token: 0x04006464 RID: 25700
			Woman
		}

		// Token: 0x02000D1F RID: 3359
		public sealed class Clips
		{
			// Token: 0x04006465 RID: 25701
			public AnimationClip idleMale;

			// Token: 0x04006466 RID: 25702
			public AnimationClip idleFemale;

			// Token: 0x04006467 RID: 25703
			public AnimationClip walkMale;

			// Token: 0x04006468 RID: 25704
			public AnimationClip walkFemale;

			// Token: 0x04006469 RID: 25705
			public AnimationClip phoneWalk;

			// Token: 0x0400646A RID: 25706
			public AnimationClip photo;

			// Token: 0x0400646B RID: 25707
			public AnimationClip video;

			// Token: 0x0400646C RID: 25708
			public AnimationClip surprisedFace;

			// Token: 0x0400646D RID: 25709
			public AudioClip shutter;

			// Token: 0x0400646E RID: 25710
			public NPCReactionAudioProfileSO voices;
		}
	}
}
