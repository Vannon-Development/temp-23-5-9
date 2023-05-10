using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class WalkMech : MonoBehaviour
    {
        public float speed;
        public GameObject flipTransform;

        private Rigidbody2D _body;
        private Animator _ani;
        private static readonly int Walking = Animator.StringToHash("walking");

        private void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _ani = GetComponent<Animator>();
        }

        public void OnWalk(InputValue value)
        {
            var xVal = value.Get<Vector2>().x;
            if (xVal.NearZero())
            {
                _ani.SetBool(Walking, false);
                _body.velocity = new Vector2(0, _body.velocity.y);
            }
            else
            {
                _ani.SetBool(Walking, true);
                _body.velocity = new Vector2(speed * xVal, _body.velocity.y);
                flipTransform.transform.localScale = new Vector3(xVal < 0 ? -1 : 1, 1, 1);
            }
        }
    }
}
