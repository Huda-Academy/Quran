using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageManager : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelPosition;
    public float percentThreshold = 0.2f;

    private void Start()
    {
        panelPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float difference = eventData.pressPosition.x - eventData.position.x;
        transform.position = panelPosition - new Vector3(difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newPanelPosition = panelPosition;
            if (percentage > 0)
            {
                newPanelPosition += new Vector3(-Screen.width, 0, 0);
            }
            else if (percentage < 0)
            {
                newPanelPosition += new Vector3(Screen.width, 0, 0);
            }
            StartCoroutine(SmoothMove(transform.position, newPanelPosition, 0.5f));
            panelPosition = newPanelPosition;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelPosition, 0.5f));
        }
    }

    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

}
