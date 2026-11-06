using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Detection;
using Features.MallHack;
using Features.Score;
using Features.Suspicion;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.NPCs;
using ProjectExhibitionist.PhotoBooth;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.WantedPoster
{
	// Token: 0x02000B77 RID: 2935
	[DisallowMultipleComponent]
	public class WantedPosterPhotoSystem : MonoBehaviour
	{
		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x0600535E RID: 21342 RVA: 0x00205CEF File Offset: 0x00203EEF
		// (set) Token: 0x0600535F RID: 21343 RVA: 0x00205CF6 File Offset: 0x00203EF6
		public static WantedPosterPhotoSystem Instance { get; private set; }

		// Token: 0x06005360 RID: 21344 RVA: 0x00205CFE File Offset: 0x00203EFE
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			WantedPosterPhotoSystem.Instance = null;
			WantedPosterPhotoSystem._posters.Clear();
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06005361 RID: 21345 RVA: 0x00205D10 File Offset: 0x00203F10
		internal static IReadOnlyList<WantedPoster> RegisteredPosters
		{
			get
			{
				return WantedPosterPhotoSystem._posters;
			}
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x00205D18 File Offset: 0x00203F18
		public static void RegisterPoster(WantedPoster poster)
		{
			if (poster == null || WantedPosterPhotoSystem._posters.Contains(poster))
			{
				return;
			}
			poster.CaptureAuthoredMaterial();
			WantedPosterPhotoSystem._posters.Add(poster);
			if (WantedPosterPhotoSystem.Instance != null)
			{
				WantedPosterPhotoSystem.Instance.RequestPosterVisibilityRefresh();
			}
			else
			{
				poster.SetWantedLevel(0);
			}
			if (WantedPosterPhotoSystem.Instance != null && WantedPosterPhotoSystem.Instance._runtimeMaterial != null)
			{
				poster.ApplyRuntimeMaterial(WantedPosterPhotoSystem.Instance._runtimeMaterial);
			}
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x00205D9C File Offset: 0x00203F9C
		public static void UnregisterPoster(WantedPoster poster)
		{
			WantedPosterPhotoSystem._posters.Remove(poster);
			if (WantedPosterPhotoSystem.Instance != null)
			{
				WantedPosterPhotoSystem.Instance.RequestPosterVisibilityRefresh();
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06005364 RID: 21348 RVA: 0x00205DC1 File Offset: 0x00203FC1
		// (set) Token: 0x06005365 RID: 21349 RVA: 0x00205DC9 File Offset: 0x00203FC9
		public bool HasPhoto { get; private set; }

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06005366 RID: 21350 RVA: 0x00205DD2 File Offset: 0x00203FD2
		public float MostWantedNearbyRadius
		{
			get
			{
				return this.mostWantedNearbyRadius;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06005367 RID: 21351 RVA: 0x00205DDA File Offset: 0x00203FDA
		public Material CurrentRuntimeMaterial
		{
			get
			{
				return this._runtimeMaterial;
			}
		}

		// Token: 0x06005368 RID: 21352 RVA: 0x00205DE4 File Offset: 0x00203FE4
		public void ApplyCurrentMaterialTo(GameObject go)
		{
			if (go == null || this._runtimeMaterial == null)
			{
				return;
			}
			Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].sharedMaterial = this._runtimeMaterial;
			}
		}

		// Token: 0x06005369 RID: 21353 RVA: 0x00205E30 File Offset: 0x00204030
		public void ApplyCurrentMaterialSnapshotTo(GameObject go)
		{
			if (go == null || this._runtimeMaterial == null || this._posterRT == null)
			{
				return;
			}
			if (go.GetComponent<WantedPosterMaterialSnapshot>() != null)
			{
				return;
			}
			RenderTexture renderTexture = new RenderTexture(this._posterRT.descriptor)
			{
				name = "WantedPosterHeldSnapshot (runtime)",
				hideFlags = HideFlags.DontSave
			};
			renderTexture.Create();
			Graphics.Blit(this._posterRT, renderTexture);
			Material material = new Material(this._runtimeMaterial)
			{
				name = "WantedPosterHeldSnapshot (runtime)",
				hideFlags = HideFlags.DontSave
			};
			material.SetTexture("_BaseMap", renderTexture);
			material.mainTexture = renderTexture;
			Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>(true);
			Material[] array = new Material[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				array[i] = componentsInChildren[i].sharedMaterial;
				componentsInChildren[i].sharedMaterial = material;
			}
			go.AddComponent<WantedPosterMaterialSnapshot>().Initialize(material, renderTexture, componentsInChildren, array);
		}

		// Token: 0x0600536A RID: 21354 RVA: 0x00205F24 File Offset: 0x00204124
		private void Awake()
		{
			if (WantedPosterPhotoSystem.Instance != null && WantedPosterPhotoSystem.Instance != this)
			{
				if (!this.IsCoreOwned || WantedPosterPhotoSystem.Instance.IsCoreOwned)
				{
					base.enabled = false;
					Object.Destroy(this);
					return;
				}
				WantedPosterPhotoSystem instance = WantedPosterPhotoSystem.Instance;
				WantedPosterPhotoSystem.Instance = this;
				instance.enabled = false;
				Object.Destroy(instance);
			}
			else
			{
				WantedPosterPhotoSystem.Instance = this;
			}
			this._visibilityRandom = new Random(Environment.TickCount ^ base.GetInstanceID());
			if (base.GetComponent<WantedPosterNpcDirector>() == null)
			{
				base.gameObject.AddComponent<WantedPosterNpcDirector>();
			}
		}

		// Token: 0x0600536B RID: 21355 RVA: 0x00205FBD File Offset: 0x002041BD
		private void OnEnable()
		{
			if (WantedPosterPhotoSystem.Instance != this)
			{
				return;
			}
			this.EnsureCamera();
			this.EnsurePhotoRenderTexture();
			SmartphoneFlash.OnPhotoCaptured += this.HandleNpcPhotoCaptured;
			this.BindSuspicionManager();
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x00205FF0 File Offset: 0x002041F0
		private void OnDisable()
		{
			this.RestorePlayerRendererLayers();
			SmartphoneFlash.OnPhotoCaptured -= this.HandleNpcPhotoCaptured;
			this.UnbindSuspicionManager();
			if (this._captureRoutine != null)
			{
				base.StopCoroutine(this._captureRoutine);
				this._captureRoutine = null;
			}
		}

		// Token: 0x0600536D RID: 21357 RVA: 0x0020602C File Offset: 0x0020422C
		private void Update()
		{
			if (this._suspicionManager == null)
			{
				this.BindSuspicionManager();
			}
			if (this._currentWantedLevel > 0 && Time.unscaledTime >= this._nextProximityRefreshTime)
			{
				this._nextProximityRefreshTime = Time.unscaledTime + this.proximityRefreshInterval;
				this.RefreshPosterVisibility();
			}
		}

		// Token: 0x0600536E RID: 21358 RVA: 0x0020607B File Offset: 0x0020427B
		private void LateUpdate()
		{
			if (this._visibilityRefreshPending)
			{
				this.RefreshPosterVisibility();
			}
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x0020608C File Offset: 0x0020428C
		private void OnDestroy()
		{
			if (this._photoRT != null)
			{
				this._photoRT.Release();
				Object.Destroy(this._photoRT);
			}
			if (this._posterRT != null)
			{
				this._posterRT.Release();
				Object.Destroy(this._posterRT);
			}
			if (this._runtimeMaterial != null)
			{
				Object.Destroy(this._runtimeMaterial);
			}
			if (WantedPosterPhotoSystem.Instance == this)
			{
				WantedPosterPhotoSystem.Instance = null;
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06005370 RID: 21360 RVA: 0x00206110 File Offset: 0x00204310
		private bool IsCoreOwned
		{
			get
			{
				return base.gameObject.scene.IsValid() && base.gameObject.scene.name == "_Core";
			}
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x00206151 File Offset: 0x00204351
		private void RequestPosterVisibilityRefresh()
		{
			this._visibilityRefreshPending = true;
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x0020615C File Offset: 0x0020435C
		public void ResetForNewRun()
		{
			if (this._captureRoutine != null)
			{
				base.StopCoroutine(this._captureRoutine);
				this._captureRoutine = null;
			}
			this.HasPhoto = false;
			this._nextInitialCaptureAttempt = 0f;
			this._nextRefreshTime = 0f;
			this._nextProximityRefreshTime = 0f;
			this._currentWantedLevel = 0;
			this._visibilityRefreshPending = false;
			this._posterVisibilityRolls.Clear();
			this._visibilityRandom = new Random(Environment.TickCount ^ base.GetInstanceID());
			WantedPosterPhotoSystem._posters.RemoveAll((WantedPoster poster) => poster == null);
			foreach (WantedPoster wantedPoster in Object.FindObjectsByType<WantedPoster>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				wantedPoster.RestoreAuthoredMaterial();
				wantedPoster.SetWantedLevel(0);
			}
			WantedPosterMaterialSnapshot[] array2 = Object.FindObjectsByType<WantedPosterMaterialSnapshot>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].RestoreAndDispose();
			}
			if (this._runtimeMaterial != null)
			{
				Object.Destroy(this._runtimeMaterial);
				this._runtimeMaterial = null;
			}
			if (this._posterRT != null)
			{
				this._posterRT.Release();
				Object.Destroy(this._posterRT);
				this._posterRT = null;
			}
		}

		// Token: 0x06005373 RID: 21363 RVA: 0x00206294 File Offset: 0x00204494
		private static bool IsGameplayReadyForNpcCapture()
		{
			if (!GameSystemsBootstrapper.IsRunReady(true))
			{
				return false;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			return instance == null || !instance.IsTransitioning;
		}

		// Token: 0x06005374 RID: 21364 RVA: 0x002062C8 File Offset: 0x002044C8
		private void HandleNpcPhotoCaptured(SmartphoneFlash phone)
		{
			if (!WantedPosterPhotoSystem.IsGameplayReadyForNpcCapture())
			{
				return;
			}
			NPCWantedPosterBehaviour npcwantedPosterBehaviour = ((phone != null) ? phone.GetComponentInParent<NPCWantedPosterBehaviour>() : null);
			if (npcwantedPosterBehaviour != null && npcwantedPosterBehaviour.IsPhotographingPoster)
			{
				if (this.debugLogs)
				{
					Debug.Log("[WantedPosterPhoto] Scatto al poster ignorato come nuova foto del player.");
				}
				return;
			}
			NPCPhotoBoothPrintBehaviour npcphotoBoothPrintBehaviour = ((phone != null) ? phone.GetComponentInParent<NPCPhotoBoothPrintBehaviour>() : null);
			if (npcphotoBoothPrintBehaviour != null && npcphotoBoothPrintBehaviour.IsPhotographingPrint)
			{
				if (this.debugLogs)
				{
					Debug.Log("[WantedPosterPhoto] Scatto alla foto Photo Booth ignorato come nuova foto del player.");
				}
				return;
			}
			HackMeNpcObserver hackMeNpcObserver = ((phone != null) ? phone.GetComponentInParent<HackMeNpcObserver>() : null);
			if ((hackMeNpcObserver != null && hackMeNpcObserver.IsBusy) || HackMeAudienceMember.IsWatchingHackedScreen(phone))
			{
				if (this.debugLogs)
				{
					Debug.Log("[WantedPosterPhoto] Scatto al display HackMe ignorato come nuova foto del player.");
				}
				return;
			}
			if (this.HasPhoto && Time.time < this._nextRefreshTime)
			{
				if (this.debugLogs)
				{
					Debug.Log("[WantedPosterPhoto] Scatto ignorato: prossimo aggiornamento tra " + string.Format("{0:F1}s.", this._nextRefreshTime - Time.time));
				}
				return;
			}
			string text = ((phone != null) ? phone.transform.root.name : "NPC");
			this.QueueNpcCapture(text, Time.frameCount);
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x002063FF File Offset: 0x002045FF
		[ContextMenu("Force Capture")]
		public void ForceCapture()
		{
			if (this._captureRoutine != null)
			{
				return;
			}
			this._captureRoutine = base.StartCoroutine(this.ForceCaptureAtEndOfFrame("debug tool"));
		}

		// Token: 0x06005376 RID: 21366 RVA: 0x00206421 File Offset: 0x00204621
		private void QueueNpcCapture(string sourceName, int shutterFrame)
		{
			if (this._captureRoutine != null)
			{
				return;
			}
			if (this.HasPhoto && Time.time < this._nextRefreshTime)
			{
				return;
			}
			this._captureRoutine = base.StartCoroutine(this.CaptureNpcPhoto(sourceName, shutterFrame));
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x00206456 File Offset: 0x00204656
		private IEnumerator CaptureNpcPhoto(string sourceName, int shutterFrame)
		{
			Transform shutterSubject = WantedPosterPhotoSystem.ResolvePlayerTransform();
			try
			{
				yield return new WaitForEndOfFrame();
				yield return null;
				if (!WantedPosterPhotoSystem.IsGameplayReadyForNpcCapture() || shutterSubject == null || shutterSubject != WantedPosterPhotoSystem.ResolvePlayerTransform())
				{
					yield break;
				}
				if (!this.ValidateCaptureAssets())
				{
					yield break;
				}
				this.EnsurePhotoRenderTexture();
				if (NpcPhotoRewardController.TryCopyLatestPortrait(this._photoRT, shutterFrame))
				{
					this.CompleteCapture(sourceName, true);
					yield break;
				}
				yield return new WaitForEndOfFrame();
				if (!WantedPosterPhotoSystem.IsGameplayReadyForNpcCapture() || shutterSubject == null || shutterSubject != WantedPosterPhotoSystem.ResolvePlayerTransform())
				{
					yield break;
				}
				if (this.CaptureWithCamera())
				{
					this.CompleteCapture(sourceName, true);
				}
			}
			finally
			{
				this._captureRoutine = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06005378 RID: 21368 RVA: 0x00206473 File Offset: 0x00204673
		private IEnumerator ForceCaptureAtEndOfFrame(string sourceName)
		{
			try
			{
				yield return new WaitForEndOfFrame();
				if (this.CaptureWithCamera())
				{
					this.CompleteCapture(sourceName, true);
				}
			}
			finally
			{
				this._captureRoutine = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06005379 RID: 21369 RVA: 0x00206489 File Offset: 0x00204689
		private void QueueInitialPortrait()
		{
			if (!base.isActiveAndEnabled || !this.IsInitialPortraitAttemptReady())
			{
				return;
			}
			this._nextInitialCaptureAttempt = Time.unscaledTime + 2f;
			this._captureRoutine = base.StartCoroutine(this.CaptureInitialPortrait());
		}

		// Token: 0x0600537A RID: 21370 RVA: 0x002064BF File Offset: 0x002046BF
		private bool IsInitialPortraitAttemptReady()
		{
			return !this.HasPhoto && this._captureRoutine == null && !PlayerStealthBridge.IsFullyHidden && Time.unscaledTime >= this._nextInitialCaptureAttempt && WantedPosterPhotoSystem.IsGameplayReadyForNpcCapture();
		}

		// Token: 0x0600537B RID: 21371 RVA: 0x002064EC File Offset: 0x002046EC
		private IEnumerator CaptureInitialPortrait()
		{
			try
			{
				yield return new WaitForEndOfFrame();
				if (this.HasPhoto || this._currentWantedLevel <= 0 || PlayerStealthBridge.IsFullyHidden || !WantedPosterPhotoSystem.IsGameplayReadyForNpcCapture())
				{
					yield break;
				}
				if (this.CaptureWithCamera())
				{
					this.CompleteCapture("initial wanted portrait", false);
				}
			}
			finally
			{
				this._captureRoutine = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600537C RID: 21372 RVA: 0x002064FC File Offset: 0x002046FC
		private void BindSuspicionManager()
		{
			SuspicionManager instance = SuspicionManager.Instance;
			if (instance == this._suspicionManager)
			{
				return;
			}
			this.UnbindSuspicionManager();
			this._suspicionManager = instance;
			if (this._suspicionManager != null && this._suspicionManager.OnLevelChanged != null)
			{
				this._suspicionManager.OnLevelChanged.AddListener(new UnityAction<int>(this.HandleWantedLevelChanged));
			}
			this.HandleWantedLevelChanged((this._suspicionManager != null) ? this._suspicionManager.CurrentLevel : 0);
		}

		// Token: 0x0600537D RID: 21373 RVA: 0x00206584 File Offset: 0x00204784
		private void UnbindSuspicionManager()
		{
			if (this._suspicionManager != null && this._suspicionManager.OnLevelChanged != null)
			{
				this._suspicionManager.OnLevelChanged.RemoveListener(new UnityAction<int>(this.HandleWantedLevelChanged));
			}
			this._suspicionManager = null;
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x002065C4 File Offset: 0x002047C4
		private void HandleWantedLevelChanged(int level)
		{
			this._currentWantedLevel = Mathf.Max(0, level);
			this.RefreshPosterVisibility();
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[WantedPoster] Wanted level {0}: ", this._currentWantedLevel) + string.Format("{0}/{1} poster attivi.", this.CountVisibleGatedPosters(), this.CountGatedPosters()));
			}
		}

		// Token: 0x0600537F RID: 21375 RVA: 0x0020662C File Offset: 0x0020482C
		public void RefreshPosterVisibility()
		{
			this._visibilityRefreshPending = false;
			WantedPosterPhotoSystem._posters.RemoveAll((WantedPoster poster) => poster == null);
			foreach (WantedPoster wantedPoster in this._posterVisibilityRolls.Keys.Where<WantedPoster>((WantedPoster poster) => poster == null).ToArray<WantedPoster>())
			{
				this._posterVisibilityRolls.Remove(wantedPoster);
			}
			List<WantedPoster> list = WantedPosterPhotoSystem._posters.Where<WantedPoster>((WantedPoster poster) => poster != null && poster.GateByWantedLevel).ToList<WantedPoster>();
			if (list.Count == 0)
			{
				return;
			}
			List<WantedPoster> list2 = list.Where<WantedPoster>((WantedPoster poster) => this._currentWantedLevel >= Mathf.Max(1, poster.MinimumWantedLevel)).ToList<WantedPoster>();
			HashSet<WantedPoster> hashSet = new HashSet<WantedPoster>(list2);
			foreach (WantedPoster wantedPoster2 in list)
			{
				if (!hashSet.Contains(wantedPoster2))
				{
					wantedPoster2.SetWantedVisibility(false);
				}
			}
			if (list2.Count == 0)
			{
				return;
			}
			if (!this.HasPhoto || this._runtimeMaterial == null)
			{
				foreach (WantedPoster wantedPoster3 in list2)
				{
					wantedPoster3.SetWantedVisibility(false);
				}
				this.QueueInitialPortrait();
				return;
			}
			foreach (WantedPoster wantedPoster4 in list2)
			{
				this.EnsureVisibilityRoll(wantedPoster4);
			}
			float num = ((this._currentWantedLevel <= 0) ? 0f : ((this._currentWantedLevel == 1) ? this.exposedVisibleFraction : this.wantedVisibleFraction));
			num = Mathf.Clamp01(num);
			int num2 = ((this._currentWantedLevel > 0) ? Mathf.Clamp(Mathf.CeilToInt((float)list2.Count * num), 1, list2.Count) : 0);
			HashSet<WantedPoster> hashSet2 = new HashSet<WantedPoster>(list2.OrderBy<WantedPoster, float>((WantedPoster poster) => this._posterVisibilityRolls[poster]).Take<WantedPoster>(num2));
			Transform player = WantedPosterPhotoSystem.ResolvePlayerTransform();
			float num3 = this.mostWantedNearbyRadius * this.mostWantedNearbyRadius;
			bool flag = this._currentWantedLevel >= 3 && player != null;
			if (this._currentWantedLevel > 0 && this._currentWantedLevel < 3 && player != null)
			{
				int num4 = ((this._currentWantedLevel == 1) ? this.exposedMinimumNearby : this.wantedMinimumNearby);
				if (num4 > 0)
				{
					float earlyRadiusSqr = this.earlyLevelNearbyRadius * this.earlyLevelNearbyRadius;
					IEnumerable<WantedPoster> enumerable = list2;
					Func<WantedPoster, bool> func;
					Func<WantedPoster, bool> <>9__5;
					if ((func = <>9__5) == null)
					{
						func = (<>9__5 = (WantedPoster poster) => (poster.NpcInterestPoint - player.position).sqrMagnitude <= earlyRadiusSqr);
					}
					IEnumerable<WantedPoster> enumerable2 = enumerable.Where<WantedPoster>(func);
					Func<WantedPoster, float> <>9__6;
					Func<WantedPoster, float> func2;
					if ((func2 = <>9__6) == null)
					{
						func2 = (<>9__6 = (WantedPoster poster) => this._posterVisibilityRolls[poster]);
					}
					foreach (WantedPoster wantedPoster5 in enumerable2.OrderBy<WantedPoster, float>(func2).Take<WantedPoster>(num4))
					{
						hashSet2.Add(wantedPoster5);
					}
				}
			}
			foreach (WantedPoster wantedPoster6 in list2)
			{
				bool flag2 = hashSet2.Contains(wantedPoster6);
				if (flag)
				{
					flag2 |= (wantedPoster6.NpcInterestPoint - player.position).sqrMagnitude <= num3;
				}
				wantedPoster6.SetWantedVisibility(flag2);
			}
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x00206A5C File Offset: 0x00204C5C
		public int CountVisibleGatedPosters()
		{
			int num = 0;
			foreach (WantedPoster wantedPoster in WantedPosterPhotoSystem._posters)
			{
				if (wantedPoster != null && wantedPoster.GateByWantedLevel && wantedPoster.Renderer != null && wantedPoster.Renderer.enabled)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06005381 RID: 21377 RVA: 0x00206ADC File Offset: 0x00204CDC
		public int CountGatedPosters()
		{
			int num = 0;
			foreach (WantedPoster wantedPoster in WantedPosterPhotoSystem._posters)
			{
				if (wantedPoster != null && wantedPoster.GateByWantedLevel)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x00206B40 File Offset: 0x00204D40
		private void EnsureVisibilityRoll(WantedPoster poster)
		{
			if (this._posterVisibilityRolls.ContainsKey(poster))
			{
				return;
			}
			if (this._visibilityRandom == null)
			{
				this._visibilityRandom = new Random(Environment.TickCount ^ base.GetInstanceID());
			}
			this._posterVisibilityRolls.Add(poster, (float)this._visibilityRandom.NextDouble());
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x00206B94 File Offset: 0x00204D94
		private static Transform ResolvePlayerTransform()
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
				SensoryManager instance = SensoryManager.Instance;
				StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
				if (stealthActor != null)
				{
					return stealthActor.transform;
				}
				Transform transform;
				try
				{
					GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
					transform = ((gameObject != null) ? gameObject.transform : null);
				}
				catch (UnityException)
				{
					transform = null;
				}
				return transform;
			}
		}

		// Token: 0x06005384 RID: 21380 RVA: 0x00206C24 File Offset: 0x00204E24
		private bool ValidateCaptureAssets()
		{
			if (this.posterBaseTexture != null && this.posterTemplateMaterial != null)
			{
				return true;
			}
			Debug.LogError("[WantedPosterPhoto] posterBaseTexture/posterTemplateMaterial non assegnati.");
			return false;
		}

		// Token: 0x06005385 RID: 21381 RVA: 0x00206C50 File Offset: 0x00204E50
		private bool CaptureWithCamera()
		{
			Transform transform = WantedPosterPhotoSystem.ResolvePlayerTransform();
			if (transform == null)
			{
				Debug.LogWarning("[WantedPosterPhoto] Player non disponibile, scatto annullato.");
				return false;
			}
			if (!this.ValidateCaptureAssets())
			{
				return false;
			}
			Vector3 vector = this.ResolveAimPoint(transform);
			Vector3 vector2 = this.PickCameraPosition(transform.transform, vector);
			this.EnsureCamera();
			this.SyncPhotoCameraWithScene();
			this._photoCamera.transform.position = vector2;
			this._photoCamera.transform.rotation = Quaternion.LookRotation(vector - vector2);
			try
			{
				this.RemapPlayerRendererLayers(transform.transform);
				this.RenderPhoto();
			}
			finally
			{
				this.RestorePlayerRendererLayers();
			}
			return true;
		}

		// Token: 0x06005386 RID: 21382 RVA: 0x00206D00 File Offset: 0x00204F00
		private void CompleteCapture(string sourceName, bool startRefreshCooldown = true)
		{
			this.CompositeAndDistribute();
			this.HasPhoto = true;
			float num = Mathf.Max(1f, this.minimumRefreshInterval);
			float num2 = Mathf.Max(num, this.maximumRefreshInterval);
			this._nextRefreshTime = (startRefreshCooldown ? (Time.time + Random.Range(num, num2)) : 0f);
			this.RequestPosterVisibilityRefresh();
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[WantedPosterPhoto] {0}: foto aggiornata su {1} poster. ", sourceName, WantedPosterPhotoSystem._posters.Count) + string.Format("Prossimo aggiornamento tra {0:F1}s.", this._nextRefreshTime - Time.time));
			}
		}

		// Token: 0x06005387 RID: 21383 RVA: 0x00206DA4 File Offset: 0x00204FA4
		private void RemapPlayerRendererLayers(Transform playerRoot)
		{
			this.RestorePlayerRendererLayers();
			if (playerRoot == null)
			{
				return;
			}
			int num = LayerMask.NameToLayer("Player");
			if (num < 0)
			{
				return;
			}
			int num2 = ((this._photoCamera != null) ? this._photoCamera.cullingMask : 0);
			playerRoot.GetComponentsInChildren<Renderer>(false, this._playerRenderers);
			this._posterCaptureVisibility.HideRegisteredPosters(-1);
			for (int i = 0; i < this._playerRenderers.Count; i++)
			{
				Renderer renderer = this._playerRenderers[i];
				if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy && !this._posterCaptureVisibility.HideIfPoster(renderer))
				{
					if (renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly || renderer.allowOcclusionWhenDynamic)
					{
						this._playerRendererRestores.Add(new WantedPosterPhotoSystem.RendererRestore(renderer, renderer.shadowCastingMode, renderer.allowOcclusionWhenDynamic));
						if (renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
						{
							renderer.shadowCastingMode = ShadowCastingMode.On;
						}
						renderer.allowOcclusionWhenDynamic = false;
					}
					GameObject gameObject = renderer.gameObject;
					if ((num2 & (1 << gameObject.layer)) == 0)
					{
						this._playerLayerRestores.Add(new WantedPosterPhotoSystem.LayerRestore(gameObject, gameObject.layer));
						gameObject.layer = num;
					}
				}
			}
		}

		// Token: 0x06005388 RID: 21384 RVA: 0x00206EE4 File Offset: 0x002050E4
		private void RestorePlayerRendererLayers()
		{
			this._posterCaptureVisibility.Restore();
			for (int i = this._playerRendererRestores.Count - 1; i >= 0; i--)
			{
				WantedPosterPhotoSystem.RendererRestore rendererRestore = this._playerRendererRestores[i];
				if (!(rendererRestore.Renderer == null))
				{
					rendererRestore.Renderer.shadowCastingMode = rendererRestore.ShadowCastingMode;
					rendererRestore.Renderer.allowOcclusionWhenDynamic = rendererRestore.AllowOcclusionWhenDynamic;
				}
			}
			this._playerRendererRestores.Clear();
			for (int j = this._playerLayerRestores.Count - 1; j >= 0; j--)
			{
				WantedPosterPhotoSystem.LayerRestore layerRestore = this._playerLayerRestores[j];
				if (layerRestore.GameObject != null)
				{
					layerRestore.GameObject.layer = layerRestore.Layer;
				}
			}
			this._playerLayerRestores.Clear();
			this._playerRenderers.Clear();
		}

		// Token: 0x06005389 RID: 21385 RVA: 0x00206FB6 File Offset: 0x002051B6
		private void OnValidate()
		{
			this.minimumRefreshInterval = Mathf.Max(1f, this.minimumRefreshInterval);
			this.maximumRefreshInterval = Mathf.Max(this.minimumRefreshInterval, this.maximumRefreshInterval);
		}

		// Token: 0x0600538A RID: 21386 RVA: 0x00206FE8 File Offset: 0x002051E8
		private Vector3 ResolveAimPoint(Transform player)
		{
			Animator componentInChildren = player.GetComponentInChildren<Animator>();
			if (componentInChildren != null && componentInChildren.isHuman)
			{
				Transform boneTransform = componentInChildren.GetBoneTransform(HumanBodyBones.Head);
				if (boneTransform != null)
				{
					return boneTransform.position - Vector3.up * 0.12f;
				}
			}
			StealthActor component = player.GetComponent<StealthActor>();
			if (!(component != null))
			{
				return player.position + Vector3.up * 1.5f;
			}
			return component.Position;
		}

		// Token: 0x0600538B RID: 21387 RVA: 0x0020706C File Offset: 0x0020526C
		private Vector3 PickCameraPosition(Transform playerRoot, Vector3 aimPoint)
		{
			Vector3 vector = playerRoot.forward;
			vector.y = 0f;
			vector = ((vector.sqrMagnitude > 0.001f) ? vector.normalized : Vector3.forward);
			float num = this.perspectiveArcDegrees * 0.5f;
			for (int i = 0; i < Mathf.Max(1, this.maxAngleAttempts); i++)
			{
				float num2 = Random.Range(-num, num);
				Vector3 vector2 = Quaternion.Euler(0f, num2, 0f) * vector;
				Vector3 vector3 = aimPoint + vector2 * this.photoDistance;
				if (!Physics.Linecast(aimPoint, vector3, this.obstructionMask, QueryTriggerInteraction.Ignore))
				{
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[WantedPosterPhoto] Prospettiva scelta: {0:F0}° (tentativo {1}).", num2, i + 1));
					}
					return vector3;
				}
			}
			if (this.debugLogs)
			{
				Debug.Log("[WantedPosterPhoto] Tutti gli angoli ostruiti, fallback frontale ravvicinato.");
			}
			return aimPoint + vector * (this.photoDistance * 0.5f);
		}

		// Token: 0x0600538C RID: 21388 RVA: 0x00207170 File Offset: 0x00205370
		private void EnsureCamera()
		{
			if (this._photoCamera != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("WantedPosterPhotoCamera");
			gameObject.transform.SetParent(base.transform, false);
			this._photoCamera = gameObject.AddComponent<Camera>();
			this._photoCamera.enabled = false;
			this._photoCamera.fieldOfView = this.photoFov;
			this._photoCamera.clearFlags = CameraClearFlags.Skybox;
			this._photoCamera.backgroundColor = new Color(0.07f, 0.09f, 0.14f, 1f);
			this._photoCamera.nearClipPlane = 0.08f;
			this._photoCamera.farClipPlane = 14f;
			this._photoCamera.cullingMask = WantedPosterPhotoSystem.BuildPhotoLayerMask(Camera.main);
			this._photoCamera.allowHDR = false;
			this._photoCamera.allowMSAA = false;
			this._photoCamera.allowDynamicResolution = false;
			this._photoCamera.useOcclusionCulling = false;
			this._photoCamera.depthTextureMode = DepthTextureMode.None;
			UniversalAdditionalCameraData universalAdditionalCameraData = this._photoCamera.GetUniversalAdditionalCameraData();
			if (universalAdditionalCameraData != null)
			{
				universalAdditionalCameraData.renderType = CameraRenderType.Base;
				universalAdditionalCameraData.SetRenderer(1);
				universalAdditionalCameraData.renderPostProcessing = false;
				universalAdditionalCameraData.renderShadows = false;
				universalAdditionalCameraData.requiresDepthOption = CameraOverrideOption.Off;
				universalAdditionalCameraData.requiresColorOption = CameraOverrideOption.Off;
				universalAdditionalCameraData.antialiasing = AntialiasingMode.None;
				universalAdditionalCameraData.volumeLayerMask = 0;
				universalAdditionalCameraData.stopNaN = false;
				universalAdditionalCameraData.dithering = false;
				universalAdditionalCameraData.allowXRRendering = false;
			}
		}

		// Token: 0x0600538D RID: 21389 RVA: 0x002072DC File Offset: 0x002054DC
		private void SyncPhotoCameraWithScene()
		{
			if (this._photoCamera == null)
			{
				return;
			}
			Camera main = Camera.main;
			this._photoCamera.farClipPlane = 14f;
			this._photoCamera.cullingMask = WantedPosterPhotoSystem.BuildPhotoLayerMask(main);
			if (main != null && main.clearFlags == CameraClearFlags.Color)
			{
				this._photoCamera.clearFlags = CameraClearFlags.Color;
				Color backgroundColor = main.backgroundColor;
				backgroundColor.a = 1f;
				this._photoCamera.backgroundColor = backgroundColor;
				return;
			}
			if (RenderSettings.skybox != null)
			{
				this._photoCamera.clearFlags = CameraClearFlags.Skybox;
				return;
			}
			this._photoCamera.clearFlags = CameraClearFlags.Color;
			this._photoCamera.backgroundColor = new Color(0.07f, 0.09f, 0.14f, 1f);
		}

		// Token: 0x0600538E RID: 21390 RVA: 0x002073A8 File Offset: 0x002055A8
		private static int BuildPhotoLayerMask(Camera sceneCamera)
		{
			int num = ((sceneCamera != null) ? sceneCamera.cullingMask : 0);
			WantedPosterPhotoSystem.RemoveLayerFromMask(ref num, "UI");
			WantedPosterPhotoSystem.RemoveLayerFromMask(ref num, "Triggers");
			WantedPosterPhotoSystem.RemoveLayerFromMask(ref num, "TVCameraOnly");
			WantedPosterPhotoSystem.RemoveLayerFromMask(ref num, "StadiumScreenOnly");
			WantedPosterPhotoSystem.RemoveLayerFromMask(ref num, "StadiumMonitorVCam");
			WantedPosterPhotoSystem.RemoveLayerFromMask(ref num, "CharacterPreview");
			WantedPosterPhotoSystem.RemoveLayerFromMask(ref num, "VisionBlocker");
			WantedPosterPhotoSystem.RemoveLayerFromMask(ref num, "PostProcess");
			num |= 8;
			WantedPosterPhotoSystem.AddLayerToMask(ref num, "Default");
			WantedPosterPhotoSystem.AddLayerToMask(ref num, "ACSCulling");
			WantedPosterPhotoSystem.AddLayerToMask(ref num, "Floor");
			WantedPosterPhotoSystem.AddLayerToMask(ref num, "Interactable");
			WantedPosterPhotoSystem.AddLayerToMask(ref num, "InteractableTerrain");
			WantedPosterPhotoSystem.AddLayerToMask(ref num, "Glass");
			WantedPosterPhotoSystem.AddLayerToMask(ref num, "Player");
			WantedPosterPhotoSystem.AddLayerToMask(ref num, "BodyPart");
			WantedPosterPhotoSystem.AddLayerToMask(ref num, "PlayerHead");
			WantedPosterPhotoSystem.AddLayerToMask(ref num, "PlayerClothing");
			return num;
		}

		// Token: 0x0600538F RID: 21391 RVA: 0x002074A8 File Offset: 0x002056A8
		private static void AddLayerToMask(ref int mask, string layerName)
		{
			int num = LayerMask.NameToLayer(layerName);
			if (num >= 0)
			{
				mask |= 1 << num;
			}
		}

		// Token: 0x06005390 RID: 21392 RVA: 0x002074CC File Offset: 0x002056CC
		private static void RemoveLayerFromMask(ref int mask, string layerName)
		{
			int num = LayerMask.NameToLayer(layerName);
			if (num >= 0)
			{
				mask &= ~(1 << num);
			}
		}

		// Token: 0x06005391 RID: 21393 RVA: 0x002074F0 File Offset: 0x002056F0
		private void EnsurePhotoRenderTexture()
		{
			if (this._photoRT == null)
			{
				this._photoRT = new RenderTexture(this.rectW, this.rectH, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
				{
					name = "WantedPosterPhoto (runtime)"
				};
				this._photoRT.Create();
				return;
			}
			if (!this._photoRT.IsCreated())
			{
				this._photoRT.Create();
			}
		}

		// Token: 0x06005392 RID: 21394 RVA: 0x00207558 File Offset: 0x00205758
		private void RenderPhoto()
		{
			this.EnsurePhotoRenderTexture();
			this._photoCamera.targetTexture = this._photoRT;
			try
			{
				StillPhotoRenderUtility.Render(this._photoCamera, this._photoRT);
			}
			finally
			{
				this._photoCamera.targetTexture = null;
			}
		}

		// Token: 0x06005393 RID: 21395 RVA: 0x002075AC File Offset: 0x002057AC
		private void CompositeAndDistribute()
		{
			if (this._posterRT == null)
			{
				this._posterRT = new RenderTexture(this.posterBaseTexture.width, this.posterBaseTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
				{
					name = "WantedPosterComposite (runtime)"
				};
				this._posterRT.Create();
				Graphics.Blit(this.posterBaseTexture, this._posterRT);
			}
			int num = this._posterRT.height - this.rectY - this.rectH;
			Graphics.CopyTexture(this._photoRT, 0, 0, 0, 0, this.rectW, this.rectH, this._posterRT, 0, 0, this.rectX, num);
			if (this._runtimeMaterial == null)
			{
				this._runtimeMaterial = new Material(this.posterTemplateMaterial)
				{
					name = "WantedPoster (runtime)"
				};
				this._runtimeMaterial.SetTexture("_BaseMap", this._posterRT);
				this._runtimeMaterial.mainTexture = this._posterRT;
			}
			foreach (WantedPoster wantedPoster in WantedPosterPhotoSystem._posters)
			{
				if (wantedPoster != null && wantedPoster.Renderer.sharedMaterial != this._runtimeMaterial)
				{
					wantedPoster.ApplyRuntimeMaterial(this._runtimeMaterial);
				}
			}
		}

		// Token: 0x040053E1 RID: 21473
		private const int StreamedEnvironmentLayer = 3;

		// Token: 0x040053E2 RID: 21474
		private const float PhotoFarClip = 14f;

		// Token: 0x040053E4 RID: 21476
		private static readonly List<WantedPoster> _posters = new List<WantedPoster>();

		// Token: 0x040053E5 RID: 21477
		[Header("Poster Assets (originali, mai modificati)")]
		[Tooltip("Texture base del poster (quella con il riquadro vuoto). Asset originale third-party.")]
		[SerializeField]
		private Texture2D posterBaseTexture;

		// Token: 0x040053E6 RID: 21478
		[Tooltip("Materiale originale del poster, usato come template per la copia runtime.")]
		[SerializeField]
		private Material posterTemplateMaterial;

		// Token: 0x040053E7 RID: 21479
		[Header("Riquadro foto (pixel texture, origine in alto a sinistra)")]
		[Tooltip("Rettangolo del riquadro vuoto sulla texture del poster. Calibrato sulla texture 1024x1024.")]
		[SerializeField]
		private int rectX = 70;

		// Token: 0x040053E8 RID: 21480
		[SerializeField]
		private int rectY = 245;

		// Token: 0x040053E9 RID: 21481
		[SerializeField]
		private int rectW = 500;

		// Token: 0x040053EA RID: 21482
		[SerializeField]
		private int rectH = 575;

		// Token: 0x040053EB RID: 21483
		[Header("Scatto")]
		[Tooltip("FOV verticale della camera che scatta la foto.")]
		[SerializeField]
		private float photoFov = 35f;

		// Token: 0x040053EC RID: 21484
		[Tooltip("Distanza della camera dal punto mirato (testa del player). Valori bassi = primo piano: a 1.0m con FOV 35 il busto riempie quasi tutta la foto.")]
		[SerializeField]
		private float photoDistance = 1f;

		// Token: 0x040053ED RID: 21485
		[Tooltip("Arco (gradi) entro cui viene scelta la prospettiva random, centrato sul davanti del player. 60 = sempre frontale con leggera variazione (±30°); 180 = da frontale a pienamente laterale. Mai da dietro in ogni caso.")]
		[Range(10f, 180f)]
		[SerializeField]
		private float perspectiveArcDegrees = 60f;

		// Token: 0x040053EE RID: 21486
		[Tooltip("Layer considerati ostacoli: se tra player e posizione candidata della camera c'è qualcosa su questi layer, si riprova con un altro angolo.")]
		[SerializeField]
		private LayerMask obstructionMask = 1;

		// Token: 0x040053EF RID: 21487
		[Tooltip("Tentativi massimi di trovare un angolo non ostruito prima del fallback frontale ravvicinato.")]
		[SerializeField]
		private int maxAngleAttempts = 8;

		// Token: 0x040053F0 RID: 21488
		[Header("Trigger")]
		[Tooltip("Intervallo minimo (s) prima che una nuova foto NPC possa aggiornare i poster.")]
		[Min(1f)]
		[SerializeField]
		private float minimumRefreshInterval = 60f;

		// Token: 0x040053F1 RID: 21489
		[Tooltip("Intervallo massimo (s). Dopo ogni aggiornamento viene scelta una durata casuale tra minimo e massimo, cosi' i poster non cambiano tutti a ritmo meccanico.")]
		[Min(1f)]
		[SerializeField]
		private float maximumRefreshInterval = 120f;

		// Token: 0x040053F2 RID: 21490
		[Header("Distribuzione per Wanted Level")]
		[Tooltip("Quota casuale dei poster attivi a livello Exposed.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float exposedVisibleFraction = 0.2f;

		// Token: 0x040053F3 RID: 21491
		[Tooltip("Quota casuale dei poster attivi a livello Wanted. I poster scelti a Exposed rimangono inclusi perche' ogni poster usa lo stesso roll per tutta la run.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float wantedVisibleFraction = 0.65f;

		// Token: 0x040053F4 RID: 21492
		[Tooltip("Radius used to guarantee a small, findable subset around the player at Exposed and Wanted. The global random distribution still applies outside this radius.")]
		[Min(1f)]
		[SerializeField]
		private float earlyLevelNearbyRadius = 55f;

		// Token: 0x040053F5 RID: 21493
		[Tooltip("Minimum authored posters enabled near the player at Exposed, when available.")]
		[Min(0f)]
		[SerializeField]
		private int exposedMinimumNearby = 1;

		// Token: 0x040053F6 RID: 21494
		[Tooltip("Minimum authored posters enabled near the player at Wanted, when available.")]
		[Min(0f)]
		[SerializeField]
		private int wantedMinimumNearby = 3;

		// Token: 0x040053F7 RID: 21495
		[Tooltip("Dal livello Most Wanted in poi tutti i poster entro questo raggio dal player sono attivi; fuori dal raggio resta attiva la quota Wanted.")]
		[Min(1f)]
		[SerializeField]
		private float mostWantedNearbyRadius = 70f;

		// Token: 0x040053F8 RID: 21496
		[Tooltip("Intervallo tra i controlli di prossimita' mentre il player e' Most Wanted.")]
		[Min(0.1f)]
		[SerializeField]
		private float proximityRefreshInterval = 1f;

		// Token: 0x040053F9 RID: 21497
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040053FB RID: 21499
		private Camera _photoCamera;

		// Token: 0x040053FC RID: 21500
		private RenderTexture _photoRT;

		// Token: 0x040053FD RID: 21501
		private RenderTexture _posterRT;

		// Token: 0x040053FE RID: 21502
		private Material _runtimeMaterial;

		// Token: 0x040053FF RID: 21503
		private float _nextRefreshTime;

		// Token: 0x04005400 RID: 21504
		private Coroutine _captureRoutine;

		// Token: 0x04005401 RID: 21505
		private float _nextInitialCaptureAttempt;

		// Token: 0x04005402 RID: 21506
		private readonly List<Renderer> _playerRenderers = new List<Renderer>(64);

		// Token: 0x04005403 RID: 21507
		private readonly WantedPosterCaptureVisibility _posterCaptureVisibility = new WantedPosterCaptureVisibility();

		// Token: 0x04005404 RID: 21508
		private readonly List<WantedPosterPhotoSystem.LayerRestore> _playerLayerRestores = new List<WantedPosterPhotoSystem.LayerRestore>(64);

		// Token: 0x04005405 RID: 21509
		private readonly List<WantedPosterPhotoSystem.RendererRestore> _playerRendererRestores = new List<WantedPosterPhotoSystem.RendererRestore>(64);

		// Token: 0x04005406 RID: 21510
		private SuspicionManager _suspicionManager;

		// Token: 0x04005407 RID: 21511
		private int _currentWantedLevel;

		// Token: 0x04005408 RID: 21512
		private readonly Dictionary<WantedPoster, float> _posterVisibilityRolls = new Dictionary<WantedPoster, float>();

		// Token: 0x04005409 RID: 21513
		private Random _visibilityRandom;

		// Token: 0x0400540A RID: 21514
		private float _nextProximityRefreshTime;

		// Token: 0x0400540B RID: 21515
		private bool _visibilityRefreshPending;

		// Token: 0x02000B78 RID: 2936
		private readonly struct LayerRestore
		{
			// Token: 0x06005396 RID: 21398 RVA: 0x00207828 File Offset: 0x00205A28
			public LayerRestore(GameObject gameObject, int layer)
			{
				this.GameObject = gameObject;
				this.Layer = layer;
			}

			// Token: 0x0400540C RID: 21516
			public readonly GameObject GameObject;

			// Token: 0x0400540D RID: 21517
			public readonly int Layer;
		}

		// Token: 0x02000B79 RID: 2937
		private readonly struct RendererRestore
		{
			// Token: 0x06005397 RID: 21399 RVA: 0x00207838 File Offset: 0x00205A38
			public RendererRestore(Renderer renderer, ShadowCastingMode shadowCastingMode, bool allowOcclusionWhenDynamic)
			{
				this.Renderer = renderer;
				this.ShadowCastingMode = shadowCastingMode;
				this.AllowOcclusionWhenDynamic = allowOcclusionWhenDynamic;
			}

			// Token: 0x0400540E RID: 21518
			public readonly Renderer Renderer;

			// Token: 0x0400540F RID: 21519
			public readonly ShadowCastingMode ShadowCastingMode;

			// Token: 0x04005410 RID: 21520
			public readonly bool AllowOcclusionWhenDynamic;
		}
	}
}
