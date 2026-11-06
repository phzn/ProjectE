using System;
using Features.NPC;
using ProjectExhibitionist.CharacterCustomization.Core.Storage;
using UnityEngine;

namespace Features.Prison
{
	// Token: 0x02000DC9 RID: 3529
	public class PrisonCellmateGender : MonoBehaviour
	{
		// Token: 0x06006849 RID: 26697 RVA: 0x0029B73C File Offset: 0x0029993C
		private void Awake()
		{
			bool flag = NPCRosterSession.PopulationMode == NpcPopulationMode.FemaleOnly;
			if (this.maleCellmate != null)
			{
				this.maleCellmate.SetActive(!flag);
			}
			if (this.femaleCellmate != null)
			{
				this.femaleCellmate.SetActive(flag);
			}
			if (this.hideWhenFemale == null)
			{
				return;
			}
			foreach (GameObject gameObject in this.hideWhenFemale)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(!flag);
				}
			}
		}

		// Token: 0x04006AC2 RID: 27330
		[SerializeField]
		private GameObject maleCellmate;

		// Token: 0x04006AC3 RID: 27331
		[SerializeField]
		private GameObject femaleCellmate;

		// Token: 0x04006AC4 RID: 27332
		[Tooltip("Spenti quando il compagno di cella è femmina (stesso modello della detenuta seduta).")]
		[SerializeField]
		private GameObject[] hideWhenFemale;
	}
}
