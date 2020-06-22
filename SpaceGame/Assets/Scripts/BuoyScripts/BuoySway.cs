using UnityEngine;

public class BuoySway : AReset, IObserver
{
    [SerializeField] private Vector2 m_direction;
    [SerializeField] private float m_impulse;
    [SerializeField] private MotherShipCollisionHandler m_collisionHandler;
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private float m_heft;
    [SerializeField] private TimerView m_tview;

    private float m_maxFuel;

    [Range(0,1)]
    [SerializeField] private float m_movement = 1;
    
    private Vector2 m_collisionImpulse;

    private float m_time;
    private bool m_doGetUpdate = false;

    private Quaternion m_initialRotation;
    private Vector3 m_initialPosition;
    private Vector2 m_initialDirection;
    
    private void Awake()
    {
        m_initialDirection = m_direction;
        m_collisionHandler.collision += Collide;
        m_initialRotation = transform.localRotation;
        m_initialPosition = transform.position;
        
        Reset();
        GetComponentInChildren<BuoyFillUp>().onCargoFilled += cargo =>
        {
            m_doGetUpdate = false;
        };

        if (m_tview)
        {
            m_tview.OnTimerInit(timer =>
            {
                m_maxFuel = m_tview.GetMaxFuel();
                timer.Attach(this);
            });
        }
    }

    public override void Reset()
    {
        m_direction = m_initialDirection * m_heft;
        m_doGetUpdate = true;
    }
    
    
    void Update()
    {

        m_time += Time.deltaTime;
        
        //get angle
        var Phi = Mathf.Sin(m_time) * m_impulse;
        
        //get rotation axis
        var Axis = Vector2.Perpendicular(m_direction.normalized);
        
        //rotate around axis
        transform.localRotation = m_initialRotation * Quaternion.AngleAxis(Phi,new Vector3(Axis.x,0,Axis.y));

        transform.position = Vector3.Lerp(transform.position, m_initialPosition + m_direction.normalized.Promote() * (Phi * m_movement),Time.deltaTime);

    }

    private void Collide(ISubject subject)
    {
        var v3 = m_playerController.GetVelocity();
        
        m_direction.x += v3.x;
        m_direction.y += v3.y;
    }

    public void GetUpdate(ISubject subject)
    {
        if(!m_doGetUpdate) return;
        if (subject is Timer timer)
        {
            m_movement = timer.GetTime() / m_maxFuel;
        }
    }
}
