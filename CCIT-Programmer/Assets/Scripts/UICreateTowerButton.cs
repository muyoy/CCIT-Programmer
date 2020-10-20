using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UICreateTowerButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Sprite normalImage;
    public Sprite hoverImage;
    public Sprite clickImage;

    public UnityEvent mouseDownEvent;
    public UnityEvent mouseReleaseEvent;

    private Image coolTimeImage;
    private Image image;
    bool isEnter = false;
    bool isDown = false;

    // Start is called before the first frame update
    void Start()
    {
        //coolTimeImage = transform.Find("CoolTimeImage").GetComponent<Image>();
        image = GetComponent<Image>();
        if (normalImage == null)
        {
            normalImage = image.sprite;
        }
        else if (normalImage.name != image.sprite.name)
        {
            image.sprite = normalImage;
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        image.sprite = clickImage;
        mouseDownEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
        if (isEnter)
        {
            image.sprite = hoverImage;
        }
        else
        {
            image.sprite = normalImage;
        }

        mouseReleaseEvent.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
        image.sprite = hoverImage;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
        if (isDown)
        {
            image.sprite = clickImage;
        }
        else
        {
            image.sprite = normalImage;
        }
    }

}
