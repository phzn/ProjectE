using System;
using Features.Inventory;
using Features.NippleRotor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.EggRotor
{
	// Token: 0x02001235 RID: 4661
	[DefaultExecutionOrder(-800)]
	public sealed class EggRotorRuntimeBootstrap : MonoBehaviour
	{
		// Token: 0x060090B9 RID: 37049 RVA: 0x003B4025 File Offset: 0x003B2225
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			EggRotorRuntimeBootstrap._instance = null;
		}

		// Token: 0x060090BA RID: 37050 RVA: 0x003B402D File Offset: 0x003B222D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Install()
		{
			if (EggRotorRuntimeBootstrap._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[EggRotorRuntime]");
			Object.DontDestroyOnLoad(gameObject);
			EggRotorRuntimeBootstrap._instance = gameObject.AddComponent<EggRotorRuntimeBootstrap>();
		}

		// Token: 0x060090BB RID: 37051 RVA: 0x003B4057 File Offset: 0x003B2257
		private void Awake()
		{
			if (EggRotorRuntimeBootstrap._instance != null && EggRotorRuntimeBootstrap._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			EggRotorRuntimeBootstrap._instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.LoadConfig();
		}

		// Token: 0x060090BC RID: 37052 RVA: 0x003B4096 File Offset: 0x003B2296
		private void OnEnable()
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x060090BD RID: 37053 RVA: 0x003B40A9 File Offset: 0x003B22A9
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}

		// Token: 0x060090BE RID: 37054 RVA: 0x003B40BC File Offset: 0x003B22BC
		private void Start()
		{
			this.EnsurePlayerController();
		}

		// Token: 0x060090BF RID: 37055 RVA: 0x003B40BC File Offset: 0x003B22BC
		private void Update()
		{
			this.EnsurePlayerController();
		}

		// Token: 0x060090C0 RID: 37056 RVA: 0x003B40BC File Offset: 0x003B22BC
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this.EnsurePlayerController();
		}

		// Token: 0x060090C1 RID: 37057 RVA: 0x003B40C4 File Offset: 0x003B22C4
		private void LoadConfig()
		{
			if (this._configLoadAttempted)
			{
				return;
			}
			this._configLoadAttempted = true;
			this._config = Resources.Load<EggRotorConfigSO>("EggRotor/EggRotorConfig");
			if (this._config == null)
			{
				Debug.LogWarning("[EggRotor] Resources/EggRotor/EggRotorConfig not found; the egg rotor stays inactive. Run Tools/Egg Rotor/Build Assets.");
			}
		}

		// Token: 0x060090C2 RID: 37058 RVA: 0x003B4100 File Offset: 0x003B2300
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
			Transform transform2 = transform.Find("EggRotorRuntime");
			if (transform2 == null)
			{
				transform2 = new GameObject("EggRotorRuntime").transform;
				transform2.SetParent(transform, false);
			}
			EggRotorController orAdd = EggRotorRuntimeBootstrap.GetOrAdd<EggRotorController>(transform2.gameObject);
			EggRotorRuntimeBootstrap.GetOrAdd<NippleRotorFaceDriver>(transform2.gameObject);
			EggRotorRuntimeBootstrap.GetOrAdd<EggRotorHud>(transform2.gameObject);
			EggRotorRuntimeBootstrap.GetOrAdd<EggRotorClimax>(transform2.gameObject);
			orAdd.Configure(this._config, instance);
			this._configuredInventory = instance;
			this._controller = orAdd;
		}

		// Token: 0x060090C3 RID: 37059 RVA: 0x003B4208 File Offset: 0x003B2408
		private static T GetOrAdd<T>(GameObject owner) where T : Component
		{
			T component = owner.GetComponent<T>();
			if (!(component != null))
			{
				return owner.AddComponent<T>();
			}
			return component;
		}

		// Token: 0x040094DB RID: 38107
		public const string RuntimeObjectName = "EggRotorRuntime";

		// Token: 0x040094DC RID: 38108
		private static EggRotorRuntimeBootstrap _instance;

		// Token: 0x040094DD RID: 38109
		private EggRotorConfigSO _config;

		// Token: 0x040094DE RID: 38110
		private bool _configLoadAttempted;

		// Token: 0x040094DF RID: 38111
		private PlayerInventoryManager _configuredInventory;

		// Token: 0x040094E0 RID: 38112
		private EggRotorController _controller;
	}
}
