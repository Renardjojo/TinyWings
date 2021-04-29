using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using SuperBlur;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(PointCollider), typeof(MyRigidbody))]
public class PlayerControler : MonoBehaviour
{
    public float m_thrust = 1f;
    public float raycastVDistance = 1f;

    [SerializeField]
    private float minZCameraPos = -15.0f;

    private MyRigidbody m_rigid;
    
    //UI
    [SerializeField] private TextFloatLink FPS;
    [SerializeField] private TextFloatLink SpeedText;
    [SerializeField] private TextFloatLink BestSpeed;
    [SerializeField] private TextFloatLink ChronoText;
    [SerializeField] private TextFloatLink BestDistanceText;
    [SerializeField] private TextFloatLink DistanceText;

    private float Speed = 0, BestSpeeed = 0, BestDistance = 0;
    [SerializeField] private float Chrono = 60f;
    
    int framesPassed = 0;
    float fpsTotal = 0f;

    public float velocityZoomFactor = 0.5f;
    
    [SerializeField]
    private CinemachineVirtualCamera vcam;

    [SerializeField]
    private SuperBlurFast m_blurScript;

    [Header("End game setting")]
    [SerializeField] private float doneLevelSpeed = 3f;
    [SerializeField] private TextFloatLink FinalScoreText;
    [SerializeField] private GameObject CanvasScoreGO;
    private bool levelIsDone = false;
    


    // Start is called before the first frame update
    void Awake()
    {
        BestSpeeed = PlayerPrefs.GetFloat("BestSpeed", 0);
        BestDistance = PlayerPrefs.GetFloat("BestDistance", 0);
        m_rigid = GetComponent<MyRigidbody>();
    }

    void Start()
    {
        minZCameraPos = vcam.m_Lens.OrthographicSize;

        BestSpeed.SetTextWithRoundFloat(BestSpeeed);
        BestDistanceText.SetTextWithRoundFloat(BestDistance, 0);
    }

    void Update()
    {
        if (Chrono <= 0)
        {
            if (!levelIsDone)
            {
                m_blurScript.enabled = true;
                CanvasScoreGO.SetActive(true);
                FinalScoreText.SetTextWithRoundFloat(transform.position.x, 0);
            }
            
            levelIsDone = true;
            Chrono = 0f;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0f, Time.unscaledDeltaTime / doneLevelSpeed);
            m_blurScript.interpolation = Mathf.Lerp(m_blurScript.interpolation, 1, Time.unscaledDeltaTime / doneLevelSpeed);
        }
        else
        {
            Chrono -= Time.deltaTime; 
        }
        
        ChronoText.SetTextWithRoundFloat(Chrono, 3);
        
        Zoom();

        fpsTotal += 1 / Time.unscaledDeltaTime;
        framesPassed++;

        FPS.SetTextWithRoundFloat(Mathf.RoundToInt(fpsTotal / framesPassed));
    }

    private void Zoom()
    {
        vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, minZCameraPos + Speed * velocityZoomFactor, Time.deltaTime);
    }

    void FixedUpdate()
    {
        Speed = m_rigid.m_velocity.magnitude;

        SpeedText.SetTextWithRoundFloat(Speed);
        DistanceText.SetTextWithRoundFloat(transform.position.x, 0);
        
        if (BestSpeeed < Speed)
        {
            BestSpeeed = Speed;
            BestSpeed.SetTextWithRoundFloat(BestSpeeed);
            PlayerPrefs.SetFloat("BestSpeed", BestSpeeed);
        }
        
        if (BestDistance < transform.position.x)
        {
            BestDistance = transform.position.x;
            BestDistanceText.SetTextWithRoundFloat(BestDistance, 0);
            PlayerPrefs.SetFloat("BestDistance", BestDistance);
        }
        
        if ((Input.touchCount > 0 || Input.GetKey(KeyCode.Space)) && m_rigid.m_isGrounded)
        {
            m_rigid.AddForce(transform.right * m_thrust * Time.fixedDeltaTime * Time.timeScale);
            Debug.DrawLine(transform.position, transform.position + new Vector3((transform.right * m_thrust).x, (transform.right * m_thrust).y, 0f), Color.yellow);
        }
    }

    void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
