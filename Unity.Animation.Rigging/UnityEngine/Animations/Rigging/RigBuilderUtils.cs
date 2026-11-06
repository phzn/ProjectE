using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200005E RID: 94
	internal static class RigBuilderUtils
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00008120 File Offset: 0x00006320
		public static Playable[] BuildRigPlayables(PlayableGraph graph, IRigLayer layer)
		{
			if (layer == null || layer.jobs == null || layer.jobs.Length == 0)
			{
				return null;
			}
			int num = layer.jobs.Length;
			Playable[] array = new Playable[num];
			for (int i = 0; i < num; i++)
			{
				IAnimationJobBinder binder = layer.constraints[i].binder;
				array[i] = binder.CreatePlayable(graph, layer.jobs[i]);
			}
			for (int j = 1; j < num; j++)
			{
				array[j].AddInput(array[j - 1], 0, 1f);
			}
			return array;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x000081B8 File Offset: 0x000063B8
		public static IEnumerable<RigBuilderUtils.PlayableChain> BuildPlayables(Animator animator, PlayableGraph graph, IList<IRigLayer> layers, SyncSceneToStreamLayer syncSceneToStreamLayer)
		{
			RigBuilderUtils.PlayableChain[] array = new RigBuilderUtils.PlayableChain[layers.Count + 1];
			int num = 1;
			foreach (IRigLayer rigLayer in layers)
			{
				RigBuilderUtils.PlayableChain playableChain = default(RigBuilderUtils.PlayableChain);
				playableChain.name = rigLayer.name;
				if (rigLayer.Initialize(animator))
				{
					playableChain.playables = RigBuilderUtils.BuildRigPlayables(graph, rigLayer);
				}
				array[num++] = playableChain;
			}
			if (syncSceneToStreamLayer.Initialize(animator, layers) && syncSceneToStreamLayer.IsValid())
			{
				array[0] = new RigBuilderUtils.PlayableChain
				{
					name = "syncSceneToStream",
					playables = new Playable[] { RigUtils.syncSceneToStreamBinder.CreatePlayable(graph, syncSceneToStreamLayer.job) }
				};
			}
			return array;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000829C File Offset: 0x0000649C
		public static PlayableGraph BuildPlayableGraph(Animator animator, IList<IRigLayer> layers, SyncSceneToStreamLayer syncSceneToStreamLayer)
		{
			PlayableGraph playableGraph = PlayableGraph.Create(animator.gameObject.transform.name + "_Rigs");
			playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
			RigBuilderUtils.BuildPlayableGraph(playableGraph, animator, layers, syncSceneToStreamLayer);
			return playableGraph;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x000082DC File Offset: 0x000064DC
		public static void BuildPlayableGraph(PlayableGraph graph, Animator animator, IList<IRigLayer> layers, SyncSceneToStreamLayer syncSceneToStreamLayer)
		{
			foreach (RigBuilderUtils.PlayableChain playableChain in RigBuilderUtils.BuildPlayables(animator, graph, layers, syncSceneToStreamLayer))
			{
				if (playableChain.IsValid())
				{
					AnimationPlayableOutput animationPlayableOutput = AnimationPlayableOutput.Create(graph, string.Format("{0}-Output", playableChain.name), animator);
					animationPlayableOutput.SetAnimationStreamSource(AnimationStreamSource.PreviousInputs);
					animationPlayableOutput.SetSortingOrder(RigBuilderUtils.k_AnimationOutputPriority);
					animationPlayableOutput.SetSourcePlayable(playableChain.playables[playableChain.playables.Length - 1]);
				}
			}
		}

		// Token: 0x04000107 RID: 263
		private static readonly ushort k_AnimationOutputPriority = 1000;

		// Token: 0x02000085 RID: 133
		public struct PlayableChain
		{
			// Token: 0x0600031C RID: 796 RVA: 0x0000A651 File Offset: 0x00008851
			public bool IsValid()
			{
				return this.playables != null && this.playables.Length != 0;
			}

			// Token: 0x0400016F RID: 367
			public string name;

			// Token: 0x04000170 RID: 368
			public Playable[] playables;
		}
	}
}
