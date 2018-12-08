using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField]
    Texture2D walkCursor = null;

    [SerializeField]
    Texture2D targetCursor = null;

    [SerializeField]
    Texture2D unknownCursor = null;

    [SerializeField]
    Vector2 cursorHotspot = new Vector2(0, 0);

    CameraRaycaster raycaster;

	void Start () {
        raycaster = GetComponent<CameraRaycaster>();
        raycaster.OnLayerChanged += OnLayerChange;
	}
	
	void OnLayerChange (Layer layerhit) {
        Debug.Log("here cursor affordances");

        switch(layerhit)
        {
            case Layer.Enemy:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Debug.LogWarning("Unknown Cursor");
                break;
        }
	}
}
