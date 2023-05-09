using UnityEngine;

namespace Character
{
    public class FollowCam : MonoBehaviour
    {
        public MainCharacter character;
        public float speed;
    
        void Update()
        {
            var diff = character.transform.position - transform.position;
            var frameMotion = speed * Time.deltaTime;
            if (diff.magnitude < frameMotion)
                frameMotion = diff.magnitude;
            transform.position += diff;
        }
    }
}
