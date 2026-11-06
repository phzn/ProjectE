using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.Inventory.Placement
{
	// Token: 0x02001147 RID: 4423
	public class PlacementBeamVisualizer : MonoBehaviour
	{
		// Token: 0x0600865C RID: 34396 RVA: 0x0036D37E File Offset: 0x0036B57E
		private void Awake()
		{
			this.CreateBeamMesh();
		}

		// Token: 0x0600865D RID: 34397 RVA: 0x0036D386 File Offset: 0x0036B586
		private void Start()
		{
			if (this.inventoryManager == null)
			{
				this.inventoryManager = Object.FindObjectOfType<PlayerInventoryManager>();
			}
		}

		// Token: 0x0600865E RID: 34398 RVA: 0x0036D3A4 File Offset: 0x0036B5A4
		private void CreateBeamMesh()
		{
			GameObject gameObject = new GameObject("[PlacementBeam]");
			gameObject.transform.SetParent(base.transform, false);
			this._meshFilter = gameObject.AddComponent<MeshFilter>();
			this._meshRenderer = gameObject.AddComponent<MeshRenderer>();
			Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
			if (shader == null)
			{
				shader = Shader.Find("Unlit/Transparent");
			}
			if (shader == null)
			{
				shader = Shader.Find("Hidden/Internal-Colored");
			}
			this._beamMaterial = new Material(shader);
			this._beamMaterial.color = this.validColor;
			this._beamMaterial.SetFloat("_Surface", 1f);
			this._beamMaterial.SetFloat("_ZWrite", 0f);
			this._beamMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
			this._beamMaterial.renderQueue = 3000;
			this._meshRenderer.material = this._beamMaterial;
			this._meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			this._meshRenderer.receiveShadows = false;
			gameObject.SetActive(false);
		}

		// Token: 0x0600865F RID: 34399 RVA: 0x0036D4B0 File Offset: 0x0036B6B0
		public void Activate()
		{
			this._isActive = true;
			if (this._meshFilter != null)
			{
				this._meshFilter.gameObject.SetActive(true);
			}
			this.UpdateHandObjectReference();
		}

		// Token: 0x06008660 RID: 34400 RVA: 0x0036D4DE File Offset: 0x0036B6DE
		public void Deactivate()
		{
			this._isActive = false;
			if (this._meshFilter != null)
			{
				this._meshFilter.gameObject.SetActive(false);
			}
		}

		// Token: 0x06008661 RID: 34401 RVA: 0x0036D508 File Offset: 0x0036B708
		private void UpdateHandObjectReference()
		{
			if (this.inventoryManager != null)
			{
				GameObject spawnedHandObject = this.inventoryManager.GetSpawnedHandObject();
				if (spawnedHandObject != null)
				{
					this.handObjectTransform = spawnedHandObject.transform;
				}
			}
		}

		// Token: 0x06008662 RID: 34402 RVA: 0x0036D544 File Offset: 0x0036B744
		public void UpdateBeam(PlacementResult result)
		{
			if (!this._isActive || this._meshFilter == null)
			{
				return;
			}
			this._targetColor = (result.IsValid ? this.validColor : this.invalidColor);
			this._currentColor = Color.Lerp(this._currentColor, this._targetColor, Time.deltaTime * 15f);
			this._beamMaterial.color = this._currentColor;
			Vector3 beamStartPosition = this.GetBeamStartPosition();
			Vector3 position = result.Position;
			this.GenerateConeMesh(beamStartPosition, position, this.beamBaseRadius, this.beamTipRadius);
			this._meshFilter.transform.position = beamStartPosition;
			this._meshFilter.transform.rotation = Quaternion.LookRotation(position - beamStartPosition);
		}

		// Token: 0x06008663 RID: 34403 RVA: 0x0036D608 File Offset: 0x0036B808
		private Vector3 GetBeamStartPosition()
		{
			if (this.handObjectTransform != null)
			{
				return this.handObjectTransform.position;
			}
			Camera main = Camera.main;
			if (main != null)
			{
				return main.transform.position + main.transform.forward * 0.3f;
			}
			return base.transform.position;
		}

		// Token: 0x06008664 RID: 34404 RVA: 0x0036D670 File Offset: 0x0036B870
		private void GenerateConeMesh(Vector3 start, Vector3 end, float baseRadius, float tipRadius)
		{
			if (this._beamMesh == null)
			{
				this._beamMesh = new Mesh();
				this._beamMesh.name = "PlacementBeam";
			}
			float num = Vector3.Distance(start, end);
			if (num < 0.01f)
			{
				num = 0.01f;
			}
			Vector3[] array = new Vector3[(this.beamSegments + 1) * 2];
			for (int i = 0; i <= this.beamSegments; i++)
			{
				float num2 = (float)i / (float)this.beamSegments * 3.1415927f * 2f;
				float num3 = Mathf.Cos(num2) * baseRadius;
				float num4 = Mathf.Sin(num2) * baseRadius;
				array[i] = new Vector3(num3, num4, 0f);
			}
			for (int j = 0; j <= this.beamSegments; j++)
			{
				float num5 = (float)j / (float)this.beamSegments * 3.1415927f * 2f;
				float num6 = Mathf.Cos(num5) * tipRadius;
				float num7 = Mathf.Sin(num5) * tipRadius;
				array[this.beamSegments + 1 + j] = new Vector3(num6, num7, num);
			}
			int[] array2 = new int[this.beamSegments * 6];
			for (int k = 0; k < this.beamSegments; k++)
			{
				int num8 = k;
				int num9 = (k + 1) % (this.beamSegments + 1);
				int num10 = this.beamSegments + 1 + k;
				int num11 = this.beamSegments + 1 + (k + 1) % (this.beamSegments + 1);
				int num12 = k * 6;
				array2[num12] = num8;
				array2[num12 + 1] = num9;
				array2[num12 + 2] = num10;
				array2[num12 + 3] = num9;
				array2[num12 + 4] = num11;
				array2[num12 + 5] = num10;
			}
			this._beamMesh.vertices = array;
			this._beamMesh.triangles = array2;
			this._beamMesh.RecalculateNormals();
			this._beamMesh.RecalculateBounds();
			this._meshFilter.mesh = this._beamMesh;
		}

		// Token: 0x06008665 RID: 34405 RVA: 0x0036D848 File Offset: 0x0036BA48
		private void OnDestroy()
		{
			if (this._beamMaterial != null)
			{
				Object.Destroy(this._beamMaterial);
			}
			if (this._beamMesh != null)
			{
				Object.Destroy(this._beamMesh);
			}
			if (this._meshFilter != null && this._meshFilter.gameObject != null)
			{
				Object.Destroy(this._meshFilter.gameObject);
			}
		}

		// Token: 0x040088E9 RID: 35049
		[Header("Beam Appearance")]
		[SerializeField]
		private Color validColor = new Color(0.2f, 0.9f, 0.4f, 0.4f);

		// Token: 0x040088EA RID: 35050
		[SerializeField]
		private Color invalidColor = new Color(0.9f, 0.2f, 0.2f, 0.4f);

		// Token: 0x040088EB RID: 35051
		[SerializeField]
		private float beamBaseRadius = 0.02f;

		// Token: 0x040088EC RID: 35052
		[SerializeField]
		private float beamTipRadius = 0.08f;

		// Token: 0x040088ED RID: 35053
		[SerializeField]
		private int beamSegments = 16;

		// Token: 0x040088EE RID: 35054
		[SerializeField]
		private int beamLengthSegments = 8;

		// Token: 0x040088EF RID: 35055
		[Header("References")]
		[Tooltip("Transform of the object in hand (spawned hand object)")]
		[SerializeField]
		private Transform handObjectTransform;

		// Token: 0x040088F0 RID: 35056
		[Tooltip("If null, will try to find from inventory manager on start")]
		[SerializeField]
		private PlayerInventoryManager inventoryManager;

		// Token: 0x040088F1 RID: 35057
		private MeshFilter _meshFilter;

		// Token: 0x040088F2 RID: 35058
		private MeshRenderer _meshRenderer;

		// Token: 0x040088F3 RID: 35059
		private Mesh _beamMesh;

		// Token: 0x040088F4 RID: 35060
		private Material _beamMaterial;

		// Token: 0x040088F5 RID: 35061
		private bool _isActive;

		// Token: 0x040088F6 RID: 35062
		private Color _currentColor;

		// Token: 0x040088F7 RID: 35063
		private Color _targetColor;
	}
}
