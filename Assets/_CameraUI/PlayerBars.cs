using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
  [RequireComponent(typeof(CanvasRenderer))]
  public class PlayerBars : MonoBehaviour
  {
    [SerializeField] CanvasRenderer healthPoolHolder;
    [SerializeField] CanvasRenderer manaPoolHolder;
    Player player;

    // Use this for initialization
    void Start()
    {
      player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
      UpdateHealthPool();
      UpdateManaPool();
    }
    
    void UpdateHealthPool()
    {
      float yValue = (185f * player.healthAsPercentage) - 185f;
      healthPoolHolder.transform.localPosition = new Vector3(0, yValue, 0);
    }

    void UpdateManaPool()
    {
      float yValue = (185f * player.manaAsPercentage) - 185f;
      manaPoolHolder.transform.localPosition = new Vector3(0, yValue, 0);
    }
  }
}