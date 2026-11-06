using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000038 RID: 56
	public class PrefabControlPlayable : PlayableBehaviour
	{
		// Token: 0x06000293 RID: 659 RVA: 0x00009354 File Offset: 0x00007554
		public static ScriptPlayable<PrefabControlPlayable> Create(PlayableGraph graph, GameObject prefabGameObject, Transform parentTransform)
		{
			if (prefabGameObject == null)
			{
				return ScriptPlayable<PrefabControlPlayable>.Null;
			}
			ScriptPlayable<PrefabControlPlayable> scriptPlayable = ScriptPlayable<PrefabControlPlayable>.Create(graph, 0);
			scriptPlayable.GetBehaviour().Initialize(prefabGameObject, parentTransform);
			return scriptPlayable;
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000294 RID: 660 RVA: 0x00009388 File Offset: 0x00007588
		public GameObject prefabInstance
		{
			get
			{
				return this.m_Instance;
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00009390 File Offset: 0x00007590
		public GameObject Initialize(GameObject prefabGameObject, Transform parentTransform)
		{
			if (prefabGameObject == null)
			{
				throw new ArgumentNullException("Prefab cannot be null");
			}
			if (this.m_Instance != null)
			{
				Debug.LogWarningFormat("Prefab Control Playable ({0}) has already been initialized with a Prefab ({1}).", new object[]
				{
					prefabGameObject.name,
					this.m_Instance.name
				});
			}
			else
			{
				this.m_Instance = Object.Instantiate<GameObject>(prefabGameObject, parentTransform, false);
				this.m_Instance.name = prefabGameObject.name + " [Timeline]";
				this.m_Instance.SetActive(false);
				PrefabControlPlayable.SetHideFlagsRecursive(this.m_Instance);
			}
			return this.m_Instance;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000942E File Offset: 0x0000762E
		public override void OnPlayableDestroy(Playable playable)
		{
			if (this.m_Instance)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this.m_Instance);
					return;
				}
				Object.DestroyImmediate(this.m_Instance);
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000945B File Offset: 0x0000765B
		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			if (this.m_Instance == null)
			{
				return;
			}
			this.m_Instance.SetActive(true);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00009478 File Offset: 0x00007678
		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			if (this.m_Instance != null && info.effectivePlayState == PlayState.Paused)
			{
				this.m_Instance.SetActive(false);
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000094A0 File Offset: 0x000076A0
		private static void SetHideFlagsRecursive(GameObject gameObject)
		{
			if (gameObject == null)
			{
				return;
			}
			gameObject.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
			if (!Application.isPlaying)
			{
				gameObject.hideFlags |= HideFlags.HideInHierarchy;
			}
			foreach (object obj in gameObject.transform)
			{
				PrefabControlPlayable.SetHideFlagsRecursive(((Transform)obj).gameObject);
			}
		}

		// Token: 0x040000E0 RID: 224
		private GameObject m_Instance;
	}
}
