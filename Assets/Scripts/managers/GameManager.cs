using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;


    public static GameManager Instance { get; private set; }

    [Header("win or lose panels")]
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject winLosePanel;
    [SerializeField] private GameObject winPartPanel; 
    [SerializeField] private GameObject losePartPanelNoReward;
    [SerializeField] private GameObject losePartPanelReward;
    [Header("win or lose buttons")]
    [SerializeField] private Button nextGameWinCondition;
    [SerializeField] private Button restartLoseConditionNoreward;
    [SerializeField] private Button restartLoseConditionReward;
    [SerializeField] private Button rewardForMoreSeconds;
    [Header("win or lose texts")]
    [SerializeField] private TextMeshProUGUI addSecondsInfoText;
    [SerializeField] private TextMeshProUGUI secondsAmountText;
    [SerializeField] private TextMeshProUGUI forRewardText;

    [SerializeField] private GameObject timerPanel;
    [SerializeField] private Button menu;
    [SerializeField] private Image backGround;
    [SerializeField] private Transform PanelsLocation;
    [SerializeField] private SpritesPack spritesPack;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject basicPanel;
    [SerializeField] private AudioManager _audio;

    [Header("Timer")]
    [SerializeField] private Image timerSliderImage;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("MENU")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button toMainMenu;
    [SerializeField] private Button continueGame;
    [SerializeField] private Button sound;
    [SerializeField] private Button restart;
    [SerializeField] private Sprite soundONSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private TextMeshProUGUI nextButtonText;

    [Header("TUTORIAL")]
    [SerializeField] private GameObject hint1;
    [SerializeField] private TextMeshProUGUI hint1Text;
    [SerializeField] private GameObject hint2;
    [SerializeField] private TextMeshProUGUI hint2Text;

    [SerializeField] private GameObject confeti;

    public AudioManager GetAudio { get => _audio; }

    //to del    
    [SerializeField] private Button win;
    [SerializeField] private Button lose;

    //translatings and texts
    [Header("panels translation")]
    [SerializeField] private TextMeshProUGUI winLoseMessages;

    private bool isRestaring;
    private bool isTouchActive;
    private bool isGameStarted;
    private bool isRewardedStarted;
    private bool isRewardedOK;
    private bool isRewardedUsed;
    private bool isLastOpenerWorked;
    private bool isActiveButtonWinContinue;
    private bool isActiveButtonLoseRestart;
    private bool isActiveButtonLoseReward;

    private bool isInterstitialStarted;

    private int pairAmount;
    private int overallPanels
    {
        get
        {
            return (int)(Globals.PanelsNumber.x * Globals.PanelsNumber.y) + Globals.AdditionalPanelsAmount;
        }
    }

    private int collectedPanels;
        
    private float currentTimer;
    private float _timer;
    private readonly float timerUpdateCooldown = 1f;

    private Ray ray;
    private RaycastHit hit;
        
    private List<Panel> panels = new List<Panel>();
    private List<Panel> groupsToCompare = new List<Panel>();

    private Action nextLevelAction;
    private Translation lang;

    private int OneByOnePanelOpened = 0;

    

    private void Awake()
    {
       
        //Screen.SetResolution(1200, 600, true);
        //SaveLoadManager.Load();
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        back.SetActive(true);


        if (Globals.IsMobilePlatform)
        {
            if (Globals.PanelsNumber.x <= 3)
            {
                mainCamera.fieldOfView = 50;
            }
            else if (Globals.PanelsNumber.x <= 4)
            {
                mainCamera.fieldOfView = 54;
            }
            else if (Globals.PanelsNumber.x <= 5)
            {
                mainCamera.fieldOfView = 57;
            }
            else
            {
                mainCamera.fieldOfView = 60;
            }
            
        }
        else
        {
            if (Globals.PanelsNumber.x <= 3)
            {
                mainCamera.fieldOfView = 50;
            }
            else if (Globals.PanelsNumber.x <= 4)
            {
                mainCamera.fieldOfView = 55;
            }
            else if (Globals.PanelsNumber.x <= 5)
            {
                mainCamera.fieldOfView = 57;
            }
            else if(Globals.PanelsNumber.x <= 7)
            {
                mainCamera.fieldOfView = 65;
            }
            else if (Globals.PanelsNumber.x <= 8)
            {
                mainCamera.fieldOfView = 65;
            }

            //if (Globals.PanelsNumber.x < 8) mainCamera.fieldOfView = 70;
        }

        
        lang = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();

        hint1.SetActive(false);
        hint1Text.text = lang.hint1;
        hint2.SetActive(false);
        hint2Text.text = lang.hint2;

        nextButtonText.text = lang.next;

        winLosePanel.SetActive(false);
        winPartPanel.SetActive(false);
        losePartPanelNoReward.SetActive(false);
        losePartPanelReward.SetActive(false);
        confeti.SetActive(false);

        addSecondsInfoText.text = lang.getMoreSecondsInfo;
        secondsAmountText.text = Globals.RewardedSeconds.ToString() + " " + lang.secondsAmountPart;
        forRewardText.text = lang.forRewarded;

        pairAmount = (int)Globals.CurrentPairGroupType;

        try
        {
            int count = Panel.CreatePanels((int)Globals.PanelsNumber.x, (int)Globals.PanelsNumber.y,
            basicPanel, PanelsLocation, ref panels, Globals.AdditionalPanelsAmount);

            Panel.ArrangePanels(spritesPack.GetRandomPack((int)(count / pairAmount), Globals.Difficulty), count, pairAmount, ref panels);
            backGround.sprite = spritesPack.GetRandomBackGround();
        }
        catch (Exception ex)
        {
            print(ex);

            if (!Globals.IsRepeteGame)
            {
                SceneManager.LoadScene("Gameplay");
            }
            else
            {
                Globals.GameDesignManager.SetLevelData(Globals.GameTypeRepete, Globals.GameLevelRepete);
            }
        }

        
        timerSliderImage.fillAmount = 1f;
        timerText.text = "";

        if (Globals.HowManyLost > 2)
        {
            Globals.HowManyLost = 0;
            currentTimer = Globals.StageDurationInSec * 1.2f;
        }
        else
        {
            currentTimer = Globals.StageDurationInSec;
        }
        
        
        timerPanel.SetActive(true);

        menuPanel.SetActive(false);

        StartCoroutine(playShowPanels());
        StartCoroutine(fadeScreenOff());

        //TO DEL
        win.onClick.AddListener(() =>
        {
            gameWin();
        });
        //TO DEL
        lose.onClick.AddListener(() =>
        {
            currentTimer = 0;
        });

        //MENU========================================================
        menu.onClick.AddListener(() =>
        {
            _audio.PlaySound_Click();
            menuPanel.SetActive(true);
            isGameStarted = false;
        });
        toMainMenu.onClick.AddListener(() =>
        {
            _audio.PlaySound_Click();
            SceneManager.LoadScene("MainMenu");
        });
        continueGame.onClick.AddListener(() =>
        {
            _audio.PlaySound_Click();
            isGameStarted = true;
            menuPanel.SetActive(false);
        });
        restart.onClick.AddListener(() =>
        {
            _audio.PlaySound_Click();
            restartCurrentGame();
        });
        sound.onClick.AddListener(() =>
        {
            if (Globals.IsSoundOn)
            {
                _audio.Mute();
                Globals.IsSoundOn = false;
                Globals.MainPlayerData.S = 0;
            }
            else
            {
                _audio.UnMute();
                _audio.PlaySound_Click();
                Globals.IsSoundOn = true;
                Globals.MainPlayerData.S = 1;
            }

            if (Globals.IsSoundOn)
            {
                sound.GetComponent<Image>().sprite = soundONSprite;
            }
            else
            {
                sound.GetComponent<Image>().sprite = soundOffSprite;
            }

            SaveLoadManager.Save();

        });
        if (Globals.IsSoundOn)
        {
            sound.GetComponent<Image>().sprite = soundONSprite;
        }
        else
        {
            sound.GetComponent<Image>().sprite = soundOffSprite;
        }
        
        //============================================================

        Resources.UnloadUnusedAssets();

        nextGameWinCondition.onClick.AddListener(() =>
        {
            if (isActiveButtonWinContinue) return;
            isActiveButtonWinContinue = true;
            _audio.PlaySound_Click();
            ShowInterstitial();
        });

        restartLoseConditionNoreward.onClick.AddListener(() =>
        {
            if (isActiveButtonLoseRestart) return;
            isActiveButtonLoseRestart = true;
            _audio.PlaySound_Click();
            restartCurrentGame();
        });

        restartLoseConditionReward.onClick.AddListener(() =>
        {
            if (isActiveButtonLoseReward) return;
            isActiveButtonLoseReward = true;
            _audio.PlaySound_Click();
            restartCurrentGame();
        });

        rewardForMoreSeconds.onClick.AddListener(() =>
        {
            _audio.PlaySound_Click();
            ShowRewarded();
        });

        if (Globals.IsSoundOn)
        {
            _audio.UnMute();
        }
        else
        {
            _audio.Mute();
        }
    }

    private void Start()
    {
        print(Globals.IsSoundOn);

        if (Globals.IsSoundOn)
        {
            _audio.UnMute();
        }
        else
        {
            _audio.Mute();
        }
        
        if (Globals.MainPlayerData.H1 == 0 && Globals.GameLevel == 0)
        {
            StartCoroutine(showHints());
        }

        print(Globals.GameLevel + " - current level");
    }


    private void Update()
    {
        if (isRewardedStarted)
        {
            if (isRewardedOK)
            {
                isRewardedStarted = false;
                isRewardedOK = false;
                isRewardedUsed = true;
                //givePrizeForReward();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //gameWin();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //gameLose();
        }

        if (!isGameStarted) return;

        if (currentTimer <= 0)
        {
            gameLose();
        }

        if (isGameStarted)
        {
            currentTimer -= Time.deltaTime;
            //timer update every 1 sec
            if (_timer > timerUpdateCooldown)
            {
                _timer = 0;
                if (currentTimer >= 0) updateTimer();
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }

       
        //check for finding
        if (groupsToCompare.Count == pairAmount)
        {
            
            bool isOK = true;
            int ID = -1;
            for (int i = 0; i < groupsToCompare.Count; i++)
            {                
                if (ID >= 0 && groupsToCompare[i].ID != ID)
                {
                    
                    isOK = false;
                    break;
                }
                else if (ID < 0)
                {
                    ID = groupsToCompare[i].ID;
                }
            }

            if (isOK)
            {
                for (int i = 0; i < groupsToCompare.Count; i++)
                {
                    groupsToCompare[i].MakeCompleted();
                    groupsToCompare[i].panelTransform.localEulerAngles = new Vector3(0f, 180f, 0f);
                    collectedPanels++;
                }

                groupsToCompare.Clear();
                OneByOnePanelOpened++;

                if (OneByOnePanelOpened > 1)
                {
                    //
                    //kjfbkerbfhebrfhvjevfejvfjev
                }
            }     
            else
            {
                //groupsToCompare.Clear();
                for (int i = 0; i < groupsToCompare.Count; i++)
                {
                    if ((groupsToCompare[i].IsFaceOn && groupsToCompare[i].IsClosing) || !groupsToCompare[i].IsFaceOn)
                    {
                        groupsToCompare.Remove(groupsToCompare[i]);
                    }
                }

                OneByOnePanelOpened = 0;
            }
        } 
        else if (groupsToCompare.Count > 0)
        {
            for (int i = 0; i < groupsToCompare.Count; i++)
            {
                if ((groupsToCompare[i].IsFaceOn && groupsToCompare[i].IsClosing) || !groupsToCompare[i].IsFaceOn)
                {
                    groupsToCompare.Remove(groupsToCompare[i]);
                    OneByOnePanelOpened = 0;
                }
            }
            
        }
     
        //WIN CONDITION
        if (overallPanels == collectedPanels && !isRestaring)
        {
            gameWin();
        }
        //PREWIN
        if ((overallPanels - collectedPanels) == pairAmount && !isLastOpenerWorked && Globals.PanelsNumber.x > 4 && pairAmount > 2)
        {
            openLastPanels();
        }



        if (Input.GetMouseButton(0) && isTouchActive)
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 20))
            {                
                if (hit.collider.TryGetComponent(out Panel takenPanel))
                {
                    if (groupsToCompare.Count < pairAmount && !takenPanel.IsFaceOn &&  !groupsToCompare.Contains(takenPanel))
                    {
                        if (takenPanel.TryShowFace())
                        {
                            groupsToCompare.Add(takenPanel);
                        }
                    }
                    
                }                
            }
        }

    }

    private void openLastPanels()
    {
        isLastOpenerWorked = true;
        StartCoroutine(playLastPanels());
    }
    private IEnumerator playLastPanels()
    {
        yield return new WaitForSeconds(0.2f);


        groupsToCompare.Clear();

        List<Panel> toShow = new List<Panel>();    

        for (int i = 0; i < panels.Count; i++)
        {
            if (!panels[i].IsCompleted)
            {
                //panels[i].TryShowFace();
                groupsToCompare.Add(panels[i]);
                toShow.Add(panels[i]);                
            }
        }

        for (int i = 0; i < toShow.Count; i++)
        {
            toShow[i].TryShowFace();
            yield return new WaitForSeconds(0.2f);
        }
    }


    private void gameWin()
    {
        if (Globals.HowManyLost>0) Globals.HowManyLost--;
                
        StartCoroutine(playGameWin());
    }
    private IEnumerator playGameWin()
    {
        isGameStarted = false;
        _audio.PlaySound_WinGame();

        yield return new WaitForSeconds(1f);
                
        confeti.SetActive(true);
        timerPanel.SetActive(false);
        //PanelsLocation.gameObject.SetActive(false);
        hidePanel();
        winLosePanel.SetActive(true);
        winPartPanel.SetActive(true);
        winLoseMessages.text = lang.winText;

        
        nextLevelAction = toNextLevel;
        if (!Globals.IsRepeteGame) SaveLoadManager.Save();
        isRestaring = true;

        Debug.LogWarning("GAME WIN");
    }


    private void hidePanel()
    {
        PanelsLocation.transform.DOMove(new Vector3(0, -10, 0), 1);
    }

    private void showPanel()
    {
        PanelsLocation.transform.DOMove(Vector3.zero, 1);
    }

    private void gameLose()
    {
        Globals.HowManyLost++;
        _audio.PlaySound_LoseGame();
        //PanelsLocation.gameObject.SetActive(false);
        hidePanel();
        winLoseMessages.text = lang.loseText;
        winLosePanel.SetActive(true);

        isGameStarted = false;
        
        isRestaring = true;
        Debug.LogWarning("GAME LOST");

        print(DateTime.Now.Subtract(Globals.TimeWhenLastRewardedWas).TotalSeconds + " till starting rewarded");

        if (1 == 2/*DateTime.Now.Subtract(Globals.TimeWhenLastRewardedWas).TotalSeconds > Globals.INTERVAL_FOR_REWARDED && !isRewardedUsed*/)
        {            
            losePartPanelReward.SetActive(true);
            losePartPanelNoReward.SetActive(false);
        }
        else
        {
            timerPanel.SetActive(false);
            losePartPanelReward.SetActive(false);
            losePartPanelNoReward.SetActive(true);
            nextLevelAction = restartCurrentGame;
        }
    }


    private void ShowRewarded()
    {
        StartCoroutine(playRewarded());
    }
    private IEnumerator playRewarded()
    {
        print("how much time till start rew: " + DateTime.Now.Subtract(Globals.TimeWhenLastRewardedWas).TotalSeconds);

        if (1 == 2 /*DateTime.Now.Subtract(Globals.TimeWhenLastRewardedWas).TotalSeconds > Globals.INTERVAL_FOR_REWARDED*/)
        {
            yield return new WaitForSeconds(0.5f);

            _audio.Mute();
            print("starting rewarded");
                       
            Globals.TimeWhenLastRewardedWas = DateTime.Now;
            Globals.TimeWhenLastInterstitialWas = DateTime.Now;
            Globals.RewardedAmount++;
            //YandexGame.OpenVideoEvent = rewardStarted;
            //YandexGame.RewardVideoEvent = rewardedClosedOK;
            //YandexGame.CloseVideoEvent = advRewardedClosed;//nextLevelAction;
            //YandexGame.ErrorVideoEvent = advRewardedError;//nextLevelAction;
            //YandexGame.RewVideoShow(155);

            //todel
            rewardedClosedOK(5);
            advRewardedClosed();
        }
        else
        {
            print("not a time for interstitital");
            nextLevelAction?.Invoke();
        }
    }



    private void ShowInterstitial()
    {
        StartCoroutine(playInterstitial());
    }
    private IEnumerator playInterstitial()
    {
        print("how much time till start: " + DateTime.Now.Subtract(Globals.TimeWhenLastInterstitialWas).TotalSeconds);

        if (1==2/*DateTime.Now.Subtract(Globals.TimeWhenLastInterstitialWas).TotalSeconds > Globals.INTERVAL_FOR_INTERSTITITAL*/)
        {
            yield return new WaitForSeconds(0.5f);

            _audio.Mute();
            isTouchActive = false;
            print("starting interstitital");

            GameObject TransitionScreen = Instantiate(Resources.Load<GameObject>("TransitionCanvas"));
            TransitionScreen.gameObject.name = "TransitionScreen";
            TransitionScreen.transform.GetChild(0).GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 1);

            Globals.TimeWhenLastInterstitialWas = DateTime.Now;
            Globals.InterstitialsAmount++;
            //YandexGame.OpenFullAdEvent = advStarted;
            //YandexGame.CloseFullAdEvent = advClosed;//nextLevelAction;
            //YandexGame.ErrorFullAdEvent = advError;//nextLevelAction;
            //YandexGame.FullscreenShow();

            
        }
        else
        {
            print("not a time for interstitital");
            errorText.text += "not a time for interstitital\n";
            nextLevelAction?.Invoke();
        }
    }

    public void HandleLeftApplication(object sender, EventArgs args)
    {
        print("HandleLeftApplication event received");
        errorText.text += "HandleLeftApplication event received\n";
    }

    public void HandleInterstitialShown(object sender, EventArgs args)
    {
        print("HandleInterstitialShown event received");
        errorText.text += "HandleInterstitialShown event received\n";
    }

    
    private void advStarted(object sender, EventArgs args)
    {
        print("adv was OK");
        errorText.text += "adv was OK\n";

        //Time.timeScale = 0;
    }

    private void rewardStarted()
    {
        losePartPanelReward.SetActive(false);
        losePartPanelNoReward.SetActive(true);


        print("reward was OK");
        isRewardedStarted = true;

        Time.timeScale = 0;
    }

    private void advClosed(object sender, EventArgs args)
    {
        print("adv was closed");
        Time.timeScale = 1;
        isTouchActive = true;
        nextLevelAction?.Invoke();
    }

    private void rewardedClosedOK(int value)
    {
        //155
        if (value == 5)
        {
            isRewardedOK = true;
        }
    }

    private void advRewardedClosed()
    {
        print("rewarded was OK-closed");
        Time.timeScale = 1;
        winLosePanel.SetActive(false);
        StartCoroutine(waitAndDo());
    }
    private IEnumerator waitAndDo()
    {
        yield return new WaitForSeconds(0.3f);

        if (isRewardedUsed)
        {
            givePrizeForReward();
        }
        else
        {
            print("rewarded was not watched");
            winLosePanel.SetActive(true);
        }
    }

    private void givePrizeForReward()
    {
        showPanel();
        currentTimer = Globals.RewardedSeconds;
        isGameStarted = true;
        isRestaring = false;
        winLosePanel.SetActive(false);
        losePartPanelReward.SetActive(false);
        losePartPanelNoReward.SetActive(false);
        timerPanel.SetActive(true);
        if (Globals.IsSoundOn)
        {
            _audio.UnMute();
        }
        else
        {
            _audio.Mute();
        }
        OneByOnePanelOpened = 0;
        groupsToCompare.Clear();
        _audio.PlaySound_Success();
    }


    

    private void restartCurrentGame()
    {        
        StartCoroutine(playrestartCurrentGame());
    }
    private IEnumerator playrestartCurrentGame()
    {
        //isRestaring = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(fadeScreenOn());
        yield return new WaitForSeconds(1);
        

        if (!Globals.IsRepeteGame)
        {
            SceneManager.LoadScene("Gameplay");
        }
        else
        {
            Globals.GameDesignManager.SetLevelData(Globals.GameTypeRepete, Globals.GameLevelRepete);
        }
    }

    private void toNextLevel()
    {        
        StartCoroutine(playNextLevel());
    }
    private IEnumerator playNextLevel()
    {
        //isRestaring = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(fadeScreenOn());
        yield return new WaitForSeconds(1);

        if (!Globals.IsRepeteGame)
        {
            Globals.GameDesignManager.SetLevelData(true);
        }
        else
        {
            Globals.GameDesignManager.SetLevelData(Globals.GameTypeRepete, Globals.GameLevelRepete);
        }
        
    }

    private void updateTimer()
    {
        timerSliderImage.fillAmount = currentTimer / Globals.StageDurationInSec;
        int minutes = (int)(currentTimer / 60f);
        int seconds = (int)(currentTimer - (minutes * 60));

        string minutesToShow = minutes.ToString();
        string secondsToShow = seconds.ToString();

        if (minutes <=0)
        {
            minutesToShow = "00";
        }
        else if (minutes < 10)
        {
            minutesToShow = "0" + minutes.ToString();
        }

        if (seconds < 10)
        {
            secondsToShow = "0" + seconds.ToString();
        }

        timerText.text = minutesToShow + ":" + secondsToShow;
    }

    

    private IEnumerator playShowPanels()
    {        
        float xmin = 1000;
        float xmax = -1000;
        float ymin = 1000;
        float ymax = -1000;

        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetVisibility(false);
            panels[i].transform.DOScale(Vector3.zero, 0);
            if (panels[i].transform.position.x < xmin)
            {
                xmin = panels[i].transform.position.x;
            }
            if (panels[i].transform.position.x > xmax)
            {
                xmax = panels[i].transform.position.x;
            }
            if (panels[i].transform.position.y < ymin)
            {
                ymin = panels[i].transform.position.y;
            }
            if (panels[i].transform.position.y > ymax)
            {
                ymax = panels[i].transform.position.y;
            }
        }

        yield return new WaitForSeconds(0.5f);

        _audio.PlaySound_CardShuffle();

        float x = (xmin + xmax) / 2f;
        float y = (ymin + ymax) / 2f;

        for (float j = 0; j < 4; j+=0.15f)
        {
            for (int i = 0; i < panels.Count; i++)
            {
                if (
                    panels[i].transform.position.x < (x + j) 
                    && panels[i].transform.position.x > (x - j)
                    && panels[i].transform.position.y < (y + j)
                    && panels[i].transform.position.y > (y - j)
                    )
                {
                    if (!panels[i].GetVisibility())
                    {                        
                        panels[i].SetVisibility(true);                        
                        panels[i].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine);
                    }
                    
                }
            }

            yield return new WaitForSeconds(0.02f);
        }

        _audio.StopAny();

        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetVisibility(true);
        }

        isTouchActive = true;
        isGameStarted = true;
    }

    private IEnumerator fadeScreenOff()
    {
        GameObject TransitionScreen = Instantiate(Resources.Load<GameObject>("TransitionCanvas"));
        TransitionScreen.gameObject.name = "TransitionScreen";
        TransitionScreen.transform.GetChild(0).GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 0);
        TransitionScreen.transform.GetChild(0).GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 1);
        TransitionScreen.SetActive(true);
        yield return new WaitForSeconds(1);

        TransitionScreen.SetActive(false);
    }

    private IEnumerator fadeScreenOn()
    {
        GameObject TransitionScreen = Instantiate(Resources.Load<GameObject>("TransitionCanvas"));
        TransitionScreen.gameObject.name = "TransitionScreen";
        TransitionScreen.transform.GetChild(0).GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0);
        TransitionScreen.transform.GetChild(0).GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 1);
        TransitionScreen.SetActive(true);
        yield return new WaitForSeconds(1);                
    }

    private IEnumerator showHints()
    {        
        yield return new WaitForSeconds(2);
        Globals.MainPlayerData.H1 = 1;
        Globals.MainPlayerData.H1 = 2;
        SaveLoadManager.Save();


        hint1.transform.DOScale(Vector3.zero, 0);
        hint1.SetActive(true);
        _audio.PlaySound_Success();
        hint1.transform.DOScale(Vector3.one, 0.3f);




        for (float i = 0; i < 5; i += 0.1f)
        {
            if (winLosePanel.activeSelf)
            {
                hint1.SetActive(false);
                hint2.SetActive(false);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        hint1.transform.DOScale(Vector3.zero, 0.3f);

        yield return new WaitForSeconds(1);
        hint1.SetActive(false);

        hint2.transform.DOScale(Vector3.zero, 0);
        hint2.SetActive(true);
        hint2.transform.DOScale(Vector3.one, 0.3f);
        _audio.PlaySound_Success();

        for (float i = 0; i < 5; i += 0.1f)
        {
            if (winLosePanel.activeSelf)
            {
                hint1.SetActive(false);
                hint2.SetActive(false);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        hint2.transform.DOScale(Vector3.zero, 0.3f);
        yield return new WaitForSeconds(1);
        hint2.SetActive(false);
                
    }
}
