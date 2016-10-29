using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUpdating : MonoBehaviour {

    [SerializeField]
    public Text _EnnemiText, _StageText;


    public void UpdateEnnemis(int number)
    {
        _EnnemiText.text = number.ToString();
    }

    public void UpdateStage(int number)
    {
        _StageText.text = number.ToString();
    }

}
