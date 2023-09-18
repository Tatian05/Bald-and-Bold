using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Shadow : MonoBehaviour
{
    [SerializeField] ShadowCaster2D _shadowCaster;
    public void ShadowToggle(bool toggle) => _shadowCaster.enabled = toggle;
}
