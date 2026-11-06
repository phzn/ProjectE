using System;
using System.IO;
using System.Threading;
using Features.PhoneCompanion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x020003D1 RID: 977
	public class CameraController : MonoBehaviour
	{
		// Token: 0x06001909 RID: 6409 RVA: 0x0009F000 File Offset: 0x0009D200
		private void Start()
		{
			this.folder = PhonePhotoStorage.ImagesFolder;
			this.zoom05xButton.onClick.AddListener(delegate
			{
				this.SetTargetFOV(100f, this.zoom05xButton);
			});
			this.zoom1xButton.onClick.AddListener(delegate
			{
				this.SetTargetFOV(70f, this.zoom1xButton);
			});
			this.zoom5xButton.onClick.AddListener(delegate
			{
				this.SetTargetFOV(40f, this.zoom5xButton);
			});
			this.zoom10xButton.onClick.AddListener(delegate
			{
				this.SetTargetFOV(30f, this.zoom10xButton);
			});
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0009F088 File Offset: 0x0009D288
		private void SetTargetFOV(float newFOV, Button clickedButton)
		{
			this.targetFOV = newFOV;
			clickedButton.transform.localScale = new Vector3(this.buttonScaleFactor, this.buttonScaleFactor, 1f);
			foreach (Button button in new Button[] { this.zoom05xButton, this.zoom1xButton, this.zoom5xButton, this.zoom10xButton })
			{
				if (button != clickedButton)
				{
					button.transform.localScale = Vector3.one;
					button.GetComponentInChildren<TMP_Text>().color = Color.white;
				}
			}
			clickedButton.GetComponentInChildren<TMP_Text>().color = this.textSelectedColor;
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0009F134 File Offset: 0x0009D334
		private string uniqueFilename(int width, int height)
		{
			this.folder = PhonePhotoStorage.ImagesFolder;
			int num = Random.Range(10000, 999999);
			return string.Format("{0}/IMG_{1}.{2}", this.folder, num, this.format.ToString().ToLower());
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x0009F188 File Offset: 0x0009D388
		public void CaptureScreenshot()
		{
			this.capture = true;
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x0009F194 File Offset: 0x0009D394
		public void LoadImages(byte[] imageByte, Image img)
		{
			Texture2D texture2D = new Texture2D(1920, 1080);
			texture2D.LoadImage(imageByte);
			Vector2 vector = new Vector2(0.5f, 0.5f);
			Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), vector, 100f);
			img.sprite = sprite;
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x0009F1FB File Offset: 0x0009D3FB
		private void DisableScreenShotSnap()
		{
			this.screenShotPanel.SetActive(false);
			this.LoadImages(this.fileDataSnap, this.recentSnapFR);
			this.LoadImages(this.fileDataSnap, this.recentSnapBK);
			this.fileDataSnap = null;
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x0009F234 File Offset: 0x0009D434
		private void Update()
		{
			this.cFront.targetTexture = this.rt;
			this.cBack.targetTexture = this.rt;
			if (this.cBack.isActiveAndEnabled)
			{
				this.currentCamera = this.cBack;
			}
			else if (this.cFront.isActiveAndEnabled)
			{
				this.currentCamera = this.cFront;
			}
			this.currentCamera.fieldOfView = Mathf.Lerp(this.currentCamera.fieldOfView, this.targetFOV, Time.deltaTime * this.zoomSpeed);
			this.currentCamera.fieldOfView = Mathf.Clamp(this.currentCamera.fieldOfView, this.minFOV, this.maxFOV);
			if (this.capture)
			{
				this.capture = false;
				if (this.hideGameObject != null)
				{
					this.hideGameObject.SetActive(false);
				}
				if (this.renderTexture == null)
				{
					this.rect = new Rect(0f, 0f, (float)this.captureWidth, (float)this.captureHeight);
					this.renderTexture = new RenderTexture(this.captureWidth, this.captureHeight, 24);
					this.screenShot = new Texture2D(this.captureWidth, this.captureHeight, TextureFormat.RGB24, false);
				}
				Camera component = base.GetComponent<Camera>();
				component.targetTexture = this.renderTexture;
				component.Render();
				RenderTexture.active = this.renderTexture;
				this.screenShot.ReadPixels(this.rect, 0, 0);
				component.targetTexture = null;
				RenderTexture.active = null;
				string filename = this.uniqueFilename((int)this.rect.width, (int)this.rect.height);
				byte[] fileHeader = null;
				byte[] fileData = null;
				if (this.format == CameraController.Format.PNG)
				{
					fileData = this.screenShot.EncodeToPNG();
					this.fileDataSnap = this.screenShot.EncodeToPNG();
					this.LoadImages(fileData, this.screenShotPanel.GetComponent<Image>());
					this.screenShotPanel.SetActive(true);
					base.Invoke("DisableScreenShotSnap", 2f);
					GameObject gameObject = Object.Instantiate<GameObject>(this.imagePrefab);
					gameObject.transform.SetParent(this.imageParent, false);
					gameObject.transform.SetAsFirstSibling();
					gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
					gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					this.LoadImages(fileData, gameObject.transform.GetChild(0).GetComponent<Image>());
				}
				new Thread(delegate
				{
					FileStream fileStream = File.Create(filename);
					if (fileHeader != null)
					{
						fileStream.Write(fileHeader, 0, fileHeader.Length);
					}
					fileStream.Write(fileData, 0, fileData.Length);
					fileStream.Close();
				}).Start();
				if (this.hideGameObject != null)
				{
					this.hideGameObject.SetActive(true);
				}
				if (!this.optimizeForManyScreenshots)
				{
					Object.Destroy(this.renderTexture);
					this.renderTexture = null;
					this.screenShot = null;
				}
			}
		}

		// Token: 0x04001A08 RID: 6664
		public int captureWidth;

		// Token: 0x04001A09 RID: 6665
		public int captureHeight;

		// Token: 0x04001A0A RID: 6666
		public GameObject hideGameObject;

		// Token: 0x04001A0B RID: 6667
		public bool optimizeForManyScreenshots = true;

		// Token: 0x04001A0C RID: 6668
		public CameraController.Format format;

		// Token: 0x04001A0D RID: 6669
		public string folder;

		// Token: 0x04001A0E RID: 6670
		private byte[] fileDataSnap;

		// Token: 0x04001A0F RID: 6671
		private Rect rect;

		// Token: 0x04001A10 RID: 6672
		private RenderTexture renderTexture;

		// Token: 0x04001A11 RID: 6673
		[HideInInspector]
		public Texture2D screenShot;

		// Token: 0x04001A12 RID: 6674
		private bool capture;

		// Token: 0x04001A13 RID: 6675
		public GameObject screenShotPanel;

		// Token: 0x04001A14 RID: 6676
		public Camera cFront;

		// Token: 0x04001A15 RID: 6677
		public Camera cBack;

		// Token: 0x04001A16 RID: 6678
		public RenderTexture rt;

		// Token: 0x04001A17 RID: 6679
		public Image recentSnapFR;

		// Token: 0x04001A18 RID: 6680
		public Image recentSnapBK;

		// Token: 0x04001A19 RID: 6681
		public GameObject imagePrefab;

		// Token: 0x04001A1A RID: 6682
		public Transform imageParent;

		// Token: 0x04001A1B RID: 6683
		public Button zoom05xButton;

		// Token: 0x04001A1C RID: 6684
		public Button zoom1xButton;

		// Token: 0x04001A1D RID: 6685
		public Button zoom5xButton;

		// Token: 0x04001A1E RID: 6686
		public Button zoom10xButton;

		// Token: 0x04001A1F RID: 6687
		public float zoomSpeed = 2f;

		// Token: 0x04001A20 RID: 6688
		public float minFOV = 30f;

		// Token: 0x04001A21 RID: 6689
		public float maxFOV = 100f;

		// Token: 0x04001A22 RID: 6690
		private float targetFOV = 70f;

		// Token: 0x04001A23 RID: 6691
		public float buttonScaleFactor = 1.35f;

		// Token: 0x04001A24 RID: 6692
		public Color textSelectedColor = Color.red;

		// Token: 0x04001A25 RID: 6693
		private Camera currentCamera;

		// Token: 0x020003D2 RID: 978
		public enum Format
		{
			// Token: 0x04001A27 RID: 6695
			PNG
		}
	}
}
