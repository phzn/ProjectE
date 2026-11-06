using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000446 RID: 1094
	public class vAnimateUV : MonoBehaviour
	{
		// Token: 0x06001B76 RID: 7030 RVA: 0x000ACF70 File Offset: 0x000AB170
		private void Update()
		{
			this.offSet.x = this.offSet.x + this.speed.x * Time.deltaTime;
			this.offSet.y = this.offSet.y + this.speed.y * Time.deltaTime;
			for (int i = 0; i < this.textureParameters.Length; i++)
			{
				this._renderer.material.SetTextureOffset(this.textureParameters[i], this.offSet);
			}
		}

		// Token: 0x04001CA4 RID: 7332
		public Vector2 speed;

		// Token: 0x04001CA5 RID: 7333
		public Renderer _renderer;

		// Token: 0x04001CA6 RID: 7334
		public string[] textureParameters = new string[] { "_MainTex" };

		// Token: 0x04001CA7 RID: 7335
		private Vector2 offSet;
	}
}
