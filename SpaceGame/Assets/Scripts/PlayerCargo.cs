using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerCargo : MonoBehaviour
{

    [SerializeField] private TMP_Text _Text;
    [SerializeField] private int _CargoLimit;
    private int _SpaceOccupied = 0;
    void Update()
    {
        UpdateText();
    }
    public void AddCargo(int amount = 1)
    {
        if (!SpaceIsFull())
        {
            _SpaceOccupied += 1;
            UpdateText();
        }
    }
    public void ClearCargo()
    {
        _SpaceOccupied = 0;
        UpdateText();
    }
    public bool SpaceIsFull()
    {
        if (_SpaceOccupied < _CargoLimit)
        {
            return false;
        }
        return true;
    }
    private void UpdateText()
    {
        string message = _SpaceOccupied.ToString() + " / " + _CargoLimit.ToString();

        _Text.SetText(message);

    }
}
