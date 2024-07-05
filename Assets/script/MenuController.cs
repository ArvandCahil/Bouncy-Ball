using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject creditPanel;

    void Start()
    {
        mainMenuPanel.SetActive(true);
        settingPanel.SetActive(false);
        levelPanel.SetActive(false);
        creditPanel.SetActive(false);
    }

    public void showSetting()
    {
        settingPanel.SetActive(true);
    }

    public void hideSetting()
    {
        settingPanel.SetActive(false) ;
    }

    public void showMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingPanel.SetActive(false);
        levelPanel.SetActive(false);
        creditPanel.SetActive(false);
    }

    public void showLevel()
    {
        levelPanel.SetActive(true);
        creditPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        settingPanel.SetActive(false);
    }

    public void showCredit()
    {
        creditPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        settingPanel.SetActive(false);
        levelPanel.SetActive(false);
    }
}
