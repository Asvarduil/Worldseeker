using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorControl : DebuggableBehavior
{
    #region Variables / Properties

    public List<string> RecognizedEntities;
    public bool IsOpen = false;
    public Lockout DoorLockout;

    public string ColorProperty;
    public float FieldFadeOutRate = 0.5f;
    public float FieldFadeInRate = 0.5f;

    public List<string> AuthorizedWeapons = new List<string>();

    private BoxCollider _collider;
    private Material _doorMaterial;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _doorMaterial = renderer.material;
    }

    public void Update()
    {
        if (!IsOpen)
            return;

        if(DoorLockout.CanAttempt())
        {
            StartCoroutine(CloseDoor());
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        DebugMessage("A collision occurred!");

        if (!RecognizedEntities.Contains(collision.collider.tag))
        {
            DebugMessage("Tag " + collision.collider.tag + " cannot open this door.");
            return;
        }

        var projectileClassifier = collision.collider.gameObject.GetComponent<WeaponClassification>();
        if (projectileClassifier == null)
        {
            DebugMessage("No Weapon Classification was found on the impacting projectile.  Ignoring impact.");
            return;
        }

        CheckWeaponAuthorization(projectileClassifier.Classification);
    }

    #endregion Hooks

    #region Methods

    public void CheckWeaponAuthorization(string weaponId)
    {
        if(! AuthorizedWeapons.Contains(weaponId))
        {
            DebugMessage("This weapon cannot open this door.");
            // TODO: Send notification to UI that the current weapon does not open this door.
            return;
        }

        StartCoroutine(OpenDoor());
    }

    public IEnumerator OpenDoor()
    {
        DebugMessage("Opening door...");

        DoorLockout.NoteLastOccurrence();
        IsOpen = true;

        _collider.enabled = false;

        float alphaRate = 1.0f;
        while(Mathf.Abs(alphaRate - 0.0f) > 0.001f)
        {
            Color current = renderer.material.GetColor(ColorProperty);
            alphaRate = current.a;
            alphaRate = Mathf.Lerp(alphaRate, 0.0f, FieldFadeOutRate);

            current.a = alphaRate;
            renderer.material.SetColor(ColorProperty, current);

            yield return null;
        }
    }

    public IEnumerator CloseDoor()
    {
        DebugMessage("Closing door...");

        float alphaRate = 0.0f;
        while (Mathf.Abs(alphaRate - 1.0f) > 0.001f)
        {
            Color current = renderer.material.GetColor(ColorProperty);
            alphaRate = current.a;
            alphaRate = Mathf.Lerp(alphaRate, 1.0f, FieldFadeInRate);

            current.a = alphaRate;
            renderer.material.SetColor(ColorProperty, current);

            yield return null;
        }

        _collider.enabled = true;
        IsOpen = false;

        yield return null;
    }

    #endregion Methods
}
