using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerControl : MonoBehaviour
{
    //public int playerID = 1;
    public int controllerID = 1;
    public bool _lockedIn = false;

    [SerializeField] private Text _txtInfo;
    [SerializeField] private GameObject _joinButton;
    [SerializeField] private Material[] _buttonMaterial;
    [SerializeField] private Material _playerMaterial;
    private bool _notPressed;
    [SerializeField] private float _buttonOffset;
    //ui stuff
    [SerializeField] private UiPlayer _playerUi;
    [SerializeField] private PlayerUiController _uiController;
    [SerializeField] private int _playerId;
    private Color _playerColor;

    private void Start()
    {
        _playerColor = _playerMaterial.color;
        _playerUi.NewPlayerColor(_playerColor);
    }

    private void Update()
    {
        if(this._lockedIn == true && SceneManager.GetActiveScene().name == "CharacterSelection")
        {
            RandomizeColor();
            _joinButton.transform.position = new Vector3(transform.position.x, _joinButton.transform.position.y, transform.position.z + _buttonOffset);
        }
    }
    public void Active(int ctrl)
    {
        this.enabled = true;
        _lockedIn = true;
        controllerID = ctrl;
        _joinButton.GetComponent<Renderer>().material = _buttonMaterial[1];
        this.gameObject.AddComponent<DontDestroyOnLoad>();
        _uiController.SetPlayerActive(_playerId);

    }

    public void DeActive()
    {
        _uiController.SetPlayerInactive(_playerId);
        Destroy(this.gameObject.GetComponent<DontDestroyOnLoad>());
        this.enabled = false;
        _lockedIn = false;
        _joinButton.GetComponent<Renderer>().material = _buttonMaterial[0];

    }

    public void RandomizeColor()
    {
        if (_notPressed && Mathf.Abs(Input.GetAxis("DPadHorizontalP" + controllerID)) > 0.1f)
        {
            _playerColor = new Color(Random.Range(0.1f, 1), Random.Range(0.1f, 1), Random.Range(0.1f, 1));
            _playerMaterial.SetColor("_Color", _playerColor);
            _playerUi.NewPlayerColor(_playerColor);
            _notPressed = false;
        }
        else if(Mathf.Abs(Input.GetAxis("DPadHorizontalP" + controllerID)) < 0.1f)
            _notPressed = true;
    }
}
