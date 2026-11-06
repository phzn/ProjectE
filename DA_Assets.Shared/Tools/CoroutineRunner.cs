using System;
using System.Collections;
using UnityEngine;

namespace DA_Assets.Tools
{
	// Token: 0x0200000C RID: 12
	internal static class CoroutineRunner
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00002819 File Offset: 0x00000A19
		public static Coroutine StartCoroutine(IEnumerator coroutine)
		{
			CoroutineRunner.CreateGameObjectIfNotExists();
			return CoroutineRunner._coroutineRunnerInstance.StartCoroutine(coroutine);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000282B File Offset: 0x00000A2B
		private static void CreateGameObjectIfNotExists()
		{
			if (CoroutineRunner._coroutineRunnerGameObject == null)
			{
				CoroutineRunner._coroutineRunnerGameObject = new GameObject("CoroutineRunner");
				CoroutineRunner._coroutineRunnerInstance = CoroutineRunner._coroutineRunnerGameObject.AddComponent<CoroutineRunner.CoroutineRunnerBehaviour>();
				if (Application.isPlaying)
				{
					Object.DontDestroyOnLoad(CoroutineRunner._coroutineRunnerGameObject);
				}
			}
		}

		// Token: 0x0400001C RID: 28
		private static GameObject _coroutineRunnerGameObject;

		// Token: 0x0400001D RID: 29
		private static CoroutineRunner.CoroutineRunnerBehaviour _coroutineRunnerInstance;

		// Token: 0x0200004E RID: 78
		private class CoroutineRunnerBehaviour : MonoBehaviour
		{
		}
	}
}
