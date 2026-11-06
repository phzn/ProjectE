using System;
using System.IO;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000096 RID: 150
	[AddComponentMenu("More Mountains/Tools/Utilities/MMScreenshot")]
	public class MMScreenshot : MonoBehaviour
	{
		// Token: 0x06000339 RID: 825 RVA: 0x000103C3 File Offset: 0x0000E5C3
		protected virtual void LateUpdate()
		{
			this.DetectInput();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000103CB File Offset: 0x0000E5CB
		protected virtual void DetectInput()
		{
			if (Input.GetKeyDown(this.ScreenshotShortcut))
			{
				this.TakeScreenshot();
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000103E0 File Offset: 0x0000E5E0
		protected virtual void TakeScreenshot()
		{
			if (!Directory.Exists(this.FolderName))
			{
				Directory.CreateDirectory(this.FolderName);
			}
			string text = "";
			MMScreenshot.Methods method = this.Method;
			if (method != MMScreenshot.Methods.ScreenCapture)
			{
				if (method == MMScreenshot.Methods.RenderTexture)
				{
					text = this.TakeRenderTextureScreenshot();
				}
			}
			else
			{
				text = this.TakeScreenCaptureScreenshot();
			}
			MMDebug.DebugLogInfo("[MMScreenshot] Screenshot taken and saved at " + text, "", 3, true);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00010444 File Offset: 0x0000E644
		protected virtual string TakeScreenCaptureScreenshot()
		{
			float num = (float)(Screen.width * this.GameViewSizeMultiplier);
			float num2 = (float)(Screen.height * this.GameViewSizeMultiplier);
			string text = string.Concat(new string[]
			{
				this.FolderName,
				"/screenshot_",
				num.ToString(),
				"x",
				num2.ToString(),
				"_",
				DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
				".png"
			});
			ScreenCapture.CaptureScreenshot(text, this.GameViewSizeMultiplier);
			return text;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000104D8 File Offset: 0x0000E6D8
		protected virtual string TakeRenderTextureScreenshot()
		{
			string text = string.Concat(new string[]
			{
				this.FolderName,
				"/screenshot_",
				this.ResolutionWidth.ToString(),
				"x",
				this.ResolutionHeight.ToString(),
				"_",
				DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
				".png"
			});
			RenderTexture renderTexture = new RenderTexture(this.ResolutionWidth, this.ResolutionHeight, 24);
			this.TargetCamera.targetTexture = renderTexture;
			Texture2D texture2D = new Texture2D(this.ResolutionWidth, this.ResolutionHeight, TextureFormat.RGB24, false);
			this.TargetCamera.Render();
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)this.ResolutionWidth, (float)this.ResolutionHeight), 0, 0);
			this.TargetCamera.targetTexture = null;
			RenderTexture.active = null;
			Object.Destroy(renderTexture);
			byte[] array = texture2D.EncodeToPNG();
			File.WriteAllBytes(text, array);
			return text;
		}

		// Token: 0x04000465 RID: 1125
		public string FolderName = "Screenshots";

		// Token: 0x04000466 RID: 1126
		[Header("Screenshot")]
		public MMScreenshot.Methods Method;

		// Token: 0x04000467 RID: 1127
		public KeyCode ScreenshotShortcut = KeyCode.K;

		// Token: 0x04000468 RID: 1128
		[MMEnumCondition("Method", new int[] { 0 })]
		public int GameViewSizeMultiplier = 3;

		// Token: 0x04000469 RID: 1129
		[MMEnumCondition("Method", new int[] { 1 })]
		public Camera TargetCamera;

		// Token: 0x0400046A RID: 1130
		[MMEnumCondition("Method", new int[] { 1 })]
		public int ResolutionWidth;

		// Token: 0x0400046B RID: 1131
		[MMEnumCondition("Method", new int[] { 1 })]
		public int ResolutionHeight;

		// Token: 0x0400046C RID: 1132
		[Header("Controls")]
		[MMInspectorButton("TakeScreenshot")]
		public bool TakeScreenshotButton;

		// Token: 0x020003F1 RID: 1009
		public enum Methods
		{
			// Token: 0x04001E93 RID: 7827
			ScreenCapture,
			// Token: 0x04001E94 RID: 7828
			RenderTexture
		}
	}
}
