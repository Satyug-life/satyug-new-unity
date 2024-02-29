using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngineInternal;

public class StatueManager : MonoBehaviour
{
    public static StatueManager StatueManagerInstance;

    private Camera _Camera;
    [HideInInspector] public string selectedPlacedID;

    public UnityEvent onSuccessfullyPlaced;
    public UnityEvent onFailurefullyPlaced;
    public UnityEvent onCompleteEvent;
    public bool isGameComplete = false;

    
    [SerializeField] UnityEvent onSetGlowOffEvent;
    [SerializeField] UnityEvent onSetGlowOnEvent;
    [SerializeField] UnityEvent onAttemptCompleteEvent;

    public StatueHandler statueHandler;
    [SerializeField] CinemachineBrain cinemachineBrain;

    public string collectionID;
    public string userID;

    [Header("Attempts")]
    private int totalAttempt = 2;
    public int currentAttempt = 0;

    [HideInInspector] public bool isGameStart;

    private void Awake()
    {
        if (StatueManagerInstance == null) StatueManagerInstance = this;
        isGameStart = true;
        _Camera = Camera.main;
    }

    public void ChangeBlendTime()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 2f;
    }

    private Accessories selectedAccessories { get; set; }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo))
            {
                Interactible interactible = hitInfo.transform.GetComponent<Interactible>();
                if(interactible != null)
                {
                    interactible.Interaction();
                }
            }
        }
    }

    #region GetAndSetSelectedAccessoryName

    private WaitForSeconds m_WaitingTime = new WaitForSeconds(6f);
    private IEnumerator m_Coroutine;
    internal Accessories GetSelectedAccessoryName()
    {
        return selectedAccessories;
    }

    public void SetAccesory(int accessories)
    {
        for (int i = 0; i < arrows.Length; i++) { arrows[i].SetActive(false); }

        isGameComplete = false;
        currentAttempt = 0;
        selectedAccessories = (Accessories)accessories;

        if(m_Coroutine != null)
            StopCoroutine(m_Coroutine);

        onSetGlowOffEvent?.Invoke();

        m_Coroutine = enumerator();
        StartCoroutine(m_Coroutine);
    }

    private IEnumerator enumerator()
    {
        yield return m_WaitingTime;

        if (StatueManager.StatueManagerInstance.isGameComplete) { yield break; }

        onSetGlowOnEvent?.Invoke();
    }
    #endregion

    #region Highlight
    [SerializeField] GameObject[] arrows;
    public void SetArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].SetActive(false);
            if (arrows[i].name == GetSelectedAccessoryName().ToString())
            {
                arrows[i].SetActive(true);
            }
        }
    }

    public void DisableArrow()
    {
        for(int i = 0;i < arrows.Length; i++)
        {
            arrows[i].SetActive(false);
        }
    }
    #endregion

    #region Attempts

    public void CheckAttempts()
    {
        currentAttempt++;

        if (currentAttempt > totalAttempt)
        {
            onAttemptCompleteEvent?.Invoke();
            currentAttempt = 0;
        }
    }

    #endregion

    public void IsGameStart()
    {
        isGameStart = false;
    }
}


public enum Accessories
{
    ArrowHolder,
    RudrakshArmBangle,
    RudrakshMala,
    RudrakshKundel,
    PhoolonKiMala
}