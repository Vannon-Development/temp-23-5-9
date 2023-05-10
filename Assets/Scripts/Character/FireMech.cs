using UnityEngine;

namespace Character
{
    public class FireMech : MonoBehaviour
    {
        public Shot shot;
        public Transform shotPosition;
        public Transform flipTransform;

        public void OnFire()
        {
            Shot.Fire(shot, shotPosition.position, flipTransform.localScale.x > 0);
        }
    }
}
