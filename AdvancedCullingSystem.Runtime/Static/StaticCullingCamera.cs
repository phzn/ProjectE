using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x0200001F RID: 31
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Camera))]
	public class StaticCullingCamera : MonoBehaviour
	{
		// Token: 0x060000A5 RID: 165 RVA: 0x00004D3C File Offset: 0x00002F3C
		private void Reset()
		{
			this._tolerance = Vector2.one;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004D49 File Offset: 0x00002F49
		private void Start()
		{
			if (CameraZone.Instances.Count == 0)
			{
				Debug.Log("StaticCullingCamera : Not found Camera Zones in scene");
				base.enabled = false;
				return;
			}
			this._tree = this.FindNearestVisibilityTree();
			if (this._tree == null)
			{
				Debug.Log("StaticCullingCamera : Can't find nearest CameraZone");
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004D88 File Offset: 0x00002F88
		private void Update()
		{
			Vector3 position = base.transform.position;
			if (this._tree == null || !this._tree.Root.Bounds.Contains(position))
			{
				this._tree = this.FindNearestVisibilityTree();
				if (this._tree == null)
				{
					return;
				}
			}
			this._tree.SetVisible(position, this._tolerance);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004DEC File Offset: 0x00002FEC
		private VisibilityTree FindNearestVisibilityTree()
		{
			if (CameraZone.Instances.Count == 0)
			{
				return null;
			}
			Vector3 position = base.transform.position;
			foreach (CameraZone cameraZone in CameraZone.Instances)
			{
				if (!(cameraZone == null))
				{
					VisibilityTree visibilityTree = cameraZone.VisibilityTree;
					if (visibilityTree != null && visibilityTree.CullingTargets != null && visibilityTree.Root.Bounds.Contains(position))
					{
						return visibilityTree;
					}
				}
			}
			return null;
		}

		// Token: 0x040000A0 RID: 160
		[SerializeField]
		private bool _drawCells;

		// Token: 0x040000A1 RID: 161
		[Range(0f, 1f)]
		[SerializeField]
		private Vector2 _tolerance;

		// Token: 0x040000A2 RID: 162
		private VisibilityTree _tree;
	}
}
