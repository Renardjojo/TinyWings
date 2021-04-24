using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
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
    [SerializeField] private TextFloatLink S;
    [SerializeField] private TextFloatLink MS;
    [SerializeField] private TextFloatLink V;
    [SerializeField] private TextFloatLink MV;

    private float s = 0, ms = 0, v = 0, mv = 0;

    [SerializeField]
    private CinemachineVirtualCamera vcam;
    
    public float velocityZoomFactor = 0.5f;

    /* Public Variables */
    public float FPSDisplayfrequency = 0.25f;
    
    private IEnumerator FPSCorroutine() {
        for(;;){
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(FPSDisplayfrequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;
 
            // Display it
            FPS.SetTextWithRoundFloat(Mathf.RoundToInt(frameCount / timeSpan));
        }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        ms = PlayerPrefs.GetFloat("MS", 0);
        mv = PlayerPrefs.GetFloat("MV", 0);
        m_rigid = GetComponent<MyRigidbody>();
    }

    void Start()
    {
        StartCoroutine(FPSCorroutine());
        minZCameraPos = vcam.m_Lens.OrthographicSize;

        MS.SetTextWithRoundFloat(ms);
        MV.SetTextWithRoundFloat(mv);
    }

    void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, minZCameraPos + s * velocityZoomFactor, Time.deltaTime);
    }

    void FixedUpdate()
    {
        s = m_rigid.m_velocity.magnitude;
        v = transform.position.y;
        
        S.SetTextWithRoundFloat(s);
        V.SetTextWithRoundFloat(v);
        
        if (ms < s)
        {
            ms = s;
            MS.SetTextWithRoundFloat(ms);
            PlayerPrefs.SetFloat("MS", ms);
        }
        
        if (mv < v)
        {
            mv = v;
            MV.SetTextWithRoundFloat(mv);
            PlayerPrefs.SetFloat("MV", mv);
        }
        
        if ((Input.touchCount > 0 || Input.GetKey(KeyCode.Space)) && m_rigid.m_isGrounded)
        {
            m_rigid.AddForce(transform.right * m_thrust * Time.fixedDeltaTime);
            Debug.DrawLine(transform.position, transform.position + new Vector3((transform.right * m_thrust).x, (transform.right * m_thrust).y, 0f), Color.yellow);
        }
    }

    void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
