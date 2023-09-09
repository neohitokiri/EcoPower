using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    [SerializeField] public float velocidadMovimiento = 5.0f;
    [SerializeField] public Vector2 posicionInicial;
    [SerializeField] public float fuerzaSalto = 7.0f;
    [SerializeField] public bool enElSuelo = false;
    [SerializeField] public bool enElAgua = false;

//    [SerializeField] private int muertesAcumuladas = 0;
//    [SerializeField] public int puntosAcumulados = 0;
//    [SerializeField] public int puntosDesplegados = 0;

    [SerializeField] public float intensidadMaximaDeLaLuz;
    [SerializeField] public float intensidadMinimaDeLaLuz;

    [SerializeField] private ParticleSystem particulas;
    [SerializeField] private ParticleSystem particulasMuerte;

    private Rigidbody cuerpoRigido;
    private Animator animaciones;

    private AudioSource sonidoSalto;
    private AudioSource sonidoMuerte;

    private Vector2 moveInput;

    [SerializeField] public UnityEngine.Rendering.Universal.Light2D luzGlobal;
    [SerializeField] public GameObject menuPausa;

    private void Start() {
        foreach(Transform child in transform)
        {
            switch(child.gameObject.name)
            {
                case "SonidoSalto":
                    sonidoSalto = child.gameObject.GetComponent<AudioSource>(); break;
                case "SonidoMuerte":
                    sonidoMuerte = child.gameObject.GetComponent<AudioSource>(); break;
                case "Particulas":
                    particulas = child.gameObject.GetComponent<ParticleSystem>(); break;
                case "ParticulasMuerte":
                particulasMuerte = child.gameObject.GetComponent<ParticleSystem>(); break;
            }
        }

        intensidadMaximaDeLaLuz = 2.0f;
        intensidadMinimaDeLaLuz = 0.2f;
        cuerpoRigido = GetComponent<Rigidbody>();
        animaciones = GetComponent<Animator>();
        posicionInicial = transform.position;

        var totalColeccionables = GameObject.FindGameObjectsWithTag("Coleccionable");
        ControladorGlobal.Instance.SetTotalColeccionables(totalColeccionables != null ? totalColeccionables.Length : 0);
    }

    void Awake()
    {
    }

    void Update()
    {
//        float direccionPorTouch = 0;

        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

//        cuerpoRigido.velocity = new Vector3(-movimientoHorizontal * velocidadMovimiento, cuerpoRigido.velocity.y, 0.0f);

        moveInput.x = movimientoHorizontal;
        moveInput.y = movimientoVertical;
        moveInput.Normalize();

        cuerpoRigido.velocity = new Vector3(moveInput.x * velocidadMovimiento, cuerpoRigido.velocity.y, moveInput.y * velocidadMovimiento);

        if ((Input.GetButtonDown("Jump")))
        {
            enElSuelo = false;
            cuerpoRigido.AddForce(new Vector3(0f, fuerzaSalto, 0f), ForceMode.Impulse);
            sonidoSalto.Play();
            particulas.Play();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 0;
            menuPausa.SetActive(true);
        }

        if (movimientoHorizontal > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (movimientoHorizontal < 0)
            transform.localScale = new Vector3(1, 1, 1);

        animaciones.SetInteger("Salto", (int) cuerpoRigido.velocity.y);
        animaciones.SetBool("Piso", enElSuelo);

        Debug.Log("Movimiento Horizontal: " + movimientoHorizontal.ToString() + "   -    Movimiento Vertical: " + movimientoVertical.ToString());
        if(enElSuelo)
            animaciones.SetFloat("MovimientoHorizontal", Mathf.Abs(movimientoHorizontal != 0 || movimientoVertical != 0 ? 1 : 0));

    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Suelo"))
            enElSuelo = true;        
    }
    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
            enElSuelo = true;

        if (collision.gameObject.CompareTag("PlataformaMovil"))
        {
            enElSuelo = true;
            transform.parent = collision.transform;
        }

        if (collision.gameObject.CompareTag("Morir"))
            Morir();
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("PlataformaMovil"))
        {
            transform.parent = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        enElAgua = other.gameObject.CompareTag("Agua");
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Agua"))
        {
            enElAgua = false;
        };
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Agua"))
        {
            enElAgua = true;
        };
    }

    void Morir()
    {
/*
        sonidoMuerte.Play();
        transform.position = posicionInicial;
        cuerpoRigido.velocity = Vector2.zero;
        cuerpoRigido.angularVelocity = 0;
        cuerpoRigido.bodyType = RigidbodyType2D.Static;
        // Partículas para indicar muerte
        particulasMuerte.transform.position = transform.position;
        particulasMuerte.Play();

        cuerpoRigido.bodyType = RigidbodyType2D.Dynamic;
        ControladorGlobal.Instance.IncrementarMuertes();
        Debug.Log(ControladorGlobal.Instance.muertesAcumuladas);
*/
    }

//    public void Iluminar()
//    {
//        puntosAcumulados++;
//        float intensidadPorLuz = (intensidadMaximaDeLaLuz - intensidadMinimaDeLaLuz) / puntosDesplegados;
//        luzGlobal.intensity = 0.2f + (intensidadPorLuz * puntosAcumulados);

////        luzGlobal.intensity = 1 + (luz * porcentajeLuz / 100);
//    }

}
