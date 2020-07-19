using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUI : MenuUI
{
    public bool isTitleScreenVersion;
    public SavePanelMode mode;
    public List<SavePanelUI> savePanels;
    public ReisenSavePoint currentSavePoint;

    public int selectedSaveIndex;

    //redundant with menuUI but we bear with it
    public new void Update()
    {
        if (!InFocus())
        {
            if (gainFocus)
            {
                Init();
                inFocus = true;
            }
            else
            {
                foreach (SavePanelUI menuElement in savePanels)
                    menuElement.Blur();
            }

            return;
        }

        if (!gainFocus)
        {
            inFocus = false;
            return;
        }

        InputDirection dir = Controls.getInputDirectionDown();
        if (dir == InputDirection.N)
        {
            selectedSaveIndex--;
            if (selectedSaveIndex < 0)
                selectedSaveIndex = savePanels.Count - 1;
            UpdateOption();
        }
        else if (dir == InputDirection.S)
        {
            selectedSaveIndex++;
            if (selectedSaveIndex >= savePanels.Count)
                selectedSaveIndex = 0;
            UpdateOption();
        }

        if (Controls.confirmInputDown())
        {
            if(mode == SavePanelMode.Saving && currentSavePoint != null)
            {
                ReisenGameManager.instance.SaveGame(savePanels[selectedSaveIndex].fileName, currentSavePoint);
                Init();
            }
            else
            {
                if (isTitleScreenVersion)
                    TitleScreenUtils.instance.LoadGame(savePanels[selectedSaveIndex].fileName);
                else
                    ReisenGameManager.instance.LoadGame(savePanels[selectedSaveIndex].fileName);

                if (!persistOnExit)
                    this.Close();
                else
                    this.Blur();
                this.previousMenu?.Close();
            }
        }
        if (Controls.cancelInputDown())
        {
            if (!isGameplayMenu)
            {
                if (persistOnExit)
                    this.Blur();
                else
                    this.Close();
                if (prevMenuMode == ParentMenuStatusPostSelect.close)
                    previousMenu?.Open();
                else
                    previousMenu?.Focus();
            }
            else
            {
                this.Close();
                RpgGameManager.instance.ResumeGameplay();
            }
        }
    }

    internal void UpdateOption()
    {
        for (int i = 0; i < savePanels.Count; i++)
        {
            SavePanelUI panel = savePanels[i];
            if (i == selectedSaveIndex)
            {
                panel.Focus();
            }
            else
                panel.Blur();
        }
    }

    public void Show(SavePanelMode mode)
    {
        this.gameObject.SetActive(true);
        this.mode = mode;
        Open();
        Init();
    }

    public void Show(int mode)
    {
        this.gameObject.SetActive(true);
        this.mode = (SavePanelMode) mode;
        Open();
        Init();
    }

    public void ShowLoadMenu(ControllableGridMenu previousMenu)
    {
        this.mode = SavePanelMode.Loading;
        this.previousMenu = previousMenu;
        Open();
        Init();
    }

    public void Hide()
    {
        Blur();
        Close();
    }

    public override void Init()
    {
        for(int i = 0; i < SaveManager.saveNames.Count; i++)
        {
            string saveName = SaveManager.saveNames[i];
            SavePanelUI panel = savePanels[i];
            panel.fileName = saveName;
            GGJReisenSave saveData = SaveManager.FetchSaveData(saveName);
            if(saveData != null)
                panel.InitializeSavePanel(saveData);
            else
                panel.InitializeSavePanel();
        }

        UpdateOption();
    }
}

public enum SavePanelMode
{
    Saving,
    Loading
}