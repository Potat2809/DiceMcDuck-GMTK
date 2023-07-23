
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform Player;
    public Vector3 offset;
    [Range(1,10)]
    public float smoothfactor;

    private void LateUpdate()
    {
        follow();
    }

    private void follow()
    {
        Vector3 Playerposition = Player.position + offset;
        Vector3 smoothposition = Vector3.Lerp(transform.position, Playerposition, smoothfactor*Time.fixedDeltaTime);
        transform.position = Playerposition;
    }
}
