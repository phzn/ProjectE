using System;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200003B RID: 59
	public class TwistCorrectionJobBinder<T> : AnimationJobBinder<TwistCorrectionJob, T> where T : struct, IAnimationJobData, ITwistCorrectionData
	{
		// Token: 0x06000115 RID: 277 RVA: 0x00006074 File Offset: 0x00004274
		public override TwistCorrectionJob Create(Animator animator, ref T data, Component component)
		{
			TwistCorrectionJob twistCorrectionJob = default(TwistCorrectionJob);
			twistCorrectionJob.source = ReadOnlyTransformHandle.Bind(animator, data.source);
			twistCorrectionJob.sourceInverseBindRotation = Quaternion.Inverse(data.source.localRotation);
			twistCorrectionJob.axisMask = data.twistAxis;
			WeightedTransformArray twistNodes = data.twistNodes;
			WeightedTransformArrayBinder.BindReadWriteTransforms(animator, component, twistNodes, out twistCorrectionJob.twistTransforms);
			WeightedTransformArrayBinder.BindWeights(animator, component, twistNodes, data.twistNodesProperty, out twistCorrectionJob.twistWeights);
			twistCorrectionJob.weightBuffer = new NativeArray<float>(twistNodes.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			twistCorrectionJob.twistBindRotations = new NativeArray<Quaternion>(twistNodes.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < twistNodes.Count; i++)
			{
				Transform transform = twistNodes[i].transform;
				twistCorrectionJob.twistBindRotations[i] = transform.localRotation;
			}
			return twistCorrectionJob;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00006167 File Offset: 0x00004367
		public override void Destroy(TwistCorrectionJob job)
		{
			job.twistTransforms.Dispose();
			job.twistWeights.Dispose();
			job.twistBindRotations.Dispose();
			job.weightBuffer.Dispose();
		}
	}
}
