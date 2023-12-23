using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] Volume volume;
    ChromaticAberration chromaAbe;


    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet<ChromaticAberration>(out chromaAbe);
    }

    public void Intensity(Slider _slider)
    {
        chromaAbe.intensity.value = _slider.value;
    }
}
