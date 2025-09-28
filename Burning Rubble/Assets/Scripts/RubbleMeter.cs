using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class RubbleMeter : MonoBehaviour
{
    private int currRubbleAmt;
    [SerializeField] private int MAX_AMT;
    [SerializeField] private Image rubbleBar;
    [SerializeField] private TextMeshProUGUI rubbleText;
    [SerializeField] private int rubbleChargeAmt = 100;
    private InputAction rubbleAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rubbleAction = InputSystem.actions.FindAction("Rubble");
    }

    // Update is called once per frame
    void Update()
    {
        if(rubbleAction.WasPerformedThisFrame()) UseRubble(rubbleChargeAmt) ; //UseRubble(rubbleChargeAmt);
    }

    public void GainRubble(int rubble)
    {
        currRubbleAmt = Mathf.Clamp(currRubbleAmt + rubble, 0, MAX_AMT);
        //Debug.Log(currRubbleAmt);
        UpdateUI();
    }

    public void UseRubble(int rubble)
    {
        if (currRubbleAmt >= rubble)
        {
            currRubbleAmt -= rubble;
            UpdateUI();
            Debug.Log("I ran");
        }
    }

    private void UpdateUI()
    {
        if (currRubbleAmt == MAX_AMT)
        {
            rubbleBar.fillAmount = 1;
            rubbleText.text = "MAX";
        }
        else
        {
            int rubbleCharges = currRubbleAmt / rubbleChargeAmt;
            rubbleText.text = "Rubble: " + rubbleCharges;
            float barFill = ((currRubbleAmt*1.0f - (rubbleCharges * rubbleChargeAmt*1.0f)) / rubbleChargeAmt*1.0f);
            rubbleBar.fillAmount = barFill;
        }
    }
}
