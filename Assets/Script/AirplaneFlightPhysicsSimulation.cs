using UnityEngine;

using UnityEngine.InputSystem;

public class AirplaneFlightPhysicsSimulation : MonoBehaviour

{

    public float thrust = 50f;

    public float liftCoefficient = 0.1f;

    public float dragCoefficient = 0.02f;

    public float pitchPower = 10f;

    public float rollPower = 10f;

    public float yawPower = 5f;

    public float stallAngle = 30f;

    public float stallLiftMultiplier = 0.3f;

    Rigidbody rb;

    bool engineOn = false;

    void Start()

    {

        rb = GetComponent<Rigidbody>();

        rb.centerOfMass = new Vector3(0, -0.6f, -0.2f);

    }

    void FixedUpdate()

    {

        var kb = Keyboard.current;

        if (kb == null) return;

        // THRUST

        if (kb.spaceKey.isPressed)

        {

            engineOn = true;

            rb.AddRelativeForce(Vector3.forward * thrust, ForceMode.Acceleration);

        }

        // SPEED

        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        // LIFT

        if (engineOn && forwardSpeed > 5f)

        {

            float lift = forwardSpeed * forwardSpeed * liftCoefficient;

            float pitchAngle = Vector3.Angle(

                transform.forward,

                Vector3.ProjectOnPlane(transform.forward, Vector3.up)

            );

            if (pitchAngle > stallAngle)

            {

                lift *= stallLiftMultiplier;

            }

            rb.AddForce(transform.up * lift, ForceMode.Acceleration);

        }

        // DRAG

        Vector3 drag = -rb.linearVelocity * dragCoefficient;

        rb.AddForce(drag);

        // SIDE DRAG

        Vector3 sideVel = Vector3.Dot(rb.linearVelocity, transform.right) * transform.right;

        rb.AddForce(-sideVel * dragCoefficient);

        // CONTROL

        float pitch = 0;

        float roll = 0;

        float yaw = 0;

        if (kb.wKey.isPressed) pitch = 1;

        if (kb.sKey.isPressed) pitch = -1;

        if (kb.aKey.isPressed) roll = 1;

        if (kb.dKey.isPressed) roll = -1;

        if (kb.qKey.isPressed) yaw = -1;

        if (kb.eKey.isPressed) yaw = 1;

        rb.AddRelativeTorque(new Vector3(

            pitch * pitchPower,

            yaw * yawPower,

            -roll * rollPower

        ));

    }

}
