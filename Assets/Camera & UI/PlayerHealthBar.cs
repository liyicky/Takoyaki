using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class PlayerHealthBar : MonoBehaviour
{

    CanvasRenderer healthPoolHolder;
    Player player;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<Player>();
        healthPoolHolder = GetComponent<CanvasRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // float xValue = -(player.healthAsPercentage / 2f) - 0.5f;
        float yValue = (player.healthAsPercentage * 0.01f) * -421f;
        healthPoolHolder.transform.localPosition = Vector3.zero;
        // healthPoolHolder.uvRect = new Rect(healthPoolHolder.uvRect.xValue, healthPoolHolder.uvRect.xValue)
        // healthPoolSprite.
        // healthPoolSprite.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }
}
