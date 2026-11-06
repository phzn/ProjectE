using System;
using Features.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.NippleRotor
{
	// Token: 0x02000F62 RID: 3938
	[DefaultExecutionOrder(-800)]
	public sealed class NippleRotorRuntimeBootstrap : MonoBehaviour
	{
		// Token: 0x06007698 RID: 30360 RVA: 0x00302C80 File Offset: 0x00300E80
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			NippleRotorRuntimeBootstrap._instance = null;
		}

		// Token: 0x06007699 RID: 30361 RVA: 0x00302C88 File Offset: 0x00300E88
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Install()
		{
			if (NippleRotorRuntimeBootstrap._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[NippleRotorRuntime]");
			Object.DontDestroyOnLoad(gameObject);
			NippleRotorRuntimeBootstrap._instance = gameObject.AddComponent<NippleRotorRuntimeBootstrap>();
		}

		// Token: 0x0600769A RID: 30362 RVA: 0x00302CB2 File Offset: 0x00300EB2
		private void Awake()
		{
			if (NippleRotorRuntimeBootstrap._instance != null && NippleRotorRuntimeBootstrap._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			NippleRotorRuntimeBootstrap._instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.LoadConfig();
		}

		// Token: 0x0600769B RID: 30363 RVA: 0x00302CF1 File Offset: 0x00300EF1
		private void OnEnable()
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x0600769C RID: 30364 RVA: 0x00302D04 File Offset: 0x00300F04
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}

		// Token: 0x0600769D RID: 30365 RVA: 0x00302D17 File Offset: 0x00300F17
		private void Start()
		{
			this.EnsurePlayerController();
		}

		// Token: 0x0600769E RID: 30366 RVA: 0x00302D17 File Offset: 0x00300F17
		private void Update()
		{
			this.EnsurePlayerController();
		}

		// Token: 0x0600769F RID: 30367 RVA: 0x00302D17 File Offset: 0x00300F17
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this.EnsurePlayerController();
		}

		// Token: 0x060076A0 RID: 30368 RVA: 0x00302D1F File Offset: 0x00300F1F
		private void LoadConfig()
		{
			if (this._configLoadAttempted)
			{
				return;
			}
			this._configLoadAttempted = true;
			this._config = Resources.Load<NippleRotorConfigSO>("NippleRotor/NippleRotorConfig");
			if (this._config == null)
			{
				Debug.LogWarning("[NippleRotor] Resources/NippleRotor/NippleRotorConfig not found; the nipple rotor stays inactive. Run Tools/Nipple Rotor/Build Assets.");
			}
		}

		// Token: 0x060076A1 RID: 30369 RVA: 0x00302D5C File Offset: 0x00300F5C
		private void EnsurePlayerController()
		{
			if (this._config == null)
			{
				return;
			}
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (instance == null)
			{
				this._configuredInventory = null;
				this._controller = null;
				return;
			}
			Animator playerAnimator = instance.PlayerAnimator;
			if (this._configuredInventory == instance && this._controller != null && this._controller.PlayerAnimator == playerAnimator && playerAnimator != null)
			{
				return;
			}
			if (playerAnimator == null)
			{
				return;
			}
			Transform transform = playerAnimator.transform;
			Transform transform2 = transform.Find("NippleRotorRuntime");
			if (transform2 == null)
			{
				transform2 = new GameObject("NippleRotorRuntime").transform;
				transform2.SetParent(transform, false);
			}
			NippleRotorController orAdd = NippleRotorRuntimeBootstrap.GetOrAdd<NippleRotorController>(transform2.gameObject);
			NippleRotorRuntimeBootstrap.GetOrAdd<NippleRotorFaceDriver>(transform2.gameObject);
			NippleRotorRuntimeBootstrap.GetOrAdd<NippleRotorHud>(transform2.gameObject);
			orAdd.Configure(this._config, instance);
			this._configuredInventory = instance;
			this._controller = orAdd;
		}

		// Token: 0x060076A2 RID: 30370 RVA: 0x00302E58 File Offset: 0x00301058
		private static T GetOrAdd<T>(GameObject owner) where T : Component
		{
			T component = owner.GetComponent<T>();
			if (!(component != null))
			{
				return owner.AddComponent<T>();
			}
			return component;
		}

		// Token: 0x040079DA RID: 31194
		public const string RuntimeObjectName = "NippleRotorRuntime";

		// Token: 0x040079DB RID: 31195
		private static NippleRotorRuntimeBootstrap _instance;

		// Token: 0x040079DC RID: 31196
		private NippleRotorConfigSO _config;

		// Token: 0x040079DD RID: 31197
		private bool _configLoadAttempted;

		// Token: 0x040079DE RID: 31198
		private PlayerInventoryManager _configuredInventory;

		// Token: 0x040079DF RID: 31199
		private NippleRotorController _controller;
	}
}
