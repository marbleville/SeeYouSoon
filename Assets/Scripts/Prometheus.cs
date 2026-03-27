using UnityEngine;

public class Prometheus : ADrivable
{
    public bool IsDriven { get; private set; } = false;
    public GameObject drivingText;
    public GameObject carSounds;

    private Vector3 playerOffset = new Vector3(0, 3, -5);
    private PrometeoCarController carController;
    private Rigidbody carRb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
        carController = GetComponent<PrometeoCarController>();
        carRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        carController.enabled = IsDriven;
        carRb.isKinematic = !IsDriven;
        carSounds.SetActive(IsDriven);
    }

    public override void OnDrive()
    {
        // move the camera into a good spot 
        // disbale player control && hide player 
        if (IsDriven)
        {
            EnablePlayer();
            IsDriven = !IsDriven;
            interactDinstance = 2;
        }
        else
        {
            DisablePlayer();
            IsDriven = !IsDriven;
            interactDinstance = 10;
        }
    }

    private void DisablePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (!player)
        {
            Debug.Log("No player found.");
            return;
        }

        if (drivingText)
        {
            drivingText.SetActive(true);
        }

        player.transform.SetParent(gameObject.transform);

        CharacterController controller = player.GetComponent<CharacterController>();
        CapsuleCollider collider = player.GetComponent<CapsuleCollider>();

        controller.enabled = false;
        collider.enabled = false;

        player.transform.localPosition = playerOffset;
        player.transform.rotation = gameObject.transform.rotation;
    }

    private void EnablePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (!player)
        {
            Debug.Log("No player found.");
            return;
        }

        if (drivingText)
        {
            drivingText.SetActive(false);
        }


        player.transform.localPosition = new Vector3(-2, 1, 0);

        player.transform.SetParent(null);

        CharacterController controller = player.GetComponent<CharacterController>();
        CapsuleCollider collider = player.GetComponent<CapsuleCollider>();

        controller.enabled = true;
        collider.enabled = true;
    }
}
