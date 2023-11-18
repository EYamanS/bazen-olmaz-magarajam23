using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WalkingSoundManager : MonoBehaviour
{
    private int lastClipIndex = 0;

    [SerializeField]
    WalkMaterialSoundPair[] soundPairs;
    [SerializeField] LayerMask groundLayerMask;

    PhysicsMaterial2D steppingOnMaterial;
    AudioSource _source;

    private AudioClip[] relevantClips;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, Vector2.down, 50, groundLayerMask);

        if (hit.collider != null)
        {

            steppingOnMaterial = hit.collider.sharedMaterial;
            relevantClips = soundPairs.FirstOrDefault(pair => pair._mat == steppingOnMaterial).walkClips;
        }
        else
        {
            relevantClips = null;
        }

    }

    public void WalkStep()
    {
        _source.clip = relevantClips[lastClipIndex % relevantClips.Length];
        _source.PlayOneShot(_source.clip);
        lastClipIndex++;
    }
}

[System.Serializable]
public struct WalkMaterialSoundPair
{
    public PhysicsMaterial2D _mat;
    public AudioClip[] walkClips;
}