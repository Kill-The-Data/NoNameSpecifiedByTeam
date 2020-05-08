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
    public int SpaceOccupied
    {
        get => m_spaceOccupiedImpl;
        private set
        {
            m_spaceOccupiedImpl = value;
            UpdateView();
        }
    }

    //in the beginning update the text manually to avoid displaying "New Text"
    //and setup the slider ui variables
    public void Start()
    {
        InitSlider();
        UpdateView();
    }

    //Initialize the Slider
    private void InitSlider()
    {
        m_slider.maxValue = m_cargoLimit;
        m_slider.minValue = 0;
        m_lerp = m_slider.gameObject.AddComponent<LerpSlider>();
        m_lerp.Init(m_slider, m_tweenSpeed);
    }
    
    //Update the Slider & Text
    //TODO(kukash): see below
    private void Update()
    {
        UpdateView();
    }

    //check if space is full and otherwise add n element to the inventory
    public void AddCargo(int amount = 1)
    {
        if (SpaceAvailable(amount))
        {
            SpaceOccupied += amount;
        }
    }

    //remove all cargo from the player
    public void ClearCargo()
    {
        SpaceOccupied = 0;
    }

    //check if you can insert n elements into the player inventory
    public bool SpaceAvailable(int space_to_fill = 1)
    {
        return SpaceOccupied + space_to_fill <= m_cargoLimit;
    }

    //check if the inventory is completely full
    public bool SpaceIsFull()
    {
        return SpaceOccupied >= m_cargoLimit;
    }
    
    //update the text & the slider to reflect the status of the inventory
    private void UpdateView()
    {
        //TODO(kukash): reimplement the text and split this function up into 
        //TODO(cont.): UpdateText() and UpdateSlider() so that the get=> property 
        //TODO(cont.): SpaceOccupied saves us some computation time again!
        //m_text.SetText($"{SpaceOccupied} / {m_cargoLimit}");
        m_lerp.UpdateSlider(SpaceOccupied);
    }
}
