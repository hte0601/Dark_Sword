using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PlayerJoystick : MonoBehaviour
{
    [Header("Joystick")]
    public GameObject InvisibleJoystick;
    public GameObject LeftArrow;
    public GameObject RightArrow;

    public static float JoystickX;
    public GameObject canvas;
    public Camera cam;

    private Vector2 GetCanvasPos()
    {
        RectTransform CanvasRect = canvas.GetComponent<RectTransform>();
        //world to screen
        var screenPos = cam.WorldToScreenPoint(InvisibleJoystick.transform.position);  //= RectTransformUtility.WorldToScreenPoint(cam, InvisibleJoystick.transform.position)
        
        return screenPos;
    }

    //mobile
    public void JoystickDrag(BaseEventData _data)
    {
        PointerEventData data = _data as PointerEventData;
        Vector2 Pos = data.position;
        
        Vector2 moveVector = Pos - GetCanvasPos();
        if(moveVector.x > 20) // 두 버튼 간격 40
        {
            JoystickX = 1f;
            RightArrow.GetComponent<Button>().interactable = false;
        }
        else if(moveVector.x < -20)
        {
            JoystickX = -1f;
            LeftArrow.GetComponent<Button>().interactable = false;
        }
        else
        {
            JoystickX = 0;
            LeftArrow.GetComponent<Button>().interactable = true;
            RightArrow.GetComponent<Button>().interactable = true;
        }
    }
    
    public void JoystickEndDrag(BaseEventData _data)
    {
        JoystickX = 0;
        LeftArrow.GetComponent<Button>().interactable = true;
        RightArrow.GetComponent<Button>().interactable = true;
    }
}
