using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x0200005E RID: 94
	public class MMRandomInstantiator : MonoBehaviour
	{
		// Token: 0x06000212 RID: 530 RVA: 0x00009FAC File Offset: 0x000081AC
		protected virtual void Awake()
		{
			if (this.StartMode == MMRandomInstantiator.StartModes.Awake)
			{
				this.InstantiateRandomObject();
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00009FBC File Offset: 0x000081BC
		protected virtual void Start()
		{
			if (this.StartMode == MMRandomInstantiator.StartModes.Start)
			{
				this.InstantiateRandomObject();
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009FD0 File Offset: 0x000081D0
		public virtual void InstantiateRandomObject()
		{
			if (this.RandomPool.Count == 0)
			{
				return;
			}
			if (this.DestroyPreviouslyInstantiatedObject && this._instantiatedGameObject != null)
			{
				Object.DestroyImmediate(this._instantiatedGameObject);
			}
			int num = Random.Range(0, this.RandomPool.Count);
			this._instantiatedGameObject = Object.Instantiate<GameObject>(this.RandomPool[num], base.transform.position, base.transform.rotation);
			SceneManager.MoveGameObjectToScene(this._instantiatedGameObject, base.gameObject.scene);
			this._instantiatedGameObject.name = this.InstantiatedObjectName;
			if (this.ParentInstantiatedToThisObject)
			{
				this._instantiatedGameObject.transform.SetParent(base.transform);
			}
		}

		// Token: 0x040002D6 RID: 726
		[Header("Random instantiation")]
		public MMRandomInstantiator.StartModes StartMode;

		// Token: 0x040002D7 RID: 727
		public string InstantiatedObjectName = "RandomInstantiated";

		// Token: 0x040002D8 RID: 728
		public bool ParentInstantiatedToThisObject = true;

		// Token: 0x040002D9 RID: 729
		public bool DestroyPreviouslyInstantiatedObject = true;

		// Token: 0x040002DA RID: 730
		public List<GameObject> RandomPool;

		// Token: 0x040002DB RID: 731
		[Header("Test")]
		[MMInspectorButton("InstantiateRandomObject")]
		public bool InstantiateButton;

		// Token: 0x040002DC RID: 732
		protected GameObject _instantiatedGameObject;

		// Token: 0x020003CD RID: 973
		public enum StartModes
		{
			// Token: 0x04001DF6 RID: 7670
			Awake,
			// Token: 0x04001DF7 RID: 7671
			Start,
			// Token: 0x04001DF8 RID: 7672
			None
		}
	}
}
