using UnityEngine;

namespace Character
{
    public class FollowCam : MonoBehaviour
    {
        public GameObject character;
    
        private void FixedUpdate()
        {
            transform.position = new Vector3(character.transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
