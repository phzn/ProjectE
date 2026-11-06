using System;
using System.Linq;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000022 RID: 34
	public class InputActionReference : ScriptableObject
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000761C File Offset: 0x0000581C
		public InputActionAsset asset
		{
			get
			{
				return this.m_Asset;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00007624 File Offset: 0x00005824
		public InputAction action
		{
			get
			{
				if (this.m_Action == null)
				{
					if (this.m_Asset == null)
					{
						return null;
					}
					this.m_Action = this.m_Asset.FindAction(new Guid(this.m_ActionId));
				}
				return this.m_Action;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00007660 File Offset: 0x00005860
		public void Set(InputAction action)
		{
			if (action == null)
			{
				this.m_Asset = null;
				this.m_ActionId = null;
				return;
			}
			InputActionMap actionMap = action.actionMap;
			if (actionMap == null || actionMap.asset == null)
			{
				throw new InvalidOperationException(string.Format("Action '{0}' must be part of an InputActionAsset in order to be able to create an InputActionReference for it", action));
			}
			this.SetInternal(actionMap.asset, action);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000076B8 File Offset: 0x000058B8
		public void Set(InputActionAsset asset, string mapName, string actionName)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			if (string.IsNullOrEmpty(mapName))
			{
				throw new ArgumentNullException("mapName");
			}
			if (string.IsNullOrEmpty(actionName))
			{
				throw new ArgumentNullException("actionName");
			}
			InputActionMap inputActionMap = asset.FindActionMap(mapName, false);
			if (inputActionMap == null)
			{
				throw new ArgumentException(string.Format("No action map '{0}' in '{1}'", mapName, asset), "mapName");
			}
			InputAction inputAction = inputActionMap.FindAction(actionName, false);
			if (inputAction == null)
			{
				throw new ArgumentException(string.Format("No action '{0}' in map '{1}' of asset '{2}'", actionName, mapName, asset), "actionName");
			}
			this.SetInternal(asset, inputAction);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000774C File Offset: 0x0000594C
		private void SetInternal(InputActionAsset asset, InputAction action)
		{
			InputActionMap actionMap = action.actionMap;
			if (!asset.actionMaps.Contains(actionMap))
			{
				throw new ArgumentException(string.Format("Action '{0}' is not contained in asset '{1}'", action, asset), "action");
			}
			this.m_Asset = asset;
			this.m_ActionId = action.id.ToString();
			base.name = InputActionReference.GetDisplayName(action);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000077B8 File Offset: 0x000059B8
		public override string ToString()
		{
			try
			{
				InputAction action = this.action;
				return string.Concat(new string[]
				{
					this.m_Asset.name,
					":",
					action.actionMap.name,
					"/",
					action.name
				});
			}
			catch
			{
				if (this.m_Asset != null)
				{
					return this.m_Asset.name + ":" + this.m_ActionId;
				}
			}
			return base.ToString();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00007858 File Offset: 0x00005A58
		internal static string GetDisplayName(InputAction action)
		{
			string text;
			if (action == null)
			{
				text = null;
			}
			else
			{
				InputActionMap actionMap = action.actionMap;
				text = ((actionMap != null) ? actionMap.name : null);
			}
			if (!string.IsNullOrEmpty(text))
			{
				InputActionMap actionMap2 = action.actionMap;
				return ((actionMap2 != null) ? actionMap2.name : null) + "/" + action.name;
			}
			if (action == null)
			{
				return null;
			}
			return action.name;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000078B2 File Offset: 0x00005AB2
		internal string ToDisplayName()
		{
			if (!string.IsNullOrEmpty(base.name))
			{
				return base.name;
			}
			return InputActionReference.GetDisplayName(this.action);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000078D3 File Offset: 0x00005AD3
		public static implicit operator InputAction(InputActionReference reference)
		{
			if (reference == null)
			{
				return null;
			}
			return reference.action;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000078E0 File Offset: 0x00005AE0
		public static InputActionReference Create(InputAction action)
		{
			if (action == null)
			{
				return null;
			}
			InputActionReference inputActionReference = ScriptableObject.CreateInstance<InputActionReference>();
			inputActionReference.Set(action);
			return inputActionReference;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x000078F4 File Offset: 0x00005AF4
		internal static void ResetCachedAction()
		{
			Object[] array = Resources.FindObjectsOfTypeAll(typeof(InputActionReference));
			for (int i = 0; i < array.Length; i++)
			{
				((InputActionReference)array[i]).m_Action = null;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000792D File Offset: 0x00005B2D
		public InputAction ToInputAction()
		{
			return this.action;
		}

		// Token: 0x040000C7 RID: 199
		[SerializeField]
		internal InputActionAsset m_Asset;

		// Token: 0x040000C8 RID: 200
		[SerializeField]
		internal string m_ActionId;

		// Token: 0x040000C9 RID: 201
		[NonSerialized]
		private InputAction m_Action;
	}
}
