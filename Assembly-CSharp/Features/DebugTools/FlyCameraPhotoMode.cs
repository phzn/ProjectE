using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.DebugTools
{
	// Token: 0x02001342 RID: 4930
	[DisallowMultipleComponent]
	public sealed class FlyCameraPhotoMode : MonoBehaviour
	{
		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x0600984A RID: 38986 RVA: 0x003E65B7 File Offset: 0x003E47B7
		// (set) Token: 0x0600984B RID: 38987 RVA: 0x003E65BF File Offset: 0x003E47BF
		public bool IsOpen { get; private set; }

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x0600984C RID: 38988 RVA: 0x003E65C8 File Offset: 0x003E47C8
		public static bool AnyOpen
		{
			get
			{
				return FlyCameraPhotoMode.OpenModes.Count > 0;
			}
		}

		// Token: 0x0600984D RID: 38989 RVA: 0x003E65D8 File Offset: 0x003E47D8
		public static bool IsPhotoCamera(Camera camera)
		{
			if (camera == null)
			{
				return false;
			}
			foreach (FlyCameraPhotoMode flyCameraPhotoMode in FlyCameraPhotoMode.OpenModes)
			{
				if (flyCameraPhotoMode != null && flyCameraPhotoMode.IsOpen && flyCameraPhotoMode._camera == camera)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17001844 RID: 6212
		// (get) Token: 0x0600984E RID: 38990 RVA: 0x003E6654 File Offset: 0x003E4854
		public bool IsCapturing
		{
			get
			{
				return this._capture != null || this._saveTask != null;
			}
		}

		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x0600984F RID: 38991 RVA: 0x003E6669 File Offset: 0x003E4869
		// (set) Token: 0x06009850 RID: 38992 RVA: 0x003E6671 File Offset: 0x003E4871
		public string LastSavedPath { get; private set; }

		// Token: 0x06009851 RID: 38993 RVA: 0x003E667A File Offset: 0x003E487A
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetOpenModes()
		{
			FlyCameraPhotoMode.OpenModes.Clear();
		}

		// Token: 0x06009852 RID: 38994 RVA: 0x003E6688 File Offset: 0x003E4888
		public void Enter(Camera camera, Camera gameplayCamera, string exitKey = "K", bool showSpeedControl = true)
		{
			if (camera == null || this.IsOpen)
			{
				return;
			}
			this._camera = camera;
			this._cameraData = camera.GetUniversalAdditionalCameraData();
			this._previousPostProcessing = this._cameraData.renderPostProcessing;
			this._previousVolumeMask = this._cameraData.volumeLayerMask;
			this._previousVolumeTrigger = this._cameraData.volumeTrigger;
			UniversalAdditionalCameraData universalAdditionalCameraData = ((gameplayCamera != null) ? gameplayCamera.GetComponent<UniversalAdditionalCameraData>() : null);
			this._cameraData.volumeLayerMask = ((universalAdditionalCameraData != null) ? universalAdditionalCameraData.volumeLayerMask : this._previousVolumeMask);
			UniversalAdditionalCameraData cameraData = this._cameraData;
			cameraData.volumeLayerMask |= 4;
			this._cameraData.volumeTrigger = camera.transform;
			this._cameraData.renderPostProcessing = true;
			this.EnsurePresentation();
			this._volume.transform.SetParent(camera.transform, false);
			this._volume.gameObject.SetActive(true);
			this._viewfinder.Initialize(camera, exitKey, showSpeedControl);
			this._viewfinder.SetCaptureInProgress(this._saveTask != null);
			this._viewfinder.SetVisible(true);
			this.IsOpen = true;
			FlyCameraPhotoMode.OpenModes.Add(this);
		}

		// Token: 0x06009853 RID: 38995 RVA: 0x003E67CC File Offset: 0x003E49CC
		public void Exit()
		{
			this.IsOpen = false;
			FlyCameraPhotoMode.OpenModes.Remove(this);
			if (this._capture != null)
			{
				base.StopCoroutine(this._capture);
				this._capture = null;
			}
			if (this._viewfinder != null)
			{
				this._viewfinder.SetVisible(false);
			}
			if (this._volume != null)
			{
				this._volume.gameObject.SetActive(false);
			}
			if (this._cameraData != null)
			{
				this._cameraData.renderPostProcessing = this._previousPostProcessing;
				this._cameraData.volumeLayerMask = this._previousVolumeMask;
				this._cameraData.volumeTrigger = this._previousVolumeTrigger;
				this._cameraData = null;
			}
			this._camera = null;
		}

		// Token: 0x06009854 RID: 38996 RVA: 0x003E6890 File Offset: 0x003E4A90
		public void CapturePhoto()
		{
			if (!this.IsOpen || !base.isActiveAndEnabled || this.IsCapturing || this._camera == null || !this._camera.enabled || Time.unscaledTime < this._nextCaptureTime)
			{
				return;
			}
			this._nextCaptureTime = Time.unscaledTime + 0.45f;
			this._viewfinder.SetCaptureInProgress(true);
			this._capture = base.StartCoroutine(this.CaptureAtEndOfFrame());
		}

		// Token: 0x06009855 RID: 38997 RVA: 0x003E690D File Offset: 0x003E4B0D
		private IEnumerator CaptureAtEndOfFrame()
		{
			this._viewfinder.SetVisible(false);
			yield return new WaitForEndOfFrame();
			Texture2D texture2D = null;
			try
			{
				if (!this.IsOpen || this._camera == null || !this._camera.enabled)
				{
					yield break;
				}
				texture2D = ScreenCapture.CaptureScreenshotAsTexture();
				if (texture2D == null)
				{
					throw new InvalidOperationException("The screen capture returned no image.");
				}
				byte[] png = texture2D.EncodeToPNG();
				if (png == null || png.Length == 0)
				{
					throw new InvalidOperationException("PNG encoding failed.");
				}
				string primary = PlayerMediaExport.GetDirectory("Screenshots");
				string fileName = string.Concat(new string[]
				{
					"Photo_",
					DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"),
					"_",
					Guid.NewGuid().ToString("N").Substring(0, 8),
					".png"
				});
				this._saveTask = Task.Run<string>(() => FlyCameraPhotoMode.SavePng(png, primary, primary, fileName));
				this._viewfinder.ShowShutter();
				this._shutterAudio.PlayOneShot(this._shutterClip, 0.55f);
				yield break;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PhotoMode] Could not capture photo: " + ex.Message, this);
				this._viewfinder.ShowError();
				yield break;
			}
			finally
			{
				if (texture2D != null)
				{
					Object.Destroy(texture2D);
				}
				this._viewfinder.SetVisible(this.IsOpen);
				this._capture = null;
			}
			yield break;
		}

		// Token: 0x06009856 RID: 38998 RVA: 0x003E691C File Offset: 0x003E4B1C
		private void Update()
		{
			if (this._saveTask == null || !this._saveTask.IsCompleted)
			{
				return;
			}
			Task<string> saveTask = this._saveTask;
			this._saveTask = null;
			if (this._viewfinder != null)
			{
				this._viewfinder.SetCaptureInProgress(false);
			}
			if (saveTask.IsFaulted)
			{
				Debug.LogWarning("[PhotoMode] Could not save photo: " + saveTask.Exception.GetBaseException().Message, this);
				if (this.IsOpen)
				{
					this._viewfinder.ShowError();
				}
				return;
			}
			this.LastSavedPath = saveTask.Result;
			Debug.Log("[PhotoMode] Photo saved: " + this.LastSavedPath, this);
			if (this.IsOpen)
			{
				this._viewfinder.ShowSaved("Media / FlyMode / Photos");
			}
		}

		// Token: 0x06009857 RID: 38999 RVA: 0x003E69DD File Offset: 0x003E4BDD
		internal static string GetScreenshotDirectory(string dataPath, RuntimePlatform platform)
		{
			return Path.Combine(PlayerMediaExport.ResolveRoot(dataPath, platform), "FlyMode", "Photos");
		}

		// Token: 0x06009858 RID: 39000 RVA: 0x003E69F8 File Offset: 0x003E4BF8
		internal static string SavePng(byte[] png, string primary, string fallback, string fileName)
		{
			string text;
			try
			{
				text = FlyCameraPhotoMode.WritePng(png, primary, fileName);
			}
			catch (UnauthorizedAccessException)
			{
				text = FlyCameraPhotoMode.WritePng(png, fallback, fileName);
			}
			catch (IOException)
			{
				text = FlyCameraPhotoMode.WritePng(png, fallback, fileName);
			}
			return text;
		}

		// Token: 0x06009859 RID: 39001 RVA: 0x003E6A48 File Offset: 0x003E4C48
		private static string WritePng(byte[] png, string directory, string fileName)
		{
			Directory.CreateDirectory(directory);
			string text = Path.Combine(directory, fileName);
			using (FileStream fileStream = new FileStream(text, FileMode.CreateNew, FileAccess.Write, FileShare.None))
			{
				fileStream.Write(png, 0, png.Length);
			}
			return text;
		}

		// Token: 0x0600985A RID: 39002 RVA: 0x003E6A98 File Offset: 0x003E4C98
		private void EnsurePresentation()
		{
			if (this._volume == null)
			{
				GameObject gameObject = new GameObject("[PhotoMode] Lens Volume")
				{
					layer = 2
				};
				gameObject.transform.SetParent(base.transform, false);
				SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
				sphereCollider.isTrigger = true;
				sphereCollider.radius = 0.025f;
				this._volume = gameObject.AddComponent<Volume>();
				this._volume.isGlobal = false;
				this._volume.priority = 20000f;
				this._volume.blendDistance = 0f;
				VolumeProfile volumeProfile = Resources.Load<VolumeProfile>("FlyCameraPhotoProfile");
				if (volumeProfile != null)
				{
					this._volume.sharedProfile = volumeProfile;
					this._profile = this._volume.profile;
				}
				else
				{
					this._profile = FlyCameraPhotoMode.CreatePhotoProfile();
					this._volume.sharedProfile = this._profile;
				}
				this._profile.name = "FlyCamera Photo Profile (Runtime)";
				this._profile.hideFlags = HideFlags.DontSave;
			}
			if (this._viewfinder == null)
			{
				GameObject gameObject2 = new GameObject("[PhotoMode] Viewfinder");
				gameObject2.transform.SetParent(base.transform, false);
				this._viewfinder = gameObject2.AddComponent<FlyCameraPhotoViewfinder>();
			}
			if (this._shutterAudio == null)
			{
				this._shutterAudio = base.gameObject.AddComponent<AudioSource>();
				this._shutterAudio.playOnAwake = false;
				this._shutterAudio.spatialBlend = 0f;
				this._shutterAudio.ignoreListenerPause = true;
				this._shutterClip = FlyCameraPhotoMode.CreateShutterClip();
			}
		}

		// Token: 0x0600985B RID: 39003 RVA: 0x003E6C20 File Offset: 0x003E4E20
		internal static VolumeProfile CreatePhotoProfile()
		{
			VolumeProfile volumeProfile = ScriptableObject.CreateInstance<VolumeProfile>();
			ColorAdjustments colorAdjustments = volumeProfile.Add<ColorAdjustments>(false);
			colorAdjustments.contrast.Override(12f);
			colorAdjustments.saturation.Override(-6f);
			volumeProfile.Add<WhiteBalance>(false).temperature.Override(5f);
			Vignette vignette = volumeProfile.Add<Vignette>(false);
			vignette.intensity.Override(0.24f);
			vignette.smoothness.Override(0.55f);
			FilmGrain filmGrain = volumeProfile.Add<FilmGrain>(false);
			filmGrain.type.Override(FilmGrainLookup.Thin1);
			filmGrain.intensity.Override(0.12f);
			filmGrain.response.Override(0.85f);
			volumeProfile.Add<ChromaticAberration>(false).intensity.Override(0.035f);
			volumeProfile.Add<MotionBlur>(false).intensity.Override(0f);
			return volumeProfile;
		}

		// Token: 0x0600985C RID: 39004 RVA: 0x003E6CF4 File Offset: 0x003E4EF4
		private static AudioClip CreateShutterClip()
		{
			float[] array = new float[7497];
			Random random = new Random(73);
			for (int i = 0; i < array.Length; i++)
			{
				float num = (float)i / 44100f;
				float num2 = Mathf.Exp(-num * 155f);
				float num3 = ((num >= 0.055f) ? (Mathf.Exp(-(num - 0.055f) * 110f) * 0.7f) : 0f);
				float num4 = (float)(random.NextDouble() * 2.0 - 1.0) * 0.65f + Mathf.Sin(num * 2f * 3.1415927f * 1850f) * 0.25f;
				array[i] = num4 * (num2 + num3) * 0.55f;
			}
			AudioClip audioClip = AudioClip.Create("Photo shutter", array.Length, 1, 44100, false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x0600985D RID: 39005 RVA: 0x003E6DDA File Offset: 0x003E4FDA
		private void OnDisable()
		{
			this.Exit();
		}

		// Token: 0x0600985E RID: 39006 RVA: 0x003E6DE4 File Offset: 0x003E4FE4
		private void OnDestroy()
		{
			this.Exit();
			if (this._volume != null)
			{
				FlyCameraPhotoMode.Release(this._volume.gameObject);
			}
			if (this._viewfinder != null)
			{
				FlyCameraPhotoMode.Release(this._viewfinder.gameObject);
			}
			if (this._profile != null)
			{
				foreach (VolumeComponent volumeComponent in this._profile.components)
				{
					FlyCameraPhotoMode.Release(volumeComponent);
				}
				FlyCameraPhotoMode.Release(this._profile);
			}
			if (this._shutterAudio != null)
			{
				FlyCameraPhotoMode.Release(this._shutterAudio);
			}
			if (this._shutterClip != null)
			{
				FlyCameraPhotoMode.Release(this._shutterClip);
			}
		}

		// Token: 0x0600985F RID: 39007 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void Release(Object value)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x04009D68 RID: 40296
		private static readonly HashSet<FlyCameraPhotoMode> OpenModes = new HashSet<FlyCameraPhotoMode>();

		// Token: 0x04009D6A RID: 40298
		private Camera _camera;

		// Token: 0x04009D6B RID: 40299
		private UniversalAdditionalCameraData _cameraData;

		// Token: 0x04009D6C RID: 40300
		private bool _previousPostProcessing;

		// Token: 0x04009D6D RID: 40301
		private LayerMask _previousVolumeMask;

		// Token: 0x04009D6E RID: 40302
		private Transform _previousVolumeTrigger;

		// Token: 0x04009D6F RID: 40303
		private Volume _volume;

		// Token: 0x04009D70 RID: 40304
		private VolumeProfile _profile;

		// Token: 0x04009D71 RID: 40305
		private FlyCameraPhotoViewfinder _viewfinder;

		// Token: 0x04009D72 RID: 40306
		private AudioSource _shutterAudio;

		// Token: 0x04009D73 RID: 40307
		private AudioClip _shutterClip;

		// Token: 0x04009D74 RID: 40308
		private Coroutine _capture;

		// Token: 0x04009D75 RID: 40309
		private Task<string> _saveTask;

		// Token: 0x04009D76 RID: 40310
		private float _nextCaptureTime;
	}
}
