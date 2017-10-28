using Boo.Lang;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region exposed fields
    public string[] _inputUseItem;
    [SerializeField]
    private float _movementSpeed = 1.0f;
    [SerializeField]
    private string _inputAxisX = "Horizontal";
    [SerializeField]
    private string _inputAxisY = "Vertical";
	public GameObject _prefabCandy;
	private Vector3 prevInputVector;
    #endregion

    private Rigidbody2D _rigidbody;
    private PlayerInventory _inventory;


    // Use this for initialization
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _inventory = GetComponent<PlayerInventory>();
		prevInputVector = new Vector3 (1.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdatePosition();
        CheckInputs();
    }

    private void CheckInputs()
    {
		//TODO: implement button - item bindings
		for(var i = 0; i < 4; i++)
		{
			var inputName = _inputUseItem[i];
			if (Input.GetButtonDown(inputName))
			{
				if (i == 1) {
					var pos = new Vector3 (
						Mathf.Floor(transform.position.x) + 0.5f,
						Mathf.Floor(transform.position.y) + 0.5f, -7.0f);
					var candy = Instantiate (_prefabCandy, pos, Quaternion.identity);
					print (prevInputVector);
					candy.GetComponent<Rigidbody2D> ().AddForce (1000.0f * prevInputVector);
				}
			}
		}
    }

    private void UpdatePosition()
    {
        var inputVector = new Vector3(Input.GetAxis(_inputAxisX), Input.GetAxis(_inputAxisY));
        if (inputVector.sqrMagnitude >= 1.0f) {
            inputVector = inputVector.normalized;
        }
		if (inputVector.sqrMagnitude >= 0.0001) {
			prevInputVector = inputVector.normalized;
		}

        var speedVector = inputVector * _movementSpeed;
        _rigidbody.velocity = speedVector;
    }

    public void PickUpItem(Collectable collectable)
    {
        _inventory.Add(collectable);
	}
}