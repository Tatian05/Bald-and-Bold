using System.Collections.Generic;
using UnityEngine;
public class PauseGO : IScreen
{
    Dictionary<Behaviour, bool> _before;
    Transform _root;
    List<Vector2> _rbForces = new List<Vector2>();
    List<string> _rbSimulated = new List<string>();
    public PauseGO(Transform root)
    {
        _before = new Dictionary<Behaviour, bool>();
        _root = root;
    }
    public void Activate()
    {
        var count = 0;
        foreach (var keyValue in _before)
        {
            if (keyValue.Key.CompareTag("SetSkin") || keyValue.Key.GetComponent<AudioListener>()) continue;
            keyValue.Key.enabled = keyValue.Value;
            var rb = keyValue.Key.GetComponent<Rigidbody2D>();
            if (rb)
            {
                if (_rbSimulated.Contains(rb.name))
                    rb.simulated = true;
                rb.WakeUp();
                rb.AddForce(_rbForces[count], ForceMode2D.Impulse);
                count++;
            }
        }
        _rbForces.Clear();
        _before.Clear();
    }
    public void Deactivate()
    {
        foreach (var b in _root.GetComponentsInChildren<Behaviour>())
        {
            if (b.CompareTag("SetSkin") || b.GetComponent<AudioListener>()) continue;
            _before[b] = b.enabled;
            var rb = b.GetComponent<Rigidbody2D>();
            if (rb)
            {
                if (rb.simulated)
                {
                    _rbSimulated.Add(rb.name);
                    rb.simulated = false;
                }
                _rbForces.Add(rb.velocity);
                rb.Sleep();
            }
            b.enabled = false;
        }
    }

    public void PauseObjectsInCinematic()
    {
        foreach (var b in _root.GetComponentsInChildren<Behaviour>())
        {
            if (b.CompareTag("Cinematica") || b.CompareTag("SetSkin")) continue;
            _before[b] = b.enabled;
            var rb = b.GetComponent<Rigidbody2D>();
            if (rb) rb.simulated = false;
            b.enabled = false;
        }
    }
    public void UnpauseObjectsInCinematic()
    {
        foreach (var b in _root.GetComponentsInChildren<Behaviour>())
        {
            if (b.CompareTag("Cinematica") || b.CompareTag("SetSkin")) continue;
            _before[b] = b.enabled;
            var rb = b.GetComponent<Rigidbody2D>();
            if (rb) rb.simulated = true;
            b.enabled = true;
        }
    }
    public void Free()
    {
        GameObject.Destroy(_root.gameObject);
    }
}
