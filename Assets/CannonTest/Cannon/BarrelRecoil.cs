using System.Collections;
using UnityEngine;

public class BarrelRecoil : MonoBehaviour
{
    [SerializeField]
    private Vector3 recoilPositionIncrement = new Vector3(0f, 0.5f, 0f);

    private Vector3 initialLocalPosition;

    private void Awake()
    {
        this.initialLocalPosition = this.transform.localPosition;
    }

    public void Recoil(float duration = 0.15f)
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.RecoilCoroutine(duration));
    }

    private IEnumerator RecoilCoroutine(float duration)
    {
        var elapsed = 0f;
        var recoilLocalPosition = this.initialLocalPosition + this.recoilPositionIncrement;

        while(elapsed < duration)
        {
            var lerpAmount = elapsed / (duration / 2);
            if(lerpAmount > 1) lerpAmount = 2 - lerpAmount;

            this.transform.localPosition =
                Vector3.Lerp(this.initialLocalPosition, recoilLocalPosition, lerpAmount);

            elapsed += Time.deltaTime;
            yield return null;
        }

        this.transform.localPosition = this.initialLocalPosition;
    }
}
