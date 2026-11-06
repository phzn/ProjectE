using System;
using System.IO;
using UnityEngine;

namespace RealToon.Tools.FrameByFrameRendering
{
	// Token: 0x02000437 RID: 1079
	[AddComponentMenu("RealToon/Tools/Frame By Frame Rendering/Frame By Frame Rendering (Manual)")]
	public class FrameByFrameRendering_Manual : MonoBehaviour
	{
		// Token: 0x06001B2D RID: 6957 RVA: 0x000A9CBC File Offset: 0x000A7EBC
		private void Start()
		{
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
			this.PictureModeCont = this.PictureMode;
			this.PathFolderCont = this.PathFolder;
			this.PNGFileNameCont = this.PNGFileName;
			this.FrameNumberCont = this.FrameNumber;
			this.DirInfo = new DirectoryInfo(this.PathFolder);
			if (!Directory.Exists(this.PathFolder))
			{
				Directory.CreateDirectory(this.PathFolder);
				this.info = "Folder '" + this.PathFolder + "' Has Been Created To Your Root Project Folder.";
				Debug.LogWarning(this.info);
			}
			if (!this.PictureMode)
			{
				this.info = "Frame by Frame Rendering Mode";
				Debug.LogWarning(this.info);
				if (this.DirInfo.GetFiles().Length != 0)
				{
					this.PreventRender = true;
					this.Render = false;
					this.info = "(Frame by Frame Mode) Rendering not started because there are already rendered frames or files in this folder ('" + this.PathFolder + "'), Please empty this folder or make another folder by changing the Path Folder.";
					Debug.LogError(this.info);
					return;
				}
			}
			else
			{
				this.info = "Picture or Single Frame Rendering Mode";
				Debug.LogWarning(this.info);
			}
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x000A9E28 File Offset: 0x000A8028
		private void Update()
		{
			this.PictureMode = this.PictureModeCont;
			this.PathFolder = this.PathFolderCont;
			this.PNGFileName = this.PNGFileNameCont;
			this.LastRenderedFrame = this.FrameNumberCont;
			if (this.FrameNumber <= 0)
			{
				this.FrameNumber = this.FrameNumberCont;
			}
			if (!this.PreventRender && this.Render)
			{
				if (!this.PictureMode)
				{
					string text = string.Format("{0}/" + this.PNGFileNameCont + " {1:D04}.png", this.PathFolderCont, this.FrameNumber);
					this.CurrentRenderedFile = text;
					ScreenCapture.CaptureScreenshot(text);
					this.FrameNumber++;
					this.FrameNumberCont = this.FrameNumber;
					this.info = this.CurrentRenderedFile;
					Debug.LogWarning(this.info);
					this.Render = false;
					return;
				}
				string text2 = string.Format(string.Concat(new string[]
				{
					"{0}/",
					this.PNGFileNameCont,
					" ",
					DateTime.Now.ToString("hh_mm_ss"),
					".png"
				}), this.PathFolderCont, this.FrameNumber);
				this.CurrentRenderedFile = text2;
				ScreenCapture.CaptureScreenshot(text2);
				this.info = this.CurrentRenderedFile;
				Debug.LogWarning(this.info);
				this.Render = false;
			}
		}

		// Token: 0x04001C1C RID: 7196
		[Header("(Frame By Frame Rendering V1.0.0)")]
		[Header("Click 'Play' & 'Render' button to start render one by one.")]
		[Space(20f)]
		[SerializeField]
		[Tooltip("Current Frame Number")]
		public int FrameNumber;

		// Token: 0x04001C1D RID: 7197
		[Space(5f)]
		[SerializeField]
		[Tooltip("Start Render")]
		public bool Render;

		// Token: 0x04001C1E RID: 7198
		[Header("==============================")]
		[Header("(Settings)")]
		[Space(5f)]
		[SerializeField]
		[Tooltip("Example Path: C:/TheNameOfTheFolder (Default folder name is Rendered Files and it will be created to your unity root project folder if this set to empty.")]
		public string PathFolder = "Rendered Files";

		// Token: 0x04001C1F RID: 7199
		[SerializeField]
		[Tooltip("PNG File Name")]
		public string PNGFileName = "Frame";

		// Token: 0x04001C20 RID: 7200
		[Space(15f)]
		[SerializeField]
		[Tooltip("Render single frame or single image only, For Illustration or Art use.")]
		public bool PictureMode;

		// Token: 0x04001C21 RID: 7201
		[Header("==============================")]
		[Header("(Information [Display Only] )")]
		[Space(5f)]
		[Tooltip("Display the information of the operation or rendering. (Display Only)")]
		public int LastRenderedFrame;

		// Token: 0x04001C22 RID: 7202
		public string info = string.Empty;

		// Token: 0x04001C23 RID: 7203
		private string CurrentRenderedFile = string.Empty;

		// Token: 0x04001C24 RID: 7204
		private string PathFolderCont = "Rendered Files";

		// Token: 0x04001C25 RID: 7205
		private string PNGFileNameCont = "Frame";

		// Token: 0x04001C26 RID: 7206
		private int FrameNumberCont;

		// Token: 0x04001C27 RID: 7207
		private bool PictureModeCont;

		// Token: 0x04001C28 RID: 7208
		private bool PreventRender;

		// Token: 0x04001C29 RID: 7209
		private DirectoryInfo DirInfo;
	}
}
