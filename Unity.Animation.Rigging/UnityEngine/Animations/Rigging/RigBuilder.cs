using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200005D RID: 93
	[RequireComponent(typeof(Animator))]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[AddComponentMenu("Animation Rigging/Setup/Rig Builder")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/RiggingWorkflow.html#rig-builder-component")]
	public class RigBuilder : MonoBehaviour, IAnimationWindowPreview, IRigEffectorHolder
	{
		// Token: 0x0600025E RID: 606 RVA: 0x00007C95 File Offset: 0x00005E95
		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				this.Build();
			}
			RigBuilder.OnAddRigBuilderCallback onAddRigBuilderCallback = RigBuilder.onAddRigBuilder;
			if (onAddRigBuilderCallback == null)
			{
				return;
			}
			onAddRigBuilderCallback(this);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00007CB5 File Offset: 0x00005EB5
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				this.Clear();
			}
			RigBuilder.OnRemoveRigBuilderCallback onRemoveRigBuilderCallback = RigBuilder.onRemoveRigBuilder;
			if (onRemoveRigBuilderCallback == null)
			{
				return;
			}
			onRemoveRigBuilderCallback(this);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00007CD4 File Offset: 0x00005ED4
		private void OnDestroy()
		{
			this.Clear();
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00007CDC File Offset: 0x00005EDC
		public void Evaluate(float deltaTime)
		{
			if (!this.graph.IsValid())
			{
				return;
			}
			this.SyncLayers();
			this.graph.Evaluate(deltaTime);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00007D10 File Offset: 0x00005F10
		private void Update()
		{
			if (!this.graph.IsValid())
			{
				return;
			}
			this.SyncLayers();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00007D34 File Offset: 0x00005F34
		public void SyncLayers()
		{
			if (this.m_RuntimeRigLayers == null)
			{
				return;
			}
			this.syncSceneToStreamLayer.Update(this.m_RuntimeRigLayers);
			int i = 0;
			int num = this.m_RuntimeRigLayers.Length;
			while (i < num)
			{
				if (this.m_RuntimeRigLayers[i].IsValid() && this.m_RuntimeRigLayers[i].active)
				{
					this.m_RuntimeRigLayers[i].Update();
				}
				i++;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00007D9C File Offset: 0x00005F9C
		public bool Build()
		{
			if (this.m_IsInPreview)
			{
				return false;
			}
			this.Clear();
			Animator component = base.GetComponent<Animator>();
			if (component == null || this.layers.Count == 0)
			{
				return false;
			}
			IRigLayer[] array = this.layers.ToArray();
			this.m_RuntimeRigLayers = array;
			this.graph = RigBuilderUtils.BuildPlayableGraph(component, this.m_RuntimeRigLayers, this.syncSceneToStreamLayer);
			if (!this.graph.IsValid())
			{
				return false;
			}
			this.graph.Play();
			return true;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00007E24 File Offset: 0x00006024
		public bool Build(PlayableGraph graph)
		{
			if (this.m_IsInPreview)
			{
				return false;
			}
			this.Clear();
			Animator component = base.GetComponent<Animator>();
			if (component == null || this.layers.Count == 0)
			{
				return false;
			}
			IRigLayer[] array = this.layers.ToArray();
			this.m_RuntimeRigLayers = array;
			RigBuilderUtils.BuildPlayableGraph(graph, component, this.m_RuntimeRigLayers, this.syncSceneToStreamLayer);
			return true;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00007E88 File Offset: 0x00006088
		public void Clear()
		{
			if (this.m_IsInPreview)
			{
				return;
			}
			if (this.graph.IsValid())
			{
				this.graph.Destroy();
			}
			if (this.m_RuntimeRigLayers != null)
			{
				IRigLayer[] runtimeRigLayers = this.m_RuntimeRigLayers;
				for (int i = 0; i < runtimeRigLayers.Length; i++)
				{
					runtimeRigLayers[i].Reset();
				}
				this.m_RuntimeRigLayers = null;
			}
			this.syncSceneToStreamLayer.Reset();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00007EF4 File Offset: 0x000060F4
		public void StartPreview()
		{
			this.m_IsInPreview = true;
			if (!base.enabled)
			{
				return;
			}
			if (this.m_RuntimeRigLayers == null)
			{
				IRigLayer[] array = this.layers.ToArray();
				this.m_RuntimeRigLayers = array;
			}
			Animator component = base.GetComponent<Animator>();
			if (component != null)
			{
				IRigLayer[] array = this.m_RuntimeRigLayers;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Initialize(component);
				}
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00007F5B File Offset: 0x0000615B
		public void StopPreview()
		{
			this.m_IsInPreview = false;
			if (!base.enabled)
			{
				return;
			}
			if (Application.isPlaying)
			{
				return;
			}
			this.Clear();
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00007F7C File Offset: 0x0000617C
		public void UpdatePreviewGraph(PlayableGraph graph)
		{
			if (!base.enabled)
			{
				return;
			}
			if (!graph.IsValid() || this.m_RuntimeRigLayers == null)
			{
				return;
			}
			this.syncSceneToStreamLayer.Update(this.m_RuntimeRigLayers);
			foreach (IRigLayer rigLayer in this.m_RuntimeRigLayers)
			{
				if (rigLayer.IsValid() && rigLayer.active)
				{
					rigLayer.Update();
				}
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00007FE4 File Offset: 0x000061E4
		public Playable BuildPreviewGraph(PlayableGraph graph, Playable inputPlayable)
		{
			if (!base.enabled)
			{
				return inputPlayable;
			}
			if (this.m_RuntimeRigLayers == null)
			{
				this.StartPreview();
			}
			Animator component = base.GetComponent<Animator>();
			if (component == null || this.m_RuntimeRigLayers == null || this.m_RuntimeRigLayers.Length == 0)
			{
				return inputPlayable;
			}
			foreach (RigBuilderUtils.PlayableChain playableChain in RigBuilderUtils.BuildPlayables(component, graph, this.m_RuntimeRigLayers, this.syncSceneToStreamLayer))
			{
				if (playableChain.playables != null && playableChain.playables.Length != 0)
				{
					playableChain.playables[0].AddInput(inputPlayable, 0, 1f);
					inputPlayable = playableChain.playables[playableChain.playables.Length - 1];
				}
			}
			return inputPlayable;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600026B RID: 619 RVA: 0x000080B4 File Offset: 0x000062B4
		// (set) Token: 0x0600026C RID: 620 RVA: 0x000080CF File Offset: 0x000062CF
		public List<RigLayer> layers
		{
			get
			{
				if (this.m_RigLayers == null)
				{
					this.m_RigLayers = new List<RigLayer>();
				}
				return this.m_RigLayers;
			}
			set
			{
				this.m_RigLayers = value;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600026D RID: 621 RVA: 0x000080D8 File Offset: 0x000062D8
		// (set) Token: 0x0600026E RID: 622 RVA: 0x000080F3 File Offset: 0x000062F3
		private SyncSceneToStreamLayer syncSceneToStreamLayer
		{
			get
			{
				if (this.m_SyncSceneToStreamLayer == null)
				{
					this.m_SyncSceneToStreamLayer = new SyncSceneToStreamLayer();
				}
				return this.m_SyncSceneToStreamLayer;
			}
			set
			{
				this.m_SyncSceneToStreamLayer = value;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600026F RID: 623 RVA: 0x000080FC File Offset: 0x000062FC
		// (set) Token: 0x06000270 RID: 624 RVA: 0x00008104 File Offset: 0x00006304
		public PlayableGraph graph { get; private set; }

		// Token: 0x040000FF RID: 255
		[SerializeField]
		private List<RigLayer> m_RigLayers;

		// Token: 0x04000100 RID: 256
		private IRigLayer[] m_RuntimeRigLayers;

		// Token: 0x04000101 RID: 257
		private SyncSceneToStreamLayer m_SyncSceneToStreamLayer;

		// Token: 0x04000102 RID: 258
		[SerializeField]
		private List<RigEffectorData> m_Effectors = new List<RigEffectorData>();

		// Token: 0x04000103 RID: 259
		private bool m_IsInPreview;

		// Token: 0x04000104 RID: 260
		public static RigBuilder.OnAddRigBuilderCallback onAddRigBuilder;

		// Token: 0x04000105 RID: 261
		public static RigBuilder.OnRemoveRigBuilderCallback onRemoveRigBuilder;

		// Token: 0x02000083 RID: 131
		// (Invoke) Token: 0x06000315 RID: 789
		public delegate void OnAddRigBuilderCallback(RigBuilder rigBuilder);

		// Token: 0x02000084 RID: 132
		// (Invoke) Token: 0x06000319 RID: 793
		public delegate void OnRemoveRigBuilderCallback(RigBuilder rigBuilder);
	}
}
