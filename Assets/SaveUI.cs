using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    public SavePanelMode mode;
    public GameObject canvas;
    public List<SavePanelUI> savePanels;
    public ReisenSavePoint currentSavePoint;

    public int selectedSaveIndex;

    public static SaveUI instance;

    public void Awake()
    {
        if (SaveUI.instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);
    }

    public void Update()
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
                InitializeSavePanels();
            }
            else
                ReisenGameManager.instance.LoadGame(savePanels[selectedSaveIndex].fileName);
        }
        if (Controls.cancelInputDown())
        {
            ReisenGameManager.instance.EndSaveProcess();
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
        this.mode = mode;
        this.enabled = true;
        canvas.SetActive(true);
        InitializeSavePanels();
    }

    public void Hide()
    {
        this.enabled = false;
        canvas.SetActive(false);
    }

    public void InitializeSavePanels()
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