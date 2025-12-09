using UnityEngine;
using TMPro;

public class JoinScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI p1Text;
    [SerializeField] private TextMeshProUGUI p2Text;
    [SerializeField] private TextMeshProUGUI p3Text;
    [SerializeField] private TextMeshProUGUI p4Text;

    public void SetPlayerJoinedText(int player)
    {
        switch(player)
        {
            case 1:
                p1Text.SetText("P1 READY");
                break;
            case 2:
                p2Text.SetText("P2 READY");
                break;
            case 3:
                p3Text.SetText("P3 READY");
                break;
            case 4:
                p4Text.SetText("P4 READY");
                break;
        }
    }

    public void ClearJoinScreen()
    {
        Destroy(gameObject);
    }
}
