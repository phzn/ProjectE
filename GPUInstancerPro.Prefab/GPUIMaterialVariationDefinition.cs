using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000007 RID: 7
	[CreateAssetMenu(menuName = "Rendering/GPU Instancer Pro/Material Variation Definition", order = 811)]
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:BestPractices#Prefab_Manager_Material_Variations")]
	public class GPUIMaterialVariationDefinition : ScriptableObject
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002694 File Offset: 0x00000894
		public void AddVariation(int renderKey, int bufferIndex, Vector4 value)
		{
			if (this._hashDict == null)
			{
				this._hashDict = new Dictionary<int, int>();
			}
			int num;
			if (!this._hashDict.TryGetValue(renderKey, out num))
			{
				num = GPUIUtility.GenerateHash(new int[]
				{
					this.GetInstanceIDGPUI(),
					renderKey
				});
				this._hashDict.Add(renderKey, num);
			}
			GPUIMaterialVariationDataProvider.GetMaterialVariationDataWithHash(this, num).AddVariation(renderKey, bufferIndex, value);
		}

		// Token: 0x0400000F RID: 15
		[SerializeField]
		public Material material;

		// Token: 0x04000010 RID: 16
		[SerializeField]
		public Shader replacementShader;

		// Token: 0x04000011 RID: 17
		[SerializeField]
		public string bufferName = "variationBuffer";

		// Token: 0x04000012 RID: 18
		[SerializeField]
		public GPUIMVDefinitionItem[] items;

		// Token: 0x04000013 RID: 19
		[SerializeField]
		public bool isCrowdAnimations;

		// Token: 0x04000014 RID: 20
		[NonSerialized]
		private Dictionary<int, int> _hashDict;
	}
}
