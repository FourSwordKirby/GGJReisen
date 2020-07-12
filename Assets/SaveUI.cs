using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUI : MenuUI
{
    public SavePanelMode mode;
    public List<SavePanelUI> savePanels;
    public ReisenSavePoint currentSavePoint;

    public int selectedSaveIndex;

    public new void Update()
    {
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
                ReisenGameManager.instance.LoadGame(savePanels[selectedSaveIndex].fileName);
                this.Close();
            }
        }
        if (Controls.cancelInputDown())
        {
            if (!isGameplayMenu)
            {
                previousMenu?.Open();
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
        this.enabled = true;
        Open();
        Init();
    }

    public void Hide()
    {
        this.enabled = false;
        Close();
    }

    public override void Init()
    {
        foreach(SavePanelUI panel in savePanels)
        {
            string saveName = panel.fileName;
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