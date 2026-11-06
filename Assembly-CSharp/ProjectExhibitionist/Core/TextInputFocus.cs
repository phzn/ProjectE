using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A76 RID: 2678
	[DefaultExecutionOrder(-32000)]
	public sealed class TextInputFocus : MonoBehaviour
	{
		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06004C33 RID: 19507 RVA: 0x001DA44E File Offset: 0x001D864E
		public static bool BlocksGameplayInput
		{
			get
			{
				if (GameplayInputGate.BlocksGameplayInput)
				{
					return true;
				}
				if (TextInputFocus.HasFocusedField)
				{
					TextInputFocus._lastFocusedFrame = Time.frameCount;
				}
				return TextInputFocus._lastFocusedFrame == Time.frameCount;
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06004C34 RID: 19508 RVA: 0x001DA478 File Offset: 0x001D8678
		public static bool HasFocusedField
		{
			get
			{
				EventSystem current = EventSystem.current;
				GameObject gameObject = ((current != null && current.isActiveAndEnabled) ? current.currentSelectedGameObject : null);
				if (TextInputFocus._selection != gameObject)
				{
					TextInputFocus._selection = gameObject;
					TextInputFocus._tmpField = ((gameObject != null) ? gameObject.GetComponentInParent<TMP_InputField>() : null);
					TextInputFocus._legacyField = ((gameObject != null) ? gameObject.GetComponentInParent<InputField>() : null);
				}
				return (TextInputFocus._tmpField != null && TextInputFocus._tmpField.isActiveAndEnabled && TextInputFocus._tmpField.IsInteractable() && TextInputFocus._tmpField.isFocused) || (TextInputFocus._legacyField != null && TextInputFocus._legacyField.isActiveAndEnabled && TextInputFocus._legacyField.IsInteractable() && TextInputFocus._legacyField.isFocused);
			}
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x001DA54B File Offset: 0x001D874B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRuntimeState()
		{
			TextInputFocus._selection = null;
			TextInputFocus._tmpField = null;
			TextInputFocus._legacyField = null;
			TextInputFocus._lastFocusedFrame = -1;
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x001DA565 File Offset: 0x001D8765
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Bootstrap()
		{
			if (Object.FindObjectOfType<TextInputFocus>() != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[TextInputFocus]");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<TextInputFocus>();
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x001DA58B File Offset: 0x001D878B
		private void Update()
		{
			this.SetPointerOnly(GameplayInputGate.BlocksGameplayInput);
			if (TextInputFocus.HasFocusedField)
			{
				TextInputFocus._lastFocusedFrame = Time.frameCount;
			}
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x001DA5AC File Offset: 0x001D87AC
		private void SetPointerOnly(bool pointerOnly)
		{
			if (!pointerOnly)
			{
				foreach (KeyValuePair<EventSystem, bool> keyValuePair in this._navigationStates)
				{
					if (keyValuePair.Key != null)
					{
						keyValuePair.Key.sendNavigationEvents = keyValuePair.Value;
					}
				}
				this._navigationStates.Clear();
				return;
			}
			EventSystem current = EventSystem.current;
			if (current == null)
			{
				return;
			}
			if (!this._navigationStates.ContainsKey(current))
			{
				this._navigationStates.Add(current, current.sendNavigationEvents);
			}
			current.sendNavigationEvents = false;
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x001DA660 File Offset: 0x001D8860
		private void OnDisable()
		{
			this.SetPointerOnly(false);
		}

		// Token: 0x04004DC0 RID: 19904
		private static GameObject _selection;

		// Token: 0x04004DC1 RID: 19905
		private static TMP_InputField _tmpField;

		// Token: 0x04004DC2 RID: 19906
		private static InputField _legacyField;

		// Token: 0x04004DC3 RID: 19907
		private static int _lastFocusedFrame = -1;

		// Token: 0x04004DC4 RID: 19908
		private readonly Dictionary<EventSystem, bool> _navigationStates = new Dictionary<EventSystem, bool>();
	}
}
