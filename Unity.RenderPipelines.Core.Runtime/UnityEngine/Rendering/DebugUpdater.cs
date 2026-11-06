using System;
using System.Collections;
using System.Reflection;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.UI;

namespace UnityEngine.Rendering
{
	// Token: 0x02000069 RID: 105
	internal class DebugUpdater : MonoBehaviour
	{
		// Token: 0x06000374 RID: 884 RVA: 0x0000FCA5 File Offset: 0x0000DEA5
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void RuntimeInit()
		{
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000FCA7 File Offset: 0x0000DEA7
		internal static void SetEnabled(bool enabled)
		{
			if (enabled)
			{
				DebugUpdater.EnableRuntime();
				return;
			}
			DebugUpdater.DisableRuntime();
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000FCB8 File Offset: 0x0000DEB8
		private static void EnableRuntime()
		{
			if (DebugUpdater.s_Instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject();
			gameObject.name = "[Debug Updater]";
			DebugUpdater.s_Instance = gameObject.AddComponent<DebugUpdater>();
			DebugUpdater.s_Instance.m_Orientation = Screen.orientation;
			Object.DontDestroyOnLoad(gameObject);
			DebugManager.instance.EnableInputActions();
			EnhancedTouchSupport.Enable();
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000FD11 File Offset: 0x0000DF11
		private static void DisableRuntime()
		{
			DebugManager instance = DebugManager.instance;
			instance.displayRuntimeUI = false;
			instance.displayPersistentRuntimeUI = false;
			if (DebugUpdater.s_Instance != null)
			{
				CoreUtils.Destroy(DebugUpdater.s_Instance.gameObject);
				DebugUpdater.s_Instance = null;
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000FD47 File Offset: 0x0000DF47
		internal static void HandleInternalEventSystemComponents(bool uiEnabled)
		{
			if (DebugUpdater.s_Instance == null)
			{
				return;
			}
			if (uiEnabled)
			{
				DebugUpdater.s_Instance.EnsureExactlyOneEventSystem();
				return;
			}
			DebugUpdater.s_Instance.DestroyDebugEventSystem();
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000FD70 File Offset: 0x0000DF70
		private void EnsureExactlyOneEventSystem()
		{
			EventSystem[] array = Object.FindObjectsOfType<EventSystem>();
			EventSystem component = base.GetComponent<EventSystem>();
			if (array.Length > 1 && component != null)
			{
				Debug.Log("More than one EventSystem detected in scene. Destroying EventSystem owned by DebugUpdater.");
				this.DestroyDebugEventSystem();
				return;
			}
			if (array.Length == 0)
			{
				Debug.Log("No EventSystem available. Creating a new EventSystem to enable Rendering Debugger runtime UI.");
				this.CreateDebugEventSystem();
				return;
			}
			base.StartCoroutine(this.DoAfterInputModuleUpdated(new Action(this.CheckInputModuleExists)));
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000FDD8 File Offset: 0x0000DFD8
		private IEnumerator DoAfterInputModuleUpdated(Action action)
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			action();
			yield break;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000FDE7 File Offset: 0x0000DFE7
		private void CheckInputModuleExists()
		{
			if (EventSystem.current != null && EventSystem.current.currentInputModule == null)
			{
				Debug.LogWarning("Found a game object with EventSystem component but no corresponding BaseInputModule component - Debug UI input might not work correctly.");
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000FE14 File Offset: 0x0000E014
		private void AssignDefaultActions()
		{
			if (EventSystem.current != null)
			{
				InputSystemUIInputModule inputSystemUIInputModule = EventSystem.current.currentInputModule as InputSystemUIInputModule;
				if (inputSystemUIInputModule != null)
				{
					MethodInfo method = inputSystemUIInputModule.GetType().GetMethod("AssignDefaultActions");
					if (method != null)
					{
						method.Invoke(inputSystemUIInputModule, null);
					}
				}
			}
			this.CheckInputModuleExists();
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000FE6A File Offset: 0x0000E06A
		private void CreateDebugEventSystem()
		{
			base.gameObject.AddComponent<EventSystem>();
			base.gameObject.AddComponent<InputSystemUIInputModule>();
			base.StartCoroutine(this.DoAfterInputModuleUpdated(new Action(this.AssignDefaultActions)));
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000FEA0 File Offset: 0x0000E0A0
		private void DestroyDebugEventSystem()
		{
			Object component = base.GetComponent<EventSystem>();
			InputSystemUIInputModule component2 = base.GetComponent<InputSystemUIInputModule>();
			if (component2)
			{
				CoreUtils.Destroy(component2);
				base.StartCoroutine(this.DoAfterInputModuleUpdated(new Action(this.AssignDefaultActions)));
			}
			CoreUtils.Destroy(component);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000FEE8 File Offset: 0x0000E0E8
		private void Update()
		{
			DebugManager instance = DebugManager.instance;
			if (this.m_RuntimeUiWasVisibleLastFrame != instance.displayRuntimeUI)
			{
				DebugUpdater.HandleInternalEventSystemComponents(instance.displayRuntimeUI);
			}
			instance.UpdateActions();
			if (instance.GetAction(DebugAction.EnableDebugMenu) != 0f || instance.GetActionToggleDebugMenuWithTouch())
			{
				instance.displayRuntimeUI = !instance.displayRuntimeUI;
			}
			if (instance.displayRuntimeUI)
			{
				if (instance.GetAction(DebugAction.ResetAll) != 0f)
				{
					instance.Reset();
				}
				if (instance.GetActionReleaseScrollTarget())
				{
					instance.SetScrollTarget(null);
				}
			}
			if (this.m_Orientation != Screen.orientation)
			{
				base.StartCoroutine(DebugUpdater.RefreshRuntimeUINextFrame());
				this.m_Orientation = Screen.orientation;
			}
			this.m_RuntimeUiWasVisibleLastFrame = instance.displayRuntimeUI;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000FF9A File Offset: 0x0000E19A
		private static IEnumerator RefreshRuntimeUINextFrame()
		{
			yield return null;
			DebugManager.instance.ReDrawOnScreenDebug();
			yield break;
		}

		// Token: 0x040001EF RID: 495
		private static DebugUpdater s_Instance;

		// Token: 0x040001F0 RID: 496
		private ScreenOrientation m_Orientation;

		// Token: 0x040001F1 RID: 497
		private bool m_RuntimeUiWasVisibleLastFrame;
	}
}
