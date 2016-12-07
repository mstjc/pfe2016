using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUpdating : MonoBehaviour {

    [SerializeField]
    private Text _CurrentStageText;

    [SerializeField]
    private Text _EnnemiesRemainingText;

    public void UpdateStage(int stage)
    {
        _CurrentStageText.text = stage.ToString();
    }

	public void UpdateStage(string stage)
	{
		_CurrentStageText.text = stage;
	}

	public void UpdateEnnemiesRemaining(int ennemies)
    {
        _EnnemiesRemainingText.text = ennemies.ToString();
    }
}
