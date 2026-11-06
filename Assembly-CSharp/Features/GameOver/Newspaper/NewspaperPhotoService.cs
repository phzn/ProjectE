using System;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;

namespace Features.GameOver.Newspaper
{
	// Token: 0x020011D9 RID: 4569
	[DisallowMultipleComponent]
	public class NewspaperPhotoService : MonoBehaviour
	{
		// Token: 0x1700166F RID: 5743
		// (get) Token: 0x06008C72 RID: 35954 RVA: 0x00395A56 File Offset: 0x00393C56
		// (set) Token: 0x06008C73 RID: 35955 RVA: 0x00395A5D File Offset: 0x00393C5D
		public static NewspaperPhotoService Instance { get; private set; }

		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x06008C74 RID: 35956 RVA: 0x00395A65 File Offset: 0x00393C65
		public RenderTexture Photo
		{
			get
			{
				if (!this._hasPhoto)
				{
					return null;
				}
				return this.photoTarget;
			}
		}

		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x06008C75 RID: 35957 RVA: 0x00395A77 File Offset: 0x00393C77
		public bool HasPhoto
		{
			get
			{
				return this._hasPhoto;
			}
		}

		// Token: 0x06008C76 RID: 35958 RVA: 0x00395A7F File Offset: 0x00393C7F
		private void Awake()
		{
			if (NewspaperPhotoService.Instance != null && NewspaperPhotoService.Instance != this)
			{
				Object.Destroy(this);
				return;
			}
			NewspaperPhotoService.Instance = this;
		}

		// Token: 0x06008C77 RID: 35959 RVA: 0x00395AA8 File Offset: 0x00393CA8
		private void OnDestroy()
		{
			if (this._allocatedTarget && this.photoTarget != null)
			{
				this.photoTarget.Release();
				Object.Destroy(this.photoTarget);
				this.photoTarget = null;
			}
			if (NewspaperPhotoService.Instance == this)
			{
				NewspaperPhotoService.Instance = null;
			}
		}

		// Token: 0x06008C78 RID: 35960 RVA: 0x00395AFC File Offset: 0x00393CFC
		public void CapturePhotoFromMainCamera()
		{
			this._hasPhoto = false;
			Camera main = Camera.main;
			if (main == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[NewspaperPhotoService] Camera.main is null, capture skipped.");
				}
				return;
			}
			this.EnsureTarget();
			RenderTexture active = RenderTexture.active;
			RenderTexture targetTexture = main.targetTexture;
			int cullingMask = main.cullingMask;
			try
			{
				if (this.excludeUiLayerOnCapture)
				{
					int num = LayerMask.NameToLayer("UI");
					if (num >= 0)
					{
						main.cullingMask &= ~(1 << num);
					}
				}
				main.targetTexture = this.photoTarget;
				StillPhotoRenderUtility.Render(main, this.photoTarget);
				this._hasPhoto = true;
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[NewspaperPhotoService] Photo captured ({0}x{1}).", this.photoTarget.width, this.photoTarget.height));
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[NewspaperPhotoService] Capture failed: " + ex.Message);
			}
			finally
			{
				main.targetTexture = targetTexture;
				main.cullingMask = cullingMask;
				RenderTexture.active = active;
			}
		}

		// Token: 0x06008C79 RID: 35961 RVA: 0x00395C20 File Offset: 0x00393E20
		private void EnsureTarget()
		{
			if (this.photoTarget != null)
			{
				if (!this.photoTarget.IsCreated())
				{
					this.photoTarget.Create();
				}
				return;
			}
			int num = Mathf.Max(64, this.captureSize);
			this.photoTarget = new RenderTexture(num, num, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
			{
				name = "NewspaperPhoto (runtime)"
			};
			this.photoTarget.Create();
			this._allocatedTarget = true;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[NewspaperPhotoService] Allocated runtime RenderTexture {0}x{1}.", num, num));
			}
		}

		// Token: 0x04008F6E RID: 36718
		[Header("Render Texture")]
		[Tooltip("RenderTexture di destinazione della foto. Se null, ne viene allocata una a runtime (captureSize x captureSize). Assegnare l'asset NewspaperPhoto.renderTexture per referenziarlo staticamente dal RawImage del giornale.")]
		[SerializeField]
		private RenderTexture photoTarget;

		// Token: 0x04008F6F RID: 36719
		[Tooltip("Lato (px) della RenderTexture allocata a runtime quando photoTarget e' null.")]
		[SerializeField]
		private int captureSize = 1920;

		// Token: 0x04008F70 RID: 36720
		[Header("Capture")]
		[Tooltip("Se true, esclude il layer 'UI' dal culling durante la cattura. Utile solo se esiste UI in Screen Space - Camera su Camera.main (la Overlay e' gia' esclusa dal render su RT).")]
		[SerializeField]
		private bool excludeUiLayerOnCapture = true;

		// Token: 0x04008F71 RID: 36721
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008F72 RID: 36722
		private bool _hasPhoto;

		// Token: 0x04008F73 RID: 36723
		private bool _allocatedTarget;
	}
}
