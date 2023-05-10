using System;
using BehaviorTree;
using UnityEngine;

namespace Character
{
    public class AlienController : MonoBehaviour
    {
        public TextAsset aiDocument;
        public float moveSpeed;
        public float attackHoldTime;
        public float attackSpeed;
        
        private class Context : TreeContext
        {
            public GameObject Player;
            public GameObject Alien;
            public float MoveSpeed;
            public Rigidbody2D Body;
            public Animator Ani;
            public float AttackHoldTime;
            public float AttackSpeed;
            public float BaseY;
        }

        private BehaviorTree<Context> _tree;
        private Context _context;
        
        private static readonly int Moving = Animator.StringToHash("moving");

        private void Start()
        {
            _context = new Context()
            {
                Player = GameObject.FindGameObjectWithTag("Player"),
                Alien = gameObject,
                MoveSpeed = moveSpeed,
                Body = GetComponent<Rigidbody2D>(),
                Ani = GetComponent<Animator>(),
                AttackHoldTime = attackHoldTime,
                AttackSpeed = attackSpeed,
                BaseY = transform.position.y
            };
            _tree = new BehaviorTree<Context>(_context);
            _tree.RegisterNode("InRange", typeof(PlayerInRange));
            _tree.RegisterNode("MoveLeft", typeof(MoveLeft));
            _tree.RegisterNode("Hold", typeof(HoldPos));
            _tree.RegisterNode("Attack", typeof(AttackPlayer));
            _tree.RegisterNode("Return", typeof(ReturnToBase));
            _tree.SetRoot(aiDocument.text);
        }

        private void Update()
        {
            _tree.Tick();
        }

        private class PlayerInRange : LeafNode<Context>
        {
            public PlayerInRange(Context context) : base(context) { }

            public override Status Tick()
            {
                var dist = Mathf.Abs(Context.Alien.transform.position.x - Context.Player.transform.position.x);
                if(dist < 4.0f)
                    return Status.Success;
                return Status.Failure;
            }
        }

        private class HoldPos : LeafNode<Context>
        {
            private bool _reset = true;
            private float _time;
            
            public HoldPos(Context context) : base(context) { }

            public override Status Tick()
            {
                if (_reset)
                {
                    _time = 0;
                    _reset = false;
                }
                
                Context.Body.velocity = Vector2.zero;
                Context.Ani.SetBool(Moving, false);

                if (_time > Context.AttackHoldTime)
                {
                    _reset = true;
                    return Status.Success;
                }

                _time += Time.deltaTime;
                return Status.Running;
            }
        }

        private class AttackPlayer : LeafNode<Context>
        {
            private bool _reset = true;
            private float _time;
            private Vector2 _direction;
            
            public AttackPlayer(Context context) : base(context) { }

            public override Status Tick()
            {
                if (_reset)
                {
                    _direction = Context.Player.transform.position - Context.Alien.transform.position;
                    _time = _direction.magnitude / Context.AttackSpeed;
                    _direction.Normalize();
                    _reset = false;
                }

                Context.Ani.SetBool(Moving, true);
                if (_time <= 0)
                {
                    _reset = true;
                    return Status.Success;
                }

                _time -= Time.deltaTime;
                Context.Body.velocity = _direction * Context.AttackSpeed;
                Context.Ani.SetBool(Moving, true);
                return Status.Running;
            }
        }

        private class ReturnToBase : LeafNode<Context>
        {
            private bool _reset = true;
            private Vector2 _direction;
            private float _time;
            
            public ReturnToBase(Context context) : base(context) { }

            public override Status Tick()
            {
                if (_reset)
                {
                    var dist = Context.BaseY - Context.Alien.transform.position.y;
                    _direction = new Vector2(Mathf.Abs(dist) * -1.5f, dist);
                    _time = _direction.magnitude / Context.MoveSpeed;
                    _direction.Normalize();
                    _reset = false;
                }

                if (_time <= 0)
                {
                    _reset = true;
                    return Status.Success;
                }

                _time -= Time.deltaTime;
                Context.Body.velocity = _direction * Context.MoveSpeed;
                Context.Ani.SetBool(Moving, true);
                return Status.Running;
            }
        }

        private class MoveLeft : LeafNode<Context>
        {
            public MoveLeft(Context context) : base(context) { }

            public override Status Tick()
            {
                Context.Body.velocity = new Vector2(-1 * Context.MoveSpeed, 0);
                Context.Ani.SetBool(Moving, true);
                return Status.Success;
            }
        }
    }
}
