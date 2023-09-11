using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private Cannon cannon;

    [SerializeField]
    private float deadZone = 10f;

    [SerializeField]
    private float touchCheckDuration = 0.1f;

    [SerializeField]
    private float dragCheckDuration = 0.1f;

    private bool dragged;

    private Vector2 startPosition;

    private Vector2 endPosition;

    private Vector2 prevPosition;

    private Coroutine touchCheck;

    private Coroutine dragCheck;

    private void Update()
    {
        if(this.dragged)
        {
            var diff = this.endPosition - this.startPosition;

            diff.x = Mathf.Clamp(diff.x / 300f, -1f, 1f);
            diff.y = Mathf.Clamp(-diff.y / 300f, -1f, 1f);
            
            this.cannon.Rotate(diff);

            if(this.prevPosition == this.endPosition && this.dragCheck == null)
            {
                this.dragCheck = this.StartCoroutine(this.DragCheckCoroutine(this.dragCheckDuration));
            }
            else
            {
                this.prevPosition = this.endPosition;
            }
        }
    }

    private void Touch()
    {
        this.cannon.Shot();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.startPosition = this.endPosition = eventData.position;
        this.touchCheck = this.StartCoroutine(this.TouchCheckCoroutine(this.touchCheckDuration));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if((eventData.position - this.prevPosition).magnitude > this.deadZone)
        {
            this.endPosition = eventData.position;
        }

        if(this.touchCheck == null)
        {
            this.dragged = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(this.touchCheck != null)
        {
            this.StopCoroutine(this.touchCheck);
            this.touchCheck = null;
            this.Touch();
        }

        if(this.dragCheck != null)
        {
            this.StopCoroutine(this.dragCheck);
            this.dragCheck = null;
        }

        this.dragged = false;
        this.startPosition = this.endPosition = default;
    }

    private IEnumerator TouchCheckCoroutine(float duration)
    {
        var elapsed = 0f;

        while(elapsed < duration)
        {
            var diff = this.endPosition - this.startPosition;

            if(diff.magnitude > this.deadZone)
            {
                this.dragged = true;
                break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if(!this.dragged)
        {
            this.Touch();
        }

        this.touchCheck = null;
    }

    private IEnumerator DragCheckCoroutine(float duration)
    {
        var elapsed = 0f;
        var stopped = true;

        while(elapsed < duration)
        {
            var diff = this.endPosition - this.prevPosition;

            if(diff.magnitude > this.deadZone)
            {
                stopped = false;
                break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if(stopped)
        {
            this.startPosition = this.endPosition;
        }

        this.dragCheck = null;
    }
}
