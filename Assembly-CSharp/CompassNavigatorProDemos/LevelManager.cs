using System;
using System.Collections;
using CompassNavigatorPro;
using UnityEngine;
using UnityEngine.Events;

namespace CompassNavigatorProDemos
{
	// Token: 0x020002B1 RID: 689
	public class LevelManager : MonoBehaviour
	{
		// Token: 0x06001377 RID: 4983 RVA: 0x0007E603 File Offset: 0x0007C803
		private IEnumerator Start()
		{
			this.compass = CompassPro.instance;
			this.compass.OnPOIVisited.AddListener(new UnityAction<CompassProPOI>(this.OnPOIVisited));
			this.compass.OnMiniMapMouseClick.AddListener(new UnityAction<Vector3, int>(this.OnMiniMapClick));
			this.compass.OnPOIMiniMapIconMouseEnter.AddListener(new UnityAction<CompassProPOI>(this.OnPOIHover));
			this.compass.OnPOIMiniMapIconMouseExit.AddListener(new UnityAction<CompassProPOI>(this.OnPOIExit));
			this.compass.OnPOIMiniMapIconMouseClick.AddListener(new UnityAction<CompassProPOI, int>(this.OnPOIClick));
			this.compass.OnPOIEnterCircle.AddListener(new UnityAction<CompassProPOI>(this.OnPOIEnterCircle));
			this.compass.OnPOIExitCircle.AddListener(new UnityAction<CompassProPOI>(this.OnPOIExitCircle));
			WaitForSeconds w = new WaitForSeconds(0.5f);
			int num;
			for (int i = 1; i <= this.initialPoiCount; i = num + 1)
			{
				yield return w;
				this.AddRandomPOI();
				num = i;
			}
			yield break;
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0007E614 File Offset: 0x0007C814
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.B))
			{
				this.compass.POIShowBeacon(5f, 1.1f, 1f, new Color(1f, 1f, 0.25f), true);
			}
			if (Input.GetKey(KeyCode.Z))
			{
				this.compass.miniMapZoomLevel -= Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.X))
			{
				this.compass.miniMapZoomLevel += Time.deltaTime;
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				this.compass.showCompassBar = !this.compass.showCompassBar;
			}
			if (Input.GetKeyDown(KeyCode.M))
			{
				this.compass.showMiniMap = !this.compass.showMiniMap;
			}
			RaycastHit raycastHit;
			if (Input.GetKeyDown(KeyCode.V) && Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f)), out raycastHit))
			{
				this.compass.POIShowBeacon(raycastHit.point, 5f, 0.4f, 1f, new Color(0f, 0.5f, 1f), true);
			}
			if (Input.GetKeyDown(KeyCode.T))
			{
				this.compass.miniMapFullScreenState = !this.compass.miniMapFullScreenState;
			}
			if (Input.GetKeyDown(KeyCode.H))
			{
				foreach (CompassProPOI compassProPOI in this.compass.pois)
				{
					compassProPOI.StartCircleAnimation();
				}
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				this.compass.Scan(true).OnScanHit.AddListener(new UnityAction<ScanEffect, CompassProPOI, Transform>(this.OnScanHit));
			}
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0007E7E0 File Offset: 0x0007C9E0
		private void OnScanHit(ScanEffect scan, CompassProPOI poi, Transform hitTransform)
		{
			if (!poi.enabled)
			{
				poi.enabled = true;
			}
			poi.StartCircleAnimation();
			this.compass.POIShowBeacon(poi, 3f, 1f, true);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0007E810 File Offset: 0x0007CA10
		private void OnGUI()
		{
			if (this.POIUnderMouse != null)
			{
				Rect miniMapIconScreenRect = this.POIUnderMouse.GetMiniMapIconScreenRect();
				miniMapIconScreenRect = new Rect(miniMapIconScreenRect.center.x - 100f, (float)Screen.height - miniMapIconScreenRect.y - 85f, 200f, 25f);
				GUIStyle style = GUI.skin.GetStyle("Label");
				style.alignment = TextAnchor.UpperCenter;
				style.normal.textColor = Color.yellow;
				GUI.Label(miniMapIconScreenRect, this.POIUnderMouse.title, style);
			}
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0007E8A8 File Offset: 0x0007CAA8
		private void AddRandomPOI()
		{
			Vector3 vector = new Vector3((float)Random.Range(-50, 50), 1f, (float)Random.Range(-50, 50));
			this.AddPOI(vector);
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0007E8DC File Offset: 0x0007CADC
		private void AddPOI(Vector3 position)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.transform.position = position;
			gameObject.GetComponent<Renderer>().material = this.sphereMaterial;
			CompassProPOI compassProPOI = gameObject.AddComponent<CompassProPOI>();
			string text = "Target ";
			int num = this.poiNumber + 1;
			this.poiNumber = num;
			compassProPOI.title = text + num.ToString();
			compassProPOI.titleVisibility = TitleVisibility.Always;
			compassProPOI.visitedText = "Target " + this.poiNumber.ToString() + " acquired!";
			int num2 = Random.Range(0, this.icons.Length / 2);
			compassProPOI.iconNonVisited = this.icons[num2 * 2];
			compassProPOI.iconVisited = this.icons[num2 * 2 + 1];
			compassProPOI.showOnScreenIndicator = true;
			compassProPOI.showOffScreenIndicator = true;
			compassProPOI.hideWhenVisited = true;
			num2 = Random.Range(0, this.soundClips.Length);
			compassProPOI.visitedAudioClipOverride = this.soundClips[num2];
			compassProPOI.miniMapCircleAnimationWhenAppears = true;
			compassProPOI.miniMapCircleAnimationRepetitions = 1;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0007E9D1 File Offset: 0x0007CBD1
		private void OnPOIVisited(CompassProPOI poi)
		{
			Debug.Log(poi.title + " has been reached.");
			base.StartCoroutine(this.PickUpPOI(poi));
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0007E9F6 File Offset: 0x0007CBF6
		private IEnumerator PickUpPOI(CompassProPOI poi)
		{
			int num;
			for (int i = 0; i < 10; i = num + 1)
			{
				poi.transform.localScale *= 0.9f;
				yield return null;
				num = i;
			}
			Object.Destroy(poi.gameObject);
			this.AddRandomPOI();
			yield break;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0007EA0C File Offset: 0x0007CC0C
		private void OnPOIClick(CompassProPOI poi, int mouseButtonIndex)
		{
			Debug.Log(poi.title + " has been clicked on minimap.");
			if (mouseButtonIndex == 1)
			{
				Debug.Log("Removing POI...");
				Object.Destroy(poi.gameObject);
			}
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0007EA3C File Offset: 0x0007CC3C
		private void OnPOIHover(CompassProPOI poi)
		{
			this.POIUnderMouse = poi;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x0007EA45 File Offset: 0x0007CC45
		private void OnPOIExit(CompassProPOI poi)
		{
			this.POIUnderMouse = null;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0007EA4E File Offset: 0x0007CC4E
		private void OnMiniMapClick(Vector3 position, int buttonIndex)
		{
			if (buttonIndex == 0)
			{
				Debug.Log(string.Format("User clicked on mini-map. Creating a POI at world position: {0}", position));
				position.y = 1f;
				this.AddPOI(position);
				return;
			}
			this.compass.ResetDragOffset();
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0007EA87 File Offset: 0x0007CC87
		private void OnPOIEnterCircle(CompassProPOI poi)
		{
			Debug.Log("Entering circle of poi " + poi.title);
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0007EA9E File Offset: 0x0007CC9E
		private void OnPOIExitCircle(CompassProPOI poi)
		{
			Debug.Log("Exiting circle of poi " + poi.title);
		}

		// Token: 0x0400155C RID: 5468
		public int initialPoiCount = 1;

		// Token: 0x0400155D RID: 5469
		public Material sphereMaterial;

		// Token: 0x0400155E RID: 5470
		public Sprite[] icons;

		// Token: 0x0400155F RID: 5471
		public AudioClip[] soundClips;

		// Token: 0x04001560 RID: 5472
		private int poiNumber;

		// Token: 0x04001561 RID: 5473
		private CompassPro compass;

		// Token: 0x04001562 RID: 5474
		private CompassProPOI POIUnderMouse;
	}
}
