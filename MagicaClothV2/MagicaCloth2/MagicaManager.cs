using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;

namespace MagicaCloth2
{
	// Token: 0x0200007C RID: 124
	public static class MagicaManager
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0001131A File Offset: 0x0000F51A
		public static TimeManager Time
		{
			get
			{
				List<IManager> list = MagicaManager.managers;
				return ((list != null) ? list[0] : null) as TimeManager;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00011333 File Offset: 0x0000F533
		public static TeamManager Team
		{
			get
			{
				List<IManager> list = MagicaManager.managers;
				return ((list != null) ? list[1] : null) as TeamManager;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0001134C File Offset: 0x0000F54C
		public static ClothManager Cloth
		{
			get
			{
				List<IManager> list = MagicaManager.managers;
				return ((list != null) ? list[2] : null) as ClothManager;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x00011365 File Offset: 0x0000F565
		public static RenderManager Render
		{
			get
			{
				List<IManager> list = MagicaManager.managers;
				return ((list != null) ? list[3] : null) as RenderManager;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0001137E File Offset: 0x0000F57E
		public static TransformManager Bone
		{
			get
			{
				List<IManager> list = MagicaManager.managers;
				return ((list != null) ? list[4] : null) as TransformManager;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00011397 File Offset: 0x0000F597
		public static VirtualMeshManager VMesh
		{
			get
			{
				List<IManager> list = MagicaManager.managers;
				return ((list != null) ? list[5] : null) as VirtualMeshManager;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001EB RID: 491 RVA: 0x000113B0 File Offset: 0x0000F5B0
		public static SimulationManager Simulation
		{
			get
			{
				List<IManager> list = MagicaManager.managers;
				return ((list != null) ? list[6] : null) as SimulationManager;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000113C9 File Offset: 0x0000F5C9
		public static ColliderManager Collider
		{
			get
			{
				List<IManager> list = MagicaManager.managers;
				return ((list != null) ? list[7] : null) as ColliderManager;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060001ED RID: 493 RVA: 0x000113E2 File Offset: 0x0000F5E2
		public static WindManager Wind
		{
			get
			{
				List<IManager> list = MagicaManager.managers;
				return ((list != null) ? list[8] : null) as WindManager;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000113FB File Offset: 0x0000F5FB
		public static PreBuildManager PreBuild
		{
			get
			{
				List<IManager> list = MagicaManager.managers;
				return ((list != null) ? list[9] : null) as PreBuildManager;
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00011418 File Offset: 0x0000F618
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Initialize()
		{
			MagicaManager.Dispose();
			MagicaManager.managers = new List<IManager>();
			MagicaManager.managers.Add(new TimeManager());
			MagicaManager.managers.Add(new TeamManager());
			MagicaManager.managers.Add(new ClothManager());
			MagicaManager.managers.Add(new RenderManager());
			MagicaManager.managers.Add(new TransformManager());
			MagicaManager.managers.Add(new VirtualMeshManager());
			MagicaManager.managers.Add(new SimulationManager());
			MagicaManager.managers.Add(new ColliderManager());
			MagicaManager.managers.Add(new WindManager());
			MagicaManager.managers.Add(new PreBuildManager());
			foreach (IManager manager in MagicaManager.managers)
			{
				manager.Initialize();
			}
			MagicaManager.InitCustomGameLoop();
			Application.quitting += MagicaManager.OnAppQuitting;
			MagicaManager.isPlaying = true;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0001152C File Offset: 0x0000F72C
		private static void OnAppQuitting()
		{
			MagicaManager.Dispose();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00011534 File Offset: 0x0000F734
		private static void Dispose()
		{
			if (MagicaManager.managers != null)
			{
				foreach (IManager manager in MagicaManager.managers)
				{
					manager.Dispose();
				}
				MagicaManager.managers = null;
			}
			MagicaManager.OnPreSimulation = null;
			MagicaManager.OnPostSimulation = null;
			Application.quitting -= MagicaManager.OnAppQuitting;
			MagicaManager.isPlaying = false;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000115B8 File Offset: 0x0000F7B8
		public static bool IsPlaying()
		{
			return MagicaManager.isPlaying && Application.isPlaying;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000115CC File Offset: 0x0000F7CC
		public static void InitCustomGameLoop()
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			if (MagicaManager.CheckRegist(ref currentPlayerLoop))
			{
				return;
			}
			MagicaManager.SetCustomGameLoop(ref currentPlayerLoop);
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000115F8 File Offset: 0x0000F7F8
		private static void SetCustomGameLoop(ref PlayerLoopSystem playerLoop)
		{
			PlayerLoopSystem playerLoopSystem = default(PlayerLoopSystem);
			playerLoopSystem.type = typeof(MagicaManager);
			playerLoopSystem.updateDelegate = delegate
			{
				MagicaManager.UpdateMethod updateMethod = MagicaManager.afterEarlyUpdateDelegate;
				if (updateMethod == null)
				{
					return;
				}
				updateMethod();
			};
			MagicaManager.AddPlayerLoop(playerLoopSystem, ref playerLoop, "EarlyUpdate", string.Empty, 1, false);
			playerLoopSystem = default(PlayerLoopSystem);
			playerLoopSystem.type = typeof(MagicaManager);
			playerLoopSystem.updateDelegate = delegate
			{
				MagicaManager.UpdateMethod updateMethod2 = MagicaManager.afterFixedUpdateDelegate;
				if (updateMethod2 == null)
				{
					return;
				}
				updateMethod2();
			};
			MagicaManager.AddPlayerLoop(playerLoopSystem, ref playerLoop, "FixedUpdate", "ScriptRunBehaviourFixedUpdate", 0, false);
			playerLoopSystem = default(PlayerLoopSystem);
			playerLoopSystem.type = typeof(MagicaManager);
			playerLoopSystem.updateDelegate = delegate
			{
				MagicaManager.UpdateMethod updateMethod3 = MagicaManager.firstPreUpdateDelegate;
				if (updateMethod3 == null)
				{
					return;
				}
				updateMethod3();
			};
			MagicaManager.AddPlayerLoop(playerLoopSystem, ref playerLoop, "PreUpdate", string.Empty, -1, false);
			playerLoopSystem = default(PlayerLoopSystem);
			playerLoopSystem.type = typeof(MagicaManager);
			playerLoopSystem.updateDelegate = delegate
			{
				MagicaManager.UpdateMethod updateMethod4 = MagicaManager.afterUpdateDelegate;
				if (updateMethod4 != null)
				{
					updateMethod4();
				}
				if (Application.isPlaying)
				{
					MagicaManager.UpdateMethod updateMethod5 = MagicaManager.defaultUpdateDelegate;
					if (updateMethod5 == null)
					{
						return;
					}
					updateMethod5();
				}
			};
			MagicaManager.AddPlayerLoop(playerLoopSystem, ref playerLoop, "Update", "ScriptRunDelayedTasks", 0, false);
			playerLoopSystem = default(PlayerLoopSystem);
			playerLoopSystem.type = typeof(MagicaManager);
			playerLoopSystem.updateDelegate = delegate
			{
				MagicaManager.UpdateMethod updateMethod6 = MagicaManager.beforeLateUpdateDelegate;
				if (updateMethod6 == null)
				{
					return;
				}
				updateMethod6();
			};
			MagicaManager.AddPlayerLoop(playerLoopSystem, ref playerLoop, "PreLateUpdate", "ScriptRunBehaviourLateUpdate", 0, true);
			playerLoopSystem = default(PlayerLoopSystem);
			playerLoopSystem.type = typeof(MagicaManager);
			playerLoopSystem.updateDelegate = delegate
			{
				MagicaManager.UpdateMethod updateMethod7 = MagicaManager.afterLateUpdateDelegate;
				if (updateMethod7 == null)
				{
					return;
				}
				updateMethod7();
			};
			MagicaManager.AddPlayerLoop(playerLoopSystem, ref playerLoop, "PreLateUpdate", "ScriptRunBehaviourLateUpdate", 0, false);
			playerLoopSystem = default(PlayerLoopSystem);
			playerLoopSystem.type = typeof(MagicaManager);
			playerLoopSystem.updateDelegate = delegate
			{
				MagicaManager.UpdateMethod updateMethod8 = MagicaManager.afterDelayedDelegate;
				if (updateMethod8 == null)
				{
					return;
				}
				updateMethod8();
			};
			MagicaManager.AddPlayerLoop(playerLoopSystem, ref playerLoop, "PostLateUpdate", "ScriptRunDelayedDynamicFrameRate", 0, false);
			playerLoopSystem = default(PlayerLoopSystem);
			playerLoopSystem.type = typeof(MagicaManager);
			playerLoopSystem.updateDelegate = delegate
			{
				MagicaManager.UpdateMethod updateMethod9 = MagicaManager.afterRenderingDelegate;
				if (updateMethod9 == null)
				{
					return;
				}
				updateMethod9();
			};
			MagicaManager.AddPlayerLoop(playerLoopSystem, ref playerLoop, "PostLateUpdate", "FinishFrameRendering", 0, false);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00011898 File Offset: 0x0000FA98
		private static void AddPlayerLoop(PlayerLoopSystem method, ref PlayerLoopSystem playerLoop, string categoryName, string systemName, int firstLast = 0, bool before = false)
		{
			int num = Array.FindIndex<PlayerLoopSystem>(playerLoop.subSystemList, (PlayerLoopSystem s) => s.type.Name == categoryName);
			PlayerLoopSystem playerLoopSystem = playerLoop.subSystemList[num];
			List<PlayerLoopSystem> list = new List<PlayerLoopSystem>(playerLoopSystem.subSystemList);
			if (firstLast < 0)
			{
				list.Insert(0, method);
			}
			else if (firstLast > 0)
			{
				list.Add(method);
			}
			else
			{
				int num2 = list.FindIndex((PlayerLoopSystem h) => h.type.Name.Contains(systemName));
				if (before)
				{
					list.Insert(num2, method);
				}
				else
				{
					list.Insert(num2 + 1, method);
				}
			}
			playerLoopSystem.subSystemList = list.ToArray();
			playerLoop.subSystemList[num] = playerLoopSystem;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00011950 File Offset: 0x0000FB50
		private static bool CheckRegist(ref PlayerLoopSystem playerLoop)
		{
			Type t = typeof(MagicaManager);
			Func<PlayerLoopSystem, bool> <>9__0;
			foreach (PlayerLoopSystem playerLoopSystem in playerLoop.subSystemList)
			{
				if (playerLoopSystem.subSystemList != null)
				{
					IEnumerable<PlayerLoopSystem> subSystemList2 = playerLoopSystem.subSystemList;
					Func<PlayerLoopSystem, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (PlayerLoopSystem x) => x.type == t);
					}
					if (subSystemList2.Any<PlayerLoopSystem>(func))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000119C9 File Offset: 0x0000FBC9
		public static void SetGlobalTimeScale(float timeScale)
		{
			if (MagicaManager.IsPlaying())
			{
				MagicaManager.Time.GlobalTimeScale = Mathf.Clamp01(timeScale);
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000119E4 File Offset: 0x0000FBE4
		public static float GetGlobalTimeScale()
		{
			if (MagicaManager.IsPlaying())
			{
				return MagicaManager.Time.GlobalTimeScale;
			}
			object obj = "MagicaManager is not starting!";
			Develop.LogError(in obj);
			return 1f;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00011A15 File Offset: 0x0000FC15
		public static void SetSimulationFrequency(int freq)
		{
			if (MagicaManager.IsPlaying())
			{
				MagicaManager.Time.simulationFrequency = Mathf.Clamp(freq, 30, 150);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00011A38 File Offset: 0x0000FC38
		public static int GetSimulationFrequency()
		{
			if (MagicaManager.IsPlaying())
			{
				return MagicaManager.Time.simulationFrequency;
			}
			object obj = "MagicaManager is not starting!";
			Develop.LogError(in obj);
			return 0;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00011A65 File Offset: 0x0000FC65
		public static void SetMaxSimulationCountPerFrame(int count)
		{
			if (MagicaManager.IsPlaying())
			{
				MagicaManager.Time.maxSimulationCountPerFrame = Mathf.Clamp(count, 1, 5);
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00011A80 File Offset: 0x0000FC80
		public static int GetMaxSimulationCountPerFrame()
		{
			if (MagicaManager.IsPlaying())
			{
				return MagicaManager.Time.maxSimulationCountPerFrame;
			}
			object obj = "MagicaManager is not starting!";
			Develop.LogError(in obj);
			return 0;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00011AAD File Offset: 0x0000FCAD
		public static void SetUpdateLocation(TimeManager.UpdateLocation updateLocation)
		{
			if (MagicaManager.IsPlaying())
			{
				MagicaManager.Time.updateLocation = updateLocation;
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00011AC4 File Offset: 0x0000FCC4
		public static TimeManager.UpdateLocation GetUpdateLocation()
		{
			if (MagicaManager.IsPlaying())
			{
				return MagicaManager.Time.updateLocation;
			}
			object obj = "MagicaManager is not starting!";
			Develop.LogError(in obj);
			return TimeManager.UpdateLocation.AfterLateUpdate;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00011AF1 File Offset: 0x0000FCF1
		public static void UnloadUnusedData()
		{
			if (MagicaManager.IsPlaying())
			{
				MagicaManager.PreBuild.UnloadUnusedData();
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00011B04 File Offset: 0x0000FD04
		public static void SetInitializationLocation(MagicaManager.InitializationLocation initLocation)
		{
			MagicaManager.initializationLocation = initLocation;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00011B0C File Offset: 0x0000FD0C
		public static void SetSplitProxyMeshVertexCount(int vertexCount)
		{
			if (MagicaManager.IsPlaying())
			{
				MagicaManager.Simulation.splitProxyMeshVertexCount = vertexCount;
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00011B20 File Offset: 0x0000FD20
		public static int GetSplitProxyMeshVertexCount()
		{
			if (MagicaManager.IsPlaying())
			{
				return MagicaManager.Simulation.splitProxyMeshVertexCount;
			}
			object obj = "MagicaManager is not starting!";
			Develop.LogError(in obj);
			return 0;
		}

		// Token: 0x0400035E RID: 862
		private static List<IManager> managers;

		// Token: 0x0400035F RID: 863
		public static MagicaManager.UpdateMethod afterEarlyUpdateDelegate;

		// Token: 0x04000360 RID: 864
		public static MagicaManager.UpdateMethod afterFixedUpdateDelegate;

		// Token: 0x04000361 RID: 865
		public static MagicaManager.UpdateMethod firstPreUpdateDelegate;

		// Token: 0x04000362 RID: 866
		public static MagicaManager.UpdateMethod afterUpdateDelegate;

		// Token: 0x04000363 RID: 867
		public static MagicaManager.UpdateMethod beforeLateUpdateDelegate;

		// Token: 0x04000364 RID: 868
		public static MagicaManager.UpdateMethod afterLateUpdateDelegate;

		// Token: 0x04000365 RID: 869
		public static MagicaManager.UpdateMethod afterDelayedDelegate;

		// Token: 0x04000366 RID: 870
		public static MagicaManager.UpdateMethod afterRenderingDelegate;

		// Token: 0x04000367 RID: 871
		public static MagicaManager.UpdateMethod defaultUpdateDelegate;

		// Token: 0x04000368 RID: 872
		private static volatile bool isPlaying;

		// Token: 0x04000369 RID: 873
		public static Action OnPreSimulation;

		// Token: 0x0400036A RID: 874
		public static Action OnPostSimulation;

		// Token: 0x0400036B RID: 875
		internal static MagicaManager.InitializationLocation initializationLocation;

		// Token: 0x0200007D RID: 125
		// (Invoke) Token: 0x06000204 RID: 516
		public delegate void UpdateMethod();

		// Token: 0x0200007E RID: 126
		public enum InitializationLocation
		{
			// Token: 0x0400036D RID: 877
			Start,
			// Token: 0x0400036E RID: 878
			Awake
		}
	}
}
