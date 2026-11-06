using System;
using System.Collections.Concurrent;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000353 RID: 851
	[Serializable]
	public class SpriteProcessor : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001679 RID: 5753 RVA: 0x000937C2 File Offset: 0x000919C2
		public Sprite GetSprite(FObject fobject)
		{
			fobject.Data.SpritePath.IsEmpty();
			return null;
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00004F31 File Offset: 0x00003131
		public void SaveSprite(Texture2D texture, FObject fobject)
		{
		}

		// Token: 0x0400182E RID: 6190
		private int _errorLogSplitLimit = 50;

		// Token: 0x0400182F RID: 6191
		private ConcurrentBag<FObject> failedObjects = new ConcurrentBag<FObject>();
	}
}
