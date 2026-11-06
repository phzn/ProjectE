using System;
using Features.Detection;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C3D RID: 3133
	public class StadiumCommentaryController : MonoBehaviour
	{
		// Token: 0x06005A56 RID: 23126 RVA: 0x00235E75 File Offset: 0x00234075
		private void OnEnable()
		{
			this._nextFireTime = (this.fireImmediately ? Time.time : (Time.time + this.intervalSeconds));
		}

		// Token: 0x06005A57 RID: 23127 RVA: 0x00235E98 File Offset: 0x00234098
		private void OnDisable()
		{
			SimpleDialogueManager instance = SimpleDialogueManager.Instance;
			if (instance != null && instance.IsDialogueActive && instance.IsAutoDialogue)
			{
				instance.ForceEndDialogue();
			}
		}

		// Token: 0x06005A58 RID: 23128 RVA: 0x00235ECC File Offset: 0x002340CC
		private void Update()
		{
			if (StadiumCommentaryController.IsDirectorBusy())
			{
				SimpleDialogueManager instance = SimpleDialogueManager.Instance;
				if (instance != null && instance.IsDialogueActive && instance.IsAutoDialogue)
				{
					instance.ForceEndDialogue();
				}
				this._nextFireTime = Time.time + this.intervalSeconds;
				return;
			}
			if (Time.time < this._nextFireTime)
			{
				return;
			}
			this._nextFireTime = Time.time + this.intervalSeconds;
			this.FireLine();
		}

		// Token: 0x06005A59 RID: 23129 RVA: 0x00235F40 File Offset: 0x00234140
		private static bool IsDirectorBusy()
		{
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			return instance != null && instance.IsEventActive;
		}

		// Token: 0x06005A5A RID: 23130 RVA: 0x00235F64 File Offset: 0x00234164
		private void FireLine()
		{
			if (this.lines == null || this.lines.lines == null || this.lines.lines.Length == 0)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[StadiumCommentaryController] Nessuna frase configurata.", this);
				}
				return;
			}
			SimpleDialogueManager instance = SimpleDialogueManager.Instance;
			if (instance == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[StadiumCommentaryController] SimpleDialogueManager non in scena.", this);
				}
				return;
			}
			if (instance.IsDialogueActive)
			{
				if (this.debugLogs)
				{
					Debug.Log("[StadiumCommentaryController] Dialogo già attivo, salto questo tick.", this);
				}
				return;
			}
			string text = this.PickRandomLine();
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			DialogueLine dialogueLine = new DialogueLine
			{
				speakerName = this.speakerName,
				text = text,
				localizationScope = "stadium",
				speakerLocalizationScope = "dialogue_speaker"
			};
			instance.StartAutoDialogue(new DialogueLine[] { dialogueLine }, this.dwellDuration, null);
			if (this.debugLogs)
			{
				Debug.Log("[StadiumCommentaryController] \"" + text + "\"", this);
			}
		}

		// Token: 0x06005A5B RID: 23131 RVA: 0x00236064 File Offset: 0x00234264
		private string PickRandomLine()
		{
			string[] array = this.lines.lines;
			if (array.Length == 1)
			{
				return array[0];
			}
			int num = Random.Range(0, array.Length);
			if (num == this._lastIndex)
			{
				num = (num + 1) % array.Length;
			}
			this._lastIndex = num;
			return array[num];
		}

		// Token: 0x04005B4B RID: 23371
		[Header("Lines")]
		[Tooltip("Pool di frasi pescate a random a ogni tick.")]
		[SerializeField]
		private StadiumCommentaryLinesSO lines;

		// Token: 0x04005B4C RID: 23372
		[Tooltip("Nome speaker mostrato sopra la frase. Lascia vuoto per nascondere.")]
		[SerializeField]
		private string speakerName = "Commentator";

		// Token: 0x04005B4D RID: 23373
		[Header("Timing")]
		[Tooltip("Secondi tra una frase e la successiva.")]
		[Min(1f)]
		[SerializeField]
		private float intervalSeconds = 30f;

		// Token: 0x04005B4E RID: 23374
		[Tooltip("Se true, mostra la prima frase subito all'ingresso in campo; altrimenti aspetta un intero intervallo.")]
		[SerializeField]
		private bool fireImmediately = true;

		// Token: 0x04005B4F RID: 23375
		[Tooltip("Secondi di permanenza della frase dopo che la typewriter ha finito.")]
		[Min(0f)]
		[SerializeField]
		private float dwellDuration = 3.5f;

		// Token: 0x04005B50 RID: 23376
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005B51 RID: 23377
		private float _nextFireTime;

		// Token: 0x04005B52 RID: 23378
		private int _lastIndex = -1;
	}
}
