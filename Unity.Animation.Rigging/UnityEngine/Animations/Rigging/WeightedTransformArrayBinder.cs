using System;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200003F RID: 63
	public class WeightedTransformArrayBinder
	{
		// Token: 0x06000129 RID: 297 RVA: 0x000063E8 File Offset: 0x000045E8
		public static void BindReadOnlyTransforms(Animator animator, Component component, WeightedTransformArray weightedTransformArray, out NativeArray<ReadOnlyTransformHandle> transforms)
		{
			transforms = new NativeArray<ReadOnlyTransformHandle>(weightedTransformArray.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < weightedTransformArray.Count; i++)
			{
				transforms[i] = ReadOnlyTransformHandle.Bind(animator, weightedTransformArray[i].transform);
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006438 File Offset: 0x00004638
		public static void BindReadWriteTransforms(Animator animator, Component component, WeightedTransformArray weightedTransformArray, out NativeArray<ReadWriteTransformHandle> transforms)
		{
			transforms = new NativeArray<ReadWriteTransformHandle>(weightedTransformArray.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < weightedTransformArray.Count; i++)
			{
				transforms[i] = ReadWriteTransformHandle.Bind(animator, weightedTransformArray[i].transform);
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006488 File Offset: 0x00004688
		public static void BindWeights(Animator animator, Component component, WeightedTransformArray weightedTransformArray, string name, out NativeArray<PropertyStreamHandle> weights)
		{
			weights = new NativeArray<PropertyStreamHandle>(weightedTransformArray.Count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < weightedTransformArray.Count; i++)
			{
				weights[i] = animator.BindStreamProperty(component.transform, component.GetType(), name + ".m_Item" + i.ToString() + ".weight");
			}
		}
	}
}
