using System;
using UnityEngine;

public class IonBomb : Projectile
{
    public Func<bool> DetonateCondition;

    protected override void Update()
    {
        base.Update();

        if(DetonateCondition())
            Destroy(gameObject);

        transform.position += Speed * Time.deltaTime * transform.forward;
    }
}
