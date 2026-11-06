using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003ED RID: 1005
	[Serializable]
	public class VectorImage : ScriptableObject
	{
		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x060020BE RID: 8382 RVA: 0x0007C13D File Offset: 0x0007A33D
		public float width
		{
			get
			{
				return this.size.x;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x060020BF RID: 8383 RVA: 0x0007C14A File Offset: 0x0007A34A
		public float height
		{
			get
			{
				return this.size.y;
			}
		}

		// Token: 0x04000D9F RID: 3487
		[SerializeField]
		internal int version = 0;

		// Token: 0x04000DA0 RID: 3488
		[SerializeField]
		internal Texture2D atlas = null;

		// Token: 0x04000DA1 RID: 3489
		[SerializeField]
		internal VectorImageVertex[] vertices = null;

		// Token: 0x04000DA2 RID: 3490
		[SerializeField]
		internal ushort[] indices = null;

		// Token: 0x04000DA3 RID: 3491
		[SerializeField]
		internal GradientSettings[] settings = null;

		// Token: 0x04000DA4 RID: 3492
		[SerializeField]
		internal Vector2 size = Vector2.zero;
	}
}
