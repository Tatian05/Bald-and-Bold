using UnityEngine;
public class PlayerShop : MonoBehaviour
{
    [SerializeField] float _speedMovement;
    [SerializeField] GameObject _shopCanvas, _collectionCanvas;

    Animator _animator;

    System.Action _currentState;
    InputManager _inputManager;
    bool _onShopTrigger, _onProbadorTrigger;
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _inputManager = InputManager.Instance;
        _currentState = DefaultState;
    }
    void Update()
    {
        _currentState();
        if (Input.GetKeyDown(KeyCode.F3)) Helpers.PersistantData.persistantDataSaved.coins += 200;
    }
    void DefaultState()
    {
        Vector3 inputs = new Vector3(_inputManager.GetAxisRaw("Horizontal"), 0, 0);

        transform.position += inputs * _speedMovement * Time.deltaTime;

        PlayAnimation(inputs.x);

        if (inputs.magnitude != 0)
            transform.eulerAngles = inputs.x < 0 ? new Vector3(0, 180, 0) : Vector3.zero;

        if (_inputManager.GetButtonDown("Interact") && _onShopTrigger)
        {
            _shopCanvas.SetActive(true);
            _currentState = delegate
            {
                PlayAnimation(inputs.x);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            };
        }

        if (_inputManager.GetButtonDown("Interact") && _onProbadorTrigger)
        {
            _collectionCanvas.SetActive(true);
            _currentState = delegate
            {
                PlayAnimation(inputs.x);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            };
        }
    }
    void PlayAnimation(float xAxis) => _animator.SetInteger("xAxis", (int)Mathf.Abs(xAxis));

    public void SetDefaultState()
    {
        _currentState = DefaultState;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<NextSceneOnTrigger>()) _currentState = delegate { PlayAnimation(0); };
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>() && collision.transform.parent.name == "Seller") _onShopTrigger = true;

        if (collision.GetComponent<ShowKeyUI>() && collision.transform.parent.name == "Probador") _onProbadorTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>() && collision.transform.parent.name == "Seller") _onShopTrigger = false;

        if (collision.GetComponent<ShowKeyUI>() && collision.transform.parent.name == "Probador") _onProbadorTrigger = false;
    }
    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
