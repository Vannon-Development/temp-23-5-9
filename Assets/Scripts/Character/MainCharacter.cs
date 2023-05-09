using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class MainCharacter : MonoBehaviour
    {
        public float walkSpeed;
        public GameObject sprite;
        
        private void OnWalk(InputValue value)
        {
            var val = value.Get<Vector2>();
            if(val.x.NearZero())
                _context.CurrentState.Idle();
            else
                _context.CurrentState.Move(val.x * walkSpeed);
        }

        private void OnJump()
        {
            
        }

        private void OnFire()
        {
            
        }
    
        private StateContext _context;

        private void Start()
        {
            _context = new StateContext()
            {
                Body = GetComponent<Rigidbody2D>(),
                Ani = GetComponent<Animator>(),
                Sprite = sprite
            };
            _context.CurrentState = _context.States[(int)StateContext.StateName.Idle];
            _context.CurrentState.Begin(_context);
        }

        private class StateContext
        {
            public State CurrentState;
            public Rigidbody2D Body;
            public Animator Ani;
            public float CurrentMotion;
            public GameObject Sprite;

            public readonly State[] States =
            {
                new IdleState(),
                new WalkState(),
                new JumpState(),
                new FallState()
            };
            
            public enum StateName { Idle, Walk, Jump, Fall }

        }

        private abstract class State
        {
            protected StateContext Context;

            public virtual void Begin(StateContext context)
            {
                Context = context;
            }
            
            protected void ChangeState(StateContext.StateName state)
            {
                Context.CurrentState = Context.States[(int)state];
                Context.CurrentState.Begin(Context);
            }

            public virtual void Idle() {}
            public virtual void Move(float value) {}
        }

        private class IdleState : State
        {
            public override void Begin(StateContext context)
            {
                base.Begin(context);
                Context.Ani.SetBool("walking", false);
                Context.Body.velocity = Vector2.zero;
                
            }

            public override void Move(float value)
            {
                base.Move(value);
                Context.CurrentMotion = value;
                ChangeState(StateContext.StateName.Walk);
            }
        }

        private class WalkState : State
        {
            public override void Begin(StateContext context)
            {
                base.Begin(context);
                Context.Ani.SetBool("walking", true);
                Context.Body.velocity = new Vector2(Context.CurrentMotion, 0);
                Context.Sprite.transform.localScale = new Vector2(Context.CurrentMotion < 0 ? -1 : 1, 1);
            }

            public override void Idle()
            {
                base.Idle();
                ChangeState(StateContext.StateName.Idle);
            }
        }

        private class JumpState : State
        {
            
        }

        private class FallState : State
        {
            
        }
    }
}
