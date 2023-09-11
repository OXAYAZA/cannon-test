using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private Vector3 initialLocalPosition;

    private Vector3 initialForward;

    private Quaternion initialLocalRotation;

    private void Awake()
    {
        var trm = this.transform;
        this.initialLocalPosition = trm.localPosition;
        this.initialForward = trm.forward;
        this.initialLocalRotation = trm.localRotation;
    }

    public void Shake(float duration = 0.15f, float positionOffsetStrength = 0.025f, float rotationOffsetStrength = 0.05f)
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.CameraShakeCoroutine(duration, positionOffsetStrength, rotationOffsetStrength));
    }

    private IEnumerator CameraShakeCoroutine(float duration, float positionOffsetStrength, float rotationOffsetStrength)
    {
        var elapsed = 0f;
        var currentMagnitude = 1f;

        while(elapsed < duration)
        {
            var x = (Random.value - 0.5f) * currentMagnitude * positionOffsetStrength;
            var y = (Random.value - 0.5f) * currentMagnitude * positionOffsetStrength;

            var lerpAmount = currentMagnitude * rotationOffsetStrength;
            var lookAtVector = Vector3.Lerp(this.initialForward, Random.insideUnitCircle, lerpAmount);

            this.transform.localPosition = this.initialLocalPosition + new Vector3(x, y, 0);
            this.transform.localRotation = Quaternion.LookRotation(lookAtVector);

            elapsed += Time.deltaTime;
            currentMagnitude = (1 - elapsed / duration) * (1 - elapsed / duration);

            yield return null;
        }

        var trm = this.transform;
        trm.localPosition = this.initialLocalPosition;
        trm.localRotation = this.initialLocalRotation;
    }
}
