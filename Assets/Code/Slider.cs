using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public GameObject sensslider;
    public PlayerSettings sensetivityChange;
    // Start is called before the first frame update
    void Start()
    {
        sensslider = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        sensetivityChange.mouseSensitivityY = sensslider.GetComponent<UnityEngine.UI.Slider>().value;
        sensetivityChange.mouseSensitivityX = sensslider.GetComponent<UnityEngine.UI.Slider>().value;
    }
}
