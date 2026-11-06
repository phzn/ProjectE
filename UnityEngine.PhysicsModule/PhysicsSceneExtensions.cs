using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.SceneManagement;

namespace UnityEngine
{
	// Token: 0x02000038 RID: 56
	public static class PhysicsSceneExtensions
	{
		// Token: 0x060004A2 RID: 1186 RVA: 0x000068E0 File Offset: 0x00004AE0
		public static PhysicsScene GetPhysicsScene(this Scene scene)
		{
			bool flag = !scene.IsValid();
			if (flag)
			{
				throw new ArgumentException("Cannot get physics scene; Unity scene is invalid.", "scene");
			}
			PhysicsScene physicsScene_Internal = PhysicsSceneExtensions.GetPhysicsScene_Internal(scene);
			bool flag2 = physicsScene_Internal.IsValid();
			if (flag2)
			{
				return physicsScene_Internal;
			}
			throw new Exception("The physics scene associated with the Unity scene is invalid.");
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00006930 File Offset: 0x00004B30
		[StaticAccessor("GetPhysicsManager()", StaticAccessorType.Dot)]
		[NativeMethod("GetPhysicsSceneFromUnityScene")]
		private static PhysicsScene GetPhysicsScene_Internal(Scene scene)
		{
			PhysicsScene physicsScene;
			PhysicsSceneExtensions.GetPhysicsScene_Internal_Injected(ref scene, out physicsScene);
			return physicsScene;
		}

		// Token: 0x060004A4 RID: 1188
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetPhysicsScene_Internal_Injected(ref Scene scene, out PhysicsScene ret);
	}
}
