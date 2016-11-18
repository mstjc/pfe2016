using UnityEngine;
using UnityEngine.UI;
using System;

public class GunHUD : MonoBehaviour {

    public Gun _Gun;
    public Image _FillImage;
    public Text _Text;
    public Color _FullColor = Color.green;
    public Color _EmptyColor = Color.red;
    private bool _Blink = false;

    public void OnEnable()
    {
        if (_Gun != null)
        {
            _Gun.Fired += OnGunFired;
            _Gun.Reloaded += OnGunReloaded;
            _Gun.FinishedReloading += OnFinishedReloading;
            RefreshHUD();
        }
    }

    public void OnDisable()
    {
        if(_Gun != null)
        {
            _Gun.Fired -= OnGunFired;
            _Gun.Reloaded -= OnGunReloaded;
            _Gun.FinishedReloading -= OnFinishedReloading;
        }
    }

    public void FixedUpdate()
    {
        if(_Blink)
        {
            _Text.color = new Color(_Text.color.r, _Text.color.g, _Text.color.b, Mathf.PingPong(Time.time * 3, 1.0f) + 0.2f);
            _FillImage.color = _Text.color;
        }
        else
        {
        }
    }

    private void OnGunFired(object sender, System.EventArgs e)
    {
        RefreshHUD();
    }

    private void OnGunReloaded(object sender, EventArgs e)
    {
        RefreshHUD();
    }

    private void OnFinishedReloading(object sender, System.EventArgs e)
    {
        RefreshHUD();
    }

    private void RefreshHUD()
    {
        if(_Gun != null)
        {
            _Blink = _Gun.NbBullets <= 2;

            _Text.text = _Gun.NbBullets.ToString();
            _FillImage.color = Color.Lerp(_EmptyColor, _FullColor, ((float)_Gun.NbBullets) / ((float)_Gun.NbBulletsInShell));
            _Text.color = _FillImage.color;
        }
    }
}
