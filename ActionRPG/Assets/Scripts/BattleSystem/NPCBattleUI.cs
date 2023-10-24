using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBattleUI : MonoBehaviour
{
    #region Variables

    private Slider _hpSlider;

    public float MinimumHP
    {
        get => _hpSlider.minValue;
        set => _hpSlider.minValue = value;
    }
    public float MaximumHP
    {
        get => _hpSlider.maxValue;
        set => _hpSlider.maxValue = value;
    }

    public float Value
    {
        get => _hpSlider.value;
        set => _hpSlider.value = value;
    }

    #endregion Variables
    
    #region Unity Methods

    private void Awake()
    {
        _hpSlider = gameObject.GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        GetComponent<Canvas>().enabled = true;
    }

    private void OnDisable()
    { 
        GetComponent<Canvas>().enabled = false;
    }

    #endregion Unity Methods
}
