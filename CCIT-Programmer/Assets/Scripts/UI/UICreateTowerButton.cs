using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UICreateTowerButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    //버튼 상태에 따른 이미지
    public Sprite normalImage;
    public Sprite hoverImage;
    public Sprite clickImage;

    //마우스 다운시와 업시 이벤트
    public UnityEvent mouseDownEvent;
    public UnityEvent mouseReleaseEvent;

    //쿨타임 이미지 컴포넌트
    private Image coolTimeImage;
    //버튼 이미지 컴포넌트
    private Image image;
    
    //버튼 상태
    bool isEnter = false;
    bool isDown = false;

    
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

    //마우스 클릭시
    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        image.sprite = clickImage;
        mouseDownEvent.Invoke();
    }

    //마우스 클릭 종료시
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

    //마우스 호버 시작
    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
        image.sprite = hoverImage;
    }

    //마우스 호버 종료
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
