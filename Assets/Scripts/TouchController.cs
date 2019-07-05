using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TouchController : MonoBehaviour
{

    Vector2 m_touchMovement;

    [Range(50, 150)]
    public int m_minDragDistance = 100;

    [Range(20, 250)]
    public int m_minSwipeDistance = 50;

    public Text m_diagnosticText1;
    public Text m_diagnosticText2;

    public bool m_useDiagnostic = true;

    public Action<Vector2> SwipeEnd;

    void Start()
    {
        Diagnostic("", "");
    }

    void Diagnostic(string text1, string text2)
    {
        m_diagnosticText1.gameObject.SetActive(m_useDiagnostic);
        m_diagnosticText2.gameObject.SetActive(m_useDiagnostic);

        if (m_diagnosticText1 && m_diagnosticText2)
        {
            m_diagnosticText1.text = text1;
            m_diagnosticText2.text = text2;
        }

    }

    string SwipeDiagnostic(Vector2 swipeMovement)
    {
        string direction = "";

        // horizontal
        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            direction = (swipeMovement.x >= 0) ? "right" : "left";

        }
        // vertical
        else
        {
            direction = (swipeMovement.y >= 0) ? "up" : "down";

        }

        return direction;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                m_touchMovement = Vector2.zero;
                Diagnostic("", "");

            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                m_touchMovement += touch.deltaPosition;

                if (m_touchMovement.magnitude > m_minDragDistance)
                {

                    Diagnostic("Swiping ", m_touchMovement.ToString() + " " + SwipeDiagnostic(m_touchMovement));
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (m_touchMovement.magnitude > m_minSwipeDistance)
                {
                    if (SwipeEnd != null)
                    {
                        SwipeEnd(m_touchMovement);
                    }
                    Diagnostic("Swipe detected", m_touchMovement.ToString() + " " + SwipeDiagnostic(m_touchMovement));
                }
            }


        }
    }
}
