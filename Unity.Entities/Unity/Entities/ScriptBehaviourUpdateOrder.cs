using System;
using System.Collections.Generic;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Unity.Entities
{
	// Token: 0x02000282 RID: 642
	public static class ScriptBehaviourUpdateOrder
	{
		// Token: 0x06001119 RID: 4377 RVA: 0x0004879D File Offset: 0x0004699D
		internal static bool AppendToPlayerLoop(Type updateType, PlayerLoopSystem.UpdateFunction updateFunction, ref PlayerLoopSystem playerLoop, Type playerLoopSystemType)
		{
			return ScriptBehaviourUpdateOrder.AppendToPlayerLoopList(updateType, updateFunction, ref playerLoop, playerLoopSystemType);
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x000487A8 File Offset: 0x000469A8
		internal static bool AppendToCurrentPlayerLoop(Type updateType, PlayerLoopSystem.UpdateFunction updateFunction, Type playerLoopSystemType)
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			if (!ScriptBehaviourUpdateOrder.AppendToPlayerLoop(updateType, updateFunction, ref currentPlayerLoop, playerLoopSystemType))
			{
				return false;
			}
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
			return true;
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x000487D0 File Offset: 0x000469D0
		internal static bool IsInPlayerLoop(PlayerLoopSystem.UpdateFunction updateFunction, ref PlayerLoopSystem playerLoop)
		{
			return ScriptBehaviourUpdateOrder.IsInPlayerLoopList(updateFunction, ref playerLoop);
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x000487DC File Offset: 0x000469DC
		internal static bool IsInCurrentPlayerLoop(PlayerLoopSystem.UpdateFunction updateFunction)
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			return ScriptBehaviourUpdateOrder.IsInPlayerLoopList(updateFunction, ref currentPlayerLoop);
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x000487F7 File Offset: 0x000469F7
		internal static bool RemoveFromPlayerLoop(PlayerLoopSystem.UpdateFunction updateFunction, ref PlayerLoopSystem playerLoop)
		{
			return ScriptBehaviourUpdateOrder.RemoveFromPlayerLoopList(updateFunction, ref playerLoop);
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00048800 File Offset: 0x00046A00
		internal static void RemoveFromCurrentPlayerLoop(PlayerLoopSystem.UpdateFunction updateFunction)
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			if (ScriptBehaviourUpdateOrder.RemoveFromPlayerLoop(updateFunction, ref currentPlayerLoop))
			{
				PlayerLoop.SetPlayerLoop(currentPlayerLoop);
			}
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00048824 File Offset: 0x00046A24
		public static void AppendWorldToPlayerLoop(World world, ref PlayerLoopSystem playerLoop)
		{
			if (world == null)
			{
				return;
			}
			InitializationSystemGroup existingSystemManaged = world.GetExistingSystemManaged<InitializationSystemGroup>();
			if (existingSystemManaged != null)
			{
				ScriptBehaviourUpdateOrder.AppendSystemToPlayerLoop(existingSystemManaged, ref playerLoop, typeof(Initialization));
			}
			SimulationSystemGroup existingSystemManaged2 = world.GetExistingSystemManaged<SimulationSystemGroup>();
			if (existingSystemManaged2 != null)
			{
				ScriptBehaviourUpdateOrder.AppendSystemToPlayerLoop(existingSystemManaged2, ref playerLoop, typeof(Update));
			}
			PresentationSystemGroup existingSystemManaged3 = world.GetExistingSystemManaged<PresentationSystemGroup>();
			if (existingSystemManaged3 != null)
			{
				ScriptBehaviourUpdateOrder.AppendSystemToPlayerLoop(existingSystemManaged3, ref playerLoop, typeof(PreLateUpdate));
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00048888 File Offset: 0x00046A88
		public static void AppendWorldToCurrentPlayerLoop(World world)
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			ScriptBehaviourUpdateOrder.AppendWorldToPlayerLoop(world, ref currentPlayerLoop);
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x000488A9 File Offset: 0x00046AA9
		public static bool IsWorldInPlayerLoop(World world, PlayerLoopSystem playerLoop)
		{
			return ScriptBehaviourUpdateOrder.IsWorldInPlayerLoopList(world, ref playerLoop);
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x000488B3 File Offset: 0x00046AB3
		public static bool IsWorldInCurrentPlayerLoop(World world)
		{
			return ScriptBehaviourUpdateOrder.IsWorldInPlayerLoop(world, PlayerLoop.GetCurrentPlayerLoop());
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x000488C0 File Offset: 0x00046AC0
		public static void RemoveWorldFromPlayerLoop(World world, ref PlayerLoopSystem playerLoop)
		{
			ScriptBehaviourUpdateOrder.RemoveWorldFromPlayerLoopList(world, ref playerLoop);
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x000488CC File Offset: 0x00046ACC
		public static void RemoveWorldFromCurrentPlayerLoop(World world)
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			ScriptBehaviourUpdateOrder.RemoveWorldFromPlayerLoop(world, ref currentPlayerLoop);
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x000488F0 File Offset: 0x00046AF0
		public static void AppendSystemToPlayerLoop(ComponentSystemBase system, ref PlayerLoopSystem playerLoop, Type playerLoopSystemType)
		{
			ScriptBehaviourUpdateOrder.DummyDelegateWrapper dummyDelegateWrapper = new ScriptBehaviourUpdateOrder.DummyDelegateWrapper(system);
			if (!ScriptBehaviourUpdateOrder.AppendToPlayerLoop(system.GetType(), new PlayerLoopSystem.UpdateFunction(dummyDelegateWrapper.TriggerUpdate), ref playerLoop, playerLoopSystemType))
			{
				throw new ArgumentException(string.Format("Could not find PlayerLoopSystem with type={0}", playerLoopSystemType));
			}
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00048930 File Offset: 0x00046B30
		private static bool AppendToPlayerLoopList(Type updateType, PlayerLoopSystem.UpdateFunction updateFunction, ref PlayerLoopSystem playerLoop, Type playerLoopSystemType)
		{
			if (updateType == null || updateFunction == null || playerLoopSystemType == null)
			{
				return false;
			}
			if (playerLoop.type == playerLoopSystemType)
			{
				int num = ((playerLoop.subSystemList != null) ? playerLoop.subSystemList.Length : 0);
				PlayerLoopSystem[] array = new PlayerLoopSystem[num + 1];
				for (int i = 0; i < num; i++)
				{
					array[i] = playerLoop.subSystemList[i];
				}
				array[num] = new PlayerLoopSystem
				{
					type = updateType,
					updateDelegate = updateFunction
				};
				playerLoop.subSystemList = array;
				return true;
			}
			if (playerLoop.subSystemList != null)
			{
				for (int j = 0; j < playerLoop.subSystemList.Length; j++)
				{
					if (ScriptBehaviourUpdateOrder.AppendToPlayerLoopList(updateType, updateFunction, ref playerLoop.subSystemList[j], playerLoopSystemType))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00048A00 File Offset: 0x00046C00
		private static bool IsInPlayerLoopList(PlayerLoopSystem.UpdateFunction updateFunction, ref PlayerLoopSystem playerLoop)
		{
			if (updateFunction == null)
			{
				return false;
			}
			for (int i = 0; i < playerLoop.subSystemList.Length; i++)
			{
				if (ScriptBehaviourUpdateOrder.IsInPlayerLoopList(updateFunction, ref playerLoop.subSystemList[i]))
				{
					return true;
				}
			}
			return playerLoop.updateDelegate == updateFunction;
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00048A48 File Offset: 0x00046C48
		private static bool IsWorldInPlayerLoopList(World world, ref PlayerLoopSystem playerLoop)
		{
			if (world == null || !world.IsCreated)
			{
				return false;
			}
			if (ScriptBehaviourUpdateOrder.IsDelegateForWorldSystem(world, ref playerLoop))
			{
				return true;
			}
			if (playerLoop.subSystemList != null)
			{
				for (int i = 0; i < playerLoop.subSystemList.Length; i++)
				{
					if (ScriptBehaviourUpdateOrder.IsWorldInPlayerLoopList(world, ref playerLoop.subSystemList[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00048AA0 File Offset: 0x00046CA0
		private static bool RemoveFromPlayerLoopList(ScriptBehaviourUpdateOrder.RemoveFromPlayerLoopDelegate removeDelegate, ref PlayerLoopSystem playerLoop)
		{
			if (removeDelegate == null || playerLoop.subSystemList == null || playerLoop.subSystemList.Length == 0)
			{
				return false;
			}
			bool flag = false;
			List<PlayerLoopSystem> list = new List<PlayerLoopSystem>(playerLoop.subSystemList.Length);
			for (int i = 0; i < playerLoop.subSystemList.Length; i++)
			{
				ref PlayerLoopSystem ptr = ref playerLoop.subSystemList[i];
				flag |= ScriptBehaviourUpdateOrder.RemoveFromPlayerLoopList(removeDelegate, ref ptr);
				if (!removeDelegate(ref ptr))
				{
					list.Add(ptr);
				}
			}
			if (list.Count != playerLoop.subSystemList.Length)
			{
				playerLoop.subSystemList = list.ToArray();
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00048B31 File Offset: 0x00046D31
		private static bool RemoveFromPlayerLoopList(PlayerLoopSystem.UpdateFunction updateFunction, ref PlayerLoopSystem playerLoop)
		{
			return ScriptBehaviourUpdateOrder.RemoveFromPlayerLoopList(delegate(ref PlayerLoopSystem pl)
			{
				return pl.updateDelegate == updateFunction;
			}, ref playerLoop);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00048B50 File Offset: 0x00046D50
		private static void RemoveWorldFromPlayerLoopList(World world, ref PlayerLoopSystem playerLoop)
		{
			ScriptBehaviourUpdateOrder.RemoveFromPlayerLoopList(delegate(ref PlayerLoopSystem pl)
			{
				return ScriptBehaviourUpdateOrder.IsDelegateForWorldSystem(world, ref pl);
			}, ref playerLoop);
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00048B70 File Offset: 0x00046D70
		private static bool IsDelegateForWorldSystem(World world, ref PlayerLoopSystem playerLoop)
		{
			return typeof(ComponentSystemBase).IsAssignableFrom(playerLoop.type) && (playerLoop.updateDelegate.Target as ScriptBehaviourUpdateOrder.DummyDelegateWrapper).System.World == world;
		}

		// Token: 0x02000283 RID: 643
		// (Invoke) Token: 0x0600112E RID: 4398
		private delegate bool RemoveFromPlayerLoopDelegate(ref PlayerLoopSystem playerLoop);

		// Token: 0x02000284 RID: 644
		internal class DummyDelegateWrapper
		{
			// Token: 0x17000121 RID: 289
			// (get) Token: 0x06001131 RID: 4401 RVA: 0x00048BA9 File Offset: 0x00046DA9
			internal ComponentSystemBase System
			{
				get
				{
					return this.m_System;
				}
			}

			// Token: 0x06001132 RID: 4402 RVA: 0x00048BB1 File Offset: 0x00046DB1
			public DummyDelegateWrapper(ComponentSystemBase sys)
			{
				this.m_System = sys;
			}

			// Token: 0x06001133 RID: 4403 RVA: 0x00048BC0 File Offset: 0x00046DC0
			public void TriggerUpdate()
			{
				if (this.m_System.m_StatePtr != null)
				{
					this.m_System.Update();
				}
			}

			// Token: 0x04000692 RID: 1682
			private readonly ComponentSystemBase m_System;
		}
	}
}
