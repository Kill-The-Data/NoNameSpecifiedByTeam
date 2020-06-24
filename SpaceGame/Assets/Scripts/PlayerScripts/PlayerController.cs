using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using EventHandler = SpaceGame.EventHandler;
public class PlayerController : MonoBehaviour
{
    [Header(" --- Setup --- ")] 
    [Tooltip("Hide or Show the gizmos")] 
    [SerializeField] private bool m_showGizmos = false;
    [Header(" --- Movement --- ")]

    [FormerlySerializedAs("Drag")]
    [Tooltip("Modify the Drag (aka how much the Spaceship slows down"),Range(0,1)]
    [SerializeField] private float m_drag = 0.95F;
    
    [FormerlySerializedAs("Acceleration")]
    [Tooltip("Modify the Acceleration of the Spaceship, 0.5F is default")]
    [SerializeField] private float m_acceleration = 0.5F;
    
    [FormerlySerializedAs("RotationSpeed")]
    [Tooltip("Modify the Rotation Speed of the Spacecraft")]
    [SerializeField] private float m_rotationSpeed = 2.0F;

    [Tooltip("By how much the spaceship banks")]
    [SerializeField] private float m_bankingFactor = 0.01F;
    [SerializeField] private Transform m_spaceShipModel;
    private Quaternion m_spaceShipModelInitialRotation = Quaternion.identity;

    [Tooltip("The sound file to play when accelerating")] 
    [SerializeField] private string m_clip;

    [Tooltip("The particles systems to disable when there is no thrust")]
    [SerializeField] private List<ParticleSystem> m_particles;

    [Tooltip("The Radius in which player input is ignored")] 
    [SerializeField] private float m_ignoreRadius = 0;
    
    private static AudioSource m_source = null;
    
    private Vector3 m_speed;
    public const float MIN_EPSILON = 0.0001f;

    private float m_bankAngle = 0;
    
    private Camera mainCamera = null;
    private List<ParticleSystem.EmissionModule> m_emitters = new List<ParticleSystem.EmissionModule>();

    private float m_setDrag;
    
    public void Awake()
    {
        m_setDrag = m_drag;
        if (m_source == null)
        {
            GameObject go = new GameObject("AudioSourceChild");
            go.transform.parent = transform;
            m_source = go.AddComponent<AudioSource>();
            
            SoundManager.ExecuteOnAwake(manager =>
            {
                m_source.clip = manager.GetSound(m_clip);
                m_source.loop = true;
                m_source.Play();
            });
        }
    }

    public void Start()
    {
        mainCamera = Camera.main;
        foreach (var psystem in m_particles)
        {
            m_emitters.Add(psystem.emission);
        }

        EventHandler.Instance.TutorialStart += ResetController;

    }

    //gets called on state enter to reset player 
    public void ResetController()
    {
        Enable();
        transform.position = new Vector3(0, 0, transform.position.z);
        m_speed = Vector3.zero;
    }

    private bool m_isEnabled = true;

    public void Disable() => m_isEnabled = false;
    public void Enable() => m_isEnabled = true;
    
    
    void Update()
    {
        //check if the mouse is held down & check if
        //Input is currently enabled
        if (Input.GetMouseButton(0) && m_isEnabled)
        {
            foreach (var emitter in m_emitters)
            {
                if(!emitter.enabled)
                {
                    var e = emitter;
                    e.enabled = true;
                }
            }
            
            
            var pPos = transform.position;
            
            //transform the mouse position to game coordinates
            Vector2 mPos = Input.mousePosition;
            Vector3 wPos = mainCamera.ScreenToWorldPoint(new Vector3(mPos.x,mPos.y,pPos.z));
            wPos.z = pPos.z;

            //get the relative direction
            var dir = wPos - pPos;
            var magnitude = dir.magnitude;
            
            dir.Normalize();

            if (magnitude > m_ignoreRadius)
            {
                //accelerate towards the direction
                m_speed += dir * (m_acceleration * Time.deltaTime);
                
                //WOOP WOOP, BankAngle PullUP!
                m_bankAngle = Mathf.Acos(Vector3.Dot(m_speed.normalized, dir.normalized));

                m_bankAngle = Mathf.Max(m_bankAngle, 1) * m_bankingFactor;
                
                //add sign to bank-angle
                if (Vector3.Dot(Vector3.forward,Vector3.Cross(m_speed, dir))< 0)
                {
                    m_bankAngle = -m_bankAngle;
                }
                
                //lazy initialize rotation
                if (m_spaceShipModelInitialRotation == Quaternion.identity)
                {
                    m_spaceShipModelInitialRotation = m_spaceShipModel.localRotation;
                }
                
                //assemble the quaternion for the new rotation
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.forward);

                //rotate towards the look-direction
                transform.rotation = Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime * m_rotationSpeed);
            }
        }
        else
        {
            foreach (var emitter in m_emitters)
            {
                if(emitter.enabled)
                {
                    var e = emitter;
                    e.enabled = false;
                }
            }

            m_bankAngle = 0;
        }
        
        //check if the speed is above our epsilon value and apply drag if it is
        if (Math.Abs(m_speed.y) >= MIN_EPSILON || Math.Abs(m_speed.x) >= MIN_EPSILON)
        {
            m_speed *= m_drag;
        } else {
            //below a certain epsilon simply set the speed to 0
            m_speed = Vector3.zero;
        }

        //do euler step
        transform.position += m_speed * Time.deltaTime;

        m_spaceShipModel.localRotation =
            Quaternion.Slerp(
                m_spaceShipModel.localRotation,
                Quaternion.Euler(m_bankAngle,0,0),
                Time.deltaTime);
        m_source.volume = m_speed.magnitude / 100.0f;
    }

    #if (UNITY_EDITOR)

    void OnDrawGizmosSelected()
    {
        if (!m_showGizmos) return;
        var position = transform.position;
        UnityEditor.Handles.DrawLine(position,position+m_speed);
        UnityEditor.Handles.DrawWireDisc(position+m_speed,Vector3.back, 0.1f);
        UnityEditor.Handles.DrawWireDisc(position,Vector3.back,m_ignoreRadius);
        
        if(mainCamera){
            Vector2 mPos = Input.mousePosition;
            Vector3 wPos = mainCamera.ScreenToWorldPoint(new Vector3(mPos.x,mPos.y,position.z));
            UnityEditor.Handles.DrawLine(position,wPos);
        }
    }

    #endif
    
    
    public Vector3 Collide(float factor = 0.95f)
    {
        var transfer = m_speed * factor;
        m_speed *= 1 - factor;
        if(m_speed.sqrMagnitude < 1 ) m_speed = m_speed.normalized * 2;
        return transfer;
    }
    
    public void ResolveCollision(Vector3 direction)
    {
        m_speed += direction.normalized * 10 * Time.deltaTime;
    }
    
    

    public Vector3 GetVelocity() => m_speed;

    public void BeginCutscene()
    {
        m_drag = 0.95f;
        Disable();
    }

    public void FinishCutscene()
    {
        m_drag = m_setDrag;
        Enable();
    }
}
