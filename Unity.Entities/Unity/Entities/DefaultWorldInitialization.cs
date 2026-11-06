using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Unity.Entities
{
	// Token: 0x0200006C RID: 108
	public static class DefaultWorldInitialization
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600028D RID: 653 RVA: 0x0000BA60 File Offset: 0x00009C60
		// (remove) Token: 0x0600028E RID: 654 RVA: 0x0000BA94 File Offset: 0x00009C94
		internal static event Action<World> DefaultWorldInitialized;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600028F RID: 655 RVA: 0x0000BAC8 File Offset: 0x00009CC8
		// (remove) Token: 0x06000290 RID: 656 RVA: 0x0000BAFC File Offset: 0x00009CFC
		internal static event Action DefaultWorldDestroyed;

		// Token: 0x06000291 RID: 657 RVA: 0x0000BB2F File Offset: 0x00009D2F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void CleanupWorldBeforeSceneLoad()
		{
			DefaultWorldInitialization.DomainUnloadOrPlayModeChangeShutdown();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000BB38 File Offset: 0x00009D38
		private static void RegisterUnloadOrPlayModeChangeShutdown()
		{
			if (DefaultWorldInitialization.s_UnloadOrPlayModeChangeShutdownRegistered)
			{
				return;
			}
			GameObject gameObject = new GameObject
			{
				hideFlags = HideFlags.HideInHierarchy
			};
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			else
			{
				gameObject.hideFlags = HideFlags.HideAndDontSave;
			}
			gameObject.AddComponent<DefaultWorldInitializationProxy>().IsActive = true;
			RuntimeApplication.RegisterFrameUpdateToCurrentPlayerLoop();
			DefaultWorldInitialization.s_UnloadOrPlayModeChangeShutdownRegistered = true;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000BB88 File Offset: 0x00009D88
		internal static void DomainUnloadOrPlayModeChangeShutdown()
		{
			if (!DefaultWorldInitialization.s_UnloadOrPlayModeChangeShutdownRegistered)
			{
				return;
			}
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			foreach (World world in World.s_AllWorlds)
			{
				ScriptBehaviourUpdateOrder.RemoveWorldFromPlayerLoop(world, ref currentPlayerLoop);
			}
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
			RuntimeApplication.UnregisterFrameUpdateToCurrentPlayerLoop();
			World.DisposeAllWorlds();
			EntityComponentStore.s_entityStore.Data.Dispose();
			DefaultWorldInitialization.s_UnloadOrPlayModeChangeShutdownRegistered = false;
			Action defaultWorldDestroyed = DefaultWorldInitialization.DefaultWorldDestroyed;
			if (defaultWorldDestroyed == null)
			{
				return;
			}
			defaultWorldDestroyed();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000BC1C File Offset: 0x00009E1C
		public static World Initialize(string defaultWorldName, bool editorWorld = false)
		{
			World world2;
			using (new ProfilerMarker("Create World & Systems").Auto())
			{
				DefaultWorldInitialization.RegisterUnloadOrPlayModeChangeShutdown();
				if (!editorWorld)
				{
					ICustomBootstrap customBootstrap = DefaultWorldInitialization.CreateBootStrap();
					if (customBootstrap != null && customBootstrap.Initialize(defaultWorldName))
					{
						return World.DefaultGameObjectInjectionWorld;
					}
				}
				World world = new World(defaultWorldName, editorWorld ? WorldFlags.Editor : WorldFlags.Game);
				World.DefaultGameObjectInjectionWorld = world;
				DefaultWorldInitialization.AddSystemToRootLevelSystemGroupsInternal(world, DefaultWorldInitialization.GetAllSystemTypeIndices(WorldSystemFilterFlags.Default, editorWorld));
				ScriptBehaviourUpdateOrder.AppendWorldToCurrentPlayerLoop(world);
				Action<World> defaultWorldInitialized = DefaultWorldInitialization.DefaultWorldInitialized;
				if (defaultWorldInitialized != null)
				{
					defaultWorldInitialized(world);
				}
				world2 = world;
			}
			return world2;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000BCBC File Offset: 0x00009EBC
		public static void AddSystemsToRootLevelSystemGroups(World world, IEnumerable<Type> systemTypes)
		{
			NativeList<SystemTypeIndex> nativeList = new NativeList<SystemTypeIndex>(16, Allocator.Temp);
			foreach (Type type in systemTypes)
			{
				SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndex(type);
				nativeList.Add(in systemTypeIndex);
			}
			DefaultWorldInitialization.AddSystemToRootLevelSystemGroupsInternal(world, nativeList);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000BD24 File Offset: 0x00009F24
		public static void AddSystemsToRootLevelSystemGroups(World world, IReadOnlyList<Type> systemTypes)
		{
			NativeList<SystemTypeIndex> nativeList = new NativeList<SystemTypeIndex>(16, Allocator.Temp);
			foreach (Type type in systemTypes)
			{
				SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndex(type);
				nativeList.Add(in systemTypeIndex);
			}
			DefaultWorldInitialization.AddSystemToRootLevelSystemGroupsInternal(world, nativeList);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000BD8C File Offset: 0x00009F8C
		public static void AddSystemsToRootLevelSystemGroups(World world, params Type[] systemTypes)
		{
			NativeList<SystemTypeIndex> nativeList = new NativeList<SystemTypeIndex>(systemTypes.Length, Allocator.Temp);
			for (int i = 0; i < systemTypes.Length; i++)
			{
				SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndex(systemTypes[i]);
				nativeList.Add(in systemTypeIndex);
			}
			DefaultWorldInitialization.AddSystemToRootLevelSystemGroupsInternal(world, nativeList);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000BDD0 File Offset: 0x00009FD0
		public static void AddSystemsToRootLevelSystemGroups(World world, NativeList<SystemTypeIndex> systemTypes)
		{
			DefaultWorldInitialization.AddSystemToRootLevelSystemGroupsInternal(world, systemTypes);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000BDDC File Offset: 0x00009FDC
		internal static void AddSystemToRootLevelSystemGroupsInternal<T>(World world, NativeList<SystemTypeIndex> systemTypesOrig, ComponentSystemGroup defaultGroup, T rootGroups) where T : struct, DefaultWorldInitialization.IIdentifyRootGroups
		{
			List<SystemTypeIndex> list = new List<SystemTypeIndex>();
			List<SystemTypeIndex> list2 = new List<SystemTypeIndex>();
			foreach (SystemTypeIndex systemTypeIndex in systemTypesOrig)
			{
				if (!TypeManager.IsSystemTypeIndex(systemTypeIndex))
				{
					throw new InvalidOperationException("Bad type");
				}
				if (systemTypeIndex.IsManaged)
				{
					list.Add(systemTypeIndex);
				}
				else
				{
					list2.Add(systemTypeIndex);
				}
			}
			NativeList<SystemHandle> orCreateSystemsAndLogException = world.GetOrCreateSystemsAndLogException(systemTypesOrig, Allocator.Temp);
			for (int i = 0; i < systemTypesOrig.Length; i++)
			{
				SystemHandle systemHandle = orCreateSystemsAndLogException[i];
				if (!rootGroups.IsRootGroup(systemTypesOrig[i]))
				{
					NativeList<TypeManager.SystemAttribute> systemAttributes = TypeManager.GetSystemAttributes(systemTypesOrig[i], TypeManager.SystemAttributeKind.UpdateInGroup, Allocator.Temp);
					if (systemAttributes.Length == 0)
					{
						defaultGroup.AddSystemToUpdateList(systemHandle);
					}
					foreach (TypeManager.SystemAttribute systemAttribute in systemAttributes)
					{
						ComponentSystemGroup componentSystemGroup = DefaultWorldInitialization.FindGroup(world, systemTypesOrig[i], systemAttribute);
						if (componentSystemGroup != null)
						{
							componentSystemGroup.AddSystemToUpdateList(systemHandle);
						}
					}
				}
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000BF2C File Offset: 0x0000A12C
		internal static void AddSystemToRootLevelSystemGroupsInternal(World world, NativeList<SystemTypeIndex> systemTypesOrig)
		{
			using (new ProfilerMarker("AddSystems").Auto())
			{
				InitializationSystemGroup orCreateSystemManaged = world.GetOrCreateSystemManaged<InitializationSystemGroup>();
				SimulationSystemGroup orCreateSystemManaged2 = world.GetOrCreateSystemManaged<SimulationSystemGroup>();
				ComponentSystemGroup orCreateSystemManaged3 = world.GetOrCreateSystemManaged<PresentationSystemGroup>();
				DefaultWorldInitialization.AddSystemToRootLevelSystemGroupsInternal<DefaultWorldInitialization.DefaultRootGroups>(world, systemTypesOrig, orCreateSystemManaged2, default(DefaultWorldInitialization.DefaultRootGroups));
				orCreateSystemManaged.SortSystems();
				orCreateSystemManaged2.SortSystems();
				orCreateSystemManaged3.SortSystems();
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000BFA4 File Offset: 0x0000A1A4
		private static ComponentSystemGroup FindGroup(World world, SystemTypeIndex systemType, TypeManager.SystemAttribute attr)
		{
			SystemTypeIndex targetSystemTypeIndex = attr.TargetSystemTypeIndex;
			if (!TypeManager.IsSystemTypeIndex(targetSystemTypeIndex) || !targetSystemTypeIndex.IsGroup)
			{
				throw new InvalidOperationException(string.Format("Invalid [{0}] attribute for {1}: target group must be derived from {2}.", "UpdateInGroupAttribute", systemType, "ComponentSystemGroup"));
			}
			if ((attr.Flags & 1) != 0 && (attr.Flags & 2) != 0)
			{
				throw new InvalidOperationException(string.Format("The system {0} can not specify both OrderFirst=true and OrderLast=true in its [{1}] attribute.", systemType, "UpdateInGroupAttribute"));
			}
			ComponentSystemBase existingSystemManaged = world.GetExistingSystemManaged(targetSystemTypeIndex);
			if (existingSystemManaged == null)
			{
				bool flag = TypeManager.GetSystemAttributes(targetSystemTypeIndex, TypeManager.SystemAttributeKind.DisableAutoCreation, Allocator.Temp).Length > 0;
				NativeText.ReadOnly systemName = TypeManager.GetSystemName(targetSystemTypeIndex);
				if (flag)
				{
					Debug.LogWarning(string.Format("A system {0} wants to execute in {1} but this group has [{2}] and {3} does not. The system will not be added to any group and thus not update.", new object[] { systemType, systemName, "DisableAutoCreationAttribute", systemType }));
				}
				else
				{
					Debug.LogWarning(string.Format("A system {0} could not be added to group {1}, because the group was not created in the world {2}. Fix these errors before continuing. The system will not be added to any group and thus not update.", systemType, systemName, world.Name));
				}
			}
			return existingSystemManaged as ComponentSystemGroup;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00002287 File Offset: 0x00000487
		public static void DefaultLazyEditModeInitialize()
		{
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000C0A0 File Offset: 0x0000A2A0
		public static IReadOnlyList<Type> GetAllSystems(WorldSystemFilterFlags filterFlags, bool requireExecuteInEditor = false)
		{
			IReadOnlyList<Type> readOnlyList;
			using (new ProfilerMarker("GetAllSystems").Auto())
			{
				NativeList<SystemTypeIndex> allSystemTypeIndices = DefaultWorldInitialization.GetAllSystemTypeIndices(filterFlags, requireExecuteInEditor);
				List<Type> list = new List<Type>();
				for (int i = 0; i < allSystemTypeIndices.Length; i++)
				{
					list.Add(TypeManager.GetSystemType(allSystemTypeIndices[i]));
				}
				readOnlyList = list;
			}
			return readOnlyList;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000C120 File Offset: 0x0000A320
		public static NativeList<SystemTypeIndex> GetAllSystemTypeIndices(WorldSystemFilterFlags filterFlags, bool requireExecuteInEditor = false)
		{
			return TypeManager.GetSystemTypeIndices(filterFlags, requireExecuteInEditor ? WorldSystemFilterFlags.Editor : ((WorldSystemFilterFlags)0U));
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000C130 File Offset: 0x0000A330
		private static ICustomBootstrap CreateBootStrap()
		{
			IEnumerable<Type> typesDerivedFrom = TypeManager.GetTypesDerivedFrom(typeof(ICustomBootstrap));
			Type type = null;
			foreach (Type type2 in typesDerivedFrom)
			{
				if (!type2.IsAbstract && !type2.ContainsGenericParameters)
				{
					if (type == null)
					{
						type = type2;
					}
					else if (type.IsAssignableFrom(type2))
					{
						type = type2;
					}
					else if (!type2.IsAssignableFrom(type))
					{
						string text = "Multiple custom ICustomBootstrap specified, ignoring ";
						Type type3 = type2;
						Debug.LogError(text + ((type3 != null) ? type3.ToString() : null));
					}
				}
			}
			ICustomBootstrap customBootstrap = null;
			if (type != null)
			{
				customBootstrap = Activator.CreateInstance(type) as ICustomBootstrap;
			}
			return customBootstrap;
		}

		// Token: 0x040000B1 RID: 177
		private static bool s_UnloadOrPlayModeChangeShutdownRegistered;

		// Token: 0x0200006D RID: 109
		internal interface IIdentifyRootGroups
		{
			// Token: 0x060002A0 RID: 672
			bool IsRootGroup(SystemTypeIndex type);
		}

		// Token: 0x0200006E RID: 110
		private struct DefaultRootGroups : DefaultWorldInitialization.IIdentifyRootGroups
		{
			// Token: 0x060002A1 RID: 673 RVA: 0x0000C1E8 File Offset: 0x0000A3E8
			public bool IsRootGroup(SystemTypeIndex type)
			{
				return type == TypeManager.GetSystemTypeIndex<InitializationSystemGroup>() || type == TypeManager.GetSystemTypeIndex<SimulationSystemGroup>() || type == TypeManager.GetSystemTypeIndex<PresentationSystemGroup>();
			}
		}
	}
}
