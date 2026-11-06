using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200075D RID: 1885
	public class RubberStampExample : MonoBehaviour
	{
		// Token: 0x060030B1 RID: 12465 RVA: 0x00112D1B File Offset: 0x00110F1B
		public void Start()
		{
			this.UpdateStampPositions();
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x00112D24 File Offset: 0x00110F24
		public void ChangeStamp(int index)
		{
			if (index > this.StampPrefabs.Length)
			{
				return;
			}
			Vector3 localPosition = this.currentStamp.transform.localPosition;
			Vector3 localEulerAngles = this.currentStamp.transform.localEulerAngles;
			Object.Destroy(this.currentStamp);
			this.currentStamp = Object.Instantiate<GameObject>(this.StampPrefabs[index]);
			this.currentStamp.transform.parent = base.transform;
			this.currentStamp.transform.localPosition = localPosition;
			this.currentStamp.transform.localEulerAngles = localEulerAngles;
			this.currentStamp.GetComponent<MudStamp>().SetSurface(this.currentSurface);
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x00112DCB File Offset: 0x00110FCB
		public void ApplyStamp()
		{
			this.currentStamp.GetComponent<MudStamp>().SetSurface(this.currentSurface);
			this.currentStamp.GetComponent<MudStamp>().StampSurface();
			this.MoveStamp();
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x00112DFC File Offset: 0x00110FFC
		public void MoveStamp()
		{
			this.currentRow++;
			if (this.currentRow >= this.stampPositions.Count)
			{
				this.currentRow = 0;
				this.ClearStamps();
			}
			base.transform.position = this.stampPositions[this.currentRow];
		}

		// Token: 0x060030B5 RID: 12469 RVA: 0x00112E53 File Offset: 0x00111053
		public void ClearStamps()
		{
			DemoSceneController.instance.Clear();
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x00112E5F File Offset: 0x0011105F
		public void SetCurrentDMS(DynamicMudSurface surface)
		{
			this.currentSurface = surface;
			this.UpdateStampPositions();
			this.MoveStamp();
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x00112E74 File Offset: 0x00111074
		private void UpdateStampPositions()
		{
			GameObject[] array = Object.FindObjectsOfType<GameObject>();
			this.stampPositionObjects = new List<GameObject>();
			this.stampPositions = new List<Vector3>();
			foreach (GameObject gameObject in array)
			{
				if (gameObject.activeSelf && gameObject.name == "stampPosition")
				{
					this.stampPositionObjects.Add(gameObject);
					this.stampPositions.Add(gameObject.transform.position);
				}
			}
		}

		// Token: 0x04003124 RID: 12580
		public GameObject[] StampPrefabs;

		// Token: 0x04003125 RID: 12581
		[SerializeField]
		private DynamicMudSurface currentSurface;

		// Token: 0x04003126 RID: 12582
		[SerializeField]
		private GameObject currentStamp;

		// Token: 0x04003127 RID: 12583
		private List<GameObject> stampPositionObjects;

		// Token: 0x04003128 RID: 12584
		private List<Vector3> stampPositions;

		// Token: 0x04003129 RID: 12585
		private int currentRow;
	}
}
