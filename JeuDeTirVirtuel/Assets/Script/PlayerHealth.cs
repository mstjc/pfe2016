using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    [SerializeField]
    private float _StartingHealth = 100f;
    [SerializeField]
    private Slider _Slider;
    [SerializeField]
    private Image _FillImage;
    [SerializeField]
    private Color _FullHealthColor = Color.green;
    [SerializeField]
    private Color _ZeroHealthColor = Color.red;
    [SerializeField]
    private GameManager _GM;

    private float _CurrentHealth;
    private bool _Dead;


    private void OnEnable()
    {
        _CurrentHealth = _StartingHealth;
        _Dead = false;

        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
        // Adjust the player's current health, update the UI based on the new health and check whether or not the player is dead.
        _CurrentHealth -= amount;

        SetHealthUI();

        if (_CurrentHealth <= 0f && !_Dead)
        {
            OnDeath();
        }
    }

    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        _Slider.value = _CurrentHealth;

        _FillImage.color = Color.Lerp(_ZeroHealthColor, _FullHealthColor, _CurrentHealth / _StartingHealth);
    }

    private void OnDeath()
    {
        _Dead = true;
        _GM.Reset();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
