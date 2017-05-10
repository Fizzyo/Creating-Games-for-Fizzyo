using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //public
    public float maxHeight = 5f;
    public bool smoothMovement = true;
    public float maxFizzyoPressure = 0.5f; //calibrated on start screen
    public GameObject missilePrefab;
    public Text debugTextPressure;

    //private
    float destXSpeed = 0.02f;
    private float smoothing = 0.03f;
    private float xSpeed = 0f;

    // Use this for initialization
    void Start()
    {

        //load our sensor calibration if already set
        if (PlayerPrefs.HasKey("Max Fizzyo Pressure"))
        {
            maxFizzyoPressure = PlayerPrefs.GetFloat("Max Fizzyo Pressure");
            Debug.Log("Set max fizzyo pressure val to: " + maxFizzyoPressure);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //get the pressure value as a joystick axis 0-1 blow out -1-0 breath in
        //float fizzyoVal = Input.GetAxisRaw("Horizontal");

        //get the pressure value from our fizzyo device class:
        //  *  Uses logged pressure data if useRecordedData in the editor instance is set to true.
        //  *  Direct from sensor if useRecordedData in the editor instance is false
        float pressure = Fizzyo.FizzyoDevice.Instance().Pressure();

        //Display debug info
        if (debugTextPressure)
        {
            debugTextPressure.text = string.Format("{0:0}", pressure * 100);
        }

        float destHeight = maxHeight * Mathf.Min((pressure / maxFizzyoPressure), 1);

        float y;

        if (smoothMovement)
        {
            y = transform.position.y + ((destHeight - transform.position.y) * smoothing);
        }
        else
        {
            y = destHeight;
        }

        float x = transform.position.x;
        if (y > 0.15f)
        {
            xSpeed += (destXSpeed - xSpeed) * smoothing;
            x += xSpeed;
        }
        else
        {
            xSpeed = 0;
        }
        transform.position = new Vector3(x, y, transform.position.z);

        if (Fizzyo.FizzyoDevice.Instance().ButtonDown() || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            var pos = transform.position;
            pos.y += 0.5f;
            Instantiate(missilePrefab, pos, transform.rotation);
        }
    }

}
