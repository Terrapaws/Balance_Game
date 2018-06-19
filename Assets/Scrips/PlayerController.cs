using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phidget22;
using Phidget22.Events;


public class PlayerController : MonoBehaviour {

    public float speed;

    private Rigidbody rb;

    //Balence Board code
    static VoltageRatioInput[] inputs = new VoltageRatioInput[4];
    static double[] defaultInputs;
    static bool initializedInputs;
    public int waitMillis;
    public static PlayerController instance;
    Vector3 move;
    public float moveMultiplier;
    // Balence Board code

    void OnEnable()
    {
        instance = this;
        if (!initializedInputs)
            StartCoroutine(InitInputs(waitMillis));
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        //rb.inertiaTensor = tensor;
    }
    IEnumerator InitInputs(int waitMillis)
    {
        defaultInputs = new double[inputs.Length];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = new VoltageRatioInput();
            inputs[i].Channel = i;
            inputs[i].Open(waitMillis);
            yield return new WaitForSecondsRealtime(waitMillis / 1000);
            defaultInputs[i] = inputs[i].VoltageRatio;
        }
        initializedInputs = true;
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Debug.Log("Quit!!");
            Application.Quit();
        }

    }

    float GetInput(int channel)
    {
        if (inputs[channel].Attached)
            return (float)(inputs[channel].VoltageRatio - defaultInputs[channel]);
        else
            throw new UnityException("The code is trying to get the voltage ratio of a non-connected sensor");
    }

    void OnApplicationQuit()
    {
        for (int i = 0; i < inputs.Length; i++)
            inputs[i].Close();
    }

    void FixedUpdate()
    {
        /*float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveUp = Input.GetAxis("Jump");
        Vector3 movement = new Vector3(moveHorizontal, moveUp, moveVertical);
        rb.AddForce(movement * speed);*/

        if (!initializedInputs)
			return;
		this.move = new Vector3(-1, -1, -1) * GetInput(2);
		this.move += new Vector3(-1, -1, 1) * GetInput(3);
		this.move += new Vector3(1, -2, -1) * GetInput(1);
		this.move += new Vector3(1, -1, 1) * GetInput(0);
		this.move = Vector3.ClampMagnitude(this.move * this.moveMultiplier, this.speed);
        this.rb.velocity = this.move;


    }
}
