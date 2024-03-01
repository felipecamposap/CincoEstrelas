using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public Volume volume;
    ChromaticAberration chromaAbe;


    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        volume = Object.FindAnyObjectByType<Volume>();
        volume.profile.TryGet<ChromaticAberration>(out chromaAbe);
    }

    public void Intensity(Slider _slider)
    {
        chromaAbe.intensity.value = _slider.value;
    }
}
