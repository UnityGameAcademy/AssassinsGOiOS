using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInput : MonoBehaviour
{
    // store horizontal input
    float m_h;
    public float H { get { return m_h; } }

    // store the vertical input 
    float m_v;
    public float V { get { return m_v; } }

    // global flag for enabling and disabling user input
    bool m_inputEnabled = false;
    public bool InputEnabled { get { return m_inputEnabled; } set { m_inputEnabled = value; } }

    public TouchController touchController;


    public void ClearInput()
    {
        m_h = 0f;
        m_v = 0f;
    }

    // get keyboard input
    public void GetKeyInput()
    {
        // if input is enabled, just get the raw axis data from the Horizontal and Vertical virtual axes (defined in InputManager)
        if (m_inputEnabled)
        {
            m_h = Input.GetAxisRaw("Horizontal");
            m_v = Input.GetAxisRaw("Vertical");
        }
        // if input is disabled, ensure that extra key input does not cause unintended movement
        else
        {
            ClearInput();
        }
    }

    public void GetInput()
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER

        // if we are building for Mac/PC, use the keyboard input
        GetKeyInput();
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        // if using a touchscreen, no need to do anything extra
        // our OnSwipeEnd event handler is responding to the TouchController's swipe Actions

#endif
    }



#if UNITY_STANDALONE || UNITY_WEBPLAYER


    //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

    // subscribe to the TouchController's Swipe Event
    private void Awake()
    {
        if (touchController != null)
        {
            touchController.SwipeEnd += OnSwipeEnd;
        }
    }

    // if this object is destroyed, unsubscribe 
    private void OnDestroy()
    {
        if (touchController != null)
        {
            touchController.SwipeEnd -= OnSwipeEnd;
        }
    }

    // event handler; this is triggered whenever the TouchController registers a swipe
    public void OnSwipeEnd(Vector2 inputVector)
    {
        // if input is not enabled, do nothing
        if (!m_inputEnabled)
        {
            return;
        }

        // horizontal swipe
        if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
        {
            // set our horizontal input to either 1 or -1
            m_h = (inputVector.x >= 0f) ? 1f : -1f;
            m_v = 0f;

        }
        // vertical swipe
        else
        {
            // set the vertical input to 1 or -1
            m_h = 0f;
            m_v = (inputVector.y >= 0f) ? 1f : -1f;

        }
    }



#endif //End of mobile platform dependendent compilation section started above with #elif


}

