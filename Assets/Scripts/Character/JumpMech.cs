using System;
using UnityEngine;

namespace Character
{
    public class JumpMech : MonoBehaviour
    {
        public float force;
        
        private Rigidbody2D _body;
        private Animator _ani;
        private static readonly int InAir = Animator.StringToHash("inAir");

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _ani = GetComponent<Animator>();
        }

        private void OnJump()
        {
            if (_body.velocity.y.NearZero())
            {
                _body.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
                _ani.SetBool(InAir, true);
            }
        }
        
        private void FixedUpdate()
        {
            if(_body.velocity.y.NearZero())
                _ani.SetBool(InAir, false);
            else
                _ani.SetBool(InAir, true);
        }
    }
}
