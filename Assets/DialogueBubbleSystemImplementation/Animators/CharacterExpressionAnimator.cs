using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterExpressionAnimator : MonoBehaviour
{
    public Animator animator;

    public GameObject expressionRendererFront;
    public SpriteRenderer expressionRendererBack;

    public List<Texture2D> expressions;
    public List<Sprite> expressionSprites;

    public CharacterExpression? targetExpression = null;

    public void changeExpression(CharacterExpression expression)
    {
        targetExpression = expression;
        animator.SetTrigger("NormalExpression");
    }

    private void animateExpression()
    {
        if (targetExpression.HasValue)
        {
            expressionRendererFront.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", expressions[(int)targetExpression.Value]);
            expressionRendererBack.sprite = expressionSprites[(int)targetExpression.Value];
        }
        else
        {
            Debug.LogWarning("animateExpression called, but no targetExpression was set.");
            expressionRendererFront.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", expressions[0]);
            expressionRendererBack.sprite = expressionSprites[0];
        }
    }
}