using System;
using System.Collections;
using System.Collections.Generic;
using Features.CensorBars;
using Features.Detection;
using Features.Localization;
using Features.MainMenu;
using Features.MallHack;
using Features.MatchRecap;
using Features.Missions;
using Features.WantedPoster;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.NPCs;
using ProjectExhibitionist.PhotoBooth;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Features.Score
{
	// Token: 0x02000C61 RID: 3169
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RunScoreHUD))]
	public sealed class NpcPhotoRewardController : MonoBehaviour
	{
		// Token: 0x06005BDA RID: 23514 RVA: 0x00240B8C File Offset: 0x0023ED8C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRewardCooldown()
		{
			NpcPhotoRewardController._nextRewardAllowedTime = 0f;
		}

		// Token: 0x06005BDB RID: 23515 RVA: 0x00240B98 File Offset: 0x0023ED98
		private void Awake()
		{
			this._scoreHud = base.GetComponent<RunScoreHUD>();
			Text componentInChildren = base.GetComponentInChildren<Text>(true);
			this._font = LocalizedLegacyGameText.SourceFont(componentInChildren);
			this.EnsureOverlay();
			this.EnsureAudio();
			this._portraitVariantCursor = Random.Range(0, NpcPhotoRewardController.BeautyYawOffsets.Length);
		}

		// Token: 0x06005BDC RID: 23516 RVA: 0x00240BE4 File Offset: 0x0023EDE4
		private void OnEnable()
		{
			if (!NpcPhotoRewardController._enabledControllers.Contains(this))
			{
				NpcPhotoRewardController._enabledControllers.Add(this);
			}
			this.TryAcquireGlobalSubscription();
			if (this._ownsGlobalSubscription)
			{
				this.PrewarmPortraitResources();
				this.EnsureRendererWarmup();
			}
		}

		// Token: 0x06005BDD RID: 23517 RVA: 0x00240C18 File Offset: 0x0023EE18
		private void Update()
		{
			if (this._overlayCanvas != null)
			{
				this._overlayCanvas.enabled = !NpcPhotoRewardController.IsDetectionFlowActive();
			}
			if (!this._ownsGlobalSubscription && NpcPhotoRewardController._globalOwner == null)
			{
				this.TryAcquireGlobalSubscription();
				if (this._ownsGlobalSubscription)
				{
					this.PrewarmPortraitResources();
					this.EnsureRendererWarmup();
				}
			}
		}

		// Token: 0x06005BDE RID: 23518 RVA: 0x00240C78 File Offset: 0x0023EE78
		private void OnDisable()
		{
			RunScoreHUD.ReleaseScorePresentation(this);
			bool ownsGlobalSubscription = this._ownsGlobalSubscription;
			NpcPhotoRewardController._enabledControllers.Remove(this);
			this.ReleaseGlobalSubscription();
			NpcPhotoRewardController.PromoteStandbyOwner();
			base.StopAllCoroutines();
			this._captureRoutine = null;
			this._presentationPumpRoutine = null;
			this._rendererWarmupRoutine = null;
			this._flashRoutine = null;
			this._captureRequests.Clear();
			this.RestoreSubjectAfterCapture();
			while (this._waitingPresentations.Count > 0)
			{
				NpcPhotoRewardController.PhotoPresentation value = this._waitingPresentations.First.Value;
				this._waitingPresentations.RemoveFirst();
				this.ReleasePresentation(value);
			}
			for (int i = 0; i < this._liveCards.Count; i++)
			{
				NpcPhotoRewardController.CardRuntime cardRuntime = this._liveCards[i];
				if (cardRuntime.Photo != null)
				{
					cardRuntime.Photo.texture = null;
				}
				this.ReleasePresentation(cardRuntime.Presentation);
				if (cardRuntime.Rect != null)
				{
					Object.Destroy(cardRuntime.Rect.gameObject);
				}
			}
			this._liveCards.Clear();
			this.ReleaseInFlightPresentations();
			this._activeCardCount = 0;
			Array.Clear(this._occupiedSlots, 0, this._occupiedSlots.Length);
			if (this._overlayRoot != null)
			{
				Object.Destroy(this._overlayRoot.gameObject);
			}
			this._overlayRoot = null;
			this._overlayCanvas = null;
			this._screenFlash = null;
			if (ownsGlobalSubscription && this._scoreHud != null)
			{
				this._scoreHud.RevealAuthoritativeScore(false);
			}
		}

		// Token: 0x06005BDF RID: 23519 RVA: 0x00240DF4 File Offset: 0x0023EFF4
		private void OnDestroy()
		{
			NpcPhotoRewardController._enabledControllers.Remove(this);
			this.ReleaseGlobalSubscription();
			NpcPhotoRewardController.PromoteStandbyOwner();
			if (this._portraitCamera != null)
			{
				Object.Destroy(this._portraitCamera.gameObject);
			}
			this._portraitCamera = null;
			this.ReleaseInFlightPresentations();
			while (this._portraitTexturePool.Count > 0)
			{
				NpcPhotoRewardController.DestroyPortraitTexture(this._portraitTexturePool.Dequeue());
			}
		}

		// Token: 0x06005BE0 RID: 23520 RVA: 0x00240E64 File Offset: 0x0023F064
		private void OnValidate()
		{
			this.pointsPerPhoto = Mathf.Max(1, this.pointsPerPhoto);
			this.portraitWidth = Mathf.Max(128, this.portraitWidth);
			this.portraitHeight = Mathf.Max(128, this.portraitHeight);
			this.portraitFarClipPlane = Mathf.Clamp(this.portraitFarClipPlane, 20f, 120f);
			this.enterDuration = Mathf.Max(0.05f, this.enterDuration);
			this.holdDuration = Mathf.Max(0f, this.holdDuration);
			this.exitDuration = Mathf.Max(0.05f, this.exitDuration);
		}

		// Token: 0x06005BE1 RID: 23521 RVA: 0x00240F0C File Offset: 0x0023F10C
		private void TryAcquireGlobalSubscription()
		{
			if (this._ownsGlobalSubscription)
			{
				return;
			}
			if (NpcPhotoRewardController._globalOwner != null && NpcPhotoRewardController._globalOwner != this)
			{
				return;
			}
			NpcPhotoRewardController._globalOwner = this;
			this._ownsGlobalSubscription = true;
			SmartphoneFlash.OnPhotoCaptured -= this.HandleNpcPhotoCaptured;
			SmartphoneFlash.OnPhotoCaptured += this.HandleNpcPhotoCaptured;
		}

		// Token: 0x06005BE2 RID: 23522 RVA: 0x00240F6C File Offset: 0x0023F16C
		private void ReleaseGlobalSubscription()
		{
			SmartphoneFlash.OnPhotoCaptured -= this.HandleNpcPhotoCaptured;
			if (NpcPhotoRewardController._globalOwner == this)
			{
				NpcPhotoRewardController._globalOwner = null;
			}
			this._ownsGlobalSubscription = false;
		}

		// Token: 0x06005BE3 RID: 23523 RVA: 0x00240F9C File Offset: 0x0023F19C
		private static void PromoteStandbyOwner()
		{
			if (NpcPhotoRewardController._globalOwner != null)
			{
				return;
			}
			for (int i = NpcPhotoRewardController._enabledControllers.Count - 1; i >= 0; i--)
			{
				NpcPhotoRewardController npcPhotoRewardController = NpcPhotoRewardController._enabledControllers[i];
				if (npcPhotoRewardController == null)
				{
					NpcPhotoRewardController._enabledControllers.RemoveAt(i);
				}
				else if (npcPhotoRewardController.isActiveAndEnabled)
				{
					npcPhotoRewardController.TryAcquireGlobalSubscription();
					if (npcPhotoRewardController._ownsGlobalSubscription)
					{
						if (npcPhotoRewardController._scoreHud != null)
						{
							npcPhotoRewardController._scoreHud.RevealAuthoritativeScore(false);
						}
						npcPhotoRewardController.PrewarmPortraitResources();
						npcPhotoRewardController.EnsureRendererWarmup();
						return;
					}
				}
			}
		}

		// Token: 0x06005BE4 RID: 23524 RVA: 0x00241030 File Offset: 0x0023F230
		private static bool DeferNextPositiveChangeOnAllHuds()
		{
			bool flag = false;
			for (int i = NpcPhotoRewardController._enabledControllers.Count - 1; i >= 0; i--)
			{
				NpcPhotoRewardController npcPhotoRewardController = NpcPhotoRewardController._enabledControllers[i];
				if (npcPhotoRewardController == null)
				{
					NpcPhotoRewardController._enabledControllers.RemoveAt(i);
				}
				else
				{
					RunScoreHUD scoreHud = npcPhotoRewardController._scoreHud;
					if (scoreHud != null && scoreHud.DeferNextPositiveChange())
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06005BE5 RID: 23525 RVA: 0x00241094 File Offset: 0x0023F294
		private static void CancelDeferredPositiveChangeOnAllHuds()
		{
			for (int i = NpcPhotoRewardController._enabledControllers.Count - 1; i >= 0; i--)
			{
				NpcPhotoRewardController npcPhotoRewardController = NpcPhotoRewardController._enabledControllers[i];
				if (npcPhotoRewardController == null)
				{
					NpcPhotoRewardController._enabledControllers.RemoveAt(i);
				}
				else if (npcPhotoRewardController._scoreHud != null)
				{
					npcPhotoRewardController._scoreHud.CancelDeferredPositiveChange();
				}
			}
		}

		// Token: 0x06005BE6 RID: 23526 RVA: 0x002410F4 File Offset: 0x0023F2F4
		private static void RevealRewardOnAllHuds(int scoreTotal, RunScoreHUD impactHud)
		{
			for (int i = NpcPhotoRewardController._enabledControllers.Count - 1; i >= 0; i--)
			{
				NpcPhotoRewardController npcPhotoRewardController = NpcPhotoRewardController._enabledControllers[i];
				if (npcPhotoRewardController == null)
				{
					NpcPhotoRewardController._enabledControllers.RemoveAt(i);
				}
				else
				{
					RunScoreHUD scoreHud = npcPhotoRewardController._scoreHud;
					if (!(scoreHud == null))
					{
						if (scoreHud == impactHud)
						{
							scoreHud.PlayArrivalImpact(scoreTotal);
						}
						else
						{
							scoreHud.RevealThroughTotal(scoreTotal);
						}
					}
				}
			}
		}

		// Token: 0x06005BE7 RID: 23527 RVA: 0x00241164 File Offset: 0x0023F364
		private void HandleNpcPhotoCaptured(SmartphoneFlash phone)
		{
			if (!NpcPhotoRewardController.IsEligibleCielPhoto(phone))
			{
				return;
			}
			if (Time.unscaledTime < NpcPhotoRewardController._nextRewardAllowedTime)
			{
				return;
			}
			MatchStatsService instance = MatchStatsService.Instance;
			if (instance == null || !instance.IsRunTrackingActive)
			{
				return;
			}
			Transform root = phone.transform.root;
			this._captureRequests.Enqueue(new NpcPhotoRewardController.PhotoRequest(phone.transform.position, (root != null) ? root.name : "NPC", Time.frameCount));
			if (this._captureRoutine == null)
			{
				this._captureRoutine = base.StartCoroutine(this.CaptureQueuedPhotosAtEndOfFrame());
			}
		}

		// Token: 0x06005BE8 RID: 23528 RVA: 0x002411FC File Offset: 0x0023F3FC
		public static bool IsEligibleCielPhoto(SmartphoneFlash phone)
		{
			if (NpcPhotoRewardController.IsDetectionFlowActive())
			{
				return false;
			}
			if (phone == null || !phone.gameObject.activeInHierarchy)
			{
				return false;
			}
			NPCWantedPosterBehaviour componentInParent = phone.GetComponentInParent<NPCWantedPosterBehaviour>();
			if (componentInParent != null && componentInParent.IsPhotographingPoster)
			{
				return false;
			}
			NPCPhotoBoothPrintBehaviour componentInParent2 = phone.GetComponentInParent<NPCPhotoBoothPrintBehaviour>();
			if (componentInParent2 != null && componentInParent2.IsPhotographingPrint)
			{
				return false;
			}
			HackMeNpcObserver componentInParent3 = phone.GetComponentInParent<HackMeNpcObserver>();
			return (!(componentInParent3 != null) || !componentInParent3.IsBusy) && !HackMeAudienceMember.IsWatchingHackedScreen(phone);
		}

		// Token: 0x06005BE9 RID: 23529 RVA: 0x00241284 File Offset: 0x0023F484
		public static bool TryCopyLatestPortrait(RenderTexture destination, int minimumShutterFrame)
		{
			if (destination == null || !destination.IsCreated())
			{
				return false;
			}
			if (NpcPhotoRewardController._latestSharedPortrait == null || !NpcPhotoRewardController._latestSharedPortrait.IsCreated())
			{
				return false;
			}
			if (NpcPhotoRewardController._latestSharedPortraitShutterFrame != minimumShutterFrame)
			{
				return false;
			}
			float num = (float)NpcPhotoRewardController._latestSharedPortrait.width / (float)NpcPhotoRewardController._latestSharedPortrait.height;
			float num2 = (float)destination.width / (float)destination.height;
			Vector2 one = Vector2.one;
			if (num < num2)
			{
				one.y = num / num2;
			}
			else if (num > num2)
			{
				one.x = num2 / num;
			}
			Vector2 vector = (Vector2.one - one) * 0.5f;
			Graphics.Blit(NpcPhotoRewardController._latestSharedPortrait, destination, one, vector);
			return true;
		}

		// Token: 0x06005BEA RID: 23530 RVA: 0x00241339 File Offset: 0x0023F539
		private static void PublishSharedPortrait(RenderTexture portrait, int shutterFrame)
		{
			NpcPhotoRewardController._latestSharedPortrait = portrait;
			NpcPhotoRewardController._latestSharedPortraitShutterFrame = shutterFrame;
		}

		// Token: 0x06005BEB RID: 23531 RVA: 0x00241347 File Offset: 0x0023F547
		private IEnumerator CaptureQueuedPhotosAtEndOfFrame()
		{
			while (this._captureRequests.Count > 0)
			{
				yield return new WaitForEndOfFrame();
				NpcPhotoRewardController.PhotoRequest photoRequest = this._captureRequests.Dequeue();
				int num = 1;
				while (this._captureRequests.Count > 0 && this._captureRequests.Peek().ShutterFrame == photoRequest.ShutterFrame && this._captureRequests.Peek().Subject == photoRequest.Subject)
				{
					photoRequest = this._captureRequests.Dequeue();
					num = NpcPhotoRewardController.SaturatingAdd(num, 1);
				}
				if (!NpcPhotoRewardController.IsDetectionFlowActive())
				{
					NpcPhotoRewardController.PhotoTagResult photoTagResult;
					RenderTexture renderTexture = this.CaptureCielPortrait(photoRequest, out photoTagResult);
					if (!(renderTexture == null))
					{
						if (NpcPhotoRewardController.IsDetectionFlowActive())
						{
							this.ReleaseTexture(renderTexture);
						}
						else
						{
							MatchStatsService instance = MatchStatsService.Instance;
							int num2 = ((instance != null) ? instance.CurrentPoints : 0);
							bool flag = NpcPhotoRewardController.DeferNextPositiveChangeOnAllHuds();
							int num3 = NpcPhotoRewardController.SaturatingMultiply(NpcPhotoRewardController.SaturatingAdd(this.pointsPerPhoto, photoTagResult.BonusPoints), num);
							RunScoreAwardRequest runScoreAwardRequest = new RunScoreAwardRequest(RunScoreReason.NpcPhoto, num3, "NPC PHOTO", photoTagResult.DisplayText, "photo:" + photoRequest.ShutterFrame.ToString(), RunScorePresentation.External);
							RunScoreAward runScoreAward;
							if (instance == null || !instance.AwardPoints(runScoreAwardRequest, out runScoreAward))
							{
								if (flag)
								{
									NpcPhotoRewardController.CancelDeferredPositiveChangeOnAllHuds();
								}
								this.ReleaseTexture(renderTexture);
							}
							else
							{
								MissionGameplaySignals.NotifyNpcPhotosCommitted(num);
								NpcPhotoRewardController._nextRewardAllowedTime = Time.unscaledTime + this.rewardCooldownSeconds;
								int currentPoints = instance.CurrentPoints;
								int num4 = Mathf.Max(0, currentPoints - num2);
								NpcPhotoRewardController.PublishSharedPortrait(renderTexture, photoRequest.ShutterFrame);
								NpcPhotoSessionArchive.Add(renderTexture, photoRequest.SourceName, num);
								this.EnqueuePresentation(new NpcPhotoRewardController.PhotoPresentation
								{
									Texture = renderTexture,
									Points = num4,
									ScoreTotal = currentPoints,
									PhotoCount = num,
									Tags = photoTagResult.DisplayText
								});
								if (this.debugLogs)
								{
									Debug.Log(string.Format("[NpcPhotoReward] {0}: {1} foto valide, +{2} punti.", photoRequest.SourceName, num, num4), this);
								}
							}
						}
					}
				}
			}
			this._captureRoutine = null;
			this.EnsureRendererWarmup();
			yield break;
		}

		// Token: 0x06005BEC RID: 23532 RVA: 0x00241358 File Offset: 0x0023F558
		private static Transform ResolvePhotoSubject()
		{
			Transform player = PlayerRegistry.Player;
			if (player != null)
			{
				if (!player.gameObject.activeInHierarchy)
				{
					return null;
				}
				return player;
			}
			else
			{
				StealthActor stealthActor = ((SensoryManager.Instance != null) ? SensoryManager.Instance.GetPlayer() : null);
				if (!(stealthActor != null) || !stealthActor.gameObject.activeInHierarchy)
				{
					return null;
				}
				return stealthActor.transform;
			}
		}

		// Token: 0x06005BED RID: 23533 RVA: 0x002413C0 File Offset: 0x0023F5C0
		private RenderTexture CaptureCielPortrait(NpcPhotoRewardController.PhotoRequest request, out NpcPhotoRewardController.PhotoTagResult tags)
		{
			tags = default(NpcPhotoRewardController.PhotoTagResult);
			Transform subject = request.Subject;
			if (subject == null || !subject.gameObject.activeInHierarchy || subject != NpcPhotoRewardController.ResolvePhotoSubject())
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[NpcPhotoReward] Soggetto non piu' disponibile per questo scatto: foto ignorata.", this);
				}
				return null;
			}
			Vector3 vector;
			Vector3 vector2;
			NpcPhotoRewardController.ResolvePortraitFraming(subject.transform, out vector, out vector2);
			float num;
			bool flag;
			Vector3 vector3 = this.PickPortraitPosition(subject.transform, vector, vector2, request.PhonePosition, out num, out flag);
			this.EnsurePortraitCamera();
			if (this._portraitCamera == null)
			{
				return null;
			}
			this.SyncPortraitCameraWithScene();
			if (!flag)
			{
				this._portraitCamera.cullingMask = NpcPhotoRewardController.BuildCielLayerMask();
				this._portraitCamera.clearFlags = CameraClearFlags.Color;
				this._portraitCamera.backgroundColor = new Color(0.07f, 0.09f, 0.14f, 1f);
			}
			this._portraitCamera.fieldOfView = num;
			this._portraitCamera.transform.SetPositionAndRotation(vector3, Quaternion.LookRotation(vector - vector3, Vector3.up));
			tags = this.EvaluatePhotoTags(subject.transform, vector2);
			this.AimPortraitKeyLight(vector3, vector);
			RenderTexture renderTexture = this.AcquirePortraitTexture();
			if (renderTexture == null)
			{
				return null;
			}
			RenderTexture renderTexture2;
			try
			{
				this.PrepareSubjectForCapture(subject.transform);
				this._portraitCamera.targetTexture = renderTexture;
				if (this._portraitKeyLight != null)
				{
					this._portraitKeyLight.enabled = true;
				}
				StillPhotoRenderUtility.Render(this._portraitCamera, renderTexture);
				this._rendererWarmed = true;
				renderTexture2 = renderTexture;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NpcPhotoReward] Cattura fallita: " + ex.Message, this);
				this.ReleaseTexture(renderTexture);
				renderTexture2 = null;
			}
			finally
			{
				if (this._portraitKeyLight != null)
				{
					this._portraitKeyLight.enabled = false;
				}
				this._portraitCamera.targetTexture = null;
				this.RestoreSubjectAfterCapture();
			}
			return renderTexture2;
		}

		// Token: 0x06005BEE RID: 23534 RVA: 0x002415B8 File Offset: 0x0023F7B8
		private NpcPhotoRewardController.PhotoTagResult EvaluatePhotoTags(Transform playerRoot, Vector3 faceForward)
		{
			if (this._portraitCamera == null || playerRoot == null)
			{
				return default(NpcPhotoRewardController.PhotoTagResult);
			}
			Animator componentInChildren = playerRoot.GetComponentInChildren<Animator>();
			Transform transform = ((componentInChildren != null && componentInChildren.isHuman) ? componentInChildren.GetBoneTransform(HumanBodyBones.Head) : null);
			Transform transform2 = ((componentInChildren != null && componentInChildren.isHuman) ? (componentInChildren.GetBoneTransform(HumanBodyBones.UpperChest) ?? componentInChildren.GetBoneTransform(HumanBodyBones.Chest)) : null);
			Transform transform3 = ((componentInChildren != null && componentInChildren.isHuman) ? componentInChildren.GetBoneTransform(HumanBodyBones.Hips) : null);
			Vector3 vector = ((transform != null) ? (transform.position + Vector3.up * 0.04f) : (playerRoot.position + Vector3.up * 1.62f));
			Vector3 vector2 = ((transform2 != null) ? transform2.position : (playerRoot.position + Vector3.up * 1.23f));
			Vector3 vector3 = ((transform3 != null) ? (transform3.position - Vector3.up * 0.06f) : (playerRoot.position + Vector3.up * 0.86f));
			Vector3 vector4 = Vector3.ProjectOnPlane(this._portraitCamera.transform.position - playerRoot.position, Vector3.up);
			if (vector4.sqrMagnitude < 0.0001f)
			{
				vector4 = faceForward;
			}
			vector4.Normalize();
			float num = Vector3.Dot(faceForward.normalized, vector4);
			ClothingToggleController componentInChildren2 = playerRoot.GetComponentInChildren<ClothingToggleController>();
			bool flag = ((componentInChildren2 == null) ? PlayerExposureState.IsExposed : (!componentInChildren2.IsTopPhysicallyCovered));
			bool flag2 = ((componentInChildren2 == null) ? PlayerExposureState.IsExposed : (!componentInChildren2.IsBottomPhysicallyCovered));
			bool flag3 = num > -0.25f && this.IsTagPointInsidePortrait(vector, 0.055f);
			bool flag4 = num > 0.12f && flag && this.IsTagPointInsidePortrait(vector2, 0.075f);
			bool flag5 = num < -0.12f && flag2 && this.IsTagPointInsidePortrait(vector3, 0.075f);
			return new NpcPhotoRewardController.PhotoTagResult(flag3, flag4, flag5);
		}

		// Token: 0x06005BEF RID: 23535 RVA: 0x002417F0 File Offset: 0x0023F9F0
		private bool IsTagPointInsidePortrait(Vector3 worldPoint, float margin)
		{
			Vector3 vector = this._portraitCamera.WorldToViewportPoint(worldPoint);
			return vector.z > this._portraitCamera.nearClipPlane && vector.x >= margin && vector.x <= 1f - margin && vector.y >= margin && vector.y <= 1f - margin;
		}

		// Token: 0x06005BF0 RID: 23536 RVA: 0x00241854 File Offset: 0x0023FA54
		private void EnsurePortraitCamera()
		{
			if (this._portraitCamera != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("NPC Ciel Portrait Camera")
			{
				hideFlags = HideFlags.DontSave
			};
			gameObject.transform.SetParent(base.transform, false);
			this._portraitCamera = gameObject.AddComponent<Camera>();
			this._portraitCamera.enabled = false;
			this._portraitCamera.nearClipPlane = 0.08f;
			this._portraitCamera.farClipPlane = this.portraitFarClipPlane;
			this._portraitCamera.clearFlags = CameraClearFlags.Skybox;
			this._portraitCamera.backgroundColor = new Color(0.07f, 0.09f, 0.14f, 1f);
			this._portraitCamera.cullingMask = NpcPhotoRewardController.BuildPortraitLayerMask(Camera.main);
			this._portraitCamera.allowHDR = false;
			this._portraitCamera.allowMSAA = false;
			this._portraitCamera.allowDynamicResolution = false;
			this._portraitCamera.useOcclusionCulling = false;
			this._portraitCamera.depthTextureMode = DepthTextureMode.None;
			this._portraitCamera.layerCullSpherical = true;
			GameObject gameObject2 = new GameObject("NPC Portrait Face Light")
			{
				hideFlags = HideFlags.DontSave
			};
			gameObject2.transform.SetParent(gameObject.transform, false);
			this._portraitKeyLight = gameObject2.AddComponent<Light>();
			this._portraitKeyLight.type = LightType.Directional;
			this._portraitKeyLight.color = new Color(1f, 0.9f, 0.82f, 1f);
			this._portraitKeyLight.intensity = 0.65f;
			this._portraitKeyLight.shadows = LightShadows.None;
			this._portraitKeyLight.renderMode = LightRenderMode.ForcePixel;
			this._portraitKeyLight.cullingMask = NpcPhotoRewardController.BuildCielLayerMask();
			this._portraitKeyLight.enabled = false;
			UniversalAdditionalCameraData universalAdditionalCameraData = this._portraitCamera.GetUniversalAdditionalCameraData();
			if (universalAdditionalCameraData != null)
			{
				universalAdditionalCameraData.renderType = CameraRenderType.Base;
				universalAdditionalCameraData.SetRenderer(1);
				universalAdditionalCameraData.renderPostProcessing = false;
				universalAdditionalCameraData.renderShadows = false;
				universalAdditionalCameraData.requiresColorOption = CameraOverrideOption.Off;
				universalAdditionalCameraData.requiresDepthOption = CameraOverrideOption.Off;
				universalAdditionalCameraData.antialiasing = AntialiasingMode.None;
				universalAdditionalCameraData.volumeLayerMask = 0;
				universalAdditionalCameraData.stopNaN = false;
				universalAdditionalCameraData.dithering = false;
				universalAdditionalCameraData.allowXRRendering = false;
			}
		}

		// Token: 0x06005BF1 RID: 23537 RVA: 0x00241A68 File Offset: 0x0023FC68
		private void SyncPortraitCameraWithScene()
		{
			if (this._portraitCamera == null)
			{
				return;
			}
			Camera main = Camera.main;
			this._portraitCamera.cullingMask = NpcPhotoRewardController.BuildPortraitLayerMask(main);
			this._portraitCamera.farClipPlane = this.portraitFarClipPlane;
			if (main != null && main.clearFlags == CameraClearFlags.Color)
			{
				this._portraitCamera.clearFlags = CameraClearFlags.Color;
				Color backgroundColor = main.backgroundColor;
				backgroundColor.a = 1f;
				this._portraitCamera.backgroundColor = backgroundColor;
				return;
			}
			if (RenderSettings.skybox != null)
			{
				this._portraitCamera.clearFlags = CameraClearFlags.Skybox;
				return;
			}
			this._portraitCamera.clearFlags = CameraClearFlags.Color;
			this._portraitCamera.backgroundColor = new Color(0.07f, 0.09f, 0.14f, 1f);
		}

		// Token: 0x06005BF2 RID: 23538 RVA: 0x00241B34 File Offset: 0x0023FD34
		private static int BuildPortraitLayerMask(Camera sceneCamera)
		{
			int num = ((sceneCamera != null) ? sceneCamera.cullingMask : 0);
			NpcPhotoRewardController.RemoveLayerFromMask(ref num, "UI");
			NpcPhotoRewardController.RemoveLayerFromMask(ref num, "Triggers");
			NpcPhotoRewardController.RemoveLayerFromMask(ref num, "TVCameraOnly");
			NpcPhotoRewardController.RemoveLayerFromMask(ref num, "StadiumScreenOnly");
			NpcPhotoRewardController.RemoveLayerFromMask(ref num, "StadiumMonitorVCam");
			NpcPhotoRewardController.RemoveLayerFromMask(ref num, "CharacterPreview");
			NpcPhotoRewardController.RemoveLayerFromMask(ref num, "VisionBlocker");
			NpcPhotoRewardController.RemoveLayerFromMask(ref num, "PostProcess");
			num |= NpcPhotoRewardController.BuildCielLayerMask();
			num |= 8;
			NpcPhotoRewardController.AddLayerToMask(ref num, "Default");
			NpcPhotoRewardController.AddLayerToMask(ref num, "ACSCulling");
			NpcPhotoRewardController.AddLayerToMask(ref num, "Floor");
			NpcPhotoRewardController.AddLayerToMask(ref num, "Interactable");
			NpcPhotoRewardController.AddLayerToMask(ref num, "InteractableTerrain");
			NpcPhotoRewardController.AddLayerToMask(ref num, "Glass");
			return num;
		}

		// Token: 0x06005BF3 RID: 23539 RVA: 0x00241C0C File Offset: 0x0023FE0C
		private static int BuildCielLayerMask()
		{
			int num = 0;
			NpcPhotoRewardController.AddLayerToMask(ref num, "Player");
			NpcPhotoRewardController.AddLayerToMask(ref num, "BodyPart");
			NpcPhotoRewardController.AddLayerToMask(ref num, "PlayerHead");
			NpcPhotoRewardController.AddLayerToMask(ref num, "PlayerClothing");
			return num;
		}

		// Token: 0x06005BF4 RID: 23540 RVA: 0x00241C4C File Offset: 0x0023FE4C
		private static void AddLayerToMask(ref int mask, string layerName)
		{
			int num = LayerMask.NameToLayer(layerName);
			if (num >= 0)
			{
				mask |= 1 << num;
			}
		}

		// Token: 0x06005BF5 RID: 23541 RVA: 0x00241C70 File Offset: 0x0023FE70
		private static void RemoveLayerFromMask(ref int mask, string layerName)
		{
			int num = LayerMask.NameToLayer(layerName);
			if (num >= 0)
			{
				mask &= ~(1 << num);
			}
		}

		// Token: 0x06005BF6 RID: 23542 RVA: 0x00241C94 File Offset: 0x0023FE94
		private void PrepareSubjectForCapture(Transform subjectRoot)
		{
			this.RestoreSubjectAfterCapture();
			if (subjectRoot == null || this._portraitCamera == null)
			{
				return;
			}
			int num = LayerMask.NameToLayer("Player");
			if (num < 0)
			{
				return;
			}
			int num2 = NpcPhotoRewardController.BuildCielLayerMask();
			this._subjectRenderers.Clear();
			subjectRoot.GetComponentsInChildren<Renderer>(true, this._subjectRenderers);
			this._posterCaptureVisibility.HideRegisteredPosters(-1);
			for (int i = 0; i < this._subjectRenderers.Count; i++)
			{
				Renderer renderer = this._subjectRenderers[i];
				if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy && !this._posterCaptureVisibility.HideIfPoster(renderer))
				{
					if (renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly || renderer.allowOcclusionWhenDynamic)
					{
						this._subjectRendererRestores.Add(new NpcPhotoRewardController.RendererRestore(renderer, renderer.shadowCastingMode, renderer.allowOcclusionWhenDynamic));
						if (renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
						{
							renderer.shadowCastingMode = ShadowCastingMode.On;
						}
						renderer.allowOcclusionWhenDynamic = false;
					}
					GameObject gameObject = renderer.gameObject;
					int layer = gameObject.layer;
					if ((num2 & (1 << layer)) == 0)
					{
						this._subjectLayerRestores.Add(new NpcPhotoRewardController.LayerRestore(gameObject, layer));
						gameObject.layer = num;
					}
				}
			}
		}

		// Token: 0x06005BF7 RID: 23543 RVA: 0x00241DD4 File Offset: 0x0023FFD4
		private void RestoreSubjectAfterCapture()
		{
			this._posterCaptureVisibility.Restore();
			for (int i = this._subjectRendererRestores.Count - 1; i >= 0; i--)
			{
				NpcPhotoRewardController.RendererRestore rendererRestore = this._subjectRendererRestores[i];
				if (!(rendererRestore.Renderer == null))
				{
					rendererRestore.Renderer.shadowCastingMode = rendererRestore.ShadowCastingMode;
					rendererRestore.Renderer.allowOcclusionWhenDynamic = rendererRestore.AllowOcclusionWhenDynamic;
				}
			}
			this._subjectRendererRestores.Clear();
			for (int j = this._subjectLayerRestores.Count - 1; j >= 0; j--)
			{
				NpcPhotoRewardController.LayerRestore layerRestore = this._subjectLayerRestores[j];
				if (layerRestore.GameObject != null)
				{
					layerRestore.GameObject.layer = layerRestore.Layer;
				}
			}
			this._subjectLayerRestores.Clear();
			this._subjectRenderers.Clear();
		}

		// Token: 0x06005BF8 RID: 23544 RVA: 0x00241EA8 File Offset: 0x002400A8
		private static void ResolvePortraitFraming(Transform playerRoot, out Vector3 aimPoint, out Vector3 faceForward)
		{
			Animator componentInChildren = playerRoot.GetComponentInChildren<Animator>();
			Transform transform = ((componentInChildren != null && componentInChildren.isHuman) ? componentInChildren.GetBoneTransform(HumanBodyBones.Head) : null);
			Transform transform2 = ((componentInChildren != null && componentInChildren.isHuman) ? componentInChildren.GetBoneTransform(HumanBodyBones.LeftEye) : null);
			Transform transform3 = ((componentInChildren != null && componentInChildren.isHuman) ? componentInChildren.GetBoneTransform(HumanBodyBones.RightEye) : null);
			object obj;
			if (!(componentInChildren != null) || !componentInChildren.isHuman)
			{
				obj = null;
			}
			else if ((obj = componentInChildren.GetBoneTransform(HumanBodyBones.UpperChest)) == null)
			{
				obj = componentInChildren.GetBoneTransform(HumanBodyBones.Chest) ?? componentInChildren.GetBoneTransform(HumanBodyBones.Spine);
			}
			Transform transform4 = obj;
			Vector3 up = Vector3.up;
			Vector3 vector;
			if (transform2 != null && transform3 != null)
			{
				vector = (transform2.position + transform3.position) * 0.5f;
			}
			else if (transform != null)
			{
				vector = transform.position + up * 0.06f;
			}
			else
			{
				vector = playerRoot.position + up * 1.62f;
			}
			faceForward = ((transform != null) ? (vector - transform.position) : playerRoot.forward);
			faceForward = Vector3.ProjectOnPlane(faceForward, up);
			if (faceForward.sqrMagnitude < 1E-06f)
			{
				faceForward = ((transform != null) ? Vector3.ProjectOnPlane(transform.forward, up) : Vector3.ProjectOnPlane(playerRoot.forward, up));
			}
			if (faceForward.sqrMagnitude < 1E-06f)
			{
				faceForward = Vector3.forward;
			}
			faceForward.Normalize();
			float num = Mathf.Clamp(((transform4 != null) ? Mathf.Abs(Vector3.Dot(vector - transform4.position, up)) : 0.42f) * 0.42f, 0.13f, 0.18f);
			aimPoint = vector - up * num;
		}

		// Token: 0x06005BF9 RID: 23545 RVA: 0x002420A4 File Offset: 0x002402A4
		private Vector3 PickPortraitPosition(Transform playerRoot, Vector3 aimPoint, Vector3 faceForward, Vector3 phonePosition, out float shotFieldOfView, out bool renderEnvironment)
		{
			int portraitVariantCursor = this._portraitVariantCursor;
			this._portraitVariantCursor = portraitVariantCursor + 1;
			int num = portraitVariantCursor % NpcPhotoRewardController.BeautyYawOffsets.Length;
			if (num < 0)
			{
				num += NpcPhotoRewardController.BeautyYawOffsets.Length;
			}
			Vector3 up = Vector3.up;
			Vector3 vector = Vector3.ProjectOnPlane(phonePosition - aimPoint, up);
			float num2 = ((vector.sqrMagnitude > 0.0001f) ? Vector3.SignedAngle(faceForward, vector.normalized, up) : 0f);
			float num3 = Mathf.Clamp(this.maximumFrontAngle, 24f, 38f);
			float num4 = NpcPhotoRewardController.BeautyYawOffsets[num];
			bool flag = Mathf.Abs(num4) > 90f;
			float num5 = Mathf.Clamp(num2, -num3, num3) * (flag ? 0.1f : 0.28f);
			float num6 = (flag ? Mathf.DeltaAngle(0f, num4 + num5) : Mathf.Clamp(num4 + num5, -num3, num3));
			float num7 = NpcPhotoRewardController.BeautyDistances[num];
			float num8 = NpcPhotoRewardController.BeautyHeightOffsets[num];
			shotFieldOfView = Mathf.Clamp(this.portraitFieldOfView + NpcPhotoRewardController.BeautyFovOffsets[num], 27f, 47f);
			renderEnvironment = true;
			Vector3 vector2 = faceForward;
			float num9 = 0f;
			float num10 = num7;
			for (int i = 0; i < 6; i++)
			{
				float num11;
				switch (i)
				{
				case 0:
					num11 = num6;
					break;
				case 1:
					num11 = Mathf.Clamp(-num6 * 0.72f, -num3, num3);
					break;
				case 2:
					num11 = 0f;
					break;
				case 3:
					num11 = Mathf.Clamp(num2 * 0.45f, -num3, num3);
					break;
				case 4:
					num11 = -num3 * 0.55f;
					break;
				default:
					num11 = num3 * 0.55f;
					break;
				}
				Vector3 vector3 = Quaternion.Euler(0f, num11, 0f) * faceForward;
				Vector3 vector4 = aimPoint + vector3.normalized * num7 + up * num8;
				Vector3 vector5 = vector4 - aimPoint;
				float magnitude = vector5.magnitude;
				Vector3 vector6 = ((magnitude > 0.0001f) ? (vector5 / magnitude) : vector3);
				float availablePortraitCameraDistance = this.GetAvailablePortraitCameraDistance(playerRoot, aimPoint, vector6, magnitude);
				if (availablePortraitCameraDistance >= magnitude - 0.025f)
				{
					return vector4;
				}
				if (availablePortraitCameraDistance > num9)
				{
					num9 = availablePortraitCameraDistance;
					vector2 = vector6;
					num10 = magnitude;
				}
			}
			if (num9 >= 0.68f)
			{
				float num12 = Mathf.Max(0.64f, num9 - 0.035f);
				float num13 = shotFieldOfView * 0.5f * 0.017453292f;
				shotFieldOfView = Mathf.Clamp(2f * Mathf.Atan(Mathf.Tan(num13) * num10 / num12) * 57.29578f, shotFieldOfView, 52f);
				return aimPoint + vector2 * num12;
			}
			renderEnvironment = false;
			shotFieldOfView = Mathf.Max(shotFieldOfView, 40f);
			return aimPoint + faceForward * 0.92f + up * 0.02f;
		}

		// Token: 0x06005BFA RID: 23546 RVA: 0x0024238C File Offset: 0x0024058C
		private float GetAvailablePortraitCameraDistance(Transform playerRoot, Vector3 aimPoint, Vector3 direction, float requestedDistance)
		{
			if (requestedDistance <= 0.2f)
			{
				return requestedDistance;
			}
			int num = Physics.SphereCastNonAlloc(aimPoint + direction * 0.08f, 0.13f, direction, this._cameraProbeHits, Mathf.Max(0f, requestedDistance - 0.08f), NpcPhotoRewardController.BuildPortraitObstructionMask(), QueryTriggerInteraction.Ignore);
			float num2 = requestedDistance;
			for (int i = 0; i < num; i++)
			{
				Collider collider = this._cameraProbeHits[i].collider;
				if (!(collider == null))
				{
					Transform transform = collider.transform;
					if (!(transform == playerRoot) && !transform.IsChildOf(playerRoot))
					{
						num2 = Mathf.Min(num2, Mathf.Max(0f, 0.08f + this._cameraProbeHits[i].distance - 0.025f));
					}
				}
			}
			return num2;
		}

		// Token: 0x06005BFB RID: 23547 RVA: 0x00242458 File Offset: 0x00240658
		private static int BuildPortraitObstructionMask()
		{
			int num = 0;
			NpcPhotoRewardController.AddLayerToMask(ref num, "Default");
			NpcPhotoRewardController.AddLayerToMask(ref num, "Floor");
			NpcPhotoRewardController.AddLayerToMask(ref num, "InteractableTerrain");
			return num;
		}

		// Token: 0x06005BFC RID: 23548 RVA: 0x0024248C File Offset: 0x0024068C
		private void AimPortraitKeyLight(Vector3 cameraPosition, Vector3 aimPoint)
		{
			if (this._portraitKeyLight == null)
			{
				return;
			}
			Vector3 vector = (aimPoint - cameraPosition).normalized + Vector3.down * 0.28f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.down;
			}
			this._portraitKeyLight.transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
		}

		// Token: 0x06005BFD RID: 23549 RVA: 0x00242504 File Offset: 0x00240704
		private void PrewarmPortraitResources()
		{
			this.EnsurePortraitCamera();
			while (this._portraitTexturePool.Count < 2)
			{
				RenderTexture renderTexture = this.CreatePortraitTexture();
				if (renderTexture == null)
				{
					break;
				}
				this._portraitTexturePool.Enqueue(renderTexture);
			}
		}

		// Token: 0x06005BFE RID: 23550 RVA: 0x00242543 File Offset: 0x00240743
		private void EnsureRendererWarmup()
		{
			if (!this._ownsGlobalSubscription || this._rendererWarmed || this._rendererWarmupRoutine != null)
			{
				return;
			}
			this._rendererWarmupRoutine = base.StartCoroutine(this.WarmRendererOutsideShutterFrame());
		}

		// Token: 0x06005BFF RID: 23551 RVA: 0x00242570 File Offset: 0x00240770
		private IEnumerator WarmRendererOutsideShutterFrame()
		{
			yield return null;
			yield return new WaitForEndOfFrame();
			if (this._rendererWarmed || this._captureRoutine != null)
			{
				this._rendererWarmupRoutine = null;
				yield break;
			}
			RenderTexture renderTexture = this.AcquirePortraitTexture();
			if (this._portraitCamera == null || renderTexture == null)
			{
				this._rendererWarmupRoutine = null;
				yield break;
			}
			int cullingMask = this._portraitCamera.cullingMask;
			try
			{
				this._portraitCamera.cullingMask = 0;
				StillPhotoRenderUtility.Render(this._portraitCamera, renderTexture);
				this._rendererWarmed = true;
				yield break;
			}
			catch (Exception ex)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[NpcPhotoReward] Warmup renderer saltato: " + ex.Message, this);
				}
				yield break;
			}
			finally
			{
				this._portraitCamera.targetTexture = null;
				this._portraitCamera.cullingMask = cullingMask;
				this.ReleaseTexture(renderTexture);
				this._rendererWarmupRoutine = null;
			}
			yield break;
		}

		// Token: 0x06005C00 RID: 23552 RVA: 0x00242580 File Offset: 0x00240780
		private RenderTexture AcquirePortraitTexture()
		{
			while (this._portraitTexturePool.Count > 0)
			{
				RenderTexture renderTexture = this._portraitTexturePool.Dequeue();
				if (!(renderTexture == null))
				{
					if (renderTexture.width == this.portraitWidth && renderTexture.height == this.portraitHeight && renderTexture.depth >= 24)
					{
						if (!renderTexture.IsCreated())
						{
							renderTexture.Create();
						}
						Object @object = renderTexture;
						string text = "Ciel NPC Photo {0}";
						int captureSequence = this._captureSequence;
						this._captureSequence = captureSequence + 1;
						@object.name = string.Format(text, captureSequence);
						return renderTexture;
					}
					NpcPhotoRewardController.DestroyPortraitTexture(renderTexture);
				}
			}
			return this.CreatePortraitTexture();
		}

		// Token: 0x06005C01 RID: 23553 RVA: 0x00242620 File Offset: 0x00240820
		private RenderTexture CreatePortraitTexture()
		{
			RenderTexture renderTexture = new RenderTexture(this.portraitWidth, this.portraitHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			string text = "Ciel NPC Photo (pooled {0})";
			int captureSequence = this._captureSequence;
			this._captureSequence = captureSequence + 1;
			renderTexture.name = string.Format(text, captureSequence);
			renderTexture.antiAliasing = 1;
			renderTexture.filterMode = FilterMode.Bilinear;
			renderTexture.wrapMode = TextureWrapMode.Clamp;
			renderTexture.useMipMap = false;
			renderTexture.autoGenerateMips = false;
			renderTexture.useDynamicScale = false;
			RenderTexture renderTexture2 = renderTexture;
			RenderTexture renderTexture3;
			try
			{
				if (!renderTexture2.Create())
				{
					Debug.LogWarning("[NpcPhotoReward] RenderTexture portrait non supportata.", this);
					NpcPhotoRewardController.DestroyPortraitTexture(renderTexture2);
					renderTexture3 = null;
				}
				else
				{
					renderTexture3 = renderTexture2;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NpcPhotoReward] Impossibile preallocare la foto: " + ex.Message, this);
				NpcPhotoRewardController.DestroyPortraitTexture(renderTexture2);
				renderTexture3 = null;
			}
			return renderTexture3;
		}

		// Token: 0x06005C02 RID: 23554 RVA: 0x002426E8 File Offset: 0x002408E8
		private void EnqueuePresentation(NpcPhotoRewardController.PhotoPresentation presentation)
		{
			if (this._waitingPresentations.Count >= 12)
			{
				NpcPhotoRewardController.PhotoPresentation value = this._waitingPresentations.Last.Value;
				value.Points = NpcPhotoRewardController.SaturatingAdd(value.Points, presentation.Points);
				value.PhotoCount = NpcPhotoRewardController.SaturatingAdd(value.PhotoCount, presentation.PhotoCount);
				value.ScoreTotal = presentation.ScoreTotal;
				value.Tags = presentation.Tags;
				this.ReleaseTexture(value.Texture);
				value.Texture = presentation.Texture;
				presentation.Texture = null;
				return;
			}
			this._waitingPresentations.AddLast(presentation);
			this.EnsureOverlay();
			if (this._presentationPumpRoutine == null)
			{
				this._presentationPumpRoutine = base.StartCoroutine(this.StartWaitingPresentationsNextFrame());
			}
		}

		// Token: 0x06005C03 RID: 23555 RVA: 0x002427A8 File Offset: 0x002409A8
		private IEnumerator StartWaitingPresentationsNextFrame()
		{
			yield return null;
			while (NpcPhotoRewardController.IsDetectionFlowActive())
			{
				yield return null;
			}
			this._presentationPumpRoutine = null;
			this.StartWaitingPresentations();
			yield break;
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x002427B8 File Offset: 0x002409B8
		private void StartWaitingPresentations()
		{
			if (!base.isActiveAndEnabled || this._overlayRoot == null)
			{
				return;
			}
			if (NpcPhotoRewardController.IsDetectionFlowActive())
			{
				if (this._presentationPumpRoutine == null)
				{
					this._presentationPumpRoutine = base.StartCoroutine(this.StartWaitingPresentationsNextFrame());
				}
				return;
			}
			while (this._waitingPresentations.Count > 0 && this._activeCardCount < 1)
			{
				int num = this.FindFreeSlot();
				if (num < 0)
				{
					return;
				}
				this._occupiedSlots[num] = true;
				this._activeCardCount++;
				NpcPhotoRewardController.PhotoPresentation value = this._waitingPresentations.First.Value;
				this._waitingPresentations.RemoveFirst();
				base.StartCoroutine(this.PresentCard(value, num));
			}
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x00242864 File Offset: 0x00240A64
		private int FindFreeSlot()
		{
			for (int i = 0; i < this._occupiedSlots.Length; i++)
			{
				if (!this._occupiedSlots[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x00242891 File Offset: 0x00240A91
		private IEnumerator PresentCard(NpcPhotoRewardController.PhotoPresentation presentation, int slot)
		{
			this._presentationsInFlight[slot] = presentation;
			while (NpcPhotoRewardController.IsDetectionFlowActive())
			{
				yield return null;
			}
			while (!RunScoreHUD.TryAcquireScorePresentation(this))
			{
				yield return null;
			}
			NpcPhotoRewardController.CardRuntime card = this.CreateCard(presentation, slot);
			if (card == null)
			{
				NpcPhotoRewardController.RevealRewardOnAllHuds(presentation.ScoreTotal, this._scoreHud);
				this.ReleasePresentation(presentation);
				RunScoreHUD.ReleaseScorePresentation(this);
				this.FinishPresentationSlot(slot);
				yield break;
			}
			this._liveCards.Add(card);
			this.PulseScreenFlash();
			this.PlayPopupSound();
			Vector2 destination = new Vector2(38f + (float)slot * 24f, 34f + (float)slot * 22f);
			Vector2 origin = destination + new Vector2(-125f, -72f);
			card.Rect.anchoredPosition = origin;
			card.Rect.localScale = Vector3.one * 0.84f;
			card.Rect.localRotation = Quaternion.Euler(0f, 0f, -9f + (float)slot * 1.5f);
			card.Group.alpha = 0f;
			this.PlayFeelCardScale(card.Rect);
			float elapsed = 0f;
			while (elapsed < this.enterDuration)
			{
				elapsed += NpcPhotoRewardController.RewardUnscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / this.enterDuration);
				float num2 = NpcPhotoRewardController.EaseOutBack(num);
				card.Rect.anchoredPosition = Vector2.LerpUnclamped(origin, destination, num2);
				card.Rect.localRotation = Quaternion.Euler(0f, 0f, Mathf.LerpUnclamped(-9f + (float)slot * 1.5f, -2.5f + (float)slot, num2));
				card.Group.alpha = NpcPhotoRewardController.EaseOutCubic(num);
				yield return null;
			}
			card.Rect.anchoredPosition = destination;
			card.Group.alpha = 1f;
			while (NpcPhotoRewardController.IsDetectionFlowActive())
			{
				yield return null;
			}
			base.StartCoroutine(this.FlyRewardBadge(card.Badge, presentation.Points, presentation.ScoreTotal));
			float num3 = ((this._waitingPresentations.Count > 0) ? Mathf.Min(this.holdDuration, 0.45f) : this.holdDuration);
			yield return this.WaitUnscaled(num3);
			Vector2 exitPosition = destination + new Vector2(-92f, -30f);
			Vector3 exitScale = Vector3.one * 0.92f;
			Quaternion exitRotation = Quaternion.Euler(0f, 0f, -7f);
			Vector3 startScale = card.Rect.localScale;
			Quaternion startRotation = card.Rect.localRotation;
			elapsed = 0f;
			while (elapsed < this.exitDuration)
			{
				elapsed += NpcPhotoRewardController.RewardUnscaledDeltaTime;
				float num4 = NpcPhotoRewardController.EaseInCubic(Mathf.Clamp01(elapsed / this.exitDuration));
				card.Rect.anchoredPosition = Vector2.Lerp(destination, exitPosition, num4);
				card.Rect.localScale = Vector3.Lerp(startScale, exitScale, num4);
				card.Rect.localRotation = Quaternion.Slerp(startRotation, exitRotation, num4);
				card.Group.alpha = 1f - num4;
				yield return null;
			}
			this._liveCards.Remove(card);
			if (card.Photo != null)
			{
				card.Photo.texture = null;
			}
			if (card.Rect != null)
			{
				Object.Destroy(card.Rect.gameObject);
			}
			this.ReleasePresentation(presentation);
			RunScoreHUD.ReleaseScorePresentation(this);
			this.FinishPresentationSlot(slot);
			yield break;
		}

		// Token: 0x06005C07 RID: 23559 RVA: 0x002428AE File Offset: 0x00240AAE
		private void FinishPresentationSlot(int slot)
		{
			if (slot >= 0 && slot < this._occupiedSlots.Length)
			{
				this._presentationsInFlight[slot] = null;
				this._occupiedSlots[slot] = false;
			}
			this._activeCardCount = Mathf.Max(0, this._activeCardCount - 1);
			this.StartWaitingPresentations();
		}

		// Token: 0x06005C08 RID: 23560 RVA: 0x002428EC File Offset: 0x00240AEC
		private NpcPhotoRewardController.CardRuntime CreateCard(NpcPhotoRewardController.PhotoPresentation presentation, int slot)
		{
			this.EnsureOverlay();
			if (this._overlayRoot == null || presentation.Texture == null || this._font == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject(string.Format("Photographed Card {0}", slot + 1), new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup)
			});
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(this._overlayRoot, false);
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.zero;
			component.pivot = Vector2.zero;
			component.sizeDelta = new Vector2(350f, 450f);
			component.SetAsLastSibling();
			CanvasGroup component2 = gameObject.GetComponent<CanvasGroup>();
			component2.interactable = false;
			component2.blocksRaycasts = false;
			NpcPhotoRewardController.Stretch(this.CreateImage("Shadow", component, new Color(0.05f, 0.015f, 0.08f, 0.5f)).rectTransform, 8f, -8f, 10f, -10f);
			NpcPhotoRewardController.Stretch(this.CreateImage("Polaroid", component, new Color(1f, 0.975f, 0.94f, 1f)).rectTransform, 0f, 0f, 0f, 0f);
			RawImage rawImage = this.CreateRawImage("Ciel Portrait", component, presentation.Texture);
			NpcPhotoRewardController.Stretch(rawImage.rectTransform, 22.5f, 22.5f, 16f, 68f);
			NpcPhotoRewardController.SetAnchoredRect(this.CreateImage("Caption Tint", component, new Color(1f, 0.86f, 0.91f, 1f)).rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0f, 0f), new Vector2(0f, 62f));
			string reaction = NpcPhotoRewardController.PhotoCardLabels[Random.Range(0, NpcPhotoRewardController.PhotoCardLabels.Length)];
			string reactionLabel = ((presentation.PhotoCount > 1) ? string.Format("{0} x{1}!", reaction, presentation.PhotoCount) : (reaction + "!"));
			Text text = this.CreateText("Reaction", component, reactionLabel, 29, FontStyle.Bold, new Color(0.25f, 0.055f, 0.25f, 1f), TextAnchor.MiddleCenter);
			NpcPhotoRewardController.SetAnchoredRect(text.rectTransform, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(12f, 6f), new Vector2(-12f, 60f));
			NpcPhotoRewardController.AddOutline(text, new Color(1f, 1f, 1f, 0.55f), new Vector2(1f, -1f));
			int photoCount = presentation.PhotoCount;
			LocalizedLegacyGameText.BindComputed(text, delegate
			{
				if (GameLocalization.CurrentCode == "en")
				{
					return reactionLabel;
				}
				string source = GameLocalization.GetSource("rank_ui", reaction);
				if (photoCount <= 1)
				{
					return GameLocalization.FormatSource("rank_ui", "{0}!", new object[] { source });
				}
				return GameLocalization.FormatSource("rank_ui", "{0} x{1}!", new object[] { source, photoCount });
			}).FitTranslationToRectangle();
			RectTransform rectTransform = this.CreateImage("Photo Points", component, new Color(0.95f, 0.12f, 0.43f, 1f)).rectTransform;
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			Vector2 vector = new Vector2(1f, 1f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(-35f, -27f);
			rectTransform.sizeDelta = new Vector2(124f, 52f);
			rectTransform.localRotation = Quaternion.Euler(0f, 0f, 4f);
			Text text2 = this.CreateText("Value", rectTransform, string.Format("+{0}", presentation.Points), 23, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter);
			NpcPhotoRewardController.Stretch(text2.rectTransform, 4f, 4f, 4f, 4f);
			NpcPhotoRewardController.AddOutline(text2, new Color(0.35f, 0f, 0.18f, 0.65f), new Vector2(1f, -1f));
			return new NpcPhotoRewardController.CardRuntime
			{
				Presentation = presentation,
				Rect = component,
				Group = component2,
				Badge = rectTransform,
				Photo = rawImage,
				Slot = slot
			};
		}

		// Token: 0x06005C09 RID: 23561 RVA: 0x00242D60 File Offset: 0x00240F60
		private void PlayFeelCardScale(RectTransform card)
		{
			MMF_Player mmf_Player = card.gameObject.AddComponent<MMF_Player>();
			mmf_Player.PlayerTimescaleMode = TimescaleModes.Unscaled;
			MMF_Scale mmf_Scale = (MMF_Scale)mmf_Player.AddFeedback(typeof(MMF_Scale), true);
			mmf_Scale.AnimateScaleTarget = card;
			mmf_Scale.Mode = MMF_Scale.Modes.ToDestination;
			mmf_Scale.AnimateScaleDuration = this.enterDuration;
			mmf_Scale.DestinationScale = Vector3.one;
			mmf_Scale.DetermineScaleOnPlay = true;
			mmf_Scale.UniformScaling = true;
			mmf_Scale.RemapCurveZero = 0f;
			mmf_Scale.RemapCurveOne = 1f;
			MMTweenType mmtweenType = new MMTweenType(new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(0.72f, 1.12f),
				new Keyframe(1f, 1f)
			}));
			mmf_Scale.AnimateScaleTweenX = mmtweenType;
			mmf_Scale.AnimateScaleTweenY = mmtweenType;
			mmf_Scale.AnimateScaleTweenZ = mmtweenType;
			mmf_Player.Initialization(true);
			mmf_Player.PlayFeedbacks();
		}

		// Token: 0x06005C0A RID: 23562 RVA: 0x00242E55 File Offset: 0x00241055
		private IEnumerator FlyRewardBadge(RectTransform sourceBadge, int points, int scoreTotal)
		{
			if (this._scoreHud == null)
			{
				yield break;
			}
			if (this._overlayRoot == null || sourceBadge == null || this._scoreHud.RewardTarget == null || this._font == null)
			{
				NpcPhotoRewardController.RevealRewardOnAllHuds(scoreTotal, this._scoreHud);
				yield break;
			}
			RectTransform token = this.CreateImage("Flying Photo Points", this._overlayRoot, new Color(0.98f, 0.13f, 0.45f, 1f)).rectTransform;
			RectTransform rectTransform = token;
			RectTransform rectTransform2 = token;
			RectTransform rectTransform3 = token;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.pivot = vector;
			rectTransform.anchorMin = (rectTransform2.anchorMax = vector);
			token.sizeDelta = new Vector2(112f, 48f);
			token.SetAsLastSibling();
			NpcPhotoRewardController.Stretch(this.CreateText("Value", token, string.Format("+{0}", points), 21, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter).rectTransform, 3f, 3f, 3f, 3f);
			Vector2 start = this.UiWorldToOverlay(sourceBadge.position);
			Vector2 end = this.UiWorldToOverlay(this._scoreHud.RewardTarget.position);
			Vector2 control = (start + end) * 0.5f + Vector2.up * 145f;
			token.anchoredPosition = start;
			float elapsed = 0f;
			while (elapsed < 0.58f && token != null)
			{
				elapsed += NpcPhotoRewardController.RewardUnscaledDeltaTime;
				float num = NpcPhotoRewardController.EaseInOutCubic(Mathf.Clamp01(elapsed / 0.58f));
				float num2 = 1f - num;
				token.anchoredPosition = num2 * num2 * start + 2f * num2 * num * control + num * num * end;
				token.localScale = Vector3.one * Mathf.Lerp(1f, 0.48f, num);
				token.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(-5f, 12f, num));
				yield return null;
			}
			if (token != null)
			{
				Object.Destroy(token.gameObject);
			}
			while (NpcPhotoRewardController.IsDetectionFlowActive())
			{
				yield return null;
			}
			NpcPhotoRewardController.RevealRewardOnAllHuds(scoreTotal, this._scoreHud);
			base.StartCoroutine(this.PlayStarBurst(end));
			yield break;
		}

		// Token: 0x06005C0B RID: 23563 RVA: 0x00242E79 File Offset: 0x00241079
		private IEnumerator PlayStarBurst(Vector2 center)
		{
			Sprite sprite = ((this._scoreHud != null) ? this._scoreHud.RewardIconSprite : null);
			if (this._overlayRoot == null || sprite == null)
			{
				yield break;
			}
			GameObject gameObject = new GameObject("Photo Reward Star Burst", new Type[] { typeof(RectTransform) });
			RectTransform container = gameObject.GetComponent<RectTransform>();
			container.SetParent(this._overlayRoot, false);
			RectTransform rectTransform = container;
			RectTransform rectTransform2 = container;
			RectTransform rectTransform3 = container;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform3.pivot = vector;
			rectTransform.anchorMin = (rectTransform2.anchorMax = vector);
			container.anchoredPosition = center;
			container.sizeDelta = Vector2.zero;
			container.SetAsLastSibling();
			Image[] stars = new Image[8];
			RectTransform[] starRects = new RectTransform[8];
			for (int i = 0; i < 8; i++)
			{
				stars[i] = this.CreateImage(string.Format("Star {0}", i + 1), container, Color.white);
				stars[i].sprite = sprite;
				stars[i].preserveAspect = true;
				stars[i].raycastTarget = false;
				stars[i].color = ((i % 2 == 0) ? new Color(1f, 0.79f, 0.12f, 1f) : new Color(1f, 0.24f, 0.55f, 1f));
				starRects[i] = stars[i].rectTransform;
				RectTransform rectTransform4 = starRects[i];
				RectTransform rectTransform5 = starRects[i];
				RectTransform rectTransform6 = starRects[i];
				vector = new Vector2(0.5f, 0.5f);
				rectTransform6.pivot = vector;
				rectTransform4.anchorMin = (rectTransform5.anchorMax = vector);
				starRects[i].sizeDelta = Vector2.one * ((i % 2 == 0) ? 26f : 18f);
			}
			float elapsed = 0f;
			while (elapsed < 0.56f && container != null)
			{
				elapsed += NpcPhotoRewardController.RewardUnscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / 0.56f);
				float num2 = NpcPhotoRewardController.EaseOutCubic(num) * 92f;
				for (int j = 0; j < 8; j++)
				{
					float num3 = 45f * (float)j + 12f;
					Vector2 vector2 = new Vector2(Mathf.Cos(num3 * 0.017453292f), Mathf.Sin(num3 * 0.017453292f));
					starRects[j].anchoredPosition = vector2 * num2;
					starRects[j].localRotation = Quaternion.Euler(0f, 0f, num3 + num * 135f);
					starRects[j].localScale = Vector3.one * (1f - num * 0.35f);
					Color color = stars[j].color;
					color.a = 1f - num;
					stars[j].color = color;
				}
				yield return null;
			}
			if (container != null)
			{
				Object.Destroy(container.gameObject);
			}
			yield break;
		}

		// Token: 0x06005C0C RID: 23564 RVA: 0x00242E90 File Offset: 0x00241090
		private void EnsureOverlay()
		{
			if (this._overlayRoot != null)
			{
				return;
			}
			this._rootCanvas = base.GetComponentInParent<Canvas>();
			if (this._rootCanvas == null)
			{
				Debug.LogError("[NpcPhotoReward] Nessun Canvas parent trovato per il popup foto.", this);
				return;
			}
			GameObject gameObject = new GameObject("NPC Photo Reward Overlay", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas)
			});
			this._overlayRoot = gameObject.GetComponent<RectTransform>();
			this._overlayRoot.SetParent(this._rootCanvas.transform, false);
			this._overlayRoot.anchorMin = Vector2.zero;
			this._overlayRoot.anchorMax = Vector2.one;
			this._overlayRoot.offsetMin = Vector2.zero;
			this._overlayRoot.offsetMax = Vector2.zero;
			this._overlayRoot.SetAsLastSibling();
			this._overlayCanvas = gameObject.GetComponent<Canvas>();
			this._overlayCanvas.overrideSorting = true;
			this._overlayCanvas.sortingOrder = this._rootCanvas.sortingOrder + 60;
			GameplayHudVisibility.RegisterCanvas(this._overlayCanvas);
			this._overlayCanvas.enabled = !NpcPhotoRewardController.IsDetectionFlowActive();
			this._screenFlash = this.CreateImage("Capture Flash", this._overlayRoot, new Color(1f, 1f, 1f, 0f));
			NpcPhotoRewardController.Stretch(this._screenFlash.rectTransform, 0f, 0f, 0f, 0f);
			this._screenFlash.raycastTarget = false;
			this._screenFlash.rectTransform.SetAsFirstSibling();
		}

		// Token: 0x06005C0D RID: 23565 RVA: 0x0024302C File Offset: 0x0024122C
		private void EnsureAudio()
		{
			if (this._uiAudio != null)
			{
				return;
			}
			this._uiAudio = base.GetComponent<AudioSource>();
			if (this._uiAudio == null)
			{
				this._uiAudio = base.gameObject.AddComponent<AudioSource>();
			}
			this._uiAudio.playOnAwake = false;
			this._uiAudio.loop = false;
			this._uiAudio.spatialBlend = 0f;
			this._uiAudio.ignoreListenerPause = true;
			GameAudioRouter.Route(this._uiAudio, GameAudioRouter.Channel.Sfx);
		}

		// Token: 0x06005C0E RID: 23566 RVA: 0x002430B3 File Offset: 0x002412B3
		private void PlayPopupSound()
		{
			this.EnsureAudio();
			if (this._uiAudio != null && this.popupSound != null)
			{
				this._uiAudio.PlayOneShot(this.popupSound, this.popupVolume);
			}
		}

		// Token: 0x06005C0F RID: 23567 RVA: 0x002430EE File Offset: 0x002412EE
		private void PulseScreenFlash()
		{
			this.EnsureOverlay();
			if (this._screenFlash == null)
			{
				return;
			}
			if (this._flashRoutine != null)
			{
				base.StopCoroutine(this._flashRoutine);
			}
			this._flashRoutine = base.StartCoroutine(this.ScreenFlashRoutine());
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x0024312B File Offset: 0x0024132B
		private IEnumerator ScreenFlashRoutine()
		{
			float elapsed = 0f;
			while (elapsed < 0.045f)
			{
				elapsed += NpcPhotoRewardController.RewardUnscaledDeltaTime;
				this.SetScreenFlashAlpha(Mathf.Lerp(0f, 0.3f, Mathf.Clamp01(elapsed / 0.045f)));
				yield return null;
			}
			elapsed = 0f;
			while (elapsed < 0.16f)
			{
				elapsed += NpcPhotoRewardController.RewardUnscaledDeltaTime;
				this.SetScreenFlashAlpha(Mathf.Lerp(0.3f, 0f, Mathf.Clamp01(elapsed / 0.16f)));
				yield return null;
			}
			this.SetScreenFlashAlpha(0f);
			this._flashRoutine = null;
			yield break;
		}

		// Token: 0x06005C11 RID: 23569 RVA: 0x0024313C File Offset: 0x0024133C
		private void SetScreenFlashAlpha(float alpha)
		{
			if (this._screenFlash == null)
			{
				return;
			}
			Color color = this._screenFlash.color;
			color.a = alpha;
			this._screenFlash.color = color;
		}

		// Token: 0x06005C12 RID: 23570 RVA: 0x00243178 File Offset: 0x00241378
		private Vector2 UiWorldToOverlay(Vector3 worldPosition)
		{
			Camera camera = ((this._rootCanvas != null && this._rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay) ? this._rootCanvas.worldCamera : null);
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(camera, worldPosition);
			Vector2 vector2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this._overlayRoot, vector, camera, out vector2);
			return vector2;
		}

		// Token: 0x06005C13 RID: 23571 RVA: 0x002431C8 File Offset: 0x002413C8
		private Image CreateImage(string objectName, Transform parent, Color color)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.GetComponent<RectTransform>().SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06005C14 RID: 23572 RVA: 0x00243228 File Offset: 0x00241428
		private RawImage CreateRawImage(string objectName, Transform parent, Texture texture)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(RawImage)
			});
			gameObject.GetComponent<RectTransform>().SetParent(parent, false);
			RawImage component = gameObject.GetComponent<RawImage>();
			component.texture = texture;
			StreamerCensorImage.Bind(component, false);
			component.color = Color.white;
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x06005C15 RID: 23573 RVA: 0x0024329C File Offset: 0x0024149C
		private Text CreateText(string objectName, Transform parent, string content, int size, FontStyle style, Color color, TextAnchor alignment)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Text)
			});
			gameObject.GetComponent<RectTransform>().SetParent(parent, false);
			Text component = gameObject.GetComponent<Text>();
			component.font = this._font;
			component.fontSize = size;
			component.fontStyle = style;
			component.color = color;
			component.alignment = alignment;
			component.horizontalOverflow = HorizontalWrapMode.Overflow;
			component.verticalOverflow = VerticalWrapMode.Overflow;
			component.supportRichText = true;
			component.raycastTarget = false;
			component.text = content;
			LocalizedLegacyGameText.BindFontOnly(component);
			return component;
		}

		// Token: 0x06005C16 RID: 23574 RVA: 0x00243344 File Offset: 0x00241544
		private static void AddOutline(Text text, Color color, Vector2 distance)
		{
			Outline outline = text.gameObject.AddComponent<Outline>();
			outline.effectColor = color;
			outline.effectDistance = distance;
			outline.useGraphicAlpha = true;
		}

		// Token: 0x06005C17 RID: 23575 RVA: 0x00243365 File Offset: 0x00241565
		private static void Stretch(RectTransform rect, float left, float right, float top, float bottom)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(left, bottom);
			rect.offsetMax = new Vector2(-right, -top);
		}

		// Token: 0x06005C18 RID: 23576 RVA: 0x0018F85B File Offset: 0x0018DA5B
		private static void SetAnchoredRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
		{
			rect.anchorMin = anchorMin;
			rect.anchorMax = anchorMax;
			rect.offsetMin = offsetMin;
			rect.offsetMax = offsetMax;
		}

		// Token: 0x06005C19 RID: 23577 RVA: 0x0024339A File Offset: 0x0024159A
		private IEnumerator WaitUnscaled(float duration)
		{
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += NpcPhotoRewardController.RewardUnscaledDeltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005C1A RID: 23578 RVA: 0x002433AC File Offset: 0x002415AC
		private static bool IsDetectionFlowActive()
		{
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			return instance != null && instance.isActiveAndEnabled && instance.IsEventActive;
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06005C1B RID: 23579 RVA: 0x002433D8 File Offset: 0x002415D8
		private static float RewardUnscaledDeltaTime
		{
			get
			{
				if (!NpcPhotoRewardController.IsDetectionFlowActive())
				{
					return Time.unscaledDeltaTime;
				}
				return 0f;
			}
		}

		// Token: 0x06005C1C RID: 23580 RVA: 0x002433EC File Offset: 0x002415EC
		private static float EaseOutCubic(float t)
		{
			float num = 1f - Mathf.Clamp01(t);
			return 1f - num * num * num;
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x00243411 File Offset: 0x00241611
		private static float EaseInCubic(float t)
		{
			t = Mathf.Clamp01(t);
			return t * t * t;
		}

		// Token: 0x06005C1E RID: 23582 RVA: 0x00243420 File Offset: 0x00241620
		private static float EaseInOutCubic(float t)
		{
			t = Mathf.Clamp01(t);
			if (t >= 0.5f)
			{
				return 1f - Mathf.Pow(-2f * t + 2f, 3f) * 0.5f;
			}
			return 4f * t * t * t;
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x0024346C File Offset: 0x0024166C
		private static float EaseOutBack(float t)
		{
			float num = Mathf.Clamp01(t) - 1f;
			return 1f + 2.70158f * num * num * num + 1.70158f * num * num;
		}

		// Token: 0x06005C20 RID: 23584 RVA: 0x002434A1 File Offset: 0x002416A1
		private static int SaturatingAdd(int left, int right)
		{
			if (left <= 0)
			{
				return Mathf.Max(0, right);
			}
			if (right <= 0)
			{
				return left;
			}
			if (right <= 2147483647 - left)
			{
				return left + right;
			}
			return int.MaxValue;
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x002434C8 File Offset: 0x002416C8
		private static int SaturatingMultiply(int left, int right)
		{
			if (left <= 0 || right <= 0)
			{
				return 0;
			}
			if (left <= 2147483647 / right)
			{
				return left * right;
			}
			return int.MaxValue;
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x002434E7 File Offset: 0x002416E7
		private void ReleasePresentation(NpcPhotoRewardController.PhotoPresentation presentation)
		{
			if (presentation == null || presentation.Texture == null)
			{
				return;
			}
			this.ReleaseTexture(presentation.Texture);
			presentation.Texture = null;
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x00243510 File Offset: 0x00241710
		private void ReleaseInFlightPresentations()
		{
			for (int i = 0; i < this._presentationsInFlight.Length; i++)
			{
				this.ReleasePresentation(this._presentationsInFlight[i]);
				this._presentationsInFlight[i] = null;
			}
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x00243548 File Offset: 0x00241748
		private void ReleaseTexture(RenderTexture texture)
		{
			if (texture == null)
			{
				return;
			}
			if (NpcPhotoRewardController._latestSharedPortrait == texture)
			{
				NpcPhotoRewardController._latestSharedPortrait = null;
				NpcPhotoRewardController._latestSharedPortraitShutterFrame = -1;
			}
			if (texture.width == this.portraitWidth && texture.height == this.portraitHeight && this._portraitTexturePool.Count < 6)
			{
				this._portraitTexturePool.Enqueue(texture);
				return;
			}
			NpcPhotoRewardController.DestroyPortraitTexture(texture);
		}

		// Token: 0x06005C25 RID: 23589 RVA: 0x002435B5 File Offset: 0x002417B5
		private static void DestroyPortraitTexture(RenderTexture texture)
		{
			if (texture == null)
			{
				return;
			}
			if (NpcPhotoRewardController._latestSharedPortrait == texture)
			{
				NpcPhotoRewardController._latestSharedPortrait = null;
				NpcPhotoRewardController._latestSharedPortraitShutterFrame = -1;
			}
			texture.Release();
			Object.Destroy(texture);
		}

		// Token: 0x04005D1F RID: 23839
		private const int MaximumVisibleCards = 1;

		// Token: 0x04005D20 RID: 23840
		private const int MaximumQueuedPresentations = 12;

		// Token: 0x04005D21 RID: 23841
		private const int PrewarmedPortraitCount = 2;

		// Token: 0x04005D22 RID: 23842
		private const int MaximumPooledPortraits = 6;

		// Token: 0x04005D23 RID: 23843
		private const int LightweightRendererIndex = 1;

		// Token: 0x04005D24 RID: 23844
		private const int PortraitDepthBufferBits = 24;

		// Token: 0x04005D25 RID: 23845
		private const int StreamedEnvironmentLayer = 3;

		// Token: 0x04005D26 RID: 23846
		private const float BacklogHoldDuration = 0.45f;

		// Token: 0x04005D27 RID: 23847
		private const float CameraProbeRadius = 0.13f;

		// Token: 0x04005D28 RID: 23848
		private static readonly float[] BeautyYawOffsets = new float[] { 0f, -24f, 26f, 150f, -155f, 180f, -36f, 37f, 118f, -122f };

		// Token: 0x04005D29 RID: 23849
		private static readonly float[] BeautyDistances = new float[] { 1.2f, 0.92f, 1.55f, 1.18f, 1.68f, 1.4f, 0.98f, 1.48f, 1.3f, 1.58f };

		// Token: 0x04005D2A RID: 23850
		private static readonly float[] BeautyFovOffsets = new float[] { 0f, -5f, 8f, 2f, 11f, 5f, -3f, 7f, 4f, 9f };

		// Token: 0x04005D2B RID: 23851
		private static readonly float[] BeautyHeightOffsets = new float[] { 0.01f, 0.1f, -0.08f, 0.16f, -0.11f, 0.06f, -0.04f, 0.12f, -0.06f, 0.09f };

		// Token: 0x04005D2C RID: 23852
		private static readonly string[] PhotoCardLabels = new string[] { "CAUGHT", "BUSTED", "SPOTTED", "SNAPPED", "EXPOSED", "GOTCHA" };

		// Token: 0x04005D2D RID: 23853
		private static NpcPhotoRewardController _globalOwner;

		// Token: 0x04005D2E RID: 23854
		private static RenderTexture _latestSharedPortrait;

		// Token: 0x04005D2F RID: 23855
		private static int _latestSharedPortraitShutterFrame = -1;

		// Token: 0x04005D30 RID: 23856
		private static readonly List<NpcPhotoRewardController> _enabledControllers = new List<NpcPhotoRewardController>();

		// Token: 0x04005D31 RID: 23857
		[Header("Reward")]
		[Min(1f)]
		[SerializeField]
		private int pointsPerPhoto = 500;

		// Token: 0x04005D32 RID: 23858
		[Tooltip("Secondi minimi tra due ricompense foto (popup + punti). Gli NPC continuano a scattare normalmente (animazione/flash), ma gli scatti dentro il cooldown non producono ne' card ne' punti — anti-spam nelle folle di paparazzi.")]
		[Min(0f)]
		[SerializeField]
		private float rewardCooldownSeconds = 10f;

		// Token: 0x04005D33 RID: 23859
		private static float _nextRewardAllowedTime;

		// Token: 0x04005D34 RID: 23860
		[Header("Runtime portrait")]
		[Min(128f)]
		[SerializeField]
		private int portraitWidth = 360;

		// Token: 0x04005D35 RID: 23861
		[Min(128f)]
		[SerializeField]
		private int portraitHeight = 432;

		// Token: 0x04005D36 RID: 23862
		[Range(25f, 60f)]
		[SerializeField]
		private float portraitFieldOfView = 34f;

		// Token: 0x04005D37 RID: 23863
		[Tooltip("Far clip dedicato alle foto. 60 m mantiene leggibili gli interni senza usare il far clip molto piu' costoso della camera gameplay.")]
		[Range(20f, 120f)]
		[SerializeField]
		private float portraitFarClipPlane = 60f;

		// Token: 0x04005D38 RID: 23864
		[Range(20f, 42f)]
		[SerializeField]
		private float maximumFrontAngle = 38f;

		// Token: 0x04005D39 RID: 23865
		[Header("Presentation (unscaled seconds)")]
		[Min(0.05f)]
		[SerializeField]
		private float enterDuration = 0.26f;

		// Token: 0x04005D3A RID: 23866
		[Min(0f)]
		[SerializeField]
		private float holdDuration = 1.85f;

		// Token: 0x04005D3B RID: 23867
		[Min(0.05f)]
		[SerializeField]
		private float exitDuration = 0.36f;

		// Token: 0x04005D3C RID: 23868
		[Header("UI SFX")]
		[SerializeField]
		private AudioClip popupSound;

		// Token: 0x04005D3D RID: 23869
		[Range(0f, 1f)]
		[SerializeField]
		private float popupVolume = 0.72f;

		// Token: 0x04005D3E RID: 23870
		[Header("Optional debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005D3F RID: 23871
		private readonly Queue<NpcPhotoRewardController.PhotoRequest> _captureRequests = new Queue<NpcPhotoRewardController.PhotoRequest>();

		// Token: 0x04005D40 RID: 23872
		private readonly Queue<RenderTexture> _portraitTexturePool = new Queue<RenderTexture>();

		// Token: 0x04005D41 RID: 23873
		private readonly LinkedList<NpcPhotoRewardController.PhotoPresentation> _waitingPresentations = new LinkedList<NpcPhotoRewardController.PhotoPresentation>();

		// Token: 0x04005D42 RID: 23874
		private readonly List<NpcPhotoRewardController.CardRuntime> _liveCards = new List<NpcPhotoRewardController.CardRuntime>();

		// Token: 0x04005D43 RID: 23875
		private readonly NpcPhotoRewardController.PhotoPresentation[] _presentationsInFlight = new NpcPhotoRewardController.PhotoPresentation[1];

		// Token: 0x04005D44 RID: 23876
		private readonly List<Renderer> _subjectRenderers = new List<Renderer>(64);

		// Token: 0x04005D45 RID: 23877
		private readonly WantedPosterCaptureVisibility _posterCaptureVisibility = new WantedPosterCaptureVisibility();

		// Token: 0x04005D46 RID: 23878
		private readonly List<NpcPhotoRewardController.LayerRestore> _subjectLayerRestores = new List<NpcPhotoRewardController.LayerRestore>(64);

		// Token: 0x04005D47 RID: 23879
		private readonly List<NpcPhotoRewardController.RendererRestore> _subjectRendererRestores = new List<NpcPhotoRewardController.RendererRestore>(64);

		// Token: 0x04005D48 RID: 23880
		private readonly RaycastHit[] _cameraProbeHits = new RaycastHit[16];

		// Token: 0x04005D49 RID: 23881
		private readonly bool[] _occupiedSlots = new bool[1];

		// Token: 0x04005D4A RID: 23882
		private RunScoreHUD _scoreHud;

		// Token: 0x04005D4B RID: 23883
		private Canvas _rootCanvas;

		// Token: 0x04005D4C RID: 23884
		private Canvas _overlayCanvas;

		// Token: 0x04005D4D RID: 23885
		private RectTransform _overlayRoot;

		// Token: 0x04005D4E RID: 23886
		private Image _screenFlash;

		// Token: 0x04005D4F RID: 23887
		private AudioSource _uiAudio;

		// Token: 0x04005D50 RID: 23888
		private Camera _portraitCamera;

		// Token: 0x04005D51 RID: 23889
		private Light _portraitKeyLight;

		// Token: 0x04005D52 RID: 23890
		private Font _font;

		// Token: 0x04005D53 RID: 23891
		private Coroutine _captureRoutine;

		// Token: 0x04005D54 RID: 23892
		private Coroutine _presentationPumpRoutine;

		// Token: 0x04005D55 RID: 23893
		private Coroutine _rendererWarmupRoutine;

		// Token: 0x04005D56 RID: 23894
		private Coroutine _flashRoutine;

		// Token: 0x04005D57 RID: 23895
		private int _activeCardCount;

		// Token: 0x04005D58 RID: 23896
		private int _captureSequence;

		// Token: 0x04005D59 RID: 23897
		private int _portraitVariantCursor;

		// Token: 0x04005D5A RID: 23898
		private bool _ownsGlobalSubscription;

		// Token: 0x04005D5B RID: 23899
		private bool _rendererWarmed;

		// Token: 0x02000C62 RID: 3170
		private readonly struct PhotoRequest
		{
			// Token: 0x06005C28 RID: 23592 RVA: 0x002437AC File Offset: 0x002419AC
			public PhotoRequest(Vector3 phonePosition, string sourceName, int shutterFrame)
			{
				this.PhonePosition = phonePosition;
				this.SourceName = sourceName;
				this.ShutterFrame = shutterFrame;
				this.Subject = NpcPhotoRewardController.ResolvePhotoSubject();
			}

			// Token: 0x04005D5C RID: 23900
			public readonly Vector3 PhonePosition;

			// Token: 0x04005D5D RID: 23901
			public readonly string SourceName;

			// Token: 0x04005D5E RID: 23902
			public readonly int ShutterFrame;

			// Token: 0x04005D5F RID: 23903
			public readonly Transform Subject;
		}

		// Token: 0x02000C63 RID: 3171
		private readonly struct LayerRestore
		{
			// Token: 0x06005C29 RID: 23593 RVA: 0x002437CE File Offset: 0x002419CE
			public LayerRestore(GameObject gameObject, int layer)
			{
				this.GameObject = gameObject;
				this.Layer = layer;
			}

			// Token: 0x04005D60 RID: 23904
			public readonly GameObject GameObject;

			// Token: 0x04005D61 RID: 23905
			public readonly int Layer;
		}

		// Token: 0x02000C64 RID: 3172
		private readonly struct RendererRestore
		{
			// Token: 0x06005C2A RID: 23594 RVA: 0x002437DE File Offset: 0x002419DE
			public RendererRestore(Renderer renderer, ShadowCastingMode shadowCastingMode, bool allowOcclusionWhenDynamic)
			{
				this.Renderer = renderer;
				this.ShadowCastingMode = shadowCastingMode;
				this.AllowOcclusionWhenDynamic = allowOcclusionWhenDynamic;
			}

			// Token: 0x04005D62 RID: 23906
			public readonly Renderer Renderer;

			// Token: 0x04005D63 RID: 23907
			public readonly ShadowCastingMode ShadowCastingMode;

			// Token: 0x04005D64 RID: 23908
			public readonly bool AllowOcclusionWhenDynamic;
		}

		// Token: 0x02000C65 RID: 3173
		private sealed class PhotoPresentation
		{
			// Token: 0x04005D65 RID: 23909
			public RenderTexture Texture;

			// Token: 0x04005D66 RID: 23910
			public int Points;

			// Token: 0x04005D67 RID: 23911
			public int ScoreTotal;

			// Token: 0x04005D68 RID: 23912
			public int PhotoCount = 1;

			// Token: 0x04005D69 RID: 23913
			public string Tags;
		}

		// Token: 0x02000C66 RID: 3174
		private readonly struct PhotoTagResult
		{
			// Token: 0x06005C2C RID: 23596 RVA: 0x00243804 File Offset: 0x00241A04
			public PhotoTagResult(bool face, bool boobs, bool ass)
			{
				this.Face = face;
				this.Boobs = boobs;
				this.Ass = ass;
			}

			// Token: 0x17000F0C RID: 3852
			// (get) Token: 0x06005C2D RID: 23597 RVA: 0x0024381B File Offset: 0x00241A1B
			public int BonusPoints
			{
				get
				{
					return (this.Face ? 100 : 0) + (this.Boobs ? 250 : 0) + (this.Ass ? 250 : 0);
				}
			}

			// Token: 0x17000F0D RID: 3853
			// (get) Token: 0x06005C2E RID: 23598 RVA: 0x0024384C File Offset: 0x00241A4C
			public string DisplayText
			{
				get
				{
					string text = "PHOTO";
					if (this.Face)
					{
						text += " · FACE";
					}
					if (this.Boobs)
					{
						text += " · BOOBS";
					}
					if (this.Ass)
					{
						text += " · ASS";
					}
					return text;
				}
			}

			// Token: 0x04005D6A RID: 23914
			public readonly bool Face;

			// Token: 0x04005D6B RID: 23915
			public readonly bool Boobs;

			// Token: 0x04005D6C RID: 23916
			public readonly bool Ass;
		}

		// Token: 0x02000C67 RID: 3175
		private sealed class CardRuntime
		{
			// Token: 0x04005D6D RID: 23917
			public NpcPhotoRewardController.PhotoPresentation Presentation;

			// Token: 0x04005D6E RID: 23918
			public RectTransform Rect;

			// Token: 0x04005D6F RID: 23919
			public CanvasGroup Group;

			// Token: 0x04005D70 RID: 23920
			public RectTransform Badge;

			// Token: 0x04005D71 RID: 23921
			public RawImage Photo;

			// Token: 0x04005D72 RID: 23922
			public int Slot;
		}
	}
}
