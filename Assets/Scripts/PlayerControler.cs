using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerControler : MonoBehaviour
{
    public float thrust = 1f;
    private Rigidbody2D rigid;
    private BoxCollider2D box;
    public float raycastVDistance = 1f;

    [SerializeField]
    private float minZCameraPos = -15.0f;
    
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
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        ms = PlayerPrefs.GetFloat("MS", 0);
        mv = PlayerPrefs.GetFloat("MV", 0);
    }

    void Start()
    {
        StartCoroutine(FPSCorroutine());
        minZCameraPos = vcam.m_Lens.OrthographicSize;
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position - transform.up * (box.size.y / 2 + 0.001f), -transform.up, raycastVDistance);
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
        s = rigid.velocity.magnitude;
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
        
        if ((Input.touchCount > 0 || Input.GetKey(KeyCode.Space)) && IsGrounded())
        {
            Debug.DrawLine(transform.position - transform.up * (box.size.y / 2 + 0.001f), transform.position - transform.up * (box.size.y / 2 + 0.001f) + -transform.up * raycastVDistance, Color.green);
            rigid.AddForce(transform.right * thrust, ForceMode2D.Force);
        }
        else
        {
            Debug.DrawLine(transform.position - transform.up * (box.size.y / 2 + 0.001f), transform.position - transform.up * (box.size.y / 2 + 0.001f) + -transform.up * raycastVDistance, Color.red);
        }
    }

    void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
