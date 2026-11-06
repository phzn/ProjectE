using System;
using System.Collections;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000852 RID: 2130
	public class IconCapture : MonoBehaviour
	{
		// Token: 0x06003875 RID: 14453 RVA: 0x00149B87 File Offset: 0x00147D87
		[ContextMenu("Capture")]
		public void Capture()
		{
			base.StartCoroutine(this.CaptureCoroutine());
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x00149B96 File Offset: 0x00147D96
		private IEnumerator CaptureCoroutine()
		{
			if (this.outfits.Length == 0)
			{
				yield return new WaitForEndOfFrame();
				if (this.iconCamera != null)
				{
					this.iconCamera.targetTexture = this.iconTexture;
					this.iconCamera.Render();
				}
				RenderTexture.active = this.iconTexture;
				Texture2D texture2D = new Texture2D(this.iconTexture.width, this.iconTexture.height, TextureFormat.RGBA32, false);
				Rect rect = new Rect(new Rect(0f, 0f, (float)this.iconTexture.width, (float)this.iconTexture.height));
				texture2D.ReadPixels(rect, 0, 0);
				texture2D.Apply();
				texture2D.EncodeToPNG();
				yield return null;
			}
			Outfit[] array = this.outfits;
			for (int i = 0; i < array.Length; i++)
			{
				Outfit outfit = Object.Instantiate<Outfit>(array[i], this.OutfitSystem);
				for (int j = 0; j < this.Colors.Length; j++)
				{
					outfit.SetColor(this.Colors[j], j + 1, false);
				}
				yield return new WaitForEndOfFrame();
				if (this.iconCamera != null)
				{
					this.iconCamera.targetTexture = this.iconTexture;
					this.iconCamera.Render();
				}
				RenderTexture.active = this.iconTexture;
				Texture2D texture2D2 = new Texture2D(this.iconTexture.width, this.iconTexture.height, TextureFormat.RGBA32, false);
				Rect rect2 = new Rect(new Rect(0f, 0f, (float)this.iconTexture.width, (float)this.iconTexture.height));
				texture2D2.ReadPixels(rect2, 0, 0);
				texture2D2.Apply();
				texture2D2.EncodeToPNG();
			}
			array = null;
			yield break;
		}

		// Token: 0x0400395B RID: 14683
		[SerializeField]
		private RenderTexture iconTexture;

		// Token: 0x0400395C RID: 14684
		[SerializeField]
		private Camera iconCamera;

		// Token: 0x0400395D RID: 14685
		[SerializeField]
		private string path;

		// Token: 0x0400395E RID: 14686
		[SerializeField]
		private Transform OutfitSystem;

		// Token: 0x0400395F RID: 14687
		[SerializeField]
		private Outfit[] outfits;

		// Token: 0x04003960 RID: 14688
		[SerializeField]
		private Color[] Colors;
	}
}
