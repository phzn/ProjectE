using System;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

namespace Features.Detection
{
	// Token: 0x02001251 RID: 4689
	internal sealed class CoverPoseAnimationStreamRelease : IDisposable
	{
		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x060091D4 RID: 37332 RVA: 0x003BC2B0 File Offset: 0x003BA4B0
		public bool WasEvaluated
		{
			get
			{
				return this._evaluations.IsCreated && this._evaluations[0] > 0;
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x060091D5 RID: 37333 RVA: 0x003BC2D0 File Offset: 0x003BA4D0
		public int WrittenProperties
		{
			get
			{
				if (!this._evaluations.IsCreated)
				{
					return 0;
				}
				return this._evaluations[1];
			}
		}

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x060091D6 RID: 37334 RVA: 0x003BC2ED File Offset: 0x003BA4ED
		public int SkippedProperties
		{
			get
			{
				if (!this._evaluations.IsCreated)
				{
					return 0;
				}
				return this._evaluations[2];
			}
		}

		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x060091D7 RID: 37335 RVA: 0x003BC30A File Offset: 0x003BA50A
		public bool Finished
		{
			get
			{
				return !this._graph.IsValid() || !this._playable.IsValid<AnimationScriptPlayable>() || this.WasEvaluated;
			}
		}

		// Token: 0x060091D8 RID: 37336 RVA: 0x003BC330 File Offset: 0x003BA530
		public CoverPoseAnimationStreamRelease(Animator animator, PlayableGraph graph, List<CoverBlendShapeEntry> entries)
		{
			this._graph = graph;
			try
			{
				this._handles = new NativeArray<PropertyStreamHandle>(entries.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this._values = new NativeArray<float>(entries.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this._evaluations = new NativeArray<int>(3, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < entries.Count; i++)
				{
					CoverBlendShapeEntry coverBlendShapeEntry = entries[i];
					SkinnedMeshRenderer skinnedMeshRenderer = ((coverBlendShapeEntry != null) ? coverBlendShapeEntry.TargetRenderer : null);
					if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null) && skinnedMeshRenderer.transform.IsChildOf(animator.transform))
					{
						int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(coverBlendShapeEntry.BlendShapeName);
						if (blendShapeIndex >= 0)
						{
							this._handles[i] = animator.BindStreamProperty(skinnedMeshRenderer.transform, typeof(SkinnedMeshRenderer), "blendShape." + coverBlendShapeEntry.BlendShapeName);
							BlendShapeManager blendShapeManager = BlendShapeManager.ResolveForRig(skinnedMeshRenderer);
							float num;
							this._values[i] = ((blendShapeManager != null) ? (blendShapeManager.TryGetAuthoredBlendShapeValue(coverBlendShapeEntry.BlendShapeName, out num) ? num : 0f) : skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex));
						}
					}
				}
				this._playable = AnimationScriptPlayable.Create<CoverPoseAnimationStreamRelease.ReleaseJob>(graph, new CoverPoseAnimationStreamRelease.ReleaseJob
				{
					Handles = this._handles,
					Values = this._values,
					Evaluations = this._evaluations
				}, 0);
				this._output = AnimationPlayableOutput.Create(graph, "Cover deformation release", animator);
				this._output.SetAnimationStreamSource(AnimationStreamSource.PreviousInputs);
				this._output.SetSortingOrder(1001);
				this._output.SetSourcePlayable(this._playable);
			}
			catch
			{
				this.Dispose();
				throw;
			}
		}

		// Token: 0x060091D9 RID: 37337 RVA: 0x003BC50C File Offset: 0x003BA70C
		public void Dispose()
		{
			if (this._graph.IsValid())
			{
				if (this._output.IsOutputValid<AnimationPlayableOutput>())
				{
					this._graph.DestroyOutput<AnimationPlayableOutput>(this._output);
				}
				if (this._playable.IsValid<AnimationScriptPlayable>())
				{
					this._graph.DestroyPlayable<AnimationScriptPlayable>(this._playable);
				}
			}
			if (this._handles.IsCreated)
			{
				this._handles.Dispose();
			}
			if (this._values.IsCreated)
			{
				this._values.Dispose();
			}
			if (this._evaluations.IsCreated)
			{
				this._evaluations.Dispose();
			}
		}

		// Token: 0x04009629 RID: 38441
		private PlayableGraph _graph;

		// Token: 0x0400962A RID: 38442
		private AnimationPlayableOutput _output;

		// Token: 0x0400962B RID: 38443
		private AnimationScriptPlayable _playable;

		// Token: 0x0400962C RID: 38444
		private NativeArray<PropertyStreamHandle> _handles;

		// Token: 0x0400962D RID: 38445
		private NativeArray<float> _values;

		// Token: 0x0400962E RID: 38446
		private NativeArray<int> _evaluations;

		// Token: 0x02001252 RID: 4690
		private struct ReleaseJob : IAnimationJob
		{
			// Token: 0x060091DA RID: 37338 RVA: 0x00004F31 File Offset: 0x00003131
			public void ProcessRootMotion(AnimationStream stream)
			{
			}

			// Token: 0x060091DB RID: 37339 RVA: 0x003BC5AC File Offset: 0x003BA7AC
			public void ProcessAnimation(AnimationStream stream)
			{
				if (this.Evaluations[0] != 0)
				{
					return;
				}
				this.Evaluations[0] = 1;
				for (int i = 0; i < this.Handles.Length; i++)
				{
					PropertyStreamHandle propertyStreamHandle = this.Handles[i];
					if (!propertyStreamHandle.IsValid(stream))
					{
						int num = this.Evaluations[2];
						this.Evaluations[2] = num + 1;
					}
					else
					{
						try
						{
							propertyStreamHandle.SetFloat(stream, this.Values[i]);
							int num = this.Evaluations[1];
							this.Evaluations[1] = num + 1;
						}
						catch (InvalidOperationException)
						{
							int num = this.Evaluations[2];
							this.Evaluations[2] = num + 1;
						}
					}
				}
			}

			// Token: 0x0400962F RID: 38447
			[ReadOnly]
			public NativeArray<PropertyStreamHandle> Handles;

			// Token: 0x04009630 RID: 38448
			[ReadOnly]
			public NativeArray<float> Values;

			// Token: 0x04009631 RID: 38449
			public NativeArray<int> Evaluations;
		}
	}
}
