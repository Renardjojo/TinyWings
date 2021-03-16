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
    [SerializeField] private TextFloatLink H;
    [SerializeField] private TextFloatLink MH;
    [SerializeField] private TextFloatLink V;
    [SerializeField] private TextFloatLink MV;

    private float h = 0, mh = 0, v = 0, mv = 0;

    [SerializeField]
    private CinemachineVirtualCamera vcam;
    
    public float velocityZoomFactor = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        mh = PlayerPrefs.GetFloat("MH", 0);
        mv = PlayerPrefs.GetFloat("MH", 0);
    }

    void Start()
    {
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
        vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, minZCameraPos + rigid.velocity.magnitude * velocityZoomFactor, Time.deltaTime);
    }

    void FixedUpdate()
    {
        h = transform.position.x;
        v = transform.position.y;

        if (mh < h)
        {
            mh = h;
            MH.SetTextWithRoundFloat(mh);
            PlayerPrefs.SetFloat("MH", mh);
        }
        
        if (mv < v)
        {
            mv = v;
            MV.SetTextWithRoundFloat(mv);
            PlayerPrefs.SetFloat("MV", mv);
        }
        
        H.SetTextWithRoundFloat(h);
        V.SetTextWithRoundFloat(v);

        if (Input.GetKey(KeyCode.Space) && IsGrounded())
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
