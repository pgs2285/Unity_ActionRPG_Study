using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    #region Variables

    public float speed;
    public GameObject muzzleEffect;   // 총구 이펙트 등, 발사체 부위의 이펙트    
    public GameObject hitEffect;        // 적중 이펙트 등, 적중시 이펙트    
    public AudioClip shotFSX;           // 발사음
    public AudioClip hitFSX;             // 적중음

    private bool collided;                  // 적에게 적중했나 여부
    private Rigidbody rb;
    [HideInInspector] private AttackBehaviour attackBehaviour;  // 쿨타임, 데미지 요소를 불러오기위해 AttackBehaviour component 저장.
    [HideInInspector] public GameObject owner;  // 발사체를 발사한 객체를 저장하기 위해 사용.
    [HideInInspector] public GameObject target;  // 발사체가 명중한 객체를 저장하기 위해 사용.
#endregion Variables
    // Start is called before the first frame update
    void Start()
    {
        if (target)
        {
            Vector3 dest = target.transform.position;
            dest.y += 1.5f; // 빌끝이 아닌 몸통에 맞게 하기 위해 y축을 1.5만큼 높여준다. 추후 수정이 필요하다.
            transform.LookAt(dest); // 바라보게 하는 함수인데... 3d에서만 사용가능하므로 주의해서 사용하자.
        }
        if (owner)
        {
            Collider projectileCollider = GetComponent<Collider>();
            Collider[] ownerColliders = owner.GetComponentsInChildren<Collider>();

            foreach (Collider col in ownerColliders)
            {
                Physics.IgnoreCollision(projectileCollider, col); // 두 객체의 충돌을 무시한다.
            }
        }

        if (muzzleEffect)   // 만약 발사 이펙트가 있다면,
        {
            GameObject muzzleVFX = Instantiate(muzzleEffect, transform.position, Quaternion.identity); // 발사 이펙트를 생성한다.
            muzzleVFX.transform.forward = gameObject.transform.forward;
            ParticleSystem particleSystem = muzzleVFX.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                Destroy(muzzleVFX, particleSystem.main.duration);
            }
            else
            {// particle system이 자식에 있을때를 방지 
                ParticleSystem childParticleSystem = muzzleVFX.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
                if(childParticleSystem)
                {
                    Destroy(muzzleVFX, childParticleSystem.main.duration);
                }
                
            }
        }

        if (shotFSX && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(shotFSX);// playOneShot: 한번만 재생
        }
    }

    private void FixedUpdate()
    {
        if (speed != 0 && rb != null)
        {
            rb.position = (transform.position) * (speed * Time.deltaTime);
        }
    }

    protected void OnCollisionEnter(Collision col)
    {
        if (collided) return;   // 이미 한번 충돌이 되었다면 바로 return.
        collided = true;
        
        Collider projectileCollider = GetComponent<Collider>();
        projectileCollider.enabled = false;
        
        if(hitFSX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(hitFSX);
        }

        speed = 0;
        rb.isKinematic = true;  // 물리엔진을 끈다.
        
        ContactPoint contact = col.contacts[0]; // 처음 충돌한 지점을 저장한다.  
        Quaternion contactRotation = Quaternion.FromToRotation(Vector3.up, contact.normal); // 충돌한 지점의 법선벡터를 구한다.
        // Quaternion : 회전을 표현하는 클래스. FromToRotation : 두 벡터 사이의 회전을 구한다.
        // contact.normal 은 충돌한 지점의 법선벡터를 리턴한다.
        Vector3 contactPoint = contact.point; // 충돌한 지점을 저장한다.

        if (hitEffect)
        {
            GameObject hitVFX = Instantiate(hitEffect, contactPoint, contactRotation); // 충돌 이펙트를 생성한다.
        }

    }
}
