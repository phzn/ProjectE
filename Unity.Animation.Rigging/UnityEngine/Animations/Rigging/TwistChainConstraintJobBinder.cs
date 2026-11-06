using System;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000038 RID: 56
	public class TwistChainConstraintJobBinder<T> : AnimationJobBinder<TwistChainConstraintJob, T> where T : struct, IAnimationJobData, ITwistChainConstraintData
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00005D24 File Offset: 0x00003F24
		public override TwistChainConstraintJob Create(Animator animator, ref T data, Component component)
		{
			Transform[] array = ConstraintsUtils.ExtractChain(data.root, data.tip);
			float[] array2 = ConstraintsUtils.ExtractSteps(array);
			TwistChainConstraintJob twistChainConstraintJob = default(TwistChainConstraintJob);
			twistChainConstraintJob.chain = new NativeArray<ReadWriteTransformHandle>(array.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			twistChainConstraintJob.steps = new NativeArray<float>(array.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			twistChainConstraintJob.weights = new NativeArray<float>(array.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			twistChainConstraintJob.rotations = new NativeArray<Quaternion>(array.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			twistChainConstraintJob.rootTarget = ReadWriteTransformHandle.Bind(animator, data.rootTarget);
			twistChainConstraintJob.tipTarget = ReadWriteTransformHandle.Bind(animator, data.tipTarget);
			for (int i = 0; i < array.Length; i++)
			{
				twistChainConstraintJob.chain[i] = ReadWriteTransformHandle.Bind(animator, array[i]);
				twistChainConstraintJob.steps[i] = array2[i];
				twistChainConstraintJob.weights[i] = Mathf.Clamp01(data.curve.Evaluate(array2[i]));
			}
			twistChainConstraintJob.rotations[0] = Quaternion.identity;
			twistChainConstraintJob.rotations[array.Length - 1] = Quaternion.identity;
			for (int j = 1; j < array.Length - 1; j++)
			{
				twistChainConstraintJob.rotations[j] = Quaternion.Inverse(Quaternion.Lerp(array[0].rotation, array[array.Length - 1].rotation, twistChainConstraintJob.weights[j])) * array[j].rotation;
			}
			return twistChainConstraintJob;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005EB3 File Offset: 0x000040B3
		public override void Destroy(TwistChainConstraintJob job)
		{
			job.chain.Dispose();
			job.weights.Dispose();
			job.steps.Dispose();
			job.rotations.Dispose();
		}
	}
}
