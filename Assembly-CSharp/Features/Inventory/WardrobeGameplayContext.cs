using System;
using System.Collections.Generic;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Inventory
{
	// Token: 0x02001108 RID: 4360
	public static class WardrobeGameplayContext
	{
		// Token: 0x06008468 RID: 33896 RVA: 0x0035FC74 File Offset: 0x0035DE74
		public static IDisposable BeginPresentationOverride(Component owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			WardrobeGameplayContext.PresentationScope presentationScope = new WardrobeGameplayContext.PresentationScope
			{
				owner = owner
			};
			WardrobeGameplayContext.Scopes.Add(presentationScope);
			return presentationScope;
		}

		// Token: 0x06008469 RID: 33897 RVA: 0x0035FCAE File Offset: 0x0035DEAE
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetScopes()
		{
			WardrobeGameplayContext.Scopes.Clear();
		}

		// Token: 0x0600846A RID: 33898 RVA: 0x0035FCBC File Offset: 0x0035DEBC
		public static bool IsPresentationOnly(Component owner = null)
		{
			WardrobeGameplayContext.Scopes.RemoveAll((WardrobeGameplayContext.PresentationScope scope) => scope.owner == null);
			return WardrobeGameplayContext.Scopes.Count != 0 || (owner != null && owner.GetComponentInParent<CustomizationPreviewPhysicsGuard>(true) != null) || WardrobeGameplayContext.IsPresentationScene(SceneManager.GetActiveScene().name) || (owner != null && WardrobeGameplayContext.IsPresentationScene(owner.gameObject.scene.name));
		}

		// Token: 0x0600846B RID: 33899 RVA: 0x0035FD54 File Offset: 0x0035DF54
		private static bool IsPresentationScene(string sceneName)
		{
			return string.Equals(sceneName, "MainMenu", StringComparison.Ordinal) || string.Equals(sceneName, "LongWayHome_RestaurantIntro", StringComparison.Ordinal);
		}

		// Token: 0x04008737 RID: 34615
		private static readonly List<WardrobeGameplayContext.PresentationScope> Scopes = new List<WardrobeGameplayContext.PresentationScope>();

		// Token: 0x02001109 RID: 4361
		private sealed class PresentationScope : IDisposable
		{
			// Token: 0x0600846D RID: 33901 RVA: 0x0035FD7E File Offset: 0x0035DF7E
			public void Dispose()
			{
				WardrobeGameplayContext.Scopes.Remove(this);
				this.owner = null;
			}

			// Token: 0x04008738 RID: 34616
			internal Component owner;
		}
	}
}
