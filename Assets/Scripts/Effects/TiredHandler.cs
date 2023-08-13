using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class TiredHandler : MonoBehaviour
{
    [SerializeField]
    private Volume darkVolume;

   
    private float tiredness = 0;
    public float Tiredness { get { return tiredness / 1.5f; } set { tiredness = value * 1.5f; } }
    private Vignette vignetteDark;
    private ColorAdjustments adjustment;
    [SerializeField]
    private float timeUntilAsleep = 20;
    private float _timeUntilAsleep;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        darkVolume.profile.TryGet<Vignette>(out vignetteDark);
        darkVolume.profile.TryGet<ColorAdjustments>(out adjustment);
        _timeUntilAsleep = timeUntilAsleep;
    }

    

    // Update is called once per frame
    void Update()
    {
        vignetteDark.intensity.Override(tiredness + 0.3f);
        float modified = Mathf.Clamp(tiredness - 0.7f, 0, 1);
        modified = 1 - modified;
        adjustment.colorFilter.Override(new Color(modified, modified, modified));

        if (!isPaused)
        {
            
            this.Tiredness = (timeUntilAsleep-_timeUntilAsleep) / timeUntilAsleep;
            _timeUntilAsleep = Mathf.Clamp(_timeUntilAsleep - Time.deltaTime,0,timeUntilAsleep);
        }
       
    }
    public void SetPause(bool pause)
    {
        this.isPaused = pause;
    }
    public void ResetTired()
    {
        this._timeUntilAsleep = timeUntilAsleep;
    }
}
