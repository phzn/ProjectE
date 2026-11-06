using System;
using System.Collections.Generic;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000076 RID: 118
	public static class ConstraintsUtils
	{
		// Token: 0x060002FF RID: 767 RVA: 0x0000A0C4 File Offset: 0x000082C4
		public static Transform[] ExtractChain(Transform root, Transform tip)
		{
			if (!tip.IsChildOf(root))
			{
				return new Transform[0];
			}
			List<Transform> list = new List<Transform>();
			Transform transform = tip;
			while (transform != root)
			{
				list.Add(transform);
				transform = transform.parent;
			}
			list.Add(root);
			list.Reverse();
			return list.ToArray();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000A118 File Offset: 0x00008318
		public static float[] ExtractLengths(Transform[] chain)
		{
			float[] array = new float[chain.Length];
			array[0] = 0f;
			for (int i = 1; i < chain.Length; i++)
			{
				array[i] = chain[i].localPosition.magnitude;
			}
			return array;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000A158 File Offset: 0x00008358
		public static float[] ExtractSteps(Transform[] chain)
		{
			float[] array = ConstraintsUtils.ExtractLengths(chain);
			float totalLength = 0f;
			Array.ForEach<float>(array, delegate(float length)
			{
				totalLength += length;
			});
			float[] array2 = new float[array.Length];
			float num = 0f;
			for (int i = 0; i < array.Length; i++)
			{
				num += array[i];
				float num2 = num / totalLength;
				array2[i] = num2;
			}
			return array2;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000A1C8 File Offset: 0x000083C8
		public static string ConstructConstraintDataPropertyName(string property)
		{
			return "m_Data." + property;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000A1D8 File Offset: 0x000083D8
		public static string ConstructCustomPropertyName(Component component, string property)
		{
			string[] array = new string[5];
			array[0] = component.transform.GetInstanceID().ToString();
			array[1] = "/";
			int num = 2;
			Type type = component.GetType();
			array[num] = ((type != null) ? type.ToString() : null);
			array[3] = "/";
			array[4] = property;
			return string.Concat(array);
		}
	}
}
