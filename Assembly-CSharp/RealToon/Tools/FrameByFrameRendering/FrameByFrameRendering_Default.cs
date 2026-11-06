using System;
using System.IO;
using UnityEngine;

namespace RealToon.Tools.FrameByFrameRendering
{
	// Token: 0x02000436 RID: 1078
	[AddComponentMenu("RealToon/Tools/Frame By Frame Rendering/Frame By Frame Rendering (Default)")]
	public class FrameByFrameRendering_Default : MonoBehaviour
	{
		// Token: 0x06001B2A RID: 6954 RVA: 0x000A985C File Offset: 0x000A7A5C
		private void Start()
		{
			if (this.StartFrameCont <= -1)
			{
				this.StartFrameCheck = false;
				this.StartRendering = false;
				this.info = "Rendering has not started because 'Start Frame' value is less than 0.";
				Debug.LogError(this.info);
			}
			if (this.EndFrameCont <= 0)
			{
				this.StartFrameCheck = false;
				this.StartRendering = false;
				this.info = "Rendering has not started because 'End Frame' value is 0 or less than 0.";
				Debug.LogError(this.info);
			}
			if (this.FrameRateCont <= 0)
			{
				this.StartFrameCheck = false;
				this.StartRendering = false;
				this.info = "Rendering has not started because 'Frame Rate' value is 0 or less than 0.";
				Debug.LogError(this.info);
			}
			if (this.PNGFileName == string.Empty)
			{
				this.PNGFileName = "Frame";
				this.info = "File Name set to 'Frame' because the field is not set or empty.";
				Debug.LogError(this.info);
			}
			if (this.PathFolder == string.Empty)
			{
				this.PathFolder = "Rendered Files";
				this.info = "Folder Path set to 'Rendered Files' and will be created to your UNITY ROOT PROJECT FOLDER because the field is not set or empty.";
				Debug.LogError(this.info);
			}
			Time.captureFramerate = this.FrameRate;
			this.StartFrameCont = this.StartFrame;
			this.EndFrameCont = this.EndFrame;
			this.FrameRateCont = this.FrameRate;
			this.SingleFrameRenderingCont = this.SingleFrameRenderingMode;
			this.PathFolderCont = this.PathFolder;
			this.PNGFileNameCont = this.PNGFileName;
			this.DirInfo = new DirectoryInfo(this.PathFolder);
			if (!Directory.Exists(this.PathFolder))
			{
				Directory.CreateDirectory(this.PathFolder);
				this.info = "Folder '" + this.PathFolder + "' Has Been Created To Your Root Project Folder.";
				Debug.LogWarning(this.info);
			}
			if (this.SingleFrameRenderingMode)
			{
				this.StartFrameCheck = true;
				this.StartRendering = true;
				this.info = "Picture or Single Frame Rendering Mode";
				Debug.LogWarning(this.info);
				this.EndFrameCont = 1;
				return;
			}
			this.info = "Video/Animation Mode";
			Debug.LogWarning(this.info);
			if (this.DirInfo.GetFiles().Length != 0)
			{
				this.StartFrameCheck = false;
				this.StartRendering = false;
				this.info = "(Video/Animation Mode) Rendering not started because there are already rendered frames or files in this folder ('" + this.PathFolder + "'), Please empty this folder or make another folder by changing the Path Folder.";
				Debug.LogError(this.info);
				return;
			}
			this.StartFrameCheck = true;
			this.StartRendering = true;
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x000A9A94 File Offset: 0x000A7C94
		private void Update()
		{
			this.CurrentFrame = Time.frameCount - 1;
			this.StartFrame = this.StartFrameCont;
			this.EndFrame = this.EndFrameCont;
			this.SingleFrameRenderingMode = this.SingleFrameRenderingCont;
			this.PathFolder = this.PathFolderCont;
			this.PNGFileName = this.PNGFileNameCont;
			if (this.PathFolder == string.Empty)
			{
				this.PathFolder = "Rendered Files";
				return;
			}
			if (this.StartFrameCheck)
			{
				if (this.CurrentFrame == this.StartFrameCont)
				{
					this.info = "Rendering Has Started.";
					Debug.LogWarning(this.info);
					this.StartRendering = true;
				}
				if (this.StartRendering)
				{
					if (!this.SingleFrameRenderingMode)
					{
						string text = string.Format("{0}/" + this.PNGFileNameCont + " {1:D04}.png", this.PathFolderCont, this.CurrentFrame);
						this.CurrentRenderedFile = text;
						this.info = text;
						ScreenCapture.CaptureScreenshot(text);
					}
					else
					{
						string text2 = string.Format(string.Concat(new string[]
						{
							"{0}/",
							this.PNGFileNameCont,
							" ",
							DateTime.Now.ToString("hh_mm_ss"),
							".png"
						}), this.PathFolderCont, this.CurrentFrame);
						this.CurrentRenderedFile = text2;
						this.info = text2;
						ScreenCapture.CaptureScreenshot(text2);
					}
					if (this.CurrentFrame == this.EndFrame)
					{
						this.info = "Rendering Has Ended. [Last Rendered File: " + this.CurrentRenderedFile + "]";
						Debug.LogWarning(this.info);
						this.StartRendering = false;
						this.StartFrameCheck = false;
					}
				}
			}
		}

		// Token: 0x04001C0A RID: 7178
		[Header("(Frame By Frame Rendering V1.0.0)")]
		[Header("Click 'Play' button to start render.")]
		[Space(20f)]
		[Header("==============================")]
		[Header("(Settings)")]
		[Space(5f)]
		[SerializeField]
		[Tooltip("Example Path: C:/TheNameOfTheFolder (Default folder name is Rendered Files and it will be created to your unity root project folder if this set to empty.")]
		public string PathFolder = "Rendered Files";

		// Token: 0x04001C0B RID: 7179
		[SerializeField]
		[Tooltip("PNG File Name")]
		public string PNGFileName = "Frame";

		// Token: 0x04001C0C RID: 7180
		[Space(15f)]
		[SerializeField]
		[Tooltip("Frame Rate (Set this to 24 if you want Film Style framerate.")]
		public int FrameRate = 24;

		// Token: 0x04001C0D RID: 7181
		[SerializeField]
		[Tooltip("Start frame to start rendering.")]
		public int StartFrame;

		// Token: 0x04001C0E RID: 7182
		[SerializeField]
		[Tooltip("End frame to end rendering.")]
		public int EndFrame = 100;

		// Token: 0x04001C0F RID: 7183
		[Space(15f)]
		[SerializeField]
		[Tooltip("Render single frame or single image only, For Illustration or Art use.")]
		public bool SingleFrameRenderingMode;

		// Token: 0x04001C10 RID: 7184
		[Header("==============================")]
		[Header("(Information [Display Only] )")]
		[Space(5f)]
		[Tooltip("Display the current frame of your scene/play. (Display Only)")]
		public int CurrentFrame;

		// Token: 0x04001C11 RID: 7185
		[Tooltip("Display the information of the operation or rendering. (Display Only)")]
		public string info = string.Empty;

		// Token: 0x04001C12 RID: 7186
		private bool StartRendering;

		// Token: 0x04001C13 RID: 7187
		private bool StartFrameCheck;

		// Token: 0x04001C14 RID: 7188
		private string CurrentRenderedFile = string.Empty;

		// Token: 0x04001C15 RID: 7189
		private int StartFrameCont;

		// Token: 0x04001C16 RID: 7190
		private int EndFrameCont = 100;

		// Token: 0x04001C17 RID: 7191
		private int FrameRateCont = 24;

		// Token: 0x04001C18 RID: 7192
		private string PathFolderCont = "Rendered Files";

		// Token: 0x04001C19 RID: 7193
		private string PNGFileNameCont = "Frame";

		// Token: 0x04001C1A RID: 7194
		private bool SingleFrameRenderingCont;

		// Token: 0x04001C1B RID: 7195
		private DirectoryInfo DirInfo;
	}
}
