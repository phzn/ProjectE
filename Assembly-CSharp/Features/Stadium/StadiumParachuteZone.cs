using System;
using Features.Inventory;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C4B RID: 3147
	public sealed class StadiumParachuteZone : MonoBehaviour
	{
		// Token: 0x06005AD2 RID: 23250 RVA: 0x0023973C File Offset: 0x0023793C
		public StadiumParachuteLaunchPoint FindApproach(Vector3 position)
		{
			if (!this.Contains(position))
			{
				return null;
			}
			foreach (StadiumParachuteLaunchPoint stadiumParachuteLaunchPoint in this.launchPoints)
			{
				if (stadiumParachuteLaunchPoint != null && stadiumParachuteLaunchPoint.transform.IsChildOf(base.transform) && stadiumParachuteLaunchPoint.ContainsApproach(position))
				{
					return stadiumParachuteLaunchPoint;
				}
			}
			return null;
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06005AD3 RID: 23251 RVA: 0x00239795 File Offset: 0x00237995
		public StadiumParachuteController BoundPlayer
		{
			get
			{
				return this._bound;
			}
		}

		// Token: 0x06005AD4 RID: 23252 RVA: 0x002397A0 File Offset: 0x002379A0
		public bool Contains(Vector3 worldPosition)
		{
			return base.isActiveAndEnabled && new Bounds(this.areaCenter, this.areaSize).Contains(base.transform.InverseTransformPoint(worldPosition));
		}

		// Token: 0x06005AD5 RID: 23253 RVA: 0x002397DC File Offset: 0x002379DC
		private void Update()
		{
			if (Time.unscaledTime < this._nextBind)
			{
				return;
			}
			this._nextBind = Time.unscaledTime + 0.5f;
			Transform player = PlayerRegistry.Player;
			if (player == null)
			{
				return;
			}
			vThirdPersonController componentInChildren = player.GetComponentInChildren<vThirdPersonController>(true);
			if (componentInChildren == null || (this._bound != null && this._bound.gameObject == componentInChildren.gameObject))
			{
				return;
			}
			if (this._bound != null)
			{
				this._bound.Unbind(this);
			}
			this._bound = componentInChildren.GetComponent<StadiumParachuteController>();
			if (this._bound == null)
			{
				this._bound = componentInChildren.gameObject.AddComponent<StadiumParachuteController>();
			}
			this._bound.Bind(this);
		}

		// Token: 0x06005AD6 RID: 23254 RVA: 0x002398A1 File Offset: 0x00237AA1
		private void OnDisable()
		{
			if (this._bound != null)
			{
				this._bound.Unbind(this);
			}
			this._bound = null;
		}

		// Token: 0x04005BF8 RID: 23544
		public InventoryItemSO item;

		// Token: 0x04005BF9 RID: 23545
		public StadiumParachuteVisual canopyPrefab;

		// Token: 0x04005BFA RID: 23546
		public StadiumParachuteProfile profile;

		// Token: 0x04005BFB RID: 23547
		public InteractionConfigSO readyHud;

		// Token: 0x04005BFC RID: 23548
		public InteractionConfigSO flightHud;

		// Token: 0x04005BFD RID: 23549
		public AudioClip openingSound;

		// Token: 0x04005BFE RID: 23550
		public AudioClip windSound;

		// Token: 0x04005BFF RID: 23551
		public AudioClip landingSound;

		// Token: 0x04005C00 RID: 23552
		public Vector3 areaCenter = new Vector3(2.5f, 16f, 4.4f);

		// Token: 0x04005C01 RID: 23553
		public Vector3 areaSize = new Vector3(176f, 64f, 210f);

		// Token: 0x04005C02 RID: 23554
		public float fieldHeight = -7.4f;

		// Token: 0x04005C03 RID: 23555
		public StadiumParachuteLaunchPoint[] launchPoints = new StadiumParachuteLaunchPoint[0];

		// Token: 0x04005C04 RID: 23556
		private StadiumParachuteController _bound;

		// Token: 0x04005C05 RID: 23557
		private float _nextBind;
	}
}
