using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000005 RID: 5
	[ExecuteInEditMode]
	[AddComponentMenu("Azure[Sky] Dynamic Skybox/Azure Core System")]
	public sealed class AzureCoreSystem : MonoBehaviour
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000022FE File Offset: 0x000004FE
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002306 File Offset: 0x00000506
		public AzureTimeSystem timeSystem
		{
			get
			{
				return this.m_timeSystem;
			}
			set
			{
				this.m_timeSystem = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000230F File Offset: 0x0000050F
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002317 File Offset: 0x00000517
		public AzureWeatherSystem weatherSystem
		{
			get
			{
				return this.m_weatherSystem;
			}
			set
			{
				this.m_weatherSystem = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002320 File Offset: 0x00000520
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002328 File Offset: 0x00000528
		public AzureEventSystem eventSystem
		{
			get
			{
				return this.m_eventSystem;
			}
			set
			{
				this.m_eventSystem = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002331 File Offset: 0x00000531
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002339 File Offset: 0x00000539
		public AzureCoreUpdateMode coreUpdateMode
		{
			get
			{
				return this.m_coreUpdateMode;
			}
			set
			{
				this.m_coreUpdateMode = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002342 File Offset: 0x00000542
		public float timeSinceLastUpdate
		{
			get
			{
				return this.m_timeSinceLastUpdate;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000234A File Offset: 0x0000054A
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002352 File Offset: 0x00000552
		public float refreshRate
		{
			get
			{
				return this.m_refreshRate;
			}
			set
			{
				this.m_refreshRate = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000235B File Offset: 0x0000055B
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00002363 File Offset: 0x00000563
		public UnityEvent onCoreUpdateEvent
		{
			get
			{
				return this.m_onCoreUpdateEvent;
			}
			set
			{
				this.m_onCoreUpdateEvent = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000236C File Offset: 0x0000056C
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00002374 File Offset: 0x00000574
		public ReflectionProbe reflectionProbe
		{
			get
			{
				return this.m_reflectionProbe;
			}
			set
			{
				this.m_reflectionProbe = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000237D File Offset: 0x0000057D
		// (set) Token: 0x06000027 RID: 39 RVA: 0x00002385 File Offset: 0x00000585
		public float environmentRefreshRate
		{
			get
			{
				return this.m_environmentRefreshRate;
			}
			set
			{
				this.m_environmentRefreshRate = Mathf.Max(0.1f, value);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002398 File Offset: 0x00000598
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000023A0 File Offset: 0x000005A0
		public List<AzureFollowTarget> followTargetList
		{
			get
			{
				return this.m_followTargetList;
			}
			set
			{
				this.m_followTargetList = value;
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000023A9 File Offset: 0x000005A9
		private void Reset()
		{
			this.m_timeSystem.UpdateCalendar();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000023B8 File Offset: 0x000005B8
		private void Start()
		{
			this.m_timeSystem.Start();
			this.m_weatherSystem.evaluationTime = this.m_timeSystem.evaluationTime;
			this.m_weatherSystem.Start();
			this.UpdateFollowTargets();
			UnityEvent onCoreUpdateEvent = this.m_onCoreUpdateEvent;
			if (onCoreUpdateEvent == null)
			{
				return;
			}
			onCoreUpdateEvent.Invoke();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002407 File Offset: 0x00000607
		private void OnEnable()
		{
			this.m_eventSystem.RegisterEvents();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002414 File Offset: 0x00000614
		private void OnDisable()
		{
			this.m_eventSystem.UnregisterEvents();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002424 File Offset: 0x00000624
		private void Update()
		{
			if (Application.isPlaying)
			{
				this.m_timeSystem.UpdateTimeOfDay();
				this.m_weatherSystem.evaluationTime = this.m_timeSystem.evaluationTime;
				if (this.m_coreUpdateMode == AzureCoreUpdateMode.EveryFrame)
				{
					this.m_timeSystem.UpdateCelestialBodies();
					this.m_weatherSystem.UpdateWeatherSystem();
					this.m_weatherSystem.OverrideTargetProperties();
					UnityEvent onCoreUpdateEvent = this.m_onCoreUpdateEvent;
					if (onCoreUpdateEvent != null)
					{
						onCoreUpdateEvent.Invoke();
					}
				}
				else
				{
					this.m_timeSinceLastUpdate += Time.deltaTime;
					if (this.m_timeSinceLastUpdate >= this.m_refreshRate)
					{
						this.m_timeSystem.UpdateCelestialBodies();
						this.m_weatherSystem.UpdateWeatherSystem();
						this.m_weatherSystem.OverrideTargetProperties();
						UnityEvent onCoreUpdateEvent2 = this.m_onCoreUpdateEvent;
						if (onCoreUpdateEvent2 != null)
						{
							onCoreUpdateEvent2.Invoke();
						}
						this.m_timeSinceLastUpdate = 0f;
					}
				}
				this.UpdateFollowTargets();
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000024FC File Offset: 0x000006FC
		private void UpdateFollowTargets()
		{
			if (this.m_followTargetList.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < this.m_followTargetList.Count; i++)
			{
				if (this.m_followTargetList[i].follower && this.m_followTargetList[i].target && this.m_followTargetList[i].follower.position != this.m_followTargetList[i].target.position)
				{
					this.m_followTargetList[i].follower.position = this.m_followTargetList[i].target.position;
				}
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000025C4 File Offset: 0x000007C4
		public void UpdateReflectionProbe()
		{
			if (this.m_reflectionProbe && this.m_reflectionProbe.refreshMode == ReflectionProbeRefreshMode.ViaScripting)
			{
				bool flag = this.m_reflectionProbe.cullingMask != 0;
				if (this.ShouldRefreshEnvironment(ref this.m_nextReflectionProbeRefreshTime, ref this.m_lastReflectionProbeEvaluationTime, ref this.m_lastReflectionProbeWeatherPreset, ref this.m_lastReflectionProbeWeatherIndex, flag))
				{
					this.m_reflectionProbe.RenderProbe();
				}
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002628 File Offset: 0x00000828
		public void UpdateDynamicGI()
		{
			if (this.ShouldRefreshEnvironment(ref this.m_nextDynamicGIRefreshTime, ref this.m_lastDynamicGIEvaluationTime, ref this.m_lastDynamicGIWeatherPreset, ref this.m_lastDynamicGIWeatherIndex, false))
			{
				DynamicGI.UpdateEnvironment();
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002650 File Offset: 0x00000850
		private bool ShouldRefreshEnvironment(ref float nextRefreshTime, ref float lastEvaluationTime, ref AzureWeatherPreset lastWeatherPreset, ref int lastWeatherIndex, bool capturesSceneContent)
		{
			float num = ((this.m_timeSystem != null) ? this.m_timeSystem.evaluationTime : 0f);
			AzureWeatherPreset azureWeatherPreset = ((this.m_weatherSystem != null) ? this.m_weatherSystem.currentWeatherPreset : null);
			int num2 = ((this.m_weatherSystem != null) ? this.m_weatherSystem.globalWeatherIndex : (-1));
			bool flag = float.IsNaN(lastEvaluationTime);
			float num3 = (flag ? float.PositiveInfinity : (Mathf.Abs(Mathf.DeltaAngle(lastEvaluationTime * 15f, num * 15f)) / 15f));
			bool flag2 = azureWeatherPreset != lastWeatherPreset || num2 != lastWeatherIndex;
			bool flag3 = flag || flag2 || num3 > 0.0001f;
			bool flag4 = capturesSceneContent || (this.m_timeSystem != null && this.m_timeSystem.dayLength > 0f) || (this.m_weatherSystem != null && (this.m_weatherSystem.isWeatherChanging || (this.m_weatherSystem.weatherZoneList != null && this.m_weatherSystem.weatherZoneList.Count > 0)));
			if (!flag3 && !flag4)
			{
				return false;
			}
			float unscaledTime = Time.unscaledTime;
			bool flag5 = !flag && num3 >= 0.25f;
			bool flag6 = flag || flag2 || flag5;
			if (Application.isPlaying && !flag6 && unscaledTime < nextRefreshTime)
			{
				return false;
			}
			nextRefreshTime = unscaledTime + Mathf.Max(0.1f, this.m_environmentRefreshRate);
			lastEvaluationTime = num;
			lastWeatherPreset = azureWeatherPreset;
			lastWeatherIndex = num2;
			return true;
		}

		// Token: 0x0400000A RID: 10
		[SerializeField]
		private AzureTimeSystem m_timeSystem = new AzureTimeSystem();

		// Token: 0x0400000B RID: 11
		[SerializeField]
		private AzureWeatherSystem m_weatherSystem = new AzureWeatherSystem();

		// Token: 0x0400000C RID: 12
		[SerializeField]
		private AzureEventSystem m_eventSystem = new AzureEventSystem();

		// Token: 0x0400000D RID: 13
		[SerializeField]
		private AzureCoreUpdateMode m_coreUpdateMode;

		// Token: 0x0400000E RID: 14
		[SerializeField]
		private float m_timeSinceLastUpdate;

		// Token: 0x0400000F RID: 15
		[SerializeField]
		private float m_refreshRate = 0.1f;

		// Token: 0x04000010 RID: 16
		[SerializeField]
		private UnityEvent m_onCoreUpdateEvent = new UnityEvent();

		// Token: 0x04000011 RID: 17
		[SerializeField]
		private ReflectionProbe m_reflectionProbe;

		// Token: 0x04000012 RID: 18
		[SerializeField]
		[Min(0.1f)]
		private float m_environmentRefreshRate = 1f;

		// Token: 0x04000013 RID: 19
		private float m_nextDynamicGIRefreshTime;

		// Token: 0x04000014 RID: 20
		private float m_nextReflectionProbeRefreshTime;

		// Token: 0x04000015 RID: 21
		private float m_lastDynamicGIEvaluationTime = float.NaN;

		// Token: 0x04000016 RID: 22
		private float m_lastReflectionProbeEvaluationTime = float.NaN;

		// Token: 0x04000017 RID: 23
		private AzureWeatherPreset m_lastDynamicGIWeatherPreset;

		// Token: 0x04000018 RID: 24
		private AzureWeatherPreset m_lastReflectionProbeWeatherPreset;

		// Token: 0x04000019 RID: 25
		private int m_lastDynamicGIWeatherIndex = -1;

		// Token: 0x0400001A RID: 26
		private int m_lastReflectionProbeWeatherIndex = -1;

		// Token: 0x0400001B RID: 27
		[SerializeField]
		private List<AzureFollowTarget> m_followTargetList = new List<AzureFollowTarget>();
	}
}
