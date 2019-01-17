using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
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
      float yValue = (185f * player.healthAsPercentage) - 185f;
      healthPoolHolder.transform.localPosition = new Vector3(0, yValue, 0);
    }
  }
}