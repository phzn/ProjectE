using System;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A5B RID: 2651
	public static class GameplayInput
	{
		// Token: 0x06004B72 RID: 19314 RVA: 0x001D57CD File Offset: 0x001D39CD
		public static bool GetKey(KeyCode key)
		{
			return !GameplayInputGate.BlocksGameplayInput && (Input.GetKey(key) || ControllerGameplayBridge.Read(key, 0));
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x001D57EC File Offset: 0x001D39EC
		public static bool GetKey(string key)
		{
			KeyCode keyCode;
			return !GameplayInputGate.BlocksGameplayInput && (Input.GetKey(key) || (ControllerGameplayBridge.TryKey(key, out keyCode) && ControllerGameplayBridge.Read(keyCode, 0)));
		}

		// Token: 0x06004B74 RID: 19316 RVA: 0x001D581F File Offset: 0x001D3A1F
		public static bool GetKeyDown(KeyCode key)
		{
			return !GameplayInputGate.BlocksGameplayInput && (Input.GetKeyDown(key) || ControllerGameplayBridge.Read(key, 1));
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x001D583C File Offset: 0x001D3A3C
		public static bool GetKeyDown(string key)
		{
			KeyCode keyCode;
			return !GameplayInputGate.BlocksGameplayInput && (Input.GetKeyDown(key) || (ControllerGameplayBridge.TryKey(key, out keyCode) && ControllerGameplayBridge.Read(keyCode, 1)));
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x001D586F File Offset: 0x001D3A6F
		public static bool GetKeyUp(KeyCode key)
		{
			return !GameplayInputGate.BlocksGameplayInput && (Input.GetKeyUp(key) || ControllerGameplayBridge.Read(key, 2));
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x001D588C File Offset: 0x001D3A8C
		public static bool GetKeyUp(string key)
		{
			KeyCode keyCode;
			return !GameplayInputGate.BlocksGameplayInput && (Input.GetKeyUp(key) || (ControllerGameplayBridge.TryKey(key, out keyCode) && ControllerGameplayBridge.Read(keyCode, 2)));
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x001D58BF File Offset: 0x001D3ABF
		public static bool GetButton(string name)
		{
			return !GameplayInputGate.BlocksGameplayInput && Input.GetButton(name);
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x001D58D0 File Offset: 0x001D3AD0
		public static bool GetButtonDown(string name)
		{
			return !GameplayInputGate.BlocksGameplayInput && Input.GetButtonDown(name);
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x001D58E1 File Offset: 0x001D3AE1
		public static bool GetButtonUp(string name)
		{
			return !GameplayInputGate.BlocksGameplayInput && Input.GetButtonUp(name);
		}

		// Token: 0x06004B7B RID: 19323 RVA: 0x001D58F2 File Offset: 0x001D3AF2
		public static float GetAxis(string name)
		{
			if (!GameplayInputGate.BlocksGameplayInput)
			{
				return GameplayInput.CombineAxis(name, Input.GetAxis(name));
			}
			return 0f;
		}

		// Token: 0x06004B7C RID: 19324 RVA: 0x001D590D File Offset: 0x001D3B0D
		public static float GetAxisRaw(string name)
		{
			if (!GameplayInputGate.BlocksGameplayInput)
			{
				return GameplayInput.CombineAxis(name, Input.GetAxisRaw(name));
			}
			return 0f;
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x001D5928 File Offset: 0x001D3B28
		private static float CombineAxis(string name, float keyboard)
		{
			if (!ControllerInput.IsActive || !ControllerGameplayBridge.AllowsAxes)
			{
				return keyboard;
			}
			return ControllerInput.GetAxis(name);
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x001D5940 File Offset: 0x001D3B40
		public static bool GetMouseButton(int button)
		{
			return !GameplayInputGate.BlocksGameplayInput && (Input.GetMouseButton(button) || (button == 0 && ControllerGameplayBridge.Read(KeyCode.Mouse0, 0)));
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x001D5965 File Offset: 0x001D3B65
		public static bool GetMouseButtonDown(int button)
		{
			return !GameplayInputGate.BlocksGameplayInput && (Input.GetMouseButtonDown(button) || (button == 0 && ControllerGameplayBridge.Read(KeyCode.Mouse0, 1)));
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x001D598A File Offset: 0x001D3B8A
		public static bool GetMouseButtonUp(int button)
		{
			return !GameplayInputGate.BlocksGameplayInput && (Input.GetMouseButtonUp(button) || (button == 0 && ControllerGameplayBridge.Read(KeyCode.Mouse0, 2)));
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06004B81 RID: 19329 RVA: 0x001D59AF File Offset: 0x001D3BAF
		public static bool anyKeyDown
		{
			get
			{
				return !GameplayInputGate.BlocksGameplayInput && (Input.anyKeyDown || (ControllerGameplayBridge.AllowsAxes && ControllerInput.SubmitDown));
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06004B82 RID: 19330 RVA: 0x001D59D1 File Offset: 0x001D3BD1
		public static Vector2 mouseScrollDelta
		{
			get
			{
				if (GameplayInputGate.BlocksGameplayInput)
				{
					return Vector2.zero;
				}
				if (!ControllerInput.IsActive || !ControllerGameplayBridge.AllowsAxes)
				{
					return Input.mouseScrollDelta;
				}
				return new Vector2(0f, ControllerInput.Scroll * 10f);
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06004B83 RID: 19331 RVA: 0x001D5A09 File Offset: 0x001D3C09
		public static Vector3 mousePosition
		{
			get
			{
				return Input.mousePosition;
			}
		}
	}
}
