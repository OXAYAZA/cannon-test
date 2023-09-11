using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class PowerSlider : MonoBehaviour
{
    [SerializeField]
    private Cannon cannon;

    [SerializeField]
    private Text number;

    private Slider slider;

    private void Awake()
    {
        this.slider = this.GetComponent<Slider>();
    }

    private void Start()
    {
        var val = this.cannon.shotForce / (this.cannon.maxShotForce - this.cannon.minShotForce);
        this.slider.value = val;
        this.number.text = Mathf.Round(val * 100f).ToString(CultureInfo.InvariantCulture);

        this.slider.onValueChanged.AddListener(this.UpdateValue);
    }

    private void UpdateValue(float val)
    {
        this.cannon.SetShotForce(val);
        this.number.text = Mathf.Round(val * 100f).ToString(CultureInfo.InvariantCulture);
    }
}
