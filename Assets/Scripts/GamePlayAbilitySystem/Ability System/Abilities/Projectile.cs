using System.Collections;
using AbilitySystem;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    public AbilitySystemCharacter Source;
    //Source where Projectile instantiated

    [SerializeField]
    public AbilitySystemCharacter Target;
    //Target where Projectile attack to xx

    [SerializeField]
    private Vector3 Speed;

    [SerializeField]
    private Vector3 Acceleration;

    public IEnumerator TravelToTarget() //Move to target
    {
        Vector3 actualSpeed = Speed;
        while (Vector3.Distance(Target.transform.position, this.transform.position) > 0.2)
        {
            // Direction of travel
            var direction = (Target.transform.position - this.transform.position).normalized;
            this.transform.position += Vector3.Scale(direction, Speed) * Time.deltaTime;
            Speed += Acceleration * Time.deltaTime;
            yield return null;
        }

        yield break;
    }
}
