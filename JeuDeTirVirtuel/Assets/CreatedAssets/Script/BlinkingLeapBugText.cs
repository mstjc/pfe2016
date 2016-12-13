using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkingLeapBugText : MonoBehaviour {

    private Text _Component;

    void Awake()
    {
        _Component = GetComponent<Text>();
    }

    void OnActivate()
    {
    }
	
	void Start ()
    {
        StartCoroutine(FlashLabel());
    }
	
	void Update ()
    {
	
	}

    IEnumerator FlashLabel()
    {
        while(true)
        {
            _Component.text = "";
            yield return new WaitForSeconds(.7F);
            _Component.text = "<< Press Here >>";
            yield return new WaitForSeconds(.7F);
        }
    }
}
