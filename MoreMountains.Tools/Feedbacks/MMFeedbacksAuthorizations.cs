using System;
using MoreMountains.FeedbacksForThirdParty;
using MoreMountains.Tools;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200019F RID: 415
	public class MMFeedbacksAuthorizations : MMMonoBehaviour
	{
		// Token: 0x060009C0 RID: 2496 RVA: 0x0002CE44 File Offset: 0x0002B044
		private void ToggleAnimation()
		{
			this.AnimationParameter = !this.AnimationParameter;
			this.AnimatorSpeed = !this.AnimatorSpeed;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0002CE64 File Offset: 0x0002B064
		private void ToggleAudio()
		{
			this.AudioFilterDistortion = !this.AudioFilterDistortion;
			this.AudioFilterEcho = !this.AudioFilterEcho;
			this.AudioFilterHighPass = !this.AudioFilterHighPass;
			this.AudioFilterLowPass = !this.AudioFilterLowPass;
			this.AudioFilterReverb = !this.AudioFilterReverb;
			this.AudioMixerSnapshotTransition = !this.AudioMixerSnapshotTransition;
			this.AudioSource = !this.AudioSource;
			this.AudioSourcePitch = !this.AudioSourcePitch;
			this.AudioSourceStereoPan = !this.AudioSourceStereoPan;
			this.AudioSourceVolume = !this.AudioSourceVolume;
			this.MMPlaylist = !this.MMPlaylist;
			this.MMSoundManagerAllSoundsControl = !this.MMSoundManagerAllSoundsControl;
			this.MMSoundManagerSaveAndLoad = !this.MMSoundManagerSaveAndLoad;
			this.MMSoundManagerSound = !this.MMSoundManagerSound;
			this.MMSoundManagerSoundControl = !this.MMSoundManagerSoundControl;
			this.MMSoundManagerSoundFade = !this.MMSoundManagerSoundFade;
			this.MMSoundManagerTrackControl = !this.MMSoundManagerTrackControl;
			this.MMSoundManagerTrackFade = !this.MMSoundManagerTrackFade;
			this.Sound = !this.Sound;
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0002CF90 File Offset: 0x0002B190
		private void ToggleCamera()
		{
			this.CameraShake = !this.CameraShake;
			this.CameraZoom = !this.CameraZoom;
			this.CinemachineImpulse = !this.CinemachineImpulse;
			this.CinemachineImpulseClear = !this.CinemachineImpulseClear;
			this.CinemachineImpulseSource = !this.CinemachineImpulseSource;
			this.CinemachineTransition = !this.CinemachineTransition;
			this.ClippingPlanes = !this.ClippingPlanes;
			this.Fade = !this.Fade;
			this.FieldOfView = !this.FieldOfView;
			this.Flash = !this.Flash;
			this.OrthographicSize = !this.OrthographicSize;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0002D042 File Offset: 0x0002B242
		private void ToggleDebug()
		{
			this.Comment = !this.Comment;
			this.Log = !this.Log;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0002D062 File Offset: 0x0002B262
		private void ToggleEvents()
		{
			this.MMGameEvent = !this.MMGameEvent;
			this.UnityEvents = !this.UnityEvents;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0002D084 File Offset: 0x0002B284
		private void ToggleGameObject()
		{
			this.Broadcast = !this.Broadcast;
			this.Collider = !this.Collider;
			this.Collider2D = !this.Collider2D;
			this.DestroyTargetObject = !this.DestroyTargetObject;
			this.EnableBehaviour = !this.EnableBehaviour;
			this.FloatController = !this.FloatController;
			this.InstantiateObject = !this.InstantiateObject;
			this.MMRadioSignal = !this.MMRadioSignal;
			this.Rigidbody = !this.Rigidbody;
			this.Rigidbody2D = !this.Rigidbody2D;
			this.SetActive = !this.SetActive;
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0002D138 File Offset: 0x0002B338
		private void ToggleHaptics()
		{
			this.HapticClip = !this.HapticClip;
			this.HapticContinuous = !this.HapticContinuous;
			this.HapticControl = !this.HapticControl;
			this.HapticEmphasis = !this.HapticEmphasis;
			this.HapticPreset = !this.HapticPreset;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0002D190 File Offset: 0x0002B390
		private void ToggleLight()
		{
			this.Light = !this.Light;
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0002D1A1 File Offset: 0x0002B3A1
		private void ToggleLoop()
		{
			this.Looper = !this.Looper;
			this.LooperStart = !this.LooperStart;
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0002D1C1 File Offset: 0x0002B3C1
		private void ToggleParticles()
		{
			this.ParticlesInstantiation = !this.ParticlesInstantiation;
			this.ParticlesPlay = !this.ParticlesPlay;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0002D1E1 File Offset: 0x0002B3E1
		private void TogglePause()
		{
			this.HoldingPause = !this.HoldingPause;
			this.Pause = !this.Pause;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0002D204 File Offset: 0x0002B404
		private void ToggleFlicker()
		{
			this.Flicker = !this.Flicker;
			this.Fog = !this.Fog;
			this.Material = !this.Material;
			this.MMBlink = !this.MMBlink;
			this.ShaderGlobal = !this.ShaderGlobal;
			this.ShaderController = !this.ShaderController;
			this.Skybox = !this.Skybox;
			this.SpriteRenderer = !this.SpriteRenderer;
			this.TextureOffset = !this.TextureOffset;
			this.TextureScale = !this.TextureScale;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0002D2A7 File Offset: 0x0002B4A7
		private void ToggleScene()
		{
			this.LoadScene = !this.LoadScene;
			this.UnloadScene = !this.UnloadScene;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0002D2C7 File Offset: 0x0002B4C7
		private void ToggleTime()
		{
			this.FreezeFrame = !this.FreezeFrame;
			this.TimescaleModifier = !this.TimescaleModifier;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0002D2E8 File Offset: 0x0002B4E8
		private void ToggleTransform()
		{
			this.Destination = !this.Destination;
			this.Position = !this.Position;
			this.PositionShake = !this.PositionShake;
			this.RotatePositionAround = !this.RotatePositionAround;
			this.Rotation = !this.Rotation;
			this.RotationShake = !this.RotationShake;
			this.Scale = !this.Scale;
			this.ScaleShake = !this.ScaleShake;
			this.SquashAndStretch = !this.SquashAndStretch;
			this.Wiggle = !this.Wiggle;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0002D38C File Offset: 0x0002B58C
		private void ToggleUI()
		{
			this.CanvasGroup = !this.CanvasGroup;
			this.CanvasGroupBlocksRaycasts = !this.CanvasGroupBlocksRaycasts;
			this.FloatingText = !this.FloatingText;
			this.Graphic = !this.Graphic;
			this.GraphicCrossFade = !this.GraphicCrossFade;
			this.Image = !this.Image;
			this.ImageAlpha = !this.ImageAlpha;
			this.ImageFill = !this.ImageFill;
			this.ImageRaycastTarget = !this.ImageRaycastTarget;
			this.ImageTextureOffset = !this.ImageTextureOffset;
			this.ImageTextureScale = !this.ImageTextureScale;
			this.RectTransformAnchor = !this.RectTransformAnchor;
			this.RectTransformOffset = !this.RectTransformOffset;
			this.RectTransformPivot = !this.RectTransformPivot;
			this.RectTransformSizeDelta = !this.RectTransformSizeDelta;
			this.Text = !this.Text;
			this.TextColor = !this.TextColor;
			this.TextFontSize = !this.TextFontSize;
			this.VideoPlayer = !this.VideoPlayer;
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0002D4B8 File Offset: 0x0002B6B8
		private void ToggleTextMeshPro()
		{
			this.TMPAlpha = !this.TMPAlpha;
			this.TMPCharacterSpacing = !this.TMPCharacterSpacing;
			this.TMPColor = !this.TMPColor;
			this.TMPCountTo = !this.TMPCountTo;
			this.TMPDilate = !this.TMPDilate;
			this.TMPFontSize = !this.TMPFontSize;
			this.TMPLineSpacing = !this.TMPLineSpacing;
			this.TMPOutlineColor = !this.TMPOutlineColor;
			this.TMPOutlineWidth = !this.TMPOutlineWidth;
			this.TMPParagraphSpacing = !this.TMPParagraphSpacing;
			this.TMPSoftness = !this.TMPSoftness;
			this.TMPText = !this.TMPText;
			this.TMPTextReveal = !this.TMPTextReveal;
			this.TMPWordSpacing = !this.TMPWordSpacing;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0002D598 File Offset: 0x0002B798
		private void Start()
		{
			MMF_Animation.FeedbackTypeAuthorized = this.AnimationParameter;
			MMF_AnimatorSpeed.FeedbackTypeAuthorized = this.AnimatorSpeed;
			MMF_AudioFilterDistortion.FeedbackTypeAuthorized = this.AudioFilterDistortion;
			MMF_AudioFilterEcho.FeedbackTypeAuthorized = this.AudioFilterEcho;
			MMF_AudioFilterHighPass.FeedbackTypeAuthorized = this.AudioFilterHighPass;
			MMF_AudioFilterLowPass.FeedbackTypeAuthorized = this.AudioFilterLowPass;
			MMF_AudioFilterReverb.FeedbackTypeAuthorized = this.AudioFilterReverb;
			MMF_AudioMixerSnapshotTransition.FeedbackTypeAuthorized = this.AudioMixerSnapshotTransition;
			MMF_AudioSource.FeedbackTypeAuthorized = this.AudioSource;
			MMF_AudioSourcePitch.FeedbackTypeAuthorized = this.AudioSourcePitch;
			MMF_AudioSourceStereoPan.FeedbackTypeAuthorized = this.AudioSourceStereoPan;
			MMF_AudioSourceVolume.FeedbackTypeAuthorized = this.AudioSourceVolume;
			MMF_Playlist.FeedbackTypeAuthorized = this.MMPlaylist;
			MMF_MMSoundManagerAllSoundsControl.FeedbackTypeAuthorized = this.MMSoundManagerAllSoundsControl;
			MMF_MMSoundManagerSaveLoad.FeedbackTypeAuthorized = this.MMSoundManagerSaveAndLoad;
			MMF_MMSoundManagerSound.FeedbackTypeAuthorized = this.MMSoundManagerSound;
			MMF_MMSoundManagerSoundControl.FeedbackTypeAuthorized = this.MMSoundManagerSoundControl;
			MMF_MMSoundManagerSoundFade.FeedbackTypeAuthorized = this.MMSoundManagerSoundFade;
			MMF_MMSoundManagerTrackControl.FeedbackTypeAuthorized = this.MMSoundManagerTrackControl;
			MMF_MMSoundManagerTrackFade.FeedbackTypeAuthorized = this.MMSoundManagerTrackFade;
			MMF_Sound.FeedbackTypeAuthorized = this.Sound;
			MMF_CameraShake.FeedbackTypeAuthorized = this.CameraShake;
			MMF_CameraZoom.FeedbackTypeAuthorized = this.CameraZoom;
			MMF_CinemachineImpulse.FeedbackTypeAuthorized = this.CinemachineImpulse;
			MMF_CinemachineImpulseClear.FeedbackTypeAuthorized = this.CinemachineImpulseClear;
			MMF_CinemachineImpulseSource.FeedbackTypeAuthorized = this.CinemachineImpulseSource;
			MMF_CinemachineTransition.FeedbackTypeAuthorized = this.CinemachineTransition;
			MMF_CameraClippingPlanes.FeedbackTypeAuthorized = this.ClippingPlanes;
			MMF_CameraFieldOfView.FeedbackTypeAuthorized = this.FieldOfView;
			MMF_CameraOrthographicSize.FeedbackTypeAuthorized = this.OrthographicSize;
			MMF_DebugComment.FeedbackTypeAuthorized = this.Comment;
			MMF_DebugLog.FeedbackTypeAuthorized = this.Log;
			MMF_MMGameEvent.FeedbackTypeAuthorized = this.MMGameEvent;
			MMF_Events.FeedbackTypeAuthorized = this.UnityEvents;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.Broadcast;
			MMF_Collider.FeedbackTypeAuthorized = this.Collider;
			MMF_Collider2D.FeedbackTypeAuthorized = this.Collider2D;
			MMF_Destroy.FeedbackTypeAuthorized = this.DestroyTargetObject;
			MMF_Enable.FeedbackTypeAuthorized = this.EnableBehaviour;
			MMF_FloatController.FeedbackTypeAuthorized = this.FloatController;
			MMF_InstantiateObject.FeedbackTypeAuthorized = this.InstantiateObject;
			MMF_RadioSignal.FeedbackTypeAuthorized = this.MMRadioSignal;
			MMF_Rigidbody.FeedbackTypeAuthorized = this.Rigidbody;
			MMF_Rigidbody2D.FeedbackTypeAuthorized = this.Rigidbody2D;
			MMF_SetActive.FeedbackTypeAuthorized = this.SetActive;
			MMF_Haptics.FeedbackTypeAuthorized = this.HapticClip;
			MMF_NVContinuous.FeedbackTypeAuthorized = this.HapticContinuous;
			MMF_NVControl.FeedbackTypeAuthorized = this.HapticControl;
			MMF_NVEmphasis.FeedbackTypeAuthorized = this.HapticEmphasis;
			MMF_NVPreset.FeedbackTypeAuthorized = this.HapticPreset;
			MMF_Light.FeedbackTypeAuthorized = this.Light;
			MMF_Pause.FeedbackTypeAuthorized = this.Looper;
			MMF_Pause.FeedbackTypeAuthorized = this.LooperStart;
			MMF_ParticlesInstantiation.FeedbackTypeAuthorized = this.ParticlesInstantiation;
			MMF_Particles.FeedbackTypeAuthorized = this.ParticlesPlay;
			MMF_Pause.FeedbackTypeAuthorized = this.HoldingPause;
			MMF_Pause.FeedbackTypeAuthorized = this.Pause;
			MMF_Flicker.FeedbackTypeAuthorized = this.Flicker;
			MMF_Fog.FeedbackTypeAuthorized = this.Fog;
			MMF_Material.FeedbackTypeAuthorized = this.Material;
			MMF_Blink.FeedbackTypeAuthorized = this.MMBlink;
			MMF_ShaderGlobal.FeedbackTypeAuthorized = this.ShaderGlobal;
			MMF_Skybox.FeedbackTypeAuthorized = this.Skybox;
			MMF_SpriteRenderer.FeedbackTypeAuthorized = this.SpriteRenderer;
			MMF_TextureOffset.FeedbackTypeAuthorized = this.TextureOffset;
			MMF_TextureScale.FeedbackTypeAuthorized = this.TextureScale;
			MMF_LoadScene.FeedbackTypeAuthorized = this.LoadScene;
			MMF_UnloadScene.FeedbackTypeAuthorized = this.UnloadScene;
			MMF_FreezeFrame.FeedbackTypeAuthorized = this.FreezeFrame;
			MMF_TimescaleModifier.FeedbackTypeAuthorized = this.TimescaleModifier;
			MMF_DestinationTransform.FeedbackTypeAuthorized = this.Destination;
			MMF_Position.FeedbackTypeAuthorized = this.Position;
			MMF_PositionShake.FeedbackTypeAuthorized = this.PositionShake;
			MMF_RotatePositionAround.FeedbackTypeAuthorized = this.RotatePositionAround;
			MMF_Rotation.FeedbackTypeAuthorized = this.Rotation;
			MMF_RotationShake.FeedbackTypeAuthorized = this.RotationShake;
			MMF_Scale.FeedbackTypeAuthorized = this.Scale;
			MMF_ScaleShake.FeedbackTypeAuthorized = this.ScaleShake;
			MMF_SquashAndStretch.FeedbackTypeAuthorized = this.SquashAndStretch;
			MMF_Wiggle.FeedbackTypeAuthorized = this.Wiggle;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.CanvasGroup;
			MMF_CanvasGroupBlocksRaycasts.FeedbackTypeAuthorized = this.CanvasGroupBlocksRaycasts;
			MMF_FloatingText.FeedbackTypeAuthorized = this.FloatingText;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.RectTransformAnchor;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.RectTransformOffset;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.RectTransformPivot;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.RectTransformSizeDelta;
			MMF_VideoPlayer.FeedbackTypeAuthorized = this.VideoPlayer;
			MMF_ShaderController.FeedbackTypeAuthorized = this.ShaderController;
			MMF_Graphic.FeedbackTypeAuthorized = this.Graphic;
			MMF_GraphicCrossFade.FeedbackTypeAuthorized = this.GraphicCrossFade;
			MMF_Image.FeedbackTypeAuthorized = this.Image;
			MMF_ImageAlpha.FeedbackTypeAuthorized = this.ImageAlpha;
			MMF_ImageFill.FeedbackTypeAuthorized = this.ImageFill;
			MMF_ImageRaycastTarget.FeedbackTypeAuthorized = this.ImageRaycastTarget;
			MMF_ImageTextureOffset.FeedbackTypeAuthorized = this.ImageTextureOffset;
			MMF_ImageTextureScale.FeedbackTypeAuthorized = this.ImageTextureScale;
			MMF_Text.FeedbackTypeAuthorized = this.Text;
			MMF_TextColor.FeedbackTypeAuthorized = this.TextColor;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.TextFontSize;
			MMF_Fade.FeedbackTypeAuthorized = this.Fade;
			MMF_Flash.FeedbackTypeAuthorized = this.Flash;
			MMF_Bloom_URP.FeedbackTypeAuthorized = this.Bloom;
			MMF_ChromaticAberration_URP.FeedbackTypeAuthorized = this.ChromaticAberration;
			MMF_LensDistortion_URP.FeedbackTypeAuthorized = this.LensDistortion;
			MMF_ColorAdjustments_URP.FeedbackTypeAuthorized = this.ColorGrading;
			MMF_LensDistortion_URP.FeedbackTypeAuthorized = this.LensDistortion;
			MMF_Vignette_URP.FeedbackTypeAuthorized = this.Vignette;
			MMF_TMPAlpha.FeedbackTypeAuthorized = this.TMPAlpha;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.TMPCharacterSpacing;
			MMF_TMPColor.FeedbackTypeAuthorized = this.TMPColor;
			MMF_TMPCountTo.FeedbackTypeAuthorized = this.TMPCountTo;
			MMF_TMPDilate.FeedbackTypeAuthorized = this.TMPDilate;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.TMPFontSize;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.TMPLineSpacing;
			MMF_TMPOutlineColor.FeedbackTypeAuthorized = this.TMPOutlineColor;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.TMPOutlineWidth;
			MMF_FeedbackBase.FeedbackTypeAuthorized = this.TMPParagraphSpacing;
			MMF_TMPSoftness.FeedbackTypeAuthorized = this.TMPSoftness;
			MMF_TMPText.FeedbackTypeAuthorized = this.TMPText;
			MMF_TMPTextReveal.FeedbackTypeAuthorized = this.TMPTextReveal;
		}

		// Token: 0x04000B86 RID: 2950
		[MMInspectorGroup("Animation", true, 16, false)]
		[MMInspectorButton("ToggleAnimation")]
		public bool ToggleAnimationButton;

		// Token: 0x04000B87 RID: 2951
		public bool AnimationParameter = true;

		// Token: 0x04000B88 RID: 2952
		public bool AnimatorSpeed = true;

		// Token: 0x04000B89 RID: 2953
		[MMInspectorGroup("Audio", true, 17, false)]
		[MMInspectorButton("ToggleAudio")]
		public bool ToggleAudioButton;

		// Token: 0x04000B8A RID: 2954
		public bool AudioFilterDistortion = true;

		// Token: 0x04000B8B RID: 2955
		public bool AudioFilterEcho = true;

		// Token: 0x04000B8C RID: 2956
		public bool AudioFilterHighPass = true;

		// Token: 0x04000B8D RID: 2957
		public bool AudioFilterLowPass = true;

		// Token: 0x04000B8E RID: 2958
		public bool AudioFilterReverb = true;

		// Token: 0x04000B8F RID: 2959
		public bool AudioMixerSnapshotTransition = true;

		// Token: 0x04000B90 RID: 2960
		public bool AudioSource = true;

		// Token: 0x04000B91 RID: 2961
		public bool AudioSourcePitch = true;

		// Token: 0x04000B92 RID: 2962
		public bool AudioSourceStereoPan = true;

		// Token: 0x04000B93 RID: 2963
		public bool AudioSourceVolume = true;

		// Token: 0x04000B94 RID: 2964
		public bool MMPlaylist = true;

		// Token: 0x04000B95 RID: 2965
		public bool MMSoundManagerAllSoundsControl = true;

		// Token: 0x04000B96 RID: 2966
		public bool MMSoundManagerSaveAndLoad = true;

		// Token: 0x04000B97 RID: 2967
		public bool MMSoundManagerSound = true;

		// Token: 0x04000B98 RID: 2968
		public bool MMSoundManagerSoundControl = true;

		// Token: 0x04000B99 RID: 2969
		public bool MMSoundManagerSoundFade = true;

		// Token: 0x04000B9A RID: 2970
		public bool MMSoundManagerTrackControl = true;

		// Token: 0x04000B9B RID: 2971
		public bool MMSoundManagerTrackFade = true;

		// Token: 0x04000B9C RID: 2972
		public bool Sound = true;

		// Token: 0x04000B9D RID: 2973
		[MMInspectorGroup("Camera", true, 18, false)]
		[MMInspectorButton("ToggleCamera")]
		public bool ToggleCameraButton;

		// Token: 0x04000B9E RID: 2974
		public bool CameraShake = true;

		// Token: 0x04000B9F RID: 2975
		public bool CameraZoom = true;

		// Token: 0x04000BA0 RID: 2976
		public bool CinemachineImpulse = true;

		// Token: 0x04000BA1 RID: 2977
		public bool CinemachineImpulseClear = true;

		// Token: 0x04000BA2 RID: 2978
		public bool CinemachineImpulseSource = true;

		// Token: 0x04000BA3 RID: 2979
		public bool CinemachineTransition = true;

		// Token: 0x04000BA4 RID: 2980
		public bool ClippingPlanes = true;

		// Token: 0x04000BA5 RID: 2981
		public bool Fade = true;

		// Token: 0x04000BA6 RID: 2982
		public bool FieldOfView = true;

		// Token: 0x04000BA7 RID: 2983
		public bool Flash = true;

		// Token: 0x04000BA8 RID: 2984
		public bool OrthographicSize = true;

		// Token: 0x04000BA9 RID: 2985
		[MMInspectorGroup("Debug", true, 19, false)]
		[MMInspectorButton("ToggleDebug")]
		public bool ToggleDebugButton;

		// Token: 0x04000BAA RID: 2986
		public bool Comment = true;

		// Token: 0x04000BAB RID: 2987
		public bool Log = true;

		// Token: 0x04000BAC RID: 2988
		[MMInspectorGroup("Events", true, 20, false)]
		[MMInspectorButton("ToggleEvents")]
		public bool ToggleEventsButton;

		// Token: 0x04000BAD RID: 2989
		public bool MMGameEvent = true;

		// Token: 0x04000BAE RID: 2990
		public bool UnityEvents = true;

		// Token: 0x04000BAF RID: 2991
		[MMInspectorGroup("GameObject", true, 47, false)]
		[MMInspectorButton("ToggleGameObject")]
		public bool ToggleGameObjectButton;

		// Token: 0x04000BB0 RID: 2992
		public bool Broadcast = true;

		// Token: 0x04000BB1 RID: 2993
		public bool Collider = true;

		// Token: 0x04000BB2 RID: 2994
		public bool Collider2D = true;

		// Token: 0x04000BB3 RID: 2995
		public bool DestroyTargetObject = true;

		// Token: 0x04000BB4 RID: 2996
		public bool EnableBehaviour = true;

		// Token: 0x04000BB5 RID: 2997
		public bool FloatController = true;

		// Token: 0x04000BB6 RID: 2998
		public bool InstantiateObject = true;

		// Token: 0x04000BB7 RID: 2999
		public bool MMRadioSignal = true;

		// Token: 0x04000BB8 RID: 3000
		public bool Rigidbody = true;

		// Token: 0x04000BB9 RID: 3001
		public bool Rigidbody2D = true;

		// Token: 0x04000BBA RID: 3002
		public bool SetActive = true;

		// Token: 0x04000BBB RID: 3003
		[MMInspectorGroup("Haptics", true, 22, false)]
		[MMInspectorButton("ToggleHaptics")]
		public bool ToggleHapticsButton;

		// Token: 0x04000BBC RID: 3004
		public bool HapticClip = true;

		// Token: 0x04000BBD RID: 3005
		public bool HapticContinuous = true;

		// Token: 0x04000BBE RID: 3006
		public bool HapticControl = true;

		// Token: 0x04000BBF RID: 3007
		public bool HapticEmphasis = true;

		// Token: 0x04000BC0 RID: 3008
		public bool HapticPreset = true;

		// Token: 0x04000BC1 RID: 3009
		[MMInspectorGroup("Light", true, 23, false)]
		[MMInspectorButton("ToggleLight")]
		public bool ToggleLightButton;

		// Token: 0x04000BC2 RID: 3010
		public bool Light = true;

		// Token: 0x04000BC3 RID: 3011
		[MMInspectorGroup("Loop", true, 24, false)]
		[MMInspectorButton("ToggleLoop")]
		public bool ToggleLoopButton;

		// Token: 0x04000BC4 RID: 3012
		public bool Looper = true;

		// Token: 0x04000BC5 RID: 3013
		public bool LooperStart = true;

		// Token: 0x04000BC6 RID: 3014
		[MMInspectorGroup("Particles", true, 25, false)]
		[MMInspectorButton("ToggleParticles")]
		public bool ToggleParticlesButton;

		// Token: 0x04000BC7 RID: 3015
		public bool ParticlesInstantiation = true;

		// Token: 0x04000BC8 RID: 3016
		public bool ParticlesPlay = true;

		// Token: 0x04000BC9 RID: 3017
		[MMInspectorGroup("Pause", true, 26, false)]
		[MMInspectorButton("TogglePause")]
		public bool TogglePauseButton;

		// Token: 0x04000BCA RID: 3018
		public bool HoldingPause = true;

		// Token: 0x04000BCB RID: 3019
		public bool Pause = true;

		// Token: 0x04000BCC RID: 3020
		[MMInspectorGroup("Post Process", true, 27, false)]
		[MMInspectorButton("TogglePostProcess")]
		public bool TogglePostProcessButton;

		// Token: 0x04000BCD RID: 3021
		public bool Bloom = true;

		// Token: 0x04000BCE RID: 3022
		public bool ChromaticAberration = true;

		// Token: 0x04000BCF RID: 3023
		public bool ColorGrading = true;

		// Token: 0x04000BD0 RID: 3024
		public bool DepthOfField = true;

		// Token: 0x04000BD1 RID: 3025
		public bool GlobalPPVolumeAutoBlend = true;

		// Token: 0x04000BD2 RID: 3026
		public bool LensDistortion = true;

		// Token: 0x04000BD3 RID: 3027
		public bool PPMovingFilter = true;

		// Token: 0x04000BD4 RID: 3028
		public bool Vignette = true;

		// Token: 0x04000BD5 RID: 3029
		[MMInspectorGroup("Flicker", true, 28, false)]
		[MMInspectorButton("ToggleFlicker")]
		public bool ToggleFlickerButton;

		// Token: 0x04000BD6 RID: 3030
		public bool Flicker = true;

		// Token: 0x04000BD7 RID: 3031
		public bool Fog = true;

		// Token: 0x04000BD8 RID: 3032
		public bool Material = true;

		// Token: 0x04000BD9 RID: 3033
		public bool MMBlink = true;

		// Token: 0x04000BDA RID: 3034
		public bool ShaderGlobal = true;

		// Token: 0x04000BDB RID: 3035
		public bool ShaderController = true;

		// Token: 0x04000BDC RID: 3036
		public bool Skybox = true;

		// Token: 0x04000BDD RID: 3037
		public bool SpriteRenderer = true;

		// Token: 0x04000BDE RID: 3038
		public bool TextureOffset = true;

		// Token: 0x04000BDF RID: 3039
		public bool TextureScale = true;

		// Token: 0x04000BE0 RID: 3040
		[MMInspectorGroup("Scene", true, 29, false)]
		[MMInspectorButton("ToggleScene")]
		public bool ToggleSceneButton;

		// Token: 0x04000BE1 RID: 3041
		public bool LoadScene = true;

		// Token: 0x04000BE2 RID: 3042
		public bool UnloadScene = true;

		// Token: 0x04000BE3 RID: 3043
		[MMInspectorGroup("Time", true, 31, false)]
		[MMInspectorButton("ToggleTime")]
		public bool ToggleTimeButton;

		// Token: 0x04000BE4 RID: 3044
		public bool FreezeFrame = true;

		// Token: 0x04000BE5 RID: 3045
		public bool TimescaleModifier = true;

		// Token: 0x04000BE6 RID: 3046
		[MMInspectorGroup("Transform", true, 32, false)]
		[MMInspectorButton("ToggleTransform")]
		public bool ToggleTransformButton;

		// Token: 0x04000BE7 RID: 3047
		public bool Destination = true;

		// Token: 0x04000BE8 RID: 3048
		public bool Position = true;

		// Token: 0x04000BE9 RID: 3049
		public bool PositionShake = true;

		// Token: 0x04000BEA RID: 3050
		public bool RotatePositionAround = true;

		// Token: 0x04000BEB RID: 3051
		public bool Rotation = true;

		// Token: 0x04000BEC RID: 3052
		public bool RotationShake = true;

		// Token: 0x04000BED RID: 3053
		public bool Scale = true;

		// Token: 0x04000BEE RID: 3054
		public bool ScaleShake = true;

		// Token: 0x04000BEF RID: 3055
		public bool SquashAndStretch = true;

		// Token: 0x04000BF0 RID: 3056
		public bool Wiggle = true;

		// Token: 0x04000BF1 RID: 3057
		[MMInspectorGroup("UI", true, 33, false)]
		[MMInspectorButton("ToggleUI")]
		public bool ToggleUiButton;

		// Token: 0x04000BF2 RID: 3058
		public bool CanvasGroup = true;

		// Token: 0x04000BF3 RID: 3059
		public bool CanvasGroupBlocksRaycasts = true;

		// Token: 0x04000BF4 RID: 3060
		public bool FloatingText = true;

		// Token: 0x04000BF5 RID: 3061
		public bool Graphic = true;

		// Token: 0x04000BF6 RID: 3062
		public bool GraphicCrossFade = true;

		// Token: 0x04000BF7 RID: 3063
		public bool Image = true;

		// Token: 0x04000BF8 RID: 3064
		public bool ImageAlpha = true;

		// Token: 0x04000BF9 RID: 3065
		public bool ImageFill = true;

		// Token: 0x04000BFA RID: 3066
		public bool ImageRaycastTarget = true;

		// Token: 0x04000BFB RID: 3067
		public bool ImageTextureOffset = true;

		// Token: 0x04000BFC RID: 3068
		public bool ImageTextureScale = true;

		// Token: 0x04000BFD RID: 3069
		public bool RectTransformAnchor = true;

		// Token: 0x04000BFE RID: 3070
		public bool RectTransformOffset = true;

		// Token: 0x04000BFF RID: 3071
		public bool RectTransformPivot = true;

		// Token: 0x04000C00 RID: 3072
		public bool RectTransformSizeDelta = true;

		// Token: 0x04000C01 RID: 3073
		public bool Text = true;

		// Token: 0x04000C02 RID: 3074
		public bool TextColor = true;

		// Token: 0x04000C03 RID: 3075
		public bool TextFontSize = true;

		// Token: 0x04000C04 RID: 3076
		public bool VideoPlayer = true;

		// Token: 0x04000C05 RID: 3077
		[MMInspectorGroup("TextMesh Pro", true, 30, false)]
		[MMInspectorButton("ToggleTextMeshPro")]
		public bool ToggleTextMeshProButton;

		// Token: 0x04000C06 RID: 3078
		public bool TMPAlpha = true;

		// Token: 0x04000C07 RID: 3079
		public bool TMPCharacterSpacing = true;

		// Token: 0x04000C08 RID: 3080
		public bool TMPColor = true;

		// Token: 0x04000C09 RID: 3081
		public bool TMPCountTo = true;

		// Token: 0x04000C0A RID: 3082
		public bool TMPDilate = true;

		// Token: 0x04000C0B RID: 3083
		public bool TMPFontSize = true;

		// Token: 0x04000C0C RID: 3084
		public bool TMPLineSpacing = true;

		// Token: 0x04000C0D RID: 3085
		public bool TMPOutlineColor = true;

		// Token: 0x04000C0E RID: 3086
		public bool TMPOutlineWidth = true;

		// Token: 0x04000C0F RID: 3087
		public bool TMPParagraphSpacing = true;

		// Token: 0x04000C10 RID: 3088
		public bool TMPSoftness = true;

		// Token: 0x04000C11 RID: 3089
		public bool TMPText = true;

		// Token: 0x04000C12 RID: 3090
		public bool TMPTextReveal = true;

		// Token: 0x04000C13 RID: 3091
		public bool TMPWordSpacing = true;
	}
}
