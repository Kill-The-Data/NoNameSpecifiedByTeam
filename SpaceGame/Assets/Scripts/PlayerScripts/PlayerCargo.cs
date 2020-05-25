using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerCargo : MonoBehaviour, IObserver
{

    [Header(" --- Setup ---")]
    [SerializeField] private TMP_Text m_text = null;
    [SerializeField] private Slider m_slider = null;


    [Header(" --- Slider UI setup ---")]
    [Range(0, 1.5f)]
    [SerializeField] private float m_tweenSpeed = 1.0f;
    [Tooltip("The slider should never reach 0, only close to 0 so that it does not disappear")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float m_MinSliderValue = 0.1f;

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
    //setup the slider ui variables
    //reset reset space occupied
    //gets executed on ingame state enter
    public void ResetCargo()
    {
        InitSlider();

        SpaceOccupied = 0;
    }

    //Initialize the Slider
    private void InitSlider()
    {
        m_slider.maxValue = m_cargoLimit;
        m_slider.minValue = 0;
        m_lerp = m_slider.gameObject.AddComponent<LerpSlider>();
        m_lerp.Init(m_slider, m_tweenSpeed, m_MinSliderValue);
    }



    //check if space is full and otherwise add n element to the inventory
    public void AddCargo(int amount = 1)
    {
        if (SpaceAvailable(amount))
        {
            SpaceOccupied += amount;
        }
    }

    public void SetFill(int amount)
    {
        SpaceOccupied = amount;
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
        UpdateSlider();
        UpdateText();
    }
    private void UpdateSlider()
    {
        m_lerp.UpdateSlider(SpaceOccupied);
    }
    private void UpdateText()
    {
        if (SpaceOccupied != 0)
        {
            m_text.SetText($"{SpaceOccupied} / {m_cargoLimit}");
        }
        else
        {
            m_text.SetText("");
        }
    }

    public void GetUpdate(ISubject subject)
    {
        if (subject is MotherShipCollisionHandler collision)
        {
            SetFill(collision.LeftoverCargo);
        }
    }
}
