using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerCargo : MonoBehaviour
{

    [Header(" --- Setup ---")]
    [SerializeField] private TMP_Text m_text = null;
    [SerializeField] private Slider m_slider = null;


    [Header(" --- Tween setup ---")]
    [Range(0, 1.5f)]
    [SerializeField] private float m_tweenSpeed = 1.0f;

    [Header(" --- Cargo ---")]
    [Tooltip("How much items the Player can hold at any given time")]
    [SerializeField] private int m_cargoLimit = 2;


    private int m_spaceOccupiedImpl;
    private LerpSlider m_lerp = null;

    //whenever the occupied space is updated also update the text
    public int m_spaceOccupied
    {
        get => m_spaceOccupiedImpl;
        private set
        {
            m_spaceOccupiedImpl = value;
            UpdateView();
        }
    }

    //in the beginning update the text manually to avoid displaying "New Text"
    public void Start()
    {
        m_slider.maxValue = m_cargoLimit;
        m_slider.minValue = 0;
        m_lerp = m_slider.gameObject.AddComponent<LerpSlider>();
        m_lerp.Init(m_slider, m_tweenSpeed);
        UpdateView();
    }
    private void Update()
    {
        UpdateView();
    }

    //check if space is full and otherwise add n element to the inventory
    public void AddCargo(int amount = 1)
    {
        if (SpaceAvailable(amount))
        {
            m_spaceOccupied += amount;
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

    //update the text && the slider to reflect the status of the inventory
    private void UpdateView()
    {
        //m_text.SetText($"{m_spaceOccupied} / {m_cargoLimit}");
        m_lerp.UpdateSlider(m_spaceOccupied);
    }
}
