using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000C4 RID: 196
	internal class SharedDecalEntityManager : IDisposable
	{
		// Token: 0x0600060D RID: 1549 RVA: 0x00025488 File Offset: 0x00023688
		public DecalEntityManager Get()
		{
			if (this.m_DecalEntityManager == null)
			{
				this.m_DecalEntityManager = new DecalEntityManager();
				foreach (DecalProjector decalProjector in Object.FindObjectsOfType<DecalProjector>())
				{
					if (decalProjector.isActiveAndEnabled && !this.m_DecalEntityManager.IsValid(decalProjector.decalEntity))
					{
						decalProjector.decalEntity = this.m_DecalEntityManager.CreateDecalEntity(decalProjector);
					}
				}
				DecalProjector.onDecalAdd += this.OnDecalAdd;
				DecalProjector.onDecalRemove += this.OnDecalRemove;
				DecalProjector.onDecalPropertyChange += this.OnDecalPropertyChange;
				DecalProjector.onDecalMaterialChange += this.OnDecalMaterialChange;
				DecalProjector.onAllDecalPropertyChange += this.OnAllDecalPropertyChange;
			}
			this.m_ReferenceCounter++;
			return this.m_DecalEntityManager;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00025559 File Offset: 0x00023759
		public void Release(DecalEntityManager decalEntityManager)
		{
			if (this.m_ReferenceCounter == 0)
			{
				return;
			}
			this.m_ReferenceCounter--;
			if (this.m_ReferenceCounter == 0)
			{
				this.Dispose();
			}
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00025580 File Offset: 0x00023780
		public void Dispose()
		{
			this.m_DecalEntityManager.Dispose();
			this.m_DecalEntityManager = null;
			this.m_ReferenceCounter = 0;
			DecalProjector.onDecalAdd -= this.OnDecalAdd;
			DecalProjector.onDecalRemove -= this.OnDecalRemove;
			DecalProjector.onDecalPropertyChange -= this.OnDecalPropertyChange;
			DecalProjector.onDecalMaterialChange -= this.OnDecalMaterialChange;
			DecalProjector.onAllDecalPropertyChange -= this.OnAllDecalPropertyChange;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x000255FB File Offset: 0x000237FB
		private void OnDecalAdd(DecalProjector decalProjector)
		{
			if (!this.m_DecalEntityManager.IsValid(decalProjector.decalEntity))
			{
				decalProjector.decalEntity = this.m_DecalEntityManager.CreateDecalEntity(decalProjector);
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00025622 File Offset: 0x00023822
		private void OnDecalRemove(DecalProjector decalProjector)
		{
			this.m_DecalEntityManager.DestroyDecalEntity(decalProjector.decalEntity);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00025635 File Offset: 0x00023835
		private void OnDecalPropertyChange(DecalProjector decalProjector)
		{
			if (this.m_DecalEntityManager.IsValid(decalProjector.decalEntity))
			{
				this.m_DecalEntityManager.UpdateDecalEntityData(decalProjector.decalEntity, decalProjector);
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0002565C File Offset: 0x0002385C
		private void OnAllDecalPropertyChange()
		{
			this.m_DecalEntityManager.UpdateAllDecalEntitiesData();
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00025669 File Offset: 0x00023869
		private void OnDecalMaterialChange(DecalProjector decalProjector)
		{
			this.OnDecalRemove(decalProjector);
			this.OnDecalAdd(decalProjector);
		}

		// Token: 0x040004E9 RID: 1257
		private DecalEntityManager m_DecalEntityManager;

		// Token: 0x040004EA RID: 1258
		private int m_ReferenceCounter;
	}
}
