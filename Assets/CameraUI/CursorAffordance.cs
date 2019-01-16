using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]

public class CursorAffordance : MonoBehaviour
{
    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Texture2D errorCursor = null;

    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 5);

    [SerializeField] const int walkableLayerNumber = 9;
    [SerializeField] const int enemyLayerNumber = 10;
    

    CameraRaycaster cameraRaycaster;
    // Start is called before the first frame update
    void Start()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged;
    }

    void OnLayerChanged(int layer)
    {
        var i = layer;
        switch (layer)
        {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(errorCursor, cursorHotspot, CursorMode.Auto);
                Debug.LogError("Don't know what cursor to show");
                return;
        }
    }

    // TODO: consider de-registering OnLayerChanged on leaving all game scenes
}
