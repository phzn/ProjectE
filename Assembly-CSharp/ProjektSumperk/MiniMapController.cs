using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x020003F7 RID: 1015
	public class MiniMapController : MonoBehaviour
	{
		// Token: 0x060019B9 RID: 6585 RVA: 0x000A30C8 File Offset: 0x000A12C8
		private void Start()
		{
			this.miniMapScale = new Vector3(this.miniMap.localScale.x / 10f, this.miniMap.localScale.z / 10f, 1f);
			this.buildingDots = new List<GameObject>();
			foreach (Transform transform in this.buildings)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.buildingDotPrefab, this.miniMap);
				this.buildingDots.Add(gameObject);
				this.UpdateMiniMapPosition(transform, gameObject.transform);
			}
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x000A3188 File Offset: 0x000A1388
		private void Update()
		{
			this.UpdateMiniMapPosition(this.player, this.playerMarker.transform);
			this.miniMap.localScale = new Vector3(this.miniMapSize, this.miniMapSize, this.miniMapSize);
			for (int i = 0; i < this.buildings.Count; i++)
			{
				if (!this.buildings[i].gameObject.activeSelf)
				{
					Object.Destroy(this.buildingDots[i]);
					this.buildingDots.RemoveAt(i);
					this.buildings.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x000A322C File Offset: 0x000A142C
		private void UpdateMiniMapPosition(Transform target, Transform icon)
		{
			Vector3 position = target.position;
			Vector3 vector = new Vector3((position.x - this.miniMap.position.x) * this.miniMapScale.x / this.miniMapScaleModifier, (position.z - this.miniMap.position.z) * this.miniMapScale.y / this.miniMapScaleModifier, 0f);
			icon.localPosition = vector;
			icon.localRotation = Quaternion.Euler(0f, 0f, -target.rotation.eulerAngles.y);
		}

		// Token: 0x04001AE8 RID: 6888
		public Transform player;

		// Token: 0x04001AE9 RID: 6889
		public List<Transform> buildings;

		// Token: 0x04001AEA RID: 6890
		public Image playerMarker;

		// Token: 0x04001AEB RID: 6891
		public GameObject buildingDotPrefab;

		// Token: 0x04001AEC RID: 6892
		public Transform miniMap;

		// Token: 0x04001AED RID: 6893
		public float miniMapScaleModifier = 1f;

		// Token: 0x04001AEE RID: 6894
		private Vector3 miniMapScale;

		// Token: 0x04001AEF RID: 6895
		[Range(1f, 5f)]
		public float miniMapSize = 1f;

		// Token: 0x04001AF0 RID: 6896
		private List<GameObject> buildingDots;
	}
}
