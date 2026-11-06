using System;
using Features.Detection;
using UnityEngine;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E26 RID: 3622
	public class PlayerActionEmoteRuntimeService : MonoBehaviour
	{
		// Token: 0x06006C12 RID: 27666 RVA: 0x002B3FD8 File Offset: 0x002B21D8
		private void Awake()
		{
			if (this.playerActionAnimator == null)
			{
				this.playerActionAnimator = base.GetComponent<PlayerActionAnimator>();
			}
			if (this.playerActionAnimator == null)
			{
				this.playerActionAnimator = base.GetComponentInChildren<PlayerActionAnimator>();
			}
			if (this.playerActionAnimator == null)
			{
				this.playerActionAnimator = base.GetComponentInParent<PlayerActionAnimator>();
			}
			if (this.emotePropHandler == null)
			{
				this.emotePropHandler = base.GetComponent<EmotePropHandler>();
			}
			if (this.emotePropHandler == null)
			{
				this.emotePropHandler = base.GetComponentInChildren<EmotePropHandler>();
			}
			if (this.emotePropHandler == null)
			{
				this.emotePropHandler = base.GetComponentInParent<EmotePropHandler>();
			}
		}

		// Token: 0x06006C13 RID: 27667 RVA: 0x002B4084 File Offset: 0x002B2284
		public float TriggerAction(PlayerActionEntry actionEntry, EmoteDefinitionSO emoteDefinition = null, bool startAutoPropTimers = true)
		{
			if (actionEntry == null)
			{
				return 0f;
			}
			if (emoteDefinition != null && this.emotePropHandler != null)
			{
				this.emotePropHandler.SetupProps(emoteDefinition);
				if (startAutoPropTimers)
				{
					this.emotePropHandler.StartAutoTimers();
				}
			}
			else if (emoteDefinition != null && this.debugLogs)
			{
				Debug.LogWarning("[PlayerActionEmoteRuntimeService] EmoteDefinition provided but EmotePropHandler is missing. Props will not be available for AE_EmoteProp events.");
			}
			float num = 0f;
			if (this.playerActionAnimator != null)
			{
				if (this.debugLogs && actionEntry.TriggerPrefab != null && !actionEntry.TriggerPrefab.endActionManualy)
				{
					Debug.LogWarning(string.Format("[PlayerActionEmoteRuntimeService] Action '{0}' uses trigger '{1}' with endActionManualy=false. The animation can end before configured Duration ({2:0.00}s).", actionEntry.ActionId, actionEntry.TriggerPrefab.name, actionEntry.Duration));
				}
				num = this.playerActionAnimator.TriggerAction(actionEntry);
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning("[PlayerActionEmoteRuntimeService] PlayerActionAnimator is missing. Action trigger skipped.");
			}
			if (num > 0f)
			{
				float num2 = this.ResolveConfiguredDuration(actionEntry, emoteDefinition);
				return Mathf.Max(num, num2);
			}
			return this.ResolveConfiguredDuration(actionEntry, emoteDefinition);
		}

		// Token: 0x06006C14 RID: 27668 RVA: 0x002B418C File Offset: 0x002B238C
		private float ResolveConfiguredDuration(PlayerActionEntry actionEntry, EmoteDefinitionSO emoteDefinition)
		{
			float num = ((actionEntry != null) ? Mathf.Max(0f, actionEntry.Duration) : 0f);
			float num2 = ((emoteDefinition != null) ? Mathf.Max(0f, emoteDefinition.Duration) : 0f);
			if (this.debugLogs && actionEntry != null && emoteDefinition != null && Mathf.Abs(num - num2) > 0.01f)
			{
				Debug.LogWarning(string.Format("[PlayerActionEmoteRuntimeService] Duration mismatch for action '{0}': PlayerActionEntry={1:0.00}s, EmoteDefinition={2:0.00}s. Using max value.", actionEntry.ActionId, num, num2));
			}
			return Mathf.Max(num, num2);
		}

		// Token: 0x06006C15 RID: 27669 RVA: 0x002B4220 File Offset: 0x002B2420
		public void FinishAction(bool cleanupProps)
		{
			if (this.playerActionAnimator != null)
			{
				this.playerActionAnimator.FinishCurrentAction();
				this.playerActionAnimator.RestoreOverrideLayer();
			}
			if (cleanupProps && this.emotePropHandler != null)
			{
				this.emotePropHandler.CleanupAllProps();
			}
		}

		// Token: 0x04006E93 RID: 28307
		[Header("References")]
		[SerializeField]
		private PlayerActionAnimator playerActionAnimator;

		// Token: 0x04006E94 RID: 28308
		[SerializeField]
		private EmotePropHandler emotePropHandler;

		// Token: 0x04006E95 RID: 28309
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;
	}
}
