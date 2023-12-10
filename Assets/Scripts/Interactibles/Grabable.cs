using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Grabable : NetworkBehaviour
{
    public abstract bool OnGrab(GameObject Hand);
    public abstract void OnRelease();
    public virtual void OnPrimaryAction() { }
    public virtual void OnSecondaryAction() { }
}
