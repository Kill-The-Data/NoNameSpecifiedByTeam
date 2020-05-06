﻿using UnityEngine;
using TMPro;

public class PlayerCargo : MonoBehaviour
{

    [Header(" --- Setup ---")]
    [SerializeField] private TMP_Text m_text = null;
    
    
    [Header(" --- Cargo ---")]
    [Tooltip("How much items the Player can hold at any given time")]
    [SerializeField] private int m_cargoLimit = 2;


    private int m_spaceOccupiedImpl;
    
    //whenever the occupied space is updated also update the text
    private int m_spaceOccupied
    {
        get => m_spaceOccupiedImpl;
        set
        {
            m_spaceOccupiedImpl = value;
            UpdateText();
        }
    }

    //in the beginning update the text manually to avoid displaying "New Text"
    public void Start()
    {
        UpdateText();
    }

    
    //check if space is full and otherwise add n element to the inventory
    public void AddCargo(int amount = 1)
    {
        if (SpaceAvailable(amount))
        {
            m_spaceOccupied+=amount;
        }
    }

    //remove all cargo from the player
    public void ClearCargo()
    {
        m_spaceOccupied = 0;
    }

    //check if you can insert n elements into the player inventory
    public bool SpaceAvailable(int space_to_fill = 1)
    {
        return m_spaceOccupied + space_to_fill <= m_cargoLimit;
    }

    //check if the inventory is completely full
    public bool SpaceIsFull()
    {
        return m_spaceOccupied >= m_cargoLimit;
    }

    //update the text to reflect the status of the inventory
    private void UpdateText()
    {
        m_text.SetText($"{m_spaceOccupied} / {m_cargoLimit}");

    }
}