using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class MainCharacter : MonoBehaviour
    {
        private void OnWalk(InputValue value)
        {
            var val = value.Get<Vector2>();
            if (val.magnitude.NearZero())
                _context.CurrentState.Idle();
            else
                _context.CurrentState.Move(val);
        }
    
        private StateContext _context;

        private void Start()
        {
            _context = new StateContext()
            {
                Body = GetComponent<Rigidbody2D>()
            };
            _context.CurrentState = _context.States[(int)StateContext.StateName.Idle];
            _context.CurrentState.Begin(_context);
        }

        private class StateContext
        {
            public State CurrentState;
            public Rigidbody2D Body;
            
            public readonly State[] States =
            {
                new IdleState(),
                new WalkState()
            };
            
            public enum StateName { Idle, Walk }

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
            public virtual void Move(Vector2 value) {}
        }

        private class IdleState : State
        {
            public override void Move(Vector2 value)
            {
                base.Move(value);
                
            }
        }

        private class WalkState : State
        {
            
        }
    }
}
