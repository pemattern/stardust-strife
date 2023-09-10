using UnityEngine;

public class Laser : Projectile
{
    protected override void Update()
    {
        base.Update();

        transform.position += Speed * Time.deltaTime * transform.forward;
    }
}
