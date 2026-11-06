using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core
{
	// Token: 0x02000AD2 RID: 2770
	public class CustomizationCoreOrchestrator : MonoBehaviour
	{
		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06004E91 RID: 20113 RVA: 0x001E7DB1 File Offset: 0x001E5FB1
		// (set) Token: 0x06004E92 RID: 20114 RVA: 0x001E7DB9 File Offset: 0x001E5FB9
		public CustomizationSessionContext CurrentSession { get; private set; }

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06004E93 RID: 20115 RVA: 0x001E7DC2 File Offset: 0x001E5FC2
		// (set) Token: 0x06004E94 RID: 20116 RVA: 0x001E7DCA File Offset: 0x001E5FCA
		public ICharacterCustomizationBackend ActiveBackend { get; private set; }

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06004E95 RID: 20117 RVA: 0x001E7DD4 File Offset: 0x001E5FD4
		// (remove) Token: 0x06004E96 RID: 20118 RVA: 0x001E7E0C File Offset: 0x001E600C
		public event Action<CustomizationSessionContext, ICharacterCustomizationBackend> OnSessionInitialized;

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06004E97 RID: 20119 RVA: 0x001E7E44 File Offset: 0x001E6044
		// (remove) Token: 0x06004E98 RID: 20120 RVA: 0x001E7E7C File Offset: 0x001E607C
		public event Action<CustomizationSessionContext> OnSessionClosed;

		// Token: 0x06004E99 RID: 20121 RVA: 0x001E7EB1 File Offset: 0x001E60B1
		private void Awake()
		{
			this.CacheBackends();
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x001E7EBC File Offset: 0x001E60BC
		public bool InitializeSession(CustomizationSessionContext context)
		{
			if (context == null)
			{
				Debug.LogError("[CustomizationCore] Cannot initialize session: context is null.");
				return false;
			}
			if (!context.IsValid())
			{
				Debug.LogError("[CustomizationCore] Cannot initialize session: context is invalid.");
				return false;
			}
			this.CloseSession();
			this.CacheBackends();
			for (int i = 0; i < this._backends.Count; i++)
			{
				ICharacterCustomizationBackend characterCustomizationBackend = this._backends[i];
				if (characterCustomizationBackend != null && characterCustomizationBackend.CanInitialize(context) && characterCustomizationBackend.Initialize(context))
				{
					this.ActiveBackend = characterCustomizationBackend;
					this.CurrentSession = context;
					if (this.verboseLogs)
					{
						Debug.Log("[CustomizationCore] Session initialized with backend: " + characterCustomizationBackend.BackendId);
					}
					Action<CustomizationSessionContext, ICharacterCustomizationBackend> onSessionInitialized = this.OnSessionInitialized;
					if (onSessionInitialized != null)
					{
						onSessionInitialized(context, characterCustomizationBackend);
					}
					return true;
				}
			}
			Debug.LogError(string.Format("[CustomizationCore] No backend can initialize target: {0}, characterId: {1}", context.targetType, context.characterId));
			return false;
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x001E7F94 File Offset: 0x001E6194
		public void CloseSession()
		{
			if (this.ActiveBackend == null)
			{
				this.CurrentSession = null;
				return;
			}
			CustomizationSessionContext currentSession = this.CurrentSession;
			this.ActiveBackend.Shutdown();
			this.ActiveBackend = null;
			this.CurrentSession = null;
			if (this.verboseLogs)
			{
				Debug.Log("[CustomizationCore] Session closed.");
			}
			Action<CustomizationSessionContext> onSessionClosed = this.OnSessionClosed;
			if (onSessionClosed == null)
			{
				return;
			}
			onSessionClosed(currentSession);
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x001E7FF4 File Offset: 0x001E61F4
		public bool TryGetBackend<TBackend>(out TBackend backend) where TBackend : class
		{
			TBackend tbackend = this.ActiveBackend as TBackend;
			if (tbackend != null)
			{
				backend = tbackend;
				return true;
			}
			backend = default(TBackend);
			return false;
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x001E802C File Offset: 0x001E622C
		private void CacheBackends()
		{
			this._backends.Clear();
			for (int i = 0; i < this.backendBehaviours.Count; i++)
			{
				ICharacterCustomizationBackend characterCustomizationBackend = this.backendBehaviours[i] as ICharacterCustomizationBackend;
				if (characterCustomizationBackend != null)
				{
					this._backends.Add(characterCustomizationBackend);
				}
			}
			if (this.autoCollectBackendsFromChildren)
			{
				MonoBehaviour[] componentsInChildren = base.GetComponentsInChildren<MonoBehaviour>(true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					ICharacterCustomizationBackend characterCustomizationBackend2 = componentsInChildren[j] as ICharacterCustomizationBackend;
					if (characterCustomizationBackend2 != null && !this._backends.Contains(characterCustomizationBackend2))
					{
						this._backends.Add(characterCustomizationBackend2);
					}
				}
			}
			if (this.verboseLogs)
			{
				Debug.Log(string.Format("[CustomizationCore] Cached backends: {0}", this._backends.Count));
			}
		}

		// Token: 0x04004FED RID: 20461
		[Header("Backends")]
		[SerializeField]
		private List<MonoBehaviour> backendBehaviours = new List<MonoBehaviour>();

		// Token: 0x04004FEE RID: 20462
		[SerializeField]
		private bool autoCollectBackendsFromChildren = true;

		// Token: 0x04004FEF RID: 20463
		[Header("Debug")]
		[SerializeField]
		private bool verboseLogs;

		// Token: 0x04004FF0 RID: 20464
		private readonly List<ICharacterCustomizationBackend> _backends = new List<ICharacterCustomizationBackend>();
	}
}
