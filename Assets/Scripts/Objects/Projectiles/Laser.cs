using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Laser : Projectile
{
    protected override void Update()
    {
        base.Update();

        transform.position += Speed * Time.deltaTime * transform.forward;
    }
}
