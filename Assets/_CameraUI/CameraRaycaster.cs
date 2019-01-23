using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using RPG.Characters;
using System;

namespace RPG.CameraUI
{
	public class CameraRaycaster : MonoBehaviour
	{
		// INSPECTOR PROPERTIES RENDERED BY CUSTOM EDITOR SCRIPT
		[SerializeField] Texture2D walkCursor;
    [SerializeField] Texture2D targetCursor;
		[SerializeField] Texture2D unknownCursor;
		[SerializeField] Vector2 cursorHotspot = new Vector2(0, 5);

		const int walkableLayerNumber = 9;
		float maxRaycastDepth = 100f; // Hard coded value

		/* --------- DELEGATES ---------- */
		public delegate void OnMouseOverTerrain(Vector3 destination);
		public event OnMouseOverTerrain onMouseOverTerrain;

		public delegate void OnMouseOverEnemy(Enemy enemy);
		public event OnMouseOverEnemy onMouseOverEnemy;
		/* ------------------- */

		void Update()
    {
      if (EventSystem.current.IsPointerOverGameObject())
      {
				// handle UI here
      }
			else
			{
				PerformRaycast();
      	// NewMethod();	
			}
    }

		void PerformRaycast()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (RaycastForEnemy(ray)) { return; }
			if (RaycastForWalkable(ray)) { return; }
			Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
		}

    private bool RaycastForEnemy(Ray ray)
    {
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, maxRaycastDepth))
			{
				var gameObjectHit = hit.collider.gameObject;
				var enemyHit = gameObjectHit.GetComponent<Enemy>();
				if (enemyHit)
				{
					Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
					onMouseOverEnemy(enemyHit);
					return true;
				}
			}
      return false;
    }

    private bool RaycastForWalkable(Ray ray)
    {
			int layerMask = 1 << walkableLayerNumber;
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, maxRaycastDepth, layerMask))
			{
					Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
					onMouseOverTerrain(hit.point);
					return true;
			}
      return false;
    }
	}
}