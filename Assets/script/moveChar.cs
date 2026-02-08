using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class moveChar : MonoBehaviour
{
    public float moveSpeed = 6f;      // سرعة المشي
    public float runSpeed = 10f;      // سرعة الجري
    public float gravity = -9.81f;    // الجاذبية
    public float jumpHeight = 1.5f;   // ارتفاع القفزة
    CharacterController controller;
    private Animator anim;

    Vector3 velocity;
    bool isGrounded;
    public AudioSource jumpAudio;

    Transform body;
    public Transform camera;
    float angle;
    float mouseX;
    float mouseY;
    public float speedRotation;

    public ParticleSystem particle1;
    public ParticleSystem particle2;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        body = GetComponent<Transform>();

    }

    void Update()
    {
        // التحقق إن اللاعب على الأرض
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // يثبت اللاعب على الأرض

        // إدخال اللاعب: W,S,A,D
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");


        // اتجاه الحركة
        Vector3 move = transform.right * h + transform.forward * v;

        // سرعة المشي والجري (Shift)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        float speed = Mathf.Clamp01(move.magnitude);
        bool jumping = Input.GetButtonDown("Jump");

        anim.SetFloat("Speed", speed);
        anim.SetBool("isjumping",jumping);

        // القفز
        if (jumping && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpAudio.Play();
        }

        // الجاذبية
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        CameraRotation();

        if (!particle1.isPlaying)
        {
            particle1.Play();
            particle2.Play();
        }
      

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "win")
           SceneManager.LoadScene("win");

        else if(other.tag == "lose")
          
           SceneManager.LoadScene("lose");
           
    }

    private void CameraRotation()
    {
        body.Rotate(0,mouseX*speedRotation,0);
        angle -= mouseY * speedRotation;
        angle = Mathf.Clamp(angle,-90,90);
        camera.localRotation = Quaternion.Euler(angle,0,0);
    }
}