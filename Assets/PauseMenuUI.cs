using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuUI : MenuUI
{
    public SpeechBubble ItemDescriptionBubble;
    public CharacterDialogueAnimator CharacterProp;

    public Transform startingPositon;

    public bool ReisenVisible;

    public override void Update()
    {
        if (group.currentMenuElementIndex < 2)
            ShowReisen();
        else
            HideReisen();
        base.Update();
    }

    public override void Open()
    {
        ItemDescriptionBubble.gameObject.SetActive(false);
        base.Open();
    }

    public override void Close()
    {
        HideDescription();
        HideReisen();
        base.Close();
    }

    public override void Focus()
    {
        HideDescription();
        base.Focus();
    }

    public override void Blur()
    {
        gainFocus = false;
        //really hacky way to make sure the pause menu doesn't disappear or anything when diving into its submenus
        //We don't call base.blur for this reason..
    }

    public void ShowReisen()
    {
        if(!ReisenVisible)
        {
            ReisenVisible = true;
            StartCoroutine(MoveProp(startingPositon.position));
        }
    }

    public void HideReisen()
    {
        if (ReisenVisible)
        {
            ReisenVisible = false;
            StartCoroutine(MoveProp(startingPositon.position + Vector3.down * 5f));
        }
    }

    private IEnumerator MoveProp(Vector3 targetPosition)
    {
        Vector3 originalPos = CharacterProp.transform.position;
        float distance = (originalPos - targetPosition).magnitude;

        float timer = 0.0f;
        float totalTime = 0.4f;
        while ((CharacterProp.transform.position - targetPosition).magnitude > 0.01f)
        {
            timer += Time.deltaTime;
            CharacterProp.transform.position = Vector3.Lerp(originalPos, targetPosition, timer/ totalTime);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void ShowDescription(string description, CharacterExpression expression = CharacterExpression.normal)
    {
        CharacterProp.Turn(-1);
        CharacterProp.startTalking();
        CharacterProp.changeExpression(expression);
        ItemDescriptionBubble.gameObject.SetActive(true);
        ItemDescriptionBubble.SetDialogueBubbleContent(description);
        ItemDescriptionBubble.Show();
        Debug.Log(description);
    }

    public void HideDescription()
    {
        CharacterProp.Turn(0);
        CharacterProp.stopTalking();
        CharacterProp.changeExpression(CharacterExpression.normal);
        ItemDescriptionBubble.Hide();
        Debug.Log("hiding description");
    }
}
