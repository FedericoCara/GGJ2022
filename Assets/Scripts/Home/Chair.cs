using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chair : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    public bool IsOn {
        get => toggle.isOn;
        set => toggle.isOn = value;
    }

    public void SetIsOnWithoutNotify(bool value) {
        toggle.SetIsOnWithoutNotify(value);
    }

    public void OnChairClicked() {
        HomeController.Instance.OnChairChanged();
    }

}
