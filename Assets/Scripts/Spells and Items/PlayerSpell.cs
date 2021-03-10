using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    public GameObject projectile;
    public float minDamage;
    public float maxDamage;
    public float projectileForce;
    public bool limitedUse = false;
    public int usesLeft = 1;
    public bool rotateTowardsVelocity = true;

    public PlayerMovement player;

    [SerializeField]
    public Sprite spellIcon;

    public virtual void Start()
    {
        Use();
    }
    public virtual void Use()
    {
        Vector3 direction = player.dir;
        Quaternion rot = Quaternion.identity;
        if (rotateTowardsVelocity)
        {
            rot = Quaternion.Euler(0, Vector3.Angle(direction, Vector3.right), 0);
        }
        Vector3 spawnPos = transform.position + direction.normalized;

        if (!Physics2D.OverlapCircle(spawnPos, .1f))
        {
            GameObject spell = Instantiate(projectile, spawnPos, rot);
            spell.GetComponent<ProjectileManager>().moveDirection = direction.normalized * projectileForce;
            spell.GetComponent<ProjectileManager>().damage = Random.Range(minDamage, maxDamage);
            Destroy(gameObject);
        }
    }
}
