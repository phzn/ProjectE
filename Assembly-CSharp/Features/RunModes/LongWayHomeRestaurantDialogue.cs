using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Features.RunModes
{
	// Token: 0x02000CDD RID: 3293
	[DisallowMultipleComponent]
	public sealed class LongWayHomeRestaurantDialogue : MonoBehaviour
	{
		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x06005FA5 RID: 24485 RVA: 0x0025E247 File Offset: 0x0025C447
		// (set) Token: 0x06005FA6 RID: 24486 RVA: 0x0025E24F File Offset: 0x0025C44F
		public bool IsRunning { get; private set; }

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06005FA7 RID: 24487 RVA: 0x0025E258 File Offset: 0x0025C458
		// (set) Token: 0x06005FA8 RID: 24488 RVA: 0x0025E260 File Offset: 0x0025C460
		public bool LastDialogueCompleted { get; private set; }

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06005FA9 RID: 24489 RVA: 0x0025E269 File Offset: 0x0025C469
		// (set) Token: 0x06005FAA RID: 24490 RVA: 0x0025E271 File Offset: 0x0025C471
		public string ReactionSpeaker { get; private set; }

		// Token: 0x06005FAB RID: 24491 RVA: 0x0025E27C File Offset: 0x0025C47C
		public IEnumerator PlayTableConversation(Action onDrinkingWarning = null)
		{
			return this.PlayLines(new DialogueLine[]
			{
				LongWayHomeRestaurantDialogue.Line("Haru", "Lost my Valorant ranked yesterday. Some guy kept trolling."),
				LongWayHomeRestaurantDialogue.Line("Haruto", "Sounds about right."),
				LongWayHomeRestaurantDialogue.Line("Haru", "Ciel, slow down. We have class tomorrow!")
			}, 2, onDrinkingWarning);
		}

		// Token: 0x06005FAC RID: 24492 RVA: 0x0025E2D0 File Offset: 0x0025C4D0
		public IEnumerator PlayUndressingReaction()
		{
			this.ReactionSpeaker = ((Random.Range(0, 2) == 0) ? "Haru" : "Haruto");
			string text = ((Random.Range(0, 2) == 0) ? "When did she take her clothes off? Nice boobs!" : "When did she take her clothes off? Nice ass!");
			return this.PlayLines(new DialogueLine[] { LongWayHomeRestaurantDialogue.Line(this.ReactionSpeaker, text) }, -1, null);
		}

		// Token: 0x06005FAD RID: 24493 RVA: 0x0025E32B File Offset: 0x0025C52B
		public IEnumerator PlayGymShowerConfrontation(string photographer)
		{
			return this.PlayLines(new DialogueLine[]
			{
				LongWayHomeRestaurantDialogue.Line(string.IsNullOrWhiteSpace(photographer) ? "???" : photographer, "Hey, she got the wrong locker room... This is the men's!"),
				LongWayHomeRestaurantDialogue.Line("Ciel", ". . .")
			}, -1, null);
		}

		// Token: 0x06005FAE RID: 24494 RVA: 0x0025E36A File Offset: 0x0025C56A
		private static DialogueLine Line(string speaker, string text)
		{
			return new DialogueLine
			{
				speakerName = speaker,
				text = text,
				localizationScope = "story"
			};
		}

		// Token: 0x06005FAF RID: 24495 RVA: 0x0025E38A File Offset: 0x0025C58A
		private IEnumerator PlayLines(DialogueLine[] lines, int cueLineIndex = -1, Action onLineStarted = null)
		{
			LongWayHomeRestaurantDialogue.<>c__DisplayClass30_0 CS$<>8__locals1 = new LongWayHomeRestaurantDialogue.<>c__DisplayClass30_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.cueLineIndex = cueLineIndex;
			CS$<>8__locals1.onLineStarted = onLineStarted;
			if (!base.isActiveAndEnabled || this.IsRunning)
			{
				Debug.LogWarning("[LongWayHomeRestaurantDialogue] Dialogue is already running or the helper is disabled.", this);
				yield break;
			}
			LongWayHomeRestaurantDialogue.<>c__DisplayClass30_0 CS$<>8__locals2 = CS$<>8__locals1;
			int num = this._generation + 1;
			this._generation = num;
			CS$<>8__locals2.generation = num;
			this.IsRunning = true;
			this.LastDialogueCompleted = false;
			this._dialogueCompleted = false;
			try
			{
				LongWayHomeRestaurantDialogue.<>c__DisplayClass30_1 CS$<>8__locals3 = new LongWayHomeRestaurantDialogue.<>c__DisplayClass30_1();
				CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
				float deadline = Time.realtimeSinceStartup + this.readinessTimeout;
				while (this.IsCurrent(CS$<>8__locals3.CS$<>8__locals1.generation))
				{
					SimpleDialogueManager simpleDialogueManager = LongWayHomeRestaurantDialogue.ResolveManager();
					if (simpleDialogueManager != null && simpleDialogueManager.isActiveAndEnabled && simpleDialogueManager.DialoguePanelVisual != null && simpleDialogueManager.DialogueTextVisual != null && !simpleDialogueManager.IsDialogueActive && PlayerActivityGate.TryAcquire("LongWayHomeRestaurantDialogue#" + base.GetInstanceID().ToString(), this, PlayerActivity.None, PlayerContextResource.ModalUI, PlayerActivity.AllPlayerActions, out this._lease, false, null))
					{
						this._manager = simpleDialogueManager;
						break;
					}
					if (Time.realtimeSinceStartup >= deadline)
					{
						Debug.LogError("[LongWayHomeRestaurantDialogue] The existing dialogue UI or ModalUI resource is unavailable.", this);
						yield break;
					}
					yield return null;
				}
				if (!this.IsCurrent(CS$<>8__locals3.CS$<>8__locals1.generation) || this._manager == null)
				{
					yield break;
				}
				this.RevealExistingPresentation();
				SimpleDialogueManager simpleDialogueManager2 = this._manager;
				if (simpleDialogueManager2.OnDialogueStarted == null)
				{
					simpleDialogueManager2.OnDialogueStarted = new UnityEvent();
				}
				this._manager.OnDialogueStarted.AddListener(new UnityAction(this.OnDialogueStarted));
				CS$<>8__locals3.dialogueOwner = (this._dialogueOwner = new object());
				if (CS$<>8__locals3.CS$<>8__locals1.onLineStarted != null)
				{
					CS$<>8__locals3.lineIndex = -1;
					CS$<>8__locals3.cueInvoked = false;
					CS$<>8__locals3.manager = this._manager;
					this._lineAdvancedListener = delegate
					{
						if (((!CS$<>8__locals3.CS$<>8__locals1.<>4__this.IsCurrent(CS$<>8__locals3.CS$<>8__locals1.generation) || !CS$<>8__locals3.CS$<>8__locals1.<>4__this._ownsDialogue) | CS$<>8__locals3.cueInvoked) || !CS$<>8__locals3.manager.IsDialogueOwnedBy(CS$<>8__locals3.dialogueOwner))
						{
							return;
						}
						int num2 = CS$<>8__locals3.lineIndex + 1;
						CS$<>8__locals3.lineIndex = num2;
						if (num2 != CS$<>8__locals3.CS$<>8__locals1.cueLineIndex)
						{
							return;
						}
						CS$<>8__locals3.cueInvoked = true;
						CS$<>8__locals3.CS$<>8__locals1.onLineStarted();
					};
					simpleDialogueManager2 = this._manager;
					if (simpleDialogueManager2.OnLineAdvanced == null)
					{
						simpleDialogueManager2.OnLineAdvanced = new UnityEvent();
					}
					this._manager.OnLineAdvanced.AddListener(this._lineAdvancedListener);
				}
				this._ownsDialogue = true;
				this._manager.StartOwnedDialogue(CS$<>8__locals3.dialogueOwner, lines, delegate
				{
					if (CS$<>8__locals3.CS$<>8__locals1.<>4__this.IsCurrent(CS$<>8__locals3.CS$<>8__locals1.generation) && CS$<>8__locals3.CS$<>8__locals1.<>4__this._ownsDialogue)
					{
						CS$<>8__locals3.CS$<>8__locals1.<>4__this._dialogueCompleted = true;
					}
				});
				while (this.IsCurrent(CS$<>8__locals3.CS$<>8__locals1.generation) && this._ownsDialogue && !this._dialogueCompleted && this._manager != null && this._manager.isActiveAndEnabled && this._manager.IsDialogueActive)
				{
					yield return null;
				}
				if (this.IsCurrent(CS$<>8__locals3.CS$<>8__locals1.generation))
				{
					this.LastDialogueCompleted = this._dialogueCompleted;
				}
				CS$<>8__locals3 = null;
			}
			finally
			{
				if (CS$<>8__locals1.generation == this._generation)
				{
					this.ReleasePresentation(!this.LastDialogueCompleted);
					this.IsRunning = false;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06005FB0 RID: 24496 RVA: 0x0025E3AE File Offset: 0x0025C5AE
		private bool IsCurrent(int generation)
		{
			return generation == this._generation && this.IsRunning && base.isActiveAndEnabled;
		}

		// Token: 0x06005FB1 RID: 24497 RVA: 0x0025E3C9 File Offset: 0x0025C5C9
		private static SimpleDialogueManager ResolveManager()
		{
			if (SimpleDialogueManager.Instance != null)
			{
				return SimpleDialogueManager.Instance;
			}
			return Object.FindObjectOfType<SimpleDialogueManager>(true);
		}

		// Token: 0x06005FB2 RID: 24498 RVA: 0x0025E3E4 File Offset: 0x0025C5E4
		private void RevealExistingPresentation()
		{
			Transform transform = this._manager.DialoguePanelVisual.transform;
			foreach (Canvas canvas in transform.GetComponentsInParent<Canvas>(true))
			{
				this._canvases.Add(new LongWayHomeRestaurantDialogue.CanvasState
				{
					Canvas = canvas,
					Enabled = canvas.enabled
				});
				canvas.enabled = true;
			}
			foreach (CanvasGroup canvasGroup in transform.GetComponentsInParent<CanvasGroup>(true))
			{
				this._groups.Add(new LongWayHomeRestaurantDialogue.GroupState
				{
					Group = canvasGroup,
					Alpha = canvasGroup.alpha
				});
				canvasGroup.alpha = 1f;
			}
			this._audio = this._manager.GetComponent<AudioSource>();
			if (this._audio != null)
			{
				this._audioWasMuted = this._audio.mute;
				this._audio.mute = false;
			}
		}

		// Token: 0x06005FB3 RID: 24499 RVA: 0x0025E4DF File Offset: 0x0025C6DF
		private void OnDialogueStarted()
		{
			this._ownsDialogue = this._manager != null && this._manager.IsDialogueOwnedBy(this._dialogueOwner);
		}

		// Token: 0x06005FB4 RID: 24500 RVA: 0x0025E509 File Offset: 0x0025C709
		public void Abort()
		{
			this._generation++;
			this.LastDialogueCompleted = false;
			this.ReleasePresentation(true);
			this.IsRunning = false;
		}

		// Token: 0x06005FB5 RID: 24501 RVA: 0x0025E530 File Offset: 0x0025C730
		private void ReleasePresentation(bool endOwnedDialogue)
		{
			SimpleDialogueManager manager = this._manager;
			bool ownsDialogue = this._ownsDialogue;
			object dialogueOwner = this._dialogueOwner;
			this._ownsDialogue = false;
			this._dialogueOwner = null;
			try
			{
				if (manager != null)
				{
					UnityEvent onDialogueStarted = manager.OnDialogueStarted;
					if (onDialogueStarted != null)
					{
						onDialogueStarted.RemoveListener(new UnityAction(this.OnDialogueStarted));
					}
					if (this._lineAdvancedListener != null)
					{
						UnityEvent onLineAdvanced = manager.OnLineAdvanced;
						if (onLineAdvanced != null)
						{
							onLineAdvanced.RemoveListener(this._lineAdvancedListener);
						}
					}
					if (endOwnedDialogue && ownsDialogue)
					{
						manager.TryEndDialogue(dialogueOwner, true);
					}
				}
			}
			finally
			{
				this._manager = null;
				this._lineAdvancedListener = null;
				foreach (LongWayHomeRestaurantDialogue.CanvasState canvasState in this._canvases)
				{
					if (canvasState.Canvas != null)
					{
						canvasState.Canvas.enabled = canvasState.Enabled;
					}
				}
				this._canvases.Clear();
				foreach (LongWayHomeRestaurantDialogue.GroupState groupState in this._groups)
				{
					if (groupState.Group != null)
					{
						groupState.Group.alpha = groupState.Alpha;
					}
				}
				this._groups.Clear();
				if (this._audio != null)
				{
					this._audio.mute = this._audioWasMuted;
				}
				this._audio = null;
				PlayerActivityLease lease = this._lease;
				if (lease != null)
				{
					lease.Dispose();
				}
				this._lease = null;
			}
		}

		// Token: 0x06005FB6 RID: 24502 RVA: 0x0025E70C File Offset: 0x0025C90C
		private void OnDisable()
		{
			this.Abort();
		}

		// Token: 0x06005FB7 RID: 24503 RVA: 0x0025E70C File Offset: 0x0025C90C
		private void OnDestroy()
		{
			this.Abort();
		}

		// Token: 0x0400610C RID: 24844
		[Tooltip("Maximum wait for the existing dialogue UI to become available. Reading has no timeout.")]
		[SerializeField]
		[Min(1f)]
		private float readinessTimeout = 20f;

		// Token: 0x0400610D RID: 24845
		private readonly List<LongWayHomeRestaurantDialogue.CanvasState> _canvases = new List<LongWayHomeRestaurantDialogue.CanvasState>();

		// Token: 0x0400610E RID: 24846
		private readonly List<LongWayHomeRestaurantDialogue.GroupState> _groups = new List<LongWayHomeRestaurantDialogue.GroupState>();

		// Token: 0x0400610F RID: 24847
		private SimpleDialogueManager _manager;

		// Token: 0x04006110 RID: 24848
		private PlayerActivityLease _lease;

		// Token: 0x04006111 RID: 24849
		private AudioSource _audio;

		// Token: 0x04006112 RID: 24850
		private bool _audioWasMuted;

		// Token: 0x04006113 RID: 24851
		private bool _ownsDialogue;

		// Token: 0x04006114 RID: 24852
		private bool _dialogueCompleted;

		// Token: 0x04006115 RID: 24853
		private object _dialogueOwner;

		// Token: 0x04006116 RID: 24854
		private UnityAction _lineAdvancedListener;

		// Token: 0x04006117 RID: 24855
		private int _generation;

		// Token: 0x02000CDE RID: 3294
		private struct CanvasState
		{
			// Token: 0x0400611B RID: 24859
			public Canvas Canvas;

			// Token: 0x0400611C RID: 24860
			public bool Enabled;
		}

		// Token: 0x02000CDF RID: 3295
		private struct GroupState
		{
			// Token: 0x0400611D RID: 24861
			public CanvasGroup Group;

			// Token: 0x0400611E RID: 24862
			public float Alpha;
		}
	}
}
