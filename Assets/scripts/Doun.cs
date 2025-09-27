using UnityEngine;

public class Doun : MonoBehaviour
{

    [SerializeField] private GameObject truPlayer;

    private StateMachine StateMachine;
    private InputManager InputManager;

    void Start()
    {
        StateMachine = GetComponent<StateMachine>();
        InputManager = GetComponent<InputManager>();

        gameObject.SetActive(true);
    }

    void Update()
    {

    }

    public void DownMove()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            truPlayer.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
            truPlayer.SetActive(false);
        }
    }
}
