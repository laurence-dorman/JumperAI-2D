using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    [SerializeField] GameManager gameManager;
    Text scoreText;
    Text modeTrackerText;
    Text modeTitleText;

    Button manualButton;
    Button RBSButton;
    Button FISButton;
    Toggle failsafeToggle;


    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "modeTracker")
            {
                modeTrackerText = child.gameObject.GetComponent<Text>();
            }
            else if (child.tag == "Score")
            {
                scoreText = child.gameObject.GetComponent<Text>();
            }
            else if (child.tag == "modeTitle")
            {
                modeTitleText = child.gameObject.GetComponent<Text>();
            }
            else if (child.tag == "ManualButton")
            {
                manualButton = child.gameObject.GetComponent<Button>();
            }
            else if (child.tag == "RBSButton")
            {
                RBSButton = child.gameObject.GetComponent<Button>();
            }
            else if (child.tag == "FISButton")
            {
                FISButton = child.gameObject.GetComponent<Button>();
            }
            else if (child.tag == "Failsafe")
            {
                failsafeToggle = child.gameObject.GetComponent<Toggle>();
                gameManager.player.failsafe = failsafeToggle.isOn;
            }
        }

        scoreText.text = gameManager.GetScore().ToString();

        manualButton.onClick.AddListener(delegate{ChangeState(PlayerScript.ControlState.Manual);});
        RBSButton.onClick.AddListener(delegate{ChangeState(PlayerScript.ControlState.RBS);});
        FISButton.onClick.AddListener(delegate{ChangeState(PlayerScript.ControlState.Fuzzy);});
        failsafeToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(failsafeToggle);});
    }

    void ToggleValueChanged(Toggle change)
    {
        gameManager.player.failsafe = change.isOn;
    }

    void ChangeState(PlayerScript.ControlState state)
    {
        gameManager.player.controlState = state;
    }

    void SetModeTracker()
    {
        modeTrackerText.text = "Current mode: ";
        switch (gameManager.player.controlState)
        {
            case PlayerScript.ControlState.Manual:
                {
                    modeTrackerText.text += "Manual";
                    break;
                }
            case PlayerScript.ControlState.RBS:
                {
                    modeTrackerText.text += "RBS";
                    break;
                }
            case PlayerScript.ControlState.Fuzzy:
                {
                    modeTrackerText.text += "Fuzzy";
                    break;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = gameManager.GetScore().ToString();
        SetModeTracker();
    }
}
