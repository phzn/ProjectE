using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000092 RID: 146
	[Serializable]
	public class MMLayer
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600032E RID: 814 RVA: 0x000102E9 File Offset: 0x0000E4E9
		public virtual int LayerIndex
		{
			get
			{
				return this._layerIndex;
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000102F1 File Offset: 0x0000E4F1
		public virtual void Set(int _layerIndex)
		{
			if (_layerIndex > 0 && _layerIndex < 32)
			{
				this._layerIndex = _layerIndex;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00010303 File Offset: 0x0000E503
		public virtual int Mask
		{
			get
			{
				return 1 << this._layerIndex;
			}
		}

		// Token: 0x0400045E RID: 1118
		[SerializeField]
		protected int _layerIndex;
	}
}
