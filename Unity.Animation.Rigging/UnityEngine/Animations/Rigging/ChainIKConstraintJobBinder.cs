using System;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200000D RID: 13
	public class ChainIKConstraintJobBinder<T> : AnimationJobBinder<ChainIKConstraintJob, T> where T : struct, IAnimationJobData, IChainIKConstraintData
	{
		// Token: 0x06000033 RID: 51 RVA: 0x000029B8 File Offset: 0x00000BB8
		public override ChainIKConstraintJob Create(Animator animator, ref T data, Component component)
		{
			Transform[] array = ConstraintsUtils.ExtractChain(data.root, data.tip);
			ChainIKConstraintJob chainIKConstraintJob = default(ChainIKConstraintJob);
			chainIKConstraintJob.chain = new NativeArray<ReadWriteTransformHandle>(array.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			chainIKConstraintJob.linkLengths = new NativeArray<float>(array.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			chainIKConstraintJob.linkPositions = new NativeArray<Vector3>(array.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			chainIKConstraintJob.maxReach = 0f;
			int num = array.Length - 1;
			for (int i = 0; i < array.Length; i++)
			{
				chainIKConstraintJob.chain[i] = ReadWriteTransformHandle.Bind(animator, array[i]);
				chainIKConstraintJob.linkLengths[i] = ((i != num) ? Vector3.Distance(array[i].position, array[i + 1].position) : 0f);
				chainIKConstraintJob.maxReach += chainIKConstraintJob.linkLengths[i];
			}
			chainIKConstraintJob.target = ReadOnlyTransformHandle.Bind(animator, data.target);
			chainIKConstraintJob.targetOffset = AffineTransform.identity;
			if (data.maintainTargetPositionOffset)
			{
				chainIKConstraintJob.targetOffset.translation = data.tip.position - data.target.position;
			}
			if (data.maintainTargetRotationOffset)
			{
				chainIKConstraintJob.targetOffset.rotation = Quaternion.Inverse(data.target.rotation) * data.tip.rotation;
			}
			chainIKConstraintJob.chainRotationWeight = FloatProperty.Bind(animator, component, data.chainRotationWeightFloatProperty);
			chainIKConstraintJob.tipRotationWeight = FloatProperty.Bind(animator, component, data.tipRotationWeightFloatProperty);
			AnimationJobCacheBuilder animationJobCacheBuilder = new AnimationJobCacheBuilder();
			chainIKConstraintJob.maxIterationsIdx = animationJobCacheBuilder.Add((float)data.maxIterations);
			chainIKConstraintJob.toleranceIdx = animationJobCacheBuilder.Add(data.tolerance);
			chainIKConstraintJob.cache = animationJobCacheBuilder.Build();
			return chainIKConstraintJob;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002BCD File Offset: 0x00000DCD
		public override void Destroy(ChainIKConstraintJob job)
		{
			job.chain.Dispose();
			job.linkLengths.Dispose();
			job.linkPositions.Dispose();
			job.cache.Dispose();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002C00 File Offset: 0x00000E00
		public override void Update(ChainIKConstraintJob job, ref T data)
		{
			job.cache.SetRaw((float)data.maxIterations, job.maxIterationsIdx, 0);
			job.cache.SetRaw(data.tolerance, job.toleranceIdx, 0);
		}
	}
}
