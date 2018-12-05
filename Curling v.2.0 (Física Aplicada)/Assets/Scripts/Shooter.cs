using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Shooter : MonoBehaviour{
    private Arrow m_Arrow;

    [Header("Deslizamento")]
    public bool liberado = false;
    public Collider gelo;
    public float tempo;
    public float diminuir;
    public float staticInicial;

    [Header("Physics")]
    public float m_MaxForce;

    [Header("Force UI")]
    public float m_TimeToMaxForce;
    public Slider m_ForceSlider;
    public Image m_ForceFillImage;
    public Color m_MinForceColor;
    public Color m_MaxForceColor;
    public AnimationCurve m_Curve;

    [Header("HUD")]
    public TextMeshProUGUI bluePointsText;
    public TextMeshProUGUI redPointsText;
    public TextMeshProUGUI blueWin;
    public TextMeshProUGUI redWin;

    [Header("Outros")]
    public CameraFollow camera;
    public GameObject player1;
    public GameObject player2;
    public pointsCollider alvo;
    public triggerEnter trigger;

    private bool m_Shoot;
    private float m_ForceDirection = 1;
    private bool click = false;

    int player = 0;
    int jogadas = 0;
    int bluePoints = 0;
    int redPoints = 0;
    GameObject objeto;
    GameObject pinguim;

    public enum ShooterState{
        None, Charging, Release, Moving
    }
    private ShooterState m_CurrentState = ShooterState.None;

    void Start(){
        spawn();
        m_Arrow = GetComponent<Arrow>();
        blueWin.gameObject.SetActive(false);
        redWin.gameObject.SetActive(false);
    }

    private void Awake(){
        staticInicial = gelo.material.dynamicFriction;
    }

    private void Update(){
        float horizontal = Input.GetAxis("Horizontal");

        RotateArrow(horizontal, 0);

        if (click == false){
            Shoot();
        }
    }

    private void RotateArrow(float horizontal, float vertical){
        if (m_CurrentState == ShooterState.None){
            objeto.transform.Rotate(vertical, horizontal, 0);
            transform.Rotate(vertical, horizontal, 0);
            m_Arrow.SetPosition(0, transform.position);
            m_Arrow.SetPosition(1, transform.position + transform.forward * (m_ForceSlider.value + 2));
        }
    }

    void spawn(){
        if (player == 0){
            objeto = Instantiate(player1, new Vector3(5.8f , 1.0f, 0.0f), Quaternion.identity) as GameObject;
            objeto.name = "Pinguim1";
            camera.target = objeto.transform;
            player = 1;
        }else{
            RotateArrow(0, 0);
            objeto = Instantiate(player2, new Vector3(5.8f, 1.0f, 0.0f), Quaternion.identity) as GameObject;
            objeto.name = "Pinguim2";
            camera.target = objeto.transform;
            player = 0;
        }
    }

    private void Shoot(){
        if (m_CurrentState == ShooterState.None && Input.GetButtonDown("Fire1")){
            m_ForceSlider.value = 0.0f;
            m_ForceDirection = 1;
            m_CurrentState = ShooterState.Charging;
        }

        if (m_CurrentState == ShooterState.Charging && Input.GetButton("Fire1")){
            m_ForceSlider.value += (Time.deltaTime / m_TimeToMaxForce) * m_ForceDirection;
            m_ForceFillImage.color = Color.Lerp(m_MinForceColor, m_MaxForceColor, m_Curve.Evaluate(m_ForceSlider.value));
            if (m_ForceSlider.value >= 1.0f || m_ForceSlider.value <= 0.0f){
                m_ForceDirection *= -1;
            }
        }

        if (m_CurrentState == ShooterState.Charging && Input.GetButtonUp("Fire1")){
            float force = m_ForceSlider.value * m_MaxForce;
            objeto.GetComponent<Rigidbody>().AddForce(transform.forward * force);
            m_CurrentState = ShooterState.Moving;
            StartCoroutine(CheckStop());
        }

        if (m_CurrentState == ShooterState.Moving && Input.GetKeyDown("space") && gelo.material.dynamicFriction > 0){
            tempo = 0;
            gelo.material.dynamicFriction -= diminuir;
            liberado = true;
            if (gelo.material.dynamicFriction <= 0.0f){
                gelo.material.dynamicFriction = 0.1f;
            }
        }

        if(liberado == true){
            tempo += Time.deltaTime;
        }

        if (m_CurrentState == ShooterState.Moving && tempo >= 0.2f){
            gelo.material.dynamicFriction = staticInicial;
        }
    }

    private IEnumerator CheckStop(){
        m_Arrow.SetPosition(0, transform.position);
        m_Arrow.SetPosition(1, transform.position);
        yield return new WaitForSeconds(1.0f);

        while (objeto.GetComponent<Rigidbody>().velocity.magnitude > 0.0001f){
            yield return new WaitForEndOfFrame();
        }

        objeto.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_CurrentState = ShooterState.None;
        spawn();
        jogadas++;

        if (jogadas == 6 && GameObject.Find("Alvo").GetComponent<triggerEnter>().entrou == true){
            pinguim = alvo.FindClosestPenguim();
            if (pinguim.name == "Pinguim1"){
                bluePoints++;
                bluePointsText.text = bluePoints.ToString();
            }else if (pinguim.name == "Pinguim2"){
                redPoints++;
                redPointsText.text = redPoints.ToString();
            }
            alvo.destroyAll();
            StartCoroutine(CheckWin());
            jogadas = 0;
        }else if (jogadas == 6 && GameObject.Find("Alvo").GetComponent<triggerEnter>().entrou == false){
            print("Nenhum pinguim atingiu o alvo.");
            jogadas = 0;
            alvo.destroyAll();
        }
    }

    private IEnumerator CheckWin(){
        if (bluePoints == 3){
            blueWin.gameObject.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene("MenuScene");
        }else if (redPoints==3){
            redWin.gameObject.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene("MenuScene");
        }
    }
}