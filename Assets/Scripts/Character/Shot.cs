using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Shot : MonoBehaviour
    {
        public float speed;
        public int damage;

        private static readonly Queue<Shot> Shots = new();

        private Rigidbody2D _body;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            Invoke(nameof(ReturnBullet), 1.0f);
        }

        private void FixedUpdate()
        {
            _body.velocity = new Vector2(speed, 0);
        }

        private void ReturnBullet()
        {
            Shots.Enqueue(this);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ReturnBullet();
            var life = other.attachedRigidbody.gameObject.GetComponent<LifeControl>();
            if(life != null) life.DoDamage(damage);
        }

        public static void Fire(Shot prefab, Vector3 position, bool forward)
        { 
            var bullet = Shots.Count != 0 ? Shots.Dequeue() : Instantiate(prefab);
            bullet.transform.position = position;
            bullet.transform.localScale = new Vector3(forward ? 1 : -1, 1, 1);
            bullet.speed = Mathf.Abs(bullet.speed) * (forward ? 1 : -1);
            bullet.gameObject.SetActive(true);
        }
    }
}
