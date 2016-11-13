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
        //StartCoroutine(FlashLabel());
    }
	
	void Start ()
    {
        StartCoroutine(FlashLabel());
    }
	
	// Update is called once per frame
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
