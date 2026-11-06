using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x0200001A RID: 26
	[NotKeyable]
	[Serializable]
	public class ControlPlayableAsset : PlayableAsset, IPropertyPreview, ITimelineClipAsset
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00006B2E File Offset: 0x00004D2E
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00006B36 File Offset: 0x00004D36
		internal bool controllingDirectors { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00006B3F File Offset: 0x00004D3F
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00006B47 File Offset: 0x00004D47
		internal bool controllingParticles { get; private set; }

		// Token: 0x060001AA RID: 426 RVA: 0x00006B50 File Offset: 0x00004D50
		public void OnEnable()
		{
			if (this.particleRandomSeed == 0U)
			{
				this.particleRandomSeed = (uint)Random.Range(1, 10000);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00006B6B File Offset: 0x00004D6B
		public override double duration
		{
			get
			{
				return this.m_Duration;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00006B73 File Offset: 0x00004D73
		public ClipCaps clipCaps
		{
			get
			{
				return ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | (this.m_SupportLoop ? ClipCaps.Looping : ClipCaps.None);
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00006B84 File Offset: 0x00004D84
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			if (this.prefabGameObject != null)
			{
				if (ControlPlayableAsset.s_CreatedPrefabs.Contains(this.prefabGameObject))
				{
					Debug.LogWarningFormat("Control Track Clip ({0}) is causing a prefab to instantiate itself recursively. Aborting further instances.", new object[] { base.name });
					return Playable.Create(graph, 0);
				}
				ControlPlayableAsset.s_CreatedPrefabs.Add(this.prefabGameObject);
			}
			Playable playable = Playable.Null;
			List<Playable> list = new List<Playable>();
			GameObject gameObject = this.sourceGameObject.Resolve(graph.GetResolver());
			if (this.prefabGameObject != null)
			{
				Transform transform = ((gameObject != null) ? gameObject.transform : null);
				ScriptPlayable<PrefabControlPlayable> scriptPlayable = PrefabControlPlayable.Create(graph, this.prefabGameObject, transform);
				gameObject = scriptPlayable.GetBehaviour().prefabInstance;
				list.Add(scriptPlayable);
			}
			this.m_Duration = PlayableBinding.DefaultDuration;
			this.m_SupportLoop = false;
			this.controllingParticles = false;
			this.controllingDirectors = false;
			if (gameObject != null)
			{
				IList<PlayableDirector> list3;
				if (!this.updateDirector)
				{
					IList<PlayableDirector> list2 = ControlPlayableAsset.k_EmptyDirectorsList;
					list3 = list2;
				}
				else
				{
					list3 = this.GetComponent<PlayableDirector>(gameObject);
				}
				IList<PlayableDirector> list4 = list3;
				IList<ParticleSystem> list6;
				if (!this.updateParticle)
				{
					IList<ParticleSystem> list5 = ControlPlayableAsset.k_EmptyParticlesList;
					list6 = list5;
				}
				else
				{
					list6 = this.GetControllableParticleSystems(gameObject);
				}
				IList<ParticleSystem> list7 = list6;
				this.UpdateDurationAndLoopFlag(list4, list7);
				PlayableDirector component = go.GetComponent<PlayableDirector>();
				if (component != null)
				{
					this.m_ControlDirectorAsset = component.playableAsset;
				}
				if (go == gameObject && this.prefabGameObject == null)
				{
					Debug.LogWarningFormat("Control Playable ({0}) is referencing the same PlayableDirector component than the one in which it is playing.", new object[] { base.name });
					this.active = false;
					if (!this.searchHierarchy)
					{
						this.updateDirector = false;
					}
				}
				if (this.active)
				{
					this.CreateActivationPlayable(gameObject, graph, list);
				}
				if (this.updateDirector)
				{
					this.SearchHierarchyAndConnectDirector(list4, graph, list, this.prefabGameObject != null);
				}
				if (this.updateParticle)
				{
					this.SearchHierarchyAndConnectParticleSystem(list7, graph, list);
				}
				if (this.updateITimeControl)
				{
					ControlPlayableAsset.SearchHierarchyAndConnectControlableScripts(ControlPlayableAsset.GetControlableScripts(gameObject), graph, list);
				}
				playable = ControlPlayableAsset.ConnectPlayablesToMixer(graph, list);
			}
			if (this.prefabGameObject != null)
			{
				ControlPlayableAsset.s_CreatedPrefabs.Remove(this.prefabGameObject);
			}
			if (!playable.IsValid<Playable>())
			{
				playable = Playable.Create(graph, 0);
			}
			return playable;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00006DB0 File Offset: 0x00004FB0
		private static Playable ConnectPlayablesToMixer(PlayableGraph graph, List<Playable> playables)
		{
			Playable playable = Playable.Create(graph, playables.Count);
			for (int num = 0; num != playables.Count; num++)
			{
				ControlPlayableAsset.ConnectMixerAndPlayable(graph, playable, playables[num], num);
			}
			playable.SetPropagateSetTime(true);
			return playable;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00006DF4 File Offset: 0x00004FF4
		private void CreateActivationPlayable(GameObject root, PlayableGraph graph, List<Playable> outplayables)
		{
			ScriptPlayable<ActivationControlPlayable> scriptPlayable = ActivationControlPlayable.Create(graph, root, this.postPlayback);
			if (scriptPlayable.IsValid<ScriptPlayable<ActivationControlPlayable>>())
			{
				outplayables.Add(scriptPlayable);
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00006E24 File Offset: 0x00005024
		private void SearchHierarchyAndConnectParticleSystem(IEnumerable<ParticleSystem> particleSystems, PlayableGraph graph, List<Playable> outplayables)
		{
			foreach (ParticleSystem particleSystem in particleSystems)
			{
				if (particleSystem != null)
				{
					this.controllingParticles = true;
					outplayables.Add(ParticleControlPlayable.Create(graph, particleSystem, this.particleRandomSeed));
				}
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00006E90 File Offset: 0x00005090
		private void SearchHierarchyAndConnectDirector(IEnumerable<PlayableDirector> directors, PlayableGraph graph, List<Playable> outplayables, bool disableSelfReferences)
		{
			foreach (PlayableDirector playableDirector in directors)
			{
				if (playableDirector != null)
				{
					if (playableDirector.playableAsset != this.m_ControlDirectorAsset)
					{
						outplayables.Add(DirectorControlPlayable.Create(graph, playableDirector));
						this.controllingDirectors = true;
					}
					else if (disableSelfReferences)
					{
						playableDirector.enabled = false;
					}
				}
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00006F14 File Offset: 0x00005114
		private static void SearchHierarchyAndConnectControlableScripts(IEnumerable<MonoBehaviour> controlableScripts, PlayableGraph graph, List<Playable> outplayables)
		{
			foreach (MonoBehaviour monoBehaviour in controlableScripts)
			{
				outplayables.Add(TimeControlPlayable.Create(graph, (ITimeControl)monoBehaviour));
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00006F6C File Offset: 0x0000516C
		private static void ConnectMixerAndPlayable(PlayableGraph graph, Playable mixer, Playable playable, int portIndex)
		{
			graph.Connect<Playable, Playable>(playable, 0, mixer, portIndex);
			mixer.SetInputWeight(playable, 1f);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00006F88 File Offset: 0x00005188
		internal IList<T> GetComponent<T>(GameObject gameObject)
		{
			List<T> list = new List<T>();
			if (gameObject != null)
			{
				if (this.searchHierarchy)
				{
					gameObject.GetComponentsInChildren<T>(true, list);
				}
				else
				{
					gameObject.GetComponents<T>(list);
				}
			}
			return list;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00006FBE File Offset: 0x000051BE
		internal static IEnumerable<MonoBehaviour> GetControlableScripts(GameObject root)
		{
			if (root == null)
			{
				yield break;
			}
			foreach (MonoBehaviour monoBehaviour in root.GetComponentsInChildren<MonoBehaviour>())
			{
				if (monoBehaviour is ITimeControl)
				{
					yield return monoBehaviour;
				}
			}
			MonoBehaviour[] array = null;
			yield break;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00006FD0 File Offset: 0x000051D0
		internal void UpdateDurationAndLoopFlag(IList<PlayableDirector> directors, IList<ParticleSystem> particleSystems)
		{
			if (directors.Count == 0 && particleSystems.Count == 0)
			{
				return;
			}
			double num = double.NegativeInfinity;
			bool flag = false;
			foreach (PlayableDirector playableDirector in directors)
			{
				if (playableDirector.playableAsset != null)
				{
					double num2 = playableDirector.playableAsset.duration;
					if (playableDirector.playableAsset is TimelineAsset && num2 > 0.0)
					{
						num2 = (double)((DiscreteTime)num2).OneTickAfter();
					}
					num = Math.Max(num, num2);
					flag = flag || playableDirector.extrapolationMode == DirectorWrapMode.Loop;
				}
			}
			foreach (ParticleSystem particleSystem in particleSystems)
			{
				num = Math.Max(num, (double)particleSystem.main.duration);
				flag = flag || particleSystem.main.loop;
			}
			this.m_Duration = (double.IsNegativeInfinity(num) ? PlayableBinding.DefaultDuration : num);
			this.m_SupportLoop = flag;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00007118 File Offset: 0x00005318
		private IList<ParticleSystem> GetControllableParticleSystems(GameObject go)
		{
			List<ParticleSystem> list = new List<ParticleSystem>();
			if (this.searchHierarchy || go.GetComponent<ParticleSystem>() != null)
			{
				ControlPlayableAsset.GetControllableParticleSystems(go.transform, list, ControlPlayableAsset.s_SubEmitterCollector);
				ControlPlayableAsset.s_SubEmitterCollector.Clear();
			}
			return list;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00007160 File Offset: 0x00005360
		private static void GetControllableParticleSystems(Transform t, ICollection<ParticleSystem> roots, HashSet<ParticleSystem> subEmitters)
		{
			ParticleSystem component = t.GetComponent<ParticleSystem>();
			if (component != null && !subEmitters.Contains(component))
			{
				roots.Add(component);
				ControlPlayableAsset.CacheSubEmitters(component, subEmitters);
			}
			for (int i = 0; i < t.childCount; i++)
			{
				ControlPlayableAsset.GetControllableParticleSystems(t.GetChild(i), roots, subEmitters);
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000071B4 File Offset: 0x000053B4
		private static void CacheSubEmitters(ParticleSystem ps, HashSet<ParticleSystem> subEmitters)
		{
			if (ps == null)
			{
				return;
			}
			for (int i = 0; i < ps.subEmitters.subEmittersCount; i++)
			{
				subEmitters.Add(ps.subEmitters.GetSubEmitterSystem(i));
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000071FC File Offset: 0x000053FC
		public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		{
			if (director == null)
			{
				return;
			}
			if (ControlPlayableAsset.s_ProcessedDirectors.Contains(director))
			{
				return;
			}
			ControlPlayableAsset.s_ProcessedDirectors.Add(director);
			GameObject gameObject = this.sourceGameObject.Resolve(director);
			if (gameObject != null)
			{
				if (this.updateParticle)
				{
					ControlPlayableAsset.PreviewParticles(driver, gameObject.GetComponentsInChildren<ParticleSystem>(true));
				}
				if (this.active)
				{
					ControlPlayableAsset.PreviewActivation(driver, new GameObject[] { gameObject });
				}
				if (this.updateITimeControl)
				{
					ControlPlayableAsset.PreviewTimeControl(driver, director, ControlPlayableAsset.GetControlableScripts(gameObject));
				}
				if (this.updateDirector)
				{
					ControlPlayableAsset.PreviewDirectors(driver, this.GetComponent<PlayableDirector>(gameObject));
				}
			}
			ControlPlayableAsset.s_ProcessedDirectors.Remove(director);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000072A8 File Offset: 0x000054A8
		internal static void PreviewParticles(IPropertyCollector driver, IEnumerable<ParticleSystem> particles)
		{
			foreach (ParticleSystem particleSystem in particles)
			{
				driver.AddFromName<ParticleSystem>(particleSystem.gameObject, "randomSeed");
				driver.AddFromName<ParticleSystem>(particleSystem.gameObject, "autoRandomSeed");
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000730C File Offset: 0x0000550C
		internal static void PreviewActivation(IPropertyCollector driver, IEnumerable<GameObject> objects)
		{
			foreach (GameObject gameObject in objects)
			{
				driver.AddFromName(gameObject, "m_IsActive");
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000735C File Offset: 0x0000555C
		internal static void PreviewTimeControl(IPropertyCollector driver, PlayableDirector director, IEnumerable<MonoBehaviour> scripts)
		{
			foreach (MonoBehaviour monoBehaviour in scripts)
			{
				IPropertyPreview propertyPreview = monoBehaviour as IPropertyPreview;
				if (propertyPreview != null)
				{
					propertyPreview.GatherProperties(director, driver);
				}
				else
				{
					driver.AddFromComponent(monoBehaviour.gameObject, monoBehaviour);
				}
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000073C0 File Offset: 0x000055C0
		internal static void PreviewDirectors(IPropertyCollector driver, IEnumerable<PlayableDirector> directors)
		{
			foreach (PlayableDirector playableDirector in directors)
			{
				if (!(playableDirector == null))
				{
					TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset;
					if (!(timelineAsset == null))
					{
						timelineAsset.GatherProperties(playableDirector, driver);
					}
				}
			}
		}

		// Token: 0x04000098 RID: 152
		private const int k_MaxRandInt = 10000;

		// Token: 0x04000099 RID: 153
		private static readonly List<PlayableDirector> k_EmptyDirectorsList = new List<PlayableDirector>(0);

		// Token: 0x0400009A RID: 154
		private static readonly List<ParticleSystem> k_EmptyParticlesList = new List<ParticleSystem>(0);

		// Token: 0x0400009B RID: 155
		private static readonly HashSet<ParticleSystem> s_SubEmitterCollector = new HashSet<ParticleSystem>();

		// Token: 0x0400009C RID: 156
		[SerializeField]
		public ExposedReference<GameObject> sourceGameObject;

		// Token: 0x0400009D RID: 157
		[SerializeField]
		public GameObject prefabGameObject;

		// Token: 0x0400009E RID: 158
		[SerializeField]
		public bool updateParticle = true;

		// Token: 0x0400009F RID: 159
		[SerializeField]
		public uint particleRandomSeed;

		// Token: 0x040000A0 RID: 160
		[SerializeField]
		public bool updateDirector = true;

		// Token: 0x040000A1 RID: 161
		[SerializeField]
		public bool updateITimeControl = true;

		// Token: 0x040000A2 RID: 162
		[SerializeField]
		public bool searchHierarchy;

		// Token: 0x040000A3 RID: 163
		[SerializeField]
		public bool active = true;

		// Token: 0x040000A4 RID: 164
		[SerializeField]
		public ActivationControlPlayable.PostPlaybackState postPlayback = ActivationControlPlayable.PostPlaybackState.Revert;

		// Token: 0x040000A5 RID: 165
		private PlayableAsset m_ControlDirectorAsset;

		// Token: 0x040000A6 RID: 166
		private double m_Duration = PlayableBinding.DefaultDuration;

		// Token: 0x040000A7 RID: 167
		private bool m_SupportLoop;

		// Token: 0x040000A8 RID: 168
		private static HashSet<PlayableDirector> s_ProcessedDirectors = new HashSet<PlayableDirector>();

		// Token: 0x040000A9 RID: 169
		private static HashSet<GameObject> s_CreatedPrefabs = new HashSet<GameObject>();
	}
}
