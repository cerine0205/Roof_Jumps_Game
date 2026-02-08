using UnityEngine;

public class move : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Animator anim;
    private CharacterController controller;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);

        controller.Move(move * moveSpeed * Time.deltaTime);

        // قيمة السرعة الصحيحة بين 0 و 1
        float speed = Mathf.Clamp01(move.magnitude);

        anim.SetFloat("Speed", speed);

        if (speed > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(move);
        }
    }
}