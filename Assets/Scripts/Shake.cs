using UnityEngine;

public class Shake : MonoBehaviour
{
    public Animator camAnim;

    public void CamShake()
    {
        camAnim.SetTrigger("shake");
    }

    public void CamShakeStop()
    {
        camAnim.ResetTrigger("shake");
    }
}
