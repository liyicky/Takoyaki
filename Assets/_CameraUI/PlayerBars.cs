using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
  public class PlayerBars : MonoBehaviour
  {
    [SerializeField] Image healthPoolHolder;
    [SerializeField] Image manaPoolHolder;
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
      healthPoolHolder.fillAmount = player.healthAsPercentage;
    }

    void UpdateManaPool()
    {
      manaPoolHolder.fillAmount = player.manaAsPercentage;
    }
  }
}