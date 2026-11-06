using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NPCs.Scripts
{
	// Token: 0x02000EC2 RID: 3778
	public class NPCActionSpot : MonoBehaviour
	{
		// Token: 0x0600711C RID: 28956 RVA: 0x002DED43 File Offset: 0x002DCF43
		public float GetRandomDuration()
		{
			return Random.Range(this.workDurationRange.x, this.workDurationRange.y);
		}

		// Token: 0x0600711D RID: 28957 RVA: 0x002DED60 File Offset: 0x002DCF60
		public string GetRandomAnimationParam()
		{
			if (this.animationBoolParams == null || this.animationBoolParams.Count == 0)
			{
				return "";
			}
			return this.animationBoolParams[Random.Range(0, this.animationBoolParams.Count)];
		}

		// Token: 0x0600711E RID: 28958 RVA: 0x002DED9C File Offset: 0x002DCF9C
		private void OnDrawGizmos()
		{
			Gizmos.color = this.gizmoColor;
			Gizmos.DrawWireSphere(base.transform.position, 0.3f);
			Gizmos.DrawLine(base.transform.position, base.transform.position + base.transform.forward * 0.5f);
		}

		// Token: 0x040073E3 RID: 29667
		[Header("Settings")]
		[Tooltip("Chi può usare questo punto?")]
		public NPCRole allowedRole;

		// Token: 0x040073E4 RID: 29668
		[Tooltip("Quanto tempo l'NPC deve stare qui a lavorare (Min/Max secondi).")]
		public Vector2 workDurationRange = new Vector2(5f, 10f);

		// Token: 0x040073E5 RID: 29669
		[Header("Animations")]
		[Tooltip("Lista dei nomi dei parametri BOOL dell'Animator da attivare qui. Ne verrà scelto uno a caso.")]
		public List<string> animationBoolParams = new List<string>();

		// Token: 0x040073E6 RID: 29670
		[Header("Props")]
		[Tooltip("Lista dei nomi dei GameObject figli dell'NPC da attivare durante l'azione (es. 'Broom', 'Dustpan').")]
		public List<string> propsToEnable = new List<string>();

		// Token: 0x040073E7 RID: 29671
		[Header("Settings")]
		[Tooltip("Se true, attende la fine del ciclo di animazione corrente prima di fermarsi, anche se il tempo è scaduto.")]
		public bool finishAnimationLoop = true;

		// Token: 0x040073E8 RID: 29672
		[Header("Debug")]
		[Tooltip("Colore del Gizmo per identificarlo nella scena.")]
		public Color gizmoColor = Color.yellow;
	}
}
