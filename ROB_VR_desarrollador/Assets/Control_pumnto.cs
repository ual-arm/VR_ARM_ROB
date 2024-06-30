using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class TMP_DropdownManager2 : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TMP_Dropdown dropdown2;


    public struct DropdownOption
    {
        public string name;
        public float value1;
        public float value2;
        // Agrega otros campos según sea necesario

        public DropdownOption(string _name, float _value1, float _value2)
        {
            name = _name;
            value1 = _value1;
            value2 = _value2;
        }
    }

    public List<DropdownOption> options = new List<DropdownOption>();

    void Start()
    {
        dropdown2.onValueChanged.AddListener(OnDropdown2ValueChanged);
    }


    public void OnDropdown2ValueChanged(int index)
    {
            dropdown.value = index; // Actualiza la selección en dropdown

    }
}