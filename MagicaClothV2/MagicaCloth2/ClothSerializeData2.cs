using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000020 RID: 32
	[Serializable]
	public class ClothSerializeData2 : IDataValidate, IValid, ITransform
	{
		// Token: 0x0600008E RID: 142 RVA: 0x0000706A File Offset: 0x0000526A
		public bool IsValid()
		{
			return true;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000706D File Offset: 0x0000526D
		public void DataValidate()
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002137 File Offset: 0x00000337
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000706F File Offset: 0x0000526F
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			this.initData.GetUsedTransform(transformSet);
			this.preBuildData.GetUsedTransform(transformSet);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00007089 File Offset: 0x00005289
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			this.initData.ReplaceTransform(replaceDict);
			this.preBuildData.ReplaceTransform(replaceDict);
		}

		// Token: 0x040000E4 RID: 228
		[SerializeField]
		public ClothInitSerializeData initData = new ClothInitSerializeData();

		// Token: 0x040000E5 RID: 229
		[SerializeField]
		public SelectionData selectionData = new SelectionData();

		// Token: 0x040000E6 RID: 230
		[NonSerialized]
		public Dictionary<Transform, VertexAttribute> boneAttributeDict = new Dictionary<Transform, VertexAttribute>();

		// Token: 0x040000E7 RID: 231
		[NonSerialized]
		public List<VertexAttribute[]> vertexAttributeList = new List<VertexAttribute[]>();

		// Token: 0x040000E8 RID: 232
		public PreBuildSerializeData preBuildData = new PreBuildSerializeData();
	}
}
