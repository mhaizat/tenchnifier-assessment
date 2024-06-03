using System;
using UnityEngine;

public class ManagerHub : MonoBehaviour
{
    private static ManagerHub _instance;
    public static ManagerHub Instance { get { return _instance; } }

    [SerializeField] private DataManager dataManager;

    public static event Action OnReshuffleCards;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);
        GetDataManager().LoadGameData();

    }

    public static void ReshuffleCards()
    {
        OnReshuffleCards?.Invoke();
    }
 
    private void OnApplicationQuit()
    {
        dataManager.SaveGameData();
    }

    public DataManager GetDataManager() { return dataManager; }
}
