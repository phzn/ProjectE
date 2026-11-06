using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x02000958 RID: 2392
	[DisallowMultipleComponent]
	public sealed class PoiBoundaryPresentation : MonoBehaviour
	{
		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06004135 RID: 16693 RVA: 0x00182D8F File Offset: 0x00180F8F
		public static Color BoundaryColor
		{
			get
			{
				return new Color(0.91f, 0.36f, 0.49f, 0.62f);
			}
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x00182DAC File Offset: 0x00180FAC
		private void OnEnable()
		{
			this._walls.Clear();
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
			{
				Material sharedMaterial = renderer.sharedMaterial;
				if (!(sharedMaterial == null) && !(sharedMaterial.shader == null) && (!(sharedMaterial.shader.name != "Project Exhibitionist/Demo Boundary Light") || !(sharedMaterial.shader.name != "Project Exhibitionist/POI Frosted Glass")))
				{
					renderer.enabled = false;
					if (!renderer.name.Contains("skylight blackout") && !renderer.name.EndsWith("sky fade"))
					{
						this._walls.Add(renderer);
					}
				}
			}
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			this.SetVisible(SceneManager.GetActiveScene() == base.gameObject.scene);
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x00182EA4 File Offset: 0x001810A4
		private void OnActiveSceneChanged(Scene previous, Scene current)
		{
			this.SetVisible(current == base.gameObject.scene);
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x00182EC0 File Offset: 0x001810C0
		private void SetVisible(bool visible)
		{
			for (int i = 0; i < this._walls.Count; i++)
			{
				if (this._walls[i] != null)
				{
					this._walls[i].enabled = visible;
				}
			}
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x00182F09 File Offset: 0x00181109
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			this.SetVisible(false);
		}

		// Token: 0x0400411F RID: 16671
		public const string ShaderName = "Project Exhibitionist/Demo Boundary Light";

		// Token: 0x04004120 RID: 16672
		private readonly List<Renderer> _walls = new List<Renderer>();
	}
}
