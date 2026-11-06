using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AzureSky;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000BAE RID: 2990
	[DefaultExecutionOrder(-90)]
	public class VehicleLightsManager : MonoBehaviour
	{
		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x060055C2 RID: 21954 RVA: 0x002147A2 File Offset: 0x002129A2
		// (set) Token: 0x060055C3 RID: 21955 RVA: 0x002147A9 File Offset: 0x002129A9
		public static VehicleLightsManager Instance { get; private set; }

		// Token: 0x060055C4 RID: 21956 RVA: 0x002147B1 File Offset: 0x002129B1
		public static void Register(VehicleLightsController c)
		{
			if (c == null)
			{
				return;
			}
			VehicleLightsManager._all.Add(c);
			VehicleLightsManager.EnsureInstance();
		}

		// Token: 0x060055C5 RID: 21957 RVA: 0x002147CE File Offset: 0x002129CE
		public static void Unregister(VehicleLightsController c)
		{
			VehicleLightsManager._all.Remove(c);
			if (VehicleLightsManager.Instance != null)
			{
				VehicleLightsManager.Instance._active.Remove(c);
			}
		}

		// Token: 0x060055C6 RID: 21958 RVA: 0x002147FA File Offset: 0x002129FA
		private static void EnsureInstance()
		{
			if (VehicleLightsManager.Instance != null)
			{
				return;
			}
			VehicleLightsManager.Instance = new GameObject("VehicleLightsManager (auto)").AddComponent<VehicleLightsManager>();
			VehicleLightsManager.Instance._spawnedSelf = true;
		}

		// Token: 0x060055C7 RID: 21959 RVA: 0x00214829 File Offset: 0x00212A29
		private void Awake()
		{
			if (VehicleLightsManager.Instance != null && VehicleLightsManager.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			VehicleLightsManager.Instance = this;
		}

		// Token: 0x060055C8 RID: 21960 RVA: 0x00214857 File Offset: 0x00212A57
		private void Start()
		{
			this.ResolveRefs();
			this.UpdateNightState();
		}

		// Token: 0x060055C9 RID: 21961 RVA: 0x00214868 File Offset: 0x00212A68
		private void ResolveRefs()
		{
			if (this.playerTransform == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					this.playerTransform = gameObject.transform;
				}
			}
			if (this.azureSystem == null)
			{
				this.azureSystem = Object.FindObjectOfType<AzureCoreSystem>();
			}
		}

		// Token: 0x060055CA RID: 21962 RVA: 0x002148BC File Offset: 0x00212ABC
		private void Update()
		{
			if (VehicleSimulationPause.IsPaused)
			{
				return;
			}
			if (Time.time - this._lastUpdateTime < this.updateInterval)
			{
				return;
			}
			this._lastUpdateTime = Time.time;
			if (this.playerTransform == null || this.azureSystem == null)
			{
				this.ResolveRefs();
			}
			if (this.playerTransform == null)
			{
				return;
			}
			this.CheckTimeChange();
			this.ProcessCars();
		}

		// Token: 0x060055CB RID: 21963 RVA: 0x00214930 File Offset: 0x00212B30
		private void CheckTimeChange()
		{
			int num = ((this.azureSystem != null) ? this.azureSystem.timeSystem.hour : DateTime.Now.Hour);
			if (num != this._lastHour)
			{
				this._lastHour = num;
				this.UpdateNightState();
			}
		}

		// Token: 0x060055CC RID: 21964 RVA: 0x00214984 File Offset: 0x00212B84
		private void UpdateNightState()
		{
			float num = ((this.azureSystem != null) ? this.azureSystem.timeSystem.timeOfDay : ((float)DateTime.Now.Hour));
			if (this.nightStartHour > this.nightEndHour)
			{
				this._isNight = num >= this.nightStartHour || num < this.nightEndHour;
				return;
			}
			this._isNight = num >= this.nightStartHour && num < this.nightEndHour;
		}

		// Token: 0x060055CD RID: 21965 RVA: 0x00214A08 File Offset: 0x00212C08
		private void ProcessCars()
		{
			this._toProcess.Clear();
			this._toProcess.AddRange(VehicleLightsManager._all);
			if (this._toProcess.Count == 0)
			{
				if (this._spawnedSelf && this._active.Count == 0)
				{
					VehicleLightsManager.Instance = null;
					Object.Destroy(base.gameObject);
				}
				return;
			}
			Vector3 playerPos = this.playerTransform.position;
			this._toProcess.Sort(delegate(VehicleLightsController a, VehicleLightsController b)
			{
				if (a == null)
				{
					return 1;
				}
				if (b == null)
				{
					return -1;
				}
				float sqrMagnitude2 = (a.Position - playerPos).sqrMagnitude;
				float sqrMagnitude3 = (b.Position - playerPos).sqrMagnitude;
				return sqrMagnitude2.CompareTo(sqrMagnitude3);
			});
			float num = this.maxLightDistance * this.maxLightDistance;
			float num2 = (this.maxLightDistance + this.lightHysteresis) * (this.maxLightDistance + this.lightHysteresis);
			int num3 = 0;
			for (int i = 0; i < this._toProcess.Count; i++)
			{
				VehicleLightsController vehicleLightsController = this._toProcess[i];
				if (!(vehicleLightsController == null))
				{
					float sqrMagnitude = (vehicleLightsController.Position - playerPos).sqrMagnitude;
					bool flag = this._active.Contains(vehicleLightsController);
					bool flag2 = false;
					if (this._isNight && num3 < this.maxActiveCars)
					{
						flag2 = (flag ? (sqrMagnitude <= num2) : (sqrMagnitude <= num));
					}
					else if (this._isNight)
					{
						flag2 = flag && sqrMagnitude <= num2;
					}
					if (flag2)
					{
						if (!flag)
						{
							this._active.Add(vehicleLightsController);
							vehicleLightsController.SetLightsEnabled(true);
						}
						else
						{
							vehicleLightsController.AimHeadlights();
						}
						if (vehicleLightsController.HasPendingFlash)
						{
							vehicleLightsController.TickFlash();
						}
						num3++;
					}
					else if (flag)
					{
						this._active.Remove(vehicleLightsController);
						vehicleLightsController.SetLightsEnabled(false);
					}
				}
			}
			if (this.showDebugInfo && Time.frameCount % 120 == 0)
			{
				Debug.Log(string.Format("[VehicleLightsManager] Night={0} lit={1}/{2} total={3}", new object[]
				{
					this._isNight,
					this._active.Count,
					this.maxActiveCars,
					this._toProcess.Count
				}));
			}
		}

		// Token: 0x060055CE RID: 21966 RVA: 0x00214C2D File Offset: 0x00212E2D
		private void OnDestroy()
		{
			if (VehicleLightsManager.Instance == this)
			{
				VehicleLightsManager.Instance = null;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x060055CF RID: 21967 RVA: 0x00214C42 File Offset: 0x00212E42
		public bool IsNight
		{
			get
			{
				return this._isNight;
			}
		}

		// Token: 0x060055D0 RID: 21968 RVA: 0x00214C4A File Offset: 0x00212E4A
		public void SetBudget(float maxDistance, int maxCars)
		{
			this.maxLightDistance = Mathf.Max(1f, maxDistance);
			this.maxActiveCars = Mathf.Max(0, maxCars);
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x060055D1 RID: 21969 RVA: 0x00214C6A File Offset: 0x00212E6A
		public float MaxLightDistance
		{
			get
			{
				return this.maxLightDistance;
			}
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x060055D2 RID: 21970 RVA: 0x00214C72 File Offset: 0x00212E72
		public int MaxActiveCars
		{
			get
			{
				return this.maxActiveCars;
			}
		}

		// Token: 0x04005626 RID: 22054
		[Header("References (auto-resolved)")]
		[SerializeField]
		private Transform playerTransform;

		// Token: 0x04005627 RID: 22055
		[SerializeField]
		private AzureCoreSystem azureSystem;

		// Token: 0x04005628 RID: 22056
		[Header("Distance culling")]
		[Tooltip("Cars farther than this have their headlights off (project budget).")]
		[SerializeField]
		private float maxLightDistance = 40f;

		// Token: 0x04005629 RID: 22057
		[Tooltip("Extra distance before turning off (anti-flicker hysteresis).")]
		[SerializeField]
		private float lightHysteresis = 6f;

		// Token: 0x0400562A RID: 22058
		[Tooltip("Max cars with headlights on at once.")]
		[SerializeField]
		private int maxActiveCars = 8;

		// Token: 0x0400562B RID: 22059
		[Header("Time (matches StreetLightManager)")]
		[SerializeField]
		private float nightStartHour = 17f;

		// Token: 0x0400562C RID: 22060
		[SerializeField]
		private float nightEndHour = 6f;

		// Token: 0x0400562D RID: 22061
		[Header("Performance")]
		[SerializeField]
		private float updateInterval = 0.3f;

		// Token: 0x0400562E RID: 22062
		[Header("Debug")]
		[SerializeField]
		private bool showDebugInfo;

		// Token: 0x0400562F RID: 22063
		private static readonly HashSet<VehicleLightsController> _all = new HashSet<VehicleLightsController>();

		// Token: 0x04005630 RID: 22064
		private readonly List<VehicleLightsController> _toProcess = new List<VehicleLightsController>();

		// Token: 0x04005631 RID: 22065
		private readonly HashSet<VehicleLightsController> _active = new HashSet<VehicleLightsController>();

		// Token: 0x04005632 RID: 22066
		private float _lastUpdateTime;

		// Token: 0x04005633 RID: 22067
		private bool _isNight;

		// Token: 0x04005634 RID: 22068
		private int _lastHour = -999;

		// Token: 0x04005635 RID: 22069
		private bool _spawnedSelf;
	}
}
